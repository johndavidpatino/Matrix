Imports WebMatrix.Util
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports CoreProject
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Web.Services
Imports System.Web.Script.Services
Imports ClosedXML.Excel.XLPredefinedFormat

Public Class Capacitacion
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Private VolverCoord As Boolean
#End Region
#Region " Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(86, UsuarioID) = False Then
            Response.Redirect("../TH_TalentoHumano/Default.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("idtrabajo") IsNot Nothing Then
                Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
                hfidtrabajo.Value = idtrabajo
                If Request.QueryString("idcapacitacionref") IsNot Nothing Then
                    hfidCapacitacionRef.Value = Int64.Parse(Request.QueryString("idcapacitacionref").ToString)
                Else
                    hfidCapacitacionRef.Value = ""
                End If
                CargarCapacitaciones()
                If Request.QueryString("Coordinador") IsNot Nothing Then
                    VolverCoord = True
                    btnVolver.Visible = True
                Else
                    VolverCoord = False
                End If

            Else
                hfidtrabajo.Value = 0
                btnVolver.Visible = False
            End If
            CargarCapacitaciones()
            CargarResponsable()
            cargarCargos()
            Me.ddlresponsable.SelectedValue = Session("IDUsuario").ToString
            CargarGrid()
            If Request.QueryString("idcapacitacionref") IsNot Nothing Then
                Dim o As New Capacitaciones
                Dim idcap As Int64 = o.AdicionarRefuerzo(hfidCapacitacionRef.Value)
                hfidCapacitacion.Value = idcap
                CargarInfo(idcap)
                ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            End If
            'btnPlanilla.Visible = False
        End If
    End Sub
    Protected Sub ddlparticipanteempty_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddlresponsable_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlresponsable.DataBound
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    Protected Sub ddlparticipantefooter_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        DirectCast(sender, DropDownList).Items.Insert(0, InsertarItemSeleccion)
    End Sub
    'Protected Sub gvlista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvlista.RowCommand
    '    If e.CommandName = "Select" Then
    '        Dim index As Int32 = Int32.Parse(e.CommandArgument)
    '        Quitar(index)
    '    End If
    'End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idCapacitacion As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    CargarInfo(idCapacitacion)
                    'log(idCapacitacion, 3)
                Case "Eliminar"
                    Dim idCapacitacion As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Eliminar(idCapacitacion)
                    CargarCapacitaciones()
                    ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
                    log(idCapacitacion, 4)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargarCapacitaciones()
    End Sub
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        Limpiar()
        CargarGrid()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        txtUbicacion.Focus()
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            CargarCapacitaciones()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            GuardarCapacitacion()
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
            log(hfidCapacitacion.Value, 2)
            'Limpiar()
            CargarCapacitaciones()
            CargarNoAprobaron()
            ActivateAccordion(3, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(32, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
#Region "Funciones y Metodos"
    Public Sub Limpiar()
        ViewState("Items") = Nothing
        Dim oItems As New List(Of listadeItems)
        'gvlista.DataSource = oItems
        'gvlista.DataBind()

        txtUbicacion.Text = String.Empty
        txtFecha.Text = String.Empty
        txtHoras.Text = String.Empty
        txtActividad.Text = String.Empty
        'ddlresponsable.SelectedValue = "-1"
        txtObjetivoActividad.Text = String.Empty
        txtModoEvaluacion.Text = String.Empty
        hfidCapacitacion.Value = String.Empty
        txtEvaluadorPor.Text = String.Empty
        'hfidtrabajo.Value = ""
        hfidCapacitacionRef.Value = ""
    End Sub
    Sub cargarCargos()
        Dim oCargos As New Cargos
        Dim oCapacitaciones As New CapacitacionesParticipantes
        If Not (hfidCapacitacionRef.Value = "") Then
            'ddlCargos.DataSource = oCapacitaciones.TH_CargosRefuerzo_Get(hfidCapacitacionRef.Value)
        Else
            'ddlCargos.DataSource = oCargos.DevolverTodos
        End If
        'ddlCargos.DataTextField = "Cargo"
        'ddlCargos.DataValueField = "Id"
        'ddlCargos.DataBind()
        'ddlCargos.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = -1, .Text = "--Seleccione--"})
    End Sub
    Public Sub CargarCapacitaciones()
        Try
            Dim TrabajoID As Int64 = Int64.Parse(hfidtrabajo.Value)
            Dim oCapacitaciones As New Capacitaciones
            Dim listacapacitaciones = (From lcapa In oCapacitaciones.DevolverxTrabajoID(TrabajoID)
                                       Select Id = lcapa.id,
                                       TrabId = lcapa.TrabajoId,
                                       JobBook = lcapa.JobBook,
                                       NombreTrabajo = lcapa.NombreTrabajo,
                                       Ubicacion = lcapa.ubicacion,
                                       Fecha = lcapa.Fecha,
                                       Horas = lcapa.Duracion,
                                       Actividad = lcapa.Actividad,
                                       Responsable = lcapa.Apellidos & " " & lcapa.Nombres,
                                       ModoEvaluacion = lcapa.ModoEvaluacion,
                                       EvaluadoPor = lcapa.Capacitador).OrderBy(Function(c) c.Id)
            If Not String.IsNullOrEmpty(txtBuscar.Text) Then
                gvDatos.DataSource = listacapacitaciones.Where(Function(c) c.Ubicacion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Actividad.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Responsable.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.EvaluadoPor.ToUpper.Contains(txtBuscar.Text.ToUpper)).ToList()
            Else
                gvDatos.DataSource = listacapacitaciones.ToList()
            End If
            gvDatos.DataBind()
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub CargarResponsable()
        Try
            Dim oPersona As New Personas
            Dim listapersonas = (From lpersona In oPersona.TH_Usuarios_Combo_Get()
                                 Select Id = lpersona.id,
                              Nombre = lpersona.id.ToString & " " & lpersona.Nombres).OrderBy(Function(p) p.Nombre)
            ddlresponsable.DataSource = listapersonas.ToList()
            ddlresponsable.DataValueField = "Id"
            ddlresponsable.DataTextField = "Nombre"
            ddlresponsable.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarGrid()
        Try
            Dim oItems As New List(Of listadeItems)

            If ViewState("Items") IsNot Nothing Then
                oItems = CType(ViewState("Items"), List(Of listadeItems))
            End If

            'gvlista.DataSource = oItems.ToList
            'gvlista.DataBind()

            Dim oPersona As New Personas
            Dim oPersonasRefuerzo As New CapacitacionesParticipantes
            'Dim listapersonas = (From lpersona In oPersona.DevolverxCargoID(ddlCargos.SelectedValue)
            '                     Where lpersona.Activo = True
            '                     Select Id = lpersona.id,
            '                     Nombre = lpersona.id.ToString & " - " & lpersona.Apellidos & " " & lpersona.Nombres).OrderBy(Function(p) p.Id)
            'If Not (hfidCapacitacionRef.Value = "") Then
            '    Dim oCapacitaciones As New CapacitacionesParticipantes
            '    listapersonas = (From lpersona In oPersonasRefuerzo.TH_CapacitacionesParticipantesRefuerzo_Get(ddlCargos.SelectedValue, hfidCapacitacionRef.Value)
            '                     Where lpersona.Activo = True
            '                     Select Id = lpersona.id,
            '                     Nombre = lpersona.id.ToString & " - " & lpersona.Apellidos & " " & lpersona.Nombres).OrderBy(Function(p) p.Id)
            'End If
            'If gvlista.Rows.Count > 0 Then
            '    ''dropdown del footer del grid
            '    Dim ofooter As Control = gvlista.FooterRow
            '    CType(ofooter.FindControl("ddlparticipantefooter"), DropDownList).DataSource = listapersonas.ToList
            '    CType(ofooter.FindControl("ddlparticipantefooter"), DropDownList).DataValueField = "Id"
            '    CType(ofooter.FindControl("ddlparticipantefooter"), DropDownList).DataTextField = "Nombre"
            '    CType(ofooter.FindControl("ddlparticipantefooter"), DropDownList).DataBind()
            'Else
            '    ''dropdown del empty
            '    Dim oControl As Control = gvlista.Controls(0).Controls(0)
            '    CType(oControl.FindControl("ddlparticipanteempty"), DropDownList).DataSource = listapersonas.ToList
            '    CType(oControl.FindControl("ddlparticipanteempty"), DropDownList).DataValueField = "Id"
            '    CType(oControl.FindControl("ddlparticipanteempty"), DropDownList).DataTextField = "Nombre"
            '    CType(oControl.FindControl("ddlparticipanteempty"), DropDownList).DataBind()
            'End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub CargarNoAprobaron()
        Dim o As New CapacitacionesParticipantes
        Dim listapersonas = (From lpersona In o.TH_CapacitacionesParticipantesNoAprobaron_Get(Nothing, hfidCapacitacion.Value)
                             Select Id = lpersona.id,
                                 Nombre = lpersona.Apellidos & " " & lpersona.Nombres, CargoTexto = lpersona.CargoTexto, Eficacia = lpersona.Eficacia).OrderBy(Function(p) p.Nombre)
        Me.gvResultado.DataSource = listapersonas
        Me.gvResultado.DataBind()
        If Me.gvResultado.Rows.Count > 0 Then
            Me.pnlAcciones.Visible = True
        Else
            Me.pnlAcciones.Visible = False
        End If
    End Sub
    Public Sub CargarInfo(ByVal idCapacitacion As Int64)
        Try
            Dim oItems As New List(Of listadeItems)
            Dim oCapacitacion As New Capacitaciones
            Dim info = oCapacitacion.DevolverxID(idCapacitacion)
            txtUbicacion.Text = info.ubicacion
            txtFecha.Text = info.Fecha
            txtHoras.Text = info.Duracion
            txtActividad.Text = info.Actividad
            ddlresponsable.SelectedValue = info.Responsable
            txtObjetivoActividad.Text = info.ObjetivoActividad
            txtModoEvaluacion.Text = info.ModoEvaluacion
            hfidCapacitacion.Value = info.id
            txtEvaluadorPor.Text = info.Capacitador
            hfidtrabajo.Value = info.TrabajoId
            'hdCapacitacionIdParticipantes.Text = info.id



            Dim oDetalle As New CapacitacionesParticipantes
            Dim infodetalle = oDetalle.DevolverXCapacitacionId(idCapacitacion)
            For Each fila As TH_CapacitacionesParticipantes_Get_Result In infodetalle
                oItems.Add(New listadeItems With {.Aprobo = fila.Aprobo, .NombreParticipante = fila.Apellidos & " " & fila.Nombres, .Eficacia = fila.Eficacia, .ParticipanteId = fila.Participante})
            Next
            ViewState("Items") = oItems
            CargarGrid()
            CargarNoAprobaron()
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub Agregar(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim oItems As New List(Of listadeItems)
            If ViewState("Items") IsNot Nothing Then
                oItems = CType(ViewState("Items"), List(Of listadeItems))
            End If

            'If gvlista.Rows.Count > 0 Then
            '    ''dropdown del footer del grid
            '    Dim ofooter As Control = gvlista.FooterRow
            '    Dim oddlparticipantefooter As DropDownList = CType(ofooter.FindControl("ddlparticipantefooter"), DropDownList)
            '    Dim otxtEficaciafooter As TextBox = CType(ofooter.FindControl("txtEficaciafooter"), TextBox)
            '    Dim ochkAprobofooter As CheckBox = CType(ofooter.FindControl("chkAprobofooter"), CheckBox)


            '    If oddlparticipantefooter.SelectedValue = "-1" Then
            '        Throw New Exception("Debe Elegir un participante para agregar")
            '    End If

            '    If String.IsNullOrEmpty(otxtEficaciafooter.Text) Then
            '        Throw New Exception("Debe ingresar una eficacia para agregar")
            '    End If


            '    Dim Sw As Int32 = 0
            '    For Each fila As listadeItems In oItems
            '        If fila.ParticipanteId = Int64.Parse(oddlparticipantefooter.SelectedValue) Then
            '            Sw = 1
            '        End If
            '    Next

            '    If Sw = 0 Then
            '        Dim efi As Double
            '        efi = Double.Parse(otxtEficaciafooter.Text, New System.Globalization.CultureInfo("es-CO"))
            '        If efi < 0 Or efi > 5 Then
            '            Throw New Exception("La calificación debe estar entre 0 y 5")
            '        Else
            '            Dim aprobo As Boolean = True
            '            If efi < "3,5" Then aprobo = False

            '            oItems.Add(New listadeItems With {.ParticipanteId = Int64.Parse(oddlparticipantefooter.SelectedValue),
            '                                          .NombreParticipante = oddlparticipantefooter.SelectedItem.Text,
            '                                          .Eficacia = efi,
            '                                          .Aprobo = aprobo})
            '        End If
            '    Else
            '        Throw New Exception("Ya existe el participante en la lista")
            '    End If


            'Else
            '    ''dropdown del empty
            '    Dim oControl As Control = gvlista.Controls(0).Controls(0)
            '    Dim oddlparticipanteempty As DropDownList = CType(oControl.FindControl("ddlparticipanteempty"), DropDownList)
            '    Dim otxtEficaciaemtpy As TextBox = CType(oControl.FindControl("txtEficaciaemtpy"), TextBox)

            '    Dim ochkAproboempty As CheckBox = CType(oControl.FindControl("chkAproboempty"), CheckBox)

            '    If oddlparticipanteempty.SelectedValue = "-1" Then
            '        Throw New Exception("Debe Elegir un participante para agregar")
            '    End If

            '    If String.IsNullOrEmpty(otxtEficaciaemtpy.Text) Then
            '        Throw New Exception("Debe ingresar una eficacia para agregar")
            '    End If

            '    Dim Sw As Int32 = 0
            '    For Each fila As listadeItems In oItems
            '        If fila.ParticipanteId = Int64.Parse(oddlparticipanteempty.SelectedValue) Then
            '            Sw = 1
            '        End If
            '    Next

            '    If Sw = 0 Then
            '        Dim efi As Double
            '        efi = Double.Parse(otxtEficaciaemtpy.Text, New System.Globalization.CultureInfo("es-CO"))
            '        If efi < 0 Or efi > 5 Then
            '            Throw New Exception("La calificación debe estar entre 0 y 5")
            '        Else
            '            Dim aprobo As Boolean = True
            '            If efi < "3,5" Then aprobo = False

            '            oItems.Add(New listadeItems With {.ParticipanteId = Int64.Parse(oddlparticipanteempty.SelectedValue),
            '                                              .NombreParticipante = oddlparticipanteempty.SelectedItem.Text,
            '                                              .Eficacia = efi,
            '                                              .Aprobo = aprobo})
            '        End If
            '    Else
            '        Throw New Exception("Ya existe el participante en la lista")
            '    End If


            'End If

            'ViewState("Items") = oItems
            'gvlista.DataSource = ViewState("Items")
            'gvlista.DataBind()
            CargarGrid()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Public Sub Quitar(ByVal index As Int32)
        Dim oItems As New List(Of listadeItems)

        If ViewState("Items") IsNot Nothing Then
            oItems = CType(ViewState("Items"), List(Of listadeItems))
        End If

        oItems.RemoveAt(index)
        ViewState("Items") = oItems
        CargarGrid()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
    Public Sub GuardarCapacitacion()
        Try
            Dim oItems As New List(Of listadeItems)

            If ViewState("Items") IsNot Nothing Then
                oItems = CType(ViewState("Items"), List(Of listadeItems))
            Else
                'Throw New Exception("Debe ingresar por lo menos un participante")
            End If

            ''Capacitaciones
            Dim oCapacitacion As New Capacitaciones
            Dim Ubicacion = "", Actividad = "", Capacitador = "", ObjetivoActividad = "", ModoEvaluacion As String = ""
            Dim Fecha As Date
            Dim Duracion As Byte
            Dim CapacitacionID, ResponsableID, TrabajoID As Int64

            If Not String.IsNullOrEmpty(txtUbicacion.Text) Then
                Ubicacion = txtUbicacion.Text
            End If

            If Not String.IsNullOrEmpty(txtFecha.Text) Then
                'Throw New Exception($"Fecha: {txtFecha.Text}")
                If ValidarFecha(txtFecha.Text) Then
                    Fecha = Date.Parse(txtFecha.Text)
                Else
                    Throw New Exception("Fecha No Válida")
                End If
            End If

            If Not String.IsNullOrEmpty(txtHoras.Text) Then
                Duracion = Byte.Parse(txtHoras.Text)
            End If

            If Not String.IsNullOrEmpty(txtActividad.Text) Then
                Actividad = txtActividad.Text
            End If

            ResponsableID = Int64.Parse(ddlresponsable.SelectedValue)

            If Not String.IsNullOrEmpty(txtObjetivoActividad.Text) Then
                ObjetivoActividad = txtObjetivoActividad.Text
            End If

            If Not String.IsNullOrEmpty(txtModoEvaluacion.Text) Then
                ModoEvaluacion = txtModoEvaluacion.Text
            End If

            If Not String.IsNullOrEmpty(hfidtrabajo.Value) Then
                TrabajoID = Int64.Parse(hfidtrabajo.Value)
            End If

            If Not String.IsNullOrEmpty(hfidCapacitacion.Value) Then
                CapacitacionID = Int64.Parse(hfidCapacitacion.Value)
            End If

            If Not String.IsNullOrEmpty(txtEvaluadorPor.Text) Then
                Capacitador = txtEvaluadorPor.Text
            End If

            CapacitacionID = oCapacitacion.Guardar(CapacitacionID, Ubicacion, Fecha, Duracion, Actividad, ResponsableID, Capacitador, ObjetivoActividad, ModoEvaluacion, TrabajoID)

            hfidCapacitacion.Value = CapacitacionID

            Dim oDetalle As New CapacitacionesParticipantes
            oDetalle.EliminarXCapacitacionID(CapacitacionID)

            For Each fila As listadeItems In oItems
                oDetalle.Guardar(fila.ID, CapacitacionID, fila.ParticipanteId, fila.Eficacia, Nothing, fila.Aprobo)
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Eliminar(ByVal CapacitacionID As Int64)
        Try

            Dim oDetalle As New CapacitacionesParticipantes
            oDetalle.EliminarXCapacitacionID(CapacitacionID)

            Dim oCapacitacion As New Capacitaciones
            oCapacitacion.Eliminar(CapacitacionID)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function ValidarFecha(ByVal txtFecha As String) As Boolean
        Dim dt As Date

        Dim blnFlag As Boolean = Date.TryParse(txtFecha, dt)

        If blnFlag Then
            Return True
        Else
            Return False
        End If
    End Function


#End Region
    <Serializable()>
    Private Class listadeItems
        Public Property ID As Int64 = -1
        Public Property ParticipanteId As Int64
        Public Property NombreParticipante As String
        Public Property Eficacia As Double
        Public Property Oportunidad As String
        Public Property Aprobo As Boolean = False
    End Class



    Private Sub btnVolver_Click(sender As Object, e As System.EventArgs) Handles btnVolver.Click
        If Request.QueryString("Coordinador") IsNot Nothing Then
            If Request.QueryString("Coordinador").ToString = 2 Then
                Response.Redirect("../OP_Cuantitativo/TrabajosCallCenter.aspx")
            Else
                Response.Redirect("../OP_Cuantitativo/TrabajosCoordinador.aspx")
            End If
        Else
            Response.Redirect("../OP_Cuantitativo/Trabajos.aspx")
        End If
    End Sub

    'Private Sub ddlCargos_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCargos.SelectedIndexChanged
    '    CargarGrid()
    '    ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    'End Sub

    Private Sub btnRetro_Click(sender As Object, e As System.EventArgs) Handles btnRetro.Click
        Response.Redirect("../TH_TalentoHumano/Capacitacion.aspx?idtrabajo=" & hfidtrabajo.Value & "&idcapacitacionref=" & hfidCapacitacion.Value)
    End Sub

    Private Sub btnDocumentos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDocumentos.Click
        Dim o As New WorkFlow
        Dim idWF As Int64 = o.ObtenerWorkflowXTrabajoXTarea(hfidtrabajo.Value, 3).FirstOrDefault.id
        If hfidCapacitacion.Value <> "" Then
            Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & hfidtrabajo.Value & "&IdDocumento=76&TipoContenedor=1&IdWorkFlow=" & idWF)
        End If
    End Sub


    Sub CargarInformacion(ByVal Capacitacion As Int64)
        Dim op As New ProcesosInternos
        Dim Encabezado As New List(Of CC_EncabezadoPlanillaAsistencia_Result)
        Dim Participantes As New List(Of CC_ParticipantesPlanillaAsistencia_Result)

        If op.ParticipantesCapacitacion(Capacitacion, 1).Count > 0 Then
            Encabezado = op.EncabezadoPlanilla(Capacitacion, 1).ToList
            Participantes = op.ParticipantesCapacitacion(Capacitacion, 1).ToList
            CrearPlantilla(Encabezado, Participantes, 1)
        End If
        If op.ParticipantesCapacitacion(Capacitacion, 2).Count > 0 Then
            Encabezado = op.EncabezadoPlanilla(Capacitacion, 2).ToList
            Participantes = op.ParticipantesCapacitacion(Capacitacion, 2).ToList
            CrearPlantilla(Encabezado, Participantes, 2)
        End If
        If op.ParticipantesCapacitacion(Capacitacion, 4).Count > 0 Then
            Encabezado = op.EncabezadoPlanilla(Capacitacion, 4).ToList
            Participantes = op.ParticipantesCapacitacion(Capacitacion, 4).ToList
            CrearPlantilla(Encabezado, Participantes, 4)
        End If
    End Sub

    Sub CrearPlantilla(ByVal Encabezado As List(Of CC_EncabezadoPlanillaAsistencia_Result), ByVal Participantes As List(Of CC_ParticipantesPlanillaAsistencia_Result), ByVal Num As Int64)

        Dim urlFija As String
        Dim Url As String


        Dim path As String = Server.MapPath("~/Images/")
        urlFija = "~/PlanillaCapacitacion/"
        Url = "..\PlanillaCapacitacion\"
        urlFija = Server.MapPath(urlFija & "\")
        Dim Persona As New TH_PersonasGET_Result
        Dim pdfw As PdfWriter
        Dim documentoPDF As New Document(iTextSharp.text.PageSize.A4.Rotate, 0, 0, 20, 20) 'Creamos el objeto documento PDF
        pdfw = PdfWriter.GetInstance(documentoPDF, New FileStream(urlFija & "\" & "PlanillaCapacitacion-" & hfidCapacitacion.Value & "-" & Num & ".pdf", FileMode.Create))
        documentoPDF.Open()
        documentoPDF.NewPage()


        documentoPDF.NewPage()
        Dim aTable = New iTextSharp.text.pdf.PdfPTable(2)
        aTable.DefaultCell.Border = BorderStyle.None
        Dim Ancho As Single() = {0.45F, 3.25F}
        Dim Imagen As iTextSharp.text.Image
        Imagen = iTextSharp.text.Image.GetInstance(path & "logo-titulo.png")
        Imagen.ScalePercent(10)
        Dim Img = New PdfPCell
        Img.Border = 2
        Img.AddElement(Imagen)

        Dim C22 = New PdfPCell(New Paragraph("PLANILLA DE ASISTENCIA ACTIVIDADES DE FORMACIÓN OPERACIONES CUANTITATIVAS", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD)))
        C22.HorizontalAlignment = 2
        aTable.SetWidths(Ancho)
        aTable.AddCell(Img)
        aTable.AddCell(C22)

        Dim aTable1 = New iTextSharp.text.pdf.PdfPTable(2)

        Dim C1 = New PdfPCell(New Paragraph("Versión: 4", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C1.HorizontalAlignment = 1
        Dim C2 = New PdfPCell(New Paragraph("Fecha: Sep-14-2014", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C2.HorizontalAlignment = 1
        aTable1.AddCell(C1)
        aTable1.AddCell(C2)


        Dim aTable2 = New iTextSharp.text.pdf.PdfPTable(6)
        aTable2.DefaultCell.Border = Rectangle.NO_BORDER
        aTable2.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable2.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable2.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable2.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable2.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable2.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

        Dim CL3 = New PdfPCell(New Paragraph("Ciudad:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL3.HorizontalAlignment = 2
        Dim CL4 = New PdfPCell(New Paragraph(Encabezado(0).Ubicacion, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL4.HorizontalAlignment = 3
        Dim CL5 = New PdfPCell(New Paragraph("Fecha:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL5.HorizontalAlignment = 2
        Dim CL6 = New PdfPCell(New Paragraph(Encabezado(0).Fecha, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL6.HorizontalAlignment = 3
        Dim CL7 = New PdfPCell(New Paragraph("Responsable:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL7.HorizontalAlignment = 2
        Dim CL8 = New PdfPCell(New Paragraph(Encabezado(0).Responsable, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL8.HorizontalAlignment = 3

        Dim CL9 = New PdfPCell(New Paragraph("Actividad:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL9.HorizontalAlignment = 2
        Dim CL10 = New PdfPCell(New Paragraph(Encabezado(0).Actividad, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL10.HorizontalAlignment = 1
        Dim CL11 = New PdfPCell(New Paragraph("Proyecto:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL11.HorizontalAlignment = 2
        Dim CL12 = New PdfPCell(New Paragraph(Encabezado(0).Proyecto, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL12.HorizontalAlignment = 3
        Dim CL13 = New PdfPCell(New Paragraph("Duracion:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL13.HorizontalAlignment = 2
        Dim CL14 = New PdfPCell(New Paragraph(Encabezado(0).Duracion, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL14.HorizontalAlignment = 3

        aTable2.AddCell(CL3)
        aTable2.AddCell(CL4)
        aTable2.AddCell(CL5)
        aTable2.AddCell(CL6)
        aTable2.AddCell(CL7)
        aTable2.AddCell(CL8)

        aTable2.AddCell(CL9)
        aTable2.AddCell(CL10)
        aTable2.AddCell(CL11)
        aTable2.AddCell(CL12)
        aTable2.AddCell(CL13)
        aTable2.AddCell(CL14)


        Dim aTable5 = New iTextSharp.text.pdf.PdfPTable(4)
        aTable5.DefaultCell.Border = Rectangle.NO_BORDER
        aTable5.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable5.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable5.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable5.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))


        Dim CL55 = New PdfPCell(New Paragraph("Cargo: ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL55.HorizontalAlignment = 2
        Dim CL56 = New PdfPCell(New Paragraph(Encabezado(0).Cargo, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL56.HorizontalAlignment = 1
        Dim CL57 = New PdfPCell(New Paragraph("Valor Encuesta: ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL57.HorizontalAlignment = 2


        aTable5.AddCell(CL55)
        aTable5.AddCell(CL56)
        aTable5.AddCell(CL57)
        If IsNothing(Encabezado(0).ValorEncuesta) Then
            Dim CL58 = New PdfPCell(New Paragraph(0, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL58.HorizontalAlignment = 3
            aTable5.AddCell(CL58)
        Else
            Dim CL58 = New PdfPCell(New Paragraph(FormatCurrency(Encabezado(0).ValorEncuesta, 0), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL58.HorizontalAlignment = 3
            aTable5.AddCell(CL58)
        End If




        Dim aTable3 = New iTextSharp.text.pdf.PdfPTable(10)
        aTable3.DefaultCell.Border = Rectangle.NO_BORDER
        aTable3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

        Dim CL15 = New PdfPCell(New Paragraph("N° ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL15.HorizontalAlignment = 1
        Dim CL16 = New PdfPCell(New Paragraph("Cedula", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL16.HorizontalAlignment = 1
        Dim CL17 = New PdfPCell(New Paragraph("Nombre del Participante", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL17.HorizontalAlignment = 1
        Dim CL18 = New PdfPCell(New Paragraph("Tiempo de experiencia en el cargo", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL18.HorizontalAlignment = 1
        Dim CL19 = New PdfPCell(New Paragraph("Calificación Evaluación *", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL19.HorizontalAlignment = 1
        Dim CL20 = New PdfPCell(New Paragraph("Acciones a tomar **", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL20.HorizontalAlignment = 1
        Dim CL21 = New PdfPCell(New Paragraph("Personal Ipsos***", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL21.HorizontalAlignment = 1
        Dim CL22 = New PdfPCell(New Paragraph("Contratista***", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL22.HorizontalAlignment = 1
        Dim CL23 = New PdfPCell(New Paragraph("Firma", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL23.HorizontalAlignment = 1
        Dim CL24 = New PdfPCell(New Paragraph("Nombre del contratista****", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL24.HorizontalAlignment = 1



        aTable3.AddCell(CL15)
        aTable3.AddCell(CL16)
        aTable3.AddCell(CL17)
        aTable3.AddCell(CL18)
        aTable3.AddCell(CL19)
        aTable3.AddCell(CL20)
        aTable3.AddCell(CL21)
        aTable3.AddCell(CL22)
        aTable3.AddCell(CL23)
        aTable3.AddCell(CL24)
        Dim Ancho2 As Single() = {0.2F, 0.35F, 0.6F, 0.5F, 0.45F, 0.5F, 0.45F, 0.45F, 0.45F, 0.45F}
        aTable3.SetWidths(Ancho2)

        For i = 0 To Participantes.Count - 1
            Dim CL25 = New PdfPCell(New Paragraph(i + 1, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL25.HorizontalAlignment = 1
            Dim CL26 = New PdfPCell(New Paragraph(Participantes(i).Cedula, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL26.HorizontalAlignment = 1
            Dim CL27 = New PdfPCell(New Paragraph(Participantes(i).NombreParticipante, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL27.HorizontalAlignment = 1
            Dim CL28 = New PdfPCell(New Paragraph(Participantes(i).TiempoExperiencia, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL28.HorizontalAlignment = 1
            Dim CL29 = New PdfPCell(New Paragraph(Participantes(i).Calificacion, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL29.HorizontalAlignment = 1
            Dim CL30 = New PdfPCell(New Paragraph(Participantes(i).AccionesATomar, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL30.HorizontalAlignment = 1
            Dim CL31 = New PdfPCell(New Paragraph(Participantes(i).PersonalIpsos, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL31.HorizontalAlignment = 1
            Dim CL32 = New PdfPCell(New Paragraph(Participantes(i).Contratista, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL32.HorizontalAlignment = 1
            Dim CL33 = New PdfPCell(New Paragraph(Participantes(i).Firma, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL33.HorizontalAlignment = 1
            Dim CL34 = New PdfPCell(New Paragraph(Participantes(i).NombreContratista, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL34.HorizontalAlignment = 1

            aTable3.AddCell(CL25)
            aTable3.AddCell(CL26)
            aTable3.AddCell(CL27)
            aTable3.AddCell(CL28)
            aTable3.AddCell(CL29)
            aTable3.AddCell(CL30)
            aTable3.AddCell(CL31)
            aTable3.AddCell(CL32)
            aTable3.AddCell(CL33)
            aTable3.AddCell(CL34)
        Next
        Dim aTable4 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable4.DefaultCell.Border = Rectangle.NO_BORDER
        Dim CL35 = New PdfPCell(New Paragraph("* El índice de calificación se realizará en una escala de (1-5). Esta calificación incluye también evaluación práctica en el caso de la evaluación básica de campo", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL35.HorizontalAlignment = 1
        Dim CL36 = New PdfPCell(New Paragraph("** Acciones a tomar: Retroalimentación, re entrenamiento, no aceptación, acompañamiento. Si no fue necesario tomar ninguna acción, se marcará como N/A", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL36.HorizontalAlignment = 1
        Dim CL37 = New PdfPCell(New Paragraph("***Si el personal es contratista se registra en la columna con una (X) y el nombre, de ser personal propio identificar como Ipsos con una (X) sobre la columna", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL37.HorizontalAlignment = 1
        Dim CL38 = New PdfPCell(New Paragraph("**** Nombre del contratista: Nombre de la persona contratada por Ipsos Napoleón Franco que tiene a su cargo encuestadores y/o supervisores.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL38.HorizontalAlignment = 1
        aTable4.AddCell(CL35)
        aTable4.AddCell(CL36)
        aTable4.AddCell(CL37)
        aTable4.AddCell(CL38)







        documentoPDF.Add(aTable)
        documentoPDF.Add(aTable1)
        documentoPDF.Add(aTable2)
        documentoPDF.Add(aTable5)
        documentoPDF.Add(aTable3)
        documentoPDF.Add(aTable4)
        documentoPDF.AddAuthor(Session("IDUsuario").ToString)
        documentoPDF.AddTitle("PlanillaCapacitacion")
        documentoPDF.AddCreationDate()
        documentoPDF.Close() 'Cerramos el objeto documento, guardamos y creamos el PDF


        'Comprobamos si se ha creado el fichero PDF
        If System.IO.File.Exists(urlFija & "\" & "PlanillaCapacitacion-" & hfidCapacitacion.Value & "-" & Num & ".pdf") Then
            'ShowNotification("Archivo Generado", ShowNotifications.InfoNotification)
            'Response.Redirect("../CC_FinzOpe/ListadodeRequerimientos.aspx?Tipo=4" & "&Id=" & InfoContr.Item(0).Identificacion& & "&TrabajoId=1")
            ShowWindows("../CC_FinzOpe/ListadodeRequerimientos.aspx?Tipo=6" & "&Id=" & hfidCapacitacion.Value & "&TrabajoId=" & Num)

        Else

            ShowNotification("El fichero PDF no se ha generado, compruebe que tiene permisos en la carpeta de destino.", ShowNotifications.InfoNotification)
        End If

    End Sub



    Protected Sub btnPlanilla_Click(sender As Object, e As EventArgs) Handles btnPlanilla.Click
        CargarInformacion(hfidCapacitacion.Value)
    End Sub

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getCapacitacionPersonas(Identificacion As Long?,
                                            Nombre As String,
                                            ContratistaId As Long?,
                                            NombreContratista As String,
                                            CapacitacionId As Long?,
                                            SonParticipantes As Integer,
                                            Page As Integer,
                                            PageSize As Integer) As List(Of CoreProject.TH_CapacitacionPersonas_Get_Result)

        Dim capacitaciones As New Capacitaciones()
        Dim parameters As New Capacitaciones.TH_CapacitacionPersonasParameters With {
            .Identificacion = Identificacion,
            .Nombre = Nombre,
            .ContratistaId = ContratistaId,
            .NombreContratista = NombreContratista,
            .CapacitacionId = CapacitacionId,
            .SonParticipantes = SonParticipantes,
            .Page = Page,
            .PageSize = PageSize
        }
        Return capacitaciones.TH_CapacitacionPersonas_Get(parameters)

    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getCapacitacionParticipantes(CapacitacionId As Long)
        Dim capacitaciones As New Capacitaciones()
        Return capacitaciones.TH_CapacitacionParticipantes_Get(CapacitacionId)
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function addCapacitacionPerson(CapacitacionId As Long, Participante As Long, Eficacia As Long?, OportunidadMejora As String, Aprobo As Boolean)
        Dim capacitaciones As New Capacitaciones()
        Dim parameters As New Capacitaciones.TH_CapacitacionesParticipantes_Add_Parameters With {
            .CapacitacionId = CapacitacionId,
            .Eficacia = Eficacia,
            .Aprobo = Aprobo,
            .OportunidadMejora = OportunidadMejora,
            .Participante = Participante
        }
        Return capacitaciones.TH_CapacitacionesParticipantes_Add(parameters)
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function removeCapacitacionParticipant(ParticipantId As Long) As Boolean
        Dim capacitaciones As New Capacitaciones()
        Return capacitaciones.TH_CapacitacionesParticipantes_Del(ParticipantId)
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function updateCapacitacionParticipant(CapacitacionParticipanteId As Long, CapacitacionId As Long, Participante As Long?, Eficacia As Double?, OportunidadMejora As String, Aprobo As Boolean) As Boolean
        Dim capacitaciones As New Capacitaciones()
        Dim paramters = New Capacitaciones.TH_CapacitacionesParticipantes_Update_Parameters With {
            .CapacitacionParticipanteId = CapacitacionParticipanteId,
            .CapacitacionId = CapacitacionId,
            .Eficacia = Eficacia,
            .Aprobo = Aprobo,
            .OportunidadMejora = OportunidadMejora,
            .Participante = Participante
        }
        Return capacitaciones.TH_CapacitacionesParticipantes_Update(paramters)
    End Function
End Class