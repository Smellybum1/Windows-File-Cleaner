[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [string]$Path,

    [switch]$RecordManualAcceptance,

    [string]$Summary
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$notesRoot = Join-Path $repoRoot ".local\release-acceptance"
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)

function Resolve-PortableReleaseAcceptanceNotesPath {
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
        throw "No portable release acceptance notes folder exists: $notesRoot"
    }

    $latest = Get-ChildItem -LiteralPath $notesRoot -Filter "release-acceptance-*.md" -File |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1

    if ($null -eq $latest) {
        throw "No portable release acceptance notes files found in: $notesRoot"
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
        throw "Portable release acceptance notes path must stay under ignored .local: $localRoot"
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

function Set-ChecklistStatus {
    param(
        [Parameter(Mandatory)]
        [int]$Number,

        [Parameter(Mandatory)]
        [ValidateSet("Pass", "Issue", "Not checked")]
        [string]$Status
    )

    $heading = "### $Number. Portable release check"
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
    throw "Pass -RecordManualAcceptance only after README, normal launch, fixture launch/read-only Scan, portable boundary, and real-profile stop boundary were manually confirmed."
}

$notesPath = Resolve-PortableReleaseAcceptanceNotesPath -RequestedPath $Path
if (-not (Test-Path -LiteralPath $notesPath -PathType Leaf)) {
    throw "Portable release acceptance notes file does not exist: $notesPath"
}

$fullNotesPath = [System.IO.Path]::GetFullPath($notesPath)
Assert-IgnoredLocalPath -FullPath $fullNotesPath

$script:lines = @(Get-Content -LiteralPath $fullNotesPath)

Set-CheckboxLine -Label "Package was launched normally or normal launch was intentionally deferred." -Checked $true
Set-CheckboxLine -Label "Fixture launch and read-only fixture Scan were completed or intentionally deferred." -Checked $true
Set-OverallResult -Result "Pass"
foreach ($number in 2..6) {
    Set-ChecklistStatus -Number $number -Status "Pass"
}

$summaryText = if ([string]::IsNullOrWhiteSpace($Summary)) {
    "Manual portable release acceptance recorded with Record-LocalReleaseAcceptanceNotes after README review, normal launch, fixture launch/read-only Scan, portable boundary review, and real-profile stop boundary confirmation."
}
else {
    $Summary.Trim()
}
Set-SummaryLine -SummaryText $summaryText

if ($PSCmdlet.ShouldProcess($fullNotesPath, "Record manual portable release acceptance")) {
    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllLines($fullNotesPath, $script:lines, $utf8NoBom)
}

Write-Host "Recorded manual portable release acceptance notes."
Write-Host "Notes file: $fullNotesPath"
Write-Host "Boundary: this updated ignored markdown notes only; it did not launch WPF, scan, move, restore, delete, approve cleanup, or create cleanup history."
Write-Host "Review with:"
Write-Host ".\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path `"$fullNotesPath`""
Write-Host ".\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path `"$fullNotesPath`" -RequireComplete"
