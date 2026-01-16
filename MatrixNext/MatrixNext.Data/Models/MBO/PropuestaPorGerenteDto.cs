namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para representar propuestas creadas/enviadas por gerente de cuentas
/// Mapeado desde SP: MBO_PropuestasCreadasEnviadasSinAnuncioGC
/// </summary>
public class PropuestaPorGerenteDto
{
    /// <summary>
    /// Nombres del gerente de cuentas
    /// </summary>
    public string Nombres { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del gerente de cuentas
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del gerente (calculado)
    /// </summary>
    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();

    /// <summary>
    /// Total de propuestas en gestión del gerente
    /// </summary>
    public int PropuestasEnGestion { get; set; }

    /// <summary>
    /// Número de propuestas por actualizar del gerente
    /// </summary>
    public int PropuestasPorActualizar { get; set; }

    /// <summary>
    /// Porcentaje de propuestas por actualizar
    /// </summary>
    public decimal PorcentajePorActualizar => PropuestasEnGestion > 0 
        ? Math.Round((decimal)PropuestasPorActualizar / PropuestasEnGestion * 100, 2) 
        : 0;
}
