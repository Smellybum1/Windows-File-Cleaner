# ADR 0020: Defer Installed Shortcut and Installer

Date: 2026-06-02
Status: accepted
Owner: project-owner

## Context

Portable v1 now has a self-contained local package, package-local launch scripts, package verification, ignored acceptance notes, an accepted-package launcher, and a daily local readiness wrapper. Those tools make daily use possible without installing anything.

The remaining deployment temptation is to add a Desktop shortcut, Start Menu shortcut, or installer so the app feels more like a normal Windows app. That is plausible later, but doing it now would create user-profile or system-level artifacts while the accepted package baseline can already be launched through verified commands. A local debug-build shortcut may also exist, but it is development-only context and must not become accepted-package evidence.

Constraints:

- Storage Scan remains read-only unless the user explicitly asks for a gated cleanup action.
- Accepted portable package evidence is local ignored evidence, not app persistence or cleanup history.
- Installed shortcuts and installers write outside the portable package folder and need explicit user approval before creation.
- Any future shortcut or installer must not widen cleanup behavior, add permanent deletion, add broad/all-manifest restore, add custom real-profile Quarantine, or add persisted cleanup history.

## Decision

Keep the accepted Portable Release Package launch commands as the daily path for now.

Do not create installed Desktop shortcuts, Start Menu shortcuts, installer artifacts, services, scheduled tasks, or background update/install behavior in the current v1 path.

Any future installed shortcut or installer must be a separate explicit user-approved packaging packet. That packet must define:

- whether the target is an accepted Portable Release Package, a release-local launch script, a repo-level accepted launcher, or a future installer-managed location,
- how the target avoids debug-build paths as daily-use evidence,
- how stale accepted-package evidence and package/current-HEAD mismatches are shown,
- what files or user-profile artifacts are created, updated, or removed,
- how uninstall or cleanup of those installed artifacts works,
- which read-only verification command proves the shortcut or installer target without launching WPF, scanning, moving, restoring, deleting, approving cleanup, or creating cleanup history.

Until that packet exists, the recommended daily workflow remains:

- use `tools\Invoke-DailyLocalReadiness.cmd` for read-only daily evidence,
- use `tools\Start-AcceptedLocalRelease.cmd -PrintOnly` or `tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly` to print accepted package launch commands,
- remove `-PrintOnly` only when the user intentionally wants to launch the accepted package.

## Options considered

### Option A: Keep accepted package launch commands as the daily path

Pros:

- Uses already accepted package evidence.
- Avoids writing installed artifacts outside the repo and ignored package folders.
- Keeps package/current-HEAD warning behavior visible and expected.
- Keeps deployment separate from cleanup movement gates.

Cons:

- Daily launch still involves terminal commands.
- The app feels less installed than a normal Windows desktop app.

### Option B: Add an installed shortcut now

Pros:

- Faster daily launch.
- Familiar Desktop or Start Menu workflow.

Cons:

- Creates user-profile artifacts before shortcut target, update, stale package, and removal behavior are designed.
- Could accidentally point daily use at a debug build or stale package.
- Would need user approval because it writes outside the portable package folder.

### Option C: Add an installer now

Pros:

- Most familiar Windows distribution shape.
- Could later support updates and Start Menu integration.

Cons:

- Too heavy for a local reversible-only v1.
- Introduces installer state, upgrade/uninstall behavior, and trust questions before the cleanup workflow needs them.
- Risks mixing deployment persistence with cleanup history or app persistence.

## Why this decision

Option A keeps the live-product path moving while preserving the safety boundary. The app already has accepted-package evidence and launcher commands; installing artifacts is a deployment decision with its own lifecycle and rollback questions.

## Consequences

Positive consequences:

- Future threads have a clear answer when shortcut or installer automation comes up.
- Debug-build shortcuts stay explicitly development-only.
- Portable v1 remains reversible-only and local-first.

Negative consequences:

- Daily use remains command-driven until a later approved packaging packet.
- A later shortcut or installer packet still needs separate design and verification work.

## Reversal cost

Low to medium. A future ADR or packaging packet can supersede this decision by adding an installed shortcut or installer, but it must define target selection, stale-target handling, removal behavior, and verification before creating local installed artifacts.

## Follow-up work

- If daily terminal launch remains annoying, run a new Grill with Docs packet for an installed shortcut or installer.
- Keep accepted package verification and daily readiness print-only commands as the recommended daily path until then.
- Cut and accept a fresh package only when app behavior changes should ship as the next accepted package baseline.

## Supersedes

- None.

## Superseded by

- None.
