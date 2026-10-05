# 0007 — Contraseñas semilla fuera del código fuente

## Contexto

Al inicializar el repositorio en git para subirlo a GitHub, `Data/SqliteSeedData.cs` tenía las
contraseñas de los usuarios semilla (admin inicial, usuario adicional) como constantes de texto
plano en el código. Subido a git, esas contraseñas quedan en el historial para siempre, aunque
después se cambien en la base de datos real.

## Decisión

Las contraseñas semilla se leen en tiempo de ejecución desde `appsettings.json → SeedPasswords`
(una clave por usuario, ej. `"Admin"`, `"LAH"`), con el mismo patrón manual de lectura (`JsonDocument`)
que ya usa `AppDbContextFactory` para la cadena de conexión. `appsettings.json` real está en
`.gitignore` (nunca se sube); `appsettings.example.json` sí se versiona, con valores placeholder
("cambiar-esta-password").

Si una clave de `SeedPasswords` falta para un usuario puntual, ese usuario simplemente **no se
crea** (no se lanza excepción, no se bloquea el arranque) — el seed es best-effort, igual que el
resto de `AsegurarCatalogosBase`.

## Por qué así y no de otra forma

- Se descartó un valor hardcodeado de respaldo ("si falta la config, usar esta contraseña por
  defecto"): habría reintroducido el mismo problema que se quería resolver (un secreto fijo en el
  código), solo que como fallback en vez de como valor principal.
- El instalador sigue funcionando igual para el usuario final: `appsettings.json` (con los valores
  reales) se publica igual que siempre dentro del paquete de instalación (`dotnet publish` lo
  copia al output, Inno Setup lo empaqueta) — lo único que cambió es que ese archivo nunca viaja
  por git, solo por el instalador.
