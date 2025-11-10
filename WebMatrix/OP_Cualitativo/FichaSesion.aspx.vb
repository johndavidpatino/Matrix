Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class FichaSesion
    Inherits System.Web.UI.Page

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("idtrabajo") IsNot Nothing Then
                Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
                hfidtrabajo.Value = idtrabajo
                CargarHabeasData(hfidtrabajo.Value)

                If Request.QueryString("op") IsNot Nothing Then
                    btnGuardar.Visible = False
                    btnCancelar.Visible = False
                    btnEntrega.Visible = False
                    btnVolverOP.Visible = True

                Else
                    Dim oUsuario As New US.RolesUsuarios
                    Dim Usuario = oUsuario.obtenerRolesXUsuario(Session("IDUsuario").ToString, True)
                    Dim Count As Int16 = 0
                    For Each oRolId In Usuario
                        Dim RolId As Int32? = oRolId.RolId
                        If RolId = "6" Or RolId = "7" Or RolId = "8" Then
                            Count = Count + 1
                        End If

                        If Count > 0 Then
                            btnGuardar.Visible = True
                            btnCancelar.Visible = True
                            btnEntrega.Visible = True
                            btnVolverOP.Visible = False
                        Else
                            btnGuardar.Visible = False
                            btnCancelar.Visible = False
                            btnEntrega.Visible = False
                            btnVolverOP.Visible = True
                        End If
                    Next
                End If
            Else
                Response.Redirect("~/Home.aspx")
            End If

            CargarSesiones()
            CargarInfo()
            CargarAyudasCuali()
            CargarTiposReclutamiento()
            ObtenerAyudas()
            ObtenerTipoReclutamiento()
        End If
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            If rblIncentivos.SelectedValue = "1" And txtPresupuestoIncentivo.Text = "" Then
                ShowNotification("Digite el Presupuesto del Incentivo Económico", ShowNotifications.ErrorNotification)
                txtPresupuestoIncentivo.Focus()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If

            If rblIncentivos.SelectedValue = "1" And txtDistribucionIncentivo.Text = "" Then
                ShowNotification("Digite la Distribución del Incentivo Económico", ShowNotifications.ErrorNotification)
                txtDistribucionIncentivo.Focus()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If

            If rblCompraIpsos.SelectedValue = "1" And txtPresupuesto.Text = "" Then
                ShowNotification("Digite el Presupuesto del Incentivo Compra Ipsos", ShowNotifications.ErrorNotification)
                txtPresupuesto.Focus()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If

            If rblCompraIpsos.SelectedValue = "1" And txtDistribucionCompra.Text = "" Then
                ShowNotification("Digite la Distribución del Incentivo Compra Ipsos", ShowNotifications.ErrorNotification)
                txtPresupuesto.Focus()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If

            Dim selectedCount As Integer = chbReclutamiento.Items.Cast(Of ListItem)().Count(Function(li) li.Selected)
            If selectedCount = 0 Then
                ShowNotification("Seleccione el Tipo de Reclutamiento a utilizar", ShowNotifications.ErrorNotification)
                chbReclutamiento.Focus()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If

            If txtExclusionesyRestricciones.Text = "" Then
                ShowNotification("Escriba las Exclusiones y Restricciones Específicas", ShowNotifications.ErrorNotification)
                txtExclusionesyRestricciones.Focus()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If

            If txtRecursosPropiedadesCliente.Text = "" Then
                ShowNotification("Escriba los Recursos Propiedad del Cliente", ShowNotifications.ErrorNotification)
                txtRecursosPropiedadesCliente.Focus()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If

            If txtBackups.Text = "" Then
                ShowNotification("Escriba los Backups Necesarios para la Sesión", ShowNotifications.ErrorNotification)
                txtBackups.Focus()
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If

            GuardarFichaSesion()
            ActualizarHabeasData(hfidtrabajo.Value)
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            'Limpiar()
            CargarSesiones()
            CargarInfo()
            CargarHabeasData(hfidtrabajo.Value)
            CargarAyudasCuali()
            CargarTiposReclutamiento()
            ObtenerAyudas()
            ObtenerTipoReclutamiento()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub rblIncentivos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblIncentivos.SelectedIndexChanged
        If rblIncentivos.SelectedValue = "1" Then
            txtPresupuestoIncentivo.Enabled = True
            txtDistribucionIncentivo.Enabled = True
        Else
            txtPresupuestoIncentivo.Enabled = False
            txtPresupuestoIncentivo.Text = ""
            txtDistribucionIncentivo.Enabled = False
            txtDistribucionIncentivo.Text = ""
        End If
    End Sub
    Protected Sub rblCompraIpsos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblCompraIpsos.SelectedIndexChanged
        If rblCompraIpsos.SelectedValue = "1" Then
            txtPresupuesto.Enabled = True
            txtDistribucionCompra.Enabled = True
        Else
            txtPresupuesto.Enabled = False
            txtPresupuesto.Text = ""
            txtDistribucionCompra.Enabled = False
            txtDistribucionCompra.Text = ""
        End If
    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub GuardarAyudas()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        For Each li As ListItem In chbAyudas.Items
            oSegmentos.GuardarAyudas(hfidtrabajo.Value, li.Value, li.Selected)
        Next
    End Sub

    Sub ObtenerAyudas()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        For i As Integer = 0 To oSegmentos.ObtenerAyudasRequeridasCualiList(hfidtrabajo.Value).ToList.Count - 1
            Dim val As Integer = oSegmentos.ObtenerAyudasRequeridasCualiList(hfidtrabajo.Value).Item(i).TipoAyuda
            For Each li As ListItem In chbAyudas.Items
                If li.Value = val Then
                    li.Selected = True
                    Exit For
                End If
            Next
        Next

    End Sub

    Sub GuardarTipoReclutamiento()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        For Each li As ListItem In chbReclutamiento.Items
            oSegmentos.GuardarTipoReclutamiento(hfidtrabajo.Value, li.Value, li.Selected)
        Next
    End Sub

    Sub ObtenerTipoReclutamiento()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        For i As Integer = 0 To oSegmentos.ObtenerReclutamientoRequeridoCualiList(hfidtrabajo.Value).ToList.Count - 1
            Dim val As Integer = oSegmentos.ObtenerReclutamientoRequeridoCualiList(hfidtrabajo.Value).Item(i).TipoReclutamiento
            For Each li As ListItem In chbReclutamiento.Items
                If li.Value = val Then
                    li.Selected = True
                    Exit For
                End If
            Next
        Next

    End Sub
    Sub CargarSesiones()
        Dim o As New CoreProject.CampoCualitativo
        gvSesiones.DataSource = o.ObtenerSesionesxTrabajo(hfidtrabajo.Value)
        gvSesiones.DataBind()
    End Sub

    Sub CargarAyudasCuali()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        Me.chbAyudas.DataSource = oSegmentos.ObtenerAyudasCuali
        Me.chbAyudas.DataTextField = "Ayuda"
        Me.chbAyudas.DataValueField = "id"
        Me.chbAyudas.DataBind()
    End Sub

    Sub CargarTiposReclutamiento()
        Dim oReclutamiento As New CoreProject.SegmentosCuali
        Me.chbReclutamiento.DataSource = oReclutamiento.ObtenerTipoReclutamiento
        Me.chbReclutamiento.DataTextField = "Tipo"
        Me.chbReclutamiento.DataValueField = "id"
        Me.chbReclutamiento.DataBind()
    End Sub

    Public Sub CargarInfo()

        Dim idtrabajo As Int64 = Int64.Parse(hfidtrabajo.Value)
        Dim oTrabajo As New Trabajo
        Dim info = oTrabajo.ObtenerInfoTrabajoCuali(idtrabajo)

        txtJobBook.Text = info.JobBook
        txtNombreProyecto.Text = info.NombreTrabajo

        If info.IncentivoEconomico Then
            rblIncentivos.SelectedValue = "1"
            txtPresupuestoIncentivo.Enabled = True
            txtDistribucionIncentivo.Enabled = True
        Else
            rblIncentivos.SelectedValue = "0"
            txtPresupuestoIncentivo.Enabled = False
            txtDistribucionIncentivo.Enabled = False
        End If

        txtPresupuestoIncentivo.Text = info.PresupuestoIncentivo
        txtDistribucionIncentivo.Text = info.DistribucionIncentivo

        If info.RegalosCliente Then
            rblRegaloClientes.SelectedValue = "1"
        Else
            rblRegaloClientes.SelectedValue = "0"
        End If

        If info.CompraIpsos Then
            rblCompraIpsos.SelectedValue = "1"
            txtPresupuesto.Enabled = True
            txtDistribucionCompra.Enabled = True
        Else
            rblCompraIpsos.SelectedValue = "0"
            txtPresupuesto.Enabled = False
            txtDistribucionCompra.Enabled = False
        End If

        txtPresupuesto.Text = info.PresupuestoCompra
        txtDistribucionCompra.Text = info.DistribucionCompra

        txtExclusionesyRestricciones.Text = info.ExclusionesRestricciones
        txtRecursosPropiedadesCliente.Text = info.RecursosPropiedadCliente
        txtBackups.Text = info.Backups
    End Sub

    Public Sub CargarHabeasData(ByVal idTrabajo As Int64)
        Dim oTrabajo As New Trabajo
        Dim oPropuesta As New Propuesta
        Dim trabajo = oTrabajo.obtenerXId(idTrabajo)
        Dim idPropuesta = trabajo.IdPropuesta
        Dim info = oPropuesta.DevolverxID(idPropuesta)

        If info.RequestHabeasData IsNot Nothing Then
            txtHabeasData.Text = info.RequestHabeasData
        End If

    End Sub

    Public Sub GuardarFichaSesion()
        Try
            Dim oTrabajo As New Trabajo
            Dim ent As New PY_TrabajoCuali
            ent.TrabajoId = hfidtrabajo.Value
            ent.IncentivoEconomico = rblIncentivos.SelectedValue
            If txtPresupuestoIncentivo.Text = "" Then
                ent.PresupuestoIncentivo = "0"
            Else
                ent.PresupuestoIncentivo = txtPresupuestoIncentivo.Text
            End If
            ent.DistribucionIncentivo = txtDistribucionIncentivo.Text
            ent.RegalosCliente = rblRegaloClientes.SelectedValue
            ent.CompraIpsos = rblCompraIpsos.SelectedValue
            If txtPresupuesto.Text = "" Then
                ent.PresupuestoCompra = "0"
            Else
                ent.PresupuestoCompra = txtPresupuesto.Text
            End If
            ent.DistribucionCompra = txtDistribucionCompra.Text
            ent.ExclusionesRestricciones = txtExclusionesyRestricciones.Text
            ent.RecursosPropiedadCliente = txtRecursosPropiedadesCliente.Text
            ent.Backups = txtBackups.Text
            oTrabajo.GuardarTrabajoCuali(ent)

            GuardarAyudas()
            GuardarTipoReclutamiento()
            EnviarEmail(False)
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
        Catch ex As Exception
            ShowNotification("Ha ocurrido un error al intentar ingresar el registro - " & ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Sub ActualizarHabeasData(ByVal idTrabajo As Int64)
        Dim oTrabajo As New Trabajo
        Dim oPropuesta As New Propuesta
        Dim trabajo = oTrabajo.obtenerXId(idTrabajo)
        Dim idPropuesta = trabajo.IdPropuesta

        If Not txtHabeasData.Text = "" Then
            oPropuesta.ActualizarPropuesta_HabeasData(idPropuesta, txtHabeasData.Text)
        End If
    End Sub

    Sub EnviarEmail(ByVal Nuevo As Boolean)
        Dim oEnviarCorreo As New EnviarCorreo
        If String.IsNullOrEmpty(hfidtrabajo.Value) Then
            Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar una entrega")
        End If
        oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/EntregaTrabajoSesiones.aspx?idFicha=" & Request.QueryString("idtrabajo").ToString & "&nuevo=" & Nuevo)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnEntrega_Click(sender As Object, e As EventArgs) Handles btnEntrega.Click
        Try
            If String.IsNullOrEmpty(hfidfichasesion.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar una entrega")
            End If
            Dim script As String = "window.open('../Emails/EntregaTrabajoSesiones.aspx?idFicha=" & Request.QueryString("idtrabajo").ToString & "','cal','width=400,height=250,left=270,top=180')"
            Dim page As Page = DirectCast(Context.Handler, Page)
            ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim idtrabajo As Int64 = Int64.Parse(hfidtrabajo.Value)
        Dim oTrabajo As New Trabajo
        Dim info = oTrabajo.DevolverxID(idtrabajo)
        Response.Redirect("../PY_Proyectos/TrabajosCualitativos.aspx?ProyectoId=" & info.ProyectoId)
    End Sub
    Protected Sub btnVolverOP_Click(sender As Object, e As EventArgs) Handles btnVolverOP.Click
        Response.Redirect("../OP_Cualitativo/Trabajos.aspx")
    End Sub
#End Region

End Class