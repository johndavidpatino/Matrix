Imports CoreProject
Imports WebMatrix.Util

Public Class HWH_Admin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

            If permisos.VerificarPermisoUsuario(150, UsuarioID) = False Then
                Response.Redirect("../Home/Default.aspx")
            End If
        End If
        If Request.QueryString("idHWH") IsNot Nothing And Request.QueryString("idHWH") <> "" Then
            Dim teletrabajo As New TeleTrabajoC
            Dim hwh = teletrabajo.BuscarXId(Request.QueryString("idHWH"))
            If hwh IsNot Nothing Then
                ActivateAccordion(1)
                CargarXId(hwh.id)
            End If
        End If
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If txtFechaIni.Text <> "" And txtFechaIni.Text <> "" Then
            cargarListadoHWH()
        Else
            ShowNotification("Las fechas son obligatorias para la búsqueda de HWH", ShowNotifications.InfoNotificationLong)
        End If
    End Sub

    Sub cargarListadoHWH()
        Dim estado = If(ddlEstados.SelectedValue = "-1", Nothing, ddlEstados.SelectedValue)
        Dim fechaIni = If(txtFechaIni.Text = "", Nothing, txtFechaIni.Text)
        Dim fechaFin = If(txtFechaFin.Text = "", Nothing, txtFechaFin.Text)

        Dim teletrabajo As New TeleTrabajoC
        Dim hwh As New List(Of TH_TeletrabajoGet_Result)

        If estado = 0 Then
            hwh = teletrabajo.BuscarXjefeDirectoXEstadoXFechas(Convert.ToInt64(Session("IDUsuario")), Nothing, fechaIni, fechaFin)
        Else
            hwh = teletrabajo.BuscarXjefeDirectoXEstadoXFechas(Convert.ToInt64(Session("IDUsuario")), estado, fechaIni, fechaFin)
        End If

        If hwh.Count > 0 Then
            gvTeleTrabajo.DataSource = hwh
            gvTeleTrabajo.DataBind()

            pnlTeleTrabajo.Visible = True

            Dim dataJefe = Nothing
            If estado = 0 Then
                dataJefe = teletrabajo.BuscarTeleTrabajosJefeXJefe(fechaIni, fechaFin, Convert.ToInt64(Session("IDUsuario")), Nothing)
            Else
                dataJefe = teletrabajo.BuscarTeleTrabajosJefeXJefe(fechaIni, fechaFin, Convert.ToInt64(Session("IDUsuario")), estado)
            End If

            Dim teleTrabajoJefe = AdaptarTeletrabajoAGantt(dataJefe)

            If teleTrabajoJefe.Count > 0 Then
                Dim dataGantt = CrearTablaGantt(teleTrabajoJefe)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "cargarGantt", "cargarGantt(" + SerializarAJSON(dataGantt) + ");", True)
            End If
        Else
            gvTeleTrabajo.DataSource = Nothing
            gvTeleTrabajo.DataBind()

            pnlTeleTrabajo.Visible = False
            ShowNotification("No se encontraron días de Easy Work", ShowNotifications.InfoNotification)
        End If
    End Sub

    Private Sub gvTeleTrabajo_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvTeleTrabajo.RowCommand
        If e.CommandName = "Gestionar" Then
            Dim id = gvTeleTrabajo.DataKeys(CInt(e.CommandArgument))("id")
            CargarXId(id)
        End If
    End Sub

    Private Sub gvTeleTrabajo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvTeleTrabajo.PageIndexChanging
        gvTeleTrabajo.PageIndex = e.NewPageIndex
        cargarListadoHWH()
    End Sub

    Private Sub CargarXId(id As Long?)
        Dim teletrabajo As New TeleTrabajoC
        Dim hwh = teletrabajo.BuscarXId(id)

        hfId.Value = id
        hfEstado.Value = hwh.Cod_Estado
        txtUsuario.Text = hwh.Id_Usuario
        lblUsuario.InnerText = hwh.Nombre_Usuario
        txtEstado.Text = hwh.Estado
        txtFecha.Text = hwh.Fecha_Programada

        Select Case hwh.Cod_Estado
            Case 1
                btnAprobar.Visible = True
                btnRechazar.Visible = True
                btnAnular.Visible = False
            Case 2
                btnAprobar.Visible = False
                btnRechazar.Visible = False
                btnAnular.Visible = True
            Case 3
                btnAprobar.Visible = False
                btnRechazar.Visible = False
                btnAnular.Visible = True
            Case Else
                btnAprobar.Visible = False
                btnRechazar.Visible = False
                btnAnular.Visible = False
        End Select

        txtObservaciones.Focus()
    End Sub

    Private Sub limpiar()
        gvTeleTrabajo.DataSource = Nothing
        gvTeleTrabajo.DataBind()

        txtUsuario.Text = ""
        lblUsuario.InnerText = ""
        txtEstado.Text = ""
        txtFecha.Text = ""
        txtObservaciones.Text = ""
        pnlTeleTrabajo.Visible = False
    End Sub

    Private Sub btnLimpiar_ServerClick(sender As Object, e As EventArgs) Handles btnLimpiar.ServerClick
        ddlEstados.SelectedValue = 1
        limpiar()
    End Sub

    Private Sub btnLimpiarBuscar_Click(sender As Object, e As EventArgs) Handles btnLimpiarBuscar.Click
        ddlEstados.SelectedValue = 1
        limpiar()
    End Sub

    Private Sub btnAprobar_ServerClick(sender As Object, e As EventArgs) Handles btnAprobar.ServerClick
        Try
            Dim estado = 2
            Dim teletrabajo As New TeleTrabajoC
            teletrabajo.ActualizarGestion(hfId.Value, estado, Session("IDUsuario"), txtObservaciones.Text)
            EnviarEmail(hfId.Value, "Aprobada")
            limpiar()
            ShowNotification("Se aprobó correctamente el día de HWH", ShowNotifications.InfoNotification)
            cargarListadoHWH()
        Catch ex As Exception
            ShowNotification("Se ha presentado un error: " + ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Private Sub btnRechazar_ServerClick(sender As Object, e As EventArgs) Handles btnRechazar.ServerClick
        Try
            Dim estado = 3
            Dim teletrabajo As New TeleTrabajoC
            teletrabajo.ActualizarGestion(hfId.Value, estado, Session("IDUsuario"), txtObservaciones.Text)
            EnviarEmail(hfId.Value, "Rechazada")
            limpiar()
            ShowNotification("Se rechazó correctamente el día de HWH", ShowNotifications.InfoNotification)
            cargarListadoHWH()
        Catch ex As Exception
            ShowNotification("Se ha presentado un error: " + ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Private Sub btnAnular_ServerClick(sender As Object, e As EventArgs) Handles btnAnular.ServerClick
        Try
            Dim estado = 4
            Dim teletrabajo As New TeleTrabajoC
            teletrabajo.ActualizarGestion(hfId.Value, estado, Session("IDUsuario"), txtObservaciones.Text)
            EnviarEmail(hfId.Value, "Anulada")
            limpiar()
            ShowNotification("Se anuló correctamente el día de HWH", ShowNotifications.InfoNotification)
            cargarListadoHWH()
        Catch ex As Exception
            ShowNotification("Se ha presentado un error: " + ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Sub EnviarEmail(Id As Long, Estado As String)
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(Id) Then
                Throw New Exception("Debe elegir un Easy Work para envair correo al Usuario")
            End If
            oEnviarCorreo.enviarCorreoNoAuthentication(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/EnvioAUsuarioHWH.aspx?idHWH=" & Id & "&estadoHWH=" & Estado)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Public Function AdaptarTeletrabajoAGantt(data As List(Of TH_TeleTrabajoJefeXJefe_Result)) As List(Of TH_TeleTrabajoAllArea)
        Dim TeleTrabajoAllArea As New List(Of TH_TeleTrabajoAllArea)

        For Each item As TH_TeleTrabajoJefeXJefe_Result In data
            Dim itemAllArea As New TH_TeleTrabajoAllArea
            itemAllArea.id = item.id
            itemAllArea.Usuario = item.Usuario
            itemAllArea.Nombre = item.NombreUsuario
            itemAllArea.Descripcion = item.Observaciones
            itemAllArea.Estado = item.NombreEstado
            itemAllArea.FechaInicio = Convert.ToDateTime(item.Fecha + " 00:00:00").ToString
            itemAllArea.FechaFinal = Convert.ToDateTime(item.Fecha + " 23:59:59").ToString


            TeleTrabajoAllArea.Add(itemAllArea)
        Next

        Return TeleTrabajoAllArea
    End Function
    Public Function CrearTablaGantt(data As List(Of TH_TeleTrabajoAllArea)) As Gantt
        Dim Cronograma = New Gantt()
        Dim FechaInicial = Nothing
        Dim FechaFinal = Nothing
        Dim ListaSerie = New List(Of serie)
        Dim dependency = Nothing
        Dim c = 0
        For Each row As TH_TeleTrabajoAllArea In data
            c += 1
            If FechaInicial Is Nothing Then
                FechaInicial = row.FechaInicio
            Else
                Dim FechaMenor = Convert.ToDateTime(FechaInicial)
                Dim FechaActual = Convert.ToDateTime(row.FechaInicio)
                If (Not (row.FechaInicio Is Nothing)) Then
                    If FechaActual.Date < FechaMenor.Date Then
                        FechaInicial = FechaActual.ToString("dd/MM/yyyy")
                    End If
                End If
            End If

            If FechaFinal Is Nothing Then
                FechaFinal = row.FechaFinal
            Else
                Dim FechaMayor = Convert.ToDateTime(FechaFinal)
                Dim FechaActual = Convert.ToDateTime(row.FechaFinal)
                If (Not (row.FechaFinal Is Nothing)) Then
                    If FechaActual.Date > FechaMayor.Date Then
                        FechaFinal = FechaActual.ToString("dd/MM/yyyy")
                    End If
                End If
            End If

            Dim S As New serie()
            S.name = row.Nombre
            S.id = row.id
            S.parent = "Listado_TeleTrabajo"
            S.fstart = Format(Convert.ToDateTime(row.FechaInicio), "dd/MM/yyyy")
            S.fend = Format(Convert.ToDateTime(row.FechaFinal), "dd/MM/yyyy")
            S.owner = row.Usuario
            S.estado = row.Estado
            S.descripcion = row.Descripcion
            If (Not (S.fstart Is Nothing)) Then
                ListaSerie.Add(S)
            End If
        Next

        Cronograma.FechaIni = Format(Convert.ToDateTime(FechaInicial), "dd/MM/yyyy")
        Cronograma.FechaFin = Format(Convert.ToDateTime(FechaFinal), "dd/MM/yyyy")
        Cronograma.series = ListaSerie

        Return Cronograma
    End Function
    Shared Function SerializarAJSON(Of T)(ByVal objeto As T) As String
        Dim JSON As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer
        Return JSON.Serialize(objeto)
    End Function

#Region "Objetos Adicionales"
    Public Class TH_TeleTrabajoAllArea
        Public Property id As String
        Public Property Usuario As String
        Public Property Nombre As String
        Public Property Descripcion As String
        Public Property Estado As String
        Public Property FechaInicio As String
        Public Property FechaFinal As String

    End Class

    Public Class Gantt
        Private _FechaIni As String
        Private _FechaFin As String
        Private _series As List(Of serie)
        Public Property FechaIni() As String
            Get
                Return _FechaIni
            End Get
            Set(ByVal value As String)
                _FechaIni = value
            End Set
        End Property
        Public Property FechaFin() As String
            Get
                Return _FechaFin
            End Get
            Set(ByVal value As String)
                _FechaFin = value
            End Set
        End Property
        Public Property series() As List(Of serie)
            Get
                Return _series
            End Get
            Set(ByVal value As List(Of serie))
                _series = value
            End Set
        End Property
    End Class

    Public Class serie
        Private _name As String
        Private _id As String
        Private _parent As String
        Private _fstart As String
        Private _fend As String
        Private _owner As String
        Private _estado As String
        Private _descripcion As String

        Public Property name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property
        Public Property id() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property
        Public Property parent() As String
            Get
                Return _parent
            End Get
            Set(ByVal value As String)
                _parent = value
            End Set
        End Property
        Public Property fstart() As String
            Get
                Return _fstart
            End Get
            Set(ByVal value As String)
                _fstart = value
            End Set
        End Property
        Public Property fend() As String
            Get
                Return _fend
            End Get
            Set(ByVal value As String)
                _fend = value
            End Set
        End Property
        Public Property owner() As String
            Get
                Return _owner
            End Get
            Set(ByVal value As String)
                _owner = value
            End Set
        End Property

        Public Property estado() As String
            Get
                Return _estado
            End Get
            Set(ByVal value As String)
                _estado = value
            End Set
        End Property

        Public Property descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property
    End Class
#End Region
End Class