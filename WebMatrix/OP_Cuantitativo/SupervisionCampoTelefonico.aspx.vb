Imports CoreProject

Public Class SupervisionCampoTelefonico
    Inherits System.Web.UI.Page

#Region "Funciones y Métodos"
    Public Function ObtenerUsuarios() As List(Of ObtenerUsuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Try
            Return Data.ObtenerUsuarios
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Public Function ObtenerGeneralidades() As List(Of f_Generalidades_Get_Result)
    '    Dim Data As New OP.SupervisionCampoTelefonico
    '    Try
    '        Return Data.ObtenerGeneralidades
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()
    End Sub
    Function Validar() As Boolean
        Dim val As Boolean = True
        If txtIdentificacionCT.Text = String.Empty Then
            lblResult.Text = "Especifique la Identificacion"
            val = False
        End If
        If txtTrabajoId.Text = String.Empty Then
            lblResult.Text = "Especifique el ID del trabajo"
            val = False
        End If
        Return val
    End Function
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("TrabajoId") IsNot Nothing Then
                Dim TrabajoId As Int64 = Int64.Parse(Request.QueryString("TrabajoId").ToString)
                Dim Data As New OP.SupervisionCampoTelefonico
                'txtNoEstudio.Text = Data.ObtenerMaxNoEstudio(TrabajoId) + 1
                txtTrabajoId.Text = TrabajoId
                CargarCombo(ddlOperador, "Id", "Nombres", ObtenerUsuarios())
                'CargarCombo(ddlACC01, "Id", "Generalidad", ObtenerGeneralidades())
                'CargarCombo(ddlACC02, "Id", "Generalidad", ObtenerGeneralidades())
                'CargarCombo(ddlACC03, "Id", "Generalidad", ObtenerGeneralidades())
                'CargarCombo(ddlACC04, "Id", "Generalidad", ObtenerGeneralidades())
                'CargarCombo(ddlCOM01, "Id", "Generalidad", ObtenerGeneralidades())
                'CargarCombo(ddlCOM02, "Id", "Generalidad", ObtenerGeneralidades())
                'CargarCombo(ddlCOM03, "Id", "Generalidad", ObtenerGeneralidades())
                'CargarCombo(ddlCOM04, "Id", "Generalidad", ObtenerGeneralidades())
                ddlACC01.SelectedValue = 1
                ddlACC02.SelectedValue = 1
                ddlACC03.SelectedValue = 1
                ddlACC04.SelectedValue = 1
                ddlCOM01.SelectedValue = 1
                ddlCOM02.SelectedValue = 1
                ddlCOM03.SelectedValue = 1
                ddlCOM04.SelectedValue = 1
            Else
                Response.Redirect("TrabajosProyectos.aspx")
            End If
        End If
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Session("IdUsuario") = 1047223102
        If Validar() Then
            Try

                Dim Data As New OP.SupervisionCampoTelefonico
                Data.AgregarSupervisionCampoTelefonico(txtTrabajoId.Text, txtIdentificacionCT.Text, Session("IdUsuario"), ddlOperador.SelectedValue, DateTime.UtcNow.AddHours(-5), ChkCRI01.Checked, ChkCRI02.Checked, ChkCRI03.Checked, ChkCRI04.Checked, ChkCRI05.Checked, ChkCRI06.Checked, ChkCRI07.Checked, ChkCRI08.Checked, ChkCRI09.Checked, ChkCRI10.Checked, ChkCRI11.Checked, ChkCRI12.Checked, ChkCRI13.Checked, ddlCOM01.SelectedValue, ddlCOM02.SelectedValue, ddlCOM03.SelectedValue, ddlCOM04.SelectedValue, ddlACC01.SelectedValue, ddlACC02.SelectedValue, ddlACC03.SelectedValue, ddlACC04.SelectedValue, txtObservaciones.Content)
                Response.Redirect("SupervisionCampoTelefonico.aspx?TrabajoId=" & txtTrabajoId.Text)
            Catch ex As Exception
                lblResult.Text = ex.Message
            End Try
        End If
    End Sub
#End Region
End Class