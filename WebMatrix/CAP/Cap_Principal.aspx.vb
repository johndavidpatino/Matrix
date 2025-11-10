Imports WebMatrix.Util
Imports Utilidades.Encripcion
Imports System.IO
Imports CoreProject


Public Class Cap_Principal
    Inherits System.Web.UI.Page
    Dim _Cati As New IQ.Cati()
    Dim _CaraCara As New IQ.CaraCara()
    Dim _ActSub As New IQ.ActSubcontratadas()
    Dim _OnLine As New IQ.OnLine()
    Dim Usuario As String
    Dim Alternativa As String
    Dim Metodologia As String
    Dim Accion As String = ""
    Dim UrlReturn As String
    Dim Nacional As String
    Dim idPropuesta As String
    Dim _TotalVenta As Decimal = 0
    Dim _TotalCostoDirecto As Decimal = 0
    Dim _TotalGrossMargin As Decimal = 0
    Dim _TotalActSubCosto As Decimal = 0
    Dim _TotalActSubGasto As Decimal = 0
    Dim _prod As New IQ.UCPreguntas()
    Dim _sesiones As New IQ.sesionesGrupo()
    Dim _entrevistas As New IQ.Entrevistas()
    Dim _url As String
    Dim DiasCampo As Integer = 0
    Dim DiasProceso As Integer = 0
    Dim DiasInformes As Integer = 0
    Dim DiasDiseno As Integer = 0
    Dim _GM As New IQ.GrossMargin()
    Dim General As New IQ.IquoteGeneral()
    Dim _general As New IQ.IquoteGeneral()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            hfOperaciones.Value = "1"
            If Request.QueryString("ACCION") IsNot Nothing Then
                Select Case Request.QueryString("ACCION")
                    Case "2"
                        hfOperaciones.Value = "0"
                    Case "6"
                        hfOperaciones.Value = "0"
                    Case "7"
                        hfOperaciones.Value = "0"
                    Case "8"
                        hfOperaciones.Value = "0"
                    Case "9"
                        hfOperaciones.Value = "0"
                    Case "10"
                        hfOperaciones.Value = "0"
                End Select
            End If

            'chkAñoActual.Enabled = False
            'seschkAñoActual.Enabled = False
            'EntchkAñoActual.Enabled = False
            'OnchkAñoActual.Enabled = False
            'CatichkAñoActual.Enabled = False

            If Not IsPostBack Then
                Dim oU As New US.UsuariosUnidades
                Dim lUnidades = oU.ObtenerUsuariosUnidades(Session("IDUsuario").ToString)
                Dim flagUU = False
                For Each unidad In lUnidades
                    If unidad.UnidadId = 17 Then
                        flagUU = True
                    End If
                Next
                If Not (flagUU) Then
                    Response.Redirect("../Home/Default.aspx")
                End If
                Dim oConstante As New Constantes
                Dim constantes = oConstante.obtenerXId(8)
                Dim anos = constantes.Valor.Split(";")
                anoActual.InnerHtml = "Año " + anos(0) + ": "
                anoSiguiente.InnerHtml = "Año " + anos(1) + ": "
                constantes = oConstante.obtenerXId(9)
                If Convert.ToInt64(constantes.Valor) = 1 Then
                    CatichkAñoSiguiente.Enabled = True
                    CatichkAñoActual.Enabled = True
                Else
                    CatichkAñoSiguiente.Enabled = True
                    CatichkAñoActual.Enabled = True
                End If

                ObtenerQuerystring()
                Dim par As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)

                'Dim datos As IQ_ObtenerDatosPropuesta_Result
                Dim datos = _Cati.ObtenerDatosPropuesta(CType(Session("PARAMETROS"), IQ_Parametros).IdPropuesta)
                lblNomPropuesta.Text = datos.Titulo
                lblNomCliente.Text = datos.Cliente
                par.ParUnidad = datos.UNIDAD
                par.ParNomPresupuesto = datos.Titulo
                iqLbljobBook.Text = datos.JobBook
                lblplazo.Text = datos.Plazo.ToString()
                lblAnticipo.Text = datos.Anticipo.ToString()
                lblSaldo.Text = datos.Saldo.ToString()
                lblValorFinanciacion.Text = _Cati.ObtenerValorFinanciacion(par).ToString("C")

                If datos.UNIDAD = 22252 Then
                    chbObserver.Checked = True
                    chbObserver.Enabled = False
                Else
                    'chbObserver.Enabled = True
                End If

                hfTope.Value = (datos.TOPE * 100).ToString()
                UC_Producto1.unidad = par.ParUnidad
                UC_Producto2.unidad = par.ParUnidad
                UC_Producto3.unidad = par.ParUnidad

                'CARGAMOS LOS DATOS GENERALES DE CADA PRESUPUESTO
                Dim D As New IQ_DatosGeneralesPresupuesto
                D.IdPropuesta = par.IdPropuesta
                D.ParAlternativa = par.ParAlternativa

                D = _Cati.ObtenerDatosGenerales(D)
                If (D IsNot Nothing) Then
                    txtObservaciones.Text = D.Observaciones.ToString
                    txtDescripcion.Text = D.Descripcion.ToString()
                    txtDiasDiseno.Text = If(D.DiasDiseno.ToString() = "", 0, D.DiasDiseno.ToString())
                    txtDiasCampo.Text = If(D.DiasCampo.ToString() = "", 0, D.DiasCampo.ToString())
                    txtDiasInformes.Text = If(D.DiasInformes.ToString() = "", 0, D.DiasInformes.ToString())
                    txtDiasProceso.Text = If(D.DiasProcesamiento.ToString() = "", 0, D.DiasProcesamiento.ToString())
                    txtNumMediciones.Text = If(D.NumeroMediciones.ToString() = "", "1", D.NumeroMediciones.ToString())
                    txtMesesMediciones.Text = If(D.MesesMediciones.ToString() = "", "1", D.MesesMediciones)
                    If D.TasaCambio IsNot Nothing Then
                        lblTasa.Text = Convert.ToDecimal(D.TasaCambio).ToString("C")
                    Else
                        lblTasa.Text = Convert.ToDecimal(_general.ObtenerTasaCambioDia()).ToString("C")
                    End If


                    SumarDias()

                Else
                    lblTasa.Text = Convert.ToDecimal(_general.ObtenerTasaCambioDia()).ToString("C")
                End If

                CargarCuantitativos(CType(Session("PARAMETROS"), IQ_Parametros))
                'se deben cargar  cada una de las tecnicas en este punto
                CatiCargarIncidencia(lstIncidencia_101)
                CatiCargarIncidencia(CCLstIncidencia)

                CargarOferta(sesLstOferta)
                CargarOferta(EntLstOferta)

                If Accion = "3" Then
                    panel100.Enabled = False
                    PanelCaraCara.Enabled = False
                    PanelSesiones.Enabled = False
                    PanelEntrevistas.Enabled = False
                    PanelOnline.Enabled = False
                    PanelSub.Enabled = False
                End If

                'SI EXISTE LA VARIABLE GM  SIGNIFICA QUE FUE REDIRECCIONADO DIRECTAMENTE A MODIFICACION DEL GROSS MARGIN
                'ESTA SITUACION SOLO SE PRESENTA CUANDO SE VA A BAJAR EL GROSS MARGIN 
                pnNotificacion.Visible = False
                If txtNuevoGM.Text = "" Then
                    If Request.QueryString("GMO") IsNot Nothing Then
                        'ESTA VARIABLES DEBEN VENIR OBLIGATORIAMENTE EN LA  DIRECCION 
                        hfTipoCalculo.Value = Request.QueryString("TIPOCALCULO")

                        If hfTipoCalculo.Value = "1" Then
                            txtNuevoGM.Text = Request.QueryString("GMU")
                            txtGMOpera.Text = Request.QueryString("GMO")
                            par.ParNacional = CInt(Request.QueryString("FASE"))
                            par.MetCodigo = CInt(Request.QueryString("METODOLOGIA"))
                            gmTxtContrasena.Visible = True
                            lblContrasena.Visible = True
                            pnNotificacion.Visible = False
                            Session("PARAMETROS") = par
                            CargarJBE(par, 0, 0, 1)
                            CargarJBI(par, 0, 0, 1)

                            lkgm_ModalPopupExtender.Show()
                            UpdatePanel5.Update()
                        ElseIf hfTipoCalculo.Value = "2" Then
                            txtNuevoGM.Text = Request.QueryString("GM")
                            txtGMOpera.Text = Request.QueryString("GMO")
                            gmTxtContrasena.Visible = True
                            lblContrasena.Visible = True
                            pnNotificacion.Visible = False
                            Session("PARAMETROS") = par
                            lkgm_ModalPopupExtender.Show()
                            UpdatePanel5.Update()
                        End If

                    Else
                        gmTxtContrasena.Visible = False
                        lblContrasena.Visible = False
                    End If

                End If
                lblalternativaNum.Text = par.ParAlternativa & "  DE  " & _Cati.UltimaAlternativa(par).ToString()

                ' lkSalir.PostBackUrl = Request.UrlReferrer.PathAndQuery.ToString()


                'Else
                '    Dim control As String = Request.Form("__EVENTTARGET")
                '    If Right(control, 9) = "CheckBox1" Then

                '    End If
            End If

            ScriptManager.RegisterStartupScript(upHoras, Me.GetType(), "MyAction", "CreateAccordion();", True)
            ScriptManager.RegisterStartupScript(upHorasEntrevistas, Me.GetType(), "MyAction1", "CreateAccordionEnt();", True)
            ScriptManager.RegisterStartupScript(UpdatePanel11, Me.GetType(), "Filter1", " FilterGrid();", True)


        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try

    End Sub



