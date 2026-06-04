# Verified Portable Package Candidate

Date: 2026-06-04

Status: completed

## Goal

Cut and verify a portable package candidate from the latest app-code commit after the real-profile Quarantine inline status wording fix, without promoting it to the accepted package baseline before a human acceptance pass.

## Safety Profile

`terminal-readonly` for package verification and acceptance-notes summarization. `Publish-LocalRelease.cmd` wrote ignored `.local\releases` package artifacts only and ran MVP preflight before publishing. No WPF launch, real-profile scan, movement, restore, deletion, approval, installed shortcut, installer behavior, or cleanup history was performed.

## Package Candidate

- Package: `.local\releases\windows-file-cleaner-v20260604-121922`
- Package zip: `.local\releases\windows-file-cleaner-v20260604-121922.zip`
- Package commit: `e6ac3eb467bffb4e41bd5697da702f649292315d`
- Executable SHA-256: `0E733191EF0B52742F35BAC5C86B34CE7075731DFB4A849EDD9E0EAA330025C1`
- Initial ignored acceptance notes: `.local\release-acceptance\release-acceptance-20260604-122009.md`
- Current pending acceptance notes: `.local\release-acceptance\release-acceptance-20260604-164509.md`

The initial ignored acceptance notes are not complete: verifier and commit evidence are recorded, but normal launch, fixture launch, overall result, and the remaining checklist items are not recorded. Refreshed pending notes were later generated after docs/tooling `HEAD` advanced beyond the app package commit; the current notes stamp exact `-ReleasePath` commands, record verifier evidence from their generation time, and require explicit `-RecordCommitMismatch` if the human accepts the expected package/current-HEAD mismatch. Later docs/tooling commits can make notes/current-HEAD status differ again; use package acceptance summary status lines for live mismatch context. The current accepted package baseline remains `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869` until a human package acceptance pass is completed and recorded.

## Verification

- `cmd.exe /c tools\Publish-LocalRelease.cmd`
- `cmd.exe /c tools\Test-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -RequireCurrentCommit`
- `cmd.exe /c tools\Start-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-122009.md"`

## ADRs

No ADR added. This follows ADR 0020: portable packages remain the v1 daily path, accepted package launch commands remain preferred until a fresh package is human-accepted, and installed shortcut or installer automation stays deferred.

## Follow-Up

- If the user wants this package promoted, run the package acceptance pass from `docs/operations/portable-release.md`, then record completion against `.local\release-acceptance\release-acceptance-20260604-164509.md` with `tools\Record-LocalReleaseAcceptanceNotes.cmd -RecordManualAcceptance -RecordCommitMismatch` only after intentionally accepting the expected package/current-HEAD mismatch.
- After this docs packet is committed, the package commit will intentionally be behind the docs-only closeout commit. Use package metadata and acceptance notes for package provenance rather than treating the package as built from that later docs-only commit.
