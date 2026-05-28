using System.IO;
using WindowsFileCleaner.Core;

namespace WindowsFileCleaner.App;

public sealed class StorageEntryRow
{
    public StorageEntryRow(StorageEntry entry, int depth, bool isShortlisted = false, string? cleanupScopePath = null)
    {
        Entry = entry;
        Depth = depth;
        IsShortlisted = isShortlisted;
        RelativePath = FormatRelativePath(entry.FullPath, cleanupScopePath);
    }

    public StorageEntryRow(StorageReviewEntry reviewEntry, bool isShortlisted = false, string? cleanupScopePath = null)
        : this(reviewEntry.Entry, reviewEntry.Depth, isShortlisted, cleanupScopePath)
    {
    }

    public StorageEntry Entry { get; }
    public int Depth { get; }
    public bool IsShortlisted { get; }
    public string Shortlist => IsShortlisted ? "Yes" : "";
    public string Name => $"{new string(' ', Depth * 2)}{Entry.Name}";
    public string RelativePath { get; }
    public string ParentLocation => FormatParentLocation(Entry.FullPath);
    public string FullPath => Entry.FullPath;
    public string Type => Entry.IsDirectory ? "Folder" : "File";
    public string Size => Entry.SizeDisplay;
    public long SizeBytes => Entry.SizeBytes;
    public int ContainedFileCount => Entry.FileCount;
    public int ContainedFolderCount => Entry.IsDirectory ? Math.Max(0, Entry.FolderCount - 1) : 0;
    public int ContainedTotalCount => ContainedFileCount + ContainedFolderCount;
    public string Contents => Entry.IsDirectory
        ? $"{FormatCount(ContainedFileCount, "file")} | {FormatCount(ContainedFolderCount, "folder")} inside"
        : "Single file";
    public string Importance => FormatImportance(Entry.ImportanceRating);
    public string Recommendation => FormatRecommendation(Entry.DeletionRecommendation);
    public string Categories => Entry.BloatCategories.Count == 0
        ? "None"
        : string.Join(", ", Entry.BloatCategories.Select(FormatCategory));
    public string AccessStatus => Entry.IsAccessible ? "Readable" : "Access issue";
    public string LastModified => Entry.LastModifiedUtc?.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "Unknown";
    public string Evidence => Entry.Evidence;
    public string Error => Entry.ErrorMessage ?? "";

    private static string FormatParentLocation(string fullPath)
    {
        try
        {
            var parent = Path.GetDirectoryName(fullPath);
            return string.IsNullOrWhiteSpace(parent) ? "(none)" : parent;
        }
        catch (ArgumentException)
        {
            return "(unknown)";
        }
    }

    private static string FormatRelativePath(string fullPath, string? cleanupScopePath)
    {
        if (string.IsNullOrWhiteSpace(cleanupScopePath))
        {
            return "";
        }

        try
        {
            return PathSafety.IsWithinScope(cleanupScopePath, fullPath)
                ? Path.GetRelativePath(PathSafety.GetFullPath(cleanupScopePath), fullPath)
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

    private static string FormatCount(int count, string singular)
    {
        var label = count == 1 ? singular : singular + "s";
        return $"{count:N0} {label}";
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
