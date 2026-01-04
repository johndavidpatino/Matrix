# Análisis Detallado - Módulo TH_TalentoHumano: Gestión de Ausencias

## Descripción General

El módulo de **Gestión de Ausencias** es un subsistema transversal que maneja:
- Solicitudes de **vacaciones, permisos, licencias** (empleados)
- Solicitudes de **incapacidades médicas** (enfermedad, accidente, maternidad)
- **Aprobación/Rechazo** de solicitudes (RRHH, Coordinadores)
- **Reporte y control** de ausencias (RRHH)
- **Visualización de calendario** de ausencias del equipo (Coordinadores/Jefes)

---

## Flujo de Negocio

### 1. Solicitud de Ausencia (Empleado)

```
Empleado abre SolicitudAusencia.aspx
    ↓
Selecciona tipo: Vacación (1) / Permiso (2) / Licencia sin remuneración (3) / Incapacidad (4-8)
    ↓
Ingresa fechas: FechaInicio, FechaFin
    ↓
Sistema calcula automáticamente:
  - Días Calendario (total de días en rango)
  - Días Laborales (considera fines de semana y tipo de salario del empleado)
    ↓
Selecciona aprobador (generalmente su jefe)
    ↓
Agrega observaciones (opcional)
    ↓
Valida:
  - Rango de fechas válido (inicio <= fin)
  - Sin solapamiento con solicitudes previas (excepto ciertos tipos)
  - Días disponibles (si es vacación)
    ↓
Si VÁLIDO:
  - Crea registro con Estado = 1 (Radicada)
  - Envía email al aprobador
  - Muestra confirmación
    ↓
Si INVÁLIDO:
  - Muestra mensaje de error
  - Permite corregir y reintentar
```

### 2. Aprobación (RRHH / Coordinador)

```
RRHH abre GestionAusenciaRRHH.aspx → Panel "Aprobaciones"
    ↓
Filtra por tipo de solicitud (opcional)
    ↓
Visualiza Grid con solicitudes en Estado = 5 (Pendiente Aprobación)
    ↓
Revisa información:
  - Empleado
  - Tipo
  - Fechas
  - Días
  - Observaciones
    ↓
Elige acción:
  A) APROBAR:
     - Estado → 20 (Aprobada)
     - Registra: FechaAprobacion, VoBo1, FechaVoBo1
     - Si es Vacación (Tipo = 1): ejecuta CausarVacaciones (descuenta saldo)
     - Envía email de confirmación
  B) RECHAZAR:
     - Estado → 10 (Rechazada)
     - Registra: FechaAprobacion, VoBo1, FechaVoBo1
     - Envía email con motivo rechazo
```

### 3. Seguimiento (Empleado)

```
Empleado abre SolicitudAusencia.aspx → Panel "Historial"
    ↓
Visualiza Grid con todas sus solicitudes (historiales, pendientes, aprobadas, rechazadas)
    ↓
Abre SolicitudAusencia.aspx → Panel "Beneficios Pendientes"
    ↓
Ve días disponibles de vacación/permisos sin usar
```

### 4. Visualización de Ausencias del Equipo (Coordinador)

```
Coordinador abre AusenciasEquipo.aspx
    ↓
Sistema valida si tiene subordinados asignados
    ↓
Si SÍ tiene:
  - Muestra lista de subordinados
  - WebMethod: getAusenciasPersonas(jefeId, search)
  - Permite seleccionar rango de fechas
  - WebMethod: getAusenciasEquipo(jefeId, fInicio, fFin)
  - Muestra calendario/timeline de ausencias
    ↓
Si NO tiene:
  - Redirige a SolicitudAusencia.aspx
    ↓
Puede:
  - Agregar subordinado: addAusenciasSubordinado(jefeId, empleadoId)
  - Remover subordinado: removeAusenciasSubordinado(subordinadoId)
  - Ver beneficios pendientes: getBeneficiosPendientes(empleadoId)
```

