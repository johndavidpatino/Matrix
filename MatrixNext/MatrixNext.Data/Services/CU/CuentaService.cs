using System;
using System.Collections.Generic;
using MatrixNext.Data.Adapters.CU;
using MatrixNext.Data.Modules.CU.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.CU
{
    public class CuentaService
    {
        private readonly CuentaDataAdapter _adapter;
        private readonly ILogger<CuentaService> _logger;

        public CuentaService(CuentaDataAdapter adapter, ILogger<CuentaService> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public JobBookSearchViewModel Buscar(JobBookSearchViewModel filtros, long usuarioId)
        {
            filtros ??= new JobBookSearchViewModel();
            try
            {
                filtros.Resultados = _adapter.BuscarJobBooks(
                    filtros.Titulo,
                    filtros.JobBook,
                    filtros.IdPropuesta,
                    usuarioId,
                    filtros.TypeSearch <= 0 ? 1 : filtros.TypeSearch
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error buscando JobBooks");
                filtros.Resultados = new List<JobBookResultViewModel>();
            }

            return filtros;
        }

        public JobBookContextViewModel? ObtenerContexto(long? idBrief, long? idPropuesta, long? idEstudio, long usuarioId)
        {
            try
            {
                return _adapter.ObtenerContexto(idBrief, idPropuesta, idEstudio, usuarioId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo contexto de JobBook");
                return null;
            }
        }

        public (bool success, string message) ClonarBrief(long idBrief, long idUsuario, int idUnidad, string nuevoNombre)
        {
            try
            {
                _adapter.ClonarBrief(idBrief, idUsuario, idUnidad, nuevoNombre);
                return (true, "Brief clonado correctamente");
            }
            catch (NotImplementedException nie)
            {
                _logger.LogWarning(nie, "CloneBrief pendiente de confirmacion en legacy");
                return (false, "Clonación de Brief no implementada. Revisar MatrixNext/Areas/CU/TODO_CU_CUENTAS.md (falta confirmar SP legacy).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clonando brief");
                return (false, ex.Message);
            }
        }
    }
}
