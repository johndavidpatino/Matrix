Imports CoreProject
Imports System.Globalization

Public Class ActasComite
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

    Private _seg_id As String
    Public Property SeguimientoId As String
        Get
            Return ViewState("_seg_id")
        End Get
        Set(value As String)
            ViewState("_seg_id") = value
        End Set
    End Property

    Public Function ObtenerUsuarios() As List(Of ObtenerUsuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Try
            Return Data.ObtenerUsuarios
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

    Public Function ObtenerEstadoSeguimiento() As List(Of SG_EstadoSeguimiento_Get_Result)
        Dim Data As New SG.SeguimientosActaComite
        Try
            Return Data.ObtenerEstadoSeguimiento
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerTipoReunion() As List(Of SG_TipoReunion_Get_Result)
        Dim Data As New SG.ActasComite
        Try
            Return Data.ObtenerTipoReunion
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()
    End Sub

    Public Sub CargarActaId()
        Dim _sgActas As New SG.ActasComite
        Dim list As List(Of SG_ActaComite_Get_Result)
        If Request.QueryString("ActaId") IsNot Nothing Then
            btnGuardar.Text = "Editar"
            ActaId = Request.QueryString("ActaId")
            list = _sgActas.ObtenerActasComite(ActaId)
            For Each l In list
                txtConclusiones.Content = l.Conclusiones
                txtDescripcion.Content = l.Descripcion
                txtNoActa.Text = l.NoActa
                ddlLider.SelectedValue = l.UsuarioLidera
                ddlTipoReunion.SelectedValue = l.TipoReunionId
                ddlUnidad.SelectedValue = l.UnidadId
                txtOrdenDia.Text = l.OrdenDia
            Next
            DvParticipantes.Visible = True
            DvSeguimientos.Visible = True
            gvParticipantes.DataSource = CargarParticipanteActasId(ActaId)
            gvParticipantes.DataBind()
            gvTareas.DataSource = CargarSeguimientos(ActaId)
            gvTareas.DataBind()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarCombo(ddlLider, "id", "Nombres", ObtenerUsuarios())
            CargarCombo(ddResponsable, "id", "Nombres", ObtenerUsuarios())
            CargarCombo(ddlTipoReunion, "id", "TipoReunion", ObtenerTipoReunion())
            CargarCombo(ddlUnidad, "id", "Unidad", ObtenerUnidad())
            CargarCombo(ddlEstadoSeg, "id", "EstadoSeguimiento", ObtenerEstadoSeguimiento())
            CargarActaId()
        End If
    End Sub

    Sub Limpiar()
        txtNoActa.Text = String.Empty
    End Sub
    Function Validar() As Boolean
        Dim Val = True
        If txtNoActa.Text = String.Empty Then
            lblResult.Text = "Escriba el Numero del acta"
            Val = False
        End If
        Return Val
    End Function

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim List As New List(Of Usuarios_Result)
        If Validar() = True Then
            Dim Data As New SG.ActasComite
            If btnGuardar.Text = "Guardar" Then
                ActaId = Data.GuardarActaComite(ddlUnidad.SelectedValue, txtNoActa.Text, ddlTipoReunion.SelectedValue, 1, 1, txtOrdenDia.Text, txtConclusiones.Content, txtDescripcion.Content, Session("UsuarioId"), ddlLider.SelectedValue)
                lblResult.Text = "Registro guardado correctamente"
                Response.Redirect("~/SG_Actas/ActasComite.aspx?ActaId=" & ActaId.ToString)
                Limpiar()
            Else
                Data.EditarActaComite(ActaId, ddlUnidad.SelectedValue, txtNoActa.Text, ddlTipoReunion.SelectedValue, 1, 1, txtOrdenDia.Text, txtConclusiones.Content, txtDescripcion.Content, Session("UsuarioId"), ddlLider.SelectedValue)
                lblResult.Text = "Registro EDITADO correctamente"
                Response.Redirect("~/SG_Actas/ActasComite.aspx?ActaId=" & ActaId.ToString)
                Limpiar()
            End If
        End If
    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        SelUser.CargarGrid(1, txtQuery.Text)
    End Sub

    Public Function CargarParticipanteActasId(ByVal ActaId As Integer) As List(Of SG_AsistenteActaComite_Get_ActaId_Result)
        Dim _sgPActas As New SG.AsistenteActa
        Dim list As List(Of SG_AsistenteActaComite_Get_ActaId_Result)
        list = _sgPActas.ConsultarAsistenteActaXActaId(ActaId)
        Return list
    End Function

    Protected Sub btnAddAsistentes_Click(sender As Object, e As EventArgs) Handles btnAddAsistentes.Click
        Dim List As List(Of Usuarios_Result)
        List = Session("Usuarios")
        For Each a As Usuarios_Result In List
            Dim DataIA As New SG.AsistenteActa
            Try
                DataIA.GuardarAsistenteActa(a.id, ActaId)
            Catch ex As Exception
                Throw ex
            End Try
        Next
        DvParticipantes.Visible = True
        gvParticipantes.DataSource = CargarParticipanteActasId(ActaId)
        gvParticipantes.DataBind()
    End Sub

    Protected Sub gvParticipantes_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvParticipantes.RowCommand
        Try
            Select Case e.CommandName
                Case "eliminar"
                    Dim PA As New SG.AsistenteActa
                    Dim Id As Int32 = Int32.Parse(gvParticipantes.DataKeys(CInt(e.CommandArgument))("Id"))
                    ActaId = gvParticipantes.DataKeys(CInt(e.CommandArgument))("ActaId")
                    PA.EliminarAsistenteActa(Id)
                    gvParticipantes.DataSource = CargarParticipanteActasId(ActaId)
                    gvParticipantes.DataBind()
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function CargarSeguimientos(ByVal ActaId As Integer) As List(Of SG_SeguimientosActaComite_Get_Result)
        Dim _sgActas As New SG.SeguimientosActaComite
        Dim list As List(Of SG_SeguimientosActaComite_Get_Result)
        list = _sgActas.ObtenerSeguimientos(ActaId)
        Return list
    End Function

    Protected Sub gvTareas_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTareas.RowCommand
        Dim PA As New SG.SeguimientosActaComite
        Try
            Select Case e.CommandName
                Case "eliminar"
                    Dim Id As Int32 = Int32.Parse(gvTareas.DataKeys(CInt(e.CommandArgument))("Id"))
                    ActaId = gvTareas.DataKeys(CInt(e.CommandArgument))("ActaId")
                    PA.EliminarSeguimiento(Id)
                    gvTareas.DataSource = CargarSeguimientos(ActaId)
                    gvTareas.DataBind()
                Case "Editar"
                    SeguimientoId = Int32.Parse(gvTareas.DataKeys(CInt(e.CommandArgument))("Id"))
                    ActaId = gvTareas.DataKeys(CInt(e.CommandArgument))("ActaId")
                    txtAccion.Text = gvTareas.DataKeys(CInt(e.CommandArgument))("Accion").ToString
                    ddResponsable.SelectedValue = gvTareas.DataKeys(CInt(e.CommandArgument))("ResponsableId")
                    Dim fi = CDate(gvTareas.DataKeys(CInt(e.CommandArgument))("FechaInicio"))
                    Dim fl = CDate(gvTareas.DataKeys(CInt(e.CommandArgument))("FechaCompromiso"))
                    Dim fc = CDate(gvTareas.DataKeys(CInt(e.CommandArgument))("FechaCierre"))
                    ddlEstadoSeg.SelectedValue = gvTareas.DataKeys(CInt(e.CommandArgument))("EstadoId")
                    calFechaInicio.Text = fi.Month & "/" & fi.Day & "/" & fi.Year
                    calFechaCompromiso.Text = fl.Month & "/" & fl.Day & "/" & fl.Year
                    calFechaCierre.Text = fc.Month & "/" & fc.Day & "/" & fc.Year
                    btnAddTarea.Text = "Editar seguimiento"
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnAddTarea_Click(sender As Object, e As EventArgs) Handles btnAddTarea.Click
        If btnAddTarea.Text = "Editar seguimiento" Then
            Dim SG As New SG.SeguimientosActaComite
            SG.EditarSeguimiento(SeguimientoId, ActaId, txtAccion.Text, ddResponsable.SelectedValue, DateTime.ParseExact(calFechaInicio.Text, "M/d/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(calFechaCompromiso.Text, "M/d/yyyy", CultureInfo.InvariantCulture), ddlEstadoSeg.SelectedValue, DateTime.ParseExact(calFechaCierre.Text, "M/d/yyyy", CultureInfo.InvariantCulture))
            gvTareas.DataSource = CargarSeguimientos(ActaId)
            gvTareas.DataBind()
            btnAddTarea.Text = "Nuevo seguimiento"
        Else
            Try
                Dim SG As New SG.SeguimientosActaComite
                SG.GuardarSeguimiento(ActaId, txtAccion.Text, ddResponsable.SelectedValue, DateTime.ParseExact(calFechaInicio.Text, "M/d/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(calFechaCompromiso.Text, "M/d/yyyy", CultureInfo.InvariantCulture), ddlEstadoSeg.SelectedValue, DateTime.ParseExact(calFechaCierre.Text, "M/d/yyyy", CultureInfo.InvariantCulture))
                gvTareas.DataSource = CargarSeguimientos(ActaId)
                gvTareas.DataBind()
                btnAddTarea.Text = "Nuevo seguimiento"
            Catch ex As Exception
                Throw ex
            End Try

        End If
    End Sub
End Class