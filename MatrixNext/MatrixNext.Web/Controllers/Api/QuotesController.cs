using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Web.DTOs;
using MatrixNext.Web.Services.EQ;
using MatrixNext.Web.Areas.EQ.Services.Internal;
using MatrixNext.Web.Areas.EQ.Models;

namespace MatrixNext.Web.Controllers.Api;

/// <summary>
/// API REST para gestión de cotizaciones EasyQuote
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class QuotesController : ControllerBase
{
    private readonly EasyCostService _costService;
    private readonly EasyQuoteRetrievalService _retrievalService;
    private readonly ILogger<QuotesController> _logger;

    public QuotesController(
        EasyCostService costService,
        EasyQuoteRetrievalService retrievalService,
        ILogger<QuotesController> logger)
    {
        _costService = costService;
        _retrievalService = retrievalService;
        _logger = logger;
    }

    /// <summary>
    /// GET /api/quotes
    /// Lista todas las quotes con paginación
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<EQQuoteHeaderSummary>), 200)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 50)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest("Parámetros de paginación inválidos");

        var summaries = await _retrievalService.GetQuoteSummariesAsync(page, pageSize);
        var totalCount = await _retrievalService.GetTotalQuotesCountAsync();

        return Ok(new
        {
            data = summaries,
            page,
            pageSize,
            totalCount,
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }

    /// <summary>
    /// GET /api/quotes/{id}
    /// Obtiene una quote por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EasyQuoteViewModel), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(long id)
    {
        var quote = await _retrievalService.GetQuoteByIdAsync(id);
        
        if (quote == null)
            return NotFound($"Quote {id} no encontrada");

        return Ok(quote);
    }

    /// <summary>
    /// POST /api/quotes/search
    /// Busca quotes por criterios
    /// </summary>
    [HttpPost("search")]
    [ProducesResponseType(typeof(List<EQQuoteHeaderSummary>), 200)]
    public async Task<IActionResult> Search([FromBody] QuoteSearchRequest request)
    {
        var results = await _retrievalService.SearchQuotesAsync(
            request.Cliente,
            request.SL,
            request.Propuesta,
            request.FechaDesde,
            request.FechaHasta
        );

        return Ok(results);
    }

    /// <summary>
    /// POST /api/quotes/calculate
    /// Calcula costos sin guardar
    /// </summary>
    [HttpPost("calculate")]
    [ProducesResponseType(typeof(EQSummary), 200)]
    public IActionResult Calculate([FromBody] EasyQuoteViewModel vm)
    {
        if (vm == null)
            return BadRequest("ViewModel vacío");

        try
        {
            var summary = _costService.CalculateCost(vm);
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculando quote");
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// POST /api/quotes
    /// Crea una nueva quote con cálculo de costos
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SaveQuoteResult), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] EasyQuoteViewModel vm)
    {
        if (vm == null)
            return BadRequest("ViewModel vacío");

        try
        {
            var result = await _costService.SaveQuoteWithCostAsync(vm);
            
            return CreatedAtAction(
                nameof(GetById), 
                new { id = result.QuoteId }, 
                result
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando quote");
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// PUT /api/quotes/{id}
    /// Actualiza una quote existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SaveQuoteResult), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(long id, [FromBody] EasyQuoteViewModel vm)
    {
        if (vm == null)
            return BadRequest("ViewModel vacío");

        var exists = await _retrievalService.QuoteExistsAsync(id);
        if (!exists)
            return NotFound($"Quote {id} no encontrada");

        try
        {
            // Asegurar que el ID del ViewModel coincida
            vm.Id = id;
            
            var result = await _costService.SaveQuoteWithCostAsync(vm);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando quote {QuoteId}", id);
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// DELETE /api/quotes/{id}
    /// Elimina (soft delete) una quote
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(long id)
    {
        var exists = await _retrievalService.QuoteExistsAsync(id);
        if (!exists)
            return NotFound($"Quote {id} no encontrada");

        // TODO: Implementar soft delete cuando se agregue campo IsDeleted al modelo
        _logger.LogWarning("Soft delete no implementado aún para Quote {QuoteId}", id);
        
        return NoContent();
    }
}

/// <summary>
/// Request DTO para búsqueda de quotes
/// </summary>
public class QuoteSearchRequest
{
    public string? Cliente { get; set; }
    public string? SL { get; set; }
    public string? Propuesta { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
}

/// <summary>
/// Response DTO para guardar quotes
/// </summary>
public class SaveQuoteResult
{
    public long QuoteId { get; set; }
    public EQSummary Summary { get; set; } = new();
    public DateTime SavedAt { get; set; } = DateTime.Now;
}
