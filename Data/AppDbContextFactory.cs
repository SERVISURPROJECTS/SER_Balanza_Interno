using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SER_Balanza_Interno.Data
{
    public enum DbProvider
    {
        Postgres,
        Sqlite,
        SqlServer
    }

    /// <summary>
    /// Arma el DbContext segun el proveedor configurado en appsettings.json ("Provider": "Postgres" | "Sqlite" | "SqlServer"),
    /// para poder cambiar de motor sin tocar codigo. Si no hay appsettings.json (o falta la cadena), cae a un archivo
    /// SQLite local en %LocalAppData% para que la app arranque igual.
    /// El archivo SQLite siempre se abre cifrado con SQLCipher (ver SqliteKeyProtector / SqliteEncryption):
    /// no es legible con ninguna herramienta externa sin la clave protegida por DPAPI de este usuario/maquina.
    /// </summary>
    public static class AppDbContextFactory
    {
        public static string GetDatabaseFolder()
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SER_Balanza_Interno");

            Directory.CreateDirectory(folder);

            return folder;
        }

        public static string GetDatabasePath() => Path.Combine(GetDatabaseFolder(), "balanza.db");

        /// <summary>
        /// Resuelve "Data Source=balanza.db" (u otro valor relativo de appsettings.json) siempre
        /// contra %LocalAppData%\SER_Balanza_Interno, nunca contra el directorio de arranque del
        /// proceso (que puede ser Program Files, donde la app no puede escribir sin ser administrador).
        /// </summary>
        public static string ResolverRutaSqlite(string connectionStringCruda)
        {
            const string prefijo = "Data Source=";
            var ruta = connectionStringCruda.StartsWith(prefijo, StringComparison.OrdinalIgnoreCase)
                ? connectionStringCruda[prefijo.Length..].Trim()
                : connectionStringCruda.Trim();

            return Path.IsPathRooted(ruta) ? ruta : Path.Combine(GetDatabaseFolder(), ruta);
        }

        /// <summary>
        /// Arma la cadena de conexion SQLite final (ruta absoluta + clave SQLCipher).
        /// </summary>
        public static string BuildSqliteConnectionString(string rutaAbsoluta)
        {
            var claveHex = SqliteKeyProtector.ObtenerClaveHex(Path.GetDirectoryName(rutaAbsoluta)!);

            return new SqliteConnectionStringBuilder
            {
                DataSource = rutaAbsoluta,
                Password = $"x'{claveHex}'"
            }.ToString();
        }

        public static (DbProvider Provider, string ConnectionString) ObtenerConfiguracion()
        {
            var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (File.Exists(settingsPath))
            {
                using var stream = File.OpenRead(settingsPath);
                using var doc = JsonDocument.Parse(stream);
                var root = doc.RootElement;

                var proveedorTexto = root.TryGetProperty("Provider", out var p) ? p.GetString() : null;
                var proveedor = proveedorTexto?.Trim().ToLowerInvariant() switch
                {
                    "sqlite" => DbProvider.Sqlite,
                    "sqlserver" => DbProvider.SqlServer,
                    _ => DbProvider.Postgres
                };

                if (root.TryGetProperty("ConnectionStrings", out var cadenas))
                {
                    var clave = proveedor.ToString();
                    if (cadenas.TryGetProperty(clave, out var cs) && cs.GetString() is { Length: > 0 } conn)
                    {
                        return (proveedor, conn);
                    }
                }

                // Compatibilidad con el formato anterior de un unico "AppConnection" (Postgres).
                if (root.TryGetProperty("AppConnection", out var legacy) && legacy.GetString() is { Length: > 0 } legacyConn)
                {
                    return (DbProvider.Postgres, legacyConn);
                }
            }

            return (DbProvider.Sqlite, "Data Source=balanza.db");
        }

        public static AppDbContext Create()
        {
            var (proveedor, connectionString) = ObtenerConfiguracion();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            switch (proveedor)
            {
                case DbProvider.Sqlite:
                    var ruta = ResolverRutaSqlite(connectionString);
                    builder.UseSqlite(BuildSqliteConnectionString(ruta));
                    break;
                case DbProvider.SqlServer:
                    builder.UseSqlServer(connectionString);
                    break;
                default:
                    builder.UseNpgsql(connectionString);
                    break;
            }

            return new AppDbContext(builder.Options);
        }

        /// <summary>
        /// Las migraciones de EF Core solo se mantienen para SQLite (motor local de instalacion).
        /// Se usa tanto en diseño ("dotnet ef migrations add") como en tiempo de ejecucion para no
        /// depender del "Provider" configurado en appsettings.json.
        /// </summary>
        public static AppDbContext CreateForSqliteMigrations()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(BuildSqliteConnectionString(GetDatabasePath()))
                .Options;

            return new AppDbContext(options);
        }
    }

    // Permite ejecutar "dotnet ef migrations add ..." / "dotnet ef database update" desde este proyecto.
    // Siempre apunta a SQLite: es el unico proveedor para el que se mantienen migraciones.
    public class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args) => AppDbContextFactory.CreateForSqliteMigrations();
    }
}
