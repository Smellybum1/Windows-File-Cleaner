# Windows File Cleaner

Windows File Cleaner is a local Windows-only WPF desktop app for reviewing storage under `C:\Users\moxhe`.

The current MVP centers on a read-only Storage Scan. It can also execute and undo Quarantine from the visible WPF app against synthetic fixture Cleanup Scopes, execute the first exact real-profile Quarantine phase for `C:\Users\moxhe` only after ADR 0017/0018 readiness and exact `QUARANTINE` pass, discover action-scoped Restore Manifests under the selected Quarantine Root, select one discovered Restore Manifest for review, preview selected restore confirmation, restore selected discovered fixture manifests, and restore exactly one selected real-profile Restore Manifest after ADR 0019 gates pass. The first live exact real-profile Quarantine batch and selected restore recovery loop both succeeded by user report, including rediscovery, rescan, and restored-path confirmation.

Current readiness evidence is tracked in `docs/features/2026-05-28-mvp-readiness-audit.md`.

The remaining path to a safe live product is tracked in `docs/features/2026-06-01-live-product-readiness-roadmap.md`.

Fresh-thread handoff notes live in `docs/codex/thread-handoff.md`.

## Safety Status

- Storage Scan does not modify scanned files.
- The visible WPF app can move files and narrow eligible folders when the Cleanup Scope is a recognized synthetic fixture and the exact Quarantine confirmation gate is open.
- The visible WPF app can move files and narrow eligible folders from the exact real-profile Cleanup Scope `C:\Users\moxhe` only for the first ADR 0018 phase after all readiness evidence, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, and immediate pre-execution revalidation pass. Cross-volume folder Quarantine uses a guarded copy-then-delete fallback because the preferred `D:` Quarantine Root is on a different drive than the real profile. Codex and automated tests do not click this real-profile movement path.
- The first user-clicked exact real-profile Quarantine trust batch succeeded by user report on 2026-06-01 with one small `pip\cache\http\b\c` row moved, one completed Restore Manifest, `moved 1, failed 0`, and zero readiness blockers. This does not enable broader scopes, broad Undo Quarantine, permanent deletion, or cleanup history.
- The visible WPF app can undo only the current synthetic fixture Quarantine execution; that current-fixture undo remains available after a rescan until undo is attempted.
- The visible WPF app can discover action-scoped Restore Manifests under the selected Quarantine Root without restoring them.
- The visible WPF app can select one discovered Restore Manifest and preview selected manifest readiness without restoring it.
- The visible WPF app can restore a selected discovered fixture Restore Manifest after selected manifest readiness and exact `RESTORE` confirmation.
- The visible WPF app can restore exactly one selected real-profile Restore Manifest whose Cleanup Scope is exactly `C:\Users\moxhe` after selected manifest readiness, exact `RESTORE`, and immediate selected-restore revalidation; Codex and automated tests do not run this movement.
- Cross-volume selected restore for directories uses the same guarded copy-then-delete directory fallback as cross-volume Quarantine. The first live selected restore retry for the first real-profile Quarantine manifest succeeded by user report with `Restored 1, failed 0`; follow-up rediscovery showed the manifest restored/already restored, rescan completed normally, and the restored `pip\cache\http\b\c` path appeared again.
- `.\tools\Summarize-RestoreManifests.cmd` can summarize action-scoped Restore Manifests under a selected Quarantine Root without launching WPF, scanning, moving, restoring, deleting, writing manifests, approving cleanup, or creating cleanup history.
- The visible WPF app keeps selected restore execution unavailable for custom non-fixture Restore Manifests and non-exact real-profile Restore Manifests.
- Discovery, selected-manifest review, and restore-readiness panes live in the Quarantine tab's `Restore Manifest Review` panel and do not expose all-manifest restore actions; selected restore goes through selected manifest readiness and the selected restore gate.
- The Restore Manifest review summary compactly names discovery, selected-manifest readiness, all-manifest readiness, selected restore gate, and selected restore result state with read-only/no-restore tooltip and automation help text.
- Selected restore gate panes show fixture/exact-real-profile versus preview-only scope status, keep selected manifest readiness separate from restore approval, fold real-profile selected-restore revalidation blockers into the executable gate, highlight the key gate/result/blocker line above the dense audit text including `Can execute`, exact `RESTORE`, and real-profile `Can proceed` evidence when available, and have visible hoverable `?` help cues for both the exact `RESTORE` confirmation field and the current gate state.
- The visible WPF app can preview all-manifest readiness for discovered manifests without restoring them, with a visible hoverable `?` help cue that mirrors the no-restore boundary.
- Custom/non-exact real-profile WPF Quarantine execution and broad WPF Undo Quarantine remain unavailable.
- The visible WPF app does not delete files.
- CSV exports write only to a path selected by the user.
- Scan Report Export and review navigation controls use tooltips and automation help text to keep report-only, in-memory, no-rescan, and no-file-modified boundaries available.
- Storage Review Filter buttons and Type, Size, and Category filters use tooltips and automation help text to keep review-only, no-rescan, no-file-modified, no-permission-change for Access issues, and not-cleanup-approval boundaries available.
- Cleanup Scope Scan Gate status has a visible hoverable `?` help cue that mirrors the current locked/ready summary into tooltip/help text and keeps clear that the status is read-only, does not start scanning by itself, and does not approve cleanup.
- Cleanup Scope Safety Note has a visible hoverable `?` help cue that mirrors the current fixture/real-profile/custom note into tooltip/help text and keeps clear that the note is read-only scope context, does not run preflight, create fixtures, start scanning by itself, persist approval, move files, or approve cleanup.
- The WPF header uses compact Review Shortlist totals positioned before compact scan totals when space allows, plus a wrapping status strip for the Cleanup Scope Safety Note, scan-gate summary, and scan-gate detail so wide windows use horizontal space before consuming review height.
- The WPF work surface uses horizontal tabs for Safety Summary, Review, Quarantine, and Main Grid so the dense review and readiness sections each have their own page while the Cleanup Scope, scan totals, Review Shortlist totals, and scan-gate header stay visible. Safety Summary shortcuts and current-session quarantined grid switches auto-focus Main Grid after they change which rows it shows, and Main Grid mirrors the active review lens above Storage Scan rows so the current filters/search/focus stay visible.
- The real-profile preflight and fixture-review acknowledgement has a visible hoverable `?` help cue that mirrors the acknowledgement tooltip/help text, stays hidden for fixture/custom scopes, and keeps clear that checking the box does not run preflight, create fixtures, start scanning, persist approval, or approve cleanup.
- Review Mix and Matched Review Mix have visible hoverable `?` help cues that mirror their dynamic summary text into tooltips and automation help text so whole-scan and active-review-lens counts stay read-only review context, not rescans, file modification, storage-savings proof, or cleanup approval. Main Grid also mirrors the active review lens as compact read-only orientation text above Storage Scan rows.
- Safety Summary is on its own tab page, its header starts with the visible panel name, summarizes compact risk counts with lightweight neutral/warning styling, header tooltip/help text mirrors the summary and names the current header state, its visible `?` help cue mirrors the same header help text, and its review shortcuts use disabled-state tooltips and automation help text to keep read-only shortcut scope, no-rescan, no-file-modified, no-permission-change, no-link-following, and not-cleanup-approval boundaries available. After a shortcut applies its review lens, Main Grid is selected so the filtered rows are visible.
- Review Shortlist is an in-memory review aid, not cleanup approval.
- Review Shortlist export and clear controls use tooltips and automation help text to keep report-only and in-memory-only boundaries available.
- Review Shortlist bulk actions label their scope as visible rows and include tooltips and automation help text so they apply only to the current displayed review window, not cleanup approval.
- Review Shortlist Safety Mix summarizes shortlisted rows from completed scan data only; its visible hoverable `?` help cue plus tooltip/help text mirrors the summary and keeps clear that it is review context, not cleanup approval, Quarantine readiness, or storage-savings proof.
- Quarantine Preview is a dry run only, and its semantically styled inline status plus visible hoverable `?` help cue, state-naming mirrored tooltip/help text, and preview/gate panes keep Review Shortlist and Quarantine Preview separate from cleanup approval.
- Quarantine Preview, Quarantine Readiness Summary, and Quarantine Execution Gate show an Execution Readiness contract that names fixture-executable, exact-real-profile, and custom-preview-only states, includes Quarantine Root Execution Safety, non-`D:` root acknowledgement evidence, Pre-Execution Revalidation, and Real-Profile Restore Readiness evidence when supporting evidence exists, and keeps grouped missing readiness dimensions visible without treating readiness display alone as cleanup approval. Pre-Execution Revalidation also blocks source files or folder descendants that are currently in use or inaccessible before movement.
- When real-profile readiness has many blockers, WPF preview/gate output prioritizes representative first-phase blockers before truncation so the 10-row cap, 1 GB cap, no-category rows, and strict descendant checks stay visible.
- Core Real-Profile Quarantine Approval Evidence records that exact `QUARANTINE` is necessary but not sufficient. WPF now uses it as one input for the exact `C:\Users\moxhe` first-phase execution guard while keeping custom and non-exact real-profile scopes blocked.
- Quarantine Preview and preview export controls use tooltips and automation help text to keep dry-run and report-only boundaries available; when parent/child overlap creates redundant preview rows, `Remove overlapping parents` removes broader parent rows from Review Shortlist, clears stale preview/gate state, and requires a fresh preview without modifying files.
- Restore Manifest Draft and Quarantine Confirmation Draft are in-memory readiness evidence only.
- Quarantine Root Selection, non-`D:` root readiness acknowledgement with a visible hoverable `?` help cue, Quarantine Preview, inline preview readiness, compact Quarantine Readiness Summary with concise key-blocker labels, highlighted Quarantine gate key status, Quarantine Execution Gate, fixture execution, and current-fixture undo are grouped in the dedicated Quarantine tab; its header starts with the visible panel name, summarizes shortlist, preview, current quarantined, and undo state with lightweight semantic styling, header tooltip/help text mirrors the summary and names the current header state, its visible `?` help cue mirrors the same header help text, while verbose gate details stay in a large constrained scroll area so real-profile evidence is readable without taking over the tab.
- Quarantine Execution Gate enables execution for fixture Cleanup Scopes after preview readiness and exact `QUARANTINE` confirmation, and for the exact `C:\Users\moxhe` first phase only after ADR 0018 readiness, approval evidence, and immediate revalidation also pass. Custom and non-exact real-profile scopes stay preview-only. The shortlist confirmation field has a visible hoverable `?` help cue that mirrors the exact-confirmation tooltip without becoming cleanup approval. The visible action is `Quarantine included shortlist`, meaning all included rows from the current Review Shortlist preview are moved together.
- ADR 0017 records a Real-Profile Quarantine Readiness Contract: real-profile WPF Quarantine execution remains unavailable until a richer readiness model, immediate pre-execution revalidation, Quarantine Root safety checks, trusted Undo Quarantine/recovery behavior, and explicit user approval semantics are designed and tested.
- ADR 0019 records the Real-Profile Selected Restore Execution contract. The current WPF app implements that selected-manifest-only restore path for exact `C:\Users\moxhe` Restore Manifests, and ADR 0017/0018 now gate the first exact real-profile forward Quarantine path.
- For exact real-profile selected Restore Manifests, the Selected Restore Execution Gate shows Selected Restore Pre-Execution Revalidation evidence, including stale missing-quarantine-path blockers, and reruns that revalidation immediately before restore movement.
- Current-Session Quarantined Review can switch the main grid to entries still in `Moved` state from the current in-memory Restore Manifest after fixture or approved exact real-profile Quarantine execution. It is read-only and does not discover older manifests, restore, move, delete, or create cleanup history; older/discovered Restore Manifests stay in manifest discovery/readiness panes for now.
- Review Grid Mode Status labels whether the main grid is showing Storage Scan rows or current-session quarantined items, while Main Grid Active Review Lens Summary mirrors the current Storage Scan filters/search/focus above Storage Scan rows and hides for current-session quarantined rows. The grid-mode line uses lightweight styling for neutral/informational/warning states, points back to `Back to scan rows`, warns when scan rows may be stale after fixture Quarantine execution, and its visible `?` help cue plus tooltip/help text mirrors dynamic text and current status state with read-only, no-rescan, no-restore, and not-cleanup-approval boundaries.
- `Current quarantined` shows the available current-session moved-entry count when entries exist, and `Current quarantined` / `Back to scan rows` tooltips/help text explain disabled states, current-session scope, read-only behavior, discovery for older manifests, and that returning to scan rows does not rescan or undo. Both controls select Main Grid after switching the visible row set.
- Quarantine and selected-restore execution controls have disabled-state tooltips and automation help text that keep fixture/exact-real-profile restore gates, fixture-only Quarantine gates, and custom/non-exact real-profile blockers visible.
- All-manifest and selected-manifest readiness controls have scope tooltips and automation help text that keep read-only/no-restore and selected-only/not-approval boundaries visible; all-manifest readiness also has a visible hoverable `?` help cue.
- Manifest discovery and selection controls have visible hoverable `?` help cues plus disabled-state tooltips and automation help text that keep discovery read-only and selection separate from restore approval.
- Quarantine Action Draft shows action-scoped item and manifest paths before execution creates them.
- Write-ahead Restore Manifest modeling shows planned status/write order before fixture execution writes the manifest.
- Restore Manifest File Store writes action-scoped manifest JSON during fixture-tested execution paths.
- Quarantine Executor is fixture-tested in the core library and wired to the WPF app for fixture scopes and the exact first real-profile phase only; directory moves can use a guarded cross-volume copy-then-delete fallback under the same write-ahead Restore Manifest path.
- Undo Quarantine Executor is fixture-tested in the core library and wired to the WPF app for current-fixture undo, fixture selected restore, and exact real-profile selected restore.
- Quarantine Manifest Discovery is read-only and does not move, restore, delete, create, or clean up files or folders.
- Selected Restore Manifest Review, Selected Restore Confirmation Draft, and Restore Readiness Preview are read-only and do not call Undo Quarantine execution.
- Selected Restore Execution calls Undo Quarantine Executor for selected discovered fixture manifests and exact real-profile selected Restore Manifests after the ADR 0019 gates pass.
- Fixture tests include source-level guards against accidental cleanup-execution filesystem calls, read-only readiness builders calling movement executors or manifest writers, WPF movement executor calls drifting outside the known gated execution bridge methods, and WPF real-profile approval evidence being used as execution enablement instead of display-only gate evidence.
- Real-profile scans require an explicit acknowledgement that MVP preflight and fixture review were run; the acknowledgement tooltip and automation help text keep clear that checking it does not run preflight, create fixtures, start scanning by itself, persist approval, or approve cleanup.
- Scan-gate ready wording is scope-specific: fixture scopes point later cleanup actions back to preview and exact confirmation, exact real-profile scopes point to the later ADR 0018 Quarantine gates, and custom scopes keep cleanup execution unavailable. The Scan button mirrors this wording in tooltips and automation help text.
- The Cancel scan control uses disabled-state tooltip and automation help text to keep clear that cancellation only requests stopping the in-progress read-only Storage Scan and does not move, delete, quarantine, restore, or approve cleanup.
- Cleanup Scope typed/browse and Quarantine Root typed/browse controls use tooltips and automation help text to keep path-selection, scan-gate, preview-only, and no-folder-creation boundaries available.
- Selected-row review actions use tooltips and automation help text to keep shortlist, focus, file-preview, copy, and Explorer inspection boundaries available.
- User-typed Storage Review Search is debounced for large real-profile scans; its input tooltip and automation help text expose prefix examples and read-only boundaries, and the status bar is the only pending-search indicator for now.

