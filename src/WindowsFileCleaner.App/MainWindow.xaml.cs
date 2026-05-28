using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using WindowsFileCleaner.Core;

namespace WindowsFileCleaner.App;

public partial class MainWindow : Window
{
    private const int MaxDisplayedRows = 2000;
    private CancellationTokenSource? _scanCancellation;
    private StorageScanReview? _currentReview;
    private StorageReviewFilter _currentFilter = StorageReviewFilter.All;
    private StorageEntryRow? _selectedRow;

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
            UpdateReviewMix();

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

    private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentReview is null)
        {
            return;
        }

        var exportRows = _currentReview.ApplyFilter(_currentFilter);
        var dialog = new SaveFileDialog
        {
            Title = "Export Storage Scan CSV",
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            FileName = $"storage-scan-{DateTime.Now:yyyyMMdd-HHmmss}-{FormatFileNameFilter(_currentFilter)}.csv",
            AddExtension = true,
            DefaultExt = ".csv"
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        try
        {
            File.WriteAllText(dialog.FileName, StorageScanCsvExporter.Export(exportRows));
            StatusText.Text = $"Exported {exportRows.Count:N0} rows to CSV. No scanned files were modified.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Export CSV failed", MessageBoxButton.OK, MessageBoxImage.Error);
            StatusText.Text = "CSV export failed. No scanned files were modified.";
        }
    }

    private void ResultsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ResultsGrid.SelectedItem is not StorageEntryRow row)
        {
            _selectedRow = null;
            DetailTitleText.Text = "Select a result";
            DetailPathText.Text = "";
            DetailMetaText.Text = "";
            DetailEvidenceText.Text = "";
            DetailChildrenText.Text = "";
            CopyPathButton.IsEnabled = false;
            OpenInExplorerButton.IsEnabled = false;
            return;
        }

        _selectedRow = row;
        DetailTitleText.Text = row.Entry.Name;
        DetailPathText.Text = row.FullPath;
        DetailMetaText.Text = $"{row.Size} | {row.Type} | {row.Importance} | {row.Recommendation}";
        DetailEvidenceText.Text = string.IsNullOrWhiteSpace(row.Error)
            ? row.Evidence
            : $"{row.Evidence}\n\nAccess issue: {row.Error}";
        DetailChildrenText.Text = FormatChildSummary(row.Entry);
        CopyPathButton.IsEnabled = true;
        OpenInExplorerButton.IsEnabled = true;
    }

    private void CopyPathButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedRow is null)
        {
            return;
        }

        var plan = PathInspectionPlanBuilder.Build(_selectedRow.Entry);
        Clipboard.SetText(plan.PathToCopy);
        StatusText.Text = "Selected path copied. No files were modified.";
    }

    private void OpenInExplorerButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedRow is null)
        {
            return;
        }

        try
        {
            var plan = PathInspectionPlanBuilder.Build(_selectedRow.Entry);
            Process.Start(new ProcessStartInfo
            {
                FileName = plan.ExplorerFileName,
                Arguments = plan.ExplorerArguments,
                UseShellExecute = true
            });
            StatusText.Text = "Opened selected path in File Explorer. No files were modified.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Open in Explorer failed", MessageBoxButton.OK, MessageBoxImage.Error);
            StatusText.Text = "Could not open selected path. No files were modified.";
        }
    }

    private void SetScanningState(bool isScanning)
    {
        ScanButton.IsEnabled = !isScanning;
        CancelButton.IsEnabled = isScanning;
        ScopePathBox.IsEnabled = !isScanning;
        ExportCsvButton.IsEnabled = !isScanning && _currentReview is not null;
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
        ExportCsvButton.IsEnabled = true;
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
        var largestShownBytes = rows.Select(row => row.SizeBytes).DefaultIfEmpty(0).Max();
        FilterSummaryText.Text = $"{FormatFilter(_currentFilter)}: {rows.Length:N0} shown, largest row {ByteSizeFormatter.Format(largestShownBytes)}";
    }

    private void UpdateReviewMix()
    {
        if (_currentReview is null)
        {
            ReviewMixText.Text = "Review mix appears after a scan.";
            return;
        }

        var summary = _currentReview.Summary;
        ReviewMixText.Text =
            $"Review mix: " +
            $"Likely safe {summary.LikelySafeCount:N0} (largest {ByteSizeFormatter.Format(summary.LikelySafeLargestEntryBytes)}) | " +
            $"Caution {summary.CautionCount:N0} (largest {ByteSizeFormatter.Format(summary.CautionLargestEntryBytes)}) | " +
            $"High risk {summary.HighRiskCount:N0} (largest {ByteSizeFormatter.Format(summary.HighRiskLargestEntryBytes)}) | " +
            $"Quarantine candidates {summary.QuarantineCandidateCount:N0} (largest {ByteSizeFormatter.Format(summary.QuarantineCandidateLargestEntryBytes)})";
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

    private static string FormatFileNameFilter(StorageReviewFilter filter)
    {
        return filter switch
        {
            StorageReviewFilter.LikelySafe => "likely-safe",
            StorageReviewFilter.Caution => "caution",
            StorageReviewFilter.HighRisk => "high-risk",
            StorageReviewFilter.QuarantineCandidates => "quarantine-candidates",
            _ => "all"
        };
    }

    private static string FormatChildSummary(StorageEntry entry)
    {
        var children = StorageChildSummaryBuilder.Build(entry);
        if (children.Count == 0)
        {
            return entry.IsDirectory
                ? "No immediate children were found or the folder could not be expanded."
                : "Files do not have immediate children.";
        }

        return string.Join(
            Environment.NewLine,
            children.Select(child =>
                $"{child.Name} | {child.SizeDisplay} | {FormatImportance(child.ImportanceRating)} | {FormatRecommendation(child.DeletionRecommendation)} | {FormatCategories(child.BloatCategories)}"));
    }

    private static string FormatImportance(ImportanceRating rating)
    {
        return rating switch
        {
            ImportanceRating.LikelySafe => "Likely safe",
            ImportanceRating.Caution => "Caution",
            ImportanceRating.HighRisk => "High risk",
            _ => rating.ToString()
        };
    }

    private static string FormatRecommendation(DeletionRecommendation recommendation)
    {
        return recommendation switch
        {
            DeletionRecommendation.Keep => "Keep",
            DeletionRecommendation.Inspect => "Inspect",
            DeletionRecommendation.QuarantineCandidate => "Quarantine candidate",
            DeletionRecommendation.DeleteLater => "Delete later",
            _ => recommendation.ToString()
        };
    }

    private static string FormatCategories(IReadOnlyList<BloatCategory> categories)
    {
        return categories.Count == 0
            ? "None"
            : string.Join(", ", categories.Select(FormatCategory));
    }

    private static string FormatCategory(BloatCategory category)
    {
        return category switch
        {
            BloatCategory.Unknown => "Unknown",
            BloatCategory.ProfileContainer => "Profile container",
            BloatCategory.ApplicationDataArea => "AppData area",
            BloatCategory.BrowserData => "Browser data",
            BloatCategory.OldDownload => "Old download",
            BloatCategory.TemporaryFolder => "Temporary folder",
            BloatCategory.InstallerCache => "Installer cache",
            BloatCategory.AppCache => "App cache",
            BloatCategory.GpuShaderCache => "GPU shader cache",
            BloatCategory.DuplicateFileCandidate => "Duplicate file candidate",
            BloatCategory.OldGameFile => "Old game file",
            BloatCategory.NodePackageCache => "Node package cache",
            BloatCategory.PythonPackageCache => "Python package cache",
            BloatCategory.WindowsAppLeftover => "Windows app leftover",
            BloatCategory.ProtectedLocation => "Protected location",
            BloatCategory.ReparsePoint => "Reparse point",
            BloatCategory.AccessIssue => "Access issue",
            _ => category.ToString()
        };
    }
}
