using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using MatrixNext.Data.Modules.CU.Models;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.CU
{
    /// <summary>
    /// Adapter para operaciones de JobBook/Brief (Default.aspx legacy)
    /// Usa SP legado CU_InfoGeneralJobBook_GET para consultas.
    /// </summary>
    public class CuentaDataAdapter
    {
        private readonly string _connectionString;

        public CuentaDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'MatrixDb' not found");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public List<JobBookResultViewModel> BuscarJobBooks(string? titulo, string? jobBook, long? idPropuesta, long idUsuario, int typeSearch, long? idBrief = null, long? idEstudio = null)
        {
            using var conn = CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Titulo", titulo);
            parameters.Add("@JobBook", jobBook);
            parameters.Add("@IdPropuesta", idPropuesta);
            parameters.Add("@IdBrief", idBrief);
            parameters.Add("@IdEstudio", idEstudio);
            parameters.Add("@Gerente", idUsuario);
            parameters.Add("@TypeSearch", typeSearch);

            var result = conn.Query<JobBookResultViewModel>(
                "CU_InfoGeneralJobBook_GET",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result?.AsList() ?? new List<JobBookResultViewModel>();
        }

        public JobBookContextViewModel? ObtenerContexto(long? idBrief, long? idPropuesta, long? idEstudio, long idUsuario)
        {
            var data = BuscarJobBooks(null, null, idPropuesta, idUsuario, 3, idBrief, idEstudio);
            if (data.Count == 0)
            {
                return null;
            }

            var first = data[0];
            return new JobBookContextViewModel
            {
                IdBrief = first.IdBrief,
                IdPropuesta = first.IdPropuesta,
                IdEstudio = first.IdEstudio,
                Cliente = first.Cliente,
                Titulo = first.Titulo,
                NumJobBook = first.NumJobBook
            };
        }

        public void ClonarBrief(long idBrief, long idUsuario, int idUnidad, string nuevoNombre)
        {
            // SP por confirmar en legacy (CU_Brief.CloneBrief). No se implementa sin evidencia.
            throw new NotImplementedException("CloneBrief requiere confirmar SP en legacy (CU_Brief.CloneBrief)");
        }
    }
}
