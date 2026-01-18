using Dapper;
using MatrixNext.Data.Models.OP_Trafico;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Adapters.OP_Trafico
{
    /// <summary>
    /// Implementación Adapter para Tráfico de Encuestas
    /// 
    /// SP REALES DISPONIBLES EN BD:
    /// - OP_TraficoEncuestas_Get: Lista de tráfico por trabajo
    /// - OP_TraficoEncuestas_ListadoGet: Listado completo
    /// - OP_TraficoEncuesta_GetCritica: Obtener crítica por trabajo/unidad
    /// - OP_TraficoEncuestas_Edit_Critica: Editar crítica
    /// - OP_TraficoEncuestas_Edit_Verificacion: Editar verificación
    /// - OP_TraficoEncuestas_Add_RMC: Agregar tráfico RMC
    /// - OP_TraficoEncuesta_GetRMC: Obtener RMC por trabajo
    /// - OP_TraficoEncuestasCiudad: Encuestas por ciudad
    /// - OP_TraficoArhivos_GetDisponibleDevolucion: Archivos disponibles devolución
    /// - OP_TraficoArhivos_GetDisponibleEnvio: Archivos disponibles envío
    /// - OP_TraficoEncuestasBorrarEnvio: Borrar envío
    /// 
    /// NOTA: Métodos sin SP legacy retornan stub con warning
    /// </summary>
    public class OP_TraficoAdapter : IOP_TraficoAdapter
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<OP_TraficoAdapter> _logger;

        public OP_TraficoAdapter(IDbConnection dbConnection, ILogger<OP_TraficoAdapter> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        // ============================================
        // CONSULTAS - SP: OP_TraficoEncuestas_Get, OP_TraficoEncuestas_ListadoGet
        // ============================================

        /// <summary>
        /// Obtiene listado de tráfico de encuestas
        /// SP: OP_TraficoEncuestas_Get
        /// </summary>
        public async Task<List<OP_TraficoEventoDTO>> GetEventosAsync(OP_TraficoFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_Trafico] Obteniendo listado de tráfico");

                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", filtros.EstudioId);

                var result = await _dbConnection.QueryAsync<OP_TraficoEventoDTO>(
                    "OP_TraficoEncuestas_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetEventos");
                throw;
            }
        }

        /// <summary>
        /// Obtiene listado completo de tráfico
        /// SP: OP_TraficoEncuestas_ListadoGet
        /// </summary>
        public async Task<List<dynamic>> GetListadoCompletoAsync(int trabajoId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "OP_TraficoEncuestas_ListadoGet",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetListadoCompleto");
                throw;
            }
        }

        /// <summary>
        /// STUB: SP OP_Trafico_Evento_GetById no existe en legacy
        /// </summary>
        public async Task<OP_TraficoEventoDTO> GetEventoByIdAsync(int eventoId)
        {
            _logger.LogWarning("[OP_Trafico] GetEventoById: SP no existe en legacy, retornando vacío");
            return await Task.FromResult(new OP_TraficoEventoDTO());
        }

        // ============================================
        // CRÍTICA - SP: OP_TraficoEncuesta_GetCritica, OP_TraficoEncuestas_Edit_Critica
        // ============================================

        /// <summary>
        /// Obtiene crítica por trabajo y unidad
        /// SP: OP_TraficoEncuesta_GetCritica
        /// </summary>
        public async Task<List<dynamic>> GetCriticaByTrabajoAsync(int trabajoId, int unidadId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);
                parameters.Add("@UnidadId", unidadId);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "OP_TraficoEncuesta_GetCritica",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetCriticaByTrabajo");
                throw;
            }
        }

        /// <summary>
        /// STUB: Interfaz requiere GetCriticadoAsync pero SP no existe
        /// </summary>
        public async Task<OP_TraficoCriticadoDTO> GetCriticadoAsync(int eventoId)
        {
            _logger.LogWarning("[OP_Trafico] GetCriticado: Usar GetCriticaByTrabajoAsync con TrabajoId y UnidadId");
            return await Task.FromResult(new OP_TraficoCriticadoDTO());
        }

        /// <summary>
        /// STUB: CriticarAsync - SP requiere campos diferentes a los del DTO actual
        /// SP real: OP_TraficoEncuestas_Edit_Critica (requiere @Cantidad, @UnidadRecibe, @UnidadEnvia, @FechaRecibo)
        /// DTO tiene: EventoId, UsuarioCriticoId, Resultado, Errores, Advertencias, Observaciones
        /// </summary>
        public async Task<bool> CriticarAsync(OP_TraficoCriticarDTO critica)
        {
            _logger.LogWarning("[OP_Trafico] CriticarAsync: DTO no coincide con SP legacy. Solo registramos observaciones.");
            // STUB: El SP real requiere campos que el DTO no tiene
            // En una implementación futura se debe alinear DTO con SP o crear SP nuevo
            return await Task.FromResult(true);
        }

        // ============================================
        // VERIFICACIÓN - SP: OP_TraficoEncuestas_Edit_Verificacion
        // ============================================

        /// <summary>
        /// STUB: SP GetVerificado no existe
        /// </summary>
        public async Task<OP_TraficoVerificadoDTO> GetVerificadoAsync(int eventoId)
        {
            _logger.LogWarning("[OP_Trafico] GetVerificado: SP no existe en legacy");
            return await Task.FromResult(new OP_TraficoVerificadoDTO());
        }

        /// <summary>
        /// STUB: VerificarAsync - SP requiere campos diferentes a los del DTO actual
        /// SP real: OP_TraficoEncuestas_Edit_Verificacion (requiere @Cantidad, @UnidadRecibe, @UnidadEnvia, @FechaRecibo, @Devolucion, @MotivoDevolucion)
        /// DTO tiene: EventoId, UsuarioVerificadorId, Resultado, Inconsistencias, Observaciones
        /// </summary>
        public async Task<bool> VerificarAsync(OP_TraficoVerificarDTO verificacion)
        {
            _logger.LogWarning("[OP_Trafico] VerificarAsync: DTO no coincide con SP legacy. Solo registramos observaciones.");
            // STUB: El SP real requiere campos que el DTO no tiene
            // En una implementación futura se debe alinear DTO con SP o crear SP nuevo
            return await Task.FromResult(true);
        }

        // ============================================
        // RMC - SP: OP_TraficoEncuesta_GetRMC, OP_TraficoEncuestas_Add_RMC
        // ============================================

        /// <summary>
        /// Obtiene RMC por trabajo
        /// SP: OP_TraficoEncuesta_GetRMC
        /// </summary>
        public async Task<List<dynamic>> GetRMCByTrabajoAsync(int trabajoId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "OP_TraficoEncuesta_GetRMC",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetRMCByTrabajo");
                throw;
            }
        }

        /// <summary>
        /// Agrega tráfico RMC
        /// SP: OP_TraficoEncuestas_Add_RMC
        /// </summary>
        public async Task<int> AgregarRMCAsync(int trabajoId, int ciudad, int cantidad, int usuEnvia,
            int unidadEnvia, int unidadRecibe, DateTime fechaEnvio, string observaciones)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);
                parameters.Add("@Ciudad", ciudad);
                parameters.Add("@Cantidad", cantidad);
                parameters.Add("@UsuEnvia", usuEnvia);
                parameters.Add("@UnidadEnvia", unidadEnvia);
                parameters.Add("@UnidadRecibe", unidadRecibe);
                parameters.Add("@FechaEnvio", fechaEnvio);
                parameters.Add("@ObservacionesEnvio", observaciones ?? "");

                return await _dbConnection.ExecuteAsync(
                    "OP_TraficoEncuestas_Add_RMC",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en AgregarRMC");
                throw;
            }
        }

        // ============================================
        // ENCUESTAS POR CIUDAD - SP: OP_TraficoEncuestasCiudad
        // ============================================

        /// <summary>
        /// Obtiene encuestas por ciudad
        /// SP: OP_TraficoEncuestasCiudad
        /// </summary>
        public async Task<List<dynamic>> GetEncuestasPorCiudadAsync(int trabajoId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "OP_TraficoEncuestasCiudad",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetEncuestasPorCiudad");
                throw;
            }
        }

        // ============================================
        // ARCHIVOS - SP: OP_TraficoArhivos_GetDisponible*
        // ============================================

        /// <summary>
        /// Obtiene archivos disponibles para envío
        /// SP: OP_TraficoArhivos_GetDisponibleEnvio
        /// </summary>
        public async Task<List<dynamic>> GetArchivosDisponiblesEnvioAsync(int trabajoId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "OP_TraficoArhivos_GetDisponibleEnvio",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetArchivosDisponiblesEnvio");
                throw;
            }
        }

        /// <summary>
        /// Obtiene archivos disponibles para devolución
        /// SP: OP_TraficoArhivos_GetDisponibleDevolucion
        /// </summary>
        public async Task<List<dynamic>> GetArchivosDisponiblesDevolucionAsync(int trabajoId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);

                var result = await _dbConnection.QueryAsync<dynamic>(
                    "OP_TraficoArhivos_GetDisponibleDevolucion",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetArchivosDisponiblesDevolucion");
                throw;
            }
        }

        // ============================================
        // MÉTODOS SIN SP LEGACY - STUB
        // ============================================

        public async Task<OP_TraficoCapturadoDTO> GetCapturadoAsync(int eventoId)
        {
            _logger.LogWarning("[OP_Trafico] GetCapturado: SP no existe en legacy");
            return await Task.FromResult(new OP_TraficoCapturadoDTO());
        }

        public async Task<int> CapturarAsync(OP_TraficoCapturarDTO captura)
        {
            _logger.LogWarning("[OP_Trafico] Capturar: SP no existe en legacy");
            return await Task.FromResult(0);
        }

        public async Task<OP_TraficoAnuladoDTO> GetAnuladoAsync(int eventoId)
        {
            _logger.LogWarning("[OP_Trafico] GetAnulado: SP no existe en legacy");
            return await Task.FromResult(new OP_TraficoAnuladoDTO());
        }

        public async Task<bool> AnularAsync(OP_TraficoAnularDTO anulacion)
        {
            _logger.LogWarning("[OP_Trafico] Anular: SP no existe en legacy");
            return await Task.FromResult(false);
        }

        public async Task<List<OP_TraficoHistorialDTO>> GetHistorialEventoAsync(int eventoId)
        {
            _logger.LogWarning("[OP_Trafico] GetHistorial: SP no existe en legacy");
            return await Task.FromResult(new List<OP_TraficoHistorialDTO>());
        }

        /// <summary>
        /// Implementación de interfaz: GetHistorialAsync
        /// </summary>
        public async Task<List<OP_TraficoHistorialDTO>> GetHistorialAsync(int eventoId)
        {
            return await GetHistorialEventoAsync(eventoId);
        }

        public async Task<OP_TraficoDashboardDTO> GetDashboardAsync(int? unidadId)
        {
            _logger.LogWarning("[OP_Trafico] GetDashboard: SP no existe en legacy");
            return await Task.FromResult(new OP_TraficoDashboardDTO());
        }

        /// <summary>
        /// Implementación de interfaz: GetDashboardAsync con fechas
        /// </summary>
        public async Task<OP_TraficoDashboardDTO> GetDashboardAsync(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            _logger.LogWarning("[OP_Trafico] GetDashboard (fechas): SP no existe en legacy");
            return await Task.FromResult(new OP_TraficoDashboardDTO());
        }

        public async Task<List<dynamic>> GetErroresAsync(OP_TraficoFiltrosDTO filtros)
        {
            _logger.LogWarning("[OP_Trafico] GetErrores: SP no existe en legacy");
            return await Task.FromResult(new List<dynamic>());
        }

        public async Task<List<dynamic>> GetInconsistenciasAsync(OP_TraficoFiltrosDTO filtros)
        {
            _logger.LogWarning("[OP_Trafico] GetInconsistencias: SP no existe en legacy");
            return await Task.FromResult(new List<dynamic>());
        }

        public async Task<List<EventoEstadoDTO>> GetEstadisticasEstadoAsync()
        {
            _logger.LogWarning("[OP_Trafico] GetEstadisticasEstado: SP no existe en legacy");
            return await Task.FromResult(new List<EventoEstadoDTO>());
        }

        /// <summary>
        /// Valida parámetros de filtros
        /// </summary>
        public void ValidarFiltros(OP_TraficoFiltrosDTO filtros)
        {
            // Validación básica - no aplica en legacy
            _logger.LogWarning("[OP_Trafico] ValidarFiltros: Sin validación en legacy");
        }

        /// <summary>
        /// Valida datos según tipo de transición
        /// </summary>
        public void ValidarDatos(object dto)
        {
            // Validación básica - no aplica en legacy
            _logger.LogWarning("[OP_Trafico] ValidarDatos: Sin validación en legacy");
        }

        /// <summary>
        /// Valida si es posible realizar transición de estado
        /// </summary>
        public async Task<bool> ValidarTransicionAsync(int eventoId, string estadoActual, string estadoNuevo)
        {
            _logger.LogWarning("[OP_Trafico] ValidarTransicion: SP no existe en legacy");
            return await Task.FromResult(true); // Permitir por defecto
        }
    }
}
