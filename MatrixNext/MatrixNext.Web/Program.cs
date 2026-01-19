using MatrixNext.Data.Modules.TH;
using MatrixNext.Data.Modules.CU;
using MatrixNext.Data.Modules.US;
using MatrixNext.Data.Modules.CC;
using MatrixNext.Data.Services;
using MatrixNext.Data.Services.Usuarios;
using MatrixNext.Web.Middleware;
using MatrixNext.Web.Areas.EQ.Services;
using MatrixNext.Web.Areas.EQ.Services.Internal;
using MatrixNext.Web.Areas.EQ.Services.Masters;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.OP.Interfaces;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.Options;
using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.Services.Shared;
using Microsoft.EntityFrameworkCore;
using MatrixNext.Data.Services.GD.Interfaces;
using MatrixNext.Data.Services.GD;
using MatrixNext.Data.Adapters.PY;
using MatrixNext.Data.Services.PY.Interfaces;
using MatrixNext.Data.Services.PY;
using MatrixNext.Data.Adapters.GD;
using MatrixNext.Data.Services.Pnc;
using MatrixNext.Data.Adapters.Pnc;
using MatrixNext.Data.Adapters.TH;
using MatrixNext.Data.Services.TH;
using MatrixNext.Data.Services.TH.Interfaces;
using MatrixNext.Data.Adapters.RP;
using MatrixNext.Data.Services.RP;
using MatrixNext.Data.Adapters.OP_RO;
using MatrixNext.Data.Services.OP_RO;
using MatrixNext.Data.Adapters.OP_Trafico;
using MatrixNext.Data.Services.OP_Trafico;
using MatrixNext.Data.Services.Authorization;
using MatrixNext.Data.Adapters.SGC;
using MatrixNext.Data.Services.SGC;
using MatrixNext.Data.Adapters.IT;
using MatrixNext.Data.Services.IT;
using System.Data;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();

// Add in-memory caching for catalogs (S4-006.3 performance optimization)
builder.Services.AddMemoryCache();

// Configure session and authentication
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".Matrix.Session";
});

builder.Services.AddAuthentication("MatrixCookies")
    .AddCookie("MatrixCookies", options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.Name = ".Matrix.Auth";
    });

builder.Services.AddHttpContextAccessor();

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

builder.Services.Configure<PresupuestoNotificationOptions>(builder.Configuration.GetSection("Notifications:Presupuestos"));

// Register data services
var connectionString = builder.Configuration.GetConnectionString("MatrixDb");
builder.Services.AddScoped(sp => new LogService(connectionString!));

// ===== SPRINT 10-11: RP_Reportes + OP_RO + OP_Trafico =====
// Dapper connection (shared)
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString!));

// RP_Reportes (Sprint 10)
builder.Services.AddScoped<IReportesAdapter, ReportesAdapter>();
builder.Services.AddScoped<IReportesService, ReportesService>();

// OP_RO (Sprint 11A)
builder.Services.AddScoped<IOP_ROAdapter, OP_ROAdapter>();
builder.Services.AddScoped<IOP_ROService, OP_ROService>();

// OP_Trafico (Sprint 11B)
builder.Services.AddScoped<IOP_TraficoAdapter, OP_TraficoAdapter>();
builder.Services.AddScoped<IOP_TraficoService, OP_TraficoService>();

// ===== SPRINT 13: SGC_Calidad (Auditorías + Acciones Mejora) =====
// Adapters and Services for SGC (Sistema de Gestión de Calidad)
builder.Services.AddScoped<ISGCAuditoriaAdapter, SGCAuditoriaAdapter>();
builder.Services.AddScoped<ISGCAccionMejoraAdapter, SGCAccionMejoraAdapter>();
builder.Services.AddScoped<ISGCAuditoriaService, SGCAuditoriaService>();
builder.Services.AddScoped<ISGCAccionMejoraService, SGCAccionMejoraService>();

