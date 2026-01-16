namespace MatrixNext.Data.Models.MBO;

/// <summary>
/// DTO para representar índices manuales de cuentas
/// Mapeado desde SP: MBO_PGIndicesManuales
/// </summary>
public class IndiceManualDto
{
    /// <summary>
    /// Identificador de cuenta
    /// </summary>
    public int IdCuenta { get; set; }

    /// <summary>
    /// Nombre de la cuenta
    /// </summary>
    public string NombreCuenta { get; set; } = string.Empty;

    /// <summary>
    /// Índice manual 1 (Criterio específico del negocio)
    /// </summary>
    public decimal IndiceCT1 { get; set; }

    /// <summary>
    /// Índice manual 2 (Criterio específico del negocio)
    /// </summary>
    public decimal IndiceCT2 { get; set; }

    /// <summary>
    /// Índice manual 3 (Criterio específico del negocio)
    /// </summary>
    public decimal IndiceCT3 { get; set; }

    /// <summary>
    /// Año de referencia
    /// </summary>
    public int Año { get; set; }

    /// <summary>
    /// Mes de referencia
    /// </summary>
    public int Mes { get; set; }
}
