namespace WindowsFileCleaner.Core;

public sealed record StorageReviewSummary(
    int TotalEntries,
    int LikelySafeCount,
    int CautionCount,
    int HighRiskCount,
    int QuarantineCandidateCount,
    long LikelySafeBytes,
    long CautionBytes,
    long HighRiskBytes,
    long QuarantineCandidateBytes);

