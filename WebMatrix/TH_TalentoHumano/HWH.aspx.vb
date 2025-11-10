Imports CoreProject
Imports WebMatrix.Util

Public Class HWH
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnEnviar_Click(sender As Object, e As EventArgs) Handles btnEnviar.Click
        If txtUsuario.Text.Trim = "" Then
            ShowNotification("El documento del empleado es obligatorio", ShowNotifications.ErrorNotificationLong)
            Exit Sub
        End If
        If txtFecha.Text.Trim = "" Then
            ShowNotification("La fecha en la que se realizá el Teletrabajo es obligatoria", ShowNotifications.ErrorNotificationLong)
            Exit Sub
        End If

        Try
            If validarHWH() Then
                Guardar()
                EnviarEmail(hfId.Value, hfUsuario.Value)
            Else
                txtUsuarioBuscar.Text = txtUsuario.Text
                lblUsuarioBuscar.InnerText = lblUsuario.InnerText

                cargarListado()
            End If
        Catch ex As Exception
            ShowNotification("Ocurrio un error al ejecutar la instrucción - " & ex.Message, ShowNotifications.InfoNotification)
        End Try
    End Sub

    Private Sub txtUsuarioBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtUsuarioBuscar.TextChanged
        lblUsuarioBuscar.InnerText = ""
        BuscarNombre(txtUsuarioBuscar.Text, 2)
        If lblUsuarioBuscar.InnerText <> "" Then
            divFechaListado.Visible = True
            txtFechaIniListado.Text = DateSerial(Year(Date.Now), Month(Date.Now), 1).ToString("dd/MM/yyyy")
            txtFechaFinListado.Text = DateSerial(Year(Date.Now), Month(Date.Now) + 2, 0).ToString("dd/MM/yyyy")
        Else
            divFechaListado.Visible = False
            txtFechaIniListado.Text = ""
            txtFechaFinListado.Text = ""
        End If
    End Sub

    Sub txtUsuario_TextChanged(sender As Object, e As EventArgs) Handles txtUsuario.TextChanged
        BuscarNombre(txtUsuario.Text, 1)
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If txtUsuarioBuscar.Text = "" Then
            ShowNotification("El usuario que realizó el Easy Work es obligatorio", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        If txtFechaIniListado.Text = "" Then
            ShowNotification("La fecha Inicial para la búsqueda de Easy Work, es obligatoria", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        If txtFechaFinListado.Text = "" Then
            ShowNotification("La fecha Final para la búsqueda de Easy Work, es obligatoria", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        If Convert.ToDateTime(txtFechaIniListado.Text) > Convert.ToDateTime(txtFechaFinListado.Text) Then
            ShowNotification("Para hacer una búsqueda de Easy Work, la fecha Inicial debe ser menor o igual a la fecha Final", ShowNotifications.ErrorNotification)
            Exit Sub
        End If

        cargarListado()
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        limpiar()
    End Sub

    Private Sub btnLimpiarBuscar_Click(sender As Object, e As EventArgs) Handles btnLimpiarBuscar.Click
        limpiar()
    End Sub

    Private Sub gvTeleTrabajo_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvTeleTrabajo.RowCommand
        If e.CommandName = "Gestionar" Then
            Dim id = gvTeleTrabajo.DataKeys(CInt(e.CommandArgument))("id")
            Dim teletrabajo As New TeleTrabajoC
            Dim hwh = teletrabajo.BuscarXId(id)

            hfId.Value = id
            hfEstado.Value = hwh.Cod_Estado
            txtUsuarioEdit.Text = hwh.Id_Usuario
            lblUsuarioEdit.InnerText = hwh.Nombre_Usuario
            txtEstadoEdit.Text = hwh.Estado
            txtFechaEdit.Text = hwh.Fecha_Programada

            Select Case hwh.Cod_Estado
                Case 1
                    btnAnular.Visible = True
                Case 2
                    btnAnular.Visible = True
                Case 3
                    btnAnular.Visible = False
                Case Else
                    btnAnular.Visible = False
            End Select

            txtObservacionesEdit.Focus()
        End If
    End Sub

    Private Sub gvTeleTrabajo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvTeleTrabajo.PageIndexChanging
        gvTeleTrabajo.PageIndex = e.NewPageIndex
        cargarListado()
    End Sub

    Private Sub btnAnular_ServerClick(sender As Object, e As EventArgs) Handles btnAnular.ServerClick
        Try
            Dim estado = 4
            Dim teletrabajo As New TeleTrabajoC
            teletrabajo.ActualizarGestion(hfId.Value, estado, txtUsuarioEdit.Text, txtObservacionesEdit.Text)
            ShowNotification("Se anuló correctamente el día de Easy Work", ShowNotifications.InfoNotification)
            cargarListado()
        Catch ex As Exception
            ShowNotification("Se ha presentado un error: " + ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Private Sub btnLimpiarEdit_ServerClick(sender As Object, e As EventArgs) Handles btnLimpiarEdit.ServerClick
        limpiar()
    End Sub

#End Region

#Region "Funciones y Metodos"
    Function validarHWH() As Boolean
        Dim FechaActual = Date.UtcNow.AddHours(-5).AddMonths(1)
        Dim PrimerDiaDelMes = DateSerial(Year(FechaActual), Month(FechaActual), 1)
        Dim UltimoDiaDelMes = DateSerial(Year(FechaActual), Month(FechaActual) + 1, 0)

        Dim teletrabajo As New TeleTrabajoC
        Dim hwh = teletrabajo.BuscarXUsuarioXFecha(txtUsuario.Text, PrimerDiaDelMes, UltimoDiaDelMes)

        If (hwh.Count > 1) Then
            Dim contadorHWH = 0
            For Each itemHWH In hwh
                If itemHWH.Cod_Estado <> 3 And itemHWH.Cod_Estado <> 4 Then
                    contadorHWH = +1
                    If Not validarQuincena(itemHWH.Fecha_Programada, Convert.ToDateTime(txtFecha.Text)) Then
                        Return False
                    End If
                End If
            Next

            If contadorHWH > 1 Then
                ShowNotification("Solo puede tener programado 1 día de Easy Work por Quincena (al mes máximo 2)", ShowNotifications.ErrorNotificationLong)
                Return False
            End If
        End If

        If hwh.Count = 1 Then
            Return validarQuincena(hwh.Item(0).Fecha_Programada, Convert.ToDateTime(txtFecha.Text))
        End If

        Return True
    End Function

    Function validarQuincena(fechaAnterior, fechaNueva) As Boolean
        Dim dia1 = DateTime.Parse(fechaAnterior).Day
        Dim dia2 = DateTime.Parse(fechaNueva).Day
        If (dia1 <= 15 And dia2 <= 15) Or (dia1 > 15 And dia2 > 15) Then
            ShowNotification("Debe seleccionar otra quincena para tomar el Easy Work", ShowNotifications.ErrorNotificationLong)
            Return False
        ElseIf dia1 + 1 = dia2 Or dia2 + 1 = dia1 Then
            ShowNotification("No se puede guardar el Easy Work con días consecutivos", ShowNotifications.ErrorNotificationLong)
            Return False
        Else
            Return True
        End If
    End Function

    Sub Guardar()
        Dim teletrabajo As New TeleTrabajoC
        Dim t = New TH_Teletrabajo
        t.Usuario = txtUsuario.Text
        t.Fecha = txtFecha.Text
        t.Estado = 1
        t.FechaCreacion = Date.UtcNow.AddHours(-5).ToString("dd/MM/yyyy HH:mm:ss")
        t.Observaciones = txtObservaciones.Text

        teletrabajo.Guardar(t)
        ShowNotification("La fecha del Easy Work se guardó correctamente", ShowNotifications.InfoNotification)
        hfUsuario.Value = txtUsuario.Text

        limpiar()
        txtUsuarioBuscar.Text = t.Usuario
        hfId.Value = t.id

        BuscarNombre(txtUsuarioBuscar.Text, 2)
        divFechaListado.Visible = True
        txtFechaIniListado.Text = DateSerial(Year(Date.Now), Month(Date.Now), 1).ToString("dd/MM/yyyy")
        txtFechaFinListado.Text = DateSerial(Year(Date.Now), Month(Date.Now) + 2, 0).ToString("dd/MM/yyyy")

        cargarListado()
    End Sub

    Sub BuscarNombre(ByVal identificacion As String, ByVal control As String)
        Dim o As New RegistroPersonas
        Dim cedula As Int64? = Nothing
        If IsNumeric(identificacion) Then cedula = identificacion
        Dim Persona = o.TH_PersonasGet(cedula, "")
        If Persona.Count <= 0 Then
            ShowNotification("El documento del empleado no existe", ShowNotifications.ErrorNotificationLong)
            formFecha.Enabled = False
            lblUsuario.InnerText = ""
            Exit Sub
        End If

        formFecha.Enabled = True
        Select Case control
            Case 1
                lblUsuario.InnerText = Persona.Item(0).Nombres + " " + Persona.Item(0).Apellidos
            Case 2
                lblUsuarioBuscar.InnerText = Persona.Item(0).Nombres + " " + Persona.Item(0).Apellidos
        End Select

    End Sub

    Sub cargarListado()
        Dim usuario = txtUsuarioBuscar.Text
        Dim teletrabajo As New TeleTrabajoC
        Dim listaTeleTrabajo = teletrabajo.BuscarXUsuario(usuario)

        gvTeleTrabajo.DataSource = listaTeleTrabajo
        gvTeleTrabajo.DataBind()
        gvTeleTrabajo.Visible = True

        Dim fechaIni = txtFechaIniListado.Text
        Dim fechaFin = txtFechaFinListado.Text

        Dim Jefe = teletrabajo.BuscarTeleTrabajosJefeXId(fechaIni, fechaFin, usuario)
        Dim teleTrabajoJefe = AdaptarTeletrabajoAGantt(Jefe)

        If teleTrabajoJefe.Count > 0 Then
            Dim dataGantt = CrearTablaGantt(teleTrabajoJefe)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "cargarGantt", "cargarGantt(" + SerializarAJSON(dataGantt) + ");", True)
        End If

    End Sub

    Sub limpiar()
        txtUsuario.Text = ""
        lblUsuario.InnerText = ""
        txtFecha.Text = ""
        txtObservaciones.Text = ""
        formFecha.Enabled = False

        txtUsuarioBuscar.Text = ""
        lblUsuarioBuscar.InnerText = ""
        divFechaListado.Visible = False
        txtFechaIniListado.Text = ""
        txtFechaFinListado.Text = ""
        gvTeleTrabajo.DataSource = Nothing
        gvTeleTrabajo.DataBind()

        txtUsuarioEdit.Text = ""
        lblUsuarioEdit.InnerText = ""
        txtEstadoEdit.Text = ""
        txtFechaEdit.Text = ""
        txtObservacionesEdit.Text = ""
    End Sub

    Sub EnviarEmail(Id As Long, usuario As String)
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(Id) Then
                Throw New Exception("Debe elegir un Easy Work o guardarlo antes de enviar este correo al Manager")
            End If
            oEnviarCorreo.enviarCorreoNoAuthentication(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/EnvioAManagerHWH.aspx?idHWH=" & Id & "&usuarioHWH=" & usuario)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Public Function AdaptarTeletrabajoAGantt(data As List(Of TH_TeleTrabajoJefeXId_Result)) As List(Of TH_TeleTrabajoAllJefe)
        Dim TeleTrabajoAllJefe As New List(Of TH_TeleTrabajoAllJefe)

        For Each item As TH_TeleTrabajoJefeXId_Result In data
            Dim itemAllJefe As New TH_TeleTrabajoAllJefe
            itemAllJefe.id = item.id
            itemAllJefe.Usuario = item.Usuario
            itemAllJefe.Nombre = item.NombreUsuario
            itemAllJefe.Descripcion = item.Observaciones
            itemAllJefe.Estado = item.NombreEstado
            itemAllJefe.FechaInicio = Convert.ToDateTime(item.Fecha + " 00:00:00").ToString
            itemAllJefe.FechaFinal = Convert.ToDateTime(item.Fecha + " 23:59:59").ToString


            TeleTrabajoAllJefe.Add(itemAllJefe)
        Next

        Return TeleTrabajoAllJefe
    End Function

    Public Function CrearTablaGantt(data As List(Of TH_TeleTrabajoAllJefe)) As Gantt
        Dim Cronograma = New Gantt()
        Dim FechaInicial = Nothing
        Dim FechaFinal = Nothing
        Dim ListaSerie = New List(Of serie)
        Dim dependency = Nothing
        Dim c = 0
        For Each row As TH_TeleTrabajoAllJefe In data
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

#End Region

#Region "Objetos Adicionales"
    Public Class TH_TeleTrabajoAllJefe
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