namespace MatrixNext.Data.Entities
{
    public class IQ_Muestra_1
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public int MetCodigo { get; set; }
        public int CiuCodigo { get; set; }
        public int MuIdentificador { get; set; }
        public int DeptCodigo { get; set; }
        public int MuCantidad { get; set; }
        public int ParNacional { get; set; }

        public virtual IQ_Parametros? IQ_Parametros { get; set; }
    }
}
