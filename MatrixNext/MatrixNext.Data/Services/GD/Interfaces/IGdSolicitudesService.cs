using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD.Models;

namespace MatrixNext.Data.Services.GD.Interfaces
{
    public interface IGdSolicitudesService
    {
        // Lectura
        Task<(bool success, List<SolicitudListDto> data, string message)> ObtenerSolicitudes();
        Task<(bool success, SolicitudDocumentoDto? data, string message)> ObtenerSolicitudById(int id);

        // Creación
        Task<(bool success, int id, string message)> CrearSolicitud(SolicitudCreateInputDto dto);

        // Revisores
        Task<(bool success, string message)> AsignarRevisores(int idSolicitud, List<int> idRevisores);

        // Form data
        Task<(bool success, SolicitudFormDataDto formData)> ObtenerFormData();
    }
}
