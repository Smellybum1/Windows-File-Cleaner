namespace WindowsFileCleaner.Core;

public sealed record QuarantineRootSafetyNote(
    string Label,
    string Message,
    string RootPath,
    bool CanPreview,
    bool IsPreferredDrive);
