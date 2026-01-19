using MatrixNext.Data.DTOs.RP;
using MatrixNext.Data.Services.RP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Areas.RP.Controllers;

/// <summary>
/// Controller para indicadores de calidad
/// Migrado de: WebMatrix/RP_Reportes/IndicadoresCalidad.aspx
/// SP: REP_Diligenciamiento_Esquema_Analisis, REP_Porcentaje_Diligenciamiento_Brief, REP_Envio_Propuestas_48Horas
/// </summary>
[Area("RP")]
[Authorize]
public class IndicadoresController : Controller
{
    private readonly IIndicadoresCalidadService _service;
    private readonly ILogger<IndicadoresController> _logger;

    public IndicadoresController(IIndicadoresCalidadService service, ILogger<IndicadoresController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Vista principal de indicadores de calidad
    /// GET: /RP/Indicadores/Calidad
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Calidad(short? año, short? mes, short? estado, string? usuario, int tipoReporte = 1)
    {
        try
        {
            var filtros = new IndicadoresCalidadFiltrosDto
            {
                Año = año ?? (short)DateTime.Now.Year,
                Mes = mes,
                Estado = estado,
                Usuario = usuario,
                TipoReporte = tipoReporte
            };

            var viewModel = await _service.PrepararViewModelAsync(filtros);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar indicadores de calidad");
            TempData["Error"] = "Error al cargar los indicadores";
            return View(new IndicadoresCalidadViewModel());
        }
    }

    /// <summary>
    /// Actualizar indicadores con filtros via AJAX
    /// POST: /RP/Indicadores/Actualizar
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Actualizar([FromBody] IndicadoresCalidadFiltrosDto filtros)
    {
        try
        {
            if (filtros.TipoReporte < 1 || filtros.TipoReporte > 3)
            {
                return Json(new { success = false, message = "Debe seleccionar un tipo de reporte válido" });
            }

            if (!filtros.Año.HasValue)
            {
                return Json(new { success = false, message = "Debe seleccionar un año" });
            }

            var viewModel = await _service.PrepararViewModelAsync(filtros);
            
            return Json(new { 
                success = true, 
                resumen = filtros.TipoReporte switch
                {
                    1 => viewModel.ResumenEsquema as object,
                    2 => viewModel.ResumenBrief,
                    3 => viewModel.ResumenPropuestas,
                    _ => null
                },
                detalle = filtros.TipoReporte switch
                {
                    1 => viewModel.DetalleEsquema as object,
                    2 => viewModel.DetalleBrief,
                    3 => viewModel.DetallePropuestas,
                    _ => null
                },
                usuarios = viewModel.UsuariosDisponibles
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar indicadores");
            return Json(new { success = false, message = "Error al obtener los datos" });
        }
    }

    /// <summary>
    /// Exportar indicadores a Excel
    /// POST: /RP/Indicadores/Exportar
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Exportar([FromBody] IndicadoresCalidadFiltrosDto filtros)
    {
        try
        {
            var bytes = await _service.ExportarExcelAsync(filtros);
            var nombreReporte = filtros.TipoReporte switch
            {
                1 => "EsquemaAnalisis",
                2 => "DiligenciamientoBrief",
                3 => "Propuestas48Horas",
                _ => "Indicadores"
            };
            var fileName = $"Indicadores_{nombreReporte}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al exportar indicadores");
            return BadRequest(new { success = false, message = "Error al exportar" });
        }
    }
}
