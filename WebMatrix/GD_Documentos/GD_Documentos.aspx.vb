Imports CoreProject
Imports WebMatrix.Util
Imports System.Web.Configuration

Public Class GD_Documentos
    Inherits System.Web.UI.Page

#Region "Enumeradores"
    Enum TiposContenedores
        Trabajo = 1
    End Enum
    Enum TiposAcciones
        Editar = 1
        Consulta = 2
    End Enum
#End Region
#Region "Propiedades"

    Private _IdContenedor As Int64
    Public Property IdContenedor() As Int64
        Get
            Return _IdContenedor
        End Get
        Set(ByVal value As Int64)
            _IdContenedor = value
        End Set
    End Property

    Private _IdDocumento As Int64
    Public Property IdDocumento() As Int64
        Get
            Return _IdDocumento
        End Get
        Set(ByVal value As Int64)
            _IdDocumento = value
        End Set
    End Property

    Private _TipoContenedor As TiposContenedores
    Public Property TipoContenedor() As TiposContenedores
        Get
            Return _TipoContenedor
        End Get
        Set(ByVal value As TiposContenedores)
            _TipoContenedor = value
        End Set
    End Property

    Private _IdUsuario As Int64
    Public Property IdUsuario() As Int64
        Get
            Return _IdUsuario
        End Get
        Set(ByVal value As Int64)
            _IdUsuario = value
        End Set
    End Property

    Private _URLRetorno As Int16
    Public Property URLRetorno() As Int16
        Get
            Return _URLRetorno
        End Get
        Set(ByVal value As Int16)
            _URLRetorno = value
        End Set
    End Property


    Private _TipoAccion As TiposAcciones
    Public Property TipoAccion() As TiposAcciones
        Get
            Return _TipoAccion
        End Get
        Set(ByVal value As TiposAcciones)
            _TipoAccion = value
        End Set
    End Property

    Private _IdWorkFlow As Int64
    Public Property IdWorkFlow() As Int64
        Get
            Return _IdWorkFlow
        End Get
        Set(ByVal value As Int64)
            _IdWorkFlow = value
        End Set
    End Property


#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        IdUsuario = Session("IdUsuario")

        If Request.QueryString("IdContenedor") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdContenedor"), IdContenedor)
        End If

        If Request.QueryString("URLRetorno") IsNot Nothing Then
            Long.TryParse(Request.QueryString("URLRetorno"), URLRetorno)
        End If

        If Request.QueryString("TipoAccion") IsNot Nothing Then
            Long.TryParse(Request.QueryString("TipoAccion"), TipoAccion)
        End If

        If Request.QueryString("IdDocumento") IsNot Nothing Then
            Long.TryParse(Request.QueryString("IdDocumento"), IdDocumento)
        End If

        If Request.QueryString("TipoContenedor") IsNot Nothing Then
            Integer.TryParse(Request.QueryString("TipoContenedor"), TipoContenedor)
        End If

        If Request.QueryString("IdWorkFlow") IsNot Nothing Then
            Integer.TryParse(Request.QueryString("IdWorkFlow"), IdWorkFlow)
        End If

        If Not IsPostBack Then
            CargarDocumentos()
            lbtnVolver.PostBackUrl = Request.UrlReferrer.PathAndQuery
            If TipoAccion > 0 Then
                If TipoAccion = TiposAcciones.Consulta Then
                    accordion1.Visible = "False"
                Else
                    ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                End If
            End If
        End If


    End Sub
    Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGrabar.Click
        Try
            If IsValid Then
                Grabar()
                CargarDocumentos()
                ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
                log(0, 2)
            End If
        Catch ex As Exception
            ShowNotification("Ha ocurrido un error al intentar ingresar el registro - " & ex.Message, ShowNotifications.ErrorNotification)
        End Try

    End Sub
    Private Sub gvDocumentosCargados_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDocumentosCargados.PageIndexChanging
        gvDocumentosCargados.PageIndex = e.NewPageIndex
        CargarDocumentos()
    End Sub
    Private Sub gvDocumentosCargados_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDocumentosCargados.RowCommand
        If e.CommandName = "Descargar" Then
            Dim oRepositorioDocumentos As New RepositorioDocumentos
            Dim oeRepositorioDocumentos As New GD_RepositorioDocumentos
            oeRepositorioDocumentos = oRepositorioDocumentos.obtenerDocumentosXId(gvDocumentosCargados.DataKeys(CInt(e.CommandArgument))("IdDocumentoRepositorio"))
            descargarArchivos(oeRepositorioDocumentos.Url)
            log(0, 5)
        End If
    End Sub


