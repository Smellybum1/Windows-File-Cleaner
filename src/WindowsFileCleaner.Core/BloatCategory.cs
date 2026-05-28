namespace WindowsFileCleaner.Core;

public enum BloatCategory
{
    Unknown,
    CleanupScopeRoot,
    ProfileContainer,
    ApplicationDataArea,
    BrowserData,
    CloudSyncData,
    CredentialData,
    OldDownload,
    TemporaryFolder,
    InstallerCache,
    AppCache,
    GpuShaderCache,
    DuplicateFileCandidate,
    LargeOldFile,
    OldGameFile,
    NodePackageCache,
    PythonPackageCache,
    WindowsAppData,
    WindowsAppLeftover,
    InstalledApplication,
    GameData,
    ProtectedLocation,
    ReparsePoint,
    AccessIssue
}
