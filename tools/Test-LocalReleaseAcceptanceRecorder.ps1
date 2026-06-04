[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$testRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\release-acceptance-recording-test")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$recorderScript = Join-Path $PSScriptRoot "Record-LocalReleaseAcceptanceNotes.ps1"
$summaryScript = Join-Path $PSScriptRoot "Summarize-LocalReleaseAcceptanceNotes.ps1"

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

function Get-FileText {
    param(
        [Parameter(Mandatory)]
        [string]$Path
    )

    return [System.IO.File]::ReadAllText($Path)
}

function New-TestAcceptanceNotes {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [bool]$VerifierEvidenceRecorded,

        [Parameter(Mandatory)]
        [bool]$CommitEvidenceRecorded
    )

    Assert-UnderLocalPath -Path $Path

    $verifierCheck = if ($VerifierEvidenceRecorded) { "x" } else { " " }
    $commitCheck = if ($CommitEvidenceRecorded) { "x" } else { " " }
    $verifierItemStatus = if ($VerifierEvidenceRecorded) { "x" } else { " " }

    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.Add("# Portable Release Acceptance Notes")
    $lines.Add("")
    $lines.Add("Created: 2026-06-04 00:00:00")
    $lines.Add("Release folder: $testRoot\release")
    $lines.Add("")
    $lines.Add("Acceptance evidence:")
    $lines.Add("")
    $lines.Add("- Repository: $repoFullPath")
    $lines.Add("- Git branch: main")
    $lines.Add("- Git commit: recording-test")
    $lines.Add("- Worktree status at notes creation: clean")
    $lines.Add("- Release metadata commit: recording-test-release")
    $lines.Add("- Release metadata worktree status at publish: clean")
    $lines.Add("- Release metadata preflight skipped: False")
    $lines.Add("- Executable: $testRoot\release\app\WindowsFileCleaner.App.exe")
    $lines.Add("- README-FIRST.txt: $testRoot\release\README-FIRST.txt")
    $lines.Add("- Normal launch script: $testRoot\release\Launch-WindowsFileCleaner.cmd")
    $lines.Add("- Normal launch command: & `"$testRoot\release\app\WindowsFileCleaner.App.exe`"")
    $lines.Add("- Fixture launch script: $testRoot\release\Launch-WindowsFileCleaner-Fixture.cmd")
    $lines.Add("- Fixture launch command: & `"$testRoot\release\app\WindowsFileCleaner.App.exe`" --scope `"$repoRoot\.local\storage-scan-smoke-fixture`"")
    $lines.Add("- Fixture Cleanup Scope: $repoRoot\.local\storage-scan-smoke-fixture")
    $lines.Add("- [$verifierCheck] Verifier passed for this release package.")
    $lines.Add("- [$commitCheck] Package commit matched current HEAD or mismatch was intentionally recorded.")
    $lines.Add("- [ ] Package was launched normally or normal launch was intentionally deferred.")
    $lines.Add("- [ ] Fixture launch and read-only fixture Scan were completed or intentionally deferred.")
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
    $lines.Add("- [ ] Pass")
    $lines.Add("- [ ] Pass with issues noted")
    $lines.Add("- [ ] Blocked")
    $lines.Add("")
    $lines.Add("Summary:")
    $lines.Add("")
    $lines.Add("- ")
    $lines.Add("")
    $lines.Add("Checklist:")
    for ($number = 1; $number -le 6; $number++) {
        $statusCheck = if ($number -eq 1) { $verifierItemStatus } else { " " }
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

function Invoke-Recorder {
    param(
        [string[]]$Arguments = @()
    )

    $commandArguments = @(
        "-NoProfile",
        "-ExecutionPolicy",
        "Bypass",
        "-File",
        $recorderScript
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

function Invoke-Summary {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [switch]$RequireComplete
    )

    $arguments = @("-NoProfile", "-ExecutionPolicy", "Bypass", "-File", $summaryScript, "-Path", $Path)
    if ($RequireComplete.IsPresent) {
        $arguments += "-RequireComplete"
    }

    $output = @(powershell.exe @arguments 2>&1 | ForEach-Object { [string]$_ })
    return [pscustomobject]@{
        ExitCode = $LASTEXITCODE
        Output = $output
    }
}

Assert-UnderLocalPath -Path $testRoot

$missingVerifierPath = Join-Path $testRoot "release-acceptance-missing-verifier.md"
$missingCommitPath = Join-Path $testRoot "release-acceptance-missing-commit.md"

try {
    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }

    New-Item -ItemType Directory -Path $testRoot -Force | Out-Null
    New-TestAcceptanceNotes -Path $missingVerifierPath -VerifierEvidenceRecorded:$false -CommitEvidenceRecorded:$true
    New-TestAcceptanceNotes -Path $missingCommitPath -VerifierEvidenceRecorded:$true -CommitEvidenceRecorded:$false

    $missingManualResult = Invoke-Recorder
    if ($missingManualResult.ExitCode -ne 1) {
        throw "Recorder should reject calls without -RecordManualAcceptance. Exit code: $($missingManualResult.ExitCode)"
    }

    Assert-ContainsText -Lines $missingManualResult.Output -ExpectedText "Pass -RecordManualAcceptance only after README, normal launch, fixture launch/read-only Scan, portable boundary, and real-profile stop boundary were manually confirmed."

    $missingVerifierBefore = Get-FileText -Path $missingVerifierPath
    $missingVerifierResult = Invoke-Recorder -Arguments @(
        "-Path",
        $missingVerifierPath,
        "-RecordManualAcceptance"
    )
    if ($missingVerifierResult.ExitCode -ne 1) {
        throw "Recorder should reject notes without verifier evidence. Exit code: $($missingVerifierResult.ExitCode)"
    }

    Assert-ContainsText -Lines $missingVerifierResult.Output -ExpectedText "Verifier evidence is not recorded. Run the package verifier before recording manual portable release acceptance."
    Assert-DoesNotContainText -Lines $missingVerifierResult.Output -UnexpectedText "Recorded manual portable release acceptance notes."
    if ((Get-FileText -Path $missingVerifierPath) -ne $missingVerifierBefore) {
        throw "Recorder changed notes that were missing verifier evidence."
    }

    $missingCommitBefore = Get-FileText -Path $missingCommitPath
    $missingCommitResult = Invoke-Recorder -Arguments @(
        "-Path",
        $missingCommitPath,
        "-RecordManualAcceptance"
    )
    if ($missingCommitResult.ExitCode -ne 1) {
        throw "Recorder should reject missing commit evidence without -RecordCommitMismatch. Exit code: $($missingCommitResult.ExitCode)"
    }

    Assert-ContainsText -Lines $missingCommitResult.Output -ExpectedText "Commit evidence is not recorded. Pass -RecordCommitMismatch only after reviewing and intentionally accepting the package/current-HEAD mismatch."
    Assert-DoesNotContainText -Lines $missingCommitResult.Output -UnexpectedText "Recorded manual portable release acceptance notes."
    if ((Get-FileText -Path $missingCommitPath) -ne $missingCommitBefore) {
        throw "Recorder changed notes that were missing commit evidence without explicit mismatch acceptance."
    }

    $whatIfResult = Invoke-Recorder -Arguments @(
        "-Path",
        $missingCommitPath,
        "-RecordManualAcceptance",
        "-RecordCommitMismatch",
        "-Summary",
        "Test-only what-if acceptance should not be written.",
        "-WhatIf"
    )
    if ($whatIfResult.ExitCode -ne 0) {
        throw "Recorder -WhatIf should exit 0 for otherwise valid notes. Exit code: $($whatIfResult.ExitCode)"
    }

    Assert-ContainsText -Lines $whatIfResult.Output -ExpectedText "What if: Performing the operation"
    if ((Get-FileText -Path $missingCommitPath) -ne $missingCommitBefore) {
        throw "Recorder -WhatIf changed notes."
    }

    $recordResult = Invoke-Recorder -Arguments @(
        "-Path",
        $missingCommitPath,
        "-RecordManualAcceptance",
        "-RecordCommitMismatch",
        "-Summary",
        "Test-only recorded portable release acceptance with explicit commit mismatch evidence."
    )
    if ($recordResult.ExitCode -ne 0) {
        throw "Recorder should pass with explicit commit mismatch evidence. Exit code: $($recordResult.ExitCode)"
    }

    Assert-ContainsText -Lines $recordResult.Output -ExpectedText "Recorded manual portable release acceptance notes."
    Assert-ContainsText -Lines $recordResult.Output -ExpectedText "Package/current-HEAD mismatch evidence was explicitly recorded."

    $completionResult = Invoke-Summary -Path $missingCommitPath -RequireComplete
    if ($completionResult.ExitCode -ne 0) {
        throw "Recorded notes should pass completion summary. Exit code: $($completionResult.ExitCode)"
    }

    Assert-ContainsText -Lines $completionResult.Output -ExpectedText "Completion check: complete. Portable release acceptance evidence is complete; no recorder action is pending."

    Write-Host "Local release acceptance recorder regression passed."
    Write-Host "Boundary: test notes were written under ignored .local only; this did not launch WPF, scan, move, restore, delete, approve cleanup, promote a package, or create cleanup history."
}
finally {
    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }
}
