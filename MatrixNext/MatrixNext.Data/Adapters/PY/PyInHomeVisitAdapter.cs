using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.PY
{
    public class PyInHomeVisitAdapter : IPyInHomeVisitAdapter
    {
        private readonly string _connectionString;
        private readonly MatrixDbContext _context;

        public PyInHomeVisitAdapter(IConfiguration config, MatrixDbContext context)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
            _context = context;
        }

        /// <summary>
        /// Obtiene lista de InHome por trabajo usando SP
        /// SP: OP_MuestraTrabajosCuali_InHomeGet(@Id INT, @TrabajoId BIGINT)
        /// </summary>
        public async Task<List<InHomeVisitDto>> ObtenerInHomesPorTrabajo(long trabajoId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@Id", null);
            parametros.Add("@TrabajoId", trabajoId);

            var resultado = await connection.QueryAsync<InHomeVisitDto>(
                "OP_MuestraTrabajosCuali_InHomeGet",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        /// <summary>
        /// Obtiene InHome por ID usando SP
        /// </summary>
        public async Task<InHomeVisitDto?> ObtenerInHomePorId(long id)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@Id", id);
            parametros.Add("@TrabajoId", null);

            var resultado = await connection.QueryFirstOrDefaultAsync<InHomeVisitDto>(
                "OP_MuestraTrabajosCuali_InHomeGet",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado;
        }

        /// <summary>
        /// Obtiene log de cambios InHome usando SP
        /// SP: OP_LogInHomeCuali_Get(@IdSesion BIGINT)
        /// </summary>
        public async Task<List<LogInHomeDto>> ObtenerLogInHome(long idInHome)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@IdSesion", idInHome);

            var resultado = await connection.QueryAsync<LogInHomeDto>(
                "OP_LogInHomeCuali_Get",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        /// <summary>
        /// Guarda o actualiza InHome usando EF Core (no hay SP Add en legacy)
        /// Entidad: OP_MuestraTrabajosCuali_InHome
        /// </summary>
        public async Task<long> GuardarInHome(InHomeVisitInputDto input)
        {
            // TODO: Registrar entidad OP_MuestraTrabajosCuali_InHome en MatrixDbContext
            // Por ahora retornar ID temporal
            await Task.CompletedTask;
            return input.Id ?? 0;
            
            /*
            var entidad = input.Id.HasValue
                ? await _context.Set<Entities.OP_MuestraTrabajosCuali_InHome>()
                    .FirstOrDefaultAsync(x => x.Id == input.Id.Value)
                : null;

            if (entidad == null)
            {
                // Crear nueva
                entidad = new Entities.OP_MuestraTrabajosCuali_InHome
                {
                    TrabajoId = input.TrabajoId,
                    SegmentoId = input.SegmentoId,
                    CiudadId = input.CiudadId,
                    Moderador = input.Moderador,
                    GrupoObjetivo = input.GrupoObjetivo,
                    CantidadVisitas = input.CantidadVisitas,
                    Direccion = input.Direccion,
                    FechaInicio = !string.IsNullOrWhiteSpace(input.FechaInicio) 
                        ? DateTime.Parse(input.FechaInicio) 
                        : null,
                    FechaFin = !string.IsNullOrWhiteSpace(input.FechaFin) 
                        ? DateTime.Parse(input.FechaFin) 
                        : null,
                    Honorarios = input.Honorarios,
                    Gastos = input.Gastos,
                    Otros = input.Otros,
                    Observaciones = input.Observaciones
                };

                _context.Set<Entities.OP_MuestraTrabajosCuali_InHome>().Add(entidad);
            }
            else
            {
                // Actualizar existente
                entidad.SegmentoId = input.SegmentoId;
                entidad.CiudadId = input.CiudadId;
                entidad.Moderador = input.Moderador;
                entidad.GrupoObjetivo = input.GrupoObjetivo;
                entidad.CantidadVisitas = input.CantidadVisitas;
                entidad.Direccion = input.Direccion;
                entidad.FechaInicio = !string.IsNullOrWhiteSpace(input.FechaInicio) 
                    ? DateTime.Parse(input.FechaInicio) 
                    : null;
                entidad.FechaFin = !string.IsNullOrWhiteSpace(input.FechaFin) 
                    ? DateTime.Parse(input.FechaFin) 
                    : null;
                entidad.Honorarios = input.Honorarios;
                entidad.Gastos = input.Gastos;
                entidad.Otros = input.Otros;
                entidad.Observaciones = input.Observaciones;
            }

            await _context.SaveChangesAsync();
            return entidad.Id;
            */
        }

        /// <summary>
        /// Guarda log de cambios usando EF Core
        /// Entidad: OP_LogInHomeCuali
        /// </summary>
        public async Task GuardarLogInHome(long idInHome, long trabajoId, string usuario, string estado, string observacion)
        {
            // TODO: Registrar entidad OP_LogInHomeCuali en MatrixDbContext
            await Task.CompletedTask;
            /*
            var log = new Entities.OP_LogInHomeCuali
            {
                IdInHome = idInHome,
                IdTrabajo = trabajoId,
                Fecha = DateTime.Now,
                Usuario = usuario,
                Estado = estado,
                Observacion = observacion
            };

            _context.Set<Entities.OP_LogInHomeCuali>().Add(log);
            await _context.SaveChangesAsync();
            */
        }
    }
}
