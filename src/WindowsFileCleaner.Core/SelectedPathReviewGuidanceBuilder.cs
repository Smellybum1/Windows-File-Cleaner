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

        if (entry.BloatCategories.Contains(BloatCategory.CleanupScopeRoot))
        {
            return new SelectedPathReviewGuidance(
                "Inspect children, not the scope root",
                [
                    "This is the Cleanup Scope root. Review largest immediate children instead of shortlisting the whole scanned folder.",
                    "Cleaning the scope root itself would be unsafe and is not approved by the current app."
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

        if (entry.BloatCategories.Contains(BloatCategory.GpuShaderCache))
        {
            return new SelectedPathReviewGuidance(
                "Review shader cache",
                [
                    "GPU shader caches can usually be rebuilt, but cleanup may cause temporary shader recompile delays or affect apps that are currently running.",
                    "Prefer narrowing to specific shader-cache child rows and use Quarantine Preview before any future cleanup execution."
                ]);
        }

        if (entry.BloatCategories.Contains(BloatCategory.PythonPackageCache))
        {
            return new SelectedPathReviewGuidance(
                "Review Python cache",
                [
                    "Python package caches are often rebuildable, but active development tools can still depend on nearby environments, wheels, or indexes.",
                    "Target only recognized cache rows after inspection; do not shortlist virtual environments, source folders, or Codex-related paths."
                ]);
        }

        if (entry.BloatCategories.Contains(BloatCategory.NodePackageCache))
        {
            return new SelectedPathReviewGuidance(
                "Review Node cache",
                [
                    "Node package caches may be rebuildable, but dependency folders can be part of active projects or tools.",
                    "Use parent/depth context and child rows to avoid shortlisting active project dependencies."
                ]);
        }

        if (entry.BloatCategories.Contains(BloatCategory.AppCache))
        {
            return new SelectedPathReviewGuidance(
                "Review app cache",
                [
                    "This looks cache-like, but AppData can mix disposable cache files with active app state.",
                    "Prefer specific cache child rows over broad app folders, then use Quarantine Preview for a dry-run review."
                ]);
        }

        if (entry.BloatCategories.Contains(BloatCategory.ApplicationDataArea))
        {
            return new SelectedPathReviewGuidance(
                "Inspect AppData carefully",
                [
                    "This is inside AppData, where caches, settings, sessions, credentials, and app state can sit close together.",
                    "Use the category filters, child breakdown, and file preview before expanding cleanup rules."
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
}
