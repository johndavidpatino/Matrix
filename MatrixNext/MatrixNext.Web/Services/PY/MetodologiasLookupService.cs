using Dapper;
using System.Data;
using System.Data.SqlClient;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Services.PY
{
    public interface IMetodologiasLookupService
    {
        Task<List<MetodologiaVM>> ObtenerMetodologiasAsync();
        Task<Dictionary<int, string>> ObtenerMapaMetodologiasAsync();
    }

    public class MetodologiasLookupService : IMetodologiasLookupService
    {
        private readonly ILogger<MetodologiasLookupService> _logger;
        private readonly string _connectionString;

        public MetodologiasLookupService(ILogger<MetodologiasLookupService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("LegacyDatabase")
                ?? throw new InvalidOperationException("LegacyDatabase connection string not found");
        }

        public async Task<List<MetodologiaVM>> ObtenerMetodologiasAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                // Preferimos SP si existe: OP_Metodologias_Get (sin parámetros)
                IEnumerable<dynamic> rows;
                try
                {
                    rows = await connection.QueryAsync("OP_Metodologias_Get", commandType: CommandType.StoredProcedure);
                }
                catch
                {
                    // Fallback a tabla común si el SP no existe en el entorno
                    rows = await connection.QueryAsync("SELECT id, MetNombre FROM OP_Metodologias");
                }

                var list = new List<MetodologiaVM>();
                foreach (var row in rows)
                {
                    var dict = (IDictionary<string, object>)row;
                    int id = 0;
                    string nombre = string.Empty;

                    // Id: soportar variantes de nombre de columna
                    if (dict.ContainsKey("Id") && dict["Id"] != null)
                        id = Convert.ToInt32(dict["Id"]);
                    else if (dict.ContainsKey("id") && dict["id"] != null)
                        id = Convert.ToInt32(dict["id"]);

                    // Nombre: soportar MetNombre / Nombre / Metodologia
                    if (dict.ContainsKey("MetNombre") && dict["MetNombre"] != null)
                        nombre = dict["MetNombre"].ToString() ?? string.Empty;
                    else if (dict.ContainsKey("Nombre") && dict["Nombre"] != null)
                        nombre = dict["Nombre"].ToString() ?? string.Empty;
                    else if (dict.ContainsKey("Metodologia") && dict["Metodologia"] != null)
                        nombre = dict["Metodologia"].ToString() ?? string.Empty;

                    if (id > 0)
                    {
                        list.Add(new MetodologiaVM { Id = id, Nombre = nombre });
                    }
                }

                return list.OrderBy(x => x.Nombre).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo de Metodologías desde legacy");
                return new List<MetodologiaVM>();
            }
        }

        public async Task<Dictionary<int, string>> ObtenerMapaMetodologiasAsync()
        {
            var list = await ObtenerMetodologiasAsync();
            return list.GroupBy(x => x.Id).ToDictionary(g => g.Key, g => g.First().Nombre);
        }
    }
}
