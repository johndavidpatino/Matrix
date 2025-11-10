Imports CoreProject

Public Class UsuariosUnidades
    Inherits System.Web.UI.Page

    Public Function ObtenerTipoGrupoUnidad() As List(Of TipoGrupoUnidadCombo_Result)
        Dim Data As New US.TipoGrupoUnidad
        Try
            Return Data.ObtenerTipoGrupoUnidadCombo()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerGrupoUnidad(ByVal TipoGrupo As Integer) As List(Of GrupoUnidadCombo_Result)
        Dim Data As New US.GrupoUnidad
        Try
            Return Data.ObtenerGrupoUnidadCombo(TipoGrupo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerUnidad(ByVal Grupo As Integer) As List(Of UnidadesCombo_Result)
        Dim Data As New US.Unidades
        Try
            Return Data.ObtenerUnidadCombo(Grupo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.ClearSelection()
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()
    End Sub

    Sub Limpiar()
    End Sub

    Function Validar() As Boolean
        Dim val As Boolean = True
        Return val
    End Function

    Public Function Consultar(ByVal UsuarioId As String) As List(Of UsuariosUnidades_Result)
        Dim Data As New US.UsuariosUnidades
        Dim Info As List(Of UsuariosUnidades_Result)
        Try
            Info = Data.ObtenerUsuariosUnidades(UsuarioId)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Session("ListaCargada") = Consultar(Request.QueryString("IdUsuario"))
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("IdUsuario") IsNot Nothing Then
                Dim IdUsuario As Int64 = Int64.Parse(Request.QueryString("IdUsuario").ToString)
                CargarGrid(1)
                CargarCombo(ddlTipoGrupoUnidad, "Id", "TipoGrupoUnidad", ObtenerTipoGrupoUnidad)
                CargarCombo(ddlGrupoUnidad, "Id", "GrupoUnidad", ObtenerGrupoUnidad(ddlTipoGrupoUnidad.SelectedValue))
                CargarCombo(ddlUnidad, "Id", "Unidad", ObtenerUnidad(ddlGrupoUnidad.SelectedValue))
            Else
                Response.Redirect("Usuarios.aspx")
            End If
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        datos.Visible = True
        lista.Visible = False
        btnGuardar.Visible = True
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Validar() = True Then
            Dim US As New US.UsuariosUnidades
            US.GuardarUsuariosUnidades(Request.QueryString("IdUsuario"), ddlUnidad.SelectedValue)
            lblResult.Text = "GU agregada correctamente"
            Limpiar()
            CargarGrid(1)
            lista.Visible = True
            datos.Visible = False
        End If
    End Sub

    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        gvDatos.DataSource = Session("ListaCargada")
        gvDatos.DataBind()
    End Sub

    Protected Sub ddlTipoGrupoUnidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoGrupoUnidad.SelectedIndexChanged
        CargarCombo(ddlGrupoUnidad, "Id", "GrupoUnidad", ObtenerGrupoUnidad(ddlTipoGrupoUnidad.SelectedValue))
        CargarCombo(ddlUnidad, "Id", "Unidad", ObtenerUnidad(ddlGrupoUnidad.SelectedValue))
    End Sub

    Protected Sub ddlGrupoUnidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGrupoUnidad.SelectedIndexChanged
        CargarCombo(ddlUnidad, "Id", "Unidad", ObtenerUnidad(ddlGrupoUnidad.SelectedValue))
    End Sub
End Class