---

## Entidades y Modelos

### DTO/ViewModel Necesarios

```csharp
// Solicitud de Ausencia
public class AusenciaViewModel
{
    public long Id { get; set; }
    public long IdEmpleado { get; set; }
    public string NombreEmpleado { get; set; }
    public int Tipo { get; set; }
    public string TipoNombre { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int DiasCalendario { get; set; }
    public int DiasLaborales { get; set; }
    public int Estado { get; set; }
    public string EstadoNombre { get; set; }
    public string Observaciones { get; set; }
    public long AprobadoPor { get; set; }
    public string NombreAprobador { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    public DateTime FechaRegistro { get; set; }
    public long RegistradoPor { get; set; }
}

// Incapacidad (extiende Ausencia)
public class IncapacidadViewModel : AusenciaViewModel
{
    public string EntidadConsulta { get; set; }
    public string IPSPrestadora { get; set; }
    public string NoRegistroMedico { get; set; }
    public int TipoIncapacidad { get; set; }
    public string TipoIncapacidadNombre { get; set; }
    public string ClaseAusencia { get; set; }
    public bool EsSOAT { get; set; }
    public DateTime? FechaAccidenteTrabajo { get; set; }
    public string DXAsociado { get; set; }
    public string CodigoCIE { get; set; }
    public string CategoriaDX { get; set; }
    public string Comentarios { get; set; }
    public byte[]? DocumentoIncapacidad { get; set; } // Archivo PDF/imagen
}

// Formulario de solicitud
public class SolicitudAusenciaFormViewModel
{
    public int TipoAusencia { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Observaciones { get; set; }
    public long AprobadorId { get; set; }
    // Para incapacidad
    public int? TipoIncapacidad { get; set; }
    public string? EntidadConsulta { get; set; }
    public string? NoRegistroMedico { get; set; }
    public IFormFile? DocumentoIncapacidad { get; set; }
}

// Beneficios pendientes
public class BeneficioPendienteViewModel
{
    public long IdEmpleado { get; set; }
    public string NombreEmpleado { get; set; }
    public int TipoBeneficio { get; set; }
    public string TipoBeneficioNombre { get; set; }
    public int DiasDisponibles { get; set; }
    public int DiasPendientes { get; set; }
    public DateTime? UltimoPeriodoCausado { get; set; }
}

// Reporte de ausencias
public class ReporteAusenciaViewModel
{
    public string Identificacion { get; set; }
    public string NombreEmpleado { get; set; }
    public string AreaSL { get; set; }
    public DateTime FechaIngreso { get; set; }
    public string TipoAusencia { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int DiasCalendario { get; set; }
    public int DiasLaborales { get; set; }
    public string Estado { get; set; }
}

// Ausencias del equipo
public class AusenciaEquipoViewModel
{
    public long IdEmpleado { get; set; }
    public string NombreEmpleado { get; set; }
    public string Cargo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string TipoAusencia { get; set; }
    public int DiasLaborales { get; set; }
}
```

---

## Tablas SQL

### TH_SolicitudAusencia
```sql
CREATE TABLE TH_SolicitudAusencia (
    id BIGINT PRIMARY KEY IDENTITY,
    idEmpleado BIGINT NOT NULL FK → TH_Empleados,
    Tipo TINYINT NOT NULL FK → TH_TipoSolicitudAusencia,
    FInicio DATETIME NOT NULL,
    FFin DATETIME NOT NULL,
    DiasCalendario INT NOT NULL,
    DiasLaborales INT NOT NULL,
    Estado TINYINT NOT NULL (1=Radicada, 5=Pendiente, 20=Aprobada, 10=Rechazada),
    ObservacionesSolicitud NVARCHAR(MAX),
    AprobadoPor BIGINT FK → TH_Empleados,
    FechaAprobacion DATETIME NULL,
    VoBo1 BIGINT NULL FK → TH_Empleados,
    FechaVoBo1 DATETIME NULL,
    FechaRegistro DATETIME NOT NULL,
    RegistradoPor BIGINT NOT NULL FK → TH_Empleados,
    Activo BIT DEFAULT 1
);
```

