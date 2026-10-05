# 0008 — Fallo transitorio de Inno Setup al publicar

## Contexto

`scripts/publish-and-package.ps1` falla intermitentemente, casi al final de la etapa de
compresión del instalador, con un error genérico de Inno Setup
("El sistema no puede encontrar el archivo especificado"). Pasó en varias publicaciones de
versión distintas (1.1.0, 1.1.1, 2.0.0, 2.1.3, 2.1.5, 2.1.9...), nunca en el mismo archivo. El
`dotnet publish` previo siempre había terminado bien; el contenido de la carpeta `publish/` estaba
intacto después del fallo.

## Decisión / diagnóstico

Es un falso positivo consistente con un antivirus (Windows Defender) escaneando en segundo plano
un DLL recién extraído del build (ej. `e_sqlcipher.dll`, un binario nativo de criptografía, perfil
típico de heurística de AV) justo en el instante en que Inno Setup intenta leerlo para
comprimirlo. No es un archivo realmente faltante ni corrupto.

**Cómo resolverlo cuando pase**: no repetir todo el script (no hace falta volver a correr
`dotnet publish`). Alcanza con reinvocar `ISCC.exe` directo contra la carpeta de publish que ya
existe:

```powershell
& "<ruta a>\ISCC.exe" "installer\setup.iss" "/DMyAppVersion=<version>" "/DMyPublishDir=<repo>\bin\Release\net8.0-windows\win-x64\publish"
```

En todos los casos observados, el reintento funcionó a la primera.

## Por qué no se "arregló" de raíz

- No se agregó una exclusión de Windows Defender para la carpeta del proyecto: es una decisión que
  afecta la postura de seguridad de la máquina del desarrollador, no algo que el código del
  proyecto deba decidir por su cuenta.
- No se agregó un retry automático dentro del script: el fallo es infrecuente y rápido de
  resolver a mano (un solo comando), y un retry automático "silencioso" podría ocultar un fallo
  real de compilación distinto detrás del mismo manejo. Se prefiere que quien publica vea el error
  y decida conscientemente reintentar.
