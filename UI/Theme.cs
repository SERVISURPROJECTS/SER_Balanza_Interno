using System.Drawing.Drawing2D;

namespace SER_Balanza_Interno.UI
{
    /// <summary>
    /// Paleta y estilos comunes, inspirados en el sistema de pesaje de referencia:
    /// barra de titulo azul, campos de captura en amarillo palido, grillas con encabezado azul.
    /// </summary>
    public static class Theme
    {
        public static readonly Color AzulBarra = ColorTranslator.FromHtml("#27408B");
        public static readonly Color AzulBarraClaro = ColorTranslator.FromHtml("#3A5BA0");
        public static readonly Color AmarilloCampo = ColorTranslator.FromHtml("#FFFFCC");
        public static readonly Color FondoFormulario = ColorTranslator.FromHtml("#ECECEC");
        public static readonly Color FondoToolbar = ColorTranslator.FromHtml("#F4F4F4");
        public static readonly Color BordeGrid = ColorTranslator.FromHtml("#B9C3D6");
        public static readonly Color VerdeAccion = ColorTranslator.FromHtml("#2E9E3F");
        public static readonly Color RojoEstado = ColorTranslator.FromHtml("#E53935");

        public static readonly Font FuenteBase = new("Segoe UI", 9F);
        public static readonly Font FuenteNegrita = new("Segoe UI", 9F, FontStyle.Bold);
        public static readonly Font FuenteBarra = new("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font FuenteDigital = new("Consolas", 28F, FontStyle.Bold);
        public static readonly Font FuenteTitulo = new("Segoe UI", 14F, FontStyle.Bold);

        public static Icon? IconoApp { get; private set; }

        public static void Inicializar()
        {
            try
            {
                var ruta = Path.Combine(AppContext.BaseDirectory, "Resources", "app.ico");
                if (File.Exists(ruta))
                {
                    IconoApp = new Icon(ruta);
                }
            }
            catch
            {
                IconoApp = null;
            }
        }

        public static void AplicarFormulario(Form form)
        {
            form.Font = FuenteBase;
            form.BackColor = FondoFormulario;
            if (IconoApp is not null) form.Icon = IconoApp;
        }

        /// <summary>Barra de titulo de seccion estilo "Modo de Pesaje Automatico".</summary>
        public static Panel CrearBarraTitulo(string texto)
        {
            var panel = new Panel
            {
                BackColor = AzulBarra,
                Height = 26,
                Dock = DockStyle.Top
            };
            var label = new Label
            {
                Text = texto,
                ForeColor = Color.White,
                Font = FuenteBarra,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0)
            };
            panel.Controls.Add(label);
            return panel;
        }

        public static void EstilizarCampo(Control control)
        {
            control.BackColor = AmarilloCampo;
        }

        public static void EstilizarGrid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.BackgroundColor = Color.White;
            grid.GridColor = BordeGrid;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = AzulBarra;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = FuenteNegrita;
            grid.ColumnHeadersHeight = 26;
            grid.RowHeadersVisible = false;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 250);
            grid.DefaultCellStyle.SelectionBackColor = AzulBarraClaro;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        }

        public static void EstilizarBotonIcono(Button boton, Image? icono)
        {
            boton.Font = FuenteBase;
            boton.TextAlign = ContentAlignment.MiddleRight;
            boton.ImageAlign = ContentAlignment.MiddleLeft;
            boton.TextImageRelation = TextImageRelation.ImageBeforeText;
            boton.Padding = new Padding(6, 0, 4, 0);
            if (icono is not null) boton.Image = icono;
            boton.UseVisualStyleBackColor = true;
        }

        public static void EstilizarBotonGrande(Button boton, Image? icono)
        {
            boton.Font = FuenteBase;
            boton.TextAlign = ContentAlignment.BottomCenter;
            boton.ImageAlign = ContentAlignment.TopCenter;
            boton.TextImageRelation = TextImageRelation.ImageAboveText;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.BackColor = FondoToolbar;
            boton.Cursor = Cursors.Hand;
            if (icono is not null) boton.Image = icono;
        }
    }
}
