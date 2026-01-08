using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.OP;
using MatrixNext.Web.Services;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Implementación del servicio de gestión de muestra por ciudad
    /// </summary>
    public class OpMuestraService : IOpMuestraService
    {
        private readonly MatrixDbContext _dbContext;
        private readonly ILogger<OpMuestraService> _logger;
        private readonly IEmailService _emailService;

        public OpMuestraService(
            MatrixDbContext dbContext,
            ILogger<OpMuestraService> logger,
            IEmailService emailService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<List<MuestraCiudadListItemVM>> ObtenerMuestraPorTrabajoAsync(long trabajoId)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                // Query con JOIN a Divipola para obtener nombres de departamento y ciudad
                var muestras = await connection.QueryAsync<MuestraCiudadDto>(@"
                    SELECT 
                        m.Id,
                        d.DivDeptoNombre AS Departamento,
                        d.DivMuniNombre AS Ciudad,
                        m.CiudadId,
                        m.Cantidad,
                        m.FechaInicio,
                        m.FechaFin,
                        CONCAT(u.Nombres, ' ', u.Apellidos) AS CoordinadorNombre
                    FROM OP_MuestraTrabajos m
                    LEFT JOIN C_Divipola d ON m.CiudadId = d.DivMuniCodigo
                    LEFT JOIN TH_Personas u ON m.Coordinador = u.IdPersona
                    WHERE m.TrabajoId = @TrabajoId
                    ORDER BY d.DivMuniNombre",
                    new { TrabajoId = trabajoId });

                return muestras.Select(m => new MuestraCiudadListItemVM
                {
                    Id = m.Id,
                    Departamento = m.Departamento,
                    Ciudad = m.Ciudad,
                    CiudadId = m.CiudadId,
                    Cantidad = m.Cantidad,
                    FechaInicio = m.FechaInicio,
                    FechaFin = m.FechaFin,
                    CoordinadorNombre = m.CoordinadorNombre
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener muestra del trabajo {TrabajoId}", trabajoId);
                return new List<MuestraCiudadListItemVM>();
            }
        }

        public async Task<MuestraCiudadVM?> ObtenerMuestraPorIdAsync(long idMuestra)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var muestra = await connection.QueryFirstOrDefaultAsync<MuestraCiudadDto>(@"
                    SELECT Id, TrabajoId, CiudadId, Cantidad, FechaInicio, FechaFin, Coordinador AS CoordinadorId
                    FROM OP_MuestraTrabajos
                    WHERE Id = @IdMuestra",
                    new { IdMuestra = idMuestra });

                if (muestra == null)
                    return null;

                return new MuestraCiudadVM
                {
                    Id = muestra.Id,
                    TrabajoId = muestra.TrabajoId,
                    CiudadId = muestra.CiudadId ?? 0,
                    Cantidad = muestra.Cantidad,
                    FechaInicio = muestra.FechaInicio,
                    FechaFin = muestra.FechaFin,
                    CoordinadorId = muestra.CoordinadorId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener muestra {IdMuestra}", idMuestra);
                return null;
            }
        }

        public async Task<double> ObtenerMuestraPorCiudadAsync(long trabajoId, int ciudadId)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var cantidad = await connection.QueryFirstOrDefaultAsync<double?>(@"
                    SELECT Cantidad
                    FROM OP_MuestraTrabajos
                    WHERE TrabajoId = @TrabajoId AND CiudadId = @CiudadId",
                    new { TrabajoId = trabajoId, CiudadId = ciudadId });

                return cantidad ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener muestra de ciudad {CiudadId} en trabajo {TrabajoId}", 
                    ciudadId, trabajoId);
                return 0;
            }
        }

        public async Task<long> GuardarMuestraAsync(MuestraCiudadVM model)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                if (model.Id.HasValue && model.Id.Value > 0)
                {
                    // Actualizar muestra existente
                    await connection.ExecuteAsync(@"
                        UPDATE OP_MuestraTrabajos
                        SET Cantidad = @Cantidad,
                            FechaInicio = @FechaInicio,
                            FechaFin = @FechaFin,
                            Coordinador = @CoordinadorId
                        WHERE Id = @Id",
                        new
                        {
                            model.Id,
                            model.Cantidad,
                            model.FechaInicio,
                            model.FechaFin,
                            model.CoordinadorId
                        });

                    _logger.LogInformation("Muestra {Id} actualizada para trabajo {TrabajoId}, ciudad {CiudadId}",
                        model.Id, model.TrabajoId, model.CiudadId);

                    return model.Id.Value;
                }
                else
                {
                    // Insertar nueva muestra
                    var id = await connection.QuerySingleAsync<long>(@"
                        INSERT INTO OP_MuestraTrabajos (TrabajoId, CiudadId, Cantidad, FechaInicio, FechaFin, Coordinador)
                        VALUES (@TrabajoId, @CiudadId, @Cantidad, @FechaInicio, @FechaFin, @CoordinadorId);
                        SELECT CAST(SCOPE_IDENTITY() as bigint);",
                        new
                        {
                            model.TrabajoId,
                            model.CiudadId,
                            model.Cantidad,
                            model.FechaInicio,
                            model.FechaFin,
                            model.CoordinadorId
                        });

                    _logger.LogInformation("Muestra {Id} creada para trabajo {TrabajoId}, ciudad {CiudadId}",
                        id, model.TrabajoId, model.CiudadId);

                    return id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar muestra para trabajo {TrabajoId}", model.TrabajoId);
                throw;
            }
        }

        public async Task<bool> ActualizarFechasConPlaneacionAsync(ActualizarFechasMuestraVM model)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                await using var transaction = await connection.BeginTransactionAsync();
                try
                {
                    // 1. Actualizar fechas en OP_MuestraTrabajos
                    await connection.ExecuteAsync(@"
                        UPDATE OP_MuestraTrabajos
                        SET FechaInicio = @FechaInicio,
                            FechaFin = @FechaFin
                        WHERE Id = @IdMuestra",
                        new
                        {
                            model.IdMuestra,
                            model.FechaInicio,
                            model.FechaFin
                        },
                        transaction: transaction);

                    // 2. Ejecutar SP de auto-planeación
                    // Mapea a OP_AjusteProduccionAutoCiudad
                    await connection.ExecuteAsync(
                        "OP_AjusteProduccionAutoCiudad",
                        new
                        {
                            IdMuestra = model.IdMuestra,
                            lun = model.Lunes,
                            mar = model.Martes,
                            mie = model.Miercoles,
                            jue = model.Jueves,
                            vie = model.Viernes,
                            sab = model.Sabado,
                            dom = model.Domingo,
                            fest = !model.ExcluirFestivos // SP usa 'fest' para incluir festivos
                        },
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure);

                    // 3. Obtener información de muestra y coordinador para email
                    var muestra = await connection.QueryFirstOrDefaultAsync<MuestraCiudadDto>(@"
                        SELECT 
                            m.Id,
                            d.DivDeptoNombre AS Departamento,
                            d.DivMuniNombre AS Ciudad,
                            m.CiudadId,
                            m.Cantidad,
                            m.FechaInicio,
                            m.FechaFin,
                            CONCAT(u.Nombres, ' ', u.Apellidos) AS CoordinadorNombre,
                            u.Email AS CoordinadorEmail
                        FROM OP_MuestraTrabajos m
                        LEFT JOIN C_Divipola d ON m.CiudadId = d.DivMuniCodigo
                        LEFT JOIN TH_Personas u ON m.Coordinador = u.IdPersona
                        WHERE m.Id = @IdMuestra",
                        new { IdMuestra = model.IdMuestra },
                        transaction: transaction);

                    await transaction.CommitAsync();

                    // 4. Enviar email al coordinador si existe email
                    if (muestra != null && !string.IsNullOrWhiteSpace(muestra.CoordinadorEmail))
                    {
                        try
                        {
                            var cuerpoEmail = GenerarCuerpoEmailActualizacionMuestra(muestra, model);
                            await _emailService.EnviarAsync(
                                destinatario: muestra.CoordinadorEmail,
                                asunto: $"Actualización de Muestra - {muestra.Ciudad}",
                                cuerpo: cuerpoEmail,
                                esHtml: true);

                            _logger.LogInformation(
                                "Email de actualización de muestra enviado a {CoordinadorEmail}",
                                muestra.CoordinadorEmail);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex,
                                "No se pudo enviar email de actualización de muestra al coordinador {Email}",
                                muestra.CoordinadorEmail);
                            // No lanzamos excepción aquí para no bloquear la actualización
                        }
                    }

                    _logger.LogInformation("Fechas y planeación actualizadas para muestra {IdMuestra}", model.IdMuestra);
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar fechas con planeación para muestra {IdMuestra}", model.IdMuestra);
                return false;
            }
        }

        /// <summary>
        /// Genera el cuerpo del email de actualización de muestra para el coordinador.
        /// </summary>
        private string GenerarCuerpoEmailActualizacionMuestra(
            MuestraCiudadDto muestra,
            ActualizarFechasMuestraVM detalles)
        {
            var diasSeleccionados = new List<string>();
            if (detalles.Lunes) diasSeleccionados.Add("Lunes");
            if (detalles.Martes) diasSeleccionados.Add("Martes");
            if (detalles.Miercoles) diasSeleccionados.Add("Miércoles");
            if (detalles.Jueves) diasSeleccionados.Add("Jueves");
            if (detalles.Viernes) diasSeleccionados.Add("Viernes");
            if (detalles.Sabado) diasSeleccionados.Add("Sábado");
            if (detalles.Domingo) diasSeleccionados.Add("Domingo");

            var diasTexto = string.Join(", ", diasSeleccionados.Any() ? diasSeleccionados : new List<string> { "Todos los días" });

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; background-color: #f9f9f9; padding: 20px; }}
        .header {{ background-color: #004B87; color: white; padding: 20px; border-radius: 5px 5px 0 0; }}
        .content {{ background-color: white; padding: 20px; border-radius: 0 0 5px 5px; }}
        .field {{ margin-bottom: 15px; border-bottom: 1px solid #eee; padding-bottom: 10px; }}
        .field-label {{ font-weight: bold; color: #004B87; }}
        .alert {{ background-color: #e8f4f8; border-left: 4px solid #004B87; padding: 10px; margin: 15px 0; }}
        .footer {{ color: #666; font-size: 12px; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Actualización de Muestra</h2>
        </div>
        <div class='content'>
            <p>Estimado(a) {muestra.CoordinadorNombre},</p>
            
            <p>Le informamos que la muestra para la ciudad de <strong>{muestra.Ciudad}</strong> ha sido actualizada con los siguientes cambios:</p>
            
            <div class='field'>
                <span class='field-label'>Ciudad:</span> {muestra.Ciudad}, {muestra.Departamento}
            </div>
            
            <div class='field'>
                <span class='field-label'>Cantidad de Muestra:</span> {muestra.Cantidad} personas
            </div>
            
            <div class='field'>
                <span class='field-label'>Fecha de Inicio:</span> {muestra.FechaInicio:dd/MM/yyyy}
            </div>
            
            <div class='field'>
                <span class='field-label'>Fecha de Fin:</span> {muestra.FechaFin:dd/MM/yyyy}
            </div>
            
            <div class='field'>
                <span class='field-label'>Días de Ejecución:</span> {diasTexto}
            </div>
            
            <div class='field'>
                <span class='field-label'>Excluir Festivos:</span> {(detalles.ExcluirFestivos ? "Sí" : "No")}
            </div>
            
            <div class='alert'>
                <strong>Nota:</strong> La planeación de producción ha sido actualizada automáticamente de acuerdo a los nuevos parámetros.
            </div>
            
            <p>Si tiene alguna pregunta o necesita hacer ajustes adicionales, favor contactar al equipo de Operaciones.</p>
            
            <p>Cordial saludo,<br>
            <strong>Sistema de Gestión de Operaciones</strong></p>
            
            <div class='footer'>
                <p>Este es un mensaje automático generado por el Sistema Matrix. Por favor, no responda a este correo.</p>
            </div>
        </div>
    </div>
</body>
</html>";
        }

        public async Task<bool> EliminarMuestraAsync(long idMuestra)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var rowsAffected = await connection.ExecuteAsync(@"
                    DELETE FROM OP_MuestraTrabajos
                    WHERE Id = @IdMuestra",
                    new { IdMuestra = idMuestra });

                if (rowsAffected > 0)
                {
                    _logger.LogInformation("Muestra {IdMuestra} eliminada correctamente", idMuestra);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar muestra {IdMuestra}", idMuestra);
                return false;
            }
        }

        public async Task<double> CalcularTotalMuestraAsync(long trabajoId)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var total = await connection.QueryFirstOrDefaultAsync<double?>(@"
                    SELECT SUM(Cantidad)
                    FROM OP_MuestraTrabajos
                    WHERE TrabajoId = @TrabajoId",
                    new { TrabajoId = trabajoId });

                return total ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calcular total de muestra del trabajo {TrabajoId}", trabajoId);
                return 0;
            }
        }

        #region DTOs Internos

        private class MuestraCiudadDto
        {
            public long Id { get; set; }
            public long TrabajoId { get; set; }
            public int? CiudadId { get; set; }
            public string? Departamento { get; set; }
            public string? Ciudad { get; set; }
            public double Cantidad { get; set; }
            public DateTime? FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
            public long? CoordinadorId { get; set; }
            public string? CoordinadorNombre { get; set; }
            public string? CoordinadorEmail { get; set; }
        }

        #endregion
    }
}
