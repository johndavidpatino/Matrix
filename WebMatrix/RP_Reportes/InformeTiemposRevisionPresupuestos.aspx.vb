Imports System.IO
Imports WebMatrix.Util
Imports CoreProject

Public Class REP_InformeTiemposRevisionPresupuestos
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim o As New OP_Cuanti
            Me.txtFechaTerminacion.Text = Date.UtcNow.AddHours(-5).Date
            Me.txtFechaInicio.Text = DateAdd(DateInterval.Day, -15, Now.Date)

            InformeListado(txtFechaInicio.Text, txtFechaTerminacion.Text)
            InformeResumen(txtFechaInicio.Text, txtFechaTerminacion.Text)
        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As System.EventArgs) Handles btnExportar.Click
        Me.gvListado.Visible = True
        'Actualiza los datos del gridview
        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        Me.gvListado.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvListado)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=ExportadoRevisionPresupuestos.xls")
        Response.Charset = "UTF-8"

        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvListado.Visible = False

    End Sub

#End Region

#Region "Metodos"
    Sub InformeListado(ByVal fInicio As Date, fFin As Date)
        Dim o As New CoreProject.Reportes.RP_GerOpe
        Me.gvListado.DataSource = o.TiemposRevisionPresupuestosListado(fInicio, fFin, Nothing)
        Me.gvListado.DataBind()
    End Sub
    Sub InformeResumen(ByVal fInicio As Date, fFin As Date)
        Dim o As New CoreProject.Reportes.RP_GerOpe
        Me.gvResumen.DataSource = o.TiemposRevisionPresupuestosResumen(fInicio, fFin, Nothing)
        Me.gvResumen.DataBind()
    End Sub

#End Region


    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        InformeListado(txtFechaInicio.Text, txtFechaTerminacion.Text)
        InformeResumen(txtFechaInicio.Text, txtFechaTerminacion.Text)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
End Class