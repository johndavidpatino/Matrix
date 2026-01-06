using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Web.Areas.EQ.Controllers
{
    [Area("EQ")]
    [Route("EQ/[controller]")]
    public class MaestrasAdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MaestrasAdminController> _logger;

        public MaestrasAdminController(IConfiguration configuration, ILogger<MaestrasAdminController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // GET: EQ/MaestrasAdmin/Index
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Tablas = new List<string>
            {
                "eq_param_penetracion",
                "eq_param_metodologia",
                "eq_param_precio",
                "eq_param_script_proc",
                "eq_valor_hora_ops",
                "eq_param_cati",
                "eq_param_online",
                "eq_param_factores",
                "eq_rate_horas"
            };
            return View();
        }

        // GET: EQ/MaestrasAdmin/Tabla/{nombreTabla}
        [HttpGet("Tabla/{nombreTabla}")]
        public async Task<IActionResult> Tabla(string nombreTabla)
        {
            try
            {
                // Validar nombre tabla para prevenir SQL injection
                var tablasPermitidas = new[] 
                { 
                    "eq_param_penetracion", "eq_param_metodologia", "eq_param_precio",
                    "eq_param_script_proc", "eq_valor_hora_ops", "eq_param_cati",
                    "eq_param_online", "eq_param_factores", "eq_rate_horas"
                };

                if (!tablasPermitidas.Contains(nombreTabla))
                    return BadRequest("Tabla no válida");

                var registros = await ObtenerRegistrosMaestra(nombreTabla);
                ViewBag.NombreTabla = nombreTabla;
                ViewBag.Registros = registros;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener maestros: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // POST: EQ/MaestrasAdmin/Desactivar
        [HttpPost("Desactivar")]
        public async Task<IActionResult> Desactivar(string tabla, int id, DateTime fechaFin)
        {
            try
            {
                // Validar fecha
                if (fechaFin <= DateTime.Now.Date)
                    return BadRequest("La fecha de vigencia debe ser futura");

                // Validar tabla
                var tablasPermitidas = new[] 
                { 
                    "eq_param_penetracion", "eq_param_metodologia", "eq_param_precio",
                    "eq_param_script_proc", "eq_valor_hora_ops", "eq_param_cati",
                    "eq_param_online", "eq_param_factores", "eq_rate_horas"
                };

                if (!tablasPermitidas.Contains(tabla))
                    return BadRequest("Tabla no válida");

                using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    using (var cmd = new SqlCommand("sp_eq_desactivar_maestro", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tabla_nombre", tabla);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@fecha_fin", fechaFin.Date);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return Ok(new { success = true, message = $"Registro desactivado efectivo {fechaFin:yyyy-MM-dd}" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al desactivar: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // GET: EQ/MaestrasAdmin/Versiones/{tabla}/{id}
        [HttpGet("Versiones/{tabla}/{id}")]
        public async Task<IActionResult> Versiones(string tabla, int id)
        {
            try
            {
                var tablasPermitidas = new[] 
                { 
                    "eq_param_penetracion", "eq_param_metodologia", "eq_param_precio",
                    "eq_param_script_proc", "eq_valor_hora_ops", "eq_param_cati",
                    "eq_param_online", "eq_param_factores", "eq_rate_horas"
                };

                if (!tablasPermitidas.Contains(tabla))
                    return BadRequest("Tabla no válida");

                using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();

                    using (var cmd = new SqlCommand("sp_eq_obtener_versiones", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tabla_nombre", tabla);
                        cmd.Parameters.AddWithValue("@id", id);

                        var dt = new DataTable();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            dt.Load(reader);
                        }

                        return Json(new { success = true, versiones = dt });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener versiones: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // GET: EQ/MaestrasAdmin/Activos
        [HttpGet("Activos")]
        public async Task<IActionResult> Activos(string tabla)
        {
            try
            {
                var tablasPermitidas = new[] 
                { 
                    "eq_param_penetracion", "eq_param_metodologia", "eq_param_precio",
                    "eq_param_script_proc", "eq_valor_hora_ops", "eq_param_cati",
                    "eq_param_online", "eq_param_factores", "eq_rate_horas"
                };

                if (string.IsNullOrEmpty(tabla) || !tablasPermitidas.Contains(tabla))
                    return BadRequest("Tabla no válida");

                var registros = await ObtenerRegistrosVigentes(tabla);
                return Json(new { success = true, registros = registros });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // Helper: Obtener registros con versionado
        private async Task<List<Dictionary<string, object>>> ObtenerRegistrosMaestra(string tabla)
        {
            var registros = new List<Dictionary<string, object>>();

            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await conn.OpenAsync();

                // Query dinámica segura
                string query = $@"
                    SELECT * FROM dbo.{tabla}
                    ORDER BY vigente_desde DESC
                ";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandTimeout = 30;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            registros.Add(row);
                        }
                    }
                }
            }

            return registros;
        }

        // Helper: Obtener solo registros vigentes
        private async Task<List<Dictionary<string, object>>> ObtenerRegistrosVigentes(string tabla)
        {
            var registros = new List<Dictionary<string, object>>();

            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await conn.OpenAsync();

                string query = $@"
                    SELECT * FROM dbo.{tabla}
                    WHERE vigente_desde <= CAST(GETDATE() AS DATE)
                      AND (vigente_hasta IS NULL OR vigente_hasta > CAST(GETDATE() AS DATE))
                    ORDER BY vigente_desde DESC
                ";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandTimeout = 30;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            registros.Add(row);
                        }
                    }
                }
            }

            return registros;
        }
    }
}
