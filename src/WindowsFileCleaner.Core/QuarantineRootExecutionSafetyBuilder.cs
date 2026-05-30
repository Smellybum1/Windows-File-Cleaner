namespace WindowsFileCleaner.Core;

public static class QuarantineRootExecutionSafetyBuilder
{
    public static QuarantineRootExecutionSafety Build(
        QuarantineActionDraft actionDraft,
        bool nonPreferredQuarantineRootAcknowledged = false,
        long manifestOverheadBytes = QuarantineRootExecutionSafety.DefaultManifestOverheadBytes,
        long? requiredBytesOverride = null,
        long? availableFreeBytesOverride = null)
    {
        return Build(
            actionDraft.CleanupScopePath,
            actionDraft.QuarantineRootPath,
            actionDraft.ActionRootPath,
            actionDraft.ItemsRootPath,
            actionDraft.RestoreManifestPath,
            actionDraft.Entries.Select(entry => entry.ActionQuarantinePath).ToArray(),
            actionDraft.TotalBytes,
            nonPreferredQuarantineRootAcknowledged,
            manifestOverheadBytes,
            requiredBytesOverride,
            availableFreeBytesOverride);
    }

    public static QuarantineRootExecutionSafety Build(
        string cleanupScopePath,
        string quarantineRootPath,
        string actionRootPath,
        string itemsRootPath,
        string restoreManifestPath,
        IReadOnlyList<string> itemDestinationPaths,
        long plannedMoveBytes,
        bool nonPreferredQuarantineRootAcknowledged = false,
        long manifestOverheadBytes = QuarantineRootExecutionSafety.DefaultManifestOverheadBytes,
        long? requiredBytesOverride = null,
        long? availableFreeBytesOverride = null)
    {
        var blockers = new List<string>();
        var trimmedRoot = (quarantineRootPath ?? "").Trim();
        var isFullyQualifiedRoot = !string.IsNullOrWhiteSpace(trimmedRoot) && Path.IsPathFullyQualified(trimmedRoot);

        string cleanupScope = "";
        string quarantineRoot = trimmedRoot;
        string actionRoot = (actionRootPath ?? "").Trim();
        string itemsRoot = (itemsRootPath ?? "").Trim();
        string restoreManifest = (restoreManifestPath ?? "").Trim();

        TryNormalize(cleanupScopePath, "Cleanup Scope", blockers, out cleanupScope);
        TryNormalize(trimmedRoot, "Quarantine Root", blockers, out quarantineRoot);
        TryNormalize(actionRootPath, "Quarantine action root", blockers, out actionRoot);
        TryNormalize(itemsRootPath, "Quarantine items root", blockers, out itemsRoot);
        TryNormalize(restoreManifestPath, "Restore Manifest path", blockers, out restoreManifest);

        if (!isFullyQualifiedRoot)
        {
            blockers.Add("Quarantine Root must be fully qualified before execution.");
        }

        var isPreferredRoot = IsPreferredQuarantineRoot(quarantineRoot);
        if (!isPreferredRoot && !nonPreferredQuarantineRootAcknowledged)
        {
            blockers.Add("Non-D: Quarantine roots require an extra acknowledgement before execution.");
        }

        AddContainmentBlockers(cleanupScope, quarantineRoot, blockers);
        AddLayoutBlockers(quarantineRoot, actionRoot, itemsRoot, restoreManifest, itemDestinationPaths, blockers);
        AddCollisionBlockers(actionRoot, restoreManifest, itemDestinationPaths, blockers);

        var requiredBytes = requiredBytesOverride ?? checked(plannedMoveBytes + manifestOverheadBytes);
        var availableFreeBytes = availableFreeBytesOverride ?? TryGetAvailableFreeBytes(quarantineRoot);
        var hasCapacityEvidence = availableFreeBytes is not null;
        var hasEnoughFreeSpace = availableFreeBytes is not null && availableFreeBytes.Value >= requiredBytes;

        if (!hasCapacityEvidence)
        {
            blockers.Add("Available free space for the Quarantine Root could not be checked.");
        }
        else if (!hasEnoughFreeSpace)
        {
            blockers.Add($"Quarantine Root has insufficient free space. Required: {ByteSizeFormatter.Format(requiredBytes)}. Available: {ByteSizeFormatter.Format(availableFreeBytes!.Value)}.");
        }

        return new QuarantineRootExecutionSafety(
            cleanupScope,
            quarantineRoot,
            actionRoot,
            itemsRoot,
            restoreManifest,
            plannedMoveBytes,
            manifestOverheadBytes,
            requiredBytes,
            availableFreeBytes,
            isFullyQualifiedRoot,
            isPreferredRoot,
            nonPreferredQuarantineRootAcknowledged,
            hasCapacityEvidence,
            hasEnoughFreeSpace,
            blockers,
            BuildReviewNotes(isPreferredRoot, nonPreferredQuarantineRootAcknowledged));
    }