// ===== SPRINT 14: ES_Estadistica (Brief, Diseño, Metodología) =====
// Adapters and Services for ES (Estadística)
builder.Services.AddScoped<MatrixNext.Data.Adapters.ES.IESBriefDisenoMuestralAdapter, MatrixNext.Data.Adapters.ES.ESBriefDisenoMuestralAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Adapters.ES.IESDisenoMuestralAdapter, MatrixNext.Data.Adapters.ES.ESDisenoMuestralAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Adapters.ES.IESMetodologiaCampoAdapter, MatrixNext.Data.Adapters.ES.ESMetodologiaCampoAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.ES.IESBriefDisenoMuestralService, MatrixNext.Data.Services.ES.ESBriefDisenoMuestralService>();
builder.Services.AddScoped<MatrixNext.Data.Services.ES.IESDisenoMuestralService, MatrixNext.Data.Services.ES.ESDisenoMuestralService>();
builder.Services.AddScoped<MatrixNext.Data.Services.ES.IESMetodologiaCampoService, MatrixNext.Data.Services.ES.ESMetodologiaCampoService>();

// ===== SPRINT 15: IT (Infraestructura Tecnológica - Sincronización) =====
// Adapter and Service for IT (Sync issues, trabajos, encuestas piloto)
builder.Services.AddScoped<IITSyncAdapter, ITSyncAdapter>();
builder.Services.AddScoped<IITSyncService, ITSyncService>();

// ===== SPRINT 16: MBO (Management By Objectives) =====
// MBO Fase 1: AOT (Achievement of Tasks)
builder.Services.AddScoped<MatrixNext.Data.Adapters.MBO.IAOTAdapter, MatrixNext.Data.Adapters.MBO.AOTAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.MBO.IAOTService, MatrixNext.Data.Services.MBO.AOTService>();

// MBO Fase 2: Campo (Field Quality Management)
builder.Services.AddScoped<MatrixNext.Data.Adapters.MBO.ICampoAdapter, MatrixNext.Data.Adapters.MBO.CampoAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.MBO.ICampoService, MatrixNext.Data.Services.MBO.CampoService>();

// MBO Fase 3: Propuestas y Gestión (Proposals & Management)
builder.Services.AddScoped<MatrixNext.Data.Adapters.MBO.IPropuestasAdapter, MatrixNext.Data.Adapters.MBO.PropuestasAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.MBO.IPropuestasService, MatrixNext.Data.Services.MBO.PropuestasService>();

// ===== SPRINT 19: PC_PropiedadCliente (Inventario Productos Internos) =====
// Adapter and Service for PC (Productos internos entre unidades)
builder.Services.AddScoped<MatrixNext.Data.Adapters.PC.IProductoInternoAdapter, MatrixNext.Data.Adapters.PC.ProductoInternoAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.PC.IProductoInternoService, MatrixNext.Data.Services.PC.ProductoInternoService>();

// ===== SPRINT 20: INV_Inventario (Módulo final - 28/28 módulos) =====
// Adapters and Services for INV (Registro Artículos, Asignaciones, Stock, Legalizaciones, Mantenimiento)
builder.Services.AddScoped<MatrixNext.Data.Adapters.INV.IRegistroArticulosAdapter, MatrixNext.Data.Adapters.INV.RegistroArticulosAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Adapters.INV.IAsignacionesAdapter, MatrixNext.Data.Adapters.INV.AsignacionesAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Adapters.INV.IStockConsumiblesAdapter, MatrixNext.Data.Adapters.INV.StockConsumiblesAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Adapters.INV.ILegalizacionesAdapter, MatrixNext.Data.Adapters.INV.LegalizacionesAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Adapters.INV.IMantenimientoEquiposAdapter, MatrixNext.Data.Adapters.INV.MantenimientoEquiposAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.INV.IRegistroArticulosService, MatrixNext.Data.Services.INV.RegistroArticulosService>();
builder.Services.AddScoped<MatrixNext.Data.Services.INV.IAsignacionesService, MatrixNext.Data.Services.INV.AsignacionesService>();
builder.Services.AddScoped<MatrixNext.Data.Services.INV.IStockConsumiblesService, MatrixNext.Data.Services.INV.StockConsumiblesService>();
builder.Services.AddScoped<MatrixNext.Data.Services.INV.ILegalizacionesService, MatrixNext.Data.Services.INV.LegalizacionesService>();
builder.Services.AddScoped<MatrixNext.Data.Services.INV.IMantenimientoEquiposService, MatrixNext.Data.Services.INV.MantenimientoEquiposService>();

