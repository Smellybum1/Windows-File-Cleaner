[CmdletBinding()]
param(
    [string]$QuarantineRoot = "D:\WindowsFileCleanerQuarantine",

    [string]$CleanupScope,

    [switch]$ShowEntries,

    [switch]$RecoveryReviewOnly,

    [switch]$UndoWorkOnly,

    [switch]$RequireAny,

    [switch]$RequireNoRecoveryReview,

    [switch]$RequireNoUndoWork
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Format-ByteSize {
    param(
        [long]$Bytes
    )

    if ($Bytes -lt 1024) {
        return "$Bytes B"
    }

    $units = @("KB", "MB", "GB", "TB")
    $size = [double]$Bytes
    $unitIndex = -1

    do {
        $size = $size / 1024
        $unitIndex++
    } while ($size -ge 1024 -and $unitIndex -lt ($units.Count - 1))

    return ("{0:0.##} {1}" -f $size, $units[$unitIndex])
}

function Resolve-FullPath {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [string]$Name = "Path"
    )

    if (-not [System.IO.Path]::IsPathRooted($Path)) {
        throw "$Name must be a fully qualified path: $Path"
    }

    return [System.IO.Path]::GetFullPath($Path).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
}

function Test-SamePath {
    param(
        [string]$Left,

        [string]$Right
    )

    if ([string]::IsNullOrWhiteSpace($Left) -or [string]::IsNullOrWhiteSpace($Right)) {
        return $false
    }

    $leftFullPath = [System.IO.Path]::GetFullPath($Left).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    $rightFullPath = [System.IO.Path]::GetFullPath($Right).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    return $leftFullPath.Equals($rightFullPath, [System.StringComparison]::OrdinalIgnoreCase)
}

function Test-PathWithinRoot {
    param(
        [string]$RootPath,

        [string]$CandidatePath
    )

    if ([string]::IsNullOrWhiteSpace($RootPath) -or [string]::IsNullOrWhiteSpace($CandidatePath)) {
        return $false
    }

    $rootFullPath = [System.IO.Path]::GetFullPath($RootPath).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    $candidateFullPath = [System.IO.Path]::GetFullPath($CandidatePath).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    return $candidateFullPath.Equals($rootFullPath, [System.StringComparison]::OrdinalIgnoreCase) -or
        $candidateFullPath.StartsWith($rootFullPath + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)
}

function Get-PropertyValue {
    param(
        [object]$Object,

        [Parameter(Mandatory)]
        [string]$Name,

        $DefaultValue = $null
    )

    if ($null -eq $Object) {
        return $DefaultValue
    }

    $property = $Object.PSObject.Properties[$Name]
    if ($null -eq $property) {
        return $DefaultValue
    }

    return $property.Value
}

function Add-StatusCount {
    param(
        [hashtable]$Counts,

        [string]$Status
    )

    if ([string]::IsNullOrWhiteSpace($Status)) {
        $Status = "Unknown"
    }

    if (-not $Counts.ContainsKey($Status)) {
        $Counts[$Status] = 0
    }

    $Counts[$Status] = [int]$Counts[$Status] + 1
}

function Get-StatusCount {
    param(
        [hashtable]$Counts,

        [string]$Status
    )

    if (-not $Counts.ContainsKey($Status)) {
        return 0
    }

    return [int]$Counts[$Status]
}

function Test-RequiresRecoveryReview {
    param(
        [string]$ActionStatus,

        [hashtable]$EntryStatusCounts
    )

    if ($ActionStatus -in @("PartialFailure", "Failed", "RestorePartialFailure", "RestoreFailed")) {
        return $true
    }

    return (Get-StatusCount -Counts $EntryStatusCounts -Status "Moving") -gt 0 -or
        (Get-StatusCount -Counts $EntryStatusCounts -Status "Failed") -gt 0 -or
        (Get-StatusCount -Counts $EntryStatusCounts -Status "Restoring") -gt 0 -or
        (Get-StatusCount -Counts $EntryStatusCounts -Status "RestoreFailed") -gt 0
}

