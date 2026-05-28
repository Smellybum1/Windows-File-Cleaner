# Domain Context

---
last_reviewed: 2026-05-28
owner: project-owner
stability: evolving
---

## Purpose

This file defines the shared domain language for this project.

It should describe the product concepts that matter to users, developers, and Codex. Use this file to prevent repeated explanations, naming drift, and inconsistent code generation.

## Product summary

This project is a local Windows cleanup app for the project owner, who needs to reduce storage used by `C:\Users` on a Windows 11 system drive.

The immediate situation is:

- `C:` is a 250 GB Windows 11 operating-system partition.
- `C:\Users` is using about 75 GB.
- The goal is to trim that usage by identifying and removing unwanted bloat without harming important user files, application state, credentials, source code, game saves, current apps such as Codex, or Windows profile behavior.

The preferred product shape is a Windows-only desktop app built with C# and WPF. The first production workflow is named Storage Scan and should be a read-only scan and review experience, not cleanup execution.

## Core concepts

### System Drive

Status: draft
Last reviewed: 2026-05-28

#### Definition

The System Drive is the Windows operating-system partition. For this project, the known system drive is `C:`.

#### Examples

- `C:` on the project owner's Windows 11 machine.

#### Non-examples

- External drives.
- Non-Windows partitions.
- Cloud storage providers.

#### Lifecycle

- Exists before this app runs.
- Must not be modified by the app except through user-approved cleanup actions within the allowed cleanup scope.

#### Relationships

- Contains the User Profile Root.
- Contains protected operating-system locations outside the initial cleanup scope.

#### Code implications

- Use `SystemDrive` for the domain concept if represented in code.
- Use `systemDrivePath` for the filesystem path.
- Do not hard-code broad cleanup behavior across all of `C:`.

### User Profile Root

Status: draft  
Last reviewed: 2026-05-28

#### Definition

The User Profile Root is the top-level Windows folder containing user profiles. For this project, the known profile root is `C:\Users`, but the initial Cleanup Scope is limited to `C:\Users\moxhe`.

#### Examples

- `C:\Users`
- `C:\Users\moxhe`

#### Non-examples

- `C:\Windows`
- `C:\Program Files`
- Other drives or arbitrary folders outside the selected cleanup scope.

#### Lifecycle

- Exists before this app runs.
- Is scanned when the user requests storage analysis.
- Is never deleted as a whole by the app.

#### Relationships

- Belongs to the System Drive.
- Contains user profile folders.
- Contains high-risk subfolders such as `Documents`, `Desktop`, browser profiles, and `AppData`.

#### Code implications

- Use `UserProfileRoot` for the domain concept if represented in code.
- Use `userProfileRootPath` for the path.
- Do not call this simply `users`, `root`, or `folder` when the domain meaning matters.

### Cleanup Scope

Status: draft  
Last reviewed: 2026-05-28

#### Definition

Cleanup Scope is the set of paths the app is allowed to scan or propose cleanup for in a given run.

The initial Cleanup Scope is `C:\Users\moxhe`.

#### Examples

- Include: `C:\Users\moxhe`
- Possible future include: another selected user profile folder under `C:\Users`
- Possible future exclude: specific high-risk folders chosen by the user

#### Non-examples

- The entire `C:` drive by default.
- Windows system folders.
- Any path the user has not approved for scanning.

#### Lifecycle

- Chosen before a scan.
- Used to constrain scan and recommendation behavior.
- May be narrowed before cleanup execution.

#### Relationships

- Contains folders and files that may become Cleanup Candidates.
- Must respect Protected Locations.

#### Code implications

- Use `CleanupScope` for the domain concept.
- Use `cleanupScopePath` or `cleanupScopePaths` for paths.
- Keep scan and cleanup logic constrained to the active Cleanup Scope.

### Cleanup Scope Root

Status: draft
Last reviewed: 2026-05-28

#### Definition

The Cleanup Scope Root is the top-level path selected for a Storage Scan run.

For the initial real scan, the Cleanup Scope Root is `C:\Users\moxhe`.

#### Examples

- `C:\Users\moxhe` when scanning the real profile.
- `D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture` when running the fixture launcher.

#### Non-examples

- Child folders under the selected scope, such as `AppData`, `Downloads`, or `DXCache`.
- A Cleanup Candidate approved for quarantine.
- A storage savings estimate.

#### Lifecycle

- Chosen before a Storage Scan starts.
- Appears as the root Storage Scan row.
- Is shown as High risk / Keep with Cleanup scope root and Protected location categories.
- Remains read-only and should be reviewed through child rows.

#### Relationships

- Belongs to Cleanup Scope.
- Supports Protected Location and the rule that whole profile/scope containers are not cleanup targets.
- Complements Child Breakdown, Selected Path Hierarchy Context, and Selected Path Review Guidance.

#### Code implications

- Use `BloatCategory.CleanupScopeRoot` for the scan root row.
- The scanner should pass root context explicitly rather than trying to infer the root from path shape alone.
- Keep the Cleanup Scope Root as `High risk` / `Keep`.
- Do not shortlist, quarantine, or delete the Cleanup Scope Root as a whole.

### Cleanup Scope Selection

Status: draft
Last reviewed: 2026-05-28

#### Definition

Cleanup Scope Selection is the pre-scan action of choosing the Cleanup Scope path in the WPF app.

The path may be typed or chosen through the native folder browser. Selecting a folder updates the Cleanup Scope text and safety note, but does not start scanning.

#### Examples

- Type `C:\Users\moxhe`.
- Use `Browse...` to choose the synthetic fixture Cleanup Scope.
- Use `Browse...` to choose a custom folder before manually clicking `Scan`.

#### Non-examples

- A Storage Scan.
- A Cleanup Action.
- Real-profile preflight acknowledgement.
- Approval to scan a real profile.

#### Lifecycle

- Available before a Storage Scan starts.
- Disabled while scanning.
- Re-enabled after a scan completes or is canceled.
- Feeds Cleanup Scope Safety Note and Cleanup Scope Scan Gate.

#### Relationships

- Belongs to Cleanup Scope.
- Supports fixture-first review by making synthetic Cleanup Scope selection less error-prone.
- Does not bypass the real-profile scan gate.

#### Code implications

- Use `BrowseScopeButton` for the WPF folder browser action.
- Keep folder selection separate from `Scan`; never auto-scan after a folder is selected.
- Keep the selected path in `ScopePathBox` so existing safety-note and scan-gate behavior remains authoritative.

### Cleanup Scope Safety Note

Status: draft
Last reviewed: 2026-05-28

#### Definition

A Cleanup Scope Safety Note is read-only UI text that explains what kind of Cleanup Scope is currently entered and what safety step should happen before scanning it.

Initial scope note kinds are Fixture Cleanup Scope, Real Profile Cleanup Scope, Custom Cleanup Scope, Choose Cleanup Scope, and Check Cleanup Scope.

#### Examples

- Fixture Cleanup Scope: remind the user this is the synthetic fixture review path and the app still waits for a manual Scan click.
- Real Profile Cleanup Scope: remind the user to run MVP preflight and fixture review before scanning the real profile.
- Custom Cleanup Scope: remind the user to verify the path before scanning real user files.

#### Non-examples

- A Cleanup Action.
- A scan approval.
- Proof that preflight was run.
- A replacement for the README or MVP preflight script.

#### Lifecycle

- Generated when the WPF app starts.
- Updates when the Cleanup Scope text changes.
- Remains read-only and does not persist state.

#### Relationships

- Supports Cleanup Scope, Storage Scan, fixture-based verification, and the read-only before cleanup rule.
- Feeds Cleanup Scope Scan Gate, which controls whether Scan can start for the current scope.
- Does not modify cleanup eligibility.

#### Code implications

- Use `CleanupScopeSafetyNote` and `CleanupScopeSafetyNoteBuilder`.
- Keep the note informational; do not scan, create fixtures, or run preflight from the note builder.

### Cleanup Scope Scan Gate

Status: draft
Last reviewed: 2026-05-28

#### Definition

