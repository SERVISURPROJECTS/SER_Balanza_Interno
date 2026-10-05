# 0001 — Coma = separador de miles, punto = decimal

## Contexto

Los campos de peso en `FrmPeso` necesitan un formateo de miles en vivo mientras se escribe (ej.
tipear "45000" y que se vea "45,000"), y ese mismo texto se reparsea al guardar. Hubo un bug
real donde `lblNeto`/`txtPesoLiquido` se formateaban con la cultura regional de Windows (`N0` sin
proveedor explícito) pero se reparseaban con una convención fija distinta (`FormatoPeso`) —
cuando ambas no coincidían, una coma de miles se reinterpretaba como el punto decimal fijo,
truncando el valor (ej. 27000 guardado como 27).

## Decisión

Una sola convención, fija, usada **siempre** (tipeo en vivo, recarga de un registro guardado, e
impresión): coma = separador de miles, punto = separador decimal — independiente de la
configuración regional de Windows. Implementada en `FrmPeso.cs` como `NumberFormatInfo FormatoPeso`
y usada explícitamente (nunca el `ToString()`/`TryParse` default) en cada conversión
texto↔número de estos campos.

## Por qué así y no de otra forma

- Se probó primero con punto=miles/coma=decimal (convención hispanoamericana típica), pero se
  decidió cambiar a coma=miles/punto=decimal porque es lo que se imprime en la boleta
  (`BoletaPrinter` ya usaba `CultureInfo.InvariantCulture`, que es coma/punto) — unificar evita
  que la pantalla y el papel impreso muestren el mismo número con separadores distintos.
- Se descartó depender de `CultureInfo.CurrentCulture` (la configuración regional de Windows)
  porque es exactamente la causa raíz del bug que motivó este ADR: dos puntos del código usando
  cada uno "la cultura por defecto" pueden terminar en culturas distintas si una de las dos pasa
  un `IFormatProvider` explícito y la otra no.

## Cómo aplicarlo

Cualquier `ToString`/`TryParse` nuevo sobre Bruto/Tara/Neto/Descuento/PesoLiquido en `FrmPeso.cs`
debe pasar `FormatoPeso` explícitamente. Si aparece un campo de peso nuevo en otra pantalla, debe
usar la misma convención (coma/punto), no la cultura del sistema.
