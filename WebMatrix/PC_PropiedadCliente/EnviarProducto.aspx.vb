Imports CoreProject
Imports System.Globalization

Public Class EnviarProducto
    Inherits System.Web.UI.Page

    Private _id As Integer
    Private _desc As String
    Private _uni As String
    Private _nuni As String
    Private _cant As Integer

    Public Property PIId As Integer
        Get
            Return ViewState("_id")
        End Get
        Set(value As Integer)
            ViewState("_id") = value
        End Set
    End Property

    Public Property Desc As String
        Get
            Return ViewState("_desc")
        End Get
        Set(value As String)
            ViewState("_desc") = value
        End Set
    End Property

    Public Property Unidad As String
        Get
            Return ViewState("_uni")
        End Get
        Set(value As String)
            ViewState("_uni") = value
        End Set
    End Property

    Public Property NUnidad As String
        Get
            Return ViewState("_nuni")
        End Get
        Set(value As String)
            ViewState("_nuni") = value
        End Set
    End Property

    Public Property Cant As Integer
        Get
            Return ViewState("_cant")
        End Get
        Set(value As Integer)
            ViewState("_cant") = value
        End Set
    End Property

    Public Function ObtenerUnidad() As List(Of UnidadCombo_Result)
        Dim Data As New SG.Actas
        Try
            Return Data.ObtenerUnidad
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerUsuarios() As List(Of ObtenerUsuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Try
            Return Data.ObtenerUsuarios
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerTipoMovimiento() As List(Of CU_TipoMovimientoCombo_Result)
        Dim Data As New CU.ProductoInterno
        Try
            Return Data.ObtenerTipoMovimiento
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()
    End Sub

    Sub Limpiar()
        txtNombre.Text = String.Empty
    End Sub

    Function Validar() As Boolean
        Dim val As Boolean = True
        If txtNombre.Text = String.Empty Then
            lblResult.Text = "Especifique el nombre"
            val = False
        End If
        If txtCantidad.Text = String.Empty Then
            lblResult.Text = "Especifique la cantidad"
            val = False
        End If
        If txtDescripcion.Text = String.Empty Then
            lblResult.Text = "Especifique la descripción"
            val = False
        End If
        Return val
    End Function

    Public Function ConsultarProductoInternoRecibe(ByVal IdUsuario As Integer, ByVal IdProyecto As Integer) As List(Of CU_ProductoInterno_Get_Result)
        Dim Data As New CU.ProductoInterno
        Dim Info As List(Of CU_ProductoInterno_Get_Result)
        Try
            Info = Data.ObtenerProductoInternoRecibe(IdUsuario, IdProyecto)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                'OJO aqui va el id del que recibe
                Session("ListaCargada") = ConsultarProductoInternoRecibe(Session("UsuarioId"), Session("ProyectoId"))
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("ProyectoId") = 7
            Session("UsuarioId") = "0"
            CargarCombo(ddlUnidadEnvia, "id", "Unidad", ObtenerUnidad())
            'CargarCombo(ddlEnvia, "id", "Nombres", ObtenerUsuarios)
            CargarCombo(ddlRecibe, "id", "Nombres", ObtenerUsuarios)
            CargarCombo(ddlTipoMovimiento, "id", "TipoMovimiento", ObtenerTipoMovimiento)
            CargarGrid(1)
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarGrid(1)
    End Sub

    Protected Sub gvDatos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Editar"
                    PIId = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    'Dim fe = CDate(gvDatos.DataKeys(CInt(e.CommandArgument))("FechaEnvio"))
                    'Dim fr = CDate(gvDatos.DataKeys(CInt(e.CommandArgument))("FechaRecepcion"))
                    'calFechaEnvio.Text = fe.Month & "/" & fe.Day & "/" & fe.Year
                    'calFechaRecepcion.Text = fr.Month & "/" & fr.Day & "/" & fr.Year
                    ddlTipoMovimiento.SelectedValue = gvDatos.DataKeys(CInt(e.CommandArgument))("Tipo")
                    txtNombre.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Producto")
                    Desc = gvDatos.DataKeys(CInt(e.CommandArgument))("Descripcion")
                    txtDescripcion.Text = Desc
                    Unidad = gvDatos.DataKeys(CInt(e.CommandArgument))("UnidadRecibe")
                    NUnidad = gvDatos.DataKeys(CInt(e.CommandArgument))("NombreUnidadRecibe")
                    ddlUnidadEnvia.SelectedValue = Unidad
                    Cant = gvDatos.DataKeys(CInt(e.CommandArgument))("Cantidad")
                    Dim htmltpl = "<style type='text/css'>" _
                    & ".form-row {overflow:hidden; font-size:11px; border-bottom:1.5px solid #eee; }" _
                    & "</style>" _
                    & "<style type='text/css'>" _
                    & ".form-titles {overflow:hidden; font-size:11px; width:120px;line-height:25px; }" _
                    & "</style>" _
                    & "<div style='margin: 10px 10px 10px 10px; width:98%; background-color:White;'>" _
                    & "<table style='width:98%;'>" _
                    & "<tr><td class='form-titles'><strong>Descripción:</strong></td>" _
                    & "<td class='form-row'>" & Desc & "</td></tr>" _
                    & "<tr><td class='form-titles'><strong>Unidad recibe:</strong></td>" _
                    & "<td class='form-row'>" & NUnidad & "</td></tr>" _
                    & "<tr><td class='form-titles'><strong>Cantidad:</strong></td>" _
                    & "<td class='form-row'>" & Cant & "</td></tr>" _
                    & "</table>" _
                    & "</div>"
                    Tpl.InnerHtml = htmltpl
                    'ddlEnvia.SelectedValue = gvDatos.DataKeys(CInt(e.CommandArgument))("Envia")
                    'ddlRecibe.SelectedValue = gvDatos.DataKeys(CInt(e.CommandArgument))("Recibe")
                    'txtObservaciones.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Observaciones")
                    datos.Visible = True
                    btnEditar.Visible = True
                    lista.Visible = False
                Case "Eliminar"
                    PIId = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Dim US As New CU.ProductoInterno
                    US.EliminarProductoInterno(PIId)
                    CargarGrid(1)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If Validar() = True Then
            Dim US As New CU.ProductoInterno
            If Cant < CDbl(txtCantidad.Text) Then
                lblResult.Text = "La cantidad no puede ser mayor de la que recibio"
            Else
                US.EditarCantProductoInterno(PIId, Cant - CDbl(txtCantidad.Text))
                US.GuardarProductoInterno(Session("ProyectoId"), DateTime.ParseExact(calFechaEnvio.Text, "M/d/yyyy", CultureInfo.InvariantCulture), ddlTipoMovimiento.SelectedValue, ddlUnidadEnvia.SelectedValue, ddlUnidadEnvia.SelectedValue, txtNombre.Text, txtDescripcion.Text, txtCantidad.Text, Session("UsuarioId"), ddlRecibe.SelectedValue(), DateTime.UtcNow.AddHours(-5), txtObservaciones.Text)
                lblResult.Text = "Producto Interno editado correctamente"
                Limpiar()
                CargarGrid(1)
                lista.Visible = True
                datos.Visible = False
            End If
        End If
    End Sub

    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        gvDatos.DataSource = Session("ListaCargada")
        gvDatos.DataBind()
    End Sub

End Class