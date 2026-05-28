[CmdletBinding(SupportsShouldProcess)]
param(
    [string]$Root = ".local\storage-scan-smoke-fixture"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)

if ([System.IO.Path]::IsPathRooted($Root)) {
    $fixtureRoot = [System.IO.Path]::GetFullPath($Root).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
}
else {
    $fixtureRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot $Root)).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
}

if (-not ($fixtureRoot.Equals($repoFullPath, [System.StringComparison]::OrdinalIgnoreCase) -or
    $fixtureRoot.StartsWith($repoFullPath + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase))) {
    throw "Fixture root must stay inside the repository: $repoFullPath"
}

function New-SmokeFixtureFile {
    param(
        [Parameter(Mandatory)]
        [string]$RelativePath,

        [Parameter(Mandatory)]
        [string]$Content,

        [Parameter(Mandatory)]
        [datetime]$LastWriteTimeUtc
    )

    $fullPath = Join-Path $fixtureRoot $RelativePath
    $directory = Split-Path -Parent $fullPath

    if ($PSCmdlet.ShouldProcess($fullPath, "Create synthetic Storage Scan fixture file")) {
        New-Item -ItemType Directory -Path $directory -Force | Out-Null
        Set-Content -LiteralPath $fullPath -Value $Content -Encoding UTF8
        (Get-Item -LiteralPath $fullPath).LastWriteTimeUtc = $LastWriteTimeUtc
    }
}

$now = [datetime]::UtcNow

New-SmokeFixtureFile -RelativePath "Downloads\old-installer.msi" -Content "Synthetic old installer for WPF smoke testing." -LastWriteTimeUtc $now.AddDays(-120)
New-SmokeFixtureFile -RelativePath "AppData\Local\Temp\scratch.tmp" -Content "Synthetic temp file." -LastWriteTimeUtc $now.AddDays(-5)
New-SmokeFixtureFile -RelativePath "AppData\Local\NVIDIA\DXCache\shader.bin" -Content "Synthetic GPU shader cache." -LastWriteTimeUtc $now.AddDays(-5)
New-SmokeFixtureFile -RelativePath "AppData\Local\pip\Cache\http-v2\response.body" -Content "Synthetic Python package cache." -LastWriteTimeUtc $now.AddDays(-40)
New-SmokeFixtureFile -RelativePath "AppData\Local\Google\Chrome\User Data\Default\Preferences" -Content "Synthetic browser profile settings." -LastWriteTimeUtc $now.AddDays(-1)
New-SmokeFixtureFile -RelativePath "Documents\important.txt" -Content "Synthetic protected document." -LastWriteTimeUtc $now
New-SmokeFixtureFile -RelativePath ".codex\config.json" -Content "{ `"synthetic`": true }" -LastWriteTimeUtc $now
New-SmokeFixtureFile -RelativePath "Unknown\notes.txt" -Content "Synthetic uncategorized note." -LastWriteTimeUtc $now

Write-Output "Fixture Cleanup Scope: $fixtureRoot"
Write-Output "Run WPF smoke test:"
Write-Output "dotnet run --project src\WindowsFileCleaner.App -- --scope `"$fixtureRoot`""
