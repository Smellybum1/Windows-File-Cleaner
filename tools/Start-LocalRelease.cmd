@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Start-LocalRelease.ps1" %*
exit /b %ERRORLEVEL%
