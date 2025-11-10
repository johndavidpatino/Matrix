Imports CoreProject

Public Class QuejasReclamosProveedores
    Inherits System.Web.UI.Page

#Region "Enumerados"
    Enum TipoEstudio
        JobBookExterno = 1
        JobBookInterno = 2
        CentroCosto = 3
    End Enum
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim vista = ""
            If Request.QueryString("vista") IsNot Nothing Then
                vista = Request.QueryString("vista").ToString
            End If
            If vista = "nuevo" Then
                pnlNuevo.Visible = True
                btnListado.Visible = True
                Limpiar()
                pnlListado.Visible = False
            Else
                cargarQRP()
            End If

            cargarTipos()
            cargarBusquedaTipo()
        End If
    End Sub

    Private Sub btnBuscarProveedor_Click(sender As Object, e As EventArgs) Handles btnBuscarProveedor.Click
        Dim o As New FI.Ordenes
        Dim identificacion As Int64? = Nothing
        Dim proveedor As String = Nothing
        If IsNumeric(Me.txtNitProveedor.Text) Then identificacion = Me.txtNitProveedor.Text
        If Not Me.txtNombreProveedor.Text = "" Then proveedor = Me.txtNombreProveedor.Text
        Me.gvProveedores.DataSource = o.ObtenerContratistas(identificacion, proveedor, True)
        Me.gvProveedores.DataBind()
        Me.UPanelProveedores.Update()
    End Sub

    Private Sub gvProveedores_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvProveedores.RowCommand
        If e.CommandName = "Seleccionar" Then
            txtProveedor.Text = Server.HtmlDecode(gvProveedores.Rows(e.CommandArgument).Cells(0).Text)
            Dim nombreProveedor = Server.HtmlDecode(gvProveedores.Rows(e.CommandArgument).Cells(1).Text)
            lblNombreProveedor.InnerText = nombreProveedor
            upProveedor.Update()
        End If
    End Sub

    Private Sub gvCC_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCC.RowCommand
        If e.CommandName = "Seleccionar" Then
            txtCC.Text = Server.HtmlDecode(gvCC.Rows(e.CommandArgument).Cells(0).Text)
            txtNombreCC.InnerText = Server.HtmlDecode(gvCC.Rows(e.CommandArgument).Cells(1).Text)
        End If
        upCentroCosto.Update()
    End Sub
    Private Sub gvQuejasProveedores_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvQuejasProveedores.RowCommand
        If e.CommandName = "Seleccionar" Then
            pnlNuevo.Visible = True
            btnListado.Visible = True
            Limpiar()
            Dim id = gvQuejasProveedores.DataKeys(CInt(e.CommandArgument))("id")
            hfId.Value = id
            Dim o = New FI.Formatos
            Dim el = o.ObtenerQuejasReclamosProveedores(id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            LlenarDatos(el(0))
            pnlListado.Visible = False
        End If
    End Sub

    Private Sub btnBuscarCC_Click(sender As Object, e As EventArgs) Handles btnBuscarCC.Click
        cargarGrillaBusquedaCentroCostoXTipoYValor(ddlTipo.SelectedValue, txtCCBusqueda.Text)
    End Sub

    Private Sub ddlTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipo.SelectedIndexChanged
        cargarBusquedaTipo()
        txtNombreJobBusqueda.Text = ""
        txtJobBusqueda.Text = ""
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Validar()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        pnlNuevo.Visible = True
        btnListado.Visible = True
        Limpiar()
        pnlListado.Visible = False
    End Sub

    Private Sub btnListado_Click(sender As Object, e As EventArgs) Handles btnListado.Click
        cargarQRP()
        pnlNuevo.Visible = False
        Me.btnListado.Visible = False
        Limpiar()
        pnlListado.Visible = True
    End Sub
    Private Sub txtJobBusqueda_TextChanged(sender As Object, e As EventArgs) Handles txtJobBusqueda.TextChanged
        Dim daProyecto As New Proyecto
        Dim daTrabajo As New Trabajo
        Dim oP As PY_Proyectos_Get_Result
        Dim oT As PY_Trabajos_Get_Result
        Dim oT1 As PY_Trabajo0
        Dim daU As New US.Unidades
        Dim idUnidad As Integer = 0
        Dim idGrupoUnidad As Integer = 0
        Dim daO As New FI.Ordenes
        Select Case ddlTipo.SelectedValue
            Case 1
                oP = daProyecto.obtenerXJobBook(txtJobBusqueda.Text)
                If Not ((oP Is Nothing)) Then
                    txtNombreJobBusqueda.Text = oP.Nombre
                    txtNombreJobBusqueda.ReadOnly = True
                Else
                    txtNombreJobBusqueda.ReadOnly = False
                    txtNombreJobBusqueda.Text = ""
                    txtNombreJobBusqueda.Focus()
                End If
            Case 2
                oT1 = daTrabajo.ObtenerTrabajoXJob(txtJobBusqueda.Text)
                If Not ((oT1 Is Nothing)) Then
                    txtNombreJobBusqueda.Text = oT1.NombreTrabajo
                    txtNombreJobBusqueda.ReadOnly = True
                Else
                    txtNombreJobBusqueda.ReadOnly = False
                    txtNombreJobBusqueda.Text = ""
                    txtNombreJobBusqueda.Focus()
                End If
        End Select
    End Sub

#Region "Cargues"
    Sub cargarQRP()
        Dim usuario = Int64.Parse(Session("IDUsuario").ToString())
        Dim daF As New FI.Formatos
        Dim datasource = daF.ObtenerQuejasReclamosProveedores(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, usuario)

        gvQuejasProveedores.DataSource = datasource
        gvQuejasProveedores.DataBind()
    End Sub

    Sub cargarTipos()
        ddlTipo.Items.Clear()
        ddlTipo.Items.Insert(0, New WebControls.ListItem("JBE - JobBookExterno", "1"))
        ddlTipo.Items.Insert(0, New WebControls.ListItem("JBI - JobBookInterno", "2"))
        ddlTipo.Items.Insert(0, New WebControls.ListItem("Centro de Costo", "3"))
        ddlTipo.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))
    End Sub

    Sub cargarCentroCostoDropDown(ByVal ddl As DropDownList, ByVal tipo As Byte)
        ddl.DataSource = obtenerCentroCosto(tipo, Nothing)
        ddl.DataValueField = "id"
        ddl.DataTextField = "nombre"
        ddl.DataBind()
        ddl.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub cargarGrillaBusquedaCentroCostoXTipoYValor(ByVal tipo As Byte, ByVal valorBusqueda As String)
        Me.gvCC.DataSource = obtenerCentroCosto(tipo, valorBusqueda)
        Me.gvCC.DataBind()
    End Sub

    Sub cargarBusquedaTipo()
        If ddlTipo.SelectedValue = TipoEstudio.JobBookExterno Or ddlTipo.SelectedValue = TipoEstudio.JobBookInterno Then
            divJob.Visible = True
            divCentroCostos.Visible = False
        ElseIf ddlTipo.SelectedValue = 3 Then
            divJob.Visible = False
            divCentroCostos.Visible = True
        Else
            divJob.Visible = False
            divCentroCostos.Visible = False
        End If
    End Sub
