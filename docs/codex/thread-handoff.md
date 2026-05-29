# Thread Handoff

Last updated: 2026-05-30

Use this when starting a fresh Codex thread for this repository.

## Current state

- Repo: `D:\Codex\Windows File Cleaner`
- GitHub: `Smellybum1/Windows-File-Cleaner`
- Branch: `main`
- Latest completed packet: Fixture Checklist Selected Restore Gate Cue
- Current app stack: C# / WPF / .NET 8
- Desktop shortcut target: `D:\Codex\Windows File Cleaner\src\WindowsFileCleaner.App\bin\Debug\net8.0-windows\WindowsFileCleaner.App.exe`

## Product status

Windows File Cleaner is a local Windows-only desktop app for reviewing storage under `C:\Users\moxhe`.

The current MVP has a read-only Storage Scan that can inspect large real-profile scans with filters, collapsible Safety Summary and Quarantine Shortlist panels with panel-name closed-header summaries plus mirrored header tooltip/help text, visible hoverable header `?` help cues, state-naming header tooltip/help text, and Safety Summary/Quarantine Shortlist header state styling, semantically styled inline Quarantine Preview readiness/status in the Quarantine Shortlist panel with state-naming mirrored tooltip/help text and a visible hoverable `?` help cue, Quarantine Execution Gate tooltip/help text with a visible hoverable `?` help cue, Selected Restore Execution Gate tooltip/help text with a visible hoverable `?` help cue, semantically styled Review Grid Mode Status above the main grid with state-naming mirrored tooltip/help text and a visible hoverable `?` help cue, Cleanup Scope Safety Note and Quarantine Root Safety Note help text plus visible hoverable `?` help cues, WPF smoke coverage for the thirteen hoverable `?` help-cue affordance settings, current-session quarantined-items grid switching with dynamic `Current quarantined (N)` / `Back to scan rows` help text, shortlist-level fixture Quarantine wording, preserves current-fixture undo after a post-execution rescan, manual fixture Show children/Clipboard crash fix, scan cancel help text, scan-gate summary help text plus a visible hoverable `?` help cue, real-profile acknowledgement help text plus a visible hoverable `?` help cue, Review Mix, Matched Review Mix, and Review Shortlist Safety Mix hoverable `?` help cues with mirrored tooltip/help text, Safety Summary shortcut help text, review-lens filter help text, manifest discovery/selection help text, debounced search and search-input automation help text, scan-gate automation help text, Cleanup Scope input/browse automation help text, Quarantine Root input/browse automation help text, selected-row action automation help text, visible-row shortlist automation help text, execution/readiness automation help text, review report/preview automation help text, review toolbar automation help text, review navigation/export tooltip clarity, visible scope-specific Cleanup Scope Scan Gate status/tooltip wording, Cleanup Scope and Quarantine Root browse tooltip clarity, selected-row action tooltip clarity, Review Shortlist Safety Mix mirrored tooltip/help text, visible-row Review Shortlist bulk labels/tooltips, review toolbar report/preview tooltip clarity, selected-folder child/descendant focus, selected-folder summaries, hotspot trail, file preview, CSV export, Review Shortlist, fixture launcher checklist output aligned with hoverable `?` help-cue wording, checklist-only mode now covered by MVP preflight, and execution-policy-friendly `.cmd` wrappers for human-facing PowerShell tools aligned with recent review polish, Quarantine Preview and Quarantine Execution Gate scope-status/approval-boundary wording without technical implementation-flag wording, execution-control tooltip clarity, readiness scope tooltip clarity, Restore Manifest Draft, Quarantine Confirmation Draft, confirmation label wording polish, Quarantine Action Draft, manifest discovery with all-manifest restore wording, Restore Manifest wording polish, selected manifest readiness label polish, all-manifest readiness label polish, selected manifest review with readiness-evidence wording, selected restore gate scope-status/approval-boundary wording without technical implementation-flag wording, selected-restore scope-status checklist coverage, all-manifest readiness preview with all-manifest restore wording, and all-manifest restore boundary checklist coverage.

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
- Scan Gate Summary Help Cue added a visible hoverable `?` cue beside the locked/ready scan-gate summary, mirrors dynamic summary tooltip/help text, and expands WPF smoke affordance coverage to nine cues.
- Quarantine Root Safety Note Help Cue added a visible hoverable `?` cue beside the Quarantine Root Safety Note, mirrors dynamic note tooltip/help text, and expands WPF smoke affordance coverage to ten cues.
- Cleanup Scope Safety Note Help Cue added a visible hoverable `?` cue beside the Cleanup Scope Safety Note, mirrors dynamic note tooltip/help text, and expands WPF smoke affordance coverage to eleven cues.
- Full Local MVP Preflight After Cleanup Scope Cue passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files. The first sandboxed attempt failed on user-level NuGet config access, then the same command passed with approved access.
- Preflight Fixture Checklist Step added `Start-MvpFixtureReview.ps1 -ChecklistOnly` to the full MVP preflight path, with `-SkipFixtureChecklist` for focused loops; `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore` passed and printed the checklist without creating fixture files, launching WPF, or scanning real user files.
- Full Local MVP Preflight After Checklist Step passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, and whitespace diff check all passed without scanning or modifying real user files.
- CI Preflight CMD Wrapper Alignment updated GitHub Actions to run `tools\Invoke-MvpPreflight.cmd` under `cmd`, matching the preferred local wrapper path; `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore` passed locally without scanning or modifying real user files.
- Quarantine Execution Label Docs Alignment updated current-facing manual-review docs to use the visible `Quarantine included shortlist` label instead of the older generic execution wording; docs-only, no scan or cleanup behavior changed.
- Selected Restore Gate Technical Wording removed the technical `Execution implemented` field from the selected restore gate pane while keeping fixture-only selected restore and custom/real-profile blockers unchanged; `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- Quarantine Gate Technical Wording removed the technical `Execution implemented` field from Quarantine Preview and Quarantine Execution Gate panes while keeping fixture-only Quarantine execution and custom/real-profile blockers unchanged; `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- Full Local MVP Preflight After Quarantine Gate Wording passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, and whitespace diff check all passed without scanning or modifying real user files.
- Fixture Checklist Quarantine Button Label updated `Start-MvpFixtureReview.cmd -ChecklistOnly` step 7 to say `click Quarantine included shortlist`, matching the visible WPF action label; checklist-only output passed without preflight, fixture creation, WPF launch, scan, or file modification.
- Full Local MVP Preflight After Checklist Label passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, checklist-only output with `click Quarantine included shortlist`, and whitespace diff check all passed without scanning or modifying real user files.
- Quarantine Preview Placeholder Label updated WPF startup/reset placeholder text to say `Preview shortlist quarantine` instead of the older generic preview label; WPF smoke coverage now checks startup and stale-reset placeholders.
- Full Local MVP Preflight After Placeholder Label passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, and whitespace diff check all passed without scanning or modifying real user files.
- Quarantine Execution Gate Preview Button Label translates the WPF missing-preview blocker to `Use Preview shortlist quarantine before entering confirmation text.` while leaving the core gate wording unchanged.
- Quarantine Execution Gate Help Cue adds a visible hoverable `?` cue beside the WPF gate readout, mirrors concise dynamic tooltip/help text for startup, closed, open, executed, and undone gate states, and expands WPF smoke affordance coverage to twelve cues.
- Full Local MVP Preflight After Gate Help Cue passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output with the Quarantine Execution Gate `?` help cue, and whitespace diff check all passed without scanning or modifying real user files.
- Selected Restore Gate Help Cue adds a visible hoverable `?` cue beside the WPF Selected Restore Execution Gate, mirrors concise dynamic tooltip/help text for waiting, closed, open, restored, and custom/real-profile blocked states, and expands WPF smoke affordance coverage to thirteen cues.
- Full Local MVP Preflight After Selected Restore Gate Cue passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output with the Selected Restore Execution Gate `?` help cue, and whitespace diff check all passed without scanning or modifying real user files.
- Fixture Checklist Selected Restore Gate Cue updated checklist step 8 and README manual review wording to check the Selected Restore Execution Gate `?` help cue in waiting/closed/open/restored states and watch for visual crowding.
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
- Visible Row Shortlist Automation Help Text added matching WPF automation help text for `Shortlist visible rows` and `Remove visible rows`.
- Scan Gate Automation Help Text added disabled-state tooltip support and matching dynamic WPF automation help text for `Scan`.
- Quarantine Root Input Automation Help Text added matching WPF automation help text for the typed Quarantine Root field.
- Cleanup Scope Input Automation Help Text added tooltip and matching WPF automation help text for the typed Cleanup Scope field.
- Storage Review Search Input Automation Help Text added matching WPF automation help text for the search field with prefix examples and read-only/no-rescan/no-cleanup-approval wording.
- Review Lens Filter Help Text added tooltip and matching WPF automation help text for Storage Review Filter buttons plus Type, Size, and Category filters.
- Safety Summary Shortcut Help Text added disabled-state tooltip and matching WPF automation help text for Safety Summary review shortcut buttons.
- Real Profile Acknowledgement Help Text added tooltip and matching WPF automation help text for the real-profile preflight and fixture-review acknowledgement checkbox.
- Real Profile Acknowledgement Help Cue added a visible hoverable `?` cue beside the real-profile acknowledgement, mirrors the checkbox tooltip/help text, hides for fixture/custom scopes, and expanded WPF smoke affordance coverage to eight cues.
- Manifest Discovery and Selection Help Text added disabled-state tooltip and matching WPF automation help text for `Discover manifests` and Restore Manifest selection.
- Scan Cancel Help Text added disabled-state tooltip and matching WPF automation help text for `Cancel`.
- Manual Fixture Show Children and Clipboard Fix made `Show children` status name `Reset view` as the way back and catches busy-Clipboard Copy path failures instead of crashing.
- User completed manual fixture review through fixture Quarantine execution and observed that, after a rescan, the moved file disappears from Storage Scan rows as expected.
- Preserve Current Fixture Undo After Rescan keeps `Undo fixture quarantine` available after that post-execution rescan until undo is attempted.
- User feedback from the same pass: Quarantine Preview success was easy to miss because it appeared only in the status bar; consider stronger visible feedback.
- User suggested a separate quarantined-files area so moved files remain reviewable/restorable independently of the current Storage Scan grid.
- User expected one button for the Review Shortlist rather than per-file approval. Shortlist-Level Quarantine Wording renamed the fixture action to `Quarantine included shortlist`, renamed preview to `Preview shortlist quarantine`, and extended smoke coverage to two included rows moving/restoring together.
- Current-Session Quarantined Review moved the Quarantine shortlist gate out of the right detail scroll panel, grouped it with Quarantine Root, and added `Quarantined` / `Back to scan rows` main-grid switching for current in-memory Restore Manifest entries still in `Moved` state.
- User review showed the new Quarantine shortlist area made the grid too small; Collapsible Review Panels made Safety Summary and Quarantine shortlist collapsible and constrained verbose gate details.
- User verified the collapsible panels worked and asked for closed-panel summaries; Collapsed Panel Summaries added compact Safety Summary counts and Quarantine shortlist/preview/current-quarantine/undo state to the Expander headers.
- Quarantine Preview Inline Status added a compact preview/readiness line inside the Quarantine shortlist panel so preview success is visible near the controls, not only in the status bar.
- Review Grid Mode Status added a compact line above the main grid that identifies Storage Scan rows versus current-session quarantined rows, points to `Back to scan rows`, and warns when scan rows may be stale after fixture Quarantine execution.
- Fixture Checklist Review Polish Alignment updated `Start-MvpFixtureReview.ps1 -ChecklistOnly` prompts to include collapsible panel review, inline preview readiness, Review Grid Mode Status, and `Quarantined` / `Back to scan rows`.
- Quarantined View Control Help Text added dynamic disabled/enabled tooltip and automation help text for `Quarantined` and `Back to scan rows`; Current Quarantined Label later renamed the visible grid switch to `Current quarantined`.
- Quarantine Preview Status Styling added lightweight semantic styling to the inline Quarantine Preview status: neutral waiting, success ready/completed, warning blockers/stale, and error preview failure.
- Fixture Checklist Preview Status Styling Alignment updated `Start-MvpFixtureReview.ps1 -ChecklistOnly` prompts to include styled inline Quarantine Preview readiness states.
- Quarantine Preview Error Style Coverage added WPF smoke coverage for invalid Quarantine Root preview attempts using the inline error status style.
- Full Local MVP Preflight After Review Polish passed `Invoke-MvpPreflight.ps1`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Review Grid Mode Status Styling added neutral/informational/warning styling to distinguish ordinary scan rows, stale scan rows, and current-session quarantined review.
- Fixture Checklist Grid Mode Styling Alignment updated `Start-MvpFixtureReview.ps1 -ChecklistOnly` prompts to include styled Review Grid Mode Status states.
- Retire Stale Grid-Mode Styling Follow-ups updated feature briefs so they no longer list Review Grid Mode Status styling as future work after that packet landed.
- Collapsed Panel Header Help Text mirrors Safety Summary and Quarantine shortlist closed-header summaries into tooltip and automation help text with read-only/not-cleanup-approval wording.
- Full Local MVP Preflight After Header Help passed `Invoke-MvpPreflight.ps1`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Current Quarantined Label renamed the visible current-session quarantined grid switch to `Current quarantined`, while older/discovered Restore Manifest review remains in manifest discovery/readiness panes.
- Current Quarantined Count Label updates that grid switch to show `Current quarantined (N)` when current-session moved entries are available, while keeping empty/disabled states compact and older/discovered Restore Manifest review in manifest discovery/readiness panes.
- Full Local MVP Preflight After Current Quarantined Count passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Full Local MVP Preflight After Current Label passed `Invoke-MvpPreflight.ps1`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- User ran the latest manual fixture review checklist through steps 1-11 and reported that all checked behavior worked; user also agreed that useful collapsed panel header summaries are desirable.
- Review Grid Mode Status Help Text mirrored dynamic grid-mode wording into tooltip and automation help text with read-only, no-rescan, no-file-modified, no-restore, and not-cleanup-approval boundaries.
- Full Local MVP Preflight After Grid Mode Help passed `Invoke-MvpPreflight.ps1`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Quarantine Preview Status Help Text mirrored dynamic inline preview wording into tooltip and automation help text with read-only, no-create, no-move, no-restore, no-delete, and not-cleanup-approval boundaries.
- Quarantine Shortlist Header Styling added lightweight closed-header styling for neutral, needs-preview/blocked/stale, preview-ready/undo-completed, and current-quarantined states without changing cleanup execution or restore availability.
- Safety Summary Header Styling added lightweight closed-header styling for waiting/neutral and safety-signal warning states without changing scan behavior, cleanup execution, or scan gates.
- Full Local MVP Preflight After Safety Header Styling passed `Invoke-MvpPreflight.ps1`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Collapsed Header State Help Text added textual `Header state:` wording to Safety Summary and Quarantine shortlist header tooltip/help text so collapsed-header state is not color-only.
- Full Local MVP Preflight After Header State Help passed `Invoke-MvpPreflight.ps1`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Status State Help Text added textual `Status state:` wording to Review Grid Mode Status and inline Quarantine Preview tooltip/help text so styled status state is not color-only.
- Collapsed Header Summary Labels made collapsed Safety Summary and Quarantine Shortlist headers start with their visible panel names while preserving compact summary counts, state styling, tooltip/help text, and cleanup boundaries.
- Full Local MVP Preflight After Header Summary Labels passed `Invoke-MvpPreflight.ps1`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Review Shortlist Safety Mix Help Text mirrored the dynamic safety mix into tooltip/help text with no-rescan, no-file-modified, no-Quarantine-readiness-proof, no-storage-savings-proof, and not-cleanup-approval boundaries.
- Review Mix Help Text mirrored Review Mix and Matched Review Mix into tooltip/help text with no-rescan, no-file-modified, no-storage-savings-proof, and not-cleanup-approval boundaries.
- Review Mix Help Cues added visible circular `?` help cues beside Review Mix and Matched Review Mix so their mirrored tooltip/help text is easier to discover.
- Review Shortlist Safety Mix Help Cue added a visible circular `?` help cue beside Review Shortlist Safety Mix so its mirrored tooltip/help text is easier to discover before Quarantine Preview.
- Full Local MVP Preflight After Safety Cue passed `Invoke-MvpPreflight.ps1`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Review Grid Mode Status Help Cue added a visible circular `?` help cue beside Review Grid Mode Status so its mirrored status-state tooltip/help text is easier to discover above the main grid.
- Quarantine Preview Status Help Cue added a visible circular `?` help cue beside the inline Quarantine Preview readiness/status line so its mirrored dry-run/status-state tooltip/help text is easier to discover before any fixture execution gate.
- Full Local MVP Preflight After Help Cues passed `Invoke-MvpPreflight.ps1`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Collapsed Header Help Cues added visible circular `?` help cues beside Safety Summary and Quarantine Shortlist collapsed headers so their mirrored header summary/state tooltip/help text is easier to discover.
- Full Local MVP Preflight After Header Cues passed `Invoke-MvpPreflight.ps1`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Hoverable Help Cue Affordance added the Windows help cursor and short tooltip delay to the seven existing circular `?` cues without changing scan, Quarantine, restore, or manifest behavior.
- Execution-Policy Friendly Fixture Launcher added `.\tools\Start-MvpFixtureReview.cmd`, which invokes the existing PowerShell launcher with process-scoped `-ExecutionPolicy Bypass` and forwards arguments such as `-ChecklistOnly` and `-SkipPreflight`.
- Full Local MVP Preflight After CMD Launcher passed `Invoke-MvpPreflight.ps1`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed, and preflight now suggests `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.
- Execution-Policy Friendly Tool Wrappers added `.\tools\Invoke-MvpPreflight.cmd` and `.\tools\New-StorageScanSmokeFixture.cmd`, making process-scoped bypass wrappers available for all human-facing PowerShell tools.
- Full Local MVP Preflight Through CMD Wrapper passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Fixture Checklist Hoverable Help Cues aligned `Start-MvpFixtureReview.ps1 -ChecklistOnly` and README fixture launcher wording with the current hoverable `?` help-cue affordance.
- Full Local MVP Preflight After Hoverable Checklist passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Hoverable Help Cue Affordance Coverage added WPF smoke assertions that all tracked circular `?` cues use the Help cursor and prompt tooltip delay.
- Full Local MVP Preflight After Help Cue Coverage passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.
- Full Local MVP Preflight After Scan Gate Cue passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.

## Best next work

Prefer a small packet. Good candidates:

1. Run a manual fixture visual polish pass with `.\tools\Start-MvpFixtureReview.cmd -ChecklistOnly` and then `.\tools\Start-MvpFixtureReview.cmd`; verify the updated prompts match the visible app, including the scan-gate summary `?` help cue, Review Mix / Matched Review Mix / Review Shortlist Safety Mix / Review Grid Mode Status / inline Quarantine Preview status / Quarantine Execution Gate / Selected Restore Execution Gate / collapsed panel header hoverable `?` help cues and prompt tooltip/help text, Selected Restore Execution Gate cue waiting/closed/open/restored states without crowding the gate area, panel-name collapsed header summaries/state styling and state-naming tooltip/help text, styled inline Quarantine Preview readiness state-naming tooltip/help text, and `Current quarantined (N)` / `Back to scan rows` help text.
2. Decide whether a future broader restore/history design should put discovered Restore Manifest entries in a separate tab/grid or keep them in the manifest discovery/readiness panes.
3. Continue Quarantine Preview/readiness wording review in the visible app, still without enabling real-profile execution.
4. Retest review navigation/export tooltips, Cleanup Scope/Quarantine Root browse tooltips, the scan-gate summary `?` help cue, the real-profile acknowledgement `?` help cue, selected-row action tooltips, visible-row shortlist labels/tooltips, report/preview tooltips, execution-control tooltips, readiness tooltips, and Quarantine Execution Scope Status in the visible app during fixture or real-profile review; consider whether always-visible help icons are needed for other safety-critical gates.
5. Start real-profile Quarantine execution design only after the user explicitly wants to move beyond preview and the safety/undo story is reviewed again.

## Commands

```powershell
git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch
.\tools\Invoke-MvpPreflight.ps1
.\tools\Invoke-MvpPreflight.cmd
dotnet build WindowsFileCleaner.sln
dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj
dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj
dotnet run --project src\WindowsFileCleaner.App
.\tools\New-StorageScanSmokeFixture.cmd
.\tools\Start-MvpFixtureReview.cmd
.\tools\Start-MvpFixtureReview.cmd -ChecklistOnly
.\tools\New-StorageScanSmokeFixture.ps1
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