Cleanup Scope Scan Gate is the read-only decision that controls whether the WPF `Scan` action can start for the current Cleanup Scope.

For real-profile scopes under `C:\Users\moxhe`, the gate requires explicit acknowledgement that MVP preflight and fixture review were run before enabling `Scan`.

#### Examples

- Fixture Cleanup Scope: `Scan` can start without real-profile acknowledgement.
- Real Profile Cleanup Scope: `Scan` stays disabled until the user ticks the preflight and fixture-review acknowledgement.
- Blank or invalid Cleanup Scope: `Scan` stays disabled.

#### Non-examples

- Running MVP preflight from the WPF app.
- Creating fixture files.
- A Cleanup Action.
- Cleanup approval.
- Proof that preflight was actually run.

#### Lifecycle

- Generated when the WPF app starts.
- Updates when the Cleanup Scope text changes or acknowledgement changes.
- Is not persisted.
- Does not modify files.

#### Relationships

- Uses Cleanup Scope Safety Note.
- Supports fixture-based verification before real-profile scans.
- Does not affect Storage Scan classification, recommendations, Review Shortlist, Quarantine Preview, or cleanup eligibility.

#### Code implications

- Use `CleanupScopeScanGate` and `CleanupScopeScanGateBuilder`.
- Keep the gate local and read-only.
- Reset the acknowledgement when the Cleanup Scope changes.
- Continue to enforce the gate in the scan-start method, not only through button state.

### Cleanup Candidate

Status: draft  
Last reviewed: 2026-05-28

#### Definition

A Cleanup Candidate is a file or folder proposed for user review because it may be unwanted bloat or may offer recoverable storage.

A Cleanup Candidate is not automatically safe to remove.

#### Examples

- A large cache folder.
- A temporary installer.
- A duplicate export.
- A stale download the user recognizes as unwanted.

#### Non-examples

- A confirmed file to delete.
- A protected profile folder.
- A file marked only because it is hidden, old, or large.

#### Lifecycle

- Created by a scan or rule.
- Reviewed by the user.
- Can be ignored, marked safe to remove, moved to quarantine, moved to Recycle Bin, or deleted only through an explicit Cleanup Action.

#### Relationships

- Belongs to a Cleanup Scope.
- May be associated with a Bloat Category, estimated Storage Savings, Importance Rating, Deletion Recommendation, risk level, and evidence.

#### Code implications

- Use `CleanupCandidate` for candidate entities.
- Use `cleanupCandidateId` for identifiers.
- Use `cleanupCandidates` for collections.
- Do not call candidates `junk` in code unless the user explicitly adopts that term.

### Bloat Category

Status: draft  
Last reviewed: 2026-05-28

#### Definition

A Bloat Category is a human-readable reason a Cleanup Candidate may be unwanted or removable.

Initial candidate categories include cleanup scope roots, profile containers, AppData areas, browser data, cloud sync data, credential data, old downloads, temporary folders, installer caches, app caches, GPU shader caches, duplicate files, large old files, old game files, game data, Node or Python package caches, Windows app data, Windows app leftovers, and installed applications.

#### Examples

- Old downloads.
- Cleanup scope roots.
- Profile containers.
- AppData areas.
- Browser data.
- Cloud sync data.
- Credential data.
- Temporary folders.
- Installer caches.
- App caches.
- GPU shader caches.
- Duplicate files.
- Large old files.
- Old game files.
- Game data, including game saves, game profiles, mod managers, and mod-loader configuration.
- Node package caches.
- Python package caches.
- Windows app data.
- Windows app leftovers.
- Installed applications.

#### Non-examples

- "Large file" by itself without stale last-modified evidence.
- "Unknown folder" by itself.
- Any category that implies deletion is safe without evidence.
- A container category by itself; containers should usually be inspected through their children.
- `No category`; uncategorized rows are filterable but do not have a Bloat Category.

#### Lifecycle

- Assigned during scan or review.
- Refined when the user confirms or rejects recommendations.
- Used to explain why a candidate may be removable.

#### Relationships

- Helps explain Cleanup Candidates.
- Feeds Importance Ratings and Deletion Recommendations.

#### Code implications

- Use `BloatCategory` for the category concept.
- Use specific category names in code and UI rather than generic labels such as `junk`.
- Container categories such as Profile container, AppData area, and Browser data should be conservative and should not imply cleanup approval.
- Specific rebuildable cache rows with strong cache evidence, such as GPU shader caches or package-cache rows under an app cache path, may be `Likely safe` / `Quarantine candidate`; broad parent containers such as `AppData`, `NVIDIA`, or `pip` should remain inspection-first.

### Cloud Sync Data

Status: draft
Last reviewed: 2026-05-29

#### Definition

Cloud Sync Data is user-owned data managed by a cloud sync provider or local sync client.

It may be large, duplicated, or stale-looking, but it should be treated as high-risk until the user reviews the sync status and the provider-specific recovery path.

#### Examples

- OneDrive folders.
- Dropbox folders.
- Google Drive folders.
- iCloud Drive or iCloud Photos folders.
- Nextcloud, Syncthing, or MEGA sync folders.

#### Non-examples

- A downloaded installer copied into Downloads.
- A local cache with clear rebuildable-cache evidence.
- A cloud sync app cache that has been separately reviewed and classified as a narrow cache path.

#### Lifecycle

- Identified during Storage Scan classification.
- Shown for review as `Cloud sync data`.
- Treated as a Protected Location by default.

#### Relationships

- Belongs to Bloat Category.
- Supports Protected Location and the rule that user-owned files are not cleanup candidates by default.

#### Code implications

- Use `BloatCategory.CloudSyncData`.
- Keep Cloud Sync Data as `High risk` / `Keep` until the user defines precise reviewed exceptions.

### Credential Data

Status: draft
Last reviewed: 2026-05-29

#### Definition

Credential Data is authentication, password manager, key, token, or sensitive access configuration data.

It must be treated as high-risk even when it appears small, duplicated, hidden, stale, or cache-like.

#### Examples

- SSH key folders or private key files.
- `.gnupg`, `.aws`, `.azure`, or `.kube` configuration folders.
- 1Password, Bitwarden, KeePass, or KeePassXC folders.
- KeePass `.kdbx` vault files.

#### Non-examples

- Browser cache files without profile or credential evidence.
- Public documentation about credentials.
- A test fixture key inside the repo test tree.

#### Lifecycle

- Identified during Storage Scan classification.
- Shown for review as `Credential data`.
- Treated as a Protected Location by default.

#### Relationships

- Belongs to Bloat Category.
- Supports Protected Location, Selected Path Review Guidance, Quarantine Preview blockers, and the rule that credentials must not be cleaned automatically.

#### Code implications

- Use `BloatCategory.CredentialData`.
- Keep Credential Data as `High risk` / `Keep`.
- Do not preview or export credential file contents.

### Storage Review Filter

Status: draft  
Last reviewed: 2026-05-28

#### Definition

A Storage Review Filter narrows Storage Scan results so the user can inspect a focused set of paths.

Initial filters are All, Likely safe, Caution, High risk, Quarantine candidates, and Access issues.

#### Examples

- Filter to High risk rows to review protected paths.
- Filter to Quarantine candidates to inspect likely cleanup opportunities.
- Filter to Caution rows to review cache and package-cache areas.
- Filter to Access issues to inspect paths the scan could not fully read.

#### Non-examples

- A Cleanup Action.
- A deletion approval.
- A hidden rule that changes scanner results.

#### Lifecycle

- Available after a Storage Scan completes.
- Applies to the in-memory review results.
- Does not modify files.

#### Relationships

- Uses Importance Ratings and Deletion Recommendations.
- Uses scanner access status for Access issues.
- Helps review Cleanup Candidates.

#### Code implications

- Use `StorageReviewFilter` for filter values.
- Keep filters read-only.
- Show counts so the user can understand scan composition.
- Access issues are informational and must not trigger permission changes or cleanup actions.

### Storage Review Search

Status: draft
Last reviewed: 2026-05-28

#### Definition

Storage Review Search is a read-only text search applied to the current Storage Scan review results.

