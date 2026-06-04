@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-LocalReleaseAcceptanceCommandStamping.ps1" %*
exit /b %ERRORLEVEL%
