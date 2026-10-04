using SER_Balanza_Interno.Controllers;
using SER_Balanza_Interno.Models;
using SER_Balanza_Interno.UI;

namespace SER_Balanza_Interno.Views
{
    public partial class FrmUsuarios : Form
    {
        private readonly UsuarioController _controller = new();
        private Usuario? _seleccionado;

        public FrmUsuarios()
        {
            InitializeComponent();
            Theme.AplicarFormulario(this);
            Theme.EstilizarGrid(grid);
            Theme.EstilizarCampo(txtNombre);
            Theme.EstilizarCampo(txtUsuario);
            Theme.EstilizarCampo(txtPassword);
            Theme.EstilizarCampo(cmbRol);
            CargarRoles();
            CargarUsuarios();
            LimpiarFormulario();
        }

        private void CargarRoles()
        {
            cmbRol.DisplayMember = nameof(rol.Nombre);
            cmbRol.ValueMember = nameof(rol.Id);
            cmbRol.DataSource = _controller.GetRoles();
        }

        private void CargarUsuarios()
        {
            var seleccionId = _seleccionado?.Id;
            grid.DataSource = _controller.GetAll()
                .Select(u => new
                {
                    u.Id,
                    u.Nombre,
                    Usuario = u.usuario,
                    Rol = u.id_rol,
                    u.Habilitado
                })
                .ToList();

            if (grid.Columns["Id"] != null) grid.Columns["Id"].Visible = false;
        }

        private void LimpiarFormulario()
        {
            _seleccionado = null;
            txtNombre.Text = "";
            txtUsuario.Text = "";
            txtPassword.Text = "";
            chkHabilitado.Checked = true;
            cmbRol.SelectedIndex = cmbRol.Items.Count > 0 ? 0 : -1;
            lblInfoPassword.Text = "";
            grid.ClearSelection();
        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.CurrentRow is null) return;

            var id = (int)grid.CurrentRow.Cells["Id"].Value;
            _seleccionado = _controller.GetAll().FirstOrDefault(u => u.Id == id);
            if (_seleccionado is null) return;

            txtNombre.Text = _seleccionado.Nombre;
            txtUsuario.Text = _seleccionado.usuario;
            txtPassword.Text = "";
            chkHabilitado.Checked = _seleccionado.Habilitado;
            cmbRol.SelectedValue = _seleccionado.id_rol ?? 0;
            lblInfoPassword.Text = "Dejar en blanco para mantener la contraseña actual.";
        }

        private void btnNuevo_Click(object sender, EventArgs e) => LimpiarFormulario();

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var idRol = cmbRol.SelectedValue is int v ? v : (int?)null;
            bool ok;

            if (_seleccionado is null)
            {
                ok = _controller.Create(txtNombre.Text.Trim(), txtUsuario.Text.Trim(), txtPassword.Text, idRol, chkHabilitado.Checked);
            }
            else
            {
                ok = _controller.Update(_seleccionado.Id, txtNombre.Text.Trim(), txtUsuario.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtPassword.Text) ? null : txtPassword.Text, idRol, chkHabilitado.Checked);
            }

            if (!ok)
            {
                MessageBox.Show(_controller.ErrorMessage ?? "No se pudo guardar el usuario.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CargarUsuarios();
            LimpiarFormulario();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_seleccionado is null)
            {
                MessageBox.Show("Seleccione un usuario de la lista.", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show($"¿Eliminar al usuario '{_seleccionado.usuario}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            if (!_controller.Delete(_seleccionado.Id))
            {
                MessageBox.Show(_controller.ErrorMessage ?? "No se pudo eliminar el usuario.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CargarUsuarios();
            LimpiarFormulario();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
            LimpiarFormulario();
        }
    }
}
