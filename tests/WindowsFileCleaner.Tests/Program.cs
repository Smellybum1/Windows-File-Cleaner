using WindowsFileCleaner.Core;

var tests = new StorageScanTests();
tests.ScannerTotalsFilesAndClassifiesCandidates();
tests.ScannerRefusesToLeaveCleanupScope();
tests.LaunchOptionsDefaultAndScopeArgument();
tests.CleanupScopeSafetyNoteDistinguishesFixtureAndRealProfile();
tests.ClassifierLabelsRealScanContainerPatterns();
tests.ClassifierLabelsLargeOldUnknownFilesConservatively();
tests.ReviewBuilderSummarizesAndFiltersResults();
tests.ReviewBuilderFiltersAccessIssues();
tests.ReviewSearchCombinesWithReviewAndCategoryFilters();
tests.ReviewSearchSupportsFieldPrefixes();
tests.StorageScanSafetySummaryHighlightsReviewBoundaries();
tests.StorageScanSafetyShortcutsMapToReadOnlyFilters();
tests.ReviewShortlistTracksSelectedRowsWithoutModifyingReview();
tests.QuarantinePreviewBuildsReadOnlyPlanFromShortlist();
tests.QuarantinePreviewCsvExporterWritesReviewReport();
tests.RestoreManifestDraftBuildsJsonUndoMetadataFromIncludedPreviewRows();
tests.QuarantineConfirmationDraftChecksPreviewAndManifestReadiness();
tests.QuarantineConfirmationDraftReportsPreviewAndManifestBlockers();
tests.ChildSummaryShowsLargestImmediateChildren();
tests.SelectedPathReviewGuidanceExplainsReviewNextSteps();
tests.PathInspectionPlanBuildsExplorerArguments();
tests.SelectedFileContentPreviewReadsBoundedTextOnly();
tests.CsvExporterWritesEscapedReviewRows();
tests.ByteSizeFormatterUsesReadableUnits();
tests.ProductionCodeDoesNotContainCleanupExecutionCalls();

Console.WriteLine("All WindowsFileCleaner.Tests checks passed.");

internal sealed class StorageScanTests
{
    public void ScannerTotalsFilesAndClassifiesCandidates()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-installer.msi", 1024 * 1024 * 3, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"AppData\Local\Temp\scratch.tmp", 1024 * 256, DateTimeOffset.UtcNow.AddDays(-2));
        fixture.WriteFile(@".codex\config.json", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"Documents\important.txt", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var rows = Flatten(result.Root).ToArray();

        Assert(result.TotalBytes == 3_409_920, $"Expected total bytes to be 3,409,920 but got {result.TotalBytes}.");
        Assert(result.InaccessibleCount == 0, "Fixture scan should have no inaccessible paths.");

        var installer = Single(rows, "old-installer.msi");
        Assert(installer.BloatCategories.Contains(BloatCategory.OldDownload), "Old installer should be categorized as an old download.");
        Assert(installer.BloatCategories.Contains(BloatCategory.InstallerCache), "Old installer should be categorized as installer cache.");
        Assert(installer.ImportanceRating == ImportanceRating.LikelySafe, "Old installer should be likely safe.");
        Assert(installer.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate, "Old installer should be a quarantine candidate.");

        var codexFolder = Single(rows, ".codex");
        Assert(codexFolder.ImportanceRating == ImportanceRating.HighRisk, "Codex folder should be high risk.");
        Assert(codexFolder.DeletionRecommendation == DeletionRecommendation.Keep, "Codex folder should be kept.");

        var documentsFolder = Single(rows, "Documents");
        Assert(documentsFolder.ImportanceRating == ImportanceRating.HighRisk, "Documents folder should be high risk.");
    }

    public void ScannerRefusesToLeaveCleanupScope()
    {
        using var fixture = TestFixture.Create();
        fixture.WriteFile(@"Downloads\file.txt", 10, DateTimeOffset.UtcNow);

        var outside = Path.GetFullPath(Path.Combine(fixture.RootPath, "..", "outside.txt"));
        Assert(!PathSafety.IsWithinScope(fixture.RootPath, outside), "Sibling path must not be inside Cleanup Scope.");
    }

    public void LaunchOptionsDefaultAndScopeArgument()
    {
        var defaultOptions = StorageScanLaunchOptions.Parse([]);
        Assert(
            defaultOptions.CleanupScopePath == StorageScanOptions.DefaultForCurrentUser().CleanupScopePath,
            "Launch options should default to the current user's Cleanup Scope.");

        var separateScope = @"D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture";
        var parsedSeparate = StorageScanLaunchOptions.Parse(["--scope", separateScope]);
        Assert(parsedSeparate.CleanupScopePath == separateScope, "Launch options should parse --scope <path>.");

        var equalsScope = @"D:\Codex\Windows File Cleaner\.local\another-smoke-fixture";
        var parsedEquals = StorageScanLaunchOptions.Parse([$"--scope={equalsScope}"]);
        Assert(parsedEquals.CleanupScopePath == equalsScope, "Launch options should parse --scope=<path>.");

        var missingScopeFailed = false;
        try
        {
            StorageScanLaunchOptions.Parse(["--scope"]);
        }
        catch (ArgumentException)
        {
            missingScopeFailed = true;
        }

        Assert(missingScopeFailed, "Launch options should reject --scope without a path.");
    }

    public void CleanupScopeSafetyNoteDistinguishesFixtureAndRealProfile()
    {
        var realProfile = CleanupScopeSafetyNoteBuilder.Build(@"C:\Users\moxhe");
        Assert(realProfile.IsRealUserProfileScope, "Default Cleanup Scope should be recognized as a real user profile scope.");
        Assert(!realProfile.IsFixtureScope, "Default Cleanup Scope should not be recognized as fixture scope.");
        Assert(realProfile.Label == "Real Profile Cleanup Scope", "Real profile scope should use the expected label.");
        Assert(
            realProfile.Message.Contains("preflight", StringComparison.OrdinalIgnoreCase),
            "Real profile scope should remind the user to run preflight before scanning.");

        var realProfileChild = CleanupScopeSafetyNoteBuilder.Build(@"C:\Users\moxhe\AppData\Local");
        Assert(realProfileChild.IsRealUserProfileScope, "Child paths under the default Cleanup Scope should still be recognized as real user profile scope.");

        var fixture = CleanupScopeSafetyNoteBuilder.Build(@"D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture");
        Assert(fixture.IsFixtureScope, "Synthetic fixture scope should be recognized as fixture scope.");
        Assert(!fixture.IsRealUserProfileScope, "Synthetic fixture scope should not be recognized as real profile scope.");
        Assert(fixture.Label == "Fixture Cleanup Scope", "Fixture scope should use the expected label.");

        var custom = CleanupScopeSafetyNoteBuilder.Build(@"D:\Scratch\Review");
        Assert(!custom.IsFixtureScope, "Custom path should not be recognized as fixture scope.");
        Assert(!custom.IsRealUserProfileScope, "Custom path should not be recognized as default real profile scope.");
        Assert(custom.Label == "Custom Cleanup Scope", "Custom path should use the expected label.");

        var empty = CleanupScopeSafetyNoteBuilder.Build(" ");
        Assert(empty.Label == "Choose Cleanup Scope", "Blank paths should ask for a Cleanup Scope.");
    }

