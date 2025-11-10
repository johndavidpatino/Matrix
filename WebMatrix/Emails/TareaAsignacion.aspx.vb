Imports System.IO
Imports CoreProject

Public Class TareaAsignacion
    Inherits System.Web.UI.Page

#Region "Enum"
    Enum TipoAsignacion
        Asignada = 1
        Desasignada = 2
    End Enum
#End Region
#Region "Propiedades"

    Private _idTrabajo As Int64
    Public Property idTrabajo() As Int64
        Get
            Return _idTrabajo
        End Get
        Set(ByVal value As Int64)
            _idTrabajo = value
        End Set
    End Property
    Private _idTipoAsignacion As TipoAsignacion
    Public Property idTipoAsignacion() As TipoAsignacion
        Get
            Return _idTipoAsignacion
        End Get
        Set(ByVal value As TipoAsignacion)
            _idTipoAsignacion = value
        End Set
    End Property
    Private _idWorkFlow As Int64
    Public Property idWorkFlow() As Int64
        Get
            Return _idWorkFlow
        End Get
        Set(ByVal value As Int64)
            _idWorkFlow = value
        End Set
    End Property
    Private _idUsuario As Int64
    Public Property idUsuario() As Int64
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Int64)
            _idUsuario = value
        End Set
    End Property

#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("IdTrabajo") Is Nothing Then
            Int64.TryParse(Request.QueryString("IdTrabajo"), idTrabajo)
        End If
        If Not Request.QueryString("IdWorkFlow") Is Nothing Then
            Int64.TryParse(Request.QueryString("IdWorkFlow"), idWorkFlow)
        End If
        If Not Request.QueryString("IdTipoAsignacion") Is Nothing Then
            Int16.TryParse(Request.QueryString("IdTipoAsignacion"), idTipoAsignacion)
        End If
        If Not Request.QueryString("IdUsuarioNotificar") Is Nothing Then
            Int64.TryParse(Request.QueryString("IdUsuarioNotificar"), idUsuario)
        End If
        parametrizarMensaje()

        Dim page As Page = DirectCast(Context.Handler, Page)
        GenerarHtml()
    End Sub
#End Region

#Region "Metodos"
    Function obtenerEstadoXId(ByVal id As Int16) As CORE_WorkflowEstados
        Dim oWorkFlowEstados As New WorkFlowEstados
        Return oWorkFlowEstados.obtenerXId(id)
    End Function
    Function obtenerTrabajoXId(ByVal id As Int64) As PY_Trabajo_Get_Result
        Dim oPY_Trabajos_Get_Result As New Trabajo
        Return oPY_Trabajos_Get_Result.DevolverxID(id)
    End Function
    Function obtenerWorkFlowXId(ByVal id As Int64) As CORE_WorkFlow_Trabajos_Get_Result
        Dim o As New WorkFlow
        Return o.obtenerXId(id)
    End Function
    Sub parametrizarMensaje()
        Dim oWT As CORE_WorkFlow_Trabajos_Get_Result
        Dim oT As PY_Trabajo_Get_Result

        oWT = obtenerWorkFlowXId(idWorkFlow)
        oT = obtenerTrabajoXId(idTrabajo)

        lblJobBook.Text = oT.JobBook
        lblTrabajoId.Text = oT.id
        lblTrabajoNombre.Text = oT.NombreTrabajo
        lblTareaNombre.Text = oWT.Tarea

        lblAsunto.Text = "Matrix." & " " & oT.JobBook & " ID: " & oT.id & " " & oT.NombreTrabajo & " - " & lblAsunto.Text.Replace("@1", [Enum].GetName(idTipoAsignacion.GetType, idTipoAsignacion))

        If idTipoAsignacion = TipoAsignacion.Asignada Then
            lblCuerpo2Notificacion.Text = "Esta tarea le ha sido asignada."
        Else
            lblCuerpo2Notificacion.Text = "La tarea habia sido inicialmente asignada a usted, pero se revoco esta asignación."
        End If
    End Sub
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)
        Dim lstUsuariosNotificar As New List(Of String)
        Dim contenido As String
        Dim oEnviarCorreo As New EnviarCorreo
        lstUsuariosNotificar.Add(correoUsuarioNotificar(idUsuario))
        Me.pnlBody.RenderControl(hw)
        contenido = sb.ToString
        contenido = contenido & "<br/>"
        
        oEnviarCorreo.sendMail(lstUsuariosNotificar, Me.lblAsunto.Text, contenido)
    End Sub
    Function correoUsuarioNotificar(ByVal idUsuario As Int64) As String
        Dim oUsuarios As New US.Usuarios
        Return oUsuarios.UsuarioGet(idUsuario).Email
    End Function
#End Region

End Class