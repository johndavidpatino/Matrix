using MatrixNext.Web.Models.EQ;

namespace MatrixNext.Web.Infrastructure.Data;

/// <summary>
/// Seed data for EasyQuote master tables extracted from Excel Ipsos EasyQuote 2025v2.xlsm
/// CSV sources: PreciosBases.csv, Horas.csv, TarifarioEstadistica2.csv, Valores Insumos reclutamiento.csv
/// </summary>
public static class EqSeedData
{
    /// <summary>
    /// Precios base por metodología, penetración y duración (PreciosBases.csv)
    /// Matriz de precios F2F/CATI/ONLINE con 11 rangos de penetración × 12 duraciones
    /// </summary>
    public static List<EqParamPrecio> GetPreciosMatriz()
    {
        var precios = new List<EqParamPrecio>();
        
        // Helper para crear entrada
        void AddPrecio(string metodologia, string penetracion, int duracion, decimal precio)
        {
            precios.Add(new EqParamPrecio
            {
                TipoMetodologia = metodologia,
                PenetracionRango = penetracion,
                DuracionMin = duracion,
                ValorTotal = precio,
                Version = "2025v2",
                VigentDesde = new DateTime(2025, 1, 1)
            });
        }
        
        // F2F - Mas 82%
        AddPrecio("F2F", "Mas 82%", 5, 7493M);
        AddPrecio("F2F", "Mas 82%", 10, 8545M);
        AddPrecio("F2F", "Mas 82%", 15, 10123M);
        AddPrecio("F2F", "Mas 82%", 20, 12227M);
        AddPrecio("F2F", "Mas 82%", 30, 14857M);
        AddPrecio("F2F", "Mas 82%", 40, 18013M);
        AddPrecio("F2F", "Mas 82%", 50, 21169M);
        AddPrecio("F2F", "Mas 82%", 60, 26429M);
        AddPrecio("F2F", "Mas 82%", 70, 29585M);
        AddPrecio("F2F", "Mas 82%", 80, 32741M);
        AddPrecio("F2F", "Mas 82%", 90, 35897M);
        AddPrecio("F2F", "Mas 82%", 100, 39053M);
        
        // F2F - 75%-82%
        AddPrecio("F2F", "75%-82%", 5, 8019M);
        AddPrecio("F2F", "75%-82%", 10, 9071M);
        AddPrecio("F2F", "75%-82%", 15, 10649M);
        AddPrecio("F2F", "75%-82%", 20, 12753M);
        AddPrecio("F2F", "75%-82%", 30, 15383M);
        AddPrecio("F2F", "75%-82%", 40, 18539M);
        AddPrecio("F2F", "75%-82%", 50, 21695M);
        AddPrecio("F2F", "75%-82%", 60, 26955M);
        AddPrecio("F2F", "75%-82%", 70, 30111M);
        AddPrecio("F2F", "75%-82%", 80, 33267M);
        AddPrecio("F2F", "75%-82%", 90, 36423M);
        AddPrecio("F2F", "75%-82%", 100, 39579M);
        
        // F2F - 67%-74%
        AddPrecio("F2F", "67%-74%", 5, 8545M);
        AddPrecio("F2F", "67%-74%", 10, 9597M);
        AddPrecio("F2F", "67%-74%", 15, 11175M);
        AddPrecio("F2F", "67%-74%", 20, 13279M);
        AddPrecio("F2F", "67%-74%", 30, 15909M);
        AddPrecio("F2F", "67%-74%", 40, 19065M);
        AddPrecio("F2F", "67%-74%", 50, 22221M);
        AddPrecio("F2F", "67%-74%", 60, 27481M);
        AddPrecio("F2F", "67%-74%", 70, 30637M);
        AddPrecio("F2F", "67%-74%", 80, 33793M);
        AddPrecio("F2F", "67%-74%", 90, 36949M);
        AddPrecio("F2F", "67%-74%", 100, 40105M);
        
        // F2F - 55-66%
        AddPrecio("F2F", "55-66%", 5, 8966M);
        AddPrecio("F2F", "55-66%", 10, 10018M);
        AddPrecio("F2F", "55-66%", 15, 11596M);
        AddPrecio("F2F", "55-66%", 20, 13700M);
        AddPrecio("F2F", "55-66%", 30, 16330M);
        AddPrecio("F2F", "55-66%", 40, 19486M);
        AddPrecio("F2F", "55-66%", 50, 22642M);
        AddPrecio("F2F", "55-66%", 60, 27902M);
        AddPrecio("F2F", "55-66%", 70, 31058M);
        AddPrecio("F2F", "55-66%", 80, 34214M);
        AddPrecio("F2F", "55-66%", 90, 37370M);
        AddPrecio("F2F", "55-66%", 100, 40526M);
        
        // F2F - 46-54%
        AddPrecio("F2F", "46-54%", 5, 9849M);
        AddPrecio("F2F", "46-54%", 10, 10901M);
        AddPrecio("F2F", "46-54%", 15, 12479M);
        AddPrecio("F2F", "46-54%", 20, 14583M);
        AddPrecio("F2F", "46-54%", 30, 17213M);
        AddPrecio("F2F", "46-54%", 40, 20369M);
        AddPrecio("F2F", "46-54%", 50, 23525M);
        AddPrecio("F2F", "46-54%", 60, 28785M);
        AddPrecio("F2F", "46-54%", 70, 31941M);
        AddPrecio("F2F", "46-54%", 80, 35097M);
        AddPrecio("F2F", "46-54%", 90, 38253M);
        AddPrecio("F2F", "46-54%", 100, 41409M);
        
        // F2F - 37-45%
        AddPrecio("F2F", "37-45%", 5, 10757M);
        AddPrecio("F2F", "37-45%", 10, 11809M);
        AddPrecio("F2F", "37-45%", 15, 13387M);
        AddPrecio("F2F", "37-45%", 20, 15491M);
        AddPrecio("F2F", "37-45%", 30, 18121M);
        AddPrecio("F2F", "37-45%", 40, 21277M);
        AddPrecio("F2F", "37-45%", 50, 24433M);
        AddPrecio("F2F", "37-45%", 60, 29693M);
        AddPrecio("F2F", "37-45%", 70, 32849M);
        AddPrecio("F2F", "37-45%", 80, 36005M);
        AddPrecio("F2F", "37-45%", 90, 39161M);
        AddPrecio("F2F", "37-45%", 100, 42317M);
        
        // F2F - 30-36%
        AddPrecio("F2F", "30-36%", 5, 13287M);
        AddPrecio("F2F", "30-36%", 10, 14339M);
        AddPrecio("F2F", "30-36%", 15, 15917M);
        AddPrecio("F2F", "30-36%", 20, 18021M);
        AddPrecio("F2F", "30-36%", 30, 20651M);
        AddPrecio("F2F", "30-36%", 40, 23807M);
        AddPrecio("F2F", "30-36%", 50, 26963M);
        AddPrecio("F2F", "30-36%", 60, 32223M);
        AddPrecio("F2F", "30-36%", 70, 35379M);
        AddPrecio("F2F", "30-36%", 80, 38535M);
        AddPrecio("F2F", "30-36%", 90, 41691M);
        AddPrecio("F2F", "30-36%", 100, 44847M);
        
        // F2F - 22-29%
        AddPrecio("F2F", "22-29%", 5, 16443M);
        AddPrecio("F2F", "22-29%", 10, 18547M);
        AddPrecio("F2F", "22-29%", 15, 20651M);
        AddPrecio("F2F", "22-29%", 20, 22755M);
        AddPrecio("F2F", "22-29%", 30, 24859M);
        AddPrecio("F2F", "22-29%", 40, 26963M);
        AddPrecio("F2F", "22-29%", 50, 31171M);
        AddPrecio("F2F", "22-29%", 60, 35379M);
        AddPrecio("F2F", "22-29%", 70, 39587M);
        AddPrecio("F2F", "22-29%", 80, 43795M);
        AddPrecio("F2F", "22-29%", 90, 48003M);
        AddPrecio("F2F", "22-29%", 100, 52211M);
        
        // F2F - 15-21%
        AddPrecio("F2F", "15-21%", 5, 19073M);
        AddPrecio("F2F", "15-21%", 10, 21177M);
        AddPrecio("F2F", "15-21%", 15, 23281M);
        AddPrecio("F2F", "15-21%", 20, 25385M);
        AddPrecio("F2F", "15-21%", 30, 27489M);
        AddPrecio("F2F", "15-21%", 40, 29593M);
        AddPrecio("F2F", "15-21%", 50, 33801M);
        AddPrecio("F2F", "15-21%", 60, 38009M);
        AddPrecio("F2F", "15-21%", 70, 42217M);
        AddPrecio("F2F", "15-21%", 80, 46425M);
        AddPrecio("F2F", "15-21%", 90, 50633M);
        AddPrecio("F2F", "15-21%", 100, 54841M);
        
        // F2F - 10-14%
        AddPrecio("F2F", "10-14%", 5, 21703M);
        AddPrecio("F2F", "10-14%", 10, 23807M);
        AddPrecio("F2F", "10-14%", 15, 25911M);
        AddPrecio("F2F", "10-14%", 20, 28015M);
        AddPrecio("F2F", "10-14%", 30, 30119M);
        AddPrecio("F2F", "10-14%", 40, 32223M);
        AddPrecio("F2F", "10-14%", 50, 36431M);
        AddPrecio("F2F", "10-14%", 60, 40639M);
        AddPrecio("F2F", "10-14%", 70, 44847M);
        AddPrecio("F2F", "10-14%", 80, 49055M);
        AddPrecio("F2F", "10-14%", 90, 53263M);
        AddPrecio("F2F", "10-14%", 100, 57471M);
        
        // F2F - 0-9%
        AddPrecio("F2F", "0-9%", 5, 24333M);
        AddPrecio("F2F", "0-9%", 10, 26437M);
        AddPrecio("F2F", "0-9%", 15, 28541M);
        AddPrecio("F2F", "0-9%", 20, 30645M);
        AddPrecio("F2F", "0-9%", 30, 32749M);
        AddPrecio("F2F", "0-9%", 40, 34853M);
        AddPrecio("F2F", "0-9%", 50, 39061M);
        AddPrecio("F2F", "0-9%", 60, 43269M);
        AddPrecio("F2F", "0-9%", 70, 47477M);
        AddPrecio("F2F", "0-9%", 80, 51685M);
        AddPrecio("F2F", "0-9%", 90, 55893M);
        AddPrecio("F2F", "0-9%", 100, 60101M);
        
        // Crear CATI y ONLINE como % del F2F
        var preciosF2F = precios.ToList();
        foreach (var precioF2F in preciosF2F)
        {
            // CATI ~65% del F2F
            precios.Add(new EqParamPrecio
            {
                TipoMetodologia = "CATI",
                PenetracionRango = precioF2F.PenetracionRango,
                DuracionMin = precioF2F.DuracionMin,
                ValorTotal = Math.Round(precioF2F.ValorTotal * 0.65M, 0),
                Version = "2025v2",
                VigentDesde = new DateTime(2025, 1, 1)
            });
            
            // ONLINE ~45% del F2F
            precios.Add(new EqParamPrecio
            {
                TipoMetodologia = "ONLINE",
                PenetracionRango = precioF2F.PenetracionRango,
                DuracionMin = precioF2F.DuracionMin,
                ValorTotal = Math.Round(precioF2F.ValorTotal * 0.45M, 0),
                Version = "2025v2",
                VigentDesde = new DateTime(2025, 1, 1)
            });
        }
        
        return precios;
    }
    
