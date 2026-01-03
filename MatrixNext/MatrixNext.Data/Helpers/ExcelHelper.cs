using ClosedXML.Excel;
using System.Data;

namespace MatrixNext.Data.Helpers
{
    /// <summary>
    /// Helper para generación de archivos Excel usando ClosedXML
    /// Basado en la funcionalidad de Utilidades.ResponseExcel del sistema legacy
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// Genera un archivo Excel a partir de una colección de datos
        /// </summary>
        /// <typeparam name="T">Tipo de datos</typeparam>
        /// <param name="data">Colección de datos</param>
        /// <param name="sheetName">Nombre de la hoja</param>
        /// <param name="columnNames">Nombres de columnas separados por punto y coma (opcional)</param>
        /// <returns>Stream con el archivo Excel generado</returns>
        public static MemoryStream GenerateExcel<T>(
            IEnumerable<T> data, 
            string sheetName, 
            string? columnNames = null)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            if (data == null || !data.Any())
            {
                // Crear hoja vacía con mensaje
                worksheet.Cell(1, 1).Value = "No se encontraron datos";
                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;
                return stream;
            }

            // Convertir los datos a DataTable para facilitar el manejo
            var dataTable = ToDataTable(data);

            // Si se especifican nombres de columnas personalizados, usarlos
            if (!string.IsNullOrWhiteSpace(columnNames))
            {
                var customColumns = columnNames.Split(';')
                    .Select(c => c.Trim())
                    .Where(c => !string.IsNullOrEmpty(c))
                    .ToArray();

                // Filtrar y reordenar columnas según la especificación
                var filteredTable = new DataTable();
                foreach (var colName in customColumns)
                {
                    if (dataTable.Columns.Contains(colName))
                    {
                        filteredTable.Columns.Add(colName, dataTable.Columns[colName]!.DataType);
                    }
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    var newRow = filteredTable.NewRow();
                    foreach (var colName in customColumns)
                    {
                        if (dataTable.Columns.Contains(colName))
                        {
                            newRow[colName] = row[colName];
                        }
                    }
                    filteredTable.Rows.Add(newRow);
                }

                dataTable = filteredTable;
            }

            // Insertar la tabla en la hoja
            var table = worksheet.Cell(1, 1).InsertTable(dataTable);

            // Aplicar estilos al encabezado
            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Autoajustar columnas
            worksheet.Columns().AdjustToContents();

            // Aplicar bordes a toda la tabla
            table.Theme = XLTableTheme.TableStyleMedium2;

            // Guardar en stream
            var resultStream = new MemoryStream();
            workbook.SaveAs(resultStream);
            resultStream.Position = 0;
            return resultStream;
        }

        /// <summary>
        /// Convierte una colección de objetos a DataTable
        /// </summary>
        private static DataTable ToDataTable<T>(IEnumerable<T> data)
        {
            var dataTable = new DataTable();
            var properties = typeof(T).GetProperties();

            // Crear columnas
            foreach (var prop in properties)
            {
                var propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                dataTable.Columns.Add(prop.Name, propertyType);
            }

            // Agregar filas
            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(item);
                    row[prop.Name] = value ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        /// <summary>
        /// Convierte un DataTable a bytes de Excel
        /// </summary>
        public static byte[] GenerateExcelBytes(DataTable dataTable, string sheetName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);
            
            var table = worksheet.Cell(1, 1).InsertTable(dataTable);
            
            // Estilos
            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            
            worksheet.Columns().AdjustToContents();
            table.Theme = XLTableTheme.TableStyleMedium2;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
