using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.OP.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Servicio para la revisión de productividad multirrol en OP.
    /// Implementa lógica centralizada para revisión por PMO, Coordinador, Campo y MyS/Call.
    /// </summary>
    public class OpRevisionProductividadService : IOpRevisionProductividadService
    {
        private readonly MatrixDbContext _context;
        private readonly ILogger<OpRevisionProductividadService> _logger;
        private const string SP_OP_CuantiDapper = "OP_CuantiDapper_Get";

        public OpRevisionProductividadService(
            MatrixDbContext context,
            ILogger<OpRevisionProductividadService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<PlanillaProductividadDto>> ObtenerPlanillasPorRolAsync(int trabajoId, string rol)
        {
            try
            {
                var planillas = new List<PlanillaProductividadDto>();

                // TODO: Implementar llamada a SP "OP_CuantiDapper_Get" con rol como parámetro
                // La lógica varía según el rol:
                // - PMO: Revisa todas las planillas de producción (TipoActividad 1-20)
                // - Coordinador: Revisa planillas de su zona (TipoActividad 1-15)
                // - Campo: Revisa planillas de su ciudad (TipoActividad 1-10)
                // - MyS/Call: Revisa planillas de CATI/CAWI (TipoActividad 21-23)

                _logger.LogInformation("Obtenidas planillas para trabajo {TrabajoId} con rol {Rol}", trabajoId, rol);
                return planillas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo planillas para trabajo {TrabajoId}", trabajoId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> AprobarPlanillaAsync(int planillaId, decimal montoAutorizado, int usuarioId)
        {
            try
            {
                // TODO: Implementar actualización de planilla en BD
                // - Cambiar estado a "Aprobada" (2)
                // - Registrar monto autorizado
                // - Registrar usuario y fecha de aprobación
                // - Llamar SP OP_PlanillaProductividad_Aprobar

                _logger.LogInformation("Planilla {PlanillaId} aprobada por usuario {UsuarioId} con monto {Monto}", 
                    planillaId, usuarioId, montoAutorizado);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aprobando planilla {PlanillaId}", planillaId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> RechazarPlanillaAsync(int planillaId, string observacion, int usuarioId)
        {
            try
            {
                // TODO: Implementar rechazo de planilla en BD
                // - Cambiar estado a "Rechazada" (3)
                // - Registrar observación del rechazo
                // - Registrar usuario y fecha de rechazo
                // - Llamar SP OP_PlanillaProductividad_Rechazar

                _logger.LogWarning("Planilla {PlanillaId} rechazada por usuario {UsuarioId}. Observación: {Obs}", 
                    planillaId, usuarioId, observacion);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rechazando planilla {PlanillaId}", planillaId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<(bool Valid, string Message)> ValidarMontosPlanillaAsync(int trabajoId, decimal montoTotal)
        {
            try
            {
                // TODO: Obtener máximo autorizado del trabajo desde TrabajoOPCuanti
                // - Consultar tabla: TrabajoOPCuanti.CCProduccionPST (presupuesto)
                // - Validar que montoTotal no exceda presupuesto

                if (montoTotal < 0)
                {
                    return (false, "El monto no puede ser negativo");
                }

                // Placeholder: En implementación real, consultar presupuesto
                return (true, "Validación exitosa");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando montos para trabajo {TrabajoId}", trabajoId);
                throw;
            }
        }
    }
}
