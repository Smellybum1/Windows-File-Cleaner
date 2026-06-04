[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$testRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\daily-readiness-fixture-acceptance-test")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$releaseRoot = Join-Path $testRoot "releases"
$releaseName = "windows-file-cleaner-v20260604-000001"
$releasePath = Join-Path $releaseRoot $releaseName
$acceptedNotesPath = Join-Path $testRoot "release-acceptance-daily-readiness-complete.md"
$incompleteFixtureNotesPath = Join-Path $testRoot "fixture-acceptance-daily-readiness-incomplete.md"
$completeFixtureNotesPath = Join-Path $testRoot "fixture-acceptance-daily-readiness-complete.md"
$quarantineRoot = Join-Path $testRoot "quarantine-root"
$dailyReadinessScript = Join-Path $PSScriptRoot "Invoke-DailyLocalReadiness.ps1"
$fixtureScope = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\storage-scan-smoke-fixture")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)

function Assert-UnderLocalPath {
    param(
        [Parameter(Mandatory)]
        [string]$Path
    )

    $resolved = [System.IO.Path]::GetFullPath($Path).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    if (-not ($resolved.Equals($localRoot, [System.StringComparison]::OrdinalIgnoreCase) -or
            $resolved.StartsWith($localRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase))) {
        throw "Test path must stay under ignored .local: $resolved"
    }
}

function Assert-ContainsText {
    param(
        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string[]]$Lines,

        [Parameter(Mandatory)]
        [string]$ExpectedText
    )

    $joined = ($Lines -join [Environment]::NewLine)
    $compactJoined = [regex]::Replace($joined, "\s+", " ").Trim()
    $compactExpectedText = [regex]::Replace($ExpectedText, "\s+", " ").Trim()
    if ($joined.IndexOf($ExpectedText, [System.StringComparison]::Ordinal) -lt 0 -and
        $compactJoined.IndexOf($compactExpectedText, [System.StringComparison]::Ordinal) -lt 0) {
        throw "Expected output to contain: $ExpectedText"
    }
}

function Assert-DoesNotContainText {
    param(
        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string[]]$Lines,

        [Parameter(Mandatory)]
        [string]$UnexpectedText
    )

    $joined = ($Lines -join [Environment]::NewLine)
    $compactJoined = [regex]::Replace($joined, "\s+", " ").Trim()
    $compactUnexpectedText = [regex]::Replace($UnexpectedText, "\s+", " ").Trim()
    if ($joined.IndexOf($UnexpectedText, [System.StringComparison]::Ordinal) -ge 0 -or
        $compactJoined.IndexOf($compactUnexpectedText, [System.StringComparison]::Ordinal) -ge 0) {
        throw "Expected output not to contain: $UnexpectedText"
    }
}

function Get-GitOutput {
    param(
        [Parameter(Mandatory)]
        [string[]]$Arguments
    )

    try {
        $output = & git @Arguments 2>$null
        if ($LASTEXITCODE -eq 0) {
            return (($output | Out-String).Trim())
        }
    }
    catch {
    }

    return "unknown"
}

function Write-Lines {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string[]]$Lines
    )

    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllLines($Path, $Lines, $utf8NoBom)
}

function Write-Text {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [string]$Text
    )

    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllText($Path, $Text, $utf8NoBom)
}

