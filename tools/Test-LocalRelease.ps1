[CmdletBinding()]
param(
    [string]$ReleaseRoot = ".local\releases",
    [string]$ReleasePath,
    [switch]$AllowDirtyPublish,
    [switch]$AllowSkippedPreflight,
    [switch]$RequireCurrentCommit
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)

function Resolve-UnderLocalPath {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [string]$Description
    )

    if ([System.IO.Path]::IsPathRooted($Path)) {
        $resolved = [System.IO.Path]::GetFullPath($Path).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    }
    else {
        $resolved = [System.IO.Path]::GetFullPath((Join-Path $repoRoot $Path)).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    }

    if (-not ($resolved.Equals($localRoot, [System.StringComparison]::OrdinalIgnoreCase) -or
        $resolved.StartsWith($localRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase))) {
        throw "$Description must stay under the ignored .local directory: $localRoot"
    }

    return $resolved
}

function Get-LatestReleasePath {
    param(
        [Parameter(Mandatory)]
        [string]$RootPath
    )

    if (-not (Test-Path -LiteralPath $RootPath -PathType Container)) {
        throw "Release root does not exist: $RootPath"
    }

    $latest = Get-ChildItem -LiteralPath $RootPath -Directory -Filter "windows-file-cleaner-v*" |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1

    if ($null -eq $latest) {
        throw "No local release folders found in: $RootPath"
    }

    return $latest.FullName
}

function Get-MetadataValue {
    param(
        [string[]]$Lines,

        [Parameter(Mandatory)]
        [string]$Prefix
    )

    foreach ($line in $Lines) {
        if ($line.StartsWith($Prefix, [System.StringComparison]::OrdinalIgnoreCase)) {
            return $line.Substring($Prefix.Length).Trim()
        }
    }

    return ""
}

function Add-CheckResult {
    param(
        [System.Collections.Generic.List[string]]$Failures,

        [System.Collections.Generic.List[string]]$Warnings,

        [Parameter(Mandatory)]
        [bool]$Passed,

        [Parameter(Mandatory)]
        [string]$PassedMessage,

        [Parameter(Mandatory)]
        [string]$FailureMessage,

        [switch]$WarningOnly
    )

    if ($Passed) {
        Write-Host "[ok] $PassedMessage"
        return
    }

    if ($WarningOnly) {
        Write-Host "[warn] $FailureMessage"
        $Warnings.Add($FailureMessage)
        return
    }

    Write-Host "[fail] $FailureMessage"
    $Failures.Add($FailureMessage)
}

function Get-GitOutput {
    param(
        [Parameter(Mandatory)]
        [string[]]$Arguments
    )

    try {
        $output = & git @Arguments 2>$null
        if ($LASTEXITCODE -eq 0) {
            return (($output | Out-String).Trim())
        }
    }
    catch {
    }

    return ""
}

$releaseRootFullPath = Resolve-UnderLocalPath -Path $ReleaseRoot -Description "Release root"
$releaseDir = if ([string]::IsNullOrWhiteSpace($ReleasePath)) {
    Get-LatestReleasePath -RootPath $releaseRootFullPath
}
else {
    Resolve-UnderLocalPath -Path $ReleasePath -Description "Release path"
}

$releaseName = Split-Path -Leaf $releaseDir
$appDir = Join-Path $releaseDir "app"
$appExePath = Join-Path $appDir "WindowsFileCleaner.App.exe"
$metadataPath = Join-Path $releaseDir "release-metadata.txt"
$zipPath = Join-Path (Split-Path -Parent $releaseDir) "$releaseName.zip"
$failures = [System.Collections.Generic.List[string]]::new()
$warnings = [System.Collections.Generic.List[string]]::new()

Write-Host "Portable v1 release verifier"
Write-Host "Repository: $repoFullPath"
Write-Host "Release: $releaseDir"
Write-Host "This verifier reads local ignored release files only; it does not launch WPF, scan, move, restore, delete, or create cleanup history."
Write-Host ""

Add-CheckResult -Failures $failures -Warnings $warnings -Passed (Test-Path -LiteralPath $releaseDir -PathType Container) -PassedMessage "Release folder exists." -FailureMessage "Release folder is missing: $releaseDir"
Add-CheckResult -Failures $failures -Warnings $warnings -Passed ($releaseName -match "^windows-file-cleaner-v\d{8}-\d{6}$") -PassedMessage "Release folder name matches portable v1 stamp format." -FailureMessage "Release folder name does not match windows-file-cleaner-vYYYYMMDD-HHMMSS: $releaseName"
Add-CheckResult -Failures $failures -Warnings $warnings -Passed (Test-Path -LiteralPath $appExePath -PathType Leaf) -PassedMessage "Published executable exists." -FailureMessage "Published executable is missing: $appExePath"
Add-CheckResult -Failures $failures -Warnings $warnings -Passed (Test-Path -LiteralPath $metadataPath -PathType Leaf) -PassedMessage "Release metadata exists." -FailureMessage "Release metadata is missing: $metadataPath"
Add-CheckResult -Failures $failures -Warnings $warnings -Passed (Test-Path -LiteralPath $zipPath -PathType Leaf) -PassedMessage "Release zip exists beside the folder." -FailureMessage "Release zip is missing: $zipPath"

