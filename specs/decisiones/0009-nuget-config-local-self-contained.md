# 0009 — `NuGet.Config` local para permitir publish self-contained

## Contexto

Al intentar `dotnet publish --self-contained true` por primera vez, falló con
`NU1100: Unable to resolve 'Microsoft.NETCore.App.Runtime.win-x64'` (y paquetes equivalentes de
WindowsDesktop/AspNetCore). La máquina de desarrollo tiene configurado `packageSourceMapping` a
nivel de usuario (`%APPDATA%\NuGet\NuGet.Config`) que restringe qué paquetes puede traer cada
fuente — y esos tres paquetes de runtime no estaban habilitados desde `nuget.org` en esa config
global (usada por muchos otros proyectos de la máquina, no solo este).

## Decisión

Un `NuGet.Config` **local al proyecto** (en la raíz del repo, versionado en git) que agrega
patrones de `packageSourceMapping` adicionales para `nuget.org` — específicamente los paquetes de
runtime necesarios para self-contained (`Microsoft.NETCore.App.Runtime.win-x64`,
`Microsoft.WindowsDesktop.App.Runtime.win-x64`, `Microsoft.AspNetCore.App.Runtime.win-x64`, y
otros que fueron apareciendo: `SQLitePCLRaw.*`, `System.Security.Cryptography.ProtectedData`,
`runtime.any.*`, etc.). NuGet combina los `packageSourceMapping` de la config de proyecto con la
de usuario para la misma fuente — no hace falta repetir ni *clear*-ear los patrones ya permitidos
globalmente, solo sumar los que faltan.

## Por qué así y no de otra forma

- Se descartó modificar la config global de NuGet del usuario (`%APPDATA%\NuGet\NuGet.Config`):
  es compartida por todos los demás proyectos de esa máquina; ampliarla para resolver un problema
  de este proyecto puntual habría sido un cambio con efecto secundario fuera del repo, invisible
  para cualquier otra persona que clone este proyecto en otra máquina.
- Con el `NuGet.Config` local, cualquiera que clone el repo y corra `dotnet publish
  --self-contained` tiene exactamente el mismo comportamiento sin tocar nada fuera del proyecto.
