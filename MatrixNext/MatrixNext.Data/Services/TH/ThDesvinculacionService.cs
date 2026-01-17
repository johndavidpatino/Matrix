using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.TH;
using MatrixNext.Data.Adapters.TH.Models;
using MatrixNext.Data.Services.TH.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.TH
{
    /// <summary>
    /// Servicio para gestiÃ³n de Desvinculaciones
    /// Orquesta el flujo de desvinculaciÃ³n con evaluaciones
    /// </summary>
    public class ThDesvinculacionService : IThDesvinculacionService
    {
        private readonly IThDesvinculacionAdapter _desvinculacionAdapter;
        private readonly ILogger<ThDesvinculacionService> _logger;

        public ThDesvinculacionService(
            IThDesvinculacionAdapter desvinculacionAdapter,
            ILogger<ThDesvinculacionService> logger)
        {
            _desvinculacionAdapter = desvinculacionAdapter;
            _logger = logger;
        }

        public async Task<ApiResponse<List<DesvinculacionDto>>> ObtenerDesvinculaciones(int pageSize, int pageIndex, string? textoBuscado)
        {
            try
            {
                var desvinculaciones = await _desvinculacionAdapter.ObtenerDesvinculaciones(pageSize, pageIndex, textoBuscado);
                return ApiResponse<List<DesvinculacionDto>>.Ok(desvinculaciones, "Desvinculaciones obtenidas correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener desvinculaciones");
                return ApiResponse<List<DesvinculacionDto>>.Error("Error al obtener desvinculaciones. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<long>> IniciarProcesoDesvinculacion(DesvinculacionInputDto input)
        {
            try
            {
                if (input.FechaRetiro == default(DateTime))
                    return ApiResponse<long>.Error("Fecha de retiro es requerida");

                var newId = await _desvinculacionAdapter.IniciarProcesoDesvinculacion(input);
                return ApiResponse<long>.Ok(newId, "Proceso de desvinculaciÃ³n iniciado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar desvinculaciÃ³n");
                return ApiResponse<long>.Error("Error al procesar la solicitud. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<List<dynamic>>> ObtenerEvaluacionesDesvinculacion(long desvinculacionId)
        {
            try
            {
                var evaluaciones = await _desvinculacionAdapter.ObtenerEvaluacionesDesvinculacion(desvinculacionId);
                return ApiResponse<List<dynamic>>.Ok(evaluaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener evaluaciones");
                return ApiResponse<List<dynamic>>.Error("Error al obtener evaluaciones. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<bool>> GuardarEvaluacionDesvinculacion(DesvinculacionEvaluacionInputDto input, string? usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario))
                    return ApiResponse<bool>.Error("Usuario es requerido");

                var result = await _desvinculacionAdapter.GuardarEvaluacionDesvinculacion(input, usuario);
                if (!result)
                    return ApiResponse<bool>.Error("No se pudo guardar la evaluaciÃ³n");

                return ApiResponse<bool>.Ok(true, "EvaluaciÃ³n guardada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar evaluaciÃ³n");
                return ApiResponse<bool>.Error("Error al procesar la solicitud. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<bool>> FinalizarProcesoDesvinculacion(long desvinculacionId)
        {
            try
            {
                var result = await _desvinculacionAdapter.FinalizarProcesoDesvinculacion(desvinculacionId);
                if (!result)
                    return ApiResponse<bool>.Error("No se pudo finalizar el proceso");

                return ApiResponse<bool>.Ok(true, "Proceso finalizado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al finalizar desvinculaciÃ³n");
                return ApiResponse<bool>.Error("Error al procesar la solicitud. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<string>> GenerarPDFDesvinculacion(long desvinculacionId)
        {
            try
            {
                var pdfBase64 = await _desvinculacionAdapter.GenerarPDFDesvinculacion(desvinculacionId);
                if (string.IsNullOrEmpty(pdfBase64))
                    return ApiResponse<string>.Error("No se pudo generar el PDF");

                return ApiResponse<string>.Ok(pdfBase64, "PDF generado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar PDF");
                return ApiResponse<string>.Error("Error al procesar la solicitud. Por favor intente nuevamente.");
            }
        }
    }
}

