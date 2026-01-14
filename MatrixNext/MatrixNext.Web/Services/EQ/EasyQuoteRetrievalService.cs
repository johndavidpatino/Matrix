using Microsoft.EntityFrameworkCore;
using MatrixNext.Web.DTOs;
using MatrixNext.Web.Services.EQ.Adapters;
using MatrixNext.Web.Models.EQ;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Areas.EQ.Models;

namespace MatrixNext.Web.Services.EQ;

/// <summary>
/// Servicio para consulta/retrieval de quotes desde EF Core
/// </summary>
public class EasyQuoteRetrievalService
{
    private readonly MatrixDbContext _context;
    private readonly ILogger<EasyQuoteRetrievalService> _logger;

    public EasyQuoteRetrievalService(
        MatrixDbContext context,
        ILogger<EasyQuoteRetrievalService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene una quote por ID con todas sus relaciones
    /// </summary>
    public async Task<EasyQuoteViewModel?> GetQuoteByIdAsync(long id)
    {
        var entity = await _context.EqQuoteHeaders
            .Include(q => q.Questionnaires)
            .Include(q => q.Methodologies)
            .Include(q => q.SampleCities)
            .Include(q => q.StaffSL)
            .Include(q => q.Mysteries)
            .Include(q => q.CostResult)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (entity == null)
        {
            _logger.LogWarning("Quote {QuoteId} no encontrada", id);
            return null;
        }

        var adapter = new QuoteHeaderToViewModelAdapter();
        return adapter.ToViewModel(entity);
    }

    /// <summary>
    /// Lista todas las quotes con paginación
    /// </summary>
    public async Task<List<EQQuoteHeaderSummary>> GetQuoteSummariesAsync(
        int pageNumber = 1, 
        int pageSize = 50)
    {
        var skip = (pageNumber - 1) * pageSize;

        var summaries = await _context.EqQuoteHeaders
            .OrderByDescending(q => q.FechaCreacion)
            .Skip(skip)
            .Take(pageSize)
                .Select(q => new EQQuoteHeaderSummary
                {
                    Id = q.Id,
                    PropuestaNombre = q.PropuestaNombre ?? string.Empty,
                    Cliente = q.Cliente ?? string.Empty,
                    SL = q.SL ?? string.Empty,
                    FechaCreacion = q.FechaCreacion,
                    FechaModificacion = q.FechaModificacion,
                    GrupoObjetivo = q.GrupoObjetivo ?? string.Empty
                })
            .ToListAsync();

        return summaries;
    }

    /// <summary>
    /// Busca quotes por criterios
    /// </summary>
    public async Task<List<EQQuoteHeaderSummary>> SearchQuotesAsync(
        string? cliente = null,
        string? sl = null,
        string? propuesta = null,
        DateTime? fechaDesde = null,
        DateTime? fechaHasta = null)
    {
        var query = _context.EqQuoteHeaders.AsQueryable();

        if (!string.IsNullOrWhiteSpace(cliente))
        {
            var clienteFilter = cliente!;
            query = query.Where(q => (q.Cliente ?? string.Empty).Contains(clienteFilter));
        }

        if (!string.IsNullOrWhiteSpace(sl))
        {
            var slFilter = sl!;
            query = query.Where(q => (q.SL ?? string.Empty).Contains(slFilter));
        }

        if (!string.IsNullOrWhiteSpace(propuesta))
        {
            var propuestaFilter = propuesta!;
            query = query.Where(q => (q.PropuestaNombre ?? string.Empty).Contains(propuestaFilter));
        }

        if (fechaDesde.HasValue)
            query = query.Where(q => q.FechaCreacion >= fechaDesde.Value);

        if (fechaHasta.HasValue)
            query = query.Where(q => q.FechaCreacion <= fechaHasta.Value);

        var summaries = await query
            .OrderByDescending(q => q.FechaCreacion)
            .Take(100) // Límite de seguridad
            .Select(q => new EQQuoteHeaderSummary
            {
                Id = q.Id,
                PropuestaNombre = q.PropuestaNombre ?? string.Empty,
                Cliente = q.Cliente ?? string.Empty,
                SL = q.SL ?? string.Empty,
                FechaCreacion = q.FechaCreacion,
                FechaModificacion = q.FechaModificacion,
                GrupoObjetivo = q.GrupoObjetivo ?? string.Empty
            })
            .ToListAsync();

        return summaries;
    }

    /// <summary>
    /// Cuenta total de quotes en el sistema
    /// </summary>
    public async Task<int> GetTotalQuotesCountAsync()
    {
        return await _context.EqQuoteHeaders.CountAsync();
    }

    /// <summary>
    /// Verifica si una quote existe
    /// </summary>
    public async Task<bool> QuoteExistsAsync(long id)
    {
        return await _context.EqQuoteHeaders.AnyAsync(q => q.Id == id);
    }
}

/// <summary>
/// DTO para resumen de quotes
/// </summary>
public class EQQuoteHeaderSummary
{
    public long Id { get; set; }
    public string PropuestaNombre { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty;
    public string SL { get; set; } = string.Empty;
    public string GrupoObjetivo { get; set; } = string.Empty;
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
}
