namespace WindowsFileCleaner.Core;

public static class PreExecutionRevalidationBuilder
{
    public static PreExecutionRevalidation Build(
        QuarantinePreview preview,
        QuarantineConfirmationDraft confirmationDraft,
        QuarantineActionDraft actionDraft,
        QuarantineRootExecutionSafety rootExecutionSafety,
        DateTimeOffset revalidatedAtUtc)
    {
        var blockers = new List<string>();

        AddConsistencyBlockers(preview, confirmationDraft, actionDraft, rootExecutionSafety, blockers);
        AddRootSafetyBlockers(rootExecutionSafety, blockers);
        AddActionCollisionBlockers(actionDraft, blockers);
        AddEntryBlockers(preview, actionDraft, blockers);

        return new PreExecutionRevalidation(
            revalidatedAtUtc.ToUniversalTime(),
            PathSafety.GetFullPath(preview.CleanupScopePath),
            PathSafety.GetFullPath(preview.QuarantineRootPath),
            PathSafety.GetFullPath(actionDraft.ActionRootPath),
            PathSafety.GetFullPath(actionDraft.ItemsRootPath),
            PathSafety.GetFullPath(actionDraft.RestoreManifestPath),
            preview.IncludedCount,
            preview.IncludedBytes,
            rootExecutionSafety.CanUseForExecution,
            blockers,
            [
                "No folders were created and no files were modified by this Pre-Execution Revalidation.",
                "Revalidation must run again immediately before any future real-profile Quarantine movement."
            ]);
    }

    private static void AddConsistencyBlockers(
        QuarantinePreview preview,
        QuarantineConfirmationDraft confirmationDraft,
        QuarantineActionDraft actionDraft,
        QuarantineRootExecutionSafety rootExecutionSafety,
        List<string> blockers)
    {
        foreach (var blocker in confirmationDraft.Blockers)
        {
            blockers.Add($"Quarantine Confirmation Draft: {blocker}");
        }

        if (!SamePath(preview.CleanupScopePath, confirmationDraft.CleanupScopePath)
            || !SamePath(preview.CleanupScopePath, actionDraft.CleanupScopePath)
            || !SamePath(preview.CleanupScopePath, rootExecutionSafety.CleanupScopePath))
        {
            blockers.Add("Cleanup Scope no longer matches across preview, confirmation, action draft, and root safety.");
        }

        if (!SamePath(preview.QuarantineRootPath, confirmationDraft.QuarantineRootPath)
            || !SamePath(preview.QuarantineRootPath, actionDraft.QuarantineRootPath)
            || !SamePath(preview.QuarantineRootPath, rootExecutionSafety.QuarantineRootPath))
        {
            blockers.Add("Quarantine Root no longer matches across preview, confirmation, action draft, and root safety.");
        }

        if (!SamePath(actionDraft.ActionRootPath, rootExecutionSafety.ActionRootPath))
        {
            blockers.Add("Quarantine action root no longer matches root safety.");
        }

        if (!SamePath(actionDraft.ItemsRootPath, rootExecutionSafety.ItemsRootPath))
        {
            blockers.Add("Quarantine items root no longer matches root safety.");
        }

        if (!SamePath(actionDraft.RestoreManifestPath, rootExecutionSafety.RestoreManifestPath))
        {
            blockers.Add("Restore Manifest path no longer matches root safety.");
        }

        if (preview.IncludedCount != confirmationDraft.IncludedCount
            || preview.IncludedCount != actionDraft.EntryCount)
        {
            blockers.Add("Included row count changed after review; recreate Quarantine Preview and action draft.");
        }

        if (preview.IncludedBytes != confirmationDraft.IncludedBytes
            || preview.IncludedBytes != actionDraft.TotalBytes)
        {
            blockers.Add("Included byte count changed after review; recreate Quarantine Preview and action draft.");
        }

        if (!string.Equals(actionDraft.RestoreManifestDraftId, confirmationDraft.RestoreManifestDraftId, StringComparison.Ordinal))
        {
            blockers.Add("Quarantine Action Draft does not reference the same Restore Manifest Draft as confirmation.");
        }
    }

    private static void AddRootSafetyBlockers(
        QuarantineRootExecutionSafety rootExecutionSafety,
        List<string> blockers)
    {
        foreach (var blocker in rootExecutionSafety.Blockers)
        {
            blockers.Add($"Quarantine Root Execution Safety: {blocker}");
        }
    }

