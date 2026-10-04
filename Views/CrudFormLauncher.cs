namespace SER_Balanza_Interno.Views
{
    public static class CrudFormLauncher
    {
        public static void Open(Type entityType)
        {
            var formType = typeof(FrmCrudGenerico<>).MakeGenericType(entityType);
            using var form = (Form)Activator.CreateInstance(formType)!;
            form.ShowDialog();
        }

        /// <summary>Abre el CRUD mostrando solo los registros que cumplen <paramref name="filtro"/>
        /// y marcando automaticamente los nuevos registros con <paramref name="alCrear"/>.</summary>
        public static void OpenFiltrado<T>(Func<T, bool> filtro, Action<T> alCrear, string titulo) where T : class, new()
        {
            using var form = new FrmCrudGenerico<T>(filtro, alCrear, titulo);
            form.ShowDialog();
        }
    }
}
