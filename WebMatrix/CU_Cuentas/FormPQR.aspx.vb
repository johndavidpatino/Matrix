Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class FormPQR
    Inherits System.Web.UI.Page
#Region " Eventos del Control"
    Protected Sub ddlfuncionariodesignado_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlfuncionariodesignado.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddlrecibida_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlrecibida.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddlfuncionariocierre_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlfuncionariocierre.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarPQR()
            CargarResponsable()
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(12, UsuarioID) = False Then
                btnNuevo.Visible = False
                gvDatos.Columns.Item(7).Visible = False
                gvDatos.Columns.Item(8).Visible = False
                btnGuardar.Visible = False
                btnCerrarPQR.Visible = False
            End If


        End If
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            Guardar()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            log(11, hfIdPQR.Value, 2)
            Limpiar()
            CargarPQR()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            CargarPQR()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        Limpiar()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        txtFecha.Text = Date.UtcNow.AddHours(-5).Date
        ddlfuncionariodesignado.SelectedValue = Session("IDUsuario").ToString
        ddlrecibida.SelectedValue = Session("IDUsuario").ToString
        txtEstablecidaPor.Focus()
    End Sub
    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarPQR()
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idPQR As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Limpiar()
                    Cargarinfo(idPQR)
                Case "Eliminar"
                    Dim idPQR As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Eliminar(idPQR)
                    CargarPQR()
                    ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)

                Case "Detalles"
                    Dim idPQR As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Limpiar()
                    Detalles(idPQR)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub btnCerrarPQR_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCerrarPQR.Click
        Try
            Cerrar()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            log(15, hfIdPQR.Value, 2)
            Limpiar()
            CargarPQR()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(2, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
#End Region
#Region "Funciones y Metodos"
    Public Sub Limpiar()
        hfIdPQR.Value = String.Empty
        txtFecha.Text = String.Empty
        txtEstablecidaPor.Text = String.Empty
        TxtEmpresa.Text = String.Empty
        ddlfuncionariodesignado.SelectedValue = "-1"
        ddlrecibida.SelectedValue = "-1"
        txtDescripcion.Text = String.Empty
        txtAccionInmediata.Text = String.Empty
        txtSituacion.Text = String.Empty
        txtAccion.Text = String.Empty
        txtFechaCierre.Text = String.Empty
        ddlfuncionariocierre.SelectedValue = "-1"
        txtRespuestaPQR.Text = String.Empty
    End Sub
    Public Sub CargarPQR()
        Try
            Dim oPQR As New Pqr
            Dim listaPQR = (From lpqr In oPQR.DevolverTodos()
                             Select id = lpqr.id, Fecha = lpqr.Fecha,
                             EstablecidaPor = lpqr.EstablecidaPor, Empresa = lpqr.Empresa, Descripcion = lpqr.Descripcion,
                             AccionInmediata = lpqr.AccionInmediata, Situacion = lpqr.Situacion, Accion = lpqr.Accion, FechaCierre = lpqr.FechaCierre,
                             RespuestaPQR = lpqr.RespuestaPQR, FuncionarioCierre = lpqr.FuncionarioCierre, Cerrada = lpqr.Cerrada, Recibe = lpqr.Recibe, Designado = lpqr.Designado, Cierra = lpqr.Cierra,
                             FuncionarioRecibe = lpqr.FuncionarioRecibe, FuncionarioDesignado = lpqr.FuncionarioDesignado
                             Where FuncionarioRecibe = Session("IDUsuario").ToString OrElse FuncionarioDesignado = Session("IDUsuario").ToString Or FuncionarioCierre = Session("IDUsuario").ToString).OrderBy(Function(p) p.id)
            If (Not String.IsNullOrEmpty(txtBuscar.Text)) Then
                    gvDatos.DataSource = listaPQR.Where(Function(c) (c.EstablecidaPor.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Empresa.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Descripcion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.AccionInmediata.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Situacion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Accion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.RespuestaPQR.ToUpper.Contains(txtBuscar.Text.ToUpper))).ToList
            Else
                gvDatos.DataSource = listaPQR.ToList
            End If
            gvDatos.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Guardar()
        Try
            Dim IdPQR, FuncionarioQRecibe, FuncionarioDesignado As Int64?
            Dim Fecha As DateTime?
            Dim EstablecidaPor = "", Empresa = "", Descripcion = "", AccionInmediata = "", Situacion = "", Accion As String = ""
            Dim oPQR As New Pqr

            If Not String.IsNullOrEmpty(hfIdPQR.Value) Then
                IdPQR = Int64.Parse(hfIdPQR.Value)
            End If

            If Not String.IsNullOrEmpty(txtFecha.Text) Then
                Fecha = txtFecha.Text
            End If

            If Not String.IsNullOrEmpty(txtEstablecidaPor.Text) Then
                EstablecidaPor = txtEstablecidaPor.Text
            End If

            If Not String.IsNullOrEmpty(TxtEmpresa.Text) Then
                Empresa = TxtEmpresa.Text
            End If

            If Not ddlfuncionariodesignado.SelectedValue = "-1" Then
                FuncionarioDesignado = Int64.Parse(ddlfuncionariodesignado.SelectedValue)
            End If

            If Not ddlrecibida.SelectedValue = "-1" Then
                FuncionarioQRecibe = Int64.Parse(ddlrecibida.SelectedValue)
            End If

            If Not String.IsNullOrEmpty(txtDescripcion.Text) Then
                Descripcion = txtDescripcion.Text
            End If

            If Not String.IsNullOrEmpty(txtAccionInmediata.Text) Then
                AccionInmediata = txtAccionInmediata.Text
            End If

            If Not String.IsNullOrEmpty(txtSituacion.Text) Then
                Situacion = txtSituacion.Text
            End If

            If Not String.IsNullOrEmpty(txtAccion.Text) Then
                Accion = txtAccion.Text
            End If


            oPQR.Guardar(IdPQR, Fecha, EstablecidaPor, Empresa, Descripcion, AccionInmediata, FuncionarioQRecibe, FuncionarioDesignado, Situacion, Accion)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Cerrar()
        Try
            Dim IdPQR, FuncionarioCierre As Int64?
            Dim FechaCierre As DateTime?
            Dim RespuestaPQR As String = ""
            Dim oPQR As New Pqr

            If Not String.IsNullOrEmpty(hfIdPQR.Value) Then
                IdPQR = Int64.Parse(hfIdPQR.Value)
            Else
                Throw New Exception("Debe elegir un PQR para cerrar")
            End If

            If Not String.IsNullOrEmpty(txtFechaCierre.Text) Then
                FechaCierre = txtFechaCierre.Text
            End If

            If Not ddlfuncionariocierre.SelectedValue = "-1" Then
                FuncionarioCierre = ddlfuncionariocierre.SelectedValue
            End If
            If Not String.IsNullOrEmpty(txtRespuestaPQR.Text) Then
                RespuestaPQR = txtRespuestaPQR.Text
            End If

            oPQR.Cerrar(IdPQR, FechaCierre, FuncionarioCierre, RespuestaPQR)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Cargarinfo(ByVal idPQR As Int64)
        Try
            Dim oPQR As New Pqr
            Dim info = oPQR.DevolverxID(idPQR)
            hfIdPQR.Value = idPQR

            If info.Fecha IsNot Nothing Then
                txtFecha.Text = info.Fecha
            End If

            If Not String.IsNullOrEmpty(info.EstablecidaPor) And info.EstablecidaPor IsNot Nothing Then
                txtEstablecidaPor.Text = info.EstablecidaPor
            End If

            If Not String.IsNullOrEmpty(info.Empresa) And info.Empresa IsNot Nothing Then
                TxtEmpresa.Text = info.Empresa
            End If

            If info.FuncionarioDesignado IsNot Nothing Then
                ddlfuncionariodesignado.SelectedValue = info.FuncionarioDesignado
            End If

            If info.FuncionarioRecibe IsNot Nothing Then
                ddlrecibida.SelectedValue = info.FuncionarioRecibe
            Else
                ddlrecibida.SelectedValue = Session("IDUsuario").ToString
            End If

            If Not String.IsNullOrEmpty(info.Descripcion) And info.Descripcion IsNot Nothing Then
                txtDescripcion.Text = info.Descripcion
            End If

            If Not String.IsNullOrEmpty(info.AccionInmediata) And info.AccionInmediata IsNot Nothing Then
                txtAccionInmediata.Text = info.AccionInmediata
            End If

            If Not String.IsNullOrEmpty(info.Situacion) And info.Situacion IsNot Nothing Then
                txtSituacion.Text = info.Situacion
            End If

            If Not String.IsNullOrEmpty(info.Accion) And info.Accion IsNot Nothing Then
                txtAccion.Text = info.Accion
            End If
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Eliminar(ByVal idPQR As Int64)
        Try
            Dim oPqr As New Pqr
            oPqr.Eliminar(idPQR)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Detalles(ByVal idPQR As Int64)
        Try
            Dim oPQR As New Pqr
            Dim info = oPQR.DevolverxID(idPQR)
            hfIdPQR.Value = idPQR

            If info.FechaCierre IsNot Nothing Then
                txtFechaCierre.Text = info.FechaCierre
            Else
                txtFechaCierre.Text = Date.UtcNow.AddHours(-5).Date
            End If

            If info.FuncionarioCierre IsNot Nothing Then
                ddlfuncionariocierre.SelectedValue = info.FuncionarioCierre
            Else
                ddlfuncionariocierre.SelectedValue = Session("IDUsuario").ToString
            End If

            If Not String.IsNullOrEmpty(info.RespuestaPQR) And info.RespuestaPQR IsNot Nothing Then
                txtRespuestaPQR.Text = info.RespuestaPQR
            End If

            ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarResponsable()
        Try
            Dim oPersona As New Personas
            Dim listapersonas = (From lpersona In oPersona.TH_Usuarios_Combo_Get()
                              Select Id = lpersona.id,
                              Nombre = lpersona.Nombres).OrderBy(Function(p) p.Nombre)

            ddlfuncionariodesignado.DataSource = listapersonas.ToList()
            ddlfuncionariodesignado.DataValueField = "Id"
            ddlfuncionariodesignado.DataTextField = "Nombre"
            ddlfuncionariodesignado.DataBind()

            ddlrecibida.DataSource = listapersonas.ToList()
            ddlrecibida.DataValueField = "Id"
            ddlrecibida.DataTextField = "Nombre"
            ddlrecibida.DataBind()

            ddlfuncionariocierre.DataSource = listapersonas.ToList()
            ddlfuncionariocierre.DataValueField = "Id"
            ddlfuncionariocierre.DataTextField = "Nombre"
            ddlfuncionariocierre.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub log(ByVal idfrom As Int16, ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Dim log As New LogEjecucion
        log.Guardar(idfrom, iddoc, Now(), Session("IDUsuario"), idaccion)
    End Sub
#End Region






End Class