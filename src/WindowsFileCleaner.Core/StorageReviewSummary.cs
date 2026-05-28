namespace WindowsFileCleaner.Core;

public sealed record StorageReviewSummary(
    int TotalEntries,
    int LikelySafeCount,
    int CautionCount,
    int HighRiskCount,
    int QuarantineCandidateCount,
    long LargestEntryBytes,
    long LikelySafeLargestEntryBytes,
    long CautionLargestEntryBytes,
    long HighRiskLargestEntryBytes,
    long QuarantineCandidateLargestEntryBytes);
