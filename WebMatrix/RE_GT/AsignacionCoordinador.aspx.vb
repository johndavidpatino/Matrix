Imports CoreProject
Imports WebMatrix.Util

Public Class AsignacionCoordinador
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Private _proyectoId As Int64
    Public Property proyectoId() As Int64
        Get
            Return _proyectoId
        End Get
        Set(ByVal value As Int64)
            _proyectoId = value
        End Set
    End Property
    Private _idUsuario As Int64
    Public Property idUsuario() As Int64
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Int64)
            _idUsuario = value
        End Set
    End Property
#End Region

#Region "Funciones y Métodos"
	Sub CargarTrabajos()

		Dim oTrabajo As New GestionTrabajosOP
		Dim oMetodologias As New MetodologiaOperaciones
		Dim oCoord As New CoordinacionCampo
		Dim Id As Int64? = Nothing
		Dim Nombre As String = Nothing
		Dim JobBook As String = Nothing
		If IsNumeric(txtID.Text) Then Id = txtID.Text
		If Not (txtNombre.Text = "") Then Nombre = txtNombre.Text.Trim
		If Not (txtJobBook.Text = "") Then JobBook = txtJobBook.Text.Trim
		'Dim litrabajos = oTrabajo.ListaTrabajos(Id, Nombre, JobBook, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, 1).OrderByDescending(Function(x) x.id)
		gvTrabajos.DataSource = oTrabajo.ListaTrabajos(Id, Nombre, JobBook, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, 1).OrderByDescending(Function(x) x.id).ToList
		gvTrabajos.DataBind()
		accordion0.Visible = True
		accordion1.Visible = False
	End Sub
	Sub CargarMuestra(ByVal TrabajoId As Int64)
        Dim o As New GestionTrabajosOP
        gvDatos.DataSource = o.MuestraXTrabajo(Nothing, TrabajoId)
        gvDatos.DataBind()
    End Sub
    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()
    End Sub
    Public Function ObtenerUsuarios() As List(Of ObtenerUsuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Try
            Return Data.ObtenerUsuarios
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarCoordinadores()
        Try
            Dim oUsuarios As New US.Usuarios

            Dim listapersonas = (From lpersona In oUsuarios.UsuariosxRol(ListaRoles.CoordinadorDeCampo)
                                 Select Id = lpersona.id,
                                 Nombre = lpersona.Apellidos & " " & lpersona.Nombres).OrderBy(Function(p) p.Nombre)
            ddlLider.DataSource = listapersonas.ToList()
            ddlLider.DataValueField = "Id"
            ddlLider.DataTextField = "Nombre"
            ddlLider.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub EnviarEmail(ByVal Ciudad As String, ByVal Usuario As Int64)
        Try
            If String.IsNullOrEmpty(hfidTrabajo.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
            End If
            Dim script As String = "window.open('../Emails/CoordinadorAsignado.aspx?idTrabajo=" & hfidTrabajo.Value & "&ciudad=" & Ciudad & "&usuariocoordinadorasignado=" & Usuario & "','cal','width=400,height=250,left=270,top=180')"
            Dim page As Page = DirectCast(Context.Handler, Page)
            ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub

    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(28, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(51, UsuarioID) = False Then
            Response.Redirect("../RE_GT/RecoleccionDeDatos.aspx")
        End If
        If Not IsPostBack Then
            If Request.QueryString("TipoTecnicaid") IsNot Nothing Then
                hfidTipoTecnica.Value = Request.QueryString("TipoTecnicaid").ToString
            End If
            CargarTrabajos()
            'CargarCombo(ddlLider, "id", "Nombres", ObtenerUsuarios())
            CargarCoordinadores()
        End If
    End Sub
	Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
		gvTrabajos.PageIndex = e.NewPageIndex
		CargarTrabajos()
	End Sub
	Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Asignar" Then
			accordion1.Visible = True
			accordion0.Visible = False
			Me.hfidTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
            CargarMuestra(hfidTrabajo.Value)
        End If
    End Sub
	Private Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
		gvDatos.PageIndex = e.NewPageIndex
		CargarMuestra(hfidTrabajo.Value)
	End Sub
	Private Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
		If e.CommandName = "Asignar" Then
			Me.hfidMuestra.Value = Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id")
		End If
	End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Dim oCoordinacion As New CoordinacionCampo
        Dim Ent As New OP_MuestraTrabajos
        Ent = oCoordinacion.ObtenerMuestraxId(hfidMuestra.Value)
        Ent.Coordinador = ddlLider.SelectedValue
        oCoordinacion.GuardarMuestraXEstudio(Ent)
        log(hfidMuestra.Value, 3)
        CargarTrabajos()
        ShowNotification("Coordinador Asignado", ShowNotifications.InfoNotification)
		EnviarEmail(Ent.C_Divipola.DivMuniNombre, ddlLider.SelectedValue)
	End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        CargarTrabajos()
    End Sub

	Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
		CargarTrabajos()
	End Sub
#End Region


End Class