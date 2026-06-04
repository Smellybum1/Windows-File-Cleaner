[CmdletBinding()]
param(
    [switch]$SkipRestore,
    [switch]$SkipFixtureWhatIf,
    [switch]$SkipFixtureChecklist,
    [switch]$SkipFixtureAcceptanceNotesCheck,
    [switch]$SkipDailyReadinessFixtureAcceptanceCheck,
    [switch]$SkipDailyReadinessLatestPackageNotesCheck,
    [switch]$SkipAcceptedLocalReleaseSelectionCheck,
    [switch]$SkipLocalReleaseAcceptanceSummaryCheck,
    [switch]$SkipLocalReleaseAcceptanceRecorderCheck,
    [switch]$SkipRealProfileNextBatchStopGuardCheck,
    [switch]$SkipDailyReadinessUndoSpotlightCheck,
    [switch]$SkipDiffCheck
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$fixtureScript = Join-Path $PSScriptRoot "New-StorageScanSmokeFixture.ps1"
$fixtureReviewScript = Join-Path $PSScriptRoot "Start-MvpFixtureReview.ps1"
$fixtureAcceptanceNotesTestScript = Join-Path $PSScriptRoot "Test-FixtureAcceptanceNotes.ps1"
$dailyReadinessFixtureAcceptanceTestScript = Join-Path $PSScriptRoot "Test-DailyReadinessFixtureAcceptanceNotes.ps1"
$dailyReadinessLatestPackageNotesTestScript = Join-Path $PSScriptRoot "Test-DailyReadinessLatestPackageNotes.ps1"
$acceptedLocalReleaseSelectionTestScript = Join-Path $PSScriptRoot "Test-AcceptedLocalReleaseSelection.ps1"
$localReleaseAcceptanceSummaryTestScript = Join-Path $PSScriptRoot "Test-LocalReleaseAcceptanceSummary.ps1"
$localReleaseAcceptanceRecorderTestScript = Join-Path $PSScriptRoot "Test-LocalReleaseAcceptanceRecorder.ps1"
$realProfileNextBatchStopGuardTestScript = Join-Path $PSScriptRoot "Test-RealProfileNextBatchStopGuard.ps1"
$dailyReadinessUndoSpotlightTestScript = Join-Path $PSScriptRoot "Test-DailyReadinessExactProfileUndoSpotlight.ps1"
$fixtureRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\storage-scan-smoke-fixture")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)

function Invoke-PreflightStep {
    param(
        [Parameter(Mandatory)]
        [string]$Name,

        [Parameter(Mandatory)]
        [scriptblock]$Command
    )

    Write-Host ""
    Write-Host "== $Name =="
    $global:LASTEXITCODE = 0
    & $Command
    $exitCode = $global:LASTEXITCODE
    if ($exitCode -ne 0) {
        throw "Preflight step '$Name' failed with exit code $exitCode."
    }
}

Push-Location $repoRoot
try {
    if (-not $SkipRestore) {
        Invoke-PreflightStep -Name "Restore" -Command {
            & dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config
        }
    }

    Invoke-PreflightStep -Name "Build" -Command {
        & dotnet build WindowsFileCleaner.sln --no-restore
    }

    Invoke-PreflightStep -Name "Core tests" -Command {
        & dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build
    }

    Invoke-PreflightStep -Name "WPF app tests" -Command {
        & dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build
    }

    if (-not $SkipFixtureWhatIf) {
        Invoke-PreflightStep -Name "Fixture dry run" -Command {
            & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $fixtureScript -WhatIf
        }
    }

    if (-not $SkipFixtureChecklist) {
        Invoke-PreflightStep -Name "Fixture checklist" -Command {
            & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $fixtureReviewScript -ChecklistOnly
        }
    }

    if (-not $SkipFixtureAcceptanceNotesCheck) {
        Invoke-PreflightStep -Name "Fixture acceptance notes regression" -Command {
            & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $fixtureAcceptanceNotesTestScript
        }
    }

    if (-not $SkipDailyReadinessFixtureAcceptanceCheck) {
        Invoke-PreflightStep -Name "Daily readiness fixture acceptance notes regression" -Command {
            & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $dailyReadinessFixtureAcceptanceTestScript
        }
    }

    if (-not $SkipDailyReadinessLatestPackageNotesCheck) {
        Invoke-PreflightStep -Name "Daily readiness latest package notes regression" -Command {
            & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $dailyReadinessLatestPackageNotesTestScript
        }
    }

    if (-not $SkipAcceptedLocalReleaseSelectionCheck) {
        Invoke-PreflightStep -Name "Accepted local release selection regression" -Command {
            & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $acceptedLocalReleaseSelectionTestScript
        }
    }

    if (-not $SkipLocalReleaseAcceptanceSummaryCheck) {
        Invoke-PreflightStep -Name "Local release acceptance summary regression" -Command {
            & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $localReleaseAcceptanceSummaryTestScript
        }
    }

    if (-not $SkipLocalReleaseAcceptanceRecorderCheck) {
        Invoke-PreflightStep -Name "Local release acceptance recorder regression" -Command {
            & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $localReleaseAcceptanceRecorderTestScript
        }
    }

    if (-not $SkipRealProfileNextBatchStopGuardCheck) {
        Invoke-PreflightStep -Name "Real-profile next-batch stop guard regression" -Command {
            & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $realProfileNextBatchStopGuardTestScript
        }
    }

    if (-not $SkipDailyReadinessUndoSpotlightCheck) {
        Invoke-PreflightStep -Name "Daily readiness exact-profile undo spotlight regression" -Command {
            & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $dailyReadinessUndoSpotlightTestScript
        }
    }

    if (-not $SkipDiffCheck) {
        Invoke-PreflightStep -Name "Whitespace diff check" -Command {
            & git -c "safe.directory=$repoRoot" diff --check
        }
    }

    Write-Host ""
    Write-Host "MVP preflight passed. No real user files were scanned or modified."
    Write-Host "Next manual fixture step:"
    Write-Host ".\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes"
    Write-Host "This creates the synthetic fixture, writes ignored acceptance notes, and launches WPF with this Cleanup Scope:"
    Write-Host $fixtureRoot
    Write-Host "After filling the notes, use the printed Summarize-FixtureAcceptanceNotes commands to review open items and require completion."
}
finally {
    Pop-Location
}
