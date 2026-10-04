using System.Drawing.Drawing2D;

namespace SER_Balanza_Interno.UI
{
    /// <summary>
    /// Iconos simples dibujados por codigo (GDI+), para no depender de archivos externos
    /// ni de que una fuente de simbolos especifica este instalada.
    /// </summary>
    public static class IconFactory
    {
        private static Bitmap Crear(int size, Action<Graphics, int> dibujar)
        {
            var bmp = new Bitmap(size, size);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            dibujar(g, size);
            return bmp;
        }

        public static Image Usuario(int size = 32, Color? color = null)
        {
            var c = color ?? Theme.AzulBarra;
            return Crear(size, (g, s) =>
            {
                using var brush = new SolidBrush(c);
                g.FillEllipse(brush, s * 0.30f, s * 0.12f, s * 0.40f, s * 0.40f);
                g.FillPie(brush, s * 0.12f, s * 0.48f, s * 0.76f, s * 0.70f, 180, 180);
            });
        }

        public static Image Grupo(int size = 32, Color? color = null)
        {
            var c = color ?? Theme.AzulBarra;
            return Crear(size, (g, s) =>
            {
                using var brush = new SolidBrush(c);
                g.FillEllipse(brush, s * 0.12f, s * 0.14f, s * 0.32f, s * 0.32f);
                g.FillPie(brush, s * 0.00f, s * 0.48f, s * 0.56f, s * 0.60f, 180, 180);
                g.FillEllipse(brush, s * 0.52f, s * 0.14f, s * 0.32f, s * 0.32f);
                g.FillPie(brush, s * 0.44f, s * 0.48f, s * 0.56f, s * 0.60f, 180, 180);
            });
        }

        public static Image Camion(int size = 32, Color? color = null)
        {
            var c = color ?? Theme.AzulBarra;
            return Crear(size, (g, s) =>
            {
                using var brush = new SolidBrush(c);
                g.FillRectangle(brush, s * 0.05f, s * 0.35f, s * 0.50f, s * 0.30f);
                g.FillRectangle(brush, s * 0.55f, s * 0.45f, s * 0.30f, s * 0.20f);
                using var pen = new Pen(c, Math.Max(1.5f, s * 0.04f));
                g.DrawPolygon(pen, new[]
                {
                    new PointF(s*0.55f, s*0.45f), new PointF(s*0.72f, s*0.45f),
                    new PointF(s*0.85f, s*0.60f), new PointF(s*0.55f, s*0.60f)
                });
                g.FillEllipse(brush, s * 0.12f, s * 0.62f, s * 0.14f, s * 0.14f);
                g.FillEllipse(brush, s * 0.65f, s * 0.62f, s * 0.14f, s * 0.14f);
            });
        }

