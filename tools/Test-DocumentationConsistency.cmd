@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-DocumentationConsistency.ps1" %*
exit /b %ERRORLEVEL%
