@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-RealProfileNextBatchStopGuard.ps1" %*
exit /b %ERRORLEVEL%
