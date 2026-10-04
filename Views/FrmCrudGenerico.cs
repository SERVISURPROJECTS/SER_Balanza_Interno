using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Reflection;
using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.UI;

namespace SER_Balanza_Interno.Views
{
    /// <summary>
    /// CRUD generico por reflexion sobre un DbSet, reutilizado para las entidades
    /// que no requieren una pantalla a medida (ver FrmUsuarios para el caso especial de Usuario).
    /// </summary>
    public class FrmCrudGenerico<T> : Form where T : class, new()
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<T> _repo;
        private readonly BindingSource _bindingSource = new();
        private readonly DataGridView _grid = new();
        private readonly Panel _detailPanel = new() { AutoScroll = true };
        private readonly Button _btnNuevo = new() { Text = "Nuevo" };
        private readonly Button _btnGuardar = new() { Text = "Guardar" };
        private readonly Button _btnEliminar = new() { Text = "Eliminar" };
        private readonly Button _btnRefrescar = new() { Text = "Refrescar" };
        private readonly Label _lblEstado = new() { AutoSize = true, ForeColor = Color.Firebrick };

        private readonly List<PropertyInfo> _properties;
        private readonly Dictionary<string, Control> _editors = new();
        private readonly Func<T, bool>? _filtro;
        private readonly Action<T>? _alCrear;
        private T? _seleccionado;

        public FrmCrudGenerico() : this(null, null, null)
        {
        }

        /// <param name="filtro">Si se indica, la grilla solo muestra los registros que lo cumplen (ej. solo clientes).</param>
        /// <param name="alCrear">Se aplica a cada registro nuevo antes de guardarlo (ej. marcar es_cliente = true).</param>
        /// <param name="tituloOverride">Titulo de la ventana; si no se indica se usa el nombre del tipo.</param>
        public FrmCrudGenerico(Func<T, bool>? filtro, Action<T>? alCrear = null, string? tituloOverride = null)
        {
            _uow = UnitOfWorkFactory.Create();
            _repo = _uow.Repository<T>();
            _filtro = filtro;
            _alCrear = alCrear;

            _properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<ColumnAttribute>() != null)
                .ToList();

            Text = tituloOverride ?? $"CRUD - {typeof(T).Name}";
            Width = 1000;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(800, 500);
            Theme.AplicarFormulario(this);

            BuildLayout();
            Theme.EstilizarGrid(_grid);
            Theme.EstilizarBotonIcono(_btnNuevo, IconFactory.Nuevo());
            Theme.EstilizarBotonIcono(_btnGuardar, IconFactory.Guardar());
            Theme.EstilizarBotonIcono(_btnEliminar, IconFactory.Eliminar());
            Theme.EstilizarBotonIcono(_btnRefrescar, IconFactory.Refrescar());
            CargarDatos();
            LimpiarFormulario();

