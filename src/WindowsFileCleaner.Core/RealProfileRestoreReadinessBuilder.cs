namespace WindowsFileCleaner.Core;

public static class RealProfileRestoreReadinessBuilder
{
    public static RealProfileRestoreReadiness Build(
        SelectedRestoreManifestReview? review,
        SelectedRestoreConfirmationDraft? confirmationDraft,
        SelectedRestoreExecutionGate? executionGate,
        DateTimeOffset checkedAtUtc,
        bool isSelectedManifestRealProfileUndoImplemented = false)
    {
        var blockers = new List<string>();
        AddReviewBlockers(review, blockers);
        AddConfirmationBlockers(review, confirmationDraft, blockers);
        AddGateBlockers(confirmationDraft, executionGate, blockers);

        if (!isSelectedManifestRealProfileUndoImplemented)
        {
            blockers.Add("Selected-manifest real-profile Undo Quarantine remains unavailable in this build.");
        }

        var readiness = review?.Readiness;
        var selectedManifest = review?.SelectedManifest;
        var cleanupScopePath = selectedManifest?.CleanupScopePath ?? "";

        return new RealProfileRestoreReadiness(
            checkedAtUtc.ToUniversalTime(),
            GetOptionalFullPath(review?.QuarantineRootPath),
            GetOptionalFullPath(review?.ActionsRootPath),
            string.IsNullOrWhiteSpace(review?.SelectedManifestPath) ? null : PathSafety.GetFullPath(review.SelectedManifestPath),
            selectedManifest?.ActionId ?? readiness?.ActionId ?? "",
            cleanupScopePath,
            IsRealProfileScope(cleanupScopePath),
            isSelectedManifestRealProfileUndoImplemented,
            UsesRestoreManifestOnly: true,
            readiness?.RestorableCount ?? 0,
            readiness?.Entries.Where(entry => entry.CanRestore).Sum(entry => entry.SizeBytes) ?? 0,
            readiness?.BlockedCount ?? 0,
            readiness?.RecoveryReviewCount ?? 0,
            readiness?.AlreadyRestoredCount ?? 0,
            readiness?.NotMovedCount ?? 0,
            blockers,
            BuildReviewNotes(isSelectedManifestRealProfileUndoImplemented));
    }

    private static void AddReviewBlockers(
        SelectedRestoreManifestReview? review,
        List<string> blockers)
    {
        if (review is null)
        {
            blockers.Add("Selected Restore Manifest Review has not been checked for real-profile Undo readiness.");
            return;
        }

        foreach (var issue in review.SelectionIssues)
        {
            blockers.Add($"Selected Restore Manifest Review issue: {issue}");
        }

        if (review.SelectedManifest is null)
        {
            blockers.Add("No selected Restore Manifest is available for real-profile Undo readiness.");
        }

        if (review.Readiness is null)
        {
            blockers.Add("Selected Restore Manifest readiness has not been previewed for real-profile Undo readiness.");
            return;
        }

        if (!IsRealProfileScope(review.SelectedManifest?.CleanupScopePath ?? ""))
        {
            blockers.Add("Selected Restore Manifest Cleanup Scope must be the exact real-profile scope C:\\Users\\moxhe before it can prove real-profile Undo readiness.");
        }

        foreach (var blocker in review.Readiness.Blockers)
        {
            blockers.Add($"Selected Restore Manifest readiness: {blocker}");
        }

        if (!review.Readiness.HasRestorableEntries)
        {
            blockers.Add("Selected Restore Manifest has no restorable entries for real-profile Undo readiness.");
        }

        if (review.Readiness.BlockedCount > 0)
        {
            blockers.Add($"{review.Readiness.BlockedCount:N0} blocked selected-restore readiness row(s) must be resolved before real-profile Undo readiness.");
        }

        if (review.Readiness.RecoveryReviewCount > 0)
        {
            blockers.Add($"{review.Readiness.RecoveryReviewCount:N0} recovery-review selected-restore row(s) must be resolved before real-profile Undo readiness.");
        }

        if (review.Readiness.NotMovedCount > 0)
        {
            blockers.Add($"{review.Readiness.NotMovedCount:N0} not-moved selected-restore row(s) must be resolved before real-profile Undo readiness.");
        }

        if (review.Readiness.RequiresRecoveryReview)
        {
            blockers.Add("Selected Restore Manifest requires recovery review before real-profile Undo readiness.");
        }
    }

