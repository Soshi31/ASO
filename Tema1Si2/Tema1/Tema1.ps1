$ErrorActionPreference = 'Stop'

Write-Host 'Servicii care ruleaza in acest moment:' -ForegroundColor Cyan

$servicii = Get-CimInstance Win32_Service |
    Where-Object { $_.State -eq 'Running' } |
    Sort-Object DisplayName

if (-not $servicii)
{
    Write-Host 'Nu au fost gasite servicii in starea Running.' -ForegroundColor Yellow
    exit 0
}

foreach ($serviciu in $servicii)
{
    [pscustomobject]@{
        NumeServiciu = $serviciu.Name
        Afisare      = $serviciu.DisplayName
        Stare        = $serviciu.State
        StartMode    = $serviciu.StartMode
        Path         = $serviciu.PathName
    }
}
