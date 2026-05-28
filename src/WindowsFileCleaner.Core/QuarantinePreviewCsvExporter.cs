using System.Text;

namespace WindowsFileCleaner.Core;

public static class QuarantinePreviewCsvExporter
{
    public static string Export(QuarantinePreview preview)
    {
        var builder = new StringBuilder();
        AppendRow(
            builder,
            [
                "Cleanup scope",
                "Quarantine root",
                "Disposition",
                "Source path",
                "Destination path",
                "Name",
                "Type",
                "Size bytes",
                "Size",
                "Importance",
                "Recommendation",
                "Categories",
                "Reasons",
                "Evidence",
                "Access status",
                "Access issue",
                "Preview note"
            ]);

        foreach (var entry in preview.Entries)
        {
            AppendRow(
                builder,
                [
                    preview.CleanupScopePath,
                    preview.QuarantineRootPath,
                    FormatDisposition(entry.Disposition),
                    entry.SourcePath,
                    entry.DestinationPath ?? "",
                    entry.Entry.Name,
                    entry.Entry.IsDirectory ? "Folder" : "File",
                    entry.Entry.SizeBytes.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    entry.Entry.SizeDisplay,
                    FormatImportance(entry.Entry.ImportanceRating),
                    FormatRecommendation(entry.Entry.DeletionRecommendation),
                    FormatCategories(entry.Entry.BloatCategories),
                    string.Join("; ", entry.Reasons),
                    entry.Entry.Evidence,
                    FormatAccessStatus(entry.Entry),
                    entry.Entry.ErrorMessage ?? "",
                    "No files were modified."
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

    private static string FormatDisposition(QuarantinePreviewDisposition disposition)
    {
        return disposition switch
        {
            QuarantinePreviewDisposition.Included => "Included",
            QuarantinePreviewDisposition.Blocked => "Blocked",
            QuarantinePreviewDisposition.Redundant => "Redundant",
            _ => disposition.ToString()
        };
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
