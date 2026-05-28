using WindowsFileCleaner.Core;

namespace WindowsFileCleaner.App;

public sealed class SizeThresholdFilterOption
{
    public SizeThresholdFilterOption(StorageSizeThresholdFilter filter, string label, string fileNameSegment)
    {
        Filter = filter;
        Label = label;
        FileNameSegment = fileNameSegment;
    }

    public StorageSizeThresholdFilter Filter { get; }
    public string Label { get; }
    public string FileNameSegment { get; }
}
