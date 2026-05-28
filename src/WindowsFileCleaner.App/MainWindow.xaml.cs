using System.Windows;
using System.Windows.Controls;
using WindowsFileCleaner.Core;

namespace WindowsFileCleaner.App;

public partial class MainWindow : Window
{
    private const int MaxDisplayedRows = 2000;
    private CancellationTokenSource? _scanCancellation;
    private StorageScanReview? _currentReview;
    private StorageReviewFilter _currentFilter = StorageReviewFilter.All;

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
            _currentReview = StorageScanReviewBuilder.Build(result);
            _currentFilter = StorageReviewFilter.All;
            var rows = ApplyCurrentFilter();

            ResultsGrid.ItemsSource = rows;
            TotalSizeText.Text = result.TotalSizeDisplay;
            FolderCountText.Text = result.FolderCount.ToString("N0");
            FileCountText.Text = result.FileCount.ToString("N0");
            AccessIssueCountText.Text = result.InaccessibleCount.ToString("N0");
            StatusText.Text = $"Storage Scan completed. Showing {rows.Length:N0} paths. No files were modified.";
            UpdateFilterButtons();
            UpdateFilterSummary();

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

    private void AllFilterButton_Click(object sender, RoutedEventArgs e)
    {
        SetFilter(StorageReviewFilter.All);
    }

    private void LikelySafeFilterButton_Click(object sender, RoutedEventArgs e)
    {
        SetFilter(StorageReviewFilter.LikelySafe);
    }

    private void CautionFilterButton_Click(object sender, RoutedEventArgs e)
    {
        SetFilter(StorageReviewFilter.Caution);
    }

    private void HighRiskFilterButton_Click(object sender, RoutedEventArgs e)
    {
        SetFilter(StorageReviewFilter.HighRisk);
    }

    private void QuarantineCandidateFilterButton_Click(object sender, RoutedEventArgs e)
    {
        SetFilter(StorageReviewFilter.QuarantineCandidates);
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

    private void SetFilter(StorageReviewFilter filter)
    {
        if (_currentReview is null)
        {
            return;
        }

        _currentFilter = filter;
        var rows = ApplyCurrentFilter();
        ResultsGrid.ItemsSource = rows;
        UpdateFilterButtons();
        UpdateFilterSummary();

        if (rows.Length > 0)
        {
            ResultsGrid.SelectedIndex = 0;
        }
        else
        {
            ResultsGrid.SelectedItem = null;
        }
    }

    private void UpdateFilterButtons()
    {
        if (_currentReview is null)
        {
            AllFilterButton.Content = "All";
            LikelySafeFilterButton.Content = "Likely safe";
            CautionFilterButton.Content = "Caution";
            HighRiskFilterButton.Content = "High risk";
            QuarantineCandidateFilterButton.Content = "Quarantine candidates";
            return;
        }

        var summary = _currentReview.Summary;
        AllFilterButton.Content = $"All ({summary.TotalEntries:N0})";
        LikelySafeFilterButton.Content = $"Likely safe ({summary.LikelySafeCount:N0})";
        CautionFilterButton.Content = $"Caution ({summary.CautionCount:N0})";
        HighRiskFilterButton.Content = $"High risk ({summary.HighRiskCount:N0})";
        QuarantineCandidateFilterButton.Content = $"Quarantine candidates ({summary.QuarantineCandidateCount:N0})";
    }

    private StorageEntryRow[] ApplyCurrentFilter()
    {
        if (_currentReview is null)
        {
            return [];
        }

        return _currentReview
            .ApplyFilter(_currentFilter)
            .Take(MaxDisplayedRows)
            .Select(entry => new StorageEntryRow(entry))
            .ToArray();
    }

    private void UpdateFilterSummary()
    {
        if (_currentReview is null)
        {
            FilterSummaryText.Text = "No scan loaded";
            return;
        }

        var rows = ApplyCurrentFilter();
        var shownBytes = rows.Sum(row => row.SizeBytes);
        FilterSummaryText.Text = $"{FormatFilter(_currentFilter)}: {rows.Length:N0} shown, {ByteSizeFormatter.Format(shownBytes)}";
    }

    private static string FormatFilter(StorageReviewFilter filter)
    {
        return filter switch
        {
            StorageReviewFilter.LikelySafe => "Likely safe",
            StorageReviewFilter.Caution => "Caution",
            StorageReviewFilter.HighRisk => "High risk",
            StorageReviewFilter.QuarantineCandidates => "Quarantine candidates",
            _ => "All"
        };
    }
}
