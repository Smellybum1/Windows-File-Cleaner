namespace WindowsFileCleaner.Core;

public sealed record QuarantineManifestDiscoveryIssue(
    string Path,
    string Message);
