Imports CoreProject
Imports System.IO
Imports CoreProject.OP_Cuanti
Imports CoreProject.OP
Imports WebMatrix.EnviarCorreo

Public Class ObservacionesIPSMail
    Inherits System.Web.UI.Page

#Region "Enumerados"
    Enum ETipo
        Revision = 1
        Adicion = 2
    End Enum
#End Region


#Region "Propiedades"
    Private _revision As OP_IPS_Revision_Get_Result
    Public Property revision() As OP_IPS_Revision_Get_Result
        Get
            Return _revision
        End Get
        Set(ByVal value As OP_IPS_Revision_Get_Result)
            _revision = value
        End Set
    End Property
    Private _tipo As ETipo
    Public Property Tipo() As ETipo
        Get
            Return _tipo
        End Get
        Set(ByVal value As ETipo)
            _tipo = value
        End Set
    End Property

#End Region



#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim idRevision As Int64
        Dim esQueryValid As Boolean = True
        Dim daRevision As New RevisionIPS

        If Request.QueryString("idRevision") Is Nothing Then
            esQueryValid = False
        ElseIf Int64.TryParse(Request.QueryString("idRevision"), idRevision) = False Then
            esQueryValid = False
        End If

        If Request.QueryString("tipo") Is Nothing Then
            esQueryValid = False
        ElseIf Short.TryParse(Request.QueryString("tipo"), Tipo) = False Then
            esQueryValid = False
        End If


        If esQueryValid = False Then
            Exit Sub
        Else
            revision = daRevision.DevolverxID(idRevision)
            GenerarHtml()
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"

    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)
        Dim oUsuarios As New US.Usuarios
        Dim destinatarios As New List(Of String)

        Dim daTrabajo As New Trabajo
        Dim info = daTrabajo.obtenerXId(revision.TrabajoId)


        Select Case Tipo
            Case ETipo.Adicion
                destinatarios.Add(oUsuarios.UsuarioGet(revision.IdUsuarioAsignado).Email)
                Me.lblAsunto.Text = "Matrix: Registro de Observaciones-Trabajo: " & info.id
                Me.lblBodyIPS.Text = "Se han hecho observaciones al trabajo "
                Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/OP_Cuantitativo/IPS.aspx?idtrabajo=" & revision.TrabajoId & "&IdTarea=" & revision.TareaId & "&fromgerencia=1"

            Case ETipo.Revision
                destinatarios.Add(oUsuarios.UsuarioGet(revision.IdUsuarioRegistra).Email)
                Me.lblAsunto.Text = "Matrix: Respuesta a Observaciones-Trabajo: " & info.id
                Me.lblBodyIPS.Text = "Se han revisado las observaciones del trabajo "
                Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/OP_Cuantitativo/IPS.aspx?idtrabajo=" & revision.TrabajoId & "&IdTarea=" & revision.TareaId & "&fromgerencia=1"

        End Select
        'Al COE siempre lo notificamos
        destinatarios.Add(oUsuarios.UsuarioGet(info.COE).Email)

        Dim o As New CT_Tareas
        Dim lstTareas = o.TareasList(revision.TareaId, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault

        If lstTareas.TareaId = 13 Then
            destinatarios.Add("Norma.Piravaguen@ipsos.com")
            destinatarios.Add("Nestor.Espitia@ipsos.com")
            destinatarios.Add("Jesus.Arevalo@ipsos.com")
            destinatarios.Add("Leonardo.Castillo@ipsos.com")
        End If

        Me.lblTrabajo.Text = info.NombreTrabajo

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class