## Requirements

- Windows 11
- .NET 8 SDK and Windows Desktop runtime
- Local repo path: `D:\Codex\Windows File Cleaner`

## Verify Before Real Scan

Run the MVP preflight from the repository root before scanning real user files:

```powershell
.\tools\Invoke-MvpPreflight.cmd
```

The `.cmd` tool wrappers call the existing PowerShell scripts with process-scoped `-ExecutionPolicy Bypass`, so they work when direct `.ps1` execution is blocked without changing your machine or user execution policy. If your shell already allows scripts, the `.ps1` commands still work.

The preflight restores, builds, runs both test harnesses, runs the fixture generator in `-WhatIf` mode, prints the fixture review checklist in checklist-only mode, and runs `git diff --check`. It fails if any child command exits non-zero. It does not scan `C:\Users\moxhe`. After it passes, the success output points to the notes-enabled fixture launcher command for the next visible fixture pass. Checklist-only output also repeats that exact visible fixture command while stating that checklist-only mode did not run preflight, create fixture files, launch WPF, scan, move, restore, delete, or create cleanup history.

The individual commands are:

```powershell
dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config
dotnet build WindowsFileCleaner.sln --no-restore
dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build
dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly
git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check
```

Expected test output:

```txt
All WindowsFileCleaner.Tests checks passed.
All WindowsFileCleaner.App.Tests checks passed.
```

