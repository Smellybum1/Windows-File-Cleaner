using System.IO;
using System.Windows;
using System.Windows.Threading;
using WindowsFileCleaner.App;
using WindowsFileCleaner.Core;

internal static class Program
{
    [STAThread]
    public static int Main()
    {
        try
        {
            var createdApplication = Application.Current is null;
            var application = Application.Current ?? new Application
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown
            };

            try
            {
                var tests = new MainWindowSmokeTests();
                tests.MainWindowDefaultsToCurrentUserCleanupScope();
                tests.MainWindowUsesLaunchCleanupScopeWithoutStartingScan();
                tests.MainWindowUsesWrappingReviewToolbarLayout();
                tests.MainWindowRunsFixtureStorageScanThroughWpfShell();
                tests.MainWindowShowsDisplayLimitForLargeFixtureScan();
                tests.MainWindowRunsFixtureReviewInteractionsThroughWpfShell();
                tests.MainWindowBlocksQuarantinePreviewForParentWithProtectedDescendant();
            }
            finally
            {
                if (createdApplication)
                {
                    application.Shutdown();
                }
            }

            Console.WriteLine("All WindowsFileCleaner.App.Tests checks passed.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return 1;
        }
    }
}

internal sealed class MainWindowSmokeTests
{
    public void MainWindowDefaultsToCurrentUserCleanupScope()
    {
        var window = new MainWindow();
        try
        {
            Assert(
                window.CurrentCleanupScopePath == StorageScanOptions.DefaultForCurrentUser().CleanupScopePath,
                "MainWindow should default to the current user's Cleanup Scope.");
            Assert(
                window.CleanupScopeSafetyNoteTextValue.Contains("Real Profile Cleanup Scope", StringComparison.OrdinalIgnoreCase),
                "MainWindow should show a real-profile Cleanup Scope safety note at startup.");
            Assert(
                window.CleanupScopeSafetyNoteTextValue.Contains("preflight", StringComparison.OrdinalIgnoreCase),
                "Default Cleanup Scope safety note should remind the user to run preflight.");
            Assert(window.CurrentStatusText == "Ready", "MainWindow should not start scanning when constructed.");
            Assert(!window.CanStartStorageScan, "MainWindow should require preflight acknowledgement before scanning the real profile.");
            Assert(window.IsRealProfilePreflightConfirmationVisible, "MainWindow should show the real-profile preflight acknowledgement.");
            Assert(!window.IsRealProfilePreflightConfirmed, "Real-profile preflight acknowledgement should start unchecked.");
            Assert(
                window.ScanGateTextValue.Contains("Confirm MVP preflight", StringComparison.OrdinalIgnoreCase),
                "Real-profile scan gate should explain why Scan is disabled.");
            window.ConfirmRealProfilePreflightForRealProfileScan();
            Assert(window.IsRealProfilePreflightConfirmed, "Real-profile preflight acknowledgement should be settable by the user.");
            Assert(window.CanStartStorageScan, "Real profile Scan should be enabled after acknowledgement.");
            Assert(
                window.ScanGateTextValue.Contains("read-only", StringComparison.OrdinalIgnoreCase),
                "Confirmed real-profile scan gate should preserve read-only wording.");
            Assert(!window.CanExportScanCsv, "MainWindow should not allow CSV export before a scan.");
            Assert(!window.CanUseEntryTypeFilter, "MainWindow should not allow type filtering before a scan.");
            Assert(!window.CanResetReviewView, "MainWindow should not allow review view reset before a scan.");
            Assert(window.SearchHelpToolTipValue.Contains("path:", StringComparison.OrdinalIgnoreCase), "Search tooltip should show field-prefix examples.");
            Assert(window.SearchHelpToolTipValue.Contains("category:", StringComparison.OrdinalIgnoreCase), "Search tooltip should include category-prefix guidance.");
            Assert(window.SearchHelpToolTipValue.Contains("access:readable", StringComparison.OrdinalIgnoreCase), "Search tooltip should include readable access-prefix guidance.");
            Assert(window.SearchHelpToolTipValue.Contains("issue:denied", StringComparison.OrdinalIgnoreCase), "Search tooltip should include access issue message-prefix guidance.");
        }
        finally
        {
            window.Close();
        }
    }

