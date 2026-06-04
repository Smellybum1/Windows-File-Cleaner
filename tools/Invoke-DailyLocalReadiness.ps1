[CmdletBinding()]
param(
    [string]$AcceptanceNotesPath,

    [string]$FixtureAcceptanceNotesPath,

    [string]$QuarantineRoot,

    [string]$CleanupScope,

    [switch]$IncludeFixtureAcceptanceNotes,

    [switch]$RequireFixtureAcceptanceComplete,

    [switch]$ShowRestoreEntries,

    [switch]$RecoveryReviewOnly,

    [switch]$UndoWorkOnly,

    [switch]$RequireNoRecoveryReview,

    [switch]$RequireNoUndoWork,

    [switch]$RequireAnyDisplayedRestoreManifest,

    [switch]$RequireNoDisplayedRecoveryReview,

    [switch]$RequireNoDisplayedUndoWork,

    [switch]$RequireAnyRestoreManifest
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$acceptedLauncher = Join-Path $PSScriptRoot "Start-AcceptedLocalRelease.cmd"
$releaseNotesSummary = Join-Path $PSScriptRoot "Summarize-LocalReleaseAcceptanceNotes.cmd"
$fixtureNotesSummary = Join-Path $PSScriptRoot "Summarize-FixtureAcceptanceNotes.cmd"
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

function Invoke-DailyReadinessInformationalStep {
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
        Write-Host "Informational daily local readiness step did not complete: $Title"
        Write-Host "Exit code: $LASTEXITCODE"
        Write-Host "Accepted package readiness continues to use completed accepted notes only."
    }
}

Write-Host "Daily local readiness check"
Write-Host "Repository: $repoFullPath"
Write-Host "Boundary: read-only and print-only; this does not create shortcuts, install anything, launch WPF, click Scan, scan, move, restore, delete, approve cleanup, or create cleanup history."
Write-Host "Package verification: the accepted package verifier runs once before printing the normal launch command; the fixture print-only command then skips duplicate package verification in this same readiness flow."
Write-Host "Latest package acceptance notes: informational only; incomplete candidate notes do not replace the completed accepted package baseline."
Write-Host "Fixture acceptance notes: optional local ignored-note summary only; strict completion is checked only when requested."
Write-Host "Stop before real-profile movement unless the specific batch or selected Restore Manifest has fresh readiness evidence, exact confirmation, and explicit user approval."

$notesArguments = @("-RequireComplete")
if (-not [string]::IsNullOrWhiteSpace($AcceptanceNotesPath)) {
    $notesArguments += @("-Path", $AcceptanceNotesPath)
}

$fixtureNotesArguments = @()
if (-not [string]::IsNullOrWhiteSpace($FixtureAcceptanceNotesPath)) {
    $fixtureNotesArguments += @("-Path", $FixtureAcceptanceNotesPath)
}
if ($RequireFixtureAcceptanceComplete.IsPresent) {
    $fixtureNotesArguments += "-RequireComplete"
}
$shouldSummarizeFixtureAcceptance = $IncludeFixtureAcceptanceNotes.IsPresent -or $RequireFixtureAcceptanceComplete.IsPresent -or (-not [string]::IsNullOrWhiteSpace($FixtureAcceptanceNotesPath))

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
if ($RequireAnyDisplayedRestoreManifest.IsPresent) {
    $restoreArguments += "-RequireAnyDisplayed"
}
if ($RequireNoDisplayedRecoveryReview.IsPresent) {
    $restoreArguments += "-RequireNoDisplayedRecoveryReview"
}
if ($RequireNoDisplayedUndoWork.IsPresent) {
    $restoreArguments += "-RequireNoDisplayedUndoWork"
}
if ($RequireAnyRestoreManifest.IsPresent) {
    $restoreArguments += "-RequireAny"
}

Invoke-DailyReadinessStep -Title "Accepted package evidence" -CommandPath $releaseNotesSummary -Arguments $notesArguments
Invoke-DailyReadinessInformationalStep -Title "Latest package acceptance notes (informational)" -CommandPath $releaseNotesSummary
if ($shouldSummarizeFixtureAcceptance) {
    Invoke-DailyReadinessStep -Title "Fixture acceptance notes evidence" -CommandPath $fixtureNotesSummary -Arguments $fixtureNotesArguments
}
Invoke-DailyReadinessStep -Title "Accepted normal launch command" -CommandPath $acceptedLauncher -Arguments $acceptedNormalArguments
Invoke-DailyReadinessStep -Title "Accepted fixture launch command (same verified package)" -CommandPath $acceptedLauncher -Arguments $acceptedFixtureArguments
Invoke-DailyReadinessStep -Title "Restore Manifest summary" -CommandPath $restoreManifestSummary -Arguments $restoreArguments

Write-Host ""
Write-Host "Daily local readiness check passed."
Write-Host "No WPF app was launched, no scan was started, and no files were moved, restored, deleted, or added to cleanup history."
