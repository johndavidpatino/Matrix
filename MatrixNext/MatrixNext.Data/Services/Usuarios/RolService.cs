using System;
using System.Collections.Generic;
using System.Linq;
using MatrixNext.Data.Modules.US.Usuarios.Adapters;
using MatrixNext.Data.Modules.US.Usuarios.Models;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Services.Usuarios
{
    public class RolService
    {
        private readonly string _connectionString;

        public RolService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("MatrixDatabase")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'MatrixDatabase' not found");
        }

        public (bool success, string message, List<RolViewModel> data) ObtenerTodos()
        {
            try
            {
                using (var adapter = new UsuarioDataAdapter(_connectionString))
                {
                    var roles = adapter.ObtenerTodosRoles();
                    var vm = roles.Select(r => new RolViewModel
                    {
                        Id = r.Id,
                        Nombre = r.Rol,
                        Descripcion = r.Descripcion
                    }).ToList();

                    return (true, "Roles obtenidos", vm);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener roles: {ex.Message}", new List<RolViewModel>());
            }
        }
    }
}