function New-TestLocalRelease {
    param(
        [Parameter(Mandatory)]
        [string]$ReleaseDirectory
    )

    Assert-UnderLocalPath -Path $ReleaseDirectory

    $appDirectory = Join-Path $ReleaseDirectory "app"
    $appExePath = Join-Path $appDirectory "WindowsFileCleaner.App.exe"
    $metadataPath = Join-Path $ReleaseDirectory "release-metadata.txt"
    $readmePath = Join-Path $ReleaseDirectory "README-FIRST.txt"
    $launchScriptPath = Join-Path $ReleaseDirectory "Launch-WindowsFileCleaner.cmd"
    $fixtureLaunchScriptPath = Join-Path $ReleaseDirectory "Launch-WindowsFileCleaner-Fixture.cmd"
    $zipPath = Join-Path (Split-Path -Parent $ReleaseDirectory) "$releaseName.zip"
    $zipSha256Path = "$zipPath.sha256"

    New-Item -ItemType Directory -Path $appDirectory -Force | Out-Null

    Write-Text -Path $appExePath -Text "Synthetic daily readiness fixture acceptance regression executable placeholder. This file must never be launched."

    $launchScriptLines = @(
        "@echo off",
        "setlocal",
        "",
        '"%~dp0app\WindowsFileCleaner.App.exe" %*',
        "exit /b %ERRORLEVEL%"
    )
    Write-Lines -Path $launchScriptPath -Lines $launchScriptLines

    $fixtureLaunchScriptLines = @(
        "@echo off",
        "setlocal",
        "",
        ('"%~dp0app\WindowsFileCleaner.App.exe" --scope "{0}" %*' -f $fixtureScope),
        "exit /b %ERRORLEVEL%"
    )
    Write-Lines -Path $fixtureLaunchScriptPath -Lines $fixtureLaunchScriptLines

    $readmeLines = @(
        "Windows File Cleaner portable v1",
        "",
        "Start here:",
        "1. Run Launch-WindowsFileCleaner.cmd to start the packaged app normally.",
        "2. Run Launch-WindowsFileCleaner-Fixture.cmd to start with the repo-local smoke fixture Cleanup Scope prefilled.",
        "3. You can also run app\WindowsFileCleaner.App.exe directly.",
        "",
        "Fixture launch boundary:",
        "- Fixture launch only prefills the Cleanup Scope.",
        "- It does not create the fixture, click Scan, move, restore, delete, or approve cleanup.",
        "",
        "Portable package boundary:",
        "- Portable v1 is not an installer and does not create shortcuts, services, scheduled tasks, or background automation.",
        "- Portable v1 is reversible-only: read-only Storage Scan, review, gated Quarantine, and selected restore.",
        "- Portable v1 excludes permanent deletion, persisted cleanup history, broad/all-manifest restore, custom real-profile Quarantine, and non-exact real-profile movement.",
        "- Real-profile movement remains gated by existing readiness checks, exact confirmation text, and explicit user action.",
        "- Codex and automated checks must not click real-profile movement.",
        "",
        "Release metadata:",
        "- See release-metadata.txt in this folder for commit, branch, publish command, executable path, zip path, and safety boundary evidence."
    )
    Write-Lines -Path $readmePath -Lines $readmeLines

    $commit = Get-GitOutput -Arguments @("-C", $repoFullPath, "rev-parse", "HEAD")
    $appExeSha256 = (Get-FileHash -LiteralPath $appExePath -Algorithm SHA256).Hash.ToUpperInvariant()

    $metadataLines = @(
        "Windows File Cleaner portable v1 release",
        "Created local time: 2026-06-04 00:00:00 +10:00",
        "Repository: $repoFullPath",
        "Branch: main",
        "Commit: $commit",
        "Worktree status at publish: clean",
        ".NET SDK: synthetic",
        "Project: $repoRoot\src\WindowsFileCleaner.App\WindowsFileCleaner.App.csproj",
        "Configuration: Release",
        "Runtime: win-x64",
        "Self-contained: true",
        "Preflight command: $PSScriptRoot\Invoke-MvpPreflight.cmd",
        "Preflight skipped: False",
        "Publish command: synthetic daily readiness fixture acceptance regression",
        "App directory: $appDirectory",
        "Executable: $appExePath",
        "Executable SHA256: $appExeSha256",
        "Zip path: $zipPath",
        "Zip SHA256 sidecar: $zipSha256Path",
        "Readme: $readmePath",
        "Launch script: $launchScriptPath",
        "Fixture launch script: $fixtureLaunchScriptPath",
        "Fixture Cleanup Scope: $fixtureScope",
        "",
        "Safety boundary:",
        "- Portable v1 is reversible-only: Storage Scan, review, gated Quarantine, and selected restore.",
        "- It is not an installer and does not create a desktop shortcut.",
        "- It does not enable permanent deletion, persisted cleanup history, broad/all-manifest restore, custom real-profile Quarantine, or non-exact real-profile movement.",
        "- Storage Scan remains read-only; real-profile movement still requires the existing readiness gates and explicit user action."
    )
    Write-Lines -Path $metadataPath -Lines $metadataLines

    if (Test-Path -LiteralPath $zipPath -PathType Leaf) {
        Remove-Item -LiteralPath $zipPath -Force
    }
    Compress-Archive -Path (Join-Path $ReleaseDirectory "*") -DestinationPath $zipPath -Force

    $zipSha256 = (Get-FileHash -LiteralPath $zipPath -Algorithm SHA256).Hash.ToUpperInvariant()
    $zipSha256Line = "{0}  {1}" -f $zipSha256, (Split-Path -Leaf $zipPath)
    Write-Lines -Path $zipSha256Path -Lines @($zipSha256Line)
}

