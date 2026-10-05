# 03 — Análisis de calidad de grano

## Contexto

Pantalla aparte (`Views/FrmAnalisis.cs`) donde se carga el análisis de laboratorio de un pesaje ya
registrado: 8 factores de calidad (Humedad, Impureza, Partido, Dañado, Otro Color, Dañado por
Calor, Enfermo, Verde), cada uno con su parámetro límite (del producto), valor medido, % de
descuento y descuento en peso. El total de descuento resultante se sincroniza con el `peso`
asociado para que la boleta imprima el valor correcto (ver [[04-boleta-impresion]]).

## Requisitos

1. Se busca el pesaje por N° de Pesaje o N° de Remito; si ya tiene un análisis guardado, se
   precarga (estado "C"=cerrado, bloqueado para edición salvo que se reabra a propósito).
2. Los descuentos por fila se cargan a mano (no hay fórmula automática por factor todavía — el
   `Desc_%`/`Descuento` de cada fila es manual); el Total (% y peso) se **suma** de las filas en
   vivo salvo que esté activo el Modo Manual.
3. Peso Líquido se muestra según la regla: si hay un Análisis con su propio Total Descuento
   calculado, ese manda; si no hay Análisis todavía, se usa el Peso Líquido ya guardado en el
   `peso` como referencia (o Peso Neto si tampoco hay eso) — ver
   [[decisiones/0003-peso-liquido-bidireccional-analisis]].
4. Al guardar, el Total Descuento (y el Peso Líquido mostrado) se propaga al `peso` asociado,
   para que quien solo abre `FrmPeso` (sin pasar por Análisis) vea el dato actualizado.
5. Un análisis cerrado ("C") se puede reabrir desde la UI (click en el badge de Estado) para
   corregirlo; al reabrir vuelve a "O" en pantalla y al guardar vuelve a quedar "C".
6. El checkbox **Modo Manual** permite cargar Total Descuento %/Total Descuento/Peso Líquido a
   mano sin que la edición de la grilla los recalcule — ver
   [[decisiones/0002-modo-manual-bypass-validaciones]] (misma idea que en Peso, aplicada acá).

## Diseño actual

### Grilla de factores (`InicializarGridFactores`)

5 columnas no auto-generadas: Factor (fija), Parámetro, Análisis, Descuento %, Descuento. 8 filas
fijas, una por factor. `grid.CellEndEdit` dispara `RecalcularTotales()` (salvo Modo Manual).

### Carga de parámetros por producto (`CargarParametrosProducto`)

Al elegir/perder foco el combo de Producto, copia los 8 pares (parámetro, factor de descuento) del
`Producto` elegido a la columna Parámetro de cada fila (el factor de descuento queda en
`Row.Tag`, no visible, para uso futuro de una fórmula automática que hoy no existe —
`CalcularDescuentoPorcentaje` está implementado en `AnalisisController` pero no se invoca desde la
UI actual).

### Totales y Peso Líquido (`RecalcularTotales`)

```csharp
private void RecalcularTotales()
{
    if (_modoManual) return;
    // suma DescuentoPorcentaje y Descuento de las 8 filas
    lblTotalPorcentaje.Text = totalPorcentaje...
    lblTotalPeso.Text = totalPeso...
    if (_pesoActual is not null)
        txtPesoLiquido.Text = (_pesoActual.Neto - totalPeso)...
}
```

`lblTotalPorcentaje`/`lblTotalPeso` son, pese al nombre `lbl*`, TextBox editables — el usuario
puede sobreescribir el total sumado antes de guardar (patrón consistente con `lblNeto` en
`FrmPeso`).

### Guardado (`AnalisisController.GuardarInterno`)

- Valida que el pesaje exista y que Producto no esté vacío (única validación dura).
- Persiste los 8 factores tal como están en la grilla (parámetro, valor medido, %, peso) +
  `TotalDescPorcent`/`TotalDescPeso` tal como se muestran en pantalla (si el usuario los
  sobreescribió a mano, se guarda ese valor, no un recálculo server-side).
- Al final, propaga a `peso`:
  ```csharp
  pesoRef.TotalDesc = nuevo.TotalDescPeso;
  pesoRef.PesoLiquido = input.PesoLiquido ?? (pesoRef.Neto - (nuevo.TotalDescPeso ?? 0));
  ```
  `input.PesoLiquido` es el valor que estaba en `txtPesoLiquido` al guardar (manual o calculado) —
  prevalece sobre el cálculo automático si el usuario lo tocó.
- El análisis queda `Estado = "C"` al guardar (cerrado).

### Vínculo con Peso (`btnVerPeso_Click`, `FrmPeso.btnVerAnalisis_Click`)

Navegación cruzada: desde Análisis se puede volver al pesaje (`new FrmPeso(nroConsec)`); desde
Peso se abre Análisis para el pesaje actual, pero primero se guarda el pesaje tal como está en
pantalla (ver [[02-pesaje]] y el ADR de sincronización).
