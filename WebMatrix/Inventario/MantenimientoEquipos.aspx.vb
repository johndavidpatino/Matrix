Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports ClosedXML.Excel
Imports CoreProject.OP
Imports CoreProject.INV
Imports System.IO

Public Class MantenimientoEquipos
    Inherits System.Web.UI.Page


#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarActivosFijos()
            CargarActivosFijosAS()
            CargarPerifericos()
            EstadosArticulos()
            CargarSedes()

        End If

        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)

    End Sub

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub gvArticulos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvArticulos.PageIndexChanging
        gvArticulos.PageIndex = e.NewPageIndex
        CargarColumnas()
        gvArticulos.DataSource = obtenerRegistrosArticulos()
        gvArticulos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub gvMantenimientos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMantenimientos.PageIndexChanging
        gvMantenimientos.PageIndex = e.NewPageIndex
        CargarColumnasMt()
        gvMantenimientos.DataSource = ObtenerMantenimientos()
        gvMantenimientos.DataBind()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub gvArticulos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvArticulos.RowCommand

        Dim lstRegistroArticulos As List(Of INV_RegistroArticulos_Get_Result)

        If e.CommandName = "Abrir" Then

            lstRegistroArticulos = ObtenerRegistroArticulos(gvArticulos.DataKeys(e.CommandArgument).Value)

            lblIdAsignar.Text = lstRegistroArticulos(0).Id
            lblAsignar.Visible = True
            lblIdAsignar.Visible = True
            lblArticulo.Visible = True

            If lstRegistroArticulos(0).IdArticulo = 1 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo & " - " & lstRegistroArticulos(0).TipoComputador & "- Nombre Equipo:" & lstRegistroArticulos(0).NombreEquipo
            ElseIf lstRegistroArticulos(0).IdArticulo = 2 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo & " - " & lstRegistroArticulos(0).TipoServidor
            ElseIf lstRegistroArticulos(0).IdArticulo = 3 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo & " - " & lstRegistroArticulos(0).TipoPeriferico
            ElseIf lstRegistroArticulos(0).IdArticulo = 4 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo & " - Id Tablet: " & lstRegistroArticulos(0).IdTablet
            ElseIf lstRegistroArticulos(0).IdArticulo = 5 Or lstRegistroArticulos(0).IdArticulo = 6 Or lstRegistroArticulos(0).IdArticulo = 10 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo
            End If

            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)

        End If

    End Sub

    Private Sub gvMantenimientos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMantenimientos.RowCommand

        If e.CommandName = "Actualizar" Then

            Dim lstMantenimientos As List(Of INV_MantenimientoEquipos_Get_Result)
            lstMantenimientos = ObtenerMantenimientosxId(gvMantenimientos.DataKeys(e.CommandArgument)("Id"))

            hfTipoAccion.Value = 1
            hfIdMantenimiento.Value = gvMantenimientos.DataKeys(e.CommandArgument)("Id")
            lblIdAsignar.Text = lstMantenimientos(0).IdActivoFijo
            lblAsignar.Visible = True
            lblIdAsignar.Visible = True
            lblArticulo.Visible = True

            If lstMantenimientos(0).IdArticulo = 1 Then
                lblArticulo.Text = lstMantenimientos(0).Articulo & " - " & lstMantenimientos(0).TipoComputador & "- Nombre Equipo:" & lstMantenimientos(0).NombreEquipo
            ElseIf lstMantenimientos(0).IdArticulo = 2 Then
                lblArticulo.Text = lstMantenimientos(0).Articulo
            ElseIf lstMantenimientos(0).IdArticulo = 3 Then
                lblArticulo.Text = lstMantenimientos(0).Articulo & " - " & lstMantenimientos(0).TipoPeriferico
            ElseIf lstMantenimientos(0).IdArticulo = 4 Then
                lblArticulo.Text = lstMantenimientos(0).Articulo & " - Id Tablet: " & lstMantenimientos(0).IdTablet
            ElseIf lstMantenimientos(0).IdArticulo = 5 Or lstMantenimientos(0).IdArticulo = 6 Or lstMantenimientos(0).IdArticulo = 10 Then
                lblArticulo.Text = lstMantenimientos(0).Articulo
            End If


            txtFecha.Text = lstMantenimientos(0).Fecha

            ddlTipo.SelectedValue = lstMantenimientos(0).IdTipoMantenimiento

            If lstMantenimientos(0).IdUsuarioResponsable IsNot Nothing Then
                hfIdUsuario.Value = lstMantenimientos(0).IdUsuarioResponsable
            End If

            If lstMantenimientos(0).UsuarioResponsable IsNot Nothing Then
                txtUsuario.Text = lstMantenimientos(0).UsuarioResponsable
            End If

            txtObservacion.Text = lstMantenimientos(0).Observaciones

            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If
    End Sub

    Private Sub gvUsuarios_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvUsuarios.RowCommand
        If e.CommandName = "Seleccionar" Then
            txtUsuario.Text = Server.HtmlDecode(gvUsuarios.Rows(e.CommandArgument).Cells(0).Text)
            hfIdUsuario.Value = Me.gvUsuarios.DataKeys(CInt(e.CommandArgument))("Id")

            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If
    End Sub

