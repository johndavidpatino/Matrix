using MatrixNext.Data.Adapters.PY;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY
{
    public class PyTrabajosService : IPyTrabajosService
    {
        private readonly IPyTrabajosAdapter _adapter;
        public PyTrabajosService(IPyTrabajosAdapter adapter) => _adapter = adapter;

        public async Task<DuplicarTrabajoResultDto> DuplicarTrabajoCompleto(DuplicarTrabajoInputDto input, string usuario)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.TrabajoIdOrigen <= 0) throw new ArgumentException("TrabajoIdOrigen requerido", nameof(input.TrabajoIdOrigen));
            if (string.IsNullOrWhiteSpace(input.NombreNuevo)) throw new ArgumentException("NombreNuevo requerido", nameof(input.NombreNuevo));
            if (input.ProyectoIdNuevo <= 0) throw new ArgumentException("ProyectoIdNuevo requerido", nameof(input.ProyectoIdNuevo));

            return await _adapter.DuplicarTrabajoCompleto(input);
        }

        public async Task<TrabajoConfiguracionDto> ObtenerConfiguracionTrabajo(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            return new TrabajoConfiguracionDto(); // TODO: Implementar en adapter
        }

        public async Task<bool> GuardarConfiguracionTrabajo(TrabajoConfiguracionInputDto input, string usuario)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.TrabajoId <= 0) throw new ArgumentException("TrabajoId requerido", nameof(input.TrabajoId));
            if (string.IsNullOrWhiteSpace(usuario)) throw new ArgumentException("Usuario requerido", nameof(usuario));
            return true; // TODO: Implementar en adapter
        }

        public async Task<bool> ValidarTrabajoListo(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            var config = await ObtenerConfiguracionTrabajo(trabajoId);
            return config != null;
        }

        public async Task<dynamic> ObtenerEstadoTrabajo(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            return new { EstadoGeneral = "Sin Iniciar", EspecificacionesCompletadas = false, MuestrasValidadas = false, AvanceEjecucion = 0 };
        }

        public async Task<bool> CerrarTrabajo(int trabajoId, string motivo, string usuario)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            if (string.IsNullOrWhiteSpace(motivo)) throw new ArgumentException("Motivo requerido", nameof(motivo));
            if (string.IsNullOrWhiteSpace(usuario)) throw new ArgumentException("Usuario requerido", nameof(usuario));
            return true; // TODO: Implementar en adapter
        }
    }
}
