using MatrixNext.Data.Adapters.PY;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY
{
    public class PyDistribucionEntrevistasService : IPyDistribucionEntrevistasService
    {
        private readonly IPyDistribucionEntrevistasAdapter _adapter;
        public PyDistribucionEntrevistasService(IPyDistribucionEntrevistasAdapter adapter) => _adapter = adapter;

        public async Task<List<EntrevistaCualiDto>> ObtenerEntrevistasPendientes(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            return new List<EntrevistaCualiDto>(); // TODO: Implementar en adapter
        }

        public async Task<List<DistribucionEntrevistaDto>> ObtenerDistribucionAsignada(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            return new List<DistribucionEntrevistaDto>(); // TODO: Implementar en adapter
        }

        public async Task<int> GuardarDistribucion(DistribucionEntrevistaInputDto input, string usuario)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.TrabajoId <= 0) throw new ArgumentException("TrabajoId requerido", nameof(input.TrabajoId));
            if (string.IsNullOrWhiteSpace(usuario)) throw new ArgumentException("Usuario requerido", nameof(usuario));

            var id = await _adapter.GuardarDistribucion(input);
            if (id > 0)
            {
                await GuardarLogEntrevista((int)id, "CREACIÓN", "Distribución creada", usuario);
            }
            return (int)id;
        }

        public async Task<bool> ActualizarEstadoDistribucion(int distribucionId, string nuevoEstado, string observaciones)
        {
            if (distribucionId <= 0) throw new ArgumentException("DistribucionId > 0", nameof(distribucionId));
            if (string.IsNullOrWhiteSpace(nuevoEstado)) throw new ArgumentException("NuevoEstado requerido", nameof(nuevoEstado));
            
            short estadoCode = 1; // TODO: mapear estado a código
            await _adapter.ActualizarEstadoDistribucion(distribucionId, estadoCode);
            return true;
        }

        public async Task<List<LogEntrevistaCualiDto>> ObtenerLogDistribucion(int distribucionId)
        {
            if (distribucionId <= 0) throw new ArgumentException("DistribucionId > 0", nameof(distribucionId));
            return await _adapter.ObtenerLogEntrevistas(distribucionId);
        }

        public async Task<int> GuardarLogEntrevista(int distribucionId, string evento, string descripcion, string usuario)
        {
            if (distribucionId <= 0) throw new ArgumentException("DistribucionId > 0", nameof(distribucionId));
            if (string.IsNullOrWhiteSpace(evento)) throw new ArgumentException("Evento requerido", nameof(evento));
            if (string.IsNullOrWhiteSpace(usuario)) throw new ArgumentException("Usuario requerido", nameof(usuario));

            await _adapter.GuardarLogEntrevista(distribucionId, distribucionId, usuario, 1, descripcion ?? "");
            return distribucionId;
        }

        public async Task<List<ModeradorCualiDto>> ObtenerModeradoresDisponibles(DateTime fecha, string zona)
        {
            if (fecha.Date < DateTime.Today) throw new ArgumentException("Fecha no en pasado", nameof(fecha));
            return new List<ModeradorCualiDto>(); // TODO: Implementar en adapter
        }

        public async Task<dynamic> ObtenerAvanceEntrevistas(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            return new { TotalEntrevistas = 0, EntrevistasRealizadas = 0, EntrevistasPendientes = 0, PercentajeCompletacion = 0 };
        }

        public async Task<List<string>> ValidarDistribucionCompleta(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId > 0", nameof(trabajoId));
            return new List<string>(); // TODO: Validaciones
        }
    }
}
