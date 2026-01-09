using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.OP.Models;
using MatrixNext.Web.Services.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Implementación del servicio de fichas técnicas
/// Ref: ANALISIS_OP_CUALITATIVO_FASE4_FLUJOS2Y3.md § 3.3
/// </summary>
public class OpFichasTecnicasService : IOpFichasTecnicasService
{
    private readonly MatrixDbContext _context;
    private readonly string _connectionString;
    private readonly ILogger<OpFichasTecnicasService> _logger;
    private readonly IEmailService _emailService;

    public OpFichasTecnicasService(
        MatrixDbContext context,
        IConfiguration configuration,
        ILogger<OpFichasTecnicasService> logger,
        IEmailService emailService)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("MatrixDb")!;
        _logger = logger;
        _emailService = emailService;
    }

    public async Task<(bool Success, FichaTecnicaVm Data, string Error)> ObtenerFichaEntrevistaAsync(long trabajoId)
    {
        try
        {
            // Ref: FichaEntrevista.aspx.vb líneas 41-123 (cargarDatos)
            using var connection = new SqlConnection(_connectionString);
            
            var ficha = await connection.QueryFirstOrDefaultAsync<FichaTecnicaVm>(
                @"SELECT 
                    f.*,
                    t.Nombre AS TrabajoNombre
                  FROM OP_FichasTecnicas f
                  INNER JOIN PY_Trabajos t ON f.TrabajoId = t.Id
                  WHERE f.TrabajoId = @TrabajoId AND f.TipoFicha = 1",
                new { TrabajoId = trabajoId });

            if (ficha == null)
            {
                // Crear nueva ficha en blanco
                ficha = new FichaTecnicaVm
                {
                    TrabajoId = trabajoId,
                    TipoFicha = 1,
                    EstadoFicha = "Borrador"
                };
            }

            return (true, ficha, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo ficha entrevista trabajo {TrabajoId}", trabajoId);
            return (false, null!, ex.Message);
        }
    }

    public async Task<(bool Success, string Error)> GuardarFichaEntrevistaAsync(
        FichaTecnicaVm ficha, long usuarioId)
    {
        try
        {
            // Ref: FichaEntrevista.aspx.vb líneas 125-214 (btnGuardar_Click)
            // VALIDACIONES (8 documentadas en FASE4 § 3.3 PASO 3.2)
            
            // 1. Validar presupuesto
            var (validacionOk, disponible, errorValidacion) = await ValidarPresupuestoIncentivosAsync(
                ficha.TrabajoId, ficha.MontoIncentivos);
            
            if (!validacionOk)
                return (false, errorValidacion);

            if (ficha.MontoIncentivos > disponible)
                return (false, $"Monto de incentivos ({ficha.MontoIncentivos:C}) excede disponible ({disponible:C})");

            // 2. Validar fechas
            if (ficha.FechaInicioReclutamiento.HasValue && ficha.FechaFinReclutamiento.HasValue)
            {
                if (ficha.FechaInicioReclutamiento > ficha.FechaFinReclutamiento)
                    return (false, "Fecha inicio reclutamiento debe ser menor a fecha fin");
            }

            // 3. Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(ficha.Objetivos))
                return (false, "Objetivos son obligatorios");

            if (string.IsNullOrWhiteSpace(ficha.PerfilEntrevistados))
                return (false, "Perfil de entrevistados es obligatorio");

            if (ficha.CantidadEntrevistas <= 0)
                return (false, "Cantidad de entrevistas debe ser mayor a 0");

            // GUARDAR
            using var connection = new SqlConnection(_connectionString);
            
            var existente = await connection.QueryFirstOrDefaultAsync<long?>(
                "SELECT Id FROM OP_FichasTecnicas WHERE TrabajoId = @TrabajoId AND TipoFicha = 1",
                new { ficha.TrabajoId });

            if (existente == null)
            {
                // INSERT
                await connection.ExecuteAsync(
                    @"INSERT INTO OP_FichasTecnicas 
                        (TrabajoId, TipoFicha, Objetivos, PerfilEntrevistados, CantidadEntrevistas, 
                         Metodologia, TematicaPrincipal, MontoIncentivos, AyudasAudiovisuales, 
                         RecursosAdicionales, CantidadReclutadores, PerfilReclutadores, 
                         FechaInicioReclutamiento, FechaFinReclutamiento, LugarRealizacion, 
                         DireccionCompleta, CiudadId, FechaRealizacion, HoraInicio, HoraFin, 
                         HabeasDataFirmado, ObservacionesGenerales, EstadoFicha, 
                         CreadoPor, FechaCreacion)
                      VALUES 
                        (@TrabajoId, @TipoFicha, @Objetivos, @PerfilEntrevistados, @CantidadEntrevistas,
                         @Metodologia, @TematicaPrincipal, @MontoIncentivos, @AyudasAudiovisuales,
                         @RecursosAdicionales, @CantidadReclutadores, @PerfilReclutadores,
                         @FechaInicioReclutamiento, @FechaFinReclutamiento, @LugarRealizacion,
                         @DireccionCompleta, @CiudadId, @FechaRealizacion, @HoraInicio, @HoraFin,
                         @HabeasDataFirmado, @ObservacionesGenerales, 'Borrador',
                         @CreadoPor, GETDATE())",
                    new
                    {
                        ficha.TrabajoId,
                        ficha.TipoFicha,
                        ficha.Objetivos,
                        ficha.PerfilEntrevistados,
                        ficha.CantidadEntrevistas,
                        ficha.Metodologia,
                        ficha.TematicaPrincipal,
                        ficha.MontoIncentivos,
                        ficha.AyudasAudiovisuales,
                        ficha.RecursosAdicionales,
                        ficha.CantidadReclutadores,
                        ficha.PerfilReclutadores,
                        ficha.FechaInicioReclutamiento,
                        ficha.FechaFinReclutamiento,
                        ficha.LugarRealizacion,
                        ficha.DireccionCompleta,
                        ficha.CiudadId,
                        ficha.FechaRealizacion,
                        ficha.HoraInicio,
                        ficha.HoraFin,
                        ficha.HabeasDataFirmado,
                        ficha.ObservacionesGenerales,
                        CreadoPor = usuarioId
                    });
            }
            else
            {
                // UPDATE
                await connection.ExecuteAsync(
                    @"UPDATE OP_FichasTecnicas 
                      SET Objetivos = @Objetivos,
                          PerfilEntrevistados = @PerfilEntrevistados,
                          CantidadEntrevistas = @CantidadEntrevistas,
                          Metodologia = @Metodologia,
                          TematicaPrincipal = @TematicaPrincipal,
                          MontoIncentivos = @MontoIncentivos,
                          AyudasAudiovisuales = @AyudasAudiovisuales,
                          RecursosAdicionales = @RecursosAdicionales,
                          CantidadReclutadores = @CantidadReclutadores,
                          PerfilReclutadores = @PerfilReclutadores,
                          FechaInicioReclutamiento = @FechaInicioReclutamiento,
                          FechaFinReclutamiento = @FechaFinReclutamiento,
                          LugarRealizacion = @LugarRealizacion,
                          DireccionCompleta = @DireccionCompleta,
                          CiudadId = @CiudadId,
                          FechaRealizacion = @FechaRealizacion,
                          HoraInicio = @HoraInicio,
                          HoraFin = @HoraFin,
                          HabeasDataFirmado = @HabeasDataFirmado,
                          ObservacionesGenerales = @ObservacionesGenerales,
                          FechaModificacion = GETDATE()
                      WHERE TrabajoId = @TrabajoId AND TipoFicha = 1",
                    new
                    {
                        ficha.TrabajoId,
                        ficha.Objetivos,
                        ficha.PerfilEntrevistados,
                        ficha.CantidadEntrevistas,
                        ficha.Metodologia,
                        ficha.TematicaPrincipal,
                        ficha.MontoIncentivos,
                        ficha.AyudasAudiovisuales,
                        ficha.RecursosAdicionales,
                        ficha.CantidadReclutadores,
                        ficha.PerfilReclutadores,
                        ficha.FechaInicioReclutamiento,
                        ficha.FechaFinReclutamiento,
                        ficha.LugarRealizacion,
                        ficha.DireccionCompleta,
                        ficha.CiudadId,
                        ficha.FechaRealizacion,
                        ficha.HoraInicio,
                        ficha.HoraFin,
                        ficha.HabeasDataFirmado,
                        ficha.ObservacionesGenerales
                    });
            }

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando ficha entrevista trabajo {TrabajoId}", ficha.TrabajoId);
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string Error)> EntregarFichaEntrevistaAsync(long trabajoId, long usuarioId)
    {
        try
        {
            // Ref: FichaEntrevista.aspx.vb líneas 216-267 (btnEntregar_Click con EnviarCorreo)
            using var connection = new SqlConnection(_connectionString);
            
            // Cambiar estado a Entregada
            await connection.ExecuteAsync(
                @"UPDATE OP_FichasTecnicas 
                  SET EstadoFicha = 'Entregada',
                      FechaEntrega = GETDATE(),
                      EntregadoPor = @UsuarioId
                  WHERE TrabajoId = @TrabajoId AND TipoFicha = 1",
                new { TrabajoId = trabajoId, UsuarioId = usuarioId });

            // Enviar correo de notificación
            // TODO: Implementar envío de correo con template
            // await _emailService.SendEmailAsync(destinatarios, subject, body);

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error entregando ficha entrevista trabajo {TrabajoId}", trabajoId);
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, FichaTecnicaVm Data, string Error)> ObtenerFichaSesionAsync(long trabajoId)
    {
        // Similar a ObtenerFichaEntrevistaAsync, TipoFicha = 2
        return await ObtenerFichaPorTipoAsync(trabajoId, 2);
    }

    public async Task<(bool Success, string Error)> GuardarFichaSesionAsync(
        FichaTecnicaVm ficha, long usuarioId)
    {
        ficha.TipoFicha = 2;
        return await GuardarFichaEntrevistaAsync(ficha, usuarioId);
    }

    public async Task<(bool Success, FichaTecnicaVm Data, string Error)> ObtenerFichaObservacionAsync(long trabajoId)
    {
        // Similar a ObtenerFichaEntrevistaAsync, TipoFicha = 3
        return await ObtenerFichaPorTipoAsync(trabajoId, 3);
    }

    public async Task<(bool Success, string Error)> GuardarFichaObservacionAsync(
        FichaTecnicaVm ficha, long usuarioId)
    {
        ficha.TipoFicha = 3;
        return await GuardarFichaEntrevistaAsync(ficha, usuarioId);
    }

    public async Task<(bool Success, decimal Disponible, string Error)> ValidarPresupuestoIncentivosAsync(
        long trabajoId, decimal montoSolicitado)
    {
        try
        {
            // Ref: FichaEntrevista.aspx.vb líneas 269-305 (ValidarPresupuesto)
            using var connection = new SqlConnection(_connectionString);
            
            var resultado = await connection.QueryFirstOrDefaultAsync<dynamic>(
                @"SELECT 
                    ISNULL(p.PresupuestoIncentivos, 0) AS PresupuestoTotal,
                    ISNULL(SUM(f.MontoIncentivos), 0) AS MontoUtilizado
                  FROM PY_Trabajos t
                  LEFT JOIN OP_PresupuestosCualitativo p ON t.Id = p.TrabajoId
                  LEFT JOIN OP_FichasTecnicas f ON t.Id = f.TrabajoId
                  WHERE t.Id = @TrabajoId
                  GROUP BY p.PresupuestoIncentivos",
                new { TrabajoId = trabajoId });

            if (resultado == null)
                return (false, 0, "No se encontró presupuesto configurado para este trabajo");

            decimal presupuestoTotal = resultado.PresupuestoTotal;
            decimal montoUtilizado = resultado.MontoUtilizado;
            decimal disponible = presupuestoTotal - montoUtilizado;

            return (true, disponible, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando presupuesto trabajo {TrabajoId}", trabajoId);
            return (false, 0, ex.Message);
        }
    }

    public async Task<(bool Success, string Error)> ActualizarHabeasDataAsync(
        long trabajoId, bool habeasDataFirmado)
    {
        try
        {
            // Ref: FichaEntrevista.aspx.vb líneas 307-332
            using var connection = new SqlConnection(_connectionString);
            
            await connection.ExecuteAsync(
                @"UPDATE OP_FichasTecnicas 
                  SET HabeasDataFirmado = @HabeasDataFirmado,
                      FechaModificacion = GETDATE()
                  WHERE TrabajoId = @TrabajoId",
                new { TrabajoId = trabajoId, HabeasDataFirmado = habeasDataFirmado });

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando Habeas Data trabajo {TrabajoId}", trabajoId);
            return (false, ex.Message);
        }
    }

    // Helper method
    private async Task<(bool Success, FichaTecnicaVm Data, string Error)> ObtenerFichaPorTipoAsync(
        long trabajoId, int tipoFicha)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var ficha = await connection.QueryFirstOrDefaultAsync<FichaTecnicaVm>(
                @"SELECT 
                    f.*,
                    t.Nombre AS TrabajoNombre
                  FROM OP_FichasTecnicas f
                  INNER JOIN PY_Trabajos t ON f.TrabajoId = t.Id
                  WHERE f.TrabajoId = @TrabajoId AND f.TipoFicha = @TipoFicha",
                new { TrabajoId = trabajoId, TipoFicha = tipoFicha });

            if (ficha == null)
            {
                ficha = new FichaTecnicaVm
                {
                    TrabajoId = trabajoId,
                    TipoFicha = tipoFicha,
                    EstadoFicha = "Borrador"
                };
            }

            return (true, ficha, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo ficha tipo {TipoFicha} trabajo {TrabajoId}", 
                tipoFicha, trabajoId);
            return (false, null!, ex.Message);
        }
    }
}