#Region "Utilidadedes generales"

    Private Sub CargarTipoServicio()

        Try

            lstTipoServicio.DataSource = _CaraCara.ObtenerTipoServicio()
            lstTipoServicio.DataTextField = "TS_Descripcion"
            lstTipoServicio.DataValueField = "TS_Id"
            lstTipoServicio.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CargarTipoEvidencia()

        Try

            lstTipoEvidencia.DataSource = _CaraCara.ObtenerTipoEvidencia()
            lstTipoEvidencia.DataTextField = "TE_Descripcion"
            lstTipoEvidencia.DataValueField = "TE_Id"
            lstTipoEvidencia.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CargarEviencia()

        Try

            lstEvidencia.DataSource = _CaraCara.ObtenerEvidencia()
            lstEvidencia.DataTextField = "EV_Descripcion"
            lstEvidencia.DataValueField = "EV_Id"
            lstEvidencia.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CargarContactos()

        Try

            lstNumContactos.DataSource = _CaraCara.ObtenerCantidadContactos()
            lstNumContactos.DataTextField = "CC_Cantidad"
            lstNumContactos.DataValueField = "CC_Id"
            lstNumContactos.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub CargarTipoMuestra(ByVal lst As DropDownList, ByVal Metodologia As Integer)

        Try
            lst.DataSource = _Cati.ObtenerOpcionesMuestra(Metodologia)
            lst.DataTextField = "DescIdentMuestra"
            lst.DataValueField = "IdIdentificador"
            lst.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CargarFases(ByVal lst As DropDownList, ByVal tec As Integer)
        Try
            lst.DataSource = _Cati.ObtenerFases(tec)
            lst.DataTextField = "DescFase"
            lst.DataValueField = "IdFase"
            lst.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub LimpiarControles(ByVal p As Control)

        Dim c As Control
        For Each c In p.Controls
            If c.GetType.ToString() = "System.Web.UI.WebControls.TextBox" Then
                CType(c, TextBox).Text = ""
            End If

            If c.GetType.ToString() = "System.Web.UI.WebControls.DropDownList" Then
                If CType(c, DropDownList).Items.Count > 0 Then
                    CType(c, DropDownList).SelectedIndex = 0
                End If

            End If

            If c.GetType.ToString() = "System.Web.UI.WebControls.CheckBoxList" Then
                LimpiarListBox(CType(c, CheckBoxList))
            End If

            If c.GetType.ToString() = "System.Web.UI.WebControls.GridView" Then
                CType(c, GridView).DataSource = Nothing
                CType(c, GridView).DataBind()
            End If

            If c.GetType.ToString() = "System.Web.UI.WebControls.Label" Then
                CType(c, Label).Text = ""
            End If

            If c.GetType.ToString() = "System.Web.UI.WebControls.RadioButtonList" Then
                CType(c, RadioButtonList).SelectedIndex = -1
            End If


            If c.GetType.ToString() = " System.Web.UI.WebControls.CheckBox" Then
                CType(c, CheckBox).Checked = False
            End If

            If c.HasControls Then
                LimpiarControles(c)
            End If
        Next

    End Sub

    Private Sub LimpiarListBox(ByVal chk As CheckBoxList)
        For i = 0 To chk.Items.Count - 1
            If chk.Items(i).Selected = True Then
                chk.Items(i).Selected = False
            End If

        Next
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCuanti.RowCommand
        Dim cuanti As String
        Dim cmdName = e.CommandName

        Dim oConstante As New Constantes
        Dim constantes = oConstante.obtenerXId(8)
        Dim anos = constantes.Valor.Split(";")
        anoActual.InnerHtml = "Año " + anos(0) + ": "
        anoSiguiente.InnerHtml = "Año " + anos(1) + ": "
        constantes = oConstante.obtenerXId(9)
        If Convert.ToInt64(constantes.Valor) = 1 Then
            CatichkAñoSiguiente.Enabled = True
            CatichkAñoActual.Enabled = True
        Else
            CatichkAñoSiguiente.Enabled = True
            CatichkAñoActual.Enabled = True
        End If

        Try
            'validamos que antes de ingresar  a crear un presupuesto haya  digitado la descripcion general para la alternativa 
            If txtDescripcion.Text.Trim <> "" Then

                Select Case e.CommandName

                    Case "TEC"
                        cuanti = CType(gvCuanti.Rows(e.CommandArgument).Cells(1).Controls(0), LinkButton).Text

                        Select Case cuanti

                            Case "200"
                                'CATI
                                UpdatePanel3.Update()
                                hfTecnica.Value = "200"
                                LimpiarControles(panel100)
                                CargarMetodologias(CatiLstMetodologia, Convert.ToInt32(hfTecnica.Value))
                                CatiLstMetodologia.Enabled = True
                                Dim par As IQ_Parametros
                                par = CType(Session("PARAMETROS"), IQ_Parametros)
                                par.TecCodigo = 200
                                CargarFases(CatiLstFase, par.TecCodigo)

                                'CatichkAñoActual.Checked = False
                                'CatichkAñoActual.Enabled = False
                                'CatichkAñoSiguiente.Checked = True

                                CargarProcesos(par.TecCodigo, chkProcesos)

                                For i = 0 To chkProcesos.Items.Count - 1
                                    chkProcesos.Items(i).Selected = True
                                Next

                                CatiTxtNProcesosDC.Text = 1
                                CatiTxtNProcesosTL.Text = 1
                                CatiTxtNProcesosTablas.Text = 1
                                CatiTxtNProcesosBases.Text = 1
                                CatiLstFase.Enabled = True
                                CatiTxtObservaciones.Text = "NO TENGO OBSERVACIONES"

                                TabContainer1.Tabs(1).Visible = False
                                TabContainer1.Tabs(2).Visible = False
                                TabContainer1.Tabs(0).Visible = True
                                TabContainer1.Tabs(3).Visible = False
                                TabContainer1.Tabs(4).Visible = False
                                TabContainer1.Tabs(5).Visible = False
                                upTabContainer.Update()
                                ' UpdatePanel2.Update()
                                lkb1_ModalPopupExtender.Show()


                            Case "100"
                                'CARA CARA 

                                hfTecnica.Value = "100"
                                LimpiarControles(PanelCaraCara)
                                CCLstFases.Enabled = True
                                CargarMetodologias(CCLstMetodologia, Convert.ToInt32(hfTecnica.Value))
                                CargarTipoServicio()
                                CargarTipoEvidencia()
                                CargarEviencia()
                                CargarContactos()

                                CCLstMetodologia.Enabled = True
                                Dim par As IQ_Parametros
                                par = CType(Session("PARAMETROS"), IQ_Parametros)
                                par.TecCodigo = 100
                                CargarFases(CCLstFases, par.TecCodigo)

                                'chkAñoActual.Checked = False
                                'chkAñoActual.Enabled = False
                                'chkAñoSiguiente.Checked = True

                                CCTxtObservaciones.Text = "NO TENGO OBSERVACIONES"
                                CargarProcesos(par.TecCodigo, CCchkProcesos)

                                For i = 0 To CCchkProcesos.Items.Count - 1
                                    CCchkProcesos.Items(i).Selected = True
                                Next

                                CCTxtNumProcDC.Text = 1
                                CCTxtNumProcTL.Text = 1
                                CCTxtNumProcTablas.Text = 1
                                CCTxtNumProcBases.Text = 1

                                'CCChkDispPropio.Visible = False

                                TabContainer1.Tabs(1).Visible = True
                                TabContainer1.Tabs(2).Visible = False
                                TabContainer1.Tabs(0).Visible = False
                                TabContainer1.Tabs(3).Visible = False
                                TabContainer1.Tabs(4).Visible = False
                                TabContainer1.Tabs(5).Visible = False
                                upTabContainer.Update()

                                lkb1_ModalPopupExtender.Show()

                            Case "300"
                                'online

                                hfTecnica.Value = "300"
                                LimpiarControles(PanelOnline)
                                CCLstFases.Enabled = True
                                CargarMetodologias(onLstMetodologia, Convert.ToInt32(hfTecnica.Value))
                                onLstMetodologia.Enabled = True
                                Dim par As IQ_Parametros
                                par = CType(Session("PARAMETROS"), IQ_Parametros)
                                par.TecCodigo = 300
                                CargarFases(onLstFase, par.TecCodigo)

                                'OnchkAñoActual.Checked = False
                                'OnchkAñoActual.Enabled = False
                                'OnchkAñoSiguiente.Checked = True

                                onLstFase.SelectedIndex = 1
                                'onLstFase.Enabled = False
                                OnTxtObservaciones.Text = "NO TENGO OBSERVACIONES"

                                CargarProcesos(par.TecCodigo, onChkProcesos)
                                For i = 0 To onChkProcesos.Items.Count - 1
                                    onChkProcesos.Items(i).Selected = True
                                Next

                                onTxtNumProcesosDC.Text = 1
                                onTxtNumProcesosTL.Text = 1
                                onTxtNumProcesosTablas.Text = 1
                                onTxtNumProcesosBases.Text = 1
                                TabContainer1.Tabs(1).Visible = False
                                TabContainer1.Tabs(2).Visible = False
                                TabContainer1.Tabs(0).Visible = False
                                TabContainer1.Tabs(3).Visible = False
                                TabContainer1.Tabs(4).Visible = False
                                TabContainer1.Tabs(5).Visible = True
                                upTabContainer.Update()

                                lkb1_ModalPopupExtender.Show()

                            Case "600"
                                'Sesiones de grupo
                                hfTecnica.Value = "600"
                                Dim par As IQ_Parametros
                                par = CType(Session("PARAMETROS"), IQ_Parametros)
                                par.TecCodigo = 600

                                LimpiarControles(PanelSesiones)
                                CargarMetodologias(SesLstMetodologia, Convert.ToInt32(hfTecnica.Value))
                                SesLstMetodologia.Enabled = True
                                CargarFases(sesLstFase, par.TecCodigo)

                                'seschkAñoActual.Checked = False
                                'seschkAñoActual.Enabled = False
                                'seschkAñoSiguiente.Checked = True

                                CargarProcesos(par.TecCodigo, SesChkProcesos)
                                CargarOpciones(sesChkOpciones)
                                SesTxtObservaciones.Text = "NO TENGO OBSERVACIONES"
                                TabContainer1.Tabs(0).Visible = False
                                TabContainer1.Tabs(1).Visible = False
                                TabContainer1.Tabs(2).Visible = False
                                TabContainer1.Tabs(3).Visible = True
                                TabContainer1.Tabs(4).Visible = False
                                TabContainer1.Tabs(5).Visible = False

                                gvCargosSesiones.DataSource = _sesiones.ObtenerCargos(par, 1)
                                gvCargosSesiones.DataBind()

                                upTabContainer.Update()

                                'Dim mePage As Page = CType(HttpContext.Current.Handler, Page)
                                'ScriptManager.RegisterStartupScript(mePage, Me.GetType(), "MoverAccordion", "moverDiv();", True)

                                lkb1_ModalPopupExtender.Show()

                            Case "700"
                                'Entrevistas
                                hfTecnica.Value = "700"
                                Dim par As IQ_Parametros
                                par = CType(Session("PARAMETROS"), IQ_Parametros)
                                par.TecCodigo = 700

                                LimpiarControles(PanelEntrevistas)
                                CargarFases(EntLstFase, par.TecCodigo)

                                'EntchkAñoActual.Checked = False
                                'EntchkAñoActual.Enabled = False
                                'EntchkAñoSiguiente.Checked = True

                                CargarMetodologias(EntLstMetodologia, Convert.ToInt32(hfTecnica.Value))
                                EntLstMetodologia.Enabled = True
                                CargarOpciones(EntChkOpciones)
                                CargarProcesos(par.TecCodigo, EntChkProcesos)
                                EntTxtObservaciones.Text = "NO TENGO OBSERVACIONES"
                                TabContainer1.Tabs(0).Visible = False
                                TabContainer1.Tabs(1).Visible = False
                                TabContainer1.Tabs(2).Visible = False
                                TabContainer1.Tabs(3).Visible = False
                                TabContainer1.Tabs(4).Visible = True
                                TabContainer1.Tabs(5).Visible = False

                                gvCargosEntrevistas.DataSource = _entrevistas.ObtenerCargos(par, 1)
                                gvCargosEntrevistas.DataBind()

                                upTabContainer.Update()

                                lkb1_ModalPopupExtender.Show()

                        End Select
                    Case "SUB"
                        cuanti = CType(gvCuanti.Rows(e.CommandArgument).Cells(1).Controls(0), LinkButton).Text

                        UpdatePanel3.Update()
                        TabContainer1.Tabs(1).Visible = False
                        TabContainer1.Tabs(2).Visible = True
                        TabContainer1.Tabs(0).Visible = False
                        CargarActividades(CInt(cuanti))
                        lkb1_ModalPopupExtender.Show()

                    Case "GM"
                        Dim par As IQ_Parametros
                        cuanti = CType(gvCuanti.Rows(e.CommandArgument).Cells(1).Controls(0), LinkButton).Text
                        par = CType(Session("PARAMETROS"), IQ_Parametros)
                        par.TecCodigo = CInt(cuanti)
                        If (_Cati.ExisteCati(par)) Then
                            par.MetCodigo = _Cati.ObtenerMetodologia(par)
                            Session("PARAMETROS") = par
                            CargarDatosCati()
                        End If

                        lkgm_ModalPopupExtender.Show()


                End Select
            Else
                ShowNotification("Digite la descripcion para esta alternaiva !", WebMatrix.ShowNotifications.ErrorNotification)
            End If


        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try


    End Sub

    Protected Sub gvNacionales_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim cuanti As String
        Dim cmdName = e.CommandName
        Dim gvNacionales As GridView = CType(sender, GridView)
        Try

            Dim accion As String = ""
            If Request.QueryString("ACCION") IsNot Nothing Then
                accion = Request.QueryString("ACCION")
            End If

            Select Case e.CommandName

                Case "TEC"
                    cuanti = CType(gvNacionales.Rows(e.CommandArgument).Cells(0).Controls(0), LinkButton).Text

                    Select Case cuanti

                        Case "200"
                            'CATI

                            UpdatePanel3.Update()
                            hfTecnica.Value = "200"
                            CargarMetodologias(CatiLstMetodologia, Convert.ToInt32(hfTecnica.Value))
                            CatiLstMetodologia.Enabled = False
                            Dim par As IQ_Parametros
                            par = CType(Session("PARAMETROS"), IQ_Parametros)
                            par.TecCodigo = 200
                            CargarFases(CatiLstFase, par.TecCodigo)
                            CargarProcesos(par.TecCodigo, chkProcesos)
                            'LEEMOS DESDE LA SUBGRILLA LOS PARAMETROS DE METODOLOGIA Y NACIONALES
                            CargarParametrosSubgrilla(e.CommandArgument, gvNacionales)
                            par = CType(Session("PARAMETROS"), IQ_Parametros)
                            CatiLstFase.SelectedIndex = CatiLstFase.Items.IndexOf(CatiLstFase.Items.FindByValue(par.ParNacional))
                            CatiLstFase.Enabled = False
                            If accion = "2" And Not _Cati.PresupuestoAprobado(par) Then
                                panel100.Enabled = True
                            Else
                                If _Cati.PresupuestoRevisado(par) Or _Cati.PresupuestoAprobado(par) Then
                                    panel100.Enabled = False
                                Else
                                    panel100.Enabled = True
                                End If
                            End If

                            CargarDatosCati()

                            TabContainer1.Tabs(1).Visible = False
                            TabContainer1.Tabs(2).Visible = False
                            TabContainer1.Tabs(0).Visible = True
                            TabContainer1.Tabs(3).Visible = False
                            TabContainer1.Tabs(4).Visible = False
                            TabContainer1.Tabs(5).Visible = False
                            upTabContainer.Update()
                            lkb1_ModalPopupExtender.Show()

                        Case "100"
                            'CARA CARA 
                            hfTecnica.Value = "100"
                            CargarMetodologias(CCLstMetodologia, Convert.ToInt32(hfTecnica.Value))
                            CCLstMetodologia.Enabled = False
                            CargarTipoServicio()
                            CargarTipoEvidencia()
                            CargarEviencia()
                            CargarContactos()

                            Dim par As IQ_Parametros
                            par = CType(Session("PARAMETROS"), IQ_Parametros)
                            par.TecCodigo = 100
                            CargarFases(CCLstFases, par.TecCodigo)

                            CargarProcesos(par.TecCodigo, CCchkProcesos)
                            'LEEMOS DESDE LA SUBGRILLA LOS PARAMERSO DE METODOLOGIA Y NACIONALES
                            CargarParametrosSubgrilla(e.CommandArgument, gvNacionales)
                            par = CType(Session("PARAMETROS"), IQ_Parametros)
                            CCLstFases.SelectedIndex = CCLstFases.Items.IndexOf(CCLstFases.Items.FindByValue(par.ParNacional))

                            CCCargarDepartamentos()

                            Session("PARAMETROS") = par
                            'Procedimeinto para cargar los valores existente de la  tecnica 
                            If accion = "2" And Not _Cati.PresupuestoAprobado(par) Then
                                PanelCaraCara.Enabled = True
                            Else
                                If _Cati.PresupuestoRevisado(par) Or _Cati.PresupuestoAprobado(par) Then
                                    PanelCaraCara.Enabled = False
                                Else
                                    PanelCaraCara.Enabled = True
                                End If
                            End If

                            CargarCaraCara()

                            TabContainer1.Tabs(0).Visible = False
                            TabContainer1.Tabs(1).Visible = True
                            TabContainer1.Tabs(2).Visible = False
                            TabContainer1.Tabs(3).Visible = False
                            TabContainer1.Tabs(4).Visible = False
                            TabContainer1.Tabs(5).Visible = False

                            upTabContainer.Update()
                            lkb1_ModalPopupExtender.Show()
                        Case "300"
                            'ONLINE
                            hfTecnica.Value = "300"
                            CargarMetodologias(onLstMetodologia, Convert.ToInt32(hfTecnica.Value))
                            onLstMetodologia.Enabled = False
                            Dim par As IQ_Parametros
                            par = CType(Session("PARAMETROS"), IQ_Parametros)
                            par.TecCodigo = 300
                            CargarFases(onLstFase, par.TecCodigo)

                            'LEEMOS DESDE LA SUBGRILLA LOS PARAMERSO DE METODOLOGIA Y NACIONALES
                            CargarParametrosSubgrilla(e.CommandArgument, gvNacionales)
                            par = CType(Session("PARAMETROS"), IQ_Parametros)
                            onLstFase.SelectedIndex = CCLstFases.Items.IndexOf(CCLstFases.Items.FindByValue(par.ParNacional))
                            onLstFase.Enabled = False

                            Session("PARAMETROS") = par
                            'Procedimeinto para cargar los valores existente de la  tecnica 
                            If accion = "2" And Not _Cati.PresupuestoAprobado(par) Then
                                PanelOnline.Enabled = True
                            Else
                                If _Cati.PresupuestoRevisado(par) Or _Cati.PresupuestoAprobado(par) Then
                                    PanelOnline.Enabled = False
                                Else
                                    PanelOnline.Enabled = True
                                End If
                            End If


                            CargarOnline(par)

                            TabContainer1.Tabs(0).Visible = False
                            TabContainer1.Tabs(1).Visible = False
                            TabContainer1.Tabs(2).Visible = False
                            TabContainer1.Tabs(3).Visible = False
                            TabContainer1.Tabs(4).Visible = False
                            TabContainer1.Tabs(5).Visible = True

                            upTabContainer.Update()
                            lkb1_ModalPopupExtender.Show()

                        Case "600"
                            'SESIONES
                            hfTecnica.Value = "600"
                            CargarMetodologias(SesLstMetodologia, Convert.ToInt32(hfTecnica.Value))
                            SesLstMetodologia.Enabled = False
                            Dim par As IQ_Parametros
                            par = CType(Session("PARAMETROS"), IQ_Parametros)
                            par.TecCodigo = 600
                            CargarFases(sesLstFase, par.TecCodigo)
                            CargarProcesos(par.TecCodigo, SesChkProcesos)
                            'LEEMOS DESDE LA SUBGRILLA LOS PARAMERSO DE METODOLOGIA Y NACIONALES
                            CargarParametrosSubgrilla(e.CommandArgument, gvNacionales)
                            par = CType(Session("PARAMETROS"), IQ_Parametros)
                            'CARGAMOS EL VALOR DE LA FASE Y BLOQUEAMOS EL CONTROL
                            sesLstFase.SelectedIndex = sesLstFase.Items.IndexOf(sesLstFase.Items.FindByValue(par.ParNacional))
                            sesCargarDepartamento()


                            Session("PARAMETROS") = par
                            If accion = "2" And Not _Cati.PresupuestoAprobado(par) Then
                                PanelSesiones.Enabled = True
                            Else
                                If _Cati.PresupuestoRevisado(par) Or _Cati.PresupuestoAprobado(par) Then
                                    PanelSesiones.Enabled = False
                                Else
                                    PanelSesiones.Enabled = True
                                End If
                            End If


                            CargarSesiones(CType(Session("PARAMETROS"), IQ_Parametros))

                            TabContainer1.Tabs(0).Visible = False
                            TabContainer1.Tabs(1).Visible = False
                            TabContainer1.Tabs(2).Visible = False
                            TabContainer1.Tabs(3).Visible = True
                            TabContainer1.Tabs(4).Visible = False
                            TabContainer1.Tabs(5).Visible = False

                            upTabContainer.Update()
                            lkb1_ModalPopupExtender.Show()

                        Case "700"
                            'ENTREVISTAS
                            hfTecnica.Value = "700"
                            CargarMetodologias(EntLstMetodologia, Convert.ToInt32(hfTecnica.Value))
                            EntLstMetodologia.Enabled = False
                            Dim par As IQ_Parametros
                            par = CType(Session("PARAMETROS"), IQ_Parametros)
                            par.TecCodigo = 700
                            CargarFases(EntLstFase, par.TecCodigo)
                            CargarProcesos(par.TecCodigo, EntChkProcesos)
                            'LEEMOS DESDE LA SUBGRILLA LOS PARAMERSO DE METODOLOGIA Y NACIONALES
                            CargarParametrosSubgrilla(e.CommandArgument, gvNacionales)
                            par = CType(Session("PARAMETROS"), IQ_Parametros)
                            EntLstFase.SelectedIndex = EntLstFase.Items.IndexOf(EntLstFase.Items.FindByValue(par.ParNacional))
                            entCargarDepartametos()


                            Session("PARAMETROS") = par
                            If accion = "2" And Not _Cati.PresupuestoAprobado(par) Then
                                PanelEntrevistas.Enabled = True
                            Else
                                If _Cati.PresupuestoRevisado(par) Or _Cati.PresupuestoAprobado(par) Then
                                    PanelEntrevistas.Enabled = False
                                Else
                                    PanelEntrevistas.Enabled = True
                                End If

                            End If


                            CargarEntrevistas(CType(Session("PARAMETROS"), IQ_Parametros))

                            TabContainer1.Tabs(0).Visible = False
                            TabContainer1.Tabs(1).Visible = False
                            TabContainer1.Tabs(2).Visible = False
                            TabContainer1.Tabs(3).Visible = False
                            TabContainer1.Tabs(4).Visible = True
                            TabContainer1.Tabs(5).Visible = False

                            upTabContainer.Update()
                            lkb1_ModalPopupExtender.Show()

                    End Select
                Case "SUB"
                    cuanti = CType(gvNacionales.Rows(e.CommandArgument).Cells(0).Controls(0), LinkButton).Text
                    hfTecnica.Value = "999"

                    Dim par As IQ_Parametros
                    par = CType(Session("PARAMETROS"), IQ_Parametros)
                    par.TecCodigo = CInt(cuanti)
                    CargarParametrosSubgrilla(e.CommandArgument, gvNacionales)
                    'par.MetCodigo = _Cati.ObtenerMetodologia(par)

                    par = CType(Session("PARAMETROS"), IQ_Parametros)
                    If _Cati.PresupuestoRevisado(par) Or _Cati.PresupuestoAprobado(par) Then
                        PanelSub.Enabled = False
                    Else
                        PanelSub.Enabled = True
                    End If

                    TabContainer1.Tabs(0).Visible = False
                    TabContainer1.Tabs(1).Visible = False
                    TabContainer1.Tabs(2).Visible = True
                    TabContainer1.Tabs(3).Visible = False
                    TabContainer1.Tabs(4).Visible = False
                    TabContainer1.Tabs(5).Visible = False
                    SubLblTotal.Text = _ActSub.TotalizarActSub(par).ToString("C0")
                    CargarActividades(CInt(cuanti))
                    upTabContainer.Update()
                    upActSub.Update()
                    lkb1_ModalPopupExtender.Show()

                Case "GM"
                    Dim par As IQ_Parametros
                    cuanti = CType(gvNacionales.Rows(e.CommandArgument).Cells(0).Controls(0), LinkButton).Text
                    par = CType(Session("PARAMETROS"), IQ_Parametros)
                    par.TecCodigo = CInt(cuanti)
                    hfTipoCalculo.Value = "1"
                    CargarParametrosSubgrilla(e.CommandArgument, gvNacionales)
                    par = CType(Session("PARAMETROS"), IQ_Parametros)
                    'par.MetCodigo = _Cati.ObtenerMetodologia(par)
                    Session("PARAMETROS") = par
                    lblgmActual.Text = CType(gvNacionales.Rows(e.CommandArgument).Cells(11).Controls(0), LinkButton).Text

                    txtNuevoGM.Text = CType(gvNacionales.Rows(e.CommandArgument).Cells(11).Controls(0), LinkButton).Text
                    txtValorVentaSimular.Text = ""
                    txtGMOpera.Text = ""
                    lblGMsimulado.Text = ""
                    lblValorVentaSimulado.Text = ""
                    'lblgmActual.Text = hfTope.Value
                    lblContrasena.Visible = False
                    gmTxtContrasena.Visible = False
                    pnNotificacion.Visible = False


                    'txtNuevoGM.Text = ""
                    CargarJBI(par, 0, 0, False)
                    CargarJBE(par, 0, 0, False)

                    UpdatePanel5.Update()
                    lkgm_ModalPopupExtender.Show()

                Case "CONTROL"
                    'CONSULTA DE CONTROL DE EJECUCION DE PRESUPUESTO
                    Dim par As IQ_Parametros
                    cuanti = CType(gvNacionales.Rows(e.CommandArgument).Cells(0).Controls(0), LinkButton).Text
                    par = CType(Session("PARAMETROS"), IQ_Parametros)
                    par.TecCodigo = CInt(cuanti)
                    CargarParametrosSubgrilla(e.CommandArgument, gvNacionales)
                    par = CType(Session("PARAMETROS"), IQ_Parametros)
                    If Request.QueryString("ACCION") IsNot Nothing Then
                        Response.Redirect("ControlCostos.aspx?IdPropuesta=" & par.IdPropuesta & "&Alternativa=" & par.ParAlternativa & "&Metodologia=" & par.MetCodigo & "&Nacional=" & par.ParNacional & "&ACCION=" & Request.QueryString("ACCION") & "&o_3453oioioioo_1133=" & hfOperaciones.Value)
                    Else
                        Response.Redirect("ControlCostos.aspx?IdPropuesta=" & par.IdPropuesta & "&Alternativa=" & par.ParAlternativa & "&Metodologia=" & par.MetCodigo & "&Nacional=" & par.ParNacional & "&o_3453oioioioo_1133=" & hfOperaciones.Value)
                    End If

                Case "BORRAR"
                    Dim par As New IQ_Parametros
                    par.IdPropuesta = CType(Session("PARAMETROS"), IQ_Parametros).IdPropuesta
                    par.ParAlternativa = CType(Session("PARAMETROS"), IQ_Parametros).ParAlternativa
                    ' esta seccion se corrigio   para soluconar el problema de la unidad 
                    par.ParUnidad = CType(Session("PARAMETROS"), IQ_Parametros).ParUnidad
                    Session("PARAMETROS") = par
                    CargarParametrosSubgrilla(e.CommandArgument, gvNacionales)
                    par = Session("PARAMETROS")
                    _Cati.BorrarPresupuesto(par)
                    CargarCuantitativos(par)
                    UpdatePanel2.Update()


                Case "REVISADO"
                    Dim btn As ImageButton = CType(gvNacionales.Rows(e.CommandArgument).Cells(12).Controls(1), ImageButton)
                    If btn.ImageUrl = "~/CAP/Imagenes/checkbox-checked.png" Then
                        btn.ImageUrl = "~/CAP/Imagenes/checkbox-unchecked.png"
                        btn.Attributes.Add("onclick", "javascript" & ":return confirm('Esta seguro que desea marcar este presupuesto como  REVISADO');")
                    Else
                        btn.ImageUrl = "~/CAP/Imagenes/checkbox-checked.png"
                        btn.Attributes.Add("onclick", "javascript" & ":return confirm('Esta seguro que desea marcar este presupuesto como NO REVISADO');")

                    End If

                    Dim r As GridViewRow
                    Dim rowCout As Integer = gvCuanti.Rows.Count
                    Dim par As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)

                    r = CType(gvNacionales.Rows(e.CommandArgument).Cells(12).Parent, GridViewRow)
                    par.MetCodigo = CInt(r.Cells(3).Text)
                    par.ParNacional = CInt(r.Cells(1).Text)


                    If CType(r.Cells(12).FindControl("ImageButton1"), ImageButton).ImageUrl = "~/CAP/Imagenes/checkbox-checked.png" Then
                        'TECNICA COLUMNA 1
                        par.TecCodigo = CInt(CType(r.Cells(0).Controls(0), LinkButton).Text)
                        par.ParRevisado = True
                        par.ParRevisadoPor = CDec(Session("IDUsuario"))
                        par.ParFechaRevision = Date.UtcNow.AddHours(-5)
                        _Cati.ActualizarRevision(par)

                    Else
                        par.TecCodigo = CInt(CType(r.Cells(0).Controls(0), LinkButton).Text)
                        par.ParRevisado = False
                        par.ParRevisadoPor = CDec(Session("IDUsuario"))
                        par.ParFechaRevision = Date.UtcNow.AddHours(-5)
                        _Cati.ActualizarRevision(par)



                    End If

                    'VALIDAMOS SI TODOS LOS PRESUPUESTOS  CREADOS YA HAN SIDO REVISADOS  Y ENVIAMOS EL MAIL 
                    'If _Cati.ValidarRevisiones(par) Then
                    '    EnviarEmail(par)
                    'End If

                    EnviarEmail(par)

                    UpdatePanel2.Update()

                Case "JBINT"
                    If chbObserver.Checked = True Then
                        Exit Sub
                    End If
                    If Not (hfOperaciones.Value = "0") Then
                        Exit Sub
                    End If
                    If _general.ExisteAlternativaAprobada(CType(Session("PARAMETROS"), IQ_Parametros)) Then
                        Dim par As IQ_Parametros
                        cuanti = CType(gvNacionales.Rows(e.CommandArgument).Cells(0).Controls(0), LinkButton).Text
                        par = CType(Session("PARAMETROS"), IQ_Parametros)
                        par.TecCodigo = CInt(cuanti)
                        CargarParametrosSubgrilla(e.CommandArgument, gvNacionales)
                        par = CType(Session("PARAMETROS"), IQ_Parametros)
                        If Request.QueryString("ACCION") IsNot Nothing Then
                            Response.Redirect("CostosJBInterno.aspx?JOBBOOK=" & iqLbljobBook.Text & "&ACCION=" & Request.QueryString("ACCION"))
                        Else
                            Response.Redirect("CostosJBInterno.aspx?JOBBOOK=" & iqLbljobBook.Text)
                        End If

                    End If


                Case "JBEXT"
                    If _general.ExisteAlternativaAprobada(CType(Session("PARAMETROS"), IQ_Parametros)) Then
                        Dim par As IQ_Parametros
                        cuanti = CType(gvNacionales.Rows(e.CommandArgument).Cells(0).Controls(0), LinkButton).Text
                        par = CType(Session("PARAMETROS"), IQ_Parametros)
                        par.TecCodigo = CInt(cuanti)
                        CargarParametrosSubgrilla(e.CommandArgument, gvNacionales)
                        par = CType(Session("PARAMETROS"), IQ_Parametros)
                        If chbObserver.Checked = True Then
                            If Request.QueryString("ACCION") IsNot Nothing Then
                                Response.Redirect("CostosJBExternoObserver.aspx?JOBBOOK=" & iqLbljobBook.Text & "&ACCION=" & Request.QueryString("ACCION"))
                            Else
                                Response.Redirect("CostosJBExternoObserver.aspx?JOBBOOK=" & iqLbljobBook.Text)
                            End If
                        Else
                            If Request.QueryString("ACCION") IsNot Nothing Then
                                Response.Redirect("CostosJBExterno.aspx?JOBBOOK=" & iqLbljobBook.Text & "&ACCION=" & Request.QueryString("ACCION"))
                            Else
                                Response.Redirect("CostosJBExterno.aspx?JOBBOOK=" & iqLbljobBook.Text)
                            End If
                        End If

                    End If


            End Select

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try

    End Sub

    Private Sub CargarParametrosSubgrilla(ByVal r As Integer, ByVal gv As GridView)

        Try
            Dim p As New IQ_Parametros
            p.IdPropuesta = CType(Session("PARAMETROS"), IQ_Parametros).IdPropuesta
            p.ParAlternativa = CType(Session("PARAMETROS"), IQ_Parametros).ParAlternativa
            p.TecCodigo = CType(Session("PARAMETROS"), IQ_Parametros).TecCodigo
            p.ParUnidad = CType(Session("PARAMETROS"), IQ_Parametros).ParUnidad

            p.ParNacional = CInt(gv.Rows(r).Cells(1).Text)
            p.MetCodigo = CInt((gv.Rows(r).Cells(3).Text))
            Session("PARAMETROS") = p

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOk.Click

        Try
            Dim Mensaje As String

            lkb1_ModalPopupExtender.Show()
            Select Case hfTecnica.Value
                Case "200"
                    Mensaje = ValidarCati(0)
                    If Mensaje = "" Then
                        'Guardamos
                        GuardarCati(7)

                        'LimpiarControles(panel100)
                    Else
                        ShowNotification(Mensaje, WebMatrix.ShowNotifications.InfoNotification)
                    End If

                Case "300"
                    Mensaje = ValidarOnline(0)
                    If Mensaje = "" Then
                        If gvOnLineMuestra.Rows.Count > 0 Then
                            'Guardamos cara a cara 
                            Dim par As IQ_Parametros
                            par = Session("PARAMETROS")
                            'se deben guardar los nuevos valores y fectar los  calculos
                            EfectuarCalculosOnLIne(par, 1)
                            CargarCuantitativos(par)

                        Else
                            ShowNotification("Ingrese la muestra en el boton agregar antes de proceder !", WebMatrix.ShowNotifications.InfoNotification)
                        End If

                    Else
                        ShowNotification(Mensaje, WebMatrix.ShowNotifications.InfoNotification)
                    End If
                Case "100"
                    Mensaje = ValidarCaraCara(1)
                    If Mensaje = "" Then
                        If CCGvMuestra.Rows.Count > 0 Then
                            'Guardamos cara a cara 
                            Dim par As IQ_Parametros
                            par = Session("PARAMETROS")
                            'se deben guardar los nuevos valores y fectar los  calculos
                            GuardarCaraCara(7, Nothing)
                            CargarCuantitativos(par)

                        Else
                            ShowNotification("Ingrese las ciudades de la muestra!", WebMatrix.ShowNotifications.InfoNotification)
                        End If

                    Else
                        ShowNotification(Mensaje, WebMatrix.ShowNotifications.InfoNotification)
                    End If

                Case "600"
                    'sesiones
                    Mensaje = ValidarSesiones(0)
                    If Mensaje = "" Then
                        Dim par As IQ_Parametros
                        par = Session("PARAMETROS")
                        If gvSesionesMuestra.Rows.Count > 0 Then

                            EfectuarCalculoSesiones(par)
                            CargarCuantitativos(par)
                        Else
                            ShowNotification("Ingrese la muestra!", WebMatrix.ShowNotifications.InfoNotification)
                        End If

                    Else
                        ShowNotification(Mensaje, WebMatrix.ShowNotifications.InfoNotification)
                    End If

                Case "700"
                    'entrevistas
                    Mensaje = ValidarEntrevistas(0)
                    If Mensaje = "" Then
                        Dim par As IQ_Parametros
                        par = Session("PARAMETROS")

                        If gvEntrevistasMuestra.Rows.Count > 0 Then
                            EfectuarCalculosEntrevistas(par)
                            CargarCuantitativos(par)
                        Else
                            ShowNotification("Ingrese la muestra!", WebMatrix.ShowNotifications.InfoNotification)
                        End If

                    Else
                        ShowNotification(Mensaje, WebMatrix.ShowNotifications.InfoNotification)
                    End If

                Case "999"
                    'Cuando es la pantalla de actvidades subcontratadas
                    GurardarActSub()
                    Dim par As IQ_Parametros
                    par = Session("PARAMETROS")
                    CargarCuantitativos(par)


            End Select
            GuardarDatosGenerales()


        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub


    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click


        Try

            Dim Mensaje As String
            lkb1_ModalPopupExtender.Show()
            Select Case hfTecnica.Value
                Case "200"
                    Mensaje = ValidarCati(0)
                    If Mensaje = "" Then
                        GuardarCati(0)
                        CargarCuantitativos(CType(Session("PARAMETROS"), IQ_Parametros))
                        UpdatePanel2.Update()
                        lkb1_ModalPopupExtender.Hide()
                    Else
                        ShowNotification(Mensaje, WebMatrix.ShowNotifications.ErrorNotification)
                    End If


                Case "100"
                    Mensaje = ValidarCaraCara(1)
                    If Mensaje = "" Then
                        If CCGvMuestra.Rows.Count > 0 Then
                            GuardarCaraCara(1, Nothing)
                            CargarCuantitativos(CType(Session("PARAMETROS"), IQ_Parametros))
                            UpdatePanel2.Update()
                            lkb1_ModalPopupExtender.Hide()
                        Else
                            ShowNotification("Ingrese la muestra !", WebMatrix.ShowNotifications.ErrorNotification)
                        End If

                    Else
                        ShowNotification(Mensaje, WebMatrix.ShowNotifications.ErrorNotification)
                    End If


                Case "300"
                    Mensaje = ValidarOnline(0)
                    If Mensaje = "" Then
                        If gvOnLineMuestra.Rows.Count > 0 Then
                            GuardarOnLIne(CType(Session("PARAMETROS"), IQ_Parametros), 2)
                            CargarCuantitativos(CType(Session("PARAMETROS"), IQ_Parametros))
                            UpdatePanel2.Update()
                            lkb1_ModalPopupExtender.Hide()
                        Else
                            ShowNotification("Ingrese la muestra !", WebMatrix.ShowNotifications.ErrorNotification)
                        End If

                    Else
                        ShowNotification(Mensaje, WebMatrix.ShowNotifications.ErrorNotification)
                    End If


                Case "600"
                    Mensaje = ValidarSesiones(0)
                    If Mensaje = "" Then

                        If gvSesionesMuestra.Rows.Count > 0 Then
                            GuardarSesionesGrupo(CType(Session("PARAMETROS"), IQ_Parametros), 2)
                            CargarCuantitativos(CType(Session("PARAMETROS"), IQ_Parametros))
                            UpdatePanel2.Update()
                            lkb1_ModalPopupExtender.Hide()
                        Else
                            ShowNotification("Ingrese la muestra !", WebMatrix.ShowNotifications.ErrorNotification)
                        End If

                    Else
                        ShowNotification(Mensaje, WebMatrix.ShowNotifications.ErrorNotification)
                    End If

                Case "700"
                    Mensaje = ValidarEntrevistas(0)
                    If Mensaje = "" Then
                        If gvEntrevistasMuestra.Rows.Count > 0 Then
                            GUardarEntrevistas(CType(Session("PARAMETROS"), IQ_Parametros), 2)
                            CargarCuantitativos(CType(Session("PARAMETROS"), IQ_Parametros))
                            UpdatePanel2.Update()
                            lkb1_ModalPopupExtender.Hide()
                        Else
                            ShowNotification("Ingrese la muestra !", WebMatrix.ShowNotifications.ErrorNotification)
                        End If

                    Else
                        ShowNotification(Mensaje, WebMatrix.ShowNotifications.ErrorNotification)
                    End If


                Case "999"
                    'Cuando es la pantalla de actvidades suncontratadas

                    GurardarActSub()
                    Dim P As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)
                    'DEBEMOS EFECTUAR LOS CALCULOS DE CADA  TECNICA NUEVAMENTE  PUES YA EXISTEN NUEVAS ACTIVIDADES SUBCONTRATADAS
                    RecalcularCostosActSub(P, 7)
                    CargarCuantitativos(P)
                    UpdatePanel2.Update()
                    lkb1_ModalPopupExtender.Hide()

            End Select

            GuardarDatosGenerales()

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try

    End Sub

    Private Sub GuardarDatosGenerales()
        Try

            If CInt(txtDiasCampo.Text) >= 1 Then
                Dim D As New IQ_DatosGeneralesPresupuesto
                Dim PAR1 As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)
                D.IdPropuesta = PAR1.IdPropuesta
                D.ParAlternativa = PAR1.ParAlternativa
                D.Descripcion = txtDescripcion.Text
                D.Observaciones = txtObservaciones.Text
                D.DiasDiseno = If(txtDiasDiseno.Text = "", 0, CInt(txtDiasDiseno.Text))
                D.DiasCampo = If(txtDiasCampo.Text = "", 0, CInt(txtDiasCampo.Text))
                D.DiasInformes = If(txtDiasInformes.Text = "", 0, CInt(txtDiasInformes.Text))
                D.DiasProcesamiento = If(txtDiasProceso.Text = "", 0, CInt(txtDiasProceso.Text))
                D.Anticipo = If(lblAnticipo.Text = "", 0, CInt(lblAnticipo.Text))
                D.Plazo = If(lblplazo.Text = "", 0, CInt(lblplazo.Text))
                D.Saldo = If(lblSaldo.Text = "", 0, CInt(lblSaldo.Text))
                D.NumeroMediciones = CInt(txtNumMediciones.Text)
                D.MesesMediciones = CInt(txtMesesMediciones.Text)
                If chbObserver.Checked = True Then D.TipoPresupuesto = 2 Else D.TipoPresupuesto = 1

                'Obtenemos la tasa que se encuentre en parametros generales para el dia
                D.TasaCambio = _general.ObtenerTasaCambioDia()
                _Cati.InsertarDatosGenerales(D)

            Else
                ShowNotification("LOS DIAS DE CAMPO DEBEN SER MAYORES A  0.", WebMatrix.ShowNotifications.ErrorNotification)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub



    Private Sub RecalcularCostosActSub(ByVal p As IQ_Parametros, ByVal Calcular As Integer)
        Try
            Dim CalculoAuto As Integer
            If Calcular = 7 Then
                CalculoAuto = 1
            Else
                CalculoAuto = 0
            End If


            Select Case p.TecCodigo
                'CARA CARA 
                Case "100"

                    EfectuarCaculosCaraCara(p, CalculoAuto)


                Case "200"


                    EfectuarCalulosCati(p, CalculoAuto)

                Case "300"


                    EfectuarCalculosOnLIne(p, 1)

                Case "600"


                    EfectuarCalculoSesiones(p)

                Case "700"

                    EfectuarCalculosEntrevistas(p)

            End Select

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        lkb1_ModalPopupExtender.Hide()

    End Sub

    Private Sub CargarCuantitativos(ByVal par As IQ_Parametros)
        Try

            gvCuanti.DataSource = _Cati.ObtenerCuantitativos(par)
            gvCuanti.DataBind()



        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try


    End Sub

    Private Sub CargarMetodologias(ByVal lst As DropDownList, ByVal T As Integer)

        lst.DataSource = _Cati.OnbtenerMetodologias(T)
        lst.DataTextField = "MetNombre"
        lst.DataValueField = "MetCodigo"
        lst.DataBind()

    End Sub

    Private Sub ObtenerQuerystring()
        'se debe cargar la propuesta dependiendo de la opcion ya sea para creacion o actualizacion.
        'USUARIO: Usario actual de la aplicacion
        'PROPUESTAID: NUmero de lapropuesta
        'ALTERNATIVA : Numero de alternativa generada en  presupuestos
        'ACCION: Accion a realizar (creacion de un nuevo presupuesto, modificacion , creacion de alternativa)
        'URLRETURN: url de retorno
        Dim par As New IQ_Parametros
        par.Usuario = CDec(Session("IDUsuario"))
        If Request.QueryString("IDPROPUESTA") IsNot Nothing Then
            idPropuesta = Request.QueryString("idPropuesta")
            par.IdPropuesta = CLng(idPropuesta)
        End If


        If Request.QueryString("ALTERNATIVA") IsNot Nothing Then
            Alternativa = Request.QueryString("ALTERNATIVA")
            par.ParAlternativa = CInt(Alternativa)
        Else
            par.ParAlternativa = 1
        End If

        If Request.QueryString("ACCION") IsNot Nothing Then
            Accion = Request.QueryString("ACCION")

        End If

        If Request.QueryString("URLRETURN") IsNot Nothing Then
            UrlReturn = Request.QueryString("URLRETURN")
        End If

        If Request.QueryString("NACIONAL") IsNot Nothing Then
            Nacional = Request.QueryString("NACIONAL")
            par.ParNacional = Convert.ToBoolean(CInt(Nacional))

        End If

        lblPropuesta.Text = par.IdPropuesta
        lblAlternativa.Text = par.ParAlternativa

        Session("PARAMETROS") = par

    End Sub

    'Botones de la venta de modificacion del gross margin



    'TipoCalculo (1: cambo de gross margin, 2: cambio de gm de operaciones y de unidad 
    Private Function AjustarGrossMargin(ByVal TipoCalculo As Integer) As Boolean
        Try
            Dim valido As Boolean = True

            lkgm_ModalPopupExtender.Show()
            Dim GM As String = ""
            Dim valorLimiteAprobacionGerente, gmCalculado, gmOpe, gmUni As Decimal
            Dim par As IQ_Parametros
            Dim GMOPS As Double = _general.ObtenerParametrosGenerales(551)

            par = CType(Session("PARAMETROS"), IQ_Parametros)
            valorLimiteAprobacionGerente = _general.ObtenerTasaCambioDia() * _general.ObtenerValorTotalXalternativa(par)
            Dim ValorVenta As Double? = _Cati.ObtenerParametros(par).ParValorVenta
            Dim valTope As Double = hfTope.Value
            If ValorVenta > 150000000 And ValorVenta < 350000000 Then valTope = valTope + 1
            If ValorVenta > 50000000 And ValorVenta <= 150000000 Then valTope = valTope + 2.5
            If ValorVenta <= 50000000 Then valTope = valTope + 5


            If Request.QueryString("GMU") IsNot Nothing Then
                GM = Request.QueryString("GMU")
            End If

            If ((txtValorVentaSimular.Text <> "") And TipoCalculo = 1) Or ((txtNuevoGM.Text <> "" Or txtGMOpera.Text <> "") And TipoCalculo = 2) Then
                '1. debemos valdar que e sun valor numerico
                '2. Cuand se aumenta el valor se debe permitir
                '3. cuando se disminuye el valor se debe pedir la clave para disminuirlo.
                'el tipo de calculo indica  desde que lugar se ejecuta el ambio de gross, 1. un presupuesto, 2. general
                If hfTipoCalculo.Value = "1" Then


                    If TipoCalculo = 1 Then
                        par.ParGrossMargin = _GM.SimularGM(par, CDec(txtValorVentaSimular.Text), 1) * 100
                    ElseIf TipoCalculo = 2 Then
                        par.ParGrossMargin = If((txtNuevoGM.Text.Trim = String.Empty), CDec(lblgmActual.Text), CDec(txtNuevoGM.Text))
                    End If
                    ' par.ParGrossMargin = CDbl(txtNuevoGM.Text)
                    Dim flag As Boolean = False
                    If IsNumeric(txtGMOpera.Text) Then
                        If (CDec(txtGMOpera.Text) / 100) < GMOPS Then
                            flag = True
                        End If
                    End If
                    If (par.ParGrossMargin < CDbl(valTope)) Or flag = True Then
                        'ENVIAMOS UNA NOTIFICACION POR CORREO A LA PERSONA AUTORIZADA A EFECTUAR ESTA OPERACION , IGUALMENTE DESPLEGAMOS  UNA NOTIFICACION 
                        'INFORMANDO AL USUARIO QUE YA SE ENVIO  EL CORREO A LA PERSONA INDICADA , SE DEBE ANEXAR EN EL CORREO EL TIPO DE CALCULO A EFECTUAR (1,2)
                        'ASI COMO LOS RESPECTIVOS DATOS DEL PRESUPUESTO, PARA MODIFICAR UN PRESUPUESTO UNICO SE DEBEN ENVIAR TODOS LOS DATOS DE (IDPROPUESTA, ALTERVATIVA, NACIONALES,METODOLOGIA)
                        'PARA EL CASO 2 , SE DEBE ENVIAR UNICAMENTE IDPROPUESTA Y ALTERNATIVA PUES SE MODIFICAN TODOS LOS PRESUPUESTOS INVOLUCRADOS EN LA ALTERNATIVA

                        If gmTxtContrasena.Visible = False Then
                            gmTxtContrasena.Visible = True
                            lblContrasena.Visible = True
                            pnNotificacion.Visible = True
                            valido = False
                            ShowNotification("DIGITE EL PASSWORD AUTORIZADO.", WebMatrix.ShowNotifications.ErrorNotification)

                        Else
                            Dim cla = New US.Usuarios
                            Dim password As String = ""
                            password = Cifrado(1, 2, gmTxtContrasena.Text, "Ipsos*23432_2013", "Ipsos*23432_2013")

                            Dim UsuAutorizado As Boolean = _GM.ValidarUsuarioAutorizado(CType(Session("PARAMETROS"), IQ_Parametros).ParUnidad, CDec(Session("IDUsuario")))
                            Dim resul As Long = cla.VerificarLoginXId(CDec(Session("IDUsuario")), password)

                            If gmTxtContrasena.Text <> "" And Not (resul = -1) And UsuAutorizado = True Then

                                If TipoCalculo = 1 Then
                                    gmCalculado = _GM.SimularGM(par, CDec(txtValorVentaSimular.Text), 0)
                                    CargarJBI(par, -1, gmCalculado, False)
                                    CargarJBE(par, -1, gmCalculado, False)

                                ElseIf TipoCalculo = 2 Then
                                    If txtGMOpera.Text = String.Empty Then
                                        gmOpe = -1
                                    Else
                                        gmOpe = CDec(txtGMOpera.Text) / 100
                                    End If
                                    If txtNuevoGM.Text = String.Empty Then
                                        gmUni = -1
                                    Else
                                        gmUni = CDec(txtNuevoGM.Text) / 100
                                    End If
                                    GM = _GM.SimularVenta(par, gmUni, gmOpe, 0)
                                    CargarJBI(par, gmOpe, gmUni, False)
                                    CargarJBE(par, gmOpe, gmUni, False)
                                End If

                                CargarCuantitativos(CType(Session("PARAMETROS"), IQ_Parametros))
                                UpdatePanel2.Update()
                                ' lkgm_ModalPopupExtender.Hide()
                            Else
                                valido = False
                                ShowNotification("LA CONTRASENA NO COINCIDE, O SU USUARIO NO ESTA AUTORIZADO PARA REALIZAR ESTA TRANSACCION!!", WebMatrix.ShowNotifications.ErrorNotification)
                            End If
                        End If

                    Else
                        gmTxtContrasena.Visible = False
                        gmTxtContrasena.Visible = False
                        lblContrasena.Visible = False
                        pnNotificacion.Visible = False

                        If TipoCalculo = 1 Then
                            gmCalculado = _GM.SimularGM(par, CDec(txtValorVentaSimular.Text), 0)
                        ElseIf TipoCalculo = 2 Then
                            If txtGMOpera.Text = String.Empty Then
                                gmOpe = -1
                            Else
                                gmOpe = CDec(txtGMOpera.Text) / 100
                            End If

                            If txtNuevoGM.Text = String.Empty Then
                                gmUni = -1
                            Else
                                gmUni = CDec(txtNuevoGM.Text) / 100
                            End If
                            gmCalculado = _GM.SimularVenta(par, gmUni, gmOpe, 0)
                        End If
                        CargarCuantitativos(CType(Session("PARAMETROS"), IQ_Parametros))
                        UpdatePanel2.Update()
                        'lkgm_ModalPopupExtender.Hide()
                    End If

                ElseIf hfTipoCalculo.Value = "2" Then
                    '1. se deben filtrar de a tabal de parametrso todos los presupuestos  de una aternativa
                    '2. se debe actualizar e gross amrgin de todos estos presupuestos
                    '3. refrescar  la grilla  princial
                    If CDbl(txtNuevoGM.Text) < CDbl(valTope) Then

                        If gmTxtContrasena.Visible = False Then
                            gmTxtContrasena.Visible = True
                            lblContrasena.Visible = True
                            pnNotificacion.Visible = True
                            valido = False
                            ShowNotification("DIGITE EL PASSWORD AUTORIZADO.", WebMatrix.ShowNotifications.InfoNotification)
                        Else
                            Dim cla = New US.Usuarios
                            Dim password As String = ""
                            password = Cifrado(1, 2, gmTxtContrasena.Text, "Ipsos*23432_2013", "Ipsos*23432_2013")

                            Dim UsuAutorizado As Boolean = _GM.ValidarUsuarioAutorizado(CType(Session("PARAMETROS"), IQ_Parametros).ParUnidad, CDec(Session("IDUsuario")))
                            Dim resul As Long = cla.VerificarLoginXId(CDec(Session("IDUsuario")), password)

                            If gmTxtContrasena.Text <> "" And Not (resul = -1) And UsuAutorizado = True Then

                                Dim p As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)
                                Dim lst As List(Of IQ_Parametros)
                                lst = _Cati.ObtenerPresupuestosModificarGM(p.ParAlternativa, p.IdPropuesta)
                                If TipoCalculo = 1 Then
                                    _Cati.CalcularGrossMarginXAlternativa(lst, CDec(txtNuevoGM.Text), TipoCalculo, CDec(txtValorVentaSimular.Text), 0, 0)
                                ElseIf TipoCalculo = 2 Then
                                    If txtGMOpera.Text = String.Empty Then
                                        gmOpe = -1
                                    Else
                                        gmOpe = CDec(txtGMOpera.Text)
                                    End If
                                    _Cati.CalcularGrossMarginXAlternativa(lst, CDec(txtNuevoGM.Text), TipoCalculo, CDec(txtValorVentaSimular.Text), CDec(txtNuevoGM.Text), gmOpe)
                                End If


                                CargarCuantitativos(p)
                                UpdatePanel2.Update()
                                lkgm_ModalPopupExtender.Hide()
                            Else
                                valido = False
                                ShowNotification("LA CONTRASENA NO COINCIDE, O SU USUARIO NO ESTA AUTRIZADO PARA REALIZAR ESTA TRANSACCION!!!!", WebMatrix.ShowNotifications.InfoNotification)
                            End If
                        End If

                    Else
                        gmTxtContrasena.Visible = False
                        gmTxtContrasena.Visible = False
                        lblContrasena.Visible = False
                        pnNotificacion.Visible = False

                        Dim p As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)
                        Dim lst As List(Of IQ_Parametros)
                        lst = _Cati.ObtenerPresupuestosModificarGM(p.ParAlternativa, p.IdPropuesta)
                        If TipoCalculo = 1 Then
                            _Cati.CalcularGrossMarginXAlternativa(lst, CDec(txtNuevoGM.Text), TipoCalculo, CDec(txtValorVentaSimular.Text), 0, 0)
                        ElseIf TipoCalculo = 2 Then
                            If txtGMOpera.Text = String.Empty Then
                                gmOpe = -1
                            Else
                                gmOpe = CDec(txtGMOpera.Text)
                            End If
                            _Cati.CalcularGrossMarginXAlternativa(lst, CDec(txtNuevoGM.Text), TipoCalculo, CDec(txtValorVentaSimular.Text), CDec(txtNuevoGM.Text), gmOpe)
                        End If

                        CargarCuantitativos(p)
                        UpdatePanel2.Update()
                        lkgm_ModalPopupExtender.Hide()
                    End If
                End If

            End If
            lkgm_ModalPopupExtender.Hide()
            ShowNotification("El cambio ha sido realizado", WebMatrix.ShowNotifications.InfoNotification)
            Return valido
        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Function

    Protected Sub gvCuanti_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCuanti.RowDataBound


        If e.Row.RowType = DataControlRowType.DataRow Then
            _TotalVenta = _TotalVenta + CDec(e.Row.Cells(6).Text)
            _TotalCostoDirecto = _TotalCostoDirecto + CDec(e.Row.Cells(5).Text)
            _TotalActSubCosto = _TotalActSubCosto + CDec(e.Row.Cells(3).Text)
            ' _TotalGrossMargin = _TotalGrossMargin + CDec(e.Row.Cells(7).Text)
            ''_TotalActSubGasto = _TotalActSubGasto + CDec(e.Row.Cells(4).Text)
            'Cargamos la grilla gvNacionales de acerdo a la tecnica que vayamos cargando
            Dim IdPropuesta As Long = CType(Session("PARAMETROS"), IQ_Parametros).IdPropuesta
            Dim Tecnica As Integer = CInt(CType(e.Row.Cells(1).Controls(0), LinkButton).Text)
            Dim gvNacionales As GridView = CType(e.Row.FindControl("gvNacionales"), GridView)
            Dim par As IQ_Parametros
            par = CType(Session("PARAMETROS"), IQ_Parametros)
            gvNacionales.DataSource = _Cati.ObtenerNacionales(par, Tecnica)
            gvNacionales.DataBind()


        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(2).Text = "VALOR EN PESOS :"
            e.Row.Cells(6).Text = _TotalVenta.ToString("C0")
            e.Row.Cells(5).Text = _TotalCostoDirecto.ToString("C0")
            e.Row.Cells(3).Text = _TotalActSubCosto.ToString("C0")
            'e.Row.Cells(7).Text = _TotalGrossMargin.ToString("N")
            ''e.Row.Cells(4).Text = _TotalActSubGasto.ToString("C0")

            ''gets the current grid to know homw maany colums we need 
            Dim grid As GridView = CType(sender, GridView)

            Dim rowFooter As New GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal)
            Dim newCells As Literal
            Dim footerCell As TableCell
            For i = 0 To grid.Rows.Count - 1
                newCells = New Literal()
                Select Case i
                    Case 2
                        newCells.Text = "VALOR EN DOLARES:"
                    Case 6
                        newCells.Text = (_TotalVenta / CDec(lblTasa.Text)).ToString("C0")

                End Select

                footerCell = New TableCell()
                footerCell.Controls.Add(newCells)
                rowFooter.Cells.Add(footerCell)
            Next

            rowFooter.Visible = True
            Me.gvCuanti.Controls(0).Controls.AddAt(gvCuanti.Controls(0).Controls.Count, rowFooter)

        End If



    End Sub

    Protected Sub btnCancelarAjusteGM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelarAjusteGM.Click
        lkgm_ModalPopupExtender.Hide()
    End Sub

