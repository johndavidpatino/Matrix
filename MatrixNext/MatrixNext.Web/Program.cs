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
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.Services.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddHttpClient();

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

// Register data services
var connectionString = builder.Configuration.GetConnectionString("MatrixDb");
builder.Services.AddScoped(sp => new LogService(connectionString!));

// ===== SPRINT 0: SHARED SERVICES (Infraestructura) =====
// Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T0.2-T0.6
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IGridService, GridService>();
builder.Services.AddScoped<IPYPermisosService, PYPermisosService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();
builder.Services.AddScoped<GrafoAciclicoService>();

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
builder.Services.AddScoped<IOpAvancesService, OpAvancesService>();
builder.Services.AddScoped<IOpPortalService, OpPortalService>();
builder.Services.AddScoped<IOpTraficoDataAdapter, OpTraficoDataAdapter>();
builder.Services.AddScoped<IOpTraficoService, OpTraficoService>();

var app = builder.Build();

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

app.MapControllerRoute(
    name: "usuariosRoute",
    pattern: "Usuarios/{action=Index}/{id?}",
    defaults: new { controller = "Usuarios", area = "US" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
