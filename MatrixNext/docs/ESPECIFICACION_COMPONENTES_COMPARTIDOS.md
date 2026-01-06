# ESPECIFICACION_COMPONENTES_COMPARTIDOS

**Fase 4: Especificación Técnica de Componentes Compartidos** - Services, Partials, Interfaces

Documento generado: 6 enero 2026
Estatus: 🔄 EN CONSTRUCCIÓN

---

## 📊 Resumen Ejecutivo

Diseño de componentes **reutilizables** entre PY, CORE, OP, CU módulos:

1. **UploadService** (gestión de archivos)
2. **GridService** (paginación, filtros, ordenamiento)
3. **PermisosService** (autorización)
4. **EmailService** (notificaciones)
5. **Partials compartidos** (_Grid.cshtml, _Upload.cshtml, _Confirm.cshtml)
6. **ViewModels base** (BaseVM, ResultVM, PaginationVM)

---

## 1️⃣ UploadService

### 1.1 Interfaz

```csharp
public interface IUploadService
{
  /// <summary>
  /// Sube un archivo a carpeta del módulo
  /// </summary>
  /// <param name="moduleId">Ej: "PY" (Proyectos), "CORE" (Tareas), "OP" (Operaciones)</param>
  /// <param name="entityId">Ej: IdProyecto, IdTrabajo, IdTarea</param>
  /// <param name="file">IFormFile multipart/form-data</param>
  /// <returns>Ruta relativa ej: "/uploads/PY/20/documento.pdf"</returns>
  Task<UploadResultVM> SubirArchivoAsync(string moduleId, long entityId, IFormFile file);

  /// <summary>
  /// Descarga archivo verificando permisos
  /// </summary>
  Task<FileStreamResult> DescargarArchivoAsync(string rutaRelativa, long usuarioId);

  /// <summary>
  /// Elimina archivo y registra auditoría
  /// </summary>
  Task<bool> EliminarArchivoAsync(string rutaRelativa, long usuarioId, string razon);

  /// <summary>
  /// Lista archivos de una entidad
  /// </summary>
  Task<List<ArchivoVM>> ListarArchivosAsync(string moduleId, long entityId);
}
```

### 1.2 Implementación

