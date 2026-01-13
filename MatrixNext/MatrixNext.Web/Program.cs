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

// ===== SPRINT 1: CORE Services & Adapters =====
// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T1 (CORE Catálogos)
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.WorkFlowDataAdapter>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.TareasPreviasDataAdapter>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.IWorkFlowService, MatrixNext.Web.Services.CORE.WorkFlowService>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.ITareasPreviasService, MatrixNext.Web.Services.CORE.TareasPreviasService>();

// ===== SPRINT 2: PY Maestros =====
// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T2 (PY Maestros)
builder.Services.AddScoped<IProyectosService, ProyectosService>();
builder.Services.AddScoped<ITrabajosService, TrabajosService>();
builder.Services.AddScoped<ITrabajosWorkFlowService, TrabajosWorkFlowService>();
builder.Services.AddScoped<IMetodologiasLookupService, MetodologiasLookupService>();

// ===== SPRINT 3: CORE Operación =====
// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T3 (CORE Operación)
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.IAsignacionesService, MatrixNext.Web.Services.CORE.AsignacionesService>();
builder.Services.AddScoped<MatrixNext.Web.Services.CORE.IGestionTareasService, MatrixNext.Web.Services.CORE.GestionTareasService>();

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
builder.Services.AddScoped<IOpAvancesService, OpAvancesService>();
builder.Services.AddScoped<IOpPortalService, OpPortalService>();
builder.Services.AddScoped<IOpTraficoDataAdapter, OpTraficoDataAdapter>();
builder.Services.AddScoped<IOpTraficoService, OpTraficoService>();
builder.Services.AddScoped<IOpPermisosService, OpPermisosService>();
builder.Services.AddScoped<IOpEncuestasService, OpEncuestasService>();
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
builder.Services.AddScoped<IPncAdapter, PncAdapter>();
builder.Services.AddScoped<IPncService, PncService>();

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

app.Run();
