using SER_Balanza_Interno.Controllers;
using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.UI;

namespace SER_Balanza_Interno.Views
{
    public partial class FrmLogin : Form
    {
        private readonly AuthController _authController = new();

        public FrmLogin()
        {
            InitializeComponent();
            if (Theme.IconoApp is not null) Icon = Theme.IconoApp;
            picCandado.Image = IconFactory.Candado(40, Color.White);
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;

            var usuario = _authController.Login(txtUsuario.Text.Trim(), txtPassword.Text);
            if (usuario is null)
            {
                lblError.Text = _authController.ErrorMessage ?? "No se pudo iniciar sesión.";
                lblError.Visible = true;
                return;
            }

            Sesion.UsuarioActual = usuario;

            using (var seleccionBalanza = new FrmSeleccionarBalanza())
            {
                if (seleccionBalanza.ShowDialog(this) == DialogResult.OK)
                {
                    Sesion.BalanzaActual = seleccionBalanza.BalanzaSeleccionada;
                }
            }

            var pantallaPeso = new FrmPeso();
            Hide();
            pantallaPeso.FormClosed += (_, _) => Close();
            pantallaPeso.Show();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnIngresar.PerformClick();
            }
        }
    }
}
