using WindowsFileCleaner.Core;

var tests = new StorageScanTests();
tests.ScannerTotalsFilesAndClassifiesCandidates();
tests.ScannerRefusesToLeaveCleanupScope();
tests.ClassifierLabelsRealScanContainerPatterns();
tests.ReviewBuilderSummarizesAndFiltersResults();
tests.ReviewBuilderFiltersAccessIssues();
tests.ReviewShortlistTracksSelectedRowsWithoutModifyingReview();
tests.ChildSummaryShowsLargestImmediateChildren();
tests.PathInspectionPlanBuildsExplorerArguments();
tests.CsvExporterWritesEscapedReviewRows();
tests.ByteSizeFormatterUsesReadableUnits();

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

    public void ClassifierLabelsRealScanContainerPatterns()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"AppData\Local\pip\Cache\http-v2\response.body", 1024, DateTimeOffset.UtcNow.AddDays(-30));
        fixture.WriteFile(@"AppData\Local\NVIDIA\DXCache\shader.bin", 2048, DateTimeOffset.UtcNow.AddDays(-5));
        fixture.WriteFile(@"AppData\Local\Google\Chrome\User Data\Default\Preferences", 1024, DateTimeOffset.UtcNow);

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

        var csv = StorageScanCsvExporter.Export([new StorageReviewEntry(entry, Depth: 0)]);

        Assert(csv.Contains("\"Full path\",\"Name\",\"Type\",\"Size bytes\"", StringComparison.Ordinal), "CSV should include header row.");
        Assert(csv.Contains("\"C:\\Users\\moxhe\\Downloads\\setup, old.msi\"", StringComparison.Ordinal), "CSV should quote paths with commas.");
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

    private static StorageEntry Single(IEnumerable<StorageEntry> entries, string name)
    {
        return entries.Single(entry => entry.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
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

    public void Dispose()
    {
        if (Directory.Exists(RootPath))
        {
            Directory.Delete(RootPath, recursive: true);
        }
    }
}