// ===== SPRINT 21: INV Reportes + RP Indicadores/AvanceCampo =====
// INV Reportes (Legalizaciones, Remanente)
builder.Services.AddScoped<MatrixNext.Data.Services.INV.IReportesInvService, MatrixNext.Data.Services.INV.ReportesInvService>();

// RP Indicadores de Calidad (Esquema Análisis, Diligenciamiento Brief, Propuestas 48h)
builder.Services.AddScoped<MatrixNext.Data.Services.RP.IIndicadoresCalidadService, MatrixNext.Data.Services.RP.IndicadoresCalidadService>();

// RP Avance de Campo (General, Ciudad, Áreas, Remanentes, Matriz Cumplimiento)
builder.Services.AddScoped<MatrixNext.Data.Services.RP.IAvanceCampoService, MatrixNext.Data.Services.RP.AvanceCampoService>();

// Authorization Service (Sprint 10-11)
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

// ===== SPRINT 0: SHARED SERVICES (Infraestructura) =====
// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T0.2-T0.6
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IGridService, GridService>();
builder.Services.AddScoped<IPYPermisosService, PYPermisosService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<EmailQueueService>(); // Singleton for queue state management
builder.Services.AddScoped<IEmailQueueService>(sp => sp.GetRequiredService<EmailQueueService>()); // Scoped wrapper
builder.Services.AddHostedService<EmailQueueBackgroundService>(); // Background processing
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();
builder.Services.AddScoped<GrafoAciclicoService>();

// ===== SPRINT 4: EXPORT AUDIT SERVICES =====
// Ref: S4-004 (Tracking de Exportes Excel)
builder.Services.AddScoped<IOpExportesAuditoriaService, OpExportesAuditoriaService>();
builder.Services.AddHostedService<ExportAuditoriaCleanupBackgroundService>(); // Cleanup runs every hour

// ===== SPRINT 6: OP_CUALITATIVO Background Services =====
// Ref: SPRINT 6 Fase 5 - Email/Notifications
builder.Services.AddHostedService<OpReminderBackgroundService>(); // Recordatorios cada 6 horas

// ===== SPRINT 1: CORE Services & Adapters =====
// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T1 (CORE Catálogos)
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.WorkFlowDataAdapter>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.TareasPreviasDataAdapter>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.IWorkFlowService, MatrixNext.Web.Services.CORE.WorkFlowService>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.ITareasPreviasService, MatrixNext.Web.Services.CORE.TareasPreviasService>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.ITareasService, MatrixNext.Web.Services.CORE.TareasService>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.TareasPorTipoHiloDataAdapter>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.ITareasPorTipoHiloService, MatrixNext.Web.Services.CORE.TareasPorTipoHiloService>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.TareasDocumentosDataAdapter>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.ITareasDocumentosService, MatrixNext.Web.Services.CORE.TareasDocumentosService>();

// ===== SPRINT 2: PY Maestros =====
// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T2 (PY Maestros)
builder.Services.AddScoped<IProyectosService, ProyectosService>();
builder.Services.AddScoped<ITrabajosService, TrabajosService>();
builder.Services.AddScoped<ITrabajosWorkFlowService, TrabajosWorkFlowService>();
builder.Services.AddScoped<IMetodologiasLookupService, MetodologiasLookupService>();