#End Region
#Region "Metodos"
    Sub CargarDocumentos()
        Dim o As New RepositorioDocumentos
        gvDocumentosCargados.DataSource = o.obtenerDocumentosXIdContenedorXIdDocumento(IdContenedor, IdDocumento)
        gvDocumentosCargados.DataBind()
    End Sub
    Sub Grabar()
        Dim o As New RepositorioDocumentos
        Dim guid As String = System.Guid.NewGuid.ToString
        GrabarArchivo(guid.ToString)
        o.Grabar(txtNombre.Text, ObtenerURLArchivo() & "\" & guid.ToString & "-" & ufArchivo.FileName, IdDocumento, Nothing, DateTime.UtcNow.AddHours(-5), txtComentarios.Text, IdUsuario, IdContenedor)
        If IdDocumento = 13 Then
            Try
                Dim oEnviarCorreo As New EnviarCorreo
                oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/CircularCargada.aspx?idTrabajo=" & IdContenedor)
            Catch ex As Exception
            End Try
        End If

    End Sub
    Function ObtenerURLArchivo() As String
        Dim Ruta As String = ""
        Dim oeMaestroDocumentos As GD_GD_MaestroDocumentos_Get2_Result
        Dim o As New GD.GD_Procedimientos
        oeMaestroDocumentos = o.obtenerDocumentoMaestroXId(IdDocumento)
        If TipoContenedor = TiposContenedores.Trabajo Then

            Dim oTrabajo As New Trabajo
            Dim oProyecto As New Proyecto
            Dim oEstudio As New Estudio
            Dim oWorkFlow As New WorkFlow
            Dim oHilo As New Hilo

            Dim oeTrabajo As PY_Trabajos_Get_Result
            Dim oeProyecto As PY_Proyectos_Get_Result
            Dim oeEstudio As CU_Estudios
            Dim oeWorkFlow As CORE_WorkFlow_Trabajos_Get_Result
            Dim oeHilo As CORE_Hilos

            oeWorkFlow = oWorkFlow.obtenerXId(IdWorkFlow)
            oeHilo = oHilo.obtenerXId(oeWorkFlow.HiloId)
            oeTrabajo = oTrabajo.obtenerXId(oeHilo.ContenedorId)
            oeProyecto = oProyecto.obtenerXId(oeTrabajo.ProyectoId)
            oeEstudio = oEstudio.ObtenerXID(oeProyecto.EstudioId)


            Ruta = oeEstudio.JobBook & "\" & oeProyecto.JobBook & "\" & oeTrabajo.id & "\" & oeMaestroDocumentos.URL
        Else
            Ruta = oeMaestroDocumentos.URL
        End If
        Return Ruta
    End Function
    Sub GrabarArchivo(ByVal guid As String)
        Dim URL As String
        Dim RutaFisica As String

        ' Obtener la URL relativa al archivo
        URL = ObtenerURLArchivo()

        ' Concatenar la URL base del directorio virtual asegurándote de utilizar las barras adecuadas para web
        Dim urlRelativa As String = "ArchivosCargados/" & URL.TrimStart("\")

        ' Usar Server.MapPath para convertir la ruta relativa al contexto del servidor si es necesario
        RutaFisica = Server.MapPath("~/" & urlRelativa)

        ' Crear el directorio si no existe ya
        IO.Directory.CreateDirectory(RutaFisica)

        ' Guardar el archivo en la ruta construida
        ufArchivo.SaveAs(IO.Path.Combine(RutaFisica, guid.ToString() & "-" & ufArchivo.FileName))
    End Sub
    Sub descargarArchivos(ByVal url As String)
        ' Como estás usando un directorio virtual, simplemente usa la ruta relativa desde la raíz virtual
        Dim urlRelativa As String
        urlRelativa = "ArchivosCargados/" & url.TrimStart("\") ' Asegúrate de usar las barras adecuadas para el entorno web

        ' Procede con la ruta relativa. El servidor web debería manejarla correctamente en el contexto del directorio virtual
        Dim path As New IO.FileInfo(Server.MapPath("~/" & urlRelativa))  ' Usa Server.MapPath con el prefijo "~" si es necesario

        Try
            ' Debug: Opcionalmente imprime la ruta para asegurarte de que es correcta
            Console.WriteLine("Ruta relativa del archivo: " & urlRelativa)

            Response.Clear()
            Response.ClearHeaders()
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & path.Name & """")
            Response.AddHeader("Content-Length", path.Length.ToString())
            Response.ContentType = "application/octet-stream"
            Response.WriteFile(path.FullName)
            Response.End()
        Catch ex As Exception
            ' Manejo de errores con mensaje claro
            Console.WriteLine(ex.Message)
            ShowNotification("No se puede descargar el archivo: " & ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Function obtenerDocumentoMaestroXId(ByVal id As Int64) As GD_GD_MaestroDocumentos_Get2_Result
        Dim o As New GD.GD_Procedimientos
        Return o.obtenerDocumentoMaestroXId(id)
    End Function

    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(35, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub CustomValidator1_ServerValidate(ByVal sender As Object, ByVal e As ServerValidateEventArgs)

        If (ufArchivo.FileBytes.Length > ConfigurationManager.AppSettings("TamanoMaximoCargaArchivos") * 1024) Then ' 1024*KB of file size
            e.IsValid = False
            ShowNotification("El tamaño del archivo supera los limites permitidos, maximo " & ConfigurationManager.AppSettings("TamanoMaximoCargaArchivos") / 1024 & " Mb", ShowNotifications.InfoNotification)
        Else
            e.IsValid = True
        End If
    End Sub
#End Region


End Class