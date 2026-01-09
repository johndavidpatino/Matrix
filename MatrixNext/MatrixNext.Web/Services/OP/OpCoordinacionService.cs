using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.OP.Models;
using Microsoft.EntityFrameworkCore;
using System;

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
                SELECT t.id AS Id, t.JobBook, t.NombreTrabajo AS Nombre, t.Estado, m.MetNombre as Metodologia, t.ProyectoId AS IdProyecto
                FROM PY_Trabajo t
                LEFT JOIN OP_Metodologias m ON t.OP_MetodologiaId = m.Id
                WHERE t.COE = {0}
                AND ({1} IS NULL OR t.id = {1})
                AND ({2} IS NULL OR t.NombreTrabajo LIKE '%' + {2} + '%')
                AND ({3} IS NULL OR t.JobBook LIKE '%' + {3} + '%')
                AND ({4} IS NULL OR t.Estado = {4})
                ORDER BY t.id DESC";

            var trabajos = await _db.Database
                .SqlQueryRaw<TrabajoCoordinadorDto>(query, coordinadorId, DbValue(trabajoId), DbValue(nombre), DbValue(jobBook), DbValue(estado))
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
                SELECT t.id AS Id, t.JobBook, t.NombreTrabajo AS Nombre, t.Estado, m.MetNombre as Metodologia, t.ProyectoId AS IdProyecto
                FROM PY_Trabajo t
                LEFT JOIN OP_Metodologias m ON t.OP_MetodologiaId = m.Id
                WHERE t.TipoRecoleccionId IN (1, 4) -- CATI o CAWI
                AND ({0} IS NULL OR t.id = {0})
                AND ({1} IS NULL OR t.NombreTrabajo LIKE '%' + {1} + '%')
                AND ({2} IS NULL OR t.JobBook LIKE '%' + {2} + '%')
                AND ({3} IS NULL OR t.Estado = {3})
                ORDER BY t.id DESC";

            var trabajos = await _db.Database
                .SqlQueryRaw<TrabajoCoordinadorDto>(query, DbValue(trabajoId), DbValue(nombre), DbValue(jobBook), DbValue(estado))
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
                SELECT m.Id, m.CiudadId, d.DivMuniNombre as Ciudad, CAST(m.Cantidad AS int) as Muestra
                FROM OP_MuestraTrabajos m
                INNER JOIN C_Divipola d ON m.CiudadId = d.DivMuniCodigo
                WHERE m.TrabajoId = {0} AND m.Coordinador = {1}";

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
                    pa.Persona as PersonaId, 
                    p.Nombres + ' ' + p.Apellidos as Nombre,
                    c.Cargo,
                    d.DivMuniNombre as Ciudad
                FROM OP_PersonasAsignadasTrabajo pa
                INNER JOIN TH_Personas p ON pa.Persona = p.Id
                INNER JOIN TH_Cargos c ON p.CargoId = c.Id
                LEFT JOIN C_Divipola d ON pa.Ciudad = d.DivMuniCodigo
                WHERE pa.TrabajoId = {0}
                AND ({1} IS NULL OR pa.Ciudad = {1})";

            var personal = await _db.Database
                .SqlQueryRaw<PersonalAsignadoDto>(query, trabajoId, DbValue(ciudadId))
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
                LEFT JOIN OP_Encuestadores e ON p.Id = e.id
                LEFT JOIN OP_TipoEncuestador te ON e.TipoId = te.id
                LEFT JOIN TH_TipoContratacion tc ON p.TipoContratacionId = tc.Id
                LEFT JOIN C_Divipola d ON p.CiudadId = d.DivMuniCodigo
                WHERE p.Id NOT IN (
                    SELECT Persona FROM OP_PersonasAsignadasTrabajo WHERE TrabajoId = {0}
                )
                AND p.Activo = 1
                AND ({1} IS NULL OR p.CiudadId = {1})";

            var personal = await _db.Database
                .SqlQueryRaw<PersonalDisponibleDto>(query, trabajoId, DbValue(ciudadId))
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
                INSERT INTO OP_PersonasAsignadasTrabajo (TrabajoId, Persona, Ciudad, Fecha)
                VALUES ({0}, {1}, {2}, GETDATE())",
                trabajoId, personalId, DbValue(ciudadId));

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
                DELETE FROM OP_PersonasAsignadasTrabajo WHERE Id = {0}",
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
    private static object DbValue(object? value) => value ?? DBNull.Value;
}
