using System;
using System.Collections.Generic;
using System.Linq;
using MatrixNext.Data.Adapters;
using MatrixNext.Data.Models.Usuarios;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Services.Usuarios
{
    public class GrupoUnidadService
    {
        private readonly string _connectionString;

        public GrupoUnidadService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("MatrixDatabase")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'MatrixDatabase' not found");
        }

        public (bool success, string message, List<UnidadViewModel> data) ObtenerTodos()
        {
            try
            {
                using (var adapter = new UsuarioDataAdapter(_connectionString))
                {
                    var unidades = adapter.ObtenerTodasUnidades();
                    var vm = unidades.Select(u => new UnidadViewModel
                    {
                        Id = u.Id,
                        Nombre = u.Nombre,
                        Descripcion = u.Descripcion
                    }).ToList();

                    return (true, "Unidades obtenidas", vm);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener unidades: {ex.Message}", new List<UnidadViewModel>());
            }
        }
    }
}
