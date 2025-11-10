Imports CoreProject
Imports WebMatrix.Util

Public Class TrabajosFI
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

#Region "Metodos y Funciones"
    Sub CargarTrabajos()
        Dim oTrabajo As New GestionTrabajosFin
        gvTrabajos.DataSource = oTrabajo.ListadoTrabajoscc(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        gvTrabajos.DataBind()
    End Sub

    Function obtenerOPMetodologia(ByVal id As Int16) As OP_Metodologias_Get_Result
        Dim oMetodologiaOperaciones As New MetodologiaOperaciones
        Return oMetodologiaOperaciones.obtenerXId(id)
    End Function
    Function obtenerProyectoXId(ByVal id As Int64) As PY_Proyectos_Get_Result
        Dim oProyecto As New Proyecto
        Return oProyecto.obtenerXId(id)
    End Function
    Public Function CargarFichaCuantitativa() As Long
        Try
            Dim idtrabajo As Int64 = Int64.Parse(hfIdTrabajo.Value)
            Dim oTrabajo As New Trabajo
            Dim info = oTrabajo.DevolverxID(idtrabajo)

            Dim oFichaCuantitativa As New FichaCuantitativo
            Return oFichaCuantitativa.DevolverxTrabajoID(hfIdTrabajo.Value).Item(0).id

        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region

#Region "Eventos del Control"
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
        gvTrabajos.DataSource = oTrabajo.ListadoTrabajoscc(id, Nothing, Nombre, JobBook, Nothing, Nothing, Nothing, Nothing, Nothing, NoProp)
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
        ElseIf e.CommandName = "Presupuestos" Then
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Response.Redirect("../CC_FinzOpe/PresupuestosInternos.aspx?TrabajoId=" & hfIdTrabajo.Value)
        End If
    End Sub

    Sub CargarPreguntas()
        Dim oc As New PresupInt
        Dim o As New IQ.UCPreguntas
        Dim oT As New Trabajo
        Dim trabajoinfo = oT.ListadoTrabajos(hfIdTrabajo.Value, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
        Dim preg = o.TraerPreguntas(trabajoinfo.IdPropuesta, trabajoinfo.Alternativa, trabajoinfo.MetCodigo, trabajoinfo.Fase)
        If Not preg Is Nothing Then
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

        GvConteos.DataSource = oc.Conteos(hfIdTrabajo.Value)
        GvConteos.DataBind()

    End Sub
    Private Sub btnGuardarPreguntas_Click(sender As Object, e As System.EventArgs) Handles btnGuardarPreguntas.Click
        If Not (IsNumeric(CerradasMultReal.Text) And IsNumeric(CerradasReal.Text) And IsNumeric(AbiertasMultReal.Text) And IsNumeric(AbiertasReal.Text) And IsNumeric(OtrosReal.Text) And IsNumeric(DemoReal.Text)) Then
            ShowNotification("Todas las preguntas deben ser numéricas", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        Else
            'Dim op As New GestionTrabajosFin
            'Dim infop = op.ObtenerPreguntas(hfIdTrabajo.Value)
            'If Not infop Is Nothing Then
            '    infop.PregCerradasMultiples = CerradasMultReal.Text
            '    infop.PregCerradas = CerradasReal.Text
            '    infop.PregAbiertasMultiples = AbiertasMultReal.Text
            '    infop.PregAbiertas = AbiertasReal.Text
            '    infop.PregOtras = OtrosReal.Text
            '    infop.PregDemograficos = DemoReal.Text
            '    infop.TrabajoId = hfIdTrabajo.Value
            '    op.GuardarPreguntas(infop)
            'Else
            '    Dim infop2 As New CC_Preguntas
            '    infop2.PregCerradasMultiples = CerradasMultReal.Text
            '    infop2.PregCerradas = CerradasReal.Text
            '    infop2.PregAbiertasMultiples = AbiertasMultReal.Text
            '    infop2.PregAbiertas = AbiertasReal.Text
            '    infop2.PregOtras = OtrosReal.Text
            '    infop2.PregDemograficos = DemoReal.Text
            '    infop2.TrabajoId = hfIdTrabajo.Value
            '    op.GuardarPreguntas(infop2)
            'End If
            Dim o As New IQ.UCPreguntas
            Dim op As New ProcesosInternos
            Dim trabajoinfo = op.ObtenerTrabajoInfo(hfIdTrabajo.Value).Item(0)
			'Dim preg = o.TraerPreguntas(trabajoinfo.IdPropuesta, trabajoinfo.Alternativa, trabajoinfo.MetCodigo, trabajoinfo.Fase)
			Dim i = New IQ_MODEL
			Dim par = i.IQ_Parametros.First(Function(p) p.IdPropuesta = trabajoinfo.IdPropuesta And p.ParAlternativa = trabajoinfo.Alternativa And p.MetCodigo = trabajoinfo.MetCodigo And p.ParNacional = trabajoinfo.Fase)
			Dim duracionP = If(Not par Is Nothing, par.ParTiempoEncuesta, Nothing)

			op.GuardarConteo(trabajoinfo.JobBook, trabajoinfo.id, trabajoinfo.NombreTrabajo, trabajoinfo.ParUnidad, trabajoinfo.Pr_ProductCode, duracionP, CerradasReal.Text, CerradasMultReal.Text, AbiertasReal.Text, AbiertasMultReal.Text, OtrosReal.Text, DemoReal.Text, PagReal.Text, ObsReal.Text, Session("IDUsuario"))
			ShowNotification("Datos Guardados", ShowNotifications.InfoNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If

    End Sub


#End Region




End Class