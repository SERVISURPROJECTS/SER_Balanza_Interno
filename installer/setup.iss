; Script de Inno Setup para SER_Balanza_Interno.
; Compilar con: ISCC.exe installer\setup.iss /DMyAppVersion=1.0.0 /DMyPublishDir=..\bin\Release\net8.0-windows\win-x64\publish
;
; MyAppVersion y MyPublishDir se pasan como parametros desde scripts\publish-and-package.ps1.
; Si se compila manualmente sin pasarlos, se usan los valores por defecto de abajo.

#ifndef MyAppVersion
  #define MyAppVersion "1.0.0"
#endif

#ifndef MyPublishDir
  #define MyPublishDir "..\bin\Release\net8.0-windows\win-x64\publish"
#endif

#define MyAppName "SER Balanza Interno"
#define MyAppExeName "SER_Balanza_Interno.exe"
#define MyAppPublisher "Servisur Agricola"

[Setup]
AppId={{B5C2B8B1-6E1A-4E9F-9A9C-2E7B2B9B9A6A}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={userpf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=output
OutputBaseFilename=SER_Balanza_Interno_Setup
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64compatible
CloseApplications=yes
RestartApplications=no
; Instalacion por usuario (sin permisos de administrador): la app ya guarda su base de datos y
; config en %LocalAppData%, no necesita Program Files. Esto evita el prompt de UAC, que un usuario
; sin cuenta de administrador no puede aprobar.
PrivilegesRequired=lowest

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Files]
Source: "{#MyPublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Abrir {#MyAppName}"; Flags: nowait postinstall skipifsilent
