using MatrixNext.Data.DTOs.SGC;

namespace MatrixNext.Data.Adapters.SGC
{
    /// <summary>
    /// Interface para acceso a datos de Auditorías Internas
    /// Mapea SGC_AuditoriasInternasDapper de CoreProject
    /// </summary>
    public interface ISGCAuditoriaAdapter
    {
        // CRUD Auditorías
        Task<int> CreateAsync(SGCAuditoriaCreateDto dto, long userId);
        Task<SGCAuditoriaDto> GetByIdAsync(int auditoriaId);
        Task<List<SGCAuditoriaDto>> GetByFilterAsync(byte? estadoId, long? auditorId, int? anoAuditoria, long? auditadoId, int pageSize, int pageIndex);
        Task<bool> UpdateEstadoAsync(int auditoriaId, byte nuevoEstadoId, long userId);

        // Informe Auditor
        Task<int> CreateInformeAsync(SGCAuditoriaInformeCreateDto dto, long userId);
        Task<SGCAuditoriaInformeDto> GetInformeByIdAsync(int auditoriaId);
        Task<List<SGCAuditadoDto>> GetAuditadosByIdAsync(int auditoriaId);
        Task<List<SGCHallazgoDto>> GetHallazgosByIdAsync(int auditoriaId);

        // Catálogos
        Task<List<SGCNormativaDto>> GetNormativasAsync();
        Task<List<SGCTipoAuditoriaDto>> GetTiposAuditoriaAsync();
        Task<List<SGCTipoHallazgoDto>> GetTiposHallazgoAsync();
        Task<List<SGCEstadoAuditoriaDto>> GetEstadosAsync();
    }
}
