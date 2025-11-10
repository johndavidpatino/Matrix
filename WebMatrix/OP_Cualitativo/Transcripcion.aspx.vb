Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class Transcripcion
    Inherits System.Web.UI.Page

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("idtrabajo") IsNot Nothing Then
                Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
                hfidtrabajo.Value = idtrabajo
                CargarTranscripciones()
                CargarResponsable()
                CargarTranscriptor()
            Else
                Response.Redirect("~/ES_Estadistica/Trabajos.aspx")
            End If
            'hfidtrabajo.Value = 1
            CargarTranscripciones()
            CargarResponsable()
            CargarTranscriptor()
        End If
    End Sub
    Protected Sub ddltranscriptor_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddltranscriptor.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddlresponsable_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlresponsable.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        Limpiar()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        ddlresponsable.Focus()
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            CargarTranscripciones()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            GuardarTranscripcion()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            Limpiar()
            CargarTranscripciones()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarTranscripciones()
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idTranscripcion As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    CargarInfo(idTranscripcion)
                Case "Eliminar"
                    Dim idTranscripcion As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Eliminar(idTranscripcion)
                    CargarTranscripciones()
                    ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
#End Region

#Region "Funciones y Métodos"
    Public Sub Limpiar()
        hfTranscripcionId.Value = String.Empty
        ddlresponsable.SelectedValue = "-1"
        ddltranscriptor.SelectedValue = "-1"
        txtBuscar.Text = String.Empty
        txtCantidad.Text = String.Empty
        txtFechaEntrega.Text = String.Empty
        txtFechaRequerida.Text = String.Empty
        txtFechaTranscripcion.Text = String.Empty
    End Sub
    Public Sub CargarTranscripciones()
        Try
            Dim oTranscripcion As New Transcripciones
            Dim TrabajoId As Int64 = Int64.Parse(hfidtrabajo.Value)
            Dim listaTranscripciones = (From ltran In oTranscripcion.DevolverXTrabajoId(TrabajoId)
                                        Select Id = ltran.id,
                                        JobBook = ltran.JobBook,
                                        NombreTrabajo = ltran.NombreTrabajo,
                                        NombreResponsable = ltran.NombreResponsable,
                                        NombreTranscriptor = ltran.NombreTranscriptor,
                                        Cantidad = ltran.Cantidad,
                                        FechaEntrega = ltran.FechaEntrega,
                                        FechaRequerida = ltran.FechaRequerida,
                                        FechaTranscripcion = ltran.FechaTranscripcion).OrderBy(Function(t) t.Id)

            If Not String.IsNullOrEmpty(txtBuscar.Text) Then
                gvDatos.DataSource = listaTranscripciones.Where(Function(t) t.NombreResponsable.ToUpper.Contains(txtBuscar.Text.ToUpper) Or t.NombreTranscriptor.ToUpper.Contains(txtBuscar.Text.ToUpper)).ToList()
            Else
                gvDatos.DataSource = listaTranscripciones.ToList()
            End If

            gvDatos.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarResponsable()
        Try
            Dim oPersona As New Personas
            Dim listapersonas = (From lpersona In oPersona.TH_Usuarios_Combo_Get
                              Select Id = lpersona.id,
                              Nombre = lpersona.Nombres).OrderBy(Function(p) p.Nombre)
            ddlresponsable.DataSource = listapersonas.ToList()
            ddlresponsable.DataValueField = "Id"
            ddlresponsable.DataTextField = "Nombre"
            ddlresponsable.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarTranscriptor()
        Try
            Dim oPersona As New Personas
            Dim listapersonas = (From lpersona In oPersona.DevolverTodos()
                              Select Id = lpersona.ID,
                              Nombre = lpersona.Apellidos & " " & lpersona.Nombres).OrderBy(Function(p) p.Nombre)
            ddltranscriptor.DataSource = listapersonas.ToList()
            ddltranscriptor.DataValueField = "Id"
            ddltranscriptor.DataTextField = "Nombre"
            ddltranscriptor.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarInfo(ByVal idTranscripcion As Int64)
        Try
            Dim oTranscripcion As New Transcripciones
            Dim info = oTranscripcion.DevolverxID(idTranscripcion)
            ddlresponsable.SelectedValue = info.Responsable
            ddltranscriptor.SelectedValue = info.Transcriptor
            txtCantidad.Text = info.Cantidad
            txtFechaEntrega.Text = info.FechaEntrega
            txtFechaRequerida.Text = info.FechaRequerida
            If info.FechaTranscripcion IsNot Nothing Then
                txtFechaTranscripcion.Text = info.FechaTranscripcion
            End If
            hfTranscripcionId.Value = idTranscripcion
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GuardarTranscripcion()
        Try
            Dim oTranscripcion As New Transcripciones
            Dim TrabajoId As Int64 = Int64.Parse(hfidtrabajo.Value)
            Dim Responsable, Transcriptor, TranscripcionId As Int64?
            Dim Cantidad As Short?
            Dim FechaEntrega, FechaRequerida, FechaTranscripcion As Date?

            If ddlresponsable.SelectedValue = "-1" Then
                Throw New Exception("Debe Elegir un Responsable")
            Else
                Responsable = Int64.Parse(ddlresponsable.SelectedValue)
            End If

            If ddltranscriptor.SelectedValue = "-1" Then
                Throw New Exception("Debe Elegir un Transcriptor")
            Else
                Transcriptor = Int64.Parse(ddltranscriptor.SelectedValue)
            End If

            If Not String.IsNullOrEmpty(txtCantidad.Text) Then
                Cantidad = Short.Parse(txtCantidad.Text)
            End If

            If String.IsNullOrEmpty(txtFechaEntrega.Text) Then
                Throw New Exception("Debe Ingresar la fecha de entrega")
            Else
                FechaEntrega = Date.Parse(txtFechaEntrega.Text)
            End If

            If String.IsNullOrEmpty(txtFechaRequerida.Text) Then
                Throw New Exception("Debe Ingresar la fecha requerida")
            Else
                FechaRequerida = Date.Parse(txtFechaRequerida.Text)
            End If

            If Not String.IsNullOrEmpty(txtFechaTranscripcion.Text) Then
                FechaTranscripcion = Date.Parse(txtFechaTranscripcion.Text)
            End If

            If Not String.IsNullOrEmpty(hfTranscripcionId.Value) Then
                TranscripcionId = Int64.Parse(hfTranscripcionId.Value)
            End If

            oTranscripcion.Guardar(TranscripcionId, TrabajoId, Responsable, Transcriptor, Cantidad, FechaEntrega, FechaRequerida, FechaTranscripcion)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Eliminar(ByVal idTranscripcion As Int64)
        Try
            Dim oTranscripcion As New Transcripciones
            oTranscripcion.Eliminar(idTranscripcion)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gvDatos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDatos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim FechaRequerida = Convert.ToDateTime(e.Row.Cells(7).Text)
            Dim FechaEntrega = Convert.ToDateTime(e.Row.Cells(6).Text)
            If FechaRequerida < FechaEntrega Then
                e.Row.CssClass = "fueraTiempo"
            End If
        End If
    End Sub
#End Region
End Class