// ===== SPRINT 3: CORE Operación =====
// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T3 (CORE Operación)
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.IAsignacionesService, MatrixNext.Web.Services.CORE.AsignacionesService>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.IWorkFlowStateTransitionService, MatrixNext.Web.Services.CORE.WorkFlowStateTransitionService>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.IGestionTareasService, MatrixNext.Web.Services.CORE.GestionTareasService>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.IWorkFlowReportesService, MatrixNext.Web.Services.CORE.WorkFlowReportesService>();

// ===== SPRINT 4: PY Cualitativos =====
// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T4 (Trabajos Cualitativos)
builder.Services.AddScoped<ITrabajosCualiService, TrabajosCualiService>();
builder.Services.AddScoped<ISegmentosCualiService, SegmentosCualiService>();
builder.Services.AddScoped<ISesionesCualiService, SesionesCualiService>();
builder.Services.AddScoped<IMuestrasCualiService, MuestrasCualiService>();
builder.Services.AddScoped<IEntrevistadorasCualiService, EntrevistadorasCualiService>();

// ===== SPRINT 5: PY Asignaciones =====
// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T5 (Asignaciones & Reasignaciones)
builder.Services.AddScoped<IAsignacionesProyectosService, AsignacionesProyectosService>();

// ===== SPRINT 8: EasyQuote EF Core Services =====
// Ref: README_SPRINTS_5_12.md § Sprint 8 - FASE 1C
builder.Services.AddScoped<MatrixNext.Web.Services.EQ.IEasyQuoteService, MatrixNext.Web.Services.EQ.EasyQuoteService>();
builder.Services.AddScoped<MatrixNext.Web.Services.EQ.IEasyCostService, MatrixNext.Web.Services.EQ.EasyCostService>();
builder.Services.AddScoped<MatrixNext.Web.Services.EQ.IEasyMasterService, MatrixNext.Web.Services.EQ.EasyMasterService>();
builder.Services.AddScoped<MatrixNext.Web.Services.EQ.EqSeedService>(); // FASE 2: Seed service para maestras

// FASE 3: Adapters + Motor de cálculos
// QuoteHeaderToViewModelAdapter is instantiated locally in services

// FASE 4: Retrieval service para controllers
builder.Services.AddScoped<MatrixNext.Web.Services.EQ.EasyQuoteRetrievalService>();

// QuoteCalculator para motor de cálculos (usado por EasyCostService)
builder.Services.AddScoped<MatrixNext.Web.Areas.EQ.Services.Internal.QuoteCalculator>();

// ===== SPRINT 9: Home Dashboard Service =====
// Ref: README_SPRINTS_5_12.md § Sprint 9 - Home Dashboard
builder.Services.AddScoped<MatrixNext.Web.Services.Dashboard.IDashboardService, MatrixNext.Web.Services.Dashboard.DashboardService>();

// DbContext principal (PY, CORE, OP)
builder.Services.AddDbContext<MatrixDbContext>(options =>
    options.UseSqlServer(connectionString));

