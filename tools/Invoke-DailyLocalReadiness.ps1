[CmdletBinding()]
param(
    [string]$AcceptanceNotesPath,

    [string]$QuarantineRoot,

    [string]$CleanupScope,

    [switch]$ShowRestoreEntries,

    [switch]$RecoveryReviewOnly,

    [switch]$UndoWorkOnly,

    [switch]$RequireNoRecoveryReview,

    [switch]$RequireNoUndoWork,

    [switch]$RequireAnyRestoreManifest
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$acceptedLauncher = Join-Path $PSScriptRoot "Start-AcceptedLocalRelease.cmd"
$releaseNotesSummary = Join-Path $PSScriptRoot "Summarize-LocalReleaseAcceptanceNotes.cmd"
$restoreManifestSummary = Join-Path $PSScriptRoot "Summarize-RestoreManifests.cmd"

function Invoke-DailyReadinessStep {
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
        Write-Host "Daily local readiness failed during: $Title"
        Write-Host "Exit code: $LASTEXITCODE"
        exit $LASTEXITCODE
    }
}

Write-Host "Daily local readiness check"
Write-Host "Repository: $repoFullPath"
Write-Host "Boundary: read-only and print-only; this does not create shortcuts, install anything, launch WPF, click Scan, scan, move, restore, delete, approve cleanup, or create cleanup history."
Write-Host "Package verification: the accepted package verifier runs once before printing the normal launch command; the fixture print-only command then skips duplicate package verification in this same readiness flow."
Write-Host "Stop before real-profile movement unless the specific batch or selected Restore Manifest has fresh readiness evidence, exact confirmation, and explicit user approval."

$notesArguments = @("-RequireComplete")
if (-not [string]::IsNullOrWhiteSpace($AcceptanceNotesPath)) {
    $notesArguments += @("-Path", $AcceptanceNotesPath)
}

$acceptedNormalArguments = @("-PrintOnly")
$acceptedFixtureArguments = @("-Fixture", "-PrintOnly", "-SkipVerify")
if (-not [string]::IsNullOrWhiteSpace($AcceptanceNotesPath)) {
    $acceptedNormalArguments += @("-AcceptanceNotesPath", $AcceptanceNotesPath)
    $acceptedFixtureArguments += @("-AcceptanceNotesPath", $AcceptanceNotesPath)
}

$restoreArguments = @()
if (-not [string]::IsNullOrWhiteSpace($QuarantineRoot)) {
    $restoreArguments += @("-QuarantineRoot", $QuarantineRoot)
}
if (-not [string]::IsNullOrWhiteSpace($CleanupScope)) {
    $restoreArguments += @("-CleanupScope", $CleanupScope)
}

if ($ShowRestoreEntries.IsPresent) {
    $restoreArguments += "-ShowEntries"
}
if ($RecoveryReviewOnly.IsPresent) {
    $restoreArguments += "-RecoveryReviewOnly"
}
if ($UndoWorkOnly.IsPresent) {
    $restoreArguments += "-UndoWorkOnly"
}
if ($RequireNoRecoveryReview.IsPresent) {
    $restoreArguments += "-RequireNoRecoveryReview"
}
if ($RequireNoUndoWork.IsPresent) {
    $restoreArguments += "-RequireNoUndoWork"
}
if ($RequireAnyRestoreManifest.IsPresent) {
    $restoreArguments += "-RequireAny"
}

Invoke-DailyReadinessStep -Title "Accepted package evidence" -CommandPath $releaseNotesSummary -Arguments $notesArguments
Invoke-DailyReadinessStep -Title "Accepted normal launch command" -CommandPath $acceptedLauncher -Arguments $acceptedNormalArguments
Invoke-DailyReadinessStep -Title "Accepted fixture launch command (same verified package)" -CommandPath $acceptedLauncher -Arguments $acceptedFixtureArguments
Invoke-DailyReadinessStep -Title "Restore Manifest summary" -CommandPath $restoreManifestSummary -Arguments $restoreArguments

Write-Host ""
Write-Host "Daily local readiness check passed."
Write-Host "No WPF app was launched, no scan was started, and no files were moved, restored, deleted, or added to cleanup history."
