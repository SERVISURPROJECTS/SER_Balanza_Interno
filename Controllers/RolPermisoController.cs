using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Controllers
{
    public class PermisoDeRol
    {
        public int IdPermiso { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Asignado { get; set; }
    }

    /// <summary>
    /// Soporte para la pantalla de administracion de Roles y Permisos (asignar permisos como
    /// "ABM Balanza" a un rol, para que cualquier usuario con ese rol pueda hacer esa gestion).
    /// </summary>
    public class RolPermisoController
    {
        public List<rol> ObtenerRoles()
        {
            using var uow = UnitOfWorkFactory.Create();
            return uow.Repository<rol>().Query().Where(r => !r.eliminado).OrderBy(r => r.Nombre).ToList();
        }

        public List<PermisoDeRol> ObtenerPermisosDeRol(int idRol)
        {
            using var uow = UnitOfWorkFactory.Create();
            var asignados = uow.Repository<permiso_rol>().Query()
                .Where(pr => pr.id_rol == idRol)
                .Select(pr => pr.id_permiso)
                .ToHashSet();

            return uow.Repository<Permiso>().GetAll()
                .OrderBy(p => p.nombre)
                .Select(p => new PermisoDeRol { IdPermiso = p.id, Nombre = p.nombre, Asignado = asignados.Contains(p.id) })
                .ToList();
        }

        public void GuardarPermisosDeRol(int idRol, IEnumerable<PermisoDeRol> permisos)
        {
            using var uow = UnitOfWorkFactory.Create();
            var repoPermisoRol = uow.Repository<permiso_rol>();
            var existentes = repoPermisoRol.Query().Where(pr => pr.id_rol == idRol).ToList();

            foreach (var permiso in permisos)
            {
                var existente = existentes.FirstOrDefault(pr => pr.id_permiso == permiso.IdPermiso);
                if (permiso.Asignado && existente is null)
                {
                    repoPermisoRol.Add(new permiso_rol { id_permiso = permiso.IdPermiso, id_rol = idRol });
                }
                else if (!permiso.Asignado && existente is not null)
                {
                    repoPermisoRol.Remove(existente);
                }
            }

            uow.SaveChanges();
        }

        /// <summary>Crea el permiso si no existe aun (p. ej. para que aparezca en la lista antes de que algo lo haya pedido).</summary>
        public void AsegurarPermiso(string nombre)
        {
            using var uow = UnitOfWorkFactory.Create();
            var repo = uow.Repository<Permiso>();
            if (repo.Query().Any(p => p.nombre == nombre)) return;

            var items = repo.GetAll();
            var siguienteId = items.Count == 0 ? 1 : items.Max(p => p.id) + 1;
            repo.Add(new Permiso { id = siguienteId, nombre = nombre, es_grupo = false });
            uow.SaveChanges();
        }
    }
}
