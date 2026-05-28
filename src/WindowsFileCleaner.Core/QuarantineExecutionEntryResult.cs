namespace WindowsFileCleaner.Core;

public sealed record QuarantineExecutionEntryResult(
    string OriginalPath,
    string QuarantinePath,
    RestoreManifestEntryStatus Status,
    bool WasMoved,
    string? ErrorMessage);
