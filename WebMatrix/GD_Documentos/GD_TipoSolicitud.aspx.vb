Imports CoreProject
Public Class GD_TipoSolicitud
    Inherits System.Web.UI.Page

    Sub Limpiar()
        txtNombre.Text = String.Empty
        txtId.Text = String.Empty
        lblResult.Text = String.Empty
    End Sub

    Function Validar() As Boolean
        Dim val As Boolean = True
        If txtNombre.Text = String.Empty Then
            lblResult.Text = "Especifique el nombre"
            val = False
        End If
        'If txtId.Text = String.Empty Then
        '    lblResult.Text = "Especifique el id"
        '    val = False
        'End If
        Return val
    End Function

    Public Function ConsultarTipoSolicitud(ByVal Nombre As String) As List(Of GD_TipoSolicitud_Get_F_Result)
        Dim Data As New GD.GD_Procedimientos
        Dim Info As List(Of GD_TipoSolicitud_Get_F_Result)
        Try
            Info = Data.ObtenerTipoSolicitudxNombre(txtBuscar.Text)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Session("ListaCargada") = ConsultarTipoSolicitud(txtBuscar.Text)
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarGrid(1)
        End If
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        CargarGrid(1)
    End Sub

    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Editar"
                    lblResult.Text = String.Empty
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("id"))
                    txtId.Text = Id
                    txtId.Enabled = False
                    txtNombre.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Tipo")
                    datos.Visible = True
                    btnEditar.Visible = True
                    btnGuardar.Visible = False
                    lista.Visible = False
                Case "Eliminar"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("id"))
                    Dim GD As New GD.GD_Procedimientos
                    GD.EliminarTipoSolicitud(Id)
                    CargarGrid(1)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        datos.Visible = True
        txtId.Visible = False
        lblid.Visible = False
        lista.Visible = False
        btnEditar.Visible = False
        btnGuardar.Visible = True
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        If Validar() = True Then
            Dim GD As New GD.GD_Procedimientos
            GD.GuardarTipoSolicitud(txtNombre.Text)
            lblResult.Text = "Tipo Solicitud agregado correctamente"
            Limpiar()
            CargarGrid(1)
            lista.Visible = True
            datos.Visible = False
        End If
    End Sub

    Protected Sub btnEditar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditar.Click
        If Validar() = True Then
            Dim GD As New GD.GD_Procedimientos
            GD.EditarTipoSolicitud(txtId.Text, txtNombre.Text)
            lblResult.Text = "Tipo Solicitud editado correctamente"
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