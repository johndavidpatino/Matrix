Imports CoreProject

Public Class GrupoUnidad
    Inherits System.Web.UI.Page

    Sub Limpiar()
        txtNombre.Text = String.Empty
        chkActivo.Checked = False
    End Sub

    Function Validar() As Boolean
        Dim val As Boolean = True
        If txtNombre.Text = String.Empty Then
            lblResult.Text = "Especifique el nombre"
            val = False
        End If
        Return val
    End Function

    Public Function ConsultarGU(ByVal Nombre As String, ByVal Padre As Integer) As List(Of GrupoUnidad_Result)
        Dim Data As New US.GrupoUnidad
        Dim Info As List(Of GrupoUnidad_Result)
        Try
            Info = Data.ObtenerGrupoUnidad(Nombre, Padre)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Session("ListaCargada") = ConsultarGU(txtBuscar.Text, Request.QueryString("IdTipoGrupoUnidad"))
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("IdTipoGrupoUnidad") IsNot Nothing Then
                Dim IdTipoGrupoUnidad As Int64 = Int64.Parse(Request.QueryString("IdTipoGrupoUnidad").ToString)
                CargarGrid(1)
            Else
                Response.Redirect("TipoGrupoUnidad.aspx")
            End If
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarGrid(1)
    End Sub

    Protected Sub gvDatos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Editar"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Session("GUId") = Id
                    txtNombre.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("GrupoUnidad")
                    chkActivo.Checked = gvDatos.DataKeys(CInt(e.CommandArgument))("Activo")
                    datos.Visible = True
                    btnEditar.Visible = True
                    btnGuardar.Visible = False
                    lista.Visible = False
                Case "Eliminar"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Dim US As New US_Entities
                    US.US_GrupoUnidad_Del(Id)
                    CargarGrid(1)
                Case "Unidad"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Response.Redirect("Unidades.aspx?IdGrupoUnidad=" & Id.ToString)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        datos.Visible = True
        lista.Visible = False
        btnEditar.Visible = False
        btnGuardar.Visible = True
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Validar() = True Then
            Dim US As New US_Entities
            US.US_GrupoUnidad_Add(txtNombre.Text, Request.QueryString("IdTipoGrupoUnidad"), chkActivo.Checked)
            lblResult.Text = "GU agregada correctamente"
            Limpiar()
            CargarGrid(1)
            lista.Visible = True
            datos.Visible = False
        End If
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If Validar() = True Then
            Dim US As New US_Entities
            US.US_GrupoUnidad_Edit(Session("GUId"), txtNombre.Text, Request.QueryString("IdTipoGrupoUnidad"), chkActivo.Checked)
            lblResult.Text = "GU editada correctamente"
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