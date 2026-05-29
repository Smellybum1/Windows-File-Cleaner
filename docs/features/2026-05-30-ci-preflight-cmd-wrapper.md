# Feature: CI Preflight CMD Wrapper Alignment

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make GitHub Actions run MVP preflight through the same execution-policy-friendly `.cmd` wrapper that README and local user instructions prefer.

## Non-goals

- Do not change the preflight steps.
- Do not change Storage Scan, Quarantine Preview, fixture execution, undo, restore, manifests, or real-profile cleanup availability.
- Do not scan `C:\Users\moxhe`.

## Desired behavior

- CI runs `tools\Invoke-MvpPreflight.cmd`.
- The wrapper still invokes `Invoke-MvpPreflight.ps1` with process-scoped `-ExecutionPolicy Bypass`.
- CI continues to restore, build, run both test harnesses, run the fixture dry run, print the fixture checklist, and run `git diff --check`.

## Domain language changes

No new domain terms. This is verification tooling only.

## Evidence and validation gate

Evidence gathered:

- User-facing docs now prefer `.cmd` wrappers because direct `.ps1` execution can be blocked by PowerShell policy.
- CI still called `Invoke-MvpPreflight.ps1` directly, so it did not prove the preferred wrapper path.

Validation gate before implementation:

- [x] The change is limited to CI tooling and docs.
- [x] The wrapper already exists and is covered locally.
- [x] The narrowest relevant check is running the wrapper locally with a focused preflight.

## Decisions made

Small feature-level decisions:

- Use `shell: cmd` for the CI preflight step so the workflow exercises the batch wrapper directly.
- Keep all preflight behavior inside the existing script.

ADR-worthy decisions:

- [x] None.

## Completion notes

Completed on: 2026-05-30

What changed:

- Updated GitHub Actions MVP preflight to call `tools\Invoke-MvpPreflight.cmd`.
- Updated README, preflight feature notes, wrapper feature notes, progress, and handoff docs to record that CI now verifies the same wrapper path used locally.

Tests run:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore`
- `git diff --check`

ADRs added or skipped:

- Skipped. This is reversible verification tooling and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None.

Risky assumptions:

- Running the wrapper through `shell: cmd` on `windows-latest` is the clearest CI match for the local `.cmd` command.
