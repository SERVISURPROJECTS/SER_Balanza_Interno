# Índice de decisiones (ADRs)

| # | Decisión | Afecta a |
|---|---|---|
| [0001](0001-formato-numerico-coma-miles.md) | Coma = separador de miles, punto = decimal, en todo el sistema | [[02-pesaje]] |
| [0002](0002-modo-manual-bypass-validaciones.md) | Modo Manual en Peso y Análisis | [[02-pesaje]], [[03-analisis-calidad]] |
| [0003](0003-peso-liquido-bidireccional-analisis.md) | Peso Líquido: sincronización bidireccional Peso ↔ Análisis | [[02-pesaje]], [[03-analisis-calidad]], [[04-boleta-impresion]] |
| [0004](0004-pesohectolitrico-no-es-peso-liquido.md) | `PesoHectolitrico` dejó de alimentar el campo "Peso Líquido" | [[03-analisis-calidad]] |
| [0005](0005-crud-origen-destino-separado.md) | CRUD de Procedencia/Destino separado (misma tabla) | [[02-pesaje]] |
| [0006](0006-pesos-redondeados-a-entero.md) | Pesos sin decimales en todo el sistema | [[02-pesaje]], [[04-boleta-impresion]] |
| [0007](0007-seed-passwords-fuera-de-git.md) | Contraseñas semilla fuera del código fuente | [[07-proveedores-datos]] |
| [0008](0008-fallo-transitorio-inno-setup.md) | Fallo transitorio de Inno Setup al publicar | [[08-autoactualizacion]] |
| [0009](0009-nuget-config-local-self-contained.md) | `NuGet.Config` local para permitir publish self-contained | [[09-instalador]] |
| [0010](0010-autoactualizacion-via-github-releases.md) | Autoactualización vía GitHub Releases (no casillero de red) | [[08-autoactualizacion]], [[09-instalador]] |
