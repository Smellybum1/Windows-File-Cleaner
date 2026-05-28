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
    private StorageScanSafetySummary? _currentSafetySummary;
    private QuarantinePreview? _currentQuarantinePreview;
    private RestoreManifestDraft? _currentRestoreManifestDraft;
    private QuarantineConfirmationDraft? _currentQuarantineConfirmationDraft;
    private string? _currentCleanupScopePath;
    private StorageReviewFilter _currentFilter = StorageReviewFilter.All;
    private StorageCategoryFilter _currentCategoryFilter = StorageCategoryFilter.All;
    private StorageReviewSearch _currentSearch = StorageReviewSearch.Empty;
    private StorageEntryRow? _selectedRow;
    private bool _isUpdatingCategoryFilterOptions;
    private bool _isUpdatingSearchBox;

    public MainWindow()
        : this(StorageScanOptions.DefaultForCurrentUser().CleanupScopePath)
    {
    }

    public MainWindow(string initialCleanupScopePath)
    {
        InitializeComponent();
        ScopePathBox.Text = initialCleanupScopePath;
        ResultsGrid.ItemsSource = Array.Empty<StorageEntryRow>();
        UpdateCategoryFilterOptions();
        UpdateCleanupScopeSafetyNote();
    }

    public string CurrentCleanupScopePath => ScopePathBox.Text;

    public string CleanupScopeSafetyNoteTextValue => ScopeSafetyNoteText.Text;

    public string CurrentStatusText => StatusText.Text;

    public bool CanStartStorageScan => ScanButton.IsEnabled;

    public bool CanExportScanCsv => ExportCsvButton.IsEnabled;

    public int CurrentScanReportExportRowCount => BuildCurrentScanReportExportRows().Count;

    public IReadOnlyList<string> CurrentScanReportExportPaths => BuildCurrentScanReportExportRows()
        .Select(row => row.Entry.FullPath)
        .ToArray();

    public string CurrentScanReportExportFileName => BuildExportFileName();

    public bool CanUseCategoryFilter => CategoryFilterBox.IsEnabled;

    public int DisplayedRowCount => DisplayedRows.Count;

    public IReadOnlyList<StorageEntryRow> DisplayedRows => ResultsGrid.ItemsSource is IEnumerable<StorageEntryRow> rows
        ? rows.ToArray()
        : [];

    public string TotalSizeTextValue => TotalSizeText.Text;

    public string FolderCountTextValue => FolderCountText.Text;

    public string FileCountTextValue => FileCountText.Text;

    public string AccessIssueCountTextValue => AccessIssueCountText.Text;

    public string ReviewMixTextValue => ReviewMixText.Text;

    public string SafetySummaryTextValue => SafetySummaryText.Text;

    public string FilterSummaryTextValue => FilterSummaryText.Text;

    public string CurrentSearchText => SearchBox.Text;

    public string DetailGuidanceTextValue => DetailGuidanceText.Text;

    public string QuarantinePreviewTextValue => QuarantinePreviewText.Text;

    public string? SelectedRowFullPath => _selectedRow?.FullPath;

    public int ReviewShortlistCount => _shortlist.Count;

    public bool CanAddSelectedRowToReviewShortlist => AddToShortlistButton.IsEnabled;

    public bool CanRemoveSelectedRowFromReviewShortlist => RemoveFromShortlistButton.IsEnabled;

    public bool CanAddShownRowsToReviewShortlist => AddShownToShortlistButton.IsEnabled;

    public bool CanRemoveShownRowsFromReviewShortlist => RemoveShownFromShortlistButton.IsEnabled;

    public bool CanPreviewQuarantine => PreviewQuarantineButton.IsEnabled;

    public bool CanExportQuarantinePreview => ExportQuarantinePreviewButton.IsEnabled;

    public bool ReviewToolbarsUseWrappingLayout =>
        ReviewFilterToolbar is WrapPanel && ReviewActionToolbar is WrapPanel;

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
            await RunStorageScanForCurrentScopeAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Storage Scan failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            _scanCancellation?.Dispose();
            _scanCancellation = null;
            SetScanningState(isScanning: false);
        }
    }

    public async Task RunStorageScanForCurrentScopeAsync(CancellationToken cancellationToken = default)
    {
        var scopePath = ScopePathBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(scopePath))
        {
            throw new InvalidOperationException("Choose a Cleanup Scope before scanning.");
        }

        SetScanningState(isScanning: true);
        _currentSafetySummary = null;
        UpdateSafetySummary();

        try
        {
            StatusText.Text = $"Scanning {scopePath}...";
            var options = new StorageScanOptions(scopePath);
            var scanner = new StorageScanner();

            var result = await Task.Run(() => scanner.Scan(options, cancellationToken), cancellationToken);
            ApplyStorageScanResult(result);
        }
        catch (OperationCanceledException)
        {
            StatusText.Text = "Storage Scan canceled. No files were modified.";
            throw;
        }
        catch
        {
            StatusText.Text = "Storage Scan failed. No files were modified.";
            throw;
        }
        finally
        {
            SetScanningState(isScanning: false);
        }
    }

    private void ApplyStorageScanResult(StorageScanResult result)
    {
        _currentReview = StorageScanReviewBuilder.Build(result);
        _currentSafetySummary = StorageScanSafetySummaryBuilder.Build(result, _currentReview);
        _currentCleanupScopePath = result.CleanupScopePath;
        _currentFilter = StorageReviewFilter.All;
        _currentCategoryFilter = StorageCategoryFilter.All;
        _currentSearch = StorageReviewSearch.Empty;
        _shortlist.Clear();
        ClearQuarantinePreview();
        SetSearchTextSilently("");
        UpdateCategoryFilterOptions();
        var matchedEntries = ApplyCurrentReviewFilters();
        var rows = BuildDisplayedRows(matchedEntries);

        ResultsGrid.ItemsSource = rows;
        TotalSizeText.Text = result.TotalSizeDisplay;
        FolderCountText.Text = result.FolderCount.ToString("N0");
        FileCountText.Text = result.FileCount.ToString("N0");
        AccessIssueCountText.Text = result.InaccessibleCount.ToString("N0");
        StatusText.Text = FormatScanCompletedStatus(rows.Length, matchedEntries.Count);
        UpdateFilterButtons();
        UpdateFilterSummary();
        UpdateReviewMix();
        UpdateSafetySummary();
        UpdateShortlistControls();

        if (rows.Length > 0)
        {
            ResultsGrid.SelectedIndex = 0;
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        _scanCancellation?.Cancel();
    }

    private void ScopePathBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!IsInitialized)
        {
            return;
        }

        UpdateCleanupScopeSafetyNote();
    }

    private void AllFilterButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyStorageReviewFilter(StorageReviewFilter.All);
    }

    private void LikelySafeFilterButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyStorageReviewFilter(StorageReviewFilter.LikelySafe);
    }

    private void CautionFilterButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyStorageReviewFilter(StorageReviewFilter.Caution);
    }

    private void HighRiskFilterButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyStorageReviewFilter(StorageReviewFilter.HighRisk);
    }

    private void QuarantineCandidateFilterButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyStorageReviewFilter(StorageReviewFilter.QuarantineCandidates);
    }

    private void AccessIssuesFilterButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyStorageReviewFilter(StorageReviewFilter.AccessIssues);
    }

    private void SafetyHighRiskButton_Click(object sender, RoutedEventArgs e)
    {
        ApplySafetyReviewShortcut(StorageScanSafetyShortcut.HighRisk);
    }

    private void SafetyProtectedButton_Click(object sender, RoutedEventArgs e)
    {
        ApplySafetyReviewShortcut(StorageScanSafetyShortcut.ProtectedLocations);
    }

    private void SafetyAccessIssuesButton_Click(object sender, RoutedEventArgs e)
    {
        ApplySafetyReviewShortcut(StorageScanSafetyShortcut.AccessIssues);
    }

    private void SafetyReparsePointsButton_Click(object sender, RoutedEventArgs e)
    {
        ApplySafetyReviewShortcut(StorageScanSafetyShortcut.ReparsePoints);
    }

    private void SafetyQuarantineCandidatesButton_Click(object sender, RoutedEventArgs e)
    {
        ApplySafetyReviewShortcut(StorageScanSafetyShortcut.QuarantineCandidates);
    }

    private void SafetyNoCategoryButton_Click(object sender, RoutedEventArgs e)
    {
        ApplySafetyReviewShortcut(StorageScanSafetyShortcut.Uncategorized);
    }

    private void AddToShortlistButton_Click(object sender, RoutedEventArgs e)
    {
        AddSelectedPathToReviewShortlist();
    }

    private void RemoveFromShortlistButton_Click(object sender, RoutedEventArgs e)
    {
        RemoveSelectedPathFromReviewShortlist();
    }

    private void AddShownToShortlistButton_Click(object sender, RoutedEventArgs e)
    {
        AddShownRowsToReviewShortlist();
    }

    private void RemoveShownFromShortlistButton_Click(object sender, RoutedEventArgs e)
    {
        RemoveShownRowsFromReviewShortlist();
    }

    private void ClearShortlistButton_Click(object sender, RoutedEventArgs e)
    {
        ClearReviewShortlist();
    }

    private void PreviewQuarantineButton_Click(object sender, RoutedEventArgs e)
    {
        PreviewQuarantineForReviewShortlist();
    }

    public void AddSelectedPathToReviewShortlist()
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

    public void RemoveSelectedPathFromReviewShortlist()
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

    public void AddShownRowsToReviewShortlist()
    {
        if (_currentReview is null)
        {
            return;
        }

        var rows = DisplayedRows;
        if (rows.Count == 0)
        {
            return;
        }

        var addedCount = _shortlist.AddMany(rows.Select(row => row.Entry));
        if (addedCount == 0)
        {
            StatusText.Text = $"All shown paths are already in Review Shortlist ({_shortlist.Count:N0}). No files were modified.";
            UpdateShortlistControls();
            return;
        }

        ClearQuarantinePreview();
        var selectedPath = _selectedRow?.FullPath;
        RefreshResults(selectedPath);
        StatusText.Text =
            $"Shortlisted {addedCount:N0} shown path(s) ({_shortlist.Count:N0} total). " +
            "Review Shortlist is not cleanup approval. No files were modified.";
    }

    public void RemoveShownRowsFromReviewShortlist()
    {
        if (_currentReview is null)
        {
            return;
        }

        var rows = DisplayedRows;
        if (rows.Count == 0)
        {
            return;
        }

        var removedCount = _shortlist.RemoveMany(rows.Select(row => row.Entry));
        if (removedCount == 0)
        {
            StatusText.Text = $"No shown paths were in Review Shortlist ({_shortlist.Count:N0}). No files were modified.";
            UpdateShortlistControls();
            return;
        }

        ClearQuarantinePreview();
        var selectedPath = _selectedRow?.FullPath;
        RefreshResults(selectedPath);
        StatusText.Text =
            $"Removed {removedCount:N0} shown path(s) from Review Shortlist ({_shortlist.Count:N0} total). " +
            "No files were modified.";
    }

    public void ClearReviewShortlist()
    {
        _shortlist.Clear();
        ClearQuarantinePreview();
        RefreshResults();
        StatusText.Text = "Review shortlist cleared. No files were modified.";
    }

    public void PreviewQuarantineForReviewShortlist()
    {
        if (_currentReview is null || _currentCleanupScopePath is null || _shortlist.Count == 0)
        {
            return;
        }

        var shortlistedRows = _shortlist.ApplyTo(_currentReview.Entries);
        _currentQuarantinePreview = QuarantinePreviewBuilder.Build(shortlistedRows, _currentCleanupScopePath);
        _currentRestoreManifestDraft = RestoreManifestDraftBuilder.Build(
            _currentQuarantinePreview,
            DateTimeOffset.UtcNow,
            BuildDraftId("restore-manifest-draft"));
        _currentQuarantineConfirmationDraft = QuarantineConfirmationDraftBuilder.Build(
            _currentQuarantinePreview,
            _currentRestoreManifestDraft,
            DateTimeOffset.UtcNow,
            BuildDraftId("quarantine-confirmation-draft"));

        QuarantinePreviewText.Text = FormatQuarantinePreview(
            _currentQuarantinePreview,
            _currentRestoreManifestDraft,
            _currentQuarantineConfirmationDraft);
        ExportQuarantinePreviewButton.IsEnabled = ScanButton.IsEnabled;
        var blockerSummary = _currentQuarantineConfirmationDraft.HasDataBlockers
            ? $"{_currentQuarantineConfirmationDraft.Blockers.Count:N0} readiness blocker(s)"
            : "no readiness blockers";
        StatusText.Text =
            $"Quarantine Preview created: {_currentQuarantinePreview.IncludedCount:N0} included, " +
            $"{_currentQuarantinePreview.BlockedCount:N0} blocked, " +
            $"{_currentQuarantinePreview.RedundantCount:N0} redundant, " +
            $"{_currentQuarantinePreview.IncludedSizeDisplay} previewed, {blockerSummary}. No files were modified.";
    }

    private void ExportQuarantinePreviewButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentQuarantinePreview is null)
        {
            return;
        }

        var dialog = new SaveFileDialog
        {
            Title = "Export Quarantine Preview CSV",
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            FileName = $"storage-scan-{DateTime.Now:yyyyMMdd-HHmmss}-quarantine-preview.csv",
            AddExtension = true,
            DefaultExt = ".csv"
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        try
        {
            File.WriteAllText(dialog.FileName, QuarantinePreviewCsvExporter.Export(_currentQuarantinePreview));
            StatusText.Text = $"Exported {_currentQuarantinePreview.Entries.Count:N0} preview rows to CSV. No scanned files were modified.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Export preview failed", MessageBoxButton.OK, MessageBoxImage.Error);
            StatusText.Text = "Quarantine Preview export failed. No scanned files were modified.";
        }
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

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isUpdatingSearchBox || _currentReview is null)
        {
            return;
        }

        _currentSearch = StorageReviewSearch.FromText(SearchBox.Text);
        RefreshResults();
    }

    private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyStorageReviewSearch("");
    }

    private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentReview is null)
        {
            return;
        }

        var exportRows = BuildCurrentScanReportExportRows();
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

    private IReadOnlyList<StorageReviewEntry> BuildCurrentScanReportExportRows()
    {
        return _currentReview?.ApplyFilter(_currentFilter, _currentCategoryFilter, _currentSearch) ?? [];
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
            DetailGuidanceText.Text = "";
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
        DetailGuidanceText.Text = FormatSelectedPathReviewGuidance(row.Entry);
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
        UpdateCleanupScopeSafetyNote();
        ExportCsvButton.IsEnabled = !isScanning && _currentReview is not null;
        ExportShortlistCsvButton.IsEnabled = !isScanning && _currentReview is not null && _shortlist.Count > 0;
        ClearShortlistButton.IsEnabled = !isScanning && _shortlist.Count > 0;
        PreviewQuarantineButton.IsEnabled = !isScanning && _currentReview is not null && _shortlist.Count > 0;
        ExportQuarantinePreviewButton.IsEnabled = !isScanning && _currentQuarantinePreview is not null;
        CategoryFilterBox.IsEnabled = !isScanning && _currentReview is not null && CategoryFilterBox.Items.Count > 1;
        SearchBox.IsEnabled = !isScanning && _currentReview is not null;
        ClearSearchButton.IsEnabled = !isScanning && _currentReview is not null && _currentSearch.IsActive;
        UpdateShortlistControls();
        UpdateSafetyShortcutButtons();
    }

    private void UpdateCleanupScopeSafetyNote()
    {
        if (ScopeSafetyNoteText is null)
        {
            return;
        }

        var note = CleanupScopeSafetyNoteBuilder.Build(ScopePathBox.Text);
        ScopeSafetyNoteText.Text = $"{note.Label}: {note.Message}";
    }

    public void ApplyStorageReviewFilter(StorageReviewFilter filter)
    {
        if (_currentReview is null)
        {
            return;
        }

        _currentFilter = filter;
        RefreshResults();
    }

    public void ApplyBloatCategoryFilter(StorageCategoryFilter filter)
    {
        if (_currentReview is null)
        {
            return;
        }

        _currentCategoryFilter = filter;
        SelectCategoryFilterOption(_currentCategoryFilter);
        RefreshResults();
    }

    public void ApplyStorageReviewSearch(string searchText)
    {
        if (_currentReview is null)
        {
            return;
        }

        _currentSearch = StorageReviewSearch.FromText(searchText);
        SetSearchTextSilently(_currentSearch.Query);
        RefreshResults();
    }

    public void ApplySafetyReviewShortcut(StorageScanSafetyShortcut shortcut)
    {
        if (_currentReview is null)
        {
            return;
        }

        var shortcutFilter = StorageScanSafetyShortcutFilterBuilder.Build(shortcut);
        _currentFilter = shortcutFilter.ReviewFilter;
        _currentCategoryFilter = shortcutFilter.CategoryFilter;
        SelectCategoryFilterOption(_currentCategoryFilter);
        RefreshResults();
        StatusText.Text = $"Review shortcut applied: {shortcutFilter.Label}. No files were modified.";
    }

    public bool SelectDisplayedPath(string fullPath)
    {
        var row = DisplayedRows.FirstOrDefault(candidate =>
            candidate.FullPath.Equals(fullPath, StringComparison.OrdinalIgnoreCase));
        if (row is null)
        {
            return false;
        }

        ResultsGrid.SelectedItem = row;
        ResultsGrid.ScrollIntoView(row);
        return true;
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
            ExportQuarantinePreviewButton.IsEnabled = false;
            AddShownToShortlistButton.IsEnabled = false;
            RemoveShownFromShortlistButton.IsEnabled = false;
            SearchBox.IsEnabled = false;
            ClearSearchButton.IsEnabled = false;
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
        ExportQuarantinePreviewButton.IsEnabled = _currentQuarantinePreview is not null;
        SearchBox.IsEnabled = true;
        ClearSearchButton.IsEnabled = _currentSearch.IsActive;
    }

    private StorageEntryRow[] ApplyCurrentFilter()
    {
        return BuildDisplayedRows(ApplyCurrentReviewFilters());
    }

    private IReadOnlyList<StorageReviewEntry> ApplyCurrentReviewFilters()
    {
        return _currentReview?.ApplyFilter(_currentFilter, _currentCategoryFilter, _currentSearch) ?? [];
    }

    private StorageEntryRow[] BuildDisplayedRows(IReadOnlyList<StorageReviewEntry> entries)
    {
        return entries
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

        var matchedEntries = ApplyCurrentReviewFilters();
        var displayedCount = Math.Min(matchedEntries.Count, MaxDisplayedRows);
        var largestMatchedBytes = matchedEntries.Select(row => row.Entry.SizeBytes).DefaultIfEmpty(0).Max();
        var categoryLabel = _currentCategoryFilter.Kind == StorageCategoryFilterKind.All ? "" : $" + {FormatCategoryFilter(_currentCategoryFilter)}";
        var searchLabel = _currentSearch.IsActive ? $" + Search \"{_currentSearch.Query}\"" : "";
        var matchLabel = matchedEntries.Count > displayedCount
            ? $"{displayedCount:N0} shown of {matchedEntries.Count:N0} matched"
            : $"{displayedCount:N0} shown";
        var limitLabel = matchedEntries.Count > displayedCount
            ? $" Display limit {MaxDisplayedRows:N0}; narrow filters/search to inspect more matches."
            : "";
        FilterSummaryText.Text =
            $"{FormatFilter(_currentFilter)}{categoryLabel}{searchLabel}: {matchLabel}, " +
            $"largest matched row {ByteSizeFormatter.Format(largestMatchedBytes)}, " +
            $"shortlist {_shortlist.Count:N0}.{limitLabel}";
    }

    private static string FormatScanCompletedStatus(int displayedCount, int matchedCount)
    {
        return matchedCount > displayedCount
            ? $"Storage Scan completed. Showing {displayedCount:N0} of {matchedCount:N0} paths. No files were modified."
            : $"Storage Scan completed. Showing {displayedCount:N0} paths. No files were modified.";
    }

    private void UpdateShortlistControls()
    {
        var hasSelectedRow = _selectedRow is not null;
        var isShortlisted = hasSelectedRow && _shortlist.Contains(_selectedRow!.Entry);
        AddToShortlistButton.IsEnabled = hasSelectedRow && !isShortlisted && ScanButton.IsEnabled;
        RemoveFromShortlistButton.IsEnabled = hasSelectedRow && isShortlisted && ScanButton.IsEnabled;

        var hasShortlist = _shortlist.Count > 0;
        var hasUnshortlistedDisplayedRows = DisplayedRows.Any(row => !_shortlist.Contains(row.Entry));
        var hasShortlistedDisplayedRows = DisplayedRows.Any(row => _shortlist.Contains(row.Entry));
        AddShownToShortlistButton.IsEnabled = _currentReview is not null && hasUnshortlistedDisplayedRows && ScanButton.IsEnabled;
        RemoveShownFromShortlistButton.IsEnabled = _currentReview is not null && hasShortlistedDisplayedRows && ScanButton.IsEnabled;
        ExportShortlistCsvButton.IsEnabled = _currentReview is not null && hasShortlist && ScanButton.IsEnabled;
        ClearShortlistButton.IsEnabled = hasShortlist && ScanButton.IsEnabled;
        PreviewQuarantineButton.IsEnabled = _currentReview is not null && hasShortlist && ScanButton.IsEnabled;
        ExportQuarantinePreviewButton.IsEnabled = _currentQuarantinePreview is not null && ScanButton.IsEnabled;
        SearchBox.IsEnabled = _currentReview is not null && ScanButton.IsEnabled;
        ClearSearchButton.IsEnabled = _currentReview is not null && _currentSearch.IsActive && ScanButton.IsEnabled;
    }

    private void SetSearchTextSilently(string text)
    {
        _isUpdatingSearchBox = true;
        try
        {
            SearchBox.Text = text;
        }
        finally
        {
            _isUpdatingSearchBox = false;
        }
    }

    private void ClearQuarantinePreview()
    {
        _currentQuarantinePreview = null;
        _currentRestoreManifestDraft = null;
        _currentQuarantineConfirmationDraft = null;
        QuarantinePreviewText.Text = "Preview and draft readiness appear after using Preview quarantine.";
        ExportQuarantinePreviewButton.IsEnabled = false;
    }

    private static string FormatQuarantinePreview(
        QuarantinePreview preview,
        RestoreManifestDraft restoreManifestDraft,
        QuarantineConfirmationDraft confirmationDraft)
    {
        const int maxRows = 12;
        var lines = new List<string>
        {
            $"Default root: {preview.QuarantineRootPath}",
            $"Included: {preview.IncludedCount:N0} | Blocked: {preview.BlockedCount:N0} | Redundant: {preview.RedundantCount:N0}",
            $"Previewed size: {preview.IncludedSizeDisplay}",
            $"Restore Manifest Draft: {restoreManifestDraft.DraftId} | Entries: {restoreManifestDraft.EntryCount:N0} | Bytes: {restoreManifestDraft.TotalSizeDisplay} | Executed manifest: {FormatYesNo(restoreManifestDraft.IsExecutedManifest)}",
            $"Quarantine Confirmation Draft: {confirmationDraft.ConfirmationId} | Required future text: {confirmationDraft.RequiredConfirmationText} | Execution implemented: {FormatYesNo(confirmationDraft.IsExecutionImplemented)}",
            confirmationDraft.HasDataBlockers
                ? $"Readiness blockers: {confirmationDraft.Blockers.Count:N0}"
                : "Readiness blockers: 0",
            "No files were modified."
        };

        foreach (var blocker in confirmationDraft.Blockers.Take(6))
        {
            lines.Add($"Blocker | {blocker}");
        }

        if (confirmationDraft.Blockers.Count > 6)
        {
            lines.Add($"... {confirmationDraft.Blockers.Count - 6:N0} more readiness blocker(s) not shown in this pane.");
        }

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

    private static string BuildDraftId(string prefix)
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        return $"{prefix}-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}-{suffix}";
    }

    private static string FormatYesNo(bool value)
    {
        return value ? "yes" : "no";
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

    private void UpdateSafetySummary()
    {
        if (_currentSafetySummary is null)
        {
            SafetySummaryText.Text = "Safety summary appears after a scan.";
            UpdateSafetyShortcutButtons();
            return;
        }

        var summary = _currentSafetySummary;
        SafetySummaryText.Text =
            $"Safety summary: {summary.StatusLabel} | " +
            $"High risk {summary.HighRiskCount:N0} | " +
            $"Protected {summary.ProtectedLocationCount:N0} | " +
            $"Access issues {summary.AccessIssueCount:N0} | " +
            $"Reparse points {summary.ReparsePointCount:N0} | " +
            $"Quarantine candidates {summary.QuarantineCandidateCount:N0} | " +
            $"No category {summary.UncategorizedCount:N0}. " +
            string.Join(" ", summary.Notes);
        UpdateSafetyShortcutButtons();
    }

    private void UpdateSafetyShortcutButtons()
    {
        if (_currentSafetySummary is null)
        {
            SafetyHighRiskButton.Content = "Review high risk";
            SafetyProtectedButton.Content = "Review protected";
            SafetyAccessIssuesButton.Content = "Review access";
            SafetyReparsePointsButton.Content = "Review reparse";
            SafetyQuarantineCandidatesButton.Content = "Review quarantine";
            SafetyNoCategoryButton.Content = "Review no category";
            SafetyHighRiskButton.IsEnabled = false;
            SafetyProtectedButton.IsEnabled = false;
            SafetyAccessIssuesButton.IsEnabled = false;
            SafetyReparsePointsButton.IsEnabled = false;
            SafetyQuarantineCandidatesButton.IsEnabled = false;
            SafetyNoCategoryButton.IsEnabled = false;
            return;
        }

        var summary = _currentSafetySummary;
        var canUseShortcuts = ScanButton.IsEnabled;
        SafetyHighRiskButton.Content = $"Review high risk ({summary.HighRiskCount:N0})";
        SafetyProtectedButton.Content = $"Review protected ({summary.ProtectedLocationCount:N0})";
        SafetyAccessIssuesButton.Content = $"Review access ({summary.AccessIssueCount:N0})";
        SafetyReparsePointsButton.Content = $"Review reparse ({summary.ReparsePointCount:N0})";
        SafetyQuarantineCandidatesButton.Content = $"Review quarantine ({summary.QuarantineCandidateCount:N0})";
        SafetyNoCategoryButton.Content = $"Review no category ({summary.UncategorizedCount:N0})";
        SafetyHighRiskButton.IsEnabled = canUseShortcuts && summary.HighRiskCount > 0;
        SafetyProtectedButton.IsEnabled = canUseShortcuts && summary.ProtectedLocationCount > 0;
        SafetyAccessIssuesButton.IsEnabled = canUseShortcuts && summary.AccessIssueCount > 0;
        SafetyReparsePointsButton.IsEnabled = canUseShortcuts && summary.ReparsePointCount > 0;
        SafetyQuarantineCandidatesButton.IsEnabled = canUseShortcuts && summary.QuarantineCandidateCount > 0;
        SafetyNoCategoryButton.IsEnabled = canUseShortcuts && summary.UncategorizedCount > 0;
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
        var searchSegment = _currentSearch.IsActive
            ? $"-search-{FormatFileNameSearch(_currentSearch.Query)}"
            : "";
        return $"storage-scan-{DateTime.Now:yyyyMMdd-HHmmss}-{FormatFileNameFilter(_currentFilter)}-{categorySegment}{searchSegment}.csv";
    }

    private static string FormatFileNameSearch(string searchQuery)
    {
        var characters = searchQuery
            .Trim()
            .ToLowerInvariant()
            .Select(character => char.IsLetterOrDigit(character) ? character : '-')
            .ToArray();
        var segment = string.Join(
            "-",
            new string(characters)
                .Split('-', StringSplitOptions.RemoveEmptyEntries));
        return string.IsNullOrWhiteSpace(segment)
            ? "query"
            : segment.Length > 48 ? segment[..48] : segment;
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

    private void SelectCategoryFilterOption(StorageCategoryFilter filter)
    {
        var option = CategoryFilterBox.Items
            .Cast<CategoryFilterOption>()
            .FirstOrDefault(candidate => candidate.Filter == filter);
        if (option is null)
        {
            return;
        }

        _isUpdatingCategoryFilterOptions = true;
        try
        {
            CategoryFilterBox.SelectedItem = option;
        }
        finally
        {
            _isUpdatingCategoryFilterOptions = false;
        }
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

    private static string FormatSelectedPathReviewGuidance(StorageEntry entry)
    {
        var guidance = SelectedPathReviewGuidanceBuilder.Build(entry);
        return $"{guidance.ActionLabel}: {string.Join(" ", guidance.Notes)}";
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
            BloatCategory.WindowsAppData => "Windows app data",
            BloatCategory.WindowsAppLeftover => "Windows app leftover",
            BloatCategory.InstalledApplication => "Installed application",
            BloatCategory.GameData => "Game data",
            BloatCategory.ProtectedLocation => "Protected location",
            BloatCategory.ReparsePoint => "Reparse point",
            BloatCategory.AccessIssue => "Access issue",
            _ => category.ToString()
        };
    }
}
