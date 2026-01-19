using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MatrixNext.Data.Modules.TH.HWH.Models;
using MatrixNext.Data.Modules.TH.HWH.Adapters;

namespace MatrixNext.Data.Modules.TH.HWH.Services
{
    /// <summary>
    /// Interface para el servicio de HWH (Easy Work / Teletrabajo)
    /// </summary>
    public interface IHWHService
    {
        // Consultas
        Task<IEnumerable<HWHDto>> ObtenerMisSolicitudesAsync(long usuario);
        Task<IEnumerable<HWHDto>> ObtenerSolicitudesEquipoAsync(long jefe, int? estado, DateTime? fechaInicio, DateTime? fechaFin);
        Task<HWHDto?> ObtenerSolicitudAsync(long id);
        
        // Gantt
        Task<HWHGanttResult> ObtenerGanttUsuarioAsync(long usuario, DateTime fechaInicio, DateTime fechaFin);
        Task<HWHGanttResult> ObtenerGanttEquipoAsync(long jefe, DateTime fechaInicio, DateTime fechaFin, int? estado);
        
        // Operaciones
        Task<(bool Success, string Message, long? Id)> CrearSolicitudAsync(HWHCreateDto dto, long usuarioRegistro);
        Task<(bool Success, string Message)> AprobarSolicitudAsync(long id, long usuarioGestion, string? observaciones);
        Task<(bool Success, string Message)> RechazarSolicitudAsync(long id, long usuarioGestion, string observaciones);
        Task<(bool Success, string Message)> AnularSolicitudAsync(long id, long usuarioGestion, string observaciones);
        
        // Catálogos
        Task<IEnumerable<JefeAprobadorDto>> ObtenerJefesAprobadoresAsync();
    }
    
    /// <summary>
    /// Implementación del servicio de HWH con lógica de negocio
    /// </summary>
    public class HWHService : IHWHService
    {
        private readonly IHWHAdapter _adapter;
        private readonly ILogger<HWHService> _logger;
        
        public HWHService(IHWHAdapter adapter, ILogger<HWHService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }
        
