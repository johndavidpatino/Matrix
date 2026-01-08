using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.OP.Models;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Servicio para gestión de coordinación y asignación de personal
/// </summary>
public class OpCoordinacionService : IOpCoordinacionService
{
    private readonly MatrixDbContext _db;
    private readonly ILogger<OpCoordinacionService> _logger;

    public OpCoordinacionService(MatrixDbContext db, ILogger<OpCoordinacionService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<List<TrabajoCoordinadorDto>> ObtenerTrabajosPorCoordinadorAsync(long coordinadorId, long? trabajoId = null, string? nombre = null, string? jobBook = null, int? estado = null)
    {
        try
        {
            // Llamar a SP GestionTrabajosOP.ListaTrabajosXCoordinador
            var query = @"
                SELECT t.Id, t.JobBook, t.Nombre, t.Estado, m.MetNombre as Metodologia, t.IdProyecto
                FROM PY_Trabajo t
                LEFT JOIN OP_Metodologias m ON t.IdMetodologia = m.id
                WHERE t.IdCoordinador = {0}
                AND (@trabajoId IS NULL OR t.Id = @trabajoId)
                AND (@nombre IS NULL OR t.Nombre LIKE '%' + @nombre + '%')
                AND (@jobBook IS NULL OR t.JobBook LIKE '%' + @jobBook + '%')
                AND (@estado IS NULL OR t.Estado = @estado)
                ORDER BY t.Id DESC";

            var trabajos = await _db.Database
                .SqlQueryRaw<TrabajoCoordinadorDto>(query, coordinadorId)
                .ToListAsync();

            return trabajos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener trabajos del coordinador {CoordinadorId}", coordinadorId);
            return new List<TrabajoCoordinadorDto>();
        }
    }

    public async Task<List<TrabajoCoordinadorDto>> ObtenerTrabajosCallCenterAsync(long? trabajoId = null, string? nombre = null, string? jobBook = null, int? estado = null)
    {
        try
        {
            // Llamar a SP GestionTrabajosOP.ListaTrabajosCallCenter
            var query = @"
                SELECT t.Id, t.JobBook, t.Nombre, t.Estado, m.MetNombre as Metodologia, t.IdProyecto
                FROM PY_Trabajo t
                LEFT JOIN OP_Metodologias m ON t.IdMetodologia = m.id
                LEFT JOIN FichaCuantitativo fc ON t.Id = fc.IdTrabajo
                WHERE fc.TipoRecoleccionId IN (1, 4) -- CATI o CAWI
                AND (@trabajoId IS NULL OR t.Id = @trabajoId)
                AND (@nombre IS NULL OR t.Nombre LIKE '%' + @nombre + '%')
                AND (@jobBook IS NULL OR t.JobBook LIKE '%' + @jobBook + '%')
                AND (@estado IS NULL OR t.Estado = @estado)
                ORDER BY t.Id DESC";

            var trabajos = await _db.Database
                .SqlQueryRaw<TrabajoCoordinadorDto>(query)
                .ToListAsync();

            return trabajos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener trabajos de Call Center");
            return new List<TrabajoCoordinadorDto>();
        }
    }

    public async Task<List<CiudadAsignadaDto>> ObtenerCiudadesAsignadasAsync(long coordinadorId, long trabajoId)
    {
        try
        {
            // Llamar a SP CoordinacionCampo.ObtenerMuestraxCoordinadoryTrabajo
            var query = @"
                SELECT m.Id, m.CiudadId, d.DivMuniNombre as Ciudad, m.Cantidad as Muestra
                FROM CoordinacionCampo_Muestra m
                INNER JOIN C_Divipola d ON m.CiudadId = d.DivMuniCodigo
                WHERE m.IdTrabajo = {0} AND m.CoordinadorId = {1}";

            var ciudades = await _db.Database
                .SqlQueryRaw<CiudadAsignadaDto>(query, trabajoId, coordinadorId)
                .ToListAsync();

            return ciudades;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ciudades asignadas para trabajo {TrabajoId}", trabajoId);
            return new List<CiudadAsignadaDto>();
        }
    }

    public async Task<List<PersonalAsignadoDto>> ObtenerPersonalAsignadoAsync(long trabajoId, int? ciudadId = null)
    {
        try
        {
            // Llamar a SP CoordinacionCampoPersonal.ObtenerPersonalAsignado
            var query = @"
                SELECT 
                    pa.Id, 
                    pa.PersonaId, 
                    p.Nombres + ' ' + p.Apellidos as Nombre,
                    c.Cargo,
                    d.DivMuniNombre as Ciudad
                FROM CoordinacionCampo_PersonalAsignado pa
                INNER JOIN TH_Personas p ON pa.PersonaId = p.Id
                INNER JOIN TH_Cargos c ON p.CargoId = c.Id
                LEFT JOIN C_Divipola d ON p.CiudadId = d.DivMuniCodigo
                WHERE pa.IdTrabajo = {0}
                AND (@ciudadId IS NULL OR p.CiudadId = @ciudadId)";

            var personal = await _db.Database
                .SqlQueryRaw<PersonalAsignadoDto>(query, trabajoId)
                .ToListAsync();

            return personal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener personal asignado para trabajo {TrabajoId}", trabajoId);
            return new List<PersonalAsignadoDto>();
        }
    }

    public async Task<List<PersonalDisponibleDto>> ObtenerPersonalDisponibleAsync(long trabajoId, int? ciudadId = null)
    {
        try
        {
            // Llamar a SP CoordinacionCampoPersonal.ObtenerPersonalSinAsignar
            var query = @"
                SELECT 
                    p.Id,
                    p.Nombres + ' ' + p.Apellidos as Nombre,
                    c.Cargo,
                    te.Tipo,
                    tc.Tipo as Contratacion,
                    d.DivMuniNombre as Ciudad
                FROM TH_Personas p
                INNER JOIN TH_Cargos c ON p.CargoId = c.Id
                LEFT JOIN OP_Encuestadores e ON p.Id = e.PersonaId
                LEFT JOIN OP_TipoEncuestador te ON e.TipoEncuestadorId = te.Id
                LEFT JOIN TH_TipoContratacion tc ON p.TipoContratacionId = tc.Id
                LEFT JOIN C_Divipola d ON p.CiudadId = d.DivMuniCodigo
                WHERE p.Id NOT IN (
                    SELECT PersonaId FROM CoordinacionCampo_PersonalAsignado WHERE IdTrabajo = {0}
                )
                AND p.Activo = 1
                AND (@ciudadId IS NULL OR p.CiudadId = @ciudadId)";

            var personal = await _db.Database
                .SqlQueryRaw<PersonalDisponibleDto>(query, trabajoId)
                .ToListAsync();

            return personal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener personal disponible para trabajo {TrabajoId}", trabajoId);
            return new List<PersonalDisponibleDto>();
        }
    }

    public async Task<bool> AsignarPersonalAsync(long trabajoId, long personalId, int? ciudadId, long usuarioId)
    {
        try
        {
            await _db.Database.ExecuteSqlRawAsync(@"
                INSERT INTO CoordinacionCampo_PersonalAsignado (IdTrabajo, PersonaId, CiudadId, FechaAsignacion, AsignadoPor)
                VALUES ({0}, {1}, {2}, GETDATE(), {3})",
                trabajoId, personalId, ciudadId, usuarioId);

            _logger.LogInformation("Personal {PersonalId} asignado a trabajo {TrabajoId} por usuario {UsuarioId}", personalId, trabajoId, usuarioId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al asignar personal {PersonalId} a trabajo {TrabajoId}", personalId, trabajoId);
            return false;
        }
    }

    public async Task<bool> RetirarPersonalAsync(long asignacionId, long usuarioId)
    {
        try
        {
            await _db.Database.ExecuteSqlRawAsync(@"
                DELETE FROM CoordinacionCampo_PersonalAsignado WHERE Id = {0}",
                asignacionId);

            _logger.LogInformation("Asignación {AsignacionId} eliminada por usuario {UsuarioId}", asignacionId, usuarioId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al retirar asignación {AsignacionId}", asignacionId);
            return false;
        }
    }
}
