using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Services.GD.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.GD
{
    public class GdPncService : IGdPncService
    {
        private readonly ILogger<GdPncService> _logger;

        public GdPncService(ILogger<GdPncService> logger)
        {
            _logger = logger;
        }

        public Task<(bool success, IEnumerable<object> data)> ObtenerPnc()
        {
            // Pending: Implementar SP para PNC
            return Task.FromResult<(bool success, IEnumerable<object> data)>((true, Array.Empty<object>()));
        }

        public Task<(bool success, int idCreado)> CrearPnc(object dto)
        {
            // Pending: Implementar SP para PNC
            return Task.FromResult<(bool success, int idCreado)>((false, 0));
        }

        public Task<(bool success, string message)> ActualizarPnc(int id, object dto)
        {
            // Pending: Implementar SP para PNC
            return Task.FromResult<(bool success, string message)>((false, "Pendiente"));
        }
    }
}

