namespace WindowsFileCleaner.Core;

public sealed record RealProfileQuarantineApprovalEvidence(
    DateTimeOffset CheckedAtUtc,
    string CleanupScopePath,
    string QuarantineRootPath,
    QuarantineExecutionReadinessScopeKind ScopeKind,
    QuarantineExecutionReadinessDisposition ReadinessDisposition,
    string RequiredConfirmationText,
    string EnteredConfirmationText,
    bool IsConfirmationTextMatched,
    bool IsExactRealProfileScope,
    bool ReadinessHasBlockers,
    bool IsRealProfileQuarantineMovementAvailable,
    IReadOnlyList<string> Blockers,
    IReadOnlyList<string> ReviewNotes)
{
    public bool CanApproveForRealProfileMovement => Blockers.Count == 0;
}
