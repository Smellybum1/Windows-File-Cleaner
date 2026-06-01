[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$cleanupScope = "C:\Users\moxhe"

function Write-ChecklistSection {
    param(
        [Parameter(Mandatory)]
        [string]$Title,

        [Parameter(Mandatory)]
        [string[]]$Items
    )

    Write-Host ("  {0}:" -f $Title)
    for ($index = 0; $index -lt $Items.Count; $index++) {
        Write-Host ("    {0}. {1}" -f ($index + 1), $Items[$index])
    }
}

Write-Host "Real-profile next-batch WPF checklist"
Write-Host "Repository: $repoFullPath"
Write-Host "Cleanup Scope: $cleanupScope"
Write-Host "Boundary: terminal guidance only; this does not launch WPF, click Scan, scan, move, restore, delete, approve cleanup, write manifests, or create cleanup history."
Write-Host "Stop boundary: Codex must not click real-profile movement. The user must explicitly approve a specific tiny batch after reviewing WPF readiness, exact QUARANTINE, approval evidence, and immediate revalidation."
Write-Host ""
Write-Host "Recommended combined terminal review:"
Write-Host ".\tools\Invoke-RealProfileNextBatchReview.cmd"
Write-Host "This runs the exact-profile evidence preset first, then prints this checklist."
Write-Host ""
Write-Host "Prerequisite terminal evidence when running evidence separately:"
Write-Host ".\tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence"
Write-Host ""
Write-Host "Accepted package launch command printer:"
Write-Host ".\tools\Start-AcceptedLocalRelease.cmd -PrintOnly"
Write-Host "Remove -PrintOnly only when the human user intentionally wants to launch the accepted package."
Write-Host ""
Write-Host "Manual WPF checklist:"

Write-ChecklistSection -Title "Before launch" -Items @(
    "Confirm the next-batch evidence preset passed on the current worktree, including full MVP preflight unless it was intentionally skipped only for a smoke check.",
    "Confirm the accepted package evidence is complete; a package/current-HEAD warning is expected when newer commits are docs/tooling-only.",
    "Do not continue if exact-profile Restore Manifest display is missing or displayed undo-work manifests are present."
)

Write-ChecklistSection -Title "Scan and shortlist" -Items @(
    "Launch the accepted package only by human choice, keep Cleanup Scope exactly C:\Users\moxhe, and do not use a child, custom, ProgramData, or Program Files scope.",
    "Acknowledge the real-profile scan gate only after the prerequisite evidence is reviewed, then click Scan manually.",
    "Review rows through filters/search/folder inspection before shortlisting; keep Review Shortlist as review context, not cleanup approval.",
    "Shortlist at most 10 rows and at most 1 GB total for this exact batch.",
    "Use only Likely safe rows with Quarantine candidate recommendation, specific rebuildable-cache evidence, and no access issues."
)

Write-ChecklistSection -Title "Hard blockers" -Items @(
    "Stop on broad parents, Cleanup Scope Root, AppData parents, profile containers, protected locations, high-risk rows, no-category rows, access-issue rows, outside-scope evidence, reparse points, cloud sync data, credential data, source code, game saves, active app settings, or Codex/tooling state.",
    "For folders, require narrow folder scope plus strict descendant checks with no protected, high-risk, inaccessible, no-category, outside-scope, or reparse-point descendants.",
    "Do not force-close processes or bypass in-use source blockers."
)

Write-ChecklistSection -Title "Quarantine tab" -Items @(
    "Use the Quarantine tab and keep Quarantine Root fully qualified, outside the Cleanup Scope, with enough capacity; prefer the D: root unless a non-D root is intentionally acknowledged.",
    "Click Preview shortlist quarantine and verify included, blocked, redundant, and stale state wording before any confirmation.",
    "Confirm Quarantine Readiness Summary, Quarantine Root Execution Safety, selected real-profile restore trust, Pre-Execution Revalidation, Restore Manifest draft, and Quarantine Action Draft evidence are visible and clean.",
    "Confirm the highlighted gate/status text says Can execute yes, exact QUARANTINE matched only after typing, real-profile approval can proceed, and no blocker remains."
)

Write-ChecklistSection -Title "Execution boundary" -Items @(
    "Only the human user may type exact QUARANTINE and click Quarantine included shortlist for the specific reviewed batch.",
    "After any attempt, immediately read the result, recovery-review state, and Restore Manifest path; do not chain another batch.",
    "Rediscover Restore Manifests and rescan manually before further review."
)

Write-Host ""
Write-Host "This checklist is not cleanup approval and does not prove that any future WPF batch is executable."
Write-Host "No WPF app was launched, no scan was started, and no files were moved, restored, deleted, approved, or added to cleanup history."
