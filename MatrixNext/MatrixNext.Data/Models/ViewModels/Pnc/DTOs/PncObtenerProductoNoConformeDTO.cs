namespace MatrixNext.Data.Models.ViewModels.Pnc
{
    /// <summary>
    /// DTO para resultado del SP PNC_ObtenerProductoNoConforme
    /// Devuelve PNC con información relacionada (joins)
    /// </summary>
    public class PncObtenerProductoNoConformeDTO
    {
        public int Id { get; set; }
        public int? IdEstudio { get; set; }
        public int? IdTrabajo { get; set; }
        public string? JobBook { get; set; }
        public DateTime? FechaReclamo { get; set; }
        public long? IdReporta { get; set; }
        public int? IdUnidad { get; set; }
        public long? IdClienteExterno { get; set; }
        public int? FuenteReclamo { get; set; }
        public int? Categoria { get; set; }
        public int? Tarea { get; set; }
        public string? Descripcion { get; set; }
        public bool? Cerrado { get; set; }
        public DateTime? FechaCierre { get; set; }
        public long? Usuario { get; set; }
        public DateTime? FechaGrabacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        // Campos calculados del SP (joins)
        public string? NombreEstudio { get; set; }
        public string? NombreReporta { get; set; }
        public string? NombreUnidad { get; set; }
        public string? NombreCliente { get; set; }
        public string? DescripcionFuenteReclamo { get; set; }
        public string? DescripcionCategoria { get; set; }
    }
}
