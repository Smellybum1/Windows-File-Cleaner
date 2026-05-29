# Progress Log

This file is the running evidence log for Codex work in this repository.

Use it to preserve what was completed, what was verified, what was rejected, and what should happen next. Keep entries compact and factual.

## Current status

Storage Scan MVP packet implemented and tested by the user against `C:\Users\moxhe`. The app has a broad read-only review workflow, manual fixture Show children/Clipboard crash fix, scan cancel help text, real-profile acknowledgement help text, Review Mix, Matched Review Mix, Review Shortlist Safety Mix, Review Grid Mode Status, and inline Quarantine Preview status `?` help cues with tooltip/help text, Safety Summary shortcut help text, Safety Summary collapsed-header state styling, review-lens filter help text, manifest discovery/selection help text, debounced Storage Review Search for large real-profile scans, Storage Review Search input automation help text, scan-gate automation help text, Cleanup Scope input/browse automation help text, Quarantine Root input/browse automation help text, selected-row action automation help text, visible-row shortlist automation help text, execution/readiness automation help text, review report/preview automation help text, review toolbar automation help text, review navigation/export tooltip clarity, Review Grid Mode Status tooltip/help text, scope-specific Cleanup Scope Scan Gate discoverability polish, Cleanup Scope and Quarantine Root browse tooltip clarity, selected-row action tooltip clarity, Matched Review Mix, Review Shortlist Safety Mix, visible-row Review Shortlist bulk labels/tooltips, review toolbar report/preview tooltip clarity, Selected Folder Subtree Summary, Storage Hotspot Trail, Selected Folder Child Focus, Selected Folder Descendant Focus, fixture launch/preflight tooling with checklist output, checklist-only mode, approval-boundary prompt coverage, selected-restore scope-status checklist coverage, all-manifest restore boundary checklist coverage, execution-control tooltip clarity, readiness scope tooltip clarity, Undo Quarantine domain consistency, Restore Manifest wording polish, Selected Manifest Readiness label polish, and All-Manifest Readiness label polish, Quarantine Preview, Quarantine approval-boundary wording, Quarantine Execution Scope Status, Restore Manifest Draft, Quarantine Confirmation Draft, confirmation label wording polish, Quarantine Action Draft, write-ahead Restore Manifest persistence, core Quarantine execution, core Undo Quarantine, fixture-only WPF Quarantine execution, WPF undo for the current fixture execution, Current quarantined grid switching for current-session moved entries, Quarantine Manifest Discovery with all-manifest restore wording, Selected Restore Manifest Review with readiness-evidence wording, Selected Restore Confirmation Gate with scope-status/approval-boundary wording, Fixture-only Selected Restore Execution, and Restore Readiness Preview with all-manifest restore wording. Real-profile WPF Quarantine execution, real-profile WPF Undo Quarantine, permanent deletion, and persisted cleanup history remain unavailable. Fresh-thread handoff notes and a startup prompt live in `docs/codex/thread-handoff.md`.

## Next recommended work

1. Start the next thread from `docs/codex/thread-handoff.md`, optionally print the manual prompts with `.\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`, then run `.\tools\Start-MvpFixtureReview.ps1`, confirm the launched app shows Fixture Cleanup Scope, scope-specific scan-gate wording, Cleanup Scope browse tooltip, click `Scan`, and manually inspect layout, visible wording, Storage Review Search, Clear search tooltip, `parent:` search, `under:` search, Matched Review Mix, Review Shortlist Safety Mix, Selected Folder Subtree Summary, Storage Hotspot Trail, Selected Folder Child Focus (`Show children`), Selected Folder Descendant Focus (`Show descendants`), selected-row action tooltips, Storage Review Display Window controls and Previous/Next rows tooltips, Scan Report Export tooltip, Reset view tooltip, the `Relative path` and `Parent` columns, Selected Path Hierarchy Context, Selected File Content Preview, Selected Path Review Guidance, export dialogs, Safety Summary shortcuts, Review Shortlist, `Shortlist visible rows`, `Remove visible rows`, visible-row shortlist tooltips, Review Shortlist export/clear tooltips, typed/browsed Quarantine Root Selection, Quarantine Root browse tooltip, Quarantine Root Safety Note, Quarantine Preview, Preview quarantine/export tooltips, Quarantine Execution Gate, Quarantine Action Draft, execution-control tooltips, fixture-only `Execute quarantine`, `Current quarantined`, current-fixture `Undo fixture quarantine`, `Discover manifests`, `Preview selected manifest readiness`, `Preview selected restore gate` tooltip, scope status, and approval boundary, fixture-only `Restore selected fixture manifest`, `Preview all-manifest readiness`, Review Mix, Access issues filter, category filter, No category filter, Size filter, and filter wording.
2. Use `README.md` and `docs/features/2026-05-28-mvp-readiness-audit.md` to rerun the WPF app against `C:\Users\moxhe`; confirm `Scan` is disabled until the real-profile preflight acknowledgement is checked.
3. Run `.\tools\Invoke-MvpPreflight.ps1` before any later real-profile scan if the worktree changes.
4. Rerun the real scan and check whether the cleanup scope root row, `Relative path`, `Parent`, `Contents`, and `Access` columns, Size filter, `parent:` / `under:` / `access:readable` / `access:access issue` search, Matched Review Mix, Selected Folder Subtree Summary, Storage Hotspot Trail, `Show children`, `Show descendants`, Previous rows / Next rows, Safety Summary candidate and no-category examples, selected-row relative/parent/depth/access context, cache-specific Review guidance, specific rebuildable cache candidates such as `DXCache` and `pip\Cache`, conservative game/mod-manager labels such as OptiFine/CurseForge/Vortex, Cloud sync data and Credential data labels, and `Preview file` action make unfamiliar rows easier to triage.
5. Retest the Quarantine Readiness UI with a real scan and confirm typed Quarantine root destinations, broad-parent protected descendant blockers, readable relative examples, draft/readiness wording, and real-profile execution blockers are understandable.
6. Defer real-profile Quarantine execution and broad WPF Undo Quarantine that restores discovered manifests until scan review, preview semantics, confirmation semantics, restore rules, manifest write order, and failure handling are trustworthy.
7. Revisit .NET 10 before packaging or long-term distribution.

## Completed packets

### 2026-05-30: Run Full Local MVP Preflight After Help Cues

Status: completed

Evidence:

- Review Grid Mode Status Help Cue and Quarantine Preview Status Help Cue touched WPF layout, dynamic help-text wiring, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the focused app-test runs before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the recent help-cue packets were pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against the compact `?` help cues, collapsed panel header summaries/state styling, styled inline Quarantine Preview readiness, styled Review Grid Mode Status, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Quarantine Preview Status Help Cue

Status: completed

Evidence:

- User said a little question mark in a circle that can be hovered over for the tooltip would be better than relying on hidden text hover alone.
- Inline Quarantine Preview readiness/status already mirrored dynamic preview state into tooltip/help text and named neutral/success/warning/error state.
- The remaining gap was discoverability for a safety-critical dry-run/approval-boundary tooltip.

Implementation:

- Added a visible circular `?` help cue beside inline Quarantine Preview readiness/status.
- Mirrored the dynamic tooltip and automation help text onto the help cue from the existing status update path.
- Added WPF smoke assertions for the cue automation name, cue tooltip, and cue automation help text across the existing empty, needs-preview, invalid-root, ready, stale, blocked, fixture-executed, and undo status states.
- Did not change scan behavior, Quarantine Preview eligibility, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated inline Quarantine Preview `?` help-cue prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-preview-inline-status.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/features/2026-05-30-quarantine-preview-status-help-text.md`
- `docs/features/2026-05-30-status-state-help-text.md`
- `docs/features/2026-05-30-quarantine-preview-status-help-cue.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the inline preview status `?` cue is noticeable without crowding the Quarantine Shortlist area.

Rejected ideas buffer:

- Do not add a popup, modal, or larger preview badge unless the compact cue and styled inline status are still insufficient after manual fixture review.

### 2026-05-30: Add Review Grid Mode Status Help Cue

Status: completed

Evidence:

- User said a little question mark in a circle that can be hovered over for the tooltip would be better than relying on hidden text hover alone.
- Review Grid Mode Status already mirrored dynamic grid-mode and status-state wording into tooltip/help text.
- The gap was discoverability, not the underlying safety wording.

Implementation:

- Added a visible circular `?` help cue beside Review Grid Mode Status.
- Mirrored the dynamic tooltip and automation help text onto the help cue.
- Added WPF smoke assertions for the cue automation name, cue tooltip, and cue automation help text across the existing startup, scan-row, stale scan-row, current-session quarantined, returned scan-row, and empty moved-entry states.
- Did not change scan behavior, grid switching, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated Review Grid Mode Status `?` help-cue prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `docs/features/2026-05-30-review-grid-mode-status-help-text.md`
- `docs/features/2026-05-30-status-state-help-text.md`
- `docs/features/2026-05-30-review-grid-mode-status-help-cue.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the `?` cue is noticeable without crowding the main-grid area.

Rejected ideas buffer:

- This supersedes the earlier deferred idea to wait on a Review Grid Mode Status help icon because the user later preferred visible `?` cues for hidden safety tooltips.
- Do not add a popup, modal, or larger grid-mode badge unless the compact cue is still insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Safety Cue

Status: completed

Evidence:

- Review Shortlist Safety Mix Help Cue touched WPF layout, dynamic help text wiring, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the narrow app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the help-cue packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against the three `?` help cues, collapsed header summaries/state styling, styled inline Quarantine Preview readiness, styled Review Grid Mode Status, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Review Shortlist Safety Mix Help Cue

Status: completed

Evidence:

- User said a little question mark in a circle that can be hovered over for the tooltip would be better than relying on hidden text hover alone.
- Review Shortlist Safety Mix already mirrored dynamic summary wording into tooltip/help text.
- The gap was discoverability, not the underlying safety wording.

Implementation:

- Added a visible circular `?` help cue beside Review Shortlist Safety Mix.
- Mirrored the dynamic tooltip and automation help text onto the help cue.
- Added WPF smoke assertions for the cue automation name, cue tooltip, and cue automation help text across the existing empty, populated, and empty-after-removal shortlist states.
- Did not change scan behavior, Review Shortlist membership, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` initially failed when launched without quoting the path with spaces; reran as `& 'D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe'` and it passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated Review Mix / Matched Review Mix / Review Shortlist Safety Mix `?` help-cue prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-shortlist-safety-mix.md`
- `docs/features/2026-05-30-review-shortlist-safety-mix-help-text.md`
- `docs/features/2026-05-30-review-shortlist-safety-mix-help-cue.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the `?` cue is noticeable without crowding the Review Shortlist toolbar area.

Rejected ideas buffer:

- Do not add a popup, modal, or larger Safety Mix table unless the compact cue is still insufficient.

### 2026-05-30: Add Review Mix Help Cues

Status: completed

Evidence:

- User said visible circular `?` help cues would be better than relying on hidden hover over Review Mix and Matched Review Mix text.
- Review Mix Help Text already mirrored dynamic summary wording into tooltip/help text.
- The gap was discoverability, not the underlying safety wording.

Implementation:

- Added visible circular `?` help cues beside Review Mix and Matched Review Mix.
- Mirrored the dynamic tooltip and automation help text onto both help cues.
- Added WPF smoke assertions for help-cue automation names, cue tooltips, and cue automation help text across the existing startup, completed-scan, descendant-focus, and prefixed-search states.
- Did not change scan behavior, filters, search, Review Shortlist, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated Review Mix / Matched Review Mix `?` help-cue prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `docs/features/2026-05-29-matched-review-mix.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-review-mix-help-text.md`
- `docs/features/2026-05-30-review-mix-help-cues.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the `?` cues are noticeable without crowding the dense review surface.

Rejected ideas buffer:

- Do not add a popup, modal, or extra help row for these summary lines unless the compact cues are still insufficient.

### 2026-05-30: Add Review Mix Help Text

Status: completed

Evidence:

- Review Mix and Matched Review Mix are read-only context readouts near the Safety Summary and review toolbar.
- Review Shortlist Safety Mix already mirrored dynamic wording into tooltip/help text with explicit non-approval boundaries.
- The top-level Review Mix readouts had visible non-approval wording but did not expose the same boundary through tooltip/help text.

Implementation:

- Added static startup tooltip and automation help text to Review Mix and Matched Review Mix.
- Added dynamic setters so completed-scan, descendant-focus, and prefixed-search summaries are mirrored into tooltip/help text.
- Help text says the readouts are read-only whole-scan or active-review-lens context and do not rescan, modify files, prove storage savings, or approve cleanup.
- Added WPF smoke assertions for startup, completed scan, descendant focus, and prefixed-search states.
- Did not change scan behavior, filters, search, Review Shortlist, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated Review Mix / Matched Review Mix tooltip/help-text prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `docs/features/2026-05-29-matched-review-mix.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-review-mix-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture or real-profile review, confirm whether Review Mix / Matched Review Mix tooltip/help text is enough or whether either dense line needs a visible help affordance.

Rejected ideas buffer:

- Do not redesign Review Mix layout until manual review shows the compact text line is too dense.

### 2026-05-30: Add Review Shortlist Safety Mix Help Text

Status: completed

Evidence:

- Review Shortlist Safety Mix is a safety-context readout before Quarantine Preview.
- Neighboring Review Shortlist, preview, and status controls already expose tooltip and automation help text.
- Safety Mix visible text said it was review context and not cleanup approval, but that boundary was not available through tooltip/help text.

Implementation:

- Added static startup tooltip and automation help text to `ShortlistSafetyMixText`.
- Updated Review Shortlist Safety Mix updates so empty, populated, and empty-after-removal text is mirrored into tooltip/help text.
- Help text says the readout is read-only review context and does not rescan, modify files, prove Quarantine readiness, prove storage savings, or approve cleanup.
- Added WPF smoke assertions for empty, populated, and empty-after-removal states.
- Did not change scan behavior, Review Shortlist membership, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated Review Shortlist Safety Mix tooltip/help-text prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-review-shortlist-safety-mix.md`
- `docs/features/2026-05-30-review-shortlist-safety-mix-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture or real-profile review, confirm whether Safety Mix tooltip/help text is enough or whether the dense line should become compact chips/table.

Rejected ideas buffer:

- Do not redesign the Safety Mix layout until manual review shows the compact text line is too dense.

### 2026-05-30: Run Full Local MVP Preflight After Header Summary Labels

Status: completed

Evidence:

- Collapsed Header Summary Labels touched WPF header text, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the narrow app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the header-summary-label packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against panel-name collapsed headers, collapsed-header state styling/help text, styled inline Quarantine Preview readiness help text, styled Review Grid Mode Status help text, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Collapsed Header Summary Labels

Status: completed

Evidence:

- User agreed that a header panel summary while closed would be useful.
- The existing collapsed headers already exposed compact summaries, state styling, and state-naming tooltip/help text.
- The visible prefixes were sentence-style and less directly tied to the panel names than the surrounding UI.

Implementation:

- Changed the Safety Summary header prefix to `Safety Summary:`.
- Changed the Quarantine Shortlist header prefix to `Quarantine Shortlist:`.
- Aligned the initial Quarantine Shortlist XAML header with the dynamic runtime summary by including `undo unavailable`.
- Added WPF smoke assertions that both collapsed headers start with their visible panel names.
- Did not change scan behavior, Review Shortlist behavior, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed panel-name prefix checks for Safety Summary and Quarantine Shortlist collapsed headers without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-collapsed-header-state-help-text.md`
- `docs/features/2026-05-30-collapsed-header-summary-labels.md`
- `docs/features/2026-05-30-collapsed-panel-header-help-text.md`
- `docs/features/2026-05-30-quarantine-shortlist-header-styling.md`
- `docs/features/2026-05-30-safety-summary-header-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF header readability polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm whether the panel-name prefixes make the collapsed summaries easier to scan when both panels are collapsed.

Rejected ideas buffer:

- Do not add badges, help icons, or another visible row unless manual fixture review shows the panel-name prefixes and existing tooltip/help text are still insufficient.

### 2026-05-30: Add Status State Help Text

Status: completed

Evidence:

- Review Grid Mode Status and inline Quarantine Preview readiness/status already use visual state styling.
- Their tooltip/help text mirrored the visible status and safety boundary, but did not name the current semantic state.
- The previous collapsed-header state packet showed the value of avoiding color-only state cues.

Implementation:

- Added textual `Status state:` wording to Review Grid Mode Status tooltip/help text.
- Added textual `Status state:` wording to inline Quarantine Preview readiness/status tooltip/help text.
- Used safety-preserving labels: neutral, information, warning, success, and error.
- Kept the status lines compact; no new row, badge, modal, cleanup execution, restore behavior, persisted history, scan gate change, or real-profile file movement was added.
- Strengthened shared WPF smoke assertions so every exercised grid-mode and preview-status state must expose matching state text.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated state-naming status tooltip/help-text prompts without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/features/2026-05-29-review-grid-mode-status-styling.md`
- `docs/features/2026-05-30-quarantine-preview-status-help-text.md`
- `docs/features/2026-05-30-review-grid-mode-status-help-text.md`
- `docs/features/2026-05-30-status-state-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next visible fixture pass, confirm whether state-naming status tooltip/help text is enough or whether safety-critical status lines need a small always-visible help affordance.

Rejected ideas buffer:

- Do not add another visible help icon or popup until manual fixture review shows tooltip/help text is insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Header State Help

Status: completed

Evidence:

- Collapsed Header State Help Text touched WPF header help text, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the narrow app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the collapsed-header state help-text packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against collapsed header state styling and state-naming tooltip/help text, styled inline Quarantine Preview readiness tooltip/help text, styled Review Grid Mode Status tooltip/help text, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Collapsed Header State Help Text

Status: completed

Evidence:

- Safety Summary and Quarantine shortlist headers now use visual state styling, but tooltip/help text only mirrored the compact summaries and safety boundary.
- Color-only state cues are less useful for tooltip inspection and automation/screen-reader review.

Implementation:

- Added textual `Header state:` wording to Safety Summary header tooltip/help text.
- Added textual `Header state:` wording to Quarantine shortlist header tooltip/help text.
- Used safety-preserving labels: neutral, needs review, ready or completed, and current-session quarantined review.
- Kept the headers compact; no new row, badge, modal, cleanup execution, restore behavior, persisted history, scan gate change, or real-profile file movement was added.
- Added WPF smoke assertions for neutral startup state, Safety Summary needs-review state, Quarantine shortlist needs-review state, preview-ready/completed state, and current-session quarantined review state.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated state-naming header tooltip/help-text prompts without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-collapsed-header-state-help-text.md`
- `docs/features/2026-05-30-collapsed-panel-header-help-text.md`
- `docs/features/2026-05-30-quarantine-shortlist-header-styling.md`
- `docs/features/2026-05-30-safety-summary-header-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next visible fixture pass, confirm whether state-naming header tooltip/help text is enough or whether safety-critical panel headers still need a small always-visible help affordance.

Rejected ideas buffer:

- Do not add another visible help icon until manual fixture review shows the header tooltip/help text is insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Safety Header Styling

Status: completed

Evidence:

- Safety Summary Header Styling touched WPF header controls, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the narrow app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the Safety Summary header styling packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against collapsed header state styling, styled inline Quarantine Preview readiness tooltip/help text, styled Review Grid Mode Status tooltip/help text, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Safety Summary Header Styling

Status: completed

Evidence:

- User ran manual fixture checklist steps 1-11 successfully and agreed that a useful header panel summary while collapsed is desirable.
- Quarantine Shortlist Header Styling added state cues for one collapsed panel; Safety Summary still had compact text/help text but no neutral/warning state cue.
- `StorageScanSafetySummary.HasReviewWarnings` already defines whether scan safety signals need review.

Implementation:

- Added lightweight neutral/warning styling to `SafetySummaryHeaderText`.
- Waiting-for-scan uses neutral styling.
- A scanned summary with review warnings uses warning styling.
- Kept the header compact; no new row, badge, modal, cleanup execution, restore behavior, persisted history, scan gate change, or real-profile file movement was added.
- Added WPF smoke assertions for startup neutral styling and fixture post-scan warning styling.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated Safety Summary header state styling prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-collapsed-panel-header-help-text.md`
- `docs/features/2026-05-30-safety-summary-header-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF styling with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next visible fixture pass, confirm whether the styled Safety Summary header helps distinguish waiting versus needs-review without looking like cleanup approval.

Rejected ideas buffer:

- Do not add a success state to Safety Summary; the summary is not a safety guarantee or cleanup approval.

### 2026-05-28: Create Grill with Docs scaffold

Status: completed

What changed:

- Added the Grill with Docs documentation scaffold.
- Added a SkillOpt-inspired workflow note for evidence-driven, bounded documentation improvement.
- Added this progress log to preserve task evidence and rejected ideas across sessions.

Verification:

- Verified scaffold files and folders with `rg --files` plus a forced recursive listing for hidden `.codex/`.
- `git status --short` could not run because this folder is not currently a Git repository.

Docs updated:

- `AGENTS.md`
- `README-codex-grill-with-docs.md`
- `MANIFEST.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/domain/context-map.md`
- `docs/decisions/`
- `docs/features/`
- `docs/codex/`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0001-use-grill-with-docs-workflow.md`.

Open questions:

- What is this product/app?
- Who is the target user?
- What problem does it solve?
- What stack or framework will it use?
- What is the first feature or workflow?

Rejected ideas buffer:

- Do not write production code before the project summary and initial domain language are captured.

## Verification history

| Date | Check | Result | Notes |
|---|---|---|---|
| 2026-05-28 | Scaffold creation | passed | Verified visible scaffold files with `rg --files`; verified hidden `.codex/progress.md` with `Get-ChildItem -Force -Recurse`. |
| 2026-05-28 | Git status | not available | Folder is not currently a Git repository. |

### 2026-05-28: Capture initial project summary

Status: completed

Evidence:

- Product/app: local Windows cleanup app to trim unwanted bloat from `C:\Users`.
- Target user: project owner only.
- Problem: `C:` is a 250 GB Windows 11 operating-system partition, and `C:\Users` is using about 75 GB.
- Stack/framework: unknown.
- First feature/workflow: unknown.

Docs updated:

- `AGENTS.md`: added project-specific safety rules for read-only scanning, explicit confirmation, reversible cleanup, and conservative path handling.
- `docs/domain/context.md`: replaced template product summary and added initial domain concepts, business rules, lifecycle states, permissions assumptions, and deletion policy.
- `docs/domain/glossary.md`: replaced example terms with initial cleanup domain terms and forbidden synonyms.

ADRs:

- No new ADR added. The current decision is domain framing, not a durable architecture or persistence choice.

Open questions:

- What should count as unwanted bloat?
- Which paths are in scope and out of scope?
- What cleanup action should be available first?
- What stack should the app use?
- What is the first feature brief?

Rejected ideas buffer:

- Do not equate "large" with removable.
- Do not start with permanent deletion as the default first cleanup action.
- Do not treat all of `AppData` as safe bloat.

## Known risks

- Project commands are placeholders until the stack is known.
- First workflow is still unknown.
- Cleanup behavior has destructive potential and needs explicit safety decisions before implementation.

### 2026-05-28: Capture first Grill with Docs answers

Status: completed

Evidence:

- First workflow: read-only scan/report.
- Protected and sensitive folders should still be shown for inspection, with the app helping rate importance and whether deletion is advisable.
- Initial bloat categories: old downloads, temp folders, installer caches, app caches, duplicate files, old game files, Node/Python package caches, and Windows app leftovers.
- Safety constraint: cleanup should not break current apps, including Codex.
- Preferred eventual cleanup path: Quarantine on `D:` with an easy undo workflow.
- Preferred product shape: desktop app.

Docs updated:

- `AGENTS.md`: added first workflow, desktop preference, quarantine preference, and current-app safety rule.
- `docs/domain/context.md`: added Bloat Category, Importance Rating, Deletion Recommendation, Quarantine, Undo Quarantine, and rules for inspection and preserving current apps.
- `docs/domain/glossary.md`: added the new terms and clarified forbidden generic labels.
- `docs/features/2026-05-28-read-only-user-profile-scan.md`: created first draft feature brief.

ADRs:

- No new ADR yet. Stack and Quarantine architecture decisions are still open.

Open questions:

- What desktop stack should be used?
- Should the first scan target all of `C:\Users` or only the current user's profile folder?
- How deep should the first scan inspect folders by default?
- What Importance Rating labels should the UI use?
- What should the first workflow be called?

Rejected ideas buffer:

- Do not hide Protected Locations entirely; show them with conservative warnings.
- Do not build cleanup execution before the read-only scan/review workflow.
- Do not use permanent deletion as the first cleanup mechanism.

### 2026-05-28: Capture Storage Scan implementation choices

Status: completed

Evidence:

- Initial Cleanup Scope: `C:\Users\moxhe`.
- Scan mode: recursive scan of everything accessible within the Cleanup Scope.
- Importance Rating labels: `Likely safe`, `Caution`, `High risk`.
- First workflow name: Storage Scan.
- Desktop stack choice delegated to Codex.
- Local environment has .NET SDK 8.0.421 and 9.0.314, plus Windows Desktop runtimes for .NET 8 and .NET 9.
- Official .NET support policy shows .NET 10 is active LTS through November 14, 2028; .NET 8 and .NET 9 are supported through November 10, 2026.
- Microsoft Learn documents WPF as a .NET Windows desktop UI framework.

Decision:

- Use C# WPF for the desktop app.
- Initial target framework recommendation: .NET 8 for immediate local buildability.
- Revisit .NET 10 after installing its SDK.

Docs updated:

- `AGENTS.md`: replaced placeholder project commands with .NET/WPF commands.
- `docs/domain/context.md`: updated initial Cleanup Scope, recursive Storage Scan workflow, WPF product shape, and Importance Rating labels.
- `docs/domain/glossary.md`: added Storage Scan and WPF terms; updated Cleanup Scope and Importance Rating.
- `docs/features/2026-05-28-read-only-user-profile-scan.md`: updated validation gate and implementation plan.
- `docs/decisions/0002-use-dotnet-wpf-desktop-stack.md`: added stack ADR.

ADRs:

- Added `docs/decisions/0002-use-dotnet-wpf-desktop-stack.md`.

Rejected ideas buffer:

- Do not choose Electron/Tauri unless WPF proves inadequate.
- Do not implement a command-line-only product as the primary UX because the user prefers a desktop app.
- Do not scan all of `C:\Users` in the first version; start with `C:\Users\moxhe`.
- Do not install .NET 10 before the first implementation; use installed .NET 8 for now.

### 2026-05-28: Implement Storage Scan MVP packet

Status: completed

Evidence:

- Created `WindowsFileCleaner.sln`.
- Added `src/WindowsFileCleaner.Core` for read-only scanning and recommendation rules.
- Added `src/WindowsFileCleaner.App` as a WPF desktop app.
- Added `tests/WindowsFileCleaner.Tests` as a dependency-free console test harness.
- Initialized local Git repository and added origin `https://github.com/Smellybum1/Windows-File-Cleaner.git`.
- Remote currently has no refs from `git ls-remote`.

