namespace WindowsFileCleaner.Core;

public static class StorageScanSafetySummaryBuilder
{
    public static StorageScanSafetySummary Build(StorageScanResult result, StorageScanReview review)
    {
        var highRiskCount = review.Entries.Count(row => row.Entry.ImportanceRating == ImportanceRating.HighRisk);
        var protectedLocationCount = review.Entries.Count(row => row.Entry.BloatCategories.Contains(BloatCategory.ProtectedLocation));
        var accessIssueCount = review.Entries.Count(row => IsAccessIssue(row.Entry));
        var reparsePointCount = review.Entries.Count(row => row.Entry.IsReparsePoint || row.Entry.BloatCategories.Contains(BloatCategory.ReparsePoint));
        var quarantineCandidateCount = review.Entries.Count(row => row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate);
        var uncategorizedCount = review.Entries.Count(row => row.Entry.BloatCategories.Count == 0);

        return new StorageScanSafetySummary(
            result.CleanupScopePath,
            review.Entries.Count,
            highRiskCount,
            protectedLocationCount,
            accessIssueCount,
            reparsePointCount,
            quarantineCandidateCount,
            uncategorizedCount,
            BuildNotes(
                result.CleanupScopePath,
                highRiskCount,
                protectedLocationCount,
                accessIssueCount,
                reparsePointCount,
                quarantineCandidateCount,
                uncategorizedCount));
    }

    private static IReadOnlyList<string> BuildNotes(
        string cleanupScopePath,
        int highRiskCount,
        int protectedLocationCount,
        int accessIssueCount,
        int reparsePointCount,
        int quarantineCandidateCount,
        int uncategorizedCount)
    {
        var notes = new List<string>
        {
            $"Storage Scan was read-only within Cleanup Scope {cleanupScopePath}. No files were modified."
        };

        if (highRiskCount > 0 || protectedLocationCount > 0)
        {
            notes.Add("High-risk and Protected Location rows require manual review and are not cleanup approval.");
        }

        if (accessIssueCount > 0)
        {
            notes.Add("Access issues mean scan coverage is incomplete for those paths; no permissions were changed.");
        }

        if (reparsePointCount > 0)
        {
            notes.Add("Reparse points were not followed and should not be cleaned through broad actions.");
        }

        if (quarantineCandidateCount > 0)
        {
            notes.Add("Quarantine candidates are review candidates only until a separate explicit cleanup confirmation exists.");
        }

        if (uncategorizedCount > 0)
        {
            notes.Add("Uncategorized rows need inspection before category rules or cleanup decisions expand.");
        }

        return notes;
    }

    private static bool IsAccessIssue(StorageEntry entry)
    {
        return !entry.IsAccessible || entry.BloatCategories.Contains(BloatCategory.AccessIssue);
    }
}
