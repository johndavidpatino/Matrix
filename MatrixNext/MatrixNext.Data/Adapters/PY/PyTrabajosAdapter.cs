using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<PyTrabajosAdapter> _logger;

        public PyTrabajosAdapter(IConfiguration config, MatrixDbContext context, ILogger<PyTrabajosAdapter> logger)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene configuración de trabajo usando SP
        /// SP: OP_TrabajoConfiguracion_Get(@TrabajoId)
        /// Legacy: trabajoconfiguracionget(trabajoId) en CoreProject/Clases/PY/Trabajo.vb
        /// </summary>
        public async Task<TrabajoConfiguracionDto?> ObtenerConfiguracionTrabajo(long trabajoId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@TrabajoId", trabajoId);

            var resultado = await connection.QueryFirstOrDefaultAsync<TrabajoConfiguracionDto>(
                "OP_TrabajoConfiguracion_Get",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado;
        }

        /// <summary>
        /// Guarda configuración de trabajo usando SP
        /// SP: OP_TrabajoConfiguracion_Add(@TrabajoId, @FechaIni, @FechaFin, @PorcentajeVerificacion, @UnidadCritica)
        /// Legacy: guardartrabajoconfiguracion() en CoreProject/Clases/PY/Trabajo.vb línea 298
        /// </summary>
        public async Task GuardarConfiguracionTrabajo(long trabajoId, string configuracion, long usuarioId)
        {
            // NOTA: El SP legacy OP_TrabajoConfiguracion_Add usa parámetros diferentes
            // Parámetros reales: @TrabajoId, @fechaini, @fechafin, @porcentajeverificacion, @unidadcritica
            // Este método recibe configuracion como string - incompatible con SP legacy
            // Se registra warning y no se ejecuta hasta mapear correctamente los parámetros
            _logger.LogWarning(
                "GuardarConfiguracionTrabajo: SP OP_TrabajoConfiguracion_Add requiere parámetros diferentes. " +
                "TrabajoId: {TrabajoId}. Requiere implementación correcta.", trabajoId);
            
            await Task.CompletedTask;
        }

        /// <summary>
        /// Duplica trabajo completo incluyendo todas sus dependencias
        /// SP: Py_TrabajoDuplicar (parámetros muy diferentes al modelo actual)
        /// NOTA: El SP real tiene 18 parámetros diferentes a los esperados por DuplicarTrabajoInputDto
        /// Requiere refactorización del DTO o nuevo SP
        /// </summary>
        public async Task<DuplicarTrabajoResultDto> DuplicarTrabajoCompleto(DuplicarTrabajoInputDto input)
        {
            var resultado = new DuplicarTrabajoResultDto();

            // ADVERTENCIA: El SP Py_TrabajoDuplicar tiene parámetros completamente diferentes:
            // @ProyectoId, @OP_MetodologiaId, @PresupuestoId, @NombreTrabajo, @Muestra,
            // @FechaTentativaInicioCampo, @FechaTentativaFinalizacion, @COE, @Unidad, @JobBook,
            // @TipoRecoleccionId, @Estado, @IdPropuesta, @Alternativa, @MetCodigo, @Fase, @NoMedicion
            // El modelo DuplicarTrabajoInputDto no coincide con estos parámetros.
            
            _logger.LogWarning(
                "DuplicarTrabajoCompleto: El SP Py_TrabajoDuplicar tiene parámetros incompatibles con el modelo actual. " +
                "TrabajoIdOrigen: {TrabajoIdOrigen}. Requiere refactorización del DTO.", 
                input.TrabajoIdOrigen);

            resultado.ErrorMessage = "Funcionalidad de duplicación pendiente de implementación. " +
                "El SP Py_TrabajoDuplicar requiere parámetros diferentes.";
            resultado.NuevoTrabajoId = 0;
            resultado.JobBookNuevo = string.Empty;

            return await Task.FromResult(resultado);
        }
    }
}
