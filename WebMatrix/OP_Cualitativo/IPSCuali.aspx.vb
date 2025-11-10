Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.OP
Imports ClosedXML.Excel
Public Class IPSCuali1
    Inherits System.Web.UI.Page

#Region "Enumeradores"
    Enum eTarea
        CualitativoIntrumentos = 26
    End Enum
#End Region

#Region "Propiedades"
    Private _tareaactual As CORE_WorkFlow_Trabajos_Get_Result

    Public Property tareaactual() As CORE_WorkFlow_Trabajos_Get_Result
        Get
            Return _tareaactual
        End Get
        Set(ByVal value As CORE_WorkFlow_Trabajos_Get_Result)
            _tareaactual = value
        End Set
    End Property

#End Region


#Region "Eventos del Control"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("idtrabajo") IsNot Nothing Then
            If Request.QueryString("fromgerencia") IsNot Nothing Then
                hfGerentePY.Value = True
                Me.gvRevision.Columns(14).Visible = False
                Me.btnNotificar.Visible = False
                Me.btnRechazar.Visible = False
            Else
                hfGerentePY.Value = False
                Me.gvRevision.Columns(10).Visible = False
                Me.gvRevision.Columns(11).Visible = False
                Me.btnRechazar.Visible = False
            End If
            hfidtrabajo.Value = Int64.Parse(Request.QueryString("idtrabajo").ToString)
            If Not (Request.QueryString("idtarea") Is Nothing) Then hfidwf.Value = Int64.Parse(Request.QueryString("idtarea").ToString)

            Dim daCentro As New WorkFlow
            tareaactual = daCentro.obtenerXId(hfidwf.Value)
            hfidtarea.Value = tareaactual.TareaId

            lblNombreTarea.InnerText = "Tarea: " + tareaactual.Tarea

            If tareaactual.TareaId = eTarea.CualitativoIntrumentos Then
                Me.gvRevision.Columns(3).Visible = True
            Else
                Me.gvRevision.Columns(3).Visible = False
            End If
            Me.gvRevision.Columns(4).Visible = False
            Me.gvRevision.Columns(5).Visible = False
            Me.gvRevision.Columns(6).Visible = False
        Else
            Response.Redirect("~/ES_Estadistica/Trabajos.aspx")
        End If

        If Not IsPostBack Then
            CargarGrid()
        End If


    End Sub

    Private Sub gvRevision_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRevision.RowDataBound
        Dim o As New RevisionIPS
        If Request.QueryString("fromgerencia") IsNot Nothing Then

            If e.Row.RowType = DataControlRowType.EmptyDataRow Then
                DirectCast(e.Row.FindControl("ctrl_btnaddempty"), Button).Visible = False
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                DirectCast(e.Row.FindControl("ctrl_btnaddfooter"), Button).Visible = False
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                If (e.Row.RowState And DataControlRowState.Edit) > 0 Then
                    DirectCast(e.Row.FindControl("txtFechaRegedit"), TextBox).Enabled = False
                    DirectCast(e.Row.FindControl("txtInstrumentoedit"), DropDownList).Enabled = False
                    DirectCast(e.Row.FindControl("txtPreguntaedit"), TextBox).Enabled = False
                    DirectCast(e.Row.FindControl("txtObservacionedit"), DropDownList).Enabled = False
                    DirectCast(e.Row.FindControl("txtDescripcionedit"), TextBox).Enabled = False
                    DirectCast(e.Row.FindControl("txtVersionedit"), TextBox).Enabled = False
                    DirectCast(e.Row.FindControl("ddlAplicativoedit"), DropDownList).Enabled = False
                    DirectCast(e.Row.FindControl("ddlProcesoedit"), DropDownList).Enabled = False
                End If
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                DirectCast(e.Row.FindControl("txtFechaRegfooter"), TextBox).Visible = False
                DirectCast(e.Row.FindControl("txtInstrumentofooter"), DropDownList).Visible = False
                DirectCast(e.Row.FindControl("txtPreguntafooter"), TextBox).Visible = False
                DirectCast(e.Row.FindControl("txtObservacionfooter"), DropDownList).Visible = False
                DirectCast(e.Row.FindControl("txtDescripcionfooter"), TextBox).Visible = False
                DirectCast(e.Row.FindControl("txtEspaciofooter"), TextBox).Visible = False
                DirectCast(e.Row.FindControl("txtRechazarfooter"), DropDownList).Visible = False
                DirectCast(e.Row.FindControl("ddlAplicativofooter"), DropDownList).Visible = False
                DirectCast(e.Row.FindControl("ddlProcesofooter"), DropDownList).Visible = False
                DirectCast(e.Row.FindControl("txtVersionfooter"), TextBox).Visible = False
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                If Session("IDUsuario").ToString = CType(e.Row.DataItem, OP_IPS_Revision_Get_Result).IdUsuarioAsignado Then
                    If (e.Row.RowState And DataControlRowState.Edit) = 0 Then
                        DirectCast(e.Row.FindControl("ImageButton32223"), ImageButton).Visible = True
                        DirectCast(e.Row.FindControl("ImageButton2"), ImageButton).Visible = False
                    End If
                ElseIf (e.Row.RowState And DataControlRowState.Edit) = 0 OrElse e.Row.RowType = DataControlRowType.DataRow Then
                    DirectCast(e.Row.FindControl("ImageButton32223"), ImageButton).Visible = False
                    DirectCast(e.Row.FindControl("ImageButton2"), ImageButton).Visible = False
                End If
            End If
        Else

            If e.Row.RowType = DataControlRowType.DataRow And e.Row.RowState = DataControlRowState.Edit Then
                DirectCast(e.Row.FindControl("txtFechaRegedit"), TextBox).Enabled = False
                DirectCast(e.Row.FindControl("txtEspacioedit"), TextBox).Enabled = False
                DirectCast(e.Row.FindControl("txtRechazaredit"), DropDownList).Enabled = False
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                If (e.Row.RowState And DataControlRowState.Edit) > 0 Then
                    DirectCast(e.Row.FindControl("txtFechaRegedit"), TextBox).Enabled = False
                    DirectCast(e.Row.FindControl("txtEspacioedit"), TextBox).Enabled = False
                    DirectCast(e.Row.FindControl("txtRechazaredit"), DropDownList).Enabled = False
                End If
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                If Session("IDUsuario").ToString = CType(e.Row.DataItem, OP_IPS_Revision_Get_Result).IdUsuarioRegistra Then
                    If (e.Row.RowState And DataControlRowState.Edit) = 0 Then
                        DirectCast(e.Row.FindControl("ImageButton32223"), ImageButton).Visible = True
                        DirectCast(e.Row.FindControl("ImageButton2"), ImageButton).Visible = True
                    End If
                ElseIf (e.Row.RowState And DataControlRowState.Edit) = 0 OrElse e.Row.RowType = DataControlRowType.DataRow Then
                    DirectCast(e.Row.FindControl("ImageButton32223"), ImageButton).Visible = False
                    DirectCast(e.Row.FindControl("ImageButton2"), ImageButton).Visible = False
                End If
            End If

        End If



        If e.Row.RowType = DataControlRowType.EmptyDataRow Then
            If tareaactual.TareaId = eTarea.CualitativoIntrumentos Then
                DirectCast(e.Row.FindControl("txtInstrumentoEmpty"), DropDownList).Visible = True
                e.Row.FindControl("colInstrumento").Visible = True
                e.Row.FindControl("colInstrumento2").Visible = True
            Else
                DirectCast(e.Row.FindControl("txtInstrumentoEmpty"), DropDownList).Visible = False
                e.Row.FindControl("colInstrumento").Visible = False
                e.Row.FindControl("colInstrumento2").Visible = False
            End If

            DirectCast(e.Row.FindControl("ddlAplicativoEmpty"), DropDownList).Visible = False
            e.Row.FindControl("colAplicativo").Visible = False
            e.Row.FindControl("colAplicativo2").Visible = False

            DirectCast(e.Row.FindControl("ddlProcesoEmpty"), DropDownList).Visible = False
            e.Row.FindControl("colProceso").Visible = False
            e.Row.FindControl("colProceso2").Visible = False

            DirectCast(e.Row.FindControl("txtPreguntaEmpty"), TextBox).Visible = False
            e.Row.FindControl("colPregunta").Visible = False
            e.Row.FindControl("colPregunta2").Visible = False

            DirectCast(e.Row.FindControl("ddlProcesoEmpty"), DropDownList).Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = "0"})

        End If

        If e.Row.RowType = DataControlRowType.DataRow And e.Row.RowState = DataControlRowState.Edit Then
            DirectCast(e.Row.FindControl("txtFechaProgedit"), TextBox).Enabled = False
            DirectCast(e.Row.FindControl("txtFechaRegedit"), TextBox).Enabled = False
            DirectCast(e.Row.FindControl("txtUsuarioRegedit"), TextBox).Enabled = False
            DirectCast(e.Row.FindControl("txtUsuarioProgedit"), TextBox).Enabled = False
            DirectCast(e.Row.FindControl("txtEstadoedit"), TextBox).Enabled = False
            DirectCast(e.Row.FindControl("txtVersionedit"), TextBox).Enabled = False
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            DirectCast(e.Row.FindControl("txtFechaRegfooter"), TextBox).Enabled = False
            DirectCast(e.Row.FindControl("txtFechaProgfooter"), TextBox).Enabled = False
            DirectCast(e.Row.FindControl("txtUsuarioRegfooter"), TextBox).Enabled = False
            DirectCast(e.Row.FindControl("txtUsuarioProgfooter"), TextBox).Enabled = False
            DirectCast(e.Row.FindControl("txtEstadofooter"), TextBox).Enabled = False
            DirectCast(e.Row.FindControl("txtEspaciofooter"), TextBox).Enabled = False
            DirectCast(e.Row.FindControl("txtRechazarfooter"), DropDownList).Enabled = False

            DirectCast(e.Row.FindControl("ddlProcesofooter"), DropDownList).Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = "0"})
        End If

        Me.gvRevision.Columns(2).Visible = False
        If tareaactual.TareaId = eTarea.CualitativoIntrumentos Then
            Me.gvRevision.Columns(3).Visible = True
        Else
            Me.gvRevision.Columns(3).Visible = False
        End If
        Me.gvRevision.Columns(4).Visible = False
        Me.gvRevision.Columns(5).Visible = False
        Me.gvRevision.Columns(6).Visible = False
        Me.gvRevision.Columns(10).Visible = False
        Me.gvRevision.Columns(11).Visible = False
        Me.gvRevision.Columns(14).Visible = False
    End Sub

    Protected Sub gvRevision_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvRevision.RowEditing
        gvRevision.EditIndex = e.NewEditIndex
        CargarGrid()
    End Sub

    Protected Sub gvRevision_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvRevision.RowCancelingEdit
        gvRevision.EditIndex = -1
        CargarGrid()
    End Sub

    Protected Sub gvRevision_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvRevision.RowUpdating

        Dim oRevision As New RevisionIPS
        Dim oItemsRevision As New List(Of ItemsRevision)
        Dim fila As GridViewRow = gvRevision.Rows(e.RowIndex)
        Dim Index As Int32 = e.RowIndex
        Dim oid As Label = CType(fila.FindControl("lblid"), Label)
        Dim otxtPregunta As String
        Dim otxtObservacion As DropDownList = CType(fila.FindControl("txtObservacionedit"), DropDownList)
        Dim otxtDescripcion As TextBox = CType(fila.FindControl("txtDescripcionedit"), TextBox)
        Dim otxtEspacio As TextBox = CType(fila.FindControl("txtEspacioedit"), TextBox)
        Dim otxtRechazar As DropDownList = CType(fila.FindControl("txtRechazaredit"), DropDownList)
        Dim otxtUsuarioProgramador As Nullable(Of Long)
        Dim otxtInstrumento As New Byte?
        Dim otxtVersion As String
        Dim odllAplicativo As New Int32?
        Dim odllProceso As New Int64?

        If Request.QueryString("fromgerencia") IsNot Nothing Then
            otxtUsuarioProgramador = CType(Session("IDUsuario").ToString, Long?)
        Else
            otxtUsuarioProgramador = Nothing
        End If

        If tareaactual.TareaId = eTarea.CualitativoIntrumentos Then
            otxtInstrumento = CType(fila.FindControl("txtInstrumentoedit"), DropDownList).SelectedValue
        End If

        otxtVersion = CType(fila.FindControl("txtVersionedit"), TextBox).Text
        otxtPregunta = Nothing

        Dim otxtEstado As New TextBox

        If otxtEspacio.Text = Nothing Then
            otxtEstado.Text = 1
        Else
            otxtEstado.Text = 0
        End If

        oRevision.Guardar(oid.Text, hfidtrabajo.Value, hfidwf.Value, otxtPregunta, otxtObservacion.Text, otxtDescripcion.Text, otxtEspacio.Text, otxtRechazar.Text, otxtEstado.Text, Nothing, otxtUsuarioProgramador, otxtInstrumento, otxtVersion, odllAplicativo, odllProceso)

        If Request.QueryString("fromgerencia") IsNot Nothing Then
            EnviarEmail(oid.Text, 1)
        End If

        gvRevision.EditIndex = -1
        CargarGrid()

    End Sub

    Protected Sub gvRevision_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRevision.RowCommand
        Try
            If e.CommandName = "Delete" Then

                Dim oRevision As New RevisionIPS
                Dim Index As Int32 = Int32.Parse(e.CommandArgument)
                Dim row = gvRevision.Rows(Index)
                Dim oid As Label = CType(row.FindControl("lblid"), Label)
                Dim oItemsRevision As New List(Of ItemsRevision)

                oRevision.Eliminar(oid.Text)
                gvRevision.EditIndex = -1
                CargarGrid()

            End If
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try

    End Sub

    Protected Sub gvRevision_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvRevision.RowDeleting

    End Sub

    Protected Sub btnNotificar_Click(sender As Object, e As EventArgs) Handles btnNotificar.Click

        Dim oRevision As New RevisionIPS
        Dim olistarevision = oRevision.DevolverxTareaId(hfidtrabajo.Value, hfidwf.Value).FirstOrDefault

        If olistarevision.IdUsuarioAsignado IsNot Nothing Then
            EnviarEmail(gvRevision.DataKeys(0)("Id"), 2)
            ShowNotification("Notificación enviada correctamente", ShowNotifications.InfoNotification)
        Else
            ShowNotification("La Notificación no puede ser enviada, porque no hay un Usuario Asignado para esta tarea", ShowNotifications.ErrorNotification)
        End If

    End Sub

    Protected Sub btnRechazar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRechazar.Click
        'REVISION
        Dim index As Integer
        Dim Registro As New RevisionIPS
        Dim oItemsRevision As New List(Of ItemsRevision)
        If ViewState("ItemsRevision") IsNot Nothing Then
            oItemsRevision = CType(ViewState("ItemsRevision"), List(Of ItemsRevision))
        End If
        For Each row As GridViewRow In gvRevision.Rows
            Dim blnCheckedRev As String = (DirectCast(row.FindControl("txtRechazarfooter"), DropDownList)).Text
            If blnCheckedRev.IndexOf("Rechazar") > -1 Then
                Registro.Guardar(oItemsRevision(index).ID, oItemsRevision(index).TrabajoId, oItemsRevision(index).TareaId, oItemsRevision(index).Pregunta, oItemsRevision(index).Observacion, oItemsRevision(index).DescripcionObservacion, oItemsRevision(index).RespuestaProgramador, "Rechazar", oItemsRevision(index).Estado, oItemsRevision(index).UsuarioRegistra, oItemsRevision(index).UsuarioProgramador, oItemsRevision(index).Instrumento, oItemsRevision(index).Version, oItemsRevision(index).Aplicativo, oItemsRevision(index).Proceso)
            End If
            index = index + 1
        Next

        ShowNotification("Error Rechazado", ShowNotifications.InfoNotification)
        CargarInfo()
        CargarGrid()
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim idtrabajo = hfidtrabajo.Value
        Dim url = "../CORE/Gestion-Tareas.aspx?IdTrabajo=" + idtrabajo
        Response.Redirect(url)
    End Sub

    Protected Sub gvRevision_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvRevision.Sorting
        Dim filtro = e.SortExpression
        Dim olistarevision As List(Of OP_IPS_Revision_Get_Result) = Session("olistarevision")

        Select Case filtro
            Case "FechaHoraObservacion"
                Dim d = From data In olistarevision Order By data.FechaHoraObservacion Select data
                gvRevision.DataSource = d
            Case "UsuarioRegistra"
                Dim d = From data In olistarevision Order By data.UsuarioRegistra Select data
                gvRevision.DataSource = d
            Case "Instrumento"
                Dim d = From data In olistarevision Order By data.Instrumento Select data
                gvRevision.DataSource = d
            Case "Pregunta"
                Dim d = From data In olistarevision Order By data.Pregunta Select data
                gvRevision.DataSource = d
            Case "Aplicativo"
                Dim d = From data In olistarevision Order By data.Aplicativo Select data
                gvRevision.DataSource = d
            Case "Proceso"
                Dim d = From data In olistarevision Order By data.Proceso Select data
                gvRevision.DataSource = d
            Case "Observacion"
                Dim d = From data In olistarevision Order By data.Observacion Select data
                gvRevision.DataSource = d
            Case "DescripcionObservacion"
                Dim d = From data In olistarevision Order By data.DescripcionObservacion Select data
                gvRevision.DataSource = d
            Case "VersionScript"
                Dim d = From data In olistarevision Order By data.VersionScript Select data
                gvRevision.DataSource = d
            Case "FechaHoraRespuesta"
                Dim d = From data In olistarevision Order By data.FechaHoraRespuesta Select data
                gvRevision.DataSource = d
            Case "UsuarioProgramador"
                Dim d = From data In olistarevision Order By data.UsuarioProgramador Select data
                gvRevision.DataSource = d
            Case "Rechazar"
                Dim d = From data In olistarevision Order By data.Rechazar Select data
                gvRevision.DataSource = d
            Case "RespuestaProgramador"
                Dim d = From data In olistarevision Order By data.RespuestaProgramador Select data
                gvRevision.DataSource = d
            Case "Estado"
                Dim d = From data In olistarevision Order By data.Estado Select data
                gvRevision.DataSource = d
            Case Else
                Dim d = From data In olistarevision Order By data.id Select data
                gvRevision.DataSource = d
        End Select

        gvRevision.DataBind()
    End Sub
