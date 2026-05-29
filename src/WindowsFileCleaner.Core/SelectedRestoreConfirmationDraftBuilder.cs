namespace WindowsFileCleaner.Core;

public static class SelectedRestoreConfirmationDraftBuilder
{
    public static SelectedRestoreConfirmationDraft Build(
        SelectedRestoreManifestReview? review,
        DateTimeOffset draftedAtUtc,
        string confirmationId,
        bool isExecutionImplemented = false)
    {
        if (string.IsNullOrWhiteSpace(confirmationId))
        {
            throw new ArgumentException("Selected Restore Confirmation Draft id is required.", nameof(confirmationId));
        }

        var readiness = review?.Readiness;

        return new SelectedRestoreConfirmationDraft(
            confirmationId.Trim(),
            draftedAtUtc.ToUniversalTime(),
            review?.QuarantineRootPath ?? "",
            review?.ActionsRootPath ?? "",
            review?.SelectedManifestPath ?? "",
            review?.SelectedManifest?.ActionId ?? readiness?.ActionId ?? "",
            SelectedRestoreConfirmationDraft.DefaultRequiredConfirmationText,
            isExecutionImplemented,
            readiness?.RestorableCount ?? 0,
            readiness?.Entries.Where(entry => entry.CanRestore).Sum(entry => entry.SizeBytes) ?? 0,
            readiness?.BlockedCount ?? 0,
            readiness?.RecoveryReviewCount ?? 0,
            readiness?.AlreadyRestoredCount ?? 0,
            readiness?.NotMovedCount ?? 0,
            BuildBlockers(review),
            BuildReviewNotes(isExecutionImplemented));
    }

    private static IReadOnlyList<string> BuildBlockers(SelectedRestoreManifestReview? review)
    {
        var blockers = new List<string>();
        if (review is null)
        {
            blockers.Add("Preview selected Restore Manifest readiness before selected restore confirmation.");
            return blockers;
        }

        foreach (var issue in review.SelectionIssues)
        {
            blockers.Add($"Selected Restore Manifest Review issue: {issue}");
        }

        var readiness = review.Readiness;
        if (readiness is null)
        {
            blockers.Add("Selected Restore Manifest Review has no readiness preview.");
            return blockers;
        }

        if (!readiness.HasRestorableEntries)
        {
            blockers.Add("Selected Restore Manifest has no restorable entries.");
        }

        if (readiness.BlockedCount > 0)
        {
            blockers.Add($"{readiness.BlockedCount:N0} blocked restore readiness row(s) must be resolved before selected restore confirmation.");
        }

        if (readiness.RecoveryReviewCount > 0)
        {
            blockers.Add($"{readiness.RecoveryReviewCount:N0} recovery-review row(s) must be resolved before selected restore confirmation.");
        }

        if (readiness.NotMovedCount > 0)
        {
            blockers.Add($"{readiness.NotMovedCount:N0} not-moved row(s) must be resolved before selected restore confirmation.");
        }

        if (readiness.RequiresRecoveryReview)
        {
            blockers.Add("Selected Restore Manifest status requires recovery review before restore.");
        }

        return blockers;
    }

    private static IReadOnlyList<string> BuildReviewNotes(bool isExecutionImplemented)
    {
        var executionNote = isExecutionImplemented
            ? "Selected restore execution is available only after readiness blockers clear and the exact confirmation text is entered."
            : "Selected restore execution is not available for discovered manifests in this build.";

        return
        [
            "No files were modified by this selected restore confirmation draft.",
            executionNote,
            $"Selected restore execution must require the exact confirmation text: {SelectedRestoreConfirmationDraft.DefaultRequiredConfirmationText}."
        ];
    }
}
