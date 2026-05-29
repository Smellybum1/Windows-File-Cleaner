using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;
using WindowsFileCleaner.Core;

namespace WindowsFileCleaner.App;

public partial class MainWindow : Window
{
    private const int MaxDisplayedRows = 2000;
    private static readonly TimeSpan SearchDebounceDelay = TimeSpan.FromMilliseconds(350);
    private readonly StorageReviewShortlist _shortlist = new();
    private readonly DispatcherTimer _searchDebounceTimer = new() { Interval = SearchDebounceDelay };
    private CancellationTokenSource? _scanCancellation;
    private StorageScanReview? _currentReview;
    private StorageScanSafetySummary? _currentSafetySummary;
    private QuarantinePreview? _currentQuarantinePreview;
    private RestoreManifestDraft? _currentRestoreManifestDraft;
    private RestoreManifest? _currentRestoreManifest;
    private QuarantineConfirmationDraft? _currentQuarantineConfirmationDraft;
    private QuarantineExecutionGate? _currentQuarantineExecutionGate;
    private QuarantineActionDraft? _currentQuarantineActionDraft;
    private QuarantineExecutionResult? _currentQuarantineExecutionResult;
    private UndoQuarantineResult? _currentUndoQuarantineResult;
    private QuarantineManifestDiscovery? _currentQuarantineManifestDiscovery;
    private RestoreReadinessPreview? _currentRestoreReadinessPreview;
    private SelectedRestoreManifestReview? _currentSelectedRestoreManifestReview;
    private SelectedRestoreConfirmationDraft? _currentSelectedRestoreConfirmationDraft;
    private SelectedRestoreExecutionGate? _currentSelectedRestoreExecutionGate;
    private UndoQuarantineResult? _currentSelectedRestoreResult;
    private string? _currentCleanupScopePath;
    private StorageReviewFilter _currentFilter = StorageReviewFilter.All;
    private StorageCategoryFilter _currentCategoryFilter = StorageCategoryFilter.All;
    private StorageEntryTypeFilter _currentEntryTypeFilter = StorageEntryTypeFilter.All;
    private StorageSizeThresholdFilter _currentSizeThresholdFilter = StorageSizeThresholdFilter.All;
    private StorageReviewSearch _currentSearch = StorageReviewSearch.Empty;
    private int _currentDisplayStartIndex;
    private StorageEntryRow? _selectedRow;
    private bool _isScanning;
    private bool _isUpdatingCategoryFilterOptions;
    private bool _isUpdatingEntryTypeFilterOptions;
    private bool _isUpdatingSizeThresholdFilterOptions;
    private bool _isUpdatingSearchBox;
    private bool _isUpdatingQuarantineConfirmationBox;
    private bool _isUpdatingRestoreManifestSelectionBox;
    private bool _isUpdatingSelectedRestoreConfirmationBox;
    private bool _isWindowInitialized;
    private bool _isShowingQuarantinedRows;

    public MainWindow()
        : this(StorageScanOptions.DefaultForCurrentUser().CleanupScopePath)
    {
    }

    public MainWindow(string initialCleanupScopePath)
    {
        InitializeComponent();
        _searchDebounceTimer.Tick += SearchDebounceTimer_Tick;
        _isWindowInitialized = true;
        ScopePathBox.Text = initialCleanupScopePath;
        ResultsGrid.ItemsSource = Array.Empty<StorageEntryRow>();
        QuarantinedGrid.ItemsSource = Array.Empty<QuarantinedItemRow>();
        UpdateCategoryFilterOptions();
        UpdateEntryTypeFilterOptions();
        UpdateSizeThresholdFilterOptions();
        UpdateCleanupScopeSafetyNote();
        UpdateQuarantineRootSafetyNote();
        UpdateShortlistSafetyMix();
        UpdateQuarantinePreviewStatus();
        UpdateQuarantineExecutionGate();
        UpdateQuarantineManifestDiscoveryControls();
        UpdateQuarantinedViewControls();
        UpdateReviewGridModeText();
    }

    public string CurrentCleanupScopePath => ScopePathBox.Text;

    public string CleanupScopePathBoxToolTipValue => ScopePathBox.ToolTip?.ToString() ?? "";

    public string CleanupScopePathBoxAutomationHelpTextValue => AutomationProperties.GetHelpText(ScopePathBox);

    public string CleanupScopeSafetyNoteTextValue => ScopeSafetyNoteText.Text;

    public string CleanupScopeSafetyNoteToolTipValue => ScopeSafetyNoteText.ToolTip?.ToString() ?? "";

    public string CleanupScopeSafetyNoteAutomationHelpTextValue => AutomationProperties.GetHelpText(ScopeSafetyNoteText);

    public string CleanupScopeSafetyNoteHelpCueToolTipValue => CleanupScopeSafetyNoteHelpCue.ToolTip?.ToString() ?? "";

    public string CleanupScopeSafetyNoteHelpCueAutomationNameValue => AutomationProperties.GetName(CleanupScopeSafetyNoteHelpCue);

    public string CleanupScopeSafetyNoteHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(CleanupScopeSafetyNoteHelpCue);

    public string CurrentStatusText => StatusText.Text;

    public bool CanStartStorageScan => ScanButton.IsEnabled;

    public bool CanCancelStorageScan => CancelButton.IsEnabled;

    public bool CanBrowseCleanupScope => BrowseScopeButton.IsEnabled;

    public string BrowseCleanupScopeButtonText => BrowseScopeButton.Content?.ToString() ?? "";

    public string BrowseCleanupScopeButtonToolTipValue => BrowseScopeButton.ToolTip?.ToString() ?? "";

    public string BrowseCleanupScopeButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(BrowseScopeButton);

    public bool IsRealProfilePreflightConfirmationVisible => RealProfilePreflightPanel.Visibility == Visibility.Visible;

    public bool IsRealProfilePreflightConfirmed => RealProfilePreflightCheckBox.IsChecked == true;

    public string RealProfilePreflightCheckBoxToolTipValue => RealProfilePreflightCheckBox.ToolTip?.ToString() ?? "";

    public string RealProfilePreflightCheckBoxAutomationHelpTextValue => AutomationProperties.GetHelpText(RealProfilePreflightCheckBox);

    public bool IsRealProfilePreflightHelpCueVisible => RealProfilePreflightHelpCue.Visibility == Visibility.Visible
        && RealProfilePreflightPanel.Visibility == Visibility.Visible;

    public string RealProfilePreflightHelpCueToolTipValue => RealProfilePreflightHelpCue.ToolTip?.ToString() ?? "";

    public string RealProfilePreflightHelpCueAutomationNameValue => AutomationProperties.GetName(RealProfilePreflightHelpCue);

    public string RealProfilePreflightHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(RealProfilePreflightHelpCue);

    public string ScanGateTextValue => ScanGateText.Text;

    public string ScanGateSummaryTextValue => ScanGateSummaryText.Text;

    public string ScanGateSummaryToolTipValue => ScanGateSummaryText.ToolTip?.ToString() ?? "";

    public string ScanGateSummaryAutomationHelpTextValue => AutomationProperties.GetHelpText(ScanGateSummaryText);

    public string ScanGateSummaryHelpCueToolTipValue => ScanGateSummaryHelpCue.ToolTip?.ToString() ?? "";

    public string ScanGateSummaryHelpCueAutomationNameValue => AutomationProperties.GetName(ScanGateSummaryHelpCue);

    public string ScanGateSummaryHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(ScanGateSummaryHelpCue);

    public string ScanButtonToolTipValue => ScanButton.ToolTip?.ToString() ?? "";

    public string ScanButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(ScanButton);

    public string CancelButtonToolTipValue => CancelButton.ToolTip?.ToString() ?? "";

    public string CancelButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(CancelButton);

    public bool CanExportScanCsv => ExportCsvButton.IsEnabled;

    public string ExportScanCsvButtonToolTipValue => ExportCsvButton.ToolTip?.ToString() ?? "";

    public string ExportScanCsvButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(ExportCsvButton);

    public int CurrentScanReportExportRowCount => BuildCurrentScanReportExportRows().Count;

    public IReadOnlyList<string> CurrentScanReportExportPaths => BuildCurrentScanReportExportRows()
        .Select(row => row.Entry.FullPath)
        .ToArray();

    public IReadOnlyList<string> CurrentScanReportExportTypes => BuildCurrentScanReportExportRows()
        .Select(row => row.Entry.IsDirectory ? "Folder" : "File")
        .ToArray();

    public string CurrentScanReportExportFileName => BuildExportFileName();

    public string CurrentScanReportExportCsv => StorageScanCsvExporter.Export(
        BuildCurrentScanReportExportRows(),
        _currentCleanupScopePath);

    public bool CanUseCategoryFilter => CategoryFilterBox.IsEnabled;

    public bool CanUseEntryTypeFilter => EntryTypeFilterBox.IsEnabled;

    public bool CanUseSizeThresholdFilter => SizeThresholdFilterBox.IsEnabled;

    public bool CanResetReviewView => ResetViewButton.IsEnabled;

    public string AllFilterButtonToolTipValue => AllFilterButton.ToolTip?.ToString() ?? "";

    public string AllFilterButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(AllFilterButton);

    public string LikelySafeFilterButtonToolTipValue => LikelySafeFilterButton.ToolTip?.ToString() ?? "";

    public string LikelySafeFilterButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(LikelySafeFilterButton);

    public string CautionFilterButtonToolTipValue => CautionFilterButton.ToolTip?.ToString() ?? "";

    public string CautionFilterButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(CautionFilterButton);

    public string HighRiskFilterButtonToolTipValue => HighRiskFilterButton.ToolTip?.ToString() ?? "";

    public string HighRiskFilterButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(HighRiskFilterButton);

    public string QuarantineCandidateFilterButtonToolTipValue => QuarantineCandidateFilterButton.ToolTip?.ToString() ?? "";

    public string QuarantineCandidateFilterButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantineCandidateFilterButton);

    public string AccessIssuesFilterButtonToolTipValue => AccessIssuesFilterButton.ToolTip?.ToString() ?? "";

    public string AccessIssuesFilterButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(AccessIssuesFilterButton);

    public string EntryTypeFilterToolTipValue => EntryTypeFilterBox.ToolTip?.ToString() ?? "";

    public string EntryTypeFilterAutomationHelpTextValue => AutomationProperties.GetHelpText(EntryTypeFilterBox);

    public string SizeThresholdFilterToolTipValue => SizeThresholdFilterBox.ToolTip?.ToString() ?? "";

    public string SizeThresholdFilterAutomationHelpTextValue => AutomationProperties.GetHelpText(SizeThresholdFilterBox);

    public string CategoryFilterToolTipValue => CategoryFilterBox.ToolTip?.ToString() ?? "";

    public string CategoryFilterAutomationHelpTextValue => AutomationProperties.GetHelpText(CategoryFilterBox);

    public string ClearSearchButtonToolTipValue => ClearSearchButton.ToolTip?.ToString() ?? "";

    public string ClearSearchButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(ClearSearchButton);

    public string ResetReviewViewButtonToolTipValue => ResetViewButton.ToolTip?.ToString() ?? "";

    public string ResetReviewViewButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(ResetViewButton);

    public string CurrentEntryTypeFilterLabel => EntryTypeFilterBox.SelectedItem is EntryTypeFilterOption option
        ? option.Label
        : "";

    public string CurrentSizeThresholdFilterLabel => SizeThresholdFilterBox.SelectedItem is SizeThresholdFilterOption option
        ? option.Label
        : "";

    private static (string Name, string Cursor, int InitialShowDelay) GetHelpCueAffordance(string name, FrameworkElement cue)
    {
        return (name, cue.Cursor?.ToString() ?? "", ToolTipService.GetInitialShowDelay(cue));
    }

    public int DisplayedRowCount => DisplayedRows.Count;

    public IReadOnlyList<StorageEntryRow> DisplayedRows => ResultsGrid.ItemsSource is IEnumerable<StorageEntryRow> rows
        ? rows.ToArray()
        : [];

    public IReadOnlyList<QuarantinedItemRow> DisplayedQuarantinedRows => QuarantinedGrid.ItemsSource is IEnumerable<QuarantinedItemRow> rows
        ? rows.ToArray()
        : [];

    public bool IsShowingQuarantinedRows => _isShowingQuarantinedRows;

    public bool AreScanRowsVisible => ResultsGrid.Visibility == Visibility.Visible;

    public bool AreQuarantinedRowsVisible => QuarantinedGrid.Visibility == Visibility.Visible;

    public string ReviewGridModeTextValue => ReviewGridModeText.Text;

    public string ReviewGridModeStatusStyleValue => ReviewGridModeText.Tag?.ToString() ?? "";

    public string ReviewGridModeStatusFontWeightValue => ReviewGridModeText.FontWeight.ToString();

    public string ReviewGridModeToolTipValue => ReviewGridModeText.ToolTip?.ToString() ?? "";

    public string ReviewGridModeAutomationHelpTextValue => AutomationProperties.GetHelpText(ReviewGridModeText);

    public string ReviewGridModeHelpCueToolTipValue => ReviewGridModeHelpCue.ToolTip?.ToString() ?? "";

    public string ReviewGridModeHelpCueAutomationNameValue => AutomationProperties.GetName(ReviewGridModeHelpCue);

    public string ReviewGridModeHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(ReviewGridModeHelpCue);

    public IReadOnlyList<(string Name, string Cursor, int InitialShowDelay)> HoverableHelpCueAffordances =>
    [
        GetHelpCueAffordance("Safety Summary header", SafetySummaryHeaderHelpCue),
        GetHelpCueAffordance("Review Mix", ReviewMixHelpCue),
        GetHelpCueAffordance("Matched Review Mix", MatchedReviewMixHelpCue),
        GetHelpCueAffordance("Review Shortlist Safety Mix", ShortlistSafetyMixHelpCue),
        GetHelpCueAffordance("Quarantine Shortlist header", QuarantineShortlistHeaderHelpCue),
        GetHelpCueAffordance("Inline Quarantine Preview status", QuarantinePreviewStatusHelpCue),
        GetHelpCueAffordance("Quarantine Execution Gate", QuarantineExecutionGateHelpCue),
        GetHelpCueAffordance("Selected Restore Execution Gate", SelectedRestoreExecutionGateHelpCue),
        GetHelpCueAffordance("Review Grid Mode Status", ReviewGridModeHelpCue),
        GetHelpCueAffordance("Real-profile preflight acknowledgement", RealProfilePreflightHelpCue),
        GetHelpCueAffordance("Scan gate summary", ScanGateSummaryHelpCue),
        GetHelpCueAffordance("Cleanup Scope Safety Note", CleanupScopeSafetyNoteHelpCue),
        GetHelpCueAffordance("Quarantine Root Safety Note", QuarantineRootSafetyNoteHelpCue),
        GetHelpCueAffordance("Shortlist confirmation", QuarantineConfirmationHelpCue),
        GetHelpCueAffordance("Quarantine Manifest Discovery", QuarantineManifestDiscoveryHelpCue),
        GetHelpCueAffordance("Restore Manifest selection", RestoreManifestSelectionHelpCue),
        GetHelpCueAffordance("All-manifest readiness", AllManifestReadinessHelpCue),
        GetHelpCueAffordance("Selected restore confirmation", SelectedRestoreConfirmationHelpCue)
    ];

    public string? ContentsColumnSortMemberPath => ResultsGrid.Columns
        .OfType<DataGridTextColumn>()
        .FirstOrDefault(column => string.Equals(column.Header?.ToString(), "Contents", StringComparison.OrdinalIgnoreCase))
        ?.SortMemberPath;

    public bool HasRelativePathColumn => ResultsGrid.Columns
        .OfType<DataGridTextColumn>()
        .Any(column => string.Equals(column.Header?.ToString(), "Relative path", StringComparison.OrdinalIgnoreCase));

    public string TotalSizeTextValue => TotalSizeText.Text;

    public string FolderCountTextValue => FolderCountText.Text;

    public string FileCountTextValue => FileCountText.Text;

    public string AccessIssueCountTextValue => AccessIssueCountText.Text;

    public string ReviewMixTextValue => ReviewMixText.Text;

    public string ReviewMixToolTipValue => ReviewMixText.ToolTip?.ToString() ?? "";

    public string ReviewMixAutomationHelpTextValue => AutomationProperties.GetHelpText(ReviewMixText);

    public string ReviewMixHelpCueToolTipValue => ReviewMixHelpCue.ToolTip?.ToString() ?? "";

    public string ReviewMixHelpCueAutomationNameValue => AutomationProperties.GetName(ReviewMixHelpCue);

    public string ReviewMixHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(ReviewMixHelpCue);

    public string SafetySummaryTextValue => SafetySummaryText.Text;

    public string SafetySummaryHeaderTextValue => SafetySummaryHeaderText.Text;

    public string SafetySummaryHeaderToolTipValue => SafetySummaryHeaderText.ToolTip?.ToString() ?? "";

    public string SafetySummaryHeaderAutomationHelpTextValue => AutomationProperties.GetHelpText(SafetySummaryHeaderText);

    public string SafetySummaryHeaderStatusStyleValue => SafetySummaryHeaderText.Tag?.ToString() ?? "";

    public string SafetySummaryHeaderHelpCueToolTipValue => SafetySummaryHeaderHelpCue.ToolTip?.ToString() ?? "";

    public string SafetySummaryHeaderHelpCueAutomationNameValue => AutomationProperties.GetName(SafetySummaryHeaderHelpCue);

    public string SafetySummaryHeaderHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(SafetySummaryHeaderHelpCue);

    public string SafetyHighRiskButtonToolTipValue => SafetyHighRiskButton.ToolTip?.ToString() ?? "";

    public string SafetyHighRiskButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(SafetyHighRiskButton);

    public string SafetyProtectedButtonToolTipValue => SafetyProtectedButton.ToolTip?.ToString() ?? "";

    public string SafetyProtectedButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(SafetyProtectedButton);

    public string SafetyAccessIssuesButtonToolTipValue => SafetyAccessIssuesButton.ToolTip?.ToString() ?? "";

    public string SafetyAccessIssuesButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(SafetyAccessIssuesButton);

    public string SafetyReparsePointsButtonToolTipValue => SafetyReparsePointsButton.ToolTip?.ToString() ?? "";

    public string SafetyReparsePointsButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(SafetyReparsePointsButton);

    public string SafetyQuarantineCandidatesButtonToolTipValue => SafetyQuarantineCandidatesButton.ToolTip?.ToString() ?? "";

    public string SafetyQuarantineCandidatesButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(SafetyQuarantineCandidatesButton);

    public string SafetyNoCategoryButtonToolTipValue => SafetyNoCategoryButton.ToolTip?.ToString() ?? "";

    public string SafetyNoCategoryButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(SafetyNoCategoryButton);

    public string FilterSummaryTextValue => FilterSummaryText.Text;

    public string MatchedReviewMixTextValue => MatchedReviewMixText.Text;

    public string MatchedReviewMixToolTipValue => MatchedReviewMixText.ToolTip?.ToString() ?? "";

    public string MatchedReviewMixAutomationHelpTextValue => AutomationProperties.GetHelpText(MatchedReviewMixText);

    public string MatchedReviewMixHelpCueToolTipValue => MatchedReviewMixHelpCue.ToolTip?.ToString() ?? "";

    public string MatchedReviewMixHelpCueAutomationNameValue => AutomationProperties.GetName(MatchedReviewMixHelpCue);

    public string MatchedReviewMixHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(MatchedReviewMixHelpCue);

    public string ReviewSizeNoteTextValue => ReviewSizeNoteText.Text;

    public string ReviewWindowTextValue => ReviewWindowText.Text;

    public bool CanShowPreviousReviewWindow => PreviousReviewWindowButton.IsEnabled;

    public string PreviousReviewWindowButtonToolTipValue => PreviousReviewWindowButton.ToolTip?.ToString() ?? "";

    public string PreviousReviewWindowButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(PreviousReviewWindowButton);

    public bool CanShowNextReviewWindow => NextReviewWindowButton.IsEnabled;

    public string NextReviewWindowButtonToolTipValue => NextReviewWindowButton.ToolTip?.ToString() ?? "";

    public string NextReviewWindowButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(NextReviewWindowButton);

    public string CurrentSearchText => SearchBox.Text;

    public bool IsStorageReviewSearchPending => _searchDebounceTimer.IsEnabled;

    public string SearchHelpToolTipValue => SearchBox.ToolTip?.ToString() ?? "";

    public string SearchHelpAutomationHelpTextValue => AutomationProperties.GetHelpText(SearchBox);

    public string DetailGuidanceTextValue => DetailGuidanceText.Text;

    public string DetailPathContextTextValue => DetailPathContextText.Text;

    public string DetailMetaTextValue => DetailMetaText.Text;

    public string DetailSubtreeSummaryTextValue => DetailSubtreeSummaryText.Text;

    public string DetailHotspotTrailTextValue => DetailHotspotTrailText.Text;

    public string FilePreviewTextValue => FilePreviewText.Text;

    public string QuarantinePreviewTextValue => QuarantinePreviewText.Text;

    public string QuarantinePreviewStatusTextValue => QuarantinePreviewStatusText.Text;

    public string QuarantinePreviewStatusStyleValue => QuarantinePreviewStatusText.Tag?.ToString() ?? "";

    public string QuarantinePreviewStatusForegroundValue => QuarantinePreviewStatusText.Foreground.ToString();

    public string QuarantinePreviewStatusFontWeightValue => QuarantinePreviewStatusText.FontWeight.ToString();

    public string QuarantinePreviewStatusToolTipValue => QuarantinePreviewStatusText.ToolTip?.ToString() ?? "";

    public string QuarantinePreviewStatusAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantinePreviewStatusText);

    public string QuarantinePreviewStatusHelpCueToolTipValue => QuarantinePreviewStatusHelpCue.ToolTip?.ToString() ?? "";

    public string QuarantinePreviewStatusHelpCueAutomationNameValue => AutomationProperties.GetName(QuarantinePreviewStatusHelpCue);

    public string QuarantinePreviewStatusHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantinePreviewStatusHelpCue);

    public string QuarantineExecutionGateTextValue => QuarantineExecutionGateText.Text;

    public string QuarantineExecutionGateToolTipValue => QuarantineExecutionGateText.ToolTip?.ToString() ?? "";

    public string QuarantineExecutionGateAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantineExecutionGateText);

    public string QuarantineExecutionGateHelpCueToolTipValue => QuarantineExecutionGateHelpCue.ToolTip?.ToString() ?? "";

    public string QuarantineExecutionGateHelpCueAutomationNameValue => AutomationProperties.GetName(QuarantineExecutionGateHelpCue);

    public string QuarantineExecutionGateHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantineExecutionGateHelpCue);

    public double QuarantineExecutionGateViewportMaxHeight => QuarantineExecutionGateScroll.MaxHeight;

    public bool IsQuarantineShortlistExpanded => QuarantineShortlistExpander.IsExpanded;

    public bool IsSafetySummaryExpanded => SafetySummaryExpander.IsExpanded;

    public string QuarantineManifestDiscoveryTextValue => QuarantineManifestDiscoveryText.Text;

    public string RestoreReadinessPreviewTextValue => RestoreReadinessPreviewText.Text;

    public string SelectedRestoreManifestReviewTextValue => SelectedRestoreManifestReviewText.Text;

    public string SelectedRestoreExecutionGateTextValue => SelectedRestoreExecutionGateText.Text;

    public string SelectedRestoreExecutionGateToolTipValue => SelectedRestoreExecutionGateText.ToolTip?.ToString() ?? "";

    public string SelectedRestoreExecutionGateAutomationHelpTextValue => AutomationProperties.GetHelpText(SelectedRestoreExecutionGateText);

    public string SelectedRestoreExecutionGateHelpCueToolTipValue => SelectedRestoreExecutionGateHelpCue.ToolTip?.ToString() ?? "";

    public string SelectedRestoreExecutionGateHelpCueAutomationNameValue => AutomationProperties.GetName(SelectedRestoreExecutionGateHelpCue);

    public string SelectedRestoreExecutionGateHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(SelectedRestoreExecutionGateHelpCue);

    public string CurrentQuarantineRootPath => QuarantineRootBox.Text;

    public string QuarantineRootSafetyNoteTextValue => QuarantineRootSafetyNoteText.Text;

    public string QuarantineRootSafetyNoteToolTipValue => QuarantineRootSafetyNoteText.ToolTip?.ToString() ?? "";

    public string QuarantineRootSafetyNoteAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantineRootSafetyNoteText);

    public string QuarantineRootSafetyNoteHelpCueToolTipValue => QuarantineRootSafetyNoteHelpCue.ToolTip?.ToString() ?? "";

    public string QuarantineRootSafetyNoteHelpCueAutomationNameValue => AutomationProperties.GetName(QuarantineRootSafetyNoteHelpCue);

    public string QuarantineRootSafetyNoteHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantineRootSafetyNoteHelpCue);

    public string? CurrentQuarantinePreviewRootPath => _currentQuarantinePreview?.QuarantineRootPath;

    public string? CurrentRestoreManifestPath => _currentRestoreManifest?.ManifestPath;

    public string? CurrentRestoreManifestStatus => _currentRestoreManifest?.ActionStatus.ToString();

    public string? CurrentFirstQuarantinePath => _currentRestoreManifest?.Entries.FirstOrDefault()?.QuarantinePath;

    public IReadOnlyList<string> CurrentQuarantinePaths => _currentRestoreManifest?.Entries.Select(entry => entry.QuarantinePath).ToArray() ?? [];

    public bool CanUndoQuarantine => UndoQuarantineButton.IsEnabled;

    public bool CanShowQuarantinedRows => ShowQuarantinedButton.IsEnabled;

    public bool CanShowScanRows => BackToScanRowsButton.IsEnabled;

    public string ShowQuarantinedButtonText => ShowQuarantinedButton.Content?.ToString() ?? "";

    public string ShowQuarantinedButtonToolTipValue => ShowQuarantinedButton.ToolTip?.ToString() ?? "";

    public string ShowQuarantinedButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(ShowQuarantinedButton);

    public string BackToScanRowsButtonToolTipValue => BackToScanRowsButton.ToolTip?.ToString() ?? "";

    public string BackToScanRowsButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(BackToScanRowsButton);

    public string QuarantineConfirmationToolTipValue => QuarantineConfirmationBox.ToolTip?.ToString() ?? "";

    public string QuarantineConfirmationAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantineConfirmationBox);

    public string QuarantineConfirmationHelpCueToolTipValue => QuarantineConfirmationHelpCue.ToolTip?.ToString() ?? "";

    public string QuarantineConfirmationHelpCueAutomationNameValue => AutomationProperties.GetName(QuarantineConfirmationHelpCue);

    public string QuarantineConfirmationHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantineConfirmationHelpCue);

    public string ExecuteQuarantineButtonToolTipValue => ExecuteQuarantineButton.ToolTip?.ToString() ?? "";

    public string ExecuteQuarantineButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(ExecuteQuarantineButton);

    public string ExecuteQuarantineButtonText => ExecuteQuarantineButton.Content?.ToString() ?? "";

    public string UndoQuarantineButtonToolTipValue => UndoQuarantineButton.ToolTip?.ToString() ?? "";

    public string UndoQuarantineButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(UndoQuarantineButton);

    public bool CanDiscoverQuarantineManifests => DiscoverQuarantineManifestsButton.IsEnabled;

    public string DiscoverQuarantineManifestsButtonToolTipValue => DiscoverQuarantineManifestsButton.ToolTip?.ToString() ?? "";

    public string DiscoverQuarantineManifestsButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(DiscoverQuarantineManifestsButton);

    public string QuarantineManifestDiscoveryHelpCueToolTipValue => QuarantineManifestDiscoveryHelpCue.ToolTip?.ToString() ?? "";

    public string QuarantineManifestDiscoveryHelpCueAutomationNameValue => AutomationProperties.GetName(QuarantineManifestDiscoveryHelpCue);

    public string QuarantineManifestDiscoveryHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantineManifestDiscoveryHelpCue);

    public bool CanPreviewRestoreReadiness => PreviewRestoreReadinessButton.IsEnabled;

    public string PreviewRestoreReadinessButtonText => PreviewRestoreReadinessButton.Content?.ToString() ?? "";

    public string PreviewRestoreReadinessButtonToolTipValue => PreviewRestoreReadinessButton.ToolTip?.ToString() ?? "";

    public string PreviewRestoreReadinessButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(PreviewRestoreReadinessButton);

    public string AllManifestReadinessHelpCueToolTipValue => AllManifestReadinessHelpCue.ToolTip?.ToString() ?? "";

    public string AllManifestReadinessHelpCueAutomationNameValue => AutomationProperties.GetName(AllManifestReadinessHelpCue);

    public string AllManifestReadinessHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(AllManifestReadinessHelpCue);

    public bool CanSelectDiscoveredRestoreManifest => RestoreManifestSelectionBox.IsEnabled;

    public string RestoreManifestSelectionBoxToolTipValue => RestoreManifestSelectionBox.ToolTip?.ToString() ?? "";

    public string RestoreManifestSelectionBoxAutomationHelpTextValue => AutomationProperties.GetHelpText(RestoreManifestSelectionBox);

    public string RestoreManifestSelectionHelpCueToolTipValue => RestoreManifestSelectionHelpCue.ToolTip?.ToString() ?? "";

    public string RestoreManifestSelectionHelpCueAutomationNameValue => AutomationProperties.GetName(RestoreManifestSelectionHelpCue);

    public string RestoreManifestSelectionHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(RestoreManifestSelectionHelpCue);

    public bool CanPreviewSelectedRestoreManifestReadiness => PreviewSelectedRestoreManifestReadinessButton.IsEnabled;

    public string PreviewSelectedRestoreManifestReadinessButtonText => PreviewSelectedRestoreManifestReadinessButton.Content?.ToString() ?? "";

    public string PreviewSelectedRestoreManifestReadinessButtonToolTipValue => PreviewSelectedRestoreManifestReadinessButton.ToolTip?.ToString() ?? "";

    public string PreviewSelectedRestoreManifestReadinessButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(PreviewSelectedRestoreManifestReadinessButton);

    public bool CanPreviewSelectedRestoreGate => PreviewSelectedRestoreGateButton.IsEnabled;

    public string PreviewSelectedRestoreGateButtonToolTipValue => PreviewSelectedRestoreGateButton.ToolTip?.ToString() ?? "";

    public string PreviewSelectedRestoreGateButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(PreviewSelectedRestoreGateButton);

    public bool CanEnterSelectedRestoreConfirmation => SelectedRestoreConfirmationBox.IsEnabled;

    public string SelectedRestoreConfirmationToolTipValue => SelectedRestoreConfirmationBox.ToolTip?.ToString() ?? "";

    public string SelectedRestoreConfirmationAutomationHelpTextValue => AutomationProperties.GetHelpText(SelectedRestoreConfirmationBox);

    public string SelectedRestoreConfirmationHelpCueToolTipValue => SelectedRestoreConfirmationHelpCue.ToolTip?.ToString() ?? "";

    public string SelectedRestoreConfirmationHelpCueAutomationNameValue => AutomationProperties.GetName(SelectedRestoreConfirmationHelpCue);

    public string SelectedRestoreConfirmationHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(SelectedRestoreConfirmationHelpCue);

    public string CurrentSelectedRestoreConfirmationText => SelectedRestoreConfirmationBox.Text;

    public bool CanExecuteSelectedRestore => ExecuteSelectedRestoreButton.IsEnabled;

    public string ExecuteSelectedRestoreButtonToolTipValue => ExecuteSelectedRestoreButton.ToolTip?.ToString() ?? "";

    public string ExecuteSelectedRestoreButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(ExecuteSelectedRestoreButton);

    public int DiscoveredRestoreManifestCount => _currentQuarantineManifestDiscovery?.ManifestCount ?? 0;

    public string? SelectedRestoreManifestPath => RestoreManifestSelectionBox.SelectedItem is RestoreManifestSelectionOption option
        ? option.Summary.ManifestPath
        : null;

    public string? SelectedRowFullPath => _selectedRow?.FullPath;

    public int ReviewShortlistCount => _shortlist.Count;

    public string ShortlistSafetyMixTextValue => ShortlistSafetyMixText.Text;

    public string ShortlistSafetyMixToolTipValue => ShortlistSafetyMixText.ToolTip?.ToString() ?? "";

    public string ShortlistSafetyMixAutomationHelpTextValue => AutomationProperties.GetHelpText(ShortlistSafetyMixText);

    public string ShortlistSafetyMixHelpCueToolTipValue => ShortlistSafetyMixHelpCue.ToolTip?.ToString() ?? "";

    public string ShortlistSafetyMixHelpCueAutomationNameValue => AutomationProperties.GetName(ShortlistSafetyMixHelpCue);

    public string ShortlistSafetyMixHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(ShortlistSafetyMixHelpCue);

    public string QuarantineShortlistHeaderTextValue => QuarantineShortlistHeaderText.Text;

    public string QuarantineShortlistHeaderToolTipValue => QuarantineShortlistHeaderText.ToolTip?.ToString() ?? "";

    public string QuarantineShortlistHeaderAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantineShortlistHeaderText);

    public string QuarantineShortlistHeaderStatusStyleValue => QuarantineShortlistHeaderText.Tag?.ToString() ?? "";

    public string QuarantineShortlistHeaderHelpCueToolTipValue => QuarantineShortlistHeaderHelpCue.ToolTip?.ToString() ?? "";

    public string QuarantineShortlistHeaderHelpCueAutomationNameValue => AutomationProperties.GetName(QuarantineShortlistHeaderHelpCue);

    public string QuarantineShortlistHeaderHelpCueAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantineShortlistHeaderHelpCue);

    public bool CanAddSelectedRowToReviewShortlist => AddToShortlistButton.IsEnabled;

    public string AddSelectedRowToReviewShortlistButtonToolTipValue => AddToShortlistButton.ToolTip?.ToString() ?? "";

    public string AddSelectedRowToReviewShortlistButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(AddToShortlistButton);

    public bool CanRemoveSelectedRowFromReviewShortlist => RemoveFromShortlistButton.IsEnabled;

    public string RemoveSelectedRowFromReviewShortlistButtonToolTipValue => RemoveFromShortlistButton.ToolTip?.ToString() ?? "";

    public string RemoveSelectedRowFromReviewShortlistButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(RemoveFromShortlistButton);

    public bool CanPreviewSelectedFile => PreviewFileButton.IsEnabled;

    public string PreviewSelectedFileButtonToolTipValue => PreviewFileButton.ToolTip?.ToString() ?? "";

    public string PreviewSelectedFileButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(PreviewFileButton);

    public bool CanShowSelectedFolderChildren => ShowChildrenButton.IsEnabled;

    public string ShowSelectedFolderChildrenButtonToolTipValue => ShowChildrenButton.ToolTip?.ToString() ?? "";

    public string ShowSelectedFolderChildrenButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(ShowChildrenButton);

    public bool CanShowSelectedFolderDescendants => ShowDescendantsButton.IsEnabled;

    public string ShowSelectedFolderDescendantsButtonToolTipValue => ShowDescendantsButton.ToolTip?.ToString() ?? "";

    public string ShowSelectedFolderDescendantsButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(ShowDescendantsButton);

    public string CopySelectedPathButtonToolTipValue => CopyPathButton.ToolTip?.ToString() ?? "";

    public string CopySelectedPathButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(CopyPathButton);

    public string OpenSelectedPathInExplorerButtonToolTipValue => OpenInExplorerButton.ToolTip?.ToString() ?? "";

    public string OpenSelectedPathInExplorerButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(OpenInExplorerButton);

    public bool CanAddShownRowsToReviewShortlist => AddShownToShortlistButton.IsEnabled;

    public bool CanRemoveShownRowsFromReviewShortlist => RemoveShownFromShortlistButton.IsEnabled;

    public string AddShownRowsToReviewShortlistButtonText => AddShownToShortlistButton.Content?.ToString() ?? "";

    public string RemoveShownRowsFromReviewShortlistButtonText => RemoveShownFromShortlistButton.Content?.ToString() ?? "";

    public string AddShownRowsToReviewShortlistButtonToolTipValue => AddShownToShortlistButton.ToolTip?.ToString() ?? "";

    public string AddShownRowsToReviewShortlistButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(AddShownToShortlistButton);

    public string RemoveShownRowsFromReviewShortlistButtonToolTipValue => RemoveShownFromShortlistButton.ToolTip?.ToString() ?? "";

    public string RemoveShownRowsFromReviewShortlistButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(RemoveShownFromShortlistButton);

    public bool CanPreviewQuarantine => PreviewQuarantineButton.IsEnabled;

    public string ExportShortlistButtonToolTipValue => ExportShortlistCsvButton.ToolTip?.ToString() ?? "";

    public string ExportShortlistButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(ExportShortlistCsvButton);

    public string ClearShortlistButtonToolTipValue => ClearShortlistButton.ToolTip?.ToString() ?? "";

    public string ClearShortlistButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(ClearShortlistButton);

    public string PreviewQuarantineButtonToolTipValue => PreviewQuarantineButton.ToolTip?.ToString() ?? "";

    public string PreviewQuarantineButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(PreviewQuarantineButton);

    public string PreviewQuarantineButtonText => PreviewQuarantineButton.Content?.ToString() ?? "";

    public bool CanExportQuarantinePreview => ExportQuarantinePreviewButton.IsEnabled;

    public string ExportQuarantinePreviewButtonToolTipValue => ExportQuarantinePreviewButton.ToolTip?.ToString() ?? "";

    public string ExportQuarantinePreviewButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(ExportQuarantinePreviewButton);

    public bool CanEnterQuarantineConfirmation => QuarantineConfirmationBox.IsEnabled;

    public string CurrentQuarantineConfirmationText => QuarantineConfirmationBox.Text;

    public bool CanExecuteQuarantine => ExecuteQuarantineButton.IsEnabled;

    public bool CanEditQuarantineRoot => QuarantineRootBox.IsEnabled;

    public string QuarantineRootBoxToolTipValue => QuarantineRootBox.ToolTip?.ToString() ?? "";

    public string QuarantineRootBoxAutomationHelpTextValue => AutomationProperties.GetHelpText(QuarantineRootBox);

    public bool CanBrowseQuarantineRoot => BrowseQuarantineRootButton.IsEnabled;

    public string BrowseQuarantineRootButtonText => BrowseQuarantineRootButton.Content?.ToString() ?? "";

    public string BrowseQuarantineRootButtonToolTipValue => BrowseQuarantineRootButton.ToolTip?.ToString() ?? "";

    public string BrowseQuarantineRootButtonAutomationHelpTextValue => AutomationProperties.GetHelpText(BrowseQuarantineRootButton);

    public bool ReviewToolbarsUseWrappingLayout =>
        ReviewFilterToolbar is WrapPanel
        && ReviewActionToolbar is WrapPanel
        && ReviewShortlistToolbar is WrapPanel;

    private async void ScanButton_Click(object sender, RoutedEventArgs e)
    {
        var scopePath = ScopePathBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(scopePath))
        {
            MessageBox.Show(this, "Choose a Cleanup Scope before scanning.", "Storage Scan", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var scanGate = CleanupScopeScanGateBuilder.Build(scopePath, RealProfilePreflightCheckBox.IsChecked == true);
        if (!scanGate.CanScan)
        {
            MessageBox.Show(this, scanGate.Message, "Storage Scan", MessageBoxButton.OK, MessageBoxImage.Warning);
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

    private void BrowseScopeButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Choose Cleanup Scope",
            InitialDirectory = GetInitialBrowseDirectory()
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        ScopePathBox.Text = dialog.FolderName;
        StatusText.Text = "Cleanup Scope selected. Click Scan to start a read-only Storage Scan.";
    }

    private void BrowseQuarantineRootButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Choose Quarantine Root",
            InitialDirectory = GetInitialQuarantineRootBrowseDirectory()
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        var hadPreview = _currentQuarantinePreview is not null;
        QuarantineRootBox.Text = dialog.FolderName;
        StatusText.Text = hadPreview
            ? "Quarantine root selected for preview. Recreate Quarantine Preview to review destinations. No folders were created and no files were modified."
            : "Quarantine root selected for preview. No folders were created and no files were modified.";
        if (hadPreview)
        {
            UpdateQuarantinePreviewStatus("Quarantine Root changed. Recreate Quarantine Preview to review destinations. No folders were created and no files were modified.");
        }
    }

    private void DiscoverQuarantineManifestsButton_Click(object sender, RoutedEventArgs e)
    {
        DiscoverQuarantineManifestsForCurrentRoot();
    }

    private void PreviewRestoreReadinessButton_Click(object sender, RoutedEventArgs e)
    {
        PreviewRestoreReadinessForCurrentRoot();
    }

    private void PreviewSelectedRestoreManifestReadinessButton_Click(object sender, RoutedEventArgs e)
    {
        PreviewSelectedRestoreManifestReadiness();
    }

    private void PreviewSelectedRestoreGateButton_Click(object sender, RoutedEventArgs e)
    {
        PreviewSelectedRestoreGateForCurrentSelection();
    }

    public async Task RunStorageScanForCurrentScopeAsync(CancellationToken cancellationToken = default)
    {
        var scopePath = ScopePathBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(scopePath))
        {
            throw new InvalidOperationException("Choose a Cleanup Scope before scanning.");
        }

        var scanGate = CleanupScopeScanGateBuilder.Build(scopePath, RealProfilePreflightCheckBox.IsChecked == true);
        if (!scanGate.CanScan)
        {
            throw new InvalidOperationException(scanGate.Message);
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
        _currentEntryTypeFilter = StorageEntryTypeFilter.All;
        _currentSizeThresholdFilter = StorageSizeThresholdFilter.All;
        _currentSearch = StorageReviewSearch.Empty;
        _currentDisplayStartIndex = 0;
        _shortlist.Clear();
        ClearQuarantinePreview();
        SetSearchTextSilently("");
        UpdateCategoryFilterOptions();
        UpdateEntryTypeFilterOptions();
        UpdateSizeThresholdFilterOptions();
        var matchedEntries = ApplyCurrentReviewFilters();
        var rows = BuildDisplayedRows(matchedEntries);

        ResultsGrid.ItemsSource = rows;
        _isShowingQuarantinedRows = false;
        RefreshQuarantinedRowsIfVisible();
        UpdateMainGridVisibility();
        TotalSizeText.Text = result.TotalSizeDisplay;
        FolderCountText.Text = result.FolderCount.ToString("N0");
        FileCountText.Text = result.FileCount.ToString("N0");
        AccessIssueCountText.Text = result.InaccessibleCount.ToString("N0");
        StatusText.Text = CanUndoCurrentQuarantineExecution()
            ? $"{FormatScanCompletedStatus(matchedEntries.Count)} Undo fixture quarantine remains available in the Quarantine shortlist area."
            : FormatScanCompletedStatus(matchedEntries.Count);
        UpdateFilterButtons();
        UpdateFilterSummary(matchedEntries);
        UpdateReviewMix();
        UpdateSafetySummary();
        UpdateShortlistControls();
        UpdateQuarantinedViewControls();

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

        RealProfilePreflightCheckBox.IsChecked = false;
        UpdateCleanupScopeSafetyNote();
    }

    private void RealProfilePreflightCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        UpdateCleanupScopeSafetyNote();
    }

    public void ConfirmRealProfilePreflightForRealProfileScan()
    {
        RealProfilePreflightCheckBox.IsChecked = true;
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

    private void ShowQuarantinedButton_Click(object sender, RoutedEventArgs e)
    {
        ShowQuarantinedRows();
    }

    private void BackToScanRowsButton_Click(object sender, RoutedEventArgs e)
    {
        ShowScanRows();
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
            StatusText.Text = $"All visible rows are already in Review Shortlist ({_shortlist.Count:N0}). No files were modified.";
            UpdateShortlistControls();
            return;
        }

        ClearQuarantinePreview();
        var selectedPath = _selectedRow?.FullPath;
        RefreshResults(selectedPath);
        StatusText.Text =
            $"Shortlisted {addedCount:N0} visible row(s) ({_shortlist.Count:N0} total). " +
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
            StatusText.Text = $"No visible rows were in Review Shortlist ({_shortlist.Count:N0}). No files were modified.";
            UpdateShortlistControls();
            return;
        }

        ClearQuarantinePreview();
        var selectedPath = _selectedRow?.FullPath;
        RefreshResults(selectedPath);
        StatusText.Text =
            $"Removed {removedCount:N0} visible row(s) from Review Shortlist ({_shortlist.Count:N0} total). " +
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
        var quarantineRootNote = QuarantineRootSafetyNoteBuilder.Build(QuarantineRootBox.Text);
        if (!quarantineRootNote.CanPreview)
        {
            ReportInvalidQuarantineRootPath(quarantineRootNote);
            return;
        }

        _currentQuarantinePreview = QuarantinePreviewBuilder.Build(
            shortlistedRows,
            _currentCleanupScopePath,
            quarantineRootNote.RootPath);
        _currentRestoreManifestDraft = RestoreManifestDraftBuilder.Build(
            _currentQuarantinePreview,
            DateTimeOffset.UtcNow,
            BuildDraftId("restore-manifest-draft"));
        _currentQuarantineConfirmationDraft = QuarantineConfirmationDraftBuilder.Build(
            _currentQuarantinePreview,
            _currentRestoreManifestDraft,
            DateTimeOffset.UtcNow,
            BuildDraftId("quarantine-confirmation-draft"),
            IsFixtureQuarantineExecutionAvailable());
        _currentQuarantineActionDraft = _currentQuarantineConfirmationDraft.HasDataBlockers
            ? null
            : QuarantineActionDraftBuilder.Build(
                _currentQuarantinePreview,
                _currentRestoreManifestDraft,
                _currentQuarantineConfirmationDraft,
                DateTimeOffset.UtcNow,
                BuildDraftId("quarantine-action-draft"));
        _currentRestoreManifest = _currentQuarantineActionDraft is null
            ? null
            : RestoreManifestBuilder.BuildPlanned(
                _currentQuarantineActionDraft,
                _currentRestoreManifestDraft,
                DateTimeOffset.UtcNow,
                BuildDraftId("restore-manifest"));
        SetQuarantineConfirmationTextSilently("");
        UpdateQuarantineExecutionGate();

        QuarantinePreviewText.Text = FormatQuarantinePreview(
            _currentQuarantinePreview,
            _currentRestoreManifestDraft,
            _currentQuarantineConfirmationDraft);
        ExportQuarantinePreviewButton.IsEnabled = ScanButton.IsEnabled;
        var blockerSummary = _currentQuarantineConfirmationDraft.HasDataBlockers
            ? $"{_currentQuarantineConfirmationDraft.Blockers.Count:N0} readiness blocker(s)"
            : "no readiness blockers";
        StatusText.Text =
            $"Quarantine Preview created from Review Shortlist: {_currentQuarantinePreview.IncludedCount:N0} included, " +
            $"{_currentQuarantinePreview.BlockedCount:N0} blocked, " +
            $"{_currentQuarantinePreview.RedundantCount:N0} redundant, " +
            $"{_currentQuarantinePreview.IncludedSizeDisplay} previewed, {blockerSummary}. No files were modified.";
        UpdateQuarantinePreviewStatus();
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
        _currentDisplayStartIndex = 0;
        RefreshResults();
    }

    private void EntryTypeFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isUpdatingEntryTypeFilterOptions || EntryTypeFilterBox.SelectedItem is not EntryTypeFilterOption option)
        {
            return;
        }

        _currentEntryTypeFilter = option.Filter;
        _currentDisplayStartIndex = 0;
        RefreshResults();
    }

    private void SizeThresholdFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isUpdatingSizeThresholdFilterOptions || SizeThresholdFilterBox.SelectedItem is not SizeThresholdFilterOption option)
        {
            return;
        }

        _currentSizeThresholdFilter = option.Filter;
        _currentDisplayStartIndex = 0;
        RefreshResults();
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isUpdatingSearchBox || _currentReview is null)
        {
            return;
        }

        _searchDebounceTimer.Stop();
        _searchDebounceTimer.Start();
        StatusText.Text = "Search will apply after typing pauses. No files were modified.";
    }

    private void SearchDebounceTimer_Tick(object? sender, EventArgs e)
    {
        ApplySearchBoxTextFromPendingInput();
    }

    private void ApplySearchBoxTextFromPendingInput()
    {
        if (_currentReview is null)
        {
            _searchDebounceTimer.Stop();
            return;
        }

        var searchText = SearchBox.Text;
        ApplyStorageReviewSearch(searchText);
        StatusText.Text = _currentSearch.IsActive
            ? $"Search applied: \"{_currentSearch.Query}\". No files were modified."
            : "Search cleared. No files were modified.";
    }

    private void QuarantineRootBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isWindowInitialized)
        {
            return;
        }

        UpdateQuarantineRootSafetyNote();
        ClearQuarantineManifestDiscovery();
        if (_currentQuarantinePreview is null)
        {
            UpdateShortlistControls();
            return;
        }

        ClearQuarantinePreview();
        UpdateShortlistControls();
        UpdateQuarantinePreviewStatus("Quarantine root changed. Recreate Quarantine Preview to review destinations. No files were modified.");
        StatusText.Text = "Quarantine root changed. Recreate Quarantine Preview to review destinations. No files were modified.";
    }

    private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyStorageReviewSearch("");
    }

    private void ResetViewButton_Click(object sender, RoutedEventArgs e)
    {
        ResetReviewView();
    }

    private void PreviousReviewWindowButton_Click(object sender, RoutedEventArgs e)
    {
        ShowPreviousReviewWindow();
    }

    private void NextReviewWindowButton_Click(object sender, RoutedEventArgs e)
    {
        ShowNextReviewWindow();
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
            File.WriteAllText(dialog.FileName, StorageScanCsvExporter.Export(exportRows, _currentCleanupScopePath));
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
        return ApplyCurrentReviewFilters();
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
            File.WriteAllText(dialog.FileName, StorageScanCsvExporter.Export(exportRows, _currentCleanupScopePath));
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
            ShowEmptyStorageEntryDetails();
            UpdateShortlistControls();
            return;
        }

        ShowStorageEntryDetails(row);
        UpdateShortlistControls();
    }

    private void QuarantinedGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_isShowingQuarantinedRows)
        {
            return;
        }

        _selectedRow = null;
        DisableSelectedStorageRowActions();

        if (QuarantinedGrid.SelectedItem is not QuarantinedItemRow row)
        {
            ShowEmptyQuarantinedDetails();
            return;
        }

        DetailTitleText.Text = row.Name;
        DetailPathText.Text = row.OriginalPath;
        DetailPathContextText.Text =
            $"Original: {row.OriginalPath}\n" +
            $"Quarantine: {row.QuarantinePath}\n" +
            $"Restore Manifest: {row.ManifestPath}";
        DetailMetaText.Text = $"{row.Size} | {row.Type} | Restore Manifest entry status: {row.Status}";
        DetailEvidenceText.Text = "Current-session quarantined item from the in-memory Restore Manifest. This review view is read-only.";
        DetailGuidanceText.Text = "Use Undo fixture quarantine from the Quarantine shortlist area to restore current fixture entries when available. Use Back to scan rows to return to Storage Scan rows.";
        DetailSubtreeSummaryText.Text = "Quarantined rows do not have Storage Scan descendant summaries in this view.";
        DetailChildrenText.Text = "Quarantined rows do not show scanned child breakdowns in this view.";
        DetailHotspotTrailText.Text = "Quarantined rows do not show scanned hotspot trails in this view.";
        FilePreviewText.Text = "File preview is available from Storage Scan rows only. No files were modified.";
    }

    private void ShowStorageEntryDetails(StorageEntryRow row)
    {
        _selectedRow = row;
        DetailTitleText.Text = row.Entry.Name;
        DetailPathText.Text = row.FullPath;
        DetailPathContextText.Text = $"Relative: {row.RelativePath}\nParent: {row.ParentLocation}\nDepth: {row.Depth:N0} | Modified: {row.LastModified}\nContents: {row.Contents}";
        DetailMetaText.Text = $"{row.Size} | {row.Type} | {row.Importance} | {row.Recommendation} | Access: {row.AccessStatus}";
        DetailEvidenceText.Text = string.IsNullOrWhiteSpace(row.Error)
            ? row.Evidence
            : $"{row.Evidence}\n\nAccess issue: {row.Error}";
        DetailGuidanceText.Text = FormatSelectedPathReviewGuidance(row.Entry);
        DetailSubtreeSummaryText.Text = FormatStorageSubtreeReviewSummary(row.Entry, _currentCleanupScopePath);
        DetailChildrenText.Text = FormatChildSummary(row.Entry);
        DetailHotspotTrailText.Text = FormatStorageHotspotTrail(row.Entry);
        FilePreviewText.Text = row.Entry.IsDirectory
            ? "Folders do not have file content previews. Review Largest immediate children instead."
            : "Preview is loaded only when you use Preview file. No files were modified.";
    }

    private void ShowEmptyStorageEntryDetails()
    {
        DetailTitleText.Text = "Select a result";
        DetailPathText.Text = "";
        DetailPathContextText.Text = "";
        DetailMetaText.Text = "";
        DetailEvidenceText.Text = "";
        DetailGuidanceText.Text = "";
        DetailSubtreeSummaryText.Text = "";
        DetailChildrenText.Text = "";
        DetailHotspotTrailText.Text = "";
        FilePreviewText.Text = "Preview appears after selecting a file and using Preview file.";
        DisableSelectedStorageRowActions();
    }

    private void ShowEmptyQuarantinedDetails()
    {
        _selectedRow = null;
        DetailTitleText.Text = "No quarantined item selected";
        DetailPathText.Text = "";
        DetailPathContextText.Text = "Current-session quarantined rows appear after fixture Quarantine execution records Moved entries.";
        DetailMetaText.Text = "";
        DetailEvidenceText.Text = "This view is read-only and current-session-only. Use Discover manifests for older Restore Manifest review.";
        DetailGuidanceText.Text = "Use Back to scan rows to return to Storage Scan rows.";
        DetailSubtreeSummaryText.Text = "";
        DetailChildrenText.Text = "";
        DetailHotspotTrailText.Text = "";
        FilePreviewText.Text = "File preview is available from Storage Scan rows only. No files were modified.";
        DisableSelectedStorageRowActions();
    }

    private void DisableSelectedStorageRowActions()
    {
        AddToShortlistButton.IsEnabled = false;
        RemoveFromShortlistButton.IsEnabled = false;
        CopyPathButton.IsEnabled = false;
        ShowChildrenButton.IsEnabled = false;
        ShowDescendantsButton.IsEnabled = false;
        PreviewFileButton.IsEnabled = false;
        OpenInExplorerButton.IsEnabled = false;
    }

    private void CopyPathButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedRow is null)
        {
            return;
        }

        var plan = PathInspectionPlanBuilder.Build(_selectedRow.Entry);
        try
        {
            Clipboard.SetText(plan.PathToCopy);
            StatusText.Text = "Selected path copied. No files were modified.";
        }
        catch (COMException ex)
        {
            StatusText.Text = "Could not copy selected path because the Windows Clipboard was busy. Try Copy path again. No files were modified.";
            MessageBox.Show(this, ex.Message, "Copy path failed", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void ShowChildrenButton_Click(object sender, RoutedEventArgs e)
    {
        ShowSelectedFolderChildren();
    }

    private void ShowDescendantsButton_Click(object sender, RoutedEventArgs e)
    {
        ShowSelectedFolderDescendants();
    }

    public void ShowSelectedFolderChildren()
    {
        if (_currentReview is null || _selectedRow is null || !_selectedRow.Entry.IsDirectory)
        {
            return;
        }

        var selectedFolderPath = _selectedRow.FullPath;
        var selectedFolderName = _selectedRow.Entry.Name;
        _currentFilter = StorageReviewFilter.All;
        _currentCategoryFilter = StorageCategoryFilter.All;
        _currentEntryTypeFilter = StorageEntryTypeFilter.All;
        _currentSizeThresholdFilter = StorageSizeThresholdFilter.All;
        _currentSearch = StorageReviewSearch.FromText($"parent:{selectedFolderPath}");
        _currentDisplayStartIndex = 0;
        SelectCategoryFilterOption(_currentCategoryFilter);
        SelectEntryTypeFilterOption(_currentEntryTypeFilter);
        SelectSizeThresholdFilterOption(_currentSizeThresholdFilter);
        SetSearchTextSilently(_currentSearch.Query);
        RefreshResults();
        StatusText.Text = $"Focused review on immediate children of {selectedFolderName}. Use Reset view to show all rows again. No files were modified.";
    }

    public void ShowSelectedFolderDescendants()
    {
        if (_currentReview is null || _selectedRow is null || !_selectedRow.Entry.IsDirectory)
        {
            return;
        }

        var selectedFolderPath = _selectedRow.FullPath;
        var selectedFolderName = _selectedRow.Entry.Name;
        _currentFilter = StorageReviewFilter.All;
        _currentCategoryFilter = StorageCategoryFilter.All;
        _currentEntryTypeFilter = StorageEntryTypeFilter.All;
        _currentSizeThresholdFilter = StorageSizeThresholdFilter.All;
        _currentSearch = StorageReviewSearch.FromText($"under:{selectedFolderPath}");
        _currentDisplayStartIndex = 0;
        SelectCategoryFilterOption(_currentCategoryFilter);
        SelectEntryTypeFilterOption(_currentEntryTypeFilter);
        SelectSizeThresholdFilterOption(_currentSizeThresholdFilter);
        SetSearchTextSilently(_currentSearch.Query);
        RefreshResults();
        StatusText.Text = $"Focused review on descendants under {selectedFolderName}. No files were modified.";
    }

    private void PreviewFileButton_Click(object sender, RoutedEventArgs e)
    {
        PreviewSelectedFileContent();
    }

    public void PreviewSelectedFileContent()
    {
        if (_selectedRow is null || _selectedRow.Entry.IsDirectory)
        {
            return;
        }

        var preview = SelectedFileContentPreviewBuilder.Build(_selectedRow.Entry);
        FilePreviewText.Text = FormatSelectedFileContentPreview(preview);
        StatusText.Text = preview.IsContentShown
            ? "Selected file preview loaded. No files were modified."
            : "Selected file preview unavailable. No files were modified.";
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
        _isScanning = isScanning;
        CancelButton.IsEnabled = isScanning;
        ScopePathBox.IsEnabled = !isScanning;
        BrowseScopeButton.IsEnabled = !isScanning;
        QuarantineRootBox.IsEnabled = !isScanning;
        BrowseQuarantineRootButton.IsEnabled = !isScanning;
        DiscoverQuarantineManifestsButton.IsEnabled = !isScanning;
        UpdateCleanupScopeSafetyNote();
        ExportCsvButton.IsEnabled = !isScanning && _currentReview is not null;
        ExportShortlistCsvButton.IsEnabled = !isScanning && _currentReview is not null && _shortlist.Count > 0;
        ClearShortlistButton.IsEnabled = !isScanning && _shortlist.Count > 0;
        PreviewQuarantineButton.IsEnabled = !isScanning && _currentReview is not null && _shortlist.Count > 0 && CanUseQuarantineRootForPreview();
        ExportQuarantinePreviewButton.IsEnabled = !isScanning && _currentQuarantinePreview is not null && _currentQuarantineExecutionResult is null;
        CategoryFilterBox.IsEnabled = !isScanning && _currentReview is not null && CategoryFilterBox.Items.Count > 1;
        EntryTypeFilterBox.IsEnabled = !isScanning && _currentReview is not null;
        SizeThresholdFilterBox.IsEnabled = !isScanning && _currentReview is not null;
        SearchBox.IsEnabled = !isScanning && _currentReview is not null;
        ClearSearchButton.IsEnabled = !isScanning && _currentReview is not null && _currentSearch.IsActive;
        ResetViewButton.IsEnabled = !isScanning && _currentReview is not null && IsReviewViewFiltered();
        var matchedEntries = ApplyCurrentReviewFilters();
        PreviousReviewWindowButton.IsEnabled = !isScanning && _currentReview is not null && _currentDisplayStartIndex > 0;
        NextReviewWindowButton.IsEnabled = !isScanning && _currentReview is not null && _currentDisplayStartIndex + MaxDisplayedRows < matchedEntries.Count;
        UpdateShortlistControls();
        UpdateSafetyShortcutButtons();
        UpdateQuarantineExecutionGate();
        UpdateQuarantineManifestDiscoveryControls();
        UpdateQuarantinedViewControls();
    }

    private string? GetInitialBrowseDirectory()
    {
        try
        {
            var currentPath = ScopePathBox.Text.Trim();
            return Directory.Exists(currentPath)
                ? Path.GetFullPath(currentPath)
                : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
        catch (ArgumentException)
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
        catch (NotSupportedException)
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
    }

    private string? GetInitialQuarantineRootBrowseDirectory()
    {
        try
        {
            var currentPath = QuarantineRootBox.Text.Trim();
            if (Directory.Exists(currentPath))
            {
                return Path.GetFullPath(currentPath);
            }

            var note = QuarantineRootSafetyNoteBuilder.Build(currentPath);
            var root = note.CanPreview ? Path.GetPathRoot(note.RootPath) : null;
            return !string.IsNullOrWhiteSpace(root) && Directory.Exists(root)
                ? root
                : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
        catch (ArgumentException)
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
        catch (NotSupportedException)
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
    }

    private void UpdateCleanupScopeSafetyNote()
    {
        if (ScopeSafetyNoteText is null)
        {
            return;
        }

        var note = CleanupScopeSafetyNoteBuilder.Build(ScopePathBox.Text);
        var scanGate = CleanupScopeScanGateBuilder.Build(ScopePathBox.Text, RealProfilePreflightCheckBox.IsChecked == true);
        var safetyNoteText = $"{note.Label}: {note.Message}";
        ScopeSafetyNoteText.Text = safetyNoteText;
        var safetyNoteHelpText = FormatCleanupScopeSafetyNoteHelpText(safetyNoteText, note);
        ScopeSafetyNoteText.ToolTip = safetyNoteHelpText;
        AutomationProperties.SetHelpText(ScopeSafetyNoteText, safetyNoteHelpText);
        CleanupScopeSafetyNoteHelpCue.ToolTip = safetyNoteHelpText;
        AutomationProperties.SetHelpText(CleanupScopeSafetyNoteHelpCue, safetyNoteHelpText);
        var scanGateSummary = FormatScanGateSummary(note, scanGate);
        ScanGateSummaryText.Text = scanGateSummary;
        ScanGateSummaryText.Foreground = scanGate.CanScan
            ? System.Windows.Media.Brushes.DarkGreen
            : System.Windows.Media.Brushes.DarkGoldenrod;
        ScanGateText.Text = scanGate.Message;
        var scanButtonHelpText = FormatScanButtonToolTip(note, scanGate);
        ScanButton.ToolTip = scanButtonHelpText;
        AutomationProperties.SetHelpText(ScanButton, scanButtonHelpText);
        var scanGateSummaryHelpText = FormatScanGateSummaryHelpText(scanGateSummary, note, scanGate);
        ScanGateSummaryText.ToolTip = scanGateSummaryHelpText;
        AutomationProperties.SetHelpText(ScanGateSummaryText, scanGateSummaryHelpText);
        ScanGateSummaryHelpCue.ToolTip = scanGateSummaryHelpText;
        AutomationProperties.SetHelpText(ScanGateSummaryHelpCue, scanGateSummaryHelpText);
        RealProfilePreflightPanel.Visibility = note.IsRealUserProfileScope ? Visibility.Visible : Visibility.Collapsed;
        RealProfilePreflightCheckBox.IsEnabled = !_isScanning && note.IsRealUserProfileScope;
        ScanButton.IsEnabled = !_isScanning && scanGate.CanScan;
    }

    private static string FormatCleanupScopeSafetyNoteHelpText(string safetyNoteText, CleanupScopeSafetyNote note)
    {
        var boundary = note.IsRealUserProfileScope
            ? "This Cleanup Scope Safety Note is read-only scope context; it does not run MVP preflight, create fixtures, start scanning by itself, persist approval, move files, or approve cleanup."
            : note.IsFixtureScope
                ? "This Cleanup Scope Safety Note is read-only scope context; it does not create fixtures, start scanning by itself, move files, or approve cleanup. Fixture cleanup actions still require preview and exact confirmation."
                : "This Cleanup Scope Safety Note is read-only scope context; it does not start scanning by itself, move files, or approve cleanup. Verify the path before scanning real user files.";

        return $"{safetyNoteText} {boundary}";
    }

    private static string FormatScanGateSummary(CleanupScopeSafetyNote note, CleanupScopeScanGate scanGate)
    {
        if (note.IsRealUserProfileScope && !scanGate.CanScan)
        {
            return "Scan locked for real profile: run MVP preflight, review the fixture, then tick the acknowledgement.";
        }

        if (note.IsRealUserProfileScope)
        {
            return "Scan ready for real profile: read-only Storage Scan; cleanup execution remains unavailable.";
        }

        if (note.IsFixtureScope)
        {
            return "Scan ready for fixture Cleanup Scope: read-only scan first; fixture cleanup actions stay gated.";
        }

        if (scanGate.CanScan)
        {
            return "Scan ready for custom Cleanup Scope: read-only scan; cleanup execution remains unavailable.";
        }

        return "Scan waiting for a valid Cleanup Scope.";
    }

    private static string FormatScanGateSummaryHelpText(
        string summary,
        CleanupScopeSafetyNote note,
        CleanupScopeScanGate scanGate)
    {
        var boundary = note.IsRealUserProfileScope
            ? "This scan-gate status is read-only; it does not run preflight, create fixtures, start scanning by itself, persist approval, or approve cleanup."
            : note.IsFixtureScope
                ? "This scan-gate status is read-only; it does not start scanning by itself, create fixtures, move files, or approve cleanup. Fixture cleanup actions still require preview and exact confirmation."
                : scanGate.CanScan
                    ? "This scan-gate status is read-only; it does not start scanning by itself, move files, or approve cleanup. Cleanup execution remains unavailable."
                    : "This scan-gate status is read-only; it does not start scanning, move files, or approve cleanup.";

        return $"{summary} {boundary}";
    }

    private static string FormatScanButtonToolTip(CleanupScopeSafetyNote note, CleanupScopeScanGate scanGate)
    {
        if (scanGate.CanScan)
        {
            if (note.IsRealUserProfileScope)
            {
                return "Starts a read-only Storage Scan for the acknowledged real profile; cleanup execution remains unavailable.";
            }

            if (note.IsFixtureScope)
            {
                return "Starts a read-only Storage Scan for the fixture; fixture cleanup actions still require preview and exact confirmation.";
            }

            return "Starts a read-only Storage Scan for the selected Cleanup Scope; cleanup execution remains unavailable.";
        }

        if (note.IsRealUserProfileScope)
        {
            return "Scan is locked until MVP preflight and fixture review are acknowledged.";
        }

        return scanGate.Message;
    }

    private void UpdateQuarantineRootSafetyNote()
    {
        if (QuarantineRootSafetyNoteText is null)
        {
            return;
        }

        var note = QuarantineRootSafetyNoteBuilder.Build(QuarantineRootBox.Text);
        var safetyNoteText = $"{note.Label}: {note.Message}";
        var helpText = $"{safetyNoteText} Quarantine Root Safety Note is read-only preview-root context; it does not create folders, move files, write manifests, or approve cleanup.";
        QuarantineRootSafetyNoteText.Text = safetyNoteText;
        QuarantineRootSafetyNoteText.ToolTip = helpText;
        AutomationProperties.SetHelpText(QuarantineRootSafetyNoteText, helpText);
        QuarantineRootSafetyNoteHelpCue.ToolTip = helpText;
        AutomationProperties.SetHelpText(QuarantineRootSafetyNoteHelpCue, helpText);
    }

    private void UpdateQuarantineExecutionGate()
    {
        if (QuarantineExecutionGateText is null)
        {
            return;
        }

        _currentQuarantineExecutionGate = QuarantineExecutionGateBuilder.Build(
            _currentQuarantineConfirmationDraft,
            QuarantineConfirmationBox.Text);
        var hasExecutedCurrentPreview = _currentQuarantineExecutionResult is not null;
        QuarantineConfirmationBox.IsEnabled = _currentQuarantineConfirmationDraft is not null && !hasExecutedCurrentPreview && ScanButton.IsEnabled;
        ExecuteQuarantineButton.IsEnabled = _currentQuarantineExecutionGate.CanExecute && !hasExecutedCurrentPreview && ScanButton.IsEnabled;
        UndoQuarantineButton.IsEnabled = CanUndoCurrentQuarantineExecution() && ScanButton.IsEnabled;
        QuarantineExecutionGateText.Text = FormatQuarantineExecutionGate(
            _currentQuarantineExecutionGate,
            _currentQuarantineActionDraft,
            _currentRestoreManifest,
            _currentQuarantineExecutionResult,
            _currentUndoQuarantineResult);
        var gateHelpText = FormatQuarantineExecutionGateHelpText(
            _currentQuarantineExecutionGate,
            _currentQuarantineExecutionResult,
            _currentUndoQuarantineResult);
        QuarantineExecutionGateText.ToolTip = gateHelpText;
        AutomationProperties.SetHelpText(QuarantineExecutionGateText, gateHelpText);
        QuarantineExecutionGateHelpCue.ToolTip = gateHelpText;
        AutomationProperties.SetHelpText(QuarantineExecutionGateHelpCue, gateHelpText);
        UpdateQuarantineShortlistHeader();
    }

    public void ApplyStorageReviewFilter(StorageReviewFilter filter)
    {
        if (_currentReview is null)
        {
            return;
        }

        _currentFilter = filter;
        _currentDisplayStartIndex = 0;
        RefreshResults();
    }

    public void ApplyBloatCategoryFilter(StorageCategoryFilter filter)
    {
        if (_currentReview is null)
        {
            return;
        }

        _currentCategoryFilter = filter;
        _currentDisplayStartIndex = 0;
        SelectCategoryFilterOption(_currentCategoryFilter);
        RefreshResults();
    }

    public void ApplyEntryTypeFilter(StorageEntryTypeFilter filter)
    {
        if (_currentReview is null)
        {
            return;
        }

        _currentEntryTypeFilter = filter;
        _currentDisplayStartIndex = 0;
        SelectEntryTypeFilterOption(_currentEntryTypeFilter);
        RefreshResults();
    }

    public void ApplySizeThresholdFilter(StorageSizeThresholdFilter filter)
    {
        if (_currentReview is null)
        {
            return;
        }

        _currentSizeThresholdFilter = filter;
        _currentDisplayStartIndex = 0;
        SelectSizeThresholdFilterOption(_currentSizeThresholdFilter);
        RefreshResults();
    }

    public void SelectEntryTypeFilterThroughCombo(StorageEntryTypeFilter filter)
    {
        var option = EntryTypeFilterBox.Items
            .Cast<EntryTypeFilterOption>()
            .FirstOrDefault(candidate => candidate.Filter == filter);
        if (option is not null)
        {
            EntryTypeFilterBox.SelectedItem = option;
        }
    }

    public void SelectSizeThresholdFilterThroughCombo(StorageSizeThresholdFilter filter)
    {
        var option = SizeThresholdFilterBox.Items
            .Cast<SizeThresholdFilterOption>()
            .FirstOrDefault(candidate => candidate.Filter == filter);
        if (option is not null)
        {
            SizeThresholdFilterBox.SelectedItem = option;
        }
    }

    public void SelectBloatCategoryFilterThroughCombo(StorageCategoryFilter filter)
    {
        var option = CategoryFilterBox.Items
            .Cast<CategoryFilterOption>()
            .FirstOrDefault(candidate => candidate.Filter == filter);
        if (option is not null)
        {
            CategoryFilterBox.SelectedItem = option;
        }
    }

    public void SetQuarantineRootForPreview(string quarantineRootPath)
    {
        QuarantineRootBox.Text = quarantineRootPath;
    }

    public void SetQuarantineConfirmationText(string confirmationText)
    {
        QuarantineConfirmationBox.Text = confirmationText;
    }

    public void SetSelectedRestoreConfirmationText(string confirmationText)
    {
        SelectedRestoreConfirmationBox.Text = confirmationText;
    }

    public bool SelectDiscoveredRestoreManifestByPath(string manifestPath)
    {
        foreach (var option in RestoreManifestSelectionBox.Items.OfType<RestoreManifestSelectionOption>())
        {
            if (SamePath(option.Summary.ManifestPath, manifestPath))
            {
                RestoreManifestSelectionBox.SelectedItem = option;
                return true;
            }
        }

        return false;
    }

    public void DiscoverQuarantineManifestsForCurrentRoot()
    {
        _currentQuarantineManifestDiscovery = QuarantineManifestDiscoveryBuilder.Discover(QuarantineRootBox.Text);
        _currentRestoreReadinessPreview = null;
        _currentSelectedRestoreManifestReview = null;
        ClearSelectedRestoreGate();
        QuarantineManifestDiscoveryText.Text = FormatQuarantineManifestDiscovery(_currentQuarantineManifestDiscovery);
        RestoreReadinessPreviewText.Text = "Read-only all-manifest readiness appears after using Preview all-manifest readiness.";
        SetRestoreManifestSelectionOptions(_currentQuarantineManifestDiscovery.Manifests);
        SelectedRestoreManifestReviewText.Text = _currentQuarantineManifestDiscovery.ManifestCount == 0
            ? "No discovered Restore Manifest is available for selected review."
            : "Select a discovered Restore Manifest, then use Preview selected manifest readiness. No restore action is available.";
        UpdateQuarantineManifestDiscoveryControls();

        StatusText.Text = $"Quarantine Manifest Discovery completed: {_currentQuarantineManifestDiscovery.ManifestCount:N0} manifest(s), {_currentQuarantineManifestDiscovery.Issues.Count:N0} issue(s). No files were modified.";
    }

    public void PreviewRestoreReadinessForCurrentRoot()
    {
        _currentRestoreReadinessPreview = RestoreReadinessPreviewBuilder.BuildForQuarantineRoot(QuarantineRootBox.Text);
        RestoreReadinessPreviewText.Text = FormatRestoreReadinessPreview(_currentRestoreReadinessPreview);
        UpdateQuarantineManifestDiscoveryControls();

        StatusText.Text = $"Restore Readiness Preview completed: {_currentRestoreReadinessPreview.ManifestCount:N0} manifest(s), {_currentRestoreReadinessPreview.RestorableEntryCount:N0} restorable entries, {_currentRestoreReadinessPreview.BlockedEntryCount:N0} blocked. No files were modified.";
    }

    public void PreviewSelectedRestoreManifestReadiness()
    {
        ClearSelectedRestoreGate();
        _currentSelectedRestoreManifestReview = _currentQuarantineManifestDiscovery is null
            ? SelectedRestoreManifestReviewBuilder.BuildMissingDiscovery(QuarantineRootBox.Text, SelectedRestoreManifestPath)
            : SelectedRestoreManifestReviewBuilder.Build(_currentQuarantineManifestDiscovery, SelectedRestoreManifestPath);
        SelectedRestoreManifestReviewText.Text = FormatSelectedRestoreManifestReview(_currentSelectedRestoreManifestReview);
        UpdateQuarantineManifestDiscoveryControls();

        StatusText.Text = _currentSelectedRestoreManifestReview.HasReadinessPreview
            ? $"Selected Restore Manifest Review completed: {_currentSelectedRestoreManifestReview.RestorableEntryCount:N0} restorable entries, {_currentSelectedRestoreManifestReview.BlockedEntryCount:N0} blocked. No files were modified."
            : $"Selected Restore Manifest Review completed with {_currentSelectedRestoreManifestReview.SelectionIssues.Count:N0} issue(s). No files were modified.";
    }

    public void PreviewSelectedRestoreGateForCurrentSelection()
    {
        _currentSelectedRestoreConfirmationDraft = SelectedRestoreConfirmationDraftBuilder.Build(
            _currentSelectedRestoreManifestReview,
            DateTimeOffset.UtcNow,
            BuildDraftId("selected-restore-confirmation"),
            isExecutionImplemented: IsSelectedRestoreExecutionAvailable());
        _currentSelectedRestoreExecutionGate = SelectedRestoreExecutionGateBuilder.Build(
            _currentSelectedRestoreConfirmationDraft,
            SelectedRestoreConfirmationBox.Text);
        SetSelectedRestoreExecutionGateText(
            FormatSelectedRestoreExecutionGate(
                _currentSelectedRestoreConfirmationDraft,
                _currentSelectedRestoreExecutionGate,
                _currentSelectedRestoreResult),
            _currentSelectedRestoreExecutionGate,
            _currentSelectedRestoreResult);
        UpdateQuarantineManifestDiscoveryControls();

        StatusText.Text = $"Selected Restore Confirmation Draft completed: {_currentSelectedRestoreConfirmationDraft.RestorableEntryCount:N0} restorable entries, {_currentSelectedRestoreConfirmationDraft.Blockers.Count:N0} blocker(s). No files were modified.";
    }

    public void ApplyStorageReviewSearch(string searchText)
    {
        if (_currentReview is null)
        {
            return;
        }

        _searchDebounceTimer.Stop();
        _currentSearch = StorageReviewSearch.FromText(searchText);
        _currentDisplayStartIndex = 0;
        SetSearchTextSilently(_currentSearch.Query);
        RefreshResults();
    }

    public void EnterStorageReviewSearchText(string searchText)
    {
        SearchBox.Text = searchText;
    }

    public void ApplyPendingStorageReviewSearch()
    {
        ApplySearchBoxTextFromPendingInput();
    }

    public void ResetReviewView()
    {
        if (_currentReview is null)
        {
            return;
        }

        _searchDebounceTimer.Stop();
        _currentFilter = StorageReviewFilter.All;
        _currentCategoryFilter = StorageCategoryFilter.All;
        _currentEntryTypeFilter = StorageEntryTypeFilter.All;
        _currentSizeThresholdFilter = StorageSizeThresholdFilter.All;
        _currentSearch = StorageReviewSearch.Empty;
        _currentDisplayStartIndex = 0;
        SelectCategoryFilterOption(_currentCategoryFilter);
        SelectEntryTypeFilterOption(_currentEntryTypeFilter);
        SelectSizeThresholdFilterOption(_currentSizeThresholdFilter);
        SetSearchTextSilently("");
        RefreshResults();
        StatusText.Text = "Review view reset. Review Shortlist was kept. No files were modified.";
    }

    public void ShowQuarantinedRows()
    {
        var rows = BuildCurrentQuarantinedRows();
        QuarantinedGrid.ItemsSource = rows;
        _isShowingQuarantinedRows = true;
        UpdateMainGridVisibility();
        UpdateQuarantinedViewControls();
        StatusText.Text = rows.Count == 0
            ? "No current-session quarantined items are available. Use Discover manifests for older Restore Manifest review. No files were modified."
            : $"Showing {rows.Count:N0} current-session quarantined item(s) from the current fixture Restore Manifest. Use Back to scan rows to return. No files were modified.";

        if (rows.Count > 0)
        {
            QuarantinedGrid.SelectedIndex = 0;
        }
        else
        {
            ShowEmptyQuarantinedDetails();
        }
    }

    public void ShowScanRows()
    {
        _isShowingQuarantinedRows = false;
        UpdateMainGridVisibility();
        UpdateQuarantinedViewControls();
        if (ResultsGrid.SelectedItem is StorageEntryRow row)
        {
            ShowStorageEntryDetails(row);
        }

        StatusText.Text = _currentReview is null
            ? "Storage Scan rows appear after a scan. No files were modified."
            : "Showing Storage Scan rows. No files were modified.";
    }

    public void SetQuarantineShortlistExpanded(bool isExpanded)
    {
        QuarantineShortlistExpander.IsExpanded = isExpanded;
    }

    public void SetSafetySummaryExpanded(bool isExpanded)
    {
        SafetySummaryExpander.IsExpanded = isExpanded;
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
        _currentDisplayStartIndex = 0;
        SelectCategoryFilterOption(_currentCategoryFilter);
        RefreshResults();
        StatusText.Text = $"Review shortcut applied: {shortcutFilter.Label}. No files were modified.";
    }

    public void ShowNextReviewWindow()
    {
        MoveReviewWindow(MaxDisplayedRows);
    }

    public void ShowPreviousReviewWindow()
    {
        MoveReviewWindow(-MaxDisplayedRows);
    }

    private void MoveReviewWindow(int offset)
    {
        if (_currentReview is null)
        {
            return;
        }

        var matchedEntries = ApplyCurrentReviewFilters();
        if (matchedEntries.Count == 0)
        {
            return;
        }

        _currentDisplayStartIndex += offset;
        ClampDisplayStartIndex(matchedEntries.Count);
        RefreshResults();
        StatusText.Text = $"Review window changed to {FormatReviewWindowRange(matchedEntries.Count)}. No files were modified.";
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

        var matchedEntries = ApplyCurrentReviewFilters();
        ClampDisplayStartIndex(matchedEntries.Count);
        var rows = BuildDisplayedRows(matchedEntries);
        ResultsGrid.ItemsSource = rows;
        UpdateFilterButtons();
        UpdateFilterSummary();
        UpdateShortlistControls();
        UpdateReviewGridModeText();

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

    private void RefreshQuarantinedRowsIfVisible()
    {
        if (!_isShowingQuarantinedRows)
        {
            return;
        }

        QuarantinedGrid.ItemsSource = BuildCurrentQuarantinedRows();
        UpdateReviewGridModeText();
        if (QuarantinedGrid.Items.Count > 0)
        {
            QuarantinedGrid.SelectedIndex = 0;
        }
        else
        {
            ShowEmptyQuarantinedDetails();
        }
    }

    private void UpdateMainGridVisibility()
    {
        ResultsGrid.Visibility = _isShowingQuarantinedRows ? Visibility.Collapsed : Visibility.Visible;
        QuarantinedGrid.Visibility = _isShowingQuarantinedRows ? Visibility.Visible : Visibility.Collapsed;
        UpdateReviewGridModeText();
    }

    private void UpdateReviewGridModeText()
    {
        if (ReviewGridModeText is null)
        {
            return;
        }

        var currentQuarantinedCount = BuildCurrentQuarantinedRows().Count;
        if (_isShowingQuarantinedRows)
        {
            var text = currentQuarantinedCount == 0
                ? "Main grid: Current-session quarantined items. No moved entries are available; use Back to scan rows to return. No files were modified."
                : $"Main grid: Current-session quarantined items ({currentQuarantinedCount:N0}). Read-only view from the current in-memory Restore Manifest; use Back to scan rows to return. No files were modified.";
            SetReviewGridModeStatus(
                text,
                currentQuarantinedCount == 0 ? ReviewGridModeStatusStyle.Warning : ReviewGridModeStatusStyle.Information);
            return;
        }

        if (_currentReview is null)
        {
            SetReviewGridModeStatus(
                "Main grid: Storage Scan rows appear after a scan. No files were modified.",
                ReviewGridModeStatusStyle.Neutral);
            return;
        }

        var matchedCount = ApplyCurrentReviewFilters().Count;
        var quarantinedHint = currentQuarantinedCount > 0
            ? $" {currentQuarantinedCount:N0} current-session quarantined item(s) are available with Current quarantined."
            : " Current-session quarantined rows appear after fixture Quarantine execution.";
        var staleHint = _currentQuarantineExecutionResult is not null && _currentUndoQuarantineResult is null
            ? " Scan rows may be stale after fixture Quarantine execution; rescan refreshes rows."
            : "";

        SetReviewGridModeStatus(
            $"Main grid: Storage Scan rows ({FormatReviewWindowRange(matchedCount)}).{quarantinedHint}{staleHint} No files were modified.",
            string.IsNullOrEmpty(staleHint) ? ReviewGridModeStatusStyle.Neutral : ReviewGridModeStatusStyle.Warning);
    }

    private void SetReviewGridModeStatus(string text, ReviewGridModeStatusStyle style)
    {
        ReviewGridModeText.Text = text;
        ReviewGridModeText.Tag = style.ToString();
        var helpText = FormatReviewGridModeHelpText(text, style);
        ReviewGridModeText.ToolTip = helpText;
        AutomationProperties.SetHelpText(ReviewGridModeText, helpText);
        ReviewGridModeHelpCue.ToolTip = helpText;
        AutomationProperties.SetHelpText(ReviewGridModeHelpCue, helpText);
        ReviewGridModeText.Foreground = style switch
        {
            ReviewGridModeStatusStyle.Information => System.Windows.Media.Brushes.DarkCyan,
            ReviewGridModeStatusStyle.Warning => System.Windows.Media.Brushes.DarkGoldenrod,
            _ => System.Windows.Media.Brushes.SlateGray
        };
        ReviewGridModeText.FontWeight = style == ReviewGridModeStatusStyle.Neutral
            ? FontWeights.Normal
            : FontWeights.SemiBold;
    }

    private static string FormatReviewGridModeHelpText(string text, ReviewGridModeStatusStyle style)
    {
        return $"{text} Status state: {FormatReviewGridModeStatusState(style)}. Grid mode status is read-only review context; it does not rescan, modify files, restore files, or approve cleanup.";
    }

    private enum ReviewGridModeStatusStyle
    {
        Neutral,
        Information,
        Warning
    }

    private static string FormatReviewGridModeStatusState(ReviewGridModeStatusStyle style)
    {
        return style switch
        {
            ReviewGridModeStatusStyle.Information => "information",
            ReviewGridModeStatusStyle.Warning => "warning",
            _ => "neutral"
        };
    }

    private void UpdateQuarantinedViewControls()
    {
        if (ShowQuarantinedButton is null)
        {
            return;
        }

        var currentQuarantinedCount = BuildCurrentQuarantinedRows().Count;
        ShowQuarantinedButton.Content = FormatShowQuarantinedButtonText(currentQuarantinedCount);
        ShowQuarantinedButton.IsEnabled = !_isScanning
            && !_isShowingQuarantinedRows
            && currentQuarantinedCount > 0;
        BackToScanRowsButton.IsEnabled = !_isScanning && _isShowingQuarantinedRows;
        UpdateQuarantinedViewHelpText(currentQuarantinedCount);
        UpdateReviewGridModeText();
    }

    private static string FormatShowQuarantinedButtonText(int currentQuarantinedCount)
    {
        return currentQuarantinedCount > 0
            ? $"Current quarantined ({currentQuarantinedCount:N0})"
            : "Current quarantined";
    }

    private void UpdateQuarantinedViewHelpText(int currentQuarantinedCount)
    {
        var showHelpText = BuildShowQuarantinedHelpText(currentQuarantinedCount);
        ShowQuarantinedButton.ToolTip = showHelpText;
        AutomationProperties.SetHelpText(ShowQuarantinedButton, showHelpText);

        var backHelpText = _isShowingQuarantinedRows
            ? "Returns the main grid to completed Storage Scan rows without rescanning or modifying files. Current-session fixture undo remains separate."
            : "Back to scan rows becomes available only while the main grid is showing current-session quarantined items. No files are modified.";
        BackToScanRowsButton.ToolTip = backHelpText;
        AutomationProperties.SetHelpText(BackToScanRowsButton, backHelpText);
    }

    private string BuildShowQuarantinedHelpText(int currentQuarantinedCount)
    {
        if (_isScanning)
        {
            return "Current-session quarantined items can be reviewed after the read-only Storage Scan is not running. No files are modified.";
        }

        if (_isShowingQuarantinedRows)
        {
            return currentQuarantinedCount > 0
                ? "Already showing current-session quarantined Restore Manifest entries in the main grid. Use Back to scan rows to return; this view is read-only."
                : "Already showing current-session quarantined items, but no moved entries are available. Use Back to scan rows to return. No files are modified.";
        }

        if (currentQuarantinedCount > 0)
        {
            return $"Shows {currentQuarantinedCount:N0} current-session quarantined Restore Manifest item(s) in the main grid. This is read-only and does not restore, move, delete, discover older manifests, or create cleanup history.";
        }

        return "Current-session quarantined items appear after fixture Quarantine execution records moved Restore Manifest entries. Use Discover manifests for older Restore Manifest review; no files are restored, moved, deleted, or added to cleanup history.";
    }

    private IReadOnlyList<QuarantinedItemRow> BuildCurrentQuarantinedRows()
    {
        if (_currentRestoreManifest is null)
        {
            return [];
        }

        return _currentRestoreManifest.Entries
            .Where(entry => entry.Status == RestoreManifestEntryStatus.Moved)
            .Select(entry => new QuarantinedItemRow(entry, _currentRestoreManifest.ManifestPath))
            .ToArray();
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
            UndoQuarantineButton.IsEnabled = false;
            AddShownToShortlistButton.IsEnabled = false;
            RemoveShownFromShortlistButton.IsEnabled = false;
            SearchBox.IsEnabled = false;
            EntryTypeFilterBox.IsEnabled = false;
            SizeThresholdFilterBox.IsEnabled = false;
            ResetViewButton.IsEnabled = false;
            ClearSearchButton.IsEnabled = false;
            PreviousReviewWindowButton.IsEnabled = false;
            NextReviewWindowButton.IsEnabled = false;
            ReviewWindowText.Text = "Rows appear after a scan.";
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
        PreviewQuarantineButton.IsEnabled = _shortlist.Count > 0 && CanUseQuarantineRootForPreview();
        ExportQuarantinePreviewButton.IsEnabled = _currentQuarantinePreview is not null && _currentQuarantineExecutionResult is null;
        SearchBox.IsEnabled = true;
        SizeThresholdFilterBox.IsEnabled = true;
        ClearSearchButton.IsEnabled = _currentSearch.IsActive;
        ResetViewButton.IsEnabled = IsReviewViewFiltered();
    }

    private bool IsReviewViewFiltered()
    {
        return _currentFilter != StorageReviewFilter.All
            || _currentCategoryFilter.Kind != StorageCategoryFilterKind.All
            || _currentEntryTypeFilter != StorageEntryTypeFilter.All
            || _currentSizeThresholdFilter != StorageSizeThresholdFilter.All
            || _currentSearch.IsActive;
    }

    private IReadOnlyList<StorageReviewEntry> ApplyCurrentReviewFilters()
    {
        return _currentReview?.ApplyFilter(
            _currentFilter,
            _currentCategoryFilter,
            _currentEntryTypeFilter,
            _currentSizeThresholdFilter,
            _currentSearch) ?? [];
    }

    private StorageEntryRow[] BuildDisplayedRows(IReadOnlyList<StorageReviewEntry> entries)
    {
        ClampDisplayStartIndex(entries.Count);
        return entries
            .Skip(_currentDisplayStartIndex)
            .Take(MaxDisplayedRows)
            .Select(entry => new StorageEntryRow(entry, _shortlist.Contains(entry.Entry), _currentCleanupScopePath))
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

    private void UpdateEntryTypeFilterOptions()
    {
        _isUpdatingEntryTypeFilterOptions = true;
        try
        {
            if (_currentReview is null)
            {
                EntryTypeFilterBox.ItemsSource = new[]
                {
                    new EntryTypeFilterOption(StorageEntryTypeFilter.All, "All types", "all-types")
                };
                EntryTypeFilterBox.SelectedIndex = 0;
                EntryTypeFilterBox.IsEnabled = false;
                return;
            }

            var fileCount = _currentReview.Entries.Count(row => !row.Entry.IsDirectory);
            var folderCount = _currentReview.Entries.Count(row => row.Entry.IsDirectory);
            EntryTypeFilterBox.ItemsSource = new[]
            {
                new EntryTypeFilterOption(StorageEntryTypeFilter.All, "All types", "all-types"),
                new EntryTypeFilterOption(StorageEntryTypeFilter.Files, $"Files ({fileCount:N0})", "files"),
                new EntryTypeFilterOption(StorageEntryTypeFilter.Folders, $"Folders ({folderCount:N0})", "folders")
            };
            SelectEntryTypeFilterOption(_currentEntryTypeFilter);
            EntryTypeFilterBox.IsEnabled = true;
        }
        finally
        {
            _isUpdatingEntryTypeFilterOptions = false;
        }
    }

    private void UpdateSizeThresholdFilterOptions()
    {
        _isUpdatingSizeThresholdFilterOptions = true;
        try
        {
            if (_currentReview is null)
            {
                SizeThresholdFilterBox.ItemsSource = new[]
                {
                    new SizeThresholdFilterOption(StorageSizeThresholdFilter.All, "All sizes", "all-sizes")
                };
                SizeThresholdFilterBox.SelectedIndex = 0;
                SizeThresholdFilterBox.IsEnabled = false;
                return;
            }

            SizeThresholdFilterBox.ItemsSource = new[]
            {
                BuildSizeThresholdFilterOption(StorageSizeThresholdFilter.All, "All sizes", "all-sizes"),
                BuildSizeThresholdFilterOption(StorageSizeThresholdFilter.AtLeast1Mb, "1 MB+", "size-1mb-plus"),
                BuildSizeThresholdFilterOption(StorageSizeThresholdFilter.AtLeast100Mb, "100 MB+", "size-100mb-plus"),
                BuildSizeThresholdFilterOption(StorageSizeThresholdFilter.AtLeast1Gb, "1 GB+", "size-1gb-plus"),
                BuildSizeThresholdFilterOption(StorageSizeThresholdFilter.AtLeast5Gb, "5 GB+", "size-5gb-plus"),
                BuildSizeThresholdFilterOption(StorageSizeThresholdFilter.AtLeast10Gb, "10 GB+", "size-10gb-plus")
            };
            SelectSizeThresholdFilterOption(_currentSizeThresholdFilter);
            SizeThresholdFilterBox.IsEnabled = true;
        }
        finally
        {
            _isUpdatingSizeThresholdFilterOptions = false;
        }
    }

    private SizeThresholdFilterOption BuildSizeThresholdFilterOption(
        StorageSizeThresholdFilter filter,
        string label,
        string fileNameSegment)
    {
        var minimumSizeBytes = filter.GetMinimumSizeBytes();
        if (_currentReview is null || minimumSizeBytes <= 0)
        {
            return new SizeThresholdFilterOption(filter, label, fileNameSegment);
        }

        var count = _currentReview.Entries.Count(row => row.Entry.SizeBytes >= minimumSizeBytes);
        return new SizeThresholdFilterOption(filter, $"{label} ({count:N0})", fileNameSegment);
    }

    private void ClampDisplayStartIndex(int matchedCount)
    {
        if (matchedCount <= 0)
        {
            _currentDisplayStartIndex = 0;
            return;
        }

        var lastWindowStart = ((matchedCount - 1) / MaxDisplayedRows) * MaxDisplayedRows;
        _currentDisplayStartIndex = Math.Clamp(_currentDisplayStartIndex, 0, lastWindowStart);
    }

    private string FormatReviewWindowRange(int matchedCount)
    {
        if (matchedCount <= 0)
        {
            return "0 matched rows";
        }

        ClampDisplayStartIndex(matchedCount);
        var start = _currentDisplayStartIndex + 1;
        var end = Math.Min(_currentDisplayStartIndex + MaxDisplayedRows, matchedCount);
        return $"rows {start:N0}-{end:N0} of {matchedCount:N0} matched";
    }

    private void UpdateReviewWindowControls(int matchedCount)
    {
        ClampDisplayStartIndex(matchedCount);
        ReviewWindowText.Text = _currentReview is null
            ? "Rows appear after a scan."
            : FormatReviewWindowRange(matchedCount);
        var canUseWindowControls = _currentReview is not null && ScanButton.IsEnabled;
        PreviousReviewWindowButton.IsEnabled = canUseWindowControls && _currentDisplayStartIndex > 0;
        NextReviewWindowButton.IsEnabled = canUseWindowControls && _currentDisplayStartIndex + MaxDisplayedRows < matchedCount;
    }

    private void UpdateFilterSummary(IReadOnlyList<StorageReviewEntry>? currentMatchedEntries = null)
    {
        if (_currentReview is null)
        {
            FilterSummaryText.Text = "No scan loaded";
            SetMatchedReviewMixText("Matched review mix appears after a scan.");
            UpdateShortlistSafetyMix();
            UpdateReviewWindowControls(0);
            return;
        }

        var matchedEntries = currentMatchedEntries ?? ApplyCurrentReviewFilters();
        ClampDisplayStartIndex(matchedEntries.Count);
        var largestMatchedBytes = matchedEntries.Select(row => row.Entry.SizeBytes).DefaultIfEmpty(0).Max();
        var categoryLabel = _currentCategoryFilter.Kind == StorageCategoryFilterKind.All ? "" : $" + {FormatCategoryFilter(_currentCategoryFilter)}";
        var typeLabel = _currentEntryTypeFilter == StorageEntryTypeFilter.All ? "" : $" + {FormatEntryTypeFilter(_currentEntryTypeFilter)}";
        var sizeLabel = _currentSizeThresholdFilter == StorageSizeThresholdFilter.All ? "" : $" + {FormatSizeThresholdFilter(_currentSizeThresholdFilter)}";
        var searchLabel = _currentSearch.IsActive ? $" + Search \"{_currentSearch.Query}\"" : "";
        var limitLabel = matchedEntries.Count > MaxDisplayedRows
            ? $" Display window {MaxDisplayedRows:N0}; use Previous/Next rows or narrow filters/search to inspect more matches."
            : "";
        FilterSummaryText.Text =
            $"{FormatFilter(_currentFilter)}{categoryLabel}{typeLabel}{sizeLabel}{searchLabel}: {FormatReviewWindowRange(matchedEntries.Count)}, " +
            $"largest matched row {ByteSizeFormatter.Format(largestMatchedBytes)}, " +
            $"shortlist {_shortlist.Count:N0}.{limitLabel}";
        UpdateMatchedReviewMix(matchedEntries);
        UpdateReviewWindowControls(matchedEntries.Count);
    }

    private void UpdateMatchedReviewMix(IReadOnlyList<StorageReviewEntry> matchedEntries)
    {
        var likelySafeCount = matchedEntries.Count(row => row.Entry.ImportanceRating == ImportanceRating.LikelySafe);
        var cautionCount = matchedEntries.Count(row => row.Entry.ImportanceRating == ImportanceRating.Caution);
        var highRiskCount = matchedEntries.Count(row => row.Entry.ImportanceRating == ImportanceRating.HighRisk);
        var quarantineCandidateCount = matchedEntries.Count(row => row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate);
        var protectedLocationCount = matchedEntries.Count(row => row.Entry.BloatCategories.Contains(BloatCategory.ProtectedLocation));
        var accessIssueCount = matchedEntries.Count(row => !row.Entry.IsAccessible || row.Entry.BloatCategories.Contains(BloatCategory.AccessIssue));
        var uncategorizedCount = matchedEntries.Count(row => row.Entry.BloatCategories.Count == 0);

        SetMatchedReviewMixText(
            $"Matched review mix: {matchedEntries.Count:N0} rows | " +
            $"Likely safe {likelySafeCount:N0} | " +
            $"Caution {cautionCount:N0} | " +
            $"High risk {highRiskCount:N0} | " +
            $"Quarantine candidates {quarantineCandidateCount:N0} | " +
            $"Protected {protectedLocationCount:N0} | " +
            $"Access issues {accessIssueCount:N0} | " +
            $"No category {uncategorizedCount:N0}. Review context only, not cleanup approval.");
    }

    private void SetMatchedReviewMixText(string text)
    {
        MatchedReviewMixText.Text = text;
        var helpText = FormatMatchedReviewMixHelpText(text);
        MatchedReviewMixText.ToolTip = helpText;
        AutomationProperties.SetHelpText(MatchedReviewMixText, helpText);
        MatchedReviewMixHelpCue.ToolTip = helpText;
        AutomationProperties.SetHelpText(MatchedReviewMixHelpCue, helpText);
    }

    private static string FormatMatchedReviewMixHelpText(string text)
    {
        return $"{text} Matched Review Mix is read-only active-review-lens context; it does not rescan, modify files, prove storage savings, or approve cleanup. Counts cover all matched rows, not only the visible display window.";
    }

    private void UpdateShortlistSafetyMix()
    {
        if (_currentReview is null)
        {
            SetShortlistSafetyMixText("Review Shortlist is empty. Shortlist safety mix appears after rows are shortlisted.");
            UpdateQuarantineShortlistHeader();
            UpdateQuarantinePreviewStatus();
            return;
        }

        var shortlistedRows = _shortlist.ApplyTo(_currentReview.Entries);
        if (shortlistedRows.Count == 0)
        {
            SetShortlistSafetyMixText("Review Shortlist is empty. Add only recognized rows after inspection; shortlist is not cleanup approval.");
            UpdateQuarantineShortlistHeader();
            UpdateQuarantinePreviewStatus();
            return;
        }

        var likelySafeCount = shortlistedRows.Count(row => row.Entry.ImportanceRating == ImportanceRating.LikelySafe);
        var cautionCount = shortlistedRows.Count(row => row.Entry.ImportanceRating == ImportanceRating.Caution);
        var highRiskCount = shortlistedRows.Count(row => row.Entry.ImportanceRating == ImportanceRating.HighRisk);
        var quarantineCandidateCount = shortlistedRows.Count(row => row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate);
        var protectedLocationCount = shortlistedRows.Count(row => row.Entry.BloatCategories.Contains(BloatCategory.ProtectedLocation));
        var accessIssueCount = shortlistedRows.Count(row => !row.Entry.IsAccessible || row.Entry.BloatCategories.Contains(BloatCategory.AccessIssue));
        var uncategorizedCount = shortlistedRows.Count(row => row.Entry.BloatCategories.Count == 0);
        var largestShortlistedBytes = shortlistedRows.Select(row => row.Entry.SizeBytes).DefaultIfEmpty(0).Max();

        SetShortlistSafetyMixText(
            $"Shortlist safety mix: {FormatRowCount(shortlistedRows.Count)} | " +
            $"Likely safe {likelySafeCount:N0} | " +
            $"Caution {cautionCount:N0} | " +
            $"High risk {highRiskCount:N0} | " +
            $"Quarantine candidates {quarantineCandidateCount:N0} | " +
            $"Protected {protectedLocationCount:N0} | " +
            $"Access issues {accessIssueCount:N0} | " +
            $"No category {uncategorizedCount:N0} | " +
            $"Largest shortlisted row {ByteSizeFormatter.Format(largestShortlistedBytes)}. Review context only, not cleanup approval.");
        UpdateQuarantineShortlistHeader();
        UpdateQuarantinePreviewStatus();
    }

    private void SetShortlistSafetyMixText(string text)
    {
        ShortlistSafetyMixText.Text = text;
        var helpText = FormatShortlistSafetyMixHelpText(text);
        ShortlistSafetyMixText.ToolTip = helpText;
        AutomationProperties.SetHelpText(ShortlistSafetyMixText, helpText);
        ShortlistSafetyMixHelpCue.ToolTip = helpText;
        AutomationProperties.SetHelpText(ShortlistSafetyMixHelpCue, helpText);
    }

    private static string FormatShortlistSafetyMixHelpText(string text)
    {
        return $"{text} Review Shortlist Safety Mix is read-only review context; it does not rescan, modify files, prove Quarantine readiness, prove storage savings, or approve cleanup.";
    }

    private void UpdateQuarantineShortlistHeader()
    {
        if (QuarantineShortlistHeaderText is null)
        {
            return;
        }

        var previewText = _currentQuarantinePreview is null
            ? "no preview"
            : $"preview {_currentQuarantinePreview.IncludedCount:N0} included, {_currentQuarantinePreview.BlockedCount:N0} blocked";
        var movedCount = BuildCurrentQuarantinedRows().Count;
        var currentQuarantineText = movedCount > 0
            ? $"{movedCount:N0} current quarantined"
            : _currentUndoQuarantineResult is not null
                ? "undo completed"
                : "no current quarantine";
        var undoText = CanUndoCurrentQuarantineExecution()
            ? "undo available"
            : "undo unavailable";
        var headerText =
            $"Quarantine Shortlist: {_shortlist.Count:N0} shortlisted | {previewText} | {currentQuarantineText} | {undoText}";
        var style = GetQuarantineShortlistHeaderStatusStyle(movedCount);
        var helpText = $"{headerText}. Header state: {FormatQuarantineShortlistHeaderState(style)}. Header summary is read-only review context, not cleanup approval.";
        QuarantineShortlistHeaderText.Text = headerText;
        QuarantineShortlistHeaderText.Tag = style.ToString();
        QuarantineShortlistHeaderText.ToolTip = helpText;
        AutomationProperties.SetHelpText(QuarantineShortlistHeaderText, helpText);
        QuarantineShortlistHeaderHelpCue.ToolTip = helpText;
        AutomationProperties.SetHelpText(QuarantineShortlistHeaderHelpCue, helpText);
        QuarantineShortlistHeaderText.Foreground = style switch
        {
            QuarantineShortlistHeaderStatusStyle.Success => System.Windows.Media.Brushes.DarkGreen,
            QuarantineShortlistHeaderStatusStyle.Information => System.Windows.Media.Brushes.DarkCyan,
            QuarantineShortlistHeaderStatusStyle.Warning => System.Windows.Media.Brushes.DarkGoldenrod,
            _ => System.Windows.Media.Brushes.SlateGray
        };
    }

    private QuarantineShortlistHeaderStatusStyle GetQuarantineShortlistHeaderStatusStyle(int currentQuarantinedCount)
    {
        if (_currentUndoQuarantineResult is not null)
        {
            return _currentUndoQuarantineResult.Succeeded
                ? QuarantineShortlistHeaderStatusStyle.Success
                : QuarantineShortlistHeaderStatusStyle.Warning;
        }

        if (currentQuarantinedCount > 0)
        {
            return _currentQuarantineExecutionResult?.Succeeded == true
                ? QuarantineShortlistHeaderStatusStyle.Information
                : QuarantineShortlistHeaderStatusStyle.Warning;
        }

        if (_currentQuarantinePreview is not null && _currentQuarantineConfirmationDraft is not null)
        {
            return _currentQuarantineConfirmationDraft.HasDataBlockers
                ? QuarantineShortlistHeaderStatusStyle.Warning
                : QuarantineShortlistHeaderStatusStyle.Success;
        }

        return _shortlist.Count == 0
            ? QuarantineShortlistHeaderStatusStyle.Neutral
            : QuarantineShortlistHeaderStatusStyle.Warning;
    }

    private enum QuarantineShortlistHeaderStatusStyle
    {
        Neutral,
        Success,
        Information,
        Warning
    }

    private static string FormatQuarantineShortlistHeaderState(QuarantineShortlistHeaderStatusStyle style)
    {
        return style switch
        {
            QuarantineShortlistHeaderStatusStyle.Success => "ready or completed",
            QuarantineShortlistHeaderStatusStyle.Information => "current-session quarantined review",
            QuarantineShortlistHeaderStatusStyle.Warning => "needs review",
            _ => "neutral"
        };
    }

    private void UpdateQuarantinePreviewStatus(string? message = null)
    {
        if (QuarantinePreviewStatusText is null)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(message))
        {
            var style = message.Contains("could not be created", StringComparison.OrdinalIgnoreCase)
                ? QuarantinePreviewStatusStyle.Error
                : QuarantinePreviewStatusStyle.Warning;
            SetQuarantinePreviewStatus(message, style);
            return;
        }

        if (_currentUndoQuarantineResult is not null)
        {
            var result = _currentUndoQuarantineResult;
            var text = result.Succeeded
                ? $"Fixture Undo Quarantine completed: {result.RestoredCount:N0} restored. Rescan before further review."
                : $"Fixture Undo Quarantine needs recovery review: {result.RestoredCount:N0} restored, {result.FailedCount:N0} failed. Rescan before further review.";
            SetQuarantinePreviewStatus(text, result.Succeeded ? QuarantinePreviewStatusStyle.Success : QuarantinePreviewStatusStyle.Warning);
            return;
        }

        if (_currentQuarantineExecutionResult is not null)
        {
            var result = _currentQuarantineExecutionResult;
            var text = result.Succeeded
                ? $"Fixture Quarantine execution completed: {result.MovedCount:N0} included Review Shortlist row(s) moved, {result.RestoreManifest.TotalSizeDisplay} quarantined. Undo fixture quarantine remains available when enabled; rescan refreshes review rows."
                : $"Fixture Quarantine execution needs recovery review: {result.MovedCount:N0} included Review Shortlist row(s) moved, {result.FailedCount:N0} failed. Use Undo fixture quarantine when available; rescan refreshes review rows.";
            SetQuarantinePreviewStatus(text, result.Succeeded ? QuarantinePreviewStatusStyle.Success : QuarantinePreviewStatusStyle.Warning);
            return;
        }

        if (_currentQuarantinePreview is not null && _currentQuarantineConfirmationDraft is not null)
        {
            var blockerSummary = _currentQuarantineConfirmationDraft.HasDataBlockers
                ? $"{_currentQuarantineConfirmationDraft.Blockers.Count:N0} readiness blocker(s)"
                : "no readiness blockers";
            var readinessLabel = _currentQuarantineConfirmationDraft.HasDataBlockers
                ? "Quarantine Preview needs review"
                : "Quarantine Preview ready";
            var text =
                $"{readinessLabel}: {_currentQuarantinePreview.IncludedCount:N0} included, " +
                $"{_currentQuarantinePreview.BlockedCount:N0} blocked, " +
                $"{_currentQuarantinePreview.RedundantCount:N0} redundant, " +
                $"{_currentQuarantinePreview.IncludedSizeDisplay} previewed, {blockerSummary}. " +
                "Review Shortlist is the source; this is not cleanup approval. No files were modified.";
            SetQuarantinePreviewStatus(
                text,
                _currentQuarantineConfirmationDraft.HasDataBlockers ? QuarantinePreviewStatusStyle.Warning : QuarantinePreviewStatusStyle.Success);
            return;
        }

        var waitingText = _shortlist.Count == 0
            ? "Quarantine Preview readiness appears after Review Shortlist rows are added. No files were modified."
            : $"Review Shortlist has {_shortlist.Count:N0} row(s). Use Preview shortlist quarantine to review included, blocked, redundant, and readiness state. This is not cleanup approval. No files were modified.";
        SetQuarantinePreviewStatus(waitingText, _shortlist.Count == 0 ? QuarantinePreviewStatusStyle.Neutral : QuarantinePreviewStatusStyle.Warning);
    }

    private void SetQuarantinePreviewStatus(string text, QuarantinePreviewStatusStyle style)
    {
        QuarantinePreviewStatusText.Text = text;
        QuarantinePreviewStatusText.Tag = style.ToString();
        var helpText = FormatQuarantinePreviewStatusHelpText(text, style);
        QuarantinePreviewStatusText.ToolTip = helpText;
        AutomationProperties.SetHelpText(QuarantinePreviewStatusText, helpText);
        QuarantinePreviewStatusHelpCue.ToolTip = helpText;
        AutomationProperties.SetHelpText(QuarantinePreviewStatusHelpCue, helpText);
        QuarantinePreviewStatusText.Foreground = style switch
        {
            QuarantinePreviewStatusStyle.Success => System.Windows.Media.Brushes.DarkGreen,
            QuarantinePreviewStatusStyle.Warning => System.Windows.Media.Brushes.DarkGoldenrod,
            QuarantinePreviewStatusStyle.Error => System.Windows.Media.Brushes.Firebrick,
            _ => System.Windows.Media.Brushes.SlateGray
        };
        QuarantinePreviewStatusText.FontWeight = style == QuarantinePreviewStatusStyle.Neutral
            ? FontWeights.Normal
            : FontWeights.SemiBold;
    }

    private static string FormatQuarantinePreviewStatusHelpText(string text, QuarantinePreviewStatusStyle style)
    {
        return $"{text} Status state: {FormatQuarantinePreviewStatusState(style)}. Inline preview status is read-only review context; it does not create folders, move files, restore files, delete files, or approve cleanup.";
    }

    private enum QuarantinePreviewStatusStyle
    {
        Neutral,
        Success,
        Warning,
        Error
    }

    private static string FormatQuarantinePreviewStatusState(QuarantinePreviewStatusStyle style)
    {
        return style switch
        {
            QuarantinePreviewStatusStyle.Success => "success",
            QuarantinePreviewStatusStyle.Warning => "warning",
            QuarantinePreviewStatusStyle.Error => "error",
            _ => "neutral"
        };
    }

    private static string FormatRowCount(int count)
    {
        return count == 1 ? "1 row" : $"{count:N0} rows";
    }

    private string FormatScanCompletedStatus(int matchedCount)
    {
        return $"Storage Scan completed. Showing {FormatReviewWindowRange(matchedCount)}. No files were modified.";
    }

    private void UpdateShortlistControls()
    {
        var hasSelectedRow = _selectedRow is not null;
        var isShortlisted = hasSelectedRow && _shortlist.Contains(_selectedRow!.Entry);
        AddToShortlistButton.IsEnabled = hasSelectedRow && !isShortlisted && ScanButton.IsEnabled;
        RemoveFromShortlistButton.IsEnabled = hasSelectedRow && isShortlisted && ScanButton.IsEnabled;
        CopyPathButton.IsEnabled = hasSelectedRow && ScanButton.IsEnabled;
        ShowChildrenButton.IsEnabled = hasSelectedRow && _selectedRow!.Entry.IsDirectory && ScanButton.IsEnabled;
        ShowDescendantsButton.IsEnabled = hasSelectedRow && _selectedRow!.Entry.IsDirectory && ScanButton.IsEnabled;
        OpenInExplorerButton.IsEnabled = hasSelectedRow && ScanButton.IsEnabled;
        PreviewFileButton.IsEnabled = hasSelectedRow && !_selectedRow!.Entry.IsDirectory && ScanButton.IsEnabled;

        var hasShortlist = _shortlist.Count > 0;
        var hasExecutedCurrentPreview = _currentQuarantineExecutionResult is not null;
        var hasUnshortlistedDisplayedRows = DisplayedRows.Any(row => !_shortlist.Contains(row.Entry));
        var hasShortlistedDisplayedRows = DisplayedRows.Any(row => _shortlist.Contains(row.Entry));
        AddShownToShortlistButton.IsEnabled = _currentReview is not null && hasUnshortlistedDisplayedRows && ScanButton.IsEnabled;
        RemoveShownFromShortlistButton.IsEnabled = _currentReview is not null && hasShortlistedDisplayedRows && ScanButton.IsEnabled;
        ExportShortlistCsvButton.IsEnabled = _currentReview is not null && hasShortlist && ScanButton.IsEnabled;
        ClearShortlistButton.IsEnabled = hasShortlist && ScanButton.IsEnabled;
        PreviewQuarantineButton.IsEnabled = _currentReview is not null && hasShortlist && !hasExecutedCurrentPreview && ScanButton.IsEnabled && CanUseQuarantineRootForPreview();
        ExportQuarantinePreviewButton.IsEnabled = _currentQuarantinePreview is not null && !hasExecutedCurrentPreview && ScanButton.IsEnabled;
        SearchBox.IsEnabled = _currentReview is not null && ScanButton.IsEnabled;
        ClearSearchButton.IsEnabled = _currentReview is not null && _currentSearch.IsActive && ScanButton.IsEnabled;
        SizeThresholdFilterBox.IsEnabled = _currentReview is not null && ScanButton.IsEnabled;
        ResetViewButton.IsEnabled = _currentReview is not null && IsReviewViewFiltered() && ScanButton.IsEnabled;
        UpdateShortlistSafetyMix();
        UpdateQuarantineManifestDiscoveryControls();
    }

    private void UpdateQuarantineManifestDiscoveryControls()
    {
        if (DiscoverQuarantineManifestsButton is null)
        {
            return;
        }

        DiscoverQuarantineManifestsButton.IsEnabled = !_isScanning;
        PreviewRestoreReadinessButton.IsEnabled = !_isScanning;
        var hasDiscoveredManifest = _currentQuarantineManifestDiscovery?.ManifestCount > 0;
        RestoreManifestSelectionBox.IsEnabled = !_isScanning && hasDiscoveredManifest;
        PreviewSelectedRestoreManifestReadinessButton.IsEnabled = !_isScanning
            && hasDiscoveredManifest
            && SelectedRestoreManifestPath is not null;
        PreviewSelectedRestoreGateButton.IsEnabled = !_isScanning
            && _currentSelectedRestoreManifestReview?.HasReadinessPreview == true
            && _currentSelectedRestoreResult is null;
        SelectedRestoreConfirmationBox.IsEnabled = !_isScanning
            && _currentSelectedRestoreConfirmationDraft is not null
            && _currentSelectedRestoreResult is null;
        ExecuteSelectedRestoreButton.IsEnabled = !_isScanning
            && _currentSelectedRestoreExecutionGate?.CanExecute == true
            && _currentSelectedRestoreResult is null;
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

    private bool CanUseQuarantineRootForPreview()
    {
        return QuarantineRootSafetyNoteBuilder.Build(QuarantineRootBox.Text).CanPreview;
    }

    private bool IsFixtureQuarantineExecutionAvailable()
    {
        if (string.IsNullOrWhiteSpace(_currentCleanupScopePath))
        {
            return false;
        }

        return CleanupScopeSafetyNoteBuilder.Build(_currentCleanupScopePath).IsFixtureScope;
    }

    private bool CanUndoCurrentQuarantineExecution()
    {
        return _currentQuarantineExecutionResult is not null
            && _currentUndoQuarantineResult is null
            && _currentRestoreManifest is not null
            && _currentRestoreManifest.Entries.Any(entry => entry.Status == RestoreManifestEntryStatus.Moved)
            && IsFixtureQuarantineExecutionAvailable();
    }

    private bool IsSelectedRestoreExecutionAvailable()
    {
        var cleanupScopePath = _currentSelectedRestoreManifestReview?.SelectedManifest?.CleanupScopePath;
        if (string.IsNullOrWhiteSpace(cleanupScopePath) || _currentSelectedRestoreResult is not null)
        {
            return false;
        }

        return CleanupScopeSafetyNoteBuilder.Build(cleanupScopePath).IsFixtureScope;
    }

    private RestoreManifest? FindSelectedRestoreManifest()
    {
        var selectedPath = SelectedRestoreManifestPath;
        if (string.IsNullOrWhiteSpace(selectedPath) || _currentQuarantineManifestDiscovery is null)
        {
            return null;
        }

        return _currentQuarantineManifestDiscovery.RestoreManifests
            .FirstOrDefault(manifest => SamePath(manifest.ManifestPath, selectedPath));
    }

    private void ReportInvalidQuarantineRootPath(QuarantineRootSafetyNote note)
    {
        ClearQuarantinePreview();
        UpdateShortlistControls();
        UpdateQuarantinePreviewStatus($"Quarantine Preview could not be created: {note.Message} No files were modified.");
        StatusText.Text = $"Quarantine Preview could not be created: {note.Message} No files were modified.";
    }

    private void ClearQuarantinePreview()
    {
        _currentQuarantinePreview = null;
        _currentRestoreManifestDraft = null;
        _currentQuarantineConfirmationDraft = null;
        _currentQuarantineExecutionGate = null;
        _currentQuarantineActionDraft = null;
        var preserveCurrentFixtureUndo = CanUndoCurrentQuarantineExecution();
        if (!preserveCurrentFixtureUndo)
        {
            _currentRestoreManifest = null;
            _currentQuarantineExecutionResult = null;
            _currentUndoQuarantineResult = null;
        }

        SetQuarantineConfirmationTextSilently("");
        if (!preserveCurrentFixtureUndo)
        {
            QuarantinePreviewText.Text = "Preview and draft readiness appear after using Preview shortlist quarantine.";
        }

        ExportQuarantinePreviewButton.IsEnabled = false;
        UpdateQuarantineExecutionGate();
        UpdateQuarantinePreviewStatus();
        RefreshQuarantinedRowsIfVisible();
        UpdateQuarantinedViewControls();
    }

    private void ClearQuarantineManifestDiscovery()
    {
        if (QuarantineManifestDiscoveryText is null)
        {
            return;
        }

        _currentQuarantineManifestDiscovery = null;
        _currentRestoreReadinessPreview = null;
        _currentSelectedRestoreManifestReview = null;
        ClearSelectedRestoreGate();
        ClearRestoreManifestSelectionOptions();
        QuarantineManifestDiscoveryText.Text = "Read-only manifest discovery appears after using Discover manifests.";
        SelectedRestoreManifestReviewText.Text = "Selected Restore Manifest Review appears after discovery and Preview selected manifest readiness.";
        RestoreReadinessPreviewText.Text = "Read-only all-manifest readiness appears after using Preview all-manifest readiness.";
        UpdateQuarantineManifestDiscoveryControls();
    }

    private void ClearSelectedRestoreGate()
    {
        _currentSelectedRestoreConfirmationDraft = null;
        _currentSelectedRestoreExecutionGate = null;
        _currentSelectedRestoreResult = null;
        SetSelectedRestoreConfirmationTextSilently("");
        if (SelectedRestoreExecutionGateText is not null)
        {
            SetSelectedRestoreExecutionGateText("Selected Restore Confirmation Draft appears after Preview selected restore gate.");
        }
    }

    private void SetSelectedRestoreExecutionGateText(
        string text,
        SelectedRestoreExecutionGate? gate = null,
        UndoQuarantineResult? selectedRestoreResult = null)
    {
        SelectedRestoreExecutionGateText.Text = text;
        var helpText = FormatSelectedRestoreExecutionGateHelpText(gate, selectedRestoreResult);
        SelectedRestoreExecutionGateText.ToolTip = helpText;
        AutomationProperties.SetHelpText(SelectedRestoreExecutionGateText, helpText);
        SelectedRestoreExecutionGateHelpCue.ToolTip = helpText;
        AutomationProperties.SetHelpText(SelectedRestoreExecutionGateHelpCue, helpText);
    }

    private void SetRestoreManifestSelectionOptions(IReadOnlyList<RestoreManifestSummary> manifests)
    {
        if (RestoreManifestSelectionBox is null)
        {
            return;
        }

        _isUpdatingRestoreManifestSelectionBox = true;
        try
        {
            var options = manifests
                .Select(summary => new RestoreManifestSelectionOption(summary))
                .ToArray();
            RestoreManifestSelectionBox.ItemsSource = options;
            RestoreManifestSelectionBox.SelectedIndex = options.Length > 0 ? 0 : -1;
        }
        finally
        {
            _isUpdatingRestoreManifestSelectionBox = false;
        }
    }

    private void ClearRestoreManifestSelectionOptions()
    {
        if (RestoreManifestSelectionBox is null)
        {
            return;
        }

        _isUpdatingRestoreManifestSelectionBox = true;
        try
        {
            RestoreManifestSelectionBox.ItemsSource = Array.Empty<RestoreManifestSelectionOption>();
            RestoreManifestSelectionBox.SelectedIndex = -1;
        }
        finally
        {
            _isUpdatingRestoreManifestSelectionBox = false;
        }
    }

    private void SetQuarantineConfirmationTextSilently(string text)
    {
        _isUpdatingQuarantineConfirmationBox = true;
        try
        {
            QuarantineConfirmationBox.Text = text;
        }
        finally
        {
            _isUpdatingQuarantineConfirmationBox = false;
        }
    }

    private void SetSelectedRestoreConfirmationTextSilently(string text)
    {
        _isUpdatingSelectedRestoreConfirmationBox = true;
        try
        {
            SelectedRestoreConfirmationBox.Text = text;
        }
        finally
        {
            _isUpdatingSelectedRestoreConfirmationBox = false;
        }
    }

    private void QuarantineConfirmationBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isWindowInitialized || _isUpdatingQuarantineConfirmationBox)
        {
            return;
        }

        UpdateQuarantineExecutionGate();
    }

    private void RestoreManifestSelectionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_isWindowInitialized || _isUpdatingRestoreManifestSelectionBox)
        {
            return;
        }

        _currentSelectedRestoreManifestReview = null;
        ClearSelectedRestoreGate();
        SelectedRestoreManifestReviewText.Text = SelectedRestoreManifestPath is null
            ? "Select a discovered Restore Manifest, then use Preview selected manifest readiness."
            : "Selected Restore Manifest changed. Use Preview selected manifest readiness to refresh read-only readiness.";
        UpdateQuarantineManifestDiscoveryControls();
    }

    private void SelectedRestoreConfirmationBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isWindowInitialized || _isUpdatingSelectedRestoreConfirmationBox || _currentSelectedRestoreConfirmationDraft is null)
        {
            return;
        }

        _currentSelectedRestoreExecutionGate = SelectedRestoreExecutionGateBuilder.Build(
            _currentSelectedRestoreConfirmationDraft,
            SelectedRestoreConfirmationBox.Text);
        SetSelectedRestoreExecutionGateText(
            FormatSelectedRestoreExecutionGate(
                _currentSelectedRestoreConfirmationDraft,
                _currentSelectedRestoreExecutionGate,
                _currentSelectedRestoreResult),
            _currentSelectedRestoreExecutionGate,
            _currentSelectedRestoreResult);
        UpdateQuarantineManifestDiscoveryControls();
    }

    private void ExecuteQuarantineButton_Click(object sender, RoutedEventArgs e)
    {
        ExecuteQuarantineForCurrentPreview();
    }

    private void ExecuteSelectedRestoreButton_Click(object sender, RoutedEventArgs e)
    {
        ExecuteSelectedRestoreForCurrentSelection();
    }

    private void UndoQuarantineButton_Click(object sender, RoutedEventArgs e)
    {
        UndoQuarantineForCurrentExecution();
    }

    public void ExecuteQuarantineForCurrentPreview()
    {
        UpdateQuarantineExecutionGate();
        if (_currentQuarantineExecutionGate?.CanExecute != true || _currentRestoreManifest is null)
        {
            StatusText.Text = "Quarantine included shortlist gate is not open. No files were modified.";
            return;
        }

        _currentQuarantineExecutionResult = QuarantineExecutor.Execute(_currentRestoreManifest);
        _currentRestoreManifest = _currentQuarantineExecutionResult.RestoreManifest;
        _shortlist.Clear();
        RefreshResults();
        ExportQuarantinePreviewButton.IsEnabled = false;
        SetQuarantineConfirmationTextSilently("");
        QuarantinePreviewText.Text = FormatQuarantineExecutionResult(_currentQuarantineExecutionResult);
        UpdateShortlistControls();
        UpdateQuarantineExecutionGate();
        RefreshQuarantinedRowsIfVisible();
        UpdateQuarantinedViewControls();

        var result = _currentQuarantineExecutionResult;
        StatusText.Text = result.Succeeded
            ? $"Fixture Quarantine execution completed: {result.MovedCount:N0} included Review Shortlist row(s) moved, {result.RestoreManifest.TotalSizeDisplay} quarantined. Use Undo fixture quarantine to restore; rescan refreshes review rows."
            : $"Fixture Quarantine execution needs recovery review: {result.MovedCount:N0} included Review Shortlist row(s) moved, {result.FailedCount:N0} failed. Use Undo fixture quarantine when available; rescan refreshes review rows.";
        UpdateQuarantinePreviewStatus();
    }

    public void ExecuteSelectedRestoreForCurrentSelection()
    {
        if (_currentSelectedRestoreExecutionGate?.CanExecute != true || _currentSelectedRestoreConfirmationDraft is null)
        {
            StatusText.Text = "Selected restore execution gate is not open. No files were modified.";
            return;
        }

        var manifest = FindSelectedRestoreManifest();
        if (manifest is null)
        {
            StatusText.Text = "Selected Restore Manifest is no longer available in current discovery. No files were modified.";
            return;
        }

        _currentSelectedRestoreResult = UndoQuarantineExecutor.Undo(manifest);
        _currentSelectedRestoreExecutionGate = _currentSelectedRestoreExecutionGate with
        {
            Blockers = _currentSelectedRestoreExecutionGate.Blockers
                .Concat(["Selected restore execution has already been attempted for this selected manifest review. Rediscover manifests before another selected restore attempt."])
                .ToArray()
        };
        SetSelectedRestoreConfirmationTextSilently("");
        SetSelectedRestoreExecutionGateText(
            FormatSelectedRestoreExecutionGate(
                _currentSelectedRestoreConfirmationDraft,
                _currentSelectedRestoreExecutionGate,
                _currentSelectedRestoreResult),
            _currentSelectedRestoreExecutionGate,
            _currentSelectedRestoreResult);
        UpdateQuarantineManifestDiscoveryControls();

        var result = _currentSelectedRestoreResult;
        StatusText.Text = result.Succeeded
            ? $"Fixture Selected Restore completed: {result.RestoredCount:N0} restored. Rediscover manifests and rescan before further review."
            : $"Fixture Selected Restore needs recovery review: {result.RestoredCount:N0} restored, {result.FailedCount:N0} failed. Rediscover manifests and rescan before further review.";
    }

    public void UndoQuarantineForCurrentExecution()
    {
        if (!CanUndoCurrentQuarantineExecution())
        {
            StatusText.Text = "Undo fixture quarantine is not available. No files were modified.";
            return;
        }

        _currentUndoQuarantineResult = UndoQuarantineExecutor.Undo(_currentRestoreManifest!);
        _currentRestoreManifest = _currentUndoQuarantineResult.RestoreManifest;
        QuarantinePreviewText.Text = FormatUndoQuarantineResult(_currentUndoQuarantineResult);
        UpdateShortlistControls();
        UpdateQuarantineExecutionGate();
        RefreshQuarantinedRowsIfVisible();
        UpdateQuarantinedViewControls();

        var result = _currentUndoQuarantineResult;
        StatusText.Text = result.Succeeded
            ? $"Fixture Undo Quarantine completed: {result.RestoredCount:N0} restored. Rescan before further review."
            : $"Fixture Undo Quarantine needs recovery review: {result.RestoredCount:N0} restored, {result.FailedCount:N0} failed. Rescan before further review.";
        UpdateQuarantinePreviewStatus();
    }

    private static string FormatQuarantinePreview(
        QuarantinePreview preview,
        RestoreManifestDraft restoreManifestDraft,
        QuarantineConfirmationDraft confirmationDraft)
    {
        const int maxRows = 12;
        var lines = new List<string>
        {
            "Source: Review Shortlist. Only included rows can be quarantined; blocked and redundant rows stay out of execution.",
            $"Quarantine root: {preview.QuarantineRootPath}",
            $"Included: {preview.IncludedCount:N0} | Blocked: {preview.BlockedCount:N0} | Redundant: {preview.RedundantCount:N0}",
            $"Previewed size: {preview.IncludedSizeDisplay}",
            $"Restore Manifest Draft: {restoreManifestDraft.DraftId} | Entries: {restoreManifestDraft.EntryCount:N0} | Bytes: {restoreManifestDraft.TotalSizeDisplay} | Executed manifest: {FormatYesNo(restoreManifestDraft.IsExecutedManifest)}",
            $"Quarantine Confirmation Draft: {confirmationDraft.ConfirmationId} | Required confirmation text: {confirmationDraft.RequiredConfirmationText}",
            $"Execution scope status: {FormatQuarantineExecutionScopeStatus(confirmationDraft.IsExecutionImplemented)}",
            $"Approval boundary: {FormatQuarantineApprovalBoundary(confirmationDraft.IsExecutionImplemented)}",
            confirmationDraft.HasDataBlockers
                ? $"Confirmation readiness blockers: {confirmationDraft.Blockers.Count:N0}"
                : "Confirmation readiness blockers: 0",
            "No files were modified."
        };

        foreach (var blocker in confirmationDraft.Blockers.Take(6))
        {
            lines.Add($"Confirmation blocker | {blocker}");
        }

        if (confirmationDraft.Blockers.Count > 6)
        {
            lines.Add($"... {confirmationDraft.Blockers.Count - 6:N0} more readiness blocker(s) not shown in this pane.");
        }

        lines.Add("Preview rows:");
        foreach (var entry in preview.Entries.Take(maxRows))
        {
            var details = entry.Disposition == QuarantinePreviewDisposition.Included
                ? $"Source: {entry.SourcePath} -> Destination: {entry.DestinationPath}"
                : $"Source: {entry.SourcePath} | Reasons: {string.Join("; ", entry.Reasons)}";
            lines.Add($"Preview row | {FormatPreviewDisposition(entry.Disposition)} | {entry.SizeDisplay} | {details}");
        }

        if (preview.Entries.Count > maxRows)
        {
            lines.Add($"... {preview.Entries.Count - maxRows:N0} more shortlisted rows not shown in this pane.");
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static string FormatQuarantineExecutionGate(
        QuarantineExecutionGate gate,
        QuarantineActionDraft? actionDraft,
        RestoreManifest? restoreManifest,
        QuarantineExecutionResult? executionResult,
        UndoQuarantineResult? undoResult)
    {
        var lines = new List<string>
        {
            $"Required confirmation text: {gate.RequiredConfirmationText}",
            $"Entered confirmation matches: {FormatYesNo(gate.IsConfirmationTextMatched)}",
            $"Execution scope status: {FormatQuarantineExecutionScopeStatus(gate.IsExecutionImplemented)}",
            $"Approval boundary: {FormatQuarantineApprovalBoundary(gate.IsExecutionImplemented)}",
            $"Can execute: {FormatYesNo(gate.CanExecute)}",
            undoResult is not null
                ? "Fixture Undo Quarantine has restored synthetic files where possible. Current scan results are stale."
                : executionResult is null
                ? "No files were modified."
                : "Fixture Quarantine execution has moved synthetic files. Current scan results are stale."
        };

        if (actionDraft is not null)
        {
            lines.Add("Execution target: all included Review Shortlist rows in this Quarantine Action Draft.");
            lines.Add($"Quarantine Action Draft: {actionDraft.ActionId} | Entries: {actionDraft.EntryCount:N0} | Bytes: {actionDraft.TotalSizeDisplay}");
            lines.Add($"Action items root: {actionDraft.ItemsRootPath}");
            lines.Add($"Restore manifest path: {actionDraft.RestoreManifestPath}");
        }

        if (restoreManifest is not null)
        {
            lines.Add($"Write-ahead Restore Manifest: {restoreManifest.ManifestId} | Status: {FormatRestoreManifestActionStatus(restoreManifest.ActionStatus)} | Entries: {restoreManifest.EntryCount:N0}");
            lines.Add("Manifest write order: write planned manifest before the first move, then update each entry before and after its move attempt.");
            foreach (var note in restoreManifest.WriteOrderNotes.Take(3))
            {
                lines.Add($"Manifest note | {note}");
            }
        }

        if (executionResult is not null)
        {
            lines.Add($"Execution result: moved {executionResult.MovedCount:N0}, failed {executionResult.FailedCount:N0}, blockers {executionResult.Blockers.Count:N0}, recovery review: {FormatYesNo(executionResult.RequiresRecoveryReview)}");
            foreach (var resultEntry in executionResult.Entries.Take(6))
            {
                var status = resultEntry.WasMoved ? "Moved" : "Not moved";
                var error = string.IsNullOrWhiteSpace(resultEntry.ErrorMessage)
                    ? ""
                    : $" | Error: {resultEntry.ErrorMessage}";
                lines.Add($"Execution row | {status} | {resultEntry.OriginalPath} -> {resultEntry.QuarantinePath}{error}");
            }

            if (executionResult.Entries.Count > 6)
            {
                lines.Add($"... {executionResult.Entries.Count - 6:N0} more execution row(s) not shown in this pane.");
            }
        }

        if (undoResult is not null)
        {
            lines.Add($"Undo result: restored {undoResult.RestoredCount:N0}, failed {undoResult.FailedCount:N0}, blockers {undoResult.Blockers.Count:N0}, recovery review: {FormatYesNo(undoResult.RequiresRecoveryReview)}");
            foreach (var resultEntry in undoResult.Entries.Take(6))
            {
                var status = resultEntry.WasRestored ? "Restored" : "Not restored";
                var error = string.IsNullOrWhiteSpace(resultEntry.ErrorMessage)
                    ? ""
                    : $" | Error: {resultEntry.ErrorMessage}";
                lines.Add($"Undo row | {status} | {resultEntry.QuarantinePath} -> {resultEntry.OriginalPath}{error}");
            }

            if (undoResult.Entries.Count > 6)
            {
                lines.Add($"... {undoResult.Entries.Count - 6:N0} more undo row(s) not shown in this pane.");
            }
        }

        foreach (var blocker in gate.Blockers.Take(6))
        {
            lines.Add($"Execution gate blocker | {FormatQuarantineExecutionGateBlocker(blocker)}");
        }

        if (gate.Blockers.Count > 6)
        {
            lines.Add($"... {gate.Blockers.Count - 6:N0} more execution gate blocker(s) not shown in this pane.");
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static string FormatQuarantineExecutionGateHelpText(
        QuarantineExecutionGate gate,
        QuarantineExecutionResult? executionResult,
        UndoQuarantineResult? undoResult)
    {
        var state = undoResult is not null
            ? "Current fixture Undo Quarantine has already run; rescan before further review."
            : executionResult is not null
            ? "Current fixture Quarantine execution already ran; use Undo fixture quarantine if you want to restore this current fixture execution."
            : gate.CanExecute
            ? "Gate is open for fixture execution after exact QUARANTINE confirmation."
            : gate.HasBlockers
            ? $"Gate is closed: {FormatQuarantineExecutionGateBlocker(gate.Blockers[0])}"
            : "Gate is closed until exact QUARANTINE confirmation.";

        return $"Quarantine Execution Gate: {state} The gate text is review context; only the separate fixture action button can move files after exact QUARANTINE. Real-profile/custom execution remains unavailable. This cue does not create folders, move files, restore files, delete files, write manifests, or approve cleanup.";
    }

    private static string FormatQuarantineExecutionGateBlocker(string blocker)
    {
        return string.Equals(
            blocker,
            "Create a Quarantine Preview before entering confirmation text.",
            StringComparison.Ordinal)
            ? "Use Preview shortlist quarantine before entering confirmation text."
            : blocker;
    }

    private static string FormatQuarantineExecutionScopeStatus(bool isExecutionImplemented)
    {
        return isExecutionImplemented
            ? "Fixture-only execution is available only after preview readiness and exact QUARANTINE confirmation."
            : "Preview only for this Cleanup Scope; real-profile and custom execution remain unavailable.";
    }

    private static string FormatQuarantineApprovalBoundary(bool isExecutionImplemented)
    {
        return isExecutionImplemented
            ? "Review Shortlist and Quarantine Preview are not cleanup approval; exact QUARANTINE can open only fixture execution in this build."
            : "Review Shortlist and Quarantine Preview are not cleanup approval; real-profile and custom execution remain unavailable.";
    }

    private static string FormatQuarantineExecutionResult(QuarantineExecutionResult result)
    {
        var lines = new List<string>
        {
            $"Fixture Quarantine execution result: {FormatRestoreManifestActionStatus(result.RestoreManifest.ActionStatus)}",
            $"Moved: {result.MovedCount:N0} | Failed: {result.FailedCount:N0} | Recovery review: {FormatYesNo(result.RequiresRecoveryReview)}",
            $"Restore manifest path: {result.RestoreManifest.ManifestPath}",
            "Current scan and review rows are stale after execution. Rescan before selecting more cleanup candidates."
        };

        foreach (var blocker in result.Blockers.Take(6))
        {
            lines.Add($"Execution blocker | {blocker}");
        }

        foreach (var entry in result.Entries.Take(12))
        {
            var status = entry.WasMoved ? "Moved" : "Failed";
            var error = string.IsNullOrWhiteSpace(entry.ErrorMessage)
                ? ""
                : $" | Error: {entry.ErrorMessage}";
            lines.Add($"Execution row | {status} | {entry.OriginalPath} -> {entry.QuarantinePath}{error}");
        }

        if (result.Entries.Count > 12)
        {
            lines.Add($"... {result.Entries.Count - 12:N0} more execution row(s) not shown in this pane.");
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static string FormatUndoQuarantineResult(UndoQuarantineResult result)
    {
        var lines = new List<string>
        {
            $"Fixture Undo Quarantine result: {FormatRestoreManifestActionStatus(result.RestoreManifest.ActionStatus)}",
            $"Restored: {result.RestoredCount:N0} | Failed: {result.FailedCount:N0} | Recovery review: {FormatYesNo(result.RequiresRecoveryReview)}",
            $"Restore manifest path: {result.RestoreManifest.ManifestPath}",
            "Current scan and review rows are stale after undo. Rescan before selecting more cleanup candidates."
        };

        foreach (var blocker in result.Blockers.Take(6))
        {
            lines.Add($"Undo blocker | {blocker}");
        }

        foreach (var entry in result.Entries.Take(12))
        {
            var status = entry.WasRestored ? "Restored" : "Failed";
            var error = string.IsNullOrWhiteSpace(entry.ErrorMessage)
                ? ""
                : $" | Error: {entry.ErrorMessage}";
            lines.Add($"Undo row | {status} | {entry.QuarantinePath} -> {entry.OriginalPath}{error}");
        }

        if (result.Entries.Count > 12)
        {
            lines.Add($"... {result.Entries.Count - 12:N0} more undo row(s) not shown in this pane.");
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static string FormatQuarantineManifestDiscovery(QuarantineManifestDiscovery discovery)
    {
        var lines = new List<string>
        {
            "Quarantine Manifest Discovery: read-only",
            $"Quarantine root: {discovery.QuarantineRootPath}",
            $"Actions root: {discovery.ActionsRootPath}",
            $"Discovered manifests: {discovery.ManifestCount:N0} | Issues: {discovery.Issues.Count:N0}",
            "No all-manifest restore action is available from this discovery pane; fixture selected restore must go through selected manifest readiness and the selected restore gate."
        };

        foreach (var summary in discovery.Manifests.Take(8))
        {
            lines.Add($"Manifest | {FormatRestoreManifestActionStatus(summary.ActionStatus)} | Entries {summary.EntryCount:N0} | Size {summary.TotalSizeDisplay} | Moved {summary.MovedCount:N0} | Restored {summary.RestoredCount:N0} | Recovery review: {FormatYesNo(summary.RequiresRecoveryReview)} | {summary.ManifestPath}");
        }

        if (discovery.Manifests.Count > 8)
        {
            lines.Add($"... {discovery.Manifests.Count - 8:N0} more manifest(s) not shown in this pane.");
        }

        foreach (var issue in discovery.Issues.Take(8))
        {
            lines.Add($"Discovery issue | {issue.Path} | {issue.Message}");
        }

        if (discovery.Issues.Count > 8)
        {
            lines.Add($"... {discovery.Issues.Count - 8:N0} more discovery issue(s) not shown in this pane.");
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static string FormatRestoreReadinessPreview(RestoreReadinessPreview preview)
    {
        var lines = new List<string>
        {
            "Restore Readiness Preview: read-only",
            $"Quarantine root: {preview.QuarantineRootPath}",
            $"Actions root: {preview.ActionsRootPath}",
            $"Manifests: {preview.ManifestCount:N0} | Restorable manifests: {preview.RestorableManifestCount:N0} | Restorable entries: {preview.RestorableEntryCount:N0} | Blocked entries: {preview.BlockedEntryCount:N0} | Recovery review entries: {preview.RecoveryReviewEntryCount:N0}",
            $"Discovery issues: {preview.DiscoveryIssues.Count:N0}",
            "No all-manifest restore action is available from this readiness preview; fixture selected restore must go through selected manifest readiness and the selected restore gate."
        };

        foreach (var manifest in preview.Manifests.Take(6))
        {
            AddRestoreReadinessManifestLines(lines, manifest, 4);
        }

        if (preview.Manifests.Count > 6)
        {
            lines.Add($"... {preview.Manifests.Count - 6:N0} more manifest readiness result(s) not shown in this pane.");
        }

        foreach (var issue in preview.DiscoveryIssues.Take(6))
        {
            lines.Add($"Discovery issue | {issue.Path} | {issue.Message}");
        }

        if (preview.DiscoveryIssues.Count > 6)
        {
            lines.Add($"... {preview.DiscoveryIssues.Count - 6:N0} more discovery issue(s) not shown in this pane.");
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static string FormatSelectedRestoreManifestReview(SelectedRestoreManifestReview review)
    {
        var selectedPath = string.IsNullOrWhiteSpace(review.SelectedManifestPath)
            ? "(none)"
            : review.SelectedManifestPath;
        var lines = new List<string>
        {
            "Selected Restore Manifest Review: read-only",
            $"Quarantine root: {review.QuarantineRootPath}",
            $"Actions root: {review.ActionsRootPath}",
            $"Selected manifest: {selectedPath}",
            "Selected Restore Manifest Review is readiness evidence only; fixture selected restore must go through the selected restore gate."
        };

        foreach (var issue in review.SelectionIssues.Take(6))
        {
            lines.Add($"Selection issue | {issue}");
        }

        if (review.SelectionIssues.Count > 6)
        {
            lines.Add($"... {review.SelectionIssues.Count - 6:N0} more selection issue(s) not shown in this pane.");
        }

        if (review.Readiness is not null)
        {
            AddRestoreReadinessManifestLines(lines, review.Readiness, 8);
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static string FormatSelectedRestoreExecutionGate(
        SelectedRestoreConfirmationDraft confirmationDraft,
        SelectedRestoreExecutionGate gate,
        UndoQuarantineResult? selectedRestoreResult = null)
    {
        var selectedPath = string.IsNullOrWhiteSpace(confirmationDraft.SelectedManifestPath)
            ? "(none)"
            : confirmationDraft.SelectedManifestPath;
        var lines = new List<string>
        {
            $"Selected Restore Confirmation Draft: {confirmationDraft.ConfirmationId} | Required confirmation text: {confirmationDraft.RequiredConfirmationText}",
            $"Selected manifest: {selectedPath}",
            $"Restorable entries: {confirmationDraft.RestorableEntryCount:N0} | Restorable size: {confirmationDraft.RestorableSizeDisplay} | Blocked rows: {confirmationDraft.BlockedEntryCount:N0} | Recovery review rows: {confirmationDraft.RecoveryReviewEntryCount:N0} | Not moved rows: {confirmationDraft.NotMovedEntryCount:N0} | Already restored rows: {confirmationDraft.AlreadyRestoredEntryCount:N0}",
            $"Selected Restore Execution Gate: read-only",
            $"Required confirmation text: {gate.RequiredConfirmationText}",
            $"Entered confirmation matches: {FormatYesNo(gate.IsConfirmationTextMatched)}",
            $"Execution scope status: {FormatSelectedRestoreExecutionScopeStatus(gate.IsExecutionImplemented)}",
            $"Approval boundary: {FormatSelectedRestoreApprovalBoundary(gate.IsExecutionImplemented)}",
            $"Can execute: {FormatYesNo(gate.CanExecute)}",
            selectedRestoreResult is null
                ? "No files were modified by this selected restore gate."
                : "Fixture Selected Restore has restored synthetic files where possible. Current scan, discovery, and readiness rows are stale."
        };

        foreach (var blocker in gate.Blockers.Take(8))
        {
            lines.Add($"Selected restore gate blocker | {blocker}");
        }

        if (gate.Blockers.Count > 8)
        {
            lines.Add($"... {gate.Blockers.Count - 8:N0} more selected restore gate blocker(s) not shown in this pane.");
        }

        foreach (var note in gate.ReviewNotes.Take(4))
        {
            lines.Add($"Selected restore gate note | {note}");
        }

        if (selectedRestoreResult is not null)
        {
            lines.Add($"Selected restore result: Restored {selectedRestoreResult.RestoredCount:N0} | Failed {selectedRestoreResult.FailedCount:N0} | Recovery review: {FormatYesNo(selectedRestoreResult.RequiresRecoveryReview)}");

            foreach (var blocker in selectedRestoreResult.Blockers.Take(6))
            {
                lines.Add($"Selected restore result blocker | {blocker}");
            }

            foreach (var entry in selectedRestoreResult.Entries.Take(8))
            {
                var status = entry.WasRestored ? "Restored" : "Failed";
                var error = string.IsNullOrWhiteSpace(entry.ErrorMessage)
                    ? ""
                    : $" | Error: {entry.ErrorMessage}";
                lines.Add($"Selected restore row | {status} | {entry.QuarantinePath} -> {entry.OriginalPath}{error}");
            }

            if (selectedRestoreResult.Entries.Count > 8)
            {
                lines.Add($"... {selectedRestoreResult.Entries.Count - 8:N0} more selected restore row(s) not shown in this pane.");
            }
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static string FormatSelectedRestoreExecutionGateHelpText(
        SelectedRestoreExecutionGate? gate,
        UndoQuarantineResult? selectedRestoreResult = null)
    {
        var state = gate is null
            ? "Preview selected restore gate after selected manifest readiness to review exact RESTORE confirmation."
            : selectedRestoreResult is not null
            ? "Fixture Selected Restore has already run; rediscover manifests and rescan before further review."
            : gate.CanExecute
            ? "Gate is open for fixture selected restore after selected manifest readiness and exact RESTORE confirmation."
            : gate.HasBlockers
            ? $"Gate is closed: {gate.Blockers[0]}"
            : "Gate is closed until exact RESTORE confirmation.";

        return $"Selected Restore Execution Gate: {state} The gate text is review context; only the separate fixture selected restore button can restore files after selected manifest readiness and exact RESTORE. Real-profile/custom selected restore remains unavailable. This cue does not create folders, move files, restore files, delete files, write manifests, clean up folders, or approve restore.";
    }

    private static string FormatSelectedRestoreExecutionScopeStatus(bool isExecutionImplemented)
    {
        return isExecutionImplemented
            ? "Fixture-only selected restore is available only after selected manifest readiness and exact RESTORE confirmation."
            : "Preview only for this selected Restore Manifest; real-profile and custom selected restore remain unavailable.";
    }

    private static string FormatSelectedRestoreApprovalBoundary(bool isExecutionImplemented)
    {
        return isExecutionImplemented
            ? "Selected Restore Manifest Review and readiness preview are not restore approval; exact RESTORE can open only fixture selected restore in this build."
            : "Selected Restore Manifest Review and readiness preview are not restore approval; real-profile and custom selected restore remain unavailable.";
    }

    private static void AddRestoreReadinessManifestLines(
        List<string> lines,
        RestoreReadinessManifestPreview manifest,
        int entryLimit)
    {
        lines.Add($"Manifest readiness | {FormatRestoreManifestActionStatus(manifest.ActionStatus)} | Entries {manifest.EntryCount:N0} | Restorable {manifest.RestorableCount:N0} | Blocked {manifest.BlockedCount:N0} | Already restored {manifest.AlreadyRestoredCount:N0} | Recovery review {manifest.RecoveryReviewCount:N0} | {manifest.ManifestPath}");

        foreach (var blocker in manifest.Blockers.Take(3))
        {
            lines.Add($"Manifest blocker | {blocker}");
        }

        foreach (var entry in manifest.Entries.Take(entryLimit))
        {
            var blockers = entry.Blockers.Count == 0
                ? ""
                : $" | Blockers: {string.Join(" ; ", entry.Blockers.Take(2))}";
            lines.Add($"Restore readiness row | {FormatRestoreReadinessDisposition(entry.Disposition)} | {entry.SizeDisplay} | {entry.QuarantinePath} -> {entry.OriginalPath}{blockers}");
        }

        if (manifest.Entries.Count > entryLimit)
        {
            lines.Add($"... {manifest.Entries.Count - entryLimit:N0} more restore readiness row(s) not shown for this manifest.");
        }
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

    private static string FormatRestoreManifestActionStatus(RestoreManifestActionStatus status)
    {
        return status switch
        {
            RestoreManifestActionStatus.Planned => "Planned",
            RestoreManifestActionStatus.Moving => "Moving",
            RestoreManifestActionStatus.Completed => "Completed",
            RestoreManifestActionStatus.PartialFailure => "Partial failure",
            RestoreManifestActionStatus.Failed => "Failed",
            RestoreManifestActionStatus.Restoring => "Restoring",
            RestoreManifestActionStatus.Restored => "Restored",
            RestoreManifestActionStatus.RestorePartialFailure => "Restore partial failure",
            RestoreManifestActionStatus.RestoreFailed => "Restore failed",
            _ => status.ToString()
        };
    }

    private static string FormatRestoreReadinessDisposition(RestoreReadinessDisposition disposition)
    {
        return disposition switch
        {
            RestoreReadinessDisposition.Restorable => "Restorable",
            RestoreReadinessDisposition.Blocked => "Blocked",
            RestoreReadinessDisposition.AlreadyRestored => "Already restored",
            RestoreReadinessDisposition.NeedsRecoveryReview => "Needs recovery review",
            RestoreReadinessDisposition.NotMoved => "Not moved",
            _ => disposition.ToString()
        };
    }

    private void UpdateReviewMix()
    {
        if (_currentReview is null)
        {
            SetReviewMixText("Review mix appears after a scan.");
            return;
        }

        var summary = _currentReview.Summary;
        SetReviewMixText(
            $"Review mix: " +
            $"Likely safe {summary.LikelySafeCount:N0} (largest {ByteSizeFormatter.Format(summary.LikelySafeLargestEntryBytes)}) | " +
            $"Caution {summary.CautionCount:N0} (largest {ByteSizeFormatter.Format(summary.CautionLargestEntryBytes)}) | " +
            $"High risk {summary.HighRiskCount:N0} (largest {ByteSizeFormatter.Format(summary.HighRiskLargestEntryBytes)}) | " +
            $"Quarantine candidates {summary.QuarantineCandidateCount:N0} (largest {ByteSizeFormatter.Format(summary.QuarantineCandidateLargestEntryBytes)}) | " +
            $"Access issues {summary.AccessIssueCount:N0}");
    }

    private void SetReviewMixText(string text)
    {
        ReviewMixText.Text = text;
        var helpText = FormatReviewMixHelpText(text);
        ReviewMixText.ToolTip = helpText;
        AutomationProperties.SetHelpText(ReviewMixText, helpText);
        ReviewMixHelpCue.ToolTip = helpText;
        AutomationProperties.SetHelpText(ReviewMixHelpCue, helpText);
    }

    private static string FormatReviewMixHelpText(string text)
    {
        return $"{text} Review Mix is read-only whole-scan review context; it does not rescan, modify files, prove storage savings, or approve cleanup. Largest rows are triage clues because recursive parent and child rows can overlap.";
    }

    private void UpdateSafetySummary()
    {
        if (_currentSafetySummary is null)
        {
            SafetySummaryText.Text = "Safety summary appears after a scan.";
            UpdateSafetySummaryHeader();
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
            FormatAccessIssueExamples(summary) +
            FormatQuarantineCandidateExamples(summary) +
            FormatUncategorizedExamples(summary) +
            string.Join(" ", summary.Notes);
        UpdateSafetySummaryHeader();
        UpdateSafetyShortcutButtons();
    }

    private void UpdateSafetySummaryHeader()
    {
        if (SafetySummaryHeaderText is null)
        {
            return;
        }

        if (_currentSafetySummary is null)
        {
            const string waitingHeaderText = "Safety Summary: waiting for scan";
            SetSafetySummaryHeader(waitingHeaderText, SafetySummaryHeaderStatusStyle.Neutral);
            return;
        }

        var summary = _currentSafetySummary;
        var headerText =
            $"Safety Summary: {summary.StatusLabel} | " +
            $"High risk {summary.HighRiskCount:N0} | " +
            $"Protected {summary.ProtectedLocationCount:N0} | " +
            $"Access {summary.AccessIssueCount:N0} | " +
            $"Quarantine {summary.QuarantineCandidateCount:N0} | " +
            $"No category {summary.UncategorizedCount:N0}";
        var style = summary.HasReviewWarnings
            ? SafetySummaryHeaderStatusStyle.Warning
            : SafetySummaryHeaderStatusStyle.Neutral;
        SetSafetySummaryHeader(headerText, style);
    }

    private void SetSafetySummaryHeader(string headerText, SafetySummaryHeaderStatusStyle style)
    {
        var helpText = $"{headerText}. Header state: {FormatSafetySummaryHeaderState(style)}. Header summary is read-only review context, not cleanup approval.";
        SafetySummaryHeaderText.Text = headerText;
        SafetySummaryHeaderText.Tag = style.ToString();
        SafetySummaryHeaderText.ToolTip = helpText;
        AutomationProperties.SetHelpText(SafetySummaryHeaderText, helpText);
        SafetySummaryHeaderHelpCue.ToolTip = helpText;
        AutomationProperties.SetHelpText(SafetySummaryHeaderHelpCue, helpText);
        SafetySummaryHeaderText.Foreground = style switch
        {
            SafetySummaryHeaderStatusStyle.Warning => System.Windows.Media.Brushes.DarkGoldenrod,
            _ => System.Windows.Media.Brushes.SlateGray
        };
    }

    private enum SafetySummaryHeaderStatusStyle
    {
        Neutral,
        Warning
    }

    private static string FormatSafetySummaryHeaderState(SafetySummaryHeaderStatusStyle style)
    {
        return style switch
        {
            SafetySummaryHeaderStatusStyle.Warning => "needs review",
            _ => "neutral"
        };
    }

    private static string FormatAccessIssueExamples(StorageScanSafetySummary summary)
    {
        return summary.AccessIssueExamples.Count == 0
            ? ""
            : $"Access examples: {string.Join("; ", summary.AccessIssueExamples)}. ";
    }

    private static string FormatQuarantineCandidateExamples(StorageScanSafetySummary summary)
    {
        return summary.QuarantineCandidateExamples.Count == 0
            ? ""
            : $"Candidate examples: {string.Join("; ", summary.QuarantineCandidateExamples)}. ";
    }

    private static string FormatUncategorizedExamples(StorageScanSafetySummary summary)
    {
        return summary.UncategorizedExamples.Count == 0
            ? ""
            : $"No category examples: {string.Join("; ", summary.UncategorizedExamples)}. ";
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
        var typeSegment = EntryTypeFilterBox.SelectedItem is EntryTypeFilterOption typeOption
            ? typeOption.FileNameSegment
            : "all-types";
        var sizeSegment = SizeThresholdFilterBox.SelectedItem is SizeThresholdFilterOption sizeOption
            ? sizeOption.FileNameSegment
            : "all-sizes";
        var searchSegment = _currentSearch.IsActive
            ? $"-search-{FormatFileNameSearch(_currentSearch.Query)}"
            : "";
        return $"storage-scan-{DateTime.Now:yyyyMMdd-HHmmss}-{FormatFileNameFilter(_currentFilter)}-{categorySegment}-{typeSegment}-{sizeSegment}{searchSegment}.csv";
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

    private static string FormatEntryTypeFilter(StorageEntryTypeFilter filter)
    {
        return filter switch
        {
            StorageEntryTypeFilter.Files => "Files",
            StorageEntryTypeFilter.Folders => "Folders",
            _ => "All types"
        };
    }

    private static string FormatSizeThresholdFilter(StorageSizeThresholdFilter filter)
    {
        return filter switch
        {
            StorageSizeThresholdFilter.AtLeast1Mb => "1 MB+",
            StorageSizeThresholdFilter.AtLeast100Mb => "100 MB+",
            StorageSizeThresholdFilter.AtLeast1Gb => "1 GB+",
            StorageSizeThresholdFilter.AtLeast5Gb => "5 GB+",
            StorageSizeThresholdFilter.AtLeast10Gb => "10 GB+",
            _ => "All sizes"
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

    private void SelectEntryTypeFilterOption(StorageEntryTypeFilter filter)
    {
        var option = EntryTypeFilterBox.Items
            .Cast<EntryTypeFilterOption>()
            .FirstOrDefault(candidate => candidate.Filter == filter);
        if (option is null)
        {
            return;
        }

        _isUpdatingEntryTypeFilterOptions = true;
        try
        {
            EntryTypeFilterBox.SelectedItem = option;
        }
        finally
        {
            _isUpdatingEntryTypeFilterOptions = false;
        }
    }

    private void SelectSizeThresholdFilterOption(StorageSizeThresholdFilter filter)
    {
        var option = SizeThresholdFilterBox.Items
            .Cast<SizeThresholdFilterOption>()
            .FirstOrDefault(candidate => candidate.Filter == filter);
        if (option is null)
        {
            return;
        }

        _isUpdatingSizeThresholdFilterOptions = true;
        try
        {
            SizeThresholdFilterBox.SelectedItem = option;
        }
        finally
        {
            _isUpdatingSizeThresholdFilterOptions = false;
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

    private static string FormatStorageSubtreeReviewSummary(StorageEntry entry, string? cleanupScopePath)
    {
        if (!entry.IsDirectory)
        {
            return "Files do not have descendant subtree summaries.";
        }

        var summary = StorageSubtreeReviewSummaryBuilder.Build(entry, cleanupScopePath);
        if (summary.DescendantRowCount == 0)
        {
            return "No descendant rows were found for this folder.";
        }

        var lines = new List<string>
        {
            $"Descendant rows: {summary.DescendantRowCount:N0} ({summary.DescendantFileCount:N0} files | {summary.DescendantFolderCount:N0} folders).",
            $"Ratings: Likely safe {summary.LikelySafeCount:N0} | Caution {summary.CautionCount:N0} | High risk {summary.HighRiskCount:N0}.",
            $"Review flags: Quarantine candidates {summary.QuarantineCandidateCount:N0} | Protected {summary.ProtectedLocationCount:N0} | Access issues {summary.AccessIssueCount:N0} | Reparse points {summary.ReparsePointCount:N0} | No category {summary.UncategorizedCount:N0}.",
            $"Largest descendant row: {summary.LargestDescendantSizeDisplay}. Recursive folder row sizes overlap and are not storage savings or cleanup approval."
        };
        AddExampleLine(lines, "Candidate examples", summary.QuarantineCandidateExamples);
        AddExampleLine(lines, "Protected examples", summary.ProtectedLocationExamples);
        AddExampleLine(lines, "Access issue examples", summary.AccessIssueExamples);
        AddExampleLine(lines, "No category examples", summary.UncategorizedExamples);
        return string.Join(Environment.NewLine, lines);
    }

    private static void AddExampleLine(List<string> lines, string label, IReadOnlyList<string> examples)
    {
        if (examples.Count > 0)
        {
            lines.Add($"{label}: {string.Join("; ", examples)}.");
        }
    }

    private static string FormatStorageHotspotTrail(StorageEntry entry)
    {
        if (!entry.IsDirectory)
        {
            return "Files do not have descendant hotspot trails.";
        }

        var trail = StorageHotspotTrailBuilder.Build(entry);
        if (trail.Count == 0)
        {
            return "No descendant hotspot trail was found for this folder.";
        }

        var rows = trail.Select(row =>
            $"{row.Level:N0}. {row.Name} | {(row.IsDirectory ? "Folder" : "File")} | {row.SizeDisplay} | {FormatImportance(row.ImportanceRating)} | {FormatRecommendation(row.DeletionRecommendation)} | {FormatCategories(row.BloatCategories)}");
        return "Largest child at each level. Sizes overlap down the trail and are not storage savings."
            + Environment.NewLine
            + string.Join(Environment.NewLine, rows);
    }

    private static string FormatSelectedPathReviewGuidance(StorageEntry entry)
    {
        var guidance = SelectedPathReviewGuidanceBuilder.Build(entry);
        return $"{guidance.ActionLabel}: {string.Join(" ", guidance.Notes)}";
    }

    private static string FormatSelectedFileContentPreview(SelectedFileContentPreview preview)
    {
        var content = string.IsNullOrWhiteSpace(preview.Content)
            ? ""
            : $"{Environment.NewLine}{Environment.NewLine}{preview.Content}";
        return $"{preview.Label}: {preview.Message}{content}";
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
            BloatCategory.CleanupScopeRoot => "Cleanup scope root",
            BloatCategory.ProfileContainer => "Profile container",
            BloatCategory.ApplicationDataArea => "AppData area",
            BloatCategory.BrowserData => "Browser data",
            BloatCategory.CloudSyncData => "Cloud sync data",
            BloatCategory.CredentialData => "Credential data",
            BloatCategory.OldDownload => "Old download",
            BloatCategory.TemporaryFolder => "Temporary folder",
            BloatCategory.InstallerCache => "Installer cache",
            BloatCategory.AppCache => "App cache",
            BloatCategory.GpuShaderCache => "GPU shader cache",
            BloatCategory.DuplicateFileCandidate => "Duplicate file candidate",
            BloatCategory.LargeOldFile => "Large old file",
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

    private static bool SamePath(string left, string right)
    {
        try
        {
            return string.Equals(
                PathSafety.GetFullPath(left).TrimEnd(Path.DirectorySeparatorChar),
                PathSafety.GetFullPath(right).TrimEnd(Path.DirectorySeparatorChar),
                StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
        {
            return string.Equals(left.Trim(), right.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}

internal sealed class RestoreManifestSelectionOption
{
    public RestoreManifestSelectionOption(RestoreManifestSummary summary)
    {
        Summary = summary;
    }

    public RestoreManifestSummary Summary { get; }

    public string Label =>
        $"{Summary.ActionId} | {Summary.ActionStatus} | Entries {Summary.EntryCount:N0} | Moved {Summary.MovedCount:N0} | Restored {Summary.RestoredCount:N0} | {Summary.TotalSizeDisplay} | {Summary.ManifestPath}";
}
