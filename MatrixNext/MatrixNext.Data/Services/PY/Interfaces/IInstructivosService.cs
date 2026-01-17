using MatrixNext.Data.Adapters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY.Interfaces
{
    /// <summary>
    /// Servicio de Instructivos (General y Cualitativo)
    /// Encapsula la lógica de negocio para gestión de instructivos por trabajo.
    /// Ref: AUDITORIA_MATRIXNEXT_ENERO_2026.md § Violación de Arquitectura
    /// </summary>
    public interface IInstructivosService
    {
        /// <summary>
        /// Obtiene información del trabajo para contexto
        /// </summary>
        Task<InstructivoTrabajoInfoDto?> ObtenerInfoTrabajoAsync(long idTrabajo);

        /// <summary>
        /// Obtiene listado de instructivos generales del trabajo
        /// </summary>
        Task<List<UploadArchivoDto>> ObtenerInstructivosGeneralesAsync(long idTrabajo);

        /// <summary>
        /// Obtiene listado de instructivos cualitativos del trabajo
        /// </summary>
        Task<List<UploadArchivoDto>> ObtenerInstructivosCualitativosAsync(long idTrabajo);

        /// <summary>
        /// Obtiene archivo por ID
        /// </summary>
        Task<UploadArchivoDto?> ObtenerArchivoAsync(long idArchivo);

        /// <summary>
        /// Descarga archivo con validación de permisos
        /// </summary>
        Task<Stream> DescargarArchivoAsync(long idArchivo, long usuarioId);

        /// <summary>
        /// Elimina instructivo con auditoría
        /// </summary>
        Task<(bool Exitoso, string Mensaje, long? IdContenedor)> EliminarInstructivoAsync(long idArchivo, long usuarioId);

        /// <summary>
        /// Obtiene versiones de instructivos para un trabajo
        /// </summary>
        Task<List<InstructivoVersionDto>> ObtenerVersionesAsync(long idTrabajo, string tipoInstructivo);

        /// <summary>
        /// Valida si el usuario tiene permiso para acceder a instructivos del trabajo
        /// </summary>
        Task<bool> ValidarPermisoAccesoAsync(long idTrabajo, long usuarioId, string[] rolesPermitidos);
    }

    /// <summary>
    /// DTO con información mínima del trabajo para contexto de instructivos
    /// </summary>
    public class InstructivoTrabajoInfoDto
    {
        public long IdTrabajo { get; set; }
        public string NombreTrabajo { get; set; } = string.Empty;
        public string TipoTrabajo { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para versión de instructivo
    /// </summary>
    public class InstructivoVersionDto
    {
        public long IdArchivo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Version { get; set; }
        public string FechaSubida { get; set; } = string.Empty;
        public string? Usuario { get; set; }
        public string? UrlDescarga { get; set; }
    }
}
