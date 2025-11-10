using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace MatrixMoveClosedWorks
{
    class DAL
    {
        enum EEstadosTrabajoMatrix
        {
            centroInformacion = 14
        }

        string c = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;


        private string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public List<PY_TrabajoMoverCentroInformacion> obtenerTrabajosParaMover()
        {
            List<PY_TrabajoMoverCentroInformacion> lstTrabajosMover = new List<PY_TrabajoMoverCentroInformacion>();

            try
            {
                SqlDataReader Lector = null;

                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "PY_TrabajosMoverCentroInformacion";
                        cmd.CommandType = CommandType.StoredProcedure;
                        Lector = cmd.ExecuteReader();

                        while (Lector.Read())
                        {
                            lstTrabajosMover.Add(cargarTrabajoMover(Lector));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                
                this.TraceService("Error al obtener los trabajos a cerrar" + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return lstTrabajosMover;
        }

        private PY_TrabajoMoverCentroInformacion cargarTrabajoMover(SqlDataReader lector)
        {
            PY_TrabajoMoverCentroInformacion trabajoMover = new PY_TrabajoMoverCentroInformacion();
            trabajoMover.JBE = lector["JBE"].ToString();
            trabajoMover.JBP = lector["JBP"].ToString();
            trabajoMover.trabajoId = (long)lector["id"];
            trabajoMover.emailGP = lector["emailGP"].ToString();
            trabajoMover.emailCOE = lector["emailCOE"].ToString();
            trabajoMover.grupoUnidad = lector["GrupoUnidad"].ToString();
            return trabajoMover;
        }
        private constante cargarConstante(SqlDataReader lector)
        {
            constante oConstante = new constante();
            oConstante.id = (int)lector["id"];
            oConstante.Descripcion = lector["Descripcion"].ToString();
            oConstante.Valor = lector["Valor"].ToString();
            oConstante.Adicional = lector["Valor"].ToString();
            return oConstante;
        }
        public constante obtenerConstante(constante.EConstante id)
        {
            constante oConstante = new constante();

            try
            {
                SqlDataReader Lector = null;

                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "_Constantes_Get";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters[0].SqlDbType = SqlDbType.Int;
                        Lector = cmd.ExecuteReader();

                        while (Lector.Read())
                        {
                            oConstante = cargarConstante(Lector);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.TraceService("Error al obtener las constantes" + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return oConstante;
        }

        public void guardarLogMoverCarpetaTrabajos(logMoverCarpeta log)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "CI_LogMoverCarpetaMatrixTrabajosCerrados_Add";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@trabajoId", SqlDbType = SqlDbType.BigInt, Value = log.trabajoId });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@fechaInicio", SqlDbType = SqlDbType.DateTime, Value = log.fechaInicio });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@fechaFin", SqlDbType = SqlDbType.DateTime, Value = log.fechaFin });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@rutaOriginal", SqlDbType = SqlDbType.VarChar, Value = log.rutaOriginal, Size = -1 });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@rutaDestino", SqlDbType = SqlDbType.VarChar, Value = log.rutaDestino, Size = -1 });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@errorCopiar", SqlDbType = SqlDbType.Bit, Value = log.errorCopiar });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@descripcionErrorCopiar", SqlDbType = SqlDbType.VarChar, Value = (log.descripcionErrorCopiar == "") ? (object)DBNull.Value:(object)log.descripcionErrorCopiar, Size = -1 });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@errorBorrar", SqlDbType = SqlDbType.Bit, Value = log.errorBorrar });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@descripcionErrorBorrar", SqlDbType = SqlDbType.VarChar, Value = (log.descripcionErrorCopiar == "") ? (object)DBNull.Value : (object)log.descripcionErrorBorrar, Size = -1 });

                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                this.TraceService("Error al obtener los trabajos a cerrar" + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }

        private void TraceService(string content)
        {
            FileStream fs = new FileStream(@"C:\MatrixMoveClosedWorksServiceLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0L, SeekOrigin.End);
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();
        }

        public void actualizarEstadoTrabajoACentroInformacion(decimal trabajoId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "PY_Trabajo_Edit";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value = trabajoId });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@ProyectoId", SqlDbType = SqlDbType.BigInt, Value = DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@OP_MetodologiaId", SqlDbType = SqlDbType.Int, Value = DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@PresupuestoId", SqlDbType = SqlDbType.BigInt, Value = DBNull.Value, Size = -1 });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@NombreTrabajo", SqlDbType = SqlDbType.VarChar, Value = DBNull.Value, Size = 250 });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@Muestra", SqlDbType = SqlDbType.BigInt, Value = DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@FechaTentativaInicioCampo", SqlDbType = SqlDbType.Date, Value = DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@FechaTentativaFinalizacion", SqlDbType = SqlDbType.Date, Value = DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@COE", SqlDbType = SqlDbType.BigInt, Value = DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@Unidad", SqlDbType = SqlDbType.Int, Value = DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@JobBook", SqlDbType = SqlDbType.VarChar, Value = DBNull.Value, Size=20 });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@TipoRecoleccion", SqlDbType = SqlDbType.TinyInt, Value = DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@Estado", SqlDbType = SqlDbType.TinyInt, Value = EEstadosTrabajoMatrix.centroInformacion });
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                this.TraceService("Error al actualizar el estado del trabajo" + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }
    }
    class constante
    {
        public enum EConstante
        {
            UsuarioArchivos = 1,
            ContrasenaArchivos = 2,
            RutaArchivosMatrix = 3,
            RutaCentroInformacion = 4,
            horaServicioMueveTrabajosCerrados = 5,
            horaServicioIndexaTrabajosCerrados = 6
        };
        public int id { get; set; }
        public string Descripcion { get; set; }

        public string Valor { get; set; }

        public string Adicional { get; set; }

    }
    class PY_TrabajoMoverCentroInformacion
    {
        public string JBE { get; set; }
        public string JBP { get; set; }
        public decimal trabajoId { get; set; }
        public string emailGP { get; set; }
        public string emailCOE { get; set; }
        public string grupoUnidad { get; set; }

    }

    class logMoverCarpeta
    {
        public decimal id { get; set; }
        public decimal trabajoId { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string rutaOriginal { get; set; }
        public string rutaDestino { get; set; }
        public bool errorCopiar { get; set; }
        public string descripcionErrorCopiar { get; set; }
        public bool errorBorrar { get; set; }
        public string descripcionErrorBorrar { get; set; }


    }
}
