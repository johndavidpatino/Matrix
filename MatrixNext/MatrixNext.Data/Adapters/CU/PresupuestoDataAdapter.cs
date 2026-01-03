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
    public class PresupuestoDataAdapter
    {
        private readonly string _connectionString;

        public PresupuestoDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'MatrixDb' not found");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);
        
        private MatrixDbContext CreateContext() => new MatrixDbContext(_connectionString);

        /// <summary>
        /// Obtiene los presupuestos aprobados de una propuesta
        /// </summary>
        public List<PresupuestoAprobadoViewModel> ObtenerPresupuestosAprobados(long idPropuesta)
        {
            using var conn = CreateConnection();
            var presupuestos = conn.Query<PresupuestoAprobadoViewModel>(
                "CU_Presupuestos.DevolverxIdPropuestaAprobados",
                new { IdPropuesta = idPropuesta },
                commandType: CommandType.StoredProcedure
            );

            return presupuestos?.AsList() ?? new List<PresupuestoAprobadoViewModel>();
        }

        /// <summary>
        /// Obtiene los presupuestos asignados a un estudio
        /// </summary>
        public List<PresupuestoAsignadoViewModel> ObtenerPresupuestosAsignadosXEstudio(long idEstudio)
        {
            using var conn = CreateConnection();
            var presupuestos = conn.Query<PresupuestoAsignadoViewModel>(
                "CU_Presupuestos.ObtenerPresupuestosAsignadosXEstudio",
                new { IdEstudio = idEstudio },
                commandType: CommandType.StoredProcedure
            );

            return presupuestos?.AsList() ?? new List<PresupuestoAsignadoViewModel>();
        }

        /// <summary>
        /// Guarda la relación entre un estudio y sus presupuestos aprobados
        /// </summary>
        public void AsignarPresupuestosAEstudio(long idEstudio, List<long> idsPresupuestos)
        {
            using var context = CreateContext();
            
            // Eliminar asignaciones anteriores si es edición
            var asignacionesExistentes = context.Set<CU_Estudios_Presupuestos>()
                .Where(ep => ep.EstudioId == idEstudio)
                .ToList();
            
            if (asignacionesExistentes.Any())
            {
                context.Set<CU_Estudios_Presupuestos>().RemoveRange(asignacionesExistentes);
            }

            // Agregar nuevas asignaciones
            foreach (var idPresupuesto in idsPresupuestos)
            {
                context.Set<CU_Estudios_Presupuestos>().Add(new CU_Estudios_Presupuestos
                {
                    EstudioId = idEstudio,
                    PresupuestoId = idPresupuesto
                });
            }

            context.SaveChanges();
        }
    }
}
