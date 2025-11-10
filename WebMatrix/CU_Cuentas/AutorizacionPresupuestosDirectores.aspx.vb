Imports CoreProject

Public Class AutorizacionPresupuestos
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(15, UsuarioID) = False Then
                Response.Redirect("../Home.aspx")
            End If
            cargarGruposUnidades()
            CargarGerentesCuentas()
            'CargarPropuestas()
            If Request.QueryString("PropuestaId") IsNot Nothing Then
                cargarPresupuestos(Nothing, Nothing, Request.QueryString("PropuestaId"))
            Else
                cargarPresupuestos(Nothing, Nothing, Nothing)
            End If
        End If
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        cargarPresupuestos(If(CType(ddlGerenteCuentas.SelectedValue, Integer?) = -1, CType(Nothing, Integer?), CType(ddlGerenteCuentas.SelectedValue, Integer?)), If(CType(ddlUnidades.SelectedValue, Integer?) = -1, CType(Nothing, Integer?), CType(ddlUnidades.SelectedValue, Integer?)), If(CType(txtPropuestas.Text, String) = "", CType(Nothing, String), CType(txtPropuestas.Text, String)))
        upPresupuestos.Update()
    End Sub
    Private Sub gvPresupuestos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPresupuestos.PageIndexChanging
        gvPresupuestos.PageIndex = e.NewPageIndex
        cargarPresupuestos(If(CType(ddlGerenteCuentas.SelectedValue, Integer?) = -1, CType(Nothing, Integer?), CType(ddlGerenteCuentas.SelectedValue, Integer?)), If(CType(ddlUnidades.SelectedValue, Integer?) = -1, CType(Nothing, Integer?), CType(ddlUnidades.SelectedValue, Integer?)), If(CType(txtPropuestas.Text, String) = "", CType(Nothing, String), CType(txtPropuestas.Text, String)))
        upPresupuestos.Update()
    End Sub
    Private Sub gvPresupuestos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPresupuestos.RowCommand
        Dim oePresupuesto As New CU_Presupuesto_Get_Result
        Dim oPresupuesto As New Presupuesto

        If e.CommandName = "Revisar" Then
            oePresupuesto = oPresupuesto.obtenerXId(gvPresupuestos.DataKeys(e.CommandArgument)("Id"))
            Response.Redirect("../CAP/Cap_Principal.aspx?IdPropuesta=" & oePresupuesto.PropuestaId & "&Alternativa=" & oePresupuesto.Alternativa & "&Accion=4")
        End If
    End Sub
#End Region
#Region "Metodos"
    Sub cargarPresupuestos(ByVal gerenteCuentas As Int64?, ByVal grupoUnidad As Int32?, ByVal propuesta As String)
        Dim o As New Presupuesto
        gvPresupuestos.DataSource = o.obtenerPresupuestosXGrupoUnidadXGerenteCuentasAprobacionDirectores(grupoUnidad, gerenteCuentas, propuesta)
        gvPresupuestos.DataBind()
    End Sub
    Sub cargarGruposUnidades()
        Dim oGrupoUnidad As New US.GrupoUnidad
        ddlUnidades.DataSource = oGrupoUnidad.ObtenerGrupoUnidadCombo(1)
        ddlUnidades.DataValueField = "id"
        ddlUnidades.DataTextField = "GrupoUnidad"
        ddlUnidades.DataBind()
        ddlUnidades.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
    Sub CargarGerentesCuentas()
        Dim oUsuarios As New US.Usuarios
        Dim listapersonas = (From lpersona In oUsuarios.UsuariosxRol(ListaRoles.GerenteCuentas)
                             Select Id = lpersona.id,
                             Nombre = lpersona.Apellidos & " " & lpersona.Nombres).OrderBy(Function(p) p.Nombre)
        ddlGerenteCuentas.DataSource = listapersonas.ToList()
        ddlGerenteCuentas.DataValueField = "Id"
        ddlGerenteCuentas.DataTextField = "Nombre"
        ddlGerenteCuentas.DataBind()
        ddlGerenteCuentas.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub
    'Sub CargarPropuestas()
    '    Dim o As New Presupuesto
    '    ddlPropuestas.DataSource = o.DevolverPropuestasParaRevisionPresupuestos
    '    ddlPropuestas.DataValueField = "id"
    '    ddlPropuestas.DataTextField = "Titulo"
    '    ddlPropuestas.DataBind()
    '    ddlPropuestas.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    'End Sub
#End Region
End Class