#End Region

#Region "Cati"

    Private Function ValidarCati(ByVal todo As Integer) As String


        Dim Mensaje As String = ""

        If CType(UC_Producto1.FindControl("lstOferta"), DropDownList).SelectedIndex = 0 Then

            Mensaje = "Seleccione una oferta!"
            Return Mensaje
        End If


        If CType(UC_Producto1.FindControl("lstProducto"), DropDownList).SelectedIndex = 0 Then

            Mensaje = "Seleccione un producto!"
            Return Mensaje
        End If


        If txtGrupoObjetivo.Text = "" Then

            Mensaje = "Mencione el grupo objetivo!"
            Return Mensaje
        End If

        If todo = 1 Then
            If CatiLstTipoMuestra.SelectedIndex = 0 Then

                Mensaje = "Seleccione el tipo de muestra  !"
                Return Mensaje
            End If

            If CatiTxtCantidad.Text = "" Then

                Mensaje = "Digite la distribucion local!"
                Return Mensaje
            End If
        Else
            If gvMuestracati.Rows.Count = 0 Then

                Mensaje = "Asegurese de ingresar la muestra Nacional y Local!"
                Return Mensaje
            End If
        End If


        If lstIncidencia_101.SelectedIndex = 0 Then

            Mensaje = "Seleccione la incidencia!"
            Return Mensaje
        End If


        If ContarProcesos(chkProcesos) = 0 Then
            Mensaje = "Seleccione al menos un proceso!"
            Return Mensaje
        End If

        'Saber si chequeo DC, TL,Tablas,Bases
        Dim WEtapa6 As Boolean
        Dim WEtapa7 As Boolean
        Dim WEtapa8 As Boolean
        Dim WEtapa9 As Boolean
        WEtapa6 = False
        WEtapa7 = False
        WEtapa8 = False
        WEtapa9 = False
        For i = 0 To chkProcesos.Items.Count - 1
            If chkProcesos.Items(i).Selected Then
                If CInt(chkProcesos.Items(i).Value) = 6 And (CatiTxtNProcesosDC.Text = "" Or CInt(CatiTxtNProcesosDC.Text) = 0) Then
                    Mensaje = " Data Clean"
                End If
                If CInt(chkProcesos.Items(i).Value) = 7 And (CatiTxtNProcesosTL.Text = "" Or CInt(CatiTxtNProcesosTL.Text) = 0) Then
                    Mensaje = Mensaje & " Top Lines"
                End If
                If CInt(chkProcesos.Items(i).Value) = 8 And (CatiTxtNProcesosTablas.Text = "" Or CInt(CatiTxtNProcesosTablas.Text) = 0) Then
                    Mensaje = Mensaje & " Tablas"
                End If
                If CInt(chkProcesos.Items(i).Value) = 9 And (CatiTxtNProcesosBases.Text = "" Or CInt(CatiTxtNProcesosBases.Text) = 0) Then
                    Mensaje = Mensaje & " Archivos"
                End If

                If CInt(chkProcesos.Items(i).Value) = 6 Then
                    WEtapa6 = True
                End If
                If CInt(chkProcesos.Items(i).Value) = 7 Then
                    WEtapa7 = True
                End If
                If CInt(chkProcesos.Items(i).Value) = 8 Then
                    WEtapa8 = True
                End If
                If CInt(chkProcesos.Items(i).Value) = 9 Then
                    WEtapa9 = True
                End If
            End If
        Next

        If Mensaje <> "" Then
            Mensaje = "Digite numero de procesos en : " & Mensaje
            Return Mensaje
        End If

        'Chequea si digitaron numero de procesos pero no marcaron la etapa
        If CInt(CatiTxtNProcesosDC.Text) > 0 And WEtapa6 = False Then
            Mensaje = "Debe marcar la etapa DATACLEAN si registra procesos : " & Mensaje
            Return Mensaje
        End If
        If CInt(CatiTxtNProcesosTL.Text) > 0 And WEtapa7 = False Then
            Mensaje = "Debe marcar la etapa TOPLINES si registra procesos : " & Mensaje
            Return Mensaje
        End If
        If CInt(CatiTxtNProcesosTablas.Text) > 0 And WEtapa8 = False Then
            Mensaje = "Debe marcar la etapa PROCESO si registra procesos : " & Mensaje
            Return Mensaje
        End If
        If CInt(CatiTxtNProcesosBases.Text) > 0 And WEtapa9 = False Then
            Mensaje = "Debe marcar la etapa ARCHIVOS si registra procesos : " & Mensaje
            Return Mensaje
        End If

        If CatiLstMetodologia.SelectedIndex = 0 Then
            Mensaje = "Seleccione la metodologia !"
            Return Mensaje
        End If

        If CatiLstFase.SelectedIndex = 0 Then

            Mensaje = "Seleccione una opcion  !"
            Return Mensaje
        End If

        Return Mensaje
    End Function


    Private Function ContarProcesos(ByVal chk As CheckBoxList) As Integer
        Dim i As Integer
        Dim ContarSel As Integer = 0
        For i = 0 To chk.Items.Count - 1
            If chk.Items(i).Selected = True Then
                ContarSel = ContarSel + 1
            End If

        Next

        Return ContarSel
    End Function

    Private Sub CargarMuestraCati(ByVal par As IQ_Parametros)

        Try
            gvMuestracati.DataSource = _Cati.ObtenerMuestra(par)
            gvMuestracati.DataBind()
            upGVMUestraCati.Update()
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnAgregarMueCati_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregarMueCati.Click

        Dim mensaje As String
        mensaje = ValidarCati(1)
        If mensaje = "" Then
            GuardarCati(1)
        Else
            ShowNotification(mensaje, WebMatrix.ShowNotifications.InfoNotification)
        End If

    End Sub

    Private Sub GuardarCati(ByVal guardar As Integer)
        Try

            'Creamos la rutina de insercio del la tecnca CATI
            '1. Se debbe  llenarel objeto parametros
            Dim Par As New IQ_Parametros
            Par = CType(Session("PARAMETROS"), IQ_Parametros)
            Par.Usuario = CDec(Session("IDUsuario"))
            Par.ParFechaCreacion = DateTime.Now
            Par.MetCodigo = CInt(CatiLstMetodologia.SelectedValue)
            Par.ParNacional = CInt(CatiLstFase.SelectedItem.Value)

            If CatichkAñoActual.Checked = True And CatichkAñoSiguiente.Checked = False Then
                Par.ParAñoSiguiente = False
            Else
                If CatichkAñoActual.Checked = False And CatichkAñoSiguiente.Checked = True Then
                    Par.ParAñoSiguiente = True
                Else
                    If CatichkAñoActual.Checked = False And CatichkAñoSiguiente.Checked = False Then
                        Par.ParAñoSiguiente = True
                    Else
                        If CatichkAñoActual.Checked = True And CatichkAñoSiguiente.Checked = True Then
                            Par.ParAñoSiguiente = True
                        End If
                    End If
                End If
            End If

            'Par.ParUnidad = 22100 'Se debe extraer el valor de la propuesta ????
            Par.ParNProcesosDC = CInt(CatiTxtNProcesosDC.Text)
            Par.ParNProcesosTopLines = CInt(CatiTxtNProcesosTL.Text)
            Par.ParNProcesosTablas = CInt(CatiTxtNProcesosTablas.Text)
            Par.ParNProcesosBases = CInt(CatiTxtNProcesosBases.Text)
            Par.Pr_ProductCode = CType(UC_Producto1.FindControl("LstProducto"), DropDownList).SelectedValue
            Par.Pr_Offeringcode = CType(UC_Producto1.FindControl("LstOferta"), DropDownList).SelectedValue
            Par.ParGrupoObjetivo = txtGrupoObjetivo.Text
            Par.TecCodigo = CInt(hfTecnica.Value)
            Par.ParContactosNoEfectivos = If(txtMarcNoEfectivas.Text = "", 0, CInt(txtMarcNoEfectivas.Text))
            Par.ParIncidencia = CInt(lstIncidencia_101.SelectedValue)
            'Debemos insertar los valores de productividad , contactos no efectivos  y actividades subcontratadas
            Par.ParProductividad = If(txtProductividad.Text = "", 0.0, CDec(txtProductividad.Text))
            Par.ParContactosNoEfectivos = If(txtMarcNoEfectivas.Text = "", 0.0, CInt(txtMarcNoEfectivas.Text))
            Par.ParObservaciones = CatiTxtObservaciones.Text
            Par.TipoProyecto = 1
            Par.ParNomPresupuesto = lblNomPropuesta.Text

            'llenamos la muestra 
            Dim lstMuestras As New List(Of IQ_Muestra_1)
            If guardar = 1 Then

                Dim Mu2 As New IQ_Muestra_1()
                Mu2.IdPropuesta = Par.IdPropuesta
                Mu2.ParAlternativa = Par.ParAlternativa
                Mu2.ParNacional = Par.ParNacional
                Mu2.MetCodigo = CInt(CatiLstMetodologia.SelectedValue)
                Mu2.MuCantidad = CInt(CatiTxtCantidad.Text)
                Mu2.MuIdentificador = (CatiLstTipoMuestra.SelectedValue)
                lstMuestras.Add(Mu2)
            End If

            'Agregamos los procesos 
            Dim lstProc As New List(Of IQ_ProcesosPresupuesto)

            For i = 0 To chkProcesos.Items.Count - 1

                If chkProcesos.Items(i).Selected Then
                    Dim P1 = New IQ_ProcesosPresupuesto()
                    P1.IdPropuesta = Par.IdPropuesta
                    P1.ParAlternativa = Par.ParAlternativa
                    P1.ParNacional = Par.ParNacional
                    P1.MetCodigo = CInt(CatiLstMetodologia.SelectedValue)
                    P1.Porcentaje = 0
                    P1.ProcCodigo = CInt(chkProcesos.Items(i).Value)
                    lstProc.Add(P1)
                End If

            Next

            'Agregamos las preguntas
            Dim Preg As New IQ_Preguntas
            Preg.IdPropuesta = Par.IdPropuesta
            Preg.ParAlternativa = Par.ParAlternativa
            Preg.ParNacional = Par.ParNacional
            Preg.MetCodigo = CInt(CatiLstMetodologia.SelectedValue)
            Preg.PregAbiertas = CType(UC_Producto1.FindControl("AbiertasReal"), TextBox).Text
            Preg.PregAbiertasMultiples = CType(UC_Producto1.FindControl("AbiertasMultReal"), TextBox).Text
            Preg.PregCerradas = CType(UC_Producto1.FindControl("CerradasReal"), TextBox).Text
            Preg.PregCerradasMultiples = CType(UC_Producto1.FindControl("CerradasMultReal"), TextBox).Text
            Preg.PregOtras = CType(UC_Producto1.FindControl("OtrosReal"), TextBox).Text
            Preg.PregDemograficos = CType(UC_Producto1.FindControl("DemoReal"), TextBox).Text

            Par.ParTiempoEncuesta = If(CType(UC_Producto1.FindControl("txtDuracion"), TextBox).Text = "", 0, CInt(CType(UC_Producto1.FindControl("txtDuracion"), TextBox).Text))
            _Cati.InsertarPresupuesto(Par, lstMuestras, lstProc, Preg)

            CargarMuestraCati(Par)
            CatiLblTotalMuestra.Text = _Cati.TotalizarMuestra(Par)

            If guardar = 7 Then
                EfectuarCalulosCati(Par, 1)
            Else
                EfectuarCalulosCati(Par, 0)
            End If

            Par = _Cati.ObtenerParametros(Par)
            txtProductividad.Text = Par.ParProductividad.ToString()
            Dim NumEncuestadores As Decimal
            NumEncuestadores = _Cati.TotalizarMuestra(Par) / CInt(txtDiasCampo.Text) / Par.ParProductividad
            CatiLblNumEncuestadores.Text = NumEncuestadores.ToString("N2")



            txtMarcNoEfectivas.Text = Par.ParContactosNoEfectivos.ToString()



        Catch ex As Exception
            Throw ex
        End Try



    End Sub

    Private Sub EfectuarCalulosCati(ByVal par As IQ_Parametros, ByVal CalculoAuto As Integer)
        Try

            If par.ParProductividad = 0 Or CalculoAuto = 1 Then
                _Cati.CalcularProductividad(par)
            End If
            par.ParCostoDirecto = _Cati.ObtenerCostoDirecto(par)
            _Cati.calcularActSubcontratadas(par)
            par.ParValorVenta = _CaraCara.ValorVenta(par)
            'Obtenemos los valores que no aplican para el gross margin pues estos tiene su propio porcentaje y los  restamos las actividades  subcontratadas
            Dim ValorActividadesNoGM As Decimal
            ValorActividadesNoGM = _Cati.ActividadesNoAplicanGM(par)
            Dim ValorActividadesNoGMConAdmon As Decimal

            ValorActividadesNoGMConAdmon = (ValorActividadesNoGM * 1.1)
            ' par.ParGrossMargin = ((par.ParValorVenta - ValorActividadesNoGMConAdmon) - (par.ParCostoDirecto + ((par.ParActSubGasto) - ValorActividadesNoGM))) / (par.ParValorVenta - ValorActividadesNoGMConAdmon)
            par.ParGrossMargin = ((par.ParValorVenta) - (par.ParCostoDirecto + ((par.ParActSubGasto)))) / (par.ParValorVenta)
            Session("PARAMETROS") = par

            'actualizams unicamente el gross amrgin y el valor de venta  pues los demas ya han sido actuaizados desde el procedimiento
            _Cati.InsertarGrossMargin(par)


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CatiCargarIncidencia(ByVal lst As DropDownList)
        Dim i As Integer
        Dim li As ListItem
        For i = 0 To 100 Step 1
            If i = 0 Then
                li = New ListItem With {.Text = "Seleccione...", .Value = "0"}

                lst.Items.Add(li)
            Else
                li = New ListItem With {.Text = i.ToString() & "%", .Value = i.ToString()}

                lst.Items.Add(li)
            End If
        Next

    End Sub

    Private Sub CargarDatosCati()
        Try
            Dim par As IQ_Parametros

            Dim lstProc As List(Of IQ_ProcesosPresupuesto)
            Dim Preg As IQ_Preguntas
            par = CType(Session("PARAMETROS"), IQ_Parametros)
            'par.ParNacional = CBool(CatiRbNacional.SelectedItem.Value)
            par = _Cati.ObtenerParametros(par)
            Preg = _Cati.ObtenerPreguntas(par)

            lstProc = _Cati.ObtenerProcesos(par)
            CType(UC_Producto1.FindControl("LstOferta"), DropDownList).SelectedIndex = CType(UC_Producto1.FindControl("LstOferta"), DropDownList).Items.IndexOf(CType(UC_Producto1.FindControl("LstOferta"), DropDownList).Items.FindByValue(par.Pr_Offeringcode))
            UC_Producto1.cargarProductos()
            CType(UC_Producto1.FindControl("LstProducto"), DropDownList).SelectedIndex = CType(UC_Producto1.FindControl("LstProducto"), DropDownList).Items.IndexOf(CType(UC_Producto1.FindControl("LstProducto"), DropDownList).Items.FindByValue(par.Pr_ProductCode))
            UC_Producto1.CargarPreguntasPropuestas()
            CatiLstMetodologia.SelectedIndex = CatiLstMetodologia.Items.IndexOf(CatiLstMetodologia.Items.FindByValue(par.MetCodigo))
            CargarTipoMuestra(CatiLstTipoMuestra, CInt(CatiLstMetodologia.SelectedValue))

            If par.ParAñoSiguiente = True Then
                CatichkAñoActual.Checked = False
                CatichkAñoSiguiente.Checked = True
            Else
                CatichkAñoActual.Checked = True
                CatichkAñoSiguiente.Checked = False
            End If

            txtGrupoObjetivo.Text = par.ParGrupoObjetivo
            'preguntas
            CType(UC_Producto1.FindControl("AbiertasReal"), TextBox).Text = Preg.PregAbiertas
            CType(UC_Producto1.FindControl("AbiertasMultReal"), TextBox).Text = Preg.PregAbiertasMultiples
            CType(UC_Producto1.FindControl("CerradasReal"), TextBox).Text = Preg.PregCerradas
            CType(UC_Producto1.FindControl("CerradasMultReal"), TextBox).Text = Preg.PregCerradasMultiples
            CType(UC_Producto1.FindControl("OtrosReal"), TextBox).Text = Preg.PregOtras
            CType(UC_Producto1.FindControl("DemoReal"), TextBox).Text = Preg.PregDemograficos
            CType(UC_Producto1.FindControl("TxtDuracion"), TextBox).Text = par.ParTiempoEncuesta.ToString()

            'CatiTxtCantidad.Text = lstMuestra.Item(0).MuCantidad
            'txtNacional.Text = lstMuestra.Item(1).MuCantidad
            CatiCargarIncidencia(lstIncidencia_101)
            lstIncidencia_101.SelectedIndex = lstIncidencia_101.Items.IndexOf(lstIncidencia_101.Items.FindByValue(par.ParIncidencia))
            CatiTxtNProcesosDC.Text = par.ParNProcesosDC
            CatiTxtNProcesosTL.Text = par.ParNProcesosTopLines
            CatiTxtNProcesosTablas.Text = par.ParNProcesosTablas
            CatiTxtNProcesosBases.Text = par.ParNProcesosBases
            txtProductividad.Text = par.ParProductividad
            txtMarcNoEfectivas.Text = par.ParContactosNoEfectivos
            CatiTxtObservaciones.Text = If(par.ParObservaciones Is Nothing, "", par.ParObservaciones)
            'Procesos

            For i = 0 To chkProcesos.Items.Count - 1
                If _Cati.ExisteProceso(lstProc, CInt(chkProcesos.Items(i).Value)) Then
                    chkProcesos.Items(i).Selected = True
                End If
            Next
            CargarMuestraCati(par)
            CatiLblTotalMuestra.Text = _Cati.TotalizarMuestra(par)

            'diciembre 26 de 2013 - ajuste 
            Dim NumEncuestadores As Decimal
            If par.ParProductividad > 0 Then
                NumEncuestadores = _Cati.TotalizarMuestra(par) / CInt(txtDiasCampo.Text) / par.ParProductividad
            End If

            CatiLblNumEncuestadores.Text = NumEncuestadores.ToString("N2")

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub



