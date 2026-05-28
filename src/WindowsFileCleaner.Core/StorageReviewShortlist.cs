namespace WindowsFileCleaner.Core;

public sealed class StorageReviewShortlist
{
    private readonly HashSet<string> _paths = new(StringComparer.OrdinalIgnoreCase);

    public int Count => _paths.Count;

    public bool Add(StorageEntry entry)
    {
        return _paths.Add(entry.FullPath);
    }

    public int AddMany(IEnumerable<StorageEntry> entries)
    {
        var addedCount = 0;
        foreach (var entry in entries)
        {
            if (Add(entry))
            {
                addedCount++;
            }
        }

        return addedCount;
    }

    public bool Remove(StorageEntry entry)
    {
        return _paths.Remove(entry.FullPath);
    }

    public bool Contains(StorageEntry entry)
    {
        return _paths.Contains(entry.FullPath);
    }

    public void Clear()
    {
        _paths.Clear();
    }

    public IReadOnlyList<StorageReviewEntry> ApplyTo(IReadOnlyList<StorageReviewEntry> entries)
    {
        return entries.Where(row => Contains(row.Entry)).ToArray();
    }
}
