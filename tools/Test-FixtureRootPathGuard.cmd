@echo off
setlocal

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-FixtureRootPathGuard.ps1" %*
exit /b %ERRORLEVEL%
