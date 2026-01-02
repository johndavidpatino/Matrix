using MatrixNext.Data.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace MatrixNext.Data.Services;

/// <summary>
/// Servicio para registrar logs de ejecucion y entrada de usuarios.
/// </summary>
public class LogService
{
    private readonly string _connectionString;

    public LogService(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Guarda un log de ejecucion (auditoria de operaciones).
    /// </summary>
    public void GuardarLogEjecucion(int tipoOperacion, int idObjeto, long idUsuario, int estado)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            connection.Execute(
                @"INSERT INTO LOG_Ejecucion (TipoOperacion, IdObjeto, FechaOperacion, IdUsuario, Estado)
                  VALUES (@TipoOperacion, @IdObjeto, @FechaOperacion, @IdUsuario, @Estado)",
                new
                {
                    TipoOperacion = tipoOperacion,
                    IdObjeto = idObjeto,
                    FechaOperacion = DateTime.Now,
                    IdUsuario = idUsuario,
                    Estado = estado
                }
            );
        }
    }

    /// <summary>
    /// Guarda un log de entrada (acceso al sistema).
    /// </summary>
    public void GuardarLogEntrada(long idUsuario, string direccionIP, string nombreEquipo)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            connection.Execute(
                @"INSERT INTO LOG_Entrada (IdUsuario, FechaIngreso, DireccionIP, NombreEquipo)
                  VALUES (@IdUsuario, @FechaIngreso, @DireccionIP, @NombreEquipo)",
                new
                {
                    IdUsuario = idUsuario,
                    FechaIngreso = DateTime.Now,
                    DireccionIP = direccionIP,
                    NombreEquipo = nombreEquipo
                }
            );
        }
    }
}
