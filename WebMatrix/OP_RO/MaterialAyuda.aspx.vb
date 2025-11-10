Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios

Public Class MaterialAyuda
    Inherits System.Web.UI.Page

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("idtrabajo") IsNot Nothing Then
                If Request.QueryString("fromgerencia") IsNot Nothing Then
                    hfGerentePY.Value = True
                Else
                    hfGerentePY.Value = False
                    Me.gvRevision.Columns(6).Visible = False
                    Me.gvEjecucion.Columns(6).Visible = False
                    Me.btnRechazar.Visible = False
                End If
                Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
                hfidtrabajo.Value = idtrabajo
                CargarInfo()
                CargarGrid()
            Else
                Response.Redirect("~/ES_Estadistica/Trabajos.aspx")
            End If
            'hfidtrabajo.Value = 1
            'CargarInfo()
            'CargarGrid()
        End If
    End Sub
    Private Sub gvRevision_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRevision.RowDataBound
        If hfGerentePY.Value = True Then
            Try
                If e.Row.RowType = DataControlRowType.EmptyDataRow Then
                    DirectCast(e.Row.FindControl("ctrl_btnaddempty"), Button).Visible = False
                End If
            Catch ex As Exception
            End Try
            Try
                If e.Row.RowType = DataControlRowType.Footer Then
                    DirectCast(e.Row.FindControl("ctrl_btnaddfooter"), Button).Visible = False
                End If
            Catch ex As Exception
            End Try
            Try
                If e.Row.RowType = DataControlRowType.DataRow Then
                    DirectCast(e.Row.FindControl("txtPreguntaedit"), TextBox).Enabled = False
                End If
                If e.Row.RowType = DataControlRowType.DataRow Then
                    DirectCast(e.Row.FindControl("txtObservacionedit"), TextBox).Enabled = False
                End If
                If e.Row.RowType = DataControlRowType.DataRow Then
                    DirectCast(e.Row.FindControl("txtDescripcionedit"), TextBox).Enabled = False
                End If
            Catch ex As Exception
            End Try
        Else
            Try
                If e.Row.RowType = DataControlRowType.EmptyDataRow Then
                    DirectCast(e.Row.FindControl("txtEspacioEmpty"), TextBox).Enabled = False
                End If
                If e.Row.RowType = DataControlRowType.Footer Then
                    DirectCast(e.Row.FindControl("txtEspaciofooter"), TextBox).Enabled = False
                End If
                Try
                    If e.Row.RowType = DataControlRowType.DataRow Then
                        DirectCast(e.Row.FindControl("txtEspacioedit"), TextBox).Enabled = False
                    End If
                Catch ex As Exception
                End Try
            Catch ex As Exception
            End Try
        End If
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
        Try
            Dim oItemsRevision As New List(Of ItemsRevision)
            If ViewState("ItemsRevision") IsNot Nothing Then
                oItemsRevision = CType(ViewState("ItemsRevision"), List(Of ItemsRevision))
            End If

            Dim fila As GridViewRow = gvRevision.Rows(e.RowIndex)
            Dim Index As Int32 = e.RowIndex
            Dim otxtObservacion As TextBox = CType(fila.FindControl("txtObservacionedit"), TextBox)
            Dim otxtDescripcion As TextBox = CType(fila.FindControl("txtDescripcionedit"), TextBox)
            Dim otxtEspacio As TextBox = CType(fila.FindControl("txtEspacioedit"), TextBox)

            If String.IsNullOrEmpty(otxtObservacion.Text) Then
                Throw New Exception("Debe agregar por lo menos una observación")
            End If

            oItemsRevision(Index).Observacion = otxtObservacion.Text
            oItemsRevision(Index).DescripcionObservacion = otxtDescripcion.Text
            oItemsRevision(Index).RespuestaGerente = otxtEspacio.Text
            ViewState("ItemsRevision") = oItemsRevision
            gvRevision.EditIndex = -1
            CargarGrid()

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub gvRevision_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRevision.RowCommand
        Try
            Select Case e.CommandName
                Case "Eliminar"
                    Dim Index As Int32 = Int32.Parse(e.CommandArgument)
                    Dim oItemsRevision As New List(Of ItemsRevision)
                    If ViewState("ItemsRevision") IsNot Nothing Then
                        oItemsRevision = CType(ViewState("ItemsRevision"), List(Of ItemsRevision))
                    End If
                    oItemsRevision.RemoveAt(Index)
                    ViewState("ItemsRevision") = oItemsRevision
                    CargarGrid()
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    ''Ejecución
    Private Sub gvEjecucion_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEjecucion.RowDataBound
        If hfGerentePY.Value = True Then
            Try
                If e.Row.RowType = DataControlRowType.EmptyDataRow Then
                    DirectCast(e.Row.FindControl("ctrl_btnaddempty"), Button).Visible = False
                End If
            Catch ex As Exception
            End Try
            Try
                If e.Row.RowType = DataControlRowType.Footer Then
                    DirectCast(e.Row.FindControl("ctrl_btnaddfooter"), Button).Visible = False
                End If
            Catch ex As Exception
            End Try
            Try
                If e.Row.RowType = DataControlRowType.DataRow Then
                    DirectCast(e.Row.FindControl("txtPreguntaedit"), TextBox).Enabled = False
                End If
                If e.Row.RowType = DataControlRowType.DataRow Then
                    DirectCast(e.Row.FindControl("txtObservacionedit"), TextBox).Enabled = False
                End If
                If e.Row.RowType = DataControlRowType.DataRow Then
                    DirectCast(e.Row.FindControl("txtDescripcionedit"), TextBox).Enabled = False
                End If
            Catch ex As Exception
            End Try
        Else
            Try
                If e.Row.RowType = DataControlRowType.EmptyDataRow Then
                    DirectCast(e.Row.FindControl("txtEspacioEmpty"), TextBox).Enabled = False
                End If
                If e.Row.RowType = DataControlRowType.Footer Then
                    DirectCast(e.Row.FindControl("txtEspaciofooter"), TextBox).Enabled = False
                End If
                Try
                    If e.Row.RowType = DataControlRowType.DataRow Then
                        DirectCast(e.Row.FindControl("txtEspacioedit"), TextBox).Enabled = False
                    End If
                Catch ex As Exception
                End Try
            Catch ex As Exception
            End Try
        End If
    End Sub
    Protected Sub gvEjecucion_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvEjecucion.RowEditing
        gvEjecucion.EditIndex = e.NewEditIndex
        CargarGrid()
        ChangeTab(1)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub gvEjecucion_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvEjecucion.RowCancelingEdit
        gvEjecucion.EditIndex = -1
        CargarGrid()
        ChangeTab(1)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub gvEjecucion_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvEjecucion.RowUpdating
        Try
            Dim oItemsEjecucion As New List(Of ItemsEjecucion)
            If ViewState("ItemsEjecucion") IsNot Nothing Then
                oItemsEjecucion = CType(ViewState("ItemsEjecucion"), List(Of ItemsEjecucion))
            End If

            Dim fila As GridViewRow = gvEjecucion.Rows(e.RowIndex)
            Dim Index As Int32 = e.RowIndex
            Dim otxtObservacion As TextBox = CType(fila.FindControl("txtObservacionedit"), TextBox)
            Dim otxtDescripcion As TextBox = CType(fila.FindControl("txtDescripcionedit"), TextBox)
            Dim otxtEspacio As TextBox = CType(fila.FindControl("txtEspacioedit"), TextBox)

            If String.IsNullOrEmpty(otxtObservacion.Text) Then
                Throw New Exception("Debe agregar por lo menos una observación")
            End If


            oItemsEjecucion(Index).Observacion = otxtObservacion.Text
            oItemsEjecucion(Index).DescripcionObservacion = otxtDescripcion.Text
            oItemsEjecucion(Index).RespuestaGerente = otxtEspacio.Text
            ViewState("ItemsEjecucion") = oItemsEjecucion
            gvEjecucion.EditIndex = -1
            CargarGrid()
            ChangeTab(1)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub gvEjecucion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEjecucion.RowCommand
        Try
            Select Case e.CommandName
                Case "Eliminar"
                    Dim Index As Int32 = Int32.Parse(e.CommandArgument)
                    Dim oItemsEjecucion As New List(Of ItemsEjecucion)
                    If ViewState("ItemsEjecucion") IsNot Nothing Then
                        oItemsEjecucion = CType(ViewState("ItemsEjecucion"), List(Of ItemsEjecucion))
                    End If
                    oItemsEjecucion.RemoveAt(Index)
                    ViewState("ItemsEjecucion") = oItemsEjecucion
                    CargarGrid()
                    ChangeTab(1)
                    ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            GuardarCuestionario()
            ShowNotification("Información guardada correctamente", ShowNotifications.InfoNotification)
            CargarInfo()
            CargarGrid()

            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub btnNotificar_Click(sender As Object, e As EventArgs) Handles btnNotificar.Click
        EnviarEmail()
    End Sub
    Protected Sub btnRechazar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRechazar.Click
        'REVISION
        Dim index As Integer
        Dim Registro As New RevisionMaterialAyuda
        Dim oItemsRevision As New List(Of ItemsRevision)
        If ViewState("ItemsRevision") IsNot Nothing Then
            oItemsRevision = CType(ViewState("ItemsRevision"), List(Of ItemsRevision))
        End If
        For Each row As GridViewRow In gvRevision.Rows
            Dim blnCheckedRev As Boolean = (DirectCast(row.FindControl("chbRechazarRev"), CheckBox)).Checked
            If blnCheckedRev = True Then
                Registro.Guardar(oItemsRevision(index).ID, oItemsRevision(index).TrabajoId, "R", oItemsRevision(index).DescripcionObservacion, oItemsRevision(index).RespuestaGerente)

            End If
            index = index + 1
        Next


        'EJECUCION
        Dim indexEje As Integer
        Dim RegistroEje As New EjecucionMaterialAyuda
        Dim oItemsEjecucion As New List(Of ItemsEjecucion)
        If ViewState("ItemsEjecucion") IsNot Nothing Then
            oItemsEjecucion = CType(ViewState("ItemsEjecucion"), List(Of ItemsEjecucion))
        End If
        For Each row1 As GridViewRow In gvEjecucion.Rows
            Dim blnCheckedEje As Boolean = (DirectCast(row1.FindControl("chbRechazarEje"), CheckBox)).Checked
            If blnCheckedEje = True Then
                RegistroEje.Guardar(oItemsEjecucion(indexEje).ID, oItemsEjecucion(indexEje).TrabajoId, "R", oItemsEjecucion(indexEje).DescripcionObservacion, oItemsEjecucion(indexEje).RespuestaGerente)
            End If
            indexEje = indexEje + 1
        Next
        ShowNotification("Error Rechazado", ShowNotifications.InfoNotification)
        CargarInfo()
        CargarGrid()
    End Sub
