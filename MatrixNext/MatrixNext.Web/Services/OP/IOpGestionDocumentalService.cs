using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Servicio para gestión documental de cierre de trabajos.
    /// Valida documentos escaneados en rutas UNC antes de cerrar un trabajo.
    /// </summary>
    public interface IOpGestionDocumentalService
    {
        /// <summary>
        /// Valida que todos los documentos requeridos para cierre estén escaneados.
        /// </summary>
        /// <param name="trabajoId">ID del trabajo a validar.</param>
        /// <param name="rolResponsableCierre">Rol del responsable de cierre (6 = Gerente Proyectos, 10 = Gerente Operaciones).</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>
        /// Tupla con:
        /// - TodosEncontrados: true si todos los documentos están escaneados
        /// - DocumentosFaltantes: Lista de nombres de documentos no encontrados (vacía si TodosEncontrados=true)
        /// </returns>
        /// <remarks>
        /// Utilizado en TrabajosController.CerrarTrabajo() antes de cambiar estado.
        /// Si hay documentos faltantes, se debe mostrar modal de confirmación para forzar cierre.
        /// </remarks>
        Task<(bool TodosEncontrados, List<string> DocumentosFaltantes)> ValidarDocumentosEscaneadosAsync(
            long trabajoId,
            int rolResponsableCierre,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene la lista detallada de documentos faltantes (no encontrados) para un trabajo.
        /// </summary>
        /// <param name="trabajoId">ID del trabajo.</param>
        /// <param name="rolResponsableCierre">Rol del responsable de cierre.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>
        /// Lista de ViewModels con información de documentos no encontrados:
        /// - IdDocumento, NombreDocumento, FechaEscaneo, Observacion
        /// </returns>
        /// <remarks>
        /// Utilizado para mostrar en modal de confirmación de cierre.
        /// </remarks>
        Task<List<DocumentoCierreVM>> ObtenerDocumentosFaltantesAsync(
            long trabajoId,
            int rolResponsableCierre,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Valida que las rutas UNC especificadas sean accesibles.
        /// </summary>
        /// <param name="rutas">Lista de rutas UNC a validar (ej: \\servidor\compartido\folder).</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>
        /// Dictionary con ruta como key y resultado de validación:
        /// - true: ruta accesible
        /// - false: ruta no accesible o error
        /// </returns>
        /// <remarks>
        /// Utilizado para diagnóstico de problemas de acceso a documentos.
        /// Requiere credenciales de red configuradas en appsettings.json.
        /// </remarks>
        Task<Dictionary<string, bool>> ValidarRutasUNCAsync(
            List<string> rutas,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sincroniza la tabla GD_EscanerDocumentos con los documentos reales en rutas UNC.
        /// </summary>
        /// <param name="trabajoId">ID del trabajo a sincronizar.</param>
        /// <param name="rolResponsableCierre">Rol del responsable de cierre.</param>
        /// <param name="servidor">Servidor UNC (ej: co-file04).</param>
        /// <param name="unidad">Unidad de red (ej: D$).</param>
        /// <param name="jbi">JobBook del estudio (ej: 2025-01).</param>
        /// <param name="nombreTrabajo">Nombre del trabajo.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>
        /// Número de documentos actualizados.
        /// </returns>
        /// <remarks>
        /// Equivalente a EscanerArchivos() en Trabajos.aspx.vb línea 145.
        /// Lee directorios UNC y actualiza campo Encontrado en GD_EscanerDocumentos.
        /// Requiere credenciales configuradas en appsettings.json.
        /// </remarks>
        Task<int> SincronizarDocumentosEscaneadosAsync(
            long trabajoId,
            int rolResponsableCierre,
            string servidor,
            string unidad,
            string jbi,
            string nombreTrabajo,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// ViewModel para documento de cierre faltante.
    /// </summary>
    public class DocumentoCierreVM
    {
        public long IdDocumento { get; set; }
        public string NombreDocumento { get; set; } = string.Empty;
        public bool Encontrado { get; set; }
        public DateTime? FechaEscaneo { get; set; }
        public string? Observacion { get; set; }
        public string? RutaUNC { get; set; }
    }
}
