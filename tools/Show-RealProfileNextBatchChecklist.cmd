@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Show-RealProfileNextBatchChecklist.ps1" %*
exit /b %ERRORLEVEL%
