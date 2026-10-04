namespace SER_Balanza_Interno.Views
{
    partial class FrmSeleccionarBalanza
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
        private System.Windows.Forms.Label lblBalanza;
        private System.Windows.Forms.ComboBox cmbBalanza;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Button btnAceptar;

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblBalanza = new System.Windows.Forms.Label();
            this.cmbBalanza = new System.Windows.Forms.ComboBox();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Text = "Seleccionar Balanza";
            //
            // lblBalanza
            //
            this.lblBalanza.AutoSize = true;
            this.lblBalanza.Location = new System.Drawing.Point(20, 65);
            this.lblBalanza.Text = "Balanza:";
            //
            // cmbBalanza
            //
            this.cmbBalanza.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBalanza.Location = new System.Drawing.Point(20, 85);
            this.cmbBalanza.Size = new System.Drawing.Size(320, 23);
            this.cmbBalanza.BackColor = SER_Balanza_Interno.UI.Theme.AmarilloCampo;
            //
            // lblMensaje
            //
            this.lblMensaje.AutoSize = false;
            this.lblMensaje.ForeColor = System.Drawing.Color.Firebrick;
            this.lblMensaje.Location = new System.Drawing.Point(20, 115);
            this.lblMensaje.Size = new System.Drawing.Size(320, 40);
            this.lblMensaje.Text = "";
            //
            // btnAceptar
            //
            this.btnAceptar.Location = new System.Drawing.Point(20, 165);
            this.btnAceptar.Size = new System.Drawing.Size(320, 34);
            this.btnAceptar.Text = "Ingresar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            //
            // FrmSeleccionarBalanza
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 220);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblBalanza);
            this.Controls.Add(this.cmbBalanza);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.btnAceptar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Balanza";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
