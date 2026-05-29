# Domain Context

---
last_reviewed: 2026-05-29
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
- Use `ScopePathBox` for typed Cleanup Scope Selection.
- Keep folder selection separate from `Scan`; never auto-scan after a folder is selected.
- Typed-path and browse tooltips plus automation help text should say selection is path-only and does not start a scan, bypass the real-profile gate, or approve cleanup.
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
Last reviewed: 2026-05-29

#### Definition

Cleanup Scope Scan Gate is the read-only decision that controls whether the WPF `Scan` action can start for the current Cleanup Scope.

For real-profile scopes under `C:\Users\moxhe`, the gate requires explicit acknowledgement that MVP preflight and fixture review were run before enabling `Scan`.

#### Examples

- Fixture Cleanup Scope: `Scan` can start without real-profile acknowledgement.
- Fixture Cleanup Scope: the WPF header explains that the Storage Scan is read-only first and fixture cleanup actions remain gated later.
- Real Profile Cleanup Scope: `Scan` stays disabled until the user ticks the preflight and fixture-review acknowledgement.
- Real Profile Cleanup Scope with no acknowledgement: the WPF header shows a locked scan-gate summary and the disabled `Scan` button tooltip explains the lock.
- Real Profile Cleanup Scope with acknowledgement: the WPF header shows a ready read-only Storage Scan summary and keeps real-profile cleanup execution unavailable.
- Custom Cleanup Scope: the WPF header shows a ready read-only Storage Scan summary and keeps real-profile/custom cleanup execution unavailable.
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
- Keep the gate discoverable through visible summary text, acknowledgement tooltip/help text, the `Scan` button tooltip, and `Scan` button automation help text, especially when controls are disabled.
- Acknowledgement tooltip and automation help text should say checking it only records local acknowledgement that MVP preflight and fixture review were already run; it does not run preflight, create fixtures, start scanning by itself, persist approval, or approve cleanup.
- Keep ready-state wording scope-specific: fixture scopes may later use fixture-only gated cleanup actions, while real-profile and custom scopes remain preview-only.

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
Last reviewed: 2026-05-29

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
- Filter button tooltips and automation help text should say the filter narrows completed Storage Scan rows for review only, does not rescan or modify files, and does not approve cleanup.
- Access issues are informational and must not trigger permission changes or cleanup actions.

### Storage Review Search

Status: draft
Last reviewed: 2026-05-29

#### Definition

Storage Review Search is a read-only text search applied to the current Storage Scan review results.

By default, it matches path, parent path, name, category, Importance Rating, Deletion Recommendation, evidence, Access Status, and access issue text. It combines with the active Storage Review Filter and Bloat Category Filter.

Recognized field prefixes restrict search to one field:

- `path:`
- `parent:`
- `under:`
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
- Search for `parent:C:\Users\moxhe\AppData` to search only immediate children whose parent is `AppData`.
- Search for `under:C:\Users\moxhe\AppData` to search all descendants under `AppData` without matching `AppData` itself.
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
- User-typed interactive search is applied after a short debounce so large completed scans do not re-filter on every keystroke.
- Resets when a new Storage Scan completes.
- Does not modify files.

#### Relationships

- Uses Storage Scan results.
- Combines with Storage Review Filters and Bloat Category Filters.
- Supports Selected Path Hierarchy Context, Selected File Content Preview, Selected Path Review Guidance, Child Breakdown, Selected Path Inspection, and Review Shortlist.

#### Code implications

- Use `StorageReviewSearch` for the search query.
- Parse recognized field prefixes into `StorageReviewSearchField`.
- Treat `parent:` as an immediate-parent lens, not a recursive descendant tree.
- Treat `under:` as a descendant lens that excludes the selected ancestor row.
- Keep search in-memory and read-only.
- Search input tooltip and automation help text should include prefix examples and say Storage Review Search is read-only, does not rescan, does not modify files, and does not approve cleanup.
- Clear search tooltip and automation help text should say it clears only Storage Review Search, keeps Review Shortlist, and does not rescan or modify files.
- Keep direct/programmatic search application immediate for shortcuts and tests, but debounce textbox typing before refreshing large review result sets.
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
Last reviewed: 2026-05-29

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
- Tooltip and automation help text should say the control filters completed Storage Scan rows to all rows, files only, or folders only; it does not rescan, modify files, or approve cleanup.
- Do not use type filters as cleanup approval.

### Storage Size Threshold Filter

Status: draft
Last reviewed: 2026-05-29

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
- Tooltip and automation help text should say row size is a triage clue, not Storage Savings or cleanup approval.
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
- Tooltip and automation help text should say Reset view clears filters/search, keeps Review Shortlist, and does not rescan or modify files.
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
- Previous/Next rows tooltips and automation help text should say display-window navigation is in-memory and does not rescan or modify files.
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
- Defines which rows `Shortlist visible rows` and `Remove visible rows` apply to.
- Does not constrain Scan Report Export, which uses the full active review lens rather than only the visible window.

#### Code implications

- Use `_currentDisplayStartIndex` for the WPF display window offset.
- Reset the offset when filters, type filters, size threshold filters, category filters, search, safety shortcuts, or Review View Reset change the active lens.
- Keep Previous rows and Next rows read-only and disabled when no adjacent window exists.

### Bloat Category Filter

Status: draft
Last reviewed: 2026-05-29

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
- Tooltip and automation help text should say category filtering is a read-only lens over completed Storage Scan rows and does not rescan, modify files, or approve cleanup.
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

### Matched Review Mix

Status: draft
Last reviewed: 2026-05-29

#### Definition

Matched Review Mix is a read-only summary of the currently matched Storage Scan review rows after the active review lens is applied.

The active review lens can include Storage Review Filter, Bloat Category Filter, Storage Entry Type Filter, Storage Size Threshold Filter, Storage Review Search, Storage Review Display Window position, Selected Folder Child Focus, or Selected Folder Descendant Focus. Matched Review Mix counts the full matched set, not only the currently displayed 2,000-row window.

#### Examples

- After `under:C:\Users\moxhe\AppData`, show how many matched descendant rows are Likely safe, Caution, High risk, Quarantine candidates, Protected Location rows, Access issues, and Uncategorized Results.
- After `category:Python package cache`, show that the current matched rows are focused on that category.
- After a Size filter such as `1 GB+`, show the review mix of the large matched rows.

#### Non-examples

- A Cleanup Action.
- Cleanup approval.
- A storage savings estimate.
- A replacement for Review Mix over the whole scan.
- A replacement for selected-folder Descendant review summary.

#### Lifecycle

- Appears after a Storage Scan completes.
- Recomputes when the active review lens changes.
- Resets when a new Storage Scan completes.
- Does not modify files.

#### Relationships

- Uses completed Storage Scan review rows.
- Complements Review Mix, Filter Summary, Storage Review Display Window, Selected Folder Descendant Focus, and Selected Folder Subtree Summary.
- Supports Inspect before recommending removal by summarizing the current matched set before shortlisting or previewing.

#### Code implications

- Use `MatchedReviewMix` for the domain concept if represented in code.
- Use `MatchedReviewMixText` for the WPF readout.
- Count matched rows across the full active lens, not just the display window.
- Include wording that the summary is review context, not cleanup approval.

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
Last reviewed: 2026-05-29

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
- Safety Summary review shortcut tooltips and automation help text should keep shortcut scope, read-only behavior, no-rescan/no-file-modified boundaries, no-permission-change behavior for Access issues, no-link-following behavior for reparse points, and not-cleanup-approval wording available.
- In WPF, keep Safety Summary collapsible so the main review grid can recover vertical space during focused review.
- In WPF, keep the collapsed Safety Summary header useful with compact risk counts.
- Do not trigger permission changes, cleanup actions, quarantine actions, or rescans from the summary.

### Child Breakdown

Status: draft  
Last reviewed: 2026-05-29

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

### Storage Hotspot Trail

Status: draft
Last reviewed: 2026-05-29

#### Definition

Storage Hotspot Trail is a read-only selected-folder summary that follows the largest child at each folder level to show where a selected folder's size appears to concentrate.

It is a directional inspection aid, not a storage-savings calculation. Parent and child sizes overlap down the trail.

#### Examples

- Selecting `AppData` can show a largest-child path such as `Local -> NVIDIA -> DXCache -> shader.bin`.
- Selecting `pip` can show the largest cache bucket under `Cache`.
- Selecting a file shows that files do not have descendant hotspot trails.

#### Non-examples

