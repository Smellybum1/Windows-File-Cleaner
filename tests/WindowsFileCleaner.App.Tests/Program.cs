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
                tests.MainWindowRunsFixtureStorageScanThroughWpfShell();
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
            Assert(window.CurrentStatusText == "Ready", "Launch Cleanup Scope should not trigger a scan.");
            Assert(window.CanStartStorageScan, "Launch Cleanup Scope should still require a user-triggered scan.");
            Assert(!window.CanExportScanCsv, "Launch Cleanup Scope should not create exportable scan data.");
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
