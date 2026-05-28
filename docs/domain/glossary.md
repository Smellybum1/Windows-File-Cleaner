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
| Cleanup Scope Selection | Pre-scan action of typing or browsing for the Cleanup Scope path. | `BrowseScopeButton`, `ScopePathBox` | Does not start a scan, bypass preflight acknowledgement, or approve cleanup. |
| Cleanup Scope Safety Note | Read-only UI text explaining whether the current Cleanup Scope looks like a fixture, real profile, custom path, blank path, or invalid path. | `CleanupScopeSafetyNote`, `CleanupScopeSafetyNoteBuilder` | Reminds the user to run preflight and fixture review before real-profile scans. Not scan approval. |
| Cleanup Scope Scan Gate | Read-only scan-start gate derived from the Cleanup Scope and real-profile acknowledgement. | `CleanupScopeScanGate`, `CleanupScopeScanGateBuilder` | Real-profile scans require explicit acknowledgement that MVP preflight and fixture review were run. Does not run preflight or create fixtures. |
| Storage Scan | The first read-only workflow for recursively scanning the Cleanup Scope. | `StorageScan`, `storageScanId` | Must not modify files. |
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
| Storage Review Filter | Read-only filter applied to Storage Scan results. | `StorageReviewFilter`, `storageReviewFilter` | Initial filters: All, Likely safe, Caution, High risk, Quarantine candidates, Access issues. |
| Storage Review Search | Read-only text search over completed Storage Scan review rows. | `StorageReviewSearch`, `StorageReviewSearchField` | Broad search matches path, name, category, rating, recommendation, evidence, Access Status, and access issue text. Field prefixes such as `path:`, `category:`, `rating:`, and `access:` restrict matching to one field. |
| Access Status | Read-only label showing whether a Storage Scan row was readable during scan. | `AccessStatus` | User-facing values: `Readable`, `Access issue`. Searchable with `access:` and `issue:` prefixes. Does not imply permission changes or retry. |
| Storage Entry Type Filter | Read-only filter applied to completed Storage Scan rows by file/folder type. | `StorageEntryTypeFilter` | Values: All types, Files, Folders. Combines with review, category, and search filters. |
| Storage Size Threshold Filter | Read-only filter applied to completed Storage Scan rows by minimum row size. | `StorageSizeThresholdFilter` | Values include All sizes, 1 MB+, 100 MB+, 1 GB+, 5 GB+, and 10 GB+. Size is a triage lens, not cleanup approval or storage-savings proof. |
| Review View Reset | Read-only action that clears the active review lens. | `ResetReviewView` | Restores All, All categories, All types, All sizes, and empty search. Keeps Review Shortlist. |
| Storage Review Display Limit | Maximum number of matched Storage Scan review rows shown in the WPF results grid at one time. | `MaxDisplayedRows` | Current limit is 2,000 rows. The scan can contain more matched rows than the current display window. |
| Storage Review Display Window | Current slice of matched Storage Scan review rows shown in the WPF results grid. | `_currentDisplayStartIndex` | Previous rows and Next rows move through matched in-memory rows without rescanning. Resets when the active review lens changes. |
| Bloat Category Filter | Read-only category lens applied to Storage Scan results. | `StorageCategoryFilter`, `StorageCategorySummaryEntry` | Combines with the active Storage Review Filter. Includes All categories, named categories, and No category. |
| Uncategorized Result | A Storage Scan row with no assigned Bloat Category. | Empty `BloatCategories`, `StorageCategoryFilter.NoCategory` | Shows as `None` in the Categories column. Does not imply safe or unsafe. |
| Review Mix | Summary of result counts and largest rows by rating/recommendation, plus access issue count. | `StorageReviewSummary` | Do not sum flattened recursive row sizes because parent and child rows overlap. |
| Storage Review Size Note | Visible reminder that recursive Storage Scan rows can overlap. | `ReviewSizeNoteText` | Row sizes are triage clues, not confirmed storage savings. |
| Storage Scan Safety Summary | Read-only health readout for safety-relevant scan signals. | `StorageScanSafetySummary`, `StorageScanSafetySummaryBuilder`, `StorageScanSafetyShortcut`, `StorageScanSafetyShortcutFilterBuilder` | Includes bounded access issue, Quarantine candidate, and Uncategorized Result examples when present. Not a safety guarantee, cleanup approval, or Cleanup Action. |
| Child Breakdown | Read-only summary of the largest immediate children inside a selected folder. | `StorageChildSummary`, `StorageChildSummaryBuilder` | Helps inspect large containers without marking the container safe. |
| Selected Path Inspection | Read-only action for inspecting a selected scan result. | `PathInspectionPlan`, `PathInspectionPlanBuilder` | Initial actions: copy path, open in File Explorer. Not a Cleanup Action. |
| Selected Path Hierarchy Context | Read-only relative-path, parent-path, and depth context for Storage Scan review rows. | `StorageEntryRow.RelativePath`, `StorageEntryRow.ParentLocation`, `Depth` | Helps short or hashed row names make sense. Not cleanup approval. |
| Selected Row Contents Context | Read-only contained file and folder counts for Storage Scan review rows. | `StorageEntryRow.Contents`, `ContainedFileCount`, `ContainedFolderCount`, `ContainedTotalCount` | Shown in the grid, selected-row detail, and CSV export. Folder count means descendant folders, excluding the selected folder itself. The grid `Contents` column sorts by total contained items. Counts are triage context, not storage savings. |
| Selected File Content Preview | Explicit read-only preview of a bounded text sample from a selected file. | `SelectedFileContentPreview`, `SelectedFileContentPreviewBuilder` | Reads at most a small text sample after user action. Does not preview folders, Credential Data, or binary content as text. |
| Selected Path Review Guidance | Read-only next-step wording for the selected Storage Scan row. | `SelectedPathReviewGuidance`, `SelectedPathReviewGuidanceBuilder` | Explains whether to inspect children, keep by default, shortlist after review, or classify before cleanup. Not cleanup approval. |
| Review Shortlist | Temporary in-memory set of Storage Scan rows marked for follow-up review. | `StorageReviewShortlist` | Not a Cleanup Action, Quarantine approval, or persisted history. Bulk additions/removals use only currently displayed rows. |
| Quarantine Preview | Read-only dry run showing eligible, blocked, and redundant shortlisted rows before any quarantine action. | `QuarantinePreview`, `QuarantinePreviewEntry`, `QuarantinePreviewBuilder`, `QuarantinePreviewCsvExporter` | Blocks broad parent rows with protected/high-risk descendants and shows blocked descendant examples as cleanup-scope-relative paths. Does not create folders, write manifests, move files, delete files, or approve cleanup. |
| Quarantine Root Selection | Read-only preview setting for the destination root used when building Quarantine Preview paths. | `QuarantineRootBox`, `BrowseQuarantineRootButton`, `CurrentQuarantineRootPath` | Defaults to `D:\WindowsFileCleanerQuarantine`. Can be typed or browsed. Must be fully qualified before preview. Changing it clears stale previews. Does not create folders or execute Quarantine. |
| Quarantine Root Safety Note | Read-only UI text explaining whether the Quarantine Root Selection is usable for preview and whether it is on preferred `D:` storage. | `QuarantineRootSafetyNote`, `QuarantineRootSafetyNoteBuilder` | Does not validate by creating folders or touching the filesystem. |
| Scan Report Export | Read-only report generated from Storage Scan results. | `StorageScanCsvExporter` | Initial format is CSV for the active Storage Review Filter, Bloat Category Filter, Storage Entry Type Filter, Storage Size Threshold Filter, and Storage Review Search. Exports include cleanup-scope-relative path, parent/depth hierarchy context, access status, and suggested filenames describe active filters/search. |
| Quarantine | A reversible holding location for selected files or folders before deletion. | `Quarantine`, `quarantinePath` | Preferred on `D:`. Current preview default is `D:\WindowsFileCleanerQuarantine`. |
| Undo Quarantine | Restore a quarantined file or folder to its original path. | `UndoQuarantine`, `undoQuarantine` | Requires a restore manifest. |
| Restore Manifest | Versioned JSON metadata needed to undo an executed quarantine action. | `RestoreManifest`, `restoreManifestPath` | Schema version `restore-manifest.v1`; should include original path, quarantine path, size, timestamps, and action time. |
| Restore Manifest Draft | In-memory draft of future Restore Manifest metadata generated from included Quarantine Preview rows. | `RestoreManifestDraft`, `RestoreManifestEntryDraft`, `RestoreManifestDraftBuilder`, `RestoreManifestDraftJsonSerializer` | Not an executed manifest, not proof that files moved, and must not be written by preview code. |
| Quarantine Confirmation Draft | In-memory readiness check comparing a Quarantine Preview with a Restore Manifest Draft before future execution. | `QuarantineConfirmationDraft`, `QuarantineConfirmationDraftBuilder` | Not approval, not execution, and not a persisted cleanup job. Exposes blockers, counts, bytes, and the future confirmation phrase. |
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
