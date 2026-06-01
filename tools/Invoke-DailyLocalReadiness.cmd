@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Invoke-DailyLocalReadiness.ps1" %*
exit /b %ERRORLEVEL%
