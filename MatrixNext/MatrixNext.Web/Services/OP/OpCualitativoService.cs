using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.OP.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Implementación del servicio de trabajos cualitativos
/// Ref: ANALISIS_OP_CUALITATIVO_FASE5_MAPEO_BD_RIESGOS.md § 4.2
/// Strategy: Hybrid EF Core (80%) + Dapper (20%) para SPs complejos
/// </summary>
public class OpCualitativoService : IOpCualitativoService
{
    private readonly MatrixDbContext _context;
    private readonly string _connectionString;
    private readonly ILogger<OpCualitativoService> _logger;

    public OpCualitativoService(
        MatrixDbContext context,
        IConfiguration configuration,
        ILogger<OpCualitativoService> logger)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("MatrixDb")!;
        _logger = logger;
    }

    public async Task<(bool Success, List<TrabajoCualitativoVm> Data, string Error)> ObtenerTrabajosPorCoordinadorAsync(
        long usuarioId, long? coeId = null)
    {
        try
        {
            // Ref: Trabajos.aspx.vb l??neas 21-47 (CargarTrabajos con coordinador)
            // REGLA 2: Consultar CoreProject ??' usa CoordinacionCampo.ObtenerMuestraxCoordinador
            // y Trabajo.obtenerXCOE
            
            using var connection = new SqlConnection(_connectionString);
            var filtroCoe = coeId ?? usuarioId;
            var trabajos = await connection.QueryAsync<TrabajoCualitativoVm>(@"
                SELECT 
                    t.id AS Id,
                    t.NombreTrabajo AS Nombre,
                    u.Unidad AS UnidadNegocio,
                    t.COE AS CoeId,
                    COALESCE(coe.Nombres + ' ' + coe.Apellidos, 'Sin COE') AS CoeNombre,
                    t.OP_MetodologiaId AS Tipo,
                    met.MetNombre AS TipoDescripcion,
                    COALESCE(est.EstadoDesc, CAST(t.Estado AS varchar(20))) AS Estado,
                    tc.FechaInicioCampo,
                    tc.FechaFinalCampo AS FechaFinCampo,
                    t.TipoRecoleccionId AS TipoRecoleccion,
                    tr.Recoleccion AS TipoRecoleccionDescripcion
                FROM PY_Trabajo t
                LEFT JOIN OP_TrabajoConfiguracion tc ON t.id = tc.TrabajoId
                LEFT JOIN US_Unidades u ON t.Unidad = u.id
                LEFT JOIN US_Usuarios coe ON t.COE = coe.id
                LEFT JOIN OP_Metodologias met ON t.OP_MetodologiaId = met.id
                LEFT JOIN PY_EstadosTrabajo est ON t.Estado = est.id
                LEFT JOIN OP_TipoRecoleccion tr ON t.TipoRecoleccionId = tr.id
                WHERE t.COE = @CoeId
                ORDER BY t.id DESC",
                new { CoeId = filtroCoe });

            return (true, trabajos.ToList(), string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajos por coordinador {UsuarioId}", usuarioId);
            return (false, new List<TrabajoCualitativoVm>(), "Error al obtener trabajos. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, List<TrabajoCualitativoVm> Data, string Error)> ObtenerTrabajosPorCoeAsync(
        long? coeId = null, int? tipo = null, string? estado = null)
    {
        try
        {
            // Ref: CoreProject/Clases/PY/Trabajo.vb -> PY_Trabajos_Get_Cualitativos
            using var connection = new SqlConnection(_connectionString);

            var rows = await connection.QueryAsync<dynamic>(
                "PY_Trabajos_Get_Cualitativos",
                new
                {
                    id = (long?)null,
                    ProyectoId = (long?)null,
                    OP_MetodologiaId = (int?)null,
                    PresupuestoId = (string?)null,
                    NombreTrabajo = (string?)null,
                    Muestra = (long?)null,
                    FechaTentativaInicioCampo = (DateTime?)null,
                    FechaTentativaFinalizacion = (DateTime?)null,
                    COE = coeId,
                    Unidad = (int?)null,
                    JobBook = (string?)null,
                    TipoProyectoId = (short?)null,
                    TodosCampos = (string?)null
                },
                commandType: CommandType.StoredProcedure);

            var trabajos = rows.Select(r => new TrabajoCualitativoVm
            {
                Id = (long?)r?.id ?? (long?)r?.Id ?? 0,
                Nombre = (string?)r?.NombreTrabajo ?? (string?)r?.Nombre ?? string.Empty,
                UnidadNegocio = (string?)r?.Unidad ?? string.Empty,
                CoeId = (long?)r?.COE,
                Tipo = (int?)r?.OP_MetodologiaId,
                Estado = (string?)r?.EstadoDesc ?? (string?)r?.Estado ?? string.Empty
            }).ToList();

            return (true, trabajos, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajos por COE {CoeId}", coeId);
            return (false, new List<TrabajoCualitativoVm>(), "Error al obtener trabajos por COE. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, ConfiguracionTrabajoVm Data, string Error)> ObtenerConfiguracionTrabajoAsync(
        long trabajoId)
    {
        try
        {
            // Ref: Trabajos.aspx.vb líneas 145-167 (CargarConfiguracion)
            // REGLA 3: Usar EF para consultas simples
            
            var configuracion = await _context.Set<dynamic>()
                .FromSqlRaw(@"
                    SELECT 
                        t.Id AS TrabajoId,
                        t.NombreTrabajo AS TrabajoNombre,
                        u.Unidad AS UnidadNegocio,
                        tc.FechaInicioCampo,
                        tc.FechaFinalCampo AS FechaFinCampo,
                        t.TipoRecoleccionId AS TipoRecoleccion
                    FROM PY_Trabajo t
                    LEFT JOIN OP_TrabajoConfiguracion tc ON t.Id = tc.TrabajoId
                    LEFT JOIN US_Unidades u ON t.Unidad = u.id
                    WHERE t.Id = {0}", trabajoId)
                .FirstOrDefaultAsync();

            if (configuracion == null)
            {
                return (false, null!, "Trabajo no encontrado");
            }

            var vm = new ConfiguracionTrabajoVm
            {
                TrabajoId = configuracion.TrabajoId,
                TrabajoNombre = configuracion.TrabajoNombre ?? string.Empty,
                UnidadNegocio = configuracion.UnidadNegocio ?? string.Empty,
                FechaInicioCampo = configuracion.FechaInicioCampo,
                FechaFinCampo = configuracion.FechaFinCampo,
                TipoRecoleccion = configuracion.TipoRecoleccion ?? 1,
                Observaciones = string.Empty
            };

            return (true, vm, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo configuración trabajo {TrabajoId}", trabajoId);
            return (false, null!, "Error al obtener configuración del trabajo. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Error)> GuardarConfiguracionTrabajoAsync(
        long trabajoId, ConfiguracionTrabajoVm configuracion, long usuarioId)
    {
        try
        {
            // Ref: Trabajos.aspx.vb líneas 171-195 (btnGuardarConfiguracion_Click)
            // REGLA 3: Usar EF para INSERT/UPDATE simples
            
            var existente = await _context.Set<dynamic>()
                .FromSqlRaw("SELECT * FROM OP_TrabajoConfiguracion WHERE TrabajoId = {0}", trabajoId)
                .FirstOrDefaultAsync();

            if (existente == null)
            {
                // INSERT
                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO OP_TrabajoConfiguracion 
                        (TrabajoId, FechaInicioCampo, FechaFinalCampo)
                    VALUES ({0}, {1}, {2})",
                    new object[]
                    {
                        trabajoId,
                        (object?)configuracion.FechaInicioCampo ?? DBNull.Value,
                        (object?)configuracion.FechaFinCampo ?? DBNull.Value
                    });
            }
            else
            {
                // UPDATE
                await _context.Database.ExecuteSqlRawAsync(@"
                    UPDATE OP_TrabajoConfiguracion 
                    SET FechaInicioCampo = {1}, 
                        FechaFinalCampo = {2}
                    WHERE TrabajoId = {0}",
                    new object[]
                    {
                        trabajoId,
                        (object?)configuracion.FechaInicioCampo ?? DBNull.Value,
                        (object?)configuracion.FechaFinCampo ?? DBNull.Value
                    });
            }

            await _context.Database.ExecuteSqlRawAsync(@"
                UPDATE PY_Trabajo
                SET TipoRecoleccionId = {1}
                WHERE Id = {0}",
                new object[]
                {
                    trabajoId,
                    configuracion.TipoRecoleccion
                });

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando configuración trabajo {TrabajoId}", trabajoId);
            return (false, "Error al guardar configuración del trabajo. Por favor intente nuevamente.");
        }
    }

    public async Task<bool> ValidarPermisoCoordinadorAsync(long usuarioId, int permisoId)
    {
        try
        {
            // Ref: Trabajos.aspx.vb línea 26 (permisos.VerificarPermisoUsuario)
            using var connection = new SqlConnection(_connectionString);
            
            var resultado = await connection.QueryFirstOrDefaultAsync<int>(
                @"SELECT COUNT(*)
                  FROM US_PermisosUsuarios pu
                  INNER JOIN US_Permisos p ON pu.PermisoId = p.id
                  WHERE pu.UsuarioId = @UsuarioId 
                    AND p.id = @PermisoId",
                new { UsuarioId = usuarioId, PermisoId = permisoId });

            return resultado > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando permiso {PermisoId} usuario {UsuarioId}", permisoId, usuarioId);
            return false;
        }
    }

    public async Task<(bool Success, TrabajoCualitativoVm Data, string Error)> ObtenerTrabajoDetalleAsync(long trabajoId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var trabajo = await connection.QueryFirstOrDefaultAsync<TrabajoCualitativoVm>(@"
                SELECT 
                    t.id AS Id,
                    t.NombreTrabajo AS Nombre,
                    u.Unidad AS UnidadNegocio,
                    t.COE AS CoeId,
                    COALESCE(coe.Nombres + ' ' + coe.Apellidos, 'Sin COE') AS CoeNombre,
                    t.OP_MetodologiaId AS Tipo,
                    met.MetNombre AS TipoDescripcion,
                    COALESCE(est.EstadoDesc, CAST(t.Estado AS varchar(20))) AS Estado,
                    t.FechaInicio,
                    t.FechaTerminacion,
                    tc.FechaInicioCampo,
                    tc.FechaFinalCampo AS FechaFinCampo,
                    t.TipoRecoleccionId AS TipoRecoleccion,
                    tr.Recoleccion AS TipoRecoleccionDescripcion,
                    CASE WHEN EXISTS(SELECT 1 FROM PY_TrabajoCuali tc WHERE tc.TrabajoId = t.id) THEN 1 ELSE 0 END AS TieneFichaEntrevista,
                    CASE WHEN EXISTS(SELECT 1 FROM PY_TrabajoCuali tc WHERE tc.TrabajoId = t.id AND tc.IncentivoEconomico = 1) THEN 1 ELSE 0 END AS TieneFichaSesion,
                    CASE WHEN EXISTS(SELECT 1 FROM PY_TrabajoCuali tc WHERE tc.TrabajoId = t.id) THEN 1 ELSE 0 END AS TieneFichaObservacion,
                    CASE WHEN EXISTS(SELECT 1 FROM OP_MuestraTrabajos m WHERE m.TrabajoId = t.id) THEN 1 ELSE 0 END AS TieneMuestra
                FROM PY_Trabajo t
                LEFT JOIN OP_TrabajoConfiguracion tc ON t.id = tc.TrabajoId
                LEFT JOIN US_Unidades u ON t.Unidad = u.id
                LEFT JOIN US_Usuarios coe ON t.COE = coe.id
                LEFT JOIN OP_Metodologias met ON t.OP_MetodologiaId = met.id
                LEFT JOIN PY_EstadosTrabajo est ON t.Estado = est.id
                LEFT JOIN OP_TipoRecoleccion tr ON t.TipoRecoleccionId = tr.id
                WHERE t.id = @TrabajoId",
                new { TrabajoId = trabajoId });

            if (trabajo == null)
                return (false, null!, "Trabajo no encontrado");

            return (true, trabajo, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo detalle trabajo {TrabajoId}", trabajoId);
            return (false, null!, "Error al obtener detalle del trabajo. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, long TrabajoId, string Error)> CrearTrabajoAsync(
        TrabajoCualitativoVm trabajo, long usuarioId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var trabajoId = await connection.QuerySingleAsync<long>(@"
                INSERT INTO PY_Trabajo 
                    (NombreTrabajo, COE, Unidad, OP_MetodologiaId, TipoRecoleccionId, Estado, FechaInicio, FechaTerminacion)
                OUTPUT INSERTED.id
                VALUES 
                    (@Nombre, @CoeId, 1, @Tipo, @TipoRecoleccion, 1, @FechaInicio, @FechaTerminacion)",
                new
                {
                    trabajo.Nombre,
                    trabajo.CoeId,
                    trabajo.Tipo,
                    TipoRecoleccion = trabajo.TipoRecoleccion ?? 1,
                    trabajo.FechaInicio,
                    trabajo.FechaTerminacion
                });

            // Crear configuración si hay fechas de campo
            if (trabajo.FechaInicioCampo.HasValue || trabajo.FechaFinCampo.HasValue)
            {
                await connection.ExecuteAsync(@"
                    INSERT INTO OP_TrabajoConfiguracion (TrabajoId, FechaInicioCampo, FechaFinalCampo)
                    VALUES (@TrabajoId, @FechaInicioCampo, @FechaFinCampo)",
                    new
                    {
                        TrabajoId = trabajoId,
                        trabajo.FechaInicioCampo,
                        FechaFinCampo = trabajo.FechaFinCampo
                    });
            }

            _logger.LogInformation("Trabajo {TrabajoId} creado por usuario {UsuarioId}", trabajoId, usuarioId);
            return (true, trabajoId, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando trabajo");
            return (false, 0, "Error al crear trabajo. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Error)> ActualizarTrabajoAsync(
        TrabajoCualitativoVm trabajo, long usuarioId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            await connection.ExecuteAsync(@"
                UPDATE PY_Trabajo
                SET NombreTrabajo = @Nombre,
                    COE = @CoeId,
                    OP_MetodologiaId = @Tipo,
                    TipoRecoleccionId = @TipoRecoleccion,
                    FechaInicio = @FechaInicio,
                    FechaTerminacion = @FechaTerminacion
                WHERE id = @Id",
                new
                {
                    trabajo.Id,
                    trabajo.Nombre,
                    trabajo.CoeId,
                    trabajo.Tipo,
                    TipoRecoleccion = trabajo.TipoRecoleccion ?? 1,
                    trabajo.FechaInicio,
                    trabajo.FechaTerminacion
                });

            // Actualizar o crear configuración
            var existe = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(*) FROM OP_TrabajoConfiguracion WHERE TrabajoId = @TrabajoId",
                new { TrabajoId = trabajo.Id });

            if (existe > 0)
            {
                await connection.ExecuteAsync(@"
                    UPDATE OP_TrabajoConfiguracion
                    SET FechaInicioCampo = @FechaInicioCampo,
                        FechaFinalCampo = @FechaFinCampo
                    WHERE TrabajoId = @TrabajoId",
                    new
                    {
                        TrabajoId = trabajo.Id,
                        trabajo.FechaInicioCampo,
                        FechaFinCampo = trabajo.FechaFinCampo
                    });
            }
            else if (trabajo.FechaInicioCampo.HasValue || trabajo.FechaFinCampo.HasValue)
            {
                await connection.ExecuteAsync(@"
                    INSERT INTO OP_TrabajoConfiguracion (TrabajoId, FechaInicioCampo, FechaFinalCampo)
                    VALUES (@TrabajoId, @FechaInicioCampo, @FechaFinCampo)",
                    new
                    {
                        TrabajoId = trabajo.Id,
                        trabajo.FechaInicioCampo,
                        FechaFinCampo = trabajo.FechaFinCampo
                    });
            }

            _logger.LogInformation("Trabajo {TrabajoId} actualizado por usuario {UsuarioId}", trabajo.Id, usuarioId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando trabajo {TrabajoId}", trabajo.Id);
            return (false, "Error al actualizar trabajo. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, string Error)> EliminarTrabajoAsync(long trabajoId, long usuarioId)
    {
        try
        {
            // Validar que no tenga dependencias
            using var connection = new SqlConnection(_connectionString);
            
            var tieneDependencias = await connection.QueryFirstAsync<int>(@"
                SELECT 
                    (SELECT COUNT(*) FROM OP_FichaEntrevistas WHERE TrabajoId = @TrabajoId) +
                    (SELECT COUNT(*) FROM OP_FichaSesiones WHERE TrabajoId = @TrabajoId) +
                    (SELECT COUNT(*) FROM OP_FichaObservaciones WHERE TrabajoId = @TrabajoId) +
                    (SELECT COUNT(*) FROM OP_MuestraTrabajos WHERE TrabajoId = @TrabajoId) +
                    (SELECT COUNT(*) FROM PY_PlanillaModeracion WHERE TrabajoId = @TrabajoId)",
                new { TrabajoId = trabajoId });

            if (tieneDependencias > 0)
            {
                return (false, "No se puede eliminar el trabajo porque tiene fichas, muestra, programaciones o planillas asociadas");
            }

            // Eliminación lógica (cambiar estado)
            await connection.ExecuteAsync(@"
                UPDATE PY_Trabajo
                SET Estado = 99  -- Estado eliminado
                WHERE id = @TrabajoId",
                new { TrabajoId = trabajoId });

            _logger.LogInformation("Trabajo {TrabajoId} eliminado lógicamente por usuario {UsuarioId}", trabajoId, usuarioId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando trabajo {TrabajoId}", trabajoId);
            return (false, "Error al eliminar trabajo. Por favor intente nuevamente.");
        }
    }

    public async Task<(bool Success, NavigacionTrabajoVm Data, string Error)> ObtenerNavegacionTrabajoAsync(long trabajoId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var nav = await connection.QueryFirstOrDefaultAsync<dynamic>(@"
                SELECT 
                    CASE WHEN EXISTS(SELECT 1 FROM OP_FichaEntrevistas WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneFichaEntrevista,
                    CASE WHEN EXISTS(SELECT 1 FROM OP_FichaSesiones WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneFichaSesion,
                    CASE WHEN EXISTS(SELECT 1 FROM OP_FichaObservaciones WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneFichaObservacion,
                    CASE WHEN EXISTS(SELECT 1 FROM OP_MuestraTrabajos WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneMuestra,
                    CASE
                        WHEN EXISTS (
                            SELECT 1
                            FROM OP_Filtros F
                            INNER JOIN OP_Preguntas_Filtro PF ON PF.IdFiltro = F.Id
                            WHERE F.IdTrabajo = @TrabajoId
                        ) THEN 1 ELSE 0 END AS TieneFiltroReclutamiento,
                    0 AS TieneFiltroAsistencia,
                    0 AS TieneProgramacion,
                    CASE WHEN EXISTS(SELECT 1 FROM OP_IPS_Revisiones WHERE TrabajoId = @TrabajoId) THEN 1 ELSE 0 END AS TieneIps",
                new { TrabajoId = trabajoId });

            var tieneFichaEntrevista = (int?)(nav?.TieneFichaEntrevista) ?? 0;
            var tieneFichaSesion = (int?)(nav?.TieneFichaSesion) ?? 0;
            var tieneFichaObservacion = (int?)(nav?.TieneFichaObservacion) ?? 0;
            var tieneMuestra = (int?)(nav?.TieneMuestra) ?? 0;
            var tieneFiltroReclutamiento = (int?)(nav?.TieneFiltroReclutamiento) ?? 0;
            var tieneFiltroAsistencia = (int?)(nav?.TieneFiltroAsistencia) ?? 0;
            var tieneProgramacion = (int?)(nav?.TieneProgramacion) ?? 0;
            var tieneIps = (int?)(nav?.TieneIps) ?? 0;

            var navegacion = new NavigacionTrabajoVm
            {
                TrabajoId = trabajoId,
                PuedeIrAFichaEntrevista = tieneFichaEntrevista == 1,
                PuedeIrAFichaSesion = tieneFichaSesion == 1,
                PuedeIrAFichaObservacion = tieneFichaObservacion == 1,
                PuedeIrAMuestra = tieneMuestra == 1,
                PuedeIrAFiltroReclutamiento = tieneFiltroReclutamiento == 1,
                PuedeIrAFiltroAsistencia = tieneFiltroAsistencia == 1,
                PuedeIrAProgramacion = tieneProgramacion == 1,
                PuedeIrAIps = tieneIps == 1,
                MensajeNavegacion = "Navegación disponible según módulos configurados"
            };

            return (true, navegacion, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo navegación trabajo {TrabajoId}", trabajoId);
            return (false, null!, "Error al obtener navegación del trabajo. Por favor intente nuevamente.");
        }
    }
}
