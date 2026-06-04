[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoFullPath = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$notesRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\release-acceptance")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$testRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\release-acceptance-command-stamping-test")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$startLocalReleaseScript = Join-Path $PSScriptRoot "Start-LocalRelease.ps1"
$generatedNotes = [System.Collections.Generic.List[string]]::new()

function Assert-UnderLocalPath {
    param(
        [Parameter(Mandatory)]
        [string]$Path
    )

    $resolved = [System.IO.Path]::GetFullPath($Path).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    if (-not ($resolved.Equals($localRoot, [System.StringComparison]::OrdinalIgnoreCase) -or
            $resolved.StartsWith($localRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase))) {
        throw "Test path must stay under ignored .local: $resolved"
    }
}

function Assert-UnderNotesRoot {
    param(
        [Parameter(Mandatory)]
        [string]$Path
    )

    $resolved = [System.IO.Path]::GetFullPath($Path).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    if (-not ($resolved.Equals($notesRoot, [System.StringComparison]::OrdinalIgnoreCase) -or
            $resolved.StartsWith($notesRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase))) {
        throw "Generated notes path must stay under ignored release acceptance notes: $resolved"
    }
}

function Assert-ContainsText {
    param(
        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string[]]$Lines,

        [Parameter(Mandatory)]
        [string]$ExpectedText
    )

    $joined = ($Lines -join [Environment]::NewLine)
    $compactJoined = [regex]::Replace($joined, "\s+", " ").Trim()
    $compactExpectedText = [regex]::Replace($ExpectedText, "\s+", " ").Trim()
    if ($joined.IndexOf($ExpectedText, [System.StringComparison]::Ordinal) -lt 0 -and
        $compactJoined.IndexOf($compactExpectedText, [System.StringComparison]::Ordinal) -lt 0) {
        throw "Expected text to contain: $ExpectedText"
    }
}

function Assert-DoesNotContainText {
    param(
        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string[]]$Lines,

        [Parameter(Mandatory)]
        [string]$UnexpectedText
    )

    $joined = ($Lines -join [Environment]::NewLine)
    $compactJoined = [regex]::Replace($joined, "\s+", " ").Trim()
    $compactUnexpectedText = [regex]::Replace($UnexpectedText, "\s+", " ").Trim()
    if ($joined.IndexOf($UnexpectedText, [System.StringComparison]::Ordinal) -ge 0 -or
        $compactJoined.IndexOf($compactUnexpectedText, [System.StringComparison]::Ordinal) -ge 0) {
        throw "Expected text not to contain: $UnexpectedText"
    }
}

function Write-TestFile {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string[]]$Lines
    )

    Assert-UnderLocalPath -Path $Path
    $parent = Split-Path -Parent $Path
    New-Item -ItemType Directory -Path $parent -Force | Out-Null
    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllLines($Path, $Lines, $utf8NoBom)
}

function New-TestReleasePackage {
    param(
        [Parameter(Mandatory)]
        [string]$ReleaseDirectory
    )

    Assert-UnderLocalPath -Path $ReleaseDirectory

    $appDirectory = Join-Path $ReleaseDirectory "app"
    New-Item -ItemType Directory -Path $appDirectory -Force | Out-Null

    Write-TestFile -Path (Join-Path $appDirectory "WindowsFileCleaner.App.exe") -Lines @(
        "Synthetic placeholder executable for local release acceptance command stamping regression.",
        "This file is never launched."
    )
    Write-TestFile -Path (Join-Path $ReleaseDirectory "README-FIRST.txt") -Lines @(
        "Synthetic README-FIRST.txt for command stamping regression."
    )
    Write-TestFile -Path (Join-Path $ReleaseDirectory "Launch-WindowsFileCleaner.cmd") -Lines @(
        "@echo off",
        "rem Synthetic normal launch script for command stamping regression."
    )
    Write-TestFile -Path (Join-Path $ReleaseDirectory "Launch-WindowsFileCleaner-Fixture.cmd") -Lines @(
        "@echo off",
        "rem Synthetic fixture launch script for command stamping regression."
    )
}

