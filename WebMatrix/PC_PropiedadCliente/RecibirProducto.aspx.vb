Imports System.Globalization
Imports CoreProject

Public Class RecibirProducto
    Inherits System.Web.UI.Page

    Private _id As Integer
    Private _desc As String
    Private _uni As String
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

    Public Property Cant As Integer
        Get
            Return ViewState("_cant")
        End Get
        Set(value As Integer)
            ViewState("_cant") = value
        End Set
    End Property

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

    Public Function ConsultarProductoInternoEnvia(ByVal IdUsuario As Integer, ByVal IdProyecto As Integer) As List(Of CU_ProductoInterno_Get_Result)
        Dim Data As New CU.ProductoInterno
        Dim Info As List(Of CU_ProductoInterno_Get_Result)
        Try
            Info = Data.ObtenerProductoInternoEnvia(IdUsuario, IdProyecto)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                'OJO aqui va el id del que recibe
                Session("ListaCargada") = ConsultarProductoInternoEnvia(Session("UsuarioId"), Session("ProyectoId"))
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Public Function ObtenerUnidad() As List(Of UnidadCombo_Result)
        Dim Data As New SG.Actas
        Try
            Return Data.ObtenerUnidad
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("UsuarioId") = 0
            Session("ProyectoId") = 7
            CargarCombo(ddlUnidadEnvia, "id", "Unidad", ObtenerUnidad())
            CargarCombo(ddlEnvia, "id", "Nombres", ObtenerUsuarios)
            'CargarCombo(ddlRecibe, "id", "Nombres", ObtenerUsuarios)
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
                    Dim fe = CDate(gvDatos.DataKeys(CInt(e.CommandArgument))("FechaEnvio"))
                    Dim fr = CDate(gvDatos.DataKeys(CInt(e.CommandArgument))("FechaRecepcion"))
                    calFechaEnvio.Text = fe.Month & "/" & fe.Day & "/" & fe.Year
                    ddlTipoMovimiento.SelectedValue = gvDatos.DataKeys(CInt(e.CommandArgument))("Tipo")
                    txtNombre.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Producto")
                    Desc = gvDatos.DataKeys(CInt(e.CommandArgument))("Descripcion")
                    txtDescripcion.Text = Desc
                    Unidad = gvDatos.DataKeys(CInt(e.CommandArgument))("UnidadRecibe")
                    'ddlUnidadEnvia.SelectedValue = gvDatos.DataKeys(CInt(e.CommandArgument))("UnidadEnvia")
                    Cant = gvDatos.DataKeys(CInt(e.CommandArgument))("Cantidad")
                    txtCantidad.Text = Cant
                    ddlEnvia.SelectedValue = gvDatos.DataKeys(CInt(e.CommandArgument))("Envia")

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
            US.EditarProductoInterno(PIId, Session("ProyectoId"), DateTime.ParseExact(calFechaEnvio.Text, "M/d/yyyy", CultureInfo.InvariantCulture), ddlUnidadEnvia.SelectedValue, ddlUnidadEnvia.SelectedValue, ddlTipoMovimiento.SelectedValue, txtNombre.Text, txtDescripcion.Text, txtCantidad.Text, ddlEnvia.SelectedValue, Session("UsuarioId"), DateTime.UtcNow.AddHours(-5), txtObservaciones.Text)
            lblResult.Text = "Producto Interno recibido correctamente"
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