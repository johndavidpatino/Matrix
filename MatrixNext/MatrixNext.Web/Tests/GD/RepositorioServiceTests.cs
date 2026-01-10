using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD;
using MatrixNext.Data.Adapters.GD.Models;
using MatrixNext.Data.Services.GD;
using Microsoft.Extensions.Logging.Abstractions;

namespace MatrixNext.Web.Tests.GD
{
    /// <summary>
    /// Pruebas básicas en memoria para GdRepositorioService (sin hitting BD)
    /// </summary>
    public class RepositorioServiceTests
    {
        private readonly FakeRepositorioAdapter _adapter = new();
        private readonly GdRepositorioService _service;

        public RepositorioServiceTests()
        {
            _service = new GdRepositorioService(_adapter, NullLogger<GdRepositorioService>.Instance);
        }

        public async Task<IReadOnlyList<BasicTestResult>> RunAllAsync()
        {
            return new List<BasicTestResult>
            {
                await ShouldUploadAndVersionAsync(),
                await ShouldRejectInvalidIdsAsync()
            };
        }

        private async Task<BasicTestResult> ShouldUploadAndVersionAsync()
        {
            var dto = new UploadDocumentoDto
            {
                IdContenedor = 1,
                TipoContenedor = 1,
                IdDocumento = 10,
                UrlArchivo = "/uploads/gd/test.pdf",
                Comentarios = "prueba",
                UsuarioId = 7
            };

            var first = await _service.SubirDocumento(dto);
            var second = await _service.SubirDocumento(dto);

            var passed = first.success && first.version == 1 && second.version == 2;
            var message = passed ? "Versionamiento incrementa correctamente" : "El versionamiento no incrementó";

            return new BasicTestResult
            {
                Name = "Versionamiento en memoria",
                Passed = passed,
                Message = message
            };
        }

        private async Task<BasicTestResult> ShouldRejectInvalidIdsAsync()
        {
            var dto = new UploadDocumentoDto
            {
                IdContenedor = 0,
                TipoContenedor = 1,
                IdDocumento = 0,
                UrlArchivo = "/uploads/gd/invalid.pdf",
                UsuarioId = 1
            };

            var result = await _service.SubirDocumento(dto);
            var passed = !result.success;

            return new BasicTestResult
            {
                Name = "Valida ids requeridos",
                Passed = passed,
                Message = passed ? "Se rechazaron ids inválidos" : "Aceptó ids inválidos"
            };
        }
    }

    public class FakeRepositorioAdapter : IGdRepositorioAdapter
    {
        private readonly List<RepositorioDocumentoDto> _store = new();

        public Task<bool> EliminarDocumento(int id)
        {
            var removed = _store.RemoveAll(d => d.Id == id) > 0;
            return Task.FromResult(removed);
        }

        public Task<int> GuardarDocumento(UploadDocumentoDto dto, decimal version)
        {
            var nextId = _store.Count + 1;
            _store.Add(new RepositorioDocumentoDto
            {
                Id = nextId,
                IdContenedor = dto.IdContenedor,
                TipoContenedor = dto.TipoContenedor,
                IdDocumento = dto.IdDocumento,
                UrlArchivo = dto.UrlArchivo,
                Version = version,
                Comentarios = dto.Comentarios,
                UsuarioId = dto.UsuarioId,
                FechaRegistro = System.DateTime.UtcNow,
                NombreArchivo = System.IO.Path.GetFileName(dto.UrlArchivo)
            });
            return Task.FromResult(nextId);
        }

        public Task<RepositorioDocumentoDto?> ObtenerDocumentoById(int id)
        {
            var doc = _store.FirstOrDefault(d => d.Id == id);
            return Task.FromResult(doc);
        }

        public Task<List<RepositorioDocumentoDto>> ObtenerDocumentosContenedor(int idContenedor)
        {
            var docs = _store.Where(d => d.IdContenedor == idContenedor).ToList();
            return Task.FromResult(docs);
        }

        public Task<List<RepositorioListDto>> ObtenerDocumentos(int idContenedor, int tipoContenedor)
        {
            _ = tipoContenedor;
            var docs = _store
                .Where(d => d.IdContenedor == idContenedor)
                .Select(d => new RepositorioListDto
                {
                    Id = d.Id,
                    NombreArchivo = d.NombreArchivo,
                    Version = d.Version,
                    FechaRegistro = d.FechaRegistro,
                    RegistradoPor = string.Empty,
                    Comentarios = d.Comentarios
                })
                .ToList();

            return Task.FromResult(docs);
        }

        public Task<decimal> ObtenerProximaVersion(int idContenedor, int idDocumento)
        {
            var next = _store
                .Where(d => d.IdContenedor == idContenedor && d.IdDocumento == idDocumento)
                .Select(d => d.Version)
                .DefaultIfEmpty(0)
                .Max() + 1;

            return Task.FromResult(next);
        }
    }

    public class BasicTestResult
    {
        public string Name { get; set; } = string.Empty;
        public bool Passed { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