## CI Preflight

Pushes and pull requests to `main` run the same MVP preflight on GitHub Actions with a Windows runner and .NET 8. The CI job invokes `tools\Invoke-MvpPreflight.cmd`, then restores, builds, runs both test harnesses, runs the fixture generator in `-WhatIf` mode, prints the fixture review checklist in checklist-only mode, and runs `git diff --check`; any non-zero child command fails the job. It does not scan `C:\Users\moxhe`.

## Portable v1 Release

To create a local self-contained portable release folder and zip:

```powershell
.\tools\Publish-LocalRelease.cmd
```

The publisher runs MVP preflight by default, publishes the WPF app as `Release` / `win-x64` / self-contained, writes local release metadata, and creates:

```txt
.local\releases\windows-file-cleaner-vYYYYMMDD-HHMMSS\app\WindowsFileCleaner.App.exe
.local\releases\windows-file-cleaner-vYYYYMMDD-HHMMSS\README-FIRST.txt
.local\releases\windows-file-cleaner-vYYYYMMDD-HHMMSS.zip
.local\releases\windows-file-cleaner-vYYYYMMDD-HHMMSS.zip.sha256
```

The release artifacts stay under ignored `.local\releases`. The script prints the exact executable path plus normal and fixture launch commands. `README-FIRST.txt` travels with the folder and zip as the package-local start-here note, including launch options and the reversible-only safety boundary. The publisher records the packaged executable SHA-256 in `release-metadata.txt` and writes a sibling zip checksum sidecar. This is a portable package, not an installer: it does not create shortcuts, does not enable permanent deletion, does not add persisted cleanup history, and does not add broad/all-manifest restore. Portable v1 remains reversible-only: read-only Storage Scan, review, gated Quarantine, and selected restore.

Current accepted local package baseline: `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869`, with completed ignored acceptance notes at `.local\release-acceptance\release-acceptance-20260602-011743.md`. The notes summary reports verifier/current-commit/normal-launch/fixture-launch evidence recorded, overall result `Pass`, and `6 pass, 0 issue, 0 not checked, 0 not recorded`; those notes are local evidence only and are not app persistence or cleanup history.

Each release folder also contains ignored local launch scripts:

