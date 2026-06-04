@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-DailyReadinessExactProfileUndoSpotlight.ps1" %*
exit /b %ERRORLEVEL%
