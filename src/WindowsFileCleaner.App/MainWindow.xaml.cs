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
    private readonly StorageReviewShortlist _shortlist = new();
    private CancellationTokenSource? _scanCancellation;
    private StorageScanReview? _currentReview;
    private QuarantinePreview? _currentQuarantinePreview;
    private string? _currentCleanupScopePath;
    private StorageReviewFilter _currentFilter = StorageReviewFilter.All;
    private StorageCategoryFilter _currentCategoryFilter = StorageCategoryFilter.All;
    private StorageEntryRow? _selectedRow;
    private bool _isUpdatingCategoryFilterOptions;

    public MainWindow()
    {
        InitializeComponent();
        ResultsGrid.ItemsSource = Array.Empty<StorageEntryRow>();
        UpdateCategoryFilterOptions();
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
            _currentCleanupScopePath = result.CleanupScopePath;
            _currentFilter = StorageReviewFilter.All;
            _currentCategoryFilter = StorageCategoryFilter.All;
            _shortlist.Clear();
            ClearQuarantinePreview();
            UpdateCategoryFilterOptions();
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
            UpdateShortlistControls();

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

    private void AccessIssuesFilterButton_Click(object sender, RoutedEventArgs e)
    {
        SetFilter(StorageReviewFilter.AccessIssues);
    }

    private void AddToShortlistButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedRow is null)
        {
            return;
        }

        _shortlist.Add(_selectedRow.Entry);
        ClearQuarantinePreview();
        var selectedPath = _selectedRow.FullPath;
        RefreshResults(selectedPath);
        StatusText.Text = $"Added selected path to review shortlist ({_shortlist.Count:N0}). No files were modified.";
    }

    private void RemoveFromShortlistButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedRow is null)
        {
            return;
        }

        _shortlist.Remove(_selectedRow.Entry);
        ClearQuarantinePreview();
        var selectedPath = _selectedRow.FullPath;
        RefreshResults(selectedPath);
        StatusText.Text = $"Removed selected path from review shortlist ({_shortlist.Count:N0}). No files were modified.";
    }

    private void ClearShortlistButton_Click(object sender, RoutedEventArgs e)
    {
        _shortlist.Clear();
        ClearQuarantinePreview();
        RefreshResults();
        StatusText.Text = "Review shortlist cleared. No files were modified.";
    }

    private void PreviewQuarantineButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentReview is null || _currentCleanupScopePath is null || _shortlist.Count == 0)
        {
            return;
        }

        var shortlistedRows = _shortlist.ApplyTo(_currentReview.Entries);
        _currentQuarantinePreview = QuarantinePreviewBuilder.Build(shortlistedRows, _currentCleanupScopePath);
        QuarantinePreviewText.Text = FormatQuarantinePreview(_currentQuarantinePreview);
        StatusText.Text =
            $"Quarantine Preview created: {_currentQuarantinePreview.IncludedCount:N0} included, " +
            $"{_currentQuarantinePreview.BlockedCount:N0} blocked, " +
            $"{_currentQuarantinePreview.RedundantCount:N0} redundant, " +
            $"{_currentQuarantinePreview.IncludedSizeDisplay} previewed. No files were modified.";
    }

    private void CategoryFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isUpdatingCategoryFilterOptions || CategoryFilterBox.SelectedItem is not CategoryFilterOption option)
        {
            return;
        }

        _currentCategoryFilter = option.Filter;
        RefreshResults();
    }

    private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentReview is null)
        {
            return;
        }

        var exportRows = _currentReview.ApplyFilter(_currentFilter, _currentCategoryFilter);
        var dialog = new SaveFileDialog
        {
            Title = "Export Storage Scan CSV",
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            FileName = BuildExportFileName(),
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

    private void ExportShortlistCsvButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentReview is null || _shortlist.Count == 0)
        {
            return;
        }

        var exportRows = _shortlist.ApplyTo(_currentReview.Entries);
        var dialog = new SaveFileDialog
        {
            Title = "Export Review Shortlist CSV",
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            FileName = $"storage-scan-{DateTime.Now:yyyyMMdd-HHmmss}-review-shortlist.csv",
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
            StatusText.Text = $"Exported {exportRows.Count:N0} shortlisted rows to CSV. No scanned files were modified.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Export shortlist failed", MessageBoxButton.OK, MessageBoxImage.Error);
            StatusText.Text = "Review shortlist export failed. No scanned files were modified.";
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
            UpdateShortlistControls();
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
        UpdateShortlistControls();
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
        ExportShortlistCsvButton.IsEnabled = !isScanning && _currentReview is not null && _shortlist.Count > 0;
        ClearShortlistButton.IsEnabled = !isScanning && _shortlist.Count > 0;
        PreviewQuarantineButton.IsEnabled = !isScanning && _currentReview is not null && _shortlist.Count > 0;
        CategoryFilterBox.IsEnabled = !isScanning && _currentReview is not null && CategoryFilterBox.Items.Count > 1;
        UpdateShortlistControls();
    }

    private void SetFilter(StorageReviewFilter filter)
    {
        if (_currentReview is null)
        {
            return;
        }

        _currentFilter = filter;
        RefreshResults();
    }

    private void RefreshResults(string? selectedPath = null)
    {
        if (_currentReview is null)
        {
            return;
        }

        var rows = ApplyCurrentFilter();
        ResultsGrid.ItemsSource = rows;
        UpdateFilterButtons();
        UpdateFilterSummary();
        UpdateShortlistControls();

        if (rows.Length > 0)
        {
            var selectedIndex = selectedPath is null
                ? 0
                : Array.FindIndex(rows, row => row.FullPath.Equals(selectedPath, StringComparison.OrdinalIgnoreCase));
            ResultsGrid.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
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
            AccessIssuesFilterButton.Content = "Access issues";
            ExportCsvButton.IsEnabled = false;
            ExportShortlistCsvButton.IsEnabled = false;
            ClearShortlistButton.IsEnabled = false;
            PreviewQuarantineButton.IsEnabled = false;
            return;
        }

        var summary = _currentReview.Summary;
        AllFilterButton.Content = $"All ({summary.TotalEntries:N0})";
        LikelySafeFilterButton.Content = $"Likely safe ({summary.LikelySafeCount:N0})";
        CautionFilterButton.Content = $"Caution ({summary.CautionCount:N0})";
        HighRiskFilterButton.Content = $"High risk ({summary.HighRiskCount:N0})";
        QuarantineCandidateFilterButton.Content = $"Quarantine candidates ({summary.QuarantineCandidateCount:N0})";
        AccessIssuesFilterButton.Content = $"Access issues ({summary.AccessIssueCount:N0})";
        ExportCsvButton.IsEnabled = true;
        ExportShortlistCsvButton.IsEnabled = _shortlist.Count > 0;
        ClearShortlistButton.IsEnabled = _shortlist.Count > 0;
        PreviewQuarantineButton.IsEnabled = _shortlist.Count > 0;
    }

    private StorageEntryRow[] ApplyCurrentFilter()
    {
        if (_currentReview is null)
        {
            return [];
        }

        return _currentReview
            .ApplyFilter(_currentFilter, _currentCategoryFilter)
            .Take(MaxDisplayedRows)
            .Select(entry => new StorageEntryRow(entry, _shortlist.Contains(entry.Entry)))
            .ToArray();
    }

    private void UpdateCategoryFilterOptions()
    {
        _isUpdatingCategoryFilterOptions = true;
        try
        {
            if (_currentReview is null)
            {
                CategoryFilterBox.ItemsSource = new[]
                {
                    new CategoryFilterOption(StorageCategoryFilter.All, "All categories", "all-categories")
                };
                CategoryFilterBox.SelectedIndex = 0;
                CategoryFilterBox.IsEnabled = false;
                return;
            }

            var options = new List<CategoryFilterOption>
            {
                new(StorageCategoryFilter.All, "All categories", "all-categories")
            };

            var noCategoryRows = _currentReview.ApplyFilter(StorageReviewFilter.All, StorageCategoryFilter.NoCategory);
            if (noCategoryRows.Count > 0)
            {
                var largestNoCategoryBytes = noCategoryRows.Select(row => row.Entry.SizeBytes).DefaultIfEmpty(0).Max();
                options.Add(new CategoryFilterOption(
                    StorageCategoryFilter.NoCategory,
                    $"No category ({noCategoryRows.Count:N0}, largest {ByteSizeFormatter.Format(largestNoCategoryBytes)})",
                    "no-category"));
            }

            options.AddRange(_currentReview.CategorySummaries.Select(summary =>
                new CategoryFilterOption(
                    StorageCategoryFilter.ForCategory(summary.Category),
                    $"{FormatCategory(summary.Category)} ({summary.Count:N0}, largest {ByteSizeFormatter.Format(summary.LargestEntryBytes)})",
                    FormatFileNameCategory(summary.Category))));

            CategoryFilterBox.ItemsSource = options;
            CategoryFilterBox.SelectedIndex = 0;
            CategoryFilterBox.IsEnabled = options.Count > 1;
        }
        finally
        {
            _isUpdatingCategoryFilterOptions = false;
        }
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
        var categoryLabel = _currentCategoryFilter.Kind == StorageCategoryFilterKind.All ? "" : $" + {FormatCategoryFilter(_currentCategoryFilter)}";
        FilterSummaryText.Text = $"{FormatFilter(_currentFilter)}{categoryLabel}: {rows.Length:N0} shown, largest row {ByteSizeFormatter.Format(largestShownBytes)}, shortlist {_shortlist.Count:N0}";
    }

    private void UpdateShortlistControls()
    {
        var hasSelectedRow = _selectedRow is not null;
        var isShortlisted = hasSelectedRow && _shortlist.Contains(_selectedRow!.Entry);
        AddToShortlistButton.IsEnabled = hasSelectedRow && !isShortlisted && ScanButton.IsEnabled;
        RemoveFromShortlistButton.IsEnabled = hasSelectedRow && isShortlisted && ScanButton.IsEnabled;

        var hasShortlist = _shortlist.Count > 0;
        ExportShortlistCsvButton.IsEnabled = _currentReview is not null && hasShortlist && ScanButton.IsEnabled;
        ClearShortlistButton.IsEnabled = hasShortlist && ScanButton.IsEnabled;
        PreviewQuarantineButton.IsEnabled = _currentReview is not null && hasShortlist && ScanButton.IsEnabled;
    }

    private void ClearQuarantinePreview()
    {
        _currentQuarantinePreview = null;
        QuarantinePreviewText.Text = "Preview appears after using Preview quarantine.";
    }

    private static string FormatQuarantinePreview(QuarantinePreview preview)
    {
        const int maxRows = 12;
        var lines = new List<string>
        {
            $"Default root: {preview.QuarantineRootPath}",
            $"Included: {preview.IncludedCount:N0} | Blocked: {preview.BlockedCount:N0} | Redundant: {preview.RedundantCount:N0}",
            $"Previewed size: {preview.IncludedSizeDisplay}",
            "No files were modified."
        };

        foreach (var entry in preview.Entries.Take(maxRows))
        {
            var details = entry.Disposition == QuarantinePreviewDisposition.Included
                ? $"{entry.SourcePath} -> {entry.DestinationPath}"
                : $"{entry.SourcePath} | {string.Join("; ", entry.Reasons)}";
            lines.Add($"{FormatPreviewDisposition(entry.Disposition)} | {entry.SizeDisplay} | {details}");
        }

        if (preview.Entries.Count > maxRows)
        {
            lines.Add($"... {preview.Entries.Count - maxRows:N0} more shortlisted rows not shown in this pane.");
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static string FormatPreviewDisposition(QuarantinePreviewDisposition disposition)
    {
        return disposition switch
        {
            QuarantinePreviewDisposition.Included => "Included",
            QuarantinePreviewDisposition.Blocked => "Blocked",
            QuarantinePreviewDisposition.Redundant => "Redundant",
            _ => disposition.ToString()
        };
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
            $"Quarantine candidates {summary.QuarantineCandidateCount:N0} (largest {ByteSizeFormatter.Format(summary.QuarantineCandidateLargestEntryBytes)}) | " +
            $"Access issues {summary.AccessIssueCount:N0}";
    }

    private static string FormatFilter(StorageReviewFilter filter)
    {
        return filter switch
        {
            StorageReviewFilter.LikelySafe => "Likely safe",
            StorageReviewFilter.Caution => "Caution",
            StorageReviewFilter.HighRisk => "High risk",
            StorageReviewFilter.QuarantineCandidates => "Quarantine candidates",
            StorageReviewFilter.AccessIssues => "Access issues",
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
            StorageReviewFilter.AccessIssues => "access-issues",
            _ => "all"
        };
    }

    private string BuildExportFileName()
    {
        var categorySegment = CategoryFilterBox.SelectedItem is CategoryFilterOption option
            ? option.FileNameSegment
            : "all-categories";
        return $"storage-scan-{DateTime.Now:yyyyMMdd-HHmmss}-{FormatFileNameFilter(_currentFilter)}-{categorySegment}.csv";
    }

    private static string FormatFileNameCategory(BloatCategory category)
    {
        return FormatCategory(category)
            .ToLowerInvariant()
            .Replace(" ", "-", StringComparison.Ordinal);
    }

    private static string FormatCategoryFilter(StorageCategoryFilter filter)
    {
        return filter.Kind switch
        {
            StorageCategoryFilterKind.Category when filter.Category is not null => FormatCategory(filter.Category.Value),
            StorageCategoryFilterKind.NoCategory => "No category",
            _ => "All categories"
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
