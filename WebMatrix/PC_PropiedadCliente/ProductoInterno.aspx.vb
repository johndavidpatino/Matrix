Imports CoreProject
Imports System.Globalization

Public Class ProductoInterno
    Inherits System.Web.UI.Page

    Private _id As String

    Public Property PIId As Integer
        Get
            Return ViewState("_id")
        End Get
        Set(value As Integer)
            ViewState("_id") = value
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

    Public Function ObtenerUnidad() As List(Of UnidadCombo_Result)
        Dim Data As New SG.Actas
        Try
            Return Data.ObtenerUnidad
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

    Public Function ConsultarProductoInterno(ByVal Nombre As String) As List(Of CU_ProductoInterno_Get_Result)
        Dim Data As New CU.ProductoInterno
        Dim Info As List(Of CU_ProductoInterno_Get_Result)
        Try
            Info = Data.ObtenerProductoInterno()
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Session("ListaCargada") = ConsultarProductoInterno(txtBuscar.Text)
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("ProyectoId") = 7 'quemado por default
            CargarCombo(ddlUnidadEnvia, "id", "Unidad", ObtenerUnidad())
            CargarCombo(ddlUnidadRecibe, "id", "Unidad", ObtenerUnidad())
            CargarCombo(ddlRecibe, "id", "Nombres", ObtenerUsuarios)
            CargarCombo(ddlEnvia, "id", "Nombres", ObtenerUsuarios)
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
                    calFechaRecepcion.Text = fr.Month & "/" & fr.Day & "/" & fr.Year
                    ddlTipoMovimiento.SelectedValue = gvDatos.DataKeys(CInt(e.CommandArgument))("Tipo")
                    txtNombre.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Producto")
                    txtDescripcion.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Descripcion")
                    ddlUnidadEnvia.SelectedValue = gvDatos.DataKeys(CInt(e.CommandArgument))("UnidadEnvia")
                    ddlUnidadRecibe.SelectedValue = gvDatos.DataKeys(CInt(e.CommandArgument))("UnidadRecibe")
                    txtCantidad.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Cantidad")
                    'ddlEnvia.SelectedValue = gvDatos.DataKeys(CInt(e.CommandArgument))("Envia")
                    ddlRecibe.SelectedValue = gvDatos.DataKeys(CInt(e.CommandArgument))("Recibe")
                    txtObservaciones.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Observaciones")
                    datos.Visible = True
                    btnEditar.Visible = True
                    btnGuardar.Visible = False
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

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        datos.Visible = True
        lista.Visible = False
        btnEditar.Visible = False
        btnGuardar.Visible = True
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Validar() = True Then
            Dim US As New CU.ProductoInterno
            US.GuardarProductoInterno(Session("ProyectoId"), DateTime.ParseExact(calFechaEnvio.Text, "M/d/yyyy", CultureInfo.InvariantCulture), ddlUnidadEnvia.SelectedValue, ddlUnidadRecibe.SelectedValue, ddlTipoMovimiento.SelectedValue, txtNombre.Text, txtDescripcion.Text, txtCantidad.Text, ddlEnvia.SelectedValue, ddlRecibe.SelectedValue, DateTime.ParseExact(calFechaRecepcion.Text, "M/d/yyyy", CultureInfo.InvariantCulture), txtObservaciones.Text)
            lblResult.Text = "Producto Interno agregado correctamente"
            Limpiar()
            CargarGrid(1)
            lista.Visible = True
            datos.Visible = False
        End If
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If Validar() = True Then
            Dim US As New CU.ProductoInterno
            US.EditarProductoInterno(PIId, Session("ProyectoId"), DateTime.ParseExact(calFechaEnvio.Text, "M/d/yyyy", CultureInfo.InvariantCulture), ddlUnidadEnvia.SelectedValue, ddlUnidadRecibe.SelectedValue, ddlTipoMovimiento.SelectedValue, txtNombre.Text, txtDescripcion.Text, txtCantidad.Text, ddlEnvia.SelectedValue, ddlRecibe.SelectedValue, DateTime.ParseExact(calFechaRecepcion.Text, "M/d/yyyy", CultureInfo.InvariantCulture), txtObservaciones.Text)
            lblResult.Text = "Producto Interno editado correctamente"
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