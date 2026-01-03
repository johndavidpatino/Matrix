using System;
using System.Linq;
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
    }
}
