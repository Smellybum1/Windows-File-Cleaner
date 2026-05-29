namespace WindowsFileCleaner.Core;

public static class SelectedRestoreManifestReviewBuilder
{
    public static SelectedRestoreManifestReview BuildMissingDiscovery(
        string quarantineRootPath,
        string? selectedManifestPath)
    {
        var rootPath = NormalizePathOrEmpty(quarantineRootPath);
        return new SelectedRestoreManifestReview(
            rootPath,
            "",
            NormalizeNullablePath(selectedManifestPath),
            null,
            null,
            ["Run Quarantine Manifest Discovery before selecting a Restore Manifest."]);
    }

    public static SelectedRestoreManifestReview Build(
        QuarantineManifestDiscovery discovery,
        string? selectedManifestPath)
    {
        var selectedPath = NormalizeNullablePath(selectedManifestPath);

        if (discovery.ManifestCount == 0)
        {
            return CreateIssueReview(
                discovery,
                selectedPath,
                "Current Quarantine Manifest Discovery has no Restore Manifests to select.");
        }

        if (string.IsNullOrWhiteSpace(selectedPath))
        {
            return CreateIssueReview(
                discovery,
                selectedPath,
                "Select one discovered Restore Manifest before previewing selected manifest readiness.");
        }

        var selectedManifest = discovery.RestoreManifests
            .FirstOrDefault(manifest => SamePath(manifest.ManifestPath, selectedPath));
        var selectedSummary = discovery.Manifests
            .FirstOrDefault(summary => SamePath(summary.ManifestPath, selectedPath));

        if (selectedManifest is null || selectedSummary is null)
        {
            return CreateIssueReview(
                discovery,
                selectedPath,
                "Selected Restore Manifest is not part of the current Quarantine Manifest Discovery result.");
        }

        return new SelectedRestoreManifestReview(
            discovery.QuarantineRootPath,
            discovery.ActionsRootPath,
            selectedSummary.ManifestPath,
            selectedSummary,
            RestoreReadinessPreviewBuilder.BuildManifestPreview(selectedManifest),
            []);
    }

    private static SelectedRestoreManifestReview CreateIssueReview(
        QuarantineManifestDiscovery discovery,
        string? selectedManifestPath,
        string issue)
    {
        return new SelectedRestoreManifestReview(
            discovery.QuarantineRootPath,
            discovery.ActionsRootPath,
            selectedManifestPath,
            null,
            null,
            [issue]);
    }

    private static string? NormalizeNullablePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        return NormalizePathOrEmpty(path);
    }

    private static string NormalizePathOrEmpty(string path)
    {
        try
        {
            return PathSafety.GetFullPath(path);
        }
        catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
        {
            return path.Trim();
        }
    }

    private static bool SamePath(string left, string right)
    {
        return string.Equals(
            NormalizePathOrEmpty(left).TrimEnd(Path.DirectorySeparatorChar),
            NormalizePathOrEmpty(right).TrimEnd(Path.DirectorySeparatorChar),
            StringComparison.OrdinalIgnoreCase);
    }
}
