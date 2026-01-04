namespace MatrixNext.Data.Entities
{
    public class IQ_TecnicaProcesos
    {
        public int ProcCodigo { get; set; }
        public int TecCodigo { get; set; }

        public virtual IQ_Procesos? IQ_Procesos { get; set; }
    }
}
