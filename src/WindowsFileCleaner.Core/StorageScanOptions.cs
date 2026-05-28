namespace WindowsFileCleaner.Core;

public sealed record StorageScanOptions(
    string CleanupScopePath,
    int MaxDisplayChildrenPerFolder = 250)
{
    public static StorageScanOptions DefaultForCurrentUser()
    {
        return new StorageScanOptions(@"C:\Users\moxhe");
    }
}