$quarantineRootFullPath = Resolve-FullPath -Path $QuarantineRoot -Name "QuarantineRoot"
$cleanupScopeFullPath = $null
if (-not [string]::IsNullOrWhiteSpace($CleanupScope)) {
    $cleanupScopeFullPath = Resolve-FullPath -Path $CleanupScope -Name "CleanupScope"
}
$actionsRootPath = Join-Path $quarantineRootFullPath "actions"
$issues = [System.Collections.Generic.List[object]]::new()
$manifests = [System.Collections.Generic.List[object]]::new()
$entryStatusTotals = @{}
$cleanupScopes = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)

Write-Host "Restore Manifest summary"
Write-Host "Quarantine Root: $quarantineRootFullPath"
Write-Host "Actions root: $actionsRootPath"
Write-Host "Boundary: read-only summary only; this does not launch WPF, scan, move, restore, delete, write manifests, approve cleanup, or create cleanup history."
Write-Host "Selected restore boundary: WPF restore remains one selected fixture or exact C:\Users\moxhe Restore Manifest after readiness and exact RESTORE; broad/all-manifest restore remains unavailable."
Write-Host ""

if (-not (Test-Path -LiteralPath $actionsRootPath -PathType Container)) {
    $issues.Add([pscustomobject]@{
            Path = $actionsRootPath
            Message = "No quarantine actions folder exists under the selected Quarantine Root."
        })
}
else {
    $actionFolders = @(Get-ChildItem -LiteralPath $actionsRootPath -Directory)
    foreach ($actionFolder in $actionFolders) {
        $manifestPath = Join-Path $actionFolder.FullName "restore-manifest.json"
        if (-not (Test-Path -LiteralPath $manifestPath -PathType Leaf)) {
            $issues.Add([pscustomobject]@{
                    Path = $manifestPath
                    Message = "Action folder does not contain restore-manifest.json."
                })
            continue
        }

        try {
            $manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
        }
        catch {
            $issues.Add([pscustomobject]@{
                    Path = $manifestPath
                    Message = "Could not parse Restore Manifest: $($_.Exception.Message)"
                })
            continue
        }

        $schemaVersion = [string](Get-PropertyValue -Object $manifest -Name "schemaVersion" -DefaultValue "")
        $manifestQuarantineRoot = [string](Get-PropertyValue -Object $manifest -Name "quarantineRootPath" -DefaultValue "")
        $manifestActionRoot = [string](Get-PropertyValue -Object $manifest -Name "actionRootPath" -DefaultValue "")
        $manifestItemsRoot = [string](Get-PropertyValue -Object $manifest -Name "itemsRootPath" -DefaultValue "")
        $manifestDeclaredPath = [string](Get-PropertyValue -Object $manifest -Name "manifestPath" -DefaultValue "")

        if ($schemaVersion -ne "restore-manifest.v1") {
            $issues.Add([pscustomobject]@{
                    Path = $manifestPath
                    Message = "Unsupported Restore Manifest schema version: $schemaVersion."
                })
            continue
        }

        if (-not (Test-SamePath -Left $manifestQuarantineRoot -Right $quarantineRootFullPath)) {
            $issues.Add([pscustomobject]@{
                    Path = $manifestPath
                    Message = "Restore Manifest quarantine root does not match the selected Quarantine Root."
                })
            continue
        }

        if (-not (Test-SamePath -Left $manifestActionRoot -Right $actionFolder.FullName)) {
            $issues.Add([pscustomobject]@{
                    Path = $manifestPath
                    Message = "Restore Manifest action root does not match its action folder."
                })
            continue
        }

        if (-not (Test-SamePath -Left $manifestDeclaredPath -Right $manifestPath)) {
            $issues.Add([pscustomobject]@{
                    Path = $manifestPath
                    Message = "Restore Manifest path does not match its discovered file path."
                })
            continue
        }

        if (-not (Test-PathWithinRoot -RootPath $manifestActionRoot -CandidatePath $manifestItemsRoot)) {
            $issues.Add([pscustomobject]@{
                    Path = $manifestPath
                    Message = "Restore Manifest items root is outside the action root."
                })
            continue
        }

        $entries = @(Get-PropertyValue -Object $manifest -Name "entries" -DefaultValue @())
        $entryStatusCounts = @{}
        $totalBytes = [long]0

        foreach ($entry in $entries) {
            $status = [string](Get-PropertyValue -Object $entry -Name "status" -DefaultValue "Unknown")
            Add-StatusCount -Counts $entryStatusCounts -Status $status
            Add-StatusCount -Counts $entryStatusTotals -Status $status
            $totalBytes += [long](Get-PropertyValue -Object $entry -Name "sizeBytes" -DefaultValue 0)
        }

        $cleanupScopePath = [string](Get-PropertyValue -Object $manifest -Name "cleanupScopePath" -DefaultValue "")
        if (-not [string]::IsNullOrWhiteSpace($cleanupScopePath)) {
            [void]$cleanupScopes.Add($cleanupScopePath)
        }

        $actionStatus = [string](Get-PropertyValue -Object $manifest -Name "actionStatus" -DefaultValue "Unknown")
        $requiresRecoveryReview = Test-RequiresRecoveryReview -ActionStatus $actionStatus -EntryStatusCounts $entryStatusCounts

        $manifests.Add([pscustomobject]@{
                Manifest = $manifest
                ManifestPath = $manifestPath
                ActionId = [string](Get-PropertyValue -Object $manifest -Name "actionId" -DefaultValue $actionFolder.Name)
                ActionStatus = $actionStatus
                CleanupScopePath = $cleanupScopePath
                ActionRootPath = $manifestActionRoot
                UpdatedAtUtc = [string](Get-PropertyValue -Object $manifest -Name "updatedAtUtc" -DefaultValue "")
                EntryCount = $entries.Count
                TotalBytes = $totalBytes
                EntryStatusCounts = $entryStatusCounts
                RequiresRecoveryReview = $requiresRecoveryReview
                HasUndoWork = (Get-StatusCount -Counts $entryStatusCounts -Status "Moved") -gt 0
                Entries = $entries
            })
    }
}