By default, it matches path, name, category, Importance Rating, Deletion Recommendation, evidence, Access Status, and access issue text. It combines with the active Storage Review Filter and Bloat Category Filter.

Recognized field prefixes restrict search to one field:

- `path:`
- `name:`
- `category:` or `cat:`
- `rating:` or `importance:`
- `recommendation:` or `rec:`
- `evidence:`
- `issue:` or `access:`

#### Examples

- Search for `pip` to find Python package cache paths.
- Search for `NVIDIA` to find GPU shader cache paths.
- Search for `high risk` to find rows with the High risk Importance Rating.
- Search for `path:pip` to search only full paths.
- Search for `category:Python package cache` to search only Bloat Category labels.
- Search for `recommendation:Quarantine candidate` to search only Deletion Recommendation labels.
- Search for `access:readable` to search only rows that were readable during scan.
- Search for `access:access issue` or `issue:denied` to search scan access status or access issue text.
- Search for a game, app, or folder name before deciding whether to inspect it in Explorer.

#### Non-examples

- A Cleanup Action.
- A scan rescan.
- A filesystem search outside the completed Storage Scan result.
- A persisted rule or user decision.

#### Lifecycle

- Available after a Storage Scan completes.
- Resets when a new Storage Scan completes.
- Does not modify files.

#### Relationships

- Uses Storage Scan results.
- Combines with Storage Review Filters and Bloat Category Filters.
- Supports Selected Path Hierarchy Context, Selected File Content Preview, Selected Path Review Guidance, Child Breakdown, Selected Path Inspection, and Review Shortlist.

#### Code implications

- Use `StorageReviewSearch` for the search query.
- Parse recognized field prefixes into `StorageReviewSearchField`.
- Keep search in-memory and read-only.
- Do not use search text to change Bloat Categories, Importance Ratings, Deletion Recommendations, or cleanup eligibility.

### Access Status

Status: draft
Last reviewed: 2026-05-28

#### Definition

Access Status is the read-only label that tells whether a Storage Scan row was readable during the scan.

Initial user-facing values are `Readable` and `Access issue`.

#### Examples

- Show `Readable` for a fixture `Downloads` folder.
- Show `Access issue` for a path that could not be fully read.
- Export both Access Status and the scanner access issue message to CSV.

#### Non-examples

- A permission change.
- A retry-as-admin workflow.
- Cleanup approval.
- A reason to hide the row.

#### Lifecycle

- Derived during Storage Scan.
- Shown in WPF row details and reports after the scan completes.
- Remains informational and read-only.

#### Relationships

- Supports Storage Review Filter, Storage Review Search, Review Mix, Storage Scan Safety Summary, Scan Report Export, and Quarantine Preview CSV Export.
- Can be searched with Storage Review Search through `access:` and `issue:` field prefixes.
- Access issue rows should be reviewed before trusting cleanup recommendations for nearby or parent paths.

#### Code implications

- Use `StorageEntryRow.AccessStatus` for WPF display.
- Use `Readable` and `Access issue` consistently in UI/tests/docs.
- Include Access Status in broad search and `access:` / `issue:` field-prefix search.
- Export Access Status separately from access issue error text.
- Do not retry, elevate, or change permissions based on Access Status.

### Storage Entry Type Filter

Status: draft
Last reviewed: 2026-05-28

#### Definition

The Storage Entry Type Filter is a read-only lens that narrows completed Storage Scan review rows to all rows, files only, or folders only.

It helps the user separate actual files from container folders in a recursive scan.

#### Examples

- Show only files while looking for individual large stale files.
- Show only folders while inspecting large containers and child breakdowns.
- Combine Files with `Large old file` or `Installer cache`.
- Combine Folders with `Protected location` or `No category`.

#### Non-examples

- A Cleanup Action.
- A confirmation that files are safe to delete.
- A change to scanner traversal.
- A replacement for Bloat Category Filter or Storage Review Search.

#### Lifecycle

- Available after a Storage Scan completes.
- Resets to `All types` after a new Storage Scan completes.
- Combines with Storage Review Filter, Bloat Category Filter, Storage Size Threshold Filter, and Storage Review Search.
- Does not modify files.

#### Relationships

- Uses Storage Scan review results.
- Helps interpret recursive scan output where folders and files appear together.
- Applies to Scan Report Export row selection and suggested filenames.

#### Code implications

- Use `StorageEntryTypeFilter`.
- Apply type filtering in `StorageScanReview` before search.
- Keep type filtering read-only and in-memory.
- Do not use type filters as cleanup approval.

### Storage Size Threshold Filter

Status: draft
Last reviewed: 2026-05-28

#### Definition

The Storage Size Threshold Filter is a read-only lens that narrows completed Storage Scan review rows to paths at or above a selected row-size threshold.

It helps the user focus a large recursive scan on the biggest review targets without treating size as cleanup safety evidence.

#### Examples

- Show only `1 GB+` rows while reviewing a real-profile scan.
- Show only `5 GB+` rows to compare the largest app/cache containers.
- Combine `1 GB+` with `Folders` and `No category` to classify large unknown folders.
- Combine `100 MB+` with `Quarantine candidates` to review large specific cache rows.

#### Non-examples

- A Cleanup Action.
- A confirmation that a large file or folder is bloat.
- A storage-savings estimate.
- A change to scanner traversal or Bloat Category assignment.

#### Lifecycle

- Available after a Storage Scan completes.
- Resets to `All sizes` after a new Storage Scan completes.
- Combines with Storage Review Filter, Bloat Category Filter, Storage Entry Type Filter, and Storage Review Search.
- Does not modify files.

#### Relationships

- Uses Storage Scan review results.
- Complements Storage Review Size Note because recursive folder rows can overlap with child rows.
- Applies to Scan Report Export row selection and suggested filenames.

#### Code implications

- Use `StorageSizeThresholdFilter`.
- Apply size-threshold filtering in `StorageScanReview` before search.
- Keep size-threshold filtering read-only and in-memory.
- Do not use size thresholds as cleanup approval or importance scoring.

### Review View Reset

Status: draft
Last reviewed: 2026-05-28

#### Definition

Review View Reset is a read-only action that clears the active Storage Scan review lens and returns the grid to the default view.

It restores:

- Storage Review Filter: `All`
- Bloat Category Filter: `All categories`
- Storage Entry Type Filter: `All types`
- Storage Size Threshold Filter: `All sizes`
- Storage Review Search: empty

It does not clear Review Shortlist and does not modify files.

#### Examples

- Reset after combining `Files`, `Large old file`, and `path:` search.
- Reset after using Safety Summary shortcuts.
- Reset after inspecting a category-filtered review window.

#### Non-examples

- Clearing Review Shortlist.
- Clearing Quarantine Preview.
- A Cleanup Action.
- A rescan.

#### Lifecycle

- Available after a Storage Scan completes and the active review lens is filtered.
- Disabled when the review view is already at the default lens.
- Resets to disabled after Review View Reset succeeds.

#### Relationships

- Uses Storage Review Filter, Bloat Category Filter, Storage Entry Type Filter, Storage Size Threshold Filter, and Storage Review Search.
- Keeps Review Shortlist separate from the active review lens.
- Helps recover from stacked filters during manual review.

#### Code implications

- Use `ResetReviewView` in WPF.
- Preserve `StorageReviewShortlist`.
- Keep reset status wording explicit that no files were modified and the shortlist was kept.

### Storage Review Display Limit

Status: draft
Last reviewed: 2026-05-28

#### Definition

The Storage Review Display Limit is the maximum number of matched Storage Scan review rows shown in the WPF results grid at one time.

The current display limit is 2,000 rows. It is a UI review boundary, not a scanner boundary.

#### Examples

- A real scan may contain 188,580 files and initially show only rows `1-2,000` of the matched review rows in the grid.
- The filter summary can say `rows 2,001-4,000 of 12,345 matched` when the user moves to the next Storage Review Display Window.
- Narrowing by Storage Review Search, Storage Entry Type Filter, Storage Size Threshold Filter, or Bloat Category Filter can reduce the matched set and reset the display window to the first matched rows.

