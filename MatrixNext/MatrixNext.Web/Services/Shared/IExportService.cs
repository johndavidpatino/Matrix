using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.Shared
{
    /// <summary>
    /// Servicio para exportación de datos a Excel, PDF y otros formatos.
    /// </summary>
    public interface IExportService
    {
        /// <summary>
        /// Exporta una lista de datos genérica a formato Excel usando ClosedXML.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto a exportar</typeparam>
        /// <param name="data">Lista de datos a exportar</param>
        /// <param name="nombreArchivo">Nombre del archivo sin extensión</param>
        /// <param name="nombreHoja">Nombre de la hoja de Excel (por defecto "Datos")</param>
        /// <param name="tituloReporte">Título opcional que aparece en la primera fila</param>
        /// <returns>Byte array con el archivo Excel generado</returns>
        Task<byte[]> ExportarExcelAsync<T>(
            List<T> data,
            string nombreArchivo,
            string nombreHoja = "Datos",
            string tituloReporte = null) where T : class;

        /// <summary>
        /// Exporta datos con configuración personalizada de columnas.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto a exportar</typeparam>
        /// <param name="data">Lista de datos a exportar</param>
        /// <param name="nombreArchivo">Nombre del archivo sin extensión</param>
        /// <param name="configuracionColumnas">Diccionario con nombres de propiedades y encabezados personalizados</param>
        /// <param name="nombreHoja">Nombre de la hoja de Excel</param>
        /// <param name="tituloReporte">Título opcional del reporte</param>
        /// <returns>Byte array con el archivo Excel generado</returns>
        Task<byte[]> ExportarExcelPersonalizadoAsync<T>(
            List<T> data,
            string nombreArchivo,
            Dictionary<string, string> configuracionColumnas,
            string nombreHoja = "Datos",
            string tituloReporte = null) where T : class;

        /// <summary>
        /// Exporta múltiples hojas en un solo archivo Excel.
        /// </summary>
        /// <param name="hojas">Diccionario donde la clave es el nombre de la hoja y el valor es una lista de objetos</param>
        /// <param name="nombreArchivo">Nombre del archivo sin extensión</param>
        /// <returns>Byte array con el archivo Excel generado</returns>
        Task<byte[]> ExportarExcelMultiHojasAsync(
            Dictionary<string, object> hojas,
            string nombreArchivo);
    }
}
