using MatrixNext.Data.Modules.CC.Adapters;
using MatrixNext.Data.Modules.CC.DTOs;

namespace MatrixNext.Data.Modules.CC.Services
{
    /// <summary>
    /// CC FinzOpe Service - Business logic for payroll and bonification operations
    /// </summary>
    public interface ICcFinzOpeService
    {
        Task<CcLiquidacionDto> CalcularLiquidacionMensual(int idPeriodo, DateTime fechaInicio, DateTime fechaFin);
        Task<List<CcBonificacionDto>> ObtenerBonificaciones(int idPeriodo);
        Task<decimal> ObtenerProduccionTotal(DateTime fechaInicio, DateTime fechaFin, int? idTrabajo = null);
    }

    public class CcFinzOpeService : ICcFinzOpeService
    {
        private readonly ICcFinzOpeAdapter _adapter;

        public CcFinzOpeService(ICcFinzOpeAdapter adapter)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        }

        public async Task<CcLiquidacionDto> CalcularLiquidacionMensual(int idPeriodo, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _adapter.ObtenerLiquidacion(idPeriodo, fechaInicio, fechaFin);
        }

        public async Task<List<CcBonificacionDto>> ObtenerBonificaciones(int idPeriodo)
        {
            return await _adapter.ObtenerBonificaciones(idPeriodo);
        }

        public async Task<decimal> ObtenerProduccionTotal(DateTime fechaInicio, DateTime fechaFin, int? idTrabajo = null)
        {
            return await _adapter.ObtenerProduccionTotal(fechaInicio, fechaFin, idTrabajo);
        }
    }
}
