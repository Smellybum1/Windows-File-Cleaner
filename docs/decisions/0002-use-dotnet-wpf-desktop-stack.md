# ADR 0002: Use .NET WPF desktop stack

Date: 2026-05-28  
Status: accepted  
Owner: project-owner

## Context

This project is a Windows-only local cleanup app for inspecting and eventually cleaning `C:\Users\moxhe`.

The app needs:

- Direct local filesystem access.
- A desktop UI for reviewing storage usage and cleanup recommendations.
- A conservative safety model before any destructive cleanup behavior.
- A low-friction local development path on the project owner's Windows machine.

The local machine currently has these relevant SDKs and runtimes installed:

- .NET SDK 8.0.421
- .NET SDK 9.0.314
- Microsoft.WindowsDesktop.App runtimes for .NET 8 and .NET 9

Microsoft's .NET support policy currently lists .NET 10 as active LTS through November 14, 2028, while .NET 8 and .NET 9 are both supported through November 10, 2026. Microsoft Learn describes WPF as a Windows desktop UI framework for .NET.

Sources:

- https://dotnet.microsoft.com/en-us/platform/support/policy
- https://learn.microsoft.com/en-us/dotnet/desktop/wpf/

## Decision

Use C# with WPF for the Windows File Cleaner desktop app.

Initial implementation should target `.NET 8` so the project can build with the SDK and Windows Desktop runtime already installed locally.

Revisit targeting `.NET 10` after the .NET 10 SDK is installed, especially before investing in packaging or long-term distribution.

## Options considered

### Option A: C# WPF on .NET 8

Pros:

- Windows-native desktop app model.
- Direct filesystem APIs through .NET.
- Fits the single-user Windows 11 app target.
- SDK and Windows Desktop runtime are already installed locally.
- Lower complexity than Electron, Tauri, or a local web app.

Cons:

- Windows-only.
- .NET 8 has a shorter remaining support horizon than .NET 10.
- WPF UI styling is more manual than many web UI stacks.

### Option B: C# WPF on .NET 10

Pros:

- Longer LTS support horizon.
- Same Windows-native WPF fit.

Cons:

- .NET 10 SDK is not currently installed locally.
- Adds setup work before the first implementation can build.

### Option C: WinUI 3

Pros:

- Modern Windows UI framework.
- Native Windows look and feel.

Cons:

- More packaging and tooling complexity for a local MVP.
- Higher friction than WPF for quick filesystem-heavy utility work.

### Option D: Electron or Tauri

Pros:

- Web UI tooling and modern UI ecosystem.
- Tauri can be relatively lightweight.

Cons:

- Adds JavaScript/TypeScript and native bridge complexity.
- More moving pieces for local filesystem safety.
- Less direct than WPF for a Windows-only single-user utility.

### Option E: Command-line tool only

Pros:

- Fastest to implement.
- Easy to test.

Cons:

- User prefers a desktop app.
- Harder to inspect and compare cleanup candidates comfortably.
- Less suitable for future quarantine/undo review flows.

## Why this decision

WPF with C# is the most direct match for a Windows-only cleanup app that needs filesystem access, a local review UI, and low setup friction.

Targeting .NET 8 initially keeps the first implementation buildable with the installed SDK. The project can move to .NET 10 later when the SDK is installed and the app is ready for longer-term packaging.

## Consequences

Positive consequences:

- The project can start without installing new toolchains.
- Filesystem scanning and path handling can use mature .NET APIs.
- The desktop UI can remain local-only and avoid browser/server architecture.
- Automated tests can cover scanner and recommendation logic separately from WPF UI.

Negative consequences:

- The app is Windows-only.
- A future .NET 10 upgrade is likely.
- WPF UI work requires care to feel modern and dense enough for storage review.

## Reversal cost

Medium.

Scanner and recommendation logic can be kept in a non-UI library to reduce reversal cost. Replacing WPF later would still require rebuilding the UI and app shell.

## Follow-up work

- Scaffold a WPF app project and a testable core library.
- Keep scanner, rule, and recommendation logic out of WPF code-behind.
- Add project commands to `AGENTS.md`.
- Consider .NET 10 upgrade after installing the SDK.

## Supersedes

- None.

## Superseded by

- None.

