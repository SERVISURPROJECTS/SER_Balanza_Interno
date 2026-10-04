using Microsoft.EntityFrameworkCore;
using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.UI;
using SER_Balanza_Interno.Update;
using SER_Balanza_Interno.Views;
using SQLitePCL;

namespace SER_Balanza_Interno
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Habilita el proveedor nativo de SQLCipher (SQLitePCLRaw.bundle_e_sqlcipher).
            Batteries_V2.Init();

            UpdateChecker.CheckAndUpdate();

            var (proveedor, connectionStringCruda) = AppDbContextFactory.ObtenerConfiguracion();
            if (proveedor == DbProvider.Sqlite)
            {
                var rutaDb = AppDbContextFactory.ResolverRutaSqlite(connectionStringCruda);
                SqliteEncryption.AsegurarCifrado(rutaDb);

                using (var db = AppDbContextFactory.Create())
                {
                    SqliteMigrationBaseline.AsegurarBaseline(db);
                    db.Database.Migrate();
                    SqliteSeedData.AsegurarAdminInicial(db);
                    SqliteSeedData.AsegurarCatalogosBase(db);
                }

                SqliteMaintenance.EjecutarSiCorresponde();
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Theme.Inicializar();
            Application.Run(new FrmLogin());
        }
    }
}