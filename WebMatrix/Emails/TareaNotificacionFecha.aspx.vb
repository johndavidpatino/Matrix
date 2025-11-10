Imports CoreProject
Imports System.IO

Public Class TareaNotificacionFecha
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
    Private _idTarea As Int16
    Public Property idTarea() As Int16
        Get
            Return _idTarea
        End Get
        Set(ByVal value As Int16)
            _idTarea = value
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
#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("IdTrabajo") Is Nothing Then
            Int64.TryParse(Request.QueryString("IdTrabajo"), idTrabajo)
        End If
        If Not Request.QueryString("IdWorkFlow") Is Nothing Then
            Int64.TryParse(Request.QueryString("IdWorkFlow"), idWorkFlow)
        End If
        If Request.QueryString("IdTarea") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdTarea"), idTarea)
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

        lblJobBook.Text = oWT.JobBook
        lblTrabajoId.Text = oT.id
        lblTrabajoNombre.Text = oT.NombreTrabajo
        lblTareaNombre.Text = oWT.Tarea

        lblAsunto.Text = "Matrix." & " " & oT.JobBook & " ID: " & oT.id & " " & oT.NombreTrabajo & " - " & lblAsunto.Text.Replace("@1", "Actualización de fechas")

        lblInicio.Text = If(oWT.FIniP.HasValue, oWT.FIniP, "")
        lblFin.Text = If(oWT.FFinP.HasValue, oWT.FFinP, "")

    End Sub
    Sub ajustarCuerpo()


    End Sub

    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)
        Dim contenido As String
        Dim oEnviarCorreo As New EnviarCorreo
        Dim oWorkFlow As New WorkFlow
        Dim destinatarios As New List(Of String)

        Dim o As New DestinatariosCorreos
        For i As Integer = 0 To oWorkFlow.DestinariosCorreosNotificacionFechas(idTrabajo, idTarea).Count - 1
            destinatarios.Add(oWorkFlow.DestinariosCorreosNotificacionFechas(idTrabajo, idTarea).Item(i).email)
        Next

        If Not destinatarios.Count() = 0 Then

        End If
        Me.pnlBody.RenderControl(hw)
        contenido = sb.ToString
        contenido = contenido & "<br/>"

        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region


End Class