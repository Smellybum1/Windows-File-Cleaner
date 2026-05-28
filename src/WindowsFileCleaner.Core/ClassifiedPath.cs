namespace WindowsFileCleaner.Core;

public sealed record ClassifiedPath(
    IReadOnlyList<BloatCategory> BloatCategories,
    ImportanceRating ImportanceRating,
    DeletionRecommendation DeletionRecommendation,
    string Evidence);

