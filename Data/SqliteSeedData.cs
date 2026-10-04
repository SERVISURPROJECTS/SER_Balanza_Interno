using System.Text.Json;
using SER_Balanza_Interno.Controllers;
using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Data
{
    /// <summary>
    /// En una instalacion nueva (SQLite recien creado, sin usuarios todavia) crea el rol "admin" y
    /// un usuario administrador inicial para poder entrar por primera vez. No hace nada si ya existe
    /// algun usuario (instalaciones existentes, o reinicios posteriores al primer arranque).
    /// Tambien asegura los catalogos base del negocio (balanzas, documentos, permisos, etc.) en cada
    /// arranque: cada metodo es idempotente (busca por su clave natural antes de crear), asi que no
    /// duplica nada en instalaciones que ya los tengan cargados.
    ///
    /// Las contrasenas de estos usuarios semilla NO estan en el codigo (no se suben a git): se leen
    /// de la clave "SeedPasswords" en appsettings.json (ver appsettings.example.json para la
    /// plantilla). Si appsettings.json no trae la contrasena de un usuario semilla puntual, ese
    /// usuario simplemente no se crea (no rompe el arranque).
    /// </summary>
    public static class SqliteSeedData
    {
        public const string UsuarioInicial = "admin";
        public const string UsuarioLindsay = "LAH";

        private static string? ObtenerPasswordSeed(string clave)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(path)) return null;

            using var stream = File.OpenRead(path);
            using var doc = JsonDocument.Parse(stream);

            return doc.RootElement.TryGetProperty("SeedPasswords", out var seedPasswords) &&
                   seedPasswords.TryGetProperty(clave, out var valor)
                ? valor.GetString()
                : null;
        }

        public static void AsegurarAdminInicial(AppDbContext db)
        {
            if (db.Usuario.Any())
            {
                return;
            }

            var passwordInicial = ObtenerPasswordSeed("Admin");
            if (string.IsNullOrWhiteSpace(passwordInicial)) return;

            var rolAdmin = db.rol.FirstOrDefault(r => r.Nombre.ToLower() == "admin");
            if (rolAdmin is null)
            {
                rolAdmin = new rol { Nombre = "admin", activo = true, eliminado = false };
                db.rol.Add(rolAdmin);
                db.SaveChanges();
            }

            db.Usuario.Add(new Usuario
            {
                Nombre = "Administrador",
                usuario = UsuarioInicial,
                PasswordHash = AuthController.Hash(passwordInicial),
                Habilitado = true,
                Eliminado = false,
                id_rol = rolAdmin.Id
            });

            db.SaveChanges();
        }

        /// <summary>
        /// Catalogos base del negocio: rol "operador", permisos y su asignacion al rol admin, las
        /// balanzas (solo "Balanza Santa Anita" activa), la Hacienda "SAN", la Campaña "INVIERNO 2026"
        /// (sigla "INV26") y los 3 documentos de despacho vinculados a la balanza Santa Anita.
        /// </summary>
        public static void AsegurarCatalogosBase(AppDbContext db)
        {
            var rolAdmin = AsegurarRol(db, "admin");
            AsegurarRol(db, "operador");

            var permisoEditarPesaje = AsegurarPermiso(db, PermisoController.EditarPesaje);
            var permisoAbmBalanza = AsegurarPermiso(db, PermisoController.AbmBalanza);
            AsegurarPermisoRol(db, rolAdmin.Id, permisoEditarPesaje.id);
            AsegurarPermisoRol(db, rolAdmin.Id, permisoAbmBalanza.id);

            var idBalanzaSantaAnita = AsegurarBalanzas(db);

            AsegurarHacienda(db, nombre: "SAN", descripcion: "", abreviatura: "SAN");

            AsegurarCampania(db, nombre: "INVIERNO 2026", sigla: "INV26",
                validoDesde: new DateTime(2026, 6, 1), validoHasta: new DateTime(2026, 8, 31));

            AsegurarDocumento(db, "DESPACHO GRANO DE SORGO", idBalanzaSantaAnita);
            AsegurarDocumento(db, "DESPACHO GRANO DE GIRASOL", idBalanzaSantaAnita);
            AsegurarDocumento(db, "DESPACHO GRANO DE TRIGO", idBalanzaSantaAnita);

            var passwordLindsay = ObtenerPasswordSeed(UsuarioLindsay);
            if (!string.IsNullOrWhiteSpace(passwordLindsay))
            {
                AsegurarUsuario(db, nombre: "Lindsay Aguilera Huacama", usuario: UsuarioLindsay,
                    password: passwordLindsay, ci: "6870922", idRol: rolAdmin.Id, idBalanza: idBalanzaSantaAnita);
            }

            db.SaveChanges();
        }

        private static void AsegurarUsuario(AppDbContext db, string nombre, string usuario, string password,
            string ci, int idRol, int idBalanza)
        {
            if (db.Usuario.Any(u => u.usuario == usuario)) return;

            db.Usuario.Add(new Usuario
            {
                Nombre = nombre,
                usuario = usuario,
                PasswordHash = AuthController.Hash(password),
                Ci = ci,
                Habilitado = true,
                Eliminado = false,
                id_rol = idRol,
                id_balanza = idBalanza
            });
            db.SaveChanges();
        }

        private static rol AsegurarRol(AppDbContext db, string nombre)
        {
            var existente = db.rol.FirstOrDefault(r => r.Nombre.ToLower() == nombre.ToLower());
            if (existente is not null) return existente;

            var nuevo = new rol { Nombre = nombre, activo = true, eliminado = false };
            db.rol.Add(nuevo);
            db.SaveChanges();
            return nuevo;
        }

        private static Permiso AsegurarPermiso(AppDbContext db, string nombre)
        {
            var existente = db.Set<Permiso>().FirstOrDefault(p => p.nombre == nombre);
            if (existente is not null) return existente;

            var items = db.Set<Permiso>().ToList();
            var siguienteId = items.Count == 0 ? 1 : items.Max(p => p.id) + 1;
            var nuevo = new Permiso { id = siguienteId, nombre = nombre, es_grupo = false };
            db.Set<Permiso>().Add(nuevo);
            db.SaveChanges();
            return nuevo;
        }

        private static void AsegurarPermisoRol(AppDbContext db, int idRol, int idPermiso)
        {
            var existente = db.Set<permiso_rol>().Any(pr => pr.id_rol == idRol && pr.id_permiso == idPermiso);
            if (existente) return;

            db.Set<permiso_rol>().Add(new permiso_rol { id_rol = idRol, id_permiso = idPermiso });
            db.SaveChanges();
        }

        /// <summary>Las 11 balanzas administradas; solo "Balanza Santa Anita" queda activa. Devuelve su Id.</summary>
        private static int AsegurarBalanzas(AppDbContext db)
        {
            (string nombre, string descripcion, string direccion, bool activa)[] balanzas =
            {
                ("ADM CAR", "Balanza Carol", "", false),
                ("ADM BBA", "Balanza BBA", "", false),
                ("ADM TAR", "Balanza Tarope", "", false),
                ("ADM PAL", "Balanza Paltos", "", false),
                ("ADM AGI", "Balanza Agroinga", "", false),
                ("ADM SAN", "Balanza Santa Anita", "CARRETERA SAN IGNACIO - SAN VICENTE KM. 80", true),
                ("ADM MLET", "Marcela Leticia", "", false),
                ("ADM NVDA", "Nueva Vida", "", false),
                ("ADM PAT", "Balanza Pantanal", "", false),
                ("ADM PAR", "Balanza Palermo", "", false),
                ("ADM URA", "Balanza Urasai", "", false),
            };

            var repo = db.Set<balanza>();
            foreach (var (nombre, descripcion, direccion, activa) in balanzas)
            {
                var existente = repo.FirstOrDefault(b => b.nombre == nombre);
                if (existente is not null) continue;

                var items = repo.ToList();
                var siguienteId = items.Count == 0 ? 1 : items.Max(b => b.id) + 1;
                repo.Add(new balanza
                {
                    id = siguienteId,
                    nombre = nombre,
                    Descripcion = descripcion,
                    Direccion = direccion,
                    activo = activa,
                    tipo_conexion = 1,
                    nro_balanza = siguienteId,
                    id_indicador = 1,
                });
                db.SaveChanges();
            }

            return repo.First(b => b.nombre == "ADM SAN").id;
        }

        private static void AsegurarHacienda(AppDbContext db, string nombre, string descripcion, string abreviatura)
        {
            var repo = db.Set<Hacienda>();
            if (repo.Any(h => h.nombre == nombre)) return;

            var items = repo.ToList();
            var siguienteId = items.Count == 0 ? 1 : items.Max(h => h.Id) + 1;
            repo.Add(new Hacienda
            {
                Id = siguienteId,
                nombre = nombre,
                Descripcion = descripcion,
                Abreviatura = abreviatura,
                Activo = true,
                FechaCreacion = DateTime.Now
            });
            db.SaveChanges();
        }

        private static void AsegurarCampania(AppDbContext db, string nombre, string sigla, DateTime validoDesde, DateTime validoHasta)
        {
            var repo = db.Set<campania>();
            if (repo.Any(c => c.sigla == sigla)) return;

            var items = repo.ToList();
            var siguienteId = items.Count == 0 ? 1 : items.Max(c => c.id) + 1;
            repo.Add(new campania
            {
                id = siguienteId,
                nombre = nombre,
                sigla = sigla,
                activo = true,
                valido_desde = validoDesde,
                valido_hasta = validoHasta,
                fecha_creacion = DateTime.Now,
                id_usuario = 1
            });
            db.SaveChanges();
        }

        private static void AsegurarDocumento(AppDbContext db, string nombre, int idBalanza)
        {
            var repo = db.Set<documento>();
            if (repo.Any(d => d.nombre == nombre && d.Id_balanza == idBalanza)) return;

            var items = repo.ToList();
            var siguienteId = items.Count == 0 ? 1 : items.Max(d => d.id) + 1;
            repo.Add(new documento
            {
                id = siguienteId,
                nombre = nombre,
                activo = true,
                fecha_creacion = DateTime.Now,
                id_usuario = 1,
                modo_transaccion = "E",
                Id_balanza = idBalanza
            });
            db.SaveChanges();
        }
    }
}
