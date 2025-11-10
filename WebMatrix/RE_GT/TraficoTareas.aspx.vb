Imports CoreProject
Imports WebMatrix.Util
Imports ClosedXML.Excel

Public Class TraficoTareas
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Dim eTrabajoOP As New OP_TrabajoConfiguracion
    Private _IDUsuario As Int64
    Public Property IDUsuario() As Int64
        Get
            Return _IDUsuario
        End Get
        Set(ByVal value As Int64)
            _IDUsuario = value
        End Set
    End Property


    Private _URLRetorno As Int16
    Public Property URLRetorno() As Int16
        Get
            Return _URLRetorno
        End Get
        Set(ByVal value As Int16)
            _URLRetorno = value
        End Set
    End Property

#End Region

#Region "Funciones y Métodos"
    Sub CargarTrabajos()
        'Dim oTrabajo As New Trabajo
        Dim oWorkFlow As New WorkFlow
        gvTrabajos.DataSource = oWorkFlow.obtenerTrabajosWorkFlow(hfIdUnidad.Value, Nothing)
        'gvTrabajos.DataSource = oTrabajo.obtenerAllTrabajos()
        gvTrabajos.DataBind()
    End Sub
    Sub asignarURLDevolucion()
        Select Case URLRetorno
            Case UrlOriginal.RE_GT_TraficoTareas_Scripting
				lbtnVolver.PostBackUrl = "~/RE_GT/HomeRecoleccion.aspx"
			Case UrlOriginal.RE_GT_TraficoTareas_Pilotos
				lbtnVolver.PostBackUrl = "~/RE_GT/HomeRecoleccion.aspx"
			Case UrlOriginal.RE_GT_TraficoTareas_Critica
				lbtnVolver.PostBackUrl = "~/RE_GT/HomeGestionTratamiento.aspx"
			Case UrlOriginal.RE_GT_TraficoTareas_Verificacion
				lbtnVolver.PostBackUrl = "~/RE_GT/HomeGestionTratamiento.aspx"
			Case UrlOriginal.RE_GT_TraficoTareas_Captura
				lbtnVolver.PostBackUrl = "~/RE_GT/HomeGestionTratamiento.aspx"
			Case UrlOriginal.RE_GT_TraficoTareas_Codificacion
				lbtnVolver.PostBackUrl = "~/RE_GT/HomeGestionTratamiento.aspx"
			Case UrlOriginal.RE_GT_TraficoTareas_Datacleaning
				lbtnVolver.PostBackUrl = "~/RE_GT/HomeGestionTratamiento.aspx"
			Case UrlOriginal.RE_GT_TraficoTareas_Procesamiento
				lbtnVolver.PostBackUrl = "~/RE_GT/HomeGestionTratamiento.aspx"
			Case UrlOriginal.RE_GT_TraficoTareas_Estadistica
				lbtnVolver.PostBackUrl = "~/Es_Estadistica/Default.aspx"
			Case UrlOriginal.CORE_ListaTrabajosTareas
				lbtnVolver.PostBackUrl = "Gestion-Tareas-Trabajos.aspx"
			Case UrlOriginal.RE_GT_TrabajosPorGerencia
				Response.Redirect("~/RP_Reportes/TrabajosPorGerencia.aspx")
			Case UrlOriginal.RE_GT_TraficoEncuestasRMC
				Response.Redirect("~/RE_GT/TraficoEncuestas.aspx?UnidadId=38")
			Case UrlOriginal.RE_GT_CallCenter
				lbtnVolver.PostBackUrl = "~/RE_GT/HomeRecoleccion.aspx"
		End Select
	End Sub
#End Region

