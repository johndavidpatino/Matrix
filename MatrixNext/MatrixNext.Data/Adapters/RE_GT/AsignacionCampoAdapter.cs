using Dapper;
using MatrixNext.Data.DTOs.RE_GT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.RE_GT
{
    /// <summary>
    /// Adapter para acceso a datos de asignación de trabajos a coordinadores de campo
    /// Utiliza Dapper para ejecución de SPs y queries
    /// </summary>
    public class AsignacionCampoAdapter : IAsignacionCampoAdapter
    {
        private readonly IDbConnection _connection;

        public AsignacionCampoAdapter(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Obtiene lista paginada de trabajos para asignación
        /// </summary>
        public async Task<(IEnumerable<TrabajoAsignacionDto> trabajos, int totalRecords)> ObtenerTrabajosParaAsignacionAsync(
            BusquedaAsignacionDto busqueda)
        {
            // Query para obtener trabajos sin filtro
            var sql = @"
                SELECT 
                    t.IdTrabajo,
                    p.NombrePropuesta as Propuesta,
                    t.Alternativa,
                    t.JobBook,
                    t.MetCodigo,
                    ISNULL(t.IdCOE, 0) as IdCOEActual,
                    ISNULL(c.Nombre, 'Sin asignar') as COEActualNombre,
                    t.Estado
                FROM PY_Trabajos t
                LEFT JOIN GD_COE c ON t.IdCOE = c.IdCOE
                LEFT JOIN CU_Propuestas p ON t.IdPropuesta = p.IdPropuesta
                WHERE 1=1
            ";

            // Construir condiciones WHERE dinámicamente
            if (!string.IsNullOrEmpty(busqueda?.NombrePropuesta))
            {
                sql += " AND p.NombrePropuesta LIKE @NombrePropuesta";
            }

            if (!string.IsNullOrEmpty(busqueda?.JobBook))
            {
                sql += " AND t.JobBook LIKE @JobBook";
            }

            if (!string.IsNullOrEmpty(busqueda?.MetCodigo))
            {
                sql += " AND t.MetCodigo LIKE @MetCodigo";
            }

            // Contar total de registros
            var countSql = $"SELECT COUNT(*) FROM ({sql}) AS temp";

            var parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(busqueda?.NombrePropuesta))
            {
                parameters.Add("@NombrePropuesta", $"%{busqueda.NombrePropuesta}%");
            }
            if (!string.IsNullOrEmpty(busqueda?.JobBook))
            {
                parameters.Add("@JobBook", $"%{busqueda.JobBook}%");
            }
            if (!string.IsNullOrEmpty(busqueda?.MetCodigo))
            {
                parameters.Add("@MetCodigo", $"%{busqueda.MetCodigo}%");
            }

            // Obtener total de registros
            int totalRecords = await _connection.QueryFirstOrDefaultAsync<int>(
                countSql.Replace("SELECT COUNT(*) FROM", "SELECT COUNT(*) FROM (SELECT 1 FROM").Replace(") AS temp", ") AS temp"),
                parameters
            );

            // Añadir paginación
            sql += $" ORDER BY t.IdTrabajo DESC OFFSET {busqueda.PageIndex * busqueda.PageSize} ROWS FETCH NEXT {busqueda.PageSize} ROWS ONLY";

            var trabajos = await _connection.QueryAsync<TrabajoAsignacionDto>(sql, parameters);

            return (trabajos, totalRecords);
        }

        /// <summary>
        /// Obtiene información del trabajo por ID
        /// </summary>
        public async Task<TrabajoAsignacionDto> ObtenerTrabajoAsync(int idTrabajo)
        {
            var sql = @"
                SELECT 
                    t.IdTrabajo,
                    p.NombrePropuesta as Propuesta,
                    t.Alternativa,
                    t.JobBook,
                    t.MetCodigo,
                    ISNULL(t.IdCOE, 0) as IdCOEActual,
                    ISNULL(c.Nombre, 'Sin asignar') as COEActualNombre,
                    t.Estado
                FROM PY_Trabajos t
                LEFT JOIN GD_COE c ON t.IdCOE = c.IdCOE
                LEFT JOIN CU_Propuestas p ON t.IdPropuesta = p.IdPropuesta
                WHERE t.IdTrabajo = @IdTrabajo
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", idTrabajo);

            var trabajo = await _connection.QueryFirstOrDefaultAsync<TrabajoAsignacionDto>(sql, parameters);

            return trabajo;
        }

        /// <summary>
        /// Obtiene lista de usuarios COE disponibles
        /// </summary>
        public async Task<IEnumerable<UsuarioCOEDto>> ObtenerUsuariosCOEAsync()
        {
            var sql = @"
                SELECT 
                    u.IdPersona,
                    u.NombreCompleto as Nombre,
                    c.IdCOE,
                    c.Nombre as COENombre
                FROM GD_PersonasUsuarios u
                INNER JOIN GD_COE c ON u.IdCOE = c.IdCOE
                WHERE u.Activo = 1
                  AND c.Activo = 1
                ORDER BY c.Nombre, u.NombreCompleto
            ";

            var usuarios = await _connection.QueryAsync<UsuarioCOEDto>(sql);

            return usuarios;
        }

        /// <summary>
        /// Realiza la asignación del trabajo
        /// Llama a SP para actualizar IdCOE en PY_Trabajos
        /// </summary>
        public async Task AsignarTrabajoCampoAsync(AsignacionCampoDto dto)
        {
            // Usar SP para la asignación (si existe)
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", dto.IdTrabajo);
            parameters.Add("@IdCOE", dto.IdCOE);
            parameters.Add("@IdPersona", dto.IdPersona, DbType.Int32);

            // Si existe SP específico, usarlo. De lo contrario, UPDATE directo
            await _connection.ExecuteAsync(
                "PY_Trabajo.AsignarCampo",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Registra el cambio en tabla de auditoría
        /// </summary>
        public async Task GuardarLogAsignacionAsync(LogAsignacionCampoDto dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", dto.IdTrabajo);
            parameters.Add("@COEAnterior", dto.COEAnterior);
            parameters.Add("@COENuevo", dto.COENuevo);
            parameters.Add("@PersonaAnterior", dto.PersonaAnterior, DbType.Int32);
            parameters.Add("@PersonaNueva", dto.PersonaNueva, DbType.Int32);
            parameters.Add("@IdUsuario", dto.IdUsuario);
            parameters.Add("@FechaCambio", dto.FechaCambio);

            await _connection.ExecuteAsync(
                "PY_Trabajo.GuardarLogAsignacion",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene lista de COEs
        /// </summary>
        public async Task<IEnumerable<dynamic>> ObtenerCOEsAsync()
        {
            var sql = @"
                SELECT 
                    IdCOE,
                    Nombre
                FROM GD_COE
                WHERE Activo = 1
                ORDER BY Nombre
            ";

            var coes = await _connection.QueryAsync(sql);

            return coes;
        }
    }
}