Implementation:

- Storage Scan recursively scans a Cleanup Scope without modifying files.
- Initial UI defaults to `C:\Users\moxhe`.
- Scanner refuses paths outside Cleanup Scope.
- Reparse points are not followed.
- Inaccessible paths are recorded instead of crashing the scan.
- Classifier assigns Bloat Categories, Importance Ratings, Deletion Recommendations, and evidence.
- WPF UI displays totals, top 2,000 largest paths, ratings, recommendations, categories, and evidence details.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- Committed checkpoint: `de7b854 Build initial Storage Scan MVP`.
- Pushed `main` to `https://github.com/Smellybum1/Windows-File-Cleaner.git`.

Docs updated:

- `AGENTS.md`
- `docs/features/2026-05-28-read-only-user-profile-scan.md`
- `.codex/progress.md`

ADRs:

- No new ADR. The implementation follows ADR 0002.

Open questions:

- User needs to run the desktop app and confirm the first real Storage Scan output.
- Quarantine path and undo workflow remain deferred.

Rejected ideas buffer:

- Do not run the real `C:\Users\moxhe` scan automatically from the background.
- Do not add deletion or quarantine buttons before the user reviews real scan output.

### 2026-05-28: Add Storage Scan review filters from real scan feedback

Status: completed

Evidence:

- User ran the WPF app against `C:\Users\moxhe`.
- Real scan completed successfully.
- Reported totals from screenshot:
  - Total size: 58.02 GB.
  - Folders: 37,740.
  - Files: 188,580.
  - Access issues: 3.
- The scan surfaced UX/classification issues:
  - Results table was too broad without filters.
  - Large container folders such as `AppData`, `Local`, `Roaming`, `Google`, `Chrome`, and `pip` showed too many `None` categories.
  - Cache subfolders such as `NVIDIA\DXCache` and Python cache paths need clearer but still conservative labeling.

Implementation:

- Added `StorageReviewFilter`, `StorageReviewEntry`, `StorageReviewSummary`, `StorageScanReview`, and `StorageScanReviewBuilder`.
- Added WPF filter buttons for All, Likely safe, Caution, High risk, and Quarantine candidates.
- Added filter counts and displayed-size summary.
- Lightened DataGrid row presentation.
- Added conservative categories for Profile container, AppData area, Browser data, and GPU shader cache.
- Improved Python `pip` cache recognition.
- Preserved read-only behavior; no cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-review-filters.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental review/filter improvement.

Open questions:

- User should rerun the app and confirm whether filters help.
- Need user feedback on which real rows should be explicit Protected Locations.

Rejected ideas buffer:

- Do not make AppData-derived cache rows "Likely safe" just because they are caches.
- Do not add cleanup buttons based only on the first successful scan.

### 2026-05-28: Add selected-folder child breakdown

Status: completed

Evidence:

- Real scan screenshot showed large container rows such as `moxhe`, `AppData`, `Local`, `pip`, and browser folders.
- The user originally asked for the app to show what is inside folders before cleanup decisions.

Implementation:

- Added `StorageChildSummaryEntry`.
- Added `StorageChildSummaryBuilder`.
- Updated the WPF detail pane with Evidence and Largest immediate children sections.
- Child breakdown shows immediate children with name, size, importance, recommendation, and categories.
- Files explicitly show they have no immediate children.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-folder-child-breakdown.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible review UI improvement.

Open questions:

- User should rerun the app and confirm whether the child breakdown makes large folders understandable.

Rejected ideas buffer:

- Do not replace the main flat table with a tree view until the detail-pane approach is tested.

### 2026-05-28: Add selected-path inspection actions

Status: completed

Evidence:

- Storage Scan now shows enough data that the user needs to inspect selected paths manually.
- Copying and opening selected paths supports review without cleanup execution.

Implementation:

- Added `PathInspectionPlan`.
- Added `PathInspectionPlanBuilder`.
- Added Copy path and Open in Explorer buttons to the selected-row detail pane.
- Folder paths open directly in Explorer.
- File paths ask Explorer to select the file.
- Status messages state that no files were modified.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-inspection-actions.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a read-only inspection UI improvement.

Open questions:

- User should verify Copy path and Open in Explorer on real scan results.

Rejected ideas buffer:

- Do not add destructive selected-row actions next to read-only inspection actions yet.

### 2026-05-28: Add Scan Report CSV export

Status: completed

Evidence:

- Storage Scan now produces enough review data that exporting filtered results will help manual analysis.
- Exporting a report keeps the app read-only with respect to scanned files.

Implementation:

- Added `StorageScanCsvExporter`.
- Added Export CSV button to the Storage Scan toolbar.
- Export uses the current Storage Review Filter.
- Export includes path, name, type, size, importance, recommendation, categories, modified time, evidence, and access issue.
- Export writes a user-selected CSV report file.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only report feature.

Open questions:

- User should verify CSV export after a real scan.

Rejected ideas buffer:

- Do not export file contents.
- Do not treat CSV export as scan history or restore metadata.

### 2026-05-28: Add Review Mix summary and fix flattened-size semantics

Status: completed

Evidence:

- Filtered recursive scan rows overlap because parent folders include child sizes.
- Adding flattened row sizes would overstate storage and imply false savings.

Implementation:

- Added Review Mix display to WPF.
- Changed `StorageReviewSummary` byte fields from summed totals to largest-row sizes.
- Updated filter summary wording to show largest displayed row.
- Added test coverage for largest quarantine candidate row.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental reporting semantics fix.

Open questions:

- User should confirm whether largest-row triage is useful in the real scan.

Rejected ideas buffer:

- Do not report summed bytes for flattened recursive rows.
- Do not call filtered row totals Storage Savings until selections are non-overlapping.

### 2026-05-28: Add Access issues review filter

Status: completed

Evidence:

- The first real scan reported 3 access issues.
- The app showed the count but did not provide a direct way to filter to those paths.

Implementation:

- Added `AccessIssues` to `StorageReviewFilter`.
- Added access issue count and largest-row summary fields to `StorageReviewSummary`.
- Added Access issues filter behavior in `StorageScanReview`.
- Added Access issues button and Review Mix count in WPF.
- Added fixture-style coverage with a synthetic inaccessible row.
- No elevated scan, permission change, cleanup execution, or retry workflow was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-access-issues-review-filter.md`
- `docs/features/2026-05-28-storage-scan-review-filters.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review filter.

Open questions:

- Should access issues remain informational only, or should a future separate workflow retry scanning as administrator?

Rejected ideas buffer:

- Do not request elevation automatically.
- Do not change permissions to resolve access issues.

### 2026-05-28: Add Bloat Category Filter

Status: completed

Evidence:

- The real scan surfaced many category-relevant rows such as app caches, Python package caches, GPU shader caches, browser data, protected locations, and access issues.
- Risk filters alone do not let the user inspect one category of evidence at a time.

Implementation:

- Added `StorageCategorySummaryEntry`.
- Added category summaries to `StorageScanReview`.
- Added combined filtering for `StorageReviewFilter` plus optional `BloatCategory`.
- Added a WPF Category dropdown below the filter buttons.
- CSV export now uses the current review filter and selected category filter.
- Added fixture coverage for category summaries and combined filtering.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-bloat-category-filter.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review feature.

Open questions:

- Should uncategorized rows get a `None` category filter option?
- Which category labels need stronger Protected Location behavior before any cleanup preview?

Rejected ideas buffer:

- Do not treat category matches as cleanup approval.
- Do not sum category rows as Storage Savings while recursive parent/child rows overlap.

### 2026-05-28: Add No category filter

Status: completed

Evidence:

- The first real scan showed many rows with `None` in the Categories column.
- The Bloat Category Filter packet left uncategorized rows as an open question.

Implementation:

- Added `StorageCategoryFilter` and `StorageCategoryFilterKind`.
- Added core filtering for No category rows.
- Added WPF No category dropdown option when uncategorized rows exist.
- Added fixture coverage for No category filtering and combined review/category filtering.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-no-category-filter.md`
- `docs/features/2026-05-28-bloat-category-filter.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review feature.

Open questions:

- Which uncategorized real-scan folders should become explicit Bloat Categories or Protected Locations?

Rejected ideas buffer:

- Do not turn uncategorized rows into `Unknown` categories just to make them filterable.
- Do not treat No category rows as safe or unsafe by default.

### 2026-05-28: Add Review Shortlist

Status: completed

Evidence:

- The user ran the WPF app and confirmed that Storage Scan completed against `C:\Users\moxhe`.
- Real scan output is large enough that the user needs a smaller follow-up set before any cleanup preview.

Implementation:

- Added `StorageReviewShortlist` as an in-memory, per-scan selection model.
- Added Add to shortlist, Remove, Clear shortlist, and Export shortlist controls to the WPF UI.
- Added a Shortlist column to visible Storage Scan rows.
- Review Shortlist export uses the existing CSV exporter and writes only a report.
- Starting a new Storage Scan clears the Review Shortlist.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-shortlist.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review feature.

Open questions:

- Should Review Shortlist remain per-scan only, or should a later persistence model be added after cleanup approval and manifest rules are designed?

Rejected ideas buffer:

- Do not treat Review Shortlist as Quarantine approval.
- Do not persist shortlisted paths until restore-manifest and cleanup-preview semantics are defined.

### 2026-05-28: Add Quarantine Preview

Status: completed

Evidence:

- The user requested a quarantine folder preferably on `D:` with easy undo.
- Review Shortlist now provides a smaller user-selected set to preview before any cleanup action.
- Safety docs require dry-run or preview behavior before file-moving code.

Implementation:

- Added `QuarantinePreview`, `QuarantinePreviewEntry`, `QuarantinePreviewDisposition`, and `QuarantinePreviewBuilder`.
- Added default preview root `D:\WindowsFileCleanerQuarantine`.
- Added WPF Preview quarantine control and preview summary display.
- Preview output shows included, blocked, and redundant rows, non-overlapping previewed bytes, and destination paths for included rows.
- Preview blocks high-risk, protected, inaccessible, reparse-point, outside-scope, and non-quarantine-candidate rows.
- Preview marks child rows redundant when a selected parent is already included.
- No folder creation, file move, deletion, manifest write, or cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only preview feature. Actual Quarantine execution and Restore Manifest design may need ADR coverage later.

Open questions:

- Should actual Quarantine execution use `D:\WindowsFileCleanerQuarantine` by default, or ask the user to choose a path?
- Should a later preview export a restore-manifest-shaped draft?

Rejected ideas buffer:

- Do not add an Execute, Move, Delete, or Quarantine button in this packet.
- Do not write a Restore Manifest during preview.
- Do not count overlapping parent/child rows as separate Storage Savings.

### 2026-05-28: Add Quarantine Preview CSV export

Status: completed

Evidence:

- Quarantine Preview now produces more detail than the bounded UI preview pane can comfortably show.
- The product still needs review/report workflows before any cleanup execution.

Implementation:

- Added `QuarantinePreviewCsvExporter`.
- Added WPF Export preview control enabled only after a current Quarantine Preview exists.
- Exported cleanup scope, quarantine root, disposition, source path, destination path, size, importance, recommendation, categories, reasons, evidence, access issue, and no-files-modified note.
- Changing the Review Shortlist clears the current preview and disables preview export.
- No folder creation, file move, deletion, manifest write, or cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only report feature. Restore Manifest and actual Quarantine execution remain future design work.

Open questions:

- Should a later preview export support JSON for machine-readable restore-manifest drafting?
- Should preview exports include a summary row or remain row-only CSV?

Rejected ideas buffer:

- Do not call the preview export a manifest.
- Do not export an executable cleanup script.
- Do not auto-save the preview without a user-selected report path.

### 2026-05-28: Add Storage Scan Safety Summary

Status: completed

Evidence:

- The first real scan surfaced access issues, high-risk/protected rows, and many rows requiring review.
- Future cleanup work needs the read-only safety boundary to stay visible.

Implementation:

- Added `StorageScanSafetySummary`.
- Added `StorageScanSafetySummaryBuilder`.
- Added WPF Safety Summary text under Review Mix.
- Summary displays Cleanup Scope/read-only notes, high-risk count, Protected Location count, access issue count, reparse point count, Quarantine candidate count, and Uncategorized Result count.
- Starting a new scan clears the previous safety summary.
- No permission change, cleanup execution, quarantine execution, manifest write, or rescan behavior was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only reporting feature.

Open questions:

- Should the summary be exportable as part of a future scan report bundle?
- Should summary notes become clickable filters later?

Rejected ideas buffer:

- Do not use the summary as a Cleanup Action gate yet.
- Do not make the summary a safety score.
- Do not hide access issues or protected rows from review.

### 2026-05-28: Add Safety Summary review shortcuts

Status: completed

Evidence:

- Storage Scan Safety Summary exposes warning counts that should be easy to inspect.
- Existing review and category filters already provide safe read-only lenses.

Implementation:

- Added `StorageScanSafetyShortcut`.
- Added `StorageScanSafetyShortcutFilter`.
- Added `StorageScanSafetyShortcutFilterBuilder`.
- Added WPF shortcut buttons for High risk, Protected, Access issues, Reparse points, Quarantine candidates, and No category.
- Shortcuts apply existing Storage Review Filter and Bloat Category Filter combinations.
- Shortcut buttons are disabled before scans, during scans, and for zero-count buckets.
- No cleanup execution, quarantine execution, manifest write, permission change, or rescan behavior was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-safety-summary-review-shortcuts.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review UI feature.

Open questions:

- Should shortcut clicks scroll to the first matching row or preserve selection if possible?
- Should shortcuts be visually grouped with warning severity later?

Rejected ideas buffer:

- Do not create cleanup-specific shortcut actions.
- Do not treat shortcuts as approvals.
- Do not add new category synonyms for Uncategorized Results.

### 2026-05-28: Add Restore Manifest Draft

Status: completed

Evidence:

- The user wants quarantine on `D:` with easy undo.
- Quarantine Preview now proves eligible destination paths, but future Undo Quarantine needs a versioned metadata contract before file-moving code exists.

Decision:

- Added ADR 0003: use JSON Restore Manifest with schema version `restore-manifest.v1`.

Implementation:

- Added `RestoreManifestDraft`.
- Added `RestoreManifestEntryDraft`.
- Added `RestoreManifestDraftBuilder`.
- Added `RestoreManifestDraftJsonSerializer`.
- Drafts include only included Quarantine Preview rows.
- Drafts exclude blocked and redundant preview rows.
- Draft JSON clearly identifies `isExecutedManifest` as false.
- No folder creation, file move, deletion, manifest file write, quarantine execution, or undo execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/decisions/0003-use-json-restore-manifest.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-restore-manifest-draft.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0003-use-json-restore-manifest.md`.

Open questions:

- What exact manifest file path should future Quarantine execution use?
- Should future executed manifests include hashes for files, and if so which hash algorithm?
- Should future Undo Quarantine restore timestamps and attributes?

Rejected ideas buffer:

- Do not write a draft file automatically.
- Do not use CSV as the executed restore manifest format.
- Do not include blocked or redundant preview rows as draft entries.

### 2026-05-28: Add Quarantine Confirmation Draft

Status: completed

Evidence:

- Quarantine Preview and Restore Manifest Draft now exist as read-only core artifacts.
- Actual Quarantine execution still needs an explicit confirmation gate before any file-moving code.

Implementation:

- Added `QuarantineConfirmationDraft`.
- Added `QuarantineConfirmationDraftBuilder`.
- Confirmation Draft records included counts and bytes, blocked/redundant counts, Restore Manifest Draft id, required future confirmation text, data blockers, and review notes.
- Builder checks preview and manifest agreement for Cleanup Scope, Quarantine root, schema version, entry count, bytes, destination paths, missing rows, duplicate rows, and stray manifest rows.
- `IsExecutionImplemented` remains false.
- No folder creation, file move, deletion, manifest file write, quarantine execution, or undo execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-confirmation-draft.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a read-only draft gate; actual execution flow should get ADR review when designed.

Open questions:

- What exact UI should ask for the confirmation phrase?
- Should future execution require hashes before moving files?

Rejected ideas buffer:

- Do not call this a cleanup plan.
- Do not make execution availability depend only on a boolean.
- Do not treat the confirmation phrase as sufficient without matching preview and manifest data.

### 2026-05-28: Add Quarantine Readiness UI

Status: completed

Evidence:

- Restore Manifest Draft and Quarantine Confirmation Draft were implemented in core but not visible in the WPF app.
- The progress log identified WPF display as the next safety-review step before any execution flow.

Implementation:

- WPF `Preview quarantine` now builds a Restore Manifest Draft and Quarantine Confirmation Draft in memory after building the Quarantine Preview.
- The detail pane now shows preview counts, Restore Manifest Draft id and entry summary, Quarantine Confirmation Draft id, required future confirmation text, execution status, readiness blocker count, and blocker details.
- Clearing scan, shortlist, or preview state clears both drafts.
- Quarantine Preview CSV export remains unchanged as a report, not a manifest.
- No folder creation, file move, deletion, manifest file write, quarantine execution, or undo execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/features/2026-05-28-quarantine-readiness-ui.md`
- `docs/features/2026-05-28-restore-manifest-draft.md`
- `docs/features/2026-05-28-quarantine-confirmation-draft.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is display-only UI for existing read-only draft artifacts.

Open questions:

- Should future execution use a modal confirmation dialog or dedicated review screen?
- Should Restore Manifest Draft JSON be exportable before execution?

Rejected ideas buffer:

- Do not add an execute button beside preview controls.
- Do not auto-save Restore Manifest Draft JSON from the UI.
- Do not hide readiness blockers behind a single pass/fail label.

### 2026-05-28: Add conservative app data classification

Status: completed

Evidence:

- Real scan evidence included large app/game rows that needed clearer but conservative labels.
- The app must avoid breaking current apps, game saves, app settings, and installed tools.

Implementation:

- Added `WindowsAppData`, `InstalledApplication`, and `GameData` Bloat Category values.
- Added classifier hints for `AppData\Local\Packages`, `AppData\Local\Programs`, Larian/Baldur's Gate, Stellaris, Paradox, and IronyMod-style paths.
- These categories are treated as Protected Location / High risk / Keep by default.
- Added display labels in WPF and CSV exporters.
- Added fixture coverage for Windows app package data, per-user installed app folders, and known game data.
- No cleanup execution, manifest writing, file move, deletion, or Quarantine behavior was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-conservative-app-data-classification.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental conservative classifier refinement.

Open questions:

- Which specific app or game folders should get cleanup exceptions later?
- Should some Windows app package subfolders be downgraded after manual review?

Rejected ideas buffer:

- Do not mark Windows app package data as likely safe by default.
- Do not classify game folders as removable just because they look old.
- Do not make `AppData\Local\Programs` a cleanup candidate.

### 2026-05-28: Add read-only safety regression

Status: completed

Evidence:

- The MVP boundary is read-only Storage Scan plus user-selected CSV reports.
- Future cleanup execution has not been approved or designed.

Implementation:

- Added `ProductionCodeDoesNotContainCleanupExecutionCalls` to the fixture test harness.
- The guard scans production C# files under `src/`.
- The guard fails on obvious file/directory move, delete, replace, write-bytes, set-attributes, and production directory-creation APIs.
- The guard allows exactly three `File.WriteAllText(dialog.FileName, ...)` calls for user-selected CSV report exports.
- Tests still create/delete fixture directories only inside the test harness.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/features/2026-05-28-read-only-safety-regression.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a verification guard for the existing read-only boundary.

Open questions:

- Should future approved Quarantine execution add a more precise allowlist for its own file-moving implementation?

Rejected ideas buffer:

- Do not block user-selected CSV report writes.
- Do not scan docs or tests for banned APIs; fixtures are allowed to create/delete test files.
- Do not treat this guard as a substitute for execution design if cleanup actions are added later.

### 2026-05-28: Add MVP runbook

Status: completed

Evidence:

- The repo had detailed Grill with Docs files but no app-focused root `README.md`.
- Next work requires a safe manual WPF rerun against `C:\Users\moxhe`.

Implementation:

- Added `README.md` for the current Windows File Cleaner MVP.
- Documented safety status, requirements, verification commands, WPF run command, default Cleanup Scope, manual MVP checklist, current workflow, and not-yet-implemented cleanup workflows.
- Kept the existing `README-codex-grill-with-docs.md` scaffold README.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-runbook.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a documentation runbook for the current MVP.

Open questions:

- Should packaging instructions be added after a release build exists?

Rejected ideas buffer:

- Do not document cleanup execution commands before they exist.
- Do not replace the Grill with Docs scaffold docs.
- Do not claim the app is safe to delete files.

### 2026-05-28: Add MVP readiness audit

Status: completed

Evidence:

- The user provided a screenshot showing Storage Scan completed against `C:\Users\moxhe` with 58.02 GB, 37,740 folders, 188,580 files, 3 access issues, and no file modifications.
- The repo already contained WPF, .NET 8, read-only Storage Scan, fixture tests, conservative classification, preview-only Quarantine artifacts, and safety regression evidence.

Implementation:

- Added `docs/features/2026-05-28-mvp-readiness-audit.md`.
- Mapped the original MVP requirements to repo evidence and status.
- Marked latest WPF UI retest as pending manual verification.
- Marked actual Quarantine execution and Undo Quarantine execution as out of MVP.
- Linked the audit from `README.md`.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an evidence audit, not a new durable architecture or persistence decision.

Open questions:

- Should the next packet focus on manual WPF retest feedback, or start designing actual Quarantine execution only after that retest?

Rejected ideas buffer:

- Do not mark the MVP fully complete until the latest WPF review flow is manually retested.
- Do not treat Quarantine Preview, Restore Manifest Draft, or Quarantine Confirmation Draft as cleanup approval.

### 2026-05-28: Add WPF fixture smoke launch

Status: completed

Evidence:

- The MVP readiness audit identified the latest WPF UI manual retest as the main remaining verification gap.
- Core fixture tests existed, but the WPF app always opened with the default real Cleanup Scope.

Implementation:

- Added `StorageScanLaunchOptions` for parsing `--scope`.
- Updated WPF startup to construct `MainWindow` explicitly with the parsed initial Cleanup Scope.
- Added a `MainWindow` constructor overload that fills the Cleanup Scope box without starting a scan.
- Added parser coverage to the console test harness.
- Added `tools/New-StorageScanSmokeFixture.ps1` to create a synthetic fixture under `.local\storage-scan-smoke-fixture`.
- Added `.local/` to `.gitignore`.
- Updated README, AGENTS, the domain context rule, and the MVP readiness audit.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1` passed and created the ignored fixture Cleanup Scope.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed and showed intended fixture writes.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/domain/context.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-fixture-smoke-launch.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible launch/testability improvement and does not change architecture, persistence, cleanup behavior, or security posture.

Open questions:

- Should the fixture include more real-scan-inspired app/game examples after the next manual pass?

Rejected ideas buffer:

- Do not auto-scan on startup.
- Do not create fixture files from production app code.
- Do not treat fixture WPF smoke testing as a substitute for one final real-profile retest.

### 2026-05-28: Add WPF shell smoke test

Status: completed

Evidence:

- Core fixture tests already covered scan/review/preview logic.
- WPF fixture launch support already covered argument parsing.
- The remaining automated gap was proving the WPF shell consumes the launch Cleanup Scope without starting a scan.

Implementation:

- Added read-only `MainWindow` startup-state properties.
- Added `tests/WindowsFileCleaner.App.Tests` targeting `net8.0-windows` with `UseWPF`.
- Added smoke coverage for default Cleanup Scope, launch Cleanup Scope, idle startup state, enabled scan action, and disabled CSV export before scan.
- Added the app test project to `WindowsFileCleaner.sln`.
- Updated README, AGENTS, the MVP readiness audit, and this progress log.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed after restore.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-shell-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a testability improvement and does not change architecture, persistence, security posture, or cleanup behavior.

Open questions:

- Should future UI automation cover actual button/filter interactions after the manual retest?

Rejected ideas buffer:

- Do not treat WPF shell construction as proof of the full interaction flow.
- Do not add visible GUI automation dependencies before the manual fixture smoke pass identifies a real need.

### 2026-05-28: Add WPF fixture scan smoke test

Status: completed

Evidence:

