namespace WindowsFileCleaner.Core;

public sealed record RestoreManifestEntry(
    string OriginalPath,
    string RelativePath,
    string QuarantinePath,
    bool IsDirectory,
    long SizeBytes,
    DateTimeOffset? LastModifiedUtc,
    ImportanceRating ImportanceRating,
    DeletionRecommendation DeletionRecommendation,
    IReadOnlyList<BloatCategory> BloatCategories,
    string Evidence,
    RestoreManifestEntryStatus Status,
    DateTimeOffset? MoveStartedAtUtc,
    DateTimeOffset? MoveCompletedAtUtc,
    DateTimeOffset? RestoreStartedAtUtc,
    DateTimeOffset? RestoreCompletedAtUtc,
    string? ErrorMessage);
