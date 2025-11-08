param (
    [string]$modName,
    [string]$modVersion
)

Write-Host ('Packaging {0} v{1}' -f $modName, $modVersion)

# Make sure our CWD is where the script lives
Set-Location $PSScriptRoot

