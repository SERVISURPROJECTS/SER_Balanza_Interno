using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Controllers
{
    public class BoletaFactorData
    {
        public string Nombre { get; set; } = string.Empty;
        public double Parametro { get; set; }
        public double Analisis { get; set; }
        public double DescuentoPorcentaje { get; set; }
        public double DescuentoPeso { get; set; }
    }

    public class BoletaData
    {
        public string CompaniaNombre { get; set; } = string.Empty;
        public string CompaniaDireccion { get; set; } = string.Empty;
        public string CompaniaTelefono { get; set; } = string.Empty;
        public string CompaniaEmail { get; set; } = string.Empty;

        public string ProductoNombre { get; set; } = string.Empty;
        /// <summary>Nombre literal del Documento (Tipo Documento) del pesaje, ej. "DESPACHO GRANO DE SORGO".</summary>
        public string TipoDocumento { get; set; } = string.Empty;
        public string NroRemito { get; set; } = string.Empty;
        public string Campania { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int NroDespacho { get; set; }
        public int TiqueteBalanza { get; set; }
        public int? AnalisisNro { get; set; }
        public bool PesoManual { get; set; }
        public string UsuarioRegistro { get; set; } = string.Empty;

        public string Cliente { get; set; } = string.Empty;
        public string CentroAcopio { get; set; } = string.Empty;
        public string Procedencia { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public string Conductor { get; set; } = string.Empty;
        public string ConductorCi { get; set; } = string.Empty;
        public string Vehiculo { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public string Observacion { get; set; } = string.Empty;

        public double PesoBruto { get; set; }
        public double PesoTara { get; set; }
        public double PesoNeto { get; set; }
        public double Descuentos { get; set; }
        public double PesoLiquido { get; set; }

        public List<BoletaFactorData> Factores { get; set; } = new();
        public double TotalDescPorcentaje { get; set; }
        public double TotalDescPeso { get; set; }
    }

    public class BoletaController
    {
        private static readonly string[] NombresFactores =
        {
            "Humedad", "Impurezas", "Partido", "Dañado", "Grano Otro Color", "Quemados", "Grano Enfermo", "Grano Verde"
        };

        public BoletaData? ObtenerDatosBoleta(int nroConsec)
        {
            using var uow = UnitOfWorkFactory.Create();

            var peso = uow.Repository<peso>().Query().FirstOrDefault(p => p.NroConsec == nroConsec);
            if (peso is null) return null;

            var compania = uow.Repository<compania>().Query().FirstOrDefault(c => c.activo);
            var balanza = peso.Id_balanza is null ? null : uow.Repository<balanza>().Query().FirstOrDefault(b => b.id == peso.Id_balanza);
            var vehiculo = peso.id_Vehiculo is null ? null : uow.Repository<Vehiculo>().Query().FirstOrDefault(v => v.Id == peso.id_Vehiculo);
            var chofer = peso.id_Chofer is null ? null : uow.Repository<chofer>().Query().FirstOrDefault(c => c.id == peso.id_Chofer);
            var cliente = peso.Id_cliente is null ? null : uow.Repository<socio_negocio>().Query().FirstOrDefault(s => s.id == peso.Id_cliente);
            var proveedor = peso.id_Proveedor is null ? null : uow.Repository<socio_negocio>().Query().FirstOrDefault(s => s.id == peso.id_Proveedor);
            var procedencia = peso.id_origen is null ? null : uow.Repository<origen_destino>().Query().FirstOrDefault(o => o.id == peso.id_origen);
            var destino = peso.id_destino is null ? null : uow.Repository<origen_destino>().Query().FirstOrDefault(o => o.id == peso.id_destino);
            var producto = peso.Id_producto is null ? null : uow.Repository<Producto>().Query().FirstOrDefault(p => p.Id == peso.Id_producto);
            var documentoTipo = uow.Repository<documento>().Query().FirstOrDefault(doc => doc.id == peso.id_documento);
            var campania = peso.id_campania is null ? null : uow.Repository<campania>().Query().FirstOrDefault(c => c.id == peso.id_campania);
            var analisis = uow.Repository<Analisis>().Query().Where(a => a.PesoId == peso.NroConsec).OrderByDescending(a => a.DocEntry).FirstOrDefault();
            var usuarioRegistro = uow.Repository<Usuario>().Query().FirstOrDefault(u => u.Id == peso.Id_usuarioIng);

            double ParametroDe(Func<Analisis, double?> selector) => analisis is null ? 0 : selector(analisis) ?? 0;

            var factores = new List<BoletaFactorData>
            {
                new() { Nombre = NombresFactores[0], Parametro = ParametroDe(a => a.PHumedad), Analisis = ParametroDe(a => a.Humedad), DescuentoPorcentaje = ParametroDe(a => a.Desc_Humedad), DescuentoPeso = ParametroDe(a => a.DescWeightHumedad) },
                new() { Nombre = NombresFactores[1], Parametro = ParametroDe(a => a.PImpureza), Analisis = ParametroDe(a => a.Impureza), DescuentoPorcentaje = ParametroDe(a => a.Desc_Impureza), DescuentoPeso = ParametroDe(a => a.DescWeightImpureza) },
                new() { Nombre = NombresFactores[2], Parametro = ParametroDe(a => a.PPartido), Analisis = ParametroDe(a => a.Partido), DescuentoPorcentaje = ParametroDe(a => a.Desc_Partido), DescuentoPeso = ParametroDe(a => a.DescWeightPartido) },
                new() { Nombre = NombresFactores[3], Parametro = ParametroDe(a => a.PDanado), Analisis = ParametroDe(a => a.Danado), DescuentoPorcentaje = ParametroDe(a => a.Desc_Danado), DescuentoPeso = ParametroDe(a => a.DescWeightDanado) },
                new() { Nombre = NombresFactores[4], Parametro = ParametroDe(a => a.POtroColor), Analisis = ParametroDe(a => a.OtroColor), DescuentoPorcentaje = ParametroDe(a => a.Desc_OtroColor), DescuentoPeso = ParametroDe(a => a.DescWeightOtroColor) },
                new() { Nombre = NombresFactores[5], Parametro = ParametroDe(a => a.PDCalor), Analisis = ParametroDe(a => a.DCalor), DescuentoPorcentaje = ParametroDe(a => a.Desc_DCalor), DescuentoPeso = ParametroDe(a => a.DescWeightDCalor) },
                new() { Nombre = NombresFactores[6], Parametro = ParametroDe(a => a.PEnfermo), Analisis = ParametroDe(a => a.Enfermo), DescuentoPorcentaje = ParametroDe(a => a.Desc_Enfermo), DescuentoPeso = ParametroDe(a => a.DescWeightEnfermo) },
                new() { Nombre = NombresFactores[7], Parametro = ParametroDe(a => a.PVerde), Analisis = ParametroDe(a => a.Verde), DescuentoPorcentaje = ParametroDe(a => a.Desc_Verde), DescuentoPeso = ParametroDe(a => a.DescWeightVerde) },
            };

            // Descuentos y Peso Líquido son editables a mano en el formulario de pesaje (campos
            // peso.TotalDesc / peso.PesoLiquido): la boleta imprime exactamente ese valor guardado,
            // no lo recalcula a partir del Analisis (que puede estar vinculado a otro pesaje o
            // desactualizado). Si el pesaje es viejo y nunca se cargaron esos campos, se cae al
            // calculo historico desde Analisis como respaldo.
            var totalDescPeso = peso.TotalDesc ?? analisis?.TotalDescPeso ?? 0;
            var pesoLiquido = peso.PesoLiquido ?? (peso.Neto - totalDescPeso);

            return new BoletaData
            {
                // El encabezado muestra la balanza usada en el pesaje (nombre/direccion propios de esa
                // balanza), no la compania generica; esta ultima solo queda como respaldo si el pesaje
                // no tiene balanza asignada (registros previos a esta funcionalidad).
                CompaniaNombre = balanza?.Descripcion is { Length: > 0 } ? balanza.Descripcion : (compania?.nombre ?? "SERVISUR"),
                CompaniaDireccion = balanza?.Direccion is { Length: > 0 } ? balanza.Direccion : (compania?.direccion1 ?? ""),
                CompaniaTelefono = balanza?.Telefono is { Length: > 0 } ? balanza.Telefono : (compania?.telefono ?? ""),
                CompaniaEmail = balanza?.email is { Length: > 0 } ? balanza.email : (compania?.email ?? ""),

                ProductoNombre = producto?.nombre ?? "",
                TipoDocumento = documentoTipo?.nombre ?? "",
                NroRemito = peso.NroTicket ?? "",
                Campania = campania?.nombre ?? "",
                Fecha = peso.FechaIngreso,
                NroDespacho = peso.NroPesaje,
                TiqueteBalanza = peso.NroConsec,
                AnalisisNro = analisis?.DocEntry,
                PesoManual = peso.peso_manual,
                UsuarioRegistro = usuarioRegistro?.Nombre ?? "",

                Cliente = cliente?.nombre ?? "",
                CentroAcopio = destino?.nombre ?? "",
                Procedencia = procedencia?.nombre ?? "",
                Proveedor = proveedor?.nombre ?? "",
                Conductor = chofer?.nombre ?? "",
                ConductorCi = chofer?.ci ?? "",
                Vehiculo = vehiculo is null ? "" : string.IsNullOrWhiteSpace(vehiculo.Tipo)
                    ? $"{vehiculo.Modelo} {vehiculo.color}".Trim()
                    : $"{vehiculo.Tipo} {vehiculo.color}".Trim(),
                Placa = vehiculo?.Placa ?? "",
                Observacion = peso.Observacion ?? "",

                PesoBruto = peso.Bruto,
                PesoTara = peso.Tara,
                PesoNeto = peso.Neto,
                Descuentos = totalDescPeso,
                PesoLiquido = pesoLiquido,

                Factores = factores,
                TotalDescPorcentaje = analisis?.TotalDescPorcent ?? 0,
                TotalDescPeso = totalDescPeso
            };
        }
    }
}
