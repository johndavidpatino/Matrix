Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class CambiarGerenteCuentasBriefs
    Inherits System.Web.UI.Page
#Region " Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(145, UsuarioID) = False Then
                Response.Redirect("../MBO/Default.aspx")
            End If

            CargarBriefs()

            gvDatos.DataSource = CargarBriefs()
            gvDatos.DataBind()

        End If
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        gvDatos.DataSource = CargarBriefs()
        gvDatos.DataBind()
    End Sub

    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        gvDatos.DataSource = CargarBriefs()
        gvDatos.DataBind()
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
       
        If e.CommandName = "Modificar" Then
            hfidbrief.Value = gvDatos.DataKeys(e.CommandArgument)("Id")
            hfGerenteAnterior.Value = gvDatos.DataKeys(e.CommandArgument)("GerenteCuentas")
            CargarGerentesCuentas()
            upGerenteCuentasAsignar.Update()
        End If

    End Sub

#End Region

#Region "Funciones y Metodos"

    Sub CargarGerentesCuentas()

        Dim oUsuarios As New US.Usuarios

        Dim Usuarios = From x In oUsuarios.UsuariosxGrupoUnidadXrol(hfidGrupoUnidad.Value, 5)
                            Select New With {.ID = x.id, .Nombres = x.Nombres & " " & x.Apellidos}

        ddlGerente.DataSource = Usuarios.ToList
        ddlGerente.DataValueField = "ID"
        ddlGerente.DataTextField = "Nombres"
        ddlGerente.DataBind()
       
    End Sub

    Public Sub Limpiar()
        hfidbrief.Value = String.Empty
        hfGerenteAnterior.Value = String.Empty
    End Sub

    Function CargarBriefs() As List(Of CU_Brief_Get_Result)
        Dim oBusqueda As New Brief
        Dim oUsarios As New US.UsuariosUnidades
        Dim Usuarios = oUsarios.obtenerUsuariosGrupoUnidad(Session("IDUsuario").ToString).FirstOrDefault

        Dim TodosCampos As String = Nothing
        hfidGrupoUnidad.Value = Usuarios.IdGrupoUnidad

        If Not txtBuscar.Text = "" Then TodosCampos = txtBuscar.Text

        Return oBusqueda.ObtenerBriefsxGrupoUnidad(TodosCampos, hfidGrupoUnidad.Value)
    End Function

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim oBrief As New Brief
        oBrief.ActualizarGerenteCuentas(hfidbrief.Value, ddlGerente.SelectedValue)
        oBrief.GuardarLogCambioGerente(hfidbrief.Value, hfGerenteAnterior.Value, ddlGerente.SelectedValue, Session("IDUsuario").ToString)
        gvDatos.DataSource = CargarBriefs()
        gvDatos.DataBind()
        Limpiar()

    End Sub
#End Region


   
End Class