Imports CoreProject

Public Class AsignacionesProyectos
    Inherits System.Web.UI.Page

#Region "Metodos"
    Private Sub CargarUnidades()
        Dim oUnidades As New CoreProject.US.Unidades
        ddlUnidades.DataSource = oUnidades.ObtenerUnidadesxUsuario(Session("IDUsuario").ToString)
        ddlUnidades.DataTextField = "Unidad"
        ddlUnidades.DataValueField = "id"
        ddlUnidades.DataBind()
    End Sub

    Private Sub CargarProyectos()
        If Not IsNumeric(Me.ddlUnidades.SelectedValue) Then Exit Sub
        Dim oProyectos As New Proyecto
        Try
            gvDataProyectos.DataSource = oProyectos.obtenerXAsignarGerenteProyecto(ddlUnidades.SelectedValue)
            gvDataProyectos.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CargarProyectosReasignacion()
        If Not IsNumeric(Me.ddlUnidades.SelectedValue) Then Exit Sub
        Dim oProyectos As New Proyecto
        Try
            gvDataProyectosReasignar.DataSource = oProyectos.obtenerXReAsignarGerenteProyecto(ddlUnidades.SelectedValue, Nothing)
            gvDataProyectosReasignar.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Sub CargarGerentesProyectos()
        Try
            Dim oUsuarios As New US.Usuarios
            Dim oUnidades As New US.Unidades
            Dim GrupoUnidad = oUnidades.ObtenerGrupoUnidadxUnidad(hfIdUnidad.Value)
            Dim listapersonas = (From lpersona In oUsuarios.UsuariosxGrupoUnidadXrol(GrupoUnidad, ListaRoles.GerenteProyectos)
                                 Select Id = lpersona.id,
                                 Nombre = lpersona.Apellidos & " " & lpersona.Nombres).OrderBy(Function(p) p.Nombre)
            ddlUsuarios.DataSource = listapersonas.ToList()
            ddlUsuarios.DataValueField = "Id"
            ddlUsuarios.DataTextField = "Nombre"
            ddlUsuarios.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarUnidades()
            CargarProyectos()
        End If
    End Sub

    Protected Sub ddlUnidades_SelectedIndexChanged(sender As Object, e As EventArgs)
        If pnlAsignacion.Visible = True Then
            CargarProyectos()
        Else
            CargarProyectosReasignacion()
        End If
    End Sub

    Protected Sub gvDataProyectos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Select Case e.CommandName
            Case "Asignar"
                hfIDProyecto.Value = Int64.Parse(gvDataProyectos.DataKeys(CInt(e.CommandArgument))("Id"))
                hfIdUnidad.Value = Int64.Parse(gvDataProyectos.DataKeys(CInt(e.CommandArgument))("UnidadId"))
                CargarGerentesProyectos()
                ModalPopupExtenderGP.Show()
        End Select
    End Sub

    Protected Sub btnOk_Click(sender As Object, e As EventArgs)
        Dim oProyecto As New Proyecto
        oProyecto.ActualizarGerente(ddlUsuarios.SelectedValue, hfIDProyecto.Value)
        If pnlAsignacion.Visible = True Then
            CargarProyectos()
        Else
            CargarProyectosReasignacion()
        End If
    End Sub

    Protected Sub gvDataProyectosReasignar_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Select Case e.CommandName
            Case "Asignar"
                hfIDProyecto.Value = Int64.Parse(gvDataProyectosReasignar.DataKeys(CInt(e.CommandArgument))("Id"))
                hfIdUnidad.Value = Int64.Parse(gvDataProyectosReasignar.DataKeys(CInt(e.CommandArgument))("UnidadId"))
                CargarGerentesProyectos()
                ModalPopupExtenderGP.Show()
        End Select
    End Sub

    Protected Sub btnAsignacion_Click(sender As Object, e As EventArgs)
        pnlAsignacion.Visible = True
        pnlReasignacion.Visible = False
        CargarProyectos()
    End Sub

    Protected Sub btnReasignacion_Click(sender As Object, e As EventArgs)
        pnlAsignacion.Visible = False
        pnlReasignacion.Visible = True
        CargarProyectosReasignacion()
    End Sub
End Class