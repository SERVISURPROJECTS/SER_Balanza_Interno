# 05 — Usuarios, roles y permisos

## Contexto

Control de acceso basado en roles (nombre de rol, no un enum) + una tabla de permisos granulares
opcional para casos puntuales (hoy solo dos: editar pesaje existente, administrar balanzas). El
rol "admin" tiene siempre acceso total, sin pasar por la tabla de permisos.

## Requisitos

1. Login por usuario/password; solo usuarios habilitados y no eliminados pueden entrar.
2. El rol "admin" (case-insensitive) tiene acceso total siempre, sin importar qué haya
   configurado en `permiso_rol`.
3. Cualquier otro rol solo accede a lo que el nombre de rol habilita por hardcode (ver UI gating
   abajo) + los permisos puntuales que se le asignen explícitamente desde la pantalla de
   Roles/Permisos.
4. Un permiso referenciado en código que todavía no existe en la tabla se **autocrea** la primera
   vez que se consulta (no hace falta un seed manual por cada permiso nuevo que se agregue al
   código).

## Diseño actual

### Entidades

- `Usuario` (`Models/Usuario.cs`): `Id`, `Nombre`, `usuario` (login), `PasswordHash`,
  `Habilitado`, `Eliminado` (soft delete), `id_rol`, `id_balanza` (campo presente en la entidad
  pero **no usado** por el flujo de login/sesión actual — la balanza de sesión se elige en
  `FrmSeleccionarBalanza`, no se lee de acá), `Ci`.
- `rol` (`Models/rol.cs`): `Id`, `Nombre`, `activo`, `eliminado`. Sin relaciones de navegación —
  el nombre se compara como string, case-insensitive (`"admin"`, `"operador"`).
- `Permiso` (`Models/Permiso.cs`): `id` (asignado a mano, no identity), `nombre`, `es_grupo`,
  `grupo`.
- `permiso_rol` (`Models/permiso_rol.cs`): tabla puente pura, PK compuesta `(id_permiso, id_rol)`.

### Autenticación (`Controllers/AuthController.cs`)

- `Login(usuario, password)`: busca por `usuario` (no eliminado), rechaza si `Eliminado`,
  `!Habilitado`, o si el hash no coincide.
- `Hash(password)`: **SHA-256 plano** del UTF8, hex uppercase (`Convert.ToHexString`) — sin sal,
  sin iteraciones. No es bcrypt/Argon2. Aceptable para esta app (acceso local, no expuesta a
  internet) pero a tener en cuenta si se expone a un escenario de mayor riesgo.
- `UsuarioController.Create/Update` usa el mismo `Hash` al guardar contraseña.

### Permisos (`Controllers/PermisoController.cs`)

- Constantes: `EditarPesaje = "Editar Pesaje"`, `AbmBalanza = "ABM Balanza"`.
- `TienePermiso(idRol, nombrePermiso)`:
  1. `idRol == null` → `false`.
  2. Rol "admin" → siempre `true` (bypass total, no consulta `permiso_rol`).
  3. Si el permiso no existe en la tabla, se **autocrea** (siguiente id = max+1) y se asigna
     automáticamente al rol admin — y la consulta actual devuelve `false` (el rol no-admin que
     disparó la autocreación no queda con el permiso, solo admin).
  4. Si existe, devuelve si hay una fila `permiso_rol` que une `idRol` con ese `permiso.id`.
- `RolPermisoController.AsegurarPermiso(nombre)` hace el mismo autocreate (sin asignarlo a admin)
  — se usa en el constructor de `FrmRolesPermisos` para que "Editar Pesaje" y "ABM Balanza"
  aparezcan listados en la UI de administración incluso si nadie disparó su autocreación todavía
  por el uso normal.

### Administración de roles/permisos (`FrmRolesPermisos.cs` + `RolPermisoController.cs`)

- Combo de roles (no eliminados) → lista con checkboxes de **todos** los permisos, marcando los ya
  asignados (`ObtenerPermisosDeRol`).
- Guardar (`GuardarPermisosDeRol`) hace diff y agrega/quita filas en `permiso_rol`.
- "Nuevo Rol" abre el CRUD genérico (`CrudFormLauncher.Open(typeof(rol))`).

### CRUD de usuarios (`FrmUsuarios.cs` + `UsuarioController.cs`)

- Grilla excluye `Eliminado`; columnas Id (oculta), Nombre, Usuario, Rol (**id crudo, no
  resuelto a nombre** — limitación conocida de la UI actual), Habilitado.
- Combo de rol: solo roles `activo && !eliminado`.
- Baja = soft-delete (`Eliminado = true; Habilitado = false`), no hay hard-delete desde la UI.
- Dejar la contraseña en blanco al editar preserva el hash existente (no la pisa con el hash de
  cadena vacía).

### Login y selección de balanza (`FrmLogin.cs`)

`btnIngresar_Click`: `AuthController.Login` → si OK, `Sesion.UsuarioActual = usuario` → abre
`FrmSeleccionarBalanza` modal → si se acepta, `Sesion.BalanzaActual = balanza elegida` (ver
[[06-multi-balanza]]) → abre `FrmPeso` y cierra el login. La selección de balanza es **opcional**:
si se cancela el diálogo o no hay balanzas activas, `BalanzaActual` queda `null` y la app sigue
funcionando sin acotar por balanza.

### Gating de UI por rol (`FrmPeso.AplicarPermisosPorRol`)

- `tsBalanzas.Enabled` = `PermisoController.TienePermiso(idRol, AbmBalanza)` — cualquier rol al
  que se le otorgue ese permiso puede administrar balanzas, no es exclusivo de admin.
- `mnuRolesPermisos.Enabled` = `true` **solo** si el nombre de rol es literalmente `"admin"`
  (hardcode, no pasa por la tabla de permisos) — administrar roles/permisos es exclusivo del rol
  admin por diseño.
- Si el rol es `"operador"`: se deshabilitan los menús Ver/Herramientas/Reportes/Usuarios/Catálogos
  y los botones "+"/toolbar de catálogos (Usuario, Cliente, Proveedor, Producto, Vehículo, Chofer,
  Procedencia, Destino). Intención declarada en el código: el operador "solo opera Peso y
  Análisis".
- Cualquier otro nombre de rol que no sea "admin" ni "operador" queda con todo habilitado salvo lo
  que el chequeo de permiso puntual restrinja.

### Sesión (`Data/Sesion.cs`)

Holder estático de vida de proceso (no persiste entre reinicios de la app):
`UsuarioActual`, `BalanzaActual`, `HayUsuarioActivo` (conveniencia), `Cerrar()` (limpia ambos). No
se encontró una re-selección de balanza a mitad de sesión — se elige una vez al loguearse.
