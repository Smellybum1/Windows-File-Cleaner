namespace WindowsFileCleaner.Core;

public sealed record PathSnapshot(
    string FullPath,
    string Name,
    bool IsDirectory,
    bool IsAccessible,
    bool IsReparsePoint,
    DateTimeOffset? LastModifiedUtc);

