namespace WindowsFileCleaner.Core;

public static class QuarantineExecutionReadinessBuilder
{
    public static QuarantineExecutionReadiness Build(
        QuarantinePreview? preview,
        QuarantineConfirmationDraft? confirmationDraft,
        bool isSelectedManifestRealProfileUndoAvailable = false,
        bool nonPreferredQuarantineRootAcknowledged = false,
        QuarantineRootExecutionSafety? quarantineRootExecutionSafety = null)
    {
        if (preview is null)
        {
            return new QuarantineExecutionReadiness(
                CleanupScopePath: "",
                QuarantineRootPath: "",
                QuarantineExecutionReadinessScopeKind.Unknown,
                QuarantineExecutionReadinessDisposition.WaitingForPreview,
                QuarantineConfirmationDraft.DefaultRequiredConfirmationText,
                IncludedCount: 0,
                IncludedBytes: 0,
                QuarantineExecutionReadiness.DefaultRealProfileIncludedRowLimit,
                QuarantineExecutionReadiness.DefaultRealProfileIncludedByteLimit,
                IsPreferredQuarantineRoot: false,
                IsNonPreferredQuarantineRootAcknowledged: nonPreferredQuarantineRootAcknowledged,
                AllowsNarrowFolders: true,
                RequiresSelectedManifestRealProfileUndoBeforeForwardQuarantine: true,
                RequiresManualRescanAfterExecution: true,
                UsesRestoreManifestOnly: true,
                ["Create a Quarantine Preview before checking execution readiness."],
                ["No files were modified by this readiness check."]);
        }

        var cleanupScope = PathSafety.GetFullPath(preview.CleanupScopePath);
        var quarantineRoot = PathSafety.GetFullPath(preview.QuarantineRootPath);
        var scopeKind = ClassifyScope(cleanupScope);
        var isPreferredRoot = IsPreferredQuarantineRoot(quarantineRoot);
        var disposition = GetDisposition(scopeKind);
        var blockers = new List<string>();

        if (confirmationDraft is null)
        {
            blockers.Add("Create a Quarantine Confirmation Draft before checking execution readiness.");
        }
        else
        {
            blockers.AddRange(confirmationDraft.Blockers);
            if (!SamePath(preview.CleanupScopePath, confirmationDraft.CleanupScopePath))
            {
                blockers.Add("Quarantine Confirmation Draft Cleanup Scope does not match the Quarantine Preview.");
            }

            if (!SamePath(preview.QuarantineRootPath, confirmationDraft.QuarantineRootPath))
            {
                blockers.Add("Quarantine Confirmation Draft Quarantine root does not match the Quarantine Preview.");
            }
        }

        switch (scopeKind)
        {
            case QuarantineExecutionReadinessScopeKind.Fixture:
                AddFixtureBlockers(confirmationDraft, blockers);
                break;
            case QuarantineExecutionReadinessScopeKind.RealProfile:
                AddRealProfileBlockers(
                    preview,
                    quarantineRootExecutionSafety,
                    isPreferredRoot,
                    nonPreferredQuarantineRootAcknowledged,
                    isSelectedManifestRealProfileUndoAvailable,
                    blockers);
                break;
            case QuarantineExecutionReadinessScopeKind.RealProfileChild:
                blockers.Add("The first real-profile Quarantine phase is limited to C:\\Users\\moxhe. Child scopes under the profile remain preview-only.");
                break;
            case QuarantineExecutionReadinessScopeKind.Custom:
                blockers.Add("Custom non-fixture Cleanup Scopes remain preview-only for this real-profile phase.");
                break;
            default:
                blockers.Add("Choose a fixture or recognized real-profile Cleanup Scope before checking execution readiness.");
                break;
        }

        return new QuarantineExecutionReadiness(
            cleanupScope,
            quarantineRoot,
            scopeKind,
            disposition,
            confirmationDraft?.RequiredConfirmationText ?? QuarantineConfirmationDraft.DefaultRequiredConfirmationText,
            preview.IncludedCount,
            preview.IncludedBytes,
            QuarantineExecutionReadiness.DefaultRealProfileIncludedRowLimit,
            QuarantineExecutionReadiness.DefaultRealProfileIncludedByteLimit,
            isPreferredRoot,
            nonPreferredQuarantineRootAcknowledged,
            AllowsNarrowFolders: true,
            RequiresSelectedManifestRealProfileUndoBeforeForwardQuarantine: true,
            RequiresManualRescanAfterExecution: true,
            UsesRestoreManifestOnly: true,
            blockers,
            BuildReviewNotes(scopeKind, isPreferredRoot));
    }

