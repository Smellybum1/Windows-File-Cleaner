@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-DailyReadinessFixtureAcceptanceNotes.ps1" %*
exit /b %ERRORLEVEL%