#End Region

#Region "Metodos"
    Function obtenerCentroCosto(ByVal tipo As Byte, ByVal valorBusqueda As String) As List(Of FI_JBE_JBI_CC_Get_Result)
        Dim o As New FI.Ordenes
        Return o.ObtenerJBE_JBI_CC(tipo, valorBusqueda)
    End Function

    Sub Validar()
        If Trim(txtProveedor.Text) = "" Then
            AlertJS("Debe indicar el proveedor")
            btnBuscarProveedor.Focus()
            Exit Sub
        End If

        If ddlTipo.SelectedValue = -1 Then
            AlertJS("Debe seleccionar el Tipo de Estudio")
            ddlTipo.Focus()
            Exit Sub
        Else
            If ddlTipo.SelectedValue = TipoEstudio.JobBookExterno Or ddlTipo.SelectedValue = TipoEstudio.JobBookInterno Then
                If Trim(txtJobBusqueda.Text) = "" Then
                    AlertJS("Debe indicar Código del JBIJBE")
                    txtJobBusqueda.Focus()
                    Exit Sub
                End If
                If Trim(txtJobBusqueda.Text) = "" Then
                    AlertJS("Debe indicar Nombre del JBIJBE")
                    txtJobBusqueda.Focus()
                    Exit Sub
                End If
            ElseIf ddlTipo.SelectedValue = TipoEstudio.CentroCosto Then
                If Trim(txtCC.Text) = "" Then
                    AlertJS("Debe indicar Centro de Costo")
                    btnBuscarCC.Focus()
                    Exit Sub
                End If
            End If
        End If

        If Trim(txtDescripcionQueja.Text) = "" Then
            AlertJS("Debe escribir la descripción de la Queja o Reclamo")
            txtDescripcionQueja.Focus()
            Exit Sub
        End If

        If Trim(txtAccionesRequeridas.Text) = "" Then
            AlertJS("Debe escribir las acciones requeridas para la mejora")
            txtAccionesRequeridas.Focus()
            Exit Sub
        End If

        If ddlSatisfaccion.SelectedValue = -1 Then
            AlertJS("Debe seleccionar su satisfacción con las acciones implementadas")
            ddlSatisfaccion.Focus()
            Exit Sub
        End If

        If Not (IsDate(txtRespuestaFechaImp.Text)) Then
            AlertJS("Escriba la fecha de la implementación")
            txtRespuestaFechaImp.Focus()
            Exit Sub
        End If

        Dim hrefGuardar = Guardar()

        If hrefGuardar Then
            AlertJS("Registro guardado")
            Response.Redirect("QuejasReclamosProveedores.aspx")
        End If

    End Sub

    Function Guardar() As Boolean
        Dim e As New FI_QuejasReclamosProveedores
        Dim o As New FI.Formatos

        Try
            If hfId.Value <> 0 Then
                e.Id = hfId.Value
            End If
            e.Proveedor = txtProveedor.Text
            e.Contacto = txtContacto.Text
            e.TipoEstudio = ddlTipo.SelectedValue

            If ddlTipo.SelectedValue = TipoEstudio.JobBookExterno Or ddlTipo.SelectedValue = TipoEstudio.JobBookInterno Then
                e.NoEstudio = txtJobBusqueda.Text
            ElseIf ddlTipo.SelectedValue = TipoEstudio.CentroCosto Then
                e.NoEstudio = txtCC.Text
            Else
                AlertJS("No se tienen datos del estudio")
                Exit Function
            End If

            e.NombreGerente = txtGerenteProyecto.Text
            e.Descripcion = txtDescripcionQueja.Text
            e.Acciones = txtAccionesRequeridas.Text
            e.CausaRaiz = txtRespuestaCausaRaiz.Text
            e.PlanAccion = txtRespuestaPlanAccion.Text
            e.Responsable = txtRespuestaResponsable.Text
            Dim RespuestaFechaImp As Date? = Nothing
            If IsDate(txtRespuestaFechaImp.Text) Then RespuestaFechaImp = CDate(txtRespuestaFechaImp.Text)
            e.FechaImplementacion = RespuestaFechaImp
            e.Satisfaccion = ddlSatisfaccion.SelectedValue
            e.Observaciones = txtObservaciones.Text
            e.Fecha = CDate(DateTime.Now.ToString)
            e.Usuario = Int64.Parse(Session("IDUsuario").ToString())

            o.GuardarQuejasReclamosProveedores(e)
            hfId.Value = e.Id
            'Limpiar()
            Return True
        Catch ex As Exception
            AlertJS("No se pudo Guardar la información. Revisar los campos.")
            Return False
        End Try
    End Function

    Sub Limpiar()
        txtProveedor.Text = ""
        lblNombreProveedor.InnerText = ""
        txtContacto.Text = ""
        cargarTipos()
        txtCC.Text = ""
        txtNombreCC.InnerText = ""
        txtNombreCCBusqueda.InnerText = ""
        txtJobBusqueda.Text = ""
        txtNombreJobBusqueda.Text = ""
        txtGerenteProyecto.Text = ""
        txtDescripcionQueja.Text = ""
        txtAccionesRequeridas.Text = ""
        txtRespuestaCausaRaiz.Text = ""
        txtRespuestaPlanAccion.Text = ""
        txtRespuestaResponsable.Text = ""
        txtRespuestaFechaImp.Text = ""
        ddlSatisfaccion.SelectedIndex = -1
        txtObservaciones.Text = ""

        txtNitProveedor.Text = ""
        txtNombreProveedor.Text = ""
        gvProveedores.DataSourceID = Nothing
        gvProveedores.DataSource = Nothing
        gvProveedores.DataBind()

        txtCCBusqueda.Text = ""
        gvCC.DataSourceID = Nothing
        gvCC.DataSource = Nothing
        gvCC.DataBind()
    End Sub

    Sub LlenarDatos(ByVal e As FI_QuejasReclamosProveedores_Get_Result)
        txtProveedor.Text = e.Proveedor
        lblNombreProveedor.InnerText = e.NombreProveedor
        txtContacto.Text = e.Contacto
        ddlTipo.SelectedValue = e.TipoEstudio
        cargarBusquedaTipo()
        If e.TipoEstudio = TipoEstudio.JobBookExterno Then
            txtJobBusqueda.Text = e.NoEstudio
            txtNombreJobBusqueda.Text = e.NomProyecto
            txtNombreJobBusqueda.ReadOnly = True
        ElseIf e.TipoEstudio = TipoEstudio.JobBookInterno Then
            txtJobBusqueda.Text = e.NoEstudio
            txtNombreJobBusqueda.Text = e.NombreTrabajo
            txtNombreJobBusqueda.ReadOnly = True
        ElseIf e.TipoEstudio = TipoEstudio.CentroCosto Then
            txtCC.Text = e.NoEstudio
            txtNombreCC.InnerText = e.CentroDeCosto
        Else
            txtCC.Text = ""
            txtNombreCC.InnerText = ""
            txtNombreJobBusqueda.Text = ""
        End If
        txtGerenteProyecto.Text = e.NombreGerente
        txtDescripcionQueja.Text = e.Descripcion
        txtAccionesRequeridas.Text = e.Acciones
        txtRespuestaCausaRaiz.Text = e.CausaRaiz
        txtRespuestaPlanAccion.Text = e.PlanAccion
        txtRespuestaResponsable.Text = e.Responsable
        txtRespuestaFechaImp.Text = e.FechaImplementacion
        ddlSatisfaccion.SelectedValue = e.Satisfaccion
        txtObservaciones.Text = e.Observaciones
    End Sub

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

#End Region

End Class