@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Record-LocalReleaseAcceptanceNotes.ps1" %*
exit /b %ERRORLEVEL%
