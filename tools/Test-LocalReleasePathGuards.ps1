[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$outsideLocalPath = Join-Path $repoRoot "README.md"
$localReleasePublisherScript = Join-Path $PSScriptRoot "Publish-LocalRelease.ps1"
$localReleaseVerifierScript = Join-Path $PSScriptRoot "Test-LocalRelease.ps1"
$localReleaseLauncherScript = Join-Path $PSScriptRoot "Start-LocalRelease.ps1"

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
        throw "Expected output to contain: $ExpectedText"
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
        throw "Expected output not to contain: $UnexpectedText"
    }
}

function Invoke-Tool {
    param(
        [Parameter(Mandatory)]
        [string]$ScriptPath,

        [string[]]$Arguments = @()
    )

    $commandArguments = @(
        "-NoProfile",
        "-ExecutionPolicy",
        "Bypass",
        "-File",
        $ScriptPath
    ) + $Arguments

    $previousErrorActionPreference = $ErrorActionPreference
    $ErrorActionPreference = "Continue"
    try {
        $output = @(powershell.exe @commandArguments 2>&1 | ForEach-Object { [string]$_ })
    }
    finally {
        $ErrorActionPreference = $previousErrorActionPreference
    }

    return [pscustomobject]@{
        ExitCode = $LASTEXITCODE
        Output = $output
    }
}

function Assert-GuardedVerifierFailure {
    param(
        [Parameter(Mandatory)]
        [string]$Description,

        [Parameter(Mandatory)]
        [string[]]$Arguments,

        [Parameter(Mandatory)]
        [string]$ExpectedGuardText
    )

    $result = Invoke-Tool -ScriptPath $localReleaseVerifierScript -Arguments $Arguments
    if ($result.ExitCode -ne 1) {
        throw "$Description should fail before verifier output. Exit code: $($result.ExitCode)"
    }

    Assert-ContainsText -Lines $result.Output -ExpectedText $ExpectedGuardText
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Portable v1 release verifier"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "This verifier reads local ignored release files only"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Portable release verification passed."
}

function Assert-GuardedPublisherFailure {
    param(
        [Parameter(Mandatory)]
        [string]$Description,

        [Parameter(Mandatory)]
        [string[]]$Arguments,

        [Parameter(Mandatory)]
        [string]$ExpectedGuardText
    )

    $result = Invoke-Tool -ScriptPath $localReleasePublisherScript -Arguments $Arguments
    if ($result.ExitCode -ne 1) {
        throw "$Description should fail before publisher output. Exit code: $($result.ExitCode)"
    }

    Assert-ContainsText -Lines $result.Output -ExpectedText $ExpectedGuardText
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Portable v1 release publisher"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Current git status:"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Skipping MVP preflight by request"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Publish self-contained WPF app"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Portable v1 release package created."
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Launch command:"
}

function Assert-GuardedLauncherFailure {
    param(
        [Parameter(Mandatory)]
        [string]$Description,

        [Parameter(Mandatory)]
        [string[]]$Arguments,

        [Parameter(Mandatory)]
        [string]$ExpectedGuardText
    )

    $result = Invoke-Tool -ScriptPath $localReleaseLauncherScript -Arguments $Arguments
    if ($result.ExitCode -ne 1) {
        throw "$Description should fail before launcher output. Exit code: $($result.ExitCode)"
    }

    Assert-ContainsText -Lines $result.Output -ExpectedText $ExpectedGuardText
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Portable release verifier"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Portable release launcher"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Launch command:"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Print-only mode: WPF was not launched."
}

Assert-GuardedPublisherFailure `
    -Description "Publisher explicit ReleaseRoot outside ignored .local" `
    -Arguments @("-ReleaseRoot", $outsideLocalPath, "-SkipPreflight") `
    -ExpectedGuardText "Release root must stay under the ignored .local directory:"

Assert-GuardedVerifierFailure `
    -Description "Verifier explicit ReleaseRoot outside ignored .local" `
    -Arguments @("-ReleaseRoot", $outsideLocalPath) `
    -ExpectedGuardText "Release root must stay under the ignored .local directory:"

Assert-GuardedVerifierFailure `
    -Description "Verifier explicit ReleasePath outside ignored .local" `
    -Arguments @("-ReleasePath", $outsideLocalPath) `
    -ExpectedGuardText "Release path must stay under the ignored .local directory:"

Assert-GuardedLauncherFailure `
    -Description "Launcher explicit ReleaseRoot outside ignored .local" `
    -Arguments @("-ReleaseRoot", $outsideLocalPath, "-PrintOnly", "-SkipVerify") `
    -ExpectedGuardText "Release root must stay under the ignored .local directory:"

Assert-GuardedLauncherFailure `
    -Description "Launcher explicit ReleasePath outside ignored .local" `
    -Arguments @("-ReleasePath", $outsideLocalPath, "-PrintOnly", "-SkipVerify") `
    -ExpectedGuardText "Release path must stay under the ignored .local directory:"

Write-Host "Local release path guard regression passed."
Write-Host "Boundary: committed README.md was used only as a non-.local rejection target; this did not launch WPF, scan, move, restore, delete, approve cleanup, publish or promote a package, create shortcuts, install anything, write acceptance notes, write Restore Manifests, or create cleanup history."
