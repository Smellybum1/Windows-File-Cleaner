[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$trustManifestScript = Join-Path $PSScriptRoot "New-RealProfileSelectedRestoreTrustManifest.ps1"
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$testQuarantineRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\real-profile-selected-restore-trust-helper-path-guard-test")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$outsideLocalQuarantineRoot = Join-Path $repoRoot "README.md"
$uniqueSuffix = [Guid]::NewGuid().ToString("N")
$safeRelativePath = "WindowsFileCleanerRestoreTrustTest\path-guard-safe-$uniqueSuffix.txt"

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

function Get-CleanupScopeFromPreview {
    param(
        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string[]]$Lines
    )

    $prefix = "Cleanup Scope: "
    foreach ($line in $Lines) {
        if ($line.StartsWith($prefix, [System.StringComparison]::Ordinal)) {
            return $line.Substring($prefix.Length).Trim()
        }
    }

    throw "Trust helper safe preview did not print Cleanup Scope."
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

    $cleanupScope = Get-CleanupScopeFromPreview -Lines $safeResult.Output
    $profileName = Split-Path -Leaf $cleanupScope
    if ([string]::IsNullOrWhiteSpace($profileName)) {
        throw "Trust helper safe preview printed a Cleanup Scope without a profile leaf: $cleanupScope"
    }
    $escapedRelativePath = "..\$profileName\WindowsFileCleanerRestoreTrustTest\path-guard-escape-$uniqueSuffix.txt"

    $outsideLocalResult = Invoke-TrustManifestTool -Arguments @(
        "-QuarantineRoot",
        $outsideLocalQuarantineRoot,
        "-RelativePath",
        $safeRelativePath,
        "-WhatIf"
    )
    if ($outsideLocalResult.ExitCode -ne 1) {
        throw "Trust helper QuarantineRoot outside default D: root and ignored .local should fail before preview output. Exit code: $($outsideLocalResult.ExitCode)"
    }

    Assert-ContainsText -Lines $outsideLocalResult.Output -ExpectedText "QuarantineRoot must stay under the default D: Quarantine Root or the ignored .local directory."
    Assert-DoesNotContainText -Lines $outsideLocalResult.Output -UnexpectedText "Selected real-profile restore trust manifest preview:"
    Assert-DoesNotContainText -Lines $outsideLocalResult.Output -UnexpectedText "Restore Manifest:"
    Assert-DoesNotContainText -Lines $outsideLocalResult.Output -UnexpectedText "Next app command:"

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
Write-Host "Boundary: the helper was run only with -WhatIf, using an ignored .local Quarantine Root for safe preview and committed README.md only as a non-.local rejection target; this did not launch WPF, scan, move, restore, delete, approve cleanup, write Restore Manifests, modify real-profile files, install anything, or create cleanup history."
