using MatrixNext.Data.Adapters.PY;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY
{
    /// <summary>
    /// Servicio de negocio para gestión de visitas InHome.
    /// Delega operaciones a PyInHomeVisitAdapter con validaciones mínimas.
    /// </summary>
    public class PyInHomeVisitService : IPyInHomeVisitService
    {
        private readonly IPyInHomeVisitAdapter _adapter;

        public PyInHomeVisitService(IPyInHomeVisitAdapter adapter)
        {
            _adapter = adapter;
        }

        public async Task<List<InHomeVisitDto>> ObtenerInHomesPorTrabajo(int trabajoId)
        {
            if (trabajoId <= 0)
                throw new ArgumentException("ID de trabajo inválido", nameof(trabajoId));

            return await _adapter.ObtenerInHomesPorTrabajo(trabajoId);
        }

        public async Task<List<LogInHomeDto>> ObtenerLogInHome(int idInHome)
        {
            if (idInHome <= 0)
                throw new ArgumentException("ID de InHome inválido", nameof(idInHome));

            return await _adapter.ObtenerLogInHome(idInHome);
        }

        public async Task<int> GuardarInHome(InHomeVisitInputDto input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (input.TrabajoId <= 0)
                throw new ArgumentException("ID de trabajo inválido", nameof(input.TrabajoId));

            var resultado = await _adapter.GuardarInHome(input);
            return (int)resultado;
        }

        public async Task<bool> ActualizarInHome(InHomeVisitInputDto input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (input.Id <= 0)
                throw new ArgumentException("ID de InHome inválido", nameof(input.Id));

            // Usar GuardarInHome para actualización (el adapter maneja Insert/Update basado en ID)
            var resultado = await _adapter.GuardarInHome(input);
            return resultado > 0;
        }

        public async Task<int> GuardarLogInHome(int idInHome, string descripcion, string usuario)
        {
            if (idInHome <= 0)
                throw new ArgumentException("ID de InHome inválido", nameof(idInHome));
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("Descripción requerida", nameof(descripcion));

            // Obtener el InHome para extraer trabajoId
            var inHome = await _adapter.ObtenerInHomePorId(idInHome);
            if (inHome == null)
                throw new InvalidOperationException($"InHome {idInHome} no encontrado");

            await _adapter.GuardarLogInHome(idInHome, inHome.TrabajoId, usuario, "ACTUALIZACIÓN", descripcion);
            return idInHome; // Retornar el ID ya que el adapter no retorna nada
        }
    }
}
