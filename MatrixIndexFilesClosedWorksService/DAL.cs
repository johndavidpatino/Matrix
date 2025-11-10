using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace MatrixIndexFilesClosedWorks
{
    class DAL
    {
        enum EEstadosTrabajoMatrix
        {
            centroInformacion = 14
        }
        public enum EOrigenesIndexacion
        {
            CarpetasMatrix=1,
            CarpetasTrabajo=2
        }

        private string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public List<CI_IndexacionCarpetasCierreRaiz> obtenerCarpetasCierreIndexadas(EOrigenesIndexacion origenIndexacion)
        {
            List<CI_IndexacionCarpetasCierreRaiz> lstCarpetasCierreRaizIndexadas = new List<CI_IndexacionCarpetasCierreRaiz>();

            try
            {
                SqlDataReader Lector = null;

                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "CI_IndexacionCarpetasCierreRaiz_Get";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@origenIndexacion", Value = origenIndexacion, SqlDbType = SqlDbType.TinyInt });
                        Lector = cmd.ExecuteReader();

                        while (Lector.Read())
                        {
                            lstCarpetasCierreRaizIndexadas.Add(cargarCarpetaCierreRaizIndexada(Lector));
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                this.TraceService("Error al obtener las carpetas de cierre raiz indexadas " + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return lstCarpetasCierreRaizIndexadas;
        }

        public List<CI_IndexacionCarpetasCierreArchivos> obtenerArchivosIndexados(Int64 idCarpetaCierreRaiz)
        {
            List<CI_IndexacionCarpetasCierreArchivos> lstArchivosIndexados = new List<CI_IndexacionCarpetasCierreArchivos>();

            try
            {
                SqlDataReader Lector = null;

                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "CI_IndexacionCarpetasCierreArchivos_Get";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@carpetaRaizCierreId", SqlDbType = SqlDbType.BigInt, Value = idCarpetaCierreRaiz });
                        Lector = cmd.ExecuteReader();

                        while (Lector.Read())
                        {
                            lstArchivosIndexados.Add(cargarArchivoIndexado(Lector));
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                this.TraceService("Error al obtener las carpetas de cierre raiz indexadas " + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return lstArchivosIndexados;
        }

        public List<CI_OrigenesIndexacion> obtenerOrigenesIndexacion()
        {
            List<CI_OrigenesIndexacion> lstOrigenesIndexacion = new List<CI_OrigenesIndexacion>();

            try
            {
                SqlDataReader Lector = null;

                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SELECT * FROM CI_OrigenesIndexacion";
                        cmd.CommandType = CommandType.Text;
                        Lector = cmd.ExecuteReader();

                        while (Lector.Read())
                        {
                            lstOrigenesIndexacion.Add(cargarOrigenIndexacion(Lector));
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                this.TraceService("Error al obtener las carpetas de cierre raiz indexadas " + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return lstOrigenesIndexacion;
        }

        private CI_IndexacionCarpetasCierreRaiz cargarCarpetaCierreRaizIndexada(SqlDataReader lector)
        {
            CI_IndexacionCarpetasCierreRaiz carpetaCierreRaizIndexada = new CI_IndexacionCarpetasCierreRaiz();
            carpetaCierreRaizIndexada.id = (long)lector["id"];
            carpetaCierreRaizIndexada.ubicacion = lector["ubicacion"].ToString();
            carpetaCierreRaizIndexada.fechaCreacion = (DateTime)lector["fechaCreacion"];
            carpetaCierreRaizIndexada.fechaUltimaRevision = (DateTime)lector["fechaUltimaRevision"];
            carpetaCierreRaizIndexada.proyectoId = lector["proyectoId"] is DBNull ? new long?() : (long?)lector["proyectoId"];
            carpetaCierreRaizIndexada.trabajoId = lector["trabajoId"] is DBNull ? new long?() : (long?)lector["trabajoId"];
            return carpetaCierreRaizIndexada;
        }

        private CI_OrigenesIndexacion cargarOrigenIndexacion(SqlDataReader lector)
        {
            CI_OrigenesIndexacion origenIndexacion = new CI_OrigenesIndexacion();
            origenIndexacion.id = (byte)lector["id"];
            origenIndexacion.origen = lector["origen"].ToString();
            origenIndexacion.ruta = lector["ruta"].ToString();
            return origenIndexacion;
        }

        private CI_IndexacionCarpetasCierreArchivos cargarArchivoIndexado(SqlDataReader lector)
        {
            CI_IndexacionCarpetasCierreArchivos archivoIndexado = new CI_IndexacionCarpetasCierreArchivos();
            archivoIndexado.id = (long)lector["id"];
            archivoIndexado.ubicacion = lector["ubicacion"].ToString();
            archivoIndexado.fechaRegistro = (DateTime)lector["fechaRegistro"];
            archivoIndexado.carpetaRaizCierreId = (long)lector["carpetaRaizCierreId"];

            return archivoIndexado;
        }

        public long? obtenerIdTrabajoXJobBook(string jobBook)
        {
            long? idTrabajo = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SELECT id FROM PY_Trabajo PY_T WHERE PY_T.JobBook=@1";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@1", Value = jobBook, SqlDbType = SqlDbType.VarChar, Size = -1 });
                        idTrabajo = (long?)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {

                this.TraceService("Error al obtener las carpetas de cierre raiz indexadas " + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return idTrabajo;
        }

        public long? obtenerIdProyectoXJobBook(string jobBook)
        {
            long? idProyecto = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SELECT id FROM PY_Proyectos PY_T WHERE PY_T.JobBook=@1";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@1", Value = jobBook, SqlDbType = SqlDbType.VarChar, Size = -1 });
                        idProyecto = (long?)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {

                this.TraceService("Error al obtener las carpetas de cierre raiz indexadas " + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return idProyecto;
        }

        public long? obtenerIdProyectoXIdTrabajo(long idTrabajo)
        {
            long? idProyecto = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SELECT ProyectoId FROM PY_Trabajo PY_T WHERE PY_T.id=@1";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@1", Value = idTrabajo, SqlDbType = SqlDbType.BigInt});
                        idProyecto = (long?)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {

                this.TraceService("Error al obtener las carpetas de cierre raiz indexadas " + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return idProyecto;
        }
        public decimal guardarCarpetaCierreRaizIndexada(string ubicacion, DateTime fechaCreacion, DateTime fechaUltimaRevision, long? proyectoId, long? trabajoId, EOrigenesIndexacion origenIndexacion)
        {
            decimal id = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "CI_IndexacionCarpetasCierreRaiz_Add";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@ubicacion", SqlDbType = SqlDbType.NVarChar, Value = ubicacion, Size = -1 });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@fechaCreacion", SqlDbType = SqlDbType.DateTime, Value = fechaCreacion });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@fechaUltimaRevision", SqlDbType = SqlDbType.DateTime, Value = fechaUltimaRevision });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@proyectoId", SqlDbType = SqlDbType.BigInt, Value = (proyectoId.HasValue) ? (object)proyectoId.Value : DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@trabajoId", SqlDbType = SqlDbType.BigInt, Value = (trabajoId.HasValue) ? (object)trabajoId.Value : DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@origenIndexacion", SqlDbType = SqlDbType.TinyInt, Value = origenIndexacion });
                        id = (decimal)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                this.TraceService("Error al insertar el registro de la carpeta raiz " + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return id;
        }

        public void actualizarCarpetaCierreRaizIndexada(Int64 id, string ubicacion, DateTime? fechaCreacion, DateTime? fechaUltimaRevision, long? proyectoId, long? trabajoId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "CI_IndexacionCarpetasCierreRaiz_Edit";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@ubicacion", SqlDbType = SqlDbType.NVarChar, Value = (string.IsNullOrEmpty(ubicacion.Trim())) ? DBNull.Value : (object)ubicacion, Size = -1 });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@fechaCreacion", SqlDbType = SqlDbType.DateTime, Value = (fechaCreacion.HasValue) ? (object)fechaCreacion.Value : DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@fechaUltimaRevision", SqlDbType = SqlDbType.DateTime, Value = (fechaUltimaRevision.HasValue) ? (object)fechaUltimaRevision.Value : DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@proyectoId", SqlDbType = SqlDbType.BigInt, Value = (proyectoId.HasValue) ? (object)proyectoId.Value : DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@trabajoId", SqlDbType = SqlDbType.BigInt, Value = (trabajoId.HasValue) ? (object)trabajoId.Value : DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                this.TraceService("Error al insertar el registro de la carpeta raiz " + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }
        public void guardarRutaArchivosCarpetaCierre(decimal idRutaRaiz, List<string> rutasArchivos)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.strCon))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "CI_IndexacionCarpetasCierreArchivos_Add";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@carpetaRaizCierreId", SqlDbType = SqlDbType.BigInt, Value = idRutaRaiz });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@fechaRegistro", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now });
                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@ubicacion", SqlDbType = SqlDbType.NVarChar, Size = -1 });

                        for (int x = 0; x < rutasArchivos.Count; x++)
                        {
                            cmd.Parameters["@ubicacion"].Value = rutasArchivos[x];
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.TraceService("Error al insertar el registro de ruta del archivo " + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
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
        private void TraceService(string content)
        {
            FileStream fs = new FileStream(@"C:\MatrixMoveClosedWorksServiceLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0L, SeekOrigin.End);
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();
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
            horaServicioIndexaTrabajosCerrados=6
        };
        public int id { get; set; }
        public string Descripcion { get; set; }

        public string Valor { get; set; }

        public string Adicional { get; set; }

    }
    class CI_IndexacionCarpetasCierreRaiz
    {
        public long id { get; set; }
        public string ubicacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaUltimaRevision { get; set; }
        public long? proyectoId { get; set; }
        public long? trabajoId { get; set; }

    }

    class CI_IndexacionCarpetasCierreArchivos
    {
        public Int64 id { get; set; }
        public string ubicacion { get; set; }

        public DateTime fechaRegistro { get; set; }
        public Int64 carpetaRaizCierreId { get; set; }
    }

    class CI_OrigenesIndexacion
    {
        public byte id { get; set; }
        public string origen { get; set; }
        public string ruta { get; set; }
    }
}