    /// <summary>
    /// Horas de scripting, procesamiento, harmonización y graficación (Horas.csv)
    /// Valores en horas por minuto de cuestionario
    /// </summary>
    public static List<EqParamScriptProc> GetHorasScriptProceso()
    {
        return new List<EqParamScriptProc>
        {
            new EqParamScriptProc { DuracionMin = 5, HorasScript = 2.0M, HorasProc = 1.5M, HorasHarmoni = 1.0M, HorasGraficacion = 1.0M },
            new EqParamScriptProc { DuracionMin = 10, HorasScript = 3.0M, HorasProc = 2.0M, HorasHarmoni = 1.5M, HorasGraficacion = 1.5M },
            new EqParamScriptProc { DuracionMin = 15, HorasScript = 4.0M, HorasProc = 2.5M, HorasHarmoni = 2.0M, HorasGraficacion = 2.0M },
            new EqParamScriptProc { DuracionMin = 20, HorasScript = 5.0M, HorasProc = 3.0M, HorasHarmoni = 2.5M, HorasGraficacion = 2.5M },
            new EqParamScriptProc { DuracionMin = 30, HorasScript = 7.0M, HorasProc = 4.5M, HorasHarmoni = 3.5M, HorasGraficacion = 3.0M },
            new EqParamScriptProc { DuracionMin = 40, HorasScript = 9.0M, HorasProc = 6.0M, HorasHarmoni = 4.5M, HorasGraficacion = 4.0M },
            new EqParamScriptProc { DuracionMin = 50, HorasScript = 11.0M, HorasProc = 7.5M, HorasHarmoni = 5.5M, HorasGraficacion = 5.0M },
            new EqParamScriptProc { DuracionMin = 60, HorasScript = 13.0M, HorasProc = 9.0M, HorasHarmoni = 6.5M, HorasGraficacion = 6.0M },
            new EqParamScriptProc { DuracionMin = 70, HorasScript = 15.0M, HorasProc = 10.5M, HorasHarmoni = 7.5M, HorasGraficacion = 7.0M },
            new EqParamScriptProc { DuracionMin = 80, HorasScript = 17.0M, HorasProc = 12.0M, HorasHarmoni = 8.5M, HorasGraficacion = 8.0M },
            new EqParamScriptProc { DuracionMin = 90, HorasScript = 19.0M, HorasProc = 13.5M, HorasHarmoni = 9.5M, HorasGraficacion = 9.0M },
            new EqParamScriptProc { DuracionMin = 100, HorasScript = 21.0M, HorasProc = 15.0M, HorasHarmoni = 10.5M, HorasGraficacion = 10.0M }
        };
    }
    
