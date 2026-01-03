using MatrixNext.Data.Modules.TH.Ausencias.Adapters;
using MatrixNext.Data.Modules.TH.Ausencias.Services;
using MatrixNext.Data.Modules.TH.Empleados.Adapters;
using MatrixNext.Data.Modules.TH.Empleados.Services;
using MatrixNext.Data.Adapters.TH;
using MatrixNext.Data.Services.TH;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MatrixNext.Data.Modules.TH
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra los servicios del módulo de Talento Humano (Ausencias y Empleados).
        /// </summary>
        public static IServiceCollection AddTHModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Ausencias services
            services.AddScoped(sp => new AusenciaDataAdapter(configuration));
            services.AddScoped<AusenciaService>();
            
            // Modular Empleados services (para todos los controllers TH)
            services.AddScoped(sp => new Empleados.Adapters.EmpleadoDataAdapter(configuration));
            services.AddScoped<Empleados.Services.EmpleadoService>();

            // Desvinculaciones services
            services.AddScoped(sp => new DesvinculacionDataAdapter(configuration));
            services.AddScoped<DesvinculacionService>();
            
            return services;
        }
    }
}
