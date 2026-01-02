using MatrixNext.Data.Modules.US.Usuarios.Adapters;
using MatrixNext.Data.Modules.US.Usuarios.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MatrixNext.Data.Modules.US
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra los servicios del módulo de Usuarios (US).
        /// </summary>
        public static IServiceCollection AddUSModule(this IServiceCollection services, IConfiguration configuration)
        {
            var matrixDb = configuration.GetConnectionString("MatrixDb");
            var matrixDatabase = configuration.GetConnectionString("MatrixDatabase");
            var connection = matrixDatabase ?? matrixDb;
            if (string.IsNullOrWhiteSpace(connection))
                throw new InvalidOperationException("Connection string 'MatrixDatabase' or 'MatrixDb' is required for US module");

            services.AddScoped(sp => new UsuarioAuthService(connection));
            services.AddScoped(sp => new UsuarioDataAdapter(connection));
            services.AddScoped<UsuarioService>();
            return services;
        }
    }
}
