using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MatrixNext.Data.Entities;
using MatrixNext.Data.Modules.TH.Ausencias.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Modules.TH.Ausencias.Adapters
{
    /// <summary>
    /// DataAdapter para Ausencias usando Dapper
    /// Mapea directamente a las tablas: TH_SolicitudAusencia, TH_Ausencia_Incapacidades
    /// </summary>
    public class AusenciaDataAdapter
    {
        private readonly string _connectionString;

        public AusenciaDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb") ??
                throw new ArgumentNullException(nameof(configuration), "MatrixDb connection string not found");
        }

        private MatrixDbContext CreateContext()
        {
            return new MatrixDbContext(_connectionString);
        }

        private static T? GetValue<T>(IDictionary<string, object> row, string key)
        {
            // Case-insensitive lookup because stored procedures often return uppercase column names
            var match = row.Keys.FirstOrDefault(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase));
            if (match == null)
            {
                return default;
            }

            var value = row[match];
            if (value is null || value is DBNull)
            {
                return default;
            }

            var target = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            try
            {
                return (T)Convert.ChangeType(value, target);
            }
            catch
            {
                return default;
            }
        }

        private static AusenciaViewModel MapAusenciaRow(IDictionary<string, object> row)
        {
            var tipoNombre = GetValue<string>(row, "Tipo");
            var estadoNombre = GetValue<string>(row, "Estado");
            var aprobadoPorNombre = GetValue<string>(row, "AprobadoPor") ?? GetValue<string>(row, "Aprobador");

            return new AusenciaViewModel
            {
                Id = GetValue<long>(row, "ID"),
                IdEmpleado = GetValue<long>(row, "IDEMPLEADO"),
                NombreEmpleado = GetValue<string>(row, "EMPLEADO"),
                FiniCausacion = GetValue<DateTime?>(row, "FIniCausacion"),
                FFinCausacion = GetValue<DateTime?>(row, "FFinCausacion"),
                FechaInicio = GetValue<DateTime?>(row, "FInicio"),
                FechaFin = GetValue<DateTime?>(row, "FFin"),
                DiasCalendario = GetValue<short?>(row, "DiasCalendario"),
                DiasLaborales = GetValue<byte?>(row, "DiasLaborales") ?? (byte?)GetValue<short?>(row, "DiasLaborales"),
                Tipo = GetValue<byte?>(row, "TipoId") ?? GetValue<byte?>(row, "Tipo"),
                TipoNombre = tipoNombre,
                Estado = GetValue<byte?>(row, "EstadoId") ?? GetValue<byte?>(row, "Estado"),
                EstadoNombre = estadoNombre,
                AprobadoPor = GetValue<long?>(row, "AprobadoPor"),
                NombreAprobador = aprobadoPorNombre,
                FechaAprobacion = GetValue<DateTime?>(row, "FechaAprobacion"),
                VoBo1 = GetValue<long?>(row, "VoBo1"),
                FechaVoBo1 = GetValue<DateTime?>(row, "FechaVoBo1"),
                VoBo2 = GetValue<long?>(row, "VoBo2"),
                FechaVoBo2 = GetValue<DateTime?>(row, "FechaVoBo2"),
                VoBo3 = GetValue<long?>(row, "VoBo3"),
                FechaVoBo3 = GetValue<DateTime?>(row, "FechaVoBo3"),
                RegistradoPor = GetValue<long?>(row, "RegistradoPor"),
                FechaRegistro = GetValue<DateTime?>(row, "FechaRegistro"),
                ObservacionesSolicitud = GetValue<string>(row, "ObservacionesSolicitud"),
                ObservacionesAprobacion = GetValue<string>(row, "ObservacionesAprobacion")
            };
        }

        #region INSERT

        /// <summary>
        /// Crea una nueva solicitud de ausencia
        /// Inserta en TH_SolicitudAusencia con Estado = 1 (Radicada)
        /// </summary>
        public long CrearSolicitudAusencia(long idEmpleado, byte tipo, DateTime fInicio, DateTime fFin,
            short diasCalendario, byte diasLaborales, long aprobadorId, string observaciones,
            long registradoPor, DateTime? finiCausacion = null, DateTime? fFinCausacion = null)
        {
            using var context = CreateContext();

            var entity = new TH_SolicitudAusencia
            {
                IdEmpleado = idEmpleado,
                FiniCausacion = finiCausacion,
                FFinCausacion = fFinCausacion,
                FInicio = fInicio,
                FFin = fFin,
                DiasCalendario = diasCalendario,
                DiasLaborales = diasLaborales,
                Tipo = tipo,
                Estado = 1,
                AprobadoPor = aprobadorId,
                RegistradoPor = registradoPor,
                FechaRegistro = DateTime.Now,
                ObservacionesSolicitud = observaciones ?? string.Empty,
                ObservacionesAprobacion = string.Empty
            };

            context.SolicitudesAusencia.Add(entity);
            context.SaveChanges();

            return entity.Id;
        }

        /// <summary>
        /// Crea un registro de incapacidad
        /// Inserta en TH_Ausencia_Incapacidades
        /// </summary>
        public bool CrearIncapacidad(long idSolicitudAusencia, byte? entidadConsulta, string ips,
            string registroMedico, byte? tipoIncapacidad, byte? claseAusencia, byte? soat,
            DateTime? fechaAccidenteTrabajo, string comentarios, string cie)
        {
            using var context = CreateContext();

            var entity = new TH_Ausencia_Incapacidades
            {
                IdSolicitudAusencia = Convert.ToInt32(idSolicitudAusencia),
                EntidadConsulta = entidadConsulta,
                IPS = ips ?? string.Empty,
                RegistroMedico = registroMedico ?? string.Empty,
                TipoIncapacidad = tipoIncapacidad,
                ClaseAusencia = claseAusencia,
                SOAT = soat,
                FechaAccidenteTrabajo = fechaAccidenteTrabajo,
                Comentarios = comentarios ?? string.Empty,
                CIE = cie ?? string.Empty
            };

            context.AusenciaIncapacidades.Add(entity);
            return context.SaveChanges() > 0;
        }

        #endregion

        #region UPDATE

        /// <summary>
        /// Aprueba una solicitud de ausencia
        /// Actualiza Estado, FechaAprobacion, VoBo1, FechaVoBo1
        /// </summary>
        public bool AprobarSolicitud(long idSolicitud, long aprobadorId, string observacionesAprobacion = null)
        {
            using var context = CreateContext();

            var entity = context.SolicitudesAusencia.FirstOrDefault(e => e.Id == idSolicitud);
            if (entity == null)
            {
                return false;
            }

            entity.Estado = 20;
            entity.FechaAprobacion = DateTime.Now;
            entity.AprobadoPor = aprobadorId;
            entity.VoBo1 = aprobadorId;
            entity.FechaVoBo1 = DateTime.Now;
            entity.ObservacionesAprobacion = observacionesAprobacion ?? string.Empty;

            return context.SaveChanges() > 0;
        }

        /// <summary>
        /// Rechaza una solicitud de ausencia
        /// Actualiza Estado = 10, FechaAprobacion, VoBo1, FechaVoBo1
        /// </summary>
        public bool RechazarSolicitud(long idSolicitud, long aprobadorId, string observacionesAprobacion = null)
        {
            using var context = CreateContext();

            var entity = context.SolicitudesAusencia.FirstOrDefault(e => e.Id == idSolicitud);
            if (entity == null)
            {
                return false;
            }

            entity.Estado = 10;
            entity.FechaAprobacion = DateTime.Now;
            entity.AprobadoPor = aprobadorId;
            entity.VoBo1 = aprobadorId;
            entity.FechaVoBo1 = DateTime.Now;
            entity.ObservacionesAprobacion = observacionesAprobacion ?? string.Empty;

            return context.SaveChanges() > 0;
        }

        #endregion

        #region SELECT

        /// <summary>
        /// Obtiene una solicitud por ID
        /// </summary>
        public AusenciaViewModel ObtenerPorId(long id)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            var rows = connection.Query("TH_AUSENCIA_GET", parameters, commandType: CommandType.StoredProcedure)
                .Select(r => MapAusenciaRow((IDictionary<string, object>)r))
                .ToList();

            return rows.FirstOrDefault();
        }

        /// <summary>
        /// Obtiene solicitudes con filtros opcionales
        /// </summary>
        public List<AusenciaViewModel> ObtenerSolicitudes(long? idEmpleado = null, byte? tipo = null,
            byte? estado = null, DateTime? fInicio = null, DateTime? fFin = null, DateTime? fAprobacionInicio = null,
            DateTime? fAprobacionFin = null, DateTime? fRegistroInicio = null, DateTime? fRegistroFin = null, long? aprobadorId = null)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@id", null);
            dp.Add("@idEmpleado", idEmpleado);
            dp.Add("@FInicio", fInicio);
            dp.Add("@FFin", fFin);
            dp.Add("@Tipo", tipo);
            dp.Add("@Estado", estado);
            dp.Add("@FAprobacionI", fAprobacionInicio);
            dp.Add("@FAprobacionF", fAprobacionFin);
            dp.Add("@FRegistroI", fRegistroInicio);
            dp.Add("@FRegistroF", fRegistroFin);
            dp.Add("@Aprobador", aprobadorId);

            return connection.Query("TH_AUSENCIA_GET", dp, commandType: CommandType.StoredProcedure)
                .Select(r => MapAusenciaRow((IDictionary<string, object>)r))
                .ToList();
        }

        /// <summary>
        /// Obtiene solicitudes pendientes de aprobación (Estado = 5 o 1) vía SP legado
        /// </summary>
        public List<ReporteSolicitudesPendientesViewModel> ObtenerSolicitudesPendientes()
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<ReporteSolicitudesPendientesViewModel>(
                "TH_REP_SolicitudesPendientesAprobacion",
                commandType: CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// Obtiene incapacidad por ID de solicitud
        /// </summary>
        public IncapacidadViewModel ObtenerIncapacidadPorSolicitud(long idSolicitud)
        {
            using var context = CreateContext();

            var entity = context.AusenciaIncapacidades.AsNoTracking()
                .FirstOrDefault(x => x.IdSolicitudAusencia == (int)idSolicitud);

            if (entity == null)
            {
                return null;
            }

            return new IncapacidadViewModel
            {
                IdSolicitudAusencia = entity.IdSolicitudAusencia,
                EntidadConsulta = entity.EntidadConsulta,
                IPS = entity.IPS,
                RegistroMedico = entity.RegistroMedico,
                TipoIncapacidad = entity.TipoIncapacidad,
                ClaseAusencia = entity.ClaseAusencia,
                SOAT = entity.SOAT,
                FechaAccidenteTrabajo = entity.FechaAccidenteTrabajo,
                Comentarios = entity.Comentarios,
                CIE = entity.CIE
            };
        }

        /// <summary>
        /// Obtiene tipos de ausencia desde el catálogo
        /// </summary>
        public List<TipoAusenciaViewModel> ObtenerTiposAusencia()
        {
            using var connection = new SqlConnection(_connectionString);

            const string sql = @"
                SELECT id, Tipo
                FROM TH_Ausencia_Tipo
                ORDER BY Tipo;";

            return connection.Query<TipoAusenciaViewModel>(sql).ToList();
        }

        /// <summary>
        /// Obtiene aprobadores disponibles (usuarios activos con rol de aprobador)
        /// </summary>
        public List<AprobadorViewModel> ObtenerAprobadores()
        {
            using var connection = new SqlConnection(_connectionString);

            const string sql = @"
                SELECT u.Id as Id, u.Nombres + ' ' + u.Apellidos as NombreCompleto
                FROM US_Usuarios u
                WHERE u.Estado = 1
                ORDER BY NombreCompleto;";

            return connection.Query<AprobadorViewModel>(sql).ToList();
        }

        /// <summary>
        /// Obtiene beneficios pendientes
        /// </summary>
        public List<BeneficioPendienteViewModel> ObtenerBeneficiosPendientes(long idEmpleado)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@IdEmpleado", idEmpleado);

            const string sql = "TH_BeneficiosPendientes";

            return connection.Query<BeneficioPendienteViewModel>(sql, dp, commandType: CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// Obtiene ausencias del equipo para un jefe (coordinador)
        /// </summary>
        public List<AusenciaEquipoViewModel> ObtenerAusenciasEquipo(long idJefe, DateTime inicio, DateTime fin)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@IdJefe", idJefe);
            dp.Add("@FInicio", inicio);
            dp.Add("@FFin", fin);

            const string sql = "TH_AusenciasEquipo_Get";

            return connection.Query<AusenciaEquipoViewModel>(sql, dp, commandType: CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// Obtiene subordinados asignados a un jefe
        /// </summary>
        public List<SubordinadoViewModel> ObtenerSubordinados(long idJefe)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@IdJefe", idJefe);

            const string sql = "TH_AusenciasSubordinados_Get";

            return connection.Query<SubordinadoViewModel>(sql, dp, commandType: CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// Obtiene personas con ausencias
        /// </summary>
        public List<AusenciaEquipoViewModel> ObtenerPersonasConAusencias(long idJefe, string search)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@IdJefe", idJefe);
            dp.Add("@Search", search);

            const string sql = "TH_AusenciasPersonas_Get";

            return connection.Query<AusenciaEquipoViewModel>(sql, dp, commandType: CommandType.StoredProcedure).ToList();
        }

        #endregion

        #region CATÁLOGOS Y VALIDACIONES LEGADO

        public CalculoDiasViewModel CalcularDias(DateTime? inicio, DateTime? fin, bool incluirSabadoComoDiaLaboral, long idEmpleado)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@IdEmpleado", idEmpleado);
            dp.Add("@FInicio", inicio);
            dp.Add("@FFin", fin);
            dp.Add("@IncluirSabado", incluirSabadoComoDiaLaboral);

            const string sql = "TH_Ausencia_CalcularDias";

            return connection.QueryFirstOrDefault<CalculoDiasViewModel>(sql, dp, commandType: CommandType.StoredProcedure);
        }

        public ResultadoValidacionViewModel ValidarSolicitudAusencia(long idEmpleado, DateTime? inicio, DateTime? fin, int? tipo)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@IdEmpleado", idEmpleado);
            dp.Add("@FInicio", inicio);
            dp.Add("@FFin", fin);
            dp.Add("@Tipo", tipo);

            const string sql = "TH_Ausencia_Validar";

            return connection.QueryFirstOrDefault<ResultadoValidacionViewModel>(sql, dp, commandType: CommandType.StoredProcedure);
        }

        public void AnularSolicitud(long idSolicitud)
        {
            using var context = CreateContext();

            var entity = context.SolicitudesAusencia.FirstOrDefault(e => e.Id == idSolicitud);
            if (entity == null)
            {
                throw new InvalidOperationException("Solicitud no encontrada");
            }

            entity.Estado = 0;
            context.SaveChanges();
        }

        public void CausarVacaciones(long idSolicitud)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@IdSolicitud", idSolicitud);

            connection.Execute("TH_Ausencia_CausarVacaciones", dp, commandType: CommandType.StoredProcedure);
        }

        public DateTime ObtenerDiaRegreso(DateTime fecha)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@Fecha", fecha);

            return connection.ExecuteScalar<DateTime>("TH_Ausencia_DiaRegreso", dp, commandType: CommandType.StoredProcedure);
        }

        public List<PeriodoCausadoVacacionesViewModel> ObtenerCausacionVacaciones(long idSolicitud)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@IdSolicitud", idSolicitud);

            return connection.Query<PeriodoCausadoVacacionesViewModel>("TH_Ausencia_Causacion", dp, commandType: CommandType.StoredProcedure).ToList();
        }

        #endregion

        #region REPORTES

        public List<ReporteVacacionesViewModel> ReporteVacaciones()
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<ReporteVacacionesViewModel>("TH_REP_Vacaciones", commandType: CommandType.StoredProcedure).ToList();
        }

        public List<ReporteBeneficiosViewModel> ReporteBeneficios(int anno)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@Anno", anno);

            return connection.Query<ReporteBeneficiosViewModel>("TH_REP_Beneficios", dp, commandType: CommandType.StoredProcedure).ToList();
        }

        public List<ReporteAusentismoViewModel> ReporteAusentismo(int anno)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@Anno", anno);

            return connection.Query<ReporteAusentismoViewModel>("TH_REP_Ausentismo", dp, commandType: CommandType.StoredProcedure).ToList();
        }

        public List<ReporteIncapacidadesViewModel> ReporteIncapacidades(int anno)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@Anno", anno);

            return connection.Query<ReporteIncapacidadesViewModel>("TH_REP_Incapacidades", dp, commandType: CommandType.StoredProcedure).ToList();
        }

        public List<ReporteVacacionesDetalladoViewModel> ReporteVacacionesDetallado(int anno)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@Anno", anno);

            return connection.Query<ReporteVacacionesDetalladoViewModel>("TH_REP_VacacionesDetallado", dp, commandType: CommandType.StoredProcedure).ToList();
        }

        public List<ReporteVacacionesNominaViewModel> ReporteVacacionesNomina(int anno)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@Anno", anno);

            return connection.Query<ReporteVacacionesNominaViewModel>("TH_REP_Vacaciones_Nomina", dp, commandType: CommandType.StoredProcedure).ToList();
        }

        #endregion

        #region SUBORDINADOS

        public (bool success, string message) AgregarSubordinado(long idJefe, long idSubordinado)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@IdJefe", idJefe);
            dp.Add("@IdSubordinado", idSubordinado);

            var result = connection.Execute("TH_AusenciasSubordinados_Add", dp, commandType: CommandType.StoredProcedure);

            return (result > 0, result > 0 ? "Subordinado agregado correctamente" : "No se pudo agregar el subordinado");
        }

        public (bool success, string message) RemoverSubordinado(long idSubordinado)
        {
            using var connection = new SqlConnection(_connectionString);

            var dp = new DynamicParameters();
            dp.Add("@IdSubordinado", idSubordinado);

            var result = connection.Execute("TH_AusenciasSubordinados_Remove", dp, commandType: CommandType.StoredProcedure);

            return (result > 0, result > 0 ? "Subordinado removido correctamente" : "No se pudo remover el subordinado");
        }

        #endregion
    }
}