### TH_Ausencia_Incapacidades
```sql
CREATE TABLE TH_Ausencia_Incapacidades (
    id BIGINT PRIMARY KEY IDENTITY,
    idSolicitudAusencia BIGINT NOT NULL FK → TH_SolicitudAusencia,
    EntidadConsulta INT FK → TH_EntidadConsulta,
    IPSPrestadora NVARCHAR(150),
    NoRegistroMedico NVARCHAR(50),
    TipoIncapacidad INT FK → TH_TipoIncapacidad,
    ClaseAusencia NVARCHAR(50),
    EsSOAT BIT,
    FechaAccidenteTrabajo DATETIME NULL,
    DXAsociado NVARCHAR(200),
    CodigoCIE NVARCHAR(10),
    CategoriaDX NVARCHAR(50),
    Comentarios NVARCHAR(MAX),
    DocumentoIncapacidad VARBINARY(MAX) NULL
);
```

### Catálogos Necesarios
- `TH_TipoSolicitudAusencia` (Vacaciones, Permiso, Licencia, Incapacidad, etc.)
- `TH_TipoIncapacidad` (Enfermedad general, Accidente trabajo, Maternidad, etc.)
- `TH_EntidadConsulta` (EPS/IPS)
- `TH_EstadoAusencia` (Radicada, Pendiente, Aprobada, Rechazada)

---

## Servicios y Adapters (Dapper)

### AusenciaDataAdapter.cs

