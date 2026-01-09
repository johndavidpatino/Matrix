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
            // Ref: Trabajos.aspx.vb líneas 21-47 (CargarTrabajos con coordinador)
            // REGLA 2: Consultar CoreProject → usa CoordinacionCampo.ObtenerMuestraxCoordinador
            // y Trabajo.obtenerXCOE
            
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@UsuarioId", usuarioId);
            parameters.Add("@CoeId", coeId);

            // SP esperado basado en evidencia de CoreProject
            // Ref: ANALISIS_OP_CUALITATIVO_FASE5 § 5.2
            var trabajos = await connection.QueryAsync<TrabajoCualitativoVm>(
                "OP_ObtenerTrabajosCualitativosXCoordinador",
                parameters,
                commandType: CommandType.StoredProcedure);

            return (true, trabajos.ToList(), string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajos por coordinador {UsuarioId}", usuarioId);
            return (false, new List<TrabajoCualitativoVm>(), ex.Message);
        }
    }

    public async Task<(bool Success, List<TrabajoCualitativoVm> Data, string Error)> ObtenerTrabajosPorCoeAsync(
        long? coeId = null, int? tipo = null, string estado = null)
    {
        try
        {
            // Ref: Trabajos.aspx.vb líneas 38-42 (ObtenerTrabajosCualitativosxCOE)
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@CoeId", coeId);
            parameters.Add("@Tipo", tipo);
            parameters.Add("@Estado", estado);

            var trabajos = await connection.QueryAsync<TrabajoCualitativoVm>(
                "OP_ObtenerTrabajosCualitativosXCOE",
                parameters,
                commandType: CommandType.StoredProcedure);

            return (true, trabajos.ToList(), string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajos por COE {CoeId}", coeId);
            return (false, new List<TrabajoCualitativoVm>(), ex.Message);
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
                        t.Nombre AS TrabajoNombre,
                        t.UnidadNegocio,
                        tc.FechaInicioCampo,
                        tc.FechaFinCampo,
                        tc.TipoRecoleccion,
                        tc.Observaciones
                    FROM PY_Trabajos t
                    LEFT JOIN OP_TrabajosConfiguracion tc ON t.Id = tc.TrabajoId
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
                Observaciones = configuracion.Observaciones ?? string.Empty
            };

            return (true, vm, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo configuración trabajo {TrabajoId}", trabajoId);
            return (false, null!, ex.Message);
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
                .FromSqlRaw("SELECT * FROM OP_TrabajosConfiguracion WHERE TrabajoId = {0}", trabajoId)
                .FirstOrDefaultAsync();

            if (existente == null)
            {
                // INSERT
                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO OP_TrabajosConfiguracion 
                        (TrabajoId, FechaInicioCampo, FechaFinCampo, TipoRecoleccion, Observaciones, CreadoPor, FechaCreacion)
                    VALUES ({0}, {1}, {2}, {3}, {4}, {5}, GETDATE())",
                    trabajoId,
                    configuracion.FechaInicioCampo,
                    configuracion.FechaFinCampo,
                    configuracion.TipoRecoleccion,
                    configuracion.Observaciones,
                    usuarioId);
            }
            else
            {
                // UPDATE
                await _context.Database.ExecuteSqlRawAsync(@"
                    UPDATE OP_TrabajosConfiguracion 
                    SET FechaInicioCampo = {1}, 
                        FechaFinCampo = {2}, 
                        TipoRecoleccion = {3}, 
                        Observaciones = {4},
                        ModificadoPor = {5},
                        FechaModificacion = GETDATE()
                    WHERE TrabajoId = {0}",
                    trabajoId,
                    configuracion.FechaInicioCampo,
                    configuracion.FechaFinCampo,
                    configuracion.TipoRecoleccion,
                    configuracion.Observaciones,
                    usuarioId);
            }

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando configuración trabajo {TrabajoId}", trabajoId);
            return (false, ex.Message);
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
                  INNER JOIN US_Permisos p ON pu.IdPermiso = p.Id
                  WHERE pu.IdUsuario = @UsuarioId 
                    AND p.Id = @PermisoId 
                    AND pu.Activo = 1",
                new { UsuarioId = usuarioId, PermisoId = permisoId });

            return resultado > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando permiso {PermisoId} usuario {UsuarioId}", permisoId, usuarioId);
            return false;
        }
    }
}
