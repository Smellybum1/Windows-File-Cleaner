# Manual Fixture Review Runbook

Last updated: 2026-06-04

Use the tool output as the checklist source of truth:

```powershell
.\tools\Start-MvpFixtureReview.cmd -ChecklistOnly
```

After full MVP preflight has passed, the visible fixture pass can be started by the human user:

```powershell
.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes
```

Safety profile: `fixture-only` from `docs/codex/safety-profiles.md`.

Fixture roots for `New-StorageScanSmokeFixture.cmd` and `Start-MvpFixtureReview.cmd` must stay under ignored `.local`. Explicit roots outside `.local` fail before fixture writes, checklist output, or WPF launch.

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

Explicit fixture acceptance notes paths must stay under ignored `.local`.

Strict completion check:

```powershell
.\tools\Summarize-FixtureAcceptanceNotes.cmd -RequireComplete
```

When `-RequireComplete` passes, the summary reports that fixture acceptance evidence is complete and that no recorder action is pending.

Targeted fixture acceptance notes regression:

```powershell
.\tools\Test-FixtureAcceptanceNotes.cmd
```

This writes temporary ignored notes under `.local\fixture-acceptance-notes-test`, verifies summary and recorder behavior, then removes the test notes.

Targeted fixture root path guard regression:

```powershell
.\tools\Test-FixtureRootPathGuard.cmd
```

This verifies `New-StorageScanSmokeFixture.cmd` and `Start-MvpFixtureReview.cmd` reject explicit non-`.local` fixture roots before fixture writes, checklist output, or WPF launch.

Targeted daily readiness fixture acceptance notes regression:

```powershell
.\tools\Test-DailyReadinessFixtureAcceptanceNotes.cmd
```

This writes temporary ignored synthetic package files, package acceptance notes, fixture acceptance notes, and an empty Restore Manifest root under `.local\daily-readiness-fixture-acceptance-test`, verifies optional and strict daily readiness fixture-note forwarding, verifies explicit non-`.local` fixture notes paths stop before accepted launch-command printing, then removes the test folder.

MVP preflight runs the fixture root path guard regression before fixture dry-run output, then runs both fixture acceptance regressions by default after the fixture checklist. Use `.\tools\Invoke-MvpPreflight.cmd -SkipFixtureRootPathGuardCheck`, `.\tools\Invoke-MvpPreflight.cmd -SkipFixtureAcceptanceNotesCheck`, or `.\tools\Invoke-MvpPreflight.cmd -SkipDailyReadinessFixtureAcceptanceCheck` only for focused local loops where those fixture guard paths are not in scope.

Record an all-pass manual fixture review only after the visible review actually passed:

```powershell
.\tools\Record-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md" -RecordManualAcceptance
```
