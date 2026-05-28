using System.IO;
using System.Windows;
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

    private static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
