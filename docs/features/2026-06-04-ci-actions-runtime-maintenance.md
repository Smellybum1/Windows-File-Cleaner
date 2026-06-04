# Feature: CI Actions Runtime Maintenance

Date started: 2026-06-04
Status: completed
Owner: project-owner

## Goal

Keep the GitHub Actions MVP Preflight workflow on a stable, read-only Windows baseline while removing upcoming hosted-runner runtime warnings.

## Non-goals

- Do not change the MVP preflight command or its checks.
- Do not scan `C:\Users\moxhe`.
- Do not launch WPF.
- Do not move, restore, delete, quarantine, approve cleanup, write Restore Manifests, create shortcuts, install anything, or create cleanup history.
- Do not promote the pending portable package candidate.

## Desired behavior

- Pushes and pull requests still run the same read-only `tools\Invoke-MvpPreflight.cmd` entry point.
- CI uses Node 24-capable official GitHub actions.
- CI pins the hosted runner to `windows-2022` until the project intentionally validates the newer Windows 2025 / Visual Studio 2026 image.
- The workflow keeps read-only checkout permissions.
- Later packet `2026-06-04-ci-windows-image-canary.md` added a manual-only runner-image choice for intentional `windows-2025-vs2026` evaluation while keeping push and pull-request runs on `windows-2022`.

## Evidence and validation gate

Evidence gathered:

- GitHub's Node 20 deprecation notice says users should update workflows to newer action versions that run on Node 24.
- `actions/setup-dotnet@v5` documents the Node 24 runtime change and keeps the existing .NET SDK version input.
- `actions/checkout@v6` declares `runs.using: node24`; the workflow does not rely on authenticated git operations after checkout.
- GitHub's Windows image migration notice says `windows-latest` will migrate to Windows 2025 with Visual Studio 2026 in June 2026 and recommends `windows-2022` for users who want to remain on Visual Studio 2022.

Source references:

- https://github.blog/changelog/2025-09-19-deprecation-of-node-20-on-github-actions-runners/
- https://raw.githubusercontent.com/actions/setup-dotnet/v5/README.md
- https://raw.githubusercontent.com/actions/checkout/v6/action.yml
- https://github.blog/changelog/2026-05-14-github-actions-upcoming-image-migrations

Validation gate before implementation:

- [x] The change is limited to CI tooling and docs.
- [x] The local preflight entry point stays unchanged.
- [x] The runner image choice preserves the current CI baseline instead of silently accepting the image migration.

## Decisions made

Small feature-level decisions:

- Update `actions/checkout` from `v4` to `v6`.
- Update `actions/setup-dotnet` from `v4` to `v5`.
- Pin `runs-on` from `windows-latest` to `windows-2022`.

ADR-worthy decisions:

- [x] None.

## Completion notes

Completed on: 2026-06-04

What changed:

- GitHub Actions MVP Preflight now uses `actions/checkout@v6`.
- GitHub Actions MVP Preflight now uses `actions/setup-dotnet@v5`.
- GitHub Actions MVP Preflight now runs on `windows-2022` instead of floating `windows-latest`.

Tests run:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- GitHub Actions MVP Preflight #359 and #360 passed on pushed commits after the workflow moved to `actions/checkout@v6`, `actions/setup-dotnet@v5`, and `windows-2022`.

Docs updated:

- This feature brief.
- `docs/features/index.md`
- `docs/codex/current-state.md`
- `.codex/progress.md`

ADRs added or skipped:

- Skipped. This is reversible CI runtime maintenance and does not change product behavior, cleanup execution, restore execution, persistence, security, data model, deployment packaging, or core UX flow.

Open questions:

- Answered for tooling: the project now has a manual `workflow_dispatch` canary. The baseline move itself remains deferred until a human intentionally runs and reviews that canary.

Follow-up work:

- Use `docs/operations/ci.md` to run the manual canary with `runner_image=windows-2025-vs2026` before changing the baseline.

Risky assumptions:

- Hosted `windows-2022` runner support for the Node 24 official actions is validated by successful push CI runs after the update.
- Pinning to `windows-2022` remains preferable to accepting the Visual Studio 2026 migration implicitly for this Windows desktop app's release-readiness checks.
