@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-LocalReleasePathGuards.ps1" %*
exit /b %ERRORLEVEL%
