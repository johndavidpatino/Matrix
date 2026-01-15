using Dapper;
using MatrixNext.Data.DTOs.ES;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.ES
{
    /// <summary>
    /// Adapter para acceso a datos de Metodología de Campo
    /// </summary>
    public class ESMetodologiaCampoAdapter : IESMetodologiaCampoAdapter
    {
        private readonly string _connectionString;

        public ESMetodologiaCampoAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ESMetodologiaCampoOutputDto>> ObtenerTodosAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<ESMetodologiaCampoOutputDto>(
                    "ES_MetodologiaCampo_Get",
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }

        public async Task<IEnumerable<ESMetodologiaCampoOutputDto>> ObtenerPorTrabajoAsync(long trabajoId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT m.*, t.Nombre as TrabajoNombre, u.NombreCompleto as UsuarioNombre,
                           a.FechaAprobacion, 
                           CASE WHEN a.Id IS NOT NULL THEN 'Aprobada' ELSE 'Pendiente' END as EstadoAprobacion
                    FROM ES_MetodologiaCampo m
                    INNER JOIN PY_Trabajos t ON m.TrabajoId = t.Id
                    LEFT JOIN US_Usuarios u ON m.Usuario = u.Id
                    LEFT JOIN ES_AprobacionMetodologia a ON m.Id = a.IdMetodologia
                    WHERE m.TrabajoId = @TrabajoId
                    ORDER BY m.NoVersion DESC";

                var result = await connection.QueryAsync<ESMetodologiaCampoOutputDto>(
                    query,
                    new { TrabajoId = trabajoId }
                );

                return result;
            }
        }

        public async Task<IEnumerable<ESMetodologiaCampoOutputDto>> ObtenerPendientesAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT m.*, t.Nombre as TrabajoNombre, u.NombreCompleto as UsuarioNombre
                    FROM ES_MetodologiaCampo m
                    INNER JOIN PY_Trabajos t ON m.TrabajoId = t.Id
                    LEFT JOIN US_Usuarios u ON m.Usuario = u.Id
                    WHERE NOT EXISTS (
                        SELECT 1 FROM ES_AprobacionMetodologia a WHERE a.IdMetodologia = m.Id
                    )
                    ORDER BY m.Fecha DESC";

                var result = await connection.QueryAsync<ESMetodologiaCampoOutputDto>(query);

                return result;
            }
        }

        public async Task<ESMetodologiaCampoOutputDto> ObtenerPorIdAsync(long id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT m.*, t.Nombre as TrabajoNombre, u.NombreCompleto as UsuarioNombre,
                           a.FechaAprobacion,
                           CASE WHEN a.Id IS NOT NULL THEN 'Aprobada' ELSE 'Pendiente' END as EstadoAprobacion
                    FROM ES_MetodologiaCampo m
                    LEFT JOIN PY_Trabajos t ON m.TrabajoId = t.Id
                    LEFT JOIN US_Usuarios u ON m.Usuario = u.Id
                    LEFT JOIN ES_AprobacionMetodologia a ON m.Id = a.IdMetodologia
                    WHERE m.Id = @Id";

                var result = await connection.QueryFirstOrDefaultAsync<ESMetodologiaCampoOutputDto>(
                    query,
                    new { Id = id }
                );

                return result;
            }
        }

        public async Task<long> CrearAsync(ESMetodologiaCampoInputDto dto, long usuarioId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Obtener siguiente versión
                var version = await ObtenerNumeroVersionesAsync(dto.TrabajoId);
                version++;

                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", dto.TrabajoId);
                parameters.Add("@NombreEstudio", dto.NombreEstudio);
                parameters.Add("@Fecha", DateTime.Now);
                
                // Campos de selección
                parameters.Add("@Objetivo", dto.Objetivo);
                parameters.Add("@Mercado", dto.Mercado);
                parameters.Add("@Marco", dto.Marco);
                parameters.Add("@Tecnica", dto.Tecnica);
                parameters.Add("@Diseno", dto.Diseno);
                parameters.Add("@Instrucciones", dto.Instrucciones);
                parameters.Add("@Distribucion", dto.Distribucion);
                parameters.Add("@NivelConfianza", dto.NivelConfianza);
                parameters.Add("@MargenError", dto.MargenError);
                parameters.Add("@Desagregacion", dto.Desagregacion);
                parameters.Add("@Fuente", dto.Fuente);
                parameters.Add("@Variables", dto.Variables);
                parameters.Add("@Tasa", dto.Tasa);
                parameters.Add("@Procedimiento", dto.Procedimiento);

                // Campos de texto
                parameters.Add("@ObjetivoT", dto.ObjetivoT);
                parameters.Add("@MercadoT", dto.MercadoT);
                parameters.Add("@MarcoT", dto.MarcoT);
                parameters.Add("@TecnicaT", dto.TecnicaT);
                parameters.Add("@DisenoT", dto.DisenoT);
                parameters.Add("@InstruccionesT", dto.InstruccionesT);
                parameters.Add("@DistribucionT", dto.DistribucionT);
                parameters.Add("@NivelConfianzaT", dto.NivelConfianzaT);
                parameters.Add("@MargenErrorT", dto.MargenErrorT);
                parameters.Add("@DesagregacionT", dto.DesagregacionT);
                parameters.Add("@FuenteT", dto.FuenteT);
                parameters.Add("@VariablesT", dto.VariablesT);
                parameters.Add("@TasaT", dto.TasaT);
                parameters.Add("@ProcedimientoT", dto.ProcedimientoT);
                parameters.Add("@Version", (byte)version);
                parameters.Add("@Usuario", usuarioId);

                var id = await connection.ExecuteScalarAsync<long>(
                    "ES_MetodologiaCampo_Add",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return id;
            }
        }

        public async Task ActualizarAsync(long id, ESMetodologiaCampoInputDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@TrabajoId", dto.TrabajoId);
                parameters.Add("@NombreEstudio", dto.NombreEstudio);
                parameters.Add("@Fecha", DateTime.Now);
                
                // Campos de selección
                parameters.Add("@Objetivo", dto.Objetivo);
                parameters.Add("@Mercado", dto.Mercado);
                parameters.Add("@Marco", dto.Marco);
                parameters.Add("@Tecnica", dto.Tecnica);
                parameters.Add("@Diseno", dto.Diseno);
                parameters.Add("@Instrucciones", dto.Instrucciones);
                parameters.Add("@Distribucion", dto.Distribucion);
                parameters.Add("@NivelConfianza", dto.NivelConfianza);
                parameters.Add("@MargenError", dto.MargenError);
                parameters.Add("@Desagregacion", dto.Desagregacion);
                parameters.Add("@Fuente", dto.Fuente);
                parameters.Add("@Variables", dto.Variables);
                parameters.Add("@Tasa", dto.Tasa);
                parameters.Add("@Procedimiento", dto.Procedimiento);

                // Campos de texto
                parameters.Add("@ObjetivoT", dto.ObjetivoT);
                parameters.Add("@MercadoT", dto.MercadoT);
                parameters.Add("@MarcoT", dto.MarcoT);
                parameters.Add("@TecnicaT", dto.TecnicaT);
                parameters.Add("@DisenoT", dto.DisenoT);
                parameters.Add("@InstruccionesT", dto.InstruccionesT);
                parameters.Add("@DistribucionT", dto.DistribucionT);
                parameters.Add("@NivelConfianzaT", dto.NivelConfianzaT);
                parameters.Add("@MargenErrorT", dto.MargenErrorT);
                parameters.Add("@DesagregacionT", dto.DesagregacionT);
                parameters.Add("@FuenteT", dto.FuenteT);
                parameters.Add("@VariablesT", dto.VariablesT);
                parameters.Add("@TasaT", dto.TasaT);
                parameters.Add("@ProcedimientoT", dto.ProcedimientoT);

                await connection.ExecuteAsync(
                    "ES_MetodologiaCampo_Edit",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task EliminarAsync(long id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                await connection.ExecuteAsync(
                    "ES_MetodologiaCampo_Del",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<int> ObtenerNumeroVersionesAsync(long trabajoId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);

                var result = await connection.ExecuteScalarAsync<int>(
                    "ES_MetodologiaCampo_NumVersiones",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
    }
}
