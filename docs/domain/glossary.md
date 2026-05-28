# Glossary

---
last_reviewed: 2026-05-28
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
| Storage Scan | The first read-only workflow for recursively scanning the Cleanup Scope. | `StorageScan`, `storageScanId` | Must not modify files. |
| Cleanup Candidate | A file or folder proposed for review as potentially removable. | `CleanupCandidate`, `cleanupCandidateId` | Candidate does not mean safe to delete. |
| Cleanup Action | A user-approved operation that modifies files or folders. | `CleanupAction`, `cleanupActionId` | Examples include moving to Recycle Bin, quarantine, or deletion. |
| Protected Location | A path or category treated as high-risk or blocked from cleanup. | `ProtectedLocation`, `protectedLocationPath` | Includes profile essentials and sensitive app/user data until exceptions are defined. |
| Storage Savings | Estimated or confirmed recoverable disk space. | `StorageSavings`, `estimatedStorageSavings`, `confirmedStorageSavings` | Use explicit units. |
| Dry Run | A preview of what would happen without modifying files. | `DryRun`, `dryRunResult` | Prefer before Cleanup Actions. |
| Bloat Category | A reason a candidate may be unwanted, removable, or worth conservative review. | `BloatCategory`, `bloatCategory` | Includes profile containers, AppData areas, browser data, old downloads, temp folders, caches, GPU shader caches, duplicate files, old game files, package caches, and Windows app leftovers. |
| Importance Rating | Conservative estimate of how important a file or folder may be. | `ImportanceRating`, `importanceRating` | User-facing values: `Likely safe`, `Caution`, `High risk`. |
| Deletion Recommendation | Suggested action for a candidate after inspection. | `DeletionRecommendation`, `deletionRecommendation` | Examples: keep, inspect, quarantine candidate, delete later. |
| Storage Review Filter | Read-only filter applied to Storage Scan results. | `StorageReviewFilter`, `storageReviewFilter` | Initial filters: All, Likely safe, Caution, High risk, Quarantine candidates. |
| Quarantine | A reversible holding location for selected files or folders before deletion. | `Quarantine`, `quarantinePath` | Preferred on `D:`. Exact path is still open. |
| Undo Quarantine | Restore a quarantined file or folder to its original path. | `UndoQuarantine`, `undoQuarantine` | Requires a restore manifest. |
| Restore Manifest | Metadata needed to undo a quarantine action. | `RestoreManifest`, `restoreManifestPath` | Should include original path, quarantine path, size, timestamps, and action time. |
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
- What exact `D:` quarantine path should be used?
