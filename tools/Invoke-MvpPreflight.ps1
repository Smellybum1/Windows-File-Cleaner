[CmdletBinding()]
param(
    [switch]$SkipRestore,
    [switch]$SkipFixtureWhatIf
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$fixtureScript = Join-Path $PSScriptRoot "New-StorageScanSmokeFixture.ps1"
$fixtureRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\storage-scan-smoke-fixture")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)

function Invoke-PreflightStep {
    param(
        [Parameter(Mandatory)]
        [string]$Name,

        [Parameter(Mandatory)]
        [scriptblock]$Command
    )

    Write-Host ""
    Write-Host "== $Name =="
    & $Command
}

Push-Location $repoRoot
try {
    if (-not $SkipRestore) {
        Invoke-PreflightStep -Name "Restore" -Command {
            & dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config
        }
    }

    Invoke-PreflightStep -Name "Build" -Command {
        & dotnet build WindowsFileCleaner.sln --no-restore
    }

    Invoke-PreflightStep -Name "Core tests" -Command {
        & dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build
    }

    Invoke-PreflightStep -Name "WPF app tests" -Command {
        & dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build
    }

    if (-not $SkipFixtureWhatIf) {
        Invoke-PreflightStep -Name "Fixture dry run" -Command {
            & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $fixtureScript -WhatIf
        }
    }

    Write-Host ""
    Write-Host "MVP preflight passed. No real user files were scanned or modified."
    Write-Host "Next manual fixture step:"
    Write-Host ".\tools\New-StorageScanSmokeFixture.ps1"
    Write-Host "dotnet run --project src\WindowsFileCleaner.App -- --scope `"$fixtureRoot`""
}
finally {
    Pop-Location
}
