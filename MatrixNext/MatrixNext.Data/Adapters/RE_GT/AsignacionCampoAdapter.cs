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
        /// CORREGIDO: PY_Trabajos → PY_Trabajo, IdCOE → COE, eliminado JOIN a GD_COE (no existe)
        /// </summary>
        public async Task<(IEnumerable<TrabajoAsignacionDto> trabajos, int totalRecords)> ObtenerTrabajosParaAsignacionAsync(
            BusquedaAsignacionDto busqueda)
        {
            // Query corregido para tabla real PY_Trabajo
            var sql = @"
                SELECT 
                    t.id AS IdTrabajo,
                    p.NombrePropuesta as Propuesta,
                    t.Alternativa,
                    t.JobBook,
                    t.MetCodigo,
                    ISNULL(t.COE, 0) as IdCOEActual,
                    'Ver usuario COE' as COEActualNombre,
                    t.Estado
                FROM PY_Trabajo t
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
        /// CORREGIDO: PY_Trabajos → PY_Trabajo
        /// </summary>
        public async Task<TrabajoAsignacionDto> ObtenerTrabajoAsync(int idTrabajo)
        {
            var sql = @"
                SELECT 
                    t.id AS IdTrabajo,
                    p.NombrePropuesta as Propuesta,
                    t.Alternativa,
                    t.JobBook,
                    t.MetCodigo,
                    ISNULL(t.COE, 0) as IdCOEActual,
                    'Ver usuario COE' as COEActualNombre,
                    t.Estado
                FROM PY_Trabajo t
                LEFT JOIN CU_Propuestas p ON t.IdPropuesta = p.IdPropuesta
                WHERE t.id = @IdTrabajo
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", idTrabajo);

            var trabajo = await _connection.QueryFirstOrDefaultAsync<TrabajoAsignacionDto>(sql, parameters);

            return trabajo;
        }

        /// <summary>
        /// Obtiene lista de usuarios COE disponibles
        /// NOTA: Tablas GD_COE y GD_PersonasUsuarios NO EXISTEN en BD
        /// Se debe usar US_Usuarios con rol/permiso específico de COE
        /// </summary>
        public async Task<IEnumerable<UsuarioCOEDto>> ObtenerUsuariosCOEAsync()
        {
            // NOTA: Tablas GD_COE y GD_PersonasUsuarios NO EXISTEN en BD
            // Se usa US_Usuarios como alternativa (verificar permiso específico de COE)
            var sql = @"
                SELECT 
                    u.Id AS IdPersona,
                    u.NombreUsuario AS Nombre,
                    0 AS IdCOE,
                    '' AS COENombre
                FROM US_Usuarios u
                WHERE u.Activo = 1
                ORDER BY u.NombreUsuario
            ";

            var usuarios = await _connection.QueryAsync<UsuarioCOEDto>(sql);

            return usuarios;
        }

        /// <summary>
        /// Realiza la asignación del trabajo
        /// Actualiza IdCOE en PY_Trabajo usando SQL directo
        /// NOTA: No existe SP legacy para esta operación, se usa UPDATE directo
        /// </summary>
        public async Task AsignarTrabajoCampoAsync(AsignacionCampoDto dto)
        {
            // Usar UPDATE directo ya que no existe SP específico
            var sql = @"
                UPDATE PY_Trabajo 
                SET COE = @IdCOE
                WHERE id = @IdTrabajo
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", dto.IdTrabajo);
            parameters.Add("@IdCOE", dto.IdCOE);

            await _connection.ExecuteAsync(sql, parameters);
        }

        /// <summary>
        /// Registra el cambio en tabla de auditoría
        /// NOTA: No existe SP ni tabla de log para esta operación en legacy
        /// Se registra log informativo pero no se persiste
        /// </summary>
        public async Task GuardarLogAsignacionAsync(LogAsignacionCampoDto dto)
        {
            // ADVERTENCIA: No existe SP ni tabla de auditoría para esta operación en BD legacy
            // Si se requiere auditoría, crear tabla: RE_LogAsignacionCampo
            // Por ahora solo se completa la tarea sin persistir log
            
            await Task.CompletedTask;
        }

        /// <summary>
        /// Obtiene lista de COEs
        /// NOTA: Tabla GD_COE NO EXISTE en BD - devuelve lista vacía
        /// </summary>
        public async Task<IEnumerable<dynamic>> ObtenerCOEsAsync()
        {
            // NOTA: Tabla GD_COE NO EXISTE en BD
            // Retorna lista vacía - funcionalidad COE requiere implementación con tabla real
            return await Task.FromResult(Enumerable.Empty<dynamic>());
        }
    }
}