#Region "Eventos del Control"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim permisos As New Datos.ClsPermisosUsuarios
		Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

		If Request.QueryString("URLRetorno") IsNot Nothing Then
			Long.TryParse(Request.QueryString("URLRetorno"), URLRetorno)
		End If

		If Not IsPostBack Then
			If Request.QueryString("UnidadId") IsNot Nothing Then
				hfIdUnidad.Value = Int64.Parse(Request.QueryString("UnidadId").ToString)
				Select Case hfIdUnidad.Value
					Case 5 'Critica
						If permisos.VerificarPermisoUsuario(107, UsuarioID) = False Then
							Response.Redirect("../RE_GT/HomeGestionTratamiento.aspx")
						End If
					Case 6 'Verificacion
						If permisos.VerificarPermisoUsuario(109, UsuarioID) = False Then
							Response.Redirect("../RE_GT/HomeGestionTratamiento.aspx")
						End If
					Case 7 'Captura
						If permisos.VerificarPermisoUsuario(111, UsuarioID) = False Then
							Response.Redirect("../RE_GT/HomeGestionTratamiento.aspx")
						End If
					Case 8 'Codificacion
						If permisos.VerificarPermisoUsuario(108, UsuarioID) = False Then
							Response.Redirect("../RE_GT/HomeGestionTratamiento.aspx")
						End If
					Case 9 'DataCleaning
						If permisos.VerificarPermisoUsuario(110, UsuarioID) = False Then
							Response.Redirect("../RE_GT/HomeGestionTratamiento.aspx")
						End If
					Case 10 'Procesamiento
						If permisos.VerificarPermisoUsuario(112, UsuarioID) = False Then
							Response.Redirect("../RE_GT/HomeGestionTratamiento.aspx")
						End If
					Case 11 'Scripting
						'Me.btnPersonalAsignado.Visible = True
						If permisos.VerificarPermisoUsuario(115, UsuarioID) = False Then
							Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
						End If
					Case 12 'Pilotos
						If permisos.VerificarPermisoUsuario(116, UsuarioID) = False Then
							Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
						End If
					Case 13 'Estadistica
						If permisos.VerificarPermisoUsuario(121, UsuarioID) = False Then
							Response.Redirect("../ES_Estadistica/Default.aspx")
						End If
					Case 14 'Call Center
						'Me.btnPersonalAsignado.Visible = True
						If permisos.VerificarPermisoUsuario(129, UsuarioID) = False Then
							Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
						End If
				End Select

                If Request.QueryString("RolId") IsNot Nothing Then
                    hfIdRol.Value = Int64.Parse(Request.QueryString("RolId").ToString)
                Else
                    Response.Redirect("../Home.aspx")
                End If

                If Request.QueryString("TrabajoId") IsNot Nothing Then
                    hfIdTrabajo.Value = Int64.Parse(Request.QueryString("TrabajoId").ToString)
                    Session("TrabajoId") = hfIdTrabajo.Value
                    Dim oProyecto As New Proyecto
                    Dim oTrabajo As New Trabajo
                    Dim infoT = oTrabajo.obtenerXId(hfIdTrabajo.Value)
                    Session("NombreTrabajo") = infoT.id.ToString & " | " & infoT.JobBook & " | " & infoT.NombreTrabajo
                    Dim infoP = oProyecto.obtenerXId(infoT.ProyectoId)
                    If infoP.TipoProyectoId = 2 Then
                        Me.btnFichaCuanti.Visible = False
                    End If
                    accordion1.Visible = True
                    accordion0.Visible = False
                Else
                    hfIdTrabajo.Value = Nothing
                    CargarTrabajos()
                End If
                If Not Session("TrabajoId") = Nothing Then
                        hfIdTrabajo.Value = Session("TrabajoId").ToString
                        Dim oProyecto As New Proyecto
                        Dim oTrabajo As New Trabajo
                        Dim infoT = oTrabajo.obtenerXId(hfIdTrabajo.Value)
                        Dim infoP = oProyecto.obtenerXId(infoT.ProyectoId)
                        If infoP.TipoProyectoId = 2 Then
                            Me.btnFichaCuanti.Visible = False
                        End If
                    End If
            Else
				Response.Redirect("../Home.aspx")
			End If
		End If
		asignarURLDevolucion()
		Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
		smanager.RegisterPostBackControl(Me.btnPersonalAsignado)
	End Sub
	Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
		Dim oWorkFlow As New WorkFlow
		gvTrabajos.DataSource = oWorkFlow.obtenerTrabajosWorkFlow(hfIdUnidad.Value, txtBuscar.Text)
		gvTrabajos.DataBind()
		ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
	End Sub
	Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
		gvTrabajos.PageIndex = e.NewPageIndex
		CargarTrabajos()
		ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
	End Sub
	Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
		If e.CommandName = "Gestionar" Then
			hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
			Session("TrabajoId") = hfIdTrabajo.Value
			Dim oProyecto As New Proyecto
			Dim oTrabajo As New Trabajo
			Dim infoT = oTrabajo.obtenerXId(hfIdTrabajo.Value)
			Session("NombreTrabajo") = infoT.id.ToString & " | " & infoT.JobBook & " | " & infoT.NombreTrabajo
			Dim infoP = oProyecto.obtenerXId(infoT.ProyectoId)
			If infoP.TipoProyectoId = 2 Then
				Me.btnFichaCuanti.Visible = False
			End If
			accordion1.Visible = True
			accordion0.Visible = False
		ElseIf e.CommandName = "Avance" Then
			hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
			Response.Redirect("../RP_Reportes/AvanceDeCampo.aspx?TrabajoId=" & hfIdTrabajo.Value)
		End If
	End Sub
	Protected Sub btnListadoDocumentos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnListadoDocumentos.Click
		Response.Redirect("../CORE/ListaDocumentosXHilos.aspx?IdTrabajo=" & hfIdTrabajo.Value & "&URLRetorno=" & URLRetorno)
	End Sub
	Protected Sub btnEstadoTareas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEstadoTareas.Click
		Response.Redirect("../CORE/Gestion-Tareas.aspx?IdTrabajo=" & hfIdTrabajo.Value & "&URLRetorno=" & URLRetorno & "&IdUnidadEjecuta=" & hfIdUnidad.Value & "&IdRolEstima=" & hfIdRol.Value)
	End Sub
	Protected Sub btnFichaCuanti_Click(sender As Object, e As EventArgs) Handles btnFichaCuanti.Click
		Dim urlOrigen As Uri
		urlOrigen = Request.Url
        Response.Redirect("../RP_Reportes/InformacionGeneral.aspx?idTr=" & hfIdTrabajo.Value & "&URLBACK=.." & Request.Url.LocalPath.ToString & "?UnidadId=" & hfIdUnidad.Value & "|RolId=" & hfIdRol.Value & "|URLRetorno=" & URLRetorno & "|TrabajoId=" & hfIdTrabajo.Value.ToString)
        'Response.Redirect("../RE_GT/FichaCuantitativa.aspx?idtrabajo=" & hfIdTrabajo.Value)
    End Sub
