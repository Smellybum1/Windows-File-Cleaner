namespace WindowsFileCleaner.Core;

public sealed record StorageReviewSearch(string Query)
{
    public static StorageReviewSearch Empty { get; } = new("");

    public bool IsActive => !string.IsNullOrWhiteSpace(Query);

    public static StorageReviewSearch FromText(string? query)
    {
        return string.IsNullOrWhiteSpace(query)
            ? Empty
            : new StorageReviewSearch(query.Trim());
    }
}
