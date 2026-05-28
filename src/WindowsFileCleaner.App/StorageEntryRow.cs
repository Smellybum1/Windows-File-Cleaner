using WindowsFileCleaner.Core;

namespace WindowsFileCleaner.App;

public sealed class StorageEntryRow
{
    public StorageEntryRow(StorageEntry entry, int depth)
    {
        Entry = entry;
        Depth = depth;
    }

    public StorageEntryRow(StorageReviewEntry reviewEntry)
        : this(reviewEntry.Entry, reviewEntry.Depth)
    {
    }

    public StorageEntry Entry { get; }
    public int Depth { get; }
    public string Name => $"{new string(' ', Depth * 2)}{Entry.Name}";
    public string FullPath => Entry.FullPath;
    public string Type => Entry.IsDirectory ? "Folder" : "File";
    public string Size => Entry.SizeDisplay;
    public long SizeBytes => Entry.SizeBytes;
    public string Importance => FormatImportance(Entry.ImportanceRating);
    public string Recommendation => FormatRecommendation(Entry.DeletionRecommendation);
    public string Categories => Entry.BloatCategories.Count == 0
        ? "None"
        : string.Join(", ", Entry.BloatCategories.Select(FormatCategory));
    public string LastModified => Entry.LastModifiedUtc?.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "Unknown";
    public string Evidence => Entry.Evidence;
    public string Error => Entry.ErrorMessage ?? "";

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

    private static string FormatCategory(BloatCategory category)
    {
        return category switch
        {
            BloatCategory.Unknown => "Unknown",
            BloatCategory.ProfileContainer => "Profile container",
            BloatCategory.ApplicationDataArea => "AppData area",
            BloatCategory.BrowserData => "Browser data",
            BloatCategory.OldDownload => "Old download",
            BloatCategory.TemporaryFolder => "Temporary folder",
            BloatCategory.InstallerCache => "Installer cache",
            BloatCategory.AppCache => "App cache",
            BloatCategory.GpuShaderCache => "GPU shader cache",
            BloatCategory.DuplicateFileCandidate => "Duplicate file candidate",
            BloatCategory.OldGameFile => "Old game file",
            BloatCategory.NodePackageCache => "Node package cache",
            BloatCategory.PythonPackageCache => "Python package cache",
            BloatCategory.WindowsAppLeftover => "Windows app leftover",
            BloatCategory.ProtectedLocation => "Protected location",
            BloatCategory.ReparsePoint => "Reparse point",
            BloatCategory.AccessIssue => "Access issue",
            _ => category.ToString()
        };
    }
}
