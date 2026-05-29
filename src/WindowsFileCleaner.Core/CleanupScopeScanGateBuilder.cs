namespace WindowsFileCleaner.Core;

public static class CleanupScopeScanGateBuilder
{
    public static CleanupScopeScanGate Build(string cleanupScopePath, bool realProfilePreflightAcknowledged)
    {
        var note = CleanupScopeSafetyNoteBuilder.Build(cleanupScopePath);

        if (note.Label == "Choose Cleanup Scope")
        {
            return new CleanupScopeScanGate(
                CanScan: false,
                RequiresPreflightAcknowledgement: false,
                Message: "Enter a Cleanup Scope before scanning.");
        }

        if (note.Label == "Check Cleanup Scope")
        {
            return new CleanupScopeScanGate(
                CanScan: false,
                RequiresPreflightAcknowledgement: false,
                Message: "Fix the Cleanup Scope path before scanning.");
        }

        if (note.IsRealUserProfileScope && !realProfilePreflightAcknowledged)
        {
            return new CleanupScopeScanGate(
                CanScan: false,
                RequiresPreflightAcknowledgement: true,
                Message: "Confirm MVP preflight and fixture review before scanning this real profile.");
        }

        if (note.IsRealUserProfileScope)
        {
            return new CleanupScopeScanGate(
                CanScan: true,
                RequiresPreflightAcknowledgement: true,
                Message: "Real-profile scan confirmation accepted. Storage Scan remains read-only; real-profile cleanup execution remains unavailable.");
        }

        if (note.IsFixtureScope)
        {
            return new CleanupScopeScanGate(
                CanScan: true,
                RequiresPreflightAcknowledgement: false,
                Message: "Fixture scan can start. Storage Scan is read-only; fixture cleanup actions still require preview and exact confirmation.");
        }

        return new CleanupScopeScanGate(
            CanScan: true,
            RequiresPreflightAcknowledgement: false,
            Message: "Custom scope scan can start. Storage Scan is read-only; real-profile and custom cleanup execution remain unavailable.");
    }
}
