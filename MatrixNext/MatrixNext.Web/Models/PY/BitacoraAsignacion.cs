using System;
using System.Collections.Generic;

namespace MatrixNext.Web.Models.PY
{
    public class BitacoraAsignacion
    {
        public long IdAsignacion { get; set; }
        public List<RegistroBitacoraAsignacion> Registros { get; set; } = new();
    }

    public class RegistroBitacoraAsignacion
    {
        public DateTime FechaRegistro { get; set; }
        public string TipoOperacion { get; set; } = string.Empty;
        public string? GerenteAnterior { get; set; }
        public string? GerenteNuevo { get; set; }
        public string? Motivo { get; set; }
        public string? RegistradoPor { get; set; }
    }
}