            FormClosed += (_, _) => _uow.Dispose();
        }

        private static bool EsIdentity(PropertyInfo prop) =>
            prop.GetCustomAttribute<DatabaseGeneratedAttribute>()?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity;

        private IEnumerable<PropertyInfo> PropiedadesEditables() =>
            _properties.Where(p => p.PropertyType != typeof(byte[]) && !EsIdentity(p));

        private void BuildLayout()
        {
            _grid.Dock = DockStyle.Left;
            _grid.Width = 560;
            _grid.ReadOnly = true;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.MultiSelect = false;
            _grid.AutoGenerateColumns = true;
            _grid.DataSource = _bindingSource;
            _grid.SelectionChanged += (_, _) =>
            {
                if (_grid.CurrentRow?.DataBoundItem is T item)
                {
                    _seleccionado = item;
                    PopularEditores(item);
                }
            };

            _detailPanel.Dock = DockStyle.Fill;
            _detailPanel.Padding = new Padding(16);

            int y = 10;
            foreach (var prop in _properties)
            {
                var label = new Label { Text = prop.Name, AutoSize = true, Location = new Point(0, y) };
                _detailPanel.Controls.Add(label);
                y += 18;

                Control editor = CrearEditor(prop);
                editor.Location = new Point(0, y);
                _editors[prop.Name] = editor;
                _detailPanel.Controls.Add(editor);
                y += editor.Height + 14;
            }

            var panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10)
            };
            _btnNuevo.Click += (_, _) => LimpiarFormulario();
            _btnGuardar.Click += BtnGuardar_Click;
            _btnEliminar.Click += BtnEliminar_Click;
            _btnRefrescar.Click += (_, _) => { CargarDatos(); LimpiarFormulario(); };
            panelBotones.Controls.Add(_btnNuevo);
            panelBotones.Controls.Add(_btnGuardar);
            panelBotones.Controls.Add(_btnEliminar);
            panelBotones.Controls.Add(_btnRefrescar);
            panelBotones.Controls.Add(_lblEstado);

            Controls.Add(_detailPanel);
            Controls.Add(panelBotones);
            Controls.Add(_grid);
        }

        private static Control CrearEditor(PropertyInfo prop)
        {
            var tipo = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            if (tipo == typeof(byte[]))
                return new TextBox { Width = 260, Enabled = false, Text = "(binario, no editable)" };

            if (tipo == typeof(bool))
                return new CheckBox();

            if (tipo == typeof(DateTime))
                return new DateTimePicker { Width = 260, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };

            var editor = new TextBox { Width = 260 };
            if (tipo == typeof(string))
                editor.CharacterCasing = CharacterCasing.Upper;
            if (EsIdentity(prop))
                editor.Enabled = false;
            return editor;
        }

        private void PopularEditores(T entidad)
        {
            foreach (var prop in _properties)
            {
                var valor = prop.GetValue(entidad);
                var control = _editors[prop.Name];
                switch (control)
                {
                    case CheckBox chk:
                        chk.Checked = valor is bool b && b;
                        break;
                    case DateTimePicker dtp:
                        dtp.Value = valor is DateTime dt ? dt : DateTime.Now;
                        break;
                    case TextBox txt when txt.Enabled:
                        txt.Text = valor?.ToString() ?? "";
                        break;
                    case TextBox txt:
                        txt.Text = valor?.ToString() ?? "";
                        break;
                }
            }
        }

        private void LimpiarFormulario()
        {
            _seleccionado = null;
            _lblEstado.Text = "";
            foreach (var prop in _properties)
            {
                var control = _editors[prop.Name];
                switch (control)
                {
                    case CheckBox chk:
                        chk.Checked = false;
                        break;
                    case DateTimePicker dtp:
                        dtp.Value = DateTime.Now;
                        break;
                    case TextBox txt:
                        txt.Text = "";
                        break;
                }
            }
            _grid.ClearSelection();
        }

        private void CargarDatos()
        {
            var datos = _repo.GetAll();
            _bindingSource.DataSource = _filtro is null ? datos : datos.Where(_filtro).ToList();
        }

        private object? ValorDesdeEditor(PropertyInfo prop)
        {
            var control = _editors[prop.Name];
            var subyacente = Nullable.GetUnderlyingType(prop.PropertyType);
            var esNullable = subyacente != null;
            var tipoEfectivo = subyacente ?? prop.PropertyType;

            if (control is CheckBox chk) return chk.Checked;
            if (control is DateTimePicker dtp) return dtp.Value;

            if (control is TextBox txt)
            {
                var texto = txt.Text.Trim();

                // Las columnas string del esquema son NOT NULL por convencion: un campo vacio
                // debe guardarse como cadena vacia, nunca como null (violaria la restriccion).
                if (tipoEfectivo == typeof(string)) return texto;

                if (string.IsNullOrEmpty(texto))
                    return esNullable ? null : Activator.CreateInstance(tipoEfectivo);

                if (tipoEfectivo == typeof(Guid)) return Guid.Parse(texto);
                return Convert.ChangeType(texto, tipoEfectivo, CultureInfo.InvariantCulture);
            }

            return null;
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                var destino = _seleccionado ?? new T();

                foreach (var prop in PropiedadesEditables())
                {
                    prop.SetValue(destino, ValorDesdeEditor(prop));
                }

                if (_seleccionado is null)
                {
                    _alCrear?.Invoke(destino);
                    _repo.Add(destino);
                }

                _uow.SaveChanges();
                CargarDatos();
                LimpiarFormulario();
                MessageBox.Show("Guardado con éxito.", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException?.Message ?? ex.Message;
                _lblEstado.Text = $"No se pudo guardar: {detalle}";
                MessageBox.Show(detalle, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (_seleccionado is null)
            {
                _lblEstado.Text = "Seleccione una fila de la lista.";
                return;
            }

            var confirm = MessageBox.Show("¿Eliminar el registro seleccionado?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _repo.Remove(_seleccionado);
                _uow.SaveChanges();
                CargarDatos();
                LimpiarFormulario();
                MessageBox.Show("Eliminado con éxito.", "CRUD", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException?.Message ?? ex.Message;
                _lblEstado.Text = $"No se pudo eliminar: {detalle}";
                MessageBox.Show(detalle, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
