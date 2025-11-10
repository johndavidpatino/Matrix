Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class Preguntas
    Inherits System.Web.UI.Page
#Region " Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTipos()
            CargarPreguntas()
        End If
    End Sub

    Protected Sub ddlTipo_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipo.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        Limpiar()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        ddlTipo.Focus()
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            CargarPreguntas()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            GuardarPregunta()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            log(hfIdPregunta.Value, 2)
            Limpiar()
            CargarPreguntas()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idPregunta As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    CargarInfo(idPregunta)
                    log(hfIdPregunta.Value, 3)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try

    End Sub
    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarPreguntas()
    End Sub
#End Region
#Region "Funciones y Metodos"
    Public Sub Limpiar()
        Try
            ddlTipo.SelectedValue = "-1"
            txtPregunta.Text = String.Empty
            hfIdPregunta.Value = String.Empty
            chkActiva.Checked = True
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarTipos()
        Try
            Dim oTipo As New Pregunta
            Dim listatipos = (From ltipo In oTipo.PY_Tipos_Procesos_Get()
                              Select Id = ltipo.IdProceso,
                              Nombre = ltipo.Proceso).OrderBy(Function(t) t.Nombre).ToList()
            ddlTipo.DataSource = listatipos
            ddlTipo.DataValueField = "Id"
            ddlTipo.DataTextField = "Nombre"
            ddlTipo.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarPreguntas()
        Try
            Dim oPregunta As New Pregunta
            Dim listapregunta = (From lpregunta In oPregunta.DevolverTodos
                                 Select Id = lpregunta.IdPregunta,
                                 Pregunta = lpregunta.Pregunta,
                                 Tipo = lpregunta.Proceso,
                                 Activa = lpregunta.Activa).OrderBy(Function(p) p.Tipo)

            If Not String.IsNullOrEmpty(txtBuscar.Text) Then
                gvDatos.DataSource = listapregunta.Where(Function(p) p.Pregunta.ToUpper.Contains(txtBuscar.Text.ToUpper) Or p.Tipo.ToUpper.Contains(txtBuscar.Text.ToUpper)).ToList
            Else
                gvDatos.DataSource = listapregunta.ToList
            End If

            gvDatos.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarInfo(ByVal IdPregunta As Int64)
        Try
            Dim oPregunta As New Pregunta
            Dim info = oPregunta.DevolverxID(IdPregunta)
            ddlTipo.SelectedValue = info.IdProceso
            txtPregunta.Text = info.Pregunta
            chkActiva.Checked = info.Activa
            hfIdPregunta.Value = IdPregunta
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GuardarPregunta()
        Try
            Dim IdPregunta As Int64 = -1
            Dim Pregunta As String
            Dim Tipo As Int64
            Dim Activa As Boolean
            Dim oPregunta As New Pregunta

            If ddlTipo.SelectedValue = "-1" Then
                Throw New Exception("Debe elegir un tipo")
            End If

            Tipo = Int64.Parse(ddlTipo.SelectedValue)

            If String.IsNullOrEmpty(txtPregunta.Text) Then
                Throw New Exception("Debe digitar una pregunta a guardar")
            End If

            Pregunta = txtPregunta.Text
            Activa = chkActiva.Checked

            If Not String.IsNullOrEmpty(hfIdPregunta.Value) Then
                IdPregunta = Int64.Parse(hfIdPregunta.Value)
            End If

            oPregunta.Guardar(IdPregunta, Pregunta, Tipo, Activa)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(25, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region


End Class