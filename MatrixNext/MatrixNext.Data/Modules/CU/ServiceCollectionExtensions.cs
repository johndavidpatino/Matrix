using MatrixNext.Data.Adapters.CU;
using MatrixNext.Data.Services.CU;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MatrixNext.Data.Modules.CU
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra servicios del modulo CU_Cuentas.
        /// </summary>
        public static IServiceCollection AddCUModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(sp => new CuentaDataAdapter(configuration));
            services.AddScoped<CuentaService>();
            services.AddScoped(sp => new PropuestaDataAdapter(configuration));
            services.AddScoped<PropuestaService>();
            services.AddScoped(sp => new EstudioDataAdapter(configuration));
            services.AddScoped<EstudioService>();
            services.AddScoped(sp => new BriefDataAdapter(configuration));
            services.AddScoped<BriefService>();

            return services;
        }
    }
}
