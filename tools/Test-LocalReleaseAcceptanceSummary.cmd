@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-LocalReleaseAcceptanceSummary.ps1" %*
exit /b %ERRORLEVEL%
