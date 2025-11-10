Imports CoreProject
Imports WebMatrix.Util

Public Class Calendario
    Inherits System.Web.UI.Page

#Region "Eventos"
    Private Sub Calendario_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        If Session("IDUsuario") Is Nothing Then
            Response.Redirect("../Default.aspx")
        Else
            If permisos.VerificarRolUsuario(42, Int64.Parse(Session("IDUsuario").ToString())) = False Then
                Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
            End If
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarCoordinador()
        End If
    End Sub
    Private Sub ddlCoordinador_TextChanged(sender As Object, e As EventArgs) Handles ddlCoordinador.TextChanged
        Dim coordinador = If(ddlCoordinador.SelectedValue = "-1", Nothing, ddlCoordinador.SelectedValue)
        If coordinador = Nothing Then
            EstudiosGerente.Visible = False
            ShowNotification("Debe seleccionar un Coordinador para continuar", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        hfCoordinador.Value = coordinador

        EstudiosGerente.Visible = True
        CargarTrabajos()
    End Sub

    Private Sub gvTrabajos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Ver" Then
            Dim oTrabajo As New Trabajo
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("id"))
            Gantt_Cronograma.Visible = False
            Tabla_Cronograma.Visible = True
            CargarCronograma(hfIdTrabajo.Value)
        End If

    End Sub

    Private Sub li_Gantt_Cronograma_ServerClick(sender As Object, e As EventArgs) Handles li_Gantt_Cronograma.Click
        Gantt_Cronograma.Visible = True
        Tabla_Cronograma.Visible = False
        CargarCronograma(hfIdTrabajo.Value)
    End Sub

    Private Sub li_Tabla_Cronograma_ServerClick(sender As Object, e As EventArgs) Handles li_Tabla_Cronograma.Click
        Gantt_Cronograma.Visible = False
        Tabla_Cronograma.Visible = True
        CargarCronograma(hfIdTrabajo.Value)
    End Sub

    Private Sub gvTrabajos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarTrabajos()
    End Sub

#End Region

#Region "Funciones y Métodos"
    Public Sub CargarCoordinador()
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim ListaUsuarios As New List(Of Usuarios_Result)

        Dim trabajo As New Trabajo
        Dim listaCoordinadoresProyectoCuali = trabajo.ObtenerCoordinadorProyectoCuali()

        ddlCoordinador.DataSource = listaCoordinadoresProyectoCuali
        ddlCoordinador.DataTextField = "Nombre"
        ddlCoordinador.DataValueField = "GerenteProyectos"
        ddlCoordinador.DataBind()
        ddlCoordinador.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))
    End Sub

    Public Sub CargarTrabajos()
        If hfCoordinador.Value = "" Then
            ShowNotification("Debe seleccionar un Coordinador para continuar", ShowNotifications.ErrorNotification)
            Exit Sub
        End If

        Dim trabajo As New Trabajo
        Dim dataCoordinador = trabajo.ObtenerTrabajosxCoordinador(hfCoordinador.Value)

        If dataCoordinador.Count <= 0 Then
            EstudiosGerente.Visible = False
            ShowNotification("Este Coordinador no tiene Trabajos Cualitativos vinculados para ver las Tareas", ShowNotifications.ErrorNotification)
            Exit Sub
        Else
            gvTrabajos.DataSource = dataCoordinador
            gvTrabajos.DataBind()
        End If
    End Sub

    Sub CargarCronograma(IdTrabajo As Integer)
        Dim o As New CT_Tareas
        Dim data = o.TareasList(Nothing, IdTrabajo, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

        If (data.Count <= 0) Then
            Session.Remove("Cronograma")
            pnlCronograma.Visible = False
            ShowNotification("Este Trabajo no tiene Tareas vinculadas para mostrarlas", ShowNotifications.ErrorNotification)
            Exit Sub
        Else
            pnlCronograma.Visible = True
            Session.Add("Cronograma", data)

            If (Gantt_Cronograma.Visible) Then
                Dim dataGantt = CrearTablaGantt(data)
                If (Not dataGantt.FechaIni Is Nothing) And (Not dataGantt.FechaFin Is Nothing) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "cargarGantt", "cargarGantt(" + SerializarAJSON(dataGantt) + ");", True)
                Else
                    ShowNotification("No hay fechas asignadas para ninguna tarea", ShowNotifications.InfoNotification)
                    Gantt_Cronograma.Visible = False
                    Tabla_Cronograma.Visible = True
                End If
            End If
            Me.gvCronograma.DataSource = data
            Me.gvCronograma.DataBind()
        End If
    End Sub
    Public Function CrearTablaGantt(data As List(Of CT_TareasList_Result)) As Gantt
        Dim Cronograma = New Gantt()
        Dim FechaInicial = Nothing
        Dim FechaFinal = Nothing
        Dim ListaSerie = New List(Of serie)
        Dim dependency = Nothing
        Dim c = 0
        For Each row As CT_TareasList_Result In data
            c += 1
            If FechaInicial Is Nothing Then
                FechaInicial = row.FIniP
            Else
                Dim FechaMenor = Convert.ToDateTime(FechaInicial)
                Dim FechaActual = Convert.ToDateTime(row.FIniP)
                If (Not (row.FIniP Is Nothing)) Then
                    If FechaActual.Date < FechaMenor.Date Then
                        FechaInicial = FechaActual.ToString("dd/MM/yyyy")
                    End If
                End If
            End If

            If FechaFinal Is Nothing Then
                FechaFinal = row.FFinP
            Else
                Dim FechaMayor = Convert.ToDateTime(FechaFinal)
                Dim FechaActual = Convert.ToDateTime(row.FFinP)
                If (Not (row.FFinP Is Nothing)) Then
                    If FechaActual.Date > FechaMayor.Date Then
                        FechaFinal = FechaActual.ToString("dd/MM/yyyy")
                    End If
                End If
            End If

            Dim S As New serie()
            S.name = row.TAREA
            S.id = row.TareaId
            S.parent = "cronograma_tareas"
            'S.dependency = dependency
            S.fstart = Format(row.FIniP, "dd/MM/yyyy")
            S.fend = Format(row.FFinP, "dd/MM/yyyy")
            S.owner = row.Responsable
            If (Not (S.fstart Is Nothing)) Then
                ListaSerie.Add(S)
                dependency = S.id
            End If
        Next

        Cronograma.FechaIni = FechaInicial
        Cronograma.FechaFin = FechaFinal
        Cronograma.series = ListaSerie

        Return Cronograma
    End Function
    Shared Function SerializarAJSON(Of T)(ByVal objeto As T) As String
        Dim JSON As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer
        Return JSON.Serialize(objeto)
    End Function

#End Region

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