    public void MainWindowUsesLaunchCleanupScopeWithoutStartingScan()
    {
        var smokeScope = Path.Combine(
            Environment.CurrentDirectory,
            ".local",
            "storage-scan-smoke-fixture");

        var window = new MainWindow(smokeScope);
        try
        {
            Assert(
                window.CurrentCleanupScopePath == smokeScope,
                "MainWindow should use the launch Cleanup Scope.");
            Assert(
                window.CleanupScopeSafetyNoteTextValue.Contains("Fixture Cleanup Scope", StringComparison.OrdinalIgnoreCase),
                "Launch fixture Cleanup Scope should show fixture safety note.");
            Assert(
                window.CleanupScopeSafetyNoteTextValue.Contains("click Scan", StringComparison.OrdinalIgnoreCase),
                "Fixture safety note should preserve the user-triggered scan boundary.");
            Assert(window.CurrentStatusText == "Ready", "Launch Cleanup Scope should not trigger a scan.");
            Assert(window.CanStartStorageScan, "Launch Cleanup Scope should still require a user-triggered scan.");
            Assert(!window.IsRealProfilePreflightConfirmationVisible, "Fixture Cleanup Scope should not show real-profile acknowledgement.");
            Assert(!window.CanExportScanCsv, "Launch Cleanup Scope should not create exportable scan data.");
        }
        finally
        {
            window.Close();
        }
    }

    public void MainWindowUsesWrappingReviewToolbarLayout()
    {
        var window = new MainWindow();
        try
        {
            Assert(
                window.ReviewToolbarsUseWrappingLayout,
                "Review controls should use wrapping toolbars so manual fixture review is not locked to one wide row.");
        }
        finally
        {
            window.Close();
        }
    }

