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
                tests.MainWindowUsesCustomCleanupScopeWithoutExecutionAvailability();
                tests.MainWindowUsesWrappingReviewToolbarLayout();
                tests.MainWindowRunsFixtureStorageScanThroughWpfShell();
                tests.MainWindowShowsDisplayLimitForLargeFixtureScan();
                tests.MainWindowRunsFixtureReviewInteractionsThroughWpfShell();
                tests.MainWindowExecutesQuarantineForFixtureScopeOnly();
                tests.MainWindowDiscoversQuarantineManifestsReadOnly();
                tests.MainWindowKeepsQuarantineExecutionUnavailableForCustomScope();
                tests.MainWindowKeepsSelectedRestoreUnavailableForCustomScope();
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
            Assert(window.CanBrowseCleanupScope, "MainWindow should allow choosing a Cleanup Scope without starting a scan.");
            Assert(window.CanEditQuarantineRoot, "MainWindow should allow editing the read-only Quarantine Preview root before scanning.");
            Assert(window.CanBrowseQuarantineRoot, "MainWindow should allow browsing for the read-only Quarantine Preview root before scanning.");
            Assert(window.CanDiscoverQuarantineManifests, "MainWindow should allow read-only Quarantine Manifest Discovery before scanning.");
            Assert(window.CanPreviewRestoreReadiness, "MainWindow should allow read-only Restore Readiness Preview before scanning.");
            Assert(
                window.PreviewRestoreReadinessButtonText == "Preview all-manifest readiness",
                "Restore Readiness Preview button should name its all-manifest scope.");
            Assert(
                window.PreviewRestoreReadinessButtonToolTipValue.Contains("Read-only all-manifest readiness", StringComparison.OrdinalIgnoreCase)
                && window.PreviewRestoreReadinessButtonToolTipValue.Contains("no files are restored", StringComparison.OrdinalIgnoreCase),
                "All-manifest readiness tooltip should explain read-only scope and no-restore behavior.");
            Assert(!window.CanSelectDiscoveredRestoreManifest, "MainWindow should not enable Restore Manifest selection before discovery.");
            Assert(!window.CanPreviewSelectedRestoreManifestReadiness, "MainWindow should not enable selected Restore Manifest review before discovery.");
            Assert(
                window.PreviewSelectedRestoreManifestReadinessButtonText == "Preview selected manifest readiness",
                "Selected Restore Manifest readiness preview button should name the selected manifest.");
            Assert(
                window.PreviewSelectedRestoreManifestReadinessButtonToolTipValue.Contains("selected Restore Manifest only", StringComparison.OrdinalIgnoreCase)
                && window.PreviewSelectedRestoreManifestReadinessButtonToolTipValue.Contains("not restore approval", StringComparison.OrdinalIgnoreCase),
                "Selected manifest readiness tooltip should explain selected-only scope and approval boundary.");
            Assert(
                window.ExportScanCsvButtonToolTipValue.Contains("CSV report", StringComparison.OrdinalIgnoreCase)
                && window.ExportScanCsvButtonToolTipValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.ExportScanCsvButtonToolTipValue.Contains("no scanned files are modified", StringComparison.OrdinalIgnoreCase),
                "Scan Report Export tooltip should explain report-only behavior.");
            Assert(
                window.ExportScanCsvButtonAutomationHelpTextValue.Contains("CSV report", StringComparison.OrdinalIgnoreCase)
                && window.ExportScanCsvButtonAutomationHelpTextValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.ExportScanCsvButtonAutomationHelpTextValue.Contains("no scanned files are modified", StringComparison.OrdinalIgnoreCase),
                "Scan Report Export automation help text should explain report-only behavior.");
            Assert(
                window.ClearSearchButtonToolTipValue.Contains("Storage Review Search", StringComparison.OrdinalIgnoreCase)
                && window.ClearSearchButtonToolTipValue.Contains("does not rescan", StringComparison.OrdinalIgnoreCase)
                && window.ClearSearchButtonToolTipValue.Contains("modify files", StringComparison.OrdinalIgnoreCase)
                && window.ClearSearchButtonToolTipValue.Contains("Review Shortlist", StringComparison.OrdinalIgnoreCase),
                "Clear search tooltip should explain read-only search clearing.");
            Assert(
                window.ClearSearchButtonAutomationHelpTextValue.Contains("Storage Review Search", StringComparison.OrdinalIgnoreCase)
                && window.ClearSearchButtonAutomationHelpTextValue.Contains("does not rescan", StringComparison.OrdinalIgnoreCase)
                && window.ClearSearchButtonAutomationHelpTextValue.Contains("modify files", StringComparison.OrdinalIgnoreCase)
                && window.ClearSearchButtonAutomationHelpTextValue.Contains("Review Shortlist", StringComparison.OrdinalIgnoreCase),
                "Clear search automation help text should explain read-only search clearing.");
            Assert(
                window.ResetReviewViewButtonToolTipValue.Contains("filters and search", StringComparison.OrdinalIgnoreCase)
                && window.ResetReviewViewButtonToolTipValue.Contains("keeping Review Shortlist", StringComparison.OrdinalIgnoreCase)
                && window.ResetReviewViewButtonToolTipValue.Contains("does not rescan", StringComparison.OrdinalIgnoreCase)
                && window.ResetReviewViewButtonToolTipValue.Contains("modify files", StringComparison.OrdinalIgnoreCase),
                "Reset view tooltip should explain read-only reset behavior.");
            Assert(
                window.ResetReviewViewButtonAutomationHelpTextValue.Contains("filters and search", StringComparison.OrdinalIgnoreCase)
                && window.ResetReviewViewButtonAutomationHelpTextValue.Contains("keeping Review Shortlist", StringComparison.OrdinalIgnoreCase)
                && window.ResetReviewViewButtonAutomationHelpTextValue.Contains("does not rescan", StringComparison.OrdinalIgnoreCase)
                && window.ResetReviewViewButtonAutomationHelpTextValue.Contains("modify files", StringComparison.OrdinalIgnoreCase),
                "Reset view automation help text should explain read-only reset behavior.");
            Assert(
                window.PreviousReviewWindowButtonToolTipValue.Contains("previous in-memory", StringComparison.OrdinalIgnoreCase)
                && window.PreviousReviewWindowButtonToolTipValue.Contains("Storage Review Display Window", StringComparison.OrdinalIgnoreCase)
                && window.PreviousReviewWindowButtonToolTipValue.Contains("does not rescan", StringComparison.OrdinalIgnoreCase)
                && window.PreviousReviewWindowButtonToolTipValue.Contains("modify files", StringComparison.OrdinalIgnoreCase),
                "Previous rows tooltip should explain read-only display-window navigation.");
            Assert(
                window.PreviousReviewWindowButtonAutomationHelpTextValue.Contains("previous in-memory", StringComparison.OrdinalIgnoreCase)
                && window.PreviousReviewWindowButtonAutomationHelpTextValue.Contains("Storage Review Display Window", StringComparison.OrdinalIgnoreCase)
                && window.PreviousReviewWindowButtonAutomationHelpTextValue.Contains("does not rescan", StringComparison.OrdinalIgnoreCase)
                && window.PreviousReviewWindowButtonAutomationHelpTextValue.Contains("modify files", StringComparison.OrdinalIgnoreCase),
                "Previous rows automation help text should explain read-only display-window navigation.");
            Assert(
                window.NextReviewWindowButtonToolTipValue.Contains("next in-memory", StringComparison.OrdinalIgnoreCase)
                && window.NextReviewWindowButtonToolTipValue.Contains("Storage Review Display Window", StringComparison.OrdinalIgnoreCase)
                && window.NextReviewWindowButtonToolTipValue.Contains("does not rescan", StringComparison.OrdinalIgnoreCase)
                && window.NextReviewWindowButtonToolTipValue.Contains("modify files", StringComparison.OrdinalIgnoreCase),
                "Next rows tooltip should explain read-only display-window navigation.");
            Assert(
                window.NextReviewWindowButtonAutomationHelpTextValue.Contains("next in-memory", StringComparison.OrdinalIgnoreCase)
                && window.NextReviewWindowButtonAutomationHelpTextValue.Contains("Storage Review Display Window", StringComparison.OrdinalIgnoreCase)
                && window.NextReviewWindowButtonAutomationHelpTextValue.Contains("does not rescan", StringComparison.OrdinalIgnoreCase)
                && window.NextReviewWindowButtonAutomationHelpTextValue.Contains("modify files", StringComparison.OrdinalIgnoreCase),
                "Next rows automation help text should explain read-only display-window navigation.");
            Assert(
                window.AddShownRowsToReviewShortlistButtonToolTipValue.Contains("currently visible review rows", StringComparison.OrdinalIgnoreCase)
                && window.AddShownRowsToReviewShortlistButtonToolTipValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.AddShownRowsToReviewShortlistButtonToolTipValue.Contains("no files are modified", StringComparison.OrdinalIgnoreCase),
                "Visible-row shortlist add tooltip should explain scope, approval boundary, and no-file-modified behavior.");
            Assert(
                window.RemoveShownRowsFromReviewShortlistButtonToolTipValue.Contains("currently visible review rows", StringComparison.OrdinalIgnoreCase)
                && window.RemoveShownRowsFromReviewShortlistButtonToolTipValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.RemoveShownRowsFromReviewShortlistButtonToolTipValue.Contains("no files are modified", StringComparison.OrdinalIgnoreCase),
                "Visible-row shortlist remove tooltip should explain scope, approval boundary, and no-file-modified behavior.");
            Assert(
                window.ExportShortlistButtonToolTipValue.Contains("CSV report only", StringComparison.OrdinalIgnoreCase)
                && window.ExportShortlistButtonToolTipValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.ExportShortlistButtonToolTipValue.Contains("no scanned files are modified", StringComparison.OrdinalIgnoreCase),
                "Export shortlist tooltip should explain report-only behavior and the non-approval boundary.");
            Assert(
                window.ExportShortlistButtonAutomationHelpTextValue.Contains("CSV report only", StringComparison.OrdinalIgnoreCase)
                && window.ExportShortlistButtonAutomationHelpTextValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.ExportShortlistButtonAutomationHelpTextValue.Contains("no scanned files are modified", StringComparison.OrdinalIgnoreCase),
                "Export shortlist automation help text should explain report-only behavior and the non-approval boundary.");
            Assert(
                window.ClearShortlistButtonToolTipValue.Contains("in-memory Review Shortlist", StringComparison.OrdinalIgnoreCase)
                && window.ClearShortlistButtonToolTipValue.Contains("does not delete", StringComparison.OrdinalIgnoreCase)
                && window.ClearShortlistButtonToolTipValue.Contains("restore files", StringComparison.OrdinalIgnoreCase),
                "Clear shortlist tooltip should explain that only in-memory review state changes.");
            Assert(
                window.ClearShortlistButtonAutomationHelpTextValue.Contains("in-memory Review Shortlist", StringComparison.OrdinalIgnoreCase)
                && window.ClearShortlistButtonAutomationHelpTextValue.Contains("does not delete", StringComparison.OrdinalIgnoreCase)
                && window.ClearShortlistButtonAutomationHelpTextValue.Contains("restore files", StringComparison.OrdinalIgnoreCase),
                "Clear shortlist automation help text should explain that only in-memory review state changes.");
            Assert(
                window.PreviewQuarantineButtonToolTipValue.Contains("read-only Quarantine Preview", StringComparison.OrdinalIgnoreCase)
                && window.PreviewQuarantineButtonToolTipValue.Contains("does not create folders", StringComparison.OrdinalIgnoreCase)
                && window.PreviewQuarantineButtonToolTipValue.Contains("approve cleanup", StringComparison.OrdinalIgnoreCase),
                "Preview quarantine tooltip should explain dry-run preview behavior.");
            Assert(
                window.PreviewQuarantineButtonAutomationHelpTextValue.Contains("read-only Quarantine Preview", StringComparison.OrdinalIgnoreCase)
                && window.PreviewQuarantineButtonAutomationHelpTextValue.Contains("does not create folders", StringComparison.OrdinalIgnoreCase)
                && window.PreviewQuarantineButtonAutomationHelpTextValue.Contains("approve cleanup", StringComparison.OrdinalIgnoreCase),
                "Preview quarantine automation help text should explain dry-run preview behavior.");
            Assert(
                window.ExportQuarantinePreviewButtonToolTipValue.Contains("CSV report only", StringComparison.OrdinalIgnoreCase)
                && window.ExportQuarantinePreviewButtonToolTipValue.Contains("does not execute Quarantine", StringComparison.OrdinalIgnoreCase)
                && window.ExportQuarantinePreviewButtonToolTipValue.Contains("modify scanned files", StringComparison.OrdinalIgnoreCase),
                "Export preview tooltip should explain report-only behavior.");
            Assert(
                window.ExportQuarantinePreviewButtonAutomationHelpTextValue.Contains("CSV report only", StringComparison.OrdinalIgnoreCase)
                && window.ExportQuarantinePreviewButtonAutomationHelpTextValue.Contains("does not execute Quarantine", StringComparison.OrdinalIgnoreCase)
                && window.ExportQuarantinePreviewButtonAutomationHelpTextValue.Contains("modify scanned files", StringComparison.OrdinalIgnoreCase),
                "Export preview automation help text should explain report-only behavior.");
            Assert(!window.CanPreviewSelectedRestoreGate, "MainWindow should not enable selected restore gate preview before selected manifest readiness.");
            Assert(!window.CanEnterSelectedRestoreConfirmation, "MainWindow should not allow selected restore confirmation before gate preview.");
            Assert(!window.CanExecuteSelectedRestore, "MainWindow should not allow selected restore execution before gate preview.");
            Assert(!window.CanShowSelectedFolderChildren, "MainWindow should not allow selected-folder child focus before a scan result is selected.");
            Assert(!window.CanShowSelectedFolderDescendants, "MainWindow should not allow selected-folder descendant focus before a scan result is selected.");
            Assert(
                window.AddSelectedRowToReviewShortlistButtonToolTipValue.Contains("selected row", StringComparison.OrdinalIgnoreCase)
                && window.AddSelectedRowToReviewShortlistButtonToolTipValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.AddSelectedRowToReviewShortlistButtonToolTipValue.Contains("no files are modified", StringComparison.OrdinalIgnoreCase),
                "Selected-row shortlist add tooltip should explain selected-only scope and approval boundary.");
            Assert(
                window.RemoveSelectedRowFromReviewShortlistButtonToolTipValue.Contains("selected row", StringComparison.OrdinalIgnoreCase)
                && window.RemoveSelectedRowFromReviewShortlistButtonToolTipValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.RemoveSelectedRowFromReviewShortlistButtonToolTipValue.Contains("no files are modified", StringComparison.OrdinalIgnoreCase),
                "Selected-row shortlist remove tooltip should explain selected-only scope and approval boundary.");
            Assert(
                window.CopySelectedPathButtonToolTipValue.Contains("manual inspection", StringComparison.OrdinalIgnoreCase)
                && window.CopySelectedPathButtonToolTipValue.Contains("does not modify files", StringComparison.OrdinalIgnoreCase)
                && window.CopySelectedPathButtonToolTipValue.Contains("approve cleanup", StringComparison.OrdinalIgnoreCase),
                "Copy path tooltip should explain inspection-only behavior.");
            Assert(
                window.ShowSelectedFolderChildrenButtonToolTipValue.Contains("Read-only focus", StringComparison.OrdinalIgnoreCase)
                && window.ShowSelectedFolderChildrenButtonToolTipValue.Contains("parent: search", StringComparison.OrdinalIgnoreCase)
                && window.ShowSelectedFolderChildrenButtonToolTipValue.Contains("does not rescan", StringComparison.OrdinalIgnoreCase)
                && window.ShowSelectedFolderChildrenButtonToolTipValue.Contains("approve cleanup", StringComparison.OrdinalIgnoreCase),
                "Show children tooltip should explain read-only focus behavior.");
            Assert(
                window.ShowSelectedFolderDescendantsButtonToolTipValue.Contains("Read-only focus", StringComparison.OrdinalIgnoreCase)
                && window.ShowSelectedFolderDescendantsButtonToolTipValue.Contains("under: search", StringComparison.OrdinalIgnoreCase)
                && window.ShowSelectedFolderDescendantsButtonToolTipValue.Contains("does not rescan", StringComparison.OrdinalIgnoreCase)
                && window.ShowSelectedFolderDescendantsButtonToolTipValue.Contains("approve cleanup", StringComparison.OrdinalIgnoreCase),
                "Show descendants tooltip should explain read-only focus behavior.");
            Assert(
                window.PreviewSelectedFileButtonToolTipValue.Contains("bounded text sample", StringComparison.OrdinalIgnoreCase)
                && window.PreviewSelectedFileButtonToolTipValue.Contains("Credential Data", StringComparison.OrdinalIgnoreCase)
                && window.PreviewSelectedFileButtonToolTipValue.Contains("modify files", StringComparison.OrdinalIgnoreCase),
                "Preview file tooltip should explain bounded read-only behavior.");
            Assert(
                window.OpenSelectedPathInExplorerButtonToolTipValue.Contains("File Explorer", StringComparison.OrdinalIgnoreCase)
                && window.OpenSelectedPathInExplorerButtonToolTipValue.Contains("manual inspection", StringComparison.OrdinalIgnoreCase)
                && window.OpenSelectedPathInExplorerButtonToolTipValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.OpenSelectedPathInExplorerButtonToolTipValue.Contains("does not modify files", StringComparison.OrdinalIgnoreCase),
                "Open in Explorer tooltip should explain inspection-only behavior.");
            Assert(window.BrowseQuarantineRootButtonText.Contains("Browse", StringComparison.OrdinalIgnoreCase), "Quarantine root browse action should be visible in the review toolbar.");
            Assert(
                window.BrowseQuarantineRootButtonToolTipValue.Contains("Quarantine Root", StringComparison.OrdinalIgnoreCase)
                && window.BrowseQuarantineRootButtonToolTipValue.Contains("preview paths only", StringComparison.OrdinalIgnoreCase)
                && window.BrowseQuarantineRootButtonToolTipValue.Contains("does not create folders", StringComparison.OrdinalIgnoreCase)
                && window.BrowseQuarantineRootButtonToolTipValue.Contains("approve cleanup", StringComparison.OrdinalIgnoreCase),
                "Quarantine Root browse tooltip should explain preview-only selection and no-file-modified behavior.");
            Assert(
                window.CurrentQuarantineRootPath == QuarantinePreviewBuilder.DefaultQuarantineRootPath,
                "MainWindow should default Quarantine Preview to the D: root requested by the user.");
            Assert(
                window.QuarantineRootSafetyNoteTextValue.Contains("Preferred Quarantine Root", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineRootSafetyNoteTextValue.Contains("does not create folders", StringComparison.OrdinalIgnoreCase),
                "Default Quarantine root note should explain preferred D: preview-only behavior.");
            Assert(
                window.QuarantineManifestDiscoveryTextValue.Contains("Read-only manifest discovery", StringComparison.OrdinalIgnoreCase),
                "Quarantine Manifest Discovery pane should start in a read-only placeholder state.");
            Assert(
                window.RestoreReadinessPreviewTextValue.Contains("Read-only all-manifest readiness", StringComparison.OrdinalIgnoreCase)
                && window.RestoreReadinessPreviewTextValue.Contains("Preview all-manifest readiness", StringComparison.OrdinalIgnoreCase),
                "Restore Readiness Preview pane should start in a read-only all-manifest placeholder state.");
            Assert(
                window.SelectedRestoreManifestReviewTextValue.Contains("Selected Restore Manifest Review", StringComparison.OrdinalIgnoreCase),
                "Selected Restore Manifest Review pane should start in a read-only placeholder state.");
            Assert(
                window.SelectedRestoreManifestReviewTextValue.Contains("Preview selected manifest readiness", StringComparison.OrdinalIgnoreCase),
                "Selected Restore Manifest Review placeholder should name the selected manifest readiness action.");
            Assert(
                window.SelectedRestoreExecutionGateTextValue.Contains("Selected Restore Confirmation Draft", StringComparison.OrdinalIgnoreCase),
                "Selected Restore Execution Gate pane should start in a read-only placeholder state.");
            Assert(window.BrowseCleanupScopeButtonText.Contains("Browse", StringComparison.OrdinalIgnoreCase), "Cleanup Scope browse action should be visible in the header.");
            Assert(
                window.BrowseCleanupScopeButtonToolTipValue.Contains("Cleanup Scope", StringComparison.OrdinalIgnoreCase)
                && window.BrowseCleanupScopeButtonToolTipValue.Contains("does not start a scan", StringComparison.OrdinalIgnoreCase)
                && window.BrowseCleanupScopeButtonToolTipValue.Contains("real-profile gate", StringComparison.OrdinalIgnoreCase)
                && window.BrowseCleanupScopeButtonToolTipValue.Contains("approve cleanup", StringComparison.OrdinalIgnoreCase),
                "Cleanup Scope browse tooltip should explain path-only selection and scan-gate boundaries.");
            Assert(window.IsRealProfilePreflightConfirmationVisible, "MainWindow should show the real-profile preflight acknowledgement.");
            Assert(!window.IsRealProfilePreflightConfirmed, "Real-profile preflight acknowledgement should start unchecked.");
            Assert(
                window.ScanGateSummaryTextValue.Contains("Scan locked for real profile", StringComparison.OrdinalIgnoreCase)
                && window.ScanGateSummaryTextValue.Contains("run MVP preflight", StringComparison.OrdinalIgnoreCase)
                && window.ScanGateSummaryTextValue.Contains("tick the acknowledgement", StringComparison.OrdinalIgnoreCase),
                "Real-profile scan gate summary should make the locked state and next action obvious.");
            Assert(
                window.ScanButtonToolTipValue.Contains("locked", StringComparison.OrdinalIgnoreCase)
                && window.ScanButtonToolTipValue.Contains("acknowledged", StringComparison.OrdinalIgnoreCase),
                "Disabled real-profile Scan button tooltip should explain the lock.");
            Assert(
                window.ScanGateTextValue.Contains("Confirm MVP preflight", StringComparison.OrdinalIgnoreCase),
                "Real-profile scan gate should explain why Scan is disabled.");
            window.ConfirmRealProfilePreflightForRealProfileScan();
            Assert(window.IsRealProfilePreflightConfirmed, "Real-profile preflight acknowledgement should be settable by the user.");
            Assert(window.CanStartStorageScan, "Real profile Scan should be enabled after acknowledgement.");
            Assert(
                window.ScanGateSummaryTextValue.Contains("Scan ready for real profile", StringComparison.OrdinalIgnoreCase)
                && window.ScanGateSummaryTextValue.Contains("read-only Storage Scan", StringComparison.OrdinalIgnoreCase),
                "Acknowledged real-profile scan gate summary should keep the read-only ready state visible.");
            Assert(
                window.ScanGateSummaryTextValue.Contains("cleanup execution remains unavailable", StringComparison.OrdinalIgnoreCase),
                "Acknowledged real-profile scan gate summary should keep execution unavailable.");
            Assert(
                window.ScanButtonToolTipValue.Contains("read-only Storage Scan", StringComparison.OrdinalIgnoreCase)
                && window.ScanButtonToolTipValue.Contains("cleanup execution remains unavailable", StringComparison.OrdinalIgnoreCase),
                "Enabled real-profile Scan button tooltip should keep the read-only and unavailable-execution boundary visible.");
            Assert(
                window.ScanGateTextValue.Contains("read-only", StringComparison.OrdinalIgnoreCase),
                "Confirmed real-profile scan gate should preserve read-only wording.");
            Assert(!window.CanExportScanCsv, "MainWindow should not allow CSV export before a scan.");
            Assert(!window.CanUseEntryTypeFilter, "MainWindow should not allow type filtering before a scan.");
            Assert(!window.CanUseSizeThresholdFilter, "MainWindow should not allow size threshold filtering before a scan.");
            Assert(!window.CanResetReviewView, "MainWindow should not allow review view reset before a scan.");
            Assert(!window.CanEnterQuarantineConfirmation, "MainWindow should not allow quarantine confirmation before a preview exists.");
            Assert(!window.CanExecuteQuarantine, "MainWindow should not allow quarantine execution before a preview exists.");
            Assert(
                window.QuarantineConfirmationToolTipValue.Contains("Fixture-only execution gate", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineConfirmationToolTipValue.Contains("custom execution stay unavailable", StringComparison.OrdinalIgnoreCase),
                "Quarantine confirmation tooltip should explain fixture-only execution and custom blockers.");
            Assert(
                window.ExecuteQuarantineButtonToolTipValue.Contains("Quarantine Preview readiness", StringComparison.OrdinalIgnoreCase)
                && window.ExecuteQuarantineButtonToolTipValue.Contains("real-profile/custom execution remains unavailable", StringComparison.OrdinalIgnoreCase),
                "Execute quarantine tooltip should explain preview readiness and real/custom blockers.");
            Assert(
                window.UndoQuarantineButtonToolTipValue.Contains("Current fixture execution only", StringComparison.OrdinalIgnoreCase)
                && window.UndoQuarantineButtonToolTipValue.Contains("real-profile/custom undo remain unavailable", StringComparison.OrdinalIgnoreCase),
                "Undo fixture quarantine tooltip should explain current-fixture-only undo.");
            Assert(
                window.SelectedRestoreConfirmationToolTipValue.Contains("Exact RESTORE", StringComparison.OrdinalIgnoreCase)
                && window.SelectedRestoreConfirmationToolTipValue.Contains("real-profile/custom selected restore remains unavailable", StringComparison.OrdinalIgnoreCase),
                "Selected restore confirmation tooltip should explain fixture-only selected restore.");
            Assert(
                window.ExecuteSelectedRestoreButtonToolTipValue.Contains("Fixture selected restore only", StringComparison.OrdinalIgnoreCase)
                && window.ExecuteSelectedRestoreButtonToolTipValue.Contains("selected manifest readiness", StringComparison.OrdinalIgnoreCase)
                && window.ExecuteSelectedRestoreButtonToolTipValue.Contains("real-profile/custom selected restore remains unavailable", StringComparison.OrdinalIgnoreCase),
                "Restore selected fixture manifest tooltip should explain fixture-only selected restore.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Create a Quarantine Preview", StringComparison.OrdinalIgnoreCase),
                "Quarantine execution gate should explain the preview dependency at startup.");
            Assert(
                window.MatchedReviewMixTextValue.Contains("appears after a scan", StringComparison.OrdinalIgnoreCase),
                "Matched Review Mix should stay in placeholder state before a scan.");
            Assert(window.SearchHelpToolTipValue.Contains("path:", StringComparison.OrdinalIgnoreCase), "Search tooltip should show field-prefix examples.");
            Assert(window.SearchHelpToolTipValue.Contains("parent:", StringComparison.OrdinalIgnoreCase), "Search tooltip should include parent-prefix guidance.");
            Assert(window.SearchHelpToolTipValue.Contains("under:", StringComparison.OrdinalIgnoreCase), "Search tooltip should include under-prefix descendant guidance.");
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
            Assert(
                window.ScanGateSummaryTextValue.Contains("Scan ready for fixture", StringComparison.OrdinalIgnoreCase),
                "Fixture scan gate summary should make fixture readiness visible.");
            Assert(
                window.ScanGateSummaryTextValue.Contains("fixture cleanup actions stay gated", StringComparison.OrdinalIgnoreCase),
                "Fixture scan gate summary should keep later fixture cleanup actions behind gates.");
            Assert(
                window.ScanButtonToolTipValue.Contains("read-only Storage Scan", StringComparison.OrdinalIgnoreCase)
                && window.ScanButtonToolTipValue.Contains("preview and exact confirmation", StringComparison.OrdinalIgnoreCase),
                "Fixture Scan button tooltip should preserve the read-only boundary and later cleanup gates.");
            Assert(window.CanBrowseCleanupScope, "Launch Cleanup Scope should still allow choosing a different Cleanup Scope before scanning.");
            Assert(!window.IsRealProfilePreflightConfirmationVisible, "Fixture Cleanup Scope should not show real-profile acknowledgement.");
            Assert(!window.CanExportScanCsv, "Launch Cleanup Scope should not create exportable scan data.");
        }
        finally
        {
            window.Close();
        }
    }

    public void MainWindowUsesCustomCleanupScopeWithoutExecutionAvailability()
    {
        var customScope = Path.Combine(
            Environment.CurrentDirectory,
            ".local",
            "manual-review-scope");

        var window = new MainWindow(customScope);
        try
        {
            Assert(
                window.CurrentCleanupScopePath == customScope,
                "MainWindow should use the custom launch Cleanup Scope.");
            Assert(
                window.CleanupScopeSafetyNoteTextValue.Contains("Custom Cleanup Scope", StringComparison.OrdinalIgnoreCase),
                "Custom launch Cleanup Scope should show the custom safety note.");
            Assert(window.CanStartStorageScan, "Custom Cleanup Scope should allow a read-only user-triggered scan.");
            Assert(
                window.ScanGateSummaryTextValue.Contains("Scan ready for custom Cleanup Scope", StringComparison.OrdinalIgnoreCase)
                && window.ScanGateSummaryTextValue.Contains("cleanup execution remains unavailable", StringComparison.OrdinalIgnoreCase),
                "Custom scan gate summary should keep cleanup execution unavailable.");
            Assert(
                window.ScanButtonToolTipValue.Contains("read-only Storage Scan", StringComparison.OrdinalIgnoreCase)
                && window.ScanButtonToolTipValue.Contains("cleanup execution remains unavailable", StringComparison.OrdinalIgnoreCase),
                "Custom Scan button tooltip should keep the read-only and unavailable-execution boundary visible.");
            Assert(!window.IsRealProfilePreflightConfirmationVisible, "Custom Cleanup Scope should not show real-profile acknowledgement.");
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
            Assert(window.CanBrowseCleanupScope, "Cleanup Scope browse action should be re-enabled after the fixture scan.");
            Assert(window.CanExportScanCsv, "CSV export should be enabled after a completed scan.");
            Assert(window.CanUseCategoryFilter, "Category filter should be enabled after a categorized fixture scan.");
            Assert(window.CanUseEntryTypeFilter, "Type filter should be enabled after a completed scan.");
            Assert(window.CanUseSizeThresholdFilter, "Size threshold filter should be enabled after a completed scan.");
            Assert(!window.CanResetReviewView, "Reset view should be disabled while the review view is unfiltered.");
            Assert(window.CurrentEntryTypeFilterLabel.Contains("All types", StringComparison.OrdinalIgnoreCase), "Type filter should start on All types.");
            Assert(window.CurrentSizeThresholdFilterLabel.Contains("All sizes", StringComparison.OrdinalIgnoreCase), "Size threshold filter should start on All sizes.");
            Assert(window.TotalSizeTextValue != "-", "Total size card should be populated after scan.");
            Assert(window.FolderCountTextValue != "-", "Folder count card should be populated after scan.");
            Assert(window.FileCountTextValue != "-", "File count card should be populated after scan.");
            Assert(window.AccessIssueCountTextValue == "0", "Synthetic fixture should scan without access issues.");
            Assert(window.ReviewMixTextValue.Contains("Quarantine candidates", StringComparison.OrdinalIgnoreCase), "Review Mix should summarize quarantine candidates.");
            Assert(window.SafetySummaryTextValue.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Safety Summary should state the read-only boundary.");
            Assert(window.SafetySummaryTextValue.Contains("Candidate examples:", StringComparison.OrdinalIgnoreCase), "Safety Summary should show bounded quarantine candidate examples.");
            Assert(window.SafetySummaryTextValue.Contains(@"Downloads\old-installer.msi", StringComparison.OrdinalIgnoreCase), "Safety Summary candidate examples should include relative candidate paths.");
            Assert(window.SafetySummaryTextValue.Contains("No category examples:", StringComparison.OrdinalIgnoreCase), "Safety Summary should show bounded no-category examples.");
            Assert(window.SafetySummaryTextValue.Contains(@"Unknown\notes.txt", StringComparison.OrdinalIgnoreCase), "Safety Summary no-category examples should include relative uncategorized paths.");
            Assert(window.FilterSummaryTextValue.Contains("All:", StringComparison.OrdinalIgnoreCase), "Filter summary should start on the All filter.");
            Assert(
                window.MatchedReviewMixTextValue.Contains("Matched review mix:", StringComparison.OrdinalIgnoreCase)
                && window.MatchedReviewMixTextValue.Contains("Likely safe", StringComparison.OrdinalIgnoreCase)
                && window.MatchedReviewMixTextValue.Contains("Caution", StringComparison.OrdinalIgnoreCase)
                && window.MatchedReviewMixTextValue.Contains("High risk", StringComparison.OrdinalIgnoreCase)
                && window.MatchedReviewMixTextValue.Contains("Quarantine candidates", StringComparison.OrdinalIgnoreCase)
                && window.MatchedReviewMixTextValue.Contains("No category", StringComparison.OrdinalIgnoreCase)
                && window.MatchedReviewMixTextValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase),
                "Matched Review Mix should summarize the current matched rows without implying cleanup approval.");
            Assert(window.ReviewSizeNoteTextValue.Contains("parent and child rows can overlap", StringComparison.OrdinalIgnoreCase), "Review size note should explain recursive row overlap.");
            Assert(window.ReviewSizeNoteTextValue.Contains("not storage savings", StringComparison.OrdinalIgnoreCase), "Review size note should avoid treating row sizes as savings.");
            Assert(
                window.ContentsColumnSortMemberPath == "ContainedTotalCount",
                "Contents column should sort by numeric contained item count rather than formatted display text.");
            Assert(window.HasRelativePathColumn, "Results grid should expose a cleanup-scope-relative path column.");

            var rows = window.DisplayedRows;
            var rootRow = rows.Single(row =>
                row.FullPath.Equals(Path.GetFullPath(fixture.RootPath), StringComparison.OrdinalIgnoreCase));
            Assert(rootRow.RelativePath == ".", "Cleanup Scope root row should show '.' as its relative path.");
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
            Assert(downloads.RelativePath == "Downloads", "Fixture Downloads folder should show a cleanup-scope-relative path.");
            Assert(downloads.AccessStatus == "Readable", "Fixture Downloads folder should show readable access status.");
            Assert(
                downloads.Contents.Contains("1 file", StringComparison.OrdinalIgnoreCase)
                && downloads.Contents.Contains("0 folders", StringComparison.OrdinalIgnoreCase),
                "Fixture Downloads folder should expose contents counts in the grid row.");
            Assert(downloads.ContainedTotalCount == 1, "Fixture Downloads contents sort value should total contained files and folders.");
            Assert(window.SelectDisplayedPath(downloads.FullPath), "Fixture Downloads folder should be selectable for contents context.");
            Assert(window.CanShowSelectedFolderChildren, "Selected folder rows should enable selected-folder child focus.");
            Assert(window.CanShowSelectedFolderDescendants, "Selected folder rows should enable selected-folder descendant focus.");
            Assert(downloads.Contents.Contains("1 file", StringComparison.OrdinalIgnoreCase), "Folder row should expose contained file count.");
            Assert(
                window.DetailPathContextTextValue.Contains("Contents:", StringComparison.OrdinalIgnoreCase)
                && window.DetailPathContextTextValue.Contains("Relative:", StringComparison.OrdinalIgnoreCase)
                && window.DetailPathContextTextValue.Contains("Downloads", StringComparison.OrdinalIgnoreCase)
                && window.DetailPathContextTextValue.Contains("1 file", StringComparison.OrdinalIgnoreCase),
                "Selected folder detail pane should show relative path and contained file/folder counts.");
            Assert(window.DetailMetaTextValue.Contains("Access: Readable", StringComparison.OrdinalIgnoreCase), "Selected row detail pane should show access status.");
            Assert(
                window.DetailSubtreeSummaryTextValue.Contains("Descendant rows: 1", StringComparison.OrdinalIgnoreCase)
                && window.DetailSubtreeSummaryTextValue.Contains("Quarantine candidates 1", StringComparison.OrdinalIgnoreCase)
                && window.DetailSubtreeSummaryTextValue.Contains("not storage savings or cleanup approval", StringComparison.OrdinalIgnoreCase),
                "Selected folder detail pane should show a read-only descendant review summary.");
            Assert(
                window.DetailSubtreeSummaryTextValue.Contains("Candidate examples", StringComparison.OrdinalIgnoreCase)
                && window.DetailSubtreeSummaryTextValue.Contains(@"Downloads\old-installer.msi", StringComparison.OrdinalIgnoreCase),
                "Descendant summary should include bounded candidate examples.");
            Assert(
                window.DetailHotspotTrailTextValue.Contains("old-installer.msi", StringComparison.OrdinalIgnoreCase)
                && window.DetailHotspotTrailTextValue.Contains("not storage savings", StringComparison.OrdinalIgnoreCase),
                "Selected folder detail pane should show a read-only hotspot trail with overlap wording.");
            window.ShowSelectedFolderChildren();
            Assert(window.CurrentSearchText.StartsWith("parent:", StringComparison.OrdinalIgnoreCase), "Selected-folder child focus should apply a parent-prefixed search.");
            Assert(window.FilterSummaryTextValue.Contains("Search \"parent:", StringComparison.OrdinalIgnoreCase), "Selected-folder child focus should update the filter summary.");
            Assert(window.CurrentStatusText.Contains("Focused review on immediate children", StringComparison.OrdinalIgnoreCase), "Selected-folder child focus should report a read-only focus action.");
            Assert(window.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Selected-folder child focus status should preserve the read-only boundary.");
            Assert(window.DisplayedRows.Count == 1, "Fixture Downloads focus should show its one immediate child.");
            Assert(
                window.DisplayedRows.All(row => row.ParentLocation.Equals(downloads.FullPath, StringComparison.OrdinalIgnoreCase)),
                "Selected-folder child focus should show only immediate children of the selected folder.");
            Assert(
                window.DisplayedRows.Single().FullPath.EndsWith(@"Downloads\old-installer.msi", StringComparison.OrdinalIgnoreCase),
                "Selected-folder child focus should include the fixture installer child.");
            window.ResetReviewView();

            var appDataPath = Path.Combine(fixture.RootPath, "AppData");
            window.ApplyStorageReviewFilter(StorageReviewFilter.Caution);
            Assert(window.SelectDisplayedPath(appDataPath), "Fixture AppData folder should remain selectable while the Caution lens is active.");
            window.ShowSelectedFolderDescendants();
            Assert(window.CurrentSearchText.StartsWith("under:", StringComparison.OrdinalIgnoreCase), "Selected-folder descendant focus should apply an under-prefixed search.");
            Assert(window.FilterSummaryTextValue.StartsWith("All + Search \"under:", StringComparison.OrdinalIgnoreCase), "Selected-folder descendant focus should reset review lenses to All before applying under-search.");
            Assert(window.CurrentEntryTypeFilterLabel.Contains("All types", StringComparison.OrdinalIgnoreCase), "Selected-folder descendant focus should reset the type lens to All.");
            Assert(window.CurrentSizeThresholdFilterLabel.Contains("All sizes", StringComparison.OrdinalIgnoreCase), "Selected-folder descendant focus should reset the size lens to All.");
            Assert(window.CurrentStatusText.Contains("Focused review on descendants", StringComparison.OrdinalIgnoreCase), "Selected-folder descendant focus should report a read-only focus action.");
            Assert(window.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Selected-folder descendant focus status should preserve the read-only boundary.");
            Assert(
                window.MatchedReviewMixTextValue.Contains("Matched review mix:", StringComparison.OrdinalIgnoreCase)
                && window.MatchedReviewMixTextValue.Contains("Review context only", StringComparison.OrdinalIgnoreCase)
                && window.MatchedReviewMixTextValue.Contains("Quarantine candidates", StringComparison.OrdinalIgnoreCase),
                "Selected-folder descendant focus should update Matched Review Mix for the focused subtree.");
            Assert(window.DisplayedRows.Count > 1, "Fixture AppData descendant focus should show nested descendant rows.");
            Assert(
                window.DisplayedRows.All(row =>
                    !row.FullPath.Equals(appDataPath, StringComparison.OrdinalIgnoreCase)
                    && PathSafety.IsWithinScope(appDataPath, row.FullPath)),
                "Selected-folder descendant focus should show only descendants under the selected folder.");
            Assert(
                window.DisplayedRows.Any(row => row.FullPath.EndsWith(@"pip\Cache\http-v2\response.body", StringComparison.OrdinalIgnoreCase)),
                "Selected-folder descendant focus should include deeply nested fixture descendants.");
            window.ResetReviewView();

            var exportCsv = window.CurrentScanReportExportCsv;
            Assert(
                exportCsv.Contains("\"Full path\",\"Relative path\",\"Parent path\"", StringComparison.Ordinal),
                "Scan Report Export CSV should include a cleanup-scope-relative path column.");
            Assert(
                exportCsv.Contains("\"Downloads\\old-installer.msi\"", StringComparison.Ordinal),
                "Scan Report Export CSV should include fixture-relative paths for spreadsheet review.");

            Assert(window.SelectDisplayedPath(fixture.MarkerPath), "Fixture note file should be selectable for preview.");
            Assert(!window.CanShowSelectedFolderChildren, "Selected files should not enable selected-folder child focus.");
            Assert(!window.CanShowSelectedFolderDescendants, "Selected files should not enable selected-folder descendant focus.");
            Assert(
                window.DetailSubtreeSummaryTextValue.Contains("Files do not have descendant subtree summaries", StringComparison.OrdinalIgnoreCase),
                "Selected files should explain that subtree summaries apply to folders.");
            Assert(
                window.DetailHotspotTrailTextValue.Contains("Files do not have descendant hotspot trails", StringComparison.OrdinalIgnoreCase),
                "Selected files should explain that hotspot trails apply to folders.");
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
                window.MatchedReviewMixTextValue.Contains("No category 0", StringComparison.OrdinalIgnoreCase),
                "Matched Review Mix should recompute after prefixed search narrows the active review lens.");
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

            window.EnterStorageReviewSearchText("pip");
            Assert(window.IsStorageReviewSearchPending, "User-typed Storage Review Search should debounce before applying the filter.");
            Assert(window.CurrentSearchText == "pip", "Debounced user search should still update the textbox immediately.");
            Assert(
                !window.FilterSummaryTextValue.Contains("Search \"pip\"", StringComparison.OrdinalIgnoreCase),
                "Debounced user search should not refresh the large result set until the debounce is committed.");
            Assert(
                window.CurrentStatusText.Contains("typing pauses", StringComparison.OrdinalIgnoreCase),
                "Debounced user search should explain that filtering waits for typing to pause.");
            window.ApplyPendingStorageReviewSearch();
            Assert(!window.IsStorageReviewSearchPending, "Committed user search should stop the debounce timer.");
            Assert(
                window.FilterSummaryTextValue.Contains("Search \"pip\"", StringComparison.OrdinalIgnoreCase),
                "Committed user search should refresh the active review lens.");
            Assert(
                window.CurrentStatusText.Contains("Search applied", StringComparison.OrdinalIgnoreCase),
                "Committed user search should report that the search was applied without modifying files.");

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

            window.SelectSizeThresholdFilterThroughCombo(StorageSizeThresholdFilter.AtLeast1Mb);
            Assert(window.FilterSummaryTextValue.Contains("1 MB+", StringComparison.OrdinalIgnoreCase), "Size threshold combo should update the filter summary.");
            Assert(window.DisplayedRows.Count > 0, "1 MB+ size threshold should show matching fixture rows.");
            Assert(
                window.DisplayedRows.All(row => row.SizeBytes >= 1024L * 1024),
                "1 MB+ size threshold should only show rows at or above the threshold.");
            Assert(
                window.CurrentScanReportExportRowCount == window.DisplayedRows.Count,
                "Scan Report Export row count should honor the active size threshold for the fixture.");
            Assert(
                window.CurrentScanReportExportFileName.Contains("-size-1mb-plus", StringComparison.OrdinalIgnoreCase),
                "Scan Report Export filename should include active size threshold filter.");

            window.ApplySizeThresholdFilter(StorageSizeThresholdFilter.All);
            Assert(window.CurrentSizeThresholdFilterLabel.Contains("All sizes", StringComparison.OrdinalIgnoreCase), "Size threshold filter should return to All sizes.");

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
            window.ApplyStorageReviewSearch("old-installer");
            Assert(window.DisplayedRows.Count == 1, "Old installer search should narrow the quarantine candidate review window to one row.");
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
            Assert(
                window.AddShownRowsToReviewShortlistButtonText.Contains("visible rows", StringComparison.OrdinalIgnoreCase)
                && window.RemoveShownRowsFromReviewShortlistButtonText.Contains("visible rows", StringComparison.OrdinalIgnoreCase),
                "Bulk shortlist buttons should say they affect visible rows.");
            Assert(!window.CanRemoveShownRowsFromReviewShortlist, "Visible rows should not be removable before they are shortlisted.");
            Assert(!window.CanPreviewQuarantine, "Quarantine Preview should be disabled before a shortlist exists.");
            Assert(
                window.ShortlistSafetyMixTextValue.Contains("Review Shortlist is empty", StringComparison.OrdinalIgnoreCase),
                "Shortlist Safety Mix should start empty before rows are shortlisted.");

            window.AddShownRowsToReviewShortlist();
            Assert(window.ReviewShortlistCount == 1, "Bulk shortlisting visible rows should update Review Shortlist count.");
            Assert(
                window.CurrentStatusText.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase),
                "Bulk shortlisting status should preserve the non-approval boundary.");
            Assert(!window.CanAddShownRowsToReviewShortlist, "Bulk shortlist should disable once every shown row is already shortlisted.");
            Assert(window.CanRemoveShownRowsFromReviewShortlist, "Bulk-shortlisted visible rows should be removable as the current review window.");
            Assert(window.CanRemoveSelectedRowFromReviewShortlist, "Shortlisted row should be removable.");
            Assert(window.CanPreviewQuarantine, "Quarantine Preview should be available after shortlisting a row.");
            Assert(
                window.DisplayedRows.Any(row => row.FullPath == installer.FullPath && row.Shortlist == "Yes"),
                "Shortlisted row should be marked in the WPF grid.");
            Assert(
                window.ShortlistSafetyMixTextValue.Contains("Shortlist safety mix: 1 row", StringComparison.OrdinalIgnoreCase)
                && window.ShortlistSafetyMixTextValue.Contains("Likely safe 1", StringComparison.OrdinalIgnoreCase)
                && window.ShortlistSafetyMixTextValue.Contains("Quarantine candidates 1", StringComparison.OrdinalIgnoreCase)
                && window.ShortlistSafetyMixTextValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase),
                "Shortlist Safety Mix should summarize shortlisted rows without implying approval.");

            window.SetQuarantineRootForPreview(@"relative\quarantine");
            Assert(!window.CanPreviewQuarantine, "Relative Quarantine roots should disable Quarantine Preview.");
            Assert(
                window.QuarantineRootSafetyNoteTextValue.Contains("fully qualified", StringComparison.OrdinalIgnoreCase),
                "Relative Quarantine roots should show a fully qualified path warning.");

            window.ApplyEntryTypeFilter(StorageEntryTypeFilter.Files);
            window.ApplySizeThresholdFilter(StorageSizeThresholdFilter.AtLeast1Mb);
            window.ApplyStorageReviewSearch("old-installer");
            Assert(window.CanResetReviewView, "Reset view should be enabled when multiple review lenses are active.");
            window.ResetReviewView();
            Assert(!window.CanResetReviewView, "Reset view should disable after returning to the default review lens.");
            Assert(window.CurrentSearchText == "", "Reset view should clear Storage Review Search.");
            Assert(window.CurrentEntryTypeFilterLabel.Contains("All types", StringComparison.OrdinalIgnoreCase), "Reset view should restore All types.");
            Assert(window.CurrentSizeThresholdFilterLabel.Contains("All sizes", StringComparison.OrdinalIgnoreCase), "Reset view should restore All sizes.");
            Assert(window.FilterSummaryTextValue.StartsWith("All:", StringComparison.OrdinalIgnoreCase), "Reset view should restore the All review filter.");
            Assert(window.ReviewShortlistCount == 1, "Reset view should keep Review Shortlist entries.");
            Assert(
                window.CurrentStatusText.Contains("Review Shortlist was kept", StringComparison.OrdinalIgnoreCase),
                "Reset view status should explain that shortlist entries were preserved.");
            window.ApplySafetyReviewShortcut(StorageScanSafetyShortcut.QuarantineCandidates);
            window.ApplyStorageReviewSearch("old-installer");
            Assert(window.SelectDisplayedPath(installer.FullPath), "Shortlisted fixture installer should be selectable after resetting and restoring the candidate view.");

            window.RemoveShownRowsFromReviewShortlist();
            Assert(window.ReviewShortlistCount == 0, "Removing visible rows should update Review Shortlist count.");
            Assert(
                window.ShortlistSafetyMixTextValue.Contains("Review Shortlist is empty", StringComparison.OrdinalIgnoreCase),
                "Removing visible rows should refresh Shortlist Safety Mix to empty.");
            Assert(
                window.CurrentStatusText.Contains("Removed 1 visible row", StringComparison.OrdinalIgnoreCase),
                "Removing visible rows should report the visible-window removal.");
            Assert(window.CanAddShownRowsToReviewShortlist, "Visible rows should be bulk-addable after visible-window removal.");
            Assert(!window.CanRemoveShownRowsFromReviewShortlist, "Visible rows should not be removable after visible-window removal.");

            window.AddShownRowsToReviewShortlist();
            Assert(window.ReviewShortlistCount == 1, "Bulk shortlisting visible rows should be repeatable after visible-window removal.");

            var customQuarantineRoot = Path.GetFullPath(Path.Combine(fixture.RootPath, "..", "custom-quarantine-root"));
            window.SetQuarantineRootForPreview(customQuarantineRoot);
            Assert(window.CurrentQuarantineRootPath == customQuarantineRoot, "Quarantine root should be editable before preview.");

            window.PreviewQuarantineForReviewShortlist();
            Assert(window.CurrentStatusText.Contains("Quarantine Preview created", StringComparison.OrdinalIgnoreCase), "Preview action should update status text.");
            Assert(window.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Preview status should preserve the read-only boundary.");
            Assert(window.CanExportQuarantinePreview, "Quarantine Preview export should be enabled after preview creation.");
            Assert(window.CurrentQuarantinePreviewRootPath == customQuarantineRoot, "Quarantine Preview should use the typed quarantine root.");
            Assert(window.QuarantinePreviewTextValue.Contains("Quarantine root:", StringComparison.OrdinalIgnoreCase), "Preview pane should label the quarantine root.");
            Assert(window.QuarantinePreviewTextValue.Contains(customQuarantineRoot, StringComparison.OrdinalIgnoreCase), "Preview pane should show the typed quarantine root.");
            Assert(window.QuarantinePreviewTextValue.Contains("Included: 1", StringComparison.OrdinalIgnoreCase), "Preview pane should show one included fixture row.");
            Assert(window.QuarantinePreviewTextValue.Contains("Restore Manifest Draft", StringComparison.OrdinalIgnoreCase), "Preview pane should show Restore Manifest Draft readiness.");
            Assert(window.QuarantinePreviewTextValue.Contains("Quarantine Confirmation Draft", StringComparison.OrdinalIgnoreCase), "Preview pane should show Quarantine Confirmation Draft readiness.");
            Assert(
                window.QuarantinePreviewTextValue.Contains("Required confirmation text: QUARANTINE", StringComparison.OrdinalIgnoreCase)
                && !window.QuarantinePreviewTextValue.Contains("Required future text", StringComparison.OrdinalIgnoreCase),
                "Quarantine preview pane should use current confirmation wording rather than stale future wording.");
            Assert(window.QuarantinePreviewTextValue.Contains("Execution implemented: yes", StringComparison.OrdinalIgnoreCase), "Fixture preview pane should state fixture execution is available.");
            Assert(
                window.QuarantinePreviewTextValue.Contains("Execution scope status", StringComparison.OrdinalIgnoreCase)
                && window.QuarantinePreviewTextValue.Contains("Fixture-only execution is available", StringComparison.OrdinalIgnoreCase),
                "Fixture preview pane should explain fixture-only execution availability in plain language.");
            Assert(
                window.QuarantinePreviewTextValue.Contains("Approval boundary", StringComparison.OrdinalIgnoreCase)
                && window.QuarantinePreviewTextValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.QuarantinePreviewTextValue.Contains("only fixture execution", StringComparison.OrdinalIgnoreCase),
                "Fixture preview pane should keep shortlist/preview separate from cleanup approval.");
            Assert(window.QuarantinePreviewTextValue.Contains("Preview rows:", StringComparison.OrdinalIgnoreCase), "Preview pane should label row-level preview details.");
            Assert(window.QuarantinePreviewTextValue.Contains("Preview row | Included", StringComparison.OrdinalIgnoreCase), "Preview pane should distinguish included row details from readiness blockers.");
            Assert(window.CanEnterQuarantineConfirmation, "Quarantine confirmation text should be enabled after preview readiness exists.");
            Assert(!window.CanExecuteQuarantine, "Quarantine execution should remain unavailable before exact confirmation text is entered.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Type QUARANTINE", StringComparison.OrdinalIgnoreCase),
                "Execution gate should require the confirmation phrase before execution can ever be enabled.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Execution scope status", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("Fixture-only execution is available", StringComparison.OrdinalIgnoreCase),
                "Fixture execution gate should explain fixture-only scope availability.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Approval boundary", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("only fixture execution", StringComparison.OrdinalIgnoreCase),
                "Fixture execution gate should keep exact confirmation separate from broad cleanup approval.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Quarantine Action Draft", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("Action items root:", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("Restore manifest path:", StringComparison.OrdinalIgnoreCase),
                "Execution gate should show the read-only action-scoped quarantine layout draft.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Write-ahead Restore Manifest", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("Manifest write order", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("before the first move", StringComparison.OrdinalIgnoreCase),
                "Execution gate should show the planned manifest write order before file-moving code exists.");
            window.SetQuarantineConfirmationText("NOPE");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Entered confirmation matches: no", StringComparison.OrdinalIgnoreCase),
                "Wrong confirmation text should keep the execution gate unmatched.");
            window.SetQuarantineConfirmationText("QUARANTINE");
            Assert(window.CurrentQuarantineConfirmationText == "QUARANTINE", "Confirmation text should be editable in the WPF gate.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Entered confirmation matches: yes", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("Execution implemented: yes", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("Can execute: yes", StringComparison.OrdinalIgnoreCase),
                "Matched confirmation should open fixture-only execution.");
            Assert(window.CanExecuteQuarantine, "Fixture-only execution should enable after exact confirmation text.");
            Assert(
                window.QuarantinePreviewTextValue.Contains(Path.Combine(customQuarantineRoot, "preview", "Downloads", "old-installer.msi"), StringComparison.OrdinalIgnoreCase),
                "Preview pane should map included rows under the typed quarantine root.");
            Assert(window.QuarantinePreviewTextValue.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Preview pane should preserve the read-only boundary.");
            Assert(File.Exists(installer.FullPath), "Shortlisted fixture installer should still exist after preview.");
            Assert(File.Exists(fixture.MarkerPath), "Fixture marker file should still exist after review interactions.");
            Assert(!Directory.Exists(customQuarantineRoot), "Quarantine Preview should not create the typed quarantine root.");

            var changedQuarantineRoot = Path.GetFullPath(Path.Combine(fixture.RootPath, "..", "changed-quarantine-root"));
            window.SetQuarantineRootForPreview(changedQuarantineRoot);
            Assert(window.CurrentStatusText.Contains("Quarantine root changed", StringComparison.OrdinalIgnoreCase), "Changing the quarantine root should invalidate stale preview destinations.");
            Assert(window.CanPreviewQuarantine, "Changing the quarantine root should keep preview available while the shortlist exists.");
            Assert(!window.CanExportQuarantinePreview, "Changing the quarantine root should disable exporting the stale preview.");
            Assert(!window.CanEnterQuarantineConfirmation, "Changing the quarantine root should disable stale confirmation text.");
            Assert(window.CurrentQuarantineConfirmationText == "", "Changing the quarantine root should clear stale confirmation text.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Create a Quarantine Preview", StringComparison.OrdinalIgnoreCase),
                "Changing the quarantine root should reset execution gate wording.");
            Assert(
                window.QuarantinePreviewTextValue.Contains("Preview and draft readiness appear", StringComparison.OrdinalIgnoreCase),
                "Changing the quarantine root should clear stale preview readiness text.");
            Assert(!Directory.Exists(changedQuarantineRoot), "Changing the preview root should not create folders.");

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
            Assert(
                window.ShortlistSafetyMixTextValue.Contains("Review Shortlist is empty", StringComparison.OrdinalIgnoreCase),
                "Clearing should refresh Shortlist Safety Mix to empty.");
        }
        finally
        {
            window.Close();
        }
    }

    public void MainWindowExecutesQuarantineForFixtureScopeOnly()
    {
        using var fixture = SmokeFixture.Create();
        var window = new MainWindow(fixture.RootPath);
        try
        {
            RunDispatcherTask(() => window.RunStorageScanForCurrentScopeAsync());

            var installer = window.DisplayedRows.Single(row =>
                row.FullPath.EndsWith(@"Downloads\old-installer.msi", StringComparison.OrdinalIgnoreCase));
            Assert(window.SelectDisplayedPath(installer.FullPath), "Fixture installer should be selectable for execution smoke test.");
            window.AddSelectedPathToReviewShortlist();

            var customQuarantineRoot = Path.GetFullPath(Path.Combine(fixture.RootPath, "..", "execution-quarantine-root"));
            window.SetQuarantineRootForPreview(customQuarantineRoot);
            window.PreviewQuarantineForReviewShortlist();

            Assert(!window.CanExecuteQuarantine, "Fixture execution should stay closed before exact confirmation.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Execution implemented: yes", StringComparison.OrdinalIgnoreCase),
                "Fixture execution gate should show execution availability.");

            window.SetQuarantineConfirmationText("QUARANTINE");
            Assert(window.CanExecuteQuarantine, "Fixture execution should open after exact confirmation.");
            var manifestPath = window.CurrentRestoreManifestPath;
            var quarantinePath = window.CurrentFirstQuarantinePath;
            Assert(!string.IsNullOrWhiteSpace(manifestPath), "Fixture execution should expose a planned Restore Manifest path before execution.");
            Assert(!string.IsNullOrWhiteSpace(quarantinePath), "Fixture execution should expose a planned quarantine path before execution.");

            window.ExecuteQuarantineForCurrentPreview();

            Assert(!window.CanExecuteQuarantine, "Execution gate should close after the fixture execution attempt.");
            Assert(!window.CanEnterQuarantineConfirmation, "Confirmation text should disable after the fixture execution attempt.");
            Assert(window.CanUndoQuarantine, "Fixture undo should become available after a successful fixture execution.");
            Assert(!window.CanExportQuarantinePreview, "Preview export should disable after execution because preview state is stale.");
            Assert(window.ReviewShortlistCount == 0, "Fixture execution should clear Review Shortlist to prevent stale re-execution.");
            Assert(window.CurrentRestoreManifestStatus == RestoreManifestActionStatus.Completed.ToString(), "Successful fixture execution should complete the Restore Manifest.");
            Assert(File.Exists(manifestPath!), "Fixture execution should persist the action-scoped Restore Manifest.");
            Assert(!File.Exists(installer.FullPath), "Fixture execution should move the selected source file.");
            Assert(File.Exists(quarantinePath!), "Fixture execution should place the selected file in quarantine.");
            Assert(File.Exists(fixture.MarkerPath), "Fixture execution should leave unselected fixture files alone.");
            Assert(
                window.CurrentStatusText.Contains("Fixture Quarantine execution completed", StringComparison.OrdinalIgnoreCase)
                && window.CurrentStatusText.Contains("Rescan", StringComparison.OrdinalIgnoreCase),
                "Fixture execution status should report completion and stale scan state.");
            Assert(
                window.QuarantinePreviewTextValue.Contains("Fixture Quarantine execution result", StringComparison.OrdinalIgnoreCase)
                && window.QuarantinePreviewTextValue.Contains("Current scan and review rows are stale", StringComparison.OrdinalIgnoreCase),
                "Preview pane should be replaced with execution result and stale-state wording.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Execution result:", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("Current scan results are stale", StringComparison.OrdinalIgnoreCase),
                "Execution gate should retain execution evidence after the gate closes.");

            window.UndoQuarantineForCurrentExecution();

            Assert(!window.CanUndoQuarantine, "Fixture undo should disable after the undo attempt.");
            Assert(window.CurrentRestoreManifestStatus == RestoreManifestActionStatus.Restored.ToString(), "Successful fixture undo should mark the Restore Manifest restored.");
            Assert(File.Exists(installer.FullPath), "Fixture undo should restore the selected source file.");
            Assert(!File.Exists(quarantinePath!), "Fixture undo should move the selected file out of quarantine.");
            Assert(File.Exists(manifestPath!), "Fixture undo should keep the Restore Manifest for recovery evidence.");
            Assert(
                window.CurrentStatusText.Contains("Fixture Undo Quarantine completed", StringComparison.OrdinalIgnoreCase)
                && window.CurrentStatusText.Contains("Rescan", StringComparison.OrdinalIgnoreCase),
                "Fixture undo status should report completion and stale scan state.");
            Assert(
                window.QuarantinePreviewTextValue.Contains("Fixture Undo Quarantine result", StringComparison.OrdinalIgnoreCase)
                && window.QuarantinePreviewTextValue.Contains("Current scan and review rows are stale", StringComparison.OrdinalIgnoreCase),
                "Preview pane should be replaced with undo result and stale-state wording.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Undo result:", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("Fixture Undo Quarantine has restored", StringComparison.OrdinalIgnoreCase),
                "Execution gate should retain undo evidence after undo.");
        }
        finally
        {
            window.Close();
        }
    }

    public void MainWindowDiscoversQuarantineManifestsReadOnly()
    {
        using var fixture = SmokeFixture.Create();
        var setupWindow = new MainWindow(fixture.RootPath);
        var customQuarantineRoot = Path.GetFullPath(Path.Combine(fixture.RootPath, "discovery-quarantine-root"));
        string manifestPath;
        string quarantinePath;
        string originalPath;

        try
        {
            RunDispatcherTask(() => setupWindow.RunStorageScanForCurrentScopeAsync());

            var installer = setupWindow.DisplayedRows.Single(row =>
                row.FullPath.EndsWith(@"Downloads\old-installer.msi", StringComparison.OrdinalIgnoreCase));
            originalPath = installer.FullPath;
            Assert(setupWindow.SelectDisplayedPath(installer.FullPath), "Fixture installer should be selectable for discovery setup.");
            setupWindow.AddSelectedPathToReviewShortlist();
            setupWindow.SetQuarantineRootForPreview(customQuarantineRoot);
            setupWindow.PreviewQuarantineForReviewShortlist();
            setupWindow.SetQuarantineConfirmationText("QUARANTINE");

            manifestPath = setupWindow.CurrentRestoreManifestPath ?? "";
            quarantinePath = setupWindow.CurrentFirstQuarantinePath ?? "";
            setupWindow.ExecuteQuarantineForCurrentPreview();

            Assert(File.Exists(manifestPath), "Discovery setup should persist a Restore Manifest.");
            Assert(File.Exists(quarantinePath), "Discovery setup should move the synthetic file into quarantine.");
            Assert(!File.Exists(originalPath), "Discovery setup should leave the original path moved.");
        }
        finally
        {
            setupWindow.Close();
        }

        var discoveryWindow = new MainWindow(fixture.RootPath);
        try
        {
            discoveryWindow.SetQuarantineRootForPreview(customQuarantineRoot);
            Assert(discoveryWindow.CanDiscoverQuarantineManifests, "Read-only manifest discovery should be available without scanning.");
            Assert(!discoveryWindow.CanUndoQuarantine, "Restore Manifest discovery should not expose broad Undo Quarantine.");

            discoveryWindow.DiscoverQuarantineManifestsForCurrentRoot();
            var discoveryText = discoveryWindow.QuarantineManifestDiscoveryTextValue;

            Assert(
                discoveryWindow.CurrentStatusText.Contains("Quarantine Manifest Discovery completed", StringComparison.OrdinalIgnoreCase)
                && discoveryWindow.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase),
                "Discovery status should report a read-only result.");
            Assert(
                discoveryText.Contains("Quarantine Manifest Discovery: read-only", StringComparison.OrdinalIgnoreCase)
                && discoveryText.Contains("Discovered manifests: 1", StringComparison.OrdinalIgnoreCase)
                && discoveryText.Contains(manifestPath, StringComparison.OrdinalIgnoreCase),
                "Discovery pane should show the persisted manifest summary. Text: " + discoveryText);
            Assert(
                discoveryText.Contains("No all-manifest restore action is available", StringComparison.OrdinalIgnoreCase)
                && discoveryText.Contains("selected manifest readiness", StringComparison.OrdinalIgnoreCase)
                && discoveryText.Contains("selected restore gate", StringComparison.OrdinalIgnoreCase),
                "Discovery pane should not imply an all-manifest restore action is available.");
            Assert(discoveryWindow.DiscoveredRestoreManifestCount == 1, "Discovery should populate one selectable Restore Manifest.");
            Assert(discoveryWindow.CanSelectDiscoveredRestoreManifest, "Discovery should enable Restore Manifest selection when a manifest exists.");
            Assert(discoveryWindow.CanPreviewSelectedRestoreManifestReadiness, "Discovery should enable selected Restore Manifest readiness preview when a manifest is selected.");
            Assert(
                discoveryWindow.PreviewSelectedRestoreManifestReadinessButtonText == "Preview selected manifest readiness",
                "Enabled selected readiness button should name the selected manifest.");
            Assert(
                discoveryWindow.PreviewSelectedRestoreManifestReadinessButtonToolTipValue.Contains("selected Restore Manifest only", StringComparison.OrdinalIgnoreCase)
                && discoveryWindow.PreviewSelectedRestoreManifestReadinessButtonToolTipValue.Contains("not restore approval", StringComparison.OrdinalIgnoreCase),
                "Enabled selected readiness tooltip should preserve selected-only approval-boundary wording.");
            Assert(discoveryWindow.SelectedRestoreManifestPath == manifestPath, "Discovery should select the newest discovered Restore Manifest by default.");
            Assert(discoveryWindow.SelectDiscoveredRestoreManifestByPath(manifestPath), "Persisted Restore Manifest should be selectable by path.");
            Assert(File.Exists(quarantinePath), "Discovery should not move quarantined files.");
            Assert(!File.Exists(originalPath), "Discovery should not restore original paths.");

            discoveryWindow.PreviewSelectedRestoreManifestReadiness();
            var selectedReviewText = discoveryWindow.SelectedRestoreManifestReviewTextValue;

            Assert(
                discoveryWindow.CurrentStatusText.Contains("Selected Restore Manifest Review completed", StringComparison.OrdinalIgnoreCase)
                && discoveryWindow.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase),
                "Selected Restore Manifest Review status should report a read-only result.");
            Assert(
                selectedReviewText.Contains("Selected Restore Manifest Review: read-only", StringComparison.OrdinalIgnoreCase)
                && selectedReviewText.Contains("Selected manifest: " + manifestPath, StringComparison.OrdinalIgnoreCase)
                && selectedReviewText.Contains("Restore readiness row | Restorable", StringComparison.OrdinalIgnoreCase)
                && selectedReviewText.Contains("readiness evidence only", StringComparison.OrdinalIgnoreCase)
                && selectedReviewText.Contains("selected restore gate", StringComparison.OrdinalIgnoreCase),
                "Selected Restore Manifest Review pane should show only selected manifest readiness evidence. Text: " + selectedReviewText);
            Assert(discoveryWindow.CanPreviewSelectedRestoreGate, "Selected manifest readiness should enable selected restore gate preview.");
            Assert(!discoveryWindow.CanEnterSelectedRestoreConfirmation, "Selected restore confirmation should stay disabled until gate preview exists.");
            Assert(File.Exists(quarantinePath), "Selected Restore Manifest Review should not move quarantined files.");
            Assert(!File.Exists(originalPath), "Selected Restore Manifest Review should not restore original paths.");

            discoveryWindow.PreviewSelectedRestoreGateForCurrentSelection();
            var selectedGateText = discoveryWindow.SelectedRestoreExecutionGateTextValue;

            Assert(
                discoveryWindow.CurrentStatusText.Contains("Selected Restore Confirmation Draft completed", StringComparison.OrdinalIgnoreCase)
                && discoveryWindow.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase),
                "Selected restore gate status should report a read-only result.");
            Assert(discoveryWindow.CanEnterSelectedRestoreConfirmation, "Selected restore confirmation should become editable after gate preview.");
            Assert(!discoveryWindow.CanExecuteSelectedRestore, "Selected restore execution should stay closed before exact RESTORE.");
            Assert(
                selectedGateText.Contains("Selected Restore Confirmation Draft:", StringComparison.OrdinalIgnoreCase)
                && selectedGateText.Contains("Required confirmation text: RESTORE", StringComparison.OrdinalIgnoreCase)
                && !selectedGateText.Contains("Required future text", StringComparison.OrdinalIgnoreCase)
                && selectedGateText.Contains("Selected Restore Execution Gate: read-only", StringComparison.OrdinalIgnoreCase)
                && selectedGateText.Contains("Execution implemented: yes", StringComparison.OrdinalIgnoreCase)
                && selectedGateText.Contains("Execution scope status:", StringComparison.OrdinalIgnoreCase)
                && selectedGateText.Contains("Fixture-only selected restore is available", StringComparison.OrdinalIgnoreCase)
                && selectedGateText.Contains("Approval boundary:", StringComparison.OrdinalIgnoreCase)
                && selectedGateText.Contains("not restore approval", StringComparison.OrdinalIgnoreCase)
                && selectedGateText.Contains("only fixture selected restore", StringComparison.OrdinalIgnoreCase)
                && selectedGateText.Contains("Can execute: no", StringComparison.OrdinalIgnoreCase)
                && selectedGateText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase),
                "Selected restore gate pane should show fixture confirmation evidence before exact RESTORE. Text: " + selectedGateText);
            discoveryWindow.SetSelectedRestoreConfirmationText("NOPE");
            Assert(
                discoveryWindow.SelectedRestoreExecutionGateTextValue.Contains("Entered confirmation matches: no", StringComparison.OrdinalIgnoreCase),
                "Wrong selected restore confirmation text should not match.");
            Assert(!discoveryWindow.CanExecuteSelectedRestore, "Wrong selected restore confirmation text should not enable selected restore.");
            discoveryWindow.SetSelectedRestoreConfirmationText("RESTORE");
            var matchedSelectedGateText = discoveryWindow.SelectedRestoreExecutionGateTextValue;
            Assert(
                matchedSelectedGateText.Contains("Entered confirmation matches: yes", StringComparison.OrdinalIgnoreCase)
                && matchedSelectedGateText.Contains("Execution implemented: yes", StringComparison.OrdinalIgnoreCase)
                && matchedSelectedGateText.Contains("Can execute: yes", StringComparison.OrdinalIgnoreCase),
                "Exact RESTORE should open selected fixture restore execution. Text: " + matchedSelectedGateText);
            Assert(discoveryWindow.CanExecuteSelectedRestore, "Exact RESTORE should enable selected fixture restore execution.");
            Assert(
                discoveryWindow.ExecuteSelectedRestoreButtonToolTipValue.Contains("Fixture selected restore only", StringComparison.OrdinalIgnoreCase)
                && discoveryWindow.ExecuteSelectedRestoreButtonToolTipValue.Contains("selected manifest readiness", StringComparison.OrdinalIgnoreCase)
                && discoveryWindow.ExecuteSelectedRestoreButtonToolTipValue.Contains("exact RESTORE", StringComparison.OrdinalIgnoreCase),
                "Enabled selected fixture restore tooltip should still explain the fixture-only gate.");
            Assert(File.Exists(quarantinePath), "Selected restore gate should not move quarantined files.");
            Assert(!File.Exists(originalPath), "Selected restore gate should not restore original paths.");

            discoveryWindow.PreviewRestoreReadinessForCurrentRoot();
            var readinessText = discoveryWindow.RestoreReadinessPreviewTextValue;

            Assert(
                discoveryWindow.CurrentStatusText.Contains("Restore Readiness Preview completed", StringComparison.OrdinalIgnoreCase)
                && discoveryWindow.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase),
                "Restore readiness status should report a read-only result.");
            Assert(
                readinessText.Contains("Restore Readiness Preview: read-only", StringComparison.OrdinalIgnoreCase)
                && readinessText.Contains("Restorable entries: 1", StringComparison.OrdinalIgnoreCase)
                && readinessText.Contains("Restore readiness row | Restorable", StringComparison.OrdinalIgnoreCase)
                && readinessText.Contains(manifestPath, StringComparison.OrdinalIgnoreCase),
                "Restore readiness pane should show restorable Restore Manifest evidence. Text: " + readinessText);
            Assert(
                readinessText.Contains("No all-manifest restore action is available", StringComparison.OrdinalIgnoreCase)
                && readinessText.Contains("selected manifest readiness", StringComparison.OrdinalIgnoreCase)
                && readinessText.Contains("selected restore gate", StringComparison.OrdinalIgnoreCase),
                "Restore readiness pane should not imply an all-manifest restore action is available.");
            Assert(File.Exists(quarantinePath), "Restore readiness preview should not move quarantined files.");
            Assert(!File.Exists(originalPath), "Restore readiness preview should not restore original paths.");

            discoveryWindow.ExecuteSelectedRestoreForCurrentSelection();

            Assert(!discoveryWindow.CanExecuteSelectedRestore, "Selected fixture restore should disable after the execution attempt.");
            Assert(!discoveryWindow.CanEnterSelectedRestoreConfirmation, "Selected restore confirmation should disable after execution attempt.");
            Assert(File.Exists(originalPath), "Selected fixture restore should restore the original fixture file.");
            Assert(!File.Exists(quarantinePath), "Selected fixture restore should move the file out of quarantine.");
            Assert(File.Exists(manifestPath), "Selected fixture restore should keep the Restore Manifest for recovery evidence.");
            Assert(
                discoveryWindow.CurrentStatusText.Contains("Fixture Selected Restore completed", StringComparison.OrdinalIgnoreCase)
                && discoveryWindow.CurrentStatusText.Contains("Rediscover", StringComparison.OrdinalIgnoreCase),
                "Selected fixture restore status should report completion and stale discovery state.");
            Assert(
                discoveryWindow.SelectedRestoreExecutionGateTextValue.Contains("Selected restore result: Restored 1", StringComparison.OrdinalIgnoreCase)
                && discoveryWindow.SelectedRestoreExecutionGateTextValue.Contains("Current scan, discovery, and readiness rows are stale", StringComparison.OrdinalIgnoreCase)
                && discoveryWindow.SelectedRestoreExecutionGateTextValue.Contains("Selected restore row | Restored", StringComparison.OrdinalIgnoreCase),
                "Selected restore gate pane should show fixture restore result evidence.");
        }
        finally
        {
            discoveryWindow.Close();
        }
    }

    public void MainWindowKeepsQuarantineExecutionUnavailableForCustomScope()
    {
        using var fixture = SmokeFixture.CreateCustomScope();
        var window = new MainWindow(fixture.RootPath);
        try
        {
            RunDispatcherTask(() => window.RunStorageScanForCurrentScopeAsync());

            var installer = window.DisplayedRows.Single(row =>
                row.FullPath.EndsWith(@"Downloads\old-installer.msi", StringComparison.OrdinalIgnoreCase));
            Assert(window.SelectDisplayedPath(installer.FullPath), "Custom-scope installer should be selectable.");
            window.AddSelectedPathToReviewShortlist();

            var customQuarantineRoot = Path.GetFullPath(Path.Combine(fixture.RootPath, "..", "custom-scope-quarantine-root"));
            window.SetQuarantineRootForPreview(customQuarantineRoot);
            window.PreviewQuarantineForReviewShortlist();
            window.SetQuarantineConfirmationText("QUARANTINE");

            Assert(!window.CanExecuteQuarantine, "Custom non-fixture scope should keep WPF execution unavailable.");
            Assert(!window.CanUndoQuarantine, "Custom non-fixture scope should not expose fixture undo.");
            Assert(
                window.QuarantinePreviewTextValue.Contains("Execution implemented: no", StringComparison.OrdinalIgnoreCase),
                "Custom-scope preview should state execution is unavailable.");
            Assert(
                window.QuarantinePreviewTextValue.Contains("Execution scope status", StringComparison.OrdinalIgnoreCase)
                && window.QuarantinePreviewTextValue.Contains("Preview only for this Cleanup Scope", StringComparison.OrdinalIgnoreCase)
                && window.QuarantinePreviewTextValue.Contains("real-profile and custom execution remain unavailable", StringComparison.OrdinalIgnoreCase),
                "Custom-scope preview should clearly state preview-only scope status.");
            Assert(
                window.QuarantinePreviewTextValue.Contains("Approval boundary", StringComparison.OrdinalIgnoreCase)
                && window.QuarantinePreviewTextValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.QuarantinePreviewTextValue.Contains("real-profile and custom execution remain unavailable", StringComparison.OrdinalIgnoreCase),
                "Custom-scope preview should keep shortlist/preview separate from cleanup approval.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Execution scope status", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("Preview only for this Cleanup Scope", StringComparison.OrdinalIgnoreCase),
                "Custom-scope gate should clearly state preview-only scope status.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("Approval boundary", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("not cleanup approval", StringComparison.OrdinalIgnoreCase)
                && window.QuarantineExecutionGateTextValue.Contains("real-profile and custom execution remain unavailable", StringComparison.OrdinalIgnoreCase),
                "Custom-scope gate should keep preview-only scopes blocked even after exact confirmation text.");
            Assert(
                window.QuarantineExecutionGateTextValue.Contains("not available for this Cleanup Scope", StringComparison.OrdinalIgnoreCase),
                "Custom-scope gate should explain the scope-specific execution blocker.");
            Assert(File.Exists(installer.FullPath), "Custom-scope execution blocker should leave the source file untouched.");
            Assert(!Directory.Exists(customQuarantineRoot), "Custom-scope execution blocker should not create quarantine folders.");
        }
        finally
        {
            window.Close();
        }
    }

    public void MainWindowKeepsSelectedRestoreUnavailableForCustomScope()
    {
        using var fixture = SmokeFixture.CreateCustomScope();
        var customQuarantineRoot = Path.Combine(fixture.RootPath, "custom-selected-restore-quarantine-root");
        var execution = CreateExecutedRestoreManifest(
            fixture.RootPath,
            customQuarantineRoot,
            @"Downloads\old-installer.msi",
            "custom-selected-restore");
        var entry = execution.RestoreManifest.Entries.Single();
        var window = new MainWindow(fixture.RootPath);
        try
        {
            Assert(execution.Succeeded, "Custom-scope setup should create a moved Restore Manifest for selected restore blocker review.");
            Assert(File.Exists(entry.QuarantinePath), "Custom-scope setup should place the synthetic file in quarantine.");
            Assert(!File.Exists(entry.OriginalPath), "Custom-scope setup should leave the original path moved.");

            window.SetQuarantineRootForPreview(customQuarantineRoot);
            window.DiscoverQuarantineManifestsForCurrentRoot();
            Assert(window.SelectDiscoveredRestoreManifestByPath(execution.RestoreManifest.ManifestPath), "Custom-scope Restore Manifest should be selectable by path.");
            window.PreviewSelectedRestoreManifestReadiness();
            window.PreviewSelectedRestoreGateForCurrentSelection();

            Assert(
                window.SelectedRestoreExecutionGateTextValue.Contains("Execution implemented: no", StringComparison.OrdinalIgnoreCase)
                && window.SelectedRestoreExecutionGateTextValue.Contains("Execution scope status:", StringComparison.OrdinalIgnoreCase)
                && window.SelectedRestoreExecutionGateTextValue.Contains("Preview only for this selected Restore Manifest", StringComparison.OrdinalIgnoreCase)
                && window.SelectedRestoreExecutionGateTextValue.Contains("real-profile and custom selected restore remain unavailable", StringComparison.OrdinalIgnoreCase)
                && window.SelectedRestoreExecutionGateTextValue.Contains("Approval boundary:", StringComparison.OrdinalIgnoreCase)
                && window.SelectedRestoreExecutionGateTextValue.Contains("not restore approval", StringComparison.OrdinalIgnoreCase),
                "Custom-scope selected restore gate should keep execution unavailable with visible scope wording.");
            window.SetSelectedRestoreConfirmationText("RESTORE");
            Assert(
                window.SelectedRestoreExecutionGateTextValue.Contains("Entered confirmation matches: yes", StringComparison.OrdinalIgnoreCase)
                && window.SelectedRestoreExecutionGateTextValue.Contains("Can execute: no", StringComparison.OrdinalIgnoreCase),
                "Custom-scope selected restore should match RESTORE but stay closed.");
            Assert(!window.CanExecuteSelectedRestore, "Custom-scope selected restore execution should remain unavailable.");

            window.ExecuteSelectedRestoreForCurrentSelection();

            Assert(
                window.CurrentStatusText.Contains("gate is not open", StringComparison.OrdinalIgnoreCase)
                && window.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase),
                "Custom-scope selected restore execution attempt should report a closed gate.");
            Assert(File.Exists(entry.QuarantinePath), "Custom-scope selected restore blocker should leave quarantined file in place.");
            Assert(!File.Exists(entry.OriginalPath), "Custom-scope selected restore blocker should not restore original path.");
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
            Assert(
                window.ShortlistSafetyMixTextValue.Contains("Shortlist safety mix: 1 row", StringComparison.OrdinalIgnoreCase)
                && window.ShortlistSafetyMixTextValue.Contains("Quarantine candidates 1", StringComparison.OrdinalIgnoreCase),
                "Shortlist Safety Mix should summarize broad cache parents before preview blockers are evaluated.");
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
            Assert(window.QuarantinePreviewTextValue.Contains(@".cache\codex-runtimes", StringComparison.OrdinalIgnoreCase), "Preview pane should show relative protected descendant evidence.");
            var protectedDescendantReasonLine = window.QuarantinePreviewTextValue
                .Split(Environment.NewLine)
                .Single(line => line.Contains("Contains protected", StringComparison.OrdinalIgnoreCase));
            var reasonText = protectedDescendantReasonLine[(protectedDescendantReasonLine.IndexOf("Reasons:", StringComparison.OrdinalIgnoreCase) + "Reasons:".Length)..];
            Assert(!reasonText.Contains(fixture.RootPath, StringComparison.OrdinalIgnoreCase), "Preview blocker reason should not repeat the absolute fixture scope.");
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

    private static QuarantineExecutionResult CreateExecutedRestoreManifest(
        string cleanupScopePath,
        string quarantineRootPath,
        string relativePath,
        string idSuffix)
    {
        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(cleanupScopePath));
        var review = StorageScanReviewBuilder.Build(result);
        var selectedRow = review.Entries.Single(entry =>
            entry.Entry.FullPath.EndsWith(relativePath, StringComparison.OrdinalIgnoreCase));
        var preview = QuarantinePreviewBuilder.Build([selectedRow], cleanupScopePath, quarantineRootPath);
        var manifestDraft = RestoreManifestDraftBuilder.Build(
            preview,
            new DateTimeOffset(2026, 5, 29, 7, 8, 9, TimeSpan.Zero),
            $"manifest-draft-{idSuffix}");
        var confirmation = QuarantineConfirmationDraftBuilder.Build(
            preview,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 8, 9, 10, TimeSpan.Zero),
            $"confirmation-draft-{idSuffix}",
            isExecutionImplemented: true);
        var actionDraft = QuarantineActionDraftBuilder.Build(
            preview,
            manifestDraft,
            confirmation,
            new DateTimeOffset(2026, 5, 29, 9, 10, 11, TimeSpan.Zero),
            $"quarantine-action-{idSuffix}");
        var manifest = RestoreManifestBuilder.BuildPlanned(
            actionDraft,
            manifestDraft,
            new DateTimeOffset(2026, 5, 29, 10, 11, 12, TimeSpan.Zero),
            $"restore-manifest-{idSuffix}");

        return QuarantineExecutor.Execute(manifest);
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

        fixture.WriteFile(@"Downloads\old-installer.msi", new string('I', 1024 * 1024), now.AddDays(-120));
        fixture.WriteFile(@"AppData\Local\Temp\scratch.tmp", "Synthetic temp file.", now.AddDays(-5));
        fixture.WriteFile(@"AppData\Local\pip\Cache\http-v2\response.body", "Synthetic Python cache.", now.AddDays(-40));
        fixture.WriteFile(@"Documents\important.txt", "Synthetic protected document.", now);
        fixture.WriteFile(@".codex\config.json", "{ \"synthetic\": true }", now);
        fixture.WriteFile(@"Unknown\notes.txt", "Synthetic uncategorized note.", now);

        return fixture;
    }

    public static SmokeFixture CreateCustomScope()
    {
        var root = Path.Combine(
            Environment.CurrentDirectory,
            "custom-scopes",
            "app",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(root);
        var fixture = new SmokeFixture(root, Path.Combine(root, "Unknown", "notes.txt"));
        var now = DateTimeOffset.UtcNow;

        fixture.WriteFile(@"Downloads\old-installer.msi", new string('I', 1024 * 1024), now.AddDays(-120));
        fixture.WriteFile(@"Unknown\notes.txt", "Synthetic custom-scope marker.", now);

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
