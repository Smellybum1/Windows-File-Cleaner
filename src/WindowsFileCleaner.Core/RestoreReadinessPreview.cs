namespace WindowsFileCleaner.Core;

public sealed record RestoreReadinessPreview(
    string QuarantineRootPath,
    string ActionsRootPath,
    IReadOnlyList<RestoreReadinessManifestPreview> Manifests,
    IReadOnlyList<QuarantineManifestDiscoveryIssue> DiscoveryIssues)
{
    public int ManifestCount => Manifests.Count;

    public int RestorableManifestCount => Manifests.Count(manifest => manifest.HasRestorableEntries);

    public int RestorableEntryCount => Manifests.Sum(manifest => manifest.RestorableCount);

    public int BlockedEntryCount => Manifests.Sum(manifest => manifest.BlockedCount);

    public int RecoveryReviewEntryCount => Manifests.Sum(manifest => manifest.RecoveryReviewCount);

    public bool HasDiscoveryIssues => DiscoveryIssues.Count > 0;
}
