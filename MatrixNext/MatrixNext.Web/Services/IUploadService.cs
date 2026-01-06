using Microsoft.AspNetCore.Mvc;

namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Interface para servicio de carga de archivos
    /// Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 1
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// Sube un archivo a carpeta del módulo
        /// </summary>
        /// <param name="moduleId">Ej: "PY", "CORE", "OP"</param>
        /// <param name="entityId">ID de la entidad (IdProyecto, IdTrabajo, etc.)</param>
        /// <param name="file">Archivo multipart/form-data</param>
        /// <returns>Resultado con ruta relativa</returns>
        Task<UploadResultVM> SubirArchivoAsync(string moduleId, long entityId, IFormFile file);

        /// <summary>
        /// Descarga archivo verificando permisos
        /// </summary>
        Task<FileStreamResult> DescargarArchivoAsync(string rutaRelativa, long usuarioId);

        /// <summary>
        /// Elimina archivo y registra auditoría
        /// </summary>
        Task<bool> EliminarArchivoAsync(string rutaRelativa, long usuarioId, string razon);

        /// <summary>
        /// Lista archivos de una entidad
        /// </summary>
        Task<List<ArchivoVM>> ListarArchivosAsync(string moduleId, long entityId);
    }

    public class UploadResultVM
    {
        public string RutaRelativa { get; set; }
        public string RutaAbsoluta { get; set; }
        public string NombreArchivo { get; set; }
        public long TamañoBytes { get; set; }
        public DateTime FechaSubida { get; set; }
    }

    public class ArchivoVM
    {
        public string NombreArchivo { get; set; }
        public string RutaRelativa { get; set; }
        public decimal TamañoKB { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
