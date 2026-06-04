[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$notesRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\release-acceptance")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$testRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\accepted-release-selection-test")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$acceptedLauncherScript = Join-Path $PSScriptRoot "Start-AcceptedLocalRelease.ps1"

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

function New-TestReleasePackage {
    param(
        [Parameter(Mandatory)]
        [string]$ReleaseDirectory
    )

    Assert-UnderLocalPath -Path $ReleaseDirectory

    $appDirectory = Join-Path $ReleaseDirectory "app"
    New-Item -ItemType Directory -Path $appDirectory -Force | Out-Null

    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllText(
        (Join-Path $appDirectory "WindowsFileCleaner.App.exe"),
        "Synthetic placeholder. This file must never be launched by the print-only accepted launcher regression.",
        $utf8NoBom)
    [System.IO.File]::WriteAllText(
        (Join-Path $ReleaseDirectory "README-FIRST.txt"),
        "Synthetic accepted launcher selection test package.",
        $utf8NoBom)
    [System.IO.File]::WriteAllText(
        (Join-Path $ReleaseDirectory "Launch-WindowsFileCleaner.cmd"),
        "@echo off`r`nrem Synthetic test launch script.`r`n",
        $utf8NoBom)
    [System.IO.File]::WriteAllText(
        (Join-Path $ReleaseDirectory "Launch-WindowsFileCleaner-Fixture.cmd"),
        "@echo off`r`nrem Synthetic test fixture launch script.`r`n",
        $utf8NoBom)
}

function New-TestAcceptanceNotes {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [string]$ReleaseDirectory,

        [Parameter(Mandatory)]
        [bool]$Complete
    )

    Assert-UnderLocalPath -Path $Path
    Assert-UnderLocalPath -Path $ReleaseDirectory

    $check = if ($Complete) { "x" } else { " " }
    $overallPassCheck = if ($Complete) { "x" } else { " " }

    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.Add("# Portable Release Acceptance Notes")
    $lines.Add("")
    $lines.Add("Created: 2026-06-04 00:00:00")
    $lines.Add("Release folder: $ReleaseDirectory")
    $lines.Add("")
    $lines.Add("Acceptance evidence:")
    $lines.Add("")
    $lines.Add("- Repository: $repoFullPath")
    $lines.Add("- Git branch: main")
    $lines.Add("- Git commit: accepted-launcher-selection-test")
    $lines.Add("- Worktree status at notes creation: clean")
    $lines.Add("- Release metadata commit: accepted-launcher-selection-test-release")
    $lines.Add("- Release metadata worktree status at publish: clean")
    $lines.Add("- Release metadata preflight skipped: False")
    $lines.Add("- Executable: $ReleaseDirectory\app\WindowsFileCleaner.App.exe")
    $lines.Add("- README-FIRST.txt: $ReleaseDirectory\README-FIRST.txt")
    $lines.Add("- Normal launch script: $ReleaseDirectory\Launch-WindowsFileCleaner.cmd")
    $lines.Add("- Normal launch command: & `"$ReleaseDirectory\app\WindowsFileCleaner.App.exe`"")
    $lines.Add("- Fixture launch script: $ReleaseDirectory\Launch-WindowsFileCleaner-Fixture.cmd")
    $lines.Add("- Fixture launch command: & `"$ReleaseDirectory\app\WindowsFileCleaner.App.exe`" --scope `"$repoRoot\.local\storage-scan-smoke-fixture`"")
    $lines.Add("- Fixture Cleanup Scope: $repoRoot\.local\storage-scan-smoke-fixture")
    $lines.Add("- [x] Verifier passed for this release package.")
    $lines.Add("- [x] Package commit matched current HEAD or mismatch was intentionally recorded.")
    $lines.Add("- [$check] Package was launched normally or normal launch was intentionally deferred.")
    $lines.Add("- [$check] Fixture launch and read-only fixture Scan were completed or intentionally deferred.")
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
    $lines.Add("- Test-only accepted launcher selection notes.")
    $lines.Add("")
    $lines.Add("Checklist:")
    for ($number = 1; $number -le 6; $number++) {
        $statusCheck = if ($Complete -or $number -eq 1) { "x" } else { " " }
        $lines.Add("")
        $lines.Add("### $number. Portable release check")
        $lines.Add("")
        $lines.Add("Prompt: Test prompt $number")
        $lines.Add("")
        $lines.Add("- [$statusCheck] Pass")
        $lines.Add("- [ ] Issue")
        $lines.Add("- [ ] Not checked")
        $lines.Add("")
        $lines.Add("Notes:")
        $lines.Add("")
        $lines.Add("- Test-only note $number.")
    }

    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllLines($Path, $lines, $utf8NoBom)
}

