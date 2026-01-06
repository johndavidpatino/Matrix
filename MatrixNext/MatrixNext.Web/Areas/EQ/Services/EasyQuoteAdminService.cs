
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using MatrixNext.Web.Areas.EQ.Models;
using MatrixNext.Web.Areas.EQ.Services.Masters;

namespace MatrixNext.Web.Areas.EQ.Services
{
    public class EasyQuoteAdminService
    {
        private readonly EasyQuoteMasterService _masters;
        private readonly IConfiguration _config;

        public EasyQuoteAdminService(EasyQuoteMasterService masters, IConfiguration config)
        {
            _masters = masters;
            _config = config;
        }

        public EasyQuoteAdminViewModel CargarParametros() => new()
        {
            Precios = _masters.AllPrecios(),
            Horas = _masters.AllHoras(),
            ValorHoraOps = _masters.AllValorHora(),
            CostInsumos = _masters.AllCostInsumos(),
            Locaciones = _masters.AllLocaciones(),
            Envios = _masters.AllEnvios(),
            Codificacion = _masters.AllCodificacion(),
            Mystery = _masters.AllMystery(),
            CostUnitarios = _masters.AllCostUnitarios(),
            Estadistica = _masters.AllEstadistica()
        };

        public object UpsertPrecio(EasyQuoteMasterService.PrecioRow row)
        {
            const string sql = @"MERGE eq_param_precio AS t
USING (SELECT @MetodologiaCodigo m, @PenetracionCodigo p, @DuracionMin d) s
ON t.MetodologiaCodigo=s.m AND t.PenetracionCodigo=s.p AND t.DuracionMin=s.d
WHEN MATCHED THEN UPDATE SET ValorTotal=@ValorTotal
WHEN NOT MATCHED THEN INSERT (MetodologiaCodigo,PenetracionCodigo,DuracionMin,ValorTotal,ValorPerfil,ValorCoordinacion)
VALUES (@MetodologiaCodigo,@PenetracionCodigo,@DuracionMin,@ValorTotal,0,0);";
            Exec(sql, row);
            _masters.Reset();
            return new { success = true };
        }

        public object UpsertValorHora(EasyQuoteMasterService.ValorHoraRow row)
        {
            const string sql = @"MERGE eq_valor_hora_ops t USING (SELECT @Nivel n) s ON t.Nivel=s.n
WHEN MATCHED THEN UPDATE SET Variante=@Variante, ValorHora=@ValorHora
WHEN NOT MATCHED THEN INSERT (Nivel,Variante,ValorHora) VALUES (@Nivel,@Variante,@ValorHora);";
            Exec(sql, row);
            _masters.Reset();
            return new { success = true };
        }

        public object UpsertInsumo(EasyQuoteMasterService.CostInsumoRow row)
        {
            const string sql = @"MERGE eq_cost_insumos t USING (SELECT @NSE n,@Tipo t0) s ON t.NSE=s.n AND t.Tipo=s.t0
WHEN MATCHED THEN UPDATE SET ValorUnitario=@ValorUnitario
WHEN NOT MATCHED THEN INSERT (NSE,Tipo,ValorUnitario) VALUES (@NSE,@Tipo,@ValorUnitario);";
            Exec(sql, row);
            _masters.Reset();
            return new { success = true };
        }

        public object UpsertEnvio(EasyQuoteMasterService.EnvioTarifaRow row)
        {
            const string sql = @"MERGE eq_envio_tarifa t USING (SELECT @Tipologia tip) s ON t.Tipologia=s.tip
WHEN MATCHED THEN UPDATE SET KiloInicial=@KiloInicial, KiloAdicional=@KiloAdicional, SeguroPct=@SeguroPct, ValorDeclaradoMin=@ValorDeclaradoMin
WHEN NOT MATCHED THEN INSERT (Tipologia,KiloInicial,KiloAdicional,SeguroPct,ValorDeclaradoMin) VALUES (@Tipologia,@KiloInicial,@KiloAdicional,@SeguroPct,@ValorDeclaradoMin);";
            Exec(sql, row);
            _masters.Reset();
            return new { success = true };
        }

        private void Exec(string sql, object param)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            conn.Execute(sql, param);
        }
    }
}
