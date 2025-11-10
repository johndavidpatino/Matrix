Imports CoreProject

Public Class TipoUnidad
    Inherits System.Web.UI.Page

    Sub Limpiar()
        txtId.Text = String.Empty
        txtNombre.Text = String.Empty
    End Sub

    Function Validar() As Boolean
        Dim val As Boolean = True
        If txtId.Text = String.Empty Then
            lblResult.Text = "Especifique el id"
            val = False
        End If
        If txtNombre.Text = String.Empty Then
            lblResult.Text = "Especifique el nombre"
            val = False
        End If
        Return val
    End Function

    Public Function ConsultarTGU(ByVal Nombre As String) As List(Of US_TipoGrupoUnidad_Get_Result)
        Dim Data As New US.TipoGrupoUnidad
        Dim Info As List(Of US_TipoGrupoUnidad_Get_Result)
        Try
            Info = Data.ObtenerTipoGrupoUnidadNombre(Nombre)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Session("ListaCargada") = ConsultarTGU(txtBuscar.Text)
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(89, UsuarioID) = False Then
            Response.Redirect("../TH_TalentoHumano/Default.aspx")
        End If
        If Not Page.IsPostBack Then
            CargarGrid(1)
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        datos.Visible = True
        lista.Visible = False
        btnGuardar.Visible = True
        btnEditar.Visible = True
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarGrid(1)
    End Sub

    Protected Sub gvDatos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Editar"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Session("TGUId") = Id
                    txtId.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Id")
                    txtNombre.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("TipoGrupoUnidad")
                    datos.Visible = True
                    btnEditar.Visible = True
                    btnGuardar.Visible = False
                    lista.Visible = False
                Case "Eliminar"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Dim US As New US_Entities
                    US.US_TipoGrupoUnidad_Del(Id)
                    CargarGrid(1)
                Case "GrupoUnidad"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Response.Redirect("GrupoUnidad.aspx?IdTipoGrupoUnidad=" & Id.ToString)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Validar() = True Then
            Dim US As New US_Entities
            US.US_TipoGrupoUnidad_Add(txtId.Text, txtNombre.Text)
            lblResult.Text = "TGU agregada correctamente"
            Limpiar()
            CargarGrid(1)
            lista.Visible = True
            datos.Visible = False
        End If
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If Validar() = True Then
            Dim US As New US_Entities
            US.US_TipoGrupoUnidad_Edit(txtId.Text, txtNombre.Text)
            lblResult.Text = "TGU editado correctamente"
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

End Class