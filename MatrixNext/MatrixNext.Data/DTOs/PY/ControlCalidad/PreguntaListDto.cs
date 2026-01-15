namespace MatrixNext.Data.DTOs.PY.ControlCalidad
{
    /// <summary>
    /// DTO para listar Preguntas de evaluaciÃ³n
    /// </summary>
    public class PreguntaListDto
    {
        public long IdPregunta { get; set; }

        public long Id
        {
            get => IdPregunta;
            set => IdPregunta = value;
        }
        
        public int IdProceso { get; set; }

        public int TipoProceso
        {
            get => IdProceso;
            set => IdProceso = value;
        }
        
        public string Pregunta { get; set; }
        
        public bool Activa { get; set; }

        public bool EsActiva
        {
            get => Activa;
            set => Activa = value;
        }

        public string? RegistradoPor { get; set; }

        public DateTime? FechaRegistro { get; set; }
        
        public string NombreProceso { get; set; }
        
        public int Orden { get; set; }
    }
}

