using System.Text;

namespace WindowsFileCleaner.Core;

public static class SelectedFileContentPreviewBuilder
{
    public const int DefaultMaxBytes = 8192;
    public const int DefaultMaxLines = 80;
    private const int MaxPreviewCharacters = 12000;

    private static readonly UTF8Encoding StrictUtf8 = new(false, true);

    public static SelectedFileContentPreview Build(
        StorageEntry entry,
        int maxBytes = DefaultMaxBytes,
        int maxLines = DefaultMaxLines)
    {
        if (maxBytes < 128)
        {
            throw new ArgumentOutOfRangeException(nameof(maxBytes), "Preview byte limit must be at least 128 bytes.");
        }

        if (maxLines < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLines), "Preview line limit must be at least one line.");
        }

        if (entry.IsDirectory)
        {
            return Unavailable(
                "Folder selected",
                "File preview is available only for files. Use Largest immediate children for folder review.");
        }

        if (!entry.IsAccessible)
        {
            return Unavailable(
                "Access issue",
                string.IsNullOrWhiteSpace(entry.ErrorMessage)
                    ? "The selected file was not accessible during scan."
                    : $"The selected file was not accessible during scan: {entry.ErrorMessage}");
        }

        if (entry.BloatCategories.Contains(BloatCategory.CredentialData))
        {
            return Unavailable(
                "Credential data",
                "The selected file looks like credential, key, password manager, or authentication data, so its contents are not shown.");
        }

        try
        {
            var path = PathSafety.GetFullPath(entry.FullPath);
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite | FileShare.Delete,
                bufferSize: 4096,
                FileOptions.SequentialScan);

            var readLimit = maxBytes + 1;
            var buffer = new byte[readLimit];
            var bytesRead = stream.Read(buffer, 0, readLimit);
            if (bytesRead == 0)
            {
                return new SelectedFileContentPreview(
                    IsContentShown: true,
                    Label: "Empty file",
                    Message: "The selected file is empty. No files were modified.",
                    Content: "",
                    IsTruncated: false,
                    BytesRead: 0);
            }

            var isTruncated = bytesRead > maxBytes || entry.SizeBytes > maxBytes;
            var sampleLength = Math.Min(bytesRead, maxBytes);
            var sample = buffer.AsSpan(0, sampleLength);

            if (!TryDecodeText(sample, out var text, out var encodingName))
            {
                return Unavailable(
                    "Binary or unsupported file",
                    $"The first {ByteSizeFormatter.Format(sampleLength)} does not look like plain text. Use Open in Explorer for manual inspection.");
            }

            var limitedText = LimitText(text, maxLines, ref isTruncated);
            return new SelectedFileContentPreview(
                IsContentShown: true,
                Label: "Text preview",
                Message: $"Showing up to {ByteSizeFormatter.Format(sampleLength)} of {entry.SizeDisplay} as {encodingName}. No files were modified.",
                Content: limitedText,
                IsTruncated: isTruncated,
                BytesRead: sampleLength);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unavailable("Preview unavailable", $"Access was denied while reading the selected file: {ex.Message}");
        }
        catch (IOException ex)
        {
            return Unavailable("Preview unavailable", $"The selected file could not be read: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return Unavailable("Preview unavailable", $"The selected path could not be read: {ex.Message}");
        }
        catch (NotSupportedException ex)
        {
            return Unavailable("Preview unavailable", $"The selected path could not be read: {ex.Message}");
        }
    }

    private static SelectedFileContentPreview Unavailable(string label, string message)
    {
        return new SelectedFileContentPreview(
            IsContentShown: false,
            Label: label,
            Message: $"{message} No files were modified.",
            Content: "",
            IsTruncated: false,
            BytesRead: 0);
    }

    private static bool TryDecodeText(ReadOnlySpan<byte> sample, out string text, out string encodingName)
    {
        if (sample.Length >= 3 && sample[0] == 0xEF && sample[1] == 0xBB && sample[2] == 0xBF)
        {
            text = StrictUtf8.GetString(sample[3..]);
            encodingName = "UTF-8";
            return true;
        }

        if (sample.Length >= 2 && sample[0] == 0xFF && sample[1] == 0xFE)
        {
            text = Encoding.Unicode.GetString(sample[2..]);
            encodingName = "UTF-16 LE";
            return true;
        }

        if (sample.Length >= 2 && sample[0] == 0xFE && sample[1] == 0xFF)
        {
            text = Encoding.BigEndianUnicode.GetString(sample[2..]);
            encodingName = "UTF-16 BE";
            return true;
        }

        if (LooksBinary(sample))
        {
            text = "";
            encodingName = "";
            return false;
        }

        try
        {
            text = StrictUtf8.GetString(sample);
            encodingName = "UTF-8";
            return true;
        }
        catch (DecoderFallbackException)
        {
            text = Encoding.Latin1.GetString(sample);
            encodingName = "Latin-1";
            return !LooksLikeControlHeavyText(text);
        }
    }

    private static bool LooksBinary(ReadOnlySpan<byte> sample)
    {
        var controlCount = 0;
        foreach (var value in sample)
        {
            if (value == 0)
            {
                return true;
            }

            if (value < 32 && value != '\r' && value != '\n' && value != '\t')
            {
                controlCount++;
            }
        }

        return controlCount > Math.Max(4, sample.Length / 20);
    }

    private static bool LooksLikeControlHeavyText(string text)
    {
        var controlCount = text.Count(character =>
            char.IsControl(character)
            && character is not '\r'
            && character is not '\n'
            && character is not '\t');

        return controlCount > Math.Max(4, text.Length / 20);
    }

    private static string LimitText(string text, int maxLines, ref bool isTruncated)
    {
        var normalized = text.Replace("\r\n", "\n", StringComparison.Ordinal).Replace('\r', '\n');
        var lines = normalized.Split('\n');
        if (lines.Length > maxLines)
        {
            isTruncated = true;
        }

        var limited = string.Join(Environment.NewLine, lines.Take(maxLines));
        if (limited.Length > MaxPreviewCharacters)
        {
            limited = limited[..MaxPreviewCharacters];
            isTruncated = true;
        }

        return isTruncated
            ? $"{limited}{Environment.NewLine}... preview truncated ..."
            : limited;
    }
}
