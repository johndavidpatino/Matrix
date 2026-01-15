/// <summary>
/// Modelo de datos para componente reutilizable de Upload
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.6
/// </summary>
namespace MatrixNext.Web.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class UploadFrameModel
    {
        /// <summary>
        /// ID único del componente en la página
        /// </summary>
        public string IdComponente { get; set; } = "upload_" + Guid.NewGuid().ToString("N").Substring(0, 8);

        /// <summary>
        /// Título de la sección de upload
        /// </summary>
        public string TituloSeccion { get; set; } = "Cargar Archivos";

        /// <summary>
        /// Extensiones permitidas, ej: ".pdf, .docx, .xlsx"
        /// </summary>
        public string ExtensionesPermitidas { get; set; } = ".pdf, .docx, .xlsx, .jpg, .png";

        /// <summary>
        /// Tamaño máximo en bytes, default 10 MB
        /// </summary>
        public long TamanoMaximoBytess { get; set; } = 10 * 1024 * 1024;

        /// <summary>
        /// ID del contenedor (trabajo, proyecto, etc.)
        /// </summary>
        public long IdContenedor { get; set; }

        /// <summary>
        /// Tipo de contenedor (Trabajo, Proyecto, etc.)
        /// </summary>
        public string TipoContenedor { get; set; } = "Trabajo";

        /// <summary>
        /// URL endpoint para el upload
        /// </summary>
        public string UrlUpload { get; set; } = "/Upload/UploadFile";

        /// <summary>
        /// URL endpoint para eliminar archivo
        /// </summary>
        public string UrlDelete { get; set; } = "/Upload/DeleteFile";

        /// <summary>
        /// Permite seleccionar múltiples archivos
        /// </summary>
        public bool PermitirMultiple { get; set; } = true;

        /// <summary>
        /// Permite eliminar archivos cargados
        /// </summary>
        public bool PermitirEliminar { get; set; } = true;

        /// <summary>
        /// Mostrar restricciones de upload
        /// </summary>
        public bool MostrarRestricciones { get; set; } = true;

        /// <summary>
        /// No permitir archivos duplicados (por nombre)
        /// </summary>
        public bool NoPermitirDuplicados { get; set; } = true;

        /// <summary>
        /// Validar antivirus (requiere integración)
        /// </summary>
        public bool ValidarAntivirus { get; set; } = false;

        /// <summary>
        /// Callback JavaScript a ejecutar tras upload exitoso
        /// </summary>
        public string CallbackJs { get; set; } = string.Empty;

        /// <summary>
        /// Archivos actuales cargados
        /// </summary>
        public List<UploadedFileModel> ArchivosActuales { get; set; } = new();
    }

    public class UploadedFileModel
    {
        public long IdArchivo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public long TamanoBytess { get; set; }
        public DateTime FechaSubida { get; set; }
        public string UrlDescarga { get; set; } = string.Empty;
    }
}
