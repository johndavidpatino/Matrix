using Microsoft.AspNetCore.Http;

namespace MatrixNext.Web.Services.OP;

public interface IOpCargaService
{
    Task<OpCargaResult> ProcesarArchivoAsync(IFormFile archivo, OpCargaTipo tipo, CancellationToken cancellationToken = default);
}

public enum OpCargaTipo
{
    CatiRMC,
    Planillas
}

public sealed record OpCargaResult(bool EsValido, string Mensaje);
