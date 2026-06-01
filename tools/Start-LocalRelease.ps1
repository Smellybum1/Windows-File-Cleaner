[CmdletBinding()]
param(
    [string]$ReleaseRoot = ".local\releases",

    [string]$ReleasePath,

    [switch]$Fixture,

    [switch]$PrintOnly,

    [switch]$SkipVerify,

    [switch]$RequireCurrentCommit,

    [switch]$AllowDirtyPublish,

    [switch]$AllowSkippedPreflight
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$verifierScript = Join-Path $PSScriptRoot "Test-LocalRelease.ps1"
$fixtureScope = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\storage-scan-smoke-fixture")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)

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

function Format-LaunchCommand {
    param(
        [Parameter(Mandatory)]
        [string]$ExecutablePath,

        [string[]]$Arguments = @()
    )

    $parts = @("& `"$ExecutablePath`"")
    foreach ($argument in $Arguments) {
        if ($argument.Contains(" ") -or $argument.Contains("`"")) {
            $parts += "`"$($argument.Replace('"', '\"'))`""
        }
        else {
            $parts += $argument
        }
    }

    return ($parts -join " ")
}

$releaseRootFullPath = Resolve-UnderLocalPath -Path $ReleaseRoot -Description "Release root"
$releaseDir = if ([string]::IsNullOrWhiteSpace($ReleasePath)) {
    Get-LatestReleasePath -RootPath $releaseRootFullPath
}
else {
    Resolve-UnderLocalPath -Path $ReleasePath -Description "Release path"
}

$appExePath = Join-Path (Join-Path $releaseDir "app") "WindowsFileCleaner.App.exe"
if (-not (Test-Path -LiteralPath $appExePath -PathType Leaf)) {
    throw "Published executable is missing: $appExePath"
}

$readmePath = Join-Path $releaseDir "README-FIRST.txt"
$releaseLaunchScriptPath = Join-Path $releaseDir "Launch-WindowsFileCleaner.cmd"
$releaseFixtureLaunchScriptPath = Join-Path $releaseDir "Launch-WindowsFileCleaner-Fixture.cmd"

if (-not $SkipVerify.IsPresent) {
    $verifierArguments = @{
        ReleaseRoot = $ReleaseRoot
        ReleasePath = $releaseDir
    }
    if ($RequireCurrentCommit.IsPresent) {
        $verifierArguments["RequireCurrentCommit"] = $true
    }
    if ($AllowDirtyPublish.IsPresent) {
        $verifierArguments["AllowDirtyPublish"] = $true
    }
    if ($AllowSkippedPreflight.IsPresent) {
        $verifierArguments["AllowSkippedPreflight"] = $true
    }

    & $verifierScript @verifierArguments
    if ($LASTEXITCODE -ne 0) {
        throw "Portable release verification failed before launch."
    }
}

$launchArguments = @()
if ($Fixture.IsPresent) {
    $launchArguments = @("--scope", $fixtureScope)
}

$launchCommand = Format-LaunchCommand -ExecutablePath $appExePath -Arguments $launchArguments

Write-Host ""
Write-Host "Portable release launcher"
Write-Host "Repository: $repoFullPath"
Write-Host "Release: $releaseDir"
Write-Host "Executable: $appExePath"
Write-Host "Start-here README: $readmePath"
if ($Fixture.IsPresent) {
    Write-Host "Fixture Cleanup Scope: $fixtureScope"
    Write-Host "Release-local launch script: $releaseFixtureLaunchScriptPath"
    Write-Host "Boundary: fixture launch prefills the Cleanup Scope only; it does not create the fixture, click Scan, move, restore, delete, or approve cleanup."
}
else {
    Write-Host "Release-local launch script: $releaseLaunchScriptPath"
    Write-Host "Boundary: normal launch starts the packaged app only; it does not click Scan, move, restore, delete, or approve cleanup."
}
Write-Host "Launch command:"
Write-Host $launchCommand

if ($PrintOnly.IsPresent) {
    Write-Host "Print-only mode: WPF was not launched."
    exit 0
}

if ($Fixture.IsPresent) {
    Start-Process -FilePath $appExePath -ArgumentList @("--scope", $fixtureScope)
}
else {
    Start-Process -FilePath $appExePath
}
