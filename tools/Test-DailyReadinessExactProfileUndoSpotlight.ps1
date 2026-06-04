[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$testRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\daily-readiness-undo-spotlight-test")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$dailyReadinessScript = Join-Path $PSScriptRoot "Invoke-DailyLocalReadiness.ps1"
$exactCleanupScope = "C:\Users\moxhe"
$fixtureCleanupScope = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\storage-scan-smoke-fixture")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)

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

function Assert-ContainsText {
    param(
        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string[]]$Lines,

        [Parameter(Mandatory)]
        [string]$ExpectedText
    )

    $joined = ($Lines -join [Environment]::NewLine)
    if ($joined.IndexOf($ExpectedText, [System.StringComparison]::Ordinal) -lt 0) {
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
    if ($joined.IndexOf($UnexpectedText, [System.StringComparison]::Ordinal) -ge 0) {
        throw "Expected output not to contain: $UnexpectedText"
    }
}

function Get-OutputSection {
    param(
        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string[]]$Lines,

        [Parameter(Mandatory)]
        [string]$StartText
    )

    for ($index = 0; $index -lt $Lines.Count; $index++) {
        if ($Lines[$index].IndexOf($StartText, [System.StringComparison]::Ordinal) -ge 0) {
            return @($Lines[$index..($Lines.Count - 1)])
        }
    }

    throw "Expected output section to start with: $StartText"
}

function New-TestRestoreManifest {
    param(
        [Parameter(Mandatory)]
        [string]$QuarantineRoot,

        [Parameter(Mandatory)]
        [string]$CleanupScope,

        [Parameter(Mandatory)]
        [string]$ActionId,

        [Parameter(Mandatory)]
        [string]$RelativePath
    )

    Assert-UnderLocalPath -Path $QuarantineRoot

    $actionRoot = Join-Path (Join-Path $QuarantineRoot "actions") $ActionId
    $itemsRoot = Join-Path $actionRoot "items"
    $manifestPath = Join-Path $actionRoot "restore-manifest.json"
    New-Item -ItemType Directory -Path $itemsRoot -Force | Out-Null

    $now = "2026-06-04T00:00:00.0000000Z"
    $quarantinePath = Join-Path $itemsRoot $RelativePath

    $manifest = [pscustomobject]@{
        schemaVersion = "restore-manifest.v1"
        manifestId = "restore-manifest-$ActionId"
        restoreManifestDraftId = "manifest-draft-$ActionId"
        actionId = $ActionId
        createdAtUtc = $now
        updatedAtUtc = $now
        cleanupScopePath = $CleanupScope
        quarantineRootPath = $QuarantineRoot
        actionRootPath = $actionRoot
        itemsRootPath = $itemsRoot
        manifestPath = $manifestPath
        actionStatus = "Completed"
        entries = @(
            [pscustomobject]@{
                originalPath = Join-Path $CleanupScope $RelativePath
                relativePath = $RelativePath
                quarantinePath = $quarantinePath
                isDirectory = $false
                sizeBytes = 2048
                lastModifiedUtc = $now
                importanceRating = "LikelySafe"
                deletionRecommendation = "QuarantineCandidate"
                bloatCategories = @("AppCache")
                evidence = "Synthetic daily readiness exact-profile undo spotlight manifest."
                status = "Moved"
                moveStartedAtUtc = $now
                moveCompletedAtUtc = $now
                restoreStartedAtUtc = $null
                restoreCompletedAtUtc = $null
                errorMessage = $null
            }
        )
        writeOrderNotes = @("Synthetic daily readiness exact-profile undo spotlight manifest.")
    }

    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllText($manifestPath, ($manifest | ConvertTo-Json -Depth 8), $utf8NoBom)
}

function Invoke-DailyReadiness {
    param(
        [Parameter(Mandatory)]
        [string]$QuarantineRoot
    )

    $arguments = @(
        "-NoProfile",
        "-ExecutionPolicy",
        "Bypass",
        "-File",
        $dailyReadinessScript,
        "-SyntheticRestoreManifestOnly",
        "-QuarantineRoot",
        $QuarantineRoot
    )

    $output = @(powershell.exe @arguments 2>&1 | ForEach-Object { [string]$_ })
    return [pscustomobject]@{
        ExitCode = $LASTEXITCODE
        Output = $output
    }
}

Assert-UnderLocalPath -Path $testRoot

$exactActionId = "spotlight-exact-undo-work"
$fixtureActionId = "spotlight-fixture-undo-work"

try {
    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }

    New-TestRestoreManifest -QuarantineRoot $testRoot -CleanupScope $exactCleanupScope -ActionId $exactActionId -RelativePath "AppData\Local\pip\cache\http-v2\spotlight-exact.body"
    New-TestRestoreManifest -QuarantineRoot $testRoot -CleanupScope $fixtureCleanupScope -ActionId $fixtureActionId -RelativePath "fixture-cache\spotlight-fixture.tmp"

    $result = Invoke-DailyReadiness -QuarantineRoot $testRoot
    if ($result.ExitCode -ne 0) {
        throw "Daily readiness should pass for synthetic Restore Manifest spotlight evidence. Exit code: $($result.ExitCode)"
    }

    Assert-ContainsText -Lines $result.Output -ExpectedText "== Restore Manifest summary =="
    Assert-ContainsText -Lines $result.Output -ExpectedText "Synthetic Restore Manifest-only mode: accepted package evidence, accepted launch commands, and fixture acceptance notes are skipped for focused regression coverage."
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "== Accepted package evidence =="
    Assert-DoesNotContainText -Lines $result.Output -UnexpectedText "== Accepted normal launch command =="
    Assert-ContainsText -Lines $result.Output -ExpectedText $exactActionId
    Assert-ContainsText -Lines $result.Output -ExpectedText $fixtureActionId
    Assert-ContainsText -Lines $result.Output -ExpectedText "== Exact-profile undo-work stop state =="
    Assert-ContainsText -Lines $result.Output -ExpectedText "Daily local readiness check passed."

    $spotlightSection = Get-OutputSection -Lines $result.Output -StartText "== Exact-profile undo-work stop state =="
    Assert-ContainsText -Lines $spotlightSection -ExpectedText "Display filter: Cleanup Scope: C:\Users\moxhe; undo-work manifests only (1 of 2)"
    Assert-ContainsText -Lines $spotlightSection -ExpectedText "Displayed manifests with undo work: 1"
    Assert-ContainsText -Lines $spotlightSection -ExpectedText $exactActionId
    Assert-DoesNotContainText -Lines $spotlightSection -UnexpectedText $fixtureActionId

    Write-Host "Daily readiness exact-profile undo spotlight regression passed."
    Write-Host "Boundary: temporary manifests were written under ignored .local only; this did not launch WPF, scan, move, restore, delete, approve cleanup, write real Restore Manifests, or create cleanup history."
}
finally {
    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }
}
