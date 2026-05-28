using System.Globalization;

namespace WindowsFileCleaner.Core;

public static class ByteSizeFormatter
{
    private static readonly string[] Units = ["B", "KB", "MB", "GB", "TB"];

    public static string Format(long bytes)
    {
        if (bytes < 0)
        {
            return "0 B";
        }

        double value = bytes;
        var unitIndex = 0;

        while (value >= 1024 && unitIndex < Units.Length - 1)
        {
            value /= 1024;
            unitIndex++;
        }

        return unitIndex == 0
            ? string.Create(CultureInfo.InvariantCulture, $"{bytes} {Units[unitIndex]}")
            : string.Create(CultureInfo.InvariantCulture, $"{value:0.##} {Units[unitIndex]}");
    }
}