#### Non-examples

- A Cleanup Action.
- A scan failure.
- A sign that the scanner skipped the rest of the Cleanup Scope.
- A storage-savings estimate.

#### Lifecycle

- Applies after a Storage Scan completes and review rows are available.
- Recomputed when the active Storage Review Filter, Bloat Category Filter, Storage Entry Type Filter, Storage Size Threshold Filter, Storage Review Search, or Review Shortlist changes.
- Does not modify files.

#### Relationships

- Uses Storage Scan review results.
- Combines with Storage Review Filter, Bloat Category Filter, Storage Entry Type Filter, Storage Size Threshold Filter, and Storage Review Search.
- Helps keep very large scans reviewable without hiding that additional matched rows exist.

#### Code implications

- Keep the limit in the WPF review layer.
- Show the current row range and matched count when the limit is reached.
- Provide read-only Previous rows and Next rows controls over matched in-memory review results.
- Do not treat displayed rows as the complete scan result when exporting, documenting scan scope, or describing review behavior.

### Storage Review Display Window

Status: draft
Last reviewed: 2026-05-28

#### Definition

A Storage Review Display Window is the current slice of matched Storage Scan review rows shown in the WPF grid under the Storage Review Display Limit.

The initial window starts at row `1`. Previous rows and Next rows move the window through the matched in-memory result set without rescanning.

#### Examples

- `rows 1-2,000 of 12,345 matched`
- `rows 2,001-4,000 of 12,345 matched`
- `rows 4,001-4,112 of 4,112 matched`

#### Non-examples

- A Cleanup Action.
- A rescan.
- A change to Bloat Categories, Importance Ratings, or Deletion Recommendations.
- A limit on CSV export rows.

#### Lifecycle

- Available after a Storage Scan completes.
- Resets to the first matched rows when the active review lens changes.
- Moves backward or forward only within the current matched review result.
- Does not modify files.

#### Relationships

- Uses Storage Review Display Limit.
- Uses Storage Review Filter, Bloat Category Filter, Storage Entry Type Filter, Storage Size Threshold Filter, and Storage Review Search.
- Defines which rows `Shortlist shown` and `Remove shown` apply to.
- Does not constrain Scan Report Export, which uses the full active review lens rather than only the visible window.

#### Code implications

- Use `_currentDisplayStartIndex` for the WPF display window offset.
- Reset the offset when filters, type filters, size threshold filters, category filters, search, safety shortcuts, or Review View Reset change the active lens.
- Keep Previous rows and Next rows read-only and disabled when no adjacent window exists.

### Bloat Category Filter

Status: draft
Last reviewed: 2026-05-28

#### Definition

A Bloat Category Filter narrows Storage Scan results to rows that match one Bloat Category, or to rows with no category.

It is a read-only lens that can be combined with the active Storage Review Filter, Storage Entry Type Filter, Storage Size Threshold Filter, and Storage Review Search.

#### Examples

- Filter all rows to App cache.
- Filter Caution rows to Python package cache.
- Filter High risk rows to Protected location.
- Filter Access issues rows to Access issue.
- Filter to No category to inspect uncategorized rows.

#### Non-examples

- A Cleanup Action.
- A deletion approval.
- A guarantee that every row in the category is safe to remove.

#### Lifecycle

- Available after a Storage Scan completes.
- Built from Bloat Categories found in the current scan.
- Includes No category when the current scan contains Uncategorized Results.
- Resets to All categories at the start of a new scan.
- Does not modify files.

#### Relationships

- Uses Bloat Categories.
- Combines with Storage Review Filter, Storage Entry Type Filter, Storage Size Threshold Filter, and Storage Review Search.
- A row may match more than one Bloat Category Filter because a row may have multiple categories.
- No category matches Uncategorized Results and is not itself a Bloat Category.

#### Code implications

- Use `StorageCategorySummaryEntry` for category counts and largest-row size.
- Use `StorageCategoryFilter` to represent All categories, a named Bloat Category, or No category.
- Do not sum recursive row sizes across category summaries.
- Keep category filtering separate from Cleanup Actions.

### Uncategorized Result

Status: draft
Last reviewed: 2026-05-28

#### Definition

An Uncategorized Result is a Storage Scan row with no assigned Bloat Category.

It appears as `None` in the Categories column and can be reviewed through the No category filter.

#### Examples

- A large app folder that has not matched any classifier rule.
- A file whose name, path, age, and location do not match current cleanup evidence.

#### Non-examples

- A row with the `Unknown` Bloat Category.
- A row that is safe to remove.
- A row that should be ignored.

#### Lifecycle

- Created implicitly when classifier rules assign no Bloat Category.
- Can become categorized later if classifier rules improve.
- Remains read-only.

#### Relationships

- Is visible through the Bloat Category Filter's No category option.
- Helps identify classifier gaps.

#### Code implications

- Represent as an empty `BloatCategories` list.
- Do not add `BloatCategory.Unknown` just to make the row filterable.
- Use `StorageCategoryFilter.NoCategory` to filter these rows.

### Review Mix

Status: draft  
Last reviewed: 2026-05-28

#### Definition

Review Mix is a read-only summary of how Storage Scan results are distributed across Importance Ratings, Deletion Recommendations, and scanner access status.

It shows counts and largest-row sizes for Likely safe, Caution, High risk, and Quarantine candidates. It also shows the Access issues count so incomplete scan coverage is visible.

#### Examples

- Likely safe count and largest likely-safe row.
- Caution count and largest caution row.
- High risk count and largest high-risk row.
- Quarantine candidate count and largest quarantine-candidate row.
- Access issues count.

#### Non-examples

- A sum of all filtered row sizes.
- Confirmed Storage Savings.
- A cleanup approval.

#### Lifecycle

- Generated after a Storage Scan completes.
- Updates with the scan result.
- Remains read-only.

#### Relationships

- Uses Storage Review Filters.
- Uses Importance Ratings and Deletion Recommendations.
- Uses scanner access status.
- Helps the user decide where to inspect next.

#### Code implications

- Use `StorageReviewSummary` for Review Mix data.
- Do not sum recursive row sizes across flattened scan results; folders and their children overlap.
- Use largest-row sizes for triage until the app has an explicit non-overlapping selection model.
- Show Access issues as a count because unreadable rows may not have meaningful size data.

### Storage Review Size Note

Status: draft
Last reviewed: 2026-05-28

#### Definition

The Storage Review Size Note is a visible read-only reminder that folder sizes include child files and that parent/child rows can overlap in the flattened Storage Scan grid.

It tells the user to treat row sizes as triage clues, not confirmed Storage Savings.

#### Examples

- A large `AppData` folder and a large child cache folder may both appear in the review grid.
- The `Size` column can help decide what to inspect first.
- The `Size` column alone does not prove how much space a future cleanup would recover.

#### Non-examples

- A Cleanup Action.
- Confirmed Storage Savings.
- A replacement for Quarantine Preview's non-overlapping byte calculation.

#### Lifecycle

- Visible in the WPF Storage Scan review surface.
- Does not change scan, filter, shortlist, export, or preview behavior.
- Does not modify files.

#### Relationships

- Complements Review Mix, Storage Review Display Limit, Child Breakdown, and Scan Report Export.
- Reinforces the rule that flattened recursive rows must not be summed.

#### Code implications

- Keep the wording short enough to fit beside the existing review filters.
- Mention overlap and storage-savings semantics directly.
- Do not hide or change the underlying row sizes.

### Storage Scan Safety Summary

Status: draft
Last reviewed: 2026-05-28

#### Definition

A Storage Scan Safety Summary is a read-only health readout for the current Storage Scan.

It summarizes safety-relevant scan signals such as high-risk rows, Protected Locations, access issues, reparse points, Quarantine candidates, and Uncategorized Results.

#### Examples

