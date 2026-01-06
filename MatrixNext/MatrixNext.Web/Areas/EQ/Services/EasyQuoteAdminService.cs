
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
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
            Estadistica = _masters.AllEstadistica(),
            ParamMisc = _masters.AllMisc(),
            EnvioParam = _masters.GetEnvioParam() ?? new EasyQuoteMasterService.EnvioParamRow(),
            Productividad = _masters.AllProductividad(),
            BaseDatos = _masters.AllBaseDatos()
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

        public object UpsertMisc(EasyQuoteMasterService.ParamMiscRow row)
        {
            const string sql = @"MERGE eq_param_misc t USING (SELECT @Clave c) s ON t.Clave=s.c
WHEN MATCHED THEN UPDATE SET ValorDecimal=@ValorDecimal, ValorTexto=@ValorTexto
WHEN NOT MATCHED THEN INSERT (Clave,ValorDecimal,ValorTexto) VALUES (@Clave,@ValorDecimal,@ValorTexto);";
            Exec(sql, row);
            _masters.Reset();
            return new { success = true };
        }

        public object UpsertEnvioParam(EasyQuoteMasterService.EnvioParamRow row)
        {
            const string sql = @"IF EXISTS (SELECT 1 FROM eq_envio_param)
UPDATE eq_envio_param SET DivisorVolumetrico=@DivisorVolumetrico, TipologiaUrbano=@TipologiaUrbano, TipologiaNacional=@TipologiaNacional
ELSE INSERT (DivisorVolumetrico,TipologiaUrbano,TipologiaNacional) VALUES (@DivisorVolumetrico,@TipologiaUrbano,@TipologiaNacional);";
            Exec(sql, row);
            _masters.Reset();
            return new { success = true };
        }

        public object UpsertProductividad(EasyQuoteMasterService.ProductividadCiudadRow row)
        {
            const string sql = @"MERGE eq_productividad_ciudad t USING (SELECT @Ciudad c) s ON t.Ciudad=s.c
WHEN MATCHED THEN UPDATE SET Encuestadores=@Encuestadores, Productividad=@Productividad
WHEN NOT MATCHED THEN INSERT (Ciudad,Encuestadores,Productividad) VALUES (@Ciudad,@Encuestadores,@Productividad);";
            Exec(sql, row);
            _masters.Reset();
            return new { success = true };
        }

        public object UpsertBaseDatos(EasyQuoteMasterService.BaseDatosRow row)
        {
            const string sql = @"MERGE eq_cost_base_datos t USING (SELECT @Tipo t0) s ON t.Tipo=s.t0
WHEN MATCHED THEN UPDATE SET Valor=@Valor
WHEN NOT MATCHED THEN INSERT (Tipo,Valor) VALUES (@Tipo,@Valor);";
            Exec(sql, row);
            _masters.Reset();
            return new { success = true };
        }

        public object ImportPreciosCsv(IFormFile file, string version)
        {
            if (file == null || file.Length == 0) return new { success = false, message = "Archivo vacío" };
            var rows = new List<EasyQuoteMasterService.PrecioRow>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string line;
                bool first = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split(new[] { ';', ',' });
                    if (first && parts[0].Contains("Metodologia", StringComparison.OrdinalIgnoreCase))
                    {
                        first = false;
                        continue;
                    }
                    if (parts.Length < 4) continue;
                    rows.Add(new EasyQuoteMasterService.PrecioRow
                    {
                        MetodologiaCodigo = parts[0].Trim(),
                        PenetracionCodigo = parts[1].Trim(),
                        DuracionMin = int.TryParse(parts[2], out var d) ? d : 0,
                        ValorTotal = decimal.TryParse(parts[3], out var v) ? v : 0
                    });
                }
            }

            const string deleteSql = "DELETE FROM eq_param_precio";
            const string insertSql = @"INSERT INTO eq_param_precio (MetodologiaCodigo,PenetracionCodigo,DuracionMin,ValorTotal,ValorPerfil,ValorCoordinacion)
                                       VALUES (@MetodologiaCodigo,@PenetracionCodigo,@DuracionMin,@ValorTotal,0,0);";
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            conn.Open();
            using var tran = conn.BeginTransaction();
            conn.Execute(deleteSql, transaction: tran);
            conn.Execute(insertSql, rows, transaction: tran);
            // guardar version en misc
            conn.Execute(@"MERGE eq_param_misc t USING (SELECT 'PRECIOS_VERSION' clave) s ON t.Clave=s.clave
WHEN MATCHED THEN UPDATE SET ValorTexto=@ver
WHEN NOT MATCHED THEN INSERT (Clave,ValorTexto) VALUES ('PRECIOS_VERSION',@ver);", new { ver = version ?? DateTime.Now.ToString("s") }, transaction: tran);
            tran.Commit();
            _masters.Reset();
            return new { success = true, imported = rows.Count };
        }

        public object ImportValorHoraCsv(IFormFile file, string version)
        {
            if (file == null || file.Length == 0) return new { success = false, message = "Archivo vacío" };
            var rows = new List<EasyQuoteMasterService.ValorHoraRow>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string line;
                bool first = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split(new[] { ';', ',' });
                    if (first && parts[0].Contains("Nivel", StringComparison.OrdinalIgnoreCase))
                    {
                        first = false;
                        continue;
                    }
                    if (parts.Length < 3) continue;
                    rows.Add(new EasyQuoteMasterService.ValorHoraRow
                    {
                        Nivel = parts[0].Trim(),
                        Variante = parts[1].Trim(),
                        ValorHora = decimal.TryParse(parts[2], out var v) ? v : 0
                    });
                }
            }

            const string deleteSql = "DELETE FROM eq_valor_hora_ops";
            const string insertSql = @"INSERT INTO eq_valor_hora_ops (Nivel,Variante,ValorHora) VALUES (@Nivel,@Variante,@ValorHora);";
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            conn.Open();
            using var tran = conn.BeginTransaction();
            conn.Execute(deleteSql, transaction: tran);
            conn.Execute(insertSql, rows, transaction: tran);
            conn.Execute(@"MERGE eq_param_misc t USING (SELECT 'VALORHORA_VERSION' clave) s ON t.Clave=s.clave
WHEN MATCHED THEN UPDATE SET ValorTexto=@ver
WHEN NOT MATCHED THEN INSERT (Clave,ValorTexto) VALUES ('VALORHORA_VERSION',@ver);", new { ver = version ?? DateTime.Now.ToString("s") }, transaction: tran);
            tran.Commit();
            _masters.Reset();
            return new { success = true, imported = rows.Count };
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

        public object UpsertLocacion(EasyQuoteMasterService.LocacionRow row)
        {
            const string sql = @"MERGE eq_locaciones t USING (SELECT @Ciudad c) s ON t.Ciudad=s.c
WHEN MATCHED THEN UPDATE SET TarifaBase=@TarifaBase, TarifaConGross=@TarifaConGross, DiasBase=@DiasBase
WHEN NOT MATCHED THEN INSERT (Ciudad,TarifaBase,TarifaConGross,DiasBase) VALUES (@Ciudad,@TarifaBase,@TarifaConGross,@DiasBase);";
            Exec(sql, row);
            _masters.Reset();
            return new { success = true };
        }

        public object UpsertMystery(EasyQuoteMasterService.MysteryTarifaRow row)
        {
            const string sql = @"MERGE eq_tarifa_mystery t USING (SELECT @TipoVisita tv,@Complejidad comp) s ON t.TipoVisita=s.tv AND t.Complejidad=s.comp
WHEN MATCHED THEN UPDATE SET VrUnitario=@VrUnitario, OlasDefault=@OlasDefault
WHEN NOT MATCHED THEN INSERT (TipoVisita,Complejidad,VrUnitario,OlasDefault) VALUES (@TipoVisita,@Complejidad,@VrUnitario,@OlasDefault);";
            Exec(sql, row);
            _masters.Reset();
            return new { success = true };
        }

        public object UpsertCodificacion(EasyQuoteMasterService.CodificacionRow row)
        {
            const string sql = @"MERGE eq_codificacion_param t USING (SELECT @Escenario e) s ON t.Escenario=s.e
WHEN MATCHED THEN UPDATE SET Registros=@Registros, PregAbiertas=@PregAbiertas, PregAbiertasMult=@PregAbiertasMult, Dias=@Dias, Horas=@Horas, ValorIpsos=@ValorIpsos
WHEN NOT MATCHED THEN INSERT (Escenario,Registros,PregAbiertas,PregAbiertasMult,Dias,Horas,ValorIpsos) VALUES (@Escenario,@Registros,@PregAbiertas,@PregAbiertasMult,@Dias,@Horas,@ValorIpsos);";
            Exec(sql, row);
            _masters.Reset();
            return new { success = true };
        }

        public object UpsertCostUnitario(EasyQuoteMasterService.CostUnitarioOpsRow row)
        {
            const string sql = @"MERGE eq_cost_unitario_ops t USING (SELECT @CodMatrix cod) s ON t.CodMatrix=s.cod
WHEN MATCHED THEN UPDATE SET Actividad=@Actividad, Tarifa=@Tarifa, Unidad=@Unidad, Horas=@Horas
WHEN NOT MATCHED THEN INSERT (CodMatrix,Actividad,Tarifa,Unidad,Horas) VALUES (@CodMatrix,@Actividad,@Tarifa,@Unidad,@Horas);";
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
