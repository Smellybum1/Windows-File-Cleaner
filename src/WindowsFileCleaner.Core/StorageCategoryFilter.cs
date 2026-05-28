namespace WindowsFileCleaner.Core;

public sealed record StorageCategoryFilter(StorageCategoryFilterKind Kind, BloatCategory? Category)
{
    public static StorageCategoryFilter All { get; } = new(StorageCategoryFilterKind.All, null);
    public static StorageCategoryFilter NoCategory { get; } = new(StorageCategoryFilterKind.NoCategory, null);

    public static StorageCategoryFilter ForCategory(BloatCategory category)
    {
        return new StorageCategoryFilter(StorageCategoryFilterKind.Category, category);
    }
}