```txt
.local\releases\windows-file-cleaner-vYYYYMMDD-HHMMSS\Launch-WindowsFileCleaner.cmd
.local\releases\windows-file-cleaner-vYYYYMMDD-HHMMSS\Launch-WindowsFileCleaner-Fixture.cmd
```

The first launches the packaged app normally. The fixture script launches the same packaged app with the repo-local smoke fixture Cleanup Scope; it does not create the fixture or click `Scan`.

To verify the latest local package without launching WPF or scanning anything:

```powershell
.\tools\Test-LocalRelease.cmd
```

The verifier checks the ignored release folder, executable, `README-FIRST.txt`, zip, zip checksum sidecar, metadata, executable checksum, safety-boundary lines, and zip entries. By default it warns if the package commit is behind current `HEAD`; use `.\tools\Test-LocalRelease.cmd -RequireCurrentCommit` when you need the package to exactly match the current commit.

To print or start the latest verified local package from the repo root:

```powershell
.\tools\Start-LocalRelease.cmd -PrintOnly -RequireCurrentCommit
.\tools\Start-LocalRelease.cmd -Fixture -PrintOnly -RequireCurrentCommit
```

Remove `-PrintOnly` when you intentionally want to launch the packaged WPF app. The fixture mode only prefills the Cleanup Scope with the repo-local smoke fixture; it does not create fixture files or click `Scan`. The launcher verifies the package first unless `-SkipVerify` is used, and launching still does not move, restore, delete, or approve cleanup.

The launcher output also prints the package-local `README-FIRST.txt` path and the matching release-local launch script path, so you can either run from the terminal or open the packaged folder and use the start-here note.

To print the package-level acceptance checklist without launching WPF:

```powershell
.\tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit
.\tools\Start-LocalRelease.cmd -Fixture -ChecklistOnly -RequireCurrentCommit
```

Checklist-only mode verifies the package by default, prints normal and fixture launch review steps with exact launch commands, and does not launch WPF, click `Scan`, move, restore, delete, approve cleanup, or create cleanup history.

To also write a local, ignored markdown notes template for package acceptance:

```powershell
.\tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes
```

The template is written under `.local\release-acceptance`, stamps the repo branch/commit, current worktree state, package metadata commit/preflight evidence, package paths, exact normal and fixture launch commands, local evidence checkboxes, and exact post-pass summary commands. When the launcher verifies the package first, the notes pre-record verifier evidence; when `-RequireCurrentCommit` is used and passes, they also pre-record current-commit evidence. Normal launch and fixture launch/scan acceptance remain manual evidence. After filling the notes, summarize the latest ignored notes or a specific notes file:

```powershell
.\tools\Summarize-LocalReleaseAcceptanceNotes.cmd
.\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-YYYYMMDD-HHMMSS.md"
.\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete
```

The summary helper prints the stamped launch commands from ignored notes and reads ignored notes only; it does not launch WPF, scan, move, restore, delete, approve cleanup, or create cleanup history.

After you have actually completed the package acceptance pass, you can record the manual evidence in the latest ignored notes without hand-editing markdown:

```powershell
.\tools\Record-LocalReleaseAcceptanceNotes.cmd -RecordManualAcceptance
.\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete
```

`Record-LocalReleaseAcceptanceNotes` updates ignored `.local` notes only. Use it only after README review, normal launch, fixture launch/read-only fixture Scan, portable-boundary review, and real-profile stop-boundary confirmation are complete.

To print or start the latest accepted portable package from completed ignored acceptance notes:

```powershell
.\tools\Start-AcceptedLocalRelease.cmd -PrintOnly
.\tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly
```

Remove `-PrintOnly` when you intentionally want to launch the accepted package. This command first checks that the latest or selected `.local\release-acceptance` notes are complete, then delegates to `Start-LocalRelease.cmd` for package verification and launch-command printing. It does not require the package commit to match newer docs-only commits; use `Start-LocalRelease.cmd -RequireCurrentCommit` only when you want a package cut from the exact current `HEAD`.

## WPF Fixture Smoke

Use the fixture review launcher for the manual fixture UI pass:

```powershell
.\tools\Start-MvpFixtureReview.cmd
```

After a fresh preflight pass, use the notes-enabled shortcut printed by preflight to avoid rerunning preflight while still creating acceptance notes:

```powershell
.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes
```

The launcher runs preflight, creates a small synthetic Cleanup Scope inside the repo, and launches the WPF app with that scope. The app does not auto-scan; click `Scan` yourself after it opens.
Before launching, the script prints a compact fixture review checklist grouped by fixture review area: Scan header and gate; Safety Summary, Review, and Main Grid; Quarantine Preview and fixture Quarantine; Restore Manifest review and selected restore; and Real-profile and custom blockers. The checks cover the main safety, compact header Review Shortlist totals positioned before Total size when space allows, compact header scan totals, horizontal Safety Summary / Review / Quarantine / Main Grid tabs, Safety Summary shortcuts and `Current quarantined` / `Back to scan rows` Main Grid auto-focus, Main Grid active review lens summary, compact wrapping scan-header status strip, Cleanup Scope Safety Note hoverable `?` help cue, search, Safety Summary and Quarantine Shortlist tab pages, panel-name header summaries, header summary state styling plus hoverable `?` help cues and state-naming tooltip/help text, styled Review Grid Mode Status plus a hoverable `?` help cue and state-naming tooltip/help text, Quarantine Root Safety Note hoverable `?` help cue, non-`D:` root readiness acknowledgement plus its hoverable `?` help cue, styled inline Quarantine Preview readiness plus a hoverable `?` help cue and state-naming tooltip/help text, compact Quarantine Readiness Summary including preview-only `movement unavailable` wording and key-blocker labels such as `custom scope preview-only`, `exact profile scope`, `pre-execution revalidation`, `10-row cap`, `1 GB cap`, `no-category rows`, and `strict descendant checks` when those blockers apply, `Remove overlapping parents` for redundant parent/child previews, highlighted Quarantine and selected-restore key-status strips, Quarantine Execution Gate hoverable `?` help cue, Review Shortlist Safety Mix hoverable `?` help cue, Review Shortlist labels/tooltips, preview/report tooltips, preview approval-boundary, exact-confirmation `?` help cues, fixture execution, undo, `Current quarantined (N)` / `Back to scan rows`, a dedicated Quarantine-tab `Restore Manifest Review` panel for `Discover manifests`, `Preview all-manifest readiness`, Restore Manifest review summary states, and cue/control wrapping, a separate selected-restore gate step for selected-only readiness, selected-restore scope-status checks, selected restore revalidation evidence for exact real-profile Restore Manifests when available, and the Selected Restore Execution Gate hoverable `?` help cue in waiting, closed, open, and restored states without crowding the gate area, plus a final real-profile/custom blocker check for ADR 0017, ADR 0018 first-phase limits, ADR 0019, and Real-Profile Quarantine Approval Evidence.
That final real-profile/custom blocker check is not an instruction to scan `C:\Users\moxhe` during the fixture pass; use custom preview-only paths or the existing synthetic real-profile readiness coverage unless you intentionally start a separate real-profile retest after MVP preflight.

