# Feature: CI MVP Preflight

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Run the existing MVP preflight on GitHub Actions so pushed checkpoints keep read-only fixture verification visible outside the local machine.

## Non-goals

- Do not scan `C:\Users\moxhe` in CI.
- Do not launch the WPF desktop app visibly.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, manifest writing, or scan-history persistence.
- Do not replace the local pre-real-scan preflight.

## User story / job story

As the project owner, I want pushes to run the same read-only preflight as local development, so that remote checkpoints catch build, test, fixture, and diff problems before I rely on the app for another manual scan.

## Desired behavior

- Pushes to `main` run on a Windows GitHub Actions runner.
- Pull requests to `main` run the same check.
- CI installs .NET 8 SDK version `8.0.421`.
- CI runs `.\tools\Invoke-MvpPreflight.ps1`.
- CI output preserves the no-real-user-files boundary from the preflight script.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| MVP preflight | No domain change; now also runs in CI. | README only |

## Decisions made

- Reuse the local MVP preflight script instead of duplicating command lists in YAML.
- Run only on Windows because the app is WPF and targets `net8.0-windows`.
- Use read-only checkout permissions.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed locally.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed locally.
- GitHub Actions `MVP Preflight` run `26575441204` for commit `711cfb6` completed with conclusion `success`.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `.github/workflows/mvp-preflight.yml`.
- Documented CI preflight behavior in `README.md`.
- Recorded the packet in `.codex/progress.md`.

ADRs added or skipped:

- No ADR. This is a reversible repository verification workflow using the existing MVP preflight.

Follow-up work:

- Inspect future hosted-runner failures if they differ from the local Windows environment.

Rejected ideas buffer:

- Do not create a separate CI command list that can drift from local preflight.