```csharp
public class AusenciaDataAdapter
{
    private readonly IDbConnection _connection;

    // INSERT
    public bool CrearSolicitudAusencia(long idEmpleado, int tipo, DateTime fInicio, DateTime fFin, 
        int diasCalendario, int diasLaborales, long aprobadorId, string observaciones, long registradoPor)
    {
        const string query = @"
            INSERT INTO TH_SolicitudAusencia 
            (idEmpleado, Tipo, FInicio, FFin, DiasCalendario, DiasLaborales, Estado, 
             ObservacionesSolicitud, AprobadoPor, FechaRegistro, RegistradoPor, Activo)
            VALUES (@IdEmpleado, @Tipo, @FInicio, @FFin, @DiasCalendario, @DiasLaborales, 
                    1, @Observaciones, @AprobadorId, GETDATE(), @RegistradoPor, 1)";
        // Execute...
    }

    // UPDATE
    public bool AprobarSolicitud(long idSolicitud, long aprobadorId, bool aprobada)
    {
        const string query = @"
            UPDATE TH_SolicitudAusencia
            SET Estado = @Estado, FechaAprobacion = GETDATE(), 
                VoBo1 = @AprobadorId, FechaVoBo1 = GETDATE()
            WHERE id = @Id";
        // Estado = 20 si aprobada, 10 si rechazada
    }

    // SELECT - Listado con filtros
    public List<AusenciaViewModel> ObtenerSolicitudes(long? idEmpleado = null, int? tipo = null, 
        int? estado = null, DateTime? fInicio = null, DateTime? fFin = null)
    {
        const string query = @"
            SELECT sa.id, sa.idEmpleado, e.Nombres, sa.Tipo, tta.Tipo AS TipoNombre,
                   sa.FInicio, sa.FFin, sa.DiasCalendario, sa.DiasLaborales, 
                   sa.Estado, sa.ObservacionesSolicitud, sa.AprobadoPor, 
                   (SELECT CONCAT(e2.Nombres, ' ', e2.Apellidos) FROM TH_Empleados e2 WHERE e2.id = sa.AprobadoPor) AS NombreAprobador,
                   sa.FechaAprobacion, sa.FechaRegistro
            FROM TH_SolicitudAusencia sa
            JOIN TH_Empleados e ON sa.idEmpleado = e.id
            JOIN TH_TipoSolicitudAusencia tta ON sa.Tipo = tta.id
            WHERE 1=1
                AND (@IdEmpleado IS NULL OR sa.idEmpleado = @IdEmpleado)
                AND (@Tipo IS NULL OR sa.Tipo = @Tipo)
                AND (@Estado IS NULL OR sa.Estado = @Estado)
                AND (@FInicio IS NULL OR sa.FInicio >= @FInicio)
                AND (@FFin IS NULL OR sa.FFin <= @FFin)
                AND sa.Activo = 1
            ORDER BY sa.FechaRegistro DESC";
        // Map to List<AusenciaViewModel>
    }

    // SELECT - Beneficios pendientes
    public List<BeneficioPendienteViewModel> ObtenerBeneficiosPendientes(long idEmpleado)
    {
        // Query que calcula días disponibles vs usados
    }

    // SELECT - Cálculo de días
    public (int DiasCalendario, int DiasLaborales) CalcularDias(DateTime fInicio, DateTime fFin, 
        bool esSabadoLaboral, long idEmpleado)
    {
        // Lógica de cálculo considerando tipo de salario
    }

    // SELECT - Validaciones
    public bool ValidarSolicitud(long idEmpleado, DateTime fInicio, DateTime fFin, int tipo)
    {
        // Valida solapamiento, días disponibles, etc.
    }

    // Para Incapacidades
    public bool CrearIncapacidad(long idSolicitudAusencia, IncapacidadViewModel datos)
    {
        // INSERT en TH_Ausencia_Incapacidades
    }

    // SELECT - Para AusenciasEquipo
    public List<AusenciaEquipoViewModel> ObtenerAusenciasEquipo(long idJefe, DateTime fInicio, DateTime fFin)
    {
        // Query que trae ausencias de subordinados del jefe
    }

    public List<long> ObtenerSubordinados(long idJefe)
    {
        // Query que trae IDs de empleados asignados al jefe
    }

    public void AgregarSubordinado(long idJefe, long idEmpleado)
    {
        // INSERT en tabla de relación
    }

    public void RemoverSubordinado(long idEmpleado)
    {
        // DELETE en tabla de relación
    }
}
```

### AusenciaService.cs

