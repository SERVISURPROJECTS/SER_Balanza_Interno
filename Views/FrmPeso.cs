using SER_Balanza_Interno.Controllers;
using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.Models;
using SER_Balanza_Interno.UI;

namespace SER_Balanza_Interno.Views
{
    public partial class FrmPeso : Form
    {
        private readonly PesoController _controller = new();
        private readonly BoletaController _boletaController = new();
        private readonly AnalisisController _analisisController = new();
        private readonly PermisoController _permisoController = new();
        private int? _ultimoNroConsec;
        private int? _pesajeEnEdicion;
        /// <summary>
        /// Si el pesaje cargado ya tiene un Analisis registrado, ese Analisis manda sobre Total
        /// Descuento/Peso Liquido: no se recalculan solos a partir de Bruto/Tara/Descuento aqui.
        /// </summary>
        private bool _analisisRegistrado;
        /// <summary>
        /// Modo Manual (checkbox): permite registrar Tara/Bruto/Neto/Descuento/Peso Liquido a mano,
        /// sin el calculo automatico (Neto = Bruto - Tara, Peso Liquido = Neto - Descuento) ni la
        /// validacion de que Bruto debe ser mayor o igual a Tara.
        /// </summary>
        private bool _modoManual;

        public FrmPeso()
        {
            InitializeComponent();
            Theme.AplicarFormulario(this);
            AsignarIconos();
            ConstruirMenuCatalogos();
            lblUsuarioActual.Text = $"Usuario: {Sesion.UsuarioActual?.Nombre ?? "-"}";
            CargarAutocompletar();
            CargarGrid();
            CargarNroIngreso();
            RecalcularNeto();
            txtTotalDescuento.TextChanged += (_, _) => RecalcularPesoLiquido();
            grid.CellDoubleClick += Grid_CellDoubleClick;
            AplicarPermisosPorRol();
        }

        /// <summary>
        /// Por ahora el rol "operador" solo opera Peso y Análisis: el resto de menus/accesos
        /// quedan visibles (para que sepa que existen) pero deshabilitados. El rol "admin" ve todo.
        /// El acceso a Balanzas (alta/baja/modificacion) es aparte: depende del permiso configurable
        /// "ABM Balanza" sobre el rol del usuario (ver PermisoController / FrmRolesPermisos), asi que
        /// cualquier rol al que se le otorgue ese permiso puede administrar balanzas.
        /// </summary>
        private void AplicarPermisosPorRol()
        {
            var idRol = Sesion.UsuarioActual?.id_rol;

            tsBalanzas.Enabled = _permisoController.TienePermiso(idRol, PermisoController.AbmBalanza);

            using var uow = UnitOfWorkFactory.Create();
            var nombreRol = idRol is int idRolValor
                ? uow.Repository<rol>().Query().FirstOrDefault(r => r.Id == idRolValor)?.Nombre
                : null;

            // Roles y Permisos administra los permisos de todos los roles: queda solo para admin.
            var esAdmin = string.Equals(nombreRol, "admin", StringComparison.OrdinalIgnoreCase);
            mnuRolesPermisos.Enabled = esAdmin;

            var esOperador = string.Equals(nombreRol, "operador", StringComparison.OrdinalIgnoreCase);
            if (!esOperador) return;

            foreach (var item in new ToolStripItem[] { mnuVer, mnuHerramientas, mnuReportes, mnuUsuarios, mnuCatalogos })
            {
                item.Enabled = false;
            }

            foreach (var boton in new ToolStripItem[]
                { tsUsuario, tsCliente, tsProveedor, tsProducto, tsVehiculo, tsChofer, tsProcedencia, tsDestino })
            {
                boton.Enabled = false;
            }

            foreach (var boton in new[]
                { btnMasVehiculo, btnMasChofer, btnMasProducto, btnMasProveedor, btnMasCliente, btnMasProcedencia, btnMasDestino })
            {
                boton.Enabled = false;
            }
        }

        public FrmPeso(int nroConsecInicial) : this()
        {
            CargarPesajeEnFormulario(nroConsecInicial);
        }

        private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var valor = grid.Rows[e.RowIndex].Cells["PesoIn"]?.Value;
            if (valor is null || !int.TryParse(valor.ToString(), out var nroConsec)) return;

