namespace SER_Balanza_Interno.Views
{
    partial class FrmAnalisis
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

        private System.Windows.Forms.Label lblTitulo;

        private System.Windows.Forms.Label lblNroPesaje;
        private System.Windows.Forms.TextBox txtNroPesaje;
        private System.Windows.Forms.Label lblNroAnalisis;
        private System.Windows.Forms.Label lblNroAnalisisValor;

        private System.Windows.Forms.Label lblNroRemito;
        private System.Windows.Forms.TextBox txtNroRemito;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.DateTimePicker dtFecha;

        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.ComboBox cmbCliente;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Label lblEstadoValor;

        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.ComboBox cmbProducto;
        private System.Windows.Forms.Label lblPesoLiquido;
        private System.Windows.Forms.TextBox txtPesoLiquido;

        private System.Windows.Forms.Label lblPesoNetoTitulo;
        private System.Windows.Forms.Label lblPesoNeto;

        private System.Windows.Forms.DataGridView grid;

        private System.Windows.Forms.Label lblTotalTitulo;
        private System.Windows.Forms.TextBox lblTotalPorcentaje;
        private System.Windows.Forms.TextBox lblTotalPeso;
        private System.Windows.Forms.CheckBox chkModoManual;

        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnVerPeso;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCerrar;

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblNroPesaje = new System.Windows.Forms.Label();
            this.txtNroPesaje = new System.Windows.Forms.TextBox();
            this.lblNroAnalisis = new System.Windows.Forms.Label();
            this.lblNroAnalisisValor = new System.Windows.Forms.Label();
            this.lblNroRemito = new System.Windows.Forms.Label();
            this.txtNroRemito = new System.Windows.Forms.TextBox();
            this.lblFecha = new System.Windows.Forms.Label();
            this.dtFecha = new System.Windows.Forms.DateTimePicker();
            this.lblCliente = new System.Windows.Forms.Label();
            this.cmbCliente = new System.Windows.Forms.ComboBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.lblEstadoValor = new System.Windows.Forms.Label();
            this.lblProducto = new System.Windows.Forms.Label();
            this.cmbProducto = new System.Windows.Forms.ComboBox();
            this.lblPesoLiquido = new System.Windows.Forms.Label();
            this.txtPesoLiquido = new System.Windows.Forms.TextBox();
            this.lblPesoNetoTitulo = new System.Windows.Forms.Label();
            this.lblPesoNeto = new System.Windows.Forms.Label();
            this.grid = new System.Windows.Forms.DataGridView();
            this.lblTotalTitulo = new System.Windows.Forms.Label();
            this.lblTotalPorcentaje = new System.Windows.Forms.TextBox();
            this.lblTotalPeso = new System.Windows.Forms.TextBox();
            this.chkModoManual = new System.Windows.Forms.CheckBox();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnVerPeso = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(15, 12);
            this.lblTitulo.Text = "Análisis de Grano";
            //
            // lblNroPesaje
            //
            this.lblNroPesaje.AutoSize = true;
            this.lblNroPesaje.Location = new System.Drawing.Point(15, 50);
            this.lblNroPesaje.Text = "N° de Pesaje";
            //
            // txtNroPesaje
            //
            this.txtNroPesaje.Location = new System.Drawing.Point(140, 47);
            this.txtNroPesaje.Size = new System.Drawing.Size(150, 23);
            this.txtNroPesaje.Leave += new System.EventHandler(this.txtNroPesaje_Leave);
            //
            // lblNroAnalisis
            //
            this.lblNroAnalisis.AutoSize = true;
            this.lblNroAnalisis.Location = new System.Drawing.Point(430, 50);
            this.lblNroAnalisis.Text = "N° de Análisis";
            //
            // lblNroAnalisisValor
            //
            this.lblNroAnalisisValor.AutoSize = true;
            this.lblNroAnalisisValor.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblNroAnalisisValor.Location = new System.Drawing.Point(550, 50);
            this.lblNroAnalisisValor.Text = "(auto)";
            //
            // lblNroRemito
            //
            this.lblNroRemito.AutoSize = true;
            this.lblNroRemito.Location = new System.Drawing.Point(15, 85);
            this.lblNroRemito.Text = "N° de Remito";
            //
            // txtNroRemito
            //
            this.txtNroRemito.Location = new System.Drawing.Point(140, 82);
            this.txtNroRemito.Size = new System.Drawing.Size(220, 23);
            //
            // lblFecha
            //
            this.lblFecha.AutoSize = true;
            this.lblFecha.Location = new System.Drawing.Point(430, 85);
            this.lblFecha.Text = "Fecha";
            //
            // dtFecha
            //
            this.dtFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFecha.Location = new System.Drawing.Point(550, 82);
            this.dtFecha.Size = new System.Drawing.Size(150, 23);
            //
            // lblCliente
            //
            this.lblCliente.AutoSize = true;
            this.lblCliente.Location = new System.Drawing.Point(15, 120);
            this.lblCliente.Text = "Cliente";
            //
            // cmbCliente
            //
            this.cmbCliente.Location = new System.Drawing.Point(140, 117);
            this.cmbCliente.Size = new System.Drawing.Size(260, 23);
            //
            // lblEstado
            //
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(430, 120);
            this.lblEstado.Text = "Estado";
            //
            // lblEstadoValor
            //
            this.lblEstadoValor.BackColor = System.Drawing.Color.Red;
            this.lblEstadoValor.ForeColor = System.Drawing.Color.White;
            this.lblEstadoValor.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblEstadoValor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEstadoValor.Location = new System.Drawing.Point(550, 117);
            this.lblEstadoValor.Size = new System.Drawing.Size(150, 23);
            this.lblEstadoValor.Text = "O";
            //
            // lblProducto
            //
            this.lblProducto.AutoSize = true;
            this.lblProducto.Location = new System.Drawing.Point(15, 155);
            this.lblProducto.Text = "Producto";
            //
            // cmbProducto
            //
            this.cmbProducto.Location = new System.Drawing.Point(140, 152);
            this.cmbProducto.Size = new System.Drawing.Size(260, 23);
            this.cmbProducto.Leave += new System.EventHandler(this.cmbProducto_Leave);
            //
            // lblPesoLiquido
            //
            this.lblPesoLiquido.AutoSize = true;
            this.lblPesoLiquido.Location = new System.Drawing.Point(430, 155);
            this.lblPesoLiquido.Text = "Peso Líquido";
            //
            // txtPesoLiquido
            //
            this.txtPesoLiquido.Location = new System.Drawing.Point(550, 152);
            this.txtPesoLiquido.Size = new System.Drawing.Size(150, 23);
            //
            // lblPesoNetoTitulo
            //
            this.lblPesoNetoTitulo.AutoSize = true;
            this.lblPesoNetoTitulo.Location = new System.Drawing.Point(15, 190);
            this.lblPesoNetoTitulo.Text = "Peso Neto";
            //
            // lblPesoNeto
            //
            this.lblPesoNeto.AutoSize = true;
            this.lblPesoNeto.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblPesoNeto.Location = new System.Drawing.Point(140, 190);
            this.lblPesoNeto.Text = "0.00";
            //
            // grid
            //
            this.grid.Location = new System.Drawing.Point(15, 225);
            this.grid.Size = new System.Drawing.Size(685, 230);
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.RowHeadersVisible = false;
            //
            // lblTotalTitulo
            //
            this.lblTotalTitulo.AutoSize = true;
            this.lblTotalTitulo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTotalTitulo.Location = new System.Drawing.Point(430, 465);
            this.lblTotalTitulo.Text = "Total";
            //
            // lblTotalPorcentaje
            //
            this.lblTotalPorcentaje.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblTotalPorcentaje.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lblTotalPorcentaje.Location = new System.Drawing.Point(490, 462);
            this.lblTotalPorcentaje.Size = new System.Drawing.Size(100, 23);
            this.lblTotalPorcentaje.Text = "0.000";
            //
            // lblTotalPeso
            //
            this.lblTotalPeso.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblTotalPeso.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lblTotalPeso.Location = new System.Drawing.Point(600, 462);
            this.lblTotalPeso.Size = new System.Drawing.Size(100, 23);
            this.lblTotalPeso.Text = "0.00";
            //
            // chkModoManual
            //
            this.chkModoManual.AutoSize = true;
            this.chkModoManual.Location = new System.Drawing.Point(15, 465);
            this.chkModoManual.Text = "Modo Manual";
            this.chkModoManual.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.chkModoManual.ForeColor = System.Drawing.Color.DarkRed;
            this.chkModoManual.CheckedChanged += new System.EventHandler(this.chkModoManual_CheckedChanged);
            //
            // btnImprimir
            //
            this.btnImprimir.Location = new System.Drawing.Point(15, 500);
            this.btnImprimir.Size = new System.Drawing.Size(140, 32);
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.Enabled = false;
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            //
            // btnVerPeso
            //
            this.btnVerPeso.Location = new System.Drawing.Point(160, 500);
            this.btnVerPeso.Size = new System.Drawing.Size(140, 32);
            this.btnVerPeso.Text = "Ver Pesaje";
            this.btnVerPeso.Enabled = false;
            this.btnVerPeso.UseVisualStyleBackColor = true;
            this.btnVerPeso.Click += new System.EventHandler(this.btnVerPeso_Click);
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(410, 500);
            this.btnGuardar.Size = new System.Drawing.Size(140, 32);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // btnCerrar
            //
            this.btnCerrar.Location = new System.Drawing.Point(560, 500);
            this.btnCerrar.Size = new System.Drawing.Size(140, 32);
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // FrmAnalisis
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 550);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblNroPesaje);
            this.Controls.Add(this.txtNroPesaje);
            this.Controls.Add(this.lblNroAnalisis);
            this.Controls.Add(this.lblNroAnalisisValor);
            this.Controls.Add(this.lblNroRemito);
            this.Controls.Add(this.txtNroRemito);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.dtFecha);
            this.Controls.Add(this.lblCliente);
            this.Controls.Add(this.cmbCliente);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.lblEstadoValor);
            this.Controls.Add(this.lblProducto);
            this.Controls.Add(this.cmbProducto);
            this.Controls.Add(this.lblPesoLiquido);
            this.Controls.Add(this.txtPesoLiquido);
            this.Controls.Add(this.lblPesoNetoTitulo);
            this.Controls.Add(this.lblPesoNeto);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.lblTotalTitulo);
            this.Controls.Add(this.lblTotalPorcentaje);
            this.Controls.Add(this.lblTotalPeso);
            this.Controls.Add(this.chkModoManual);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.btnVerPeso);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCerrar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Análisis de Grano";
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
