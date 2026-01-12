using MatrixNext.Web.Services.OP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.IO;

namespace MatrixNext.Web.Services.OP
{
    public class OpReportService : IOpReportService
    {
        private readonly IOpCualitativoService _cualitativoService;
        private readonly IOpFichasTecnicasService _fichasService;

        public OpReportService(
            IOpCualitativoService cualitativoService,
            IOpFichasTecnicasService fichasService)
        {
            _cualitativoService = cualitativoService;
            _fichasService = fichasService;
        }

        // ========== REPORTES DE SESIONES ==========

        public async Task<List<ReportSessionDto>> GetSessionsReportAsync(
            int? trabajoId = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            string estado = null,
            int pageNumber = 1,
            int pageSize = 50)
        {
            // TODO: Implementar query a BD que obtiene sesiones con filtros
            // Placeholder: retornar lista vacía por ahora
            return await Task.FromResult(new List<ReportSessionDto>());
        }

        public async Task<byte[]> ExportSessionsToExcelAsync(List<ReportSessionDto> sessions)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sesiones");

                // Headers
                worksheet.Cell(1, 1).Value = "Sesión ID";
                worksheet.Cell(1, 2).Value = "Trabajo";
                worksheet.Cell(1, 3).Value = "Fecha Inicio";
                worksheet.Cell(1, 4).Value = "Fecha Fin";
                worksheet.Cell(1, 5).Value = "Duración (min)";
                worksheet.Cell(1, 6).Value = "Ubicación";
                worksheet.Cell(1, 7).Value = "Estado";
                worksheet.Cell(1, 8).Value = "Participantes";
                worksheet.Cell(1, 9).Value = "Moderador";

                // Estilos header
                var headerRow = worksheet.Range("A1:I1");
                headerRow.Style.Fill.BackgroundColor = XLColor.DarkBlue;
                headerRow.Style.Font.FontColor = XLColor.White;
                headerRow.Style.Font.Bold = true;

