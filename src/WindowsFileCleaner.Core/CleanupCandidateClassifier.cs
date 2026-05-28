namespace WindowsFileCleaner.Core;

public sealed class CleanupCandidateClassifier
{
    private const long LargeFileThresholdBytes = 1024L * 1024 * 1024;
    private static readonly TimeSpan LargeOldFileAge = TimeSpan.FromDays(90);

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

    private static readonly string[] BrowserContainerNames =
    [
        "Google",
        "Chrome",
        "Edge",
        "Firefox",
        "Mozilla",
        "Brave-Browser",
        "Opera Software",
        "User Data"
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

    private static readonly string[] WindowsAppDataHints =
    [
        @"\AppData\Local\Packages"
    ];

    private static readonly string[] InstalledApplicationHints =
    [
        @"\AppData\Local\Programs"
    ];

    private static readonly string[] GameDataHints =
    [
        @"\AppData\Local\Larian Studios",
        @"\AppData\LocalLow\Larian Studios",
        @"\AppData\Local\Paradox Interactive",
        @"\AppData\Roaming\Paradox Interactive",
        "Baldur's Gate 3",
        "Stellaris",
        "IronyMod",
        "IronyModManager"
    ];

    public ClassifiedPath Classify(PathSnapshot path, long sizeBytes = 0, bool isCleanupScopeRoot = false)
    {
        var categories = new HashSet<BloatCategory>();
        var evidence = new List<string>();

        if (isCleanupScopeRoot)
        {
            categories.Add(BloatCategory.CleanupScopeRoot);
            categories.Add(BloatCategory.ProtectedLocation);
            evidence.Add("This is the Cleanup Scope root. Review child rows instead of cleaning the root itself.");
        }

        if (!path.IsAccessible)
        {
            categories.Add(BloatCategory.AccessIssue);
            evidence.Add("The path could not be fully read.");
            return Build(
                categories,
                isCleanupScopeRoot ? ImportanceRating.HighRisk : ImportanceRating.Caution,
                isCleanupScopeRoot ? DeletionRecommendation.Keep : DeletionRecommendation.Inspect,
                evidence);
        }

        if (path.IsReparsePoint)
        {
            categories.Add(BloatCategory.ReparsePoint);
            evidence.Add("The path is a reparse point, so the scanner did not follow it.");
            return Build(
                categories,
                isCleanupScopeRoot ? ImportanceRating.HighRisk : ImportanceRating.Caution,
                isCleanupScopeRoot ? DeletionRecommendation.Keep : DeletionRecommendation.Inspect,
                evidence);
        }

        AddCategoryHints(path, sizeBytes, categories, evidence);

        if (isCleanupScopeRoot)
        {
            return Build(categories, ImportanceRating.HighRisk, DeletionRecommendation.Keep, evidence);
        }

        if (IsProtected(path, categories))
        {
            categories.Add(BloatCategory.ProtectedLocation);
            evidence.Add("This path overlaps a protected location such as profile data, browser data, source code, game saves, or Codex-related files.");
            return Build(categories, ImportanceRating.HighRisk, DeletionRecommendation.Keep, evidence);
        }

        if (categories.Count == 0)
        {
            evidence.Add(path.IsDirectory
                ? "No cleanup-specific category matched this folder."
                : "No cleanup-specific category matched this file.");
            return Build(categories, ImportanceRating.Caution, DeletionRecommendation.Inspect, evidence);
        }

        if (categories.Contains(BloatCategory.ProfileContainer)
            || categories.Contains(BloatCategory.BrowserData))
        {
            return Build(categories, ImportanceRating.HighRisk, DeletionRecommendation.Keep, evidence);
        }

        if (IsCautionCategory(categories, path))
        {
            return Build(categories, ImportanceRating.Caution, DeletionRecommendation.Inspect, evidence);
        }

        return Build(categories, ImportanceRating.LikelySafe, DeletionRecommendation.QuarantineCandidate, evidence);
    }

    private static void AddCategoryHints(PathSnapshot path, long sizeBytes, HashSet<BloatCategory> categories, List<string> evidence)
    {
        var name = path.Name;
        var fullPath = path.FullPath;

        if (IsProfileContainer(path))
        {
            categories.Add(BloatCategory.ProfileContainer);
            evidence.Add("This is a profile container. Review child folders instead of cleaning the container itself.");
        }

        if (ContainsSegment(fullPath, "AppData"))
        {
            categories.Add(BloatCategory.ApplicationDataArea);
            evidence.Add("This is inside AppData, which mixes caches with active app settings and should be reviewed conservatively.");
        }

        if (LooksLikeBrowserData(path))
        {
            categories.Add(BloatCategory.BrowserData);
            evidence.Add("This looks browser-related and may include profiles, history, sessions, extensions, or credentials.");
        }

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

        if (LooksLikeLargeOldFile(path, sizeBytes))
        {
            categories.Add(BloatCategory.LargeOldFile);
            evidence.Add("This is a large file that has not changed recently. Inspect it manually before considering cleanup.");
        }

        if (LooksLikeAppCache(fullPath, name))
        {
            categories.Add(BloatCategory.AppCache);
            evidence.Add("The path looks like an application cache.");
        }

        if (LooksLikeGpuShaderCache(fullPath, name))
        {
            categories.Add(BloatCategory.GpuShaderCache);
            evidence.Add("The path looks like a GPU shader cache that applications can usually rebuild, but cleanup may cause temporary recompile delays.");
        }

        if (ContainsSegment(fullPath, "node_modules") || ContainsSegment(fullPath, ".npm") || ContainsSegment(fullPath, "npm-cache"))
        {
            categories.Add(BloatCategory.NodePackageCache);
            evidence.Add("The path looks like a Node package cache or dependency folder.");
        }

        if (ContainsSegment(fullPath, "__pycache__")
            || ContainsSegment(fullPath, ".pytest_cache")
            || ContainsSegment(fullPath, "pip")
            || ContainsSegment(fullPath, "pip-cache"))
        {
            categories.Add(BloatCategory.PythonPackageCache);
            evidence.Add("The path looks like a Python cache.");
        }

        if (ContainsSegment(fullPath, "Saved Games") || ContainsSegment(fullPath, "Games") && IsOld(path.LastModifiedUtc, TimeSpan.FromDays(180)))
        {
            categories.Add(BloatCategory.OldGameFile);
            evidence.Add("The path looks game-related and has not changed recently.");
        }

        if (LooksLikeGameData(fullPath))
        {
            categories.Add(BloatCategory.GameData);
            evidence.Add("The path looks like game saves, mods, profiles, or game configuration. Treat it as active user data unless reviewed manually.");
        }

        if (LooksLikeWindowsAppData(fullPath))
        {
            categories.Add(BloatCategory.WindowsAppData);
            evidence.Add("The path is under the Windows app package data area, which can contain active app settings and local state.");
        }

        if (LooksLikeWindowsAppData(fullPath)
            && IsOld(path.LastModifiedUtc, TimeSpan.FromDays(180)))
        {
            categories.Add(BloatCategory.WindowsAppLeftover);
            evidence.Add("The path looks like a stale Windows app package area.");
        }

        if (LooksLikeInstalledApplication(fullPath))
        {
            categories.Add(BloatCategory.InstalledApplication);
            evidence.Add("The path is under the per-user installed applications area and removing it could break an installed app.");
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
        if (categories.Contains(BloatCategory.LargeOldFile)
            && !HasLikelySafeCleanupCategory(categories))
        {
            return true;
        }

        if (categories.Contains(BloatCategory.NodePackageCache)
            || categories.Contains(BloatCategory.PythonPackageCache)
            || categories.Contains(BloatCategory.OldGameFile)
            || categories.Contains(BloatCategory.WindowsAppData)
            || categories.Contains(BloatCategory.WindowsAppLeftover)
            || categories.Contains(BloatCategory.InstalledApplication)
            || categories.Contains(BloatCategory.GameData)
            || categories.Contains(BloatCategory.ApplicationDataArea)
            || categories.Contains(BloatCategory.GpuShaderCache))
        {
            return true;
        }

        return path.FullPath.Contains(@"\AppData\", StringComparison.OrdinalIgnoreCase)
            && !categories.Contains(BloatCategory.TemporaryFolder);
    }

    private static bool IsProtected(PathSnapshot path, HashSet<BloatCategory> categories)
    {
        return categories.Contains(BloatCategory.WindowsAppData)
            || categories.Contains(BloatCategory.InstalledApplication)
            || categories.Contains(BloatCategory.GameData)
            || ProtectedFolderNames.Any(name => ContainsSegment(path.FullPath, name))
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

    private static bool IsProfileContainer(PathSnapshot path)
    {
        if (!path.IsDirectory)
        {
            return false;
        }

        var parent = Directory.GetParent(path.FullPath);
        return parent is not null
            && parent.FullName.Equals(@"C:\Users", StringComparison.OrdinalIgnoreCase);
    }

    private static bool LooksLikeBrowserData(PathSnapshot path)
    {
        return BrowserProfileHints.Any(hint => path.FullPath.Contains(hint, StringComparison.OrdinalIgnoreCase))
            || BrowserContainerNames.Any(name => ContainsSegment(path.FullPath, name));
    }

    private static bool IsInstaller(PathSnapshot path)
    {
        return path.Name.EndsWith(".msi", StringComparison.OrdinalIgnoreCase)
            || path.Name.EndsWith(".msix", StringComparison.OrdinalIgnoreCase)
            || path.Name.EndsWith(".msu", StringComparison.OrdinalIgnoreCase)
            || path.Name.Contains("installer", StringComparison.OrdinalIgnoreCase)
            || path.Name.Contains("setup", StringComparison.OrdinalIgnoreCase);
    }

    private static bool LooksLikeLargeOldFile(PathSnapshot path, long sizeBytes)
    {
        return !path.IsDirectory
            && sizeBytes >= LargeFileThresholdBytes
            && IsOld(path.LastModifiedUtc, LargeOldFileAge);
    }

    private static bool HasLikelySafeCleanupCategory(HashSet<BloatCategory> categories)
    {
        return categories.Contains(BloatCategory.OldDownload)
            || categories.Contains(BloatCategory.TemporaryFolder)
            || categories.Contains(BloatCategory.InstallerCache)
            || categories.Contains(BloatCategory.AppCache);
    }

    private static bool LooksLikeAppCache(string fullPath, string name)
    {
        return ContainsSegment(fullPath, "Cache")
            || ContainsSegment(fullPath, "Caches")
            || ContainsSegment(fullPath, "Code Cache")
            || name.Equals(".cache", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("cache", StringComparison.OrdinalIgnoreCase);
    }

    private static bool LooksLikeGpuShaderCache(string fullPath, string name)
    {
        return ContainsSegment(fullPath, "DXCache")
            || ContainsSegment(fullPath, "GLCache")
            || ContainsSegment(fullPath, "D3DSCache")
            || name.Contains("shadercache", StringComparison.OrdinalIgnoreCase)
            || fullPath.Contains(@"\NVIDIA\", StringComparison.OrdinalIgnoreCase) && LooksLikeAppCache(fullPath, name);
    }

    private static bool LooksLikeWindowsAppData(string fullPath)
    {
        return WindowsAppDataHints.Any(hint => fullPath.Contains(hint, StringComparison.OrdinalIgnoreCase));
    }

    private static bool LooksLikeInstalledApplication(string fullPath)
    {
        return InstalledApplicationHints.Any(hint => fullPath.Contains(hint, StringComparison.OrdinalIgnoreCase));
    }

    private static bool LooksLikeGameData(string fullPath)
    {
        return GameDataHints.Any(hint => fullPath.Contains(hint, StringComparison.OrdinalIgnoreCase));
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