#End Region



#Region "Actividades subcontratadas"


    Private Sub CargarActividades(ByVal Tecnica As Integer)
        Try
            Dim par As IQ_Parametros
            par = CType(Session("PARAMETROS"), IQ_Parametros)
            gvActSubCont.DataSource = _ActSub.ObtenerActividadesSubcontratadas(par)
            gvActSubCont.DataBind()


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GurardarActSub()

        Try

            'Insertamos solamente las actividades cuyo valor sea mayor a cero en la grilla
            Dim R As GridViewRow
            Dim par As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)
            Dim lstAct As New List(Of IQ_CostoActividades)
            For Each R In gvActSubCont.Rows
                Dim Act As New IQ_CostoActividades
                'OJO CAMBIO Carlos
                If CDec(CType(R.Cells(3).FindControl("txtValorAct"), TextBox).Text) > -1 Then
                    Act.IdPropuesta = par.IdPropuesta
                    Act.ParAlternativa = par.ParAlternativa
                    Act.MetCodigo = par.MetCodigo
                    Act.ParNacional = par.ParNacional
                    Act.CaCosto = CDec(CType(R.Cells(3).FindControl("txtValorAct"), TextBox).Text)
                    Act.ActCodigo = CInt(R.Cells(0).Text)
                    Act.CaUnidades = 1
                    Act.CaDescripcionUnidades = ""
                    lstAct.Add(Act)
                End If

            Next
            _ActSub.InsertarCostoActividades(lstAct)
            '_CaraCara.calcularActSubcontratadas(par)
            SubLblTotal.Text = _ActSub.TotalizarActSub(par).ToString("C0")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub gvActSubCont_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvActSubCont.RowCommand

    End Sub

    Protected Sub txtValorAct_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try


            If (CType(sender, TextBox).Text <> "") Then

                ' CType(currentRow.Cells(2).FindControl("txtValorAct"), TextBox).Text = String.Format("{0:C}", CDec(CType(sender, TextBox).Text))
                CType(sender, TextBox).Text = String.Format("{0:C}", CDec(CType(sender, TextBox).Text))
                'CType(sender, TextBox).Focus()
                upActSub.Update()


            End If
        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try



    End Sub

#End Region


