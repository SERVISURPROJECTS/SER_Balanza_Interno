# 0002 — Modo Manual en Peso y Análisis

## Contexto

Las fórmulas automáticas (Neto = Bruto − Tara, Peso Líquido = Neto − Descuento) y la validación
Bruto ≥ Tara cubren el caso normal, pero hay situaciones reales donde el operador necesita cargar
un registro que no cumple esas reglas (corrección de un dato histórico, un caso de báscula/proceso
fuera de lo normal) y el sistema se lo impedía por completo.

## Decisión

Un checkbox "Modo Manual" en `FrmPeso` y en `FrmAnalisis`, independiente entre ambas pantallas,
que al activarse:

- Deja de recalcular los campos derivados (Neto/Peso Líquido en Peso; Totales/Peso Líquido en
  Análisis) cada vez que cambia un campo fuente — el usuario escribe los 5 (o 3) valores
  directamente.
- En Peso, además, hace que `PesoController` **omita** la validación Bruto ≥ Tara (`PesajeInput.
  ModoManual`). La validación de placa de vehículo obligatoria **no** se omite — eso no es parte
  de "la fórmula", es un dato mínimo de identificación del registro.

## Por qué así y no de otra forma

- Se consideró permitir editar los campos derivados directamente sin un checkbox (ya eran
  TextBox editables) pero el problema no era que no se pudieran tocar, sino que cualquier cambio
  en Bruto/Tara/Descuento los **volvía a pisar** automáticamente — de ahí la necesidad de un flag
  explícito que el usuario controla, no solo "hacerlos editables".
- Se activa `true` automáticamente al **cargar un registro ya guardado** (en ambas pantallas): si
  no, reabrir un registro con valores legítimamente manuales (ej. Bruto < Tara de un caso real)
  dispararía la fórmula automática y pisaría esos valores en pantalla apenas se repinta el
  formulario, antes de que el usuario toque nada.
- Arranca en `false` (automático) para un registro **nuevo**: es el caso común, no tiene sentido
  pedirle al operador que tilde algo para el flujo normal de todos los días.

## Alcance

No persiste en base de datos — es un estado de sesión de edición, no un atributo del registro. Si
se necesitara saber después "este pesaje se cargó en modo manual", habría que agregar una columna
nueva; hoy no existe esa necesidad declarada.
