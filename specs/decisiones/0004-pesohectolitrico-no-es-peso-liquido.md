# 0004 — `PesoHectolitrico` dejó de alimentar el campo "Peso Líquido"

## Contexto

En `FrmAnalisis`, el campo visible con la etiqueta "Peso Líquido" (`txtPesoLiquido`) estaba
conectado en código a `Analisis.PesoHectolitrico` — una columna real del esquema que en realidad
representa el peso hectolítrico (densidad de prueba del grano, kg/hl), un concepto de calidad
completamente distinto de "Peso Neto menos descuentos". Era un error de cableado, no una decisión
de diseño previa: la etiqueta decía una cosa y el dato guardado era otro.

## Decisión

El campo "Peso Líquido" de `FrmAnalisis` ahora se comporta como su nombre indica (ver
[[0003-peso-liquido-bidireccional-analisis]]) y **ya no lee ni escribe** `Analisis.
PesoHectolitrico`. Esa columna queda en el esquema/modelo (no se borró) pero sin ningún punto de
la UI que la alimente hoy.

## Por qué así y no de otra forma

- Se consideró agregar un campo nuevo separado para Peso Hectolítrico y mantener ambos — se
  descartó por ahora porque nadie pidió esa funcionalidad; agregarla habría sido inventar alcance
  no solicitado. Si en el futuro hace falta capturar el peso hectolítrico real, se puede reusar
  esa misma columna con un campo de UI propio y bien etiquetado.
- No se eliminó la columna `PesoHectolitrico` del modelo/esquema por prudencia: borrar una columna
  con datos históricos ya guardados (de instalaciones existentes) es una operación destructiva que
  no se justificaba para resolver un problema de cableado de UI.
