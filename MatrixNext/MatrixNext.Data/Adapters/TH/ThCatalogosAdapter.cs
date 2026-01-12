using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Adapters.TH.Models;
using MatrixNext.Data.Context;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Adapters.TH
{
    /// <summary>
    /// Adaptador para Catálogos de TH
    /// Proporciona acceso a tablas de referencia (Areas, Cargos, Bandas, etc.)
    /// </summary>
    public class ThCatalogosAdapter : IThCatalogosAdapter
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThCatalogosAdapter> _logger;

        public ThCatalogosAdapter(ApplicationDbContext context, ILogger<ThCatalogosAdapter> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<AreaDto>> ObtenerAreas()
        {
            return await ObtenerCatalogo<AreaDto>("Areas", "Nombre");
        }

        public async Task<List<CargoDto>> ObtenerCargos()
        {
            return await ObtenerCatalogo<CargoDto>("Cargos", "Nombre");
        }

        public async Task<List<BandaDto>> ObtenerBandas()
        {
            return await ObtenerCatalogo<BandaDto>("Bandas", "Nombre");
        }

        public async Task<List<EstadoCivilDto>> ObtenerEstadosCiviles()
        {
            return await ObtenerCatalogo<EstadoCivilDto>("EstadosCiviles", "Nombre");
        }

        public async Task<List<GrupoSanguineoDto>> ObtenerGruposSanguineos()
        {
            return await ObtenerCatalogo<GrupoSanguineoDto>("GruposSanguineos", "Nombre");
        }

        public async Task<List<SedeDto>> ObtenerSedes()
        {
            return await ObtenerCatalogo<SedeDto>("Sedes", "Nombre");
        }

        public async Task<List<TipoContratoDto>> ObtenerTiposContrato()
        {
            return await ObtenerCatalogo<TipoContratoDto>("TiposContrato", "Nombre");
        }

        public async Task<List<TiempContratoDto>> ObtenerTiemposContrato()
        {
            return await ObtenerCatalogo<TiempContratoDto>("TiemposContrato", "Nombre");
        }

        public async Task<List<EmpresaDto>> ObtenerEmpresas()
        {
            return await ObtenerCatalogo<EmpresaDto>("Empresas", "Nombre");
        }

        public async Task<List<JobFunctionDto>> ObtenerJobFunctions()
        {
            return await ObtenerCatalogo<JobFunctionDto>("JobFunctions", "Nombre");
        }

        public async Task<List<ParentescoDto>> ObtenerParentescos()
        {
            return await ObtenerCatalogo<ParentescoDto>("Parentescos", "Nombre");
        }

        public async Task<List<MotivoCambioSalarioDto>> ObtenerMotivosCambioSalario()
        {
            return await ObtenerCatalogo<MotivoCambioSalarioDto>("MotivosCambioSalario", "Nombre");
        }

        public async Task<List<TipoSalarioDto>> ObtenerTiposSalario()
        {
            return await ObtenerCatalogo<TipoSalarioDto>("TiposSalario", "Nombre");
        }

        private async Task<List<T>> ObtenerCatalogo<T>(string tableName, string nombreCampo) where T : new()
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().QueryAsync<T>(
                    $"SELECT * FROM {tableName} ORDER BY {nombreCampo}"
                );

                return resultado.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener catálogo {tableName}");
                throw;
            }
        }
    }
}
