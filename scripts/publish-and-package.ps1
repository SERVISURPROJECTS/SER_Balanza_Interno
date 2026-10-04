<#
.SYNOPSIS
    Publica SER_Balanza_Interno, genera el instalador y lo copia a la ruta de red
    de la que los operadores remotos descargan la actualizacion.

.PARAMETER Version
    Version a publicar, ej. "1.0.1". Debe actualizarse tambien en el .csproj
    (<Version>, <AssemblyVersion>, <FileVersion>) antes de correr este script.

.PARAMETER IsccPath
    Ruta al compilador de Inno Setup (ISCC.exe). Por defecto busca la instalacion
    tipica en Program Files.

.PARAMETER SharePath
    Ruta de red donde se publica el instalador y version.json. Por defecto la
    ruta configurada en appsettings.json (UpdateSharePath).

.EXAMPLE
    .\scripts\publish-and-package.ps1 -Version 1.0.1
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Version,

    [string]$IsccPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe",

    [string]$SharePath = "\\svras02\Casillero General\Alison Tacoo\balanza"
)

$ErrorActionPreference = "Stop"

$RepoRoot = Split-Path -Parent $PSScriptRoot
$ProjectPath = Join-Path $RepoRoot "SER_Balanza_Interno.csproj"
$PublishDir = Join-Path $RepoRoot "bin\Release\net8.0-windows\win-x64\publish"
$InstallerScript = Join-Path $RepoRoot "installer\setup.iss"
$InstallerOutputDir = Join-Path $RepoRoot "installer\output"
$InstallerName = "SER_Balanza_Interno_Setup.exe"

Write-Host "==> Publicando version $Version (self-contained, win-x64)..." -ForegroundColor Cyan
dotnet publish $ProjectPath -c Release -r win-x64 --self-contained true `
    -p:Version=$Version -p:AssemblyVersion="$Version.0" -p:FileVersion="$Version.0"

if (-not (Test-Path $IsccPath)) {
    throw "No se encontro ISCC.exe en '$IsccPath'. Instala Inno Setup o pasa -IsccPath."
}

Write-Host "==> Generando instalador con Inno Setup..." -ForegroundColor Cyan
& $IsccPath $InstallerScript "/DMyAppVersion=$Version" "/DMyPublishDir=$PublishDir"

$BuiltInstallerPath = Join-Path $InstallerOutputDir $InstallerName
if (-not (Test-Path $BuiltInstallerPath)) {
    throw "No se genero el instalador esperado en '$BuiltInstallerPath'."
}

if (-not (Test-Path $SharePath)) {
    throw "No se puede acceder a la ruta de red '$SharePath'. Verifica VPN/permisos."
}

Write-Host "==> Copiando instalador a '$SharePath'..." -ForegroundColor Cyan
Copy-Item $BuiltInstallerPath -Destination (Join-Path $SharePath $InstallerName) -Force

$VersionInfo = @{
    version   = $Version
    installer = $InstallerName
} | ConvertTo-Json

$VersionInfo | Out-File -FilePath (Join-Path $SharePath "version.json") -Encoding utf8 -NoNewline

Write-Host "==> Listo. Version $Version publicada en '$SharePath'." -ForegroundColor Green
