namespace MatrixNext.Web.Models.OP.Dtos
{
    /// <summary>
    /// DTO genérico para items de catálogos cascada (Unidades, Actividades, Subactividades).
    /// </summary>
    public class CatalogoItemDto
    {
        /// <summary>ID del item</summary>
        public int Id { get; set; }

        /// <summary>Nombre/Descripción del item</summary>
        public string Nombre { get; set; }

        /// <summary>Descripción adicional (opcional)</summary>
        public string Descripcion { get; set; }

        /// <summary>Si está activo para seleccionar</summary>
        public bool Activo { get; set; } = true;
    }
}