    private static void AddConfirmationBlockers(
        SelectedRestoreManifestReview? review,
        SelectedRestoreConfirmationDraft? confirmationDraft,
        List<string> blockers)
    {
        if (confirmationDraft is null)
        {
            blockers.Add("Selected Restore Confirmation Draft has not been checked for real-profile Undo readiness.");
            return;
        }

        if (confirmationDraft.RequiredConfirmationText != SelectedRestoreConfirmationDraft.DefaultRequiredConfirmationText)
        {
            blockers.Add($"Selected restore confirmation must require exact {SelectedRestoreConfirmationDraft.DefaultRequiredConfirmationText}.");
        }

        if (review is not null)
        {
            if (!SamePath(review.QuarantineRootPath, confirmationDraft.QuarantineRootPath))
            {
                blockers.Add("Selected Restore Confirmation Draft Quarantine Root does not match the selected manifest review.");
            }

            if (!SamePath(review.ActionsRootPath, confirmationDraft.ActionsRootPath))
            {
                blockers.Add("Selected Restore Confirmation Draft actions root does not match the selected manifest review.");
            }

            if (!string.Equals(review.SelectedManifest?.ActionId ?? "", confirmationDraft.ActionId, StringComparison.Ordinal))
            {
                blockers.Add("Selected Restore Confirmation Draft action id does not match the selected manifest review.");
            }

            if (!SameNullablePath(review.SelectedManifestPath, confirmationDraft.SelectedManifestPath))
            {
                blockers.Add("Selected Restore Confirmation Draft manifest path does not match the selected manifest review.");
            }
        }

        foreach (var blocker in confirmationDraft.Blockers)
        {
            blockers.Add($"Selected Restore Confirmation Draft: {blocker}");
        }
    }

    private static void AddGateBlockers(
        SelectedRestoreConfirmationDraft? confirmationDraft,
        SelectedRestoreExecutionGate? executionGate,
        List<string> blockers)
    {
        if (executionGate is null)
        {
            blockers.Add("Selected Restore Execution Gate has not been checked for real-profile Undo readiness.");
            return;
        }

        if (confirmationDraft is not null
            && executionGate.RequiredConfirmationText != confirmationDraft.RequiredConfirmationText)
        {
            blockers.Add("Selected Restore Execution Gate required confirmation text does not match the confirmation draft.");
        }

        if (!executionGate.IsConfirmationTextMatched)
        {
            blockers.Add($"Selected Restore Execution Gate requires exact {SelectedRestoreConfirmationDraft.DefaultRequiredConfirmationText} confirmation.");
        }
    }

    private static IReadOnlyList<string> BuildReviewNotes(bool isSelectedManifestRealProfileUndoImplemented)
    {
        return
        [
            "No files were modified by this real-profile restore readiness check.",
            "Real-profile recovery remains selected-manifest only for this readiness contract.",
            "Restore Manifest remains the only durable cleanup record for the first real-profile phase.",
            isSelectedManifestRealProfileUndoImplemented
                ? "Selected-manifest real-profile Undo Quarantine implementation is recorded as available by the caller."
                : "Selected-manifest real-profile Undo Quarantine is still unavailable in this build."
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

    private static bool SameNullablePath(string? left, string right)
    {
        return !string.IsNullOrWhiteSpace(left)
            && SamePath(left, right);
    }

    private static bool SamePath(string left, string right)
    {
        return PathSafety.GetFullPath(left).Equals(PathSafety.GetFullPath(right), StringComparison.OrdinalIgnoreCase);
    }

    private static string GetOptionalFullPath(string? path)
    {
        return string.IsNullOrWhiteSpace(path)
            ? ""
            : PathSafety.GetFullPath(path);
    }
}
