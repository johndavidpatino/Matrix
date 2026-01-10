using System;
using System.Collections.Generic;

namespace MatrixNext.Data.Adapters.GD.Models
{
    public class RepositorioDocumentoDto
    {
        public int Id { get; set; }
        public int IdContenedor { get; set; }
        public int TipoContenedor { get; set; }
        public int IdDocumento { get; set; }
        public string UrlArchivo { get; set; } = string.Empty;
        public decimal Version { get; set; }
        public string Comentarios { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string NombreArchivo { get; set; } = string.Empty;
    }

    public class UploadDocumentoDto
    {
        public int IdContenedor { get; set; }
        public int TipoContenedor { get; set; }
        public int IdDocumento { get; set; }
        public string UrlArchivo { get; set; } = string.Empty;
        public string Comentarios { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
    }

    public class RepositorioListDto
    {
        public int Id { get; set; }
        public string NombreArchivo { get; set; } = string.Empty;
        public decimal Version { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string RegistradoPor { get; set; } = string.Empty;
        public string Comentarios { get; set; } = string.Empty;
    }
}
