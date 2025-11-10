Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports ClosedXML.Excel
Imports CoreProject.OP
Imports CoreProject.INV

Public Class Legalizaciones
    Inherits System.Web.UI.Page


#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarBU()
        End If
        CargarColumnas()
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)
        smanager.RegisterPostBackControl(Me.btnExportarSC)
    End Sub

    Private Sub gvStock_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvStock.RowDataBound

        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioRegistra As Int64?

        UsuarioRegistra = CType(Session("IDUsuario").ToString, Int64?)

        If e.Row.RowType = DataControlRowType.DataRow Then
            If CType(e.Row.DataItem, INV_StockxLegalizar_Get_Result).Estado = "Pendiente" Then
                DirectCast(e.Row.FindControl("imgBtnLegalizar"), ImageButton).Visible = True
            Else
                DirectCast(e.Row.FindControl("imgBtnLegalizar"), ImageButton).Visible = False
            End If

            If Permisos.VerificarPermisoUsuario(138, UsuarioRegistra) = True Then
                gvStock.Columns(15).Visible = False
            Else
                gvStock.Columns(15).Visible = True
            End If
        End If

    End Sub


    Private Sub gvLegalizaciones_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLegalizaciones.RowDataBound

        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioRegistra As Int64?

        UsuarioRegistra = CType(Session("IDUsuario").ToString, Int64?)

        If permisos.VerificarPermisoUsuario(138, UsuarioRegistra) = True Then
            gvLegalizaciones.Columns(18).Visible = False
            gvLegalizaciones.Columns(19).Visible = False
            gvLegalizaciones.Columns(20).Visible = False
            gvLegalizaciones.Columns(21).Visible = False
            gvLegalizaciones.Columns(22).Visible = False
            gvLegalizaciones.Columns(23).Visible = True
        Else
            gvLegalizaciones.Columns(18).Visible = True
            gvLegalizaciones.Columns(19).Visible = True
            gvLegalizaciones.Columns(20).Visible = True
            gvLegalizaciones.Columns(21).Visible = True
            gvLegalizaciones.Columns(22).Visible = True
            gvLegalizaciones.Columns(23).Visible = False
        End If

    End Sub

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub gvStock_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvStock.PageIndexChanging
        gvStock.PageIndex = e.NewPageIndex
        gvStock.DataSource = ObtenerLegalizacionesxPersona()
        gvStock.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub gvLegalizaciones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvLegalizaciones.PageIndexChanging
        gvLegalizaciones.PageIndex = e.NewPageIndex
        gvLegalizaciones.DataSource = CargargvLegalizaciones()
        gvLegalizaciones.DataBind()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub gvStock_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvStock.RowCommand
        Dim lstStockConsumibles As List(Of INV_StockConsumibles_Get_Result)

        If e.CommandName = "Legalizar" Then

            txtNombre.Text = Server.HtmlDecode(gvStock.Rows(e.CommandArgument).Cells(2).Text)
            hfIdUsuario.Value = gvStock.DataKeys(e.CommandArgument)("UsuarioAsignado")
            hfIdUsuarioReg.Value = gvStock.DataKeys(e.CommandArgument)("UsuarioRegistra")
            hfIdConsumible.Value = gvStock.DataKeys(e.CommandArgument)("IdConsumible")
            hfIdArticulo.Value = gvStock.DataKeys(e.CommandArgument)("IdArticulo")
            hfCentroCosto.Value = gvStock.DataKeys(e.CommandArgument)("IdCentroCosto")
            hfBU.Value = gvStock.DataKeys(e.CommandArgument)("IdBU")
            hfJobBook.Value = gvStock.DataKeys(e.CommandArgument)("JobBook")
            hfJobBookCodigo.Value = gvStock.Rows(e.CommandArgument).Cells(4).Text
            hfJobBookNombre.Value = Server.HtmlDecode(gvStock.Rows(e.CommandArgument).Cells(5).Text)
            lstStockConsumibles = ObtenerStockConsumibles(hfIdConsumible.Value, hfIdUsuario.Value)

            If hfIdArticulo.Value = 8 Then
                lbllegalizado.Visible = False
                txtLegalizado.Visible = False
                lblfirmas.Visible = True
                txtFirmas.Visible = True
                lblDevoluciones.Visible = True
                txtDevoluciones.Visible = True
                lblNotasCredito.Visible = True
                txtNotasCredito.Visible = True
                lblDescuentoNomina.Visible = True
                txtDescuentoNomina.Visible = True
            Else
                lbllegalizado.Visible = True
                txtLegalizado.Visible = True
                lblfirmas.Visible = False
                txtFirmas.Visible = False
                txtFirmas.Text = ""
                lblDevoluciones.Visible = False
                txtDevoluciones.Visible = False
                txtDevoluciones.Text = ""
                lblNotasCredito.Visible = False
                txtNotasCredito.Visible = False
                txtNotasCredito.Text = ""
                lblDescuentoNomina.Visible = False
                txtDescuentoNomina.Visible = False
                txtDescuentoNomina.Text = ""
            End If

            If hfIdArticulo.Value = 8 Then
                lblRadicado.Visible = True
                txtRadicado.Visible = True
            Else
                lblRadicado.Visible = False
                txtRadicado.Visible = False
                txtRadicado.Text = ""
            End If

            If hfIdArticulo.Value = 9 Then
                lblUnidades.Visible = True
                txtUnidades.Visible = True
                lblValorCarrera.Visible = True
                txtValorCarrera.Visible = True
                txtLegalizado.Enabled = False
                txtLegalizado.Text = 1
                lblVale.Visible = True
                lblIdVale.Visible = True
                If lstStockConsumibles(0).NumeroVale IsNot Nothing Then
                    lblIdVale.Text = lstStockConsumibles(0).NumeroVale
                End If
            Else
                lblUnidades.Visible = False
                txtUnidades.Visible = False
                txtUnidades.Text = ""
                lblValorCarrera.Visible = False
                txtValorCarrera.Visible = False
                txtValorCarrera.Text = ""
                txtLegalizado.Enabled = True
                txtLegalizado.Text = ""
                lblVale.Visible = False
                lblIdVale.Visible = False
                lblIdVale.Text = ""
            End If

            lblIdLegalizar.Text = hfIdConsumible.Value
            lblLegalizar.Visible = True
            lblIdLegalizar.Visible = True
            lblArticulo.Text = gvStock.Rows(e.CommandArgument).Cells(1).Text
            lblArticulo.Visible = True
            lblUsuario.Visible = True
            lblNombreUsuario.Text = gvStock.Rows(e.CommandArgument).Cells(2).Text
            lblNombreUsuario.Visible = True
            lblCentroCosto.Text = gvStock.Rows(e.CommandArgument).Cells(3).Text
            lblCentroCosto.Visible = True
            lblBUJBI.Visible = True
            If hfCentroCosto.Value = 3 Then
                lblBUJBI.Text = gvStock.DataKeys(e.CommandArgument)("IdBU") & "-" & gvStock.Rows(e.CommandArgument).Cells(6).Text
            Else
                lblBUJBI.Text = gvStock.Rows(e.CommandArgument).Cells(4).Text & "-" & gvStock.Rows(e.CommandArgument).Cells(5).Text
            End If

            lblPendiente.Visible = True
            lblIdPendiente.Visible = True
            lblIdPendiente.Text = gvStock.DataKeys(e.CommandArgument)("Pendiente")

            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)

        End If


        If e.CommandName = "Detalle" Then
            txtNombre.Text = Server.HtmlDecode(gvStock.Rows(e.CommandArgument).Cells(2).Text)
            hfIdUsuario.Value = gvStock.DataKeys(e.CommandArgument)("UsuarioAsignado")
            hfIdUsuarioReg.Value = gvStock.DataKeys(e.CommandArgument)("UsuarioRegistra")
            hfIdConsumible.Value = gvStock.DataKeys(e.CommandArgument)("IdConsumible")
            hfIdArticulo.Value = gvStock.DataKeys(e.CommandArgument)("IdArticulo")
            hfCentroCosto.Value = gvStock.DataKeys(e.CommandArgument)("IdCentroCosto")
            hfBU.Value = gvStock.DataKeys(e.CommandArgument)("IdBU")
            hfJobBook.Value = gvStock.DataKeys(e.CommandArgument)("JobBook")
            hfJobBookCodigo.Value = gvStock.Rows(e.CommandArgument).Cells(4).Text
            hfJobBookNombre.Value = Server.HtmlDecode(gvStock.Rows(e.CommandArgument).Cells(5).Text)
            lstStockConsumibles = ObtenerStockConsumibles(hfIdConsumible.Value, hfIdUsuario.Value)

            Dim BU As Int32? = Nothing
            Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
            Dim UsuarioRegistra As Int64?
            UsuarioRegistra = CType(Session("IDUsuario").ToString, Int64?)
            Dim JobBookCodigo As String = Nothing

            BU = If(hfBU.Value = "", CType(Nothing, Int32?), CType(hfBU.Value, Int32?))
            JobBookCodigo = If(hfJobBookCodigo.Value = "", CType(Nothing, String), CType(hfJobBookCodigo.Value, String))

            If hfIdArticulo.Value = 8 Then
                gvLegalizaciones.Columns(10).Visible = True  'Firmas
                gvLegalizaciones.Columns(11).Visible = True  'Devoluciones
                gvLegalizaciones.Columns(12).Visible = True  'NotasCredito
                gvLegalizaciones.Columns(13).Visible = True  'DescuentoNomina
            Else
                gvLegalizaciones.Columns(10).Visible = False  'Firmas
                gvLegalizaciones.Columns(11).Visible = False  'Devoluciones
                gvLegalizaciones.Columns(12).Visible = False  'NotasCredito
                gvLegalizaciones.Columns(13).Visible = False  'DescuentoNomina
            End If

            If hfIdArticulo.Value = 9 Then
                gvLegalizaciones.Columns(7).Visible = True  'Unidades
                gvLegalizaciones.Columns(8).Visible = True  'Valor Carrera
            Else
                gvLegalizaciones.Columns(7).Visible = False  'Unidades
                gvLegalizaciones.Columns(8).Visible = False  'Valor Carrera
            End If

            gvLegalizaciones.DataSource = CargargvLegalizaciones()
            gvLegalizaciones.DataBind()
            ActivateAccordion(2, EffectActivateAccordion.SlideEffect)

        End If

    End Sub

    Private Sub gvLegalizaciones_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLegalizaciones.RowCommand
        Dim lstLegalizaciones As List(Of INV_Legalizaciones_Get_Result)
        Dim lstLegalizacionesxUsuario As List(Of INV_Legalizaciones_Get_Result)
        Dim lstStockConsumibles As List(Of INV_StockConsumibles_Get_Result)

        Dim BU As Int32? = Nothing
        Dim JobBookCodigo As String = Nothing
        BU = If(hfBU.Value = "", CType(Nothing, Int32?), CType(hfBU.Value, Int32?))
        JobBookCodigo = If(hfJobBookCodigo.Value = "", CType(Nothing, String), CType(hfJobBookCodigo.Value, String))

        If e.CommandName = "Actualizar" Then

            lstLegalizaciones = ObtenerLegalizacionesxId(gvLegalizaciones.DataKeys(e.CommandArgument)("Id"))

            If lstLegalizaciones(0).Radicado IsNot Nothing Then
                txtRadicado.Text = lstLegalizaciones(0).Radicado
            End If

            txtFecha.Text = lstLegalizaciones(0).Fecha
            hfIdUsuario.Value = lstLegalizaciones(0).UsuarioResponsable
            txtNombre.Text = lstLegalizaciones(0).Usuario

            If lstLegalizaciones(0).Unidades IsNot Nothing Then
                txtUnidades.Text = lstLegalizaciones(0).Unidades
            End If

            If lstLegalizaciones(0).ValorCarrera IsNot Nothing Then
                txtValorCarrera.Text = lstLegalizaciones(0).ValorCarrera
            End If

            If lstLegalizaciones(0).Firmas IsNot Nothing Then
                txtFirmas.Text = lstLegalizaciones(0).Firmas
            End If

            If lstLegalizaciones(0).Devoluciones IsNot Nothing Then
                txtDevoluciones.Text = lstLegalizaciones(0).Devoluciones
            End If

            If lstLegalizaciones(0).NotasCredito IsNot Nothing Then
                txtNotasCredito.Text = lstLegalizaciones(0).NotasCredito
            End If

            If lstLegalizaciones(0).DescuentoNomina IsNot Nothing Then
                txtDescuentoNomina.Text = lstLegalizaciones(0).DescuentoNomina
            End If

            If lstLegalizaciones(0).Legalizado IsNot Nothing Then
                txtLegalizado.Text = lstLegalizaciones(0).ValorLegalizado
            End If

            If lstLegalizaciones(0).Observaciones IsNot Nothing Then
                txtObservaciones.Text = lstLegalizaciones(0).Observaciones
            End If

            lstStockConsumibles = ObtenerStockConsumibles(gvLegalizaciones.DataKeys(e.CommandArgument)("IdConsumible"), hfIdUsuario.Value)

            If lstLegalizaciones(0).IdArticulo = 8 Then
                lbllegalizado.Visible = False
                txtLegalizado.Visible = False
                lblfirmas.Visible = True
                txtFirmas.Visible = True
                lblDevoluciones.Visible = True
                txtDevoluciones.Visible = True
                lblNotasCredito.Visible = True
                txtNotasCredito.Visible = True
                lblDescuentoNomina.Visible = True
                txtDescuentoNomina.Visible = True
            Else
                lbllegalizado.Visible = True
                txtLegalizado.Visible = True
                lblfirmas.Visible = False
                txtFirmas.Visible = False
                txtFirmas.Text = ""
                lblDevoluciones.Visible = False
                txtDevoluciones.Visible = False
                txtDevoluciones.Text = ""
                lblNotasCredito.Visible = False
                txtNotasCredito.Visible = False
                txtNotasCredito.Text = ""
                lblDescuentoNomina.Visible = False
                txtDescuentoNomina.Visible = False
                txtDescuentoNomina.Text = ""
            End If

            If lstLegalizaciones(0).IdArticulo = 9 Then
                lblUnidades.Visible = True
                txtUnidades.Visible = True
                lblValorCarrera.Visible = True
                txtValorCarrera.Visible = True
                lblVale.Visible = True
                lblIdVale.Visible = True
                If lstStockConsumibles(0).NumeroVale IsNot Nothing Then
                    lblIdVale.Text = lstStockConsumibles(0).NumeroVale
                End If

            Else
                lblUnidades.Visible = False
                txtUnidades.Visible = False
                lblValorCarrera.Visible = False
                txtValorCarrera.Visible = False
                txtValorCarrera.Text = ""
                lblVale.Visible = False
                lblIdVale.Visible = False
                lblIdVale.Text = ""
            End If

            lblIdActualizar.Text = gvLegalizaciones.DataKeys(e.CommandArgument)("Id")
            lblActualizar.Visible = True
            lblIdActualizar.Visible = True
            lblIdLegalizar.Text = lstLegalizaciones(0).IdConsumible
            lblLegalizar.Visible = True
            lblIdLegalizar.Visible = True
            lblArticulo.Text = lstLegalizaciones(0).Articulo
            lblArticulo.Visible = True
            lblUsuario.Visible = True
            lblNombreUsuario.Text = lstLegalizaciones(0).Usuario
            lblNombreUsuario.Visible = True
            lblCentroCosto.Text = lstLegalizaciones(0).CentroCosto
            lblCentroCosto.Visible = True
            lblBUJBI.Visible = True
            If lstLegalizaciones(0).IdCentroCosto = 3 Then
                lblBUJBI.Text = lstLegalizaciones(0).IdBU & "-" & lstLegalizaciones(0).BU
            Else
                lblBUJBI.Text = lstLegalizaciones(0).JobBookCodigo & "-" & lstLegalizaciones(0).JobBookNombre
            End If

            lstLegalizacionesxUsuario = CargargvLegalizaciones()

            lblPendiente.Visible = True
            lblIdPendiente.Visible = True
            lblIdPendiente.Text = gvLegalizaciones.DataKeys(e.CommandArgument)("Pendiente") + gvLegalizaciones.Rows(e.CommandArgument).Cells(14).Text

            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If

        If e.CommandName = "Eliminar" Then
            Dim oEliminar As New Inventario
            oEliminar.EliminarLegalizacion(gvLegalizaciones.DataKeys(e.CommandArgument)("Id"))

            gvLegalizaciones.DataSource = CargargvLegalizaciones()
            gvLegalizaciones.DataBind()

            AlertJS("Registro Eliminado Correctamente")
            limpiar()
            ActivateAccordion(2, EffectActivateAccordion.NoEffect)
        End If

    End Sub

    Private Sub gvLegalizaciones_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvLegalizaciones.RowDeleting

    End Sub