#End Region


#Region "Grillas"

    Public Sub CargarGrid()
        Dim oListadoArticulos As New Inventario
        gvArticulos.DataSource = oListadoArticulos.obtenerRegistroArticulosxTodos
        gvArticulos.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub CargarGridPersonas()
        Dim o As New Personas
        Dim daContratistas As New Contratista
        Dim cedula As Int64? = Nothing
        Dim nombre As String = Nothing

        If IsNumeric(txtCedulaUsuario.Text) Then cedula = txtCedulaUsuario.Text
        If Not txtNombreUsuario.Text = "" Then nombre = txtNombreUsuario.Text

        Dim lstPersonas = o.ObtenerPersonasxCCNombre(cedula, nombre)
        Dim lstContratistas = daContratistas.ObtenerContratistas(cedula, nombre, True)

        Dim un = (From x In lstPersonas
                    Select Nombres = x.Nombres & " " & x.Apellidos, Id = x.id, Ciudad = x.Ciudad, Cargo = x.Cargo
                    ).Union(
                    From y In lstContratistas
                    Select Nombres = y.Nombre, Id = y.Identificacion, Ciudad = y.Ciudad, Cargo = "Contratista"
                    ).ToList


        Me.gvUsuarios.DataSource = un
        Me.gvUsuarios.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub Guardar()

        If Not (IsDate(txtFecha.Text)) Then
            ShowNotification("Escriba la fecha del Mantenimiento", ShowNotifications.ErrorNotification)
            txtFecha.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlTipo.SelectedValue = "-1" Then
            ShowNotification("Seleccione el Tipo de Mantenimiento", ShowNotifications.ErrorNotification)
            ddlTipo.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If String.IsNullOrEmpty(txtObservacion.Text) Then
            ShowNotification("Debe escribir las observaciones correspondientes", ShowNotifications.ErrorNotification)
            txtObservacion.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If


        Dim oGuardar As New Inventario

        Dim UsuarioRegistra As Int64?
        UsuarioRegistra = CType(Session("IDUsuario").ToString, Int64?)


        If hfTipoAccion.Value = 1 Then
            oGuardar.ActualizarMantenimientoEquipos(hfIdMantenimiento.Value, lblIdAsignar.Text, UsuarioRegistra, txtFecha.Text, ddlTipo.SelectedValue, hfIdUsuario.Value, txtObservacion.Text)
        Else
            oGuardar.GuardarMantenimientoEquipos(lblIdAsignar.Text, UsuarioRegistra, txtFecha.Text, ddlTipo.SelectedValue, hfIdUsuario.Value, txtObservacion.Text)
        End If

        ShowNotification("Registro Guardado correctamente", ShowNotifications.InfoNotification)
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)

        limpiar()

    End Sub


    Public Sub CargarColumnas()

        'Computador
        If ddlIdArticulo.SelectedValue = 1 Then
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = True  'TipoComputador
            gvArticulos.Columns(21).Visible = True  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = True  'Serial
            gvArticulos.Columns(30).Visible = True  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
        End If

        'Servidor
        If ddlIdArticulo.SelectedValue = 2 Then
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = True  'Serial
            gvArticulos.Columns(30).Visible = True  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = True  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
        End If

        'Dispositivos Periféricos
        If ddlIdArticulo.SelectedValue = 3 Then
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = True  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = True  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
        End If

        'Tablet
        If ddlIdArticulo.SelectedValue = 4 Then
            gvArticulos.Columns(5).Visible = False  'Fecha Registro
            gvArticulos.Columns(6).Visible = False  'Usuario Registra
            gvArticulos.Columns(14).Visible = False  'Valor
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(16).Visible = False  'Descripcion
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = False  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = False  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = False  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = True  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = True  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
        End If

        'Celular
        If ddlIdArticulo.SelectedValue = 5 Then
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = False  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = True  'Operador
            gvArticulos.Columns(42).Visible = True  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
        End If

        'SimCard
        If ddlIdArticulo.SelectedValue = 6 Then
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = False  'Marca
            gvArticulos.Columns(24).Visible = False  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = False  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = True  'Operador
            gvArticulos.Columns(42).Visible = True  'NumeroCelular
            gvArticulos.Columns(43).Visible = True  'CantidadMinutos
        End If

        'Diademas FI
        If ddlIdArticulo.SelectedValue = 10 Then
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = True  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
        End If
		Try
			gvArticulos.Columns(31).HeaderText = "Serial Windows"
		Catch ex As Exception

		End Try
	End Sub

    Public Sub CargarColumnasMt()

        'Computador
        If ddlArticuloMt.SelectedValue = 1 Then
            gvMantenimientos.Columns(3).Visible = True  'TipoComputador
            gvMantenimientos.Columns(4).Visible = False  'TipoPeriferico
            gvMantenimientos.Columns(5).Visible = True  'Marca
            gvMantenimientos.Columns(6).Visible = True  'Modelo
            gvMantenimientos.Columns(7).Visible = True  'IdFisico
            gvMantenimientos.Columns(8).Visible = True  'Serial
            gvMantenimientos.Columns(9).Visible = True  'NombreEquipo
            gvMantenimientos.Columns(10).Visible = False  'IdTablet
            gvMantenimientos.Columns(11).Visible = False  'Chip
            gvMantenimientos.Columns(12).Visible = False  'IMEI
            gvMantenimientos.Columns(13).Visible = False  'NumeroCelular
            gvMantenimientos.Columns(14).Visible = False  'CantidadMinutos

        End If

        'Servidor
        If ddlArticuloMt.SelectedValue = 2 Then
            gvMantenimientos.Columns(3).Visible = False  'TipoComputador
            gvMantenimientos.Columns(4).Visible = False  'TipoPeriferico
            gvMantenimientos.Columns(5).Visible = True  'Marca
            gvMantenimientos.Columns(6).Visible = True  'Modelo
            gvMantenimientos.Columns(7).Visible = False  'IdFisico
            gvMantenimientos.Columns(8).Visible = True  'Serial
            gvMantenimientos.Columns(9).Visible = False  'NombreEquipo
            gvMantenimientos.Columns(10).Visible = False  'IdTablet
            gvMantenimientos.Columns(11).Visible = False  'Chip
            gvMantenimientos.Columns(12).Visible = False  'IMEI
            gvMantenimientos.Columns(13).Visible = False  'NumeroCelular
            gvMantenimientos.Columns(14).Visible = False  'CantidadMinutos

        End If

        'Dispositivos Periféricos
        If ddlArticuloMt.SelectedValue = 3 Then
            gvMantenimientos.Columns(3).Visible = False  'TipoComputador
            gvMantenimientos.Columns(4).Visible = True  'TipoPeriferico
            gvMantenimientos.Columns(5).Visible = True  'Marca
            gvMantenimientos.Columns(6).Visible = True  'Modelo
            gvMantenimientos.Columns(7).Visible = False  'IdFisico
            gvMantenimientos.Columns(8).Visible = True  'Serial
            gvMantenimientos.Columns(9).Visible = False  'NombreEquipo
            gvMantenimientos.Columns(10).Visible = False  'IdTablet
            gvMantenimientos.Columns(11).Visible = False  'Chip
            gvMantenimientos.Columns(12).Visible = False  'IMEI
            gvMantenimientos.Columns(13).Visible = False  'NumeroCelular
            gvMantenimientos.Columns(14).Visible = False  'CantidadMinutos

        End If

        'Tablet
        If ddlArticuloMt.SelectedValue = 4 Then
            gvMantenimientos.Columns(3).Visible = False  'TipoComputador
            gvMantenimientos.Columns(4).Visible = False  'TipoPeriferico
            gvMantenimientos.Columns(5).Visible = False  'Marca
            gvMantenimientos.Columns(6).Visible = True  'Modelo
            gvMantenimientos.Columns(7).Visible = False  'IdFisico
            gvMantenimientos.Columns(8).Visible = False  'Serial
            gvMantenimientos.Columns(9).Visible = False  'NombreEquipo
            gvMantenimientos.Columns(10).Visible = True  'IdTablet
            gvMantenimientos.Columns(11).Visible = True  'Chip
            gvMantenimientos.Columns(12).Visible = True  'IMEI
            gvMantenimientos.Columns(13).Visible = False  'NumeroCelular
            gvMantenimientos.Columns(14).Visible = False  'CantidadMinutos

        End If

        'Celular
        If ddlArticuloMt.SelectedValue = 5 Then
            gvMantenimientos.Columns(3).Visible = False  'TipoComputador
            gvMantenimientos.Columns(4).Visible = False  'TipoPeriferico
            gvMantenimientos.Columns(5).Visible = True  'Marca
            gvMantenimientos.Columns(6).Visible = True  'Modelo
            gvMantenimientos.Columns(7).Visible = False  'IdFisico
            gvMantenimientos.Columns(8).Visible = False  'Serial
            gvMantenimientos.Columns(9).Visible = False  'NombreEquipo
            gvMantenimientos.Columns(10).Visible = False  'IdTablet
            gvMantenimientos.Columns(11).Visible = False  'Chip
            gvMantenimientos.Columns(12).Visible = False  'IMEI
            gvMantenimientos.Columns(13).Visible = True  'NumeroCelular
            gvMantenimientos.Columns(14).Visible = False  'CantidadMinutos

        End If

        'SimCard
        If ddlArticuloMt.SelectedValue = 6 Then
            gvMantenimientos.Columns(3).Visible = False  'TipoComputador
            gvMantenimientos.Columns(4).Visible = False  'TipoPeriferico
            gvMantenimientos.Columns(5).Visible = False  'Marca
            gvMantenimientos.Columns(6).Visible = False  'Modelo
            gvMantenimientos.Columns(7).Visible = False  'IdFisico
            gvMantenimientos.Columns(8).Visible = False  'Serial
            gvMantenimientos.Columns(9).Visible = False  'NombreEquipo
            gvMantenimientos.Columns(10).Visible = False  'IdTablet
            gvMantenimientos.Columns(11).Visible = False  'Chip
            gvMantenimientos.Columns(12).Visible = False  'IMEI
            gvMantenimientos.Columns(13).Visible = True  'NumeroCelular
            gvMantenimientos.Columns(14).Visible = True  'CantidadMinutos

        End If

        'Diademas FI
        If ddlArticuloMt.SelectedValue = 10 Then
            gvMantenimientos.Columns(3).Visible = False  'TipoComputador
            gvMantenimientos.Columns(4).Visible = False  'TipoPeriferico
            gvMantenimientos.Columns(5).Visible = True  'Marca
            gvMantenimientos.Columns(6).Visible = True  'Modelo
            gvMantenimientos.Columns(7).Visible = False  'IdFisico
            gvMantenimientos.Columns(8).Visible = True  'Serial
            gvMantenimientos.Columns(9).Visible = False  'NombreEquipo
            gvMantenimientos.Columns(10).Visible = False  'IdTablet
            gvMantenimientos.Columns(11).Visible = False  'Chip
            gvMantenimientos.Columns(12).Visible = False  'IMEI
            gvMantenimientos.Columns(13).Visible = False  'NumeroCelular
            gvMantenimientos.Columns(14).Visible = False  'CantidadMinutos

        End If

    End Sub

    Function ObtenerRegistroArticulos(ByVal Id As Int64?) As List(Of INV_RegistroArticulos_Get_Result)
        Dim lstRegistroArticulos As New List(Of INV_RegistroArticulos_Get_Result)
        Dim RecordArticulos As New Inventario
        lstRegistroArticulos = RecordArticulos.obtenerRegistroArticulosxId(Id)
        Return lstRegistroArticulos
    End Function

    Function ObtenerMantenimientosxId(ByVal Id As Int64?) As List(Of INV_MantenimientoEquipos_Get_Result)
        Dim lstMantenimientos As New List(Of INV_MantenimientoEquipos_Get_Result)
        Dim RecordAsignaciones As New Inventario
        lstMantenimientos = RecordAsignaciones.ObtenerMantenimientoEquiposxId(Id)
        Return lstMantenimientos
    End Function

    Function obtenerRegistrosArticulos() As List(Of INV_RegistroArticulos_Get_Result)
        Dim oBusqueda As New Inventario
        Dim Articulo As Int64? = Nothing
        Dim TipoComputador As Int64? = Nothing
        Dim PertenecePC As Int16? = Nothing
        Dim TipoPeriferico As Int64? = Nothing
        Dim Estado As Int64? = Nothing
        Dim Sede As Int64? = Nothing
        Dim IdUsuarioAsignado As Int64? = Nothing
        Dim UsuarioAsignado As String = Nothing
        Dim Asignado As Boolean? = Nothing
        Dim TodosCampos As String = Nothing

        If Not ddlIdArticulo.SelectedValue = -1 Then Articulo = ddlIdArticulo.SelectedValue
        If Not ddlIdTipoComputador.SelectedValue = -1 Then TipoComputador = ddlIdTipoComputador.SelectedValue
        If Not ddlIdPertenecePC.SelectedValue = -1 Then PertenecePC = ddlIdPertenecePC.SelectedValue
        If Not ddlIdPeriferico.SelectedValue = -1 Then TipoPeriferico = ddlIdPeriferico.SelectedValue
        If Not ddlIdEstado.SelectedValue = -1 Then Estado = ddlIdEstado.SelectedValue
        If Not ddlIdSede.SelectedValue = -1 Then Sede = ddlIdSede.SelectedValue
        If Not ddlIdAsignado.SelectedValue = -1 Then
            If ddlIdAsignado.SelectedValue = 1 Then Asignado = True
            If ddlIdAsignado.SelectedValue = 0 Then Asignado = False
        End If

        If IsNumeric(txtIdUsuario.Text) Then IdUsuarioAsignado = txtIdUsuario.Text
        If Not txtNomUsuario.Text = "" Then UsuarioAsignado = txtNomUsuario.Text
        If Not txtTodosCampos.Text = "" Then TodosCampos = txtTodosCampos.Text

		Return oBusqueda.obtenerRegistroArticulos(Nothing, 1, Articulo, TipoComputador, PertenecePC, TipoPeriferico, Nothing, Estado, Sede, IdUsuarioAsignado, UsuarioAsignado, Asignado, Nothing, TodosCampos)
	End Function


    Function ObtenerMantenimientos() As List(Of INV_MantenimientoEquipos_Get_Result)
        Dim oBusqueda As New Inventario

        Dim Articulo As Int64? = Nothing
        Dim TipoMantenimiento As Int32? = Nothing
        Dim IdUsuarioResponsable As Int64? = Nothing
        Dim UsuarioResponsable As String = Nothing

        If Not ddlArticuloMt.SelectedValue = -1 Then Articulo = ddlArticuloMt.SelectedValue
        If Not ddlTipoMt.SelectedValue = -1 Then TipoMantenimiento = ddlTipoMt.SelectedValue
        If IsNumeric(txtIdUsuarioMt.Text) Then IdUsuarioResponsable = txtIdUsuarioMt.Text
        If Not txtUsuarioMt.Text = "" Then UsuarioResponsable = txtUsuarioMt.Text

        Return oBusqueda.ObtenerMantenimientoEquipos(Nothing, Nothing, Articulo, TipoMantenimiento, IdUsuarioResponsable, UsuarioResponsable)
    End Function