- A Cleanup Action.
- A recursive tree view.
- A safety score.
- A confirmed storage savings estimate.
- A reason to quarantine a parent folder.

#### Lifecycle

- Generated from a completed Storage Scan.
- Updates when the user selects a different result row.
- Remains read-only and in-memory.
- Does not change classifications, recommendations, Review Shortlist, Quarantine Preview, or cleanup eligibility.

#### Relationships

- Complements Child Breakdown by showing one likely path through nested storage instead of several immediate children.
- Complements Selected Folder Child Focus by suggesting which child may be worth focusing next.
- Uses Storage Scan results and the same Importance Ratings, Deletion Recommendations, and Bloat Categories as the main result.

#### Code implications

- Use `StorageHotspotTrailBuilder` for hotspot trails.
- Follow the largest child by `SizeBytes`, with deterministic name tie-breaking.
- Keep trail depth bounded.
- Include files only as the terminal row in the trail.
- Keep UI wording explicit that trail sizes overlap and are not storage savings.

### Selected Folder Subtree Summary

Status: draft
Last reviewed: 2026-05-29

#### Definition

Selected Folder Subtree Summary is a read-only selected-folder summary of the descendant rows under the selected folder.

It summarizes descendant row counts by entry type, Importance Rating, Deletion Recommendation, Bloat Category risk flags, access status, and a few largest examples so the user can understand the mix inside a large folder before shortlisting or previewing anything.

#### Examples

- Selecting `AppData` shows how many descendant rows are High risk, Protected Location, Caution, Quarantine candidate, Access issue, or Uncategorized Result.
- Selecting `Downloads` shows its descendant files and any Quarantine candidate examples.
- Selecting a file shows that files do not have descendant subtree summaries.

#### Non-examples

- A Cleanup Action.
- Cleanup approval.
- A confirmed storage-savings estimate.
- A recursive tree view.
- A replacement for inspecting child rows.

#### Lifecycle

- Generated from a completed Storage Scan.
- Updates when the user selects a different result row.
- Remains read-only and in-memory.
- Does not change classifications, recommendations, Review Shortlist, Quarantine Preview, or cleanup eligibility.

#### Relationships

- Complements Child Breakdown, Storage Hotspot Trail, Selected Folder Child Focus, Review Mix, Storage Scan Safety Summary, and Quarantine Preview.
- Uses the same Storage Scan result tree, Bloat Categories, Importance Ratings, Deletion Recommendations, and access status as the main grid.
- Supports the rule that broad parent folders should stay inspection-first when their descendants include protected, high-risk, inaccessible, or uncategorized rows.

#### Code implications

- Use `StorageSubtreeReviewSummary` and `StorageSubtreeReviewSummaryBuilder`.
- Exclude the selected folder itself from descendant counts so broad parent labels do not hide the child mix.
- Keep examples bounded and largest-first where size is relevant.
- Format example paths relative to the Cleanup Scope when possible.
- Keep wording explicit that recursive folder sizes overlap and are not storage savings.

### Selected Folder Child Focus

Status: draft
Last reviewed: 2026-05-29

#### Definition

Selected Folder Child Focus is a read-only review action that narrows the Storage Scan results to the immediate children of the currently selected folder.

It is a table-level follow-up to Child Breakdown: the detail pane shows a bounded summary, while Selected Folder Child Focus lets the user inspect and sort the matching child rows in the main grid.

#### Examples

- Select `AppData`, then show only rows whose immediate parent is `AppData`.
- Select `Downloads`, then show its files and folders in the main review grid.
- Select `pip`, then inspect only its direct cache buckets before deciding whether to drill deeper.

#### Non-examples

- A Cleanup Action.
- A recursive tree expansion.
- A claim that the selected folder or its children are safe to quarantine.
- A filesystem rescan.

#### Lifecycle

- Available only after a Storage Scan completes and the selected row is a folder.
- Applies a parent-prefixed Storage Review Search for the selected folder path.
- Resets other review lenses to All so the selected folder's immediate children are visible.
- Remains in-memory and read-only.
- Clears or changes when the user edits search, changes filters, resets the review view, selects another focus, or starts a new scan.

#### Relationships

- Depends on Storage Review Search.
- Complements Child Breakdown, Selected Path Hierarchy Context, Storage Review Display Window, and Review Shortlist.
- Does not change Bloat Categories, Importance Ratings, Deletion Recommendations, Review Shortlist, Quarantine Preview, or cleanup eligibility.

#### Code implications

- Use `StorageReviewSearchField.Parent` for parent-prefixed search.
- Use `ShowSelectedFolderChildren` for the WPF selected-row action.
- Keep the action disabled for files and before a scan result exists.
- Tooltip and automation help text should say this is read-only `parent:` focus that does not rescan, modify files, or approve cleanup.
- Status text after applying this focus should say `Reset view` returns to all rows so the user has an obvious way back.
- Keep the action read-only and avoid rescanning, opening Explorer, previewing file content, or modifying files.

### Selected Folder Descendant Focus

Status: draft
Last reviewed: 2026-05-29

#### Definition

Selected Folder Descendant Focus is a read-only review action that narrows the Storage Scan results to every descendant row under the currently selected folder.

It is the recursive counterpart to Selected Folder Child Focus. It lets the user inspect all scanned rows inside a large folder while preserving normal filters, sorting, paging, search, and export behavior.

#### Examples

- Select `AppData`, then show every scanned row under `AppData`, excluding the `AppData` row itself.
- Select `NVIDIA`, then narrow the grid to all scanned cache folders and files under that folder.
- Select `pip`, then inspect all nested cache buckets and files in one review lens.

#### Non-examples

- A Cleanup Action.
- A filesystem rescan.
- Cleanup approval.
- A tree expansion.
- A confirmed storage savings estimate.

#### Lifecycle

- Available only after a Storage Scan completes and the selected row is a folder.
- Applies an under-prefixed Storage Review Search for the selected folder path.
- Resets other review lenses to All so the selected folder's descendant rows are visible.
- Remains in-memory and read-only.
- Clears or changes when the user edits search, changes filters, resets the review view, selects another focus, or starts a new scan.

#### Relationships

- Depends on Storage Review Search.
- Complements Selected Folder Subtree Summary, Child Breakdown, Storage Hotspot Trail, Selected Folder Child Focus, Storage Review Display Window, and Review Shortlist.
- Does not change Bloat Categories, Importance Ratings, Deletion Recommendations, Review Shortlist, Quarantine Preview, or cleanup eligibility.

#### Code implications

- Use `StorageReviewSearchField.Under` for under-prefixed search.
- Use `ShowSelectedFolderDescendants` for the WPF selected-row action.
- Exclude the selected folder itself from matches.
- Keep the action disabled for files and before a scan result exists.
- Tooltip and automation help text should say this is read-only `under:` focus that does not rescan, modify files, or approve cleanup.
- Keep the action read-only and avoid rescanning, opening Explorer, previewing file content, or modifying files.

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
- Tooltip and automation help text should keep Copy path and Open in Explorer framed as manual inspection, not cleanup approval or file modification by the app.
- Clipboard failures during Copy path should be handled as a warning/status update, not as an app crash.
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
- Keep status wording, tooltip wording, and automation help text explicit that no files were modified.

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
Last reviewed: 2026-05-29

#### Definition

A Review Shortlist is a temporary in-memory set of Storage Scan rows the user marks for follow-up during manual review.

It is not a cleanup approval and does not modify files.

#### Examples

- Add a large cache folder to the Review Shortlist while inspecting Caution rows.
- Shortlist the current visible Quarantine candidate rows after narrowing with search or filters.
- Remove the current visible rows from the Review Shortlist after reviewing or correcting a broad filter.
- Add multiple likely cleanup opportunities, then export only those rows to CSV.
- Clear the Review Shortlist before starting a new review pass.

#### Non-examples

- A Cleanup Action.
- A Quarantine preview.
- A persisted selection history.
- Confirmation that a path should be moved or deleted.

#### Lifecycle

- Starts empty after each new Storage Scan.
- Changes only when the user adds selected rows, shortlists visible rows, removes selected rows, or clears the shortlist.
- Can be exported as a read-only CSV report.
- Is discarded when a new Storage Scan completes.

#### Relationships

- Uses Storage Scan results.
- Supports Selected Path Inspection, Storage Review Filters, Bloat Category Filters, and Scan Report Export.
- May inform a Quarantine Preview, but only after a separate explicit confirmation step is designed.

#### Code implications

