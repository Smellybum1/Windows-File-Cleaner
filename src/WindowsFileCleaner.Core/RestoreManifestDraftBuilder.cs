namespace WindowsFileCleaner.Core;

public static class RestoreManifestDraftBuilder
{
    public static RestoreManifestDraft Build(
        QuarantinePreview preview,
        DateTimeOffset draftedAtUtc,
        string draftId)
    {
        if (string.IsNullOrWhiteSpace(draftId))
        {
            throw new ArgumentException("Restore Manifest Draft id is required.", nameof(draftId));
        }

        return new RestoreManifestDraft(
            RestoreManifestDraft.CurrentSchemaVersion,
            draftId.Trim(),
            draftedAtUtc.ToUniversalTime(),
            preview.CleanupScopePath,
            preview.QuarantineRootPath,
            preview.Entries
                .Where(entry => entry.Disposition == QuarantinePreviewDisposition.Included && entry.DestinationPath is not null)
                .Select(entry => new RestoreManifestEntryDraft(
                    entry.SourcePath,
                    entry.DestinationPath!,
                    entry.Entry.IsDirectory,
                    entry.Entry.SizeBytes,
                    entry.Entry.LastModifiedUtc,
                    entry.Entry.ImportanceRating,
                    entry.Entry.DeletionRecommendation,
                    entry.Entry.BloatCategories,
                    entry.Entry.Evidence))
                .ToArray());
    }
}
