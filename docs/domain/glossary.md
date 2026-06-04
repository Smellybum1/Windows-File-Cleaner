# Glossary

---
last_reviewed: 2026-06-04
owner: project-owner
stability: compact
---

## Purpose

This compact glossary is the naming authority for read-first work. It keeps preferred terms and code names visible without loading every historical UI note.

Full term notes are preserved in `docs/domain/glossary-reference.md`. Open that reference only when a task needs detailed tooltip/help-text wording, old term history, or a term that is not clear here.

## Preferred Terms

| Preferred term | Code names | Compact meaning |
|---|---|---|
| System Drive | `SystemDrive`, `systemDrivePath` | Windows operating-system partition. Current known value is `C:`. |
| User Profile Root | `UserProfileRoot`, `userProfileRootPath` | Top-level user profile area. Current focus is `C:\Users\moxhe`. |
| Cleanup Scope | `CleanupScope`, `cleanupScopePath` | Path allowed for one scan or cleanup run. |
| Cleanup Scope Root | `CleanupScopeRoot`, `BloatCategory.CleanupScopeRoot` | Top-level path selected for a Storage Scan. Keep/review through children. |
| Cleanup Scope Selection | `ScopePathBox`, `BrowseScopeButton` | Pre-scan typed or browsed Cleanup Scope path. Does not start scan or approve cleanup. |
| Cleanup Scope Safety Note | `CleanupScopeSafetyNote`, `CleanupScopeSafetyNoteBuilder` | Read-only UI context for fixture, real-profile, custom, blank, or invalid scope. |
| Cleanup Scope Scan Gate | `CleanupScopeScanGate`, `CleanupScopeScanGateBuilder` | Read-only gate controlling whether Scan can start for the current scope. |
| Storage Scan | `StorageScan`, `storageScanId` | Read-only recursive scan/review workflow. Must not modify files. |
| Cleanup Candidate | `CleanupCandidate`, `cleanupCandidateId` | Path worth review as potentially removable; not automatically safe. |
| Cleanup Action | `CleanupAction`, `cleanupActionId` | User-approved file-modifying action, such as Quarantine. |
| Protected Location | `ProtectedLocation`, `protectedLocationPath` | High-risk path/category blocked or kept by default. |
| Bloat Category | `BloatCategory` | Human-readable reason a path may be review-worthy. |
| Cloud Sync Data | `BloatCategory.CloudSyncData` | User-owned synced data; protected by default. |
| Credential Data | `BloatCategory.CredentialData` | Keys, tokens, vaults, auth/config secrets; protected by default. |
| Specific Rebuildable Cache Evidence | `HasSpecificRebuildableCacheEvidence` | Strong evidence that a narrow row is a rebuildable cache, not a broad app/profile container. |
| Importance Rating | `ImportanceRating` | User-facing risk: `Likely safe`, `Caution`, `High risk`. |
| Deletion Recommendation | `DeletionRecommendation` | Suggested next action such as keep, inspect, or Quarantine candidate. |
| Storage Review Filter | `StorageReviewFilter` | Read-only filter over completed Storage Scan rows. |
| Storage Review Search | `StorageReviewSearch`, `StorageReviewSearchField` | Read-only search over completed scan results. |
| Access Status | `AccessStatus` | Readable or access issue. Does not imply permission changes. |
| Review Shortlist | `StorageReviewShortlist` | In-memory review set; not cleanup approval. |
| Quarantine Root Selection | `QuarantineRootBox`, `CurrentQuarantineRootPath` | Preview/execution destination root, defaulting to `D:\WindowsFileCleanerQuarantine`. |
| Quarantine Preview | `QuarantinePreview`, `QuarantinePreviewBuilder` | Dry run for Review Shortlist readiness. Does not move/write/delete. |
| Quarantine | `Quarantine`, `quarantinePath` | Reversible holding location before any future deletion. |
| Quarantine Action Draft | `QuarantineActionDraft` | In-memory action layout for future Quarantine execution. |
| Quarantine Confirmation Draft | `QuarantineConfirmationDraft` | In-memory readiness check and exact confirmation requirements. |
| Quarantine Execution Gate | `QuarantineExecutionGate` | Decision combining readiness, exact confirmation, and implementation availability. |
| Quarantine Executor | `QuarantineExecutor` | Core component that moves planned entries into action-scoped Quarantine paths. |
| Restore Manifest | `RestoreManifest` | Versioned JSON recovery metadata for a Quarantine action. |
| Restore Manifest File Store | `RestoreManifestFileStore` | Narrow component that writes action-scoped Restore Manifest JSON. |
| Restore Manifest Draft | `RestoreManifestDraft` | In-memory Restore Manifest model before movement. |
| Restore Manifest Action Status | `RestoreManifestActionStatus` | Manifest-level action or restore state. |
| Restore Manifest Entry Status | `RestoreManifestEntryStatus` | Per-entry movement or restore state. |
| Quarantine Manifest Discovery | `QuarantineManifestDiscovery` | Read-only discovery of action-scoped Restore Manifests. |
| Restore Readiness Preview | `RestoreReadinessPreview` | Read-only preview of blockers across discovered manifests. |
| Selected Restore Manifest Review | `SelectedRestoreManifestReview` | Read-only readiness review for one selected Restore Manifest. |
| Selected Restore Execution Gate | `SelectedRestoreExecutionGate` | Decision for selected restore after exact `RESTORE` and readiness evidence. |
| Undo Quarantine | `UndoQuarantine` | Restore quarantined files/folders using a Restore Manifest. |
| Undo Quarantine Executor | `UndoQuarantineExecutor` | Core restore component used by fixture undo and selected restore. |
| Fixture-only WPF Quarantine Execution | `ExecuteQuarantineForCurrentPreview` | Visible fixture movement path after preview readiness and exact `QUARANTINE`. |
| WPF Current Fixture Undo Quarantine | `UndoQuarantineForCurrentExecution` | Visible undo for the current fixture execution only. |
| Real-Profile Quarantine Readiness Contract | ADR 0017 | Durable prerequisites before real-profile Quarantine can move files. |
| Real-Profile Quarantine Execution Readiness | `QuarantineExecutionReadiness` | Composite readiness model for exact first-phase real-profile movement. |
| Real-Profile Quarantine Approval Evidence | `RealProfileQuarantineApprovalEvidence` | Evidence that exact `QUARANTINE` is necessary but not sufficient. |
| Quarantine Root Execution Safety | `QuarantineRootExecutionSafety` | Execution-specific validation of Quarantine root and destination layout. |
| Pre-Execution Revalidation | `PreExecutionRevalidation` | Immediate filesystem recheck before approved real-profile movement. |
| Real-Profile Restore Readiness | `RealProfileRestoreReadiness` | Recovery prerequisite for real-profile Quarantine movement. |
| Real-Profile Selected Restore Execution | `ExecuteSelectedRestoreForCurrentSelection` | Exact `C:\Users\moxhe`, one selected Restore Manifest, exact `RESTORE`, immediate revalidation. |
| Current-Session Quarantined Review | `QuarantinedItemRow`, `ShowQuarantinedButton` | Read-only grid view of current in-memory moved entries. |
| Restore Manifest Summary | `Summarize-RestoreManifests` | Terminal-only read-only Restore Manifest evidence command. |
| Fixture Acceptance Notes | `Start-MvpFixtureReview`, `Record-FixtureAcceptanceNotes` | Ignored local markdown evidence for manual fixture review. |
| Portable Release Package | `Publish-LocalRelease`, `Test-LocalRelease`, `Start-LocalRelease` | Local self-contained WPF app package, not installer or shortcut. |
| Installed Shortcut | `InstalledShortcut` | Future OS-level shortcut; not implemented in v1. |
| Installer | `Installer` | Future install/update workflow; not implemented in v1. |
| WPF | Avoid domain names unless framework-specific | Windows Presentation Foundation, the UI framework. |

## Forbidden Synonyms

| Do not use | Use instead | Reason |
|---|---|---|
| Thing, item, record, data, object | The specific domain term | Too vague outside conventional framework usage. |
| Manager, helper, util | A more specific component name | Usually hides responsibility. |
| Junk, trash | Cleanup Candidate | Too judgmental before review. |
| Delete Candidate | Cleanup Candidate | Implies the action is already chosen. |
| Safe File | Low-risk Cleanup Candidate | Safety depends on context. |
| Score | Importance Rating or Deletion Recommendation | Too vague unless the rating is named. |
| Restore Log | Restore Manifest | Manifest is the recovery record. |
| Manifest Preview | Restore Manifest Draft | Draft means no cleanup action has executed. |
| Confirmation Plan | Quarantine Confirmation Draft | Draft means no approval or execution yet. |
| Cleanup Plan | Quarantine Preview | Preview is a dry run, not an action plan. |

## Naming Rules

- Use these terms in code, tests, UI labels, docs, and filenames when the domain concept matters.
- Update this compact glossary first when introducing a durable term.
- Put detailed tooltip/help-text wording in feature briefs or reference docs, not in the read-first glossary.
