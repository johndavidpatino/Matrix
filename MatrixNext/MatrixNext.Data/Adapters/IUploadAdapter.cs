using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters
{
    /// <summary>
    /// Adaptador de archivos por contenedor (instructivos/planillas).
    /// </summary>
    public interface IUploadAdapter
    {
        Task<List<UploadArchivoDto>> ObtenerArchivosPorContenedorAsync(string tipoContenedor, long idContenedor);
        Task<UploadArchivoDto?> ObtenerArchivoAsync(long idArchivo);
        Task<Stream> DescargarArchivoAsync(long idArchivo);
        Task<bool> EliminarArchivoAsync(long idArchivo, long usuarioId, string razon);
    }

    public class UploadArchivoDto
    {
        public long IdArchivo { get; set; }
        public long IdContenedor { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Version { get; set; }
        public DateTime FechaSubida { get; set; }
        public string? UsuarioSubida { get; set; }
        public long TamanoBytess { get; set; }
    }
}