    private static void AddContainmentBlockers(
        string cleanupScopePath,
        string quarantineRootPath,
        List<string> blockers)
    {
        if (string.IsNullOrWhiteSpace(cleanupScopePath) || string.IsNullOrWhiteSpace(quarantineRootPath))
        {
            return;
        }

        if (SafeIsWithinScope(cleanupScopePath, quarantineRootPath))
        {
            blockers.Add("Quarantine Root must not be inside the Cleanup Scope.");
        }

        if (SafeIsWithinScope(quarantineRootPath, cleanupScopePath))
        {
            blockers.Add("Quarantine Root must not be a parent of the Cleanup Scope.");
        }
    }

    private static void AddLayoutBlockers(
        string quarantineRootPath,
        string actionRootPath,
        string itemsRootPath,
        string restoreManifestPath,
        IReadOnlyList<string> itemDestinationPaths,
        List<string> blockers)
    {
        if (!SafeIsWithinScope(quarantineRootPath, actionRootPath))
        {
            blockers.Add("Quarantine action root must stay inside the Quarantine Root.");
        }

        if (!SafeIsWithinScope(actionRootPath, itemsRootPath))
        {
            blockers.Add("Quarantine items root must stay inside the action root.");
        }

        if (!SafeIsWithinScope(actionRootPath, restoreManifestPath))
        {
            blockers.Add("Restore Manifest path must stay inside the action root.");
        }

        foreach (var destinationPath in itemDestinationPaths)
        {
            if (!SafeIsWithinScope(itemsRootPath, destinationPath))
            {
                blockers.Add($"Quarantine item destination must stay inside the items root: {destinationPath}");
            }
        }
    }

    private static void AddCollisionBlockers(
        string actionRootPath,
        string restoreManifestPath,
        IReadOnlyList<string> itemDestinationPaths,
        List<string> blockers)
    {
        if (PathExists(actionRootPath))
        {
            blockers.Add($"Quarantine action root already exists: {actionRootPath}");
        }

        if (PathExists(restoreManifestPath))
        {
            blockers.Add($"Restore Manifest path already exists: {restoreManifestPath}");
        }

        foreach (var destinationPath in itemDestinationPaths)
        {
            if (PathExists(destinationPath))
            {
                blockers.Add($"Quarantine item destination already exists: {destinationPath}");
            }
        }
    }

    private static IReadOnlyList<string> BuildReviewNotes(
        bool isPreferredRoot,
        bool nonPreferredQuarantineRootAcknowledged)
    {
        var rootNote = isPreferredRoot
            ? "D: is the preferred Quarantine Root for execution."
            : nonPreferredQuarantineRootAcknowledged
                ? "Non-D: Quarantine Root acknowledgement is present, but unsafe roots remain blocked."
                : "Non-D: Quarantine Roots require an extra acknowledgement before execution.";

        return
        [
            "No folders were created and no files were modified by this Quarantine Root Execution Safety check.",
            rootNote
        ];
    }

    private static bool TryNormalize(
        string? path,
        string label,
        List<string> blockers,
        out string fullPath)
    {
        try
        {
            fullPath = PathSafety.GetFullPath(path ?? "");
            return true;
        }
        catch (ArgumentException)
        {
            fullPath = path ?? "";
        }
        catch (NotSupportedException)
        {
            fullPath = path ?? "";
        }
        catch (PathTooLongException)
        {
            fullPath = path ?? "";
        }

        blockers.Add($"{label} path could not be normalized for execution safety.");
        return false;
    }

    private static bool SafeIsWithinScope(string scopePath, string candidatePath)
    {
        try
        {
            return PathSafety.IsWithinScope(scopePath, candidatePath);
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch (NotSupportedException)
        {
            return false;
        }
    }

    private static bool PathExists(string path)
    {
        try
        {
            return File.Exists(path) || Directory.Exists(path);
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch (NotSupportedException)
        {
            return false;
        }
        catch (PathTooLongException)
        {
            return false;
        }
    }

    private static bool IsPreferredQuarantineRoot(string quarantineRootPath)
    {
        try
        {
            var root = Path.GetPathRoot(quarantineRootPath);
            return root is not null && root.Equals(@"D:\", StringComparison.OrdinalIgnoreCase);
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    private static long? TryGetAvailableFreeBytes(string quarantineRootPath)
    {
        try
        {
            var root = Path.GetPathRoot(quarantineRootPath);
            if (string.IsNullOrWhiteSpace(root))
            {
                return null;
            }

            var drive = new DriveInfo(root);
            return drive.IsReady ? drive.AvailableFreeSpace : null;
        }
        catch (ArgumentException)
        {
            return null;
        }
        catch (IOException)
        {
            return null;
        }
        catch (UnauthorizedAccessException)
        {
            return null;
        }
    }
}
