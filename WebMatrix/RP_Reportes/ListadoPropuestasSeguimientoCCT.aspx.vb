Imports WebMatrix.Util
Imports CoreProject

Public Class ListadoPropuestasSeguimientoCCT
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarUnidades()
            CargarGerencias()
            CargarEstadoPropuesta()
            'Dim permisos As New Datos.ClsPermisosUsuarios
            'Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            'If permisos.VerificarPermisoUsuario(17, UsuarioID) = False Then
            '    Response.Redirect("../Home.aspx")
            'End If
            'Me.pnlActualizacion.Visible = False
        End If
    End Sub


#End Region

#Region "Metodos"

    Sub CargarUnidades()
        Dim oUnidades As New US.Unidades
        ddlUnidades.DataSource = oUnidades.ObtenerUnidadesxUsuario(Session("IDUsuario").ToString)
        ddlUnidades.DataValueField = "id"
        ddlUnidades.DataTextField = "Unidad"
        ddlUnidades.DataBind()
        'ddlUnidades.Items.Insert(0, New ListItem With {.Text = "--Ver todo--", .Value = -1})
        CargarGerentesCuentas()
    End Sub

    Sub CargarGerencias()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGerencias.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(2)
        ddlGerencias.DataValueField = "id"
        ddlGerencias.DataTextField = "GrupoUnidad"
        ddlGerencias.DataBind()
        ddlGerencias.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub

    Sub CargarGerentesCuentas()

        If ddlUnidades.SelectedValue = "-1" Or ddlUnidades.SelectedValue = "" Then
            ddlGerenteCuentas.Items.Clear()
        Else
            Dim o As New Reportes.RP_GerOpe
            ddlGerenteCuentas.DataSource = o.ObtenerUsuariosXUnidadXRol(ddlUnidades.SelectedValue, ListaRoles.GerenteCuentas)
            ddlGerenteCuentas.DataValueField = "id"
            ddlGerenteCuentas.DataTextField = "NombreCompleto"
            ddlGerenteCuentas.DataBind()
            ddlGerenteCuentas.Items.Insert(0, New ListItem With {.Text = "--Ver todo--", .Value = -1})
        End If
    End Sub

    Public Sub CargarEstadoPropuesta()
        Try
            Dim oAuxiliares As New Auxiliares
            Dim listaEstados = (From lestado In oAuxiliares.DevolverEstadoPropuesta()
                                Select Id = lestado.id, Estado = lestado.Estado).OrderBy(Function(e) e.Id).ToList()
            ddlEstado.DataSource = listaEstados
            ddlEstado.DataValueField = "Id"
            ddlEstado.DataTextField = "Estado"
            ddlEstado.DataBind()
            ddlEstado.Items.Insert(0, New ListItem With {.Text = "--Ver todo--", .Value = -1})
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub CargarPropuestas(ByVal estado As Integer?, probabilidad As Integer?, unidad As Integer?, grupounidad As Long?, gerentecuentas As Long?, gerenciaop As Long?)
        Dim o As New Reportes.RP_GerOpe
        Me.gvPropuestas.DataSource = o.ObtenerListadoPropuestasSeguimiento(gerenciaop, estado, probabilidad, unidad, grupounidad, gerentecuentas)
        Me.gvPropuestas.DataBind()
    End Sub
#End Region

    Sub CargarDeNuevo()
        Dim estado As Integer?, probabilidad As Integer?, unidad As Integer?, grupounidad As Long?, gerentecuentas As Long?, gerenciaop As Long?
        If ddlEstado.SelectedValue = "-1" Or ddlEstado.SelectedValue = "" Then
            estado = Nothing
        Else
            estado = ddlEstado.SelectedValue
        End If
        If ddlProbabilidad.SelectedValue = "-1" Or ddlProbabilidad.SelectedValue = "" Then
            probabilidad = Nothing
        Else
            probabilidad = ddlProbabilidad.SelectedValue
        End If
   
        If ddlUnidades.SelectedValue = "-1" Or ddlUnidades.SelectedValue = "" Then
            grupounidad = Nothing
        Else
            grupounidad = ddlUnidades.SelectedValue
        End If
        If ddlGerenteCuentas.SelectedValue = "-1" Or ddlGerenteCuentas.SelectedValue = "" Then
            gerentecuentas = Nothing
        Else
            gerentecuentas = ddlGerenteCuentas.SelectedValue
        End If
        If ddlGerencias.SelectedValue = "-1" Or ddlGerencias.SelectedValue = "" Then
            gerenciaop = Nothing
        Else
            gerenciaop = ddlGerencias.SelectedValue
        End If

        CargarPropuestas(estado, probabilidad, unidad, grupounidad, gerentecuentas, gerenciaop)
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim estado As Integer?, probabilidad As Integer?, unidad As Integer?, grupounidad As Long?, gerentecuentas As Long?, gerenciaop As Long?
        If ddlEstado.SelectedValue = "-1" Or ddlEstado.SelectedValue = "" Then
            estado = Nothing
        Else
            estado = ddlEstado.SelectedValue
        End If
        If ddlProbabilidad.SelectedValue = "-1" Or ddlProbabilidad.SelectedValue = "" Then
            probabilidad = Nothing
        Else
            probabilidad = ddlProbabilidad.SelectedValue
        End If
        If ddlUnidades.SelectedValue = "-1" Or ddlUnidades.SelectedValue = "" Then
            unidad = Nothing
        Else
            grupounidad = ddlUnidades.SelectedValue
        End If
        If ddlGerenteCuentas.SelectedValue = "-1" Or ddlGerenteCuentas.SelectedValue = "" Then
            gerentecuentas = Nothing
        Else
            gerentecuentas = ddlGerenteCuentas.SelectedValue
        End If
        If ddlGerencias.SelectedValue = "-1" Or ddlGerencias.SelectedValue = "" Then
            gerenciaop = Nothing
        Else
            gerenciaop = ddlGerencias.SelectedValue
        End If

        CargarPropuestas(estado, probabilidad, unidad, grupounidad, gerentecuentas, gerenciaop)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub


    Protected Sub ddlUnidades_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUnidades.SelectedIndexChanged
        CargarGerentesCuentas()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub gvPropuestas_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPropuestas.RowCommand
        If e.CommandName = "Actualizar" Then
            Me.hfPropuesta.Value = Me.gvPropuestas.DataKeys(CInt(e.CommandArgument))("Id")
            Dim oPropuesta As New Propuesta
            Dim info = oPropuesta.DevolverxID(hfPropuesta.Value)
            txtFechaInicioCampo.Text = info.FechaInicioCampo
            ddProbabilidadUpdate.SelectedValue = info.ProbabilidadId
            upGerenteAsignar.Update()
            btnUpdate.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim o As New Reportes.RP_GerOpe
        If Not (IsDate(txtFechaInicioCampo.Text)) Then Exit Sub
        o.ActualizarPropuestaSeguimiento(hfPropuesta.Value, txtFechaInicioCampo.Text, ddProbabilidadUpdate.SelectedValue)
        CargarDeNuevo()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub ListadoPropuestasSeguimientoCCT_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(128, UsuarioID) = False Then
            Response.Redirect("../home.aspx")
        End If
    End Sub
End Class