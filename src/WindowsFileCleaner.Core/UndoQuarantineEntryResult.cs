namespace WindowsFileCleaner.Core;

public sealed record UndoQuarantineEntryResult(
    string OriginalPath,
    string QuarantinePath,
    RestoreManifestEntryStatus Status,
    bool WasRestored,
    string? ErrorMessage);
