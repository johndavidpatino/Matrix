using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MatrixNext.Data.Entities;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.CU
{
    public class BriefDataAdapter
    {
        private readonly string _connectionString;

        public BriefDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'MatrixDb' not found");
        }

        private MatrixDbContext CreateContext() => new MatrixDbContext(_connectionString);

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public CU_Brief? ObtenerPorId(long id)
        {
            using var context = CreateContext();
            return context.Briefs.FirstOrDefault(b => b.Id == id);
        }

        public long Guardar(CU_Brief entidad)
        {
            using var context = CreateContext();
            if (entidad.Id == 0)
            {
                context.Briefs.Add(entidad);
            }
            else
            {
                context.Briefs.Update(entidad);
            }

            context.SaveChanges();
            return entidad.Id;
        }

        /// <summary>
        /// Clona un Brief a otra unidad usando el SP CU_Brief_Clone
        /// </summary>
        /// <param name="idBrief">ID del Brief original</param>
        /// <param name="idUsuario">ID del usuario que clona</param>
        /// <param name="idUnidad">ID de la unidad destino</param>
        /// <param name="nuevoTitulo">Nuevo título para el Brief clonado</param>
        /// <returns>ID del nuevo Brief clonado</returns>
        public long ClonarBrief(long idBrief, long idUsuario, int idUnidad, string nuevoTitulo)
        {
            using var connection = CreateConnection();
            var parameters = new
            {
                IdBrief = idBrief,
                IdUsuario = idUsuario,
                IdUnidad = idUnidad,
                NuevoNombre = nuevoTitulo
            };

            // El SP CU_Brief_Clone retorna el ID del nuevo Brief clonado
            var result = connection.ExecuteScalar<long>(
                "CU_Brief_Clone",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}
