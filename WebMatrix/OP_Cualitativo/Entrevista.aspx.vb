Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class Entrevista
    Inherits System.Web.UI.Page

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("idtrabajo") IsNot Nothing Then
                Dim idfichaentrevista As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
                hfFichaEntrevistaID.Value = idfichaentrevista
                CargarEntrevistas()
                CargarEntrevistadores()
                cargarPaises()
            Else
                Response.Redirect("FichaEntrevista.aspx")
            End If

            ddldepartamento.Items.Clear()
            ddldepartamento.DataSource = Nothing
            ddldepartamento.DataBind()
            ddlCiudad.Items.Clear()
            ddlCiudad.DataSource = Nothing
            ddlCiudad.DataBind()
        End If
    End Sub
    Protected Sub ddlpais_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlpais.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddldepartamento_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddldepartamento.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddlCiudad_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCiudad.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddlEntrevistador_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlEntrevistador.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddlpais_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlpais.SelectedIndexChanged
        If (ddlpais.SelectedValue <> "-1") Then
            Dim idPais As Int32 = Int32.Parse(ddlpais.SelectedValue)
            CargarDepartamentos(idPais)
        Else
            ddldepartamento.Items.Clear()
            ddldepartamento.DataSource = Nothing
            ddldepartamento.DataBind()
            ddlCiudad.Items.Clear()
            ddlCiudad.DataSource = Nothing
            ddlCiudad.DataBind()
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub ddldepartamento_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddldepartamento.SelectedIndexChanged
        If (ddldepartamento.SelectedValue <> "-1") Then
            Dim iddpto As Int32 = Int32.Parse(ddldepartamento.SelectedValue)
            cargarciudades(iddpto)
        Else
            ddlCiudad.Items.Clear()
            ddlCiudad.DataSource = Nothing
            ddlCiudad.DataBind()
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarEntrevistas()
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            GuardarEntrevista()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            Limpiar()
            CargarEntrevistas()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        Limpiar()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        txtPersonaAEntrevistar.Focus()
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            CargarEntrevistas()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idObservacion As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    CargarInfo(idObservacion)
                Case "Eliminar"
                    Dim idObservacion As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Eliminar(idObservacion)
                    CargarEntrevistas()
                    ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
#End Region

