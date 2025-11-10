Imports CoreProject
Imports System.IO
Imports Utilidades.Utilidades
Imports WebMatrix.EnviarCorreo

Public Class RechazarMetodologia
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        Dim IdAprobacionMetodologia, idtrabajo

        If Request.QueryString("idtrabajo") IsNot Nothing Then
            idTrabajo = Convert.ToInt64(Request.QueryString("idtrabajo").ToString)
        Else
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
            Exit Sub
        End If

        If Request.QueryString("IdAprobacionMetodologia") IsNot Nothing Then
            IdAprobacionMetodologia = Convert.ToInt64(Request.QueryString("IdAprobacionMetodologia").ToString)
        Else
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
            Exit Sub
        End If

        Dim oMetodologia As New MetodologiaCampo
        Dim Metodo = oMetodologia.ObtenerAprobacionMetXMetodologia(IdAprobacionMetodologia).FirstOrDefault

        If Metodo Is Nothing Then
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
            Exit Sub
        Else
            Dim oWT As CORE_WorkFlow_Trabajos_Get_Result
            Dim oT As PY_Trabajo_Get_Result
            Dim oUsuario As New US.Usuarios

            oWT = obtenerWorkFlowXId(idWorkFlow)
            oT = obtenerTrabajoXId(idtrabajo)
            Dim usuarioRechazo = oUsuario.obtenerUsuarioXId(Metodo.Usuario)

            lblAsunto.Text = "Matrix." & " " & oT.JobBook & " ID: " & oT.id & " " & oT.NombreTrabajo & " - " & "Metodología Rechazada"
            lblTrabajo.Text = oT.id.ToString
            lblRechazado.Text = usuarioRechazo.Nombres + " " + usuarioRechazo.Apellidos
            lblRazon.Text = Metodo.Observación

            GenerarHtml()
            '    ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)

        End If
    End Sub
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
#Region "Funciones y Metodos"
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
    Sub CargarElemento(ByVal EstudioId As Long)
        Dim oAnuncio As New CoreProject.AnuncioAprobacion
        lblAsunto.Text = lblAsunto.Text

    End Sub
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Me.pnlBody.RenderControl(hw)

        Dim oTrabajo As New Trabajo
        Dim estudio As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString())
        Dim info = oTrabajo.obtenerXId(estudio)

        Dim destinatarios As New List(Of String)
        Dim oUsuarios As New US.Usuarios
        If Not info.COE Is Nothing Then
            destinatarios.Add(oUsuarios.UsuarioGet(info.COE).Email)
        End If

        Dim listUsuarios = oUsuarios.UsuariosxUnidad(40)
        For Each li As Usuarios_Result In listUsuarios
            destinatarios.Add(li.Email)
        Next

        Dim oProyecto As New Proyecto
        destinatarios.Add(oUsuarios.UsuarioGet(oProyecto.obtenerXId(info.ProyectoId).GerenteProyectos).Email)
        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class