// ===== US module (Usuarios) =====
builder.Services.AddUSModule(builder.Configuration);
// Usuarios auxiliary services for US area controllers
builder.Services.AddScoped<RolService>();
builder.Services.AddScoped<MatrixNext.Data.Services.Usuarios.PermisosService>();
builder.Services.AddScoped<GrupoUnidadService>();
// Register TH module services (Ausencias slice)
builder.Services.AddTHModule(builder.Configuration);
// Register CU_Cuentas module services
builder.Services.AddCUModule(builder.Configuration);
// Register CC FinzOpe module services
builder.Services.AddCCModule(builder.Configuration);
// EQ module services
builder.Services.AddScoped<EasyQuoteService>();
builder.Services.AddScoped<EasyQuoteAdminService>();
builder.Services.AddScoped<EasyQuoteAdapter>();
// PY Dashboard services
builder.Services.AddScoped<IDashboardService, DashboardService>();
// CORE Dashboard services
builder.Services.AddScoped<IWorkFlowDashboardService, WorkFlowDashboardService>();
builder.Services.AddScoped<IIndicadoresCumplimientoService, IndicadoresCumplimientoService>();
// Shared services
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddScoped<QuoteCalculator>();
builder.Services.AddScoped<EasyQuoteMasterService>();
// OP módulo
builder.Services.AddScoped<IOpCatalogCacheService, OpCatalogCacheService>(); // S4-006.3 Catalog caching
builder.Services.AddScoped<IOpPortalService, OpPortalService>();
builder.Services.AddScoped<IOpTraficoDataAdapter, OpTraficoDataAdapter>();
builder.Services.AddScoped<IOpTraficoService, OpTraficoService>();
builder.Services.AddScoped<IOpPermisosService, OpPermisosService>();
builder.Services.AddScoped<IOpEncuestasService, OpEncuestasService>();

// ===== SPRINT 12.1: OP Encuestas (Activación/Anulación) =====
// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.1
builder.Services.AddScoped<MatrixNext.Data.Adapters.OP.IEncuestasAdapter, MatrixNext.Data.Adapters.OP.EncuestasAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.OP.IEncuestasService, MatrixNext.Data.Services.OP.EncuestasService>();

// ===== SPRINT 12.1.2: OP Planillas Aprobadas/Rechazadas =====
// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.2
builder.Services.AddScoped<MatrixNext.Data.Adapters.OP.IPlanillasAprobacionAdapter, MatrixNext.Data.Adapters.OP.PlanillasAprobacionAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.OP.IPlanillasAprobacionService, MatrixNext.Data.Services.OP.PlanillasAprobacionService>();

// ===== SPRINT 12.1.3: OP IPS Detallado por Tarea =====
// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.3
builder.Services.AddScoped<MatrixNext.Data.Adapters.OP.IIpsRevisionAdapter, MatrixNext.Data.Adapters.OP.IpsRevisionAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.OP.IIpsRevisionService, MatrixNext.Data.Services.OP.IpsRevisionService>();

// ===== SPRINT 12.1.4: OP Dashboard HomeRecoleccion =====
// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.4
builder.Services.AddScoped<MatrixNext.Data.Adapters.OP.IHomeRecoleccionDashboardAdapter, MatrixNext.Data.Adapters.OP.HomeRecoleccionDashboardAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.OP.IHomeRecoleccionDashboardService, MatrixNext.Data.Services.OP.HomeRecoleccionDashboardService>();

// ===== SPRINT 12.1.5: OP Correos en FichaCuantitativa =====
// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.5
builder.Services.AddScoped<MatrixNext.Data.Adapters.OP.INotificacionesOpAdapter, MatrixNext.Data.Adapters.OP.NotificacionesOpAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.OP.IOpNotificacionService, MatrixNext.Data.Services.OP.OpNotificacionService>();

// ===== SPRINT 12.1.6: OP Cierre de Trabajo con GD =====
// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.6
builder.Services.AddScoped<MatrixNext.Data.Adapters.OP.ICierreTrabajoAdapter, MatrixNext.Data.Adapters.OP.CierreTrabajoAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.OP.ICierreTrabajoService, MatrixNext.Data.Services.OP.CierreTrabajoService>();

// ===== SPRINT 12.1.7: OP Carga Masiva CATI vs Planillas =====
// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.7
builder.Services.AddScoped<MatrixNext.Data.Adapters.OP.ICargaMasivaAdapter, MatrixNext.Data.Adapters.OP.CargaMasivaAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.OP.ICargaMasivaService, MatrixNext.Data.Services.OP.CargaMasivaService>();

