using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Controllers
{
    public class UsuarioController
    {
        public string? ErrorMessage { get; private set; }

        public List<Usuario> GetAll()
        {
            using var uow = UnitOfWorkFactory.Create();
            return uow.Repository<Usuario>().Query()
                .Where(u => !u.Eliminado)
                .OrderBy(u => u.Nombre)
                .ToList();
        }

        public List<rol> GetRoles()
        {
            using var uow = UnitOfWorkFactory.Create();
            return uow.Repository<rol>().Query()
                .Where(r => r.activo && !r.eliminado)
                .OrderBy(r => r.Nombre)
                .ToList();
        }

        public bool Create(string nombre, string usuario, string password, int? idRol, bool habilitado)
        {
            ErrorMessage = null;
            using var uow = UnitOfWorkFactory.Create();
            var repo = uow.Repository<Usuario>();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Nombre, usuario y contraseña son obligatorios.";
                return false;
            }

            if (repo.Query().Any(u => u.usuario == usuario && !u.Eliminado))
            {
                ErrorMessage = "Ya existe un usuario con ese nombre de acceso.";
                return false;
            }

            repo.Add(new Usuario
            {
                Nombre = nombre,
                usuario = usuario,
                PasswordHash = AuthController.Hash(password),
                Habilitado = habilitado,
                Eliminado = false,
                id_rol = idRol
            });
            uow.SaveChanges();
            return true;
        }

        public bool Update(int id, string nombre, string usuario, string? nuevaPassword, int? idRol, bool habilitado)
        {
            ErrorMessage = null;
            using var uow = UnitOfWorkFactory.Create();
            var repo = uow.Repository<Usuario>();

            var entity = repo.Query().FirstOrDefault(u => u.Id == id && !u.Eliminado);
            if (entity is null)
            {
                ErrorMessage = "El usuario no existe.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(usuario))
            {
                ErrorMessage = "Nombre y usuario son obligatorios.";
                return false;
            }

            if (repo.Query().Any(u => u.usuario == usuario && u.Id != id && !u.Eliminado))
            {
                ErrorMessage = "Ya existe un usuario con ese nombre de acceso.";
                return false;
            }

            entity.Nombre = nombre;
            entity.usuario = usuario;
            entity.id_rol = idRol;
            entity.Habilitado = habilitado;
            if (!string.IsNullOrWhiteSpace(nuevaPassword))
            {
                entity.PasswordHash = AuthController.Hash(nuevaPassword);
            }

            uow.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            ErrorMessage = null;
            using var uow = UnitOfWorkFactory.Create();
            var repo = uow.Repository<Usuario>();

            var entity = repo.Query().FirstOrDefault(u => u.Id == id && !u.Eliminado);
            if (entity is null)
            {
                ErrorMessage = "El usuario no existe.";
                return false;
            }

            entity.Eliminado = true;
            entity.Habilitado = false;
            uow.SaveChanges();
            return true;
        }
    }
}
