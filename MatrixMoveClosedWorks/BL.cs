using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;



namespace MatrixMoveClosedWorks
{
    class BL
    {
        enum EFase
        {
            copiar = 1,
            borrar = 2
        }

        public void moverTrabajosCerrados()
        {
            string usuario;
            string contrasena;
            string rutaOrigen;
            string rutaOrigenCompleta;
            string rutaDestino;
            string rutaDestinoCompleta;
            EFase faseActual;
            logMoverCarpeta log = new logMoverCarpeta();
            List<PY_TrabajoMoverCentroInformacion> lstTrabajoMoverCI = new List<PY_TrabajoMoverCentroInformacion>();
            DAL oDAL = new DAL();

            usuario = oDAL.obtenerConstante(constante.EConstante.UsuarioArchivos).Valor;
            contrasena = oDAL.obtenerConstante(constante.EConstante.ContrasenaArchivos).Valor;
            rutaOrigen = oDAL.obtenerConstante(constante.EConstante.RutaArchivosMatrix).Valor;
            rutaDestino = oDAL.obtenerConstante(constante.EConstante.RutaCentroInformacion).Valor;

            lstTrabajoMoverCI = oDAL.obtenerTrabajosParaMover();
            using (NetworkConnection directory = new NetworkConnection(@"\\" + rutaDestino.Split(char.Parse(@"\"))[2], new NetworkCredential(usuario, contrasena)))
            {
                foreach (PY_TrabajoMoverCentroInformacion trabajo in lstTrabajoMoverCI)
                {
                    faseActual = EFase.copiar;
                    log.trabajoId = trabajo.trabajoId;
                    log.fechaInicio = DateTime.Now;
                    rutaOrigenCompleta = rutaOrigen + @"\" + trabajo.JBE.ToString() + @"\" + trabajo.JBP.ToString() + @"\" + trabajo.trabajoId.ToString();
                    rutaDestinoCompleta = rutaDestino + @"\Matrix\" + trabajo.trabajoId.ToString() + "-" + trabajo.JBP.ToString();
                    log.rutaOriginal = rutaOrigenCompleta;
                    log.rutaDestino = rutaDestinoCompleta;
                    try
                    {
                        copiarDirectorio(rutaOrigenCompleta, rutaDestinoCompleta);
                        log.errorCopiar = false;
                        log.descripcionErrorCopiar = "";
                        faseActual = EFase.borrar;
                        borrarDirectorioOrigen(rutaOrigenCompleta);
                        log.errorBorrar = false;
                        log.descripcionErrorCopiar = "";
                        oDAL.actualizarEstadoTrabajoACentroInformacion(trabajo.trabajoId);
                    }
                    catch (Exception ex)
                    {
                        switch (faseActual)
                        {
                            case EFase.copiar:
                                log.errorCopiar = true;
                                log.descripcionErrorCopiar = "Message=" + ex.Message + " - stackTrace=" + ex.StackTrace;
                                TraceService(DateTime.Now.ToString() + "-" + log.descripcionErrorCopiar);
                                break;
                            case EFase.borrar:
                                log.errorBorrar = true;
                                log.descripcionErrorBorrar = "Message=" + ex.Message + " - stackTrace=" + ex.StackTrace;
                                TraceService(DateTime.Now.ToString() + "-" + log.descripcionErrorBorrar);
                                break;
                        }
                    }
                    finally
                    {
                        log.fechaFin = DateTime.Now;
                        oDAL.guardarLogMoverCarpetaTrabajos(log);
                    }

                }
            }
        }

        private void borrarDirectorioOrigen(string rutaOrigen)
        {
            Directory.Delete(rutaOrigen, true);
        }

        private void copiarDirectorio(string rutaOrigenCompleta, string rutaDestinoCompleta)
        {
            Directory.CreateDirectory(rutaDestinoCompleta);
            foreach (string archivo in Directory.GetFiles(rutaOrigenCompleta))
            {
                File.Copy(archivo, rutaDestinoCompleta + @"\" + Path.GetFileName(archivo), true);
            }
            foreach (string carpeta in Directory.GetDirectories(rutaOrigenCompleta))
            {
                copiarDirectorio(carpeta, rutaDestinoCompleta + @"\" + new DirectoryInfo(carpeta).Name);
            }
        }

        public void TraceService(string content)
        {
            FileStream fs = new FileStream(@"C:\MatrixMoveClosedWorksServiceLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0L, SeekOrigin.End);
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();
        }

        public double obtenerMilisegundosProximaEjecucion(TimeSpan horaActual, TimeSpan horaEjecucion)
        {

            double milisegundosParaEjecucion = 0;

            if (horaActual.TotalMilliseconds >= horaEjecucion.TotalMilliseconds)
            {
                milisegundosParaEjecucion = new TimeSpan(23, 59, 0).TotalMilliseconds - horaActual.TotalMilliseconds + horaEjecucion.TotalMilliseconds;
            }
            else if (horaActual.TotalMilliseconds < horaEjecucion.TotalMilliseconds)
            {
                milisegundosParaEjecucion = horaEjecucion.TotalMilliseconds - horaActual.TotalMilliseconds;
            }
            
            return milisegundosParaEjecucion;
        }

        public bool horaEjecucionIgualHoraActual(TimeSpan horaActual, TimeSpan horaEjecucion)
        {
            horaActual = new TimeSpan((horaActual.Hours == 0) ? 23 : horaActual.Hours, (horaActual.Hours == 0) ? 59 : horaActual.Minutes, 0);
            if (horaActual.TotalMilliseconds == horaEjecucion.TotalMilliseconds)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public TimeSpan obtenerHoraEnTimeSpan(string hora)
        {
            string[] vHora;
            TimeSpan horaConvertida;
            vHora = hora.Split(Convert.ToChar(":"));
            horaConvertida = new TimeSpan(Convert.ToInt16(vHora[0]), Convert.ToInt16(vHora[1]), 0);
            return horaConvertida;
        }
    }
}
