using WindowsFileCleaner.Core;

var tests = new StorageScanTests();
tests.ScannerTotalsFilesAndClassifiesCandidates();
tests.ScannerMarksCleanupScopeRootAsProtectedKeep();
tests.ScannerRefusesToLeaveCleanupScope();
tests.LaunchOptionsDefaultAndScopeArgument();
tests.CleanupScopeSafetyNoteDistinguishesFixtureAndRealProfile();
tests.CleanupScopeScanGateRequiresRealProfileAcknowledgement();
tests.ClassifierLabelsRealScanContainerPatterns();
tests.ClassifierProtectsCloudSyncAndCredentialData();
tests.ClassifierLabelsLargeOldUnknownFilesConservatively();
tests.ReviewBuilderSummarizesAndFiltersResults();
tests.ReviewBuilderFiltersBySizeThreshold();
tests.ReviewBuilderFiltersAccessIssues();
tests.ReviewSearchCombinesWithReviewAndCategoryFilters();
tests.ReviewSearchSupportsFieldPrefixes();
tests.StorageScanSafetySummaryHighlightsReviewBoundaries();
tests.StorageScanSafetyShortcutsMapToReadOnlyFilters();
tests.ReviewShortlistTracksSelectedRowsWithoutModifyingReview();
tests.QuarantinePreviewBuildsReadOnlyPlanFromShortlist();
tests.QuarantineRootSafetyNoteRequiresFullyQualifiedPreviewRoot();
tests.QuarantinePreviewBlocksParentsWithProtectedDescendants();
tests.QuarantinePreviewCsvExporterWritesReviewReport();
tests.RestoreManifestDraftBuildsJsonUndoMetadataFromIncludedPreviewRows();
tests.QuarantineConfirmationDraftChecksPreviewAndManifestReadiness();
tests.QuarantineConfirmationDraftReportsPreviewAndManifestBlockers();
tests.QuarantineExecutionGateRequiresExactConfirmationAndImplementedExecution();
tests.QuarantineActionDraftBuildsActionScopedLayoutWithoutWritingFiles();
tests.RestoreManifestBuildsWriteAheadActionRecordFromActionDraft();
tests.RestoreManifestTracksPartialFailureStatusForFutureExecution();
tests.RestoreManifestFileStoreWritesAndReplacesManifestWithoutMovingSources();
tests.RestoreManifestFileStoreRejectsPathsOutsideActionRoot();
tests.QuarantineExecutorMovesFixtureFilesWithWriteAheadManifest();
tests.QuarantineExecutorRecordsPartialFailureWithoutOverwritingDestination();
tests.QuarantineExecutorFailsMissingSourceWithoutCreatingDestination();
tests.QuarantineExecutorStopsBeforeMovesWhenInitialManifestWriteFails();
tests.QuarantineExecutorStopsAfterPostMoveManifestWriteFails();
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

    public void ScannerMarksCleanupScopeRootAsProtectedKeep()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Unknown\notes.txt", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));

        Assert(result.Root.FullPath == Path.GetFullPath(fixture.RootPath), "Storage Scan root should match the Cleanup Scope.");
        Assert(result.Root.BloatCategories.Contains(BloatCategory.CleanupScopeRoot), "Cleanup Scope root should have an explicit category.");
        Assert(result.Root.BloatCategories.Contains(BloatCategory.ProtectedLocation), "Cleanup Scope root should be protected.");
        Assert(result.Root.ImportanceRating == ImportanceRating.HighRisk, "Cleanup Scope root should be high risk.");
        Assert(result.Root.DeletionRecommendation == DeletionRecommendation.Keep, "Cleanup Scope root should be kept.");
        Assert(result.Root.Evidence.Contains("Cleanup Scope root", StringComparison.OrdinalIgnoreCase), "Cleanup Scope root should explain why it is not a cleanup target.");
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

    public void CleanupScopeScanGateRequiresRealProfileAcknowledgement()
    {
        var realProfile = CleanupScopeScanGateBuilder.Build(@"C:\Users\moxhe", realProfilePreflightAcknowledged: false);
        Assert(!realProfile.CanScan, "Real profile scans should require explicit preflight and fixture-review acknowledgement.");
        Assert(realProfile.RequiresPreflightAcknowledgement, "Real profile scan gate should report the acknowledgement requirement.");

        var confirmedRealProfile = CleanupScopeScanGateBuilder.Build(@"C:\Users\moxhe", realProfilePreflightAcknowledged: true);
        Assert(confirmedRealProfile.CanScan, "Real profile scans should be allowed after explicit acknowledgement.");
        Assert(confirmedRealProfile.Message.Contains("read-only", StringComparison.OrdinalIgnoreCase), "Confirmed real-profile gate should keep read-only wording.");

        var fixture = CleanupScopeScanGateBuilder.Build(@"D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture", realProfilePreflightAcknowledged: false);
        Assert(fixture.CanScan, "Fixture scans should not require real-profile acknowledgement.");
        Assert(!fixture.RequiresPreflightAcknowledgement, "Fixture scan gate should not ask for real-profile acknowledgement.");

        var blank = CleanupScopeScanGateBuilder.Build(" ", realProfilePreflightAcknowledged: true);
        Assert(!blank.CanScan, "Blank Cleanup Scope should not be scannable.");
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
        fixture.WriteFile(@"AppData\Roaming\.minecraft\mods\OptiFine\config.json", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"AppData\Roaming\CurseForge\minecraft\Instances\SomePack\options.txt", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"AppData\Roaming\Vortex\downloads\skyrim\mod.zip", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var rows = Flatten(result.Root).ToArray();

        var pip = Single(rows, "pip");
        Assert(pip.BloatCategories.Contains(BloatCategory.ApplicationDataArea), "pip should be marked as AppData area.");
        Assert(pip.BloatCategories.Contains(BloatCategory.PythonPackageCache), "pip should be marked as Python package cache.");
        Assert(pip.ImportanceRating == ImportanceRating.Caution, "Broad pip container should stay caution until a specific cache child is selected.");

        var pipCache = Single(rows, "Cache");
        Assert(pipCache.BloatCategories.Contains(BloatCategory.AppCache), "pip Cache should be marked as app cache.");
        Assert(pipCache.BloatCategories.Contains(BloatCategory.PythonPackageCache), "pip Cache should be marked as Python package cache.");
        Assert(pipCache.ImportanceRating == ImportanceRating.LikelySafe, "Specific pip cache rows should be likely safe.");
        Assert(pipCache.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate, "Specific pip cache rows should be quarantine candidates.");

        var dxCache = Single(rows, "DXCache");
        Assert(dxCache.BloatCategories.Contains(BloatCategory.GpuShaderCache), "DXCache should be marked as GPU shader cache.");
        Assert(dxCache.ImportanceRating == ImportanceRating.LikelySafe, "Specific GPU shader cache rows should be likely safe.");
        Assert(dxCache.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate, "Specific GPU shader cache rows should be quarantine candidates.");

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

        var optifine = Single(rows, "OptiFine");
        Assert(optifine.BloatCategories.Contains(BloatCategory.GameData), "OptiFine folders should be marked as game data.");
        Assert(optifine.BloatCategories.Contains(BloatCategory.ProtectedLocation), "OptiFine folders should be protected.");
        Assert(optifine.ImportanceRating == ImportanceRating.HighRisk, "OptiFine folders should be high risk.");
        Assert(optifine.DeletionRecommendation == DeletionRecommendation.Keep, "OptiFine folders should be kept.");

        var curseForge = Single(rows, "CurseForge");
        Assert(curseForge.BloatCategories.Contains(BloatCategory.GameData), "CurseForge folders should be marked as game data.");
        Assert(curseForge.ImportanceRating == ImportanceRating.HighRisk, "CurseForge folders should be high risk.");
        Assert(curseForge.DeletionRecommendation == DeletionRecommendation.Keep, "CurseForge folders should be kept.");

        var vortex = Single(rows, "Vortex");
        Assert(vortex.BloatCategories.Contains(BloatCategory.GameData), "Vortex mod manager folders should be marked as game data.");
        Assert(vortex.ImportanceRating == ImportanceRating.HighRisk, "Vortex mod manager folders should be high risk.");
        Assert(vortex.DeletionRecommendation == DeletionRecommendation.Keep, "Vortex mod manager folders should be kept.");
    }

    public void ClassifierProtectsCloudSyncAndCredentialData()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"OneDrive - Personal\Archive\cloud-note.txt", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"Dropbox\Exports\shared-file.txt", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@".ssh\id_ed25519", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"AppData\Roaming\Bitwarden\data.json", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"Documents\Passwords\vault.kdbx", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var rows = Flatten(result.Root).ToArray();

        var oneDrive = Single(rows, "OneDrive - Personal");
        Assert(oneDrive.BloatCategories.Contains(BloatCategory.CloudSyncData), "OneDrive folders should be marked as cloud sync data.");
        Assert(oneDrive.BloatCategories.Contains(BloatCategory.ProtectedLocation), "OneDrive folders should be protected.");
        Assert(oneDrive.ImportanceRating == ImportanceRating.HighRisk, "OneDrive folders should be high risk.");
        Assert(oneDrive.DeletionRecommendation == DeletionRecommendation.Keep, "OneDrive folders should be kept.");

        var dropbox = Single(rows, "Dropbox");
        Assert(dropbox.BloatCategories.Contains(BloatCategory.CloudSyncData), "Dropbox folders should be marked as cloud sync data.");
        Assert(dropbox.ImportanceRating == ImportanceRating.HighRisk, "Dropbox folders should be high risk.");
        Assert(dropbox.DeletionRecommendation == DeletionRecommendation.Keep, "Dropbox folders should be kept.");

        var sshKey = Single(rows, "id_ed25519");
        Assert(sshKey.BloatCategories.Contains(BloatCategory.CredentialData), "SSH private keys should be marked as credential data.");
        Assert(sshKey.BloatCategories.Contains(BloatCategory.ProtectedLocation), "SSH private keys should be protected.");
        Assert(sshKey.ImportanceRating == ImportanceRating.HighRisk, "SSH private keys should be high risk.");
        Assert(sshKey.DeletionRecommendation == DeletionRecommendation.Keep, "SSH private keys should be kept.");

        var bitwarden = Single(rows, "Bitwarden");
        Assert(bitwarden.BloatCategories.Contains(BloatCategory.CredentialData), "Bitwarden folders should be marked as credential data.");
        Assert(bitwarden.ImportanceRating == ImportanceRating.HighRisk, "Bitwarden folders should be high risk.");
        Assert(bitwarden.DeletionRecommendation == DeletionRecommendation.Keep, "Bitwarden folders should be kept.");

        var passwordVault = Single(rows, "vault.kdbx");
        Assert(passwordVault.BloatCategories.Contains(BloatCategory.CredentialData), "KeePass vault files should be marked as credential data.");
        Assert(passwordVault.ImportanceRating == ImportanceRating.HighRisk, "KeePass vault files should be high risk.");
        Assert(passwordVault.DeletionRecommendation == DeletionRecommendation.Keep, "KeePass vault files should be kept.");
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

    public void ReviewBuilderFiltersBySizeThreshold()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-installer.msi", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"Unknown\notes.txt", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);

        var oneMegabyteRows = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageEntryTypeFilter.All,
            StorageSizeThresholdFilter.AtLeast1Mb,
            StorageReviewSearch.Empty);
        Assert(oneMegabyteRows.Count > 0, "1 MB+ size threshold should return matching rows.");
        Assert(
            oneMegabyteRows.All(row => row.Entry.SizeBytes >= 1024L * 1024),
            "1 MB+ size threshold should only return rows at or above the threshold.");

        var largeInstallerFiles = review.ApplyFilter(
            StorageReviewFilter.QuarantineCandidates,
            StorageCategoryFilter.ForCategory(BloatCategory.InstallerCache),
            StorageEntryTypeFilter.Files,
            StorageSizeThresholdFilter.AtLeast1Mb,
            StorageReviewSearch.FromText("old-installer"));
        Assert(largeInstallerFiles.Count == 1, "Size threshold should combine with review, category, type, and search filters.");
        Assert(
            largeInstallerFiles.Single().Entry.FullPath.EndsWith(@"Downloads\old-installer.msi", StringComparison.OrdinalIgnoreCase),
            "Combined size-threshold row should preserve the expected installer match.");

        var hundredMegabyteRows = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageEntryTypeFilter.All,
            StorageSizeThresholdFilter.AtLeast100Mb,
            StorageReviewSearch.Empty);
        Assert(hundredMegabyteRows.Count == 0, "100 MB+ size threshold should hide smaller fixture rows.");
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
        var accessStatusRows = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageReviewSearch.FromText("access:access issue"));
        var accessMessageRows = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageReviewSearch.FromText("issue:denied"));
        var readableRows = review.ApplyFilter(
            StorageReviewFilter.All,
            StorageCategoryFilter.All,
            StorageReviewSearch.FromText("access:readable"));

        Assert(review.Summary.AccessIssueCount == 1, "Review should count access issue rows.");
        Assert(review.Summary.AccessIssueLargestEntryBytes == 0, "Access issue largest row should reflect the unreadable row size.");
        Assert(accessRows.Count == 1, "Access issue filter should only return inaccessible rows.");
        Assert(accessRows[0].Entry.FullPath.EndsWith(@"\Locked", StringComparison.OrdinalIgnoreCase), "Access issue filter should return the inaccessible path.");
        Assert(accessStatusRows.Count == 1, "Access-prefixed search should match access issue status.");
        Assert(accessStatusRows[0].Entry.FullPath.EndsWith(@"\Locked", StringComparison.OrdinalIgnoreCase), "Access status search should return the inaccessible path.");
        Assert(accessMessageRows.Count == 1, "Issue-prefixed search should match access issue messages.");
        Assert(readableRows.Count > 0, "Access-prefixed search should match readable status.");
        Assert(readableRows.All(row => row.Entry.IsAccessible), "Readable access search should only return readable rows.");
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
        var largerQuarantineCandidate = quarantineCandidate with
        {
            FullPath = @"C:\Users\moxhe\AppData\Local\NVIDIA\DXCache",
            Name = "DXCache",
            IsDirectory = true,
            SizeBytes = 2048,
            BloatCategories = [BloatCategory.AppCache, BloatCategory.GpuShaderCache],
            Evidence = "GPU shader cache."
        };
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
        var largerUncategorized = uncategorized with
        {
            FullPath = @"C:\Users\moxhe\Unknown\large.bin",
            Name = "large.bin",
            SizeBytes = 1536
        };
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
            Children: [protectedEntry, inaccessible, reparsePoint, quarantineCandidate, largerQuarantineCandidate, uncategorized, largerUncategorized]);
        var result = new StorageScanResult(@"C:\Users\moxhe", now, now, root);
        var review = StorageScanReviewBuilder.Build(result);

        var summary = StorageScanSafetySummaryBuilder.Build(result, review);

        Assert(summary.TotalEntries == 8, "Safety summary should count flattened scan rows.");
        Assert(summary.HighRiskCount == 1, "Safety summary should count high-risk rows.");
        Assert(summary.ProtectedLocationCount == 1, "Safety summary should count protected locations.");
        Assert(summary.AccessIssueCount == 1, "Safety summary should count access issues.");
        Assert(summary.AccessIssueExamples.Count == 1, "Safety summary should expose bounded access issue examples.");
        Assert(
            summary.AccessIssueExamples[0].Contains("Locked", StringComparison.OrdinalIgnoreCase)
            && summary.AccessIssueExamples[0].Contains("Access denied", StringComparison.OrdinalIgnoreCase),
            "Safety summary access issue examples should include relative path and scanner error text.");
        Assert(summary.ReparsePointCount == 1, "Safety summary should count reparse points.");
        Assert(summary.QuarantineCandidateCount == 2, "Safety summary should count quarantine candidates.");
        Assert(summary.QuarantineCandidateExamples.Count == 2, "Safety summary should expose bounded quarantine candidate examples.");
        Assert(
            summary.QuarantineCandidateExamples[0].Contains(@"AppData\Local\NVIDIA\DXCache", StringComparison.OrdinalIgnoreCase)
            && summary.QuarantineCandidateExamples[0].Contains("2 KB", StringComparison.OrdinalIgnoreCase),
            "Safety summary candidate examples should show largest relative candidate path and size first.");
        Assert(
            summary.QuarantineCandidateExamples[1].Contains(@"Downloads\setup.msi", StringComparison.OrdinalIgnoreCase),
            "Safety summary candidate examples should include additional relative candidate paths.");
        Assert(summary.UncategorizedCount == 2, "Safety summary should count uncategorized rows.");
        Assert(summary.UncategorizedExamples.Count == 2, "Safety summary should expose bounded uncategorized examples.");
        Assert(
            summary.UncategorizedExamples[0].Contains(@"Unknown\large.bin", StringComparison.OrdinalIgnoreCase)
            && summary.UncategorizedExamples[0].Contains("1.5 KB", StringComparison.OrdinalIgnoreCase),
            "Safety summary no-category examples should show largest relative uncategorized path and size first.");
        Assert(
            summary.UncategorizedExamples[1].Contains(@"Unknown\notes.txt", StringComparison.OrdinalIgnoreCase),
            "Safety summary no-category examples should include additional relative uncategorized paths.");
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

    public void QuarantineRootSafetyNoteRequiresFullyQualifiedPreviewRoot()
    {
        var defaultNote = QuarantineRootSafetyNoteBuilder.Build(" ");

        Assert(defaultNote.CanPreview, "Blank Quarantine root should fall back to the default preview root.");
        Assert(defaultNote.RootPath == Path.GetFullPath(QuarantinePreviewBuilder.DefaultQuarantineRootPath), "Blank Quarantine root should resolve to the default path.");
        Assert(defaultNote.IsPreferredDrive, "Default Quarantine root should be on the preferred D: drive.");
        Assert(defaultNote.Message.Contains("does not create folders", StringComparison.OrdinalIgnoreCase), "Default note should preserve preview-only wording.");

        var customD = QuarantineRootSafetyNoteBuilder.Build(@"D:\ReviewQuarantine");
        Assert(customD.CanPreview, "Fully qualified D: roots should be usable for preview.");
        Assert(customD.IsPreferredDrive, "D: roots should be marked as preferred.");

        var nonD = QuarantineRootSafetyNoteBuilder.Build(@"C:\Temp\WindowsFileCleanerQuarantine");
        Assert(nonD.CanPreview, "Fully qualified non-D roots should still be usable for preview.");
        Assert(!nonD.IsPreferredDrive, "Non-D roots should not be marked as preferred.");
        Assert(nonD.Message.Contains("D:", StringComparison.OrdinalIgnoreCase), "Non-D roots should explain that D: remains preferred.");

        var relative = QuarantineRootSafetyNoteBuilder.Build(@"relative\quarantine");
        Assert(!relative.CanPreview, "Relative Quarantine roots should be blocked from preview.");
        Assert(relative.Message.Contains("fully qualified", StringComparison.OrdinalIgnoreCase), "Relative roots should ask for a fully qualified path.");
    }

    public void QuarantinePreviewBlocksParentsWithProtectedDescendants()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@".cache\general-cache.bin", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-30));
        fixture.WriteFile(@".cache\codex-runtimes\python.exe", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);
        var cacheParent = SingleReviewEntry(review.Entries, @".cache");
        var codexRuntime = SingleReviewEntry(review.Entries, @"codex-runtimes");
        var quarantineRoot = Path.Combine(fixture.RootPath, "quarantine-root");

        Assert(cacheParent.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate, "Synthetic cache parent should otherwise look eligible.");
        Assert(codexRuntime.Entry.BloatCategories.Contains(BloatCategory.ProtectedLocation), "Codex runtime descendant should be protected.");

        var preview = QuarantinePreviewBuilder.Build([cacheParent], fixture.RootPath, quarantineRoot);
        var cachePreview = preview.Entries.Single();

        Assert(preview.IncludedCount == 0, "Preview should not include a parent that contains protected descendants.");
        Assert(preview.BlockedCount == 1, "Preview should block the broad parent row.");
        Assert(cachePreview.Disposition == QuarantinePreviewDisposition.Blocked, "Cache parent should be blocked because of its protected descendant.");
        Assert(
            cachePreview.Reasons.Any(reason =>
                reason.Contains("descendant", StringComparison.OrdinalIgnoreCase)
                && reason.Contains(@".cache\codex-runtimes", StringComparison.OrdinalIgnoreCase)),
            "Blocked parent should explain protected descendant evidence with cleanup-scope-relative paths.");
        Assert(
            cachePreview.Reasons.All(reason => !reason.Contains(fixture.RootPath, StringComparison.OrdinalIgnoreCase)),
            "Blocked descendant reasons should not repeat the absolute Cleanup Scope path.");
        Assert(!Directory.Exists(quarantineRoot), "Blocked descendant preview should not create the quarantine root folder.");
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
        Assert(csv.Contains("\"Access status\",\"Access issue\",\"Preview note\"", StringComparison.Ordinal), "Preview CSV should include explicit access status columns.");
        Assert(csv.Contains("\"Included\"", StringComparison.Ordinal), "Preview CSV should include included rows.");
        Assert(csv.Contains("\"Blocked\"", StringComparison.Ordinal), "Preview CSV should include blocked rows.");
        Assert(csv.Contains("\"Readable\",\"\",\"No files were modified.\"", StringComparison.Ordinal), "Preview CSV should export readable access status.");
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

    public void QuarantineExecutionGateRequiresExactConfirmationAndImplementedExecution()
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
            new DateTimeOffset(2026, 5, 29, 1, 2, 3, TimeSpan.Zero),
            "manifest-draft-gate");
        var confirmation = QuarantineConfirmationDraftBuilder.Build(
            preview,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 2, 3, 4, TimeSpan.Zero),
            "confirmation-draft-gate");

        var missingPreviewGate = QuarantineExecutionGateBuilder.Build(null, "QUARANTINE");
        Assert(!missingPreviewGate.CanExecute, "Execution gate should stay closed before a Quarantine Preview exists.");
        Assert(missingPreviewGate.Blockers.Any(blocker => blocker.Contains("Create a Quarantine Preview", StringComparison.OrdinalIgnoreCase)), "Missing preview gate should explain the preview dependency.");

        var blankGate = QuarantineExecutionGateBuilder.Build(confirmation, "");
        Assert(!blankGate.CanExecute, "Blank confirmation text should not open execution.");
        Assert(!blankGate.IsConfirmationTextMatched, "Blank confirmation text should not match.");
        Assert(blankGate.Blockers.Any(blocker => blocker.Contains("Type QUARANTINE", StringComparison.OrdinalIgnoreCase)), "Gate should require the exact confirmation phrase.");
        Assert(blankGate.Blockers.Any(blocker => blocker.Contains("not implemented", StringComparison.OrdinalIgnoreCase)), "Gate should keep execution blocked while no executor exists.");

        var matchedGate = QuarantineExecutionGateBuilder.Build(confirmation, " QUARANTINE ");
        Assert(matchedGate.IsConfirmationTextMatched, "Gate should trim and match the exact confirmation phrase.");
        Assert(!matchedGate.CanExecute, "Matched confirmation should still not execute while execution is unimplemented.");
        Assert(!matchedGate.Blockers.Any(blocker => blocker.Contains("Type QUARANTINE", StringComparison.OrdinalIgnoreCase)), "Matched confirmation should clear the phrase blocker.");
        Assert(matchedGate.Blockers.Any(blocker => blocker.Contains("not implemented", StringComparison.OrdinalIgnoreCase)), "Matched confirmation should keep the implementation blocker.");
        Assert(matchedGate.ReviewNotes.Any(note => note.Contains("No files were modified", StringComparison.OrdinalIgnoreCase)), "Gate notes should preserve the read-only boundary.");

        var blockedConfirmation = confirmation with
        {
            Blockers = ["1 blocked preview row must be removed before confirmation."]
        };
        var blockedGate = QuarantineExecutionGateBuilder.Build(blockedConfirmation, "QUARANTINE");
        Assert(!blockedGate.CanExecute, "Confirmation data blockers should keep execution closed.");
        Assert(blockedGate.Blockers.Any(blocker => blocker.Contains("blocked preview row", StringComparison.OrdinalIgnoreCase)), "Gate should carry confirmation data blockers forward.");
        Assert(!Directory.Exists(quarantineRoot), "Building execution gates should not create the quarantine root folder.");
    }

    public void QuarantineActionDraftBuildsActionScopedLayoutWithoutWritingFiles()
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
            new DateTimeOffset(2026, 5, 29, 1, 2, 3, TimeSpan.Zero),
            "manifest-draft-action");
        var confirmation = QuarantineConfirmationDraftBuilder.Build(
            preview,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 2, 3, 4, TimeSpan.Zero),
            "confirmation-draft-action");

        var draftedAtUtc = new DateTimeOffset(2026, 5, 29, 3, 4, 5, TimeSpan.Zero);
        var actionDraft = QuarantineActionDraftBuilder.Build(
            preview,
            manifestDraft,
            confirmation,
            draftedAtUtc,
            "quarantine-action-20260529_030405");

        var expectedActionRoot = Path.GetFullPath(Path.Combine(quarantineRoot, "actions", "quarantine-action-20260529_030405"));
        var expectedItemsRoot = Path.Combine(expectedActionRoot, "items");
        var expectedManifestPath = Path.Combine(expectedActionRoot, "restore-manifest.json");
        var expectedActionPath = Path.Combine(expectedItemsRoot, "Downloads", "old-installer.msi");
        var expectedPreviewPath = Path.Combine(quarantineRoot, "preview", "Downloads", "old-installer.msi");

        Assert(actionDraft.ActionId == "quarantine-action-20260529_030405", "Action draft should preserve the normalized action id.");
        Assert(actionDraft.DraftedAtUtc == draftedAtUtc, "Action draft should preserve the UTC timestamp.");
        Assert(actionDraft.ActionRootPath == expectedActionRoot, "Action draft should use an action-scoped root.");
        Assert(actionDraft.ItemsRootPath == expectedItemsRoot, "Action draft should use an action-scoped items root.");
        Assert(actionDraft.RestoreManifestPath == expectedManifestPath, "Action draft should place the future restore manifest inside the action root.");
        Assert(actionDraft.RestoreManifestDraftId == "manifest-draft-action", "Action draft should reference the Restore Manifest Draft it was derived from.");
        Assert(actionDraft.EntryCount == 1, "Action draft should include the manifest draft rows.");
        Assert(actionDraft.TotalBytes == installer.Entry.SizeBytes, "Action draft total bytes should match included preview bytes.");

        var entry = actionDraft.Entries.Single();
        Assert(entry.RelativePath == @"Downloads\old-installer.msi", "Action entry should preserve the cleanup-scope-relative path.");
        Assert(entry.PreviewQuarantinePath == Path.GetFullPath(expectedPreviewPath), "Action entry should retain the preview destination for comparison.");
        Assert(entry.ActionQuarantinePath == Path.GetFullPath(expectedActionPath), "Action entry should map to the action-scoped destination.");
        Assert(!Directory.Exists(quarantineRoot), "Building a Quarantine Action Draft should not create the quarantine root folder.");

        var invalidActionIdFailed = false;
        try
        {
            QuarantineActionDraftBuilder.Build(
                preview,
                manifestDraft,
                confirmation,
                draftedAtUtc,
                @"..\escape");
        }
        catch (ArgumentException)
        {
            invalidActionIdFailed = true;
        }

        Assert(invalidActionIdFailed, "Action draft should reject path-like action ids.");

        var blockedConfirmation = confirmation with
        {
            Blockers = ["1 blocked preview row must be removed before confirmation."]
        };
        var blockedConfirmationFailed = false;
        try
        {
            QuarantineActionDraftBuilder.Build(
                preview,
                manifestDraft,
                blockedConfirmation,
                draftedAtUtc,
                "blocked-action");
        }
        catch (ArgumentException)
        {
            blockedConfirmationFailed = true;
        }

        Assert(blockedConfirmationFailed, "Action draft should require a confirmation draft with no data blockers.");

        var mismatchedConfirmation = confirmation with
        {
            RestoreManifestDraftId = "other-manifest-draft"
        };
        var mismatchedConfirmationFailed = false;
        try
        {
            QuarantineActionDraftBuilder.Build(
                preview,
                manifestDraft,
                mismatchedConfirmation,
                draftedAtUtc,
                "mismatched-action");
        }
        catch (ArgumentException)
        {
            mismatchedConfirmationFailed = true;
        }

        Assert(mismatchedConfirmationFailed, "Action draft should require matching preview, manifest draft, and confirmation draft metadata.");
    }

    public void RestoreManifestBuildsWriteAheadActionRecordFromActionDraft()
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
            new DateTimeOffset(2026, 5, 29, 1, 2, 3, TimeSpan.Zero),
            "manifest-draft-write-ahead");
        var confirmation = QuarantineConfirmationDraftBuilder.Build(
            preview,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 2, 3, 4, TimeSpan.Zero),
            "confirmation-draft-write-ahead");
        var actionDraft = QuarantineActionDraftBuilder.Build(
            preview,
            manifestDraft,
            confirmation,
            new DateTimeOffset(2026, 5, 29, 3, 4, 5, TimeSpan.Zero),
            "quarantine-action-write-ahead");

        var createdAtUtc = new DateTimeOffset(2026, 5, 29, 4, 5, 6, TimeSpan.Zero);
        var manifest = RestoreManifestBuilder.BuildPlanned(
            actionDraft,
            manifestDraft,
            createdAtUtc,
            "restore-manifest-write-ahead");
        var json = RestoreManifestJsonSerializer.Serialize(manifest);

        Assert(manifest.SchemaVersion == RestoreManifest.CurrentSchemaVersion, "Restore Manifest should use the current schema version.");
        Assert(manifest.ManifestId == "restore-manifest-write-ahead", "Restore Manifest should preserve the provided manifest id.");
        Assert(manifest.RestoreManifestDraftId == manifestDraft.DraftId, "Restore Manifest should reference the draft it was built from.");
        Assert(manifest.ActionId == actionDraft.ActionId, "Restore Manifest should reference the Quarantine Action Draft action id.");
        Assert(manifest.CreatedAtUtc == createdAtUtc, "Restore Manifest should preserve the created timestamp as UTC.");
        Assert(manifest.UpdatedAtUtc == createdAtUtc, "Planned Restore Manifest should start with matching created and updated timestamps.");
        Assert(manifest.ManifestPath == actionDraft.RestoreManifestPath, "Restore Manifest path should match the action-scoped manifest path.");
        Assert(manifest.ActionStatus == RestoreManifestActionStatus.Planned, "Write-ahead Restore Manifest should start as planned.");
        Assert(manifest.IsExecutedManifest, "Write-ahead Restore Manifest should be an execution record, not a draft.");
        Assert(manifest.EntryCount == 1, "Restore Manifest should include the action entries.");
        Assert(manifest.TotalBytes == installer.Entry.SizeBytes, "Restore Manifest total bytes should match the included entry.");
        Assert(manifest.WriteOrderNotes.Any(note => note.Contains("before the first", StringComparison.OrdinalIgnoreCase)), "Restore Manifest should document write-before-move ordering.");

        var entry = manifest.Entries.Single();
        var actionEntry = actionDraft.Entries.Single();
        Assert(entry.OriginalPath == actionEntry.OriginalPath, "Manifest entry should preserve the original path.");
        Assert(entry.RelativePath == actionEntry.RelativePath, "Manifest entry should preserve the cleanup-scope-relative path.");
        Assert(entry.QuarantinePath == actionEntry.ActionQuarantinePath, "Manifest entry should use the action-scoped quarantine path, not the preview path.");
        Assert(entry.QuarantinePath != actionEntry.PreviewQuarantinePath, "Executed manifest entry should not reuse the preview quarantine path.");
        Assert(entry.Status == RestoreManifestEntryStatus.Planned, "Manifest entry should start as planned before any move.");
        Assert(entry.ErrorMessage is null, "Planned manifest entries should not have errors.");
        Assert(json.Contains("\"isExecutedManifest\": true", StringComparison.Ordinal), "JSON should identify the planned action record as an executed manifest record.");
        Assert(json.Contains("\"actionStatus\": \"Planned\"", StringComparison.Ordinal), "JSON should include the planned action status.");
        Assert(json.Contains("\"status\": \"Planned\"", StringComparison.Ordinal), "JSON should include planned entry status.");
        Assert(!Directory.Exists(quarantineRoot), "Building and serializing a write-ahead Restore Manifest should not create the quarantine root folder.");
    }

    public void RestoreManifestTracksPartialFailureStatusForFutureExecution()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-one.msi", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"Downloads\old-two.msi", 1024 * 1024 * 2, DateTimeOffset.UtcNow.AddDays(-130));
        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);
        var firstInstaller = SingleReviewEntry(review.Entries, @"Downloads\old-one.msi");
        var secondInstaller = SingleReviewEntry(review.Entries, @"Downloads\old-two.msi");
        var quarantineRoot = Path.Combine(fixture.RootPath, "quarantine-root");
        var preview = QuarantinePreviewBuilder.Build([firstInstaller, secondInstaller], fixture.RootPath, quarantineRoot);
        var manifestDraft = RestoreManifestDraftBuilder.Build(
            preview,
            new DateTimeOffset(2026, 5, 29, 1, 2, 3, TimeSpan.Zero),
            "manifest-draft-partial");
        var confirmation = QuarantineConfirmationDraftBuilder.Build(
            preview,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 2, 3, 4, TimeSpan.Zero),
            "confirmation-draft-partial");
        var actionDraft = QuarantineActionDraftBuilder.Build(
            preview,
            manifestDraft,
            confirmation,
            new DateTimeOffset(2026, 5, 29, 3, 4, 5, TimeSpan.Zero),
            "quarantine-action-partial");
        var manifest = RestoreManifestBuilder.BuildPlanned(
            actionDraft,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 4, 5, 6, TimeSpan.Zero),
            "restore-manifest-partial");

        var firstPath = firstInstaller.Entry.FullPath;
        var secondPath = secondInstaller.Entry.FullPath;
        var moving = RestoreManifestBuilder.WithEntryStatus(
            manifest,
            firstPath,
            RestoreManifestEntryStatus.Moving,
            new DateTimeOffset(2026, 5, 29, 4, 6, 0, TimeSpan.Zero));
        var failedBeforeAnyMove = RestoreManifestBuilder.WithEntryStatus(
            manifest,
            firstPath,
            RestoreManifestEntryStatus.Failed,
            new DateTimeOffset(2026, 5, 29, 4, 6, 30, TimeSpan.Zero),
            "Source missing.");
        var moved = RestoreManifestBuilder.WithEntryStatus(
            moving,
            firstPath,
            RestoreManifestEntryStatus.Moved,
            new DateTimeOffset(2026, 5, 29, 4, 7, 0, TimeSpan.Zero));
        var partial = RestoreManifestBuilder.WithEntryStatus(
            moved,
            secondPath,
            RestoreManifestEntryStatus.Failed,
            new DateTimeOffset(2026, 5, 29, 4, 8, 0, TimeSpan.Zero),
            "Destination already exists.");

        Assert(moving.ActionStatus == RestoreManifestActionStatus.Moving, "A moving entry should put the manifest action in Moving status.");
        Assert(moving.MovingCount == 1, "Moving manifest should count moving entries.");
        Assert(failedBeforeAnyMove.ActionStatus == RestoreManifestActionStatus.Failed, "A failure before any completed move should mark the action failed.");
        Assert(moved.ActionStatus == RestoreManifestActionStatus.Moving, "A partially moved action without failures should remain Moving until all entries finish.");
        Assert(moved.MovedCount == 1, "Moved manifest should count moved entries.");
        Assert(partial.ActionStatus == RestoreManifestActionStatus.PartialFailure, "A mix of moved and failed entries should become partial failure.");
        Assert(partial.MovedCount == 1, "Partial failure should keep the moved count for undo.");
        Assert(partial.FailedCount == 1, "Partial failure should keep the failed count for recovery review.");
        Assert(partial.RequiresRecoveryReview, "Partial failure should require recovery review.");
        Assert(partial.Entries.Single(entry => entry.OriginalPath == secondPath).ErrorMessage == "Destination already exists.", "Failed entries should preserve failure evidence.");

        var completed = actionDraft.Entries.Aggregate(
            manifest,
            (current, entry) => RestoreManifestBuilder.WithEntryStatus(
                current,
                entry.OriginalPath,
                RestoreManifestEntryStatus.Moved,
                new DateTimeOffset(2026, 5, 29, 4, 9, 0, TimeSpan.Zero)));
        Assert(completed.ActionStatus == RestoreManifestActionStatus.Completed, "All moved entries should complete the action.");
        Assert(!completed.RequiresRecoveryReview, "Completed actions should not require recovery review.");
        Assert(!Directory.Exists(quarantineRoot), "Updating in-memory Restore Manifest statuses should not create the quarantine root folder.");
    }

    public void RestoreManifestFileStoreWritesAndReplacesManifestWithoutMovingSources()
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
            new DateTimeOffset(2026, 5, 29, 1, 2, 3, TimeSpan.Zero),
            "manifest-draft-file-store");
        var confirmation = QuarantineConfirmationDraftBuilder.Build(
            preview,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 2, 3, 4, TimeSpan.Zero),
            "confirmation-draft-file-store");
        var actionDraft = QuarantineActionDraftBuilder.Build(
            preview,
            manifestDraft,
            confirmation,
            new DateTimeOffset(2026, 5, 29, 3, 4, 5, TimeSpan.Zero),
            "quarantine-action-file-store");
        var manifest = RestoreManifestBuilder.BuildPlanned(
            actionDraft,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 4, 5, 6, TimeSpan.Zero),
            "restore-manifest-file-store");

        var firstWrite = RestoreManifestFileStore.Write(manifest);
        var firstJson = File.ReadAllText(manifest.ManifestPath);
        var movedManifest = RestoreManifestBuilder.WithEntryStatus(
            manifest,
            installer.Entry.FullPath,
            RestoreManifestEntryStatus.Moved,
            new DateTimeOffset(2026, 5, 29, 4, 6, 0, TimeSpan.Zero));
        var secondWrite = RestoreManifestFileStore.Write(movedManifest);
        var secondJson = File.ReadAllText(manifest.ManifestPath);
        var tempFiles = Directory.EnumerateFiles(Path.GetDirectoryName(manifest.ManifestPath)!, "*.tmp").ToArray();

        Assert(firstWrite.ManifestPath == manifest.ManifestPath, "File store should report the action-scoped manifest path.");
        Assert(firstWrite.BytesWritten > 0, "File store should report written JSON bytes.");
        Assert(secondWrite.ManifestPath == manifest.ManifestPath, "Replacement write should keep the same manifest path.");
        Assert(File.Exists(manifest.ManifestPath), "File store should write restore-manifest.json.");
        Assert(firstJson.Contains("\"actionStatus\": \"Planned\"", StringComparison.Ordinal), "First write should contain planned action status.");
        Assert(secondJson.Contains("\"actionStatus\": \"Completed\"", StringComparison.Ordinal), "Replacement write should contain updated action status.");
        Assert(secondJson.Contains("\"status\": \"Moved\"", StringComparison.Ordinal), "Replacement write should contain updated entry status.");
        Assert(tempFiles.Length == 0, "Successful manifest writes should not leave temporary files.");
        Assert(File.Exists(installer.Entry.FullPath), "Manifest file writes should not move or delete source files.");
        Assert(!Directory.Exists(actionDraft.ItemsRootPath), "Manifest file writes should not create the action items folder.");
    }

    public void RestoreManifestFileStoreRejectsPathsOutsideActionRoot()
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
            new DateTimeOffset(2026, 5, 29, 1, 2, 3, TimeSpan.Zero),
            "manifest-draft-invalid-file-store");
        var confirmation = QuarantineConfirmationDraftBuilder.Build(
            preview,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 2, 3, 4, TimeSpan.Zero),
            "confirmation-draft-invalid-file-store");
        var actionDraft = QuarantineActionDraftBuilder.Build(
            preview,
            manifestDraft,
            confirmation,
            new DateTimeOffset(2026, 5, 29, 3, 4, 5, TimeSpan.Zero),
            "quarantine-action-invalid-file-store");
        var manifest = RestoreManifestBuilder.BuildPlanned(
            actionDraft,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 4, 5, 6, TimeSpan.Zero),
            "restore-manifest-invalid-file-store");

        var outsidePath = Path.Combine(fixture.RootPath, "outside-restore-manifest.json");
        var outsideFailed = false;
        try
        {
            RestoreManifestFileStore.Write(manifest with
            {
                ManifestPath = outsidePath
            });
        }
        catch (ArgumentException)
        {
            outsideFailed = true;
        }

        var wrongFileName = Path.Combine(actionDraft.ActionRootPath, "not-the-manifest.json");
        var wrongFileNameFailed = false;
        try
        {
            RestoreManifestFileStore.Write(manifest with
            {
                ManifestPath = wrongFileName
            });
        }
        catch (ArgumentException)
        {
            wrongFileNameFailed = true;
        }

        Assert(outsideFailed, "File store should reject manifest paths outside the action root.");
        Assert(wrongFileNameFailed, "File store should reject unexpected manifest filenames.");
        Assert(!File.Exists(outsidePath), "Rejected outside manifest path should not be written.");
        Assert(!File.Exists(wrongFileName), "Rejected manifest filename should not be written.");
        Assert(!Directory.Exists(quarantineRoot), "Rejected manifest writes should not create quarantine folders.");
        Assert(File.Exists(installer.Entry.FullPath), "Rejected manifest writes should not move or delete source files.");
    }

    public void QuarantineExecutorMovesFixtureFilesWithWriteAheadManifest()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-one.msi", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"Downloads\old-two.msi", 1024 * 1024 * 2, DateTimeOffset.UtcNow.AddDays(-130));
        var plan = BuildPlannedRestoreManifest(
            fixture,
            [@"Downloads\old-one.msi", @"Downloads\old-two.msi"],
            "executor-success");
        var writes = new List<RestoreManifest>();

        var result = QuarantineExecutor.Execute(plan.Manifest, manifest =>
        {
            writes.Add(manifest);
            return RestoreManifestFileStore.Write(manifest);
        });

        var firstEntry = plan.Manifest.Entries.Single(entry => entry.RelativePath == @"Downloads\old-one.msi");
        var secondEntry = plan.Manifest.Entries.Single(entry => entry.RelativePath == @"Downloads\old-two.msi");
        var manifestJson = File.ReadAllText(plan.Manifest.ManifestPath);

        Assert(result.Succeeded, "Quarantine Executor should complete when all fixture moves succeed.");
        Assert(result.RestoreManifest.ActionStatus == RestoreManifestActionStatus.Completed, "Successful execution should complete the Restore Manifest action.");
        Assert(result.MovedCount == 2, "Successful execution should report moved entries.");
        Assert(result.FailedCount == 0, "Successful execution should have no failed entries.");
        Assert(writes.First().ActionStatus == RestoreManifestActionStatus.Planned, "First manifest write should be planned before any move.");
        Assert(
            writes.Any(write => write.Entries.Any(entry => entry.Status == RestoreManifestEntryStatus.Moving)),
            "Executor should write Moving status before move attempts.");
        Assert(writes.Last().ActionStatus == RestoreManifestActionStatus.Completed, "Final manifest write should be completed.");
        Assert(!File.Exists(firstEntry.OriginalPath), "Moved source file should leave its original path.");
        Assert(!File.Exists(secondEntry.OriginalPath), "Moved source file should leave its original path.");
        Assert(File.Exists(firstEntry.QuarantinePath), "Moved file should exist at its action-scoped quarantine path.");
        Assert(File.Exists(secondEntry.QuarantinePath), "Moved file should exist at its action-scoped quarantine path.");
        Assert(manifestJson.Contains("\"actionStatus\": \"Completed\"", StringComparison.Ordinal), "Persisted manifest should record completed action status.");
        Assert(manifestJson.Contains("\"status\": \"Moved\"", StringComparison.Ordinal), "Persisted manifest should record moved entry status.");
    }

    public void QuarantineExecutorRecordsPartialFailureWithoutOverwritingDestination()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-one.msi", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"Downloads\old-two.msi", 1024 * 1024 * 2, DateTimeOffset.UtcNow.AddDays(-130));
        var plan = BuildPlannedRestoreManifest(
            fixture,
            [@"Downloads\old-one.msi", @"Downloads\old-two.msi"],
            "executor-partial");
        var firstEntry = plan.Manifest.Entries.Single(entry => entry.RelativePath == @"Downloads\old-one.msi");
        var secondEntry = plan.Manifest.Entries.Single(entry => entry.RelativePath == @"Downloads\old-two.msi");
        var existingDestinationParent = Path.GetDirectoryName(secondEntry.QuarantinePath)!;
        Directory.CreateDirectory(existingDestinationParent);
        File.WriteAllText(secondEntry.QuarantinePath, "existing destination");

        var result = QuarantineExecutor.Execute(plan.Manifest);
        var manifestJson = File.ReadAllText(plan.Manifest.ManifestPath);

        Assert(!result.Succeeded, "Destination collision should prevent full success.");
        Assert(result.RestoreManifest.ActionStatus == RestoreManifestActionStatus.PartialFailure, "One moved and one failed entry should be partial failure.");
        Assert(result.MovedCount == 1, "Partial failure should report moved entries.");
        Assert(result.FailedCount == 1, "Partial failure should report failed entries.");
        Assert(result.RequiresRecoveryReview, "Partial failure should require recovery review.");
        Assert(!File.Exists(firstEntry.OriginalPath), "Successful entry should move away from source.");
        Assert(File.Exists(firstEntry.QuarantinePath), "Successful entry should move to quarantine.");
        Assert(File.Exists(secondEntry.OriginalPath), "Failed destination-collision source should remain in place.");
        Assert(File.ReadAllText(secondEntry.QuarantinePath) == "existing destination", "Executor must not overwrite existing destination files.");
        Assert(
            result.Entries.Single(entry => entry.OriginalPath == secondEntry.OriginalPath).ErrorMessage?.Contains("Destination already exists", StringComparison.OrdinalIgnoreCase) == true,
            "Destination collision should be recorded as entry error evidence.");
        Assert(manifestJson.Contains("\"actionStatus\": \"PartialFailure\"", StringComparison.Ordinal), "Persisted manifest should record partial failure status.");
        Assert(manifestJson.Contains("Destination already exists", StringComparison.Ordinal), "Persisted manifest should record destination collision evidence.");
    }

    public void QuarantineExecutorFailsMissingSourceWithoutCreatingDestination()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-installer.msi", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-120));
        var plan = BuildPlannedRestoreManifest(fixture, [@"Downloads\old-installer.msi"], "executor-missing-source");
        var entry = plan.Manifest.Entries.Single();
        File.Delete(entry.OriginalPath);

        var result = QuarantineExecutor.Execute(plan.Manifest);
        var manifestJson = File.ReadAllText(plan.Manifest.ManifestPath);

        Assert(!result.Succeeded, "Missing source should prevent execution success.");
        Assert(result.RestoreManifest.ActionStatus == RestoreManifestActionStatus.Failed, "Missing source before any move should fail the action.");
        Assert(result.FailedCount == 1, "Missing source should produce a failed entry.");
        Assert(!File.Exists(entry.QuarantinePath), "Missing source should not create a destination file.");
        Assert(!Directory.Exists(plan.ActionDraft.ItemsRootPath), "Missing source should not create the action items folder.");
        Assert(
            result.Entries.Single().ErrorMessage?.Contains("Source no longer exists", StringComparison.OrdinalIgnoreCase) == true,
            "Missing source should be recorded as entry error evidence.");
        Assert(manifestJson.Contains("\"actionStatus\": \"Failed\"", StringComparison.Ordinal), "Persisted manifest should record failed action status.");
        Assert(manifestJson.Contains("Source no longer exists", StringComparison.Ordinal), "Persisted manifest should record missing source evidence.");
    }

    public void QuarantineExecutorStopsBeforeMovesWhenInitialManifestWriteFails()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-installer.msi", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-120));
        var plan = BuildPlannedRestoreManifest(fixture, [@"Downloads\old-installer.msi"], "executor-initial-write-failure");
        var entry = plan.Manifest.Entries.Single();

        var result = QuarantineExecutor.Execute(
            plan.Manifest,
            _ => throw new IOException("simulated manifest write failure"));

        Assert(!result.Succeeded, "Initial manifest write failure should prevent execution success.");
        Assert(result.RestoreManifest.ActionStatus == RestoreManifestActionStatus.Failed, "Initial manifest write failure should fail the action before moving.");
        Assert(result.HasBlockers, "Initial manifest write failure should be returned as a blocker.");
        Assert(result.MovedCount == 0, "Initial manifest write failure must not move entries.");
        Assert(File.Exists(entry.OriginalPath), "Source should remain when planned manifest cannot be written.");
        Assert(!File.Exists(entry.QuarantinePath), "Destination should not be created when planned manifest cannot be written.");
        Assert(!Directory.Exists(plan.ActionDraft.ActionRootPath), "Initial manifest write failure should not create action folders.");
    }

    public void QuarantineExecutorStopsAfterPostMoveManifestWriteFails()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-installer.msi", 1024 * 1024, DateTimeOffset.UtcNow.AddDays(-120));
        var plan = BuildPlannedRestoreManifest(fixture, [@"Downloads\old-installer.msi"], "executor-post-move-write-failure");
        var entry = plan.Manifest.Entries.Single();
        var writeCount = 0;

        var result = QuarantineExecutor.Execute(
            plan.Manifest,
            manifest =>
            {
                writeCount++;
                if (writeCount == 3)
                {
                    throw new IOException("simulated post-move manifest write failure");
                }

                return RestoreManifestFileStore.Write(manifest);
            });
        var persistedManifestJson = File.ReadAllText(plan.Manifest.ManifestPath);

        Assert(!result.Succeeded, "Post-move manifest write failure should prevent clean success.");
        Assert(result.HasBlockers, "Post-move manifest write failure should be returned as a blocker.");
        Assert(result.MovedCount == 1, "The source can be moved before the post-move write failure is observed.");
        Assert(File.Exists(entry.QuarantinePath), "Moved file should remain in quarantine after post-move manifest write failure.");
        Assert(!File.Exists(entry.OriginalPath), "Moved file should no longer be at the source path.");
        Assert(
            persistedManifestJson.Contains("\"status\": \"Moving\"", StringComparison.Ordinal),
            "Persisted manifest should retain the last durable recovery evidence when post-move update fails.");
        Assert(result.RequiresRecoveryReview, "Post-move manifest write failure should require recovery review.");
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
        var scopeRoot = new StorageEntry(
            @"C:\Users\moxhe",
            "moxhe",
            IsDirectory: true,
            SizeBytes: 1024,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.CleanupScopeRoot, BloatCategory.ProtectedLocation],
            ImportanceRating: ImportanceRating.HighRisk,
            DeletionRecommendation: DeletionRecommendation.Keep,
            Evidence: "Cleanup Scope root.",
            Children: []);
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
        var shaderCache = new StorageEntry(
            @"C:\Users\moxhe\AppData\Local\NVIDIA\DXCache",
            "DXCache",
            IsDirectory: true,
            SizeBytes: 10L * 1024 * 1024 * 1024,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.ApplicationDataArea, BloatCategory.AppCache, BloatCategory.GpuShaderCache],
            ImportanceRating: ImportanceRating.LikelySafe,
            DeletionRecommendation: DeletionRecommendation.QuarantineCandidate,
            Evidence: "GPU shader cache.",
            Children: []);
        var pythonCache = new StorageEntry(
            @"C:\Users\moxhe\AppData\Local\pip\Cache",
            "Cache",
            IsDirectory: true,
            SizeBytes: 1024,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.ApplicationDataArea, BloatCategory.AppCache, BloatCategory.PythonPackageCache],
            ImportanceRating: ImportanceRating.LikelySafe,
            DeletionRecommendation: DeletionRecommendation.QuarantineCandidate,
            Evidence: "Python package cache.",
            Children: []);
        var appDataArea = new StorageEntry(
            @"C:\Users\moxhe\AppData\Roaming\SomeTool",
            "SomeTool",
            IsDirectory: true,
            SizeBytes: 1024,
            LastModifiedUtc: now,
            IsAccessible: true,
            IsReparsePoint: false,
            ErrorMessage: null,
            BloatCategories: [BloatCategory.ApplicationDataArea],
            ImportanceRating: ImportanceRating.Caution,
            DeletionRecommendation: DeletionRecommendation.Inspect,
            Evidence: "AppData area.",
            Children: []);

        var scopeRootGuidance = SelectedPathReviewGuidanceBuilder.Build(scopeRoot);
        Assert(scopeRootGuidance.ActionLabel == "Inspect children, not the scope root", "Cleanup Scope roots should direct review to child rows.");
        Assert(
            scopeRootGuidance.Notes.Any(note => note.Contains("whole scanned folder", StringComparison.OrdinalIgnoreCase)),
            "Cleanup Scope root guidance should warn against whole-scope cleanup.");

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

        var shaderGuidance = SelectedPathReviewGuidanceBuilder.Build(shaderCache);
        Assert(shaderGuidance.ActionLabel == "Review shader cache", "GPU shader cache rows should get specific shader-cache guidance.");
        Assert(
            shaderGuidance.Notes.Any(note => note.Contains("recompile delays", StringComparison.OrdinalIgnoreCase)),
            "Shader-cache guidance should mention temporary recompile delays.");
        Assert(
            shaderGuidance.Notes.Any(note => note.Contains("Review Shortlist", StringComparison.OrdinalIgnoreCase)),
            "Shader-cache guidance should still route likely-safe rows through Review Shortlist.");

        var pythonGuidance = SelectedPathReviewGuidanceBuilder.Build(pythonCache);
        Assert(pythonGuidance.ActionLabel == "Review Python cache", "Python package cache rows should get specific package-cache guidance.");
        Assert(
            pythonGuidance.Notes.Any(note => note.Contains("Codex-related", StringComparison.OrdinalIgnoreCase)),
            "Python-cache guidance should protect Codex-related paths.");
        Assert(
            pythonGuidance.Notes.Any(note => note.Contains("Review Shortlist", StringComparison.OrdinalIgnoreCase)),
            "Python-cache guidance should still route likely-safe rows through Review Shortlist.");

        var appDataGuidance = SelectedPathReviewGuidanceBuilder.Build(appDataArea);
        Assert(appDataGuidance.ActionLabel == "Inspect AppData carefully", "Generic AppData rows should remain careful-inspection guidance.");
        Assert(
            appDataGuidance.Notes.Any(note => note.Contains("credentials", StringComparison.OrdinalIgnoreCase)),
            "AppData guidance should mention sensitive active state.");
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

        var credentialEntry = binary with
        {
            Name = "vault.kdbx",
            FullPath = Path.Combine(fixture.RootPath, "Unknown", "vault.kdbx"),
            BloatCategories = [BloatCategory.CredentialData, BloatCategory.ProtectedLocation],
            Evidence = "Credential data."
        };
        var credentialPreview = SelectedFileContentPreviewBuilder.Build(credentialEntry);
        Assert(!credentialPreview.IsContentShown, "Credential data content should not be shown in the file preview.");
        Assert(credentialPreview.Label.Contains("Credential", StringComparison.OrdinalIgnoreCase), "Credential preview should explain why content is hidden.");
        Assert(credentialPreview.Message.Contains("not shown", StringComparison.OrdinalIgnoreCase), "Credential preview should avoid rendering secret content.");
        Assert(credentialPreview.Message.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Credential preview should preserve read-only wording.");
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

        var csv = StorageScanCsvExporter.Export(
            [new StorageReviewEntry(entry, Depth: 2), new StorageReviewEntry(inaccessible, Depth: 1)],
            @"C:\Users\moxhe");

        Assert(csv.Contains("\"Full path\",\"Relative path\",\"Parent path\",\"Depth\",\"Name\",\"Type\",\"Size bytes\",\"Size\",\"Contained files\",\"Contained folders\"", StringComparison.Ordinal), "CSV should include header row with relative path and contents counts.");
        Assert(csv.Contains("\"Access status\",\"Access issue\"", StringComparison.Ordinal), "CSV should include explicit access status columns.");
        Assert(csv.Contains("\"C:\\Users\\moxhe\\Downloads\\setup, old.msi\"", StringComparison.Ordinal), "CSV should quote paths with commas.");
        Assert(csv.Contains("\"Downloads\\setup, old.msi\"", StringComparison.Ordinal), "CSV should include a cleanup-scope-relative path for easier spreadsheet review.");
        Assert(csv.Contains("\"C:\\Users\\moxhe\\Downloads\",\"2\"", StringComparison.Ordinal), "CSV should include same-scope hierarchy context.");
        Assert(csv.Contains("\"2048\",\"2 KB\",\"1\",\"0\"", StringComparison.Ordinal), "CSV should include contained file/folder counts.");
        Assert(csv.Contains("\"Readable\",\"\"", StringComparison.Ordinal), "CSV should export readable access status.");
        Assert(csv.Contains("\"Access issue\",\"Access denied.\"", StringComparison.Ordinal), "CSV should export access issue status and message.");
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
            "File.WriteAllBytes(",
            "File.WriteAllText("
        };

        var blockedMatches = sourceFiles
            .SelectMany(file => File.ReadLines(file)
                .Select((line, index) => new SourceLine(file, index + 1, line))
                .Where(sourceLine => blockedTokens.Any(token =>
                    sourceLine.Text.Contains(token, StringComparison.Ordinal)
                    && !IsAllowedProductionFilesystemWrite(sourceLine, token))))
            .ToArray();

        Assert(blockedMatches.Length == 0, "Production code should not contain cleanup execution filesystem calls outside the allowlist: " + FormatSourceLines(blockedMatches));

        var writeTextMatches = sourceFiles
            .SelectMany(file => File.ReadLines(file)
                .Select((line, index) => new SourceLine(file, index + 1, line))
                .Where(sourceLine => sourceLine.Text.Contains("File.WriteAllText(", StringComparison.Ordinal)))
            .ToArray();
        var reportWriteMatches = writeTextMatches.Where(match => IsAllowedReportExportWrite(match, "File.WriteAllText(")).ToArray();
        var manifestWriteMatches = writeTextMatches.Where(match => IsAllowedRestoreManifestFileStoreWrite(match, "File.WriteAllText(")).ToArray();
        var executorWriteMatches = sourceFiles
            .SelectMany(file => File.ReadLines(file)
                .Select((line, index) => new SourceLine(file, index + 1, line))
                .Where(sourceLine => new[] { "Directory.CreateDirectory(", "Directory.Move(", "File.Move(" }
                    .Any(token => sourceLine.Text.Contains(token, StringComparison.Ordinal)
                        && IsAllowedQuarantineExecutorWrite(sourceLine, token))))
            .ToArray();

        Assert(reportWriteMatches.Length == 3, "Only the three user-selected CSV report writes should use File.WriteAllText.");
        Assert(
            reportWriteMatches.All(match => match.Text.Contains("dialog.FileName", StringComparison.Ordinal)),
            "File.WriteAllText should only write user-selected report exports.");
        Assert(manifestWriteMatches.Length == 1, "Only RestoreManifestFileStore should write Restore Manifest JSON.");
        Assert(executorWriteMatches.Length == 3, "Only QuarantineExecutor should create destination parents and move files or folders.");
        Assert(writeTextMatches.Length == reportWriteMatches.Length + manifestWriteMatches.Length, "Every File.WriteAllText production use should be explicitly allowlisted.");
    }

    private static bool IsAllowedProductionFilesystemWrite(SourceLine sourceLine, string token)
    {
        return IsAllowedReportExportWrite(sourceLine, token)
            || IsAllowedRestoreManifestFileStoreWrite(sourceLine, token)
            || IsAllowedQuarantineExecutorWrite(sourceLine, token);
    }

    private static bool IsAllowedReportExportWrite(SourceLine sourceLine, string token)
    {
        return token == "File.WriteAllText("
            && sourceLine.FilePath.EndsWith(@"src\WindowsFileCleaner.App\MainWindow.xaml.cs", StringComparison.OrdinalIgnoreCase)
            && sourceLine.Text.Contains("dialog.FileName", StringComparison.Ordinal);
    }

    private static bool IsAllowedRestoreManifestFileStoreWrite(SourceLine sourceLine, string token)
    {
        var allowedTokens = new[]
        {
            "Directory.CreateDirectory(",
            "File.Delete(",
            "File.Move(",
            "File.Replace(",
            "File.WriteAllText("
        };

        return sourceLine.FilePath.EndsWith(@"src\WindowsFileCleaner.Core\RestoreManifestFileStore.cs", StringComparison.OrdinalIgnoreCase)
            && allowedTokens.Contains(token);
    }

    private static bool IsAllowedQuarantineExecutorWrite(SourceLine sourceLine, string token)
    {
        var allowedTokens = new[]
        {
            "Directory.CreateDirectory(",
            "Directory.Move(",
            "File.Move("
        };

        return sourceLine.FilePath.EndsWith(@"src\WindowsFileCleaner.Core\QuarantineExecutor.cs", StringComparison.OrdinalIgnoreCase)
            && allowedTokens.Contains(token);
    }

    private static PlannedQuarantineExecution BuildPlannedRestoreManifest(
        TestFixture fixture,
        IReadOnlyList<string> relativePaths,
        string idSuffix)
    {
        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var review = StorageScanReviewBuilder.Build(result);
        var selectedRows = relativePaths
            .Select(relativePath => SingleReviewEntry(review.Entries, relativePath))
            .ToArray();
        var quarantineRoot = Path.Combine(fixture.RootPath, "quarantine-root");
        var preview = QuarantinePreviewBuilder.Build(selectedRows, fixture.RootPath, quarantineRoot);
        var manifestDraft = RestoreManifestDraftBuilder.Build(
            preview,
            new DateTimeOffset(2026, 5, 29, 1, 2, 3, TimeSpan.Zero),
            $"manifest-draft-{idSuffix}");
        var confirmation = QuarantineConfirmationDraftBuilder.Build(
            preview,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 2, 3, 4, TimeSpan.Zero),
            $"confirmation-draft-{idSuffix}");
        var actionDraft = QuarantineActionDraftBuilder.Build(
            preview,
            manifestDraft,
            confirmation,
            new DateTimeOffset(2026, 5, 29, 3, 4, 5, TimeSpan.Zero),
            $"quarantine-action-{idSuffix}");
        var manifest = RestoreManifestBuilder.BuildPlanned(
            actionDraft,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 4, 5, 6, TimeSpan.Zero),
            $"restore-manifest-{idSuffix}");

        return new PlannedQuarantineExecution(actionDraft, manifest);
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

    private sealed record PlannedQuarantineExecution(
        QuarantineActionDraft ActionDraft,
        RestoreManifest Manifest);
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
