[CmdletBinding(SupportsShouldProcess)]
param(
    [string]$FixtureRoot = ".local\storage-scan-smoke-fixture",
    [switch]$SkipPreflight,
    [switch]$SkipLaunch,
    [switch]$SkipChecklist,
    [switch]$ChecklistOnly,
    [switch]$WriteAcceptanceNotes
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$preflightScript = Join-Path $PSScriptRoot "Invoke-MvpPreflight.ps1"
$fixtureScript = Join-Path $PSScriptRoot "New-StorageScanSmokeFixture.ps1"

function Get-FixtureReviewChecklistItems {
    param(
        [Parameter(Mandatory)]
        [string]$FixturePath
    )

    @(
        "Confirm the header says Fixture Cleanup Scope, the Cleanup Scope Safety Note / scan-gate status share a compact wrapping strip, their ? help cues stay paired with the related text and mirror read-only/gated wording, Cleanup Scope browse tooltip is path-only, and fixture cleanup actions stay gated.",
        "Click Scan manually; confirm the status says no files were modified.",
        "Check the compact header Review Shortlist totals sit to the left of Total size when space allows, compact scan totals remain readable, and the horizontal tabbed workbench: Main Grid should be selected by default, Safety Summary / Review / Quarantine / Main Grid tabs should each have their own page, Safety Summary shortcuts should auto-focus Main Grid after applying their read-only review lens, Main Grid should mirror the active review lens above Storage Scan rows without implying rescan or cleanup approval, Safety Summary should start expanded with the Safety Summary panel-name prefix plus hoverable ? header help cue, and Review Mix, Matched Review Mix, and Review Shortlist Safety Mix hoverable ? help cues plus prompt tooltip/help text, header state styling, state-naming tooltip/help text, review navigation/export tooltips, and Review Shortlist labels/tooltips should stay readable.",
        "Try search examples: old-installer, parent:$FixturePath\Downloads, under:$FixturePath\AppData.",
        "Select folders and try selected-row tooltips, Show children, Show descendants, hotspot trail, subtree summary, and file preview.",
        "Shortlist fixture cleanup candidates, open the Quarantine tab, check the Quarantine Shortlist starts expanded with the Quarantine Shortlist panel-name prefix plus hoverable ? header help cue, and check header state styling, state-naming tooltip/help text, Quarantine Root browse tooltip plus safety-note ? help cue, non-D root readiness acknowledgement wording plus its hoverable ? help cue, styled inline preview readiness (neutral/success/warning/error) plus its hoverable ? help cue and state-naming tooltip/help text, compact Quarantine Readiness Summary states/tooltips including preview-only movement unavailable wording and Key blockers labels such as custom scope preview-only, exact profile scope, pre-execution revalidation, restore readiness, current build unavailable, 10-row cap, 1 GB cap, no-category rows, and strict descendant checks when those blockers apply, Remove overlapping parents for redundant parent/child previews, the shortlist confirmation ? help cue, the Quarantine Execution Gate ? help cue, preview/export tooltips, Approval boundary, Execution scope status, and execution tooltips.",
        "For fixture only, type QUARANTINE, click Quarantine included shortlist, confirm Current quarantined shows the moved-entry count, use Current quarantined / Back to scan rows and confirm both auto-focus Main Grid, check styled Review Grid Mode Status (neutral/informational/warning) plus its hoverable ? help cue and state-naming tooltip/help text, confirm the Main Grid active review lens summary appears when scan rows are showing and hides for current-session quarantined rows, then Undo fixture quarantine and rescan before more review.",
        "Use Discover manifests and Preview all-manifest readiness; check the Discover manifests ? help cue, selected manifest ? help cue, all-manifest readiness ? help cue, Restore Manifest review summary states/tooltips, cue/control pairs stay together when rows wrap, no all-manifest restore action wording, and all-manifest readiness scope tooltips.",
        "Use selected manifest readiness and the selected restore gate; check selected-only readiness wording, selected restore confirmation ? help cue, selected restore Approval boundary, Execution scope status, Selected Restore Execution Gate ? help cue in waiting/closed/open/restored states without crowding the gate area, fixture restore tooltips, and read-only selected restore revalidation evidence for exact real-profile Restore Manifests when available.",
        "Without scanning C:\Users\moxhe as part of this fixture pass, confirm real-profile/custom scopes stay preview-only when using custom preview-only paths or existing synthetic readiness evidence: Review Shortlist, clean Quarantine Preview, exact QUARANTINE, real-profile scan acknowledgement, exact RESTORE, and clean selected restore revalidation evidence do not unlock Quarantine or selected restore; the Quarantine Execution Gate should show read-only Real-Profile Quarantine Approval Evidence with Can approve real-profile movement: no, while ADR 0017 Quarantine blockers, ADR 0018 first-phase limits such as 10 rows, 1 GB, no-category, and strict descendant checks, and ADR 0019 selected-restore blockers stay explicit before any real-profile movement."
    )
}

function Get-FixtureReviewChecklistSection {
    param(
        [Parameter(Mandatory)]
        [int]$Index
    )

    if ($Index -le 1) {
        return "Scan header and gate"
    }
    elseif ($Index -le 4) {
        return "Safety Summary, Review, and Main Grid"
    }
    elseif ($Index -le 6) {
        return "Quarantine Preview and fixture Quarantine"
    }
    elseif ($Index -le 8) {
        return "Restore Manifest review and selected restore"
    }

    return "Real-profile and custom blockers"
}

function Write-FixtureReviewChecklist {
    param(
        [Parameter(Mandatory)]
        [string]$FixturePath
    )

    $checklistItems = Get-FixtureReviewChecklistItems -FixturePath $FixturePath

    Write-Host ""
    Write-Host "Manual fixture review checklist:"
    $currentSection = $null
    for ($index = 0; $index -lt $checklistItems.Count; $index++) {
        $section = Get-FixtureReviewChecklistSection -Index $index
        if ($section -ne $currentSection) {
            $currentSection = $section
            Write-Host ("  {0}:" -f $currentSection)
        }

        Write-Host ("    {0}. {1}" -f ($index + 1), $checklistItems[$index])
    }
}

function Write-FixtureAcceptanceNotesNextSteps {
    param(
        [Parameter(Mandatory)]
        [string]$NotesPath
    )

    Write-Host "After the visible pass, fill the notes file, then run:"
    Write-Host ".\tools\Summarize-FixtureAcceptanceNotes.cmd -Path `"$NotesPath`""
    Write-Host ".\tools\Summarize-FixtureAcceptanceNotes.cmd -Path `"$NotesPath`" -RequireComplete"
    Write-Host "These summary commands read ignored notes only; they do not launch WPF, scan, move, restore, delete, or create cleanup history."
}

function Get-FixtureReviewGitValue {
    param(
        [Parameter(Mandatory)]
        [string[]]$Arguments
    )

    try {
        $output = & git @Arguments 2>$null
        if ($LASTEXITCODE -eq 0) {
            $value = $output | Select-Object -First 1
            if (-not [string]::IsNullOrWhiteSpace([string]$value)) {
                return ([string]$value).Trim()
            }
        }
    }
    catch {
    }

    return "unknown"
}

function Get-FixtureReviewWorktreeStatus {
    param(
        [Parameter(Mandatory)]
        [string]$RepositoryPath
    )

    try {
        $output = & git -C $RepositoryPath status --short 2>$null
        if ($LASTEXITCODE -ne 0) {
            return "unknown"
        }

        $statusLines = @($output | Where-Object { -not [string]::IsNullOrWhiteSpace([string]$_) })
        if ($statusLines.Count -eq 0) {
            return "clean"
        }

        $suffix = if ($statusLines.Count -eq 1) { "" } else { "s" }
        return ("not clean ({0} status line{1})" -f $statusLines.Count, $suffix)
    }
    catch {
    }

    return "unknown"
}

function Get-FixtureReviewCommandValue {
    param(
        [Parameter(Mandatory)]
        [string]$Command,

        [Parameter(Mandatory)]
        [string[]]$Arguments
    )

    try {
        $output = & $Command @Arguments 2>$null
        if ($LASTEXITCODE -eq 0) {
            $value = $output | Select-Object -First 1
            if (-not [string]::IsNullOrWhiteSpace([string]$value)) {
                return ([string]$value).Trim()
            }
        }
    }
    catch {
    }

    return "unknown"
}

function Get-FixtureReviewProjectProperty {
    param(
        [Parameter(Mandatory)]
        [string]$ProjectPath,

        [Parameter(Mandatory)]
        [string]$PropertyName
    )

    try {
        if (-not (Test-Path -LiteralPath $ProjectPath)) {
            return "unknown"
        }

        [xml]$projectXml = Get-Content -Raw -LiteralPath $ProjectPath
        foreach ($propertyGroup in @($projectXml.Project.PropertyGroup)) {
            $propertyValue = $propertyGroup.$PropertyName
            if (-not [string]::IsNullOrWhiteSpace([string]$propertyValue)) {
                return ([string]$propertyValue).Trim()
            }
        }
    }
    catch {
    }

    return "unknown"
}

function New-FixtureAcceptanceNotes {
    param(
        [Parameter(Mandatory)]
        [string]$FixturePath
    )

    $notesRoot = Join-Path $repoRoot ".local\fixture-review-acceptance"
    $timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
    $notesPath = Join-Path $notesRoot ("fixture-acceptance-{0}.md" -f $timestamp)
    $checklistItems = Get-FixtureReviewChecklistItems -FixturePath $FixturePath
    $gitBranch = Get-FixtureReviewGitValue -Arguments @("-C", $repoFullPath, "rev-parse", "--abbrev-ref", "HEAD")
    $gitCommit = Get-FixtureReviewGitValue -Arguments @("-C", $repoFullPath, "rev-parse", "--short", "HEAD")
    $worktreeStatus = Get-FixtureReviewWorktreeStatus -RepositoryPath $repoFullPath
    $dotnetSdkVersion = Get-FixtureReviewCommandValue -Command "dotnet" -Arguments @("--version")
    $appProjectRelativePath = "src\WindowsFileCleaner.App\WindowsFileCleaner.App.csproj"
    $appProjectPath = Join-Path $repoRoot $appProjectRelativePath
    $appTargetFramework = Get-FixtureReviewProjectProperty -ProjectPath $appProjectPath -PropertyName "TargetFramework"
    if ($appTargetFramework -eq "unknown") {
        $appTargetFramework = Get-FixtureReviewProjectProperty -ProjectPath $appProjectPath -PropertyName "TargetFrameworks"
    }
    $appUseWpf = Get-FixtureReviewProjectProperty -ProjectPath $appProjectPath -PropertyName "UseWPF"

    New-Item -ItemType Directory -Path $notesRoot -Force | Out-Null

    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.Add("# Fixture Acceptance Notes")
    $lines.Add("")
    $lines.Add("Created: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')")
    $lines.Add("Fixture Cleanup Scope: $FixturePath")
    $lines.Add("")
    $lines.Add("Acceptance evidence:")
    $lines.Add("")
    $lines.Add("- Repository: $repoFullPath")
    $lines.Add("- Git branch: $gitBranch")
    $lines.Add("- Git commit: $gitCommit")
    $lines.Add("- Worktree status at notes creation: $worktreeStatus")
    $lines.Add("- .NET SDK: $dotnetSdkVersion")
    $lines.Add("- WPF app project: $appProjectRelativePath")
    $lines.Add("- WPF app target framework: $appTargetFramework")
    $lines.Add("- WPF enabled: $appUseWpf")
    $lines.Add('- Required preflight: `.\tools\Invoke-MvpPreflight.cmd`')
    $lines.Add('- Visible fixture command after preflight: `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`')
    $lines.Add("- [ ] Preflight passed immediately before this visible fixture pass.")
    $lines.Add("- [ ] Worktree was clean or intentional changes were recorded before launch.")
    $lines.Add('- Notes file is local/ignored under `.local` and is not cleanup history.')
    $lines.Add("")
    $lines.Add("Safety boundary:")
    $lines.Add("")
    $lines.Add("- Do not scan C:\Users\moxhe as part of this fixture pass.")
    $lines.Add("- Do not move, restore, delete, or modify real-profile files.")
    $lines.Add("- Fixture-only Quarantine execution and fixture-only selected restore are allowed only inside the synthetic Cleanup Scope.")
    $lines.Add("")
    $lines.Add("Post-pass summary commands:")
    $lines.Add("")
    $lines.Add("After filling this file, run these commands from the repository root:")
    $lines.Add("")
    $lines.Add('```powershell')
    $lines.Add(('.\tools\Summarize-FixtureAcceptanceNotes.cmd -Path "{0}"' -f $notesPath))
    $lines.Add(('.\tools\Summarize-FixtureAcceptanceNotes.cmd -Path "{0}" -RequireComplete' -f $notesPath))
    $lines.Add('```')
    $lines.Add("")
    $lines.Add("These commands read this ignored notes file only. They do not launch WPF, scan, move, restore, delete, or create cleanup history.")
    $lines.Add("")
    $lines.Add("Overall result:")
    $lines.Add("")
    $lines.Add("- [ ] Pass")
    $lines.Add("- [ ] Pass with issues noted")
    $lines.Add("- [ ] Blocked")
    $lines.Add("")
    $lines.Add("Summary:")
    $lines.Add("")
    $lines.Add("- ")
    $lines.Add("")
    $lines.Add("Checklist:")
    $lines.Add("")
    $lines.Add("The sections mirror the fixture review flow. Checklist numbers match the terminal output.")

    $currentSection = $null
    for ($index = 0; $index -lt $checklistItems.Count; $index++) {
        $section = Get-FixtureReviewChecklistSection -Index $index
        if ($section -ne $currentSection) {
            $currentSection = $section
            $lines.Add("")
            $lines.Add("## $currentSection")
        }

        $lines.Add("")
        $lines.Add("### $($index + 1). Fixture check")
        $lines.Add("")
        $lines.Add("Prompt: $($checklistItems[$index])")
        $lines.Add("")
        $lines.Add("- [ ] Pass")
        $lines.Add("- [ ] Issue")
        $lines.Add("- [ ] Not checked")
        $lines.Add("")
        $lines.Add("Notes:")
        $lines.Add("")
        $lines.Add("- ")
    }

    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllLines($notesPath, $lines, $utf8NoBom)

    return $notesPath
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
    if ($WriteAcceptanceNotes -and $PSCmdlet.ShouldProcess($fixtureFullPath, "Write fixture acceptance notes template")) {
        $notesPath = New-FixtureAcceptanceNotes -FixturePath $fixtureFullPath
        Write-Host ""
        Write-Host "Fixture acceptance notes template: $notesPath"
        Write-FixtureAcceptanceNotesNextSteps -NotesPath $notesPath
    }
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
    if ($WriteAcceptanceNotes -and $PSCmdlet.ShouldProcess($fixtureFullPath, "Write fixture acceptance notes template")) {
        $notesPath = New-FixtureAcceptanceNotes -FixturePath $fixtureFullPath
        Write-Host ""
        Write-Host "Fixture acceptance notes template: $notesPath"
        Write-FixtureAcceptanceNotesNextSteps -NotesPath $notesPath
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
