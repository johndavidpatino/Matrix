Imports WebMatrix.Util
Imports CoreProject
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports CoreProject.CC_FinzOpe
Public Class ActividadesTrabajo
    Inherits System.Web.UI.Page
    Dim op As New PresupInt

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

            If Not IsPostBack Then
                If Request.QueryString("TrabajoId") IsNot Nothing Then
                    hfIdTrabajo.Value = Request.QueryString("TrabajoId").ToString
                    Dim p As New IQ_Parametros
                    Dim ac As New CC_InfoTrabajo_Result
                    ac = op.InfoTrbajo(hfIdTrabajo.Value).Item(0)
                    p.IdPropuesta = ac.IdPropuesta
                    p.ParAlternativa = ac.Alternativa
                    p.MetCodigo = ac.MetCodigo
                    p.ParNacional = ac.Fase
                    CargarActividades(p)
                    ObtenerPresupuestosAreasInternas(hfIdTrabajo.Value)
                End If
            End If

        End If
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub


    Sub CargarActividades(ByVal p As IQ_Parametros)
        GvActividades.DataSource = ObtenerActividadesXTrabajo(p)
        GvActividades.DataBind()

    End Sub

    Public Function ObtenerActividadesXTrabajo(ByVal par As IQ_Parametros) As List(Of IQ_ObtenerControlCostosAutorizados_Result)

        Try
            Dim _IQ_Entities As New IQ_MODEL

            ' Dim C As List(Of IQ_ObtenerControlCostosAutorizados_Result)
            Dim C = _IQ_Entities.IQ_ObtenerControlCostosAutorizados(par.IdPropuesta, par.ParAlternativa, par.ParNacional, par.MetCodigo).ToList()
            Return C

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Protected Sub btnPresupuestos_Click(sender As Object, e As EventArgs) Handles btnPresupuestos.Click
        Dim lis As List(Of CC_TarifasAutomaticasGet_Result)
        lis = op.TarifasAutomaticas(Nothing)

        For I = 0 To lis.Count - 1
            CrearPresupuestos(lis.Item(I).Id)
        Next
        ObtenerPresupuestosAreasInternas(hfIdTrabajo.Value)

    End Sub
    Sub CrearPresupuestos(ByVal IdActividad As Int64)
        Dim Op As New ProcesosInternos
        Op.guardarpresupuestointerno(IdActividad, Session("IDUsuario"), hfIdTrabajo.Value)
    End Sub

    Sub ObtenerPresupuestosAreasInternas(ByVal Trabajoid As Int64)
        Dim op As New ProcesosInternos
        GvPresupuestos.DataSource = op.ObtenerPresupuestosAreasInternas(Trabajoid)
        GvPresupuestos.DataBind()

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ActualizarVrUnitario()
        ObtenerPresupuestosAreasInternas(hfIdTrabajo.Value)
    End Sub

    Sub ActualizarVrUnitario()
        For Each row As GridViewRow In Me.GvPresupuestos.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim id As Int64 = Me.GvPresupuestos.DataKeys(row.RowIndex).Value
                Dim cant As Double = 0
                If IsNumeric(DirectCast(row.FindControl("txtVrUnitario"), TextBox).Text) Then
                    cant = DirectCast(row.FindControl("txtVrUnitario"), TextBox).Text
                End If
                op.ActualizarVrunitario(id, cant)
            End If
        Next
    End Sub

    Sub TarifasAutomaticas(ByVal Id As Int64?)
        ddlActividades.DataSource = op.TarifasAutomaticas(Id)
        ddlActividades.DataValueField = "Id"
        ddlActividades.DataTextField = "Nombre"
        ddlActividades.DataBind()

    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Me.upPresupuestos.Update()
        TarifasAutomaticas(Nothing)

    End Sub

    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        CrearPresupuestos(ddlActividades.SelectedValue)
        ObtenerPresupuestosAreasInternas(hfIdTrabajo.Value)
    End Sub
End Class