$orderedManifests = @($manifests | Sort-Object -Property UpdatedAtUtc -Descending)
$displayManifests = @($orderedManifests)
$displayFilterLabels = [System.Collections.Generic.List[string]]::new()
if ($null -ne $cleanupScopeFullPath) {
    $displayManifests = @($displayManifests | Where-Object { Test-SamePath -Left $_.CleanupScopePath -Right $cleanupScopeFullPath })
    [void]$displayFilterLabels.Add("Cleanup Scope: $cleanupScopeFullPath")
}

if ($RecoveryReviewOnly.IsPresent) {
    $displayManifests = @($displayManifests | Where-Object { $_.RequiresRecoveryReview })
    [void]$displayFilterLabels.Add("recovery-review manifests only")
}

if ($UndoWorkOnly.IsPresent) {
    $displayManifests = @($displayManifests | Where-Object { $_.HasUndoWork })
    [void]$displayFilterLabels.Add("undo-work manifests only")
}

$totalEntries = 0
$totalBytes = [long]0
$recoveryReviewCount = 0
$undoWorkCount = 0
foreach ($summary in $orderedManifests) {
    $totalEntries += [int]$summary.EntryCount
    $totalBytes += [long]$summary.TotalBytes
    if ($summary.RequiresRecoveryReview) {
        $recoveryReviewCount++
    }
    if ($summary.HasUndoWork) {
        $undoWorkCount++
    }
}

