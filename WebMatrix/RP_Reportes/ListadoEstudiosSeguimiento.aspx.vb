Imports WebMatrix.Util
Imports CoreProject

Public Class ListadoEstudiosSeguimiento
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarGruposUnidad()
            CargarGerencias()
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
    Sub CargarGruposUnidad()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGrupoUnidades.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(1)
        ddlGrupoUnidades.DataValueField = "id"
        ddlGrupoUnidades.DataTextField = "GrupoUnidad"
        ddlGrupoUnidades.DataBind()
        ddlGrupoUnidades.Items.Insert(0, New ListItem With {.Text = "--Ver todo--", .Value = -1})
    End Sub

    Sub CargarUnidades()
        If ddlGrupoUnidades.SelectedValue = "-1" Or ddlGrupoUnidades.SelectedValue = "" Then
            ddlUnidades.Items.Clear()
        Else
            Dim oUnidades As New US.Unidades
            ddlUnidades.DataSource = oUnidades.ObtenerUnidadCombo(ddlGrupoUnidades.SelectedValue)
            ddlUnidades.DataValueField = "id"
            ddlUnidades.DataTextField = "Unidad"
            ddlUnidades.DataBind()
            ddlUnidades.Items.Insert(0, New ListItem With {.Text = "--Ver todo--", .Value = -1})
        End If
        CargarGerentesCuentas()
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
    Sub CargarGerencias()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGerencias.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(2)
        ddlGerencias.DataValueField = "id"
        ddlGerencias.DataTextField = "GrupoUnidad"
        ddlGerencias.DataBind()
        ddlGerencias.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = -1})
    End Sub
    Sub CargarEstudios(unidad As Integer?, grupounidad As Long?, gerentecuentas As Long?, gerenciaop As Long?)
        Dim o As New Reportes.RP_GerOpe
        Me.gvEstudios.DataSource = o.ObtenerListadoEstudiosSeguimiento(unidad, grupounidad, gerentecuentas, gerenciaop)
        Me.gvEstudios.DataBind()
    End Sub
#End Region

    Sub CargarDeNuevo()
        Dim unidad As Integer?, grupounidad As Long?, gerentecuentas As Long?, gerenciaop As Long?

        If ddlGrupoUnidades.SelectedValue = "-1" Or ddlGrupoUnidades.SelectedValue = "" Then
            unidad = Nothing
        Else
            unidad = ddlGrupoUnidades.SelectedValue
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

        CargarEstudios(unidad, grupounidad, gerentecuentas, gerenciaop)
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim unidad As Integer?, grupounidad As Long?, gerentecuentas As Long?, gerenciaop As Long?
        
        If ddlGrupoUnidades.SelectedValue = "-1" Or ddlGrupoUnidades.SelectedValue = "" Then
            unidad = Nothing
        Else
            unidad = ddlGrupoUnidades.SelectedValue
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

        CargarEstudios(unidad, grupounidad, gerentecuentas, gerenciaop)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlGrupoUnidades_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGrupoUnidades.SelectedIndexChanged
        CargarUnidades()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub ddlUnidades_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUnidades.SelectedIndexChanged
        CargarGerentesCuentas()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub gvPropuestas_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEstudios.RowCommand
        If e.CommandName = "Actualizar" Then
            Me.hfEstudio.Value = Me.gvEstudios.DataKeys(CInt(e.CommandArgument))("Id")
            Me.hfEstudio2.Value = hfEstudio.Value
            Dim oPropuesta As New Propuesta
            Dim info = oPropuesta.DevolverxID(hfEstudio.Value)
            Try
                txtFechaInicioCampo.Text = info.FechaInicioCampo
            Catch ex As Exception

            End Try
            upGerenteAsignar.Update()
            btnUpdate.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        Dim o As New Reportes.RP_GerOpe
        If Not (IsDate(txtFechaInicioCampo.Text)) Then Exit Sub
        o.ActualizarEstudioSeguimiento(hfEstudio2.Value, txtFechaInicioCampo.Text)
        CargarDeNuevo()

        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

End Class