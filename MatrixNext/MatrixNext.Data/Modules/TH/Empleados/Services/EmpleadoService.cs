using MatrixNext.Data.Modules.TH.Empleados.Adapters;
using MatrixNext.Data.Modules.TH.Empleados.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MatrixNext.Data.Modules.TH.Empleados.Services
{
    /// <summary>
    /// Servicio de negocio para gestión de empleados.
    /// Implementa validaciones y reglas de negocio antes de llamar al adaptador.
    /// </summary>
    public class EmpleadoService
    {
        private readonly EmpleadoDataAdapter _adapter;
        private readonly string _employeePhotosPhysicalPath;
        private readonly string _employeePhotosWebPath;

        public EmpleadoService(EmpleadoDataAdapter adapter, IConfiguration configuration)
        {
            _adapter = adapter;

            // Leer ruta de fotos desde configuración, con fallback a wwwroot/fotos/empleados
            var configuredPath = configuration?["Files:EmployeePhotosPath"];
            if (!string.IsNullOrWhiteSpace(configuredPath))
            {
                // Si la ruta es relativa a la web root, convertirla a ruta física
                if (!Path.IsPathRooted(configuredPath))
                {
                    var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    _employeePhotosPhysicalPath = Path.GetFullPath(Path.Combine(webRoot, configuredPath.TrimStart('/', '\\')));
                }
                else
                {
                    _employeePhotosPhysicalPath = configuredPath;
                }
            }
            else
            {
                _employeePhotosPhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fotos", "empleados");
            }

            // Asegurar que exista el directorio
            try
            {
                Directory.CreateDirectory(_employeePhotosPhysicalPath);
            }
            catch
            {
                // Ignorar errores de creación; el guardado lanzará excepción si es necesario
            }

            // Ruta web relativa usada en la BD (por ejemplo: /fotos/empleados/{fileName})
            _employeePhotosWebPath = "/fotos/empleados";
        }

        #region Empleados - CRUD Principal

        /// <summary>
        /// Obtiene listado de empleados con filtros opcionales
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<EmpleadoResumenDTO>? data)> ObtenerEmpleados(
            EmpleadoFiltroDTO? filtro = null)
        {
            try
            {
                var empleados = await _adapter.ObtenerEmpleados(filtro);
                return (true, "Empleados obtenidos exitosamente", empleados);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener empleados: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Obtiene empleado por identificación
        /// </summary>
        public async Task<(bool success, string message, EmpleadoDetalleDTO? data)> ObtenerEmpleadoPorIdentificacion(
            string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                return (false, "La identificación es requerida", null);
            }

            try
            {
                var empleado = await _adapter.ObtenerEmpleadoPorIdentificacion(identificacion);
                if (empleado == null)
                {
                    return (false, "Empleado no encontrado", null);
                }
                return (true, "Empleado encontrado", empleado);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener empleado: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Guarda un empleado (crear o actualizar)
        /// </summary>
        public async Task<(bool success, string message)> GuardarEmpleado(
            EmpleadoDetalleDTO empleado,
            string usuario)
        {
            // Validaciones básicas
            var (esValido, mensajeValidacion) = ValidarEmpleado(empleado);
            if (!esValido)
            {
                return (false, mensajeValidacion);
            }

            if (string.IsNullOrWhiteSpace(usuario))
            {
                return (false, "Usuario es requerido");
            }

            try
            {
                return await _adapter.GuardarEmpleado(empleado, usuario);
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar empleado: {ex.Message}");
            }
        }

        /// <summary>
        /// Retira un empleado
        /// </summary>
        public async Task<(bool success, string message)> RetirarEmpleado(
            string identificacion,
            DateTime fechaRetiro,
            string motivoRetiro,
            string usuario)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                return (false, "La identificación es requerida");
            }

            if (fechaRetiro > DateTime.Now)
            {
                return (false, "La fecha de retiro no puede ser futura");
            }

            if (string.IsNullOrWhiteSpace(motivoRetiro))
            {
                return (false, "El motivo de retiro es requerido");
            }

            if (string.IsNullOrWhiteSpace(usuario))
            {
                return (false, "Usuario es requerido");
            }

            // Verificar que el empleado existe y está activo
            var (existe, mensaje, empleado) = await ObtenerEmpleadoPorIdentificacion(identificacion);
            if (!existe || empleado == null)
            {
                return (false, "Empleado no encontrado");
            }

            if (empleado.Estado == "RETIRADO")
            {
                return (false, "El empleado ya se encuentra retirado");
            }

            if (empleado.FechaIngreso.HasValue && fechaRetiro < empleado.FechaIngreso.Value)
            {
                return (false, "La fecha de retiro no puede ser anterior a la fecha de ingreso");
            }

            try
            {
                return await _adapter.RetirarEmpleado(identificacion, fechaRetiro, motivoRetiro, usuario);
            }
            catch (Exception ex)
            {
                return (false, $"Error al retirar empleado: {ex.Message}");
            }
        }

        /// <summary>
        /// Reintegra un empleado retirado
        /// </summary>
        public async Task<(bool success, string message)> ReintegrarEmpleado(
            string identificacion,
            DateTime fechaReintegro,
            string usuario)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                return (false, "La identificación es requerida");
            }

            if (fechaReintegro > DateTime.Now.AddDays(30))
            {
                return (false, "La fecha de reintegro no puede ser mayor a 30 días en el futuro");
            }

            if (string.IsNullOrWhiteSpace(usuario))
            {
                return (false, "Usuario es requerido");
            }

            // Verificar que el empleado existe y está retirado
            var (existe, mensaje, empleado) = await ObtenerEmpleadoPorIdentificacion(identificacion);
            if (!existe || empleado == null)
            {
                return (false, "Empleado no encontrado");
            }

            if (empleado.Estado != "RETIRADO")
            {
                return (false, "El empleado no se encuentra retirado");
            }

            if (empleado.FechaRetiro.HasValue && fechaReintegro < empleado.FechaRetiro.Value)
            {
                return (false, "La fecha de reintegro no puede ser anterior a la fecha de retiro");
            }

            try
            {
                return await _adapter.ReintegrarEmpleado(identificacion, fechaReintegro, usuario);
            }
            catch (Exception ex)
            {
                return (false, $"Error al reintegrar empleado: {ex.Message}");
            }
        }

        #endregion

        #region Experiencia Laboral

        /// <summary>
        /// Obtiene experiencia laboral de un empleado
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<ExperienciaLaboralDTO>? data)> ObtenerExperienciaLaboral(
            string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                return (false, "La identificación es requerida", null);
            }

            try
            {
                var experiencias = await _adapter.ObtenerExperienciaLaboral(identificacion);
                return (true, "Experiencia laboral obtenida exitosamente", experiencias);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener experiencia laboral: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Guarda experiencia laboral
        /// </summary>
        public async Task<(bool success, string message)> GuardarExperienciaLaboral(ExperienciaLaboralDTO experiencia)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(experiencia.Identificacion))
            {
                return (false, "La identificación del empleado es requerida");
            }

            if (string.IsNullOrWhiteSpace(experiencia.Empresa))
            {
                return (false, "El nombre de la empresa es requerido");
            }

            if (string.IsNullOrWhiteSpace(experiencia.Cargo))
            {
                return (false, "El cargo es requerido");
            }

            if (experiencia.FechaInicio == default)
            {
                return (false, "La fecha de inicio es requerida");
            }

            if (experiencia.FechaFin.HasValue && experiencia.FechaFin.Value < experiencia.FechaInicio)
            {
                return (false, "La fecha de fin no puede ser anterior a la fecha de inicio");
            }

            try
            {
                return await _adapter.GuardarExperienciaLaboral(experiencia);
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar experiencia laboral: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina experiencia laboral
        /// </summary>
        public async Task<(bool success, string message)> EliminarExperienciaLaboral(int id)
        {
            if (id <= 0)
            {
                return (false, "ID inválido");
            }

            try
            {
                return await _adapter.EliminarExperienciaLaboral(id);
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar experiencia laboral: {ex.Message}");
            }
        }

        #endregion

        #region Educación

        /// <summary>
        /// Obtiene educación de un empleado
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<EducacionDTO>? data)> ObtenerEducacion(
            string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                return (false, "La identificación es requerida", null);
            }

            try
            {
                var educacion = await _adapter.ObtenerEducacion(identificacion);
                return (true, "Educación obtenida exitosamente", educacion);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener educación: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Guarda información de educación
        /// </summary>
        public async Task<(bool success, string message)> GuardarEducacion(EducacionDTO educacion)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(educacion.Identificacion))
            {
                return (false, "La identificación del empleado es requerida");
            }

            if (string.IsNullOrWhiteSpace(educacion.NivelEducativo))
            {
                return (false, "El nivel educativo es requerido");
            }

            if (string.IsNullOrWhiteSpace(educacion.Institucion))
            {
                return (false, "La institución es requerida");
            }

            if (string.IsNullOrWhiteSpace(educacion.TituloObtenido))
            {
                return (false, "El título obtenido es requerido");
            }

            if (educacion.FechaFin.HasValue && educacion.FechaInicio.HasValue &&
                educacion.FechaFin.Value < educacion.FechaInicio.Value)
            {
                return (false, "La fecha de fin no puede ser anterior a la fecha de inicio");
            }

            try
            {
                return await _adapter.GuardarEducacion(educacion);
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar educación: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina información de educación
        /// </summary>
        public async Task<(bool success, string message)> EliminarEducacion(int id)
        {
            if (id <= 0)
            {
                return (false, "ID inválido");
            }

            try
            {
                return await _adapter.EliminarEducacion(id);
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar educación: {ex.Message}");
            }
        }

        #endregion

        #region Hijos

        /// <summary>
        /// Obtiene hijos de un empleado
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<HijoDTO>? data)> ObtenerHijos(
            string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                return (false, "La identificación es requerida", null);
            }

            try
            {
                var hijos = await _adapter.ObtenerHijos(identificacion);
                return (true, "Hijos obtenidos exitosamente", hijos);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener hijos: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Guarda información de hijo
        /// </summary>
        public async Task<(bool success, string message)> GuardarHijo(HijoDTO hijo)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(hijo.IdentificacionEmpleado))
            {
                return (false, "La identificación del empleado es requerida");
            }

            if (string.IsNullOrWhiteSpace(hijo.NombreCompleto))
            {
                return (false, "El nombre completo del hijo es requerido");
            }

            if (hijo.FechaNacimiento == default)
            {
                return (false, "La fecha de nacimiento es requerida");
            }

            if (hijo.FechaNacimiento > DateTime.Now)
            {
                return (false, "La fecha de nacimiento no puede ser futura");
            }

            if (string.IsNullOrWhiteSpace(hijo.Genero))
            {
                return (false, "El género es requerido");
            }

            try
            {
                return await _adapter.GuardarHijo(hijo);
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar información del hijo: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina información de hijo
        /// </summary>
        public async Task<(bool success, string message)> EliminarHijo(int id)
        {
            if (id <= 0)
            {
                return (false, "ID inválido");
            }

            try
            {
                return await _adapter.EliminarHijo(id);
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar información del hijo: {ex.Message}");
            }
        }

        #endregion

        #region Contactos de Emergencia

        /// <summary>
        /// Obtiene contactos de emergencia de un empleado
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<ContactoEmergenciaDTO>? data)> ObtenerContactosEmergencia(
            string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                return (false, "La identificación es requerida", null);
            }

            try
            {
                var contactos = await _adapter.ObtenerContactosEmergencia(identificacion);
                return (true, "Contactos obtenidos exitosamente", contactos);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener contactos de emergencia: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Guarda contacto de emergencia
        /// </summary>
        public async Task<(bool success, string message)> GuardarContactoEmergencia(ContactoEmergenciaDTO contacto)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(contacto.IdentificacionEmpleado))
            {
                return (false, "La identificación del empleado es requerida");
            }

            if (string.IsNullOrWhiteSpace(contacto.NombreCompleto))
            {
                return (false, "El nombre completo es requerido");
            }

            if (string.IsNullOrWhiteSpace(contacto.Parentesco))
            {
                return (false, "El parentesco es requerido");
            }

            if (string.IsNullOrWhiteSpace(contacto.Telefono))
            {
                return (false, "El teléfono es requerido");
            }

            try
            {
                return await _adapter.GuardarContactoEmergencia(contacto);
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar contacto de emergencia: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina contacto de emergencia
        /// </summary>
        public async Task<(bool success, string message)> EliminarContactoEmergencia(int id)
        {
            if (id <= 0)
            {
                return (false, "ID inválido");
            }

            try
            {
                return await _adapter.EliminarContactoEmergencia(id);
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar contacto de emergencia: {ex.Message}");
            }
        }

        #endregion

        #region Promociones

        /// <summary>
        /// Obtiene historial de promociones
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<PromocionDTO>? data)> ObtenerPromociones(
            string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                return (false, "La identificación es requerida", null);
            }

            try
            {
                var promociones = await _adapter.ObtenerPromociones(identificacion);
                return (true, "Promociones obtenidas exitosamente", promociones);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener promociones: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Guarda una promoción
        /// </summary>
        public async Task<(bool success, string message)> GuardarPromocion(PromocionDTO promocion, string usuario)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(promocion.Identificacion))
            {
                return (false, "La identificación del empleado es requerida");
            }

            if (promocion.CargoAnterior <= 0 || promocion.CargoNuevo <= 0)
            {
                return (false, "Los cargos son requeridos");
            }

            if (promocion.CargoAnterior == promocion.CargoNuevo)
            {
                return (false, "El cargo nuevo debe ser diferente al cargo anterior");
            }

            if (promocion.FechaCambio == default)
            {
                return (false, "La fecha de cambio es requerida");
            }

            if (string.IsNullOrWhiteSpace(usuario))
            {
                return (false, "Usuario es requerido");
            }

            try
            {
                return await _adapter.GuardarPromocion(promocion, usuario);
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar promoción: {ex.Message}");
            }
        }

        #endregion

        #region Salarios

        /// <summary>
        /// Obtiene historial de salarios
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<SalarioDTO>? data)> ObtenerSalarios(
            string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                return (false, "La identificación es requerida", null);
            }

            try
            {
                var salarios = await _adapter.ObtenerSalarios(identificacion);
                return (true, "Salarios obtenidos exitosamente", salarios);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener salarios: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Guarda un cambio de salario
        /// </summary>
        public async Task<(bool success, string message)> GuardarSalario(SalarioDTO salario, string usuario)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(salario.Identificacion))
            {
                return (false, "La identificación del empleado es requerida");
            }

            if (salario.SalarioAnterior <= 0 || salario.SalarioNuevo <= 0)
            {
                return (false, "Los salarios deben ser mayores a cero");
            }

            if (salario.SalarioAnterior == salario.SalarioNuevo)
            {
                return (false, "El salario nuevo debe ser diferente al salario anterior");
            }

            if (salario.FechaCambio == default)
            {
                return (false, "La fecha de cambio es requerida");
            }

            if (string.IsNullOrWhiteSpace(usuario))
            {
                return (false, "Usuario es requerido");
            }

            try
            {
                return await _adapter.GuardarSalario(salario, usuario);
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar cambio de salario: {ex.Message}");
            }
        }

        #endregion

        #region Reportes

        /// <summary>
        /// Obtiene reporte de estado de diligenciamiento
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<EstadoDiligenciamientoEmpleadoDTO>? data)> 
            ObtenerReporteDiligenciamiento()
        {
            try
            {
                var reporte = await _adapter.ObtenerReporteDiligenciamiento();
                return (true, "Reporte obtenido exitosamente", reporte);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener reporte: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Obtiene reporte de información general de empleados
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<EmpleadoReporteInfoDTO>? data)> 
            ObtenerReporteInformacionGeneral()
        {
            try
            {
                var reporte = await _adapter.ObtenerReporteInformacionGeneral();
                return (true, "Reporte obtenido exitosamente", reporte);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener reporte: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Obtiene reporte de hijos de empleados
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<EmpleadoHijoReporteDTO>? data)> 
            ObtenerReporteHijos()
        {
            try
            {
                var reporte = await _adapter.ObtenerReporteHijos();
                return (true, "Reporte obtenido exitosamente", reporte);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener reporte: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Obtiene reporte de educación de empleados
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<EmpleadoEducacionReporteDTO>? data)> 
            ObtenerReporteEducacion()
        {
            try
            {
                var reporte = await _adapter.ObtenerReporteEducacion();
                return (true, "Reporte obtenido exitosamente", reporte);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener reporte: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Obtiene reporte de experiencia laboral de empleados
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<EmpleadoExperienciaReporteDTO>? data)> 
            ObtenerReporteExperienciaLaboral()
        {
            try
            {
                var reporte = await _adapter.ObtenerReporteExperienciaLaboral();
                return (true, "Reporte obtenido exitosamente", reporte);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener reporte: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Obtiene reporte de contactos de emergencia de empleados
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<EmpleadoContactoEmergenciaReporteDTO>? data)> 
            ObtenerReporteContactosEmergencia()
        {
            try
            {
                var reporte = await _adapter.ObtenerReporteContactosEmergencia();
                return (true, "Reporte obtenido exitosamente", reporte);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener reporte: {ex.Message}", null);
            }
        }

        #endregion

        #region Actualización de Datos Maestros

        /// <summary>
        /// Actualiza datos generales del empleado
        /// Incluye manejo de foto en base64
        /// </summary>
        public async Task<(bool success, string message)> ActualizarDatosGenerales(
            ActualizarDatosGeneralesDTO datos, long usuarioId)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(datos.TipoIdentificacion))
            {
                return (false, "El tipo de identificación es requerido");
            }

            if (string.IsNullOrWhiteSpace(datos.Identificacion))
            {
                return (false, "La identificación es requerida");
            }

            if (string.IsNullOrWhiteSpace(datos.PrimerNombre))
            {
                return (false, "El primer nombre es requerido");
            }

            if (string.IsNullOrWhiteSpace(datos.PrimerApellido))
            {
                return (false, "El primer apellido es requerido");
            }

            if (datos.FechaNacimiento.HasValue && datos.FechaNacimiento.Value > DateTime.Now.AddYears(-18))
            {
                return (false, "El empleado debe ser mayor de 18 años");
            }

            try
            {
                // Procesar foto si viene en base64
                if (!string.IsNullOrWhiteSpace(datos.FotoBase64))
                {
                    // Soporta data URIs como: data:image/jpeg;base64,/9j/4AAQ...
                    var base64 = datos.FotoBase64;
                    var commaIndex = base64.IndexOf(',');
                    if (commaIndex >= 0)
                    {
                        base64 = base64.Substring(commaIndex + 1);
                    }

                    byte[] fotoBytes;
                    try
                    {
                        fotoBytes = Convert.FromBase64String(base64);
                    }
                    catch
                    {
                        return (false, "El formato de la imagen (base64) es inválido");
                    }

                    // Validación de tamaño (2 MB)
                    const int maxBytes = 2 * 1024 * 1024;
                    if (fotoBytes.Length > maxBytes)
                    {
                        return (false, "La imagen excede el tamaño máximo permitido (2 MB)");
                    }

                    // Detectar tipo por firma (JPEG/PNG)
                    string extension;
                    if (fotoBytes.Length >= 3 && fotoBytes[0] == 0xFF && fotoBytes[1] == 0xD8 && fotoBytes[2] == 0xFF)
                    {
                        extension = ".jpg";
                    }
                    else if (fotoBytes.Length >= 8 && fotoBytes[0] == 0x89 && fotoBytes[1] == 0x50 && fotoBytes[2] == 0x4E && fotoBytes[3] == 0x47)
                    {
                        extension = ".png";
                    }
                    else
                    {
                        return (false, "Formato de imagen no soportado. Use JPG o PNG.");
                    }

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var physicalPath = Path.Combine(_employeePhotosPhysicalPath, fileName);

                    try
                    {
                        File.WriteAllBytes(physicalPath, fotoBytes);
                    }
                    catch (Exception ex)
                    {
                        return (false, $"Error al guardar la imagen: {ex.Message}");
                    }

                    // Asignar ruta web relativa (asume carpeta /fotos/empleados)
                    datos.RutaFoto = $"{_employeePhotosWebPath}/{fileName}";
                }

                await _adapter.ActualizarDatosGenerales(datos, usuarioId);
                return (true, datos.EsNuevo ? "Empleado creado exitosamente" : "Datos generales actualizados exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar datos generales: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza datos laborales del empleado
        /// </summary>
        public async Task<(bool success, string message)> ActualizarDatosLaborales(ActualizarDatosLaboralesDTO datos)
        {
            if (datos.PersonaId <= 0)
            {
                return (false, "El ID de la persona es requerido");
            }

            if (datos.FechaIngreso.HasValue && datos.FechaIngreso.Value > DateTime.Now.AddDays(30))
            {
                return (false, "La fecha de ingreso no puede ser mayor a 30 días en el futuro");
            }

            if (!string.IsNullOrWhiteSpace(datos.CorreoIpsos) && !EsEmailValido(datos.CorreoIpsos))
            {
                return (false, "El formato del correo Ipsos es inválido");
            }

            try
            {
                await _adapter.ActualizarDatosLaborales(datos);
                return (true, "Datos laborales actualizados exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar datos laborales: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza datos personales del empleado
        /// </summary>
        public async Task<(bool success, string message)> ActualizarDatosPersonales(ActualizarDatosPersonalesDTO datos)
        {
            if (datos.PersonaId <= 0)
            {
                return (false, "El ID de la persona es requerido");
            }

            if (!string.IsNullOrWhiteSpace(datos.EmailPersonal) && !EsEmailValido(datos.EmailPersonal))
            {
                return (false, "El formato del email personal es inválido");
            }

            try
            {
                await _adapter.ActualizarDatosPersonales(datos);
                return (true, "Datos personales actualizados exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar datos personales: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza información de nómina del empleado
        /// </summary>
        public async Task<(bool success, string message)> ActualizarNomina(ActualizarNominaDTO datos)
        {
            if (datos.PersonaId <= 0)
            {
                return (false, "El ID de la persona es requerido");
            }

            try
            {
                await _adapter.ActualizarNomina(datos);
                return (true, "Información de nómina actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar nómina: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza nivel de inglés del empleado
        /// </summary>
        public async Task<(bool success, string message)> ActualizarNivelIngles(ActualizarNivelInglesDTO datos)
        {
            if (datos.PersonaId <= 0)
            {
                return (false, "El ID de la persona es requerido");
            }

            try
            {
                await _adapter.ActualizarNivelIngles(datos);
                return (true, "Nivel de inglés actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar nivel de inglés: {ex.Message}");
            }
        }

        #endregion

        #region Catálogos

        /// <summary>
        /// Obtiene todas las áreas/service lines
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<AreaServiceLineDTO>? data)> ObtenerAreasServiceLines()
        {
            try
            {
                var areas = await _adapter.ObtenerAreasServiceLines();
                return (true, "Áreas obtenidas exitosamente", areas);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener áreas: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Obtiene todos los grupos sanguíneos
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<GrupoSanguineoDTO>? data)> ObtenerGruposSanguineos()
        {
            try
            {
                var grupos = await _adapter.ObtenerGruposSanguineos();
                return (true, "Grupos sanguíneos obtenidos exitosamente", grupos);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener grupos sanguíneos: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Obtiene todos los cargos
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<CargoDTO>? data)> ObtenerCargos()
        {
            try
            {
                var cargos = await _adapter.ObtenerCargos();
                return (true, "Cargos obtenidos exitosamente", cargos);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener cargos: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Obtiene todos los estados civiles
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<EstadoCivilDTO>? data)> ObtenerEstadosCiviles()
        {
            try
            {
                var estados = await _adapter.ObtenerEstadosCiviles();
                return (true, "Estados civiles obtenidos exitosamente", estados);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener estados civiles: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Obtiene todos los catálogos necesarios en un solo método
        /// </summary>
        public async Task<(bool success, string message, object? data)> ObtenerTodosCatalogos()
        {
            try
            {
                var catalogos = new
                {
                    Areas = await _adapter.ObtenerAreasServiceLines(),
                    GruposSanguineos = await _adapter.ObtenerGruposSanguineos(),
                    Cargos = await _adapter.ObtenerCargos(),
                    EstadosCiviles = await _adapter.ObtenerEstadosCiviles(),
                    Bancos = await _adapter.ObtenerBancos(),
                    TiposCuenta = await _adapter.ObtenerTiposCuenta(),
                    Eps = await _adapter.ObtenerEps(),
                    FondosPensiones = await _adapter.ObtenerFondosPensiones(),
                    FondosCesantias = await _adapter.ObtenerFondosCesantias(),
                    CajasCompensacion = await _adapter.ObtenerCajasCompensacion(),
                    Arls = await _adapter.ObtenerArls(),
                    NivelesIngles = await _adapter.ObtenerNivelesIngles(),
                    Sedes = await _adapter.ObtenerSedes(),
                    TiposContrato = await _adapter.ObtenerTiposContrato(),
                    Nse = await _adapter.ObtenerNse(),
                    TallasCamiseta = await _adapter.ObtenerTallasCamiseta(),
                    Bandas = await _adapter.ObtenerBandas(),
                    Levels = await _adapter.ObtenerLevels()
                };

                return (true, "Catálogos obtenidos exitosamente", catalogos);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener catálogos: {ex.Message}", null);
            }
        }

        #endregion

        #region Validaciones Privadas

        /// <summary>
        /// Valida datos básicos del empleado
        /// </summary>
        private (bool esValido, string mensaje) ValidarEmpleado(EmpleadoDetalleDTO empleado)
        {
            if (string.IsNullOrWhiteSpace(empleado.TipoIdentificacion))
            {
                return (false, "El tipo de identificación es requerido");
            }

            if (string.IsNullOrWhiteSpace(empleado.Identificacion))
            {
                return (false, "La identificación es requerida");
            }

            if (string.IsNullOrWhiteSpace(empleado.PrimerNombre))
            {
                return (false, "El primer nombre es requerido");
            }

            if (string.IsNullOrWhiteSpace(empleado.PrimerApellido))
            {
                return (false, "El primer apellido es requerido");
            }

            if (empleado.FechaNacimiento.HasValue && empleado.FechaNacimiento.Value > DateTime.Now.AddYears(-18))
            {
                return (false, "El empleado debe ser mayor de 18 años");
            }

            if (empleado.FechaIngreso.HasValue && empleado.FechaIngreso.Value > DateTime.Now.AddDays(30))
            {
                return (false, "La fecha de ingreso no puede ser mayor a 30 días en el futuro");
            }

            if (!string.IsNullOrWhiteSpace(empleado.Email) && !EsEmailValido(empleado.Email))
            {
                return (false, "El formato del email es inválido");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Valida formato de email
        /// </summary>
        private bool EsEmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
