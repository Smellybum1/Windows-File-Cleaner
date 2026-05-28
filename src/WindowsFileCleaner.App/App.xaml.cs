using System.Windows;
using WindowsFileCleaner.Core;

namespace WindowsFileCleaner.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        StorageScanLaunchOptions launchOptions;
        try
        {
            launchOptions = StorageScanLaunchOptions.Parse(e.Args);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Windows File Cleaner", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(1);
            return;
        }

        MainWindow = new MainWindow(launchOptions.CleanupScopePath);
        MainWindow.Show();
    }
}
