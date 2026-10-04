using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Controllers
{
    public class PesajeInput
    {
        public string TipoDocumento { get; set; } = string.Empty;
        public string Remito { get; set; } = string.Empty;
        public string Hacienda { get; set; } = string.Empty;
        public string Campania { get; set; } = string.Empty;
        public string Vehiculo { get; set; } = string.Empty;
        public string Chofer { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Procedencia { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public double Bruto { get; set; }
        public double Tara { get; set; }
        /// <summary>Si no se indica, se calcula como Bruto - Tara.</summary>
        public double? Neto { get; set; }
        public double? TotalDescuento { get; set; }
        public double? PesoLiquido { get; set; }
        public string Observacion { get; set; } = string.Empty;
        /// <summary>Si esta activo, se omite la validacion de que Bruto sea mayor o igual a Tara.</summary>
        public bool ModoManual { get; set; }
    }

    public class PesajeFiltro
    {
        public string? Remito { get; set; }
        public string? TipoDocumento { get; set; }
        public string? Hacienda { get; set; }
        public string? Campania { get; set; }
        public string? Vehiculo { get; set; }
        public string? Chofer { get; set; }
        public string? Producto { get; set; }
        public string? Proveedor { get; set; }
        public string? Cliente { get; set; }
        public string? Procedencia { get; set; }
        public string? Destino { get; set; }

        public bool EstaVacio =>
            string.IsNullOrWhiteSpace(Remito) && string.IsNullOrWhiteSpace(TipoDocumento) &&
            string.IsNullOrWhiteSpace(Hacienda) && string.IsNullOrWhiteSpace(Campania) &&
            string.IsNullOrWhiteSpace(Vehiculo) && string.IsNullOrWhiteSpace(Chofer) &&
            string.IsNullOrWhiteSpace(Producto) && string.IsNullOrWhiteSpace(Proveedor) &&
            string.IsNullOrWhiteSpace(Cliente) && string.IsNullOrWhiteSpace(Procedencia) &&
            string.IsNullOrWhiteSpace(Destino);
    }

    public class PesajeDetalle
    {
        public int NroConsec { get; set; }
        public string TipoDocumento { get; set; } = string.Empty;
        public string Remito { get; set; } = string.Empty;
        public string Hacienda { get; set; } = string.Empty;
        public string Campania { get; set; } = string.Empty;
        public string Vehiculo { get; set; } = string.Empty;
        public string Chofer { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Procedencia { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public double Bruto { get; set; }
        public double Tara { get; set; }
        public double? Neto { get; set; }
        public double? TotalDescuento { get; set; }
        public double? PesoLiquido { get; set; }
        public string Observacion { get; set; } = string.Empty;
    }

    public class PesoController
    {
        public string? ErrorMessage { get; private set; }

        /// <summary>Resuelve los nombres de catalogo de un pesaje, para precargar el formulario al editarlo.</summary>
        public PesajeDetalle? ObtenerDetalle(int nroConsec)
        {
            using var uow = UnitOfWorkFactory.Create();
            var p = uow.Repository<peso>().Query().FirstOrDefault(x => x.NroConsec == nroConsec);
            if (p is null) return null;

            string NombreDe<T>(int? id, Func<int, T?> buscar, Func<T, string> nombre) where T : class
            {
                if (id is null) return string.Empty;
                var entidad = buscar(id.Value);
                return entidad is null ? string.Empty : nombre(entidad);
            }

            var repoVehiculo = uow.Repository<Vehiculo>();
            var repoChofer = uow.Repository<chofer>();
            var repoProducto = uow.Repository<Producto>();
            var repoSocio = uow.Repository<socio_negocio>();
            var repoOrigenDestino = uow.Repository<origen_destino>();
            var repoHacienda = uow.Repository<Hacienda>();
            var repoCampania = uow.Repository<campania>();
            var repoDocumento = uow.Repository<documento>();

            return new PesajeDetalle
            {
                NroConsec = p.NroConsec,
                TipoDocumento = NombreDe(p.id_documento, id => repoDocumento.Query().FirstOrDefault(x => x.id == id), x => x.nombre),
                Hacienda = NombreDe(p.Id_Hacienda, id => repoHacienda.Query().FirstOrDefault(x => x.Id == id), x => x.nombre),
                Campania = NombreDe(p.id_campania, id => repoCampania.Query().FirstOrDefault(x => x.id == id), x => x.nombre),
                Vehiculo = NombreDe(p.id_Vehiculo, id => repoVehiculo.Query().FirstOrDefault(x => x.Id == id), x => x.Placa),
                Chofer = NombreDe(p.id_Chofer, id => repoChofer.Query().FirstOrDefault(x => x.id == id), x => x.nombre),
                Producto = NombreDe(p.Id_producto, id => repoProducto.Query().FirstOrDefault(x => x.Id == id), x => x.nombre),
                Proveedor = NombreDe(p.id_Proveedor, id => repoSocio.Query().FirstOrDefault(x => x.id == id), x => x.nombre),
                Cliente = NombreDe(p.Id_cliente, id => repoSocio.Query().FirstOrDefault(x => x.id == id), x => x.nombre),
                Procedencia = NombreDe(p.id_origen, id => repoOrigenDestino.Query().FirstOrDefault(x => x.id == id), x => x.nombre),
                Destino = NombreDe(p.id_destino, id => repoOrigenDestino.Query().FirstOrDefault(x => x.id == id), x => x.nombre),
                Remito = p.NroTicket,
                Bruto = p.Bruto,
                Tara = p.Tara,
                Neto = p.Neto,
                TotalDescuento = p.TotalDesc,
                PesoLiquido = p.PesoLiquido,
                Observacion = p.Observacion
            };
        }

        public List<peso> GetUltimos(int cantidad = 25)
        {
            using var uow = UnitOfWorkFactory.Create();
            var query = FiltrarPorBalanzaActual(uow.Repository<peso>().Query());
            return query
                .OrderByDescending(p => p.NroConsec)
                .Take(cantidad)
                .ToList();
        }

        /// <summary>
        /// Los pesajes de la balanza activa no deben mezclarse con los de otra balanza: si hay una
        /// balanza seleccionada en la sesion, se restringe la consulta a esa balanza unicamente.
        /// </summary>
        private static IQueryable<peso> FiltrarPorBalanzaActual(IQueryable<peso> query)
        {
            var idBalanza = Sesion.BalanzaActual?.id;
            return idBalanza is null ? query : query.Where(p => p.Id_balanza == idBalanza);
        }

        /// <summary>Busca pesajes combinando cualquiera de los filtros provistos (principalmente N° Remito).</summary>
        public List<peso> Buscar(PesajeFiltro filtro, int maximo = 200)
        {
            using var uow = UnitOfWorkFactory.Create();
            var query = FiltrarPorBalanzaActual(uow.Repository<peso>().Query());

            if (!string.IsNullOrWhiteSpace(filtro.Remito))
                query = query.Where(p => p.NroTicket != null && p.NroTicket.Contains(filtro.Remito));

            List<int>? IdsPorNombre<T>(string? texto, IEnumerable<T> items, Func<T, string> nombre, Func<T, int> id)
            {
                if (string.IsNullOrWhiteSpace(texto)) return null;
                return items.Where(x => nombre(x).Contains(texto, StringComparison.OrdinalIgnoreCase)).Select(id).ToList();
            }

            var idsVehiculo = IdsPorNombre(filtro.Vehiculo, uow.Repository<Vehiculo>().GetAll(), v => v.Placa, v => v.Id);
            if (idsVehiculo is not null) query = query.Where(p => p.id_Vehiculo != null && idsVehiculo.Contains(p.id_Vehiculo.Value));

            var idsChofer = IdsPorNombre(filtro.Chofer, uow.Repository<chofer>().GetAll(), c => c.nombre, c => c.id);
            if (idsChofer is not null) query = query.Where(p => p.id_Chofer != null && idsChofer.Contains(p.id_Chofer.Value));

            var idsProducto = IdsPorNombre(filtro.Producto, uow.Repository<Producto>().GetAll(), pr => pr.nombre, pr => pr.Id);
            if (idsProducto is not null) query = query.Where(p => p.Id_producto != null && idsProducto.Contains(p.Id_producto.Value));

            var idsCliente = IdsPorNombre(filtro.Cliente, uow.Repository<socio_negocio>().GetAll().Where(s => s.es_cliente), s => s.nombre, s => s.id);
            if (idsCliente is not null) query = query.Where(p => p.Id_cliente != null && idsCliente.Contains(p.Id_cliente.Value));

            var idsProveedor = IdsPorNombre(filtro.Proveedor, uow.Repository<socio_negocio>().GetAll().Where(s => s.es_proveedor), s => s.nombre, s => s.id);
            if (idsProveedor is not null) query = query.Where(p => p.id_Proveedor != null && idsProveedor.Contains(p.id_Proveedor.Value));

            var idsProcedencia = IdsPorNombre(filtro.Procedencia, uow.Repository<origen_destino>().GetAll().Where(o => !o.es_destino), o => o.nombre, o => o.id);
            if (idsProcedencia is not null) query = query.Where(p => p.id_origen != null && idsProcedencia.Contains(p.id_origen.Value));

            var idsDestino = IdsPorNombre(filtro.Destino, uow.Repository<origen_destino>().GetAll().Where(o => o.es_destino), o => o.nombre, o => o.id);
            if (idsDestino is not null) query = query.Where(p => p.id_destino != null && idsDestino.Contains(p.id_destino.Value));

            var idsHacienda = IdsPorNombre(filtro.Hacienda, uow.Repository<Hacienda>().GetAll(), h => h.nombre, h => h.Id);
            if (idsHacienda is not null) query = query.Where(p => p.Id_Hacienda != null && idsHacienda.Contains(p.Id_Hacienda.Value));

            var idsCampania = IdsPorNombre(filtro.Campania, uow.Repository<campania>().GetAll(), c => c.nombre, c => c.id);
            if (idsCampania is not null) query = query.Where(p => p.id_campania != null && idsCampania.Contains(p.id_campania.Value));

            var idsDocumento = IdsPorNombre(filtro.TipoDocumento, uow.Repository<documento>().GetAll(), d => d.nombre, d => d.id);
            if (idsDocumento is not null) query = query.Where(p => idsDocumento.Contains(p.id_documento));

            return query.OrderByDescending(p => p.NroConsec).Take(maximo).ToList();
        }

        public int? Guardar(PesajeInput input, int usuarioId) => GuardarInterno(input, usuarioId, null);

        /// <summary>Actualiza un pesaje existente (requiere que quien llame ya haya validado el permiso de edición del rol).</summary>
        public bool Actualizar(int nroConsec, PesajeInput input, int usuarioId) => GuardarInterno(input, usuarioId, nroConsec) is not null;

        private int? GuardarInterno(PesajeInput input, int usuarioId, int? nroConsecExistente)
        {
            ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(input.Vehiculo))
            {
                ErrorMessage = "La placa del vehículo es obligatoria.";
                return null;
            }

            if (!input.ModoManual && (input.Bruto <= 0 || input.Tara < 0 || input.Bruto < input.Tara))
            {
                ErrorMessage = "Los pesos bruto/tara no son válidos.";
                return null;
            }

            using var uow = UnitOfWorkFactory.Create();
            var repoPeso = uow.Repository<peso>();

            peso? existente = null;
            if (nroConsecExistente is not null)
            {
                existente = repoPeso.Query().FirstOrDefault(p => p.NroConsec == nroConsecExistente.Value);
                if (existente is null)
                {
                    ErrorMessage = "El pesaje a actualizar no existe.";
                    return null;
                }
            }

            try
            {
                int? idVehiculo = string.IsNullOrWhiteSpace(input.Vehiculo) ? null : CatalogoHelper.ObtenerOCrearVehiculo(uow, input.Vehiculo);
                int? idChofer = string.IsNullOrWhiteSpace(input.Chofer) ? null : CatalogoHelper.ObtenerOCrearChofer(uow, input.Chofer);
                int? idProducto = string.IsNullOrWhiteSpace(input.Producto) ? null : CatalogoHelper.ObtenerOCrearProducto(uow, input.Producto).Id;
                int? idProveedor = string.IsNullOrWhiteSpace(input.Proveedor) ? null : CatalogoHelper.ObtenerOCrearSocioNegocio(uow, input.Proveedor, esProveedor: true);
                int? idCliente = string.IsNullOrWhiteSpace(input.Cliente) ? null : CatalogoHelper.ObtenerOCrearSocioNegocio(uow, input.Cliente, esCliente: true);
                int? idOrigen = string.IsNullOrWhiteSpace(input.Procedencia) ? null : CatalogoHelper.ObtenerOCrearOrigenDestino(uow, input.Procedencia, esDestino: false);
                int? idDestino = string.IsNullOrWhiteSpace(input.Destino) ? null : CatalogoHelper.ObtenerOCrearOrigenDestino(uow, input.Destino, esDestino: true);
                int? idHacienda = string.IsNullOrWhiteSpace(input.Hacienda) ? null : CatalogoHelper.ObtenerOCrearHacienda(uow, input.Hacienda);
                int? idCampania = string.IsNullOrWhiteSpace(input.Campania) ? null : CatalogoHelper.ObtenerOCrearCampania(uow, input.Campania, usuarioId);
                var idBalanzaActual = Sesion.BalanzaActual?.id;
                int idDocumento = CatalogoHelper.ObtenerOCrearDocumentoPesaje(uow, usuarioId, input.TipoDocumento, idBalanzaActual);

                var ahora = DateTime.Now;
                var entidad = existente ?? new peso
                {
                    NroConsec = CatalogoHelper.SiguienteId(repoPeso, p => p.NroConsec),
                    FechaIngreso = ahora,
                    Id_usuarioIng = usuarioId,
                };
                entidad.NroPesaje = entidad.NroConsec;
                entidad.FechaSalida = ahora;
                entidad.Bruto = input.Bruto;
                entidad.Tara = input.Tara;
                entidad.Neto = input.Neto ?? (input.Bruto - input.Tara);
                entidad.TotalDesc = input.TotalDescuento ?? 0;
                entidad.PesoLiquido = input.PesoLiquido ?? entidad.Neto;
                entidad.NroTicket = input.Remito ?? string.Empty;
                entidad.UnidadPrimaria = "KG";
                entidad.Id_UsuarioSal = usuarioId;
                entidad.id_Proveedor = idProveedor;
                entidad.id_Chofer = idChofer;
                entidad.id_Vehiculo = idVehiculo;
                entidad.Id_producto = idProducto;
                entidad.Id_cliente = idCliente;
                entidad.id_origen = idOrigen;
                entidad.id_destino = idDestino;
                entidad.Id_Hacienda = idHacienda;
                entidad.id_campania = idCampania;
                entidad.id_documento = idDocumento;
                entidad.Id_balanza = idBalanzaActual;
                entidad.Observacion = input.Observacion ?? string.Empty;
                entidad.Nulo = false;
                entidad.peso_manual = true;

                if (existente is null)
                {
                    repoPeso.Add(entidad);
                }
                uow.SaveChanges();
                return entidad.NroConsec;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"No se pudo guardar el pesaje: {ex.Message}";
                return null;
            }
        }
    }
}
