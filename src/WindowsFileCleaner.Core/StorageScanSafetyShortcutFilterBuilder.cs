namespace WindowsFileCleaner.Core;

public static class StorageScanSafetyShortcutFilterBuilder
{
    public static StorageScanSafetyShortcutFilter Build(StorageScanSafetyShortcut shortcut)
    {
        return shortcut switch
        {
            StorageScanSafetyShortcut.HighRisk => new(
                StorageReviewFilter.HighRisk,
                StorageCategoryFilter.All,
                "High risk"),
            StorageScanSafetyShortcut.ProtectedLocations => new(
                StorageReviewFilter.All,
                StorageCategoryFilter.ForCategory(BloatCategory.ProtectedLocation),
                "Protected locations"),
            StorageScanSafetyShortcut.AccessIssues => new(
                StorageReviewFilter.AccessIssues,
                StorageCategoryFilter.All,
                "Access issues"),
            StorageScanSafetyShortcut.ReparsePoints => new(
                StorageReviewFilter.All,
                StorageCategoryFilter.ForCategory(BloatCategory.ReparsePoint),
                "Reparse points"),
            StorageScanSafetyShortcut.QuarantineCandidates => new(
                StorageReviewFilter.QuarantineCandidates,
                StorageCategoryFilter.All,
                "Quarantine candidates"),
            StorageScanSafetyShortcut.Uncategorized => new(
                StorageReviewFilter.All,
                StorageCategoryFilter.NoCategory,
                "No category"),
            _ => new(StorageReviewFilter.All, StorageCategoryFilter.All, "All")
        };
    }
}