    private static void AddFixtureBlockers(
        QuarantineConfirmationDraft? confirmationDraft,
        List<string> blockers)
    {
        if (confirmationDraft is null)
        {
            return;
        }

        if (!confirmationDraft.IsExecutionImplemented)
        {
            blockers.Add("Fixture-only Quarantine execution is not available for this Cleanup Scope.");
        }
    }

    private static void AddRealProfileBlockers(
        QuarantinePreview preview,
        QuarantineRootExecutionSafety? quarantineRootExecutionSafety,
        bool isPreferredRoot,
        bool nonPreferredQuarantineRootAcknowledged,
        bool isSelectedManifestRealProfileUndoAvailable,
        List<string> blockers)
    {
        blockers.Add("Real-profile WPF Quarantine execution remains unavailable in this build.");
        AddRootSafetyBlockers(preview, quarantineRootExecutionSafety, blockers);
        blockers.Add("Pre-Execution Revalidation is not implemented for real-profile execution yet.");

        if (!isSelectedManifestRealProfileUndoAvailable)
        {
            blockers.Add("Selected-manifest real-profile Undo Quarantine must be implemented and tested before forward real-profile movement.");
        }

        if (preview.IncludedCount > QuarantineExecutionReadiness.DefaultRealProfileIncludedRowLimit)
        {
            blockers.Add($"First real-profile Quarantine is capped at {QuarantineExecutionReadiness.DefaultRealProfileIncludedRowLimit:N0} included row(s).");
        }

        if (preview.IncludedBytes > QuarantineExecutionReadiness.DefaultRealProfileIncludedByteLimit)
        {
            blockers.Add($"First real-profile Quarantine is capped at {ByteSizeFormatter.Format(QuarantineExecutionReadiness.DefaultRealProfileIncludedByteLimit)}.");
        }

        if (!isPreferredRoot && !nonPreferredQuarantineRootAcknowledged)
        {
            blockers.Add("Non-D: Quarantine roots require an extra acknowledgement before real-profile execution can be considered.");
        }

        foreach (var includedEntry in preview.Entries.Where(entry => entry.IsIncluded))
        {
            AddIncludedEntryBlockers(includedEntry, blockers);
        }
    }

    private static void AddRootSafetyBlockers(
        QuarantinePreview preview,
        QuarantineRootExecutionSafety? quarantineRootExecutionSafety,
        List<string> blockers)
    {
        if (quarantineRootExecutionSafety is null)
        {
            blockers.Add("Quarantine Root Execution Safety has not been checked for real-profile execution yet.");
            return;
        }

        if (!SamePath(preview.CleanupScopePath, quarantineRootExecutionSafety.CleanupScopePath))
        {
            blockers.Add("Quarantine Root Execution Safety Cleanup Scope does not match the Quarantine Preview.");
        }

        if (!SamePath(preview.QuarantineRootPath, quarantineRootExecutionSafety.QuarantineRootPath))
        {
            blockers.Add("Quarantine Root Execution Safety Quarantine Root does not match the Quarantine Preview.");
        }

        foreach (var blocker in quarantineRootExecutionSafety.Blockers)
        {
            blockers.Add($"Quarantine Root Execution Safety: {blocker}");
        }
    }

    private static void AddIncludedEntryBlockers(
        QuarantinePreviewEntry previewEntry,
        List<string> blockers)
    {
        var entry = previewEntry.Entry;
        var prefix = $"Real-profile included row is not eligible for first-phase execution: {entry.FullPath}.";

        if (entry.ImportanceRating != ImportanceRating.LikelySafe)
        {
            blockers.Add($"{prefix} Only Likely safe rows can execute in the first real-profile phase.");
        }

        if (entry.DeletionRecommendation != DeletionRecommendation.QuarantineCandidate)
        {
            blockers.Add($"{prefix} Only Quarantine candidate rows can execute in the first real-profile phase.");
        }

        if (!entry.IsAccessible)
        {
            blockers.Add($"{prefix} Access issue rows are blocked.");
        }

        if (entry.IsReparsePoint || entry.BloatCategories.Contains(BloatCategory.ReparsePoint))
        {
            blockers.Add($"{prefix} Reparse points are blocked.");
        }

        if (entry.BloatCategories.Count == 0)
        {
            blockers.Add($"{prefix} No-category rows are blocked.");
        }

        if (entry.BloatCategories.Contains(BloatCategory.ProtectedLocation))
        {
            blockers.Add($"{prefix} Protected locations are blocked.");
        }

        if (entry.BloatCategories.Contains(BloatCategory.AccessIssue))
        {
            blockers.Add($"{prefix} Access issue categories are blocked.");
        }

        if (entry.IsDirectory)
        {
            var blockedDescendant = FindBlockedDescendant(entry);
            if (blockedDescendant is not null)
            {
                blockers.Add($"{prefix} Narrow folders are allowed only when strict descendant checks pass; blocked descendant: {blockedDescendant.FullPath}.");
            }
        }
    }

