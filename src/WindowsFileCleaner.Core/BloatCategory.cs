namespace WindowsFileCleaner.Core;

public enum BloatCategory
{
    Unknown,
    ProfileContainer,
    ApplicationDataArea,
    BrowserData,
    OldDownload,
    TemporaryFolder,
    InstallerCache,
    AppCache,
    GpuShaderCache,
    DuplicateFileCandidate,
    OldGameFile,
    NodePackageCache,
    PythonPackageCache,
    WindowsAppLeftover,
    ProtectedLocation,
    ReparsePoint,
    AccessIssue
}
