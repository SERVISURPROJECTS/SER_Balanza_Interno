# 0010 — Autoactualización vía GitHub Releases (no casillero de red)

## Contexto

El mecanismo original de autoactualización (ver [[0008-fallo-transitorio-inno-setup]] para el
contexto de cómo se publicaba) usaba una carpeta de red compartida personal (un "casillero" al
que el operador ya tenía acceso por otro motivo) como fuente de `version.json` + el instalador.
Se decidió dejar de usarla: no es un medio de distribución seguro ni confiable para este fin —
depende de permisos de un casillero personal ajeno a la aplicación, sin control de versiones, sin
registro de qué se publicó cuándo, y sujeta a que esa persona mantenga el acceso.

## Decisión

`UpdateChecker` consulta el último **Release de GitHub** del repo del proyecto
(`appsettings.json → UpdateRepo`) en vez de una ruta de red: compara `tag_name` del release contra
la versión del ensamblado actual, y si hay una más nueva, descarga el asset `.exe` adjunto al
release y lo ejecuta en silencio. El repo debe ser **público** para que esto funcione sin
credenciales (ver más abajo).

## Por qué así y no de otra forma

- **Repo público, no privado**: si el repo fuera privado, descargar assets de un release por la
  API de GitHub requiere un token de autenticación. Ese token tendría que vivir embebido en
  `appsettings.json` de cada instalación para que `UpdateChecker` lo use — cualquiera con acceso a
  esa carpeta (cualquier operador, en cualquier máquina) podría extraerlo y usarlo para acceder al
  repo con lo que ese token permita. Se evaluó y se descartó por ese riesgo; de ahí que el
  repositorio se haya dejado público a propósito.
- **GitHub Releases, no un servidor de actualizaciones dedicado**: se descartó montar
  infraestructura propia (un endpoint HTTP, un bucket de storage) porque agrega una pieza más para
  mantener y pagar, cuando GitHub ya ofrece justamente esto (versionado, assets descargables,
  historial) gratis para un repo público.
- **Publicación con `gh` CLI, con fallback manual**: el script de publicación intenta crear el
  release automáticamente si `gh` está instalado y autenticado, pero no lo exige — si no está
  disponible, deja el instalador listo y avisa la URL exacta para subirlo a mano desde el
  navegador. Esto es necesario porque en este entorno de desarrollo concreto, el propio agente
  (Claude Code) tiene bloqueado por política de sandbox el `git push`/`gh release create` directo
  (clasificador de seguridad "Data Exfiltration"/"Sensitive-Source Provenance") — el camino manual
  no es solo un respaldo teórico, fue el que efectivamente se usó para publicar el primer release
  de este mecanismo.
- **Timeout de 60s con cancelación real (no solo abandono de hilo)**: la versión anterior (basada
  en archivo de red) usaba un hilo en background con `Join(timeoutMs)` — si el timeout vencía, el
  hilo seguía corriendo huérfano. Para una descarga HTTP de ~55MB eso es más riesgoso (podría
  terminar de descargar y llamar `Environment.Exit(0)` minutos después, con el operador ya
  trabajando en la app). Por eso acá se usa `CancellationTokenSource` real: al vencer el timeout,
  las llamadas HTTP en curso se cancelan de verdad, no se abandonan.