- Show that the scan was read-only and no files were modified.
- Show that access issues mean scan coverage is incomplete.
- Show bounded access issue examples so the user can see where incomplete coverage starts.
- Show bounded Quarantine candidate examples so the user can see the largest review candidates without treating them as cleanup approval.
- Show bounded Uncategorized Result examples so the user can see which large no-category rows need classification.
- Show that Protected Locations and high-risk rows require manual review.
- Show that Quarantine candidates are only preview candidates, not approved cleanup.

#### Non-examples

- A Cleanup Action.
- A safety guarantee.
- A deletion approval.
- A replacement for inspecting rows.

#### Lifecycle

- Generated after a Storage Scan completes.
- Updates with the current scan result.
- Remains read-only.
- Is cleared by a new scan until the next result is available.

#### Relationships

- Uses Storage Scan results and Review Mix counts.
- Uses Bloat Categories, Importance Ratings, Deletion Recommendations, and scanner access status.
- Supports Review Shortlist and Quarantine Preview by reminding the user which boundaries remain unresolved.
- May expose read-only review shortcuts for safety-relevant buckets such as High risk, Protected Locations, Access issues, Reparse points, Quarantine candidates, and Uncategorized Results.

#### Code implications

- Use `StorageScanSafetySummary` and `StorageScanSafetySummaryBuilder`.
- Use `StorageScanSafetyShortcut` and `StorageScanSafetyShortcutFilterBuilder` to map summary shortcuts to Storage Review Filters and Bloat Category Filters.
- Keep access issue examples bounded and derived from scan rows.
- Keep Quarantine candidate examples bounded, size-labeled, cleanup-scope-relative, and derived from scan rows.
- Keep Uncategorized Result examples bounded, size-labeled, cleanup-scope-relative, and derived from scan rows.
- Keep summary notes derived from scan results only.
- Do not trigger permission changes, cleanup actions, quarantine actions, or rescans from the summary.

### Child Breakdown

Status: draft  
Last reviewed: 2026-05-28

#### Definition

A Child Breakdown is a read-only summary of the largest immediate children inside a selected folder.

It helps the user understand what is inside large container folders such as `AppData`, `Local`, `pip`, or browser folders without treating the container itself as safe to clean.

#### Examples

- Selecting `AppData` shows its largest immediate children such as `Local`, `Roaming`, and `LocalLow`.
- Selecting `pip` shows its largest cache subfolders.
- Selecting a file shows that files do not have children.

#### Non-examples

- A recursive duplicate of the entire results table.
- A Cleanup Action.
- A guarantee that child paths are safe to remove.

#### Lifecycle

- Generated from a completed Storage Scan.
- Updates when the user selects a different result row.
- Remains read-only.

#### Relationships

- Belongs to a selected Storage Scan result.
- Uses the same Importance Ratings, Deletion Recommendations, Bloat Categories, and Storage Savings values as the main result.

#### Code implications

- Use `StorageChildSummaryBuilder` for child summaries.
- Show only immediate children by default.
- Sort children by size descending.
- Keep the number of displayed children bounded.

### Selected Path Inspection

Status: draft  
Last reviewed: 2026-05-28

#### Definition

Selected Path Inspection is a read-only action that helps the user inspect a selected Storage Scan result outside the table.

Initial actions are copying the selected path and opening the selected path in File Explorer.

#### Examples

- Copy the selected path so it can be pasted into another tool.
- Open a selected folder in File Explorer.
- Select a file in File Explorer.

#### Non-examples

- Deleting a file.
- Moving a file.
- Quarantining a file.
- Editing a file.

#### Lifecycle

- Available only after a row is selected.
- Uses the selected Storage Scan result.
- Does not change files or recommendations.

#### Relationships

- Supports manual review of Cleanup Candidates.
- Complements Child Breakdown and Storage Review Filters.

#### Code implications

- Use `PathInspectionPlanBuilder` for Explorer launch details.
- Keep inspection actions separate from Cleanup Actions.
- Status messages should state that no files were modified.

### Selected Path Hierarchy Context

Status: draft
Last reviewed: 2026-05-28

#### Definition

Selected Path Hierarchy Context is read-only cleanup-scope-relative path, parent-path, and depth information shown for Storage Scan review rows.

It helps explain short or hashed names in deep cache folders without changing the row's Importance Rating, Deletion Recommendation, or cleanup eligibility.

#### Examples

- Show the parent path for a row named `e` inside a package cache.
- Show `AppData\Local\pip\Cache` instead of making the user infer location from full parent paths.
- Show hierarchy depth for a selected recursive scan row.
- Use parent/depth context alongside Evidence, Review Guidance, and Child Breakdown.

#### Non-examples

- A Cleanup Action.
- Cleanup approval.
- A classifier rule.
- A storage savings estimate.

#### Lifecycle

- Available after a Storage Scan completes.
- Updates when the grid row or selected row changes.
- Remains read-only and in-memory.

#### Relationships

- Supports manual review of deeply nested Storage Scan rows.
- Complements Storage Review Search, Storage Entry Type Filter, Selected Row Contents Context, Child Breakdown, and Selected Path Inspection.
- Scan Report Export also includes relative path and parent/depth context for offline review.

#### Code implications

- Use `StorageEntryRow.ParentLocation` for the WPF review grid.
- Use `StorageEntryRow.RelativePath` for cleanup-scope-relative display in the WPF review grid and selected-row detail pane.
- Keep relative path and parent/depth display derived from scan results.
- Do not use relative path, parent path, or depth context as cleanup approval.

### Selected Row Contents Context

Status: draft
Last reviewed: 2026-05-28

#### Definition

Selected Row Contents Context is read-only file and folder count information shown for Storage Scan review rows.

For folder rows, it shows contained files and descendant folders. For file rows, it identifies the row as a single file.

#### Examples

- Show that `AppData` contains many files and descendant folders.
- Show that `Downloads` contains one fixture installer in a smoke test.
- Show contents counts in the WPF grid before a row is selected.
- Include contained file/folder counts in Scan Report CSV exports.

#### Non-examples

- A Cleanup Action.
- Cleanup approval.
- A storage savings estimate.
- A replacement for Largest immediate children.

#### Lifecycle

- Available after a Storage Scan completes.
- Shown in the WPF grid for visible review rows.
- Updates when selected row changes.
- Included in CSV report exports.
- Remains read-only and in-memory except for user-selected CSV reports.

#### Relationships

- Supports manual review of recursive folder rows.
- Complements Selected Path Hierarchy Context, Child Breakdown, Storage Review Size Note, and Scan Report Export.
- Does not change Bloat Categories, Importance Ratings, Deletion Recommendations, Review Shortlist, or Quarantine Preview.

#### Code implications

- Use `StorageEntryRow.Contents`, `ContainedFileCount`, and `ContainedFolderCount` for WPF display.
- Use `StorageEntryRow.ContainedTotalCount` as the WPF grid sort key for the `Contents` column.
- Keep the grid Contents column compact so it helps triage without replacing selected-row detail.
- In CSV exports, use numeric contained counts for offline comparison.
- For folders, descendant folder count should exclude the selected folder itself.
- Do not present counts as confirmed recoverable storage.

### Selected File Content Preview

Status: draft
Last reviewed: 2026-05-29

#### Definition

Selected File Content Preview is an explicit read-only action that shows a bounded text sample from the currently selected file.

It exists to help the user understand unfamiliar file rows before deciding whether to inspect, ignore, or shortlist them.

#### Examples

- Preview a small `.txt`, `.json`, `.log`, `.csv`, or config-like file.
- Show that a selected file is binary or unsupported instead of rendering unreadable bytes.
- Explain that folders use Child Breakdown rather than file content preview.
- Explain that Credential Data contents are not shown.

#### Non-examples

- A Cleanup Action.
- Cleanup approval.
- A full file viewer or editor.
- A binary, archive, database, browser profile, credential, or installer extractor.
- A way to view secrets, keys, password vaults, tokens, or authentication configuration.

#### Lifecycle

- Available after a Storage Scan row for a file is selected.
- Runs only when the user invokes `Preview file`.
- Reads a bounded sample and stores no preview history.
- Does not modify files.

#### Relationships

