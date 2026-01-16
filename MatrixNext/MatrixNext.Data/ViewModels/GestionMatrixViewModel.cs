using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.ViewModels;

/// <summary>
/// ViewModel para la vista de gestión Matrix
/// Muestra métricas de todo el pipeline de negocio
/// </summary>
public class GestionMatrixViewModel
{
    /// <summary>
    /// Datos de gestión Matrix (briefs, propuestas, presupuestos, estudios, proyectos, trabajos)
    /// </summary>
    public GestionMatrixDto? GestionMatrix { get; set; }
}
