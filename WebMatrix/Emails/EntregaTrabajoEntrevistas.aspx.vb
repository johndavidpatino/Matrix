Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class EntregaTrabajoEntrevistas
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim script As String = "setTimeout('window.close();',50)"
        'Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idFicha") IsNot Nothing Then
            Dim estudio As Int64 = Int64.Parse(Request.QueryString("idFicha").ToString())
            Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/OP_Cualitativo/FichaEntrevista.aspx?idtrabajo=" & estudio
            CargarElemento(estudio)
            If Request.QueryString("notclose") Is Nothing Then
                GenerarHtml()
                'ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
            End If
        Else
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal TrabajoId As Long)
        Dim oEntrega As New CoreProject.EntregasTrabajos
        Dim info As PY_Trabajo_Entrega_Cualitativo_Result = oEntrega.DevolverPreEntregaxTrabajoCuali(TrabajoId)
        lblAsunto.Text = "Matrix." & " " & info.JobBook & " ID: " & info.id & " " & info.NombreTrabajo & " - Especificaciones del trabajo"
        lblNombreTrabajo.Text = info.NombreTrabajo
        lblJobBook.Text = info.JobBook
        lblIdTrabajo.Text = info.id
        lblMetodologia.Text = info.MetNombre
        lblFechaInicio.Text = info.FechaTentativaInicioCampo
        lblFechaFin.Text = info.FechaTentativaFinalizacion
        lblUnidad.Text = info.Unidad
        lblMuestra.Text = info.Muestra
        lblBackups.Text = info.Backups
        'lblIncentivoEconomico.Text = info.IncentivoEconomico
        'lblPresupuestoIncentivo.Text = info.PresupuestoIncentivo
        'lblDistribucionIncentivo.Text = info.DistribucionIncentivo
        'lblRegalosCliente.Text = info.RegalosCliente
        'lblCompraIpsos.Text = info.CompraIpsos
        'lblPresupuestoCompra.Text = info.PresupuestoCompra
        'lblDistribucionCompra.Text = info.DistribucionCompra
        'lblExclusionesRestricciones.Text = info.ExclusionesRestricciones
        'lblRecursosPropiedadCliente.Text = info.RecursosPropiedadCliente
        Dim oUsuarios As New US.Usuarios
        lblGerenteProyectos.Text = oUsuarios.UsuarioGet(info.GerenteProyectos).Nombres & " " & oUsuarios.UsuarioGet(info.GerenteProyectos).Apellidos

    End Sub

    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        Dim destinatarios As New List(Of String)
        Dim trabajo As Int64 = Int64.Parse(Request.QueryString("idFicha").ToString())
        Dim oEntrega As New CoreProject.EntregasTrabajos
        Dim info = oEntrega.DevolverPreEntregaxTrabajoCuali(trabajo)
        Dim o As New DestinatariosCorreos
        Dim oUsuarios As New US.Usuarios
        If Boolean.Parse(Request.QueryString("nuevo").ToString()) = False Then
            lblAsunto.Text = lblAsunto.Text & " - Nueva versión"
        End If
        destinatarios.Add(oUsuarios.UsuarioGet(info.GerenteProyectos).Email)
        For i As Integer = 0 To o.DestinatariosCreacionyEspecificacionesTrabajo(trabajo).Count - 1
            destinatarios.Add(o.DestinatariosCreacionyEspecificacionesTrabajo(trabajo).Item(i).CORREO)
        Next
        'destinatarios.Add("Sammy.Ariza@ipsos.com")
        'destinatarios.Add("John.Patino@ipsos.com")
        'destinatarios.Add("Cesar.Verano@ipsos.com")

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)

    End Sub
#End Region

End Class