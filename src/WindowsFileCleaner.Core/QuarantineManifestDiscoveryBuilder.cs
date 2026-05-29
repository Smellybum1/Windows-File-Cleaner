using System.Text.Json;

namespace WindowsFileCleaner.Core;

public static class QuarantineManifestDiscoveryBuilder
{
    public static QuarantineManifestDiscovery Discover(string quarantineRootPath)
    {
        var note = QuarantineRootSafetyNoteBuilder.Build(quarantineRootPath);
        var rootPath = note.RootPath;
        var actionsRootPath = note.CanPreview
            ? Path.Combine(rootPath, "actions")
            : "";
        var summaries = new List<RestoreManifestSummary>();
        var manifests = new List<RestoreManifest>();
        var issues = new List<QuarantineManifestDiscoveryIssue>();

        if (!note.CanPreview)
        {
            issues.Add(new QuarantineManifestDiscoveryIssue(
                rootPath,
                note.Message));
            return new QuarantineManifestDiscovery(rootPath, actionsRootPath, summaries, manifests, issues);
        }

        if (!Directory.Exists(actionsRootPath))
        {
            issues.Add(new QuarantineManifestDiscoveryIssue(
                actionsRootPath,
                "No quarantine actions folder exists under the selected Quarantine Root."));
            return new QuarantineManifestDiscovery(rootPath, actionsRootPath, summaries, manifests, issues);
        }

        string[] actionFolders;
        try
        {
            actionFolders = Directory.EnumerateDirectories(actionsRootPath).ToArray();
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException or IOException)
        {
            issues.Add(new QuarantineManifestDiscoveryIssue(
                actionsRootPath,
                $"Could not read quarantine actions folder: {ex.Message}"));
            return new QuarantineManifestDiscovery(rootPath, actionsRootPath, summaries, manifests, issues);
        }

        foreach (var actionFolder in actionFolders)
        {
            var manifestPath = Path.Combine(actionFolder, RestoreManifestFileStore.RestoreManifestFileName);
            if (!File.Exists(manifestPath))
            {
                issues.Add(new QuarantineManifestDiscoveryIssue(
                    manifestPath,
                    "Action folder does not contain restore-manifest.json."));
                continue;
            }

            try
            {
                var json = File.ReadAllText(manifestPath);
                var manifest = RestoreManifestJsonSerializer.Deserialize(json);
                var issue = ValidateManifest(manifest, rootPath, actionFolder, manifestPath);
                if (issue is not null)
                {
                    issues.Add(issue);
                    continue;
                }

                manifests.Add(manifest);
                summaries.Add(BuildSummary(manifest));
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException or IOException)
            {
                issues.Add(new QuarantineManifestDiscoveryIssue(
                    manifestPath,
                    $"Could not read Restore Manifest: {ex.Message}"));
            }
            catch (Exception ex) when (ex is JsonException or InvalidOperationException or NotSupportedException)
            {
                issues.Add(new QuarantineManifestDiscoveryIssue(
                    manifestPath,
                    $"Could not parse Restore Manifest: {ex.Message}"));
            }
        }

        return new QuarantineManifestDiscovery(
            rootPath,
            actionsRootPath,
            summaries
                .OrderByDescending(summary => summary.UpdatedAtUtc)
                .ThenBy(summary => summary.ActionId, StringComparer.OrdinalIgnoreCase)
                .ToArray(),
            manifests
                .OrderByDescending(manifest => manifest.UpdatedAtUtc)
                .ThenBy(manifest => manifest.ActionId, StringComparer.OrdinalIgnoreCase)
                .ToArray(),
            issues);
    }

    private static QuarantineManifestDiscoveryIssue? ValidateManifest(
        RestoreManifest manifest,
        string quarantineRootPath,
        string actionFolderPath,
        string manifestPath)
    {
        if (!string.Equals(manifest.SchemaVersion, RestoreManifest.CurrentSchemaVersion, StringComparison.Ordinal))
        {
            return new QuarantineManifestDiscoveryIssue(
                manifestPath,
                $"Unsupported Restore Manifest schema version: {manifest.SchemaVersion}.");
        }

        if (!SamePath(manifest.QuarantineRootPath, quarantineRootPath))
        {
            return new QuarantineManifestDiscoveryIssue(
                manifestPath,
                "Restore Manifest quarantine root does not match the selected Quarantine Root.");
        }

        if (!SamePath(manifest.ActionRootPath, actionFolderPath))
        {
            return new QuarantineManifestDiscoveryIssue(
                manifestPath,
                "Restore Manifest action root does not match its action folder.");
        }

        if (!SamePath(manifest.ManifestPath, manifestPath))
        {
            return new QuarantineManifestDiscoveryIssue(
                manifestPath,
                "Restore Manifest path does not match its discovered file path.");
        }

        if (!PathSafety.IsWithinScope(manifest.ActionRootPath, manifest.ItemsRootPath))
        {
            return new QuarantineManifestDiscoveryIssue(
                manifestPath,
                "Restore Manifest items root is outside the action root.");
        }

        return null;
    }

    private static RestoreManifestSummary BuildSummary(RestoreManifest manifest)
    {
        return new RestoreManifestSummary(
            manifest.ManifestPath,
            manifest.ActionId,
            manifest.ActionRootPath,
            manifest.CleanupScopePath,
            manifest.ActionStatus,
            manifest.CreatedAtUtc,
            manifest.UpdatedAtUtc,
            manifest.EntryCount,
            manifest.TotalBytes,
            manifest.MovedCount,
            manifest.RestoredCount,
            manifest.FailedCount,
            manifest.RestoreFailedCount,
            manifest.RequiresRecoveryReview);
    }

    private static bool SamePath(string left, string right)
    {
        return string.Equals(
            PathSafety.GetFullPath(left).TrimEnd(Path.DirectorySeparatorChar),
            PathSafety.GetFullPath(right).TrimEnd(Path.DirectorySeparatorChar),
            StringComparison.OrdinalIgnoreCase);
    }
}
