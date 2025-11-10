Imports CoreProject
Imports WebMatrix.Util

Public Class CambiosJBI
    Inherits System.Web.UI.Page

#Region "Propiedades"

#End Region

#Region "Funciones y Metodos"
    Sub CambiarJobBookInterno()
        Dim iqent As New IQ.Consultas
        iqent.CambiarJBI(txtIdTrabajo.Text, ddlFases.SelectedValue, txtNuevoJBI.Text)
    End Sub

    Sub CargarFases()
        Dim iqent As New IQ.Consultas
        Me.ddlFases.DataSource = iqent.FasesList
        Me.ddlFases.DataValueField = "IdFase"
        Me.ddlFases.DataTextField = "DescFase"
        Me.ddlFases.DataBind()
        Me.ddlFases.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub ValidadFaseCreada()
        Dim _ControlCostos As New IQ.ControlCostos()
        Dim oTrabajo As New Trabajo

        If txtIdTrabajo.Text = "" Then
            ShowNotification("Debe ingresar el ID del Trabajo!", ShowNotifications.ErrorNotification)
            txtIdTrabajo.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlFases.SelectedValue = "-1" Then
            ShowNotification("Debe seleccionar la Fase para modificar el JBI!", ShowNotifications.ErrorNotification)
            ddlFases.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtNuevoJBI.Text = "" Then
            ShowNotification("Debe ingresar el Nuevo JBI con el formato indicado!", ShowNotifications.ErrorNotification)
            txtNuevoJBI.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        Dim infoT = oTrabajo.obtenerXId(txtIdTrabajo.Text)

        If infoT Is Nothing Then
            ShowNotification("El Id de Trabajo NO existe!", ShowNotifications.ErrorNotification)
            txtIdTrabajo.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        Dim lstParametros = _ControlCostos.ObtenerParametros(infoT.IdPropuesta, infoT.Alternativa, ddlFases.SelectedValue, infoT.MetCodigo)

        If lstParametros.Count = 0 Then
            ShowNotification("La Fase debe estar Creada para poder realizar el cambio de JBI", ShowNotifications.ErrorNotification)
            ddlFases.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        CambiarJobBookInterno()
        logCambiosJBI(infoT.JobBook)

        ShowNotification("El JobBook Interno ha sido cambiado Correctamente!", ShowNotifications.InfoNotification)

        limpiarControles()

    End Sub

    Sub logCambiosJBI(ByVal jBIAnterior As String)
        Dim iqent As New IQ.Consultas
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        iqent.GuardarLogCambiosJBI(txtIdTrabajo.Text, jBIAnterior, txtNuevoJBI.Text, UsuarioID)
    End Sub

    Sub limpiarControles()
        txtIdTrabajo.Text = ""
        ddlFases.SelectedValue = "-1"
        txtNuevoJBI.Text = ""
    End Sub


#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(19, UsuarioID) = False Then
                Response.Redirect("../Home.aspx")
            End If
            CargarFases()
        End If
    End Sub


    Protected Sub btnCambiarJBI_Click(sender As Object, e As EventArgs) Handles btnCambiarJBI.Click
        ValidadFaseCreada()
    End Sub

#End Region

End Class