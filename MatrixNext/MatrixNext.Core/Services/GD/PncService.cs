using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatrixNext.Core.DTOs.GD;
using MatrixNext.Infrastructure.Adapters.GD;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Core.Services.GD
{
    /// <summary>
    /// Interfaz para servicio de PNC
    /// </summary>
    public interface IPncService
    {
        Task<IEnumerable<PncDto>> ObtenerPncAsync(
            long? idPnc = null,
            long? idUsuario = null,
            byte? idEstado = null,
            long? idUsuarioRegistra = null);

        Task<PncDto> ObtenerPncDetalleAsync(long idPnc);

        Task<(bool exitoso, string mensaje, long idPnc)> CrearPncAsync(PncDto pnc, long usuarioRegistra);

        Task<(bool exitoso, string mensaje)> ActualizarPncAsync(PncDto pnc, long usuarioModifica);

        Task<IEnumerable<PncCausaDto>> ObtenerCausasAsync(long idPnc);

        Task<(bool exitoso, string mensaje, long idCausa)> AgregarCausaAsync(PncCausaDto causa, long usuarioRegistra);

        Task<IEnumerable<PncSeguimientoDto>> ObtenerSeguimientoAsync(long idPnc);

        Task<IEnumerable<PncLogDto>> ObtenerLogAsync(long idPnc);

        Task<PncResumenDto> ObtenerResumenAsync();
    }

    /// <summary>
    /// Servicio de negocio para PNC (Productos No Conformes)
    /// Ref: WebMatrix - PNCClass.vb
    /// </summary>
    public class PncService : IPncService
    {
        private readonly IPncAdapter _adapter;
        private readonly ILogger<PncService> _logger;

        public PncService(IPncAdapter adapter, ILogger<PncService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<PncDto>> ObtenerPncAsync(
            long? idPnc = null,
            long? idUsuario = null,
            byte? idEstado = null,
            long? idUsuarioRegistra = null)
        {
            try
            {
                _logger.LogInformation(
                    "Obteniendo PNC con filtros: IdPnc={IdPnc}, IdUsuario={IdUsuario}, IdEstado={IdEstado}",
                    idPnc, idUsuario, idEstado);

                var pncs = await _adapter.ObtenerPncAsync(idPnc, idUsuario, idEstado, idUsuarioRegistra);
                return pncs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo listado de PNC");
                return Enumerable.Empty<PncDto>();
            }
        }

        public async Task<PncDto> ObtenerPncDetalleAsync(long idPnc)
        {
            try
            {
                if (idPnc <= 0)
                {
                    _logger.LogWarning("ID de PNC inválido: {IdPnc}", idPnc);
                    return null;
                }

                _logger.LogInformation("Obteniendo detalle de PNC: {IdPnc}", idPnc);
                return await _adapter.ObtenerPncAsync(idPnc);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo detalle de PNC {IdPnc}", idPnc);
                return null;
            }
        }

        public async Task<(bool exitoso, string mensaje, long idPnc)> CrearPncAsync(
            PncDto pnc, long usuarioRegistra)
        {
            try
            {
                // Validaciones
                if (pnc == null)
                {
                    _logger.LogWarning("Intento de crear PNC con DTO nulo");
                    return (false, "El PNC no puede estar vacío", 0);
                }

                if (!pnc.AsociadoA.HasValue || pnc.AsociadoA <= 0)
                {
                    _logger.LogWarning("PNC sin 'Asociado A' especificado");
                    return (false, "El tipo de asociación es obligatorio", 0);
                }

                if (!pnc.IdReferencia.HasValue || pnc.IdReferencia <= 0)
                {
                    _logger.LogWarning("PNC sin ID de referencia");
                    return (false, "La referencia es obligatoria", 0);
                }

                if (!pnc.IdProceso.HasValue || pnc.IdProceso <= 0)
                {
                    _logger.LogWarning("PNC sin proceso especificado");
                    return (false, "El proceso es obligatorio", 0);
                }

                if (string.IsNullOrWhiteSpace(pnc.Descripcion))
                {
                    _logger.LogWarning("PNC sin descripción");
                    return (false, "La descripción es obligatoria", 0);
                }

                if (usuarioRegistra <= 0)
                {
                    _logger.LogWarning("Usuario de registro inválido: {UsuarioId}", usuarioRegistra);
                    return (false, "Usuario inválido", 0);
                }

                // Crear
                _logger.LogInformation(
                    "Creando PNC: AsociadoA={AsociadoA}, IdReferencia={IdReferencia}, IdProceso={IdProceso}",
                    pnc.AsociadoA, pnc.IdReferencia, pnc.IdProceso);

                var idPnc = await _adapter.CrearPncAsync(pnc, usuarioRegistra);

                _logger.LogInformation("PNC {IdPnc} creado exitosamente por usuario {UsuarioId}",
                    idPnc, usuarioRegistra);

                return (true, "PNC registrado exitosamente", idPnc);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando PNC. Usuario: {UsuarioId}, Descripción: {Descripcion}",
                    usuarioRegistra, pnc?.Descripcion);
                return (false, "Error al registrar el PNC. Por favor intente nuevamente", 0);
            }
        }

        public async Task<(bool exitoso, string mensaje)> ActualizarPncAsync(
            PncDto pnc, long usuarioModifica)
        {
            try
            {
                if (pnc == null || pnc.IdPnc <= 0)
                {
                    _logger.LogWarning("Intento de actualizar PNC con ID inválido");
                    return (false, "El ID del PNC es inválido");
                }

                // Verificar que existe
                var pncExistente = await _adapter.ObtenerPncAsync(pnc.IdPnc);
                if (pncExistente == null)
                {
                    _logger.LogWarning("Intento de actualizar PNC inexistente: {IdPnc}", pnc.IdPnc);
                    return (false, "El PNC no existe");
                }

                // Validar que pueda editarse (solo si está en estado Registrado)
                if (pncExistente.IdEstado != 1)
                {
                    _logger.LogWarning("Intento de actualizar PNC {IdPnc} en estado {IdEstado}",
                        pnc.IdPnc, pncExistente.IdEstado);
                    return (false, $"No se puede editar un PNC en estado '{pncExistente.NombreEstado}'");
                }

                _logger.LogInformation("Actualizando PNC: {IdPnc} por usuario {UsuarioId}",
                    pnc.IdPnc, usuarioModifica);

                var resultado = await _adapter.ActualizarPncAsync(pnc, usuarioModifica);

                if (resultado)
                {
                    _logger.LogInformation("PNC {IdPnc} actualizado exitosamente", pnc.IdPnc);
                    return (true, "PNC actualizado exitosamente");
                }

                return (false, "No se pudo actualizar el PNC");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando PNC {IdPnc}", pnc?.IdPnc);
                return (false, "Error al actualizar el PNC. Por favor intente nuevamente");
            }
        }

        public async Task<IEnumerable<PncCausaDto>> ObtenerCausasAsync(long idPnc)
        {
            try
            {
                if (idPnc <= 0)
                {
                    _logger.LogWarning("ID de PNC inválido para obtener causas: {IdPnc}", idPnc);
                    return Enumerable.Empty<PncCausaDto>();
                }

                _logger.LogInformation("Obteniendo causas del PNC: {IdPnc}", idPnc);
                return await _adapter.ObtenerCausasAsync(idPnc);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo causas del PNC {IdPnc}", idPnc);
                return Enumerable.Empty<PncCausaDto>();
            }
        }

        public async Task<(bool exitoso, string mensaje, long idCausa)> AgregarCausaAsync(
            PncCausaDto causa, long usuarioRegistra)
        {
            try
            {
                if (causa == null)
                {
                    _logger.LogWarning("Intento de agregar causa con DTO nulo");
                    return (false, "La causa no puede estar vacía", 0);
                }

                if (!causa.IdPnc.HasValue || causa.IdPnc <= 0)
                {
                    _logger.LogWarning("Causa sin PNC especificado");
                    return (false, "El PNC es obligatorio", 0);
                }

                if (string.IsNullOrWhiteSpace(causa.DescripcionCausa))
                {
                    _logger.LogWarning("Causa sin descripción");
                    return (false, "La descripción de la causa es obligatoria", 0);
                }

                if (string.IsNullOrWhiteSpace(causa.AccionCorrectiva))
                {
                    _logger.LogWarning("Causa sin acción correctiva");
                    return (false, "La acción correctiva es obligatoria", 0);
                }

                if (!causa.IdPersonaResponsable.HasValue || causa.IdPersonaResponsable <= 0)
                {
                    _logger.LogWarning("Causa sin responsable especificado");
                    return (false, "La persona responsable es obligatoria", 0);
                }

                // Verificar que el PNC existe
                var pnc = await _adapter.ObtenerPncAsync(causa.IdPnc.Value);
                if (pnc == null)
                {
                    _logger.LogWarning("Intento de agregar causa a PNC inexistente: {IdPnc}", causa.IdPnc);
                    return (false, "El PNC no existe", 0);
                }

                _logger.LogInformation(
                    "Agregando causa al PNC {IdPnc}: Descripción={Descripcion}",
                    causa.IdPnc, causa.DescripcionCausa);

                var idCausa = await _adapter.AgregarCausaAsync(causa, usuarioRegistra);

                _logger.LogInformation("Causa {IdCausa} agregada al PNC {IdPnc} por usuario {UsuarioId}",
                    idCausa, causa.IdPnc, usuarioRegistra);

                return (true, "Causa registrada exitosamente", idCausa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error agregando causa al PNC {IdPnc}. Usuario: {UsuarioId}",
                    causa?.IdPnc, usuarioRegistra);
                return (false, "Error al registrar la causa. Por favor intente nuevamente", 0);
            }
        }

        public async Task<IEnumerable<PncSeguimientoDto>> ObtenerSeguimientoAsync(long idPnc)
        {
            try
            {
                if (idPnc <= 0)
                {
                    _logger.LogWarning("ID de PNC inválido para obtener seguimiento: {IdPnc}", idPnc);
                    return Enumerable.Empty<PncSeguimientoDto>();
                }

                _logger.LogInformation("Obteniendo seguimiento del PNC: {IdPnc}", idPnc);
                var seguimiento = await _adapter.ObtenerSeguimientoAsync(idPnc);

                // Calcular días restantes
                var now = DateTime.Now;
                foreach (var item in seguimiento)
                {
                    if (item.FechaVencimiento.HasValue)
                    {
                        item.DiasRestantes = (int)(item.FechaVencimiento.Value.Date - now.Date).TotalDays;
                    }
                }

                return seguimiento;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo seguimiento del PNC {IdPnc}", idPnc);
                return Enumerable.Empty<PncSeguimientoDto>();
            }
        }

        public async Task<IEnumerable<PncLogDto>> ObtenerLogAsync(long idPnc)
        {
            try
            {
                if (idPnc <= 0)
                {
                    _logger.LogWarning("ID de PNC inválido para obtener log: {IdPnc}", idPnc);
                    return Enumerable.Empty<PncLogDto>();
                }

                _logger.LogInformation("Obteniendo log del PNC: {IdPnc}", idPnc);
                return await _adapter.ObtenerLogAsync(idPnc);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo log del PNC {IdPnc}", idPnc);
                return Enumerable.Empty<PncLogDto>();
            }
        }

        public async Task<PncResumenDto> ObtenerResumenAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo resumen de PNC");
                return await _adapter.ObtenerResumenAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo resumen de PNC");
                return new PncResumenDto();
            }
        }
    }
}
