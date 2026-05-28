namespace WindowsFileCleaner.Core;

public static class QuarantineActionDraftBuilder
{
    public static QuarantineActionDraft Build(
        QuarantinePreview preview,
        RestoreManifestDraft restoreManifestDraft,
        QuarantineConfirmationDraft confirmationDraft,
        DateTimeOffset draftedAtUtc,
        string actionId)
    {
        if (confirmationDraft.HasDataBlockers)
        {
            throw new ArgumentException("Quarantine Action Draft requires a Quarantine Confirmation Draft with no data blockers.", nameof(confirmationDraft));
        }

        var consistencyBlockers = BuildConsistencyBlockers(preview, restoreManifestDraft, confirmationDraft);
        if (consistencyBlockers.Count > 0)
        {
            throw new ArgumentException("Quarantine Action Draft inputs do not agree: " + string.Join(" ", consistencyBlockers));
        }

        var normalizedActionId = NormalizeActionId(actionId);
        var cleanupScope = PathSafety.GetFullPath(preview.CleanupScopePath);
        var quarantineRoot = PathSafety.GetFullPath(preview.QuarantineRootPath);
        var actionRoot = Path.GetFullPath(Path.Combine(quarantineRoot, "actions", normalizedActionId));
        var itemsRoot = Path.GetFullPath(Path.Combine(actionRoot, "items"));
        var restoreManifestPath = Path.GetFullPath(Path.Combine(actionRoot, "restore-manifest.json"));

        EnsureWithinScope(quarantineRoot, actionRoot, "Quarantine Action root must stay inside the Quarantine root.");
        EnsureWithinScope(actionRoot, itemsRoot, "Quarantine Action items root must stay inside the action root.");
        EnsureWithinScope(actionRoot, restoreManifestPath, "Restore Manifest path must stay inside the action root.");

        return new QuarantineActionDraft(
            normalizedActionId,
            draftedAtUtc.ToUniversalTime(),
            cleanupScope,
            quarantineRoot,
            actionRoot,
            itemsRoot,
            restoreManifestPath,
            restoreManifestDraft.DraftId,
            restoreManifestDraft.Entries
                .Select(entry => BuildEntry(cleanupScope, itemsRoot, entry))
                .ToArray());
    }

    private static QuarantineActionEntryDraft BuildEntry(
        string cleanupScopePath,
        string itemsRootPath,
        RestoreManifestEntryDraft entry)
    {
        if (!PathSafety.IsWithinScope(cleanupScopePath, entry.OriginalPath))
        {
            throw new ArgumentException($"Restore Manifest Draft entry is outside the Cleanup Scope: {entry.OriginalPath}");
        }

        var relativePath = Path.GetRelativePath(cleanupScopePath, PathSafety.GetFullPath(entry.OriginalPath));
        if (relativePath.StartsWith("..", StringComparison.Ordinal) || Path.IsPathRooted(relativePath))
        {
            throw new ArgumentException($"Restore Manifest Draft entry cannot be mapped into a Quarantine Action path: {entry.OriginalPath}");
        }

        var actionQuarantinePath = Path.GetFullPath(Path.Combine(itemsRootPath, relativePath));
        EnsureWithinScope(itemsRootPath, actionQuarantinePath, "Quarantine Action destination must stay inside the items root.");

        return new QuarantineActionEntryDraft(
            PathSafety.GetFullPath(entry.OriginalPath),
            relativePath,
            PathSafety.GetFullPath(entry.QuarantinePath),
            actionQuarantinePath,
            entry.IsDirectory,
            entry.SizeBytes);
    }

    private static IReadOnlyList<string> BuildConsistencyBlockers(
        QuarantinePreview preview,
        RestoreManifestDraft restoreManifestDraft,
        QuarantineConfirmationDraft confirmationDraft)
    {
        var blockers = new List<string>();

        if (!SamePath(preview.CleanupScopePath, confirmationDraft.CleanupScopePath)
            || !SamePath(restoreManifestDraft.CleanupScopePath, confirmationDraft.CleanupScopePath))
        {
            blockers.Add("Cleanup Scope paths do not match.");
        }

        if (!SamePath(preview.QuarantineRootPath, confirmationDraft.QuarantineRootPath)
            || !SamePath(restoreManifestDraft.QuarantineRootPath, confirmationDraft.QuarantineRootPath))
        {
            blockers.Add("Quarantine root paths do not match.");
        }

        if (!string.Equals(restoreManifestDraft.DraftId, confirmationDraft.RestoreManifestDraftId, StringComparison.Ordinal))
        {
            blockers.Add("Restore Manifest Draft id does not match.");
        }

        if (preview.IncludedCount != confirmationDraft.IncludedCount
            || restoreManifestDraft.EntryCount != confirmationDraft.IncludedCount)
        {
            blockers.Add("Included entry counts do not match.");
        }

        if (preview.IncludedBytes != confirmationDraft.IncludedBytes
            || restoreManifestDraft.TotalBytes != confirmationDraft.IncludedBytes)
        {
            blockers.Add("Included byte counts do not match.");
        }

        return blockers;
    }

    private static string NormalizeActionId(string actionId)
    {
        var normalized = (actionId ?? "").Trim();
        if (normalized.Length == 0)
        {
            throw new ArgumentException("Quarantine Action Draft id is required.", nameof(actionId));
        }

        if (normalized is "." or ".."
            || normalized.Contains(Path.DirectorySeparatorChar)
            || normalized.Contains(Path.AltDirectorySeparatorChar)
            || normalized.Contains(Path.VolumeSeparatorChar)
            || normalized.Any(character => !(char.IsAsciiLetterOrDigit(character) || character is '-' or '_')))
        {
            throw new ArgumentException("Quarantine Action Draft id must contain only letters, digits, hyphens, or underscores.", nameof(actionId));
        }

        return normalized;
    }

    private static void EnsureWithinScope(string scopePath, string candidatePath, string message)
    {
        if (!PathSafety.IsWithinScope(scopePath, candidatePath))
        {
            throw new ArgumentException(message);
        }
    }

    private static bool SamePath(string left, string right)
    {
        return PathSafety.GetFullPath(left).Equals(PathSafety.GetFullPath(right), StringComparison.OrdinalIgnoreCase);
    }
}
