using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MatrixNext.Data.Entities;
using MatrixNext.Data.Modules.CU.Models;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.CU
{
    public class EstudioDataAdapter
    {
        private readonly string _connectionString;

        public EstudioDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'MatrixDb' not found");
        }

        private MatrixDbContext CreateContext() => new MatrixDbContext(_connectionString);

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public List<EstudioListItemViewModel> ObtenerListado(long propuestaId, long usuarioId)
        {
            using var conn = CreateConnection();
            var data = conn.Query<EstudioListItemViewModel>(
                "CU_Estudios_Get",
                new { PropuestaId = propuestaId, IdGerenteCuentas = usuarioId },
                commandType: CommandType.StoredProcedure
            );

            return data?.AsList() ?? new List<EstudioListItemViewModel>();
        }

        public CU_Estudios? ObtenerEntidad(long id)
        {
            using var context = CreateContext();
            return context.Estudios.FirstOrDefault(e => e.Id == id);
        }

        public long Guardar(CU_Estudios entidad)
        {
            using var context = CreateContext();
            if (entidad.Id == 0)
            {
                context.Estudios.Add(entidad);
            }
            else
            {
                context.Estudios.Update(entidad);
            }

            context.SaveChanges();
            return entidad.Id;
        }

        public List<CatalogoItem<byte>> ObtenerDocumentosSoporte()
        {
            using var conn = CreateConnection();
            return conn.Query<CatalogoItem<byte>>(
                "SELECT Id as Id, Descripcion as Nombre FROM CU_Estudios_DocumentosSoporte ORDER BY Id"
            ).AsList();
        }
    }
}
