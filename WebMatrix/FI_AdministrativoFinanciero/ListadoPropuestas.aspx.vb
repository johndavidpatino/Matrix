Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class FI_ListadoPropuestas
    Inherits System.Web.UI.Page


    Private _idUsuario As Int64
    Public Property idUsuario() As Int64
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Int64)
            _idUsuario = value
        End Set
    End Property


#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        idUsuario = Session("IDUsuario")

        If Me.IsPostBack = False Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(127, UsuarioID) = False Then

            End If
            CargarGerentesCuentas()
            CargarEstados()
        End If
    End Sub

    Sub CargarGerentesCuentas()
        Dim o As New CoreProject.US.Usuarios
        Dim li = o.UsuariosxRol(ListaRoles.GerenteCuentas)
        Dim list = (From lst In li
                  Select NombreCompleto = lst.Nombres & " " & lst.Apellidos, id = lst.id)
        ddlGerenteCuentas.DataSource = list
        ddlGerenteCuentas.DataValueField = "id"
        ddlGerenteCuentas.DataTextField = "NombreCompleto"
        ddlGerenteCuentas.DataBind()
        ddlGerenteCuentas.Items.Insert(0, New ListItem With {.Text = "--Ver todo--", .Value = -1})
    End Sub

    Private Sub gvEstudios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEstudios.RowCommand
        If e.CommandName = "PresupuestosAsignados" Then
            CargarPresupuestosXEstudio(Int64.Parse(Me.gvEstudios.DataKeys(CInt(e.CommandArgument))("Id")))
            Me.hfidPropuesta.Value = Int64.Parse(Me.gvEstudios.DataKeys(CInt(e.CommandArgument))("Id"))
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        ElseIf e.CommandName = "Actualizar" Then

        End If
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Buscar()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub gvEstudios_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvEstudios.PageIndexChanging
        gvEstudios.PageIndex = e.NewPageIndex
        CargarPropuestas()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

#End Region
#Region "Metodos"

    Sub CargarPropuestas()
        Dim oPropuesta As New Propuesta

        Dim id As Int64? = Nothing
        Dim gerentecuentas As Int64? = Nothing
        Dim estado As Short? = Nothing
        If IsNumeric(txtNoPropuestaBuscar.Text) Then id = txtNoPropuestaBuscar.Text
        If IsNumeric(ddlGerenteCuentas.SelectedValue) And Not (ddlGerenteCuentas.SelectedValue = "-1") Then gerentecuentas = ddlGerenteCuentas.SelectedValue
        If IsNumeric(ddlEstado.SelectedValue) And Not (ddlEstado.SelectedValue = "-1") Then estado = ddlEstado.SelectedValue
        gvEstudios.DataSource = oPropuesta.ObtenerXGerenteXPropuesta(id, gerentecuentas, estado)
        gvEstudios.DataBind()
        upEstudios.Update()
    End Sub
    Public Sub CargarEstados()
        Try
            Dim oAuxiliares As New Auxiliares
            Dim listaEstados = (From lestado In oAuxiliares.DevolverEstadoPropuesta()
                                Select Id = lestado.id, Estado = lestado.Estado).OrderBy(Function(e) e.Id).ToList()
            ddlEstado.DataSource = listaEstados
            ddlEstado.DataValueField = "Id"
            ddlEstado.DataTextField = "Estado"
            ddlEstado.DataBind()
            ddlEstado.Items.Insert(0, New ListItem With {.Text = "--Ver todas--", .Value = "-1"})
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Sub CargarPresupuestosXEstudio(ByVal estudioID As Int64)
        Dim oPresupuesto As New Presupuesto
        Dim x = oPresupuesto.DevolverxIdPropuesta(estudioID, Nothing)

        gvPresupuestosAsignadosXEstudio.DataSource = x
        gvPresupuestosAsignadosXEstudio.DataBind()
        upPresupuestosAsignadosXEstudio.Update()
    End Sub
    Function obtenerPresupuestosNuevos(ByVal presupuestosAsignados() As String, ByVal presupuestosOriginales() As String) As List(Of CU_Estudios_Presupuestos)
        Dim listCU_Estudios_Presupuestos_Nuevos As New List(Of CU_Estudios_Presupuestos)
        Dim PresupuestosNuevos = (From x In presupuestosAsignados Group Join y In presupuestosOriginales On x Equals y Into Coincidencia = Group Where Coincidencia.Count = 0).ToArray

        For Each i In PresupuestosNuevos
            listCU_Estudios_Presupuestos_Nuevos.Add(New CU_Estudios_Presupuestos With {.PresupuestoId = i.x})
        Next

        Return listCU_Estudios_Presupuestos_Nuevos

    End Function
    Function obtenerPresupuestosEliminados(ByVal presupuestosAsignados() As String, ByVal presupuestosOriginales() As String) As List(Of CU_Estudios_Presupuestos)
        Dim listCU_Estudios_Presupuestos_Eliminados As New List(Of CU_Estudios_Presupuestos)
        Dim PresupuestosEliminados = (From x In presupuestosOriginales Group Join y In presupuestosAsignados On x Equals y Into Coincidencia = Group Where Coincidencia.Count = 0).ToArray

        For Each i In PresupuestosEliminados
            listCU_Estudios_Presupuestos_Eliminados.Add(New CU_Estudios_Presupuestos With {.PresupuestoId = i.x})
        Next

        Return listCU_Estudios_Presupuestos_Eliminados

    End Function

    Sub Buscar()
        CargarPropuestas()
    End Sub

#End Region

    Private Sub gvPresupuestosAsignadosXEstudio_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPresupuestosAsignadosXEstudio.RowCommand
        Dim oePresupuesto As New CU_Presupuesto_Get_Result
        Dim oPresupuesto As New Presupuesto

        If e.CommandName = "ConsultarPresupuesto" Then
            oePresupuesto = oPresupuesto.obtenerXId(gvPresupuestosAsignadosXEstudio.DataKeys(e.CommandArgument)("Id"))
            Response.Redirect("../CAP/Cap_Principal.aspx?IdPropuesta=" & oePresupuesto.PropuestaId & "&Alternativa=" & oePresupuesto.Alternativa & "&Accion=10")
        End If
    End Sub

    Private Sub CargarPresupuestosXEstudio(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPresupuestosAsignadosXEstudio.PageIndexChanging
        Me.gvPresupuestosAsignadosXEstudio.PageIndex = e.NewPageIndex
        CargarPresupuestosXEstudio(hfidPropuesta.Value)
    End Sub
End Class