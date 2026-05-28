using WindowsFileCleaner.Core;

namespace WindowsFileCleaner.App;

public sealed class EntryTypeFilterOption
{
    public EntryTypeFilterOption(StorageEntryTypeFilter filter, string label, string fileNameSegment)
    {
        Filter = filter;
        Label = label;
        FileNameSegment = fileNameSegment;
    }

    public StorageEntryTypeFilter Filter { get; }
    public string Label { get; }
    public string FileNameSegment { get; }
}