    private static StorageEntry? FindBlockedDescendant(StorageEntry entry)
    {
        foreach (var child in entry.Children)
        {
            if (child.ImportanceRating != ImportanceRating.LikelySafe
                || child.DeletionRecommendation != DeletionRecommendation.QuarantineCandidate
                || !child.IsAccessible
                || child.IsReparsePoint
                || child.BloatCategories.Count == 0
                || child.BloatCategories.Contains(BloatCategory.ProtectedLocation)
                || child.BloatCategories.Contains(BloatCategory.AccessIssue)
                || child.BloatCategories.Contains(BloatCategory.ReparsePoint)
                || child.BloatCategories.Contains(BloatCategory.CleanupScopeRoot))
            {
                return child;
            }

            var descendant = FindBlockedDescendant(child);
            if (descendant is not null)
            {
                return descendant;
            }
        }

        return null;
    }

    private static QuarantineExecutionReadinessDisposition GetDisposition(QuarantineExecutionReadinessScopeKind scopeKind)
    {
        return scopeKind switch
        {
            QuarantineExecutionReadinessScopeKind.Fixture => QuarantineExecutionReadinessDisposition.FixtureExecutable,
            QuarantineExecutionReadinessScopeKind.RealProfile => QuarantineExecutionReadinessDisposition.RealProfileCandidate,
            QuarantineExecutionReadinessScopeKind.RealProfileChild => QuarantineExecutionReadinessDisposition.CustomPreviewOnly,
            QuarantineExecutionReadinessScopeKind.Custom => QuarantineExecutionReadinessDisposition.CustomPreviewOnly,
            _ => QuarantineExecutionReadinessDisposition.WaitingForPreview
        };
    }

    private static QuarantineExecutionReadinessScopeKind ClassifyScope(string cleanupScopePath)
    {
        var note = CleanupScopeSafetyNoteBuilder.Build(cleanupScopePath);
        if (note.IsFixtureScope)
        {
            return QuarantineExecutionReadinessScopeKind.Fixture;
        }

        var defaultProfile = PathSafety.GetFullPath(StorageScanOptions.DefaultForCurrentUser().CleanupScopePath);
        if (cleanupScopePath.Equals(defaultProfile, StringComparison.OrdinalIgnoreCase))
        {
            return QuarantineExecutionReadinessScopeKind.RealProfile;
        }

        if (PathSafety.IsWithinScope(defaultProfile, cleanupScopePath))
        {
            return QuarantineExecutionReadinessScopeKind.RealProfileChild;
        }

        return QuarantineExecutionReadinessScopeKind.Custom;
    }

    private static bool IsPreferredQuarantineRoot(string quarantineRootPath)
    {
        var root = Path.GetPathRoot(quarantineRootPath);
        return root is not null && root.Equals(@"D:\", StringComparison.OrdinalIgnoreCase);
    }

    private static IReadOnlyList<string> BuildReviewNotes(
        QuarantineExecutionReadinessScopeKind scopeKind,
        bool isPreferredRoot)
    {
        var notes = new List<string>
        {
            "No files were modified by this readiness check.",
            $"Required confirmation text remains {QuarantineConfirmationDraft.DefaultRequiredConfirmationText}.",
            "Restore Manifest remains the only durable cleanup record for the first real-profile phase.",
            "After real-profile Quarantine execution, the app should show stale-scan guidance and ask the user to rescan manually."
        };

        if (scopeKind == QuarantineExecutionReadinessScopeKind.RealProfile)
        {
            notes.Add("First real-profile Quarantine is limited to C:\\Users\\moxhe.");
            notes.Add($"First real-profile Quarantine is capped at {QuarantineExecutionReadiness.DefaultRealProfileIncludedRowLimit:N0} included row(s) and {ByteSizeFormatter.Format(QuarantineExecutionReadiness.DefaultRealProfileIncludedByteLimit)}.");
            notes.Add("Files and narrow folders are allowed only when strict descendant checks pass.");
            notes.Add(isPreferredRoot
                ? "D: is the preferred Quarantine Root for real-profile execution."
                : "Non-D: Quarantine roots require an extra acknowledgement before real-profile execution can be considered.");
        }

        return notes;
    }

    private static bool SamePath(string left, string right)
    {
        return PathSafety.GetFullPath(left).Equals(PathSafety.GetFullPath(right), StringComparison.OrdinalIgnoreCase);
    }
}
