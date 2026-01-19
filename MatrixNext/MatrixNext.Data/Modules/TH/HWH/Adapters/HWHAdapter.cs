using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using MatrixNext.Data.Modules.TH.HWH.Models;

namespace MatrixNext.Data.Modules.TH.HWH.Adapters
{
    /// <summary>
    /// Interface para el adaptador de HWH (Easy Work / Teletrabajo)
    /// </summary>
    public interface IHWHAdapter
    {
        // Consultas
        Task<IEnumerable<HWHDto>> ObtenerSolicitudesAsync(HWHBusquedaParams parametros);
        Task<HWHDto?> ObtenerSolicitudPorIdAsync(long id);
        Task<IEnumerable<HWHDto>> ObtenerSolicitudesPorUsuarioAsync(long usuario);
        Task<IEnumerable<HWHDto>> ObtenerSolicitudesPorJefeAsync(long jefeDirecto, int? estado, DateTime? fechaInicio, DateTime? fechaFin);
        
        // Vista Gantt
        Task<IEnumerable<HWHGanttDto>> ObtenerGanttPorUsuarioAsync(DateTime fechaInicio, DateTime fechaFin, long usuario);
        Task<IEnumerable<HWHGanttDto>> ObtenerGanttPorJefeAsync(DateTime fechaInicio, DateTime fechaFin, long jefeDirecto, int? estado);
        
        // Operaciones
        Task<long> CrearSolicitudAsync(HWHCreateDto dto, long usuarioRegistro);
        Task<bool> ActualizarEstadoAsync(long id, int estado, long usuarioGestion, string? observaciones);
        
        // Validaciones
        Task<IEnumerable<HWHDto>> ObtenerSolicitudesParaValidarAsync(long usuario, DateTime fechaInicio, DateTime fechaFin);
        
        // Catálogos
        Task<IEnumerable<JefeAprobadorDto>> ObtenerJefesAprobadoresAsync();
    }
    
    /// <summary>
    /// Implementación del adaptador de HWH usando Dapper y SPs legacy
    /// </summary>
    public class HWHAdapter : IHWHAdapter
    {
        private readonly string _connectionString;
        
        public HWHAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Matrix") 
                ?? throw new InvalidOperationException("Connection string 'Matrix' not found");
        }
        
        /// <summary>
        /// Obtiene solicitudes de Easy Work según los parámetros
        /// </summary>
        public async Task<IEnumerable<HWHDto>> ObtenerSolicitudesAsync(HWHBusquedaParams parametros)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", parametros.Id);
            parameters.Add("@Usuario", parametros.Usuario);
            parameters.Add("@JefeDirecto", parametros.JefeDirecto);
            parameters.Add("@Estado", parametros.Estado);
            parameters.Add("@FechaInicio", parametros.FechaInicio);
            parameters.Add("@FechaFin", parametros.FechaFin);
            
            var result = await connection.QueryAsync<HWHDto>(
                "TH_TeletrabajoGet",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            
            return result;
        }
        
