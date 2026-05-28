namespace WindowsFileCleaner.Core;

public static class PathSafety
{
    public static string GetFullPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Cleanup Scope path is required.", nameof(path));
        }

        return Path.GetFullPath(path.Trim());
    }

    public static bool IsWithinScope(string scopePath, string candidatePath)
    {
        var scope = EnsureTrailingSeparator(GetFullPath(scopePath));
        var candidate = GetFullPath(candidatePath);

        return candidate.Equals(scope.TrimEnd(Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase)
            || candidate.StartsWith(scope, StringComparison.OrdinalIgnoreCase);
    }

    private static string EnsureTrailingSeparator(string path)
    {
        return path.EndsWith(Path.DirectorySeparatorChar)
            ? path
            : path + Path.DirectorySeparatorChar;
    }
}

