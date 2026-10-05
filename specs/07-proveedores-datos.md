# 07 — Proveedores de datos

## Contexto

La app soporta tres motores de base de datos intercambiables sin recompilar (`appsettings.json →
Provider`), pero solo SQLite tiene migraciones de EF Core mantenidas — es el motor real de
instalación de campo (self-contained, cifrado, sin servidor externo). Postgres/SqlServer quedan
como opción para un despliegue centralizado futuro, con el esquema a cargo de scripts SQL a mano
(`Database/postgres_missing_tables.sql`), no de migraciones EF.

## Requisitos

1. `appsettings.json` decide el proveedor (`"Provider": "Sqlite" | "Postgres" | "SqlServer"`) y su
   cadena de conexión, sin tocar código.
2. Si falta `appsettings.json` o la clave del proveedor, la app **no debe fallar al arrancar** —
   cae a SQLite local por defecto.
3. El archivo SQLite siempre está cifrado (SQLCipher) con una clave protegida por DPAPI de la
   máquina/usuario — no debe ser legible copiándolo a otra PC ni abriéndolo con una herramienta
   genérica de SQLite.
4. Las migraciones de SQLite se aplican automáticamente al arrancar, sobre instalaciones nuevas y
   ya existentes (ver [[08-autoactualizacion]]).
5. Las contraseñas de los usuarios semilla (admin inicial, y cualquier otro) no viven en el código
   fuente — ver [[decisiones/0007-seed-passwords-fuera-de-git]].

## Diseño actual

### Resolución de configuración (`AppDbContextFactory.ObtenerConfiguracion`)

Lee `appsettings.json` manualmente con `JsonDocument` (no usa `IConfiguration`/DI — patrón elegido
para no depender de un host genérico en una app WinForms simple). Orden de resolución:

1. `Provider` + `ConnectionStrings[Provider]` si ambos están.
2. Compatibilidad retro: `AppConnection` suelto (formato viejo, asume Postgres).
3. Si nada matchea: SQLite local, `Data Source=balanza.db`.

### SQLite (único con migraciones reales)

- Ruta siempre resuelta contra `%LocalAppData%\SER_Balanza_Interno\` (nunca el directorio de
  instalación, que puede ser de solo lectura para el usuario sin admin — ver [[09-instalador]]).
- `SqliteEncryption`/`SqliteKeyProtector`: la clave de cifrado SQLCipher se genera y se guarda
  protegida con DPAPI junto al `.db`; sin esa clave (o en otra máquina/usuario) el archivo no se
  puede abrir.
- `SqliteMigrationBaseline`: maneja la transición de una base pre-EF-migrations (si existía antes
  de que se introdujeran migraciones) sin perder datos.
- `SqliteMaintenance`: `PRAGMA optimize` + `VACUUM INTO` al arrancar, como máximo una vez cada N
  días — mantenimiento liviano sin intervención manual.
- Migraciones (`Migrations/`): `InitialCreate`, `AgregarCiAUsuario` (agrega `Usuario.ci`). Se
  generan con `dotnet ef migrations add <Nombre>` apuntando siempre a SQLite
  (`AppDbContextDesignTimeFactory`/`CreateForSqliteMigrations`), independientemente de qué
  proveedor esté configurado en el `appsettings.json` de esa máquina de desarrollo.

### Postgres / SqlServer

- Sin migraciones EF — el esquema se mantiene a mano en `Database/postgres_missing_tables.sql`
  (CREATE TABLE IF NOT EXISTS por tabla faltante; no es un script de migración versionado, es un
  catch-up puntual).
- `AppDbContext.OnModelCreating` fuerza `timestamp without time zone` para columnas `DateTime` en
  Postgres (Npgsql por defecto asume `timestamptz`, lo que exigiría `Kind=Utc`; el código usa
  `DateTime.Now` con `Kind=Local` en todos lados, así que se fuerza el tipo de columna real en vez
  de cambiar ese hábito en todo el código).

### Seed de datos (`Data/SqliteSeedData.cs`)

Dos responsabilidades, ambas idempotentes (seguras de correr en cada arranque):

- `AsegurarAdminInicial`: solo actúa si la tabla `Usuario` está vacía (instalación 100% nueva);
  crea el rol admin + el usuario admin.
- `AsegurarCatalogosBase`: corre siempre, busca por clave natural antes de crear cada catálogo
  (roles, permisos base, las balanzas de la empresa, Hacienda, Campaña, documentos de despacho, y
  el usuario seed adicional de este proyecto) — nunca duplica ni pisa lo que ya exista.
- Las contraseñas de estos usuarios semilla se leen de `appsettings.json → SeedPasswords`, no
  están en el código — ver [[decisiones/0007-seed-passwords-fuera-de-git]].
