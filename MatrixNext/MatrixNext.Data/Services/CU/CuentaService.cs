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
        private readonly BriefService _briefService;
        private readonly ILogger<CuentaService> _logger;

        public CuentaService(CuentaDataAdapter adapter, BriefService briefService, ILogger<CuentaService> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _briefService = briefService ?? throw new ArgumentNullException(nameof(briefService));
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
                // TODO-P0-03: Delegar la clonación al BriefService
                var (success, message, nuevoId) = _briefService.ClonarBrief(idBrief, idUsuario, idUnidad, nuevoNombre);
                return (success, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clonando brief {IdBrief}", idBrief);
                return (false, ex.Message);
            }
        }
    }
}