#End Region

#Region "DDL"

    Sub CargarActivosFijos()
        Dim oArticulos As New Inventario
        Dim oUsuario As New US.UsuariosUnidades
        Dim Usuario = oUsuario.obtenerUnidadesXUsuario(Session("IDUsuario").ToString, True, Nothing)

        For Each oGrupoUnidad In Usuario
            Dim GrupoUnidad As Int32? = oGrupoUnidad.GrupoUnidadId
            Dim Busqueda = oArticulos.obtenerArticulosxTipoArticulo(1, GrupoUnidad)
            If Busqueda.Count > 0 Then
                Me.ddlIdArticulo.DataSource = oArticulos.obtenerArticulosxTipoArticulo(1, GrupoUnidad)
                Exit For
            Else
                Me.ddlIdArticulo.Items.Clear()
            End If
        Next

        Me.ddlIdArticulo.DataValueField = "Id"
        Me.ddlIdArticulo.DataTextField = "Articulo"
        Me.ddlIdArticulo.DataBind()
        Me.ddlIdArticulo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarActivosFijosAS()
        Dim oArticulos As New Inventario
        Dim oUsuario As New US.UsuariosUnidades
        Dim Usuario = oUsuario.obtenerUnidadesXUsuario(Session("IDUsuario").ToString, True, Nothing)

        For Each oGrupoUnidad In Usuario
            Dim GrupoUnidad As Int32? = oGrupoUnidad.GrupoUnidadId
            Dim Busqueda = oArticulos.obtenerArticulosxTipoArticulo(1, GrupoUnidad)
            If Busqueda.Count > 0 Then
                Me.ddlArticuloMt.DataSource = oArticulos.obtenerArticulosxTipoArticulo(1, GrupoUnidad)
                Exit For
            Else
                Me.ddlArticuloMt.Items.Clear()
            End If
        Next

        Me.ddlArticuloMt.DataValueField = "Id"
        Me.ddlArticuloMt.DataTextField = "Articulo"
        Me.ddlArticuloMt.DataBind()
        Me.ddlArticuloMt.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarPerifericos()
        Dim oPerifericos As New Inventario
        Me.ddlIdPeriferico.DataSource = oPerifericos.obtenerDispositivosPerifericos
        Me.ddlIdPeriferico.DataValueField = "Id"
        Me.ddlIdPeriferico.DataTextField = "Periferico"
        Me.ddlIdPeriferico.DataBind()
        Me.ddlIdPeriferico.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub


    Sub EstadosArticulos()
        Dim oEstados As New Inventario
        Me.ddlIdEstado.DataSource = oEstados.obtenerEstadosArticulos
        Me.ddlIdEstado.DataValueField = "Id"
        Me.ddlIdEstado.DataTextField = "Estado"
        Me.ddlIdEstado.DataBind()
        Me.ddlIdEstado.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarSedes()
        Dim oSedes As New Inventario
        Me.ddlIdSede.DataSource = oSedes.obtenerSedes
        Me.ddlIdSede.DataValueField = "Id"
        Me.ddlIdSede.DataTextField = "Sede"
        Me.ddlIdSede.DataBind()
        Me.ddlIdSede.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