- Use `StorageReviewShortlist` for the in-memory selection model.
- Bulk additions and removals should use only currently displayed rows, not hidden matched rows beyond the Storage Review Display Limit.
- Selected-row and visible-row control tooltips and automation help text should keep row/display-window scope, no-file-modified behavior, and not-cleanup-approval boundaries available.
- Export and clear control tooltips and automation help text should keep report-only and in-memory-only behavior available before use.
- Keep shortlisted paths separate from Cleanup Actions and Quarantine manifests.
- Do not persist or execute the Review Shortlist as an action without a future explicit approval workflow.

### Review Shortlist Safety Mix

Status: draft
Last reviewed: 2026-05-29

#### Definition

Review Shortlist Safety Mix is a read-only summary of the current Review Shortlist composition.

It counts shortlisted rows by Importance Rating, Quarantine candidate recommendation, Protected Location flag, Access issue flag, Uncategorized Result flag, and largest shortlisted row size.

It is not cleanup approval, Quarantine Preview readiness, or a storage-savings estimate.

#### Examples

- Show that a shortlist has one Likely safe Quarantine candidate and no Protected Location rows.
- Show that a broad shortlisted parent still has review-only context before Quarantine Preview evaluates blockers.
- Return to an empty message after the shortlist is cleared.

#### Non-examples

- A Cleanup Action.
- A Quarantine Preview.
- Confirmation that shortlisted rows should be moved or deleted.
- A persisted cleanup history summary.

#### Lifecycle

- Appears after a Storage Scan.
- Recomputes when Review Shortlist membership changes or when a new Storage Scan clears the shortlist.
- Does not inspect the filesystem beyond completed scan results.
- Does not modify files.

#### Relationships

- Depends on Review Shortlist and completed Storage Scan results.
- Complements Matched Review Mix by summarizing selected review rows rather than the current filter/search lens.
- Precedes Quarantine Preview but does not replace it.

#### Code implications

- Use `ShortlistSafetyMixText` for the WPF readout.
- Compute from `StorageReviewShortlist.ApplyTo(_currentReview.Entries)` so the summary follows scan review ordering and completed scan data.
- Keep the readout explicitly framed as review context only, not cleanup approval.

### Quarantine Preview

Status: draft
Last reviewed: 2026-05-29

#### Definition

A Quarantine Preview is a read-only dry run showing which shortlisted Storage Scan rows would be eligible for a future Quarantine action, where they would go, what size they represent, and which rows are blocked or redundant.

It does not create folders, write manifests, move files, delete files, or approve cleanup.
The WPF preview and execution gate panes should keep the approval boundary visible: Review Shortlist and Quarantine Preview are review evidence, not cleanup approval.
Preview and preview export control tooltips and automation help text should keep dry-run and report-only boundaries available before use.

#### Examples

- Preview a Review Shortlist and show that `old-installer.msi` would move under `D:\WindowsFileCleanerQuarantine`.
- Preview the same shortlist against a typed Quarantine Root Selection on `D:`.
- Block a high-risk browser profile row from the preview.
- Block a broad `.cache` parent row when its scanned subtree contains protected Codex runtime data.
- Show blocked descendant examples as cleanup-scope-relative paths so broad-parent blockers stay readable.
- Mark a child row as redundant when its selected parent is already included.
- Show Quarantine Execution Scope Status so fixture-only availability and real-profile/custom preview-only status are clear before confirmation.
- Show approval-boundary wording so Review Shortlist and Quarantine Preview are not mistaken for cleanup approval.
- Show that the Review Shortlist is the source and only included rows can be quarantined; blocked and redundant rows stay out of execution.

#### Non-examples

- A Cleanup Action.
- A Restore Manifest.
- A Quarantine Confirmation Draft.
- A persisted cleanup job.
- Confirmation that files should be moved.

#### Lifecycle

- Generated from the current Review Shortlist on user request.
- Uses the current Cleanup Scope and Quarantine Root Selection.
- Shows included, blocked, and redundant rows.
- May be exported as a read-only CSV report, including Access Status and access issue text.
- Is discarded when scan results, the Review Shortlist, or the Quarantine Root Selection change.
- Feeds fixture-only shortlist-level execution; included rows execute together after exact confirmation, not one selected row at a time.

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
- Build destination paths from the current Quarantine Root Selection.
- Clear stale preview and draft readiness output when the Quarantine Root Selection changes.
- Include Quarantine Execution Scope Status in the WPF preview pane.
- Include approval-boundary wording in WPF preview and gate panes.
- Include wording that makes the Review Shortlist source and all-included-row execution target visible.

### Quarantine Root Selection

Status: draft
Last reviewed: 2026-05-29

#### Definition

Quarantine Root Selection is the read-only preview setting that chooses the destination root used when building Quarantine Preview destination paths. The root can be typed or chosen with the Quarantine root browse action.

The root must be fully qualified before Quarantine Preview can build destination paths. `D:` remains preferred for future quarantine storage, but a fully qualified non-`D:` path may still be used for preview with a safety note.

It does not create folders, move files, write manifests, or approve future Quarantine execution.

#### Examples

- Use the default `D:\WindowsFileCleanerQuarantine`.
- Type another absolute `D:` folder before creating a Quarantine Preview.
- Browse to a `D:` folder before creating a Quarantine Preview.
- Type a fully qualified non-`D:` folder and see a warning that `D:` remains preferred.
- Type a relative folder such as `relative\quarantine` and see Quarantine Preview disabled until the path is corrected.

#### Non-examples

- A Cleanup Action.
- A folder creation command.
- A persisted app setting.
- Approval to execute Quarantine.

#### Lifecycle

- Visible before and after Storage Scan.
- Used when the user creates a Quarantine Preview.
- Changing it clears stale Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft output.
- Browsing for a root updates the preview setting only.
- Invalid or relative roots disable Quarantine Preview without touching the filesystem.

#### Relationships

- Feeds Quarantine Preview destination paths.
- Supports the user's preference for a quarantine location on `D:`.
- Feeds Quarantine Root Safety Note so the UI explains whether preview is currently available.
- Must remain separate from Quarantine execution approval.

#### Code implications

- Use `QuarantineRootBox` for the WPF input.
- Use `BrowseQuarantineRootButton` for the WPF browse action.
- Use `QuarantineRootSafetyNote` and `QuarantineRootSafetyNoteBuilder` for preview-root messaging and gating.
- Use the typed value when calling `QuarantinePreviewBuilder`.
- Keep the default as `D:\WindowsFileCleanerQuarantine`.
- The typed root field tooltip and automation help text should say it is a read-only preview destination and preview does not create the folder or move files.
- Browse tooltips and automation help text should say selection is for preview paths only and does not create folders, move files, or approve cleanup.
- Do not create or validate the folder by touching the filesystem during preview.

### Quarantine Root Safety Note

Status: draft
Last reviewed: 2026-05-29

#### Definition

Quarantine Root Safety Note is read-only UI text explaining whether the current Quarantine Root Selection is usable for Quarantine Preview and whether it is on the preferred `D:` drive.

It is a path-shape check for preview only. It does not prove that a folder exists, create folders, move files, write manifests, or approve future Quarantine execution.

#### Examples

- Preferred Quarantine Root: the selected root is fully qualified on `D:`.
- Non-D: Quarantine Root: the selected root is fully qualified, but `D:` remains preferred for future quarantine storage.
- Choose Full Quarantine Root: the selected root is relative and Quarantine Preview is disabled.
- Invalid Quarantine Root: the selected root cannot be parsed for preview.

#### Non-examples

- A Cleanup Action.
- A Quarantine execution approval.
- A folder existence check.
- A persisted app preference.

#### Lifecycle

- Generated when the WPF app starts.
- Updates when the Quarantine Root Selection text changes.
- Controls whether `Preview quarantine` is enabled while a Review Shortlist exists.
- Is not persisted.

#### Relationships

- Supports Quarantine Root Selection and Quarantine Preview.
- Preserves fully qualified destination review before fixture-only Quarantine execution can run.
- Complements, but does not replace, execution-time validation.

#### Code implications

- Use `QuarantineRootSafetyNote` and `QuarantineRootSafetyNoteBuilder`.
- Require fully qualified roots before building Quarantine Preview destination paths.
- Keep the note informational and read-only.
- Do not touch the filesystem from the note builder.

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
- Export tooltip and automation help text should keep report-only, not-cleanup-approval, and no-scanned-file-modified boundaries available.
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
Last reviewed: 2026-05-29

#### Definition

Quarantine is a reversible holding area for Cleanup Candidates that the user has approved for removal from the original location but may want to restore.

