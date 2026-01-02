using MatrixNext.Data.Modules.TH.Ausencias.Adapters;
using MatrixNext.Data.Modules.TH.Ausencias.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MatrixNext.Data.Modules.TH
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra los servicios del módulo de Talento Humano (Ausencias slice inicial).
        /// </summary>
        public static IServiceCollection AddTHModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(sp => new AusenciaDataAdapter(configuration));
            services.AddScoped<AusenciaService>();
            return services;
        }
    }
}
