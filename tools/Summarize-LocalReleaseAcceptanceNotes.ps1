[CmdletBinding()]
param(
    [string]$Path,

    [switch]$RequireComplete
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$notesRoot = Join-Path $repoRoot ".local\release-acceptance"

function Resolve-PortableReleaseAcceptanceNotesPath {
    param(
        [string]$RequestedPath,

        [bool]$RequireCompleteDefault
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

    $candidateNotes = @(Get-ChildItem -LiteralPath $notesRoot -Filter "release-acceptance-*.md" -File |
            Sort-Object LastWriteTime -Descending)

    if ($candidateNotes.Count -eq 0) {
        throw "No portable release acceptance notes files found in: $notesRoot"
    }

    if (-not $RequireCompleteDefault) {
        return $candidateNotes[0].FullName
    }

    foreach ($candidate in $candidateNotes) {
        $candidateLines = @(Get-Content -LiteralPath $candidate.FullName)
        if (Test-PortableReleaseAcceptanceComplete -Lines $candidateLines) {
            return $candidate.FullName
        }
    }

    throw "No complete portable release acceptance notes files found in: $notesRoot. Pass -Path to inspect an incomplete notes file."
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

        if ($line.StartsWith("### ")) {
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

function Get-PortableReleaseChecklistEntries {
    param(
        [string[]]$Lines
    )

    $entries = [System.Collections.Generic.List[object]]::new()

    for ($index = 0; $index -lt $Lines.Count; $index++) {
        $line = $Lines[$index]
        if ($line -notmatch "^### (?<number>\d+)\. Portable release check$") {
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
                Status = $status
                Prompt = $prompt
                Notes = $notesSnippet
            })
    }

    return $entries
}

function Test-PortableReleaseAcceptanceComplete {
    param(
        [string[]]$Lines
    )

    $overallIndex = [array]::IndexOf($Lines, "Overall result:")
    if ($overallIndex -lt 0) {
        return $false
    }

    $overallResult = Get-CheckedLabel -Lines $Lines -StartIndex $overallIndex -Labels @("Pass", "Pass with issues noted", "Blocked")
    if ($overallResult -notin @("Pass", "Pass with issues noted")) {
        return $false
    }

    foreach ($label in @(
            "Verifier passed for this release package.",
            "Package commit matched current HEAD or mismatch was intentionally recorded.",
            "Package was launched normally or normal launch was intentionally deferred.",
            "Fixture launch and read-only fixture Scan were completed or intentionally deferred.")) {
        if ((Get-CheckboxEvidenceState -Lines $Lines -Label $label) -ne "Recorded") {
            return $false
        }
    }

    $entries = @(Get-PortableReleaseChecklistEntries -Lines $Lines)
    if ($entries.Count -eq 0) {
        return $false
    }

    foreach ($entry in $entries) {
        if ($entry.Status -ne "Pass") {
            return $false
        }
    }

    return $true
}

$notesPath = Resolve-PortableReleaseAcceptanceNotesPath -RequestedPath $Path -RequireCompleteDefault:$RequireComplete.IsPresent
if (-not (Test-Path -LiteralPath $notesPath)) {
    throw "Portable release acceptance notes file does not exist: $notesPath"
}

$fullNotesPath = [System.IO.Path]::GetFullPath($notesPath)
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
if (-not ($fullNotesPath.StartsWith($repoFullPath + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase))) {
    throw "Portable release acceptance notes path must stay inside the repository: $repoFullPath"
}

$lines = Get-Content -LiteralPath $fullNotesPath
$overallIndex = [array]::IndexOf($lines, "Overall result:")
$overallResult = "Not recorded"
if ($overallIndex -ge 0) {
    $overallResult = Get-CheckedLabel -Lines $lines -StartIndex $overallIndex -Labels @("Pass", "Pass with issues noted", "Blocked")
}

$entries = @(Get-PortableReleaseChecklistEntries -Lines $lines)
$passCount = @($entries | Where-Object { $_.Status -eq "Pass" }).Count
$issueCount = @($entries | Where-Object { $_.Status -eq "Issue" }).Count
$notCheckedCount = @($entries | Where-Object { $_.Status -eq "Not checked" }).Count
$openCount = @($entries | Where-Object { $_.Status -eq "Not recorded" }).Count
$verifierEvidenceState = Get-CheckboxEvidenceState -Lines $lines -Label "Verifier passed for this release package."
$commitEvidenceState = Get-CheckboxEvidenceState -Lines $lines -Label "Package commit matched current HEAD or mismatch was intentionally recorded."
$normalLaunchEvidenceState = Get-CheckboxEvidenceState -Lines $lines -Label "Package was launched normally or normal launch was intentionally deferred."
$fixtureLaunchEvidenceState = Get-CheckboxEvidenceState -Lines $lines -Label "Fixture launch and read-only fixture Scan were completed or intentionally deferred."

Write-Host "Portable release acceptance notes summary"
Write-Host "Notes file: $fullNotesPath"
Write-Host ("Created: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "Created:"))
Write-Host ("Release folder: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "Release folder:"))
Write-Host ("Git branch: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "- Git branch:"))
Write-Host ("Git commit: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "- Git commit:"))
Write-Host ("Worktree at notes creation: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "- Worktree status at notes creation:"))
Write-Host ("Release metadata commit: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "- Release metadata commit:"))
Write-Host ("Release metadata preflight skipped: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "- Release metadata preflight skipped:"))
Write-Host ("Normal launch command: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "- Normal launch command:"))
Write-Host ("Fixture launch command: {0}" -f (Get-FirstMetadataValue -Lines $lines -Prefix "- Fixture launch command:"))
Write-Host ("Acceptance evidence: verifier: {0}; commit: {1}; normal launch: {2}; fixture launch: {3}" -f
    $verifierEvidenceState,
    $commitEvidenceState,
    $normalLaunchEvidenceState,
    $fixtureLaunchEvidenceState)
Write-Host ("Overall result: {0}" -f $overallResult)
Write-Host ("Checklist totals: {0} pass, {1} issue, {2} not checked, {3} not recorded" -f $passCount, $issueCount, $notCheckedCount, $openCount)

$attentionEntries = @($entries | Where-Object { $_.Status -in @("Issue", "Not checked", "Not recorded") })
if ($attentionEntries.Count -gt 0) {
    Write-Host ""
    Write-Host "Items needing review:"
    foreach ($entry in $attentionEntries) {
        $line = ("- {0}. {1}" -f $entry.Number, $entry.Status)
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
Write-Host "This is a read-only summary of local ignored notes. It does not launch WPF, scan, move, restore, delete, approve cleanup, or create cleanup history."

if ($RequireComplete) {
    $completionBlockers = [System.Collections.Generic.List[string]]::new()
    if ($verifierEvidenceState -ne "Recorded") {
        $completionBlockers.Add("Verifier evidence checkbox is $verifierEvidenceState.")
    }

    if ($commitEvidenceState -ne "Recorded") {
        $completionBlockers.Add("Commit evidence checkbox is $commitEvidenceState.")
    }

    if ($normalLaunchEvidenceState -ne "Recorded") {
        $completionBlockers.Add("Normal launch evidence checkbox is $normalLaunchEvidenceState.")
    }

    if ($fixtureLaunchEvidenceState -ne "Recorded") {
        $completionBlockers.Add("Fixture launch evidence checkbox is $fixtureLaunchEvidenceState.")
    }

    if ($overallResult -notin @("Pass", "Pass with issues noted")) {
        $completionBlockers.Add("Overall result is $overallResult.")
    }

    if ($entries.Count -eq 0) {
        $completionBlockers.Add("No portable release checklist items were found.")
    }

    if ($notCheckedCount -gt 0) {
        $completionBlockers.Add("$notCheckedCount checklist item(s) are marked Not checked.")
    }

    if ($openCount -gt 0) {
        $completionBlockers.Add("$openCount checklist item(s) are not recorded.")
    }

    Write-Host ""
    if ($completionBlockers.Count -eq 0) {
        Write-Host "Completion check: complete. Portable release acceptance notes are ready to record."
    }
    else {
        Write-Host "Completion check: incomplete."
        foreach ($blocker in $completionBlockers) {
            Write-Host "- $blocker"
        }

        exit 1
    }
}
