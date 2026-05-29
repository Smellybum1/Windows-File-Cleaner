namespace WindowsFileCleaner.Core;

public static class SelectedRestoreExecutionGateBuilder
{
    public static SelectedRestoreExecutionGate Build(
        SelectedRestoreConfirmationDraft? confirmationDraft,
        string enteredConfirmationText)
    {
        var entered = (enteredConfirmationText ?? "").Trim();
        if (confirmationDraft is null)
        {
            return new SelectedRestoreExecutionGate(
                SelectedRestoreConfirmationDraft.DefaultRequiredConfirmationText,
                entered,
                IsConfirmationTextMatched: false,
                IsExecutionImplemented: false,
                ["Preview selected restore confirmation before entering confirmation text."],
                ["No files were modified by this selected restore execution gate."]);
        }

        var required = confirmationDraft.RequiredConfirmationText;
        var isConfirmationTextMatched = string.Equals(
            entered,
            required,
            StringComparison.Ordinal);
        var blockers = confirmationDraft.Blockers.ToList();

        if (!isConfirmationTextMatched)
        {
            blockers.Add($"Type {required} to confirm the selected Restore Manifest before restore execution can be enabled.");
        }

        if (!confirmationDraft.IsExecutionImplemented)
        {
            blockers.Add("Selected restore execution is not available for discovered manifests in this build.");
        }

        return new SelectedRestoreExecutionGate(
            required,
            entered,
            isConfirmationTextMatched,
            confirmationDraft.IsExecutionImplemented,
            blockers,
            [
                "No files were modified by this selected restore execution gate.",
                confirmationDraft.IsExecutionImplemented
                    ? "Future selected restore execution must pass this gate before moving files."
                    : "A future selected restore execution flow must pass this gate before moving files."
            ]);
    }
}
