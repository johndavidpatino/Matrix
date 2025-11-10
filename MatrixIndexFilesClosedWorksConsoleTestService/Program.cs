using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixIndexFilesClosedWorks;
using System.Net;
using System.IO;

namespace MatrixIndexFilesClosedWorksConsoleTestService
{
    class Program
    {
        static void Main(string[] args)
        {
            BL oBl = new BL();
            DAL oDAL = new DAL();
            List<CI_OrigenesIndexacion> origenesIndexacion = new List<CI_OrigenesIndexacion>();
            string rutaCentroInformacion;
            string usuarioRed;
            string contrasenaRed;

            origenesIndexacion = oDAL.obtenerOrigenesIndexacion();
            rutaCentroInformacion = oDAL.obtenerConstante(constante.EConstante.RutaCentroInformacion).Valor;
            usuarioRed = oDAL.obtenerConstante(constante.EConstante.UsuarioArchivos).Valor;
            contrasenaRed = oDAL.obtenerConstante(constante.EConstante.ContrasenaArchivos).Valor;
            

            foreach (CI_OrigenesIndexacion origen in origenesIndexacion)
            {
                oBl.indexarCarpeta(rutaCentroInformacion + @"\" + origen.ruta, usuarioRed,  contrasenaRed, (DAL.EOrigenesIndexacion)origen.id);
            }
        }
    }

}

