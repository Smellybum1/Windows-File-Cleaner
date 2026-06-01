@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Summarize-FixtureAcceptanceNotes.ps1" %*
exit /b %ERRORLEVEL%
