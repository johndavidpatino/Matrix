Imports CoreProject
Imports System.Globalization

Public Class Actas
    Inherits System.Web.UI.Page

    Private _acta_id As String
    Public Property ActaId As String
        Get
            Return ViewState("_acta_id")
        End Get
        Set(value As String)
            ViewState("_acta_id") = value
        End Set
    End Property

    Private _tarea_id As String
    Public Property TareaId As String
        Get
            Return ViewState("_tarea_id")
        End Get
        Set(value As String)
            ViewState("_tarea_id") = value
        End Set
    End Property

    Public Function ObtenerEstadoActas() As List(Of SG_ActasTareasEstado_Result)
        Dim Data As New SG.Actas
        Try
            Return Data.ObtenerEstadoActas
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerTipoActas() As List(Of TipoActaCombo_Result)
        Dim Data As New SG.Actas
        Try
            Return Data.ObtenerTipoActa
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

    Public Function ObtenerUsuarios() As List(Of ObtenerUsuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Try
            Return Data.ObtenerUsuarios
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CargarActasGrid() As List(Of SG_Acta_Result)
        'Dim _sgActas As New SG.Actas
        'Dim list As List(Of SG_Acta_Result)
        'If Request.QueryString("idPQR") IsNot Nothing Then
        '    Dim CU As New CU.Pqr
        '    Dim PQRID = Request.QueryString("idPQR")
        '    list = _sgActas.ObtenerActas(PQRID)
        '    Session("ListaActas") = list
        'Else
        '    list = _sgActas.ObtenerActas
        '    Session("ListaActas") = list
        'End If
        'Return list
    End Function

    Public Function CargarTareasGrid(ByVal ActaId As Integer) As List(Of SG_Tarea_Result)
        Dim _sgActas As New SG.Tareas
        Dim list As List(Of SG_Tarea_Result)
        list = _sgActas.ConsultarTareasXActa(ActaId)
        Return list
    End Function

    Public Function CargarParticipanteActasId(ByVal ActaId As Integer) As List(Of SG_ParticipanteActa_Result)
        Dim _sgPActas As New SG.ParticipanteActa
        Dim list As List(Of SG_ParticipanteActa_Result)
        list = _sgPActas.ConsultarParticipanteActaXNoActa(ActaId)
        Return list
    End Function

    Public Function CargarActasGridNoActa(ByVal NoActa As Integer) As List(Of SG_Acta_Result)
        Dim _sgActas As New SG.Actas
        Dim list As List(Of SG_Acta_Result)
        list = _sgActas.ConsultarActasXNoActa(NoActa)
        Session("ListaActas") = list
        Return list
    End Function

    Public Function CargarActasGridTipoActa(ByVal TipoActa As Integer) As List(Of SG_Acta_Result)
        Dim _sgActas As New SG.Actas
        Dim list As List(Of SG_Acta_Result)
        list = _sgActas.ConsultarActasXNoActa(TipoActa)
        Session("ListaActas") = list
        Return list
    End Function

    Public Function CargarActasGridUnidad(ByVal UnidadId As Integer) As List(Of SG_Acta_Result)
        Dim _sgActas As New SG.Actas
        Dim list As List(Of SG_Acta_Result)
        list = _sgActas.ConsultarActasXNoActa(UnidadId)
        Session("ListaActas") = list
        Return list
    End Function

    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()
    End Sub

    Function ValidarTarea() As Boolean
        Dim Val = True
        If txtTarea.Text = String.Empty Then
            lblResult.Text = "Escriba la descripción de la tarea"
            Val = False
        End If
        Return Val
    End Function

    Function Validar() As Boolean
        Dim Val = True
        If txtCompromisosConclusiones.Content = String.Empty Then
            lblResult.Text = "Escriba los compromisos conclusiones"
            Val = False
        End If
        If txtDenominacion.Text = String.Empty Then
            lblResult.Text = "Escriba la denominación"
            Val = False
        End If
        If txtNoActa.Text = String.Empty Then
            lblResult.Text = "Escriba el Numero del acta"
            Val = False
        End If
        If txtSeguimientoAcciones.Content = String.Empty Then
            lblResult.Text = "Escriba los seguimientos acciones"
            Val = False
        End If
        If txtSeguimientoCompromisos.Content = String.Empty Then
            lblResult.Text = "Escriba los seguimientos compromisos"
            Val = False
        End If
        If txtTemasTratados.Content = String.Empty Then
            lblResult.Text = "Escriba los temas tratados"
            Val = False
        End If
        If ddTipoActa.Text = String.Empty Then
            lblResult.Text = "Seleccione un tipo de acta"
            Val = False
        End If
        Return Val
    End Function

    Sub Limpiar()
        txtCompromisosConclusiones.Content = String.Empty
        txtDenominacion.Text = String.Empty
        txtNoActa.Text = String.Empty
        txtSeguimientoAcciones.Content = String.Empty
        txtSeguimientoCompromisos.Content = String.Empty
        txtTemasTratados.Content = String.Empty
        chkActiva.Checked = False
        chkPublica.Checked = False
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'cargamos el combo de actas
            Dim Data As New SG.Actas
            CargarCombo(ddTipoActa, "id", "Tipo", ObtenerTipoActas())
            'CargarCombo(ddlTareaEstado, "id", "TareaEstado", ObtenerEstadoActas())

            txtNoActa.Text = Data.UltimoNoActa(ddTipoActa.SelectedValue)
            gvDatos.DataSource = CargarActasGrid()

            gvDatos.DataBind()
            gvDatos.DataBind()
            CargarCombo(ddTipoActaQuery, "id", "Tipo", ObtenerTipoActas())
            CargarCombo(ddUnidadQuery, "id", "Unidad", ObtenerUnidad())
            CargarCombo(ddLider, "id", "Nombres", ObtenerUsuarios())
            CargarCombo(ddResponsable, "id", "Nombres", ObtenerUsuarios())
            gvDatos.DataSource = CargarActasGrid()
            gvDatos.DataBind()
            btnGuardar.Visible = False
        End If
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim List As List(Of Usuarios_Result)
        Dim res As Integer = 0
        If Validar() = True Then
            Dim Data As New SG.Actas
            txtNoActa.Text = Data.UltimoNoActa(ddTipoActa.SelectedValue)
            res = Data.GuardarActa(txtDenominacion.Text, txtNoActa.Text, ddTipoActa.SelectedValue, 1, "1047223102", ddLider.SelectedValue, txtSeguimientoCompromisos.Content.ToString, txtSeguimientoAcciones.Content, txtTemasTratados.Content, txtCompromisosConclusiones.Content, chkActiva.Checked, 1, 1, chkPublica.Checked)
            lblResult.Text = "Registro guardado correctamente"
            btnGuardar.Visible = False
            btnCancelar.Value = "Regresar"
            List = Session("Usuarios")
            For Each a As Usuarios_Result In List
                Dim DataIA As New SG.ParticipanteActa
                Try
                    DataIA.GuardarParticipanteActa(res, a.id)
                Catch ex As Exception
                    Throw ex
                End Try
            Next
            DvParticipantes.Visible = True
            gvParticipantes.DataSource = CargarParticipanteActasId(res)
            gvParticipantes.DataBind()
            gvDatos.DataSource = CargarActasGrid()
            gvDatos.DataBind()
            ActaId = res
            'lista_actas.Visible = True
            'gestion_actas.Visible = False
            DvTareas.Visible = True
            If Request.QueryString("idPQR") IsNot Nothing Then
                'Dim CU As New CU.Pqr
                'Dim PQRID = Request.QueryString("idPQR")

                ''tener en cuenta esto q ha sido borrado de PQR (Modificado por Víctor Mercado)
                'CU.AgregarPQRActa(Request.QueryString("idPQR"), ActaId)
            Else

            End If
            Limpiar()
        End If
    End Sub

    Protected Sub gvDatos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvDatos.SelectedIndexChanged
        DvParticipantes.Visible = True
        DvTareas.Visible = True
        Dim row As GridViewRow = gvDatos.SelectedRow
        gvParticipantes.DataSource = CargarParticipanteActasId(CInt(gvDatos.DataKeys(row.RowIndex).Values("id")))
        gvParticipantes.DataBind()
        gvTareas.DataSource = CargarTareasGrid(CInt(gvDatos.DataKeys(row.RowIndex).Values("id")))
        gvTareas.DataBind()
        ActaId = CInt(gvDatos.DataKeys(row.RowIndex).Values("id"))
        txtCompromisosConclusiones.Content = Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("CompromisosConclusiones"))
        txtDenominacion.Text = Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("Denominacion"))
        txtNoActa.Text = Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("NoActa"))
        txtSeguimientoAcciones.Content = Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("SeguimientoAcciones"))
        txtSeguimientoCompromisos.Content = Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("SeguimientoCompromisos"))
        txtTemasTratados.Content = Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("TemasTratados"))
        ddTipoActa.SelectedValue = Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("TipoId"))
        chkActiva.Checked = Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("Activa"))
        chkPublica.Checked = Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("Publica"))
        ddLider.SelectedValue = gvDatos.DataKeys(row.RowIndex).Values("Lider")
        Dim e3 As Integer = gvDatos.DataKeys(row.RowIndex).Values("Lider")
        btnEditar.Visible = True
        btnVerDetalle.Visible = True
        btnGuardar.Visible = False
        Dim Activa As String = "NO"
        Dim Publica As String = "NO"

        If gvDatos.DataKeys(row.RowIndex).Values("Activa") Then
            Activa = "SI"
        End If
        If gvDatos.DataKeys(row.RowIndex).Values("Publica") Then
            Publica = "SI"
        End If
        'template
        Dim tpl = "<style type='text/css'>" _
            & ".form-row {overflow:hidden; font-size:11px; border-bottom:1.5px solid #eee; }" _
            & "</style>" _
            & "<style type='text/css'>" _
            & ".form-titles {overflow:hidden; font-size:11px; width:120px;line-height:25px; }" _
            & "</style>" _
            & "<div style='margin: 10px 10px 10px 10px; width:98%; background-color:White;'>" _
            & "<table style='width:98%;'>" _
            & "<tr><td class='form-titles'><strong>Id:</strong></td>" _
            & "<td class='form-row'>" & CInt(gvDatos.DataKeys(row.RowIndex).Values("id")) & "</td></tr>" _
            & "<tr><td class='form-titles'><strong>Compromisos conclusiones</strong></td>" _
            & "<td class='form-row'>" & Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("CompromisosConclusiones")) & "</td></tr>" _
            & "<tr><td class='form-titles'><strong>Denominación:</strong></td>" _
            & "<td class='form-row'>" & Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("Denominacion")) & "</td></tr>" _
            & "<tr><td class='form-titles'><strong>No de acta:</strong></td>" _
            & "<td class='form-row'>" & Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("NoActa")) & "</td></tr>" _
            & "<tr><td class='form-titles'><strong>Tipo Unidad:</strong></td>" _
            & "<td class='form-row'>" & Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("Tipo")) & "</td></tr>" _
            & "<tr><td class='form-titles'><strong>Secretario:</strong></td>" _
            & "<td class='form-row'>" & Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("NombreSecretario")) & "</td></tr>" _
            & "<tr><td class='form-titles'><strong>Lider:</strong></td>" _
            & "<td class='form-row'>" & Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("NombreLider")) & "</td></tr>" _
            & "<tr><td class='form-titles'><strong>Compromisos:</strong></td>" _
            & "<td class='form-row'>" & Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("SeguimientoCompromisos")) & "</td></tr>" _
            & "<tr><td class='form-titles'><strong>Acciones:</strong></td>" _
            & "<td class='form-row'>" & Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("SeguimientoAcciones")) & "</td></tr>" _
            & "<tr><td class='form-titles'><strong>Conclusiones:</strong></td>" _
            & "<td class='form-row'>" & Convert.ToString(gvDatos.DataKeys(row.RowIndex).Values("CompromisosConclusiones")) & "</td></tr>" _
            & "<tr><td class='form-titles'><strong>Activa:</strong></td>" _
            & "<td class='form-row'>" & Activa & "</td></tr>" _
            & "<tr><td class='form-titles'><strong>Publica:</strong></td>" _
            & "<td class='form-row'>" & Publica & "</td></tr>" _
            & "</table>" _
            & "</div>"
        TplD.InnerHtml = tpl
        gestion_actas.Visible = True
        lista_actas.Visible = False
    End Sub

    'Protected Sub gvDatos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
    '    If e.CommandName = "eliminar" Then
    '        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
    '        Dim Data As New SG.Actas
    '        Dim id As Integer = Convert.ToInt32(gvDatos.DataKeys(index).Value)
    '        Data.EliminarActa(id)
    '        gvDatos.DataSource = CargarActasGrid()
    '        gvDatos.DataBind()
    '    End If
    'End Sub

    Protected Sub gvDatos_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvDatos.RowDeleting
        Dim row As GridViewRow = gvDatos.Rows(e.RowIndex)
        Dim Data As New SG.Actas
        Dim Id As String = row.Cells(0).Text
        Data.EliminarActa(Id)
        gvDatos.DataSource = CargarActasGrid()
        gvDatos.DataBind()
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If Validar() = True Then
            Dim Data As New SG.Actas
            Dim res As Integer = 0
            res = Data.EditarActa(ActaId, txtDenominacion.Text, txtNoActa.Text, ddTipoActa.SelectedValue, 1, "1047223102", ddLider.SelectedValue, txtSeguimientoCompromisos.Content, txtSeguimientoAcciones.Content, txtTemasTratados.Content, txtCompromisosConclusiones.Content, chkActiva.Checked, 1, 1, chkPublica.Checked)
            If res > 0 Then
                lblResult.Text = "Registro editado correctamente"
                gvDatos.DataSource = CargarActasGrid()
                gvDatos.DataBind()
                lista_actas.Visible = True
                gestion_actas.Visible = False
                Limpiar()
                ActaId = res
            End If
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        gestion_actas.Visible = True
        lista_actas.Visible = False
        btnGuardar.Visible = True
        DvTareas.Visible = False
        DvParticipantes.Visible = False
        lblResult.Text = ""
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Select Case ddTipoFiltro.SelectedValue
            Case "0"
                gvDatos.DataSource = CargarActasGridNoActa(txtBuscar.Text)
                gvDatos.DataBind()
            Case "1"
                gvDatos.DataSource = CargarActasGridTipoActa(ddTipoActaQuery.SelectedValue)
                gvDatos.DataBind()
            Case "2"
                gvDatos.DataSource = CargarActasGridUnidad(ddUnidadQuery.SelectedValue)
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub btnVerDetalle_Click(sender As Object, e As EventArgs) Handles btnVerDetalle.Click
        lista_actas.Visible = False
        gestion_actas.Visible = False
        detalle_actas.Visible = True
        btnEditar.Visible = False
    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        SelUser.CargarGrid(1, txtQuery.Text)
    End Sub

    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        gvDatos.DataSource = Session("ListaActas")
        gvDatos.DataBind()
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddTipoFiltro.SelectedIndexChanged
        Select Case ddTipoFiltro.SelectedValue
            Case "0"
                txtBuscar.Visible = True
                ddTipoActaQuery.Visible = False
                ddUnidadQuery.Visible = False
            Case "1"
                txtBuscar.Visible = False
                ddTipoActaQuery.Visible = True
                ddUnidadQuery.Visible = False
            Case "2"
                txtBuscar.Visible = False
                ddTipoActaQuery.Visible = False
                ddUnidadQuery.Visible = True
        End Select
    End Sub

    Protected Sub gvParticipantes_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvParticipantes.RowCommand
        Try
            Select Case e.CommandName
                Case "eliminar"
                    Dim PA As New SG.ParticipanteActa
                    Dim Id As Int32 = Int32.Parse(gvParticipantes.DataKeys(CInt(e.CommandArgument))("Id"))
                    ActaId = gvParticipantes.DataKeys(CInt(e.CommandArgument))("ActaId")
                    PA.EliminarParticipanteActa(Id)
                    gvParticipantes.DataSource = CargarParticipanteActasId(ActaId)
                    gvParticipantes.DataBind()
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnAddTarea_Click(sender As Object, e As EventArgs) Handles btnAddTarea.Click

        If ValidarTarea() Then
            If btnAddTarea.Text = "Editar tarea" Then
                Dim SG As New SG.Tareas
                SG.EditarTarea(TareaId, ActaId, txtTarea.Text, ddResponsable.SelectedValue, chkCerrada.Checked, DateTime.UtcNow.AddHours(-5), DateTime.ParseExact(calFechaInicio.Text, "M/d/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(calFechaLimite.Text, "M/d/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(calFechaCierre.Text, "M/d/yyyy", CultureInfo.InvariantCulture), 3)
                If chkCerrada.Checked = True Then
                    SG.EditarEstado(TareaId, 4)
                End If
                gvTareas.DataSource = CargarTareasGrid(ActaId)
                gvTareas.DataBind()
                btnAddTarea.Text = "Nueva tarea"
            Else
                Try
                    Dim SG As New SG.Tareas
                    SG.GuardarTarea(ActaId, txtTarea.Text, ddResponsable.SelectedValue, chkCerrada.Checked, DateTime.UtcNow.AddHours(-5), DateTime.ParseExact(calFechaInicio.Text, "M/d/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(calFechaLimite.Text, "M/d/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(calFechaCierre.Text, "M/d/yyyy", CultureInfo.InvariantCulture), 1)
                    If chkCerrada.Checked = True Then
                        SG.EditarEstado(TareaId, 4)
                    End If
                    gvTareas.DataSource = CargarTareasGrid(ActaId)
                    gvTareas.DataBind()
                    btnAddTarea.Text = "Nueva tarea"
                Catch ex As Exception
                    Throw ex
                End Try

            End If
        End If
    End Sub

    Protected Sub gvTareas_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareas.RowCommand
        Dim PA As New SG.Tareas
        Try
            Select Case e.CommandName
                Case "eliminar"
                    Dim Id As Int32 = Int32.Parse(gvTareas.DataKeys(CInt(e.CommandArgument))("Id"))
                    ActaId = gvTareas.DataKeys(CInt(e.CommandArgument))("ActaId")
                    PA.EliminarTarea(Id)
                    gvTareas.DataSource = CargarTareasGrid(ActaId)
                    gvTareas.DataBind()
                Case "seguimiento"
                    Dim Id As Int32 = Int32.Parse(gvTareas.DataKeys(CInt(e.CommandArgument))("Id"))
                    Response.Redirect("Seguimiento.aspx?idTarea=" & Id.ToString)
                Case "Editar"
                    TareaId = Int32.Parse(gvTareas.DataKeys(CInt(e.CommandArgument))("Id"))
                    ActaId = gvTareas.DataKeys(CInt(e.CommandArgument))("ActaId")
                    txtTarea.Text = gvTareas.DataKeys(CInt(e.CommandArgument))("Tarea").ToString
                    ddResponsable.SelectedValue = gvTareas.DataKeys(CInt(e.CommandArgument))("Responsable")
                    Dim fi = CDate(gvTareas.DataKeys(CInt(e.CommandArgument))("FechaInicioEjecucion"))
                    Dim fl = CDate(gvTareas.DataKeys(CInt(e.CommandArgument))("FechaLimite"))
                    Dim fc = CDate(gvTareas.DataKeys(CInt(e.CommandArgument))("FechaCierre"))
                    calFechaInicio.Text = fi.Month & "/" & fi.Day & "/" & fi.Year
                    calFechaLimite.Text = fl.Month & "/" & fl.Day & "/" & fl.Year
                    calFechaCierre.Text = fc.Month & "/" & fc.Day & "/" & fc.Year
                    chkCerrada.Checked = gvTareas.DataKeys(CInt(e.CommandArgument))("Cerrada")
                    btnAddTarea.Text = "Editar tarea"
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnAddParticipantes_Click(sender As Object, e As EventArgs) Handles btnAddParticipantes.Click
        Dim List As List(Of Usuarios_Result)
        List = Session("Usuarios")
        For Each a As Usuarios_Result In List
            Dim DataIA As New SG.ParticipanteActa
            Try
                DataIA.GuardarParticipanteActa(ActaId, a.id)
            Catch ex As Exception
                Throw ex
            End Try
        Next
        DvParticipantes.Visible = True
        gvParticipantes.DataSource = CargarParticipanteActasId(ActaId)
        gvParticipantes.DataBind()
    End Sub

    Protected Sub ddTipoActa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddTipoActa.SelectedIndexChanged
        Dim Data As New SG.Actas
        txtNoActa.Text = Data.UltimoNoActa(ddTipoActa.SelectedValue)
    End Sub
End Class