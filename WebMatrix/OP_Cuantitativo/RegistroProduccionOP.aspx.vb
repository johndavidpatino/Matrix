Imports CoreProject
Imports WebMatrix.Util

Public Class RegistroProduccionOP
    Inherits System.Web.UI.Page

#Region "Propiedades"

#End Region

    Public Enum EAreas
        Procesamiento = 23
        Scripting = 18
    End Enum

    Public Enum EReproceso
        Si = 1
    End Enum

    Public Enum EActividad
        CrearScript = 36
        ReutilizarScript = 37
    End Enum

#Region "Funciones y Métodos"
    Sub CargarUnidades(identificacion As Int64)
        Dim o As New RecordProduccion
        Me.ddlAreas.DataSource = o.ObtenerUnidades(identificacion)
        Me.ddlAreas.DataTextField = "Unidad"
        Me.ddlAreas.DataValueField = "id"
        Me.ddlAreas.DataBind()
        Me.ddlAreas.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = "-1"})
    End Sub

    Sub CargarActividades()
        Dim o As New RecordProduccion
        Dim unidad As Int32? = Nothing
        If Not (Me.ddlAreas.SelectedValue = "-1") Then unidad = Me.ddlAreas.SelectedValue
        Dim op = (From x In o.MatrizActividades(unidad, Nothing, Nothing, True)
                  Order By x.Actividad
                  Group By x.Actividad, x.ActividadCod Into Group).ToList
        Me.ddlActividad.DataSource = op
        Me.ddlActividad.DataTextField = "Actividad"
        Me.ddlActividad.DataValueField = "ActividadCod"
        Me.ddlActividad.DataBind()
        Me.ddlActividad.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = "-1"})
    End Sub

    Sub CargarSubActividades()
        Dim o As New RecordProduccion
        Dim unidad As Int32? = Nothing
        Dim actividad As Int32? = Nothing
        If Not (Me.ddlAreas.SelectedValue = "-1") Then unidad = Me.ddlAreas.SelectedValue
        If Not (Me.ddlActividad.SelectedValue = "-1") Then actividad = Me.ddlActividad.SelectedValue
        Dim op = (From x In o.MatrizActividades(unidad, actividad, Nothing, True)
                  Where Not (x.SubActividadCod Is Nothing)
                  Order By x.SubActividad
                  Group By x.SubActividad, x.SubActividadCod Into Group).ToList
        Me.ddlSubActividad.DataSource = op
        Me.ddlSubActividad.DataTextField = "SubActividad"
        Me.ddlSubActividad.DataValueField = "SubActividadCod"
        Me.ddlSubActividad.DataBind()
    End Sub

    Sub CargarJBE()

        txtJBEJBICC.Text = ""
        gvJBEJBICC.DataSource = ""
        gvJBEJBICC.DataBind()
        upJBEJBICC.Update()

        Dim o As New RecordProduccion
        If IsNumeric(rbTipoJB.SelectedValue) Then
            Me.ddlJB.DataSource = o.JBE_JBI(rbTipoJB.SelectedValue)
            Me.ddlJB.DataTextField = "Nombre"
            Me.ddlJB.DataValueField = "id"
            Me.ddlJB.DataBind()
            Me.ddlJB.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = "-1"})
        End If

        If rbTipoJB.SelectedValue = 1 Or rbTipoJB.SelectedValue = 2 Then
            btnSearchJBEJBICC.Visible = True
        Else
            btnSearchJBEJBICC.Visible = False
        End If

    End Sub
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#End Region

    Private Sub ddlActividad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlActividad.SelectedIndexChanged
        CargarSubActividades()
        ocultarDdlTipoAplicativoProceso()
        ocultarCamposReproceso()
    End Sub

    Private Sub ddlAreas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAreas.SelectedIndexChanged
        CargarActividades()
        cargarListaAplicativos(ddlAreas.SelectedValue)
        ddlSubActividad.Items.Clear()
        ocultarCamposCantidades(ddlAreas.SelectedValue)

    End Sub

    Private Sub rbTipoJB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbTipoJB.SelectedIndexChanged
        CargarJBE()
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim oRecordProduccion As New RecordProduccion

        Try
            If String.IsNullOrEmpty(lblIdActualizar.Text) Then
                oRecordProduccion.grabar(ddlActividad.SelectedValue,
                                         If(String.IsNullOrEmpty(ddlSubActividad.SelectedValue), CType(Nothing, Integer?),
                                         CType(ddlSubActividad.SelectedValue, Integer?)),
                                        ddlAreas.SelectedValue, If(rbTipoJB.SelectedValue = 2, ddlJB.SelectedValue, Nothing),
                                        If(rbTipoJB.SelectedValue = 1, ddlJB.SelectedValue, Nothing),
                                        txtFecha.Text, TimeSpan.Parse(txtHoraInicial.Text),
                                         TimeSpan.Parse(txtHoraFinal.Text),
                                         If(String.IsNullOrEmpty(txtCantidadGeneral.Text), CType(Nothing, Integer?), CType(txtCantidadGeneral.Text, Integer?)),
                                        txtObservacion.Text.Replace(ChrW(10), " "),
                                         Nothing,
                                         Nothing,
                                         txtIdentificacion.Text,
                                         ddlReproceso.SelectedValue,
                                        If(String.IsNullOrEmpty(txtCantidadEfectivas.Text), CType(Nothing, Integer?), CType(txtCantidadEfectivas.Text, Integer?)),
                                         If(ddlReproceso.SelectedValue = EReproceso.Si, CType(ddlTipoReproceso.SelectedValue, Byte?),
                                         CType(Nothing, Byte?)),
                                        If(ddlTipoAplicativoProceso.SelectedValue > -1, CType(ddlTipoAplicativoProceso.SelectedValue, Byte?), CType(Nothing, Byte?)),
                                        If(String.IsNullOrEmpty(txtCantVarsScript.Text), CType(Nothing, Integer?), CType(txtCantVarsScript.Text, Integer?)),
                                         CType(Nothing, Integer?))
            Else
                oRecordProduccion.actualizar(lblIdActualizar.Text,
                                             ddlActividad.SelectedValue,
                                             If(String.IsNullOrEmpty(ddlSubActividad.SelectedValue),
                                            CType(Nothing, Integer?),
                                            CType(ddlSubActividad.SelectedValue, Integer?)),
                                            ddlAreas.SelectedValue,
                                            If(rbTipoJB.SelectedValue = 2, ddlJB.SelectedValue, Nothing),
                                            If(rbTipoJB.SelectedValue = 1, ddlJB.SelectedValue, Nothing),
                                            txtFecha.Text, TimeSpan.Parse(txtHoraInicial.Text),
                                            TimeSpan.Parse(txtHoraFinal.Text),
                                            If(String.IsNullOrEmpty(txtCantidadGeneral.Text), CType(Nothing, Integer?), CType(txtCantidadGeneral.Text, Integer?)),
                                             txtObservacion.Text.Replace(ChrW(10), " "),
                                            Nothing, Nothing, txtIdentificacion.Text, ddlReproceso.SelectedValue,
                                             If(String.IsNullOrEmpty(txtCantidadEfectivas.Text), CType(Nothing, Integer?), CType(txtCantidadEfectivas.Text, Integer?)),
                                            If(ddlReproceso.SelectedValue = EReproceso.Si, CType(ddlTipoReproceso.SelectedValue, Byte?), CType(Nothing, Byte?)),
                                            If(ddlTipoAplicativoProceso.SelectedValue > -1, CType(ddlTipoAplicativoProceso.SelectedValue, Byte?), CType(Nothing, Byte?)),
                                            If(String.IsNullOrEmpty(txtCantVarsScript.Text), CType(Nothing, Integer?), CType(txtCantVarsScript.Text, Integer?)),
                                            CType(Nothing, Integer?))
            End If
            limpiar()
            cargarProduccion()
            ShowNotification("El registro ha sido guardado correctamente", ShowNotifications.InfoNotification)
        Catch ex As Exception
            ShowNotification("A ocurrido un error - " & ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Sub limpiar()
        ddlAreas.ClearSelection()
        ddlActividad.DataSource = ""
        ddlActividad.DataBind()
        ddlSubActividad.ClearSelection()
        ddlSubActividad.DataSource = ""
        ddlSubActividad.DataBind()
        ddlJB.DataSource = ""
        ddlJB.DataBind()
        ddlTipoReproceso.ClearSelection()
        ddlTipoReproceso.Visible = False
        lblTipoReproceso.Visible = False
        ddlTipoAplicativoProceso.ClearSelection()
        ddlTipoAplicativoProceso.Visible = False
        lblTipoAplicativoProceso.Visible = False
        rbTipoJB.ClearSelection()
        ddlReproceso.ClearSelection()
        txtCantidadEfectivas.Text = ""
        txtCantidadGeneral.Text = ""
        txtHoraInicial.Text = ""
        txtHoraFinal.Text = ""
        txtObservacion.Text = ""
        lblIdActualizar.Text = ""
        lblIdActualizar.Visible = False
        lblActualizar.Visible = False
        txtJBEJBICC.Text = ""
        gvJBEJBICC.DataSource = ""
        gvJBEJBICC.DataBind()


    End Sub
    Sub cargarProduccion()
        Dim lstProduccion As New List(Of OP_Produccion_Get_Result)
        If Not String.IsNullOrEmpty(txtIdentificacion.Text) Then
            lstProduccion = obtenerProduccion(If(String.IsNullOrEmpty(txtFecha.Text), Nothing, CType(txtFecha.Text, Date?)), If(String.IsNullOrEmpty(txtFecha.Text), Nothing, CType(txtFecha.Text, Date?)), txtIdentificacion.Text, Nothing, Nothing)
            Dim o = Math.Round(lstProduccion.Sum(Function(x) x.DiferenciaMinutos).Value)
            lblCantHorasReg.Text = o
            gvDatos.DataSource = lstProduccion
            gvDatos.DataBind()
        End If
    End Sub

    Sub CargarGridJBEJBICC()

        Dim o As New RecordProduccion
        Dim Busqueda As String = Nothing

        If Not txtJBEJBICC.Text = "" Then Busqueda = txtJBEJBICC.Text

        If IsNumeric(rbTipoJB.SelectedValue) Then
            Me.gvJBEJBICC.DataSource = o.JBE_JBI_Busqueda(rbTipoJB.SelectedValue, Busqueda)
            Me.gvJBEJBICC.DataBind()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End If

    End Sub

    Function obtenerProduccion(ByVal fechaInicio As Date?, ByVal fechaFin As Date?, ByVal personaId As Int64?, ByVal id As Integer?, ByVal unidad As Integer?) As List(Of OP_Produccion_Get_Result)
        Dim lstProduccion As New List(Of OP_Produccion_Get_Result)
        Dim RecordProduccion As New RecordProduccion
        lstProduccion = RecordProduccion.obtener(If(String.IsNullOrEmpty(txtFecha.Text), Nothing, CType(txtFecha.Text, Date?)), If(String.IsNullOrEmpty(txtFecha.Text), Nothing, CType(txtFecha.Text, Date?)), txtIdentificacion.Text, id, unidad)
        Return lstProduccion
    End Function
    Private Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        cargarProduccion()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvDatos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Dim lstProduccion As List(Of OP_Produccion_Get_Result)
        If e.CommandName = "Actualizar" Then
            lstProduccion = obtenerProduccion(If(String.IsNullOrEmpty(txtFecha.Text), Nothing, CType(txtFecha.Text, Date?)), If(String.IsNullOrEmpty(txtFecha.Text), Nothing, CType(txtFecha.Text, Date?)), txtIdentificacion.Text, gvDatos.DataKeys(e.CommandArgument).Value, Nothing)
            ddlAreas.SelectedValue = lstProduccion(0).Area
            CargarActividades()
            ddlActividad.SelectedValue = lstProduccion(0).Actividad
            CargarSubActividades()
            ddlSubActividad.SelectedValue = If(lstProduccion(0).SubActividad.HasValue, lstProduccion(0).SubActividad, False)
            rbTipoJB.SelectedValue = lstProduccion(0).TipoId
            CargarJBE()
            If lstProduccion(0).TipoId = 3 Then
                ddlJB.SelectedValue = 0
            Else
                ddlJB.SelectedValue = lstProduccion(0).IdJobBook
            End If

            ddlReproceso.SelectedValue = If(lstProduccion(0).EsReproceso = "Si", 1, 0)
            txtFecha.Text = lstProduccion(0).Fecha
            txtHoraInicial.Text = lstProduccion(0).HoraInicio.ToString
            txtHoraFinal.Text = lstProduccion(0).HoraFin.ToString
            txtCantidadGeneral.Text = If(lstProduccion(0).CantidadGeneral Is Nothing, "", lstProduccion(0).CantidadGeneral)
            txtCantidadEfectivas.Text = If(lstProduccion(0).CantidadEfectivas Is Nothing, "", lstProduccion(0).CantidadEfectivas)
            txtObservacion.Text = lstProduccion(0).Observacion
            cargarListaAplicativos(ddlAreas.SelectedValue)
            ddlTipoAplicativoProceso.SelectedValue = If(lstProduccion(0).TipoAplicativoProceso.HasValue, lstProduccion(0).TipoAplicativoProceso, -1)
            ocultarDdlTipoAplicativoProceso()
            ddlTipoReproceso.SelectedValue = If(lstProduccion(0).TipoReproceso.HasValue, lstProduccion(0).TipoReproceso, -1)
            ocultarCamposReproceso()
            ocultarTipoReproceso()
            lblIdActualizar.Text = lstProduccion(0).id
            lblActualizar.Visible = True
            lblIdActualizar.Visible = True

            ocultarCamposCantidades(ddlAreas.SelectedValue)
        End If
    End Sub

    Private Sub gvJBEJBICC_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvJBEJBICC.RowCommand
        If e.CommandName = "Seleccionar" Then
            Me.ddlJB.SelectedValue = Server.HtmlDecode(gvJBEJBICC.Rows(e.CommandArgument).Cells(0).Text)

            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End If
    End Sub

    Private Sub txtFecha_TextChanged(sender As Object, e As EventArgs) Handles txtFecha.TextChanged
        cargarProduccion()
    End Sub

    Private Sub txtIdentificacion_TextChanged(sender As Object, e As EventArgs) Handles txtIdentificacion.TextChanged
        Dim nombresPersona As String
        If Not String.IsNullOrEmpty(txtIdentificacion.Text.Trim) Then
            nombresPersona = obtenerNombresPersonaXIdentificacion(txtIdentificacion.Text.Trim)
            If Not String.IsNullOrEmpty(nombresPersona) Then
                lblNombresApellidos.Text = nombresPersona
                CargarUnidades(txtIdentificacion.Text.Trim)
                If String.IsNullOrEmpty(txtFecha.Text) Then
                    gvDatos.DataSource = Nothing
                    gvDatos.DataBind()
                Else
                    cargarProduccion()
                End If
            Else
                lblNombresApellidos.Text = "Identificación no valida"
            End If
        End If
        limpiar()
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        limpiar()
        lblIdActualizar.Text = ""
        lblCantHorasReg.Text = ""
        txtFecha.Text = ""
        gvDatos.DataSource = Nothing
        gvDatos.DataBind()
    End Sub

    Sub ocultarDdlTipoAplicativoProceso()
        Dim oRecord As New RecordProduccion
        Dim actividad = oRecord.obtenerListaActividades(ddlActividad.SelectedValue)
        If (actividad.AplicaTodos = False And (ddlAreas.SelectedValue = EAreas.Procesamiento OrElse ddlAreas.SelectedValue = EAreas.Scripting)) Then
            ddlTipoAplicativoProceso.Visible = True
            lblTipoAplicativoProceso.Visible = True
        Else
            ddlTipoAplicativoProceso.Visible = False
            lblTipoAplicativoProceso.Visible = False
            ddlTipoAplicativoProceso.ClearSelection()
        End If
    End Sub
    Sub ocultarCamposCantidades(area As EAreas)

        If area = EAreas.Scripting Then
            lblCantVarsScript.Visible = True
            txtCantVarsScript.Visible = True
            txtCantVarsScript.Text = ""
        Else
            lblCantVarsScript.Visible = False
            txtCantVarsScript.Visible = False
            txtCantVarsScript.Text = ""
        End If

        If area = EAreas.Procesamiento OrElse area = EAreas.Scripting Then
            lblCantidadEfectivas.Visible = False
            txtCantidadEfectivas.Visible = False
            txtCantidadEfectivas.Text = ""
            lblCantidadGeneral.Visible = False
            txtCantidadGeneral.Visible = False
            txtCantidadGeneral.Text = ""
        Else
            lblCantidadEfectivas.Visible = True
            txtCantidadEfectivas.Visible = True
            lblCantidadGeneral.Visible = True
            txtCantidadGeneral.Visible = True
            lblCantVarsScript.Visible = False
            txtCantVarsScript.Visible = False
        End If
    End Sub
    Sub ocultarCamposReproceso()
        Dim oRecord As New RecordProduccion
        Dim actividad = oRecord.obtenerListaActividades(ddlActividad.SelectedValue)
        If actividad.AplicaTodos Then
            lblReproceso.Visible = False
            ddlReproceso.Visible = False
            ddlReproceso.ClearSelection()
            lblTipoReproceso.Visible = False
            ddlTipoReproceso.Visible = False
            ddlTipoReproceso.ClearSelection()
        Else
            lblReproceso.Visible = True
            ddlReproceso.Visible = True
            lblTipoReproceso.Visible = True
            ddlTipoReproceso.Visible = True
        End If
    End Sub

    Sub cargarListaAplicativos(area As EAreas)
        ddlTipoAplicativoProceso.Items.Clear()

        Select Case area
            Case EAreas.Scripting
                ddlTipoAplicativoProceso.Items.Add(New ListItem With {.Text = "--Seleccione--", .Value = "-1"})
                ddlTipoAplicativoProceso.Items.Add(New ListItem With {.Text = "Survey To Go", .Value = "5"})
                ddlTipoAplicativoProceso.Items.Add(New ListItem With {.Text = "Ifield", .Value = "6"})
                ddlTipoAplicativoProceso.Items.Add(New ListItem With {.Text = "Dimensions", .Value = "7"})
                ddlTipoAplicativoProceso.Items.Add(New ListItem With {.Text = "ShopMetrics", .Value = "8"})
            Case EAreas.Procesamiento
                ddlTipoAplicativoProceso.Items.Add(New ListItem With {.Text = "--Seleccione--", .Value = "-1"})
                ddlTipoAplicativoProceso.Items.Add(New ListItem With {.Text = "Dimensions", .Value = "7"})
                ddlTipoAplicativoProceso.Items.Add(New ListItem With {.Text = "ShopMetrics", .Value = "8"})
                ddlTipoAplicativoProceso.Items.Add(New ListItem With {.Text = "Gandia", .Value = "1"})
                ddlTipoAplicativoProceso.Items.Add(New ListItem With {.Text = "SPSS Statistics", .Value = "4"})
                ddlTipoAplicativoProceso.Items.Add(New ListItem With {.Text = "Harmony", .Value = "9"})
            Case Else
                ddlTipoAplicativoProceso.Items.Add(New ListItem With {.Text = "--Seleccione--", .Value = "-1"})
        End Select

    End Sub

    Sub ocultarTipoReproceso()
        If ddlReproceso.SelectedValue = EReproceso.Si Then
            ddlTipoReproceso.Visible = True
            lblTipoReproceso.Visible = True
        Else
            ddlTipoReproceso.ClearSelection()
            ddlTipoReproceso.Visible = False
            lblTipoReproceso.Visible = False
        End If
        ddlTipoReproceso.ClearSelection()
    End Sub

    Private Sub ddlReproceso_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlReproceso.SelectedIndexChanged
        ocultarTipoReproceso()
    End Sub

    Protected Sub btnSearchJBEJBICC_Click(sender As Object, e As EventArgs) Handles btnSearchJBEJBICC.Click
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnBuscarJBEJBICC_Click(sender As Object, e As EventArgs) Handles btnBuscarJBEJBICC.Click
        CargarGridJBEJBICC()
        upJBEJBICC.Update()
    End Sub

    Protected Function obtenerNombresPersonaXIdentificacion(ByVal identificacion As Long) As String
        Dim oPersonas As New Personas
        Dim o As New TH_Personas_Get_Result
        Dim nombres As String = ""
        If Not String.IsNullOrEmpty(txtIdentificacion.Text) Then
            o = oPersonas.DevolverxID(identificacion)
            If Not (o.id = 0 Or o Is Nothing) Then
                nombres = o.Nombres & " " & o.Apellidos
            End If
        End If
        Return nombres
    End Function

End Class