/// <summary>
/// Adapter consolidado para gestión de productividad multi-roles
/// 
/// VALIDACIÓN BD 2025-01:
/// TABLA REAL: OP_CuantiPlanillas
/// Columnas: Id, TrabajoId(varchar), Per_NumIdentificacionEncu, Res_Ciudad, Res_Fecha,
///           Cantidad, TipoActividad, SubidoPor, FechaCarga, Revisado, RevisadoPor, FechaRevision
/// 
/// OTRA TABLA: OP_PersonasAsignadasTrabajo
/// Columnas: id, TrabajoId(bigint), Persona, Ciudad, Fecha
/// 
/// TABLAS INEXISTENTES que se intentaban usar:
/// - CuantiPlanillas (existe: OP_CuantiPlanillas con estructura diferente)
/// - PY_Trabajos (existe: PY_Trabajo)
/// - TH_Empleado (existe: TH_Personas)
/// - PY_TrabajosPersonal (existe: OP_PersonasAsignadasTrabajo con estructura diferente)
/// - US_PermisosUsuario (existe: US_PermisosUsuarios)
///
/// NOTA: La estructura original del adapter asumía columnas que NO existen:
/// IdPlanilla, IdEmpleado, MontoReportado, MontoAutorizado, TipoProductividad, Estado,
/// Observaciones, ObservacionesRechazo, FechaRegistro, RegistradoPor, FechaAprobacion,
/// AprobadoPor, Corte16_15
///
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.8
/// </summary>
namespace MatrixNext.Data.Adapters.OP;

using Dapper;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;
using System.Data;

public class ProductividadAdapter : IProductividadAdapter
{
    private readonly IDbConnection _connection;
    private readonly ILogger<ProductividadAdapter> _logger;

