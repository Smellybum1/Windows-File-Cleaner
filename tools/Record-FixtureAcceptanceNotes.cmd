@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Record-FixtureAcceptanceNotes.ps1" %*
exit /b %ERRORLEVEL%
