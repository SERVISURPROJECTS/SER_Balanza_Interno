# 04 — Boleta impresa

## Contexto

Al guardar un pesaje (o un análisis), se puede imprimir una boleta (`UI/BoletaPrinter.cs`) con los
datos del camión, los pesos, el detalle de descuentos por factor, y los datos de la empresa/
balanza. Los datos se arman en `Controllers/BoletaController.cs` (`ObtenerDatosBoleta`), que es el
único punto de la app que combina `peso` + `Analisis` + catálogos para presentación — no hay lógica
de negocio nueva acá, solo composición de lo que ya calcularon Peso y Análisis.

## Requisitos

1. La boleta imprime **exactamente** el Total Descuento y Peso Líquido que están guardados en el
   `peso` (no los recalcula a partir del `Analisis` vinculado) — ver
   [[decisiones/0003-peso-liquido-bidireccional-analisis]].
2. Si el `peso` todavía no tiene esos campos cargados (caso histórico/legado), cae a un cálculo de
   respaldo desde el `Analisis` más reciente vinculado.
3. El encabezado de la boleta muestra los datos de la **balanza usada en el pesaje**
   (nombre/dirección/teléfono propios de esa balanza), no los de la compañía genérica — salvo que
   el pesaje no tenga balanza asignada (registros previos a esa funcionalidad), en cuyo caso cae a
   los datos de `compania`.
4. El campo "Remitente" de la boleta muestra el **nombre completo** del usuario que registró el
   pesaje, no su username.
5. Bruto, Tara, Neto, Descuentos y Peso Líquido se imprimen **redondeados a entero**, sin
   decimales — ver [[decisiones/0006-pesos-redondeados-a-entero]].

## Diseño actual

### `BoletaController.ObtenerDatosBoleta(nroConsec)`

Resuelve, a partir del `peso`: compañía/balanza, vehículo, chofer, cliente, proveedor, procedencia,
destino, producto, tipo de documento, campaña, el `Analisis` más reciente vinculado
(`Analisis.PesoId == peso.NroConsec`, el de mayor `DocEntry`), y el usuario que registró
(`Id_usuarioIng`).

Puntos de diseño no obvios:

```csharp
// Descuentos/Peso Liquido: fuente de verdad es el campo guardado en el pesaje, no un recalculo
// desde Analisis (que puede estar vinculado a otro pesaje o desactualizado).
var totalDescPeso = peso.TotalDesc ?? analisis?.TotalDescPeso ?? 0;
var pesoLiquido = peso.PesoLiquido ?? (peso.Neto - totalDescPeso);
```

```csharp
CompaniaNombre = balanza?.Descripcion is { Length: > 0 } ? balanza.Descripcion : (compania?.nombre ?? "SERVISUR"),
// ... mismo patron para Direccion/Telefono/Email
```

```csharp
UsuarioRegistro = usuarioRegistro?.Nombre ?? "",  // nombre completo, no usuarioRegistro.usuario
```

### `BoletaPrinter` (render + impresión/vista previa)

- Dibuja sobre un `Bitmap`/`Graphics` con coordenadas fijas (no hay motor de reportes; es dibujo
  manual con `DrawString` posicionado a mano) — cualquier cambio de layout implica tocar
  coordenadas numéricas en el código, no hay plantilla editable.
- Los 5 valores de peso se formatean con `"N0"` + `CultureInfo.InvariantCulture` (coma para miles,
  sin decimales) — ver [[decisiones/0001-formato-numerico-coma-miles]].
- Los factores de análisis (Parámetro/Análisis/Descuento%/Descuento por fila) se imprimen con
  `"N2"`/`"N3"` + `InvariantCulture` — sí llevan decimales, a diferencia de los 5 valores de peso
  principales (son magnitudes de otra naturaleza: porcentajes y mediciones de laboratorio, no
  kilos de báscula).
- Muestra "Obs. Peso Manual" si `peso.peso_manual` es `true` — en la práctica siempre lo es hoy
  (ver [[02-pesaje]]), así que esa leyenda aparece en todas las boletas actuales; no es un bug, es
  el estado real de la funcionalidad de lectura automática (no implementada).
