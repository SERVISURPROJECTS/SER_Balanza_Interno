using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Controllers
{
    public class PermisoController
    {
        public const string EditarPesaje = "Editar Pesaje";
        public const string AbmBalanza = "ABM Balanza";

        /// <summary>
        /// true si el usuario puede editar/guardar pesajes. El rol "admin" siempre puede;
        /// para los demas roles se consulta Permiso/permiso_rol (gestionables desde el CRUD de esas tablas).
        /// Si el permiso aun no existe se crea automaticamente y se otorga al rol "admin".
        /// </summary>
        public bool TienePermiso(int? idRol, string nombrePermiso)
        {
            if (idRol is null) return false;

            using var uow = UnitOfWorkFactory.Create();

            var rol = uow.Repository<rol>().Query().FirstOrDefault(r => r.Id == idRol);
            if (rol is not null && rol.Nombre.Equals("admin", StringComparison.OrdinalIgnoreCase))
                return true;

            var repoPermiso = uow.Repository<Permiso>();
            var permiso = repoPermiso.Query().FirstOrDefault(p => p.nombre == nombrePermiso);
            if (permiso is null)
            {
                permiso = new Permiso { id = SiguienteId(repoPermiso), nombre = nombrePermiso, es_grupo = false };
                repoPermiso.Add(permiso);

                var rolAdmin = uow.Repository<rol>().Query().FirstOrDefault(r => r.Nombre.ToLower() == "admin");
                if (rolAdmin is not null)
                {
                    uow.Repository<permiso_rol>().Add(new permiso_rol { id_permiso = permiso.id, id_rol = rolAdmin.Id });
                }
                uow.SaveChanges();
                return false;
            }

            return uow.Repository<permiso_rol>().Query().Any(pr => pr.id_permiso == permiso.id && pr.id_rol == idRol);
        }

        private static int SiguienteId(IRepository<Permiso> repo)
        {
            var items = repo.GetAll();
            return items.Count == 0 ? 1 : items.Max(p => p.id) + 1;
        }
    }
}
