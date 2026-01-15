using MatrixNext.Data.DTOs.SGC;

namespace MatrixNext.Data.Services.SGC
{
    /// <summary>
    /// Interface de servicio para Acciones de Mejora
    /// Contiene lógica de negocio, validaciones y seguimiento
    /// </summary>
    public interface ISGCAccionMejoraService
    {
        // Acciones
        Task<(bool Success, string Message)> CreateAsync(SGCAccionMejoraCreateDto dto, long userId);
        Task<SGCAccionMejoraDto> GetByIdAsync(int accionMejoraId);
        Task<List<SGCAccionMejoraDto>> GetByFilterAsync(int? procesoId, long? usuarioResponsable, int pageSize, int pageIndex);
        Task<(bool Success, string Message)> UpdateAsync(SGCAccionMejoraUpdateDto dto, long userId);
        Task<(bool Success, string Message)> DeleteAsync(int accionMejoraId, long userId);

        // Causas
        Task<(bool Success, string Message)> AddCausasAsync(int accionMejoraId, List<SGCCausaCreateDto> causas, long userId);
        Task<(bool Success, string Message)> DeleteCausaAsync(int causaId, long userId);

        // Planes de Acción
        Task<(bool Success, string Message)> AddPlanesAccionAsync(int accionMejoraId, List<SGCPlanAccionCreateDto> planes, long userId);
        Task<(bool Success, string Message)> UpdatePlanAccionAsync(SGCPlanAccionUpdateDto dto, long userId);
        Task<(bool Success, string Message)> DeletePlanAccionAsync(int planAccionId, long userId);
        Task<List<SGCPlanAccionDto>> GetPlanesAccionVencidosAsync();

        // Validaciones
        Task<(bool IsValid, string ErrorMessage)> ValidateCreateAsync(SGCAccionMejoraCreateDto dto);

        // Catálogos
        Task<List<SGCProcesoDto>> GetProcesosAsync();
        Task<List<SGCFuenteNoConformidadDto>> GetFuentesNoConformidadAsync();
        Task<List<SGCFuenteDto>> GetFuentesByTypeAsync(int fuenteNoConformidadId);
    }
}
