using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD;
using MatrixNext.Data.Services.GD.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.GD
{
    public class GdRepositorioService : IGdRepositorioService
    {
        private readonly IGdRepositorioAdapter _adapter;
        private readonly ILogger<GdRepositorioService> _logger;

        public GdRepositorioService(IGdRepositorioAdapter adapter, ILogger<GdRepositorioService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<(bool success, IEnumerable<object> data)> ObtenerDocumentos(int? trabajoId = null)
        {
            try
            {
                if (trabajoId.HasValue)
                {
                    var rows = await _adapter.RepositorioDocumentosGetXTrabajo(trabajoId.Value);
                    return (true, rows);
                }
                return (true, Array.Empty<object>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo documentos del repositorio");
                return (false, Array.Empty<object>());
            }
        }

        public async Task<(bool success, int idCreado)> UploadDocumento(object dto)
        {
            try
            {
                var id = await _adapter.RepositorioDocumentosAdd(dto);
                return (true, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subiendo documento al repositorio");
                return (false, 0);
            }
        }

        public async Task<(bool success, string message)> EliminarDocumento(int id)
        {
            try
            {
                var ok = await _adapter.EscanerDocumentosDel(id);
                return (ok, ok ? "Eliminado" : "No eliminado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando documento del repositorio");
                return (false, "Error");
            }
        }
    }
}
