using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace MatrixIndexFilesClosedWorks
{
    class BL
    {
        public enum ETipo
        {
            trabajo = 1,
            proyecto = 2
        }

        public List<string> obtenerDirectoriosNuevos(List<string> directoriosRaizActuales, List<string> directoriosRaizAnteriores)
        {
            return obtenerElementosNuevos(directoriosRaizActuales, directoriosRaizAnteriores);
        }

        public List<CI_IndexacionCarpetasCierreRaiz> obtenerDirectoriosActualizar(List<CI_IndexacionCarpetasCierreRaiz> carpetasRaiz, DateTime ultimaActualizacionMayorA)
        {
            return carpetasRaiz.Where(x => x.fechaCreacion > ultimaActualizacionMayorA).ToList();
        }

        public List<string> obtenerArchivosNuevos(List<string> archivosActuales, List<string> archivosAnteriores)
        {
            return obtenerElementosNuevos(archivosActuales, archivosAnteriores);
        }

        public List<string> obtenerTodosArchivosDirectorio(string directorio)
        {
            List<string> rutasArchivos = new List<string>();
            obtenerTodosArchivosDirectorio(directorio, rutasArchivos);
            return rutasArchivos;
        }

        private List<string> obtenerTodosArchivosDirectorio(string directorio, List<string> directorios)
        {
            List<string> rutasArchivos = directorios;

            foreach (string archivo in Directory.GetFiles(directorio))
            {
                rutasArchivos.Add(archivo);
            }
            foreach (string carpeta in Directory.GetDirectories(directorio))
            {
                obtenerTodosArchivosDirectorio(carpeta, rutasArchivos);
            }
            return rutasArchivos;
        }

        private List<string> obtenerElementosNuevos(List<string> elementosActuales, List<string> elementosAnteriores)
        {
            List<string> elementosNuevos = new List<string>();

            elementosNuevos = (from elementoActual in elementosActuales
                               join elementoAnterior in elementosAnteriores
                                     on elementoActual equals elementoAnterior
                                     into coincidencias
                               where coincidencias.Count() == 0
                               select elementoActual).ToList();
            return elementosNuevos;
        }

        public string obtenerJobBookDesdeNombreCarpeta(string nombreCarpeta, ETipo tipo)
        {
            nombreCarpeta = nombreCarpeta.Replace(" ", "");
            string jobBook = string.Empty;
            switch (tipo)
            {
                case ETipo.proyecto:
                    jobBook = Regex.Match(nombreCarpeta, @"\d{2}-\d{6}-\d{2}").Value;
                    break;
                case ETipo.trabajo:
                    jobBook = Regex.Match(nombreCarpeta, @"\d{2}-\d{6}-\d{2}-\d{2}").Value; ;
                    break;
            }
            return jobBook;
        }
        public Boolean nombreCarpetaIncluyeJobBookValido(string nombreCarpeta, ETipo tipo)
        {
            nombreCarpeta = nombreCarpeta.Replace(" ", "");
            if (tipo == ETipo.proyecto)
            {
                return string.IsNullOrEmpty((Regex.Match(nombreCarpeta, @"\d{2}-\d{6}-\d{2}").Value)) ? false : true;
            }
            else
            {
                return string.IsNullOrEmpty((Regex.Match(nombreCarpeta, @"\d{2}-\d{6}-\d{2}-\d{2}").Value)) ? false : true;
            }
        }
        public List<string> obtenerRutasSinRaiz(List<string> rutas, string raiz)
        {
            return rutas.Select(x => x.Replace(raiz, "")).ToList();
        }
        public string obtenerRutasSinRaiz(string ruta, string raiz)
        {
            return ruta.Replace(raiz, "");
        }

        public long? obtenerIdProyectoTrabajo(string nombreCarpeta, ETipo tipoBusqueda)
        {
            DAL oDAL = new DAL();
            string JB = "";
            long? id = null;

            if (tipoBusqueda==ETipo.proyecto && nombreCarpetaIncluyeJobBookValido(nombreCarpeta.Trim(), BL.ETipo.proyecto))
            {
                JB = obtenerJobBookDesdeNombreCarpeta(nombreCarpeta.Trim(), BL.ETipo.proyecto);
                id = oDAL.obtenerIdProyectoXJobBook(JB);
            }
            if (tipoBusqueda==ETipo.trabajo && nombreCarpetaIncluyeJobBookValido(nombreCarpeta.Trim(), BL.ETipo.trabajo))
            {
                JB = obtenerJobBookDesdeNombreCarpeta(nombreCarpeta.Trim(), BL.ETipo.trabajo);
                id = oDAL.obtenerIdTrabajoXJobBook(JB);
            }
            return id;
        }

        public void TraceService(string content)
        {
            FileStream fs = new FileStream(@"C:\MatrixIndexFilesClosedWorksServiceLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0L, SeekOrigin.End);
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();
        }

        public static string quitarCaracteresEspeciales(string fileName)
        {
            List<char> caracteresInvalidos;
            caracteresInvalidos = Path.GetInvalidFileNameChars().ToList();
            caracteresInvalidos.Remove(Convert.ToChar(@"\"));
            return caracteresInvalidos.Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public void indexarCarpeta(string rutaCarpeta, string usuarioRed, string contrasenaRed, DAL.EOrigenesIndexacion origenIndexacion)
        {
            DAL oDAL = new DAL();
            List<string> carpetasActuales = new List<string>();
            List<CI_IndexacionCarpetasCierreRaiz> carpetasIndexadas = new List<CI_IndexacionCarpetasCierreRaiz>();
            List<CI_IndexacionCarpetasCierreRaiz> carpetasAActualizar = new List<CI_IndexacionCarpetasCierreRaiz>();
            List<string> soloNombresCarpetasIndexadas = new List<string>();
            List<string> soloNombresCarpetasActualizar = new List<string>();
            List<string> carpetasNuevas = new List<string>();
            string ubicacionSinCarpetaRaiz;
            string[] vRutaCarpeta;
            string servidor;
            vRutaCarpeta = rutaCarpeta.Split(Convert.ToChar(@"\"));
            servidor = vRutaCarpeta[2];
            DirectoryInfo carpetaRaiz = new DirectoryInfo(rutaCarpeta);

            carpetasIndexadas = oDAL.obtenerCarpetasCierreIndexadas(origenIndexacion);
            carpetasAActualizar = obtenerDirectoriosActualizar(carpetasIndexadas, DateTime.Now.AddMonths(-1));
            soloNombresCarpetasIndexadas = carpetasIndexadas.Select(x => carpetaRaiz + x.ubicacion).ToList();
            soloNombresCarpetasActualizar = carpetasAActualizar.Select(x => carpetaRaiz + x.ubicacion).ToList();

            using (NetworkConnection directory = new NetworkConnection(@"\\" + servidor, new NetworkCredential(usuarioRed, contrasenaRed)))
            {
                carpetasActuales = carpetaRaiz.GetDirectories().Select(x => x.FullName).ToList();
                carpetasNuevas = obtenerDirectoriosNuevos(carpetasActuales, soloNombresCarpetasIndexadas);
                foreach (string carpetaNueva in carpetasNuevas)
                {
                    long? idProyecto = null;
                    long? idTrabajo = null;
                    decimal idCarpetaNueva;
                    List<string> archivos = new List<string>();

                    ubicacionSinCarpetaRaiz = obtenerRutasSinRaiz(carpetaNueva, carpetaRaiz.ToString());

                    idProyecto = obtenerIdProyecto(ubicacionSinCarpetaRaiz, origenIndexacion);
                    idTrabajo = obtenerIdTrabajo(ubicacionSinCarpetaRaiz, origenIndexacion);

                    idCarpetaNueva = oDAL.guardarCarpetaCierreRaizIndexada(ubicacionSinCarpetaRaiz, DateTime.UtcNow.AddHours(-5), DateTime.UtcNow.AddHours(-5), idProyecto, idTrabajo, origenIndexacion);
                    archivos = obtenerTodosArchivosDirectorio(carpetaNueva);
                    archivos = obtenerRutasSinRaiz(archivos, carpetaRaiz.ToString() + ubicacionSinCarpetaRaiz + @"\");
                    oDAL.guardarRutaArchivosCarpetaCierre(idCarpetaNueva, archivos);
                }
                foreach (CI_IndexacionCarpetasCierreRaiz carpetaActualizar in carpetasAActualizar)
                {
                    if (Directory.Exists(carpetaRaiz + carpetaActualizar.ubicacion))
                    {
                        List<string> archivosIndexados = new List<string>();
                        List<string> archivosActuales = new List<string>();
                        List<string> archivosNuevos = new List<string>();
                        ubicacionSinCarpetaRaiz = obtenerRutasSinRaiz(carpetaActualizar.ubicacion, carpetaRaiz.ToString());
                        archivosIndexados = oDAL.obtenerArchivosIndexados(carpetaActualizar.id).Select(y => y.ubicacion).ToList();
                        archivosActuales = obtenerTodosArchivosDirectorio(carpetaRaiz + carpetaActualizar.ubicacion);
                        archivosActuales = obtenerRutasSinRaiz(archivosActuales, carpetaRaiz.ToString() + ubicacionSinCarpetaRaiz + @"\");
                        archivosNuevos = obtenerArchivosNuevos(archivosActuales, archivosIndexados);
                        if (archivosNuevos.Count > 0)
                        {
                            oDAL.guardarRutaArchivosCarpetaCierre(carpetaActualizar.id, archivosNuevos);
                            oDAL.actualizarCarpetaCierreRaizIndexada(carpetaActualizar.id, string.Empty, null, DateTime.UtcNow.AddHours(-5), null, null);
                        }
                    }
                }
            }
        }

        public long? obtenerIdProyecto(string carpeta, DAL.EOrigenesIndexacion origenIndexacion)
        {
            long? idProyecto = null;
            long idTrabajo = 0;
            DAL oDAL = new DAL();
            if (origenIndexacion == DAL.EOrigenesIndexacion.CarpetasMatrix)
            {
                idTrabajo = Convert.ToInt64(carpeta.Substring(0, carpeta.IndexOf(Convert.ToChar("-")) + 1));
                idProyecto = oDAL.obtenerIdProyectoXIdTrabajo(idTrabajo);
            }
            else
            {
                idProyecto = obtenerIdProyectoTrabajo(carpeta, ETipo.proyecto);
            }
            return idProyecto;
        }

        public long? obtenerIdTrabajo(string carpeta, DAL.EOrigenesIndexacion origenIndexacion)
        {
            long? idTrabajo = null;
            DAL oDAL = new DAL();
            if (origenIndexacion == DAL.EOrigenesIndexacion.CarpetasMatrix)
            {
                idTrabajo = Convert.ToInt64(carpeta.Substring(0, carpeta.IndexOf(Convert.ToChar("-")) + 1));
            }
            else
            {
                idTrabajo = obtenerIdProyectoTrabajo(carpeta, ETipo.trabajo);
            }
            return idTrabajo;
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