The `.cmd` launcher calls the existing PowerShell script with process-scoped `-ExecutionPolicy Bypass`, so it works when direct `.ps1` execution is blocked without changing your machine or user execution policy. If your shell already allows scripts, `.\tools\Start-MvpFixtureReview.ps1` still works.

To print only that checklist without running preflight, creating fixture files, or launching WPF:

```powershell
.\tools\Start-MvpFixtureReview.cmd -ChecklistOnly
```

Checklist-only output ends with the exact post-preflight visible fixture command, `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`, plus the no-preflight/no-fixture/no-WPF/no-scan/no-movement boundary for that checklist-only run.

To also write a local, ignored markdown notes template for the manual pass:

```powershell
.\tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes
```

The template is written under `.local\fixture-review-acceptance`, stamps the repo path, Git branch/commit, worktree status at notes creation, .NET SDK, WPF app project/target framework/WPF flag, required preflight command, local evidence checkboxes, and exact post-pass summary commands, then gives each grouped checklist item pass/issue/not-checked slots. Use `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` after a successful preflight when you want the same notes template created before the visible fixture app launches. When notes are written, the launcher also prints exact follow-up summary commands for that notes file, including the `-RequireComplete` completion check.

After a fixture pass, summarize the latest ignored notes or a specific notes file:

```powershell
.\tools\Summarize-FixtureAcceptanceNotes.cmd
.\tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-YYYYMMDD-HHMMSS.md"
.\tools\Summarize-FixtureAcceptanceNotes.cmd -RequireComplete
```

The summary helper reads local notes only, reports the stamped worktree status at notes creation, acceptance-evidence checkbox states, the overall result, checklist totals, and issue/not-checked/not-recorded items with compact notes or prompt previews, and does not launch WPF, scan, move, restore, delete, or create cleanup history. `-RequireComplete` exits non-zero until the local notes have recorded preflight/worktree evidence, an overall pass result, and no not-checked or not-recorded checklist items.

For focused troubleshooting, the individual fixture commands are:

```powershell
.\tools\New-StorageScanSmokeFixture.cmd
```

Then run the printed command, which has this shape:

```powershell
dotnet run --project src\WindowsFileCleaner.App -- --scope "D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture"
```

This only fills the Cleanup Scope box. Click `Scan` yourself after the app opens.

Use `-SkipChecklist` only when you intentionally want the launcher output without the reminder checklist.

The automated `WindowsFileCleaner.App.Tests` project also scans a synthetic fixture through the WPF shell, exercises read-only review interactions, proves fixture-only Quarantine execution and undo, verifies manifest discovery, selected manifest review, selected restore confirmation gate, fixture selected restore execution, all-manifest readiness preview, and read-only Quarantine Root Execution Safety, Pre-Execution Revalidation, Real-Profile Restore Readiness, and Selected Restore Pre-Execution Revalidation evidence in preview/gate output, verifies custom non-fixture execution, synthetic real-profile and real-profile child readiness output, keeps ADR 0018 first-phase real-profile blockers visible in WPF preview/gate output, proves exact real-profile selected restore gates can open without executing movement in tests, proves stale real-profile selected restore remains blocked, checks review navigation/export, report/preview, browse, selected-row, execution-gate, selected-restore gate, and restore-readiness tooltip and automation help text boundaries, and checks that the review toolbars use wrapping layout, but it does not replace checking the visible layout and controls by eye.

## Real-Profile Selected Restore Trust Test

Use this only when you intentionally want to test the ADR 0019 selected real-profile restore path. It does not run real-profile Quarantine and does not use existing personal files.

1. Run full preflight:

```powershell
.\tools\Invoke-MvpPreflight.cmd
```

2. Create one sacrificial Restore Manifest:

```powershell
.\tools\New-RealProfileSelectedRestoreTrustManifest.cmd
```

The helper writes a synthetic quarantined file and Restore Manifest under `D:\WindowsFileCleanerQuarantine`, targets `C:\Users\moxhe\WindowsFileCleanerRestoreTrustTest\restore-target.txt`, and refuses to continue if that restore target already exists.

3. Launch the app:

```powershell
dotnet run --project src\WindowsFileCleaner.App -- --scope "C:\Users\moxhe"
```

4. In the app, open `Quarantine`, set Quarantine Root to `D:\WindowsFileCleanerQuarantine`, then use `Discover manifests`.
5. Select the generated `real-profile-selected-restore-trust-*` Restore Manifest.
6. Click `Preview selected manifest readiness`.
7. Click `Preview selected restore gate`.
8. Type exactly `RESTORE`.
9. Click `Restore selected manifest` only if the highlighted selected-restore strip says `Can execute: yes`, exact `RESTORE` matched, and, for exact real-profile selected restore, `Can proceed: yes`.
10. Confirm the result says `Selected restore result: Restored 1`, then rediscover manifests and rescan before further review.

Stop there. Do not use this trust test as approval for real-profile Quarantine execution.

## Restore Manifest Summary

To summarize action-scoped Restore Manifests under the default Quarantine Root without restoring anything:

```powershell
.\tools\Summarize-RestoreManifests.cmd
```

To summarize a specific Quarantine Root and include entry-level paths:

```powershell
.\tools\Summarize-RestoreManifests.cmd -QuarantineRoot "D:\WindowsFileCleanerQuarantine" -ShowEntries
```

The summary helper reads `actions\*\restore-manifest.json`, reports manifest counts, entry status counts, size, cleanup scopes, undo-work and recovery-review flags, and discovery issues. It is read-only: it does not launch WPF, scan, move, restore, delete, write manifests, approve cleanup, or create cleanup history. Use `-RequireAny` when a verification step should fail if no valid Restore Manifests are found.

## Run The App

```powershell
dotnet run --project src\WindowsFileCleaner.App
```

Default Cleanup Scope:

```txt
C:\Users\moxhe
```

