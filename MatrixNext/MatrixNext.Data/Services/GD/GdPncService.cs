using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Models.GD;
using MatrixNext.Data.Services.GD.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.GD
{
    /// <summary>
    /// Servicio para gestión de Productos No Conformes
    /// SP utilizados: PNC_Productos_Get, PNC_Productos_Add, PNC_Seguimiento_Get,
    /// PNC_Productos_Causas_Add, PNC_ProductoNoConformeCausas_Get, PNC_ProductoNoConformeAcciones_Get,
    /// PNC_Productos_Log_Get, PNC_Productos_Log_Estado_Add, PNC_Producto_UpdateEstado
    /// </summary>
    public class GdPncService : IGdPncService
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<GdPncService> _logger;

        public GdPncService(IDbConnection connection, ILogger<GdPncService> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene lista de PNC según filtros
        /// SP: PNC_Productos_Get
        /// </summary>
        public async Task<(bool success, IEnumerable<PncDto> data)> ObtenerPncAsync(PncBusquedaParams? filtros = null)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", filtros?.Id);
                parameters.Add("@responsable", filtros?.Responsable);
                parameters.Add("@estado", filtros?.Estado);
                parameters.Add("@usuarioRegistra", filtros?.UsuarioRegistra);

                var result = await _connection.QueryAsync<PncDto>(
                    "PNC_Productos_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return (true, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener PNC. Filtros: {@Filtros}", filtros);
                return (false, Array.Empty<PncDto>());
            }
        }

        /// <summary>
        /// Obtiene un PNC por su ID
        /// </summary>
        public async Task<(bool success, PncDto? data)> ObtenerPncPorIdAsync(long id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@responsable", null);
                parameters.Add("@estado", null);
                parameters.Add("@usuarioRegistra", null);

                var result = await _connection.QueryFirstOrDefaultAsync<PncDto>(
                    "PNC_Productos_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return (true, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener PNC por ID: {Id}", id);
                return (false, null);
            }
        }

        /// <summary>
        /// Obtiene detalle completo de un PNC (producto, causas, acciones, historial)
        /// </summary>
        public async Task<(bool success, PncDetalleViewModel? data)> ObtenerDetalleCompletoAsync(long id)
        {
            try
            {
                var (successPnc, pnc) = await ObtenerPncPorIdAsync(id);
                if (!successPnc || pnc == null)
                    return (false, null);

                var (_, causas) = await ObtenerCausasAsync(id);
                var (_, acciones) = await ObtenerAccionesAsync(id);
                var (_, historial) = await ObtenerHistorialEstadosAsync(id);

                var viewModel = new PncDetalleViewModel
                {
                    Producto = pnc,
                    Causas = causas,
                    Acciones = acciones,
                    HistorialEstados = historial
                };

                return (true, viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalle completo de PNC: {Id}", id);
                return (false, null);
            }
        }

        /// <summary>
        /// Obtiene seguimiento de PNC por estado
        /// SP: PNC_Seguimiento_Get
        /// Estados: 1=Cerrado, 2=No tiene causas, 3=No tiene acciones, 4=Gestionado
        /// </summary>
        public async Task<(bool success, IEnumerable<PncSeguimientoDto> data)> ObtenerSeguimientoAsync(byte? estado = null)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Estado", estado);

                var result = await _connection.QueryAsync<PncSeguimientoDto>(
                    "PNC_Seguimiento_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return (true, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener seguimiento PNC. Estado: {Estado}", estado);
                return (false, Array.Empty<PncSeguimientoDto>());
            }
        }

        /// <summary>
        /// Crea un nuevo PNC
        /// SP: PNC_Productos_Add
        /// </summary>
        public async Task<(bool success, long idCreado, string message)> CrearPncAsync(PncCrearDto dto, long usuarioId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@asociadoA", dto.AsociadoA);
                parameters.Add("@proyectoId", dto.ProyectoId);
                parameters.Add("@trabajoId", dto.TrabajoId);
                parameters.Add("@proceso", dto.Proceso);
                parameters.Add("@procedimiento", dto.Procedimiento);
                parameters.Add("@unidad", dto.Unidad);
                parameters.Add("@personaIdentifica", dto.PersonaIdentifica);
                parameters.Add("@fechaReclamo", dto.FechaReclamo);
                parameters.Add("@fuente", dto.Fuente);
                parameters.Add("@categoria", dto.Categoria);
                parameters.Add("@tarea", dto.Tarea);
                parameters.Add("@responsable", dto.Responsable);
                parameters.Add("@informarA", dto.InformarA);
                parameters.Add("@descripcion", dto.Descripcion);
                parameters.Add("@estado", 1); // Estado inicial: Abierto
                parameters.Add("@fechaCreacion", DateTime.Now);
                parameters.Add("@usuario", usuarioId);

                var id = await _connection.ExecuteScalarAsync<long>(
                    "PNC_Productos_Add",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("PNC creado: {Id} por usuario {UsuarioId}", id, usuarioId);
                return (true, id, "Producto No Conforme registrado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear PNC. Usuario: {UsuarioId}", usuarioId);
                return (false, 0, "Error al registrar el Producto No Conforme");
            }
        }

        /// <summary>
        /// Actualiza el estado de un PNC
        /// SP: PNC_Producto_UpdateEstado, PNC_Productos_Log_Estado_Add
        /// </summary>
        public async Task<(bool success, string message)> ActualizarEstadoAsync(long id, byte estado, string observacion, long usuarioId)
        {
            try
            {
                // Actualizar estado
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@estado", estado);

                await _connection.ExecuteAsync(
                    "PNC_Producto_UpdateEstado",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                // Registrar en log
                var logParams = new DynamicParameters();
                logParams.Add("@idProducto", id);
                logParams.Add("@estado", estado);
                logParams.Add("@fecha", DateTime.Now);
                logParams.Add("@usuario", usuarioId);
                logParams.Add("@observacion", observacion);

                await _connection.ExecuteAsync(
                    "PNC_Productos_Log_Estado_Add",
                    logParams,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Estado de PNC {Id} actualizado a {Estado} por usuario {UsuarioId}", id, estado, usuarioId);
                return (true, "Estado actualizado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar estado de PNC: {Id}", id);
                return (false, "Error al actualizar el estado");
            }
        }

        /// <summary>
        /// Obtiene las causas de un PNC
        /// SP: PNC_ProductoNoConformeCausas_Get
        /// </summary>
        public async Task<(bool success, IEnumerable<PncCausaDto> data)> ObtenerCausasAsync(long pncId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdPNC", pncId);

                var result = await _connection.QueryAsync<PncCausaDto>(
                    "PNC_ProductoNoConformeCausas_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return (true, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener causas de PNC: {PncId}", pncId);
                return (false, Array.Empty<PncCausaDto>());
            }
        }

        /// <summary>
        /// Crea una causa para un PNC
        /// SP: PNC_Productos_Causas_Add
        /// </summary>
        public async Task<(bool success, long idCreado, string message)> CrearCausaAsync(long pncId, PncCausaDto dto, long usuarioId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@productoId", pncId);
                parameters.Add("@causa", dto.Causa);
                parameters.Add("@correccion", dto.Correccion);
                parameters.Add("@fechaEstimadaCierre", dto.FechaEstimadaCierre);
                parameters.Add("@usuario", usuarioId);
                parameters.Add("@fechaCreacion", DateTime.Now);

                var id = await _connection.ExecuteScalarAsync<long>(
                    "PNC_Productos_Causas_Add",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Causa creada para PNC {PncId}: {CausaId} por usuario {UsuarioId}", pncId, id, usuarioId);
                return (true, id, "Causa registrada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear causa para PNC: {PncId}", pncId);
                return (false, 0, "Error al registrar la causa");
            }
        }

        /// <summary>
        /// Obtiene las acciones de un PNC
        /// SP: PNC_ProductoNoConformeAcciones_Get
        /// </summary>
        public async Task<(bool success, IEnumerable<PncAccionDto> data)> ObtenerAccionesAsync(long pncId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdPNC", pncId);

                var result = await _connection.QueryAsync<PncAccionDto>(
                    "PNC_ProductoNoConformeAcciones_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return (true, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener acciones de PNC: {PncId}", pncId);
                return (false, Array.Empty<PncAccionDto>());
            }
        }

        /// <summary>
        /// Crea una acción para un PNC
        /// </summary>
        public async Task<(bool success, long idCreado, string message)> CrearAccionAsync(long pncId, long causaId, PncAccionDto dto, long usuarioId)
        {
            try
            {
                // Nota: El SP específico puede variar, usar query directo si es necesario
                var sql = @"
                    INSERT INTO PNC_ProductoNoConformeAcciones 
                    (IdPNC, IdCausa, Accion, Responsable, FechaCompromiso, FechaCierre, Observacion, Estado, Usuario, FechaCreacion)
                    VALUES 
                    (@PncId, @CausaId, @Accion, @ResponsableId, @FechaCompromiso, @FechaCierre, @Observacion, 1, @Usuario, GETDATE());
                    SELECT SCOPE_IDENTITY();";

                var id = await _connection.ExecuteScalarAsync<long>(sql, new
                {
                    PncId = pncId,
                    CausaId = causaId,
                    dto.Accion,
                    dto.ResponsableId,
                    dto.FechaCompromiso,
                    dto.FechaCierre,
                    dto.Observacion,
                    Usuario = usuarioId
                });

                _logger.LogInformation("Acción creada para PNC {PncId}, Causa {CausaId}: {AccionId}", pncId, causaId, id);
                return (true, id, "Acción registrada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear acción para PNC: {PncId}, Causa: {CausaId}", pncId, causaId);
                return (false, 0, "Error al registrar la acción");
            }
        }

        /// <summary>
        /// Obtiene historial de estados de un PNC
        /// SP: PNC_Productos_Log_Get
        /// </summary>
        public async Task<(bool success, IEnumerable<PncLogEstadoDto> data)> ObtenerHistorialEstadosAsync(long pncId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idProducto", pncId);

                var result = await _connection.QueryAsync<PncLogEstadoDto>(
                    "PNC_Productos_Log_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return (true, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial de estados de PNC: {PncId}", pncId);
                return (false, Array.Empty<PncLogEstadoDto>());
            }
        }

        /// <summary>
        /// Obtiene catálogo de procesos
        /// </summary>
        public async Task<(bool success, IEnumerable<CatalogoItem> data)> ObtenerProcesosAsync()
        {
            try
            {
                var result = await _connection.QueryAsync<CatalogoItem>(
                    "SELECT id AS Id, Descripcion AS Nombre FROM PNC_Procesos WHERE Activo = 1 ORDER BY Descripcion"
                );
                return (true, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener catálogo de procesos PNC");
                return (false, Array.Empty<CatalogoItem>());
            }
        }

        /// <summary>
        /// Obtiene catálogo de categorías
        /// </summary>
        public async Task<(bool success, IEnumerable<CatalogoItem> data)> ObtenerCategoriasAsync()
        {
            try
            {
                var result = await _connection.QueryAsync<CatalogoItem>(
                    "SELECT Id, Descripcion AS Nombre FROM PNC_Categorias WHERE Activo = 1 ORDER BY Descripcion"
                );
                return (true, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener catálogo de categorías PNC");
                return (false, Array.Empty<CatalogoItem>());
            }
        }

        /// <summary>
        /// Obtiene catálogo de fuentes de reclamo
        /// </summary>
        public async Task<(bool success, IEnumerable<CatalogoItem> data)> ObtenerFuentesAsync()
        {
            try
            {
                var result = await _connection.QueryAsync<CatalogoItem>(
                    "SELECT Id, Descripcion AS Nombre FROM PNC_FuenteReclamo WHERE Activo = 1 ORDER BY Descripcion"
                );
                return (true, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener catálogo de fuentes PNC");
                return (false, Array.Empty<CatalogoItem>());
            }
        }

        /// <summary>
        /// Obtiene procedimientos por proceso
        /// </summary>
        public async Task<(bool success, IEnumerable<CatalogoItem> data)> ObtenerProcedimientosAsync(byte procesoId)
        {
            try
            {
                var result = await _connection.QueryAsync<CatalogoItem>(
                    "SELECT id AS Id, Descripcion AS Nombre FROM PNC_Procedimientos WHERE ProcesoId = @ProcesoId AND Activo = 1 ORDER BY Descripcion",
                    new { ProcesoId = procesoId }
                );
                return (true, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener procedimientos para proceso: {ProcesoId}", procesoId);
                return (false, Array.Empty<CatalogoItem>());
            }
        }

        /// <summary>
        /// Prepara el ViewModel para la vista Index
        /// </summary>
        public async Task<PncIndexViewModel> PrepararViewModelAsync(PncBusquedaParams? filtros, long usuarioId)
        {
            var viewModel = new PncIndexViewModel
            {
                Filtros = filtros ?? new PncBusquedaParams()
            };

            // Cargar PNC del usuario
            var filtrosConUsuario = filtros ?? new PncBusquedaParams();
            filtrosConUsuario.UsuarioRegistra = usuarioId;
            
            var (_, productos) = await ObtenerPncAsync(filtrosConUsuario);
            viewModel.Productos = productos;

            // Cargar seguimiento
            var (_, seguimiento) = await ObtenerSeguimientoAsync(filtros?.Estado);
            viewModel.Seguimiento = seguimiento;

            // Cargar catálogos para filtros
            var (_, procesos) = await ObtenerProcesosAsync();
            viewModel.Procesos = procesos;

            var (_, categorias) = await ObtenerCategoriasAsync();
            viewModel.Categorias = categorias;

            var (_, fuentes) = await ObtenerFuentesAsync();
            viewModel.Fuentes = fuentes;

            // Estados fijos
            viewModel.Estados = new List<CatalogoItem>
            {
                new CatalogoItem { Id = 1, Nombre = "Cerrado" },
                new CatalogoItem { Id = 2, Nombre = "Sin causas" },
                new CatalogoItem { Id = 3, Nombre = "Sin acciones" },
                new CatalogoItem { Id = 4, Nombre = "Gestionado" }
            };

            return viewModel;
        }
    }
}