        /// <summary>
        /// Obtiene las solicitudes del usuario
        /// </summary>
        public async Task<IEnumerable<HWHDto>> ObtenerMisSolicitudesAsync(long usuario)
        {
            try
            {
                return await _adapter.ObtenerSolicitudesPorUsuarioAsync(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo solicitudes del usuario {Usuario}", usuario);
                return Enumerable.Empty<HWHDto>();
            }
        }
        
        /// <summary>
        /// Obtiene las solicitudes del equipo de un jefe
        /// </summary>
        public async Task<IEnumerable<HWHDto>> ObtenerSolicitudesEquipoAsync(
            long jefe, int? estado, DateTime? fechaInicio, DateTime? fechaFin)
        {
            try
            {
                return await _adapter.ObtenerSolicitudesPorJefeAsync(jefe, estado, fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo solicitudes del equipo. Jefe: {Jefe}", jefe);
                return Enumerable.Empty<HWHDto>();
            }
        }
        
        /// <summary>
        /// Obtiene una solicitud por su ID
        /// </summary>
        public async Task<HWHDto?> ObtenerSolicitudAsync(long id)
        {
            try
            {
                return await _adapter.ObtenerSolicitudPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo solicitud {Id}", id);
                return null;
            }
        }
        
        /// <summary>
        /// Obtiene datos de Gantt para un usuario
        /// </summary>
        public async Task<HWHGanttResult> ObtenerGanttUsuarioAsync(
            long usuario, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var datos = await _adapter.ObtenerGanttPorUsuarioAsync(fechaInicio, fechaFin, usuario);
                return ConvertirAGanttResult(datos.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo Gantt usuario {Usuario}", usuario);
                return new HWHGanttResult();
            }
        }
        
        /// <summary>
        /// Obtiene datos de Gantt para el equipo de un jefe
        /// </summary>
        public async Task<HWHGanttResult> ObtenerGanttEquipoAsync(
            long jefe, DateTime fechaInicio, DateTime fechaFin, int? estado)
        {
            try
            {
                var datos = await _adapter.ObtenerGanttPorJefeAsync(fechaInicio, fechaFin, jefe, estado);
                return ConvertirAGanttResult(datos.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo Gantt equipo. Jefe: {Jefe}", jefe);
                return new HWHGanttResult();
            }
        }
        
        /// <summary>
        /// Crea una nueva solicitud de Easy Work con validaciones de negocio
        /// </summary>
        public async Task<(bool Success, string Message, long? Id)> CrearSolicitudAsync(
            HWHCreateDto dto, long usuarioRegistro)
        {
            try
            {
                // Validar reglas de negocio (máximo 2 por mes, 1 por quincena)
                var validacion = await ValidarReglaQuincenaAsync(dto.Usuario, dto.FechaProgramada);
                if (!validacion.Success)
                {
                    return (false, validacion.Message, null);
                }
                
                var id = await _adapter.CrearSolicitudAsync(dto, usuarioRegistro);
                
                _logger.LogInformation(
                    "Solicitud Easy Work {Id} creada. Usuario: {Usuario}, Fecha: {Fecha}",
                    id, dto.Usuario, dto.FechaProgramada);
                
                return (true, "La fecha del Easy Work se guardó correctamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando solicitud Easy Work. Usuario: {Usuario}", dto.Usuario);
                return (false, "Error al crear la solicitud de Easy Work", null);
            }
        }
        
        /// <summary>
        /// Aprueba una solicitud
        /// </summary>
        public async Task<(bool Success, string Message)> AprobarSolicitudAsync(
            long id, long usuarioGestion, string? observaciones)
        {
            try
            {
                var solicitud = await _adapter.ObtenerSolicitudPorIdAsync(id);
                if (solicitud == null)
                {
                    return (false, "La solicitud no existe");
                }
                
                if (!solicitud.PuedeGestionar)
                {
                    return (false, "La solicitud no puede ser aprobada en su estado actual");
                }
                
                var resultado = await _adapter.ActualizarEstadoAsync(
                    id, HWHEstados.Aprobado, usuarioGestion, observaciones);
                
                if (resultado)
                {
                    _logger.LogInformation(
                        "Solicitud Easy Work {Id} aprobada por {Usuario}",
                        id, usuarioGestion);
                    return (true, "La solicitud fue aprobada correctamente");
                }
                
                return (false, "No se pudo aprobar la solicitud");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aprobando solicitud {Id}", id);
                return (false, "Error al aprobar la solicitud");
            }
        }
        
        /// <summary>
        /// Rechaza una solicitud
        /// </summary>
        public async Task<(bool Success, string Message)> RechazarSolicitudAsync(
            long id, long usuarioGestion, string observaciones)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(observaciones))
                {
                    return (false, "Debe indicar el motivo del rechazo");
                }
                
                var solicitud = await _adapter.ObtenerSolicitudPorIdAsync(id);
                if (solicitud == null)
                {
                    return (false, "La solicitud no existe");
                }
                
                if (!solicitud.PuedeGestionar)
                {
                    return (false, "La solicitud no puede ser rechazada en su estado actual");
                }
                
                var resultado = await _adapter.ActualizarEstadoAsync(
                    id, HWHEstados.Rechazado, usuarioGestion, observaciones);
                
                if (resultado)
                {
                    _logger.LogInformation(
                        "Solicitud Easy Work {Id} rechazada por {Usuario}. Motivo: {Motivo}",
                        id, usuarioGestion, observaciones);
                    return (true, "La solicitud fue rechazada");
                }
                
                return (false, "No se pudo rechazar la solicitud");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rechazando solicitud {Id}", id);
                return (false, "Error al rechazar la solicitud");
            }
        }
        
        /// <summary>
        /// Anula una solicitud
        /// </summary>
        public async Task<(bool Success, string Message)> AnularSolicitudAsync(
            long id, long usuarioGestion, string observaciones)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(observaciones))
                {
                    return (false, "Debe indicar el motivo de la anulación");
                }
                
                var solicitud = await _adapter.ObtenerSolicitudPorIdAsync(id);
                if (solicitud == null)
                {
                    return (false, "La solicitud no existe");
                }
                
                if (!solicitud.PuedeAnular)
                {
                    return (false, "La solicitud no puede ser anulada en su estado actual");
                }
                
                var resultado = await _adapter.ActualizarEstadoAsync(
                    id, HWHEstados.Anulado, usuarioGestion, observaciones);
                
