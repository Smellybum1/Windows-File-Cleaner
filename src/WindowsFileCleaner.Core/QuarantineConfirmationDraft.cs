namespace WindowsFileCleaner.Core;

public sealed record QuarantineConfirmationDraft(
    string ConfirmationId,
    DateTimeOffset DraftedAtUtc,
    string CleanupScopePath,
    string QuarantineRootPath,
    string RestoreManifestDraftId,
    string RequiredConfirmationText,
    int IncludedCount,
    long IncludedBytes,
    int BlockedCount,
    int RedundantCount,
    IReadOnlyList<string> Blockers,
    IReadOnlyList<string> ReviewNotes)
{
    public const string DefaultRequiredConfirmationText = "QUARANTINE";

    public string IncludedSizeDisplay => ByteSizeFormatter.Format(IncludedBytes);
    public bool HasDataBlockers => Blockers.Count > 0;
    public bool IsExecutionImplemented => false;
}
