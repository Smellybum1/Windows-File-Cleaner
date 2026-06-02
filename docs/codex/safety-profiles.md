# Safety Profiles

Last updated: 2026-06-03

Use these short profile names in feature briefs, progress notes, and handoff docs instead of repeating the full no-launch/no-scan/no-movement boilerplate every time.

## `docs-only`

Allowed:

- Edit committed documentation.
- Run text searches, markdown checks, and git diff checks.

Not allowed:

- Launch WPF.
- Scan real-profile files.
- Move, restore, delete, quarantine, approve cleanup, write Restore Manifests, create shortcuts, install anything, or create cleanup history.

## `terminal-readonly`

Allowed:

- Run repository scripts that explicitly state they are read-only or print-only.
- Read ignored local evidence such as release acceptance notes, fixture acceptance notes, package metadata, and Restore Manifests.
- Print launch commands without launching WPF.

Not allowed:

- Launch WPF.
- Click `Scan`.
- Scan `C:\Users\moxhe`.
- Move, restore, delete, quarantine, approve cleanup, write Restore Manifests, create shortcuts, install anything, or create cleanup history.

## `fixture-only`

Allowed:

- Create or use the synthetic fixture Cleanup Scope.
- Launch WPF against the fixture when the human intentionally starts that workflow.
- Execute fixture Quarantine, fixture undo, and fixture selected restore during manual fixture review.

Not allowed:

- Treat fixture success as real-profile movement approval.
- Scan or modify `C:\Users\moxhe`.
- Create permanent deletion, broad/all-manifest restore, custom real-profile movement, installed shortcuts, installer behavior, or cleanup history.

## `accepted-package-printonly`

Allowed:

- Verify accepted local release evidence.
- Print accepted normal or fixture launch commands.
- Surface the expected accepted-package/current-HEAD warning.

Not allowed:

- Launch WPF while `-PrintOnly` is present.
- Create shortcuts or install anything.
- Treat the accepted package as current-HEAD proof when later docs-only commits exist.

## `real-profile-user-click-only`

Allowed:

- Show terminal evidence and WPF readiness.
- Ask the human user to review a specific tiny exact `C:\Users\moxhe` batch when fresh evidence has passed.

Required before any user-clicked real-profile Quarantine:

- Exact Cleanup Scope `C:\Users\moxhe`.
- At most 10 rows and 1 GB.
- Likely safe plus Quarantine candidate only.
- No broad parents, protected/high-risk/no-category/access-issue rows, outside-scope evidence, reparse points, cloud sync data, credential data, source code, game saves, active app settings, or Codex/tooling state.
- Clean Quarantine Root Execution Safety.
- Selected real-profile restore trust.
- Exact `QUARANTINE`.
- Real-Profile Quarantine Approval Evidence.
- Immediate Pre-Execution Revalidation.
- Explicit human approval for the specific batch.

Not allowed:

- Codex or automated checks clicking real-profile movement.
- Broad/all-manifest real-profile Undo Quarantine.
- Custom/non-exact real-profile Quarantine.
- Permanent deletion.
- Persisted cleanup history.
