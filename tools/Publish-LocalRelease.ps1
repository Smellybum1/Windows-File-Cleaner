[CmdletBinding()]
param(
    [string]$ReleaseRoot = ".local\releases",
    [string]$Configuration = "Release",
    [string]$Runtime = "win-x64",
    [switch]$SkipPreflight
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$preflightCommand = Join-Path $PSScriptRoot "Invoke-MvpPreflight.cmd"
$projectPath = Join-Path $repoRoot "src\WindowsFileCleaner.App\WindowsFileCleaner.App.csproj"

function Resolve-ReleaseRoot {
    param(
        [Parameter(Mandatory)]
        [string]$Path
    )

    if ([System.IO.Path]::IsPathRooted($Path)) {
        $resolved = [System.IO.Path]::GetFullPath($Path).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    }
    else {
        $resolved = [System.IO.Path]::GetFullPath((Join-Path $repoRoot $Path)).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    }

    if (-not ($resolved.Equals($localRoot, [System.StringComparison]::OrdinalIgnoreCase) -or
        $resolved.StartsWith($localRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase))) {
        throw "Release root must stay under the ignored .local directory: $localRoot"
    }

    return $resolved
}

function Invoke-ReleaseStep {
    param(
        [Parameter(Mandatory)]
        [string]$Name,

        [Parameter(Mandatory)]
        [scriptblock]$Command
    )

    Write-Host ""
    Write-Host "== $Name =="
    $global:LASTEXITCODE = 0
    & $Command
    $exitCode = $global:LASTEXITCODE
    if ($exitCode -ne 0) {
        throw "Release step '$Name' failed with exit code $exitCode."
    }
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
        return ""
    }

    return ""
}

function Format-CommandLine {
    param(
        [Parameter(Mandatory)]
        [string[]]$Parts
    )

    $formatted = foreach ($part in $Parts) {
        if ($part.Contains(" ") -or $part.Contains("`"")) {
            '"' + $part.Replace('"', '\"') + '"'
        }
        else {
            $part
        }
    }

    return ($formatted -join " ")
}

function Get-RunningDebugAppProcesses {
    $debugOutputRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot "src\WindowsFileCleaner.App\bin\Debug"))
    $processes = @(Get-Process -Name "WindowsFileCleaner.App" -ErrorAction SilentlyContinue | Where-Object {
        -not [string]::IsNullOrWhiteSpace($_.Path) -and
            $_.Path.StartsWith($debugOutputRoot, [System.StringComparison]::OrdinalIgnoreCase)
    })

    return $processes
}

$releaseRootFullPath = Resolve-ReleaseRoot -Path $ReleaseRoot
$stamp = Get-Date -Format "yyyyMMdd-HHmmss"
$releaseName = "windows-file-cleaner-v$stamp"
$releaseDir = Join-Path $releaseRootFullPath $releaseName
$appDir = Join-Path $releaseDir "app"
$zipPath = Join-Path $releaseRootFullPath "$releaseName.zip"
$metadataPath = Join-Path $releaseDir "release-metadata.txt"
$appExePath = Join-Path $appDir "WindowsFileCleaner.App.exe"
$publishArguments = @(
    "publish",
    $projectPath,
    "-c",
    $Configuration,
    "-r",
    $Runtime,
    "--self-contained",
    "true",
    "-o",
    $appDir,
    "/p:PublishSingleFile=false"
)