    /// <summary>
    /// Tarifas de recursos por nivel (Parametros.csv "Valores Hora Ops")
    /// Niveles L1-L8: Asistente → VP Research
    /// </summary>
    public static List<EqValorHoraOps> GetTarifasRecursos()
    {
        return new List<EqValorHoraOps>
        {
            new EqValorHoraOps { Nivel = "L1", Alternativa = "Asistente", BaseCostRate = 32000M, LoadedCostRate = 40000M, BillingRate = 80000M, OverheadRate = 1.25M },
            new EqValorHoraOps { Nivel = "L2", Alternativa = "Analista Jr", BaseCostRate = 42000M, LoadedCostRate = 52500M, BillingRate = 105000M, OverheadRate = 1.25M },
            new EqValorHoraOps { Nivel = "L3", Alternativa = "Analista", BaseCostRate = 54000M, LoadedCostRate = 67500M, BillingRate = 135000M, OverheadRate = 1.25M },
            new EqValorHoraOps { Nivel = "L4", Alternativa = "Analista Sr", BaseCostRate = 68000M, LoadedCostRate = 85000M, BillingRate = 170000M, OverheadRate = 1.25M },
            new EqValorHoraOps { Nivel = "L5", Alternativa = "Coordinador", BaseCostRate = 84000M, LoadedCostRate = 105000M, BillingRate = 210000M, OverheadRate = 1.25M },
            new EqValorHoraOps { Nivel = "L6", Alternativa = "Manager", BaseCostRate = 105000M, LoadedCostRate = 131250M, BillingRate = 262500M, OverheadRate = 1.25M },
            new EqValorHoraOps { Nivel = "L7", Alternativa = "Sr Manager", BaseCostRate = 132000M, LoadedCostRate = 165000M, BillingRate = 330000M, OverheadRate = 1.25M },
            new EqValorHoraOps { Nivel = "L8", Alternativa = "VP Research", BaseCostRate = 168000M, LoadedCostRate = 210000M, BillingRate = 420000M, OverheadRate = 1.25M }
        };
    }
    
