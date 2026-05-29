# Glossary

---
last_reviewed: 2026-05-29
owner: project-owner
stability: evolving
---

## Purpose

This glossary is the naming authority for the project.

Use it to keep code, filenames, database fields, API routes, UI copy, tests, docs, and Codex prompts aligned.

## Naming policy

Use these terms consistently in:

- Code
- Filenames
- Database fields
- API routes
- UI copy
- Tests
- Documentation
- Codex prompts

If a term needs to change, update this glossary first, then update code and docs consistently.

## Preferred terms

| Preferred term | Meaning | Use in code | Notes |
|---|---|---|---|
| System Drive | The Windows operating-system partition. | `SystemDrive`, `systemDrivePath` | Known current value is `C:`. |
| User Profile Root | The top-level folder containing Windows user profiles. | `UserProfileRoot`, `userProfileRootPath` | Known current value is `C:\Users`. |
| Cleanup Scope | The path or paths allowed for a scan or cleanup run. | `CleanupScope`, `cleanupScopePath`, `cleanupScopePaths` | Initial value is `C:\Users\moxhe`. Must constrain both scanning and cleanup execution. |
| Cleanup Scope Root | The top-level path selected for a Storage Scan run. | `CleanupScopeRoot`, `BloatCategory.CleanupScopeRoot` | Shown as High risk / Keep so the whole scanned folder is reviewed through child rows, not treated as a cleanup target. |
| Cleanup Scope Selection | Pre-scan action of typing or browsing for the Cleanup Scope path. | `BrowseScopeButton`, `ScopePathBox` | Does not start a scan, bypass preflight acknowledgement, or approve cleanup. Typed-path and browse tooltips plus automation help text should keep those boundaries available. |
| Cleanup Scope Safety Note | Read-only UI text explaining whether the current Cleanup Scope looks like a fixture, real profile, custom path, blank path, or invalid path. | `CleanupScopeSafetyNote`, `CleanupScopeSafetyNoteBuilder` | Reminds the user to run preflight and fixture review before real-profile scans. Not scan approval. |
| Cleanup Scope Scan Gate | Read-only scan-start gate derived from the Cleanup Scope and real-profile acknowledgement. | `CleanupScopeScanGate`, `CleanupScopeScanGateBuilder` | Real-profile scans require explicit acknowledgement that MVP preflight and fixture review were run. Does not run preflight or create fixtures. Ready wording should stay scope-specific about later cleanup execution availability; acknowledgement tooltip/help text and `Scan` tooltip/help text should mirror the gate boundary. |
| Storage Scan | The first read-only workflow for recursively scanning the Cleanup Scope. | `StorageScan`, `storageScanId` | Must not modify files. Scan cancellation tooltip/help text should keep clear that cancellation only requests stopping the in-progress read-only scan and does not move, delete, quarantine, restore, or approve cleanup. |
| Cleanup Candidate | A file or folder proposed for review as potentially removable. | `CleanupCandidate`, `cleanupCandidateId` | Candidate does not mean safe to delete. |
| Cleanup Action | A user-approved operation that modifies files or folders. | `CleanupAction`, `cleanupActionId` | Examples include moving to Recycle Bin, quarantine, or deletion. |
| Protected Location | A path or category treated as high-risk or blocked from cleanup. | `ProtectedLocation`, `protectedLocationPath` | Includes profile essentials, sensitive app/user data, cloud sync data, credential data, game data, and mod-manager data until exceptions are defined. |
| Storage Savings | Estimated or confirmed recoverable disk space. | `StorageSavings`, `estimatedStorageSavings`, `confirmedStorageSavings` | Use explicit units. |
| Dry Run | A preview of what would happen without modifying files. | `DryRun`, `dryRunResult` | Prefer before Cleanup Actions. |
| Bloat Category | A reason a candidate may be unwanted, removable, or worth conservative review. | `BloatCategory`, `bloatCategory` | Includes cleanup scope roots, profile containers, AppData areas, browser data, cloud sync data, credential data, old downloads, temp folders, caches, GPU shader caches, duplicate files, large old files, old game files, game data including mod-manager data, package caches, Windows app data, Windows app leftovers, and installed applications. |
| Cloud Sync Data | User-owned data managed by a cloud sync provider or sync client. | `BloatCategory.CloudSyncData` | Examples include OneDrive, Dropbox, Google Drive, iCloud Drive, Nextcloud, Syncthing, and MEGA. Protected by default. |
| Credential Data | Authentication, password manager, key, token, or sensitive access configuration data. | `BloatCategory.CredentialData` | Examples include SSH keys, `.gnupg`, `.aws`, `.azure`, `.kube`, 1Password, Bitwarden, KeePass, and `.kdbx` vaults. Protected by default. |
| Specific Rebuildable Cache Evidence | Strong cache-path evidence that the selected row is a rebuildable cache rather than a broad app or profile container. | `HasSpecificRebuildableCacheEvidence` | Used for rows such as `DXCache` or package-cache rows under an app cache path. It can support `Likely safe` / `Quarantine candidate`, but it is not cleanup approval and broad parents remain inspection-first. |
| Importance Rating | Conservative estimate of how important a file or folder may be. | `ImportanceRating`, `importanceRating` | User-facing values: `Likely safe`, `Caution`, `High risk`. |
| Deletion Recommendation | Suggested action for a candidate after inspection. | `DeletionRecommendation`, `deletionRecommendation` | Examples: keep, inspect, quarantine candidate, delete later. |
| Storage Review Filter | Read-only filter applied to Storage Scan results. | `StorageReviewFilter`, `storageReviewFilter` | Initial filters: All, Likely safe, Caution, High risk, Quarantine candidates, Access issues. Filter button tooltips and automation help text should keep review-only, no-rescan, no-file-modified, no-permission-change for Access issues, and not-cleanup-approval boundaries available. |
| Storage Review Search | Read-only text search over completed Storage Scan review rows. | `StorageReviewSearch`, `StorageReviewSearchField` | Broad search matches path, parent path, name, category, rating, recommendation, evidence, Access Status, and access issue text. Field prefixes such as `path:`, `parent:`, `under:`, `category:`, `rating:`, and `access:` restrict matching to one field. Search input tooltip and automation help text should expose prefix examples and read-only/no-rescan/no-cleanup-approval boundaries. Clear search tooltip and automation help text should keep no-rescan, no-file-modified, and Review Shortlist-preserving boundaries available. |
| Access Status | Read-only label showing whether a Storage Scan row was readable during scan. | `AccessStatus` | User-facing values: `Readable`, `Access issue`. Searchable with `access:` and `issue:` prefixes. Does not imply permission changes or retry. |
| Storage Entry Type Filter | Read-only filter applied to completed Storage Scan rows by file/folder type. | `StorageEntryTypeFilter` | Values: All types, Files, Folders. Combines with review, category, and search filters. Tooltip and automation help text should keep no-rescan, no-file-modified, and not-cleanup-approval boundaries available. |
| Storage Size Threshold Filter | Read-only filter applied to completed Storage Scan rows by minimum row size. | `StorageSizeThresholdFilter` | Values include All sizes, 1 MB+, 100 MB+, 1 GB+, 5 GB+, and 10 GB+. Size is a triage lens, not cleanup approval or storage-savings proof; tooltip and automation help text should say so. |
| Review View Reset | Read-only action that clears the active review lens. | `ResetReviewView` | Restores All, All categories, All types, All sizes, and empty search. Keeps Review Shortlist. Tooltip and automation help text should keep no-rescan and no-file-modified boundaries available. |
| Storage Review Display Limit | Maximum number of matched Storage Scan review rows shown in the WPF results grid at one time. | `MaxDisplayedRows` | Current limit is 2,000 rows. The scan can contain more matched rows than the current display window. |
| Storage Review Display Window | Current slice of matched Storage Scan review rows shown in the WPF results grid. | `_currentDisplayStartIndex` | Previous rows and Next rows move through matched in-memory rows without rescanning. Resets when the active review lens changes. Tooltip and automation help text should keep in-memory/no-rescan/no-file-modified boundaries available. |
| Bloat Category Filter | Read-only category lens applied to Storage Scan results. | `StorageCategoryFilter`, `StorageCategorySummaryEntry` | Combines with the active Storage Review Filter. Includes All categories, named categories, and No category. Tooltip and automation help text should keep read-only/no-rescan/no-file-modified/not-cleanup-approval boundaries available. |
| Uncategorized Result | A Storage Scan row with no assigned Bloat Category. | Empty `BloatCategories`, `StorageCategoryFilter.NoCategory` | Shows as `None` in the Categories column. Does not imply safe or unsafe. |
| Review Mix | Summary of result counts and largest rows by rating/recommendation, plus access issue count. | `StorageReviewSummary` | Do not sum flattened recursive row sizes because parent and child rows overlap. |
| Matched Review Mix | Read-only summary of the currently matched Storage Scan review rows after active filters/search/focus are applied. | `MatchedReviewMixText` | Counts the full matched set, not only the visible 2,000-row display window. Review context only, not cleanup approval or storage savings. |
| Storage Review Size Note | Visible reminder that recursive Storage Scan rows can overlap. | `ReviewSizeNoteText` | Row sizes are triage clues, not confirmed storage savings. |
| Storage Scan Safety Summary | Read-only health readout for safety-relevant scan signals. | `StorageScanSafetySummary`, `StorageScanSafetySummaryBuilder`, `StorageScanSafetyShortcut`, `StorageScanSafetyShortcutFilterBuilder` | Includes bounded access issue, Quarantine candidate, and Uncategorized Result examples when present. Not a safety guarantee, cleanup approval, or Cleanup Action. Collapsed header tooltip/help text should mirror compact risk counts; review shortcut tooltips and automation help text should keep read-only/no-rescan/no-file-modified, no-permission-change, no-link-following, and not-cleanup-approval boundaries available. |
| Child Breakdown | Read-only summary of the largest immediate children inside a selected folder. | `StorageChildSummary`, `StorageChildSummaryBuilder` | Helps inspect large containers without marking the container safe. |
| Storage Hotspot Trail | Read-only selected-folder summary that follows the largest child at each level. | `StorageHotspotTrailEntry`, `StorageHotspotTrailBuilder` | Shows where size appears to concentrate inside a folder. Parent/child sizes overlap and are not storage savings or cleanup approval. |
| Selected Folder Subtree Summary | Read-only selected-folder summary of descendant review rows by rating, recommendation, access status, and risk flags. | `StorageSubtreeReviewSummary`, `StorageSubtreeReviewSummaryBuilder` | Excludes the selected folder itself from counts. Descendant rows are review context, not storage savings or cleanup approval. |
| Selected Folder Child Focus | Read-only action that narrows Storage Scan review rows to the immediate children of the selected folder. | `ShowSelectedFolderChildren`, `ShowChildrenButton`, `StorageReviewSearchField.Parent` | Applies `parent:<selected path>` search and resets other review lenses to All. Tooltip and automation help text should keep no-rescan, no-file-modified, and no-cleanup-approval boundaries available; status text should say `Reset view` returns to all rows. |
| Selected Folder Descendant Focus | Read-only action that narrows Storage Scan review rows to every descendant under the selected folder. | `ShowSelectedFolderDescendants`, `ShowDescendantsButton`, `StorageReviewSearchField.Under` | Applies `under:<selected path>` search, excludes the selected folder itself, and resets other review lenses to All. Tooltip and automation help text should keep no-rescan, no-file-modified, and no-cleanup-approval boundaries available. |
| Selected Path Inspection | Read-only action for inspecting a selected scan result. | `PathInspectionPlan`, `PathInspectionPlanBuilder` | Initial actions: copy path, open in File Explorer. Not a Cleanup Action. Tooltip and automation help text should keep manual-inspection and no-app-file-modification boundaries available. Clipboard failures during Copy path should show a warning/status update instead of crashing the app. |
| Selected Path Hierarchy Context | Read-only relative-path, parent-path, and depth context for Storage Scan review rows. | `StorageEntryRow.RelativePath`, `StorageEntryRow.ParentLocation`, `Depth` | Helps short or hashed row names make sense. Not cleanup approval. |
| Selected Row Contents Context | Read-only contained file and folder counts for Storage Scan review rows. | `StorageEntryRow.Contents`, `ContainedFileCount`, `ContainedFolderCount`, `ContainedTotalCount` | Shown in the grid, selected-row detail, and CSV export. Folder count means descendant folders, excluding the selected folder itself. The grid `Contents` column sorts by total contained items. Counts are triage context, not storage savings. |
| Selected File Content Preview | Explicit read-only preview of a bounded text sample from a selected file. | `SelectedFileContentPreview`, `SelectedFileContentPreviewBuilder` | Reads at most a small text sample after user action. Does not preview folders, Credential Data, or binary content as text. Tooltip and automation help text should keep bounded read-only behavior available. |
| Selected Path Review Guidance | Read-only next-step wording for the selected Storage Scan row. | `SelectedPathReviewGuidance`, `SelectedPathReviewGuidanceBuilder` | Explains whether to inspect children, keep by default, shortlist after review, or classify before cleanup. Not cleanup approval. |
| Review Shortlist | Temporary in-memory set of Storage Scan rows marked for follow-up review. | `StorageReviewShortlist` | Not a Cleanup Action, Quarantine approval, or persisted history. Selected-row and visible-row additions/removals must keep row/window scope, no-file-modified behavior, and not-cleanup-approval boundaries available. Export and clear controls should keep report-only and in-memory-only boundaries available through tooltips and automation help text. Quarantine shortlist collapsed-header tooltip/help text should mirror shortlist, preview, current quarantined, and undo state; lightweight header styling may distinguish waiting, ready, informational, and warning states without implying cleanup approval. |
| Review Shortlist Safety Mix | Read-only summary of shortlisted rows by rating, recommendation, and risk flags. | `ShortlistSafetyMixText` | Review context only. It is not cleanup approval, Quarantine readiness, or storage-savings proof. |
| Quarantine Preview | Read-only dry run showing eligible, blocked, and redundant shortlisted rows before any quarantine action. | `QuarantinePreview`, `QuarantinePreviewEntry`, `QuarantinePreviewBuilder`, `QuarantinePreviewCsvExporter`, `QuarantinePreviewStatusText` | Blocks broad parent rows with protected/high-risk descendants and shows blocked descendant examples as cleanup-scope-relative paths. Does not create folders, write manifests, move files, delete files, or approve cleanup; WPF inline status, preview/gate panes, control tooltips, and automation help text should keep dry-run/report-only boundaries available. Inline status styling may distinguish neutral, success, warning, and error states, but must not imply cleanup approval; its tooltip/help text should mirror dynamic status and no-create/no-move/no-restore/no-delete/no-approval boundaries. The Review Shortlist is the source, and all included rows are the execution target after exact confirmation. |
| Quarantine Root Selection | Read-only preview setting for the destination root used when building Quarantine Preview paths. | `QuarantineRootBox`, `BrowseQuarantineRootButton`, `CurrentQuarantineRootPath` | Defaults to `D:\WindowsFileCleanerQuarantine`. Can be typed or browsed. Must be fully qualified before preview. Changing it clears stale previews. Does not create folders or execute Quarantine. Typed-root and browse tooltips plus automation help text should keep preview-only and no-folder-creation boundaries available. |
| Quarantine Root Safety Note | Read-only UI text explaining whether the Quarantine Root Selection is usable for preview and whether it is on preferred `D:` storage. | `QuarantineRootSafetyNote`, `QuarantineRootSafetyNoteBuilder` | Does not validate by creating folders or touching the filesystem. |
| Scan Report Export | Read-only report generated from Storage Scan results. | `StorageScanCsvExporter` | Initial format is CSV for the active Storage Review Filter, Bloat Category Filter, Storage Entry Type Filter, Storage Size Threshold Filter, and Storage Review Search. Exports include cleanup-scope-relative path, parent/depth hierarchy context, access status, and suggested filenames describe active filters/search. Tooltip and automation help text should keep report-only and no-scanned-file-modified boundaries available. |
| Quarantine | A reversible holding location for selected files or folders before deletion. | `Quarantine`, `quarantinePath` | Preferred on `D:`. Current preview default is `D:\WindowsFileCleanerQuarantine`. |
| Undo Quarantine | Restore a quarantined file or folder to its original path. | `UndoQuarantine`, `undoQuarantine` | Requires a Restore Manifest. Core restore behavior is fixture-tested and wired to WPF for the current fixture execution only. |
| Restore Manifest | Versioned JSON metadata needed to recover, inspect, or undo a quarantine action after explicit execution begins. | `RestoreManifest`, `RestoreManifestEntry`, `RestoreManifestBuilder`, `RestoreManifestJsonSerializer`, `restoreManifestPath` | Schema version `restore-manifest.v1`; write-ahead model writes a planned manifest before the first move and updates it before/after each move and restore attempt. |
| Quarantine Manifest Discovery | Read-only discovery of action-scoped Restore Manifests under the selected Quarantine Root. | `QuarantineManifestDiscovery`, `QuarantineManifestDiscoveryBuilder`, `QuarantineManifestDiscoveryIssue`, `DiscoverQuarantineManifestsButton` | Searches only `<quarantine-root>\actions\*\restore-manifest.json`. Does not restore, move, delete, clean up folders, or create cleanup history; WPF output and discovery control tooltip/help text should state that no all-manifest restore action is available from discovery and that discovery is read-only. |
| Restore Manifest Summary | Compact read-only status for one discovered Restore Manifest. | `RestoreManifestSummary` | Derived from manifest metadata for discovery display. Not approval to restore and not cleanup history. |
| Selected Restore Manifest Review | Read-only review of one discovered Restore Manifest and its restore readiness. | `SelectedRestoreManifestReview`, `SelectedRestoreManifestReviewBuilder`, `RestoreManifestSelectionBox`, `PreviewSelectedRestoreManifestReadinessButton` | Selection is not approval to restore. Does not call Undo Quarantine execution, write manifests, create folders, move files, delete files, or clean up folders. WPF selection and readiness tooltips plus automation help text should keep selected-only/not-approval scope visible, including while controls are disabled. |
| Selected Restore Confirmation Draft | Read-only confirmation readiness check for one selected Restore Manifest. | `SelectedRestoreConfirmationDraft`, `SelectedRestoreConfirmationDraftBuilder`, `PreviewSelectedRestoreGateButton` | Required confirmation text is `RESTORE`. Not approval, not execution, and not a persisted cleanup job. |
| Selected Restore Execution Gate | Gate combining selected restore confirmation, typed text, and execution availability. | `SelectedRestoreExecutionGate`, `SelectedRestoreExecutionGateBuilder`, `SelectedRestoreConfirmationBox` | Can open only for selected fixture Restore Manifests in the current build. WPF panes, disabled-control tooltips, and automation help text should show fixture-only versus preview-only scope status and the approval boundary. Real-profile and custom non-fixture selected restore stay blocked. |
| Fixture-only Selected Restore Execution | Visible WPF execution path that restores a selected discovered fixture Restore Manifest after selected manifest readiness and exact `RESTORE` confirmation. | `ExecuteSelectedRestoreForCurrentSelection`, `ExecuteSelectedRestoreButton`, `CanExecuteSelectedRestore` | Uses `UndoQuarantineExecutor`; real-profile and custom non-fixture selected restore stay blocked. |
| Restore Readiness Preview | Read-only preview of whether discovered Restore Manifests appear ready for future Undo Quarantine. | `RestoreReadinessPreview`, `RestoreReadinessPreviewBuilder`, `PreviewRestoreReadinessButton` | Checks path blockers and entry states across discovered manifests. Does not restore, write manifests, create folders, move files, delete files, or approve cleanup; WPF wording, tooltip, and automation help text should name the all-manifest readiness scope and route fixture selected restore through selected manifest readiness and the selected restore gate. |
| Restore Readiness Entry | Read-only preview row for one Restore Manifest entry. | `RestoreReadinessEntryPreview`, `RestoreReadinessDisposition` | Values include Restorable, Blocked, Already restored, Needs recovery review, and Not moved. Not an Undo Quarantine result. |
| Restore Manifest Action Status | Overall action or undo state recorded in a Restore Manifest. | `RestoreManifestActionStatus` | Values: Planned, Moving, Completed, Partial failure, Failed, Restoring, Restored, Restore partial failure, Restore failed. Partial/failure/restore-in-progress states require recovery review. |
| Restore Manifest Entry Status | Per-entry move or restore state recorded in a Restore Manifest. | `RestoreManifestEntryStatus` | Values: Planned, Moving, Moved, Failed, Restoring, Restored, Restore failed. Undo Quarantine restores Moved entries and reviews non-restorable states. |
| Restore Manifest File Store | Narrow component allowed to write action-scoped Restore Manifest JSON files. | `RestoreManifestFileStore`, `RestoreManifestFileWriteResult` | Uses temp-file replacement under the action root. Used by fixture-tested core execution, fixture-only WPF execution, and undo. |
| Quarantine Executor | Narrow core component that moves planned Restore Manifest entries into action-scoped quarantine paths. | `QuarantineExecutor` | Fixture-tested and wired to WPF for fixture scopes only. Not Undo Quarantine, not real-profile WPF execution, and not permanent deletion. |
| Quarantine Execution Result | Summary returned by Quarantine Executor after a fixture-proven execution attempt. | `QuarantineExecutionResult` | Includes moved count, failed count, blockers, and recovery-review need. |
| Quarantine Execution Entry Result | Per-entry outcome returned by Quarantine Executor. | `QuarantineExecutionEntryResult` | Records original path, quarantine path, entry status, whether it moved, and error evidence. |
| Fixture-only WPF Quarantine Execution | Visible-app execution path that moves synthetic fixture files after preview readiness and exact confirmation. | `ExecuteQuarantineForCurrentPreview`, `QuarantineExecutionGate.CanExecute`, `ExecuteQuarantineButton` | Available only for recognized fixture Cleanup Scopes. The visible action should read as shortlist-level execution, such as `Quarantine included shortlist`, because one exact confirmation applies to all included Review Shortlist rows. Real-profile and custom non-fixture execution stay blocked; tooltip and automation help text should keep that boundary available. |
| WPF Current Fixture Undo Quarantine | Visible-app undo path that restores the current fixture Quarantine execution. | `UndoQuarantineForCurrentExecution`, `UndoQuarantineButton`, `CanUndoQuarantine` | Available only after current fixture execution, and should remain available after a post-execution rescan until undo is attempted. Does not discover Restore Manifests or run for real-profile scopes; tooltip and automation help text should keep that boundary available. |
| Current-Session Quarantined Review | Read-only WPF grid view of current in-memory Restore Manifest entries still in Moved state. | `QuarantinedItemRow`, `ShowQuarantinedButton`, `BackToScanRowsButton` | Current-session and fixture execution state only; not Quarantine Manifest Discovery, cleanup history, restore approval, or real-profile Undo Quarantine. The visible grid-switch label should expose current-session scope, such as `Current quarantined`; button tooltips and automation help text should explain disabled states, read-only behavior, older-manifest discovery boundaries, and that returning to scan rows does not rescan or undo. |
| Review Grid Mode Status | Visible WPF text that names what the main grid is currently showing. | `ReviewGridModeText` | Distinguishes Storage Scan rows from Current-Session Quarantined Review rows, keeps quarantined view read-only, points to `Back to scan rows`, and warns when Storage Scan rows may be stale after fixture Quarantine execution. Lightweight styling may distinguish neutral, informational, and warning states, but must not imply cleanup approval. Tooltip/help text should mirror the dynamic status and say it does not rescan, modify files, restore files, or approve cleanup. |
| Undo Quarantine Executor | Narrow core component that restores Moved Restore Manifest entries from quarantine paths to original paths. | `UndoQuarantineExecutor` | Fixture-tested and used by current-fixture WPF undo. Not real-profile WPF Undo, not rollback inside Quarantine Executor, and not quarantine-folder cleanup. |
| Undo Quarantine Result | Summary returned by Undo Quarantine Executor after a fixture-proven restore attempt. | `UndoQuarantineResult` | Includes restored count, failed count, blockers, and recovery-review need. |
| Undo Quarantine Entry Result | Per-entry outcome returned by Undo Quarantine Executor. | `UndoQuarantineEntryResult` | Records original path, quarantine path, entry status, whether it restored, and error evidence. |
| Restore Manifest Draft | In-memory draft of future Restore Manifest metadata generated from included Quarantine Preview rows. | `RestoreManifestDraft`, `RestoreManifestEntryDraft`, `RestoreManifestDraftBuilder`, `RestoreManifestDraftJsonSerializer` | Not an executed manifest, not proof that files moved, and must not be written by preview code. |
| Quarantine Confirmation Draft | In-memory readiness check comparing a Quarantine Preview with a Restore Manifest Draft before execution. | `QuarantineConfirmationDraft`, `QuarantineConfirmationDraftBuilder` | Not approval, not execution, and not a persisted cleanup job. Exposes blockers, counts, bytes, confirmation phrase, and execution availability for the current Cleanup Scope. |
| Quarantine Execution Gate | Decision that combines confirmation-readiness blockers, exact typed confirmation text, and implementation availability before WPF Quarantine execution. | `QuarantineExecutionGate`, `QuarantineExecutionGateBuilder` | Can open only for fixture scopes in the current build. Real-profile and custom non-fixture execution stay blocked. |
| Quarantine Execution Scope Status | Read-only WPF wording that explains whether the current Cleanup Scope is fixture-executable or preview-only. | `FormatQuarantineExecutionScopeStatus` | Fixture-only execution status is not cleanup approval. Preview-only status means real-profile and custom execution remain unavailable. Disabled confirmation/execution tooltips and automation help text should repeat the same boundary. |
| Quarantine Action Draft | In-memory read-only action-scoped layout for future Quarantine execution item paths and restore manifest path. | `QuarantineActionDraft`, `QuarantineActionEntryDraft`, `QuarantineActionDraftBuilder` | Uses `<quarantine-root>\actions\<action-id>\items\...` and `<quarantine-root>\actions\<action-id>\restore-manifest.json`. Does not create folders or write manifests. |
| Desktop App | The preferred app shape for this project. | `DesktopApp` only if needed | Implement with C# and WPF. |
| WPF | Windows Presentation Foundation, the selected desktop UI framework. | Avoid domain-model names unless framework-specific. | Use for Windows-only desktop UI. |