The preferred quarantine location is on `D:`. The current preview default is `D:\WindowsFileCleanerQuarantine`, and Quarantine Root Selection can change the preview/execution destination root before fixture-only cleanup execution.

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
- Persist a planned Restore Manifest before moving files.
- Do not treat Quarantine Root Selection as approval to create folders or move files.

### Restore Manifest

Status: draft
Last reviewed: 2026-05-29

#### Definition

A Restore Manifest is versioned JSON metadata that records enough information to recover, inspect, or undo a Quarantine action after explicit execution begins.

The selected durable format is JSON with schema version `restore-manifest.v1`.

The selected write order is write-ahead: after explicit confirmation opens an execution flow, the app should write a planned Restore Manifest before the first file or folder move, then update that manifest before and after each move attempt.

#### Examples

- Original path for each quarantined file or folder.
- Action-scoped quarantine path for each selected entry.
- Size, last modified time, Importance Rating, Deletion Recommendation, Bloat Categories, and evidence captured at action time.
- Cleanup Scope, Quarantine root, action root, items root, and manifest path used for the action.
- Action status such as Planned, Moving, Completed, Partial failure, Failed, Restoring, Restored, Restore partial failure, or Restore failed.
- Entry status such as Planned, Moving, Moved, Failed, Restoring, Restored, or Restore failed.
- Failure message for an entry that could not be moved or restored.

#### Non-examples

- A Quarantine Preview CSV export.
- A Restore Manifest Draft.
- A scan report.
- A backup of file contents.
- Proof that every selected file was moved successfully.

#### Lifecycle

- A Restore Manifest Draft may be generated in memory from included Quarantine Preview rows.
- A planned Restore Manifest may be generated in memory from a Quarantine Action Draft before WPF file-moving code is wired.
- An executed Restore Manifest should be written only after explicit user confirmation starts Quarantine execution.
- The first execution write should happen before the first move, with every entry in Planned status.
- Before moving an entry, Quarantine execution should update that entry to Moving and write the manifest again.
- After each move attempt, Quarantine execution should update that entry to Moved or Failed and write the manifest again.
- Undo Quarantine uses Moved entries from the executed manifest to restore files when feasible.
- Before restoring an entry, Undo Quarantine should update that entry to Restoring and write the manifest again.
- After each restore attempt, Undo Quarantine should update that entry to Restored or Restore failed and write the manifest again.
- Planned, Moving, Failed, Restoring, and Restore failed entries require recovery review because the app may need to inspect source and destination paths before undo.
- Schema changes require versioning and migration consideration.

#### Relationships

- Depends on Quarantine Preview for draft shape.
- Depends on Quarantine Action Draft for action-scoped paths.
- Depends on Quarantine execution for actual manifest writing.
- Is checked by Quarantine Confirmation Draft before execution.
- Required by Undo Quarantine.

#### Code implications

- Use `RestoreManifestDraft`, `RestoreManifestEntryDraft`, `RestoreManifestDraftBuilder`, and `RestoreManifestDraftJsonSerializer` for draft-only proof.
- Use `RestoreManifest`, `RestoreManifestEntry`, `RestoreManifestBuilder`, and `RestoreManifestJsonSerializer` for the planned/executed action record shape.
- Use `RestoreManifestActionStatus` and `RestoreManifestEntryStatus` for move, restore, partial-failure, and recovery state.
- Use `RestoreManifestFileStore` only for writing the action-scoped Restore Manifest JSON file after explicit execution starts.
- Do not write Restore Manifest files in preview or draft code.
- Do not write Restore Manifest files from `RestoreManifestBuilder`; it models the JSON action record only.
- Use a versioned JSON schema for future executed manifests.
- Keep preview CSV exports separate from Restore Manifest drafts and executed manifests.

### Restore Manifest Action Status

Status: draft
Last reviewed: 2026-05-29

#### Definition

Restore Manifest Action Status is the overall state of a Quarantine action or Undo Quarantine action recorded in the Restore Manifest.

Current values are Planned, Moving, Completed, Partial failure, Failed, Restoring, Restored, Restore partial failure, and Restore failed.

#### Examples

- Planned: the write-ahead manifest exists before any move has begun.
- Moving: at least one entry is moving or has moved, and the action has not reached a final state.
- Completed: all entries are recorded as moved.
- Partial failure: at least one entry moved and at least one entry failed.
- Failed: every attempted entry failed or no recoverable move completed.
- Restoring: at least one entry is being restored or has restored, and the full manifest has not reached a final restored state.
- Restored: all entries are recorded as restored.
- Restore partial failure: at least one entry restored and at least one restore attempt failed.
- Restore failed: every restore attempt failed or no recoverable restore completed.

#### Non-examples

- A user approval.
- A Cleanup Action recommendation.
- A replacement for per-entry status.

#### Code implications

- Use `RestoreManifestActionStatus`.
- Treat Partial failure, Failed, Restoring, Restore partial failure, and Restore failed as recovery-review states unless the caller has separate evidence that no review is needed.

### Restore Manifest Entry Status

Status: draft
Last reviewed: 2026-05-29

#### Definition

Restore Manifest Entry Status is the per-entry move or restore state recorded in the Restore Manifest.

Current values are Planned, Moving, Moved, Failed, Restoring, Restored, and Restore failed.

#### Examples

- Planned before a selected file or folder is touched.
- Moving after the manifest is updated immediately before a move attempt.
- Moved after the move succeeds and the manifest is updated.
- Failed after a move attempt fails and the manifest records the error.
- Restoring after the manifest is updated immediately before a restore attempt.
- Restored after Undo Quarantine successfully moves the entry back to its original path.
- Restore failed after a restore attempt fails and the manifest records the error.

#### Non-examples

- A scan access status.
- A deletion recommendation.
- Proof that Undo Quarantine has run.

#### Code implications

- Use `RestoreManifestEntryStatus`.
- Undo Quarantine should restore Moved entries and require recovery review for Planned, Moving, Failed, Restoring, and Restore failed entries.

### Restore Manifest File Store

Status: draft
Last reviewed: 2026-05-29

#### Definition

Restore Manifest File Store is the narrow production component allowed to write action-scoped Restore Manifest JSON files.

It writes `restore-manifest.json` under the Quarantine Action root by first writing a temporary file in the same folder, then replacing or moving that temporary file into place.

#### Examples

- Write `D:\WindowsFileCleanerQuarantine\actions\quarantine-action-...\restore-manifest.json`.
- Replace an existing Restore Manifest after an entry status changes from Moving to Moved.

#### Non-examples

- A file-moving executor.
- A Quarantine Preview exporter.
- A UI event handler.
- A general-purpose file writer.
- Approval to create item folders or move scanned files.

#### Lifecycle

- Called by WPF fixture-only Quarantine execution after the gate opens.
- Used by fixture tests, Quarantine Executor, and Undo Quarantine Executor to prove manifest persistence against synthetic files.
- Future real-profile WPF Quarantine execution should call it only after a separate decision.
- Quarantine and Undo execution should treat manifest write failure as a blocker before moving or restoring files and as recovery-review evidence after a file has already moved.

#### Relationships

- Persists Restore Manifest.
- Depends on ADR 0004 action-scoped layout.
- Depends on ADR 0005 write-ahead Restore Manifest.
- Uses the temp-replace write pattern accepted in ADR 0006.

#### Code implications

- Use `RestoreManifestFileStore` and `RestoreManifestFileWriteResult`.
- Keep filesystem write API use allowlisted to this component and existing user-selected CSV exports.
- Validate that `ManifestPath` stays inside `ActionRootPath`.
- Validate that the filename is `restore-manifest.json`.
- Do not create action item folders or move scanned files from this component.

### Quarantine Executor

Status: draft
Last reviewed: 2026-05-29

#### Definition

Quarantine Executor is the narrow core component that moves Restore Manifest entries from their original paths to action-scoped quarantine paths.

It is fixture-tested and wired to the WPF app for fixture Cleanup Scopes only.

#### Examples

- Move a fixture file from `...\Downloads\old-installer.msi` to `...\actions\quarantine-action-...\items\Downloads\old-installer.msi`.
- Record a destination collision as a failed Restore Manifest entry without overwriting the destination.
- Stop before any move when the planned Restore Manifest cannot be written.

#### Non-examples

- Quarantine Preview.
- Quarantine Action Draft.
- Restore Manifest File Store.
- Undo Quarantine.
- Permanent deletion.
- WPF execution wiring.

#### Lifecycle

