namespace MatrixNext.Data.Models;

public class LogEjecucion
{
    public long Id { get; set; }
    public int TipoOperacion { get; set; }
    public int IdObjeto { get; set; }
    public DateTime FechaOperacion { get; set; }
    public long IdUsuario { get; set; }
    public int Estado { get; set; }
}
