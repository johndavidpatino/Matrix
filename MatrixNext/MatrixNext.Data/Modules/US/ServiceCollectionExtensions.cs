using MatrixNext.Data.Modules.US.Feedback.Adapters;
using MatrixNext.Data.Modules.US.Feedback.Services;
using MatrixNext.Data.Modules.US.GruposPermisos.Adapters;
using MatrixNext.Data.Modules.US.GruposPermisos.Services;
using MatrixNext.Data.Modules.US.RolesPermisos.Adapters;
using MatrixNext.Data.Modules.US.RolesPermisos.Services;
using MatrixNext.Data.Modules.US.TipoGrupoUnidad.Adapters;
using MatrixNext.Data.Modules.US.TipoGrupoUnidad.Services;
using MatrixNext.Data.Modules.US.Unidades.Adapters;
using MatrixNext.Data.Modules.US.Unidades.Services;
using MatrixNext.Data.Modules.US.Usuarios.Adapters;
using MatrixNext.Data.Modules.US.Usuarios.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

            // Usuarios
            services.AddScoped(sp => new UsuarioAuthService(connection));
            services.AddScoped(sp => new UsuarioDataAdapter(connection));
            services.AddScoped<UsuarioService>();

            // Grupos de Permisos
            services.AddScoped<IGrupoPermisoAdapter>(sp =>
                new GrupoPermisoAdapter(connection, sp.GetRequiredService<ILogger<GrupoPermisoAdapter>>()));
            services.AddScoped<IGrupoPermisoService, GrupoPermisoService>();

            // Tipo de Grupo de Unidad
            services.AddScoped<ITipoGrupoUnidadAdapter>(sp =>
                new TipoGrupoUnidadAdapter(connection, sp.GetRequiredService<ILogger<TipoGrupoUnidadAdapter>>()));
            services.AddScoped<ITipoGrupoUnidadService, TipoGrupoUnidadService>();

            // Unidades
            services.AddScoped<IUnidadAdapter>(sp =>
                new UnidadAdapter(connection, sp.GetRequiredService<ILogger<UnidadAdapter>>()));
            services.AddScoped<IUnidadService, UnidadService>();

            // Feedback
            services.AddScoped<IFeedbackAdapter>(sp =>
                new FeedbackAdapter(connection, sp.GetRequiredService<ILogger<FeedbackAdapter>>()));
            services.AddScoped<IFeedbackService, FeedbackService>();

            // RolesPermisos
            services.AddScoped<IRolPermisoAdapter>(sp =>
                new RolPermisoAdapter(connection, sp.GetRequiredService<ILogger<RolPermisoAdapter>>()));
            services.AddScoped<IRolPermisoService, RolPermisoService>();

            return services;
        }
    }
}