        public static Image Caja(int size = 32, Color? color = null)
        {
            var c = color ?? Theme.AzulBarra;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, Math.Max(1.5f, s * 0.05f));
                g.DrawRectangle(pen, s * 0.15f, s * 0.30f, s * 0.70f, s * 0.55f);
                g.DrawLine(pen, s * 0.15f, s * 0.45f, s * 0.85f, s * 0.45f);
                g.DrawLine(pen, s * 0.50f, s * 0.30f, s * 0.50f, s * 0.45f);
                g.DrawLine(pen, s * 0.15f, s * 0.30f, s * 0.50f, s * 0.15f);
                g.DrawLine(pen, s * 0.85f, s * 0.30f, s * 0.50f, s * 0.15f);
            });
        }

        public static Image Volante(int size = 32, Color? color = null)
        {
            var c = color ?? Theme.AzulBarra;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, Math.Max(1.5f, s * 0.07f));
                g.DrawEllipse(pen, s * 0.15f, s * 0.15f, s * 0.70f, s * 0.70f);
                using var brush = new SolidBrush(c);
                g.FillEllipse(brush, s * 0.42f, s * 0.42f, s * 0.16f, s * 0.16f);
                g.DrawLine(pen, s * 0.50f, s * 0.50f, s * 0.50f, s * 0.18f);
                g.DrawLine(pen, s * 0.50f, s * 0.50f, s * 0.22f, s * 0.68f);
                g.DrawLine(pen, s * 0.50f, s * 0.50f, s * 0.78f, s * 0.68f);
            });
        }

        public static Image FlechaEntrada(int size = 32, Color? color = null)
        {
            var c = color ?? Theme.VerdeAccion;
            return Crear(size, (g, s) =>
            {
                using var brush = new SolidBrush(c);
                g.FillPolygon(brush, new[]
                {
                    new PointF(s*0.10f, s*0.50f), new PointF(s*0.55f, s*0.50f),
                    new PointF(s*0.55f, s*0.30f), new PointF(s*0.90f, s*0.55f),
                    new PointF(s*0.55f, s*0.80f), new PointF(s*0.55f, s*0.60f),
                    new PointF(s*0.10f, s*0.60f)
                });
            });
        }

        public static Image FlechaSalida(int size = 32, Color? color = null)
        {
            var c = color ?? Theme.RojoEstado;
            return Crear(size, (g, s) =>
            {
                using var brush = new SolidBrush(c);
                g.FillPolygon(brush, new[]
                {
                    new PointF(s*0.90f, s*0.50f), new PointF(s*0.45f, s*0.50f),
                    new PointF(s*0.45f, s*0.30f), new PointF(s*0.10f, s*0.55f),
                    new PointF(s*0.45f, s*0.80f), new PointF(s*0.45f, s*0.60f),
                    new PointF(s*0.90f, s*0.60f)
                });
            });
        }

        public static Image Matraz(int size = 32, Color? color = null)
        {
            var c = color ?? Theme.AzulBarra;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, Math.Max(1.5f, s * 0.05f));
                g.DrawLine(pen, s * 0.40f, s * 0.12f, s * 0.40f, s * 0.42f);
                g.DrawLine(pen, s * 0.60f, s * 0.12f, s * 0.60f, s * 0.42f);
                g.DrawLine(pen, s * 0.33f, s * 0.12f, s * 0.67f, s * 0.12f);
                g.DrawPolygon(pen, new[]
                {
                    new PointF(s*0.40f, s*0.42f), new PointF(s*0.60f, s*0.42f),
                    new PointF(s*0.82f, s*0.85f), new PointF(s*0.18f, s*0.85f)
                });
                using var brush = new SolidBrush(c);
                g.FillPolygon(brush, new[]
                {
                    new PointF(s*0.33f, s*0.60f), new PointF(s*0.67f, s*0.60f),
                    new PointF(s*0.78f, s*0.82f), new PointF(s*0.22f, s*0.82f)
                });
            });
        }

        public static Image Intercambio(int size = 32, Color? color = null)
        {
            var c = color ?? Theme.AzulBarra;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, Math.Max(1.5f, s * 0.07f));
                g.DrawArc(pen, s * 0.15f, s * 0.15f, s * 0.70f, s * 0.70f, -200, 170);
                g.DrawArc(pen, s * 0.15f, s * 0.15f, s * 0.70f, s * 0.70f, -20, 170);
                using var brush = new SolidBrush(c);
                g.FillPolygon(brush, new[] { new PointF(s*0.82f, s*0.22f), new PointF(s*0.68f, s*0.18f), new PointF(s*0.74f, s*0.32f) });
                g.FillPolygon(brush, new[] { new PointF(s*0.18f, s*0.78f), new PointF(s*0.32f, s*0.82f), new PointF(s*0.26f, s*0.68f) });
            });
        }

        public static Image Puerta(int size = 32, Color? color = null)
        {
            var c = color ?? Theme.RojoEstado;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, Math.Max(1.5f, s * 0.06f));
                g.DrawRectangle(pen, s * 0.20f, s * 0.12f, s * 0.35f, s * 0.76f);
                g.DrawLine(pen, s * 0.55f, s * 0.50f, s * 0.85f, s * 0.50f);
                using var brush = new SolidBrush(c);
                g.FillPolygon(brush, new[] { new PointF(s*0.85f, s*0.50f), new PointF(s*0.70f, s*0.40f), new PointF(s*0.70f, s*0.60f) });
            });
        }

        public static Image Balanza(int size = 32, Color? color = null)
        {
            var c = color ?? Theme.AzulBarra;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, Math.Max(1.5f, s * 0.06f)) { StartCap = LineCap.Round, EndCap = LineCap.Round };
                g.DrawLine(pen, s * 0.5f, s * 0.18f, s * 0.5f, s * 0.80f);
                g.DrawLine(pen, s * 0.30f, s * 0.80f, s * 0.70f, s * 0.80f);
                g.DrawLine(pen, s * 0.16f, s * 0.30f, s * 0.84f, s * 0.30f);
                g.DrawArc(pen, s * 0.08f, s * 0.30f, s * 0.20f, s * 0.18f, 0, 180);
                g.DrawArc(pen, s * 0.72f, s * 0.30f, s * 0.20f, s * 0.18f, 0, 180);
            });
        }

        public static Image Guardar(int size = 20, Color? color = null)
        {
            var c = color ?? Color.Black;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, 1.3f);
                g.DrawRectangle(pen, s * 0.15f, s * 0.12f, s * 0.70f, s * 0.76f);
                g.DrawRectangle(pen, s * 0.28f, s * 0.12f, s * 0.44f, s * 0.28f);
                g.DrawRectangle(pen, s * 0.28f, s * 0.55f, s * 0.44f, s * 0.33f);
            });
        }

        public static Image Nuevo(int size = 20, Color? color = null)
        {
            var c = color ?? Color.Black;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, 1.3f);
                g.DrawRectangle(pen, s * 0.18f, s * 0.12f, s * 0.54f, s * 0.70f);
                g.DrawLine(pen, s * 0.72f, s * 0.60f, s * 0.92f, s * 0.60f);
                g.DrawLine(pen, s * 0.82f, s * 0.50f, s * 0.82f, s * 0.70f);
            });
        }

        public static Image Imprimir(int size = 20, Color? color = null)
        {
            var c = color ?? Color.Black;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, 1.3f);
                g.DrawRectangle(pen, s * 0.20f, s * 0.35f, s * 0.60f, s * 0.30f);
                g.DrawRectangle(pen, s * 0.30f, s * 0.12f, s * 0.40f, s * 0.25f);
                g.DrawRectangle(pen, s * 0.30f, s * 0.62f, s * 0.40f, s * 0.26f);
            });
        }

        public static Image Eliminar(int size = 20, Color? color = null)
        {
            var c = color ?? Theme.RojoEstado;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, 1.4f);
                g.DrawRectangle(pen, s * 0.25f, s * 0.28f, s * 0.50f, s * 0.58f);
                g.DrawLine(pen, s * 0.16f, s * 0.22f, s * 0.84f, s * 0.22f);
                g.DrawLine(pen, s * 0.40f, s * 0.12f, s * 0.60f, s * 0.12f);
                g.DrawLine(pen, s * 0.38f, s * 0.38f, s * 0.38f, s * 0.74f);
                g.DrawLine(pen, s * 0.62f, s * 0.38f, s * 0.62f, s * 0.74f);
            });
        }

        public static Image Buscar(int size = 20, Color? color = null)
        {
            var c = color ?? Color.Black;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, Math.Max(1.4f, s * 0.09f));
                g.DrawEllipse(pen, s * 0.12f, s * 0.12f, s * 0.55f, s * 0.55f);
                g.DrawLine(pen, s * 0.60f, s * 0.60f, s * 0.90f, s * 0.90f);
            });
        }

        public static Image Refrescar(int size = 20, Color? color = null)
        {
            var c = color ?? Color.Black;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, 1.6f);
                g.DrawArc(pen, s * 0.15f, s * 0.15f, s * 0.70f, s * 0.70f, 30, 280);
                using var brush = new SolidBrush(c);
                g.FillPolygon(brush, new[] { new PointF(s*0.78f, s*0.18f), new PointF(s*0.62f, s*0.18f), new PointF(s*0.70f, s*0.34f) });
            });
        }

        public static Image MasVerde(int size = 18)
        {
            return Crear(size, (g, s) =>
            {
                using var brush = new SolidBrush(Theme.VerdeAccion);
                g.FillEllipse(brush, 0, 0, s, s);
                using var pen = new Pen(Color.White, Math.Max(1.5f, s * 0.14f));
                g.DrawLine(pen, s * 0.5f, s * 0.25f, s * 0.5f, s * 0.75f);
                g.DrawLine(pen, s * 0.25f, s * 0.5f, s * 0.75f, s * 0.5f);
            });
        }

        public static Image CerrarSesion(int size = 20, Color? color = null)
        {
            var c = color ?? Color.Black;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, 1.5f);
                g.DrawArc(pen, s * 0.10f, s * 0.12f, s * 0.55f, s * 0.76f, 90, 180);
                g.DrawLine(pen, s * 0.40f, s * 0.50f, s * 0.90f, s * 0.50f);
                using var brush = new SolidBrush(c);
                g.FillPolygon(brush, new[] { new PointF(s*0.90f, s*0.50f), new PointF(s*0.75f, s*0.40f), new PointF(s*0.75f, s*0.60f) });
            });
        }

        public static Image Candado(int size = 24, Color? color = null)
        {
            var c = color ?? Color.White;
            return Crear(size, (g, s) =>
            {
                using var pen = new Pen(c, Math.Max(1.5f, s * 0.08f));
                g.DrawArc(pen, s * 0.28f, s * 0.10f, s * 0.44f, s * 0.40f, 180, 180);
                using var brush = new SolidBrush(c);
                g.FillRectangle(brush, s * 0.18f, s * 0.42f, s * 0.64f, s * 0.48f);
            });
        }
    }
}
