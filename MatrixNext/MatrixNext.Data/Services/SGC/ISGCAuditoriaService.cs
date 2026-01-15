using MatrixNext.Data.DTOs.SGC;

namespace MatrixNext.Data.Services.SGC
{
    /// <summary>
    /// Interface de servicio para Auditorías Internas
    /// Contiene lógica de negocio, validaciones y permisos
    /// </summary>
    public interface ISGCAuditoriaService
    {
        // Auditorías
        Task<(bool Success, string Message)> CreateAsync(SGCAuditoriaCreateDto dto, long userId);
        Task<SGCAuditoriaDto> GetByIdAsync(int auditoriaId);
        Task<List<SGCAuditoriaDto>> GetByFilterAsync(byte? estadoId, int? anoAuditoria, int pageSize, int pageIndex, long userId, byte userRoleId);
        Task<(bool Success, string Message)> UpdateEstadoAsync(int auditoriaId, byte nuevoEstadoId, long userId);

        // Informe Auditor
        Task<(bool Success, string Message)> CreateInformeAsync(SGCAuditoriaInformeCreateDto dto, long userId);
        Task<SGCAuditoriaInformeDto> GetInformeByIdAsync(int auditoriaId);

        // Validaciones
        Task<(bool IsValid, string ErrorMessage)> ValidateCreateAsync(SGCAuditoriaCreateDto dto);
        Task<(bool IsValid, string ErrorMessage)> ValidateInformeAsync(SGCAuditoriaInformeCreateDto dto);

        // Catálogos
        Task<List<SGCNormativaDto>> GetNormativasAsync();
        Task<List<SGCTipoAuditoriaDto>> GetTiposAuditoriaAsync();
        Task<List<SGCTipoHallazgoDto>> GetTiposHallazgoAsync();
        Task<List<SGCEstadoAuditoriaDto>> GetEstadosAsync();
    }

    /// <summary>
    /// DTO para respuesta de operación
    /// </summary>
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static OperationResult CreateSuccess(string message, object data = null)
            => new() { Success = true, Message = message, Data = data };

        public static OperationResult CreateFailure(string message)
            => new() { Success = false, Message = message };
    }
}
