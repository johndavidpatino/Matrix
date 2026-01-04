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

        /// <summary>
        /// Obtiene alternativas registradas para una propuesta.
        /// </summary>
        public List<AlternativaViewModel> ObtenerAlternativas(long idPropuesta)
        {
            using var context = CreateContext();

            var alternativas = context.IQDatosGenerales
                .Where(x => x.IdPropuesta == idPropuesta)
                .Select(x => new AlternativaViewModel
                {
                    IdPropuesta = x.IdPropuesta,
                    ParAlternativa = x.ParAlternativa,
                    Descripcion = x.Descripcion,
                    Observaciones = x.Observaciones,
                    DiasCampo = x.DiasCampo,
                    DiasDiseno = x.DiasDiseno,
                    DiasInformes = x.DiasInformes,
                    DiasProcesamiento = x.DiasProcesamiento,
                    NumeroMediciones = x.NumeroMediciones,
                    MesesMediciones = x.MesesMediciones,
                    TipoPresupuesto = x.TipoPresupuesto,
                    NoIQuote = x.NoIQuote
                })
                .ToList();

            var resumenPresupuestos = context.Presupuestos
                .Where(p => p.PropuestaId == idPropuesta)
                .GroupBy(p => p.Alternativa)
                .Select(g => new
                {
                    Alternativa = g.Key,
                    Total = g.Count(),
                    ValorTotal = g.Sum(p => p.Valor.HasValue ? Convert.ToDecimal(p.Valor.Value) : 0m)
                })
                .ToList();

            foreach (var alt in alternativas)
            {
                var resumen = resumenPresupuestos.FirstOrDefault(r => r.Alternativa == alt.ParAlternativa);
                if (resumen != null)
                {
                    alt.TotalPresupuestos = resumen.Total;
                    alt.ValorTotal = resumen.ValorTotal;
                }
            }

            return alternativas.OrderBy(a => a.ParAlternativa).ToList();
        }

        public EditarAlternativaViewModel? ObtenerDatosGenerales(long idPropuesta, int alternativa)
        {
            using var context = CreateContext();
            var entidad = context.IQDatosGenerales
                .FirstOrDefault(x => x.IdPropuesta == idPropuesta && x.ParAlternativa == alternativa);

            if (entidad == null)
            {
                return null;
            }

            return new EditarAlternativaViewModel
            {
                IdPropuesta = entidad.IdPropuesta,
                ParAlternativa = entidad.ParAlternativa,
                Descripcion = entidad.Descripcion,
                Observaciones = entidad.Observaciones,
                DiasCampo = entidad.DiasCampo,
                DiasDiseno = entidad.DiasDiseno,
                DiasProcesamiento = entidad.DiasProcesamiento,
                DiasInformes = entidad.DiasInformes,
                NumeroMediciones = entidad.NumeroMediciones,
                MesesMediciones = entidad.MesesMediciones,
                TipoPresupuesto = entidad.TipoPresupuesto,
                NoIQuote = entidad.NoIQuote
            };
        }

        /// <summary>
        /// Inserta o actualiza los datos generales de alternativa.
        /// </summary>
        public (bool success, string message, int alternativa) GuardarDatosGenerales(EditarAlternativaViewModel model)
        {
            using var context = CreateContext();

            var alternativa = model.ParAlternativa > 0
                ? model.ParAlternativa
                : CalcularSiguienteAlternativa(context, model.IdPropuesta);

            var entidad = context.IQDatosGenerales
                .FirstOrDefault(x => x.IdPropuesta == model.IdPropuesta && x.ParAlternativa == alternativa);

            if (entidad == null)
            {
                entidad = new IQ_DatosGeneralesPresupuesto
                {
                    IdPropuesta = model.IdPropuesta,
                    ParAlternativa = alternativa
                };
                context.IQDatosGenerales.Add(entidad);
            }

            entidad.Descripcion = model.Descripcion?.Trim();
            entidad.Observaciones = model.Observaciones?.Trim();
            entidad.DiasCampo = model.DiasCampo;
            entidad.DiasDiseno = model.DiasDiseno;
            entidad.DiasProcesamiento = model.DiasProcesamiento;
            entidad.DiasInformes = model.DiasInformes;
            entidad.NumeroMediciones = model.NumeroMediciones;
            entidad.MesesMediciones = model.MesesMediciones;
            entidad.TipoPresupuesto = model.TipoPresupuesto;
            entidad.NoIQuote = model.NoIQuote?.Trim();

            context.SaveChanges();
            return (true, "Datos generales guardados", alternativa);
        }

        public int CalcularSiguienteAlternativa(long idPropuesta)
        {
            using var context = CreateContext();
            return CalcularSiguienteAlternativa(context, idPropuesta);
        }

        private static int CalcularSiguienteAlternativa(MatrixDbContext context, long idPropuesta)
        {
            var maxAlt = context.IQDatosGenerales
                .Where(x => x.IdPropuesta == idPropuesta)
                .Select(x => (int?)x.ParAlternativa)
                .Max();

            var maxPresupuestoAlt = context.Presupuestos
                .Where(p => p.PropuestaId == idPropuesta && p.Alternativa.HasValue)
                .Select(p => (int?)p.Alternativa)
                .Max();

            var current = new[] { maxAlt ?? 0, maxPresupuestoAlt ?? 0 }.Max();
            return current + 1;
        }
    }
}
