namespace MatrixNext.Web.Options;

/// <summary>
/// Opciones de configuración para el servicio de Gestión Documental.
/// Contiene las rutas UNC y credenciales para acceso a documentos escaneados.
/// </summary>
public class GestionDocumentalOptions
{
    /// <summary>
    /// Nombre de la sección en appsettings.json
    /// </summary>
    public const string SectionName = "GestionDocumental";

    /// <summary>
    /// Ruta base UNC para documentos escaneados.
    /// Ejemplo: \\servidor\compartido\documentos
    /// </summary>
    public string RutaBaseUNC { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del servidor de archivos.
    /// Ejemplo: SERVIDOR-IPSOS
    /// </summary>
    public string Servidor { get; set; } = string.Empty;

    /// <summary>
    /// Usuario para acceso a rutas UNC (opcional si usa autenticación integrada)
    /// </summary>
    public string? Usuario { get; set; }

    /// <summary>
    /// Contraseña para acceso a rutas UNC (opcional si usa autenticación integrada)
    /// </summary>
    public string? Contraseña { get; set; }

    /// <summary>
    /// Timeout en segundos para validación de rutas UNC.
    /// Por defecto: 30 segundos
    /// </summary>
    public int TimeoutSegundos { get; set; } = 30;

    /// <summary>
    /// Indica si se debe validar acceso a rutas UNC al iniciar el servicio.
    /// Por defecto: false (para evitar bloqueos en desarrollo)
    /// </summary>
    public bool ValidarAccesoInicio { get; set; } = false;

    /// <summary>
    /// Lista de extensiones de archivo permitidas para documentos.
    /// Por defecto: pdf, jpg, jpeg, png, doc, docx, xls, xlsx
    /// </summary>
    public List<string> ExtensionesPermitidas { get; set; } = new()
    {
        ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx"
    };
}
