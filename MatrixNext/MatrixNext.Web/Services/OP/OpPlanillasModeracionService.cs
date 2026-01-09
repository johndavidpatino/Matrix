using System.Data;
using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.OP.Models;
using MatrixNext.Web.Services.Shared;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Implementación del servicio de planillas de moderación e informes
/// Ref: AdministracionRegistroPlanillas.aspx.vb + RegistroPlanillasCualitativo.aspx.vb
/// </summary>
public class OpPlanillasModeracionService : IOpPlanillasModeracionService
{
    private readonly MatrixDbContext _context;
    private readonly IDbConnection _connection;
    private readonly IExportService _exportService;
    private readonly ILogger<OpPlanillasModeracionService> _logger;

    public OpPlanillasModeracionService(
        MatrixDbContext context,
        IDbConnection connection,
        IExportService exportService,
        ILogger<OpPlanillasModeracionService> logger)
    {
        _context = context;
        _connection = connection;
        _exportService = exportService;
        _logger = logger;
    }

    public async Task<(bool success, List<PlanillaListItemVm> data, int totalRecords, string error)> ObtenerPlanillasAsync(
        string? tipoPlantilla,
        short? idEstado,
        int pageIndex = 0,
        int pageSize = 25)
    {
        try
        {
            // Query base para ambos tipos de planillas
            var sql = @"
                SELECT 
                    IdPlanilla,
                    TipoPlantilla = @TipoPlantilla,
                    IdJob,
                    JobDesc,
                    Fecha,
                    Tecnica = ISNULL(NombreTecnica, Tecnica),
                    Muestra,
                    ResponsableNombre = CASE 
                        WHEN @TipoPlantilla = 'Moderacion' THEN NombreModerador 
                        ELSE Analista 
                    END,
                    IdEstadoAprobacion,
                    EstadoAprobacion = CASE IdEstadoAprobacion
                        WHEN 1 THEN 'En Espera'
                        WHEN 2 THEN 'Aprobado'
                        WHEN 3 THEN 'No Aprobado'
                        ELSE 'Desconocido'
                    END,
                    Observaciones,
                    FechaCreacion,
                    FechaModificacion
                FROM (
                    -- Planillas de Moderación
                    SELECT 
                        pm.IdPlanilla,
                        pm.IdJob,
                        pm.JobDesc,
                        pm.FechaPlanilla AS Fecha,
                        t.NombreTecnica,
                        NULL AS Tecnica,
                        pm.Muestra,
                        u.Nombre AS NombreModerador,
                        NULL AS Analista,
                        pm.IdEstadoAprobacion,
                        pm.Observaciones,
                        pm.FechaCreacion,
                        pm.FechaModificacion
                    FROM PY_PlanillaModeracion pm
                    LEFT JOIN PY_TecnicasCualitativas t ON pm.IdTecnica = t.IdTecnica
                    LEFT JOIN US_Usuarios u ON pm.IdModerador = u.Id
                    WHERE (@TipoPlantilla IS NULL OR @TipoPlantilla = 'Moderacion')
                      AND (@IdEstado IS NULL OR pm.IdEstadoAprobacion = @IdEstado)
                    
                    UNION ALL
                    
                    -- Planillas de Informes
                    SELECT 
                        pi.IdPlanilla,
                        pi.IdJob,
                        pi.JobDesc,
                        pi.Fecha,
                        NULL AS NombreTecnica,
                        pi.Tecnica,
                        pi.Muestra,
                        NULL AS NombreModerador,
                        pi.Analista,
                        pi.IdEstadoAprobacion,
                        pi.Observaciones,
                        pi.FechaCreacion,
                        pi.FechaModificacion
                    FROM PY_PlanillaInformes pi
                    WHERE (@TipoPlantilla IS NULL OR @TipoPlantilla = 'Informes')
                      AND (@IdEstado IS NULL OR pi.IdEstadoAprobacion = @IdEstado)
                ) AS Planillas
                ORDER BY FechaCreacion DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var countSql = @"
                SELECT COUNT(*)
                FROM (
                    SELECT IdPlanilla FROM PY_PlanillaModeracion
                    WHERE (@TipoPlantilla IS NULL OR @TipoPlantilla = 'Moderacion')
                      AND (@IdEstado IS NULL OR IdEstadoAprobacion = @IdEstado)
                    UNION ALL
                    SELECT IdPlanilla FROM PY_PlanillaInformes
                    WHERE (@TipoPlantilla IS NULL OR @TipoPlantilla = 'Informes')
                      AND (@IdEstado IS NULL OR IdEstadoAprobacion = @IdEstado)
                ) AS Total";

            var parameters = new
            {
                TipoPlantilla = tipoPlantilla,
                IdEstado = idEstado,
                Offset = pageIndex * pageSize,
                PageSize = pageSize
            };

            var data = (await _connection.QueryAsync<PlanillaListItemVm>(sql, parameters)).AsList();
            var totalRecords = await _connection.ExecuteScalarAsync<int>(countSql, parameters);

            // Asignar TipoPlantilla correcto para cada registro
            foreach (var item in data)
            {
                item.TipoPlantilla = item.ResponsableNombre.Contains("@") ? "Informes" : "Moderacion";
            }

            return (true, data, totalRecords, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener planillas. Tipo: {TipoPlantilla}, Estado: {IdEstado}", tipoPlantilla, idEstado);
            return (false, new List<PlanillaListItemVm>(), 0, "Error al obtener planillas");
        }
    }

    public async Task<(bool success, PlanillaModeracionVm? data, string error)> ObtenerPlanillaModeracionAsync(long idPlanilla)
    {
        try
        {
            var sql = @"
                SELECT 
                    pm.IdPlanilla,
                    pm.IdJob,
                    pm.JobDesc,
                    pm.FechaPlanilla,
                    pm.IdTecnica,
                    t.NombreTecnica,
                    pm.Muestra,
                    pm.IdModerador,
                    u.Nombre AS NombreModerador,
                    pm.Observaciones,
                    pm.IdEstadoAprobacion,
                    CASE pm.IdEstadoAprobacion
                        WHEN 1 THEN 'En Espera'
                        WHEN 2 THEN 'Aprobado'
                        WHEN 3 THEN 'No Aprobado'
                    END AS EstadoAprobacion,
                    pm.FechaCreacion,
                    pm.FechaModificacion,
                    pm.UsuarioCreacion,
                    pm.UsuarioModificacion
                FROM PY_PlanillaModeracion pm
                LEFT JOIN PY_TecnicasCualitativas t ON pm.IdTecnica = t.IdTecnica
                LEFT JOIN US_Usuarios u ON pm.IdModerador = u.Id
                WHERE pm.IdPlanilla = @IdPlanilla";

            var data = await _connection.QueryFirstOrDefaultAsync<PlanillaModeracionVm>(sql, new { IdPlanilla = idPlanilla });

            if (data == null)
            {
                return (false, null, "Planilla de moderación no encontrada");
            }

            return (true, data, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener planilla de moderación ID: {IdPlanilla}", idPlanilla);
            return (false, null, "Error al obtener planilla de moderación");
        }
    }

    public async Task<(bool success, PlanillaInformeVm? data, string error)> ObtenerPlanillaInformeAsync(long idPlanilla)
    {
        try
        {
            var sql = @"
                SELECT 
                    pi.IdPlanilla,
                    pi.IdJob,
                    pi.JobDesc,
                    pi.Fecha,
                    pi.Tecnica,
                    pi.Muestra,
                    pi.IdCuentasUU,
                    pi.Analista,
                    pi.ServiceLineName,
                    pi.Observaciones,
                    pi.IdEstadoAprobacion,
                    CASE pi.IdEstadoAprobacion
                        WHEN 1 THEN 'En Espera'
                        WHEN 2 THEN 'Aprobado'
                        WHEN 3 THEN 'No Aprobado'
                    END AS EstadoAprobacion,
                    pi.FechaCreacion,
                    pi.FechaModificacion,
                    pi.UsuarioCreacion,
                    pi.UsuarioModificacion
                FROM PY_PlanillaInformes pi
                WHERE pi.IdPlanilla = @IdPlanilla";

            var data = await _connection.QueryFirstOrDefaultAsync<PlanillaInformeVm>(sql, new { IdPlanilla = idPlanilla });

            if (data == null)
            {
                return (false, null, "Planilla de informes no encontrada");
            }

            return (true, data, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener planilla de informes ID: {IdPlanilla}", idPlanilla);
            return (false, null, "Error al obtener planilla de informes");
        }
    }

    public async Task<(bool success, long idPlanilla, string error)> GuardarPlanillaModeracionAsync(
        PlanillaModeracionVm model,
        long usuarioId)
    {
        try
        {
            // Validaciones
            if (model.IdJob == null || model.IdJob == 0)
            {
                return (false, 0, "JobBook es requerido");
            }

            if (model.FechaPlanilla == null)
            {
                return (false, 0, "Fecha de planilla es requerida");
            }

            if (model.IdTecnica == null)
            {
                return (false, 0, "Técnica es requerida");
            }

            if (model.IdModerador == null)
            {
                return (false, 0, "Moderador es requerido");
            }

            var fechaActual = DateTime.UtcNow.AddHours(-5); // Colombia UTC-5

            if (model.IdPlanilla == 0) // INSERT
            {
                var insertSql = @"
                    INSERT INTO PY_PlanillaModeracion (
                        IdJob, JobDesc, FechaPlanilla, IdTecnica, Muestra, 
                        IdModerador, Observaciones, IdEstadoAprobacion,
                        FechaCreacion, UsuarioCreacion
                    )
                    VALUES (
                        @IdJob, @JobDesc, @FechaPlanilla, @IdTecnica, @Muestra,
                        @IdModerador, @Observaciones, 1,
                        @FechaCreacion, @UsuarioCreacion
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                var idPlanilla = await _connection.ExecuteScalarAsync<long>(insertSql, new
                {
                    model.IdJob,
                    model.JobDesc,
                    model.FechaPlanilla,
                    model.IdTecnica,
                    model.Muestra,
                    model.IdModerador,
                    model.Observaciones,
                    FechaCreacion = fechaActual,
                    UsuarioCreacion = usuarioId
                });

                _logger.LogInformation("Planilla de moderación creada. ID: {IdPlanilla}, JobBook: {IdJob}, Usuario: {UsuarioId}",
                    idPlanilla, model.IdJob, usuarioId);

                return (true, idPlanilla, string.Empty);
            }
            else // UPDATE
            {
                var updateSql = @"
                    UPDATE PY_PlanillaModeracion
                    SET IdJob = @IdJob,
                        JobDesc = @JobDesc,
                        FechaPlanilla = @FechaPlanilla,
                        IdTecnica = @IdTecnica,
                        Muestra = @Muestra,
                        IdModerador = @IdModerador,
                        Observaciones = @Observaciones,
                        FechaModificacion = @FechaModificacion,
                        UsuarioModificacion = @UsuarioModificacion
                    WHERE IdPlanilla = @IdPlanilla";

                await _connection.ExecuteAsync(updateSql, new
                {
                    model.IdPlanilla,
                    model.IdJob,
                    model.JobDesc,
                    model.FechaPlanilla,
                    model.IdTecnica,
                    model.Muestra,
                    model.IdModerador,
                    model.Observaciones,
                    FechaModificacion = fechaActual,
                    UsuarioModificacion = usuarioId
                });

                _logger.LogInformation("Planilla de moderación actualizada. ID: {IdPlanilla}, Usuario: {UsuarioId}",
                    model.IdPlanilla, usuarioId);

                return (true, model.IdPlanilla, string.Empty);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar planilla de moderación. ID: {IdPlanilla}, Usuario: {UsuarioId}",
                model.IdPlanilla, usuarioId);
            return (false, 0, "Error al guardar planilla de moderación");
        }
    }

    public async Task<(bool success, long idPlanilla, string error)> GuardarPlanillaInformeAsync(
        PlanillaInformeVm model,
        long usuarioId)
    {
        try
        {
            // Validaciones
            if (model.IdJob == null || model.IdJob == 0)
            {
                return (false, 0, "JobBook es requerido");
            }

            if (model.Fecha == null)
            {
                return (false, 0, "Fecha es requerida");
            }

            if (string.IsNullOrWhiteSpace(model.Tecnica))
            {
                return (false, 0, "Técnica es requerida");
            }

            if (string.IsNullOrWhiteSpace(model.Analista))
            {
                return (false, 0, "Analista es requerido");
            }

            var fechaActual = DateTime.UtcNow.AddHours(-5); // Colombia UTC-5

            if (model.IdPlanilla == 0) // INSERT
            {
                var insertSql = @"
                    INSERT INTO PY_PlanillaInformes (
                        IdJob, JobDesc, Fecha, Tecnica, Muestra,
                        IdCuentasUU, Analista, ServiceLineName, Observaciones, IdEstadoAprobacion,
                        FechaCreacion, UsuarioCreacion
                    )
                    VALUES (
                        @IdJob, @JobDesc, @Fecha, @Tecnica, @Muestra,
                        @IdCuentasUU, @Analista, @ServiceLineName, @Observaciones, 1,
                        @FechaCreacion, @UsuarioCreacion
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                var idPlanilla = await _connection.ExecuteScalarAsync<long>(insertSql, new
                {
                    model.IdJob,
                    model.JobDesc,
                    model.Fecha,
                    model.Tecnica,
                    model.Muestra,
                    model.IdCuentasUU,
                    model.Analista,
                    model.ServiceLineName,
                    model.Observaciones,
                    FechaCreacion = fechaActual,
                    UsuarioCreacion = usuarioId
                });

                _logger.LogInformation("Planilla de informes creada. ID: {IdPlanilla}, JobBook: {IdJob}, Usuario: {UsuarioId}",
                    idPlanilla, model.IdJob, usuarioId);

                return (true, idPlanilla, string.Empty);
            }
            else // UPDATE
            {
                var updateSql = @"
                    UPDATE PY_PlanillaInformes
                    SET IdJob = @IdJob,
                        JobDesc = @JobDesc,
                        Fecha = @Fecha,
                        Tecnica = @Tecnica,
                        Muestra = @Muestra,
                        IdCuentasUU = @IdCuentasUU,
                        Analista = @Analista,
                        ServiceLineName = @ServiceLineName,
                        Observaciones = @Observaciones,
                        FechaModificacion = @FechaModificacion,
                        UsuarioModificacion = @UsuarioModificacion
                    WHERE IdPlanilla = @IdPlanilla";

                await _connection.ExecuteAsync(updateSql, new
                {
                    model.IdPlanilla,
                    model.IdJob,
                    model.JobDesc,
                    model.Fecha,
                    model.Tecnica,
                    model.Muestra,
                    model.IdCuentasUU,
                    model.Analista,
                    model.ServiceLineName,
                    model.Observaciones,
                    FechaModificacion = fechaActual,
                    UsuarioModificacion = usuarioId
                });

                _logger.LogInformation("Planilla de informes actualizada. ID: {IdPlanilla}, Usuario: {UsuarioId}",
                    model.IdPlanilla, usuarioId);

                return (true, model.IdPlanilla, string.Empty);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar planilla de informes. ID: {IdPlanilla}, Usuario: {UsuarioId}",
                model.IdPlanilla, usuarioId);
            return (false, 0, "Error al guardar planilla de informes");
        }
    }

    public async Task<(bool success, string error)> AprobarPlanillaAsync(
        long idPlanilla,
        string tipoPlantilla,
        long usuarioId,
        string? observaciones = null)
    {
        try
        {
            var tabla = tipoPlantilla == "Moderacion" ? "PY_PlanillaModeracion" : "PY_PlanillaInformes";
            var fechaActual = DateTime.UtcNow.AddHours(-5);

            var sql = $@"
                UPDATE {tabla}
                SET IdEstadoAprobacion = 2,
                    Observaciones = ISNULL(@Observaciones, Observaciones),
                    FechaModificacion = @FechaModificacion,
                    UsuarioModificacion = @UsuarioModificacion
                WHERE IdPlanilla = @IdPlanilla
                  AND IdEstadoAprobacion = 1"; // Solo aprobar si está en espera

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                IdPlanilla = idPlanilla,
                Observaciones = observaciones,
                FechaModificacion = fechaActual,
                UsuarioModificacion = usuarioId
            });

            if (rowsAffected == 0)
            {
                return (false, "Planilla no encontrada o ya procesada");
            }

            _logger.LogInformation("Planilla aprobada. Tipo: {TipoPlantilla}, ID: {IdPlanilla}, Usuario: {UsuarioId}",
                tipoPlantilla, idPlanilla, usuarioId);

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al aprobar planilla. Tipo: {TipoPlantilla}, ID: {IdPlanilla}",
                tipoPlantilla, idPlanilla);
            return (false, "Error al aprobar planilla");
        }
    }

    public async Task<(bool success, string error)> RechazarPlanillaAsync(
        long idPlanilla,
        string tipoPlantilla,
        long usuarioId,
        string observaciones)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(observaciones))
            {
                return (false, "Observaciones son requeridas para rechazar");
            }

            var tabla = tipoPlantilla == "Moderacion" ? "PY_PlanillaModeracion" : "PY_PlanillaInformes";
            var fechaActual = DateTime.UtcNow.AddHours(-5);

            var sql = $@"
                UPDATE {tabla}
                SET IdEstadoAprobacion = 3,
                    Observaciones = @Observaciones,
                    FechaModificacion = @FechaModificacion,
                    UsuarioModificacion = @UsuarioModificacion
                WHERE IdPlanilla = @IdPlanilla
                  AND IdEstadoAprobacion = 1"; // Solo rechazar si está en espera

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                IdPlanilla = idPlanilla,
                Observaciones = observaciones,
                FechaModificacion = fechaActual,
                UsuarioModificacion = usuarioId
            });

            if (rowsAffected == 0)
            {
                return (false, "Planilla no encontrada o ya procesada");
            }

            _logger.LogInformation("Planilla rechazada. Tipo: {TipoPlantilla}, ID: {IdPlanilla}, Usuario: {UsuarioId}",
                tipoPlantilla, idPlanilla, usuarioId);

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al rechazar planilla. Tipo: {TipoPlantilla}, ID: {IdPlanilla}",
                tipoPlantilla, idPlanilla);
            return (false, "Error al rechazar planilla");
        }
    }

    public async Task<byte[]> ExportarPlanillasExcelAsync(string? tipoPlantilla, short? idEstado)
    {
        var (success, data, _, _) = await ObtenerPlanillasAsync(tipoPlantilla, idEstado, 0, 10000);

        if (!success || !data.Any())
        {
            return Array.Empty<byte>();
        }

        var nombreArchivo = $"Planillas_{tipoPlantilla ?? "Todas"}_{DateTime.Now:yyyyMMdd}";
        return await _exportService.ExportarExcelAsync(data, nombreArchivo, "Planillas", "Reporte de Planillas");
    }

    public async Task<List<JobBookSearchVm>> BuscarJobBooksAsync(string termino)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(termino) || termino.Length < 2)
            {
                return new List<JobBookSearchVm>();
            }

            // Query contra tabla JobBook en CoreProject (ajustar según estructura real)
            var sql = @"
                SELECT TOP 20
                    IdJob,
                    JobDesc,
                    Cliente = ISNULL(NombreCliente, ''),
                    FechaInicio
                FROM PY_Trabajo
                WHERE (JobDesc LIKE @Termino OR CAST(IdJob AS VARCHAR) LIKE @Termino)
                  AND Activo = 1
                ORDER BY IdJob DESC";

            var results = await _connection.QueryAsync<JobBookSearchVm>(sql, new
            {
                Termino = $"%{termino}%"
            });

            return results.AsList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar JobBooks. Término: {Termino}", termino);
            return new List<JobBookSearchVm>();
        }
    }

    public async Task<List<ModeradorVm>> ObtenerModeradoresAsync()
    {
        try
        {
            // Query contra US_Usuarios filtrando por rol o permiso específico
            var sql = @"
                SELECT 
                    IdModerador = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Activo = CAST(1 AS BIT)
                FROM US_Usuarios u
                WHERE u.Activo = 1
                  AND u.Email IS NOT NULL
                  AND EXISTS (
                      SELECT 1 FROM US_UsuariosPermisos up
                      WHERE up.IdUsuario = u.Id
                        AND up.IdPermiso IN (42, 148) -- Permisos para moderadores
                  )
                ORDER BY u.Nombre";

            var results = await _connection.QueryAsync<ModeradorVm>(sql);
            return results.AsList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener moderadores");
            return new List<ModeradorVm>();
        }
    }

    public async Task<List<TecnicaVm>> ObtenerTecnicasAsync(string? tipoTecnica = null)
    {
        try
        {
            var sql = @"
                SELECT 
                    IdTecnica,
                    NombreTecnica,
                    TipoTecnica,
                    Activo = CAST(1 AS BIT)
                FROM PY_TecnicasCualitativas
                WHERE Activo = 1
                  AND (@TipoTecnica IS NULL OR TipoTecnica = @TipoTecnica)
                ORDER BY NombreTecnica";

            var results = await _connection.QueryAsync<TecnicaVm>(sql, new { TipoTecnica = tipoTecnica });
            return results.AsList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener técnicas. Tipo: {TipoTecnica}", tipoTecnica);
            return new List<TecnicaVm>();
        }
    }
}