    public void ClassifierLabelsRealScanContainerPatterns()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"AppData\Local\pip\Cache\http-v2\response.body", 1024, DateTimeOffset.UtcNow.AddDays(-30));
        fixture.WriteFile(@"AppData\Local\NVIDIA\DXCache\shader.bin", 2048, DateTimeOffset.UtcNow.AddDays(-5));
        fixture.WriteFile(@"AppData\Local\Google\Chrome\User Data\Default\Preferences", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"AppData\Local\Packages\Microsoft.WindowsStore_8wekyb3d8bbwe\Settings\settings.dat", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"AppData\Local\Programs\SomeApp\app.exe", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"AppData\Local\Larian Studios\Baldur's Gate 3\PlayerProfiles\profile.dat", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var rows = Flatten(result.Root).ToArray();

        var pip = Single(rows, "pip");
        Assert(pip.BloatCategories.Contains(BloatCategory.ApplicationDataArea), "pip should be marked as AppData area.");
        Assert(pip.BloatCategories.Contains(BloatCategory.PythonPackageCache), "pip should be marked as Python package cache.");
        Assert(pip.ImportanceRating == ImportanceRating.Caution, "pip cache container should stay caution.");

        var dxCache = Single(rows, "DXCache");
        Assert(dxCache.BloatCategories.Contains(BloatCategory.GpuShaderCache), "DXCache should be marked as GPU shader cache.");
        Assert(dxCache.ImportanceRating == ImportanceRating.Caution, "GPU shader cache should stay caution.");

        var userData = Single(rows, "User Data");
        Assert(userData.BloatCategories.Contains(BloatCategory.BrowserData), "Browser User Data should be marked as browser data.");
        Assert(userData.ImportanceRating == ImportanceRating.HighRisk, "Browser User Data should be high risk.");
        Assert(userData.DeletionRecommendation == DeletionRecommendation.Keep, "Browser User Data should be kept.");

        var packages = Single(rows, "Packages");
        Assert(packages.BloatCategories.Contains(BloatCategory.WindowsAppData), "Packages should be marked as Windows app data.");
        Assert(packages.BloatCategories.Contains(BloatCategory.ProtectedLocation), "Windows app package data should be protected.");
        Assert(packages.ImportanceRating == ImportanceRating.HighRisk, "Windows app package data should be high risk.");
        Assert(packages.DeletionRecommendation == DeletionRecommendation.Keep, "Windows app package data should be kept.");

        var programs = Single(rows, "Programs");
        Assert(programs.BloatCategories.Contains(BloatCategory.InstalledApplication), "AppData Local Programs should be marked as installed application data.");
        Assert(programs.ImportanceRating == ImportanceRating.HighRisk, "Installed application folders should be high risk.");
        Assert(programs.DeletionRecommendation == DeletionRecommendation.Keep, "Installed application folders should be kept.");

        var baldursGate = Single(rows, "Baldur's Gate 3");
        Assert(baldursGate.BloatCategories.Contains(BloatCategory.GameData), "Known game folders should be marked as game data.");
        Assert(baldursGate.ImportanceRating == ImportanceRating.HighRisk, "Game data should be high risk.");
        Assert(baldursGate.DeletionRecommendation == DeletionRecommendation.Keep, "Game data should be kept.");
    }

    public void ClassifierLabelsLargeOldUnknownFilesConservatively()
    {
        var classifier = new CleanupCandidateClassifier();
        var oldLargeFile = new PathSnapshot(
            @"C:\Users\moxhe\Downloads\fa3a693d7c1f.bin",
            "fa3a693d7c1f.bin",
            IsDirectory: false,
            IsAccessible: true,
            IsReparsePoint: false,
            LastModifiedUtc: DateTimeOffset.UtcNow.AddDays(-180));

        var oldLargeClassification = classifier.Classify(oldLargeFile, sizeBytes: 2L * 1024 * 1024 * 1024);

        Assert(oldLargeClassification.BloatCategories.Contains(BloatCategory.LargeOldFile), "Large stale files should be labeled for review.");
        Assert(oldLargeClassification.ImportanceRating == ImportanceRating.LikelySafe, "Old large Downloads files with download evidence can remain likely-safe candidates.");
        Assert(oldLargeClassification.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate, "Old large Downloads files should still use quarantine candidate recommendation.");
        Assert(oldLargeClassification.Evidence.Contains("large file", StringComparison.OrdinalIgnoreCase), "Large stale files should explain their evidence.");

        var unknownOldLargeFile = oldLargeFile with
        {
            FullPath = @"C:\Users\moxhe\Unknown\fa3a693d7c1f.bin"
        };
        var unknownClassification = classifier.Classify(unknownOldLargeFile, sizeBytes: 2L * 1024 * 1024 * 1024);

        Assert(unknownClassification.BloatCategories.Contains(BloatCategory.LargeOldFile), "Large stale unknown files should be labeled for review.");
        Assert(unknownClassification.ImportanceRating == ImportanceRating.Caution, "Large stale unknown files should stay caution.");
        Assert(unknownClassification.DeletionRecommendation == DeletionRecommendation.Inspect, "Large stale unknown files should require inspection.");
    }

    public void ReviewBuilderSummarizesAndFiltersResults()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-installer.msi", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"AppData\Local\Temp\scratch.tmp", 1024, DateTimeOffset.UtcNow.AddDays(-2));
        fixture.WriteFile(@".codex\config.json", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"Unknown\notes.txt", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);

        Assert(review.Summary.TotalEntries > 0, "Review should include flattened entries.");
        Assert(review.Summary.LikelySafeCount > 0, "Review should count likely safe entries.");
        Assert(review.Summary.CautionCount > 0, "Review should count caution entries.");
        Assert(review.Summary.HighRiskCount > 0, "Review should count high-risk entries.");
        Assert(review.Summary.QuarantineCandidateCount > 0, "Review should count quarantine candidates.");
        Assert(review.Summary.AccessIssueCount == 0, "Fixture review should count zero access issues.");
        Assert(review.Summary.LargestEntryBytes >= 1024 * 1024, "Review should record the largest row size.");
        Assert(review.Summary.QuarantineCandidateLargestEntryBytes == 1024 * 1024, "Review should record the largest quarantine candidate row without summing recursive rows.");

        var installerSummary = review.CategorySummaries.Single(summary => summary.Category == BloatCategory.InstallerCache);
        Assert(installerSummary.Count > 0, "Review should summarize installer cache rows by category.");
        Assert(installerSummary.LargestEntryBytes == 1024 * 1024, "Category summary should record largest category row without summing recursive rows.");

        var highRiskRows = review.ApplyFilter(StorageReviewFilter.HighRisk);
        Assert(highRiskRows.All(row => row.Entry.ImportanceRating == ImportanceRating.HighRisk), "High risk filter should only return high-risk entries.");

        var quarantineRows = review.ApplyFilter(StorageReviewFilter.QuarantineCandidates);
        Assert(quarantineRows.All(row => row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate), "Quarantine filter should only return quarantine candidates.");

        var installerRows = review.ApplyFilter(StorageReviewFilter.All, BloatCategory.InstallerCache);
        Assert(installerRows.Count > 0, "Category filter should return matching category rows.");
        Assert(installerRows.All(row => row.Entry.BloatCategories.Contains(BloatCategory.InstallerCache)), "Category filter should only return rows with that category.");

        var appDataCautionRows = review.ApplyFilter(StorageReviewFilter.Caution, BloatCategory.ApplicationDataArea);
        Assert(appDataCautionRows.Count > 0, "Category filter should combine with the selected review filter.");
        Assert(appDataCautionRows.All(row => row.Entry.ImportanceRating == ImportanceRating.Caution), "Combined filter should preserve the review filter.");
        Assert(appDataCautionRows.All(row => row.Entry.BloatCategories.Contains(BloatCategory.ApplicationDataArea)), "Combined filter should preserve the category filter.");

        var noCategoryRows = review.ApplyFilter(StorageReviewFilter.All, StorageCategoryFilter.NoCategory);
        Assert(noCategoryRows.Count > 0, "No category filter should return uncategorized rows.");
        Assert(noCategoryRows.All(row => row.Entry.BloatCategories.Count == 0), "No category filter should only return uncategorized rows.");

        var noCategoryCautionRows = review.ApplyFilter(StorageReviewFilter.Caution, StorageCategoryFilter.NoCategory);
        Assert(noCategoryCautionRows.Count > 0, "No category filter should combine with the selected review filter.");
        Assert(noCategoryCautionRows.All(row => row.Entry.ImportanceRating == ImportanceRating.Caution), "No category combined filter should preserve the review filter.");
        Assert(noCategoryCautionRows.All(row => row.Entry.BloatCategories.Count == 0), "No category combined filter should preserve the no-category filter.");

        var fileRows = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageEntryTypeFilter.Files,
            StorageReviewSearch.Empty);
        Assert(fileRows.Count > 0, "File type filter should return file rows.");
        Assert(fileRows.All(row => !row.Entry.IsDirectory), "File type filter should only return files.");

        var folderRows = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageEntryTypeFilter.Folders,
            StorageReviewSearch.Empty);
        Assert(folderRows.Count > 0, "Folder type filter should return folder rows.");
        Assert(folderRows.All(row => row.Entry.IsDirectory), "Folder type filter should only return folders.");

        var searchedInstallerFiles = review.ApplyFilter(
            StorageReviewFilter.QuarantineCandidates,
            StorageCategoryFilter.ForCategory(BloatCategory.InstallerCache),
            StorageEntryTypeFilter.Files,
            StorageReviewSearch.FromText("old-installer"));
        Assert(searchedInstallerFiles.Count > 0, "Type filter should combine with review, category, and search filters.");
        Assert(
            searchedInstallerFiles.All(row =>
                !row.Entry.IsDirectory
                && row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate
                && row.Entry.BloatCategories.Contains(BloatCategory.InstallerCache)
                && row.Entry.FullPath.Contains("old-installer", StringComparison.OrdinalIgnoreCase)),
            "Combined type-filtered rows should preserve all active review lenses.");
    }

    public void ReviewBuilderFiltersAccessIssues()
    {
        var now = DateTimeOffset.UtcNow;
        var inaccessible = new StorageEntry(
            @"C:\Users\moxhe\Locked",
            "Locked",
            IsDirectory: true,
            SizeBytes: 0,
            LastModifiedUtc: null,
            IsAccessible: false,
            IsReparsePoint: false,
            ErrorMessage: "Access denied.",
            BloatCategories: [BloatCategory.AccessIssue],
            ImportanceRating: ImportanceRating.Caution,
            DeletionRecommendation: DeletionRecommendation.Inspect,
            Evidence: "The path could not be fully read.",
            Children: []);
        var readable = new StorageEntry(
            @"C:\Users\moxhe\Downloads\setup.msi",
            "setup.msi",
            IsDirectory: false,
            SizeBytes: 1024,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.InstallerCache],
            ImportanceRating: ImportanceRating.LikelySafe,
            DeletionRecommendation: DeletionRecommendation.QuarantineCandidate,
            Evidence: "Installer.",
            Children: []);
        var root = new StorageEntry(
            @"C:\Users\moxhe",
            "moxhe",
            IsDirectory: true,
            SizeBytes: readable.SizeBytes,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [],
            ImportanceRating: ImportanceRating.Caution,
            DeletionRecommendation: DeletionRecommendation.Inspect,
            Evidence: "Root.",
            Children: [readable, inaccessible]);
        var result = new StorageScanResult(@"C:\Users\moxhe", now, now, root);

        var review = StorageScanReviewBuilder.Build(result);
        var accessRows = review.ApplyFilter(StorageReviewFilter.AccessIssues);

        Assert(review.Summary.AccessIssueCount == 1, "Review should count access issue rows.");
        Assert(review.Summary.AccessIssueLargestEntryBytes == 0, "Access issue largest row should reflect the unreadable row size.");
        Assert(accessRows.Count == 1, "Access issue filter should only return inaccessible rows.");
        Assert(accessRows[0].Entry.FullPath.EndsWith(@"\Locked", StringComparison.OrdinalIgnoreCase), "Access issue filter should return the inaccessible path.");
    }

    public void ReviewSearchCombinesWithReviewAndCategoryFilters()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"AppData\Local\pip\Cache\http-v2\response.body", 1024, DateTimeOffset.UtcNow.AddDays(-30));
        fixture.WriteFile(@"Downloads\setup.msi", 1024, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"Documents\budget.xlsx", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"Unknown\notes.txt", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);

        var pipRows = review.ApplyFilter(
            StorageReviewFilter.Caution,
            StorageCategoryFilter.All,
            StorageReviewSearch.FromText("python package cache"));
        Assert(pipRows.Count > 0, "Search should find Python package cache rows by user-facing spaced category text.");
        Assert(
            pipRows.All(row => row.Entry.BloatCategories.Contains(BloatCategory.PythonPackageCache)),
            "Python package cache search within Caution should only return matching cache rows in this fixture.");

        var installerRows = review.ApplyFilter(
            StorageReviewFilter.QuarantineCandidates,
            StorageCategoryFilter.ForCategory(BloatCategory.InstallerCache),
            StorageReviewSearch.FromText("setup"));
        Assert(installerRows.Count > 0, "Search should combine with Quarantine candidates and category filters.");
        Assert(
            installerRows.All(row =>
                row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate
                && row.Entry.BloatCategories.Contains(BloatCategory.InstallerCache)
                && row.Entry.FullPath.Contains("setup", StringComparison.OrdinalIgnoreCase)),
            "Combined search results should preserve recommendation, category, and search filters.");

        var highRiskRows = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageReviewSearch.FromText("high risk"));
        Assert(highRiskRows.Count > 0, "Search should match spaced Importance Rating text.");
        Assert(highRiskRows.All(row => row.Entry.ImportanceRating == ImportanceRating.HighRisk), "High risk search should find high-risk rows in this fixture.");
    }

    public void ReviewSearchSupportsFieldPrefixes()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"AppData\Local\pip\Cache\http-v2\response.body", 1024, DateTimeOffset.UtcNow.AddDays(-30));
        fixture.WriteFile(@"Downloads\setup.msi", 1024, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"Documents\budget.xlsx", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);

        var parsed = StorageReviewSearch.FromText(" category: Python package cache ");
        Assert(parsed.IsActive, "Prefixed search with a term should be active.");
        Assert(parsed.Query == "category: Python package cache", "Prefixed search should preserve display query text.");
        Assert(parsed.Field == StorageReviewSearchField.Category, "Prefixed search should record the requested field.");
        Assert(parsed.Term == "Python package cache", "Prefixed search should strip the field prefix for matching.");
        Assert(!StorageReviewSearch.FromText("category: ").IsActive, "Prefixed search without a term should be inactive.");
        Assert(StorageReviewSearch.FromText("unknown:setup").Field == StorageReviewSearchField.Any, "Unrecognized prefixes should stay broad search.");
        Assert(StorageReviewSearch.FromText("unknown:setup").Term == "unknown:setup", "Unrecognized prefixes should remain literal search text.");

        var categoryRows = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageReviewSearch.FromText("category:Python package cache"));
        Assert(categoryRows.Count > 0, "Category-prefixed search should match category text.");
        Assert(
            categoryRows.All(row => row.Entry.BloatCategories.Contains(BloatCategory.PythonPackageCache)),
            "Category-prefixed search should only match category fields.");

        var pathRows = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageReviewSearch.FromText("path:pip"));
        Assert(pathRows.Count > 0, "Path-prefixed search should match path text.");
        Assert(
            pathRows.All(row => row.Entry.FullPath.Contains("pip", StringComparison.OrdinalIgnoreCase)),
            "Path-prefixed search should only match full paths.");

        var categoryTextInPathField = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageReviewSearch.FromText("path:Python package cache"));
        Assert(categoryTextInPathField.Count == 0, "Path-prefixed search should not match category-only text.");

        var ratingRows = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageReviewSearch.FromText("rating:high risk"));
        Assert(ratingRows.Count > 0, "Rating-prefixed search should match user-facing rating text.");
        Assert(ratingRows.All(row => row.Entry.ImportanceRating == ImportanceRating.HighRisk), "Rating-prefixed search should only match ratings.");

        var recommendationRows = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageReviewSearch.FromText("recommendation:quarantine candidate"));
        Assert(recommendationRows.Count > 0, "Recommendation-prefixed search should match user-facing recommendation text.");
        Assert(
            recommendationRows.All(row => row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate),
            "Recommendation-prefixed search should only match recommendations.");
    }

    public void StorageScanSafetySummaryHighlightsReviewBoundaries()
    {
        var now = DateTimeOffset.UtcNow;
        var protectedEntry = new StorageEntry(
            @"C:\Users\moxhe\Documents",
            "Documents",
            IsDirectory: true,
            SizeBytes: 4096,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.ProtectedLocation],
            ImportanceRating: ImportanceRating.HighRisk,
            DeletionRecommendation: DeletionRecommendation.Keep,
            Evidence: "Protected location.",
            Children: []);
        var inaccessible = new StorageEntry(
            @"C:\Users\moxhe\Locked",
            "Locked",
            IsDirectory: true,
            SizeBytes: 0,
            LastModifiedUtc: null,
            IsAccessible: false,
            IsReparsePoint: false,
            ErrorMessage: "Access denied.",
            BloatCategories: [BloatCategory.AccessIssue],
            ImportanceRating: ImportanceRating.Caution,
            DeletionRecommendation: DeletionRecommendation.Inspect,
            Evidence: "The path could not be fully read.",
            Children: []);
        var reparsePoint = new StorageEntry(
            @"C:\Users\moxhe\Linked",
            "Linked",
            IsDirectory: true,
            SizeBytes: 0,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: true,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.ReparsePoint],
            ImportanceRating: ImportanceRating.Caution,
            DeletionRecommendation: DeletionRecommendation.Inspect,
            Evidence: "Reparse point.",
            Children: []);
        var quarantineCandidate = new StorageEntry(
            @"C:\Users\moxhe\Downloads\setup.msi",
            "setup.msi",
            IsDirectory: false,
            SizeBytes: 1024,
            LastModifiedUtc: now.AddDays(-120),
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.InstallerCache],
            ImportanceRating: ImportanceRating.LikelySafe,
            DeletionRecommendation: DeletionRecommendation.QuarantineCandidate,
            Evidence: "Installer.",
            Children: []);
        var uncategorized = new StorageEntry(
            @"C:\Users\moxhe\Unknown\notes.txt",
            "notes.txt",
            IsDirectory: false,
            SizeBytes: 512,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [],
            ImportanceRating: ImportanceRating.Caution,
            DeletionRecommendation: DeletionRecommendation.Inspect,
            Evidence: "No cleanup-specific category matched this file.",
            Children: []);
        var root = new StorageEntry(
            @"C:\Users\moxhe",
            "moxhe",
            IsDirectory: true,
            SizeBytes: 5632,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.ProfileContainer],
            ImportanceRating: ImportanceRating.Caution,
            DeletionRecommendation: DeletionRecommendation.Inspect,
            Evidence: "Root.",
            Children: [protectedEntry, inaccessible, reparsePoint, quarantineCandidate, uncategorized]);
        var result = new StorageScanResult(@"C:\Users\moxhe", now, now, root);
        var review = StorageScanReviewBuilder.Build(result);

        var summary = StorageScanSafetySummaryBuilder.Build(result, review);

        Assert(summary.TotalEntries == 6, "Safety summary should count flattened scan rows.");
        Assert(summary.HighRiskCount == 1, "Safety summary should count high-risk rows.");
        Assert(summary.ProtectedLocationCount == 1, "Safety summary should count protected locations.");
        Assert(summary.AccessIssueCount == 1, "Safety summary should count access issues.");
        Assert(summary.ReparsePointCount == 1, "Safety summary should count reparse points.");
        Assert(summary.QuarantineCandidateCount == 1, "Safety summary should count quarantine candidates.");
        Assert(summary.UncategorizedCount == 1, "Safety summary should count uncategorized rows.");
        Assert(summary.StatusLabel == "Review needed", "Safety summary should require review when warning signals exist.");
        Assert(summary.Notes.Any(note => note.Contains("No files were modified", StringComparison.OrdinalIgnoreCase)), "Safety notes should preserve the read-only boundary.");
        Assert(summary.Notes.Any(note => note.Contains("no permissions were changed", StringComparison.OrdinalIgnoreCase)), "Safety notes should not imply permission changes.");
        Assert(summary.Notes.Any(note => note.Contains("review candidates only", StringComparison.OrdinalIgnoreCase)), "Safety notes should not treat quarantine candidates as approval.");
    }

    public void StorageScanSafetyShortcutsMapToReadOnlyFilters()
    {
        AssertShortcut(
            StorageScanSafetyShortcut.HighRisk,
            StorageReviewFilter.HighRisk,
            StorageCategoryFilter.All,
            "High risk");
        AssertShortcut(
            StorageScanSafetyShortcut.ProtectedLocations,
            StorageReviewFilter.All,
            StorageCategoryFilter.ForCategory(BloatCategory.ProtectedLocation),
            "Protected locations");
        AssertShortcut(
            StorageScanSafetyShortcut.AccessIssues,
            StorageReviewFilter.AccessIssues,
            StorageCategoryFilter.All,
            "Access issues");
        AssertShortcut(
            StorageScanSafetyShortcut.ReparsePoints,
            StorageReviewFilter.All,
            StorageCategoryFilter.ForCategory(BloatCategory.ReparsePoint),
            "Reparse points");
        AssertShortcut(
            StorageScanSafetyShortcut.QuarantineCandidates,
            StorageReviewFilter.QuarantineCandidates,
            StorageCategoryFilter.All,
            "Quarantine candidates");
        AssertShortcut(
            StorageScanSafetyShortcut.Uncategorized,
            StorageReviewFilter.All,
            StorageCategoryFilter.NoCategory,
            "No category");
    }

    public void ReviewShortlistTracksSelectedRowsWithoutModifyingReview()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-installer.msi", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"Unknown\notes.txt", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);
        var installer = review.Entries.Single(row => row.Entry.Name.Equals("old-installer.msi", StringComparison.OrdinalIgnoreCase));
        var notes = review.Entries.Single(row => row.Entry.Name.Equals("notes.txt", StringComparison.OrdinalIgnoreCase));
        var originalEntryCount = review.Entries.Count;
        var shortlist = new StorageReviewShortlist();

        Assert(shortlist.Add(installer.Entry), "Adding a row to the shortlist should report a new selection.");
        Assert(!shortlist.Add(installer.Entry), "Adding the same row twice should not duplicate it.");
        Assert(shortlist.Add(notes.Entry), "Adding a second row should report a new selection.");
        Assert(shortlist.Count == 2, "Shortlist should count unique selected paths.");
        Assert(shortlist.Contains(installer.Entry), "Shortlist should contain selected entries.");

        var selectedRows = shortlist.ApplyTo(review.Entries);
        Assert(selectedRows.Count == 2, "Shortlist should project selected rows from review entries.");
        Assert(selectedRows[0].Entry.FullPath == installer.Entry.FullPath, "Shortlist projection should preserve review ordering.");
        Assert(review.Entries.Count == originalEntryCount, "Shortlist should not modify review entries.");

        Assert(shortlist.Remove(installer.Entry), "Removing a selected row should succeed.");
        Assert(!shortlist.Contains(installer.Entry), "Removed row should no longer be selected.");

        shortlist.Clear();
        Assert(shortlist.Count == 0, "Clear should remove every shortlisted path.");

        var addedMany = shortlist.AddMany([installer.Entry, installer.Entry, notes.Entry]);
        Assert(addedMany == 2, "Bulk add should count only newly shortlisted paths.");
        Assert(shortlist.Count == 2, "Bulk add should keep shortlist paths unique.");
        Assert(shortlist.Contains(installer.Entry) && shortlist.Contains(notes.Entry), "Bulk add should shortlist each unique path.");

        var removedMany = shortlist.RemoveMany([installer.Entry, installer.Entry]);
        Assert(removedMany == 1, "Bulk remove should count only paths that were shortlisted.");
        Assert(shortlist.Count == 1, "Bulk remove should update the shortlist count.");
        Assert(!shortlist.Contains(installer.Entry) && shortlist.Contains(notes.Entry), "Bulk remove should remove only matching paths.");
    }

    public void QuarantinePreviewBuildsReadOnlyPlanFromShortlist()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\OldInstallers\setup.msi", 1024 * 1024 * 2, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"Documents\important.txt", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"Unknown\notes.txt", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);
        var parent = SingleReviewEntry(review.Entries, @"Downloads\OldInstallers");
        var child = SingleReviewEntry(review.Entries, @"Downloads\OldInstallers\setup.msi");
        var protectedRow = SingleReviewEntry(review.Entries, @"Documents");
        var nonCandidate = SingleReviewEntry(review.Entries, @"Unknown\notes.txt");
        var quarantineRoot = Path.Combine(fixture.RootPath, "quarantine-root");

        var preview = QuarantinePreviewBuilder.Build(
            [child, parent, protectedRow, nonCandidate],
            fixture.RootPath,
            quarantineRoot);

        Assert(preview.IncludedCount == 1, "Preview should include only one non-overlapping eligible row.");
        Assert(preview.BlockedCount == 2, "Preview should block protected and non-candidate rows.");
        Assert(preview.RedundantCount == 1, "Preview should mark selected children as redundant when a parent is included.");
        Assert(preview.IncludedBytes == parent.Entry.SizeBytes, "Preview should not double-count a child covered by an included parent.");

        var parentPreview = preview.Entries.Single(entry => entry.SourcePath == parent.Entry.FullPath);
        Assert(parentPreview.Disposition == QuarantinePreviewDisposition.Included, "Eligible parent should be included.");
        Assert(
            parentPreview.DestinationPath == Path.GetFullPath(Path.Combine(quarantineRoot, "preview", "Downloads", "OldInstallers")),
            "Included entry should map under the quarantine preview root.");

        var childPreview = preview.Entries.Single(entry => entry.SourcePath == child.Entry.FullPath);
        Assert(childPreview.Disposition == QuarantinePreviewDisposition.Redundant, "Child should be redundant because parent is included.");

        var protectedPreview = preview.Entries.Single(entry => entry.SourcePath == protectedRow.Entry.FullPath);
        Assert(protectedPreview.Disposition == QuarantinePreviewDisposition.Blocked, "Protected row should be blocked.");
        Assert(protectedPreview.Reasons.Any(reason => reason.Contains("High-risk", StringComparison.OrdinalIgnoreCase)), "Protected row should explain high-risk blocking.");

        var nonCandidatePreview = preview.Entries.Single(entry => entry.SourcePath == nonCandidate.Entry.FullPath);
        Assert(nonCandidatePreview.Disposition == QuarantinePreviewDisposition.Blocked, "Non-candidate row should be blocked.");
        Assert(nonCandidatePreview.Reasons.Any(reason => reason.Contains("Quarantine candidate", StringComparison.OrdinalIgnoreCase)), "Non-candidate row should explain recommendation blocking.");
        Assert(!Directory.Exists(quarantineRoot), "Preview should not create the quarantine root folder.");
    }

    public void QuarantinePreviewCsvExporterWritesReviewReport()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\OldInstallers\setup, old.msi", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"Documents\important.txt", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);
        var installer = SingleReviewEntry(review.Entries, @"Downloads\OldInstallers\setup, old.msi");
        var protectedRow = SingleReviewEntry(review.Entries, @"Documents");
        var quarantineRoot = Path.Combine(fixture.RootPath, "quarantine-root");
        var preview = QuarantinePreviewBuilder.Build([installer, protectedRow], fixture.RootPath, quarantineRoot);

        var csv = QuarantinePreviewCsvExporter.Export(preview);

        Assert(csv.Contains("\"Cleanup scope\",\"Quarantine root\",\"Disposition\"", StringComparison.Ordinal), "Preview CSV should include header row.");
        Assert(csv.Contains("\"Included\"", StringComparison.Ordinal), "Preview CSV should include included rows.");
        Assert(csv.Contains("\"Blocked\"", StringComparison.Ordinal), "Preview CSV should include blocked rows.");
        Assert(csv.Contains("\"setup, old.msi\"", StringComparison.Ordinal), "Preview CSV should quote names with commas.");
        Assert(csv.Contains("\"No files were modified.\"", StringComparison.Ordinal), "Preview CSV should state that no files were modified.");
        Assert(csv.Contains("High-risk rows require manual review and are blocked from this preview.", StringComparison.Ordinal), "Preview CSV should export blocked reasons.");
        Assert(csv.Contains(Path.GetFullPath(Path.Combine(quarantineRoot, "preview", "Downloads", "OldInstallers", "setup, old.msi")), StringComparison.Ordinal), "Preview CSV should include destination paths for included rows.");
        Assert(!Directory.Exists(quarantineRoot), "Exporting a preview should not create the quarantine root folder.");
    }

    public void RestoreManifestDraftBuildsJsonUndoMetadataFromIncludedPreviewRows()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\OldInstallers\setup.msi", 1024 * 1024 * 2, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"Documents\important.txt", 1024, DateTimeOffset.UtcNow);
        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);
        var parent = SingleReviewEntry(review.Entries, @"Downloads\OldInstallers");
        var child = SingleReviewEntry(review.Entries, @"Downloads\OldInstallers\setup.msi");
        var protectedRow = SingleReviewEntry(review.Entries, @"Documents");
        var quarantineRoot = Path.Combine(fixture.RootPath, "quarantine-root");
        var preview = QuarantinePreviewBuilder.Build(
            [child, parent, protectedRow],
            fixture.RootPath,
            quarantineRoot);
        var draftedAtUtc = new DateTimeOffset(2026, 5, 28, 1, 2, 3, TimeSpan.Zero);

        var draft = RestoreManifestDraftBuilder.Build(preview, draftedAtUtc, "draft-test-1");
        var json = RestoreManifestDraftJsonSerializer.Serialize(draft);

        Assert(draft.SchemaVersion == RestoreManifestDraft.CurrentSchemaVersion, "Draft should use the current Restore Manifest schema version.");
        Assert(draft.DraftId == "draft-test-1", "Draft should preserve the provided draft id.");
        Assert(draft.DraftedAtUtc == draftedAtUtc, "Draft should preserve the drafted timestamp as UTC.");
        Assert(draft.EntryCount == 1, "Draft should include only included preview rows.");
        Assert(draft.TotalBytes == parent.Entry.SizeBytes, "Draft total bytes should reflect included rows only.");
        Assert(!draft.IsExecutedManifest, "Draft must not identify as an executed manifest.");

        var entry = draft.Entries.Single();
        Assert(entry.OriginalPath == parent.Entry.FullPath, "Draft entry should preserve the original path.");
        Assert(entry.QuarantinePath == Path.GetFullPath(Path.Combine(quarantineRoot, "preview", "Downloads", "OldInstallers")), "Draft entry should preserve the quarantine path.");
        Assert(entry.BloatCategories.Contains(BloatCategory.InstallerCache), "Draft entry should preserve category evidence.");
        Assert(!draft.Entries.Any(row => row.OriginalPath == child.Entry.FullPath), "Draft should exclude redundant preview rows.");
        Assert(!draft.Entries.Any(row => row.OriginalPath == protectedRow.Entry.FullPath), "Draft should exclude blocked preview rows.");
        Assert(json.Contains("\"schemaVersion\": \"restore-manifest.v1\"", StringComparison.Ordinal), "JSON should include the schema version.");
        Assert(json.Contains("\"isExecutedManifest\": false", StringComparison.Ordinal), "JSON should clearly identify the draft as not executed.");
        Assert(json.Contains("\"quarantinePath\"", StringComparison.Ordinal), "JSON should include quarantine paths for undo.");
        Assert(!Directory.Exists(quarantineRoot), "Building and serializing a Restore Manifest Draft should not create the quarantine root folder.");
    }

    public void QuarantineConfirmationDraftChecksPreviewAndManifestReadiness()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-installer.msi", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-120));
        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);
        var installer = SingleReviewEntry(review.Entries, @"Downloads\old-installer.msi");
        var quarantineRoot = Path.Combine(fixture.RootPath, "quarantine-root");
        var preview = QuarantinePreviewBuilder.Build([installer], fixture.RootPath, quarantineRoot);
        var manifestDraft = RestoreManifestDraftBuilder.Build(
            preview,
            new DateTimeOffset(2026, 5, 28, 1, 2, 3, TimeSpan.Zero),
            "manifest-draft-1");

        var confirmation = QuarantineConfirmationDraftBuilder.Build(
            preview,
            manifestDraft,
            new DateTimeOffset(2026, 5, 28, 2, 3, 4, TimeSpan.Zero),
            " confirmation-draft-1 ");

        Assert(confirmation.ConfirmationId == "confirmation-draft-1", "Confirmation draft should trim and preserve the provided id.");
        Assert(confirmation.RestoreManifestDraftId == "manifest-draft-1", "Confirmation draft should reference the Restore Manifest Draft id.");
        Assert(confirmation.IncludedCount == preview.IncludedCount, "Confirmation draft should copy the preview included count.");
        Assert(confirmation.IncludedBytes == preview.IncludedBytes, "Confirmation draft should copy preview included bytes.");
        Assert(confirmation.BlockedCount == 0, "Clean confirmation draft should have no blocked preview rows.");
        Assert(confirmation.RedundantCount == 0, "Clean confirmation draft should have no redundant preview rows.");
        Assert(!confirmation.HasDataBlockers, "Matching preview and manifest draft should have no data blockers.");
        Assert(!confirmation.IsExecutionImplemented, "Confirmation draft must not imply execution exists.");
        Assert(confirmation.RequiredConfirmationText == QuarantineConfirmationDraft.DefaultRequiredConfirmationText, "Confirmation draft should expose the future confirmation phrase.");
        Assert(confirmation.ReviewNotes.Any(note => note.Contains("No files were modified", StringComparison.OrdinalIgnoreCase)), "Confirmation notes should preserve the read-only boundary.");
        Assert(confirmation.ReviewNotes.Any(note => note.Contains("not implemented", StringComparison.OrdinalIgnoreCase)), "Confirmation notes should state execution is not implemented.");
        Assert(!Directory.Exists(quarantineRoot), "Building a Quarantine Confirmation Draft should not create the quarantine root folder.");
    }

    public void QuarantineConfirmationDraftReportsPreviewAndManifestBlockers()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\OldInstallers\setup.msi", 1024 * 1024 * 2, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"Documents\important.txt", 1024, DateTimeOffset.UtcNow);
        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);
        var parent = SingleReviewEntry(review.Entries, @"Downloads\OldInstallers");
        var child = SingleReviewEntry(review.Entries, @"Downloads\OldInstallers\setup.msi");
        var protectedRow = SingleReviewEntry(review.Entries, @"Documents");
        var quarantineRoot = Path.Combine(fixture.RootPath, "quarantine-root");
        var preview = QuarantinePreviewBuilder.Build(
            [child, parent, protectedRow],
            fixture.RootPath,
            quarantineRoot);
        var manifestDraft = RestoreManifestDraftBuilder.Build(
            preview,
            new DateTimeOffset(2026, 5, 28, 1, 2, 3, TimeSpan.Zero),
            "manifest-draft-2") with
        {
            QuarantineRootPath = Path.Combine(quarantineRoot, "other-root")
        };

        var confirmation = QuarantineConfirmationDraftBuilder.Build(
            preview,
            manifestDraft,
            new DateTimeOffset(2026, 5, 28, 2, 3, 4, TimeSpan.Zero),
            "confirmation-draft-2");

        Assert(confirmation.HasDataBlockers, "Confirmation draft should report preview or manifest mismatches.");
        Assert(confirmation.Blockers.Any(blocker => blocker.Contains("blocked preview row", StringComparison.OrdinalIgnoreCase)), "Confirmation draft should report blocked preview rows.");
        Assert(confirmation.Blockers.Any(blocker => blocker.Contains("redundant preview row", StringComparison.OrdinalIgnoreCase)), "Confirmation draft should report redundant preview rows.");
        Assert(confirmation.Blockers.Any(blocker => blocker.Contains("Quarantine root", StringComparison.OrdinalIgnoreCase)), "Confirmation draft should report quarantine root mismatches.");
        Assert(!Directory.Exists(quarantineRoot), "Blocked confirmation draft checks should not create the quarantine root folder.");
    }

    public void ChildSummaryShowsLargestImmediateChildren()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"AppData\Local\large-cache.bin", 1024 * 1024 * 2, DateTimeOffset.UtcNow.AddDays(-5));
        fixture.WriteFile(@"AppData\Roaming\medium-cache.bin", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-5));
        fixture.WriteFile(@"AppData\Local\Nested\deep-file.bin", 1024 * 1024 * 4, DateTimeOffset.UtcNow.AddDays(-5));
        fixture.WriteFile(@"Downloads\outside.bin", 1024 * 512, DateTimeOffset.UtcNow.AddDays(-120));

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var appData = Flatten(result.Root).Single(entry => entry.Name.Equals("AppData", StringComparison.OrdinalIgnoreCase));
        var summary = StorageChildSummaryBuilder.Build(appData, maxChildren: 2);

        Assert(summary.Count == 2, "Child summary should respect the max child count.");
        Assert(summary[0].Name == "Local", "Largest immediate child should be Local because it contains nested storage.");
        Assert(summary[1].Name == "Roaming", "Second largest immediate child should be Roaming.");
        Assert(summary.All(child => Path.GetDirectoryName(child.FullPath)?.Equals(appData.FullPath, StringComparison.OrdinalIgnoreCase) == true), "Child summary should only include immediate children.");

        var file = Flatten(result.Root).Single(entry => entry.Name.Equals("outside.bin", StringComparison.OrdinalIgnoreCase));
        Assert(StorageChildSummaryBuilder.Build(file).Count == 0, "Files should not have child summaries.");
    }

    public void SelectedPathReviewGuidanceExplainsReviewNextSteps()
    {
        var now = DateTimeOffset.UtcNow;
        var profile = new StorageEntry(
            @"C:\Users\moxhe",
            "moxhe",
            IsDirectory: true,
            SizeBytes: 1024,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.ProfileContainer],
            ImportanceRating: ImportanceRating.Caution,
            DeletionRecommendation: DeletionRecommendation.Inspect,
            Evidence: "Profile container.",
            Children: []);
        var protectedEntry = new StorageEntry(
            @"C:\Users\moxhe\Documents",
            "Documents",
            IsDirectory: true,
            SizeBytes: 1024,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.ProtectedLocation],
            ImportanceRating: ImportanceRating.HighRisk,
            DeletionRecommendation: DeletionRecommendation.Keep,
            Evidence: "Protected location.",
            Children: []);
        var installer = new StorageEntry(
            @"C:\Users\moxhe\Downloads\old-installer.msi",
            "old-installer.msi",
            IsDirectory: false,
            SizeBytes: 1024,
            LastModifiedUtc: now.AddDays(-120),
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.OldDownload, BloatCategory.InstallerCache],
            ImportanceRating: ImportanceRating.LikelySafe,
            DeletionRecommendation: DeletionRecommendation.QuarantineCandidate,
            Evidence: "Old installer.",
            Children: []);
        var uncategorized = new StorageEntry(
            @"C:\Users\moxhe\Unknown\notes.txt",
            "notes.txt",
            IsDirectory: false,
            SizeBytes: 1024,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [],
            ImportanceRating: ImportanceRating.Caution,
            DeletionRecommendation: DeletionRecommendation.Inspect,
            Evidence: "No cleanup-specific category matched this file.",
            Children: []);

        var profileGuidance = SelectedPathReviewGuidanceBuilder.Build(profile);
        Assert(profileGuidance.ActionLabel == "Inspect children, not the container", "Profile containers should direct review to child rows.");
        Assert(
            profileGuidance.Notes.Any(note => note.Contains("whole profile container", StringComparison.OrdinalIgnoreCase)),
            "Profile guidance should warn against whole-container cleanup.");

        var protectedGuidance = SelectedPathReviewGuidanceBuilder.Build(protectedEntry);
        Assert(protectedGuidance.ActionLabel == "Keep by default", "Protected rows should be keep-by-default guidance.");
        Assert(
            protectedGuidance.Notes.Any(note => note.Contains("specific reviewed cache rows", StringComparison.OrdinalIgnoreCase)),
            "Protected guidance should steer cleanup toward specific reviewed cache rows later.");

        var installerGuidance = SelectedPathReviewGuidanceBuilder.Build(installer);
        Assert(installerGuidance.ActionLabel == "Shortlist after review", "Likely-safe quarantine candidates should still require review before shortlisting.");
        Assert(
            installerGuidance.Notes.Any(note => note.Contains("not deletion approval", StringComparison.OrdinalIgnoreCase)),
            "Quarantine candidate guidance should not imply deletion approval.");

        var uncategorizedGuidance = SelectedPathReviewGuidanceBuilder.Build(uncategorized);
        Assert(uncategorizedGuidance.ActionLabel == "Classify before cleanup", "Uncategorized rows should require classification before cleanup.");
    }

    public void PathInspectionPlanBuildsExplorerArguments()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-installer.msi", 1024, DateTimeOffset.UtcNow.AddDays(-120));

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var downloads = Flatten(result.Root).Single(entry => entry.Name.Equals("Downloads", StringComparison.OrdinalIgnoreCase));
        var installer = Flatten(result.Root).Single(entry => entry.Name.Equals("old-installer.msi", StringComparison.OrdinalIgnoreCase));

        var folderPlan = PathInspectionPlanBuilder.Build(downloads);
        Assert(folderPlan.PathToCopy == downloads.FullPath, "Folder path should be copied exactly.");
        Assert(folderPlan.ExplorerFileName == "explorer.exe", "Explorer executable should be explorer.exe.");
        Assert(folderPlan.ExplorerArguments == $"\"{downloads.FullPath}\"", "Folder plan should open the folder directly.");

        var filePlan = PathInspectionPlanBuilder.Build(installer);
        Assert(filePlan.PathToCopy == installer.FullPath, "File path should be copied exactly.");
        Assert(filePlan.ExplorerArguments == $"/select,\"{installer.FullPath}\"", "File plan should ask Explorer to select the file.");
    }

    public void SelectedFileContentPreviewReadsBoundedTextOnly()
    {
        using var fixture = TestFixture.Create();
        var now = DateTimeOffset.UtcNow;

        fixture.WriteText(@"Unknown\notes.txt", "first line\nsecond line\nthird line", now);
        fixture.WriteBytes(@"Unknown\binary.bin", [0, 1, 2, 3, 4, 5], now);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var rows = Flatten(result.Root).ToArray();

        var notes = Single(rows, "notes.txt");
        var textPreview = SelectedFileContentPreviewBuilder.Build(notes, maxBytes: 128, maxLines: 2);
        Assert(textPreview.IsContentShown, "Text file preview should show bounded text content.");
        Assert(textPreview.Content.Contains("first line", StringComparison.Ordinal), "Text preview should include the selected file content.");
        Assert(!textPreview.Content.Contains("third line", StringComparison.Ordinal), "Text preview should respect the line limit.");
        Assert(textPreview.IsTruncated, "Text preview should report truncation when byte or line limits are reached.");
        Assert(textPreview.Message.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Text preview should preserve read-only wording.");

        var binary = Single(rows, "binary.bin");
        var binaryPreview = SelectedFileContentPreviewBuilder.Build(binary);
        Assert(!binaryPreview.IsContentShown, "Binary content should not be rendered as text.");
        Assert(binaryPreview.Label.Contains("Binary", StringComparison.OrdinalIgnoreCase), "Binary preview should explain why content is hidden.");
        Assert(binaryPreview.Message.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Binary preview should preserve read-only wording.");

        var rootPreview = SelectedFileContentPreviewBuilder.Build(result.Root);
        Assert(!rootPreview.IsContentShown, "Folder preview should not try to read folder content as a file.");
        Assert(rootPreview.Label.Contains("Folder", StringComparison.OrdinalIgnoreCase), "Folder preview should explain that only files can be previewed.");
    }

    public void CsvExporterWritesEscapedReviewRows()
    {
        var entry = new StorageEntry(
            @"C:\Users\moxhe\Downloads\setup, old.msi",
            "setup, old.msi",
            IsDirectory: false,
            SizeBytes: 2048,
            LastModifiedUtc: new DateTimeOffset(2026, 5, 28, 1, 2, 3, TimeSpan.Zero),
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.OldDownload, BloatCategory.InstallerCache],
            ImportanceRating: ImportanceRating.LikelySafe,
            DeletionRecommendation: DeletionRecommendation.QuarantineCandidate,
            Evidence: "Installer, old download with \"quoted\" evidence.",
            Children: []);

        var csv = StorageScanCsvExporter.Export([new StorageReviewEntry(entry, Depth: 2)]);

        Assert(csv.Contains("\"Full path\",\"Parent path\",\"Depth\",\"Name\",\"Type\",\"Size bytes\"", StringComparison.Ordinal), "CSV should include header row.");
        Assert(csv.Contains("\"C:\\Users\\moxhe\\Downloads\\setup, old.msi\"", StringComparison.Ordinal), "CSV should quote paths with commas.");
        Assert(csv.Contains("\"C:\\Users\\moxhe\\Downloads\",\"2\"", StringComparison.Ordinal), "CSV should include same-scope hierarchy context.");
        Assert(csv.Contains("\"Likely safe\"", StringComparison.Ordinal), "CSV should use user-facing importance labels.");
        Assert(csv.Contains("\"Quarantine candidate\"", StringComparison.Ordinal), "CSV should use user-facing recommendation labels.");
        Assert(csv.Contains("\"Old download; Installer cache\"", StringComparison.Ordinal), "CSV should export formatted categories.");
        Assert(csv.Contains("\"Installer, old download with \"\"quoted\"\" evidence.\"", StringComparison.Ordinal), "CSV should escape quotes in evidence.");
    }

    public void ByteSizeFormatterUsesReadableUnits()
    {
        Assert(ByteSizeFormatter.Format(0) == "0 B", "0 bytes should be 0 B.");
        Assert(ByteSizeFormatter.Format(1024) == "1 KB", "1024 bytes should be 1 KB.");
        Assert(ByteSizeFormatter.Format(1024 * 1024 * 3) == "3 MB", "3 MiB should be 3 MB.");
    }

    public void ProductionCodeDoesNotContainCleanupExecutionCalls()
    {
        var repositoryRoot = FindRepositoryRoot();
        var sourceRoot = Path.Combine(repositoryRoot, "src");
        var sourceFiles = Directory.EnumerateFiles(sourceRoot, "*.cs", SearchOption.AllDirectories).ToArray();
        var blockedTokens = new[]
        {
            "Directory.CreateDirectory(",
            "Directory.Delete(",
            "Directory.Move(",
            "File.Copy(",
            "File.Delete(",
            "File.Move(",
            "File.Replace(",
            "File.SetAttributes(",
            "File.WriteAllBytes("
        };

        var blockedMatches = sourceFiles
            .SelectMany(file => File.ReadLines(file)
                .Select((line, index) => new SourceLine(file, index + 1, line))
                .Where(sourceLine => blockedTokens.Any(token => sourceLine.Text.Contains(token, StringComparison.Ordinal))))
            .ToArray();

        Assert(blockedMatches.Length == 0, "Production code should not contain cleanup execution filesystem calls: " + FormatSourceLines(blockedMatches));

        var reportWriteMatches = sourceFiles
            .SelectMany(file => File.ReadLines(file)
                .Select((line, index) => new SourceLine(file, index + 1, line))
                .Where(sourceLine => sourceLine.Text.Contains("File.WriteAllText(", StringComparison.Ordinal)))
            .ToArray();

        Assert(reportWriteMatches.Length == 3, "Only the three user-selected CSV report writes should use File.WriteAllText.");
        Assert(
            reportWriteMatches.All(match =>
                match.FilePath.EndsWith(@"src\WindowsFileCleaner.App\MainWindow.xaml.cs", StringComparison.OrdinalIgnoreCase)
                && match.Text.Contains("dialog.FileName", StringComparison.Ordinal)),
            "File.WriteAllText should only write user-selected report exports.");
    }

    private static StorageEntry Single(IEnumerable<StorageEntry> entries, string name)
    {
        return entries.Single(entry => entry.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private static StorageReviewEntry SingleReviewEntry(IEnumerable<StorageReviewEntry> entries, string relativePath)
    {
        return entries.Single(entry => entry.Entry.FullPath.EndsWith(relativePath, StringComparison.OrdinalIgnoreCase));
    }

    private static void AssertShortcut(
        StorageScanSafetyShortcut shortcut,
        StorageReviewFilter expectedReviewFilter,
        StorageCategoryFilter expectedCategoryFilter,
        string expectedLabel)
    {
        var result = StorageScanSafetyShortcutFilterBuilder.Build(shortcut);
        Assert(result.ReviewFilter == expectedReviewFilter, $"{shortcut} should map to the expected review filter.");
        Assert(result.CategoryFilter == expectedCategoryFilter, $"{shortcut} should map to the expected category filter.");
        Assert(result.Label == expectedLabel, $"{shortcut} should use the expected label.");
    }

    private static IEnumerable<StorageEntry> Flatten(StorageEntry entry)
    {
        yield return entry;

        foreach (var child in entry.Children.SelectMany(Flatten))
        {
            yield return child;
        }
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(Environment.CurrentDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "WindowsFileCleaner.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not find repository root from test working directory.");
    }

    private static string FormatSourceLines(IReadOnlyList<SourceLine> sourceLines)
    {
        return sourceLines.Count == 0
            ? "none"
            : string.Join("; ", sourceLines.Select(line => $"{line.FilePath}:{line.LineNumber}: {line.Text.Trim()}"));
    }

    private sealed record SourceLine(string FilePath, int LineNumber, string Text);
}

internal sealed class TestFixture : IDisposable
{
    private TestFixture(string rootPath)
    {
        RootPath = rootPath;
    }

    public string RootPath { get; }

    public static TestFixture Create()
    {
        var root = Path.Combine(
            Environment.CurrentDirectory,
            "test-fixtures",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(root);
        return new TestFixture(root);
    }

    public void WriteFile(string relativePath, int byteCount, DateTimeOffset lastModifiedUtc)
    {
        var fullPath = Path.Combine(RootPath, relativePath);
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllBytes(fullPath, Enumerable.Repeat((byte)'x', byteCount).ToArray());
        File.SetLastWriteTimeUtc(fullPath, lastModifiedUtc.UtcDateTime);
    }

    public void WriteText(string relativePath, string content, DateTimeOffset lastModifiedUtc)
    {
        var fullPath = Path.Combine(RootPath, relativePath);
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(fullPath, content);
        File.SetLastWriteTimeUtc(fullPath, lastModifiedUtc.UtcDateTime);
    }

    public void WriteBytes(string relativePath, byte[] content, DateTimeOffset lastModifiedUtc)
    {
        var fullPath = Path.Combine(RootPath, relativePath);
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllBytes(fullPath, content);
        File.SetLastWriteTimeUtc(fullPath, lastModifiedUtc.UtcDateTime);
    }

    public void Dispose()
    {
        if (Directory.Exists(RootPath))
        {
            Directory.Delete(RootPath, recursive: true);
        }
    }
}
