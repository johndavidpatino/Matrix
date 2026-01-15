using MatrixNext.Infrastructure.DTOs.SGC;

namespace MatrixNext.Infrastructure.Adapters.SGC
{
    /// <summary>
    /// Interface para acceso a datos de Acciones de Mejora
    /// Mapea AccionesMejoraDapper de CoreProject
    /// </summary>
    public interface ISGCAccionMejoraAdapter
    {
        // CRUD Acciones
        Task<int> CreateAsync(SGCAccionMejoraCreateDto dto, long userId);
        Task<SGCAccionMejoraDto> GetByIdAsync(int accionMejoraId);
        Task<List<SGCAccionMejoraDto>> GetByFilterAsync(int? procesoId, long? usuarioResponsable, byte? estadoId, int pageSize, int pageIndex);
        Task<bool> UpdateAsync(SGCAccionMejoraUpdateDto dto, long userId);
        Task<bool> DeleteAsync(int accionMejoraId, long userId);

        // Causas
        Task<List<SGCCausaDto>> GetCausasByIdAsync(int accionMejoraId);
        Task<bool> AddCausasAsync(int accionMejoraId, List<SGCCausaCreateDto> causas);
        Task<bool> DeleteCausaAsync(int causaId, long userId);

        // Planes de Acción
        Task<List<SGCPlanAccionDto>> GetPlanesAccionByIdAsync(int accionMejoraId);
        Task<bool> AddPlanesAccionAsync(int accionMejoraId, List<SGCPlanAccionCreateDto> planes);
        Task<bool> UpdatePlanAccionAsync(SGCPlanAccionUpdateDto dto, long userId);
        Task<bool> DeletePlanAccionAsync(int planAccionId, long userId);

        // Catálogos
        Task<List<SGCProcesoDto>> GetProcesosAsync();
        Task<List<SGCFuenteNoConformidadDto>> GetFuentesNoConformidadAsync();
        Task<List<SGCFuenteDto>> GetFuentesByTypeAsync(int fuenteNoConformidadId);
    }

    /// <summary>
    /// DTO para Proceso
    /// </summary>
    public class SGCProcesoDto
    {
        public int ProcesoId { get; set; }
        public string NombreProceso { get; set; }
    }

    /// <summary>
    /// DTO para Fuente de No Conformidad
    /// </summary>
    public class SGCFuenteNoConformidadDto
    {
        public int FuenteNoConformidadId { get; set; }
        public string NombreFuente { get; set; }
    }

    /// <summary>
    /// DTO para Fuente específica
    /// </summary>
    public class SGCFuenteDto
    {
        public int FuenteId { get; set; }
        public int FuenteNoConformidadId { get; set; }
        public string NombreFuente { get; set; }
    }
}