- Core tests already covered scanner and review logic.
- WPF shell smoke tests covered construction and launch-scope wiring.
- The next evidence gap was proving a fixture scan updates WPF state without touching real profile data.

Implementation:

- Refactored the Scan button path to call `RunStorageScanForCurrentScopeAsync`.
- Added read-only WPF state properties for smoke-test assertions.
- Added `WindowsFileCleaner.App.Tests` coverage that creates a synthetic fixture, runs Storage Scan through `MainWindow`, and asserts visible status, summary, filter, row, rating, recommendation, and category state.
- Verified fixture marker files remain after the scan.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-fixture-scan-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a testability improvement and does not change architecture, persistence, security posture, or cleanup behavior.

Open questions:

- Should future WPF automation cover Review Shortlist and Quarantine Preview interactions?

Rejected ideas buffer:

- Do not treat WPF fixture scan state as proof of visible layout quality.
- Do not automate real-profile scans in the test harness.

### 2026-05-28: Add WPF review interaction smoke test

Status: completed

Evidence:

- README manual MVP checklist asks for filters, Review Shortlist, and Quarantine Preview to be checked before relying on the app against real profile data.
- Core tests already covered review and preview logic.
- WPF fixture scan tests covered scan state but not review-control interactions.

Implementation:

- Added WPF command methods for Storage Review Filters, Bloat Category Filters, Safety Summary review shortcuts, displayed-row selection, Review Shortlist changes, and Quarantine Preview generation.
- Added `WindowsFileCleaner.App.Tests` coverage that runs a synthetic fixture scan through `MainWindow`, applies review filters and safety shortcuts, shortlists a likely-safe candidate, creates a Quarantine Preview, checks Restore Manifest Draft and Quarantine Confirmation Draft text, and verifies fixture files remain.
- Kept export dialogs manual and no cleanup execution was added.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-review-interaction-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is testability and smoke coverage for existing read-only UI behavior, not a durable architecture, persistence, security, deployment, or cleanup-execution decision.

Open questions:

- Should visible GUI automation or screenshot verification be added after the manual fixture pass?

Rejected ideas buffer:

- Do not automate file export dialogs in this packet.
- Do not treat WPF review interaction state as proof of visible layout quality.
- Do not automate real-profile scans in the test harness.

### 2026-05-28: Polish WPF review toolbar layout

Status: completed

Evidence:

- README and MVP audit still identify the visible fixture UI pass as the next manual verification step.
- The review toolbar previously used fixed grid columns and a horizontal action stack that could crowd as labels and counts grow.
- WPF app smoke tests can verify the intended wrapping toolbar structure without launching a visible desktop window.

Implementation:

- Replaced the fixed review toolbar grid with two named `WrapPanel` toolbars.
- Kept Filter Summary as its own wrapping line between filters and action controls.
- Added a small read-only WPF layout property for smoke-test assertions.
- Added `WindowsFileCleaner.App.Tests` coverage that verifies the review controls use wrapping toolbars.
- No scanner, classifier, cleanup, quarantine execution, or manifest-writing behavior was changed.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-review-toolbar-layout-polish.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible UI layout polish and does not change architecture, persistence, security, deployment, or cleanup behavior.

Open questions:

- Should the entire app later get a broader visual design pass after the real-profile retest?

Rejected ideas buffer:

- Do not introduce a new UI framework or dependency for this polish.
- Do not treat wrapping layout structure as proof of visual quality.

### 2026-05-28: Add MVP preflight script

Status: completed

Evidence:

- README verification commands existed individually, but the full pre-real-scan sequence was easy to run incompletely.
- The fixture generator already supported `-WhatIf`.
- Progress history repeatedly used the same restore, build, core test, app test, and fixture dry-run sequence.

Implementation:

- Added `tools/Invoke-MvpPreflight.ps1`.
- Preflight runs restore, build, core tests, WPF app tests, the fixture generator in `-WhatIf` mode, and `git diff --check`.
- Preflight prints the next fixture review launcher command.
- Added `-SkipRestore`, `-SkipFixtureWhatIf`, and `-SkipDiffCheck` switches for focused local loops.
- No real profile scan, WPF launch, fixture creation, cleanup execution, quarantine execution, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a local verification wrapper around existing commands and does not change architecture, persistence, security, deployment, or cleanup behavior.

Open questions:

- Should the app later expose a built-in diagnostics/preflight screen?

Rejected ideas buffer:

- Do not make preflight launch the WPF app or scan `C:\Users\moxhe`.
- Do not create fixture files by default from preflight.
- Do not hide individual commands from README; keep them visible for troubleshooting.

### 2026-05-28: Add MVP fixture review launcher

Status: completed

Evidence:

- Progress log identified the manual fixture UI pass as the next recommended work.
- Preflight and fixture scripts existed, but the user still had to run multiple commands and copy the fixture launch command manually.
- WPF launch-scope support only pre-fills the Cleanup Scope and does not auto-scan.

Implementation:

- Added `tools/Start-MvpFixtureReview.ps1`.
- The launcher runs MVP preflight by default, creates the synthetic fixture Cleanup Scope inside the repo, and launches the WPF app with that fixture scope.
- The launcher prints that the app will not auto-scan and that the user must click `Scan`.
- Added `-SkipPreflight`, `-SkipLaunch`, and `-WhatIf` support for safe verification and focused local loops.
- No real profile scan, auto-scan, cleanup execution, quarantine execution, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf` passed and showed intended preflight, fixture creation, and WPF launch actions without executing them.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-fixture-review-launcher.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a local workflow launcher around existing preflight, fixture, and WPF launch paths; it does not change architecture, persistence, security, deployment, or cleanup behavior.

Open questions:

- Should the app later include an in-app fixture/demo mode for visual QA?

Rejected ideas buffer:

- Do not make the launcher scan automatically.
- Do not point the launcher at `C:\Users\moxhe`.
- Do not make fixture creation happen from production app code.

### 2026-05-28: Add Selected Path Review Guidance

Status: completed

Evidence:

- The user-tested real scan showed very large rows where `Caution` plus passive evidence was not enough to guide review.
- Existing selected-row detail already showed evidence and Child Breakdown, so the smallest improvement was next-step wording for the selected path.

Implementation:

- Added `SelectedPathReviewGuidance` and `SelectedPathReviewGuidanceBuilder`.
- Added a Review guidance section to the WPF selected-row detail pane.
- Guidance covers access issues, reparse points, profile containers, protected/high-risk rows, quarantine candidates, cache/package rows, Uncategorized Results, and generic evidence review.
- Added core guidance coverage and WPF smoke coverage that selected Quarantine candidates show guidance before shortlisting.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-review-guidance.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible selected-row review improvement and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should guidance become user-customizable after actual cleanup execution exists?

Rejected ideas buffer:

- Do not turn selected-row guidance into cleanup approval language.
- Do not hide High risk rows; explain the safest next review step.
- Do not use Selected Path Review Guidance as a cleanup executor.

### 2026-05-28: Add Cleanup Scope Safety Note

Status: completed

Evidence:

- The project requires fixture-based verification before real-profile scans.
- The app previously showed only the Cleanup Scope path field, so fixture-vs-real scope context was documented but not visible in the WPF shell.

Implementation:

- Added `CleanupScopeSafetyNote` and `CleanupScopeSafetyNoteBuilder`.
- Added a WPF note below the Cleanup Scope controls.
- The note distinguishes Fixture Cleanup Scope, Real Profile Cleanup Scope, Custom Cleanup Scope, Choose Cleanup Scope, and Check Cleanup Scope.
- Added core tests for real-profile, fixture, custom, and blank scope notes.
- Added WPF smoke assertions for default real-profile startup and fixture launch startup notes.
- No scan blocking, preflight execution, fixture creation, cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-safety-note.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only UI reminder and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a future release record the last successful preflight timestamp for local-only display?

Rejected ideas buffer:

- Do not treat the note as proof that preflight ran.
- Do not make the note run shell commands, create fixtures, or block scanning.
- Do not add a modal pre-scan gate before the visible fixture workflow is manually tested.

### 2026-05-28: Add Storage Review Search

Status: completed

Evidence:

- The real scan scale was 188,580 files and 37,740 folders.
- Existing filters helped broad triage, but finding a specific app, tool, cache, or game path still required scrolling or CSV export.

Implementation:

- Added `StorageReviewSearch`.
- Extended `StorageScanReview` filtering to combine Storage Review Filter, Bloat Category Filter, and Storage Review Search.
- Added a WPF Search field and Clear search action.
- Search matches path, name, category, Importance Rating, Deletion Recommendation, evidence, and access issue text.
- Search normalizes whitespace, hyphens, and underscores so spaced search terms can match enum-style labels such as `HighRisk` and `PythonPackageCache`.
- Search resets after each new Storage Scan.
- No filesystem rescan, persistence, cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible in-memory review feature and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should search later support explicit field prefixes such as `path:`, `category:`, or `rating:`?

Rejected ideas buffer:

- Do not make search rescan, watch the filesystem, or search outside completed scan results.
- Do not persist search history.
- Do not use search results as cleanup approval.

### 2026-05-28: Add Storage Review Display Limit wording

Status: completed

Evidence:

- The user reran the WPF app against `C:\Users\moxhe` and provided a screenshot showing the scan completed.
- The real scan contained 188,580 files, 37,740 folders, and 3 access issues.
- The WPF grid caps visible rows at 2,000, but prior wording did not clearly distinguish displayed rows from matched review rows.

Implementation:

- Split active matched review rows from WPF displayed rows.
- Updated completed-scan status to say `Showing 2,000 of ... paths` when the display cap is reached.
- Updated Filter Summary to say `2,000 shown of ... matched`, label largest-row triage as the largest matched row, and suggest narrowing with filters/search.
- Added a WPF smoke test with a large synthetic fixture that exceeds the 2,000-row display limit.
- No scanner traversal, classification, export, cleanup execution, Quarantine execution, Undo Quarantine, or manifest-writing behavior was changed.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-display-limit.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF review wording and smoke coverage; it does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a future UI add paging or a virtualized tree/grid for all matched rows?

Rejected ideas buffer:

- Do not lower scan depth or skip files to fit the grid.
- Do not call displayed rows the complete scan result.
- Do not add cleanup execution while improving review wording.

### 2026-05-28: Add Shortlist shown action

Status: completed

Evidence:

- The real scan result set is large enough that adding one row at a time to Review Shortlist is cumbersome.
- Storage Review Display Limit wording now makes the visible grid boundary explicit, so bulk shortlisting can safely target only visible rows.

Implementation:

- Added `StorageReviewShortlist.AddMany` for unique bulk additions.
- Added a WPF `Shortlist shown` action that adds only currently displayed rows to Review Shortlist.
- Status text states that Review Shortlist is not cleanup approval and that no files were modified.
- `Shortlist shown` disables when all currently displayed rows are already shortlisted.
- No hidden matched rows, cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors during final preflight.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-shortlist-shown-review-rows.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a later review UI support selecting a specific subset of displayed rows before bulk shortlisting?

Rejected ideas buffer:

- Do not add all matched rows when the grid is capped.
- Do not treat Review Shortlist as cleanup approval.
- Do not add cleanup execution from the shortlist toolbar.

### 2026-05-28: Add Remove shown shortlist action

Status: completed

Evidence:

- `Shortlist shown` makes visible-window bulk review easier, but correcting a broad visible-window shortlist previously required clearing the whole Review Shortlist or removing one selected row at a time.
- Review Shortlist remains an in-memory review aid, so visible-window removal is reversible and does not touch scanned files.

Implementation:

- Added `StorageReviewShortlist.RemoveMany` for unique bulk removals.
- Added a WPF `Remove shown` action that removes only currently displayed rows from Review Shortlist.
- `Remove shown` disables when no currently displayed rows are shortlisted.
- Updated WPF smoke coverage for add shown, remove shown, selected-row add, preview generation, and read-only status text.
- No hidden matched rows, cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors during final preflight.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-shortlist-shown-review-rows.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a later review UI support selecting a specific subset of displayed rows before bulk shortlisting/removal?

Rejected ideas buffer:

- Do not add or remove all matched rows when the grid is capped.
- Do not treat Review Shortlist as cleanup approval.
- Do not add cleanup execution from the shortlist toolbar.

### 2026-05-28: Align Scan Report Export with Storage Review Search

Status: completed

Evidence:

- Storage Review Search narrows the WPF grid and filter summary, but the main Scan Report Export path still used only Storage Review Filter and Bloat Category Filter.
- A searched review should export the same active review lens, while still exporting all matched rows rather than only the 2,000 displayed rows.

Implementation:

- Added a WPF export-row helper that applies Storage Review Filter, Bloat Category Filter, and Storage Review Search.
- Updated Export CSV to use that helper.
- Added WPF smoke coverage that searched export rows honor the active `pip` search.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors during final preflight.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only report alignment fix and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should export filenames include a sanitized search segment later?

Rejected ideas buffer:

- Do not export only displayed rows; export all rows matched by the active review lens.
- Do not treat exported reports as cleanup history or restore manifests.

### 2026-05-28: Add searched Scan Report Export filenames

Status: completed

Evidence:

- Scan Report Export now honors Storage Review Search, but the suggested filename still did not show when a search was active.
- Report filenames should help the user distinguish searched CSV exports without becoming persisted scan history.

Implementation:

- Added a sanitized `search-...` segment to the main Scan Report Export filename when Storage Review Search is active.
- Search filename segments use lowercase letters/digits separated by hyphens and are capped in length.
- Added WPF smoke coverage for searched and cleared-search export filename behavior.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors during final preflight.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only report naming behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should exports include all rows, active filter rows, or both as separate options?

Rejected ideas buffer:

- Do not treat report filenames as scan history.
- Do not include raw user paths in generated filenames.

### 2026-05-28: Add hierarchy context to Scan Report Export

Status: completed

Evidence:

- Recursive Storage Scan rows are flattened in the grid and CSV report.
- Full paths are present, but spreadsheet review is easier when each row also carries its immediate parent and depth.

Implementation:

- Added `Parent path` and `Depth` columns to `StorageScanCsvExporter`.
- Kept root-level parent path blank so the export does not invent a parent outside the reviewed hierarchy.
- Extended CSV fixture coverage for the new hierarchy columns.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only report schema improvement and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a future export include a separate cleanup-scope-relative path column?

Rejected ideas buffer:

- Do not make Scan Report Export a persisted scan history feature.
- Do not include cleanup approval or restore-manifest data in this CSV.

### 2026-05-28: Add Storage Review Size Note

Status: completed

Evidence:

- The real scan screenshot showed large parent and child folders together in the flattened review grid.
- Review Mix and filter summaries avoid summing rows internally, but the WPF review surface did not state the recursive size rule near the grid.

Implementation:

- Added a visible Storage Review Size Note below the filter summary.
- The note says folder sizes include children, parent/child rows can overlap, and row sizes are triage clues rather than Storage Savings.
- Added WPF smoke coverage for the note text.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-size-note.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible UI wording and smoke coverage.

Open questions:

- Is the note visually readable in the next manual fixture UI pass?

Rejected ideas buffer:

- Do not hide recursive parent or child rows to avoid overlap confusion.
- Do not label row sizes as cleanup savings before Quarantine Preview or a future explicit cleanup plan computes non-overlapping bytes.

### 2026-05-28: Add Storage Review field-prefix search

Status: completed

Evidence:

- The real scan scale makes broad text search useful but sometimes noisy.
- The Storage Review Search feature brief left `path:`, `category:`, and `rating:` prefixes as a deferred question.

Implementation:

- Added `StorageReviewSearchField` and prefix parsing in `StorageReviewSearch`.
- Supported `path:`, `name:`, `category:`/`cat:`, `rating:`/`importance:`, `recommendation:`/`rec:`, `evidence:`, and `issue:`/`access:`.
- Updated `StorageScanReview` to restrict matching to the parsed field when a recognized prefix is used.
- Preserved unprefixed broad search and treated unrecognized prefixes as literal broad search text.
- Added WPF search tooltip examples and smoke coverage for prefixed search summary/export filename behavior.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible in-memory search behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Are the tooltip examples discoverable enough during the next manual fixture UI pass?

Rejected ideas buffer:

- Do not make prefixed search rescan the filesystem or persist search history.
- Do not use prefixed search matches as cleanup approval.

### 2026-05-28: Add Large old file classification

Status: completed

Evidence:

- The real scan screenshot included large files with little category context.
- `No category` remains useful, but old multi-gigabyte files deserve a conservative triage label when last-modified evidence is stale.

Implementation:

- Added `BloatCategory.LargeOldFile`.
- Passed file size into `CleanupCandidateClassifier`.
- Labeled files at least 1 GB and older than 90 days as `Large old file`.
- Kept unknown large old files as `Caution` / `Inspect`; size and age alone do not approve cleanup.
- Kept large old files with stronger cleanup evidence, such as old Downloads or installer evidence, on the existing likely-safe/quarantine-candidate path.
- Added display/export labels and classifier coverage.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-read-only-user-profile-scan.md`
- `docs/features/2026-05-28-large-old-file-classification.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible classifier triage behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does the real scan now surface helpful `Large old file` rows?

Rejected ideas buffer:

- Do not classify directories as Large old file from recursive size because parent and child rows overlap.
- Do not treat large old files as cleanup approval without stronger category evidence.

### 2026-05-28: Add Storage Entry Type Filter

Status: completed

Evidence:

- Recursive scan review mixes folders and files in one flattened grid.
- The user needs to inspect both container folders and individual files, especially after adding Large old file classification.

Implementation:

- Added `StorageEntryTypeFilter` with `All`, `Files`, and `Folders`.
- Added core filtering that combines entry type with review filter, Bloat Category Filter, and Storage Review Search.
- Added a WPF Type filter combo box.
- Included active type in Filter Summary and Scan Report Export filenames.
- Added core and WPF smoke coverage.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `docs/features/2026-05-28-storage-entry-type-filter.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review filtering and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does Files plus Large old file help the next real scan review?

Rejected ideas buffer:

- Do not treat file-only rows as automatically safer than folders.
- Do not hide the underlying completed scan rows; type is only an active review lens.

### 2026-05-28: Add Review View Reset

Status: completed

Evidence:

- Review filters now include rating, category, type, search, and Safety Summary shortcuts.
- Stacked review lenses can make it cumbersome to return to full review without losing Review Shortlist state.

Implementation:

- Added `Reset view` to the WPF review toolbar.
- Added `ResetReviewView` to restore All, All categories, All types, and empty search.
- Kept Review Shortlist intact during reset.
- Added reset-enabled state handling and WPF smoke coverage.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-view-reset.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Is Reset view discoverable enough during manual fixture review?

Rejected ideas buffer:

- Do not combine Reset view with Clear shortlist.
- Do not treat reset as a rescan or cleanup action.

### 2026-05-28: Add Selected Path Hierarchy Context

Status: completed

Evidence:

- Real scan output included deeply nested cache rows with short names such as one-letter folders and hash fragments.
- Those rows are hard to interpret from `Name` alone, even though the selected-row full path exists in the detail pane.

Implementation:

- Added `StorageEntryRow.ParentLocation`.
- Added a `Parent` column to the WPF Storage Scan grid.
- Added selected-row parent path, hierarchy depth, and modified-time context to the detail pane.
- Added WPF smoke coverage for parent/depth context.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-hierarchy-context.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does the `Parent` column make the next real scan easier to review without overcrowding the table?

Rejected ideas buffer:

- Do not classify short or deeply nested names as safe solely from their hierarchy context.

### 2026-05-28: Add Selected File Content Preview

Status: completed

Evidence:

- The original product request asked the app to show what is in files before rating importance or recommending cleanup.
- The current selected-row detail pane showed metadata, evidence, guidance, and child breakdowns, but could not preview selected file content.

Implementation:

- Added `SelectedFileContentPreview` and `SelectedFileContentPreviewBuilder`.
- Added an explicit WPF `Preview file` action for selected files.
- Added a `File preview` section to the selected-row detail pane.
- Kept preview bounded to a small text sample and avoided rendering binary-looking content as text.
- Added core coverage for text, binary, and folder preview outcomes.
- Added WPF smoke coverage for previewing a selected fixture text file.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-file-content-preview.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only review action and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does selected file preview help with real scan rows without exposing too much sensitive text in the review pane?

Rejected ideas buffer:

- Do not automatically preview file content when row selection changes.
- Do not treat previewed text as cleanup approval.

### 2026-05-28: Tune Cache-Specific Review Guidance

Status: completed

Evidence:

- The real scan showed large cache-heavy rows such as `NVIDIA\DXCache` and Python package cache paths.
- The classifier intentionally keeps these rows conservative, but generic guidance was less useful than category-specific review wording.

Implementation:

- Added GPU shader cache guidance that mentions rebuildability and temporary shader recompile delays.
- Added Python package cache guidance that protects active development tooling and Codex-related paths.
- Added Node package cache guidance that warns about active project dependencies.
- Added app cache guidance that prefers specific child rows over broad app folders.
- Added generic AppData guidance for rows with AppData evidence but no narrower cache category.
- No Bloat Category, Importance Rating, Deletion Recommendation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/features/2026-05-28-cache-specific-review-guidance.md`
- `docs/features/2026-05-28-selected-path-review-guidance.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible wording and triage guidance, not architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does cache-specific guidance make the next real scan easier to act on without making cache rows feel automatically safe?

Rejected ideas buffer:

- Do not promote GPU shader caches or package caches to likely-safe cleanup candidates solely because they are cache-like.

### 2026-05-28: Add Real Profile Scan Gate

Status: completed

Evidence:

- The project requires fixture-based verification before scanning real user files.
- The previous Cleanup Scope Safety Note reminded the user to run preflight and fixture review, but docs explicitly noted that the reminder was not proof and did not block scanning.

Implementation:

- Added `CleanupScopeScanGate` and `CleanupScopeScanGateBuilder`.
- Added a WPF acknowledgement checkbox for real-profile Cleanup Scopes.
- Disabled `Scan` for `C:\Users\moxhe` and child scopes until the acknowledgement is checked.
- Kept fixture Cleanup Scopes scan-ready without the real-profile acknowledgement.
- Enforced the gate in `RunStorageScanForCurrentScopeAsync`, not only through button state.
- Reset acknowledgement when the Cleanup Scope changes.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, preflight execution from WPF, fixture creation from WPF, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-safety-note.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-real-profile-scan-gate.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible local scan-start gate and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a future app version display the last successful preflight time if a trustworthy local marker exists?

Rejected ideas buffer:

- Do not run preflight or create fixture files from WPF.
- Do not persist real-profile scan acknowledgement yet.

### 2026-05-28: Add Selected Row Contents Context

Status: completed

Evidence:

- Large recursive scan rows need more context than size alone because parent and child row sizes overlap.
- The app already shows largest immediate children, but selected rows did not show contained file/folder counts.

Implementation:

- Added contained file and descendant folder counts to `StorageEntryRow`.
- Added contents context to the WPF selected-row detail pane.
- Added `Contained files` and `Contained folders` columns to Scan Report CSV export.
- Added WPF fixture coverage for selected-folder contents context.
- Added CSV coverage for exported contents counts.
- No scanner traversal, Bloat Category, Importance Rating, Deletion Recommendation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `docs/features/2026-05-28-selected-row-contents-context.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review/export context and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Do contents counts make the next real scan easier to review alongside largest-child breakdowns?

Rejected ideas buffer:

- Do not treat contained file/folder counts as recoverable storage savings.

### 2026-05-28: Add Cleanup Scope Root Classification

Status: completed

Evidence:

- The first real scan screenshot showed the top `C:\Users\moxhe` row as an ordinary caution/inspect row with no category.
- The scan root should be reviewed through child rows and should never look like a cleanup target.
- Path-shape inference is not enough because fixture and custom Cleanup Scope roots also need root-specific treatment.

Implementation:

- Added `BloatCategory.CleanupScopeRoot`.
- Passed explicit scan-root context from `StorageScanner` into `CleanupCandidateClassifier`.
- Classified the Cleanup Scope Root as `High risk` / `Keep` with `Cleanup scope root` and `Protected location` categories.
- Added Selected Path Review Guidance for scope-root rows.
- Added WPF, Scan Report CSV, and Quarantine Preview CSV labels for `Cleanup scope root`.
- Added core scanner coverage and WPF fixture coverage.
- No child classification, scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuilding.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-root-classification.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a conservative scan classification and review-guidance change, not a persistence, cleanup execution, security, deployment, or public API decision.

Open questions:

- Does the next real scan make the first row clearly read as the reviewed scope rather than bloat?

Rejected ideas buffer:

- Do not infer Cleanup Scope Root solely from `C:\Users` path shape.
- Do not allow the scope root into cleanup execution or quarantine.

### 2026-05-28: Add Quarantine Preview Protected Descendant Blocker

Status: completed

Evidence:

- Broad cache-like rows can contain protected descendants, including Codex runtime data.
- A broad parent row may otherwise appear to be a Quarantine candidate even though moving it would also move protected child data.
- Quarantine Preview already has the scanned child tree in memory, so it can block broad parent preview without touching the filesystem again.

Implementation:

- Added descendant blocker checks to `QuarantinePreviewBuilder`.
- Blocked parent preview when descendants are protected, high-risk, inaccessible, reparse points, or Cleanup Scope Roots.
- Added blocked reason text with example descendant paths and guidance to select narrower reviewed child rows.
- Added fixture coverage for a synthetic `.cache` parent containing protected `codex-runtimes` data.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or additional filesystem reads were added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview-protected-descendant-blocker.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a conservative read-only preview rule within existing Quarantine Preview behavior.

Open questions:

- In the next real scan, are broad cache-parent blocker reasons readable enough when paths are long?

Rejected ideas buffer:

- Do not rescan the filesystem from Quarantine Preview to discover blockers.
- Do not allow a broad parent to be included just because the parent row itself is a Quarantine candidate.

### 2026-05-28: Add WPF Proof for Quarantine Preview Protected Descendant Blocker

Status: completed

Evidence:

- Core tests proved the protected-descendant blocker, but the WPF workflow also needs to show the blocked reason and readiness blockers clearly.
- The app should prove the same boundary through selection, Review Shortlist, Preview quarantine, and the preview pane.

Implementation:

- Added a WPF smoke fixture with `.cache` containing protected `codex-runtimes` data.
- Added WPF smoke coverage that selects the broad `.cache` parent, adds it to Review Shortlist, runs Quarantine Preview, and asserts:
  - `0 included`
  - `1 blocked`
  - blocked preview readiness wording
  - `codex-runtimes` descendant evidence
  - narrower-row guidance
  - no-files-modified wording
- No production code, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or real user file access was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `docs/features/2026-05-28-quarantine-preview-protected-descendant-blocker.md`
- `docs/features/2026-05-28-wpf-review-interaction-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is WPF smoke coverage for an existing read-only preview rule.

