using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MatrixNext.Web.Models.ViewModels.GD
{
    public class RepositorioDocumentoVM
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

    public class UploadDocumentoVM
    {
        [Required]
        public int IdContenedor { get; set; }

        [Required]
        public int TipoContenedor { get; set; }

        [Required]
        public int IdDocumento { get; set; }

        [Required(ErrorMessage = "Seleccione un archivo")]
        public IFormFile? Archivo { get; set; }

        [MaxLength(500)]
        public string? Comentarios { get; set; }
    }

    public class RepositorioListVM
    {
        public int Id { get; set; }
        public string NombreArchivo { get; set; } = string.Empty;
        public decimal Version { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string RegistradoPor { get; set; } = string.Empty;
        public string Comentarios { get; set; } = string.Empty;
    }

    public class RepositorioIndexVM
    {
        public int IdContenedor { get; set; }
        public int TipoContenedor { get; set; }
        public int? IdDocumento { get; set; }
        public List<RepositorioListVM> Documentos { get; set; } = new();
    }
}
