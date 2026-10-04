using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Controllers
{
    public class AuthController
    {
        public string? ErrorMessage { get; private set; }

        public Usuario? Login(string usuario, string password)
        {
            ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Ingrese usuario y contraseña.";
                return null;
            }

            using var uow = UnitOfWorkFactory.Create();

            var user = uow.Repository<Usuario>().Query().FirstOrDefault(u => u.usuario == usuario);
            if (user is null || user.Eliminado)
            {
                ErrorMessage = "Usuario o contraseña incorrectos.";
                return null;
            }

            if (!user.Habilitado)
            {
                ErrorMessage = "El usuario se encuentra deshabilitado.";
                return null;
            }

            if (!string.Equals(user.PasswordHash, Hash(password), StringComparison.Ordinal))
            {
                ErrorMessage = "Usuario o contraseña incorrectos.";
                return null;
            }

            return user;
        }

        public static string Hash(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}