Open questions:

- Should the visible preview pane separate row-specific blockers from confirmation-readiness blockers more clearly?

Rejected ideas buffer:

- Do not weaken confirmation readiness just to make the blocked-row count read as a single blocker.

### 2026-05-28: Improve Quarantine Preview Pane Readability

Status: completed

Evidence:

- The WPF preview pane showed confirmation-readiness blockers and row-level blocked reasons close together.
- A blocked broad parent can have both confirmation blockers and row-specific reasons, so the visible wording should keep those concepts separate.

Implementation:

- Renamed the readiness count line to `Confirmation readiness blockers`.
- Labeled readiness entries as `Confirmation blocker`.
- Added a `Preview rows:` section before row-level included/blocked/redundant entries.
- Labeled each row entry as `Preview row | Included`, `Preview row | Blocked`, or `Preview row | Redundant`.
- Updated WPF smoke assertions for both included-row preview and protected-descendant blocked preview.
- No Quarantine Preview eligibility rules, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or real user file access was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-quarantine-preview-pane-readability.md`
- `docs/features/2026-05-28-quarantine-preview-protected-descendant-blocker.md`
- `docs/features/2026-05-28-wpf-review-interaction-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF wording and smoke coverage.

Open questions:

- Does the real scan need a more structured preview table instead of plain text for long blocked paths?

Rejected ideas buffer:

- Do not change preview eligibility or confirmation readiness semantics as part of wording polish.

### 2026-05-28: Add Access Status Review Field

Status: completed

Evidence:

- The real scan showed access issues, and incomplete scan coverage should stay visible in the main review and exported reports.
- Access issue rows were countable/filterable, but normal review rows did not expose a simple readable/access-issue label.

Implementation:

- Added `StorageEntryRow.AccessStatus` with user-facing values `Readable` and `Access issue`.
- Added an `Access` column to the WPF Storage Scan grid.
- Added `Access: ...` to selected-row metadata.
- Added `Access status` columns to Scan Report CSV and Quarantine Preview CSV exports.
- Added core CSV coverage for readable and access issue statuses.
- Added WPF fixture coverage for readable row access status and selected-row detail metadata.
- No access retry, permission change, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuilding.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-access-status-review-field.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `docs/features/2026-05-28-quarantine-preview-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only UI/report context.

Open questions:

- In the next real scan, does the Access column make the three access issue rows easier to find?

Rejected ideas buffer:

- Do not add retry-as-admin or permission-changing behavior as part of access status display.

### 2026-05-28: Add Access Status Search

Status: completed

Evidence:

- The real scan screenshot showed readable rows and 3 access issue rows.
- Access Status is now visible in the grid, selected-row metadata, and CSV exports, so it should also participate in the existing search workflow.

Implementation:

- Added Access Status matching to broad Storage Review Search.
- Added Access Status matching to `access:` and `issue:` field-prefix search.
- Preserved access issue message search for `access:<error text>` and `issue:<error text>`.
- Updated the WPF search tooltip to include `access:readable`, `access:access issue`, and `issue:denied` examples.
- Added core coverage for `access:readable`, `access:access issue`, and access issue message searches.
- Added WPF fixture coverage for `access:readable` search, searched export filename hints, and access-prefix tooltip guidance.
- No access retry, permission change, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-access-status-search.md`
- `docs/features/2026-05-28-access-status-review-field.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible in-memory review behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, do `access:readable`, `access:access issue`, and `issue:<error text>` make access review easier alongside the Access issues filter?

Rejected ideas buffer:

- Do not add retry-as-admin or permission-changing behavior as part of access status search.

### 2026-05-28: Add Storage Review Display Window

Status: completed

Evidence:

- The real scan contained 188,580 files and 37,740 folders, while the WPF grid intentionally displays at most 2,000 rows at once.
- The app explained the cap but did not let the user move beyond the first matched row window inside the app.

Implementation:

- Added read-only Previous rows and Next rows controls to the WPF review toolbar.
- Added a Storage Review Display Window label showing active row ranges such as `rows 1-2,000 of N matched`.
- Added `_currentDisplayStartIndex` to page through matched in-memory review rows without rescanning.
- Reset the display window to the first matched rows when filters, type filters, category filters, search, safety shortcuts, or Review View Reset change the active review lens.
- Updated completed-scan status and filter summary wording to use active row-window ranges.
- Corrected Scan Report Export row selection to include the active Storage Entry Type Filter.
- Added WPF fixture coverage for next/previous row windows, display-window reset, read-only status wording, and type-filtered exports.
- No scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-display-window.md`
- `docs/features/2026-05-28-storage-review-display-limit.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF review behavior and does not change architecture, persistence, security, deployment, cleanup execution, or public APIs.

Open questions:

- In the next real scan, are row-window controls enough, or would a virtualized tree/grid still be worth designing later?

Rejected ideas buffer:

- Do not rescan or discard matched rows when changing display windows.
- Do not make `Shortlist shown` apply to rows outside the current display window.

### 2026-05-28: Tighten Display Window Reset for Combo Filters

Status: completed

Evidence:

- Storage Review Display Window should reset when the active review lens changes.
- Programmatic filter helpers already reset the display window, but the actual WPF category/type combo-box event handlers needed the same reset path for manual UI selection.

Implementation:

- Reset `_currentDisplayStartIndex` in `CategoryFilterBox_SelectionChanged`.
- Reset `_currentDisplayStartIndex` in `EntryTypeFilterBox_SelectionChanged`.
- Added WPF test hooks that select type/category options through the real combo boxes.
- Added WPF fixture coverage proving combo-driven type and no-category filter changes reset the display window to the first matched rows.
- No scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `docs/features/2026-05-28-storage-review-display-window.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a small WPF state-consistency fix inside an existing read-only review behavior.

Open questions:

- None.

Rejected ideas buffer:

- Do not rely only on programmatic helper methods when the visible WPF controls use separate event handlers.

### 2026-05-28: Add Contents Column to Storage Review Grid

Status: completed

Evidence:

- The app already computed contained file/folder counts and exported them to CSV, but the main review grid only showed those counts after selecting a row.
- Large real-profile container rows are easier to compare when contents counts are visible before selection.

Implementation:

- Added a WPF grid `Contents` column bound to `StorageEntryRow.Contents`.
- Kept contents counts read-only and derived from the completed Storage Scan result.
- Added WPF fixture coverage proving a folder row exposes file/folder counts in the visible row data.
- No scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-row-contents-context.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF display of existing read-only scan context.

Open questions:

- In the next real scan, is the grid `Contents` column useful without making the table feel too crowded?

Rejected ideas buffer:

- Do not treat contents counts as storage savings or cleanup approval.

### 2026-05-28: Sort Contents Column by Contained Count

Status: completed

Evidence:

- The WPF grid `Contents` column displays formatted text, which is useful to read but weak as a sort value.
- The user-tested real scan surfaced many large containers, so comparing rows by total contained items is useful review context before selecting a row.

Implementation:

- Added `StorageEntryRow.ContainedTotalCount` as the numeric sort value for contained files plus descendant folders.
- Set the WPF grid `Contents` column `SortMemberPath` to `ContainedTotalCount`.
- Added WPF fixture coverage for the column sort contract and a folder row's numeric contained-item total.
- Kept the change read-only; no scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-row-contents-context.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF review behavior for an existing read-only context signal.

Open questions:

- In the next real scan, does sorting by `Contents` help separate broad buckets from focused cleanup candidates?

Rejected ideas buffer:

- Do not rank cleanup safety by contained-item count alone.

### 2026-05-28: Add Relative Paths to Scan Report Export

Status: completed

Evidence:

- Real-profile rows all share the long `C:\Users\moxhe` prefix, which can make spreadsheet review harder than the in-app tree context.
- Earlier progress left a relative-path export column as an open follow-up for Scan Report Export.

Implementation:

- Added a `Relative path` CSV column to `StorageScanCsvExporter`.
- Derived relative paths from the completed Cleanup Scope when available; unsupported or outside-scope rows leave the relative path blank.
- Updated WPF export and Review Shortlist export to pass the completed scan scope to CSV generation.
- Added a WPF test hook for the current Scan Report Export CSV and fixture coverage for relative paths.
- Kept this report-only; no scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible CSV report shape improvement.

Open questions:

- None.

Rejected ideas buffer:

- Do not replace full paths in exports; relative paths are additional review context.

### 2026-05-28: Show Access Issue Examples in Safety Summary

Status: completed

Evidence:

- The first real scan reported 3 access issues.
- The app already counted and filtered access issues, but the top safety summary did not show any concrete example paths before the user clicked into the filtered review.

Implementation:

- Added bounded access issue examples to `StorageScanSafetySummary`.
- Derived up to three cleanup-scope-relative examples from completed scan rows, including scanner error text when available.
- Updated WPF Safety Summary text to show access examples when incomplete scan coverage exists.
- Added core coverage for relative access issue examples and scanner error text.
- Kept this read-only; no scanner retry, permission change, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only summary display improvement.

Open questions:

- In the next real scan, are the three access issue examples enough context before using the Access issues filter?

Rejected ideas buffer:

- Do not add automatic elevated retries or permission changes for access issues.

### 2026-05-28: Add CI MVP Preflight Workflow

Status: completed

Evidence:

- Local MVP preflight already restores, builds, runs core tests, runs WPF app tests, runs the synthetic fixture generator in `-WhatIf` mode, and runs `git diff --check`.
- Remote pushes should use the same read-only gate so the command list does not drift from local verification.

Implementation:

- Added `.github/workflows/mvp-preflight.yml`.
- Configured the workflow for pushes and pull requests targeting `main`.
- Used a Windows runner, read-only repository permissions, .NET SDK `8.0.421`, and the existing `.\tools\Invoke-MvpPreflight.ps1` script.
- Documented CI preflight behavior in `README.md`.
- No scanner traversal changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- GitHub Actions `MVP Preflight` run `26575441204` for commit `711cfb6` completed with conclusion `success`.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-ci-mvp-preflight.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible repository verification workflow using the existing MVP preflight.

Open questions:

- None.

Rejected ideas buffer:

- Do not duplicate the local preflight command list in CI YAML.

### 2026-05-28: Add Cleanup Scope Browse Action

Status: completed

Evidence:

- The WPF app allowed typing a Cleanup Scope path, but manual fixture/custom scope review is less error-prone with native folder selection.
- Cleanup Scope Selection should remain separate from Storage Scan so fixture review and real-profile preflight gates stay explicit.

Implementation:

- Added a `Browse...` button beside the Cleanup Scope path box.
- Used WPF's native `OpenFolderDialog` to choose a folder.
- Folder selection updates `ScopePathBox`, which keeps existing Cleanup Scope Safety Note and Scan Gate behavior authoritative.
- Disabled browsing while scanning and re-enabled it after fixture scans.
- Added WPF smoke assertions for browse visibility and availability.
- No scanner traversal changes, auto-scan behavior, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-browse-action.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible WPF usability improvement that does not change scan, persistence, security, deployment, or cleanup behavior.

Open questions:

- Does the extra header button fit comfortably during the next manual fixture UI pass?

Rejected ideas buffer:

- Do not auto-start a scan after folder selection.

### 2026-05-28: Promote Specific Rebuildable Cache Candidates

Status: completed

Evidence:

- The real scan screenshot showed large cache rows such as `NVIDIA\DXCache` and `pip\Cache`.
- Broad AppData-adjacent folders should stay conservative, but specific rebuildable cache rows should be easier to find through Quarantine candidates.

Implementation:

- Added a narrow `HasSpecificRebuildableCacheEvidence` classifier rule.
- Promoted specific GPU shader cache rows and package-cache rows with app-cache evidence to `Likely safe` / `Quarantine candidate`.
- Kept broad parent folders such as `pip`, `NVIDIA`, generic `AppData`, browser data, installed apps, Windows app data, game data, source-code paths, and Codex-related paths conservative.
- Reordered Selected Path Review Guidance so likely-safe cache rows still show cache-specific warning text and Review Shortlist / Quarantine Preview wording.
- No scanner traversal changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cache-specific-review-guidance.md`
- `docs/features/2026-05-28-specific-rebuildable-cache-candidates.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible classification and review-guidance refinement that does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, do `DXCache` and `pip\Cache` appearing under Quarantine candidates make review easier without making broad parent folders look safe?

Rejected ideas buffer:

- Do not promote generic `AppData`, app, profile, package, source-code, Codex, or browser parent folders just because they contain a cache-like descendant.

### 2026-05-28: Add Storage Review Relative Path Column

Status: completed

Evidence:

- The real scan includes many rows under the same `C:\Users\moxhe` prefix and many short or repeated names, such as cache folders and hashed files.
- Scan Report Export already includes cleanup-scope-relative paths for spreadsheet review; the WPF grid did not show that same compact context directly.

Implementation:

- Added `StorageEntryRow.RelativePath`, derived from the completed Cleanup Scope.
- Added a WPF `Relative path` grid column after `Name`.
- Added `Relative:` to the selected-row detail context above `Parent:`.
- Kept full parent path context and all cleanup recommendations unchanged.
- No scanner traversal changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-hierarchy-context.md`
- `docs/features/2026-05-28-storage-review-relative-path-column.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only UI review context and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does the extra grid column fit comfortably during the next manual fixture and real-profile review passes?

Rejected ideas buffer:

- Do not remove the full `Parent` column yet; relative path and parent path answer different review questions.

### 2026-05-28: Show Safety Summary Candidate Examples

Status: completed

Evidence:

- Storage Scan Safety Summary already counted Quarantine candidates and showed access issue examples.
- Real-profile review benefits from concrete candidate examples before using filters, search, Review Shortlist, or Quarantine Preview.

Implementation:

- Added `StorageScanSafetySummary.QuarantineCandidateExamples`.
- Built up to three largest Quarantine candidate examples using cleanup-scope-relative paths and row sizes.
- Added `Candidate examples:` to WPF Safety Summary text.
- Kept examples read-only and separate from Review Shortlist, Quarantine Preview, and cleanup approval.
- No scanner traversal changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `docs/features/2026-05-28-safety-summary-candidate-examples.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only summary context and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Do the candidate examples help real-profile review, or does the Safety Summary become too dense?

Rejected ideas buffer:

- Do not use Safety Summary candidate examples as cleanup approval or as a replacement for Review Shortlist and Quarantine Preview.

### 2026-05-28: Use Relative Paths for Quarantine Preview Blocker Examples

Status: completed

Evidence:

- Quarantine Preview protected-descendant blockers were useful, but absolute descendant paths are noisy in real-profile review where every row shares the same Cleanup Scope prefix.
- The app now uses relative paths elsewhere in the grid, detail pane, safety summary, and CSV reports.

Implementation:

- Updated protected-descendant blocker reasons to format descendant examples relative to the Cleanup Scope.
- Kept absolute source/destination paths in preview row details for precise identity.
- Added core and WPF smoke coverage for `.cache\codex-runtimes` relative blocker evidence.
- No scanner traversal changes, preview eligibility changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview-protected-descendant-blocker.md`
- `docs/features/2026-05-28-quarantine-preview-relative-blocker-examples.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only preview wording and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, are relative blocker examples enough, or does the preview pane need a structured blocked-descendant table?

Rejected ideas buffer:

- Do not remove absolute source paths from preview row details; they remain useful for precise identity.

### 2026-05-28: Show Safety Summary No Category Examples

Status: completed

Evidence:

- Storage Scan Safety Summary counted Uncategorized Results and exposed a No category review shortcut, but it did not show concrete examples.
- The real scan surfaced many unfamiliar rows where the first step is classification, not cleanup.

Implementation:

- Added `StorageScanSafetySummary.UncategorizedExamples`.
- Built up to three largest no-category examples using cleanup-scope-relative paths and row sizes.
- Added `No category examples:` to WPF Safety Summary text.
- Kept examples read-only and separate from cleanup recommendations, Review Shortlist, and Quarantine Preview.
- No scanner traversal changes, classification changes, recommendation changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `docs/features/2026-05-28-safety-summary-no-category-examples.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only summary context and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, do no-category examples help target classification work, or does the Safety Summary become too dense?

Rejected ideas buffer:

- Do not treat no-category examples as cleanup candidates; they are classification prompts.

### 2026-05-28: Add Storage Size Threshold Filter

Status: completed

Evidence:

- User's real scan screenshot showed a 58.02 GB profile with many large rows, including multi-GB app/cache containers and many smaller recursive rows.
- The app already had review/category/type/search lenses, but no direct way to focus on rows above a chosen size.

Implementation:

- Added `StorageSizeThresholdFilter` with All sizes, 1 MB+, 100 MB+, 1 GB+, 5 GB+, and 10 GB+.
- Applied size-threshold filtering in `StorageScanReview` so WPF display, review-window paging, and Scan Report Export use the same active review lens.
- Added a WPF Size combo box, filter-summary wording, reset behavior, and export filename segment.
- Kept the change read-only; no scanner traversal changes, classification changes, recommendation changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-size-threshold-filter.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review filtering and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, do `1 GB+`, `5 GB+`, and `10 GB+` make large-row triage easier without overemphasizing size as safety evidence?

Rejected ideas buffer:

- Do not make large rows more likely to be cleanup candidates solely because they match a size threshold.

### 2026-05-28: Extend Game Mod Data Protection Hints

Status: completed

Evidence:

- The real scan surfaced game and mod-related folders that should be clearer than generic uncategorized rows before any cleanup execution exists.
- Conservative app-data classification already treats game data as Protected Location / High risk / Keep.

Implementation:

- Extended game data hints to include Minecraft, OptiFine, CurseForge, Modrinth, Vortex, and Nexus Mods paths.
- Added fixture coverage proving OptiFine, CurseForge, and Vortex folders are `Game data`, `High risk`, and `Keep`.
- Kept the change conservative and read-only; no scanner traversal changes, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-conservative-app-data-classification.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This extends an existing conservative classifier rule and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Which specific game or mod-manager cache subfolders, if any, should become cleanup exceptions after manual review?

Rejected ideas buffer:

- Do not treat mod-manager downloads or profiles as safe bloat just because they can be large.

### 2026-05-29: Add Cloud Sync and Credential Protection

Status: completed

Evidence:

- The app is intended to review `C:\Users\moxhe`, where cloud sync roots and credential/key/password-manager paths can be large or hidden but should not be cleanup targets by default.
- Selected File Content Preview should help with unfamiliar text files without exposing secrets from credential rows.

Implementation:

- Added `CloudSyncData` and `CredentialData` Bloat Category values.
- Added conservative classifier hints for common cloud sync providers and credential/password-manager/key paths.
- Added user-facing labels in WPF rows, category filters, Scan Report Export, and Quarantine Preview CSV.
- Blocked Selected File Content Preview for Credential Data.
- Added fixture coverage for OneDrive, Dropbox, SSH keys, Bitwarden, KeePass vault files, and credential preview blocking.
- Kept the change conservative and read-only; no scanner traversal changes, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-file-content-preview.md`
- `docs/features/2026-05-29-cloud-sync-credential-protection.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible conservative classification and preview-safety refinement that does not change architecture, persistence, deployment, or cleanup execution.

Open questions:

- Which cloud-provider cache folders, if any, should become cleanup exceptions after manual review?

Rejected ideas buffer:

- Do not preview credential file contents merely because the file is small or text-like.
- Do not classify cloud sync roots as cleanup candidates without provider-specific review.

### 2026-05-29: Add Quarantine Root Preview Selection

Status: completed

Evidence:

- The user requested a quarantine folder preferably on `D:` and easy undo later.
- Quarantine Preview already showed destination paths, but the preview root was an invisible fixed default.
- Keeping the root visible and editable improves review without adding cleanup execution.

Implementation:

- Added a WPF Quarantine root text box defaulting to `D:\WindowsFileCleanerQuarantine`.
- Routed Quarantine Preview destination paths through the typed Quarantine Root Selection.
- Cleared stale Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft output when the root changes.
- Added WPF smoke coverage proving custom preview roots affect destination paths and do not create folders.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview.md`
- `docs/features/2026-05-29-quarantine-root-preview-selection.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only preview UI refinement and does not change cleanup execution, persistence, deployment, or manifest writes.

Open questions:

- In the next manual fixture or real-profile pass, is the Quarantine root field readable in the wrapped review toolbar?

Rejected ideas buffer:

- Do not persist the Quarantine Root Selection until actual Quarantine execution needs local settings.
- Do not create or validate the quarantine folder by touching the filesystem during preview.

### 2026-05-29: Add Quarantine Root Browse Action

Status: completed

Evidence:

- The user prefers a quarantine location on `D:`.
- Typing a root path works, but browsing is less error-prone during manual review.
- Browsing can update preview destinations without creating folders or moving files.

Implementation:

- Added `BrowseQuarantineRootButton` next to the Quarantine root text box.
- Added a native folder picker for Quarantine Root Selection.
- Added WPF smoke coverage that the browse action is present and enabled before scanning.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-root-browse-action.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only UI refinement and does not change cleanup execution, persistence, deployment, or manifest writes.

Open questions:

- In the next manual fixture pass, does browsing to a `D:` root make preview destination review easier?

Rejected ideas buffer:

- Do not treat browsing to a root as approval to execute Quarantine.
- Do not create the selected folder during browse or preview.

### 2026-05-29: Split Review Shortlist and Quarantine Toolbar

Status: completed

Evidence:

- Quarantine Root Selection and browse controls made the review action toolbar wider.
- The next recommended work is a visible fixture pass, so keeping review controls easy to scan is useful before more manual testing.

Implementation:

- Kept search, row-window, type, size, and category controls in the existing wrapping Review Action toolbar.
- Moved Review Shortlist and Quarantine Preview controls into a separate wrapping `ReviewShortlistToolbar`.
- Extended the WPF smoke layout assertion to require the new wrapping toolbar.
- Kept the change UI-only and read-only; no scanner behavior, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `docs/features/2026-05-28-wpf-review-toolbar-layout-polish.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible UI layout polish and does not change architecture, persistence, security, deployment, or cleanup behavior.

Open questions:

- In the next manual fixture pass, does the separated shortlist/quarantine toolbar make the Quarantine root controls easier to read?

Rejected ideas buffer:

- Do not treat structural layout assertions as proof of visible polish; keep the manual fixture pass as the visual check.

### 2026-05-29: Add Quarantine Root Safety Note

Status: completed

Evidence:

- Quarantine Root Selection is now typed or browsable, so obvious relative-path mistakes should be caught before Quarantine Preview builds destination paths.
- The user prefers quarantine on `D:`, but fixture review can still benefit from fully qualified non-`D:` preview roots.

Implementation:

- Added `QuarantineRootSafetyNote` and `QuarantineRootSafetyNoteBuilder`.
- Added WPF safety-note text under the Quarantine root controls.
- Disabled `Preview quarantine` when the current root is relative or invalid.
- Kept blank roots falling back to `D:\WindowsFileCleanerQuarantine`.
- Kept fully qualified non-`D:` roots usable for preview with non-preferred wording.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-root-safety-note.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only preview guard and does not change cleanup execution, persistence, deployment, or manifest writes.

Open questions:

- Should actual Quarantine execution require an existing `D:` folder, offer to create it, or support a stricter destination policy?

Rejected ideas buffer:

- Do not create or probe the quarantine folder just to decide whether Quarantine Preview can run.

### 2026-05-29: Add Quarantine Execution Gate

Status: completed

Evidence:

- Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft already prove preview and undo metadata shape.
- Actual Quarantine execution still needs explicit confirmation semantics before file-moving code exists.

Implementation:

- Added `QuarantineExecutionGate` and `QuarantineExecutionGateBuilder`.
- Added WPF confirmation text, disabled `Execute quarantine` button, and execution gate readout.
- Required exact `QUARANTINE` text while preserving confirmation-readiness blockers.
- Kept execution unavailable because Quarantine execution is not implemented.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-execution-gate.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only gate and does not decide file-moving layout, manifest write order, or Undo Quarantine behavior.

Open questions:

- What exact manifest write order should actual Quarantine execution use?
- Should future execution require a selected manifest path, a generated action id, or both?

Rejected ideas buffer:

- Do not add real file-moving code in the same packet as the first visible execution gate.
- Do not let matching confirmation text override data blockers or unimplemented execution support.

### 2026-05-29: Add Quarantine Action Draft

Status: completed

Evidence:

- Quarantine execution needs a concrete action-scoped destination and manifest layout before file-moving code exists.
- Preview paths should remain preview-only and separate from future executed quarantine paths.

Implementation:

- Added ADR 0004 for action-scoped quarantine layout.
- Added `QuarantineActionDraft`, `QuarantineActionEntryDraft`, and `QuarantineActionDraftBuilder`.
- Mapped future item paths under `<quarantine-root>\actions\<action-id>\items\...`.
- Mapped the future Restore Manifest path to `<quarantine-root>\actions\<action-id>\restore-manifest.json`.
- Added consistency checks across Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft metadata.
- Added WPF Quarantine Execution Gate readout for action items root and restore manifest path.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0004-use-action-scoped-quarantine-layout.md`
- `docs/features/2026-05-29-quarantine-action-draft.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0004-use-action-scoped-quarantine-layout.md`.

