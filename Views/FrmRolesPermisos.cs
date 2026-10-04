using SER_Balanza_Interno.Controllers;
using SER_Balanza_Interno.Models;
using SER_Balanza_Interno.UI;

namespace SER_Balanza_Interno.Views
{
    public partial class FrmRolesPermisos : Form
    {
        private readonly RolPermisoController _controller = new();
        private List<PermisoDeRol> _permisosActuales = new();

        public FrmRolesPermisos()
        {
            InitializeComponent();
            Theme.AplicarFormulario(this);
            Theme.EstilizarBotonIcono(btnGuardar, IconFactory.Guardar());
            Theme.EstilizarBotonIcono(btnNuevoRol, IconFactory.Nuevo());

            // Los permisos que ya usa la app (se autocrean la primera vez que algo los consulta);
            // los dejamos disponibles de entrada para no depender de que ya se hayan usado una vez.
            _controller.AsegurarPermiso(PermisoController.EditarPesaje);
            _controller.AsegurarPermiso(PermisoController.AbmBalanza);

            CargarRoles();
        }

        private void CargarRoles()
        {
            var seleccionado = cmbRol.SelectedValue as int?;
            cmbRol.DisplayMember = nameof(rol.Nombre);
            cmbRol.ValueMember = nameof(rol.Id);
            cmbRol.DataSource = _controller.ObtenerRoles();
            if (seleccionado is not null) cmbRol.SelectedValue = seleccionado;
        }

        private void cmbRol_SelectedIndexChanged(object sender, EventArgs e) => CargarPermisos();

        private void CargarPermisos()
        {
            if (cmbRol.SelectedValue is not int idRol)
            {
                listaPermisos.Items.Clear();
                return;
            }

            _permisosActuales = _controller.ObtenerPermisosDeRol(idRol);
            listaPermisos.Items.Clear();
            foreach (var permiso in _permisosActuales)
            {
                listaPermisos.Items.Add(permiso.Nombre, permiso.Asignado);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbRol.SelectedValue is not int idRol)
            {
                MessageBox.Show("Seleccione un rol.", "Roles y Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (var i = 0; i < _permisosActuales.Count; i++)
            {
                _permisosActuales[i].Asignado = listaPermisos.GetItemChecked(i);
            }

            _controller.GuardarPermisosDeRol(idRol, _permisosActuales);
            MessageBox.Show("Permisos guardados correctamente.", "Roles y Permisos",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnNuevoRol_Click(object sender, EventArgs e)
        {
            CrudFormLauncher.Open(typeof(rol));
            CargarRoles();
        }

        private void btnCerrar_Click(object sender, EventArgs e) => Close();
    }
}
