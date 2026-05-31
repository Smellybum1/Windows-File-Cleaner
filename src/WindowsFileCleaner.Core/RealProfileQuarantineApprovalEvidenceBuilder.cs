namespace WindowsFileCleaner.Core;

public static class RealProfileQuarantineApprovalEvidenceBuilder
{
    public static RealProfileQuarantineApprovalEvidence Build(
        QuarantineExecutionReadiness? readiness,
        string enteredConfirmationText,
        DateTimeOffset checkedAtUtc,
        bool isRealProfileQuarantineMovementAvailable = false)
    {
        var entered = (enteredConfirmationText ?? "").Trim();
        var required = readiness?.RequiredConfirmationText ?? QuarantineConfirmationDraft.DefaultRequiredConfirmationText;
        var isConfirmationTextMatched = string.Equals(entered, required, StringComparison.Ordinal);
        var isExactRealProfileScope = readiness?.ScopeKind == QuarantineExecutionReadinessScopeKind.RealProfile
            && readiness.Disposition == QuarantineExecutionReadinessDisposition.RealProfileCandidate;
        var blockers = new List<string>();

        if (readiness is null)
        {
            blockers.Add("Create Real-Profile Quarantine Execution Readiness before checking approval evidence.");
        }
        else
        {
            if (!isExactRealProfileScope)
            {
                blockers.Add("Real-profile Quarantine approval evidence requires the exact C:\\Users\\moxhe Cleanup Scope.");
            }

            foreach (var readinessBlocker in readiness.Blockers)
            {
                blockers.Add($"Readiness: {readinessBlocker}");
            }
        }

        if (!isConfirmationTextMatched)
        {
            blockers.Add($"Type {required} to confirm the reviewed real-profile Quarantine readiness before movement can be considered.");
        }

        if (!isRealProfileQuarantineMovementAvailable)
        {
            blockers.Add("Real-profile Quarantine movement remains unavailable in this build.");
        }

        return new RealProfileQuarantineApprovalEvidence(
            checkedAtUtc,
            readiness?.CleanupScopePath ?? "",
            readiness?.QuarantineRootPath ?? "",
            readiness?.ScopeKind ?? QuarantineExecutionReadinessScopeKind.Unknown,
            readiness?.Disposition ?? QuarantineExecutionReadinessDisposition.WaitingForPreview,
            required,
            entered,
            isConfirmationTextMatched,
            isExactRealProfileScope,
            readiness?.HasBlockers ?? true,
            isRealProfileQuarantineMovementAvailable,
            blockers,
            BuildReviewNotes(isRealProfileQuarantineMovementAvailable));
    }

    private static IReadOnlyList<string> BuildReviewNotes(bool isRealProfileQuarantineMovementAvailable)
    {
        var notes = new List<string>
        {
            "No files were modified by this approval evidence check.",
            $"Exact {QuarantineConfirmationDraft.DefaultRequiredConfirmationText} is necessary but not sufficient for real-profile Quarantine movement.",
            "Real-profile movement also requires clean readiness, immediate revalidation, Quarantine Root execution safety, recovery readiness, and explicit movement availability."
        };

        if (!isRealProfileQuarantineMovementAvailable)
        {
            notes.Add("Current build keeps real-profile Quarantine movement unavailable.");
        }

        return notes;
    }
}
