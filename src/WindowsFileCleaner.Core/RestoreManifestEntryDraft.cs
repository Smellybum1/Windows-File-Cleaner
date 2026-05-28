namespace WindowsFileCleaner.Core;

public sealed record RestoreManifestEntryDraft(
    string OriginalPath,
    string QuarantinePath,
    bool IsDirectory,
    long SizeBytes,
    DateTimeOffset? LastModifiedUtc,
    ImportanceRating ImportanceRating,
    DeletionRecommendation DeletionRecommendation,
    IReadOnlyList<BloatCategory> BloatCategories,
    string Evidence);
