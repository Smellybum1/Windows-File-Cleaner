# Feature: WPF Real-Profile Approval Evidence Output

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Surface the core Real-Profile Quarantine Approval Evidence in the existing WPF Quarantine Execution Gate so the visible app explains why exact `QUARANTINE` is still not enough for real-profile/custom movement.

## Non-goals

- Do not enable real-profile WPF Quarantine execution.
- Do not add a new WPF panel or control.
- Do not change fixture-only Quarantine execution.
- Do not scan, move, restore, delete, create, write, or clean up real-profile files.

## Implementation

- WPF now builds `RealProfileQuarantineApprovalEvidence` when the Quarantine Execution Gate refreshes for non-fixture scopes.
- Fixture scopes skip the real-profile approval-evidence block to keep the fixture path focused.
- The gate output shows exact confirmation match, exact real-profile scope evidence, readiness blocker presence, current-build movement availability, and whether movement can be approved.
- The boundary line says exact `QUARANTINE` is necessary but not sufficient and that the evidence does not create folders, move files, restore files, delete files, write manifests, persist approval, or approve cleanup.

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added WPF gate output for read-only approval evidence on custom, exact synthetic real-profile, and synthetic real-profile-child scopes.
- Added WPF smoke coverage that exact `QUARANTINE` is shown as matched while `Can approve real-profile movement` remains `no`.
- Added WPF smoke coverage that fixture gate output does not show this real-profile-only evidence.
- Later checklist-alignment packet updated fixture review prompts and README manual checks to ask reviewers to look for this visible approval evidence.

ADRs:

- No ADR added. This is a reversible WPF visibility packet under accepted ADR 0018 and does not change execution availability.

Follow-up work:

- Continue visible readiness polish or manual fixture review before any real-profile execution packet.

Open questions:

- None for this output packet.

Risky assumptions:

- Gate text is the right first WPF surface because it refreshes as the user types confirmation and does not require another control.
