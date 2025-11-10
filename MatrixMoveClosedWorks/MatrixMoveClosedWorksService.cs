using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace MatrixMoveClosedWorks
{

    public partial class MatrixMoveClosedWorksService : ServiceBase
    {
        public bool intervaloAlineado { get; set; } = false;
        private Timer timer = new Timer();
        public MatrixMoveClosedWorksService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            BL oBL = new BL();
            oBL.TraceService(DateTime.Now.ToString() + " - Servicio iniciado correctamente");
            timer.Elapsed += timer_Elapsed;
            timer.Interval = 120000; /*Se asignan inicialmente 2 minutos, mientras que se asigna el intervalo real*/
            timer.Start();
        }

        protected override void OnStop()
        {
            BL oBL = new BL();
            oBL.TraceService(DateTime.Now.ToString() + " - Servicio detenido");
        }
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            BL oBL = new BL();
            if (!(intervaloAlineado))
            {
                DAL oDAL = new DAL();
                string hora;
                double milisegundos = 0;
                hora = oDAL.obtenerConstante(constante.EConstante.horaServicioMueveTrabajosCerrados).Valor;
                TimeSpan horaActual = new TimeSpan((DateTime.Now.Hour == 0) ? 23 : DateTime.Now.Hour, (DateTime.Now.Hour == 0) ? 59 : DateTime.Now.Minute, 0);
                TimeSpan horaEjecucion = oBL.obtenerHoraEnTimeSpan(hora);
                milisegundos = oBL.obtenerMilisegundosProximaEjecucion(horaActual, horaEjecucion);
                intervaloAlineado = oBL.horaEjecucionIgualHoraActual(horaActual, horaEjecucion);
                timer.Interval = milisegundos;
                oBL.TraceService(DateTime.Now.ToString() + " - Intervalo ajustado, proxima ejecucion en - " + milisegundos.ToString() + " milisegundos");
                if (intervaloAlineado)
                {
                    oBL.TraceService(DateTime.Now.ToString() + " - Intervalo alineado");
                }
                else
                {
                    oBL.TraceService(DateTime.Now.ToString() + " - Intervalo no alineado");
                };
            }
            moverTrabajos();
            oBL.TraceService(DateTime.Now.ToString() + " - Ejecutado");
        }
        private void moverTrabajos()
        {
            BL oBL = new BL();
            oBL.moverTrabajosCerrados();
        }


    }
}