#End Region


#Region "Grillas"

    Public Sub CargarColumnas()
        If ddlIdArticulo.SelectedValue = 8 Then
            gvStock.Columns(8).Visible = True  'TotalFirmas
            gvStock.Columns(9).Visible = True  'TotalDevoluciones
            gvStock.Columns(10).Visible = True  'TotalNotasCredito
            gvStock.Columns(11).Visible = True  'TotalDescuentoNomina
        Else
            gvStock.Columns(8).Visible = False  'TotalFirmas
            gvStock.Columns(9).Visible = False  'TotalDevoluciones
            gvStock.Columns(10).Visible = False  'TotalNotasCredito
            gvStock.Columns(11).Visible = False  'TotalDescuentoNomina
        End If
    End Sub

    Sub CargarGridPersonas()
        Dim o As New Personas
        Dim cedula As Int64? = Nothing
        Dim nombre As String = Nothing
        If IsNumeric(txtCedulaUsuario.Text) Then cedula = txtCedulaUsuario.Text
        If Not txtNombreUsuario.Text = "" Then nombre = txtNombreUsuario.Text
        Me.gvUsuarios.DataSource = o.ObtenerPersonasxCCNombre(cedula, nombre)
        Me.gvUsuarios.DataBind()
    End Sub

    Function ObtenerStockConsumibles(ByVal IdConsumible As Int64?, ByVal IdUsuario As Int64?) As List(Of INV_StockConsumibles_Get_Result)
        Dim lstStockConsumibles As New List(Of INV_StockConsumibles_Get_Result)
        Dim RecordStock As New Inventario
        lstStockConsumibles = RecordStock.ObtenerStockConsumiblesxIdusuario(IdConsumible, IdUsuario)
        Return lstStockConsumibles
    End Function

    Function ObtenerLegalizacionesxUsuarioAsignado(ByVal BU As Int32?, ByVal JobBookCodigo As String, ByVal Articulo As Int64?, ByVal UsuarioAsignado As Int64?, ByVal IdConsumible As Int64?) As List(Of INV_StockxLegalizar_Get_Result)
        Dim lstLegalizacionesxUsuarioAsignado As New List(Of INV_StockxLegalizar_Get_Result)
        Dim RecordLegalizaciones As New Inventario
        lstLegalizacionesxUsuarioAsignado = RecordLegalizaciones.ObtenerStockxLegalizarxUsuarioAsignado(BU, JobBookCodigo, Articulo, UsuarioAsignado, IdConsumible)
        Return lstLegalizacionesxUsuarioAsignado
    End Function

    Function ObtenerLegalizacionesxId(ByVal Id As Int64?) As List(Of INV_Legalizaciones_Get_Result)
        Dim lstLegalizaciones As New List(Of INV_Legalizaciones_Get_Result)
        Dim RecordLegalizaciones As New Inventario
        lstLegalizaciones = RecordLegalizaciones.ObtenerLegalizacionesxId(Id)
        Return lstLegalizaciones
    End Function

    Function CargargvLegalizaciones() As List(Of INV_Legalizaciones_Get_Result)
        Dim oCargar As New Inventario
        Dim BU As Int32? = Nothing
        Dim JobBookCodigo As String = Nothing
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioRegistra As Int64?
        UsuarioRegistra = CType(Session("IDUsuario").ToString, Int64?)
        BU = If(hfBU.Value = "", CType(Nothing, Int32?), CType(hfBU.Value, Int32?))
        JobBookCodigo = If(hfJobBookCodigo.Value = "", CType(Nothing, String), CType(hfJobBookCodigo.Value, String))

        If permisos.VerificarPermisoUsuario(138, UsuarioRegistra) = True Then

            Return oCargar.ObtenerLegalizaciones(hfIdConsumible.Value, BU, JobBookCodigo, hfIdArticulo.Value, hfIdUsuario.Value, Nothing)
        Else
            Return oCargar.ObtenerLegalizaciones(hfIdConsumible.Value, BU, JobBookCodigo, hfIdArticulo.Value, hfIdUsuarioReg.Value, hfIdUsuario.Value)
        End If

    End Function

    Function ObtenerLegalizacionesxPersona() As List(Of INV_StockxLegalizar_Get_Result)
        Dim oBusqueda As New Inventario
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios

        Dim BU As Int32? = Nothing
        Dim JobBookCodigo As String = Nothing
        Dim Articulo As Int64? = Nothing
        Dim TipoProducto As Int64? = Nothing
        Dim UsuarioRegistra As Int64?
        Dim UsuarioAsignado As Int64?
        Dim IdConsumible As Int64?

        If Not ddlBU.SelectedValue = -1 Then BU = ddlBU.SelectedValue
        If Not txtJBIJBE.Text = "" Then JobBookCodigo = txtJBIJBE.Text
        If Not ddlIdArticulo.SelectedValue = -1 Then Articulo = ddlIdArticulo.SelectedValue
        TipoProducto = If(ddlIdArticulo.SelectedValue = 7, CType(1, Int64?), CType(Nothing, Int64?))
        UsuarioRegistra = Nothing
        If Not txtIdUsuario.Text = "" Then UsuarioAsignado = txtIdUsuario.Text
        If Not txtIdConsumible.Text = "" Then IdConsumible = txtIdConsumible.Text

        Return oBusqueda.ObtenerStockxLegalizar(BU, JobBookCodigo, Articulo, TipoProducto, UsuarioRegistra, UsuarioAsignado, IdConsumible)
    End Function

