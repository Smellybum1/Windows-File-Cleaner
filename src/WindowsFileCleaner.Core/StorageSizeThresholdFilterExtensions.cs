namespace WindowsFileCleaner.Core;

public static class StorageSizeThresholdFilterExtensions
{
    private const long OneMegabyte = 1024L * 1024;
    private const long OneGigabyte = 1024L * 1024 * 1024;

    public static long GetMinimumSizeBytes(this StorageSizeThresholdFilter filter)
    {
        return filter switch
        {
            StorageSizeThresholdFilter.AtLeast1Mb => OneMegabyte,
            StorageSizeThresholdFilter.AtLeast100Mb => 100L * OneMegabyte,
            StorageSizeThresholdFilter.AtLeast1Gb => OneGigabyte,
            StorageSizeThresholdFilter.AtLeast5Gb => 5L * OneGigabyte,
            StorageSizeThresholdFilter.AtLeast10Gb => 10L * OneGigabyte,
            _ => 0
        };
    }
}
