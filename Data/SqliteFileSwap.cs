namespace SER_Balanza_Interno.Data
{
    /// <summary>
    /// Reemplaza un archivo SQLite por otro (compactado o recien cifrado) de forma segura:
    /// reintenta el swap si Windows todavia tiene el archivo bloqueado por un handle que tarda
    /// en liberarse (antivirus, indexador, finalizers pendientes del pool de SQLite), y si algo
    /// falla a mitad de camino, restaura el archivo original intacto.
    /// </summary>
    public static class SqliteFileSwap
    {
        public static void Reemplazar(string dbPath, string tempPath)
        {
            var backupPath = dbPath + ".bak";

            // Fuerza a liberar cualquier SafeHandle de SQLite pendiente de finalizar antes del swap.
            GC.Collect();
            GC.WaitForPendingFinalizers();

            ConReintentos(() => File.Delete(backupPath));
            ConReintentos(() => File.Move(dbPath, backupPath));

            try
            {
                ConReintentos(() => File.Move(tempPath, dbPath));
                ConReintentos(() => File.Delete(dbPath + "-wal"));
                ConReintentos(() => File.Delete(dbPath + "-shm"));
                ConReintentos(() => File.Delete(backupPath));
            }
            catch
            {
                ConReintentos(() => File.Delete(dbPath));
                ConReintentos(() => File.Move(backupPath, dbPath));
                throw;
            }
        }

        private static void ConReintentos(Action accion, int intentos = 30, int esperaMs = 300)
        {
            for (var intento = 1; ; intento++)
            {
                try
                {
                    accion();
                    return;
                }
                catch (IOException) when (intento < intentos)
                {
                    Thread.Sleep(esperaMs);
                }
            }
        }
    }
}