- Receives a planned Restore Manifest.
- Writes the planned Restore Manifest before the first move.
- Writes each entry as Moving before a move attempt.
- Revalidates source existence, destination availability, and reparse-point status before moving.
- Moves the file or folder when checks pass.
- Writes each entry as Moved or Failed after the move attempt.
- Returns a Quarantine Execution Result summarizing moved entries, failed entries, blockers, and recovery-review need.

#### Relationships

- Depends on Restore Manifest and Restore Manifest File Store.
- Uses the action-scoped layout from Quarantine Action Draft.
- Implements the fixture-first boundary accepted in ADR 0007.
- Pairs with fixture-first Undo Quarantine Executor.
- Precedes real-profile WPF execution wiring.

#### Code implications

- Use `QuarantineExecutor`, `QuarantineExecutionResult`, and `QuarantineExecutionEntryResult`.
- Keep filesystem move APIs allowlisted only in this component.
- Keep WPF `Execute quarantine` disabled for real-profile and custom non-fixture Cleanup Scopes.
- Do not overwrite existing destination paths.
- Do not implement rollback or Undo Quarantine inside the executor.

### Undo Quarantine Executor

Status: draft
Last reviewed: 2026-05-29

#### Definition

Undo Quarantine Executor is the narrow core component that restores Restore Manifest entries recorded as Moved from their action-scoped quarantine paths back to their original paths.

It is fixture-tested and is used by the WPF app for current-fixture undo and fixture-only selected restore. Real-profile WPF Undo Quarantine and all-manifest restore for discovered Restore Manifests remain unavailable.

#### Examples

- Move a fixture file from `...\actions\quarantine-action-...\items\Downloads\old-installer.msi` back to `...\Downloads\old-installer.msi`.
- Refuse to restore when the original path already exists, preserving the quarantined file and the new original file.
- Restore one entry while recording Restore failed for another entry that could not be restored.
- Stop before later restore attempts when the Restore Manifest cannot be written.

#### Non-examples

- Real-profile WPF Undo Quarantine.
- All-manifest WPF Undo Quarantine for discovered manifests.
- Quarantine Executor rollback.
- Permanent deletion.
- Cleaning up empty quarantine action folders.
- Reconstructing files that no longer exist in quarantine.

#### Lifecycle

- Receives an executed Restore Manifest with at least one Moved entry.
- Restores only entries recorded as Moved.
- Writes each entry as Restoring before a restore attempt.
- Revalidates quarantine path existence, original path availability, and reparse-point status before restoring.
- Creates only the original parent folder when needed.
- Moves the file or folder back to its original path when checks pass.
- Writes each entry as Restored or Restore failed after the restore attempt.
- Returns an Undo Quarantine Result summarizing restored entries, failed restore entries, blockers, and recovery-review need.

#### Relationships

- Depends on Restore Manifest and Restore Manifest File Store.
- Reverses entries moved by Quarantine Executor.
- Implements the fixture-first boundary accepted in ADR 0008.
- Supports WPF Current Fixture Undo Quarantine and Fixture-only Selected Restore Execution.
- Precedes real-profile WPF Undo Quarantine and any all-manifest restore workflow for discovered Restore Manifests.

#### Code implications

- Use `UndoQuarantineExecutor`, `UndoQuarantineResult`, and `UndoQuarantineEntryResult`.
- Keep filesystem move-back APIs allowlisted only in this component.
- Keep real-profile WPF Undo Quarantine and all-manifest restore for discovered Restore Manifests unavailable until separate design packets.
- Do not overwrite existing original paths.
- Do not permanently delete quarantined items or cleanup action folders from this component.

### Quarantine Confirmation Draft

Status: draft
Last reviewed: 2026-05-29

#### Definition

A Quarantine Confirmation Draft is an in-memory readiness check that compares a Quarantine Preview with a Restore Manifest Draft before WPF Quarantine execution.

It lists data blockers, records the exact preview counts and bytes to review, exposes the confirmation phrase, and records whether WPF execution is available for the current Cleanup Scope.

#### Examples

- Show that a Quarantine Preview has 1 included row, 0 blocked rows, 0 redundant rows, and matching Restore Manifest Draft metadata.
- Block confirmation when the preview still contains blocked or redundant rows.
- Block confirmation when the Restore Manifest Draft does not match the preview Cleanup Scope, Quarantine root, entry count, destination paths, or bytes.
- Show fixture-only execution availability or preview-only real-profile/custom status as Quarantine Execution Scope Status.

#### Non-examples

- A Cleanup Action.
- A user approval.
- A persisted cleanup job.
- An executed Restore Manifest.
- A file-moving command.

#### Lifecycle

- Generated in memory after a Quarantine Preview and Restore Manifest Draft exist.
- Used to identify unresolved data blockers before WPF execution.
- Discarded when the scan, Review Shortlist, Quarantine Preview, or Restore Manifest Draft changes.
- Does not move files or write manifests itself.

#### Relationships

- Depends on Quarantine Preview.
- Depends on Restore Manifest Draft.
- Precedes explicit approval or Quarantine Cleanup Action.
- Supports Undo Quarantine design by checking that preview and draft metadata agree before execution.

#### Code implications

- Use `QuarantineConfirmationDraft` and `QuarantineConfirmationDraftBuilder`.
- Use `HasDataBlockers` only as readiness evidence, not as permission to execute.
- Keep `IsExecutionImplemented` true only for recognized fixture Cleanup Scopes in the current build.
- Surface `IsExecutionImplemented` as plain-language Quarantine Execution Scope Status in WPF.
- Do not create folders, move files, delete files, write manifests, or persist cleanup jobs from confirmation draft code.

### Quarantine Execution Gate

Status: draft
Last reviewed: 2026-05-29

#### Definition

Quarantine Execution Gate is the decision that combines Quarantine Confirmation Draft blockers, exact confirmation text, and implementation availability before WPF Quarantine execution can run.

In the current build the gate can open only for recognized fixture Cleanup Scopes. It remains closed for real-profile and custom non-fixture Cleanup Scopes.

#### Examples

- Before Quarantine Preview exists, show that preview must be created first.
- After a clean Quarantine Confirmation Draft exists, require the exact text `QUARANTINE`.
- After `QUARANTINE` is typed for a clean fixture preview, allow fixture-only WPF execution.
- After `QUARANTINE` is typed for a real-profile or custom non-fixture scope, keep the gate closed.
- Carry forward blocked preview row or manifest mismatch blockers from Quarantine Confirmation Draft.
- Show Quarantine Execution Scope Status before `Can execute` so the scope boundary is plain even when exact confirmation text matches.
- Show disabled-state tooltips and automation help text on confirmation and execution controls so fixture-only and real-profile/custom blockers remain visible before gates open.

#### Non-examples

- A Cleanup Action.
- The file-moving command itself.
- A Restore Manifest write.
- Approval to bypass preview blockers.

#### Lifecycle

- Generated when the WPF app starts with a blocker saying Quarantine Preview is required first.
- Regenerated after Quarantine Preview and Quarantine Confirmation Draft are created.
- Updates when the confirmation text changes.
- Resets when scan results, Review Shortlist, Quarantine Preview, Restore Manifest Draft, or Quarantine Root Selection change.

#### Relationships

- Depends on Quarantine Confirmation Draft.
- Precedes Quarantine execution button behavior.
- Preserves explicit confirmation semantics separately from preview and manifest readiness.

#### Code implications

- Use `QuarantineExecutionGate` and `QuarantineExecutionGateBuilder`.
- `CanExecute` must require no blockers, exact confirmation text, and implemented execution support.
- Keep `CanExecute` false for real-profile and custom non-fixture Cleanup Scopes.
- Keep Quarantine Execution Scope Status visible in the gate readout.
- In WPF, keep verbose gate details height-constrained when shown in the Quarantine shortlist area so the main review grid remains usable.
- In WPF, keep the Quarantine shortlist area collapsible so the user can recover grid height after reviewing the gate.
- In WPF, keep the collapsed Quarantine shortlist header useful with shortlist, preview, current quarantined, and undo state.
- Keep confirmation and execution control tooltips and automation help text aligned with fixture-only execution and real-profile/custom blockers.
- Do not create folders, move files, delete files, write manifests, or persist cleanup jobs from gate builder code.

### Quarantine Execution Scope Status

Status: draft
Last reviewed: 2026-05-29

#### Definition

Quarantine Execution Scope Status is read-only WPF wording that explains whether the current Cleanup Scope has fixture-only execution available or is preview-only.

It is shown in Quarantine Preview and Quarantine Execution Gate output so real-profile and custom Cleanup Scopes stay visibly blocked even if the preview is clean and the confirmation text matches.

