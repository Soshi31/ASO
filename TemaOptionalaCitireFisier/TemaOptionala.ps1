$ErrorActionPreference = 'Stop'

$relativePath = [Console]::In.ReadLine()

if ([string]::IsNullOrWhiteSpace($relativePath))
{
    Write-Host 'Eroare: nu a fost furnizat niciun path.' -ForegroundColor Red
    exit 1
}

if (-not (Test-Path -LiteralPath $relativePath))
{
    Write-Host 'Eroare: fisierul nu exista.' -ForegroundColor Red
    exit 1
}

Get-Content -LiteralPath $relativePath