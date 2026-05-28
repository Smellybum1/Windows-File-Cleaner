using WindowsFileCleaner.Core;

namespace WindowsFileCleaner.App;

public sealed class CategoryFilterOption
{
    public CategoryFilterOption(StorageCategoryFilter filter, string label, string fileNameSegment)
    {
        Filter = filter;
        Label = label;
        FileNameSegment = fileNameSegment;
    }

    public StorageCategoryFilter Filter { get; }
    public string Label { get; }
    public string FileNameSegment { get; }
}
