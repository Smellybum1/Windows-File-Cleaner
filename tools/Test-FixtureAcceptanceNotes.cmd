@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-FixtureAcceptanceNotes.ps1" %*
exit /b %ERRORLEVEL%
