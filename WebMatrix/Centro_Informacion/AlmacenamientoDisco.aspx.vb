Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports CoreProject.CIEntities
Imports ClosedXML.Excel
Imports CoreProject.OP

Public Class AlmacenamientoDisco
    Inherits System.Web.UI.Page


#Region "Eventos"

    Private Sub AlmacenamientoDisco_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(137, UsuarioID) = False Then
            Response.Redirect("../Home.aspx")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarIdMedios()
            cargarTrabajosCerrados()
        End If

    End Sub

    Private Sub gvTrabajosCerrados_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajosCerrados.PageIndexChanging
        gvMedios.PageIndex = e.NewPageIndex
    End Sub
    Private Sub gvTrabajosCerrados_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvTrabajosCerrados.RowCommand
        Select Case e.CommandName
            Case "Centroinformacion"
                pnlCierreActual.Visible = True
                restaurarPaso1()
                restaurarPaso2()
                hfIdTrabajo.Value = gvTrabajosCerrados.DataKeys(e.CommandArgument).Value
                lblTrabajoActual.Text = hfIdTrabajo.Value & " - " & gvTrabajosCerrados.Rows(e.CommandArgument).Cells(1).Text
                CargarMedios()
        End Select
    End Sub
    Private Sub gvMedios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMedios.RowCommand
        Dim lstDetalle As CI_DetalleAlmacenamiento_Get_Result
        If e.CommandName = "Actualizar" Then
            lstDetalle = obtenerMedioXId(gvMedios.DataKeys(e.CommandArgument).Value)
            CargarIdMedios()
            ddlIdMaestro.SelectedValue = lstDetalle.IdMedio
            txtContiene.Text = lstDetalle.Contiene
            txtObservacionDetalle.Text = lstDetalle.Observacion
            hfIdMedio.Value = gvMedios.DataKeys(e.CommandArgument).Value
        End If
    End Sub

    Protected Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click

        Dim oAgregar As New CentroInformacion

        lblIdMedio.Text = oAgregar.GuardarMaestroAlmacenamiento(Session("IDUsuario").ToString, txtObservacion.Text, ddlTipoAlmacenamiento.SelectedValue)

        restaurarPaso1()

        ShowNotification("Datos Guardados!", ShowNotifications.InfoNotification)
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)

        CargarIdMedios()

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim oGuardar As New CentroInformacion
        Try
            If String.IsNullOrEmpty(hfIdMedio.Value) Then
                oGuardar.GuardarDetalleAlmacenamiento(ddlIdMaestro.SelectedValue, hfIdTrabajo.Value, txtContiene.Text, txtObservacionDetalle.Text)
            Else
                oGuardar.ActualizarDetalleAlmacenamiento(hfIdMedio.Value, ddlIdMaestro.SelectedValue, hfIdTrabajo.Value, txtContiene.Text, txtObservacionDetalle.Text)
            End If

            Me.ddlTipoAlmacenamiento.SelectedValue = "-1"
            Me.txtObservacion.Text = ""

            CargarMedios()
            ShowNotification("El registro ha sido guardado correctamente", ShowNotifications.InfoNotification)
        Catch ex As Exception
            ShowNotification("A ocurrido un error - " & ex.Message, ShowNotifications.ErrorNotification)
        End Try

    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        restaurarPaso2()
    End Sub
    Protected Sub btnFinalizar_Click(sender As Object, e As EventArgs) Handles btnFinalizar.Click
        Dim daT As New Trabajo
        If gvMedios.Rows.Count > 0 Then
            daT.CambioEstado(hfIdTrabajo.Value, 14, "Cierre en centro de información", Session("IDUsuario").ToString())
            cargarTrabajosCerrados()
            restaurarTodo()
            ShowNotification("El trabajo ha sido cerrado satisfactoriamente", ShowNotifications.InfoNotification)
        Else
            ShowNotification("No se puede cerrar el trabajo, porque aún no se ha asociado un medio", ShowNotifications.InfoNotification)
        End If
        
    End Sub
#End Region
#Region "Metodos"
    Sub cargarTrabajosCerrados()
        Dim daT As New Trabajo
        gvTrabajosCerrados.DataSource = daT.ListadoTrabajos(Nothing, 12, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        gvTrabajosCerrados.DataBind()
    End Sub
    Public Sub CargarMedios()
        gvMedios.DataSource = obtenerMediosXTrabajo(hfIdTrabajo.Value)
        gvMedios.DataBind()
        If gvMedios.Rows.Count > 0 Then
            pnlMedios.Visible = True
            lblMsjMedios.Visible = True
        Else
            pnlMedios.Visible = False
            lblMsjMedios.Visible = False
        End If
    End Sub
    Function obtenerMedioXId(ByVal Id As Int64?) As CI_DetalleAlmacenamiento_Get_Result
        Dim RecordDetalle As New CentroInformacion
        Return RecordDetalle.obtenerdetallesxid(Id).FirstOrDefault
    End Function
    Sub CargarIdMedios()
        Dim oIdMaestro As New CentroInformacion
        Me.ddlIdMaestro.DataSource = oIdMaestro.obtenermaestrotodos
        Me.ddlIdMaestro.DataValueField = "Id"
        Me.ddlIdMaestro.DataTextField = "Id"
        Me.ddlIdMaestro.DataBind()
        Me.ddlIdMaestro.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub restaurarPaso1()
        Me.ddlTipoAlmacenamiento.SelectedValue = "-1"
        Me.txtObservacion.Text = ""
        lblIdMedio.Text = ""
        lblIdMedio.Visible = False
        lblMsgIdMedio.Visible = False

    End Sub
    Sub restaurarPaso2()
        Me.ddlIdMaestro.SelectedValue = -1

        Me.txtContiene.Text = ""
        Me.txtObservacionDetalle.Text = ""

        gvMedios.DataSource = Nothing
        gvMedios.DataBind()
    End Sub
    Sub restaurarTodo()
        restaurarPaso1()
        restaurarPaso2()
        pnlCierreActual.Visible = False
        hfIdTrabajo.Value = ""
        hfIdMedio.Value = ""
    End Sub
    Function obtenerMediosXTrabajo(ByVal idTrabajo As Int64) As List(Of CI_DetalleAlmacenamiento_Get_Result)
        Dim oBusqueda As New CentroInformacion
        Return oBusqueda.obtenerdetallesxidtrabajo(hfIdTrabajo.Value)
    End Function
#End Region
End Class