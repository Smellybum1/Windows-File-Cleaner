[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$notesRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\release-acceptance")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$testRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\daily-readiness-latest-package-notes-test")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$completeNotesPath = Join-Path $notesRoot "release-acceptance-daily-latest-test-complete.md"
$incompleteNotesPath = Join-Path $notesRoot "release-acceptance-daily-latest-test-incomplete.md"
$malformedNotesPath = Join-Path $notesRoot "release-acceptance-daily-latest-test-malformed.md"
$outsideLocalNotesPath = Join-Path $repoRoot "README.md"
$fixtureNotesPath = Join-Path $testRoot "fixture-acceptance-daily-latest-test-incomplete.md"
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

function Write-Lines {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string[]]$Lines
    )

    Assert-UnderLocalPath -Path $Path
    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllLines($Path, $Lines, $utf8NoBom)
}

function New-TestPackageAcceptanceNotes {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [bool]$Complete,

        [Parameter(Mandatory)]
        [bool]$CommitEvidenceRecorded,

        [Parameter(Mandatory)]
        [string]$ReleaseFolder
    )

    Assert-UnderLocalPath -Path $Path
    Assert-UnderLocalPath -Path $ReleaseFolder

    $completeCheck = if ($Complete) { "x" } else { " " }
    $commitCheck = if ($CommitEvidenceRecorded) { "x" } else { " " }
    $normalLaunchCheck = if ($Complete) { "x" } else { " " }
    $fixtureLaunchCheck = if ($Complete) { "x" } else { " " }
    $overallPassCheck = if ($Complete) { "x" } else { " " }
    $appExePath = Join-Path (Join-Path $ReleaseFolder "app") "WindowsFileCleaner.App.exe"

    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.Add("# Portable Release Acceptance Notes")
    $lines.Add("")
    $lines.Add("Created: 2026-06-04 00:00:00")
    $lines.Add("Release folder: $ReleaseFolder")
    $lines.Add("")
    $lines.Add("Acceptance evidence:")
    $lines.Add("")
    $lines.Add("- Repository: $repoFullPath")
    $lines.Add("- Git branch: main")
    $lines.Add("- Git commit: daily-latest-package-notes-test")
    $lines.Add("- Worktree status at notes creation: clean")
    $lines.Add("- Release metadata commit: daily-latest-package-notes-test-release")
    $lines.Add("- Release metadata worktree status at publish: clean")
    $lines.Add("- Release metadata preflight skipped: False")
    $lines.Add("- Executable: $appExePath")
    $lines.Add("- README-FIRST.txt: $ReleaseFolder\README-FIRST.txt")
    $lines.Add("- Normal launch script: $ReleaseFolder\Launch-WindowsFileCleaner.cmd")
    $lines.Add("- Normal launch command: & `"$appExePath`"")
    $lines.Add("- Fixture launch script: $ReleaseFolder\Launch-WindowsFileCleaner-Fixture.cmd")
    $lines.Add("- Fixture launch command: & `"$appExePath`" --scope `"$fixtureScope`"")
    $lines.Add("- Fixture Cleanup Scope: $fixtureScope")
    $lines.Add("- [x] Verifier passed for this release package.")
    $lines.Add("- [$commitCheck] Package commit matched current HEAD or mismatch was intentionally recorded.")
    $lines.Add("- [$normalLaunchCheck] Package was launched normally or normal launch was intentionally deferred.")
    $lines.Add("- [$fixtureLaunchCheck] Fixture launch and read-only fixture Scan were completed or intentionally deferred.")
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
    $lines.Add("- [$overallPassCheck] Pass")
    $lines.Add("- [ ] Pass with issues noted")
    $lines.Add("- [ ] Blocked")
    $lines.Add("")
    $lines.Add("Summary:")
    $lines.Add("")
    $lines.Add("- Test-only daily readiness latest package notes regression.")
    $lines.Add("")
    $lines.Add("Checklist:")

    for ($number = 1; $number -le 6; $number++) {
        $statusCheck = if ($Complete -or $number -eq 1) { "x" } else { " " }
        $lines.Add("")
        $lines.Add("### $number. Portable release check")
        $lines.Add("")
        $lines.Add("Prompt: Test-only portable release prompt $number")
        $lines.Add("")
        $lines.Add("- [$statusCheck] Pass")
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
        [string]$Path
    )

    Assert-UnderLocalPath -Path $Path

    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.Add("# Fixture Review Acceptance Notes")
    $lines.Add("")
    $lines.Add("Created: 2026-06-04 00:00:00")
    $lines.Add("")
    $lines.Add("Acceptance evidence:")
    $lines.Add("")
    $lines.Add("- Repository: $repoFullPath")
    $lines.Add("- Git branch: main")
    $lines.Add("- Git commit: daily-latest-package-notes-test")
    $lines.Add("- Worktree status at notes creation: clean")
    $lines.Add("- .NET SDK: synthetic")
    $lines.Add("- WPF app project: src\WindowsFileCleaner.App\WindowsFileCleaner.App.csproj")
    $lines.Add("- WPF app target framework: net8.0-windows")
    $lines.Add("- WPF enabled: true")
    $lines.Add("- Fixture Cleanup Scope: $fixtureScope")
    $lines.Add("- [ ] Preflight passed immediately before this visible fixture pass.")
    $lines.Add("- [ ] Worktree was clean or intentional changes were recorded before launch.")
    $lines.Add("- Notes file is local/ignored under ``.local`` and is not app persistence or cleanup history.")
    $lines.Add("")
    $lines.Add("Overall result:")
    $lines.Add("")
    $lines.Add("- [ ] Pass")
    $lines.Add("- [ ] Pass with issues noted")
    $lines.Add("- [ ] Blocked")
    $lines.Add("")
    $lines.Add("Summary:")
    $lines.Add("")
    $lines.Add("- Test-only fixture blocker so daily readiness stops before accepted launch commands.")
    $lines.Add("")
    $lines.Add("Checklist:")
    $lines.Add("")
    $lines.Add("## Scan header and gate")
    $lines.Add("")
    $lines.Add("### 1. Fixture check")
    $lines.Add("")
    $lines.Add("Prompt: Test-only daily latest package notes prompt")
    $lines.Add("")
    $lines.Add("- [ ] Pass")
    $lines.Add("- [ ] Issue")
    $lines.Add("- [ ] Not checked")
    $lines.Add("")
    $lines.Add("Notes:")
    $lines.Add("")
    $lines.Add("- Test-only fixture note.")

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

