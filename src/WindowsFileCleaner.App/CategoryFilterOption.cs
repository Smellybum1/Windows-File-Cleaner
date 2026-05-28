using WindowsFileCleaner.Core;

namespace WindowsFileCleaner.App;

public sealed class CategoryFilterOption
{
    public CategoryFilterOption(BloatCategory? category, string label, string fileNameSegment)
    {
        Category = category;
        Label = label;
        FileNameSegment = fileNameSegment;
    }

    public BloatCategory? Category { get; }
    public string Label { get; }
    public string FileNameSegment { get; }
}