#End Region

#Region "Formulario"

    Sub limpiar()
        Me.txtFecha.Text = ""
        Me.ddlTipo.ClearSelection()
        Me.txtUsuario.Text = ""
        Me.txtCedulaUsuario.Text = ""
        Me.txtNombreUsuario.Text = ""
        hfIdUsuario.Value = "0"
        hfTipoAccion.Value = "0"
        hfIdMantenimiento.Value = "0"
        Me.txtObservacion.Text = ""

        lblIdAsignar.Text = ""
        lblArticulo.Text = ""
        lblIdAsignar.Visible = False
        lblAsignar.Visible = False
        lblArticulo.Visible = False

        ddlIdArticulo.ClearSelection()
        ddlIdTipoComputador.ClearSelection()
        ddlIdPertenecePC.ClearSelection()
        ddlIdPeriferico.ClearSelection()
        ddlIdEstado.ClearSelection()
        ddlIdSede.ClearSelection()
        ddlIdAsignado.ClearSelection()
        txtIdUsuario.Text = ""
        txtNomUsuario.Text = ""
        gvArticulos.DataSource = Nothing
        gvArticulos.DataBind()

        ddlArticuloMt.ClearSelection()
        ddlTipoMt.ClearSelection()
        txtIdUsuarioMt.Text = ""
        txtUsuarioMt.Text = ""
        gvMantenimientos.DataSource = Nothing
        gvMantenimientos.DataBind()

    End Sub