#End Region

#Region "Funciones y Metodos"
    Public Sub CargarGrid()
        Try

            Dim oItemsRevision As New List(Of ItemsRevision)

            If ViewState("ItemsRevision") IsNot Nothing Then
                oItemsRevision = CType(ViewState("ItemsRevision"), List(Of ItemsRevision))
            End If

            gvRevision.DataSource = oItemsRevision.ToList
            gvRevision.DataBind()

            Dim oItemsEjecucion As New List(Of ItemsEjecucion)

            If ViewState("ItemsEjecucion") IsNot Nothing Then
                oItemsEjecucion = CType(ViewState("ItemsEjecucion"), List(Of ItemsEjecucion))
            End If

            gvEjecucion.DataSource = oItemsEjecucion.ToList
            gvEjecucion.DataBind()


        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarInfo()
        Try
            Dim TrabajoID As Int64 = Int64.Parse(hfidtrabajo.Value)

            ''Revisión
            Dim oRevision As New RevisionMaterialAyuda
            Dim olistarevision = oRevision.DevolverxTrabajoID(TrabajoID)
            Dim oItemsrevision As New List(Of ItemsRevision)
            For Each fila As OP_RO_RevisionMaterialAyuda_Get_Result In olistarevision
                oItemsrevision.Add(New ItemsRevision With {.ID = fila.id, .DescripcionObservacion = fila.DescripcionObservacion, .FechaHoraObservacion = fila.FechaHoraObservacion, .FechaHoraRespuesta = fila.FechaHoraRespuesta,
                                                             .Observacion = fila.Observacion, .RespuestaGerente = fila.RespuestaGerenteProyectos, .TrabajoId = fila.TrabajoId})
            Next

            ViewState("ItemsRevision") = oItemsrevision


            ''Ejecución
            Dim oEjecucion As New EjecucionMaterialAyuda
            Dim olistaejecucion = oEjecucion.DevolverxTrabajoID(TrabajoID)
            Dim oItemsEjecucion As New List(Of ItemsEjecucion)
            For Each fila As OP_RO_EjecucionMaterialAyuda_Get_Result In olistaejecucion
                oItemsEjecucion.Add(New ItemsEjecucion With {.ID = fila.id, .DescripcionObservacion = fila.DescripcionObservacion, .FechaHoraObservacion = fila.FechaHoraObservacion, .FechaHoraRespuesta = fila.FechaHoraRespuesta,
                                                             .Observacion = fila.Observacion, .RespuestaGerente = fila.RespuestaGerenteProyectos, .TrabajoId = fila.TrabajoId})
            Next
            ViewState("ItemsEjecucion") = oItemsEjecucion


        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub AgregarRevision(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim TrabajoID As Int64 = Int64.Parse(hfidtrabajo.Value)
            Dim oItemsRevision As New List(Of ItemsRevision)

            If ViewState("ItemsRevision") IsNot Nothing Then
                oItemsRevision = CType(ViewState("ItemsRevision"), List(Of ItemsRevision))
            End If

            If gvRevision.Rows.Count > 0 Then
                ''dropdown del footer del grid
                Dim ofooter As Control = gvRevision.FooterRow
                Dim otxtObservacionfooter As TextBox = CType(ofooter.FindControl("txtObservacionfooter"), TextBox)
                Dim otxtDescripcionfooter As TextBox = CType(ofooter.FindControl("txtDescripcionfooter"), TextBox)
                Dim otxtEspaciofooter As TextBox = CType(ofooter.FindControl("txtEspaciofooter"), TextBox)

                If String.IsNullOrEmpty(otxtObservacionfooter.Text) Then
                    Throw New Exception("Debe agregar por lo menos una observación")
                End If

                oItemsRevision.Add(New ItemsRevision With {.ID = -1, .Observacion = otxtObservacionfooter.Text,
                                                           .DescripcionObservacion = otxtDescripcionfooter.Text, .RespuestaGerente = otxtEspaciofooter.Text, .TrabajoId = TrabajoID})


            Else
                ''dropdown del empty
                Dim oControl As Control = gvRevision.Controls(0).Controls(0)
                Dim otxtObservacionEmpty As TextBox = CType(oControl.FindControl("txtObservacionEmpty"), TextBox)
                Dim otxtDescripcionEmpty As TextBox = CType(oControl.FindControl("txtDescripcionEmpty"), TextBox)
                Dim otxtEspacioEmpty As TextBox = CType(oControl.FindControl("txtEspacioEmpty"), TextBox)

                If String.IsNullOrEmpty(otxtObservacionEmpty.Text) Then
                    Throw New Exception("Debe agregar por lo menos una observación")
                End If

                oItemsRevision.Add(New ItemsRevision With {.ID = -1, .Observacion = otxtObservacionEmpty.Text,
                                                           .DescripcionObservacion = otxtDescripcionEmpty.Text, .RespuestaGerente = otxtEspacioEmpty.Text, .TrabajoId = TrabajoID})

            End If

            ViewState("ItemsRevision") = oItemsRevision
            CargarGrid()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub AgregarEjecucion(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim TrabajoID As Int64 = Int64.Parse(hfidtrabajo.Value)
            Dim oItemsEjecucion As New List(Of ItemsEjecucion)

            If ViewState("ItemsEjecucion") IsNot Nothing Then
                oItemsEjecucion = CType(ViewState("ItemsEjecucion"), List(Of ItemsEjecucion))
            End If

            If gvEjecucion.Rows.Count > 0 Then
                ''dropdown del footer del grid
                Dim ofooter As Control = gvEjecucion.FooterRow
                Dim otxtObservacionfooter As TextBox = CType(ofooter.FindControl("txtObservacionfooter"), TextBox)
                Dim otxtDescripcionfooter As TextBox = CType(ofooter.FindControl("txtDescripcionfooter"), TextBox)
                Dim otxtEspaciofooter As TextBox = CType(ofooter.FindControl("txtEspaciofooter"), TextBox)

                If String.IsNullOrEmpty(otxtObservacionfooter.Text) Then
                    Throw New Exception("Debe agregar por lo menos una observación")
                End If

                oItemsEjecucion.Add(New ItemsEjecucion With {.ID = -1, .Observacion = otxtObservacionfooter.Text,
                                                           .DescripcionObservacion = otxtDescripcionfooter.Text, .RespuestaGerente = otxtEspaciofooter.Text, .TrabajoId = TrabajoID})


            Else
                ''dropdown del empty
                Dim oControl As Control = gvEjecucion.Controls(0).Controls(0)
                Dim otxtObservacionEmpty As TextBox = CType(oControl.FindControl("txtObservacionEmpty"), TextBox)
                Dim otxtDescripcionEmpty As TextBox = CType(oControl.FindControl("txtDescripcionEmpty"), TextBox)
                Dim otxtEspacioEmpty As TextBox = CType(oControl.FindControl("txtEspacioEmpty"), TextBox)

                If String.IsNullOrEmpty(otxtObservacionEmpty.Text) Then
                    Throw New Exception("Debe agregar por lo menos una observación")
                End If

                oItemsEjecucion.Add(New ItemsEjecucion With {.ID = -1, .Observacion = otxtObservacionEmpty.Text,
                                                           .DescripcionObservacion = otxtDescripcionEmpty.Text, .RespuestaGerente = otxtEspacioEmpty.Text, .TrabajoId = TrabajoID})

            End If

            ViewState("ItemsEjecucion") = oItemsEjecucion
            CargarGrid()
            ChangeTab(1)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GuardarCuestionario()
        Try
            Dim TrabajoID As Int64 = Int64.Parse(hfidtrabajo.Value)
            ''Revisión
            Dim oItemsRevision As New List(Of ItemsRevision)
            If ViewState("ItemsRevision") IsNot Nothing Then
                oItemsRevision = CType(ViewState("ItemsRevision"), List(Of ItemsRevision))
            End If

            Dim oRevision As New RevisionMaterialAyuda
            For Each fila As ItemsRevision In oItemsRevision
                oRevision.Guardar(fila.ID, TrabajoID, fila.Observacion, fila.DescripcionObservacion, fila.RespuestaGerente)
            Next

            ''Eliminar los items de revisión borrados de la lista
            Dim IDRevision As New List(Of Int64)
            Dim olistarevision = oRevision.DevolverxTrabajoID(TrabajoID)
            For Each fila As OP_RO_RevisionMaterialAyuda_Get_Result In olistarevision
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

            ''Ejecución
            Dim oItemsEjecucion As New List(Of ItemsEjecucion)
            If ViewState("ItemsEjecucion") IsNot Nothing Then
                oItemsEjecucion = CType(ViewState("ItemsEjecucion"), List(Of ItemsEjecucion))
            End If

            Dim oEjecucion As New EjecucionMaterialAyuda
            For Each fila As ItemsEjecucion In oItemsEjecucion
                oEjecucion.Guardar(fila.ID, TrabajoID, fila.Observacion, fila.DescripcionObservacion, fila.RespuestaGerente)
            Next

            ''Eliminar items borrado de la lista de ejecución
            Dim IDEjecucion As New List(Of Int64)
            Dim olistaejecucion = oEjecucion.DevolverxTrabajoID(TrabajoID)
            For Each fila As OP_RO_EjecucionMaterialAyuda_Get_Result In olistaejecucion
                Dim Sw As Int32 = 0
                For Each item As ItemsEjecucion In oItemsEjecucion
                    If fila.id = item.ID Then
                        Sw = 1
                    ElseIf item.ID = -1 Then
                        Sw = 1
                    End If
                Next
                If Sw = 0 Then
                    IDEjecucion.Add(fila.id)
                End If
            Next

            For Each ID As Int64 In IDEjecucion
                oEjecucion.Eliminar(ID)
            Next


            ViewState("ItemsRevision") = Nothing
            ViewState("ItemsEjecucion") = Nothing
            CargarInfo()
            CargarGrid()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub EnviarEmail()
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(hfidtrabajo.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
            End If
            If Request.QueryString("fromgerencia") IsNot Nothing Then
                oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/ObservacionesRO.aspx?idTrabajo=" & hfidtrabajo.Value & "&tipoRO=MaterialAyuda&fromgerencia=yes")
            Else
                oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/ObservacionesRO.aspx?idTrabajo=" & hfidtrabajo.Value & "&tipoRO=MaterialAyuda")
            End If
            ShowNotification("Notificación enviada correctamente", ShowNotifications.InfoNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
#End Region

#Region "Propiedades"
    <Serializable()>
    Private Class ItemsRevision
        Public Property ID As Int64 = -1
        Public Property TrabajoId As Int64
        Public Property FechaHoraObservacion As DateTime?
        Public Property FechaHoraRespuesta As DateTime?
        Public Property Observacion As String
        Public Property DescripcionObservacion As String
        Public Property RespuestaGerente As String
    End Class
    <Serializable()>
    Private Class ItemsEjecucion
        Public Property ID As Int64 = -1
        Public Property TrabajoId As Int64
        Public Property FechaHoraObservacion As DateTime?
        Public Property FechaHoraRespuesta As DateTime?
        Public Property Observacion As String
        Public Property DescripcionObservacion As String
        Public Property RespuestaGerente As String
    End Class
#End Region


End Class