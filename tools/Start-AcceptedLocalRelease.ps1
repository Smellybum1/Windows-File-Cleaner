[CmdletBinding()]
param(
    [string]$AcceptanceNotesPath,

    [switch]$Fixture,

    [switch]$ChecklistOnly,

    [switch]$PrintOnly,

    [switch]$SkipVerify,

    [switch]$AllowDirtyPublish,

    [switch]$AllowSkippedPreflight
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$notesRoot = Join-Path $repoRoot ".local\release-acceptance"
$localReleaseLauncher = Join-Path $PSScriptRoot "Start-LocalRelease.ps1"

function Resolve-UnderLocalPath {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [string]$Description
    )

    if ([System.IO.Path]::IsPathRooted($Path)) {
        $resolved = [System.IO.Path]::GetFullPath($Path).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    }
    else {
        $resolved = [System.IO.Path]::GetFullPath((Join-Path $repoRoot $Path)).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    }

    if (-not ($resolved.Equals($localRoot, [System.StringComparison]::OrdinalIgnoreCase) -or
        $resolved.StartsWith($localRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase))) {
        throw "$Description must stay under the ignored .local directory: $localRoot"
    }

    return $resolved
}

function Get-LatestAcceptanceNotesPath {
    if (-not (Test-Path -LiteralPath $notesRoot -PathType Container)) {
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

    return ""
}

function Test-CheckedLine {
    param(
        [string[]]$Lines,

        [Parameter(Mandatory)]
        [string]$Label
    )

    $escapedLabel = [regex]::Escape($Label)
    foreach ($line in $Lines) {
        if ($line -match "^- \[[xX]\] $escapedLabel$") {
            return $true
        }
    }

    return $false
}

function Test-ChecklistComplete {
    param(
        [string[]]$Lines
    )

    $sectionCount = 0
    for ($index = 0; $index -lt $Lines.Count; $index++) {
        if ($Lines[$index] -notmatch "^### \d+\. Portable release check$") {
            continue
        }

        $sectionCount++
        $hasPass = $false
        for ($lineIndex = $index + 1; $lineIndex -lt $Lines.Count; $lineIndex++) {
            if ($Lines[$lineIndex].StartsWith("### ")) {
                break
            }

            if ($Lines[$lineIndex] -match "^- \[[xX]\] Pass$") {
                $hasPass = $true
                break
            }
        }

        if (-not $hasPass) {
            return $false
        }
    }

    return $sectionCount -gt 0
}

function Get-OverallResult {
    param(
        [string[]]$Lines
    )

    $overallIndex = [array]::IndexOf($Lines, "Overall result:")
    if ($overallIndex -lt 0) {
        return ""
    }

    for ($index = $overallIndex + 1; $index -lt $Lines.Count; $index++) {
        $line = $Lines[$index]
        if ($line.StartsWith("#")) {
            break
        }

        if ($line -match "^- \[[xX]\] (?<label>Pass|Pass with issues noted|Blocked)$") {
            return $Matches["label"]
        }
    }

    return ""
}

$notesPath = if ([string]::IsNullOrWhiteSpace($AcceptanceNotesPath)) {
    Get-LatestAcceptanceNotesPath
}
else {
    Resolve-UnderLocalPath -Path $AcceptanceNotesPath -Description "Acceptance notes path"
}

if (-not (Test-Path -LiteralPath $notesPath -PathType Leaf)) {
    throw "Accepted portable release notes file does not exist: $notesPath"
}

$notesLines = @(Get-Content -LiteralPath $notesPath)
$releasePath = Get-FirstMetadataValue -Lines $notesLines -Prefix "Release folder:"
if ([string]::IsNullOrWhiteSpace($releasePath)) {
    throw "Accepted portable release notes do not include a Release folder line: $notesPath"
}

$releasePath = Resolve-UnderLocalPath -Path $releasePath -Description "Accepted release path"
$completionBlockers = [System.Collections.Generic.List[string]]::new()
if (-not (Test-CheckedLine -Lines $notesLines -Label "Verifier passed for this release package.")) {
    $completionBlockers.Add("Verifier evidence is not recorded.")
}

if (-not (Test-CheckedLine -Lines $notesLines -Label "Package commit matched current HEAD or mismatch was intentionally recorded.")) {
    $completionBlockers.Add("Commit evidence is not recorded.")
}

if (-not (Test-CheckedLine -Lines $notesLines -Label "Package was launched normally or normal launch was intentionally deferred.")) {
    $completionBlockers.Add("Normal launch evidence is not recorded.")
}

if (-not (Test-CheckedLine -Lines $notesLines -Label "Fixture launch and read-only fixture Scan were completed or intentionally deferred.")) {
    $completionBlockers.Add("Fixture launch evidence is not recorded.")
}

$overallResult = Get-OverallResult -Lines $notesLines
if ($overallResult -notin @("Pass", "Pass with issues noted")) {
    $completionBlockers.Add("Overall result is not recorded as Pass or Pass with issues noted.")
}

if (-not (Test-ChecklistComplete -Lines $notesLines)) {
    $completionBlockers.Add("One or more portable release checklist items are not marked Pass.")
}

if ($completionBlockers.Count -gt 0) {
    Write-Host "Accepted portable release launcher cannot continue because the notes are incomplete:"
    foreach ($blocker in $completionBlockers) {
        Write-Host "- $blocker"
    }
    Write-Host ""
    Write-Host "Notes file: $notesPath"
    Write-Host "This check read ignored notes only; it did not launch WPF, scan, move, restore, delete, approve cleanup, or create cleanup history."
    exit 1
}

Write-Host "Accepted portable release launcher"
Write-Host "Repository: $repoFullPath"
Write-Host "Acceptance notes: $notesPath"
Write-Host "Accepted release: $releasePath"
Write-Host "Boundary: accepted package selection reads ignored notes only; it does not launch WPF until the delegated launcher is allowed to start it."
Write-Host "Note: the package verifier may warn when the accepted app package commit is behind newer docs-only commits; accepted notes remain the package acceptance source."
if ($SkipVerify.IsPresent) {
    Write-Host "Delegated package verification: skipped by request. Use only after package verification already passed in this same local readiness or acceptance flow."
}
Write-Host ""

$launcherArguments = @{
    ReleasePath = $releasePath
}
if ($Fixture.IsPresent) {
    $launcherArguments["Fixture"] = $true
}
if ($ChecklistOnly.IsPresent) {
    $launcherArguments["ChecklistOnly"] = $true
}
if ($PrintOnly.IsPresent) {
    $launcherArguments["PrintOnly"] = $true
}
if ($SkipVerify.IsPresent) {
    $launcherArguments["SkipVerify"] = $true
}
if ($AllowDirtyPublish.IsPresent) {
    $launcherArguments["AllowDirtyPublish"] = $true
}
if ($AllowSkippedPreflight.IsPresent) {
    $launcherArguments["AllowSkippedPreflight"] = $true
}

& $localReleaseLauncher @launcherArguments
exit $LASTEXITCODE
