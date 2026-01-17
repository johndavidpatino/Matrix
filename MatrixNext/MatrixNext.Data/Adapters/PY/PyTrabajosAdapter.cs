using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.PY
{
    /// <summary>
    /// Adapter para operaciones complejas de Trabajos PY
    /// Incluye duplicación completa de trabajos
    /// </summary>
    public class PyTrabajosAdapter : IPyTrabajosAdapter
    {
        private readonly string _connectionString;
        private readonly MatrixDbContext _context;

        public PyTrabajosAdapter(IConfiguration config, MatrixDbContext context)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
            _context = context;
        }

        /// <summary>
        /// Obtiene configuración de trabajo usando SP
        /// SP: PY_TrabajosConfiguracionGet(@TrabajoId BIGINT)
        /// Legacy: trabajoconfiguracionget(trabajoId)
        /// </summary>
        public async Task<TrabajoConfiguracionDto?> ObtenerConfiguracionTrabajo(long trabajoId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@TrabajoId", trabajoId);

            var resultado = await connection.QueryFirstOrDefaultAsync<TrabajoConfiguracionDto>(
                "PY_TrabajosConfiguracionGet",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado;
        }

        /// <summary>
        /// Guarda configuración de trabajo usando SP
        /// SP: PY_TrabajosConfiguracion_Add
        /// Legacy: guardartrabajoconfiguracion(config)
        /// </summary>
        public async Task GuardarConfiguracionTrabajo(long trabajoId, string configuracion, long usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@TrabajoId", trabajoId);
            parametros.Add("@Configuracion", configuracion);
            parametros.Add("@Usuario", usuarioId.ToString());
            parametros.Add("@Fecha", DateTime.Now);

            await connection.ExecuteAsync(
                "PY_TrabajosConfiguracion_Add",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Duplica trabajo completo incluyendo todas sus dependencias
        /// SP principal: PY_TrabajosDuplicar (11 parámetros)
        /// Workflow complejo que orquesta múltiples operaciones
        /// </summary>
        public async Task<DuplicarTrabajoResultDto> DuplicarTrabajoCompleto(DuplicarTrabajoInputDto input)
        {
            var resultado = new DuplicarTrabajoResultDto();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                // Paso 1: Duplicar trabajo base usando SP
                var parametros = new DynamicParameters();
                parametros.Add("@TrabajoIdOrigen", input.TrabajoIdOrigen);
                parametros.Add("@NombreNuevo", input.NombreNuevo);
                parametros.Add("@JobbookNuevo", input.JobbookNuevo);
                parametros.Add("@ProyectoIdNuevo", input.ProyectoIdNuevo);
                parametros.Add("@ClienteIdNuevo", input.ClienteIdNuevo);
                parametros.Add("@TipoModalidad", input.TipoModalidad);
                parametros.Add("@FechaInicioNueva", input.FechaInicioNueva);
                parametros.Add("@FechaFinNueva", input.FechaFinNueva);
                parametros.Add("@Observaciones", input.Observaciones);
                parametros.Add("@UsuarioId", input.UsuarioId);
                parametros.Add("@DuplicarEspecificaciones", input.DuplicarEspecificaciones);
                parametros.Add("@NuevoTrabajoId", dbType: DbType.Int64, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "PY_TrabajosDuplicar",
                    parametros,
                    commandType: CommandType.StoredProcedure);

                resultado.NuevoTrabajoId = parametros.Get<long>("@NuevoTrabajoId");
                resultado.JobBookNuevo = input.JobbookNuevo;

                // Paso 2: Duplicar especificaciones si está habilitado (ya incluido en SP)
                resultado.EspecificacionesDuplicadas = input.DuplicarEspecificaciones;

                // Paso 3: Duplicar muestra por ciudad (si está habilitado)
                if (input.DuplicarMuestra)
                {
                    // NOTA: Requiere implementación de DuplicarMuestra SP
                    // Legacy: muestra(trabajo, ciudad) - requiere iteración por ciudades
                    // Por ahora marcamos como pendiente
                    resultado.MuestraDuplicada = false;
                }

                // Paso 4: Duplicar configuración (si está habilitado)
                if (input.DuplicarConfiguracion)
                {
                    var configOrigen = await ObtenerConfiguracionTrabajo(input.TrabajoIdOrigen);
                    if (configOrigen != null)
                    {
                        await GuardarConfiguracionTrabajo(
                            resultado.NuevoTrabajoId,
                            configOrigen.Configuracion ?? string.Empty,
                            input.UsuarioId);
                        resultado.ConfiguracionDuplicada = true;
                    }
                }

                // Paso 5: Duplicar hilo workflow (si está habilitado)
                if (input.DuplicarHilo)
                {
                    // NOTA: Requiere implementación de PY_HiloDuplicar SP
                    // Legacy: hilo(trabajoIdOrigen, trabajoIdDestino)
                    // Por ahora marcamos como pendiente
                    resultado.HiloDuplicado = false;
                }

                // Paso 6: Copiar documentos físicos (si está habilitado)
                if (input.CopiarDocumentos)
                {
                    // NOTA: Operación de file system - requiere servicio específico
                    // Legacy: copiardocumentos(trabajoIdOrigen, trabajoIdDestino)
                    // Por ahora marcamos como pendiente
                    resultado.DocumentosCopiadosResult = false;
                }

                return resultado;
            }
            catch (Exception ex)
            {
                resultado.ErrorMessage = "Error duplicando trabajo. Por favor intente nuevamente.";
                return resultado;
            }
        }
    }
}
