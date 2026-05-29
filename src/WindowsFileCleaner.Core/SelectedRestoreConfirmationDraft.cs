namespace WindowsFileCleaner.Core;

public sealed record SelectedRestoreConfirmationDraft(
    string ConfirmationId,
    DateTimeOffset DraftedAtUtc,
    string QuarantineRootPath,
    string ActionsRootPath,
    string SelectedManifestPath,
    string ActionId,
    string RequiredConfirmationText,
    bool IsExecutionImplemented,
    int RestorableEntryCount,
    long RestorableBytes,
    int BlockedEntryCount,
    int RecoveryReviewEntryCount,
    int AlreadyRestoredEntryCount,
    int NotMovedEntryCount,
    IReadOnlyList<string> Blockers,
    IReadOnlyList<string> ReviewNotes)
{
    public const string DefaultRequiredConfirmationText = "RESTORE";

    public string RestorableSizeDisplay => ByteSizeFormatter.Format(RestorableBytes);

    public bool HasDataBlockers => Blockers.Count > 0;
}
