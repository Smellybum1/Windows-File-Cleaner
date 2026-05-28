using System.Text;

namespace WindowsFileCleaner.Core;

public static class StorageScanCsvExporter
{
    public static string Export(IReadOnlyList<StorageReviewEntry> entries, string? cleanupScopePath = null)
    {
        var builder = new StringBuilder();
        AppendRow(
            builder,
            [
                "Full path",
                "Relative path",
                "Parent path",
                "Depth",
                "Name",
                "Type",
                "Size bytes",
                "Size",
                "Contained files",
                "Contained folders",
                "Importance",
                "Recommendation",
                "Categories",
                "Last modified UTC",
                "Evidence",
                "Access status",
                "Access issue"
            ]);

        foreach (var entry in entries)
        {
            AppendRow(
                builder,
                [
                    entry.Entry.FullPath,
                    FormatRelativePath(entry, cleanupScopePath),
                    FormatParentPath(entry),
                    entry.Depth.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    entry.Entry.Name,
                    entry.Entry.IsDirectory ? "Folder" : "File",
                    entry.Entry.SizeBytes.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    entry.Entry.SizeDisplay,
                    entry.Entry.FileCount.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    GetContainedFolderCount(entry.Entry).ToString(System.Globalization.CultureInfo.InvariantCulture),
                    FormatImportance(entry.Entry.ImportanceRating),
                    FormatRecommendation(entry.Entry.DeletionRecommendation),
                    FormatCategories(entry.Entry.BloatCategories),
                    entry.Entry.LastModifiedUtc?.ToString("O") ?? "",
                    entry.Entry.Evidence,
                    FormatAccessStatus(entry.Entry),
                    entry.Entry.ErrorMessage ?? ""
                ]);
        }

        return builder.ToString();
    }

    private static void AppendRow(StringBuilder builder, IReadOnlyList<string> cells)
    {
        for (var i = 0; i < cells.Count; i++)
        {
            if (i > 0)
            {
                builder.Append(',');
            }

            builder.Append(Escape(cells[i]));
        }

        builder.AppendLine();
    }

    private static string Escape(string value)
    {
        return $"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";
    }

    private static string FormatParentPath(StorageReviewEntry entry)
    {
        return entry.Depth <= 0
            ? ""
            : Path.GetDirectoryName(entry.Entry.FullPath) ?? "";
    }

    private static string FormatRelativePath(StorageReviewEntry entry, string? cleanupScopePath)
    {
        if (string.IsNullOrWhiteSpace(cleanupScopePath))
        {
            return "";
        }

        try
        {
            return PathSafety.IsWithinScope(cleanupScopePath, entry.Entry.FullPath)
                ? Path.GetRelativePath(PathSafety.GetFullPath(cleanupScopePath), entry.Entry.FullPath)
                : "";
        }
        catch (ArgumentException)
        {
            return "";
        }
        catch (NotSupportedException)
        {
            return "";
        }
    }

    private static int GetContainedFolderCount(StorageEntry entry)
    {
        return entry.IsDirectory ? Math.Max(0, entry.FolderCount - 1) : 0;
    }

    private static string FormatAccessStatus(StorageEntry entry)
    {
        return entry.IsAccessible ? "Readable" : "Access issue";
    }

    private static string FormatImportance(ImportanceRating rating)
    {
        return rating switch
        {
            ImportanceRating.LikelySafe => "Likely safe",
            ImportanceRating.Caution => "Caution",
            ImportanceRating.HighRisk => "High risk",
            _ => rating.ToString()
        };
    }

    private static string FormatRecommendation(DeletionRecommendation recommendation)
    {
        return recommendation switch
        {
            DeletionRecommendation.Keep => "Keep",
            DeletionRecommendation.Inspect => "Inspect",
            DeletionRecommendation.QuarantineCandidate => "Quarantine candidate",
            DeletionRecommendation.DeleteLater => "Delete later",
            _ => recommendation.ToString()
        };
    }

    private static string FormatCategories(IReadOnlyList<BloatCategory> categories)
    {
        return categories.Count == 0
            ? "None"
            : string.Join("; ", categories.Select(FormatCategory));
    }

    private static string FormatCategory(BloatCategory category)
    {
        return category switch
        {
            BloatCategory.Unknown => "Unknown",
            BloatCategory.CleanupScopeRoot => "Cleanup scope root",
            BloatCategory.ProfileContainer => "Profile container",
            BloatCategory.ApplicationDataArea => "AppData area",
            BloatCategory.BrowserData => "Browser data",
            BloatCategory.CloudSyncData => "Cloud sync data",
            BloatCategory.CredentialData => "Credential data",
            BloatCategory.OldDownload => "Old download",
            BloatCategory.TemporaryFolder => "Temporary folder",
            BloatCategory.InstallerCache => "Installer cache",
            BloatCategory.AppCache => "App cache",
            BloatCategory.GpuShaderCache => "GPU shader cache",
            BloatCategory.DuplicateFileCandidate => "Duplicate file candidate",
            BloatCategory.LargeOldFile => "Large old file",
            BloatCategory.OldGameFile => "Old game file",
            BloatCategory.NodePackageCache => "Node package cache",
            BloatCategory.PythonPackageCache => "Python package cache",
            BloatCategory.WindowsAppData => "Windows app data",
            BloatCategory.WindowsAppLeftover => "Windows app leftover",
            BloatCategory.InstalledApplication => "Installed application",
            BloatCategory.GameData => "Game data",
            BloatCategory.ProtectedLocation => "Protected location",
            BloatCategory.ReparsePoint => "Reparse point",
            BloatCategory.AccessIssue => "Access issue",
            _ => category.ToString()
        };
    }
}
