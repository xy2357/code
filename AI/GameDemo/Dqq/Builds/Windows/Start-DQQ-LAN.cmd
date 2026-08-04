@echo off
setlocal
cd /d "%~dp0"

powershell.exe -NoProfile -Command "try { $result = Invoke-RestMethod 'http://10.27.238.57:5077/health' -TimeoutSec 3; if ($result.status -ne 'ok') { exit 1 } } catch { exit 1 }"
if errorlevel 1 (
    echo Cannot connect to DQQ server: http://10.27.238.57:5077
    echo Make sure this computer is on the same LAN as the server.
    pause
    exit /b 1
)

start "" "%~dp0Dqq.exe" --server-url=http://10.27.238.57:5077
endlocal