When the app is opened against the default real-profile Cleanup Scope, `Scan` stays disabled until you tick the preflight and fixture-review acknowledgement in the header.
The header also shows a scan-gate status line with a hoverable `?` help cue, plus Scan button tooltip/automation help text, so the locked or ready state is visible before scanning, with scope-specific wording for fixture, real-profile, and custom scopes.

## Manual MVP Check

After the app opens:

1. Confirm the scope box shows the intended Cleanup Scope; use `Browse...` if you want to choose a fixture or custom folder before scanning, and confirm its tooltip says browsing does not start a scan, bypass the real-profile gate, or approve cleanup.
2. Confirm the Cleanup Scope Safety Note matches the path: fixture first for smoke testing, real profile only after preflight. Its `?` help cue should mirror the note and keep clear that the note is read-only scope context, not a scan start, preflight run, fixture creation, persisted approval, or cleanup approval. The header safety note, scan-gate summary, and scan-gate detail should sit in a compact wrapping strip that uses horizontal space before pushing content downward.
3. For the real profile, confirm the scan-gate status says the scan is locked and its `?` help cue mirrors the current locked/ready state without implying auto-scan or cleanup approval, then tick the acknowledgement that MVP preflight and fixture review were run; fixture scopes do not require this real-profile acknowledgement. The acknowledgement `?` help cue should mirror the checkbox tooltip/help text and keep clear that this is local acknowledgement only, not preflight execution, fixture creation, auto-scan, persistence, or cleanup approval.
4. Click `Scan`; the `Cancel` tooltip/help text should keep clear that cancellation only requests stopping an in-progress read-only Storage Scan and does not move, delete, quarantine, restore, or approve cleanup.
5. Confirm the status says no files were modified.
6. Review the compact Review Shortlist totals for shortlisted size, folders, and files; they should sit to the left of `Total size` when space allows. Review the compact header scan totals for total size, folders, files, and access issues. The shortlist size is row-size review context, not storage savings or cleanup approval.
7. If the status or filter summary says `rows 1-2,000 of ... matched`, use `Next rows` / `Previous rows` to move through matched in-memory rows, or narrow with search and filters. Their tooltips should keep no-rescan and no-file-modified wording visible.
8. Treat row sizes as triage clues, not storage savings; folder rows include children and can overlap with child rows.
9. Use Review Mix, Matched Review Mix, and Safety Summary to inspect the cleanup scope root, high-risk, protected, access issue examples, Quarantine candidate examples, No category examples, reparse point, quarantine candidate, and no-category rows. Review Mix summarizes the whole scan; Matched Review Mix summarizes the current filters/search/focus. Their `?` help cues should mirror the current summaries and keep them read-only, no-rescan, no-file-modified, not-storage-savings-proof, and not-cleanup-approval. Safety Summary shortcut tooltips and automation help text should keep shortcuts read-only and separate from rescans, permission changes, link following, file modification, or cleanup approval. Clicking a Safety Summary shortcut should select Main Grid after applying its review lens, and the Main Grid active review lens summary should show the applied filter/search/focus above Storage Scan rows.
10. Use Storage Review Search for specific names such as `pip`, `NVIDIA`, `Codex`, app names, or game/mod-manager folders; use prefixes such as `path:pip`, `parent:C:\Users\moxhe\AppData`, `under:C:\Users\moxhe\AppData`, `category:Python package cache`, `rating:High risk`, `recommendation:Quarantine candidate`, `access:readable`, or `access:access issue` when you want one field. The search input tooltip and automation help text should keep those examples plus read-only/no-rescan/no-cleanup-approval wording available.
    Search typed into the box applies after a short pause so large scan results do not re-filter on every keystroke.