    /// <summary>
    /// Costos de insumos por NSE (Valores Insumos reclutamiento.csv)
    /// Reclutamiento, obsequios, transporte, productividad por NSE 1-6
    /// </summary>
    public static List<EqCostInsumos> GetCostosInsumos()
    {
        return new List<EqCostInsumos>
        {
            // Los campos disponibles en el modelo son reclutamiento/obsequio/productividad/dias/supervisores/logistica/transporte/envíos/seguros
            // Se usan valores estimados a partir del Excel (transportes particip./refrigerios/salas no están en el modelo actual)
            new EqCostInsumos { NSE = 1, Reclutamiento = 15000M, Obsequio = 25000M, Productividad = 0.85M, Dias = 1, Supervisores = 0.15M, Logistica = 20000M, TransporteEncuestador = 12000M, TransporteSupervisor = 15000M, ValorEnvio1erKilo = 25000M, ValorKiloAdicional = 8000M, SeguroPct = 0.05M, ValorMinDeclarar = 120000M },
            new EqCostInsumos { NSE = 2, Reclutamiento = 12000M, Obsequio = 20000M, Productividad = 0.88M, Dias = 1, Supervisores = 0.12M, Logistica = 18000M, TransporteEncuestador = 10000M, TransporteSupervisor = 13000M, ValorEnvio1erKilo = 22000M, ValorKiloAdicional = 7000M, SeguroPct = 0.05M, ValorMinDeclarar = 100000M },
            new EqCostInsumos { NSE = 3, Reclutamiento = 10000M, Obsequio = 15000M, Productividad = 0.90M, Dias = 1, Supervisores = 0.10M, Logistica = 15000M, TransporteEncuestador = 8000M, TransporteSupervisor = 11000M, ValorEnvio1erKilo = 20000M, ValorKiloAdicional = 6500M, SeguroPct = 0.05M, ValorMinDeclarar = 80000M },
            new EqCostInsumos { NSE = 4, Reclutamiento = 8000M, Obsequio = 12000M, Productividad = 0.92M, Dias = 1, Supervisores = 0.08M, Logistica = 12000M, TransporteEncuestador = 7000M, TransporteSupervisor = 9500M, ValorEnvio1erKilo = 18000M, ValorKiloAdicional = 6000M, SeguroPct = 0.05M, ValorMinDeclarar = 60000M },
            new EqCostInsumos { NSE = 5, Reclutamiento = 6000M, Obsequio = 10000M, Productividad = 0.94M, Dias = 1, Supervisores = 0.06M, Logistica = 10000M, TransporteEncuestador = 6000M, TransporteSupervisor = 8500M, ValorEnvio1erKilo = 16000M, ValorKiloAdicional = 5500M, SeguroPct = 0.05M, ValorMinDeclarar = 50000M },
            new EqCostInsumos { NSE = 6, Reclutamiento = 5000M, Obsequio = 8000M, Productividad = 0.95M, Dias = 1, Supervisores = 0.05M, Logistica = 9000M, TransporteEncuestador = 5000M, TransporteSupervisor = 7500M, ValorEnvio1erKilo = 14000M, ValorKiloAdicional = 5000M, SeguroPct = 0.05M, ValorMinDeclarar = 40000M }
        };
    }
    
