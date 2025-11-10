Imports CoreProject

Public Class Revision
    Inherits System.Web.UI.Page

    Sub Limpiar()
        txtRutaArchivo.Text = ""
        txtMetRec.Text = ""
        txtTiempoRete.Text = ""
        txtDisposicion.Text = ""
    End Sub

    Private DocumentoId As Integer
    Private RevisionId As Integer

    Public Property DID As Integer
        Get
            Return ViewState("DocumentoId")
        End Get
        Set(value As Integer)
            ViewState("DocumentoId") = value
        End Set
    End Property

    Public Property RID As Integer
        Get
            Return ViewState("RevisionId")
        End Get
        Set(value As Integer)
            ViewState("RevisionId") = value
        End Set
    End Property

    Function Validar() As Boolean
        Dim val As Boolean = True
        If txtRutaArchivo.Text = String.Empty Then
            val = False
            lblResult.Text = "Diligencie la ruta"
        End If
        If txtMetRec.Text = String.Empty Then
            val = False
            lblResult.Text = "Diligencie la meta"
        End If
        If txtTiempoRete.Text = String.Empty Then
            val = False
            lblResult.Text = "Diligencie el tiempo de retención"
        End If
        If txtDisposicion.Text = String.Empty Then
            val = False
            lblResult.Text = "Diligencie la disposición"
        End If
        Return val
    End Function

    Public Function ConsultarRevision(ByVal IdUsuario As Integer) As List(Of GD_Revisiones_Get_Result)
        Dim Data As New GD.GD_Procedimientos
        Dim Info As List(Of GD_Revisiones_Get_Result)
        Try
            Info = Data.ObtenerRevisionUsuario(IdUsuario)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Dim IdUsuario As Integer = Request.QueryString("IdUsuario").ToString
                Session("ListaCargada") = ConsultarRevision(IdUsuario)
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("IdUsuario") IsNot Nothing Then
                Dim IdUsuario As Int64 = Int64.Parse(Request.QueryString("IdUsuario").ToString)
                CargarGrid(1)
            Else
                'Response.Redirect("Usuarios.aspx")
                lista.Visible = False
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
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("IdRevision"))
                    RID = Id
                    DID = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("DocumentoId"))
                    Dim dtpl = "<style type='text/css'>" _
                    & ".form-row {overflow:hidden; font-size:11px; border-bottom:1.5px solid #eee; }" _
                    & "</style>" _
                    & "<style type='text/css'>" _
                    & ".form-titles {overflow:hidden; font-size:11px; width:120px;line-height:25px; }" _
                    & "</style>" _
                    & "<div style='margin: 10px 10px 10px 10px; width:98%; background-color:White;'>" _
                    & "<table style='width:98%;'>" _
                    & "<tr><td class='form-titles'><strong>Documento ID:</strong></td>" _
                    & "<td class='form-row'>" & gvDatos.DataKeys(CInt(e.CommandArgument))("DocumentoId") & "</td></tr>" _
                    & "<tr><td class='form-titles'><strong>Usuario</strong></td>" _
                    & "<td class='form-row'>" & gvDatos.DataKeys(CInt(e.CommandArgument))("UsuarioId") & "</td></tr>" _
                    & "<tr><td class='form-titles'><strong>Tipo de revisión:</strong></td>" _
                    & "<td class='form-row'>" & gvDatos.DataKeys(CInt(e.CommandArgument))("TipoRevision") & "</td></tr>" _
                    & "<tr><td class='form-titles'><strong>Documento:</strong></td>" _
                    & "<td class='form-row'>" & gvDatos.DataKeys(CInt(e.CommandArgument))("NombreDocumento") & "</td></tr>" _
                    & "</table>" _
                    & "</div>"
                    TPL.InnerHtml = dtpl
                    txtRutaArchivo.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("NombreDocumento")
                    datos.Visible = True
                    btnGuardar.Visible = True
                    lista.Visible = False
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Validar() = True Then
            Dim US As New GD.GD_Procedimientos
            US.editarRevision(RID, DID, Request.QueryString("IdUsuario").ToString, Date.UtcNow.AddHours(-5), 2)
            US.IngresarDocumentoControlado(DID, True, txtRutaArchivo.Text, txtMetRec.Text, txtTiempoRete.Text, txtDisposicion.Text)
            lblResult.Text = "Documento revisado correctamente"
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