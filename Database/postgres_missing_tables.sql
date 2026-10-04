-- Script generado automaticamente a partir del esquema de SQL Server (servisur @ 196.19.9.18)
-- Destino: PostgreSQL servisur @ 196.19.9.49 (AppConnection)
-- Crea unicamente las tablas (estructura, sin datos) que no existan aun.

CREATE TABLE IF NOT EXISTS "Analisis" (
    "DocEntry" integer NOT NULL,
    "DocNum" integer,
    "PesoId" integer,
    "ProductoId" integer,
    "ClienteId" integer,
    "Cancelado" char(1),
    "Estado" char(1),
    "UsuarioIdReg" integer NOT NULL,
    "FechaDoc" timestamp NOT NULL,
    "FechaReg" timestamp NOT NULL,
    "FechaAct" timestamp,
    "UsuarioIdAct" integer,
    "PHumedad" double precision,
    "PImpureza" double precision,
    "PPartido" double precision,
    "PDanado" double precision,
    "POtroColor" double precision,
    "PDCalor" double precision,
    "PEnfermo" double precision,
    "PVerde" double precision,
    "Humedad" double precision,
    "Impureza" double precision,
    "Partido" double precision,
    "Danado" double precision,
    "OtroColor" double precision,
    "DCalor" double precision,
    "Enfermo" double precision,
    "Verde" double precision,
    "Desc_Humedad" double precision,
    "Desc_Impureza" double precision,
    "Desc_Partido" double precision,
    "Desc_Danado" double precision,
    "Desc_OtroColor" double precision,
    "Desc_DCalor" double precision,
    "Desc_Enfermo" double precision,
    "Desc_Verde" double precision,
    "DescWeightHumedad" double precision,
    "DescWeightImpureza" double precision,
    "DescWeightPartido" double precision,
    "DescWeightDanado" double precision,
    "DescWeightOtroColor" double precision,
    "DescWeightDCalor" double precision,
    "DescWeightEnfermo" double precision,
    "DescWeightVerde" double precision,
    "TotalDescPorcent" double precision,
    "TotalDescPeso" double precision,
    "Unidad" varchar(50),
    "PesoHectolitrico" double precision,
    CONSTRAINT "PK_Analisis" PRIMARY KEY ("DocEntry")
);

