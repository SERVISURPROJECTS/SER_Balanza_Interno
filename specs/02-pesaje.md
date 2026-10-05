# 02 — Pesaje

## Contexto

Es la pantalla principal (`Views/FrmPeso.cs` + `.Designer.cs`), donde el operador registra cada
camión que pasa por la báscula: datos del camión/carga + los cinco valores de peso (Tara, Bruto,
Neto, Total Descuento, Peso Líquido).

## Requisitos

1. Neto se calcula como Bruto − Tara, y Peso Líquido como Neto − Total Descuento, **en vivo**
   mientras se edita el formulario, salvo que el pesaje ya tenga un Análisis registrado (ese manda,
   ver [[03-analisis-calidad]]) o esté activo el Modo Manual.
2. Bruto debe ser ≥ Tara para guardar — **salvo** que el Modo Manual esté activo.
3. Todos los campos de peso se muestran y se guardan con la misma convención numérica en todo el
   sistema (ver [[decisiones/0001-formato-numerico-coma-miles]]) y siempre redondeados a entero
   (ver [[decisiones/0006-pesos-redondeados-a-entero]]).
4. El operador puede forzar un registro manual completo (los 5 valores, sin ninguna de las
   validaciones/cálculos de 1-2) vía el checkbox **Modo Manual** — ver
   [[decisiones/0002-modo-manual-bypass-validaciones]].
5. Al presionar "Ver Análisis" sobre un pesaje cargado, el pesaje se guarda primero (con los
   valores tal como están en pantalla) antes de abrir `FrmAnalisis`, para que ambas pantallas
   arranquen sincronizadas — ver [[decisiones/0003-peso-liquido-bidireccional-analisis]].
6. Los pesajes mostrados en la cola/búsqueda están acotados a la balanza activa de la sesión
   (ver [[06-multi-balanza]]).

## Diseño actual

### Campos y su control

| Campo | Control | Tipo | Notas |
|---|---|---|---|
| Peso Tara | `txtTara` | TextBox editable | dispara `RecalcularNeto` |
| Peso Bruto | `txtBruto` | TextBox editable | dispara `RecalcularNeto` |
| Peso Neto | `lblNeto` | TextBox (pese al nombre `lbl*`, es editable) | autocalculado salvo Modo Manual/Análisis registrado |
| Total Descuento | `txtTotalDescuento` | TextBox editable | dispara `RecalcularPesoLiquido` |
| Peso Líquido | `txtPesoLiquido` | TextBox editable | autocalculado salvo Modo Manual/Análisis registrado |

Todos comparten el formateo de miles en vivo (`FormatearMiles_TextChanged`) y la restricción de
tecla (`SoloNumeroConComa_KeyPress`, pese al nombre ya no restringe a coma sino al separador
decimal configurado — ver ADR 0001).

### Flags de estado del formulario (`FrmPeso.cs`)

- `_analisisRegistrado` (bool): `true` si el pesaje cargado ya tiene un `Analisis` asociado.
  Bloquea `RecalcularPesoLiquido` (el Análisis manda).
- `_modoManual` (bool): estado del checkbox `chkModoManual`. Bloquea tanto `RecalcularNeto` como
  `RecalcularPesoLiquido`, y se pasa a `PesajeInput.ModoManual` para que
  `PesoController.GuardarInterno` omita la validación Bruto≥Tara.
  - Se activa solo (`true`) automáticamente al cargar un pesaje ya existente
    (`CargarPesajeEnFormulario`), para no reinterpretar con la fórmula valores ya guardados.
  - Arranca en `false` (automático) al crear un pesaje nuevo (`LimpiarFormulario`/`btnNuevo_Click`).
  - El usuario puede tildar/destildar libremente en cualquier momento.

### Guardado (`PesoController.GuardarInterno`, `Controllers/PesoController.cs`)

- Resuelve/crea catálogos referenciados por nombre (vehículo, chofer, producto, proveedor,
  cliente, procedencia, destino, hacienda, campaña, documento) vía `CatalogoHelper`.
- Validación dura (no bypasseable): placa de vehículo obligatoria.
- Validación soft (bypasseable con `ModoManual`): `Bruto <= 0 || Tara < 0 || Bruto < Tara`.
- `entidad.peso_manual = true` siempre — no hay en esta versión un camino de lectura automática
  desde una balanza física (el botón "Obtener Peso" y el panel LED del formulario están
  deshabilitados/ocultos: son UI preparada para una integración futura, no funcional hoy).
- Filtra listados (`GetUltimos`, `Buscar`) por la balanza activa de la sesión
  (`FiltrarPorBalanzaActual`, ver [[06-multi-balanza]]).

### Catálogos embebidos en la pantalla (botones "+")

Cliente/Proveedor y Procedencia/Destino usan `CrudFormLauncher.OpenFiltrado<T>` para abrir el CRUD
genérico acotado a un subconjunto de filas de la misma tabla física (`socio_negocio`,
`origen_destino`) — ver [[decisiones/0005-crud-origen-destino-separado]].
