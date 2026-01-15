/// <summary>
/// Adapter consolidado para gestión de productividad multi-roles
/// Ejecuta SPs con fallback a consultas directas
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

    public async Task<List<ProductividadPlanillaDto>> ObtenerPlanillasPorRolAsync(
        FiltrosProductividadDto filtros, 
        string rol, 
        long usuarioId)
    {
        try
        {
            // SP según rol: OP_CuantiDapper.CuantiProdProductividad_Get
            var query = @"
                SELECT 
                    p.IdPlanilla,
                    p.IdTrabajo,
                    t.NumeroTrabajo,
                    p.IdEmpleado,
                    CONCAT(e.Nombres, ' ', e.Apellidos) AS NombreEmpleado,
                    e.NumeroIdentificacion,
                    p.Fecha,
                    p.Cantidad,
                    p.MontoReportado,
                    p.MontoAutorizado,
                    p.TipoProductividad,
                    p.Estado,
                    p.Observaciones,
                    p.ObservacionesRechazo,
                    p.FechaRegistro,
                    p.RegistradoPor,
                    p.FechaAprobacion,
                    p.AprobadoPor,
                    p.Corte16_15 AS Corte16_15,
                    MONTH(p.Fecha) AS Mes,
                    YEAR(p.Fecha) AS Año,
                    CASE WHEN @Rol IN ('PMO', 'Coordinador') THEN 1 ELSE 0 END AS PuedeAprobar,
                    CASE WHEN @Rol IN ('PMO', 'Coordinador') THEN 1 ELSE 0 END AS PuedeRechazar,
                    CASE WHEN p.Estado = 'Pendiente' THEN 1 ELSE 0 END AS PuedeEditar
                FROM CuantiPlanillas p
                INNER JOIN PY_Trabajos t ON p.IdTrabajo = t.IdTrabajo
                INNER JOIN TH_Empleado e ON p.IdEmpleado = e.IdEmpleado
                WHERE 1=1
                    AND (@IdTrabajo IS NULL OR p.IdTrabajo = @IdTrabajo)
                    AND (@IdEmpleado IS NULL OR p.IdEmpleado = @IdEmpleado)
                    AND (@FechaInicio IS NULL OR p.Fecha >= @FechaInicio)
                    AND (@FechaFin IS NULL OR p.Fecha <= @FechaFin)
                    AND (@Corte IS NULL OR p.Corte16_15 = @Corte)
                    AND (@Mes IS NULL OR MONTH(p.Fecha) = @Mes)
                    AND (@Año IS NULL OR YEAR(p.Fecha) = @Año)
                    AND (@Estado IS NULL OR @Estado = 'Todos' OR p.Estado = @Estado)
                    AND (@TipoProductividad IS NULL OR p.TipoProductividad = @TipoProductividad)";

            // Filtro adicional por rol
            if (rol == "Coordinador")
            {
                query += @"
                    AND EXISTS (
                        SELECT 1 FROM PY_TrabajosPersonal tp
                        WHERE tp.IdTrabajo = p.IdTrabajo
                          AND tp.IdEmpleado = @UsuarioId
                          AND tp.Cargo = 'Coordinador'
                    )";
            }
            else if (rol == "Campo")
            {
                query += " AND p.IdEmpleado = @UsuarioId";
            }
            else if (rol == "MyS")
            {
                query += " AND p.TipoProductividad IN ('Supervisión', 'Llamadas')";
            }

            query += @"
                ORDER BY p.Fecha DESC, p.IdPlanilla DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var parameters = new
            {
                filtros.IdTrabajo,
                filtros.IdEmpleado,
                filtros.FechaInicio,
                filtros.FechaFin,
                filtros.Corte,
                filtros.Mes,
                filtros.Año,
                filtros.Estado,
                filtros.TipoProductividad,
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

    public async Task<ResumenProductividadDto> ObtenerResumenAsync(int año, int mes, int corte, long? idTrabajo = null)
    {
        try
        {
            var query = @"
                SELECT 
                    COUNT(*) AS TotalPlanillas,
                    SUM(CASE WHEN Estado = 'Pendiente' THEN 1 ELSE 0 END) AS PendientesAprobacion,
                    SUM(CASE WHEN Estado = 'Aprobado' THEN 1 ELSE 0 END) AS Aprobadas,
                    SUM(CASE WHEN Estado = 'Rechazado' THEN 1 ELSE 0 END) AS Rechazadas,
                    SUM(MontoReportado) AS TotalMontoReportado,
                    SUM(ISNULL(MontoAutorizado, 0)) AS TotalMontoAutorizado
                FROM CuantiPlanillas
                WHERE YEAR(Fecha) = @Año
                  AND MONTH(Fecha) = @Mes
                  AND Corte16_15 = @Corte
                  AND (@IdTrabajo IS NULL OR IdTrabajo = @IdTrabajo)";

            var resumen = await _connection.QuerySingleAsync<ResumenProductividadDto>(query, new { Año = año, Mes = mes, Corte = corte, IdTrabajo = idTrabajo });
            resumen.Corte = corte;
            resumen.Mes = mes;
            resumen.Año = año;

            _logger.LogInformation("Resumen productividad: {Total} planillas, {Pendientes} pendientes. Periodo: {Año}/{Mes} Corte {Corte}",
                resumen.TotalPlanillas, resumen.PendientesAprobacion, año, mes, corte);
            
            return resumen;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo resumen productividad. Año: {Año}, Mes: {Mes}, Corte: {Corte}", año, mes, corte);
            throw;
        }
    }

    public async Task<bool> AprobarPlanillaAsync(AprobacionPlanillaDto aprobacion)
    {
        try
        {
            // SP: CuantiPlanillasTrabajosUpdate
            var query = @"
                UPDATE CuantiPlanillas
                SET Estado = 'Aprobado',
                    MontoAutorizado = @MontoAutorizado,
                    Observaciones = ISNULL(@Observaciones, Observaciones),
                    FechaAprobacion = @FechaAprobacion,
                    AprobadoPor = @AprobadoPor
                WHERE IdPlanilla = @IdPlanilla
                  AND Estado = 'Pendiente'";

            var rowsAffected = await _connection.ExecuteAsync(query, new
            {
                aprobacion.IdPlanilla,
                aprobacion.MontoAutorizado,
                aprobacion.Observaciones,
                FechaAprobacion = DateTime.Now,
                aprobacion.AprobadoPor
            });

            if (rowsAffected > 0)
            {
                _logger.LogInformation("Planilla {Id} aprobada por usuario {UserId} con monto {Monto}", 
                    aprobacion.IdPlanilla, aprobacion.AprobadoPor, aprobacion.MontoAutorizado);
                return true;
            }

            _logger.LogWarning("Planilla {Id} no fue aprobada (posiblemente ya no está pendiente)", aprobacion.IdPlanilla);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error aprobando planilla {Id}", aprobacion.IdPlanilla);
            throw;
        }
    }

    public async Task<bool> RechazarPlanillaAsync(long idPlanilla, string observaciones, long usuarioId)
    {
        try
        {
            // SP: CuantiPlanillasTrabajosRemove
            var query = @"
                UPDATE CuantiPlanillas
                SET Estado = 'Rechazado',
                    ObservacionesRechazo = @Observaciones,
                    FechaAprobacion = @FechaRechazo,
                    AprobadoPor = @UsuarioId
                WHERE IdPlanilla = @IdPlanilla
                  AND Estado = 'Pendiente'";

            var rowsAffected = await _connection.ExecuteAsync(query, new
            {
                IdPlanilla = idPlanilla,
                Observaciones = observaciones,
                FechaRechazo = DateTime.Now,
                UsuarioId = usuarioId
            });

            if (rowsAffected > 0)
            {
                _logger.LogInformation("Planilla {Id} rechazada por usuario {UserId}. Observaciones: {Obs}", 
                    idPlanilla, usuarioId, observaciones);
                return true;
            }

            _logger.LogWarning("Planilla {Id} no fue rechazada (posiblemente ya no está pendiente)", idPlanilla);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rechazando planilla {Id}", idPlanilla);
            throw;
        }
    }

    public async Task<bool> TienePermisoAsync(long usuarioId, long idTrabajo, string accion, string rol)
    {
        try
        {
            // Verificar permisos según rol y acción
            // PMO (100): puede aprobar/rechazar todos los trabajos
            // Coordinador (135): puede aprobar/rechazar sus trabajos
            // Campo (156): solo puede ver sus propias planillas
            // MyS (157): puede ver supervisión y llamadas

            if (rol == "PMO")
            {
                // PMO siempre tiene permisos
                return true;
            }
            else if (rol == "Coordinador")
            {
                // Verificar si es coordinador del trabajo
                var query = @"
                    SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
                    FROM PY_TrabajosPersonal
                    WHERE IdTrabajo = @IdTrabajo
                      AND IdEmpleado = @UsuarioId
                      AND Cargo = 'Coordinador'";

                return await _connection.ExecuteScalarAsync<bool>(query, new { IdTrabajo = idTrabajo, UsuarioId = usuarioId });
            }
            else
            {
                // Campo y MyS: solo pueden ver, no aprobar/rechazar
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

    public async Task<(int Corte, int Mes, int Año)> CalcularCorte16_15Async(DateTime fecha)
    {
        try
        {
            int corte, mes, año;
            
            if (fecha.Day >= 1 && fecha.Day <= 15)
            {
                // Primera quincena
                corte = 1;
                mes = fecha.Month;
                año = fecha.Year;
            }
            else
            {
                // Segunda quincena
                corte = 2;
                mes = fecha.Month;
                año = fecha.Year;
            }

            _logger.LogInformation("Corte calculado: {Fecha} → {Año}/{Mes} Corte {Corte}", 
                fecha.ToString("yyyy-MM-dd"), año, mes, corte);
            
            return (corte, mes, año);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculando corte para fecha {Fecha}", fecha);
            return (0, 0, 0);
        }
    }

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
                FROM US_PermisosUsuario
                WHERE IdUsuario = @UsuarioId
                  AND IdPermiso IN (100, 135, 156, 157)";

            var permisos = await _connection.QuerySingleOrDefaultAsync<PermisosProductividadDto>(query, new { UsuarioId = usuarioId });
            
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

    public async Task<List<dynamic>> ObtenerTrabajosAsignadosAsync(long usuarioId, string rol)
    {
        try
        {
            string query;

            if (rol == "PMO")
            {
                // PMO ve todos los trabajos activos
                query = @"
                    SELECT IdTrabajo, NumeroTrabajo, NombreProyecto, Estado
                    FROM PY_Trabajos
                    WHERE Estado IN ('Activo', 'En Curso')
                    ORDER BY FechaInicio DESC";
            }
            else if (rol == "Coordinador")
            {
                // Coordinador ve sus trabajos asignados
                query = @"
                    SELECT t.IdTrabajo, t.NumeroTrabajo, t.NombreProyecto, t.Estado
                    FROM PY_Trabajos t
                    INNER JOIN PY_TrabajosPersonal tp ON t.IdTrabajo = tp.IdTrabajo
                    WHERE tp.IdEmpleado = @UsuarioId
                      AND tp.Cargo = 'Coordinador'
                      AND t.Estado IN ('Activo', 'En Curso')
                    ORDER BY t.FechaInicio DESC";
            }
            else
            {
                // Campo y MyS: solo trabajos donde tienen planillas
                query = @"
                    SELECT DISTINCT t.IdTrabajo, t.NumeroTrabajo, t.NombreProyecto, t.Estado
                    FROM PY_Trabajos t
                    INNER JOIN CuantiPlanillas p ON t.IdTrabajo = p.IdTrabajo
                    WHERE p.IdEmpleado = @UsuarioId
                      AND t.Estado IN ('Activo', 'En Curso')
                    ORDER BY t.FechaInicio DESC";
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
