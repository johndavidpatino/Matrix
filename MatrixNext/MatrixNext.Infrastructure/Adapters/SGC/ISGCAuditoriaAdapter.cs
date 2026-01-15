using MatrixNext.Infrastructure.DTOs.SGC;

namespace MatrixNext.Infrastructure.Adapters.SGC
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

    /// <summary>
    /// DTO para Normativa
    /// </summary>
    public class SGCNormativaDto
    {
        public short Id { get; set; }
        public string Estandar { get; set; }
    }

    /// <summary>
    /// DTO para Tipo de Auditoría
    /// </summary>
    public class SGCTipoAuditoriaDto
    {
        public short Id { get; set; }
        public string TipoAuditoria { get; set; }
    }

    /// <summary>
    /// DTO para Tipo de Hallazgo
    /// </summary>
    public class SGCTipoHallazgoDto
    {
        public byte Id { get; set; }
        public string Nombre { get; set; }
    }

    /// <summary>
    /// DTO para Estado Auditoría
    /// </summary>
    public class SGCEstadoAuditoriaDto
    {
        public byte Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
