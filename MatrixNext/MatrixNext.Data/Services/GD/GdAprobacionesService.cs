using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD;
using MatrixNext.Data.Services.GD.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.GD
{
    public class GdAprobacionesService : IGdAprobacionesService
    {
        private readonly IGdAprobacionesAdapter _adapter;
        private readonly ILogger<GdAprobacionesService> _logger;

        public GdAprobacionesService(IGdAprobacionesAdapter adapter, ILogger<GdAprobacionesService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<(bool success, IEnumerable<object> data)> ObtenerRevisionesPendientes()
        {
            try
            {
                var rows = await _adapter.RevisionesGetRev(new { });
                return (true, rows);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo revisiones pendientes");
                return (false, Array.Empty<object>());
            }
        }

        public async Task<(bool success, string message)> AprobarRevision(int revisionId, string? observacion = null)
        {
            try
            {
                var ok = await _adapter.RevisionesEdit(new { RevisionId = revisionId, Estado = "Aprobado", Observacion = observacion });
                return (ok, ok ? "Aprobado" : "No aprobado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aprobando revisión");
                return (false, "Error");
            }
        }

        public async Task<(bool success, string message)> RechazarRevision(int revisionId, string? observacion = null)
        {
            try
            {
                var ok = await _adapter.RevisionesEdit(new { RevisionId = revisionId, Estado = "Rechazado", Observacion = observacion });
                return (ok, ok ? "Rechazado" : "No rechazado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rechazando revisión");
                return (false, "Error");
            }
        }
    }
}
