using Dapper;
using MatrixNext.Data.Modules.TH.Empleados.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MatrixNext.Data.Modules.TH.Empleados.Adapters
{
    /// <summary>
    /// Adaptador de datos para gestión de empleados.
    /// Ejecuta stored procedures de la base de datos usando Dapper.
    /// </summary>
    public class EmpleadoDataAdapter
    {
        private readonly string _connectionString;

        public EmpleadoDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb") 
                ?? throw new InvalidOperationException("Connection string 'MatrixDb' not found.");
        }

        #region Empleados - CRUD Principal

        /// <summary>
        /// Obtiene listado de empleados con filtros opcionales
        /// SP: TH_Empleados_Get
        /// </summary>
        public async Task<IEnumerable<EmpleadoResumenDTO>> ObtenerEmpleados(EmpleadoFiltroDTO? filtro = null)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@TipoIdentificacion", filtro?.TipoIdentificacion);
            parameters.Add("@Identificacion", filtro?.Identificacion);
            parameters.Add("@Nombre", filtro?.Nombre);
            parameters.Add("@IdCargo", filtro?.IdCargo);
            parameters.Add("@IdMunicipio", filtro?.IdMunicipio);
            parameters.Add("@Estado", filtro?.Estado ?? "ACTIVO");

            return await connection.QueryAsync<EmpleadoResumenDTO>(
                "TH_Empleados_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene empleado por identificación
        /// SP: TH_Empleado_GetPorIdentificacion
        /// </summary>
        public async Task<EmpleadoDetalleDTO?> ObtenerEmpleadoPorIdentificacion(string identificacion)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Identificacion", identificacion);

            return await connection.QueryFirstOrDefaultAsync<EmpleadoDetalleDTO>(
                "TH_Empleado_GetPorIdentificacion",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Crea o actualiza un empleado
        /// SP: TH_Empleado_InsertUpdate
        /// </summary>
        public async Task<(bool success, string message)> GuardarEmpleado(EmpleadoDetalleDTO empleado, string usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@TipoIdentificacion", empleado.TipoIdentificacion);
            parameters.Add("@Identificacion", empleado.Identificacion);
            parameters.Add("@PrimerNombre", empleado.PrimerNombre);
            parameters.Add("@SegundoNombre", empleado.SegundoNombre);
            parameters.Add("@PrimerApellido", empleado.PrimerApellido);
            parameters.Add("@SegundoApellido", empleado.SegundoApellido);
            parameters.Add("@FechaNacimiento", empleado.FechaNacimiento);
            parameters.Add("@LugarNacimiento", empleado.LugarNacimiento);
            parameters.Add("@Genero", empleado.Genero);
            parameters.Add("@EstadoCivil", empleado.EstadoCivil);
            parameters.Add("@GrupoSanguineo", empleado.GrupoSanguineo);
            parameters.Add("@RH", empleado.RH);
            parameters.Add("@Direccion", empleado.Direccion);
            parameters.Add("@IdMunicipio", empleado.IdMunicipio);
            parameters.Add("@Telefono", empleado.Telefono);
            parameters.Add("@Celular", empleado.Celular);
            parameters.Add("@Email", empleado.Email);
            parameters.Add("@IdCargo", empleado.IdCargo);
            parameters.Add("@FechaIngreso", empleado.FechaIngreso);
            parameters.Add("@TallaCamisa", empleado.TallaCamisa);
            parameters.Add("@TallaPantalon", empleado.TallaPantalon);
            parameters.Add("@TallaZapatos", empleado.TallaZapatos);
            parameters.Add("@TieneHijos", empleado.TieneHijos);
            parameters.Add("@NumeroHijos", empleado.NumeroHijos);
            parameters.Add("@TieneVehiculo", empleado.TieneVehiculo);
            parameters.Add("@TieneLicenciaConduccion", empleado.TieneLicenciaConduccion);
            parameters.Add("@CategoriaLicencia", empleado.CategoriaLicencia);
            parameters.Add("@Banco", empleado.Banco);
            parameters.Add("@TipoCuenta", empleado.TipoCuenta);
            parameters.Add("@NumeroCuenta", empleado.NumeroCuenta);
            parameters.Add("@Usuario", usuario);

            try
            {
                await connection.ExecuteAsync(
                    "TH_Empleado_InsertUpdate",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Empleado guardado exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al guardar empleado: {ex.Message}");
            }
        }

        /// <summary>
        /// Retira un empleado (cambia estado a RETIRADO)
        /// SP: TH_Empleado_Retirar
        /// </summary>
        public async Task<(bool success, string message)> RetirarEmpleado(
            string identificacion,
            DateTime fechaRetiro,
            string motivoRetiro,
            string usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Identificacion", identificacion);
            parameters.Add("@FechaRetiro", fechaRetiro);
            parameters.Add("@MotivoRetiro", motivoRetiro);
            parameters.Add("@Usuario", usuario);

            try
            {
                await connection.ExecuteAsync(
                    "TH_Empleado_Retirar",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Empleado retirado exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al retirar empleado: {ex.Message}");
            }
        }

        /// <summary>
        /// Reintegra un empleado retirado
        /// SP: TH_Empleado_Reintegrar
        /// </summary>
        public async Task<(bool success, string message)> ReintegrarEmpleado(
            string identificacion,
            DateTime fechaReintegro,
            string usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Identificacion", identificacion);
            parameters.Add("@FechaReintegro", fechaReintegro);
            parameters.Add("@Usuario", usuario);

            try
            {
                await connection.ExecuteAsync(
                    "TH_Empleado_Reintegrar",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Empleado reintegrado exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al reintegrar empleado: {ex.Message}");
            }
        }

        #endregion

        #region Experiencia Laboral

        /// <summary>
        /// Obtiene experiencia laboral de un empleado
        /// SP: TH_ExperienciaLaboral_Get
        /// </summary>
        public async Task<IEnumerable<ExperienciaLaboralDTO>> ObtenerExperienciaLaboral(string identificacion)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Identificacion", identificacion);

            return await connection.QueryAsync<ExperienciaLaboralDTO>(
                "TH_ExperienciaLaboral_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Guarda experiencia laboral
        /// SP: TH_ExperienciaLaboral_InsertUpdate
        /// </summary>
        public async Task<(bool success, string message)> GuardarExperienciaLaboral(ExperienciaLaboralDTO experiencia)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", experiencia.Id);
            parameters.Add("@Identificacion", experiencia.Identificacion);
            parameters.Add("@Empresa", experiencia.Empresa);
            parameters.Add("@Cargo", experiencia.Cargo);
            parameters.Add("@FechaInicio", experiencia.FechaInicio);
            parameters.Add("@FechaFin", experiencia.FechaFin);
            parameters.Add("@Funciones", experiencia.Funciones);
            parameters.Add("@MotivoRetiro", experiencia.MotivoRetiro);
            parameters.Add("@TelefonoReferencia", experiencia.TelefonoReferencia);

            try
            {
                await connection.ExecuteAsync(
                    "TH_ExperienciaLaboral_InsertUpdate",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Experiencia laboral guardada exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al guardar experiencia: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina experiencia laboral
        /// SP: TH_ExperienciaLaboral_Delete
        /// </summary>
        public async Task<(bool success, string message)> EliminarExperienciaLaboral(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            try
            {
                await connection.ExecuteAsync(
                    "TH_ExperienciaLaboral_Delete",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Experiencia laboral eliminada exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al eliminar experiencia: {ex.Message}");
            }
        }

        #endregion

        #region Educación

        /// <summary>
        /// Obtiene educación de un empleado
        /// SP: TH_Educacion_Get
        /// </summary>
        public async Task<IEnumerable<EducacionDTO>> ObtenerEducacion(string identificacion)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Identificacion", identificacion);

            return await connection.QueryAsync<EducacionDTO>(
                "TH_Educacion_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Guarda información de educación
        /// SP: TH_Educacion_InsertUpdate
        /// </summary>
        public async Task<(bool success, string message)> GuardarEducacion(EducacionDTO educacion)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", educacion.Id);
            parameters.Add("@Identificacion", educacion.Identificacion);
            parameters.Add("@NivelEducativo", educacion.NivelEducativo);
            parameters.Add("@Institucion", educacion.Institucion);
            parameters.Add("@TituloObtenido", educacion.TituloObtenido);
            parameters.Add("@FechaInicio", educacion.FechaInicio);
            parameters.Add("@FechaFin", educacion.FechaFin);
            parameters.Add("@Graduado", educacion.Graduado);
            parameters.Add("@TarjetaProfesional", educacion.TarjetaProfesional);

            try
            {
                await connection.ExecuteAsync(
                    "TH_Educacion_InsertUpdate",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Información de educación guardada exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al guardar educación: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina información de educación
        /// SP: TH_Educacion_Delete
        /// </summary>
        public async Task<(bool success, string message)> EliminarEducacion(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            try
            {
                await connection.ExecuteAsync(
                    "TH_Educacion_Delete",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Información de educación eliminada exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al eliminar educación: {ex.Message}");
            }
        }

        #endregion

        #region Hijos

        /// <summary>
        /// Obtiene hijos de un empleado
        /// SP: TH_Hijos_Get
        /// </summary>
        public async Task<IEnumerable<HijoDTO>> ObtenerHijos(string identificacion)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdentificacionEmpleado", identificacion);

            return await connection.QueryAsync<HijoDTO>(
                "TH_Hijos_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Guarda información de hijo
        /// SP: TH_Hijos_InsertUpdate
        /// </summary>
        public async Task<(bool success, string message)> GuardarHijo(HijoDTO hijo)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", hijo.Id);
            parameters.Add("@IdentificacionEmpleado", hijo.IdentificacionEmpleado);
            parameters.Add("@NombreCompleto", hijo.NombreCompleto);
            parameters.Add("@FechaNacimiento", hijo.FechaNacimiento);
            parameters.Add("@Identificacion", hijo.Identificacion);
            parameters.Add("@Genero", hijo.Genero);
            parameters.Add("@EstudiaActualmente", hijo.EstudiaActualmente);
            parameters.Add("@InstitucionEducativa", hijo.InstitucionEducativa);
            parameters.Add("@Grado", hijo.Grado);

            try
            {
                await connection.ExecuteAsync(
                    "TH_Hijos_InsertUpdate",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Información de hijo guardada exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al guardar información del hijo: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina información de hijo
        /// SP: TH_Hijos_Delete
        /// </summary>
        public async Task<(bool success, string message)> EliminarHijo(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            try
            {
                await connection.ExecuteAsync(
                    "TH_Hijos_Delete",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Información de hijo eliminada exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al eliminar información del hijo: {ex.Message}");
            }
        }

        #endregion

        #region Contactos de Emergencia

        /// <summary>
        /// Obtiene contactos de emergencia de un empleado
        /// SP: TH_ContactosEmergencia_Get
        /// </summary>
        public async Task<IEnumerable<ContactoEmergenciaDTO>> ObtenerContactosEmergencia(string identificacion)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdentificacionEmpleado", identificacion);

            return await connection.QueryAsync<ContactoEmergenciaDTO>(
                "TH_ContactosEmergencia_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Guarda contacto de emergencia
        /// SP: TH_ContactosEmergencia_InsertUpdate
        /// </summary>
        public async Task<(bool success, string message)> GuardarContactoEmergencia(ContactoEmergenciaDTO contacto)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", contacto.Id);
            parameters.Add("@IdentificacionEmpleado", contacto.IdentificacionEmpleado);
            parameters.Add("@NombreCompleto", contacto.NombreCompleto);
            parameters.Add("@Parentesco", contacto.Parentesco);
            parameters.Add("@Telefono", contacto.Telefono);
            parameters.Add("@Celular", contacto.Celular);
            parameters.Add("@Direccion", contacto.Direccion);

            try
            {
                await connection.ExecuteAsync(
                    "TH_ContactosEmergencia_InsertUpdate",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Contacto de emergencia guardado exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al guardar contacto de emergencia: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina contacto de emergencia
        /// SP: TH_ContactosEmergencia_Delete
        /// </summary>
        public async Task<(bool success, string message)> EliminarContactoEmergencia(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            try
            {
                await connection.ExecuteAsync(
                    "TH_ContactosEmergencia_Delete",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Contacto de emergencia eliminado exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al eliminar contacto: {ex.Message}");
            }
        }

        #endregion

        #region Promociones

        /// <summary>
        /// Obtiene historial de promociones/cambios de cargo
        /// SP: TH_Promociones_Get
        /// </summary>
        public async Task<IEnumerable<PromocionDTO>> ObtenerPromociones(string identificacion)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Identificacion", identificacion);

            return await connection.QueryAsync<PromocionDTO>(
                "TH_Promociones_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Registra una promoción/cambio de cargo
        /// SP: TH_Promociones_Insert
        /// </summary>
        public async Task<(bool success, string message)> GuardarPromocion(PromocionDTO promocion, string usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Identificacion", promocion.Identificacion);
            parameters.Add("@CargoAnterior", promocion.CargoAnterior);
            parameters.Add("@CargoNuevo", promocion.CargoNuevo);
            parameters.Add("@FechaCambio", promocion.FechaCambio);
            parameters.Add("@Motivo", promocion.Motivo);
            parameters.Add("@Observaciones", promocion.Observaciones);
            parameters.Add("@Usuario", usuario);

            try
            {
                await connection.ExecuteAsync(
                    "TH_Promociones_Insert",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Promoción registrada exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al registrar promoción: {ex.Message}");
            }
        }

        #endregion

        #region Salarios

        /// <summary>
        /// Obtiene historial de salarios
        /// SP: TH_Salarios_Get
        /// </summary>
        public async Task<IEnumerable<SalarioDTO>> ObtenerSalarios(string identificacion)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Identificacion", identificacion);

            return await connection.QueryAsync<SalarioDTO>(
                "TH_Salarios_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Registra un cambio de salario
        /// SP: TH_Salarios_Insert
        /// </summary>
        public async Task<(bool success, string message)> GuardarSalario(SalarioDTO salario, string usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Identificacion", salario.Identificacion);
            parameters.Add("@SalarioAnterior", salario.SalarioAnterior);
            parameters.Add("@SalarioNuevo", salario.SalarioNuevo);
            parameters.Add("@FechaCambio", salario.FechaCambio);
            parameters.Add("@Motivo", salario.Motivo);
            parameters.Add("@Observaciones", salario.Observaciones);
            parameters.Add("@Usuario", usuario);

            try
            {
                await connection.ExecuteAsync(
                    "TH_Salarios_Insert",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                
                return (true, "Cambio de salario registrado exitosamente");
            }
            catch (SqlException ex)
            {
                return (false, $"Error al registrar cambio de salario: {ex.Message}");
            }
        }

        #endregion

        #region Reportes

        /// <summary>
        /// Obtiene reporte de estado de diligenciamiento de información de empleados
        /// SP: TH_ReporteDiligenciamientoEmpleados_Get
        /// </summary>
        public async Task<IEnumerable<EstadoDiligenciamientoEmpleadoDTO>> ObtenerReporteDiligenciamiento()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<EstadoDiligenciamientoEmpleadoDTO>(
                "TH_ReporteDiligenciamientoEmpleados_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene reporte de información general de empleados
        /// SP: TH_Empleados_Reporte_Info
        /// </summary>
        public async Task<IEnumerable<EmpleadoReporteInfoDTO>> ObtenerReporteInformacionGeneral()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<EmpleadoReporteInfoDTO>(
                "TH_Empleados_Reporte_Info",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene reporte de hijos de empleados
        /// SP: TH_Hijos_Report
        /// </summary>
        public async Task<IEnumerable<EmpleadoHijoReporteDTO>> ObtenerReporteHijos()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<EmpleadoHijoReporteDTO>(
                "TH_Hijos_Report",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene reporte de educación de empleados
        /// SP: TH_Educacion_Report
        /// </summary>
        public async Task<IEnumerable<EmpleadoEducacionReporteDTO>> ObtenerReporteEducacion()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<EmpleadoEducacionReporteDTO>(
                "TH_Educacion_Report",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene reporte de experiencia laboral de empleados
        /// SP: TH_ExperienciaLaboral_Report
        /// </summary>
        public async Task<IEnumerable<EmpleadoExperienciaReporteDTO>> ObtenerReporteExperienciaLaboral()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<EmpleadoExperienciaReporteDTO>(
                "TH_ExperienciaLaboral_Report",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene reporte de contactos de emergencia de empleados
        /// SP: TH_ContactosEmergencia_Report
        /// </summary>
        public async Task<IEnumerable<EmpleadoContactoEmergenciaReporteDTO>> ObtenerReporteContactosEmergencia()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<EmpleadoContactoEmergenciaReporteDTO>(
                "TH_ContactosEmergencia_Report",
                commandType: CommandType.StoredProcedure
            );
        }

        #endregion

        #region Actualización de Datos Maestros

        /// <summary>
        /// Actualiza o crea datos generales del empleado
        /// SP: TH_Empleado_ActualizarDatosGenerales o TH_Empleado_GrabarDatosGenerales
        /// Equivalente a updateDatosGenerales del legacy
        /// </summary>
        public async Task ActualizarDatosGenerales(ActualizarDatosGeneralesDTO datos, long usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@EsNuevo", datos.EsNuevo);
            parameters.Add("@Id", datos.Id);
            parameters.Add("@TipoIdentificacion", datos.TipoIdentificacion);
            parameters.Add("@Identificacion", datos.Identificacion);
            parameters.Add("@PrimerNombre", datos.PrimerNombre);
            parameters.Add("@SegundoNombre", datos.SegundoNombre);
            parameters.Add("@PrimerApellido", datos.PrimerApellido);
            parameters.Add("@SegundoApellido", datos.SegundoApellido);
            parameters.Add("@NombrePreferido", datos.NombrePreferido);
            parameters.Add("@FechaNacimiento", datos.FechaNacimiento);
            parameters.Add("@Genero", datos.Genero);
            parameters.Add("@EstadoCivilId", datos.EstadoCivilId);
            parameters.Add("@GrupoSanguineoId", datos.GrupoSanguineoId);
            parameters.Add("@Nacionalidad", datos.Nacionalidad);
            parameters.Add("@RutaFoto", datos.RutaFoto);
            parameters.Add("@UsuarioId", usuarioId);

            // El SP determinará si es inserción o actualización basado en EsNuevo
            await connection.ExecuteAsync(
                "TH_Empleado_ActualizarDatosGenerales",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Actualiza datos laborales del empleado
        /// SP: TH_Empleado_ActualizarDatosLaborales
        /// Equivalente a updateDatosLaborales del legacy
        /// </summary>
        public async Task ActualizarDatosLaborales(ActualizarDatosLaboralesDTO datos)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@PersonaId", datos.PersonaId);
            parameters.Add("@IdIStaff", datos.IdIStaff);
            parameters.Add("@JefeInmediatoId", datos.JefeInmediatoId);
            parameters.Add("@SedeId", datos.SedeId);
            parameters.Add("@CorreoIpsos", datos.CorreoIpsos);
            parameters.Add("@FechaIngreso", datos.FechaIngreso);
            parameters.Add("@CentroCosto", datos.CentroCosto);
            parameters.Add("@TipoContratoId", datos.TipoContratoId);
            parameters.Add("@TiempoContratoId", datos.TiempoContratoId);
            parameters.Add("@Empresa", datos.Empresa);
            parameters.Add("@JobFunctionId", datos.JobFunctionId);
            parameters.Add("@AreaId", datos.AreaId);
            parameters.Add("@BandaId", datos.BandaId);
            parameters.Add("@CargoId", datos.CargoId);
            parameters.Add("@LevelId", datos.LevelId);
            parameters.Add("@Observaciones", datos.Observaciones);

            await connection.ExecuteAsync(
                "TH_Empleado_ActualizarDatosLaborales",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Actualiza datos personales del empleado
        /// SP: TH_Empleado_ActualizarDatosPersonales
        /// Equivalente a updateDatosPersonales del legacy
        /// </summary>
        public async Task ActualizarDatosPersonales(ActualizarDatosPersonalesDTO datos)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@PersonaId", datos.PersonaId);
            parameters.Add("@MunicipioCiudadId", datos.MunicipioCiudadId);
            parameters.Add("@Direccion", datos.Direccion);
            parameters.Add("@NseId", datos.NseId);
            parameters.Add("@TelefonoFijo", datos.TelefonoFijo);
            parameters.Add("@TelefonoCelular", datos.TelefonoCelular);
            parameters.Add("@EmailPersonal", datos.EmailPersonal);
            parameters.Add("@Barrio", datos.Barrio);
            parameters.Add("@Localidad", datos.Localidad);
            parameters.Add("@MunicipioNacimientoId", datos.MunicipioNacimientoId);
            parameters.Add("@TallaCamisetaId", datos.TallaCamisetaId);

            await connection.ExecuteAsync(
                "TH_Empleado_ActualizarDatosPersonales",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Actualiza información de nómina del empleado
        /// SP: TH_Empleado_ActualizarNomina
        /// Equivalente a updateNomina del legacy
        /// </summary>
        public async Task ActualizarNomina(ActualizarNominaDTO datos)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@PersonaId", datos.PersonaId);
            parameters.Add("@BancoId", datos.BancoId);
            parameters.Add("@TipoCuentaId", datos.TipoCuentaId);
            parameters.Add("@NumeroCuenta", datos.NumeroCuenta);
            parameters.Add("@FondoPensionesId", datos.FondoPensionesId);
            parameters.Add("@FondoCesantiasId", datos.FondoCesantiasId);
            parameters.Add("@EpsId", datos.EpsId);
            parameters.Add("@CajaCompensacionId", datos.CajaCompensacionId);
            parameters.Add("@ArlId", datos.ArlId);

            await connection.ExecuteAsync(
                "TH_Empleado_ActualizarNomina",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Actualiza nivel de inglés del empleado
        /// SP: TH_Empleado_ActualizarNivelIngles
        /// Equivalente a updateNivelIngles del legacy
        /// </summary>
        public async Task ActualizarNivelIngles(ActualizarNivelInglesDTO datos)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@PersonaId", datos.PersonaId);
            parameters.Add("@NivelInglesId", datos.NivelInglesId);

            await connection.ExecuteAsync(
                "TH_Empleado_ActualizarNivelIngles",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        #endregion

        #region Catálogos

        /// <summary>
        /// Obtiene listado de áreas/service lines
        /// SP: TH_Areas_Get
        /// Equivalente a getAreasServiceLines del legacy
        /// </summary>
        public async Task<IEnumerable<AreaServiceLineDTO>> ObtenerAreasServiceLines()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<AreaServiceLineDTO>(
                "TH_Areas_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de grupos sanguíneos
        /// SP: TH_GruposSanguineos_Get
        /// Equivalente a getGruposSanguineos del legacy
        /// </summary>
        public async Task<IEnumerable<GrupoSanguineoDTO>> ObtenerGruposSanguineos()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<GrupoSanguineoDTO>(
                "TH_GruposSanguineos_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de cargos
        /// SP: TH_Cargos_Get
        /// Equivalente a getCargos del legacy
        /// </summary>
        public async Task<IEnumerable<CargoDTO>> ObtenerCargos()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<CargoDTO>(
                "TH_Cargos_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de estados civiles
        /// SP: TH_EstadosCiviles_Get
        /// Equivalente a getEstadosCiviles del legacy
        /// </summary>
        public async Task<IEnumerable<EstadoCivilDTO>> ObtenerEstadosCiviles()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<EstadoCivilDTO>(
                "TH_EstadosCiviles_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de bancos
        /// </summary>
        public async Task<IEnumerable<BancoDTO>> ObtenerBancos()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<BancoDTO>(
                "TH_Bancos_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de tipos de cuenta
        /// </summary>
        public async Task<IEnumerable<TipoCuentaDTO>> ObtenerTiposCuenta()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<TipoCuentaDTO>(
                "TH_TiposCuenta_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de EPS
        /// </summary>
        public async Task<IEnumerable<EpsDTO>> ObtenerEps()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<EpsDTO>(
                "TH_EPS_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de fondos de pensiones
        /// </summary>
        public async Task<IEnumerable<FondoPensionesDTO>> ObtenerFondosPensiones()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<FondoPensionesDTO>(
                "TH_FondosPensiones_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de fondos de cesantías
        /// </summary>
        public async Task<IEnumerable<FondoCesantiasDTO>> ObtenerFondosCesantias()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<FondoCesantiasDTO>(
                "TH_FondosCesantias_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de cajas de compensación
        /// </summary>
        public async Task<IEnumerable<CajaCompensacionDTO>> ObtenerCajasCompensacion()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<CajaCompensacionDTO>(
                "TH_CajasCompensacion_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de ARL
        /// </summary>
        public async Task<IEnumerable<ArlDTO>> ObtenerArls()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<ArlDTO>(
                "TH_ARL_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de niveles de inglés
        /// </summary>
        public async Task<IEnumerable<NivelInglesDTO>> ObtenerNivelesIngles()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<NivelInglesDTO>(
                "TH_NivelesIngles_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de sedes
        /// </summary>
        public async Task<IEnumerable<SedeDTO>> ObtenerSedes()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<SedeDTO>(
                "TH_Sedes_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de tipos de contrato
        /// </summary>
        public async Task<IEnumerable<TipoContratoDTO>> ObtenerTiposContrato()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<TipoContratoDTO>(
                "TH_TiposContrato_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de NSE
        /// </summary>
        public async Task<IEnumerable<NseDTO>> ObtenerNse()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<NseDTO>(
                "TH_NSE_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de tallas de camiseta
        /// </summary>
        public async Task<IEnumerable<TallaCamisetaDTO>> ObtenerTallasCamiseta()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<TallaCamisetaDTO>(
                "TH_TallasCamiseta_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de bandas
        /// </summary>
        public async Task<IEnumerable<BandaDTO>> ObtenerBandas()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<BandaDTO>(
                "TH_Bandas_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Obtiene listado de levels
        /// </summary>
        public async Task<IEnumerable<LevelDTO>> ObtenerLevels()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<LevelDTO>(
                "TH_Levels_Get",
                commandType: CommandType.StoredProcedure
            );
        }

        #endregion
    }
}
