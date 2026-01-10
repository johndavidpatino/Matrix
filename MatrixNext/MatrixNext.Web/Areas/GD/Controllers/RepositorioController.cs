using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD.Models;
using MatrixNext.Data.Services.GD.Interfaces;
using MatrixNext.Web.Models.ViewModels.GD;
using MatrixNext.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.GD.Controllers
{
    [Area("GD")]
    [Authorize]
    public class RepositorioController : Controller
    {
        private readonly IGdRepositorioService _service;
        private readonly IUploadService _uploadService;
        private readonly ILogger<RepositorioController> _logger;

        public RepositorioController(
            IGdRepositorioService service,
            IUploadService uploadService,
            ILogger<RepositorioController> logger)
        {
            _service = service;
            _uploadService = uploadService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int idContenedor, int tipoContenedor, int? idDocumento = null)
        {
            ViewData["Title"] = "Repositorio de Documentos";

            var vm = new RepositorioIndexVM
            {
                IdContenedor = idContenedor,
                TipoContenedor = tipoContenedor,
                IdDocumento = idDocumento,
                Documentos = new System.Collections.Generic.List<RepositorioListVM>()
            };

            if (idContenedor <= 0)
            {
                ViewData["Error"] = "Debe especificar un contenedor (IdContenedor).";
                return View(vm);
            }

            var (success, data, message) = await _service.ObtenerDocumentos(idContenedor, tipoContenedor);
            if (success)
            {
                vm.Documentos = data.Select(ToListVm).ToList();
            }
            else
            {
                ViewData["Error"] = string.IsNullOrWhiteSpace(message) ? "No se pudieron obtener los documentos" : message;
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Upload(int idContenedor, int tipoContenedor, int? idDocumento = null)
        {
            var vm = new UploadDocumentoVM
            {
                IdContenedor = idContenedor,
                TipoContenedor = tipoContenedor,
                IdDocumento = idDocumento ?? 0
            };

            return PartialView("~/Areas/GD/Views/Repositorio/_UploadModal.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(UploadDocumentoVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Areas/GD/Views/Repositorio/_UploadModal.cshtml", vm);
            }

            var userId = GetUserId();
            if (userId <= 0)
            {
                return Json(new { success = false, message = "Usuario no autenticado" });
            }

            if (vm.Archivo == null)
            {
                return Json(new { success = false, message = "Archivo requerido" });
            }

            UploadResultVM? uploadResult = null;
            try
            {
                uploadResult = await _uploadService.SubirArchivoAsync("GD", vm.IdContenedor, vm.Archivo);

                var dto = new UploadDocumentoDto
                {
                    IdContenedor = vm.IdContenedor,
                    TipoContenedor = vm.TipoContenedor,
                    IdDocumento = vm.IdDocumento,
                    UrlArchivo = uploadResult.RutaRelativa ?? string.Empty,
                    Comentarios = vm.Comentarios ?? string.Empty,
                    UsuarioId = userId
                };

                var (success, idCreado, version, message) = await _service.SubirDocumento(dto);

                if (!success)
                {
                    // Rollback archivo físico si falla persistencia
                    if (!string.IsNullOrWhiteSpace(uploadResult.RutaRelativa))
                    {
                        await _uploadService.EliminarArchivoAsync(uploadResult.RutaRelativa, userId, "Rollback por error al guardar en BD");
                    }

                    return Json(new { success = false, message });
                }

                _logger.LogInformation("Documento {IdDocumentoRepo} subido por {UserId} en contenedor {Contenedor}", idCreado, userId, vm.IdContenedor);

                return Json(new
                {
                    success = true,
                    message = message,
                    id = idCreado,
                    version,
                    nombre = uploadResult.NombreArchivo,
                    ruta = uploadResult.RutaRelativa
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error subiendo documento al repositorio");

                if (uploadResult != null && !string.IsNullOrWhiteSpace(uploadResult.RutaRelativa))
                {
                    await _uploadService.EliminarArchivoAsync(uploadResult.RutaRelativa!, userId, "Rollback por excepción");
                }

                return Json(new { success = false, message = "Error subiendo el documento" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Descargar(int id)
        {
            var (success, doc, _) = await _service.ObtenerDocumento(id);
            if (!success || doc == null)
            {
                return NotFound();
            }

            var userId = GetUserId();

            var fileStream = await _uploadService.DescargarArchivoAsync(doc.UrlArchivo, userId);
            return fileStream;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (found, doc, message) = await _service.ObtenerDocumento(id);
            if (!found || doc == null)
            {
                return Json(new { success = false, message = string.IsNullOrWhiteSpace(message) ? "No encontrado" : message });
            }

            var userId = GetUserId();

            try
            {
                if (!string.IsNullOrWhiteSpace(doc.UrlArchivo))
                {
                    await _uploadService.EliminarArchivoAsync(doc.UrlArchivo, userId, "Eliminado desde repositorio");
                }

                var (success, msg) = await _service.EliminarDocumento(id);
                return Json(new { success, message = msg });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error eliminando documento {DocId}", id);
                return Json(new { success = false, message = "Error eliminando documento" });
            }
        }

        private static RepositorioListVM ToListVm(RepositorioListDto dto)
        {
            return new RepositorioListVM
            {
                Id = dto.Id,
                NombreArchivo = dto.NombreArchivo,
                Version = dto.Version,
                FechaRegistro = dto.FechaRegistro,
                Comentarios = dto.Comentarios,
                RegistradoPor = dto.RegistradoPor
            };
        }

        private int GetUserId()
        {
            var claimValue = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";
            return int.TryParse(claimValue, out var id) ? id : 0;
        }
    }
}
