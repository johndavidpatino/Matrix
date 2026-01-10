using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD;
using MatrixNext.Data.Services.GD.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.GD
{
    public class GdSolicitudesService : IGdSolicitudesService
    {
        private readonly IGdSolicitudesAdapter _adapter;
        private readonly ILogger<GdSolicitudesService> _logger;

        public GdSolicitudesService(IGdSolicitudesAdapter adapter, ILogger<GdSolicitudesService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<(bool success, IEnumerable<object> data)> ObtenerSolicitudes()
        {
            // Pending: implement SP to list solicitudes
            return (true, Array.Empty<object>());
        }

        public async Task<(bool success, int idCreado)> CrearSolicitud(object dto)
        {
            try
            {
                var id = await _adapter.SolDocumentosAdd(dto);
                return (true, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando solicitud documento");
                return (false, 0);
            }
        }

        public async Task<(bool success, string message)> AsignarRevisores(int solicitudId, IEnumerable<int> usuariosIds)
        {
            try
            {
                foreach (var userId in usuariosIds)
                {
                    var okId = await _adapter.RevisionesAdd(new { SolicitudId = solicitudId, UsuarioId = userId });
                    if (okId <= 0)
                    {
                        return (false, "Error asignando revisores");
                    }
                }
                return (true, "Asignados");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error asignando revisores");
                return (false, "Error");
            }
        }
    }
}