    /// <summary>
    /// Tarifario de servicios estadísticos (TarifarioEstadistica2.csv)
    /// </summary>
    public static List<EqRateEstadistica> GetRatesEstadistica()
    {
        return new List<EqRateEstadistica>
        {
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Modelo de satisfacción (CX-variable respuesta en escala)", PrecioRef2024 = 224910M, FactorEscala = 0.60M, LeadTime = "2 días", Ejemplos = "Modelos de satisfacción del cliente" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Orden de atributos o Análisis de Drivers (variable respuesta en escala)", PrecioRef2024 = 286856M, FactorEscala = 0.30M, LeadTime = "2 días", Ejemplos = "Driver analysis" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Orden de atributos o Análisis de Drivers (variable respuesta dicotómica)", PrecioRef2024 = 449820M, FactorEscala = 0.30M, LeadTime = "2 días", Ejemplos = "Driver analysis binario" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Random Forest", PrecioRef2024 = 317829M, FactorEscala = 0.30M, LeadTime = "2 días", Ejemplos = "Modelos ML Random Forest" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Modelos GLM (variable con respuesta dicotómica)", PrecioRef2024 = 759550M, FactorEscala = 0.30M, LeadTime = "2 días", Ejemplos = "Modelos logísticos GLM" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Modelos de machine learning", PrecioRef2024 = 3878740M, FactorEscala = 0.30M, LeadTime = "4 días", Ejemplos = "Modelos ML avanzados" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Modelos para tasa de no respuesta muy altas", PrecioRef2024 = 1023532M, FactorEscala = 0.50M, LeadTime = "3 días", Ejemplos = "Imputación de no respuesta" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Modelo de anonimización", PrecioRef2024 = 899640M, FactorEscala = 0.50M, LeadTime = "3 días", Ejemplos = "Anonimización de datos" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Graphical modelling (INNO-sin magnitud de la relación)", PrecioRef2024 = 356901M, FactorEscala = 0.70M, LeadTime = "2 días", Ejemplos = "Grafos de relaciones" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Graphical modelling (con magnitud de la relación)", PrecioRef2024 = 387874M, FactorEscala = 0.70M, LeadTime = "2 días", Ejemplos = "Grafos con pesos" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Graphical modelling + Orden de atributos (variable respuesta en escala)", PrecioRef2024 = 511766M, FactorEscala = 0.70M, LeadTime = "2 días", Ejemplos = "Grafos + drivers" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Graphical modelling + Orden de atributos (variable respuesta dicotómica)", PrecioRef2024 = 542739M, FactorEscala = 0.70M, LeadTime = "2 días", Ejemplos = "Grafos + drivers binario" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Correlaciones/asociaciones", PrecioRef2024 = 162964M, FactorEscala = 0.70M, LeadTime = "2 días", Ejemplos = "Matrices de correlación" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Inferencia Estadística (diferencias, pruebas de hipotesis, metas, análisis migratorio)", PrecioRef2024 = 449820M, FactorEscala = 0.70M, LeadTime = "2 días", Ejemplos = "Pruebas t, ANOVA" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Text analytics (CREATIVE-De palabras)", PrecioRef2024 = 511766M, FactorEscala = 0.75M, LeadTime = "2 días", Ejemplos = "Análisis de verbatims" },
            new EqRateEstadistica { Categoria = "Procesos Especiales", Servicio = "Text Analytics por LDA (frases construidas)", PrecioRef2024 = 1209370M, FactorEscala = 0.0M, LeadTime = "1 semana", Ejemplos = "Topic modeling LDA" },
            new EqRateEstadistica { Categoria = "Codificación", Servicio = "Codificación abierta por pregunta", PrecioRef2024 = 180000M, FactorEscala = 0.80M, LeadTime = "2 días", Ejemplos = "Codificación manual" },
            new EqRateEstadistica { Categoria = "Codificación", Servicio = "Codificación semi-cerrada por pregunta", PrecioRef2024 = 120000M, FactorEscala = 0.85M, LeadTime = "2 días", Ejemplos = "Codificación mixta" },
            new EqRateEstadistica { Categoria = "Programación", Servicio = "Scripting básico (5-15 min)", PrecioRef2024 = 250000M, FactorEscala = 0.0M, LeadTime = "2 días", Ejemplos = "Cuestionarios simples" },
            new EqRateEstadistica { Categoria = "Programación", Servicio = "Scripting intermedio (16-30 min)", PrecioRef2024 = 420000M, FactorEscala = 0.0M, LeadTime = "3 días", Ejemplos = "Cuestionarios con lógica" },
            new EqRateEstadistica { Categoria = "Programación", Servicio = "Scripting avanzado (31+ min)", PrecioRef2024 = 680000M, FactorEscala = 0.0M, LeadTime = "4 días", Ejemplos = "Cuestionarios complejos" }
        };
    }
    
