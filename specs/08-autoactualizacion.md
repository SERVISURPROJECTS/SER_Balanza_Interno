# 08 — Autoactualización

## Contexto

El operador remoto no tiene acceso directo a la red interna de la empresa. La primera versión de
este mecanismo usaba una carpeta de red compartida ("casillero") a la que el operador ya tenía
acceso por otro motivo — se reemplazó por los Releases del repo de GitHub del proyecto, ver
[[decisiones/0010-autoactualizacion-via-github-releases]].

## Requisitos

1. Al arrancar, la app revisa si hay una versión más nueva publicada como Release de GitHub, sin
   bloquear el arranque si GitHub no está disponible (sin internet, timeout, repo sin releases).
2. Si hay versión nueva, se descarga el instalador (asset `.exe` del release) y se ejecuta en
   silencio; la app actual se cierra para liberar los archivos antes de que el instalador los
   reemplace.
3. Publicar una versión nueva debe ser un solo paso reproducible.
4. Un fallo de red nunca debe impedir que el operador use la app ese día — la actualización es
   "best effort".
5. El repositorio de GitHub consultado debe ser **público** (sin esto, descargar el asset del
   release requeriría embeber un token de acceso en cada instalación, lo cual es un riesgo de
   seguridad — ver el ADR).

## Diseño actual

### Chequeo al arrancar (`Update/UpdateChecker.cs`)

- `CheckAndUpdate()` se llama desde `Program.cs`, **antes** de `ApplicationConfiguration.Initialize()`.
- Corre en un `Task` con un `CancellationTokenSource(60000)` (60s) que cancela automáticamente
  tanto la consulta a la API como la descarga del instalador si se excede ese tiempo — nunca deja
  un hilo en segundo plano que pueda terminar de golpe la app más tarde, a destiempo, mientras el
  operador ya la está usando.
- Lee `appsettings.json → UpdateRepo` (formato `"owner/nombre-repo"`). Si falta, no hace nada.
- `GET https://api.github.com/repos/{UpdateRepo}/releases/latest`: toma `tag_name` (ej. `"v2.3.0"`,
  se le quita la `v` inicial) y lo compara contra
  `Assembly.GetExecutingAssembly().GetName().Version`.
- Si hay una versión más nueva: busca en `assets[]` el primero cuyo `name` termine en `.exe`,
  descarga su `browser_download_url` a una carpeta temporal, lo ejecuta con
  `/VERYSILENT /NORESTART /SUPPRESSMSGBOXES`, y llama `Environment.Exit(0)` inmediatamente para
  soltar el lock de archivos antes de que el instalador los sobrescriba.
- Cualquier excepción se traga silenciosamente (try/catch envolvente en `CheckAndUpdate`) — nunca
  debe tumbar el arranque de la app.
- No requiere token de autenticación: la API de Releases y la descarga de assets de un repo
  público de GitHub son de lectura libre.

### Publicación (`scripts/publish-and-package.ps1`)

1. `dotnet publish` self-contained win-x64 con la versión pasada por parámetro.
2. Compila el instalador con Inno Setup (`installer/setup.iss`).
3. Si hay `gh` (GitHub CLI) instalado y autenticado, crea el Release (`gh release create vX.Y.Z
   <instalador> --repo <UpdateRepo>`) y sube el instalador como asset — el tag del release
   (`vX.Y.Z`) es lo que `UpdateChecker` compara.
4. Si `gh` no está disponible/autenticado (o se pasa `-SkipRelease`), el script deja el instalador
   listo en `installer\output\` y avisa que hay que subirlo a mano a
   `https://github.com/<UpdateRepo>/releases/new` — es el camino que se usó manualmente antes de
   tener `gh` instalado, y sigue siendo válido como respaldo.

Antes de correrlo hay que subir `<Version>`/`<AssemblyVersion>`/`<FileVersion>` en
`SER_Balanza_Interno.csproj` y verificar que compila (`dotnet build`).

Ver [[decisiones/0008-fallo-transitorio-inno-setup]] por un fallo de compilación intermitente
conocido en el paso de Inno Setup.

### Config relevante (`appsettings.json`)

```json
"UpdateRepo": "SERVISURPROJECTS/SER_Balanza_Interno"
```

Centralizado en un solo valor para poder cambiar de repo sin tocar código ni recompilar.
