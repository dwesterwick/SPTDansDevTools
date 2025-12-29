param (
    [string]$guid,
    [string]$modName,
    [string]$author,
    [string]$modVersion,
    [string]$sptVersion
)

Set-Location $PSScriptRoot

$modInfoRelativePath = "ModInfo.cs"
$modInfoAbsolutePath = Join-Path $PSScriptRoot $modInfoRelativePath

Write-Host ('Updating {0}...' -f $modInfoAbsolutePath)

# Read the original file contents
$originalContent = Get-Content -Path $modInfoAbsolutePath -Raw
$updatedContent = $originalContent

# Update property values
Write-Host ('Updating {0}...setting properties: GUID={1}, MODNAME={2}, AUTHOR={3}, MODVERSION={4}, SPTVERSIONCOMPATIBILITY={5}.' -f $modInfoAbsolutePath, $guid, $modName, $author, $modVersion, $sptVersion)
$updatedContent = $updatedContent -replace 'GUID = ".*"' , ('GUID = "{0}"' -f $guid)
$updatedContent = $updatedContent -replace 'MODNAME = ".*"' , ('MODNAME = "{0}"' -f $modName)
$updatedContent = $updatedContent -replace 'AUTHOR = ".*"' , ('AUTHOR = "{0}"' -f $author)
$updatedContent = $updatedContent -replace 'MODVERSION = ".*"' , ('MODVERSION = "{0}"' -f $modVersion)
$updatedContent = $updatedContent -replace 'SPTVERSIONCOMPATIBILITY = ".*"' , ('SPTVERSIONCOMPATIBILITY = "{0}"' -f $sptVersion)

# Write modified contents back to the file
$updatedContent | Out-File -FilePath $modInfoAbsolutePath

Write-Host ('Updating {0}...done.' -f $modInfoAbsolutePath)