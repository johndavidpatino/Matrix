Imports CoreProject
Imports System.IO

Public Class Tarea1
    Inherits System.Web.UI.Page
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
    Private _idEstado As Int16
    Public Property idEstado() As Int16
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Int16)
            _idEstado = value
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
    Private _idProyecto As Int64
    Public Property idProyecto() As Int64
        Get
            Return _idProyecto
        End Get
        Set(ByVal value As Int64)
            _idProyecto = value
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
        If Request.QueryString("IdProyecto") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdProyecto"), idProyecto)
        End If
        If Not Request.QueryString("IdEstado") Is Nothing Then
            Int16.TryParse(Request.QueryString("IdEstado"), idEstado)
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
        Dim oWE As CORE_WorkflowEstados

        oWT = obtenerWorkFlowXId(idWorkFlow)
        oT = obtenerTrabajoXId(idTrabajo)
        oWE = obtenerEstadoXId(idEstado)

        lblJobBook.Text = oWT.JobBook
        lblTrabajoId.Text = oT.id
        lblTrabajoNombre.Text = oT.NombreTrabajo
        lblTareaNombre.Text = oWT.Tarea

        lblAsunto.Text = "Matrix." & " " & oT.JobBook & " ID: " & oT.id & " " & oT.NombreTrabajo & " - " & lblAsunto.Text.Replace("@1", oWE.Estado)

        lblEstado.Text = oWE.Estado
        lblObservacion.Text = oWT.Observacion

    End Sub
    Sub ajustarCuerpo()


    End Sub
    Function obtenerUsuariosNotificacion(ByVal idEstado As WorkFlow.Estados) As List(Of String)
        Dim oWorkFlow As New WorkFlow
        Dim oWT As CORE_WorkFlow_Trabajos_Get_Result
        oWT = obtenerWorkFlowXId(idWorkFlow)
        If idEstado = WorkFlow.Estados.Devuelta Then
            Return oWorkFlow.obtenerUsuariosNotificacionTareaDevuelta(idTrabajo, oWT.TareaId).Select(Function(x) x.Email).ToList
        Else
            Return oWorkFlow.obtenerUsuariosNotificacionTareas(idTrabajo, oWT.TareaId).Select(Function(x) x.Email).ToList
        End If

    End Function
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)
        Dim contenido As String
        Dim oEnviarCorreo As New EnviarCorreo
        Dim lstUsuariosANotificar As New List(Of String)

        lstUsuariosANotificar = obtenerUsuariosNotificacion(idEstado)

        If lstUsuariosANotificar.Count > 0 Then
            pnlDescripcionSinUsuariosANotificar.Visible = False
        Else
            Dim oUsuarios As New US.Usuarios
            Dim oeUsuarios As US_Usuarios
            pnlDescripcionConUsuariosANotificar.Visible = False
            oeUsuarios = oUsuarios.obtenerUsuarioXId(oUsuarios.DevolverIdXNombreUsuario(Me.User.Identity.Name))
            lstUsuariosANotificar.Add(oeUsuarios.Email)
        End If

        Me.pnlBody.RenderControl(hw)
        contenido = sb.ToString
        contenido = contenido & "<br/>"
        

        oEnviarCorreo.sendMail(lstUsuariosANotificar, Me.lblAsunto.Text, contenido)
    End Sub
#End Region


End Class