namespace WindowsFileCleaner.Core;

public sealed class CleanupCandidateClassifier
{
    private static readonly string[] ProtectedFolderNames =
    [
        "Desktop",
        "Documents",
        "Pictures",
        "Videos",
        "Music",
        "Favorites",
        "Contacts",
        "Saved Games"
    ];

    private static readonly string[] BrowserProfileHints =
    [
        @"\AppData\Local\Google\Chrome\User Data",
        @"\AppData\Local\Microsoft\Edge\User Data",
        @"\AppData\Roaming\Mozilla\Firefox\Profiles",
        @"\AppData\Roaming\Opera Software",
        @"\AppData\Local\BraveSoftware\Brave-Browser\User Data"
    ];

    private static readonly string[] CodexHints =
    [
        @"\.codex",
        @"\codex-runtimes",
        @"\AppData\Local\Programs\Codex",
        @"\AppData\Roaming\Codex"
    ];

    private static readonly string[] SourceCodeHints =
    [
        @"\source",
        @"\sources",
        @"\repos",
        @"\repositories",
        @"\projects",
        @"\workspace",
        @"\workspaces",
        @"\dev"
    ];

    public ClassifiedPath Classify(PathSnapshot path)
    {
        var categories = new HashSet<BloatCategory>();
        var evidence = new List<string>();

        if (!path.IsAccessible)
        {
            categories.Add(BloatCategory.AccessIssue);
            evidence.Add("The path could not be fully read.");
            return Build(categories, ImportanceRating.Caution, DeletionRecommendation.Inspect, evidence);
        }

        if (path.IsReparsePoint)
        {
            categories.Add(BloatCategory.ReparsePoint);
            evidence.Add("The path is a reparse point, so the scanner did not follow it.");
            return Build(categories, ImportanceRating.Caution, DeletionRecommendation.Inspect, evidence);
        }

        if (IsProtected(path))
        {
            categories.Add(BloatCategory.ProtectedLocation);
            evidence.Add("This path overlaps a protected location such as profile data, browser data, source code, game saves, or Codex-related files.");
            return Build(categories, ImportanceRating.HighRisk, DeletionRecommendation.Keep, evidence);
        }

        AddCategoryHints(path, categories, evidence);

        if (categories.Count == 0)
        {
            evidence.Add(path.IsDirectory
                ? "No cleanup-specific category matched this folder."
                : "No cleanup-specific category matched this file.");
            return Build(categories, ImportanceRating.Caution, DeletionRecommendation.Inspect, evidence);
        }

        if (IsCautionCategory(categories, path))
        {
            return Build(categories, ImportanceRating.Caution, DeletionRecommendation.Inspect, evidence);
        }

        return Build(categories, ImportanceRating.LikelySafe, DeletionRecommendation.QuarantineCandidate, evidence);
    }