// ===== SPRINT 12.1.8: OP Consolidar Productividad Multiroles =====
// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.8
builder.Services.AddScoped<MatrixNext.Data.Adapters.OP.IProductividadAdapter, MatrixNext.Data.Adapters.OP.ProductividadAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.OP.IProductividadConsolidadoService, MatrixNext.Data.Services.OP.ProductividadConsolidadoService>();

// ===== SPRINT 12.1.9: OP Tráfico de Encuestas Completo =====
// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.9
builder.Services.AddScoped<MatrixNext.Data.Adapters.OP.ITraficoAdapter, MatrixNext.Data.Adapters.OP.TraficoAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.OP.ITraficoService, MatrixNext.Data.Services.OP.TraficoService>();

// ===== SPRINT 12.1.10: OP Supervisión Telefónica =====
// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.10
builder.Services.AddScoped<MatrixNext.Data.Adapters.OP.ISupervisionAdapter, MatrixNext.Data.Adapters.OP.SupervisionAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.OP.ISupervisionService, MatrixNext.Data.Services.OP.SupervisionService>();

// ===== SPRINT 12.2: PY Distribución Entrevistas, Variables Control, InHome Visits =====
// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.1-12.2.3
builder.Services.AddScoped<MatrixNext.Data.Adapters.PY.IDistribucionAdapter, MatrixNext.Data.Adapters.PY.DistribucionAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.PY.IDistribucionService, MatrixNext.Data.Services.PY.DistribucionService>();

builder.Services.AddScoped<IOpCargaService, OpCargaService>();
builder.Services.AddScoped<IOpPlanillasService, OpPlanillasService>();
builder.Services.AddScoped<IOpProductividadService, OpProductividadService>();
builder.Services.AddScoped<IOpIpsService, OpIpsService>();
builder.Services.AddScoped<IOpPresupuestosService, OpPresupuestosService>();
builder.Services.AddScoped<IOpProduccionService, OpProduccionService>();
builder.Services.AddScoped<IOpIFieldService, OpIFieldService>();
builder.Services.AddScoped<IOpSupervisionService, OpSupervisionService>();
builder.Services.AddScoped<IOpTrabajosService, OpTrabajosService>();
builder.Services.AddScoped<IOpCoordinacionService, OpCoordinacionService>();
builder.Services.AddScoped<IOpFichaService, OpFichaService>();
builder.Services.AddScoped<IOpEstimacionService, OpEstimacionService>();
builder.Services.AddScoped<IOpMuestraService, OpMuestraService>();
builder.Services.AddScoped<IOpFestivosService, OpFestivosService>();
builder.Services.AddScoped<IOpGestionDocumentalService, OpGestionDocumentalService>();
builder.Services.AddScoped<IOpRevisionProductividadService, OpRevisionProductividadService>();
builder.Services.AddScoped<IOpRegistroProduccionService, OpRegistroProduccionService>();

// ===== SPRINT OP_CUALITATIVO: Servicios para módulo cualitativo =====
// Ref: BACKLOG_MODULO_OP_CUALITATIVO.md § Sprint 1 - Infrastructure
builder.Services.AddScoped<IOpCualitativoService, OpCualitativoService>();
builder.Services.AddScoped<IOpFiltrosService, OpFiltrosService>();
builder.Services.AddScoped<IOpFichasTecnicasService, OpFichasTecnicasService>();
builder.Services.AddScoped<IOpProgramacionService, OpProgramacionService>();
builder.Services.AddScoped<IOpNotificacionService, OpNotificacionService>(); // Sprint 6 - Email Notifications
builder.Services.AddScoped<IOpBulkImportService, OpBulkImportService>(); // Sprint 6 - Bulk Import
builder.Services.AddScoped<IOpPlanillasModeracionService, OpPlanillasModeracionService>(); // Sprint 2
builder.Services.AddScoped<IOpReportService, OpReportService>(); // Sprint 6 - Reportes/Exportes
builder.Services.AddScoped<IOpAdvancedFiltersService, OpAdvancedFiltersService>(); // Sprint 6 - Filtros avanzados
builder.Services.AddScoped<IOpNotificationService, OpNotificationService>(); // Sprint 6 - Notificaciones
// IOpIpsService ya registrado previamente

