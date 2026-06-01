@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Invoke-RealProfileQuarantineReadiness.ps1" %*
exit /b %ERRORLEVEL%