        /// <summary>
        /// Obtiene una solicitud por su ID
        /// </summary>
        public async Task<HWHDto?> ObtenerSolicitudPorIdAsync(long id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            
            var result = await connection.QueryFirstOrDefaultAsync<HWHDto>(
                "TH_TeletrabajoGet",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            
            return result;
        }
        
        /// <summary>
        /// Obtiene solicitudes de un usuario específico
        /// </summary>
        public async Task<IEnumerable<HWHDto>> ObtenerSolicitudesPorUsuarioAsync(long usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Usuario", usuario);
            
            var result = await connection.QueryAsync<HWHDto>(
                "TH_TeletrabajoGet",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            
            return result;
        }
        
        /// <summary>
        /// Obtiene solicitudes del equipo de un jefe
        /// </summary>
        public async Task<IEnumerable<HWHDto>> ObtenerSolicitudesPorJefeAsync(
            long jefeDirecto, int? estado, DateTime? fechaInicio, DateTime? fechaFin)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@JefeDirecto", jefeDirecto);
            parameters.Add("@Estado", estado == 0 ? null : estado);
            parameters.Add("@FechaInicio", fechaInicio);
            parameters.Add("@FechaFin", fechaFin);
            
            var result = await connection.QueryAsync<HWHDto>(
                "TH_TeletrabajoGet",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            
            return result;
        }
        
        /// <summary>
        /// Obtiene datos para Gantt por usuario
        /// </summary>
        public async Task<IEnumerable<HWHGanttDto>> ObtenerGanttPorUsuarioAsync(
            DateTime fechaInicio, DateTime fechaFin, long usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@FechaInicio", fechaInicio);
            parameters.Add("@FechaFin", fechaFin);
            parameters.Add("@Id", usuario);
            
            var result = await connection.QueryAsync<HWHGanttDto>(
                "TH_TeleTrabajoJefeXId",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            
            return result;
        }
        
        /// <summary>
        /// Obtiene datos para Gantt por jefe
        /// </summary>
        public async Task<IEnumerable<HWHGanttDto>> ObtenerGanttPorJefeAsync(
            DateTime fechaInicio, DateTime fechaFin, long jefeDirecto, int? estado)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@FechaInicio", fechaInicio);
            parameters.Add("@FechaFin", fechaFin);
            parameters.Add("@JefeDirecto", jefeDirecto);
            parameters.Add("@Estado", estado == 0 ? null : estado);
            
            var result = await connection.QueryAsync<HWHGanttDto>(
                "TH_TeleTrabajoJefeXJefe",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            
            return result;
        }
        
        /// <summary>
        /// Crea una nueva solicitud de Easy Work
        /// </summary>
        public async Task<long> CrearSolicitudAsync(HWHCreateDto dto, long usuarioRegistro)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Usuario", dto.Usuario);
            parameters.Add("@Fecha", dto.FechaProgramada);
            parameters.Add("@Estado", HWHEstados.Pendiente);
            parameters.Add("@Observaciones", dto.Observaciones);
            parameters.Add("@FechaCreacion", DateTime.UtcNow.AddHours(-5));
            parameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
            
            await connection.ExecuteAsync(
                "TH_TeletrabajoAdd",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            
            var id = parameters.Get<long>("@Id");
            
            // Registrar en log
            await RegistrarLogAsync(connection, id, HWHEstados.Pendiente, dto.Observaciones, usuarioRegistro);
            
            return id;
        }
        
        /// <summary>
        /// Actualiza el estado de una solicitud (aprobar, rechazar, anular)
        /// </summary>
        public async Task<bool> ActualizarEstadoAsync(long id, int estado, long usuarioGestion, string? observaciones)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Estado", estado);
            parameters.Add("@UsuarioGestion", usuarioGestion);
            parameters.Add("@ObservacionesGestion", observaciones);
            
            var rows = await connection.ExecuteAsync(
                "TH_TeletrabajoUpdate",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            
            // Registrar en log
            await RegistrarLogAsync(connection, id, estado, observaciones, usuarioGestion);
            
            return rows > 0;
        }
        
        /// <summary>
        /// Obtiene solicitudes para validar reglas de quincena
        /// </summary>
        public async Task<IEnumerable<HWHDto>> ObtenerSolicitudesParaValidarAsync(
            long usuario, DateTime fechaInicio, DateTime fechaFin)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Usuario", usuario);
            parameters.Add("@FechaInicio", fechaInicio);
            parameters.Add("@FechaFin", fechaFin);
            
            var result = await connection.QueryAsync<HWHDto>(
                "TH_TeletrabajoGet",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            
            return result;
        }
        
        /// <summary>
        /// Obtiene lista de jefes aprobadores
        /// </summary>
        public async Task<IEnumerable<JefeAprobadorDto>> ObtenerJefesAprobadoresAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            
            var result = await connection.QueryAsync<JefeAprobadorDto>(
                "TH_HWH_AprobacionManager_Get",
                commandType: CommandType.StoredProcedure
            );
            
            return result;
        }
        
        /// <summary>
        /// Registra en el log de teletrabajo
        /// </summary>
        private async Task RegistrarLogAsync(IDbConnection connection, long idTeletrabajo, int estado, string? observaciones, long usuario)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdTeletrabajo", idTeletrabajo);
            parameters.Add("@Estado", estado);
            parameters.Add("@Observaciones", observaciones);
            parameters.Add("@Usuario", usuario);
            
            await connection.ExecuteAsync(
                "TH_LogTeleTrabajoAdd",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