#End Region

#Region "DDL"

    Sub CargarBU()
        Dim oBU As New Inventario
        Me.ddlBU.DataSource = oBU.obtenerBU
        Me.ddlBU.DataValueField = "Id"
        Me.ddlBU.DataTextField = "Nombre"
        Me.ddlBU.DataBind()
        Me.ddlBU.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub
#End Region

#Region "Formulario"

    Sub Guardar()

        If txtRadicado.Visible = True And String.IsNullOrEmpty(txtRadicado.Text) Then
            AlertJS("Digite el Número de Radicado")
            txtRadicado.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If

        If Not (IsDate(txtFecha.Text)) Then
            AlertJS("Escriba la fecha de la asignación")
            txtFecha.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If hfIdUsuario.Value = 0 Then
            AlertJS("Debe seleccionar el Usuario Responsable")
            txtNombre.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtUnidades.Visible = True And String.IsNullOrEmpty(txtUnidades.Text) Then
            AlertJS("Debe ingresar el número del vale del Taxi")
            txtUnidades.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtValorCarrera.Visible = True And String.IsNullOrEmpty(txtValorCarrera.Text) Then
            AlertJS("Debe ingresar el valor de la carrera del Taxi")
            txtValorCarrera.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtFirmas.Visible = True And String.IsNullOrEmpty(txtFirmas.Text) Then
            AlertJS("Debe ingresar un valor para las Firmas")
            txtFirmas.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtDevoluciones.Visible = True And String.IsNullOrEmpty(txtDevoluciones.Text) Then
            AlertJS("Debe ingresar un valor para las Devoluciones")
            txtDevoluciones.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtNotasCredito.Visible = True And String.IsNullOrEmpty(txtNotasCredito.Text) Then
            AlertJS("Debe ingresar un valor para las Notas de Crédito")
            txtNotasCredito.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtDescuentoNomina.Visible = True And String.IsNullOrEmpty(txtDescuentoNomina.Text) Then
            AlertJS("Debe ingresar un valor para los Descuentos de Nómina")
            txtDescuentoNomina.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtLegalizado.Visible = True And String.IsNullOrEmpty(txtLegalizado.Text) Then
            AlertJS("Debe ingresar el valor a legalizar")
            txtLegalizado.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        Dim oGuardar As New Inventario

        Dim IdConsumible As Int64? = lblIdLegalizar.Text

        Dim UsuarioRegistra As Int64?
        UsuarioRegistra = CType(Session("IDUsuario").ToString, Int64?)

        Dim Radicado As String = Nothing
        If Not txtRadicado.Text = "" Then Radicado = txtRadicado.Text

        Dim Unidades As Int32? = Nothing
        If Not txtUnidades.Text = "" Then Unidades = txtUnidades.Text

        Dim ValorCarrera As Int64? = Nothing
        If Not txtValorCarrera.Text = "" Then ValorCarrera = txtValorCarrera.Text

        Dim Firmas As Int64? = Nothing
        If Not txtFirmas.Text = "" Then Firmas = txtFirmas.Text

        Dim Devoluciones As Int64? = Nothing
        If Not txtDevoluciones.Text = "" Then Devoluciones = txtDevoluciones.Text

        Dim NotasCredito As Int64? = Nothing
        If Not txtNotasCredito.Text = "" Then NotasCredito = txtNotasCredito.Text

        Dim DescuentoNomina As Int64? = Nothing
        If Not txtDescuentoNomina.Text = "" Then DescuentoNomina = txtDescuentoNomina.Text

        Dim ValorLegalizado As Int64? = Nothing
        ValorLegalizado = Firmas + Devoluciones + NotasCredito + DescuentoNomina

        If hfIdArticulo.Value = 8 Then
            txtLegalizado.Text = ValorLegalizado
        Else
            txtLegalizado.Text = txtLegalizado.Text
        End If

        Dim Pendiente As Int64? = Nothing
        Dim Legalizado As Boolean? = Nothing
        Dim TipoLegalizacion As Int16? = Nothing
        Dim BU As Int32? = Nothing
        Dim JobBook As Int64? = Nothing
        Dim JobBookCodigo As String = Nothing
        Dim JobBookNombre As String = Nothing

        Pendiente = lblIdPendiente.Text - txtLegalizado.Text
        If Pendiente < 0 Then
            AlertJS("El Valor Legalizado no puede ser mayor al valor por legalizar!")
            lblIdPendiente.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        ElseIf Pendiente = 0 Then
            Legalizado = True
            TipoLegalizacion = 2
        Else
            Legalizado = False
            TipoLegalizacion = 1
        End If


        BU = If(hfBU.Value = "", CType(Nothing, Int32?), CType(hfBU.Value, Int32?))
        JobBook = If(hfJobBook.Value = "", CType(Nothing, Int64?), CType(hfJobBook.Value, Int64?))
        JobBookCodigo = If(hfJobBookCodigo.Value = "", CType(Nothing, String), CType(hfJobBookCodigo.Value, String))
        JobBookNombre = If(hfJobBookNombre.Value = "", CType(Nothing, String), CType(hfJobBookNombre.Value, String))

        If String.IsNullOrEmpty(lblIdActualizar.Text) Then
            oGuardar.GuardarLegalizaciones(IdConsumible, UsuarioRegistra, TipoLegalizacion, Radicado, txtFecha.Text, hfIdUsuario.Value, Unidades, Firmas, Devoluciones, NotasCredito, DescuentoNomina, txtLegalizado.Text, Pendiente, txtObservaciones.Text, Legalizado, hfCentroCosto.Value, BU, JobBook, JobBookCodigo, JobBookNombre, ValorCarrera, False, Nothing, Nothing)
        Else
            oGuardar.ActualizarLegalizaciones(lblIdActualizar.Text, IdConsumible, UsuarioRegistra, TipoLegalizacion, Radicado, txtFecha.Text, hfIdUsuario.Value, Unidades, Firmas, Devoluciones, NotasCredito, DescuentoNomina, txtLegalizado.Text, Pendiente, txtObservaciones.Text, Legalizado, hfCentroCosto.Value, BU, JobBook, JobBookCodigo, JobBookNombre, ValorCarrera, False, Nothing, Nothing)
        End If

        ShowNotification("La Legalización se ha hecho correctamente", ShowNotifications.InfoNotification)
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)

        limpiar()
        gvLegalizaciones.DataSource = Nothing
        gvLegalizaciones.DataBind()

    End Sub

    Sub chkVerificar_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checkbox As CheckBox = sender
        Dim row As GridViewRow = checkbox.NamingContainer
        Dim index As Integer = row.RowIndex
        Dim oUpdate As New Inventario
        If checkbox.Checked = False Then
            oUpdate.ActualizarLegalizaciones(gvLegalizaciones.DataKeys(index)("Id"), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, False, DateTime.UtcNow.AddHours(-5), Session("IDUsuario").ToString)
        Else
            oUpdate.ActualizarLegalizaciones(gvLegalizaciones.DataKeys(index)("Id"), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, True, DateTime.UtcNow.AddHours(-5), Session("IDUsuario").ToString)
        End If
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Sub limpiar()
        Me.txtRadicado.Text = ""
        Me.txtFecha.Text = ""
        Me.txtNombre.Text = ""
        hfIdUsuario.Value = "0"
        hfIdUsuarioReg.Value = "0"
        hfIdConsumible.Value = "0"
        hfIdArticulo.Value = "0"
        hfCentroCosto.Value = "0"
        hfBU.Value = "0"
        hfJobBook.Value = "0"
        hfJobBookCodigo.Value = "0"
        hfJobBookNombre.Value = "0"
        Me.txtUnidades.Text = ""
        Me.txtValorCarrera.Text = ""
        Me.txtFirmas.Text = ""
        Me.txtDevoluciones.Text = ""
        Me.txtNotasCredito.Text = ""
        Me.txtDescuentoNomina.Text = ""
        Me.txtLegalizado.Text = ""
        Me.txtObservaciones.Text = ""

        lblIdActualizar.Text = ""
        lblIdLegalizar.Text = ""
        lblArticulo.Text = ""
        lblNombreUsuario.Text = ""
        lblCentroCosto.Text = ""
        lblBUJBI.Text = ""
        lblIdVale.Text = ""
        lblIdPendiente.Text = ""
        lblActualizar.Visible = False
        lblIdActualizar.Visible = False
        lblIdLegalizar.Visible = False
        lblLegalizar.Visible = False
        lblArticulo.Visible = False
        lblUsuario.Visible = False
        lblNombreUsuario.Visible = False
        lblCentroCosto.Visible = False
        lblBUJBI.Visible = False
        lblVale.Visible = False
        lblIdVale.Visible = False
        lblPendiente.Visible = False
        lblIdPendiente.Visible = False

        ddlCentroCosto.ClearSelection()
        ddlBU.ClearSelection()
        txtJBIJBE.Text = ""
        txtNombreJBIJBE.Text = ""
        ddlIdArticulo.ClearSelection()
        txtIdUsuario.Text = ""
        hfIdUsuario.Value = "0"
        txtIdConsumible.Text = ""
        gvStock.DataSource = Nothing
        gvStock.DataBind()

    End Sub

