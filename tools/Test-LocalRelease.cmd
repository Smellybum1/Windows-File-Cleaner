@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-LocalRelease.ps1" %*
exit /b %ERRORLEVEL%
