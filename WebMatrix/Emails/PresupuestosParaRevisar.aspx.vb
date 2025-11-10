Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class PresupuestosParaRevisarMail
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("PropuestaId") IsNot Nothing Then
            Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/CU_Cuentas/RevisionPresupuestos.aspx?Propuestaid=" & Request.QueryString("PropuestaId")
            GenerarHtml()
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        Else
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal EstudioId As Long)
        Dim oAnuncio As New CoreProject.AnuncioAprobacion
        lblAsunto.Text = lblAsunto.Text

    End Sub
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)
        Dim alternativas As New List(Of Integer)
        Dim propuestaId As Int64

        Long.TryParse(Request.QueryString("PropuestaId").ToString(), propuestaId)
        Dim estudio As Int64 = propuestaId

        Dim oUsuarios As New US.Usuarios
        Dim oPresupuesto As New Presupuesto
        Dim oPropuesta As New Propuesta
        Dim destinatarios As New List(Of String)
        Dim oBrief As New Brief
        Dim lstMetodologiasTecnicas As New List(Of CU_Propuesta_Presupuestos_TecnicaMetodologia_Result)

        Dim list = oPresupuesto.DevolverxIdPropuestaParaRevision(estudio)

        Dim infpropu = oPropuesta.DevolverxID(estudio)

        Me.lblTitulo.Text = infpropu.Titulo

        Dim listUsuarios = oUsuarios.UsuariosXrolXPropuesta(ListaRoles.GerenteOperaciones, estudio)

        alternativas = Request.QueryString("alternativas").ToString().Split(",").Select(Function(x) CInt(x)).ToList
        lstMetodologiasTecnicas = oPresupuesto.obtenerMetodologiaTecnicaXAlternativasPropuesta(propuestaId, alternativas)
        lblAsunto.Text &= " - " & String.Join("/", lstMetodologiasTecnicas.GroupBy(Function(x) x.TecNombre).Select(Function(x) x.Key).ToList)
        lblMetodologia.Text = " - " & String.Join("<br/>", lstMetodologiasTecnicas.GroupBy(Function(x) x.MetNombre).Select(Function(x) x.Key).ToList)

        For Each li As Usuarios_Result In listUsuarios
            destinatarios.Add(li.Email)
        Next
        Try
            Me.lblGerenteCuentas.Text = oUsuarios.UsuarioGet(oBrief.DevolverxID(infpropu.Brief).GerenteCuentas).Nombres & " " & oUsuarios.UsuarioGet(oBrief.DevolverxID(infpropu.Brief).GerenteCuentas).Apellidos
        Catch ex As Exception

        End Try
        destinatarios.Add(oUsuarios.UsuarioGet(oBrief.DevolverxID(infpropu.Brief).GerenteCuentas).Email)
        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class