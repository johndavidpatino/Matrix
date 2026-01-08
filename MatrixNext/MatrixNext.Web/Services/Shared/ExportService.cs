using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.Shared
{
    /// <summary>
    /// Implementación del servicio de exportación usando ClosedXML.
    /// </summary>
    public class ExportService : IExportService
    {
        private readonly ILogger<ExportService> _logger;

        public ExportService(ILogger<ExportService> logger)
        {
            _logger = logger;
        }

        public async Task<byte[]> ExportarExcelAsync<T>(
            List<T> data,
            string nombreArchivo,
            string nombreHoja = "Datos",
            string? tituloReporte = null) where T : class
        {
            return await Task.Run(() =>
            {
                try
                {
                    _logger.LogInformation("Iniciando exportación de {Count} registros a Excel: {NombreArchivo}", data.Count, nombreArchivo);

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add(nombreHoja);
                        var filaActual = 1;

                        // Agregar título si se proporciona
                        if (!string.IsNullOrEmpty(tituloReporte))
                        {
                            worksheet.Cell(filaActual, 1).Value = tituloReporte;
                            worksheet.Cell(filaActual, 1).Style.Font.Bold = true;
                            worksheet.Cell(filaActual, 1).Style.Font.FontSize = 14;
                            filaActual += 2; // Dejar una fila en blanco
                        }

                        // Obtener propiedades del tipo T
                        var propiedades = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        
                        if (propiedades.Length == 0)
                        {
                            throw new InvalidOperationException("El tipo no tiene propiedades públicas para exportar.");
                        }

                        // Crear encabezados
                        var filaEncabezado = filaActual;
                        for (int i = 0; i < propiedades.Length; i++)
                        {
                            var celda = worksheet.Cell(filaEncabezado, i + 1);
                            celda.Value = FormatearNombrePropiedad(propiedades[i].Name);
                            celda.Style.Font.Bold = true;
                            celda.Style.Fill.BackgroundColor = XLColor.LightGray;
                            celda.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                        filaActual++;

                        // Agregar datos
                        foreach (var item in data)
                        {
                            for (int i = 0; i < propiedades.Length; i++)
                            {
                                var valor = propiedades[i].GetValue(item);
                                var celda = worksheet.Cell(filaActual, i + 1);

                                if (valor != null)
                                {
                                    // Formatear según el tipo de dato
                                    if (valor is DateTime fecha)
                                    {
                                        celda.Value = fecha;
                                        celda.Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                                    }
                                    else if (valor is decimal || valor is double || valor is float)
                                    {
                                        celda.Value = Convert.ToDouble(valor);
                                        celda.Style.NumberFormat.Format = "#,##0.00";
                                    }
                                    else if (valor is int || valor is long)
                                    {
                                        celda.Value = Convert.ToInt64(valor);
                                    }
                                    else if (valor is bool booleano)
                                    {
                                        celda.Value = booleano ? "Sí" : "No";
                                    }
                                    else
                                    {
                                        celda.Value = valor.ToString();
                                    }
                                }
                                else
                                {
                                    celda.Value = string.Empty;
                                }
                            }
                            filaActual++;
                        }

                        // Ajustar ancho de columnas
                        worksheet.Columns().AdjustToContents();

                        // Congelar primera fila (encabezados)
                        var filaCongelar = string.IsNullOrEmpty(tituloReporte) ? 1 : 3;
                        worksheet.SheetView.FreezeRows(filaCongelar);

                        // Agregar autofiltro
                        var rangoTabla = worksheet.Range(filaEncabezado, 1, filaActual - 1, propiedades.Length);
                        rangoTabla.SetAutoFilter();

                        // Convertir a byte array
                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            _logger.LogInformation("Exportación exitosa: {NombreArchivo}.xlsx ({Size} bytes)", nombreArchivo, stream.Length);
                            return stream.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al exportar Excel: {NombreArchivo}", nombreArchivo);
                    throw;
                }
            });
        }

        public async Task<byte[]> ExportarExcelPersonalizadoAsync<T>(
            List<T> data,
            string nombreArchivo,
            Dictionary<string, string> configuracionColumnas,
            string nombreHoja = "Datos",
            string? tituloReporte = null) where T : class
        {
            return await Task.Run(() =>
            {
                try
                {
                    _logger.LogInformation("Iniciando exportación personalizada de {Count} registros: {NombreArchivo}", data.Count, nombreArchivo);

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add(nombreHoja);
                        var filaActual = 1;

                        // Agregar título si se proporciona
                        if (!string.IsNullOrEmpty(tituloReporte))
                        {
                            worksheet.Cell(filaActual, 1).Value = tituloReporte;
                            worksheet.Cell(filaActual, 1).Style.Font.Bold = true;
                            worksheet.Cell(filaActual, 1).Style.Font.FontSize = 14;
                            filaActual += 2;
                        }

                        var propiedades = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => configuracionColumnas.ContainsKey(p.Name))
                            .ToList();

                        // Crear encabezados personalizados
                        var filaEncabezado = filaActual;
                        for (int i = 0; i < propiedades.Count; i++)
                        {
                            var celda = worksheet.Cell(filaEncabezado, i + 1);
                            celda.Value = configuracionColumnas[propiedades[i].Name];
                            celda.Style.Font.Bold = true;
                            celda.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
                            celda.Style.Font.FontColor = XLColor.White;
                            celda.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        }
                        filaActual++;

                        // Agregar datos
                        foreach (var item in data)
                        {
                            for (int i = 0; i < propiedades.Count; i++)
                            {
                                var valor = propiedades[i].GetValue(item);
                                var celda = worksheet.Cell(filaActual, i + 1);

                                if (valor != null)
                                {
                                    if (valor is DateTime fecha)
                                    {
                                        celda.Value = fecha;
                                        celda.Style.DateFormat.Format = "dd/MM/yyyy";
                                    }
                                    else if (valor is decimal || valor is double || valor is float)
                                    {
                                        celda.Value = Convert.ToDouble(valor);
                                        celda.Style.NumberFormat.Format = "#,##0.00";
                                    }
                                    else if (valor is int || valor is long)
                                    {
                                        celda.Value = Convert.ToInt64(valor);
                                    }
                                    else if (valor is bool booleano)
                                    {
                                        celda.Value = booleano ? "Sí" : "No";
                                    }
                                    else
                                    {
                                        celda.Value = valor.ToString();
                                    }
                                }
                            }
                            filaActual++;
                        }

                        // Ajustar ancho de columnas
                        worksheet.Columns().AdjustToContents();

                        // Congelar encabezados
                        var filaCongelar = string.IsNullOrEmpty(tituloReporte) ? 1 : 3;
                        worksheet.SheetView.FreezeRows(filaCongelar);

                        // Agregar autofiltro
                        var rangoTabla = worksheet.Range(filaEncabezado, 1, filaActual - 1, propiedades.Count);
                        rangoTabla.SetAutoFilter();

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            return stream.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al exportar Excel personalizado: {NombreArchivo}", nombreArchivo);
                    throw;
                }
            });
        }

        public async Task<byte[]> ExportarExcelMultiHojasAsync(
            Dictionary<string, object> hojas,
            string nombreArchivo)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _logger.LogInformation("Iniciando exportación multi-hojas: {NombreArchivo} ({NumHojas} hojas)", nombreArchivo, hojas.Count);

                    using (var workbook = new XLWorkbook())
                    {
                        foreach (var hoja in hojas)
                        {
                            var nombreHoja = hoja.Key;
                            var datos = hoja.Value;

                            if (datos == null) continue;

                            var tipoLista = datos.GetType();
                            if (!tipoLista.IsGenericType) continue;

                            var tipoElemento = tipoLista.GetGenericArguments()[0];
                            var propiedades = tipoElemento.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                            var worksheet = workbook.Worksheets.Add(nombreHoja);
                            var filaActual = 1;

                            // Encabezados
                            for (int i = 0; i < propiedades.Length; i++)
                            {
                                var celda = worksheet.Cell(filaActual, i + 1);
                                celda.Value = FormatearNombrePropiedad(propiedades[i].Name);
                                celda.Style.Font.Bold = true;
                                celda.Style.Fill.BackgroundColor = XLColor.LightGray;
                            }
                            filaActual++;

                            // Datos
                            var lista = datos as System.Collections.IEnumerable;
                            if (lista == null) continue;
                            foreach (var item in lista)
                            {
                                for (int i = 0; i < propiedades.Length; i++)
                                {
                                    var valor = propiedades[i].GetValue(item);
                                    var celda = worksheet.Cell(filaActual, i + 1);

                                    if (valor != null)
                                    {
                                        if (valor is DateTime fecha)
                                        {
                                            celda.Value = fecha;
                                            celda.Style.DateFormat.Format = "dd/MM/yyyy";
                                        }
                                        else if (valor is decimal || valor is double || valor is float)
                                        {
                                            celda.Value = Convert.ToDouble(valor);
                                            celda.Style.NumberFormat.Format = "#,##0.00";
                                        }
                                        else
                                        {
                                            celda.Value = valor.ToString();
                                        }
                                    }
                                }
                                filaActual++;
                            }

                            worksheet.Columns().AdjustToContents();
                            worksheet.SheetView.FreezeRows(1);
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            return stream.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al exportar Excel multi-hojas: {NombreArchivo}", nombreArchivo);
                    throw;
                }
            });
        }

        /// <summary>
        /// Formatea nombres de propiedades en PascalCase a texto legible.
        /// Ejemplo: "FechaCreacion" -> "Fecha Creación"
        /// </summary>
        private string FormatearNombrePropiedad(string nombrePropiedad)
        {
            if (string.IsNullOrEmpty(nombrePropiedad))
                return nombrePropiedad;

            var resultado = string.Concat(
                nombrePropiedad.Select((c, i) =>
                    i > 0 && char.IsUpper(c) ? " " + c : c.ToString()
                )
            );

            return resultado;
        }
    }
}
