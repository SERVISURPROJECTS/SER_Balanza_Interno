# 09 — Instalador

## Contexto

`installer/setup.iss` (Inno Setup 6) empaqueta el `dotnet publish` self-contained en un único
`.exe` de instalación. Distribuido normalmente vía el casillero de red (ver
[[08-autoactualizacion]]); cuando esa ruta no está disponible, también se sube manualmente como
asset de un GitHub Release para que un usuario externo lo descargue directo.

## Requisitos

1. La máquina destino **no necesita tener .NET 8 instalado** (self-contained) — la primera
   instalación manual de un operador nuevo no puede depender de instalar un runtime aparte.
2. La instalación **no debe requerir una cuenta de administrador** — un operador remoto puede no
   tener esas credenciales en su máquina y solo el admin de esa PC las tiene.
3. El instalador queda versionado igual que la app (mismo número de versión en ambos).

## Diseño actual

### Self-contained (`scripts/publish-and-package.ps1`)

`dotnet publish -r win-x64 --self-contained true`. Esto infla el instalador a ~55MB (incluye el
runtime de .NET completo) contra ~5MB de una build framework-dependent — se acepta ese costo a
cambio de no depender de que la máquina destino tenga el runtime correcto instalado. Requirió un
`NuGet.Config` local con `packageSourceMapping` adicional para los paquetes de runtime (ver
[[decisiones/0009-nuget-config-local-self-contained]]).

### Sin privilegios de administrador

```ini
[Setup]
DefaultDirName={userpf}\{#MyAppName}
PrivilegesRequired=lowest
```

`{userpf}` instala en el perfil del usuario actual (`%LocalAppData%\Programs\...` para una cuenta
no-admin) en vez de Program Files (`{autopf}`), y `PrivilegesRequired=lowest` evita que Windows
dispare el diálogo de UAC. Es consistente con que la app ya guarda su base de datos/config en
`%LocalAppData%` (ver [[07-proveedores-datos]]) — no había necesidad real de Program Files.

Nota aparte (no relacionada con permisos de administrador): al ser un `.exe` sin firma digital,
Windows SmartScreen puede mostrar una advertencia de "editor desconocido" la primera vez que se
ejecuta en una máquina nueva — se resuelve con un clic ("Más información → Ejecutar de todas
formas"), cualquier usuario estándar puede hacerlo. Firmar el ejecutable evitaría ese aviso, pero
implica comprar un certificado de firma de código y verificar la empresa ante una entidad
certificadora — evaluado y descartado por ahora dado el costo/trámite frente al beneficio (un solo
clic) para una app de uso interno.

### Versión

`/DMyAppVersion=<version>` se pasa al compilar (ver `scripts/publish-and-package.ps1`), tomado del
mismo valor que `<Version>` en el `.csproj` — instalador y ensamblado siempre quedan con el mismo
número.

### Distribución vía GitHub Releases

El instalador se publica como asset de un Release del repo de GitHub del proyecto — es la fuente
de la que lee `UpdateChecker` (ver [[08-autoactualizacion]]) y también de donde se puede descargar
manualmente por HTTPS para una instalación nueva (sin depender de una ruta de red interna ni de
AnyDesk). Ver [[decisiones/0010-autoactualizacion-via-github-releases]] por qué se eligió este
mecanismo en vez de la carpeta de red compartida usada originalmente.
