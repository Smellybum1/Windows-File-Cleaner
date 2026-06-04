[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$testRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\fixture-acceptance-notes-test")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$summaryScript = Join-Path $PSScriptRoot "Summarize-FixtureAcceptanceNotes.ps1"
$recorderScript = Join-Path $PSScriptRoot "Record-FixtureAcceptanceNotes.ps1"

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

function New-TestFixtureAcceptanceNotes {
    param(
        [Parameter(Mandatory)]
        [string]$Path
    )

    Assert-UnderLocalPath -Path $Path

    $fixtureScope = Join-Path $repoRoot ".local\storage-scan-smoke-fixture"
    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.Add("# Fixture Review Acceptance Notes")
    $lines.Add("")
    $lines.Add("Created: 2026-06-04 00:00:00")
    $lines.Add("")
    $lines.Add("Acceptance evidence:")
    $lines.Add("")
    $lines.Add("- Repository: $repoFullPath")
    $lines.Add("- Git branch: main")
    $lines.Add("- Git commit: fixture-notes-test")
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
    $lines.Add("- ")
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
            $statusCheck = if ($number -eq 1) { "x" } else { " " }
            $lines.Add("")
            $lines.Add("### $number. Fixture check")
            $lines.Add("")
            $lines.Add("Prompt: Synthetic fixture acceptance prompt $number")
            $lines.Add("")
            $lines.Add("- [$statusCheck] Pass")
            $lines.Add("- [ ] Issue")
            $lines.Add("- [ ] Not checked")
            $lines.Add("")
            $lines.Add("Notes:")
            $lines.Add("")
            $lines.Add("- Synthetic fixture acceptance note $number.")
            $number++
        }
    }

    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllLines($Path, $lines, $utf8NoBom)
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

Assert-UnderLocalPath -Path $testRoot

$notesPath = Join-Path $testRoot "fixture-acceptance-notes-test.md"

try {
    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }

    New-Item -ItemType Directory -Path $testRoot -Force | Out-Null
    New-TestFixtureAcceptanceNotes -Path $notesPath

    $summaryResult = Invoke-Summary -Path $notesPath
    if ($summaryResult.ExitCode -ne 0) {
        throw "Incomplete fixture notes summary should exit 0 without -RequireComplete. Exit code: $($summaryResult.ExitCode)"
    }

    Assert-ContainsText -Lines $summaryResult.Output -ExpectedText "Fixture acceptance notes summary"
    Assert-ContainsText -Lines $summaryResult.Output -ExpectedText "Acceptance evidence: preflight passed: Not recorded; worktree clean/intentional: Not recorded"
    Assert-ContainsText -Lines $summaryResult.Output -ExpectedText "Checklist totals: 1 pass, 0 issue, 0 not checked, 9 not recorded"
    Assert-ContainsText -Lines $summaryResult.Output -ExpectedText "Items needing review:"
    Assert-ContainsText -Lines $summaryResult.Output -ExpectedText "After an actual all-pass visible fixture review, record these ignored notes with:"
    Assert-ContainsText -Lines $summaryResult.Output -ExpectedText ".\tools\Record-FixtureAcceptanceNotes.cmd -Path `"$notesPath`" -RecordManualAcceptance"
    Assert-ContainsText -Lines $summaryResult.Output -ExpectedText "Fill notes manually instead when there were issues or not-checked items."

    $completionResult = Invoke-Summary -Path $notesPath -RequireComplete
    if ($completionResult.ExitCode -ne 1) {
        throw "Incomplete fixture notes -RequireComplete should exit 1. Exit code: $($completionResult.ExitCode)"
    }

    Assert-ContainsText -Lines $completionResult.Output -ExpectedText "Completion check: incomplete."
    Assert-ContainsText -Lines $completionResult.Output -ExpectedText "Preflight evidence checkbox is Not recorded."
    Assert-ContainsText -Lines $completionResult.Output -ExpectedText "Worktree evidence checkbox is Not recorded."
    Assert-ContainsText -Lines $completionResult.Output -ExpectedText "Overall result is Not recorded."
    Assert-ContainsText -Lines $completionResult.Output -ExpectedText "9 checklist item(s) are not recorded."

    $missingManualResult = Invoke-Recorder -Arguments @("-Path", $notesPath)
    if ($missingManualResult.ExitCode -ne 1) {
        throw "Fixture recorder should reject calls without -RecordManualAcceptance. Exit code: $($missingManualResult.ExitCode)"
    }

    Assert-ContainsText -Lines $missingManualResult.Output -ExpectedText "Pass -RecordManualAcceptance only after the visible fixture pass, read-only fixture Scan, fixture Quarantine/undo review, Restore Manifest review, selected restore review, and real-profile/custom blocker checks were manually confirmed."

    $beforeWhatIf = Get-FileText -Path $notesPath
    $whatIfResult = Invoke-Recorder -Arguments @(
        "-Path",
        $notesPath,
        "-RecordManualAcceptance",
        "-Summary",
        "Test-only fixture acceptance should not be written in WhatIf mode.",
        "-WhatIf"
    )
    if ($whatIfResult.ExitCode -ne 0) {
        throw "Fixture recorder -WhatIf should exit 0 for otherwise valid notes. Exit code: $($whatIfResult.ExitCode)"
    }

    Assert-ContainsText -Lines $whatIfResult.Output -ExpectedText "What if: Performing the operation"
    Assert-ContainsText -Lines $whatIfResult.Output -ExpectedText "WhatIf: fixture acceptance notes were not changed."
    if ((Get-FileText -Path $notesPath) -ne $beforeWhatIf) {
        throw "Fixture recorder -WhatIf changed notes."
    }

    $recordResult = Invoke-Recorder -Arguments @(
        "-Path",
        $notesPath,
        "-RecordManualAcceptance",
        "-Summary",
        "Test-only recorded fixture acceptance for regression coverage."
    )
    if ($recordResult.ExitCode -ne 0) {
        throw "Fixture recorder should pass with explicit manual acceptance. Exit code: $($recordResult.ExitCode)"
    }

    Assert-ContainsText -Lines $recordResult.Output -ExpectedText "Recorded manual fixture acceptance notes."
    Assert-ContainsText -Lines $recordResult.Output -ExpectedText "Boundary: this command updates ignored markdown notes only; it does not launch WPF, create fixtures, scan, move, restore, delete, approve cleanup, or create cleanup history."

    $completeResult = Invoke-Summary -Path $notesPath -RequireComplete
    if ($completeResult.ExitCode -ne 0) {
        throw "Recorded fixture notes should pass completion summary. Exit code: $($completeResult.ExitCode)"
    }

    Assert-ContainsText -Lines $completeResult.Output -ExpectedText "Completion check: complete. Acceptance notes are ready to record."
    Assert-ContainsText -Lines $completeResult.Output -ExpectedText "Checklist totals: 10 pass, 0 issue, 0 not checked, 0 not recorded"
    Assert-DoesNotContainText -Lines $completeResult.Output -UnexpectedText "After an actual all-pass visible fixture review, record these ignored notes with:"

    Write-Host "Fixture acceptance notes regression passed."
    Write-Host "Boundary: temporary notes were written under ignored .local only; this did not launch WPF, create fixtures, scan, move, restore, delete, approve cleanup, or create cleanup history."
}
finally {
    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }
}
