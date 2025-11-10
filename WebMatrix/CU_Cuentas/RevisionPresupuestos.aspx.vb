Imports CoreProject

Public Class RevisionPresupuestos
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(21, UsuarioID) = False Then
                Response.Redirect("../Home.aspx")
            End If
            If Request.QueryString("PropuestaId") IsNot Nothing Then
                cargarPresupuestos(Request.QueryString("PropuestaId"))
            End If
        End If
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        cargarPresupuestos()
    End Sub
    Private Sub gvPresupuestos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPresupuestos.PageIndexChanging
        gvPresupuestos.PageIndex = e.NewPageIndex
        cargarPresupuestos()
    End Sub
    Private Sub gvPresupuestos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPresupuestos.RowCommand
        Dim oePresupuesto As New CU_Presupuesto_Get_Result
        Dim oPresupuesto As New Presupuesto
        If e.CommandName = "Revisar" Then
            oePresupuesto = oPresupuesto.obtenerXId(gvPresupuestos.DataKeys(e.CommandArgument)("Id"))
            Response.Redirect("../CAP/Cap_Principal.aspx?IdPropuesta=" & oePresupuesto.PropuestaId & "&Alternativa=" & oePresupuesto.Alternativa & "&Accion=2")
        End If
        If e.CommandName = "ReviewNew" Then
            oePresupuesto = oPresupuesto.obtenerXId(gvPresupuestos.DataKeys(e.CommandArgument)("Id"))
            Dim info As New oJobBook
            Dim oData As New CU_JobBook.DAL
            Dim rData = oData.InfoJobBookGet(IdPropuesta:=oePresupuesto.PropuestaId).FirstOrDefault
            info.Cliente = rData.Cliente
            info.Estado = rData.Estado
            info.GerenteCuentas = rData.GerenteCuentas
            info.GerenteCuentasID = rData.GerenteCuentasID
            info.IdBrief = rData.IdBrief
            info.IdEstudio = rData.IdEstudio
            info.IdPropuesta = rData.IdPropuesta
            info.IdUnidad = rData.IdUnidad
            info.MarcaCategoria = rData.MarcaCategoria
            info.Titulo = rData.Titulo
            info.Unidad = rData.Unidad
            info.Viabilidad = rData.Viabilidad
            info.NumJobBook = rData.NumJobbook
            info.Alternativa = oePresupuesto.Alternativa
            info.ReviewOPS = True
			info.GuardarCambios = True
			Session("InfoJobBook") = info
            Response.Redirect("Presupuesto.aspx")
        End If
    End Sub
#End Region
#Region "Metodos"
    Sub cargarPresupuestos(Optional ByVal PropuestaId As Int64 = 0)
        Dim o As New Presupuesto
        Dim GerenteOperacionesId As Int64 = Session("IDUsuario").ToString
        Dim Revisado As Boolean = chbRevisados.Checked
        Dim TituloPropuesta As String = Nothing
        Dim IdPropuesta As Int64? = Nothing
        Dim IdTrabajo As Int64? = Nothing
        Dim jobbook As String = Nothing
        If Not (PropuestaId = 0) Then IdPropuesta = PropuestaId
        If IsNumeric(txtNoPropuestaBuscar.Text) Then IdPropuesta = txtNoPropuestaBuscar.Text
        If IsNumeric(txtIdTrabajo.Text) Then IdTrabajo = txtIdTrabajo.Text
        If Not (txtJobBook.Text = "") Then jobbook = txtJobBook.Text
        If Not (txtNombreBuscar.Text = "") Then TituloPropuesta = txtNombreBuscar.Text
        gvPresupuestos.DataSource = o.ObtenerPresupuestosParaRevisar(GerenteOperacionesId, Revisado, TituloPropuesta, IdPropuesta, IdTrabajo, jobbook)
        gvPresupuestos.DataBind()
    End Sub

#End Region

    Protected Sub chbRevisados_CheckedChanged(sender As Object, e As EventArgs) Handles chbRevisados.CheckedChanged
        cargarPresupuestos()
    End Sub
End Class