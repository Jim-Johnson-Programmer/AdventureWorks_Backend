<#!
.SYNOPSIS
    Deletes Serilog log files for the API project, optionally by developer or all logs.

.PARAMETER LogDirectory
    Path to the folder containing log files. Defaults to ..\src\SalesOrders\AWMicroservices.SalesOrders.API\logs relative to the script location.

.PARAMETER Developer
    Optional developer name to filter log files (if log files are named per developer, e.g., log-devname-20260511.json). If omitted, deletes all log-*.json files.

.EXAMPLE
    ./Clear-Logs.ps1
    ./Clear-Logs.ps1 -Developer alice
    ./Clear-Logs.ps1 -LogDirectory "C:\Project\logs" -Developer bob
#>
param(
    [string]$LogDirectory = (Join-Path $PSScriptRoot "..\src\SalesOrders\AWMicroservices.SalesOrders.API\logs"),
    [string]$Developer = ""
)

$logPath = Resolve-Path $LogDirectory -ErrorAction SilentlyContinue
if (-not $logPath) {
    Write-Warning "Log directory not found: $LogDirectory"
    exit 0
}

if ($Developer -ne "") {
    $pattern = "log-*${Developer}*.json"
    Write-Host "Deleting logs for developer: $Developer"
} else {
    $pattern = "log-*.json"
    Write-Host "Deleting all log files"
}

$deleted = 0
Get-ChildItem -Path $logPath -Filter $pattern -File |
    ForEach-Object {
        Remove-Item $_.FullName -Force
        Write-Host "Deleted: $($_.Name)"
        $deleted++
    }

Write-Host "`nCleanup complete. $deleted file(s) removed."
