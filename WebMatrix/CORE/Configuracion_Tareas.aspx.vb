Imports CoreProject
Imports WebMatrix.Util

Public Class Configuracion_Tareas
    Inherits System.Web.UI.Page

#Region "Propiedades"

#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cargarTareas()
            CargarRoles()
            cargarUnidades()
            cargarListaTareas()
        End If
    End Sub

    Private Sub gvTareas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTareas.PageIndexChanging
        gvTareas.PageIndex = e.NewPageIndex
        cargarTareas()
    End Sub
    Private Sub gvTareas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareas.RowCommand
        If e.CommandName = "Editar" Then
            cargarTarea(gvTareas.DataKeys(e.CommandArgument)("Id"))
            hfIdTarea.Value = gvTareas.DataKeys(e.CommandArgument)("Id")
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        ElseIf e.CommandName = "DocumentosEntregables" Then
            Response.Redirect("Configuracion_Tareas_Documentos.aspx?IdTarea=" & gvTareas.DataKeys(e.CommandArgument)("Id") & "&IdTipoDocumento=2")
        ElseIf e.CommandName = "DocumentosRequeridos" Then
            Response.Redirect("Configuracion_Tareas_Documentos.aspx?IdTarea=" & gvTareas.DataKeys(e.CommandArgument)("Id") & "&IdTipoDocumento=1")
        End If
    End Sub
    Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGrabar.Click
        Dim idTarea As Int64?

        If Not (String.IsNullOrEmpty(hfIdTarea.Value)) Then
            idTarea = hfIdTarea.Value
        End If

        Dim oTarea As New CoreProject.Tarea
        oTarea.grabar(idTarea, txtTarea.Text, ddlNoEmpiezaAntesDe.SelectedValue, ddlNoTerminaAntesDe.SelectedValue, txtTiempoPromedioDias.Text, chkRequiereEstimacion.Checked, ddlRolEstima.SelectedValue, ddlUnidadEjecuta.SelectedValue, ddlUnidadRecibe.SelectedValue, ddlRolEjecuta.SelectedValue, chkVisible.Checked)

        ShowNotification("Registro almacenado correctamente", ShowNotifications.InfoNotification)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        limpiarControles()

    End Sub
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        limpiarControles()
    End Sub
#End Region
#Region "Metodos"
    Sub cargarTareas()
        Dim oCORE_Tareas As New CoreProject.Tarea
        gvTareas.DataSource = oCORE_Tareas.obtenerTodas
        gvTareas.DataBind()
    End Sub
    Sub cargarListaTareas()
        Dim oCORE_Tareas As New CoreProject.Tarea
        ddlNoEmpiezaAntesDe.DataSource = oCORE_Tareas.obtenerTodas
        ddlNoEmpiezaAntesDe.DataValueField = "Id"
        ddlNoEmpiezaAntesDe.DataTextField = "Tarea"
        ddlNoEmpiezaAntesDe.DataBind()
        ddlNoEmpiezaAntesDe.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})

        ddlNoTerminaAntesDe.DataSource = oCORE_Tareas.obtenerTodas
        ddlNoTerminaAntesDe.DataValueField = "Id"
        ddlNoTerminaAntesDe.DataTextField = "Tarea"
        ddlNoTerminaAntesDe.DataBind()
        ddlNoTerminaAntesDe.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})

    End Sub
    Sub CargarRoles()
        Dim Roles As New US.Roles
        ddlRolEjecuta.DataSource = Roles.ObtenerRolCombo
        ddlRolEjecuta.DataValueField = "Id"
        ddlRolEjecuta.DataTextField = "Rol"
        ddlRolEjecuta.DataBind()
        ddlRolEjecuta.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
        ddlRolEstima.DataSource = Roles.ObtenerRolCombo
        ddlRolEstima.DataValueField = "Id"
        ddlRolEstima.DataTextField = "Rol"
        ddlRolEstima.DataBind()
        ddlRolEstima.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
    Sub cargarUnidades()
        Dim Unidades As New CoreProject.Unidades
        ddlUnidadEjecuta.DataSource = Unidades.obtenerTodas
        ddlUnidadEjecuta.DataValueField = "Id"
        ddlUnidadEjecuta.DataTextField = "Unidad"
        ddlUnidadEjecuta.DataBind()
        ddlUnidadEjecuta.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
        ddlUnidadRecibe.DataSource = Unidades.obtenerTodas
        ddlUnidadRecibe.DataValueField = "Id"
        ddlUnidadRecibe.DataTextField = "Unidad"
        ddlUnidadRecibe.DataBind()
        ddlUnidadRecibe.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
    Sub cargarTarea(ByVal id As Int64)
        Dim oTarea As New CoreProject.Tarea
        Dim oeTarea As New CORE_Tareas_Get_Result

        oeTarea = oTarea.obtenerXId(id)

        txtTarea.Text = oeTarea.Tarea
        ddlNoEmpiezaAntesDe.SelectedValue = If(oeTarea.NoEmpiezaAntesDe.HasValue, oeTarea.NoEmpiezaAntesDe, -1)
        ddlNoTerminaAntesDe.SelectedValue = If(oeTarea.NoTerminaAntesDe.HasValue, oeTarea.NoTerminaAntesDe, -1)
        txtTiempoPromedioDias.Text = oeTarea.TiempoPromedioDias
        chkRequiereEstimacion.Checked = oeTarea.RequiereEstimacion
        ddlRolEstima.SelectedValue = If(oeTarea.RolEstima.HasValue, oeTarea.RolEstima, -1)
        chkVisible.Checked = oeTarea.Visible
        ddlUnidadEjecuta.SelectedValue = If(oeTarea.UnidadEjecuta.HasValue, oeTarea.UnidadEjecuta, -1)
        ddlUnidadRecibe.SelectedValue = If(oeTarea.UnidadRecibe.HasValue, oeTarea.UnidadRecibe, -1)
        ddlRolEjecuta.SelectedValue = If(oeTarea.RolEjecuta.HasValue, oeTarea.RolEjecuta, -1)
    End Sub
    Sub limpiarControles()
        txtTarea.Text = ""
        ddlNoEmpiezaAntesDe.SelectedValue = -1
        ddlNoEmpiezaAntesDe.SelectedValue = -1
        txtTiempoPromedioDias.Text = ""
        chkRequiereEstimacion.Checked = False
        ddlRolEstima.SelectedValue = -1
        chkVisible.Checked = False
        ddlUnidadEjecuta.SelectedValue = -1
        ddlUnidadRecibe.SelectedValue = -1
        ddlRolEjecuta.SelectedValue = -1
        hfIdTarea.Value = ""
    End Sub
#End Region
End Class