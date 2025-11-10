Imports CoreProject
Imports System.IO
Imports Utilidades.Utilidades
Imports WebMatrix.EnviarCorreo

Public Class MetodologiaDeCampoMail
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idtrabajo") IsNot Nothing Then
            Dim estudio As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString())
            'CargarElemento(estudio)
            'CargarPresupuestosXEstudio(estudio)
            Dim oTrabajo As New Trabajo
            Dim info = oTrabajo.obtenerXId(estudio)
            Me.lblTrabajo.Text = info.NombreTrabajo
            Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/ES_Estadistica/MetodologiaDeCampo.aspx?idtrabajo=" & estudio
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

        Me.pnlBody.RenderControl(hw)

        Dim oTrabajo As New Trabajo
        Dim estudio As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString())
        Dim info = oTrabajo.obtenerXId(estudio)

        Dim destinatarios As New List(Of String)
        Dim oUsuarios As New US.Usuarios
        If Not info.COE Is Nothing Then
            destinatarios.Add(oUsuarios.UsuarioGet(info.COE).Email)
        End If

        Dim oMetodologias As New MetodologiaOperaciones
        Dim liTrabajos = (From ltraba In oTrabajo.obtenerAllTrabajos
                          From lmetod In oMetodologias.obtenerTodos()
                        Where ltraba.OP_MetodologiaId = lmetod.Id And lmetod.MetGrupoUnidad And ltraba.id = estudio
                        Select ltraba)

        Dim GrupoUnidad As Integer = (From lmetod In oMetodologias.obtenerTodos()
                        Where lmetod.Id = oTrabajo.obtenerXId(estudio).OP_MetodologiaId).FirstOrDefault.MetGrupoUnidad

        Dim listUsuarios = oUsuarios.UsuariosxGrupoUnidadXrol(GrupoUnidad, ListaRoles.GerenteOperaciones)

        For Each li As Usuarios_Result In listUsuarios
            destinatarios.Add(li.Email)
        Next

        Dim c As New CoordinacionCampo
        Dim EstudioList = c.ObtenerMuestraxEstudioList(estudio)
        For Each li As OP_MuestraTrabajos In EstudioList
            If Not li.Coordinador Is Nothing Then
                Dim Coordinador = oUsuarios.UsuarioGet(EstudioList.Item(0).Coordinador)
                destinatarios.Add(Coordinador.Email)
            End If
        Next

        Dim oProyecto As New Proyecto
        destinatarios.Add(oUsuarios.UsuarioGet(oProyecto.obtenerXId(info.ProyectoId).GerenteProyectos).Email)
        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        'destinatarios.Add("John.Patino@ipsos.com")
        'destinatarios.Add("Cesar.Verano@ipsos.com")
        
        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class