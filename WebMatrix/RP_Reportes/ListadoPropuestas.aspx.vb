Imports WebMatrix.Util
Imports CoreProject

Public Class ListadoPropuestas_2
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarGruposUnidad()
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(17, UsuarioID) = False Then
                Response.Redirect("../Home.aspx")
            End If
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
        ddlGrupoUnidades.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub

    Sub CargarPropuestas(fechaini As Date, fechafin As Date, grupounidad As Int32)
        Dim o As New Reportes.Directores
        Me.gvPropuestas.DataSource = o.ObtenerListadoPropuestas(fechaini, fechafin, grupounidad)
        Me.gvPropuestas.DataBind()
    End Sub
#End Region


    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If Not (IsDate(Me.txtFechaInicio.Text)) Or Not (IsDate(Me.txtFechaTerminacion.Text)) Then
            ShowNotification("Seleccione primero las fechas antes de continuar", ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
        If Me.ddlGrupoUnidades.SelectedValue = 0 Or Me.ddlGrupoUnidades.SelectedValue = -1 Then
            ShowNotification("Seleccione primero la unidad antes de continuar", ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
        CargarPropuestas(txtFechaInicio.Text, txtFechaTerminacion.Text, ddlGrupoUnidades.SelectedValue)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
End Class