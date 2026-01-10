using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD;
using MatrixNext.Data.Adapters.GD.Models;
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

        public async Task<(bool success, List<RepositorioListDto> data, string message)> ObtenerDocumentos(int idContenedor, int tipoContenedor)
        {
            try
            {
                var rows = await _adapter.ObtenerDocumentos(idContenedor, tipoContenedor);
                return (true, rows, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo documentos del repositorio");
                return (false, new List<RepositorioListDto>(), "Error obteniendo documentos");
            }
        }

        public async Task<(bool success, RepositorioDocumentoDto? data, string message)> ObtenerDocumento(int id)
        {
            try
            {
                var doc = await _adapter.ObtenerDocumentoById(id);
                return (doc != null, doc, doc != null ? string.Empty : "No encontrado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo documento del repositorio por id");
                return (false, null, "Error obteniendo documento");
            }
        }

        public async Task<(bool success, int idCreado, decimal version, string message)> SubirDocumento(UploadDocumentoDto dto)
        {
            try
            {
                if (dto.IdContenedor <= 0 || dto.IdDocumento <= 0)
                {
                    return (false, 0, 0, "IdContenedor e IdDocumento son requeridos");
                }

                var version = await _adapter.ObtenerProximaVersion(dto.IdContenedor, dto.IdDocumento);
                var id = await _adapter.GuardarDocumento(dto, version);
                var stored = await _adapter.ObtenerDocumentoById(id);
                var storedVersion = stored?.Version ?? version;
                return (true, id, storedVersion, "Documento guardado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subiendo documento al repositorio");
                return (false, 0, 0, "Error guardando documento");
            }
        }

        public async Task<(bool success, string message)> EliminarDocumento(int id)
        {
            try
            {
                var ok = await _adapter.EliminarDocumento(id);
                return (ok, ok ? "Eliminado" : "No eliminado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando documento del repositorio");
                return (false, "Error eliminando documento");
            }
        }
    }
}
