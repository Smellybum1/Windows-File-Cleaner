namespace WindowsFileCleaner.Core;

public sealed record SelectedPathReviewGuidance(
    string ActionLabel,
    IReadOnlyList<string> Notes);
