using System.Net;

namespace MatrixNext.Web.Middleware;

/// <summary>
/// Middleware para capturar y manejar excepciones globalmente
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "text/html";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorId = Guid.NewGuid().ToString();
        _logger.LogError(exception, "ErrorId: {ErrorId} - {Message}", errorId, exception.Message);

        // En desarrollo, mostrar detalles del error
        if (_env.IsDevelopment())
        {
            var errorHtml = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Error - Matrix</title>
    <link href='/assets/libs/bootstrap/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body>
    <div class='container mt-5'>
        <div class='alert alert-danger'>
            <h4 class='alert-heading'>Error Interno del Servidor</h4>
            <p><strong>ErrorId:</strong> {errorId}</p>
            <p><strong>Mensaje:</strong> {exception.Message}</p>
            <hr>
            <pre>{exception.StackTrace}</pre>
        </div>
    </div>
</body>
</html>";
            await context.Response.WriteAsync(errorHtml);
        }
        else
        {
            // En producción, redirigir a página de error genérica
            context.Response.Redirect("/Home/Error");
        }
    }
}
