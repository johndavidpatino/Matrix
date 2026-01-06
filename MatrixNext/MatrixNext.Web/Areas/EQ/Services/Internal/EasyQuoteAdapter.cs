using System.Data;
using System.Data.SqlClient;
using Dapper;
using MatrixNext.Web.Areas.EQ.Models;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Web.Areas.EQ.Services.Internal
{
    public class EasyQuoteAdapter
    {
        private readonly string _connString;
        public EasyQuoteAdapter(IConfiguration config)
        {
            _connString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public EasyQuoteViewModel NuevaQuote() => new EasyQuoteViewModel
        {
            SampleCities = new()
            {
                new EQSampleCity { Ciudad = "Bogota", Activa = true },
                new EQSampleCity { Ciudad = "B/quilla" },
                new EQSampleCity { Ciudad = "M/llin" },
                new EQSampleCity { Ciudad = "Cali" },
                new EQSampleCity { Ciudad = "B/manga" },
                new EQSampleCity { Ciudad = "C/gena" },
                new EQSampleCity { Ciudad = "Otras Ciudades" }
            },
            MysteryVisits = new()
            {
                new EQMysteryVisit { TipoVisita = "TIPO VISITA 1", Complejidad = "Basica - Video", NumOlas = 1 }
            },
            StaffSL = new()
            {
                new EQStaffSL { Nivel = "L4" },
                new EQStaffSL { Nivel = "L5" },
                new EQStaffSL { Nivel = "L6" },
                new EQStaffSL { Nivel = "L7" }
            }
        };

        public EasyQuoteViewModel? ObtenerQuote(long id)
        {
            using var conn = new SqlConnection(_connString);
            using var multi = conn.QueryMultiple("EQ_Quote_Get", new { Id = id }, commandType: CommandType.StoredProcedure);

            var header = multi.ReadFirstOrDefault<EQHeader>();
            if (header == null) return null;
            var q = multi.ReadFirstOrDefault<EQQuestionnaire>() ?? new EQQuestionnaire();
            var m = multi.ReadFirstOrDefault<EQMethodology>() ?? new EQMethodology();
            var cities = multi.Read<EQSampleCity>()?.ToList() ?? new List<EQSampleCity>();
            var mys = multi.Read<EQMysteryVisit>()?.ToList() ?? new List<EQMysteryVisit>();
            var staff = multi.Read<EQStaffSL>()?.ToList() ?? new List<EQStaffSL>();

            return new EasyQuoteViewModel
            {
                Id = id,
                Header = header,
                Questionnaire = q,
                Methodology = m,
                SampleCities = cities,
                MysteryVisits = mys,
                StaffSL = staff,
                Summary = new EQSummary()
            };
        }

        public long Guardar(EasyQuoteViewModel vm)
        {
            using var conn = new SqlConnection(_connString);
            var dp = new DynamicParameters();
            dp.Add("@Id", vm.Id, direction: ParameterDirection.InputOutput);
            dp.Add("@Nombre", vm.Header.Nombre);
            dp.Add("@GrupoObjetivo", vm.Header.GrupoObjetivo);
            dp.Add("@Cliente", vm.Header.Cliente);
            dp.Add("@FechaAprobacionEstimada", vm.Header.FechaAprobacionEstimada);
            dp.Add("@FechaCampo", vm.Header.FechaCampo);
            dp.Add("@ProbAprobacion", vm.Header.ProbAprobacion);
            dp.Add("@SL", vm.Header.SL);
            dp.Add("@MetodologiaSL", vm.Header.MetodologiaSL);
            dp.Add("@RecordDetail", vm.Header.RecordDetail);
            dp.Add("@CategoriaProducto", vm.Header.CategoriaProducto);
            dp.Add("@ValorProveedorExterno", vm.Header.ValorProveedorExterno);
            dp.Add("@ValorProveedorInternacional", vm.Header.ValorProveedorInternacional);
            dp.Add("@ValorGMU", vm.Header.ValorGMU);

            var tvpQuestionnaire = ToDataTable(new[] { vm.Questionnaire });
            dp.Add("@Questionnaire", tvpQuestionnaire.AsTableValuedParameter("EQ_QuestionnaireType"));

            var tvpMethodology = ToDataTable(new[] { vm.Methodology });
            dp.Add("@Methodology", tvpMethodology.AsTableValuedParameter("EQ_MethodologyType"));

            var tvpCities = ToDataTable(vm.SampleCities);
            dp.Add("@SampleCities", tvpCities.AsTableValuedParameter("EQ_SampleCityType"));

            var tvpMystery = ToDataTable(vm.MysteryVisits);
            dp.Add("@Mystery", tvpMystery.AsTableValuedParameter("EQ_MysteryVisitType"));

            var tvpStaff = ToDataTable(vm.StaffSL);
            dp.Add("@StaffSL", tvpStaff.AsTableValuedParameter("EQ_StaffSLType"));

            conn.Execute("EQ_Quote_Save", dp, commandType: CommandType.StoredProcedure);
            return dp.Get<long>("@Id");
        }

        // Helpers: convert list to DataTable for TVP
        private static DataTable ToDataTable<T>(IEnumerable<T> data)
        {
            var dt = new DataTable();
            var props = typeof(T).GetProperties();
            foreach (var p in props)
            {
                var colType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                dt.Columns.Add(p.Name, colType);
            }

            foreach (var item in data)
            {
                var row = dt.NewRow();
                foreach (var p in props)
                {
                    row[p.Name] = p.GetValue(item) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