#End Region


	Private Sub btnPersonalAsignado_Click(sender As Object, e As System.EventArgs) Handles btnPersonalAsignado.Click
        Dim excel As New List(Of Array)
        Dim Titulos As String = "IdAsignacion;TrabajoId;Nombres;Apellidos;Identificacion;Cargo;CodDane;Ciudad"
        Dim DynamicColNames() As String
        Dim lstCambios As List(Of OP_ListadoPersonasAsignadasTrabajo_Result)
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("Listado")

        DynamicColNames = Titulos.Split(CChar(";"))
        excel.Add(DynamicColNames)
        Dim o As New CoordinacionCampoPersonal
        lstCambios = o.ListadoPersonasAsignadas(hfIdTrabajo.Value)

        For Each x In lstCambios
            excel.Add((x.IdAsignacion.ToString() & ";" & x.TrabajoId & ";" & x.Nombres & ";" & x.Apellidos & ";" & x.Identificacion & ";" & x.Cargo & ";" & x.CodDane.ToString & ";" & x.Ciudad & ";").Split(CChar(";")).ToArray())
        Next
        worksheet.Cell("A1").Value = excel
        'worksheet.Range("C2:L100").DataType = XLCellValues.Number
        'worksheet.Range("C2:L100").Style.NumberFormat.NumberFormatId = 4
        Crearexcel(workbook, "Listado")
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""PersonalAsignadoTrabajo " & hfIdTrabajo.Value & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)

            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

	Private Sub btnVerTrabajos_Click(sender As Object, e As EventArgs) Handles btnVerTrabajos.Click
		accordion1.Visible = False
		accordion0.Visible = True
	End Sub
End Class