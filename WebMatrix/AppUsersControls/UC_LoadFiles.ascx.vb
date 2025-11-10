Public Class UC_LoadFiles
	Inherits System.Web.UI.UserControl
    Public DocumentoId As Int64
    Public ContenedorId As Int64
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not (Session("oContenedorDocumento") Is Nothing) Then
                Dim oContenedor = DirectCast(Session("oContenedorDocumento"), oContenedorDocumento)
                DocumentoId = oContenedor.DocumentoId
                ContenedorId = oContenedor.ContenedorId
            Else
                DocumentoId = 0
                ContenedorId = 0
            End If
            CargarDocumentos()
        End If
    End Sub

    Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGrabar.Click
        Try
            If ufArchivo.HasFile Then
                Dim oContenedor = DirectCast(Session("oContenedorDocumento"), oContenedorDocumento)
                DocumentoId = oContenedor.DocumentoId
                ContenedorId = oContenedor.ContenedorId
                Grabar()
                CargarDocumentos()

                log(0, 2)
            End If
        Catch ex As Exception
            Console.WriteLine("Ha ocurrido un error al intentar ingresar el registro - " & ex.Message, ShowNotifications.ErrorNotification)
        End Try

    End Sub

#Region "Metodos"
    Sub CargarDocumentos()
        If Not (Session("oContenedorDocumento") Is Nothing) Then
            Dim oContenedor = DirectCast(Session("oContenedorDocumento"), oContenedorDocumento)
            DocumentoId = oContenedor.DocumentoId
            ContenedorId = oContenedor.ContenedorId
        Else
            DocumentoId = 0
            ContenedorId = 0
        End If
        Dim o As New CoreProject.RepositorioDocumentos
        gvDocumentosCargados.DataSource = o.obtenerDocumentosXIdContenedorXIdDocumento(ContenedorId, DocumentoId)
        gvDocumentosCargados.DataBind()
    End Sub
    Sub Grabar()
        Dim o As New CoreProject.RepositorioDocumentos
        Dim guid As String = System.Guid.NewGuid.ToString
        GrabarArchivo(guid.ToString)
        o.Grabar(txtNombre.Text, ObtenerURLArchivo() & "\" & guid.ToString & "-" & ufArchivo.FileName, DocumentoId, Nothing, DateTime.UtcNow.AddHours(-5), txtComentarios.Text, Session("IDUsuario").ToString, ContenedorId)
        'If DocumentoId = 13 Then
        '    Try
        '        Dim oEnviarCorreo As New EnviarCorreo
        '        oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/CircularCargada.aspx?idTrabajo=" & IdContenedor)
        '    Catch ex As Exception
        '    End Try
        'End If

    End Sub
    Function ObtenerURLArchivo() As String
        Dim Ruta As String = ""
        Dim oeMaestroDocumentos As CoreProject.GD_GD_MaestroDocumentos_Get2_Result
        Dim o As New CoreProject.GD.GD_Procedimientos
        oeMaestroDocumentos = o.obtenerDocumentoMaestroXId(DocumentoId)
        'If TipoContenedor = TiposContenedores.Trabajo Then

        '    Dim oTrabajo As New Trabajo
        '    Dim oProyecto As New Proyecto
        '    Dim oEstudio As New Estudio
        '    Dim oWorkFlow As New WorkFlow
        '    Dim oHilo As New Hilo

        '    Dim oeTrabajo As PY_Trabajos_Get_Result
        '    Dim oeProyecto As PY_Proyectos_Get_Result
        '    Dim oeEstudio As CU_Estudios
        '    Dim oeWorkFlow As CORE_WorkFlow_Trabajos_Get_Result
        '    Dim oeHilo As CORE_Hilos

        '    oeWorkFlow = oWorkFlow.obtenerXId(IdWorkFlow)
        '    oeHilo = oHilo.obtenerXId(oeWorkFlow.HiloId)
        '    oeTrabajo = oTrabajo.obtenerXId(oeHilo.ContenedorId)
        '    oeProyecto = oProyecto.obtenerXId(oeTrabajo.ProyectoId)
        '    oeEstudio = oEstudio.ObtenerXID(oeProyecto.EstudioId)


        '    Ruta = oeEstudio.JobBook & "\" & oeProyecto.JobBook & "\" & oeTrabajo.id & "\" & oeMaestroDocumentos.URL
        'Else
        Ruta = oeMaestroDocumentos.URL
        'End If
        Ruta = oeMaestroDocumentos.URL
        Return Ruta
    End Function
    Sub GrabarArchivo(ByVal guid As String)
        Dim URL As String
        Dim RutaFisica As String
        URL = ObtenerURLArchivo()

        URL = "\ArchivosCargados\" & URL

        RutaFisica = MapPath(URL)

        IO.Directory.CreateDirectory(RutaFisica)

        ufArchivo.SaveAs(RutaFisica & "\" & guid.ToString & "-" & ufArchivo.FileName)

    End Sub
    Sub descargarArchivos(ByVal url As String)
        Dim urlFija As String
        urlFija = "\ArchivosCargados"
        'HACK Deuda tecnica Aquí se deberia colocar la ruta raíz en un parametro en la base de datos

        urlFija = Server.MapPath(urlFija & "\" & url)

        Dim path As New IO.FileInfo(urlFija)

        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" & path.Name)
        Response.AddHeader("Content-Length", path.Length.ToString())
        Response.ContentType = "application/octet-stream"
        Response.WriteFile(urlFija)
        Response.End()

    End Sub
    Function obtenerDocumentoMaestroXId(ByVal id As Int64) As CoreProject.GD_GD_MaestroDocumentos_Get2_Result
        Dim o As New CoreProject.GD.GD_Procedimientos
        Return o.obtenerDocumentoMaestroXId(id)
    End Function

    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New CoreProject.LogEjecucion
            log.Guardar(35, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub CustomValidator1_ServerValidate(ByVal sender As Object, ByVal e As ServerValidateEventArgs)

        If (ufArchivo.FileBytes.Length > ConfigurationManager.AppSettings("TamanoMaximoCargaArchivos") * 1024) Then ' 1024*KB of file size
            e.IsValid = False
            Console.WriteLine("El tamaño del archivo supera los limites permitidos, maximo " & ConfigurationManager.AppSettings("TamanoMaximoCargaArchivos") / 1024 & " Mb", ShowNotifications.InfoNotification)
        Else
            e.IsValid = True
        End If
    End Sub

    Protected Sub gvDocumentosCargados_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "Descargar" Then
            Dim oRepositorioDocumentos As New CoreProject.RepositorioDocumentos
            Dim oeRepositorioDocumentos As New CoreProject.GD_RepositorioDocumentos
            oeRepositorioDocumentos = oRepositorioDocumentos.obtenerDocumentosXId(gvDocumentosCargados.DataKeys(CInt(e.CommandArgument))("IdDocumentoRepositorio"))
            descargarArchivos(oeRepositorioDocumentos.Url)
            log(0, 5)
        End If
    End Sub
#End Region


End Class