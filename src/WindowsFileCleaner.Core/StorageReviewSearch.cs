namespace WindowsFileCleaner.Core;

public sealed record StorageReviewSearch(string Query, StorageReviewSearchField Field, string Term)
{
    public static StorageReviewSearch Empty { get; } = new("", StorageReviewSearchField.Any, "");

    public bool IsActive => !string.IsNullOrWhiteSpace(Query);

    public static StorageReviewSearch FromText(string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Empty;
        }

        var trimmed = query.Trim();
        var field = ParseFieldPrefix(trimmed, out var term);
        return string.IsNullOrWhiteSpace(term)
            ? Empty
            : new StorageReviewSearch(trimmed, field, term);
    }

    private static StorageReviewSearchField ParseFieldPrefix(string query, out string term)
    {
        var separatorIndex = query.IndexOf(':', StringComparison.Ordinal);
        if (separatorIndex <= 0)
        {
            term = query;
            return StorageReviewSearchField.Any;
        }

        var prefix = query[..separatorIndex].Trim();
        var candidateTerm = query[(separatorIndex + 1)..].Trim();
        var field = prefix.ToLowerInvariant() switch
        {
            "path" => StorageReviewSearchField.Path,
            "name" => StorageReviewSearchField.Name,
            "category" or "cat" => StorageReviewSearchField.Category,
            "rating" or "importance" => StorageReviewSearchField.Rating,
            "recommendation" or "rec" => StorageReviewSearchField.Recommendation,
            "evidence" => StorageReviewSearchField.Evidence,
            "issue" or "access" or "accessissue" => StorageReviewSearchField.AccessIssue,
            _ => StorageReviewSearchField.Any
        };

        term = field == StorageReviewSearchField.Any
            ? query
            : candidateTerm;
        return field;
    }
}
