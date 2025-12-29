param (
    [string]$modName
)

Write-Host ('Copying server files for {0}...' -f $modName)

Set-Location $PSScriptRoot

$destinationAbsolute = Join-Path $PSScriptRoot ('..\..\..\SPT\user\mods\{0}-Server\' -f $modName)

$serverLibraryAbsolute = Join-Path $PSScriptRoot ('bin\Debug\{0}-Server\{0}-Server.dll' -f $modName)

Copy-Item -Path $serverLibraryAbsolute -Destination $destinationAbsolute | Out-Null

Write-Host ('Copying server files for {0}...done.' -f $modName)