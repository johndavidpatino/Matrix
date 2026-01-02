using System;
using System.Collections.Generic;
using MatrixNext.Data.Models.Usuarios;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Services.Usuarios
{
    public class PermisosService
    {
        private readonly string _connectionString;

        public PermisosService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("MatrixDatabase")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'MatrixDatabase' not found");
        }

        public (bool success, string message, List<string> data) ObtenerTodos()
        {
            // Placeholder: implement permisos retrieval when DB structure is analyzed
            return (true, "OK", new List<string>());
        }
    }
}
