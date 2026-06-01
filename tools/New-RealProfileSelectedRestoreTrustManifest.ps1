[CmdletBinding(SupportsShouldProcess)]
param(
    [string]$QuarantineRoot = "D:\WindowsFileCleanerQuarantine",
    [string]$RelativePath = "WindowsFileCleanerRestoreTrustTest\restore-target.txt"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if ([System.IO.Path]::IsPathRooted($RelativePath)) {
    throw "RelativePath must be relative to the current user profile."
}

$cleanupScope = [System.IO.Path]::GetFullPath([Environment]::GetFolderPath("UserProfile")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
if (-not $cleanupScope.EndsWith("\moxhe", [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "This trust helper is only intended for the moxhe real-profile scope. Current profile: $cleanupScope"
}

if (-not [System.IO.Path]::IsPathRooted($QuarantineRoot)) {
    throw "QuarantineRoot must be a fully qualified path."
}

$quarantineRootFullPath = [System.IO.Path]::GetFullPath($QuarantineRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$originalPath = [System.IO.Path]::GetFullPath((Join-Path $cleanupScope $RelativePath))

if (-not ($originalPath.StartsWith($cleanupScope + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase))) {
    throw "The restore target must stay inside the current user profile: $cleanupScope"
}

if (Test-Path -LiteralPath $originalPath) {
    throw "Refusing to create trust manifest because the restore target already exists: $originalPath"
}

$stamp = [DateTimeOffset]::UtcNow.ToString("yyyyMMddHHmmss")
$actionId = "real-profile-selected-restore-trust-$stamp"
$actionRoot = [System.IO.Path]::GetFullPath((Join-Path (Join-Path $quarantineRootFullPath "actions") $actionId))
$itemsRoot = [System.IO.Path]::GetFullPath((Join-Path $actionRoot "items"))
$manifestPath = [System.IO.Path]::GetFullPath((Join-Path $actionRoot "restore-manifest.json"))
$quarantinePath = [System.IO.Path]::GetFullPath((Join-Path $itemsRoot $RelativePath))

if (Test-Path -LiteralPath $actionRoot) {
    throw "Refusing to reuse an existing action root: $actionRoot"
}

$now = [DateTimeOffset]::UtcNow
$nowText = $now.ToString("O")
$content = @"
Windows File Cleaner real-profile selected restore trust test.
Created UTC: $nowText
This is a sacrificial file created under quarantine and restored by the app only if you click Restore selected manifest.
"@

$manifest = [ordered]@{
    schemaVersion = "restore-manifest.v1"
    manifestId = "restore-manifest-$actionId"
    restoreManifestDraftId = "manifest-draft-$actionId"
    actionId = $actionId
    createdAtUtc = $nowText
    updatedAtUtc = $nowText
    cleanupScopePath = $cleanupScope
    quarantineRootPath = $quarantineRootFullPath
    actionRootPath = $actionRoot
    itemsRootPath = $itemsRoot
    manifestPath = $manifestPath
    actionStatus = "Completed"
    entries = @(
        [ordered]@{
            originalPath = $originalPath
            relativePath = $RelativePath
            quarantinePath = $quarantinePath
            isDirectory = $false
            sizeBytes = [System.Text.Encoding]::UTF8.GetByteCount($content)
            lastModifiedUtc = $nowText
            importanceRating = "LikelySafe"
            deletionRecommendation = "QuarantineCandidate"
            bloatCategories = @("AppCache")
            evidence = "Synthetic real-profile selected restore trust test. This manifest was created by a local helper and should restore only the sacrificial trust-test file."
            status = "Moved"
            moveStartedAtUtc = $nowText
            moveCompletedAtUtc = $nowText
            restoreStartedAtUtc = $null
            restoreCompletedAtUtc = $null
            errorMessage = $null
        }
    )
    writeOrderNotes = @(
        "Synthetic selected restore trust manifest.",
        "No real user file was quarantined by this helper.",
        "Restore only after reviewing the selected manifest in Windows File Cleaner."
    )
}

if ($PSCmdlet.ShouldProcess($actionRoot, "Create real-profile selected restore trust manifest")) {
    New-Item -ItemType Directory -Path (Split-Path -Parent $quarantinePath) -Force | Out-Null
    Set-Content -LiteralPath $quarantinePath -Value $content -Encoding UTF8
    (Get-Item -LiteralPath $quarantinePath).LastWriteTimeUtc = $now.UtcDateTime

    New-Item -ItemType Directory -Path $actionRoot -Force | Out-Null
    $json = $manifest | ConvertTo-Json -Depth 8
    Set-Content -LiteralPath $manifestPath -Value $json -Encoding UTF8
}

if ($WhatIfPreference) {
    Write-Output "Selected real-profile restore trust manifest preview:"
}
else {
    Write-Output "Created selected real-profile restore trust manifest:"
}
Write-Output "Cleanup Scope: $cleanupScope"
Write-Output "Quarantine Root: $quarantineRootFullPath"
Write-Output "Restore Manifest: $manifestPath"
Write-Output "Quarantine source: $quarantinePath"
Write-Output "Restore target: $originalPath"
Write-Output ""
Write-Output "Next app command:"
Write-Output "dotnet run --project src\WindowsFileCleaner.App -- --scope `"$cleanupScope`""
Write-Output ""
Write-Output "In the app: open Quarantine, set Quarantine Root to $quarantineRootFullPath, Discover manifests, select this manifest, preview selected readiness, preview selected restore gate, type RESTORE, and click Restore selected manifest only if the gate says Can execute: yes."
