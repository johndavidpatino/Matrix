using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MatrixNext.Web.Models.EQ;
using MatrixNext.Web.DTOs;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Areas.EQ.Services.Internal;
using MatrixNext.Web.Services.EQ.Adapters;
using MatrixNext.Web.Areas.EQ.Models;
using MatrixNext.Data.Services;

namespace MatrixNext.Web.Services.EQ
{
    /// <summary>
    /// Servicio EF Core para cálculos de costos EasyQuote
    /// Orquesta el motor de cálculos (QuoteCalculator) con persistencia EF
    /// </summary>
    public class EasyCostService : IEasyCostService
    {
        private readonly MatrixDbContext _context;
        private readonly QuoteCalculator _calculator;
        private readonly ILogger<EasyCostService> _logger;

        public EasyCostService(
            MatrixDbContext context, 
            QuoteCalculator calculator,
            ILogger<EasyCostService> logger)
        {
            _context = context;
            _calculator = calculator;
            _logger = logger;
        }

        // ===== MÉTODOS PARA CONTROLLERS (FASE 4) =====

        /// <summary>
        /// Calcula costos sin persistir (solo cálculo)
        /// </summary>
        public EQSummary CalculateCost(EasyQuoteViewModel vm, DateTime? fechaCotizacion = null)
        {
            return _calculator.Calcular(vm, fechaCotizacion);
        }

        /// <summary>
        /// Guarda quote completa con cálculo de costos
        /// </summary>
        public async Task<Controllers.Api.SaveQuoteResult> SaveQuoteWithCostAsync(EasyQuoteViewModel vm, DateTime? fechaCotizacion = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // PASO 1: Calcular costos
                var summary = _calculator.Calcular(vm, fechaCotizacion);

                // PASO 2: Mapear ViewModel → Entity
                var adapter = new QuoteHeaderToViewModelAdapter();
                var entity = adapter.ToEntity(vm);
                entity.FechaModificacion = DateTime.Now;
                
                if (entity.Id == 0)
                {
                    entity.FechaCreacion = DateTime.Now;
                    _context.EqQuoteHeaders.Add(entity);
                }
                else
                {
                    _context.EqQuoteHeaders.Update(entity);
                }

                await _context.SaveChangesAsync();

                // PASO 3: Guardar resultado de costos
                var costResult = new EqCostResult
                {
                    QuoteHeaderId = entity.Id,
                    Moneda = "COP",
                    FechaCalculo = DateTime.UtcNow,
                    FechaModificacion = DateTime.UtcNow,
                    CostoCampo = summary.CostoCampo,
                    CostoCalidad = summary.CostoCalidad,
                    Viaticos = summary.Viaticos,
                    Incentivos = summary.Incentivos,
                    Insumos = summary.Insumos,
                    StaffOps = summary.StaffOps,
                    CompraProducto = summary.CompraProducto,
                    Tablets = summary.Tablets,
                    DirectCostOps = summary.DirectCostOps,
                    GM = summary.GM,
                    PB_RMF = summary.PB_RMF,
                    ProfTime = summary.ProfTime,
                    OP = summary.OP,
                    AOTTotal = summary.AOT,
                    PctOP = summary.PorcOP
                };

                _context.EqCostResults.Add(costResult);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation("Quote {QuoteId} guardada exitosamente con costos", entity.Id);

                return new Controllers.Api.SaveQuoteResult
                {
                    QuoteId = entity.Id,
                    Summary = summary,
                    SavedAt = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error guardando quote");
                throw;
            }
        }

