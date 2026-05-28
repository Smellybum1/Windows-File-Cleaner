namespace WindowsFileCleaner.Core;

public sealed record SelectedFileContentPreview(
    bool IsContentShown,
    string Label,
    string Message,
    string Content,
    bool IsTruncated,
    int BytesRead);