// Registrar opciones de configuración
builder.Services.Configure<GestionDocumentalOptions>(
    builder.Configuration.GetSection(GestionDocumentalOptions.SectionName));

// ===== GD module (Gestión Documental) =====
// Ref: BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE1.md § T1.3-T1.4
builder.Services.AddScoped<IGdCatalogosService, GdCatalogosService>();
builder.Services.AddScoped<IGdMaestroService, GdMaestroService>();
builder.Services.AddScoped<IGdSolicitudesService, GdSolicitudesService>();
builder.Services.AddScoped<IGdRepositorioService, GdRepositorioService>();
builder.Services.AddScoped<IGdAprobacionesService, GdAprobacionesService>();
builder.Services.AddScoped<IGdPncService, GdPncService>();
builder.Services.AddScoped<IGdEmailService, GdEmailService>();

// GD adapters
builder.Services.AddScoped<IGdCatalogosAdapter, GdCatalogosAdapter>();
builder.Services.AddScoped<IGdMaestroAdapter, GdMaestroAdapter>();
builder.Services.AddScoped<IGdSolicitudesAdapter, GdSolicitudesAdapter>();
builder.Services.AddScoped<IGdRepositorioAdapter, GdRepositorioAdapter>();
builder.Services.AddScoped<IGdAprobacionesAdapter, GdAprobacionesAdapter>();

// ===== PNC module (Producto No Conforme) =====
// Ref: FASE 5 PARTE A - Sprint 8 (PNC)
builder.Services.AddScoped<MatrixNext.Data.Adapters.Pnc.IPncAdapter, MatrixNext.Data.Adapters.Pnc.PncAdapter>();
builder.Services.AddScoped<MatrixNext.Data.Services.Pnc.IPncService, MatrixNext.Data.Services.Pnc.PncService>();

// ===== SPRINT 3: PY Proyectos Pendientes Module =====
// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § Sprint 3
// PY Adapters
builder.Services.AddScoped<IPyInHomeVisitAdapter, PyInHomeVisitAdapter>();
builder.Services.AddScoped<IPyVariablesControlAdapter, PyVariablesControlAdapter>();
builder.Services.AddScoped<IPyInstructivosAdapter, PyInstructivosAdapter>();
builder.Services.AddScoped<IPyPlanillasAdapter, PyPlanillasAdapter>();
builder.Services.AddScoped<IPyDistribucionEntrevistasAdapter, PyDistribucionEntrevistasAdapter>();
builder.Services.AddScoped<IPyTrabajosAdapter, PyTrabajosAdapter>();

// PY Services
builder.Services.AddScoped<IPyInHomeVisitService, PyInHomeVisitService>();
builder.Services.AddScoped<IPyVariablesControlService, PyVariablesControlService>();
builder.Services.AddScoped<IPyInstructivosService, PyInstructivosService>();
builder.Services.AddScoped<IPyPlanillasService, PyPlanillasService>();
builder.Services.AddScoped<IPyDistribucionEntrevistasService, PyDistribucionEntrevistasService>();
builder.Services.AddScoped<IPyTrabajosService, PyTrabajosService>();
builder.Services.AddScoped<IInstructivosService, InstructivosService>(); // Ref: AUDITORIA - Corrección violación arquitectura

// ===== SPRINT 4: TH Talento Humano Module =====
// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § Sprint 4 TH
// TH Adapters
builder.Services.AddScoped<IThEmpleadosAdapter, ThEmpleadosAdapter>();
builder.Services.AddScoped<IThDesvinculacionAdapter, ThDesvinculacionAdapter>();
builder.Services.AddScoped<IThCatalogosAdapter, ThCatalogosAdapter>();

