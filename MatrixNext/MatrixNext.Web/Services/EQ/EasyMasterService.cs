using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MatrixNext.Web.Models.EQ;
using MatrixNext.Web.DTOs;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Data.Services;

namespace MatrixNext.Web.Services.EQ
{
    /// <summary>
    /// Servicio EF Core para acceso a datos maestros/referenciales EasyQuote
    /// Incluye caching en memoria para optimizar performance
    /// </summary>
    public class EasyMasterService : IEasyMasterService
    {
        private readonly MatrixDbContext _context;
        private readonly IMemoryCache _cache;

        public EasyMasterService(MatrixDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<ApiResponse<List<EasyMasterPrecioDto>>> GetPrecioMatrizAsync(string tipoMetodologia)
        {
            try
            {
                string cacheKey = $"eq_precio_matriz_{tipoMetodologia}";
                if (_cache.TryGetValue(cacheKey, out List<EasyMasterPrecioDto> cached))
                {
                    return new ApiResponse<List<EasyMasterPrecioDto>>
                    {
                        Success = true,
                        Data = cached,
                        Message = "Datos desde caché"
                    };
                }

                var precios = await _context.EqParamPrecios
                    .Where(p => p.TipoMetodologia == tipoMetodologia)
                    .OrderBy(p => p.DuracionMin)
                    .Select(p => new EasyMasterPrecioDto
                    {
                        Id = p.Id,
                        TipoMetodologia = p.TipoMetodologia,
                        PenetracionRango = p.PenetracionRango,
                        DuracionMin = p.DuracionMin,
                        ValorTotal = p.ValorTotal
                    })
                    .ToListAsync();

                // Cachear por 30 minutos
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(cacheKey, precios, cacheOptions);

                return new ApiResponse<List<EasyMasterPrecioDto>>
                {
                    Success = true,
                    Data = precios
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<EasyMasterPrecioDto>>
                {
                    Success = false,
                    Message = $"Error al obtener matriz de precios: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<EasyMasterScriptProcDto>> GetHorasByDuracionAsync(int duracionMin)
        {
            try
            {
                var horas = await _context.EqParamScriptProcs
                    .FirstOrDefaultAsync(h => h.DuracionMin == duracionMin);

                if (horas == null)
                {
                    return new ApiResponse<EasyMasterScriptProcDto>
                    {
                        Success = false,
                        Message = "Registro no encontrado"
                    };
                }

                return new ApiResponse<EasyMasterScriptProcDto>
                {
                    Success = true,
                    Data = new EasyMasterScriptProcDto
                    {
                        DuracionMin = horas.DuracionMin,
                        HorasScript = horas.HorasScript,
                        HorasProc = horas.HorasProc,
                        HorasHarmoni = horas.HorasHarmoni,
                        HorasGraficacion = horas.HorasGraficacion
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<EasyMasterScriptProcDto>
                {
                    Success = false,
                    Message = $"Error al obtener horas: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<List<EasyMasterValorHoraDto>>> GetValorHoraOpsAsync()
        {
            try
            {
                const string cacheKey = "eq_valor_hora_ops";
                if (_cache.TryGetValue(cacheKey, out List<EasyMasterValorHoraDto> cached))
                {
                    return new ApiResponse<List<EasyMasterValorHoraDto>>
                    {
                        Success = true,
                        Data = cached
                    };
                }

                var tarifas = await _context.EqValorHoraOps
                    .OrderBy(t => t.Nivel)
                    .Select(t => new EasyMasterValorHoraDto
                    {
                        Nivel = t.Nivel,
                        Alternativa = t.Alternativa,
                        BaseCostRate = t.BaseCostRate,
                        LoadedCostRate = t.LoadedCostRate,
                        BillingRate = t.BillingRate
                    })
                    .ToListAsync();

                // Cachear por 30 minutos
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(cacheKey, tarifas, cacheOptions);

                return new ApiResponse<List<EasyMasterValorHoraDto>>
                {
                    Success = true,
                    Data = tarifas
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<EasyMasterValorHoraDto>>
                {
                    Success = false,
                    Message = $"Error al obtener tarifas horas: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<EasyMasterCostInsumosDto>> GetCostInsumosAsync(int nse)
        {
            try
            {
                var insumo = await _context.EqCostInsumos
                    .FirstOrDefaultAsync(i => i.NSE == nse);

                if (insumo == null)
                {
                    return new ApiResponse<EasyMasterCostInsumosDto>
                    {
                        Success = false,
                        Message = "Registro no encontrado"
                    };
                }

                return new ApiResponse<EasyMasterCostInsumosDto>
                {
                    Success = true,
                    Data = new EasyMasterCostInsumosDto
                    {
                        NSE = insumo.NSE,
                        Reclutamiento = insumo.Reclutamiento,
                        Obsequio = insumo.Obsequio,
                        Productividad = insumo.Productividad,
                        TransporteEncuestador = insumo.TransporteEncuestador
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<EasyMasterCostInsumosDto>
                {
                    Success = false,
                    Message = $"Error al obtener costos insumos: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<List<EasyMasterRateEstadisticaDto>>> GetRateEstadisticaAsync()
        {
            try
            {
                const string cacheKey = "eq_rate_estadistica";
                if (_cache.TryGetValue(cacheKey, out List<EasyMasterRateEstadisticaDto> cached))
                {
                    return new ApiResponse<List<EasyMasterRateEstadisticaDto>>
                    {
                        Success = true,
                        Data = cached
                    };
                }

                var rates = await _context.EqRateEstadisticas
                    .OrderBy(r => r.Categoria)
                    .Select(r => new EasyMasterRateEstadisticaDto
                    {
                        Id = r.Id,
                        Categoria = r.Categoria,
                        Servicio = r.Servicio,
                        PrecioRef2024 = r.PrecioRef2024
                    })
                    .ToListAsync();

                // Cachear por 30 minutos
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(cacheKey, rates, cacheOptions);

                return new ApiResponse<List<EasyMasterRateEstadisticaDto>>
                {
                    Success = true,
                    Data = rates
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<EasyMasterRateEstadisticaDto>>
                {
                    Success = false,
                    Message = $"Error al obtener rates estadística: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<List<EasyMasterLocacionesDto>>> GetLocacionesAsync()
        {
            try
            {
                const string cacheKey = "eq_locaciones";
                if (_cache.TryGetValue(cacheKey, out List<EasyMasterLocacionesDto> cached))
                {
                    return new ApiResponse<List<EasyMasterLocacionesDto>>
                    {
                        Success = true,
                        Data = cached
                    };
                }

                var locaciones = await _context.EqLocaciones
                    .OrderBy(l => l.Ciudad)
                    .Select(l => new EasyMasterLocacionesDto
                    {
                        Id = l.Id,
                        Ciudad = l.Ciudad,
                        TarifaBase = l.TarifaBase
                    })
                    .ToListAsync();

                // Cachear por 30 minutos
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(cacheKey, locaciones, cacheOptions);

                return new ApiResponse<List<EasyMasterLocacionesDto>>
                {
                    Success = true,
                    Data = locaciones
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<EasyMasterLocacionesDto>>
                {
                    Success = false,
                    Message = $"Error al obtener locaciones: {ex.Message}"
                };
            }
        }
    }
}
