Imports CoreProject

Public Class Unidades
    Inherits System.Web.UI.Page

    Sub Limpiar()
        txtNombre.Text = String.Empty
    End Sub

    Function Validar() As Boolean
        Dim val As Boolean = True
        If txtNombre.Text = String.Empty Then
            lblResult.Text = "Especifique el nombre"
            val = False
        End If
        Return val
    End Function

    Public Function ConsultarU(ByVal Nombre As String, ByVal Padre As Integer) As List(Of Unidades_Result)
        Dim Data As New US.Unidades
        Dim Info As List(Of Unidades_Result)
        Try
            Info = Data.ObtenerUnidades(Nombre, Padre, Nothing)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Session("ListaCargada") = ConsultarU(txtBuscar.Text, Request.QueryString("IdGrupoUnidad"))
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("IdGrupoUnidad") IsNot Nothing Then
                Dim IdGrupoUnidad As Int64 = Int64.Parse(Request.QueryString("IdGrupoUnidad").ToString)
                CargarGrid(1)
            Else
                Response.Redirect("GrupoUnidad.aspx")
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
                    txtId.Text = Id
                    txtNombre.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Unidad")
                    datos.Visible = True
                    btnEditar.Visible = True
                    btnGuardar.Visible = False
                    lista.Visible = False
                Case "Eliminar"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Dim US As New US_Entities
                    US.US_Unidades_Del(Id)
                    CargarGrid(1)
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
            US.US_Unidades_Add(txtId.Text, txtNombre.Text, Request.QueryString("IdGrupoUnidad"))
            lblResult.Text = "Unidad agregada correctamente"
            Limpiar()
            CargarGrid(1)
            lista.Visible = True
            datos.Visible = False
        End If
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If Validar() = True Then
            Dim US As New US_Entities
            US.US_Unidades_Edit(txtId.Text, txtNombre.Text, Request.QueryString("IdGrupoUnidad"))
            lblResult.Text = "Unidad editada correctamente"
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