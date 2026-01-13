using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MatrixNext.Web.Models.EQ;
using MatrixNext.Web.DTOs;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Areas.EQ.Services.Internal;
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

        public EasyCostService(MatrixDbContext context, QuoteCalculator calculator)
        {
            _context = context;
            _calculator = calculator;
        }

        public async Task<ApiResponse<EasyCostResultDto>> CalculateAsync(int quoteHeaderId)
        {
            try
            {
                // Cargar cotización con todos sus detalles
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

                // TODO FASE 3: Usar el motor de cálculos existente (QuoteCalculator)
                // Por ahora creamos un resultado vacío
                var costResult = new EqCostResult
                {
                    QuoteHeaderId = quoteHeaderId,
                    Moneda = "COP",
                    FechaCalculo = DateTime.UtcNow,
                    FechaModificacion = DateTime.UtcNow,
                    // Los costos serán calculados por el motor en FASE 3
                    CostoCampo = 0m,
                    CostoCalidad = 0m,
                    Viaticos = 0m,
                    Incentivos = 0m,
                    Insumos = 0m,
                    StaffOps = 0m,
                    GM = 0m,
                    PB_RMF = 0m,
                    OP = 0m,
                    AOTTotal = 0m
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
