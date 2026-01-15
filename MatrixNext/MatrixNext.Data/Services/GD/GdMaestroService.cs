using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD;
using MatrixNext.Data.Adapters.GD.Models;
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

        public async Task<(bool success, List<MaestroDocumentoDto> data)> ObtenerMaestros()
        {
            try
            {
                var rows = await _adapter.ObtenerMaestros();
                return (true, rows);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo maestro documentos");
                return (false, new List<MaestroDocumentoDto>());
            }
        }

        public async Task<(bool success, MaestroDocumentoDto? data, DocumentoControlledDto? controlado)> ObtenerMaestroById(int id)
        {
            try
            {
                var maestro = await _adapter.ObtenerMaestroById(id);
                var controlado = await _adapter.ObtenerControlledDocById(id);
                var success = maestro != null;
                return (success, maestro, controlado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo maestro por id");
                return (false, null, null);
            }
        }

        public async Task<(bool success, int idCreado, string message)> CrearMaestro(MaestroDocumentoDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Nombre))
                {
                    return (false, 0, "El nombre del documento es requerido");
                }

                var (valid, validationMessage) = ValidarPorTipo(dto);
                if (!valid)
                {
                    return (false, 0, validationMessage);
                }

                var id = await _adapter.CrearMaestroConControlled(dto);
                return (true, id, "Documento creado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando maestro documento");
                return (false, 0, "Error");
            }
        }

        public async Task<(bool success, string message)> ActualizarMaestro(int id, MaestroDocumentoDto dto)
        {
            try
            {
                var (valid, validationMessage) = ValidarPorTipo(dto);
                if (!valid)
                {
                    return (false, validationMessage);
                }

                var ok = dto.TipoSolicitud == 1
                    ? await _adapter.ActualizarMaestroConstitucion(id, dto)
                    : await _adapter.ActualizarMaestroActualizacion(id, dto);

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
                var okMaestro = await _adapter.AnularMaestro(id);
                var okControlado = await _adapter.AnularControlado(id);
                var success = okMaestro && okControlado;
                return (success, success ? "Anulado" : "No anulado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error anulando maestro documento");
                return (false, "Error");
            }
        }

        public async Task<(bool success, MaestroFormDataDto data)> ObtenerFormData()
        {
            try
            {
                var tipos = await _adapter.ObtenerTiposSolicitud();
                var procesos = await _adapter.ObtenerProcesos();
                var usuarios = await _adapter.ObtenerUsuarios();

                return (true, new MaestroFormDataDto
                {
                    TiposSolicitud = tipos,
                    Procesos = procesos,
                    Usuarios = usuarios
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo form data maestro");
                return (false, new MaestroFormDataDto());
            }
        }

        private static (bool valid, string message) ValidarPorTipo(MaestroDocumentoDto dto)
        {
            // 1 = ConstrucciÃ³n, 2 = ActualizaciÃ³n, 3 = AnulaciÃ³n (solo controlador de Delete)
            if (dto.TipoSolicitud == 1)
            {
                var ctrl = dto.ControlledDoc;
                if (ctrl == null)
                {
                    return (false, "Debe suministrar el documento controlado");
                }

                if (string.IsNullOrWhiteSpace(ctrl.Ubicacion))
                {
                    return (false, "La ubicaciÃ³n del documento controlado es requerida");
                }

                if (string.IsNullOrWhiteSpace(ctrl.MetodoRecuperacion))
                {
                    return (false, "El mÃ©todo de recuperaciÃ³n es requerido");
                }

                if (ctrl.TiempoRetencion <= 0)
                {
                    return (false, "El tiempo de retenciÃ³n debe ser mayor a 0");
                }

                if (string.IsNullOrWhiteSpace(ctrl.DisposicionFinal))
                {
                    return (false, "La disposiciÃ³n final es requerida");
                }
            }

            // For TipoSolicitud 2 we allow same validation as base fields above.
            return (true, string.Empty);
        }
    }
}

