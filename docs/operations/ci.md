# CI Runbook

Last updated: 2026-06-04

This runbook covers GitHub Actions MVP Preflight and the manual Windows image canary.

## Normal Push And PR CI

Workflow:

- `.github/workflows/mvp-preflight.yml`
- Job: `Windows read-only preflight`
- Default runner: `windows-2022`
- Command: `tools\Invoke-MvpPreflight.cmd`

Push and pull-request runs use:

```yaml
runs-on: "${{ inputs.runner_image || 'windows-2022' }}"
```

The no-input push fallback was first validated by GitHub Actions MVP Preflight [#360](https://github.com/Smellybum1/Windows-File-Cleaner/actions/runs/26938525187) on commit `3e2e2ae`.

Representative current-path evidence: [#391](https://github.com/Smellybum1/Windows-File-Cleaner/actions/runs/26949669118) passed on commit `9204247` in `1m 41s`, after the daily readiness stop-action reminder joined default MVP preflight coverage. Earlier [#389](https://github.com/Smellybum1/Windows-File-Cleaner/actions/runs/26949030208) passed on commit `00bdfb3` after exact skip-switch documentation coverage joined documentation consistency, earlier [#387](https://github.com/Smellybum1/Windows-File-Cleaner/actions/runs/26948341868) passed on commit `1adce0a` after initial skip-switch documentation coverage joined documentation consistency, earlier [#385](https://github.com/Smellybum1/Windows-File-Cleaner/actions/runs/26947457429) passed on commit `7baf70d` after the real-profile selected restore trust-helper path guard regression joined default MVP preflight, and earlier [#365](https://github.com/Smellybum1/Windows-File-Cleaner/actions/runs/26940183876) passed on commit `98d3412` after active feature-index checks joined documentation consistency. Do not update this line for every green docs-only push; refresh it only when the CI path, runner baseline, or preflight coverage meaningfully changes.

Safety profile: `terminal-readonly` from `docs/codex/safety-profiles.md`. CI restores, builds, runs tests, runs fixture and synthetic regression checks, checks active documentation consistency including active feature-index entries, and checks whitespace. It must not launch WPF, scan `C:\Users\moxhe`, move, restore, delete, approve cleanup, write real Restore Manifests, create shortcuts, install anything, promote a package, or create cleanup history.

## Local Equivalent

Before workflow changes or before real-profile scan review after code/workflow changes, run:

```powershell
.\tools\Invoke-MvpPreflight.cmd
```

Focused documentation consistency check:

```powershell
.\tools\Test-DocumentationConsistency.cmd
```

## Focused Local Skip Switches

Use skip switches only for narrow local loops where the skipped step is outside the current change. Do not use skipped preflight output as real-profile scan or movement evidence.

Current `Invoke-MvpPreflight.cmd` skip switches:

- `-SkipRestore`: skip NuGet restore when dependencies are already restored.
- `-SkipFixtureRootPathGuardCheck`: skip the fixture root path guard regression.
- `-SkipFixtureWhatIf`: skip fixture dry-run output.
- `-SkipFixtureChecklist`: skip fixture checklist-only output.
- `-SkipFixtureAcceptanceNotesCheck`: skip fixture acceptance notes regression.
- `-SkipDailyReadinessFixtureAcceptanceCheck`: skip daily readiness fixture acceptance notes regression.
- `-SkipDailyReadinessLatestPackageNotesCheck`: skip daily readiness latest package notes regression.
- `-SkipLocalReleasePathGuardCheck`: skip local release path guard regression.
- `-SkipLocalReleaseAcceptanceCommandStampingCheck`: skip local release acceptance command stamping regression.
- `-SkipAcceptedLocalReleaseSelectionCheck`: skip accepted local release selection regression.
- `-SkipLocalReleaseAcceptanceSummaryCheck`: skip local release acceptance summary regression.
- `-SkipLocalReleaseAcceptanceRecorderCheck`: skip local release acceptance recorder regression.
- `-SkipRealProfileSelectedRestoreTrustHelperPathGuardCheck`: skip real-profile selected restore trust helper path guard regression.
- `-SkipRealProfileNextBatchStopGuardCheck`: skip real-profile next-batch stop guard regression.
- `-SkipDailyReadinessUndoSpotlightCheck`: skip daily readiness exact-profile undo spotlight regression.
- `-SkipDocumentationConsistencyCheck`: skip documentation consistency regression.
- `-SkipDiffCheck`: skip whitespace diff check.

`tools\Test-DocumentationConsistency.cmd` verifies this section exactly matches the current `Invoke-MvpPreflight.cmd` skip switches, so missing or stale switch entries fail before preflight can pass.

## Manual Windows Image Canary

Use this only when intentionally evaluating Windows 2025 / Visual Studio 2026 hosted runner readiness.

1. Open GitHub Actions for the repository.
2. Select `MVP Preflight`.
3. Run the workflow manually from branch `main`.
4. Choose `runner_image=windows-2025-vs2026`.
5. Review the `Show runner image` step for `Requested runner image`, `RUNNER_OS`, `ImageOS`, and `ImageVersion`.
6. Review the full preflight result and record the run number before making any baseline decision.

Use `runner_image=windows-2022` only as a manual control run. Normal push and pull-request runs already use `windows-2022`.

## Baseline Change Rule

A passing canary does not by itself move the baseline. If the human accepts the canary result, update the default runner in a separate packet, run local MVP preflight, push, and verify the new push CI result.

The manual canary is not package acceptance evidence, real-profile movement evidence, or approval for cleanup execution.
