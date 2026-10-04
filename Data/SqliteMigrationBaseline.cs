using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace SER_Balanza_Interno.Data
{
    /// <summary>
    /// Si el archivo SQLite ya existia (creado antes de adoptar migraciones de EF Core), registra la
    /// migracion inicial como "ya aplicada" sin ejecutar su Up() ni tocar una sola tabla o fila.
    /// Asi Database.Migrate() no intenta recrear un esquema que ya esta presente con datos.
    /// En una instalacion nueva (sin tablas todavia) no hace nada: Migrate() crea todo desde cero.
    /// </summary>
    public static class SqliteMigrationBaseline
    {
        private const string InitialMigrationId = "20261001210556_InitialCreate";
        private const string ProductVersion = "8.0.11";

        public static void AsegurarBaseline(AppDbContext db)
        {
            var conn = db.Database.GetDbConnection();
            conn.Open();
            try
            {
                if (TablaExiste(conn, "__EFMigrationsHistory"))
                {
                    return;
                }

                if (!TablaExiste(conn, "peso"))
                {
                    return;
                }

                using (var create = conn.CreateCommand())
                {
                    create.CommandText =
                        "CREATE TABLE \"__EFMigrationsHistory\" (" +
                        "\"MigrationId\" TEXT NOT NULL CONSTRAINT \"PK___EFMigrationsHistory\" PRIMARY KEY, " +
                        "\"ProductVersion\" TEXT NOT NULL)";
                    create.ExecuteNonQuery();
                }

                using (var insert = conn.CreateCommand())
                {
                    insert.CommandText =
                        "INSERT INTO \"__EFMigrationsHistory\" (\"MigrationId\", \"ProductVersion\") VALUES ($id, $version)";

                    var idParam = insert.CreateParameter();
                    idParam.ParameterName = "$id";
                    idParam.Value = InitialMigrationId;
                    insert.Parameters.Add(idParam);

                    var versionParam = insert.CreateParameter();
                    versionParam.ParameterName = "$version";
                    versionParam.Value = ProductVersion;
                    insert.Parameters.Add(versionParam);

                    insert.ExecuteNonQuery();
                }
            }
            finally
            {
                conn.Close();
            }
        }

        private static bool TablaExiste(DbConnection conn, string tabla)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name=$n";

            var param = cmd.CreateParameter();
            param.ParameterName = "$n";
            param.Value = tabla;
            cmd.Parameters.Add(param);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }
    }
}
