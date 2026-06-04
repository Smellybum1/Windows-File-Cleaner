[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$readmePath = Join-Path $repoRoot "README.md"
$featureIndexPath = Join-Path $repoRoot "docs\features\index.md"
$currentStatePath = Join-Path $repoRoot "docs\codex\current-state.md"
$progressPath = Join-Path $repoRoot ".codex\progress.md"
$threadHandoffPath = Join-Path $repoRoot "docs\codex\thread-handoff.md"
$ciRunbookPath = Join-Path $repoRoot "docs\operations\ci.md"
$mvpPreflightPath = Join-Path $repoRoot "tools\Invoke-MvpPreflight.ps1"

function Get-Text {
    param(
        [Parameter(Mandatory)]
        [string]$Path
    )

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "Required documentation file is missing: $Path"
    }

    return Get-Content -LiteralPath $Path -Raw
}

function Assert-ContainsText {
    param(
        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string]$Text,

        [Parameter(Mandatory)]
        [string]$ExpectedText,

        [Parameter(Mandatory)]
        [string]$Description
    )

    if ($Text.IndexOf($ExpectedText, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        throw "$Description is missing expected text: $ExpectedText"
    }
}

function Get-ReferencedMarkdownPaths {
    param(
        [Parameter(Mandatory)]
        [string]$Text
    )

    $matches = [regex]::Matches($Text, "(?<![A-Za-z0-9_./\\-])(?<path>(?:docs|\.codex|README)[/\\][A-Za-z0-9_./\\-]+\.md|README\.md)")
    $paths = [System.Collections.Generic.SortedSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    foreach ($match in $matches) {
        [void]$paths.Add($match.Groups["path"].Value)
    }

    return @($paths)
}

function Assert-ReferencedPathsExist {
    param(
        [Parameter(Mandatory)]
        [string]$Text,

        [Parameter(Mandatory)]
        [string]$Description
    )

    foreach ($relativePath in Get-ReferencedMarkdownPaths -Text $Text) {
        $pathForJoin = $relativePath -replace "/", "\"
        $fullPath = [System.IO.Path]::GetFullPath((Join-Path $repoRoot $pathForJoin))
        if (-not (Test-Path -LiteralPath $fullPath -PathType Leaf)) {
            throw "$Description references a missing markdown file: $relativePath"
        }
    }
}

function Get-MarkdownSection {
    param(
        [Parameter(Mandatory)]
        [string]$Text,

        [Parameter(Mandatory)]
        [string]$Header
    )

    $pattern = "(?ms)^##\s+$([regex]::Escape($Header))\s*\r?\n(?<body>.*?)(?=^##\s+|\z)"
    $match = [regex]::Match($Text, $pattern)
    if (-not $match.Success) {
        throw "Feature index is missing the '$Header' section."
    }

    return $match.Groups["body"].Value
}

function Get-ActiveFeatureBriefReferences {
    param(
        [Parameter(Mandatory)]
        [string]$FeatureIndexText
    )

    $activeSection = Get-MarkdownSection -Text $FeatureIndexText -Header "Active Or Current"
    $matches = [regex]::Matches($activeSection, '(?m)^\s*-\s+`(?<path>[^`]+\.md)`\s*:')
    $paths = [System.Collections.Generic.SortedSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    foreach ($match in $matches) {
        [void]$paths.Add($match.Groups["path"].Value)
    }

    if ($paths.Count -eq 0) {
        throw "Feature index Active Or Current section does not list any feature briefs."
    }

    return @($paths)
}

function Assert-ActiveFeatureBriefsExist {
    param(
        [Parameter(Mandatory)]
        [string]$FeatureIndexText
    )

    $featuresRoot = [System.IO.Path]::GetFullPath((Join-Path $repoRoot "docs\features"))
    if (-not $featuresRoot.EndsWith([System.IO.Path]::DirectorySeparatorChar)) {
        $featuresRoot = $featuresRoot + [System.IO.Path]::DirectorySeparatorChar
    }

    foreach ($briefReference in Get-ActiveFeatureBriefReferences -FeatureIndexText $FeatureIndexText) {
        $repoRelativePath = if ($briefReference -match "[/\\]") {
            $briefReference
        }
        else {
            Join-Path "docs\features" $briefReference
        }

        $pathForJoin = $repoRelativePath -replace "/", "\"
        $fullPath = [System.IO.Path]::GetFullPath((Join-Path $repoRoot $pathForJoin))
        if (-not $fullPath.StartsWith($featuresRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
            throw "Feature index Active Or Current entry must stay under docs/features: $briefReference"
        }

        if (-not (Test-Path -LiteralPath $fullPath -PathType Leaf)) {
            throw "Feature index Active Or Current entry references a missing feature brief: $briefReference"
        }
    }
}

function Get-CurrentStatePacketName {
    param(
        [Parameter(Mandatory)]
        [string]$CurrentStateText,

        [Parameter(Mandatory)]
        [string]$Label
    )

    $pattern = "(?m)^- $([regex]::Escape($Label)): (?<value>.+?)\.\s*$"
    $match = [regex]::Match($CurrentStateText, $pattern)
    if (-not $match.Success) {
        throw "Current state does not include '$Label'."
    }

    return $match.Groups["value"].Value.Trim([char[]]@([char]" ", [char]0x60))
}

function Get-BacktickPacketSlug {
    param(
        [Parameter(Mandatory)]
        [string]$Text,

        [Parameter(Mandatory)]
        [string]$Label,

        [Parameter(Mandatory)]
        [string]$Description
    )

    $pattern = '(?m)^-? ?' + [regex]::Escape($Label) + ': `(?<slug>[^`]+)`'
    $match = [regex]::Match($Text, $pattern)
    if (-not $match.Success) {
        throw "$Description does not include '$Label' with a backtick-wrapped packet slug."
    }

    return $match.Groups["slug"].Value.Trim()
}

function Convert-PacketSlugToName {
    param(
        [Parameter(Mandatory)]
        [string]$Slug
    )

    $withoutDate = $Slug -replace "^\d{4}-\d{2}-\d{2}-", ""
    return ($withoutDate -replace "-", " ").Trim().ToLowerInvariant()
}

function Normalize-PacketName {
    param(
        [Parameter(Mandatory)]
        [string]$Name
    )

    return ($Name -replace "\s+", " ").Trim().ToLowerInvariant()
}

function Assert-PacketBreadcrumbAligned {
    param(
        [Parameter(Mandatory)]
        [string]$CurrentStateText,

        [Parameter(Mandatory)]
        [string]$ProgressText,

        [Parameter(Mandatory)]
        [string]$ThreadHandoffText,

        [Parameter(Mandatory)]
        [string]$Label
    )

    $currentStateName = Normalize-PacketName -Name (Get-CurrentStatePacketName -CurrentStateText $CurrentStateText -Label $Label)
    $progressSlug = Get-BacktickPacketSlug -Text $ProgressText -Label $Label -Description "Progress log"
    $threadSlug = Get-BacktickPacketSlug -Text $ThreadHandoffText -Label $Label -Description "Thread handoff"

    if ($progressSlug -ne $threadSlug) {
        throw "$Label differs between progress ('$progressSlug') and thread handoff ('$threadSlug')."
    }

    $slugName = Convert-PacketSlugToName -Slug $progressSlug
    if ($currentStateName -ne $slugName) {
        throw "$Label differs between current state ('$currentStateName') and packet slug ('$slugName')."
    }
}

function Get-MvpPreflightSkipSwitches {
    param(
        [Parameter(Mandatory)]
        [string]$PreflightScriptText
    )

    $matches = [regex]::Matches($PreflightScriptText, '\[switch\]\$(?<name>Skip[A-Za-z0-9]+)')
    $switches = [System.Collections.Generic.SortedSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    foreach ($match in $matches) {
        [void]$switches.Add($match.Groups["name"].Value)
    }

    if ($switches.Count -eq 0) {
        throw "MVP preflight script does not expose any skip switches."
    }

    return @($switches)
}

function Assert-MvpPreflightSkipSwitchesDocumented {
    param(
        [Parameter(Mandatory)]
        [string]$CiRunbookText,

        [Parameter(Mandatory)]
        [string]$PreflightScriptText
    )

    foreach ($switchName in Get-MvpPreflightSkipSwitches -PreflightScriptText $PreflightScriptText) {
        Assert-ContainsText `
            -Text $CiRunbookText `
            -ExpectedText "-$switchName" `
            -Description "CI runbook focused local skip switches"
    }
}

$readmeText = Get-Text -Path $readmePath
$featureIndexText = Get-Text -Path $featureIndexPath
$currentStateText = Get-Text -Path $currentStatePath
$progressText = Get-Text -Path $progressPath
$threadHandoffText = Get-Text -Path $threadHandoffPath
$ciRunbookText = Get-Text -Path $ciRunbookPath
$mvpPreflightText = Get-Text -Path $mvpPreflightPath

foreach ($entry in @(
        @{ Text = $readmeText; Description = "README" },
        @{ Text = $featureIndexText; Description = "Feature index" },
        @{ Text = $currentStateText; Description = "Current state" },
        @{ Text = $progressText; Description = "Progress log" },
        @{ Text = $threadHandoffText; Description = "Thread handoff" }
    )) {
    Assert-ReferencedPathsExist -Text $entry.Text -Description $entry.Description
}

Assert-ActiveFeatureBriefsExist -FeatureIndexText $featureIndexText

foreach ($requiredRunbook in @(
        "docs/operations/ci.md",
        "docs/operations/daily-use.md",
        "docs/operations/portable-release.md",
        "docs/operations/manual-fixture-review.md",
        "docs/operations/restore-manifest-review.md"
    )) {
    Assert-ContainsText -Text $featureIndexText -ExpectedText $requiredRunbook -Description "Feature index operational references"
    Assert-ContainsText -Text $threadHandoffText -ExpectedText $requiredRunbook -Description "Thread handoff operational runbooks"
}

Assert-PacketBreadcrumbAligned `
    -CurrentStateText $currentStateText `
    -ProgressText $progressText `
    -ThreadHandoffText $threadHandoffText `
    -Label "Latest docs/workflow packet"

Assert-PacketBreadcrumbAligned `
    -CurrentStateText $currentStateText `
    -ProgressText $progressText `
    -ThreadHandoffText $threadHandoffText `
    -Label "Latest tooling/evidence packet"

Assert-ContainsText `
    -Text $threadHandoffText `
    -ExpectedText "docs/operations/ci.md" `
    -Description "Thread handoff startup context"

Assert-MvpPreflightSkipSwitchesDocumented `
    -CiRunbookText $ciRunbookText `
    -PreflightScriptText $mvpPreflightText

Write-Host "Documentation consistency regression passed."
Write-Host "Boundary: checked committed documentation links, active feature-index entries, packet breadcrumbs, and MVP preflight skip-switch docs only; this did not launch WPF, scan, move, restore, delete, approve cleanup, write manifests, install anything, or create cleanup history."
