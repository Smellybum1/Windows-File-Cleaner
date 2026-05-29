using System.IO;
using WindowsFileCleaner.Core;

namespace WindowsFileCleaner.App;

public sealed class QuarantinedItemRow
{
    public QuarantinedItemRow(RestoreManifestEntry entry, string manifestPath)
    {
        OriginalPath = entry.OriginalPath;
        RelativePath = entry.RelativePath;
        QuarantinePath = entry.QuarantinePath;
        IsDirectory = entry.IsDirectory;
        SizeBytes = entry.SizeBytes;
        ManifestPath = manifestPath;
        Status = FormatStatus(entry.Status);
    }

    public string Status { get; }
    public string Name => FormatName(OriginalPath);
    public string RelativePath { get; }
    public string OriginalPath { get; }
    public string QuarantinePath { get; }
    public bool IsDirectory { get; }
    public string Type => IsDirectory ? "Folder" : "File";
    public string Size => ByteSizeFormatter.Format(SizeBytes);
    public long SizeBytes { get; }
    public string ManifestPath { get; }

    private static string FormatName(string path)
    {
        try
        {
            var trimmedPath = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var name = Path.GetFileName(trimmedPath);
            return string.IsNullOrWhiteSpace(name) ? path : name;
        }
        catch (ArgumentException)
        {
            return path;
        }
    }

    private static string FormatStatus(RestoreManifestEntryStatus status)
    {
        return status switch
        {
            RestoreManifestEntryStatus.Moved => "Moved",
            RestoreManifestEntryStatus.Restored => "Restored",
            RestoreManifestEntryStatus.RestoreFailed => "Restore failed",
            _ => status.ToString()
        };
    }
}
