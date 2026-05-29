namespace WindowsFileCleaner.Core;

public static class StorageHotspotTrailBuilder
{
    public static IReadOnlyList<StorageHotspotTrailEntry> Build(StorageEntry entry, int maxDepth = 8)
    {
        if (maxDepth <= 0 || !entry.IsDirectory || entry.Children.Count == 0)
        {
            return [];
        }

        var trail = new List<StorageHotspotTrailEntry>();
        var current = entry;

        for (var level = 1; level <= maxDepth; level++)
        {
            var child = current.Children
                .OrderByDescending(candidate => candidate.SizeBytes)
                .ThenBy(candidate => candidate.Name, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault();
            if (child is null)
            {
                break;
            }

            trail.Add(new StorageHotspotTrailEntry(
                level,
                child.Name,
                child.FullPath,
                child.IsDirectory,
                child.SizeBytes,
                child.ImportanceRating,
                child.DeletionRecommendation,
                child.BloatCategories));

            if (!child.IsDirectory || child.Children.Count == 0)
            {
                break;
            }

            current = child;
        }

        return trail;
    }
}