- Supports manual inspection of Storage Scan rows.
- Complements Selected Path Hierarchy Context, Selected Path Review Guidance, Child Breakdown, and Selected Path Inspection.
- Does not affect Bloat Categories, Importance Ratings, Deletion Recommendations, Review Shortlist, or Quarantine Preview.

#### Code implications

- Use `SelectedFileContentPreview` and `SelectedFileContentPreviewBuilder`.
- Keep default reads bounded to a small text sample.
- Do not render binary-looking content as text.
- Do not render Credential Data content.
- Keep status wording explicit that no files were modified.

### Selected Path Review Guidance

Status: draft
Last reviewed: 2026-05-28

#### Definition

Selected Path Review Guidance is read-only wording shown for the currently selected Storage Scan row. It explains the safest next review step for that row, based on access status, reparse-point status, Bloat Categories, Importance Rating, and Deletion Recommendation.

#### Examples

- Tell the user to inspect children instead of shortlisting a whole profile container.
- Tell the user to keep Protected Location rows by default.
- Tell the user that a Quarantine candidate may be added to Review Shortlist only after recognition and review.
- Tell the user that GPU shader cache candidates may be rebuildable but can cause temporary recompile delays.
- Tell the user that Python or Node package cache candidates should be separated from active development tools and Codex-related paths before using Review Shortlist.
- Tell the user to classify Uncategorized Results before cleanup.

#### Non-examples

- A Cleanup Action.
- Cleanup approval.
- A replacement for category evidence or Child Breakdown.
- A persisted user decision.

#### Lifecycle

- Generated when a Storage Scan result row is selected.
- Updates when selection changes.
- Remains read-only and is discarded with the current in-memory scan state.

#### Relationships

- Uses Storage Scan results, Bloat Categories, Importance Ratings, Deletion Recommendations, Protected Locations, Access issues, Reparse points, Review Shortlist, Quarantine Preview, and Child Breakdown.
- Supports manual review before any future Cleanup Action.

#### Code implications

- Use `SelectedPathReviewGuidance` and `SelectedPathReviewGuidanceBuilder`.
- Keep guidance conservative and action-oriented.
- Do not let guidance create, move, delete, quarantine, restore, or persist files.

### Review Shortlist

Status: draft
Last reviewed: 2026-05-28

#### Definition

A Review Shortlist is a temporary in-memory set of Storage Scan rows the user marks for follow-up during manual review.

It is not a cleanup approval and does not modify files.

#### Examples

- Add a large cache folder to the Review Shortlist while inspecting Caution rows.
- Shortlist the currently shown Quarantine candidate rows after narrowing with search or filters.
- Remove the currently shown rows from the Review Shortlist after reviewing or correcting a broad filter.
- Add multiple likely cleanup opportunities, then export only those rows to CSV.
- Clear the Review Shortlist before starting a new review pass.

#### Non-examples

- A Cleanup Action.
- A Quarantine preview.
- A persisted selection history.
- Confirmation that a path should be moved or deleted.

#### Lifecycle

- Starts empty after each new Storage Scan.
- Changes only when the user adds selected rows, shortlists shown rows, removes selected rows, or clears the shortlist.
- Can be exported as a read-only CSV report.
- Is discarded when a new Storage Scan completes.

#### Relationships

- Uses Storage Scan results.
- Supports Selected Path Inspection, Storage Review Filters, Bloat Category Filters, and Scan Report Export.
- May inform a Quarantine Preview, but only after a separate explicit confirmation step is designed.

#### Code implications

- Use `StorageReviewShortlist` for the in-memory selection model.
- Bulk additions and removals should use only currently displayed rows, not hidden matched rows beyond the Storage Review Display Limit.
- Keep shortlisted paths separate from Cleanup Actions and Quarantine manifests.
- Do not persist or execute the Review Shortlist as an action without a future explicit approval workflow.

### Quarantine Preview

Status: draft
Last reviewed: 2026-05-28

#### Definition

A Quarantine Preview is a read-only dry run showing which shortlisted Storage Scan rows would be eligible for a future Quarantine action, where they would go, what size they represent, and which rows are blocked or redundant.

It does not create folders, write manifests, move files, delete files, or approve cleanup.

#### Examples

- Preview a Review Shortlist and show that `old-installer.msi` would move under `D:\WindowsFileCleanerQuarantine`.
- Block a high-risk browser profile row from the preview.
- Block a broad `.cache` parent row when its scanned subtree contains protected Codex runtime data.
- Show blocked descendant examples as cleanup-scope-relative paths so broad-parent blockers stay readable.
- Mark a child row as redundant when its selected parent is already included.

#### Non-examples

- A Cleanup Action.
- A Restore Manifest.
- A Quarantine Confirmation Draft.
- A persisted cleanup job.
- Confirmation that files should be moved.

#### Lifecycle

- Generated from the current Review Shortlist on user request.
- Uses the current Cleanup Scope and default Quarantine root.
- Shows included, blocked, and redundant rows.
- May be exported as a read-only CSV report, including Access Status and access issue text.
- Is discarded when scan results or the Review Shortlist change.

#### Relationships

- Depends on Review Shortlist.
- Precedes any future Quarantine Cleanup Action.
- Feeds Restore Manifest Draft and Quarantine Confirmation Draft work.
- Helps validate Storage Savings without double-counting selected parent/child paths.
- Uses Quarantine as the future destination concept but does not perform Quarantine.

#### Code implications

- Use `QuarantinePreview`, `QuarantinePreviewEntry`, and `QuarantinePreviewBuilder`.
- Use `QuarantinePreviewCsvExporter` for CSV reports.
- Keep preview generation read-only.
- Do not create the quarantine folder or restore manifest during preview.
- Compute previewed bytes from non-overlapping included rows only.
- Block high-risk, inaccessible, reparse-point, outside-scope, protected-location, and non-quarantine-candidate rows.
- Block parent rows that contain protected, high-risk, inaccessible, reparse-point, or Cleanup Scope Root descendants.
- Format blocked descendant examples as cleanup-scope-relative paths when possible.

### Scan Report Export

Status: draft  
Last reviewed: 2026-05-28

#### Definition

A Scan Report Export is a read-only report file generated from Storage Scan results.

The initial export format is CSV for the currently active Storage Review Filter, Storage Entry Type Filter, Storage Size Threshold Filter, selected Bloat Category Filter, and Storage Review Search.

#### Examples

- Export all Storage Scan rows to CSV.
- Export only High risk rows to CSV.
- Export Quarantine candidates to CSV for spreadsheet review.
- Export files-only rows for large file review.
- Export `1 GB+` rows for large-path review.
- Export App cache rows within the current review filter to CSV.

#### Non-examples

- A Cleanup Action.
- A restore manifest.
- A backup of files.
- A permanent scan history feature.

#### Lifecycle

- Available after a Storage Scan completes.
- Uses the current Storage Review Filter, Storage Entry Type Filter, Storage Size Threshold Filter, selected Bloat Category Filter, and Storage Review Search.
- Includes cleanup-scope-relative paths so repeated `C:\Users\moxhe` prefixes do not hide the meaningful part of each row in spreadsheets.
- Includes parent path and depth columns so recursive rows keep their hierarchy context outside the app.
- Includes Access Status and access issue text so incomplete scan coverage remains visible outside the app.
- Writes a user-selected CSV report path and suggests a filename that describes active review filters/type/size/search.
- Does not modify scanned files.

#### Relationships

- Uses Storage Scan results.
- Uses Storage Review Filters.
- Uses Storage Entry Type Filter.
- Uses Storage Size Threshold Filter.
- Uses Bloat Category Filters.
- Uses Storage Review Search.
- May export a Review Shortlist, but that export remains a report rather than a Cleanup Action.
- May export a Quarantine Preview, but that export remains a report rather than a Restore Manifest.
- Supports manual review before Quarantine or any future cleanup action.

#### Code implications

- Use `StorageScanCsvExporter` for CSV report generation.
- Export cleanup-scope-relative paths when the completed scan scope is available.
- Export user-facing labels for Importance Ratings and Deletion Recommendations.
- Export parent path and depth from the flattened Storage Review row.
- Export Access Status separately from access issue text.
- Keep generated report filenames descriptive and filesystem-safe when filters, type, size, or search are active.
- Keep exports separate from Quarantine manifests.

