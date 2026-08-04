[CmdletBinding()]
param(
    [int]$Port = 5077,
    [switch]$Restart
)

$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$serverDirectory = Join-Path $projectRoot 'Builds\Server'
$serverExecutable = Join-Path $serverDirectory 'Dqq.MatchServer.exe'
$firewallRuleName = 'DQQ Match Server (TCP 5077)'

if (-not (Test-Path -LiteralPath $serverExecutable -PathType Leaf)) {
    throw "Match server executable was not found: $serverExecutable"
}

$lanAddress = Get-NetIPConfiguration |
    Where-Object { $_.IPv4DefaultGateway -and $_.IPv4Address } |
    ForEach-Object { $_.IPv4Address.IPAddress } |
    Where-Object { $_ -and -not $_.StartsWith('169.254.') } |
    Select-Object -First 1

if (-not $lanAddress) {
    throw 'No active LAN IPv4 address with a default gateway was found.'
}

$serverPath = [System.IO.Path]::GetFullPath($serverExecutable)
$runningServers = Get-CimInstance Win32_Process -Filter "Name = 'Dqq.MatchServer.exe'" |
    Where-Object { $_.ExecutablePath -and [System.IO.Path]::GetFullPath($_.ExecutablePath) -eq $serverPath }

if ($Restart) {
    foreach ($server in $runningServers) {
        Stop-Process -Id $server.ProcessId -Force
    }
    $runningServers = @()
}

if (-not $runningServers) {
    $process = Start-Process -FilePath $serverExecutable -WorkingDirectory $serverDirectory `
        -WindowStyle Hidden -PassThru
    Write-Host "Started DQQ match server (PID $($process.Id))."
}
else {
    Write-Host "DQQ match server is already running (PID $($runningServers[0].ProcessId))."
}

$isAdministrator = ([Security.Principal.WindowsPrincipal] `
        [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole(
        [Security.Principal.WindowsBuiltInRole]::Administrator)

try {
    $existingRule = Get-NetFirewallRule -DisplayName $firewallRuleName -ErrorAction SilentlyContinue
}
catch {
    $existingRule = $null
}

if ($existingRule) {
    Write-Host 'The local-subnet firewall rule is already configured.'
}
elseif ($isAdministrator) {
    New-NetFirewallRule -DisplayName $firewallRuleName -Direction Inbound -Action Allow `
        -Protocol TCP -LocalPort $Port -Program $serverExecutable -RemoteAddress LocalSubnet `
        -Profile Any | Out-Null
    Write-Host 'Created a firewall rule limited to the local subnet.'
}
else {
    Write-Warning "Firewall setup needs an elevated PowerShell. Run this script once as administrator."
}

$healthUrl = "http://${lanAddress}:$Port/health"
$ready = $false
for ($attempt = 0; $attempt -lt 20; $attempt++) {
    try {
        $health = Invoke-RestMethod -Uri $healthUrl -TimeoutSec 1
        if ($health.status -eq 'ok') {
            $ready = $true
            break
        }
    }
    catch {
        Start-Sleep -Milliseconds 250
    }
}

if (-not $ready) {
    throw "The server did not become reachable at $healthUrl. Check whether TCP port $Port is already in use."
}

Write-Host "DQQ LAN match server is ready: http://${lanAddress}:$Port"
Write-Host "Client health check: $healthUrl"
