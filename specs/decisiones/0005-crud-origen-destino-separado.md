# 0005 — CRUD de Procedencia/Destino separado (misma tabla física)

## Contexto

`origen_destino` es una sola tabla que guarda tanto procedencias como destinos, distinguidos por
el flag `es_destino`. El CRUD genérico original abría una sola grilla mezclando ambos tipos de
fila, obligando al usuario a buscar entre filas que no le interesaban (ej. abrir "Destino" y ver
también las procedencias).

## Decisión

Dos entradas de menú/botón separadas ("Procedencia" y "Destino"), cada una abriendo el mismo CRUD
genérico pero **filtrado**: `CrudFormLauncher.OpenFiltrado<origen_destino>(filtro, alCrear, ...)`
— la grilla solo muestra las filas que cumplen el filtro, y los registros nuevos creados desde esa
pantalla se marcan automáticamente con el flag correcto (`alCrear`).

## Por qué así y no de otra forma

- Se replicó el mismo patrón ya existente para `socio_negocio` (Cliente/Proveedor, distinguidos
  por `es_cliente`/`es_proveedor`) en vez de inventar uno nuevo — consistencia con una solución
  que ya funcionaba para el mismo problema estructural (una tabla, dos roles por flag).
- Se descartó partir `origen_destino` en dos tablas separadas: habría significado una migración de
  esquema y tocar todas las FK existentes (`peso.id_origen`/`id_destino`) para un problema que es
  puramente de presentación en la UI, no del modelo de datos.
