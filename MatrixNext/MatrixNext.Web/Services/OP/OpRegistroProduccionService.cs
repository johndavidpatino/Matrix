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
    /// Servicio para el registro de actividades de producción en OP.
    /// Implementa lógica de cascading dropdowns, búsqueda de JobBooks y validaciones.
    /// </summary>
    public class OpRegistroProduccionService : IOpRegistroProduccionService
    {
        private readonly MatrixDbContext _context;
        private readonly ILogger<OpRegistroProduccionService> _logger;

        public OpRegistroProduccionService(
            MatrixDbContext context,
            ILogger<OpRegistroProduccionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<CatalogoItemDto>> ObtenerUnidadesAsync()
        {
            try
            {
                // TODO: Consultar tabla de unidades/áreas disponibles
                // SELECT IdUnidad, NombreUnidad FROM Catalogo_Unidades WHERE Activo=1
                var unidades = new List<CatalogoItemDto>();

                _logger.LogInformation("Obtenidas {Count} unidades para registro", unidades.Count);
                return unidades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo unidades");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<List<CatalogoItemDto>> ObtenerActividadesAsync(int unidadId)
        {
            try
            {
                // TODO: Consultar actividades por unidad (cascada)
                // SELECT IdActividad, NombreActividad FROM Catalogo_Actividades 
                // WHERE IdUnidad=@UnidadId AND Activo=1
                var actividades = new List<CatalogoItemDto>();

                _logger.LogInformation("Obtenidas {Count} actividades para unidad {UnidadId}", actividades.Count, unidadId);
                return actividades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo actividades para unidad {UnidadId}", unidadId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<List<CatalogoItemDto>> ObtenerSubactividadesAsync(int actividadId)
        {
            try
            {
                // TODO: Consultar subactividades por actividad (cascada)
                // SELECT IdSubactividad, NombreSubactividad FROM Catalogo_Subactividades 
                // WHERE IdActividad=@ActividadId AND Activo=1
                var subactividades = new List<CatalogoItemDto>();

                _logger.LogInformation("Obtenidas {Count} subactividades para actividad {ActividadId}", subactividades.Count, actividadId);
                return subactividades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo subactividades para actividad {ActividadId}", actividadId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<List<JobBookDto>> BuscarJobBooksAsync(string criterio, string tipo)
        {
            try
            {
                // TODO: Implementar búsqueda según tipo
                // Tipos soportados: "JBE" (JobBook Encuesta), "JBI" (JobBook Interno), "CC" (Centro de Costo)
                // SELECT IdJobBook, NombreJobBook FROM JobBooks WHERE Tipo=@Tipo AND (Codigo LIKE @Criterio OR Nombre LIKE @Criterio)

                if (string.IsNullOrWhiteSpace(criterio))
                    return new List<JobBookDto>();

                var jobBooks = new List<JobBookDto>();

                _logger.LogInformation("Búsqueda de JobBooks: tipo={Tipo}, criterio={Criterio}, resultados={Count}", tipo, criterio, jobBooks.Count);
                return jobBooks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error buscando JobBooks con criterio {Criterio}", criterio);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<int> RegistrarActividadAsync(RegistroProduccionDto registro)
        {
            try
            {
                // TODO: Validar primero
                var (valido, mensaje) = await ValidarRegistroAsync(registro);
                if (!valido)
                    throw new InvalidOperationException($"Registro inválido: {mensaje}");

                // TODO: Insertar en tabla OP_RegistroProduccion
                // - IdUnidad
                // - IdActividad
                // - IdSubactividad
                // - IdJobBook
                // - Cantidad
                // - HoraInicio / HoraFin
                // - Fecha
                // - UsuarioRegistro
                // - FechaRegistro

                int idRegistro = 0; // Placeholder

                _logger.LogInformation("Actividad de producción registrada: ID={IdRegistro}, Usuario={Usuario}", idRegistro, registro.UsuarioId);
                return idRegistro;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando actividad de producción");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<(bool Valid, string Message)> ValidarRegistroAsync(RegistroProduccionDto registro)
        {
            try
            {
                if (registro == null)
                    return (false, "El registro no puede ser nulo");

                if (registro.UnidadId <= 0)
                    return (false, "Debe seleccionar una unidad");

                if (registro.ActividadId <= 0)
                    return (false, "Debe seleccionar una actividad");

                if (registro.SubactividadId <= 0)
                    return (false, "Debe seleccionar una subactividad");

                if (registro.Cantidad <= 0)
                    return (false, "La cantidad debe ser mayor a 0");

                if (string.IsNullOrWhiteSpace(registro.Fecha))
                    return (false, "Debe especificar la fecha del registro");

                if (!DateTime.TryParse(registro.Fecha, out var fecha))
                    return (false, "La fecha tiene formato inválido");

                if (fecha > DateTime.Now)
                    return (false, "No se puede registrar actividades en fechas futuras");

                return (true, "");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando registro de producción");
                throw;
            }
        }
    }
}