## Forbidden synonyms

| Do not use | Use instead | Reason |
|---|---|---|
| Thing | The specific domain term | Too vague. |
| Item | The specific domain term | Too vague; often causes naming drift. |
| Record | The specific domain term | Too database-oriented unless discussing persistence. |
| Data | The specific domain term | Too generic. |
| Object | The specific domain term | Too generic. |
| Manager | A more specific service/component name | Usually hides responsibility. |
| Helper | A more specific function/module name | Usually hides responsibility. |
| Util | A more specific function/module name | Usually hides responsibility. |
| Junk | Cleanup Candidate | Too judgmental before user review. |
| Trash | Cleanup Candidate or Cleanup Action | Confuses candidate status with removal action. |
| Delete Candidate | Cleanup Candidate | Implies the action is already chosen. |
| Safe File | Low-risk Cleanup Candidate | Safety depends on user context and category rules. |
| Bloat | Cleanup Candidate, unwanted bloat | User-facing shorthand only; define concrete categories before code relies on it. |
| Score | Importance Rating or Deletion Recommendation | Too vague unless the specific rating is named. |
| Restore Log | Restore Manifest | Use manifest when it is required for undo behavior. |
| Manifest Preview | Restore Manifest Draft | Use draft when no cleanup action has executed. |
| Confirmation Plan | Quarantine Confirmation Draft | Use draft while no cleanup action has executed and no approval has been given. |
| Cleanup Plan | Quarantine Preview | Use the more precise preview term until actual Cleanup Actions exist. |

## Term template

Use this template when adding new terms:

```md
### Term Name

Status: draft | stable | deprecated  
Last reviewed: YYYY-MM-DD

#### Definition

...

#### Use this term when

...

#### Do not use this term when

...

#### Preferred code names

- Entity/type:
- ID:
- Collection:
- Route:
- Component:
- Test name:

#### Forbidden synonyms

- ...
```

## Deprecated terms

Use this section when renaming language.

| Deprecated term | Replacement | Deprecated on | Notes |
|---|---|---|---|
| None yet |  |  |  |

## Open naming questions

- Should user-facing copy say "cleanup candidate", "review candidate", or something friendlier?
- Should the default preview root `D:\WindowsFileCleanerQuarantine` remain the default for actual Quarantine execution?