```csharp
public class UploadService : IUploadService
{
  private readonly IWebHostEnvironment _hostEnv;
  private readonly IAuditoriaService _auditoria;
  private readonly string _basePath = "uploads";

  public UploadService(IWebHostEnvironment hostEnv, IAuditoriaService auditoria)
  {
    _hostEnv = hostEnv;
    _auditoria = auditoria;
  }

  public async Task<UploadResultVM> SubirArchivoAsync(string moduleId, long entityId, IFormFile file)
  {
    // Validar
    if (file == null || file.Length == 0)
      throw new ArgumentException("Archivo vacío");

    var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".csv" };
    var fileExt = Path.GetExtension(file.FileName).ToLower();
    if (!allowedExtensions.Contains(fileExt))
      throw new ArgumentException($"Extensión no permitida: {fileExt}");

    // Crear ruta
    var carpetaModulo = Path.Combine(_hostEnv.WebRootPath, _basePath, moduleId);
    var carpetaEntidad = Path.Combine(carpetaModulo, entityId.ToString());
    Directory.CreateDirectory(carpetaEntidad);

    // Generar nombre único
    var nombreUnico = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
    var rutaFisica = Path.Combine(carpetaEntidad, nombreUnico);
    var rutaRelativa = Path.Combine(_basePath, moduleId, entityId.ToString(), nombreUnico)
      .Replace("\\", "/");

    // Guardar archivo
    using (var fileStream = new FileStream(rutaFisica, FileMode.Create))
    {
      await file.CopyToAsync(fileStream);
    }

    // Auditoría
    await _auditoria.LogearAsync(new AuditoriaVM
    {
      Entidad = $"{moduleId}_Archivo",
      EntidadId = entityId,
      Accion = "Upload",
      Detalles = $"Archivo: {file.FileName}, Tamaño: {file.Length} bytes",
      RutaArchivo = rutaRelativa
    });

    return new UploadResultVM
    {
      RutaRelativa = rutaRelativa,
      RutaAbsoluta = Path.Combine(_basePath, rutaRelativa),
      NombreArchivo = file.FileName,
      TamañoBytes = file.Length,
      FechaSubida = DateTime.UtcNow
    };
  }

  public async Task<FileStreamResult> DescargarArchivoAsync(string rutaRelativa, long usuarioId)
  {
    var rutaFisica = Path.Combine(_hostEnv.WebRootPath, rutaRelativa);

    if (!File.Exists(rutaFisica))
      throw new FileNotFoundException("Archivo no encontrado");

    // Validar permisos (ej: usuario creó este archivo)
    // ⚠️ Agregar validación según lógica de negocio

    var fileStream = new FileStream(rutaFisica, FileMode.Open, FileAccess.Read);
    var nombreArchivo = Path.GetFileName(rutaFisica);
    var mimeType = ObtenerMimeType(rutaFisica);

    // Auditoría
    await _auditoria.LogearAsync(new AuditoriaVM
    {
      Accion = "Download",
      Detalles = $"Usuario {usuarioId} descargó {rutaRelativa}",
      RutaArchivo = rutaRelativa
    });

    return new FileStreamResult(fileStream, mimeType) { FileDownloadName = nombreArchivo };
  }

  public async Task<bool> EliminarArchivoAsync(string rutaRelativa, long usuarioId, string razon)
  {
    var rutaFisica = Path.Combine(_hostEnv.WebRootPath, rutaRelativa);

    if (!File.Exists(rutaFisica))
      return false;

    File.Delete(rutaFisica);

    await _auditoria.LogearAsync(new AuditoriaVM
    {
      Accion = "Delete",
      Detalles = $"Usuario {usuarioId} eliminó {rutaRelativa}. Razón: {razon}",
      RutaArchivo = rutaRelativa
    });

    return true;
  }

  public async Task<List<ArchivoVM>> ListarArchivosAsync(string moduleId, long entityId)
  {
    var carpetaEntidad = Path.Combine(_hostEnv.WebRootPath, _basePath, moduleId, entityId.ToString());

    if (!Directory.Exists(carpetaEntidad))
      return new List<ArchivoVM>();

    var archivos = new List<ArchivoVM>();
    foreach (var archivo in Directory.GetFiles(carpetaEntidad))
    {
      var info = new FileInfo(archivo);
      archivos.Add(new ArchivoVM
      {
        NombreArchivo = Path.GetFileName(archivo),
        RutaRelativa = $"/{_basePath}/{moduleId}/{entityId}/{Path.GetFileName(archivo)}".Replace("\\", "/"),
        TamañoKB = Math.Round((decimal)info.Length / 1024, 2),
        FechaCreacion = info.CreationTime
      });
    }

    return archivos;
  }

  private string ObtenerMimeType(string rutaArchivo)
  {
    var ext = Path.GetExtension(rutaArchivo).ToLower();
    return ext switch
    {
      ".pdf" => "application/pdf",
      ".doc" => "application/msword",
      ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
      ".xls" => "application/vnd.ms-excel",
      ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      ".csv" => "text/csv",
      _ => "application/octet-stream"
    };
  }
}
```

### 1.3 ViewModels

```csharp
public class UploadResultVM
{
  public string RutaRelativa { get; set; }
  public string RutaAbsoluta { get; set; }
  public string NombreArchivo { get; set; }
  public long TamañoBytes { get; set; }
  public DateTime FechaSubida { get; set; }
}

public class ArchivoVM
{
  public string NombreArchivo { get; set; }
  public string RutaRelativa { get; set; }
  public decimal TamañoKB { get; set; }
  public DateTime FechaCreacion { get; set; }
}
```

### 1.4 Uso en Controllers

```csharp
[HttpPost("SubirDocumento")]
[Authorize(Roles = "GerenteProyectos,Coordinador")]
public async Task<IActionResult> SubirDocumento(long proyectoId, IFormFile file)
{
  try
  {
    var resultado = await _uploadService.SubirArchivoAsync("PY", proyectoId, file);
    return Ok(resultado);
  }
  catch (ArgumentException ex)
  {
    return BadRequest(ex.Message);
  }
}

[HttpGet("DescargarDocumento")]
public async Task<IActionResult> DescargarDocumento(string rutaRelativa)
{
  var usuarioId = User.GetUserId();
  try
  {
    return await _uploadService.DescargarArchivoAsync(rutaRelativa, usuarioId);
  }
  catch (FileNotFoundException)
  {
    return NotFound();
  }
}
```