function Invoke-StartLocalRelease {
    param(
        [Parameter(Mandatory)]
        [string]$ReleaseDirectory,

        [switch]$RequireCurrentCommit
    )

    $arguments = @(
        "-NoProfile",
        "-ExecutionPolicy",
        "Bypass",
        "-File",
        $startLocalReleaseScript,
        "-ReleasePath",
        $ReleaseDirectory,
        "-ChecklistOnly",
        "-WriteAcceptanceNotes",
        "-SkipVerify"
    )
    if ($RequireCurrentCommit.IsPresent) {
        $arguments += "-RequireCurrentCommit"
    }

    $previousErrorActionPreference = $ErrorActionPreference
    $ErrorActionPreference = "Continue"
    try {
        $output = @(powershell.exe @arguments 2>&1 | ForEach-Object { [string]$_ })
    }
    finally {
        $ErrorActionPreference = $previousErrorActionPreference
    }

    return [pscustomobject]@{
        ExitCode = $LASTEXITCODE
        Output = $output
    }
}

function Get-GeneratedNotesPath {
    param(
        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string[]]$Lines
    )

    $prefix = "Portable release acceptance notes template: "
    foreach ($line in $Lines) {
        if ($line.StartsWith($prefix, [System.StringComparison]::Ordinal)) {
            return [System.IO.Path]::GetFullPath($line.Substring($prefix.Length).Trim())
        }
    }

    throw "Start-LocalRelease output did not include a generated acceptance notes path."
}

function Assert-GeneratedNotes {
    param(
        [Parameter(Mandatory)]
        [string]$NotesPath,

        [Parameter(Mandatory)]
        [string]$ReleaseDirectory
    )

    Assert-UnderNotesRoot -Path $NotesPath
    if (-not (Test-Path -LiteralPath $NotesPath -PathType Leaf)) {
        throw "Generated notes file is missing: $NotesPath"
    }

    $lines = @(Get-Content -LiteralPath $NotesPath)
    Assert-ContainsText -Lines $lines -ExpectedText "Release folder: $ReleaseDirectory"
    Assert-ContainsText -Lines $lines -ExpectedText "Notes file is local/ignored under ``.local`` and is not app persistence or cleanup history."
    Assert-ContainsText -Lines $lines -ExpectedText "These commands read this ignored notes file only. They do not launch WPF, scan, move, restore, delete, approve cleanup, or create cleanup history."

    return $lines
}

function Remove-TestGeneratedNotes {
    if (-not (Test-Path -LiteralPath $notesRoot -PathType Container)) {
        return
    }

    foreach ($notesFile in Get-ChildItem -LiteralPath $notesRoot -File -Filter "release-acceptance-*.md") {
        $notesPath = [System.IO.Path]::GetFullPath($notesFile.FullName)
        Assert-UnderNotesRoot -Path $notesPath
        $content = Get-Content -LiteralPath $notesPath -Raw
        if ($content.IndexOf($testRoot, [System.StringComparison]::OrdinalIgnoreCase) -ge 0) {
            Remove-Item -LiteralPath $notesPath -Force
        }
    }
}

Assert-UnderLocalPath -Path $testRoot
Assert-UnderNotesRoot -Path $notesRoot

$behindHeadReleasePath = Join-Path $testRoot "behind-head-release"
$currentCommitReleasePath = Join-Path $testRoot "current-commit-release"

