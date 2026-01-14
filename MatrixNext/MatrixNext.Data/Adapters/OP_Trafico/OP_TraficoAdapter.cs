using Dapper;
using MatrixNext.Data.Models.OP_Trafico;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Adapters.OP_Trafico
{
    /// <summary>
    /// Implementación Adapter para Operational Traffic
    /// Gestiona workflow: Capturado → Criticado → Verificado → Anulado
    /// Utiliza Dapper para ejecutar SP contra BD
    /// REGLA 2: Mapeo exacto nombres/parámetros desde CoreProject
    /// REGLA 3: Validación respuestas
    /// REGLA 4: Ejecución SP
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
        // CONSULTAS GENERALES
        // ============================================

        public async Task<List<OP_TraficoEventoDTO>> GetEventosAsync(OP_TraficoFiltrosDTO filtros)
        {
            try
            {
                ValidarFiltros(filtros);
                _logger.LogInformation("[OP_Trafico] Iniciando GetEventos con filtros");

                var parameters = new DynamicParameters();
                parameters.Add("@Estado", filtros.Estado ?? "");
                parameters.Add("@Tipo", filtros.Tipo ?? "");
                parameters.Add("@FechaDesde", filtros.FechaDesde ?? DateTime.MinValue);
                parameters.Add("@FechaHasta", filtros.FechaHasta ?? DateTime.Now);
                parameters.Add("@UsuarioId", filtros.UsuarioId);
                parameters.Add("@EstudioId", filtros.EstudioId);
                parameters.Add("@Codigo", filtros.Codigo ?? "");
                parameters.Add("@PageNumber", filtros.PageNumber);
                parameters.Add("@PageSize", filtros.PageSize);

                var result = await _dbConnection.QueryAsync<OP_TraficoEventoDTO>(
                    "OP_Trafico_Eventos_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                _logger.LogInformation($"[OP_Trafico] GetEventos retornó {result.Count()} registros");
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetEventos");
                throw;
            }
        }

        public async Task<OP_TraficoEventoDTO> GetEventoByIdAsync(int eventoId)
        {
            try
            {
                _logger.LogInformation($"[OP_Trafico] Iniciando GetEventoById: {eventoId}");

                var parameters = new DynamicParameters();
                parameters.Add("@EventoId", eventoId);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OP_TraficoEventoDTO>(
                    "OP_Trafico_Evento_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                return result ?? new OP_TraficoEventoDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_Trafico] Error en GetEventoById: {eventoId}");
                throw;
            }
        }

        // ============================================
        // ESTADO: CAPTURADO
        // ============================================

        public async Task<OP_TraficoCapturadoDTO> GetCapturadoAsync(int eventoId)
        {
            try
            {
                _logger.LogInformation($"[OP_Trafico] Obteniendo capturado: {eventoId}");

                var parameters = new DynamicParameters();
                parameters.Add("@EventoId", eventoId);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OP_TraficoCapturadoDTO>(
                    "OP_Trafico_Capturado_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                if (result == null)
                    return new OP_TraficoCapturadoDTO();

                // Obtener datos capturados asociados
                result.DatosCapturados = await ObtenerDatosCapturadosAsync(eventoId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetCapturado");
                throw;
            }
        }

        public async Task<int> CapturarAsync(OP_TraficoCapturarDTO captura)
        {
            try
            {
                ValidarDatos(captura);
                _logger.LogInformation("[OP_Trafico] Capturando datos para estudio: " + captura.EstudioId);

                var parameters = new DynamicParameters();
                parameters.Add("@EstudioId", captura.EstudioId);
                parameters.Add("@Tipo", captura.Tipo);
                parameters.Add("@Descripcion", captura.Descripcion);
                parameters.Add("@UsuarioCapturistaId", captura.UsuarioCapturistaId);
                parameters.Add("@NumeroEncuestas", captura.NumeroEncuestas);
                parameters.Add("@NumeroTrabajadores", captura.NumeroTrabajadores);
                parameters.Add("@Observaciones", captura.Observaciones ?? "");
                parameters.Add("@IdOutput", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dbConnection.ExecuteAsync(
                    "OP_Trafico_Capturado_Save",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                var eventoId = parameters.Get<int>("@IdOutput");
                _logger.LogInformation($"[OP_Trafico] Evento capturado: {eventoId}");

                return eventoId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en Capturar");
                throw;
            }
        }

        // ============================================
        // ESTADO: CRITICADO
        // ============================================

        public async Task<OP_TraficoCriticadoDTO> GetCriticadoAsync(int eventoId)
        {
            try
            {
                _logger.LogInformation($"[OP_Trafico] Obteniendo criticado: {eventoId}");

                var parameters = new DynamicParameters();
                parameters.Add("@EventoId", eventoId);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OP_TraficoCriticadoDTO>(
                    "OP_Trafico_Criticado_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                if (result == null)
                    return new OP_TraficoCriticadoDTO();

                // Obtener errores y advertencias
                result.Errores = await ObtenerErroresAsync(eventoId);
                result.Advertencias = await ObtenerAdvertenciasAsync(eventoId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetCriticado");
                throw;
            }
        }

        public async Task<bool> CriticarAsync(OP_TraficoCriticarDTO critica)
        {
            try
            {
                ValidarDatos(critica);
                _logger.LogInformation($"[OP_Trafico] Criticando evento: {critica.EventoId}");

                var parameters = new DynamicParameters();
                parameters.Add("@EventoId", critica.EventoId);
                parameters.Add("@UsuarioCriticoId", critica.UsuarioCriticoId);
                parameters.Add("@Resultado", critica.Resultado);
                parameters.Add("@NumeroErrores", critica.Errores?.Count ?? 0);
                parameters.Add("@NumeroAdvertencias", critica.Advertencias?.Count ?? 0);
                parameters.Add("@Observaciones", critica.Observaciones ?? "");

                var result = await _dbConnection.ExecuteAsync(
                    "OP_Trafico_Criticado_Save",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en Criticar");
                throw;
            }
        }

        // ============================================
        // ESTADO: VERIFICADO
        // ============================================

        public async Task<OP_TraficoVerificadoDTO> GetVerificadoAsync(int eventoId)
        {
            try
            {
                _logger.LogInformation($"[OP_Trafico] Obteniendo verificado: {eventoId}");

                var parameters = new DynamicParameters();
                parameters.Add("@EventoId", eventoId);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OP_TraficoVerificadoDTO>(
                    "OP_Trafico_Verificado_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                if (result == null)
                    return new OP_TraficoVerificadoDTO();

                // Obtener inconsistencias
                result.Inconsistencias = await ObtenerInconsistenciasAsync(eventoId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetVerificado");
                throw;
            }
        }

        public async Task<bool> VerificarAsync(OP_TraficoVerificarDTO verificacion)
        {
            try
            {
                ValidarDatos(verificacion);
                _logger.LogInformation($"[OP_Trafico] Verificando evento: {verificacion.EventoId}");

                var parameters = new DynamicParameters();
                parameters.Add("@EventoId", verificacion.EventoId);
                parameters.Add("@UsuarioVerificadorId", verificacion.UsuarioVerificadorId);
                parameters.Add("@Resultado", verificacion.Resultado);
                parameters.Add("@NumeroInconsistencias", verificacion.Inconsistencias?.Count ?? 0);
                parameters.Add("@Observaciones", verificacion.Observaciones ?? "");

                var result = await _dbConnection.ExecuteAsync(
                    "OP_Trafico_Verificado_Save",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en Verificar");
                throw;
            }
        }

        // ============================================
        // ESTADO: ANULADO
        // ============================================

        public async Task<OP_TraficoAnuladoDTO> GetAnuladoAsync(int eventoId)
        {
            try
            {
                _logger.LogInformation($"[OP_Trafico] Obteniendo anulado: {eventoId}");

                var parameters = new DynamicParameters();
                parameters.Add("@EventoId", eventoId);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OP_TraficoAnuladoDTO>(
                    "OP_Trafico_Anulado_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                return result ?? new OP_TraficoAnuladoDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetAnulado");
                throw;
            }
        }

        public async Task<bool> AnularAsync(OP_TraficoAnularDTO anulacion)
        {
            try
            {
                ValidarDatos(anulacion);
                _logger.LogInformation($"[OP_Trafico] Anulando evento: {anulacion.EventoId}");

                var parameters = new DynamicParameters();
                parameters.Add("@EventoId", anulacion.EventoId);
                parameters.Add("@UsuarioAnuladorId", anulacion.UsuarioAnuladorId);
                parameters.Add("@MotivoAnulacion", anulacion.MotivoAnulacion);
                parameters.Add("@Observaciones", anulacion.Observaciones ?? "");

                var result = await _dbConnection.ExecuteAsync(
                    "OP_Trafico_Anulado_Save",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en Anular");
                throw;
            }
        }

        // ============================================
        // HISTORIAL Y AUDITORÍA
        // ============================================

        public async Task<List<OP_TraficoHistorialDTO>> GetHistorialAsync(int eventoId)
        {
            try
            {
                _logger.LogInformation($"[OP_Trafico] Obteniendo historial: {eventoId}");

                var parameters = new DynamicParameters();
                parameters.Add("@EventoId", eventoId);

                var result = await _dbConnection.QueryAsync<OP_TraficoHistorialDTO>(
                    "OP_Trafico_Evento_Historial_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetHistorial");
                throw;
            }
        }

        // ============================================
        // DASHBOARDS Y REPORTES
        // ============================================

        public async Task<OP_TraficoDashboardDTO> GetDashboardAsync(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            try
            {
                _logger.LogInformation("[OP_Trafico] Obteniendo dashboard");

                var parameters = new DynamicParameters();
                parameters.Add("@FechaDesde", fechaDesde ?? DateTime.Now.AddDays(-30));
                parameters.Add("@FechaHasta", fechaHasta ?? DateTime.Now);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OP_TraficoDashboardDTO>(
                    "OP_Trafico_Dashboard_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                if (result == null)
                    result = new OP_TraficoDashboardDTO();

                // Obtener estadísticas por estado
                result.EventosPorEstado = await GetEstadisticasEstadoAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetDashboard");
                throw;
            }
        }

        public async Task<List<EventoEstadoDTO>> GetEstadisticasEstadoAsync()
        {
            try
            {
                _logger.LogInformation("[OP_Trafico] Obteniendo estadísticas por estado");

                var result = await _dbConnection.QueryAsync<EventoEstadoDTO>(
                    "OP_Trafico_EstadisticasEstado_Get",
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_Trafico] Error en GetEstadisticasEstado");
                throw;
            }
        }

        // ============================================
        // VALIDACIONES
        // ============================================

        public void ValidarFiltros(OP_TraficoFiltrosDTO filtros)
        {
            if (filtros == null)
                throw new ArgumentNullException(nameof(filtros));

            if (filtros.FechaDesde.HasValue && filtros.FechaHasta.HasValue && filtros.FechaDesde > filtros.FechaHasta)
                throw new InvalidOperationException("FechaDesde no puede ser mayor a FechaHasta");

            if (filtros.PageNumber < 1)
                throw new ArgumentException("PageNumber debe ser > 0");

            if (filtros.PageSize < 1 || filtros.PageSize > 1000)
                throw new ArgumentException("PageSize debe estar entre 1 y 1000");
        }

        public void ValidarDatos(object dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var tipo = dto.GetType();
            _logger.LogInformation($"[OP_Trafico] Validando datos de tipo: {tipo.Name}");

            // TODO: Implementar validaciones específicas según tipo DTO
        }

        public async Task<bool> ValidarTransicionAsync(int eventoId, string estadoActual, string estadoNuevo)
        {
            _logger.LogInformation($"[OP_Trafico] Validando transición: {estadoActual} → {estadoNuevo}");

            // STATE MACHINE: Capturado → Criticado → Verificado → Anulado
            var transicionesValidas = new Dictionary<string, List<string>>
            {
                { EstadosTrafico.CAPTURADO, new() { EstadosTrafico.CRITICADO, EstadosTrafico.ANULADO } },
                { EstadosTrafico.CRITICADO, new() { EstadosTrafico.VERIFICADO, EstadosTrafico.ANULADO } },
                { EstadosTrafico.VERIFICADO, new() { EstadosTrafico.ANULADO } },
                { EstadosTrafico.ANULADO, new() }
            };

            if (!transicionesValidas.ContainsKey(estadoActual))
                return false;

            var permitidas = transicionesValidas[estadoActual];
            return await Task.FromResult(permitidas.Contains(estadoNuevo));
        }

        // ============================================
        // HELPERS PRIVADOS
        // ============================================

        private async Task<List<DatosCapturaDTO>> ObtenerDatosCapturadosAsync(int eventoId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EventoId", eventoId);

            var result = await _dbConnection.QueryAsync<DatosCapturaDTO>(
                "OP_Trafico_DatosCapturados_Get",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60);

            return result.ToList();
        }

        private async Task<List<ErrorCriticaDTO>> ObtenerErroresAsync(int eventoId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EventoId", eventoId);

            var result = await _dbConnection.QueryAsync<ErrorCriticaDTO>(
                "OP_Trafico_Errores_Get",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60);

            return result.ToList();
        }

        private async Task<List<AdvertenciaCriticaDTO>> ObtenerAdvertenciasAsync(int eventoId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EventoId", eventoId);

            var result = await _dbConnection.QueryAsync<AdvertenciaCriticaDTO>(
                "OP_Trafico_Advertencias_Get",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60);

            return result.ToList();
        }

        private async Task<List<InconsistenciaDTO>> ObtenerInconsistenciasAsync(int eventoId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EventoId", eventoId);

            var result = await _dbConnection.QueryAsync<InconsistenciaDTO>(
                "OP_Trafico_Inconsistencias_Get",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60);

            return result.ToList();
        }
    }
}
