[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [string]$Path,

    [switch]$RecordManualAcceptance,

    [string]$Summary
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$notesRoot = Join-Path $repoRoot ".local\fixture-review-acceptance"
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)

function Resolve-FixtureAcceptanceNotesPath {
    param(
        [string]$RequestedPath
    )

    if (-not [string]::IsNullOrWhiteSpace($RequestedPath)) {
        if ([System.IO.Path]::IsPathRooted($RequestedPath)) {
            return [System.IO.Path]::GetFullPath($RequestedPath)
        }

        return [System.IO.Path]::GetFullPath((Join-Path $repoRoot $RequestedPath))
    }

    if (-not (Test-Path -LiteralPath $notesRoot)) {
        throw "No fixture acceptance notes folder exists: $notesRoot"
    }

    $latest = Get-ChildItem -LiteralPath $notesRoot -Filter "fixture-acceptance-*.md" -File |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1

    if ($null -eq $latest) {
        throw "No fixture acceptance notes files found in: $notesRoot"
    }

    return $latest.FullName
}

function Assert-IgnoredLocalPath {
    param(
        [Parameter(Mandatory)]
        [string]$FullPath
    )

    $resolved = [System.IO.Path]::GetFullPath($FullPath)
    if (-not ($resolved.StartsWith($localRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase))) {
        throw "Fixture acceptance notes path must stay under ignored .local: $localRoot"
    }
}

function Set-CheckboxLine {
    param(
        [Parameter(Mandatory)]
        [string]$Label,

        [Parameter(Mandatory)]
        [bool]$Checked
    )

    $desiredMark = if ($Checked) { "x" } else { " " }
    $escapedLabel = [regex]::Escape($Label)

    for ($index = 0; $index -lt $script:lines.Count; $index++) {
        if ($script:lines[$index] -match "^- \[[ xX]\] $escapedLabel$") {
            $script:lines[$index] = "- [$desiredMark] $Label"
            return
        }
    }

    throw "Could not find checkbox line: $Label"
}

function Set-OverallResult {
    param(
        [Parameter(Mandatory)]
        [ValidateSet("Pass", "Pass with issues noted", "Blocked")]
        [string]$Result
    )

    $overallIndex = [array]::IndexOf($script:lines, "Overall result:")
    if ($overallIndex -lt 0) {
        throw "Could not find Overall result section."
    }

    foreach ($label in @("Pass", "Pass with issues noted", "Blocked")) {
        $desiredMark = if ($label -eq $Result) { "x" } else { " " }
        $found = $false
        for ($index = $overallIndex + 1; $index -lt $script:lines.Count; $index++) {
            if ($script:lines[$index].StartsWith("Summary:")) {
                break
            }

            if ($script:lines[$index] -match "^- \[[ xX]\] $([regex]::Escape($label))$") {
                $script:lines[$index] = "- [$desiredMark] $label"
                $found = $true
                break
            }
        }

        if (-not $found) {
            throw "Could not find overall result line: $label"
        }
    }
}

function Set-FixtureChecklistStatus {
    param(
        [Parameter(Mandatory)]
        [int]$Number,

        [Parameter(Mandatory)]
        [ValidateSet("Pass", "Issue", "Not checked")]
        [string]$Status
    )

    $heading = "### $Number. Fixture check"
    $headingIndex = [array]::IndexOf($script:lines, $heading)
    if ($headingIndex -lt 0) {
        throw "Could not find checklist heading: $heading"
    }

    $endIndex = $script:lines.Count - 1
    for ($index = $headingIndex + 1; $index -lt $script:lines.Count; $index++) {
        if ($script:lines[$index].StartsWith("### ")) {
            $endIndex = $index - 1
            break
        }
    }

    foreach ($label in @("Pass", "Issue", "Not checked")) {
        $desiredMark = if ($label -eq $Status) { "x" } else { " " }
        $found = $false
        for ($index = $headingIndex + 1; $index -le $endIndex; $index++) {
            if ($script:lines[$index] -match "^- \[[ xX]\] $([regex]::Escape($label))$") {
                $script:lines[$index] = "- [$desiredMark] $label"
                $found = $true
                break
            }
        }

        if (-not $found) {
            throw "Could not find $label status line for checklist item $Number."
        }
    }
}