    public ProductividadAdapter(
        IDbConnection connection,
        ILogger<ProductividadAdapter> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene planillas de productividad por rol
    /// TABLA REAL: OP_CuantiPlanillas (estructura diferente a la esperada)
    /// </summary>
    public async Task<List<ProductividadPlanillaDto>> ObtenerPlanillasPorRolAsync(
        FiltrosProductividadDto filtros, 
        string rol, 
        long usuarioId)
    {
        try
        {
            // Query ajustado a estructura REAL de OP_CuantiPlanillas
            var query = @"
                SELECT 
                    p.Id AS IdPlanilla,
                    CAST(p.TrabajoId AS BIGINT) AS IdTrabajo,
                    p.TrabajoId AS NumeroTrabajo,
                    0 AS IdEmpleado,
                    p.Per_NumIdentificacionEncu AS NumeroIdentificacion,
                    per.nombres + ' ' + per.apellidos AS NombreEmpleado,
                    p.Res_Fecha AS Fecha,
                    p.Cantidad,
                    0 AS MontoReportado,
                    0 AS MontoAutorizado,
                    CAST(p.TipoActividad AS VARCHAR) AS TipoProductividad,
                    CASE WHEN p.Revisado = 1 THEN 'Revisado' ELSE 'Pendiente' END AS Estado,
                    '' AS Observaciones,
                    '' AS ObservacionesRechazo,
                    p.FechaCarga AS FechaRegistro,
                    p.SubidoPor AS RegistradoPor,
                    p.FechaRevision AS FechaAprobacion,
                    p.RevisadoPor AS AprobadoPor,
                    0 AS Corte16_15,
                    MONTH(p.Res_Fecha) AS Mes,
                    YEAR(p.Res_Fecha) AS Año,
                    p.Res_Ciudad AS Ciudad,
                    CASE WHEN @Rol IN ('PMO', 'Coordinador') THEN 1 ELSE 0 END AS PuedeAprobar,
                    CASE WHEN @Rol IN ('PMO', 'Coordinador') THEN 1 ELSE 0 END AS PuedeRechazar,
                    CASE WHEN p.Revisado = 0 THEN 1 ELSE 0 END AS PuedeEditar
                FROM OP_CuantiPlanillas p
                LEFT JOIN TH_Personas per ON p.Per_NumIdentificacionEncu = per.num_identificacion
                WHERE 1=1
                    AND (@IdTrabajo IS NULL OR p.TrabajoId = CAST(@IdTrabajo AS VARCHAR))
                    AND (@FechaInicio IS NULL OR p.Res_Fecha >= @FechaInicio)
                    AND (@FechaFin IS NULL OR p.Res_Fecha <= @FechaFin)
                    AND (@Mes IS NULL OR MONTH(p.Res_Fecha) = @Mes)
                    AND (@Año IS NULL OR YEAR(p.Res_Fecha) = @Año)
                    AND (@Estado IS NULL OR @Estado = 'Todos' 
                         OR (@Estado = 'Pendiente' AND p.Revisado = 0)
                         OR (@Estado = 'Revisado' AND p.Revisado = 1))";

            // Filtro adicional por rol usando OP_PersonasAsignadasTrabajo
            if (rol == "Coordinador")
            {
                query += @"
                    AND EXISTS (
                        SELECT 1 FROM OP_PersonasAsignadasTrabajo pat
                        WHERE pat.TrabajoId = CAST(p.TrabajoId AS BIGINT)
                          AND pat.Persona = @UsuarioId
                    )";
            }

            query += @"
                ORDER BY p.Res_Fecha DESC, p.Id DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var parameters = new
            {
                IdTrabajo = filtros.IdTrabajo.HasValue ? filtros.IdTrabajo.Value.ToString() : null,
                filtros.FechaInicio,
                filtros.FechaFin,
                filtros.Mes,
                filtros.Año,
                filtros.Estado,
                Rol = rol,
                UsuarioId = usuarioId,
                Offset = (filtros.PageNumber - 1) * filtros.PageSize,
                filtros.PageSize
            };

            var planillas = await _connection.QueryAsync<ProductividadPlanillaDto>(query, parameters);
            
            _logger.LogInformation("Obtenidas {Count} planillas para rol {Rol}, usuario {UserId}", 
                planillas.Count(), rol, usuarioId);
            
            return planillas.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo planillas. Rol: {Rol}, UsuarioId: {UserId}", rol, usuarioId);
            throw;
        }
    }

    /// <summary>
    /// Obtiene resumen de productividad
    /// TABLA REAL: OP_CuantiPlanillas (sin columnas de montos ni corte)
    /// </summary>
    public async Task<ResumenProductividadDto> ObtenerResumenAsync(int año, int mes, int corte, long? idTrabajo = null)
    {
        try
        {
            var query = @"
                SELECT 
                    COUNT(*) AS TotalPlanillas,
                    SUM(CASE WHEN Revisado = 0 THEN 1 ELSE 0 END) AS PendientesAprobacion,
                    SUM(CASE WHEN Revisado = 1 THEN 1 ELSE 0 END) AS Aprobadas,
                    0 AS Rechazadas,
                    CAST(SUM(Cantidad) AS DECIMAL(18,2)) AS TotalMontoReportado,
                    0 AS TotalMontoAutorizado
                FROM OP_CuantiPlanillas
                WHERE YEAR(Res_Fecha) = @Año
                  AND MONTH(Res_Fecha) = @Mes
                  AND (@IdTrabajo IS NULL OR TrabajoId = CAST(@IdTrabajo AS VARCHAR))";

            var resumen = await _connection.QueryFirstOrDefaultAsync<ResumenProductividadDto>(query, 
                new { Año = año, Mes = mes, IdTrabajo = idTrabajo });
            
            if (resumen == null)
            {
                resumen = new ResumenProductividadDto();
            }
            
            resumen.Corte = corte;
            resumen.Mes = mes;
            resumen.Año = año;

            _logger.LogInformation("Resumen productividad: {Total} planillas, {Pendientes} pendientes. Periodo: {Año}/{Mes}",
                resumen.TotalPlanillas, resumen.PendientesAprobacion, año, mes);
            
            return resumen;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo resumen productividad. Año: {Año}, Mes: {Mes}", año, mes);
            throw;
        }
    }

    /// <summary>
    /// Marca planilla como revisada
    /// TABLA REAL: OP_CuantiPlanillas (columna Revisado, RevisadoPor, FechaRevision)
    /// </summary>
    public async Task<bool> AprobarPlanillaAsync(AprobacionPlanillaDto aprobacion)
    {
        try
        {
            var query = @"
                UPDATE OP_CuantiPlanillas
                SET Revisado = 1,
                    RevisadoPor = @AprobadoPor,
                    FechaRevision = GETDATE()
                WHERE Id = @IdPlanilla
                  AND Revisado = 0";

            var rowsAffected = await _connection.ExecuteAsync(query, new
            {
                aprobacion.IdPlanilla,
                aprobacion.AprobadoPor
            });

            if (rowsAffected > 0)
            {
                _logger.LogInformation("Planilla {Id} marcada como revisada por usuario {UserId}", 
                    aprobacion.IdPlanilla, aprobacion.AprobadoPor);
                return true;
            }

            _logger.LogWarning("Planilla {Id} no fue actualizada (ya revisada o no existe)", aprobacion.IdPlanilla);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error aprobando planilla {Id}", aprobacion.IdPlanilla);
            throw;
        }
    }

    /// <summary>
    /// Rechazar planilla - NO IMPLEMENTADO
    /// NOTA: OP_CuantiPlanillas no tiene columna de estado rechazado
    /// </summary>
    public Task<bool> RechazarPlanillaAsync(long idPlanilla, string observaciones, long usuarioId)
    {
        _logger.LogWarning("RechazarPlanilla: OP_CuantiPlanillas no soporta estado 'Rechazado' - solo Revisado bit");
        throw new NotImplementedException(
            "La tabla OP_CuantiPlanillas no tiene columna para estados de rechazo. " +
            "Solo existe columna 'Revisado' (bit).");
    }

    /// <summary>
    /// Verifica permiso según rol
    /// NOTA: Simplificado porque PY_TrabajosPersonal no existe con estructura esperada
    /// </summary>
    public async Task<bool> TienePermisoAsync(long usuarioId, long idTrabajo, string accion, string rol)
    {
        try
        {
            if (rol == "PMO")
            {
                return true;
            }
            else if (rol == "Coordinador")
            {
                // Verificar usando OP_PersonasAsignadasTrabajo
                var query = @"
                    SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
                    FROM OP_PersonasAsignadasTrabajo
                    WHERE TrabajoId = @IdTrabajo
                      AND Persona = @UsuarioId";

                return await _connection.ExecuteScalarAsync<bool>(query, 
                    new { IdTrabajo = idTrabajo, UsuarioId = usuarioId });
            }
            else
            {
                return accion == "Ver";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando permiso. UsuarioId: {UserId}, Trabajo: {Trabajo}, Accion: {Accion}", 
                usuarioId, idTrabajo, accion);
            return false;
        }
    }

    /// <summary>
    /// Calcula periodo de corte 16-15 (lógica de negocio)
    /// </summary>
    public Task<(int Corte, int Mes, int Año)> CalcularCorte16_15Async(DateTime fecha)
    {
        int corte, mes, año;
        
        if (fecha.Day >= 1 && fecha.Day <= 15)
        {
            corte = 1;
            mes = fecha.Month;
            año = fecha.Year;
        }
        else
        {
            corte = 2;
            mes = fecha.Month;
            año = fecha.Year;
        }

        _logger.LogInformation("Corte calculado: {Fecha} → {Año}/{Mes} Corte {Corte}", 
            fecha.ToString("yyyy-MM-dd"), año, mes, corte);
        
        return Task.FromResult((corte, mes, año));
    }

    /// <summary>
    /// Obtiene permisos de usuario
    /// TABLA: US_PermisosUsuarios (corregido de US_PermisosUsuario)
    /// </summary>
    public async Task<PermisosProductividadDto> ObtenerPermisosUsuarioAsync(long usuarioId)
    {
        try
        {
            var query = @"
                SELECT 
                    MAX(CASE WHEN IdPermiso = 100 THEN 1 ELSE 0 END) AS PuedeVerPMO,
                    MAX(CASE WHEN IdPermiso = 135 THEN 1 ELSE 0 END) AS PuedeVerCoordinador,
                    MAX(CASE WHEN IdPermiso = 156 THEN 1 ELSE 0 END) AS PuedeVerCampo,
                    MAX(CASE WHEN IdPermiso = 157 THEN 1 ELSE 0 END) AS PuedeVerMyS,
                    MAX(CASE WHEN IdPermiso IN (100, 135) THEN 1 ELSE 0 END) AS PuedeAprobar,
                    MAX(CASE WHEN IdPermiso IN (100, 135) THEN 1 ELSE 0 END) AS PuedeRechazar,
                    MAX(CASE WHEN IdPermiso IN (100, 135, 156) THEN 1 ELSE 0 END) AS PuedeEditar
                FROM US_PermisosUsuarios
                WHERE IdUsuario = @UsuarioId
                  AND IdPermiso IN (100, 135, 156, 157)";

            var permisos = await _connection.QueryFirstOrDefaultAsync<PermisosProductividadDto>(query, 
                new { UsuarioId = usuarioId });
            
            if (permisos == null)
            {
                _logger.LogWarning("Usuario {UserId} no tiene permisos de productividad asignados", usuarioId);
                permisos = new PermisosProductividadDto();
            }

            // Determinar rol actual (prioridad: PMO > Coordinador > Campo > MyS)
            if (permisos.PuedeVerPMO) permisos.RolActual = "PMO";
            else if (permisos.PuedeVerCoordinador) permisos.RolActual = "Coordinador";
            else if (permisos.PuedeVerCampo) permisos.RolActual = "Campo";
            else if (permisos.PuedeVerMyS) permisos.RolActual = "MyS";
            else permisos.RolActual = "Sin permisos";

            _logger.LogInformation("Permisos usuario {UserId}: Rol={Rol}, Aprobar={Aprobar}", 
                usuarioId, permisos.RolActual, permisos.PuedeAprobar);
            
            return permisos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo permisos para usuario {UserId}", usuarioId);
            return new PermisosProductividadDto { RolActual = "Error" };
        }
    }

    /// <summary>
    /// Obtiene trabajos asignados al usuario
    /// TABLAS: PY_Trabajo (corregido de PY_Trabajos), OP_PersonasAsignadasTrabajo
    /// </summary>
    public async Task<List<dynamic>> ObtenerTrabajosAsignadosAsync(long usuarioId, string rol)
    {
        try
        {
            string query;

            if (rol == "PMO")
            {
                // PMO ve todos los trabajos activos
                query = @"
                    SELECT id AS IdTrabajo, CAST(id AS VARCHAR) AS NumeroTrabajo, 
                           NombreTrabajo AS NombreProyecto, '' AS Estado
                    FROM PY_Trabajo
                    ORDER BY id DESC";
            }
            else if (rol == "Coordinador")
            {
                // Coordinador ve sus trabajos asignados
                query = @"
                    SELECT t.id AS IdTrabajo, CAST(t.id AS VARCHAR) AS NumeroTrabajo, 
                           t.NombreTrabajo AS NombreProyecto, '' AS Estado
                    FROM PY_Trabajo t
                    INNER JOIN OP_PersonasAsignadasTrabajo pat ON t.id = pat.TrabajoId
                    WHERE pat.Persona = @UsuarioId
                    ORDER BY t.id DESC";
            }
            else
            {
                // Campo y MyS: trabajos donde tienen planillas
                query = @"
                    SELECT DISTINCT CAST(p.TrabajoId AS BIGINT) AS IdTrabajo, 
                           p.TrabajoId AS NumeroTrabajo, 
                           t.NombreTrabajo AS NombreProyecto, '' AS Estado
                    FROM OP_CuantiPlanillas p
                    LEFT JOIN PY_Trabajo t ON CAST(p.TrabajoId AS BIGINT) = t.id
                    WHERE p.SubidoPor = @UsuarioId
                    ORDER BY IdTrabajo DESC";
            }

            var trabajos = await _connection.QueryAsync<dynamic>(query, new { UsuarioId = usuarioId });
            
            _logger.LogInformation("Obtenidos {Count} trabajos asignados para usuario {UserId} con rol {Rol}", 
                trabajos.Count(), usuarioId, rol);
            
            return trabajos.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajos asignados. UsuarioId: {UserId}, Rol: {Rol}", usuarioId, rol);
            return new List<dynamic>();
        }
    }
}
