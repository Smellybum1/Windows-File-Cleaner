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
    Write-Host "  1. Confirm the header says Fixture Cleanup Scope, the Cleanup Scope Safety Note / scan-gate status share a compact wrapping strip, their ? help cues stay paired with the related text and mirror read-only/gated wording, Cleanup Scope browse tooltip is path-only, and fixture cleanup actions stay gated."
    Write-Host "  2. Click Scan manually; confirm the status says no files were modified."
    Write-Host "  3. Check Review Mix, Matched Review Mix, and Review Shortlist Safety Mix hoverable ? help cues plus prompt tooltip/help text, collapsible Safety Summary header/details with the Safety Summary panel-name prefix plus hoverable ? header help cue, header state styling, and state-naming tooltip/help text, review navigation/export tooltips, and Review Shortlist labels/tooltips."
    Write-Host "  4. Try search examples: old-installer, parent:$FixturePath\Downloads, under:$FixturePath\AppData."
    Write-Host "  5. Select folders and try selected-row tooltips, Show children, Show descendants, hotspot trail, subtree summary, and file preview."
    Write-Host "  6. Shortlist fixture cleanup candidates, check the collapsible Quarantine Shortlist header/details with the Quarantine Shortlist panel-name prefix plus hoverable ? header help cue, header state styling, and state-naming tooltip/help text, Quarantine Root browse tooltip plus safety-note ? help cue, styled inline preview readiness (neutral/success/warning/error) plus its hoverable ? help cue and state-naming tooltip/help text, compact Quarantine Readiness Summary states/tooltips, Remove overlapping parents for redundant parent/child previews, the shortlist confirmation ? help cue, the Quarantine Execution Gate ? help cue, preview/export tooltips, Approval boundary, Execution scope status, and execution tooltips."
    Write-Host "  7. For fixture only, type QUARANTINE, click Quarantine included shortlist, confirm Current quarantined shows the moved-entry count, use Current quarantined / Back to scan rows plus styled Review Grid Mode Status (neutral/informational/warning) and its hoverable ? help cue plus state-naming tooltip/help text, then Undo fixture quarantine and rescan before more review."
    Write-Host "  8. Use Discover manifests and Preview all-manifest readiness; check the Discover manifests ? help cue, selected manifest ? help cue, all-manifest readiness ? help cue, cue/control pairs stay together when rows wrap, no all-manifest restore action wording, and all-manifest readiness scope tooltips."
    Write-Host "  9. Use selected manifest readiness and the selected restore gate; check selected-only readiness wording, selected restore confirmation ? help cue, selected restore Approval boundary, Execution scope status, Selected Restore Execution Gate ? help cue in waiting/closed/open/restored states without crowding the gate area, and restore tooltips."
    Write-Host "  10. Confirm real-profile/custom scopes stay preview-only: Review Shortlist, clean Quarantine Preview, exact QUARANTINE, real-profile scan acknowledgement, and exact RESTORE do not unlock Quarantine or selected restore; ADR 0017 Quarantine blockers and ADR 0019 selected-restore blockers must stay explicit before any real-profile movement."
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
