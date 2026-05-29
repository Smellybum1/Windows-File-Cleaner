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
    Write-Host "  1. Confirm the header says Fixture Cleanup Scope, Scan ready for fixture, Cleanup Scope browse tooltip is path-only, and fixture cleanup actions stay gated."
    Write-Host "  2. Click Scan manually; confirm the status says no files were modified."
    Write-Host "  3. Check Review Mix, Matched Review Mix, and Review Shortlist Safety Mix ? help cues plus tooltip/help text, collapsible Safety Summary header/details with the Safety Summary panel-name prefix plus header state styling and state-naming tooltip/help text, review navigation/export tooltips, and Review Shortlist labels/tooltips."
    Write-Host "  4. Try search examples: old-installer, parent:$FixturePath\Downloads, under:$FixturePath\AppData."
    Write-Host "  5. Select folders and try selected-row tooltips, Show children, Show descendants, hotspot trail, subtree summary, and file preview."
    Write-Host "  6. Shortlist fixture cleanup candidates, check the collapsible Quarantine Shortlist header/details with the Quarantine Shortlist panel-name prefix plus header state styling and state-naming tooltip/help text, Quarantine Root browse tooltip, styled inline preview readiness (neutral/success/warning/error) plus state-naming tooltip/help text, preview/export tooltips, Approval boundary, Execution scope status, and execution tooltips."
    Write-Host "  7. For fixture only, type QUARANTINE, execute quarantine, use Current quarantined / Back to scan rows plus styled Review Grid Mode Status (neutral/informational/warning) and its ? help cue plus state-naming tooltip/help text, then Undo fixture quarantine and rescan before more review."
    Write-Host "  8. Use Discover manifests, selected manifest readiness/gate, and all-manifest readiness preview; check no all-manifest restore action plus readiness scope tooltips, selected restore Approval boundary, Execution scope status, and restore tooltips."
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