```csharp
public class AusenciaService
{
    private readonly AusenciaDataAdapter _adapter;
    private readonly ILogger<AusenciaService> _logger;

    // Crear solicitud
    public (bool success, string message, long id) CrearSolicitud(long idEmpleado, 
        SolicitudAusenciaFormViewModel modelo)
    {
        try
        {
            // Validar
            if (modelo.FechaInicio > modelo.FechaFin)
                return (false, "Fechas inválidas", 0);

            if (!ValidarSolicitud(idEmpleado, modelo.FechaInicio, modelo.FechaFin, modelo.TipoAusencia))
                return (false, "Solicitud no válida (solapamiento, saldo insuficiente, etc.)", 0);

            // Calcular días
            var (diasCal, diasLab) = _adapter.CalcularDias(modelo.FechaInicio, modelo.FechaFin, 
                ObtenerEsSabadoLaboral(idEmpleado), idEmpleado);

            // Crear
            var id = _adapter.CrearSolicitudAusencia(idEmpleado, modelo.TipoAusencia, 
                modelo.FechaInicio, modelo.FechaFin, diasCal, diasLab, 
                modelo.AprobadorId, modelo.Observaciones, idEmpleado);

            // Si es incapacidad, agregar datos adicionales
            if (modelo.TipoIncapacidad.HasValue)
            {
                // CrearIncapacidad(id, modelo)
            }

            return (true, "Solicitud radicada correctamente", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando solicitud");
            return (false, ex.Message, 0);
        }
    }

    // Aprobar
    public (bool success, string message) AprobarSolicitud(long idSolicitud, long idAprobador)
    {
        try
        {
            var solicitud = _adapter.ObtenerSolicitudPorId(idSolicitud);
            if (solicitud == null)
                return (false, "Solicitud no encontrada");

            // Aprobar
            _adapter.AprobarSolicitud(idSolicitud, idAprobador, true);

            // Si es vacación, "causar" (descontar del saldo)
            if (solicitud.Tipo == 1) // Vacaciones
            {
                CausarVacaciones(idSolicitud);
            }

            // Enviar email (implementation in NotificationService)

            return (true, "Solicitud aprobada");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    // Rechazar
    public (bool success, string message) RechazarSolicitud(long idSolicitud, long idAprobador)
    {
        try
        {
            _adapter.AprobarSolicitud(idSolicitud, idAprobador, false);
            // Enviar email
            return (true, "Solicitud rechazada");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    // Reportes
    public List<ReporteAusenciaViewModel> ObtenerReporteAusentismo(int año)
    {
        // Usa procedimiento almacenado o query específica
    }

    public List<ReporteAusenciaViewModel> ObtenerReporteVacaciones(int año)
    {
        // Reporte de vacaciones disfrutadas vs pendientes
    }

    // Para AusenciasEquipo
    public List<AusenciaEquipoViewModel> ObtenerAusenciasEquipo(long idJefe, DateTime fInicio, DateTime fFin)
    {
        return _adapter.ObtenerAusenciasEquipo(idJefe, fInicio, fFin);
    }

    public void AsignarSubordinado(long idJefe, long idEmpleado)
    {
        _adapter.AgregarSubordinado(idJefe, idEmpleado);
    }

    public void RemoverSubordinado(long idEmpleado)
    {
        _adapter.RemoverSubordinado(idEmpleado);
    }
}
```

---

## Controllers

### AusenciasController.cs

```csharp
[Route("Ausencias")]
[Authorize]
public class AusenciasController : Controller
{
    private readonly AusenciaService _service;

    // Index: Listado de solicitudes del usuario
    [HttpGet("")]
    public IActionResult Index()
    {
        var usuarioId = ObtenerUsuarioId();
        var (success, message, data) = _service.ObtenerSolicitudes(idEmpleado: usuarioId);
        return View(data);
    }

    // Create: Nueva solicitud
    [HttpGet("Create")]
    public IActionResult Create()
    {
        var modelo = new SolicitudAusenciaFormViewModel();
        CargarAprobadores(); // Select list
        return View(modelo);
    }

    [HttpPost("Create")]
    public IActionResult Create(SolicitudAusenciaFormViewModel modelo)
    {
        if (!ModelState.IsValid)
            return View(modelo);

        var usuarioId = ObtenerUsuarioId();
        var (success, message, id) = _service.CrearSolicitud(usuarioId, modelo);

        if (!success)
        {
            ModelState.AddModelError("", message);
            return View(modelo);
        }

        TempData["SuccessMessage"] = message;
        return RedirectToAction("Details", new { id });
    }

    // Details
    [HttpGet("Details/{id}")]
    public IActionResult Details(long id)
    {
        var (success, data) = _service.ObtenerSolicitudPorId(id);
        if (!success || data == null)
            return NotFound();
        return View(data);
    }

    // History (para empleado)
    [HttpGet("History")]
    public IActionResult History()
    {
        var usuarioId = ObtenerUsuarioId();
        var (success, message, data) = _service.ObtenerSolicitudes(idEmpleado: usuarioId);
        return View(data);
    }

    // Beneficios pendientes
    [HttpGet("BeneficiosPendientes")]
    public IActionResult BeneficiosPendientes()
    {
        var usuarioId = ObtenerUsuarioId();
        var (success, message, data) = _service.ObtenerBeneficiosPendientes(usuarioId);
        return View(data);
    }

    // Para aprobaciones (si el usuario es aprobador)
    [HttpGet("PorAprobar")]
    [Authorize(Roles = "RRHH,Coordinador")]
    public IActionResult PorAprobar()
    {
        var (success, message, data) = _service.ObtenerSolicitudesPendientes();
        return View(data);
    }

    [HttpPost("Aprobar/{id}")]
    [Authorize(Roles = "RRHH,Coordinador")]
    public IActionResult Aprobar(long id)
    {
        var usuarioId = ObtenerUsuarioId();
        var (success, message) = _service.AprobarSolicitud(id, usuarioId);

        if (!success)
            return Json(new { success = false, message });

        return Json(new { success = true, message = "Solicitud aprobada" });
    }

    [HttpPost("Rechazar/{id}")]
    [Authorize(Roles = "RRHH,Coordinador")]
    public IActionResult Rechazar(long id)
    {
        var usuarioId = ObtenerUsuarioId();
        var (success, message) = _service.RechazarSolicitud(id, usuarioId);

        if (!success)
            return Json(new { success = false, message });

        return Json(new { success = true, message = "Solicitud rechazada" });
    }
}
```

