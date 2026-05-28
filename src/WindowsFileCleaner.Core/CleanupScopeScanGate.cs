namespace WindowsFileCleaner.Core;

public sealed record CleanupScopeScanGate(
    bool CanScan,
    bool RequiresPreflightAcknowledgement,
    string Message);
