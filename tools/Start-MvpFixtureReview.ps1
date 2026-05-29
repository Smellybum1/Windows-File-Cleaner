[CmdletBinding(SupportsShouldProcess)]
param(
    [string]$FixtureRoot = ".local\storage-scan-smoke-fixture",
    [switch]$SkipPreflight,
    [switch]$SkipLaunch,
    [switch]$SkipChecklist,
    [switch]$ChecklistOnly
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$preflightScript = Join-Path $PSScriptRoot "Invoke-MvpPreflight.ps1"
$fixtureScript = Join-Path $PSScriptRoot "New-StorageScanSmokeFixture.ps1"

function Write-FixtureReviewChecklist {
    param(
        [Parameter(Mandatory)]
        [string]$FixturePath
    )

    Write-Host ""
    Write-Host "Manual fixture review checklist:"
    Write-Host "  1. Confirm the header says Fixture Cleanup Scope and Scan ready for fixture."
    Write-Host "  2. Click Scan manually; confirm the status says no files were modified."
    Write-Host "  3. Check Review Mix, Matched Review Mix, Safety Summary, Review Shortlist Safety Mix, and visible-row shortlist labels."
    Write-Host "  4. Try search examples: old-installer, parent:$FixturePath\Downloads, under:$FixturePath\AppData."
    Write-Host "  5. Select folders and try Show children, Show descendants, hotspot trail, subtree summary, and file preview."
    Write-Host "  6. Shortlist the fixture cleanup candidate, create Quarantine Preview, and check Approval boundary plus Execution scope status."
    Write-Host "  7. For fixture only, type QUARANTINE, execute quarantine, then Undo fixture quarantine and rescan before more review."
    Write-Host "  8. Use Discover manifests, selected readiness/gate, and restore-readiness preview; check no all-manifest restore action plus selected restore Approval boundary and Execution scope status."
    Write-Host "  9. Confirm real-profile/custom Quarantine and selected restore execution remain unavailable in wording before any later real-profile work."
}

if ([System.IO.Path]::IsPathRooted($FixtureRoot)) {
    $fixtureFullPath = [System.IO.Path]::GetFullPath($FixtureRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
}
else {
    $fixtureFullPath = [System.IO.Path]::GetFullPath((Join-Path $repoRoot $FixtureRoot)).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
}

if (-not ($fixtureFullPath.Equals($repoFullPath, [System.StringComparison]::OrdinalIgnoreCase) -or
    $fixtureFullPath.StartsWith($repoFullPath + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase))) {
    throw "Fixture root must stay inside the repository: $repoFullPath"
}

if ($ChecklistOnly) {
    Write-Host ""
    Write-Host "Fixture Cleanup Scope: $fixtureFullPath"
    Write-Host "Checklist-only mode. No preflight, fixture creation, or WPF launch will run."
    Write-FixtureReviewChecklist -FixturePath $fixtureFullPath
    return
}

Push-Location $repoRoot
try {
    if (-not $SkipPreflight) {
        if ($PSCmdlet.ShouldProcess("MVP preflight", "Run")) {
            & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $preflightScript
        }
    }

    if ($PSCmdlet.ShouldProcess($fixtureFullPath, "Create synthetic Storage Scan fixture")) {
        & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $fixtureScript -Root $fixtureFullPath
    }

    Write-Host ""
    Write-Host "Fixture Cleanup Scope: $fixtureFullPath"
    Write-Host "The WPF app will only prefill the Cleanup Scope. It will not auto-scan."
    Write-Host "After the app opens, click Scan yourself and confirm the status says no files were modified."
    if (-not $SkipChecklist) {
        Write-FixtureReviewChecklist -FixturePath $fixtureFullPath
    }

    if (-not $SkipLaunch) {
        if ($PSCmdlet.ShouldProcess("Windows File Cleaner fixture review", "Launch WPF app")) {
            & dotnet run --project src\WindowsFileCleaner.App -- --scope $fixtureFullPath
        }
    }
    else {
        Write-Host "Launch skipped. Manual command:"
        Write-Host "dotnet run --project src\WindowsFileCleaner.App -- --scope `"$fixtureFullPath`""
    }
}
finally {
    Pop-Location
}
