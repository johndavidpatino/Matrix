using MatrixNext.Data.Context;
using MatrixNext.Data.Modules.TH.Ausencias.Adapters;
using MatrixNext.Data.Modules.TH.Ausencias.Services;
using MatrixNext.Data.Modules.TH.Capacitaciones.Adapters;
using MatrixNext.Data.Modules.TH.Capacitaciones.Services;
using MatrixNext.Data.Modules.TH.Contratistas.Adapters;
using MatrixNext.Data.Modules.TH.Contratistas.Services;
using MatrixNext.Data.Modules.TH.Empleados.Adapters;
using MatrixNext.Data.Modules.TH.Empleados.Services;
using MatrixNext.Data.Modules.TH.HojasVida.Adapters;
using MatrixNext.Data.Modules.TH.HojasVida.Services;
using MatrixNext.Data.Adapters.TH;
using MatrixNext.Data.Services.TH;
using MatrixNext.Data.Services.TH.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.TH
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra los servicios del módulo de Talento Humano (Ausencias y Empleados).
        /// </summary>
        public static IServiceCollection AddTHModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Register ApplicationDbContext for Dapper-based TH adapters
            var connectionString = configuration.GetConnectionString("MatrixDb");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Ausencias services
            services.AddScoped(sp => new AusenciaDataAdapter(
                configuration, 
                sp.GetRequiredService<ILogger<AusenciaDataAdapter>>()));
            services.AddScoped<AusenciaService>();
            
            // Modular Empleados services (para todos los controllers TH)
            services.AddScoped(sp => new Empleados.Adapters.EmpleadoDataAdapter(
                configuration,
                sp.GetRequiredService<ILogger<Empleados.Adapters.EmpleadoDataAdapter>>()));
            services.AddScoped<Empleados.Services.EmpleadoService>();

            // Desvinculaciones services
            services.AddScoped(sp => new DesvinculacionDataAdapter(configuration));
            services.AddScoped<DesvinculacionService>();
            
            // Sprint 4 TH API Adapters and Services
            services.AddScoped<IThEmpleadosAdapter, ThEmpleadosAdapter>();
            services.AddScoped<IThEmpleadosService, ThEmpleadosService>();
            
            services.AddScoped<IThCatalogosAdapter, ThCatalogosAdapter>();
            services.AddScoped<IThCatalogosService, ThCatalogosService>();
            services.AddScoped<IThDesvinculacionAdapter, ThDesvinculacionAdapter>();
            services.AddScoped<IThDesvinculacionService, ThDesvinculacionService>();
            
            // Capacitaciones services (Sprint Fase 1 TH)
            services.AddScoped<ICapacitacionAdapter, CapacitacionAdapter>();
            services.AddScoped<ICapacitacionService, CapacitacionService>();
            
            // Contratistas services (Sprint Fase 1 TH)
            services.AddScoped<IContratistaAdapter, ContratistaAdapter>();
            services.AddScoped<IContratistaService, ContratistaService>();
            
            // HojasVida services (Sprint Fase 1 TH - Reclutamiento)
            services.AddScoped<IHojaVidaAdapter, HojaVidaAdapter>();
            services.AddScoped<IHojaVidaService, HojaVidaService>();
            
            return services;
        }
    }
}
