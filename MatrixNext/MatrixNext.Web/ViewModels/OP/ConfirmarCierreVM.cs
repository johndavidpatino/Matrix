using System.Collections.Generic;

namespace MatrixNext.Web.ViewModels.OP
{
    /// <summary>
    /// ViewModel para modal de confirmación de cierre de trabajo
    /// </summary>
    public class ConfirmarCierreVM
    {
        public long TrabajoId { get; set; }
        public string NombreTrabajo { get; set; } = string.Empty;
        public string? JobBook { get; set; }
        public bool TodosDocumentosEncontrados { get; set; }
        public List<string> DocumentosFaltantes { get; set; } = new();
        public int RolResponsableCierre { get; set; }
        public string? Observaciones { get; set; }
    }
}