    private static void AddCategoryHints(PathSnapshot path, HashSet<BloatCategory> categories, List<string> evidence)
    {
        var name = path.Name;
        var fullPath = path.FullPath;

        if (ContainsSegment(fullPath, "Downloads") && IsOld(path.LastModifiedUtc, TimeSpan.FromDays(90)))
        {
            categories.Add(BloatCategory.OldDownload);
            evidence.Add("This is in Downloads and has not changed recently.");
        }

        if (IsTemporaryName(name) || ContainsSegment(fullPath, "Temp") || ContainsSegment(fullPath, "tmp"))
        {
            categories.Add(BloatCategory.TemporaryFolder);
            evidence.Add("The name or path looks temporary.");
        }

        if (IsInstaller(path))
        {
            categories.Add(BloatCategory.InstallerCache);
            evidence.Add("The file or folder looks like an installer or installer cache.");
        }

        if (LooksLikeAppCache(fullPath, name))
        {
            categories.Add(BloatCategory.AppCache);
            evidence.Add("The path looks like an application cache.");
        }

        if (ContainsSegment(fullPath, "node_modules") || ContainsSegment(fullPath, ".npm") || ContainsSegment(fullPath, "npm-cache"))
        {
            categories.Add(BloatCategory.NodePackageCache);
            evidence.Add("The path looks like a Node package cache or dependency folder.");
        }

        if (ContainsSegment(fullPath, "__pycache__") || ContainsSegment(fullPath, ".pytest_cache") || ContainsSegment(fullPath, "pip") && ContainsSegment(fullPath, "Cache"))
        {
            categories.Add(BloatCategory.PythonPackageCache);
            evidence.Add("The path looks like a Python cache.");
        }

        if (ContainsSegment(fullPath, "Saved Games") || ContainsSegment(fullPath, "Games") && IsOld(path.LastModifiedUtc, TimeSpan.FromDays(180)))
        {
            categories.Add(BloatCategory.OldGameFile);
            evidence.Add("The path looks game-related and has not changed recently.");
        }

        if (fullPath.Contains(@"\AppData\Local\Packages\", StringComparison.OrdinalIgnoreCase)
            && IsOld(path.LastModifiedUtc, TimeSpan.FromDays(180)))
        {
            categories.Add(BloatCategory.WindowsAppLeftover);
            evidence.Add("The path looks like a stale Windows app package area.");
        }
    }

    private static ClassifiedPath Build(
        HashSet<BloatCategory> categories,
        ImportanceRating importanceRating,
        DeletionRecommendation deletionRecommendation,
        List<string> evidence)
    {
        return new ClassifiedPath(
            categories.OrderBy(category => category.ToString()).ToArray(),
            importanceRating,
            deletionRecommendation,
            string.Join(" ", evidence.Distinct(StringComparer.OrdinalIgnoreCase)));
    }

    private static bool IsCautionCategory(HashSet<BloatCategory> categories, PathSnapshot path)
    {
        if (categories.Contains(BloatCategory.NodePackageCache)
            || categories.Contains(BloatCategory.PythonPackageCache)
            || categories.Contains(BloatCategory.OldGameFile)
            || categories.Contains(BloatCategory.WindowsAppLeftover))
        {
            return true;
        }

        return path.FullPath.Contains(@"\AppData\", StringComparison.OrdinalIgnoreCase)
            && !categories.Contains(BloatCategory.TemporaryFolder);
    }

    private static bool IsProtected(PathSnapshot path)
    {
        return ProtectedFolderNames.Any(name => ContainsSegment(path.FullPath, name))
            || BrowserProfileHints.Any(hint => path.FullPath.Contains(hint, StringComparison.OrdinalIgnoreCase))
            || CodexHints.Any(hint => path.FullPath.Contains(hint, StringComparison.OrdinalIgnoreCase))
            || SourceCodeHints.Any(hint => path.FullPath.Contains(hint, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsTemporaryName(string name)
    {
        return name.Equals("temp", StringComparison.OrdinalIgnoreCase)
            || name.Equals("tmp", StringComparison.OrdinalIgnoreCase)
            || name.StartsWith("tmp", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsInstaller(PathSnapshot path)
    {
        return path.Name.EndsWith(".msi", StringComparison.OrdinalIgnoreCase)
            || path.Name.EndsWith(".msix", StringComparison.OrdinalIgnoreCase)
            || path.Name.EndsWith(".msu", StringComparison.OrdinalIgnoreCase)
            || path.Name.Contains("installer", StringComparison.OrdinalIgnoreCase)
            || path.Name.Contains("setup", StringComparison.OrdinalIgnoreCase);
    }

    private static bool LooksLikeAppCache(string fullPath, string name)
    {
        return ContainsSegment(fullPath, "Cache")
            || ContainsSegment(fullPath, "Caches")
            || ContainsSegment(fullPath, "Code Cache")
            || name.Equals(".cache", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("cache", StringComparison.OrdinalIgnoreCase);
    }

    private static bool ContainsSegment(string fullPath, string segment)
    {
        var normalized = fullPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        return normalized.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
            .Any(part => part.Equals(segment, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsOld(DateTimeOffset? lastModifiedUtc, TimeSpan threshold)
    {
        return lastModifiedUtc is not null && DateTimeOffset.UtcNow - lastModifiedUtc.Value > threshold;
    }
}

