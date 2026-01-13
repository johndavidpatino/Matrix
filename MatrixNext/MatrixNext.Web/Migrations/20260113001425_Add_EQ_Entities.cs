using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatrixNext.Web.Migrations
{
    /// <inheritdoc />
    public partial class Add_EQ_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "CORE_Tareas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NoEmpiezaAntesDe = table.Column<long>(type: "bigint", nullable: true),
                    NoTerminaAntesDe = table.Column<long>(type: "bigint", nullable: true),
                    TiempoPromedioDias = table.Column<short>(type: "smallint", nullable: true),
                    RequiereEstimacion = table.Column<bool>(type: "bit", nullable: true),
                    RolEstima = table.Column<long>(type: "bigint", nullable: true),
                    UnidadEjecuta = table.Column<long>(type: "bigint", nullable: true),
                    UnidadRecibe = table.Column<long>(type: "bigint", nullable: true),
                    RolEjecuta = table.Column<long>(type: "bigint", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: true),
                    Orden = table.Column<long>(type: "bigint", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORE_Tareas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "eq_cost_insumos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NSE = table.Column<int>(type: "int", nullable: false),
                    Reclutamiento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Obsequio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Productividad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dias = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Supervisores = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Logistica = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransporteEncuestador = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransporteSupervisor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorEnvio1erKilo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorKiloAdicional = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SeguroPct = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorMinDeclarar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_cost_insumos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "eq_locaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TarifaBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaConGross = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiasBase = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_locaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "eq_param_precio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoMetodologia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PenetracionRango = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DuracionMin = table.Column<int>(type: "int", nullable: false),
                    ValorPerfil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorCoord = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VigentDesde = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VigentHasta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_param_precio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "eq_param_script_proc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuracionMin = table.Column<int>(type: "int", nullable: false),
                    HorasScript = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HorasProc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HorasHarmoni = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HorasGraficacion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_param_script_proc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "eq_quote_header",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropuestaNombre = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GrupoObjetivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cliente = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaAprobacionEstimada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCampo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProbabilidadAprobacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MetodologiaSL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RecordDetail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoriaProducto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValorProveedorExterno = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorProveedorInternacional = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorGMU = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notas = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_quote_header", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "eq_rate_estadistica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Servicio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HorasEstimadas = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrecioRef2024 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FactorEscala = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeadTime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ejemplos = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FactorEconomiaEscala = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_rate_estadistica", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "eq_valor_hora_ops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nivel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Alternativa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BaseCostRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OverheadRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LoadedCostRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BillingRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VigentDesde = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VigentHasta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_valor_hora_ops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IdGerenteProyectos = table.Column<long>(type: "bigint", nullable: true),
                    IdUnidad = table.Column<long>(type: "bigint", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    JobBook = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlows",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTrabajo = table.Column<long>(type: "bigint", nullable: false),
                    IdTarea = table.Column<long>(type: "bigint", nullable: false),
                    IdTipoHilo = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Creada"),
                    Prioridad = table.Column<int>(type: "int", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "eq_cost_result",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuoteHeaderId = table.Column<int>(type: "int", nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CostoCampo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoCalidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Viaticos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Incentivos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Insumos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Logistica = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StaffOps = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estadistica = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Scripting = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCleaning = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TopLines = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Procesamiento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Harmoni = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Graficacion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompraProducto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tablets = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoDirectoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoConIncentivos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DirectCostOps = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PB_RMF = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProfTime = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PctOP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AOTUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AOTTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaCalculo = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_cost_result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_eq_cost_result_eq_quote_header_QuoteHeaderId",
                        column: x => x.QuoteHeaderId,
                        principalTable: "eq_quote_header",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "eq_methodology",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuoteHeaderId = table.Column<int>(type: "int", nullable: false),
                    MetodologiaRecoleccion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tecnica1Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tecnica1Flag = table.Column<bool>(type: "bit", nullable: false),
                    Tecnica2Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tecnica2Flag = table.Column<bool>(type: "bit", nullable: false),
                    Tecnica3Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tecnica3Flag = table.Column<bool>(type: "bit", nullable: false),
                    BaseDatos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IncidenciaLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IncidenciaValor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MetodologiasMix = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_methodology", x => x.Id);
                    table.ForeignKey(
                        name: "FK_eq_methodology_eq_quote_header_QuoteHeaderId",
                        column: x => x.QuoteHeaderId,
                        principalTable: "eq_quote_header",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "eq_mystery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuoteHeaderId = table.Column<int>(type: "int", nullable: false),
                    TipoVisita = table.Column<int>(type: "int", nullable: false),
                    Complejidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumOlas = table.Column<int>(type: "int", nullable: false),
                    Desplazamientos = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tanques = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Alertas = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EdicionVideo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AlquilerEquipos = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompraDispositivos = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Seguimiento = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_mystery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_eq_mystery_eq_quote_header_QuoteHeaderId",
                        column: x => x.QuoteHeaderId,
                        principalTable: "eq_quote_header",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "eq_questionnaire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuoteHeaderId = table.Column<int>(type: "int", nullable: false),
                    DuracionMinutos = table.Column<int>(type: "int", nullable: false),
                    PenetracionLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PenetracionValor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PreguntasAbiertas = table.Column<int>(type: "int", nullable: false),
                    PreguntasAbiertasMultiples = table.Column<int>(type: "int", nullable: false),
                    OtrosProcesos = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TopLine = table.Column<bool>(type: "bit", nullable: false),
                    DataCleaning = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ASCII = table.Column<bool>(type: "bit", nullable: false),
                    ScriptReclutamiento = table.Column<bool>(type: "bit", nullable: false),
                    Scripting = table.Column<bool>(type: "bit", nullable: false),
                    TipoScript = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Codificacion = table.Column<bool>(type: "bit", nullable: false),
                    Procesamiento = table.Column<bool>(type: "bit", nullable: false),
                    NumProcesamientos = table.Column<int>(type: "int", nullable: false),
                    ProcesoEstadistico = table.Column<bool>(type: "bit", nullable: false),
                    ClasePrueba = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Refrigeracion = table.Column<bool>(type: "bit", nullable: false),
                    CompraProducto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EtiquetadoTipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Embalaje = table.Column<bool>(type: "bit", nullable: false),
                    ProductosATestear = table.Column<int>(type: "int", nullable: false),
                    ProductosPorRespondiente = table.Column<int>(type: "int", nullable: false),
                    PatinadoresPorCiudad = table.Column<int>(type: "int", nullable: false),
                    Siembra = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_questionnaire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_eq_questionnaire_eq_quote_header_QuoteHeaderId",
                        column: x => x.QuoteHeaderId,
                        principalTable: "eq_quote_header",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "eq_sample_city",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuoteHeaderId = table.Column<int>(type: "int", nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    MuestraTotal = table.Column<int>(type: "int", nullable: false),
                    NSE1 = table.Column<int>(type: "int", nullable: false),
                    NSE2 = table.Column<int>(type: "int", nullable: false),
                    NSE3 = table.Column<int>(type: "int", nullable: false),
                    NSE4 = table.Column<int>(type: "int", nullable: false),
                    NSE5 = table.Column<int>(type: "int", nullable: false),
                    NSE6 = table.Column<int>(type: "int", nullable: false),
                    MetodologiaTecnicaReferenciada = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SobreMuestraPct = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PesoProductoGramos = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EnvioCiudades = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_sample_city", x => x.Id);
                    table.ForeignKey(
                        name: "FK_eq_sample_city_eq_quote_header_QuoteHeaderId",
                        column: x => x.QuoteHeaderId,
                        principalTable: "eq_quote_header",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "eq_staff_sl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuoteHeaderId = table.Column<int>(type: "int", nullable: false),
                    Nivel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HorasMinimas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HorasPresupuestadas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TarifaNivel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fuente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eq_staff_sl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_eq_staff_sl_eq_quote_header_QuoteHeaderId",
                        column: x => x.QuoteHeaderId,
                        principalTable: "eq_quote_header",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PY_AsignacionProyectos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProyecto = table.Column<long>(type: "bigint", nullable: false),
                    IdGerenteProyecto = table.Column<long>(type: "bigint", nullable: false),
                    NombreGerenteProyecto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoAsignacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdGerentePrevio = table.Column<long>(type: "bigint", nullable: true),
                    NombreGerentePrevio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PY_AsignacionProyectos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PY_AsignacionProyectos_Proyectos_IdProyecto",
                        column: x => x.IdProyecto,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trabajos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProyecto = table.Column<long>(type: "bigint", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IdMetodologia = table.Column<int>(type: "int", nullable: false),
                    IdTipoProyecto = table.Column<int>(type: "int", nullable: false),
                    JobBook = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    IdCoordinador = table.Column<long>(type: "bigint", nullable: true),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabajos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trabajos_Proyectos_IdProyecto",
                        column: x => x.IdProyecto,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrabajosCuali",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProyecto = table.Column<long>(type: "bigint", nullable: false),
                    IdTrabajoRelacionado = table.Column<long>(type: "bigint", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Creado"),
                    JobBook = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdCoordinador = table.Column<long>(type: "bigint", nullable: true),
                    IdGerenteProyecto = table.Column<long>(type: "bigint", nullable: true),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PresupuestoEstimado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TipoEstudio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumeroParticipantesEstimado = table.Column<int>(type: "int", nullable: true),
                    Ubicacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProyectoId = table.Column<long>(type: "bigint", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrabajosCuali", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrabajosCuali_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalTable: "Proyectos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ObservacionesTareas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdWorkFlow = table.Column<long>(type: "bigint", nullable: false),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TipoOperacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservacionesTareas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObservacionesTareas_WorkFlows_IdWorkFlow",
                        column: x => x.IdWorkFlow,
                        principalTable: "WorkFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TareasPrevias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTarea = table.Column<long>(type: "bigint", nullable: false),
                    IdTareaPreviaRequerida = table.Column<long>(type: "bigint", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareasPrevias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TareasPrevias_WorkFlows_IdTarea",
                        column: x => x.IdTarea,
                        principalTable: "WorkFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TareasPrevias_WorkFlows_IdTareaPreviaRequerida",
                        column: x => x.IdTareaPreviaRequerida,
                        principalTable: "WorkFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowUsuariosAsignados",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdWorkFlow = table.Column<long>(type: "bigint", nullable: false),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, defaultValue: "Responsable"),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowUsuariosAsignados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkFlowUsuariosAsignados_WorkFlows_IdWorkFlow",
                        column: x => x.IdWorkFlow,
                        principalTable: "WorkFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VariablesControl",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTrabajo = table.Column<long>(type: "bigint", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TipoDato = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariablesControl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariablesControl_Trabajos_IdTrabajo",
                        column: x => x.IdTrabajo,
                        principalTable: "Trabajos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SegmentosCuali",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTrabajoCuali = table.Column<long>(type: "bigint", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NumeroParticipantes = table.Column<int>(type: "int", nullable: false),
                    CuotaMinima = table.Column<int>(type: "int", nullable: true),
                    CuotaMaxima = table.Column<int>(type: "int", nullable: true),
                    CriteriosInclusion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CriteriosExclusion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentosCuali", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SegmentosCuali_TrabajosCuali_IdTrabajoCuali",
                        column: x => x.IdTrabajoCuali,
                        principalTable: "TrabajosCuali",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntrevistadorasCuali",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTrabajoCuali = table.Column<long>(type: "bigint", nullable: false),
                    IdSegmento = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Especialidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumeroEntrevistasAsignadas = table.Column<int>(type: "int", nullable: false),
                    NumeroEntrevistasCompletadas = table.Column<int>(type: "int", nullable: false),
                    PorcentajeCumplimiento = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaTermino = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Asignado"),
                    NivelExperiencia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Disponibilidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Disponible"),
                    Notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrabajoCualiId = table.Column<long>(type: "bigint", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrevistadorasCuali", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntrevistadorasCuali_SegmentosCuali_IdSegmento",
                        column: x => x.IdSegmento,
                        principalTable: "SegmentosCuali",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EntrevistadorasCuali_TrabajosCuali_TrabajoCualiId",
                        column: x => x.TrabajoCualiId,
                        principalTable: "TrabajosCuali",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SesionesCuali",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTrabajoCuali = table.Column<long>(type: "bigint", nullable: false),
                    IdSegmento = table.Column<long>(type: "bigint", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaProgramada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEjecucion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraInicio = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    HoraFin = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    DuracionEstimada = table.Column<int>(type: "int", nullable: true),
                    DuracionReal = table.Column<int>(type: "int", nullable: true),
                    Ubicacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Moderador = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NumeroParticipantesPlaneado = table.Column<int>(type: "int", nullable: true),
                    NumeroParticipantesReal = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Planeada"),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlGrabacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SegmentoId = table.Column<long>(type: "bigint", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesionesCuali", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SesionesCuali_SegmentosCuali_SegmentoId",
                        column: x => x.SegmentoId,
                        principalTable: "SegmentosCuali",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SesionesCuali_TrabajosCuali_IdTrabajoCuali",
                        column: x => x.IdTrabajoCuali,
                        principalTable: "TrabajosCuali",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MuestrasCuali",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTrabajoCuali = table.Column<long>(type: "bigint", nullable: false),
                    IdSegmento = table.Column<long>(type: "bigint", nullable: true),
                    IdSesion = table.Column<long>(type: "bigint", nullable: true),
                    NumeroMuestra = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NombreParticipante = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Edad = table.Column<int>(type: "int", nullable: true),
                    Genero = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Estrato = table.Column<int>(type: "int", nullable: true),
                    Ocupacion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Planeada"),
                    FechaContacto = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaEjecucion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DuracionEntrevista = table.Column<int>(type: "int", nullable: true),
                    CalidadDatos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MotivoRechazo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SesionId = table.Column<long>(type: "bigint", nullable: true),
                    IdEntrevistador = table.Column<long>(type: "bigint", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuestrasCuali", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MuestrasCuali_EntrevistadorasCuali_IdEntrevistador",
                        column: x => x.IdEntrevistador,
                        principalTable: "EntrevistadorasCuali",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MuestrasCuali_SegmentosCuali_IdSegmento",
                        column: x => x.IdSegmento,
                        principalTable: "SegmentosCuali",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MuestrasCuali_SesionesCuali_SesionId",
                        column: x => x.SesionId,
                        principalTable: "SesionesCuali",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MuestrasCuali_TrabajosCuali_IdTrabajoCuali",
                        column: x => x.IdTrabajoCuali,
                        principalTable: "TrabajosCuali",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParticipantesSesion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSesion = table.Column<long>(type: "bigint", nullable: false),
                    IdMuestra = table.Column<long>(type: "bigint", nullable: false),
                    Asistencia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HoraLlegada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraSalida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalidadRespuestas = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MotivoInasistencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MuestraId = table.Column<long>(type: "bigint", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreacion = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioModificacion = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantesSesion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipantesSesion_MuestrasCuali_MuestraId",
                        column: x => x.MuestraId,
                        principalTable: "MuestrasCuali",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParticipantesSesion_SesionesCuali_IdSesion",
                        column: x => x.IdSesion,
                        principalTable: "SesionesCuali",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CORE_Tareas_Nombre",
                table: "CORE_Tareas",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_CORE_Tareas_Visible",
                table: "CORE_Tareas",
                column: "Visible");

            migrationBuilder.CreateIndex(
                name: "IX_EntrevistadorasCuali_Estado",
                table: "EntrevistadorasCuali",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_EntrevistadorasCuali_IdSegmento",
                table: "EntrevistadorasCuali",
                column: "IdSegmento");

            migrationBuilder.CreateIndex(
                name: "IX_EntrevistadorasCuali_IdTrabajoCuali",
                table: "EntrevistadorasCuali",
                column: "IdTrabajoCuali");

            migrationBuilder.CreateIndex(
                name: "IX_EntrevistadorasCuali_IdUsuario",
                table: "EntrevistadorasCuali",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_EntrevistadorasCuali_TrabajoCualiId",
                table: "EntrevistadorasCuali",
                column: "TrabajoCualiId");

            migrationBuilder.CreateIndex(
                name: "IX_EqCostInsumos_NSE",
                table: "eq_cost_insumos",
                column: "NSE");

            migrationBuilder.CreateIndex(
                name: "IX_EqCostResult_QuoteHeaderId",
                table: "eq_cost_result",
                column: "QuoteHeaderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EqLocaciones_Ciudad",
                table: "eq_locaciones",
                column: "Ciudad");

            migrationBuilder.CreateIndex(
                name: "IX_EqMethodology_QuoteHeaderId",
                table: "eq_methodology",
                column: "QuoteHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_EqMystery_QuoteHeaderId",
                table: "eq_mystery",
                column: "QuoteHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_EqParamPrecio_TipoMetodologia_Duracion_Penetracion",
                table: "eq_param_precio",
                columns: new[] { "TipoMetodologia", "DuracionMin", "PenetracionRango" });

            migrationBuilder.CreateIndex(
                name: "IX_EqParamScriptProc_DuracionMin",
                table: "eq_param_script_proc",
                column: "DuracionMin");

            migrationBuilder.CreateIndex(
                name: "IX_EqQuestionnaire_QuoteHeaderId",
                table: "eq_questionnaire",
                column: "QuoteHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_EqQuoteHeader_Cliente",
                table: "eq_quote_header",
                column: "Cliente");

            migrationBuilder.CreateIndex(
                name: "IX_EqQuoteHeader_FechaCreacion",
                table: "eq_quote_header",
                column: "FechaCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_EqRateEstadistica_Categoria",
                table: "eq_rate_estadistica",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_EqSampleCity_Ciudad",
                table: "eq_sample_city",
                column: "Ciudad");

            migrationBuilder.CreateIndex(
                name: "IX_EqSampleCity_QuoteHeaderId",
                table: "eq_sample_city",
                column: "QuoteHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_EqStaffSL_QuoteHeaderId",
                table: "eq_staff_sl",
                column: "QuoteHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_EqValorHoraOps_Nivel_Alternativa",
                table: "eq_valor_hora_ops",
                columns: new[] { "Nivel", "Alternativa" });

            migrationBuilder.CreateIndex(
                name: "IX_MuestrasCuali_Estado",
                table: "MuestrasCuali",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_MuestrasCuali_IdEntrevistador",
                table: "MuestrasCuali",
                column: "IdEntrevistador");

            migrationBuilder.CreateIndex(
                name: "IX_MuestrasCuali_IdSegmento",
                table: "MuestrasCuali",
                column: "IdSegmento");

            migrationBuilder.CreateIndex(
                name: "IX_MuestrasCuali_IdTrabajoCuali",
                table: "MuestrasCuali",
                column: "IdTrabajoCuali");

            migrationBuilder.CreateIndex(
                name: "IX_MuestrasCuali_NumeroMuestra",
                table: "MuestrasCuali",
                column: "NumeroMuestra");

            migrationBuilder.CreateIndex(
                name: "IX_MuestrasCuali_SesionId",
                table: "MuestrasCuali",
                column: "SesionId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservacionTarea_WorkFlowFecha",
                table: "ObservacionesTareas",
                columns: new[] { "IdWorkFlow", "FechaCreacion" });

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantesSesion_Asistencia",
                table: "ParticipantesSesion",
                column: "Asistencia");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantesSesion_IdMuestra",
                table: "ParticipantesSesion",
                column: "IdMuestra");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantesSesion_IdSesion",
                table: "ParticipantesSesion",
                column: "IdSesion");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantesSesion_MuestraId",
                table: "ParticipantesSesion",
                column: "MuestraId");

            migrationBuilder.CreateIndex(
                name: "IX_Proyecto_Activo",
                table: "Proyectos",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Proyecto_IdGerenteProyectos",
                table: "Proyectos",
                column: "IdGerenteProyectos");

            migrationBuilder.CreateIndex(
                name: "IX_Proyecto_IdUnidad",
                table: "Proyectos",
                column: "IdUnidad");

            migrationBuilder.CreateIndex(
                name: "IX_PY_AsignacionProyectos_IdProyecto",
                schema: "dbo",
                table: "PY_AsignacionProyectos",
                column: "IdProyecto");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentosCuali_Activo",
                table: "SegmentosCuali",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentosCuali_IdTrabajoCuali",
                table: "SegmentosCuali",
                column: "IdTrabajoCuali");

            migrationBuilder.CreateIndex(
                name: "IX_SesionesCuali_Estado",
                table: "SesionesCuali",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_SesionesCuali_FechaProgramada",
                table: "SesionesCuali",
                column: "FechaProgramada");

            migrationBuilder.CreateIndex(
                name: "IX_SesionesCuali_IdTrabajoCuali",
                table: "SesionesCuali",
                column: "IdTrabajoCuali");

            migrationBuilder.CreateIndex(
                name: "IX_SesionesCuali_SegmentoId",
                table: "SesionesCuali",
                column: "SegmentoId");

            migrationBuilder.CreateIndex(
                name: "IX_TareaPrevia_IdTareaPreviaRequerida",
                table: "TareasPrevias",
                column: "IdTareaPreviaRequerida");

            migrationBuilder.CreateIndex(
                name: "IX_TareasPrevias_IdTarea",
                table: "TareasPrevias",
                column: "IdTarea");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajo_Activo",
                table: "Trabajos",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajo_Estado",
                table: "Trabajos",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajo_IdProyecto",
                table: "Trabajos",
                column: "IdProyecto");

            migrationBuilder.CreateIndex(
                name: "IX_TrabajosCuali_Activo",
                table: "TrabajosCuali",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_TrabajosCuali_Estado",
                table: "TrabajosCuali",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_TrabajosCuali_IdProyecto",
                table: "TrabajosCuali",
                column: "IdProyecto");

            migrationBuilder.CreateIndex(
                name: "IX_TrabajosCuali_ProyectoId",
                table: "TrabajosCuali",
                column: "ProyectoId");

            migrationBuilder.CreateIndex(
                name: "IX_VariablesControl_IdTrabajo",
                table: "VariablesControl",
                column: "IdTrabajo");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlow_Estado",
                table: "WorkFlows",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlow_IdTrabajo",
                table: "WorkFlows",
                column: "IdTrabajo");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowUsuarioAsignado_IdUsuario",
                table: "WorkFlowUsuariosAsignados",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowUsuariosAsignados_IdWorkFlow",
                table: "WorkFlowUsuariosAsignados",
                column: "IdWorkFlow");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CORE_Tareas");

            migrationBuilder.DropTable(
                name: "eq_cost_insumos");

            migrationBuilder.DropTable(
                name: "eq_cost_result");

            migrationBuilder.DropTable(
                name: "eq_locaciones");

            migrationBuilder.DropTable(
                name: "eq_methodology");

            migrationBuilder.DropTable(
                name: "eq_mystery");

            migrationBuilder.DropTable(
                name: "eq_param_precio");

            migrationBuilder.DropTable(
                name: "eq_param_script_proc");

            migrationBuilder.DropTable(
                name: "eq_questionnaire");

            migrationBuilder.DropTable(
                name: "eq_rate_estadistica");

            migrationBuilder.DropTable(
                name: "eq_sample_city");

            migrationBuilder.DropTable(
                name: "eq_staff_sl");

            migrationBuilder.DropTable(
                name: "eq_valor_hora_ops");

            migrationBuilder.DropTable(
                name: "ObservacionesTareas");

            migrationBuilder.DropTable(
                name: "ParticipantesSesion");

            migrationBuilder.DropTable(
                name: "PY_AsignacionProyectos",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TareasPrevias");

            migrationBuilder.DropTable(
                name: "VariablesControl");

            migrationBuilder.DropTable(
                name: "WorkFlowUsuariosAsignados");

            migrationBuilder.DropTable(
                name: "eq_quote_header");

            migrationBuilder.DropTable(
                name: "MuestrasCuali");

            migrationBuilder.DropTable(
                name: "Trabajos");

            migrationBuilder.DropTable(
                name: "WorkFlows");

            migrationBuilder.DropTable(
                name: "EntrevistadorasCuali");

            migrationBuilder.DropTable(
                name: "SesionesCuali");

            migrationBuilder.DropTable(
                name: "SegmentosCuali");

            migrationBuilder.DropTable(
                name: "TrabajosCuali");

            migrationBuilder.DropTable(
                name: "Proyectos");
        }
    }
}
