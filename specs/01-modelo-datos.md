# 01 — Modelo de datos

## Contexto

El modelo usa EF Core con tabla-por-clase, mapeado 1:1 a columnas existentes (atributos
`[Table]`/`[Column]` explícitos en cada clase bajo `Models/`). Los nombres de clase/columna no son
consistentes en casing (`peso`, `balanza`, `chofer` en minúscula vs. `Usuario`, `Producto`,
`Vehiculo` en PascalCase) porque el esquema reutiliza tablas de un sistema previo — no es un
criterio de diseño nuevo, es historia heredada.

## Entidad central: `peso` (tabla `peso`)

Un registro = un pesaje de un camión. Campos clave:

- `NroConsec` (PK), `NroPesaje`, `NroTicket` (N° de remito)
- Pesos: `Bruto`, `Tara`, `Neto` (todos `double`, no nullable)
- Descuento/líquido (nullable, se completan a mano o desde Análisis):
  `TotalDesc`, `PesoLiquido`
- Catálogo (todas FK opcionales salvo donde se indica):
  `id_Vehiculo`, `id_Chofer`, `id_Proveedor`, `Id_cliente`, `Id_producto`,
  `id_origen`/`id_destino` (procedencia/destino, ver `origen_destino`),
  `Id_Hacienda`, `id_campania`, `id_documento` (no nullable — siempre se resuelve al guardar),
  `Id_balanza` (ver [[06-multi-balanza]])
- Auditoría: `FechaIngreso`, `FechaSalida`, `Id_usuarioIng`, `Id_UsuarioSal`
- `peso_manual` (bool): hoy siempre `true` al guardar desde `PesoController` — no hay camino de
  lectura automática de balanza activo en esta versión (ver [[02-pesaje]])
- `Nulo` (bool): soft-delete, no hay UI que lo setee a `true` todavía
- Campos presentes en el modelo pero sin uso de negocio activo observado:
  `PesoUnidSec`/`unidadSecundaria`, `Importe`, `TipoTara`/`PesoTara`, `Credito`, `Lote`,
  `ModeService`/`produccion`, `PesoInKey` (vínculo a `pesoin`, tabla de pesaje "de entrada"
  separada, no integrada al flujo actual), `cultivo`

## `Analisis` (tabla `Analisis`)

Un registro = el análisis de calidad de un `peso` (`PesoId`, FK lógica sin constraint declarada
en el modelo EF — la relación se resuelve a mano con queries, no con navigation properties).

- 8 factores de calidad, cada uno con 4 columnas paralelas (prefijo `P`=parámetro límite del
  producto, sin prefijo=valor medido, `Desc_`=% de descuento, `DescWeight`=descuento en peso):
  Humedad, Impureza, Partido, Danado, OtroColor, DCalor, Enfermo, Verde
- Totales: `TotalDescPorcent`, `TotalDescPeso` — ver [[03-analisis-calidad]] para cómo se calculan
  y cómo se sincronizan con `peso.TotalDesc`/`peso.PesoLiquido`
- `Estado` ("O"=abierto/editable, "C"=cerrado) y `Cancelado` — ver [[03-analisis-calidad]]
- `PesoHectolitrico`: columna legada, ya no se escribe desde la UI (ver
  [[decisiones/0004-pesohectolitrico-no-es-peso-liquido]])

## `Producto` (tabla `Producto`)

Catálogo de granos. Trae, por cada uno de los 8 factores de calidad, un **parámetro límite**
(ej. `Humedad`) y un **factor de descuento** (ej. `FDHumedad`) — son los valores que
`FrmAnalisis.CargarParametrosProducto` precarga en la grilla al elegir el producto.

## Catálogos de apoyo

| Tabla/Clase | Para qué | Campo distintivo |
|---|---|---|
| `socio_negocio` | Clientes y proveedores (misma tabla) | `es_cliente`, `es_proveedor` (un registro puede ser ambos) |
| `origen_destino` | Procedencia y destino (misma tabla) | `es_destino` (false=procedencia, true=destino) — ver [[decisiones/0005-crud-origen-destino-separado]] |
| `Vehiculo` | Camiones | `Placa`, `Tara` (tara de referencia, no siempre usada) |
| `chofer` | Choferes | `ci` |
| `Hacienda` | Centro de acopio/propiedad agrícola | — |
| `campania` | Campaña agrícola (temporada), `sigla`, vigencia `valido_desde`/`valido_hasta` | — |
| `documento` | Tipo de documento de despacho, ligado a una balanza (`Id_balanza`) | `modo_transaccion` |
| `balanza` | Báscula física/lógica — ver [[06-multi-balanza]] | — |
| `compania` | Datos de la empresa (nombre, dirección, teléfono) para la boleta | `activo` (una sola fila activa a la vez) |

## Usuarios, roles, permisos

Ver [[05-usuarios-roles-permisos]] para `Usuario`, `rol`, `Permiso`, `permiso_rol`.

## Tablas presentes en el modelo sin flujo de negocio activo

`pesoin`, `factor`, `indicador`, `numeracion`, `Marca`, `prod_fac_rank`,
`log_transferencia_pesaje`: existen como clases EF (mapeadas a tablas reales) pero no tienen un
formulario o controller que las use en el flujo actual más allá de lectura puntual
(`indicador` se referencia solo al sembrar `balanza.id_indicador = 1` en el seed). Se documentan
acá para que no se asuma que son dead code a borrar sin verificar primero si otro consumidor
(reporte externo, vista SQL) depende de ellas.