#End Region


    Protected Sub btnSearchUsuario_Click(sender As Object, e As EventArgs) Handles btnSearchUsuario.Click
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Guardar()
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        limpiar()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarColumnas()
        gvArticulos.DataSource = obtenerRegistrosArticulos()
        gvArticulos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnBuscarMt_Click(sender As Object, e As EventArgs) Handles btnBuscarMt.Click
        CargarColumnasMt()
        gvMantenimientos.DataSource = ObtenerMantenimientos()
        gvMantenimientos.DataBind()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnBuscarUsuario_Click(sender As Object, e As EventArgs) Handles btnBuscarUsuario.Click
        CargarGridPersonas()
        UPanelUsuarios.Update()
    End Sub

    Protected Sub ddlIdArticulo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlIdArticulo.SelectedIndexChanged
        If ddlIdArticulo.SelectedValue = 1 Then
            lblIdTipoComputador.Visible = True
            ddlIdTipoComputador.Visible = True
            lblIdPertenecePC.Visible = True
            ddlIdPertenecePC.Visible = True
        Else
            lblIdTipoComputador.Visible = False
            ddlIdTipoComputador.Visible = False
            ddlIdTipoComputador.ClearSelection()
            lblIdPertenecePC.Visible = False
            ddlIdPertenecePC.Visible = False
            ddlIdPertenecePC.ClearSelection()
        End If

        If ddlIdArticulo.SelectedValue = 3 Then
            lblIdPeriferico.Visible = True
            ddlIdPeriferico.Visible = True
        Else
            lblIdPeriferico.Visible = False
            ddlIdPeriferico.Visible = False
            ddlIdPeriferico.ClearSelection()
        End If

        ActivateAccordion(0, EffectActivateAccordion.NoEffect)

    End Sub


    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        CargarColumnasMt()
        gvMantenimientos.DataSource = ObtenerMantenimientos()
        gvMantenimientos.DataBind()
        gvMantenimientos.Visible = True
        'Actualiza los datos del gridview
        gvMantenimientos.AllowPaging = False
        gvMantenimientos.DataBind()

        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        gvMantenimientos.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvMantenimientos)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=Listado_Mantenimientos.xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvMantenimientos.Visible = False
    End Sub

    Private Sub _AlmacenamientoDisco_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(141, UsuarioID) = False AndAlso permisos.VerificarPermisoUsuario(142, UsuarioID) = False Then
            Response.Redirect("../Inventario/RegistroArticulos.aspx")
        End If
    End Sub



End Class