    public void MainWindowRunsFixtureStorageScanThroughWpfShell()
    {
        using var fixture = SmokeFixture.Create();
        var window = new MainWindow(fixture.RootPath);
        try
        {
            RunDispatcherTask(() => window.RunStorageScanForCurrentScopeAsync());

            Assert(window.CurrentStatusText.Contains("Storage Scan completed", StringComparison.OrdinalIgnoreCase), "Fixture scan should complete through the WPF shell.");
            Assert(window.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Fixture scan should preserve the read-only status text.");
            Assert(window.DisplayedRowCount > 0, "Fixture scan should show rows in the WPF grid.");
            Assert(window.CanStartStorageScan, "Scan button should be re-enabled after the fixture scan.");
            Assert(window.CanExportScanCsv, "CSV export should be enabled after a completed scan.");
            Assert(window.CanUseCategoryFilter, "Category filter should be enabled after a categorized fixture scan.");
            Assert(window.CanUseEntryTypeFilter, "Type filter should be enabled after a completed scan.");
            Assert(!window.CanResetReviewView, "Reset view should be disabled while the review view is unfiltered.");
            Assert(window.CurrentEntryTypeFilterLabel.Contains("All types", StringComparison.OrdinalIgnoreCase), "Type filter should start on All types.");
            Assert(window.TotalSizeTextValue != "-", "Total size card should be populated after scan.");
            Assert(window.FolderCountTextValue != "-", "Folder count card should be populated after scan.");
            Assert(window.FileCountTextValue != "-", "File count card should be populated after scan.");
            Assert(window.AccessIssueCountTextValue == "0", "Synthetic fixture should scan without access issues.");
            Assert(window.ReviewMixTextValue.Contains("Quarantine candidates", StringComparison.OrdinalIgnoreCase), "Review Mix should summarize quarantine candidates.");
            Assert(window.SafetySummaryTextValue.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Safety Summary should state the read-only boundary.");
            Assert(window.FilterSummaryTextValue.Contains("All:", StringComparison.OrdinalIgnoreCase), "Filter summary should start on the All filter.");
            Assert(window.ReviewSizeNoteTextValue.Contains("parent and child rows can overlap", StringComparison.OrdinalIgnoreCase), "Review size note should explain recursive row overlap.");
            Assert(window.ReviewSizeNoteTextValue.Contains("not storage savings", StringComparison.OrdinalIgnoreCase), "Review size note should avoid treating row sizes as savings.");
            Assert(
                window.ContentsColumnSortMemberPath == "ContainedTotalCount",
                "Contents column should sort by numeric contained item count rather than formatted display text.");

            var rows = window.DisplayedRows;
            var rootRow = rows.Single(row =>
                row.FullPath.Equals(Path.GetFullPath(fixture.RootPath), StringComparison.OrdinalIgnoreCase));
            Assert(
                rootRow.Importance == "High risk"
                && rootRow.Recommendation == "Keep"
                && rootRow.Categories.Contains("Cleanup scope root", StringComparison.OrdinalIgnoreCase)
                && rootRow.Categories.Contains("Protected location", StringComparison.OrdinalIgnoreCase),
                "Fixture Cleanup Scope root should be shown as a protected keep row.");
            Assert(
                rows.Any(row =>
                    row.FullPath.EndsWith(@"Downloads\old-installer.msi", StringComparison.OrdinalIgnoreCase)
                    && row.Importance == "Likely safe"
                    && row.Recommendation == "Quarantine candidate"
                    && row.Categories.Contains("Installer cache", StringComparison.OrdinalIgnoreCase)),
                "Fixture installer should appear as a likely-safe quarantine candidate.");
            Assert(
                rows.Any(row =>
                    row.FullPath.EndsWith("Documents", StringComparison.OrdinalIgnoreCase)
                    && row.Importance == "High risk"
                    && row.Recommendation == "Keep"
                    && row.Categories.Contains("Protected location", StringComparison.OrdinalIgnoreCase)),
                "Fixture Documents folder should appear as a protected high-risk row.");
            Assert(
                rows.Any(row => row.Categories.Contains("Python package cache", StringComparison.OrdinalIgnoreCase)),
                "Fixture scan should surface Python package cache category evidence.");

            var downloads = rows.Single(row =>
                row.FullPath.EndsWith("Downloads", StringComparison.OrdinalIgnoreCase));
            Assert(downloads.AccessStatus == "Readable", "Fixture Downloads folder should show readable access status.");
            Assert(
                downloads.Contents.Contains("1 file", StringComparison.OrdinalIgnoreCase)
                && downloads.Contents.Contains("0 folders", StringComparison.OrdinalIgnoreCase),
                "Fixture Downloads folder should expose contents counts in the grid row.");
            Assert(downloads.ContainedTotalCount == 1, "Fixture Downloads contents sort value should total contained files and folders.");
            Assert(window.SelectDisplayedPath(downloads.FullPath), "Fixture Downloads folder should be selectable for contents context.");
            Assert(downloads.Contents.Contains("1 file", StringComparison.OrdinalIgnoreCase), "Folder row should expose contained file count.");
            Assert(
                window.DetailPathContextTextValue.Contains("Contents:", StringComparison.OrdinalIgnoreCase)
                && window.DetailPathContextTextValue.Contains("1 file", StringComparison.OrdinalIgnoreCase),
                "Selected folder detail pane should show contained file/folder counts.");
            Assert(window.DetailMetaTextValue.Contains("Access: Readable", StringComparison.OrdinalIgnoreCase), "Selected row detail pane should show access status.");

            Assert(window.SelectDisplayedPath(fixture.MarkerPath), "Fixture note file should be selectable for preview.");
            Assert(window.CanPreviewSelectedFile, "Selected file preview should be enabled for a selected file.");
            window.PreviewSelectedFileContent();
            Assert(
                window.FilePreviewTextValue.Contains("Synthetic uncategorized note.", StringComparison.OrdinalIgnoreCase),
                "Selected file preview should show bounded text content for a fixture text file.");
            Assert(
                window.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase),
                "Selected file preview status should preserve the read-only boundary.");

            window.ApplyStorageReviewSearch("pip");
            Assert(window.CanResetReviewView, "Reset view should be enabled after search narrows review rows.");
            Assert(window.CurrentSearchText == "pip", "Applying Storage Review Search should update WPF search text.");
            Assert(window.FilterSummaryTextValue.Contains("Search \"pip\"", StringComparison.OrdinalIgnoreCase), "Search should update the filter summary.");
            Assert(window.DisplayedRows.Count > 0, "Search should show matching fixture rows.");
            Assert(
                window.DisplayedRows.All(row =>
                    row.FullPath.Contains("pip", StringComparison.OrdinalIgnoreCase)
                    || row.Categories.Contains("Python package cache", StringComparison.OrdinalIgnoreCase)
                    || row.Evidence.Contains("Python", StringComparison.OrdinalIgnoreCase)),
                "Search should only show rows matching path, category, or evidence text.");
            Assert(
                window.CurrentScanReportExportRowCount == window.DisplayedRows.Count,
                "Scan Report Export row count should honor the active Storage Review Search.");
            Assert(
                window.CurrentScanReportExportPaths.All(path => path.Contains("pip", StringComparison.OrdinalIgnoreCase)),
                "Scan Report Export paths should honor the active Storage Review Search.");
            Assert(
                window.CurrentScanReportExportFileName.Contains("-search-pip.csv", StringComparison.OrdinalIgnoreCase),
                "Scan Report Export filename should include a sanitized active search segment.");

            window.ApplyStorageReviewSearch("category:Python package cache");
            Assert(window.FilterSummaryTextValue.Contains("Search \"category:Python package cache\"", StringComparison.OrdinalIgnoreCase), "Prefixed search should keep the search text visible.");
            Assert(
                window.DisplayedRows.All(row => row.Categories.Contains("Python package cache", StringComparison.OrdinalIgnoreCase)),
                "Category-prefixed search should only show matching category rows.");
            Assert(
                window.CurrentScanReportExportFileName.Contains("-search-category-python-package-cache.csv", StringComparison.OrdinalIgnoreCase),
                "Scan Report Export filename should include a sanitized prefixed search segment.");

            window.ApplyStorageReviewSearch("access:readable");
            Assert(window.FilterSummaryTextValue.Contains("Search \"access:readable\"", StringComparison.OrdinalIgnoreCase), "Access-prefixed search should update the filter summary.");
            Assert(window.DisplayedRows.Count > 0, "Readable access search should show fixture rows.");
            Assert(
                window.DisplayedRows.All(row => row.AccessStatus == "Readable"),
                "Readable access search should show only readable rows in the fixture.");
            Assert(
                window.CurrentScanReportExportFileName.Contains("-search-access-readable.csv", StringComparison.OrdinalIgnoreCase),
                "Scan Report Export filename should include a sanitized access-status search segment.");

            window.ApplyStorageReviewSearch("");
            Assert(window.CurrentSearchText == "", "Clearing Storage Review Search should clear WPF search text.");
            Assert(!window.FilterSummaryTextValue.Contains("Search \"", StringComparison.OrdinalIgnoreCase), "Cleared search should be removed from the filter summary.");
            Assert(
                !window.CurrentScanReportExportFileName.Contains("-search-", StringComparison.OrdinalIgnoreCase),
                "Scan Report Export filename should omit the search segment after search is cleared.");

            Assert(File.Exists(fixture.MarkerPath), "Fixture marker file should still exist after the read-only scan.");
        }
        finally
        {
            window.Close();
        }
    }

    public void MainWindowShowsDisplayLimitForLargeFixtureScan()
    {
        using var fixture = SmokeFixture.CreateLargeResultSet();
        var window = new MainWindow(fixture.RootPath);
        try
        {
            RunDispatcherTask(() => window.RunStorageScanForCurrentScopeAsync());

            Assert(
                window.DisplayedRowCount == 2000,
                "Large fixture scan should cap the WPF grid at the display limit.");
            Assert(
                window.CurrentStatusText.Contains("Showing rows 1-2,000", StringComparison.OrdinalIgnoreCase),
                "Large fixture status should show the active review window.");
            Assert(
                window.FilterSummaryTextValue.Contains("rows 1-2,000", StringComparison.OrdinalIgnoreCase),
                "Filter summary should show the active review window.");
            Assert(
                window.ReviewWindowTextValue.Contains("rows 1-2,000", StringComparison.OrdinalIgnoreCase),
                "Review window text should show the first matched row window.");
            Assert(!window.CanShowPreviousReviewWindow, "First review window should not allow previous rows.");
            Assert(window.CanShowNextReviewWindow, "Large fixture scan should allow moving to the next review window.");
            Assert(
                window.FilterSummaryTextValue.Contains("largest matched row", StringComparison.OrdinalIgnoreCase),
                "Filter summary should label largest-row size as matched-row triage.");

            var firstWindowPaths = window.DisplayedRows
                .Select(row => row.FullPath)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            window.ShowNextReviewWindow();
            Assert(window.DisplayedRowCount > 0, "Next review window should show remaining matched rows.");
            Assert(window.DisplayedRowCount < 2000, "Next review window should not repeat the full first display window.");
            Assert(
                window.ReviewWindowTextValue.Contains("rows 2,001-", StringComparison.OrdinalIgnoreCase),
                "Next review window text should show the second matched row window.");
            Assert(
                window.FilterSummaryTextValue.Contains("rows 2,001-", StringComparison.OrdinalIgnoreCase),
                "Filter summary should update after moving to the next review window.");
            Assert(window.CanShowPreviousReviewWindow, "Second review window should allow returning to previous rows.");
            Assert(!window.CanShowNextReviewWindow, "Large fixture with one partial second window should not allow another next page.");
            Assert(
                window.DisplayedRows.All(row => !firstWindowPaths.Contains(row.FullPath)),
                "Next review window should not repeat first-window rows.");
            Assert(
                window.CurrentStatusText.Contains("Review window changed", StringComparison.OrdinalIgnoreCase)
                && window.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase),
                "Changing review windows should preserve the read-only status boundary.");

            window.ShowPreviousReviewWindow();
            Assert(window.DisplayedRowCount == 2000, "Previous review window should return to the capped first window.");
            Assert(window.ReviewWindowTextValue.Contains("rows 1-2,000", StringComparison.OrdinalIgnoreCase), "Previous review window should restore the first row window.");

            window.ShowNextReviewWindow();
            window.SelectEntryTypeFilterThroughCombo(StorageEntryTypeFilter.Files);
            Assert(
                window.ReviewWindowTextValue.Contains("rows 1-2,000", StringComparison.OrdinalIgnoreCase),
                "Changing the type filter through the WPF combo should reset the review window to the first matched rows.");
            Assert(
                window.CurrentScanReportExportTypes.All(type => type == "File"),
                "Scan Report Export rows should honor the active file type filter.");
            window.ShowNextReviewWindow();
            window.SelectBloatCategoryFilterThroughCombo(StorageCategoryFilter.NoCategory);
            Assert(
                window.ReviewWindowTextValue.Contains("rows 1-2,000", StringComparison.OrdinalIgnoreCase),
                "Changing the category filter through the WPF combo should reset the review window to the first matched rows.");
            Assert(
                window.DisplayedRows.All(row => row.Categories == "None"),
                "No-category combo selection should show only uncategorized rows.");
            Assert(File.Exists(fixture.MarkerPath), "Large fixture marker file should still exist after the read-only scan.");
        }
        finally
        {
            window.Close();
        }
    }

