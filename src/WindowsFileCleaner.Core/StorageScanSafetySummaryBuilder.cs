namespace WindowsFileCleaner.Core;

public static class StorageScanSafetySummaryBuilder
{
    private const int MaxAccessIssueExamples = 3;
    private const int MaxQuarantineCandidateExamples = 3;

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
            BuildAccessIssueExamples(result.CleanupScopePath, review.Entries),
            BuildQuarantineCandidateExamples(result.CleanupScopePath, review.Entries),
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

    private static IReadOnlyList<string> BuildAccessIssueExamples(string cleanupScopePath, IReadOnlyList<StorageReviewEntry> entries)
    {
        return entries
            .Where(row => IsAccessIssue(row.Entry))
            .OrderBy(row => row.Entry.FullPath, StringComparer.OrdinalIgnoreCase)
            .Take(MaxAccessIssueExamples)
            .Select(row => FormatAccessIssueExample(cleanupScopePath, row.Entry))
            .ToArray();
    }

    private static IReadOnlyList<string> BuildQuarantineCandidateExamples(string cleanupScopePath, IReadOnlyList<StorageReviewEntry> entries)
    {
        return entries
            .Where(row => row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate)
            .OrderByDescending(row => row.Entry.SizeBytes)
            .ThenBy(row => row.Entry.FullPath, StringComparer.OrdinalIgnoreCase)
            .Take(MaxQuarantineCandidateExamples)
            .Select(row => FormatQuarantineCandidateExample(cleanupScopePath, row.Entry))
            .ToArray();
    }

    private static string FormatAccessIssueExample(string cleanupScopePath, StorageEntry entry)
    {
        var path = FormatScopeRelativePath(cleanupScopePath, entry.FullPath);
        return string.IsNullOrWhiteSpace(entry.ErrorMessage)
            ? path
            : $"{path} ({entry.ErrorMessage})";
    }

    private static string FormatQuarantineCandidateExample(string cleanupScopePath, StorageEntry entry)
    {
        return $"{FormatScopeRelativePath(cleanupScopePath, entry.FullPath)} ({entry.SizeDisplay})";
    }

    private static string FormatScopeRelativePath(string cleanupScopePath, string fullPath)
    {
        try
        {
            return PathSafety.IsWithinScope(cleanupScopePath, fullPath)
                ? Path.GetRelativePath(PathSafety.GetFullPath(cleanupScopePath), fullPath)
                : fullPath;
        }
        catch (ArgumentException)
        {
            return fullPath;
        }
        catch (NotSupportedException)
        {
            return fullPath;
        }
    }

    private static bool IsAccessIssue(StorageEntry entry)
    {
        return !entry.IsAccessible || entry.BloatCategories.Contains(BloatCategory.AccessIssue);
    }
}