CREATE TABLE IF NOT EXISTS "balanza" (
    "id" integer NOT NULL,
    "nombre" varchar(100) NOT NULL,
    "tipo_conexion" integer NOT NULL,
    "ip" varchar(50),
    "puerto" integer,
    "puerto_serial" varchar(50),
    "baudRate" integer,
    "dataBits" integer,
    "parity" integer,
    "stopBits" integer,
    "nro_balanza" integer,
    "activo" boolean,
    "id_indicador" integer NOT NULL,
    "semaforo" boolean,
    "rfid" boolean,
    "Descripcion" varchar(50),
    "Direccion" varchar(50),
    "Telefono" varchar(30),
    "email" varchar(20),
    CONSTRAINT "PK_balanza" PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "campania" (
    "id" integer NOT NULL,
    "nombre" varchar(100) NOT NULL,
    "sigla" varchar(100) NOT NULL,
    "activo" boolean NOT NULL,
    "valido_desde" date NOT NULL,
    "valido_hasta" date NOT NULL,
    "fecha_creacion" timestamp NOT NULL,
    "id_usuario" integer NOT NULL,
    CONSTRAINT "PK_campania" PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "chofer" (
    "id" integer NOT NULL,
    "ci" varchar(50),
    "nombre" varchar(50) NOT NULL,
    "direccion" varchar(50),
    "telefono" varchar(50),
    "activo" boolean NOT NULL,
    CONSTRAINT "PK_chofer" PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "compania" (
    "id" integer NOT NULL,
    "nombre" varchar(100) NOT NULL,
    "razon_social" varchar(100),
    "nit" varchar(50),
    "direccion1" varchar(100),
    "direccion2" varchar(100),
    "telefono" varchar(100),
    "logo" bytea,
    "email" varchar(50),
    "activo" boolean NOT NULL,
    CONSTRAINT "PK_compania" PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "documento" (
    "id" integer NOT NULL,
    "nombre" varchar(100) NOT NULL,
    "activo" boolean NOT NULL,
    "fecha_creacion" timestamp NOT NULL,
    "id_usuario" integer NOT NULL,
    "modo_transaccion" varchar(1) NOT NULL,
    "Id_balanza" integer,
    "IdProducto" integer,
    CONSTRAINT "PK_documento" PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "factor" (
    "id" integer NOT NULL,
    "nombre" varchar(100) NOT NULL,
    "Aud_Anulado" smallint,
    "Aud_FechaReg" timestamp,
    "Aud_UsuarioReg" varchar(50),
    "Aud_IpReg" varchar(15),
    "Aud_FechaMod" timestamp,
    "Aud_UsuarioMod" varchar(50),
    "Aud_IpMod" varchar(15),
    CONSTRAINT "PK_factor" PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "Hacienda" (
    "Id" integer NOT NULL,
    "nombre" varchar(100),
    "Descripcion" varchar(100),
    "Direccion" varchar(200),
    "Abreviatura" varchar(20),
    "FechaCreacion" timestamp,
    "Activo" boolean,
    CONSTRAINT "PK_Hacienda" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "indicador" (
    "id" integer NOT NULL,
    "nombre" varchar(100),
    "comando" varchar(100),
    "cant_caracter" integer,
    "caracter_ini" varchar(10),
    "longitud_dato" integer,
    "posicion" integer,
    CONSTRAINT "PK_indicador" PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "log_transferencia_pesaje" (
    "IdLog" integer GENERATED ALWAYS AS IDENTITY NOT NULL,
    "FechaHora" timestamp NOT NULL,
    "PesoInKey" integer NOT NULL,
    "Placa" varchar(20),
    "Producto" varchar(100),
    "Peso" numeric(10,2),
    "FechaIngreso" date,
    "Cliente" varchar(150),
    "Proveedor" varchar(150),
    "Chofer" varchar(150),
    "Remito" varchar(50),
    "DocumentoOrigen" varchar(150),
    "HaciendaOrigen" varchar(150),
    "BalanzaOrigen" varchar(50) NOT NULL,
    "BalanzaDestino" varchar(50) NOT NULL,
    "DocumentoDestino" varchar(150),
    "HaciendaDestino" varchar(150),
    "Usuario" varchar(50) NOT NULL,
    "Equipo" varchar(100),
    "Resultado" varchar(20) NOT NULL,
    CONSTRAINT "PK_log_transferencia_pesaje" PRIMARY KEY ("IdLog")
);

CREATE TABLE IF NOT EXISTS "Marca" (
    "id_marca" integer GENERATED ALWAYS AS IDENTITY NOT NULL,
    "SD" text,
    "S" text,
    "F" text,
    "E" text,
    "FA" text,
    CONSTRAINT "PK_Marca" PRIMARY KEY ("id_marca")
);

CREATE TABLE IF NOT EXISTS "numeracion" (
    "id" integer NOT NULL,
    "nombre" varchar(100) NOT NULL,
    "numeracion_inicial" integer NOT NULL,
    "prefijo" varchar(100),
    "valor" integer NOT NULL,
    "id_documento" integer,
    "activo" boolean,
    CONSTRAINT "PK_numeracion" PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "origen_destino" (
    "id" integer NOT NULL,
    "nombre" varchar(50) NOT NULL,
    "es_destino" boolean NOT NULL,
    "activo" boolean NOT NULL,
    CONSTRAINT "PK_origen_destino" PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "Permiso" (
    "id" integer NOT NULL,
    "nombre" varchar(50) NOT NULL,
    "es_grupo" boolean,
    "grupo" integer,
    CONSTRAINT "PK_Permiso" PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "permiso_rol" (
    "id_permiso" integer NOT NULL,
    "id_rol" integer NOT NULL,
    CONSTRAINT "PK_permiso_rol" PRIMARY KEY ("id_permiso", "id_rol")
);

CREATE TABLE IF NOT EXISTS "peso" (
    "NroConsec" integer NOT NULL,
    "NroPesaje" integer NOT NULL,
    "FechaIngreso" timestamp NOT NULL,
    "FechaSalida" timestamp NOT NULL,
    "Bruto" double precision NOT NULL,
    "Tara" double precision NOT NULL,
    "Neto" double precision NOT NULL,
    "UnidadPrimaria" varchar(50) NOT NULL,
    "unidadSecundaria" varchar(50),
    "PesoUnidSec" double precision,
    "Id_usuarioIng" integer NOT NULL,
    "id_Proveedor" integer,
    "id_Chofer" integer,
    "id_Vehiculo" integer,
    "Id_producto" integer,
    "Id_UsuarioSal" integer NOT NULL,
    "Observacion" varchar(255),
    "Nulo" boolean NOT NULL,
    "Id_cliente" integer,
    "Importe" numeric(18,4),
    "peso_manual" boolean NOT NULL,
    "NroTicket" varchar(50),
    "PesoLiquido" double precision,
    "TipoTara" integer,
    "PesoTara" double precision,
    "id_origen" integer,
    "id_destino" integer,
    "Credito" boolean,
    "Lote" varchar(50),
    "TotalDesc" double precision,
    "ModeService" boolean,
    "produccion" boolean,
    "id_campania" integer,
    "id_documento" integer NOT NULL,
    "Id_balanza" integer,
    "PesoInKey" integer,
    "Id_Hacienda" integer,
    "cultivo" varchar(50),
    CONSTRAINT "PK_peso" PRIMARY KEY ("NroConsec")
);

CREATE TABLE IF NOT EXISTS "pesoin" (
    "PesoInKey" integer GENERATED ALWAYS AS IDENTITY NOT NULL,
    "NroPesaje" integer NOT NULL,
    "id_Vehiculo" integer,
    "Id_cliente" integer,
    "Id_producto" integer,
    "pesoin" timestamp NOT NULL,
    "peso" double precision NOT NULL,
    "unidad_primaria" varchar(50) NOT NULL,
    "unidad_secundaria" varchar(50),
    "PesoUnidSec" double precision,
    "notas" varchar(255),
    "id_Chofer" integer,
    "id_Proveedor" integer,
    "peso_manual" boolean NOT NULL,
    "id_usuario" integer NOT NULL,
    "Importe" numeric(18,4),
    "NroTicket" varchar(50),
    "id_origen" integer,
    "id_destino" integer,
    "Credito" boolean,
    "Lote" varchar(50),
    "ModeService" boolean,
    "produccion" boolean,
    "id_campania" integer,
    "id_documento" integer,
    "Id_balanza" integer,
    "Id_Hacienda" integer,
    "cultivo" varchar(50),
    CONSTRAINT "PK_pesoin" PRIMARY KEY ("PesoInKey")
);

CREATE TABLE IF NOT EXISTS "prod_fac_rank" (
    "id" integer NOT NULL,
    "idProducto" integer NOT NULL,
    "idfactor" integer NOT NULL,
    "rank_from" double precision,
    "rank_to" double precision,
    "discount" double precision,
    "Aud_Anulado" smallint,
    "Aud_FechaReg" timestamp,
    "Aud_UsuarioReg" varchar(50),
    "Aud_IpReg" varchar(15),
    "Aud_FechaMod" timestamp,
    "Aud_UsuarioMod" varchar(50),
    "Aud_IpMod" varchar(15),
    CONSTRAINT "PK_prod_fac_rank" PRIMARY KEY ("id", "idProducto", "idfactor")
);

CREATE TABLE IF NOT EXISTS "Producto" (
    "Id" integer NOT NULL,
    "nombre" varchar(50) NOT NULL,
    "activo" boolean NOT NULL,
    "HabilitarParametro" boolean NOT NULL,
    "Humedad" double precision,
    "FDHumedad" double precision,
    "Impureza" double precision,
    "FDImpureza" double precision,
    "Partido" double precision,
    "FDPartido" double precision,
    "Danado" double precision,
    "FDDanado" double precision,
    "OtroColor" double precision,
    "FDOtroColor" double precision,
    "DanadoPorCalor" double precision,
    "FDDanadoPorCalor" double precision,
    "Enfermo" double precision,
    "FDEnfermo" double precision,
    "Verde" double precision,
    "FDVerde" double precision,
    "UnidadOpcional" varchar(50),
    "FactorUO" double precision,
    "HabilitadoUO" boolean,
    "id_producto_padre" integer,
    "factor_correccion" double precision,
    CONSTRAINT "PK_Producto" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "rol" (
    "Id" integer NOT NULL,
    "Nombre" varchar(50) NOT NULL,
    "activo" boolean NOT NULL,
    "eliminado" boolean NOT NULL,
    CONSTRAINT "PK_rol" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "socio_negocio" (
    "id" integer NOT NULL,
    "nombre" varchar(50) NOT NULL,
    "direccion" varchar(50),
    "telefono" varchar(50),
    "contacto" varchar(50),
    "es_cliente" boolean NOT NULL,
    "es_proveedor" boolean NOT NULL,
    "activo" boolean NOT NULL,
    CONSTRAINT "PK_socio_negocio" PRIMARY KEY ("id")
);

CREATE TABLE IF NOT EXISTS "Usuario" (
    "Id" integer GENERATED ALWAYS AS IDENTITY NOT NULL,
    "Nombre" varchar(50) NOT NULL,
    "usuario" varchar(50) NOT NULL,
    "PasswordHash" varchar(250) NOT NULL,
    "Habilitado" boolean NOT NULL,
    "Eliminado" boolean NOT NULL,
    "id_rol" integer,
    "id_balanza" integer,
    "ci" varchar(20),
    CONSTRAINT "PK_Usuario" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "Vehiculo" (
    "Id" integer NOT NULL,
    "Placa" varchar(50) NOT NULL,
    "Marca" varchar(50),
    "Modelo" varchar(50),
    "Tipo" varchar(50),
    "color" varchar(50),
    "Propietario" varchar(50),
    "Activo" boolean NOT NULL,
    "Tara" double precision,
    "TipoTara" integer,
    "TaraAdquirida" date,
    "taraExpira" date,
    CONSTRAINT "PK_Vehiculo" PRIMARY KEY ("Id")
);

