using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Data
{
    /// <summary>
    /// Usuario autenticado para la sesión actual de la aplicación (vida == proceso).
    /// </summary>
    public static class Sesion
    {
        public static Usuario? UsuarioActual { get; set; }

        /// <summary>Balanza elegida al iniciar sesion; todo pesaje/documento registrado en la sesion se etiqueta con ella.</summary>
        public static balanza? BalanzaActual { get; set; }

        public static bool HayUsuarioActivo => UsuarioActual is not null;

        public static void Cerrar()
        {
            UsuarioActual = null;
            BalanzaActual = null;
        }
    }
}
