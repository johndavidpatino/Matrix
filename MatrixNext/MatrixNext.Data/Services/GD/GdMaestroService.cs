using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD;
using MatrixNext.Data.Services.GD.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.GD
{
    public class GdMaestroService : IGdMaestroService
    {
        private readonly IGdMaestroAdapter _adapter;
        private readonly ILogger<GdMaestroService> _logger;

        public GdMaestroService(IGdMaestroAdapter adapter, ILogger<GdMaestroService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<(bool success, IEnumerable<object> data)> ObtenerMaestros()
        {
            try
            {
                var rows = await _adapter.MaestroDocumentosGet();
                return (true, rows);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo maestro documentos");
                return (false, Array.Empty<object>());
            }
        }

        public async Task<(bool success, object? data)> ObtenerMaestroById(int id)
        {
            // Pending: implement specific SP or filter
            return (false, null);
        }

        public async Task<(bool success, int idCreado)> CrearMaestro(object dto)
        {
            try
            {
                var id = await _adapter.MaestroDocumentosAdd(dto);
                return (true, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando maestro documento");
                return (false, 0);
            }
        }

        public async Task<(bool success, string message)> ActualizarMaestro(int id, object dto)
        {
            try
            {
                var ok = await _adapter.DocumentosMaestrosUpdate(dto);
                return (ok, ok ? "Actualizado" : "Sin cambios");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando maestro documento");
                return (false, "Error");
            }
        }

        public async Task<(bool success, string message)> AnularMaestro(int id)
        {
            try
            {
                var ok = await _adapter.DocumentosControladosActivo(id, false);
                return (ok, ok ? "Anulado" : "No anulado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error anulando maestro documento");
                return (false, "Error");
            }
        }
    }
}
