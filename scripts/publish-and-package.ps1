<#
.SYNOPSIS
    Publica SER_Balanza_Interno, genera el instalador y lo sube como asset de un Release de
    GitHub (de ahi lo descargan la autoactualizacion de las instalaciones existentes y
    cualquier instalacion nueva).

.PARAMETER Version
    Version a publicar, ej. "1.0.1". Debe actualizarse tambien en el .csproj
    (<Version>, <AssemblyVersion>, <FileVersion>) antes de correr este script.

.PARAMETER IsccPath
    Ruta al compilador de Inno Setup (ISCC.exe). Por defecto busca la instalacion
    tipica en Program Files.

.PARAMETER Repo
    Repositorio de GitHub "owner/nombre" donde publicar el Release. Por defecto el
    configurado en appsettings.json (UpdateRepo).

.PARAMETER SkipRelease
    Si se indica, no intenta crear el Release (solo genera el instalador local en
    installer\output). Util si no tenes `gh` autenticado a mano y vas a subirlo manual.

.EXAMPLE
    .\scripts\publish-and-package.ps1 -Version 1.0.1
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Version,

    [string]$IsccPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe",

    [string]$Repo,

    [switch]$SkipRelease
)

$ErrorActionPreference = "Stop"

$RepoRoot = Split-Path -Parent $PSScriptRoot
$ProjectPath = Join-Path $RepoRoot "SER_Balanza_Interno.csproj"
$PublishDir = Join-Path $RepoRoot "bin\Release\net8.0-windows\win-x64\publish"
$InstallerScript = Join-Path $RepoRoot "installer\setup.iss"
$InstallerOutputDir = Join-Path $RepoRoot "installer\output"
$InstallerName = "SER_Balanza_Interno_Setup.exe"

if (-not $Repo) {
    $settingsPath = Join-Path $RepoRoot "appsettings.json"
    if (Test-Path $settingsPath) {
        $settings = Get-Content $settingsPath -Raw | ConvertFrom-Json
        $Repo = $settings.UpdateRepo
    }
}

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

if ($SkipRelease) {
    Write-Host "==> Listo. Instalador en '$BuiltInstallerPath' (no se subio Release, -SkipRelease)." -ForegroundColor Yellow
    exit 0
}

if (-not $Repo) {
    Write-Host "==> No se encontro UpdateRepo en appsettings.json ni se paso -Repo." -ForegroundColor Yellow
    Write-Host "    Instalador generado en '$BuiltInstallerPath'. Subilo manualmente como Release." -ForegroundColor Yellow
    exit 0
}

$ghCommand = Get-Command gh -ErrorAction SilentlyContinue
if (-not $ghCommand) {
    Write-Host "==> GitHub CLI (gh) no esta instalado." -ForegroundColor Yellow
    Write-Host "    Instalador generado en '$BuiltInstallerPath'. Subilo manualmente a https://github.com/$Repo/releases/new" -ForegroundColor Yellow
    exit 0
}

$authStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "==> gh no esta autenticado (correr 'gh auth login')." -ForegroundColor Yellow
    Write-Host "    Instalador generado en '$BuiltInstallerPath'. Subilo manualmente a https://github.com/$Repo/releases/new" -ForegroundColor Yellow
    exit 0
}

Write-Host "==> Creando Release v$Version en $Repo..." -ForegroundColor Cyan
gh release create "v$Version" $BuiltInstallerPath --repo $Repo --title "v$Version" --notes "Instalador v$Version"

Write-Host "==> Listo. Version $Version publicada en https://github.com/$Repo/releases/tag/v$Version" -ForegroundColor Green
