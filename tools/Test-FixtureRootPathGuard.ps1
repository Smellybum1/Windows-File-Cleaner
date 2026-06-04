[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$outsideLocalPath = Join-Path $repoRoot "README.md"
$fixtureCreatorScript = Join-Path $PSScriptRoot "New-StorageScanSmokeFixture.ps1"
$fixtureReviewScript = Join-Path $PSScriptRoot "Start-MvpFixtureReview.ps1"

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

function Assert-GuardedCreatorFailure {
    $result = Invoke-Tool -ScriptPath $fixtureCreatorScript -Arguments @("-Root", $outsideLocalPath, "-WhatIf")
    if ($result.ExitCode -ne 1) {
        throw "Fixture creator explicit Root outside ignored .local should fail before WhatIf output. Exit code: $($result.ExitCode)"
    }

    Assert-ContainsText -Lines $result.Output -ExpectedText "Fixture root must stay under the ignored .local directory:"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "What if:"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Create synthetic Storage Scan fixture file"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Fixture Cleanup Scope:"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Run WPF smoke test:"
}

function Assert-GuardedReviewFailure {
    $result = Invoke-Tool -ScriptPath $fixtureReviewScript -Arguments @("-FixtureRoot", $outsideLocalPath, "-ChecklistOnly")
    if ($result.ExitCode -ne 1) {
        throw "Fixture review explicit FixtureRoot outside ignored .local should fail before checklist output. Exit code: $($result.ExitCode)"
    }

    Assert-ContainsText -Lines $result.Output -ExpectedText "Fixture root must stay under the ignored .local directory:"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Fixture Cleanup Scope:"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Checklist-only mode."
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Manual fixture review checklist:"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "Next visible fixture command"
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "The WPF app will only prefill the Cleanup Scope."
}

Assert-GuardedCreatorFailure
Assert-GuardedReviewFailure

Write-Host "Fixture root path guard regression passed."
Write-Host "Boundary: committed README.md was used only as a non-.local rejection target; this did not launch WPF, create fixture files, scan, move, restore, delete, approve cleanup, write acceptance notes, write Restore Manifests, install anything, or create cleanup history."