Open questions:

- What exact manifest write order should actual Quarantine execution use?
- How should partial move failures update the executed Restore Manifest?

Rejected ideas buffer:

- Do not reuse preview paths as executed quarantine paths.
- Do not use a flat quarantine root for all moved items.

### 2026-05-29: Add Write-Ahead Restore Manifest Model

Status: completed

Evidence:

- Sidecar safety review found a manifest timing conflict: domain context said to persist a Restore Manifest before moving files, while ADR 0003 still said after file moves are attempted.
- Actual Quarantine execution needs a recoverable write order and partial-failure states before any file-moving code exists.

Implementation:

- Added ADR 0005 for write-ahead Restore Manifest ordering.
- Added `RestoreManifest`, `RestoreManifestEntry`, `RestoreManifestBuilder`, and `RestoreManifestJsonSerializer`.
- Added `RestoreManifestActionStatus` and `RestoreManifestEntryStatus`.
- Built a planned Restore Manifest from the Quarantine Action Draft using action-scoped quarantine paths, not preview paths.
- Added in-memory status transitions for Moving, Moved, Failed, Completed, Partial failure, and Failed action outcomes.
- Added WPF Quarantine Execution Gate readout for write-ahead manifest status and write order.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest file writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0003-use-json-restore-manifest.md`
- `docs/decisions/0004-use-action-scoped-quarantine-layout.md`
- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `docs/features/2026-05-29-quarantine-action-draft.md`
- `docs/features/2026-05-29-quarantine-execution-gate.md`
- `docs/features/2026-05-29-write-ahead-restore-manifest.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0005-use-write-ahead-restore-manifest.md`.

Open questions:

- What exact recovery UI should handle Moving entries after interruption?
- How should the future app surface leftover temp files after a hard crash?

Rejected ideas buffer:

- Do not write a final-only manifest after all moves succeed.
- Do not keep the old write-after-attempt wording; it creates a recovery gap.
- Do not add file-moving API allowlists until a narrow execution component exists.

### 2026-05-29: Add Restore Manifest File Store

Status: completed

Evidence:

- ADR 0005 requires a planned Restore Manifest to be written before any future move.
- Sidecar safety review recommended introducing write APIs only in a narrow execution component with a strict source allowlist.
- Manifest writing can be proven against fixtures before adding file-moving code.

Implementation:

- Added ADR 0006 for temp-file replacement Restore Manifest writes.
- Added `RestoreManifestFileStore` and `RestoreManifestFileWriteResult`.
- The file store validates that `ManifestPath` stays inside `ActionRootPath` and that the filename is `restore-manifest.json`.
- The file store writes JSON to a temporary file in the same action folder, then replaces or moves it into place.
- Added fixture-backed tests for first write, replacement write, invalid outside paths, invalid filenames, temp cleanup, source preservation, and not creating the action items folder.
- Updated the source-level filesystem-call regression to allow write APIs only for user-selected CSV exports and `RestoreManifestFileStore`.
- Kept WPF execution unavailable; no scanned files are moved, deleted, quarantined, or restored by the app.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `docs/features/2026-05-29-write-ahead-restore-manifest.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`.

Open questions:

- How should the future app surface leftover temp files after a hard crash?
- Should future Undo Quarantine expose a manifest integrity check before restore?

Rejected ideas buffer:

- Do not write manifests directly to the final path.
- Do not loosen the filesystem-call guard globally.
- Do not wire WPF execution in the same packet as the first manifest writer.

### 2026-05-29: Add Fixture-First Quarantine Executor

Status: completed

Evidence:

- Restore Manifest File Store is fixture-tested and can write action-scoped manifests.
- Sidecar safety review recommended a separate executor component, a narrow allowlist, and keeping WPF execution closed.
- MVP needs actual Quarantine movement eventually, but synthetic fixture execution should prove move semantics first.

Implementation:

- Added ADR 0007 for the fixture-first Quarantine Executor boundary.
- Added `QuarantineExecutor`, `QuarantineExecutionResult`, and `QuarantineExecutionEntryResult`.
- The executor writes the planned Restore Manifest before any move, writes Moving before each move attempt, revalidates source/destination/reparse status, moves the file or folder, then writes Moved or Failed.
- The executor continues after per-entry move failures so partial-failure manifests can be produced.
- The executor stops before later moves when a manifest write fails.
- Extended the source-level filesystem-call regression to allow `Directory.CreateDirectory`, `Directory.Move`, and `File.Move` only in `QuarantineExecutor`.
- Kept WPF execution unavailable; `Execute quarantine` remains disabled/status-only.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `docs/decisions/0007-use-fixture-first-quarantine-executor.md`
- `docs/features/2026-05-29-quarantine-executor-fixture-first.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0007-use-fixture-first-quarantine-executor.md`.

Open questions:

- What exact WPF stale-state checks are required before calling the executor?
- What recovery UI should handle Moving entries after interruption?
- How should the app surface leftover temp manifest files after a hard crash?

Rejected ideas buffer:

- Do not wire WPF execution in the same packet as the first executor.
- Do not implement rollback inside Quarantine Executor; Undo Quarantine needs a separate design.
- Do not overwrite existing quarantine destinations.

### 2026-05-29: Add Fixture-First Undo Quarantine

Status: completed

Evidence:

- User requested quarantine on `D:` with an easy undo path.
- Core fixture-first Quarantine Executor can already produce Moved Restore Manifest entries.
- ADR 0008 selects a separate fixture-first Undo Quarantine Executor before WPF execution or WPF undo is wired.

Implementation:

- Added `UndoQuarantineExecutor`, `UndoQuarantineResult`, and `UndoQuarantineEntryResult`.
- Extended Restore Manifest action statuses with Restoring, Restored, RestorePartialFailure, and RestoreFailed.
- Extended Restore Manifest entry statuses with Restoring, Restored, and RestoreFailed.
- Added restore start/completion timestamps to Restore Manifest entries.
- Undo restores only Moved entries, writes Restoring before each restore attempt, refuses original-path collisions, keeps move failures for recovery review, checks missing quarantine paths and reparse points, then writes Restored or RestoreFailed.
- Undo continues after per-entry restore failures and stops before later restore attempts when manifest writing fails.
- Extended the source-level filesystem-call regression to allow `Directory.CreateDirectory`, `Directory.Move`, and `File.Move` only in `UndoQuarantineExecutor` for restore movement.
- Kept WPF Quarantine execution and WPF Undo Quarantine unavailable.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `docs/decisions/0007-use-fixture-first-quarantine-executor.md`
- `docs/decisions/0008-use-fixture-first-undo-quarantine.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-quarantine-executor-fixture-first.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `docs/features/2026-05-29-undo-quarantine-fixture-first.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0008-use-fixture-first-undo-quarantine.md`.

Open questions:

- What UI should discover and select existing Restore Manifests?
- Should successful WPF Undo Quarantine offer to clean up empty action folders?
- How should the app surface leftover temp manifest files after a hard crash?

Rejected ideas buffer:

- Do not overwrite original paths during undo.
- Do not automatically delete quarantine action folders after restore.
- Do not implement same-execution rollback inside Quarantine Executor.
- Do not wire WPF Undo Quarantine in the same packet as core fixture undo.

### 2026-05-29: Add Fixture-Only WPF Quarantine Execution

Status: completed

Evidence:

- Core Quarantine Executor and Undo Quarantine Executor are fixture-tested.
- ADR 0009 selects visible WPF execution for fixture Cleanup Scopes only.
- Real-profile and custom non-fixture execution must remain unavailable.

Implementation:

- Added ADR 0009 for fixture-only WPF Quarantine execution.
- Extended `QuarantineConfirmationDraft` so execution availability can be true for fixture scopes and false elsewhere.
- Updated `QuarantineExecutionGateBuilder` to open only when readiness blockers are clear, exact `QUARANTINE` is entered, and execution is available.
- Wired WPF `Execute quarantine` to `QuarantineExecutor.Execute` for recognized fixture Cleanup Scopes.
- After execution, WPF shows execution result evidence, clears stale Review Shortlist state, disables re-execution for the current preview, and tells the user to rescan.
- Added WPF smoke coverage proving a fixture execution moves a synthetic file and writes `restore-manifest.json`.
- Added WPF smoke coverage proving a custom non-fixture scope remains blocked even after exact confirmation.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0009-use-fixture-only-wpf-quarantine-execution.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-wpf-fixture-only-quarantine-execution.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0009-use-fixture-only-wpf-quarantine-execution.md`.

Open questions:

- What visible WPF Undo Quarantine flow should discover and restore manifests?
- What additional confirmation or backup step should exist before real-profile execution?
- Should a fixture execution clear the grid entirely or is stale-state wording enough?

Rejected ideas buffer:

- Do not enable real-profile WPF execution in this packet.
- Do not implement file movement directly in WPF.
- Do not leave the gate enabled after execution.
- Do not imply WPF Undo Quarantine exists after fixture execution.

### 2026-05-29: Add WPF Current Fixture Undo Quarantine

Status: completed

Evidence:

- Core Undo Quarantine is fixture-tested.
- WPF fixture-only Quarantine execution produces a current in-memory Restore Manifest.
- ADR 0010 selects current-fixture-execution WPF undo before broad manifest discovery or real-profile undo.

Implementation:

- Added ADR 0010 for WPF current fixture undo.
- Added `Undo fixture quarantine` to the WPF execution gate area.
- Wired WPF undo to `UndoQuarantineExecutor.Undo` for the current fixture Restore Manifest.
- WPF undo restores the synthetic source file, updates Restore Manifest status, shows undo result evidence, disables repeat undo, and preserves stale-state wording.
- Kept real-profile WPF undo and manifest discovery unavailable.
- Extended WPF smoke coverage to execute and undo a fixture Quarantine action.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-wpf-current-fixture-undo-quarantine.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`.

Open questions:

- What UI should discover and select old Restore Manifests?
- Should successful undo offer to clean up empty action folders?
- What additional confirmation should real-profile undo require?

Rejected ideas buffer:

- Do not add manifest discovery in this packet.
- Do not enable real-profile undo.
- Do not delete quarantine folders after undo.
- Do not implement restore movement in WPF.

### 2026-05-29: Add Quarantine Manifest Discovery

Status: completed

Evidence:

- WPF current-fixture undo proves only the current in-memory Restore Manifest.
- ADR 0011 selects read-only discovery before any broad WPF Undo Quarantine.
- Existing Restore Manifests are action-scoped under `<quarantine-root>\actions\<action-id>\restore-manifest.json`.

Implementation:

- Added ADR 0011 for read-only Quarantine Manifest Discovery.
- Added Restore Manifest JSON deserialization.
- Added `QuarantineManifestDiscovery`, `QuarantineManifestDiscoveryBuilder`, `QuarantineManifestDiscoveryIssue`, and `RestoreManifestSummary`.
- Discovery reads direct action-scoped `restore-manifest.json` files, summarizes valid manifests, and reports missing/invalid/path-mismatch issues.
- Added WPF `Discover manifests` status-only action and pane.
- Kept discovered-manifest restore, real-profile execution, permanent deletion, cleanup history, and quarantine-folder cleanup unavailable.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0011-use-read-only-quarantine-manifest-discovery.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-quarantine-manifest-discovery.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0011-use-read-only-quarantine-manifest-discovery.md`.

Open questions:

- What UI should select one old Restore Manifest for future Undo Quarantine?
- Should discovery support browsing directly to a single manifest file?
- Should successful undo offer to clean up empty action folders?

Rejected ideas buffer:

- Do not restore discovered old manifests in this packet.
- Do not call discovery cleanup history.
- Do not scan the entire quarantine root recursively.
- Do not clean up empty action folders.

### 2026-05-29: Fix MVP Preflight Failure Propagation

Status: completed

Evidence:

- During Quarantine Manifest Discovery work, a WPF app test failure still let `Invoke-MvpPreflight.ps1` print `MVP preflight passed`.
- PowerShell native command failures did not throw from `$ErrorActionPreference = "Stop"` without checking `$LASTEXITCODE`.

Implementation:

- Updated `Invoke-PreflightStep` to reset and check `$global:LASTEXITCODE` for every step.
- Added a source-level regression check that the preflight script captures and throws on native non-zero exit codes.
- Updated README preflight wording to state that non-zero child commands fail preflight/CI.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-mvp-preflight-failure-propagation.md`
- `.codex/progress.md`

ADRs:

- No ADR. This is a reversible verification script fix.

Open questions:

- Should the preflight script also emit structured machine-readable output later?

Rejected ideas buffer:

- Do not add a fake failure mode to the production preflight script.
- Do not duplicate the local preflight command list in CI.

### 2026-05-29: Add Restore Readiness Preview

Status: completed

Evidence:

- Quarantine Manifest Discovery can show old action-scoped Restore Manifests but did not show entry-level restore blockers.
- ADR 0012 selects read-only Restore Readiness Preview before broad WPF Undo Quarantine.
- Undo Quarantine Executor already defines restore blockers such as missing quarantine paths and original-path collisions.

Implementation:

- Added ADR 0012 for read-only Restore Readiness Preview.
- Added `RestoreReadinessPreview`, `RestoreReadinessManifestPreview`, `RestoreReadinessEntryPreview`, `RestoreReadinessDisposition`, and `RestoreReadinessPreviewBuilder`.
- Extended discovery to retain valid Restore Manifest objects for readiness evaluation.
- WPF now exposes `Preview all-manifest readiness` as a status-only action under the selected Quarantine Root.
- Readiness preview reports restorable, blocked, already-restored, recovery-review, and not-moved entries without restoring files.
- Kept discovered-manifest restore, real-profile execution, permanent deletion, cleanup history, manifest writes, and quarantine-folder cleanup unavailable.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0012-use-read-only-restore-readiness-preview.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-restore-readiness-preview.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0012-use-read-only-restore-readiness-preview.md`.

Open questions:

- What UI should select one discovered Restore Manifest for execution?
- What exact confirmation should broad WPF Undo Quarantine require?
- Should successful restore offer empty action-folder cleanup?

Rejected ideas buffer:

- Do not restore discovered old manifests in this packet.
- Do not treat readiness as approval to restore.
- Do not add manifest selection yet.
- Do not clean up empty action folders.

### 2026-05-29: Add Selected Restore Manifest Review

Status: completed

Evidence:

- Quarantine Manifest Discovery can find older action-scoped Restore Manifests.
- Restore Readiness Preview can evaluate all discovered manifests, but future broad Undo Quarantine needs explicit one-manifest selection.
- ADR 0013 selects read-only Selected Restore Manifest Review before any selected old-manifest restore execution.

Implementation:

- Added ADR 0013 for read-only Selected Restore Manifest Review.
- Added `SelectedRestoreManifestReview` and `SelectedRestoreManifestReviewBuilder`.
- WPF now populates `RestoreManifestSelectionBox` after `Discover manifests`, auto-selects the newest discovered Restore Manifest, and exposes `Preview selected readiness`.
- Selected readiness evaluates only the selected Restore Manifest and reports selection issues for missing discovery, blank selection, or stale paths.
- Kept discovered-manifest restore, real-profile execution, permanent deletion, cleanup history, manifest writes, and quarantine-folder cleanup unavailable.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0013-use-read-only-selected-restore-manifest-review.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-selected-restore-manifest-review.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0013-use-read-only-selected-restore-manifest-review.md`.

Open questions:

- What exact confirmation phrase should selected broad Undo Quarantine require?
- Should future restore execution allow only manifests with zero readiness blockers?
- Should successful restore offer empty action-folder cleanup?

Rejected ideas buffer:

- Do not restore selected old manifests in this packet.
- Do not treat selected readiness as approval to restore.
- Do not add cleanup history in this packet.
- Do not remove all-manifest Restore Readiness Preview.

### 2026-05-29: Add Selected Restore Confirmation Gate

Status: completed

Evidence:

- Selected Restore Manifest Review can focus one discovered Restore Manifest and preview selected readiness.
- Future selected restore execution needs exact confirmation semantics before any file-moving workflow is exposed.
- ADR 0014 selects read-only Selected Restore Confirmation Draft and Selected Restore Execution Gate before fixture-first selected restore execution.

Implementation:

- Added ADR 0014 for read-only Selected Restore Confirmation Gate.
- Added `SelectedRestoreConfirmationDraft`, `SelectedRestoreConfirmationDraftBuilder`, `SelectedRestoreExecutionGate`, and `SelectedRestoreExecutionGateBuilder`.
- Core selected restore gate requires exact `RESTORE`, clear blockers, and explicit execution availability before `CanExecute` can open.
- WPF now exposes `Preview selected restore gate` and a selected restore confirmation text box after selected readiness.
- WPF passes selected restore execution as unavailable, so typing `RESTORE` can match but still leaves `Can execute: no`.
- Kept selected old-manifest restore, real-profile execution, permanent deletion, cleanup history, manifest writes, and quarantine-folder cleanup unavailable.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0014-use-read-only-selected-restore-confirmation-gate.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-selected-restore-confirmation-gate.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0014-use-read-only-selected-restore-confirmation-gate.md`.

Open questions:

- Should future selected restore execution require zero blocked, recovery-review, and not-moved readiness rows?
- Should successful selected restore offer empty action-folder cleanup?
- What stale-state wording should appear when readiness changes between preview and execution?

Rejected ideas buffer:

- Do not restore selected old manifests in this packet.
- Do not treat typed `RESTORE` as approval while execution is unavailable.
- Do not expose a selected restore execution button yet.
- Do not reuse the Quarantine confirmation phrase for restore.

### 2026-05-29: Add Fixture-only Selected Restore Execution

Status: completed

Evidence:

- Selected Restore Manifest Review and Selected Restore Confirmation Gate prove selected manifest, selected readiness, and exact `RESTORE` confirmation semantics.
- Current-fixture WPF undo proves the visible app can call `UndoQuarantineExecutor` safely for synthetic fixture files.
- ADR 0015 selects fixture-only selected restore execution before any real-profile selected restore workflow.

Implementation:

- Added ADR 0015 for fixture-only selected restore execution.
- Added WPF `Restore selected fixture manifest` action.
- WPF now marks selected restore execution available only when the selected Restore Manifest Cleanup Scope is recognized as a fixture.
- WPF calls `UndoQuarantineExecutor.Undo` for selected discovered fixture Restore Manifests after clean selected readiness and exact `RESTORE`.
- WPF shows selected restore result evidence and stale-state wording, then disables repeat selected restore for the current selected review.
- Custom non-fixture selected restore remains blocked even when `RESTORE` is typed.
- Kept real-profile selected restore, real-profile Quarantine execution, permanent deletion, cleanup history, and quarantine-folder cleanup unavailable.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors after rerunning by itself.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors after rerunning by itself.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0015-use-fixture-only-selected-restore-execution.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-fixture-only-selected-restore-execution.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0015-use-fixture-only-selected-restore-execution.md`.

Open questions:

- Should successful selected restore offer empty action-folder cleanup?
- What extra backup/manual review should real-profile selected restore require?
- Should selected restore refresh discovery automatically after execution or require the user to rediscover?

Rejected ideas buffer:

- Do not enable real-profile selected restore in this packet.
- Do not implement restore movement in WPF.
- Do not clean up action folders after selected restore.
- Do not let selected restore run more than once for the same current selected review.

### 2026-05-29: Add Selected Folder Child Focus

Status: completed

Evidence:

- The user's real scan screenshot showed large container rows and short/hash-like child names that are hard to reason about in a flat list.
- Existing Child Breakdown shows only a bounded detail-pane summary; the main grid still needed a direct way to focus on a selected folder's immediate children.
- Storage Review Search already provided an in-memory read-only lens that could be safely extended with parent matching.

Implementation:

- Added `parent:` Storage Review Search support through `StorageReviewSearchField.Parent`.
- Added WPF `Show children` selected-row action for folders.
- `Show children` resets review/category/type/size filters to All, applies `parent:<selected folder full path>`, focuses the grid on immediate child rows, and reports that no files were modified.
- Files keep `Show children` disabled.
- Kept real-profile cleanup execution, Quarantine execution changes, Undo changes, permanent deletion, cleanup history, and classification changes out of this packet.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed before rebuild.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed before rebuild.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuild.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuild.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `docs/features/2026-05-29-selected-folder-child-focus.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible in-memory review and WPF inspection improvement that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Open questions:

- Should a later tree/grid view replace Selected Folder Child Focus if real-profile review still feels too flat?
- Should repeated child focus show breadcrumbs?

Rejected ideas buffer:

- Do not build a full tree view before proving this smaller selected-folder focus action.
- Do not preserve active filters during `Show children`; hidden immediate children would make the action feel broken.
- Do not make child focus imply cleanup safety or shortlist approval.

### 2026-05-29: Add Storage Hotspot Trail

Status: completed

Evidence:

- The user's real scan screenshot showed large nested storage buckets where the next review step is understanding where size concentrates.
- Child Breakdown and Selected Folder Child Focus make immediate children inspectable, but the selected-row detail pane still lacked a fast largest-branch cue.
- Existing `StorageEntry.Children` can provide this in-memory without rescanning or changing cleanup eligibility.

Implementation:

- Added `StorageHotspotTrailEntry` and `StorageHotspotTrailBuilder`.
- Added WPF Largest hotspot trail detail text for selected rows.
- The trail follows the largest child at each level, stops at a terminal file/folder or bounded depth, and uses deterministic name tie-breaking.
- The WPF detail pane says trail sizes overlap and are not storage savings.
- Files explicitly report that they do not have descendant hotspot trails.
- Kept real-profile cleanup execution, Quarantine execution changes, Undo changes, permanent deletion, cleanup history, and classification changes out of this packet.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-storage-hotspot-trail.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible read-only review aid that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Open questions:

- Should a later version show multiple hotspot trails instead of only the single largest-child trail?
- Should the trail eventually support clickable rows that focus the grid on that child?

Rejected ideas buffer:

- Do not call the trail a savings estimate.
- Do not use the hotspot trail to auto-shortlist or recommend cleanup.
- Do not build a full tree view before manual review proves the smaller trail/focus tools are insufficient.

### 2026-05-29: Add Selected Folder Subtree Summary

Status: completed

Evidence:

- The user's real scan screenshot showed large container rows where the next decision depends on the descendant mix, not just parent size.
- Child Breakdown, Storage Hotspot Trail, and Selected Folder Child Focus help inspect shape, but the detail pane still lacked a compact descendant risk/count summary.
- Quarantine Preview already blocks broad parents with protected descendants, so a pre-preview read-only summary helps explain why broad parents should stay inspection-first.

Implementation:

- Added `StorageSubtreeReviewSummary` and `StorageSubtreeReviewSummaryBuilder`.
- Added WPF Descendant review summary detail text for selected rows.
- The summary excludes the selected folder itself, counts descendant files/folders, Importance Ratings, Quarantine candidates, Protected Location rows, Access issues, Reparse points, and Uncategorized Results.
- The summary shows bounded examples for Quarantine candidates, Protected Location rows, Access issues, and Uncategorized Results.
- The WPF detail pane says recursive row sizes overlap and are not storage savings or cleanup approval.
- Files explicitly report that they do not have descendant subtree summaries.
- Kept real-profile cleanup execution, Quarantine execution changes, Undo changes, permanent deletion, cleanup history, and classification changes out of this packet.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-selected-folder-subtree-summary.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible read-only review aid that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Open questions:

- Should the summary eventually include clickable review shortcuts for each descendant bucket?
- Should the summary appear as a compact table instead of text after manual layout review?

Rejected ideas buffer:

- Do not turn descendant summary counts into cleanup approval.
- Do not sum flattened recursive row sizes as savings.
- Do not add clickable bulk shortcuts before manual review proves the wording/layout works.

### 2026-05-29: Add Selected Folder Descendant Focus

Status: completed

Evidence:

- The user's real scan screenshot showed large flat results and nested buckets where immediate-child focus is useful but not always enough.
- Selected Folder Subtree Summary shows descendant risk/count context, but the grid still needed a recursive selected-folder lens for sorting, paging, filtering, shortlisting, and export.
- Existing Storage Review Search could support this safely as an in-memory read-only field prefix.

Implementation:

- Added `under:` Storage Review Search support through `StorageReviewSearchField.Under`.
- Added WPF `Show descendants` selected-row action for folders.
- `Show descendants` resets review/category/type/size filters to All, applies `under:<selected folder full path>`, focuses the grid on descendant rows, excludes the selected folder itself, and reports that no files were modified.
- Files keep `Show descendants` disabled.
- Kept real-profile cleanup execution, Quarantine execution changes, Undo changes, permanent deletion, cleanup history, and classification changes out of this packet.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `docs/features/2026-05-29-selected-folder-descendant-focus.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible in-memory review and WPF inspection improvement that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Open questions:

- Should descendant focus eventually preserve the active rating/category filters when invoked from a future summary bucket shortcut?
- Should descendant bucket shortcuts be added after a visible layout pass?

Rejected ideas buffer:

- Do not build a full tree view before proving this smaller recursive focus action.
- Do not preserve active filters during `Show descendants`; hidden descendants would make the action feel broken.
- Do not make descendant focus imply cleanup safety, storage savings, or shortlist approval.

### 2026-05-29: Add Matched Review Mix