#End Region

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Guardar()
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        limpiar()
        gvLegalizaciones.DataSource = Nothing
        gvLegalizaciones.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        gvStock.DataSource = ObtenerLegalizacionesxPersona()
        gvStock.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnBuscarUsuario_Click(sender As Object, e As EventArgs) Handles btnBuscarUsuario.Click
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        CargarGridPersonas()
        UPanelUsuarios.Update()
    End Sub

    Protected Sub ddlCentroCosto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCentroCosto.SelectedIndexChanged
        hfJobBook.Value = "0"
        If ddlCentroCosto.SelectedValue = 3 Then
            lblBU.Visible = True
            ddlBU.Visible = True
            lblJBIJBE.Visible = False
            txtJBIJBE.Visible = False
            txtJBIJBE.Text = ""
            lblNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Text = ""
        Else
            lblJBIJBE.Visible = True
            txtJBIJBE.Visible = True
            txtJBIJBE.Text = ""
            lblNombreJBIJBE.Visible = True
            txtNombreJBIJBE.Visible = True
            txtNombreJBIJBE.Text = ""
            lblBU.Visible = False
            ddlBU.Visible = False
            ddlBU.ClearSelection()
        End If
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub txtJBIJBE_TextChanged(sender As Object, e As EventArgs) Handles txtJBIJBE.TextChanged
        Dim daProyecto As New Proyecto
        Dim daTrabajo As New Trabajo
        Dim oP As PY_Proyectos_Get_Result
        Dim oT As PY_Trabajo0
        Select Case ddlCentroCosto.SelectedValue
            Case 1
                oP = daProyecto.obtenerXJobBook(txtJBIJBE.Text)
                If Not ((oP Is Nothing)) Then
                    txtNombreJBIJBE.Text = oP.Nombre
                    hfJobBook.Value = oP.id
                Else
                    txtNombreJBIJBE.Text = ""
                    hfJobBook.Value = ""
                End If

            Case 2
                oT = daTrabajo.ObtenerTrabajoXJob(txtJBIJBE.Text)
                If Not ((oT Is Nothing)) Then
                    txtNombreJBIJBE.Text = oT.NombreTrabajo
                    hfJobBook.Value = oT.id
                Else
                    txtNombreJBIJBE.Text = ""
                    hfJobBook.Value = ""
                End If
        End Select
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        exportarExcel()
    End Sub

    Protected Sub btnExportarSC_Click(sender As Object, e As EventArgs) Handles btnExportarSC.Click
        exportarExcelSC()
    End Sub

