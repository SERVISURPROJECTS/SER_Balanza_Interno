using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Controllers
{
    /// <summary>
    /// Resolucion "buscar o crear" para los catalogos que se cargan como texto libre
    /// desde pantallas operativas (Peso, Analisis), capturando el Id generado para usarlo como FK.
    /// </summary>
    public static class CatalogoHelper
    {
        public static int SiguienteId<T>(IRepository<T> repo, Func<T, int> selector) where T : class
        {
            var items = repo.GetAll();
            return items.Count == 0 ? 1 : items.Max(selector) + 1;
        }

        public static int ObtenerOCrearVehiculo(IUnitOfWork uow, string placa)
        {
            placa = placa.Trim().ToUpperInvariant();
            var repo = uow.Repository<Vehiculo>();
            var existente = repo.Query().FirstOrDefault(v => v.Placa.ToUpper() == placa);
            if (existente != null) return existente.Id;

            var nuevo = new Vehiculo { Id = SiguienteId(repo, v => v.Id), Placa = placa, Activo = true };
            repo.Add(nuevo);
            return nuevo.Id;
        }

        public static int ObtenerOCrearChofer(IUnitOfWork uow, string nombre)
        {
            nombre = nombre.Trim();
            var repo = uow.Repository<chofer>();
            var existente = repo.Query().FirstOrDefault(c => c.nombre == nombre);
            if (existente != null) return existente.id;

            var nuevo = new chofer { id = SiguienteId(repo, c => c.id), nombre = nombre, activo = true };
            repo.Add(nuevo);
            return nuevo.id;
        }

        public static Producto ObtenerOCrearProducto(IUnitOfWork uow, string nombre)
        {
            nombre = nombre.Trim();
            var repo = uow.Repository<Producto>();
            var existente = repo.Query().FirstOrDefault(p => p.nombre == nombre);
            if (existente != null) return existente;

            var nuevo = new Producto { Id = SiguienteId(repo, p => p.Id), nombre = nombre, activo = true, HabilitarParametro = false };
            repo.Add(nuevo);
            return nuevo;
        }

        public static int ObtenerOCrearSocioNegocio(IUnitOfWork uow, string nombre, bool esCliente = false, bool esProveedor = false)
        {
            nombre = nombre.Trim();
            var repo = uow.Repository<socio_negocio>();
            var existente = repo.Query().FirstOrDefault(s => s.nombre == nombre);
            if (existente != null)
            {
                if (esCliente && !existente.es_cliente) existente.es_cliente = true;
                if (esProveedor && !existente.es_proveedor) existente.es_proveedor = true;
                return existente.id;
            }

            var nuevo = new socio_negocio
            {
                id = SiguienteId(repo, s => s.id),
                nombre = nombre,
                es_cliente = esCliente,
                es_proveedor = esProveedor,
                activo = true
            };
            repo.Add(nuevo);
            return nuevo.id;
        }

        public static int ObtenerOCrearOrigenDestino(IUnitOfWork uow, string nombre, bool esDestino)
        {
            nombre = nombre.Trim();
            var repo = uow.Repository<origen_destino>();
            var existente = repo.Query().FirstOrDefault(o => o.nombre == nombre && o.es_destino == esDestino);
            if (existente != null) return existente.id;

            var nuevo = new origen_destino { id = SiguienteId(repo, o => o.id), nombre = nombre, es_destino = esDestino, activo = true };
            repo.Add(nuevo);
            return nuevo.id;
        }

        public static int ObtenerOCrearHacienda(IUnitOfWork uow, string nombre)
        {
            nombre = nombre.Trim();
            var repo = uow.Repository<Hacienda>();
            var existente = repo.Query().FirstOrDefault(h => h.nombre == nombre);
            if (existente != null) return existente.Id;

            var nuevo = new Hacienda { Id = SiguienteId(repo, h => h.Id), nombre = nombre, Activo = true, FechaCreacion = DateTime.Now };
            repo.Add(nuevo);
            return nuevo.Id;
        }

        public static int ObtenerOCrearCampania(IUnitOfWork uow, string nombre, int usuarioId)
        {
            nombre = nombre.Trim();
            var repo = uow.Repository<campania>();
            var existente = repo.Query().FirstOrDefault(c => c.nombre == nombre);
            if (existente != null) return existente.id;

            var hoy = DateTime.Today;
            var nuevo = new campania
            {
                id = SiguienteId(repo, c => c.id),
                nombre = nombre,
                sigla = nombre.Length <= 10 ? nombre : nombre[..10],
                activo = true,
                valido_desde = hoy,
                valido_hasta = hoy.AddYears(1),
                fecha_creacion = DateTime.Now,
                id_usuario = usuarioId
            };
            repo.Add(nuevo);
            return nuevo.id;
        }

        /// <summary>
        /// El documento queda "casado" a la balanza activa de la sesion: se busca/crea por
        /// nombre + balanza, para no mezclar el contador/registro de documentos entre balanzas distintas.
        /// </summary>
        public static int ObtenerOCrearDocumentoPesaje(IUnitOfWork uow, int usuarioId, string? nombreTipeado = null, int? idBalanza = null)
        {
            var nombreDocumento = string.IsNullOrWhiteSpace(nombreTipeado) ? "PESAJE BALANZA" : nombreTipeado.Trim();
            var repo = uow.Repository<documento>();
            var existente = repo.Query().FirstOrDefault(d => d.nombre == nombreDocumento && d.Id_balanza == idBalanza);
            if (existente != null) return existente.id;

            var nuevo = new documento
            {
                id = SiguienteId(repo, d => d.id),
                nombre = nombreDocumento,
                activo = true,
                fecha_creacion = DateTime.Now,
                id_usuario = usuarioId,
                modo_transaccion = "E",
                Id_balanza = idBalanza
            };
            repo.Add(nuevo);
            return nuevo.id;
        }
    }
}