Status: completed

Evidence:

- Selected Folder Descendant Focus and field-prefixed search make current review lenses more important.
- The whole-scan Review Mix does not explain the currently matched rows after `under:`, `parent:`, category, size, or access lenses are active.
- A read-only summary line can improve triage without adding cleanup execution or more buttons.

Implementation:

- Added WPF Matched Review Mix under Filter Summary.
- Matched Review Mix counts the full active matched set, not only the visible display window.
- The readout includes rows, Likely safe, Caution, High risk, Quarantine candidates, Protected, Access issues, and No category.
- The readout recomputes through the existing filter-summary refresh path and states that it is review context, not cleanup approval.
- Kept real-profile cleanup execution, Quarantine execution changes, Undo changes, permanent deletion, cleanup history, classification changes, and bucket shortcut buttons out of this packet.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-matched-review-mix.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible read-only UI summary that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Open questions:

- Should future bucket shortcuts be added from Matched Review Mix after visible layout review?
- Should Matched Review Mix eventually include largest-row sizes per bucket?

Rejected ideas buffer:

- Do not add clickable bucket shortcuts in this packet.
- Do not show matched byte sums as savings.
- Do not use Matched Review Mix to auto-shortlist or recommend cleanup.

### 2026-05-29: Debounce Storage Review Search typing

Status: completed

Evidence:

- User tested the real `C:\Users\moxhe` scan and reported sluggish typing in the Search box.
- Real-profile searches such as `cache`, `chrome`, and `user data` matched tens or hundreds of thousands of rows.
- The WPF `SearchBox_TextChanged` path refreshed the large result grid synchronously on every keystroke.
- User retested after the debounce change and reported that Search typing feels much better.
- User confirmed the status-bar message is enough; no separate visible Search pending indicator is needed right now.

Implementation:

- Added a 350 ms debounce for user-typed Storage Review Search text.
- Kept direct/programmatic search application immediate for tests, review shortcuts, and selected-folder focus flows.
- Reused the already computed matched result set when updating the post-scan filter summary, avoiding one duplicate large-filter pass.
- Preserved read-only behavior; no scan, cleanup, quarantine, restore, or classification behavior changed.

Verification:

- `dotnet build WindowsFileCleaner.sln` passed with 0 warnings and 0 errors after the running app was closed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj` passed.

Docs updated:

- `docs/domain/context.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible UI responsiveness change with no persistence, cleanup execution, security, or data-model impact.

Open questions:

- None for this packet.

Rejected ideas buffer:

- Do not reduce search coverage just to improve speed.
- Do not make search require pressing Enter unless real-profile typing remains sluggish after debounce.
- Do not add a separate visible Search pending indicator unless future testing shows the status bar is insufficient.

### 2026-05-29: Add fresh-thread handoff note

Status: completed

Evidence:

- User said the current thread is getting slow and asked for a handoff polish pass plus a prompt for a new thread.
- Repo was clean on `main` tracking `origin/main` before the handoff packet.
- Latest pushed functional packet was `ce7c1db Debounce storage review search`, with GitHub Actions MVP Preflight passing.

Implementation:

- Added `docs/codex/thread-handoff.md` with current state, safety boundary, recent verification, best next work, commands, and a startup prompt.
- Linked the handoff note from `README.md`.
- Updated the progress log current status and next recommended work to point new threads at the handoff note.
- Added README wording that user-typed Storage Review Search is debounced and uses the status bar as the pending-search indicator.

Verification:

- Docs-only packet; no production code changed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is handoff documentation only.

Open questions:

- None.

Rejected ideas buffer:

- Do not hide the real-profile cleanup execution gaps in the handoff prompt.
- Do not ask the next thread to continue from memory; require it to inspect current files and git state.

### 2026-05-29: Add Review Shortlist Safety Mix

Status: completed

Evidence:

- User asked this thread to prefer Review Shortlist safety context before any real cleanup execution.
- Review Shortlist is already an in-memory review aid, and Quarantine Preview remains the separate dry-run readiness step.
- WPF app smoke tests already cover shortlist actions, making them the narrowest relevant verification surface.

Implementation:

- Added WPF `ShortlistSafetyMixText` beneath the Review Shortlist/Quarantine toolbar.
- The readout summarizes shortlisted rows by Likely safe, Caution, High risk, Quarantine candidates, Protected, Access issues, No category, and largest shortlisted row.
- Empty, removed, and cleared shortlist states return to empty wording.
- Kept all behavior read-only and derived only from completed Storage Scan review rows.
- Kept real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, persisted cleanup history, and cleanup eligibility unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-shortlist-safety-mix.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible read-only UI context improvement with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later layout pass convert Review Shortlist Safety Mix into compact chips or a table if the visible line feels too dense?

Rejected ideas buffer:

- Do not treat Shortlist Safety Mix as Quarantine Preview readiness.
- Do not show shortlisted row sizes as confirmed storage savings.

### 2026-05-29: Add Scan Gate Discoverability Polish

Status: completed

Evidence:

- The real-profile scan gate was functionally correct, but its visible locked/ready state relied on quiet checkbox and gray text.
- Handoff guidance lists scan-gate discoverability as preferred safety work before real cleanup execution.
- WPF startup tests already cover real-profile and fixture gate states.

Implementation:

- Added a visible `ScanGateSummaryText` line in the WPF header.
- Added `Scan` button tooltip wording for locked real-profile, acknowledged real-profile, fixture, and custom/invalid states.
- Kept the existing real-profile acknowledgement requirement, scan-start enforcement, and fixture/custom behavior unchanged.
- Kept all behavior read-only; no scan was run against the real profile and no cleanup execution was added.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/features/2026-05-29-scan-gate-discoverability-polish.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible WPF wording/discoverability improvement with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the extra header line fit comfortably during the next visible fixture and real-profile review pass?

Rejected ideas buffer:

- Do not replace the acknowledgement checkbox with a modal until manual review proves the inline gate is still easy to miss.
- Do not run preflight from the WPF app as part of scan-gate polish.

### 2026-05-29: Add Quarantine Execution Scope Status

Status: completed

Evidence:

- Handoff guidance recommends Quarantine Preview/readiness clarity before any real cleanup execution.
- Existing WPF tests proved custom non-fixture execution stayed unavailable, but visible wording leaned on technical `Execution implemented: no` text.
- Fixture and custom Quarantine Preview/Gate flows already have narrow WPF smoke coverage.

Implementation:

- Added plain-language `Execution scope status` lines to Quarantine Preview output.
- Added the same scope-status wording to Quarantine Execution Gate output.
- Fixture scopes now state fixture-only execution is available only after preview readiness and exact `QUARANTINE` confirmation.
- Custom and real-profile-style non-fixture scopes state the workflow is preview-only and real-profile/custom execution remains unavailable.
- Kept existing `Execution implemented` lines for continuity and did not change eligibility, confirmation, execution, restore, or persistence behavior.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-execution-scope-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF wording/readiness clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the extra line make the Quarantine Preview/Gate panes easier to scan during the next visible fixture review?

Rejected ideas buffer:

- Do not replace the technical `Execution implemented` line before manual review confirms the new wording is sufficient.
- Do not use scope-status wording as permission to enable real-profile execution.

### 2026-05-29: Add Fixture Review Checklist Output

Status: completed

Evidence:

- The next recommended work remains a visible fixture review pass.
- `Start-MvpFixtureReview.ps1` already launched the safe fixture workflow, but the detailed review checklist lived in README rather than the terminal where the user starts the pass.
- Recent packets added scan-gate, shortlist, and execution-scope wording that should be checked together during fixture review.

Implementation:

- Added a compact manual fixture review checklist to `tools/Start-MvpFixtureReview.ps1`.
- The checklist covers fixture scope wording, manual Scan, no-files-modified status, review summaries, search/focus actions, Review Shortlist Safety Mix, Quarantine Preview, Quarantine Execution Scope Status, fixture-only execution/undo, manifest review, and real/custom blockers.
- Added `-SkipChecklist` for focused loops.
- Kept the launcher from scanning automatically, scanning real-profile files, or modifying anything during `-WhatIf`.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and printed the checklist without creating fixture files or launching WPF.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch -SkipChecklist` passed and omitted the checklist for focused loops.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-28-mvp-fixture-review-launcher.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a local workflow-output improvement with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- After the next visible fixture review, should the checklist be shortened or split into grouped prompts?

Rejected ideas buffer:

- Do not make the launcher click Scan automatically.
- Do not replace the full README checklist with only the compact launcher checklist.

### 2026-05-29: Add Fixture Review Checklist-Only Mode

Status: completed

Evidence:

- The next recommended work remains a visible fixture review pass.
- The compact launcher checklist is useful as a terminal prompt, but the user may want to print it without running preflight, creating fixture files, or launching WPF.
- The fixture launcher already resolves and validates the fixture root before doing work.

Implementation:

- Added `-ChecklistOnly` to `tools/Start-MvpFixtureReview.ps1`.
- Checklist-only mode prints the resolved Fixture Cleanup Scope and manual review checklist, then exits before preflight, fixture creation, or WPF launch.
- Kept the existing repository containment guard for the fixture root.
- Kept real-profile scanning and cleanup execution unchanged.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly -FixtureRoot ".local\storage-scan-smoke-fixture"` passed and resolved the fixture path inside the repo.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the checklist in dry-run launcher output.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch -SkipChecklist` passed and omitted the checklist for focused loops.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-review-checklist-only-mode.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a local workflow-output option with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- After the next visible fixture review, should the checklist be shortened or split into grouped prompts?

Rejected ideas buffer:

- Do not make checklist-only mode create fixture files.
- Do not make the launcher click Scan automatically.

### 2026-05-29: Add Quarantine Approval Boundary Wording

Status: completed

Evidence:

- Handoff guidance recommends Quarantine Preview/readiness clarity before real cleanup execution.
- Existing preview/gate panes showed execution scope status, but the approval boundary was spread across surrounding wording.
- WPF smoke tests already exercise fixture and custom non-fixture Quarantine Preview/Gate panes.

Implementation:

- Added an `Approval boundary:` line to Quarantine Preview output.
- Added the same approval-boundary line to Quarantine Execution Gate output.
- Fixture wording says Review Shortlist and Quarantine Preview are not cleanup approval and exact `QUARANTINE` can open only fixture execution in this build.
- Preview-only wording says Review Shortlist and Quarantine Preview are not cleanup approval and real-profile/custom execution remains unavailable.
- Kept real-profile Quarantine execution, custom execution, restore behavior, permanent deletion, cleanup history, and eligibility rules unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-29-quarantine-approval-boundary-wording.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF wording/readiness clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the extra line make the preview/gate panes easier to scan during the next visible fixture review?

Rejected ideas buffer:

- Do not use approval-boundary wording as permission to enable real-profile execution.
- Do not remove the existing execution scope status line before manual review.

### 2026-05-29: Add Fixture Checklist Approval Boundary

Status: completed

Evidence:

- The latest WPF Quarantine Preview/Gate panes now show `Approval boundary:` wording.
- The fixture launcher checklist still told the user to check Execution scope status, but not the new approval-boundary line.
- The next recommended work remains manual fixture review.

Implementation:

- Updated `tools/Start-MvpFixtureReview.ps1` checklist line 6 to mention Approval boundary plus Execution scope status.
- Updated README and feature notes so fixture-review prompts match the latest WPF panes.
- Kept real-profile scanning, fixture creation, WPF launch, Quarantine Preview, execution, restore, and cleanup eligibility behavior unchanged.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the updated checklist in dry-run launcher output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-checklist-approval-boundary.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local checklist wording with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- After visible fixture review, should the checklist be shortened or split into grouped prompts?

Rejected ideas buffer:

- Do not add more checklist items for every WPF line; keep the terminal prompt compact.

### 2026-05-29: Add Confirmation Label Wording

Status: completed

Evidence:

- Fixture-only Quarantine execution and fixture-only selected restore now exist.
- WPF Quarantine Preview and Selected Restore panes still said `Required future text`, which was stale and could make current fixture-only gates harder to understand.
- Existing WPF smoke tests already cover Quarantine Preview/Gate and Selected Restore Gate wording.

Implementation:

- Changed WPF Quarantine Confirmation Draft output to `Required confirmation text: QUARANTINE`.
- Changed WPF Selected Restore Confirmation Draft output to `Required confirmation text: RESTORE`.
- Added WPF smoke assertions that the stale `Required future text` label is absent from those panes.
- Updated current docs and test wording to use required confirmation text.
- Kept confirmation phrases, execution gates, fixture-only execution, real-profile blockers, restore behavior, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- Initial parallel core test attempts hit transient Windows build-output file locks while another .NET command was building; sequential verification passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0014-use-read-only-selected-restore-confirmation-gate.md`
- `docs/features/2026-05-28-quarantine-confirmation-draft.md`
- `docs/features/2026-05-28-quarantine-readiness-ui.md`
- `docs/features/2026-05-29-confirmation-label-wording.md`
- `docs/features/2026-05-29-quarantine-execution-gate.md`
- `docs/features/2026-05-29-selected-restore-confirmation-gate.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible wording alignment with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- None for this packet.

Rejected ideas buffer:

- Do not rename model properties or change gate semantics for a visible-label polish packet.

### 2026-05-29: Add Selected Restore Scope Status

Status: completed

Evidence:

- Fixture-only selected restore execution exists, but the Selected Restore Execution Gate pane relied on technical `Execution implemented` wording to explain fixture-only versus preview-only behavior.
- Quarantine Preview/Gate panes already show `Execution scope status` and `Approval boundary` lines, which made selected restore the remaining similar safety-gate wording gap.
- Existing WPF smoke tests cover fixture selected restore and custom non-fixture selected restore blocking.

Implementation:

- Added `Execution scope status` and `Approval boundary` lines to Selected Restore Execution Gate output.
- Fixture wording says fixture-only selected restore is available only after selected readiness and exact `RESTORE` confirmation.
- Preview-only wording says real-profile and custom selected restore remain unavailable.
- Kept selected restore gate behavior, fixture-only execution, real-profile blockers, custom blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selected-restore-scope-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF wording/readiness clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the extra selected restore gate wording make the pane easier to scan during the next visible fixture review?

Rejected ideas buffer:

- Do not use selected restore scope-status wording as permission to enable real-profile selected restore.
- Do not remove the technical `Execution implemented` line before manual review confirms the clearer wording is sufficient.

### 2026-05-29: Add Fixture Checklist Selected Restore Scope

Status: completed

Evidence:

- The latest WPF Selected Restore Execution Gate panes now show `Execution scope status` and `Approval boundary` wording.
- The fixture launcher checklist still referred to selected readiness/gate generically and did not tell the user to check the new selected restore scope-status line.
- The next recommended work remains manual fixture review.

Implementation:

- Updated `tools/Start-MvpFixtureReview.ps1` checklist line 8 to call out selected restore `Approval boundary` plus `Execution scope status`.
- Updated checklist line 9 to explicitly name both Quarantine and selected restore execution blockers for real-profile/custom scopes.
- Updated README and feature notes so fixture-review prompts match the latest WPF selected restore pane.
- Kept real-profile scanning, fixture creation, WPF launch, Quarantine execution, selected restore execution, permanent deletion, and cleanup history behavior unchanged.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the updated checklist in dry-run launcher output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-selected-restore-scope-status.md`
- `docs/features/2026-05-29-fixture-checklist-selected-restore-scope.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local checklist wording with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- After visible fixture review, should selected restore scope-status wording stay in the compact checklist or move only to README?

Rejected ideas buffer:

- Do not add a separate checklist item for every selected restore line; keep the terminal prompt compact.

### 2026-05-29: Add Broad Restore Action Wording

Status: completed

Evidence:

- Fixture-only selected restore execution now exists for selected discovered fixture manifests.
- Quarantine Manifest Discovery, Selected Restore Manifest Review, and Restore Readiness Preview still used generic `No restore action is available` wording, which could be read as contradicting fixture-only selected restore.
- Existing WPF smoke tests cover discovery, selected review, readiness preview, selected restore gate, and fixture-only selected restore execution.

Implementation:

- Updated Quarantine Manifest Discovery output to say no broad restore action is available from discovery and fixture selected restore must go through selected readiness and the selected restore gate.
- Updated Restore Readiness Preview output with the same broad-restore boundary.
- Updated Selected Restore Manifest Review output to say selected review is readiness evidence only and fixture selected restore must go through the selected restore gate.
- Kept discovery, readiness preview, selected review, selected restore gate behavior, fixture-only execution, real-profile blockers, custom blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-broad-restore-action-wording.md`
- `docs/features/2026-05-29-quarantine-manifest-discovery.md`
- `docs/features/2026-05-29-restore-readiness-preview.md`
- `docs/features/2026-05-29-selected-restore-manifest-review.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF wording/readiness clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does `broad restore action` read clearly during visible fixture review?

Rejected ideas buffer:

- Do not use broad-restore wording as permission to add broad WPF Undo Quarantine.

### 2026-05-29: Add Fixture Checklist Broad Restore Wording

Status: completed

Evidence:

- The latest WPF discovery/readiness/selected-review panes now distinguish no broad restore action from fixture-only selected restore.
- The fixture launcher checklist still told the user to use discovery/readiness/gate but did not explicitly tell them to check the no-broad-restore wording.
- The next recommended work remains manual fixture review.

Implementation:

- Updated `tools/Start-MvpFixtureReview.ps1` checklist line 8 to call out no broad restore action plus selected restore Approval boundary and Execution scope status.
- Updated README and feature notes so fixture-review prompts match the latest WPF restore panes.
- Kept real-profile scanning, fixture creation, WPF launch, Quarantine execution, selected restore execution, permanent deletion, and cleanup history behavior unchanged.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the updated checklist in dry-run launcher output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-broad-restore-action-wording.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-checklist-broad-restore-wording.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local checklist wording with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Is `no broad restore action` clear enough in the compact checklist?

Rejected ideas buffer:

- Do not add another checklist line for every restore pane; keep the terminal prompt compact until visible review proves it needs splitting.

### 2026-05-29: Add All-Manifest Restore Wording

Status: completed

Evidence:

- The latest WPF discovery/readiness panes used `broad restore action` to distinguish all-manifest restore from fixture selected restore.
- That wording was accurate but internal; `all-manifest restore action` names the unavailable action more concretely.
- Existing WPF smoke tests cover discovery and readiness wording.

Implementation:

- Updated Quarantine Manifest Discovery and Restore Readiness Preview output to say no all-manifest restore action is available.
- Updated WPF smoke assertions for the new wording.
- Updated the fixture launcher checklist to use no all-manifest restore action wording.
- Updated README, domain docs, related feature notes, and handoff.
- Kept discovery/readiness preview behavior, selected restore gate behavior, fixture-only execution, real-profile blockers, custom blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the all-manifest restore checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-all-manifest-restore-wording.md`
- `docs/features/2026-05-29-broad-restore-action-wording.md`
- `docs/features/2026-05-29-fixture-checklist-broad-restore-wording.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-quarantine-manifest-discovery.md`
- `docs/features/2026-05-29-restore-readiness-preview.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF wording/readiness clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- If all-manifest restore is ever designed, should the button use `Restore all discovered manifests` or another explicit label?

Rejected ideas buffer:

- Do not add an all-manifest restore domain term before execution design exists.

### 2026-05-29: Add Visible Row Shortlist Labels

Status: completed

Evidence:

- Review Shortlist bulk actions already operate only on the current Storage Review Display Window.
- Existing button/status wording used `shown`, while README/domain language needed to reinforce visible-row scope before any cleanup execution.
- WPF smoke tests already exercise bulk add/remove behavior.

Implementation:

- Renamed WPF bulk shortlist buttons to `Shortlist visible rows` and `Remove visible rows`.
- Updated bulk shortlist status text to say visible rows.
- Updated fixture launcher checklist wording to prompt visible-row shortlist label review.
- Added WPF smoke assertions for the visible-row button labels.
- Updated README, domain docs, related feature notes, and handoff.
- Kept Review Shortlist behavior read-only, in-memory, and limited to displayed rows; no Quarantine Preview, execution, restore, deletion, or cleanup history behavior changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the visible-row shortlist label review prompt without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-visible-row-shortlist-labels.md`
- `docs/features/2026-05-28-shortlist-shown-review-rows.md`
- `docs/features/2026-05-28-storage-review-display-window.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI wording clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later layout pass add icons or tooltips for Review Shortlist bulk actions?

Rejected ideas buffer:

- Do not rename internal method names in this packet.
- Do not make the action apply to all matched rows.

### 2026-05-29: Add Scope-Specific Scan Gate Ready Wording

Status: completed

Evidence:

- Cleanup Scope Scan Gate behavior was correct, but non-real ready wording was generic even though fixture-only cleanup actions exist later behind preview and exact confirmation gates.
- Handoff guidance prefers scan-gate discoverability before any real cleanup execution.
- Core and WPF tests already cover scan-gate states.

Implementation:

- Updated core scan-gate ready messages for acknowledged real-profile, fixture, and custom scopes.
- Updated WPF scan-gate summaries and Scan tooltips with scope-specific execution-boundary wording.
- Added core and WPF smoke assertions for real-profile, fixture, and custom ready states.
- Updated fixture launcher checklist wording to mention gated fixture cleanup actions.
- Kept scan enablement, acknowledgement behavior, Storage Scan, Quarantine Preview, execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the fixture cleanup actions stay gated checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-scope-specific-scan-gate-ready-wording.md`
- `docs/features/2026-05-29-scan-gate-discoverability-polish.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible wording clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the longer scope-specific header wording fit comfortably during visible fixture and real-profile review?

Rejected ideas buffer:

- Do not make fixture scan readiness imply cleanup approval.
- Do not hide fixture-only cleanup execution by saying no cleanup execution exists for fixture scopes.

### 2026-05-29: Add Execution Control Tooltip Clarity

Status: completed

Evidence:

- Selected restore confirmation tooltip still said discovered-manifest restore execution was unavailable even though fixture selected restore now exists after selected readiness and exact `RESTORE`.
- Quarantine execution and undo buttons did not carry their own tooltip boundary text.
- WPF smoke tests already cover startup disabled controls and selected fixture restore flow.

Implementation:

- Updated Quarantine confirmation, Execute quarantine, Undo fixture quarantine, selected restore confirmation, and Restore selected fixture manifest tooltips.
- Enabled disabled-control tooltip display for those execution gate controls.
- Added WPF smoke assertions for startup tooltip wording and enabled fixture selected restore tooltip wording.
- Updated fixture launcher checklist wording to include execution and restore tooltip review.
- Kept Quarantine Preview, Quarantine Execution Gate, Selected Restore Execution Gate, fixture-only execution, restore, real-profile blockers, custom blockers, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed execution/restore tooltip review prompts without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-execution-control-tooltip-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should future layout polish add inline help icons for execution gates if tooltips are not discoverable enough?

Rejected ideas buffer:

- Do not add new execution paths.
- Do not rely only on pane text when disabled controls can carry their own safety context.

### 2026-05-29: Add Undo Quarantine Domain Consistency

Status: completed

Evidence:

- Current WPF app can undo the current fixture execution and run fixture-only selected restore through `UndoQuarantineExecutor`.
- `docs/domain/context.md` still said Undo Quarantine Executor was fixture-tested but not wired to WPF yet.
- ADR 0010 context also read like current state rather than decision-time state.

Implementation:

- Updated Undo Quarantine Executor current-state wording in domain context.
- Clarified ADR 0010 context as decision-time wording.
- Preserved real-profile WPF Undo Quarantine and all-manifest discovered-manifest restore blockers.
- Left older feature briefs historical when their stale-sounding wording describes packet-start behavior.

Verification:

- `rg -n "not wired to the WPF|not wired to WPF|does not expose undo yet|Keep WPF Undo Quarantine unavailable" docs/domain docs/decisions/0010-use-current-fixture-execution-wpf-undo.md` found no current-state stale wording.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `docs/domain/context.md`
- `docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`
- `docs/features/2026-05-29-undo-quarantine-domain-consistency.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a docs consistency correction, not a new durable decision.

Open questions:

- Should future docs distinguish current-state domain context from historical ADR/feature context more explicitly?

Rejected ideas buffer:

- Do not rewrite old feature briefs that accurately describe packet-start behavior.
- Do not turn this docs correction into a real-profile undo design packet.

### 2026-05-29: Add Restore Manifest Wording Polish

Status: completed

Evidence:

- Current-facing README, WPF tooltip, and WPF smoke assertion messages still used `old manifest` wording.
- The glossary already uses Restore Manifest as the preferred term.
- Handoff guidance prefers readiness clarity before any real cleanup execution.

Implementation:

- Updated the disabled current-fixture undo tooltip to refer to discovered Restore Manifests.
- Updated README manual check wording to say discovery should state that no all-manifest restore action is available.
- Updated WPF smoke assertion messages to use Restore Manifest/all-manifest restore wording.
- Updated domain context and glossary wording for current undo and selected restore boundaries.
- Kept Quarantine Manifest Discovery, Restore Readiness Preview, current-fixture undo, fixture-only selected restore, real-profile blockers, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the all-manifest restore checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-restore-manifest-wording-polish.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible wording clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- If all-manifest restore is designed later, what should the final button label be?

Rejected ideas buffer:

- Do not rename historical ADR rationale wholesale in this packet.
- Do not introduce a new `discovered-manifest restore` term when `Restore Manifest` already exists.

### 2026-05-29: Add Selected Manifest Readiness Label Polish

Status: completed

Evidence:

- The visible WPF action label `Preview selected readiness` was compact but did not name the selected Restore Manifest.
- README, checklist output, domain docs, and tests used the same shorter selected-readiness wording in current-facing guidance.
- Handoff guidance prefers manual fixture/review clarity before any real cleanup execution.

