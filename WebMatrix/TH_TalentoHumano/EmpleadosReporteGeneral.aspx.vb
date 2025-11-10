Public Class EmpleadosReporteGeneral
	Inherits System.Web.UI.Page
	Enum EReportes
		InformacionGeneral = 1
		Hijos = 2
		Educacion = 3
		ExperienciaLaboral = 4
		ContactosEmergencia = 5
	End Enum
	Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
		Select Case ddlTipoReporte.SelectedValue
			Case EReportes.InformacionGeneral
				reporteGeneral()
			Case EReportes.Hijos
				reporteHijos()
			Case EReportes.Educacion
				reporteEducacion()
			Case EReportes.ExperienciaLaboral
				reporteExperienciaLaboral()
			Case EReportes.ContactosEmergencia
				reporteContactosEmergencia()
		End Select
	End Sub
	Sub reporteGeneral()
		Dim empleados As New CoreProject.Empleados
		Dim reporte = empleados.obtenerReporteInformacionEmpleados()
		Dim excel As New Utilidades.ResponseExcel
		excel.responseExcel(Of CoreProject.TH_Empleados_Reporte_Info_Result)(Response, "RRHH-BD-Empleados-InformacionGeneral", "InformacionGeneral", nombresColumnasReporteGeneral(), reporte)
	End Sub
	Sub reporteHijos()
		Dim empleados As New CoreProject.Empleados
        Dim reporte = empleados.obtenerReporteHijosEmpleadosReport()
        Dim excel As New Utilidades.ResponseExcel
        excel.responseExcel(Of CoreProject.TH_Hijos_Report_Result)(Response, "RRHH-BD-Empleados-Hijos", "Hijos", nombresColumnasReporteHijos(), reporte)
    End Sub
	Sub reporteEducacion()
		Dim educacion As New CoreProject.TH.Educacion
        Dim reporte = educacion.ObtenerEducacionEmpleadosReport()
        Dim excel As New Utilidades.ResponseExcel
        excel.responseExcel(Of CoreProject.TH_Educacion_Report_Result)(Response, "RRHH-BD-Empleados-Educacion", "Educacion", nombresColumnasReporteEducacion(), reporte)
    End Sub
	Sub reporteExperienciaLaboral()
		Dim experienciaLaboral As New CoreProject.TH.ExperienciaLaboral
        Dim reporte = experienciaLaboral.getExperienciaLaboralEmpleadosReport()
        Dim excel As New Utilidades.ResponseExcel
        excel.responseExcel(Of CoreProject.TH_ExperienciaLaboral_Report_Result)(Response, "RRHH-BD-Empleados-ExperienciaLaboral", "Experiencia Laboral", nombresColumnasReporteExperienciaLaboral(), reporte)
    End Sub
	Sub reporteContactosEmergencia()
		Dim contactosEmergencia As New CoreProject.Personas
        Dim reporte = contactosEmergencia.obtenerContactosEmergenciaEmpleadosReport()
        Dim excel As New Utilidades.ResponseExcel
        excel.responseExcel(Of CoreProject.TH_ContactosEmergencia_Report_Result)(Response, "RRHH-BD-Empleados-ContactosEmergencia", "Contactos de emergencia", nombresColumnasReporteContactosEmergencia(), reporte)
    End Sub

	Private Function nombresColumnasReporteContactosEmergencia() As IEnumerable
        Dim nombresColumnas As String = "CedulaEmpleado;Empleado;ContactoEmergencia;telefonoCelular;parentescoTxt"
        Return nombresColumnas.Split(";")
	End Function

	Private Function nombresColumnasReporteExperienciaLaboral() As IEnumerable(Of String)
        Dim nombresColumnas As String = "CedulaEmpleado;Empleado;Empresa;FechaInicio;FechaFin;Cargo;EnInvestigacionMercados"
        Return nombresColumnas.Split(";")
	End Function

	Private Function nombresColumnasReporteGeneral() As IEnumerable(Of String)
		Dim nombresColumnas As String = "TipoIdentificacion;id;Nombres;Apellidos;nombrePreferido;FechaNacimiento;Edad;Genero;EstadoCivil;GrupoSanguineo;Nacionalidad;EmployeeId;BUNameITalent;jobFunction;JefeInmediato;Sede;correoIpsos;FechaIngresoIpsos;TipoContrato;Empresa;observaciones;SalarioActual;Banco;TipoCuenta;NumeroCuenta;EPS;FondoPensiones;FondoCesantias;CajaCompensacion;ARL;NivelIngles;CiudadResidencia;DireccionResidencia;BarrioResidencia;Localidad;NSE;TelefonoFijo;TelefonoCelular;EmailPersonal;fechaCreacion;fechaUltimaActualizacion;banda;level;Area;Cargo;Usuario;TallaCamiseta;Ciudad_Municipio_Nacimiento;Departamento_Nacimiento"
		Return nombresColumnas.Split(";")
	End Function
	Private Function nombresColumnasReporteHijos() As IEnumerable(Of String)
        Dim nombresColumnas As String = "CedulaEmpleado;Empleado;NombreHijo;Genero;FechaNacimiento"
        Return nombresColumnas.Split(";")
	End Function
    Private Function nombresColumnasReporteEducacion() As IEnumerable(Of String)
        Dim nombresColumnas As String = "CedulaEmpleado;Empleado;Titulo;Institucion;Pais;Ciudad;FechaInicio;FechaFin;Modalidad;Tipo;Estado"
        Return nombresColumnas.Split(";")
    End Function

    Private Sub Home4_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(31, UsuarioID) = False Then
            Response.Redirect("../home.aspx")
        End If
    End Sub
End Class