            CargarPesajeEnFormulario(nroConsec);
        }

        private void CargarPesajeEnFormulario(int nroConsec)
        {
            var detalle = _controller.ObtenerDetalle(nroConsec);
            if (detalle is null)
            {
                MessageBox.Show($"No se encontró el pesaje N° {nroConsec}.", "Pesaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Al cargar un pesaje ya guardado se activa Modo Manual: los valores guardados son los
            // definitivos y no deben recalcularse solos a partir de Bruto/Tara al reabrirlo (si el
            // usuario quiere volver al calculo automatico, puede destildar el checkbox).
            chkModoManual.Checked = true;

            cmbTipoDocumento.Text = detalle.TipoDocumento;
            txtRemito.Text = detalle.Remito;
            cmbHacienda.Text = detalle.Hacienda;
            cmbCampania.Text = detalle.Campania;
            cmbVehiculo.Text = detalle.Vehiculo;
            cmbChofer.Text = detalle.Chofer;
            cmbProducto.Text = detalle.Producto;
            cmbProveedor.Text = detalle.Proveedor;
            cmbCliente.Text = detalle.Cliente;
            cmbProcedencia.Text = detalle.Procedencia;
            cmbDestino.Text = detalle.Destino;
            txtBruto.Text = detalle.Bruto.ToString("N0", FormatoPeso);
            txtTara.Text = detalle.Tara.ToString("N0", FormatoPeso);
            txtObservacion.Text = detalle.Observacion;
            txtNroIngreso.Text = detalle.NroConsec.ToString();

            RecalcularNeto();
            if (detalle.Neto is not null) lblNeto.Text = detalle.Neto.Value.ToString("N0", FormatoPeso);

            var analisis = _analisisController.BuscarPorPeso(nroConsec);
            _analisisRegistrado = analisis is not null;
            var totalDescuento = detalle.TotalDescuento ?? analisis?.TotalDescPeso ?? 0;
            txtTotalDescuento.Text = totalDescuento.ToString("N0", FormatoPeso);
            txtPesoLiquido.Text = (detalle.PesoLiquido ?? (detalle.Bruto - detalle.Tara - totalDescuento)).ToString("N0", FormatoPeso);

            _pesajeEnEdicion = detalle.NroConsec;
            _ultimoNroConsec = detalle.NroConsec;
            btnImprimir.Enabled = true;
            btnVerAnalisis.Enabled = true;
        }

        private void btnVerAnalisis_Click(object sender, EventArgs e)
        {
            // Se guarda primero (sin limpiar el formulario) para que el Analisis arranque siempre con
            // el Peso Liquido/Total Descuento tal como estan en pantalla: Analisis relee el pesaje
            // desde la base de datos, asi que si se edito algo aqui sin guardar, abriria con datos
            // desactualizados (eso es lo que generaba el desfase Peso vs. Analisis).
            var nro = GuardarPesajeActual();
            if (nro is null) return;

            _pesajeEnEdicion = nro;
            _ultimoNroConsec = nro;
            btnImprimir.Enabled = true;

            var frmAnalisis = new FrmAnalisis(nro.Value);
            frmAnalisis.Show();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var filtro = new PesajeFiltro
            {
                Remito = txtRemito.Text.Trim(),
                TipoDocumento = cmbTipoDocumento.Text.Trim(),
                Hacienda = cmbHacienda.Text.Trim(),
                Campania = cmbCampania.Text.Trim(),
                Vehiculo = cmbVehiculo.Text.Trim(),
                Chofer = cmbChofer.Text.Trim(),
                Producto = cmbProducto.Text.Trim(),
                Proveedor = cmbProveedor.Text.Trim(),
                Cliente = cmbCliente.Text.Trim(),
                Procedencia = cmbProcedencia.Text.Trim(),
                Destino = cmbDestino.Text.Trim(),
            };

            if (filtro.EstaVacio)
            {
                MessageBox.Show("Ingrese al menos un criterio de búsqueda (por ejemplo, el N° de Remito).", "Buscar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var resultados = _controller.Buscar(filtro);
            MostrarResultadosBusqueda(resultados);

            if (resultados.Count == 0)
            {
                MessageBox.Show("No se encontraron pesajes con esos criterios.", "Buscar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (resultados.Count == 1)
            {
                CargarPesajeEnFormulario(resultados[0].NroConsec);
            }
        }

        private void MostrarResultadosBusqueda(List<Models.peso> resultados)
        {
            using var uow = UnitOfWorkFactory.Create();
            var vehiculos = uow.Repository<Vehiculo>().GetAll().ToDictionary(v => v.Id, v => v.Placa);
            var productos = uow.Repository<Producto>().GetAll().ToDictionary(p => p.Id, p => p.nombre);
            var clientes = uow.Repository<socio_negocio>().GetAll().ToDictionary(s => s.id, s => s.nombre);
            var choferes = uow.Repository<chofer>().GetAll().ToDictionary(c => c.id, c => c.nombre);

            grid.DataSource = resultados
                .Select(p => new
                {
                    PesoIn = p.NroConsec,
                    IdVehiculo = p.id_Vehiculo.HasValue && vehiculos.ContainsKey(p.id_Vehiculo.Value) ? vehiculos[p.id_Vehiculo.Value] : "",
                    FechaIngreso = p.FechaIngreso,
                    Peso = p.Neto,
                    Producto = p.Id_producto.HasValue && productos.ContainsKey(p.Id_producto.Value) ? productos[p.Id_producto.Value] : "",
                    Cliente = p.Id_cliente.HasValue && clientes.ContainsKey(p.Id_cliente.Value) ? clientes[p.Id_cliente.Value] : "",
                    Chofer = p.id_Chofer.HasValue && choferes.ContainsKey(p.id_Chofer.Value) ? choferes[p.id_Chofer.Value] : "",
                    Remito = p.NroTicket
                })
                .ToList();

            if (grid.Columns["PesoIn"] is not null) grid.Columns["PesoIn"].HeaderText = "PesoIn#";
            if (grid.Columns["IdVehiculo"] is not null) grid.Columns["IdVehiculo"].HeaderText = "ID Vehículo";
            if (grid.Columns["FechaIngreso"] is not null)
            {
                grid.Columns["FechaIngreso"].HeaderText = "Fecha Ingreso";
                grid.Columns["FechaIngreso"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
        }

        private void AsignarIconos()
        {
            tsUsuario.Image = IconFactory.Usuario(32);
            tsCliente.Image = IconFactory.Grupo(32);
            tsProveedor.Image = IconFactory.Camion(32);
            tsProducto.Image = IconFactory.Caja(32);
            tsVehiculo.Image = IconFactory.Camion(32);
            tsChofer.Image = IconFactory.Volante(32);
            tsProcedencia.Image = IconFactory.FlechaEntrada(32);
            tsDestino.Image = IconFactory.FlechaSalida(32);
            tsAnalisis.Image = IconFactory.Matraz(32);
            tsBalanzas.Image = IconFactory.Balanza(32);
            tsTransferencia.Image = IconFactory.Intercambio(32);
            tsSalir.Image = IconFactory.Puerta(32);

            btnMasVehiculo.Image = IconFactory.MasVerde(18);
            btnMasChofer.Image = IconFactory.MasVerde(18);
            btnMasProducto.Image = IconFactory.MasVerde(18);
            btnMasProveedor.Image = IconFactory.MasVerde(18);
            btnMasCliente.Image = IconFactory.MasVerde(18);
            btnMasProcedencia.Image = IconFactory.MasVerde(18);
            btnMasDestino.Image = IconFactory.MasVerde(18);

            Theme.EstilizarBotonIcono(btnNuevo, IconFactory.Nuevo());
            Theme.EstilizarBotonIcono(btnGuardar, IconFactory.Guardar());
            Theme.EstilizarBotonIcono(btnImprimir, IconFactory.Imprimir());
            Theme.EstilizarBotonIcono(btnBuscar, IconFactory.Buscar());
            Theme.EstilizarBotonIcono(btnVerAnalisis, IconFactory.Matraz(18));
            Theme.EstilizarGrid(grid);

            // Convencion fija: "." decimal, "," miles (se autoinserta al escribir). Ver nota en el
            // menu Ayuda > Formato de números.
            foreach (var txt in new[] { txtBruto, txtTara, lblNeto, txtTotalDescuento, txtPesoLiquido })
            {
                txt.KeyPress += SoloNumeroConComa_KeyPress;
                txt.TextChanged += FormatearMiles_TextChanged;
            }

            // Toda la informacion cargada por el usuario se guarda en mayuscula por defecto.
            foreach (var combo in new[] { cmbTipoDocumento, cmbHacienda, cmbCampania, cmbVehiculo, cmbChofer, cmbProducto, cmbProveedor, cmbCliente, cmbProcedencia, cmbDestino })
            {
                combo.KeyPress += ForzarMayusculas_KeyPress;
            }

            btnMasVehiculo.Click += (_, _) => AbrirCatalogo<Vehiculo>();
            btnMasChofer.Click += (_, _) => AbrirCatalogo<chofer>();
            btnMasProducto.Click += (_, _) => AbrirCatalogo<Producto>();
            btnMasProveedor.Click += (_, _) => AbrirProveedores();
            btnMasCliente.Click += (_, _) => AbrirClientes();
            btnMasProcedencia.Click += (_, _) => AbrirProcedencias();
            btnMasDestino.Click += (_, _) => AbrirDestinos();
        }

        /// <summary>
        /// Convencion fija del sistema para pesos: "," separador de miles (se agrega solo
        /// automaticamente, no se tipea), "." separador decimal. Se usa igual para tipear, para la
        /// vista (al cargar un pesaje guardado) y para la impresion de la boleta.
        /// </summary>
        private static readonly System.Globalization.NumberFormatInfo FormatoPeso = new()
        {
            NumberDecimalSeparator = ".",
            NumberGroupSeparator = ",",
        };

        /// <summary>Deja pasar digitos, control (backspace, etc.) y un unico punto decimal.</summary>
        private static void SoloNumeroConComa_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (char.IsDigit(e.KeyChar)) return;

            if (e.KeyChar == '.' && sender is TextBox txt && !txt.Text.Contains('.'))
            {
                return;
            }

            e.Handled = true;
        }

        /// <summary>Convierte a mayuscula cada caracter tipeado en combos editables (ComboBox no tiene CharacterCasing).</summary>
        private static void ForzarMayusculas_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            e.KeyChar = char.ToUpperInvariant(e.KeyChar);
        }

        /// <summary>
        /// Reformatea el campo insertando la coma de miles a medida que se escribe
        /// (ej. al tipear "45000" el campo pasa a mostrar "45,000"), preservando la posicion del cursor.
        /// </summary>
        private static void FormatearMiles_TextChanged(object? sender, EventArgs e)
        {
            if (sender is not TextBox txt) return;

            var textoOriginal = txt.Text;
            var digitosAntesDelCursor = textoOriginal[..txt.SelectionStart].Count(char.IsDigit);

            var indicePunto = textoOriginal.IndexOf('.');
            var parteEnteraCruda = indicePunto >= 0 ? textoOriginal[..indicePunto] : textoOriginal;
            var parteDecimalCruda = indicePunto >= 0 ? textoOriginal[(indicePunto + 1)..] : null;

            var parteEntera = new string(parteEnteraCruda.Where(char.IsDigit).ToArray()).TrimStart('0');
            var parteDecimal = parteDecimalCruda is null ? null : new string(parteDecimalCruda.Where(char.IsDigit).ToArray());

            string textoFormateado;
            if (textoOriginal.Length == 0)
            {
                textoFormateado = "";
            }
            else if (parteEntera.Length == 0)
            {
                textoFormateado = indicePunto >= 0 ? "0." + parteDecimal : "0";
            }
            else if (long.TryParse(parteEntera, out var valorEntero))
            {
                textoFormateado = valorEntero.ToString("N0", FormatoPeso);
                if (indicePunto >= 0) textoFormateado += "." + parteDecimal;
            }
            else
            {
                return; // numero demasiado grande para long: se deja como esta, no se reformatea.
            }

            if (textoFormateado == textoOriginal) return;

            txt.TextChanged -= FormatearMiles_TextChanged;
            txt.Text = textoFormateado;

            var nuevaPosicion = 0;
            var digitosContados = 0;
            foreach (var c in textoFormateado)
            {
                if (digitosContados >= digitosAntesDelCursor) break;
                nuevaPosicion++;
                if (char.IsDigit(c)) digitosContados++;
            }
            txt.SelectionStart = Math.Min(nuevaPosicion, textoFormateado.Length);
            txt.TextChanged += FormatearMiles_TextChanged;
        }

        /// <summary>Convierte texto formateado con punto de miles / coma decimal a double.</summary>
        private static bool TryParsePeso(string texto, out double valor) =>
            double.TryParse(texto, System.Globalization.NumberStyles.Number, FormatoPeso, out valor);

        private void AbrirCatalogo<T>() where T : class, new()
        {
            CrudFormLauncher.Open(typeof(T));
            CargarAutocompletar();
        }

        private void AbrirClientes()
        {
            CrudFormLauncher.OpenFiltrado<socio_negocio>(s => s.es_cliente, s => s.es_cliente = true, "CRUD - Cliente");
            CargarAutocompletar();
        }

        private void AbrirProveedores()
        {
            CrudFormLauncher.OpenFiltrado<socio_negocio>(s => s.es_proveedor, s => s.es_proveedor = true, "CRUD - Proveedor");
            CargarAutocompletar();
        }

        private void AbrirProcedencias()
        {
            CrudFormLauncher.OpenFiltrado<origen_destino>(o => !o.es_destino, o => o.es_destino = false, "CRUD - Procedencia");
            CargarAutocompletar();
        }

        private void AbrirDestinos()
        {
            CrudFormLauncher.OpenFiltrado<origen_destino>(o => o.es_destino, o => o.es_destino = true, "CRUD - Destino");
            CargarAutocompletar();
        }

        private void tsCliente_Click(object sender, EventArgs e) => AbrirClientes();
        private void tsProveedor_Click(object sender, EventArgs e) => AbrirProveedores();
        private void tsProducto_Click(object sender, EventArgs e) => AbrirCatalogo<Producto>();
        private void tsVehiculo_Click(object sender, EventArgs e) => AbrirCatalogo<Vehiculo>();
        private void tsChofer_Click(object sender, EventArgs e) => AbrirCatalogo<chofer>();
        private void tsProcedencia_Click(object sender, EventArgs e) => AbrirProcedencias();
        private void tsDestino_Click(object sender, EventArgs e) => AbrirDestinos();

        private void ConstruirMenuCatalogos()
        {
            foreach (var (nombre, tipo) in EntityRegistry.Entidades)
            {
                var item = new ToolStripMenuItem(nombre);
                item.Click += (_, _) => CrudFormLauncher.Open(tipo);
                mnuCatalogos.DropDownItems.Add(item);
            }
        }

        private void CargarNroIngreso()
        {
            using var uow = UnitOfWorkFactory.Create();
            var items = uow.Repository<peso>().GetAll();
            var siguiente = items.Count == 0 ? 1 : items.Max(p => p.NroConsec) + 1;
            txtNroIngreso.Text = siguiente.ToString();
        }

        private void CargarAutocompletar()
        {
            using var uow = UnitOfWorkFactory.Create();

            void Autocompletar(ComboBox combo, IEnumerable<string> valores)
            {
                combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                combo.AutoCompleteSource = AutoCompleteSource.ListItems;
                combo.Items.Clear();
                combo.Items.AddRange(valores.Distinct().OrderBy(v => v).Cast<object>().ToArray());
            }

            Autocompletar(cmbVehiculo, uow.Repository<Vehiculo>().GetAll().Select(v => v.Placa));
            Autocompletar(cmbChofer, uow.Repository<chofer>().GetAll().Select(c => c.nombre));
            Autocompletar(cmbProducto, uow.Repository<Producto>().GetAll().Select(p => p.nombre));
            Autocompletar(cmbProveedor, uow.Repository<socio_negocio>().GetAll().Where(s => s.es_proveedor).Select(s => s.nombre));
            Autocompletar(cmbCliente, uow.Repository<socio_negocio>().GetAll().Where(s => s.es_cliente).Select(s => s.nombre));
            Autocompletar(cmbProcedencia, uow.Repository<origen_destino>().GetAll().Where(o => !o.es_destino).Select(o => o.nombre));
            Autocompletar(cmbDestino, uow.Repository<origen_destino>().GetAll().Where(o => o.es_destino).Select(o => o.nombre));
            Autocompletar(cmbHacienda, uow.Repository<Hacienda>().GetAll().Select(h => h.nombre));
            Autocompletar(cmbCampania, uow.Repository<campania>().GetAll().Select(c => c.nombre));
            Autocompletar(cmbTipoDocumento, uow.Repository<documento>().GetAll().Select(d => d.nombre));
        }

        private void CargarGrid() => MostrarResultadosBusqueda(_controller.GetUltimos());

        private void RecalcularNeto()
        {
            if (_modoManual) return;

            TryParsePeso(txtBruto.Text, out var bruto);
            TryParsePeso(txtTara.Text, out var tara);
            lblNeto.Text = (bruto - tara).ToString("N0", FormatoPeso);
            RecalcularPesoLiquido();
        }

        /// <summary>
        /// Peso Liquido = Peso Neto - Total Descuento, igual que en Analisis. Solo se autocalcula
        /// mientras el pesaje no tenga un Analisis registrado (ese manda si existe, ver
        /// <see cref="_analisisRegistrado"/>) y no este activo el Modo Manual; el campo sigue siendo
        /// editable a mano en cualquier caso.
        /// </summary>
        private void RecalcularPesoLiquido()
        {
            if (_analisisRegistrado || _modoManual) return;

            TryParsePeso(txtBruto.Text, out var bruto);
            TryParsePeso(txtTara.Text, out var tara);
            TryParsePeso(txtTotalDescuento.Text, out var totalDescuento);
            txtPesoLiquido.Text = (bruto - tara - totalDescuento).ToString("N0", FormatoPeso);
        }

        private void txtPeso_TextChanged(object sender, EventArgs e) => RecalcularNeto();

        /// <summary>
        /// Al activar el Modo Manual, Tara/Bruto/Neto/Descuento/Peso Liquido quedan 100% a cargo del
        /// usuario: no se recalculan solos y al guardar no se exige que Bruto sea mayor o igual a Tara.
        /// Al desactivarlo, se recalcula Neto/Peso Liquido de inmediato a partir de lo que haya en
        /// pantalla.
        /// </summary>
        private void chkModoManual_CheckedChanged(object sender, EventArgs e)
        {
            _modoManual = chkModoManual.Checked;
            if (!_modoManual)
            {
                RecalcularNeto();
            }
        }

        /// <summary>
        /// Valida y guarda el pesaje tal como esta en el formulario (alta o edicion segun
        /// <see cref="_pesajeEnEdicion"/>), mostrando un MessageBox si algo falla. No limpia el
        /// formulario ni muestra el mensaje de exito: eso lo decide cada boton que la use.
        /// </summary>
        private int? GuardarPesajeActual()
        {
            if (Sesion.UsuarioActual is null)
            {
                MessageBox.Show("No hay una sesión activa.", "Pesaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (_pesajeEnEdicion is not null &&
                !_permisoController.TienePermiso(Sesion.UsuarioActual.id_rol, PermisoController.EditarPesaje))
            {
                MessageBox.Show("Su rol no tiene permiso para editar pesajes existentes.", "Pesaje",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (!TryParsePeso(txtBruto.Text, out var bruto) || !TryParsePeso(txtTara.Text, out var tara))
            {
                MessageBox.Show("Ingrese valores numéricos válidos para Bruto y Tara.", "Pesaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            var input = new PesajeInput
            {
                TipoDocumento = cmbTipoDocumento.Text.Trim(),
                Remito = txtRemito.Text.Trim(),
                Hacienda = cmbHacienda.Text.Trim(),
                Campania = cmbCampania.Text.Trim(),
                Vehiculo = cmbVehiculo.Text.Trim(),
                Chofer = cmbChofer.Text.Trim(),
                Producto = cmbProducto.Text.Trim(),
                Proveedor = cmbProveedor.Text.Trim(),
                Cliente = cmbCliente.Text.Trim(),
                Procedencia = cmbProcedencia.Text.Trim(),
                Destino = cmbDestino.Text.Trim(),
                Bruto = bruto,
                Tara = tara,
                Neto = TryParsePeso(lblNeto.Text, out var neto) ? neto : (bruto - tara),
                TotalDescuento = TryParsePeso(txtTotalDescuento.Text, out var totalDesc) ? totalDesc : 0,
                PesoLiquido = TryParsePeso(txtPesoLiquido.Text, out var pesoLiquido) ? pesoLiquido : (bruto - tara),
                Observacion = txtObservacion.Text.Trim(),
                ModoManual = _modoManual
            };

            int? nro;
            if (_pesajeEnEdicion is not null)
            {
                var ok = _controller.Actualizar(_pesajeEnEdicion.Value, input, Sesion.UsuarioActual.Id);
                nro = ok ? _pesajeEnEdicion : null;
            }
            else
            {
                nro = _controller.Guardar(input, Sesion.UsuarioActual.Id);
            }

            if (nro is null)
            {
                MessageBox.Show(_controller.ErrorMessage ?? "No se pudo guardar el pesaje.", "Error al guardar",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return nro;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var nro = GuardarPesajeActual();
            if (nro is null) return;

            MessageBox.Show($"Guardado con éxito. N° consec: {nro}", "Pesaje",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            _ultimoNroConsec = nro;
            _pesajeEnEdicion = null;
            btnImprimir.Enabled = true;
            btnVerAnalisis.Enabled = true;

            LimpiarFormulario();
            CargarAutocompletar();
            CargarGrid();
            CargarNroIngreso();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            var nrosSeleccionados = grid.SelectedRows.Cast<DataGridViewRow>()
                .Select(fila => fila.Cells["PesoIn"]?.Value)
                .Where(valor => valor is not null)
                .Select(valor => int.TryParse(valor!.ToString(), out var n) ? n : (int?)null)
                .Where(n => n is not null)
                .Select(n => n!.Value)
                .Distinct()
                .ToList();

            if (nrosSeleccionados.Count == 0)
            {
                MessageBox.Show("Seleccione una o más filas de la grilla para imprimir.", "Imprimir",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var boletas = nrosSeleccionados
                .Select(nro => _boletaController.ObtenerDatosBoleta(nro))
                .Where(d => d is not null)
                .Select(d => d!)
                .ToList();

            if (boletas.Count == 0)
            {
                MessageBox.Show("No se encontró ningún pesaje para imprimir.", "Imprimir",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BoletaPrinter.MostrarVistaPrevia(boletas);
        }

        private void LimpiarFormulario()
        {
            txtRemito.Text = "";
            cmbTipoDocumento.Text = "";
            cmbHacienda.Text = "";
            cmbCampania.Text = "";
            cmbVehiculo.Text = "";
            cmbChofer.Text = "";
            cmbProducto.Text = "";
            cmbProveedor.Text = "";
            cmbCliente.Text = "";
            cmbProcedencia.Text = "";
            cmbDestino.Text = "";
            txtBruto.Text = "";
            txtTara.Text = "";
            txtTotalDescuento.Text = "0";
            txtPesoLiquido.Text = "0";
            txtObservacion.Text = "";
            _analisisRegistrado = false;
            chkModoManual.Checked = false;
            RecalcularNeto();
            cmbHacienda.Focus();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            _ultimoNroConsec = null;
            _pesajeEnEdicion = null;
            btnImprimir.Enabled = false;
            btnVerAnalisis.Enabled = false;
            LimpiarFormulario();
            CargarGrid();
        }

        private void mnuUsuarios_Click(object sender, EventArgs e) => new FrmUsuarios().ShowDialog();

        private void mnuRolesPermisos_Click(object sender, EventArgs e) => new FrmRolesPermisos().ShowDialog();

        private void mnuAnalisis_Click(object sender, EventArgs e) => new FrmAnalisis().ShowDialog();

        private void tsBalanzas_Click(object sender, EventArgs e) => CrudFormLauncher.Open(typeof(Models.balanza));

        private void mnuCerrarSesion_Click(object sender, EventArgs e)
        {
            Sesion.Cerrar();
            var login = new FrmLogin();
            Hide();
            login.FormClosed += (_, _) => Close();
            login.Show();
        }

        private void mnuSalir_Click(object sender, EventArgs e) => Application.Exit();

        private void mnuAyudaFormatoNumeros_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "En los campos de peso (Bruto, Tara, Peso Neto, Total Descuento, Peso Líquido):\n\n" +
                "• El punto ( . ) es el separador de miles y se agrega solo a medida que escribe.\n" +
                "• La coma ( , ) es el separador decimal.\n\n" +
                "Ejemplo: al escribir 45000 el campo muestra 45.000.",
                "Formato de números", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
