# 00 — Overview

## Qué es

`SER_Balanza_Interno` es una aplicación de escritorio (WinForms, .NET 8) para el registro de
pesaje de camiones en una báscula de granos (Servisur Agrícola). Reemplaza una planilla/proceso
manual de báscula: registra el peso bruto/tara/neto de cada camión, permite un análisis de calidad
de grano (descuentos por humedad, impurezas, etc.) y emite una boleta impresa con el resultado
final (Peso Líquido = Peso Neto − Descuentos).

## Para quién

- **Operador de báscula**: usuario del día a día. Pesa camiones, carga análisis de calidad,
  imprime boletas. Acceso restringido a Peso y Análisis (ver [[05-usuarios-roles-permisos]]).
- **Administrador**: gestiona catálogos (clientes, proveedores, productos, vehículos, choferes,
  balanzas), usuarios/roles, y tiene acceso completo.
- **Operador remoto**: corre la app en una máquina fuera de la oficina central, sin acceso directo
  a la red interna salvo una carpeta compartida puntual (el "casillero") usada para
  autoactualización — ver [[08-autoactualizacion]].

## Alcance funcional actual

| Módulo | Spec |
|---|---|
| Modelo de datos | [[01-modelo-datos]] |
| Pesaje (Bruto/Tara/Neto/Descuento/Líquido) | [[02-pesaje]] |
| Análisis de calidad de grano | [[03-analisis-calidad]] |
| Boleta impresa | [[04-boleta-impresion]] |
| Usuarios, roles y permisos | [[05-usuarios-roles-permisos]] |
| Multi-balanza | [[06-multi-balanza]] |
| Proveedores de datos (Sqlite/Postgres/SqlServer) | [[07-proveedores-datos]] |
| Autoactualización | [[08-autoactualizacion]] |
| Instalador | [[09-instalador]] |

Decisiones puntuales con su razón de ser (por qué se hizo así y no de otra forma) están en
`decisiones/` como ADRs cortos — ver el índice en [[decisiones/00-indice]].

## Convenciones de este set de specs

- Cada spec de feature describe el **estado actual** del sistema (no un plan a futuro). Si cambia
  el comportamiento, la spec correspondiente se actualiza en el mismo cambio de código.
- Las referencias a archivos son relativas a la raíz del repo.
- Las decisiones no obvias (el "por qué", no el "qué") van en un ADR aparte, enlazado desde la
  spec de feature que corresponda — así la spec de feature no se satura de justificaciones
  históricas y el ADR queda buscable por tema.