    public void MainWindowRunsFixtureReviewInteractionsThroughWpfShell()
    {
        using var fixture = SmokeFixture.Create();
        var window = new MainWindow(fixture.RootPath);
        try
        {
            RunDispatcherTask(() => window.RunStorageScanForCurrentScopeAsync());

            window.ApplyEntryTypeFilter(StorageEntryTypeFilter.Files);
            Assert(window.FilterSummaryTextValue.Contains("Files", StringComparison.OrdinalIgnoreCase), "File type filter should update the filter summary.");
            Assert(window.DisplayedRows.Count > 0, "File type filter should show fixture files.");
            Assert(window.DisplayedRows.All(row => row.Type == "File"), "File type filter should only show files.");
            Assert(window.CurrentScanReportExportTypes.All(type => type == "File"), "Scan Report Export should honor the active file type filter.");
            Assert(window.CurrentScanReportExportFileName.Contains("-files", StringComparison.OrdinalIgnoreCase), "Scan Report Export filename should include active file type filter.");
            Assert(window.CanResetReviewView, "Reset view should be enabled after type filtering.");

            window.ApplyEntryTypeFilter(StorageEntryTypeFilter.Folders);
            Assert(window.FilterSummaryTextValue.Contains("Folders", StringComparison.OrdinalIgnoreCase), "Folder type filter should update the filter summary.");
            Assert(window.DisplayedRows.Count > 0, "Folder type filter should show fixture folders.");
            Assert(window.DisplayedRows.All(row => row.Type == "Folder"), "Folder type filter should only show folders.");
            Assert(window.CurrentScanReportExportTypes.All(type => type == "Folder"), "Scan Report Export should honor the active folder type filter.");

            window.ApplyEntryTypeFilter(StorageEntryTypeFilter.All);
            Assert(window.CurrentEntryTypeFilterLabel.Contains("All types", StringComparison.OrdinalIgnoreCase), "Type filter should return to All types.");

            window.ApplyStorageReviewFilter(StorageReviewFilter.QuarantineCandidates);
            Assert(window.FilterSummaryTextValue.Contains("Quarantine candidates:", StringComparison.OrdinalIgnoreCase), "Quarantine candidate filter should update the filter summary.");
            Assert(window.DisplayedRows.Count > 0, "Quarantine candidate filter should show fixture rows.");
            Assert(
                window.DisplayedRows.All(row => row.Recommendation == "Quarantine candidate"),
                "Quarantine candidate filter should only show quarantine candidates.");

            window.ApplyBloatCategoryFilter(StorageCategoryFilter.ForCategory(BloatCategory.InstallerCache));
            Assert(window.FilterSummaryTextValue.Contains("Installer cache", StringComparison.OrdinalIgnoreCase), "Category filter should update the filter summary.");
            Assert(
                window.DisplayedRows.All(row => row.Categories.Contains("Installer cache", StringComparison.OrdinalIgnoreCase)),
                "Installer cache category filter should only show installer cache rows.");

            window.ApplySafetyReviewShortcut(StorageScanSafetyShortcut.ProtectedLocations);
            Assert(window.CurrentStatusText.Contains("Review shortcut applied", StringComparison.OrdinalIgnoreCase), "Safety shortcut should report a read-only review action.");
            Assert(window.FilterSummaryTextValue.Contains("Protected location", StringComparison.OrdinalIgnoreCase), "Protected shortcut should update the filter summary.");
            Assert(
                window.DisplayedRows.Count > 0 && window.DisplayedRows.All(row => row.Categories.Contains("Protected location", StringComparison.OrdinalIgnoreCase)),
                "Protected shortcut should show protected rows.");

            window.ApplySafetyReviewShortcut(StorageScanSafetyShortcut.Uncategorized);
            Assert(window.FilterSummaryTextValue.Contains("No category", StringComparison.OrdinalIgnoreCase), "No-category shortcut should update the filter summary.");
            Assert(
                window.DisplayedRows.Count > 0 && window.DisplayedRows.All(row => row.Categories == "None"),
                "No-category shortcut should show only uncategorized rows.");

            window.ApplySafetyReviewShortcut(StorageScanSafetyShortcut.QuarantineCandidates);
            var installer = window.DisplayedRows.Single(row =>
                row.FullPath.EndsWith(@"Downloads\old-installer.msi", StringComparison.OrdinalIgnoreCase));

            Assert(window.SelectDisplayedPath(installer.FullPath), "Fixture installer should be selectable in WPF results.");
            Assert(window.SelectedRowFullPath == installer.FullPath, "Selecting a displayed row should update selected path state.");
            Assert(
                installer.ParentLocation.EndsWith(@"\Downloads", StringComparison.OrdinalIgnoreCase),
                "Displayed rows should expose parent path context for deep review rows.");
            Assert(
                window.DetailPathContextTextValue.Contains("Parent:", StringComparison.OrdinalIgnoreCase)
                && window.DetailPathContextTextValue.Contains("Downloads", StringComparison.OrdinalIgnoreCase)
                && window.DetailPathContextTextValue.Contains("Depth:", StringComparison.OrdinalIgnoreCase),
                "Selected-row detail pane should show parent path and hierarchy depth context.");
            Assert(
                window.DetailGuidanceTextValue.Contains("Shortlist after review", StringComparison.OrdinalIgnoreCase),
                "Selecting a quarantine candidate should show review guidance before shortlisting.");
            Assert(
                window.DetailGuidanceTextValue.Contains("not deletion approval", StringComparison.OrdinalIgnoreCase),
                "Selected-row review guidance should not imply deletion approval.");
            Assert(window.CanAddSelectedRowToReviewShortlist, "Selected fixture installer should be addable to Review Shortlist.");
            Assert(window.CanAddShownRowsToReviewShortlist, "Displayed quarantine candidates should be bulk-addable to Review Shortlist.");
            Assert(!window.CanRemoveShownRowsFromReviewShortlist, "Shown rows should not be removable before they are shortlisted.");
            Assert(!window.CanPreviewQuarantine, "Quarantine Preview should be disabled before a shortlist exists.");

            window.AddShownRowsToReviewShortlist();
            Assert(window.ReviewShortlistCount == 1, "Bulk shortlisting shown rows should update Review Shortlist count.");
            Assert(
                window.CurrentStatusText.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase),
                "Bulk shortlisting status should preserve the non-approval boundary.");
            Assert(!window.CanAddShownRowsToReviewShortlist, "Bulk shortlist should disable once every shown row is already shortlisted.");
            Assert(window.CanRemoveShownRowsFromReviewShortlist, "Bulk-shortlisted shown rows should be removable as the current review window.");
            Assert(window.CanRemoveSelectedRowFromReviewShortlist, "Shortlisted row should be removable.");
            Assert(window.CanPreviewQuarantine, "Quarantine Preview should be available after shortlisting a row.");
            Assert(
                window.DisplayedRows.Any(row => row.FullPath == installer.FullPath && row.Shortlist == "Yes"),
                "Shortlisted row should be marked in the WPF grid.");

            window.ApplyEntryTypeFilter(StorageEntryTypeFilter.Files);
            window.ApplyStorageReviewSearch("old-installer");
            Assert(window.CanResetReviewView, "Reset view should be enabled when multiple review lenses are active.");
            window.ResetReviewView();
            Assert(!window.CanResetReviewView, "Reset view should disable after returning to the default review lens.");
            Assert(window.CurrentSearchText == "", "Reset view should clear Storage Review Search.");
            Assert(window.CurrentEntryTypeFilterLabel.Contains("All types", StringComparison.OrdinalIgnoreCase), "Reset view should restore All types.");
            Assert(window.FilterSummaryTextValue.StartsWith("All:", StringComparison.OrdinalIgnoreCase), "Reset view should restore the All review filter.");
            Assert(window.ReviewShortlistCount == 1, "Reset view should keep Review Shortlist entries.");
            Assert(
                window.CurrentStatusText.Contains("Review Shortlist was kept", StringComparison.OrdinalIgnoreCase),
                "Reset view status should explain that shortlist entries were preserved.");
            window.ApplySafetyReviewShortcut(StorageScanSafetyShortcut.QuarantineCandidates);
            Assert(window.SelectDisplayedPath(installer.FullPath), "Shortlisted fixture installer should be selectable after resetting and restoring the candidate view.");

            window.RemoveShownRowsFromReviewShortlist();
            Assert(window.ReviewShortlistCount == 0, "Removing shown rows should update Review Shortlist count.");
            Assert(
                window.CurrentStatusText.Contains("Removed 1 shown", StringComparison.OrdinalIgnoreCase),
                "Removing shown rows should report the visible-window removal.");
            Assert(window.CanAddShownRowsToReviewShortlist, "Shown rows should be bulk-addable after visible-window removal.");
            Assert(!window.CanRemoveShownRowsFromReviewShortlist, "Shown rows should not be removable after visible-window removal.");

            window.AddShownRowsToReviewShortlist();
            Assert(window.ReviewShortlistCount == 1, "Bulk shortlisting shown rows should be repeatable after visible-window removal.");

            window.PreviewQuarantineForReviewShortlist();
            Assert(window.CurrentStatusText.Contains("Quarantine Preview created", StringComparison.OrdinalIgnoreCase), "Preview action should update status text.");
            Assert(window.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Preview status should preserve the read-only boundary.");
            Assert(window.CanExportQuarantinePreview, "Quarantine Preview export should be enabled after preview creation.");
            Assert(window.QuarantinePreviewTextValue.Contains("Included: 1", StringComparison.OrdinalIgnoreCase), "Preview pane should show one included fixture row.");
            Assert(window.QuarantinePreviewTextValue.Contains("Restore Manifest Draft", StringComparison.OrdinalIgnoreCase), "Preview pane should show Restore Manifest Draft readiness.");
            Assert(window.QuarantinePreviewTextValue.Contains("Quarantine Confirmation Draft", StringComparison.OrdinalIgnoreCase), "Preview pane should show Quarantine Confirmation Draft readiness.");
            Assert(window.QuarantinePreviewTextValue.Contains("Execution implemented: no", StringComparison.OrdinalIgnoreCase), "Preview pane should state execution is not implemented.");
            Assert(window.QuarantinePreviewTextValue.Contains("Preview rows:", StringComparison.OrdinalIgnoreCase), "Preview pane should label row-level preview details.");
            Assert(window.QuarantinePreviewTextValue.Contains("Preview row | Included", StringComparison.OrdinalIgnoreCase), "Preview pane should distinguish included row details from readiness blockers.");
            Assert(window.QuarantinePreviewTextValue.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Preview pane should preserve the read-only boundary.");
            Assert(File.Exists(installer.FullPath), "Shortlisted fixture installer should still exist after preview.");
            Assert(File.Exists(fixture.MarkerPath), "Fixture marker file should still exist after review interactions.");

            window.RemoveSelectedPathFromReviewShortlist();
            Assert(window.ReviewShortlistCount == 0, "Removing selected row should clear Review Shortlist count.");
            Assert(!window.CanPreviewQuarantine, "Quarantine Preview should be disabled after shortlist removal.");
            Assert(!window.CanExportQuarantinePreview, "Quarantine Preview export should be disabled after shortlist removal clears preview state.");
            Assert(
                window.QuarantinePreviewTextValue.Contains("Preview and draft readiness appear", StringComparison.OrdinalIgnoreCase),
                "Removing the shortlisted row should clear preview readiness text.");

            Assert(window.CanAddSelectedRowToReviewShortlist, "Removed selected row should be addable again through the selected-row action.");
            window.AddSelectedPathToReviewShortlist();
            Assert(window.ReviewShortlistCount == 1, "Selected-row shortlisting should still update Review Shortlist count.");
            window.ClearReviewShortlist();
            Assert(window.ReviewShortlistCount == 0, "Clearing after selected-row shortlisting should empty Review Shortlist.");
        }
        finally
        {
            window.Close();
        }
    }

    public void MainWindowBlocksQuarantinePreviewForParentWithProtectedDescendant()
    {
        using var fixture = SmokeFixture.CreateProtectedDescendantPreviewCase();
        var window = new MainWindow(fixture.RootPath);
        try
        {
            RunDispatcherTask(() => window.RunStorageScanForCurrentScopeAsync());

            var cacheParent = window.DisplayedRows.Single(row =>
                row.FullPath.EndsWith(".cache", StringComparison.OrdinalIgnoreCase));

            Assert(cacheParent.Recommendation == "Quarantine candidate", "Broad cache parent should otherwise look eligible in the fixture.");
            Assert(window.SelectDisplayedPath(cacheParent.FullPath), "Broad cache parent should be selectable.");
            Assert(window.CanAddSelectedRowToReviewShortlist, "Broad cache parent should be shortlistable before preview blockers are evaluated.");

            window.AddSelectedPathToReviewShortlist();
            Assert(window.ReviewShortlistCount == 1, "Selected broad cache parent should be added to Review Shortlist.");
            Assert(window.CanPreviewQuarantine, "Quarantine Preview should be available for the shortlisted cache parent.");

            window.PreviewQuarantineForReviewShortlist();

            Assert(
                window.CurrentStatusText.Contains("0 included", StringComparison.OrdinalIgnoreCase)
                && window.CurrentStatusText.Contains("1 blocked", StringComparison.OrdinalIgnoreCase)
                && window.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase),
                "Preview status should report the protected-descendant blocker without modifying files.");
            Assert(window.QuarantinePreviewTextValue.Contains("Included: 0", StringComparison.OrdinalIgnoreCase), "Preview pane should show zero included rows.");
            Assert(window.QuarantinePreviewTextValue.Contains("Blocked: 1", StringComparison.OrdinalIgnoreCase), "Preview pane should show one blocked row.");
            Assert(window.QuarantinePreviewTextValue.Contains("Confirmation readiness blockers:", StringComparison.OrdinalIgnoreCase), "Confirmation readiness should surface blockers.");
            Assert(window.QuarantinePreviewTextValue.Contains("Confirmation blocker |", StringComparison.OrdinalIgnoreCase), "Confirmation blockers should be labeled separately from preview rows.");
            Assert(window.QuarantinePreviewTextValue.Contains("blocked preview row", StringComparison.OrdinalIgnoreCase), "Confirmation readiness should call out the blocked preview row.");
            Assert(window.QuarantinePreviewTextValue.Contains("Preview rows:", StringComparison.OrdinalIgnoreCase), "Preview pane should label row-level preview details.");
            Assert(window.QuarantinePreviewTextValue.Contains("Preview row | Blocked", StringComparison.OrdinalIgnoreCase), "Blocked row details should be labeled as preview rows.");
            Assert(window.QuarantinePreviewTextValue.Contains("codex-runtimes", StringComparison.OrdinalIgnoreCase), "Preview pane should show the protected descendant evidence.");
            Assert(window.QuarantinePreviewTextValue.Contains("Select narrower reviewed child rows", StringComparison.OrdinalIgnoreCase), "Preview pane should guide the user toward narrower rows.");
            Assert(window.QuarantinePreviewTextValue.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Preview pane should preserve read-only wording.");
            Assert(File.Exists(fixture.MarkerPath), "Protected descendant fixture file should still exist after preview.");
        }
        finally
        {
            window.Close();
        }
    }

    private static void RunDispatcherTask(Func<Task> action)
    {
        Exception? exception = null;
        var dispatcher = Dispatcher.CurrentDispatcher;
        var previousContext = SynchronizationContext.Current;
        SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(dispatcher));
        var frame = new DispatcherFrame();

        dispatcher.BeginInvoke(async () =>
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                frame.Continue = false;
            }
        });

        Dispatcher.PushFrame(frame);
        SynchronizationContext.SetSynchronizationContext(previousContext);

        if (exception is not null)
        {
            throw exception;
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

internal sealed class SmokeFixture : IDisposable
{
    private SmokeFixture(string rootPath, string markerPath)
    {
        RootPath = rootPath;
        MarkerPath = markerPath;
    }

    public string RootPath { get; }

    public string MarkerPath { get; }

    public static SmokeFixture Create()
    {
        var root = Path.Combine(
            Environment.CurrentDirectory,
            "test-fixtures",
            "app",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(root);
        var fixture = new SmokeFixture(root, Path.Combine(root, "Unknown", "notes.txt"));
        var now = DateTimeOffset.UtcNow;

        fixture.WriteFile(@"Downloads\old-installer.msi", "Synthetic old installer.", now.AddDays(-120));
        fixture.WriteFile(@"AppData\Local\Temp\scratch.tmp", "Synthetic temp file.", now.AddDays(-5));
        fixture.WriteFile(@"AppData\Local\pip\Cache\http-v2\response.body", "Synthetic Python cache.", now.AddDays(-40));
        fixture.WriteFile(@"Documents\important.txt", "Synthetic protected document.", now);
        fixture.WriteFile(@".codex\config.json", "{ \"synthetic\": true }", now);
        fixture.WriteFile(@"Unknown\notes.txt", "Synthetic uncategorized note.", now);

        return fixture;
    }

    public static SmokeFixture CreateLargeResultSet()
    {
        const int bulkFileCount = 2050;
        var root = Path.Combine(
            Environment.CurrentDirectory,
            "test-fixtures",
            "app",
            "large",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(root);
        var fixture = new SmokeFixture(root, Path.Combine(root, "Unknown", "large-fixture-marker.txt"));
        var now = DateTimeOffset.UtcNow;

        fixture.WriteFile(@"Unknown\large-fixture-marker.txt", "Synthetic marker for capped display smoke test.", now);

        for (var index = 0; index < bulkFileCount; index++)
        {
            fixture.WriteFile(
                $@"Bulk\bulk-file-{index:D4}.tmp",
                "Synthetic bulk row.",
                now.AddMinutes(-index));
        }

        return fixture;
    }

    public static SmokeFixture CreateProtectedDescendantPreviewCase()
    {
        var root = Path.Combine(
            Environment.CurrentDirectory,
            "test-fixtures",
            "app",
            "protected-descendant",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(root);
        var protectedDescendantPath = Path.Combine(root, ".cache", "codex-runtimes", "python.exe");
        var fixture = new SmokeFixture(root, protectedDescendantPath);
        var now = DateTimeOffset.UtcNow;

        fixture.WriteFile(@".cache\general-cache.bin", "Synthetic cache data.", now.AddDays(-30));
        fixture.WriteFile(@".cache\codex-runtimes\python.exe", "Synthetic protected Codex runtime.", now);

        return fixture;
    }

    public void Dispose()
    {
        if (Directory.Exists(RootPath))
        {
            Directory.Delete(RootPath, recursive: true);
        }
    }

    private void WriteFile(string relativePath, string content, DateTimeOffset lastModifiedUtc)
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
}