#### Examples

- Fixture Cleanup Scope: state that fixture-only execution is available only after preview readiness and exact `QUARANTINE` confirmation.
- Real-profile or custom Cleanup Scope: state that the workflow is preview-only and real-profile/custom execution remains unavailable.
- Disabled Quarantine confirmation/execution controls repeat the same fixture-only and real/custom boundary in tooltips and automation help text.

#### Non-examples

- Cleanup approval.
- A Cleanup Action.
- A replacement for Quarantine Preview blockers.
- A reason to enable real-profile execution.

#### Lifecycle

- Generated from the current execution-availability flag when Quarantine Preview or Quarantine Execution Gate text is formatted.
- Updates when a new preview is created for a different Cleanup Scope.
- Does not persist state or modify files.

#### Relationships

- Depends on Quarantine Confirmation Draft and Quarantine Execution Gate implementation availability.
- Supports fixture-only WPF Quarantine Execution by making the scope boundary explicit.
- Reinforces the rule that real-profile WPF Quarantine execution remains unavailable.

#### Code implications

- Use `FormatQuarantineExecutionScopeStatus` for WPF output.
- Keep wording explicit about `fixture-only`, `preview only`, `real-profile`, and `custom` scopes.
- Keep disabled-control tooltips and automation help text consistent with the visible scope-status wording.
- Do not use this status as permission to move files.

### Fixture-only WPF Quarantine Execution

Status: draft
Last reviewed: 2026-05-29

#### Definition

Fixture-only WPF Quarantine Execution is the visible-app cleanup execution path that is available only when the scanned Cleanup Scope is a recognized synthetic fixture.

It lets the WPF app call the core Quarantine Executor after Quarantine Preview readiness and exact `QUARANTINE` confirmation, while keeping real-profile cleanup execution unavailable.

#### Examples

- Running the WPF app against `...\test-fixtures\app\...`, shortlisting an eligible synthetic installer, previewing quarantine, typing `QUARANTINE`, then moving that synthetic file into the action-scoped quarantine items folder.
- Shortlisting multiple eligible synthetic rows, previewing quarantine once, typing `QUARANTINE` once, then moving all included Review Shortlist rows together.
- Writing `restore-manifest.json` under the selected action root for a fixture execution.
- Showing that the current scan and review rows are stale after execution.

#### Non-examples

- Real-profile WPF Quarantine execution.
- WPF Undo Quarantine, except current-fixture undo.
- Permanent deletion.
- Cleanup history.
- Quarantine folder cleanup.

#### Lifecycle

- Requires a completed fixture Storage Scan.
- Requires Review Shortlist and Quarantine Preview.
- Requires a Quarantine Confirmation Draft with no data blockers.
- Requires the exact confirmation text `QUARANTINE`.
- Calls `QuarantineExecutor.Execute` with the planned Restore Manifest.
- Executes all included rows in the current Quarantine Action Draft together.
- Displays execution results and recovery-review need.
- Clears stale Review Shortlist state, points the user to `Undo fixture quarantine`, and explains that rescan refreshes review rows.

#### Relationships

- Depends on Quarantine Execution Gate.
- Depends on Quarantine Executor and Restore Manifest File Store.
- Implements ADR 0009.
- Precedes any real-profile WPF Quarantine execution.

#### Code implications

- Use `CleanupScopeSafetyNoteBuilder` to distinguish fixture scopes from real/custom scopes.
- Use `QuarantineExecutor.Execute`; do not implement file movement in WPF code.
- Keep real-profile and custom non-fixture execution blocked in WPF.
- Label the WPF action around included Review Shortlist rows so it is not mistaken for selected-row execution.
- After execution, disable re-execution for the current preview and mark scan state stale.
- Preserve current-fixture undo state if a post-execution rescan clears stale preview state before undo is attempted.

### WPF Current Fixture Undo Quarantine

Status: draft
Last reviewed: 2026-05-29

#### Definition

WPF Current Fixture Undo Quarantine is the visible-app undo path that restores the current fixture-only Quarantine execution from its in-memory Restore Manifest.

It is not a manifest discovery or history workflow.

#### Examples

- After fixture-only WPF execution moves `...\Downloads\old-installer.msi` into action-scoped quarantine, clicking `Undo fixture quarantine` restores that same synthetic file to its original path.
- Showing Undo result rows and recovery-review state in the same review pane.
- Updating the Restore Manifest status from Completed to Restored.

#### Non-examples

- Real-profile WPF Undo Quarantine.
- Restoring an older Restore Manifest from disk.
- Permanent deletion.
- Quarantine folder cleanup.
- Re-scanning or refreshing the filesystem snapshot.

#### Lifecycle

- Available only after current fixture-only WPF Quarantine execution records Moved entries.
- Calls `UndoQuarantineExecutor.Undo` with the current in-memory Restore Manifest.
- Displays restored and failed restore counts.
- Disables repeat undo for the current execution attempt.
- Keeps stale-state wording visible and tells the user to rescan before further review.
- Remains available after a post-execution rescan until undo is attempted, because the moved source row can disappear from refreshed Storage Scan rows.

#### Relationships

- Depends on Fixture-only WPF Quarantine Execution.
- Depends on Undo Quarantine Executor.
- Implements ADR 0010.
- Precedes manifest discovery/history and real-profile WPF Undo Quarantine.

#### Code implications

- Use `UndoQuarantineForCurrentExecution` for the WPF current-execution undo action.
- Use `UndoQuarantineExecutor.Undo`; do not implement restore movement in WPF code.
- Keep undo unavailable before execution, after an undo attempt, and for real-profile/custom non-fixture scopes.
- Do not tie current-fixture undo availability to the moved source row remaining visible after rescan.
- Do not clean up quarantine folders in this action.

### Current-Session Quarantined Review

Status: draft
Last reviewed: 2026-05-29

#### Definition

Current-Session Quarantined Review is a read-only WPF grid view that shows entries still in `Moved` state from the current in-memory Restore Manifest.

It is reached with the `Quarantined` button and returns to Storage Scan rows with `Back to scan rows`.

#### Examples

- After fixture-only WPF Quarantine execution moves two included Review Shortlist rows, `Quarantined` shows both original paths, quarantine paths, sizes, and manifest path.
- After a post-execution rescan removes the moved source rows from Storage Scan results, `Quarantined` still shows the current-session moved entries while current-fixture undo is available.
- After `Undo fixture quarantine`, the view becomes empty because no current-session entries remain in `Moved` state.

#### Non-examples

- Quarantine Manifest Discovery.
- Persisted cleanup history.
- Real-profile WPF Undo Quarantine.
- Restore approval or restore execution.
- A reason to keep moved source rows visible in refreshed Storage Scan results.

#### Lifecycle

- Available only when the current in-memory Restore Manifest has `Moved` entries.
- Uses current-session fixture execution state; it does not scan discovered Restore Manifests from disk.
- Remains available after a post-execution rescan while current-fixture undo remains available.
- Shows only entries still recorded as `Moved`.
- Becomes empty or unavailable after current-fixture undo restores those entries.
- Does not create, move, delete, restore, write, or clean up files or folders.

#### Relationships

- Depends on Restore Manifest and Restore Manifest Entry Status.
- Complements WPF Current Fixture Undo Quarantine by keeping moved entries visible even when Storage Scan rows refresh.
- Older or discovered manifests remain under Quarantine Manifest Discovery, Selected Restore Manifest Review, and Restore Readiness Preview.

#### Code implications

- Use `QuarantinedItemRow` for WPF grid rows.
- Use `ShowQuarantinedButton` and `BackToScanRowsButton` for the view switch.
- Populate the view from current Restore Manifest entries with `RestoreManifestEntryStatus.Moved`.
- Keep this view read-only and current-session-only until a separate discovered-manifest design exists.

### Quarantine Manifest Discovery

Status: draft
Last reviewed: 2026-05-29

#### Definition

Quarantine Manifest Discovery is the read-only workflow that finds action-scoped Restore Manifests under the selected Quarantine Root.

It is discovery and status review only, not Undo Quarantine execution and not persisted cleanup history.

#### Examples

- Looking under `<quarantine-root>\actions\*\restore-manifest.json`.
- Showing that one discovered Restore Manifest is Completed and has Moved entries.
- Reporting an invalid or unreadable manifest file as a discovery issue.
- Showing that no all-manifest restore action is available from discovery and that fixture selected restore must go through selected manifest readiness and the selected restore gate.

#### Non-examples

