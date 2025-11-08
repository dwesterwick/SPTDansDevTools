param (
    [string]$modVersion
)

Set-Location $PSScriptRoot

$modInfoRelativePath = "ModInfo.cs"
$modInfoAbsolutePath = Join-Path $PSScriptRoot $modInfoRelativePath

Write-Host ('Updating {0}...' -f $modInfoAbsolutePath)

$originalContent = Get-Content -Path $modInfoAbsolutePath -Raw

$updatedContent = $originalContent -replace 'MODVERSION = ".*"' , ('MODVERSION = "{0}"' -f $modVersion)

$updatedContent | Out-File -FilePath $modInfoAbsolutePath

Write-Host ('Updating {0}...done.' -f $modInfoAbsolutePath)