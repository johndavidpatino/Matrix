// MatrixNext.Core/DTOs/RE_GT/MenuItemDto.cs
// DTO genérico para items de menú de navegación (reutilizable para landing pages)

namespace MatrixNext.Core.DTOs.RE_GT
{
    /// <summary>
    /// DTO para items de menú en páginas de navegación
    /// Usado por RecoleccionDatos, GestionTratamiento landing pages
    /// </summary>
    public class MenuItemDto
    {
        /// <summary>Identificador único del item</summary>
        public int Id { get; set; }

        /// <summary>Nombre/Título del item</summary>
        public string Titulo { get; set; }

        /// <summary>URL destino</summary>
        public string Url { get; set; }

        /// <summary>Descripción tooltip</summary>
        public string Descripcion { get; set; }

        /// <summary>Icono CSS class (Font Awesome)</summary>
        public string IconoCss { get; set; }

        /// <summary>Orden de visualización</summary>
        public int Orden { get; set; }

        /// <summary>Sección/Grupo al que pertenece</summary>
        public string Seccion { get; set; }

        /// <summary>Indica si está habilitado</summary>
        public bool Habilitado { get; set; } = true;
    }

    /// <summary>
    /// DTO para agrupar items por sección
    /// </summary>
    public class MenuSeccionDto
    {
        /// <summary>Nombre de la sección</summary>
        public string Nombre { get; set; }

        /// <summary>Descripción de la sección</summary>
        public string Descripcion { get; set; }

        /// <summary>Icono de la sección</summary>
        public string IconoCss { get; set; }

        /// <summary>Items dentro de la sección</summary>
        public List<MenuItemDto> Items { get; set; } = new();

        /// <summary>Orden de visualización</summary>
        public int Orden { get; set; }
    }

    /// <summary>
    /// DTO específico para página de Recolección de Datos
    /// </summary>
    public class RecoleccionDatosMenuDto
    {
        /// <summary>Título de la página</summary>
        public string TituloPagina { get; set; } = "Recolección de Datos";

        /// <summary>Descripción general</summary>
        public string Descripcion { get; set; } = "Acceso a funcionalidades de recolección de datos operacional";

        /// <summary>Secciones del menú agrupadas</summary>
        public List<MenuSeccionDto> Secciones { get; set; } = new();

        /// <summary>Permiso requerido (legacy: 26)</summary>
        public int PermisoRequerido { get; set; } = 26;

        /// <summary>Indica si el usuario tiene acceso</summary>
        public bool TieneAcceso { get; set; } = true;
    }

    /// <summary>
    /// DTO para página de Gestión y Tratamiento de Datos
    /// </summary>
    public class GestionTratamientoDatosMenuDto
    {
        /// <summary>Título de la página</summary>
        public string TituloPagina { get; set; } = "Gestión y Tratamiento de Datos";

        /// <summary>Descripción general</summary>
        public string Descripcion { get; set; } = "Acceso a funcionalidades de gestión y tratamiento de información";

        /// <summary>Secciones del menú</summary>
        public List<MenuSeccionDto> Secciones { get; set; } = new();

        /// <summary>Permiso requerido</summary>
        public int PermisoRequerido { get; set; } = 27; // Ajustar según DB

        /// <summary>Acceso del usuario</summary>
        public bool TieneAcceso { get; set; } = true;
    }
}