function Get-FixtureChecklistNumbers {
    $numbers = [System.Collections.Generic.List[int]]::new()
    foreach ($line in $script:lines) {
        if ($line -match "^### (?<number>\d+)\. Fixture check$") {
            $numbers.Add([int]$Matches["number"])
        }
    }

    return $numbers
}

function Set-SummaryLine {
    param(
        [Parameter(Mandatory)]
        [string]$SummaryText
    )

    $summaryIndex = [array]::IndexOf($script:lines, "Summary:")
    if ($summaryIndex -lt 0) {
        throw "Could not find Summary section."
    }

    for ($index = $summaryIndex + 1; $index -lt $script:lines.Count; $index++) {
        if ($script:lines[$index].StartsWith("Checklist:")) {
            break
        }

        if ($script:lines[$index].Trim().StartsWith("-")) {
            $script:lines[$index] = "- $SummaryText"
            return
        }
    }

    throw "Could not find Summary bullet."
}

if (-not $RecordManualAcceptance.IsPresent) {
    throw "Pass -RecordManualAcceptance only after the visible fixture pass, read-only fixture Scan, fixture Quarantine/undo review, Restore Manifest review, selected restore review, and real-profile/custom blocker checks were manually confirmed."
}

$notesPath = Resolve-FixtureAcceptanceNotesPath -RequestedPath $Path
if (-not (Test-Path -LiteralPath $notesPath -PathType Leaf)) {
    throw "Fixture acceptance notes file does not exist: $notesPath"
}

$fullNotesPath = [System.IO.Path]::GetFullPath($notesPath)
Assert-IgnoredLocalPath -FullPath $fullNotesPath

$script:lines = @(Get-Content -LiteralPath $fullNotesPath)

Set-CheckboxLine -Label "Preflight passed immediately before this visible fixture pass." -Checked $true
Set-CheckboxLine -Label "Worktree was clean or intentional changes were recorded before launch." -Checked $true
Set-OverallResult -Result "Pass"

$checklistNumbers = @(Get-FixtureChecklistNumbers)
if ($checklistNumbers.Count -eq 0) {
    throw "No fixture checklist items were found."
}

foreach ($number in $checklistNumbers) {
    Set-FixtureChecklistStatus -Number $number -Status "Pass"
}

$summaryText = if ([string]::IsNullOrWhiteSpace($Summary)) {
    "Manual fixture acceptance recorded with Record-FixtureAcceptanceNotes after visible fixture review, read-only fixture Scan, fixture Quarantine and undo review, Restore Manifest review, selected restore review, and real-profile/custom blocker checks."
}
else {
    $Summary.Trim()
}
Set-SummaryLine -SummaryText $summaryText

$recorded = $false
if ($PSCmdlet.ShouldProcess($fullNotesPath, "Record manual fixture acceptance")) {
    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllLines($fullNotesPath, $script:lines, $utf8NoBom)
    $recorded = $true
}

if ($recorded) {
    Write-Host "Recorded manual fixture acceptance notes."
}
else {
    Write-Host "WhatIf: fixture acceptance notes were not changed."
}
Write-Host "Notes file: $fullNotesPath"
Write-Host "Boundary: this command updates ignored markdown notes only; it does not launch WPF, create fixtures, scan, move, restore, delete, approve cleanup, or create cleanup history."
Write-Host "Review with:"
Write-Host ".\tools\Summarize-FixtureAcceptanceNotes.cmd -Path `"$fullNotesPath`""
Write-Host ".\tools\Summarize-FixtureAcceptanceNotes.cmd -Path `"$fullNotesPath`" -RequireComplete"
