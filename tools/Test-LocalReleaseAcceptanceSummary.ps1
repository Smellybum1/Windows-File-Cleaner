[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$notesRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\release-acceptance")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$testRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\release-acceptance-summary-test")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
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
    $compactOutput = Get-CompactOutputText -Text $joined
    $compactExpected = Get-CompactOutputText -Text $ExpectedText
    if (($joined.IndexOf($ExpectedText, [System.StringComparison]::Ordinal) -lt 0) -and
        ($compactOutput.IndexOf($compactExpected, [System.StringComparison]::Ordinal) -lt 0)) {
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
    $compactOutput = Get-CompactOutputText -Text $joined
    $compactUnexpected = Get-CompactOutputText -Text $UnexpectedText
    if (($joined.IndexOf($UnexpectedText, [System.StringComparison]::Ordinal) -ge 0) -or
        ($compactOutput.IndexOf($compactUnexpected, [System.StringComparison]::Ordinal) -ge 0)) {
        throw "Expected output not to contain: $UnexpectedText"
    }
}

function Get-CompactOutputText {
    param(
        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string]$Text
    )

    return [regex]::Replace($Text, "\s+", " ").Trim()
}

function Get-CurrentTestGitCommit {
    $commit = & git -C $repoRoot rev-parse --short HEAD
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($commit)) {
        throw "Test could not read current git commit."
    }

    return $commit.Trim()
}