### Importance Rating

Status: draft  
Last reviewed: 2026-05-28

#### Definition

An Importance Rating is the app's conservative estimate of how important a file or folder may be to the user, Windows, or installed apps.

The rating helps the user decide whether a Cleanup Candidate should be ignored, inspected more deeply, quarantined, or eventually deleted.

#### Examples

- High risk: browser profiles, credentials, active app settings, source code, current Codex-related files.
- Caution: unknown app folders, package caches that may be needed by active tooling.
- Likely safe: clearly temporary files, stale installers, reviewed old downloads.

#### Non-examples

- A guarantee that cleanup is safe.
- A replacement for user review.

#### Lifecycle

- Estimated during scan.
- Displayed during review.
- May change after deeper inspection or user feedback.

#### Relationships

- Belongs to a Cleanup Candidate.
- Influences the Deletion Recommendation and risk level.

#### Code implications

- Use `ImportanceRating` for the rating concept.
- Use these user-facing values: `Likely safe`, `Caution`, `High risk`.
- Keep rating values conservative and explainable.

### Deletion Recommendation

Status: draft  
Last reviewed: 2026-05-28

#### Definition

A Deletion Recommendation is the app's suggested action for a Cleanup Candidate.

It should explain whether the candidate appears safe to ignore, inspect further, quarantine, or delete later.

#### Examples

- Keep.
- Inspect.
- Quarantine candidate.
- Delete later after quarantine and confirmation.
- Specific rebuildable cache rows such as `DXCache` or `pip\Cache` can be Quarantine candidates after the app identifies strong cache-path evidence; their broad parent folders should remain Inspect.

#### Non-examples

- Automatic deletion.
- A hidden decision made without showing the path and evidence.

#### Lifecycle

- Generated during scan or review.
- Updated after inspection.
- Can be overridden by the user.

#### Relationships

- Depends on Bloat Category, Importance Rating, risk level, and evidence.
- May lead to a Cleanup Action only after explicit approval.

#### Code implications

- Use `DeletionRecommendation` for the recommendation concept.
- Avoid action names that imply cleanup has already been approved.

### Cleanup Action

Status: draft  
Last reviewed: 2026-05-28

#### Definition

A Cleanup Action is a user-approved operation that changes files or folders, such as moving a Cleanup Candidate to the Recycle Bin, moving it to quarantine, or deleting it.

#### Examples

- Move a selected cache folder to the Recycle Bin.
- Move selected files to a quarantine folder.
- Permanently delete selected files after explicit confirmation.

#### Non-examples

- Read-only scanning.
- Estimating storage usage.
- Showing a recommendation.

#### Lifecycle

- Proposed after a scan.
- Previewed in a dry run where possible.
- Executed only after explicit confirmation.
- Logged with paths, sizes, outcome, and errors.

#### Relationships

- Applies to one or more Cleanup Candidates.
- May produce Storage Savings.
- May fail partially and require recovery guidance.

#### Code implications

- Use `CleanupAction` for actions.
- Use `cleanupActionId` for identifiers if actions are persisted.
- Separate read-only analysis code from cleanup execution code.

### Protected Location

Status: draft  
Last reviewed: 2026-05-28

#### Definition

A Protected Location is a path or category that the app should not clean automatically and should treat as high-risk.

Protected Locations may still be scanned for size and shown for inspection, but recommendations should be conservative and cleanup should be blocked or strongly warned unless the user defines a precise exception.

#### Examples

- Windows profile essentials.
- Cleanup Scope Roots.
- Documents, Desktop, Pictures, Videos, Music.
- Browser profiles and credentials.
- Cloud sync roots and synced user files.
- Credential, key, password manager, and authentication configuration folders.
- Application settings under `AppData`.
- Windows app package data under `AppData\Local\Packages`.
- Per-user installed applications under `AppData\Local\Programs`.
- Source code folders.
- Game saves, game profiles, game mods, and game configuration.
- Mod-manager folders such as Minecraft/OptiFine, CurseForge, Modrinth, Vortex, and Nexus Mods data.

#### Non-examples

- A user-selected temporary folder that has been reviewed.
- A cache category with clear regeneration behavior and user approval.

#### Lifecycle

- Defined in product rules before scans and recommendations.
- May be expanded as the user identifies sensitive locations.
- May have category-specific exceptions only after explicit user approval.

#### Relationships

- Can exist inside a Cleanup Scope.
- Blocks or raises the risk of Cleanup Candidates.

#### Code implications

- Use `ProtectedLocation` for this concept.
- Use `protectedLocationPath` for paths.
- Do not bypass protection rules in cleanup execution.

### Storage Savings

Status: draft  
Last reviewed: 2026-05-28

#### Definition

Storage Savings is the amount of disk space expected or confirmed to be recovered by cleanup.

#### Examples

- Estimated bytes for Cleanup Candidates before action.
- Confirmed bytes freed after a Cleanup Action.

#### Non-examples

- Total file size without action context.
- A guarantee that Windows will immediately report the same free-space increase.

#### Lifecycle

- Estimated during scan.
- Previewed before cleanup.
- Confirmed after cleanup when feasible.

#### Relationships

- Derived from Cleanup Candidates and Cleanup Actions.

#### Code implications

- Use `StorageSavings` when represented as a domain concept.
- Use explicit units for sizes in code and UI.
- Avoid ambiguous names such as `space` or `size` when the meaning is recoverable storage.

### Quarantine

Status: draft  
Last reviewed: 2026-05-28

#### Definition

Quarantine is a reversible holding area for Cleanup Candidates that the user has approved for removal from the original location but may want to restore.

The preferred quarantine location is on `D:`. The current preview default is `D:\WindowsFileCleanerQuarantine`; actual cleanup execution can revisit whether that should remain configurable.

#### Examples

- Moving a reviewed cache folder from `C:\Users` to a quarantine folder on `D:`.
- Keeping a manifest that records the original path so the action can be undone.

#### Non-examples

- Permanent deletion.
- Moving files without a restore manifest.
- A backup strategy for important user data.

#### Lifecycle

- Created before the first quarantine Cleanup Action.
- Receives approved Cleanup Candidates.
- Supports Undo Quarantine by restoring files to their original paths when feasible.
- May later support permanent deletion after a separate confirmation.

#### Relationships

- Is a type of Cleanup Action destination.
- Requires an Undo Quarantine workflow.
- Produces temporary Storage Savings on `C:` while consuming storage on `D:`.

#### Code implications

- Use `Quarantine` for the holding area concept.
- Use `quarantinePath` for the destination path.
- Persist a restore manifest before moving files.

### Restore Manifest

Status: draft
Last reviewed: 2026-05-28

#### Definition

A Restore Manifest is versioned metadata that records enough information to undo a completed Quarantine action.

The selected durable format is JSON with schema version `restore-manifest.v1`.

#### Examples

- Original path for each quarantined file or folder.
- Quarantine path for each moved entry.
- Size, last modified time, Importance Rating, Deletion Recommendation, Bloat Categories, and evidence captured at action time.
- Cleanup Scope and Quarantine root used for the action.

#### Non-examples

- A Quarantine Preview CSV export.
- A Restore Manifest Draft.
- A scan report.
- A backup of file contents.

#### Lifecycle

- A Restore Manifest Draft may be generated in memory from included Quarantine Preview rows.
- An executed Restore Manifest should be written only after explicit user confirmation and after Quarantine execution is attempted.
- Undo Quarantine uses the executed manifest to restore files when feasible.
- Schema changes require versioning and migration consideration.

#### Relationships

- Depends on Quarantine Preview for draft shape.
- Depends on Quarantine execution for actual manifest writing.
- Is checked by Quarantine Confirmation Draft before any future execution flow.
- Required by Undo Quarantine.

#### Code implications

