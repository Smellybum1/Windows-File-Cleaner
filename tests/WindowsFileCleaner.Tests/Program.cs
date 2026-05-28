using WindowsFileCleaner.Core;

var tests = new StorageScanTests();
tests.ScannerTotalsFilesAndClassifiesCandidates();
tests.ScannerRefusesToLeaveCleanupScope();
tests.ByteSizeFormatterUsesReadableUnits();

Console.WriteLine("All WindowsFileCleaner.Tests checks passed.");

internal sealed class StorageScanTests
{
    public void ScannerTotalsFilesAndClassifiesCandidates()
    {
        using var fixture = TestFixture.Create();

        fixture.WriteFile(@"Downloads\old-installer.msi", 1024 * 1024 * 3, DateTimeOffset.UtcNow.AddDays(-120));
        fixture.WriteFile(@"AppData\Local\Temp\scratch.tmp", 1024 * 256, DateTimeOffset.UtcNow.AddDays(-2));
        fixture.WriteFile(@".codex\config.json", 1024, DateTimeOffset.UtcNow);
        fixture.WriteFile(@"Documents\important.txt", 1024, DateTimeOffset.UtcNow);

        var scanner = new StorageScanner();
        var result = scanner.Scan(new StorageScanOptions(fixture.RootPath));
        var rows = Flatten(result.Root).ToArray();

        Assert(result.TotalBytes == 3_409_920, $"Expected total bytes to be 3,409,920 but got {result.TotalBytes}.");
        Assert(result.InaccessibleCount == 0, "Fixture scan should have no inaccessible paths.");

        var installer = Single(rows, "old-installer.msi");
        Assert(installer.BloatCategories.Contains(BloatCategory.OldDownload), "Old installer should be categorized as an old download.");
        Assert(installer.BloatCategories.Contains(BloatCategory.InstallerCache), "Old installer should be categorized as installer cache.");
        Assert(installer.ImportanceRating == ImportanceRating.LikelySafe, "Old installer should be likely safe.");
        Assert(installer.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate, "Old installer should be a quarantine candidate.");

        var codexFolder = Single(rows, ".codex");
        Assert(codexFolder.ImportanceRating == ImportanceRating.HighRisk, "Codex folder should be high risk.");
        Assert(codexFolder.DeletionRecommendation == DeletionRecommendation.Keep, "Codex folder should be kept.");

        var documentsFolder = Single(rows, "Documents");
        Assert(documentsFolder.ImportanceRating == ImportanceRating.HighRisk, "Documents folder should be high risk.");
    }

    public void ScannerRefusesToLeaveCleanupScope()
    {
        using var fixture = TestFixture.Create();
        fixture.WriteFile(@"Downloads\file.txt", 10, DateTimeOffset.UtcNow);

        var outside = Path.GetFullPath(Path.Combine(fixture.RootPath, "..", "outside.txt"));
        Assert(!PathSafety.IsWithinScope(fixture.RootPath, outside), "Sibling path must not be inside Cleanup Scope.");
    }

    public void ByteSizeFormatterUsesReadableUnits()
    {
        Assert(ByteSizeFormatter.Format(0) == "0 B", "0 bytes should be 0 B.");
        Assert(ByteSizeFormatter.Format(1024) == "1 KB", "1024 bytes should be 1 KB.");
        Assert(ByteSizeFormatter.Format(1024 * 1024 * 3) == "3 MB", "3 MiB should be 3 MB.");
    }

    private static StorageEntry Single(IEnumerable<StorageEntry> entries, string name)
    {
        return entries.Single(entry => entry.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private static IEnumerable<StorageEntry> Flatten(StorageEntry entry)
    {
        yield return entry;

        foreach (var child in entry.Children.SelectMany(Flatten))
        {
            yield return child;
        }
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}

internal sealed class TestFixture : IDisposable
{
    private TestFixture(string rootPath)
    {
        RootPath = rootPath;
    }

    public string RootPath { get; }

    public static TestFixture Create()
    {
        var root = Path.Combine(
            Environment.CurrentDirectory,
            "test-fixtures",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(root);
        return new TestFixture(root);
    }

    public void WriteFile(string relativePath, int byteCount, DateTimeOffset lastModifiedUtc)
    {
        var fullPath = Path.Combine(RootPath, relativePath);
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllBytes(fullPath, Enumerable.Repeat((byte)'x', byteCount).ToArray());
        File.SetLastWriteTimeUtc(fullPath, lastModifiedUtc.UtcDateTime);
    }

    public void Dispose()
    {
        if (Directory.Exists(RootPath))
        {
            Directory.Delete(RootPath, recursive: true);
        }
    }
}
