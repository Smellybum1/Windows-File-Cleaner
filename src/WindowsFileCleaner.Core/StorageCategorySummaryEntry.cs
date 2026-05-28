namespace WindowsFileCleaner.Core;

public sealed record StorageCategorySummaryEntry(
    BloatCategory Category,
    int Count,
    long LargestEntryBytes);
