using MatrixNext.Data.Adapters.CU;
using MatrixNext.Data.Entities;
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
            // Adapters
            services.AddScoped(sp => new CuentaDataAdapter(configuration));
            services.AddScoped(sp => new PropuestaDataAdapter(configuration));
            services.AddScoped(sp => new EstudioDataAdapter(configuration));
            services.AddScoped(sp => new BriefDataAdapter(configuration));
            services.AddScoped(sp => new PresupuestoDataAdapter(configuration));

            // DbContext para IQuoteCalculatorService
            services.AddScoped(sp => 
            {
                var connString = configuration.GetConnectionString("MatrixDb");
                return new MatrixDbContext(connString!);
            });

            // Services
            services.AddScoped<CuentaService>();
            services.AddScoped<PropuestaService>();
            services.AddScoped<EstudioService>();
            services.AddScoped<BriefService>();
            services.AddScoped<PresupuestoService>();
            
            // Fase 2 - Presupuesto extendido
            services.AddScoped<IQuoteCalculatorService>();
            services.AddScoped<PresupuestoServiceExtended>();

            return services;
        }
    }
}