function Invoke-AcceptedLauncher {
    param(
        [string[]]$Arguments = @()
    )

    $commandArguments = @(
        "-NoProfile",
        "-ExecutionPolicy",
        "Bypass",
        "-File",
        $acceptedLauncherScript
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

$completeReleasePath = Join-Path $testRoot "complete-release"
$incompleteReleasePath = Join-Path $testRoot "incomplete-release"
$completeNotesPath = Join-Path $notesRoot "release-acceptance-selection-test-complete.md"
$incompleteNotesPath = Join-Path $notesRoot "release-acceptance-selection-test-incomplete.md"

try {
    foreach ($path in @($completeNotesPath, $incompleteNotesPath)) {
        if (Test-Path -LiteralPath $path -PathType Leaf) {
            Remove-Item -LiteralPath $path -Force
        }
    }

    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }

    New-Item -ItemType Directory -Path $notesRoot -Force | Out-Null
    New-TestReleasePackage -ReleaseDirectory $completeReleasePath
    New-TestReleasePackage -ReleaseDirectory $incompleteReleasePath
    New-TestAcceptanceNotes -Path $completeNotesPath -ReleaseDirectory $completeReleasePath -Complete:$true
    New-TestAcceptanceNotes -Path $incompleteNotesPath -ReleaseDirectory $incompleteReleasePath -Complete:$false

    $baseTime = Get-Date
    [System.IO.File]::SetLastWriteTime($completeNotesPath, $baseTime.AddMinutes(1))
    [System.IO.File]::SetLastWriteTime($incompleteNotesPath, $baseTime.AddMinutes(2))

    $explicitIncompleteResult = Invoke-AcceptedLauncher -Arguments @(
        "-AcceptanceNotesPath",
        $incompleteNotesPath,
        "-PrintOnly",
        "-SkipVerify"
    )
    if ($explicitIncompleteResult.ExitCode -ne 1) {
        throw "Explicit incomplete accepted notes should stop before launch-command printing. Exit code: $($explicitIncompleteResult.ExitCode)"
    }

    Assert-ContainsText -Lines $explicitIncompleteResult.Output -ExpectedText "Accepted portable release launcher cannot continue because the notes are incomplete:"
    Assert-ContainsText -Lines $explicitIncompleteResult.Output -ExpectedText "Normal launch evidence is not recorded."
    Assert-ContainsText -Lines $explicitIncompleteResult.Output -ExpectedText "Fixture launch evidence is not recorded."
    Assert-ContainsText -Lines $explicitIncompleteResult.Output -ExpectedText "Overall result is not recorded as Pass or Pass with issues noted."
    Assert-ContainsText -Lines $explicitIncompleteResult.Output -ExpectedText "One or more portable release checklist items are not marked Pass."
    Assert-ContainsText -Lines $explicitIncompleteResult.Output -ExpectedText "This check read ignored notes only; it did not launch WPF, scan, move, restore, delete, approve cleanup, or create cleanup history."
    Assert-DoesNotContainText -Lines $explicitIncompleteResult.Output -UnexpectedText "Launch command:"
    Assert-DoesNotContainText -Lines $explicitIncompleteResult.Output -UnexpectedText "Print-only mode: WPF was not launched."

    $defaultSelectionResult = Invoke-AcceptedLauncher -Arguments @(
        "-PrintOnly",
        "-SkipVerify"
    )
    if ($defaultSelectionResult.ExitCode -ne 0) {
        throw "Default accepted launcher selection should use the latest complete notes. Exit code: $($defaultSelectionResult.ExitCode)"
    }

    Assert-ContainsText -Lines $defaultSelectionResult.Output -ExpectedText "Accepted portable release launcher"
    Assert-ContainsText -Lines $defaultSelectionResult.Output -ExpectedText "Acceptance notes: $completeNotesPath"
    Assert-ContainsText -Lines $defaultSelectionResult.Output -ExpectedText "Accepted release: $completeReleasePath"
    Assert-ContainsText -Lines $defaultSelectionResult.Output -ExpectedText "Delegated package verification: skipped by request. Use only after package verification already passed in this same local readiness or acceptance flow."
    Assert-ContainsText -Lines $defaultSelectionResult.Output -ExpectedText "Print-only mode: WPF was not launched."
    Assert-DoesNotContainText -Lines $defaultSelectionResult.Output -UnexpectedText "Acceptance notes: $incompleteNotesPath"
    Assert-DoesNotContainText -Lines $defaultSelectionResult.Output -UnexpectedText "Accepted release: $incompleteReleasePath"

    Write-Host "Accepted local release selection regression passed."
    Write-Host "Boundary: temporary notes and synthetic package files were written under ignored .local only; this did not launch WPF, scan, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history."
}
finally {
    foreach ($path in @($completeNotesPath, $incompleteNotesPath)) {
        if (Test-Path -LiteralPath $path -PathType Leaf) {
            Remove-Item -LiteralPath $path -Force
        }
    }

    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }
}
