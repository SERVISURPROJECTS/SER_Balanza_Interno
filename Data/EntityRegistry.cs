using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Data
{
    /// <summary>
    /// Tablas administrables por el CRUD generico (Usuario tiene pantalla propia por el manejo de contraseña/rol).
    /// </summary>
    public static class EntityRegistry
    {
        public static readonly (string Nombre, Type Tipo)[] Entidades =
        {
            ("Campaña", typeof(campania)),
            ("Chofer", typeof(chofer)),
            ("Compañía", typeof(compania)),
            ("Documento", typeof(documento)),
            ("Factor", typeof(factor)),
            ("Hacienda", typeof(Hacienda)),
            ("Indicador", typeof(indicador)),
            ("Log transferencia pesaje", typeof(log_transferencia_pesaje)),
            ("Marca", typeof(Marca)),
            ("Numeración", typeof(numeracion)),
            ("Origen / Destino", typeof(origen_destino)),
            ("Permiso", typeof(Permiso)),
            ("Permiso x Rol", typeof(permiso_rol)),
            ("Peso (salida)", typeof(peso)),
            ("Peso (ingreso)", typeof(pesoin)),
            ("Producto x Factor (rank)", typeof(prod_fac_rank)),
            ("Producto", typeof(Producto)),
            ("Rol", typeof(rol)),
            ("Socio de negocio", typeof(socio_negocio)),
            ("Vehículo", typeof(Vehiculo)),
        };
    }
}
