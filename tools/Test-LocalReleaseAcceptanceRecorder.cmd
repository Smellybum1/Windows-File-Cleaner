@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-LocalReleaseAcceptanceRecorder.ps1" %*
exit /b %ERRORLEVEL%
