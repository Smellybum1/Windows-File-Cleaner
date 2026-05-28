namespace WindowsFileCleaner.Core;

public static class SelectedPathReviewGuidanceBuilder
{
    public static SelectedPathReviewGuidance Build(StorageEntry entry)
    {
        if (!entry.IsAccessible || entry.BloatCategories.Contains(BloatCategory.AccessIssue))
        {
            return new SelectedPathReviewGuidance(
                "Investigate access issue",
                [
                    "Storage Scan could not fully read this path, so the size and child details may be incomplete.",
                    "Do not shortlist this path for cleanup until the access issue is understood; no permissions were changed."
                ]);
        }

        if (entry.IsReparsePoint || entry.BloatCategories.Contains(BloatCategory.ReparsePoint))
        {
            return new SelectedPathReviewGuidance(
                "Inspect link target",
                [
                    "This path is a reparse point, and Storage Scan did not follow it.",
                    "Avoid broad cleanup actions here because the visible path may point somewhere else."
                ]);
        }

        if (entry.BloatCategories.Contains(BloatCategory.ProfileContainer))
        {
            return new SelectedPathReviewGuidance(
                "Inspect children, not the container",
                [
                    "This is a profile container. Review the largest immediate children instead of shortlisting the whole folder.",
                    "Cleaning a whole profile container would be unsafe and is not approved by the current app."
                ]);
        }

        if (entry.ImportanceRating == ImportanceRating.HighRisk
            || entry.BloatCategories.Contains(BloatCategory.ProtectedLocation))
        {
            return new SelectedPathReviewGuidance(
                "Keep by default",
                [
                    "This path overlaps protected, app, browser, game, source-code, or profile data.",
                    "Use the child breakdown and Explorer for inspection; cleanup should target only specific reviewed cache rows later."
                ]);
        }

        if (entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate)
        {
            return new SelectedPathReviewGuidance(
                "Shortlist after review",
                [
                    "This looks like one of the better cleanup candidates, but it is still not deletion approval.",
                    "Add it to Review Shortlist only if you recognize it, then use Quarantine Preview to check the dry-run outcome."
                ]);
        }

        if (IsCacheOrPackageReview(entry))
        {
            return new SelectedPathReviewGuidance(
                "Inspect before shortlisting",
                [
                    "This looks cache-like or package-cache-related, but current apps and development tools may still depend on it.",
                    "Prefer reviewing smaller child rows and using Quarantine Preview before any future cleanup execution."
                ]);
        }

        if (entry.BloatCategories.Count == 0)
        {
            return new SelectedPathReviewGuidance(
                "Classify before cleanup",
                [
                    "No cleanup-specific category matched this path.",
                    "Inspect the largest immediate children or open it in Explorer before expanding category rules."
                ]);
        }

        return new SelectedPathReviewGuidance(
            "Inspect evidence",
            [
                "Review the category evidence and largest immediate children before shortlisting this path.",
                "Storage Scan is read-only; no files were modified."
            ]);
    }

    private static bool IsCacheOrPackageReview(StorageEntry entry)
    {
        return entry.BloatCategories.Contains(BloatCategory.ApplicationDataArea)
            || entry.BloatCategories.Contains(BloatCategory.AppCache)
            || entry.BloatCategories.Contains(BloatCategory.GpuShaderCache)
            || entry.BloatCategories.Contains(BloatCategory.NodePackageCache)
            || entry.BloatCategories.Contains(BloatCategory.PythonPackageCache);
    }
}
