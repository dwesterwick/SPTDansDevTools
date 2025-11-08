param (
    [string]$modVersion
)

Set-Location $PSScriptRoot

$modInfoRelativePath = "ModInfo.cs"
$modInfoAbsolutePath = Join-Path $PSScriptRoot $modInfoRelativePath

Write-Host ('Updating {0}...' -f $modInfoAbsolutePath)

# Read the original file contents
$originalContent = Get-Content -Path $modInfoAbsolutePath -Raw

# Update property values
$updatedContent = $originalContent -replace 'MODVERSION = ".*"' , ('MODVERSION = "{0}"' -f $modVersion)

# Write modified contents back to the file
$updatedContent | Out-File -FilePath $modInfoAbsolutePath

Write-Host ('Updating {0}...done.' -f $modInfoAbsolutePath)