Write-Host ("Found manifests: {0} | Issues: {1} | Entries: {2} | Size: {3}" -f $orderedManifests.Count, $issues.Count, $totalEntries, (Format-ByteSize -Bytes $totalBytes))
Write-Host ("Entry statuses: planned {0}, moving {1}, moved {2}, failed {3}, restoring {4}, restored {5}, restore failed {6}" -f `
        (Get-StatusCount -Counts $entryStatusTotals -Status "Planned"), `
        (Get-StatusCount -Counts $entryStatusTotals -Status "Moving"), `
        (Get-StatusCount -Counts $entryStatusTotals -Status "Moved"), `
        (Get-StatusCount -Counts $entryStatusTotals -Status "Failed"), `
        (Get-StatusCount -Counts $entryStatusTotals -Status "Restoring"), `
        (Get-StatusCount -Counts $entryStatusTotals -Status "Restored"), `
        (Get-StatusCount -Counts $entryStatusTotals -Status "RestoreFailed"))
Write-Host "Manifests with undo work: $undoWorkCount"
Write-Host "Manifests needing recovery review: $recoveryReviewCount"
if ($displayFilterLabels.Count -gt 0) {
    $displayFilterText = [string]::Join("; ", [string[]]$displayFilterLabels.ToArray())
    Write-Host ("Display filter: {0} ({1} of {2})" -f $displayFilterText, $displayManifests.Count, $orderedManifests.Count)
}

if ($cleanupScopes.Count -gt 0) {
    Write-Host "Cleanup Scopes:"
    foreach ($cleanupScope in @($cleanupScopes | Sort-Object)) {
        Write-Host "  - $cleanupScope"
    }
}
else {
    Write-Host "Cleanup Scopes: none discovered"
}

Write-Host ""

if ($displayManifests.Count -gt 0) {
    Write-Host "Manifests:"
    foreach ($summary in $displayManifests) {
        Write-Host ("- {0} | Status: {1} | Entries: {2} | Size: {3} | Moved {4} | Restored {5} | Failed {6} | Restore failed {7} | Undo work: {8} | Recovery review: {9}" -f `
                $summary.ActionId, `
                $summary.ActionStatus, `
                $summary.EntryCount, `
                (Format-ByteSize -Bytes $summary.TotalBytes), `
                (Get-StatusCount -Counts $summary.EntryStatusCounts -Status "Moved"), `
                (Get-StatusCount -Counts $summary.EntryStatusCounts -Status "Restored"), `
                (Get-StatusCount -Counts $summary.EntryStatusCounts -Status "Failed"), `
                (Get-StatusCount -Counts $summary.EntryStatusCounts -Status "RestoreFailed"), `
                ($(if ($summary.HasUndoWork) { "yes" } else { "no" })), `
                ($(if ($summary.RequiresRecoveryReview) { "yes" } else { "no" })))
        Write-Host "  Cleanup Scope: $($summary.CleanupScopePath)"
        Write-Host "  Manifest: $($summary.ManifestPath)"
        Write-Host "  Updated UTC: $($summary.UpdatedAtUtc)"

        if ($ShowEntries.IsPresent) {
            foreach ($entry in $summary.Entries) {
                $entryStatus = [string](Get-PropertyValue -Object $entry -Name "status" -DefaultValue "Unknown")
                $entryType = if ([bool](Get-PropertyValue -Object $entry -Name "isDirectory" -DefaultValue $false)) { "Directory" } else { "File" }
                $entrySize = [long](Get-PropertyValue -Object $entry -Name "sizeBytes" -DefaultValue 0)
                $relativePath = [string](Get-PropertyValue -Object $entry -Name "relativePath" -DefaultValue "")
                $originalPath = [string](Get-PropertyValue -Object $entry -Name "originalPath" -DefaultValue "")
                $quarantinePath = [string](Get-PropertyValue -Object $entry -Name "quarantinePath" -DefaultValue "")
                $errorMessage = [string](Get-PropertyValue -Object $entry -Name "errorMessage" -DefaultValue "")
                Write-Host ("  Entry | {0} | {1} | {2} | {3}" -f $entryStatus, $entryType, (Format-ByteSize -Bytes $entrySize), $relativePath)
                Write-Host "    Original: $originalPath"
                Write-Host "    Quarantine: $quarantinePath"
                if (-not [string]::IsNullOrWhiteSpace($errorMessage)) {
                    Write-Host "    Error: $errorMessage"
                }
            }
        }
    }
}
elseif ($displayFilterLabels.Count -gt 0) {
    Write-Host "Manifests: none matched the selected display filter."
}

if ($issues.Count -gt 0) {
    Write-Host ""
    Write-Host "Discovery issues:"
    foreach ($issue in $issues) {
        Write-Host "- $($issue.Path) | $($issue.Message)"
    }
}

if ($RequireAny.IsPresent -and $orderedManifests.Count -eq 0) {
    Write-Host ""
    Write-Host "Restore Manifest summary failed because -RequireAny was set and no valid manifests were found."
    exit 1
}

if ($RequireNoRecoveryReview.IsPresent -and $recoveryReviewCount -gt 0) {
    Write-Host ""
    Write-Host "Restore Manifest summary failed because -RequireNoRecoveryReview was set and $recoveryReviewCount manifest(s) need recovery review."
    exit 1
}

if ($RequireNoUndoWork.IsPresent -and $undoWorkCount -gt 0) {
    Write-Host ""
    Write-Host "Restore Manifest summary failed because -RequireNoUndoWork was set and $undoWorkCount manifest(s) still have undo work."
    exit 1
}
