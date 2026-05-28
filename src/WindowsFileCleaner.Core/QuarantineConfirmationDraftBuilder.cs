namespace WindowsFileCleaner.Core;

public static class QuarantineConfirmationDraftBuilder
{
    public static QuarantineConfirmationDraft Build(
        QuarantinePreview preview,
        RestoreManifestDraft restoreManifestDraft,
        DateTimeOffset draftedAtUtc,
        string confirmationId,
        bool isExecutionImplemented = false)
    {
        if (string.IsNullOrWhiteSpace(confirmationId))
        {
            throw new ArgumentException("Quarantine Confirmation Draft id is required.", nameof(confirmationId));
        }

        var blockers = BuildBlockers(preview, restoreManifestDraft);

        return new QuarantineConfirmationDraft(
            confirmationId.Trim(),
            draftedAtUtc.ToUniversalTime(),
            preview.CleanupScopePath,
            preview.QuarantineRootPath,
            restoreManifestDraft.DraftId,
            QuarantineConfirmationDraft.DefaultRequiredConfirmationText,
            isExecutionImplemented,
            preview.IncludedCount,
            preview.IncludedBytes,
            preview.BlockedCount,
            preview.RedundantCount,
            blockers,
            BuildReviewNotes(isExecutionImplemented));
    }

    private static IReadOnlyList<string> BuildBlockers(
        QuarantinePreview preview,
        RestoreManifestDraft restoreManifestDraft)
    {
        var blockers = new List<string>();

        if (preview.IncludedCount == 0)
        {
            blockers.Add("Quarantine Preview has no included rows.");
        }

        if (preview.BlockedCount > 0)
        {
            blockers.Add($"{preview.BlockedCount:N0} blocked preview row(s) must be removed or reviewed before confirmation.");
        }

        if (preview.RedundantCount > 0)
        {
            blockers.Add($"{preview.RedundantCount:N0} redundant preview row(s) must be removed before confirmation.");
        }

        if (!SamePath(preview.CleanupScopePath, restoreManifestDraft.CleanupScopePath))
        {
            blockers.Add("Restore Manifest Draft Cleanup Scope does not match the Quarantine Preview.");
        }

        if (!SamePath(preview.QuarantineRootPath, restoreManifestDraft.QuarantineRootPath))
        {
            blockers.Add("Restore Manifest Draft Quarantine root does not match the Quarantine Preview.");
        }

        if (restoreManifestDraft.SchemaVersion != RestoreManifestDraft.CurrentSchemaVersion)
        {
            blockers.Add("Restore Manifest Draft schema version is not supported.");
        }

        if (restoreManifestDraft.EntryCount != preview.IncludedCount)
        {
            blockers.Add("Restore Manifest Draft entry count does not match included Quarantine Preview rows.");
        }

        if (restoreManifestDraft.TotalBytes != preview.IncludedBytes)
        {
            blockers.Add("Restore Manifest Draft total bytes do not match included Quarantine Preview rows.");
        }

        AddEntryBlockers(preview, restoreManifestDraft, blockers);

        return blockers;
    }

    private static IReadOnlyList<string> BuildReviewNotes(bool isExecutionImplemented)
    {
        var executionNote = isExecutionImplemented
            ? "Fixture-only Quarantine execution is available after readiness blockers clear and the exact confirmation text is entered."
            : "Quarantine execution is not available for this Cleanup Scope in this build.";

        return
        [
            "No files were modified by this confirmation draft.",
            executionNote,
            $"Execution must require the exact confirmation text: {QuarantineConfirmationDraft.DefaultRequiredConfirmationText}."
        ];
    }

    private static void AddEntryBlockers(
        QuarantinePreview preview,
        RestoreManifestDraft restoreManifestDraft,
        List<string> blockers)
    {
        var includedPreviewRows = preview.Entries
            .Where(entry => entry.Disposition == QuarantinePreviewDisposition.Included && entry.DestinationPath is not null)
            .ToArray();
        var manifestRowsByOriginalPath = restoreManifestDraft.Entries
            .GroupBy(entry => PathSafety.GetFullPath(entry.OriginalPath), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.ToArray(), StringComparer.OrdinalIgnoreCase);

        foreach (var previewEntry in includedPreviewRows)
        {
            var previewSourcePath = PathSafety.GetFullPath(previewEntry.SourcePath);
            if (!manifestRowsByOriginalPath.TryGetValue(previewSourcePath, out var manifestRows))
            {
                blockers.Add($"Restore Manifest Draft is missing included preview row: {previewEntry.SourcePath}");
                continue;
            }

            if (manifestRows.Length > 1)
            {
                blockers.Add($"Restore Manifest Draft has duplicate entries for: {previewEntry.SourcePath}");
                continue;
            }

            var manifestRow = manifestRows[0];
            if (!SamePath(previewEntry.DestinationPath!, manifestRow.QuarantinePath))
            {
                blockers.Add($"Restore Manifest Draft destination does not match preview row: {previewEntry.SourcePath}");
            }

            if (previewEntry.SizeBytes != manifestRow.SizeBytes)
            {
                blockers.Add($"Restore Manifest Draft size does not match preview row: {previewEntry.SourcePath}");
            }
        }

        var previewSources = includedPreviewRows
            .Select(entry => PathSafety.GetFullPath(entry.SourcePath))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var manifestRow in restoreManifestDraft.Entries)
        {
            if (!previewSources.Contains(PathSafety.GetFullPath(manifestRow.OriginalPath)))
            {
                blockers.Add($"Restore Manifest Draft contains a row that is not included in the preview: {manifestRow.OriginalPath}");
            }
        }
    }

    private static bool SamePath(string left, string right)
    {
        return PathSafety.GetFullPath(left).Equals(PathSafety.GetFullPath(right), StringComparison.OrdinalIgnoreCase);
    }
}
