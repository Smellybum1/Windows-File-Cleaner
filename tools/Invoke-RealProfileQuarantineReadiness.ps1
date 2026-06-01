[CmdletBinding()]
param(
    [string]$AcceptanceNotesPath,

    [string]$QuarantineRoot,

    [switch]$SkipMvpPreflight,

    [switch]$ShowRestoreEntries,

    [switch]$RequireAnyRestoreManifest,

    [switch]$RequireNoRecoveryReview,

    [switch]$RequireNoUndoWork
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$mvpPreflight = Join-Path $PSScriptRoot "Invoke-MvpPreflight.cmd"
$dailyReadiness = Join-Path $PSScriptRoot "Invoke-DailyLocalReadiness.cmd"
$restoreManifestSummary = Join-Path $PSScriptRoot "Summarize-RestoreManifests.cmd"

function Invoke-RealProfileReadinessStep {
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
        Write-Host "Real-profile Quarantine readiness review failed during: $Title"
        Write-Host "Exit code: $LASTEXITCODE"
        exit $LASTEXITCODE
    }
}

function New-RestoreManifestArguments {
    param(
        [switch]$RecoveryReviewOnly,

        [switch]$UndoWorkOnly
    )

    $arguments = @()
    if (-not [string]::IsNullOrWhiteSpace($QuarantineRoot)) {
        $arguments += @("-QuarantineRoot", $QuarantineRoot)
    }

    if ($ShowRestoreEntries.IsPresent) {
        $arguments += "-ShowEntries"
    }
    if ($RecoveryReviewOnly.IsPresent) {
        $arguments += "-RecoveryReviewOnly"
    }
    if ($UndoWorkOnly.IsPresent) {
        $arguments += "-UndoWorkOnly"
    }
    if ($RequireAnyRestoreManifest.IsPresent) {
        $arguments += "-RequireAny"
    }
    if ($RequireNoRecoveryReview.IsPresent) {
        $arguments += "-RequireNoRecoveryReview"
    }
    if ($RequireNoUndoWork.IsPresent) {
        $arguments += "-RequireNoUndoWork"
    }

    return $arguments
}

Write-Host "Real-profile Quarantine readiness review"
Write-Host "Repository: $repoFullPath"
Write-Host "Boundary: terminal evidence only; this does not launch WPF, click Scan, scan C:\Users\moxhe, move, restore, delete, approve cleanup, or create cleanup history."
Write-Host "Purpose: gather preflight, accepted-package, and Restore Manifest evidence before a future user-clicked exact C:\Users\moxhe Quarantine batch."
Write-Host "Stop boundary: Codex must not click real-profile movement. The user must explicitly approve a specific tiny batch after reviewing WPF readiness, exact QUARANTINE, approval evidence, and immediate revalidation."

if ($SkipMvpPreflight.IsPresent) {
    Write-Host ""
    Write-Host "MVP preflight: skipped by request. Do not use skipped preflight output as fresh real-profile movement evidence."
}
else {
    Invoke-RealProfileReadinessStep -Title "Full MVP preflight" -CommandPath $mvpPreflight
}

$dailyArguments = @()
if (-not [string]::IsNullOrWhiteSpace($AcceptanceNotesPath)) {
    $dailyArguments += @("-AcceptanceNotesPath", $AcceptanceNotesPath)
}
if (-not [string]::IsNullOrWhiteSpace($QuarantineRoot)) {
    $dailyArguments += @("-QuarantineRoot", $QuarantineRoot)
}
if ($RequireAnyRestoreManifest.IsPresent) {
    $dailyArguments += "-RequireAnyRestoreManifest"
}
if ($ShowRestoreEntries.IsPresent) {
    $dailyArguments += "-ShowRestoreEntries"
}
if ($RequireNoRecoveryReview.IsPresent) {
    $dailyArguments += "-RequireNoRecoveryReview"
}
if ($RequireNoUndoWork.IsPresent) {
    $dailyArguments += "-RequireNoUndoWork"
}

Invoke-RealProfileReadinessStep -Title "Daily local readiness" -CommandPath $dailyReadiness -Arguments $dailyArguments
Invoke-RealProfileReadinessStep -Title "Recovery-review Restore Manifest focus" -CommandPath $restoreManifestSummary -Arguments (New-RestoreManifestArguments -RecoveryReviewOnly)
Invoke-RealProfileReadinessStep -Title "Undo-work Restore Manifest focus" -CommandPath $restoreManifestSummary -Arguments (New-RestoreManifestArguments -UndoWorkOnly)

Write-Host ""
Write-Host "Real-profile Quarantine readiness review passed."
Write-Host "This is not cleanup approval and does not prove that any future WPF batch is executable."
Write-Host "Before any next real-profile Quarantine click, keep the batch exact C:\Users\moxhe, at most 10 rows and 1 GB, Likely safe + Quarantine candidate only, no broad parents, no high-risk/protected/no-category/access-issue rows, with Quarantine Root safety, selected restore trust, exact QUARANTINE, Real-Profile Quarantine Approval Evidence, and immediate Pre-Execution Revalidation visible."
Write-Host "No WPF app was launched, no real-profile scan was started, and no files were moved, restored, deleted, or added to cleanup history."