function New-TestReleaseAcceptanceNotes {
    param(
        [Parameter(Mandatory)]
        [string]$Path
    )

    Assert-UnderLocalPath -Path $Path

    $appExePath = Join-Path (Join-Path $releasePath "app") "WindowsFileCleaner.App.exe"
    $readmePath = Join-Path $releasePath "README-FIRST.txt"
    $launchScriptPath = Join-Path $releasePath "Launch-WindowsFileCleaner.cmd"
    $fixtureLaunchScriptPath = Join-Path $releasePath "Launch-WindowsFileCleaner-Fixture.cmd"

    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.Add("# Portable Release Acceptance Notes")
    $lines.Add("")
    $lines.Add("Created: 2026-06-04 00:00:00")
    $lines.Add("Release folder: $releasePath")
    $lines.Add("")
    $lines.Add("Acceptance evidence:")
    $lines.Add("")
    $lines.Add("- Repository: $repoFullPath")
    $lines.Add("- Git branch: main")
    $lines.Add("- Git commit: daily-readiness-fixture-acceptance-test")
    $lines.Add("- Worktree status at notes creation: clean")
    $lines.Add("- Release metadata commit: daily-readiness-fixture-acceptance-test-release")
    $lines.Add("- Release metadata worktree status at publish: clean")
    $lines.Add("- Release metadata preflight skipped: False")
    $lines.Add("- Executable: $appExePath")
    $lines.Add("- README-FIRST.txt: $readmePath")
    $lines.Add("- Normal launch script: $launchScriptPath")
    $lines.Add("- Normal launch command: & `"$appExePath`"")
    $lines.Add("- Fixture launch script: $fixtureLaunchScriptPath")
    $lines.Add("- Fixture launch command: & `"$appExePath`" --scope `"$fixtureScope`"")
    $lines.Add("- Fixture Cleanup Scope: $fixtureScope")
    $lines.Add("- [x] Verifier passed for this release package.")
    $lines.Add("- [x] Package commit matched current HEAD or mismatch was intentionally recorded.")
    $lines.Add("- [x] Package was launched normally or normal launch was intentionally deferred.")
    $lines.Add("- [x] Fixture launch and read-only fixture Scan were completed or intentionally deferred.")
    $lines.Add("- Notes file is local/ignored under ``.local`` and is not app persistence or cleanup history.")
    $lines.Add("")
    $lines.Add("Safety boundary:")
    $lines.Add("")
    $lines.Add("- Portable v1 is reversible-only: read-only Storage Scan, review, gated Quarantine, and selected restore.")
    $lines.Add("- Portable v1 is not an installer and does not create shortcuts, services, scheduled tasks, or background automation.")
    $lines.Add("- Portable v1 excludes permanent deletion, persisted cleanup history, broad/all-manifest restore, custom real-profile Quarantine, and non-exact real-profile movement.")
    $lines.Add("")
    $lines.Add("Overall result:")
    $lines.Add("")
    $lines.Add("- [x] Pass")
    $lines.Add("- [ ] Pass with issues noted")
    $lines.Add("- [ ] Blocked")
    $lines.Add("")
    $lines.Add("Summary:")
    $lines.Add("")
    $lines.Add("- Test-only complete package acceptance notes for daily readiness fixture acceptance regression.")
    $lines.Add("")
    $lines.Add("Checklist:")

    for ($number = 1; $number -le 6; $number++) {
        $lines.Add("")
        $lines.Add("### $number. Portable release check")
        $lines.Add("")
        $lines.Add("Prompt: Test-only portable release prompt $number")
        $lines.Add("")
        $lines.Add("- [x] Pass")
        $lines.Add("- [ ] Issue")
        $lines.Add("- [ ] Not checked")
        $lines.Add("")
        $lines.Add("Notes:")
        $lines.Add("")
        $lines.Add("- Test-only package acceptance note $number.")
    }

    Write-Lines -Path $Path -Lines $lines
}