Push-Location $repoRoot
try {
    Write-Host "Portable v1 release publisher"
    Write-Host "Repository: $repoFullPath"
    Write-Host ""
    Write-Host "Current git status:"
    $statusOutput = & git -c "safe.directory=$repoRoot" status --short --branch
    $statusOutput | ForEach-Object { Write-Host $_ }
    if ($LASTEXITCODE -ne 0) {
        throw "Unable to read git status."
    }

    $porcelainStatus = Get-GitOutput -Arguments @("-c", "safe.directory=$repoRoot", "status", "--porcelain")
    $worktreeStatus = if ([string]::IsNullOrWhiteSpace($porcelainStatus)) { "clean" } else { "dirty or intentional local changes" }
    if ($worktreeStatus -ne "clean") {
        Write-Warning "Publishing with local changes. Metadata will record the worktree as dirty or intentional."
    }

    if (-not $SkipPreflight) {
        $runningDebugApps = @(Get-RunningDebugAppProcesses)
        if ($runningDebugApps.Count -gt 0) {
            Write-Host "Running Debug app processes are locking build output:"
            foreach ($process in $runningDebugApps) {
                Write-Host ("  {0} {1}" -f $process.Id, $process.Path)
            }

            throw "Close the running Debug WindowsFileCleaner.App process before publishing, then rerun tools\Publish-LocalRelease.cmd."
        }

        Invoke-ReleaseStep -Name "MVP preflight" -Command {
            & cmd.exe /c $preflightCommand
        }
    }
    else {
        Write-Warning "Skipping MVP preflight by request. Use only for focused local packaging checks."
    }

    if (Test-Path -LiteralPath $releaseDir) {
        throw "Release directory already exists: $releaseDir"
    }

    New-Item -ItemType Directory -Path $releaseDir -Force | Out-Null

    Invoke-ReleaseStep -Name "Publish self-contained WPF app" -Command {
        & dotnet @publishArguments
    }

    if (-not (Test-Path -LiteralPath $appExePath -PathType Leaf)) {
        throw "Published executable was not found: $appExePath"
    }

    $branch = Get-GitOutput -Arguments @("-c", "safe.directory=$repoRoot", "rev-parse", "--abbrev-ref", "HEAD")
    $commit = Get-GitOutput -Arguments @("-c", "safe.directory=$repoRoot", "rev-parse", "HEAD")
    $sdkVersion = (& dotnet --version | Select-Object -First 1)
    $publishCommandLine = Format-CommandLine -Parts (@("dotnet") + $publishArguments)

    $metadata = @(
        "Windows File Cleaner portable v1 release",
        "Created local time: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss zzz")",
        "Repository: $repoFullPath",
        "Branch: $branch",
        "Commit: $commit",
        "Worktree status at publish: $worktreeStatus",
        ".NET SDK: $sdkVersion",
        "Project: $projectPath",
        "Configuration: $Configuration",
        "Runtime: $Runtime",
        "Self-contained: true",
        "Preflight command: $preflightCommand",
        "Preflight skipped: $($SkipPreflight.IsPresent)",
        "Publish command: $publishCommandLine",
        "App directory: $appDir",
        "Executable: $appExePath",
        "Zip path: $zipPath",
        "",
        "Safety boundary:",
        "- Portable v1 is reversible-only: Storage Scan, review, gated Quarantine, and selected restore.",
        "- It is not an installer and does not create a desktop shortcut.",
        "- It does not enable permanent deletion, persisted cleanup history, broad/all-manifest restore, custom real-profile Quarantine, or non-exact real-profile movement.",
        "- Storage Scan remains read-only; real-profile movement still requires the existing readiness gates and explicit user action."
    )

    Set-Content -LiteralPath $metadataPath -Value $metadata -Encoding UTF8

    Invoke-ReleaseStep -Name "Create release zip" -Command {
        Compress-Archive -Path (Join-Path $releaseDir "*") -DestinationPath $zipPath -Force
    }

    if (-not (Test-Path -LiteralPath $zipPath -PathType Leaf)) {
        throw "Release zip was not found: $zipPath"
    }

    Write-Host ""
    Write-Host "Portable v1 release package created."
    Write-Host "App folder: $appDir"
    Write-Host "Executable: $appExePath"
    Write-Host "Zip: $zipPath"
    Write-Host "Metadata: $metadataPath"
    Write-Host ""
    Write-Host "Launch command:"
    Write-Host "& `"$appExePath`""
    Write-Host ""
    Write-Host "Fixture launch command:"
    $fixtureRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\storage-scan-smoke-fixture")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    Write-Host "& `"$appExePath`" --scope `"$fixtureRoot`""
    Write-Host ""
    Write-Host "Release artifacts are under ignored .local; only the publisher scripts and docs should be committed."
}
finally {
    Pop-Location
}
