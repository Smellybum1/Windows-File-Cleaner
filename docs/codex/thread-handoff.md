# Thread Handoff

Last updated: 2026-05-29

Use this when starting a fresh Codex thread for this repository.

## Current state

- Repo: `D:\Codex\Windows File Cleaner`
- GitHub: `Smellybum1/Windows-File-Cleaner`
- Branch: `main`
- Latest completed packet: Execution and Readiness Automation Help Text
- Current app stack: C# / WPF / .NET 8
- Desktop shortcut target: `D:\Codex\Windows File Cleaner\src\WindowsFileCleaner.App\bin\Debug\net8.0-windows\WindowsFileCleaner.App.exe`

## Product status

Windows File Cleaner is a local Windows-only desktop app for reviewing storage under `C:\Users\moxhe`.

The current MVP has a read-only Storage Scan that can inspect large real-profile scans with filters, debounced search, browse automation help text, selected-row action automation help text, execution/readiness automation help text, review report/preview automation help text, review toolbar automation help text, review navigation/export tooltip clarity, visible scope-specific Cleanup Scope Scan Gate status/tooltip wording, Cleanup Scope and Quarantine Root browse tooltip clarity, selected-row action tooltip clarity, Review Mix, Matched Review Mix, Review Shortlist Safety Mix, visible-row Review Shortlist bulk labels/tooltips, review toolbar report/preview tooltip clarity, selected-folder child/descendant focus, selected-folder summaries, hotspot trail, file preview, CSV export, Review Shortlist, fixture launcher checklist output and checklist-only mode, Quarantine Preview, Quarantine approval-boundary wording, Quarantine Execution Scope Status, execution-control tooltip clarity, readiness scope tooltip clarity, Restore Manifest Draft, Quarantine Confirmation Draft, confirmation label wording polish, Quarantine Action Draft, manifest discovery with all-manifest restore wording, Restore Manifest wording polish, selected manifest readiness label polish, all-manifest readiness label polish, selected manifest review with readiness-evidence wording, selected restore gate scope-status/approval-boundary wording, selected-restore scope-status checklist coverage, all-manifest readiness preview with all-manifest restore wording, and all-manifest restore boundary checklist coverage.

Fixture-only cleanup execution exists for synthetic Cleanup Scopes. Real-profile cleanup execution remains intentionally unavailable.

## Safety boundary

- Do not enable real-profile file movement without a new Grill with Docs pass, feature brief, tests, and ADR review if needed.
- Do not implement permanent deletion as the next step.
- Keep Storage Scan read-only.
- Keep Review Shortlist as review context, not cleanup approval.
- Keep Quarantine Preview as a dry run until explicit execution gates are designed and tested.
- Treat browser profiles, credentials, game saves, cloud sync data, source code, active app settings, Codex/tooling caches, and broad `AppData` parents conservatively.

## Verified recently

