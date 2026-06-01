@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Publish-LocalRelease.ps1" %*
exit /b %ERRORLEVEL%