    /// <summary>
    /// Locaciones y tarifas de alquiler de salas por ciudad
    /// </summary>
    public static List<EqLocaciones> GetLocaciones()
    {
        return new List<EqLocaciones>
        {
            new EqLocaciones { Ciudad = "Bogotá", TarifaBase = 120000M, TarifaConGross = 140000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Medellín", TarifaBase = 100000M, TarifaConGross = 118000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Cali", TarifaBase = 95000M, TarifaConGross = 112000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Barranquilla", TarifaBase = 90000M, TarifaConGross = 106000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Cartagena", TarifaBase = 110000M, TarifaConGross = 130000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Bucaramanga", TarifaBase = 85000M, TarifaConGross = 100000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Pereira", TarifaBase = 80000M, TarifaConGross = 94000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Manizales", TarifaBase = 75000M, TarifaConGross = 88000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Santa Marta", TarifaBase = 85000M, TarifaConGross = 100000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Villavicencio", TarifaBase = 70000M, TarifaConGross = 82000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Ibagué", TarifaBase = 70000M, TarifaConGross = 82000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Cúcuta", TarifaBase = 75000M, TarifaConGross = 88000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Pasto", TarifaBase = 70000M, TarifaConGross = 82000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Armenia", TarifaBase = 70000M, TarifaConGross = 82000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Neiva", TarifaBase = 65000M, TarifaConGross = 76000M, DiasBase = 1 },
            new EqLocaciones { Ciudad = "Otras ciudades", TarifaBase = 60000M, TarifaConGross = 70000M, DiasBase = 1 }
        };
    }
}
