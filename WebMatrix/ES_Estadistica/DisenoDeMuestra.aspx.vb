Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios

Public Class DisenoDeMuestra
    Inherits System.Web.UI.Page
#Region " Eventos del Control"
    Protected Sub gvDatos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDatos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblobjetivos As Label = e.Row.FindControl("lblobjetivos")
            If (lblobjetivos.Text.Length > 25) Then
                lblobjetivos.Text = lblobjetivos.Text.Substring(0, 25) + "..."
            End If

            Dim lblpoblacion As Label = e.Row.FindControl("lblpoblacion")
            If (lblpoblacion.Text.Length > 25) Then
                lblpoblacion.Text = lblpoblacion.Text.Substring(0, 25) + "..."
            End If

            Dim lblmercado As Label = e.Row.FindControl("lblmercado")
            If (lblmercado.Text.Length > 25) Then
                lblmercado.Text = lblmercado.Text.Substring(0, 25) + "..."
            End If

            Dim lblmarco As Label = e.Row.FindControl("lblmarco")
            If (lblmarco.Text.Length > 25) Then
                lblmarco.Text = lblmarco.Text.Substring(0, 25) + "..."
            End If

            Dim lbltecnica As Label = e.Row.FindControl("lbltecnica")
            If (lbltecnica.Text.Length > 25) Then
                lbltecnica.Text = lbltecnica.Text.Substring(0, 25) + "..."
            End If

            Dim lbldiseno As Label = e.Row.FindControl("lbldiseno")
            If (lbldiseno.Text.Length > 25) Then
                lbldiseno.Text = lbldiseno.Text.Substring(0, 25) + "..."
            End If

            Dim lbltamano As Label = e.Row.FindControl("lbltamano")
            If (lbltamano.Text.Length > 25) Then
                lbltamano.Text = lbltamano.Text.Substring(0, 25) + "..."
            End If

            Dim lblfiabilidad As Label = e.Row.FindControl("lblfiabilidad")
            If (lblfiabilidad.Text.Length > 25) Then
                lblfiabilidad.Text = lblfiabilidad.Text.Substring(0, 25) + "..."
            End If

            Dim lbldesagregacion As Label = e.Row.FindControl("lbldesagregacion")
            If (lbldesagregacion.Text.Length > 25) Then
                lbldesagregacion.Text = lbldesagregacion.Text.Substring(0, 25) + "..."
            End If

            Dim lblfuente As Label = e.Row.FindControl("lblfuente")
            If (lblfuente.Text.Length > 25) Then
                lblfuente.Text = lblfuente.Text.Substring(0, 25) + "..."
            End If

            Dim lblponderacion As Label = e.Row.FindControl("lblponderacion")
            If (lblponderacion.Text.Length > 25) Then
                lblponderacion.Text = lblponderacion.Text.Substring(0, 25) + "..."
            End If
        End If
    End Sub
    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarDiseno()
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idDiseno As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    CargarInfo(idDiseno)
                    log(idDiseno, 3)
                Case "Eliminar"
                    Dim idDiseno As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Eliminar(idDiseno)
                    CargarDiseno()
                    ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
                    log(hfiddiseno.Value, 4)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(36, UsuarioID) = False Then
                Me.btnNuevo.Visible = False
            End If
            If Request.QueryString("idbrief") IsNot Nothing Then
                Dim idbrief As Int64 = Int64.Parse(Request.QueryString("idbrief").ToString)
                hfidbrief.Value = idbrief
                CargarDiseno()
            Else
                Response.Redirect("BriefDisenoDeMuestra.aspx")
            End If
        End If
    End Sub

    Protected Sub chklista_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chklista.SelectedIndexChanged
        For Each chkitem As ListItem In chklista.Items
            Select Case chkitem.Value
                Case 1
                    If chkitem.Selected Then
                        pnlobjetivos.Visible = True
                    Else
                        pnlobjetivos.Visible = False
                    End If
                Case 2
                    If chkitem.Selected Then
                        pnlpoblacion.Visible = True
                    Else
                        pnlpoblacion.Visible = False
                    End If
                Case 3
                    If chkitem.Selected Then
                        pnlmercado.Visible = True
                    Else
                        pnlmercado.Visible = False
                    End If
                Case 4
                    If chkitem.Selected Then
                        pnlmarco.Visible = True
                    Else
                        pnlmarco.Visible = False
                    End If
                Case 5
                    If chkitem.Selected Then
                        pnltecnica.Visible = True
                    Else
                        pnltecnica.Visible = False
                    End If
                Case 6
                    If chkitem.Selected Then
                        pnldiseno.Visible = True
                    Else
                        pnldiseno.Visible = False
                    End If
                Case 7
                    If chkitem.Selected Then
                        pnltamano.Visible = True
                    Else
                        pnltamano.Visible = False
                    End If
                Case 8
                    If chkitem.Selected Then
                        pnlfiabilidad.Visible = True
                    Else
                        pnlfiabilidad.Visible = False
                    End If
                Case 9
                    If chkitem.Selected Then
                        pnldesagregacion.Visible = True
                    Else
                        pnldesagregacion.Visible = False
                    End If
                Case 10
                    If chkitem.Selected Then
                        pnlfuente.Visible = True
                    Else
                        pnlfuente.Visible = False
                    End If
                Case 11
                    If chkitem.Selected Then
                        pnlPonderacion.Visible = True
                    Else
                        pnlPonderacion.Visible = False
                    End If
            End Select
        Next
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            CargarDiseno()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        Limpiar()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        txtFecha.Text = Date.UtcNow.AddHours(-5).Date
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            Guardar()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            log(hfiddiseno.Value, 2)
            Limpiar()
            CargarDiseno()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)

        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Protected Sub btnDocumentos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDocumentos.Click
        If hfiddiseno.Value <> "" Then
            Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & hfiddiseno.Value & "&IdDocumento=5")
        End If
    End Sub
