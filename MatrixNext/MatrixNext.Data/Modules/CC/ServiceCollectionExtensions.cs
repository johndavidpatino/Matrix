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

            services.AddScoped(sp => new CcFinzOpeAdapter(webMatrixConnection));
            services.AddScoped<ICcFinzOpeAdapter>(sp => sp.GetRequiredService<CcFinzOpeAdapter>());
            services.AddScoped<ICcFinzOpeService, CcFinzOpeService>();
            
            return services;
        }
    }
}