function New-TestFixtureAcceptanceNotes {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [bool]$Complete
    )

    Assert-UnderLocalPath -Path $Path

    $evidenceCheck = if ($Complete) { "x" } else { " " }
    $overallPassCheck = if ($Complete) { "x" } else { " " }

    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.Add("# Fixture Review Acceptance Notes")
    $lines.Add("")
    $lines.Add("Created: 2026-06-04 00:00:00")
    $lines.Add("")
    $lines.Add("Acceptance evidence:")
    $lines.Add("")
    $lines.Add("- Repository: $repoFullPath")
    $lines.Add("- Git branch: main")
    $lines.Add("- Git commit: daily-readiness-fixture-acceptance-test")
    $lines.Add("- Worktree status at notes creation: clean")
    $lines.Add("- .NET SDK: synthetic")
    $lines.Add("- WPF app project: src\WindowsFileCleaner.App\WindowsFileCleaner.App.csproj")
    $lines.Add("- WPF app target framework: net8.0-windows")
    $lines.Add("- WPF enabled: true")
    $lines.Add("- Fixture Cleanup Scope: $fixtureScope")
    $lines.Add("- [$evidenceCheck] Preflight passed immediately before this visible fixture pass.")
    $lines.Add("- [$evidenceCheck] Worktree was clean or intentional changes were recorded before launch.")
    $lines.Add("- Notes file is local/ignored under ``.local`` and is not app persistence or cleanup history.")
    $lines.Add("")
    $lines.Add("Overall result:")
    $lines.Add("")
    $lines.Add("- [$overallPassCheck] Pass")
    $lines.Add("- [ ] Pass with issues noted")
    $lines.Add("- [ ] Blocked")
    $lines.Add("")
    $lines.Add("Summary:")
    $lines.Add("")
    $lines.Add("- Test-only fixture acceptance notes for daily readiness regression.")
    $lines.Add("")
    $lines.Add("Checklist:")

    $sections = @(
        [pscustomobject]@{ Name = "Scan header and gate"; Count = 2 },
        [pscustomobject]@{ Name = "Safety Summary, Review, and Main Grid"; Count = 2 },
        [pscustomobject]@{ Name = "Quarantine Preview and fixture Quarantine"; Count = 2 },
        [pscustomobject]@{ Name = "Restore Manifest review and selected restore"; Count = 2 },
        [pscustomobject]@{ Name = "Real-profile and custom blockers"; Count = 2 }
    )

    $number = 1
    foreach ($section in $sections) {
        $lines.Add("")
        $lines.Add("## $($section.Name)")
        for ($index = 1; $index -le $section.Count; $index++) {
            $statusCheck = if ($Complete -or $number -eq 1) { "x" } else { " " }
            $lines.Add("")
            $lines.Add("### $number. Fixture check")
            $lines.Add("")
            $lines.Add("Prompt: Test-only daily readiness fixture acceptance prompt $number")
            $lines.Add("")
            $lines.Add("- [$statusCheck] Pass")
            $lines.Add("- [ ] Issue")
            $lines.Add("- [ ] Not checked")
            $lines.Add("")
            $lines.Add("Notes:")
            $lines.Add("")
            $lines.Add("- Test-only fixture acceptance note $number.")
            $number++
        }
    }

    Write-Lines -Path $Path -Lines $lines
}

function Invoke-DailyReadiness {
    param(
        [string[]]$Arguments = @()
    )

    $commandArguments = @(
        "-NoProfile",
        "-ExecutionPolicy",
        "Bypass",
        "-File",
        $dailyReadinessScript
    ) + $Arguments

    $previousErrorActionPreference = $ErrorActionPreference
    $ErrorActionPreference = "Continue"
    try {
        $output = @(powershell.exe @commandArguments 2>&1 | ForEach-Object { [string]$_ })
    }
    finally {
        $ErrorActionPreference = $previousErrorActionPreference
    }

    return [pscustomobject]@{
        ExitCode = $LASTEXITCODE
        Output = $output
    }
}

function Get-BaseDailyArguments {
    param(
        [Parameter(Mandatory)]
        [string]$FixtureNotesPath
    )

    return @(
        "-AcceptanceNotesPath",
        $acceptedNotesPath,
        "-FixtureAcceptanceNotesPath",
        $FixtureNotesPath,
        "-QuarantineRoot",
        $quarantineRoot
    )
}

Assert-UnderLocalPath -Path $testRoot

