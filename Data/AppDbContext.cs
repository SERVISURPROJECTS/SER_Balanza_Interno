using Microsoft.EntityFrameworkCore;
using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Analisis> Analisis { get; set; } = null!;
        public DbSet<balanza> balanza { get; set; } = null!;
        public DbSet<campania> campania { get; set; } = null!;
        public DbSet<chofer> chofer { get; set; } = null!;
        public DbSet<compania> compania { get; set; } = null!;
        public DbSet<documento> documento { get; set; } = null!;
        public DbSet<factor> factor { get; set; } = null!;
        public DbSet<Hacienda> Hacienda { get; set; } = null!;
        public DbSet<indicador> indicador { get; set; } = null!;
        public DbSet<log_transferencia_pesaje> log_transferencia_pesaje { get; set; } = null!;
        public DbSet<Marca> Marca { get; set; } = null!;
        public DbSet<numeracion> numeracion { get; set; } = null!;
        public DbSet<origen_destino> origen_destino { get; set; } = null!;
        public DbSet<Permiso> Permiso { get; set; } = null!;
        public DbSet<permiso_rol> permiso_rol { get; set; } = null!;
        public DbSet<peso> peso { get; set; } = null!;
        public DbSet<pesoin> pesoin { get; set; } = null!;
        public DbSet<prod_fac_rank> prod_fac_rank { get; set; } = null!;
        public DbSet<Producto> Producto { get; set; } = null!;
        public DbSet<rol> rol { get; set; } = null!;
        public DbSet<socio_negocio> socio_negocio { get; set; } = null!;
        public DbSet<Usuario> Usuario { get; set; } = null!;
        public DbSet<Vehiculo> Vehiculo { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Las columnas en Postgres fueron creadas como "timestamp without time zone" (ver
            // Database/postgres_missing_tables.sql). El proveedor Npgsql asume "timestamp with time zone"
            // por defecto para DateTime, lo que exigiria Kind=Utc; se fuerza el tipo real de columna para
            // poder seguir usando DateTime.Now (Kind=Local). Esto es especifico de Postgres: SQLite y
            // SQL Server no necesitan (ni entienden) este tipo, por eso solo se aplica con ese proveedor.
            if (Database.ProviderName?.Contains("Npgsql", StringComparison.OrdinalIgnoreCase) == true)
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    foreach (var property in entityType.GetProperties())
                    {
                        if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                        {
                            property.SetColumnType("timestamp without time zone");
                        }
                    }
                }
            }
        }
    }
}
