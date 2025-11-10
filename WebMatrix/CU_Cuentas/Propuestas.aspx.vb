Imports WebMatrix.Util
Imports CoreProject
'Imports CoreProject.CU_Model
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports System.Threading.Tasks


Public Class Propuestas
    Inherits System.Web.UI.Page
#Region " Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
			'Dim taskArray(6) As Task
			'taskArray(0) = Task.Factory.StartNew(Sub(obj As Object)
			'                                         CargarTipoPropuesta()
			'                                     End Sub,
			'                                     0)
			'taskArray(1) = Task.Factory.StartNew(Sub(obj As Object)
			'                                         CargarProbabilidadApro()
			'                                     End Sub,
			'                                     1)
			'taskArray(2) = Task.Factory.StartNew(Sub(obj As Object)
			'                                         CargarEstadoPropuesta()
			'                                     End Sub,
			'                                     2)
			'taskArray(3) = Task.Factory.StartNew(Sub(obj As Object)
			'                                         CargarOrigenPropuesta()
			'                                     End Sub,
			'                                     3)
			'taskArray(4) = Task.Factory.StartNew(Sub(obj As Object)
			'                                         CargarRazones()
			'                                     End Sub,
			'                                     4)
			'taskArray(5) = Task.Factory.StartNew(Sub(obj As Object)
			'                                         CargarBrief()
			'                                     End Sub,
			'                                     5)
			'taskArray(6) = Task.Factory.StartNew(Sub(obj As Object)
			'                                         CargarPropuestas()
			'                                     End Sub,
			'                                     6)
			CargarProbabilidadApro()
			CargarEstadoPropuesta()
			CargarRazones()
			'CargarBrief()
			CargarPropuestas()
            'Task.WaitAll(taskArray)
            If Request.QueryString("IdBrief") IsNot Nothing Then
                Dim IdBrief As Int64 = Int64.Parse(Request.QueryString("IdBrief").ToString())
                CargarBrief(IdBrief)
            ElseIf Request.QueryString("IdPropuesta") IsNot Nothing Then
                Dim IdPropuesta As Int64 = Int64.Parse(Request.QueryString("IdPropuesta").ToString())
                CargarInfo(IdPropuesta)
            End If
            Validar()
        End If
    End Sub
	Protected Sub ddlprobabilidadaprob_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlprobabilidadaprob.DataBound
		DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
	End Sub
	Protected Sub ddlestadopropuesta_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlestadopropuesta.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddestadospropuesta_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddEstadosPropuesta.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
	Protected Sub ddlrazonesnoaprob_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlrazonesnoaprob.DataBound
		DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
	End Sub
	Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        Dim oPropuesta As New Propuesta
        If ddEstadosPropuesta.SelectedIndex = -1 Or ddEstadosPropuesta.SelectedIndex = 0 Then
            CargarPropuestas()
        Else
            gvDatos.DataSource = oPropuesta.ObtenerXIdGerenteCuentasXIdEstado(Session("IDUsuario").ToString, ddEstadosPropuesta.SelectedValue)
            gvDatos.DataBind()
        End If
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            CargarPropuestas()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
		Try
			Guardar()
			ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
			log(4, hfidpropuesta.Value, 2)
			'Limpiar()
			CargarPropuestas(hfidpropuesta.Value)
			accordion2.Visible = True
			accordion3.Visible = True
			accordion4.Visible = True
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
		End Try
	End Sub
	Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
		Try
			Select Case e.CommandName
				Case "Modificar"
					Dim idPropuesta As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
					'Limpiar()
					Detalles(idPropuesta)
					CargarInfo(idPropuesta)
					CargarPropuestas(idPropuesta)
					ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
				Case "Eliminar"
					Dim idPropuesta As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
					Eliminar(idPropuesta)
					CargarPropuestas()
					ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
				Case "Envio"
					Dim idPropuesta As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
					Response.Redirect("EnvioPresupuestosRevision.aspx?PropuestaId=" & idPropuesta)
				Case "Detalles"
					Dim idPropuesta As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
					Limpiar()
					Detalles(idPropuesta)
				Case "Presupuestos"
					CargarPresupuestos(Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id")))
					Me.hfidpropuesta.Value = (Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id")))
			End Select
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			ActivateAccordion(0, EffectActivateAccordion.NoEffect)
		End Try
	End Sub

	Protected Sub ddlestadopropuesta_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlestadopropuesta.SelectedIndexChanged
        Select Case ddlestadopropuesta.SelectedValue
            Case EstadoPropuesta.Creada
                txtFechaEnvio.Text = ""
                txtFechaAprobacion.Text = ""
                txtFechaEnvio.Enabled = False
                ddlrazonesnoaprob.SelectedIndex = -1
                ddlrazonesnoaprob.Enabled = False
            Case EstadoPropuesta.Enviada
                txtFechaEnvio.Enabled = True
				ddlrazonesnoaprob.SelectedIndex = -1
				ddlrazonesnoaprob.Enabled = False
            Case EstadoPropuesta.Vendida
                txtFechaEnvio.Enabled = True
				ddlrazonesnoaprob.SelectedIndex = -1
				ddlrazonesnoaprob.Enabled = False
            Case EstadoPropuesta.Perdida
                ddlrazonesnoaprob.Enabled = True
        End Select
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub btnEstudio_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEstudio.Click
        Try
            If String.IsNullOrEmpty(hfidpropuesta.Value) Then
                Throw New Exception("Debe Elegir una propuesta para ir al estudio")
            End If
            Response.Redirect("Estudios.aspx?IdPropuesta=" & hfidpropuesta.Value)

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub btnGuardarObservacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardarObservacion.Click
        Try
            If String.IsNullOrEmpty(hfidpropuesta.Value) Then
                Throw New Exception("Debe Elegir una propuesta para guardar las observaciones")
            End If
            Dim idPropuesta As Int64 = Int64.Parse(hfidpropuesta.Value)
            GuardarObservaciones(idPropuesta)
            ShowNotification("Observación guardada correctamente", ShowNotifications.InfoNotification)
            log(4, hfidpropuesta.Value, 3)
            Limpiar()
            CargarPropuestas()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(2, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Private Sub gvPresupuestosAsignadosXPropuesta_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPresupuestosAsignadosXPropuesta.PageIndexChanging
        Me.gvPresupuestosAsignadosXPropuesta.PageIndex = e.NewPageIndex
        CargarPresupuestos(hfidpropuesta.Value)
    End Sub
    Private Sub gvPresupuestosAsignadosXPropuesta_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPresupuestosAsignadosXPropuesta.RowCommand
        Dim oePresupuesto As New CU_Presupuesto_Get_Result
        Dim oPresupuesto As New Presupuesto
        'CargarPresupuestos(hfidpropuesta.Value)
        If e.CommandName = "EditarPresupuesto" Then
            oePresupuesto = oPresupuesto.obtenerXId(gvPresupuestosAsignadosXPropuesta.DataKeys(e.CommandArgument)("Id"))
            Response.Redirect("../CAP/Cap_Principal.aspx?IdPropuesta=" & oePresupuesto.PropuestaId & "&Alternativa=" & oePresupuesto.Alternativa & "&Accion=1")
        End If
        If e.CommandName = "Duplicar" Then
            oePresupuesto = oPresupuesto.obtenerXId(gvPresupuestosAsignadosXPropuesta.DataKeys(e.CommandArgument)("Id"))
            hfpD.Value = oePresupuesto.PropuestaId
            hfaD.Value = oePresupuesto.Alternativa
            uPanelDuplicacion.Update()
        End If
    End Sub
#End Region
#Region "Funciones y Metodos"
    Public Sub Limpiar()
		ddlestadopropuesta.SelectedValue = EstadoPropuesta.Creada
		'hfidpropuesta.Value = String.Empty
		txtTitulo.Text = String.Empty
		ddlprobabilidadaprob.SelectedValue = "-1"
		txtFechaEnvio.Text = String.Empty
        ddlestadopropuesta.SelectedValue = "-1"
		txtFechaAprobacion.Text = String.Empty
		ddlrazonesnoaprob.SelectedValue = "-1"
		txtBrief.Text = ""
		chkTracking.Checked = False
        txtTituloDetalle.Text = String.Empty
        txttipopropuestaDetalle.Text = String.Empty
        txtprobabilidadaprobdetalle.Text = String.Empty
        txtEstadoPropuestadetalle.Text = String.Empty
        txtObservaciones.Text = String.Empty
        lblobservacionesant.Text = String.Empty
        lblobservacionesant.Visible = False
        lbltituloobservaciones.Visible = False
        txtFechaInicioCampo.Text = ""
        txtHabeasData.Text = ""
    End Sub
	Public Sub CargarProbabilidadApro()
		Try
			Dim oAuxiliares As New Auxiliares
			Dim listaProbabilidades = (From lprob In oAuxiliares.DevolverProbabilidadAprobacion()
									   Select Id = lprob.id,
									   probabilidad = lprob.probabilidad).OrderBy(Function(p) p.probabilidad).ToList()
			ddlprobabilidadaprob.DataSource = listaProbabilidades
			ddlprobabilidadaprob.DataValueField = "Id"
			ddlprobabilidadaprob.DataTextField = "probabilidad"
			ddlprobabilidadaprob.DataBind()
		Catch ex As Exception
			Throw ex
		End Try
	End Sub
	Public Sub CargarEstadoPropuesta()
        Try
            Dim oAuxiliares As New Auxiliares
            Dim listaEstados = (From lestado In oAuxiliares.DevolverEstadoPropuesta()
                                Select Id = lestado.id, Estado = lestado.Estado).OrderBy(Function(e) e.Id).ToList()
            ddlestadopropuesta.DataSource = listaEstados
            ddlestadopropuesta.DataValueField = "Id"
            ddlestadopropuesta.DataTextField = "Estado"
            ddlestadopropuesta.DataBind()
            ddEstadosPropuesta.DataSource = listaEstados
            ddEstadosPropuesta.DataValueField = "Id"
            ddEstadosPropuesta.DataTextField = "Estado"
            ddEstadosPropuesta.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
	Public Sub CargarRazones()
		Try
			Dim oAuxiliar As New Auxiliares
			Dim ListaRazones = (From lrazon In oAuxiliar.DevolverRazonesNoAprobacion
								Select Id = lrazon.id,
								razon = lrazon.razon).OrderBy(Function(r) r.razon).ToList()
			ddlrazonesnoaprob.DataSource = ListaRazones
			ddlrazonesnoaprob.DataValueField = "Id"
			ddlrazonesnoaprob.DataTextField = "razon"
			ddlrazonesnoaprob.DataBind()
		Catch ex As Exception
			Throw ex
		End Try
	End Sub
	Public Sub CargarPropuestas(Optional ByVal idPropuesta As Long? = Nothing)
		Try
			Dim oPropuesta As New Propuesta
			Dim oBrief As New Brief

			Dim lstpropuestas As List(Of CU_Propuestas_Get_Result)


			lstpropuestas = oPropuesta.ObtenerXIdGerenteCuentas(Session("IDUsuario").ToString)

			If (Not String.IsNullOrEmpty(txtBuscar.Text)) Then
				'Dim list = lstpropuestas.Where(Function(p) (p.Id.ToString = txtBuscar.Text)).ToList()
				'If list.Count = 0 Then
				'    list = lstpropuestas.Where(Function(p) (p.Titulo.ToUpper.Contains(txtBuscar.Text.ToUpper))).ToList()
				'End If
				gvDatos.DataSource = lstpropuestas.Where(Function(p) (p.Id.ToString = txtBuscar.Text) Or (p.Titulo.ToUpper.Contains(txtBuscar.Text.ToUpper))).ToList() 'list
			ElseIf Not idPropuesta Is Nothing Then
				gvDatos.DataSource = lstpropuestas.Where(Function(p) (p.Id.ToString = idPropuesta)).ToList() 'list
			Else
				gvDatos.DataSource = lstpropuestas.ToList()
			End If
			gvDatos.DataBind()

		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
		End Try
	End Sub
	Public Sub Detalles(ByVal idPropuesta As Int64)
        Try
            hfidpropuesta.Value = idPropuesta
            Dim oPropuesta As New Propuesta
            Dim info = oPropuesta.DevolverxID(idPropuesta)

            'If info.EstadoId = EstadoPropuesta.Vendida Then
            hfidpropuesta.Value = idPropuesta
            txtTituloDetalle.Text = info.Titulo
            txttipopropuestaDetalle.Text = info.TipoPropuesta
            txtprobabilidadaprobdetalle.Text = info.Probabilidad
            txtEstadoPropuestadetalle.Text = info.Estado

            Dim oSeguimiento As New SeguimientoPropuesta
            Dim infoobservacion = oSeguimiento.DevolverSeguimientoPropuesta(idPropuesta)
            Dim strObservacion As New StringBuilder
            Dim Sw As Int32 = 0
            For Each fila As CU_SeguimientoPropuestas_Get_Result In infoobservacion
                strObservacion.AppendLine("<br/><b>Fecha: </b> " & fila.Fecha)
                strObservacion.AppendLine("<br/>" & fila.Observacion)
                strObservacion.AppendLine("<br/>")
                Sw = 1
            Next

            If Sw = 1 Then
                lblobservacionesant.Visible = True
                lbltituloobservaciones.Visible = True
                lblobservacionesant.Text = strObservacion.ToString()
            Else
                lblobservacionesant.Visible = False
                lbltituloobservaciones.Visible = False
                lblobservacionesant.Text = String.Empty
            End If
            'ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
            If Not (info.EstadoId = EstadoPropuesta.Vendida) Then
                btnEstudio.Enabled = False
            Else
                btnEstudio.Enabled = True
            End If
            'Else
            'Limpiar()
            'Throw New Exception("La propuesta no tiene un estado de vendida por tal motivo no se mostrará el detalle del registro")
            'End If

        Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
		End Try
    End Sub
    Public Sub Guardar()
        Try
            Dim oPropuesta As New Propuesta
            Dim idPropuesta As Int64?
            Dim RazonesNoAprobacion As Short?
            Dim FechaAprobacion As Date? = Nothing
            Dim FechaEnvio As Date? = Nothing
            Dim FechaCampo As Date? = Nothing
            Dim FechaInicio As Date? = Nothing
            Dim JobBook = "", FormaEnvio As String = ""

            If IsDate(txtFechaEnvio.Text) Then FechaEnvio = CDate(txtFechaEnvio.Text)
            If IsDate(txtFechaAprobacion.Text) Then FechaAprobacion = CDate(txtFechaAprobacion.Text)
            If IsDate(txtFechaInicioCampo.Text) Then FechaInicio = CDate(txtFechaInicioCampo.Text)

            If Not String.IsNullOrEmpty(hfidpropuesta.Value) Then
                idPropuesta = Int64.Parse(hfidpropuesta.Value)
            End If

			If ddlprobabilidadaprob.SelectedValue = "-1" Then
				Throw New Exception("Debe seleccionar una probabilidad de aprobación")
			End If

			If ddlestadopropuesta.SelectedValue = "-1" Then
                Throw New Exception("Debe seleccionar un estado de la propuesta")
            End If

			If txtBrief.Text = "" Then
				Throw New Exception("Debe seleccionar un brief asociado")
			End If

			Select Case ddlestadopropuesta.SelectedValue
                Case EstadoPropuesta.Cancelada
                    FechaAprobacion = Nothing
                Case EstadoPropuesta.Creada
                    FechaEnvio = Nothing
                    txtFechaEnvio.Text = ""
                    txtFechaAprobacion.Text = ""
                    FechaAprobacion = Nothing
                Case EstadoPropuesta.Enviada
                    If FechaEnvio Is Nothing Then Throw New Exception("Digite la fecha de envío")
				Case EstadoPropuesta.Vendida
					If FechaEnvio Is Nothing Then Throw New Exception("Digite la fecha de envío")
					If FechaAprobacion Is Nothing Then Throw New Exception("Digite la fecha de aprobación")
				Case EstadoPropuesta.Perdida
                    If FechaAprobacion Is Nothing Then Throw New Exception("Digite la fecha de no aprobación")
                    If ddlrazonesnoaprob.SelectedIndex = -1 Then Throw New Exception("Debe seleccionar la razón de no aprobación")
            End Select

            If Not ddlrazonesnoaprob.SelectedValue = "-1" Then
                RazonesNoAprobacion = Short.Parse(ddlrazonesnoaprob.SelectedValue)
            End If

            If ddlestadopropuesta.SelectedValue = EstadoPropuesta.Creada Or ddlestadopropuesta.SelectedValue = EstadoPropuesta.Enviada Or ddlestadopropuesta.SelectedValue = EstadoPropuesta.Vendida Then
                If Not String.IsNullOrEmpty(txtFechaInicioCampo.Text) Then
                    If ValidarFecha(txtFechaInicioCampo.Text) Then
                        If Date.Parse(txtFechaInicioCampo.Text) > Now() Then
                            FechaCampo = Date.Parse(txtFechaInicioCampo.Text)
                        Else
                            Throw New Exception("La fecha de inicio de campo debe ser mayor a la fecha actual")
                        End If
                    Else
                        Throw New Exception("Fecha de Inicio de Campo No Válida")
                    End If
                Else
                    Throw New Exception("Digite la fecha de Inicio de Campo")
                End If

            End If

            If chkInternacional.Checked = True Then
                JobBook = txtJobBookInt.Text
            Else
                JobBook = txtJobBook.Text
            End If
            If (Not (JobBook.Length = 9 Or JobBook.Length = 12)) And Me.ddlestadopropuesta.SelectedValue = EstadoPropuesta.Vendida Then
                Throw New Exception("Debe escribir el JobBook antes de continuar")
            End If
            If txtHabeasData.Text = "" Then
                Throw New Exception("Debe ingresar los Requerimientos De Habeas Data/ Confidencialidad/ Privacidad Y Seguridad De La Información")
            End If
			hfidpropuesta.Value = oPropuesta.Guardar(idPropuesta, txtTitulo.Text, Byte.Parse(1), Decimal.Parse(ddlprobabilidadaprob.SelectedValue), FechaEnvio, Byte.Parse(ddlestadopropuesta.SelectedValue), Byte.Parse(1), FechaAprobacion, RazonesNoAprobacion, FormaEnvio, Int64.Parse(txtBrief.Text), chkTracking.Checked, JobBook, chkInternacional.Checked, txtAnticipo.Text, txtSaldo.Text, txtPlazo.Text, FechaCampo, txtHabeasData.Text)
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
		End Try
    End Sub
    Public Sub GuardarObservaciones(ByVal idpropuesta As Int64)
        Try
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            Dim Observacion As String

            If String.IsNullOrEmpty(txtObservaciones.Text) Then
                Throw New Exception("Debe Ingresar una observación")
            Else
                Observacion = txtObservaciones.Text
            End If

            Dim oSeguimiento As New SeguimientoPropuesta
            oSeguimiento.Guardar(Observacion, UsuarioID, idpropuesta)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub CargarInfo(ByVal idpropuesta As Int64)
		Try
			accordion2.Visible = True
			accordion3.Visible = True
			accordion4.Visible = True

			Dim oPropuesta As New Propuesta
			Dim info = oPropuesta.DevolverxID(idpropuesta)
			hfidpropuesta.Value = idpropuesta
			txtTitulo.Text = info.Titulo

			If info.ProbabilidadId IsNot Nothing Then
				ddlprobabilidadaprob.SelectedValue = info.ProbabilidadId
			End If

			If info.FechaEnvio IsNot Nothing Then
				txtFechaEnvio.Text = info.FechaEnvio
			End If

			If info.EstadoId IsNot Nothing Then
				ddlestadopropuesta.SelectedValue = info.EstadoId
			End If

			If info.FechaAprob IsNot Nothing Then
				txtFechaAprobacion.Text = info.FechaAprob
			End If

			If info.RazonNoAprobId IsNot Nothing Then
				ddlrazonesnoaprob.SelectedValue = info.RazonNoAprobId
			End If
			If info.FechaInicioCampo IsNot Nothing Then
				Me.txtFechaInicioCampo.Text = info.FechaInicioCampo
			End If
			txtBrief.Text = info.Brief
			chkTracking.Checked = info.Tracking
			'txtJobBook.Text = info.JobBook
			chkInternacional.Checked = info.internacional
			If chkInternacional.Checked = True Then
				Me.txtJobBook.Visible = False
				Me.txtJobBook.Text = ""
				Me.txtJobBookInt.Visible = True
				Me.txtJobBookInt.Text = info.JobBook
			Else
				Me.txtJobBook.Visible = True
				Me.txtJobBook.Text = info.JobBook
				Me.txtJobBookInt.Text = ""
				Me.txtJobBookInt.Visible = False
			End If
			If info.RequestHabeasData IsNot Nothing Then
				txtHabeasData.Text = info.RequestHabeasData
			End If
			CargarAlternativas(idpropuesta)
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
		End Try
    End Sub

    Public Sub Eliminar(ByVal idpropuesta As Int64)
        Try
            Dim oPropuesta As New Propuesta
            oPropuesta.Eliminar(idpropuesta)
        Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
		End Try
    End Sub

    Public Sub CargarBrief(ByVal idbrief As Int64)
        Try
            Limpiar()
			accordion2.Visible = True
			accordion1.Visible = False
			txtTitulo.Focus()
            txtBrief.Text = idbrief
            Dim oBrief As New Brief
            Dim infoB = oBrief.DevolverxID(idbrief)
            Dim oCliente As New Cliente
            Dim infoC = oCliente.DevolverxID(infoB.ClienteId)
            txtAnticipo.Text = infoC.Anticipo
            txtSaldo.Text = infoC.Saldo
            txtPlazo.Text = infoC.Plazo
            txtTitulo.Text = infoB.Titulo
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ValidarFecha(ByVal txtFecha As String) As Boolean
        Dim dt As DateTime

        Dim blnFlag As Boolean = DateTime.TryParse(txtFecha, dt)

        If blnFlag Then
            Return True
        Else
            Return False
        End If
    End Function

    Sub CargarPresupuestos(ByVal propuestaId As Int64)
        Dim oPresupuesto As New Presupuesto
        gvPresupuestosAsignadosXPropuesta.DataSource = oPresupuesto.DevolverxIdPropuesta(propuestaId, Nothing)
        gvPresupuestosAsignadosXPropuesta.DataBind()
        upPresupuestosAsignadosXPropuesta.Update()
    End Sub

    Sub CargarAlternativas(ByVal propuestaId As Int64)
        Me.pnlAlternativas.Visible = True
        Try
            Dim oPresupuesto As New Presupuesto
            gvAlternativas.DataSource = oPresupuesto.DevolverxIdPropuesta(propuestaId, Nothing)
            gvAlternativas.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Sub CargarPresupuestosPorAlternativa(ByVal propuesta As Int64, ByVal alternativa As Int32)
        Dim o As New IQ.Transacciones
        Me.gvPresupuestos.DataSource = o.PresupuestosPorAlternativa(propuesta, alternativa)
        Me.gvPresupuestos.DataBind()
        Me.pnlPresupuestos.Visible = True
    End Sub

    Public Sub log(ByVal idfrom As Int16, ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Dim log As New LogEjecucion
        log.Guardar(idfrom, iddoc, Now(), Session("IDUsuario"), idaccion)

    End Sub

#End Region

	Protected Sub btnAddIquote_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddIquote.Click
		If hfidpropuesta.Value = "0" Or hfidpropuesta.Value = "" Then
			ShowNotification("Primero debe guardar la propuesta", ShowNotifications.ErrorNotificationLong)
			Exit Sub
		End If
		Try
			Response.Redirect("../CAP/Cap_Principal.aspx?idPropuesta=" & hfidpropuesta.Value)
		Catch ex As Exception

		End Try
	End Sub

	Protected Sub btnDisenoMuestral_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDisenoMuestral.Click
        Try
            If String.IsNullOrEmpty(hfidpropuesta.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
                Exit Sub
            Else
                Response.Redirect("../ES_Estadistica/BriefDisenoDeMuestra.aspx?idpropuesta=" & hfidpropuesta.Value)
            End If
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Public Sub Validar()
        Try

            If Session("IDUsuario") IsNot Nothing Then
                Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
                Dim oValidacion As New Validacion
                If Not oValidacion.ValidarPermiso("Propuesta", UsuarioID) Then
                    btnGuardar.Enabled = False
                    gvDatos.Columns.Item(7).Visible = False
                    'gvDatos.Columns.Item(8).Visible = False
                    btnDisenoMuestral.Enabled = False
                    btnGuardarObservacion.Enabled = False
                    btnEstudio.Enabled = False
                Else
                    btnGuardar.Enabled = True
                    gvDatos.Columns.Item(7).Visible = True
                    'gvDatos.Columns.Item(8).Visible = True
                    btnDisenoMuestral.Enabled = True
                    btnGuardarObservacion.Enabled = True
                    btnEstudio.Enabled = True
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnDocumentos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDocumentos.Click
        If hfidpropuesta.Value <> "" Then
            Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & hfidpropuesta.Value & "&IdDocumento=2")
        End If
    End Sub

    
    Protected Sub ddEstadosPropuesta_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddEstadosPropuesta.SelectedIndexChanged
        Dim oPropuesta As New Propuesta
        If ddEstadosPropuesta.SelectedIndex = -1 Or ddEstadosPropuesta.SelectedIndex = 0 Then
            CargarPropuestas()
        Else
            gvDatos.DataSource = oPropuesta.ObtenerXIdGerenteCuentasXIdEstado(Session("IDUsuario").ToString, ddEstadosPropuesta.SelectedValue)
            gvDatos.DataBind()
        End If
    End Sub

    Private Sub btnQuitarFiltro_Click(sender As Object, e As System.EventArgs) Handles btnQuitarFiltro.Click
        Me.txtBuscar.Text = ""
        Me.ddEstadosPropuesta.SelectedIndex = 0
        CargarPropuestas()
    End Sub

    Protected Sub chkInternacional_CheckedChanged(sender As Object, e As EventArgs) Handles chkInternacional.CheckedChanged
        If chkInternacional.Checked = True Then
            Me.txtJobBook.Visible = False
            Me.txtJobBook.Text = ""
            Me.txtJobBookInt.Visible = True
        Else
            Me.txtJobBook.Visible = True
            Me.txtJobBookInt.Text = ""
            Me.txtJobBookInt.Visible = False
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnDuplicarAlternativa_Click(sender As Object, e As EventArgs) Handles btnDuplicarAlternativa.Click
        Dim o As New IQ.Transacciones
        If Not IsNumeric(txtNewPropuesta.Text) Then
            ShowNotification("El número de propuesta debe ser numérica", ShowNotifications.ErrorNotification)
            ActivateAccordion(3, EffectActivateAccordion.NoEffect)
        End If
        o.DuplicarAlternativaToPropuesta(hfpD.Value, hfaD.Value, txtNewPropuesta.Text)
        ShowNotification("Alternativa duplicada a la propuesta" & txtNewPropuesta.Text, ShowNotifications.InfoNotificationLong)
        txtNewPropuesta.Text = ""
        uPanelDuplicacion.Update()
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub gvAlternativas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvAlternativas.RowCommand
        If e.CommandName = "Duplicar" Then
            hfaD.Value = gvAlternativas.DataKeys(e.CommandArgument)("Alternativa")
            hfpD.Value = hfidpropuesta.Value
            uPanelDuplicacion.Update()
        End If
        If e.CommandName = "VerlPresupuestos" Then
            hfAlternativa.Value = gvAlternativas.DataKeys(e.CommandArgument)("Alternativa")
            CargarPresupuestosPorAlternativa(hfidpropuesta.Value, hfAlternativa.Value)
        End If
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub gvPresupuestos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvPresupuestos.RowCommand
        If e.CommandName = "Duplicar" Then
            hfPPD.Value = hfidpropuesta.Value
            hfPAD.Value = gvPresupuestos.DataKeys(e.CommandArgument)("Alternativa")
            hfPMD.Value = gvPresupuestos.DataKeys(e.CommandArgument)("MetCodigo")
            hfPFD.Value = gvPresupuestos.DataKeys(e.CommandArgument)("ParNacional")
            CargarAlternativasDisponibles()
        End If
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub

    Sub CargarAlternativasDisponibles()
        Dim o As New IQ.Transacciones
        Me.ddlAlternativa.DataSource = o.AlternativasDisponibles(hfPPD.Value)
        'Me.ddlAlternativa.DataTextField = "Alternativa"
        'Me.ddlAlternativa.DataValueField = "Alternativa"
        Me.ddlAlternativa.DataBind()
        Me.UPanelDuplicacionPresupuesto.Update()
        CargarFasesDisponibles()
    End Sub

    Sub CargarFasesDisponibles()
        Dim o As New IQ.Transacciones
        Me.ddlFase.DataSource = o.FasesDisponibles(hfPPD.Value, ddlAlternativa.SelectedValue, hfPMD.Value)
        Me.ddlFase.DataTextField = "Fase"
        Me.ddlFase.DataValueField = "id"
        Me.ddlFase.DataBind()
        Me.UPanelDuplicacionPresupuesto.Update()
    End Sub

    Private Sub ddlAlternativa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAlternativa.SelectedIndexChanged
        CargarFasesDisponibles()
    End Sub

    Private Sub btnCopiarPresupuesto_Click(sender As Object, e As EventArgs) Handles btnCopiarPresupuesto.Click
        Dim o As New IQ.Transacciones
        If Not (IsNumeric(Me.ddlFase.SelectedValue)) Or Not (IsNumeric(Me.ddlAlternativa.SelectedValue)) Then
            ShowNotification("Los parametros no son correctos", ShowNotifications.ErrorNotification)
            ActivateAccordion(3, EffectActivateAccordion.NoEffect)
        End If
        o.CopiarPresupuesto(hfPPD.Value, hfPAD.Value, hfPMD.Value, hfPFD.Value, ddlAlternativa.SelectedValue, ddlFase.SelectedValue)
        ShowNotification("Presupuesto copiado correctamente", ShowNotifications.InfoNotificationLong)
        uPanelDuplicacion.Update()
        CargarPresupuestosPorAlternativa(hfidpropuesta.Value, hfAlternativa.Value)
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub
End Class