Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios

Public Class ControlCalidadInforme
    Inherits System.Web.UI.Page
#Region " Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("idtrabajo") IsNot Nothing Then
                Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
                hfidtrabajo.Value = idtrabajo
                CargarControlCalidad()
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
            CargarControlCalidad()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            GuardarControlCalidad()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            log(hfIdControl.Value, 2)
            Limpiar()
            CargarControlCalidad()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarControlCalidad()
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idControlCalidad As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    CargarInfo(idControlCalidad)
                    log(hfIdControl.Value, 3)
                Case "Eliminar"
                    Dim idControlCalidad As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Eliminar(idControlCalidad)
                    CargarControlCalidad()
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
    End Sub
    Public Sub CargarControlCalidad()
        Try
            Dim oControlCalidad As New ControlCalidad
            Dim TrabajoId As Int64 = Int64.Parse(hfidtrabajo.Value)
            Dim listaControl = (From lcontrol In oControlCalidad.DevolverxIdTipoProcesoyTrabajo(TrabajoId, TipoProceso.ControlCalidadInforme)
                                Select Id = lcontrol.id,
                                JobBook = lcontrol.JobBook,
                                NombreTrabajo = lcontrol.NombreTrabajo,
                                Evaluador = lcontrol.Evaluador,
                                RoleEvaluador = lcontrol.RolEvaluador,
                                Fecha = lcontrol.Fecha,
                                Analista = lcontrol.Apellidos & " " & lcontrol.Nombres).OrderBy(Function(c) c.Id)

            If Not String.IsNullOrEmpty(txtBuscar.Text) Then
                gvDatos.DataSource = listaControl.Where(Function(c) c.Evaluador.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Analista.ToUpper.Contains(txtBuscar.Text.ToUpper)).ToList
            Else
                gvDatos.DataSource = listaControl.ToList
            End If
            gvDatos.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarGrid()
        Try
            Dim oPregunta As New Pregunta
            Dim listapregunta = (From lpregunta In oPregunta.DevolverxIdTipo(TipoProceso.ControlCalidadInforme)
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

    Private Sub CargarInfo(ByVal idControlCalidad As Int64)
        Try
            Dim oControlCalidad As New ControlCalidad
            Dim infoControl = oControlCalidad.DevolverxID(idControlCalidad)
            txtEvaluadorPor.Text = infoControl.Evaluador
            txtRoleEvaluador.Text = infoControl.RolEvaluador
            txtFecha.Text = infoControl.Fecha
            ddlresponsable.SelectedValue = infoControl.Persona

            Dim oDetalle As New DetalleControlCalidad
            Dim infoDetalle = oDetalle.DevolverxIDControl(idControlCalidad)

            For Each item As GridViewRow In gvPreguntas.Rows
                Dim IdPregunta As Int64 = Int64.Parse(CType(item.Cells(0).FindControl("lblID"), Label).Text)
                For Each fila As PY_DetalleControlCalidad_Get_Result In infoDetalle
                    If fila.IdPregunta = IdPregunta Then
                        If fila.Si Then
                            CType(item.Cells(2).FindControl("rblcumple"), RadioButtonList).SelectedValue = "1"
                        ElseIf Not fila.Si Then
                            CType(item.Cells(2).FindControl("rblcumple"), RadioButtonList).SelectedValue = "0"
                        End If
                        CType(item.Cells(3).FindControl("txtComentario"), TextBox).Text = fila.Comentarios
                    End If
                Next
            Next
            hfIdControl.Value = idControlCalidad
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)



        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GuardarControlCalidad()
        Try
            Dim TrabajoId As Int64 = Int64.Parse(hfidtrabajo.Value)
            Dim Evaluador, RoleEvaluador As String
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

            Dim oControlCalidad As New ControlCalidad
            Dim IdControl As Int64

            If Not String.IsNullOrEmpty(hfIdControl.Value) Then
                IdControl = Int64.Parse(hfIdControl.Value)
            End If
            IdControl = oControlCalidad.Guardar(IdControl, TrabajoId, Evaluador, RoleEvaluador, IdPersona, Fecha, TipoProceso.ControlCalidadInforme)

            Dim oDetalle As New DetalleControlCalidad
            oDetalle.EliminarXControlCalidadID(IdControl)

            For Each item As GridViewRow In gvPreguntas.Rows
                Dim IdDetalle As Int64
                Dim IdPregunta As Int64 = Int64.Parse(CType(item.Cells(0).FindControl("lblID"), Label).Text)
                Dim Radio As RadioButtonList = CType(item.Cells(2).FindControl("rblcumple"), RadioButtonList)
                Dim Cumple As Boolean

                Select Case Radio.SelectedValue
                    Case "0"
                        Cumple = False
                    Case "1"
                        Cumple = True
                End Select

                Dim txtComentario As TextBox = CType(item.Cells(3).FindControl("txtComentario"), TextBox)

                oDetalle.Guardar(IdDetalle, IdControl, IdPregunta, Cumple, txtComentario.Text)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub Eliminar(ByVal idControlCalidad As Int64)
        Try
            Dim oDetalle As New DetalleControlCalidad
            oDetalle.EliminarXControlCalidadID(idControlCalidad)

            Dim oControl As New ControlCalidad
            oControl.Eliminar(idControlCalidad)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(21, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region




End Class