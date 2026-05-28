namespace WindowsFileCleaner.Core;

public sealed record PathInspectionPlan(
    string PathToCopy,
    string ExplorerFileName,
    string ExplorerArguments);

