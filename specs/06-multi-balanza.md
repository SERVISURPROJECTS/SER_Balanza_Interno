# 06 — Multi-balanza

## Contexto

El modelo de datos y los controllers están preparados para que **una sola instalación** maneje
varias básculas físicas (reporta a una misma base compartida, ej. Postgres central), cada una con
su propia configuración de conexión (red o serial) y su propio contador de documentos — pero hoy
la báscula activa se elige **una vez por sesión**, no hay lectura automática de peso desde el
hardware (ver [[02-pesaje]]).

## Requisitos

1. Una instalación puede tener configuradas N balanzas (`balanza.activo = true` las habilitadas
   para elegir).
2. Al loguearse, el usuario elige la balanza activa de esa sesión (o ninguna).
3. Los pesajes y el contador de documentos quedan **marcados** con la balanza activa al momento de
   guardarlos.
4. Los listados/búsquedas de pesajes de `FrmPeso` solo muestran los de la balanza activa de la
   sesión (si hay una elegida).
5. Si no hay balanza activa (ninguna configurada, o el usuario no eligió ninguna), el sistema
   sigue funcionando sin acotar por balanza — no es un requisito bloqueante.

## Diseño actual

### Entidad `balanza` (`Models/balanza.cs`)

Guarda nombre, descripción/dirección/teléfono/email (para la boleta, ver [[04-boleta-impresion]])
y parámetros de conexión física: `tipo_conexion`, `ip`/`puerto` (red) o `puerto_serial`/`baudRate`/
`dataBits`/`parity`/`stopBits` (serial) — preparado para una integración de lectura automática que
**no está implementada** todavía (ver [[02-pesaje]], "Obtener Peso" deshabilitado).

`BalanzaController.ObtenerActivas()` es el único método del controller: trae las `activo == true`
ordenadas por `Descripcion`.

### Selección al login (`Views/FrmSeleccionarBalanza.cs`)

- Se muestra **una sola vez, al loguearse** (invocado desde `FrmLogin.btnIngresar_Click` justo
  después de autenticar) — no se vuelve a pedir en medio de la sesión.
- Combo con las balanzas activas (`Descripcion` visible, `id` como valor).
- Si no hay ninguna activa, igual deja continuar sin elegir (combo deshabilitado).
- Al aceptar, `FrmLogin` asigna `Sesion.BalanzaActual`.

### Alcance en `Sesion` (`Data/Sesion.cs`)

`Sesion.BalanzaActual` (`balanza?`) puede quedar `null` (sin balanzas activas, o diálogo no
confirmado) — todo el filtrado de abajo trata `null` como "sin restricción", no como error.

### Filtrado de pesajes (`PesoController.cs`)

```csharp
private static IQueryable<peso> FiltrarPorBalanzaActual(IQueryable<peso> query)
{
    var idBalanza = Sesion.BalanzaActual?.id;
    return idBalanza is null ? query : query.Where(p => p.Id_balanza == idBalanza);
}
```

Aplicado en `GetUltimos` y `Buscar` (la cola de la pantalla y la búsqueda por filtros). Al guardar
(`GuardarInterno`), `entidad.Id_balanza = Sesion.BalanzaActual?.id` — cada pesaje queda etiquetado
con la balanza que estaba activa en el momento de guardarlo (o `null` si no había ninguna).

### Documento por balanza (`CatalogoHelper.ObtenerOCrearDocumentoPesaje`)

Busca/crea el `documento` (tipo de documento de despacho) filtrando por **nombre + `Id_balanza`**
a la vez — el mismo nombre de documento (ej. "PESAJE BALANZA") es una fila distinta por cada
balanza, para no mezclar el contador/numeración entre básculas distintas. Se le pasa
`Sesion.BalanzaActual?.id` desde `PesoController.GuardarInterno`.

## Fuera de alcance actual

- No hay forma de cambiar de balanza activa sin cerrar sesión y volver a entrar.
- `Usuario.id_balanza` existe como columna pero no se usa en el flujo de login/sesión (no se lee
  para preseleccionar la balanza del usuario) — es un campo heredado sin consumidor activo hoy.
- No hay lectura automática de peso desde el hardware configurado en `balanza` (ver [[02-pesaje]]).
