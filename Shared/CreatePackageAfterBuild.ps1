param (
    [string]$modName,
    [string]$modVersion
)

# Set path to 7-Zip executable
$pathTo7z = "C:\Program Files\7-Zip\7z.exe"

Write-Host ('Packaging {0} v{1}...' -f $modName, $modVersion)

Set-Location $PSScriptRoot

# Create the build folder
$packageFolderAbsolute = Join-Path $PSScriptRoot "..\Dist"

New-Item -ItemType Directory -Path $packageFolderAbsolute -Force | Out-Null
Remove-Item -Path ('{0}\*' -f $packageFolderAbsolute) -Recurse -Force | Out-Null

# Create server and client folders
$serverFolderAbsolute = Join-Path $packageFolderAbsolute ('user\mods\{0}' -f $modName)
$clientFolderAbsolute = Join-Path $packageFolderAbsolute ('BepInEx\plugins\{0}' -f $modName)

New-Item -ItemType Directory -Path $serverFolderAbsolute -Force | Out-Null
New-Item -ItemType Directory -Path $clientFolderAbsolute -Force | Out-Null

# Copy all files

Write-Host ('Packaging {0} v{1}...copying files...' -f $modName, $modVersion)

$configFileAbsolute = Join-Path $PSScriptRoot 'Config\config.json'
$serverLibraryAbsolute = Join-Path $PSScriptRoot ('..\Server\bin\Debug\{0}-Server\{0}-Server.dll' -f $modName)
$clientLibraryAbsolute = Join-Path $PSScriptRoot ('..\Client\bin\Debug\netstandard2.1\{0}-Client.dll' -f $modName)

Copy-Item -Path $configFileAbsolute -Destination $serverFolderAbsolute | Out-Null
Copy-Item -Path $serverLibraryAbsolute -Destination $serverFolderAbsolute | Out-Null

Copy-Item -Path $clientLibraryAbsolute -Destination $clientFolderAbsolute | Out-Null

# Create 7zip archive

Write-Host ('Packaging {0} v{1}...creating archive...' -f $modName, $modVersion)

$archiveName = Join-Path $packageFolderAbsolute ('{0}-{1}.7z' -f $modName, $modVersion)
$sourceFiles = Join-Path $packageFolderAbsolute '*'
$arguments = "a", "-t7z", $archiveName, $sourceFiles

& $pathTo7z $arguments | Out-Null

Write-Host ('Packaging {0} v{1}...done.' -f $modName, $modVersion)