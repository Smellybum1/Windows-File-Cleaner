[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$trustManifestScript = Join-Path $PSScriptRoot "New-RealProfileSelectedRestoreTrustManifest.ps1"
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$testQuarantineRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\real-profile-selected-restore-trust-helper-path-guard-test")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$uniqueSuffix = [Guid]::NewGuid().ToString("N")
$safeRelativePath = "WindowsFileCleanerRestoreTrustTest\path-guard-safe-$uniqueSuffix.txt"
$escapedRelativePath = "..\moxhe\WindowsFileCleanerRestoreTrustTest\path-guard-escape-$uniqueSuffix.txt"

if (-not $testQuarantineRoot.StartsWith($localRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "Test quarantine root must stay under ignored .local before cleanup: $localRoot"
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

function Invoke-TrustManifestTool {
    param(
        [Parameter(Mandatory)]
        [string[]]$Arguments
    )

    $commandArguments = @(
        "-NoProfile",
        "-ExecutionPolicy",
        "Bypass",
        "-File",
        $trustManifestScript
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

try {
    $safeResult = Invoke-TrustManifestTool -Arguments @(
        "-QuarantineRoot",
        $testQuarantineRoot,
        "-RelativePath",
        $safeRelativePath,
        "-WhatIf"
    )
    if ($safeResult.ExitCode -ne 0) {
        throw "Trust helper safe WhatIf should pass. Exit code: $($safeResult.ExitCode)"
    }

    Assert-ContainsText -Lines $safeResult.Output -ExpectedText "Selected real-profile restore trust manifest preview:"
    Assert-ContainsText -Lines $safeResult.Output -ExpectedText "Quarantine Root: $testQuarantineRoot"
    Assert-ContainsText -Lines $safeResult.Output -ExpectedText "Next app command:"

    $escapedResult = Invoke-TrustManifestTool -Arguments @(
        "-QuarantineRoot",
        $testQuarantineRoot,
        "-RelativePath",
        $escapedRelativePath,
        "-WhatIf"
    )
    if ($escapedResult.ExitCode -ne 1) {
        throw "Trust helper escaped RelativePath should fail before preview output. Exit code: $($escapedResult.ExitCode)"
    }

    Assert-ContainsText -Lines $escapedResult.Output -ExpectedText "Quarantine source must stay inside the action items root:"
    Assert-DoesNotContainText -Lines $escapedResult.Output -UnexpectedText "Selected real-profile restore trust manifest preview:"
    Assert-DoesNotContainText -Lines $escapedResult.Output -UnexpectedText "Restore Manifest:"
    Assert-DoesNotContainText -Lines $escapedResult.Output -UnexpectedText "Next app command:"

    if (Test-Path -LiteralPath $testQuarantineRoot) {
        throw "Trust helper path guard regression should not create test quarantine files in WhatIf mode: $testQuarantineRoot"
    }
}
finally {
    if (Test-Path -LiteralPath $testQuarantineRoot) {
        Remove-Item -LiteralPath $testQuarantineRoot -Recurse -Force
    }
}

Write-Host "Real-profile selected restore trust manifest path guard regression passed."
Write-Host "Boundary: the helper was run only with -WhatIf and an ignored .local Quarantine Root; this did not launch WPF, scan, move, restore, delete, approve cleanup, write Restore Manifests, modify real-profile files, install anything, or create cleanup history."