#Region "Cara Cara"

    Private Sub CargarDepartamentos(ByVal lst As DropDownList, ByVal ciuPrincipales As Boolean, ByVal nacional As Boolean)

        Try

            lst.DataSource = _CaraCara.ObtenerDepartamentos(ciuPrincipales, nacional)
            lst.DataTextField = "DivDeptoNombre"
            lst.DataValueField = "DivDepto"
            lst.DataBind()

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub CCBtnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CCBtnAgregar.Click

        Try

            Dim mensaje As String = ValidarCaraCara(0)
            If mensaje = "" Then
                GuardarCaraCara(0, Nothing)
            Else
                ShowNotification(mensaje, WebMatrix.ShowNotifications.ErrorNotification)
            End If

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Private Sub EfectuarCaculosCaraCara(ByVal par As IQ_Parametros, ByVal CalculoAuto As Integer)
        Try

            Dim prueba As Double
            prueba = _CaraCara.ObtenerPrueba()
            If par.ParProductividad = 0 Or CalculoAuto = 1 Then
                par.ParProductividad = _CaraCara.CalcularPoductividad(par)
            End If

            par.ParCostoDirecto = _CaraCara.ObtenerCostoDirecto(par)
            _CaraCara.calcularActSubcontratadas(par)
            par.ParValorVenta = _CaraCara.ValorVenta(par)
            'Obtenemos los valores que no aplican para el gross margin pues estos tiene su propio porcentaje y los  restamos las actividades  subcontratadas
            Dim ValorActividadesNoGM As Decimal
            ValorActividadesNoGM = _Cati.ActividadesNoAplicanGM(par)
            Dim ValorActividadesNoGMConAdmon As Decimal

            ValorActividadesNoGMConAdmon = (ValorActividadesNoGM * 1.1)
            'par.ParGrossMargin = ((par.ParValorVenta - ValorActividadesNoGMConAdmon) - (par.ParCostoDirecto + ((par.ParActSubGasto) - ValorActividadesNoGM))) / (par.ParValorVenta - ValorActividadesNoGMConAdmon)
            par.ParGrossMargin = ((par.ParValorVenta) - (par.ParCostoDirecto + ((par.ParActSubGasto)))) / (par.ParValorVenta)
            Session("PARAMETROS") = par

            'actualizams unicamente el gross amrgin y el valor de venta  pues los demas ya han sido actuaizados desde el procedimiento
            _CaraCara.InsertarGrossMargin(par)
            CCTxtProductividad.Text = par.ParProductividad.ToString
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub CargarCiudadesMuestra(ByVal par As IQ_Parametros, ByVal gv As GridView)
        Try
            gv.DataSource = _CaraCara.ObtenerMuestra(par).Tables(0)
            gv.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub CCLstDepartamento_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CCLstDepartamento.SelectedIndexChanged
        If CCLstDepartamento.SelectedIndex > 0 Then
            CargarCiudades(CInt(CCLstDepartamento.SelectedValue), CCLstCiudad, False)
        End If
    End Sub

    Private Sub CargarCiudades(ByVal departamento As Integer, ByVal lstCiu As DropDownList, ByVal ciuPrincipal As Boolean)
        Try

            lstCiu.DataSource = _CaraCara.ObtenerCiudades(departamento, ciuPrincipal)
            lstCiu.DataTextField = "DivMuniNombre"
            lstCiu.DataValueField = "DivMunicipio"
            lstCiu.DataBind()

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Protected Sub CCLstMetodologia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CCLstMetodologia.SelectedIndexChanged

        Try
            BloquearSegunMetodologia()

            'DEBEMOS VALIDAR  SI  ESTA SELECCIONADA LA OPCION NACIONAL Y VALIDAR SI EXISTE EL PRESUPUESTO PARA CARGARLO
            If CCLstMetodologia.SelectedIndex > 0 Then
                CargarTipoMuestra(CCLstTipoMuestra, CInt(CCLstMetodologia.SelectedValue))

                If CCLstFases.SelectedIndex > -1 Then
                    Dim par As New IQ_Parametros

                    par.IdPropuesta = CType(Session("PARAMETROS"), IQ_Parametros).IdPropuesta
                    par.ParAlternativa = CType(Session("PARAMETROS"), IQ_Parametros).ParAlternativa
                    par.ParNacional = CInt(CCLstFases.SelectedValue)
                    par.MetCodigo = CInt(CCLstMetodologia.SelectedValue)

                    par.ParUnidad = CType(Session("PARAMETROS"), IQ_Parametros).ParUnidad
                    Session("PARAMETROS") = par
                    If (_Cati.ExisteCati(CType(Session("PARAMETROS"), IQ_Parametros))) Then
                        CCLstMetodologia.Enabled = False
                        CCLstFases.Enabled = False
                        CargarCaraCara()
                    Else
                        'SE DEBEN LIMPIAR LOS CONTROLES EXCEPTO LA METODOLOGIA Y Y LA OPCION DE NACIONAL
                    End If

                Else
                    ShowNotification("Seleccione la opcion Nacional/Internacional", WebMatrix.ShowNotifications.ErrorNotification)
                    CCLstMetodologia.SelectedIndex = 0
                End If

            End If
        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try

    End Sub

    Private Sub BloquearSegunMetodologia()
        Try

            If CCLstMetodologia.SelectedIndex > 0 Then
                Select Case CCLstMetodologia.SelectedValue
                    'HOGARES (Mostrar el chek de probabilistico)
                    Case "100"
                        'Habiltamos
                        CCCHKProbabilistico.Visible = True
                        PanelLocalizacion.Visible = False
                        PanelMistery.Visible = False
                        PanelIncidencia.Visible = True

                        'LOCALIZACION CENTRAL (Mostar e campo de encuestadores por punto )

                    Case "120"

                        PanelLocalizacion.Visible = True
                        CCCHKProbabilistico.Visible = False
                        PanelMistery.Visible = False
                        PanelIncidencia.Visible = True



                    Case "125"
                        PanelLocalizacion.Visible = True
                        CCCHKProbabilistico.Visible = False
                        PanelMistery.Visible = False
                        PanelIncidencia.Visible = True

                        'Mistery
                    Case "140"
                        PanelMistery.Visible = True
                        PanelIncidencia.Visible = False


                        'ESPECIALIZADO La muestra debe definirse ciudad por ciudad (Como se describe arriba) en dos niveles BAJO y ALTO
                    Case Else
                        CCCHKProbabilistico.Visible = True
                        PanelLocalizacion.Visible = False

                        PanelMistery.Visible = False
                        PanelIncidencia.Visible = True
                End Select
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function ValidarCaraCara(ByVal guardar As Integer) As String
        Dim Mensaje As String = ""

        If CType(UC_Producto2.FindControl("lstOferta"), DropDownList).SelectedIndex = 0 Then
            Mensaje = "Seleccione una oferta!"
            Return Mensaje
        End If

        If CType(UC_Producto2.FindControl("lstProducto"), DropDownList).SelectedIndex = 0 Then
            Mensaje = "Seleccione un producto!"
            Return Mensaje
        End If

        If CCTxtGrupoObj.Text = "" Then
            Mensaje = "Mencione el grupo objetivo!"
            Return Mensaje
        End If

        If CCLstMetodologia.SelectedIndex = 0 Then
            Mensaje = "Seleccione la incidencia!"
            Return Mensaje
        End If

        If ContarProcesos(CCchkProcesos) = 0 Then
            Mensaje = "Seleccione al menos un proceso!"
            Return Mensaje
        End If

        If CCLstMetodologia.SelectedIndex = 0 Then
            Mensaje = "Seleccione la metodologia !"
            Return Mensaje
        End If

        'Saber si chequeo DC, TL,Tablas,Bases
        Dim WEtapa6 As Boolean
        Dim WEtapa7 As Boolean
        Dim WEtapa8 As Boolean
        Dim WEtapa9 As Boolean
        WEtapa6 = False
        WEtapa7 = False
        WEtapa8 = False
        WEtapa9 = False
        For i = 0 To CCchkProcesos.Items.Count - 1
            If CCchkProcesos.Items(i).Selected Then
                If CInt(CCchkProcesos.Items(i).Value) = 6 AndAlso (CCTxtNumProcDC.Text = "" OrElse CInt(CCTxtNumProcDC.Text) = 0) Then
                    Mensaje = " Data Clean"
                End If
                If CInt(CCchkProcesos.Items(i).Value) = 7 AndAlso (CCTxtNumProcTL.Text = "" OrElse CInt(CCTxtNumProcTL.Text) = 0) Then
                    Mensaje = Mensaje & " Top Lines"
                End If
                If CInt(CCchkProcesos.Items(i).Value) = 8 AndAlso (CCTxtNumProcTablas.Text = "" OrElse CInt(CCTxtNumProcTablas.Text) = 0) Then
                    Mensaje = Mensaje & " Tablas"
                End If
                If CInt(CCchkProcesos.Items(i).Value) = 9 AndAlso (CCTxtNumProcBases.Text = "" OrElse CInt(CCTxtNumProcBases.Text) = 0) Then
                    Mensaje = Mensaje & " Archivos"
                End If

                If CInt(CCchkProcesos.Items(i).Value) = 6 Then
                    WEtapa6 = True
                End If
                If CInt(CCchkProcesos.Items(i).Value) = 7 Then
                    WEtapa7 = True
                End If
                If CInt(CCchkProcesos.Items(i).Value) = 8 Then
                    WEtapa8 = True
                End If
                If CInt(CCchkProcesos.Items(i).Value) = 9 Then
                    WEtapa9 = True
                End If

            End If
        Next

        If Mensaje <> "" Then
            Mensaje = "Digite numero de procesos en : " & Mensaje
            Return Mensaje
        End If

        'Chequea si digitaron numero de procesos pero no marcaron la etapa
        If CCTxtNumProcDC.Text.Trim <> "" AndAlso CInt(CCTxtNumProcDC.Text) > 0 AndAlso WEtapa6 = False Then
            Mensaje = "Debe marcar la etapa DATACLEAN si registra procesos : " & Mensaje
            Return Mensaje
        End If
        If CCTxtNumProcTL.Text.Trim <> "" AndAlso CInt(CCTxtNumProcTL.Text) > 0 AndAlso WEtapa7 = False Then
            Mensaje = "Debe marcar la etapa TOPLINES si registra procesos : " & Mensaje
            Return Mensaje
        End If
        If CCTxtNumProcTablas.Text.Trim <> "" AndAlso CInt(CCTxtNumProcTablas.Text) > 0 AndAlso WEtapa8 = False Then
            Mensaje = "Debe marcar la etapa PROCESO si registra procesos : " & Mensaje
            Return Mensaje
        End If
        If CCTxtNumProcBases.Text.Trim <> "" AndAlso CInt(CCTxtNumProcBases.Text) > 0 AndAlso WEtapa9 = False Then
            Mensaje = "Debe marcar la etapa ARCHIVOS si registra procesos : " & Mensaje
            Return Mensaje
        End If


        If CCLstIncidencia.Visible = True Then
            If CCLstIncidencia.SelectedIndex = 0 Then
                Mensaje = "Seleccione la  incidencia !"
                Return Mensaje
            End If
        End If




        'Guradar = 0 , significa que se esta aregando  desde el boton de adicion de muestra
        If guardar = 0 Then

            If CCLstDepartamento.SelectedIndex = 0 Then

                Mensaje = "Seleccione el departamento !"
                Return Mensaje
            End If

            If CCLstCiudad.SelectedIndex = 0 Then

                Mensaje = "Seleccione la ciudad  !"
                Return Mensaje
            End If

            If CCTxtCantidad.Text = "" And CCTxtCantidad.Visible = True Then
                Mensaje = "Digite la muestra . !"
                Return Mensaje
            End If

            If CCLstTipoMuestra.SelectedIndex = 0 Then
                Mensaje = "Seleccione a dificultad de la muestra !"
                Return Mensaje
            End If

        End If

        If CCLstFases.SelectedIndex = 0 Then
            Mensaje = "Seleccione una opcion !"
            Return Mensaje
        End If

        If PanelLocalizacion.Visible Then

            If CCEncXPunto.Text = "" Then
                Mensaje = "Digite los encuestadores por punto  !"
                Return Mensaje
            End If



            If CCPorIntercep.Text = "" Then
                Mensaje = "Digite el porcentaje por interceptacion !"
                Return Mensaje
            End If

            If CCPorRecluta.Text = "" Then
                Mensaje = "Digite el porcentaje por reclutamiento !"
                Return Mensaje
            End If

            If CCTxtUniProductos.Text = "" Then
                Mensaje = "Digite la unidades del producto !"
                Return Mensaje
            End If

            If CCTxtValorUniProd.Text = "" Then
                Mensaje = "Digite el valor unitario del producto !"
                Return Mensaje
            End If


            If CCLstTipoCLT.SelectedIndex = 0 Then
                Mensaje = "Seleccione  el tipo de CLT !"
                Return Mensaje
            End If

            If CCEncXPunto.Text = "" Then
                Mensaje = "Digite el  numero de encuestadores por punto. !"
                Return Mensaje
            End If

            If CCtxtAlqEquip.Text = "" Then
                Mensaje = "Digite el valor de alquiler de equipos !"
                Return Mensaje
            End If

            'extrano comportamiento
            CCtxtTotalPorcentaje.Text = CInt(CCPorIntercep.Text) + CInt(CCPorRecluta.Text)
            If CCtxtTotalPorcentaje.Text <> "100" Then
                Mensaje = "La suma de los porcentajes debe ser igual a 100 !"
                Return Mensaje
            End If

        End If

        'significa que se esta guardando desde e boton de adicionar parametros mistery 
        If guardar = 3 Then
            If PanelMistery.Visible = True Then
                If lstTipoServicio.SelectedIndex = 0 Then
                    Mensaje = "Seleccione el tipo de servicio"
                    Return Mensaje
                End If

                If lstTipoEvidencia.SelectedIndex = 0 Then
                    Mensaje = "Seleccione el tipo de evidencia"
                    Return Mensaje
                End If

                If lstEvidencia.SelectedIndex = 0 Then
                    Mensaje = "Seleccione  si tiene o no evidencia"
                    Return Mensaje
                End If

                If lstNumContactos.SelectedIndex = 0 Then
                    Mensaje = "Seleccione la cantidad de contactos"
                    Return Mensaje
                End If


                If CCTxtTiempoCritica.Text.Trim = "" Then
                    Mensaje = "Digite el tiempo de critica "
                    Return Mensaje
                End If


            End If
        End If



        Return Mensaje
    End Function

    'Private Sub GuardarCaraCara(ByVal guardar As Integer, Optional ByVal mNew As IQ_Muestra_1 = Nothing)
    Private Sub GuardarCaraCara(ByVal guardar As Integer, Optional ByVal lstmNew As List(Of IQ_Muestra_1) = Nothing)

        'habia un 0 
        Dim mensaje As String = ValidarCaraCara(guardar)

        If mensaje = "" Then

            'Agregamos cada una de las ciudades de la muestra 
            Dim par As New IQ_Parametros
            par.Usuario = CDec(Session("IDUsuario"))
            par.ParFechaCreacion = DateTime.Now
            par.IdPropuesta = CType(Session("PARAMETROS"), IQ_Parametros).IdPropuesta
            par.ParAlternativa = CType(Session("PARAMETROS"), IQ_Parametros).ParAlternativa
            par.ParUnidad = CType(Session("PARAMETROS"), IQ_Parametros).ParUnidad
            par.ParNacional = CInt(CCLstFases.SelectedValue)

            'chkAñoActual.Enabled = False
            If chkAñoActual.Checked = True And chkAñoSiguiente.Checked = False Then
                par.ParAñoSiguiente = False
            Else
                If chkAñoActual.Checked = False And chkAñoSiguiente.Checked = True Then
                    par.ParAñoSiguiente = True
                Else
                    If chkAñoActual.Checked = False And chkAñoSiguiente.Checked = False Then
                        par.ParAñoSiguiente = True
                    Else
                        If chkAñoActual.Checked = True And chkAñoSiguiente.Checked = True Then
                            par.ParAñoSiguiente = True
                        End If
                    End If
                End If
            End If

            par.Pr_ProductCode = CType(UC_Producto2.FindControl("LstProducto"), DropDownList).SelectedValue
            par.Pr_Offeringcode = CType(UC_Producto2.FindControl("LstOferta"), DropDownList).SelectedValue
            par.ParGrupoObjetivo = CCTxtGrupoObj.Text
            par.TecCodigo = CInt(hfTecnica.Value)
            If PanelIncidencia.Visible = False Then
                par.ParIncidencia = 100
            Else
                par.ParIncidencia = CInt(CCLstIncidencia.SelectedValue)
            End If

            par.MetCodigo = CInt(CCLstMetodologia.SelectedValue)
            par.ParNProcesosDC = If(String.IsNullOrEmpty(CCTxtNumProcDC.Text.Trim), 0, CInt(CCTxtNumProcDC.Text))
            par.ParNProcesosTopLines = If(String.IsNullOrEmpty(CCTxtNumProcTL.Text.Trim), 0, CInt(CCTxtNumProcTL.Text))
            par.ParNProcesosTablas = If(String.IsNullOrEmpty(CCTxtNumProcTablas.Text.Trim), 0, CInt(CCTxtNumProcTablas.Text))
            par.ParNProcesosBases = If(String.IsNullOrEmpty(CCTxtNumProcBases.Text.Trim), 0, CInt(CCTxtNumProcBases.Text))

            'Debemos insertar los valores de productividad , contactos no efectivos  y actividades subcontratadas
            par.ParProductividad = If(CCTxtProductividad.Text = "", 0.0, CDec(CCTxtProductividad.Text))
            par.ParContactosNoEfectivos = If(txtMarcNoEfectivas.Text = "", 0.0, CInt(txtMarcNoEfectivas.Text))
            par.ParEncuestadoresPunto = If(CCEncXPunto.Visible, CInt(CCEncXPunto.Text), 0)
            par.ParObservaciones = CCTxtObservaciones.Text
            par.TipoProyecto = 1
            par.ParNomPresupuesto = lblNomPropuesta.Text

            If CCEncXPunto.Visible = True Then
                par.ParEncuestadoresPunto = CInt(CCEncXPunto.Text)

            End If

            If CCCHKProbabilistico.Visible = True Then
                par.ParProbabilistico = If(CCCHKProbabilistico.Checked, True, False)
            End If

            If chkTablet.Visible = True Then
                par.ParUsaTablet = If(chkTablet.Checked, True, False)
            End If
            If ChkPapel.Visible = True Then
                par.ParUsaPapel = If(ChkPapel.Checked, True, False)
            End If

            'If CCChkDispPropio.Visible = True Then
            '    par.ParDispPropio = If(CCChkDispPropio.Checked, True, False)
            'End If

            If PanelLocalizacion.Visible Then
                par.ParPorcentajeIntercep = CInt(CCPorIntercep.Text)
                par.ParPorcentajeRecluta = CInt(CCPorRecluta.Text)
                par.ParUnidadesProducto = CInt(CCTxtUniProductos.Text)
                par.ParValorUnitarioProd = CDec(CCTxtValorUniProd.Text)
                par.ParTipoCLT = CInt(CCLstTipoCLT.SelectedValue)
                par.ParAlquilerEquipos = CDec(CCtxtAlqEquip.Text)
                par.ParApoyoLogistico = If(CCchkApoyoLogis.Checked, True, False)
                par.ParAccesoInternet = If(CCchkAccesoInter.Checked, True, False)

            End If

            'Agregamos los procesos 
            Dim lstProc As New List(Of IQ_ProcesosPresupuesto)
            For i = 0 To CCchkProcesos.Items.Count - 1
                If CCchkProcesos.Items(i).Selected Then
                    Dim P1 = New IQ_ProcesosPresupuesto()
                    P1.IdPropuesta = par.IdPropuesta
                    P1.ParAlternativa = par.ParAlternativa
                    P1.MetCodigo = par.MetCodigo
                    P1.ParNacional = par.ParNacional
                    P1.Porcentaje = 0
                    P1.ProcCodigo = CInt(CCchkProcesos.Items(i).Value)
                    lstProc.Add(P1)
                End If
            Next

            'Agregamos las preguntas
            Dim Preg As New IQ_Preguntas
            Preg.IdPropuesta = par.IdPropuesta
            Preg.ParAlternativa = par.ParAlternativa
            Preg.MetCodigo = par.MetCodigo
            Preg.ParNacional = par.ParNacional
            Preg.PregAbiertas = CType(UC_Producto2.FindControl("AbiertasReal"), TextBox).Text
            Preg.PregAbiertasMultiples = CType(UC_Producto2.FindControl("AbiertasMultReal"), TextBox).Text
            Preg.PregCerradas = CType(UC_Producto2.FindControl("CerradasReal"), TextBox).Text
            Preg.PregCerradasMultiples = CType(UC_Producto2.FindControl("CerradasMultReal"), TextBox).Text
            Preg.PregOtras = CType(UC_Producto2.FindControl("OtrosReal"), TextBox).Text
            Preg.PregDemograficos = CType(UC_Producto2.FindControl("DemoReal"), TextBox).Text
            par.ParTiempoEncuesta = If(CType(UC_Producto2.FindControl("txtDuracion"), TextBox).Text = "", 0, CInt(CType(UC_Producto2.FindControl("txtDuracion"), TextBox).Text))

            'Agregamos  los valores de la muestra teniendo en cuenta las ciudades
            Dim lstMuestra As New List(Of IQ_Muestra_1)
            Dim m As New IQ_Muestra_1
            If guardar = 0 Then

                If CCLstTipoMuestra.SelectedValue = "1" Then
                    m = New IQ_Muestra_1
                    m.IdPropuesta = par.IdPropuesta
                    m.ParAlternativa = par.ParAlternativa
                    m.ParNacional = par.ParNacional
                    m.MetCodigo = par.MetCodigo
                    m.CiuCodigo = CInt(CCLstCiudad.SelectedValue)
                    m.DeptCodigo = CInt(CCLstDepartamento.SelectedValue)
                    m.MuCantidad = CInt(CCTxtCantidad.Text)
                    m.MuIdentificador = CCLstTipoMuestra.SelectedValue

                Else
                    Dim mAlto As New IQ_Muestra_1
                    mAlto.IdPropuesta = par.IdPropuesta
                    mAlto.ParAlternativa = par.ParAlternativa
                    mAlto.ParNacional = par.ParNacional
                    mAlto.MetCodigo = par.MetCodigo
                    mAlto.CiuCodigo = CInt(CCLstCiudad.SelectedValue)
                    mAlto.DeptCodigo = CInt(CCLstDepartamento.SelectedValue)
                    mAlto.MuCantidad = CInt(CCTxtCantidad.Text)
                    mAlto.MuIdentificador = CCLstTipoMuestra.SelectedValue
                    lstMuestra.Add(mAlto)
                End If
            End If


            'If mNew IsNot Nothing Then
            '    lstMuestra.Add(mNew)
            'End If
            'se agregan las tarifas mistery


            If lstmNew IsNot Nothing Then
                _CaraCara.InsertarPresupuesto(par, lstmNew, Preg, lstProc)
            Else
                _CaraCara.InsertarPresupuesto(par, lstMuestra, Preg, lstProc)
            End If

            If guardar = 3 Then
                Dim v As New IQ_TarifasMistery()
                v.TS_Id = CInt(lstTipoServicio.SelectedValue)
                v.TE_Id = CInt(lstTipoEvidencia.SelectedValue)
                v.EV_Id = CInt(lstEvidencia.SelectedValue)
                v.CC_Id = CInt(lstNumContactos.SelectedValue)


                Dim t As New IQ_ValorVisitaMistery()
                t.TM_Valor = CDec(txtValor.Text)
                t.VM_Id = _CaraCara.ObtenerIdValorVisitaMistery(v).VM_Id
                t.IdPropuesta = par.IdPropuesta
                t.ParAlternativa = par.ParAlternativa
                t.ParNacional = par.ParNacional
                t.MetCodigo = par.MetCodigo
                t.TE_TiempoCritica = CInt(CCTxtTiempoCritica.Text)
                _CaraCara.InsertarTarifaMistery(t)
                _CaraCara.InsertarTiempoCritica(t)
            End If


            If CCLstTipoMuestra.SelectedValue = "1" And m IsNot Nothing Then
                _CaraCara.DistribuirMuestra(m)
            End If

            '3: se esta insertando desde los parametros de mistery
            If guardar <> 3 Then

                If guardar = 7 Then
                    EfectuarCaculosCaraCara(par, 1)
                Else
                    EfectuarCaculosCaraCara(par, 0)
                End If



                CCTxtProductividad.Text = par.ParProductividad.ToString()

                CargarCiudadesMuestra(par, CCGvMuestra)

                CCTxtCantidad.Text = ""
                lblTotalAlta.Text = _CaraCara.TotalizarMuestra(par).ToString()

                'diciembre 26 de 2013 - ajuste 
                Dim numEncuestadores As Decimal
                numEncuestadores = _CaraCara.TotalizarMuestra(par) / CInt(txtDiasCampo.Text) / par.ParProductividad
                CCLblNumEncuetadores.Text = numEncuestadores.ToString("N2")
            End If

            cargarTarifasMistery(par)
            CCLstCiudad.ClearSelection()
            CCLstDepartamento.ClearSelection()
            CCLstTipoMuestra.ClearSelection()

        Else
            ShowNotification(mensaje, WebMatrix.ShowNotifications.ErrorNotification)
        End If

    End Sub
    Private Sub CargarCaraCara()

        Try
            Dim par As IQ_Parametros
            par = CType(Session("PARAMETROS"), IQ_Parametros)
            'par.ParNacional = CInt(CCrbNacional.SelectedValue)
            par = _CaraCara.ObtenerCaraCara(par)
            CType(UC_Producto2.FindControl("LstOferta"), DropDownList).SelectedIndex = CType(UC_Producto2.FindControl("LstOferta"), DropDownList).Items.IndexOf(CType(UC_Producto2.FindControl("LstOferta"), DropDownList).Items.FindByValue(par.Pr_Offeringcode))
            UC_Producto2.cargarProductos()
            CType(UC_Producto2.FindControl("LstProducto"), DropDownList).SelectedIndex = CType(UC_Producto2.FindControl("LstProducto"), DropDownList).Items.IndexOf(CType(UC_Producto2.FindControl("LstProducto"), DropDownList).Items.FindByValue(par.Pr_ProductCode))
            UC_Producto2.CargarPreguntasPropuestas()
            CCLstMetodologia.SelectedIndex = CCLstMetodologia.Items.IndexOf(CCLstMetodologia.Items.FindByValue(par.MetCodigo))
            CargarTipoMuestra(CCLstTipoMuestra, CInt(CCLstMetodologia.SelectedValue))
            BloquearSegunMetodologia()
            CCTxtGrupoObj.Text = par.ParGrupoObjetivo

            If par.ParAñoSiguiente = True Then
                chkAñoActual.Checked = False
                chkAñoSiguiente.Checked = True
            Else
                chkAñoActual.Checked = True
                chkAñoSiguiente.Checked = False
            End If

            CCTxtNumProcDC.Text = par.ParNProcesosDC.ToString()
            CCTxtNumProcTL.Text = par.ParNProcesosTopLines.ToString()
            CCTxtNumProcTablas.Text = par.ParNProcesosTablas.ToString()
            CCTxtNumProcBases.Text = par.ParNProcesosBases.ToString()
            CCEncXPunto.Text = If(par.ParEncuestadoresPunto = 0, "", par.ParEncuestadoresPunto)

            'preguntas
            CType(UC_Producto2.FindControl("AbiertasReal"), TextBox).Text = par.IQ_Preguntas.PregAbiertas
            CType(UC_Producto2.FindControl("AbiertasMultReal"), TextBox).Text = par.IQ_Preguntas.PregAbiertasMultiples
            CType(UC_Producto2.FindControl("CerradasReal"), TextBox).Text = par.IQ_Preguntas.PregCerradas
            CType(UC_Producto2.FindControl("CerradasMultReal"), TextBox).Text = par.IQ_Preguntas.PregCerradasMultiples
            CType(UC_Producto2.FindControl("OtrosReal"), TextBox).Text = par.IQ_Preguntas.PregOtras
            CType(UC_Producto2.FindControl("DemoReal"), TextBox).Text = par.IQ_Preguntas.PregDemograficos
            CType(UC_Producto2.FindControl("TxtDuracion"), TextBox).Text = par.ParTiempoEncuesta.ToString()

            CatiCargarIncidencia(CCLstIncidencia)
            CCLstIncidencia.SelectedIndex = CCLstIncidencia.Items.IndexOf(CCLstIncidencia.Items.FindByValue(par.ParIncidencia))
            'OJO-Sobra CatiTxtNumProcesos.Text = par.ParNumeroProcesos.ToString()
            CCTxtProductividad.Text = par.ParProductividad.ToString()
            CCTxtObservaciones.Text = If(par.ParObservaciones Is Nothing, "", par.ParObservaciones)

            If PanelLocalizacion.Visible Then
                CCPorIntercep.Text = par.ParPorcentajeIntercep.ToString()
                CCPorRecluta.Text = par.ParPorcentajeRecluta.ToString()
                CCTxtUniProductos.Text = par.ParUnidadesProducto.ToString()
                CCTxtValorUniProd.Text = par.ParValorUnitarioProd.ToString()
                CCLstTipoCLT.SelectedIndex = If(par.ParTipoCLT Is Nothing, 0, CCLstTipoCLT.Items.IndexOf(CCLstTipoCLT.Items.FindByValue(par.ParTipoCLT)))
                CCtxtAlqEquip.Text = par.ParAlquilerEquipos.ToString()
                CCchkApoyoLogis.Checked = If(par.ParApoyoLogistico Is Nothing, False, par.ParApoyoLogistico)
                CCchkAccesoInter.Checked = If(par.ParAccesoInternet Is Nothing, False, par.ParAccesoInternet)
                If par.ParPorcentajeIntercep IsNot Nothing And par.ParPorcentajeRecluta IsNot Nothing Then
                    CCtxtTotalPorcentaje.Text = (par.ParPorcentajeIntercep + par.ParPorcentajeRecluta).ToString()
                End If

                CCtxtTotalProd.Text = (par.ParUnidadesProducto * par.ParValorUnitarioProd).ToString()

            End If
            chkTablet.Checked = If(par.ParUsaTablet Is Nothing, False, par.ParUsaTablet)
            ChkPapel.Checked = If(par.ParUsaPapel Is Nothing, False, par.ParUsaPapel)

            If par.ParUsaTablet = True Then
                'CCChkDispPropio.Visible = True
                'CCChkDispPropio.Checked = If(par.ParDispPropio Is Nothing, False, par.ParDispPropio)
            End If

            'Procesos
            For i = 0 To CCchkProcesos.Items.Count - 1
                If _Cati.ExisteProceso(par.IQ_ProcesosPresupuesto.ToList(), CInt(CCchkProcesos.Items(i).Value)) Then
                    CCchkProcesos.Items(i).Selected = True
                Else
                    CCchkProcesos.Items(i).Selected = False
                End If
            Next

            Session("PARAMETROS") = par
            lblTotalAlta.Text = _CaraCara.TotalizarMuestra(par).ToString()
            CargarCiudadesMuestra(par, CCGvMuestra)

            'diciembre 26 de 2013 - ajuste 
            Dim numEncuestadores As Decimal
            numEncuestadores = _CaraCara.TotalizarMuestra(par) / CInt(txtDiasCampo.Text) / If(par.ParProductividad = 0, 1, par.ParProductividad)
            CCLblNumEncuetadores.Text = numEncuestadores.ToString("N2")
            cargarTarifasMistery(par)
            upCaraCara.Update()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    Protected Sub CCGvMuestra_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles CCGvMuestra.RowCommand
        Try
            Select Case e.CommandName

                Case "DEL"
                    Dim m As New IQ_Muestra_1
                    Dim par As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)

                    m.IdPropuesta = par.IdPropuesta
                    m.ParAlternativa = par.ParAlternativa
                    m.MetCodigo = par.MetCodigo
                    m.ParNacional = par.ParNacional
                    m.CiuCodigo = CInt(CCGvMuestra.Rows(e.CommandArgument).Cells(1).Text)
                    m.MuIdentificador = 1
                    _CaraCara.BorrarCiudadMuestra(m)
                    CargarCiudadesMuestra(par, CCGvMuestra)
                    lblTotalAlta.Text = _CaraCara.TotalizarMuestra(par)


            End Select

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try

    End Sub



    Protected Sub CCLstFases_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CCLstFases.SelectedIndexChanged
        Try
            'Si es nacional se debe cargar  solo los departamentos del pais, si es internacional los paises

            If CCLstFases.SelectedIndex > 0 Then
                CCCargarDepartamentos()
            End If


        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Private Sub CCCargarDepartamentos()
        Try

            If CCLstFases.SelectedValue = "2" Then
                CargarDepartamentos(CCLstDepartamento, False, False)
            Else

                CargarDepartamentos(CCLstDepartamento, False, True)

            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub CatiLstMetodologia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CatiLstMetodologia.SelectedIndexChanged
        Try
            'DEBEMOS VALIDAR  SI  ESTA SELECCIONADA LA OPCION NACIONAL Y VALIDAR SI EXISTE EL PRESUPUESTO PARA CARGARLO
            If CatiLstMetodologia.SelectedIndex > 0 Then
                'CARGAMOS LAS OPCIONES DE LA MUESTRA SIEMPRE 
                CargarTipoMuestra(CatiLstTipoMuestra, CInt(CatiLstMetodologia.SelectedValue))

                If CatiLstFase.SelectedIndex > 0 Then
                    Dim par As New IQ_Parametros

                    par.IdPropuesta = CType(Session("PARAMETROS"), IQ_Parametros).IdPropuesta
                    par.ParAlternativa = CType(Session("PARAMETROS"), IQ_Parametros).ParAlternativa
                    par.ParNacional = CInt(CatiLstFase.SelectedValue)
                    par.MetCodigo = CInt(CatiLstMetodologia.SelectedValue)
                    par.ParUnidad = CType(Session("PARAMETROS"), IQ_Parametros).ParUnidad
                    Session("PARAMETROS") = par
                    If (_Cati.ExisteCati(CType(Session("PARAMETROS"), IQ_Parametros))) Then
                        CatiLstMetodologia.Enabled = False
                        CatiLstFase.Enabled = False
                        CargarDatosCati()
                    Else
                        'SE DEBEN LIMPIAR LOS CONTROLES EXCEPTO METODOLOGIA Y NACIONALES
                    End If

                Else
                    ShowNotification("Seleccione la opcion Nacional/Internacional", WebMatrix.ShowNotifications.ErrorNotification)
                    CatiLstMetodologia.SelectedIndex = 0
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try


    End Sub

#End Region