- User confirmed real-profile scan works.
- User confirmed the real-profile scan gate checkbox enabled `Scan` after acknowledgement.
- User reported Search typing was sluggish on large real-profile results.
- `ce7c1db` added debounced user-typed Storage Review Search.
- User retested and confirmed Search typing feels much better.
- User confirmed the status-bar pending-search message is enough; do not add another indicator unless future testing contradicts this.
- GitHub Actions MVP Preflight passed for `ce7c1db`: `https://github.com/Smellybum1/Windows-File-Cleaner/actions/runs/26616156540`
- Review Shortlist Safety Mix was added after the fresh-thread handoff docs so shortlist composition is visible before Quarantine Preview. It is read-only and not cleanup approval.
- Scan Gate Discoverability Polish added a visible locked/ready scan-gate summary and Scan button tooltip without changing the acknowledgement requirement.
- Quarantine Execution Scope Status added plain-language fixture-only versus preview-only scope wording to Quarantine Preview and Quarantine Execution Gate output.
- Fixture Review Checklist Output added a compact terminal checklist to `Start-MvpFixtureReview.ps1`.
- Fixture Review Checklist-Only Mode added `Start-MvpFixtureReview.ps1 -ChecklistOnly`, which prints the same checklist without preflight, fixture creation, or WPF launch.
- Quarantine Approval Boundary Wording added a compact preview/gate line that keeps Review Shortlist and Quarantine Preview separate from cleanup approval.
- Fixture Checklist Approval Boundary updated the launcher checklist to call out Approval boundary plus Execution scope status during fixture review.
- Confirmation Label Wording replaced stale WPF `Required future text` labels with `Required confirmation text` for Quarantine and Selected Restore panes.
- Selected Restore Scope Status added plain-language fixture-only versus preview-only selected restore scope wording plus an approval-boundary line to the Selected Restore Execution Gate.
- Fixture Checklist Selected Restore Scope updated the launcher checklist to call out selected restore Approval boundary plus Execution scope status during fixture review.
- Broad Restore Action Wording updated discovery/readiness/selected-review panes so they say no broad restore action is available and route fixture selected restore through selected readiness and the selected restore gate.
- Fixture Checklist Broad Restore Wording updated the launcher checklist to call out no broad restore action wording during fixture review.
- All-Manifest Restore Wording refined user-facing `broad restore action` wording to `all-manifest restore action` in WPF and checklist surfaces.
- Visible Row Shortlist Labels renamed bulk shortlist buttons/status text to `Shortlist visible rows` / `Remove visible rows` without changing the read-only display-window behavior.
- Scope-Specific Scan Gate Ready Wording made ready scan-gate summaries/tooltips distinguish fixture gated cleanup actions from unavailable real-profile/custom cleanup execution.
- Execution Control Tooltip Clarity updated disabled Quarantine and selected-restore controls so their tooltips match fixture-only gates and real-profile/custom blockers.
- Undo Quarantine Domain Consistency corrected current domain wording so Undo Quarantine Executor is described as used by current-fixture undo and fixture-only selected restore, while real-profile/all-manifest restore stays unavailable.
- Restore Manifest Wording Polish aligned current-facing manifest wording with Restore Manifest language in the current-fixture undo tooltip, README manual check, WPF smoke assertion messages, and domain docs without changing restore availability.
- Selected Manifest Readiness Label Polish renamed the visible selected-readiness action to `Preview selected manifest readiness` and aligned current checklist/docs/tests wording without changing restore availability.
- All-Manifest Readiness Label Polish renamed the all-manifest readiness action to `Preview all-manifest readiness` and aligned current checklist/docs/tests wording without changing restore availability.
- Readiness Scope Tooltip Clarity added scope/approval-boundary tooltips to `Preview all-manifest readiness` and `Preview selected manifest readiness`, including disabled-state tooltip support for selected manifest readiness.
- Visible-Row Shortlist Tooltip Clarity added disabled-state scope/no-file-modified/not-cleanup-approval tooltips to `Shortlist visible rows` and `Remove visible rows`.
- Review Toolbar Report and Preview Tooltip Clarity added disabled-state report-only, in-memory-only, and dry-run tooltips to `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview`.
- Selection Browse Tooltip Clarity added disabled-state path-only/preview-only/no-approval tooltips to Cleanup Scope and Quarantine Root `Browse...` controls.
- Selected-Row Action Tooltip Clarity added disabled-state review-only/inspection-only tooltips to selected-row shortlist, copy, focus, preview, and Explorer actions.
- Review Navigation and Export Tooltip Clarity added disabled-state report-only/no-rescan/in-memory tooltips to `Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows`.
- Review Toolbar Automation Help Text added matching WPF automation help text for `Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows`.
- Review Report and Preview Automation Help Text added matching WPF automation help text for `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview`.
- Selected Row Action Automation Help Text added matching WPF automation help text for `Add to shortlist`, `Remove`, `Copy path`, `Show children`, `Show descendants`, `Preview file`, and `Open in Explorer`.
- Selection Browse Automation Help Text added matching WPF automation help text for Cleanup Scope and Quarantine Root `Browse...`.
- Execution and Readiness Automation Help Text added matching WPF automation help text for Quarantine execution, current-fixture undo, restore readiness, selected restore gate, and fixture selected restore controls, plus a tooltip for `Preview selected restore gate`.

## Best next work

Prefer a small packet. Good candidates:

1. Manual fixture visual polish pass using `.\tools\Start-MvpFixtureReview.ps1`; optionally preview the prompts with `.\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`, use the launcher checklist, then update docs with any findings.
2. Continue Quarantine Preview/readiness wording review in the visible app, still without enabling real-profile execution.
3. Retest Review Shortlist Safety Mix, review navigation/export tooltips, Cleanup Scope/Quarantine Root browse tooltips, selected-row action tooltips, visible-row shortlist labels/tooltips, report/preview tooltips, execution-control tooltips, readiness tooltips, and Quarantine Execution Scope Status in the visible app during fixture or real-profile review; consider whether always-visible help icons are needed for safety-critical gates.
4. During the next visible fixture/real-profile pass, confirm the scope-specific scan-gate summary line fits comfortably and makes locked/ready state obvious.
5. Start real-profile Quarantine execution design only after the user explicitly wants to move beyond preview and the safety/undo story is reviewed again.

## Commands

```powershell
git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch
.\tools\Invoke-MvpPreflight.ps1
dotnet build WindowsFileCleaner.sln
dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj
dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj
dotnet run --project src\WindowsFileCleaner.App
.\tools\Start-MvpFixtureReview.ps1
.\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly
```

## Startup prompt

```text
We are continuing Windows File Cleaner in D:\Codex\Windows File Cleaner.

Read AGENTS.md, .codex/progress.md, README.md, docs/codex/thread-handoff.md, docs/domain/context.md, docs/domain/glossary.md, and relevant docs/features/ and docs/decisions/ before implementing.

Current state: main is pushed through the latest small read-only review polish packet. The app is a C#/.NET 8 WPF local Windows cleanup reviewer for C:\Users\moxhe. Storage Scan is read-only. Fixture-only Quarantine execution and fixture-only selected restore exist, but real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, and persisted cleanup history remain intentionally unavailable.

User verification: real-profile scan works; search typing was sluggish, then debounced search fixed it; the status-bar message is enough. Keep this as a local-first, safety-gated app. Do not move or delete real-profile files unless I explicitly ask after a Grill with Docs pass.

Please inspect current git status first, then choose the best next small packet. Prefer manual fixture/real-profile review polish, Quarantine Preview/readiness clarity, scan-gate discoverability, or Review Shortlist safety context before any real cleanup execution. Run the narrowest relevant checks, update docs, and commit/push at clean packet boundaries.
```