        public async Task<ApiResponse<EasyCostResultDto>> CalculateAsync(int quoteHeaderId)
        {
            try
            {
                // Cargar cotización con todos sus detalles (requerido por adapter y motor)
                var quote = await _context.EqQuoteHeaders
                    .Include(q => q.Questionnaires)
                    .Include(q => q.Methodologies)
                    .Include(q => q.SampleCities)
                    .Include(q => q.Mysteries)
                    .Include(q => q.StaffSL)
                    .FirstOrDefaultAsync(q => q.Id == quoteHeaderId);

                if (quote == null)
                {
                    return new ApiResponse<EasyCostResultDto>
                    {
                        Success = false,
                        Message = "Cotización no encontrada"
                    };
                }

                // PASO 1: Convertir EqQuoteHeader → EasyQuoteViewModel usando adapter
                var adapter = new QuoteHeaderToViewModelAdapter();
                var vm = adapter.ToViewModel(quote);

                // PASO 2: Ejecutar motor de cálculos (26 fórmulas)
                var summary = _calculator.Calcular(vm);

                // PASO 3: Persistir resultado en BD
                var costResult = new EqCostResult
                {
                    QuoteHeaderId = quoteHeaderId,
                    Moneda = "COP",
                    FechaCalculo = DateTime.UtcNow,
                    FechaModificacion = DateTime.UtcNow,
                    // Mapeo directo desde EQSummary (resultado del motor)
                    CostoCampo = summary.CostoCampo,
                    CostoCalidad = summary.CostoCalidad,
                    Viaticos = summary.Viaticos,
                    Incentivos = summary.Incentivos,
                    Insumos = summary.Insumos,
                    StaffOps = summary.StaffOps,
                    CompraProducto = summary.CompraProducto,
                    Tablets = summary.Tablets,
                    DirectCostOps = summary.DirectCostOps,
                    GM = summary.GM,
                    PB_RMF = summary.PB_RMF,
                    ProfTime = summary.ProfTime,
                    OP = summary.OP,
                    AOTTotal = summary.AOT,
                    PctOP = summary.PorcOP
                };

                _context.EqCostResults.Add(costResult);
                await _context.SaveChangesAsync();

                return new ApiResponse<EasyCostResultDto>
                {
                    Success = true,
                    Message = "Cálculo completado exitosamente",
                    Data = MapToDto(costResult)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<EasyCostResultDto>
                {
                    Success = false,
                    Message = $"Error al calcular costos: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<EasyCostResultDto>> GetLastCalculationAsync(int quoteHeaderId)
        {
            try
            {
                var result = await _context.EqCostResults
                    .Where(r => r.QuoteHeaderId == quoteHeaderId)
                    .OrderByDescending(r => r.FechaCalculo)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return new ApiResponse<EasyCostResultDto>
                    {
                        Success = false,
                        Message = "No hay cálculos para esta cotización"
                    };
                }

                return new ApiResponse<EasyCostResultDto>
                {
                    Success = true,
                    Data = MapToDto(result)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<EasyCostResultDto>
                {
                    Success = false,
                    Message = $"Error al obtener cálculo: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> ValidateQuoteAsync(int quoteHeaderId)
        {
            try
            {
                var quote = await _context.EqQuoteHeaders
                    .Include(q => q.Questionnaires)
                    .Include(q => q.Methodologies)
                    .Include(q => q.SampleCities)
                    .FirstOrDefaultAsync(q => q.Id == quoteHeaderId);

                if (quote == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Cotización no encontrada"
                    };
                }

                var errors = new List<string>();

                // Validaciones
                if (string.IsNullOrEmpty(quote.PropuestaNombre))
                    errors.Add("Nombre de propuesta requerido");

                if (quote.Questionnaires == null || quote.Questionnaires.Count == 0)
                    errors.Add("Se requiere al menos un cuestionario");

                if (quote.Methodologies == null || quote.Methodologies.Count == 0)
                    errors.Add("Se requiere al menos una metodología");

                if (quote.SampleCities == null || quote.SampleCities.Count == 0)
                    errors.Add("Se requiere al menos una ciudad/muestra");

                if (errors.Any())
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = $"Validación fallida: {string.Join(", ", errors)}",
                        Data = false
                    };
                }

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Cotización válida para cálculo",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error al validar cotización: {ex.Message}"
                };
            }
        }

        // ===== HELPERS =====

        private EasyCostResultDto MapToDto(EqCostResult result)
        {
            return new EasyCostResultDto
            {
                Id = result.Id,
                QuoteHeaderId = result.QuoteHeaderId,
                Moneda = result.Moneda,
                CostoCampo = result.CostoCampo,
                CostoCalidad = result.CostoCalidad,
                Viaticos = result.Viaticos,
                Incentivos = result.Incentivos,
                Insumos = result.Insumos,
                StaffOps = result.StaffOps,
                Estadistica = result.Estadistica,
                Scripting = result.Scripting,
                DataCleaning = result.DataCleaning,
                Procesamiento = result.Procesamiento,
                CostoDirectoTotal = result.CostoDirectoTotal,
                DirectCostOps = result.DirectCostOps,
                GM = result.GM,
                PB_RMF = result.PB_RMF,
                OP = result.OP,
                PctOP = result.PctOP,
                AOTTotal = result.AOTTotal,
                FechaCalculo = result.FechaCalculo
            };
        }
    }
}
