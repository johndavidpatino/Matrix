using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.TH.Models;
using MatrixNext.Data.Services.TH.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Api.Controllers.TH
{
    /// <summary>
    /// Controller para Catálogos de TH
    /// Proporciona acceso a tablas de referencia
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/th/[controller]")]
    public class CatalogosController : ControllerBase
    {
        private readonly IThCatalogosService _service;
        private readonly ILogger<CatalogosController> _logger;

        public CatalogosController(IThCatalogosService service, ILogger<CatalogosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// GET /api/th/catalogos/areas
        /// </summary>
        [HttpGet("areas")]
        public async Task<ActionResult<ApiResponse<List<AreaDto>>>> GetAreas()
        {
            try
            {
                var resultado = await _service.ObtenerAreas();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAreas");
                return StatusCode(500, ApiResponse<List<AreaDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/catalogos/cargos
        /// </summary>
        [HttpGet("cargos")]
        public async Task<ActionResult<ApiResponse<List<CargoDto>>>> GetCargos()
        {
            try
            {
                var resultado = await _service.ObtenerCargos();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetCargos");
                return StatusCode(500, ApiResponse<List<CargoDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/catalogos/bandas
        /// </summary>
        [HttpGet("bandas")]
        public async Task<ActionResult<ApiResponse<List<BandaDto>>>> GetBandas()
        {
            try
            {
                var resultado = await _service.ObtenerBandas();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetBandas");
                return StatusCode(500, ApiResponse<List<BandaDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/catalogos/estados-civiles
        /// </summary>
        [HttpGet("estados-civiles")]
        public async Task<ActionResult<ApiResponse<List<EstadoCivilDto>>>> GetEstadosCiviles()
        {
            try
            {
                var resultado = await _service.ObtenerEstadosCiviles();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetEstadosCiviles");
                return StatusCode(500, ApiResponse<List<EstadoCivilDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/catalogos/grupos-sanguineos
        /// </summary>
        [HttpGet("grupos-sanguineos")]
        public async Task<ActionResult<ApiResponse<List<GrupoSanguineoDto>>>> GetGruposSanguineos()
        {
            try
            {
                var resultado = await _service.ObtenerGruposSanguineos();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetGruposSanguineos");
                return StatusCode(500, ApiResponse<List<GrupoSanguineoDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/catalogos/sedes
        /// </summary>
        [HttpGet("sedes")]
        public async Task<ActionResult<ApiResponse<List<SedeDto>>>> GetSedes()
        {
            try
            {
                var resultado = await _service.ObtenerSedes();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetSedes");
                return StatusCode(500, ApiResponse<List<SedeDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/catalogos/tipos-contrato
        /// </summary>
        [HttpGet("tipos-contrato")]
        public async Task<ActionResult<ApiResponse<List<TipoContratoDto>>>> GetTiposContrato()
        {
            try
            {
                var resultado = await _service.ObtenerTiposContrato();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetTiposContrato");
                return StatusCode(500, ApiResponse<List<TipoContratoDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/catalogos/tiempos-contrato
        /// </summary>
        [HttpGet("tiempos-contrato")]
        public async Task<ActionResult<ApiResponse<List<TiempContratoDto>>>> GetTiemposContrato()
        {
            try
            {
                var resultado = await _service.ObtenerTiemposContrato();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetTiemposContrato");
                return StatusCode(500, ApiResponse<List<TiempContratoDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/catalogos/empresas
        /// </summary>
        [HttpGet("empresas")]
        public async Task<ActionResult<ApiResponse<List<EmpresaDto>>>> GetEmpresas()
        {
            try
            {
                var resultado = await _service.ObtenerEmpresas();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetEmpresas");
                return StatusCode(500, ApiResponse<List<EmpresaDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/catalogos/job-functions
        /// </summary>
        [HttpGet("job-functions")]
        public async Task<ActionResult<ApiResponse<List<JobFunctionDto>>>> GetJobFunctions()
        {
            try
            {
                var resultado = await _service.ObtenerJobFunctions();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetJobFunctions");
                return StatusCode(500, ApiResponse<List<JobFunctionDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/catalogos/parentescos
        /// </summary>
        [HttpGet("parentescos")]
        public async Task<ActionResult<ApiResponse<List<ParentescoDto>>>> GetParentescos()
        {
            try
            {
                var resultado = await _service.ObtenerParentescos();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetParentescos");
                return StatusCode(500, ApiResponse<List<ParentescoDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/catalogos/motivos-cambio-salario
        /// </summary>
        [HttpGet("motivos-cambio-salario")]
        public async Task<ActionResult<ApiResponse<List<MotivoCambioSalarioDto>>>> GetMotivosCambioSalario()
        {
            try
            {
                var resultado = await _service.ObtenerMotivosCambioSalario();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetMotivosCambioSalario");
                return StatusCode(500, ApiResponse<List<MotivoCambioSalarioDto>>.Error("Error interno del servidor"));
            }
        }

        /// <summary>
        /// GET /api/th/catalogos/tipos-salario
        /// </summary>
        [HttpGet("tipos-salario")]
        public async Task<ActionResult<ApiResponse<List<TipoSalarioDto>>>> GetTiposSalario()
        {
            try
            {
                var resultado = await _service.ObtenerTiposSalario();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetTiposSalario");
                return StatusCode(500, ApiResponse<List<TipoSalarioDto>>.Error("Error interno del servidor"));
            }
        }
    }
}