- Restoring a discovered Restore Manifest.
- Real-profile WPF Undo Quarantine.
- Cleanup history.
- Quarantine folder cleanup.
- Scanning arbitrary files under the Quarantine Root.

#### Lifecycle

- Uses the current Quarantine Root Selection.
- Reads direct action-scoped `restore-manifest.json` files under the `actions` folder.
- Deserializes supported Restore Manifest JSON.
- Produces Restore Manifest Summary rows and discovery issues.
- Does not create, move, delete, restore, or clean up files or folders.

#### Relationships

- Depends on Quarantine Root Selection.
- Depends on Restore Manifest and the action-scoped layout from ADR 0004.
- Implements ADR 0011.
- Feeds Selected Restore Manifest Review.
- Precedes broad WPF Undo Quarantine for discovered manifests.

#### Code implications

- Use `QuarantineManifestDiscovery`, `QuarantineManifestDiscoveryBuilder`, and `QuarantineManifestDiscoveryIssue`.
- Keep discovery limited to `<quarantine-root>\actions\*\restore-manifest.json`.
- Keep discovery itself status-only; Selected Restore Manifest Review and selected manifest readiness are separate read-only workflows until a restore execution design exists.
- WPF discovery output should distinguish discovery from all-manifest restore execution while pointing fixture-only restore toward the selected restore gate.
- WPF discovery control tooltip and automation help text should state that discovery is read-only and does not restore, move, delete, clean up folders, or create cleanup history.
- Do not call this cleanup history.

### Restore Manifest Summary

Status: draft
Last reviewed: 2026-05-29

#### Definition

Restore Manifest Summary is a compact read-only status view of one discovered Restore Manifest.

#### Examples

- Manifest path.
- Action id.
- Action status.
- Entry count and total size.
- Moved/restored/failed counts.
- Recovery-review flag.

#### Non-examples

- The full Restore Manifest JSON.
- Approval to restore.
- Cleanup history.

#### Lifecycle

- Created from a valid discovered Restore Manifest.
- Shown in Quarantine Manifest Discovery.
- Discarded when the Quarantine Root changes or discovery is rerun.

#### Relationships

- Belongs to Quarantine Manifest Discovery.
- Summarizes Restore Manifest.

#### Code implications

- Use `RestoreManifestSummary`.
- Keep it read-only and derived from manifest metadata.

### Selected Restore Manifest Review

Status: draft
Last reviewed: 2026-05-29

#### Definition

Selected Restore Manifest Review is the read-only workflow that focuses on one discovered Restore Manifest and previews readiness for that manifest only.

It is not approval to restore and does not restore files.

#### Examples

- Selecting one discovered Restore Manifest after `Discover manifests`.
- Showing the selected manifest path and readiness rows for that manifest.
- Reporting that a selected path is no longer part of the current Quarantine Manifest Discovery result.

#### Non-examples

- Broad WPF Undo Quarantine execution.
- Real-profile WPF Undo Quarantine.
- Approval to restore.
- Writing an updated Restore Manifest.
- Moving files out of quarantine.
- Cleaning up empty action folders.

#### Lifecycle

- Uses the current Quarantine Manifest Discovery result.
- Populates selection from discovered Restore Manifest Summary rows.
- Recomputes Restore Readiness Preview for the selected Restore Manifest when the user requests selected manifest readiness.
- Is discarded when the Quarantine Root changes, discovery is rerun, or another manifest is selected.
- Does not create, move, delete, restore, write, or clean up files or folders.

#### Relationships

- Depends on Quarantine Manifest Discovery.
- Depends on Restore Manifest Summary and Restore Readiness Preview.
- Implements ADR 0013.
- Feeds Selected Restore Confirmation Draft.
- Precedes broad WPF Undo Quarantine for discovered manifests.

#### Code implications

- Use `SelectedRestoreManifestReview` and `SelectedRestoreManifestReviewBuilder`.
- Keep selection read-only and derived from current discovery results.
- Keep selected manifest readiness separate from approval and execution.
- Keep the WPF selection control tooltip and automation help text explicit that selection focuses one discovered Restore Manifest for read-only review and does not move, restore, delete, write manifests, or clean up folders.
- Keep the WPF selected manifest readiness tooltip and automation help text explicit that the action is selected-only and not restore approval.
- Do not call `UndoQuarantineExecutor.Undo`.

### Selected Restore Confirmation Draft

Status: draft
Last reviewed: 2026-05-29

#### Definition

Selected Restore Confirmation Draft is the read-only confirmation readiness check for one Selected Restore Manifest Review.

It summarizes the selected Restore Manifest, restorable entries, restorable bytes, required confirmation text, and readiness blockers before any selected restore execution.

#### Examples

- Showing required confirmation text `RESTORE` for one selected Restore Manifest.
- Showing that one selected manifest has one restorable entry and no readiness blockers.
- Showing that selected restore confirmation is blocked by original-path collisions or recovery-review rows.

#### Non-examples

- Approval to restore.
- Broad WPF Undo Quarantine execution.
- Real-profile WPF Undo Quarantine.
- Writing an updated Restore Manifest.
- Moving files out of quarantine.
- Cleaning up empty action folders.

#### Lifecycle

- Created after Selected Restore Manifest Review has readiness output.
- Is discarded when discovery is rerun, a different Restore Manifest is selected, selected manifest readiness changes, or the Quarantine Root changes.
- Feeds Selected Restore Execution Gate.
- Does not create, move, delete, restore, write, or clean up files or folders.

#### Relationships

- Depends on Selected Restore Manifest Review.
- Depends on Restore Readiness Preview.
- Implements ADR 0014.
- Precedes selected restore execution.

#### Code implications

- Use `SelectedRestoreConfirmationDraft` and `SelectedRestoreConfirmationDraftBuilder`.
- Required confirmation text is `RESTORE`.
- Keep readiness blockers separate from execution availability.
- Do not call `UndoQuarantineExecutor.Undo`.

### Selected Restore Execution Gate

Status: draft
Last reviewed: 2026-05-29

#### Definition

Selected Restore Execution Gate is the gate that combines a Selected Restore Confirmation Draft, typed confirmation text, and selected restore execution availability.

In the current WPF app, selected restore execution availability can be true only for selected discovered Restore Manifests whose Cleanup Scope is a recognized fixture Cleanup Scope.

#### Examples

- Showing `Entered confirmation matches: yes` after the user types `RESTORE`.
- Showing `Execution implemented: yes` and `Can execute: yes` for a selected fixture Restore Manifest after exact `RESTORE` confirmation.
- Showing `Execution implemented: no` and `Can execute: no` for real-profile or custom non-fixture selected Restore Manifests.
- Showing an execution scope status that distinguishes fixture-only selected restore from preview-only real-profile/custom selected restore.
- Showing an approval boundary that says selected manifest readiness is not restore approval.
- Reporting confirmation-readiness blockers before any future restore action can open.

#### Non-examples

- Approval to restore.
- Real-profile WPF Undo Quarantine execution.
- A persisted cleanup job.

#### Lifecycle

- Created after Selected Restore Confirmation Draft.
- Refreshes when the selected restore confirmation text changes.
- Is discarded when selected manifest readiness, selected manifest, discovery, or Quarantine Root changes.
- Does not create, move, delete, restore, write, or clean up files or folders by itself.

#### Relationships

- Depends on Selected Restore Confirmation Draft.
- Implements ADR 0014.
- Precedes any future selected restore execution.

#### Code implications

- Use `SelectedRestoreExecutionGate` and `SelectedRestoreExecutionGateBuilder`.
- Keep `CanExecute` false unless the exact `RESTORE` text matches, selected restore execution is implemented, and blockers are clear.
- WPF must keep selected restore execution unavailable for real-profile and custom non-fixture manifests.
- WPF should show `Execution scope status` and `Approval boundary` lines so users do not have to infer fixture-only versus preview-only behavior from `Execution implemented`.

### Fixture-only Selected Restore Execution

Status: draft
Last reviewed: 2026-05-29

#### Definition

Fixture-only Selected Restore Execution is the visible WPF restore path that restores a selected discovered Restore Manifest only when that manifest belongs to a recognized fixture Cleanup Scope.

It is the fixture proof for selected discovered Restore Manifest restore, not real-profile Undo Quarantine.

#### Examples

- Discovering a fixture Restore Manifest in a new WPF window, selecting it, previewing selected manifest readiness, typing `RESTORE`, and restoring the synthetic file from quarantine.
- Showing selected restore result rows and stale-state guidance after the restore attempt.
- Blocking the same workflow for a custom non-fixture Cleanup Scope even when `RESTORE` is typed.

