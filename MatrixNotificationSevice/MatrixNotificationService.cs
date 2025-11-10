using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Net.Mail;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Utilidades.Utilidades;

namespace MatrixNotificationSevice
{
	partial class MatrixNotificationService : ServiceBase
	{

		private Timer timer = new Timer();
		string Host = System.Configuration.ConfigurationManager.AppSettings["Host"];
		string Puerto = System.Configuration.ConfigurationManager.AppSettings["Puerto"];
		string Correo = System.Configuration.ConfigurationManager.AppSettings["Correo"];
		string CorreoAutenticacion = System.Configuration.ConfigurationManager.AppSettings["CorreoAutenticacion"];
		string Clave = System.Configuration.ConfigurationManager.AppSettings["Clave"];
		private string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;


		public MatrixNotificationService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			try
			{
				this.TraceService(System.Environment.NewLine + DateTime.Now + System.Environment.NewLine + "Start service");
				string TimeInterval = ConfigurationManager.AppSettings["TimeInterval"].ToString();
				this.timer.Elapsed += new ElapsedEventHandler(this.OnElapsedTime);
				this.timer.Interval = Convert.ToDouble(TimeInterval);
				this.timer.Enabled = true;
			}
			catch (Exception ex)
			{
				this.TraceService("Message:" + ex.Message + "StackTrace:" + ex.StackTrace);
			}
		}

		protected override void OnStop()
		{
			this.TraceService(System.Environment.NewLine + DateTime.Now + System.Environment.NewLine + "Stopping service");
			this.timer.Enabled = false;
		}

		private void TraceService(string content)
		{
			FileStream fs = new FileStream(@"C:\MatrixNotificationServiceLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
			StreamWriter sw = new StreamWriter(fs);
			sw.BaseStream.Seek(0L, SeekOrigin.End);
			sw.WriteLine(content);
			sw.Flush();
			sw.Close();
		}

		private void prueba()
		{
			try
			{
				string Location = ConfigurationManager.AppSettings["SourcePath"].ToString();


			}
			catch (Exception ex)
			{
				this.TraceService(ex.Message.ToString());
			}
		}


		private void OnElapsedTime(object source, ElapsedEventArgs e)
		{
			this.TraceService(System.Environment.NewLine + DateTime.Now + System.Environment.NewLine + "Execute");
			ObtenerReglas();
			//this.TraceService("Another entry at " + DateTime.Now);
		}


		private void SendMail(string from, string to, string subject, string body)
		{
			List<string> LTo = new List<string>();
			LTo.Add(to);
			try
			{
				Utilidades.EnviarCorreo oEnviarCorreo = new Utilidades.EnviarCorreo();
				oEnviarCorreo.sendMail(LTo, subject, body);
			}
			catch (Exception ex)
			{
				this.TraceService(System.Environment.NewLine + "Fecha:" + DateTime.Now + System.Environment.NewLine + "Message: " + ex.Message + System.Environment.NewLine + "StackTrace: " + ex.StackTrace + System.Environment.NewLine + "InnerException: " + ex.InnerException);
			}


		}

		private void ObtenerReglas()
		{

			try
			{
				SqlDataReader Lector = null;

				using (SqlConnection conn = new SqlConnection(this.strCon))
				{
					using (SqlCommand cmd = conn.CreateCommand())
					{
						conn.Open();
						cmd.CommandText = "RU_ObtenerReglas";
						cmd.CommandType = CommandType.StoredProcedure;
						Lector = cmd.ExecuteReader();

						while (Lector.Read())
						{
							EjecutarReglas(int.Parse(Lector[0].ToString()));
						}
					}
				}


			}
			catch (Exception ex)
			{
				this.TraceService("Error al obtener las reglas" + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
			}


		}

		private void EjecutarReglas(int id)
		{

			try
			{
				SqlDataReader Lector = null;
				int i;
				string Para, Tema, mensaje;
				using (SqlConnection conn = new SqlConnection(this.strCon))
				{
					using (SqlCommand cmd = conn.CreateCommand())
					{
						conn.Open();
						cmd.CommandText = "RU_ValidarCondicion";
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
						Lector = cmd.ExecuteReader();
						Lector.NextResult();
						while (Lector.Read())
						{
							Para = Lector[0].ToString();
							Tema = Lector[1].ToString();
							mensaje = Lector[2].ToString();
							//Se debe establecer en tres pues los tres primeros ocntienen los destinatarios, tema y el mensaje 
							//y sobre el mensaje se haran los reemplazos
							for (i = 3; i <= Lector.FieldCount - 1; i++)
							{
								mensaje = mensaje.Replace("@" + (i - 2).ToString(), Lector[i].ToString());
							}
							SendMail(Correo, Para, Tema, mensaje);
							updateLastExcutionTime(id);

						}
					}
				}


			}
			catch (Exception ex)
			{
				this.TraceService("Error al ejecutar la regla" + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
			}

		}

		private void updateLastExcutionTime(int id)
		{
			try
			{

				using (SqlConnection conn = new SqlConnection(this.strCon))
				{
					using (SqlCommand cmd = conn.CreateCommand())
					{
						conn.Open();
						cmd.CommandText = "RU_ActualizarUltimaEjecucion";
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
						cmd.ExecuteNonQuery();


					}
				}


			}
			catch (Exception ex)
			{
				this.TraceService("Error al actualizaqr las fechas" + System.Environment.NewLine + ex.Message + System.Environment.NewLine + ex.StackTrace);
			}



		}

	}
}
