[CmdletBinding()]
param(
    [string]$ReleaseRoot = ".local\releases",

    [string]$ReleasePath,

    [switch]$Fixture,

    [switch]$ChecklistOnly,

    [switch]$WriteAcceptanceNotes,

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

function Get-PortableReleaseChecklistItems {
    param(
        [Parameter(Mandatory)]
        [string]$ReadmeFile,

        [Parameter(Mandatory)]
        [string]$NormalLaunchScript,

        [Parameter(Mandatory)]
        [string]$NormalLaunchCommand,

        [Parameter(Mandatory)]
        [string]$FixtureLaunchScript,

        [Parameter(Mandatory)]
        [string]$FixtureLaunchCommand,

        [Parameter(Mandatory)]
        [string]$ExecutablePath,

        [Parameter(Mandatory)]
        [string]$FixtureScopePath,

        [Parameter(Mandatory)]
        [bool]$VerificationSkipped
    )

    $verificationText = if ($VerificationSkipped) {
        "Verification was skipped by request; run tools\Test-LocalRelease.cmd -RequireCurrentCommit before trusting this package."
    }
    else {
        "Confirm the verifier output above passed for this release folder."
    }

    @(
        $verificationText,
        "Open README-FIRST.txt and confirm it names portable v1, launch choices, fixture-launch boundaries, and reversible-only safety boundaries. README-FIRST.txt: $ReadmeFile",
        "Normal launch path: use Launch-WindowsFileCleaner.cmd or the printed executable command; confirm the app opens without clicking Scan by itself. Launch script: $NormalLaunchScript. Launch command: $NormalLaunchCommand. Executable: $ExecutablePath",
        "Fixture launch path: use Launch-WindowsFileCleaner-Fixture.cmd or the printed fixture command; confirm the Cleanup Scope is prefilled with the fixture path, then click Scan manually and confirm the scan is read-only. Fixture launch script: $FixtureLaunchScript. Fixture launch command: $FixtureLaunchCommand. Fixture Cleanup Scope: $FixtureScopePath",
        "Confirm the package remains portable: no installer, shortcut, service, scheduled task, permanent deletion, broad/all-manifest restore, or cleanup history.",
        "Stop before real-profile movement unless a specific user-approved readiness gate and exact confirmation are in place."
    )
}

function Write-PortableReleaseChecklist {
    param(
        [Parameter(Mandatory)]
        [string]$ReleaseDirectory,

        [Parameter(Mandatory)]
        [string]$ReadmeFile,

        [Parameter(Mandatory)]
        [string]$NormalLaunchScript,

        [Parameter(Mandatory)]
        [string]$NormalLaunchCommand,

        [Parameter(Mandatory)]
        [string]$FixtureLaunchScript,

        [Parameter(Mandatory)]
        [string]$FixtureLaunchCommand,

        [Parameter(Mandatory)]
        [string]$ExecutablePath,

        [Parameter(Mandatory)]
        [string]$FixtureScopePath,

        [Parameter(Mandatory)]
        [bool]$VerificationSkipped
    )

    Write-Host ""
    Write-Host "Portable release acceptance checklist:"
    $checklistItems = Get-PortableReleaseChecklistItems `
        -ReadmeFile $ReadmeFile `
        -NormalLaunchScript $NormalLaunchScript `
        -NormalLaunchCommand $NormalLaunchCommand `
        -FixtureLaunchScript $FixtureLaunchScript `
        -FixtureLaunchCommand $FixtureLaunchCommand `
        -ExecutablePath $ExecutablePath `
        -FixtureScopePath $FixtureScopePath `
        -VerificationSkipped $VerificationSkipped

    Write-Host ("  1. {0}" -f $checklistItems[0])
    Write-Host "  2. Open README-FIRST.txt and confirm it names portable v1, launch choices, fixture-launch boundaries, and reversible-only safety boundaries."
    Write-Host "     README-FIRST.txt: $ReadmeFile"
    Write-Host "  3. Normal launch path: use Launch-WindowsFileCleaner.cmd or the printed executable command; confirm the app opens without clicking Scan by itself."
    Write-Host "     Launch script: $NormalLaunchScript"
    Write-Host "     Launch command: $NormalLaunchCommand"
    Write-Host "     Executable: $ExecutablePath"
    Write-Host "  4. Fixture launch path: use Launch-WindowsFileCleaner-Fixture.cmd or the printed fixture command; confirm the Cleanup Scope is prefilled with the fixture path, then click Scan manually and confirm the scan is read-only."
    Write-Host "     Fixture launch script: $FixtureLaunchScript"
    Write-Host "     Fixture launch command: $FixtureLaunchCommand"
    Write-Host "     Fixture Cleanup Scope: $FixtureScopePath"
    Write-Host "  5. Confirm the package remains portable: no installer, shortcut, service, scheduled task, permanent deletion, broad/all-manifest restore, or cleanup history."
    Write-Host "  6. Stop before real-profile movement unless a specific user-approved readiness gate and exact confirmation are in place."
    Write-Host "Checklist-only mode did not launch WPF, click Scan, move, restore, delete, approve cleanup, or create cleanup history."
    Write-Host "Release folder: $ReleaseDirectory"
}

function Get-LocalReleaseGitValue {
    param(
        [Parameter(Mandatory)]
        [string[]]$Arguments
    )

    try {
        $output = & git @Arguments 2>$null
        if ($LASTEXITCODE -eq 0) {
            $value = $output | Select-Object -First 1
            if (-not [string]::IsNullOrWhiteSpace([string]$value)) {
                return ([string]$value).Trim()
            }
        }
    }
    catch {
    }

    return "unknown"
}

function Get-LocalReleaseWorktreeStatus {
    param(
        [Parameter(Mandatory)]
        [string]$RepositoryPath
    )

    try {
        $output = & git -C $RepositoryPath status --short 2>$null
        if ($LASTEXITCODE -ne 0) {
            return "unknown"
        }

        $statusLines = @($output | Where-Object { -not [string]::IsNullOrWhiteSpace([string]$_) })
        if ($statusLines.Count -eq 0) {
            return "clean"
        }

        $suffix = if ($statusLines.Count -eq 1) { "" } else { "s" }
        return ("not clean ({0} status line{1})" -f $statusLines.Count, $suffix)
    }
    catch {
    }

    return "unknown"
}

function Get-LocalReleaseMetadataValue {
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

    return "unknown"
}

function Write-PortableReleaseAcceptanceNotesNextSteps {
    param(
        [Parameter(Mandatory)]
        [string]$NotesPath,

        [Parameter(Mandatory)]
        [bool]$RequireCurrentCommitEvidence
    )

    Write-Host "After the package acceptance pass, fill the notes file, then run:"
    Write-Host ".\tools\Record-LocalReleaseAcceptanceNotes.cmd -Path `"$NotesPath`" -RecordManualAcceptance"
    if (-not $RequireCurrentCommitEvidence) {
        Write-Host "If the package/current-HEAD mismatch is intentional, rerun the recorder with -RecordCommitMismatch."
    }
    Write-Host ".\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path `"$NotesPath`""
    Write-Host ".\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path `"$NotesPath`" -RequireComplete"
    Write-Host "These commands update/read ignored notes only; they do not launch WPF, scan, move, restore, delete, approve cleanup, or create cleanup history."
}

function New-PortableReleaseAcceptanceNotes {
    param(
        [Parameter(Mandatory)]
        [string]$ReleaseDirectory,

        [Parameter(Mandatory)]
        [string]$ReadmeFile,

        [Parameter(Mandatory)]
        [string]$NormalLaunchScript,

        [Parameter(Mandatory)]
        [string]$NormalLaunchCommand,

        [Parameter(Mandatory)]
        [string]$FixtureLaunchScript,

        [Parameter(Mandatory)]
        [string]$FixtureLaunchCommand,

        [Parameter(Mandatory)]
        [string]$ExecutablePath,

        [Parameter(Mandatory)]
        [string]$FixtureScopePath,

        [Parameter(Mandatory)]
        [bool]$VerificationSkipped,

        [Parameter(Mandatory)]
        [bool]$RequireCurrentCommitEvidence
    )

    $notesRoot = Join-Path $repoRoot ".local\release-acceptance"
    $timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
    $notesPath = Join-Path $notesRoot ("release-acceptance-{0}.md" -f $timestamp)
    $metadataPath = Join-Path $ReleaseDirectory "release-metadata.txt"
    $metadataLines = if (Test-Path -LiteralPath $metadataPath -PathType Leaf) {
        @(Get-Content -LiteralPath $metadataPath)
    }
    else {
        @()
    }
    $checklistItems = Get-PortableReleaseChecklistItems `
        -ReadmeFile $ReadmeFile `
        -NormalLaunchScript $NormalLaunchScript `
        -NormalLaunchCommand $NormalLaunchCommand `
        -FixtureLaunchScript $FixtureLaunchScript `
        -FixtureLaunchCommand $FixtureLaunchCommand `
        -ExecutablePath $ExecutablePath `
        -FixtureScopePath $FixtureScopePath `
        -VerificationSkipped $VerificationSkipped
    $gitBranch = Get-LocalReleaseGitValue -Arguments @("-C", $repoFullPath, "rev-parse", "--abbrev-ref", "HEAD")
    $gitCommit = Get-LocalReleaseGitValue -Arguments @("-C", $repoFullPath, "rev-parse", "--short", "HEAD")
    $worktreeStatus = Get-LocalReleaseWorktreeStatus -RepositoryPath $repoFullPath
    $verifierCheckbox = if ($VerificationSkipped) { " " } else { "x" }
    $commitCheckbox = if ((-not $VerificationSkipped) -and $RequireCurrentCommitEvidence) { "x" } else { " " }
    $verifierItemStatus = if ($VerificationSkipped) { " " } else { "x" }
    $verifierItemNotes = if ($VerificationSkipped) {
        "Verification was skipped when this notes template was created."
    }
    else {
        "Recorded automatically because Start-LocalRelease completed Test-LocalRelease successfully before writing these notes."
    }

    New-Item -ItemType Directory -Path $notesRoot -Force | Out-Null

    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.Add("# Portable Release Acceptance Notes")
    $lines.Add("")
    $lines.Add("Created: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')")
    $lines.Add("Release folder: $ReleaseDirectory")
    $lines.Add("")
    $lines.Add("Acceptance evidence:")
    $lines.Add("")
    $lines.Add("- Repository: $repoFullPath")
    $lines.Add("- Git branch: $gitBranch")
    $lines.Add("- Git commit: $gitCommit")
    $lines.Add("- Worktree status at notes creation: $worktreeStatus")
    $lines.Add("- Release metadata commit: $(Get-LocalReleaseMetadataValue -Lines $metadataLines -Prefix 'Commit:')")
    $lines.Add("- Release metadata worktree status at publish: $(Get-LocalReleaseMetadataValue -Lines $metadataLines -Prefix 'Worktree status at publish:')")
    $lines.Add("- Release metadata preflight skipped: $(Get-LocalReleaseMetadataValue -Lines $metadataLines -Prefix 'Preflight skipped:')")
    $lines.Add("- Executable: $ExecutablePath")
    $lines.Add("- README-FIRST.txt: $ReadmeFile")
    $lines.Add("- Normal launch script: $NormalLaunchScript")
    $lines.Add("- Normal launch command: $NormalLaunchCommand")
    $lines.Add("- Fixture launch script: $FixtureLaunchScript")
    $lines.Add("- Fixture launch command: $FixtureLaunchCommand")
    $lines.Add("- Fixture Cleanup Scope: $FixtureScopePath")
    $lines.Add('- Required verifier: `.\tools\Test-LocalRelease.cmd -RequireCurrentCommit`')
    $lines.Add('- Checklist command: `.\tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit`')
    $lines.Add('- Fixture checklist command: `.\tools\Start-LocalRelease.cmd -Fixture -ChecklistOnly -RequireCurrentCommit`')
    $lines.Add("- [$verifierCheckbox] Verifier passed for this release package.")
    $lines.Add("- [$commitCheckbox] Package commit matched current HEAD or mismatch was intentionally recorded.")
    $lines.Add("- [ ] Package was launched normally or normal launch was intentionally deferred.")
    $lines.Add("- [ ] Fixture launch and read-only fixture Scan were completed or intentionally deferred.")
    $lines.Add('- Notes file is local/ignored under `.local` and is not app persistence or cleanup history.')
    $lines.Add("")
    $lines.Add("Safety boundary:")
    $lines.Add("")
    $lines.Add("- Portable v1 is reversible-only: read-only Storage Scan, review, gated Quarantine, and selected restore.")
    $lines.Add("- Portable v1 is not an installer and does not create shortcuts, services, scheduled tasks, or background automation.")
    $lines.Add("- Portable v1 excludes permanent deletion, persisted cleanup history, broad/all-manifest restore, custom real-profile Quarantine, and non-exact real-profile movement.")
    $lines.Add("- Fixture launch only prefills the Cleanup Scope; it does not create fixtures or click Scan.")
    $lines.Add("- Do not move, restore, delete, or modify real-profile files from this acceptance pass.")
    $lines.Add("")
    $lines.Add("Post-pass summary commands:")
    $lines.Add("")
    $lines.Add("After filling this file, run these commands from the repository root:")
    $lines.Add("")
    $lines.Add('```powershell')
    $lines.Add(('.\tools\Record-LocalReleaseAcceptanceNotes.cmd -Path "{0}" -RecordManualAcceptance' -f $notesPath))
    if (-not $RequireCurrentCommitEvidence) {
        $lines.Add(('.\tools\Record-LocalReleaseAcceptanceNotes.cmd -Path "{0}" -RecordManualAcceptance -RecordCommitMismatch' -f $notesPath))
    }
    $lines.Add(('.\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path "{0}"' -f $notesPath))
    $lines.Add(('.\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path "{0}" -RequireComplete' -f $notesPath))
    $lines.Add('```')
    $lines.Add("")
    $lines.Add("These commands read this ignored notes file only. They do not launch WPF, scan, move, restore, delete, approve cleanup, or create cleanup history.")
    $lines.Add("")
    $lines.Add("Overall result:")
    $lines.Add("")
    $lines.Add("- [ ] Pass")
    $lines.Add("- [ ] Pass with issues noted")
    $lines.Add("- [ ] Blocked")
    $lines.Add("")
    $lines.Add("Summary:")
    $lines.Add("")
    $lines.Add("- ")
    $lines.Add("")
    $lines.Add("Checklist:")
    $lines.Add("")
    $lines.Add("Checklist numbers match the terminal output.")

    for ($index = 0; $index -lt $checklistItems.Count; $index++) {
        $lines.Add("")
        $lines.Add("### $($index + 1). Portable release check")
        $lines.Add("")
        $lines.Add("Prompt: $($checklistItems[$index])")
        $lines.Add("")
        if ($index -eq 0) {
            $lines.Add("- [$verifierItemStatus] Pass")
        }
        else {
            $lines.Add("- [ ] Pass")
        }
        $lines.Add("- [ ] Issue")
        $lines.Add("- [ ] Not checked")
        $lines.Add("")
        $lines.Add("Notes:")
        $lines.Add("")
        if ($index -eq 0) {
            $lines.Add("- $verifierItemNotes")
        }
        else {
            $lines.Add("- ")
        }
    }

    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllLines($notesPath, $lines, $utf8NoBom)

    return $notesPath
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

$normalLaunchCommand = Format-LaunchCommand -ExecutablePath $appExePath
$fixtureLaunchCommand = Format-LaunchCommand -ExecutablePath $appExePath -Arguments @("--scope", $fixtureScope)
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

if ($ChecklistOnly.IsPresent) {
    Write-PortableReleaseChecklist `
        -ReleaseDirectory $releaseDir `
        -ReadmeFile $readmePath `
        -NormalLaunchScript $releaseLaunchScriptPath `
        -NormalLaunchCommand $normalLaunchCommand `
        -FixtureLaunchScript $releaseFixtureLaunchScriptPath `
        -FixtureLaunchCommand $fixtureLaunchCommand `
        -ExecutablePath $appExePath `
        -FixtureScopePath $fixtureScope `
        -VerificationSkipped $SkipVerify.IsPresent
    if ($WriteAcceptanceNotes.IsPresent) {
        $notesPath = New-PortableReleaseAcceptanceNotes `
            -ReleaseDirectory $releaseDir `
            -ReadmeFile $readmePath `
            -NormalLaunchScript $releaseLaunchScriptPath `
            -NormalLaunchCommand $normalLaunchCommand `
            -FixtureLaunchScript $releaseFixtureLaunchScriptPath `
            -FixtureLaunchCommand $fixtureLaunchCommand `
            -ExecutablePath $appExePath `
            -FixtureScopePath $fixtureScope `
            -VerificationSkipped $SkipVerify.IsPresent `
            -RequireCurrentCommitEvidence $RequireCurrentCommit.IsPresent
        Write-Host ""
        Write-Host "Portable release acceptance notes template: $notesPath"
        Write-PortableReleaseAcceptanceNotesNextSteps -NotesPath $notesPath -RequireCurrentCommitEvidence $RequireCurrentCommit.IsPresent
    }
    exit 0
}

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
