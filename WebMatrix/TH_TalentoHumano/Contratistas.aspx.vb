Imports CoreProject
Imports WebMatrix.Util
Public Class Contratistas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(131, UsuarioID) = False Then
            Response.Redirect("../FI_AdministrativoFinanciero/Default.aspx")
        End If
        If Not IsPostBack Then
            ObtenerContratistas(Nothing, Nothing)
            CargarCiudades()
            CargarEstados()
            CargarServicios()
            CargarClasificacion()
        End If
    End Sub

    Private Sub gvContratistas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvContratistas.PageIndexChanging
        gvContratistas.PageIndex = e.NewPageIndex
        ObtenerContratistas(Nothing, Nothing)
    End Sub
    Private Sub gvContratistas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvContratistas.RowCommand
        If e.CommandName = "Actualizar" Then
            Limpiar()
            hfID.Value = Int64.Parse(Me.gvContratistas.DataKeys(CInt(e.CommandArgument))("Identificacion"))
            ObtenerInformacionContratista(hfID.Value)
            ObtenerServicios(hfID.Value)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            hfNuevo.Value = 1
        End If

    End Sub
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim op As New CoreProject.Contratista
        Dim Va As Boolean
        If hfNuevo.Value = 1 Then
            If ddlestado.SelectedValue = -1 Then
                ShowNotification("Seleccione Estado del Contratista", ShowNotifications.InfoNotification)
                ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                Exit Sub
            End If
            If txtsymphony.Text = "" Then
                ShowNotification("Ingrese Numero de Symphony", ShowNotifications.InfoNotification)
                ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                Exit Sub
            End If
            If ddlCiudad.SelectedValue = -1 Then
                ShowNotification("Seleccione Ciudad de Contratista", ShowNotifications.InfoNotification)
                ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                Exit Sub
            End If
            If ddlClasificacion.SelectedValue = -1 Then
                ShowNotification("Seleccione la Clasificación del Contratista", ShowNotifications.InfoNotification)
                ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                Exit Sub
            End If
            If txtFechaingreso.Text = "" Then
                ShowNotification("Ingrese Fecha", ShowNotifications.InfoNotification)
                ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                Exit Sub
            End If

            Actualizar()
            hfID.Value = ""
            ShowNotification("Informacion Actualizada", ShowNotifications.InfoNotification)
            ObtenerContratistas(Nothing, Nothing)
        End If
        Va = op.ExisteContratista(txtIdentificacion.Text)
        If Va = True Then
            ShowNotification("Ya Existe Un Contratista con esa Identificacion", ShowNotifications.InfoNotification)
            Limpiar()
            Exit Sub
        ElseIf GvServicios.Rows.Count > 0 Then
            Guardar()
        Else
            ShowNotification("Ingrese al menos un servicio para el contratista", ShowNotifications.InfoNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            Exit Sub
        End If
        ObtenerContratistas(Nothing, Nothing)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
        Limpiar()
        hfNuevo.Value = 0
    End Sub
    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Limpiar()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If txtIdentificacionBuscar.Text = "" And txtNombreBuscar.Text = "" Then
            ObtenerContratistas(Nothing, Nothing)
        ElseIf txtIdentificacionBuscar.Text = "" Then
            ObtenerContratistas(Nothing, txtNombreBuscar.Text)
            txtNombreBuscar.Text = String.Empty
        ElseIf txtNombreBuscar.Text = "" Then
            ObtenerContratistas(txtIdentificacionBuscar.Text, Nothing)
            txtIdentificacionBuscar.Text = String.Empty
        End If
    End Sub

    Sub ObtenerContratistas(ByVal Identificacion As Int64?, ByVal Nombre As String)
        Dim op As New CoreProject.Contratista
        gvContratistas.DataSource = op.ObtenerContratistas(Identificacion, Nombre, Nothing)
        gvContratistas.DataBind()
    End Sub

    Sub ObtenerServicios(ByVal Identificacion As Int64?)
        Dim op As New CoreProject.Contratista
        GvServicios.DataSource = op.ObtenerServiciosContratista(Identificacion)
        GvServicios.DataBind()
    End Sub

    Sub ObtenerInformacionContratista(ByVal Identificacion As Int64)
        Dim e As New TH_Contratistas
        Dim op As New CoreProject.Contratista

        e = op.ObtenerContratista(Identificacion)
        txtIdentificacion.Text = e.Identificacion
        txtNombre.Text = e.Nombre
        txtdireccion.Text = e.Direccion
        txtcorreo.Text = e.Email
        If IsNumeric(e.CiudadId) Then ddlCiudad.SelectedValue = e.CiudadId
        If IsNumeric(e.NumeroSymphony) Then txtsymphony.Text = e.NumeroSymphony
        txtdesripcion.Text = e.DescripcionCuenta
        txttelefono.Text = e.Telefono
        'If IsNumeric(e.Estado) Then ddlestado.SelectedValue = e.Estado 
        If e.Activo = True Then
            ddlestado.SelectedValue = 1
        ElseIf e.Activo = False And e.Estado = 3 Then
            ddlestado.SelectedValue = 3
        ElseIf e.Activo = False And e.Estado = 2 Then
            ddlestado.SelectedValue = 2
        End If
        txtsolicitud.Text = e.Solicitud
        txtAprobado.Text = e.Aprobado
        TxtObservacion.Text = e.Observaciones
        ddlClasificacion.SelectedValue = e.Clasificacion
        If IsDate(e.FechaRegistro) Then txtFechaingreso.Text = e.FechaRegistro
    End Sub

    Sub Guardar()
        Dim op As New CoreProject.Contratista
        op.Guardar(txtIdentificacion.Text, txtNombre.Text, txtdireccion.Text, txtcorreo.Text, 1, ddlCiudad.SelectedValue, txtsymphony.Text, 0, txtdesripcion.Text, txttelefono.Text, txtFechaingreso.Text, ddlestado.SelectedValue, txtsolicitud.Text, txtAprobado.Text, TxtObservacion.Text, ddlClasificacion.SelectedValue)
        If GvServicios.Rows.Count > 0 Then
            For i = 0 To GvServicios.Rows.Count - 1
                op.GuardarServiciosContratista(txtIdentificacion.Text, GvServicios.Rows(i).Cells(2).Text, GvServicios.Rows(i).Cells(3).Text, True)
            Next
        End If
    End Sub
    Sub Actualizar()
        Dim Contratista As New TH_Contratistas
        Dim op As New CoreProject.Contratista
        Dim Servicios As New List(Of TH_ContratistasDetalleServiciosGet_Result)
        Dim val As Boolean = False
        Servicios = op.ObtenerServiciosContratista(txtIdentificacion.Text)
        Contratista = op.ObtenerContratista(txtIdentificacion.Text)

        If ddlestado.SelectedValue = 1 Then
            op.Actualizar(txtIdentificacion.Text, txtNombre.Text, txtdireccion.Text, txtcorreo.Text, 1, ddlCiudad.SelectedValue, txtsymphony.Text, 0, txtdesripcion.Text, txttelefono.Text, txtFechaingreso.Text, ddlestado.SelectedValue, txtsolicitud.Text, txtAprobado.Text, TxtObservacion.Text, ddlClasificacion.SelectedValue)
            If Contratista.Nombre <> txtNombre.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Nombre Actualizado-" & Contratista.Nombre & "-" & txtNombre.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Clasificacion <> ddlClasificacion.SelectedValue Then
                op.LogContratistasAdd(ddlClasificacion.SelectedValue, "Clasificación Actualizada-" & Contratista.Clasificacion & "-" & ddlClasificacion.SelectedValue, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Direccion <> txtdireccion.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Direccion Actualizado-" & Contratista.Direccion & "-" & txtdireccion.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Email <> txtcorreo.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Correo Actualizado-" & Contratista.Email & "-" & txtcorreo.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.NumeroSymphony <> txtsymphony.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Sypmphony Actualizado-" & Contratista.NumeroSymphony & "-" & txtsymphony.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.DescripcionCuenta <> txtdesripcion.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Descripcion Actualizado-" & Contratista.DescripcionCuenta & "-" & txtdesripcion.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Estado = 2 Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Estado Actualizado-Activado", Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Estado = 3 Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Estado Actualizado-Activado", Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Telefono <> txttelefono.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Telefono Actualizado-" & Contratista.Telefono & "-" & txttelefono.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Solicitud <> txtsolicitud.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Solicitud Actualizado-" & Contratista.Solicitud & "-" & txtsolicitud.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Aprobado <> txtAprobado.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Aprobo Actualizado-" & Contratista.Aprobado & "-" & txtAprobado.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Observaciones <> TxtObservacion.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Observaciones Actualizado-" & Contratista.Observaciones & "-" & TxtObservacion.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If

        ElseIf ddlestado.SelectedValue <> 1 Then
            op.Actualizar(txtIdentificacion.Text, txtNombre.Text, txtdireccion.Text, txtcorreo.Text, 0, ddlCiudad.SelectedValue, txtsymphony.Text, 0, txtdesripcion.Text, txttelefono.Text, txtFechaingreso.Text, ddlestado.SelectedValue, txtsolicitud.Text, txtAprobado.Text, TxtObservacion.Text, ddlClasificacion.SelectedValue)

            If Contratista.Estado <> ddlestado.SelectedValue Then
                If ddlestado.SelectedValue = 3 Then
                    op.LogContratistasAdd(txtIdentificacion.Text, "Estado Actualizado-Retirado", Int64.Parse(Session("IDUsuario").ToString()))
                End If
                If ddlestado.SelectedValue = 2 Then
                    op.LogContratistasAdd(txtIdentificacion.Text, "Estado Actualizado-Inactivado", Int64.Parse(Session("IDUsuario").ToString()))
                End If
            End If


            If Contratista.Nombre <> txtNombre.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Nombre Actualizado-" & Contratista.Nombre & "-" & txtNombre.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Direccion <> txtdireccion.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Direccion Actualizado-" & Contratista.Direccion & "-" & txtdireccion.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Email <> txtcorreo.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Correo Actualizado-" & Contratista.Email & "-" & txtcorreo.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.NumeroSymphony <> txtsymphony.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Sypmphony Actualizado-" & Contratista.NumeroSymphony & "-" & txtsymphony.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.DescripcionCuenta <> txtdesripcion.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Descripcion Actualizado-" & Contratista.DescripcionCuenta & "-" & txtdesripcion.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Telefono <> txttelefono.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Telefono Actualizado-" & Contratista.Telefono & "-" & txttelefono.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Solicitud <> txtsolicitud.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Solicitud Actualizado-" & Contratista.Solicitud & "-" & txtsolicitud.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Aprobado <> txtAprobado.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Aprobo Actualizado-" & Contratista.Aprobado & "-" & txtAprobado.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If
            If Contratista.Observaciones <> TxtObservacion.Text Then
                op.LogContratistasAdd(txtIdentificacion.Text, "Observaciones Actualizado-" & Contratista.Observaciones & "-" & TxtObservacion.Text, Int64.Parse(Session("IDUsuario").ToString()))
            End If

            End If
            If GvServicios.Rows.Count > 0 Then
                For i = 0 To GvServicios.Rows.Count - 1
                    If Servicios.Count > 0 Then
                        For y = 0 To Servicios.Count - 1
                            If GvServicios.Rows(i).Cells(2).Text = Servicios.Item(y).ServicioId Then
                                Dim Sel As CheckBox = DirectCast(GvServicios.Rows(i).FindControl("Estado"), CheckBox)
                                op.ActualizarEstadoServicioContratista(GvServicios.Rows(i).Cells(0).Text, Sel.Checked)
                                If Sel.Checked <> Servicios.Item(y).Estado Then
                                    op.LogContratistasAdd(txtIdentificacion.Text, "Estado Servicio Actualizado-" & GvServicios.Rows(i).Cells(0).Text, Int64.Parse(Session("IDUsuario").ToString()))
                                End If
                                Exit For
                            End If
                            If y + 1 = Servicios.Count Then
                                op.GuardarServiciosContratista(txtIdentificacion.Text, GvServicios.Rows(i).Cells(2).Text, GvServicios.Rows(i).Cells(3).Text, True)
                                op.LogContratistasAdd(txtIdentificacion.Text, "Servicio Adicionado-" & GvServicios.Rows(i).Cells(2).Text, Int64.Parse(Session("IDUsuario").ToString()))
                            End If
                        Next
                    Else
                        Dim Sel As CheckBox = DirectCast(GvServicios.Rows(i).FindControl("Estado"), CheckBox)
                        op.GuardarServiciosContratista(txtIdentificacion.Text, GvServicios.Rows(i).Cells(2).Text, GvServicios.Rows(i).Cells(3).Text, Sel.Checked)
                        op.LogContratistasAdd(txtIdentificacion.Text, "Servicio Adicionado-" & GvServicios.Rows(i).Cells(2).Text, Int64.Parse(Session("IDUsuario").ToString()))
                    End If


                Next
            End If



    End Sub

    Sub CargarClasificacion()
        Dim o As New CoreProject.RegistroPersonas
        ddlClasificacion.DataSource = o.ClasificacionList
        ddlClasificacion.DataValueField = "Id"
        ddlClasificacion.DataTextField = "Clasificacion"
        ddlClasificacion.DataBind()
        ddlClasificacion.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub
    Sub CargarCiudades()
        Dim o As New CoreProject.RegistroPersonas
        ddlCiudad.DataSource = o.CiudadesList
        ddlCiudad.DataValueField = "id"
        ddlCiudad.DataTextField = "Ciudad"
        ddlCiudad.DataBind()
        ddlCiudad.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarEstados()
        Dim op As New CoreProject.Contratista
        ddlestado.DataSource = op.ObtenerEstados
        ddlestado.DataValueField = "id"
        ddlestado.DataTextField = "Estado"
        ddlestado.DataBind()
        ddlestado.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarServicios()
        Dim op As New CoreProject.Contratista
        ddlservicio.DataSource = op.ObtenerServicios(Nothing)
        ddlservicio.DataValueField = "id"
        ddlservicio.DataTextField = "Servicio"
        ddlservicio.DataBind()
        ddlservicio.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub Limpiar()
        txtIdentificacion.Text = String.Empty
        txtNombre.Text = String.Empty
        txtdireccion.Text = String.Empty
        txtcorreo.Text = String.Empty
        ddlCiudad.SelectedValue = -1
        txtsymphony.Text = String.Empty
        ddlservicio.SelectedValue = -1
        txtdesripcion.Text = String.Empty
        txttelefono.Text = String.Empty
        txtFechaingreso.Text = String.Empty
        ddlestado.SelectedValue = -1
        txtsolicitud.Text = String.Empty
        txtAprobado.Text = String.Empty
        TxtObservacion.Text = String.Empty
        GvServicios.DataSource = Nothing
        GvServicios.DataBind()
    End Sub
    Sub AdicionarGv()
        For T = 0 To GvServicios.Rows.Count - 1
            Dim row As GridViewRow = GvServicios.Rows(T)
            If ddlservicio.SelectedValue = row.Cells(2).Text Then
                ShowNotification("Servicio ya agregado", ShowNotifications.InfoNotification)
                ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                Exit Sub
            End If
        Next
  

        Dim miDataTable As New DataTable
        Dim dRow As DataRow
        Dim r As Int16 = 0
        Dim Cont As New CoreProject.Contratista


        miDataTable.Columns.Add("Id")
        miDataTable.Columns.Add("IdentificacionId")
        miDataTable.Columns.Add("ServicioId")
        miDataTable.Columns.Add("ServicioDescripcion")
        miDataTable.Columns.Add("Estado")

        For i = 0 To GvServicios.Rows.Count - 1
            dRow = miDataTable.NewRow()
            Dim row As GridViewRow = GvServicios.Rows(i)
            dRow("Id") = row.Cells(0).Text
            dRow("IdentificacionId") = row.Cells(1).Text
            dRow("ServicioId") = row.Cells(2).Text
            dRow("ServicioDescripcion") = row.Cells(3).Text
            Dim Sel As CheckBox = DirectCast(GvServicios.Rows(row.RowIndex).FindControl("Estado"), CheckBox)
            dRow("Estado") = Sel.Checked
            miDataTable.Rows.Add(dRow)
            r = i + 1
        Next

        dRow = miDataTable.NewRow()
        dRow("Id") = r + 1
        dRow("IdentificacionId") = hfID.Value
        dRow("ServicioId") = ddlservicio.SelectedValue
        dRow("ServicioDescripcion") = ddlservicio.SelectedItem
        dRow("Estado") = True


        miDataTable.Rows.Add(dRow)
        GvServicios.DataSource = miDataTable
        GvServicios.DataBind()
    End Sub


    Protected Sub btnagregar_Click(sender As Object, e As EventArgs) Handles btnagregar.Click
        Dim servicios As List(Of TH_ContratistasDetalleServiciosGet_Result)
        Dim op As New CoreProject.Contratista
        If hfID.Value <> "" Then
            servicios = op.ObtenerServiciosContratista(txtIdentificacion.Text)
            For I = 0 To servicios.Count - 1
                If servicios.Item(I).ServicioId = ddlservicio.SelectedValue Then
                    ShowNotification("Servicio ya asociado a contratista", ShowNotifications.InfoNotification)
                    ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                    Exit Sub
                End If
            Next
        End If
        AdicionarGv()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub


    Sub ValidarServicio()
   
    End Sub
End Class

