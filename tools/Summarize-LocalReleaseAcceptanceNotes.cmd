@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Summarize-LocalReleaseAcceptanceNotes.ps1" %*
exit /b %ERRORLEVEL%
