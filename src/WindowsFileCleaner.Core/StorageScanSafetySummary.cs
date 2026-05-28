namespace WindowsFileCleaner.Core;

public sealed record StorageScanSafetySummary(
    string CleanupScopePath,
    int TotalEntries,
    int HighRiskCount,
    int ProtectedLocationCount,
    int AccessIssueCount,
    int ReparsePointCount,
    int QuarantineCandidateCount,
    int UncategorizedCount,
    IReadOnlyList<string> AccessIssueExamples,
    IReadOnlyList<string> Notes)
{
    public bool HasReviewWarnings => HighRiskCount > 0
        || ProtectedLocationCount > 0
        || AccessIssueCount > 0
        || ReparsePointCount > 0
        || QuarantineCandidateCount > 0
        || UncategorizedCount > 0;

    public string StatusLabel => HasReviewWarnings ? "Review needed" : "Read-only scan complete";
}
