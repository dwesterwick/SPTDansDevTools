param (
    [string]$modName,
    [string]$modVersion
)

# Make sure our CWD is where the script lives
Set-Location $PSScriptRoot

Write-Host ('Packaging {0} v{1}' -f $modName, $modVersion)