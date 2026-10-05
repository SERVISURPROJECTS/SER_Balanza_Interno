# 0006 — Pesos sin decimales en todo el sistema

## Contexto

Durante el trabajo sobre el ADR 0001 se probó primero preservar hasta 3 decimales (precisión de
gramo) en Bruto/Tara/Neto/Descuento/Peso Líquido, tanto en pantalla como al guardar e imprimir.

## Decisión

Los 5 valores de peso se redondean **siempre a número entero** — en el formulario de `FrmPeso`
(al mostrar, al recalcular, al guardar) y en la boleta impresa (`BoletaPrinter`, formato `"N0"`).
No se preserva ningún decimal en ningún punto del sistema para estos campos.

## Por qué así y no de otra forma

- Se probó la alternativa de 3 decimales primero (parecía más "correcto" técnicamente, precisión
  de gramo) pero se revirtió explícitamente: la báscula real de esta operación trabaja en
  resolución de kilos enteros: un decimal ahí no aporta información real, solo ruido visual
  (".00" en todos lados) y una fuente más de posible confusión de formato (ver ADR 0001, que nació
  justamente de un choque de convenciones de decimales/miles).
- Es una decisión de negocio (cómo trabaja la báscula física de esta empresa), no una limitación
  técnica — el modelo de datos sigue usando `double` y no se truncó la precisión del tipo; el
  redondeo es solo en la capa de presentación/entrada (`FrmPeso`, `BoletaPrinter`), no se
  reintrodujo un `Math.Round` dentro de `PesoController`/`AnalisisController`.
