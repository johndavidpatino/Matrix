namespace MatrixNext.Data.Entities
{
    public class IQ_OpcionesAplicadas
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public int ParNacional { get; set; }
        public int MetCodigo { get; set; }
        public int IdOpcion { get; set; }
        public int TecCodigo { get; set; }
        public bool Aplica { get; set; }

        public virtual IQ_OpcionesTecnicas? IQ_OpcionesTecnicas { get; set; }
        public virtual IQ_Parametros? IQ_Parametros { get; set; }
    }
}
