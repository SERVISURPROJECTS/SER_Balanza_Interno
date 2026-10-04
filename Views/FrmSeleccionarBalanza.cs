using SER_Balanza_Interno.Controllers;
using SER_Balanza_Interno.Models;
using SER_Balanza_Interno.UI;

namespace SER_Balanza_Interno.Views
{
    public partial class FrmSeleccionarBalanza : Form
    {
        private readonly BalanzaController _controller = new();

        public balanza? BalanzaSeleccionada { get; private set; }

        public FrmSeleccionarBalanza()
        {
            InitializeComponent();
            Theme.AplicarFormulario(this);
            CargarBalanzas();
        }

        private void CargarBalanzas()
        {
            var balanzas = _controller.ObtenerActivas();
            cmbBalanza.DisplayMember = nameof(balanza.Descripcion);
            cmbBalanza.ValueMember = nameof(balanza.id);
            cmbBalanza.DataSource = balanzas;

            if (balanzas.Count == 0)
            {
                lblMensaje.Text = "No hay balanzas activas registradas. Puede continuar sin seleccionar una.";
                cmbBalanza.Enabled = false;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            BalanzaSeleccionada = cmbBalanza.SelectedItem as balanza;
            if (BalanzaSeleccionada is null && cmbBalanza.Enabled)
            {
                MessageBox.Show("Seleccione una balanza para continuar.", "Balanza",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
