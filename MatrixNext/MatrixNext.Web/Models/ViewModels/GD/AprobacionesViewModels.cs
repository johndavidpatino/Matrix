using System;
using System.Collections.Generic;

namespace MatrixNext.Web.Models.ViewModels.GD
{
    public class RevisionAprobacionVM
    {
        public int IdRevision { get; set; }
        public int DocumentoId { get; set; }
        public int UsuarioId { get; set; }
        public string TipoRevision { get; set; } = string.Empty;
        public string NombreDocumento { get; set; } = string.Empty;
        public DateTime? FechaAprobacion { get; set; }
    }

    public class AprobacionesIndexVM
    {
        public IList<RevisionAprobacionVM> Revisiones { get; set; } = new List<RevisionAprobacionVM>();
        public string? Error { get; set; }
    }
}
