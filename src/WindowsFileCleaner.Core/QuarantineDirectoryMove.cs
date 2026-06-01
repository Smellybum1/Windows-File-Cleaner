namespace WindowsFileCleaner.Core;

public static class QuarantineDirectoryMove
{
    public static void Move(
        string sourcePath,
        string destinationPath,
        bool forceCopyDeleteFallback = false,
        string copiedButCouldNotDeleteSourceMessage = "Copied directory to destination, but could not remove the source directory. Recovery review is required. ")
    {
        if (string.IsNullOrWhiteSpace(sourcePath))
        {
            throw new IOException("Source directory path is required.");
        }

        if (string.IsNullOrWhiteSpace(destinationPath))
        {
            throw new IOException("Destination directory path is required.");
        }

        if (!Directory.Exists(sourcePath))
        {
            throw new IOException($"Source directory no longer exists: {sourcePath}");
        }

        if (Directory.Exists(destinationPath) || File.Exists(destinationPath))
        {
            throw new IOException($"Destination already exists: {destinationPath}");
        }

        if (IsReparsePoint(sourcePath))
        {
            throw new IOException($"Source directory became a reparse point during move: {sourcePath}");
        }

        var destinationParent = Path.GetDirectoryName(destinationPath);
        if (string.IsNullOrWhiteSpace(destinationParent))
        {
            throw new IOException($"Destination path has no parent folder: {destinationPath}");
        }

        Directory.CreateDirectory(destinationParent);

        if (!forceCopyDeleteFallback && HaveSameRoot(sourcePath, destinationPath))
        {
            Directory.Move(sourcePath, destinationPath);
            return;
        }

        CopyThenDeleteDirectory(
            sourcePath,
            destinationPath,
            destinationParent,
            copiedButCouldNotDeleteSourceMessage);
    }

    private static bool HaveSameRoot(string sourcePath, string destinationPath)
    {
        var sourceRoot = Path.GetPathRoot(Path.GetFullPath(sourcePath));
        var destinationRoot = Path.GetPathRoot(Path.GetFullPath(destinationPath));
        return string.Equals(sourceRoot, destinationRoot, StringComparison.OrdinalIgnoreCase);
    }

    private static void CopyThenDeleteDirectory(
        string sourcePath,
        string destinationPath,
        string destinationParent,
        string copiedButCouldNotDeleteSourceMessage)
    {
        var stagingPath = Path.Combine(
            destinationParent,
            $".{Path.GetFileName(destinationPath)}.copying-{Guid.NewGuid():N}");

        try
        {
            CopyDirectoryTree(sourcePath, stagingPath);

            if (Directory.Exists(destinationPath) || File.Exists(destinationPath))
            {
                throw new IOException($"Destination already exists: {destinationPath}");
            }

            Directory.Move(stagingPath, destinationPath);

            try
            {
                Directory.Delete(sourcePath, recursive: true);
            }
            catch (Exception ex)
            {
                throw new IOException(
                    copiedButCouldNotDeleteSourceMessage
                    + ex.Message,
                    ex);
            }
        }
        catch
        {
            TryDeleteDirectory(stagingPath);
            throw;
        }
    }

    private static void CopyDirectoryTree(string sourcePath, string stagingPath)
    {
        var pendingDirectories = new Stack<(string SourcePath, string DestinationPath)>();
        pendingDirectories.Push((sourcePath, stagingPath));

        while (pendingDirectories.Count > 0)
        {
            var current = pendingDirectories.Pop();
            if (IsReparsePoint(current.SourcePath))
            {
                throw new IOException($"Source directory became a reparse point during copy: {current.SourcePath}");
            }

            Directory.CreateDirectory(current.DestinationPath);

            foreach (var filePath in Directory.EnumerateFiles(current.SourcePath))
            {
                if (IsReparsePoint(filePath))
                {
                    throw new IOException($"Source file became a reparse point during copy: {filePath}");
                }

                var destinationFilePath = Path.Combine(current.DestinationPath, Path.GetFileName(filePath));
                File.Copy(filePath, destinationFilePath, overwrite: false);
            }

            foreach (var childDirectoryPath in Directory.EnumerateDirectories(current.SourcePath))
            {
                if (IsReparsePoint(childDirectoryPath))
                {
                    throw new IOException($"Source directory became a reparse point during copy: {childDirectoryPath}");
                }

                pendingDirectories.Push((
                    childDirectoryPath,
                    Path.Combine(current.DestinationPath, Path.GetFileName(childDirectoryPath))));
            }
        }
    }

    private static void TryDeleteDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }
        catch
        {
        }
    }

    private static bool IsReparsePoint(string path)
    {
        return File.GetAttributes(path).HasFlag(FileAttributes.ReparsePoint);
    }
}
