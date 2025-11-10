Imports CoreProject
Imports WebMatrix.Util


Public Class MuestraTrabajosC
    Inherits System.Web.UI.Page

#Region "Propiedades"
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
    Sub CargarMuestra(ByVal TrabajoId As Int64)
        Dim oCoorCampo As New CoordinacionCampo
        Dim listaMuestra = (From lmuestra In oCoorCampo.ObtenerMuestraxEstudioList(TrabajoId)
                            Select idMuestra = lmuestra.Id, departamento = lmuestra.C_Divipola.DivDeptoNombre, ciudad = lmuestra.C_Divipola.DivMuniNombre,
                            cantidad = lmuestra.Cantidad).OrderBy(Function(x) x.ciudad)
        If listaMuestra.Count = 0 Then
            Exit Sub
        End If
        gvTrabajos.DataSource = listaMuestra.ToList
        gvTrabajos.DataBind()
    End Sub
    Sub CargarDepartamentos()
        Dim oCoordCampo As New CoordinacionCampo
        Dim list = (From ldep In oCoordCampo.ObtenerDepartamentos()
                  Select iddep = ldep.DivDeptoMunicipio, nomdep = ldep.DivDeptoNombre).Distinct.ToList
        ddlDepartamento.DataSource = list
        ddlDepartamento.DataValueField = "iddep"
        ddlDepartamento.DataTextField = "nomdep"
        ddlDepartamento.DataBind()
    End Sub
    Sub CargarCiudades()
        Dim oCoordCampo As New CoordinacionCampo
        Dim listciudades = (From ldep In oCoordCampo.ObtenerCiudades(ddlDepartamento.SelectedValue)
                  Select idmuni = ldep.DivMunicipio, nommuni = ldep.DivMuniNombre)
        ddlCiudad.DataSource = listciudades
        ddlCiudad.DataValueField = "idmuni"
        ddlCiudad.DataTextField = "nommuni"
        ddlCiudad.DataBind()
    End Sub
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("TrabajoId") IsNot Nothing Then
                Dim TrabajoId As Int64 = Int64.Parse(Request.QueryString("TrabajoId").ToString())
                hfdiTrabajo.Value = TrabajoId
                CargarMuestra(TrabajoId)
                CargarDepartamentos()
            End If
        End If
    End Sub
    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarMuestra(hfdiTrabajo.Value)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Eliminar" Then
            Dim idMuestra As Int64 = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("idMuestra"))
            Dim oCoordCampo As New CoordinacionCampo
            oCoordCampo.EliminarMuestraXEstudio(idMuestra)
            CargarMuestra(hfdiTrabajo.Value)
            ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End If
    End Sub
    Protected Sub ddlDepartamento_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDepartamento.SelectedIndexChanged
        If ddlDepartamento.SelectedValue = "" Then Exit Sub
        CargarCiudades()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnAddMuestra_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddMuestra.Click
        Dim oCoordCampo As New CoordinacionCampo
        Dim Ent As New OP_MuestraTrabajos
        If Not (IsNumeric(tbCantidad.Text)) Then
            ShowNotification("La cantidad debe ser numérica", ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            Exit Sub
        End If
        Ent.CiudadId = ddlCiudad.SelectedValue
        Ent.TrabajoId = hfdiTrabajo.Value
        Ent.Cantidad = tbCantidad.Text
        oCoordCampo.GuardarMuestraXEstudio(Ent)
        CargarMuestra(hfdiTrabajo.Value)
        ddlDepartamento.ClearSelection()
        ddlCiudad.ClearSelection()
        Me.tbCantidad.Text = String.Empty
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Response.Redirect("../OP_Cualitativo/Trabajos.aspx")
    End Sub
#End Region

End Class