### GestionAusenciaController.cs (RRHH)

```csharp
[Route("GestionAusencia")]
[Authorize(Roles = "RRHH")]
public class GestionAusenciaController : Controller
{
    private readonly AusenciaService _service;
    private readonly IReportGenerator _reportGenerator;

    // Panel de aprobaciones
    [HttpGet("")]
    public IActionResult Index()
    {
        var (success, message, data) = _service.ObtenerSolicitudesPendientes();
        CargarFiltros();
        return View(data);
    }

    // Filtrar por tipo
    [HttpGet("Filtrar")]
    public IActionResult Filtrar(int tipo)
    {
        var (success, message, data) = _service.ObtenerSolicitudesPendientes(tipo: tipo);
        return Json(data);
    }

    // Reporte Vacaciones
    [HttpPost("ReporteVacaciones")]
    public IActionResult ReporteVacaciones(int año)
    {
        var datos = _service.ObtenerReporteVacaciones(año);
        return GenerarExcel(datos, $"ReporteVacaciones_{año}");
    }

    // Reporte Ausentismo
    [HttpPost("ReporteAusentismo")]
    public IActionResult ReporteAusentismo(int año)
    {
        var datos = _service.ObtenerReporteAusentismo(año);
        return GenerarExcel(datos, $"ReporteAusentismo_{año}");
    }

    // Reporte Incapacidades
    [HttpPost("ReporteIncapacidades")]
    public IActionResult ReporteIncapacidades(int año)
    {
        var datos = _service.ObtenerReporteIncapacidades(año);
        return GenerarExcel(datos, $"ReporteIncapacidades_{año}");
    }

    private IActionResult GenerarExcel<T>(List<T> datos, string nombreArchivo)
    {
        var excel = _reportGenerator.GenerarExcel(datos);
        return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            $"{nombreArchivo}.xlsx");
    }
}
```

### AusenciasEquipoController.cs (Coordinador)

