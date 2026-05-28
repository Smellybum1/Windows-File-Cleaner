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
                tests.MainWindowRunsFixtureReviewInteractionsThroughWpfShell();
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
            Assert(window.CanStartStorageScan, "MainWindow should allow a user-triggered Storage Scan.");
            Assert(!window.CanExportScanCsv, "MainWindow should not allow CSV export before a scan.");
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
            Assert(window.TotalSizeTextValue != "-", "Total size card should be populated after scan.");
            Assert(window.FolderCountTextValue != "-", "Folder count card should be populated after scan.");
            Assert(window.FileCountTextValue != "-", "File count card should be populated after scan.");
            Assert(window.AccessIssueCountTextValue == "0", "Synthetic fixture should scan without access issues.");
            Assert(window.ReviewMixTextValue.Contains("Quarantine candidates", StringComparison.OrdinalIgnoreCase), "Review Mix should summarize quarantine candidates.");
            Assert(window.SafetySummaryTextValue.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Safety Summary should state the read-only boundary.");
            Assert(window.FilterSummaryTextValue.Contains("All:", StringComparison.OrdinalIgnoreCase), "Filter summary should start on the All filter.");

            var rows = window.DisplayedRows;
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
            Assert(File.Exists(fixture.MarkerPath), "Fixture marker file should still exist after the read-only scan.");
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
                window.DetailGuidanceTextValue.Contains("Shortlist after review", StringComparison.OrdinalIgnoreCase),
                "Selecting a quarantine candidate should show review guidance before shortlisting.");
            Assert(
                window.DetailGuidanceTextValue.Contains("not deletion approval", StringComparison.OrdinalIgnoreCase),
                "Selected-row review guidance should not imply deletion approval.");
            Assert(window.CanAddSelectedRowToReviewShortlist, "Selected fixture installer should be addable to Review Shortlist.");
            Assert(!window.CanPreviewQuarantine, "Quarantine Preview should be disabled before a shortlist exists.");

            window.AddSelectedPathToReviewShortlist();
            Assert(window.ReviewShortlistCount == 1, "Adding selected row should update Review Shortlist count.");
            Assert(window.CanRemoveSelectedRowFromReviewShortlist, "Shortlisted row should be removable.");
            Assert(window.CanPreviewQuarantine, "Quarantine Preview should be available after shortlisting a row.");
            Assert(
                window.DisplayedRows.Any(row => row.FullPath == installer.FullPath && row.Shortlist == "Yes"),
                "Shortlisted row should be marked in the WPF grid.");

            window.PreviewQuarantineForReviewShortlist();
            Assert(window.CurrentStatusText.Contains("Quarantine Preview created", StringComparison.OrdinalIgnoreCase), "Preview action should update status text.");
            Assert(window.CurrentStatusText.Contains("No files were modified", StringComparison.OrdinalIgnoreCase), "Preview status should preserve the read-only boundary.");
            Assert(window.CanExportQuarantinePreview, "Quarantine Preview export should be enabled after preview creation.");
            Assert(window.QuarantinePreviewTextValue.Contains("Included: 1", StringComparison.OrdinalIgnoreCase), "Preview pane should show one included fixture row.");
            Assert(window.QuarantinePreviewTextValue.Contains("Restore Manifest Draft", StringComparison.OrdinalIgnoreCase), "Preview pane should show Restore Manifest Draft readiness.");
            Assert(window.QuarantinePreviewTextValue.Contains("Quarantine Confirmation Draft", StringComparison.OrdinalIgnoreCase), "Preview pane should show Quarantine Confirmation Draft readiness.");
            Assert(window.QuarantinePreviewTextValue.Contains("Execution implemented: no", StringComparison.OrdinalIgnoreCase), "Preview pane should state execution is not implemented.");
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
