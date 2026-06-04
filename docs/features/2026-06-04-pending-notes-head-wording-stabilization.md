# Pending Notes HEAD Wording Stabilization

Date: 2026-06-04

Status: completed

## Goal

Keep pending package acceptance notes documentation accurate after later docs/tooling commits advance `HEAD` beyond the commit recorded in the ignored notes file.

## Safety Profile

`docs-only`. This updates committed documentation only. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, write acceptance notes, write Restore Manifests, or create cleanup history.

## Problem

The refreshed pending package acceptance notes at `.local\release-acceptance\release-acceptance-20260604-164509.md` were generated when the repository was at `1aa5cf1`. Later green docs/tooling commits advanced `HEAD`, so wording that described `1aa5cf1` as current became stale even though the notes path, package provenance, and human-acceptance boundary remained correct.

## Changes

- Compact current-state and handoff docs now describe refreshed pending notes as generation-time provenance.
- The portable release runbook and package feature briefs now say to use package acceptance summary status lines for live notes/current-HEAD and package/current-HEAD context.
- The historical pending-notes refresh brief now says `1aa5cf1` was the then-current `HEAD` at the follow-up refresh.
- The docs/workflow packet breadcrumb now points to this wording stabilization packet.

## Verification

- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `git diff --check`

## ADRs

No ADR added. This is documentation wording for existing package acceptance evidence under ADR 0020 and does not change package creation, package acceptance, cleanup, restore, persistence, deployment, or app behavior.

## Follow-Up

- Keep using `.local\release-acceptance\release-acceptance-20260604-164509.md` as the current pending candidate notes path until a newer pending notes file is intentionally generated or the human acceptance pass is completed.
- Keep using summary status lines rather than fixed prose for live notes/current-HEAD and package/current-HEAD comparison.