#Region "Exportar a Excel Registro Legalizaciones"
    Sub exportarExcel()
        Dim wb As New XLWorkbook
        Dim oLegalizaciones As List(Of INV_StockxLegalizar_Get_Result)
        Dim titulosRegistro As String = "IdConsumible;Articulo;UsuarioAsignado;CentroCosto;JobBookCodigo;JobBookNombre;BU;TotalEntregado;TotalFirmas;TotalDevoluciones;TotalNotasCredito;TotalDescuentoNomina;TotalLegalizado;ValorPendiente;Estado"
        oLegalizaciones = ObtenerLegalizacionesxPersona()
        Dim oExportar = (From x In oLegalizaciones
                        Select x.IdConsumible, x.Articulo, x.Usuario, x.CentroCosto, x.JobBookCodigo, x.JobBookNombre, x.BU, x.TotalEntregado, x.TotalFirmas, x.TotalDevoluciones, x.TotalNotasCredito, x.TotalDescuentoNomina, x.TotalLegalizado, x.Pendiente, x.Estado).ToList

        Dim ws = wb.Worksheets.Add("Legalizaciones")
        insertarNombreColumnasExcel(ws, titulosRegistro.Split(";"))
        ws.Cell(2, 1).InsertData(oExportar)
        exportarExcel(wb, "Legalizaciones")
    End Sub

    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(1, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub

    Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Listado_" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

#End Region

#Region "Exportar a Excel Listado Stock"
    Sub exportarExcelSC()
        Dim wb As New XLWorkbook
        Dim oLegalizaciones As List(Of INV_Legalizaciones_Get_Result)
        Dim titulosStock As String = "Id;IdConsumible;Articulo;TipoLegalizacion;Fecha;UsuarioAsignado;CentroCosto;JobBookCodigo;JobBookNombre;BU;Unidades;ValorEntregado;Firmas;Devoluciones;NotasCredito;DescuentoNomina;ValorLegalizado;ValorPendiente;Observaciones;Legalizado"
        oLegalizaciones = CargargvLegalizaciones()
        Dim oExportar = (From x In oLegalizaciones
                        Select x.Id, x.IdConsumible, x.Articulo, x.TipoLegalizacion, x.Fecha, x.Usuario, x.CentroCosto, x.JobBookCodigo, x.JobBookNombre, x.BU, x.Unidades, x.ValorEntregado, x.Firmas, x.Devoluciones, x.NotasCredito, x.DescuentoNomina, x.ValorLegalizado, x.Pendiente,
                        x.Observaciones, x.Legalizado).ToList

        Dim ws = wb.Worksheets.Add("Legalizaciones")
        insertarNombreColumnasExcelSC(ws, titulosStock.Split(";"))
        ws.Cell(2, 1).InsertData(oExportar)
        exportarExcelSC(wb, "Legalizaciones")
    End Sub

    Sub insertarNombreColumnasExcelSC(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(1, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub

    Private Sub exportarExcelSC(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Detalle_" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

#End Region

    Private Sub _AlmacenamientoDisco_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(138, UsuarioID) = False AndAlso permisos.VerificarPermisoUsuario(140, UsuarioID) = False Then
            Response.Redirect("../Inventario/RegistroArticulos.aspx")
        End If
    End Sub

    Protected Sub txtDescuentoNomina_TextChanged(sender As Object, e As EventArgs) Handles txtDescuentoNomina.TextChanged
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
End Class