# Feature: CI Windows Image Canary

Date started: 2026-06-04
Status: completed
Owner: project-owner

## Goal

Let the project intentionally test the Windows 2025 / Visual Studio 2026 hosted runner image without moving the normal push and pull-request CI baseline off `windows-2022`.

## Non-goals

- Do not change the MVP preflight command or its checks.
- Do not add a second duplicated preflight workflow.
- Do not scan `C:\Users\moxhe`.
- Do not launch WPF.
- Do not move, restore, delete, quarantine, approve cleanup, write Restore Manifests, create shortcuts, install anything, promote a package, or create cleanup history.

## Desired behavior

- Pushes and pull requests still run MVP Preflight on `windows-2022`.
- Manual MVP Preflight runs can choose `windows-2025-vs2026` or `windows-2022`.
- The workflow prints the requested runner image plus hosted image environment variables before running the unchanged preflight command.
- Checkout permissions stay read-only.

## Evidence and validation gate

Evidence gathered:

- GitHub workflow syntax supports `workflow_dispatch` inputs.
- The `choice` input type resolves to a single string.
- `jobs.<job_id>.runs-on` supports a variable containing a runner label, with expressions quoted.
- GitHub's Windows image migration notice recommends `windows-2025-vs2026` for testing and `windows-2022` for staying on Visual Studio 2022.

Source references:

- https://docs.github.com/en/actions/reference/workflows-and-actions/workflow-syntax
- https://github.blog/changelog/2026-05-14-github-actions-upcoming-image-migrations

Validation gate before implementation:

- [x] The change is limited to CI tooling and docs.
- [x] The local preflight entry point stays unchanged.
- [x] Push and pull-request runs have a `windows-2022` fallback when no manual input is present.

## Decisions made

Small feature-level decisions:

- Add `workflow_dispatch` to the existing MVP Preflight workflow instead of creating a separate workflow.
- Add a required `runner_image` choice with default `windows-2025-vs2026`.
- Use `runs-on: "${{ inputs.runner_image || 'windows-2022' }}"` so normal push and pull-request runs keep the stable baseline.
- Print runner image context before .NET setup and preflight.

ADR-worthy decisions:

- [x] None.

## Completion notes

Completed on: 2026-06-04

What changed:

- MVP Preflight can now be manually dispatched with a Windows runner image choice.
- Normal push and pull-request MVP Preflight runs still default to `windows-2022`.
- CI logs now print requested runner image, `RUNNER_OS`, `ImageOS`, and `ImageVersion`.

Tests run:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- GitHub Actions [MVP Preflight #360](https://github.com/Smellybum1/Windows-File-Cleaner/actions/runs/26938525187) on push commit `3e2e2ae` passed in `1m 27s`.
- The #360 push run proves the no-input fallback resolved successfully for a normal push event while keeping the baseline on `windows-2022`.

Docs updated:

- This feature brief.
- `docs/operations/ci.md` later captured the manual canary runbook and safety boundaries.
- `docs/features/index.md`
- `docs/features/2026-06-04-ci-actions-runtime-maintenance.md`
- `docs/features/2026-05-28-ci-mvp-preflight.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/codex/current-state.md`
- `.codex/progress.md`

ADRs added or skipped:

- Skipped. This is reversible CI workflow instrumentation and does not change product behavior, cleanup execution, restore execution, persistence, security, data model, deployment packaging, or core UX flow.

Open questions:

- None for this packet. The actual Windows 2025 / Visual Studio 2026 decision remains deferred until a human intentionally runs and reviews the manual canary.

Follow-up work:

- Use `docs/operations/ci.md` to manually run MVP Preflight with `runner_image=windows-2025-vs2026` when the project is ready to evaluate that image.
- If the canary passes and the human accepts the new image, update the default CI runner baseline in a separate packet.

Risky assumptions:

- The `inputs.runner_image || 'windows-2022'` expression has been validated for push runs; pull-request fallback behavior has not been separately observed, but it uses the same no-input path.
- A manual canary is enough for future image evaluation plumbing; it does not prove Windows 2025 / Visual Studio 2026 readiness until the canary is actually run and reviewed.
