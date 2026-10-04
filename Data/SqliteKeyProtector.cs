using System.Security.Cryptography;

namespace SER_Balanza_Interno.Data
{
    /// <summary>
    /// Genera y protege con DPAPI (atado a este usuario de Windows, en esta maquina) la clave de
    /// cifrado SQLCipher de la base local. El archivo protegido no se puede descifrar en otra
    /// cuenta ni copiandolo a otra PC: Windows es quien guarda el secreto, no la app.
    /// </summary>
    public static class SqliteKeyProtector
    {
        private const int LongitudClaveBytes = 32; // 256 bits
        private const string NombreArchivoClave = "clave.dat";

        public static string ObtenerClaveHex(string carpetaDatos)
        {
            var rutaClave = Path.Combine(carpetaDatos, NombreArchivoClave);

            byte[] claveSinProteger;
            if (File.Exists(rutaClave))
            {
                var protegida = File.ReadAllBytes(rutaClave);
                claveSinProteger = ProtectedData.Unprotect(protegida, null, DataProtectionScope.CurrentUser);
            }
            else
            {
                claveSinProteger = RandomNumberGenerator.GetBytes(LongitudClaveBytes);
                var protegida = ProtectedData.Protect(claveSinProteger, null, DataProtectionScope.CurrentUser);
                File.WriteAllBytes(rutaClave, protegida);
            }

            return Convert.ToHexString(claveSinProteger);
        }
    }
}