- Use `RestoreManifestDraft`, `RestoreManifestEntryDraft`, `RestoreManifestDraftBuilder`, and `RestoreManifestDraftJsonSerializer` for draft-only proof.
- Do not write Restore Manifest files in preview or draft code.
- Use a versioned JSON schema for future executed manifests.
- Keep preview CSV exports separate from Restore Manifest drafts and executed manifests.

### Quarantine Confirmation Draft

Status: draft
Last reviewed: 2026-05-28

#### Definition

A Quarantine Confirmation Draft is an in-memory readiness check that compares a Quarantine Preview with a Restore Manifest Draft before any future Quarantine execution.

It lists data blockers, records the exact preview counts and bytes to review, exposes the future confirmation phrase, and states that execution is not implemented in the current build.

#### Examples

- Show that a Quarantine Preview has 1 included row, 0 blocked rows, 0 redundant rows, and matching Restore Manifest Draft metadata.
- Block confirmation when the preview still contains blocked or redundant rows.
- Block confirmation when the Restore Manifest Draft does not match the preview Cleanup Scope, Quarantine root, entry count, destination paths, or bytes.

#### Non-examples

- A Cleanup Action.
- A user approval.
- A persisted cleanup job.
- An executed Restore Manifest.
- A file-moving command.

#### Lifecycle

- Generated in memory after a Quarantine Preview and Restore Manifest Draft exist.
- Used to identify unresolved data blockers before future execution work.
- Discarded when the scan, Review Shortlist, Quarantine Preview, or Restore Manifest Draft changes.
- Must remain read-only until an explicit Quarantine execution workflow exists.

#### Relationships

- Depends on Quarantine Preview.
- Depends on Restore Manifest Draft.
- Precedes any future explicit approval or Quarantine Cleanup Action.
- Supports Undo Quarantine design by checking that preview and draft metadata agree before execution.

#### Code implications

- Use `QuarantineConfirmationDraft` and `QuarantineConfirmationDraftBuilder`.
- Use `HasDataBlockers` only as readiness evidence, not as permission to execute.
- Keep `IsExecutionImplemented` false until a separate execution packet is designed and approved.
- Do not create folders, move files, delete files, write manifests, or persist cleanup jobs from confirmation draft code.

### Undo Quarantine

Status: draft  
Last reviewed: 2026-05-28

#### Definition

Undo Quarantine restores quarantined files or folders to their original locations when feasible.

#### Examples

- Restore a quarantined app cache because an app stopped working.
- Restore a quarantined folder to inspect it again.

#### Non-examples

- Restoring permanently deleted files.
- Reconstructing files that were changed outside the app.

#### Lifecycle

- Available after a successful Quarantine action.
- Uses the restore manifest to return files to original paths.
- Must warn if the original path now exists or cannot be restored safely.

#### Relationships

- Depends on Quarantine.
- Reverses a quarantine Cleanup Action.

#### Code implications

- Use `UndoQuarantine` for the workflow concept.
- Use a manifest format that records original path, quarantine path, size, timestamps, and action time.

## Business rules

Capture durable rules that affect implementation.

### Rule: Storage Scan is the first workflow

Storage Scan is the first production workflow. It recursively scans `C:\Users\moxhe` and presents a read-only review of storage usage, Cleanup Candidates, Bloat Categories, Importance Ratings, and Deletion Recommendations.

Implementation implications:

- Recursive scanning should be expected, but it must handle access errors and long-running scans gracefully.
- The first version should verify scanner behavior against fixture directories before scanning the real Cleanup Scope.
- The WPF app may be launched with `--scope` to prefill a synthetic Cleanup Scope for manual smoke testing; startup must not auto-scan.
- The WPF app should show a Cleanup Scope Safety Note so fixture and real-profile scopes are visibly distinct.
- Storage Scan must not modify files.

### Rule: Read-only before cleanup

The first workflow should be a read-only scan and review workflow before any Cleanup Action is available.

Implementation implications:

- Scan and recommendation logic must not modify files.
- Cleanup execution must be separate from analysis.
- The user should see paths, sizes, risk level, and evidence before approval.

### Rule: Inspect before recommending removal

The app should help the user understand what is inside a folder before rating its importance or recommending cleanup.

Implementation implications:

- The scan should show folder contents, size contributors, modified dates, and category evidence where feasible.
- Deep rows should show parent/depth hierarchy context so short cache names are not reviewed in isolation.
- File content preview should be bounded, text-only, and explicit rather than automatic.
- The selected row should explain the safest next review step through Selected Path Review Guidance.
- The review should support Storage Review Search so large scans can find specific apps, caches, tools, and categories without scrolling.
- The review should distinguish displayed rows from matched rows when the Storage Review Display Limit is reached.
- Specific recognized cache rows can be recommended as Quarantine candidates, but broad parent folders should stay inspection-first until child rows prove the cleanup target.
- Protected Locations can be shown for awareness but should default to high importance.
- Recommendations should be explainable, not just a score.

### Rule: Large does not mean bloat

A file or folder must not become a low-risk Cleanup Candidate solely because it is large, hidden, old, or under `AppData`.

Implementation implications:

- Candidate evidence should explain why the item may be removable.
- Risk levels should be conservative until category rules are defined.
- Protected Locations should block or warn against cleanup.

### Rule: Explicit confirmation for Cleanup Actions

Any operation that moves, deletes, or otherwise modifies files must require explicit user confirmation.

Implementation implications:

- Cleanup Actions should have dry-run or preview output where possible.
- Future Quarantine execution should pass a Quarantine Confirmation Draft with no data blockers before asking for explicit approval.
- The app should log attempted paths, outcomes, and errors.
- Reversible actions are preferred over permanent deletion.

### Rule: Do not break current apps

Cleanup recommendations must prioritize preserving current app behavior over reclaiming maximum space.

Implementation implications:

- Treat active app state, development tooling, package caches, and Codex-related files conservatively.
- Caches may be candidates only when the app can explain likely regeneration behavior or the user accepts the risk.
- Prefer Quarantine plus Undo Quarantine before permanent deletion.

## Status and lifecycle rules

Use this section for cross-cutting lifecycle rules.

| Status | Meaning | Allowed next statuses | Notes |
|---|---|---|---|
| discovered | Found by a read-only scan. | candidate, ignored | Discovery alone does not imply safe removal. |
| candidate | Proposed for review as a Cleanup Candidate. | approved, ignored, protected | Candidate must show evidence and risk. |
| protected | Blocked or high-risk because it overlaps a Protected Location. | ignored | Protection can be revisited only with explicit user guidance. |
| approved | User approved a Cleanup Action. | executed, ignored | Approval should be specific to paths and action type. |
| executed | Cleanup Action was attempted. | reviewed | Outcome and errors should be logged. |
| quarantined | Candidate was moved to Quarantine. | restored, deleted | Preferred first cleanup execution state. |
| restored | Quarantined file or folder was restored. | reviewed | Restoration should use the original path manifest. |
| ignored | User or rule chose not to act. | candidate | Ignored items may appear again if rules change. |

## Permissions and visibility

This is a single-user local app for the project owner.

Current assumptions:

- No multi-user account system is planned.
- The app should operate only with the user's local permissions unless the user explicitly chooses a higher-privilege workflow later.
- Cleanup Actions require explicit confirmation from the local user.
- The app should not upload file inventories, paths, or cleanup reports unless a future feature explicitly adds export/sharing with user approval.

## Deletion and archival policy

Default policy for now:

- Prefer read-only analysis before cleanup.
- Prefer moving to Quarantine on `D:` over permanent deletion where feasible.
- Preserve enough manifest information to support Undo Quarantine.
- Use hard deletion only after explicit user approval and only for clearly selected Cleanup Candidates.
- Never delete the User Profile Root as a whole.
- Never clean outside the active Cleanup Scope.
- Treat Protected Locations as blocked or high-risk until the user defines precise exceptions.

## Open domain questions

Keep this short. Move feature-specific questions to `docs/features/`.

- Should the default preview root `D:\WindowsFileCleanerQuarantine` remain the default for actual Quarantine execution?
- Which locations should be protected from cleanup but still shown in the report?
