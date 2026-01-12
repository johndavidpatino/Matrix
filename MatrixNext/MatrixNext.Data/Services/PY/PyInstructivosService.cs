using MatrixNext.Data.Adapters.PY;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY
{
    public class PyInstructivosService : IPyInstructivosService
    {
        private readonly IPyInstructivosAdapter _adapter;
        public PyInstructivosService(IPyInstructivosAdapter adapter) => _adapter = adapter;

        public async Task<EspecificacionTecnicaDto> ObtenerEspecificacionCuanti(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            return await _adapter.ObtenerEspecificacion(trabajoId);
        }

        public async Task<EspecificacionTecnicaCualiDto> ObtenerEspecificacionCuali(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            return await _adapter.ObtenerEspecificacionCuali(trabajoId);
        }

        public async Task<int> GuardarEspecificacionCuanti(EspecificacionTecnicaInputDto input, string usuario)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var id = await _adapter.GuardarEspecificacion(input);
            if (id > 0) await NotificarCambiosEspecificacion((int)input.TrabajoId, "Cuantitativo", usuario);
            return (int)id;
        }

        public async Task<int> GuardarEspecificacionCuali(EspecificacionTecnicaCualiInputDto input, string usuario)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            // TODO: Agregar método GuardarEspecificacionCuali al adapter
            return 0;
        }

        public async Task<List<AyudaCualiDto>> ObtenerAyudasCuali(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            return new List<AyudaCualiDto>();
        }

        public async Task<int> GuardarAyudaCuali(AyudaCualiInputDto input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            return 0;
        }

        public async Task<List<TipoReclutamientoCualiDto>> ObtenerTiposReclutamientoCuali(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            return new List<TipoReclutamientoCualiDto>();
        }

        public async Task<int> GuardarTipoReclutamientoCuali(TipoReclutamientoCualiInputDto input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            return 0;
        }

        public async Task<List<dynamic>> ObtenerHistorialVersiones(int trabajoId, string tipoEspecificacion)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            return new List<dynamic>();
        }

        public async Task<bool> NotificarCambiosEspecificacion(int trabajoId, string tipoEspecificacion, string usuario)
        {
            return await Task.FromResult(true);
        }
    }
}
