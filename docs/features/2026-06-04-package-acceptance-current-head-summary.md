# Package Acceptance Current-HEAD Summary

Date: 2026-06-04

Status: completed

## Goal

Make package acceptance summaries show whether the notes creation commit matches the current repository `HEAD`, so pending human acceptance can see package/current-HEAD mismatch context without inferring it from separate commands.

## Safety Profile

`terminal-readonly`. The change adds read-only Git metadata to `Summarize-LocalReleaseAcceptanceNotes.cmd` output and extends the existing temporary-note regression. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete real-profile files, approve cleanup, create shortcuts, install anything, promote a package, write acceptance notes, or create cleanup history.

## Problem

The latest pending package acceptance notes are intentionally behind current `HEAD` after later docs/tooling commits. The summary already showed the notes `Git commit`, but did not show the current repository `HEAD` or a direct notes/current-HEAD status line. That made the expected mismatch harder to review during daily readiness and package acceptance.

## Changes

- `Summarize-LocalReleaseAcceptanceNotes.cmd` now prints `Current repository HEAD`.
- It also prints `Notes/current HEAD status` as `Matches current HEAD`, `Differs from current HEAD`, `Notes commit unavailable`, or `Current HEAD unavailable`.
- Short and full Git commit text are treated as matching when one is a prefix of the other.
- `Test-LocalReleaseAcceptanceSummary.cmd` now covers matching, differing, and missing notes commit cases.

## Verification

- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-164509.md"`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`

## ADRs

No ADR added. This is terminal-only evidence wording for existing package acceptance tooling under ADR 0020 and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

## Follow-Up

- Keep using explicit pending notes paths for candidate package review.
- Treat `Differs from current HEAD` as context for package/current-HEAD mismatch review, not as automatic acceptance or rejection.
