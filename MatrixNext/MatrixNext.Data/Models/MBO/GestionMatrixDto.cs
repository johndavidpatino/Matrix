namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para representar las transacciones de gestión en Matrix
/// Mapeado desde SP: MBO_PGGestionMatrix
/// </summary>
public class GestionMatrixDto
{
    /// <summary>
    /// Número de briefs registrados
    /// </summary>
    public int Brief { get; set; }

    /// <summary>
    /// Número de propuestas creadas
    /// </summary>
    public int Propuestas { get; set; }

    /// <summary>
    /// Número de presupuestos generados
    /// </summary>
    public int Presupuestos { get; set; }

    /// <summary>
    /// Número de estudios realizados
    /// </summary>
    public int Estudios { get; set; }

    /// <summary>
    /// Número de proyectos en curso
    /// </summary>
    public int Proyectos { get; set; }

    /// <summary>
    /// Número total de trabajos
    /// </summary>
    public int Trabajos { get; set; }
}