Implementation:

- Renamed the WPF action to `Preview selected manifest readiness` and widened the button.
- Updated placeholder text, discovery/readiness pane wording, selected restore tooltip, and selected restore scope-status text to use selected manifest readiness wording.
- Added WPF smoke assertions for the button label and related pane/tooltip wording.
- Updated core assertion messages and a selection blocker string to use selected manifest readiness wording.
- Updated README, domain docs, fixture checklist, relevant feature notes, and handoff.
- Kept manifest discovery, selected review, restore readiness preview, fixture-only selected restore, real-profile blockers, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the selected manifest readiness/gate checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selected-manifest-readiness-label-polish.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-restore-manifest-wording-polish.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI wording clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the longer button label fit comfortably during visible fixture review?

Rejected ideas buffer:

- Do not rename core types or workflows in this packet.
- Do not use the shorter `selected readiness` label again unless visible review shows the longer label does not fit.

### 2026-05-29: Add All-Manifest Readiness Label Polish

Status: completed

Evidence:

- `Preview restore readiness` was accurate but less explicit after the selected-manifest readiness label became `Preview selected manifest readiness`.
- ADR 0012 and the Restore Readiness Preview feature brief describe this workflow as evaluating discovered Restore Manifests under the selected Quarantine Root.
- Handoff guidance prefers readiness clarity before any real cleanup execution.

Implementation:

- Renamed the WPF Restore Readiness Preview action to `Preview all-manifest readiness`.
- Updated the readiness placeholder text and added WPF smoke assertions for the button label and placeholder.
- Updated README, domain docs, fixture checklist, relevant feature notes, progress, and handoff.
- Kept Restore Readiness Preview read-only and kept all restore execution availability boundaries unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the all-manifest readiness preview checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-all-manifest-readiness-label-polish.md`
- `docs/features/2026-05-29-restore-readiness-preview.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI wording clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the longer all-manifest readiness label fit comfortably during visible fixture review?

Rejected ideas buffer:

- Do not rename the domain concept away from Restore Readiness Preview.
- Do not add all-manifest restore while polishing a read-only readiness label.

### 2026-05-29: Add Readiness Scope Tooltip Clarity

Status: completed

Evidence:

- The readiness button labels now distinguish selected manifest readiness from all-manifest readiness, but the controls did not have dedicated tooltips explaining scope and approval boundaries.
- The selected manifest readiness button starts disabled before discovery, so its boundary wording should remain available via disabled-state tooltip behavior.
- Handoff guidance prefers manual fixture/review clarity before any real cleanup execution.

Implementation:

- Added a tooltip to `Preview all-manifest readiness` that says it is read-only for discovered Restore Manifests under the selected Quarantine Root and restores no files.
- Added a disabled-state tooltip to `Preview selected manifest readiness` that says it reviews the selected Restore Manifest only and is not restore approval.
- Added WPF smoke assertions for both readiness tooltips, including the enabled selected-manifest state after discovery.
- Updated README, domain docs, fixture checklist, progress, and handoff.
- Kept Restore Readiness Preview, Selected Restore Manifest Review, fixture-only selected restore, real-profile blockers, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the readiness scope tooltips checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-readiness-scope-tooltip-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- During visible fixture review, are the tooltips discoverable enough or should future polish add inline help icons?

Rejected ideas buffer:

- Do not add restore execution or approval semantics while polishing read-only readiness tooltips.

### 2026-05-29: Add Visible-Row Shortlist Tooltip Clarity

Status: completed

Evidence:

- Visible-row shortlist labels made scope clearer, but the controls did not expose their no-file-modified and not-cleanup-approval boundary before use.
- The visible-row label feature left tooltips as an open follow-up.
- Handoff guidance prefers Review Shortlist safety context before any real cleanup execution.

Implementation:

- Added disabled-state WPF tooltips to `Shortlist visible rows` and `Remove visible rows`.
- Added test-facing tooltip accessors and WPF smoke assertions for visible-row scope, not-cleanup-approval wording, and no-file-modified wording.
- Updated README, domain docs, fixture checklist, related feature notes, progress, and handoff.
- Kept Review Shortlist read-only and in-memory; no Quarantine Preview, execution, restore, deletion, or cleanup history behavior changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the visible-row shortlist labels/tooltips checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-visible-row-shortlist-tooltip-clarity.md`
- `docs/features/2026-05-29-visible-row-shortlist-labels.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- During visible fixture review, are hover tooltips discoverable enough or should a later keyboard-accessibility pass add explicit help/focus text?

Rejected ideas buffer:

- Do not add icons, persistent Review Shortlist behavior, or cleanup approval semantics while polishing visible-row tooltip clarity.

### 2026-05-29: Add Review Toolbar Report and Preview Tooltip Clarity

Status: completed

Evidence:

- The visible-row shortlist controls had safety tooltips, but adjacent report/clear/preview controls in the same toolbar did not expose equivalent pre-use boundaries.
- The toolbar includes actions that write reports, clear in-memory review state, or build dry runs, so the distinction from cleanup approval should be visible before use.
- Handoff guidance prefers manual review polish and Review Shortlist safety context before any real cleanup execution.

Implementation:

- Added disabled-state WPF tooltips to `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview`.
- Added test-facing tooltip accessors and WPF smoke assertions for report-only, in-memory-only, dry-run, and no-file-modified wording.
- Updated README, domain docs, fixture checklist, progress, and handoff.
- Kept Review Shortlist, CSV export, Quarantine Preview, cleanup execution, restore, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed Review Shortlist labels/tooltips plus preview/export tooltip checks without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-toolbar-report-preview-tooltip-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- During visible fixture review, are toolbar tooltips discoverable enough or should a later keyboard-accessibility pass add focus/help text?

Rejected ideas buffer:

- Do not add always-visible help text, export behavior changes, or cleanup approval semantics while polishing report/preview tooltip clarity.

### 2026-05-29: Add Selection Browse Tooltip Clarity

Status: completed

Evidence:

- Cleanup Scope Selection and Quarantine Root Selection already had safety notes, but the `Browse...` buttons did not expose their path-only or preview-only boundaries before use.
- Cleanup Scope Selection must stay separate from `Scan` and the real-profile scan gate.
- Quarantine Root Selection must stay separate from folder creation, file movement, and cleanup approval.

Implementation:

- Added disabled-state WPF tooltips to Cleanup Scope and Quarantine Root `Browse...` buttons.
- Added test-facing tooltip accessors and WPF smoke assertions for path-only selection, real-profile scan-gate, preview-only, no-folder-creation, and no-approval wording.
- Updated README, domain docs, fixture checklist, progress, and handoff.
- Kept browsing, Storage Scan, Quarantine Preview, cleanup execution, restore, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed Cleanup Scope and Quarantine Root browse tooltip checks without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selection-browse-tooltip-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- During visible fixture review, are browse tooltips discoverable enough or should a later keyboard-accessibility pass add focus/help text?

Rejected ideas buffer:

- Do not add modal warnings, Quarantine Root folder creation/probing, or scan-gate changes while polishing browse tooltip clarity.

### 2026-05-29: Add Selected-Row Action Tooltip Clarity

Status: completed

Evidence:

- Recent packets clarified the main Review Shortlist toolbar, report/preview controls, and browse controls.
- The selected-row action strip still lacked comparable pre-use boundary wording for shortlist, focus, preview, copy, and Explorer inspection actions.
- Domain docs already define these actions as read-only, selected-only, or inspection-only.

Implementation:

- Added disabled-state WPF tooltips to selected-row shortlist, copy, focus, preview, and Explorer actions.
- Added test-facing tooltip accessors and WPF smoke assertions for selected-only, read-only focus, bounded preview, manual inspection, no-file-modified, and not-cleanup-approval wording.
- Updated README, domain docs, fixture checklist, progress, and handoff.
- Kept selected-row action behavior, Storage Scan, Review Shortlist, Quarantine Preview, cleanup execution, restore, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the selected-row tooltip checklist item without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selected-row-action-tooltip-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- During visible fixture review, are selected-row action tooltips discoverable enough or should a later keyboard-accessibility pass add focus/help text?

Rejected ideas buffer:

- Do not add always-visible helper text, focus behavior changes, or cleanup approval semantics while polishing selected-row action tooltips.

### 2026-05-29: Add Review Navigation and Export Tooltip Clarity

Status: completed

Evidence:

- Recent packets clarified selected-row actions, browse controls, and Review Shortlist/report/preview toolbar controls.
- The main review toolbar still had read-only navigation/export controls without pre-use tooltip boundaries.
- Domain docs already define these controls as read-only, in-memory, or report-only.

Implementation:

- Added disabled-state WPF tooltips to `Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows`.
- Added test-facing tooltip accessors and WPF smoke assertions for report-only, no-rescan, in-memory display-window, Review Shortlist-preserving, no-file-modified, and not-cleanup-approval wording.
- Updated README, domain docs, fixture checklist, progress, and handoff.
- Kept Storage Review Search, Review View Reset, Storage Review Display Window, Scan Report Export, cleanup execution, restore, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the review navigation/export tooltip checklist item without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-navigation-export-tooltip-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- During visible fixture review, are review navigation/export tooltips discoverable enough or should a later keyboard-accessibility pass add focus/help text?

Rejected ideas buffer:

- Do not change export row selection, debounce indicators, display-window size, or cleanup approval semantics while polishing review navigation/export tooltips.

### 2026-05-29: Add Review Toolbar Automation Help Text

Status: completed

Evidence:

- Review Navigation and Export Tooltip Clarity made the main review toolbar boundaries visible by hover tooltip.
- The next small polish gap was non-hover discoverability for the same safety boundaries.
- Domain docs already define `Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows` as read-only, in-memory, or report-only controls.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to `Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows`.
- Added test-facing help-text accessors and WPF smoke assertions for report-only, no-rescan, in-memory display-window, Review Shortlist-preserving, no-file-modified, and not-cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Storage Review Search, Review View Reset, Storage Review Display Window, Scan Report Export, cleanup execution, restore, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-toolbar-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later accessibility pass add automation help text to the remaining safety-critical tooltiped controls?

Rejected ideas buffer:

- Do not add visible helper text, alter focus order, change enablement, or broaden this packet to every tooltiped control while proving the review-toolbar help-text pattern.

### 2026-05-29: Add Review Report and Preview Automation Help Text

Status: completed

Evidence:

- Review Toolbar Report and Preview Tooltip Clarity made `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview` boundaries visible by hover tooltip.
- The previous automation-help-text packet proved the same non-hover pattern on the main review navigation/export controls.
- Domain docs already define Review Shortlist as in-memory review context and Quarantine Preview as a dry run.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview`.
- Added test-facing help-text accessors and WPF smoke assertions for report-only, in-memory-only, dry-run, no-file-modified, and not-cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Review Shortlist behavior, Quarantine Preview eligibility/output, CSV export behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-report-preview-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later accessibility pass add automation help text to selected-row, browse, execution-gate, and restore-readiness controls?

Rejected ideas buffer:

- Do not add visible helper text, alter focus order, change enablement, or broaden this packet beyond report/preview controls while proving the help-text pattern.

### 2026-05-29: Add Selected Row Action Automation Help Text

Status: completed

Evidence:

- Selected Row Action Tooltip Clarity made selected-row action boundaries visible by hover tooltip.
- Recent automation-help-text packets proved the same non-hover pattern on main review toolbar and report/preview controls.
- Domain docs already define selected-row actions as review-only, focus-only, inspection-only, or bounded read-only preview.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to `Add to shortlist`, `Remove`, `Copy path`, `Show children`, `Show descendants`, `Preview file`, and `Open in Explorer`.
- Added test-facing help-text accessors and WPF smoke assertions for selected-only, read-only focus, bounded preview, manual inspection, no-file-modified, and not-cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept selected-row actions, Review Shortlist behavior, focus/search behavior, file preview behavior, clipboard/Explorer behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selected-row-action-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later accessibility pass add automation help text to browse, execution-gate, and restore-readiness controls?

Rejected ideas buffer:

- Do not add visible helper text, alter focus order, change enablement, or broaden this packet beyond selected-row controls while proving the help-text pattern.

### 2026-05-29: Add Selection Browse Automation Help Text

Status: completed

Evidence:

- Selection Browse Tooltip Clarity made Cleanup Scope and Quarantine Root browse boundaries visible by hover tooltip.
- Recent automation-help-text packets proved the same non-hover pattern on review toolbar, report/preview, and selected-row controls.
- Domain docs already define Cleanup Scope browsing as path selection only and Quarantine Root browsing as preview-root selection only.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to Cleanup Scope and Quarantine Root `Browse...` controls.
- Added test-facing help-text accessors and WPF smoke assertions for path-only, preview-only, no-scan, no-folder-creation, no-file-modified, scan-gate, and no-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Cleanup Scope Selection behavior, Quarantine Root Selection behavior, scan gating, preview-root behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selection-browse-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later accessibility pass add automation help text to execution-gate and restore-readiness controls?

Rejected ideas buffer:

- Do not add visible helper text, alter focus order, change enablement, weaken scan gates, probe folders, or broaden this packet beyond browse controls while proving the help-text pattern.

### 2026-05-29: Add Execution and Readiness Automation Help Text

Status: completed

Evidence:

- Execution Control Tooltip Clarity and Readiness Scope Tooltip Clarity made gate/readiness boundaries visible by hover tooltip.
- Recent automation-help-text packets proved the same non-hover pattern on review toolbar, report/preview, selected-row, and browse controls.
- Domain docs already define Quarantine execution, current-fixture undo, Restore Readiness Preview, Selected Restore Manifest Review, and Selected Restore Execution Gate boundaries.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to Quarantine confirmation, Execute quarantine, Undo fixture quarantine, Preview all-manifest readiness, Preview selected manifest readiness, selected restore confirmation, and Restore selected fixture manifest controls.
- Added a tooltip and matching automation help text to `Preview selected restore gate`.
- Added test-facing help-text accessors and WPF smoke assertions for fixture-only, read-only, selected-only, exact-confirmation, no-restore, and real-profile/custom blocker wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Quarantine Preview, Quarantine execution availability, current-fixture undo, manifest discovery, restore readiness, selected restore gate behavior, fixture-only selected restore, real-profile/custom blockers, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-execution-readiness-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/tooltip polish with no persistence, cleanup execution, restore semantics, security, deployment, or data-model change.

Open questions:

- Should visible fixture review lead to always-visible help icons for safety-critical gates, or are tooltip plus automation help text enough?

Rejected ideas buffer:

- Do not add visible helper text, alter focus order, change enablement, enable real-profile movement, or broaden this packet beyond execution/readiness controls while proving the help-text pattern.

### 2026-05-29: Add Visible Row Shortlist Automation Help Text

Status: completed

Evidence:

- Visible Row Shortlist Tooltip Clarity made the bulk Review Shortlist boundaries visible by hover tooltip.
- Domain docs already require visible-row control tooltips and automation help text to keep row/display-window scope, no-file-modified behavior, and not-cleanup-approval boundaries available.
- Recent automation-help-text packets proved the same non-hover pattern on nearby Review Shortlist report/preview controls.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to `Shortlist visible rows` and `Remove visible rows`.
- Added test-facing help-text accessors and WPF smoke assertions for visible-row scope, review-only/not-cleanup-approval wording, and no-file-modified behavior.
- Updated README, progress, handoff, and the feature brief.
- Kept Review Shortlist behavior, display-window scope, Quarantine Preview eligibility, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-visible-row-shortlist-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should visible fixture review lead to always-visible help for visible-row bulk actions, or are tooltip plus automation help text enough?

Rejected ideas buffer:

- Do not add visible helper text, alter focus order, persist Review Shortlist, change display-window scope, enable real-profile movement, or broaden this packet beyond visible-row bulk controls.

### 2026-05-29: Add Scan Gate Automation Help Text

Status: completed

Evidence:

- Scan Gate Discoverability Polish and Scope-Specific Scan Gate Ready Wording made the `Scan` tooltip dynamic and scope-specific.
- The `Scan` button did not mirror that dynamic wording into WPF automation help text.
- The disabled real-profile `Scan` tooltip should remain discoverable before acknowledgement.

Implementation:

- Added disabled-state tooltip support to `Scan`.
- Reused the dynamic `FormatScanButtonToolTip` output as `Scan` automation help text.
- Added WPF smoke assertions for locked real-profile, acknowledged real-profile, fixture, and custom Scan automation help text.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept scan enablement, acknowledgement behavior, Storage Scan behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-scan-gate-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/tooltip polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the scan-gate header and tooltip wording need tightening after visible fixture/real-profile review?

Rejected ideas buffer:

- Do not change scan enablement, preflight acknowledgement semantics, or cleanup execution availability while polishing `Scan` help text.

### 2026-05-29: Add Quarantine Root Input Automation Help Text

Status: completed

Evidence:

- Quarantine Root Selection can be typed or browsed, and the browse action already had tooltip plus automation help text.
- The typed Quarantine Root field had a preview-only tooltip but no automation help text.
- Domain docs require Quarantine Root Selection to stay preview-only and separate from folder creation, file movement, and cleanup approval.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to the Quarantine Root text box.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for preview-only, no-folder-created, and no-file-moved wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Quarantine Root Selection behavior, Quarantine Root Safety Note, preview gating, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-root-input-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does visible fixture review show that the Quarantine Root toolbar and safety note need tightening?

Rejected ideas buffer:

- Do not create, probe, persist, or approve Quarantine Root paths while polishing typed-root help text.

### 2026-05-29: Add Cleanup Scope Input Automation Help Text

Status: completed

Evidence:

- Cleanup Scope Selection can be typed or browsed, and the browse action already had tooltip plus automation help text.
- The typed Cleanup Scope field had no tooltip or automation help text.
- Domain docs require Cleanup Scope Selection to stay path-only, separate from scanning, and unable to bypass the real-profile scan gate or approve cleanup.

Implementation:

- Added WPF tooltip and `AutomationProperties.HelpText` to `ScopePathBox`.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for path-only selection, no auto-scan, no real-profile gate bypass, and no cleanup approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Cleanup Scope Selection behavior, Cleanup Scope Safety Note, Cleanup Scope Scan Gate, Storage Scan behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-cleanup-scope-input-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does visible fixture/real-profile review show that the header needs tightening?

Rejected ideas buffer:

- Do not start scans, bypass acknowledgement, or approve cleanup while polishing typed Cleanup Scope help text.

### 2026-05-29: Add Storage Review Search Input Automation Help Text

Status: completed

Evidence:

- User confirmed debounced Storage Review Search is better on large real-profile results and the status-bar pending-search message is enough.
- The Storage Review Search input already had a tooltip with prefix examples, but no matching WPF automation help text.
- Domain docs require Storage Review Search to remain in-memory, read-only, and separate from cleanup approval.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to the Storage Review Search text box.
- Added test-facing automation help text accessor and WPF smoke assertions for prefix examples, no-rescan, no-file-modified, and no-cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept search matching, debounce behavior, status-bar pending-search wording, filters, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-storage-review-search-input-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does visible fixture/real-profile review show that the search input needs always-visible help, or are tooltip plus automation help text enough?

Rejected ideas buffer:

- Do not add another pending-search indicator, alter debounce timing, change search matching, or treat search text as cleanup approval while polishing search input help text.

### 2026-05-29: Add Review Lens Filter Help Text

Status: completed

Evidence:

- Review filters are central to manual fixture and real-profile review.
- Neighboring review controls already had tooltip and automation help text, but Storage Review Filter buttons plus Type, Size, and Category filters did not.
- Domain docs require filters to stay read-only, in-memory, and separate from cleanup approval.

Implementation:

- Added WPF tooltips and `AutomationProperties.HelpText` to Storage Review Filter buttons.
- Added WPF tooltips and `AutomationProperties.HelpText` to Type, Size, and Category filter combo boxes.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for review-only, no-rescan, no-file-modified, no-permission-change, Storage Savings, and not-cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept filter behavior, search debounce, scan behavior, exports, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-lens-filter-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Do visible fixture/real-profile reviews show that filter controls need always-visible help icons, or are tooltip plus automation help text enough?

Rejected ideas buffer:

- Do not change filter behavior, add new filters, alter export selection, retry access issue paths, or treat filter state as cleanup approval while polishing review lens help text.

### 2026-05-29: Add Safety Summary Shortcut Help Text

Status: completed

Evidence:

- Safety Summary shortcuts are central to manual fixture and real-profile safety review.
- Neighboring review controls already had tooltip and automation help text, but the Safety Summary shortcut buttons did not.
- Domain docs require Safety Summary shortcuts to stay read-only and separate from rescans, permission changes, link following, file changes, and cleanup approval.

Implementation:

- Added disabled-state WPF tooltips and `AutomationProperties.HelpText` to Safety Summary shortcut buttons.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for read-only shortcut scope, no-rescan, no-file-modified, no-permission-change, no-link-following, and not-cleanup/Quarantine-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept shortcut mapping, filter behavior, scan behavior, exports, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-safety-summary-shortcut-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Do visible fixture/real-profile reviews show that Safety Summary shortcuts need always-visible help icons or stronger grouping?

Rejected ideas buffer:

- Do not change shortcut mapping, add shortcuts, retry access issue paths, follow reparse points, or treat shortcut state as cleanup approval while polishing Safety Summary shortcut help text.

### 2026-05-29: Add Real Profile Acknowledgement Help Text

Status: completed

Evidence:

- User confirmed the real-profile scan gate checkbox enabled `Scan` after acknowledgement.
- The `Scan` button mirrored scan-gate wording through tooltip and automation help text, but the acknowledgement checkbox itself did not.
- Domain docs require the real-profile acknowledgement to stay local, in-memory, and separate from running preflight, creating fixtures, starting scans by itself, persistence, or cleanup approval.

Implementation:

- Added WPF tooltip and `AutomationProperties.HelpText` to the real-profile preflight/fixture-review acknowledgement checkbox.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for MVP preflight, fixture review, no-preflight-run, no-auto-scan, and no-cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Cleanup Scope Scan Gate behavior, acknowledgement reset behavior, Storage Scan behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-real-profile-acknowledgement-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does visible real-profile review show that the scan-gate header needs shorter text or always-visible help?

Rejected ideas buffer:

- Do not run preflight, create fixtures, auto-start scans, persist acknowledgement, or treat acknowledgement as cleanup approval while polishing the real-profile acknowledgement help text.

### 2026-05-29: Add Manifest Discovery and Selection Help Text

Status: completed

Evidence:

- Quarantine Manifest Discovery and Restore Manifest selection are safety-sensitive controls that can lead toward restore review.
- Neighboring readiness and execution controls already had tooltip and automation help text, but `Discover manifests` and the selection combo box did not.
- Domain docs require discovery to remain read-only and selection to remain separate from restore approval.

Implementation:

- Added disabled-state WPF tooltips and `AutomationProperties.HelpText` to `Discover manifests` and the Restore Manifest selection combo box.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for read-only discovery, no restore/move/delete/folder-cleanup/history, selection not approval, and no manifest writes.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept discovery behavior, selection behavior, readiness previews, restore gates, fixture-only restore, real-profile blockers, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-manifest-discovery-selection-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Do visible fixture/real-profile reviews show that manifest discovery and selection need always-visible help icons?

Rejected ideas buffer:

- Do not add restore behavior, all-manifest restore, cleanup history, manifest writes, or quarantine-folder cleanup while polishing discovery/selection help text.

### 2026-05-29: Add Scan Cancel Help Text

Status: completed

Evidence:

- `Cancel` is the only header scan-control action that had no tooltip or automation help text.
- Long real-profile scans make cancellation discoverability part of the manual review safety story.
- Existing code already reports canceled scans as no-file-modified and cancellation does not invoke cleanup, restore, or deletion.

Implementation:

- Added disabled-state WPF tooltip and `AutomationProperties.HelpText` to `Cancel`.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for in-progress read-only Storage Scan cancellation and no move/delete/quarantine/restore/cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept scan cancellation behavior, scan gate behavior, Storage Scan results, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-scan-cancel-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does visible fixture/real-profile review show that `Cancel` needs a visible scanning-state note beyond tooltip/help text?

Rejected ideas buffer:

- Do not change cancellation semantics, partial-result handling, scan gates, cleanup execution, undo, restore, or deletion while polishing scan cancel help text.

### 2026-05-29: Add Manual Fixture Show Children and Clipboard Fix

Status: completed

Evidence:

- During manual fixture review, the user confirmed `Show children` worked but did not make the way back to all rows obvious.
- The app then crashed during the same review.
- Windows Application logs showed the crash was an unhandled `System.Runtime.InteropServices.COMException (0x800401D0): OpenClipboard Failed` in `CopyPathButton_Click`.

Implementation:

- Updated `Show children` status text to say `Reset view` returns to all rows.
- Wrapped `Clipboard.SetText` for `Copy path` in a `COMException` handler that shows a warning/status instead of crashing.
- Added WPF smoke coverage for the `Reset view` way-back wording after selected-folder child focus.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept selected-folder focus semantics, Review Shortlist, scan behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-manual-fixture-show-children-clipboard-fix.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a bounded manual fixture bugfix with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Is naming `Reset view` in status text enough, or should the UI add a dedicated way-back button near selected-folder focus?

Rejected ideas buffer:

- Do not add a navigation stack, change `Show children` search semantics, remove Copy path, or add cleanup behavior while fixing this manual fixture finding.

### 2026-05-29: Preserve Current Fixture Undo After Rescan

Status: completed

Evidence:

- During manual fixture review, the user executed fixture Quarantine, rescanned, and found the moved synthetic file no longer appeared in the main Storage Scan rows.
- The user correctly pointed out that undo was no longer reachable from the disappeared row context.
- Code inspection showed `ApplyStorageScanResult` called `ClearQuarantinePreview`, which cleared `_currentRestoreManifest` and `_currentQuarantineExecutionResult`, losing current-fixture undo state after rescan.
- The user suggested a separate quarantined-files area; that is recorded as follow-up because it is a larger UI packet than the immediate safety bugfix.
- The user also expected Quarantine Preview success to be more visible than the status-bar line; that remains a follow-up polish candidate.

Implementation:

- Preserved the current in-memory Restore Manifest and execution result when clearing stale Quarantine Preview state if current-fixture undo is still available.
- Updated fixture execution status text to say `Undo fixture quarantine` can restore and that rescan refreshes review rows.
- Updated post-execution rescan status text to point to the preserved undo action in the Quarantine execution area.
- Extended WPF smoke coverage so fixture execution is followed by a post-execution rescan, then confirms `Undo fixture quarantine` remains available before undo.
- Updated README, domain context, glossary, handoff, and the feature brief.
- Kept real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, cleanup history, and broad manifest restore unavailable.

Verification:

- Initial `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` was blocked because the manually launched WPF app was still running and locking `WindowsFileCleaner.App.exe`.
- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated header styling checklist prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-preserve-current-fixture-undo-after-rescan.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a bounded lifecycle bugfix for current-fixture undo under ADR 0010, with no new persistence, real-profile cleanup execution, restore architecture, or deletion behavior.

Open questions:

- Should the next UI packet add a dedicated quarantined-files area independent of the currently visible Storage Scan rows?
- Should Quarantine Preview success appear in a more prominent pane or callout instead of relying on the status bar?

Rejected ideas buffer:

- Do not keep moved source rows artificially visible after a rescan; the refreshed Storage Scan should reflect the filesystem.
- Do not add real-profile cleanup execution, real-profile Undo Quarantine, all-manifest restore, persisted cleanup history, or permanent deletion while fixing current-fixture undo reachability.

### 2026-05-29: Add Shortlist-Level Quarantine Wording

Status: completed

Evidence:

- During manual fixture review, the user expected a single button to quarantine files/folders on the Review Shortlist instead of needing to visit each file and type `QUARANTINE`.
- Existing WPF execution already acts on the current Quarantine Preview built from Review Shortlist rows, but the visible `Execute quarantine` label did not make that scope clear.

Implementation:

- Renamed `Preview quarantine` to `Preview shortlist quarantine`.
- Renamed `Execute quarantine` to `Quarantine included shortlist`.
- Updated confirmation and execution tooltips/automation help text to say `QUARANTINE` is typed once and applies to all included Review Shortlist rows.
- Added preview/gate/status wording that makes Review Shortlist the source and included rows the execution target.
- Extended WPF smoke coverage from a one-row execution to a two-row Review Shortlist execution and undo.
- Kept exact confirmation, blockers, real-profile/custom execution blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-shortlist-level-quarantine-wording.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI wording and WPF smoke coverage for existing fixture-only execution under ADR 0009.

Open questions:

- Should future real-profile execution keep `Quarantine included shortlist` as the exact label after the separate real-profile Grill with Docs pass?
- Should a dedicated Quarantined Files area show included shortlist rows immediately after execution?

Rejected ideas buffer:

- Do not remove exact confirmation while clarifying shortlist-level execution.
- Do not call the action `Quarantine all` without qualifying that only included rows move; blocked and redundant rows must stay out of execution.

### 2026-05-29: Add Current-Session Quarantined Review

Status: completed

Evidence:

- During manual fixture review, the user wanted Quarantine shortlist execution controls out of the selected-row detail scroll area and grouped with Quarantine Root.
- The user also wanted a `Quarantined`-style button that switches the main grid to quarantined items, plus a way back to normal scan rows.
- Existing docs already captured that current-fixture undo remains available after a post-execution rescan, but the visible controls were still too easy to miss.

Implementation:

- Added a dedicated Quarantine shortlist area above the main grid for Quarantine Root Selection, preview/export, `QUARANTINE` confirmation, `Quarantine included shortlist`, `Undo fixture quarantine`, and Quarantine Execution Gate text.
- Removed the shortlist execution gate from the right selected-row scroll panel.
- Added `Quarantined` and `Back to scan rows` controls.
- Added a read-only current-session quarantined grid backed by current Restore Manifest entries still in `Moved` state.
- Kept older/discovered manifest review in the existing Quarantine Manifest Discovery and readiness panes.
- Kept real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, and cleanup history unavailable.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantined-review-mode.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible UI placement and read-only current-session review packet under existing fixture-only execution and undo ADRs 0009 and 0010.

Open questions:

- Should a future `Quarantined` view include discovered Restore Manifest entries, or should discovered manifests remain in the existing manifest discovery/readiness panes?

Rejected ideas buffer:

- Do not make moved source rows persist in refreshed Storage Scan results.
- Do not use the `Quarantined` button as cleanup history or broad manifest restore.

### 2026-05-29: Make Review Panels Collapsible

Status: completed

Evidence:

- User review showed the new Quarantine shortlist area made the main grid tiny because the verbose Quarantine Execution Gate text was always expanded above the grid.
- User suggested making the Quarantine list/panel collapsible and possibly doing the same for other vertical sections.

Implementation:

- Made the Quarantine shortlist area collapsible while preserving the grouped Quarantine root, preview, execution, undo, and current-session quarantined controls.
- Kept verbose Quarantine Execution Gate details in a constrained scroll viewport when the panel is expanded.
- Made the Safety Summary section collapsible so the grid can recover additional vertical space during focused review.
- Kept all cleanup execution boundaries unchanged.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/features/2026-05-29-quarantined-review-mode.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF layout behavior with no persistence, cleanup execution, restore, data-model, or security change.

Open questions:

- Should additional review sections, such as filters or selected-row detail panes, become collapsible after the next manual pass?

Rejected ideas buffer:

- Do not hide the Quarantine Execution Gate details entirely; keep them available in a constrained view.

### 2026-05-29: Add Collapsed Panel Summaries

Status: completed

Evidence:

- User verified the collapsible panel pass worked.
- User asked for useful panel header summaries while panels are closed.

Implementation:

- Replaced static Expander header text with dynamic Safety Summary and Quarantine shortlist header text.
- Safety Summary header now keeps compact risk counts visible while collapsed.
- Quarantine shortlist header now summarizes Review Shortlist count, preview included/blocked state, current quarantined count, and undo availability/completion.
- Added WPF smoke assertions for these collapsed-header summaries across scan, preview, fixture execution, and undo.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/features/2026-05-29-quarantined-review-mode.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF header text and test coverage with no cleanup execution, persistence, restore, or data-model change.

Open questions:

- Should the closed Quarantine header also include the selected Quarantine Root, or is that too visually noisy?

### 2026-05-29: Add Quarantine Preview Inline Status

Status: completed

Evidence:

- User noticed that clicking `Preview shortlist quarantine` appeared to do nothing because the success text was only visible at the bottom/status area.
- The Quarantine shortlist area is now the control cluster for root, preview, execution, undo, and current-session quarantined review, so preview readiness should be visible there.

Implementation:

- Added inline `QuarantinePreviewStatusText` inside the Quarantine shortlist panel.
- Before preview, the line explains whether Review Shortlist rows need to be added or previewed.
- After preview, the line summarizes included, blocked, redundant, previewed bytes, readiness blockers, not-cleanup-approval wording, and no-file-modified wording.
- After Quarantine Root changes, the line says preview destinations must be regenerated.
- After fixture execution or undo, the line switches to fixture execution/undo evidence instead of dry-run wording.
- Kept real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, and cleanup history unavailable.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-preview-inline-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI visibility polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Should the inline readiness line get visual success/warning styling after a manual fixture pass, or is plain text enough?

Rejected ideas buffer:

- Do not show a modal popup for dry-run preview success unless manual review shows inline text is still too easy to miss.

### 2026-05-29: Add Review Grid Mode Status

Status: completed

Evidence:

- The main grid can now show either Storage Scan rows or Current-Session Quarantined Review rows.
- The visible `Quarantined` / `Back to scan rows` buttons help, but a persistent grid-mode label reduces ambiguity during manual fixture review and after post-execution stale scan rows.

Implementation:

- Added `ReviewGridModeText` above the main grid.
- Storage Scan mode names the scan row display window and says whether current-session quarantined items are available.
- Current-session quarantined mode identifies the read-only current in-memory Restore Manifest view and points to `Back to scan rows`.
- After fixture Quarantine execution, Storage Scan mode warns that scan rows may be stale and points to available current-session quarantined entries.
- After fixture undo clears moved entries, the mode text stops advertising current quarantined rows.
- Kept real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, and cleanup history unavailable.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF status text with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Should the Review Grid Mode Status later use a badge or visual severity style, or is plain text sufficient?

Rejected ideas buffer:

- Do not use the grid mode status as cleanup history or as proof that Storage Scan rows are refreshed after fixture execution.

### 2026-05-29: Align Fixture Checklist With Review Polish

Status: completed

Evidence:

- The WPF app now has collapsible Safety Summary and Quarantine shortlist panels, inline Quarantine Preview readiness, Review Grid Mode Status, and `Quarantined` / `Back to scan rows` grid switching.
- The terminal fixture checklist still used older generic wording for Safety Summary, Quarantine Preview, and fixture execution/undo flow.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist item 3 to prompt collapsible Safety Summary header/details review.
- Updated checklist item 6 to prompt collapsible Quarantine shortlist header/details, inline preview readiness, preview/export tooltip, approval-boundary, execution-scope, and execution tooltip review.
- Updated checklist item 7 to prompt `Quarantined` / `Back to scan rows` plus Review Grid Mode Status during fixture execution/undo review.
- Kept checklist-only mode read-only: no preflight, fixture creation, WPF launch, scan, move, restore, delete, or cleanup history.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the updated checklist in dry-run launcher output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local manual-review checklist wording with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Should the checklist be split into shorter grouped prompts if it becomes too dense during the next manual pass?

Rejected ideas buffer:

- Do not make checklist-only mode run preflight, create fixtures, launch WPF, or scan.

### 2026-05-29: Add Quarantined View Control Help Text

Status: completed

Evidence:

- `Quarantined` / `Back to scan rows` now switch the main grid between Storage Scan rows and current-session quarantined rows.
- Review Grid Mode Status labels the active grid, but the buttons still used mostly static help text and did not explain disabled states.
- User manual fixture review reported steps 1 through 11 working, and confirmed closed panel header summaries are desirable/useful.

Implementation:

- Added dynamic tooltip and automation help text for `Quarantined`.
- Added dynamic tooltip and automation help text for `Back to scan rows`.
- Disabled `Quarantined` help text now explains when current-session rows appear, when the view is already active, and when no moved entries remain after undo.
- Enabled `Quarantined` help text summarizes the current-session moved entry count and read-only/no-restore/no-delete/no-history boundary.
- `Back to scan rows` help text says returning does not rescan, modify files, or perform undo.
- Kept real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, and cleanup history unavailable.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantined-view-control-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help text with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Should discovered Restore Manifests eventually get a separate grid switch, or remain in discovery/readiness panes?

Rejected ideas buffer:

- Do not use `Quarantined` as discovered-manifest history or as a broad restore entry point.

### 2026-05-29: Add Quarantine Preview Status Styling

Status: completed

Evidence:

- User originally expected a popup after `Preview shortlist quarantine` and then noticed the inline/status text after looking near the bottom of the Quarantine shortlist area.
- Quarantine Preview Inline Status made the feedback local to the controls, but every state still used the same muted styling.

Implementation:

- Added lightweight semantic styling for `QuarantinePreviewStatusText`.
- Neutral waiting text remains normal weight.
- Ready preview and successful fixture execution/undo evidence use success styling.
- Shortlisted-but-not-previewed, stale preview, blocked preview, and recovery-review states use warning styling.
- Preview creation failure uses error styling.
- Kept Quarantine Preview as a dry run and did not add popup behavior, real-profile execution, permanent deletion, or cleanup history.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF styling with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Should Review Grid Mode Status get similar styling later, or is plain text enough there?

Rejected ideas buffer:

- Do not add a modal popup for Quarantine Preview success unless a future manual visual pass proves styled inline status is still too easy to miss.

### 2026-05-29: Align Fixture Checklist With Preview Status Styling

Status: completed

Evidence:

- Quarantine Preview Status Styling added neutral/success/warning/error inline status styling.
- `Start-MvpFixtureReview.ps1` checklist item 6 still mentioned generic inline preview readiness and did not prompt the next manual pass to inspect the new semantic states.

Implementation:

- Updated fixture checklist item 6 to prompt styled inline preview readiness review with neutral/success/warning/error states.
- Updated README launcher wording from inline preview readiness to styled inline Quarantine Preview readiness.
- Updated the fixture checklist and status-styling feature briefs to record this follow-up alignment.
- Kept checklist-only and dry-run launcher paths read-only: no preflight, fixture creation, WPF launch, scan, move, restore, delete, or cleanup history.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the updated checklist in dry-run launcher output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local manual-review checklist wording with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Does the next visible fixture pass show that the styled inline preview status is noticeable enough without a popup?

Rejected ideas buffer:

- Do not make checklist-only mode perform visual verification or launch WPF; it stays terminal-output-only.

### 2026-05-30: Add Quarantine Preview Status Help Text

Status: completed

Evidence:

- User ran the latest manual fixture review checklist through steps 1-11 and reported that all checked behavior worked.
- Collapsed panel header summaries already exist and remain useful while closed.
- The inline Quarantine Preview status is safety-relevant and dynamic, but its exact state/boundary was not mirrored into tooltip or automation help text.

Implementation:

- Added tooltip and automation help text to `QuarantinePreviewStatusText`.
- Updated dynamic status updates so the tooltip/help text mirrors the current inline status and repeats read-only, no-create, no-move, no-restore, no-delete, and not-cleanup-approval boundaries.
- Added WPF smoke assertions across waiting, shortlisted, invalid-root, ready, stale, fixture execution, fixture undo, and blocked-preview states.
- Updated the fixture checklist to prompt manual review of the inline status tooltip/help text.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-quarantine-preview-inline-status.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/features/2026-05-30-quarantine-preview-status-help-text.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next manual fixture visual pass, confirm whether the collapsed panel header summaries are visible enough when panels are closed.

### 2026-05-30: Add Quarantine Shortlist Header Styling

Status: completed

Evidence:

- User confirmed the collapsible layout worked and said a header panel summary while closed would be nice.
- The Quarantine shortlist header already summarized shortlist, preview, current quarantined, and undo state, but it used the same visual style for waiting, ready, blocked/stale, current-quarantined, and undo-completed states.

Implementation:

- Added lightweight semantic styling to `QuarantineShortlistHeaderText`.
- Header state is neutral before shortlist/preview/current quarantine, warning while shortlist needs preview or preview is blocked/stale, success for clean preview or completed undo, and information while current-session quarantined rows exist.
- Kept the header compact; no new row, badge, modal, cleanup execution, restore behavior, persisted history, or real-profile file movement was added.
- Added WPF smoke assertions for startup, shortlisted-before-preview, invalid-root, clean preview, stale preview, blocked preview, current quarantined, and undo-completed header states.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-collapsed-panel-header-help-text.md`
- `docs/features/2026-05-30-quarantine-shortlist-header-styling.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF styling with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next visible fixture pass, confirm whether the styled closed header makes the Quarantine shortlist state easier to scan without looking like cleanup approval.

### 2026-05-30: Record Manual Fixture Checklist Progress

Status: completed

Evidence:

- User ran the latest manual fixture review checklist through steps 1-11 and reported that all checked behavior worked.
- User agreed that a useful panel header summary while a panel is collapsed is desirable.
- The current build already keeps compact dynamic Safety Summary and Quarantine shortlist summaries in the Expander headers and mirrors them through tooltip/help text.

Implementation:

- Recorded the manual verification evidence and closed-header summary preference in durable docs.
- No app behavior changed.
- No real user files were scanned or modified by this docs-only packet.

Verification:

- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed the branch clean before the docs-only update.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm whether the collapsed header summaries are noticeable enough in the actual WPF window, or whether they need stronger visual treatment.

Rejected ideas buffer:

- Do not add new cleanup execution, restore execution, permanent deletion, or cleanup history in response to review-panel layout feedback.

### 2026-05-30: Add Review Grid Mode Status Help Text

Status: completed

Evidence:

- Review Grid Mode Status was visible and semantically styled, but unlike adjacent review controls it did not expose matching tooltip or automation help text.
- The status line is safety-relevant because it distinguishes Storage Scan rows from Current-Session Quarantined Review rows and warns when scan rows may be stale after fixture Quarantine execution.

Implementation:

- Added startup tooltip and automation help text to `ReviewGridModeText`.
- Mirrored each dynamic Review Grid Mode Status message into tooltip and automation help text.
- Added explicit read-only/no-rescan/no-file-modified/no-restore/not-cleanup-approval boundary wording.
- Kept Storage Scan, Quarantine Preview, fixture execution, undo, selected restore, real-profile execution availability, permanent deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated grid-mode tooltip/help-text prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `docs/features/2026-05-29-review-grid-mode-status-styling.md`
- `docs/features/2026-05-30-review-grid-mode-status-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help text with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm whether Review Grid Mode Status help text is discoverable enough without a visible help icon.

Rejected ideas buffer:

- Do not add another visible help icon for Review Grid Mode Status unless manual review shows tooltip/help text is insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Grid Mode Help

Status: completed

Evidence:

- Review Grid Mode Status Help Text touched WPF status text, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the narrow app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the grid-mode help-text packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against collapsed header summaries, styled inline Quarantine Preview readiness, styled Review Grid Mode Status tooltip/help text, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Retire Stale Grid-Mode Styling Follow-ups

Status: completed

Evidence:

- `docs/features/2026-05-29-quarantine-preview-status-styling.md` still listed Review Grid Mode Status styling as a future follow-up even though it landed in `2026-05-29-review-grid-mode-status-styling.md`.
- `docs/features/2026-05-29-review-grid-mode-status.md` still framed the question as badge versus plain text, but the current implementation is styled text.

Implementation:

- Updated the Quarantine Preview status-styling brief to mark Review Grid Mode Status styling as completed and leave only manual visual review open.
- Updated the Review Grid Mode Status brief so the remaining question is badge versus styled text after manual fixture review, not badge versus plain text.
- No code or app behavior changed.

Verification:

- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- `rg -n "Review Grid Mode Status get similar styling|plain text sufficient|badge or stay as plain text" docs README.md .codex/progress.md` found only historical progress entries and this verification note, not current feature guidance.

Docs updated:

- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is documentation cleanup only.

Open questions:

- Manual fixture review still needs to decide whether styled text is enough or a badge is worth adding.

Rejected ideas buffer:

- Do not let stale feature-brief follow-ups override newer completed packets.

### 2026-05-30: Add Collapsed Panel Header Help Text

Status: completed

Evidence:

- User verified the collapsible panels worked and confirmed that header panel summaries while closed are useful.
- Safety Summary and Quarantine shortlist headers now carry compact dynamic summaries, but long summaries can be harder to inspect when horizontal space is tight.

Implementation:

- Added tooltip and `AutomationProperties.HelpText` to the Safety Summary header.
- Added tooltip and `AutomationProperties.HelpText` to the Quarantine shortlist header.
- Mirrored each dynamic header summary into its tooltip/help text whenever scan, preview, fixture execution, or undo state changes.
- Added header text trimming so tight layouts keep panel headers compact instead of crowding the grid.
- Updated the fixture checklist to prompt header tooltip/help-text review.
- Kept Storage Scan, Quarantine Preview, fixture execution, undo, restore, real-profile execution availability, permanent deletion, and cleanup history behavior unchanged.

Verification:

- Initial `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` failed on a local variable name shadowing error in `UpdateSafetySummaryHeader`; renamed the waiting-state locals and reran the same build.
- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated header tooltip/help-text prompts without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantined-review-mode.md`
- `docs/features/2026-05-30-collapsed-panel-header-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help text and layout polish with no cleanup execution, persistence, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, is header tooltip/help text enough, or should safety-critical panel headers get a small always-visible help affordance?

Rejected ideas buffer:

- Do not add another visible help icon until manual fixture review shows header tooltip/help text is insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Header Help

Status: completed

Evidence:

- Collapsed Panel Header Help Text touched WPF header controls, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the narrow app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the header-help packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal still needs a visible fixture review pass against collapsed header tooltip/help text, styled inline Quarantine Preview readiness, styled Review Grid Mode Status, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Current Quarantined Label

Status: completed

Evidence:

- Current-Session Quarantined Review is intentionally current-session and fixture-execution state only.
- The visible `Quarantined` label could be mistaken for all quarantined history or discovered Restore Manifests even though tooltip/help text already explains the narrower scope.

Implementation:

- Renamed the visible `Quarantined` button to `Current quarantined`.
- Updated Review Grid Mode Status wording so stale Storage Scan rows point to `Current quarantined` when current-session moved entries are available.
- Added WPF smoke coverage that asserts the visible button label exposes current-session scope.
- Updated README, domain docs, feature briefs, fixture checklist, handoff, and progress notes.
- Kept current-session grid behavior unchanged: it remains read-only, fixture/current-manifest-only, and does not discover older manifests, restore, move, delete, or create cleanup history.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated `Current quarantined` checklist wording without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantined-review-mode.md`
- `docs/features/2026-05-29-quarantined-view-control-help-text.md`
- `docs/features/2026-05-30-current-quarantined-label.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI wording for existing current-session-only behavior under ADR 0010, with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- A future broader restore/history design still needs to decide whether discovered Restore Manifest entries appear in a separate tab/grid or stay in manifest discovery/readiness panes.

Rejected ideas buffer:

- Do not use the current-session grid switch as discovered-manifest history or broad restore entry point.

### 2026-05-30: Run Full Local MVP Preflight After Current Label

Status: completed

Evidence:

- Current Quarantined Label touched WPF UI text, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than narrow app-test runs before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the current-label packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal still needs a visible fixture review pass against `Current quarantined`, collapsed header tooltip/help text, styled inline Quarantine Preview readiness, and styled Review Grid Mode Status.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-29: Harden Quarantine Preview Error Style Coverage

Status: completed

Evidence:

- Quarantine Preview Status Styling documented an error style for preview creation failures.
- Existing WPF smoke assertions covered neutral, warning, success, execution, undo, and blocked-preview styling, but did not directly exercise the invalid Quarantine Root error path.

Implementation:

- Added WPF smoke coverage that attempts Quarantine Preview with a relative Quarantine Root after shortlisting a fixture row.
- Asserted the inline Quarantine Preview status says the preview could not be created, keeps no-file-modified wording visible, uses `Error` styling, and remains visually emphasized.
- Kept Quarantine Preview dry-run behavior unchanged; no files or folders are created, moved, restored, deleted, or added to cleanup history.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is test coverage for existing WPF styling behavior with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Does the next visible fixture pass show that the styled inline preview status is noticeable enough without a popup?

Rejected ideas buffer:

- Do not rely on documentation alone for semantic status states; keep each state covered by focused WPF smoke assertions when practical.

### 2026-05-29: Run Full Local MVP Preflight After Review Polish

Status: completed

Evidence:

- Recent packets changed WPF safety/status styling, checklist wording, and WPF smoke coverage around Quarantine Preview and current-session quarantined review.
- A full local preflight gives stronger evidence than narrow app-test runs before the next manual fixture pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal still needs a visible fixture review pass against the styled inline preview status.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-29: Add Review Grid Mode Status Styling

Status: completed

Evidence:

- During manual fixture review, after fixture Quarantine execution and rescan, moved files disappeared from Storage Scan rows as expected.
- Review Grid Mode Status already names Storage Scan rows versus current-session quarantined rows, but it used the same muted styling for ordinary scan rows, stale scan warnings, and current-session quarantined review.

Implementation:

- Added lightweight semantic styling for `ReviewGridModeText`.
- Ordinary Storage Scan rows use neutral styling.
- Current-session quarantined rows use informational styling.
- Stale Storage Scan rows after fixture Quarantine execution use warning styling.
- Empty current-session quarantined rows after moved entries are gone use warning styling.
- Kept the status read-only and did not add cleanup execution, restore behavior, persistent history, or new UI layout.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `docs/features/2026-05-29-review-grid-mode-status-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF styling with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Should the status eventually use a badge instead of styled text?

Rejected ideas buffer:

- Do not add tabs, modal explanations, persisted cleanup history, or discovered-manifest merging just to distinguish the current grid mode.

### 2026-05-29: Align Fixture Checklist With Grid Mode Styling

Status: completed

Evidence:

- Review Grid Mode Status Styling added neutral/informational/warning styling to the WPF grid-mode line.
- `Start-MvpFixtureReview.ps1` checklist item 7 still mentioned generic Review Grid Mode Status and did not prompt the next manual pass to inspect the new semantic states.

Implementation:

- Updated fixture checklist item 7 to prompt styled Review Grid Mode Status review with neutral/informational/warning states.
- Updated README launcher wording from Review Grid Mode Status to styled Review Grid Mode Status.
- Updated the fixture checklist and grid-mode styling feature briefs to record this follow-up alignment.
- Kept checklist-only and dry-run launcher paths read-only: no preflight, fixture creation, WPF launch, scan, move, restore, delete, or cleanup history.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the updated checklist in dry-run launcher output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-review-grid-mode-status-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local manual-review checklist wording with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Does the next visible fixture pass show that styled Review Grid Mode Status is noticeable without crowding the grid?

Rejected ideas buffer:

- Do not make checklist-only mode perform visual verification or launch WPF; it stays terminal-output-only.
