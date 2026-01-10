using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD;
using MatrixNext.Data.Models.GD;
using MatrixNext.Data.Services.GD.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.GD
{
    public class GdCatalogosService : IGdCatalogosService
    {
        private readonly IGdCatalogosAdapter _adapter;
        private readonly ILogger<GdCatalogosService> _logger;

        public GdCatalogosService(IGdCatalogosAdapter adapter, ILogger<GdCatalogosService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<(bool success, List<TipoSolicitudDto> data)> ObtenerTipoSolicitudes()
        {
            try
            {
                var data = await _adapter.ObtenerTipoSolicitudes();
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo Tipos Solicitud");
                return (false, new List<TipoSolicitudDto>());
            }
        }

        public async Task<(bool success, int idCreado)> CrearTipoSolicitud(TipoSolicitudDto dto)
        {
            try
            {
                var id = await _adapter.CrearTipoSolicitud(dto.Nombre, dto.Descripcion);
                return (true, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando Tipo Solicitud");
                return (false, 0);
            }
        }

        public async Task<(bool success, string message)> ActualizarTipoSolicitud(int id, TipoSolicitudDto dto)
        {
            try
            {
                var ok = await _adapter.ActualizarTipoSolicitud(id, dto.Nombre, dto.Descripcion);
                return (ok, ok ? "Actualizado" : "Sin cambios");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando Tipo Solicitud");
                return (false, "Error");
            }
        }

        public async Task<(bool success, string message)> EliminarTipoSolicitud(int id)
        {
            try
            {
                var ok = await _adapter.EliminarTipoSolicitud(id);
                return (ok, ok ? "Eliminado" : "No eliminado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando Tipo Solicitud");
                return (false, "Error");
            }
        }

        public async Task<(bool success, List<EstadoSolicitudDto> data)> ObtenerEstadosSolicitud()
        {
            try
            {
                var data = await _adapter.ObtenerEstadosSolicitud();
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo Estados Solicitud");
                return (false, new List<EstadoSolicitudDto>());
            }
        }

        public async Task<(bool success, int idCreado)> CrearEstadoSolicitud(EstadoSolicitudDto dto)
        {
            try
            {
                var id = await _adapter.CrearEstadoSolicitud(dto.Nombre, dto.Descripcion);
                return (true, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando Estado Solicitud");
                return (false, 0);
            }
        }

        public async Task<(bool success, string message)> ActualizarEstadoSolicitud(int id, EstadoSolicitudDto dto)
        {
            try
            {
                var ok = await _adapter.ActualizarEstadoSolicitud(id, dto.Nombre, dto.Descripcion);
                return (ok, ok ? "Actualizado" : "Sin cambios");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando Estado Solicitud");
                return (false, "Error");
            }
        }

        public async Task<(bool success, string message)> EliminarEstadoSolicitud(int id)
        {
            try
            {
                var ok = await _adapter.EliminarEstadoSolicitud(id);
                return (ok, ok ? "Eliminado" : "No eliminado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando Estado Solicitud");
                return (false, "Error");
            }
        }

        public async Task<(bool success, List<ProcesoDto> data)> ObtenerProcesos()
        {
            try
            {
                var data = await _adapter.ObtenerProcesos();
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo Procesos");
                return (false, new List<ProcesoDto>());
            }
        }

        public async Task<(bool success, int idCreado)> CrearProceso(ProcesoDto dto)
        {
            try
            {
                var id = await _adapter.CrearProceso(dto.Nombre, dto.Descripcion);
                return (true, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando Proceso");
                return (false, 0);
            }
        }

        public async Task<(bool success, string message)> ActualizarProceso(int id, ProcesoDto dto)
        {
            try
            {
                var ok = await _adapter.ActualizarProceso(id, dto.Nombre, dto.Descripcion);
                return (ok, ok ? "Actualizado" : "Sin cambios");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando Proceso");
                return (false, "Error");
            }
        }

        public async Task<(bool success, string message)> EliminarProceso(int id)
        {
            try
            {
                var ok = await _adapter.EliminarProceso(id);
                return (ok, ok ? "Eliminado" : "No eliminado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando Proceso");
                return (false, "Error");
            }
        }
    }
}