#Region "SESIONES"

    Private Function ValidarSesiones(ByVal todo As Integer) As String


        Dim Mensaje As String = ""

        If SesLstMetodologia.SelectedIndex = 0 Then

            Mensaje = "Seleccione la metodologia!"
            Return Mensaje
        End If


        If SesGrupoObjetivo.Text = "" Then

            Mensaje = "Mencione el grupo objetivo!"
            Return Mensaje
        End If

        If sesLstOferta.SelectedIndex = 0 Then

            Mensaje = "Seleccione la oferta !"
            Return Mensaje
        End If

        If sesLstProducto.SelectedIndex = 0 Then

            Mensaje = "Seleccione el producto !"
            Return Mensaje
        End If


        If ContarProcesos(SesChkProcesos) = 0 Then

            Mensaje = "Seleccione al menos un proceso!"
            Return Mensaje
        End If


        If todo = 1 Then

            If SesTxtCantidad.Text = "" Then
                Mensaje = "Digite la muestra (media). !"
                Return Mensaje
            End If


            If sesLstTipoMuestra.SelectedIndex = 0 Then
                Mensaje = "Seleccione la dificultad de la muestra   !"
                Return Mensaje
            End If

            If SesLstDepto.SelectedIndex = 0 Then
                Mensaje = "Seleccione el departamento !"
                Return Mensaje
            End If

            If SesLstCiudad.SelectedIndex = 0 Then

                Mensaje = "Seleccione la ciudad !"
                Return Mensaje
            End If

        End If

        If SesTxtCantPart.Text = "" Then

            Mensaje = "Digite la cantidad de participantes !"
            Return Mensaje
        End If

        If sesLstFase.SelectedIndex = 0 Then
            Mensaje = "Seleccione una fase !"
            Return Mensaje
        End If

        If sestxtSubcontratar.Visible = True Then
            If sestxtSubcontratar.Text.Trim = "" Then
                Mensaje = "Digite el porcentaje a subcontratar !"
                Return Mensaje
            End If

            If CInt(sestxtSubcontratar.Text) < 1 Or CInt(sestxtSubcontratar.Text) > 100 Then
                Mensaje = "El porcentaje a subcontratar debe estar entre 1 y 100 !"
                Return Mensaje
            End If

        End If

        Return Mensaje
    End Function

    Protected Sub SesLstDepto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles SesLstDepto.SelectedIndexChanged
        Try

            If SesLstDepto.SelectedIndex > 0 Then

                If sesLstFase.SelectedValue = "2" Then
                    CargarCiudades(CInt(SesLstDepto.SelectedValue), SesLstCiudad, False)
                Else
                    CargarCiudades(CInt(SesLstDepto.SelectedValue), SesLstCiudad, False)
                End If

            End If
        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try

    End Sub

    Protected Sub sesLstOferta_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles sesLstOferta.SelectedIndexChanged
        Try

            If sesLstOferta.SelectedIndex > 0 Then
                cargarProductos(sesLstProducto, sesLstOferta.SelectedValue)
            End If
        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try


    End Sub

    Protected Sub sesBetnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles sesBetnAgregar.Click
        Try
            'INGRESAMOS LOS DATOS DE LA TECNICA SESIONES DE GRUPO
            Dim mensaje As String = ValidarSesiones(1)

            If mensaje = "" Then
                GuardarSesionesGrupo(CType(Session("PARAMETROS"), IQ_Parametros), 1)

            Else
                ShowNotification(mensaje, WebMatrix.ShowNotifications.ErrorNotification)
            End If

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Private Sub GuardarSesionesGrupo(ByVal p As IQ_Parametros, ByVal Guardar As Integer)
        Try
            Dim par As New IQ_Parametros
            par.IdPropuesta = p.IdPropuesta
            par.ParAlternativa = p.ParAlternativa
            par.ParUnidad = p.ParUnidad
            par.Usuario = CDec(Session("IDUsuario"))
            par.ParFechaCreacion = DateTime.Now

            par.ParNacional = CInt(sesLstFase.SelectedValue)

            If seschkAñoActual.Checked = True And seschkAñoSiguiente.Checked = False Then
                par.ParAñoSiguiente = False
            Else
                If seschkAñoActual.Checked = False And seschkAñoSiguiente.Checked = True Then
                    par.ParAñoSiguiente = True
                Else
                    If seschkAñoActual.Checked = False And seschkAñoSiguiente.Checked = False Then
                        par.ParAñoSiguiente = True
                    Else
                        If seschkAñoActual.Checked = True And seschkAñoSiguiente.Checked = True Then
                            par.ParAñoSiguiente = True
                        End If
                    End If
                End If
            End If


            par.Pr_ProductCode = sesLstProducto.SelectedValue
            par.Pr_Offeringcode = sesLstOferta.SelectedValue
            par.ParGrupoObjetivo = SesGrupoObjetivo.Text
            par.TecCodigo = CInt(hfTecnica.Value)
            par.MetCodigo = CInt(SesLstMetodologia.SelectedValue)
            par.ParNumAsistentesSesion = CInt(SesTxtCantPart.Text)
            par.ParObservaciones = SesTxtObservaciones.Text
            par.ParHorasEntrevista = 2
            par.TipoProyecto = 2
            par.ParNomPresupuesto = lblNomPropuesta.Text

            If SesChkSubcontratar.Checked Then
                par.ParSubcontratar = True
                par.ParPorcentajeSub = CInt(sestxtSubcontratar.Text)
            Else
                par.ParSubcontratar = False
                par.ParPorcentajeSub = Nothing

            End If

            'DEBEMOS RECORRER TODAS LAS OPCIONES YGUARDAR EN LA VARIABLE ASIGANADA DENTRO DE LA TABLA DE PARAMETROS
            Dim lstOp As New List(Of IQ_OpcionesAplicadas)
            For O = 0 To sesChkOpciones.Items.Count - 1
                If sesChkOpciones.Items(O).Selected Then
                    Dim Op As New IQ_OpcionesAplicadas
                    Op.IdPropuesta = par.IdPropuesta
                    Op.ParAlternativa = par.ParAlternativa
                    Op.ParNacional = par.ParNacional
                    Op.MetCodigo = par.MetCodigo
                    Op.TecCodigo = par.TecCodigo
                    Op.IdOpcion = CInt(sesChkOpciones.Items(O).Value)
                    Op.Aplica = True
                    lstOp.Add(Op)
                Else
                    Dim Op As New IQ_OpcionesAplicadas
                    Op.IdPropuesta = par.IdPropuesta
                    Op.ParAlternativa = par.ParAlternativa
                    Op.ParNacional = par.ParNacional
                    Op.MetCodigo = par.MetCodigo
                    Op.TecCodigo = par.TecCodigo
                    Op.IdOpcion = CInt(sesChkOpciones.Items(O).Value)
                    Op.Aplica = False
                    lstOp.Add(Op)

                End If

            Next

            'Agregamos los procesos 
            Dim lstProc As New List(Of IQ_ProcesosPresupuesto)
            For i = 0 To SesChkProcesos.Items.Count - 1
                If SesChkProcesos.Items(i).Selected Then
                    Dim P1 = New IQ_ProcesosPresupuesto()
                    P1.IdPropuesta = par.IdPropuesta
                    P1.ParAlternativa = par.ParAlternativa
                    P1.MetCodigo = par.MetCodigo
                    P1.ParNacional = par.ParNacional
                    P1.Porcentaje = 0
                    P1.ProcCodigo = CInt(SesChkProcesos.Items(i).Value)
                    lstProc.Add(P1)
                End If

            Next

            'Agregamos  los valores de la muestra teniendo en cuenta las ciudades
            Dim LstMuestra As New List(Of IQ_Muestra_1)
            If Guardar = 1 Then

                Dim mMedia As New IQ_Muestra_1
                mMedia.IdPropuesta = par.IdPropuesta
                mMedia.ParAlternativa = par.ParAlternativa
                mMedia.ParNacional = par.ParNacional
                mMedia.MetCodigo = par.MetCodigo
                mMedia.CiuCodigo = CInt(SesLstCiudad.SelectedValue)
                mMedia.DeptCodigo = CInt(SesLstDepto.SelectedValue)
                mMedia.MuCantidad = CInt(SesTxtCantidad.Text)
                mMedia.MuIdentificador = CInt(sesLstTipoMuestra.SelectedValue)
                LstMuestra.Add(mMedia)
            End If
            Dim lsth As List(Of IQ_HorasProfesionales)
            lsth = CargarListaHoras(gvCargosSesiones, par)

            _sesiones.InsertarPresupuesto(par, LstMuestra, lstProc, lstOp, lsth)

            EfectuarCalculoSesiones(par)
            'una vez insertados los paremtros se hace el calculo de  valores 
            CargarMuestraSesionesEntrevistas(gvSesionesMuestra, par)
            sesLblTotalMuestra.Text = _entrevistas.TotalizarMuestra(par)
            upotr1.Update()
            upGVSesMuestra.Update()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub CargarSesiones(ByVal p As IQ_Parametros)
        Try
            Dim par As IQ_Parametros
            par = _sesiones.ObtenerSesiones(p)
            Session("PARAMETROS") = par

            If par.ParAñoSiguiente = True Then
                seschkAñoActual.Checked = False
                seschkAñoSiguiente.Checked = True
            Else
                seschkAñoActual.Checked = True
                seschkAñoSiguiente.Checked = False
            End If


            SesLstMetodologia.SelectedIndex = SesLstMetodologia.Items.IndexOf(SesLstMetodologia.Items.FindByValue(par.MetCodigo))
            CargarTipoMuestra(sesLstTipoMuestra, CInt(SesLstMetodologia.SelectedValue))
            SesGrupoObjetivo.Text = par.ParGrupoObjetivo.ToString()
            sesLstOferta.SelectedIndex = sesLstOferta.Items.IndexOf(sesLstOferta.Items.FindByValue(par.Pr_Offeringcode))
            cargarProductos(sesLstProducto, par.Pr_Offeringcode)
            sesLstProducto.SelectedIndex = sesLstProducto.Items.IndexOf(sesLstProducto.Items.FindByValue(par.Pr_ProductCode))
            SesTxtCantPart.Text = par.ParNumAsistentesSesion.ToString()
            SesTxtObservaciones.Text = If(par.ParObservaciones Is Nothing, "", par.ParObservaciones)

            'CARGAMOS LA OPCIONES PARA LUEGO SELECCIONAR LAS QUE EXISTAN
            CargarOpciones(sesChkOpciones)


            For o = 0 To sesChkOpciones.Items.Count - 1
                If _sesiones.ExisteOpcion(par.IQ_OpcionesAplicadas.ToList(), CInt(sesChkOpciones.Items(o).Value)) Then
                    sesChkOpciones.Items(o).Selected = True
                End If
            Next

            For i = 0 To SesChkProcesos.Items.Count - 1
                If _Cati.ExisteProceso(par.IQ_ProcesosPresupuesto.ToList(), CInt(SesChkProcesos.Items(i).Value)) Then
                    SesChkProcesos.Items(i).Selected = True
                End If
            Next

            If par.ParSubcontratar = True Then
                SesChkSubcontratar.Checked = True
                sestxtSubcontratar.Visible = True
                sestxtSubcontratar.Text = par.ParPorcentajeSub.ToString()
            Else
                SesChkSubcontratar.Checked = False
                sestxtSubcontratar.Visible = False
            End If


            CargarMuestraSesionesEntrevistas(gvSesionesMuestra, par)
            sesLblTotalMuestra.Text = _sesiones.TotalizarMuestra(par).ToString()

            gvCargosSesiones.DataSource = _sesiones.ObtenerCargos(par, 0)
            gvCargosSesiones.DataBind()
            upSesiones.Update()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub CargarOpciones(ByVal chk As CheckBoxList)
        Try
            chk.DataSource = _sesiones.ObtenerOpciones(CType(Session("PARAMETROS"), IQ_Parametros).TecCodigo)
            chk.DataTextField = "DescOpcion"
            chk.DataValueField = "IdOpcion"
            chk.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub sesLstFase_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles sesLstFase.SelectedIndexChanged
        Try
            If sesLstFase.SelectedIndex > 0 Then

                sesCargarDepartamento()

            End If

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Private Sub sesCargarDepartamento()
        Try

            If sesLstFase.SelectedValue = 1 Then
                CargarDepartamentos(SesLstDepto, True, True)
            Else
                CargarDepartamentos(SesLstDepto, True, False)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub EfectuarCalculoSesiones(ByVal p As IQ_Parametros)
        Try
            'insertamos las horas profesionales antes de hacer un calculo 
            _sesiones.InsertarCargos(CargarListaHoras(gvCargosSesiones, p))
            _sesiones.calcularActSubcontratadas(p)

            p.ParCostoDirecto = _sesiones.ObtenerCostoDirecto(p)

            p.ParValorVenta = _sesiones.ValorVentaCualitativo(p)
            'Obtenemos los valores que no aplican para el gross margin pues estos tiene su propio porcentaje y los  restamos las actividades  subcontratadas
            Dim ValorActividadesNoGM As Decimal
            ValorActividadesNoGM = _Cati.ActividadesNoAplicanGM(p)
            Dim ValorActividadesNoGMConAdmon As Decimal

            ValorActividadesNoGMConAdmon = (ValorActividadesNoGM * 1.1)

            'Rutina nuevo claculo de GM
            Dim WGMOps As New Double


            p.ParGrossMargin = ((p.ParValorVenta) - (p.ParCostoDirecto + ((p.ParActSubGasto)))) / (p.ParValorVenta)
            Session("PARAMETROS") = p

            'actualizams unicamente el gross amrgin y el valor de venta  pues los demas ya han sido actuaizados desde el procedimiento
            _sesiones.InsertarGrossMargin(p)


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CargarMuestraSesionesEntrevistas(ByVal gv As GridView, ByVal par As IQ_Parametros)
        Try

            gv.DataSource = _CaraCara.obtenerCiudadesMuestra(par)
            gv.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "ENTREVISTAS"

    Private Sub EfectuarCalculosEntrevistas(ByVal p As IQ_Parametros)
        Try
            'insertamos las horas profesionales antes de hacer un calculo 
            _entrevistas.InsertarCargos(CargarListaHoras(gvCargosEntrevistas, p))

            p.ParCostoDirecto = _entrevistas.ObtenerCostoDirecto(p)
            _sesiones.calcularActSubcontratadas(p)
            p.ParValorVenta = _sesiones.ValorVentaCualitativo(p)
            'Obtenemos los valores que no aplican para el gross margin pues estos tiene su propio porcentaje y los  restamos las actividades  subcontratadas
            Dim ValorActividadesNoGM As Decimal
            ValorActividadesNoGM = _Cati.ActividadesNoAplicanGM(p)
            Dim ValorActividadesNoGMConAdmon As Decimal

            ValorActividadesNoGMConAdmon = (ValorActividadesNoGM * 1.1)
            p.ParGrossMargin = ((p.ParValorVenta) - (p.ParCostoDirecto + ((p.ParActSubGasto)))) / (p.ParValorVenta)
            Session("PARAMETROS") = p

            _sesiones.InsertarGrossMargin(p)


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub EntLstDepto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles EntLstDepto.SelectedIndexChanged
        Try
            If EntLstDepto.SelectedIndex > 0 Then
                If EntLstFase.SelectedValue = "2" Then
                    CargarCiudades(CInt(EntLstDepto.SelectedValue), EntLstCiudad, False)
                Else
                    CargarCiudades(CInt(EntLstDepto.SelectedValue), EntLstCiudad, True)
                End If

            End If

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try


    End Sub

    Private Sub CargarOferta(ByVal lst As DropDownList)
        Try

            lst.DataSource = _prod.ObtenerOferta(CType(Session("PARAMETROS"), IQ_Parametros).ParUnidad)
            lst.DataTextField = "Pr_OfferingDescription"
            lst.DataValueField = "Pr_Offeringcode"
            lst.DataBind()


        Catch ex As Exception
            Throw ex
        End Try
    End Sub



    Public Sub cargarProductos(ByVal lst As DropDownList, ByVal oferta As String)

        Try
            Dim lstProds = _prod.ObtenerProductosPorUnidad(oferta)
            lst.DataSource = lstProds
            lst.DataTextField = "Pr_ProductDescription"
            lst.DataValueField = "Pr_ProductCode"
            lst.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Protected Sub EntLstOferta_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles EntLstOferta.SelectedIndexChanged

        Try

            If EntLstOferta.SelectedIndex > 0 Then

                cargarProductos(EntLstProducto, EntLstOferta.SelectedValue)
            End If

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try


    End Sub

    Private Function ValidarEntrevistas(ByVal todo As Integer) As String


        Dim Mensaje As String = ""

        If EntLstMetodologia.SelectedIndex = 0 Then

            Mensaje = "Seleccione la metodologia!"
            Return Mensaje
        End If


        If EntGrupoObjetivo.Text = "" Then

            Mensaje = "Mencione el grupo objetivo!"
            Return Mensaje
        End If

        If EntLstOferta.SelectedIndex = 0 Then

            Mensaje = "Seleccione la oferta !"
            Return Mensaje
        End If

        If EntLstProducto.SelectedIndex = 0 Then

            Mensaje = "Seleccione el producto !"
            Return Mensaje
        End If


        If ContarProcesos(EntChkProcesos) = 0 Then

            Mensaje = "Seleccione al menos un proceso!"
            Return Mensaje
        End If



        If todo = 1 Then

            If EntTxtCantidad.Text = "" Then
                Mensaje = "Digite la muestra  !"
                Return Mensaje
            End If



            If EntLstDepto.SelectedIndex = 0 Then

                Mensaje = "Seleccione el departamento !"
                Return Mensaje
            End If

            If EntLstCiudad.SelectedIndex = 0 Then

                Mensaje = "Seleccione la ciudad !"
                Return Mensaje
            End If

            If EntLstTipoMuestra.SelectedIndex = 0 Then

                Mensaje = "Seleccione la dificultad de la muestra !"
                Return Mensaje
            End If

            If EntLstTipoMuestra.SelectedIndex = 0 Then

                Mensaje = "Seleccione la dificultad de la muestra !"
                Return Mensaje
            End If

        End If


        If EntLstFase.SelectedIndex = 0 Then
            Mensaje = "Seleccione una opcion  !"
            Return Mensaje
        End If

        If EntRbHoras.SelectedIndex < 0 Then
            Mensaje = "Seleccione las horas !"
            Return Mensaje
        End If

        If EnttxtSubcontratar.Visible = True Then
            If EnttxtSubcontratar.Text.Trim = "" Then
                Mensaje = "Digite el porcentaje a subcontratar !"
                Return Mensaje
            End If

            If CInt(EnttxtSubcontratar.Text) < 1 Or CInt(EnttxtSubcontratar.Text) > 100 Then
                Mensaje = "El porcentaje a subcontratar debe estar entre 1 y 100 !"
                Return Mensaje
            End If

        End If

        Return Mensaje
    End Function

    Private Sub GUardarEntrevistas(ByVal p As IQ_Parametros, ByVal guardar As Integer)
        Try
            Dim par As New IQ_Parametros
            par.IdPropuesta = p.IdPropuesta
            par.ParAlternativa = p.ParAlternativa
            par.ParUnidad = p.ParUnidad
            par.Usuario = CDec(Session("IDUsuario"))
            par.ParFechaCreacion = DateTime.Now

            par.ParNacional = CInt(EntLstFase.SelectedValue)

            If EntchkAñoActual.Checked = True And EntchkAñoSiguiente.Checked = False Then
                par.ParAñoSiguiente = False
            Else
                If EntchkAñoActual.Checked = False And EntchkAñoSiguiente.Checked = True Then
                    par.ParAñoSiguiente = True
                Else
                    If EntchkAñoActual.Checked = False And EntchkAñoSiguiente.Checked = False Then
                        par.ParAñoSiguiente = True
                    Else
                        If EntchkAñoActual.Checked = True And EntchkAñoSiguiente.Checked = True Then
                            par.ParAñoSiguiente = True
                        End If
                    End If
                End If
            End If

            par.Pr_ProductCode = EntLstProducto.SelectedValue
            par.Pr_Offeringcode = EntLstOferta.SelectedValue
            par.ParGrupoObjetivo = EntGrupoObjetivo.Text
            par.TecCodigo = CInt(hfTecnica.Value)
            par.MetCodigo = CInt(EntLstMetodologia.SelectedValue)

            par.ParHorasEntrevista = CInt(EntRbHoras.SelectedValue)
            par.ParObservaciones = EntTxtObservaciones.Text
            par.TipoProyecto = 2
            par.ParNomPresupuesto = lblNomPropuesta.Text


            If EntChkSubcontratar.Checked Then
                par.ParSubcontratar = True
                par.ParPorcentajeSub = CInt(EnttxtSubcontratar.Text)
            Else
                par.ParSubcontratar = False
                par.ParPorcentajeSub = Nothing

            End If

            'DEBEMOS RECORRER TODAS LAS OPCIONES YGUARDAR EN LA VARIABLE ASIGANADA DENTRO DE LA TABLA DE PARAMETROS
            Dim lstOp As New List(Of IQ_OpcionesAplicadas)
            For O = 0 To EntChkOpciones.Items.Count - 1
                If EntChkOpciones.Items(O).Selected Then
                    Dim Op As New IQ_OpcionesAplicadas
                    Op.IdPropuesta = par.IdPropuesta
                    Op.ParAlternativa = par.ParAlternativa
                    Op.ParNacional = par.ParNacional
                    Op.MetCodigo = par.MetCodigo
                    Op.TecCodigo = par.TecCodigo
                    Op.IdOpcion = CInt(EntChkOpciones.Items(O).Value)
                    Op.Aplica = True
                    lstOp.Add(Op)
                Else

                    Dim Op As New IQ_OpcionesAplicadas
                    Op.IdPropuesta = par.IdPropuesta
                    Op.ParAlternativa = par.ParAlternativa
                    Op.ParNacional = par.ParNacional
                    Op.MetCodigo = par.MetCodigo
                    Op.TecCodigo = par.TecCodigo
                    Op.IdOpcion = CInt(EntChkOpciones.Items(O).Value)
                    Op.Aplica = False
                    lstOp.Add(Op)
                End If

            Next

            'Agregamos los procesos 
            Dim lstProc As New List(Of IQ_ProcesosPresupuesto)
            For i = 0 To EntChkProcesos.Items.Count - 1
                If EntChkProcesos.Items(i).Selected Then
                    Dim P1 = New IQ_ProcesosPresupuesto()
                    P1.IdPropuesta = par.IdPropuesta
                    P1.ParAlternativa = par.ParAlternativa
                    P1.MetCodigo = par.MetCodigo
                    P1.ParNacional = par.ParNacional
                    P1.Porcentaje = 0
                    P1.ProcCodigo = CInt(EntChkProcesos.Items(i).Value)
                    lstProc.Add(P1)
                End If

            Next

            'Agregamos  los valores de la muestra teniendo en cuenta las ciudades
            Dim lstMuestra As New List(Of IQ_Muestra_1)

            If guardar = 1 Then
                Dim mMedia As New IQ_Muestra_1
                mMedia.IdPropuesta = par.IdPropuesta
                mMedia.ParAlternativa = par.ParAlternativa
                mMedia.ParNacional = par.ParNacional
                mMedia.MetCodigo = par.MetCodigo
                mMedia.CiuCodigo = CInt(EntLstCiudad.SelectedValue)
                mMedia.DeptCodigo = CInt(EntLstDepto.SelectedValue)
                mMedia.MuCantidad = CInt(EntTxtCantidad.Text)
                mMedia.MuIdentificador = EntLstTipoMuestra.SelectedValue
                lstMuestra.Add(mMedia)

            End If


            _entrevistas.InsertarPresupuesto(par, lstMuestra, lstProc, lstOp)
            EfectuarCalculosEntrevistas(par)
            'una vez insertados los paremtros se hace el calculo de  valores 
            CargarMuestraSesionesEntrevistas(gvEntrevistasMuestra, par)
            entLblTotalMuestra.Text = _entrevistas.TotalizarMuestra(par).ToString()
            upEntDeptos.Update()
            upGvEntrevistasMuestra.Update()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CargarEntrevistas(ByVal p As IQ_Parametros)
        Try
            Dim par As IQ_Parametros
            par = _sesiones.ObtenerSesiones(p)
            Session("PARAMETROS") = par
            EntLstFase.Enabled = False

            If par.ParAñoSiguiente = True Then
                EntchkAñoActual.Checked = False
                EntchkAñoSiguiente.Checked = True
            Else
                EntchkAñoActual.Checked = True
                EntchkAñoSiguiente.Checked = False
            End If

            EntLstMetodologia.SelectedIndex = EntLstMetodologia.Items.IndexOf(EntLstMetodologia.Items.FindByValue(par.MetCodigo))
            CargarTipoMuestra(EntLstTipoMuestra, CInt(EntLstMetodologia.SelectedValue))
            EntGrupoObjetivo.Text = par.ParGrupoObjetivo.ToString()
            EntLstOferta.SelectedIndex = EntLstOferta.Items.IndexOf(EntLstOferta.Items.FindByValue(par.Pr_Offeringcode))
            cargarProductos(EntLstProducto, par.Pr_Offeringcode)
            EntLstProducto.SelectedIndex = EntLstProducto.Items.IndexOf(EntLstProducto.Items.FindByValue(par.Pr_ProductCode))
            EntRbHoras.SelectedIndex = EntRbHoras.Items.IndexOf(EntRbHoras.Items.FindByValue(par.ParHorasEntrevista))
            EntTxtObservaciones.Text = If(par.ParObservaciones Is Nothing, "", par.ParObservaciones)
            'CARGAMOS LA OPCIONES PARA LUEGO SELECCIONAR LAS QUE EXISTAN
            CargarOpciones(EntChkOpciones)


            For o = 0 To EntChkOpciones.Items.Count - 1
                If _sesiones.ExisteOpcion(par.IQ_OpcionesAplicadas.ToList(), CInt(EntChkOpciones.Items(o).Value)) Then
                    EntChkOpciones.Items(0).Selected = True
                End If
            Next

            For i = 0 To EntChkProcesos.Items.Count - 1
                If _Cati.ExisteProceso(par.IQ_ProcesosPresupuesto.ToList(), CInt(EntChkProcesos.Items(i).Value)) Then
                    EntChkProcesos.Items(i).Selected = True
                End If
            Next

            If par.ParSubcontratar = True Then
                EntChkSubcontratar.Checked = True
                EnttxtSubcontratar.Visible = True
                EnttxtSubcontratar.Text = par.ParPorcentajeSub.ToString()
            Else
                EntChkSubcontratar.Checked = False
                EnttxtSubcontratar.Visible = False
            End If

            CargarMuestraSesionesEntrevistas(gvEntrevistasMuestra, par)
            entLblTotalMuestra.Text = _entrevistas.TotalizarMuestra(par)
            upGvEntrevistasMuestra.Update()
            gvCargosEntrevistas.DataSource = _entrevistas.ObtenerCargos(par, 0)
            gvCargosEntrevistas.DataBind()
            UPeNTREVISTAS.Update()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub EntBtnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles EntBtnAgregar.Click
        Try

            Dim mensaje As String = ValidarEntrevistas(1)
            If mensaje = "" Then
                GUardarEntrevistas(CType(Session("PARAMETROS"), IQ_Parametros), 1)
            Else
                ShowNotification(mensaje, WebMatrix.ShowNotifications.ErrorNotification)
            End If

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub



    Protected Sub EntLstFase_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles EntLstFase.SelectedIndexChanged

        Try

            If EntLstFase.SelectedIndex > 0 Then
                entCargarDepartametos()
            End If


        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try

    End Sub


    Private Sub entCargarDepartametos()
        Try

            If EntLstFase.SelectedValue = 2 Then
                CargarDepartamentos(EntLstDepto, True, False)

            Else
                CargarDepartamentos(EntLstDepto, True, True)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region



#Region "REVISON Y APROBACION"

    Protected Sub chkRevisado_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub


    Protected Sub btnAlternativas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAlternativas.Click
        Try
            Dim par As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)
            Dim ParNuevo As IQ_Parametros = par
            Dim Alternativa As Integer
            Alternativa = par.ParAlternativa + 1
            par.ParAlternativa = Alternativa
            'SI LA ALTERNATIVA NO EXISTE DEBE CREARSE, SI NO EXISTE  EL CONSECUTIVO ANTERIOR TAMPOCO DEBERIA CREARSE Y MANTENERSE EN LA ALTERNATIVA ACTUAL
            If (Not _Cati.ExisteAlternativa(par)) Then

                If Request.QueryString("ACCION") IsNot Nothing Then
                    Response.Redirect("Cap_Principal.aspx?IdPropuesta=" & par.IdPropuesta & "&Alternativa=" & Alternativa & "&ACCION=" & Request.QueryString("ACCION"))
                    Session("PARAMETROS") = par
                Else
                    Response.Redirect("Cap_Principal.aspx?IdPropuesta=" & par.IdPropuesta & "&Alternativa=" & Alternativa)
                    Session("PARAMETROS") = par
                End If

            End If

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try


    End Sub



    Protected Sub btnAnterior_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAnterior.Click
        Try

            Dim par As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)
            Dim Alternativa As Integer

            If par.ParAlternativa > 1 Then
                Alternativa = par.ParAlternativa - 1

                If Request.QueryString("ACCION") IsNot Nothing Then
                    Response.Redirect("Cap_Principal.aspx?IdPropuesta=" & par.IdPropuesta & "&Alternativa=" & Alternativa & "&ACCION=" & Request.QueryString("ACCION"))
                Else
                    Response.Redirect("Cap_Principal.aspx?IdPropuesta=" & par.IdPropuesta & "&Alternativa=" & Alternativa)
                End If


            End If

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Protected Sub btnSiguiente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSiguiente.Click
        Try

            Dim par As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)
            Dim Alternativa As Integer

            If par.ParAlternativa < _Cati.UltimaAlternativa(par) Then
                Alternativa = par.ParAlternativa + 1

                If Request.QueryString("ACCION") IsNot Nothing Then
                    Response.Redirect("Cap_Principal.aspx?IdPropuesta=" & par.IdPropuesta & "&Alternativa=" & Alternativa & "&ACCION=" & Request.QueryString("ACCION"))
                Else
                    Response.Redirect("Cap_Principal.aspx?IdPropuesta=" & par.IdPropuesta & "&Alternativa=" & Alternativa)
                End If


            End If

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Sub EnviarEmail(ByVal par As IQ_Parametros)
        Try
            'Dim script As String = "window.open('../Emails/PresupuestosRevisados.aspx?PropuestaId=" & par.IdPropuesta & "','cal','width=400,height=250,left=270,top=180')"
            'Dim page As Page = DirectCast(Context.Handler, Page)
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
            Dim oEnviarCorreo As New EnviarCorreo
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/PresupuestosRevisados.aspx?PropuestaId=" & par.IdPropuesta & "&Alternativa=" & par.ParAlternativa & "&MetCodigo=" & par.MetCodigo & "&ParNacional=" & par.ParNacional)
        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try

    End Sub

#End Region


    Protected Sub lkSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lkSalir.Click
        hfOperaciones.Value = "1"
        If Request.QueryString("ACCION") IsNot Nothing Then
            Select Case Request.QueryString("ACCION")

                Case "1"
                    Response.Redirect("~/CU_cuentas/Propuestas.aspx")
                Case "2"
                    Response.Redirect("~/CU_cuentas/RevisionPresupuestos.aspx")
                    hfOperaciones.Value = "0"
                Case "3"
                    Response.Redirect("~/CU_cuentas/Estudios.aspx")
                Case "4"
                    Response.Redirect("~/CU_cuentas/AutorizacionPresupuestosDirectores.aspx")
                Case "5"
                    Response.Redirect("~/CU_cuentas/Proyectos.aspx")
                Case "6"
                    Response.Redirect("~/RE_GT/AsignacionCOE.aspx")
                    hfOperaciones.Value = "0"
                Case "7"
                    Response.Redirect("~/RP_Reportes/TrabajosPorGerencia.aspx")
                    hfOperaciones.Value = "0"
                Case "8"
                    Response.Redirect("~/RE_GT/AsignacionJBI.aspx ")
                    hfOperaciones.Value = "0"
                Case "9"
                    Response.Redirect("~/FI_AdministrativoFinanciero/ListadoEstudios.aspx")
                    hfOperaciones.Value = "0"
                Case "10"
                    Response.Redirect("~/FI_AdministrativoFinanciero/ListadoPropuestas.aspx")
                    hfOperaciones.Value = "0"
                Case "11"
                    Response.Redirect("PresupuestosAprobados.aspx")
                Case Else
                    Response.Redirect("~/CU_cuentas/Propuestas.aspx")
            End Select

        Else
            Response.Redirect("~/CU_cuentas/Propuestas.aspx")
        End If

    End Sub

