using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Controllers
{
    public class FactorInput
    {
        public string Nombre { get; set; } = string.Empty;
        public double Parametro { get; set; }
        public double FactorDescuento { get; set; }
        public double Analisis { get; set; }
        /// <summary>Por ahora se escriben a mano en la grilla (sin formula automatica).</summary>
        public double DescuentoPorcentaje { get; set; }
        public double DescuentoPeso { get; set; }
    }

    public class AnalisisInput
    {
        public int NroPesaje { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public DateTime FechaDoc { get; set; } = DateTime.Now;
        public List<FactorInput> Factores { get; set; } = new();
        /// <summary>Total de descuento %; se recalcula en pantalla pero el usuario puede sobreescribirlo a mano.</summary>
        public double TotalDescuentoPorcentaje { get; set; }
        /// <summary>Total de descuento en peso; se recalcula en pantalla pero el usuario puede sobreescribirlo a mano.</summary>
        public double TotalDescuentoPeso { get; set; }
        /// <summary>
        /// Peso Liquido mostrado/editado en la pantalla de Analisis: se precarga con el del Peso (si ya
        /// tenia uno cargado) o se recalcula como Peso Neto - Descuento Total, pero el usuario puede
        /// sobreescribirlo a mano antes de guardar. Al guardar, este valor queda como el definitivo y
        /// se propaga al pesaje (ver GuardarInterno).
        /// </summary>
        public double? PesoLiquido { get; set; }
    }

    public class AnalisisResultado
    {
        public int DocEntry { get; set; }
        public double TotalDescuentoPorcentaje { get; set; }
        public double TotalDescuentoPeso { get; set; }
    }

    public class AnalisisController
    {
        public string? ErrorMessage { get; private set; }

        /// <summary>Busca un pesaje existente por su N° consecutivo, necesario para vincular el analisis.</summary>
        public peso? BuscarPeso(int nroConsec)
        {
            using var uow = UnitOfWorkFactory.Create();
            return uow.Repository<peso>().Query().FirstOrDefault(p => p.NroConsec == nroConsec);
        }

        /// <summary>Busca el pesaje mas reciente que tenga ese N° de Remito, para poder buscar el analisis desde cualquiera de los dos.</summary>
        public peso? BuscarPesoPorRemito(string remito)
        {
            using var uow = UnitOfWorkFactory.Create();
            return uow.Repository<peso>().Query()
                .Where(p => p.NroTicket == remito)
                .OrderByDescending(p => p.NroConsec)
                .FirstOrDefault();
        }

        /// <summary>Ultimo analisis registrado para un pesaje (si existe), para decidir si se puede seguir editando (Estado "O") o no (Estado "C").</summary>
        public Analisis? BuscarPorPeso(int nroPesaje)
        {
            using var uow = UnitOfWorkFactory.Create();
            return uow.Repository<Analisis>().Query()
                .Where(a => a.PesoId == nroPesaje)
                .OrderByDescending(a => a.DocEntry)
                .FirstOrDefault();
        }

        /// <summary>Vista previa del DocEntry que se asignaria si se guarda un analisis nuevo ahora mismo.</summary>
        public int ObtenerSiguienteDocEntry()
        {
            using var uow = UnitOfWorkFactory.Create();
            return CatalogoHelper.SiguienteId(uow.Repository<Analisis>(), a => a.DocEntry);
        }

        /// <summary>Resuelve (o crea) el producto por nombre y devuelve sus parametros/factores de descuento configurados.</summary>
        public Producto ObtenerProducto(string nombre)
        {
            using var uow = UnitOfWorkFactory.Create();
            var producto = CatalogoHelper.ObtenerOCrearProducto(uow, nombre);
            uow.SaveChanges();
            return producto;
        }

        public static double CalcularDescuentoPorcentaje(FactorInput factor)
        {
            var exceso = factor.Analisis - factor.Parametro;
            if (exceso <= 0) return 0;
            return Math.Round(exceso * factor.FactorDescuento, 3);
        }

        public AnalisisResultado? Guardar(AnalisisInput input, int usuarioId) => GuardarInterno(input, usuarioId, null);

        /// <summary>Actualiza un analisis existente; falla si su Estado es "C" (cerrado).</summary>
        public AnalisisResultado? Actualizar(int docEntry, AnalisisInput input, int usuarioId) => GuardarInterno(input, usuarioId, docEntry);

        private AnalisisResultado? GuardarInterno(AnalisisInput input, int usuarioId, int? docEntryExistente)
        {
            ErrorMessage = null;

            using var uow = UnitOfWorkFactory.Create();

            var pesoRepo = uow.Repository<peso>();
            var pesoRef = pesoRepo.Query().FirstOrDefault(p => p.NroConsec == input.NroPesaje);
            if (pesoRef is null)
            {
                ErrorMessage = $"No existe un pesaje con N° {input.NroPesaje}.";
                return null;
            }

            if (string.IsNullOrWhiteSpace(input.Producto))
            {
                ErrorMessage = "El producto es obligatorio.";
                return null;
            }

            var repoAnalisis = uow.Repository<Analisis>();
            Analisis? existente = null;
            if (docEntryExistente is not null)
            {
                existente = repoAnalisis.Query().FirstOrDefault(a => a.DocEntry == docEntryExistente.Value);
                if (existente is null)
                {
                    ErrorMessage = "El análisis a actualizar no existe.";
                    return null;
                }
            }

            try
            {
                var idCliente = string.IsNullOrWhiteSpace(input.Cliente)
                    ? pesoRef.Id_cliente
                    : CatalogoHelper.ObtenerOCrearSocioNegocio(uow, input.Cliente, esCliente: true);

                var producto = CatalogoHelper.ObtenerOCrearProducto(uow, input.Producto);

                // Por ahora Descuento % y Descuento se toman tal cual se escribieron en la grilla (sin formula automatica).
                double PorcentajeDe(string nombre) => input.Factores.FirstOrDefault(f => f.Nombre == nombre)?.DescuentoPorcentaje ?? 0;
                double PesoDescuentoDe(string nombre) => input.Factores.FirstOrDefault(f => f.Nombre == nombre)?.DescuentoPeso ?? 0;
                double AnalisisDe(string nombre) => input.Factores.FirstOrDefault(f => f.Nombre == nombre)?.Analisis ?? 0;
                double ParametroDe(string nombre) => input.Factores.FirstOrDefault(f => f.Nombre == nombre)?.Parametro ?? 0;

                var descHumedad = PorcentajeDe("Humedad");
                var descImpureza = PorcentajeDe("Impureza");
                var descPartido = PorcentajeDe("Partido");
                var descDanado = PorcentajeDe("Dañado");
                var descOtroColor = PorcentajeDe("Otro Color");
                var descDCalor = PorcentajeDe("Dañado por Calor");
                var descEnfermo = PorcentajeDe("Enfermo");
                var descVerde = PorcentajeDe("Verde");

                var nuevo = existente ?? new Analisis
                {
                    DocEntry = CatalogoHelper.SiguienteId(repoAnalisis, a => a.DocEntry),
                    Cancelado = "N",
                    Estado = "O",
                    UsuarioIdReg = usuarioId,
                    FechaReg = DateTime.Now,
                };
                nuevo.DocNum = nuevo.DocEntry;
                nuevo.PesoId = pesoRef.NroConsec;
                nuevo.ProductoId = producto.Id;
                nuevo.ClienteId = idCliente;
                nuevo.FechaDoc = input.FechaDoc;
                if (existente is not null)
                {
                    nuevo.UsuarioIdAct = usuarioId;
                    nuevo.FechaAct = DateTime.Now;
                }
                nuevo.Unidad = "KG";

                nuevo.PHumedad = ParametroDe("Humedad");
                nuevo.PImpureza = ParametroDe("Impureza");
                nuevo.PPartido = ParametroDe("Partido");
                nuevo.PDanado = ParametroDe("Dañado");
                nuevo.POtroColor = ParametroDe("Otro Color");
                nuevo.PDCalor = ParametroDe("Dañado por Calor");
                nuevo.PEnfermo = ParametroDe("Enfermo");
                nuevo.PVerde = ParametroDe("Verde");

                nuevo.Humedad = AnalisisDe("Humedad");
                nuevo.Impureza = AnalisisDe("Impureza");
                nuevo.Partido = AnalisisDe("Partido");
                nuevo.Danado = AnalisisDe("Dañado");
                nuevo.OtroColor = AnalisisDe("Otro Color");
                nuevo.DCalor = AnalisisDe("Dañado por Calor");
                nuevo.Enfermo = AnalisisDe("Enfermo");
                nuevo.Verde = AnalisisDe("Verde");

                nuevo.Desc_Humedad = descHumedad;
                nuevo.Desc_Impureza = descImpureza;
                nuevo.Desc_Partido = descPartido;
                nuevo.Desc_Danado = descDanado;
                nuevo.Desc_OtroColor = descOtroColor;
                nuevo.Desc_DCalor = descDCalor;
                nuevo.Desc_Enfermo = descEnfermo;
                nuevo.Desc_Verde = descVerde;

                nuevo.DescWeightHumedad = PesoDescuentoDe("Humedad");
                nuevo.DescWeightImpureza = PesoDescuentoDe("Impureza");
                nuevo.DescWeightPartido = PesoDescuentoDe("Partido");
                nuevo.DescWeightDanado = PesoDescuentoDe("Dañado");
                nuevo.DescWeightOtroColor = PesoDescuentoDe("Otro Color");
                nuevo.DescWeightDCalor = PesoDescuentoDe("Dañado por Calor");
                nuevo.DescWeightEnfermo = PesoDescuentoDe("Enfermo");
                nuevo.DescWeightVerde = PesoDescuentoDe("Verde");

                // El total se toma del valor mostrado en pantalla: se recalcula automaticamente a partir
                // de las filas, pero el usuario puede sobreescribirlo a mano antes de guardar.
                nuevo.TotalDescPorcent = Math.Round(input.TotalDescuentoPorcentaje, 3);
                nuevo.TotalDescPeso = Math.Round(input.TotalDescuentoPeso, 2);

                // Al guardar, el analisis queda cerrado; para volver a editarlo hay que reabrirlo
                // explicitamente desde la pantalla (cambia el Estado a "O").
                nuevo.Estado = "C";

                if (existente is null)
                {
                    repoAnalisis.Add(nuevo);
                }

                // El analisis suele calcularse despues de guardar el pesaje: se propaga el descuento
                // y el peso liquido resultantes al pesaje para que el formulario de Peso y la boleta
                // los muestren sin tener que volver a abrir/reguardar el pesaje a mano. El Peso Liquido
                // es bidireccional: si el usuario lo sobreescribio a mano en la pantalla de Analisis,
                // ese valor (input.PesoLiquido) es el que prevalece y se guarda en el pesaje; si no lo
                // toco, se usa el calculo Peso Neto - Descuento Total. Si el usuario edita estos campos
                // manualmente en el formulario de Peso despues, esa edicion manual prevalece (se vuelve
                // a sobreescribir aqui solo si se vuelve a guardar el analisis).
                pesoRef.TotalDesc = nuevo.TotalDescPeso;
                pesoRef.PesoLiquido = input.PesoLiquido ?? (pesoRef.Neto - (nuevo.TotalDescPeso ?? 0));

                uow.SaveChanges();

                return new AnalisisResultado
                {
                    DocEntry = nuevo.DocEntry,
                    TotalDescuentoPorcentaje = nuevo.TotalDescPorcent ?? 0,
                    TotalDescuentoPeso = nuevo.TotalDescPeso ?? 0
                };
            }
            catch (Exception ex)
            {
                ErrorMessage = $"No se pudo guardar el análisis: {ex.Message}";
                return null;
            }
        }
    }
}
