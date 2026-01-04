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

        /// <summary>
        /// Obtiene los presupuestos de una alternativa, opcionalmente filtrados por técnica
        /// </summary>
        public List<PresupuestoListItemViewModel> ObtenerPresupuestos(long idPropuesta, int alternativa, int? tecnicaFiltro = null)
        {
            using var context = CreateContext();

            var query = context.IQParametros
                .Where(p => p.IdPropuesta == idPropuesta && p.ParAlternativa == alternativa);

            if (tecnicaFiltro.HasValue)
            {
                query = query.Where(p => p.TecCodigo == tecnicaFiltro.Value);
            }

            var presupuestos = query
                .Select(p => new PresupuestoListItemViewModel
                {
                    IdPropuesta = p.IdPropuesta,
                    ParAlternativa = p.ParAlternativa,
                    MetCodigo = p.MetCodigo,
                    ParNacional = p.ParNacional,
                    TecCodigo = p.TecCodigo,
                    ValorVenta = p.ParValorVenta,
                    GrossMargin = p.ParGrossMargin,
                    Revisado = p.ParRevisado,
                    FechaCreacion = p.ParFechaCreacion,
                    NoIQuote = p.NoIQuote,
                    JobBook = p.ParNumJobBook
                })
                .ToList();

            // Obtener total de muestra por presupuesto
            foreach (var pres in presupuestos)
            {
                var totalMuestra = context.IQMuestra
                    .Where(m => m.IdPropuesta == pres.IdPropuesta 
                        && m.ParAlternativa == pres.ParAlternativa 
                        && m.MetCodigo == pres.MetCodigo 
                        && m.ParNacional == pres.ParNacional)
                    .Sum(m => (int?)m.MuCantidad) ?? 0;

                pres.TotalMuestra = totalMuestra;
            }

            return presupuestos;
        }

        /// <summary>
        /// Obtiene un presupuesto completo para edición
        /// </summary>
        public EditarPresupuestoViewModel? ObtenerPresupuesto(long idPropuesta, int alternativa, int metCodigo, int nacional)
        {
            using var context = CreateContext();

            var parametros = context.IQParametros
                .FirstOrDefault(p => p.IdPropuesta == idPropuesta 
                    && p.ParAlternativa == alternativa 
                    && p.MetCodigo == metCodigo 
                    && p.ParNacional == nacional);

            if (parametros == null)
                return null;

            var preguntas = context.IQPreguntas
                .FirstOrDefault(p => p.IdPropuesta == idPropuesta 
                    && p.ParAlternativa == alternativa 
                    && p.MetCodigo == metCodigo 
                    && p.ParNacional == nacional);

            var muestra = context.IQMuestra
                .Where(m => m.IdPropuesta == idPropuesta 
                    && m.ParAlternativa == alternativa 
                    && m.MetCodigo == metCodigo 
                    && m.ParNacional == nacional)
                .Select(m => new MuestraItemViewModel
                {
                    IdPropuesta = m.IdPropuesta,
                    ParAlternativa = m.ParAlternativa,
                    MetCodigo = m.MetCodigo,
                    CiuCodigo = m.CiuCodigo,
                    MuIdentificador = m.MuIdentificador,
                    ParNacional = m.ParNacional,
                    DeptCodigo = m.DeptCodigo,
                    MuCantidad = m.MuCantidad
                })
                .ToList();

            return new EditarPresupuestoViewModel
            {
                IdPropuesta = parametros.IdPropuesta,
                ParAlternativa = parametros.ParAlternativa,
                MetCodigo = parametros.MetCodigo,
                ParNacional = parametros.ParNacional,
                TecCodigo = parametros.TecCodigo,
                TipoProyecto = parametros.TipoProyecto,
                ParGrupoObjetivo = parametros.ParGrupoObjetivo,
                ParIncidencia = parametros.ParIncidencia,
                ParProductividad = parametros.ParProductividad,
                ParProbabilistico = parametros.ParProbabilistico,
                F2FVirtual = parametros.F2FVirtual,
                PregCerradas = preguntas?.PregCerradas,
                PregCerradasMultiples = preguntas?.PregCerradasMultiples,
                PregAbiertas = preguntas?.PregAbiertas,
                PregAbiertasMultiples = preguntas?.PregAbiertasMultiples,
                PregOtras = preguntas?.PregOtras,
                PregDemograficos = preguntas?.PregDemograficos,
                ParTiempoEncuesta = parametros.ParTiempoEncuesta,
                Complejidad = parametros.Complejidad,
                DPComplejidadCuestionario = parametros.DPComplejidadCuestionario,
                ParNProcesosDC = parametros.ParNProcesosDC,
                ParNProcesosTopLines = parametros.ParNProcesosTopLines,
                ParNProcesosTablas = parametros.ParNProcesosTablas,
                ParNProcesosBases = parametros.ParNProcesosBases,
                ComplejidadCodificacion = parametros.ComplejidadCodificacion,
                DPTransformacion = parametros.DPTransformacion,
                DPUnificacion = parametros.DPUnificacion,
                DPComplejidad = parametros.DPComplejidad,
                DPPonderacion = parametros.DPPonderacion,
                DPInInterna = parametros.DPInInterna,
                DPInCliente = parametros.DPInCliente,
                DPInPanel = parametros.DPInPanel,
                DPInExterno = parametros.DPInExterno,
                DPInGMU = parametros.DPInGMU,
                DPInOtro = parametros.DPInOtro,
                DPOutCliente = parametros.DPOutCliente,
                DPOutWebDelivery = parametros.DPOutWebDelivery,
                DPOutExterno = parametros.DPOutExterno,
                DPOutGMU = parametros.DPOutGMU,
                DPOutOtro = parametros.DPOutOtro,
                ParUnidadesProducto = parametros.ParUnidadesProducto,
                ParValorUnitarioProd = parametros.ParValorUnitarioProd,
                PTApoyosPunto = parametros.PTApoyosPunto,
                PTCompra = parametros.PTCompra,
                PTNeutralizador = parametros.PTNeutralizador,
                PTTipoProducto = parametros.PTTipoProducto,
                PTLotes = parametros.PTLotes,
                PTVisitas = parametros.PTVisitas,
                PTCeldas = parametros.PTCeldas,
                PTProductosEvaluar = parametros.PTProductosEvaluar,
                ParTipoCLT = parametros.ParTipoCLT,
                ParAlquilerEquipos = parametros.ParAlquilerEquipos,
                ParApoyoLogistico = parametros.ParApoyoLogistico,
                ParAccesoInternet = parametros.ParAccesoInternet,
                ParPorcentajeIntercep = parametros.ParPorcentajeIntercep,
                ParPorcentajeRecluta = parametros.ParPorcentajeRecluta,
                ParEncuestadoresPunto = parametros.ParEncuestadoresPunto,
                ParObservaciones = parametros.ParObservaciones,
                ParUsaTablet = parametros.ParUsaTablet,
                ParUsaPapel = parametros.ParUsaPapel,
                ParDispPropio = parametros.ParDispPropio,
                Muestra = muestra
            };
        }

        /// <summary>
        /// Guarda o actualiza un presupuesto completo (transacción)
        /// </summary>
        public (bool success, string message) GuardarPresupuesto(EditarPresupuestoViewModel model, long usuarioId)
        {
            using var context = CreateContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                // 1. Guardar/actualizar IQ_Parametros
                var parametros = context.IQParametros
                    .FirstOrDefault(p => p.IdPropuesta == model.IdPropuesta 
                        && p.ParAlternativa == model.ParAlternativa 
                        && p.MetCodigo == model.MetCodigo 
                        && p.ParNacional == model.ParNacional);

                bool esNuevo = parametros == null;

                if (esNuevo)
                {
                    parametros = new IQ_Parametros
                    {
                        IdPropuesta = model.IdPropuesta,
                        ParAlternativa = model.ParAlternativa,
                        MetCodigo = model.MetCodigo,
                        ParNacional = model.ParNacional,
                        Usuario = usuarioId,
                        ParFechaCreacion = DateTime.UtcNow.AddHours(-5) // Colombia timezone
                    };
                    context.IQParametros.Add(parametros);
                }

                // Mapear 110+ propiedades
                parametros.TecCodigo = model.TecCodigo;
                parametros.TipoProyecto = model.TipoProyecto;
                parametros.ParGrupoObjetivo = model.ParGrupoObjetivo?.Trim();
                parametros.ParIncidencia = model.ParIncidencia;
                parametros.ParProductividad = model.ParProductividad;
                parametros.ParProbabilistico = model.ParProbabilistico;
                parametros.F2FVirtual = model.F2FVirtual;
                parametros.ParTiempoEncuesta = model.ParTiempoEncuesta;
                parametros.Complejidad = model.Complejidad;
                parametros.DPComplejidadCuestionario = model.DPComplejidadCuestionario;
                parametros.ParNProcesosDC = model.ParNProcesosDC;
                parametros.ParNProcesosTopLines = model.ParNProcesosTopLines;
                parametros.ParNProcesosTablas = model.ParNProcesosTablas;
                parametros.ParNProcesosBases = model.ParNProcesosBases;
                parametros.ComplejidadCodificacion = model.ComplejidadCodificacion;
                parametros.DPTransformacion = model.DPTransformacion;
                parametros.DPUnificacion = model.DPUnificacion;
                parametros.DPComplejidad = model.DPComplejidad;
                parametros.DPPonderacion = model.DPPonderacion;
                parametros.DPInInterna = model.DPInInterna;
                parametros.DPInCliente = model.DPInCliente;
                parametros.DPInPanel = model.DPInPanel;
                parametros.DPInExterno = model.DPInExterno;
                parametros.DPInGMU = model.DPInGMU;
                parametros.DPInOtro = model.DPInOtro;
                parametros.DPOutCliente = model.DPOutCliente;
                parametros.DPOutWebDelivery = model.DPOutWebDelivery;
                parametros.DPOutExterno = model.DPOutExterno;
                parametros.DPOutGMU = model.DPOutGMU;
                parametros.DPOutOtro = model.DPOutOtro;
                parametros.ParUnidadesProducto = model.ParUnidadesProducto;
                parametros.ParValorUnitarioProd = model.ParValorUnitarioProd;
                parametros.PTApoyosPunto = model.PTApoyosPunto;
                parametros.PTCompra = model.PTCompra;
                parametros.PTNeutralizador = model.PTNeutralizador;
                parametros.PTTipoProducto = model.PTTipoProducto;
                parametros.PTLotes = model.PTLotes;
                parametros.PTVisitas = model.PTVisitas;
                parametros.PTCeldas = model.PTCeldas;
                parametros.PTProductosEvaluar = model.PTProductosEvaluar;
                parametros.ParTipoCLT = model.ParTipoCLT;
                parametros.ParAlquilerEquipos = model.ParAlquilerEquipos;
                parametros.ParApoyoLogistico = model.ParApoyoLogistico;
                parametros.ParAccesoInternet = model.ParAccesoInternet;
                parametros.ParPorcentajeIntercep = model.ParPorcentajeIntercep;
                parametros.ParPorcentajeRecluta = model.ParPorcentajeRecluta;
                parametros.ParEncuestadoresPunto = model.ParEncuestadoresPunto;
                parametros.ParObservaciones = model.ParObservaciones?.Trim();
                parametros.ParUsaTablet = model.ParUsaTablet ?? true;
                parametros.ParUsaPapel = model.ParUsaPapel ?? false;
                parametros.ParDispPropio = model.ParDispPropio;

                // Calcular total preguntas
                var totalPreguntas = (model.PregCerradas ?? 0) + (model.PregCerradasMultiples ?? 0) 
                    + (model.PregAbiertas ?? 0) + (model.PregAbiertasMultiples ?? 0) 
                    + (model.PregOtras ?? 0) + (model.PregDemograficos ?? 0);
                parametros.ParTotalPreguntas = totalPreguntas;

                // 2. Guardar/actualizar IQ_Preguntas
                var preguntas = context.IQPreguntas
                    .FirstOrDefault(p => p.IdPropuesta == model.IdPropuesta 
                        && p.ParAlternativa == model.ParAlternativa 
                        && p.MetCodigo == model.MetCodigo 
                        && p.ParNacional == model.ParNacional);

                if (preguntas == null)
                {
                    preguntas = new IQ_Preguntas
                    {
                        IdPropuesta = model.IdPropuesta,
                        ParAlternativa = model.ParAlternativa,
                        MetCodigo = model.MetCodigo,
                        ParNacional = model.ParNacional
                    };
                    context.IQPreguntas.Add(preguntas);
                }

                preguntas.PregCerradas = model.PregCerradas ?? 0;
                preguntas.PregCerradasMultiples = model.PregCerradasMultiples ?? 0;
                preguntas.PregAbiertas = model.PregAbiertas ?? 0;
                preguntas.PregAbiertasMultiples = model.PregAbiertasMultiples ?? 0;
                preguntas.PregOtras = model.PregOtras ?? 0;
                preguntas.PregDemograficos = model.PregDemograficos ?? 15; // Default

                context.SaveChanges();
                transaction.Commit();

                return (true, "Presupuesto guardado exitosamente");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return (false, $"Error guardando presupuesto: {ex.Message}");
            }
        }

        /// <summary>
        /// Agrega una línea de muestra
        /// </summary>
        public (bool success, string message) AgregarMuestra(MuestraItemViewModel muestra)
        {
            using var context = CreateContext();

            try
            {
                var entidad = new IQ_Muestra_1
                {
                    IdPropuesta = muestra.IdPropuesta,
                    ParAlternativa = muestra.ParAlternativa,
                    MetCodigo = muestra.MetCodigo,
                    CiuCodigo = muestra.CiuCodigo,
                    MuIdentificador = muestra.MuIdentificador,
                    ParNacional = muestra.ParNacional,
                    DeptCodigo = muestra.DeptCodigo ?? 0,
                    MuCantidad = muestra.MuCantidad
                };

                context.IQMuestra.Add(entidad);
                context.SaveChanges();

                return (true, "Muestra agregada");
            }
            catch (Exception ex)
            {
                return (false, $"Error agregando muestra: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina una línea de muestra
        /// </summary>
        public (bool success, string message) EliminarMuestra(long idPropuesta, int alternativa, int metCodigo, int ciuCodigo, int muIdentificador, int nacional)
        {
            using var context = CreateContext();

            try
            {
                var muestra = context.IQMuestra
                    .FirstOrDefault(m => m.IdPropuesta == idPropuesta 
                        && m.ParAlternativa == alternativa 
                        && m.MetCodigo == metCodigo 
                        && m.CiuCodigo == ciuCodigo 
                        && m.MuIdentificador == muIdentificador 
                        && m.ParNacional == nacional);

                if (muestra != null)
                {
                    context.IQMuestra.Remove(muestra);
                    context.SaveChanges();
                    return (true, "Muestra eliminada");
                }

                return (false, "Muestra no encontrada");
            }
            catch (Exception ex)
            {
                return (false, $"Error eliminando muestra: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina un presupuesto completo (cascade)
        /// </summary>
        public (bool success, string message) EliminarPresupuesto(long idPropuesta, int alternativa, int metCodigo, int nacional)
        {
            using var context = CreateContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                // Eliminar muestra
                var muestra = context.IQMuestra
                    .Where(m => m.IdPropuesta == idPropuesta 
                        && m.ParAlternativa == alternativa 
                        && m.MetCodigo == metCodigo 
                        && m.ParNacional == nacional);
                context.IQMuestra.RemoveRange(muestra);

                // Eliminar preguntas
                var preguntas = context.IQPreguntas
                    .FirstOrDefault(p => p.IdPropuesta == idPropuesta 
                        && p.ParAlternativa == alternativa 
                        && p.MetCodigo == metCodigo 
                        && p.ParNacional == nacional);
                if (preguntas != null)
                    context.IQPreguntas.Remove(preguntas);

                // Eliminar procesos
                var procesos = context.IQProcesos
                    .Where(p => p.IdPropuesta == idPropuesta 
                        && p.ParAlternativa == alternativa 
                        && p.MetCodigo == metCodigo 
                        && p.ParNacional == nacional);
                context.IQProcesos.RemoveRange(procesos);

                // Eliminar parámetros (principal)
                var parametros = context.IQParametros
                    .FirstOrDefault(p => p.IdPropuesta == idPropuesta 
                        && p.ParAlternativa == alternativa 
                        && p.MetCodigo == metCodigo 
                        && p.ParNacional == nacional);

                if (parametros != null)
                    context.IQParametros.Remove(parametros);

                context.SaveChanges();
                transaction.Commit();

                return (true, "Presupuesto eliminado");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return (false, $"Error eliminando presupuesto: {ex.Message}");
            }
        }
    }
}
