using Microsoft.Data.Sqlite;

namespace SER_Balanza_Interno.Data
{
    /// <summary>
    /// Mantenimiento periodico del archivo SQLite: PRAGMA optimize (actualiza estadisticas del
    /// planificador) + VACUUM INTO (compacta a una copia nueva, sin bloquear lecturas, y la reemplaza).
    /// Se corre al arrancar la app, maximo una vez cada <see cref="IntervaloDias"/> dias.
    /// </summary>
    public static class SqliteMaintenance
    {
        private const int IntervaloDias = 15;

        public static void EjecutarSiCorresponde()
        {
            var dbPath = AppDbContextFactory.GetDatabasePath();
            if (!File.Exists(dbPath))
            {
                return;
            }

            var folder = Path.GetDirectoryName(dbPath)!;
            var markerPath = Path.Combine(folder, "ultimo_mantenimiento.txt");

            if (File.Exists(markerPath)
                && DateTime.TryParse(File.ReadAllText(markerPath), out var ultima)
                && DateTime.Now - ultima < TimeSpan.FromDays(IntervaloDias))
            {
                return;
            }

            try
            {
                EjecutarMantenimiento(dbPath);
            }
            finally
            {
                File.WriteAllText(markerPath, DateTime.Now.ToString("o"));
            }
        }

        private static void EjecutarMantenimiento(string dbPath)
        {
            var claveHex = SqliteKeyProtector.ObtenerClaveHex(Path.GetDirectoryName(dbPath)!);
            var connectionStringCifrada = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath,
                Password = $"x'{claveHex}'",
                Pooling = false
            }.ToString();

            using (var connection = new SqliteConnection(connectionStringCifrada))
            {
                connection.Open();
                using var optimize = connection.CreateCommand();
                optimize.CommandText = "PRAGMA optimize;";
                optimize.ExecuteNonQuery();
            }

            var tempPath = dbPath + ".vacuum.tmp";
            File.Delete(tempPath);

            using (var connection = new SqliteConnection(connectionStringCifrada))
            {
                connection.Open();
                using var vacuum = connection.CreateCommand();
                vacuum.CommandText = "VACUUM INTO $destino;";

                var destino = vacuum.CreateParameter();
                destino.ParameterName = "$destino";
                destino.Value = tempPath;
                vacuum.Parameters.Add(destino);

                vacuum.ExecuteNonQuery();
            }

            // Libera los handles del pool de conexiones antes de reemplazar el archivo.
            SqliteConnection.ClearAllPools();
            SqliteFileSwap.Reemplazar(dbPath, tempPath);
        }
    }
}