#End Region

#Region "Funciones y Metodos"
    Public Sub CargarGrid()

        Dim oRevision As New RevisionIPS
        Dim olistarevision = oRevision.DevolverxTareaId(hfidtrabajo.Value, hfidwf.Value)
        Session.Add("olistarevision", olistarevision)

        gvRevision.DataSource = olistarevision.ToList
        gvRevision.DataBind()
    End Sub

    Public Sub CargarInfo()
        Try
            Dim TrabajoID As Int64 = Int64.Parse(hfidtrabajo.Value)

            ''Revisión
            Dim oRevision As New RevisionIPS
            Dim olistarevision = oRevision.DevolverxTrabajoID(TrabajoID)
            Dim oItemsrevision As New List(Of ItemsRevision)
            For Each fila As OP_IPS_Revision_Get_Result In olistarevision
                oItemsrevision.Add(New ItemsRevision With {.ID = fila.id, .DescripcionObservacion = fila.DescripcionObservacion, .FechaHoraObservacion = fila.FechaHoraObservacion, .FechaHoraRespuesta = fila.FechaHoraRespuesta,
                                                             .Observacion = fila.Observacion, .Pregunta = fila.Pregunta, .RespuestaProgramador = fila.RespuestaProgramador, .Rechazar = fila.Rechazar, .Estado = fila.Estado,
                                                             .UsuarioRegistra = fila.UsuarioRegistra, .UsuarioProgramador = fila.UsuarioProgramador, .Instrumento = fila.Instrumento, .Version = fila.VersionScript, .TrabajoId = fila.TrabajoId})
            Next

            ViewState("ItemsRevision") = oItemsrevision

        Catch ex As Exception
            Throw ex
        End Try
    End Sub




    Public Sub AgregarRevision(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oItemsRevision As New List(Of ItemsRevision)
        Dim oRevision As New RevisionIPS

        If gvRevision.Rows.Count > 0 Then
            Dim ofooter As Control = gvRevision.FooterRow
            Dim otxtFechaRegfooter As New TextBox With {.Text = DateTime.Now.ToString}
            Dim otxtUsuarioRegfooter As New TextBox With {.Text = Session("IDUsuario").ToString}
            Dim otxtPreguntafooter As String
            Dim otxtObservacionfooter As DropDownList = CType(ofooter.FindControl("txtObservacionfooter"), DropDownList)
            Dim otxtDescripcionfooter As TextBox = CType(ofooter.FindControl("txtDescripcionfooter"), TextBox)
            Dim otxtEspaciofooter As New TextBox With {.Text = Nothing}
            Dim otxtEstadofooter As New TextBox
            Dim otxtInstrumentofooter As New Byte?
            Dim otxtVersionfooter As String
            Dim odllAplicativofooter As New Int32?
            Dim odllProcesofooter As New Int64?

            If otxtEspaciofooter.Text = Nothing Then
                otxtEstadofooter.Text = 1
            Else
                otxtEstadofooter.Text = 0
            End If

            If tareaactual.TareaId = eTarea.CualitativoIntrumentos Then
                otxtInstrumentofooter = CType(ofooter.FindControl("txtInstrumentofooter"), DropDownList).SelectedValue
            End If

            otxtVersionfooter = CType(ofooter.FindControl("txtVersionfooter"), TextBox).Text
            otxtPreguntafooter = Nothing

            If tareaactual.TareaId = eTarea.CualitativoIntrumentos And otxtInstrumentofooter = 0 Then
                ShowNotification("Debe seleccionar el tipo de instrumento", ShowNotifications.ErrorNotification)
                ofooter.FindControl("txtInstrumentofooter").Focus()
                Exit Sub
            End If

            If String.IsNullOrEmpty(otxtVersionfooter) Then
                ShowNotification("Debe agregar la versión", ShowNotifications.ErrorNotification)
                ofooter.FindControl("txtVersionfooter").Focus()
                Exit Sub
            End If

            If String.IsNullOrEmpty(otxtDescripcionfooter.Text) Then
                ShowNotification("Debe agregar la descripción de la observación", ShowNotifications.ErrorNotification)
                ofooter.FindControl("txtDescripcionfooter").Focus()
                Exit Sub
            End If

            oRevision.Guardar(0, hfidtrabajo.Value, hfidwf.Value, otxtPreguntafooter, otxtObservacionfooter.Text, otxtDescripcionfooter.Text, otxtEspaciofooter.Text, Nothing, otxtEstadofooter.Text, otxtUsuarioRegfooter.Text, Nothing, otxtInstrumentofooter, otxtVersionfooter, odllAplicativofooter, odllProcesofooter)


        Else
            Dim oControl As Control = gvRevision.Controls(0).Controls(0)
            Dim otxtFechaRegEmpty As New TextBox With {.Text = DateTime.Now.ToString}
            Dim otxtUsuarioRegEmpty As New TextBox With {.Text = Session("IDUsuario").ToString}
            Dim otxtPreguntaEmpty As String
            Dim otxtObservacionEmpty As DropDownList = CType(oControl.FindControl("txtObservacionEmpty"), DropDownList)
            Dim otxtDescripcionEmpty As TextBox = CType(oControl.FindControl("txtDescripcionEmpty"), TextBox)
            Dim otxtEspacioEmpty As New TextBox With {.Text = Nothing}
            Dim otxtEstadoEmpty As New TextBox
            Dim otxtInstrumentoEmpty As New Byte?
            Dim otxtVersionEmpty As String
            Dim odllAplicativoEmpty As New Int32?
            Dim odllProcesoEmpty As New Int64?

            If otxtEspacioEmpty.Text = Nothing Then
                otxtEstadoEmpty.Text = 1
            Else
                otxtEstadoEmpty.Text = 0
            End If

            If tareaactual.TareaId = eTarea.CualitativoIntrumentos Then
                otxtInstrumentoEmpty = CType(oControl.FindControl("txtInstrumentoEmpty"), DropDownList).SelectedValue
            End If

            otxtVersionEmpty = CType(oControl.FindControl("txtVersionEmpty"), TextBox).Text
            otxtPreguntaEmpty = Nothing

            If tareaactual.TareaId = eTarea.CualitativoIntrumentos And otxtInstrumentoEmpty = 0 Then
                ShowNotification("Debe seleccionar el tipo de instrumento", ShowNotifications.ErrorNotification)
                oControl.FindControl("txtInstrumentoEmpty").Focus()
                Exit Sub
            End If

            If String.IsNullOrEmpty(otxtVersionEmpty) Then
                ShowNotification("Debe agregar la versión", ShowNotifications.ErrorNotification)
                oControl.FindControl("txtVersionEmpty").Focus()
                Exit Sub
            End If

            If String.IsNullOrEmpty(otxtDescripcionEmpty.Text) Then
                ShowNotification("Debe agregar la descripción de la observación", ShowNotifications.ErrorNotification)
                oControl.FindControl("txtDescripcionEmpty").Focus()
                Exit Sub
            End If

            oRevision.Guardar(0, hfidtrabajo.Value, hfidwf.Value, otxtPreguntaEmpty, otxtObservacionEmpty.Text, otxtDescripcionEmpty.Text, otxtEspacioEmpty.Text, Nothing, otxtEstadoEmpty.Text, otxtUsuarioRegEmpty.Text, Nothing, otxtInstrumentoEmpty, otxtVersionEmpty, odllAplicativoEmpty, odllProcesoEmpty)

        End If

        If ViewState("ItemsRevision") IsNot Nothing Then
            oItemsRevision = CType(ViewState("ItemsRevision"), List(Of ItemsRevision))
        End If

        CargarGrid()

    End Sub

    Public Sub GuardarCuestionario()
        Try
            ''Revisión
            Dim oItemsRevision As New List(Of ItemsRevision)
            If ViewState("ItemsRevision") IsNot Nothing Then
                oItemsRevision = CType(ViewState("ItemsRevision"), List(Of ItemsRevision))
            End If

            Dim oRevision As New RevisionIPS
            For Each fila As ItemsRevision In oItemsRevision
                oRevision.Guardar(fila.ID, hfidtrabajo.Value, hfidwf.Value, fila.Pregunta, fila.Observacion, fila.DescripcionObservacion, fila.RespuestaProgramador, fila.Rechazar, fila.Estado, fila.UsuarioRegistra, fila.UsuarioProgramador, fila.Instrumento, fila.Version, fila.Aplicativo, fila.Proceso)
            Next

            ''Eliminar los items de revisión borrados de la lista
            Dim IDRevision As New List(Of Int64)
            Dim olistarevision = oRevision.DevolverxTrabajoID(hfidtrabajo.Value)
            For Each fila As OP_IPS_Revision_Get_Result In olistarevision
                Dim Sw As Int32 = 0
                For Each item As ItemsRevision In oItemsRevision
                    If fila.id = item.ID Then
                        Sw = 1
                    ElseIf item.ID = -1 Then
                        Sw = 1
                    End If
                Next
                If Sw = 0 Then
                    IDRevision.Add(fila.id)
                End If
            Next

            For Each ID As Int64 In IDRevision
                oRevision.Eliminar(ID)
            Next

            ViewState("ItemsRevision") = Nothing

            CargarInfo()
            CargarGrid()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        exportarExcel()
    End Sub

#Region "Exportar a Excel"
    Sub exportarExcel()
        Dim wb As New XLWorkbook
        Dim oInconsistencias As List(Of OP_IPS_Revision_Get_Result)
        Dim titulosInconsistencias As String = "TrabajoId;Tarea;FechaHoraObservacion;Instrumento;Pregunta;Aplicativo;Proceso;TipoSolicitud;DescripcionObservacion;FechaHoraRevision;Rechazar;RespuestaRevisor;Estado;UsuarioRegistra;UsuarioRevisor;Version;JobBook;NombreTrabajo;UsuarioAsignado;COE;GerenteProyecto"
        oInconsistencias = obtenerRegistrosInconsistencias()

        Dim oExportar = (From x In oInconsistencias
                         Select x.TrabajoId, x.Tarea, x.FechaHoraObservacion, x.Instrumento, x.Pregunta, x.Aplicativo, x.Proceso, x.Observacion, x.DescripcionObservacion, x.FechaHoraRespuesta, x.Rechazar, x.RespuestaProgramador,
                        x.Estado, x.UsuarioRegistra, x.UsuarioProgramador, x.VersionScript, x.JobBook, x.NombreTrabajo, x.UsuarioAsignado, x.COE, x.GerenteProyecto).ToList

        Dim ws = wb.Worksheets.Add("Registro")
        insertarNombreColumnasExcel(ws, titulosInconsistencias.Split(";"))
        ws.Cell(2, 1).InsertData(oExportar)
        exportarExcel(wb, "Registro")
    End Sub

    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(1, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub

    Function obtenerRegistrosInconsistencias() As List(Of OP_IPS_Revision_Get_Result)
        Dim oRecordInconsistencias As New RevisionIPS
        Return oRecordInconsistencias.DevolverxTareaId(hfidtrabajo.Value, hfidwf.Value)
    End Function

    Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Observaciones_" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

#End Region
    Sub EnviarEmail(ByVal idrevision As Int64, ByVal tipo As Short)
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(hfidtrabajo.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar la respuesta de las observaciones")
            End If
            Dim script As String = ""
            oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/ObservacionesIPS.aspx?idRevision=" & idrevision & "&tipo=" & tipo)
            ShowNotification("Notificación enviada correctamente", ShowNotifications.InfoNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Sub validarcheck()
        Dim dtgItem As DataGridItem
        Dim Check As CheckBox
        For Each dtgItem In gvRevision.Rows
            Check = CType(dtgItem.Cells(0).Controls(1), CheckBox)
            If Check.Checked Then
                'realizar la función que queramos
            Else
            End If
        Next
    End Sub
#End Region

#Region "Propiedades"
    <Serializable()>
    Private Class ItemsRevision
        Public Property ID As Int64 = -1
        Public Property TrabajoId As Int64?
        Public Property TareaId As Int64?
        Public Property UsuarioRegistra As Int64?
        Public Property FechaHoraObservacion As DateTime?
        Public Property FechaHoraRespuesta As DateTime?
        Public Property Pregunta As String
        Public Property Observacion As String
        Public Property DescripcionObservacion As String
        Public Property UsuarioProgramador As Int64?
        Public Property RespuestaProgramador As String
        Public Property Rechazar As String
        Public Property Estado As String
        Public Property Instrumento As Byte?
        Public Property Version As String
        Public Property Aplicativo As Int32?
        Public Property Proceso As Int64?
    End Class
#End Region

End Class