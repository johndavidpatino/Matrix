Imports CoreProject
Public Class ConfiguracionTareasXHilo
    Inherits System.Web.UI.Page


    Private _TipoHiloId As Int64
    Public Property TipoHiloId() As Int64
        Get
            Return _TipoHiloId
        End Get
        Set(ByVal value As Int64)
            _TipoHiloId = value
        End Set
    End Property

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not (Request.QueryString("TipoHiloId") Is Nothing) Then
            Int64.TryParse(Request.QueryString("TipoHiloId"), TipoHiloId)
        End If

        If Not IsPostBack Then
            cargarTiposHilos()
            If TipoHiloId > 0 Then
                cargarTareas(gvTareasAsignadas, TipoHiloId, True)
                cargarTareas(gvTareasNoAsignadas, TipoHiloId, False)
                ddlTiposHilos.SelectedValue = TipoHiloId
            End If
        End If

    End Sub
    Protected Sub ddlTiposHilos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTiposHilos.SelectedIndexChanged
        cargarTareas(gvTareasAsignadas, ddlTiposHilos.SelectedValue, True)
        cargarTareas(gvTareasNoAsignadas, ddlTiposHilos.SelectedValue, False)
    End Sub
    Private Sub gvTareasAsignadas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTareasAsignadas.PageIndexChanging
        gvTareasAsignadas.PageIndex = e.NewPageIndex
        cargarTareas(gvTareasAsignadas, ddlTiposHilos.SelectedValue, True)
    End Sub
    Private Sub gvTareasNoAsignadas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTareasNoAsignadas.PageIndexChanging
        gvTareasNoAsignadas.PageIndex = e.NewPageIndex
        cargarTareas(gvTareasNoAsignadas, ddlTiposHilos.SelectedValue, False)
    End Sub
    Private Sub gvTareasNoAsignadas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasNoAsignadas.RowCommand
        If e.CommandName = "Adicionar" Then
            Dim oTipoHilo_Tareas As New TipoHilo_Tareas
            oTipoHilo_Tareas.grabar(ddlTiposHilos.SelectedValue, gvTareasNoAsignadas.DataKeys(e.CommandArgument)("Id"))
            cargarTareas(gvTareasAsignadas, ddlTiposHilos.SelectedValue, True)
            cargarTareas(gvTareasNoAsignadas, ddlTiposHilos.SelectedValue, False)
        End If
    End Sub

    Private Sub gvTareasAsignadas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareasAsignadas.RowCommand
        If e.CommandName = "Quitar" Then
            Dim oTipoHilo_Tareas As New TipoHilo_Tareas
            oTipoHilo_Tareas.Eliminar(ddlTiposHilos.SelectedValue, gvTareasAsignadas.DataKeys(e.CommandArgument)("Id"))
            cargarTareas(gvTareasAsignadas, ddlTiposHilos.SelectedValue, True)
            cargarTareas(gvTareasNoAsignadas, ddlTiposHilos.SelectedValue, False)
        ElseIf e.CommandName = "TareasPrevias" Then
            Response.Redirect("Configuracion_Tareas_Previas.aspx?IdTarea=" & gvTareasAsignadas.DataKeys(e.CommandArgument)("Id") & "&IdTipoHilo=" & ddlTiposHilos.SelectedValue)
        End If
    End Sub
#End Region
#Region "Metodos"
    Sub cargarTiposHilos()
        Dim oTiposHilos As New TiposHilos
        ddlTiposHilos.DataSource = oTiposHilos.obtenerTodos
        ddlTiposHilos.DataValueField = "Id"
        ddlTiposHilos.DataTextField = "Hilo"
        ddlTiposHilos.DataBind()
        ddlTiposHilos.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
    Sub cargarTareas(ByVal gv As GridView, ByVal tipoHiloId As Int64, ByVal Asignadas As Boolean)
        Dim oTipoHilo_Tareas As New TipoHilo_Tareas
        gv.DataSource = oTipoHilo_Tareas.obtenerXTipoHiloId(tipoHiloId, Asignadas)
        gv.DataBind()
    End Sub
#End Region


End Class