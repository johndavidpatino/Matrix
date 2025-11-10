Imports System.IO
Imports WebMatrix.Util
Imports CoreProject
Imports System.Web.UI.DataVisualization.Charting
Public Class PlaneacionEstudiosPorSalir
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            CargarCiudades()
            CargarGerencias()
            CargarGruposUnidad()
            CargarMetodologias()
        End If
        GraficoEncuestas()

        'GraficoScriptingC()
        'GraficoScriptingP()
        'GraficoCriticaC()
        'GraficoCriticaP()
        'GraficoVerificacionC()
        'GraficoVerificacionP()
        'GraficoCapturaC()
        'GraficoCapturaP()
        'GraficoCodificacionC()
        'GraficoCodificacionP()
        'GraficoProcesamientoC()
        'GraficoProcesamientoP()
    End Sub

#Region "Campo"
    Sub GraficoEncuestas()
        Dim o As New Reportes.AvanceCampo

        Dim Gerencia As Int64? = Nothing
        Dim Unidad As Int64? = Nothing
        Dim Metodologia As Int32? = Nothing
        Dim Ciudad As Int32? = Nothing
        VariablesConsulta(Gerencia, Unidad, Metodologia, Ciudad)
        Me.gvDatos.DataSource = o.ObtenerEstudiosPlaneacion(Gerencia, Unidad, Metodologia, Ciudad)
        Me.gvDatos.DataBind()
    End Sub
    Sub GraficoDetalle()
        Dim o As New Reportes.AvanceCampo
        Dim Gerencia As Int64? = Nothing
        Dim Unidad As Int64? = Nothing
        Dim Metodologia As Int32? = Nothing
        Dim Ciudad As Int32? = Nothing

        VariablesConsulta(Gerencia, Unidad, Metodologia, Ciudad)
        Me.gvDetalle.DataSource = o.ObtenerEstudiosPlaneacionDetalle(Gerencia, Unidad, Metodologia, Ciudad, hfdAno.Value, hfdSemana.Value)
        Me.gvDetalle.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
#End Region


    Sub VariablesConsulta(ByRef Gerencia As Int64?, ByRef Unidad As Int64?, ByRef Metodologia As Int32?, ByRef Ciudad As Int32?)
        If Not (ddlGerencias.SelectedValue = -1) Then Gerencia = ddlGerencias.SelectedValue
        If Not (ddlUnidades.SelectedValue = -1) Then Unidad = ddlUnidades.SelectedValue
        If Not (ddlMetodologia.SelectedValue = -1) Then Metodologia = ddlMetodologia.SelectedValue
        If Not (ddlCiudad.SelectedValue = -1) Then Ciudad = ddlCiudad.SelectedValue
    End Sub

    Sub CargarGruposUnidad()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlUnidades.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(1)
        ddlUnidades.DataValueField = "id"
        ddlUnidades.DataTextField = "GrupoUnidad"
        ddlUnidades.DataBind()
        ddlUnidades.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub

    Sub CargarGerencias()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGerencias.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(2)
        ddlGerencias.DataValueField = "id"
        ddlGerencias.DataTextField = "GrupoUnidad"
        ddlGerencias.DataBind()
        ddlGerencias.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub

    Sub CargarMetodologias()
        Dim oMetodologias As New MetodologiaOperaciones
        ddlMetodologia.DataSource = oMetodologias.obtenerMetodologiasCuanti
        ddlMetodologia.DataValueField = "Id"
        ddlMetodologia.DataTextField = "MetNombre"
        ddlMetodologia.DataBind()
        ddlMetodologia.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub

    Sub CargarCiudades()
        Dim oCiudades As New Reportes.RP_GerOpe
        ddlCiudad.DataSource = oCiudades.ListadoDiezCiudadesPrincipales
        ddlCiudad.DataValueField = "DivMunicipio"
        ddlCiudad.DataTextField = "DivMuniNombre"
        ddlCiudad.DataBind()
        ddlCiudad.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub


    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click

    End Sub

    Private Sub gvDatos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Me.hfdAno.Value = gvDatos.DataKeys(CInt(e.CommandArgument))("Año")
        Me.hfdSemana.Value = gvDatos.DataKeys(CInt(e.CommandArgument))("Semana")
        GraficoDetalle()
    End Sub

    Protected Sub gvDatos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvDatos.SelectedIndexChanged


    End Sub
End Class
