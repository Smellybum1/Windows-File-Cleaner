using System.Text;

namespace WindowsFileCleaner.Core;

public static class RestoreManifestFileStore
{
    public const string RestoreManifestFileName = "restore-manifest.json";

    public static RestoreManifestFileWriteResult Write(RestoreManifest manifest)
    {
        var actionRootPath = PathSafety.GetFullPath(manifest.ActionRootPath);
        var manifestPath = PathSafety.GetFullPath(manifest.ManifestPath);
        ValidateManifestPath(actionRootPath, manifestPath);

        var manifestDirectory = Path.GetDirectoryName(manifestPath);
        if (string.IsNullOrWhiteSpace(manifestDirectory))
        {
            throw new ArgumentException("Restore Manifest path must include a directory.", nameof(manifest));
        }

        Directory.CreateDirectory(manifestDirectory);

        var json = RestoreManifestJsonSerializer.Serialize(manifest);
        var tempPath = Path.Combine(
            manifestDirectory,
            $".{RestoreManifestFileName}.{Guid.NewGuid():N}.tmp");

        try
        {
            File.WriteAllText(tempPath, json, Encoding.UTF8);

            if (File.Exists(manifestPath))
            {
                File.Replace(tempPath, manifestPath, destinationBackupFileName: null);
            }
            else
            {
                File.Move(tempPath, manifestPath);
            }
        }
        finally
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }

        return new RestoreManifestFileWriteResult(
            manifestPath,
            Encoding.UTF8.GetByteCount(json),
            DateTimeOffset.UtcNow);
    }

    private static void ValidateManifestPath(string actionRootPath, string manifestPath)
    {
        if (!PathSafety.IsWithinScope(actionRootPath, manifestPath))
        {
            throw new ArgumentException("Restore Manifest path must stay inside the Quarantine Action root.", nameof(manifestPath));
        }

        if (!Path.GetFileName(manifestPath).Equals(RestoreManifestFileName, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"Restore Manifest filename must be {RestoreManifestFileName}.", nameof(manifestPath));
        }
    }
}
