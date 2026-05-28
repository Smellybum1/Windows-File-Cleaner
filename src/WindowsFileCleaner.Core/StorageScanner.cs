namespace WindowsFileCleaner.Core;

public sealed class StorageScanner
{
    private readonly CleanupCandidateClassifier _classifier;

    public StorageScanner(CleanupCandidateClassifier? classifier = null)
    {
        _classifier = classifier ?? new CleanupCandidateClassifier();
    }

    public StorageScanResult Scan(StorageScanOptions options, CancellationToken cancellationToken = default)
    {
        var scope = PathSafety.GetFullPath(options.CleanupScopePath);
        if (!Directory.Exists(scope))
        {
            throw new DirectoryNotFoundException($"Cleanup Scope does not exist: {scope}");
        }

        var startedAt = DateTimeOffset.UtcNow;
        var root = ScanDirectory(scope, scope, options, cancellationToken);
        var completedAt = DateTimeOffset.UtcNow;

        return new StorageScanResult(scope, startedAt, completedAt, root);
    }

    private StorageEntry ScanDirectory(
        string scope,
        string directoryPath,
        StorageScanOptions options,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!PathSafety.IsWithinScope(scope, directoryPath))
        {
            throw new InvalidOperationException($"Scanner refused to leave Cleanup Scope: {directoryPath}");
        }

        var info = new DirectoryInfo(directoryPath);
        var snapshot = Snapshot(info);

        if (snapshot.IsReparsePoint)
        {
            return BuildEntry(snapshot, 0, [], null);
        }

        var children = new List<StorageEntry>();
        string? errorMessage = null;

        try
        {
            foreach (var child in info.EnumerateFileSystemInfos())
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (child is DirectoryInfo childDirectory)
                {
                    children.Add(ScanDirectory(scope, childDirectory.FullName, options, cancellationToken));
                }
                else if (child is FileInfo childFile)
                {
                    children.Add(ScanFile(scope, childFile));
                }
            }
        }
        catch (Exception ex) when (IsAccessException(ex))
        {
            errorMessage = ex.Message;
            snapshot = snapshot with { IsAccessible = false };
        }

        var orderedChildren = children
            .OrderByDescending(child => child.SizeBytes)
            .ThenBy(child => child.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return BuildEntry(snapshot, orderedChildren.Sum(child => child.SizeBytes), orderedChildren, errorMessage);
    }

    private StorageEntry ScanFile(string scope, FileInfo info)
    {
        if (!PathSafety.IsWithinScope(scope, info.FullName))
        {
            throw new InvalidOperationException($"Scanner refused to leave Cleanup Scope: {info.FullName}");
        }

        var snapshot = Snapshot(info);

        if (!snapshot.IsAccessible)
        {
            return BuildEntry(snapshot, 0, [], "File metadata could not be read.");
        }

        long size;
        string? errorMessage = null;

        try
        {
            size = info.Length;
        }
        catch (Exception ex) when (IsAccessException(ex))
        {
            size = 0;
            errorMessage = ex.Message;
            snapshot = snapshot with { IsAccessible = false };
        }

        return BuildEntry(snapshot, size, [], errorMessage);
    }

    private StorageEntry BuildEntry(
        PathSnapshot snapshot,
        long sizeBytes,
        IReadOnlyList<StorageEntry> children,
        string? errorMessage)
    {
        var classification = _classifier.Classify(snapshot, sizeBytes);

        return new StorageEntry(
            snapshot.FullPath,
            snapshot.Name,
            snapshot.IsDirectory,
            sizeBytes,
            snapshot.LastModifiedUtc,
            snapshot.IsAccessible,
            snapshot.IsReparsePoint,
            errorMessage,
            classification.BloatCategories,
            classification.ImportanceRating,
            classification.DeletionRecommendation,
            classification.Evidence,
            children);
    }

    private static PathSnapshot Snapshot(FileSystemInfo info)
    {
        try
        {
            var attributes = info.Attributes;
            return new PathSnapshot(
                info.FullName,
                info.Name,
                (attributes & FileAttributes.Directory) != 0,
                true,
                (attributes & FileAttributes.ReparsePoint) != 0,
                info.LastWriteTimeUtc == DateTime.MinValue
                    ? null
                    : new DateTimeOffset(info.LastWriteTimeUtc, TimeSpan.Zero));
        }
        catch (Exception ex) when (IsAccessException(ex))
        {
            return new PathSnapshot(
                info.FullName,
                info.Name,
                info is DirectoryInfo,
                false,
                false,
                null);
        }
    }

    private static bool IsAccessException(Exception ex)
    {
        return ex is UnauthorizedAccessException
            or IOException
            or PathTooLongException
            or System.Security.SecurityException;
    }
}