function New-TestAcceptanceNotes {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [bool]$Complete,

        [Parameter(Mandatory)]
        [bool]$CommitEvidenceRecorded,

        [string]$GitCommit = "summary-test",

        [string]$ReleaseMetadataCommit = "summary-test-release"
    )

    Assert-UnderLocalPath -Path $Path

    $check = if ($Complete) { "x" } else { " " }
    $commitCheck = if ($CommitEvidenceRecorded) { "x" } else { " " }
    $normalLaunchCheck = if ($Complete) { "x" } else { " " }
    $fixtureLaunchCheck = if ($Complete) { "x" } else { " " }
    $overallPassCheck = if ($Complete) { "x" } else { " " }
    $overallBlockedCheck = " "
    $overallIssuesCheck = " "

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
    $lines.Add("- Git commit: $GitCommit")
    $lines.Add("- Worktree status at notes creation: clean")
    $lines.Add("- Release metadata commit: $ReleaseMetadataCommit")
    $lines.Add("- Release metadata worktree status at publish: clean")
    $lines.Add("- Release metadata preflight skipped: False")
    $lines.Add("- Executable: $testRoot\release\app\WindowsFileCleaner.App.exe")
    $lines.Add("- README-FIRST.txt: $testRoot\release\README-FIRST.txt")
    $lines.Add("- Normal launch script: $testRoot\release\Launch-WindowsFileCleaner.cmd")
    $lines.Add("- Normal launch command: & `"$testRoot\release\app\WindowsFileCleaner.App.exe`"")
    $lines.Add("- Fixture launch script: $testRoot\release\Launch-WindowsFileCleaner-Fixture.cmd")
    $lines.Add("- Fixture launch command: & `"$testRoot\release\app\WindowsFileCleaner.App.exe`" --scope `"$repoRoot\.local\storage-scan-smoke-fixture`"")
    $lines.Add("- Fixture Cleanup Scope: $repoRoot\.local\storage-scan-smoke-fixture")
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
    $lines.Add("- [$overallIssuesCheck] Pass with issues noted")
    $lines.Add("- [$overallBlockedCheck] Blocked")
    $lines.Add("")
    $lines.Add("Summary:")
    $lines.Add("")
    $lines.Add("- Test-only summary notes.")
    $lines.Add("")
    $lines.Add("Checklist:")
    for ($number = 1; $number -le 6; $number++) {
        $statusCheck = if ($number -eq 1 -or $Complete) { "x" } else { " " }
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

function New-TestMalformedAcceptanceNotes {
    param(
        [Parameter(Mandatory)]
        [string]$Path
    )

    Assert-UnderLocalPath -Path $Path

    $lines = @(
        "# Portable Release Acceptance Notes",
        "",
        "Created: malformed summary regression",
        "",
        "This file intentionally omits release metadata, acceptance evidence, overall result, and checklist sections."
    )

    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllLines($Path, $lines, $utf8NoBom)
}

function Invoke-Summary {
    param(
        [string]$Path,

        [switch]$RequireComplete
    )

    $arguments = @("-NoProfile", "-ExecutionPolicy", "Bypass", "-File", $summaryScript)
    if (-not [string]::IsNullOrWhiteSpace($Path)) {
        $arguments += @("-Path", $Path)
    }
    if ($RequireComplete.IsPresent) {
        $arguments += "-RequireComplete"
    }

    $previousErrorActionPreference = $ErrorActionPreference
    $ErrorActionPreference = "Continue"
    try {
        $output = @(powershell.exe @arguments 2>&1 | ForEach-Object { [string]$_ })
    }
    finally {
        $ErrorActionPreference = $previousErrorActionPreference
    }

    return [pscustomobject]@{
        ExitCode = $LASTEXITCODE
        Output = $output
    }
}

Assert-UnderLocalPath -Path $testRoot
Assert-UnderLocalPath -Path $notesRoot

$incompletePath = Join-Path $testRoot "release-acceptance-summary-test-incomplete.md"
$completePath = Join-Path $testRoot "release-acceptance-summary-test-complete.md"
$malformedPath = Join-Path $testRoot "release-acceptance-summary-test-malformed.md"
$defaultIncompletePath = Join-Path $notesRoot "release-acceptance-summary-default-test-incomplete.md"
$defaultCompletePath = Join-Path $notesRoot "release-acceptance-summary-default-test-complete.md"
$defaultMalformedPath = Join-Path $notesRoot "release-acceptance-summary-default-test-malformed.md"
$outsideLocalPath = Join-Path $repoRoot "README.md"
$currentGitCommit = Get-CurrentTestGitCommit
$testFiles = @(
    $incompletePath,
    $completePath,
    $malformedPath,
    $defaultIncompletePath,
    $defaultCompletePath,
    $defaultMalformedPath
)

try {
    foreach ($file in $testFiles) {
        if (Test-Path -LiteralPath $file -PathType Leaf) {
            Remove-Item -LiteralPath $file -Force
        }
    }

    New-Item -ItemType Directory -Path $testRoot -Force | Out-Null
    New-Item -ItemType Directory -Path $notesRoot -Force | Out-Null
    New-TestAcceptanceNotes -Path $incompletePath -Complete:$false -CommitEvidenceRecorded:$false
    New-TestAcceptanceNotes -Path $completePath -Complete:$true -CommitEvidenceRecorded:$true -GitCommit $currentGitCommit -ReleaseMetadataCommit $currentGitCommit
    New-TestMalformedAcceptanceNotes -Path $malformedPath

    $incompleteResult = Invoke-Summary -Path $incompletePath
    if ($incompleteResult.ExitCode -ne 0) {
        throw "Incomplete notes summary should exit 0 without -RequireComplete. Exit code: $($incompleteResult.ExitCode)"
    }

    Assert-ContainsText -Lines $incompleteResult.Output -ExpectedText "Pending acceptance next steps:"
    Assert-ContainsText -Lines $incompleteResult.Output -ExpectedText "Complete the human package acceptance pass before recording manual acceptance."
    Assert-ContainsText -Lines $incompleteResult.Output -ExpectedText "-RecordCommitMismatch"
    Assert-ContainsText -Lines $incompleteResult.Output -ExpectedText "These commands update/read ignored notes only; they do not launch WPF, scan, move, restore, delete, approve cleanup, or create cleanup history."

    $incompleteCompletionResult = Invoke-Summary -Path $incompletePath -RequireComplete
    if ($incompleteCompletionResult.ExitCode -ne 1) {
        throw "Incomplete notes -RequireComplete should exit 1. Exit code: $($incompleteCompletionResult.ExitCode)"
    }

    Assert-ContainsText -Lines $incompleteCompletionResult.Output -ExpectedText "Pending acceptance next steps:"
    Assert-ContainsText -Lines $incompleteCompletionResult.Output -ExpectedText "Completion check: incomplete."

    $malformedResult = Invoke-Summary -Path $malformedPath
    if ($malformedResult.ExitCode -ne 0) {
        throw "Malformed-looking notes summary should exit 0 without -RequireComplete. Exit code: $($malformedResult.ExitCode)"
    }

    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Release folder: unknown"
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Current repository HEAD: "
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Notes/current HEAD status: Notes commit unavailable"
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Package/current HEAD status: Package commit unavailable"
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Acceptance evidence: verifier: Missing; commit: Missing; normal launch: Missing; fixture launch: Missing"
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Overall result: Not recorded"
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Checklist totals: 0 pass, 0 issue, 0 not checked, 0 not recorded"
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "No portable release checklist items were found."
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Pending acceptance next steps:"
    Assert-ContainsText -Lines $malformedResult.Output -ExpectedText "Verifier evidence is Missing; run the required verifier for this notes file before recording manual acceptance."
    Assert-DoesNotContainText -Lines $malformedResult.Output -UnexpectedText "All checklist items are marked Pass."
    Assert-DoesNotContainText -Lines $malformedResult.Output -UnexpectedText "-RecordCommitMismatch"

    $malformedCompletionResult = Invoke-Summary -Path $malformedPath -RequireComplete
    if ($malformedCompletionResult.ExitCode -ne 1) {
        throw "Malformed-looking notes -RequireComplete should exit 1. Exit code: $($malformedCompletionResult.ExitCode)"
    }

    Assert-ContainsText -Lines $malformedCompletionResult.Output -ExpectedText "Completion check: incomplete."
    Assert-ContainsText -Lines $malformedCompletionResult.Output -ExpectedText "Verifier evidence checkbox is Missing."
    Assert-ContainsText -Lines $malformedCompletionResult.Output -ExpectedText "Commit evidence checkbox is Missing."
    Assert-ContainsText -Lines $malformedCompletionResult.Output -ExpectedText "Normal launch evidence checkbox is Missing."
    Assert-ContainsText -Lines $malformedCompletionResult.Output -ExpectedText "Fixture launch evidence checkbox is Missing."
    Assert-ContainsText -Lines $malformedCompletionResult.Output -ExpectedText "Overall result is Not recorded."
    Assert-ContainsText -Lines $malformedCompletionResult.Output -ExpectedText "No portable release checklist items were found."
    Assert-DoesNotContainText -Lines $malformedCompletionResult.Output -UnexpectedText "All checklist items are marked Pass."

    $outsideLocalResult = Invoke-Summary -Path $outsideLocalPath
    if ($outsideLocalResult.ExitCode -ne 1) {
        throw "Explicit notes path outside ignored .local should exit 1. Exit code: $($outsideLocalResult.ExitCode)"
    }

    Assert-ContainsText -Lines $outsideLocalResult.Output -ExpectedText "Portable release acceptance notes path must stay under ignored .local: $localRoot"
    Assert-DoesNotContainText -Lines $outsideLocalResult.Output -UnexpectedText "This is a read-only summary of local ignored notes."

    New-TestAcceptanceNotes -Path $defaultCompletePath -Complete:$true -CommitEvidenceRecorded:$true
    New-TestAcceptanceNotes -Path $defaultIncompletePath -Complete:$false -CommitEvidenceRecorded:$false
    New-TestMalformedAcceptanceNotes -Path $defaultMalformedPath
    $baseTime = Get-Date
    [System.IO.File]::SetLastWriteTime($defaultCompletePath, $baseTime.AddMinutes(1))
    [System.IO.File]::SetLastWriteTime($defaultIncompletePath, $baseTime.AddMinutes(2))
    [System.IO.File]::SetLastWriteTime($defaultMalformedPath, $baseTime.AddMinutes(3))

    $defaultCompletionResult = Invoke-Summary -RequireComplete
    if ($defaultCompletionResult.ExitCode -ne 0) {
        throw "Default -RequireComplete should select the latest complete notes while skipping newer incomplete and malformed-looking notes. Exit code: $($defaultCompletionResult.ExitCode)"
    }

    Assert-ContainsText -Lines $defaultCompletionResult.Output -ExpectedText "Notes file: $defaultCompletePath"
    Assert-ContainsText -Lines $defaultCompletionResult.Output -ExpectedText "Completion check: complete. Portable release acceptance notes are ready to record."
    Assert-ContainsText -Lines $defaultCompletionResult.Output -ExpectedText "All checklist items are marked Pass."
    Assert-DoesNotContainText -Lines $defaultCompletionResult.Output -UnexpectedText "Notes file: $defaultIncompletePath"
    Assert-DoesNotContainText -Lines $defaultCompletionResult.Output -UnexpectedText "Notes file: $defaultMalformedPath"
    Assert-DoesNotContainText -Lines $defaultCompletionResult.Output -UnexpectedText "Pending acceptance next steps:"

    $completeResult = Invoke-Summary -Path $completePath -RequireComplete
    if ($completeResult.ExitCode -ne 0) {
        throw "Complete notes -RequireComplete should exit 0. Exit code: $($completeResult.ExitCode)"
    }

    Assert-ContainsText -Lines $completeResult.Output -ExpectedText "Current repository HEAD: "
    Assert-ContainsText -Lines $completeResult.Output -ExpectedText "Notes/current HEAD status: Matches current HEAD"
    Assert-ContainsText -Lines $completeResult.Output -ExpectedText "Package/current HEAD status: Matches current HEAD"
    Assert-ContainsText -Lines $completeResult.Output -ExpectedText "Completion check: complete. Portable release acceptance notes are ready to record."
    Assert-DoesNotContainText -Lines $completeResult.Output -UnexpectedText "Pending acceptance next steps:"

    $differingCommitResult = Invoke-Summary -Path $incompletePath
    if ($differingCommitResult.ExitCode -ne 0) {
        throw "Differing notes commit summary should exit 0 without -RequireComplete. Exit code: $($differingCommitResult.ExitCode)"
    }

    Assert-ContainsText -Lines $differingCommitResult.Output -ExpectedText "Notes/current HEAD status: Differs from current HEAD"
    Assert-ContainsText -Lines $differingCommitResult.Output -ExpectedText "Package/current HEAD status: Differs from current HEAD"

    Write-Host "Local release acceptance summary regression passed."
    Write-Host "Boundary: test notes were written under ignored .local only; this did not launch WPF, scan, move, restore, delete, approve cleanup, or create cleanup history."
}
finally {
    foreach ($file in $testFiles) {
        if (Test-Path -LiteralPath $file -PathType Leaf) {
            Remove-Item -LiteralPath $file -Force
        }
    }

    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        $remaining = @(Get-ChildItem -LiteralPath $testRoot -Force)
        if ($remaining.Count -eq 0) {
            Remove-Item -LiteralPath $testRoot -Force
        }
    }
}
