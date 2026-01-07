using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.PY;
using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.Services.Shared;
using System.Diagnostics;

namespace MatrixNext.Web.Tests.Performance
{
    /// <summary>
    /// Pruebas de performance para dashboards y exportación Excel.
    /// Objetivo: Queries < 3 segundos, Exports < 5 segundos
    /// </summary>
    public class DashboardPerformanceTests
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DashboardPerformanceTests> _logger;

        public DashboardPerformanceTests()
        {
            var services = new ServiceCollection();
            
            // Configurar DbContext (en memoria o conexión real)
            // services.AddDbContext<MatrixDbContext>(...);
            
            // Registrar servicios
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IWorkFlowDashboardService, WorkFlowDashboardService>();
            services.AddScoped<IIndicadoresCumplimientoService, IndicadoresCumplimientoService>();
            services.AddScoped<IExportService, ExportService>();
            
            services.AddLogging(builder => builder.AddConsole());
            
            _serviceProvider = services.BuildServiceProvider();
            _logger = _serviceProvider.GetRequiredService<ILogger<DashboardPerformanceTests>>();
        }

        /// <summary>
        /// Test: Dashboard PY debe cargar en menos de 3 segundos
        /// </summary>
        public async Task<TestResult> TestDashboardPyPerformanceAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var testName = "Dashboard PY - Resumen General";

            try
            {
                var service = _serviceProvider.GetRequiredService<IDashboardService>();
                
                var resultado = await service.ObtenerResumenGeneralAsync(null);
                
                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;

                var passed = elapsed < 3000; // Meta: < 3 segundos

                return new TestResult
                {
                    TestName = testName,
                    ElapsedMs = elapsed,
                    Passed = passed && resultado.IsSuccess,
                    Message = passed 
                        ? $"✅ PASÓ: {elapsed}ms (meta: <3000ms)" 
                        : $"❌ FALLÓ: {elapsed}ms excede 3000ms"
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return new TestResult
                {
                    TestName = testName,
                    ElapsedMs = stopwatch.ElapsedMilliseconds,
                    Passed = false,
                    Message = $"❌ ERROR: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Test: Dashboard CORE WorkFlow debe cargar en menos de 3 segundos
        /// </summary>
        public async Task<TestResult> TestWorkFlowDashboardPerformanceAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var testName = "Dashboard CORE - Resumen Tareas";

            try
            {
                var service = _serviceProvider.GetRequiredService<IWorkFlowDashboardService>();
                
                var resultado = await service.ObtenerResumenGeneralAsync();
                
                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;

                var passed = elapsed < 3000;

                return new TestResult
                {
                    TestName = testName,
                    ElapsedMs = elapsed,
                    Passed = passed && resultado.IsSuccess,
                    Message = passed 
                        ? $"✅ PASÓ: {elapsed}ms (meta: <3000ms)" 
                        : $"❌ FALLÓ: {elapsed}ms excede 3000ms"
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return new TestResult
                {
                    TestName = testName,
                    ElapsedMs = stopwatch.ElapsedMilliseconds,
                    Passed = false,
                    Message = $"❌ ERROR: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Test: Exportación Excel debe completarse en menos de 5 segundos
        /// </summary>
        public async Task<TestResult> TestExcelExportPerformanceAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var testName = "Export Excel - 1000 registros";

            try
            {
                var exportService = _serviceProvider.GetRequiredService<IExportService>();
                
                // Simular datos de prueba (1000 registros)
                var testData = Enumerable.Range(1, 1000).Select(i => new
                {
                    Id = i,
                    Nombre = $"Trabajo {i}",
                    FechaCreacion = DateTime.Now.AddDays(-i),
                    Estado = i % 2 == 0 ? "Activo" : "Cerrado",
                    Monto = i * 1000.50m
                }).ToList();

                var excelBytes = await exportService.ExportarExcelAsync(
                    testData,
                    "Test_Performance",
                    "Datos Prueba",
                    "Reporte de Prueba de Performance");
                
                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;

                var passed = elapsed < 5000 && excelBytes.Length > 0;

                return new TestResult
                {
                    TestName = testName,
                    ElapsedMs = elapsed,
                    Passed = passed,
                    Message = passed 
                        ? $"✅ PASÓ: {elapsed}ms (meta: <5000ms, tamaño: {excelBytes.Length} bytes)" 
                        : $"❌ FALLÓ: {elapsed}ms excede 5000ms o archivo vacío"
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return new TestResult
                {
                    TestName = testName,
                    ElapsedMs = stopwatch.ElapsedMilliseconds,
                    Passed = false,
                    Message = $"❌ ERROR: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Test: Exportación multi-hojas debe completarse en menos de 7 segundos
        /// </summary>
        public async Task<TestResult> TestExcelMultiHojasPerformanceAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var testName = "Export Excel Multi-Hojas - 3 hojas";

            try
            {
                var exportService = _serviceProvider.GetRequiredService<IExportService>();
                
                var testData1 = Enumerable.Range(1, 500).Select(i => new { Id = i, Nombre = $"Item {i}" }).ToList();
                var testData2 = Enumerable.Range(1, 300).Select(i => new { Id = i, Codigo = $"COD{i}" }).ToList();
                var testData3 = Enumerable.Range(1, 200).Select(i => new { Id = i, Valor = i * 100 }).ToList();

                var hojas = new Dictionary<string, object>
                {
                    { "Hoja 1", testData1 },
                    { "Hoja 2", testData2 },
                    { "Hoja 3", testData3 }
                };

                var excelBytes = await exportService.ExportarExcelMultiHojasAsync(hojas, "Test_MultiHojas");
                
                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;

                var passed = elapsed < 7000 && excelBytes.Length > 0;

                return new TestResult
                {
                    TestName = testName,
                    ElapsedMs = elapsed,
                    Passed = passed,
                    Message = passed 
                        ? $"✅ PASÓ: {elapsed}ms (meta: <7000ms, tamaño: {excelBytes.Length} bytes)" 
                        : $"❌ FALLÓ: {elapsed}ms excede 7000ms"
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return new TestResult
                {
                    TestName = testName,
                    ElapsedMs = stopwatch.ElapsedMilliseconds,
                    Passed = false,
                    Message = $"❌ ERROR: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Ejecuta todos los tests de performance
        /// </summary>
        public async Task<List<TestResult>> RunAllTestsAsync()
        {
            _logger.LogInformation("=== INICIANDO TESTS DE PERFORMANCE ===");

            var results = new List<TestResult>
            {
                await TestDashboardPyPerformanceAsync(),
                await TestWorkFlowDashboardPerformanceAsync(),
                await TestExcelExportPerformanceAsync(),
                await TestExcelMultiHojasPerformanceAsync()
            };

            _logger.LogInformation("=== RESULTADOS ===");
            foreach (var result in results)
            {
                _logger.LogInformation($"{result.TestName}: {result.Message}");
            }

            var passed = results.Count(r => r.Passed);
            var total = results.Count;
            _logger.LogInformation($"\n✅ Pasados: {passed}/{total}");

            return results;
        }
    }

    /// <summary>
    /// Resultado de un test de performance
    /// </summary>
    public class TestResult
    {
        public string TestName { get; set; } = string.Empty;
        public long ElapsedMs { get; set; }
        public bool Passed { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
