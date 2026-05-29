namespace WindowsFileCleaner.Core;

public sealed record SelectedRestoreManifestReview(
    string QuarantineRootPath,
    string ActionsRootPath,
    string? SelectedManifestPath,
    RestoreManifestSummary? SelectedManifest,
    RestoreReadinessManifestPreview? Readiness,
    IReadOnlyList<string> SelectionIssues)
{
    public bool HasSelectedManifest => SelectedManifest is not null;

    public bool HasReadinessPreview => Readiness is not null;

    public bool HasSelectionIssues => SelectionIssues.Count > 0;

    public int RestorableEntryCount => Readiness?.RestorableCount ?? 0;

    public int BlockedEntryCount => Readiness?.BlockedCount ?? 0;

    public int RecoveryReviewEntryCount => Readiness?.RecoveryReviewCount ?? 0;
}
