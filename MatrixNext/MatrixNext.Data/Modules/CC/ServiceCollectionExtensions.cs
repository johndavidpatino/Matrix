using MatrixNext.Data.Modules.CC.Adapters;
using MatrixNext.Data.Modules.CC.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MatrixNext.Data.Modules.CC
{
    /// <summary>
    /// CC FinzOpe Module DI Extension
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra los servicios del módulo de CC FinzOpe
        /// </summary>
        public static IServiceCollection AddCCModule(this IServiceCollection services, IConfiguration configuration)
        {
            var webMatrixConnection = configuration.GetConnectionString("WebMatrix") 
                ?? configuration.GetConnectionString("MatrixDb");
                
            if (string.IsNullOrWhiteSpace(webMatrixConnection))
                throw new InvalidOperationException("Connection string 'WebMatrix' or 'MatrixDb' is required for CC module");

            // CC FinzOpe Module (Sprint Pre-1)
            services.AddScoped(sp => new CcFinzOpeAdapter(webMatrixConnection));
            services.AddScoped<ICcFinzOpeAdapter>(sp => sp.GetRequiredService<CcFinzOpeAdapter>());
            services.AddScoped<ICcFinzOpeService, CcFinzOpeService>();
            
            // CC Control Presupuestos Module (Sprint 1)
            var dbConnection = new System.Data.SqlClient.SqlConnection(webMatrixConnection);
            services.AddScoped<CcControlPresupuestosAdapter>(sp => 
                new CcControlPresupuestosAdapter(dbConnection));
            services.AddScoped<ICcControlPresupuestosService, CcControlPresupuestosService>();
            
            // CC Presupuestos Internos Module (Sprint 2)
            services.AddScoped<CcPresupuestosInternosAdapter>(sp => 
                new CcPresupuestosInternosAdapter(dbConnection));
            services.AddScoped<ICcPresupuestosInternosService, CcPresupuestosInternosService>();
            
            // CC Procesos Internos Module (Sprint 3 - Fase 1)
            services.AddScoped<CcProcesosInternosAdapter>(sp => 
                new CcProcesosInternosAdapter(dbConnection));
            services.AddScoped<ICcProcesosInternosService, CcProcesosInternosService>();

            // CC Reportes Module (Sprint 4)
            services.AddScoped<CcReportesAdapter>(sp =>
                new CcReportesAdapter(dbConnection));
            services.AddScoped<ICcReportesService, CcReportesService>();

            // CC Producción Module (Sprint 5)
            services.AddScoped<CcProduccionAdapter>(sp =>
                new CcProduccionAdapter(dbConnection));
            services.AddScoped<ICcProduccionService, CcProduccionService>();
            
            return services;
        }
    }
}
