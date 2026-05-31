namespace WindowsFileCleaner.Core;

public static class SelectedRestorePreExecutionRevalidationBuilder
{
    public static SelectedRestorePreExecutionRevalidation Build(
        SelectedRestoreManifestReview? selectedReview,
        SelectedRestoreConfirmationDraft? confirmationDraft,
        SelectedRestoreExecutionGate? executionGate,
        DateTimeOffset revalidatedAtUtc,
        bool isSelectedManifestRealProfileUndoImplemented = false)
    {
        var blockers = new List<string>();
        var freshReview = BuildFreshReview(selectedReview, blockers);

        AddFreshReviewBlockers(freshReview, blockers);
        AddConsistencyBlockers(selectedReview, freshReview, confirmationDraft, executionGate, blockers);

        if (!isSelectedManifestRealProfileUndoImplemented)
        {
            blockers.Add("Selected-manifest real-profile Undo Quarantine remains unavailable in this build.");
        }

        var selectedManifest = freshReview?.SelectedManifest;
        var readiness = freshReview?.Readiness;
        var cleanupScopePath = selectedManifest?.CleanupScopePath ?? "";

        return new SelectedRestorePreExecutionRevalidation(
            revalidatedAtUtc.ToUniversalTime(),
            GetOptionalFullPath(freshReview?.QuarantineRootPath ?? selectedReview?.QuarantineRootPath),
            GetOptionalFullPath(freshReview?.ActionsRootPath ?? selectedReview?.ActionsRootPath),
            NormalizeOptionalPath(freshReview?.SelectedManifestPath ?? selectedReview?.SelectedManifestPath),
            selectedManifest?.ActionId ?? readiness?.ActionId ?? confirmationDraft?.ActionId ?? "",
            cleanupScopePath,
            IsRealProfileScope(cleanupScopePath),
            isSelectedManifestRealProfileUndoImplemented,
            executionGate?.IsConfirmationTextMatched ?? false,
            readiness?.RestorableCount ?? 0,
            readiness?.Entries.Where(entry => entry.CanRestore).Sum(entry => entry.SizeBytes) ?? 0,
            readiness?.BlockedCount ?? 0,
            readiness?.RecoveryReviewCount ?? 0,
            readiness?.AlreadyRestoredCount ?? 0,
            readiness?.NotMovedCount ?? 0,
            blockers,
            BuildReviewNotes());
    }