```csharp
[Route("AusenciasEquipo")]
[Authorize]
public class AusenciasEquipoController : Controller
{
    private readonly AusenciaService _service;

    [HttpGet("")]
    public IActionResult Index()
    {
        var usuarioId = ObtenerUsuarioId();
        var subordinados = _service.ObtenerSubordinados(usuarioId);

        if (subordinados.Count == 0)
            return RedirectToAction("Create", "Ausencias"); // Sin equipo asignado

        return View();
    }

    // AJAX: Obtener ausencias del equipo
    [HttpGet("ObtenerAusenciasEquipo")]
    public IActionResult ObtenerAusenciasEquipo(DateTime fInicio, DateTime fFin)
    {
        var usuarioId = ObtenerUsuarioId();
        var data = _service.ObtenerAusenciasEquipo(usuarioId, fInicio, fFin);
        return Json(data);
    }

    // AJAX: Obtener subordinados
    [HttpGet("ObtenerSubordinados")]
    public IActionResult ObtenerSubordinados(string search = "")
    {
        var usuarioId = ObtenerUsuarioId();
        var data = _service.BuscarSubordinados(usuarioId, search);
        return Json(data);
    }

    // AJAX: Agregar subordinado
    [HttpPost("AgregarSubordinado")]
    public IActionResult AgregarSubordinado([FromBody] long empleadoId)
    {
        var usuarioId = ObtenerUsuarioId();
        try
        {
            _service.AsignarSubordinado(usuarioId, empleadoId);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    // AJAX: Remover subordinado
    [HttpPost("RemoverSubordinado")]
    public IActionResult RemoverSubordinado([FromBody] long empleadoId)
    {
        try
        {
            _service.RemoverSubordinado(empleadoId);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    // AJAX: Beneficios pendientes de un empleado
    [HttpGet("BeneficiosPendientes/{empleadoId}")]
    public IActionResult BeneficiosPendientes(long empleadoId)
    {
        var data = _service.ObtenerBeneficiosPendientes(empleadoId);
        return Json(data);
    }
}
```

---

## Vistas (Razor)

### Views/Ausencias/Index.cshtml
```html
@model List<AusenciaViewModel>

<div class="container mt-4">
    <h2>Mis Solicitudes de Ausencia</h2>

    <div class="row mb-3">
        <div class="col-md-6">
            <a href="@Url.Action("Create")" class="btn btn-primary">Nueva Solicitud</a>
            <a href="@Url.Action("History")" class="btn btn-info">Historial</a>
            <a href="@Url.Action("BeneficiosPendientes")" class="btn btn-warning">Beneficios Pendientes</a>
        </div>
    </div>

    @if (Model.Count > 0)
    {
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Tipo</th>
                    <th>Fecha Inicio</th>
                    <th>Fecha Fin</th>
                    <th>Días</th>
                    <th>Estado</th>
                    <th>Acciones</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var item in Model)
                {
                    <tr>
                        <td>@item.TipoNombre</td>
                        <td>@item.FechaInicio.ToShortDateString()</td>
                        <td>@item.FechaFin.ToShortDateString()</td>
                        <td>@item.DiasLaborales</td>
                        <td>
                            <span class="badge" style="background-color: @GetEstadoColor(item.Estado)">
                                @item.EstadoNombre
                            </span>
                        </td>
                        <td>
                            <a href="@Url.Action("Details", new { id = item.Id })" class="btn btn-sm btn-info">Ver</a>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    }
    else
    {
        <div class="alert alert-info">No tienes solicitudes registradas</div>
    }
</div>
```