#Region "Bloqueo de columnas segun accion"

    Protected Sub gvCuanti_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCuanti.RowCreated
        'If Request.QueryString("ACCION") IsNot Nothing Then

        '    Select Case Request.QueryString("ACCION")
        '        Case "1"
        '            If (e.Row.RowType = DataControlRowType.DataRow) Then
        '                'REVISON
        '                e.Row.Cells(9).Enabled = True
        '                'APROBACION
        '                e.Row.Cells(8).Enabled = True

        '            End If
        '        Case Else
        '            If (e.Row.RowType = DataControlRowType.DataRow) Then
        '                e.Row.Cells(9).Enabled = False
        '                e.Row.Cells(8).Enabled = False

        '            End If

        '    End Select
        'End If
    End Sub

    Protected Sub gvNacionales_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'ESTA COLUMNAS SIEMPRE DEBEN ESTAR OCULTAS , NO SON IMPORTANTES PARA E USUARIO
        'ID DE LA FASE
        e.Row.Cells(1).Visible = False
        'ID DE LA METODOLOGIA 
        e.Row.Cells(3).Visible = False
        'BLOQUEAR SIEMPRE EL CAMBIO DE GROSS MARGIN 
        'e.Row.Cells(11).Enabled = False
        'INCIAMOS CON LA COLUMNA DE REVISION BLOQUEADA
        e.Row.Cells(12).Enabled = False


        If Request.QueryString("ACCION") IsNot Nothing Then

            Select Case Request.QueryString("ACCION")
                Case "2"
                    'REVISION 
                    'SE  PUEDE MODIFICAR EL GROSS
                    e.Row.Cells(11).Enabled = True
                    'SE HABILITA LA CASILLA DE REVISION 
                    e.Row.Cells(12).Enabled = True


                Case "1"
                    'MODIFICACION
                    'SE PUEDE MODIFICAR EL GROSS
                    e.Row.Cells(11).Enabled = True

                Case "3"
                    'CONSULTA 


                Case "4"
                    'MODIFICACION DE GROSS MARGIN
                    'If (e.Row.RowType = DataControlRowType.DataRow) Then
                    'End If
                    e.Row.Cells(11).Enabled = True

                Case "5"
                    'Consulta

                Case Else
                    'BLOQUEAMOS EL LA MODIFICACION DEL GROSS MARGIN 
                    ' If (e.Row.RowType = DataControlRowType.DataRow) Then
                    'End If



            End Select
        Else
            'SI NO HAY ACCION BLOQUEAMOS LA REVISON 
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                e.Row.Cells(12).Enabled = False


            End If

        End If
    End Sub
#End Region

    Protected Sub btnCambioGeneralGM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCambioGeneralGM.Click
        Try


            hfTipoCalculo.Value = "2"
            UpdatePanel5.Update()
            'lblgmActual.Text = hfTope.Value
            lblContrasena.Visible = False
            gmTxtContrasena.Visible = False
            txtNuevoGM.Text = ""
            lkgm_ModalPopupExtender.Show()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    Private Sub btnJobBookExt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnJobBookExt.Click

        If Request.QueryString("ACCION") IsNot Nothing Then
            Response.Redirect("CostosJBExterno.aspx?JOBBOOK=" & iqLbljobBook.Text & "&ACCION=" & Request.QueryString("ACCION"))
        Else
            Response.Redirect("CostosJBExterno.aspx?JOBBOOK=" & iqLbljobBook.Text)
        End If



    End Sub

    Protected Sub btnJobBookInt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnJobBookInt.Click

        If Request.QueryString("ACCION") IsNot Nothing Then
            Response.Redirect("CostosJBInterno.aspx?JOBBOOK=" & iqLbljobBook.Text & "&ACCION=" & Request.QueryString("ACCION"))
        Else
            Response.Redirect("CostosJBInterno.aspx?JOBBOOK=" & iqLbljobBook.Text)
        End If

    End Sub

    Protected Sub txtDiasCampo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDiasCampo.TextChanged
        If txtDiasCampo.Text <> "" Then
            DiasCampo = CInt(txtDiasCampo.Text)
            DiasDiseno = CInt(txtDiasDiseno.Text)
            DiasProceso = CInt(txtDiasProceso.Text)
            DiasInformes = CInt(txtDiasInformes.Text)
            SumarDias()
        End If

    End Sub
    Protected Sub txtDias_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDiasDiseno.TextChanged
        If txtDiasDiseno.Text <> "" Then
            DiasCampo = CInt(txtDiasCampo.Text)
            DiasDiseno = CInt(txtDiasDiseno.Text)
            DiasProceso = CInt(txtDiasProceso.Text)
            DiasInformes = CInt(txtDiasInformes.Text)
            SumarDias()
        End If
    End Sub
    Protected Sub txtDiasProceso_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDiasProceso.TextChanged
        If txtDiasProceso.Text <> "" Then
            DiasCampo = CInt(txtDiasCampo.Text)
            DiasDiseno = CInt(txtDiasDiseno.Text)
            DiasProceso = CInt(txtDiasProceso.Text)
            DiasInformes = CInt(txtDiasInformes.Text)
            SumarDias()
        End If
    End Sub
    Protected Sub txtDiasInformes_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDiasInformes.TextChanged
        If txtDiasInformes.Text <> "" Then
            DiasCampo = CInt(txtDiasCampo.Text)
            DiasDiseno = CInt(txtDiasDiseno.Text)
            DiasProceso = CInt(txtDiasProceso.Text)
            DiasInformes = CInt(txtDiasInformes.Text)
            SumarDias()
        End If
    End Sub
    Private Sub SumarDias()
        Try
            DiasCampo = CInt(txtDiasCampo.Text)
            DiasDiseno = CInt(txtDiasDiseno.Text)
            DiasProceso = CInt(txtDiasProceso.Text)
            DiasInformes = CInt(txtDiasInformes.Text)
            txtDiasTotal.Text = (DiasDiseno + DiasCampo + DiasInformes + DiasProceso).ToString()

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try

    End Sub
    Protected Sub CCGvMuestra_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles CCGvMuestra.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then

            e.Row.Cells(1).Visible = False

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim delButton As Button = CType(e.Row.Cells(0).Controls(1), Button)
            delButton.CommandArgument = e.Row.RowIndex.ToString()
        End If

    End Sub

    Protected Sub gvSesionesMuestra_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSesionesMuestra.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Visible = False
        End If

    End Sub

    Protected Sub gvEntrevistasMuestra_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEntrevistasMuestra.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Visible = False
        End If


    End Sub

    Protected Sub gvSesionesMuestra_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSesionesMuestra.RowCommand
        Try
            Select Case e.CommandName

                Case "DEL"
                    Dim m As New IQ_Muestra_1
                    Dim par As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)

                    m.IdPropuesta = par.IdPropuesta
                    m.ParAlternativa = par.ParAlternativa
                    m.MetCodigo = par.MetCodigo
                    m.ParNacional = par.ParNacional
                    m.CiuCodigo = CInt(gvSesionesMuestra.Rows(e.CommandArgument).Cells(1).Text)
                    m.MuIdentificador = 1
                    _CaraCara.BorrarCiudadMuestra(m)
                    CargarMuestraSesionesEntrevistas(gvSesionesMuestra, par)
                    sesLblTotalMuestra.Text = _sesiones.TotalizarMuestra(par)
                    upotr1.Update()


            End Select



        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Protected Sub gvEntrevistasMuestra_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEntrevistasMuestra.RowCommand
        Try
            Select Case e.CommandName

                Case "DEL"
                    Dim m As New IQ_Muestra_1
                    Dim par As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)

                    m.IdPropuesta = par.IdPropuesta
                    m.ParAlternativa = par.ParAlternativa
                    m.MetCodigo = par.MetCodigo
                    m.ParNacional = par.ParNacional
                    m.CiuCodigo = CInt(gvEntrevistasMuestra.Rows(e.CommandArgument).Cells(1).Text)
                    m.MuIdentificador = 1
                    _CaraCara.BorrarCiudadMuestra(m)
                    CargarMuestraSesionesEntrevistas(gvEntrevistasMuestra, par)
                    entLblTotalMuestra.Text = _entrevistas.TotalizarMuestra(par)

                    upEntDeptos.Update()

            End Select



        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Protected Sub EntLstMetodologia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles EntLstMetodologia.SelectedIndexChanged
        If EntLstMetodologia.SelectedIndex > 0 Then
            CargarTipoMuestra(EntLstTipoMuestra, CInt(EntLstMetodologia.SelectedValue))


            If EntLstFase.SelectedIndex > -1 Then
                Dim par As New IQ_Parametros

                par.IdPropuesta = CType(Session("PARAMETROS"), IQ_Parametros).IdPropuesta
                par.ParAlternativa = CType(Session("PARAMETROS"), IQ_Parametros).ParAlternativa
                par.ParNacional = CInt(EntLstFase.SelectedValue)
                par.MetCodigo = CInt(EntLstMetodologia.SelectedValue)

                par.ParUnidad = CType(Session("PARAMETROS"), IQ_Parametros).ParUnidad
                Session("PARAMETROS") = par
                If (_Cati.ExisteCati(CType(Session("PARAMETROS"), IQ_Parametros))) Then
                    EntLstMetodologia.Enabled = False
                    EntLstFase.Enabled = False
                    CargarEntrevistas(par)
                Else
                    'SE DEBEN LIMPIAR LOS CONTROLES EXCEPTO LA METODOLOGIA Y Y LA OPCION DE NACIONAL
                End If

            Else
                ShowNotification("Seleccione la  fase ", WebMatrix.ShowNotifications.ErrorNotification)
                EntLstMetodologia.SelectedIndex = 0
            End If

        End If

    End Sub

    Protected Sub SesLstMetodologia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles SesLstMetodologia.SelectedIndexChanged
        Try
            If SesLstMetodologia.SelectedIndex > 0 Then
                CargarTipoMuestra(sesLstTipoMuestra, CInt(SesLstMetodologia.SelectedValue))

                If sesLstFase.SelectedIndex > -1 Then
                    Dim par As New IQ_Parametros

                    par.IdPropuesta = CType(Session("PARAMETROS"), IQ_Parametros).IdPropuesta
                    par.ParAlternativa = CType(Session("PARAMETROS"), IQ_Parametros).ParAlternativa
                    par.ParNacional = CInt(sesLstFase.SelectedValue)
                    par.MetCodigo = CInt(SesLstMetodologia.SelectedValue)

                    par.ParUnidad = CType(Session("PARAMETROS"), IQ_Parametros).ParUnidad
                    Session("PARAMETROS") = par
                    If (_Cati.ExisteCati(CType(Session("PARAMETROS"), IQ_Parametros))) Then
                        SesLstMetodologia.Enabled = False
                        sesLstFase.Enabled = True
                        CargarSesiones(par)

                    Else
                        'SE DEBEN LIMPIAR LOS CONTROLES EXCEPTO LA METODOLOGIA Y Y LA OPCION DE NACIONAL
                        SesTxtCantPart.Text = (_sesiones.ObtenerNumAsistentes(CInt(SesLstMetodologia.SelectedValue))).ToString()
                    End If

                Else
                    ShowNotification("Seleccione la  fase ", WebMatrix.ShowNotifications.ErrorNotification)
                    SesLstMetodologia.SelectedIndex = 0
                End If

            End If

        Catch ex As Exception

        End Try


    End Sub

#Region "Online"

    Private Sub CargarProcesos(ByVal tecnica As Integer, ByVal lst As CheckBoxList)
        Try
            lst.DataSource = _OnLine.ObtenerProcesosXTecnica(tecnica)
            lst.DataTextField = "ProcDescripcion"
            lst.DataValueField = "ProcCodigo"
            lst.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function ValidarOnline(ByVal todo As Integer) As String
        Dim mensaje As String = ""


        If CType(UC_Producto3.FindControl("lstOferta"), DropDownList).SelectedIndex = 0 Then
            mensaje = "Seleccione una oferta!"
            Return mensaje
        End If

        If CType(UC_Producto3.FindControl("lstProducto"), DropDownList).SelectedIndex = 0 Then
            mensaje = "Seleccione un producto!"
            Return mensaje
        End If

        If onTxtGrupoObjetivo.Text.Trim = "" Then
            mensaje = "Mencione el grupo objetivo!"
            Return mensaje
        End If

        If ContarProcesos(onChkProcesos) = 0 Then
            mensaje = "Seleccione al menos un proceso!"
            Return mensaje
        End If

        'Saber si chequeo DC, TL,Tablas,Bases
        Dim WEtapa6 As Boolean
        Dim WEtapa7 As Boolean
        Dim WEtapa8 As Boolean
        Dim WEtapa9 As Boolean
        WEtapa6 = False
        WEtapa7 = False
        WEtapa8 = False
        WEtapa9 = False
        For i = 0 To onChkProcesos.Items.Count - 1
            If onChkProcesos.Items(i).Selected Then
                If CInt(onChkProcesos.Items(i).Value) = 6 And (onTxtNumProcesosDC.Text = "" Or CInt(onTxtNumProcesosDC.Text) = 0) Then
                    mensaje = " Data Clean"
                End If
                If CInt(onChkProcesos.Items(i).Value) = 7 And (onTxtNumProcesosTL.Text = "" Or CInt(onTxtNumProcesosTL.Text) = 0) Then
                    mensaje = mensaje & " Top Lines"
                End If
                If CInt(onChkProcesos.Items(i).Value) = 8 And (onTxtNumProcesosTablas.Text = "" Or CInt(onTxtNumProcesosTablas.Text) = 0) Then
                    mensaje = mensaje & " Tablas"
                End If
                If CInt(onChkProcesos.Items(i).Value) = 9 And (onTxtNumProcesosBases.Text = "" Or CInt(onTxtNumProcesosBases.Text) = 0) Then
                    mensaje = mensaje & " Archivos"
                End If

                If CInt(onChkProcesos.Items(i).Value) = 6 Then
                    WEtapa6 = True
                End If
                If CInt(onChkProcesos.Items(i).Value) = 7 Then
                    WEtapa7 = True
                End If
                If CInt(onChkProcesos.Items(i).Value) = 8 Then
                    WEtapa8 = True
                End If
                If CInt(onChkProcesos.Items(i).Value) = 9 Then
                    WEtapa9 = True
                End If
            End If
        Next

        If mensaje <> "" Then
            mensaje = "Digite numero de procesos para : " & mensaje
            Return mensaje
        End If

        'Chequea si digitaron numero de procesos pero no marcaron la etapa
        If CInt(onTxtNumProcesosDC.Text) > 0 And WEtapa6 = False Then
            mensaje = "Debe marcar la etapa DATACLEAN si registra procesos : " & mensaje
            Return mensaje
        End If
        If CInt(onTxtNumProcesosTL.Text) > 0 And WEtapa7 = False Then
            mensaje = "Debe marcar la etapa TOPLINES si registra procesos : " & mensaje
            Return mensaje
        End If
        If CInt(onTxtNumProcesosTablas.Text) > 0 And WEtapa8 = False Then
            mensaje = "Debe marcar la etapa PROCESO si registra procesos : " & mensaje
            Return mensaje
        End If
        If CInt(onTxtNumProcesosBases.Text) > 0 And WEtapa9 = False Then
            mensaje = "Debe marcar la etapa ARCHIVOS si registra procesos : " & mensaje
            Return mensaje
        End If

        If onLstMetodologia.SelectedIndex = 0 Then
            mensaje = "Seleccione la metodologia!"
            Return mensaje
        End If

        If todo = 1 Then
            If onTxtCantidad.Text = "" Then

                mensaje = "Digite la muestra!"
                Return mensaje
            End If

            If onLstTipoMuestra.SelectedIndex = 0 Then

                mensaje = "Seleccione la dificultad de la muestra!"
                Return mensaje
            End If

        End If


        Return mensaje
    End Function

    Private Sub GuardarOnLIne(ByVal p As IQ_Parametros, ByVal todo As Integer)
        Try
            Dim par As New IQ_Parametros
            par.IdPropuesta = p.IdPropuesta
            par.ParAlternativa = p.ParAlternativa
            par.ParUnidad = p.ParUnidad
            par.Usuario = CDec(Session("IDUsuario"))
            par.ParFechaCreacion = DateTime.Now
            par.ParNacional = CInt(onLstFase.SelectedValue)

            If OnchkAñoActual.Checked = True And OnchkAñoSiguiente.Checked = False Then
                par.ParAñoSiguiente = False
            Else
                If OnchkAñoActual.Checked = False And OnchkAñoSiguiente.Checked = True Then
                    par.ParAñoSiguiente = True
                Else
                    If OnchkAñoActual.Checked = False And OnchkAñoSiguiente.Checked = False Then
                        par.ParAñoSiguiente = True
                    Else
                        If OnchkAñoActual.Checked = True And OnchkAñoSiguiente.Checked = True Then
                            par.ParAñoSiguiente = True
                        End If
                    End If
                End If
            End If

            par.Pr_ProductCode = CType(UC_Producto3.FindControl("lstProducto"), DropDownList).SelectedValue
            par.Pr_Offeringcode = CType(UC_Producto3.FindControl("lstOferta"), DropDownList).SelectedValue
            par.ParGrupoObjetivo = onTxtGrupoObjetivo.Text
            par.TecCodigo = CInt(hfTecnica.Value)
            par.MetCodigo = CInt(onLstMetodologia.SelectedValue)

            Dim Preg As New IQ_Preguntas
            Preg.IdPropuesta = par.IdPropuesta
            Preg.ParAlternativa = par.ParAlternativa
            Preg.MetCodigo = par.MetCodigo
            Preg.ParNacional = par.ParNacional
            Preg.PregAbiertas = CType(UC_Producto3.FindControl("AbiertasReal"), TextBox).Text
            Preg.PregAbiertasMultiples = CType(UC_Producto3.FindControl("AbiertasMultReal"), TextBox).Text
            Preg.PregCerradas = CType(UC_Producto3.FindControl("CerradasReal"), TextBox).Text
            Preg.PregCerradasMultiples = CType(UC_Producto3.FindControl("CerradasMultReal"), TextBox).Text
            Preg.PregOtras = CType(UC_Producto3.FindControl("OtrosReal"), TextBox).Text
            Preg.PregDemograficos = CType(UC_Producto3.FindControl("DemoReal"), TextBox).Text
            par.ParTiempoEncuesta = If(CType(UC_Producto3.FindControl("txtDuracion"), TextBox).Text = "", 0, CInt(CType(UC_Producto3.FindControl("txtDuracion"), TextBox).Text))
            par.ParObservaciones = OnTxtObservaciones.Text
            par.TipoProyecto = 1
            par.ParNomPresupuesto = lblNomPropuesta.Text
            par.ParNProcesosDC = CInt(onTxtNumProcesosDC.Text)
            par.ParNProcesosTopLines = CInt(onTxtNumProcesosTL.Text)
            par.ParNProcesosTablas = CInt(onTxtNumProcesosTablas.Text)
            par.ParNProcesosBases = CInt(onTxtNumProcesosBases.Text)

            'Agregamos los procesos 
            Dim lstProc As New List(Of IQ_ProcesosPresupuesto)
            For i = 0 To onChkProcesos.Items.Count - 1
                If onChkProcesos.Items(i).Selected Then
                    Dim P1 = New IQ_ProcesosPresupuesto()
                    P1.IdPropuesta = par.IdPropuesta
                    P1.ParAlternativa = par.ParAlternativa
                    P1.MetCodigo = par.MetCodigo
                    P1.ParNacional = par.ParNacional
                    P1.Porcentaje = 0
                    P1.ProcCodigo = CInt(onChkProcesos.Items(i).Value)
                    lstProc.Add(P1)
                End If

            Next

            'Agregamos  los valores de la muestra teniendo en cuenta las ciudades
            Dim lstMuestra As New List(Of IQ_Muestra_1)
            If todo = 1 Then
                Dim m As New IQ_Muestra_1
                m.IdPropuesta = par.IdPropuesta
                m.ParAlternativa = par.ParAlternativa
                m.ParNacional = par.ParNacional
                m.MetCodigo = par.MetCodigo
                m.CiuCodigo = 11001
                m.DeptCodigo = 11
                m.MuCantidad = CInt(onTxtCantidad.Text)
                m.MuIdentificador = CInt(onLstTipoMuestra.SelectedValue)
                lstMuestra.Add(m)
            End If

            par.ParProductividad = If(onTxtProductividad.Text = "", 0.0, CDec(onTxtProductividad.Text))

            _OnLine.InsertarPresupuesto(par, lstMuestra, lstProc, Preg)
            CargarMuestraOnLine(gvOnLineMuestra, par)
            onTxtCantidad.Text = ""
            Session("PARAMETROS") = par
            If todo = 7 Then
                EfectuarCalculosOnLIne(par, 1)
            Else
                EfectuarCalculosOnLIne(par, 0)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub CargarOnline(ByVal p As IQ_Parametros)
        Try
            Dim par As IQ_Parametros
            par = _OnLine.ObtenerPresupuesto(p)
            Session("PARAMETROS") = par
            onLstFase.SelectedIndex = onLstFase.Items.IndexOf(onLstFase.Items.FindByValue(par.ParNacional))

            If par.ParAñoSiguiente = True Then
                OnchkAñoActual.Checked = False
                OnchkAñoSiguiente.Checked = True
            Else
                OnchkAñoActual.Checked = True
                OnchkAñoSiguiente.Checked = False
            End If

            onLstMetodologia.SelectedIndex = onLstMetodologia.Items.IndexOf(onLstMetodologia.Items.FindByValue(par.MetCodigo))
            CargarTipoMuestra(onLstTipoMuestra, CInt(onLstMetodologia.SelectedValue))
            onTxtGrupoObjetivo.Text = par.ParGrupoObjetivo.ToString()
            CType(UC_Producto3.FindControl("LstOferta"), DropDownList).SelectedIndex = CType(UC_Producto3.FindControl("LstOferta"), DropDownList).Items.IndexOf(CType(UC_Producto3.FindControl("LstOferta"), DropDownList).Items.FindByValue(par.Pr_Offeringcode))
            UC_Producto3.cargarProductos()
            CType(UC_Producto3.FindControl("LstProducto"), DropDownList).SelectedIndex = CType(UC_Producto3.FindControl("LstProducto"), DropDownList).Items.IndexOf(CType(UC_Producto3.FindControl("LstProducto"), DropDownList).Items.FindByValue(par.Pr_ProductCode))
            UC_Producto3.CargarPreguntasPropuestas()
            CType(UC_Producto3.FindControl("AbiertasReal"), TextBox).Text = par.IQ_Preguntas.PregAbiertas
            CType(UC_Producto3.FindControl("AbiertasMultReal"), TextBox).Text = par.IQ_Preguntas.PregAbiertasMultiples
            CType(UC_Producto3.FindControl("CerradasReal"), TextBox).Text = par.IQ_Preguntas.PregCerradas
            CType(UC_Producto3.FindControl("CerradasMultReal"), TextBox).Text = par.IQ_Preguntas.PregCerradasMultiples
            CType(UC_Producto3.FindControl("OtrosReal"), TextBox).Text = par.IQ_Preguntas.PregOtras
            CType(UC_Producto3.FindControl("DemoReal"), TextBox).Text = par.IQ_Preguntas.PregDemograficos
            CType(UC_Producto3.FindControl("TxtDuracion"), TextBox).Text = par.ParTiempoEncuesta.ToString()

            onTxtProductividad.Text = par.ParProductividad.ToString()
            OnTxtObservaciones.Text = If(par.ParObservaciones Is Nothing, "", par.ParObservaciones)
            onTxtNumProcesosDC.Text = par.ParNProcesosDC.ToString()
            onTxtNumProcesosTL.Text = par.ParNProcesosTopLines.ToString()
            onTxtNumProcesosTablas.Text = par.ParNProcesosTablas.ToString()
            onTxtNumProcesosBases.Text = par.ParNProcesosBases.ToString()
            CargarProcesos(par.TecCodigo, onChkProcesos)

            For i = 0 To onChkProcesos.Items.Count - 1
                If _OnLine.ExisteProceso(par.IQ_ProcesosPresupuesto.ToList(), CInt(onChkProcesos.Items(i).Value)) Then
                    onChkProcesos.Items(i).Selected = True
                End If
            Next


            'CARGA LA GRILLA DE MUESTRA 
            CargarMuestraOnLine(gvOnLineMuestra, par)
            upOnline.Update()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub onLstMetodologia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles onLstMetodologia.SelectedIndexChanged
        If onLstMetodologia.SelectedIndex > 0 Then
            CargarTipoMuestra(onLstTipoMuestra, CInt(onLstMetodologia.SelectedValue))
        End If
    End Sub
    Protected Sub BtnOnLineAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnOnLineAgregar.Click
        Dim mensaje As String
        mensaje = ValidarOnline(1)
        If mensaje = "" Then
            GuardarOnLIne(CType(Session("PARAMETROS"), IQ_Parametros), 1)
        Else
            ShowNotification(mensaje, WebMatrix.ShowNotifications.ErrorNotification)
        End If
    End Sub

    Private Sub CargarMuestraOnLine(ByVal gv As GridView, ByVal par As IQ_Parametros)
        Try
            gv.DataSource = _OnLine.ObtenerMuestra(par)
            gv.DataBind()


        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub gvOnLineMuestra_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOnLineMuestra.RowCreated
        e.Row.Cells(0).Visible = False
    End Sub

    Private Sub EfectuarCalculosOnLIne(ByVal p As IQ_Parametros, ByVal CalculoAuto As Integer)
        Try
            p.ParProductividad = If(onTxtProductividad.Text = "", 0.0, CDec(onTxtProductividad.Text))
            _OnLine.calcularActSubcontratadas(p)
            If p.ParProductividad = 0 Or CalculoAuto = 1 Then
                p.ParProductividad = _OnLine.CalcularPoductividad(p)
            End If

            p.ParCostoDirecto = _OnLine.ObtenerCostoDirecto(p)
            p.ParValorVenta = _OnLine.ValorVenta(p)
            'Obtenemos los valores que no aplican para el gross margin pues estos tiene su propio porcentaje y los  restamos las actividades  subcontratadas
            Dim ValorActividadesNoGM As Decimal
            ValorActividadesNoGM = _Cati.ActividadesNoAplicanGM(p)
            Dim ValorActividadesNoGMConAdmon As Decimal

            ValorActividadesNoGMConAdmon = (ValorActividadesNoGM * 1.1)
            p.ParGrossMargin = ((p.ParValorVenta) - (p.ParCostoDirecto + ((p.ParActSubGasto)))) / (p.ParValorVenta)
            Session("PARAMETROS") = p
            onTxtProductividad.Text = p.ParProductividad.ToString()
            'actualizams unicamente el gross amrgin y el valor de venta  pues los demas ya han sido actuaizados desde el procedimiento
            _OnLine.ActualizarCalculos(p)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region
    Protected Sub gvMuestracati_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMuestracati.RowCreated
        e.Row.Cells(1).Visible = False
    End Sub

    Protected Sub gvMuestracati_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMuestracati.RowCommand
        Try
            Select Case e.CommandName

                Case "DEL"
                    Dim m As New IQ_Muestra_1
                    Dim par As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)

                    m.IdPropuesta = par.IdPropuesta
                    m.ParAlternativa = par.ParAlternativa
                    m.MetCodigo = par.MetCodigo
                    m.ParNacional = par.ParNacional
                    m.CiuCodigo = 0
                    m.MuIdentificador = CInt(gvMuestracati.Rows(e.CommandArgument).Cells(1).Text)
                    _Cati.BorrarMuestra(m)
                    CargarMuestraCati(par)
            End Select

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub



    Protected Sub gvNacionales_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        'Dim cb1 As CheckBox = CType(e.Row.FindControl("CheckBox1"), CheckBox)
        'ScriptManager.GetCurrent(Me).RegisterAsyncPostBackControl(cb1)
        If (e.Row.RowType = DataControlRowType.DataRow) Then

            Dim btn As ImageButton = e.Row.FindControl("ImageButton1")
            If (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "REVISADO"))) Then
                btn.ImageUrl = "~/CAP/Imagenes/checkbox-checked.png"
                btn.Attributes.Add("onclick", "javascript" & ":return confirm('Esta seguro que desea marcar este presupuesto como NO REVISADO');")
                'btn.CommandArgument = e.Row.RowIndex.ToString()
            Else
                btn.ImageUrl = "~/CAP/Imagenes/checkbox-unchecked.png"
                btn.Attributes.Add("onclick", "javascript" & ":return confirm('Esta seguro que desea marcar este presupuesto como  REVISADO');")
                ' btn.CommandArgument = e.Row.RowIndex.ToString()
            End If


        End If



        If e.Row.DataItem IsNot Nothing Then
            Dim flag As Boolean = False
            'APROBADO
            If CType(e.Row.Cells(13).FindControl("CheckBox2"), CheckBox).Checked Then
                'BLOQUEAMOS LA OPCION DE REVISADO 
                e.Row.Cells(12).Enabled = False
                'BLOQUEAMOS LA OPCION DE ELIMINAR 
                e.Row.Cells(15).Enabled = False
                'BLOQUEMOS LA MODIFICAION DEL GROSS MARGIN
                e.Row.Cells(11).Enabled = False
                btnOk.Enabled = False
                btnGuardar.Enabled = False
                flag = True
            End If
            'REVISADO - NO BLOQUEAMOS EL GROSS MARGIN SOLO LA OPCION DE ELIMINAR
            If CType(e.Row.Cells(12).FindControl("ImageButton1"), ImageButton).ImageUrl = "~/CAP/Imagenes/checkbox-checked.png" Then
                'BLOQUEAMOS LA OPCION DE ELIMINAR 
                e.Row.Cells(15).Enabled = False
            End If
            If flag = False Then
                btnOk.Enabled = True
                btnGuardar.Enabled = True
            End If

        End If

    End Sub


    Protected Sub btnGuardarDatosGenerales_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardarDatosGenerales.Click
        GuardarDatosGenerales()
    End Sub

