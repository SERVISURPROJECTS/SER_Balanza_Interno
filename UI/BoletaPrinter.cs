using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing.Text;
using SER_Balanza_Interno.Controllers;

namespace SER_Balanza_Interno.UI
{
    /// <summary>
    /// Dibuja la boleta de despacho (estilo "DESPACHO GRANO DE ...") idéntica al formato original
    /// en tamaño Carta (Letter) sin márgenes excesivos, y la muestra en un diálogo de vista previa
    /// de impresión estándar de Windows con opción de exportar a PDF.
    /// </summary>
    public static class BoletaPrinter
    {
        // Tamaño de diseño de una boleta individual (1:1 con el formato original de media hoja carta).
        public const int DisenoAncho = 1014;
        public const int DisenoAlto = 640;

        public static void MostrarVistaPrevia(BoletaData datos) => MostrarVistaPrevia(new List<BoletaData> { datos });

        /// <summary>
        /// Configura explícitamente el tamaño de papel a Carta (Letter: 8.5 x 11 pulgadas)
        /// para evitar que Windows tome A4 u otro formato regional por omisión.
        /// </summary>
        private static void ConfigurarTamanoCarta(PrintDocument documento)
        {
            var papelCarta = documento.PrinterSettings.PaperSizes
                .Cast<PaperSize>()
                .FirstOrDefault(p => p.Kind == PaperKind.Letter)
                ?? new PaperSize("Letter", 850, 1100);

            documento.DefaultPageSettings.PaperSize = papelCarta;
            documento.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
        }

        /// <summary>
        /// Imprime una o varias boletas en hoja carta (mitad superior si es 1 sola,
        /// o dos por hoja con línea de corte punteada si son varias).
        /// </summary>
        public static void MostrarVistaPrevia(List<BoletaData> boletas)
        {
            if (boletas.Count == 0) return;

            var indice = 0;
            var imprimirDosPorHoja = boletas.Count > 1;
            var documento = new PrintDocument
            {
                DocumentName = boletas.Count == 1
                    ? $"Remito-{NombreArchivoSeguro(boletas[0].NroRemito)}"
                    : $"Remitos-{boletas.Count}"
            };

            ConfigurarTamanoCarta(documento);

            documento.BeginPrint += (_, _) => indice = 0;
            documento.PrintPage += (_, e) =>
            {
                // Usamos los límites totales de la página (e.PageBounds) para que el diseño
                // —que ya incluye internamente las distancias exactas del formato original—
                // ocupe la hoja carta completa sin márgenes externos duplicados.
                var bounds = e.PageBounds;
                var mitadAlto = bounds.Height / 2;

                if (!imprimirDosPorHoja)
                {
                    // Una sola boleta: ocupa la mitad superior de la hoja carta, idéntico al original.
                    DibujarEscalado(e.Graphics!, boletas[indice], new Rectangle(0, 0, bounds.Width, mitadAlto));
                    indice++;
                    e.HasMorePages = false;
                    return;
                }

                DibujarEscalado(e.Graphics!, boletas[indice], new Rectangle(0, 0, bounds.Width, mitadAlto));
                indice++;

                if (indice < boletas.Count)
                {
                    using var lapizCorte = new Pen(Color.Gray, 1f) { DashStyle = DashStyle.Dash };
                    e.Graphics!.DrawLine(lapizCorte, 0, mitadAlto, bounds.Width, mitadAlto);

                    DibujarEscalado(e.Graphics!, boletas[indice], new Rectangle(0, mitadAlto, bounds.Width, mitadAlto));
                    indice++;
                }

                e.HasMorePages = indice < boletas.Count;
            };

            var titulo = boletas.Count == 1
                ? "Vista previa - Boleta de despacho"
                : $"Vista previa - {boletas.Count} boletas de despacho";

            MostrarVentanaDeVistaPrevia(documento, titulo);
        }

        private const string ImpresoraPdf = "Microsoft Print to PDF";