11. Use the `Relative path`, `Parent`, `Contents`, and `Access` columns for short, hashed, container, or unreadable row names; sort `Contents` when you want to compare rows by total contained items, then select large folders and inspect relative path, parent/depth context, Evidence, cache-specific Review guidance, Descendant review summary, Largest immediate children, the Largest hotspot trail, and selected-row action tooltips.
12. Use the Descendant review summary to see the selected folder's descendant mix by rating, quarantine candidate count, protected rows, access issues, reparse points, and no-category rows; these counts are review context, not cleanup approval.
13. Use the Largest hotspot trail to see the biggest nested path through a selected folder; the sizes overlap down the trail and are not storage savings.
14. Use `Show children` on a selected folder to focus the grid on its immediate children; this applies a `parent:` search, resets other review lenses to All, and remains read-only. Its tooltip should keep no-rescan, no-file-modified, and not-cleanup-approval wording visible, and the status line should say `Reset view` returns to all rows.
15. Use `Show descendants` on a selected folder to focus the grid on every scanned descendant; this applies an `under:` search, excludes the selected folder itself, resets other review lenses to All, updates Matched Review Mix, and remains read-only. Its tooltip should keep no-rescan, no-file-modified, and not-cleanup-approval wording visible.
16. Use the Type filter to switch between all rows, files only, and folders only; use the Size filter to focus on rows such as `100 MB+`, `1 GB+`, or `5 GB+`. Their tooltips and automation help text should keep filtering read-only and separate from rescans, file modification, storage-savings proof, or cleanup approval.
17. Select small text files and use `Preview file` only when you intentionally want a bounded read-only text snippet; binary, Credential Data, and unsupported files should not render as text, and the tooltip should keep bounded/no-file-modified wording visible.
18. Try category filters such as Cleanup scope root, App cache, Python package cache, GPU shader cache, Large old file, Cloud sync data, Credential data, Windows app data, Installed application, Game data, Protected location, and No category. The Category filter tooltip/help text should keep this as a read-only review lens, not cleanup approval.
19. Use `Clear search` or `Reset view` after stacking filters/search; reset clears the review lens but keeps Review Shortlist. Their tooltips should keep no-rescan, no-file-modified, and shortlist-preserving wording visible.
20. Add a likely-safe cleanup candidate to the Review Shortlist; specific rebuildable cache rows such as `DXCache` or `pip\Cache` may appear here, while broad parent folders should stay inspection-first. Check Review Shortlist Safety Mix for high-risk, protected, access issue, no-category, and largest-row context; its `?` help cue plus tooltip/help text should mirror the mix and say it does not rescan, modify files, prove Quarantine readiness, prove storage savings, or approve cleanup. Confirm selected-row and visible-row shortlist tooltips keep row/window scope, no-file-modified behavior, and not-cleanup-approval wording clear. Confirm `Export shortlist` and `Clear shortlist` tooltips keep report-only and in-memory-only wording clear.
21. Confirm the horizontal tabs give Safety Summary, Review, Quarantine, and Main Grid their own pages, with Main Grid selected by default and the compact Review Shortlist totals plus scan totals staying visible in the header. Safety Summary shortcut actions should select Main Grid after applying their review lens, and Main Grid should mirror the active review lens above Storage Scan rows without implying rescan, file modification, or cleanup approval; the lens summary should hide while current-session quarantined rows are showing. Safety Summary and Quarantine Shortlist should start expanded inside their tab pages, their headers should start with the visible panel name, and header tooltip/help text should mirror the compact header summary, name the current header state, and say it is read-only review context, not cleanup approval. Their header `?` help cues should mirror the same dynamic tooltip/help text. The Safety Summary header should start neutral before scan and use warning styling when scan safety signals need review, without reading like cleanup approval. The Quarantine Shortlist header should use lightweight state styling for empty, needs-preview, preview-ready, blocked/stale, current-quarantined, and undo-completed states without reading like cleanup approval. Confirm the Quarantine tab groups Quarantine root, non-`D:` root readiness acknowledgement and its `?` help cue, `Preview shortlist quarantine`, semantically styled inline preview readiness, compact Quarantine Readiness Summary, `Export preview`, `Remove overlapping parents`, `Current quarantined`, `Back to scan rows`, highlighted Quarantine key-status strip, confirmation text and its `?` help cue, `Quarantine included shortlist`, `Undo fixture quarantine`, and the gate text without shrinking the Main Grid tab to a tiny strip. Confirm the Quarantine root points to the intended fully qualified preview/execution destination and that the safety note matches it, typing or browsing if needed; the safety note `?` help cue should mirror the note and say this is read-only preview-root context, does not create folders, move files, write manifests, or approve cleanup, and the browse tooltip should say it selects preview paths only and does not create folders, move files, or approve cleanup. For a fully qualified non-`D:` root, the acknowledgement should enable as readiness evidence only, its `?` help cue should mirror the same no-create/no-move/no-approval boundary, clear stale preview when changed, and not create folders or approve cleanup. Then click `Preview shortlist quarantine`; the inline preview readiness line should show included/blocked/redundant counts, readiness blockers, no-file-modified wording, current status state, and that preview is not cleanup approval, with success/warning/error styling that does not feel like cleanup approval. Its `?` help cue should mirror the same state and no-create/no-move/no-restore/no-delete/not-cleanup-approval boundary. The Quarantine Readiness Summary should show fixture-ready/open, exact real-profile ready/open, preview-only, stale-executed, or undo-completed wording; preview-only summaries should say `movement unavailable` and include concise `Key blockers:` labels such as `custom scope preview-only`, `exact profile scope`, `pre-execution revalidation`, `10-row cap`, `1 GB cap`, `no-category rows`, and `strict descendant checks` when blockers apply; tooltip/help text should include `Summary state:` plus no-create/no-move/no-restore/no-delete/no-manifest-write/not-cleanup-approval boundaries. Broad parent rows should be blocked when protected descendants are present, blocked descendant examples should use relative paths, confirmation readiness blockers should be separate from preview row details, approval-boundary wording should keep shortlist/preview separate from cleanup approval, redundant parent/child overlaps should keep the gate blocked until `Remove overlapping parents` or manual shortlist edits clear the overlap and a fresh preview is created, Preview shortlist quarantine and Export preview tooltips should keep dry-run/report-only wording visible, execution scope status and disabled control tooltips should distinguish fixture, exact real-profile, and preview-only custom/non-exact real-profile scopes, the Execution Readiness contract should show fixture/custom/real-profile readiness state, read-only Quarantine Root Execution Safety, non-`D:` acknowledgement evidence, Pre-Execution Revalidation, selected real-profile restore trust evidence, and grouped missing dimensions without replacing exact confirmation, the Quarantine Execution Gate `?` help cue should mirror the current gate state and not-cleanup-approval boundary, the Quarantine Action Draft should show the all-included-shortlist execution target plus action-scoped item and manifest paths, and the write-ahead Restore Manifest should show planned write-before-move ordering inside the constrained gate-detail scroller.
22. On a fixture Cleanup Scope only, typing `QUARANTINE` once should enable `Quarantine included shortlist`; clicking it moves all included Review Shortlist rows into the action-scoped quarantine path, writes `restore-manifest.json`, clears stale shortlist state, enables `Undo fixture quarantine`, enables `Current quarantined (N)`, and says that rescan refreshes review rows.
23. On that same fixture execution, `Current quarantined (N)` should show the current-session moved-entry count and switch the main grid to current-session moved Restore Manifest entries, while `Back to scan rows` should return to normal Storage Scan rows without rescanning or modifying files; both controls should select Main Grid after switching the visible rows. The Review Grid Mode Status above the grid should clearly name the active grid mode, and the Main Grid active review lens summary should still explain the Storage Scan filter/search/focus when scan rows are showing. Keep the quarantined view read-only, use warning styling when scan rows may be stale, use informational styling for current-session quarantined rows, point back to `Back to scan rows`, and expose a `?` help cue plus tooltip/help text that mirrors the status, names the current status state, and says it does not rescan, modify files, restore files, or approve cleanup. Confirm `Current quarantined (N)` and `Back to scan rows` tooltips/help text explain current-session-only scope, disabled states, no older-manifest discovery from that grid, and no rescan/undo from returning to scan rows. Clicking `Undo fixture quarantine` should restore the synthetic file/folder from quarantine, update the Restore Manifest, disable repeat undo, and keep stale-state wording visible. If you rescan before undo, the moved file may disappear from Storage Scan rows, but the status bar should say `Undo fixture quarantine` remains available in the Quarantine shortlist area until the undo attempt.
24. In the Quarantine tab's `Restore Manifest Review` panel, use `Discover manifests` against the selected Quarantine Root after checking its `?` help cue; it should show read-only Restore Manifest summaries or discovery issues, state that no all-manifest restore action is available, and keep tooltip/help text clear that discovery does not restore, move, delete, clean up folders, or create cleanup history. Resize or visually scan the manifest controls enough to confirm each `?` cue stays paired with its related control instead of wrapping by itself.
25. Select a discovered Restore Manifest and use `Preview selected manifest readiness`; it should show readiness for that manifest only without moving files, the selection `?` help cue plus selection/readiness tooltips should keep selected-only/not-approval wording visible, and it should route any selected restore through the selected restore gate.
26. Use `Preview selected restore gate`, then type `RESTORE`; for a fixture Restore Manifest it should show fixture/exact-real-profile scope status, approval-boundary wording, disabled-control tooltip wording, a highlighted selected-restore key-status strip with `Can execute: yes` and exact `RESTORE` evidence, and `Can execute: yes` in the detailed gate text. The confirmation field `?` help cue should mirror the exact `RESTORE` selected-restore boundary, and the Selected Restore Execution Gate `?` help cue should mirror waiting, closed, open, and restored states without crowding the gate area. `Restore selected manifest` should restore the synthetic fixture file while telling you to rediscover and rescan. For an exact real-profile selected Restore Manifest, the same highlighted strip should also show `Can proceed: yes`; the same gate can open only when Selected Restore Pre-Execution Revalidation passes, and Codex and fixture review should not click real-profile restore unless the user explicitly chooses that specific movement test.
27. Use `Preview all-manifest readiness` against the selected Quarantine Root after checking its `?` help cue; it should show restorable, blocked, already-restored, or recovery-review rows across discovered manifests without moving files, and its tooltip should keep read-only/no-restore wording visible.
28. On custom non-fixture or non-exact real-profile Cleanup Scopes, typing `QUARANTINE` should still leave `Quarantine included shortlist` disabled with a scope-specific blocker and no undo action. During the fixture pass, do not scan `C:\Users\moxhe` just to satisfy this check; use custom preview-only paths or existing synthetic real-profile readiness evidence unless you intentionally start a separate real-profile retest after MVP preflight. For exact synthetic real-profile readiness evidence, the Quarantine Execution Gate should show Real-Profile Quarantine Approval Evidence, and missing live source evidence should keep `Can approve real-profile movement: no` through Pre-Execution Revalidation. ADR 0018 first-phase blockers such as 10 rows, 1 GB, no-category rows, and strict descendant checks should stay visible when they apply. Selected restore should keep ADR 0019 visible by staying unavailable for custom and non-exact real-profile manifests even when `RESTORE` is typed.
29. Export CSV reports only when you intentionally choose an output file; the main report export follows the active filters/type/size/search, includes relative path, parent/depth, and access-status context for recursive rows, and the suggested filename includes the search term when one is active. Its tooltip should keep report-only, not-cleanup-approval, and no-scanned-file-modified wording visible.

