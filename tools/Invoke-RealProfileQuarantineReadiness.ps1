[CmdletBinding()]
param(
    [string]$AcceptanceNotesPath,

    [string]$FixtureAcceptanceNotesPath,

    [string]$QuarantineRoot,

    [string]$CleanupScope,

    [switch]$AllCleanupScopes,

    [switch]$SkipMvpPreflight,

    [switch]$RequireNextBatchEvidence,

    [switch]$IncludeFixtureAcceptanceNotes,

    [switch]$RequireFixtureAcceptanceComplete,

    [switch]$ShowRestoreEntries,

    [switch]$RequireAnyRestoreManifest,

    [switch]$RequireNoRecoveryReview,

    [switch]$RequireNoUndoWork,

    [switch]$RequireAnyDisplayedRestoreManifest,

    [switch]$RequireNoDisplayedRecoveryReview,

    [switch]$RequireNoDisplayedUndoWork,

    [switch]$SyntheticRestoreManifestOnly
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$mvpPreflight = Join-Path $PSScriptRoot "Invoke-MvpPreflight.cmd"
$dailyReadiness = Join-Path $PSScriptRoot "Invoke-DailyLocalReadiness.cmd"
$restoreManifestSummary = Join-Path $PSScriptRoot "Summarize-RestoreManifests.cmd"
$defaultRealProfileCleanupScope = "C:\Users\moxhe"
$defaultRealProfileCleanupScopeFullPath = [System.IO.Path]::GetFullPath($defaultRealProfileCleanupScope).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$cleanupScopeWasProvided = $PSBoundParameters.ContainsKey("CleanupScope")

if ($AllCleanupScopes.IsPresent -and $cleanupScopeWasProvided) {
    Write-Host "Choose either -CleanupScope or -AllCleanupScopes, not both."
    exit 1
}

if ($RequireNextBatchEvidence.IsPresent -and $AllCleanupScopes.IsPresent) {
    Write-Host "-RequireNextBatchEvidence is only for the exact real-profile Cleanup Scope $defaultRealProfileCleanupScope. Do not combine it with -AllCleanupScopes."
    exit 1
}

function Test-UnderLocalPath {
    param(
        [Parameter(Mandatory)]
        [string]$Path
    )

    $resolved = [System.IO.Path]::GetFullPath($Path).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    return $resolved.Equals($localRoot, [System.StringComparison]::OrdinalIgnoreCase) -or
        $resolved.StartsWith($localRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)
}

if ($SyntheticRestoreManifestOnly.IsPresent) {
    if (-not $SkipMvpPreflight.IsPresent) {
        Write-Host "-SyntheticRestoreManifestOnly is only for focused synthetic regression loops and requires -SkipMvpPreflight."
        exit 1
    }

    if ([string]::IsNullOrWhiteSpace($QuarantineRoot)) {
        Write-Host "-SyntheticRestoreManifestOnly requires an explicit -QuarantineRoot under ignored .local."
        exit 1
    }

    if (-not (Test-UnderLocalPath -Path $QuarantineRoot)) {
        Write-Host "-SyntheticRestoreManifestOnly requires -QuarantineRoot to stay under ignored .local: $localRoot"
        exit 1
    }

    if (-not [string]::IsNullOrWhiteSpace($AcceptanceNotesPath) -or
        -not [string]::IsNullOrWhiteSpace($FixtureAcceptanceNotesPath) -or
        $IncludeFixtureAcceptanceNotes.IsPresent -or
        $RequireFixtureAcceptanceComplete.IsPresent) {
        Write-Host "-SyntheticRestoreManifestOnly cannot be combined with package or fixture acceptance notes parameters."
        exit 1
    }
}

$effectiveCleanupScope = $null
if (-not $AllCleanupScopes.IsPresent) {
    if ($cleanupScopeWasProvided) {
        if ([string]::IsNullOrWhiteSpace($CleanupScope)) {
            Write-Host "CleanupScope must not be blank. Use -AllCleanupScopes to disable the default exact real-profile display focus."
            exit 1
        }

        $effectiveCleanupScope = $CleanupScope
    }
    else {
        $effectiveCleanupScope = $defaultRealProfileCleanupScope
    }

    if (-not [System.IO.Path]::IsPathRooted($effectiveCleanupScope)) {
        Write-Host "CleanupScope must be a fully qualified path: $effectiveCleanupScope"
        exit 1
    }

    $effectiveCleanupScope = [System.IO.Path]::GetFullPath($effectiveCleanupScope).TrimEnd([System.IO.Path]::DirectorySeparatorChar)

    if ($RequireNextBatchEvidence.IsPresent -and -not ([StringComparer]::OrdinalIgnoreCase.Equals($effectiveCleanupScope, $defaultRealProfileCleanupScopeFullPath))) {
        Write-Host "-RequireNextBatchEvidence is only for the exact real-profile Cleanup Scope $defaultRealProfileCleanupScope. Remove the preset or use -CleanupScope `"$defaultRealProfileCleanupScope`"."
        exit 1
    }
}

$shouldIncludeFixtureAcceptanceNotes = (-not $SyntheticRestoreManifestOnly.IsPresent) -and ($IncludeFixtureAcceptanceNotes.IsPresent -or $RequireNextBatchEvidence.IsPresent)
$shouldRequireAnyDisplayedRestoreManifest = $RequireAnyDisplayedRestoreManifest.IsPresent -or $RequireNextBatchEvidence.IsPresent
$shouldRequireNoDisplayedUndoWork = $RequireNoDisplayedUndoWork.IsPresent -or $RequireNextBatchEvidence.IsPresent

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
    if ($null -ne $effectiveCleanupScope) {
        $arguments += @("-CleanupScope", $effectiveCleanupScope)
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
    if ($RequireNoDisplayedRecoveryReview.IsPresent) {
        $arguments += "-RequireNoDisplayedRecoveryReview"
    }
    if ($shouldRequireNoDisplayedUndoWork) {
        $arguments += "-RequireNoDisplayedUndoWork"
    }

    return $arguments
}

Write-Host "Real-profile Quarantine readiness review"
Write-Host "Repository: $repoFullPath"
Write-Host "Boundary: terminal evidence only; this does not launch WPF, click Scan, scan C:\Users\moxhe, move, restore, delete, approve cleanup, or create cleanup history."
Write-Host "Purpose: gather preflight, accepted-package, and Restore Manifest evidence before a future user-clicked exact C:\Users\moxhe Quarantine batch."
Write-Host "Stop boundary: Codex must not click real-profile movement. The user must explicitly approve a specific tiny batch after reviewing WPF readiness, exact QUARANTINE, approval evidence, and immediate revalidation."
Write-Host "Fixture acceptance notes: optional daily-readiness evidence only; require completion only when formal fixture notes should be a strict gate."
if ($RequireNextBatchEvidence.IsPresent) {
    if ($SyntheticRestoreManifestOnly.IsPresent) {
        Write-Host "Next-batch evidence preset: exact C:\Users\moxhe display focus, required displayed Restore Manifest evidence, and zero displayed undo-work manifests."
    }
    else {
        Write-Host "Next-batch evidence preset: exact C:\Users\moxhe display focus, Fixture Acceptance Notes status, required displayed Restore Manifest evidence, and zero displayed undo-work manifests."
    }
}
if ($SyntheticRestoreManifestOnly.IsPresent) {
    Write-Host "Synthetic Restore Manifest-only mode: accepted package evidence, accepted launch commands, and fixture acceptance notes are skipped for focused regression coverage."
    Write-Host "Synthetic Restore Manifest-only mode must not be used as fresh real-profile movement evidence."
}
if ($null -ne $effectiveCleanupScope) {
    Write-Host "Restore Manifest display focus: $effectiveCleanupScope"
}
else {
    Write-Host "Restore Manifest display focus: all Cleanup Scopes."
}

if ($shouldRequireNoDisplayedUndoWork) {
    Write-Host "Displayed undo-work stop: checking before MVP preflight so blocked next-batch evidence does not produce fresh preflight evidence."
    Invoke-RealProfileReadinessStep -Title "Early displayed undo-work stop check" -CommandPath $restoreManifestSummary -Arguments (New-RestoreManifestArguments -UndoWorkOnly)
}

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
if (-not [string]::IsNullOrWhiteSpace($FixtureAcceptanceNotesPath)) {
    $dailyArguments += @("-FixtureAcceptanceNotesPath", $FixtureAcceptanceNotesPath)
}
if ($shouldIncludeFixtureAcceptanceNotes) {
    $dailyArguments += "-IncludeFixtureAcceptanceNotes"
}
if ($RequireFixtureAcceptanceComplete.IsPresent) {
    $dailyArguments += "-RequireFixtureAcceptanceComplete"
}
if (-not [string]::IsNullOrWhiteSpace($QuarantineRoot)) {
    $dailyArguments += @("-QuarantineRoot", $QuarantineRoot)
}
if ($null -ne $effectiveCleanupScope) {
    $dailyArguments += @("-CleanupScope", $effectiveCleanupScope)
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
if ($shouldRequireAnyDisplayedRestoreManifest) {
    $dailyArguments += "-RequireAnyDisplayedRestoreManifest"
}
if ($RequireNoDisplayedRecoveryReview.IsPresent) {
    $dailyArguments += "-RequireNoDisplayedRecoveryReview"
}
if ($shouldRequireNoDisplayedUndoWork) {
    $dailyArguments += "-RequireNoDisplayedUndoWork"
}
if ($SyntheticRestoreManifestOnly.IsPresent) {
    $dailyArguments += "-SyntheticRestoreManifestOnly"
}

Invoke-RealProfileReadinessStep -Title "Daily local readiness" -CommandPath $dailyReadiness -Arguments $dailyArguments
Invoke-RealProfileReadinessStep -Title "Recovery-review Restore Manifest focus" -CommandPath $restoreManifestSummary -Arguments (New-RestoreManifestArguments -RecoveryReviewOnly)
Invoke-RealProfileReadinessStep -Title "Undo-work Restore Manifest focus" -CommandPath $restoreManifestSummary -Arguments (New-RestoreManifestArguments -UndoWorkOnly)

Write-Host ""
Write-Host "Real-profile Quarantine readiness review passed."
Write-Host "This is not cleanup approval and does not prove that any future WPF batch is executable."
Write-Host "Before any next real-profile Quarantine click, keep the batch exact C:\Users\moxhe, at most 10 rows and 1 GB, Likely safe + Quarantine candidate only, no broad parents, no high-risk/protected/no-category/access-issue rows, with Quarantine Root safety, selected restore trust, exact QUARANTINE, Real-Profile Quarantine Approval Evidence, and immediate Pre-Execution Revalidation visible."
Write-Host "No WPF app was launched, no real-profile scan was started, and no files were moved, restored, deleted, or added to cleanup history."
