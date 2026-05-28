namespace WindowsFileCleaner.Core;

public static class QuarantineExecutionGateBuilder
{
    public static QuarantineExecutionGate Build(
        QuarantineConfirmationDraft? confirmationDraft,
        string enteredConfirmationText)
    {
        var entered = (enteredConfirmationText ?? "").Trim();
        if (confirmationDraft is null)
        {
            return new QuarantineExecutionGate(
                QuarantineConfirmationDraft.DefaultRequiredConfirmationText,
                entered,
                IsConfirmationTextMatched: false,
                IsExecutionImplemented: false,
                ["Create a Quarantine Preview before entering confirmation text."],
                ["No files were modified by this execution gate."]);
        }

        var required = confirmationDraft.RequiredConfirmationText;
        var isConfirmationTextMatched = string.Equals(
            entered,
            required,
            StringComparison.Ordinal);
        var blockers = confirmationDraft.Blockers.ToList();

        if (!isConfirmationTextMatched)
        {
            blockers.Add($"Type {required} to confirm the reviewed Quarantine Preview before execution can be enabled.");
        }

        if (!confirmationDraft.IsExecutionImplemented)
        {
            blockers.Add("Quarantine execution is not available for this Cleanup Scope in this build.");
        }

        return new QuarantineExecutionGate(
            required,
            entered,
            isConfirmationTextMatched,
            confirmationDraft.IsExecutionImplemented,
            blockers,
            [
                "No files were modified by this execution gate.",
                confirmationDraft.IsExecutionImplemented
                    ? "Fixture-only execution must pass this gate before moving files."
                    : "A future execution flow must pass this gate before moving files."
            ]);
    }
}
