using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.TH;
using MatrixNext.Data.Adapters.TH.Models;
using MatrixNext.Data.Services.TH.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.TH
{
    public class ThCatalogosService : IThCatalogosService
    {
        private readonly IThCatalogosAdapter _adapter;
        private readonly ILogger<ThCatalogosService> _logger;

        public ThCatalogosService(IThCatalogosAdapter adapter, ILogger<ThCatalogosService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<ApiResponse<List<AreaDto>>> ObtenerAreas()
        {
            try
            {
                var data = await _adapter.ObtenerAreas();
                return ApiResponse<List<AreaDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener áreas");
                return ApiResponse<List<AreaDto>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CargoDto>>> ObtenerCargos()
        {
            try
            {
                var data = await _adapter.ObtenerCargos();
                return ApiResponse<List<CargoDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener cargos");
                return ApiResponse<List<CargoDto>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<List<BandaDto>>> ObtenerBandas()
        {
            try
            {
                var data = await _adapter.ObtenerBandas();
                return ApiResponse<List<BandaDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener bandas");
                return ApiResponse<List<BandaDto>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<List<EstadoCivilDto>>> ObtenerEstadosCiviles()
        {
            try
            {
                var data = await _adapter.ObtenerEstadosCiviles();
                return ApiResponse<List<EstadoCivilDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener estados civiles");
                return ApiResponse<List<EstadoCivilDto>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<List<GrupoSanguineoDto>>> ObtenerGruposSanguineos()
        {
            try
            {
                var data = await _adapter.ObtenerGruposSanguineos();
                return ApiResponse<List<GrupoSanguineoDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener grupos");
                return ApiResponse<List<GrupoSanguineoDto>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<List<SedeDto>>> ObtenerSedes()
        {
            try
            {
                var data = await _adapter.ObtenerSedes();
                return ApiResponse<List<SedeDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener sedes");
                return ApiResponse<List<SedeDto>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<List<TipoContratoDto>>> ObtenerTiposContrato()
        {
            try
            {
                var data = await _adapter.ObtenerTiposContrato();
                return ApiResponse<List<TipoContratoDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener tipos contrato");
                return ApiResponse<List<TipoContratoDto>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<List<TiempContratoDto>>> ObtenerTiemposContrato()
        {
            try
            {
                var data = await _adapter.ObtenerTiemposContrato();
                return ApiResponse<List<TiempContratoDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener tiempos contrato");
                return ApiResponse<List<TiempContratoDto>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<List<EmpresaDto>>> ObtenerEmpresas()
        {
            try
            {
                var data = await _adapter.ObtenerEmpresas();
                return ApiResponse<List<EmpresaDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener empresas");
                return ApiResponse<List<EmpresaDto>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<List<JobFunctionDto>>> ObtenerJobFunctions()
        {
            try
            {
                var data = await _adapter.ObtenerJobFunctions();
                return ApiResponse<List<JobFunctionDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener job functions");
                return ApiResponse<List<JobFunctionDto>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<List<ParentescoDto>>> ObtenerParentescos()
        {
            try
            {
                var data = await _adapter.ObtenerParentescos();
                return ApiResponse<List<ParentescoDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener parentescos");
                return ApiResponse<List<ParentescoDto>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<List<MotivoCambioSalarioDto>>> ObtenerMotivosCambioSalario()
        {
            try
            {
                var data = await _adapter.ObtenerMotivosCambioSalario();
                return ApiResponse<List<MotivoCambioSalarioDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener motivos");
                return ApiResponse<List<MotivoCambioSalarioDto>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<List<TipoSalarioDto>>> ObtenerTiposSalario()
        {
            try
            {
                var data = await _adapter.ObtenerTiposSalario();
                return ApiResponse<List<TipoSalarioDto>>.Success(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtener tipos salario");
                return ApiResponse<List<TipoSalarioDto>>.Error(ex.Message);
            }
        }
    }
}