try {
    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }

    New-Item -ItemType Directory -Path $testRoot -Force | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $quarantineRoot "actions") -Force | Out-Null
    New-TestLocalRelease -ReleaseDirectory $releasePath
    New-TestReleaseAcceptanceNotes -Path $acceptedNotesPath
    New-TestFixtureAcceptanceNotes -Path $incompleteFixtureNotesPath -Complete:$false
    New-TestFixtureAcceptanceNotes -Path $completeFixtureNotesPath -Complete:$true

    $strictIncompleteResult = Invoke-DailyReadiness -Arguments ((Get-BaseDailyArguments -FixtureNotesPath $incompleteFixtureNotesPath) + "-RequireFixtureAcceptanceComplete")
    if ($strictIncompleteResult.ExitCode -ne 1) {
        throw "Daily readiness should fail when incomplete fixture notes are required complete. Exit code: $($strictIncompleteResult.ExitCode)"
    }

    Assert-ContainsText -Lines $strictIncompleteResult.Output -ExpectedText "== Accepted package evidence =="
    Assert-ContainsText -Lines $strictIncompleteResult.Output -ExpectedText "== Fixture acceptance notes evidence =="
    Assert-ContainsText -Lines $strictIncompleteResult.Output -ExpectedText "Completion check: incomplete."
    Assert-ContainsText -Lines $strictIncompleteResult.Output -ExpectedText "Daily local readiness failed during: Fixture acceptance notes evidence"
    Assert-DoesNotContainText -Lines $strictIncompleteResult.Output -UnexpectedText "== Accepted normal launch command =="

    $optionalIncompleteResult = Invoke-DailyReadiness -Arguments ((Get-BaseDailyArguments -FixtureNotesPath $incompleteFixtureNotesPath) + "-IncludeFixtureAcceptanceNotes")
    if ($optionalIncompleteResult.ExitCode -ne 0) {
        throw "Daily readiness should pass when incomplete fixture notes are optional. Exit code: $($optionalIncompleteResult.ExitCode)"
    }

    Assert-ContainsText -Lines $optionalIncompleteResult.Output -ExpectedText "== Fixture acceptance notes evidence =="
    Assert-ContainsText -Lines $optionalIncompleteResult.Output -ExpectedText "After an actual all-pass visible fixture review, record these ignored notes with:"
    Assert-ContainsText -Lines $optionalIncompleteResult.Output -ExpectedText "== Accepted normal launch command =="
    Assert-ContainsText -Lines $optionalIncompleteResult.Output -ExpectedText "== Accepted fixture launch command (same verified package) =="
    Assert-ContainsText -Lines $optionalIncompleteResult.Output -ExpectedText "Print-only mode: WPF was not launched."
    Assert-ContainsText -Lines $optionalIncompleteResult.Output -ExpectedText "Daily local readiness check passed."

    $strictCompleteResult = Invoke-DailyReadiness -Arguments ((Get-BaseDailyArguments -FixtureNotesPath $completeFixtureNotesPath) + "-RequireFixtureAcceptanceComplete")
    if ($strictCompleteResult.ExitCode -ne 0) {
        throw "Daily readiness should pass when complete fixture notes are required complete. Exit code: $($strictCompleteResult.ExitCode)"
    }

    Assert-ContainsText -Lines $strictCompleteResult.Output -ExpectedText "Completion check: complete. Fixture acceptance evidence is complete; no recorder action is pending."
    Assert-ContainsText -Lines $strictCompleteResult.Output -ExpectedText "Checklist totals: 10 pass, 0 issue, 0 not checked, 0 not recorded"
    Assert-ContainsText -Lines $strictCompleteResult.Output -ExpectedText "== Restore Manifest summary =="
    Assert-ContainsText -Lines $strictCompleteResult.Output -ExpectedText "== Exact-profile undo-work stop state =="
    Assert-ContainsText -Lines $strictCompleteResult.Output -ExpectedText "Daily local readiness check passed."
    Assert-DoesNotContainText -Lines $strictCompleteResult.Output -UnexpectedText "Synthetic Restore Manifest-only mode:"

    Write-Host "Daily readiness fixture acceptance notes regression passed."
    Write-Host "Boundary: temporary package files, acceptance notes, fixture notes, and empty Restore Manifest roots were written under ignored .local only; this did not launch WPF, scan, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history."
}
finally {
    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }
}
