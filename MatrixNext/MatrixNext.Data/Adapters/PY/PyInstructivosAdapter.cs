using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Entities;

namespace MatrixNext.Data.Adapters.PY
{
    /// <summary>
    /// Adapter para Instructivos Técnicos (Cuantitativos y Cualitativos)
    /// TODO: Implementar completamente cuando entidades PY estén registradas en MatrixDbContext
    /// </summary>
    public class PyInstructivosAdapter : IPyInstructivosAdapter
    {
        private readonly MatrixDbContext _context;

        public PyInstructivosAdapter(MatrixDbContext context)
        {
            _context = context;
        }

        // Implementación temporal - retorna valores por defecto hasta registrar entidades

        public async Task<EspecificacionTecnicaDto?> ObtenerEspecificacion(long trabajoId)
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<EspecificacionTecnicaDto?> ObtenerEspecificacionUltimaVersion(long trabajoId)
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<List<EspecificacionTecnicaDto>> ObtenerEspecificacionesLista(long trabajoId)
        {
            await Task.CompletedTask;
            return new List<EspecificacionTecnicaDto>();
        }

        public async Task<int> ContarVersionesEspecificacion(long trabajoId)
        {
            await Task.CompletedTask;
            return 0;
        }

        public async Task<long> GuardarEspecificacion(EspecificacionTecnicaInputDto input)
        {
            await Task.CompletedTask;
            return input.Id ?? 0;
        }

        public async Task<EspecificacionTecnicaCualiDto?> ObtenerEspecificacionCuali(long trabajoId)
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<EspecificacionTecnicaCualiDto?> ObtenerEspecificacionCualiUltimaVersion(long trabajoId)
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<List<EspecificacionTecnicaCualiDto>> ObtenerEspecificacionesCualiLista(long trabajoId)
        {
            await Task.CompletedTask;
            return new List<EspecificacionTecnicaCualiDto>();
        }

        public async Task<int> ContarVersionesEspecificacionCuali(long trabajoId)
        {
            await Task.CompletedTask;
            return 0;
        }

        public async Task<long> GuardarEspecificacionCuali(EspecificacionTecnicaCualiInputDto input)
        {
            await Task.CompletedTask;
            return input.Id ?? 0;
        }

        public async Task<List<AyudaCualiDto>> ObtenerAyudasCuali()
        {
            await Task.CompletedTask;
            return new List<AyudaCualiDto>();
        }

        public async Task<List<TipoReclutamientoCualiDto>> ObtenerTiposReclutamientoCuali()
        {
            await Task.CompletedTask;
            return new List<TipoReclutamientoCualiDto>();
        }

        public async Task<List<int>> ObtenerAyudasRequeridasPorTrabajo(long trabajoId)
        {
            await Task.CompletedTask;
            return new List<int>();
        }

        public async Task<List<int>> ObtenerReclutamientoRequeridoPorTrabajo(long trabajoId)
        {
            await Task.CompletedTask;
            return new List<int>();
        }

        public async Task GuardarAyudasRequeridas(long trabajoId, List<int> ayudasSeleccionadas)
        {
            await Task.CompletedTask;
        }

        public async Task GuardarReclutamientoRequerido(long trabajoId, List<int> tiposSeleccionados)
        {
            await Task.CompletedTask;
        }
    }
}
