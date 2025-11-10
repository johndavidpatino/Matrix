Imports CoreProject
Imports WebMatrix.Util

Public Class EstimacionProduccion
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Private _Bloqueada As Boolean
    Private _IDUsuario As Int64
    Private VolverCoord As Boolean
    Public Property IDUsuario() As Int64
        Get
            Return _IDUsuario
        End Get
        Set(ByVal value As Int64)
            _IDUsuario = value
        End Set
    End Property
#End Region

#Region "Funciones y Métodos"
    Sub CargarGv(ByVal TrabajoId As Int64)
        Dim oEstimacion As New PlaneacionProduccion
        Dim listaEstimacion = (From lEstim In oEstimacion.ObtenerEstimacionCiudadxTrabajoList(TrabajoId)
                             Select idEstim = lEstim.id, Ciudad = lEstim.C_Divipola.DivMuniNombre, Observaciones = lEstim.Observaciones,
                             FechaEstimacion = lEstim.FechaEstimacion, Activa = lEstim.Activa, Usuario = lEstim.US_Usuarios.Nombres & " " & lEstim.US_Usuarios.Apellidos).OrderByDescending(Function(x) x.FechaEstimacion)
        gvDatos.DataSource = listaEstimacion.ToList
        gvDatos.DataBind()
        gvEstimacion.DataSource = Nothing
        gvDatos.DataBind()
    End Sub
    Sub CargarGvPlaneacion(ByVal EstimacionId As Int64)
        Dim oEstimacion As New PlaneacionProduccion
        Dim listaEstimacion = (From lEstim In oEstimacion.ObtenerEstimacionxIdList(EstimacionId)
                             Select id = lEstim.id, Fecha = lEstim.Fecha, Cantidad = lEstim.Cantidad).OrderBy(Function(x) x.Fecha)
        gvEstimacion.DataSource = listaEstimacion.ToList
        gvEstimacion.DataBind()
    End Sub
    Function ActualizarEstimacion() As Boolean
        If Verificar() = True Then
            Dim oEstimacion As New PlaneacionProduccion
            For Each row As GridViewRow In gvEstimacion.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    If Not (IsNumeric(DirectCast(row.FindControl("tbCantidadEstimada"), TextBox).Text)) Then DirectCast(row.FindControl("tbCantidadEstimada"), TextBox).Text = 0
                    oEstimacion.GuardarEstimacion(Me.gvEstimacion.DataKeys(row.RowIndex).Value, DirectCast(row.FindControl("tbCantidadEstimada"), TextBox).Text)
                End If
            Next
            ShowNotification("Estimación actualizada", ShowNotifications.InfoNotification)
            Return True
        End If
        Return False
    End Function
    Function Verificar() As Boolean
        Dim suma As Long = 0
        For Each row As GridViewRow In gvEstimacion.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim valor As Long = 0
                If IsNumeric(DirectCast(row.FindControl("tbCantidadEstimada"), TextBox).Text) Then valor = DirectCast(row.FindControl("tbCantidadEstimada"), TextBox).Text
                suma = suma + (valor)
            End If
        Next
        Dim OCoordinacion As New CoordinacionCampo
        Dim oPlaneacion As New PlaneacionProduccion
        Dim Muestra As Long = OCoordinacion.ObtenerMuestraxTrabajoYCiudad(hfdiTrabajo.Value, oPlaneacion.ObtenerEstimacionCiudadxTrabajo(hfIdEstimacion.Value).CiudadId)
        If Not (Muestra = suma) Then
            ShowNotification("La muestra y la planeación no coinciden. La muestra es de " & Muestra & " y la planeción es " & suma, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            Return False
        End If
        Return True
    End Function
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("TrabajoId") IsNot Nothing Then
                Dim TrabajoId As Int64 = Int64.Parse(Request.QueryString("TrabajoId").ToString())
                hfdiTrabajo.Value = TrabajoId
                If Request.QueryString("Coordinador") IsNot Nothing Then
                    VolverCoord = True
                Else
                    VolverCoord = False
                End If
                CargarGv(TrabajoId)
            End If
        End If
    End Sub
    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarGv(hfdiTrabajo.Value)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        If e.CommandName = "Seleccionar" Then
            Dim idEstimacion As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("idEstim"))
            hfIdEstimacion.Value = idEstimacion
            Dim oEstimacion As New PlaneacionProduccion
            Dim eEstimacion = oEstimacion.ObtenerEstimacionCiudadxTrabajo(hfIdEstimacion.Value)
            If eEstimacion.Activa = False Or eEstimacion.Bloqueada = False Then
                _Bloqueada = False
            Else
                _Bloqueada = True
            End If

            CargarGvPlaneacion(hfIdEstimacion.Value)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            'Dim oCoordCampo As New CoordinacionCampo
            'oCoordCampo.EliminarMuestraXEstudio(idMuestra)
            'CargarGv(hfdiTrabajo.Value)
            'ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
        End If
    End Sub
    Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        If VolverCoord = True Then
            Response.Redirect("../OP_Cuantitativo/TrabajosCoordinador.aspx")
        Else
            Response.Redirect("../OP_Cuantitativo/Trabajos.aspx")
        End If
    End Sub
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnCAncel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCAncel.Click
        'txtFechaInicio.Text = ""
        'txtFechaTerminacion.Text = ""
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvEstimacion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEstimacion.RowDataBound
        If _Bloqueada = False Then
            If e.Row.RowType = DataControlRowType.DataRow Then
                DirectCast(e.Row.FindControl("tbCantidadEstimada"), TextBox).Enabled = True
            End If
        End If
    End Sub
    Protected Sub btnActualizarEstimacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnActualizarEstimacion.Click
        ActualizarEstimacion()
    End Sub
    Protected Sub btnGenerarPlaneacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenerarPlaneacion.Click
        Dim oPlaneacion As New PlaneacionProduccion
        Dim Ent As New OP_EstimacionesProduccionCiudad
        Ent = oPlaneacion.AgregarEstimacionCiudad(hfdiTrabajo.Value, Session("IDUsuario").ToString, Me.chbDias.Items(0).Selected, Me.chbDias.Items(1).Selected, Me.chbDias.Items(2).Selected, Me.chbDias.Items(3).Selected, Me.chbDias.Items(4).Selected, Me.chbDias.Items(5).Selected, Me.chbDias.Items(6).Selected, chbFestivosExcluir.Checked, tbObservaciones.Text, oPlaneacion.ObtenerEstimacionCiudadxTrabajo(hfIdEstimacion.Value).CiudadId)
        CargarGv(hfdiTrabajo.Value)
        hfIdEstimacion.Value = Ent.id
        CargarGvPlaneacion(hfIdEstimacion.Value)
        ShowNotification("Nueva planeación agregada y distribución automática realizada", ShowNotifications.InfoNotification)
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnActivarEstimacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnActivarEstimacion.Click
        If ActualizarEstimacion() = True Then
            Dim oPlaneacion As New PlaneacionProduccion
            oPlaneacion.ActivarEstimacion(hfIdEstimacion.Value)
            CargarGv(hfdiTrabajo.Value)
            ShowNotification("Planeación activada", ShowNotifications.InfoNotification)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End If
    End Sub
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
#End Region

End Class