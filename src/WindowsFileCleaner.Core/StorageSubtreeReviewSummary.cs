namespace WindowsFileCleaner.Core;

public sealed record StorageSubtreeReviewSummary(
    int DescendantRowCount,
    int DescendantFileCount,
    int DescendantFolderCount,
    int LikelySafeCount,
    int CautionCount,
    int HighRiskCount,
    int QuarantineCandidateCount,
    int ProtectedLocationCount,
    int AccessIssueCount,
    int ReparsePointCount,
    int UncategorizedCount,
    long LargestDescendantBytes,
    IReadOnlyList<string> QuarantineCandidateExamples,
    IReadOnlyList<string> ProtectedLocationExamples,
    IReadOnlyList<string> AccessIssueExamples,
    IReadOnlyList<string> UncategorizedExamples)
{
    public string LargestDescendantSizeDisplay => ByteSizeFormatter.Format(LargestDescendantBytes);
}
