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

Initial candidate categories include profile containers, AppData areas, browser data, old downloads, temporary folders, installer caches, app caches, GPU shader caches, duplicate files, old game files, Node or Python package caches, and Windows app leftovers.

#### Examples

- Old downloads.
- Profile containers.
- AppData areas.
- Browser data.
- Temporary folders.
- Installer caches.
- App caches.
- GPU shader caches.
- Duplicate files.
- Old game files.
- Node package caches.
- Python package caches.
- Windows app leftovers.

#### Non-examples

- "Large file" by itself.
- "Unknown folder" by itself.
- Any category that implies deletion is safe without evidence.
- A container category by itself; containers should usually be inspected through their children.

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

### Storage Review Filter

Status: draft  
Last reviewed: 2026-05-28

#### Definition

A Storage Review Filter narrows Storage Scan results so the user can inspect a focused set of paths.

Initial filters are All, Likely safe, Caution, High risk, and Quarantine candidates.

#### Examples

- Filter to High risk rows to review protected paths.
- Filter to Quarantine candidates to inspect likely cleanup opportunities.
- Filter to Caution rows to review cache and package-cache areas.

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
- Helps review Cleanup Candidates.

#### Code implications

- Use `StorageReviewFilter` for filter values.
- Keep filters read-only.
- Show counts so the user can understand scan composition.

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
- Documents, Desktop, Pictures, Videos, Music.
- Browser profiles and credentials.
- Application settings under `AppData`.
- Source code folders.
- Game saves.

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

The preferred quarantine location is on `D:`. A likely default folder name is `D:\WindowsFileCleanerQuarantine`, but the exact path is still open.

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

- What exact `D:` quarantine path should be used?
- Which locations should be protected from cleanup but still shown in the report?
