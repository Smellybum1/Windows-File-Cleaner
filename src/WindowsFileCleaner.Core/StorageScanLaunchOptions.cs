namespace WindowsFileCleaner.Core;

public sealed record StorageScanLaunchOptions(string CleanupScopePath)
{
    public static StorageScanLaunchOptions Default()
    {
        return new StorageScanLaunchOptions(StorageScanOptions.DefaultForCurrentUser().CleanupScopePath);
    }

    public static StorageScanLaunchOptions Parse(IReadOnlyList<string> args)
    {
        var cleanupScopePath = StorageScanOptions.DefaultForCurrentUser().CleanupScopePath;

        for (var index = 0; index < args.Count; index++)
        {
            var argument = args[index];
            if (argument.Equals("--scope", StringComparison.OrdinalIgnoreCase)
                || argument.Equals("/scope", StringComparison.OrdinalIgnoreCase))
            {
                if (index + 1 >= args.Count || string.IsNullOrWhiteSpace(args[index + 1]))
                {
                    throw new ArgumentException("The --scope launch argument requires a Cleanup Scope path.");
                }

                cleanupScopePath = args[++index];
                continue;
            }

            const string scopePrefix = "--scope=";
            if (argument.StartsWith(scopePrefix, StringComparison.OrdinalIgnoreCase))
            {
                cleanupScopePath = argument[scopePrefix.Length..];
                if (string.IsNullOrWhiteSpace(cleanupScopePath))
                {
                    throw new ArgumentException("The --scope launch argument requires a Cleanup Scope path.");
                }

                continue;
            }

            throw new ArgumentException($"Unknown launch argument '{argument}'. Use --scope <path> to set the initial Cleanup Scope.");
        }

        return new StorageScanLaunchOptions(cleanupScopePath.Trim());
    }
}