#Region "Funciones y Metodos"
    Public Sub Limpiar()
        cargarPaises()
        ddldepartamento.Items.Clear()
        ddldepartamento.DataSource = Nothing
        ddldepartamento.DataBind()
        ddlCiudad.Items.Clear()
        ddlCiudad.DataSource = Nothing
        ddlCiudad.DataBind()
        ddlEntrevistador.SelectedValue = "-1"
        txtPersonaAEntrevistar.Text = String.Empty
        txtDireccion.Text = String.Empty
        txtTelefono.Text = String.Empty
        txtGrupoObjetivo.Text = String.Empty
        txtCaracteristicasEspeciales.Text = String.Empty
        txtObservaciones.Text = String.Empty
        txtFecha.Text = String.Empty
        txtFechaReal.Text = String.Empty
        txtHora.Text = String.Empty
        txtHoraReal.Text = String.Empty
        chkCancelada.Checked = False
        hfEntrevistaID.Value = String.Empty
    End Sub
    Public Sub cargarPaises()
        Try
            Dim oAuxiliar As New Auxiliares
            Dim listaPaises = (From lpaises In oAuxiliar.DevolverPaises()
                               Select id = lpaises.PaiPais, pais = lpaises.PaiNombre).ToList().OrderBy(Function(p) p.pais)
            ddlpais.DataSource = listaPaises
            ddlpais.DataTextField = "pais"
            ddlpais.DataValueField = "id"
            ddlpais.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarDepartamentos(ByVal idPais As Int32)
        Try
            Dim oAuxiliar As New Auxiliares
            Dim listaDepartamentos = (From ldpto In oAuxiliar.DevolverDepartamentos(idPais)
                                      Select id = ldpto.DivDepto, dpto = ldpto.DivDeptoNombre).ToList().OrderBy(Function(d) d.dpto)
            ddldepartamento.DataSource = listaDepartamentos
            ddldepartamento.DataValueField = "id"
            ddldepartamento.DataTextField = "dpto"
            ddldepartamento.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub cargarciudades(ByVal iddpto As Int32)
        Try
            Dim oAuxiliar As New Auxiliares
            Dim listaCiudades = (From lciudad In oAuxiliar.DevolverCiudades(iddpto)
                                 Select id = lciudad.DivDeptoMunicipio, idciudad = lciudad.DivMunicipio, ciudad = lciudad.DivMuniNombre).ToList().OrderBy(Function(c) c.ciudad)
            ddlCiudad.DataSource = listaCiudades
            ddlCiudad.DataValueField = "idciudad"
            ddlCiudad.DataTextField = "ciudad"
            ddlCiudad.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarEntrevistadores()
        Try
            Dim oUsuarios As New US.Usuarios
            Dim listapersonas = (From lpersona In oUsuarios.UsuariosxRol(ListaRoles.Entrevistador)
                                 Select Id = lpersona.id,
                                 Nombre = lpersona.Apellidos & " " & lpersona.Nombres).OrderBy(Function(p) p.Nombre)
            ddlEntrevistador.DataSource = listapersonas.ToList()
            ddlEntrevistador.DataValueField = "Id"
            ddlEntrevistador.DataTextField = "Nombre"
            ddlEntrevistador.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarEntrevistas()
        Try
            Dim idfichaEntrevista As Int64 = Int64.Parse(hfFichaEntrevistaID.Value)
            Dim oEntrevista As New Entrevistas
            Dim lEntrevistas = (From lent In oEntrevista.DevolverXEntrevistasId(idfichaEntrevista)
                                  Select Id = lent.id,
                                  FichaEntrevistaId = lent.EntrevistasId,
                                  Ciudad = lent.Ciudad,
                                  PersonaAEntrevistar = lent.PersonaAEntrevistar,
                                  Direccion = lent.Direccion,
                                  Telefono = lent.Telefono,
                                  Fecha = lent.Fecha,
                                  Hora = lent.Hora).OrderBy(Function(o) o.Id)

            If Not String.IsNullOrEmpty(txtBuscar.Text) Then
                gvDatos.DataSource = lEntrevistas.Where(Function(o) o.Ciudad.ToUpper.Contains(txtBuscar.Text.ToUpper) Or o.PersonaAEntrevistar.ToUpper.Contains(txtBuscar.Text.ToUpper) Or o.Direccion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or o.Telefono.ToUpper.Contains(txtBuscar.Text.ToUpper)).ToList()
            Else
                gvDatos.DataSource = lEntrevistas.ToList
            End If
            gvDatos.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarInfo(ByVal idEntrevista As Int32)
        Try
            Dim oEntrevista As New Entrevistas
            Dim info = oEntrevista.DevolverxID(idEntrevista)
            ddlpais.SelectedValue = info.Pais
            CargarDepartamentos(info.Pais)
            ddldepartamento.SelectedValue = info.DivDepto
            cargarciudades(info.DivDepto)
            ddlEntrevistador.SelectedValue = info.EntrevistadorID
            ddlCiudad.SelectedValue = info.CiudadID
            txtPersonaAEntrevistar.Text = info.PersonaAEntrevistar
            txtDireccion.Text = info.Direccion
            txtTelefono.Text = info.Telefono
            txtGrupoObjetivo.Text = info.GrupoObjetivo
            txtCaracteristicasEspeciales.Text = info.CaracteristicasEspeciales
            txtObservaciones.Text = info.Observaciones
            txtFecha.Text = info.Fecha
            txtFechaReal.Text = info.FechaReal
            txtHora.Text = info.Hora.ToString
            txtHoraReal.Text = info.HoraReal.ToString
            chkCancelada.Checked = info.Cancelada
            hfEntrevistaID.Value = idEntrevista
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Eliminar(ByVal EntrevistaId As Int32)

        Try
            Dim oEntrevista As New Entrevistas
            oEntrevista.Eliminar(EntrevistaId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GuardarEntrevista()
        Try
            Dim oEntrevista As New Entrevistas
            Dim FichaEntrevistaId, Entrevistador, EntrevistaId As Int64
            Dim Ciudad As Int32
            Dim PersonaAEntrevistar, Direccion, Telefono, GrupoObjetivo, CaracteristicasEspeciales, Observaciones As String
            Dim Fecha, FechaReal As Date
            Dim Hora, HoraReal As TimeSpan
            Dim Cancelada As Boolean = False
            FichaEntrevistaId = Int64.Parse(hfFichaEntrevistaID.Value)
            Entrevistador = Int64.Parse(ddlEntrevistador.SelectedValue)
            Ciudad = Int32.Parse(ddlCiudad.SelectedValue)
            PersonaAEntrevistar = txtPersonaAEntrevistar.Text
            Direccion = txtDireccion.Text
            Telefono = txtTelefono.Text
            GrupoObjetivo = txtGrupoObjetivo.Text
            CaracteristicasEspeciales = txtCaracteristicasEspeciales.Text
            Observaciones = txtObservaciones.Text
            If Not String.IsNullOrEmpty(txtFecha.Text) Then
                Fecha = Date.Parse(txtFecha.Text)
            End If

            If Not String.IsNullOrEmpty(txtFechaReal.Text) Then
                FechaReal = Date.Parse(txtFechaReal.Text)
            End If

            If Not String.IsNullOrEmpty(txtHora.Text) Then
                Hora = TimeSpan.Parse(txtHora.Text)
            End If

            If Not String.IsNullOrEmpty(txtHoraReal.Text) Then
                HoraReal = TimeSpan.Parse(txtHoraReal.Text)
            End If
            Cancelada = chkCancelada.Checked

            If Not String.IsNullOrEmpty(hfEntrevistaID.Value) Then
                EntrevistaId = Int64.Parse(hfEntrevistaID.Value)
            End If

            oEntrevista.Guardar(EntrevistaId, FichaEntrevistaId, Ciudad, PersonaAEntrevistar, Direccion, Telefono, Fecha, Hora, GrupoObjetivo, CaracteristicasEspeciales, Entrevistador, FechaReal, HoraReal, Observaciones, Cancelada)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

End Class