namespace MatrixNext.Data.Models;

public class Usuario
{
    public long Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
