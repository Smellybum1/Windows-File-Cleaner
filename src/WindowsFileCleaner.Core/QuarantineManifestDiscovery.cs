namespace WindowsFileCleaner.Core;

public sealed record QuarantineManifestDiscovery(
    string QuarantineRootPath,
    string ActionsRootPath,
    IReadOnlyList<RestoreManifestSummary> Manifests,
    IReadOnlyList<RestoreManifest> RestoreManifests,
    IReadOnlyList<QuarantineManifestDiscoveryIssue> Issues)
{
    public int ManifestCount => Manifests.Count;

    public bool HasIssues => Issues.Count > 0;
}
