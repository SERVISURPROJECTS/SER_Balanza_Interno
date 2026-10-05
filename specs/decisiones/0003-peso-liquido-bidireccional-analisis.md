# 0003 — Peso Líquido: sincronización bidireccional Peso ↔ Análisis

## Contexto

Peso Líquido (= Peso Neto − Descuento) puede originarse en dos lugares: cargado a mano en
`FrmPeso` (antes de que exista un análisis de laboratorio), o calculado en `FrmAnalisis` a partir
de los factores de calidad. Antes de esta decisión, `BoletaController` recalculaba el descuento
siempre desde el `Analisis` vinculado, ignorando lo que hubiera guardado en `peso` — si ese
`Analisis` estaba desactualizado o mal cargado, la boleta imprimía un Peso Líquido absurdo
(se detectó con un caso real: descuento de 420,168 sobre un neto de 30,000, dando líquido
negativo).

## Decisión

Regla única, aplicada en los tres puntos que tocan estos campos:

1. **Si el pesaje no tiene Análisis registrado todavía**: `FrmPeso` es la fuente de verdad. El
   operador puede escribirlo a mano o dejar que se autocalcule (Neto − Descuento).
2. **Si el pesaje ya tiene un Análisis registrado**: ese Análisis manda. Al guardar el Análisis,
   su Total Descuento/Peso Líquido se **propagan** al `peso` asociado
   (`AnalisisController.GuardarInterno`), así que no hace falta reabrir `FrmPeso` a mano para que
   quede consistente.
3. **La boleta imprime lo que está guardado en `peso`** (`BoletaController`), no lo recalcula
   desde `Analisis` en el momento de imprimir — evita que un análisis vinculado a datos viejos o
   incorrectos arruine una boleta de un pesaje ya cerrado.

## Por qué así y no de otra forma

- Se descartó que la boleta siguiera leyendo siempre desde `Analisis`: es la causa directa del bug
  que motivó este ADR, y además obligaba a tener un `Analisis` cargado incluso para pesajes que no
  requieren análisis de calidad.
- Se descartó que `peso` mandara siempre (ignorando el Análisis): un análisis de laboratorio
  cargado después de pesar el camión es información más precisa y debe poder corregir lo que se
  había puesto a mano al pesar.
- La propagación ocurre **al guardar el Análisis**, no en una sincronización periódica ni al leer
  — así el estado consistente existe apenas se completa la acción que lo originó, sin ventanas de
  inconsistencia hasta la próxima vez que alguien abra algo.
- `FrmPeso.btnVerAnalisis_Click` guarda el pesaje (con lo que esté en pantalla, aunque no se haya
  tocado "Guardar" todavía) **antes** de abrir `FrmAnalisis`, porque Análisis relee siempre desde
  la base — sin este guardado previo, Análisis podía arrancar mostrando datos de la última vez que
  se guardó el pesaje, no los que el operador tenía tipeados en pantalla en ese momento.
