namespace SER_Balanza_Interno.Views
{
    partial class FrmRolesPermisos
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
        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.ComboBox cmbRol;
        private System.Windows.Forms.Button btnNuevoRol;
        private System.Windows.Forms.Label lblPermisos;
        private System.Windows.Forms.CheckedListBox listaPermisos;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCerrar;

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblRol = new System.Windows.Forms.Label();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.btnNuevoRol = new System.Windows.Forms.Button();
            this.lblPermisos = new System.Windows.Forms.Label();
            this.listaPermisos = new System.Windows.Forms.CheckedListBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(15, 12);
            this.lblTitulo.Text = "Roles y Permisos";
            //
            // lblRol
            //
            this.lblRol.AutoSize = true;
            this.lblRol.Location = new System.Drawing.Point(15, 50);
            this.lblRol.Text = "Rol:";
            //
            // cmbRol
            //
            this.cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRol.Location = new System.Drawing.Point(15, 70);
            this.cmbRol.Size = new System.Drawing.Size(260, 23);
            this.cmbRol.SelectedIndexChanged += new System.EventHandler(this.cmbRol_SelectedIndexChanged);
            //
            // btnNuevoRol
            //
            this.btnNuevoRol.Location = new System.Drawing.Point(285, 69);
            this.btnNuevoRol.Size = new System.Drawing.Size(150, 25);
            this.btnNuevoRol.Text = "Gestionar roles...";
            this.btnNuevoRol.UseVisualStyleBackColor = true;
            this.btnNuevoRol.Click += new System.EventHandler(this.btnNuevoRol_Click);
            //
            // lblPermisos
            //
            this.lblPermisos.AutoSize = true;
            this.lblPermisos.Location = new System.Drawing.Point(15, 108);
            this.lblPermisos.Text = "Permisos asignados a este rol:";
            //
            // listaPermisos
            //
            this.listaPermisos.CheckOnClick = true;
            this.listaPermisos.Location = new System.Drawing.Point(15, 128);
            this.listaPermisos.Size = new System.Drawing.Size(420, 220);
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(15, 360);
            this.btnGuardar.Size = new System.Drawing.Size(140, 32);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // btnCerrar
            //
            this.btnCerrar.Location = new System.Drawing.Point(295, 360);
            this.btnCerrar.Size = new System.Drawing.Size(140, 32);
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // FrmRolesPermisos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 410);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblRol);
            this.Controls.Add(this.cmbRol);
            this.Controls.Add(this.btnNuevoRol);
            this.Controls.Add(this.lblPermisos);
            this.Controls.Add(this.listaPermisos);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCerrar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Roles y Permisos";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
