using Microsoft.Data.Sqlite;

namespace SER_Balanza_Interno.Data
{
    /// <summary>
    /// Garantiza que el archivo SQLite quede cifrado con SQLCipher usando la clave protegida por
    /// DPAPI (SqliteKeyProtector). Si el archivo ya existia en texto plano (instalaciones anteriores
    /// a este cambio), lo cifra una sola vez con "sqlcipher_export" y reemplaza el archivo original
    /// sin perder una fila. No hace nada si el archivo no existe todavia (Migrate() lo crea ya cifrado).
    /// </summary>
    public static class SqliteEncryption
    {
        public static void AsegurarCifrado(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                return;
            }

            var claveHex = SqliteKeyProtector.ObtenerClaveHex(Path.GetDirectoryName(dbPath)!);
            var connectionStringCifrada = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath,
                Password = $"x'{claveHex}'",
                Pooling = false
            }.ToString();

            if (PuedeAbrirseConClave(connectionStringCifrada))
            {
                return; // ya esta cifrado con esta clave.
            }

            CifrarArchivoExistente(dbPath, claveHex);
        }

        private static bool PuedeAbrirseConClave(string connectionString)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT count(*) FROM sqlite_master;";
                cmd.ExecuteScalar();
                return true;
            }
            catch (SqliteException)
            {
                return false;
            }
        }

        private static void CifrarArchivoExistente(string dbPath, string claveHex)
        {
            var tempPath = dbPath + ".encrypt.tmp";
            File.Delete(tempPath);

            var connectionStringPlana = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath,
                Pooling = false
            }.ToString();

            using (var plano = new SqliteConnection(connectionStringPlana))
            {
                plano.Open();

                using (var attach = plano.CreateCommand())
                {
                    attach.CommandText = "ATTACH DATABASE $destino AS cifrada KEY $clave;";

                    var destino = attach.CreateParameter();
                    destino.ParameterName = "$destino";
                    destino.Value = tempPath;
                    attach.Parameters.Add(destino);

                    var clave = attach.CreateParameter();
                    clave.ParameterName = "$clave";
                    clave.Value = $"x'{claveHex}'";
                    attach.Parameters.Add(clave);

                    attach.ExecuteNonQuery();
                }

                using (var export = plano.CreateCommand())
                {
                    export.CommandText = "SELECT sqlcipher_export('cifrada');";
                    export.ExecuteScalar();
                }

                using (var detach = plano.CreateCommand())
                {
                    detach.CommandText = "DETACH DATABASE cifrada;";
                    detach.ExecuteNonQuery();
                }
            }

            SqliteConnection.ClearAllPools();
            SqliteFileSwap.Reemplazar(dbPath, tempPath);
        }
    }
}
