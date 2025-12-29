param (
    [string]$modName
)

Write-Host ('Copying client files for {0}...' -f $modName)

Set-Location $PSScriptRoot

$destinationAbsolute = Join-Path $PSScriptRoot ('..\..\..\BepInEx\plugins\{0}\' -f $modName)

$clientLibraryAbsolute = Join-Path $PSScriptRoot ('bin\Debug\netstandard2.1\{0}-Client.dll' -f $modName)

Copy-Item -Path $clientLibraryAbsolute -Destination $destinationAbsolute | Out-Null

Write-Host ('Copying client files for {0}...done.' -f $modName)