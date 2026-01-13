using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.EQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.EQ;

/// <summary>
/// Servicio para sembrar datos maestros de EasyQuote desde Excel CSV exports
/// Ejecutar manualmente o en desarrollo para poblar tablas maestras
/// </summary>
public class EqSeedService
{
    private readonly MatrixDbContext _context;
    private readonly ILogger<EqSeedService> _logger;

    public EqSeedService(MatrixDbContext context, ILogger<EqSeedService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Siembra todas las maestras de EasyQuote
    /// Verifica si ya existen datos para evitar duplicación
    /// </summary>
    public async Task<SeedResult> SeedAllMasterTablesAsync(bool force = false)
    {
        var result = new SeedResult();
        
        try
        {
            _logger.LogInformation("Iniciando seed de maestras EasyQuote...");
            
            // 1. Precios matriz (eq_param_precio)
            if (force || !await _context.EqParamPrecios.AnyAsync())
            {
                await SeedPreciosMatrizAsync();
                result.TablasSeeded.Add("eq_param_precio");
                _logger.LogInformation("✅ Seeded eq_param_precio");
            }
            else
            {
                result.TablasSkipped.Add("eq_param_precio");
                _logger.LogInformation("⏭️ Skipped eq_param_precio (ya tiene datos)");
            }

            // 2. Horas scripting/procesamiento (eq_param_script_proc)
            if (force || !await _context.EqParamScriptProcs.AnyAsync())
            {
                await SeedHorasScriptProcesoAsync();
                result.TablasSeeded.Add("eq_param_script_proc");
                _logger.LogInformation("✅ Seeded eq_param_script_proc");
            }
            else
            {
                result.TablasSkipped.Add("eq_param_script_proc");
                _logger.LogInformation("⏭️ Skipped eq_param_script_proc (ya tiene datos)");
            }

            // 3. Tarifas recursos (eq_valor_hora_ops)
            if (force || !await _context.EqValorHoraOps.AnyAsync())
            {
                await SeedTarifasRecursosAsync();
                result.TablasSeeded.Add("eq_valor_hora_ops");
                _logger.LogInformation("✅ Seeded eq_valor_hora_ops");
            }
            else
            {
                result.TablasSkipped.Add("eq_valor_hora_ops");
                _logger.LogInformation("⏭️ Skipped eq_valor_hora_ops (ya tiene datos)");
            }

            // 4. Costos insumos (eq_cost_insumos)
            if (force || !await _context.EqCostInsumos.AnyAsync())
            {
                await SeedCostosInsumosAsync();
                result.TablasSeeded.Add("eq_cost_insumos");
                _logger.LogInformation("✅ Seeded eq_cost_insumos");
            }
            else
            {
                result.TablasSkipped.Add("eq_cost_insumos");
                _logger.LogInformation("⏭️ Skipped eq_cost_insumos (ya tiene datos)");
            }

            // 5. Tarifario estadística (eq_rate_estadistica)
            if (force || !await _context.EqRateEstadisticas.AnyAsync())
            {
                await SeedRatesEstadisticaAsync();
                result.TablasSeeded.Add("eq_rate_estadistica");
                _logger.LogInformation("✅ Seeded eq_rate_estadistica");
            }
            else
            {
                result.TablasSkipped.Add("eq_rate_estadistica");
                _logger.LogInformation("⏭️ Skipped eq_rate_estadistica (ya tiene datos)");
            }

            // 6. Locaciones (eq_locaciones)
            if (force || !await _context.EqLocaciones.AnyAsync())
            {
                await SeedLocacionesAsync();
                result.TablasSeeded.Add("eq_locaciones");
                _logger.LogInformation("✅ Seeded eq_locaciones");
            }
            else
            {
                result.TablasSkipped.Add("eq_locaciones");
                _logger.LogInformation("⏭️ Skipped eq_locaciones (ya tiene datos)");
            }

            result.Success = true;
            _logger.LogInformation($"✅ Seed completado: {result.TablasSeeded.Count} tablas seeded, {result.TablasSkipped.Count} skipped");
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = ex.Message;
            _logger.LogError(ex, "❌ Error durante seed de maestras EasyQuote");
        }

        return result;
    }

    private async Task SeedPreciosMatrizAsync()
    {
        var precios = EqSeedData.GetPreciosMatriz();
        _context.EqParamPrecios.AddRange(precios);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Inserted {precios.Count} precios (3 metodologías × 11 penetraciones × 12 duraciones)");
    }

    private async Task SeedHorasScriptProcesoAsync()
    {
        var horas = EqSeedData.GetHorasScriptProceso();
        _context.EqParamScriptProcs.AddRange(horas);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Inserted {horas.Count} registros de horas (12 duraciones)");
    }

    private async Task SeedTarifasRecursosAsync()
    {
        var tarifas = EqSeedData.GetTarifasRecursos();
        _context.EqValorHoraOps.AddRange(tarifas);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Inserted {tarifas.Count} tarifas (L1-L8)");
    }

    private async Task SeedCostosInsumosAsync()
    {
        var costos = EqSeedData.GetCostosInsumos();
        _context.EqCostInsumos.AddRange(costos);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Inserted {costos.Count} costos de insumos (NSE 1-6)");
    }

    private async Task SeedRatesEstadisticaAsync()
    {
        var rates = EqSeedData.GetRatesEstadistica();
        _context.EqRateEstadisticas.AddRange(rates);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Inserted {rates.Count} rates estadística (procesos especiales, codificación, programación)");
    }

    private async Task SeedLocacionesAsync()
    {
        var locaciones = EqSeedData.GetLocaciones();
        _context.EqLocaciones.AddRange(locaciones);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Inserted {locaciones.Count} locaciones (ciudades principales)");
    }

    /// <summary>
    /// Verifica si las maestras ya tienen datos
    /// </summary>
    public async Task<MasterDataStatus> CheckMasterDataStatusAsync()
    {
        return new MasterDataStatus
        {
            PreciosCount = await _context.EqParamPrecios.CountAsync(),
            HorasCount = await _context.EqParamScriptProcs.CountAsync(),
            TarifasCount = await _context.EqValorHoraOps.CountAsync(),
            CostosCount = await _context.EqCostInsumos.CountAsync(),
            RatesCount = await _context.EqRateEstadisticas.CountAsync(),
            LocacionesCount = await _context.EqLocaciones.CountAsync()
        };
    }

    /// <summary>
    /// Limpia todas las maestras (SOLO DESARROLLO)
    /// </summary>
    public async Task ClearAllMasterTablesAsync()
    {
        _logger.LogWarning("⚠️ Limpiando TODAS las maestras EasyQuote...");
        
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM eq_locaciones");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM eq_rate_estadistica");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM eq_cost_insumos");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM eq_valor_hora_ops");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM eq_param_script_proc");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM eq_param_precio");
        
        _logger.LogWarning("✅ Maestras limpiadas");
    }
}

/// <summary>
/// Resultado del proceso de seeding
/// </summary>
public class SeedResult
{
    public bool Success { get; set; }
    public List<string> TablasSeeded { get; set; } = new();
    public List<string> TablasSkipped { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Estado actual de las maestras
/// </summary>
public class MasterDataStatus
{
    public int PreciosCount { get; set; }
    public int HorasCount { get; set; }
    public int TarifasCount { get; set; }
    public int CostosCount { get; set; }
    public int RatesCount { get; set; }
    public int LocacionesCount { get; set; }

    public bool AllTablesPopulated => 
        PreciosCount > 0 && HorasCount > 0 && TarifasCount > 0 && 
        CostosCount > 0 && RatesCount > 0 && LocacionesCount > 0;

    public override string ToString()
    {
        return $"eq_param_precio: {PreciosCount}, eq_param_script_proc: {HorasCount}, " +
               $"eq_valor_hora_ops: {TarifasCount}, eq_cost_insumos: {CostosCount}, " +
               $"eq_rate_estadistica: {RatesCount}, eq_locaciones: {LocacionesCount}";
    }
}