try {
    Remove-TestGeneratedNotes

    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }

    New-TestReleasePackage -ReleaseDirectory $behindHeadReleasePath
    New-TestReleasePackage -ReleaseDirectory $currentCommitReleasePath

    $behindHeadResult = Invoke-StartLocalRelease -ReleaseDirectory $behindHeadReleasePath
    if ($behindHeadResult.ExitCode -ne 0) {
        throw "Behind-current-HEAD acceptance notes stamping should exit 0. Exit code: $($behindHeadResult.ExitCode)"
    }

    $behindHeadNotesPath = Get-GeneratedNotesPath -Lines $behindHeadResult.Output
    $generatedNotes.Add($behindHeadNotesPath)
    $behindHeadLines = Assert-GeneratedNotes -NotesPath $behindHeadNotesPath -ReleaseDirectory $behindHeadReleasePath

    Assert-ContainsText -Lines $behindHeadResult.Output -ExpectedText "Portable release acceptance notes template: $behindHeadNotesPath"
    Assert-ContainsText -Lines $behindHeadResult.Output -ExpectedText ".\tools\Record-LocalReleaseAcceptanceNotes.cmd -Path `"$behindHeadNotesPath`" -RecordManualAcceptance"
    Assert-ContainsText -Lines $behindHeadResult.Output -ExpectedText ".\tools\Record-LocalReleaseAcceptanceNotes.cmd -Path `"$behindHeadNotesPath`" -RecordManualAcceptance -RecordCommitMismatch"
    Assert-ContainsText -Lines $behindHeadResult.Output -ExpectedText "If the package/current-HEAD mismatch is intentional, review the verifier warning, then run:"
    Assert-ContainsText -Lines $behindHeadLines -ExpectedText ".\tools\Test-LocalRelease.cmd -ReleasePath `"$behindHeadReleasePath`""
    Assert-ContainsText -Lines $behindHeadLines -ExpectedText ".\tools\Start-LocalRelease.cmd -ReleasePath `"$behindHeadReleasePath`" -ChecklistOnly"
    Assert-ContainsText -Lines $behindHeadLines -ExpectedText ".\tools\Start-LocalRelease.cmd -ReleasePath `"$behindHeadReleasePath`" -Fixture -ChecklistOnly"
    Assert-ContainsText -Lines $behindHeadLines -ExpectedText '- Package/current-HEAD mismatch note: current-commit evidence was not required when these notes were created; review the verifier warning and use `-RecordCommitMismatch` only if accepting that mismatch.'
    Assert-ContainsText -Lines $behindHeadLines -ExpectedText ".\tools\Record-LocalReleaseAcceptanceNotes.cmd -Path `"$behindHeadNotesPath`" -RecordManualAcceptance -RecordCommitMismatch"
    $behindHeadStampedCommandLines = @($behindHeadLines | Where-Object {
            $_.Contains("Required verifier:") -or
            $_.Contains("Checklist command:") -or
            $_.Contains("Fixture checklist command:")
        })
    Assert-DoesNotContainText -Lines $behindHeadStampedCommandLines -UnexpectedText "-RequireCurrentCommit"

    Start-Sleep -Seconds 1

    $currentCommitResult = Invoke-StartLocalRelease -ReleaseDirectory $currentCommitReleasePath -RequireCurrentCommit
    if ($currentCommitResult.ExitCode -ne 0) {
        throw "Current-commit acceptance notes stamping should exit 0. Exit code: $($currentCommitResult.ExitCode)"
    }

    $currentCommitNotesPath = Get-GeneratedNotesPath -Lines $currentCommitResult.Output
    $generatedNotes.Add($currentCommitNotesPath)
    $currentCommitLines = Assert-GeneratedNotes -NotesPath $currentCommitNotesPath -ReleaseDirectory $currentCommitReleasePath

    Assert-ContainsText -Lines $currentCommitResult.Output -ExpectedText "Portable release acceptance notes template: $currentCommitNotesPath"
    Assert-ContainsText -Lines $currentCommitResult.Output -ExpectedText ".\tools\Record-LocalReleaseAcceptanceNotes.cmd -Path `"$currentCommitNotesPath`" -RecordManualAcceptance"
    Assert-DoesNotContainText -Lines $currentCommitResult.Output -UnexpectedText "-RecordCommitMismatch"
    Assert-DoesNotContainText -Lines $currentCommitResult.Output -UnexpectedText "If the package/current-HEAD mismatch is intentional"
    Assert-ContainsText -Lines $currentCommitLines -ExpectedText ".\tools\Test-LocalRelease.cmd -ReleasePath `"$currentCommitReleasePath`" -RequireCurrentCommit"
    Assert-ContainsText -Lines $currentCommitLines -ExpectedText ".\tools\Start-LocalRelease.cmd -ReleasePath `"$currentCommitReleasePath`" -ChecklistOnly -RequireCurrentCommit"
    Assert-ContainsText -Lines $currentCommitLines -ExpectedText ".\tools\Start-LocalRelease.cmd -ReleasePath `"$currentCommitReleasePath`" -Fixture -ChecklistOnly -RequireCurrentCommit"
    Assert-DoesNotContainText -Lines $currentCommitLines -UnexpectedText "Package/current-HEAD mismatch note:"
    Assert-DoesNotContainText -Lines $currentCommitLines -UnexpectedText "-RecordCommitMismatch"

    Write-Host "Local release acceptance command stamping regression passed."
    Write-Host "Boundary: temporary synthetic release folders and generated acceptance notes were written under ignored .local only; this did not launch WPF, scan, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history."
}
finally {
    foreach ($notesPath in $generatedNotes) {
        if (-not [string]::IsNullOrWhiteSpace($notesPath)) {
            Assert-UnderNotesRoot -Path $notesPath
            if (Test-Path -LiteralPath $notesPath -PathType Leaf) {
                Remove-Item -LiteralPath $notesPath -Force
            }
        }
    }

    Remove-TestGeneratedNotes

    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }
}
