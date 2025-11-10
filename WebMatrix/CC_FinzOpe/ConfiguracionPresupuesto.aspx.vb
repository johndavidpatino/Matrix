Imports CoreProject
Imports WebMatrix.Util
Imports CoreProject.CC_FinanzasOp

Public Class ConfiguracionPresupuesto
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("TrabajoId") IsNot Nothing Then
                hfidtrabajo.Value = Request.QueryString("TrabajoId").ToString
                cargarPreguntas(hfidtrabajo.Value)
            End If
            If Request.QueryString("Tipo") IsNot Nothing Then
                HfidTipo.Value = Request.QueryString("Tipo").ToString

            End If
        End If
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub


    Sub cargarPreguntas(ByVal TrabajoId As Int64)
        Dim op As New CC_FinzOpe
        Me.ddlpregunta.DataSource = op.CC_PreguntasGestionCampoXTrabajo(TrabajoId)
        ddlpregunta.DataValueField = "Pr_Id"
        ddlpregunta.DataTextField = "Pr_Nombre"
        Me.ddlpregunta.DataBind()
    End Sub

    Sub cargarcodigos(ByVal Pr_Id As Int64)
        Dim op As New CC_FinzOpe
        Me.ddlRespuestas.DataSource = op.CC_CodigosPreguntasGestionCampo(Pr_Id)
        ddlRespuestas.DataValueField = "Cod_Codigo"
        ddlRespuestas.DataTextField = "Cod_Descripcion"
        Me.ddlRespuestas.DataBind()
    End Sub

    Sub Condiciones()
        Dim miDataTable As New DataTable
        Dim dRow As DataRow
        miDataTable.Columns.Add("Pr_Id")
        miDataTable.Columns.Add("Pregunta")
        miDataTable.Columns.Add("CodPregunta")
        miDataTable.Columns.Add("Respuesta")
        miDataTable.Columns.Add("CodRespuesta")
        miDataTable.Columns.Add("Condicion")

        For i = 0 To GvPreguntas.Rows.Count - 1
            dRow = miDataTable.NewRow()
            Dim row As GridViewRow = GvPreguntas.Rows(i)
            dRow("Pr_Id") = row.Cells(0).Text
            dRow("Pregunta") = row.Cells(1).Text
            dRow("CodPregunta") = row.Cells(2).Text
            dRow("Respuesta") = row.Cells(3).Text
            dRow("CodRespuesta") = row.Cells(4).Text
            dRow("Condicion") = row.Cells(5).Text
            miDataTable.Rows.Add(dRow)
        Next

        dRow = miDataTable.NewRow()
        dRow("Pr_Id") = Me.ddlpregunta.SelectedValue
        dRow("Pregunta") = Me.ddlpregunta.SelectedItem
        dRow("CodPregunta") = Me.ddlpregunta.SelectedValue
        dRow("Respuesta") = Me.ddlRespuestas.SelectedItem
        dRow("CodRespuesta") = Me.ddlRespuestas.SelectedValue
        dRow("Condicion") = Me.ddlcondicion.SelectedItem

        miDataTable.Rows.Add(dRow)
        GvPreguntas.DataSource = miDataTable
        GvPreguntas.DataBind()
    End Sub

    Private Sub ddlpregunta_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlpregunta.SelectedIndexChanged
        If Me.ddlpregunta.SelectedValue = "-1" Or ddlpregunta.SelectedValue = "" Then
            ddlpregunta.Items.Clear()
        Else
            cargarcodigos(Me.ddlpregunta.SelectedValue)
        End If
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnagregar_Click(sender As Object, e As EventArgs) Handles btnagregar.Click
        If Me.ddlcondicion.SelectedValue = "-1" Or ddlcondicion.SelectedValue = "" Then
            ShowNotification("Debe Seleccionar una condicion", ShowNotifications.InfoNotification)
        Else
            Condiciones()
        End If
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Function guardarpresupuesto(ByVal trabajoid As Int64, ByVal Usuarioid As Int64, ByVal Tipo As Int64, Año As Int64)
        Dim op As New PresupInt
        Dim idnuevotrabajo As Decimal
        idnuevotrabajo = op.GuardarPresupuestoInterno(trabajoid, Usuarioid, Tipo, Año)
        ShowNotification("Presupuesto Creado", ShowNotifications.InfoNotification)
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        Return idnuevotrabajo
    End Function

    Public Function Condiciones(ByVal PresupuestoId As Int64) As Int64
        Dim op As New PresupInt
        Dim info As New CC_CondicionesPresupuesto
        Dim idcondiciona As Int64
        info.PresupuestoId = PresupuestoId
        info.Estado = True
        info.Orden = 1
        info.Tipo = 2
        idcondiciona = op.guardarcondiciones(info)
        Return idcondiciona
    End Function

    Sub Preguntas(ByVal CondicionId As Int64)
        Dim op As New PresupInt
        Dim info As New CC_DetalleCondicionPresupuesto
        For i = 0 To GvPreguntas.Rows.Count - 1
            info.Pr_id = GvPreguntas.Rows(i).Cells(0).Text
            info.Codigo = GvPreguntas.Rows(i).Cells(2).Text
            info.Condicion = GvPreguntas.Rows(i).Cells(4).Text
            info.CondicionId = CondicionId
            op.guardardetallecondicion(info)
        Next

    End Sub

    Protected Sub btnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Dim Presupuestoid As Int64
        Dim condicionid As Int64

        Presupuestoid = guardarpresupuesto(hfidtrabajo.Value, Session("IDUsuario"), HfidTipo.Value, Session("WAÑO"))

        hfidpresupuesto.Value = Presupuestoid
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        condicionid = Condiciones(Presupuestoid)
        Preguntas(condicionid)

    End Sub

End Class