                // Data rows
                int row = 2;
                foreach (var session in sessions)
                {
                    worksheet.Cell(row, 1).Value = session.SesionId;
                    worksheet.Cell(row, 2).Value = session.TrabajoCodigo;
                    worksheet.Cell(row, 3).Value = session.FechaInicio;
                    worksheet.Cell(row, 4).Value = session.FechaFin;
                    worksheet.Cell(row, 5).Value = session.Duracion;
                    worksheet.Cell(row, 6).Value = session.Ubicacion;
                    worksheet.Cell(row, 7).Value = session.Estado;
                    worksheet.Cell(row, 8).Value = session.NumeroParticipantes;
                    worksheet.Cell(row, 9).Value = session.Moderador;

                    row++;
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task<byte[]> ExportSessionsToPdfAsync(List<ReportSessionDto> sessions)
        {
            // TODO: Implementar exportación a PDF (requiere iTextSharp o similar)
            return await Task.FromResult(new byte[] { });
        }


        // ========== REPORTES DE ENTREVISTAS ==========

        public async Task<List<ReportInterviewDto>> GetInterviewsReportAsync(
            int? trabajoId = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            string estado = null,
            string entrevistador = null,
            int pageNumber = 1,
            int pageSize = 50)
        {
            // TODO: Implementar query a BD
            return await Task.FromResult(new List<ReportInterviewDto>());
        }

        public async Task<byte[]> ExportInterviewsToExcelAsync(List<ReportInterviewDto> interviews)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Entrevistas");

                // Headers
                worksheet.Cell(1, 1).Value = "Entrevista ID";
                worksheet.Cell(1, 2).Value = "Trabajo";
                worksheet.Cell(1, 3).Value = "Fecha";
                worksheet.Cell(1, 4).Value = "Entrevistador";
                worksheet.Cell(1, 5).Value = "Encuestado";
                worksheet.Cell(1, 6).Value = "Duración (min)";
                worksheet.Cell(1, 7).Value = "Preguntas";
                worksheet.Cell(1, 8).Value = "Completitud (%)";
                worksheet.Cell(1, 9).Value = "Estado";

                var headerRow = worksheet.Range("A1:I1");
                headerRow.Style.Fill.BackgroundColor = XLColor.DarkGreen;
                headerRow.Style.Font.FontColor = XLColor.White;
                headerRow.Style.Font.Bold = true;

                // Data rows
                int row = 2;
                foreach (var interview in interviews)
                {
                    worksheet.Cell(row, 1).Value = interview.EntrevistaId;
                    worksheet.Cell(row, 2).Value = interview.TrabajoCodigo;
                    worksheet.Cell(row, 3).Value = interview.FechaEjecucion;
                    worksheet.Cell(row, 4).Value = interview.Entrevistador;
                    worksheet.Cell(row, 5).Value = interview.Encuestado;
                    worksheet.Cell(row, 6).Value = interview.Duracion;
                    worksheet.Cell(row, 7).Value = $"{interview.PreguntasRespondidas}/{interview.Preguntas}";
                    worksheet.Cell(row, 8).Value = interview.Completitud;
                    worksheet.Cell(row, 9).Value = interview.Estado;

                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task<byte[]> ExportInterviewsToPdfAsync(List<ReportInterviewDto> interviews)
        {
            // TODO: Implementar exportación a PDF
            return await Task.FromResult(new byte[] { });
        }


        // ========== REPORTES DE MODERADORES ==========

        public async Task<List<ReportModeratorDto>> GetModeratorsReportAsync(
            int? trabajoId = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            // TODO: Implementar query agrupada por moderador
            return await Task.FromResult(new List<ReportModeratorDto>());
        }

        public async Task<byte[]> ExportModeratorsToExcelAsync(List<ReportModeratorDto> moderators)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Moderadores");

                // Headers
                worksheet.Cell(1, 1).Value = "Moderador ID";
                worksheet.Cell(1, 2).Value = "Nombre";
                worksheet.Cell(1, 3).Value = "Total Sesiones";
                worksheet.Cell(1, 4).Value = "Completadas";
                worksheet.Cell(1, 5).Value = "Horas";
                worksheet.Cell(1, 6).Value = "Promedio Participantes";
                worksheet.Cell(1, 7).Value = "Última Sesión";

                var headerRow = worksheet.Range("A1:G1");
                headerRow.Style.Fill.BackgroundColor = XLColor.DarkRed;
                headerRow.Style.Font.FontColor = XLColor.White;
                headerRow.Style.Font.Bold = true;

                // Data rows
                int row = 2;
                foreach (var moderator in moderators)
                {
                    worksheet.Cell(row, 1).Value = moderator.ModeradorId;
                    worksheet.Cell(row, 2).Value = moderator.Nombre;
                    worksheet.Cell(row, 3).Value = moderator.TotalSesiones;
                    worksheet.Cell(row, 4).Value = moderator.SesionesCompletadas;
                    worksheet.Cell(row, 5).Value = moderator.HorasTotal;
                    worksheet.Cell(row, 6).Value = Math.Round(moderator.PromedioParticipantes, 2);
                    worksheet.Cell(row, 7).Value = moderator.UltimaSesion;

                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }


        // ========== VALIDACIONES DE CONCURRENCIA ==========

        public async Task<bool> ValidateConcurrentSessionsAsync(
            int moderadorId,
            DateTime fechaInicio,
            DateTime fechaFin,
            int? sessionIdToExclude = null)
        {
            var concurrentSessions = await GetConcurrentSessionsAsync(moderadorId, fechaInicio, fechaFin);
            
            if (sessionIdToExclude.HasValue)
            {
                concurrentSessions = concurrentSessions
                    .Where(s => s.SesionId != sessionIdToExclude.Value)
                    .ToList();
            }

            // Si hay sesiones simultáneas, no es válido
            return concurrentSessions.Count == 0;
        }

        public async Task<List<ConcurrentSessionDto>> GetConcurrentSessionsAsync(
            int moderadorId,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            // TODO: Implementar query que busca sesiones del moderador que se solapan con el rango dado
            return await Task.FromResult(new List<ConcurrentSessionDto>());
        }
    }
}