---

## 2️⃣ GridService

### 2.1 Interfaz

```csharp
public interface IGridService
{
  /// <summary>
  /// Pagina, filtra y ordena resultados
  /// </summary>
  Task<PaginationVM<T>> PaginarAsync<T>(
    string sql,
    int pageNumber = 1,
    int pageSize = 10,
    string sortBy = "Id",
    bool sortDescending = false,
    Dictionary<string, object> filtros = null
  ) where T : class;
}
```

### 2.2 Implementación

```csharp
public class GridService : IGridService
{
  private readonly IDataAdapter _dataAdapter;

  public GridService(IDataAdapter dataAdapter)
  {
    _dataAdapter = dataAdapter;
  }

  public async Task<PaginationVM<T>> PaginarAsync<T>(
    string sql,
    int pageNumber = 1,
    int pageSize = 10,
    string sortBy = "Id",
    bool sortDescending = false,
    Dictionary<string, object> filtros = null
  ) where T : class
  {
    // Validar
    if (pageNumber < 1) pageNumber = 1;
    if (pageSize < 1 || pageSize > 100) pageSize = 10;

    // Construir SQL con filtros
    var sqlConFiltros = sql;
    var parametros = new Dictionary<string, object>();

    if (filtros != null && filtros.Count > 0)
    {
      var whereConditions = new List<string>();
      foreach (var filtro in filtros)
      {
        whereConditions.Add($"{filtro.Key} = @{filtro.Key}");
        parametros[filtro.Key] = filtro.Value;
      }

      sqlConFiltros += " WHERE " + string.Join(" AND ", whereConditions);
    }

    // Ordenamiento
    var ordenamiento = sortDescending ? "DESC" : "ASC";
    sqlConFiltros += $" ORDER BY {sortBy} {ordenamiento}";

    // Contar total
    var sqlCount = $"SELECT COUNT(*) FROM ({sqlConFiltros}) AS cnt";
    var total = (int)await _dataAdapter.ExecuteScalarAsync(sqlCount, parametros);

    // Paginar
    var offset = (pageNumber - 1) * pageSize;
    var sqlPaginado = $@"
      {sqlConFiltros}
      OFFSET {offset} ROWS
      FETCH NEXT {pageSize} ROWS ONLY
    ";

    var items = await _dataAdapter.QueryAsync<T>(sqlPaginado, parametros);

    return new PaginationVM<T>
    {
      Items = items,
      PageNumber = pageNumber,
      PageSize = pageSize,
      TotalRecords = total,
      TotalPages = (int)Math.Ceiling((double)total / pageSize),
      SortBy = sortBy,
      SortDescending = sortDescending
    };
  }
}
```

### 2.3 ViewModel

```csharp
public class PaginationVM<T> where T : class
{
  public List<T> Items { get; set; } = new();
  public int PageNumber { get; set; } = 1;
  public int PageSize { get; set; } = 10;
  public int TotalRecords { get; set; }
  public int TotalPages { get; set; }
  public string SortBy { get; set; } = "Id";
  public bool SortDescending { get; set; }
  public bool HasPreviousPage => PageNumber > 1;
  public bool HasNextPage => PageNumber < TotalPages;
}
```

### 2.4 Partial View: `_Grid.cshtml`

