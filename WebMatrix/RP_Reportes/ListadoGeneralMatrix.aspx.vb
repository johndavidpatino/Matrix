Imports System.IO
Imports WebMatrix.Util
Imports CoreProject
Imports ClosedXML
Imports ClosedXML.Excel


Public Class ListadoGeneralPersonasMatrix
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExport)
    End Sub


    Protected Sub btnExportar_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        Exportar()
    End Sub

#End Region

#Region "Metodos"
    Protected Sub Exportar()
        Dim excel As New List(Of Array)
        Dim Titulos As String = "id;TipoIdentificacion;LugarExpedicion;FechaExpedicion;Nombres;Apellidos;Nacionalidad;Sexo;GrupoSanguineo;EstadoCivil;FechaNacimiento;Ciudad;BarrioResidencia;Direccion;Telefono1;Telefono2;Celular;EmailPersonal;NivelEducativo;BU;Area;Sede;Cargo;JefeInmediato;FechaIngreso;TipoContratacion;Empresa;TipoSalario;FormaSalario;SalarioActual;UltimoSalario;FechaUltimoAscenso;UltimoCargo;EstadoActual;FechaVencimientoContrato;FechaRetiro;Banco;FondoPensiones;FondoCesantias;EPS;CajaCompensacion;ARL;ACTIVO;Contratista;Usuario;EmailUsuario;HEADCOUNT;MotivoRetiro"
        Dim DynamicColNames() As String
        Dim lstCambios As List(Of TH_PersonasGET_Result)
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("ListadoPersonal")

        DynamicColNames = Titulos.Split(CChar(";"))
        excel.Add(DynamicColNames)
        Dim o As New RegistroPersonas
        lstCambios = o.ListadoPersonasGet(Nothing, Nothing)
        For Each x In lstCambios
            excel.Add((x.id & ";" & x.TipoIdentificacion & ";" & x.LugarExpedicion & ";" & x.FechaExpedicion & ";" & x.Nombres & ";" & x.Apellidos & ";" & x.Nacionalidad & ";" & x.Sexo & ";" & x.GrupoSanguineo & ";" & x.EstadoCivil & ";" & x.FechaNacimiento & ";" & x.Ciudad & ";" & x.BarrioResidencia & ";" & x.Direccion & ";" & x.Telefono1 & ";" & x.Telefono2 & ";" & x.Celular & ";" & x.EmailPersonal & ";" & x.NivelEducativo & ";" & x.BU & ";" & x.Area & ";" & x.Sede & ";" & x.Cargo & ";" & x.JefeInmediato & ";" & x.FechaIngreso & ";" & x.TipoContratacion & ";" & x.Empresa & ";" & x.TipoSalario & ";" & x.FormaSalario & ";" & x.SalarioActual & ";" & x.UltimoSalario & ";" & x.FechaUltimoAscenso & ";" & x.UltimoCargo & ";" & x.EstadoActual & ";" & x.FechaVencimientoContrato & ";" & x.FechaRetiro & ";" & x.Banco & ";" & x.FondoPensiones & ";" & x.FondoCesantias & ";" & x.EPS & ";" & x.CajaCompensacion & ";" & x.ARL & ";" & x.ACTIVO & ";" & x.Contratista & ";" & x.Usuario & ";" & x.EmailUsuario & ";" & x.HEADCOUNT & ";" & x.MotivoRetiro).Split(CChar(";")).ToArray())
        Next
        worksheet.Cell("A1").Value = excel
        Crearexcel(workbook, "ListadoPersonasMatrix -" & Now.Year & "-" & Now.Month & "-" & Now.Day)
    End Sub
    Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)

            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub


#End Region

    Private Sub ListadoGeneralPersonasMatrix_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(143, UsuarioID) = False Then
            Response.Redirect("../TH_TalentoHumano/Default.aspx")
        End If
    End Sub
End Class