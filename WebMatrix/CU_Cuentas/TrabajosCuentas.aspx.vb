Imports CoreProject
Imports WebMatrix.Util
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class TrabajosCuentas
    Inherits System.Web.UI.Page


#Region "Propiedades"
    Dim eTrabajoOP As New OP_TrabajoConfiguracion
    Private _IDUsuario As Int64
    Public Property IDUsuario() As Int64
        Get
            Return _IDUsuario
        End Get
        Set(ByVal value As Int64)
            _IDUsuario = value
        End Set
    End Property
#End Region

#Region "Funciones y Métodos"
    Sub CargarTrabajos()
        Dim oTrabajo As New Estudio
        Dim oCoord As New CoordinacionCampo

        Dim Id As Int64? = Nothing
        Dim Nombre As String = Nothing
        Dim JobBook As String = Nothing
        gvTrabajos.DataSource = oTrabajo.ObtenerTrabajos(Id, Nombre, Nothing, Request.QueryString("EstudioId").ToString).OrderByDescending(Function(x) x.id).ToList
        gvTrabajos.DataBind()
    End Sub
    Function obtenerOPMetodologia(ByVal id As Int16) As OP_Metodologias_Get_Result
        Dim oMetodologiaOperaciones As New MetodologiaOperaciones
        Return oMetodologiaOperaciones.obtenerXId(id)
    End Function
    Function obtenerProyectoXId(ByVal id As Int64) As PY_Proyectos_Get_Result
        Dim oProyecto As New Proyecto
        Return oProyecto.obtenerXId(id)
    End Function
    Public Function CargarFichaCuantitativa() As Long
        Try
            Dim idtrabajo As Int64 = Int64.Parse(hfIdTrabajo.Value)
            Dim oTrabajo As New Trabajo
            Dim info = oTrabajo.DevolverxID(idtrabajo)

            Dim oFichaCuantitativa As New FichaCuantitativo
            Return oFichaCuantitativa.DevolverxTrabajoID(hfIdTrabajo.Value).Item(0).id

        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim permisos As New Datos.ClsPermisosUsuarios
        'Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        'If permisos.VerificarPermisoUsuario(101, UsuarioID) = False Then
        '    Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
        'End If
        If Not IsPostBack Then
            CargarTrabajos()
        End If
    End Sub
    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarTrabajos()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Cronograma" Then
            'cargarTrabajo(Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")))
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Dim oTrabajo As New Trabajo
            Dim oeTrabajo = oTrabajo.ObtenerTrabajo(hfIdTrabajo.Value)
            Session("NombreTrabajo") = oeTrabajo.id.ToString & " | " & oeTrabajo.JobBook & " | " & oeTrabajo.NombreTrabajo
            Response.Redirect("../CORE/ListaTareasXHilo.aspx" & "?IdTrabajo=" & hfIdTrabajo.Value)
        ElseIf e.CommandName = "Tareas" Then
            'Response.Redirect("\CORE\Tareas.aspx" & "?IdTrabajo=" & gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id") & "&Coe=" & Session("IDUsuario").ToString)
        ElseIf e.CommandName = "Avance" Then
            hfIdTrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            Dim oTrabajo As New Trabajo
            Dim oeTrabajo = oTrabajo.ObtenerTrabajo(hfIdTrabajo.Value)
            Session("NombreTrabajo") = oeTrabajo.id.ToString & " | " & oeTrabajo.JobBook & " | " & oeTrabajo.NombreTrabajo
            Response.Redirect("../RP_Reportes/AvanceDeCampo.aspx?TrabajoId=" & hfIdTrabajo.Value)
        End If
    End Sub
#End Region


    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Response.Redirect("Estudios.aspx")
    End Sub
End Class