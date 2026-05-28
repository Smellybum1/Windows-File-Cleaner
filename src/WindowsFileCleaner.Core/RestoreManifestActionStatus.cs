namespace WindowsFileCleaner.Core;

public enum RestoreManifestActionStatus
{
    Planned,
    Moving,
    Completed,
    PartialFailure,
    Failed,
    Restoring,
    Restored,
    RestorePartialFailure,
    RestoreFailed
}
