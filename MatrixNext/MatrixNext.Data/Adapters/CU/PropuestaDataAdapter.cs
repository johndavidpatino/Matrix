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
    public class PropuestaDataAdapter
    {
        private readonly string _connectionString;

        public PropuestaDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'MatrixDb' not found");
        }

        private MatrixDbContext CreateContext() => new MatrixDbContext(_connectionString);

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public List<PropuestaListItemViewModel> ObtenerListado(long userId, byte? estadoId = null)
        {
            using var conn = CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@IdGerenteCuentas", userId);
            parameters.Add("@IdEstado", estadoId);

            var data = conn.Query<PropuestaListItemViewModel>(
                "CU_Propuestas_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return data?.AsList() ?? new List<PropuestaListItemViewModel>();
        }

        public CU_Propuestas? ObtenerEntidadPorId(long id)
        {
            using var context = CreateContext();
            return context.Propuestas.FirstOrDefault(x => x.Id == id);
        }

        public long Guardar(CU_Propuestas entidad)
        {
            using var context = CreateContext();
            if (entidad.Id == 0)
            {
                context.Propuestas.Add(entidad);
            }
            else
            {
                context.Propuestas.Update(entidad);
            }

            context.SaveChanges();
            return entidad.Id;
        }

        public bool Eliminar(long id)
        {
            using var context = CreateContext();
            var entity = context.Propuestas.FirstOrDefault(p => p.Id == id);
            if (entity == null)
            {
                return false;
            }

            context.Propuestas.Remove(entity);
            return context.SaveChanges() > 0;
        }

        public List<CatalogoItem<byte>> ObtenerEstados()
        {
            using var conn = CreateConnection();
            return conn.Query<CatalogoItem<byte>>(
                "SELECT id AS Id, Estado AS Nombre FROM CU_EstadoPropuesta ORDER BY id"
            ).AsList();
        }

        public List<CatalogoItem<decimal>> ObtenerProbabilidades()
        {
            using var conn = CreateConnection();
            return conn.Query<CatalogoItem<decimal>>(
                "SELECT id AS Id, probabilidad AS Nombre FROM CU_ProbabilidadAprobacion ORDER BY id"
            ).AsList();
        }

        public List<CatalogoItem<short>> ObtenerRazones()
        {
            using var conn = CreateConnection();
            return conn.Query<CatalogoItem<short>>(
                "SELECT id AS Id, razon AS Nombre FROM CU_RazonesNoAprobacion ORDER BY razon"
            ).AsList();
        }

        public List<ObservacionViewModel> ObtenerObservaciones(long propuestaId)
        {
            using var conn = CreateConnection();
            return conn.Query<ObservacionViewModel>(
                "CU_SeguimientoPropuestas_Get",
                new { PropuestaId = propuestaId },
                commandType: CommandType.StoredProcedure
            ).AsList();
        }

        public void GuardarObservacion(long propuestaId, long usuarioId, string observacion)
        {
            using var context = CreateContext();
            var entity = new CU_SeguimientoPropuestas
            {
                PropuestaId = propuestaId,
                UsuarioId = usuarioId,
                Observacion = observacion,
                Fecha = DateTime.Now
            };
            context.SeguimientoPropuestas.Add(entity);
            context.SaveChanges();
        }
    }
}
