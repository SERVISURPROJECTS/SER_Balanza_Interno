using SER_Balanza_Interno.Controllers;
using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Views
{
    public partial class FrmAnalisis : Form
    {
        private readonly AnalisisController _controller = new();
        private readonly BoletaController _boletaController = new();
        private peso? _pesoActual;
        private Analisis? _analisisExistente;
        private int? _nroPesajeGuardado;
        private bool _bloqueado;
        /// <summary>
        /// Modo Manual (checkbox): deja registrar Total Descuento %, Total Descuento y Peso Liquido
        /// a mano, sin que se recalculen solos al editar la grilla de factores.
        /// </summary>
        private bool _modoManual;

        private static readonly string[] NombresFactores =
        {
            "Humedad", "Impureza", "Partido", "Dañado", "Otro Color", "Dañado por Calor", "Enfermo", "Verde"
        };

        public FrmAnalisis()
        {
            InitializeComponent();
            InicializarGridFactores();
            CargarAutocompletarClientes();
            lblEstadoValor.Text = "O";
            lblEstadoValor.Cursor = Cursors.Hand;
            lblEstadoValor.Click += LblEstadoValor_Click;
            txtNroRemito.Leave += TxtNroRemito_Leave;
        }

        public FrmAnalisis(int nroPesajeInicial) : this()
        {
            txtNroPesaje.Text = nroPesajeInicial.ToString();
            CargarPesaje(nroPesajeInicial);
        }

        private void InicializarGridFactores()
        {
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Factor", HeaderText = "Factores", ReadOnly = true, Width = 140 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Parametro", HeaderText = "Parámetros", Width = 100 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Analisis", HeaderText = "Análisis", Width = 100 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "DescuentoPorcentaje", HeaderText = "Descuento %", Width = 110 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Descuento", HeaderText = "Descuento", Width = 100 });

            foreach (var nombre in NombresFactores)
            {
                var fila = new DataGridViewRow();
                fila.CreateCells(grid, nombre, 0.0, 0.0, 0.0, 0.0);
                grid.Rows.Add(fila);
            }

            grid.CellEndEdit += (_, _) => RecalcularTotales();
        }

        private void CargarAutocompletarClientes()
        {
            using var uow = UnitOfWorkFactory.Create();
            cmbCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCliente.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbCliente.Items.Clear();
            cmbCliente.Items.AddRange(uow.Repository<socio_negocio>().GetAll()
                .Where(s => s.es_cliente).Select(s => s.nombre).Distinct().OrderBy(n => n).Cast<object>().ToArray());

            cmbProducto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbProducto.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbProducto.Items.Clear();
            cmbProducto.Items.AddRange(uow.Repository<Producto>().GetAll()
                .Select(p => p.nombre).Distinct().OrderBy(n => n).Cast<object>().ToArray());
        }

        private void txtNroPesaje_Leave(object sender, EventArgs e)
        {
            if (!int.TryParse(txtNroPesaje.Text.Trim(), out var nro))
            {
                return;
            }

            CargarPesaje(nro);
        }

        private void TxtNroRemito_Leave(object? sender, EventArgs e)
        {
            var remito = txtNroRemito.Text.Trim();
            if (string.IsNullOrEmpty(remito)) return;
            if (_pesoActual is not null && _pesoActual.NroTicket == remito) return;

            var pesoEncontrado = _controller.BuscarPesoPorRemito(remito);
            if (pesoEncontrado is null)
            {
                MessageBox.Show($"No existe un pesaje con N° de Remito '{remito}'.", "Análisis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtNroPesaje.Text = pesoEncontrado.NroConsec.ToString();
            CargarPesaje(pesoEncontrado.NroConsec);
        }

        /// <summary>Busca el pesaje por N° y precarga el analisis asociado (si existe). Publico para permitir
        /// el enlace cruzado "Ver Analisis" desde FrmPeso.</summary>
        public void CargarPesaje(int nro)
        {
            _pesoActual = _controller.BuscarPeso(nro);
            if (_pesoActual is null)
            {
                lblPesoNeto.Text = "0.00";
                txtNroRemito.Text = "";
                btnVerPeso.Enabled = false;
                MessageBox.Show($"No existe un pesaje con N° {nro}.", "Análisis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnVerPeso.Enabled = true;
            lblPesoNeto.Text = _pesoActual.Neto.ToString("N2");
            txtNroRemito.Text = _pesoActual.NroTicket;

            _analisisExistente = _controller.BuscarPorPeso(nro);
            if (_analisisExistente is not null)
            {
                CargarAnalisisExistente(_analisisExistente);
                AplicarBloqueoEdicion(_analisisExistente.Estado == "C");
                return;
            }

            lblNroAnalisisValor.Text = _controller.ObtenerSiguienteDocEntry().ToString();
            lblEstadoValor.Text = "O";
            lblEstadoValor.BackColor = Color.Red;
            chkModoManual.Checked = false;
            AplicarBloqueoEdicion(false);

            // No hay analisis todavia para este pesaje: si el pesaje ya tiene su propio Peso Liquido
            // cargado (desde FrmPeso), se usa como referencia; si no, se parte de Peso Neto (sin
            // descuento todavia).
            txtPesoLiquido.Text = (_pesoActual.PesoLiquido ?? _pesoActual.Neto).ToString("N2");

            if (_pesoActual.Id_cliente is not null && string.IsNullOrWhiteSpace(cmbCliente.Text))
            {
                using var uow = UnitOfWorkFactory.Create();
                var cliente = uow.Repository<socio_negocio>().Query().FirstOrDefault(s => s.id == _pesoActual.Id_cliente);
                if (cliente is not null) cmbCliente.Text = cliente.nombre;
            }

            if (_pesoActual.Id_producto is not null && string.IsNullOrWhiteSpace(cmbProducto.Text))
            {
                using var uow = UnitOfWorkFactory.Create();
                var producto = uow.Repository<Producto>().Query().FirstOrDefault(p => p.Id == _pesoActual.Id_producto);
                if (producto is not null)
                {
                    cmbProducto.Text = producto.nombre;
                    CargarParametrosProducto(producto);
                }
            }
        }

        private void CargarAnalisisExistente(Analisis analisis)
        {
            // Un analisis ya guardado puede tener sus totales sobreescritos a mano (ver comentario
            // mas abajo): se activa Modo Manual al reabrirlo para no recalcularlos solos apenas se
            // edite una celda de la grilla.
            chkModoManual.Checked = true;

            lblNroAnalisisValor.Text = analisis.DocEntry.ToString();
            lblEstadoValor.Text = analisis.Estado;
            lblEstadoValor.BackColor = analisis.Estado == "C" ? Color.Gray : Color.Red;
            dtFecha.Value = analisis.FechaDoc;

            // El analisis ya calculo su propio descuento total: el Peso Liquido se muestra segun ese
            // calculo (Peso Neto - Descuento Total), que es la fuente de verdad una vez que existe un
            // analisis (si no coincide con lo que tenia guardado el pesaje, este valor es el que manda).
            txtPesoLiquido.Text = ((_pesoActual?.Neto ?? 0) - (analisis.TotalDescPeso ?? 0)).ToString("N2");

            using var uow = UnitOfWorkFactory.Create();
            if (analisis.ClienteId is not null)
            {
                var cliente = uow.Repository<socio_negocio>().Query().FirstOrDefault(s => s.id == analisis.ClienteId);
                if (cliente is not null) cmbCliente.Text = cliente.nombre;
            }
            if (analisis.ProductoId is not null)
            {
                var producto = uow.Repository<Producto>().Query().FirstOrDefault(p => p.Id == analisis.ProductoId);
                if (producto is not null) cmbProducto.Text = producto.nombre;
            }

            void Fila(int fila, double? parametro, double? valorAnalisis, double? descuentoPorcentaje, double? descuentoPeso, double? factorDescuento)
            {
                grid.Rows[fila].Cells["Parametro"].Value = (parametro ?? 0).ToString("N2");
                grid.Rows[fila].Cells["Analisis"].Value = (valorAnalisis ?? 0).ToString("N2");
                grid.Rows[fila].Cells["DescuentoPorcentaje"].Value = (descuentoPorcentaje ?? 0).ToString("N2");
                grid.Rows[fila].Cells["Descuento"].Value = (descuentoPeso ?? 0).ToString("N2");
                grid.Rows[fila].Tag = factorDescuento ?? 1.0;
            }

            var producto2 = analisis.ProductoId is null ? null : uow.Repository<Producto>().Query().FirstOrDefault(p => p.Id == analisis.ProductoId);
            Fila(0, analisis.PHumedad, analisis.Humedad, analisis.Desc_Humedad, analisis.DescWeightHumedad, producto2?.FDHumedad);
            Fila(1, analisis.PImpureza, analisis.Impureza, analisis.Desc_Impureza, analisis.DescWeightImpureza, producto2?.FDImpureza);
            Fila(2, analisis.PPartido, analisis.Partido, analisis.Desc_Partido, analisis.DescWeightPartido, producto2?.FDPartido);
            Fila(3, analisis.PDanado, analisis.Danado, analisis.Desc_Danado, analisis.DescWeightDanado, producto2?.FDDanado);
            Fila(4, analisis.POtroColor, analisis.OtroColor, analisis.Desc_OtroColor, analisis.DescWeightOtroColor, producto2?.FDOtroColor);
            Fila(5, analisis.PDCalor, analisis.DCalor, analisis.Desc_DCalor, analisis.DescWeightDCalor, producto2?.FDDanadoPorCalor);
            Fila(6, analisis.PEnfermo, analisis.Enfermo, analisis.Desc_Enfermo, analisis.DescWeightEnfermo, producto2?.FDEnfermo);
            Fila(7, analisis.PVerde, analisis.Verde, analisis.Desc_Verde, analisis.DescWeightVerde, producto2?.FDVerde);

            // El total guardado puede haber sido sobreescrito a mano (no siempre es la suma de las filas),
            // asi que al recargar se muestra tal cual se guardo en vez de recalcularlo desde la grilla.
            lblTotalPorcentaje.Text = (analisis.TotalDescPorcent ?? 0).ToString("N3");
            lblTotalPeso.Text = (analisis.TotalDescPeso ?? 0).ToString("N2");
        }

        private void AplicarBloqueoEdicion(bool bloqueado)
        {
            _bloqueado = bloqueado;
            cmbCliente.Enabled = !bloqueado;
            cmbProducto.Enabled = !bloqueado;
            txtPesoLiquido.Enabled = !bloqueado;
            dtFecha.Enabled = !bloqueado;
            btnGuardar.Enabled = !bloqueado;
            lblTotalPorcentaje.Enabled = !bloqueado;
            lblTotalPeso.Enabled = !bloqueado;

            foreach (DataGridViewRow fila in grid.Rows)
            {
                fila.Cells["Parametro"].ReadOnly = bloqueado;
                fila.Cells["Analisis"].ReadOnly = bloqueado;
                fila.Cells["DescuentoPorcentaje"].ReadOnly = bloqueado;
                fila.Cells["Descuento"].ReadOnly = bloqueado;
            }
            grid.DefaultCellStyle.BackColor = bloqueado ? Color.WhiteSmoke : Color.White;
        }

        /// <summary>
        /// Un analisis cerrado ("C") se muestra bloqueado; al hacer clic en el Estado se puede
        /// reabrir (pasa a "O" solo en pantalla) para poder editarlo. Al guardar vuelve a quedar "C".
        /// </summary>
        private void LblEstadoValor_Click(object? sender, EventArgs e)
        {
            if (_analisisExistente?.Estado != "C" || !_bloqueado) return;

            var confirmar = MessageBox.Show(
                "Este análisis está cerrado. ¿Desea reabrirlo para editarlo?",
                "Análisis", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes) return;

            lblEstadoValor.Text = "O";
            lblEstadoValor.BackColor = Color.Red;
            AplicarBloqueoEdicion(false);
        }

        private void cmbProducto_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbProducto.Text)) return;
            var producto = _controller.ObtenerProducto(cmbProducto.Text.Trim());
            CargarParametrosProducto(producto);
        }

        private void CargarParametrosProducto(Producto producto)
        {
            SetParametro(0, producto.Humedad, producto.FDHumedad);
            SetParametro(1, producto.Impureza, producto.FDImpureza);
            SetParametro(2, producto.Partido, producto.FDPartido);
            SetParametro(3, producto.Danado, producto.FDDanado);
            SetParametro(4, producto.OtroColor, producto.FDOtroColor);
            SetParametro(5, producto.DanadoPorCalor, producto.FDDanadoPorCalor);
            SetParametro(6, producto.Enfermo, producto.FDEnfermo);
            SetParametro(7, producto.Verde, producto.FDVerde);
            RecalcularTotales();
        }

        private void SetParametro(int fila, double? parametro, double? factorDescuento)
        {
            grid.Rows[fila].Cells["Parametro"].Value = (parametro ?? 0).ToString("N2");
            grid.Rows[fila].Tag = factorDescuento ?? 1.0;
        }

        private List<FactorInput> LeerFactores()
        {
            var lista = new List<FactorInput>();
            foreach (DataGridViewRow fila in grid.Rows)
            {
                double.TryParse(Convert.ToString(fila.Cells["Parametro"].Value), out var parametro);
                double.TryParse(Convert.ToString(fila.Cells["Analisis"].Value), out var analisis);
                double.TryParse(Convert.ToString(fila.Cells["DescuentoPorcentaje"].Value), out var descuentoPorcentaje);
                double.TryParse(Convert.ToString(fila.Cells["Descuento"].Value), out var descuentoPeso);

                lista.Add(new FactorInput
                {
                    Nombre = Convert.ToString(fila.Cells["Factor"].Value) ?? "",
                    Parametro = parametro,
                    Analisis = analisis,
                    DescuentoPorcentaje = descuentoPorcentaje,
                    DescuentoPeso = descuentoPeso
                });
            }
            return lista;
        }

        /// <summary>
        /// Por ahora Descuento % y Descuento se escriben a mano (sin formula automatica);
        /// esto solo suma lo que ya esta cargado en la grilla para mostrar los totales.
        /// </summary>
        private void RecalcularTotales()
        {
            if (_modoManual) return;

            double totalPorcentaje = 0;
            double totalPeso = 0;

            foreach (DataGridViewRow fila in grid.Rows)
            {
                double.TryParse(Convert.ToString(fila.Cells["DescuentoPorcentaje"].Value), out var pct);
                double.TryParse(Convert.ToString(fila.Cells["Descuento"].Value), out var monto);
                totalPorcentaje += pct;
                totalPeso += monto;
            }

            lblTotalPorcentaje.Text = totalPorcentaje.ToString("N3");
            lblTotalPeso.Text = totalPeso.ToString("N2");

            if (_pesoActual is not null)
            {
                txtPesoLiquido.Text = (_pesoActual.Neto - totalPeso).ToString("N2");
            }
        }

        /// <summary>
        /// Al activar Modo Manual, Total Descuento %, Total Descuento y Peso Liquido quedan 100% a
        /// cargo del usuario: no se recalculan solos al editar la grilla de factores. Al desactivarlo,
        /// se recalculan de inmediato a partir de lo que haya cargado en la grilla.
        /// </summary>
        private void chkModoManual_CheckedChanged(object sender, EventArgs e)
        {
            _modoManual = chkModoManual.Checked;
            if (!_modoManual)
            {
                RecalcularTotales();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Si el usuario termino de escribir en una celda de la grilla y hizo clic directo en
            // Guardar (sin Enter/Tab/clic afuera), la edicion todavia no esta confirmada en
            // Cells[].Value; sin este EndEdit se guardaria el valor anterior de esa celda.
            grid.EndEdit();

            if (Sesion.UsuarioActual is null)
            {
                MessageBox.Show("No hay una sesión activa.", "Análisis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_bloqueado)
            {
                MessageBox.Show("El análisis está cerrado y no se puede editar. Haga clic en el Estado para reabrirlo.", "Análisis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtNroPesaje.Text.Trim(), out var nroPesaje))
            {
                MessageBox.Show("Ingrese un N° de pesaje válido.", "Análisis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double.TryParse(txtPesoLiquido.Text.Trim(), out var pesoLiquido);
            double.TryParse(lblTotalPorcentaje.Text.Trim(), out var totalPorcentajeManual);
            double.TryParse(lblTotalPeso.Text.Trim(), out var totalPesoManual);

            var input = new AnalisisInput
            {
                NroPesaje = nroPesaje,
                Cliente = cmbCliente.Text.Trim(),
                Producto = cmbProducto.Text.Trim(),
                PesoLiquido = pesoLiquido,
                FechaDoc = dtFecha.Value,
                Factores = LeerFactores(),
                TotalDescuentoPorcentaje = totalPorcentajeManual,
                TotalDescuentoPeso = totalPesoManual
            };

            var resultado = _analisisExistente is not null
                ? _controller.Actualizar(_analisisExistente.DocEntry, input, Sesion.UsuarioActual.Id)
                : _controller.Guardar(input, Sesion.UsuarioActual.Id);
            if (resultado is null)
            {
                MessageBox.Show(_controller.ErrorMessage ?? "No se pudo guardar el análisis.", "Error al guardar",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"Guardado con éxito. Análisis N° {resultado.DocEntry}.\nDescuento total: {resultado.TotalDescuentoPorcentaje:N3}% ({resultado.TotalDescuentoPeso:N2})",
                "Análisis", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _analisisExistente = _controller.BuscarPorPeso(nroPesaje);
            lblNroAnalisisValor.Text = resultado.DocEntry.ToString();
            lblEstadoValor.Text = "C";
            lblEstadoValor.BackColor = Color.Gray;
            AplicarBloqueoEdicion(true);
            _nroPesajeGuardado = nroPesaje;
            btnImprimir.Enabled = true;
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (_nroPesajeGuardado is null) return;

            var datos = _boletaController.ObtenerDatosBoleta(_nroPesajeGuardado.Value);
            if (datos is null)
            {
                MessageBox.Show("No se encontró el pesaje a imprimir.", "Imprimir", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UI.BoletaPrinter.MostrarVistaPrevia(datos);
        }

        private void btnVerPeso_Click(object sender, EventArgs e)
        {
            if (_pesoActual is null) return;
            var frmPeso = new FrmPeso(_pesoActual.NroConsec);
            frmPeso.Show();
        }

        private void btnCerrar_Click(object sender, EventArgs e) => Close();
    }
}
