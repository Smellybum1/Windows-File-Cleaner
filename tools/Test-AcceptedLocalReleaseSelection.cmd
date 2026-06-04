@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-AcceptedLocalReleaseSelection.ps1" %*
exit /b %ERRORLEVEL%
