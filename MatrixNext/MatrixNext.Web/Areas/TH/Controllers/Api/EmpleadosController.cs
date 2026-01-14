using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.TH.Models;
using MatrixNext.Data.Services.TH.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.TH.Controllers.Api
{
    /// <summary>
    /// Controller para gestión de Empleados y datos complementarios
    /// Endpoints para CRUD de empleados, experiencia, educación, hijos, contactos, promociones y salarios
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/th/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly IThEmpleadosService _service;
        private readonly ILogger<EmpleadosController> _logger;

        public EmpleadosController(IThEmpleadosService service, ILogger<EmpleadosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region EMPLEADO PRINCIPAL

        /// <summary>
        /// GET /api/th/empleados - Obtiene lista de empleados con filtros opcionales
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EmpleadoDto>>>> GetEmpleados(
            [FromQuery] long? id = null,
            [FromQuery] string nombres = null,
            [FromQuery] string apellidos = null,
            [FromQuery] bool? activo = null,
            [FromQuery] byte? serviceLive = null,
            [FromQuery] short? cargo = null,
            [FromQuery] byte? sede = null)
        {
            try
            {
                var resultado = await _service.ObtenerEmpleados(id, nombres, apellidos, activo, serviceLive, cargo, sede);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetEmpleados");
                return StatusCode(500, ApiResponse<List<EmpleadoDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/empleados/{id} - Obtiene un empleado específico
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmpleadoDto>>> GetEmpleado(long id)
        {
            try
            {
                var resultado = await _service.ObtenerEmpleadoPorId(id);
                return resultado.Success ? Ok(resultado) : NotFound(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetEmpleado {id}");
                return StatusCode(500, ApiResponse<EmpleadoDto>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// POST /api/th/empleados - Crea un nuevo empleado
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<long>>> PostEmpleado([FromBody] EmpleadoInputDto input)
        {
            try
            {
                var resultado = await _service.CrearEmpleado(input);
                return resultado.Success ? Created($"api/th/empleados/{resultado.Data}", resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PostEmpleado");
                return StatusCode(500, ApiResponse<long>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// PUT /api/th/empleados/{id}/datos-generales - Actualiza datos generales
        /// </summary>
        [HttpPut("{id}/datos-generales")]
        public async Task<ActionResult<ApiResponse<bool>>> PutDatosGenerales(long id, [FromBody] EmpleadoInputDto input)
        {
            try
            {
                var resultado = await _service.ActualizarDatosGenerales(id, input);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PutDatosGenerales {id}");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// PUT /api/th/empleados/{id}/datos-laborales - Actualiza datos laborales
        /// </summary>
        [HttpPut("{id}/datos-laborales")]
        public async Task<ActionResult<ApiResponse<bool>>> PutDatosLaborales(long id, [FromBody] EmpleadoDatosLaboralesInputDto input)
        {
            try
            {
                input.Id = id;
                var resultado = await _service.ActualizarDatosLaborales(input);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PutDatosLaborales {id}");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// PUT /api/th/empleados/{id}/datos-personales - Actualiza datos personales
        /// </summary>
        [HttpPut("{id}/datos-personales")]
        public async Task<ActionResult<ApiResponse<bool>>> PutDatosPersonales(long id, [FromBody] EmpleadoDatosPersonalesInputDto input)
        {
            try
            {
                input.Id = id;
                var resultado = await _service.ActualizarDatosPersonales(input);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PutDatosPersonales {id}");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// PUT /api/th/empleados/{id}/nomina - Actualiza datos de nómina
        /// </summary>
        [HttpPut("{id}/nomina")]
        public async Task<ActionResult<ApiResponse<bool>>> PutNomina(long id, [FromBody] EmpleadoNominaInputDto input)
        {
            try
            {
                input.Id = id;
                var resultado = await _service.ActualizarNomina(input);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PutNomina {id}");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// PUT /api/th/empleados/{id}/salario - Actualiza salario
        /// </summary>
        [HttpPut("{id}/salario")]
        public async Task<ActionResult<ApiResponse<bool>>> PutSalario(long id, [FromBody] EmpleadoActualizarSalarioInputDto input)
        {
            try
            {
                input.EmpleadoId = id;
                var resultado = await _service.ActualizarSalario(input);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PutSalario {id}");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// PUT /api/th/empleados/{id}/retirar - Retira un empleado
        /// </summary>
        [HttpPut("{id}/retirar")]
        public async Task<ActionResult<ApiResponse<bool>>> PutRetirar(long id, [FromBody] dynamic input)
        {
            try
            {
                DateTime fechaRetiro = input.fechaRetiro;
                string observacion = input.observacion ?? "";
                var resultado = await _service.RetirarEmpleado(id, fechaRetiro, observacion);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PutRetirar {id}");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// PUT /api/th/empleados/{id}/reintegrar - Reintegra un empleado
        /// </summary>
        [HttpPut("{id}/reintegrar")]
        public async Task<ActionResult<ApiResponse<bool>>> PutReintegrar(long id, [FromBody] dynamic input)
        {
            try
            {
                DateTime fechaReintegro = input.fechaReintegro;
                var resultado = await _service.ReintegrarEmpleado(id, fechaReintegro);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PutReintegrar {id}");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        #endregion

        #region EXPERIENCIA LABORAL

        /// <summary>
        /// GET /api/th/empleados/{id}/experiencias - Obtiene experiencias laborales
        /// </summary>
        [HttpGet("{id}/experiencias")]
        public async Task<ActionResult<ApiResponse<List<ExperienciaLaboralDto>>>> GetExperiencias(long id)
        {
            try
            {
                var resultado = await _service.ObtenerExperienciasLaborales(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetExperiencias {id}");
                return StatusCode(500, ApiResponse<List<ExperienciaLaboralDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// POST /api/th/empleados/{id}/experiencias - Agrega experiencia laboral
        /// </summary>
        [HttpPost("{id}/experiencias")]
        public async Task<ActionResult<ApiResponse<long>>> PostExperiencia(long id, [FromBody] ExperienciaLaboralInputDto input)
        {
            try
            {
                input.PersonaId = id;
                var resultado = await _service.AgregarExperienciaLaboral(input);
                return resultado.Success ? Created($"api/th/empleados/{id}/experiencias/{resultado.Data}", resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PostExperiencia {id}");
                return StatusCode(500, ApiResponse<long>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// DELETE /api/th/empleados/{id}/experiencias/{experienciaId} - Elimina experiencia
        /// </summary>
        [HttpDelete("{id}/experiencias/{experienciaId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteExperiencia(long id, long experienciaId)
        {
            try
            {
                var resultado = await _service.EliminarExperienciaLaboral(experienciaId);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en DeleteExperiencia");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        #endregion

        #region EDUCACIÓN

        /// <summary>
        /// GET /api/th/empleados/{id}/educaciones - Obtiene educación
        /// </summary>
        [HttpGet("{id}/educaciones")]
        public async Task<ActionResult<ApiResponse<List<EducacionDto>>>> GetEducaciones(long id)
        {
            try
            {
                var resultado = await _service.ObtenerEducacion(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetEducaciones {id}");
                return StatusCode(500, ApiResponse<List<EducacionDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// POST /api/th/empleados/{id}/educaciones - Agrega educación
        /// </summary>
        [HttpPost("{id}/educaciones")]
        public async Task<ActionResult<ApiResponse<long>>> PostEducacion(long id, [FromBody] EducacionInputDto input)
        {
            try
            {
                input.PersonaId = id;
                var resultado = await _service.AgregarEducacion(input);
                return resultado.Success ? Created($"api/th/empleados/{id}/educaciones/{resultado.Data}", resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PostEducacion {id}");
                return StatusCode(500, ApiResponse<long>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// DELETE /api/th/empleados/{id}/educaciones/{educacionId} - Elimina educación
        /// </summary>
        [HttpDelete("{id}/educaciones/{educacionId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteEducacion(long id, long educacionId)
        {
            try
            {
                var resultado = await _service.EliminarEducacion(educacionId);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en DeleteEducacion");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        #endregion

        #region HIJOS

        /// <summary>
        /// GET /api/th/empleados/{id}/hijos - Obtiene hijos
        /// </summary>
        [HttpGet("{id}/hijos")]
        public async Task<ActionResult<ApiResponse<List<HijoDto>>>> GetHijos(long id)
        {
            try
            {
                var resultado = await _service.ObtenerHijos(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetHijos {id}");
                return StatusCode(500, ApiResponse<List<HijoDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// POST /api/th/empleados/{id}/hijos - Agrega hijo
        /// </summary>
        [HttpPost("{id}/hijos")]
        public async Task<ActionResult<ApiResponse<long>>> PostHijo(long id, [FromBody] HijoInputDto input)
        {
            try
            {
                input.PersonaId = id;
                var resultado = await _service.AgregarHijo(input);
                return resultado.Success ? Created($"api/th/empleados/{id}/hijos/{resultado.Data}", resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PostHijo {id}");
                return StatusCode(500, ApiResponse<long>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// DELETE /api/th/empleados/{id}/hijos/{hijoId} - Elimina hijo
        /// </summary>
        [HttpDelete("{id}/hijos/{hijoId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteHijo(long id, long hijoId)
        {
            try
            {
                var resultado = await _service.EliminarHijo(hijoId);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en DeleteHijo");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        #endregion

        #region CONTACTOS EMERGENCIA

        /// <summary>
        /// GET /api/th/empleados/{id}/contactos-emergencia - Obtiene contactos de emergencia
        /// </summary>
        [HttpGet("{id}/contactos-emergencia")]
        public async Task<ActionResult<ApiResponse<List<ContactoEmergenciaDto>>>> GetContactosEmergencia(long id)
        {
            try
            {
                var resultado = await _service.ObtenerContactosEmergencia(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetContactosEmergencia {id}");
                return StatusCode(500, ApiResponse<List<ContactoEmergenciaDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// POST /api/th/empleados/{id}/contactos-emergencia - Agrega contacto de emergencia
        /// </summary>
        [HttpPost("{id}/contactos-emergencia")]
        public async Task<ActionResult<ApiResponse<long>>> PostContactoEmergencia(long id, [FromBody] ContactoEmergenciaInputDto input)
        {
            try
            {
                input.PersonaId = id;
                var resultado = await _service.AgregarContactoEmergencia(input);
                return resultado.Success ? Created($"api/th/empleados/{id}/contactos-emergencia/{resultado.Data}", resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PostContactoEmergencia {id}");
                return StatusCode(500, ApiResponse<long>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// DELETE /api/th/empleados/{id}/contactos-emergencia/{contactoId} - Elimina contacto
        /// </summary>
        [HttpDelete("{id}/contactos-emergencia/{contactoId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteContactoEmergencia(long id, long contactoId)
        {
            try
            {
                var resultado = await _service.EliminarContactoEmergencia(contactoId);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en DeleteContactoEmergencia");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        #endregion

        #region PROMOCIONES

        /// <summary>
        /// GET /api/th/empleados/{id}/promociones - Obtiene promociones
        /// </summary>
        [HttpGet("{id}/promociones")]
        public async Task<ActionResult<ApiResponse<List<PromocionDto>>>> GetPromociones(long id)
        {
            try
            {
                var resultado = await _service.ObtenerPromociones(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetPromociones {id}");
                return StatusCode(500, ApiResponse<List<PromocionDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// POST /api/th/empleados/{id}/promociones - Agrega promoción
        /// </summary>
        [HttpPost("{id}/promociones")]
        public async Task<ActionResult<ApiResponse<long>>> PostPromocion(long id, [FromBody] PromocionInputDto input)
        {
            try
            {
                input.PersonaId = id;
                var resultado = await _service.AgregarPromocion(input);
                return resultado.Success ? Created($"api/th/empleados/{id}/promociones/{resultado.Data}", resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PostPromocion {id}");
                return StatusCode(500, ApiResponse<long>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// DELETE /api/th/empleados/{id}/promociones/{promocionId} - Elimina promoción
        /// </summary>
        [HttpDelete("{id}/promociones/{promocionId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeletePromocion(long id, long promocionId)
        {
            try
            {
                var resultado = await _service.EliminarPromocion(promocionId);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en DeletePromocion");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        #endregion

        #region SALARIOS

        /// <summary>
        /// GET /api/th/empleados/{id}/salarios - Obtiene salarios
        /// </summary>
        [HttpGet("{id}/salarios")]
        public async Task<ActionResult<ApiResponse<List<SalarioDto>>>> GetSalarios(long id)
        {
            try
            {
                var resultado = await _service.ObtenerSalarios(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetSalarios {id}");
                return StatusCode(500, ApiResponse<List<SalarioDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// POST /api/th/empleados/{id}/salarios - Agrega salario
        /// </summary>
        [HttpPost("{id}/salarios")]
        public async Task<ActionResult<ApiResponse<long>>> PostSalario(long id, [FromBody] SalarioInputDto input)
        {
            try
            {
                input.PersonaId = id;
                var resultado = await _service.AgregarSalario(input);
                return resultado.Success ? Created($"api/th/empleados/{id}/salarios/{resultado.Data}", resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PostSalario {id}");
                return StatusCode(500, ApiResponse<long>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// DELETE /api/th/empleados/{id}/salarios/{salarioId} - Elimina salario
        /// </summary>
        [HttpDelete("{id}/salarios/{salarioId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteSalario(long id, long salarioId)
        {
            try
            {
                var resultado = await _service.EliminarSalario(salarioId);
                return resultado.Success ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en DeleteSalario");
                return StatusCode(500, ApiResponse<bool>.Error("Error interno del servidor"));
            }
        }

        #endregion
    }
}
