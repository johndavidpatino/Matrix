Imports InfosoftGlobal
Imports CoreProject
Public Class PropuestasEstadoUnidad
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        Dim WUnidad As Integer
        Dim WNUnidad As String
        Dim WNUsuario As String
        Dim WParametroUnidad As String

        Dim UnidadAdapter As New GerencialTableAdapters.MBO_ObtenerUnidadesUsuarioTableAdapter
        Dim UnidadDataTable As New Gerencial.MBO_ObtenerUnidadesUsuarioDataTable
        Dim UnidadRow As Gerencial.MBO_ObtenerUnidadesUsuarioRow
        UnidadDataTable = UnidadAdapter.GetData(UsuarioID)

        WParametroUnidad = ""
        For Each UnidadRow In UnidadDataTable.Rows
            WUnidad = CInt(UnidadRow.IdUnidad)
            WNUnidad = UnidadRow.Unidad
            WNUsuario = UnidadRow.Usuario
            WParametroUnidad = Trim(UnidadRow.GrupoUnidad)
        Next


        Dim WPorcentaje As Decimal
        Dim WMaxpropuestas As Integer
        Dim CreadasEnviadasAdapter As New GerencialTableAdapters.MBO_PropuestasCreadasEnviadasSinAnuncioActualizarTableAdapter
        Dim CreadasEnviadasDataTable As New Gerencial.MBO_PropuestasCreadasEnviadasSinAnuncioActualizarDataTable
        Dim CreadasEnviadasRow As Gerencial.MBO_PropuestasCreadasEnviadasSinAnuncioActualizarRow
        CreadasEnviadasDataTable = CreadasEnviadasAdapter.GetData(WParametroUnidad)  'Lee solo la unidad del usuario registrado

        'CALCULAR MAXIMO DE PROPUESTAS
        For Each CreadasEnviadasRow In CreadasEnviadasDataTable.Rows
            If WMaxpropuestas < CreadasEnviadasRow.PropuestasEnGestion Then
                WMaxpropuestas = CreadasEnviadasRow.PropuestasEnGestion
            End If
        Next

        Dim xmlPropuestas As New StringBuilder()

        xmlPropuestas.Append("<chart caption='Propuestas creadas / enviadas' showvalues='1' yAxisMaxValue='" & Str(WMaxpropuestas) & "' numdivlines='20' xAxisName='Unidades' yAxisName='Cantidad' >")
        xmlPropuestas.Append("<categories>")
        For Each CreadasEnviadasRow In CreadasEnviadasDataTable.Rows
            xmlPropuestas.Append("<category label='" & CreadasEnviadasRow.GrupoUnidad & "'/>")
        Next
        xmlPropuestas.Append("</categories>")

        xmlPropuestas.Append("<dataset seriesName='Creadas y enviadas' color='00FF00'>")
        For Each CreadasEnviadasRow In CreadasEnviadasDataTable.Rows
            xmlPropuestas.Append("<set value='" & Str(CreadasEnviadasRow.PropuestasEnGestion) & "' color='00FF00' />")
        Next
        xmlPropuestas.Append("</dataset>")

        xmlPropuestas.Append("<dataset seriesName='Por actualizar' color='FF0000'>")
        For Each CreadasEnviadasRow In CreadasEnviadasDataTable.Rows
            WPorcentaje = Format(CreadasEnviadasRow.PropuestasPorActualizar / CreadasEnviadasRow.PropuestasEnGestion * 100, "##0.00")
            xmlPropuestas.Append("<set value='" & Str(CreadasEnviadasRow.PropuestasPorActualizar) & "' tooltext= '" & Str(CreadasEnviadasRow.PropuestasPorActualizar) & "{br}" & Str(WPorcentaje) & "%" & "' color='FF0000' />")
        Next
        xmlPropuestas.Append("</dataset>")

        xmlPropuestas.Append("</chart>")

        CreadasEnviadasUnidad.Text = FusionCharts.RenderChart("../FusionCharts/MSColumn3D.swf", "", xmlPropuestas.ToString(), "chartid1", "500px", "400px", False, True)
        xmlPropuestas.Clear()

        'APROBADAS SIN ANUNCIO
        Dim AprobadasSinAnuncioAdapter As New GerencialTableAdapters.MBO_PropuestasAprobadasSinAnuncioActualizarTableAdapter
        Dim AprobadasSinAnuncioDataTable As New Gerencial.MBO_PropuestasAprobadasSinAnuncioActualizarDataTable
        Dim AprobadasSinAnuncioRow As Gerencial.MBO_PropuestasAprobadasSinAnuncioActualizarRow
        AprobadasSinAnuncioDataTable = AprobadasSinAnuncioAdapter.GetData(WParametroUnidad)

        xmlPropuestas.Append("<chart caption='Propuestas Aprobadas sin anuncio' showvalues='1' yAxisMaxValue='" & Str(WMaxpropuestas) & "' numdivlines='20' xAxisName='Unidades' yAxisName='Cantidad' >")
        xmlPropuestas.Append("<categories>")
        For Each AprobadasSinAnuncioRow In AprobadasSinAnuncioDataTable.Rows
            xmlPropuestas.Append("<category label='" & AprobadasSinAnuncioRow.GrupoUnidad & "'/>")
        Next
        xmlPropuestas.Append("</categories>")

        xmlPropuestas.Append("<dataset seriesName='Aprobadas sin anuncio' color='00FF00'>")
        For Each AprobadasSinAnuncioRow In AprobadasSinAnuncioDataTable.Rows
            xmlPropuestas.Append("<set value='" & Str(AprobadasSinAnuncioRow.PropuestasAprobadas) & "' color='00FF00' />")
        Next
        xmlPropuestas.Append("</dataset>")

        xmlPropuestas.Append("<dataset seriesName='Por actualizar' color='FF0000'>")
        For Each AprobadasSinAnuncioRow In AprobadasSinAnuncioDataTable.Rows
            WPorcentaje = Format(AprobadasSinAnuncioRow.PropuestasPorActualizar / AprobadasSinAnuncioRow.PropuestasAprobadas * 100, "##0.00")
            xmlPropuestas.Append("<set value='" & Str(AprobadasSinAnuncioRow.PropuestasPorActualizar) & "' tooltext= '" & Str(AprobadasSinAnuncioRow.PropuestasPorActualizar) & "{br}" & Str(WPorcentaje) & "%" & "' color='FF0000' />")
        Next
        xmlPropuestas.Append("</dataset>")

        xmlPropuestas.Append("</chart>")

        AprobadasSinAnuncioUnidad.Text = FusionCharts.RenderChart("../FusionCharts/MSColumn3D.swf", "", xmlPropuestas.ToString(), "chartid2", "500px", "400px", False, True)
        xmlPropuestas.Clear()

        'CREADAS ENVIADAS POR GC
        Dim WGerente As String
        Dim CreadasEnviadasGCAdapter As New GerencialTableAdapters.MBO_PropuestasCreadasEnviadasSinAnuncioGCTableAdapter
        Dim CreadasEnviadasGCDataTable As New Gerencial.MBO_PropuestasCreadasEnviadasSinAnuncioGCDataTable
        Dim CreadasEnviadasGCRow As Gerencial.MBO_PropuestasCreadasEnviadasSinAnuncioGCRow
        CreadasEnviadasGCDataTable = CreadasEnviadasGCAdapter.GetData(WParametroUnidad)

        'CALCULAR MAXIMO DE PROPUESTAS
        WMaxpropuestas = 0
        For Each CreadasEnviadasGCRow In CreadasEnviadasGCDataTable.Rows
            If WMaxpropuestas < CreadasEnviadasGCRow.PropuestasEnGestion Then
                WMaxpropuestas = CreadasEnviadasGCRow.PropuestasEnGestion
            End If
        Next

        xmlPropuestas.Append("<chart caption='Creadas / enviadas por Gerente de cuentas' showvalues='1' yAxisMaxValue='" & Str(WMaxpropuestas) & "' numdivlines='20' xAxisName='Cantidad' yAxisName='Gerentes' >")
        xmlPropuestas.Append("<categories>")
        For Each CreadasEnviadasGCRow In CreadasEnviadasGCDataTable.Rows
            WGerente = CreadasEnviadasGCRow.Nombres & " " & CreadasEnviadasGCRow.Apellidos
            xmlPropuestas.Append("<category label='" & WGerente & "'/>")
        Next
        xmlPropuestas.Append("</categories>")

        xmlPropuestas.Append("<dataset seriesName='Creadas y enviadas' color='00FF00'>")
        For Each CreadasEnviadasGCRow In CreadasEnviadasGCDataTable.Rows
            xmlPropuestas.Append("<set value='" & Str(CreadasEnviadasGCRow.PropuestasEnGestion) & "' color='00FF00' />")
        Next
        xmlPropuestas.Append("</dataset>")

        xmlPropuestas.Append("<dataset seriesName='Por actualizar' color='FF0000'>")
        For Each CreadasEnviadasGCRow In CreadasEnviadasGCDataTable.Rows
            WPorcentaje = Format(CreadasEnviadasGCRow.WPorcentaje, "##0.00")
            xmlPropuestas.Append("<set value='" & Str(CreadasEnviadasGCRow.PropuestasPorActualizar) & "' tooltext= '" & Str(CreadasEnviadasGCRow.PropuestasPorActualizar) & "{br}" & Str(WPorcentaje) & "%" & "' color='FF0000' />")
        Next
        xmlPropuestas.Append("</dataset>")

        xmlPropuestas.Append("</chart>")

        CreadasEnviadasGCUnidad.Text = FusionCharts.RenderChart("../FusionCharts/MSBar3D.swf", "", xmlPropuestas.ToString(), "chartid3", "1100px", "800px", False, True)
        xmlPropuestas.Clear()

    End Sub


End Class