@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Summarize-RestoreManifests.ps1" %*
exit /b %ERRORLEVEL%
