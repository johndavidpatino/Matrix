using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD.Models;

namespace MatrixNext.Data.Services.GD.Interfaces
{
    public interface IGdMaestroService
    {
        Task<(bool success, List<MaestroDocumentoDto> data)> ObtenerMaestros();
        Task<(bool success, MaestroDocumentoDto? data, DocumentoControlledDto? controlado)> ObtenerMaestroById(int id);
        Task<(bool success, int idCreado, string message)> CrearMaestro(MaestroDocumentoDto dto);
        Task<(bool success, string message)> ActualizarMaestro(int id, MaestroDocumentoDto dto);
        Task<(bool success, string message)> AnularMaestro(int id);
        Task<(bool success, MaestroFormDataDto data)> ObtenerFormData();
    }
}
