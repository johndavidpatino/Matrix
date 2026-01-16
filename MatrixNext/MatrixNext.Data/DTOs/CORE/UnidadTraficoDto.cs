// MatrixNext.Data/DTOs/CORE/UnidadTraficoDto.cs

using System.Collections.Generic;

namespace MatrixNext.Data.DTOs.CORE
{
    /// <summary>
    /// DTO para unidades OP en TraficoTareas
    /// Sprint 17 - RE_GT TraficoTareas
    /// </summary>
    public class UnidadTraficoDto
    {
        /// <summary>ID de unidad (5-14)</summary>
        public int Id { get; set; }

        /// <summary>Nombre de la unidad</summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Descripción de la unidad</summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>ID de permiso para validar acceso</summary>
        public int PermId { get; set; }

        /// <summary>Grupo de origen: "Gestión", "Recolección", "Estadística"</summary>
        public string GrupoOrigen { get; set; } = "Gestión";

        /// <summary>
        /// Factory: Obtiene lista completa de 10 unidades OP
        /// Mapeadas de TraficoTareas.aspx.vb (Page_Load, líneas 20-45)
        /// </summary>
        public static List<UnidadTraficoDto> ObtenerUnidadesTrafico()
        {
            return new List<UnidadTraficoDto>
            {
                new() { Id = 5, Nombre = "Crítica", Descripcion = "Unidad Crítica", PermId = 107, GrupoOrigen = "Gestión" },
                new() { Id = 6, Nombre = "Verificación", Descripcion = "Unidad de Verificación", PermId = 108, GrupoOrigen = "Gestión" },
                new() { Id = 7, Nombre = "Captura", Descripcion = "Unidad de Captura", PermId = 109, GrupoOrigen = "Gestión" },
                new() { Id = 8, Nombre = "Codificación", Descripcion = "Unidad de Codificación", PermId = 110, GrupoOrigen = "Gestión" },
                new() { Id = 9, Nombre = "Data Cleaning", Descripcion = "Unidad de Data Cleaning", PermId = 111, GrupoOrigen = "Gestión" },
                new() { Id = 10, Nombre = "Procesamiento", Descripcion = "Unidad de Procesamiento", PermId = 112, GrupoOrigen = "Gestión" },
                new() { Id = 11, Nombre = "Scripting", Descripcion = "Unidad de Scripting", PermId = 113, GrupoOrigen = "Recolección" },
                new() { Id = 12, Nombre = "Pilotos", Descripcion = "Unidad de Pilotos", PermId = 114, GrupoOrigen = "Recolección" },
                new() { Id = 13, Nombre = "Estadística", Descripcion = "Unidad de Estadística", PermId = 115, GrupoOrigen = "Estadística" },
                new() { Id = 14, Nombre = "Call Center", Descripcion = "Unidad Call Center", PermId = 116, GrupoOrigen = "Recolección" }
            };
        }
    }
}