Assert-UnderLocalPath -Path $notesRoot
Assert-UnderLocalPath -Path $testRoot

try {
    foreach ($path in @($completeNotesPath, $incompleteNotesPath, $malformedNotesPath)) {
        if (Test-Path -LiteralPath $path -PathType Leaf) {
            Remove-Item -LiteralPath $path -Force
        }
    }

    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }

    New-Item -ItemType Directory -Path $notesRoot -Force | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $quarantineRoot "actions") -Force | Out-Null

    $completeReleaseFolder = Join-Path $testRoot "accepted-release"
    $incompleteReleaseFolder = Join-Path $testRoot "pending-release"
    New-TestPackageAcceptanceNotes -Path $completeNotesPath -Complete:$true -CommitEvidenceRecorded:$true -ReleaseFolder $completeReleaseFolder
    New-TestPackageAcceptanceNotes -Path $incompleteNotesPath -Complete:$false -CommitEvidenceRecorded:$false -ReleaseFolder $incompleteReleaseFolder
    New-TestFixtureAcceptanceNotes -Path $fixtureNotesPath

    $outsideLocalResult = Invoke-DailyReadiness -Arguments @(
        "-AcceptanceNotesPath",
        $outsideLocalNotesPath,
        "-QuarantineRoot",
        $quarantineRoot
    )

    if ($outsideLocalResult.ExitCode -ne 1) {
        throw "Daily readiness should fail when explicit accepted package notes path is outside ignored .local. Exit code: $($outsideLocalResult.ExitCode)"
    }

    Assert-ContainsText -Lines $outsideLocalResult.Output -ExpectedText "== Accepted package evidence =="
    Assert-ContainsText -Lines $outsideLocalResult.Output -ExpectedText "Portable release acceptance notes path must stay under ignored .local: $localRoot"
    Assert-ContainsText -Lines $outsideLocalResult.Output -ExpectedText "Daily local readiness failed during: Accepted package evidence"
    Assert-DoesNotContainText -Lines $outsideLocalResult.Output -UnexpectedText "== Latest package acceptance notes (informational) =="
    Assert-DoesNotContainText -Lines $outsideLocalResult.Output -UnexpectedText "== Accepted normal launch command =="

    $baseTime = Get-Date
    [System.IO.File]::SetLastWriteTime($completeNotesPath, $baseTime.AddMinutes(1))
    [System.IO.File]::SetLastWriteTime($incompleteNotesPath, $baseTime.AddMinutes(2))

    $result = Invoke-DailyReadiness -Arguments @(
        "-AcceptanceNotesPath",
        $completeNotesPath,
        "-FixtureAcceptanceNotesPath",
        $fixtureNotesPath,
        "-RequireFixtureAcceptanceComplete",
        "-QuarantineRoot",
        $quarantineRoot
    )

    if ($result.ExitCode -ne 1) {
        throw "Daily readiness should stop at incomplete fixture notes after showing latest package notes. Exit code: $($result.ExitCode)"
    }

    Assert-ContainsText -Lines $result.Output -ExpectedText "== Accepted package evidence =="
    Assert-ContainsText -Lines $result.Output -ExpectedText "Notes file: $completeNotesPath"
    Assert-ContainsText -Lines $result.Output -ExpectedText "Completion check: complete. Portable release acceptance evidence is complete; no recorder action is pending."
    Assert-ContainsText -Lines $result.Output -ExpectedText "== Latest package acceptance notes (informational) =="
    Assert-ContainsText -Lines $result.Output -ExpectedText "Notes file: $incompleteNotesPath"
    Assert-ContainsText -Lines $result.Output -ExpectedText "Pending acceptance next steps:"
    Assert-ContainsText -Lines $result.Output -ExpectedText "Complete the human package acceptance pass before recording manual acceptance."
    Assert-ContainsText -Lines $result.Output -ExpectedText "-RecordCommitMismatch"
    Assert-ContainsText -Lines $result.Output -ExpectedText "== Fixture acceptance notes evidence =="
    Assert-ContainsText -Lines $result.Output -ExpectedText "Daily local readiness failed during: Fixture acceptance notes evidence"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Informational daily local readiness step did not complete"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "== Accepted normal launch command =="

    Write-Lines -Path $malformedNotesPath -Lines @(
        "# Portable Release Acceptance Notes",
        "",
        "Created: malformed daily latest package notes regression",
        "",
        "This file intentionally omits the acceptance evidence and checklist sections."
    )
    [System.IO.File]::SetLastWriteTime($malformedNotesPath, (Get-Date).AddMinutes(10))

    $malformedResult = Invoke-DailyReadiness -Arguments @(
        "-AcceptanceNotesPath",
        $completeNotesPath,
        "-FixtureAcceptanceNotesPath",
        $fixtureNotesPath,
        "-RequireFixtureAcceptanceComplete",
        "-QuarantineRoot",
        $quarantineRoot
    )

    if ($malformedResult.ExitCode -ne 1) {
        throw "Daily readiness should still continue past malformed-looking latest notes and stop at incomplete fixture notes. Exit code: $($malformedResult.ExitCode)"
    }

    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "== Accepted package evidence =="
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Notes file: $completeNotesPath"
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "== Latest package acceptance notes (informational) =="
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Notes file: $malformedNotesPath"
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Acceptance evidence: verifier: Missing; commit: Missing; normal launch: Missing; fixture launch: Missing"
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Checklist totals: 0 pass, 0 issue, 0 not checked, 0 not recorded"
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Verifier evidence is Missing; run the required verifier for this notes file before recording manual acceptance."
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "== Fixture acceptance notes evidence =="
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Daily local readiness failed during: Fixture acceptance notes evidence"
    Assert-DoesNotContainText -Lines $malformedResult.Output -UnexpectedText "Informational daily local readiness step did not complete"
    Assert-DoesNotContainText -Lines $malformedResult.Output -UnexpectedText "== Accepted normal launch command =="

    Write-Host "Daily readiness latest package notes regression passed."
    Write-Host "Boundary: temporary complete, incomplete, and malformed-looking package acceptance notes, fixture notes, and an empty Restore Manifest root were written under ignored .local only; this did not launch WPF, scan, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history."
}
finally {
    foreach ($path in @($completeNotesPath, $incompleteNotesPath, $malformedNotesPath)) {
        if (Test-Path -LiteralPath $path -PathType Leaf) {
            Remove-Item -LiteralPath $path -Force
        }
    }

    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }
}
