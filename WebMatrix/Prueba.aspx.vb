Imports System.Net
Imports CoreProject

Public Class Prueba
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim o As New RepositorioDocumentos
        Dim idTrabajo As Integer = 2177
        Dim Servidor As String = "codc01"
        Dim unidad As String = "Marketing"
        Dim JBI As String = "15-055349"
        Dim NombreTrabajo As String = "Shopper Blanco"
        Dim proceso As String = "Instrumentos"
        Dim ruta As String = "\\{Servidor}\{Unidad}\Proyectos\Estudios en Proceso\{JBI} {NombreTrabajo}\Instrumentos"
        'Dim ruta As String = "\\LatCoBoF6\OperacionesCuantitativas\{Proceso}\{Unidad}\{JBI}-{NombreTrabajo}\Comunicaciones"
        ruta = obtenerRutasinComodines(ruta, Servidor, unidad, JBI, NombreTrabajo, proceso)

        'Dim lst = o.obtenerDocumentosOtroServidor()

        Using directory As New NetworkConnection("\\" & Servidor, New NetworkCredential("IpsosGroup\Sammy.Ariza", "190SAM490*"))
            lblArchivos.Text = String.Join("<br/>", IO.Directory.GetDirectories(ruta))
        End Using
    End Sub

    Function obtenerRutaSinComodines(ByVal ruta As String, ByVal servidor As String, ByVal unidad As String, ByVal jbi As String, ByVal nombretrabajo As String, ByVal proceso As String) As String
        Dim rutaSinComodines As String = ruta
        rutaSinComodines = rutaSinComodines.Replace("{Servidor}", servidor)
        rutaSinComodines = rutaSinComodines.Replace("{Unidad}", unidad)
        rutaSinComodines = rutaSinComodines.Replace("{JBI}", jbi)
        rutaSinComodines = rutaSinComodines.Replace("{NombreTrabajo}", nombretrabajo)
        rutaSinComodines = rutaSinComodines.Replace("{Proceso}", proceso)
        Return rutaSinComodines
    End Function


End Class