// TH Services
builder.Services.AddScoped<IThEmpleadosService, ThEmpleadosService>();
builder.Services.AddScoped<IThDesvinculacionService, ThDesvinculacionService>();
builder.Services.AddScoped<IThCatalogosService, ThCatalogosService>();

// ===== SPRINT 7: CORE Workflow/Tareas =====
// Ref: SPRINT_7_KICKOFF.md § Architecture
builder.Services.AddScoped<ICoreTaskService, CoreTaskService>();
builder.Services.AddScoped<ICoreWorkflowService, CoreWorkflowService>();
builder.Services.AddScoped<ICoreAssignmentService, CoreAssignmentService>();
builder.Services.AddScoped<ICoreNotificationService, CoreNotificationService>();
builder.Services.AddScoped<ICoreAuditService, CoreAuditService>();

// ===== SPRINT 17: RE_GT Recolección y Gestión/Tratamiento =====
// Ref: docs/RE_GT/SPRINT17_FASE3_PLAN.md § Fase 3
builder.Services.AddScoped<MatrixNext.Web.Services.RE_GT.IRecoleccionDatosService, MatrixNext.Web.Services.RE_GT.RecoleccionDatosService>();

// ===== SPRINT 18: RE_GT CambioJBI & AsignacionCampo =====
// Ref: docs/RE_GT/SPRINT18_PLAN.md § Fase 2
builder.Services.AddScoped<MatrixNext.Data.Adapters.RE_GT.ICambioJBIAdapter, MatrixNext.Data.Adapters.RE_GT.CambioJBIAdapter>();
builder.Services.AddScoped<MatrixNext.Web.Services.RE_GT.ICambioJBIService, MatrixNext.Web.Services.RE_GT.CambioJBIService>();
builder.Services.AddScoped<MatrixNext.Data.Adapters.RE_GT.IAsignacionCampoAdapter, MatrixNext.Data.Adapters.RE_GT.AsignacionCampoAdapter>();
builder.Services.AddScoped<MatrixNext.Web.Services.RE_GT.IAsignacionCampoService, MatrixNext.Web.Services.RE_GT.AsignacionCampoService>();

var app = builder.Build();

// ===== FASE 3: EasyQuote Master Data Seeding =====
// Ejecutar seed automático de maestras en startup (solo si no existen datos)
using (var scope = app.Services.CreateScope())
{
    var seedService = scope.ServiceProvider.GetRequiredService<MatrixNext.Web.Services.EQ.EqSeedService>();
    try
    {
        await seedService.SeedAllMasterTablesAsync(force: false);
        System.Console.WriteLine("✅ EasyQuote master data seeding completed successfully");
    }
    catch (Exception ex)
    {
        System.Console.WriteLine($"⚠️ EasyQuote master data seeding failed (non-critical): {ex.Message}");
        // No bloquear startup si el seed falla
    }
}

// Middleware global de manejo de excepciones
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map health check endpoint
app.MapHealthChecks("/health");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Gestión Documental (GD) area explicit route
app.MapAreaControllerRoute(
    name: "gd_route",
    areaName: "GD",
    pattern: "GD/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "usuariosRoute",
    pattern: "Usuarios/{action=Index}/{id?}",
    defaults: new { controller = "Usuarios", area = "US" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

// ===== SPRINT 6: SignalR Hubs para notificaciones =====
app.MapHub<MatrixNext.Web.Services.OP.Hubs.OpNotificationsHub>("/hubs/op-notifications");
app.MapHub<MatrixNext.Web.Services.CORE.CoreNotificationsHub>("/hubs/core-notifications");

// ===== SPRINT 7: SignalR Hub para WorkFlow =====
app.MapHub<MatrixNext.Web.Hubs.WorkFlowHub>("/workflowHub");

app.Run();