                if (resultado)
                {
                    _logger.LogInformation(
                        "Solicitud Easy Work {Id} anulada por {Usuario}. Motivo: {Motivo}",
                        id, usuarioGestion, observaciones);
                    return (true, "Se anuló correctamente el día de Easy Work");
                }
                
                return (false, "No se pudo anular la solicitud");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error anulando solicitud {Id}", id);
                return (false, "Error al anular la solicitud");
            }
        }
        
        /// <summary>
        /// Obtiene lista de jefes aprobadores
        /// </summary>
        public async Task<IEnumerable<JefeAprobadorDto>> ObtenerJefesAprobadoresAsync()
        {
            try
            {
                return await _adapter.ObtenerJefesAprobadoresAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo jefes aprobadores");
                return Enumerable.Empty<JefeAprobadorDto>();
            }
        }
        
        #region Métodos Privados
        
        /// <summary>
        /// Valida las reglas de quincena para Easy Work:
        /// - Máximo 2 por mes
        /// - Máximo 1 por quincena
        /// - No días consecutivos
        /// </summary>
        private async Task<(bool Success, string Message)> ValidarReglaQuincenaAsync(
            long usuario, DateTime fechaNueva)
        {
            // Obtener el mes siguiente (la validación es sobre el mes del Easy Work)
            var primerDiaDelMes = new DateTime(fechaNueva.Year, fechaNueva.Month, 1);
            var ultimoDiaDelMes = primerDiaDelMes.AddMonths(1).AddDays(-1);
            
            var solicitudesExistentes = await _adapter.ObtenerSolicitudesParaValidarAsync(
                usuario, primerDiaDelMes, ultimoDiaDelMes);
            
            // Filtrar solo las activas (no rechazadas ni anuladas)
            var solicitudesActivas = solicitudesExistentes
                .Where(s => s.Estado != HWHEstados.Rechazado && s.Estado != HWHEstados.Anulado)
                .ToList();
            
            // Validar máximo 2 por mes
            if (solicitudesActivas.Count >= 2)
            {
                return (false, "Solo puede tener programado máximo 2 días de Easy Work al mes");
            }
            
            // Validar quincena y días consecutivos
            var diaNuevo = fechaNueva.Day;
            foreach (var solicitud in solicitudesActivas)
            {
                var diaExistente = solicitud.FechaProgramada.Day;
                
                // Validar misma quincena
                var mismaQuincena = (diaExistente <= 15 && diaNuevo <= 15) || 
                                   (diaExistente > 15 && diaNuevo > 15);
                if (mismaQuincena)
                {
                    return (false, "Debe seleccionar otra quincena para tomar el Easy Work");
                }
                
                // Validar días consecutivos
                if (Math.Abs(diaExistente - diaNuevo) == 1)
                {
                    return (false, "No se puede guardar el Easy Work con días consecutivos");
                }
            }
            
            return (true, string.Empty);
        }
        
        /// <summary>
        /// Convierte lista de HWHGanttDto a HWHGanttResult
        /// </summary>
        private HWHGanttResult ConvertirAGanttResult(List<HWHGanttDto> datos)
        {
            var result = new HWHGanttResult();
            
            if (!datos.Any())
            {
                return result;
            }
            
            DateTime? fechaMin = null;
            DateTime? fechaMax = null;
            
            foreach (var item in datos)
            {
                var serie = new HWHGanttSerie
                {
                    Id = item.Id,
                    Name = item.Nombre,
                    FStart = item.FechaInicio,
                    FEnd = item.FechaFinal,
                    Owner = item.Usuario.ToString(),
                    Estado = item.Estado,
                    Descripcion = item.Descripcion
                };
                
                result.Series.Add(serie);
                
                // Calcular fechas mínima y máxima
                if (DateTime.TryParse(item.FechaInicio, out var fi))
                {
                    if (!fechaMin.HasValue || fi < fechaMin.Value)
                        fechaMin = fi;
                }
                
                if (DateTime.TryParse(item.FechaFinal, out var ff))
                {
                    if (!fechaMax.HasValue || ff > fechaMax.Value)
                        fechaMax = ff;
                }
            }
            
            result.FechaInicial = fechaMin ?? DateTime.Now;
            result.FechaFinal = fechaMax ?? DateTime.Now;
            
            return result;
        }
        
        #endregion
    }
}
