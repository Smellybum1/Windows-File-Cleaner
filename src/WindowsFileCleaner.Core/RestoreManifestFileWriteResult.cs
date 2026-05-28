namespace WindowsFileCleaner.Core;

public sealed record RestoreManifestFileWriteResult(
    string ManifestPath,
    long BytesWritten,
    DateTimeOffset WrittenAtUtc);
