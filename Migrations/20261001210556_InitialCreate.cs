using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SER_Balanza_Interno.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analisis",
                columns: table => new
                {
                    DocEntry = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocNum = table.Column<int>(type: "INTEGER", nullable: true),
                    PesoId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProductoId = table.Column<int>(type: "INTEGER", nullable: true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: true),
                    Cancelado = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    UsuarioIdReg = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaDoc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaReg = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaAct = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UsuarioIdAct = table.Column<int>(type: "INTEGER", nullable: true),
                    PHumedad = table.Column<double>(type: "REAL", nullable: true),
                    PImpureza = table.Column<double>(type: "REAL", nullable: true),
                    PPartido = table.Column<double>(type: "REAL", nullable: true),
                    PDanado = table.Column<double>(type: "REAL", nullable: true),
                    POtroColor = table.Column<double>(type: "REAL", nullable: true),
                    PDCalor = table.Column<double>(type: "REAL", nullable: true),
                    PEnfermo = table.Column<double>(type: "REAL", nullable: true),
                    PVerde = table.Column<double>(type: "REAL", nullable: true),
                    Humedad = table.Column<double>(type: "REAL", nullable: true),
                    Impureza = table.Column<double>(type: "REAL", nullable: true),
                    Partido = table.Column<double>(type: "REAL", nullable: true),
                    Danado = table.Column<double>(type: "REAL", nullable: true),
                    OtroColor = table.Column<double>(type: "REAL", nullable: true),
                    DCalor = table.Column<double>(type: "REAL", nullable: true),
                    Enfermo = table.Column<double>(type: "REAL", nullable: true),
                    Verde = table.Column<double>(type: "REAL", nullable: true),
                    Desc_Humedad = table.Column<double>(type: "REAL", nullable: true),
                    Desc_Impureza = table.Column<double>(type: "REAL", nullable: true),
                    Desc_Partido = table.Column<double>(type: "REAL", nullable: true),
                    Desc_Danado = table.Column<double>(type: "REAL", nullable: true),
                    Desc_OtroColor = table.Column<double>(type: "REAL", nullable: true),
                    Desc_DCalor = table.Column<double>(type: "REAL", nullable: true),
                    Desc_Enfermo = table.Column<double>(type: "REAL", nullable: true),
                    Desc_Verde = table.Column<double>(type: "REAL", nullable: true),
                    DescWeightHumedad = table.Column<double>(type: "REAL", nullable: true),
                    DescWeightImpureza = table.Column<double>(type: "REAL", nullable: true),
                    DescWeightPartido = table.Column<double>(type: "REAL", nullable: true),
                    DescWeightDanado = table.Column<double>(type: "REAL", nullable: true),
                    DescWeightOtroColor = table.Column<double>(type: "REAL", nullable: true),
                    DescWeightDCalor = table.Column<double>(type: "REAL", nullable: true),
                    DescWeightEnfermo = table.Column<double>(type: "REAL", nullable: true),
                    DescWeightVerde = table.Column<double>(type: "REAL", nullable: true),
                    TotalDescPorcent = table.Column<double>(type: "REAL", nullable: true),
                    TotalDescPeso = table.Column<double>(type: "REAL", nullable: true),
                    Unidad = table.Column<string>(type: "TEXT", nullable: false),
                    PesoHectolitrico = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analisis", x => x.DocEntry);
                });

            migrationBuilder.CreateTable(
                name: "balanza",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    tipo_conexion = table.Column<int>(type: "INTEGER", nullable: false),
                    ip = table.Column<string>(type: "TEXT", nullable: false),
                    puerto = table.Column<int>(type: "INTEGER", nullable: true),
                    puerto_serial = table.Column<string>(type: "TEXT", nullable: false),
                    baudRate = table.Column<int>(type: "INTEGER", nullable: true),
                    dataBits = table.Column<int>(type: "INTEGER", nullable: true),
                    parity = table.Column<int>(type: "INTEGER", nullable: true),
                    stopBits = table.Column<int>(type: "INTEGER", nullable: true),
                    nro_balanza = table.Column<int>(type: "INTEGER", nullable: true),
                    activo = table.Column<bool>(type: "INTEGER", nullable: true),
                    id_indicador = table.Column<int>(type: "INTEGER", nullable: false),
                    semaforo = table.Column<bool>(type: "INTEGER", nullable: true),
                    rfid = table.Column<bool>(type: "INTEGER", nullable: true),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    Direccion = table.Column<string>(type: "TEXT", nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_balanza", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "campania",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    sigla = table.Column<string>(type: "TEXT", nullable: false),
                    activo = table.Column<bool>(type: "INTEGER", nullable: false),
                    valido_desde = table.Column<DateTime>(type: "TEXT", nullable: false),
                    valido_hasta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    id_usuario = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campania", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chofer",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ci = table.Column<string>(type: "TEXT", nullable: false),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    direccion = table.Column<string>(type: "TEXT", nullable: false),
                    telefono = table.Column<string>(type: "TEXT", nullable: false),
                    activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chofer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "compania",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    razon_social = table.Column<string>(type: "TEXT", nullable: false),
                    nit = table.Column<string>(type: "TEXT", nullable: false),
                    direccion1 = table.Column<string>(type: "TEXT", nullable: false),
                    direccion2 = table.Column<string>(type: "TEXT", nullable: false),
                    telefono = table.Column<string>(type: "TEXT", nullable: false),
                    logo = table.Column<byte[]>(type: "BLOB", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compania", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "documento",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    activo = table.Column<bool>(type: "INTEGER", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    id_usuario = table.Column<int>(type: "INTEGER", nullable: false),
                    modo_transaccion = table.Column<string>(type: "TEXT", nullable: false),
                    Id_balanza = table.Column<int>(type: "INTEGER", nullable: true),
                    IdProducto = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documento", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "factor",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Aud_Anulado = table.Column<byte>(type: "INTEGER", nullable: true),
                    Aud_FechaReg = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Aud_UsuarioReg = table.Column<string>(type: "TEXT", nullable: false),
                    Aud_IpReg = table.Column<string>(type: "TEXT", nullable: false),
                    Aud_FechaMod = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Aud_UsuarioMod = table.Column<string>(type: "TEXT", nullable: false),
                    Aud_IpMod = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_factor", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Hacienda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    Direccion = table.Column<string>(type: "TEXT", nullable: false),
                    Abreviatura = table.Column<string>(type: "TEXT", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hacienda", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "indicador",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    comando = table.Column<string>(type: "TEXT", nullable: false),
                    cant_caracter = table.Column<int>(type: "INTEGER", nullable: true),
                    caracter_ini = table.Column<string>(type: "TEXT", nullable: false),
                    longitud_dato = table.Column<int>(type: "INTEGER", nullable: true),
                    posicion = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_indicador", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_transferencia_pesaje",
                columns: table => new
                {
                    IdLog = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FechaHora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PesoInKey = table.Column<int>(type: "INTEGER", nullable: false),
                    Placa = table.Column<string>(type: "TEXT", nullable: false),
                    Producto = table.Column<string>(type: "TEXT", nullable: false),
                    Peso = table.Column<decimal>(type: "TEXT", nullable: true),
                    FechaIngreso = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Cliente = table.Column<string>(type: "TEXT", nullable: false),
                    Proveedor = table.Column<string>(type: "TEXT", nullable: false),
                    Chofer = table.Column<string>(type: "TEXT", nullable: false),
                    Remito = table.Column<string>(type: "TEXT", nullable: false),
                    DocumentoOrigen = table.Column<string>(type: "TEXT", nullable: false),
                    HaciendaOrigen = table.Column<string>(type: "TEXT", nullable: false),
                    BalanzaOrigen = table.Column<string>(type: "TEXT", nullable: false),
                    BalanzaDestino = table.Column<string>(type: "TEXT", nullable: false),
                    DocumentoDestino = table.Column<string>(type: "TEXT", nullable: false),
                    HaciendaDestino = table.Column<string>(type: "TEXT", nullable: false),
                    Usuario = table.Column<string>(type: "TEXT", nullable: false),
                    Equipo = table.Column<string>(type: "TEXT", nullable: false),
                    Resultado = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_transferencia_pesaje", x => x.IdLog);
                });

            migrationBuilder.CreateTable(
                name: "Marca",
                columns: table => new
                {
                    id_marca = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SD = table.Column<string>(type: "TEXT", nullable: false),
                    S = table.Column<string>(type: "TEXT", nullable: false),
                    F = table.Column<string>(type: "TEXT", nullable: false),
                    E = table.Column<string>(type: "TEXT", nullable: false),
                    FA = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marca", x => x.id_marca);
                });

            migrationBuilder.CreateTable(
                name: "numeracion",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    numeracion_inicial = table.Column<int>(type: "INTEGER", nullable: false),
                    prefijo = table.Column<string>(type: "TEXT", nullable: false),
                    valor = table.Column<int>(type: "INTEGER", nullable: false),
                    id_documento = table.Column<int>(type: "INTEGER", nullable: true),
                    activo = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_numeracion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "origen_destino",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    es_destino = table.Column<bool>(type: "INTEGER", nullable: false),
                    activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_origen_destino", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Permiso",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    es_grupo = table.Column<bool>(type: "INTEGER", nullable: true),
                    grupo = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permiso", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permiso_rol",
                columns: table => new
                {
                    id_permiso = table.Column<int>(type: "INTEGER", nullable: false),
                    id_rol = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permiso_rol", x => new { x.id_permiso, x.id_rol });
                });

            migrationBuilder.CreateTable(
                name: "peso",
                columns: table => new
                {
                    NroConsec = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NroPesaje = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaSalida = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Bruto = table.Column<double>(type: "REAL", nullable: false),
                    Tara = table.Column<double>(type: "REAL", nullable: false),
                    Neto = table.Column<double>(type: "REAL", nullable: false),
                    UnidadPrimaria = table.Column<string>(type: "TEXT", nullable: false),
                    unidadSecundaria = table.Column<string>(type: "TEXT", nullable: false),
                    PesoUnidSec = table.Column<double>(type: "REAL", nullable: true),
                    Id_usuarioIng = table.Column<int>(type: "INTEGER", nullable: false),
                    id_Proveedor = table.Column<int>(type: "INTEGER", nullable: true),
                    id_Chofer = table.Column<int>(type: "INTEGER", nullable: true),
                    id_Vehiculo = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_producto = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_UsuarioSal = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacion = table.Column<string>(type: "TEXT", nullable: false),
                    Nulo = table.Column<bool>(type: "INTEGER", nullable: false),
                    Id_cliente = table.Column<int>(type: "INTEGER", nullable: true),
                    Importe = table.Column<decimal>(type: "TEXT", nullable: true),
                    peso_manual = table.Column<bool>(type: "INTEGER", nullable: false),
                    NroTicket = table.Column<string>(type: "TEXT", nullable: false),
                    PesoLiquido = table.Column<double>(type: "REAL", nullable: true),
                    TipoTara = table.Column<int>(type: "INTEGER", nullable: true),
                    PesoTara = table.Column<double>(type: "REAL", nullable: true),
                    id_origen = table.Column<int>(type: "INTEGER", nullable: true),
                    id_destino = table.Column<int>(type: "INTEGER", nullable: true),
                    Credito = table.Column<bool>(type: "INTEGER", nullable: true),
                    Lote = table.Column<string>(type: "TEXT", nullable: false),
                    TotalDesc = table.Column<double>(type: "REAL", nullable: true),
                    ModeService = table.Column<bool>(type: "INTEGER", nullable: true),
                    produccion = table.Column<bool>(type: "INTEGER", nullable: true),
                    id_campania = table.Column<int>(type: "INTEGER", nullable: true),
                    id_documento = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_balanza = table.Column<int>(type: "INTEGER", nullable: true),
                    PesoInKey = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_Hacienda = table.Column<int>(type: "INTEGER", nullable: true),
                    cultivo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peso", x => x.NroConsec);
                });

            migrationBuilder.CreateTable(
                name: "pesoin",
                columns: table => new
                {
                    PesoInKey = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NroPesaje = table.Column<int>(type: "INTEGER", nullable: false),
                    id_Vehiculo = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_cliente = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_producto = table.Column<int>(type: "INTEGER", nullable: true),
                    pesoin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    peso = table.Column<double>(type: "REAL", nullable: false),
                    unidad_primaria = table.Column<string>(type: "TEXT", nullable: false),
                    unidad_secundaria = table.Column<string>(type: "TEXT", nullable: false),
                    PesoUnidSec = table.Column<double>(type: "REAL", nullable: true),
                    notas = table.Column<string>(type: "TEXT", nullable: false),
                    id_Chofer = table.Column<int>(type: "INTEGER", nullable: true),
                    id_Proveedor = table.Column<int>(type: "INTEGER", nullable: true),
                    peso_manual = table.Column<bool>(type: "INTEGER", nullable: false),
                    id_usuario = table.Column<int>(type: "INTEGER", nullable: false),
                    Importe = table.Column<decimal>(type: "TEXT", nullable: true),
                    NroTicket = table.Column<string>(type: "TEXT", nullable: false),
                    id_origen = table.Column<int>(type: "INTEGER", nullable: true),
                    id_destino = table.Column<int>(type: "INTEGER", nullable: true),
                    Credito = table.Column<bool>(type: "INTEGER", nullable: true),
                    Lote = table.Column<string>(type: "TEXT", nullable: false),
                    ModeService = table.Column<bool>(type: "INTEGER", nullable: true),
                    produccion = table.Column<bool>(type: "INTEGER", nullable: true),
                    id_campania = table.Column<int>(type: "INTEGER", nullable: true),
                    id_documento = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_balanza = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_Hacienda = table.Column<int>(type: "INTEGER", nullable: true),
                    cultivo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pesoin", x => x.PesoInKey);
                });

            migrationBuilder.CreateTable(
                name: "prod_fac_rank",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    idProducto = table.Column<int>(type: "INTEGER", nullable: false),
                    idfactor = table.Column<int>(type: "INTEGER", nullable: false),
                    rank_from = table.Column<double>(type: "REAL", nullable: true),
                    rank_to = table.Column<double>(type: "REAL", nullable: true),
                    discount = table.Column<double>(type: "REAL", nullable: true),
                    Aud_Anulado = table.Column<byte>(type: "INTEGER", nullable: true),
                    Aud_FechaReg = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Aud_UsuarioReg = table.Column<string>(type: "TEXT", nullable: false),
                    Aud_IpReg = table.Column<string>(type: "TEXT", nullable: false),
                    Aud_FechaMod = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Aud_UsuarioMod = table.Column<string>(type: "TEXT", nullable: false),
                    Aud_IpMod = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prod_fac_rank", x => new { x.id, x.idProducto, x.idfactor });
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    activo = table.Column<bool>(type: "INTEGER", nullable: false),
                    HabilitarParametro = table.Column<bool>(type: "INTEGER", nullable: false),
                    Humedad = table.Column<double>(type: "REAL", nullable: true),
                    FDHumedad = table.Column<double>(type: "REAL", nullable: true),
                    Impureza = table.Column<double>(type: "REAL", nullable: true),
                    FDImpureza = table.Column<double>(type: "REAL", nullable: true),
                    Partido = table.Column<double>(type: "REAL", nullable: true),
                    FDPartido = table.Column<double>(type: "REAL", nullable: true),
                    Danado = table.Column<double>(type: "REAL", nullable: true),
                    FDDanado = table.Column<double>(type: "REAL", nullable: true),
                    OtroColor = table.Column<double>(type: "REAL", nullable: true),
                    FDOtroColor = table.Column<double>(type: "REAL", nullable: true),
                    DanadoPorCalor = table.Column<double>(type: "REAL", nullable: true),
                    FDDanadoPorCalor = table.Column<double>(type: "REAL", nullable: true),
                    Enfermo = table.Column<double>(type: "REAL", nullable: true),
                    FDEnfermo = table.Column<double>(type: "REAL", nullable: true),
                    Verde = table.Column<double>(type: "REAL", nullable: true),
                    FDVerde = table.Column<double>(type: "REAL", nullable: true),
                    UnidadOpcional = table.Column<string>(type: "TEXT", nullable: false),
                    FactorUO = table.Column<double>(type: "REAL", nullable: true),
                    HabilitadoUO = table.Column<bool>(type: "INTEGER", nullable: true),
                    id_producto_padre = table.Column<int>(type: "INTEGER", nullable: true),
                    factor_correccion = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    activo = table.Column<bool>(type: "INTEGER", nullable: false),
                    eliminado = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "socio_negocio",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    direccion = table.Column<string>(type: "TEXT", nullable: false),
                    telefono = table.Column<string>(type: "TEXT", nullable: false),
                    contacto = table.Column<string>(type: "TEXT", nullable: false),
                    es_cliente = table.Column<bool>(type: "INTEGER", nullable: false),
                    es_proveedor = table.Column<bool>(type: "INTEGER", nullable: false),
                    activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socio_negocio", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    usuario = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Habilitado = table.Column<bool>(type: "INTEGER", nullable: false),
                    Eliminado = table.Column<bool>(type: "INTEGER", nullable: false),
                    id_rol = table.Column<int>(type: "INTEGER", nullable: true),
                    id_balanza = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Placa = table.Column<string>(type: "TEXT", nullable: false),
                    Marca = table.Column<string>(type: "TEXT", nullable: false),
                    Modelo = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    color = table.Column<string>(type: "TEXT", nullable: false),
                    Propietario = table.Column<string>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tara = table.Column<double>(type: "REAL", nullable: true),
                    TipoTara = table.Column<int>(type: "INTEGER", nullable: true),
                    TaraAdquirida = table.Column<DateTime>(type: "TEXT", nullable: true),
                    taraExpira = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analisis");

            migrationBuilder.DropTable(
                name: "balanza");

            migrationBuilder.DropTable(
                name: "campania");

            migrationBuilder.DropTable(
                name: "chofer");

            migrationBuilder.DropTable(
                name: "compania");

            migrationBuilder.DropTable(
                name: "documento");

            migrationBuilder.DropTable(
                name: "factor");

            migrationBuilder.DropTable(
                name: "Hacienda");

            migrationBuilder.DropTable(
                name: "indicador");

            migrationBuilder.DropTable(
                name: "log_transferencia_pesaje");

            migrationBuilder.DropTable(
                name: "Marca");

            migrationBuilder.DropTable(
                name: "numeracion");

            migrationBuilder.DropTable(
                name: "origen_destino");

            migrationBuilder.DropTable(
                name: "Permiso");

            migrationBuilder.DropTable(
                name: "permiso_rol");

            migrationBuilder.DropTable(
                name: "peso");

            migrationBuilder.DropTable(
                name: "pesoin");

            migrationBuilder.DropTable(
                name: "prod_fac_rank");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "socio_negocio");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Vehiculo");
        }
    }
}
