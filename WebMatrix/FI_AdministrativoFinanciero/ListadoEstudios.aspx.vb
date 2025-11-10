Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class ListadoEstudios
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
            CargarEstudios()
        End If
    End Sub
    
    Private Sub gvEstudios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEstudios.RowCommand
        If e.CommandName = "PresupuestosAsignados" Then
            CargarPresupuestosXEstudio(Int64.Parse(Me.gvEstudios.DataKeys(CInt(e.CommandArgument))("Id")))
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
        CargarEstudios()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    
#End Region
#Region "Metodos"

    Sub CargarEstudios()
        Dim oEstudio As New Estudio

        Dim id As Int64? = Nothing
        Dim nombre As String = Nothing
        Dim jobbook As String = Nothing
        Dim propuesta As Int64? = Nothing

        If Not (txtNombreBuscar.Text = "") Then nombre = txtNombreBuscar.Text
        If Not (txtJobBook.Text = "") Then jobbook = txtJobBook.Text
        If Not (txtNoPropuestaBuscar.Text = "") Then propuesta = txtNoPropuestaBuscar.Text

        gvEstudios.DataSource = oEstudio.ObtenerConsultaEstudios(id, propuesta, jobbook, nombre)
        gvEstudios.DataBind()
        upEstudios.Update()
    End Sub
    Sub CargarPresupuestosXEstudio(ByVal estudioID As Int64)
        Dim oPresupuesto As New Presupuesto
        Dim x = oPresupuesto.ObtenerPresupuestosAsignadosXEstudio(estudioID)

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
        CargarEstudios()
    End Sub
    
#End Region

    Private Sub gvPresupuestosAsignadosXEstudio_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPresupuestosAsignadosXEstudio.RowCommand
        Dim oePresupuesto As New CU_Presupuesto_Get_Result
        Dim oPresupuesto As New Presupuesto
        oePresupuesto = oPresupuesto.obtenerXId(gvPresupuestosAsignadosXEstudio.DataKeys(e.CommandArgument)("Id"))
        If e.CommandName = "ConsultarPresupuesto" Then
            Response.Redirect("../CAP/Cap_Principal.aspx?IdPropuesta=" & oePresupuesto.PropuestaId & "&Alternativa=" & oePresupuesto.Alternativa & "&Accion=9")
        End If
    End Sub

End Class