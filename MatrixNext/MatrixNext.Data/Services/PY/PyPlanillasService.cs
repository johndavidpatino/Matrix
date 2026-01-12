using MatrixNext.Data.Adapters.PY;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY
{
    public class PyPlanillasService : IPyPlanillasService
    {
        private readonly IPyPlanillasAdapter _adapter;
        public PyPlanillasService(IPyPlanillasAdapter adapter) => _adapter = adapter;

        public async Task<List<TecnicaDto>> ObtenerTecnicas(string tipoTecnica)
        {
            if (string.IsNullOrWhiteSpace(tipoTecnica)) throw new ArgumentException("TipoTecnica es requerido", nameof(tipoTecnica));
            return await _adapter.ObtenerTecnicas(tipoTecnica);
        }

        public async Task<List<ModeradorDto>> ObtenerModeradoresDisponibles(DateTime fecha)
        {
            if (fecha.Date < DateTime.Today) throw new ArgumentException("Fecha no puede ser en el pasado", nameof(fecha));
            return new List<ModeradorDto>(); // TODO: Implementar en adapter
        }

        public async Task<int> CrearPlanillaModeracion(PlanillaModeracionInputDto input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var id = await _adapter.CrearPlanillaModeracion(input);
            return (int)id;
        }

        public async Task<bool> ActualizarPlanillaModeracion(PlanillaModeracionActualizacionDto input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.IdPlanilla <= 0) throw new ArgumentException("IdPlanilla es requerido", nameof(input.IdPlanilla));
            return true; // TODO: Implementar actualización
        }

        public async Task<List<PlanillaInformesDto>> ObtenerPlanillasInformes(DateTime fechaInicio, DateTime fechaFinal)
        {
            if (fechaFinal < fechaInicio) throw new ArgumentException("FechaFinal >= FechaInicio", nameof(fechaFinal));
            return new List<PlanillaInformesDto>(); // TODO: Implementar en adapter
        }

        public async Task<bool> ActualizarEstadoPlanillaInformes(int idPlanilla, string nuevoEstado)
        {
            if (idPlanilla <= 0) throw new ArgumentException("IdPlanilla > 0", nameof(idPlanilla));
            if (string.IsNullOrWhiteSpace(nuevoEstado)) throw new ArgumentException("NuevoEstado requerido", nameof(nuevoEstado));
            return true; // TODO: Implementar en adapter
        }

        public async Task<List<PlanillaListDto>> ObtenerPlanillasParaExportar(DateTime fechaInicio, DateTime fechaFinal)
        {
            if (fechaFinal < fechaInicio) throw new ArgumentException("FechaFinal >= FechaInicio", nameof(fechaFinal));
            var moderacion = await _adapter.ObtenerPlanillasModeracionParaExportar(fechaInicio, fechaFinal);
            // TODO: Mapear a PlanillaListDto
            return new List<PlanillaListDto>();
        }

        public async Task<bool> MarcarExportada(int idPlanilla)
        {
            if (idPlanilla <= 0) throw new ArgumentException("IdPlanilla > 0", nameof(idPlanilla));
            return true; // TODO: Implementar en adapter
        }

        public async Task<List<string>> ValidarPlanillaModeracion(int idPlanilla)
        {
            if (idPlanilla <= 0) throw new ArgumentException("IdPlanilla > 0", nameof(idPlanilla));
            return new List<string>(); // TODO: Validaciones
        }

        public async Task<dynamic> ObtenerEstadisticasPlanillas(DateTime fechaInicio, DateTime fechaFinal)
        {
            if (fechaFinal < fechaInicio) throw new ArgumentException("FechaFinal >= FechaInicio", nameof(fechaFinal));
            return new { TotalPlanillas = 0, PlanillasCompletadas = 0, ModeradoresActivos = 0, TecnicasUtilizadas = 0 };
        }
    }
}
