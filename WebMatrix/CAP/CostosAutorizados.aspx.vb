
Imports CoreProject
Imports WebMatrix.Util
Imports ClosedXML.Excel


Public Class AgregarCostosEjecutados
    Inherits System.Web.UI.Page
    Dim _costos As New IQ.ControlCostos
    Dim _Cati As New IQ.Cati
    Dim _Presupuestado As Decimal = 0
    Dim _Autorizado As Decimal = 0
    Dim _Ejecutado As Decimal = 0
    Dim _DiferenciaPA As Decimal = 0
    Dim _Porcentaje1 As Decimal = 0
    Dim _DiferenciaPE As Decimal = 0
    Dim _Porcentaje2 As Decimal = 0
    Dim _Totaldetalle As Decimal = 0
    Dim _TotalProduccion As Decimal = 0
    Dim _DiferenciaPP As Decimal = 0
    Dim _Porcentaje3 As Decimal = 0


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim p As New IQ_Parametros

            p.IdPropuesta = CInt(Request.QueryString("IdPropuesta"))
            p.ParAlternativa = CInt(Request.QueryString("Alternativa"))
            p.ParNacional = CInt(Request.QueryString("Fase"))
            p.MetCodigo = CInt(Request.QueryString("Metodologia"))
            Session("PARAMETROS") = p

            'DIC 26 DE 2013
            Dim PAR As IQ_Parametros
            Dim NumEncuestadores As Decimal
            PAR = _Cati.ObtenerParametros(p)

            Dim DG As New IQ_DatosGeneralesPresupuesto
            DG.IdPropuesta = p.IdPropuesta
            DG.ParAlternativa = p.ParAlternativa
            DG = _Cati.ObtenerDatosGenerales(DG)
            lblDiasCampo.Text = DG.DiasCampo.ToString()
            lblDuracion.Text = PAR.ParTiempoEncuesta.ToString()

            Dim D As IQ_ObtenerDescPresupuesto_Result
            D = _costos.ObtenerDatosPresupuesto(p)
            lblFase.Text = D.FASE.ToUpper()
            lblMetodologia.Text = D.METODOLOGIA.ToUpper()
            lblTecnica.Text = D.TECNICA.ToUpper()
            lblIdPropuesta.Text = p.IdPropuesta.ToString()
            lblProductividad.Text = D.ParProductividad.ToString()
            LblMuestra.Text = D.Muestra.ToString()
            lblContactosNo.Text = D.ParContactosNoEfectivos.ToString()
            lblJobBook.Text = CStr(Request.QueryString("JobBook"))
            lblNombre.Text = CStr(Request.QueryString("Nombre"))

            If Not D.ParProductividad Is Nothing Then
                NumEncuestadores = (D.Muestra / DG.DiasCampo / D.ParProductividad)
                lblNumEncuestadores.Text = NumEncuestadores.ToString("N2")
            End If


            Dim obs As New ObservacionesPresupuestos()
            obs = _costos.ObtenerObservacionesPresupuesto(p)

            lblObserPresup.Text = obs.Observaciones_Presupuesto
            lblObserGenerales.Text = obs.Observaciones_Generales


            Dim preg As IQ_Preguntas
            preg = _costos.ObtenerPreguntasPresupuesto(p)
            If (preg IsNot Nothing) Then
                lblAbiertas.Text = preg.PregAbiertas.ToString()
                lblAbiertasMult.Text = preg.PregAbiertasMultiples.ToString()
                lblCerradas.Text = preg.PregCerradas.ToString()
                lblCerradasMult.Text = preg.PregCerradasMultiples.ToString()
                lblOtros.Text = preg.PregOtras.ToString()
                lblDemograficos.Text = preg.PregDemograficos.ToString()
            End If


            CargarActividades()
            CargarEjecucionCostos(p)

        End If


    End Sub

    Private Sub CargarActividades()
        lstActividades.DataSource = _costos.ObtenerActividadesCostosAutorizados()
        lstActividades.DataTextField = "ActNombre"
        lstActividades.DataValueField = "ID"
        lstActividades.DataBind()
    End Sub



    Private Sub CargarEjecucionCostos(ByVal par As IQ_Parametros)

        gvControlCostos.DataSource = _costos.ObtenerCostosAutorizados(par)
        gvControlCostos.DataBind()
    End Sub

    Protected Sub gvControlCostos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvControlCostos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            _Presupuestado = _Presupuestado + CDec(e.Row.Cells(2).Text)
            _Autorizado = _Autorizado + CDec(e.Row.Cells(3).Text)
            _DiferenciaPA = _DiferenciaPA + CDec(e.Row.Cells(4).Text)
            _Porcentaje1 = _Porcentaje1 + CDec(e.Row.Cells(5).Text)
            _TotalProduccion = _TotalProduccion + CDec(e.Row.Cells(6).Text)
            _DiferenciaPP = _DiferenciaPP + CDec(e.Row.Cells(7).Text)
            _Porcentaje3 = _Porcentaje3 + CDec(e.Row.Cells(8).Text)

            _Ejecutado = _Ejecutado + CDec(e.Row.Cells(9).Text)
            _DiferenciaPE = _DiferenciaPE + CDec(e.Row.Cells(10).Text)
            _Porcentaje2 = _Porcentaje2 + CDec(e.Row.Cells(11).Text)


        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(1).Text = "TOTALES"
            e.Row.Cells(2).Text = _Presupuestado.ToString("C0")
            e.Row.Cells(3).Text = _Autorizado.ToString("C0")
            e.Row.Cells(4).Text = _DiferenciaPA.ToString("C0")
            'e.Row.Cells(5).Text = _Porcentaje1.ToString("N2")
            e.Row.Cells(6).Text = _TotalProduccion.ToString("C0")
            e.Row.Cells(7).Text = _DiferenciaPP.ToString("C0")
            'e.Row.Cells(8).Text = _Porcentaje3.ToString("C0")
            e.Row.Cells(9).Text = _Ejecutado.ToString("C0")
            e.Row.Cells(10).Text = _DiferenciaPE.ToString("C0")
            ' e.Row.Cells(8).Text = _Porcentaje2.ToString("N")

        End If
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        If lstActividades.SelectedIndex > 0 And txtValorAutorizado.Text <> "" Then
            Dim ca As New IQ_ControlCostos()
            Dim P As IQ_Parametros

            P = CType(Session("PARAMETROS"), IQ_Parametros)
            ca.IdPropuesta = P.IdPropuesta
            ca.ParAlternativa = P.ParAlternativa
            ca.ParNacional = P.ParNacional
            ca.MetCodigo = P.MetCodigo
            ca.ID = CInt(lstActividades.SelectedValue)
            ca.ValorAutorizado = CDec(txtValorAutorizado.Text)
            ca.Fecha = DateTime.Now
            ca.Usuario = CDec(Session("IDUsuario"))
            ca.Observacion = txtObservacion.Text
            ca.Consecutivo = _costos.ObtenerConsecutivoDetalle(ca)
            _costos.InsertarCostosAutorizados(ca)
            CargarEjecucionCostos(P)
            txtValorAutorizado.Text = ""
            txtObservacion.Text = ""
            gvdetalle.DataSource = _costos.ObtenerDetalleAutorizadosXActividad(ca)
            gvdetalle.DataBind()


        Else
            ShowNotification("ASEGURESE DE SELECCIONAR UNA ACTIVDAD Y DIGITAR UN VALOR VALIDO .", WebMatrix.ShowNotifications.ErrorNotification)
        End If



    End Sub

    Protected Sub lkVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lkVolver.Click
        Dim p As New IQ_Parametros
        p.IdPropuesta = CInt(Request.QueryString("IdPropuesta"))
        p.ParAlternativa = CInt(Request.QueryString("Alternativa"))
        p.MetCodigo = CInt(Request.QueryString("Metodologia"))
        p.ParNacional = CInt(Request.QueryString("Fase"))

        Response.Redirect("ControlPresupuestos.aspx?IdPropuesta=" & p.IdPropuesta & "&Alternativa=" & p.ParAlternativa & "&Metodologia=" & p.MetCodigo & "&Fase=" & p.ParNacional)
    End Sub

    Protected Sub gvControlCostos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gvControlCostos.SelectedIndexChanged

    End Sub



    Protected Sub gvControlCostos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvControlCostos.RowCommand
        Select Case e.CommandName

            Case "ADD"
                'debemos validar la actividad que se selecciono y cargar los existentes 
                LinkButton1_ModalPopupExtender.Show()
                Dim act As String
                act = gvControlCostos.Rows(e.CommandArgument).Cells(0).Text
                lstActividades.SelectedIndex = lstActividades.Items.IndexOf(lstActividades.Items.FindByValue(act))
                If lstActividades.SelectedIndex > 0 Then
                    Dim ca As New IQ_ControlCostos()
                    Dim P As IQ_Parametros

                    P = CType(Session("PARAMETROS"), IQ_Parametros)
                    ca.IdPropuesta = P.IdPropuesta
                    ca.ParAlternativa = P.ParAlternativa
                    ca.ParNacional = P.ParNacional
                    ca.MetCodigo = P.MetCodigo
                    ca.ID = CInt(lstActividades.SelectedValue)
                    gvdetalle.DataSource = _costos.ObtenerDetalleAutorizadosXActividad(ca)
                    gvdetalle.DataBind()


                End If
                UpdatePanel1.Update()

        End Select


    End Sub

    Protected Sub lstActividades_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstActividades.SelectedIndexChanged

        If lstActividades.SelectedIndex > 0 Then
            Dim ca As New IQ_ControlCostos()
            Dim P As IQ_Parametros

            P = CType(Session("PARAMETROS"), IQ_Parametros)
            ca.IdPropuesta = P.IdPropuesta
            ca.ParAlternativa = P.ParAlternativa
            ca.ParNacional = P.ParNacional
            ca.MetCodigo = P.MetCodigo
            ca.ID = CInt(lstActividades.SelectedValue)

            gvdetalle.DataSource = _costos.ObtenerDetalleAutorizadosXActividad(ca)
            gvdetalle.DataBind()


        End If
        UpdatePanel1.Update()

    End Sub

    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSalir.Click
        LinkButton1_ModalPopupExtender.Hide()
        Dim P As IQ_Parametros

        P = CType(Session("PARAMETROS"), IQ_Parametros)
        CargarEjecucionCostos(P)
        upGrilla.Update()

    End Sub

    Protected Sub gvdetalle_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvdetalle.RowCommand
        Select Case e.CommandName

            Case "DEL"

                If lstActividades.SelectedIndex > 0 Then
                    Dim ca As New IQ_ControlCostos()
                    Dim P As IQ_Parametros

                    P = CType(Session("PARAMETROS"), IQ_Parametros)
                    ca.IdPropuesta = P.IdPropuesta
                    ca.ParAlternativa = P.ParAlternativa
                    ca.ParNacional = P.ParNacional
                    ca.MetCodigo = P.MetCodigo
                    ca.ID = CInt(lstActividades.SelectedValue)
                    ca.Consecutivo = CInt(gvdetalle.Rows(e.CommandArgument).Cells(0).Text)
                    'Borramos el registro y cargamos nuevamente la grilla
                    _costos.BorrarDetalleAutoizadosXactividad(ca)
                    gvdetalle.DataSource = _costos.ObtenerDetalleAutorizadosXActividad(ca)
                    gvdetalle.DataBind()


                End If
                UpdatePanel1.Update()

        End Select
    End Sub

    Protected Sub gvdetalle_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvdetalle.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            _Totaldetalle = _Totaldetalle + CDec(e.Row.Cells(2).Text)


        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(1).Text = "TOTALES"
            e.Row.Cells(2).Text = _Totaldetalle.ToString("C0")


        End If
    End Sub

    Protected Sub btnExportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportar.Click
        Dim excel As New List(Of Array)
        Dim Titulos As String = "ID;ACTIVIDAD;PRESUPUESTADO;AUTORIZADO;PRESUP VS AUTORIZAD;%;PRODUCCION;PRESUP VS PROD ; %;EJECUTADO;PRESUP VS EJEC;%;TIPO VALOR"
        Dim DynamicColNames() As String
        Dim lstCambios As List(Of IQ_ObtenerControlCostosAutorizados_Result)
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("Costos")
        Dim PAR As IQ_Parametros
        PAR = CType(Session("PARAMETROS"), IQ_Parametros)

        DynamicColNames = Titulos.Split(CChar(";"))
        excel.Add(DynamicColNames)
        lstCambios = _costos.ObtenerCostosAutorizados(PAR)

        For Each x In lstCambios
            excel.Add((x.ID.ToString() & ";" & x.ActNombre & ";" & CDec(x.PRESUPUESTADO).ToString("N2") & ";" & CDec(x.AUTORIZADO).ToString("N2") & ";" & CDec(x.PRESUVSAUTORIZADO).ToString("N2") & ";" & CDec(x.PORCENTAJE1).ToString("N2") & ";" & CDec(x.PRODUCCION).ToString("N2") & ";" & CDec(x.PRESUVSPROD).ToString("N2") & ";" & CDec(x.PORCENTAJE3).ToString("N2") & ";" & CDec(x.EJECUTADO).ToString("N2") & ";" & CDec(x.PRESUVSEJECUTADO).ToString("N2") & ";" & CDec(x.PORCENTAJE2).ToString("N2") & ";" & x.ActTipoValor).Split(CChar(";")).ToArray())
        Next
        worksheet.Cell("A1").Value = excel
        worksheet.Range("C2:L100").DataType = XLCellValues.Number
        worksheet.Range("C2:L100").Style.NumberFormat.NumberFormatId = 4
        Crearexcel(workbook, "Costos")

    End Sub

    Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Control_" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)

            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

End Class