### Views/Ausencias/Create.cshtml
```html
@model SolicitudAusenciaFormViewModel

<div class="container mt-4">
    <h2>Nueva Solicitud de Ausencia</h2>

    <form asp-action="Create" method="post">
        <div class="form-group">
            <label>Tipo de Ausencia</label>
            <select asp-for="TipoAusencia" class="form-control" id="ddlTipo">
                <option value="">-- Seleccione --</option>
                @foreach (var tipo in ViewBag.Tipos)
                {
                    <option value="@tipo.Id">@tipo.Nombre</option>
                }
            </select>
        </div>

        <div class="form-row">
            <div class="form-group col-md-4">
                <label>Fecha Inicio</label>
                <input asp-for="FechaInicio" type="date" class="form-control" />
            </div>
            <div class="form-group col-md-4">
                <label>Fecha Fin</label>
                <input asp-for="FechaFin" type="date" class="form-control" />
            </div>
            <div class="form-group col-md-2">
                <label>Días Calendario</label>
                <input type="text" id="diasCal" class="form-control" readonly />
            </div>
            <div class="form-group col-md-2">
                <label>Días Laborales</label>
                <input type="text" id="diasLab" class="form-control" readonly />
            </div>
        </div>

        <div class="form-group">
            <label>Aprobador</label>
            <select asp-for="AprobadorId" class="form-control">
                <option value="">-- Seleccione --</option>
            </select>
        </div>

        <div class="form-group">
            <label>Observaciones</label>
            <textarea asp-for="Observaciones" class="form-control" rows="3"></textarea>
        </div>

        <button type="submit" class="btn btn-primary">Radicar Solicitud</button>
        <a href="@Url.Action("Index")" class="btn btn-secondary">Cancelar</a>
    </form>
</div>

<script>
    // Cálculo automático de días
    document.getElementById('FechaInicio').addEventListener('change', calcularDias);
    document.getElementById('FechaFin').addEventListener('change', calcularDias);

    function calcularDias() {
        const inicio = document.getElementById('FechaInicio').value;
        const fin = document.getElementById('FechaFin').value;

        if (inicio && fin) {
            fetch(`/Ausencias/CalcularDias?fInicio=${inicio}&fFin=${fin}`)
                .then(r => r.json())
                .then(data => {
                    document.getElementById('diasCal').value = data.diasCalendario;
                    document.getElementById('diasLab').value = data.diasLaborales;
                });
        }
    }
</script>
```

---

## Puntos Clave de Implementación

### 1. **Cálculo de Días**
- Debe considerar **fines de semana** (sábado/domingo)
- Debe considerar **tipo de salario** del empleado (algunos trabajan sábados)
- Usar clase `DateOnly` o lógica personalizada

### 2. **Validaciones**
- No permitir solapamiento de solicitudes (excepto tipos específicos)
- Validar saldo de días disponibles
- Validar rango de fechas (inicio <= fin)

### 3. **Notificaciones por Email**
- Al crear solicitud: notificar al aprobador
- Al aprobar/rechazar: notificar al empleado
- Templates: `Emails/NotificacionAusencia.html`, `Emails/AprobacionAusencia.html`, etc.

### 4. **Reportes en Excel**
- Usar **ClosedXML** (ya está en el proyecto)
- Estructurar: encabezados, datos, estilos básicos
- Exportar como `.xlsx`

### 5. **Permisos**
- Empleado: crear solicitud, ver su historial, ver beneficios pendientes
- Coordinador/Jefe: ver ausencias del equipo, asignar subordinados
- RRHH: aprobar/rechazar solicitudes, ver todos los reportes

### 6. **Flujo de Estados**
```
1 (Radicada)
    ↓ (auto)
5 (Pendiente Aprobación)
    ├→ 20 (Aprobada) → Descuentar saldo si es vacación
    └→ 10 (Rechazada)
```

---

## Dependencias y Librerías

- **ClosedXML** - Generación de reportes Excel
- **Dapper** - Data access
- **FluentValidation** (opcional) - Validaciones complejas
- **MailKit** (si se implementa email nativo) o servicio existente

---

## Recomendaciones de Prioridad

1. ✅ **Modelos/ViewModels**
2. ✅ **DataAdapter** (Dapper + SQL)
3. ✅ **Service** (lógica de negocio)
4. ✅ **Controller** (endpoints)
5. ✅ **Views** (Razor)
6. ✅ **Emails** (notifications)
7. ✅ **Reportes** (Excel)
8. ✅ **Testing**

---

## Estimación de Esfuerzo

| Componente | Horas |
|-----------|-------|
| Modelos + DTOs | 2-3 |
| DataAdapter (Dapper) | 4-5 |
| Services | 3-4 |
| Controllers | 3-4 |
| Views (4 páginas) | 5-6 |
| Reportes Excel | 2-3 |
| Emails | 2 |
| Testing | 2-3 |
| **Total** | **24-30 horas** |