    private static void AddActionCollisionBlockers(
        QuarantineActionDraft actionDraft,
        List<string> blockers)
    {
        if (PathExists(actionDraft.ActionRootPath))
        {
            blockers.Add($"Quarantine action root now exists: {actionDraft.ActionRootPath}");
        }

        if (PathExists(actionDraft.RestoreManifestPath))
        {
            blockers.Add($"Restore Manifest path now exists: {actionDraft.RestoreManifestPath}");
        }
    }

    private static void AddEntryBlockers(
        QuarantinePreview preview,
        QuarantineActionDraft actionDraft,
        List<string> blockers)
    {
        var includedPreviewRows = preview.Entries
            .Where(entry => entry.IsIncluded)
            .ToArray();
        var actionRowsByOriginalPath = actionDraft.Entries
            .GroupBy(entry => PathSafety.GetFullPath(entry.OriginalPath), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.ToArray(), StringComparer.OrdinalIgnoreCase);
        var previewSources = includedPreviewRows
            .Select(entry => PathSafety.GetFullPath(entry.SourcePath))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var previewEntry in includedPreviewRows)
        {
            var sourcePath = PathSafety.GetFullPath(previewEntry.SourcePath);
            if (!actionRowsByOriginalPath.TryGetValue(sourcePath, out var actionRows))
            {
                blockers.Add($"Quarantine Action Draft is missing included preview row: {previewEntry.SourcePath}");
                continue;
            }

            if (actionRows.Length > 1)
            {
                blockers.Add($"Quarantine Action Draft has duplicate entries for: {previewEntry.SourcePath}");
                continue;
            }

            AddEntryBlockers(preview, previewEntry, actionRows[0], actionDraft.ItemsRootPath, blockers);
        }

        foreach (var actionEntry in actionDraft.Entries)
        {
            if (!previewSources.Contains(PathSafety.GetFullPath(actionEntry.OriginalPath)))
            {
                blockers.Add($"Quarantine Action Draft contains a row that is not included in the preview: {actionEntry.OriginalPath}");
            }
        }
    }

    private static void AddEntryBlockers(
        QuarantinePreview preview,
        QuarantinePreviewEntry previewEntry,
        QuarantineActionEntryDraft actionEntry,
        string itemsRootPath,
        List<string> blockers)
    {
        var sourcePath = PathSafety.GetFullPath(previewEntry.SourcePath);

        if (!PathSafety.IsWithinScope(preview.CleanupScopePath, sourcePath))
        {
            blockers.Add($"Included source is now outside the Cleanup Scope: {sourcePath}");
        }

        if (PathExists(actionEntry.ActionQuarantinePath))
        {
            blockers.Add($"Quarantine item destination now exists: {actionEntry.ActionQuarantinePath}");
        }

        if (!PathSafety.IsWithinScope(itemsRootPath, actionEntry.ActionQuarantinePath))
        {
            blockers.Add($"Quarantine item destination is outside the items root: {actionEntry.ActionQuarantinePath}");
        }

        if (!PathExists(sourcePath))
        {
            blockers.Add($"Included source no longer exists: {sourcePath}");
            return;
        }

        if (IsReparsePoint(sourcePath))
        {
            blockers.Add($"Included source is now a reparse point: {sourcePath}");
        }

        if (actionEntry.IsDirectory)
        {
            if (!Directory.Exists(sourcePath))
            {
                blockers.Add($"Included source is no longer a directory: {sourcePath}");
            }
        }
        else
        {
            if (!File.Exists(sourcePath))
            {
                blockers.Add($"Included source is no longer a file: {sourcePath}");
                return;
            }

            var fileInfo = new FileInfo(sourcePath);
            if (fileInfo.Length != previewEntry.SizeBytes || fileInfo.Length != actionEntry.SizeBytes)
            {
                blockers.Add($"Included source size changed after preview: {sourcePath}");
            }

            if (previewEntry.Entry.LastModifiedUtc is not null
                && fileInfo.LastWriteTimeUtc != previewEntry.Entry.LastModifiedUtc.Value.UtcDateTime)
            {
                blockers.Add($"Included source modified timestamp changed after preview: {sourcePath}");
            }
        }
    }

    private static bool PathExists(string path)
    {
        try
        {
            return File.Exists(path) || Directory.Exists(path);
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch (NotSupportedException)
        {
            return false;
        }
        catch (PathTooLongException)
        {
            return false;
        }
    }

    private static bool IsReparsePoint(string path)
    {
        try
        {
            return (File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0;
        }
        catch (ArgumentException)
        {
            return true;
        }
        catch (IOException)
        {
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return true;
        }
    }

    private static bool SamePath(string left, string right)
    {
        return PathSafety.GetFullPath(left).Equals(PathSafety.GetFullPath(right), StringComparison.OrdinalIgnoreCase);
    }
}