if ($failures.Count -eq 0) {
    $metadataLines = @(Get-Content -LiteralPath $metadataPath)
    $metadataCommit = Get-MetadataValue -Lines $metadataLines -Prefix "Commit:"
    $metadataBranch = Get-MetadataValue -Lines $metadataLines -Prefix "Branch:"
    $worktreeStatus = Get-MetadataValue -Lines $metadataLines -Prefix "Worktree status at publish:"
    $runtime = Get-MetadataValue -Lines $metadataLines -Prefix "Runtime:"
    $selfContained = Get-MetadataValue -Lines $metadataLines -Prefix "Self-contained:"
    $preflightSkipped = Get-MetadataValue -Lines $metadataLines -Prefix "Preflight skipped:"
    $metadataExecutable = Get-MetadataValue -Lines $metadataLines -Prefix "Executable:"
    $metadataZip = Get-MetadataValue -Lines $metadataLines -Prefix "Zip path:"

    Add-CheckResult -Failures $failures -Warnings $warnings -Passed ($metadataBranch -eq "main") -PassedMessage "Metadata branch is main." -FailureMessage "Metadata branch is not main: $metadataBranch"
    Add-CheckResult -Failures $failures -Warnings $warnings -Passed ($worktreeStatus -eq "clean" -or $AllowDirtyPublish.IsPresent) -PassedMessage "Metadata says publish worktree was clean." -FailureMessage "Metadata says publish worktree was not clean: $worktreeStatus"
    Add-CheckResult -Failures $failures -Warnings $warnings -Passed ($runtime -eq "win-x64") -PassedMessage "Metadata runtime is win-x64." -FailureMessage "Metadata runtime is not win-x64: $runtime"
    Add-CheckResult -Failures $failures -Warnings $warnings -Passed ($selfContained -eq "true") -PassedMessage "Metadata says the package is self-contained." -FailureMessage "Metadata self-contained value is not true: $selfContained"
    Add-CheckResult -Failures $failures -Warnings $warnings -Passed ($preflightSkipped -eq "False" -or $AllowSkippedPreflight.IsPresent) -PassedMessage "Metadata says preflight was not skipped." -FailureMessage "Metadata says preflight was skipped: $preflightSkipped"
    Add-CheckResult -Failures $failures -Warnings $warnings -Passed ($metadataExecutable -eq $appExePath) -PassedMessage "Metadata executable path matches this release." -FailureMessage "Metadata executable path does not match this release: $metadataExecutable"
    Add-CheckResult -Failures $failures -Warnings $warnings -Passed ($metadataZip -eq $zipPath) -PassedMessage "Metadata zip path matches this release." -FailureMessage "Metadata zip path does not match this release: $metadataZip"

    $expectedSafetyLines = @(
        "- Portable v1 is reversible-only: Storage Scan, review, gated Quarantine, and selected restore.",
        "- It is not an installer and does not create a desktop shortcut.",
        "- It does not enable permanent deletion, persisted cleanup history, broad/all-manifest restore, custom real-profile Quarantine, or non-exact real-profile movement.",
        "- Storage Scan remains read-only; real-profile movement still requires the existing readiness gates and explicit user action."
    )

    foreach ($line in $expectedSafetyLines) {
        Add-CheckResult -Failures $failures -Warnings $warnings -Passed ($metadataLines -contains $line) -PassedMessage "Safety boundary line present: $line" -FailureMessage "Safety boundary line missing: $line"
    }

    $currentCommit = Get-GitOutput -Arguments @("-C", $repoRoot, "rev-parse", "HEAD")
    $commitMatches = -not [string]::IsNullOrWhiteSpace($metadataCommit) -and $metadataCommit -eq $currentCommit
    Add-CheckResult -Failures $failures -Warnings $warnings -Passed $commitMatches -PassedMessage "Metadata commit matches current HEAD." -FailureMessage "Metadata commit differs from current HEAD. Package: $metadataCommit Current: $currentCommit" -WarningOnly:(-not $RequireCurrentCommit.IsPresent)

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $zip = [System.IO.Compression.ZipFile]::OpenRead($zipPath)
    try {
        $zipEntryNames = @($zip.Entries | ForEach-Object { $_.FullName.Replace("/", "\") })
        Add-CheckResult -Failures $failures -Warnings $warnings -Passed ($zipEntryNames -contains "app\WindowsFileCleaner.App.exe") -PassedMessage "Zip contains app\WindowsFileCleaner.App.exe." -FailureMessage "Zip does not contain app\WindowsFileCleaner.App.exe."
        Add-CheckResult -Failures $failures -Warnings $warnings -Passed ($zipEntryNames -contains "release-metadata.txt") -PassedMessage "Zip contains release-metadata.txt." -FailureMessage "Zip does not contain release-metadata.txt."
    }
    finally {
        $zip.Dispose()
    }
}

Write-Host ""
if ($failures.Count -gt 0) {
    Write-Host "Portable release verification failed."
    foreach ($failure in $failures) {
        Write-Host "  - $failure"
    }

    exit 1
}

Write-Host "Portable release verification passed."
if ($warnings.Count -gt 0) {
    Write-Host "Warnings:"
    foreach ($warning in $warnings) {
        Write-Host "  - $warning"
    }
}

Write-Host ""
Write-Host "Executable: $appExePath"
Write-Host "Zip: $zipPath"
Write-Host "Metadata: $metadataPath"
