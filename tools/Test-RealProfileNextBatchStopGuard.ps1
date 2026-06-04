[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$localRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$testRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot ".local\real-profile-next-batch-stop-guard-test")).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
$readinessScript = Join-Path $PSScriptRoot "Invoke-RealProfileQuarantineReadiness.ps1"
$cleanupScope = "C:\Users\moxhe"

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

function New-TestRestoreManifest {
    param(
        [Parameter(Mandatory)]
        [string]$QuarantineRoot,

        [Parameter(Mandatory)]
        [string]$ActionId,

        [Parameter(Mandatory)]
        [ValidateSet("Moved", "Restored")]
        [string]$EntryStatus
    )

    $actionRoot = Join-Path (Join-Path $QuarantineRoot "actions") $ActionId
    $itemsRoot = Join-Path $actionRoot "items"
    $manifestPath = Join-Path $actionRoot "restore-manifest.json"
    New-Item -ItemType Directory -Path $itemsRoot -Force | Out-Null

    $now = "2026-06-04T00:00:00.0000000Z"
    $relativePath = "AppData\Local\pip\cache\http-v2\stop-guard.body"
    $quarantinePath = Join-Path $itemsRoot $relativePath
    $actionStatus = if ($EntryStatus -eq "Moved") { "Completed" } else { "Restored" }
    $restoreStartedAtUtc = if ($EntryStatus -eq "Restored") { $now } else { $null }
    $restoreCompletedAtUtc = if ($EntryStatus -eq "Restored") { $now } else { $null }

    $manifest = [pscustomobject]@{
        schemaVersion = "restore-manifest.v1"
        manifestId = "restore-manifest-$ActionId"
        restoreManifestDraftId = "manifest-draft-$ActionId"
        actionId = $ActionId
        createdAtUtc = $now
        updatedAtUtc = $now
        cleanupScopePath = $cleanupScope
        quarantineRootPath = $QuarantineRoot
        actionRootPath = $actionRoot
        itemsRootPath = $itemsRoot
        manifestPath = $manifestPath
        actionStatus = $actionStatus
        entries = @(
            [pscustomobject]@{
                originalPath = Join-Path $cleanupScope $relativePath
                relativePath = $relativePath
                quarantinePath = $quarantinePath
                isDirectory = $false
                sizeBytes = 1024
                lastModifiedUtc = $now
                importanceRating = "LikelySafe"
                deletionRecommendation = "QuarantineCandidate"
                bloatCategories = @("AppCache")
                evidence = "Synthetic next-batch stop guard manifest."
                status = $EntryStatus
                moveStartedAtUtc = $now
                moveCompletedAtUtc = $now
                restoreStartedAtUtc = $restoreStartedAtUtc
                restoreCompletedAtUtc = $restoreCompletedAtUtc
                errorMessage = $null
            }
        )
        writeOrderNotes = @("Synthetic next-batch stop guard manifest.")
    }

    $utf8NoBom = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllText($manifestPath, ($manifest | ConvertTo-Json -Depth 8), $utf8NoBom)
}

function Invoke-ReadinessPreset {
    param(
        [Parameter(Mandatory)]
        [string]$QuarantineRoot
    )

    $arguments = @(
        "-NoProfile",
        "-ExecutionPolicy",
        "Bypass",
        "-File",
        $readinessScript,
        "-SkipMvpPreflight",
        "-QuarantineRoot",
        $QuarantineRoot,
        "-RequireNextBatchEvidence"
    )

    $output = @(powershell.exe @arguments 2>&1 | ForEach-Object { [string]$_ })
    return [pscustomobject]@{
        ExitCode = $LASTEXITCODE
        Output = $output
    }
}

Assert-UnderLocalPath -Path $testRoot

$blockedRoot = Join-Path $testRoot "blocked"
$clearRoot = Join-Path $testRoot "clear"
Assert-UnderLocalPath -Path $blockedRoot
Assert-UnderLocalPath -Path $clearRoot

try {
    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }

    New-TestRestoreManifest -QuarantineRoot $blockedRoot -ActionId "blocked-undo-work" -EntryStatus "Moved"
    New-TestRestoreManifest -QuarantineRoot $clearRoot -ActionId "clear-restored" -EntryStatus "Restored"

    $blockedResult = Invoke-ReadinessPreset -QuarantineRoot $blockedRoot
    if ($blockedResult.ExitCode -ne 1) {
        throw "Blocked next-batch preset should exit 1 when displayed undo work exists. Exit code: $($blockedResult.ExitCode)"
    }

    Assert-ContainsText -Lines $blockedResult.Output -ExpectedText "Displayed undo-work stop: checking before MVP preflight"
    Assert-ContainsText -Lines $blockedResult.Output -ExpectedText "== Early displayed undo-work stop check =="
    Assert-ContainsText -Lines $blockedResult.Output -ExpectedText "Restore Manifest summary failed because -RequireNoDisplayedUndoWork was set"
    Assert-DoesNotContainText -Lines $blockedResult.Output -UnexpectedText "== Full MVP preflight =="
    Assert-DoesNotContainText -Lines $blockedResult.Output -UnexpectedText "== Daily local readiness =="

    $clearResult = Invoke-ReadinessPreset -QuarantineRoot $clearRoot
    if ($clearResult.ExitCode -ne 0) {
        throw "Clear next-batch preset should pass when displayed undo work is absent. Exit code: $($clearResult.ExitCode)"
    }

    Assert-ContainsText -Lines $clearResult.Output -ExpectedText "== Early displayed undo-work stop check =="
    Assert-ContainsText -Lines $clearResult.Output -ExpectedText "MVP preflight: skipped by request. Do not use skipped preflight output as fresh real-profile movement evidence."
    Assert-ContainsText -Lines $clearResult.Output -ExpectedText "== Daily local readiness =="
    Assert-ContainsText -Lines $clearResult.Output -ExpectedText "Real-profile Quarantine readiness review passed."

    Write-Host "Real-profile next-batch stop guard regression passed."
    Write-Host "Boundary: temporary manifests were written under ignored .local only; this did not launch WPF, scan, move, restore, delete, approve cleanup, write real Restore Manifests, or create cleanup history."
}
finally {
    if (Test-Path -LiteralPath $testRoot -PathType Container) {
        Remove-Item -LiteralPath $testRoot -Recurse -Force
    }
}
