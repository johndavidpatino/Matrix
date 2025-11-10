Imports CoreProject
Imports WebMatrix.Util

Public Class ConteoTrabajos
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
#End Region



#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(100, UsuarioID) = False Then
            Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
        End If
        If Not IsPostBack Then
            CargarTrabajos()
            If Not Session("TrabajoId") = Nothing Then
                hfIdTrabajo.Value = Session("TrabajoId").ToString
            End If
        End If
        If Not IsPostBack Then
            lbtnVolver.PostBackUrl = "~/OP_Cuantitativo/HomeRecoleccion.aspx"
        End If
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Dim oTrabajo As New GestionTrabajosFin
        Dim id As Int64? = Nothing
        Dim JobBook As String = Nothing
        Dim Nombre As String = Nothing
        Dim NoProp As Int64? = Nothing
        If Not (txtID.Text = "") Then id = txtID.Text
        If Not (txtJobBook.Text = "") Then JobBook = txtJobBook.Text
        If Not (txtNombreTrabajo.Text = "") Then Nombre = txtNombreTrabajo.Text
        If Not (txtNoPropuesta.Text = "") Then NoProp = txtNoPropuesta.Text
        gvTrabajos.DataSource = oTrabajo.ListadoTrabajosConteo(id, Nothing, Nombre, JobBook, Nothing, Nothing, Nothing, Nothing, Nothing, NoProp)
        gvTrabajos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarTrabajos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Actualizar" Then
            'cargarTrabajo(Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")))
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Session("TrabajoId") = hfIdTrabajo.Value
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        ElseIf e.CommandName = "Conteo" Then
            Me.pnlPreguntas.Visible = True
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            CargarPreguntas()
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        ElseIf e.CommandName = "Procesos" Then
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Response.Redirect("../CC_FinzOpe/ActividadesTrabajo.aspx?TrabajoId=" & hfIdTrabajo.Value)
        End If
    End Sub
    Protected Sub btnGuardarPreguntas_Click(sender As Object, e As EventArgs) Handles btnGuardarPreguntas.Click
		If Not (IsNumeric(DuracionReal.Text) And IsNumeric(CerradasMultReal.Text) And IsNumeric(CerradasReal.Text) And IsNumeric(AbiertasMultReal.Text) And IsNumeric(AbiertasReal.Text) And IsNumeric(OtrosReal.Text) And IsNumeric(DemoReal.Text)) Then
			ShowNotification("Todas las preguntas deben ser numéricas", ShowNotifications.ErrorNotification)
			ActivateAccordion(1, EffectActivateAccordion.NoEffect)
		Else
			Dim o As New IQ.UCPreguntas
            Dim op As New ProcesosInternos
            Dim trabajoinfo = op.ObtenerTrabajoInfo(hfIdTrabajo.Value).Item(0)
            'Dim preg = o.TraerPreguntas(trabajoinfo.IdPropuesta, trabajoinfo.Alternativa, trabajoinfo.MetCodigo, trabajoinfo.Fase)

            op.GuardarConteo(trabajoinfo.JobBook, trabajoinfo.id, trabajoinfo.NombreTrabajo, trabajoinfo.ParUnidad, trabajoinfo.Pr_ProductCode, DuracionReal.Text, CerradasReal.Text, CerradasMultReal.Text, AbiertasReal.Text, AbiertasMultReal.Text, OtrosReal.Text, DemoReal.Text, PagReal.Text, ObsReal.Text, Session("IDUsuario"))
            ShowNotification("Datos Guardados", ShowNotifications.InfoNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If
        CargarPreguntas()
        limpiar()
    End Sub
#End Region

#Region "Metodos y Funciones"
    Sub CargarTrabajos()
        Dim oTrabajo As New GestionTrabajosFin
        gvTrabajos.DataSource = oTrabajo.ListadoTrabajosConteo(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        gvTrabajos.DataBind()
    End Sub
    Sub CargarPreguntas()
        Dim oc As New PresupInt
		Dim o As New IQ.UCPreguntas
		Dim oT As New Trabajo
		Dim trabajoinfo = oT.ListadoTrabajos(hfIdTrabajo.Value, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
        Dim preg = o.TraerPreguntas(trabajoinfo.IdPropuesta, trabajoinfo.Alternativa, trabajoinfo.MetCodigo, trabajoinfo.Fase)
		If Not preg Is Nothing Then
			Dim i = New IQ_MODEL
			Dim par = i.IQ_Parametros.First(Function(p) p.IdPropuesta = trabajoinfo.IdPropuesta And p.ParAlternativa = trabajoinfo.Alternativa And p.MetCodigo = trabajoinfo.MetCodigo And p.ParNacional = trabajoinfo.Fase)
			Dim duracionP = If(Not par Is Nothing, par.ParTiempoEncuesta, Nothing)
			DuracionProp.Text = duracionP.ToString()
			CerradasProp.Text = preg.PregCerradas
			CerradasMultProp.Text = preg.PregCerradasMultiples
			AbiertasMultProp.Text = preg.PregAbiertasMultiples
			AbiertasProp.Text = preg.PregAbiertas
			OtrosProp.Text = preg.PregOtras
			DemoProp.Text = preg.PregDemograficos
		End If

		Dim op As New GestionTrabajosFin
        Dim infop = op.ObtenerPreguntas(hfIdTrabajo.Value)
		If Not infop Is Nothing Then
			CerradasMultReal.Text = infop.PregCerradasMultiples
			CerradasReal.Text = infop.PregCerradas
			AbiertasMultReal.Text = infop.PregAbiertasMultiples
			AbiertasReal.Text = infop.PregAbiertas
			OtrosReal.Text = infop.PregOtras
			DemoReal.Text = infop.PregDemograficos
		End If

		Dim conteo = oc.Conteos(hfIdTrabajo.Value)
		GvConteos.DataSource = conteo
		GvConteos.DataBind()

		If conteo.Count > 0 Then
			'Diferencias
			If DuracionProp.Text <> "" AndAlso Convert.ToDouble(DuracionProp.Text) <> 0 And Convert.ToInt64(conteo.FirstOrDefault.Duracion) <> 0 Then
				DuracionD.Text = Convert.ToString(Math.Round((Convert.ToInt64(conteo.FirstOrDefault.Duracion) / Convert.ToInt64(DuracionProp.Text) - 1), 2) * 100) + " %"
			ElseIf DuracionProp.Text = 0 Or DuracionProp.Text = "" Then
				DuracionD.Text = "100 %"
			End If
			If CerradasProp.Text <> "" AndAlso Convert.ToDouble(CerradasProp.Text) <> 0 And Convert.ToInt64(conteo.FirstOrDefault.CerradasRU) <> 0 Then
				CerradasD.Text = Convert.ToString(Math.Round((Convert.ToInt64(conteo.FirstOrDefault.CerradasRU) / Convert.ToInt64(CerradasProp.Text) - 1), 2) * 100) + " %"
			ElseIf CerradasProp.Text = 0 Or CerradasProp.Text = "" Then
				CerradasD.Text = "100 %"
			End If
			If CerradasMultProp.Text <> "" AndAlso Convert.ToDouble(CerradasMultProp.Text) <> 0 And Convert.ToInt64(conteo.FirstOrDefault.CerradasRM) <> 0 Then
				CerradasMultiplesD.Text = Convert.ToString(Math.Round((Convert.ToInt64(conteo.FirstOrDefault.CerradasRM) / Convert.ToInt64(CerradasMultProp.Text) - 1), 2) * 100) + " %"
			ElseIf CerradasMultProp.Text = 0 Or CerradasMultProp.Text = "" Then
				CerradasMultiplesD.Text = "100 %"
			End If
			If AbiertasProp.Text <> "" AndAlso Convert.ToDouble(AbiertasProp.Text) <> 0 And Convert.ToInt64(conteo.FirstOrDefault.Abiertas) <> 0 Then
				AbiertasD.Text = Convert.ToString(Math.Round((Convert.ToInt64(conteo.FirstOrDefault.Abiertas) / Convert.ToInt64(AbiertasProp.Text) - 1), 2) * 100) + " %"
			ElseIf AbiertasProp.Text = 0 Or AbiertasProp.Text = "" Then
				AbiertasD.Text = "100 %"
			End If
			If AbiertasMultProp.Text <> "" AndAlso Convert.ToDouble(AbiertasMultProp.Text) <> 0 And Convert.ToInt64(conteo.FirstOrDefault.AbiertasMul) <> 0 Then
				AbiertasMultiplesD.Text = Convert.ToString(Math.Round((Convert.ToInt64(conteo.FirstOrDefault.AbiertasMul) / Convert.ToInt64(AbiertasMultProp.Text) - 1), 2) * 100) + " %"
			ElseIf AbiertasMultProp.Text = 0 Or AbiertasMultProp.Text = "" Then
				AbiertasMultiplesD.Text = "100 %"
			End If
			If OtrosProp.Text <> "" AndAlso Convert.ToDouble(OtrosProp.Text) <> 0 And Convert.ToInt64(conteo.FirstOrDefault.Otros) <> 0 Then
				OtrosD.Text = Convert.ToString(Math.Round((Convert.ToInt64(conteo.FirstOrDefault.Otros) / Convert.ToInt64(OtrosProp.Text) - 1), 2) * 100) + " %"
			ElseIf OtrosProp.Text = 0 Or OtrosProp.Text = "" Then
				OtrosD.Text = "100 %"
			End If
			If DemoProp.Text <> "" AndAlso Convert.ToDouble(DemoProp.Text) <> 0 And Convert.ToInt64(conteo.FirstOrDefault.Demograficos) <> 0 Then
				DemograficosD.Text = Convert.ToString(Math.Round((Convert.ToInt64(conteo.FirstOrDefault.Demograficos) / Convert.ToInt64(DemoProp.Text) - 1), 2) * 100) + " %"
			ElseIf DemoProp.Text = 0 Or DemoProp.Text = "" Then
				DemograficosD.Text = "100 %"
			End If
			'If PagProp.Text <> "" AndAlso Convert.ToDouble(PagProp.Text) <> 0 And Convert.ToInt64(conteo.FirstOrDefault.Paginas) <> 0 Then
			'	PaginasD.Text = Convert.ToString(Math.Round((Convert.ToInt64(conteo.FirstOrDefault.Paginas) / Convert.ToInt64(PagProp.Text) - 1), 2) * 100) + " %"
			'End If
		End If

	End Sub
	Sub limpiar()
		DuracionReal.Text = String.Empty
		CerradasReal.Text = String.Empty
		CerradasMultReal.Text = String.Empty
        AbiertasReal.Text = String.Empty
        AbiertasMultReal.Text = String.Empty
        OtrosReal.Text = String.Empty
        DemoReal.Text = String.Empty
        PagReal.Text = String.Empty
        ObsReal.Text = String.Empty
    End Sub

#End Region



End Class