```html
@model PaginationVM<dynamic>

<div class="grid-container">
  <table class="table table-striped table-hover">
    <thead>
      <tr>
        @foreach (var col in ViewBag.Columns) // Columns pasadas desde Controller
        {
          var isSorted = col == Model.SortBy;
          var sortAsc = isSorted && Model.SortDescending;
          var sortClass = isSorted ? (Model.SortDescending ? "sort-desc" : "sort-asc") : "";
          
          <th>
            <a href="@Url.Action("Index", new { sortBy = col, sortDescending = sortAsc })" 
               class="sort-link @sortClass">
              @col
              @if (isSorted)
              {
                <span class="sort-icon">@(Model.SortDescending ? "▼" : "▲")</span>
              }
            </a>
          </th>
        }
      </tr>
    </thead>
    <tbody>
      @foreach (var item in Model.Items)
      {
        <tr>
          @foreach (var col in ViewBag.Columns)
          {
            <td>@(item.GetType().GetProperty(col)?.GetValue(item))</td>
          }
        </tr>
      }
    </tbody>
  </table>

  <!-- Paginación -->
  <nav>
    <ul class="pagination">
      @if (Model.HasPreviousPage)
      {
        <li class="page-item">
          <a class="page-link" href="@Url.Action("Index", new { page = Model.PageNumber - 1 })">Anterior</a>
        </li>
      }

      @for (int i = Math.Max(1, Model.PageNumber - 2); i <= Math.Min(Model.TotalPages, Model.PageNumber + 2); i++)
      {
        <li class="page-item @(i == Model.PageNumber ? "active" : "")">
          <a class="page-link" href="@Url.Action("Index", new { page = i })">@i</a>
        </li>
      }

      @if (Model.HasNextPage)
      {
        <li class="page-item">
          <a class="page-link" href="@Url.Action("Index", new { page = Model.PageNumber + 1 })">Siguiente</a>
        </li>
      }
    </ul>
  </nav>
</div>
```

---

## 3️⃣ PermisosService (ya especificado en Fase 3)

Reusar del documento `MATRIZ_PERMISOS_ROLES.md` sección 5.1

---

## 4️⃣ EmailService

### 4.1 Interfaz

```csharp
public interface IEmailService
{
  Task<bool> EnviarAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true);
  Task<bool> EnviarMultipleAsync(List<string> destinatarios, string asunto, string cuerpo);
  Task<bool> EnviarConArchivosAsync(string destinatario, string asunto, string cuerpo, List<string> rutasArchivos);
}
```

### 4.2 Implementación

```csharp
public class EmailService : IEmailService
{
  private readonly IConfiguration _config;
  private readonly ILogger<EmailService> _logger;

  public EmailService(IConfiguration config, ILogger<EmailService> logger)
  {
    _config = config;
    _logger = logger;
  }

  public async Task<bool> EnviarAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true)
  {
    try
    {
      using (var cliente = new SmtpClient())
      {
        cliente.Host = _config["Email:SmtpHost"];
        cliente.Port = int.Parse(_config["Email:SmtpPort"]);
        cliente.EnableSsl = bool.Parse(_config["Email:EnableSsl"]);
        cliente.Credentials = new NetworkCredential(
          _config["Email:Username"],
          _config["Email:Password"]
        );

        var mensaje = new MailMessage()
        {
          From = new MailAddress(_config["Email:SenderEmail"], _config["Email:SenderName"]),
          Subject = asunto,
          Body = cuerpo,
          IsBodyHtml = esHtml
        };

        mensaje.To.Add(destinatario);

        await cliente.SendMailAsync(mensaje);
        _logger.LogInformation($"Email enviado a {destinatario}");
        return true;
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, $"Error enviando email a {destinatario}");
      return false;
    }
  }

  public async Task<bool> EnviarMultipleAsync(List<string> destinatarios, string asunto, string cuerpo)
  {
    foreach (var dest in destinatarios)
    {
      await EnviarAsync(dest, asunto, cuerpo);
    }
    return true;
  }

  public async Task<bool> EnviarConArchivosAsync(string destinatario, string asunto, string cuerpo, List<string> rutasArchivos)
  {
    try
    {
      using (var cliente = new SmtpClient())
      {
        cliente.Host = _config["Email:SmtpHost"];
        cliente.Port = int.Parse(_config["Email:SmtpPort"]);
        cliente.EnableSsl = bool.Parse(_config["Email:EnableSsl"]);
        cliente.Credentials = new NetworkCredential(
          _config["Email:Username"],
          _config["Email:Password"]
        );

        var mensaje = new MailMessage()
        {
          From = new MailAddress(_config["Email:SenderEmail"], _config["Email:SenderName"]),
          Subject = asunto,
          Body = cuerpo,
          IsBodyHtml = true
        };

        mensaje.To.Add(destinatario);

        foreach (var rutaArchivo in rutasArchivos)
        {
          if (File.Exists(rutaArchivo))
          {
            mensaje.Attachments.Add(new Attachment(rutaArchivo));
          }
        }

        await cliente.SendMailAsync(mensaje);
        return true;
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, $"Error enviando email con archivos a {destinatario}");
      return false;
    }
  }
}
```

