using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<(bool success, IEnumerable<Adapters.GD.Models.RevisionAprobacionDto> data, string message)> ObtenerRevisionesPendientes(int usuarioId)
        {
            try
            {
                var rows = await _adapter.RevisionesGetRev(new { UsuarioId = usuarioId });
                var mapped = rows.Select(r => new Adapters.GD.Models.RevisionAprobacionDto
                {
                    IdRevision = (int)r.IdRevision,
                    DocumentoId = (int)r.DocumentoId,
                    UsuarioId = (int)r.UsuarioId,
                    TipoRevisionId = (int)r.TipoRevisionId,
                    TipoRevision = r.TipoRevision,
                    DocumentoControladoId = (int)r.DocumentoControladoId,
                    NombreDocumento = r.NombreDocumento,
                    FechaAprobacion = r.FechaAprobacion
                });

                return (true, mapped, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo revisiones pendientes");
                return (false, Array.Empty<Adapters.GD.Models.RevisionAprobacionDto>(), "Error obteniendo revisiones");
            }
        }

        public async Task<(bool success, string message)> AprobarRevision(int revisionId, int documentoId, int usuarioId)
        {
            try
            {
                var ok = await _adapter.RevisionesEdit(new
                {
                    IdRevision = revisionId,
                    DocumentoId = documentoId,
                    UsuarioId = usuarioId,
                    FechaAprobacion = DateTime.UtcNow.AddHours(-5),
                    TipoRevision = 3 // 3 = aprobado (según flujo legacy)
                });
                return (ok, ok ? "Aprobado" : "No aprobado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aprobando revisión");
                return (false, "Error aprobando la revisión");
            }
        }
    }
}