    private static SelectedRestoreManifestReview? BuildFreshReview(
        SelectedRestoreManifestReview? selectedReview,
        List<string> blockers)
    {
        if (selectedReview is null)
        {
            blockers.Add("Selected Restore Manifest Review has not been checked before pre-execution revalidation.");
            return null;
        }

        if (string.IsNullOrWhiteSpace(selectedReview.QuarantineRootPath))
        {
            blockers.Add("Selected Restore Manifest Review has no Quarantine Root for pre-execution revalidation.");
            return null;
        }

        if (string.IsNullOrWhiteSpace(selectedReview.SelectedManifestPath))
        {
            blockers.Add("Selected Restore Manifest Review has no selected Restore Manifest path for pre-execution revalidation.");
            return null;
        }

        try
        {
            var discovery = QuarantineManifestDiscoveryBuilder.Discover(selectedReview.QuarantineRootPath);
            foreach (var issue in discovery.Issues)
            {
                blockers.Add($"Quarantine Manifest Discovery issue: {issue.Message}");
            }

            return SelectedRestoreManifestReviewBuilder.Build(discovery, selectedReview.SelectedManifestPath);
        }
        catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException or IOException or UnauthorizedAccessException)
        {
            blockers.Add($"Could not rediscover selected Restore Manifest for pre-execution revalidation: {ex.Message}");
            return null;
        }
    }

    private static void AddFreshReviewBlockers(
        SelectedRestoreManifestReview? freshReview,
        List<string> blockers)
    {
        if (freshReview is null)
        {
            return;
        }

        foreach (var issue in freshReview.SelectionIssues)
        {
            blockers.Add($"Selected Restore Manifest Review issue: {issue}");
        }

        if (freshReview.SelectedManifest is null)
        {
            blockers.Add("No selected Restore Manifest is available for selected restore pre-execution revalidation.");
        }

        if (freshReview.Readiness is null)
        {
            blockers.Add("Selected Restore Manifest readiness could not be rebuilt during pre-execution revalidation.");
            return;
        }

        if (!IsRealProfileScope(freshReview.SelectedManifest?.CleanupScopePath ?? ""))
        {
            blockers.Add("Selected Restore Manifest Cleanup Scope must be the exact real-profile scope C:\\Users\\moxhe before selected restore execution can proceed.");
        }

        foreach (var blocker in freshReview.Readiness.Blockers)
        {
            blockers.Add($"Selected Restore Manifest readiness: {blocker}");
        }

        foreach (var entry in freshReview.Readiness.Entries)
        {
            foreach (var blocker in entry.Blockers)
            {
                blockers.Add($"Selected Restore Manifest entry: {blocker}");
            }
        }

        if (!freshReview.Readiness.HasRestorableEntries)
        {
            blockers.Add("Selected Restore Manifest has no restorable entries during pre-execution revalidation.");
        }

        if (freshReview.Readiness.BlockedCount > 0)
        {
            blockers.Add($"{freshReview.Readiness.BlockedCount:N0} blocked selected-restore readiness row(s) must be resolved before selected restore execution.");
        }

        if (freshReview.Readiness.RecoveryReviewCount > 0)
        {
            blockers.Add($"{freshReview.Readiness.RecoveryReviewCount:N0} recovery-review selected-restore row(s) must be resolved before selected restore execution.");
        }

        if (freshReview.Readiness.NotMovedCount > 0)
        {
            blockers.Add($"{freshReview.Readiness.NotMovedCount:N0} not-moved selected-restore row(s) must be resolved before selected restore execution.");
        }

        if (freshReview.Readiness.RequiresRecoveryReview)
        {
            blockers.Add("Selected Restore Manifest requires recovery review before selected restore execution.");
        }
    }

    private static void AddConsistencyBlockers(
        SelectedRestoreManifestReview? selectedReview,
        SelectedRestoreManifestReview? freshReview,
        SelectedRestoreConfirmationDraft? confirmationDraft,
        SelectedRestoreExecutionGate? executionGate,
        List<string> blockers)
    {
        if (confirmationDraft is null)
        {
            blockers.Add("Selected Restore Confirmation Draft has not been checked before pre-execution revalidation.");
        }
        else
        {
            if (!confirmationDraft.IsExecutionImplemented)
            {
                blockers.Add("Selected Restore Confirmation Draft does not record an implemented selected restore execution path.");
            }

            if (confirmationDraft.RequiredConfirmationText != SelectedRestoreConfirmationDraft.DefaultRequiredConfirmationText)
            {
                blockers.Add($"Selected restore confirmation must require exact {SelectedRestoreConfirmationDraft.DefaultRequiredConfirmationText}.");
            }

            foreach (var blocker in confirmationDraft.Blockers)
            {
                blockers.Add($"Selected Restore Confirmation Draft: {blocker}");
            }
        }

        if (executionGate is null)
        {
            blockers.Add("Selected Restore Execution Gate has not been checked before pre-execution revalidation.");
        }
        else
        {
            if (!executionGate.IsExecutionImplemented)
            {
                blockers.Add("Selected Restore Execution Gate does not record an implemented selected restore execution path.");
            }

            if (!executionGate.IsConfirmationTextMatched)
            {
                blockers.Add($"Selected Restore Execution Gate requires exact {SelectedRestoreConfirmationDraft.DefaultRequiredConfirmationText} confirmation.");
            }

            foreach (var blocker in executionGate.Blockers)
            {
                blockers.Add($"Selected Restore Execution Gate: {blocker}");
            }
        }

        if (selectedReview is not null && freshReview is not null)
        {
            AddReviewMismatchBlockers(selectedReview, freshReview, blockers);
        }

        if (freshReview is not null && confirmationDraft is not null)
        {
            AddDraftMismatchBlockers(freshReview, confirmationDraft, blockers);
        }

        if (confirmationDraft is not null
            && executionGate is not null
            && executionGate.RequiredConfirmationText != confirmationDraft.RequiredConfirmationText)
        {
            blockers.Add("Selected Restore Execution Gate required confirmation text does not match the confirmation draft.");
        }
    }

    private static void AddReviewMismatchBlockers(
        SelectedRestoreManifestReview selectedReview,
        SelectedRestoreManifestReview freshReview,
        List<string> blockers)
    {
        if (!SamePath(selectedReview.QuarantineRootPath, freshReview.QuarantineRootPath))
        {
            blockers.Add("Selected Restore Manifest Review Quarantine Root changed before pre-execution revalidation.");
        }

        if (!SamePath(selectedReview.ActionsRootPath, freshReview.ActionsRootPath))
        {
            blockers.Add("Selected Restore Manifest Review actions root changed before pre-execution revalidation.");
        }

        if (!SameNullablePath(selectedReview.SelectedManifestPath, freshReview.SelectedManifestPath))
        {
            blockers.Add("Selected Restore Manifest path changed before pre-execution revalidation.");
        }

        if (!string.Equals(selectedReview.SelectedManifest?.ActionId ?? "", freshReview.SelectedManifest?.ActionId ?? "", StringComparison.Ordinal))
        {
            blockers.Add("Selected Restore Manifest action id changed before pre-execution revalidation.");
        }

        if (!SamePath(selectedReview.SelectedManifest?.CleanupScopePath ?? "", freshReview.SelectedManifest?.CleanupScopePath ?? ""))
        {
            blockers.Add("Selected Restore Manifest Cleanup Scope changed before pre-execution revalidation.");
        }

        if ((selectedReview.Readiness?.EntryCount ?? 0) != (freshReview.Readiness?.EntryCount ?? 0))
        {
            blockers.Add("Selected Restore Manifest entry count changed before pre-execution revalidation.");
        }

        if ((selectedReview.Readiness?.TotalBytes ?? 0) != (freshReview.Readiness?.TotalBytes ?? 0))
        {
            blockers.Add("Selected Restore Manifest byte count changed before pre-execution revalidation.");
        }
    }

    private static void AddDraftMismatchBlockers(
        SelectedRestoreManifestReview freshReview,
        SelectedRestoreConfirmationDraft confirmationDraft,
        List<string> blockers)
    {
        if (!SamePath(freshReview.QuarantineRootPath, confirmationDraft.QuarantineRootPath))
        {
            blockers.Add("Selected Restore Confirmation Draft Quarantine Root does not match fresh selected manifest review.");
        }

        if (!SamePath(freshReview.ActionsRootPath, confirmationDraft.ActionsRootPath))
        {
            blockers.Add("Selected Restore Confirmation Draft actions root does not match fresh selected manifest review.");
        }

        if (!SameNullablePath(freshReview.SelectedManifestPath, confirmationDraft.SelectedManifestPath))
        {
            blockers.Add("Selected Restore Confirmation Draft manifest path does not match fresh selected manifest review.");
        }

        if (!string.Equals(freshReview.SelectedManifest?.ActionId ?? "", confirmationDraft.ActionId, StringComparison.Ordinal))
        {
            blockers.Add("Selected Restore Confirmation Draft action id does not match fresh selected manifest review.");
        }
    }

    private static IReadOnlyList<string> BuildReviewNotes()
    {
        return
        [
            "No folders were created and no files were modified by this Selected Restore Pre-Execution Revalidation.",
            "Revalidation must run again immediately before any future real-profile selected restore movement.",
            "Restore Manifest remains the only durable cleanup record for the first real-profile selected restore phase."
        ];
    }

    private static bool IsRealProfileScope(string cleanupScopePath)
    {
        if (string.IsNullOrWhiteSpace(cleanupScopePath))
        {
            return false;
        }

        var defaultProfile = PathSafety.GetFullPath(StorageScanOptions.DefaultForCurrentUser().CleanupScopePath);
        return PathSafety.GetFullPath(cleanupScopePath).Equals(defaultProfile, StringComparison.OrdinalIgnoreCase);
    }

    private static bool SameNullablePath(string? left, string? right)
    {
        return !string.IsNullOrWhiteSpace(left)
            && !string.IsNullOrWhiteSpace(right)
            && SamePath(left, right);
    }

    private static bool SamePath(string left, string right)
    {
        if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right))
        {
            return false;
        }

        return PathSafety.GetFullPath(left).Equals(PathSafety.GetFullPath(right), StringComparison.OrdinalIgnoreCase);
    }

    private static string? NormalizeOptionalPath(string? path)
    {
        return string.IsNullOrWhiteSpace(path)
            ? null
            : PathSafety.GetFullPath(path);
    }

    private static string GetOptionalFullPath(string? path)
    {
        return string.IsNullOrWhiteSpace(path)
            ? ""
            : PathSafety.GetFullPath(path);
    }
}