        /// <summary>
        /// Ventana propia de vista previa con botones explícitos "Imprimir..." y "Descargar PDF...".
        /// </summary>
        private static void MostrarVentanaDeVistaPrevia(PrintDocument documento, string titulo)
        {
            using var form = new Form
            {
                Text = titulo,
                Width = 980,
                Height = 800,
                StartPosition = FormStartPosition.CenterScreen
            };

            var previewControl = new PrintPreviewControl
            {
                Dock = DockStyle.Fill,
                Document = documento,
                Zoom = 1.0
            };

            var panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 46,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10)
            };

            var btnCerrar = new Button { Text = "Cerrar", Width = 110, Height = 28 };
            btnCerrar.Click += (_, _) => form.Close();

            var btnImprimir = new Button { Text = "Imprimir...", Width = 120, Height = 28 };
            btnImprimir.Click += (_, _) =>
            {
                using var dialogoImpresora = new PrintDialog { Document = documento, AllowSomePages = false, AllowSelection = false };
                if (dialogoImpresora.ShowDialog(form) != DialogResult.OK) return;

                try
                {
                    ConfigurarTamanoCarta(documento);
                    documento.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(form, $"No se pudo imprimir: {ex.Message}", "Imprimir",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            var btnDescargarPdf = new Button { Text = "Descargar PDF...", Width = 140, Height = 28 };
            btnDescargarPdf.Click += (_, _) => DescargarComoPdf(documento, form);

            panelBotones.Controls.Add(btnCerrar);
            panelBotones.Controls.Add(btnImprimir);
            panelBotones.Controls.Add(btnDescargarPdf);

            form.Controls.Add(previewControl);
            form.Controls.Add(panelBotones);
            form.ShowDialog();
        }

        private static void DescargarComoPdf(PrintDocument documento, IWin32Window ventana)
        {
            var hayImpresoraPdf = PrinterSettings.InstalledPrinters.Cast<string>()
                .Any(nombre => string.Equals(nombre, ImpresoraPdf, StringComparison.OrdinalIgnoreCase));
            if (!hayImpresoraPdf)
            {
                MessageBox.Show(ventana,
                    $"No se encontró la impresora virtual \"{ImpresoraPdf}\" en este equipo, necesaria para generar el PDF.",
                    "Descargar PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dialogoGuardar = new SaveFileDialog
            {
                Filter = "Archivo PDF (*.pdf)|*.pdf",
                FileName = $"{documento.DocumentName}.pdf",
                AddExtension = true
            };
            if (dialogoGuardar.ShowDialog(ventana) != DialogResult.OK) return;

            var impresoraOriginal = documento.PrinterSettings;
            try
            {
                var newSettings = new PrinterSettings
                {
                    PrinterName = ImpresoraPdf,
                    PrintToFile = true,
                    PrintFileName = dialogoGuardar.FileName
                };
                documento.PrinterSettings = newSettings;
                ConfigurarTamanoCarta(documento);

                documento.Print();
                MessageBox.Show(ventana, $"PDF guardado en:\n{dialogoGuardar.FileName}", "Descargar PDF",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ventana, $"No se pudo generar el PDF: {ex.Message}", "Descargar PDF",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                documento.PrinterSettings = impresoraOriginal;
                ConfigurarTamanoCarta(documento);
            }
        }

        private static string NombreArchivoSeguro(string texto)
        {
            var limpio = string.IsNullOrWhiteSpace(texto) ? "SinNumero" : texto;
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                limpio = limpio.Replace(c, '-');
            }
            return limpio;
        }

        private static GraphicsPath RectanguloRedondeado(Rectangle rect, int radio)
        {
            var path = new GraphicsPath();
            var d = radio * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static FontFamily ObtenerFamiliaMono()
        {
            foreach (var nombre in new[] { "Consolas", "Lucida Console", "Courier New" })
            {
                try
                {
                    using var f = new Font(nombre, 10);
                    if (string.Equals(f.Name, nombre, StringComparison.OrdinalIgnoreCase))
                        return new FontFamily(nombre);
                }
                catch { }
            }
            return FontFamily.GenericMonospace;
        }

        /// <summary>
        /// Genera un Bitmap con la boleta dibujada a tamaño nativo (1014x640), útil para pruebas o exportación directa a imagen.
        /// </summary>
        public static Bitmap RenderizarABitmap(BoletaData datos)
        {
            var bmp = new Bitmap(DisenoAncho, DisenoAlto);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            Dibujar(g, datos);
            return bmp;
        }

        private static void DibujarEscalado(Graphics g, BoletaData datos, Rectangle destino)
        {
            var estado = g.Save();
            try
            {
                var escala = Math.Min((float)destino.Width / DisenoAncho, (float)destino.Height / DisenoAlto);
                var xOffset = destino.Left + (destino.Width - DisenoAncho * escala) / 2f;
                var yOffset = destino.Top;

                g.TranslateTransform(xOffset, yOffset);
                g.ScaleTransform(escala, escala);
                Dibujar(g, datos);
            }
            finally
            {
                g.Restore(estado);
            }
        }

        private static void Dibujar(Graphics g, BoletaData d)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            const string familiaSans = "Arial";
            using var famMono = ObtenerFamiliaMono();

            // Fuentes según diseño original exacto:
            using var fuenteTitulo = new Font(familiaSans, 15.5F, FontStyle.Bold);
            using var fuenteHeaderSub = new Font(familiaSans, 11.5F, FontStyle.Bold);
            using var fuenteHeaderBase = new Font(familiaSans, 10.5F, FontStyle.Regular);
            using var fuenteHeaderBaseBold = new Font(familiaSans, 10.5F, FontStyle.Bold);

            using var fuenteDgTitulo = new Font(familiaSans, 11F, FontStyle.Bold);
            using var fuenteMono = new Font(famMono, 12.5F, FontStyle.Regular);
            using var fuenteMonoBold = new Font(famMono, 12.5F, FontStyle.Bold);

            using var fuentePesoTop = new Font(familiaSans, 10F, FontStyle.Bold);
            using var fuentePesoHeader = new Font(familiaSans, 10F, FontStyle.Regular);
            using var fuentePesoLbl = new Font(familiaSans, 10F, FontStyle.Bold);
            using var fuentePesoVal = new Font(familiaSans, 10F, FontStyle.Regular);
            using var fuentePesoManual = new Font(familiaSans, 10F, FontStyle.Underline);

            using var fuenteCcTitulo = new Font(familiaSans, 11F, FontStyle.Regular);
            using var fuenteCcHdr = new Font(familiaSans, 10F, FontStyle.Regular);

            using var lapizFino = new Pen(Color.Black, 1.0f);
            using var lapizNormal = new Pen(Color.Black, 1.2f);
            using var lapizGrueso = new Pen(Color.Black, 1.8f);
            using var pincelNegro = new SolidBrush(Color.Black);

            void CentrarTexto(string texto, Font fuente, float yy, float cx)
            {
                var tam = g.MeasureString(texto, fuente, PointF.Empty, StringFormat.GenericTypographic);
                g.DrawString(texto, fuente, pincelNegro, cx - tam.Width / 2f, yy);
            }

            void DibujarTextoDerecha(string texto, Font fuente, float xRight, float yy)
            {
                var tam = g.MeasureString(texto, fuente, PointF.Empty, StringFormat.GenericTypographic);
                g.DrawString(texto, fuente, pincelNegro, xRight - tam.Width, yy);
            }

            // =========================================================================
            // 1. ENCABEZADO
            // =========================================================================
            // Izquierda: Balanza y datos de contacto
            const float xLeft = 76;
            g.DrawString(d.CompaniaNombre, fuenteHeaderSub, pincelNegro, xLeft, 46);
            g.DrawString($"Dir.: {d.CompaniaDireccion}", fuenteHeaderBase, pincelNegro, xLeft, 64);
            g.DrawString($"Teléfono: {d.CompaniaTelefono}", fuenteHeaderBase, pincelNegro, xLeft, 80);
            g.DrawString($"Email: {d.CompaniaEmail}", fuenteHeaderBase, pincelNegro, xLeft, 96);

            // Centro: Título (nombre literal del Documento/Tipo Documento del pesaje), producto y N° de análisis
            var tituloDoc = d.TipoDocumento.Trim().ToUpperInvariant();
            CentrarTexto(tituloDoc, fuenteTitulo, 46, 563);
            CentrarTexto($"Producto: {d.ProductoNombre}", fuenteHeaderSub, 68, 563);
            var analisisTexto = d.AnalisisNro.HasValue ? d.AnalisisNro.Value.ToString() : "-";
            CentrarTexto($"Análisis Nro.:    {analisisTexto}", fuenteHeaderBase, 90, 540);

            // Derecha: Remito, Campaña, Fecha (alineados a la izquierda en columna común)
            const float xRight = 740;
            g.DrawString($"N° Remito: {d.NroRemito}", fuenteHeaderBaseBold, pincelNegro, xRight, 60);
            g.DrawString($"Campaña: {d.Campania}", fuenteHeaderBase, pincelNegro, xRight, 78);
            g.DrawString($"Fecha:  {d.Fecha:dd/MM/yyyy}", fuenteHeaderBase, pincelNegro, xRight, 96);

            // Línea divisoria horizontal completa debajo del encabezado
            g.DrawLine(lapizGrueso, 79, 123, 932, 123);

            // =========================================================================
            // 2. SECCIÓN DATOS GENERALES (Izquierda) & PESO (Derecha)
            // =========================================================================
            // Título "Datos Generales" centrado sobre el recuadro izquierdo
            float cxDatos = (83 + 692) / 2f;
            CentrarTexto("Datos Generales", fuenteDgTitulo, 128, cxDatos);

            // Recuadro izquierdo de Datos Generales
            var rectDg = new Rectangle(83, 144, 609, 197);
            using (var pathDg = RectanguloRedondeado(rectDg, 12))
            {
                g.DrawPath(lapizGrueso, pathDg);
            }

            // Filas de datos generales (fuente monoespaciada Consolas con colones alineados)
            var filasDg = new (string Etiqueta, string Valor)[]
            {
                ("Cliente:", d.Cliente),
                ("Centro Acopio:", d.CentroAcopio),
                ("Procedencia:", d.Procedencia),
                ("Proveedor:", d.Proveedor),
                ("Conductor:", d.Conductor),
                ("C.I.:", d.ConductorCi),
                ("Vehículo:", d.Vehiculo),
                ("Placa:", d.Placa),
                ("Observación:", d.Observacion)
            };

            float yDg = 155;
            const float xColonDg = 228;
            const float xValDg = 243;
            foreach (var (etiq, val) in filasDg)
            {
                DibujarTextoDerecha(etiq, fuenteMono, xColonDg, yDg);
                g.DrawString(val?.ToUpperInvariant() ?? "", fuenteMono, pincelNegro, xValDg, yDg);
                yDg += 19;
            }

            // Bloque derecho: Despacho, Tiquete Balanza y caja de Peso
            float cxPeso = (699 + 930) / 2f;
            CentrarTexto($"Nro. Despacho: {d.NroDespacho}", fuentePesoTop, 144, cxPeso);
            CentrarTexto($"Tiquete Balanza: {d.TiqueteBalanza}", fuentePesoTop, 160, cxPeso);
            g.DrawLine(lapizGrueso, 710, 171, 917, 171);
            CentrarTexto("P E S O   E N   K g s .", fuentePesoHeader, 175, cxPeso);

            var rectPeso = new Rectangle(699, 188, 231, 130);
            using (var pathPeso = RectanguloRedondeado(rectPeso, 10))
            {
                g.DrawPath(lapizGrueso, pathPeso);
            }

            var filasPeso = new (string Etiqueta, double Valor)[]
            {
                ("Peso Bruto:", d.PesoBruto),
                ("Peso Tara:", d.PesoTara),
                ("Peso Neto:", d.PesoNeto),
                ("Descuentos:", d.Descuentos),
                ("Peso Líquido:", d.PesoLiquido)
            };

            float yPeso = 202;
            const float xLblPeso = 814;
            const float xValPeso = 922;
            foreach (var (etiq, val) in filasPeso)
            {
                DibujarTextoDerecha(etiq, fuentePesoLbl, xLblPeso, yPeso);
                var valTexto = val.ToString("N0", System.Globalization.CultureInfo.InvariantCulture);
                DibujarTextoDerecha(valTexto, fuentePesoVal, xValPeso, yPeso);
                yPeso += 22;
            }

            if (d.PesoManual)
            {
                CentrarTexto("Obs. Peso Manual", fuentePesoManual, 328, cxPeso);
            }

            // =========================================================================
            // 3. RECUADRO CONTROL DE CALIDAD Y MATRIZ DE VALORES
            // =========================================================================
            // Recuadro superior con título y encabezados de columnas (mismo ancho que Datos Generales)
            var rectCc = new Rectangle(83, 343, 609, 31);
            using (var pathCc = RectanguloRedondeado(rectCc, 10))
            {
                g.DrawPath(lapizGrueso, pathCc);
            }
            CentrarTexto("C o n t r o l   d e   C a l i d a d", fuenteCcTitulo, 345, cxDatos);

            g.DrawString("Factores", fuenteCcHdr, pincelNegro, 163, 359);
            g.DrawString("Parámetros", fuenteCcHdr, pincelNegro, 284, 359);
            g.DrawString("Análisis", fuenteCcHdr, pincelNegro, 382, 359);
            g.DrawString("Descto. %", fuenteCcHdr, pincelNegro, 463, 359);
            g.DrawString("Descto. Kg", fuenteCcHdr, pincelNegro, 558, 359);

            // Filas de valores de la matriz (sueltas sin recuadro)
            float yFactores = 378;
            foreach (var factor in d.Factores)
            {
                var nombre = factor.Nombre.ToUpperInvariant();
                DibujarTextoDerecha(nombre, fuenteMono, 245, yFactores);
                DibujarTextoDerecha(factor.Parametro.ToString("N2", System.Globalization.CultureInfo.InvariantCulture), fuenteMono, 318, yFactores);
                DibujarTextoDerecha(factor.Analisis.ToString("N2", System.Globalization.CultureInfo.InvariantCulture), fuenteMono, 411, yFactores);
                DibujarTextoDerecha(factor.DescuentoPorcentaje.ToString("N2", System.Globalization.CultureInfo.InvariantCulture), fuenteMono, 507, yFactores);
                DibujarTextoDerecha(factor.DescuentoPeso.ToString("N2", System.Globalization.CultureInfo.InvariantCulture), fuenteMono, 620, yFactores);
                yFactores += 15;
            }

            // Línea divisoria bajo la tabla de factores (mismo ancho que la tabla)
            g.DrawLine(lapizGrueso, 88, 505, 689, 505);

            // Fila de Total Descto:
            DibujarTextoDerecha("Total Descto:", fuenteMonoBold, 414, 511);
            DibujarTextoDerecha(d.TotalDescPorcentaje.ToString("N2", System.Globalization.CultureInfo.InvariantCulture), fuenteMonoBold, 507, 511);
            DibujarTextoDerecha(d.TotalDescPeso.ToString("N2", System.Globalization.CultureInfo.InvariantCulture), fuenteMonoBold, 617, 511);

            // =========================================================================
            // 4. ESPACIO PARA FIRMAS
            // =========================================================================
            const float yLineaFirma = 575;

            // Transportista
            g.DrawLine(lapizGrueso, 84, yLineaFirma, 246, yLineaFirma);
            CentrarTexto("Transportista", fuenteMono, 581, (84 + 246) / 2f);

            // Remitente / Usuario
            g.DrawLine(lapizGrueso, 395, yLineaFirma, 557, yLineaFirma);
            var usuario = string.IsNullOrWhiteSpace(d.UsuarioRegistro) ? "manager" : d.UsuarioRegistro;
            CentrarTexto(usuario, fuenteMono, 581, (395 + 557) / 2f);
            CentrarTexto("Remitente", fuenteMono, 597, (395 + 557) / 2f);

            // Recibidor y Fecha/Hora de emisión
            g.DrawLine(lapizGrueso, 709, yLineaFirma, 874, yLineaFirma);
            CentrarTexto("Recibidor", fuenteMono, 581, (709 + 874) / 2f);
            var fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            CentrarTexto(fechaHora, fuenteMono, 608, (709 + 874) / 2f);
        }
    }
}
