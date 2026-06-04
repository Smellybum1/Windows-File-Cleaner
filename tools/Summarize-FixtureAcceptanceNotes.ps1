[CmdletBinding()]
param(
    [string]$Path,

    [switch]$RequireComplete
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

function Get-FirstMetadataValue {
    param(
        [string[]]$Lines,

        [Parameter(Mandatory)]
        [string]$Prefix
    )

    foreach ($line in $Lines) {
        if ($line.StartsWith($Prefix, [System.StringComparison]::OrdinalIgnoreCase)) {
            return $line.Substring($Prefix.Length).Trim()
        }
    }

    return "unknown"
}

function Get-CheckedLabel {
    param(
        [string[]]$Lines,

        [Parameter(Mandatory)]
        [int]$StartIndex,

        [Parameter(Mandatory)]
        [string[]]$Labels
    )

    for ($index = $StartIndex; $index -lt $Lines.Count; $index++) {
        $line = $Lines[$index]
        if ($line.StartsWith("#")) {
            break
        }

        foreach ($label in $Labels) {
            $escapedLabel = [regex]::Escape($label)
            if ($line -match "^- \[[xX]\] $escapedLabel$") {
                return $label
            }
        }
    }

    return "Not recorded"
}

function Get-CheckboxEvidenceState {
    param(
        [string[]]$Lines,

        [Parameter(Mandatory)]
        [string]$Label
    )

    $escapedLabel = [regex]::Escape($Label)
    foreach ($line in $Lines) {
        if ($line -match "^- \[[xX]\] $escapedLabel$") {
            return "Recorded"
        }

        if ($line -match "^- \[ \] $escapedLabel$") {
            return "Not recorded"
        }
    }

    return "Missing"
}

function Get-NotesSnippet {
    param(
        [string[]]$Lines,

        [Parameter(Mandatory)]
        [int]$StartIndex,

        [Parameter(Mandatory)]
        [int]$EndIndex
    )

    $snippets = [System.Collections.Generic.List[string]]::new()
    $inNotes = $false

    for ($index = $StartIndex; $index -le $EndIndex; $index++) {
        $line = $Lines[$index]
        if ($line -eq "Notes:") {
            $inNotes = $true
            continue
        }

        if (-not $inNotes) {
            continue
        }

        if ($line.StartsWith("### ") -or $line.StartsWith("## ")) {
            break
        }

        $trimmed = $line.Trim()
        if ($trimmed -eq "-" -or $trimmed -eq "") {
            continue
        }

        if ($trimmed.StartsWith("- ")) {
            $trimmed = $trimmed.Substring(2).Trim()
        }

        if (-not [string]::IsNullOrWhiteSpace($trimmed)) {
            $snippets.Add($trimmed)
        }
    }

    if ($snippets.Count -eq 0) {
        return ""
    }

    return ($snippets -join " ")
}

function Get-CompactSummaryText {
    param(
        [string]$Text,

        [int]$MaxLength = 180
    )

    if ([string]::IsNullOrWhiteSpace($Text)) {
        return ""
    }

    $compactText = [regex]::Replace($Text, "\s+", " ").Trim()
    if ($compactText.Length -le $MaxLength) {
        return $compactText
    }

    if ($MaxLength -le 3) {
        return $compactText.Substring(0, $MaxLength)
    }

    return ($compactText.Substring(0, $MaxLength - 3).TrimEnd() + "...")
}

function Get-FixtureChecklistEntries {
    param(
        [string[]]$Lines
    )

    $entries = [System.Collections.Generic.List[object]]::new()
    $currentSection = "Unknown"

    for ($index = 0; $index -lt $Lines.Count; $index++) {
        $line = $Lines[$index]
        if ($line.StartsWith("## ") -and -not $line.StartsWith("### ")) {
            $currentSection = $line.Substring(3).Trim()
            continue
        }

        if ($line -notmatch "^### (?<number>\d+)\. Fixture check$") {
            continue
        }

        $number = [int]$Matches["number"]
        $endIndex = $Lines.Count - 1
        for ($nextIndex = $index + 1; $nextIndex -lt $Lines.Count; $nextIndex++) {
            if ($Lines[$nextIndex].StartsWith("### ")) {
                $endIndex = $nextIndex - 1
                break
            }
        }

        $prompt = ""
        for ($promptIndex = $index + 1; $promptIndex -le $endIndex; $promptIndex++) {
            if ($Lines[$promptIndex].StartsWith("Prompt: ")) {
                $prompt = $Lines[$promptIndex].Substring("Prompt: ".Length).Trim()
                break
            }
        }

        $status = Get-CheckedLabel -Lines $Lines -StartIndex ($index + 1) -Labels @("Pass", "Issue", "Not checked")
        $notesSnippet = Get-NotesSnippet -Lines $Lines -StartIndex $index -EndIndex $endIndex

        $entries.Add([pscustomobject]@{
                Number = $number
                Section = $currentSection
                Status = $status
                Prompt = $prompt
                Notes = $notesSnippet
            })
    }

    return $entries
}

function Write-FixtureAcceptanceRecordingGuidance {
    param(
        [Parameter(Mandatory)]
        [string]$FullNotesPath
    )

    Write-Host ""
    Write-Host "After an actual all-pass visible fixture review, record these ignored notes with:"
    Write-Host ".\tools\Record-FixtureAcceptanceNotes.cmd -Path `"$FullNotesPath`" -RecordManualAcceptance"
    Write-Host "Fill notes manually instead when there were issues or not-checked items."
}

$notesPath = Resolve-FixtureAcceptanceNotesPath -RequestedPath $Path
if (-not (Test-Path -LiteralPath $notesPath)) {
    throw "Fixture acceptance notes file does not exist: $notesPath"
}

$fullNotesPath = [System.IO.Path]::GetFullPath($notesPath)
if (-not ($fullNotesPath.StartsWith($localRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase))) {
    throw "Fixture acceptance notes path must stay under ignored .local: $localRoot"
}

$lines = Get-Content -LiteralPath $fullNotesPath
$overallIndex = [array]::IndexOf($lines, "Overall result:")
$overallResult = "Not recorded"
if ($overallIndex -ge 0) {
    $overallResult = Get-CheckedLabel -Lines $lines -StartIndex $overallIndex -Labels @("Pass", "Pass with issues noted", "Blocked")
}

$entries = @(Get-FixtureChecklistEntries -Lines $lines)
$passCount = @($entries | Where-Object { $_.Status -eq "Pass" }).Count
$issueCount = @($entries | Where-Object { $_.Status -eq "Issue" }).Count
$notCheckedCount = @($entries | Where-Object { $_.Status -eq "Not checked" }).Count
$openCount = @($entries | Where-Object { $_.Status -eq "Not recorded" }).Count
$preflightEvidenceState = Get-CheckboxEvidenceState -Lines $lines -Label "Preflight passed immediately before this visible fixture pass."
$worktreeEvidenceState = Get-CheckboxEvidenceState -Lines $lines -Label "Worktree was clean or intentional changes were recorded before launch."
$needsRecordingGuidance =
    $preflightEvidenceState -ne "Recorded" -or
    $worktreeEvidenceState -ne "Recorded" -or
    $overallResult -notin @("Pass", "Pass with issues noted") -or
    $notCheckedCount -gt 0 -or
    $openCount -gt 0

Write-Host "Fixture acceptance notes summary"
Write-Host "Notes file: $fullNotesPath"
Write-Host ("Created: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "Created:"))
Write-Host ("Git branch: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "- Git branch:"))
Write-Host ("Git commit: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "- Git commit:"))
Write-Host ("Worktree at notes creation: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "- Worktree status at notes creation:"))
Write-Host ("WPF app: {0}; {1}; WPF enabled: {2}" -f
    (Get-FirstMetadataValue -Lines $lines -Prefix "- WPF app project:"),
    (Get-FirstMetadataValue -Lines $lines -Prefix "- WPF app target framework:"),
    (Get-FirstMetadataValue -Lines $lines -Prefix "- WPF enabled:"))
Write-Host ("Acceptance evidence: preflight passed: {0}; worktree clean/intentional: {1}" -f
    $preflightEvidenceState,
    $worktreeEvidenceState)
Write-Host ("Overall result: {0}" -f $overallResult)
Write-Host ("Checklist totals: {0} pass, {1} issue, {2} not checked, {3} not recorded" -f $passCount, $issueCount, $notCheckedCount, $openCount)

$attentionEntries = @($entries | Where-Object { $_.Status -in @("Issue", "Not checked", "Not recorded") })
if ($attentionEntries.Count -gt 0) {
    Write-Host ""
    Write-Host "Items needing review:"
    foreach ($entry in $attentionEntries) {
        $line = ("- {0}. {1}: {2}" -f $entry.Number, $entry.Section, $entry.Status)
        if (-not [string]::IsNullOrWhiteSpace($entry.Notes)) {
            $line = "$line - Notes: $(Get-CompactSummaryText -Text $entry.Notes)"
        }
        elseif (-not [string]::IsNullOrWhiteSpace($entry.Prompt)) {
            $line = "$line - Prompt: $(Get-CompactSummaryText -Text $entry.Prompt)"
        }

        Write-Host $line
    }
}
else {
    Write-Host ""
    Write-Host "All checklist items are marked Pass."
}

Write-Host ""
Write-Host "This is a read-only summary of local ignored notes. It does not launch WPF, scan, move, restore, delete, or create cleanup history."

if ($RequireComplete) {
    $completionBlockers = [System.Collections.Generic.List[string]]::new()
    if ($preflightEvidenceState -ne "Recorded") {
        $completionBlockers.Add("Preflight evidence checkbox is $preflightEvidenceState.")
    }

    if ($worktreeEvidenceState -ne "Recorded") {
        $completionBlockers.Add("Worktree evidence checkbox is $worktreeEvidenceState.")
    }

    if ($overallResult -notin @("Pass", "Pass with issues noted")) {
        $completionBlockers.Add("Overall result is $overallResult.")
    }

    if ($entries.Count -eq 0) {
        $completionBlockers.Add("No fixture checklist items were found.")
    }

    if ($notCheckedCount -gt 0) {
        $completionBlockers.Add("$notCheckedCount checklist item(s) are marked Not checked.")
    }

    if ($openCount -gt 0) {
        $completionBlockers.Add("$openCount checklist item(s) are not recorded.")
    }

    Write-Host ""
    if ($completionBlockers.Count -eq 0) {
        Write-Host "Completion check: complete. Fixture acceptance evidence is complete; no recorder action is pending."
    }
    else {
        Write-Host "Completion check: incomplete."
        foreach ($blocker in $completionBlockers) {
            Write-Host "- $blocker"
        }

        Write-FixtureAcceptanceRecordingGuidance -FullNotesPath $fullNotesPath
        exit 1
    }
}
elseif ($needsRecordingGuidance) {
    Write-FixtureAcceptanceRecordingGuidance -FullNotesPath $fullNotesPath
}