---

## 5️⃣ ViewModels Base Compartidos

```csharp
/// <summary>
/// Base para todos los ViewModels
/// </summary>
public class BaseVM
{
  public long Id { get; set; }
  public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
  public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
  public long UsuarioCreacion { get; set; }
  public long UsuarioModificacion { get; set; }
  public bool Activo { get; set; } = true;
}

/// <summary>
/// Respuesta estándar de APIs/acciones
/// </summary>
public class ResultVM
{
  public bool Exitoso { get; set; } = true;
  public string Mensaje { get; set; }
  public List<ErrorVM> Errores { get; set; } = new();
  public object Datos { get; set; }

  public static ResultVM Exito(string mensaje = "Operación exitosa", object datos = null)
  {
    return new ResultVM { Exitoso = true, Mensaje = mensaje, Datos = datos };
  }

  public static ResultVM Error(string mensaje, List<ErrorVM> errores = null)
  {
    return new ResultVM { Exitoso = false, Mensaje = mensaje, Errores = errores ?? new() };
  }
}

public class ErrorVM
{
  public string Campo { get; set; }
  public string Mensaje { get; set; }
}

/// <summary>
/// Filtros comunes para búsquedas
/// </summary>
public class FiltrosVM
{
  public string Busqueda { get; set; }
  public DateTime? FechaDesde { get; set; }
  public DateTime? FechaHasta { get; set; }
  public int Estado { get; set; } = -1; // -1 = todos
  public int PageNumber { get; set; } = 1;
  public int PageSize { get; set; } = 10;
  public string SortBy { get; set; } = "FechaCreacion";
  public bool SortDescending { get; set; } = true;
}
```

---

## 6️⃣ Partials Compartidos

### 6.1 `_Upload.cshtml`

```html
@model ArchivoVM

<div class="upload-section">
  <form method="post" enctype="multipart/form-data" id="uploadForm">
    <div class="form-group">
      <label for="file">Selecciona archivo:</label>
      <input type="file" id="file" name="file" class="form-control" accept=".pdf,.doc,.docx,.xls,.xlsx,.csv" />
      <small class="form-text text-muted">
        Archivos permitidos: PDF, Word, Excel, CSV (máx 20 MB)
      </small>
    </div>
    <button type="submit" class="btn btn-primary">Subir</button>
  </form>

  <div id="uploadProgress" class="progress" style="display:none;">
    <div class="progress-bar" role="progressbar" style="width: 0%"></div>
  </div>

  <div id="uploadResults"></div>
</div>

<script>
  $("#uploadForm").on("submit", function(e) {
    e.preventDefault();
    
    var file = $("#file")[0].files[0];
    var formData = new FormData();
    formData.append("file", file);

    $.ajax({
      url: "@Url.Action("SubirDocumento")",
      type: "POST",
      data: formData,
      processData: false,
      contentType: false,
      xhr: function() {
        var xhr = $.ajaxSettings.xhr();
        xhr.upload.addEventListener("progress", function(e) {
          if (e.lengthComputable) {
            var percentComplete = (e.loaded / e.total) * 100;
            $("#uploadProgress .progress-bar").css("width", percentComplete + "%");
          }
        });
        return xhr;
      },
      success: function(data) {
        $("#uploadResults").html(`
          <div class="alert alert-success">
            Archivo subido: ${data.nombreArchivo}
            <a href="${data.rutaRelativa}" target="_blank">Descargar</a>
          </div>
        `);
      },
      error: function() {
        $("#uploadResults").html(`<div class="alert alert-danger">Error al subir archivo</div>`);
      }
    });
  });
</script>
```

### 6.2 `_Confirm.cshtml`

