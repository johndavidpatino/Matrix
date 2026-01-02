using MatrixNext.Data.Models;
using MatrixNext.Data.Utilities;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace MatrixNext.Data.Services;

/// <summary>
/// Servicio de autenticacion que consulta la tabla US_Usuarios en la BD.
/// Emula la logica de CoreProject.US.Usuarios migrada a .NET 8.
/// </summary>
public class UsuarioAuthService
{
    private readonly string _connectionString;

    public UsuarioAuthService(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Obtiene el Id del usuario por nombre de usuario.
    /// Retorna -1 si no existe.
    /// </summary>
    public long ObtenerIdPorNombreUsuario(string nombreUsuario)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var result = connection.QueryFirstOrDefault<long?>(
                "SELECT Id FROM US_Usuarios WHERE Usuario = @Usuario",
                new { Usuario = nombreUsuario }
            );
            return result ?? -1;
        }
    }

    /// <summary>
    /// Obtiene los datos completos del usuario por nombre de usuario.
    /// </summary>
    public Usuario? ObtenerUsuarioPorNombreUsuario(string nombreUsuario)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            return connection.QueryFirstOrDefault<Usuario>(
                @"SELECT Id, Usuario AS NombreUsuario, Nombres, Apellidos, Email, Activo 
                  FROM US_Usuarios 
                  WHERE Usuario = @Usuario",
                new { Usuario = nombreUsuario }
            );
        }
    }

    /// <summary>
    /// Valida credenciales del usuario (usuario + contrasena encriptada).
    /// Retorna el Id del usuario si es válido, -1 si no existe o credenciales son inválidas.
    /// </summary>
    public long VerificarLogin(string nombreUsuario, string passwordPlaintext)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            
            // Obtener usuario
            var usuario = connection.QueryFirstOrDefault<dynamic>(
                @"SELECT Id, Password FROM US_Usuarios 
                  WHERE Usuario = @Usuario",
                new { Usuario = nombreUsuario }
            );

            if (usuario == null)
                return -1;

            // Comparar contraseña encriptada
            string passwordEncriptada = EncryptionService.EncryptPassword(passwordPlaintext);
            if (usuario.Password == passwordEncriptada)
                return usuario.Id;

            return -1;
        }
    }

    /// <summary>
    /// Obtiene el usuario por su Id.
    /// </summary>
    public Usuario? ObtenerUsuarioPorId(long usuarioId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            return connection.QueryFirstOrDefault<Usuario>(
                @"SELECT Id, Usuario AS NombreUsuario, Nombres, Apellidos, Email, Activo 
                  FROM US_Usuarios 
                  WHERE Id = @Id",
                new { Id = usuarioId }
            );
        }
    }
}
