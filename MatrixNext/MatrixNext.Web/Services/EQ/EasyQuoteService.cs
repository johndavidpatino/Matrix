using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MatrixNext.Web.Models.EQ;
using MatrixNext.Web.DTOs;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Data.Services;

namespace MatrixNext.Web.Services.EQ
{
    /// <summary>
    /// Servicio EF Core para operaciones CRUD de cotizaciones EasyQuote
    /// Reemplaza la anterior implementación Dapper (EasyQuoteAdapter)
    /// </summary>
    public class EasyQuoteService : IEasyQuoteService
    {
        private readonly MatrixDbContext _context;

        public EasyQuoteService(MatrixDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<EasyQuoteHeaderDto>> CreateAsync(EasyQuoteCreateDto dto)
        {
            try
            {
                var quote = new EqQuoteHeader
                {
                    PropuestaNombre = dto.PropuestaNombre,
                    GrupoObjetivo = dto.GrupoObjetivo,
                    Cliente = dto.Cliente,
                    SL = dto.SL,
                    MetodologiaSL = dto.MetodologiaSL,
                    RecordDetail = dto.RecordDetail ?? "N/A",
                    ProbabilidadAprobacion = dto.ProbabilidadAprobacion,
                    FechaAprobacionEstimada = dto.FechaAprobacionEstimada,
                    FechaCampo = dto.FechaCampo,
                    CategoriaProducto = dto.CategoriaProducto,
                    ValorProveedorExterno = dto.ValorProveedorExterno,
                    ValorProveedorInternacional = dto.ValorProveedorInternacional,
                    ValorGMU = dto.ValorGMU,
                    Notas = dto.Notas,
                    FechaCreacion = DateTime.UtcNow,
                    FechaModificacion = DateTime.UtcNow
                };

                _context.EqQuoteHeaders.Add(quote);
                await _context.SaveChangesAsync();

                return new ApiResponse<EasyQuoteHeaderDto>
                {
                    Success = true,
                    Message = "Cotización creada exitosamente",
                    Data = MapToHeaderDto(quote)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<EasyQuoteHeaderDto>
                {
                    Success = false,
                    Message = $"Error al crear cotización: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<EasyQuoteDetailDto>> GetAsync(int id)
        {
            try
            {
                var quote = await _context.EqQuoteHeaders
                    .Include(q => q.Questionnaires)
                    .Include(q => q.Methodologies)
                    .Include(q => q.SampleCities)
                    .Include(q => q.Mysteries)
                    .Include(q => q.StaffSL)
                    .Include(q => q.CostResult)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (quote == null)
                {
                    return new ApiResponse<EasyQuoteDetailDto>
                    {
                        Success = false,
                        Message = "Cotización no encontrada"
                    };
                }

                return new ApiResponse<EasyQuoteDetailDto>
                {
                    Success = true,
                    Data = MapToDetailDto(quote)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<EasyQuoteDetailDto>
                {
                    Success = false,
                    Message = $"Error al obtener cotización: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<PaginatedResult<EasyQuoteListDto>>> ListAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                var total = await _context.EqQuoteHeaders.CountAsync();
                var quotes = await _context.EqQuoteHeaders
                    .OrderByDescending(q => q.FechaCreacion)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(q => new EasyQuoteListDto
                    {
                        Id = q.Id,
                        PropuestaNombre = q.PropuestaNombre,
                        Cliente = q.Cliente,
                        SL = q.SL,
                        FechaCreacion = q.FechaCreacion,
                        Estado = "Activo" // TODO: Implementar lógica de estado basada en CostResult
                    })
                    .ToListAsync();

                return new ApiResponse<PaginatedResult<EasyQuoteListDto>>
                {
                    Success = true,
                    Data = new PaginatedResult<EasyQuoteListDto>
                    {
                        TotalCount = total,
                        Page = page,
                        PageSize = pageSize,
                        Items = quotes
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<EasyQuoteListDto>>
                {
                    Success = false,
                    Message = $"Error al listar cotizaciones: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<EasyQuoteHeaderDto>> UpdateAsync(int id, EasyQuoteUpdateDto dto)
        {
            try
            {
                var quote = await _context.EqQuoteHeaders.FindAsync(id);
                if (quote == null)
                {
                    return new ApiResponse<EasyQuoteHeaderDto>
                    {
                        Success = false,
                        Message = "Cotización no encontrada"
                    };
                }

                // Actualizar propiedades
                quote.PropuestaNombre = dto.PropuestaNombre;
                quote.GrupoObjetivo = dto.GrupoObjetivo;
                quote.Cliente = dto.Cliente;
                quote.SL = dto.SL;
                quote.MetodologiaSL = dto.MetodologiaSL;
                quote.RecordDetail = dto.RecordDetail ?? quote.RecordDetail;
                quote.ProbabilidadAprobacion = dto.ProbabilidadAprobacion;
                quote.FechaAprobacionEstimada = dto.FechaAprobacionEstimada;
                quote.FechaCampo = dto.FechaCampo;
                quote.CategoriaProducto = dto.CategoriaProducto;
                quote.ValorProveedorExterno = dto.ValorProveedorExterno;
                quote.ValorProveedorInternacional = dto.ValorProveedorInternacional;
                quote.ValorGMU = dto.ValorGMU;
                quote.Notas = dto.Notas;
                quote.FechaModificacion = DateTime.UtcNow;

                _context.EqQuoteHeaders.Update(quote);
                await _context.SaveChangesAsync();

                return new ApiResponse<EasyQuoteHeaderDto>
                {
                    Success = true,
                    Message = "Cotización actualizada exitosamente",
                    Data = MapToHeaderDto(quote)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<EasyQuoteHeaderDto>
                {
                    Success = false,
                    Message = $"Error al actualizar cotización: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var quote = await _context.EqQuoteHeaders.FindAsync(id);
                if (quote == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Cotización no encontrada"
                    };
                }

                _context.EqQuoteHeaders.Remove(quote);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Cotización eliminada exitosamente",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error al eliminar cotización: {ex.Message}"
                };
            }
        }

        // ===== HELPERS PARA MAPEOS =====

        private EasyQuoteHeaderDto MapToHeaderDto(EqQuoteHeader quote)
        {
            return new EasyQuoteHeaderDto
            {
                Id = quote.Id,
                PropuestaNombre = quote.PropuestaNombre,
                Cliente = quote.Cliente,
                SL = quote.SL,
                FechaCreacion = quote.FechaCreacion,
                FechaModificacion = quote.FechaModificacion
            };
        }

        private EasyQuoteDetailDto MapToDetailDto(EqQuoteHeader quote)
        {
            var detail = new EasyQuoteDetailDto
            {
                Id = quote.Id,
                PropuestaNombre = quote.PropuestaNombre,
                Cliente = quote.Cliente,
                GrupoObjetivo = quote.GrupoObjetivo,
                SL = quote.SL,
                MetodologiaSL = quote.MetodologiaSL,
                RecordDetail = quote.RecordDetail,
                FechaAprobacionEstimada = quote.FechaAprobacionEstimada,
                FechaCampo = quote.FechaCampo,
                ProbabilidadAprobacion = quote.ProbabilidadAprobacion,
                Notas = quote.Notas,
                FechaCreacion = quote.FechaCreacion,
                FechaModificacion = quote.FechaModificacion
            };

            // Mapear detalles si existen
            if (quote.Questionnaires?.Any() == true)
            {
                var q = quote.Questionnaires.First();
                detail.Questionnaire = new EasyQuestionnaireDto
                {
                    DuracionMinutos = q.DuracionMinutos,
                    PenetracionLabel = q.PenetracionLabel,
                    PenetracionValor = q.PenetracionValor,
                    PreguntasAbiertas = q.PreguntasAbiertas,
                    PreguntasAbiertasMultiples = q.PreguntasAbiertasMultiples,
                    TopLine = q.TopLine,
                    DataCleaning = q.DataCleaning,
                    ASCII = q.ASCII,
                    ScriptReclutamiento = q.ScriptReclutamiento,
                    Scripting = q.Scripting,
                    TipoScript = q.TipoScript,
                    Codificacion = q.Codificacion,
                    Procesamiento = q.Procesamiento,
                    NumProcesamientos = q.NumProcesamientos
                };
            }

            if (quote.Methodologies?.Any() == true)
            {
                var m = quote.Methodologies.First();
                detail.Methodology = new EasyMethodologyDto
                {
                    MetodologiaRecoleccion = m.MetodologiaRecoleccion,
                    Tecnica1Tipo = m.Tecnica1Tipo,
                    Tecnica1Flag = m.Tecnica1Flag,
                    Tecnica2Tipo = m.Tecnica2Tipo,
                    Tecnica2Flag = m.Tecnica2Flag,
                    BaseDatos = m.BaseDatos,
                    IncidenciaLabel = m.IncidenciaLabel,
                    IncidenciaValor = m.IncidenciaValor
                };
            }

            if (quote.SampleCities?.Any() == true)
            {
                detail.SampleCities = quote.SampleCities.Select(s => new EasySampleCityDto
                {
                    Ciudad = s.Ciudad,
                    Activa = s.Activa,
                    MuestraTotal = s.MuestraTotal,
                    NSE1 = s.NSE1,
                    NSE2 = s.NSE2,
                    NSE3 = s.NSE3,
                    NSE4 = s.NSE4,
                    NSE5 = s.NSE5,
                    NSE6 = s.NSE6,
                    SobreMuestraPct = s.SobreMuestraPct,
                    PesoProductoGramos = s.PesoProductoGramos,
                    EnvioCiudades = s.EnvioCiudades
                }).ToList();
            }

            return detail;
        }
    }
}
