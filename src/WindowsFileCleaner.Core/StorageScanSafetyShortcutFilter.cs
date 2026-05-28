namespace WindowsFileCleaner.Core;

public sealed record StorageScanSafetyShortcutFilter(
    StorageReviewFilter ReviewFilter,
    StorageCategoryFilter CategoryFilter,
    string Label);
