[CmdletBinding()]
param(
    [string]$AcceptanceNotesPath,

    [string]$FixtureAcceptanceNotesPath,

    [string]$QuarantineRoot,

    [switch]$SkipMvpPreflight,

    [switch]$RequireFixtureAcceptanceComplete,

    [switch]$ShowRestoreEntries
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$readinessReview = Join-Path $PSScriptRoot "Invoke-RealProfileQuarantineReadiness.cmd"
$nextBatchChecklist = Join-Path $PSScriptRoot "Show-RealProfileNextBatchChecklist.cmd"

function Invoke-NextBatchReviewStep {
    param(
        [Parameter(Mandatory)]
        [string]$Title,

        [Parameter(Mandatory)]
        [string]$CommandPath,

        [string[]]$Arguments = @()
    )

    Write-Host ""
    Write-Host "== $Title =="
    & $CommandPath @Arguments
    if ($LASTEXITCODE -ne 0) {
        Write-Host ""
        Write-Host "Real-profile next-batch review failed during: $Title"
        Write-Host "Exit code: $LASTEXITCODE"
        exit $LASTEXITCODE
    }
}

Write-Host "Real-profile next-batch review"
Write-Host "Repository: $repoFullPath"
Write-Host "Boundary: terminal evidence and guidance only; this does not launch WPF, click Scan, scan C:\Users\moxhe, move, restore, delete, approve cleanup, write Restore Manifests, or create cleanup history."
Write-Host "Purpose: run the exact-profile next-batch evidence preset, then print the manual WPF checklist for a future user-clicked tiny C:\Users\moxhe Quarantine batch."
Write-Host "Stop boundary: Codex must not click real-profile movement. The user must explicitly approve a specific tiny batch after reviewing WPF readiness, exact QUARANTINE, approval evidence, and immediate revalidation."
if ($SkipMvpPreflight.IsPresent) {
    Write-Host "MVP preflight: skipped by request. Use this only for script smoke checks, not fresh real-profile movement evidence."
}

$readinessArguments = @("-RequireNextBatchEvidence")
if (-not [string]::IsNullOrWhiteSpace($AcceptanceNotesPath)) {
    $readinessArguments += @("-AcceptanceNotesPath", $AcceptanceNotesPath)
}
if (-not [string]::IsNullOrWhiteSpace($FixtureAcceptanceNotesPath)) {
    $readinessArguments += @("-FixtureAcceptanceNotesPath", $FixtureAcceptanceNotesPath)
}
if (-not [string]::IsNullOrWhiteSpace($QuarantineRoot)) {
    $readinessArguments += @("-QuarantineRoot", $QuarantineRoot)
}
if ($SkipMvpPreflight.IsPresent) {
    $readinessArguments += "-SkipMvpPreflight"
}
if ($RequireFixtureAcceptanceComplete.IsPresent) {
    $readinessArguments += "-RequireFixtureAcceptanceComplete"
}
if ($ShowRestoreEntries.IsPresent) {
    $readinessArguments += "-ShowRestoreEntries"
}

Invoke-NextBatchReviewStep -Title "Exact-profile next-batch evidence preset" -CommandPath $readinessReview -Arguments $readinessArguments
Invoke-NextBatchReviewStep -Title "Manual WPF checklist" -CommandPath $nextBatchChecklist

Write-Host ""
Write-Host "Real-profile next-batch review passed."
Write-Host "This is not cleanup approval and does not prove that any future WPF batch is executable."
Write-Host "No WPF app was launched, no real-profile scan was started, and no files were moved, restored, deleted, approved, or added to cleanup history."
