namespace WindowsFileCleaner.Core;

public static class QuarantineRootSafetyNoteBuilder
{
    public static QuarantineRootSafetyNote Build(string quarantineRootPath)
    {
        var candidate = string.IsNullOrWhiteSpace(quarantineRootPath)
            ? QuarantinePreviewBuilder.DefaultQuarantineRootPath
            : quarantineRootPath.Trim();

        try
        {
            if (!Path.IsPathFullyQualified(candidate))
            {
                return new QuarantineRootSafetyNote(
                    "Choose Full Quarantine Root",
                    "Use a fully qualified preview root such as D:\\WindowsFileCleanerQuarantine. No folders were created.",
                    candidate,
                    CanPreview: false,
                    IsPreferredDrive: false);
            }

            var fullPath = Path.GetFullPath(candidate);
            var root = Path.GetPathRoot(fullPath);
            var isPreferredDrive = string.Equals(root, @"D:\", StringComparison.OrdinalIgnoreCase);
            return new QuarantineRootSafetyNote(
                isPreferredDrive ? "Preferred Quarantine Root" : "Non-D: Quarantine Root",
                isPreferredDrive
                    ? "Preview destinations will use this D: root. Preview does not create folders, move files, or write manifests."
                    : "Preview can use this fully qualified root, but D: remains preferred for future quarantine storage. Preview does not create folders.",
                fullPath,
                CanPreview: true,
                isPreferredDrive);
        }
        catch (ArgumentException)
        {
            return BuildInvalid(candidate);
        }
        catch (NotSupportedException)
        {
            return BuildInvalid(candidate);
        }
        catch (PathTooLongException)
        {
            return BuildInvalid(candidate);
        }
    }

    private static QuarantineRootSafetyNote BuildInvalid(string candidate)
    {
        return new QuarantineRootSafetyNote(
            "Invalid Quarantine Root",
            "The Quarantine root path could not be parsed for preview. No folders were created.",
            candidate,
            CanPreview: false,
            IsPreferredDrive: false);
    }
}
