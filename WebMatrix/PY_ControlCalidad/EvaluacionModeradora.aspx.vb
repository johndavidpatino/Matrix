Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios

Public Class EvaluacionModeradora
    Inherits System.Web.UI.Page
#Region " Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("idtrabajo") IsNot Nothing Then
                Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
                hfidtrabajo.Value = idtrabajo
                CargarEvaluacion()
                CargarGrid()

                CargarResponsable()
            Else
                Response.Redirect("~/ES_Estadistica/Trabajos.aspx")
            End If
        End If
    End Sub
    Protected Sub ddlresponsable_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlresponsable.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        Limpiar()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        txtEvaluadorPor.Focus()
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            CargarEvaluacion()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            GuardarEvaluacion()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            log(hfIdControl.Value, 2)
            Limpiar()
            CargarEvaluacion()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarEvaluacion()
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idEvaluacion As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    CargarInfo(idEvaluacion)
                    log(hfIdControl.Value, 3)
                Case "Eliminar"
                    Dim idEvaluacion As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Eliminar(idEvaluacion)
                    CargarEvaluacion()
                    ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
                    log(hfIdControl.Value, 4)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
#End Region
#Region "Funciones y Metodos"
    Public Sub Limpiar()
        hfIdControl.Value = String.Empty
        txtBuscar.Text = String.Empty
        txtEvaluadorPor.Text = String.Empty
        txtFecha.Text = String.Empty
        txtRoleEvaluador.Text = String.Empty
        ddlresponsable.SelectedValue = "-1"
        CargarGrid()
        txtObservacion.Text = String.Empty
    End Sub
    Public Sub CargarEvaluacion()
        Try
            Dim oEvaluacion As New Evaluacion
            Dim TrabajoId As Int64 = Int64.Parse(hfidtrabajo.Value)
            Dim listaEvaluacion = (From lEvaluacion In oEvaluacion.DevolverxIdTipoProcesoyTrabajo(TrabajoId, TipoProceso.EvaluacionModeradora)
                                Select Id = lEvaluacion.id,
                                JobBook = lEvaluacion.JobBook,
                                NombreTrabajo = lEvaluacion.NombreTrabajo,
                                Evaluador = lEvaluacion.Evaluador,
                                RoleEvaluador = lEvaluacion.RolEvaluador,
                                Fecha = lEvaluacion.Fecha,
                                Moderadora = lEvaluacion.Apellidos & " " & lEvaluacion.Nombres,
                                Comentarios = lEvaluacion.Comentarios).OrderBy(Function(c) c.Id)

            If Not String.IsNullOrEmpty(txtBuscar.Text) Then
                gvDatos.DataSource = listaEvaluacion.Where(Function(c) c.Evaluador.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Moderadora.ToUpper.Contains(txtBuscar.Text.ToUpper)).ToList
            Else
                gvDatos.DataSource = listaEvaluacion.ToList
            End If
            gvDatos.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarGrid()
        Try
            Dim oPregunta As New Pregunta
            Dim listapregunta = (From lpregunta In oPregunta.DevolverxIdTipo(TipoProceso.EvaluacionModeradora)
                                            Select Id = lpregunta.IdPregunta,
                                            Pregunta = lpregunta.Pregunta,
                                            Tipo = lpregunta.Proceso,
                                            Activa = lpregunta.Activa).OrderBy(Function(p) p.Id)

            gvPreguntas.DataSource = listapregunta.Where(Function(p) p.Activa = True).ToList
            gvPreguntas.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarResponsable()
        Try
            Dim oPersona As New Personas
            Dim listapersonas = (From lpersona In oPersona.DevolverTodos()
                              Select Id = lpersona.id,
                              Nombre = lpersona.Apellidos & " " & lpersona.Nombres).OrderBy(Function(p) p.Nombre)
            ddlresponsable.DataSource = listapersonas.ToList()
            ddlresponsable.DataValueField = "Id"
            ddlresponsable.DataTextField = "Nombre"
            ddlresponsable.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CargarInfo(ByVal idEvaluacion As Int64)
        Try
            Dim oEvaluacion As New Evaluacion
            Dim infoControl = oEvaluacion.DevolverxID(idEvaluacion)
            txtEvaluadorPor.Text = infoControl.Evaluador
            txtRoleEvaluador.Text = infoControl.RolEvaluador
            txtFecha.Text = infoControl.Fecha
            ddlresponsable.SelectedValue = infoControl.Persona
            txtObservacion.Text = infoControl.Comentarios

            Dim oDetalle As New DetalleEvaluacion
            Dim infoDetalle = oDetalle.DevolverxIDEvaluacion(idEvaluacion)

            For Each item As GridViewRow In gvPreguntas.Rows
                Dim IdPregunta As Int64 = Int64.Parse(CType(item.Cells(0).FindControl("lblID"), Label).Text)
                For Each fila As PY_DetalleEvaluacion_Get_Result In infoDetalle
                    If fila.IdPregunta = IdPregunta Then
                        Select Case fila.Calificacion
                            Case 1
                                CType(item.Cells(2).FindControl("rblcalificacion"), RadioButtonList).SelectedValue = "1"
                            Case 2
                                CType(item.Cells(2).FindControl("rblcalificacion"), RadioButtonList).SelectedValue = "2"
                            Case 3
                                CType(item.Cells(2).FindControl("rblcalificacion"), RadioButtonList).SelectedValue = "3"
                            Case 4
                                CType(item.Cells(2).FindControl("rblcalificacion"), RadioButtonList).SelectedValue = "4"
                            Case 5
                                CType(item.Cells(2).FindControl("rblcalificacion"), RadioButtonList).SelectedValue = "5"
                        End Select
                    End If
                Next
            Next
            hfIdControl.Value = idEvaluacion
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)



        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GuardarEvaluacion()
        Try
            Dim TrabajoId As Int64 = Int64.Parse(hfidtrabajo.Value)
            Dim Evaluador, RoleEvaluador, Comentarios As String
            Dim IdPersona As Int64
            Dim Fecha As Date

            If String.IsNullOrEmpty(txtEvaluadorPor.Text) Then
                Throw New Exception("Digite un Evaluador")
            Else
                Evaluador = txtEvaluadorPor.Text
            End If

            If String.IsNullOrEmpty(txtRoleEvaluador.Text) Then
                Throw New Exception("Digite un Role del Evaluador")
            Else
                RoleEvaluador = txtRoleEvaluador.Text
            End If

            If ddlresponsable.SelectedValue = "-1" Then
                Throw New Exception("Seleccione un Analista")
            Else
                IdPersona = Int64.Parse(ddlresponsable.SelectedValue)
            End If

            If String.IsNullOrEmpty(txtFecha.Text) Then
                Throw New Exception("Seleccione una fecha")
            Else
                Fecha = Date.Parse(txtFecha.Text)
            End If

            Comentarios = txtObservacion.Text

            Dim oEvaluacion As New Evaluacion
            Dim IdEvaluacion As Int64

            If Not String.IsNullOrEmpty(hfIdControl.Value) Then
                IdEvaluacion = Int64.Parse(hfIdControl.Value)
            End If
            IdEvaluacion = oEvaluacion.Guardar(IdEvaluacion, TrabajoId, IdPersona, Evaluador, RoleEvaluador, Fecha, Comentarios, TipoProceso.EvaluacionModeradora)

            Dim oDetalle As New DetalleEvaluacion
            oDetalle.EliminarXEvaluacionID(IdEvaluacion)

            For Each item As GridViewRow In gvPreguntas.Rows
                Dim IdDetalle As Int64
                Dim IdPregunta As Int64 = Int64.Parse(CType(item.Cells(0).FindControl("lblID"), Label).Text)
                Dim Radio As RadioButtonList = CType(item.Cells(2).FindControl("rblcalificacion"), RadioButtonList)
                Dim Calificacion As Integer

                Select Case Radio.SelectedValue
                    Case "1"
                        Calificacion = 1
                    Case "2"
                        Calificacion = 2
                    Case "3"
                        Calificacion = 3
                    Case "4"
                        Calificacion = 4
                    Case "5"
                        Calificacion = 5
                End Select
                oDetalle.Guardar(IdDetalle, IdEvaluacion, IdPregunta, Calificacion)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub Eliminar(ByVal idEvaluacion As Int64)
        Try
            Dim oDetalle As New DetalleEvaluacion
            oDetalle.EliminarXEvaluacionID(idEvaluacion)

            Dim oControl As New Evaluacion
            oControl.Eliminar(idEvaluacion)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(24, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region




End Class