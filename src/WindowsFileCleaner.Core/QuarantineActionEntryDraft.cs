namespace WindowsFileCleaner.Core;

public sealed record QuarantineActionEntryDraft(
    string OriginalPath,
    string RelativePath,
    string PreviewQuarantinePath,
    string ActionQuarantinePath,
    bool IsDirectory,
    long SizeBytes);
