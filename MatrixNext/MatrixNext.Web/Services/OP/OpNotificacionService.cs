using MatrixNext.Data.Services;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.OP.Models;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Data;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Implementación de servicio de notificaciones por email para OP
/// Reutiliza IEmailQueueService (cola existente)
/// Ref: WebMatrix/Emails/*.aspx (EnviarCorreo pattern)
/// </summary>
public class OpNotificacionService : IOpNotificacionService
{
    private readonly IEmailQueueService _emailQueue;
    private readonly MatrixDbContext _dbContext;
    private readonly ILogger<OpNotificacionService> _logger;
    private readonly IConfiguration _configuration;

    public OpNotificacionService(
        IEmailQueueService emailQueue,
        MatrixDbContext dbContext,
        ILogger<OpNotificacionService> logger,
        IConfiguration configuration)
    {
        _emailQueue = emailQueue;
        _dbContext = dbContext;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<(bool Success, string Error)> NotificarProgramacionCreadaAsync(long programacionId)
    {
        try
        {
            var prog = await ObtenerProgramacionAsync(programacionId);
            if (prog == null || string.IsNullOrEmpty(prog.EmailEntrevistado))
                return (false, "Programación o email no disponible");

            var asunto = $"Programación de Sesión - {prog.TipoSesion ?? "Sin especificar"}";
            var cuerpo = GenerarEmailProgramacionCreada(prog);

            await _emailQueue.QueueEmailAsync(prog.EmailEntrevistado, asunto, cuerpo, esHtml: true);
            
            _logger.LogInformation("Notificación programación {Id} encolada para {Email}",
                programacionId, prog.EmailEntrevistado);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notificando programación {Id}", programacionId);
            return (false, "Error al encolar notificación");
        }
    }

    public async Task<(bool Success, string Error)> EnviarRecordatorioSesionAsync(long programacionId)
    {
        try
        {
            var prog = await ObtenerProgramacionAsync(programacionId);
            if (prog == null || string.IsNullOrEmpty(prog.EmailEntrevistado))
                return (false, "Programación o email no disponible");

            if (!prog.FechaProgramada.HasValue)
                return (false, "Programación sin fecha");

            // Validar ventana 24-48h
            var horasRestantes = (prog.FechaProgramada.Value - DateTime.Now).TotalHours;
            if (horasRestantes < 0 || horasRestantes > 48)
            {
                _logger.LogInformation("Recordatorio fuera de ventana 24-48h para programación {Id}", programacionId);
                return (false, "Fuera de ventana de recordatorio");
            }

            var asunto = $"Recordatorio: Sesión mañana - {prog.TipoSesion ?? "Sin especificar"}";
            var cuerpo = GenerarEmailRecordatorio(prog);

            await _emailQueue.QueueEmailAsync(prog.EmailEntrevistado, asunto, cuerpo, esHtml: true);
            
            _logger.LogInformation("Recordatorio programación {Id} encolado", programacionId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando recordatorio {Id}", programacionId);
            return (false, "Error al encolar recordatorio");
        }
    }

    public async Task<(bool Success, string Error)> NotificarCambioEstadoProgramacionAsync(
        long programacionId, string estadoAnterior, string estadoNuevo)
    {
        try
        {
            var prog = await ObtenerProgramacionAsync(programacionId);
            if (prog == null || string.IsNullOrEmpty(prog.EmailEntrevistado))
                return (false, "Programación o email no disponible");

            // Solo notificar estados relevantes
            var estadosNotificables = new[] { "Confirmado", "Cancelado", "Reprogramado", "Completado" };
            if (!estadosNotificables.Contains(estadoNuevo))
                return (true, "Estado no requiere notificación");

            var asunto = $"Actualización de Estado: {estadoNuevo}";
            var cuerpo = GenerarEmailCambioEstado(prog, estadoAnterior, estadoNuevo);

            await _emailQueue.QueueEmailAsync(prog.EmailEntrevistado, asunto, cuerpo, esHtml: true);
            
            _logger.LogInformation("Notificación cambio estado {EstadoNuevo} programación {Id} encolada",
                estadoNuevo, programacionId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notificando cambio estado {Id}", programacionId);
            return (false, "Error al encolar notificación");
        }
    }

    public async Task<(bool Success, string Error)> NotificarFichaCompletadaAsync(long fichaId)
    {
        try
        {
            var ficha = await ObtenerFichaAsync(fichaId);
            if (ficha == null)
                return (false, "Ficha no encontrada");

            var destinatarios = await ObtenerEmailsStakeholdersAsync(fichaId);
            if (!destinatarios.Any())
            {
                _logger.LogWarning("Ficha {Id} sin destinatarios", fichaId);
                return (false, "Sin destinatarios");
            }

            var asunto = $"Ficha Técnica Completada - Trabajo {ficha.TrabajoId}";
            var cuerpo = GenerarEmailFichaCompletada(ficha);

            await _emailQueue.QueueEmailMultipleAsync(destinatarios, asunto, cuerpo);
            
            _logger.LogInformation("Notificación ficha {Id} encolada para {Count} destinatarios",
                fichaId, destinatarios.Count);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notificando ficha {Id}", fichaId);
            return (false, "Error al encolar notificación");
        }
    }

    public async Task<(bool Success, string Error)> NotificarAsignacionModeradorAsync(
        long programacionId, long moderadorId)
    {
        try
        {
            var prog = await ObtenerProgramacionAsync(programacionId);
            if (prog == null)
                return (false, "Programación no encontrada");

            var emailModerador = await ObtenerEmailUsuarioAsync(moderadorId);
            if (string.IsNullOrEmpty(emailModerador))
                return (false, "Email moderador no disponible");

            var asunto = $"Asignación de Sesión - {prog.TipoSesion ?? "Sin especificar"}";
            var cuerpo = GenerarEmailAsignacionModerador(prog);

            await _emailQueue.QueueEmailAsync(emailModerador, asunto, cuerpo, esHtml: true);
            
            _logger.LogInformation("Asignación moderador {ModeradorId} programación {Id} encolada",
                moderadorId, programacionId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notificando asignación {Id}", programacionId);
            return (false, "Error al encolar notificación");
        }
    }

    public async Task<(bool Success, string Error)> EnviarReporteDiarioAsync(DateTime fecha)
    {
        try
        {
            var emailsAdmin = await ObtenerEmailsAdministradoresAsync();
            if (!emailsAdmin.Any())
                return (false, "Sin administradores configurados");

            var programaciones = await ObtenerProgramacionesPorFechaAsync(fecha);
            if (!programaciones.Any())
            {
                _logger.LogInformation("Sin programaciones para {Fecha}", fecha.Date);
                return (true, "Sin programaciones");
            }

            var asunto = $"Reporte Diario de Programaciones - {fecha:dd/MM/yyyy}";
            var cuerpo = GenerarEmailReporteDiario(programaciones, fecha);

            await _emailQueue.QueueEmailMultipleAsync(emailsAdmin, asunto, cuerpo);
            
            _logger.LogInformation("Reporte {Fecha} encolado para {Count} admins", fecha.Date, emailsAdmin.Count);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando reporte {Fecha}", fecha);
            return (false, "Error al generar reporte");
        }
    }

    #region DTOs privados

    private class ProgramacionData
    {
        public long Id { get; set; }
        public long TrabajoId { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public string? TipoSesion { get; set; }
        public string? EmailEntrevistado { get; set; }
        public string? NombreEntrevistado { get; set; }
        public string? EmailModerador { get; set; }
        public string? NombreModerador { get; set; }
    }

    private class FichaData
    {
        public long Id { get; set; }
        public long TrabajoId { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    #endregion

    #region Métodos de consulta privados

    private async Task<ProgramacionData?> ObtenerProgramacionAsync(long id)
    {
        await using var conn = _dbContext.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open)
            await conn.OpenAsync();

        return await conn.QueryFirstOrDefaultAsync<ProgramacionData>(@"
            SELECT TOP 1
                pc.Id,
                pc.TrabajoId,
                pc.FechaProgramada,
                pc.TipoSesion,
                p1.Email AS EmailEntrevistado,
                CONCAT(p1.Nombres, ' ', p1.Apellidos) AS NombreEntrevistado,
                p2.Email AS EmailModerador,
                CONCAT(p2.Nombres, ' ', p2.Apellidos) AS NombreModerador
            FROM OP_ProgramacionCampo pc
            LEFT JOIN TH_Personas p1 ON pc.EntrevistadoId = p1.id
            LEFT JOIN TH_Personas p2 ON pc.ModeradorId = p2.id
            WHERE pc.Id = @Id",
            new { Id = id });
    }

    private async Task<FichaData?> ObtenerFichaAsync(long id)
    {
        await using var conn = _dbContext.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open)
            await conn.OpenAsync();

        return await conn.QueryFirstOrDefaultAsync<FichaData>(@"
            SELECT TOP 1 Id, TrabajoId, FechaCreacion
            FROM OP_FichasTecnicas
            WHERE Id = @Id",
            new { Id = id });
    }

    private async Task<string?> ObtenerEmailUsuarioAsync(long usuarioId)
    {
        await using var conn = _dbContext.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open)
            await conn.OpenAsync();

        return await conn.QueryFirstOrDefaultAsync<string>(
            "SELECT Email FROM TH_Personas WHERE id = @Id AND Email IS NOT NULL",
            new { Id = usuarioId });
    }

    private async Task<List<string>> ObtenerEmailsStakeholdersAsync(long fichaId)
    {
        await using var conn = _dbContext.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open)
            await conn.OpenAsync();

        var emails = await conn.QueryAsync<string>(@"
            SELECT DISTINCT p.Email
            FROM OP_FichasTecnicas f
            INNER JOIN PY_Trabajo t ON f.TrabajoId = t.TrabajoId
            LEFT JOIN TH_Personas p ON t.CoordinadorId = p.id
            WHERE f.Id = @FichaId AND p.Email IS NOT NULL",
            new { FichaId = fichaId });

        return emails.ToList();
    }

    private async Task<List<string>> ObtenerEmailsAdministradoresAsync()
    {
        await using var conn = _dbContext.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open)
            await conn.OpenAsync();

        var emails = await conn.QueryAsync<string>(@"
            SELECT DISTINCT p.Email
            FROM TH_Personas p
            WHERE p.Email IS NOT NULL AND p.Activo = 1");

        return emails.ToList();
    }

    private async Task<List<ProgramacionData>> ObtenerProgramacionesPorFechaAsync(DateTime fecha)
    {
        await using var conn = _dbContext.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open)
            await conn.OpenAsync();

        var programaciones = await conn.QueryAsync<ProgramacionData>(@"
            SELECT
                Id,
                TrabajoId,
                FechaProgramada,
                TipoSesion,
                NULL AS EmailEntrevistado,
                NULL AS NombreEntrevistado,
                NULL AS EmailModerador,
                NULL AS NombreModerador
            FROM OP_ProgramacionCampo
            WHERE CAST(FechaProgramada AS DATE) = @Fecha
            ORDER BY FechaProgramada",
            new { Fecha = fecha.Date });

        return programaciones.ToList();
    }

    #endregion

    #region Generadores de email (templates inline)

    private string GenerarEmailProgramacionCreada(ProgramacionData prog)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #007bff; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
        .content {{ background-color: #f8f9fa; padding: 20px; border-radius: 0 0 5px 5px; }}
        .info-block {{ margin: 15px 0; padding: 10px; background-color: white; border-left: 4px solid #007bff; }}
        .label {{ font-weight: bold; color: #555; }}
        .value {{ color: #333; margin-top: 5px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Nueva Programación de Sesión</h2>
        </div>
        <div class='content'>
            <p>Estimado/a {prog.NombreEntrevistado},</p>
            <p>Se ha programado una nueva sesión para su participación. Detalles:</p>
            
            <div class='info-block'>
                <div class='label'>Tipo de Sesión:</div>
                <div class='value'>{prog.TipoSesion}</div>
            </div>
            
            <div class='info-block'>
                <div class='label'>Fecha y Hora:</div>
                <div class='value'>{prog.FechaProgramada?.ToString("dddd, dd/MM/yyyy HH:mm")}</div>
            </div>
            
            {(!string.IsNullOrEmpty(prog.NombreModerador) ? $@"
            <div class='info-block'>
                <div class='label'>Moderador:</div>
                <div class='value'>{prog.NombreModerador}</div>
            </div>" : "")}
            
            <p style='margin-top: 20px; color: #666;'>
                Por favor, confirme su asistencia respondiendo a este correo o contactando directamente.<br>
                <strong>¡Gracias por su participación!</strong>
            </p>
        </div>
    </div>
</body>
</html>";
    }

    private string GenerarEmailRecordatorio(ProgramacionData prog)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #ffc107; color: #333; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
        .alert {{ background-color: #fff3cd; padding: 15px; border-left: 4px solid #ffc107; margin: 15px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>🔔 RECORDATORIO: Su sesión es MAÑANA</h2>
        </div>
        <div class='alert'>
            <p>Estimado/a {prog.NombreEntrevistado},</p>
            <p><strong>¡Recordatorio importante!</strong> Mañana tiene programada una sesión:</p>
            <p><strong>Hora:</strong> {prog.FechaProgramada?.ToString("HH:mm")}</p>
            {(!string.IsNullOrEmpty(prog.NombreModerador) ? $"<p><strong>Moderador:</strong> {prog.NombreModerador}</p>" : "")}
            <p><strong>Tipo:</strong> {prog.TipoSesion}</p>
            <p style='color: #856404;'><strong>Por favor, llegue 10 minutos antes.</strong></p>
        </div>
    </div>
</body>
</html>";
    }

    private string GenerarEmailCambioEstado(ProgramacionData prog, string estadoAnterior, string estadoNuevo)
    {
        var colorEstado = estadoNuevo switch
        {
            "Confirmado" => "#28a745",
            "Cancelado" => "#dc3545",
            "Reprogramado" => "#fd7e14",
            "Completado" => "#20c997",
            _ => "#6c757d"
        };

        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: {colorEstado}; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
        .status-badge {{ font-size: 24px; font-weight: bold; color: {colorEstado}; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Cambio de Estado - Sesión Programada</h2>
        </div>
        <div style='padding: 20px;'>
            <p>Estimado/a {prog.NombreEntrevistado},</p>
            <p>El estado de su programación ha cambiado:</p>
            
            <div class='status-badge'>{estadoNuevo}</div>
            
            <p><strong>Tipo:</strong> {prog.TipoSesion}</p>
            <p><strong>Fecha:</strong> {prog.FechaProgramada?.ToString("dd/MM/yyyy HH:mm")}</p>
            
            {(estadoNuevo == "Cancelado" ? "<p style='color: #dc3545;'>La sesión ha sido cancelada. Nos disculpamos por las molestias.</p>" : "")}
            {(estadoNuevo == "Reprogramado" ? "<p style='color: #fd7e14;'>Su sesión será reprogramada. Le comunicaremos la nueva fecha pronto.</p>" : "")}
        </div>
    </div>
</body>
</html>";
    }

    private string GenerarEmailFichaCompletada(FichaData ficha)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #28a745; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>✅ Ficha Técnica Completada</h2>
        </div>
        <div style='padding: 20px; background-color: #f8f9fa;'>
            <p>Se ha completado la ficha técnica para el trabajo {ficha.TrabajoId}.</p>
            <p><strong>Fecha de registro:</strong> {ficha.FechaCreacion?.ToString("dd/MM/yyyy")}</p>
            <p>Gracias por su participación en nuestro proyecto.</p>
        </div>
    </div>
</body>
</html>";
    }

    private string GenerarEmailAsignacionModerador(ProgramacionData prog)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #17a2b8; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
        .details {{ background-color: #f8f9fa; padding: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Nueva Asignación de Sesión</h2>
        </div>
        <div class='details'>
            <p>Se le ha asignado una nueva sesión como moderador:</p>
            <p><strong>Tipo:</strong> {prog.TipoSesion}</p>
            <p><strong>Fecha y Hora:</strong> {prog.FechaProgramada?.ToString("dddd, dd/MM/yyyy HH:mm")}</p>
            <p><strong>Entrevistado:</strong> {prog.NombreEntrevistado}</p>
            <p>Por favor, verifique su disponibilidad e inicie la preparación de la sesión.</p>
        </div>
    </div>
</body>
</html>";
    }

    private string GenerarEmailReporteDiario(List<ProgramacionData> programaciones, DateTime fecha)
    {
        var filas = string.Join("", programaciones.Select((p, i) =>
            $"<tr style='border-bottom: 1px solid #ddd;'><td style='padding: 10px;'>{i + 1}</td><td style='padding: 10px;'>{p.FechaProgramada?.ToString("HH:mm")}</td><td style='padding: 10px;'>{p.TipoSesion}</td></tr>"));

        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; color: #333; }}
        .container {{ max-width: 700px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #007bff; color: white; padding: 20px; text-align: center; }}
        table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
        th {{ background-color: #007bff; color: white; padding: 10px; text-align: left; }}
        td {{ padding: 10px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Reporte Diario de Programaciones</h2>
            <p>{fecha:dddd, dd/MM/yyyy}</p>
        </div>
        <p>Total de sesiones programadas: <strong>{programaciones.Count}</strong></p>
        <table>
            <thead>
                <tr><th>#</th><th>Hora</th><th>Tipo de Sesión</th></tr>
            </thead>
            <tbody>
                {filas}
            </tbody>
        </table>
    </div>
</body>
</html>";
    }

    #endregion
}
