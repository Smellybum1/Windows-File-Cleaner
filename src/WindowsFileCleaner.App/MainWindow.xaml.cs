using System.Windows;
using System.Windows.Controls;
using WindowsFileCleaner.Core;

namespace WindowsFileCleaner.App;

public partial class MainWindow : Window
{
    private const int MaxDisplayedRows = 2000;
    private CancellationTokenSource? _scanCancellation;

    public MainWindow()
    {
        InitializeComponent();
        ResultsGrid.ItemsSource = Array.Empty<StorageEntryRow>();
    }

    private async void ScanButton_Click(object sender, RoutedEventArgs e)
    {
        var scopePath = ScopePathBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(scopePath))
        {
            MessageBox.Show(this, "Choose a Cleanup Scope before scanning.", "Storage Scan", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        SetScanningState(isScanning: true);
        _scanCancellation = new CancellationTokenSource();
        var cancellationToken = _scanCancellation.Token;

        try
        {
            StatusText.Text = $"Scanning {scopePath}...";
            var options = new StorageScanOptions(scopePath);
            var scanner = new StorageScanner();

            var result = await Task.Run(() => scanner.Scan(options, cancellationToken), cancellationToken);
            var rows = Flatten(result.Root)
                .OrderByDescending(row => row.SizeBytes)
                .ThenBy(row => row.FullPath, StringComparer.OrdinalIgnoreCase)
                .Take(MaxDisplayedRows)
                .ToArray();

            ResultsGrid.ItemsSource = rows;
            TotalSizeText.Text = result.TotalSizeDisplay;
            FolderCountText.Text = result.FolderCount.ToString("N0");
            FileCountText.Text = result.FileCount.ToString("N0");
            AccessIssueCountText.Text = result.InaccessibleCount.ToString("N0");
            StatusText.Text = $"Storage Scan completed. Showing {rows.Length:N0} largest paths. No files were modified.";

            if (rows.Length > 0)
            {
                ResultsGrid.SelectedIndex = 0;
            }
        }
        catch (OperationCanceledException)
        {
            StatusText.Text = "Storage Scan canceled. No files were modified.";
        }
        catch (Exception ex)
        {
            StatusText.Text = "Storage Scan failed. No files were modified.";
            MessageBox.Show(this, ex.Message, "Storage Scan failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            _scanCancellation?.Dispose();
            _scanCancellation = null;
            SetScanningState(isScanning: false);
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        _scanCancellation?.Cancel();
    }

    private void ResultsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ResultsGrid.SelectedItem is not StorageEntryRow row)
        {
            DetailTitleText.Text = "Select a result";
            DetailPathText.Text = "";
            DetailMetaText.Text = "";
            DetailEvidenceText.Text = "";
            return;
        }

        DetailTitleText.Text = row.Entry.Name;
        DetailPathText.Text = row.FullPath;
        DetailMetaText.Text = $"{row.Size} | {row.Type} | {row.Importance} | {row.Recommendation}";
        DetailEvidenceText.Text = string.IsNullOrWhiteSpace(row.Error)
            ? row.Evidence
            : $"{row.Evidence}\n\nAccess issue: {row.Error}";
    }

    private void SetScanningState(bool isScanning)
    {
        ScanButton.IsEnabled = !isScanning;
        CancelButton.IsEnabled = isScanning;
        ScopePathBox.IsEnabled = !isScanning;
    }

    private static IEnumerable<StorageEntryRow> Flatten(StorageEntry root)
    {
        return Flatten(root, depth: 0);
    }

    private static IEnumerable<StorageEntryRow> Flatten(StorageEntry entry, int depth)
    {
        yield return new StorageEntryRow(entry, depth);

        foreach (var child in entry.Children.SelectMany(child => Flatten(child, depth + 1)))
        {
            yield return child;
        }
    }
}

