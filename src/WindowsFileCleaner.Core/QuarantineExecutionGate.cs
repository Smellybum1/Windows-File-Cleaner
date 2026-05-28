namespace WindowsFileCleaner.Core;

public sealed record QuarantineExecutionGate(
    string RequiredConfirmationText,
    string EnteredConfirmationText,
    bool IsConfirmationTextMatched,
    bool IsExecutionImplemented,
    IReadOnlyList<string> Blockers,
    IReadOnlyList<string> ReviewNotes)
{
    public bool HasBlockers => Blockers.Count > 0;
    public bool CanExecute => IsConfirmationTextMatched && IsExecutionImplemented && !HasBlockers;
}
