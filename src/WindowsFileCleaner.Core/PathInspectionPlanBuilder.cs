namespace WindowsFileCleaner.Core;

public static class PathInspectionPlanBuilder
{
    public static PathInspectionPlan Build(StorageEntry entry)
    {
        var path = PathSafety.GetFullPath(entry.FullPath);
        var arguments = entry.IsDirectory
            ? Quote(path)
            : $"/select,{Quote(path)}";

        return new PathInspectionPlan(path, "explorer.exe", arguments);
    }

    private static string Quote(string value)
    {
        return $"\"{value.Replace("\"", "\\\"", StringComparison.Ordinal)}\"";
    }
}

