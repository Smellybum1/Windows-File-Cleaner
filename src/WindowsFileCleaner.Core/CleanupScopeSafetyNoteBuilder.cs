namespace WindowsFileCleaner.Core;

public static class CleanupScopeSafetyNoteBuilder
{
    private const string FixtureScopeMarker = "storage-scan-smoke-fixture";
    private const string LocalFixtureSegment = ".local";
    private const string TestFixtureSegment = "test-fixtures";

    public static CleanupScopeSafetyNote Build(string cleanupScopePath)
    {
        if (string.IsNullOrWhiteSpace(cleanupScopePath))
        {
            return new CleanupScopeSafetyNote(
                "Choose Cleanup Scope",
                "Enter the folder to review. Run fixture review before scanning real user files.",
                IsFixtureScope: false,
                IsRealUserProfileScope: false);
        }

        string fullPath;
        try
        {
            fullPath = PathSafety.GetFullPath(cleanupScopePath);
        }
        catch
        {
            return new CleanupScopeSafetyNote(
                "Check Cleanup Scope",
                "This path cannot be normalized yet. Fix the path before scanning.",
                IsFixtureScope: false,
                IsRealUserProfileScope: false);
        }

        if (LooksLikeFixtureScope(fullPath))
        {
            return new CleanupScopeSafetyNote(
                "Fixture Cleanup Scope",
                "Use this synthetic scope for fixture review first. The app still waits for you to click Scan and remains read-only.",
                IsFixtureScope: true,
                IsRealUserProfileScope: false);
        }

        if (IsRealUserProfileScope(fullPath))
        {
            return new CleanupScopeSafetyNote(
                "Real Profile Cleanup Scope",
                "Run MVP preflight and the fixture review before scanning this real profile. Storage Scan is read-only and does not clean files.",
                IsFixtureScope: false,
                IsRealUserProfileScope: true);
        }

        return new CleanupScopeSafetyNote(
            "Custom Cleanup Scope",
            "Verify this path is intentional and run fixture review before scanning real user files. Storage Scan is read-only.",
            IsFixtureScope: false,
            IsRealUserProfileScope: false);
    }

    private static bool LooksLikeFixtureScope(string fullPath)
    {
        return ContainsSegment(fullPath, FixtureScopeMarker)
            || ContainsSegment(fullPath, TestFixtureSegment)
            || ContainsSegment(fullPath, LocalFixtureSegment) && fullPath.Contains(FixtureScopeMarker, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsRealUserProfileScope(string fullPath)
    {
        var defaultProfile = StorageScanOptions.DefaultForCurrentUser().CleanupScopePath;
        return PathSafety.IsWithinScope(defaultProfile, fullPath);
    }

    private static bool ContainsSegment(string fullPath, string segment)
    {
        var normalized = fullPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        return normalized.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
            .Any(part => part.Equals(segment, StringComparison.OrdinalIgnoreCase));
    }
}
