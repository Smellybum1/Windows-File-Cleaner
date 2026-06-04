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

The no-input push fallback was first validated by GitHub Actions MVP Preflight [#360](https://github.com/Smellybum1/Windows-File-Cleaner/actions/runs/26938525187) on commit `3e2e2ae`. Latest normal push evidence: [#365](https://github.com/Smellybum1/Windows-File-Cleaner/actions/runs/26940183876) passed on commit `98d3412` in `1m 33s`, after the documentation consistency regression started checking active feature-index entries.

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
