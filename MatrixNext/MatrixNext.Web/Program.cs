using MatrixNext.Data.Modules.TH;
using MatrixNext.Data.Modules.US;
using MatrixNext.Data.Services;
using MatrixNext.Data.Services.Usuarios;
using MatrixNext.Web.Middleware;

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
// US module (Usuarios)
builder.Services.AddUSModule(builder.Configuration);
// Usuarios auxiliary services for US area controllers
builder.Services.AddScoped<RolService>();
builder.Services.AddScoped<PermisosService>();
builder.Services.AddScoped<GrupoUnidadService>();
// Register TH module services (Ausencias slice)
builder.Services.AddTHModule(builder.Configuration);

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