#End Region
#Region "Funciones y Metodos"
    Public Sub CargarDiseno()
        Try
            Dim idbrief As Int64 = Int64.Parse(hfidbrief.Value)
            Dim oBrief As New BriefDisenoMuestral
            Dim info = oBrief.DevolverxID(idbrief)
            lbltitulo.Text = "ID Brief: " + info.id.ToString + " Propuesta: " + info.Titulo
            lbltitulo2.Text = "ID Brief: " + info.id.ToString + " Propuesta: " + info.Titulo
            lblGerente.Text = "Gerente: " + info.Gerente.ToString()
            lblGerente2.Text = "Gerente: " + info.Gerente.ToString()
            Dim oDiseno As New DisenoMuestral
            Dim listaDiseno = (From ldiseno In oDiseno.DevolverxIDBrief(idbrief)
                               Select Id = ldiseno.id,
                               Fecha = ldiseno.Fecha,
                               Objetivo = ldiseno.ObjetivoT,
                               Poblacion = ldiseno.PoblacionT,
                               Mercado = ldiseno.MercadoT,
                               Marco = ldiseno.MarcoT,
                               Tecnica = ldiseno.TecnicaT,
                               Diseno = ldiseno.DisenoT,
                               Tamano = ldiseno.TamanoT,
                               Fiabilidad = ldiseno.FiabilidadT,
                               Desagregacion = ldiseno.DesagregacionT,
                               Fuente = ldiseno.FuenteT,
                               Ponderacion = ldiseno.PonderacionT,
                               Observaciones = ldiseno.ObservacionesT).OrderBy(Function(d) d.Id)
            If (Not String.IsNullOrEmpty(txtBuscar.Text)) Then
                gvDatos.DataSource = listaDiseno.Where(Function(c) (c.Objetivo.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Poblacion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Mercado.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Marco.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Tecnica.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Diseno.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Tamano.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Fiabilidad.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Desagregacion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Fuente.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Ponderacion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Observaciones.ToUpper.Contains(txtBuscar.Text.ToUpper))).ToList
            Else
                gvDatos.DataSource = listaDiseno.ToList
            End If
            gvDatos.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub CargarDisenoSinDiseno()
        Try
            Dim idbrief As Int64 = Int64.Parse(hfidbrief.Value)
            Dim oBrief As New BriefDisenoMuestral
            Dim info = oBrief.DevolverxID(idbrief)
            lbltitulo.Text = "ID Brief: " + info.id.ToString + " Propuesta: " + info.Titulo
            lbltitulo2.Text = "ID Brief: " + info.id.ToString + " Propuesta: " + info.Titulo
            Dim oDiseno As New DisenoMuestral
            Dim listaDiseno = (From ldiseno In oDiseno.DevolverxIDBrief(idbrief)
                               Select Id = ldiseno.id,
                               Fecha = ldiseno.Fecha,
                               Objetivo = ldiseno.ObjetivoT,
                               Poblacion = ldiseno.PoblacionT,
                               Mercado = ldiseno.MercadoT,
                               Marco = ldiseno.MarcoT,
                               Tecnica = ldiseno.TecnicaT,
                               Diseno = ldiseno.DisenoT,
                               Tamano = ldiseno.TamanoT,
                               Fiabilidad = ldiseno.FiabilidadT,
                               Desagregacion = ldiseno.DesagregacionT,
                               Fuente = ldiseno.FuenteT,
                               Ponderacion = ldiseno.PonderacionT).OrderBy(Function(d) d.Id)
            If (Not String.IsNullOrEmpty(txtBuscar.Text)) Then
                gvDatos.DataSource = listaDiseno.Where(Function(c) (c.Objetivo.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Poblacion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Mercado.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Marco.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Tecnica.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Diseno.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Tamano.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Fiabilidad.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Desagregacion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Fuente.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Ponderacion.ToUpper.Contains(txtBuscar.Text.ToUpper))).ToList
            Else
                gvDatos.DataSource = listaDiseno.ToList
            End If
            gvDatos.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Limpiar()
        hfiddiseno.Value = String.Empty
        txtBuscar.Text = String.Empty
        txtDesagregacion.Text = String.Empty
        txtDiseno.Text = String.Empty
        txtFecha.Text = String.Empty
        txtFiabilidad.Text = String.Empty
        txtFuente.Text = String.Empty
        txtMarcoMuestral.Content = String.Empty
        txtMercado.Text = String.Empty
        txtObjetivos.Text = String.Empty
        txtPoblacion.Text = String.Empty
        txtPonderacion.Content = String.Empty
        txtTamano.Content = String.Empty
        txtTecnica.Text = String.Empty
        For Each chkitem As ListItem In chklista.Items
            chkitem.Selected = False
        Next
        pnlobjetivos.Visible = False
        pnlpoblacion.Visible = False
        pnlmercado.Visible = False
        pnlmarco.Visible = False
        pnltecnica.Visible = False
        pnldiseno.Visible = False
        pnltamano.Visible = False
        pnlfiabilidad.Visible = False
        pnldesagregacion.Visible = False
        pnlfuente.Visible = False
        pnlPonderacion.Visible = False
    End Sub
    Public Sub Guardar()
        Try
            Dim MuestreoProb As Boolean
            Dim Objetivos As Boolean
            Dim Poblacion As Boolean
            Dim Mercado As Boolean
            Dim Marco As Boolean
            Dim Tecnica As Boolean
            Dim Diseno As Boolean
            Dim Tamano As Boolean
            Dim Fiabilidad As Boolean
            Dim Desagregacion As Boolean
            Dim Fuente As Boolean
            Dim Ponderacion As Boolean
            Dim ObjetivosT As String = ""
            Dim PoblacionT As String = ""
            Dim MercadoT As String = ""
            Dim MarcoT As String = ""
            Dim TecnicaT As String = ""
            Dim DisenoT As String = ""
            Dim TamanoT As String = ""
            Dim FiabilidadT As String = ""
            Dim DesagregacionT As String = ""
            Dim FuenteT As String = ""
            Dim PonderacionT As String = ""
            Dim Variable As String = ""
            Dim VariableT As String = ""
            Dim Observaciones As String = ""
            Dim ObservacionesT As String = ""
            Dim idbrief As Int64 = Int64.Parse(hfidbrief.Value)
            Dim idDiseno As Int64

            If Not String.IsNullOrEmpty(hfiddiseno.Value) Then
                idDiseno = Int64.Parse(hfiddiseno.Value)
            End If

            For Each chkitem As ListItem In chklista.Items
                Select Case chkitem.Value
                    Case 0
                        If chkitem.Selected Then
                            MuestreoProb = True
                        End If
                    Case 1
                        If chkitem.Selected Then
                            Objetivos = True
                            ObjetivosT = txtObjetivos.Text.ToString()
                        End If
                    Case 2
                        If chkitem.Selected Then
                            Poblacion = True
                            PoblacionT = txtPoblacion.Text.ToString()
                        End If
                    Case 3
                        If chkitem.Selected Then
                            Mercado = True
                            MercadoT = txtMercado.Text.ToString()
                        End If
                    Case 4
                        If chkitem.Selected Then
                            Marco = True
                            MarcoT = txtMarcoMuestral.Content.ToString()
                        End If
                    Case 5
                        If chkitem.Selected Then
                            Tecnica = True
                            TecnicaT = txtTecnica.Text.ToString()
                        End If
                    Case 6
                        If chkitem.Selected Then
                            Diseno = True
                            DisenoT = txtDiseno.Text.ToString()
                        End If
                    Case 7
                        If chkitem.Selected Then
                            Tamano = True
                            TamanoT = txtTamano.Content.ToString
                        End If
                    Case 8
                        If chkitem.Selected Then
                            Fiabilidad = True
                            FiabilidadT = txtFiabilidad.Text.ToString
                        End If
                    Case 9
                        If chkitem.Selected Then
                            Desagregacion = True
                            DesagregacionT = txtDesagregacion.Text.ToString()
                        End If
                    Case 10
                        If chkitem.Selected Then
                            Fuente = True
                            FuenteT = txtFuente.Text.ToString()
                        End If
                    Case 11
                        If chkitem.Selected Then
                            Ponderacion = True
                            PonderacionT = txtPonderacion.Content.ToString()
                        End If
                    Case 12
                        If chkitem.Selected Then
                            Variable = True
                            VariableT = txtVariable.Content.ToString()
                        End If
                    Case 13
                        If chkitem.Selected Then
                            Observaciones = True
                            ObservacionesT = txtObservaciones.Text.ToString()
                        End If
                End Select
            Next

            Dim NoVersion = 1
            Dim oDiseno As New DisenoMuestral
            Dim Fecha As DateTime

            If Not String.IsNullOrEmpty(txtFecha.Text) Then
                Fecha = txtFecha.Text
            Else
                Fecha = Date.UtcNow.AddHours(-5)
            End If
            Dim flag As Boolean = False
            If idDiseno = 0 Then
                flag = True
            End If
            hfiddiseno.Value = oDiseno.Guardar(idDiseno, idbrief, Fecha, MuestreoProb, Objetivos, Poblacion, Mercado, Marco, Tecnica, Diseno, Tamano, Fiabilidad, Desagregacion, Fuente, Ponderacion, ObjetivosT, PoblacionT, MercadoT, MarcoT, TecnicaT, DisenoT, TamanoT, FiabilidadT, DesagregacionT, FuenteT, PonderacionT, Variable, VariableT, Observaciones, ObservacionesT, NoVersion)

            If flag = True Then
                Dim oEnviarCorreo As New EnviarCorreo
                Try
                    If String.IsNullOrEmpty(hfidbrief.Value) Then
                        Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
                    End If
                    oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/DisenoDeMuestra.aspx?idBrief=" & hfidbrief.Value)
                    ActivateAccordion(0, EffectActivateAccordion.NoEffect)
                Catch ex As Exception
                    ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
                    ActivateAccordion(0, EffectActivateAccordion.NoEffect)
                End Try
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarInfo(ByVal idDiseno As Int64)
        Try
            Dim oDiseno As New DisenoMuestral
            Dim info = oDiseno.DevolverxID(idDiseno)
            hfiddiseno.Value = info.id
            Dim fecha = Format(Convert.ToDateTime(info.Fecha), "dd/MM/yyyy")
            txtFecha.Text = fecha

            For Each chkitem As ListItem In chklista.Items
                Select Case chkitem.Value
                    Case 1
                        If info.Objetivo Then
                            chkitem.Selected = True
                            txtObjetivos.Text = info.ObjetivoT
                            pnlobjetivos.Visible = True
                        End If
                    Case 2
                        If info.Poblacion Then
                            chkitem.Selected = True
                            txtPoblacion.Text = info.PoblacionT
                            pnlpoblacion.Visible = True
                        End If
                    Case 3
                        If info.Mercado Then
                            chkitem.Selected = True
                            txtMercado.Text = info.MercadoT
                            pnlmercado.Visible = True
                        End If
                    Case 4
                        If info.Marco Then
                            chkitem.Selected = True
                            txtMarcoMuestral.Content = info.MarcoT
                            pnlmarco.Visible = True
                        End If
                    Case 5
                        If info.Tecnica Then
                            chkitem.Selected = True
                            txtTecnica.Text = info.TecnicaT
                            pnltecnica.Visible = True
                        End If
                    Case 6
                        If info.Diseno Then
                            chkitem.Selected = True
                            txtDiseno.Text = info.DisenoT
                            pnldiseno.Visible = True
                        End If
                    Case 7
                        If info.Tamano Then
                            chkitem.Selected = True
                            txtTamano.Content = info.TamanoT
                            pnltamano.Visible = True
                        End If
                    Case 8
                        If info.Fiabilidad Then
                            chkitem.Selected = True
                            txtFiabilidad.Text = info.FiabilidadT
                            pnlfiabilidad.Visible = True
                        End If
                    Case 9
                        If info.Desagregacion Then
                            chkitem.Selected = True
                            txtDesagregacion.Text = info.DesagregacionT
                            pnldesagregacion.Visible = True
                        End If
                    Case 10
                        If info.Fuente Then
                            chkitem.Selected = True
                            txtFuente.Text = info.FuenteT
                            pnlfuente.Visible = True
                        End If
                    Case 11
                        If info.Ponderacion Then
                            chkitem.Selected = True
                            txtPonderacion.Content = info.PonderacionT
                            pnlPonderacion.Visible = True
                        End If
                End Select
            Next
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Eliminar(ByVal idDiseno As Int64)
        Try
            Dim oDiseno As New DisenoMuestral
            oDiseno.Eliminar(idDiseno)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Dim log As New LogEjecucion
        log.Guardar(18, iddoc, Now(), Session("IDUsuario"), idaccion)

    End Sub
#End Region

    Protected Sub btnIraBrief_Click(sender As Object, e As EventArgs) Handles btnIraBrief.Click
        Dim obrief As New BriefDisenoMuestral
        Dim idpropuesta As Int64 = obrief.DevolverxID(hfidbrief.Value).Propuestaid
        Response.Redirect("BriefDisenoDeMuestra.aspx?IdPropuesta=" & idpropuesta)
    End Sub
End Class