#Region "Cargue archivos excel"
    Protected Sub AsyncFileUpload1_UploadedComplete(ByVal sender As Object, ByVal e As AjaxControlToolkit.AsyncFileUploadEventArgs) Handles AsyncFileUpload1.UploadedComplete

        Dim savePath As String = Server.MapPath("Archivos/" & "Muestra_" & Session("IdUsuario") & Path.GetExtension(AsyncFileUpload1.FileName))
        AsyncFileUpload1.SaveAs(savePath)
        'sino se genero ningun error en el cargue se debe validar el archivo  que las ciudades y los codigos coicidan 
        ClearContents(CType(sender, Control))

    End Sub

    Private Sub ClearContents(ByVal Control As Control)
        For i As Integer = 0 To Session.Keys.Count - 1
            If Session.Keys(i).Contains(Control.ClientID) Then
                Session.Remove(Session.Keys(i))
                Exit For
            End If
        Next
    End Sub
#End Region




    Protected Sub EnviarNotificacion0_Click(ByVal sender As Object, ByVal e As EventArgs) Handles EnviarNotificacion0.Click
        Dim oEnviarCorreo As New EnviarCorreo
        Dim P As IQ_Parametros
        P = CType(Session("PARAMETROS"), IQ_Parametros)
        If hfTipoCalculo.Value = "1" Then
            'ENVIAR CORREO DE NOTIFICACION GM EN ESTE PUNTO , EL LINK DEBE TENER LOS SIGUIENTES DATOS 
            Dim strRedireccionar1 As String
            strRedireccionar1 = "/CAP/Cap_Principal.aspx?IdPropuesta=" & P.IdPropuesta & "&Alternativa=" & P.ParAlternativa & "&Fase" & P.ParNacional & "&Metodologia=" & P.MetCodigo & "&GMU=" & txtNuevoGM.Text & "&TipoCalculo=" & hfTipoCalculo.Value


            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/SolicitudCambioGrossMargin.aspx?IdPropuesta=" & P.IdPropuesta & "&Alternativa=" & P.ParAlternativa & "&Metodologia=" & P.MetCodigo & "&GMU=" & txtNuevoGM.Text & "&TipoCalculo=" & hfTipoCalculo.Value & "&Fase=" & P.ParNacional & "&Unidad=" & P.ParUnidad & "&GMO=" & txtGMOpera.Text)

            lkgm_ModalPopupExtender.Hide()
            ShowNotification("Solicitud de cambio  enviada  ", WebMatrix.ShowNotifications.InfoNotification)
        ElseIf hfTipoCalculo.Value = "2" Then
            'ENVIAR CORREO DE NOTIFICACION GM EN ESTE PUNTO , EL LINK DEBE TENER LOS SIGUIENTES DATOS 
            Dim strRedireccionar2 As String
            strRedireccionar2 = "/CAP/Cap_Principal.aspx?IdPropuesta=" & P.IdPropuesta & "&Alternativa=" & P.ParAlternativa & "&GMU=" & txtNuevoGM.Text & "&TipoCalculo=" & hfTipoCalculo.Value & "&GMO=" & txtGMOpera.Text


            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/SolicitudCambioGrossMargin.aspx?IdPropuesta=" & P.IdPropuesta & "&Alternativa=" & P.ParAlternativa & "&GMU=" & txtNuevoGM.Text & "&TipoCalculo=" & hfTipoCalculo.Value & "&Unidad=" & P.ParUnidad & "&GMO=" & txtGMOpera.Text)

            lkgm_ModalPopupExtender.Hide()
            ShowNotification("Solicitud de cambio enviada ! ", WebMatrix.ShowNotifications.InfoNotification)
        End If
    End Sub


    Protected Sub gvMuestracati_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMuestracati.PageIndexChanging
        gvMuestracati.PageIndex = e.NewPageIndex
        CargarMuestraCati(CType(Session("PARAMETROS"), IQ_Parametros))
    End Sub

    Protected Sub CCGvMuestra_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles CCGvMuestra.PageIndexChanging
        CCGvMuestra.PageIndex = e.NewPageIndex
        CargarCiudadesMuestra(CType(Session("PARAMETROS"), IQ_Parametros), CCGvMuestra)
    End Sub

    Protected Sub gvSesionesMuestra_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSesionesMuestra.PageIndexChanging
        gvSesionesMuestra.PageIndex = e.NewPageIndex
        CargarMuestraSesionesEntrevistas(gvSesionesMuestra, CType(Session("PARAMETROS"), IQ_Parametros))
    End Sub

    Protected Sub gvEntrevistasMuestra_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvEntrevistasMuestra.PageIndexChanging
        gvEntrevistasMuestra.PageIndex = e.NewPageIndex

        CargarMuestraSesionesEntrevistas(gvEntrevistasMuestra, CType(Session("PARAMETROS"), IQ_Parametros))
    End Sub

    Protected Sub btnDuplicarAlternativa_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDuplicarAlternativa.Click
        General.DuplicarAlternativa(CType(Session("PARAMETROS"), IQ_Parametros))

    End Sub

    Private Function CargarListaHoras(ByVal gv As GridView, ByVal p As IQ_Parametros) As List(Of IQ_HorasProfesionales)

        Dim gr As GridViewRow
        Dim lst As New List(Of IQ_HorasProfesionales)
        Dim Horas As IQ_HorasProfesionales
        For Each gr In gv.Rows
            If gr.RowType = DataControlRowType.DataRow Then
                ' If (CInt(CType(gr.Cells(2).Controls(1), TextBox).Text) > 0) Then
                Horas = New IQ_HorasProfesionales()
                Horas.IdPropuesta = p.IdPropuesta
                Horas.ParAlternativa = p.ParAlternativa
                Horas.MetCodigo = p.MetCodigo
                Horas.ParNacional = p.ParNacional
                Horas.CodCargo = CInt(gr.Cells(0).Text)
                Horas.Horas = CInt(CType(gr.Cells(2).Controls(1), TextBox).Text)
                lst.Add(Horas)
                'End If


            End If
        Next
        Return lst

    End Function

    Protected Sub Button7_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button7.Click

        gvMuestraNueva.DataSource = _general.ObtenerCiudadesMuestra()
        gvMuestraNueva.DataBind()
        lkmu_ModalPopupExtender.Show()
    End Sub

    Protected Sub btnSalirMu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSalirMu.Click

        lkmu_ModalPopupExtender.Hide()
        UpdatePanel4.Update()


    End Sub


    Protected Sub btnAddMuestra_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddMuestra.Click
        gvMuestraNueva.DataSource = _general.ObtenerCiudadesMuestra()
        gvMuestraNueva.DataBind()
        Dim mensaje As String = ""
        Dim flag As Boolean = False
        Select Case hfTecnica.Value
            Case 100
                mensaje = ValidarCaraCara(1)
                If mensaje = "" Then
                    CargarTipoMuestra(lstDificultadNuevaMuestra, CInt(CCLstMetodologia.SelectedValue))
                    Dim par As IQ_Parametros
                    par = CType(Session("PARAMETROS"), IQ_Parametros)
                    If _CaraCara.ExisteMuestra(par) Then
                        CargarCiudadesMuestra(par, gvMuestraIngresada)
                    End If

                    flag = True
                Else
                    ShowNotification(mensaje, WebMatrix.ShowNotifications.ErrorNotification)
                End If

            Case 200
                CargarTipoMuestra(lstDificultadNuevaMuestra, CInt(CCLstMetodologia.SelectedValue))
            Case 300
                CargarTipoMuestra(lstDificultadNuevaMuestra, CInt(CCLstMetodologia.SelectedValue))
            Case 600
                CargarTipoMuestra(lstDificultadNuevaMuestra, CInt(CCLstMetodologia.SelectedValue))
            Case 700

        End Select
        If flag = True Then
            UpdatePanel11.Update()
            lkmu_ModalPopupExtender.Show()
        End If

    End Sub

    Protected Sub gvMuestraNueva_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMuestraNueva.RowCommand
        Select Case e.CommandName
            Case "ADD"
                Dim Cantidad As String
                Cantidad = CType(gvMuestraNueva.Rows(e.CommandArgument).FindControl("txtCantMuestra"), TextBox).Text
                If Cantidad <> "" And lstDificultadNuevaMuestra.SelectedIndex > 0 Then
                    Dim par As IQ_Parametros
                    par = CType(Session("PARAMETROS"), IQ_Parametros)
                    Dim m As IQ_Muestra_1
                    Dim lstMuestra As New List(Of IQ_Muestra_1)
                    m = New IQ_Muestra_1
                    m.IdPropuesta = par.IdPropuesta
                    m.ParAlternativa = par.ParAlternativa
                    m.ParNacional = par.ParNacional
                    m.MetCodigo = par.MetCodigo
                    m.CiuCodigo = CInt(CType(gvMuestraNueva.Rows(e.CommandArgument).FindControl("Label2"), Label).Text)
                    m.DeptCodigo = CInt(CType(gvMuestraNueva.Rows(e.CommandArgument).FindControl("Label1"), Label).Text)
                    m.MuCantidad = CInt(Cantidad)
                    m.MuIdentificador = CInt(lstDificultadNuevaMuestra.SelectedValue)
                    lstMuestra.Add(m)
                    GuardarCaraCara(1, lstMuestra)
                    lblTotalMuestra1.Text = _CaraCara.TotalizarMuestra(par)

                    CargarCiudadesMuestra(par, gvMuestraIngresada)

                Else
                    ShowNotification("POR FAVOR DIGITE LA MUESTRA Y SELECCIONE LA DIFCULTAD !!", WebMatrix.ShowNotifications.ErrorNotification)
                End If



        End Select
    End Sub

    Protected Sub gvMuestraIngresada_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMuestraIngresada.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Visible = False

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(1).FindControl("Button1"), Button).CommandArgument = e.Row.RowIndex.ToString()
        End If
    End Sub

    Protected Sub gvMuestraIngresada_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMuestraIngresada.RowCommand
        Try
            Select Case e.CommandName

                Case "DEL"
                    Dim m As New IQ_Muestra_1
                    Dim par As IQ_Parametros = CType(Session("PARAMETROS"), IQ_Parametros)

                    m.IdPropuesta = par.IdPropuesta
                    m.ParAlternativa = par.ParAlternativa
                    m.MetCodigo = par.MetCodigo
                    m.ParNacional = par.ParNacional
                    m.CiuCodigo = CInt(gvMuestraIngresada.Rows(e.CommandArgument).Cells(1).Text)
                    m.MuIdentificador = 1
                    _CaraCara.BorrarCiudadMuestra(m)
                    CargarCiudadesMuestra(par, gvMuestraIngresada)
                    lblTotalMuestra1.Text = _CaraCara.TotalizarMuestra(par)
                    CargarCiudadesMuestra(par, CCGvMuestra)
                    lblTotalAlta.Text = _CaraCara.TotalizarMuestra(par)


            End Select

        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub


    Protected Sub gvMuestraIngresada_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMuestraIngresada.PageIndexChanging
        gvMuestraIngresada.PageIndex = e.NewPageIndex
        CargarCiudadesMuestra(CType(Session("PARAMETROS"), IQ_Parametros), gvMuestraIngresada)
    End Sub

    Protected Sub SesChkSubcontratar_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles SesChkSubcontratar.CheckedChanged
        If SesChkSubcontratar.Checked Then
            sestxtSubcontratar.Visible = True
        Else
            sestxtSubcontratar.Visible = False
            sestxtSubcontratar.Text = ""
        End If
    End Sub

    Protected Sub EntChkSubcontratar_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles EntChkSubcontratar.CheckedChanged
        If EntChkSubcontratar.Checked Then
            EnttxtSubcontratar.Visible = True
        Else
            EnttxtSubcontratar.Visible = False
            EnttxtSubcontratar.Text = ""
        End If
    End Sub

    Protected Sub gvActSubCont_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim s As String

            CType(e.Row.FindControl("txtValorAct"), TextBox).Text = DataBinder.Eval(e.Row.DataItem, "VALOR", "{0:C}")
            s = CType(e.Row.FindControl("txtValorAct"), TextBox).Text

        End If
    End Sub

#Region "Cargue desde excel"
    Protected Sub btnCargarEx_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCargarEx.Click
        Try
            Dim p As IQ_Parametros
            p = CType(Session("PARAMETROS"), IQ_Parametros)
            Dim lstCargue As List(Of IQ_Muestra_1)
            lstCargue = _CaraCara.CargarMuestraDesdeExcel(p, Server.MapPath("Archivos/" & "Muestra_" & Session("IdUsuario") & ".XLSX"))
            If _CaraCara.ValidarExistenciaCiudades(lstCargue) And _CaraCara.ValidarIdentificadorDistribucion(lstCargue) Then
                GuardarCaraCara(1, lstCargue)
                lblTotalMuestra1.Text = _CaraCara.TotalizarMuestra(p)

                CargarCiudadesMuestra(p, gvMuestraIngresada)
            End If


        Catch ex As Exception
            ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
        End Try

    End Sub
#End Region

#Region "Mistery "
    Protected Sub lstTipoServicio_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstTipoServicio.SelectedIndexChanged

        ObenerValorVisitaMistery()
    End Sub

    Protected Sub lstEvidencia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstEvidencia.SelectedIndexChanged
        ObenerValorVisitaMistery()
    End Sub

    Protected Sub lstTipoEvidencia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstTipoEvidencia.SelectedIndexChanged

        If lstTipoEvidencia.SelectedIndex > 0 Then
            Dim T As New IQ_TipoEvidencia
            T.TE_Id = CInt(lstTipoEvidencia.SelectedValue)
            CCTxtTiempoCritica.Text = _CaraCara.ObtenerTiempoCritica(T).ToString()
        End If
        ObenerValorVisitaMistery()
    End Sub

    Protected Sub lstNumContactos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstNumContactos.SelectedIndexChanged
        ObenerValorVisitaMistery()
    End Sub

    Private Sub ObenerValorVisitaMistery()
        If lstTipoServicio.SelectedIndex > 0 And lstTipoEvidencia.SelectedIndex > 0 And lstEvidencia.SelectedIndex > 0 And lstNumContactos.SelectedIndex > 0 Then
            Dim v As New IQ_TarifasMistery()
            v.TS_Id = CInt(lstTipoServicio.SelectedValue)
            v.TE_Id = CInt(lstTipoEvidencia.SelectedValue)
            v.EV_Id = CInt(lstEvidencia.SelectedValue)
            v.CC_Id = CInt(lstNumContactos.SelectedValue)
            txtValor.Text = _CaraCara.ObtenerValorVistaMistery(v).ToString()
        End If

    End Sub

    Protected Sub btnAgergarParamMistery_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgergarParamMistery.Click
        Dim v As New IQ_TarifasMistery()
        v.TS_Id = CInt(lstTipoServicio.SelectedValue)
        v.TE_Id = CInt(lstTipoEvidencia.SelectedValue)
        v.EV_Id = CInt(lstEvidencia.SelectedValue)
        v.CC_Id = CInt(lstNumContactos.SelectedValue)
        If _CaraCara.ExisteCombinacion(v) Then
            Dim mensaje As String = ValidarCaraCara(3)
            If mensaje = "" Then
                GuardarCaraCara(3, Nothing)

            Else

                ShowNotification(mensaje, WebMatrix.ShowNotifications.ErrorNotification)
            End If


        Else
            ShowNotification("La combinacion de opciones para obtener un valor no existe, por favor verifique con tecnologia.", WebMatrix.ShowNotifications.ErrorNotification)

        End If

    End Sub

    Private Sub cargarTarifasMistery(ByVal P As IQ_Parametros)
        gvTarifasMistery.DataSource = _CaraCara.ObtenerValorMistery(P)
        gvTarifasMistery.DataBind()
        UpMistery.Update()
    End Sub
#End Region

    Protected Sub gvTarifasMistery_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTarifasMistery.RowCommand
        Select Case e.CommandName

            Case "Borrar"
                _CaraCara.BorrarValorMistery(CDec(CType(gvTarifasMistery.Rows(e.CommandArgument).FindControl("Label1"), Label).Text))
                Dim p As IQ_Parametros
                p = CType(Session("PARAMETROS"), IQ_Parametros)
                cargarTarifasMistery(p)
        End Select

    End Sub
    Private Sub chkAñoActual_CheckedChanged(sender As Object, e As EventArgs) Handles chkAñoActual.CheckedChanged
        If chkAñoActual.Checked = True And chkAñoSiguiente.Checked = True Then
            chkAñoActual.Checked = False
            chkAñoSiguiente.Checked = True
        Else
        End If
    End Sub
    Private Sub chkAñoSiguiente_CheckedChanged(sender As Object, e As EventArgs) Handles chkAñoSiguiente.CheckedChanged
        If chkAñoActual.Checked = True And chkAñoSiguiente.Checked = True Then
            chkAñoActual.Checked = False
            chkAñoSiguiente.Checked = True
        Else
        End If
    End Sub
    Protected Sub chkTablet_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkTablet.CheckedChanged
        If chkTablet.Checked = True And ChkPapel.Checked = True Then
            chkTablet.Checked = False
            ChkPapel.Checked = True
        Else
        End If

        '    If chkTablet.Checked = True Then
        '        CCChkDispPropio.Visible = True
        '    Else
        '        CCChkDispPropio.Visible = False
        '    End If

    End Sub

    Protected Sub btnSimular_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSimular.Click
        If txtValorVentaSimular.Text.Trim <> String.Empty Then
            Dim p As IQ_Parametros
            p = CType(Session("PARAMETROS"), IQ_Parametros)
            lblGMsimulado.Text = (_GM.SimularGM(p, CDec(txtValorVentaSimular.Text), 1) * 100).ToString("N2")
            CargarJBI(p, CDec(lblGMsimulado.Text) / 100, -2, True)
            CargarJBE(p, CDec(lblGMsimulado.Text) / 100, -2, True)
        Else
            ShowNotification("Digite el valor de la venta para simular el Gross Margin", WebMatrix.ShowNotifications.ErrorNotification)
        End If
    End Sub

    Protected Sub btnModificarGM_1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificarGM_1.Click
        'Dim gm As Decimal
        'Dim p As IQ_Parametros
        'p = CType(Session("PARAMETROS"), IQ_Parametros)
        'gm = _GM.SimularGM(p, CDec(txtValorVentaSimular.Text), 0)
        AjustarGrossMargin(1)
    End Sub

    Protected Sub btnSimValorVenta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSimValorVenta.Click

        Try
            If txtNuevoGM.Text.Trim <> String.Empty Or txtGMOpera.Text.Trim <> String.Empty Then
                Dim p As IQ_Parametros
                Dim gmOpe, gmUni As Decimal
                p = CType(Session("PARAMETROS"), IQ_Parametros)
                If txtGMOpera.Text = String.Empty Then
                    gmOpe = -1
                Else
                    gmOpe = CDec(txtGMOpera.Text) / 100
                End If

                If txtNuevoGM.Text = String.Empty Then
                    gmUni = -1
                Else
                    gmUni = CDec(txtNuevoGM.Text) / 100
                End If
                lblValorVentaSimulado.Text = (_GM.SimularVenta(p, If((txtNuevoGM.Text.Trim = String.Empty), -1, (CDec(txtNuevoGM.Text) / 100)), gmOpe, 1)).ToString("C")
                CargarJBI(p, gmUni, gmOpe, True)
                CargarJBE(p, gmUni, gmOpe, True)
            Else
                ShowNotification("Digite el valor del nuevo Gross Margin o el Gross Margin de Operaciones ", WebMatrix.ShowNotifications.ErrorNotification)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnModificarGM_2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificarGM_2.Click
        'Dim p As IQ_Parametros
        'Dim gmOpe, gm As Decimal
        'p = CType(Session("PARAMETROS"), IQ_Parametros)
        'If txtGMOpera.Text = String.Empty Then
        '    gmOpe = -1
        'Else
        '    gmOpe = CDec(txtGMOpera.Text)
        'End If
        'gm = _GM.SimularVenta(p, CDec(txtNuevoGM.Text), gmOpe, 0)
        AjustarGrossMargin(2)
    End Sub


    Private Sub CargarJBI(p As IQ_Parametros, gmu As Decimal, gmo As Decimal, simular As Boolean)
        GVJBI.DataSource = _GM.ObtenerCostosJBI(p, gmu, gmo, simular)
        GVJBI.DataBind()


    End Sub

    Private Sub CargarJBE(p As IQ_Parametros, gmu As Decimal, gmo As Decimal, simular As Boolean)

        GVJBE.DataSource = _GM.ObtenerCostosJBE(p, gmu, gmo, simular)
        GVJBE.DataBind()
    End Sub
    Protected Sub GVJBE_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVJBE.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            If e.Row.Cells(0).Text.ToString.IndexOf("PORCENTAJE") > -1 Then
                e.Row.Cells(1).Text = CDec(e.Row.Cells(1).Text).ToString("P2")
            Else
                e.Row.Cells(1).Text = CDec(e.Row.Cells(1).Text).ToString("C0")
            End If

            If e.Row.Cells(0).Text.ToString.IndexOf("TOTAL") > -1 Or e.Row.Cells(0).Text.ToString.IndexOf("GROSS") > -1 Or e.Row.Cells(0).Text.ToString.IndexOf("VENTA") > -1 Then
                e.Row.Font.Bold = True
            End If

            e.Row.Cells(1).CssClass = "RightAlign"

        End If

    End Sub

    Protected Sub GVJBI_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GVJBI.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(0).Text.ToString.IndexOf("PORCENTAJE") > -1 Then
                e.Row.Cells(1).Text = CDec(e.Row.Cells(1).Text).ToString("P2")
            Else
                e.Row.Cells(1).Text = CDec(e.Row.Cells(1).Text).ToString("C0")
            End If

            If e.Row.Cells(0).Text.ToString.IndexOf("TOTAL") > -1 Or e.Row.Cells(0).Text.ToString.IndexOf("GROSS") > -1 Or e.Row.Cells(0).Text.ToString.IndexOf("VENTA") > -1 Then
                e.Row.Font.Bold = True
            End If

            e.Row.Cells(1).CssClass = "RightAlign"
        End If

    End Sub

End Class