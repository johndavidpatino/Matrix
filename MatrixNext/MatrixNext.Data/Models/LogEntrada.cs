namespace MatrixNext.Data.Models;

public class LogEntrada
{
    public long Id { get; set; }
    public long IdUsuario { get; set; }
    public DateTime FechaIngreso { get; set; }
    public string DireccionIP { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;
}
