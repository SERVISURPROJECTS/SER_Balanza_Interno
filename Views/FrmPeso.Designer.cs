namespace SER_Balanza_Interno.Views
{
    partial class FrmPeso
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuArchivo;
        private System.Windows.Forms.ToolStripMenuItem mnuArchivoSalir;
        private System.Windows.Forms.ToolStripMenuItem mnuVer;
        private System.Windows.Forms.ToolStripMenuItem mnuHerramientas;
        private System.Windows.Forms.ToolStripMenuItem mnuReportes;
        private System.Windows.Forms.ToolStripMenuItem mnuUsuarios;
        private System.Windows.Forms.ToolStripMenuItem mnuRolesPermisos;
        private System.Windows.Forms.ToolStripMenuItem mnuAnalisis;
        private System.Windows.Forms.ToolStripMenuItem mnuCatalogos;
        private System.Windows.Forms.ToolStripMenuItem mnuCerrarSesion;
        private System.Windows.Forms.ToolStripMenuItem mnuSalir;
        private System.Windows.Forms.ToolStripMenuItem mnuAyuda;
        private System.Windows.Forms.ToolStripMenuItem mnuAyudaFormatoNumeros;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton tsUsuario;
        private System.Windows.Forms.ToolStripButton tsCliente;
        private System.Windows.Forms.ToolStripButton tsProveedor;
        private System.Windows.Forms.ToolStripButton tsProducto;
        private System.Windows.Forms.ToolStripButton tsVehiculo;
        private System.Windows.Forms.ToolStripButton tsChofer;
        private System.Windows.Forms.ToolStripButton tsProcedencia;
        private System.Windows.Forms.ToolStripButton tsDestino;
        private System.Windows.Forms.ToolStripButton tsAnalisis;
        private System.Windows.Forms.ToolStripButton tsBalanzas;
        private System.Windows.Forms.ToolStripButton tsTransferencia;
        private System.Windows.Forms.ToolStripButton tsSalir;
        private System.Windows.Forms.Label lblUsuarioActual;
        private System.Windows.Forms.Panel pnlBarraTitulo;

        private System.Windows.Forms.Panel pnlContenido;
        private System.Windows.Forms.Label lblTipoDocumento;
        private System.Windows.Forms.ComboBox cmbTipoDocumento;
        private System.Windows.Forms.Label lblHacienda;
        private System.Windows.Forms.ComboBox cmbHacienda;
        private System.Windows.Forms.Label lblNroIngreso;
        private System.Windows.Forms.TextBox txtNroIngreso;
        private System.Windows.Forms.Label lblRemito;
        private System.Windows.Forms.TextBox txtRemito;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label lblCampania;
        private System.Windows.Forms.ComboBox cmbCampania;
        private System.Windows.Forms.Label lblVehiculo;
        private System.Windows.Forms.ComboBox cmbVehiculo;
        private System.Windows.Forms.Button btnMasVehiculo;
        private System.Windows.Forms.Label lblChofer;
        private System.Windows.Forms.ComboBox cmbChofer;
        private System.Windows.Forms.Button btnMasChofer;
        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.ComboBox cmbProducto;
        private System.Windows.Forms.Button btnMasProducto;
        private System.Windows.Forms.Label lblProveedor;
        private System.Windows.Forms.ComboBox cmbProveedor;
        private System.Windows.Forms.Button btnMasProveedor;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.ComboBox cmbCliente;
        private System.Windows.Forms.Button btnMasCliente;
        private System.Windows.Forms.Label lblProcedencia;
        private System.Windows.Forms.ComboBox cmbProcedencia;
        private System.Windows.Forms.Button btnMasProcedencia;
        private System.Windows.Forms.Label lblDestino;
        private System.Windows.Forms.ComboBox cmbDestino;
        private System.Windows.Forms.Button btnMasDestino;

        private System.Windows.Forms.CheckBox chkModoManual;
        private System.Windows.Forms.Label lblBrutoTitulo;
        private System.Windows.Forms.TextBox txtBruto;
        private System.Windows.Forms.Button btnObtenerPeso;
        private System.Windows.Forms.Panel pnlLed;
        private System.Windows.Forms.Label lblLed;
        private System.Windows.Forms.Label lblTaraTitulo;
        private System.Windows.Forms.TextBox txtTara;
        private System.Windows.Forms.Label lblNetoTitulo;
        private System.Windows.Forms.TextBox lblNeto;
        private System.Windows.Forms.Label lblTotalDescuentoTitulo;
        private System.Windows.Forms.TextBox txtTotalDescuento;
        private System.Windows.Forms.Label lblPesoLiquidoTitulo;
        private System.Windows.Forms.TextBox txtPesoLiquido;
        private System.Windows.Forms.Label lblObservacion;
        private System.Windows.Forms.TextBox txtObservacion;

        private System.Windows.Forms.Panel pnlBarraCola;
        private System.Windows.Forms.DataGridView grid;

        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnVerAnalisis;

        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.mnuArchivo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuArchivoSalir = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHerramientas = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReportes = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRolesPermisos = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAnalisis = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCatalogos = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCerrarSesion = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSalir = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAyuda = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAyudaFormatoNumeros = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.tsUsuario = new System.Windows.Forms.ToolStripButton();
            this.tsCliente = new System.Windows.Forms.ToolStripButton();
            this.tsProveedor = new System.Windows.Forms.ToolStripButton();
            this.tsProducto = new System.Windows.Forms.ToolStripButton();
            this.tsVehiculo = new System.Windows.Forms.ToolStripButton();
            this.tsChofer = new System.Windows.Forms.ToolStripButton();
            this.tsProcedencia = new System.Windows.Forms.ToolStripButton();
            this.tsDestino = new System.Windows.Forms.ToolStripButton();
            this.tsAnalisis = new System.Windows.Forms.ToolStripButton();
            this.tsBalanzas = new System.Windows.Forms.ToolStripButton();
            this.tsTransferencia = new System.Windows.Forms.ToolStripButton();
            this.tsSalir = new System.Windows.Forms.ToolStripButton();
            this.lblUsuarioActual = new System.Windows.Forms.Label();
            this.pnlContenido = new System.Windows.Forms.Panel();

            this.lblTipoDocumento = new System.Windows.Forms.Label();
            this.cmbTipoDocumento = new System.Windows.Forms.ComboBox();
            this.lblHacienda = new System.Windows.Forms.Label();
            this.cmbHacienda = new System.Windows.Forms.ComboBox();
            this.lblNroIngreso = new System.Windows.Forms.Label();
            this.txtNroIngreso = new System.Windows.Forms.TextBox();
            this.lblRemito = new System.Windows.Forms.Label();
            this.txtRemito = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.lblCampania = new System.Windows.Forms.Label();
            this.cmbCampania = new System.Windows.Forms.ComboBox();
            this.lblVehiculo = new System.Windows.Forms.Label();
            this.cmbVehiculo = new System.Windows.Forms.ComboBox();
            this.btnMasVehiculo = new System.Windows.Forms.Button();
            this.lblChofer = new System.Windows.Forms.Label();
            this.cmbChofer = new System.Windows.Forms.ComboBox();
            this.btnMasChofer = new System.Windows.Forms.Button();
            this.lblProducto = new System.Windows.Forms.Label();
            this.cmbProducto = new System.Windows.Forms.ComboBox();
            this.btnMasProducto = new System.Windows.Forms.Button();
            this.lblProveedor = new System.Windows.Forms.Label();
            this.cmbProveedor = new System.Windows.Forms.ComboBox();
            this.btnMasProveedor = new System.Windows.Forms.Button();
            this.lblCliente = new System.Windows.Forms.Label();
            this.cmbCliente = new System.Windows.Forms.ComboBox();
            this.btnMasCliente = new System.Windows.Forms.Button();
            this.lblProcedencia = new System.Windows.Forms.Label();
            this.cmbProcedencia = new System.Windows.Forms.ComboBox();
            this.btnMasProcedencia = new System.Windows.Forms.Button();
            this.lblDestino = new System.Windows.Forms.Label();
            this.cmbDestino = new System.Windows.Forms.ComboBox();
            this.btnMasDestino = new System.Windows.Forms.Button();

            this.chkModoManual = new System.Windows.Forms.CheckBox();
            this.lblBrutoTitulo = new System.Windows.Forms.Label();
            this.txtBruto = new System.Windows.Forms.TextBox();
            this.btnObtenerPeso = new System.Windows.Forms.Button();
            this.pnlLed = new System.Windows.Forms.Panel();
            this.lblLed = new System.Windows.Forms.Label();
            this.lblTaraTitulo = new System.Windows.Forms.Label();
            this.txtTara = new System.Windows.Forms.TextBox();
            this.lblNetoTitulo = new System.Windows.Forms.Label();
            this.lblNeto = new System.Windows.Forms.TextBox();
            this.lblTotalDescuentoTitulo = new System.Windows.Forms.Label();
            this.txtTotalDescuento = new System.Windows.Forms.TextBox();
            this.lblPesoLiquidoTitulo = new System.Windows.Forms.Label();
            this.txtPesoLiquido = new System.Windows.Forms.TextBox();
            this.lblObservacion = new System.Windows.Forms.Label();
            this.txtObservacion = new System.Windows.Forms.TextBox();

            this.pnlBarraCola = new System.Windows.Forms.Panel();
            this.grid = new System.Windows.Forms.DataGridView();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnVerAnalisis = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.toolBar.SuspendLayout();
            this.pnlLed.SuspendLayout();
            this.SuspendLayout();
            //
            // menuStrip
            //
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuArchivo, this.mnuVer, this.mnuHerramientas, this.mnuReportes,
                this.mnuUsuarios, this.mnuRolesPermisos, this.mnuAnalisis, this.mnuCatalogos, this.mnuCerrarSesion, this.mnuSalir, this.mnuAyuda });
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuStrip.Name = "menuStrip";
            //
            // mnuArchivo
            //
            this.mnuArchivo.Text = "Archivo";
            this.mnuArchivo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.mnuArchivoSalir });
            //
            // mnuArchivoSalir
            //
            this.mnuArchivoSalir.Text = "Salir";
            this.mnuArchivoSalir.Click += new System.EventHandler(this.mnuSalir_Click);
            //
            // mnuVer
            //
            this.mnuVer.Text = "Ver";
            //
            // mnuHerramientas
            //
            this.mnuHerramientas.Text = "Herramientas";
            //
            // mnuReportes
            //
            this.mnuReportes.Text = "Reportes";
            //
            // mnuUsuarios
            //
            this.mnuUsuarios.Text = "Usuarios";
            this.mnuUsuarios.Click += new System.EventHandler(this.mnuUsuarios_Click);
            //
            // mnuRolesPermisos
            //
            this.mnuRolesPermisos.Text = "Roles y Permisos";
            this.mnuRolesPermisos.Click += new System.EventHandler(this.mnuRolesPermisos_Click);
            //
            // mnuAnalisis
            //
            this.mnuAnalisis.Text = "Análisis";
            this.mnuAnalisis.Click += new System.EventHandler(this.mnuAnalisis_Click);
            //
            // mnuCatalogos
            //
            this.mnuCatalogos.Text = "Catálogos";
            //
            // mnuCerrarSesion
            //
            this.mnuCerrarSesion.Text = "Cerrar sesión";
            this.mnuCerrarSesion.Click += new System.EventHandler(this.mnuCerrarSesion_Click);
            //
            // mnuSalir
            //
            this.mnuSalir.Text = "Salir";
            this.mnuSalir.Click += new System.EventHandler(this.mnuSalir_Click);
            //
            // mnuAyuda
            //
            this.mnuAyuda.Text = "Ayuda";
            this.mnuAyuda.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.mnuAyudaFormatoNumeros });
            //
            // mnuAyudaFormatoNumeros
            //
            this.mnuAyudaFormatoNumeros.Text = "Formato de números";
            this.mnuAyudaFormatoNumeros.Click += new System.EventHandler(this.mnuAyudaFormatoNumeros_Click);
            //
            // toolBar
            //
            this.toolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolBar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsUsuario, this.tsCliente, this.tsProveedor, this.tsProducto, this.tsVehiculo,
                this.tsChofer, this.tsProcedencia, this.tsDestino, this.tsAnalisis, this.tsBalanzas, this.tsTransferencia, this.tsSalir });
            this.toolBar.Height = 64;
            //
            // tsUsuario
            //
            this.tsUsuario.Text = "Usuario";
            this.tsUsuario.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsUsuario.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsUsuario.Click += new System.EventHandler(this.mnuUsuarios_Click);
            //
            // tsCliente
            //
            this.tsCliente.Text = "Cliente";
            this.tsCliente.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsCliente.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsCliente.Click += new System.EventHandler(this.tsCliente_Click);
            //
            // tsProveedor
            //
            this.tsProveedor.Text = "Proveedor";
            this.tsProveedor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsProveedor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsProveedor.Click += new System.EventHandler(this.tsProveedor_Click);
            //
            // tsProducto
            //
            this.tsProducto.Text = "Producto";
            this.tsProducto.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsProducto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsProducto.Click += new System.EventHandler(this.tsProducto_Click);
            //
            // tsVehiculo
            //
            this.tsVehiculo.Text = "Vehículo";
            this.tsVehiculo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsVehiculo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsVehiculo.Click += new System.EventHandler(this.tsVehiculo_Click);
            //
            // tsChofer
            //
            this.tsChofer.Text = "Chofer";
            this.tsChofer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsChofer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsChofer.Click += new System.EventHandler(this.tsChofer_Click);
            //
            // tsProcedencia
            //
            this.tsProcedencia.Text = "Procedencia";
            this.tsProcedencia.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsProcedencia.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsProcedencia.Click += new System.EventHandler(this.tsProcedencia_Click);
            //
            // tsDestino
            //
            this.tsDestino.Text = "Destino";
            this.tsDestino.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsDestino.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsDestino.Click += new System.EventHandler(this.tsDestino_Click);
            //
            // tsAnalisis
            //
            this.tsAnalisis.Text = "Análisis";
            this.tsAnalisis.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsAnalisis.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsAnalisis.Click += new System.EventHandler(this.mnuAnalisis_Click);
            //
            // tsBalanzas
            //
            this.tsBalanzas.Text = "Balanzas";
            this.tsBalanzas.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsBalanzas.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsBalanzas.Click += new System.EventHandler(this.tsBalanzas_Click);
            //
            // tsTransferencia
            //
            this.tsTransferencia.Text = "Transferencia";
            this.tsTransferencia.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsTransferencia.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsTransferencia.Enabled = false;
            //
            // tsSalir
            //
            this.tsSalir.Text = "Salir";
            this.tsSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsSalir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.tsSalir.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsSalir.Click += new System.EventHandler(this.mnuSalir_Click);
            //
            // lblUsuarioActual
            //
            this.lblUsuarioActual.AutoSize = true;
            this.lblUsuarioActual.Location = new System.Drawing.Point(900, 4);
            this.lblUsuarioActual.Name = "lblUsuarioActual";
            this.lblUsuarioActual.Text = "Usuario: -";
            this.lblUsuarioActual.ForeColor = System.Drawing.Color.White;
            //
            // pnlContenido
            //
            this.pnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenido.AutoScroll = true;

            AgregarCampoSimple(this.lblTipoDocumento, this.cmbTipoDocumento, "Tipo Documento:", 10);
            this.cmbTipoDocumento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            AgregarCampoSimple(this.lblHacienda, this.cmbHacienda, "Hacienda:", 45);
            AgregarCampoSimple(this.lblNroIngreso, null, "N° Ingreso:", 80);
            this.txtNroIngreso.Location = new System.Drawing.Point(140, 78);
            this.txtNroIngreso.Size = new System.Drawing.Size(220, 23);
            this.txtNroIngreso.BackColor = SER_Balanza_Interno.UI.Theme.AmarilloCampo;
            this.txtNroIngreso.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNroIngreso.ReadOnly = true;
            this.pnlContenido.Controls.Add(this.txtNroIngreso);

            AgregarCampoSimple(this.lblRemito, null, "N° Remito:", 115);
            this.txtRemito.Location = new System.Drawing.Point(140, 113);
            this.txtRemito.Size = new System.Drawing.Size(160, 23);
            this.txtRemito.BackColor = SER_Balanza_Interno.UI.Theme.AmarilloCampo;
            this.txtRemito.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemito.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.pnlContenido.Controls.Add(this.txtRemito);

            this.btnBuscar.Location = new System.Drawing.Point(306, 112);
            this.btnBuscar.Size = new System.Drawing.Size(80, 25);
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            this.pnlContenido.Controls.Add(this.btnBuscar);

            AgregarCampoSimple(this.lblCampania, this.cmbCampania, "Campaña:", 150);

            AgregarCampoConMas(this.lblVehiculo, this.cmbVehiculo, this.btnMasVehiculo, "Placa Vehículo:", 185);
            AgregarCampoConMas(this.lblChofer, this.cmbChofer, this.btnMasChofer, "Chofer:", 220);
            AgregarCampoConMas(this.lblProducto, this.cmbProducto, this.btnMasProducto, "Producto:", 255);
            AgregarCampoConMas(this.lblProveedor, this.cmbProveedor, this.btnMasProveedor, "Proveedor:", 290);
            AgregarCampoConMas(this.lblCliente, this.cmbCliente, this.btnMasCliente, "Cliente:", 325);
            AgregarCampoConMas(this.lblProcedencia, this.cmbProcedencia, this.btnMasProcedencia, "Procedencia:", 360);
            AgregarCampoConMas(this.lblDestino, this.cmbDestino, this.btnMasDestino, "Destino:", 395);

            // --- columna derecha: pesos ---
            int colDerechaX = 420;

            this.chkModoManual.AutoSize = true;
            this.chkModoManual.Location = new System.Drawing.Point(colDerechaX, 10);
            this.chkModoManual.Text = "Modo Manual";
            this.chkModoManual.Font = SER_Balanza_Interno.UI.Theme.FuenteNegrita;
            this.chkModoManual.ForeColor = System.Drawing.Color.DarkRed;
            this.chkModoManual.CheckedChanged += new System.EventHandler(this.chkModoManual_CheckedChanged);

            this.lblTaraTitulo.AutoSize = true;
            this.lblTaraTitulo.Location = new System.Drawing.Point(colDerechaX, 45);
            this.lblTaraTitulo.Text = "Peso Tara:";
            this.txtTara.Location = new System.Drawing.Point(colDerechaX, 63);
            this.txtTara.Size = new System.Drawing.Size(140, 25);
            this.txtTara.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtTara.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTara.BackColor = SER_Balanza_Interno.UI.Theme.AmarilloCampo;
            this.txtTara.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTara.TextChanged += new System.EventHandler(this.txtPeso_TextChanged);

            this.lblBrutoTitulo.AutoSize = true;
            this.lblBrutoTitulo.Location = new System.Drawing.Point(colDerechaX, 100);
            this.lblBrutoTitulo.Text = "Peso Bruto:";
            this.txtBruto.Location = new System.Drawing.Point(colDerechaX, 118);
            this.txtBruto.Size = new System.Drawing.Size(140, 25);
            this.txtBruto.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtBruto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBruto.BackColor = SER_Balanza_Interno.UI.Theme.AmarilloCampo;
            this.txtBruto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBruto.TextChanged += new System.EventHandler(this.txtPeso_TextChanged);

            this.lblNetoTitulo.AutoSize = true;
            this.lblNetoTitulo.Location = new System.Drawing.Point(colDerechaX, 155);
            this.lblNetoTitulo.Text = "Peso Neto:";
            this.lblNeto.Location = new System.Drawing.Point(colDerechaX, 173);
            this.lblNeto.Size = new System.Drawing.Size(140, 25);
            this.lblNeto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lblNeto.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblNeto.ForeColor = SER_Balanza_Interno.UI.Theme.AzulBarra;
            this.lblNeto.BackColor = SER_Balanza_Interno.UI.Theme.AmarilloCampo;
            this.lblNeto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNeto.Text = "0";

            this.lblTotalDescuentoTitulo.AutoSize = true;
            this.lblTotalDescuentoTitulo.Location = new System.Drawing.Point(colDerechaX, 210);
            this.lblTotalDescuentoTitulo.Text = "Total Descuento:";
                        this.txtTotalDescuento.Location = new System.Drawing.Point(colDerechaX, 228);
            this.txtTotalDescuento.Size = new System.Drawing.Size(140, 25);
            this.txtTotalDescuento.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalDescuento.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtTotalDescuento.ForeColor = SER_Balanza_Interno.UI.Theme.AzulBarra;
            this.txtTotalDescuento.BackColor = SER_Balanza_Interno.UI.Theme.AmarilloCampo;
            this.txtTotalDescuento.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalDescuento.Text = "0";

            this.lblPesoLiquidoTitulo.AutoSize = true;
            this.lblPesoLiquidoTitulo.Location = new System.Drawing.Point(colDerechaX, 265);
            this.lblPesoLiquidoTitulo.Text = "Peso Líquido:";
                        this.txtPesoLiquido.Location = new System.Drawing.Point(colDerechaX, 283);
            this.txtPesoLiquido.Size = new System.Drawing.Size(140, 25);
            this.txtPesoLiquido.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPesoLiquido.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtPesoLiquido.ForeColor = SER_Balanza_Interno.UI.Theme.AzulBarra;
            this.txtPesoLiquido.BackColor = SER_Balanza_Interno.UI.Theme.AmarilloCampo;
            this.txtPesoLiquido.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPesoLiquido.Text = "0";

            this.lblObservacion.AutoSize = true;
            this.lblObservacion.Location = new System.Drawing.Point(colDerechaX, 320);
            this.lblObservacion.Text = "Observación:";
            this.txtObservacion.Location = new System.Drawing.Point(colDerechaX, 338);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Size = new System.Drawing.Size(340, 70);
            this.txtObservacion.BackColor = SER_Balanza_Interno.UI.Theme.AmarilloCampo;
            this.txtObservacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtObservacion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;

            this.btnObtenerPeso.Location = new System.Drawing.Point(colDerechaX + 160, 45);
            this.btnObtenerPeso.Size = new System.Drawing.Size(90, 48);
            this.btnObtenerPeso.Text = "Obtener\nPeso";
            this.btnObtenerPeso.Enabled = false;

            this.pnlLed.BackColor = System.Drawing.Color.Black;
            this.pnlLed.Location = new System.Drawing.Point(colDerechaX + 265, 45);
            this.pnlLed.Size = new System.Drawing.Size(220, 60);
            this.pnlLed.Controls.Add(this.lblLed);
            this.lblLed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLed.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblLed.Font = new System.Drawing.Font("Consolas", 22F, System.Drawing.FontStyle.Bold);
            this.lblLed.Text = "00000000";

            this.pnlContenido.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.chkModoManual,
                this.btnObtenerPeso, this.pnlLed,
                this.lblTaraTitulo, this.txtTara,
                this.lblBrutoTitulo, this.txtBruto,
                this.lblNetoTitulo, this.lblNeto,
                this.lblTotalDescuentoTitulo, this.txtTotalDescuento,
                this.lblPesoLiquidoTitulo, this.txtPesoLiquido,
                this.lblObservacion, this.txtObservacion
            });

            this.pnlBarraCola.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBarraCola.Height = 24;
            this.pnlBarraCola.BackColor = SER_Balanza_Interno.UI.Theme.AzulBarra;

            this.grid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grid.Height = 170;
            this.grid.ReadOnly = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.MultiSelect = true;
            this.grid.Name = "grid";

            this.btnNuevo.Size = new System.Drawing.Size(130, 34);
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);

            this.btnGuardar.Size = new System.Drawing.Size(130, 34);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);

            this.btnImprimir.Size = new System.Drawing.Size(130, 34);
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.Enabled = false;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);

            this.btnVerAnalisis.Size = new System.Drawing.Size(130, 34);
            this.btnVerAnalisis.Text = "Ver Análisis";
            this.btnVerAnalisis.Enabled = false;
            this.btnVerAnalisis.Click += new System.EventHandler(this.btnVerAnalisis_Click);

            var panelBotones = new System.Windows.Forms.FlowLayoutPanel
            {
                Dock = System.Windows.Forms.DockStyle.Bottom,
                Height = 48,
                Padding = new System.Windows.Forms.Padding(10, 8, 10, 8),
                BackColor = System.Drawing.SystemColors.Control
            };
            panelBotones.Controls.Add(this.btnNuevo);
            panelBotones.Controls.Add(this.btnGuardar);
            panelBotones.Controls.Add(this.btnImprimir);
            panelBotones.Controls.Add(this.btnVerAnalisis);

            var lblCola = new System.Windows.Forms.Label
            {
                Dock = System.Windows.Forms.DockStyle.Bottom,
                Height = 20,
                Text = "Cola de Vehículos (En Progreso)",
                Font = SER_Balanza_Interno.UI.Theme.FuenteNegrita,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding = new System.Windows.Forms.Padding(4, 0, 0, 0)
            };

            //
            // FrmPeso
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 820);
            this.Controls.Add(this.pnlContenido);
            this.Controls.Add(panelBotones);
            this.Controls.Add(this.grid);
            this.Controls.Add(lblCola);
            this.Controls.Add(SER_Balanza_Interno.UI.Theme.CrearBarraTitulo("Modo de Pesaje Automático"));
            this.Controls.Add(this.lblUsuarioActual);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(1100, 700);
            this.Name = "FrmPeso";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SIR-PC Sistema de Registro de Pesaje de Camiones";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.pnlLed.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

            this.lblUsuarioActual.BringToFront();
        }

        private void AgregarCampoSimple(System.Windows.Forms.Label label, System.Windows.Forms.ComboBox? combo, string texto, int y)
        {
            label.AutoSize = true;
            label.Location = new System.Drawing.Point(15, y);
            label.Text = texto;
            this.pnlContenido.Controls.Add(label);

            if (combo is null) return;
            combo.Location = new System.Drawing.Point(140, y - 2);
            combo.Size = new System.Drawing.Size(220, 23);
            combo.BackColor = SER_Balanza_Interno.UI.Theme.AmarilloCampo;
            this.pnlContenido.Controls.Add(combo);
        }

        private void AgregarCampoConMas(System.Windows.Forms.Label label, System.Windows.Forms.ComboBox combo, System.Windows.Forms.Button boton, string texto, int y)
        {
            label.AutoSize = true;
            label.Location = new System.Drawing.Point(15, y);
            label.Text = texto;

            combo.Location = new System.Drawing.Point(140, y - 2);
            combo.Size = new System.Drawing.Size(220, 23);
            combo.BackColor = SER_Balanza_Interno.UI.Theme.AmarilloCampo;
            combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            boton.Location = new System.Drawing.Point(366, y - 3);
            boton.Size = new System.Drawing.Size(24, 24);
            boton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.Cursor = System.Windows.Forms.Cursors.Hand;

            this.pnlContenido.Controls.Add(label);
            this.pnlContenido.Controls.Add(combo);
            this.pnlContenido.Controls.Add(boton);
        }

        #endregion
    }
}