```html
@{
  var titulo = ViewBag.Titulo ?? "Confirmación";
  var mensaje = ViewBag.Mensaje ?? "¿Está seguro?";
  var accion = ViewBag.Accion ?? "Confirmar";
}

<div class="modal fade" id="confirmModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">@titulo</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        @mensaje
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
        <button type="button" class="btn btn-danger" id="confirmBtn">@accion</button>
      </div>
    </div>
  </div>
</div>

<script>
  function mostrarConfirmacion(callback) {
    $("#confirmBtn").off("click").on("click", function() {
      callback();
      new bootstrap.Modal(document.getElementById("confirmModal")).hide();
    });

    new bootstrap.Modal(document.getElementById("confirmModal")).show();
  }
</script>
```

---

## 7️⃣ Inyección de Dependencias (Program.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IGridService, GridService>();
builder.Services.AddScoped<IPermisosService, PermisosService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();
builder.Services.AddScoped<IDataAdapter, DataAdapter>();
builder.Services.AddScoped<GrafoAciclicoService>();

// DbContexts
builder.Services.AddDbContext<PY_Context>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("MatrixDB")));
builder.Services.AddDbContext<CORE_Context>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("MatrixDB")));

// Controllers & Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
```

---

## 8️⃣ Reusabilidad Matrix

### 8.1 Módulos que DEBEN usar UploadService

| Módulo | Caso Uso |
| --- | --- |
| **PY_Proyectos** | Subir Brief, Especificaciones, Documento Proyecto |
| **CORE** | Documentos por Tarea, Evidencias de Cierre |
| **OP** | Reportes Muestra, Archivos Metodología |
| **CU_Cuentas** | Brief, Propuesta, Documentos Cliente |
| **FI_Financiero** | Recibos, Comprobantes, Facturas |

### 8.2 Módulos que DEBEN usar GridService

| Módulo | Caso Uso |
| --- | --- |
| **PY_Proyectos** | Lista Proyectos, Lista Trabajos |
| **CORE** | Tráfico Tareas, Historial Cambios Estado |
| **OP** | Catálogo Metodologías, Muestras |
| **CU_Cuentas** | Estudios, Propuestas, Clientes |
| **US_Usuarios** | Usuarios, Roles, Permisos |

### 8.3 Módulos que DEBEN usar PermisosService

| Módulo | Validación |
| --- | --- |
| **Todos** | [Authorize] attributes |
| **PY_Proyectos** | Permiso 38 (listar), 97 (crear trabajos) |
| **CORE** | Permiso ?? (gestionar tareas) |
| **OP** | Permiso ?? (configurar) |

---

## 9️⃣ Matriz de Reusabilidad

| Componente | PY | CORE | OP | CU | FI | Criticidad |
| --- | --- | --- | --- | --- | --- | --- |
| **UploadService** | ✅ | ✅ | ✅ | ✅ | ✅ | 🔴 Alta |
| **GridService** | ✅ | ✅ | ✅ | ✅ | ✅ | 🔴 Alta |
| **PermisosService** | ✅ | ✅ | ✅ | ✅ | ✅ | 🔴 Alta |
| **EmailService** | ✅ | ✅ | ✅ | ✅ | ✅ | 🟠 Media |
| **_Grid.cshtml** | ✅ | ✅ | ✅ | ✅ | ✅ | 🔴 Alta |
| **_Upload.cshtml** | ✅ | ✅ | ✅ | ✅ | ✅ | 🔴 Alta |
| **BaseVM** | ✅ | ✅ | ✅ | ✅ | ✅ | 🟠 Media |
| **PaginationVM** | ✅ | ✅ | ✅ | ✅ | ✅ | 🔴 Alta |

---

## 🔟 Próximas Acciones

- [ ] Implementar UploadService (carpetas, validaciones, auditoría)
- [ ] Crear _Grid.cshtml partial reutilizable
- [ ] Diseñar _Upload.cshtml con progress bar
- [ ] Integrar EmailService en Trabajos.Guardar() (notificaciones)
- [ ] Validar reusabilidad en cada módulo

---

**Fase 4 completada.** Listo para Fase 5: Validación de Base de Datos.
