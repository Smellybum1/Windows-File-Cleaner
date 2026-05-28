namespace WindowsFileCleaner.Core;

public sealed record CleanupScopeSafetyNote(
    string Label,
    string Message,
    bool IsFixtureScope,
    bool IsRealUserProfileScope);
