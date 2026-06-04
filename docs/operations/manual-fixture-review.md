# Manual Fixture Review Runbook

Last updated: 2026-06-03

Use the tool output as the checklist source of truth:

```powershell
.\tools\Start-MvpFixtureReview.cmd -ChecklistOnly
```

After full MVP preflight has passed, the visible fixture pass can be started by the human user:

```powershell
.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes
```

Safety profile: `fixture-only` from `docs/codex/safety-profiles.md`.

## Review Areas

The checklist covers:

- Scan header and gate.
- Safety Summary, Review, and Main Grid.
- Quarantine Preview and fixture Quarantine.
- Restore Manifest review and selected restore.
- Real-profile and custom blockers.

## Notes

- The fixture pass must not scan `C:\Users\moxhe`.
- The real-profile/custom blocker check can use custom preview-only paths or existing synthetic readiness evidence.
- The notes recorder is only for after an actual all-pass visible fixture review.
- Fill notes manually instead when there were issues or not-checked items.

## Formal Notes Commands

Summarize latest fixture acceptance notes:

```powershell
.\tools\Summarize-FixtureAcceptanceNotes.cmd
```

Strict completion check:

```powershell
.\tools\Summarize-FixtureAcceptanceNotes.cmd -RequireComplete
```

Targeted fixture acceptance notes regression:

```powershell
.\tools\Test-FixtureAcceptanceNotes.cmd
```

This writes temporary ignored notes under `.local\fixture-acceptance-notes-test`, verifies summary and recorder behavior, then removes the test notes.

MVP preflight runs this regression by default after the fixture checklist. Use `.\tools\Invoke-MvpPreflight.cmd -SkipFixtureAcceptanceNotesCheck` only for focused local loops where fixture acceptance notes are not in scope.

Record an all-pass manual fixture review only after the visible review actually passed:

```powershell
.\tools\Record-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md" -RecordManualAcceptance
```
