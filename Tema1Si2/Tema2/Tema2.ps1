$ErrorActionPreference = 'Stop'

Write-Host 'DLL-uri folosite de mai multe procese:' -ForegroundColor Cyan

$moduleToProcesses = @{}

Get-Process | ForEach-Object {
    $process = $_

    try
    {
        $modules = $process.Modules
    }
    catch
    {
        return
    }

    foreach ($module in $modules)
    {
        $dllPath = $module.FileName

        if ([string]::IsNullOrWhiteSpace($dllPath))
        {
            continue
        }

        if (-not $moduleToProcesses.ContainsKey($dllPath))
        {
            $moduleToProcesses[$dllPath] = New-Object System.Collections.Generic.HashSet[string]
        }

        [void]$moduleToProcesses[$dllPath].Add($process.ProcessName)
    }
}

$dlluriMultiple = $moduleToProcesses.GetEnumerator() |
    Where-Object { $_.Value.Count -gt 1 } |
    Sort-Object { $_.Value.Count } -Descending

if (-not $dlluriMultiple)
{
    Write-Host 'Nu au fost gasite DLL-uri partajate de mai multe procese.' -ForegroundColor Yellow
    exit 0
}

foreach ($entry in $dlluriMultiple)
{
    [pscustomobject]@{
        Dll           = $entry.Key
        Procese        = ($entry.Value | Sort-Object) -join ', '
        NumarProcese   = $entry.Value.Count
    }
}
