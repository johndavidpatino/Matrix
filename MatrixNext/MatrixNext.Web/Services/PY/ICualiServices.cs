using MatrixNext.Web.Models.PY;
using MatrixNext.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.PY
{
    public interface ISesionesCualiService
    {
        Task<List<SesionesCuali>> ObtenerPorTrabajoAsync(long idTrabajoCuali);
        Task<List<SesionesCuali>> ObtenerPorSegmentoAsync(long idSegmento);
        Task<List<SesionesCuali>> ObtenerPorEstadoAsync(string estado);
        Task<SesionesCuali> ObtenerPorIdAsync(long id);
        Task<ResultVM<long>> CrearAsync(SesionesCuali sesion, long idUsuario);
        Task<ResultVM<bool>> ActualizarAsync(SesionesCuali sesion, long idUsuario);
        Task<ResultVM<bool>> CambiarEstadoAsync(long idSesion, string nuevoEstado, long idUsuario, string observacion = null);
        Task<ResultVM<bool>> EliminarAsync(long idSesion, long idUsuario);
        Task<ResultVM<bool>> RegistrarAsistenciaAsync(long idSesion, List<long> idsParticipantes, long idUsuario);
    }

    public interface IMuestrasCualiService
    {
        Task<List<MuestrasCuali>> ObtenerPorTrabajoAsync(long idTrabajoCuali);
        Task<List<MuestrasCuali>> ObtenerPorSegmentoAsync(long idSegmento);
        Task<List<MuestrasCuali>> ObtenerPorEstadoAsync(string estado);
        Task<MuestrasCuali> ObtenerPorIdAsync(long id);
        Task<ResultVM<long>> CrearAsync(MuestrasCuali muestra, long idUsuario);
        Task<ResultVM<bool>> ActualizarAsync(MuestrasCuali muestra, long idUsuario);
        Task<ResultVM<bool>> CambiarEstadoAsync(long idMuestra, string nuevoEstado, long idUsuario);
        Task<ResultVM<bool>> EliminarAsync(long idMuestra, long idUsuario);
        Task<ResultVM<bool>> AsignarEntrevistadorAsync(long idMuestra, long idEntrevistador, long idUsuario);
    }

    public interface IEntrevistadorasCualiService
    {
        Task<List<EntrevistadorasCuali>> ObtenerPorTrabajoAsync(long idTrabajoCuali);
        Task<List<EntrevistadorasCuali>> ObtenerPorSegmentoAsync(long idSegmento);
        Task<List<EntrevistadorasCuali>> ObtenerDisponiblesAsync();
        Task<EntrevistadorasCuali> ObtenerPorIdAsync(long id);
        Task<ResultVM<long>> CrearAsync(EntrevistadorasCuali entrevistador, long idUsuario);
        Task<ResultVM<bool>> ActualizarAsync(EntrevistadorasCuali entrevistador, long idUsuario);
        Task<ResultVM<bool>> CambiarDisponibilidadAsync(long idEntrevistador, string nuevaDisponibilidad, long idUsuario);
        Task<ResultVM<bool>> EliminarAsync(long idEntrevistador, long idUsuario);
        Task<ResultVM<bool>> ActualizarPorcentajeCumplimientoAsync(long idEntrevistador);
    }
}