#### Non-examples

- Real-profile WPF Undo Quarantine.
- Custom non-fixture selected restore execution.
- Permanent deletion.
- Cleanup history.
- Quarantine folder cleanup.

#### Lifecycle

- Requires a current Quarantine Manifest Discovery result.
- Requires Selected Restore Manifest Review with readiness output.
- Requires Selected Restore Confirmation Draft with no data blockers.
- Requires Selected Restore Execution Gate to be open after exact `RESTORE`.
- Calls `UndoQuarantineExecutor.Undo` for the selected Restore Manifest.
- Disables repeat selected restore execution for the current selected review after an attempt.
- Tells the user to rediscover manifests and rescan before further review.

#### Relationships

- Depends on Selected Restore Execution Gate.
- Depends on Undo Quarantine Executor.
- Implements ADR 0015.
- Precedes real-profile selected restore execution.

#### Code implications

- Use `ExecuteSelectedRestoreForCurrentSelection` for the WPF action.
- Use `UndoQuarantineExecutor.Undo`; do not implement restore movement in WPF code.
- Use `CleanupScopeSafetyNoteBuilder.IsFixtureScope` to restrict visible selected restore execution to fixture Cleanup Scopes.
- Keep real-profile and custom non-fixture selected restore execution unavailable.
- Do not clean up quarantine folders in this action.

### Restore Readiness Preview

Status: draft
Last reviewed: 2026-05-29

#### Definition

Restore Readiness Preview is the read-only workflow that evaluates whether discovered Restore Manifests under the selected Quarantine Root appear ready for a future Undo Quarantine action.

It is not approval to restore and does not restore files.

#### Examples

- Show that a Moved entry has an existing quarantine path and no original-path collision.
- Show that a restore would be blocked because the original path already exists.
- Show that a restore would be blocked because the quarantine path is missing.
- Show that a Restored entry has no restore work left.
- Show that no all-manifest restore action is available from readiness preview and that fixture selected restore must go through selected manifest readiness and the selected restore gate.

#### Non-examples

- Broad WPF Undo Quarantine execution.
- Approval to restore or selected restore execution.
- Writing an updated Restore Manifest.
- Creating original parent folders.
- Moving files out of quarantine.
- Cleaning up empty action folders.

#### Lifecycle

- Uses Quarantine Manifest Discovery for the current Quarantine Root Selection.
- Reads valid action-scoped Restore Manifests.
- Checks entry state and path readiness.
- Produces manifest-level and entry-level readiness output.
- Is discarded when the Quarantine Root changes or preview is rerun.
- Does not create, move, delete, restore, write, or clean up files or folders.

#### Relationships

- Depends on Quarantine Manifest Discovery.
- Depends on Restore Manifest.
- Precedes broad WPF Undo Quarantine.
- Complements Undo Quarantine Executor by previewing likely blockers before execution exists.

#### Code implications

- Use `RestoreReadinessPreview`, `RestoreReadinessPreviewBuilder`, `RestoreReadinessManifestPreview`, and `RestoreReadinessEntryPreview`.
- Keep it read-only. Do not call `UndoQuarantineExecutor.Undo`.
- The WPF action label should make the all-manifest readiness scope visible so it is distinct from selected manifest readiness.
- The WPF action tooltip and automation help text should repeat the read-only all-manifest scope and no-restore boundary.
- WPF readiness output should distinguish read-only blocker evidence from all-manifest restore execution while pointing fixture-only restore toward the selected restore gate.
- Recompute readiness immediately before any future restore execution because filesystem state can change.

### Restore Readiness Entry

Status: draft
Last reviewed: 2026-05-29

#### Definition

Restore Readiness Entry is a read-only preview row for one Restore Manifest entry.

#### Examples

- Restorable.
- Blocked by original-path collision.
- Blocked by missing quarantine path.
- Already restored.
- Needs recovery review.
- Not moved.

#### Non-examples

- An Undo Quarantine Entry Result.
- Proof that a future restore will succeed.
- Approval to restore.

#### Lifecycle

- Created during Restore Readiness Preview.
- Discarded with the preview.
- Does not modify the filesystem or manifest.

#### Relationships

- Belongs to Restore Readiness Preview.
- Summarizes one Restore Manifest entry.

#### Code implications

- Use `RestoreReadinessEntryPreview` and `RestoreReadinessDisposition`.
- Keep blocker text explicit and path-specific.

### Quarantine Action Draft

Status: draft
Last reviewed: 2026-05-29

#### Definition

Quarantine Action Draft is the in-memory, read-only layout for a future Quarantine Cleanup Action.

It maps included preview rows to action-scoped quarantine item paths and a Restore Manifest path without creating folders, moving files, or writing a manifest.

#### Examples

- Action items root: `D:\WindowsFileCleanerQuarantine\actions\quarantine-action-20260529_030405\items`
- Restore manifest path: `D:\WindowsFileCleanerQuarantine\actions\quarantine-action-20260529_030405\restore-manifest.json`
- Included item path: `...\items\Downloads\old-installer.msi`

#### Non-examples

- A Cleanup Action.
- An executed Restore Manifest.
- A folder creation command.
- Proof that files were moved.

#### Lifecycle

- Generated after Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft have no data blockers.
- Discarded when scan results, Review Shortlist, Quarantine Preview, Restore Manifest Draft, Quarantine Confirmation Draft, or Quarantine Root Selection change.
- Remains read-only; fixture execution uses the draft to build a planned Restore Manifest.

#### Relationships

- Depends on Restore Manifest Draft and Quarantine Confirmation Draft.
- Uses the action-scoped layout accepted in ADR 0004.
- Feeds the planned Restore Manifest model accepted in ADR 0005.
- Feeds the visible Quarantine Execution Gate readout.
- Precedes fixture execution and Undo Quarantine.

#### Code implications

- Use `QuarantineActionDraft`, `QuarantineActionEntryDraft`, and `QuarantineActionDraftBuilder`.
- Use path-safe action ids only.
- Map future item destinations under `<quarantine-root>\actions\<action-id>\items\`.
- Map the future Restore Manifest path to `<quarantine-root>\actions\<action-id>\restore-manifest.json`.
- Do not create folders, move files, delete files, write manifests, or persist cleanup jobs from action draft code.

### Undo Quarantine

Status: draft  
Last reviewed: 2026-05-29

#### Definition

Undo Quarantine restores quarantined files or folders to their original locations when feasible.

The current core implementation is fixture-tested through Undo Quarantine Executor. WPF exposes current-fixture undo after fixture-only execution and fixture-only selected restore execution for discovered fixture Restore Manifests.

#### Examples

- Restore a quarantined app cache because an app stopped working.
- Restore a quarantined folder to inspect it again.

#### Non-examples

- Restoring permanently deleted files.
- Reconstructing files that were changed outside the app.

#### Lifecycle

- Available after a Quarantine action records Moved entries in a Restore Manifest.
- Uses the Restore Manifest to return files to original paths.
- Must refuse to overwrite an original path that now exists.
- Must preserve recovery evidence when restore or manifest writes fail.
- Broad WPF Undo Quarantine that restores real-profile discovered manifests remains a future workflow.

#### Relationships

- Depends on Quarantine.
- Depends on Restore Manifest.
- Depends on Undo Quarantine Executor for the fixture-tested core restore behavior.
- Has a current-fixture WPF path for the immediately executed fixture action.
- Has read-only Quarantine Manifest Discovery for older action-scoped manifests.
- Has read-only Restore Readiness Preview for discovered manifest blockers.
- Has read-only Selected Restore Manifest Review for focusing one discovered manifest before any all-manifest restore action exists.
- Has read-only Selected Restore Confirmation Draft and Selected Restore Execution Gate for exact-confirmation semantics before any selected restore action exists.
- Has Fixture-only Selected Restore Execution for selected discovered fixture manifests.
- Shows disabled-control tooltip and automation help text wording so selected restore controls do not imply real-profile/custom selected restore is available.
- Reverses a quarantine Cleanup Action.

#### Code implications

- Use `UndoQuarantine` for the workflow concept.
- Use a manifest format that records original path, quarantine path, size, timestamps, and action time.
- Use `UndoQuarantineExecutor` only for the core restore component.

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
- WPF scan cancellation tooltip and automation help text should state that cancellation only requests stopping the in-progress read-only Storage Scan and does not move, delete, quarantine, restore, or approve cleanup.

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
- Future Quarantine execution should write a planned Restore Manifest before the first file or folder move, then update entry statuses before and after each move attempt.
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