## Current Workflow

The intended review flow is:

1. Run fixture tests.
2. Run the WPF app smoke tests.
3. Confirm the Cleanup Scope Safety Note and Cleanup Scope browse tooltip before scanning.
4. Run Storage Scan.
5. Inspect high-risk and protected rows first.
6. Check whether the grid is showing all matched rows or one 2,000-row display window; Previous/Next rows tooltips should keep in-memory/no-rescan boundaries visible.
7. Use `Next rows` / `Previous rows`, Storage Review Search, `parent:` search, `under:` search, Type filter, Size filter, category filters, and Matched Review Mix to understand large buckets and specific app/tool paths.
8. Use `Clear search` or `Reset view` when the active review lens becomes too narrow; Reset view does not clear Review Shortlist and its tooltip should say so.
9. Use Selected Path Hierarchy Context, Selected File Content Preview, Selected Path Review Guidance, Selected Folder Subtree Summary, Child Breakdown, Storage Hotspot Trail, Selected Folder Child Focus, Selected Folder Descendant Focus, Copy path, and Open in Explorer for manual inspection; selected-row action tooltips should keep review-only and inspection-only boundaries visible. If the Windows Clipboard is busy, Copy path should show a warning instead of closing the app.
10. Add interesting rows to Review Shortlist; use Review Shortlist Safety Mix to sanity-check the shortlisted row mix, and use `Shortlist visible rows` / `Remove visible rows` only for the currently displayed review window. Safety Mix `?` help cue plus tooltip/help text should mirror the visible summary and keep the no-rescan/no-file-modified/no-readiness-proof/no-savings-proof/not-cleanup-approval boundary visible, while export/clear tooltips keep report-only and in-memory-only boundaries visible.
11. Check or browse the Quarantine root and generate Quarantine Preview for Review Shortlist readiness review; the Quarantine Root Safety Note `?` help cue, styled inline preview readiness line and its tooltip/help text, compact Quarantine Readiness Summary with preview-only `movement unavailable` and `Key blockers:` labels such as `custom scope preview-only`, `exact profile scope`, `pre-execution revalidation`, `10-row cap`, `1 GB cap`, `no-category rows`, and `strict descendant checks` when relevant, browse tooltip, preview/export tooltips, and `Remove overlapping parents` tooltip should keep preview-only, no-folder-creation, dry-run, report-only, overlap-cleanup, no-move/no-restore/no-delete, no-manifest-write, and not-cleanup-approval boundaries visible.
12. For fixture scopes, optionally type `QUARANTINE` once after checking the shortlist confirmation `?` help cue, run `Quarantine included shortlist` for all included Review Shortlist rows, use `Current quarantined (N)` / `Back to scan rows` to review moved current-session entries, then use `Undo fixture quarantine` before or after a rescan to prove the reversible visible workflow.
13. Use `Discover manifests` after checking its `?` help cue when you want read-only status for action-scoped Restore Manifests under the selected Quarantine Root.
14. Select one discovered Restore Manifest after checking the selected manifest `?` help cue, then use `Preview selected manifest readiness` when you want one-action blocker evidence before any future broad Undo Quarantine.
15. Use `Preview selected restore gate` and type `RESTORE` after checking the selected restore confirmation `?` help cue; fixture selected manifests can be restored, and exact real-profile selected manifests can be restored only after passing immediate revalidation. Custom and non-exact real-profile selected restore stays unavailable.
16. Use `Preview all-manifest readiness` after checking its `?` help cue when you want read-only blocker evidence across all discovered manifests.
17. Use `.\tools\Summarize-RestoreManifests.cmd` when you want terminal-only Restore Manifest status without launching WPF or restoring anything.
18. For the exact real-profile scope `C:\Users\moxhe`, use `Quarantine included shortlist` only for a specifically approved first-phase batch after ADR 0018 readiness, exact `QUARANTINE`, approval evidence, and immediate revalidation pass. Broad Undo stays unavailable; recovery goes through `Discover manifests` and selected restore readiness.
19. Stop before custom cleanup execution, broad Undo, permanent deletion, or cleanup history.

## Not Implemented Yet

- Custom/non-exact real-profile WPF Quarantine execution.
- Broad/all-manifest real-profile WPF Undo Quarantine.
- Permanent deletion.
- Persisted cleanup history.

Those workflows require separate Grill with Docs packets after selected-restore recovery for the first exact real-profile Quarantine manifest is trusted.
