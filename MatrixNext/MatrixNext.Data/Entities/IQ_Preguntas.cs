namespace MatrixNext.Data.Entities
{
    public class IQ_Preguntas
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public int MetCodigo { get; set; }
        public int PregCerradas { get; set; }
        public int PregCerradasMultiples { get; set; }
        public int PregAbiertas { get; set; }
        public int PregAbiertasMultiples { get; set; }
        public int PregOtras { get; set; }
        public int PregDemograficos { get; set; }
        public int ParNacional { get; set; }

        public virtual IQ_Parametros? IQ_Parametros { get; set; }
    }
}
