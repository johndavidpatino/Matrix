Imports CoreProject

Public Class GD_Maestro
    Inherits System.Web.UI.Page

    Dim GDProc As New GD.GD_Procedimientos

    Public Function ObtenerTipoSolicitud() As List(Of GD_TipoSolicitud_Get_Result)
        Dim Data As New GD.GD_Procedimientos
        Try
            Return Data.ObtenerTipoSolicitud
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ObtenerProcesos() As List(Of GD_Procesos_Get_Result)
        Dim Data As New GD.GD_Procedimientos
        Try
            Return Data.ObtenerProcesos
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ObtenerResponsable() As List(Of GD_US_Usuarios_Get_Result)
        Dim Data As New GD.GD_Procedimientos
        Try
            Return Data.ObtenerUsuarios
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ObtenerDocumentos() As List(Of GD_MaestroDocumentos_Get_Result)
        Dim Data As New GD.GD_Procedimientos
        Try
            Return Data.ObtenerDocumentos
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim Data As New GD.GD_Procedimientos
            CargarCombo(ddlTipoSolicitud, "id", "Tipo", ObtenerTipoSolicitud())
            CargarCombo(ddlProceso, "idProceso", "Proceso", ObtenerProcesos())
            CargarCombo(ddlResponsable, "id", "Usuario", ObtenerResponsable())
            CargarCombo(ddlNomDocumento, "idDocumento", "Documento", ObtenerDocumentos())

        End If

    End Sub

    Protected Sub ddlTipoSolicitud_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoSolicitud.SelectedIndexChanged

        If ddlTipoSolicitud.SelectedValue = 1 Then
            txtNomDoc.Visible = True
            txtCodDoc.Visible = True
            ddlProceso.Visible = True
            lblNomDoc.Visible = True
            lblCodDoc.Visible = True
            lblProceso.Visible = True
            btnGuardar.Visible = True
        Else
            txtNomDoc.Visible = False
            txtCodDoc.Visible = False
            ddlProceso.Visible = False
            lblNomDoc.Visible = False
            lblCodDoc.Visible = False
            lblProceso.Visible = False
        End If

        If ddlTipoSolicitud.SelectedValue = 2 Then
            lblNomDoc2.Visible = True
            ddlNomDocumento.Visible = True
            btnGuardar.Visible = True
        Else
            lblNomDoc2.Visible = False
            ddlNomDocumento.Visible = False
        End If

        If ddlTipoSolicitud.SelectedValue = 3 Then
            btnGuardar.Visible = True
            lblNomDoc2.Visible = True
            ddlNomDocumento.Visible = True

            txtUbiArchivo.Visible = False
            txtMetRec.Visible = False
            txtTiempoRet.Visible = False
            txtDisFin.Visible = False

            lblUbiArchivo.Visible = False
            lblMetRec.Visible = False
            lblTiempoRet.Visible = False
            LblDisFin.Visible = False
        Else
            txtUbiArchivo.Visible = True
            txtMetRec.Visible = True
            txtTiempoRet.Visible = True
            txtDisFin.Visible = True

            lblUbiArchivo.Visible = True
            lblMetRec.Visible = True
            lblTiempoRet.Visible = True
            LblDisFin.Visible = True
        End If


    End Sub

    'CoreProject.TipoSolicitud.Construccion Then

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click

        Try
            If ddlTipoSolicitud.SelectedValue = 1 Then
                Construccion()
            ElseIf ddlTipoSolicitud.SelectedValue = 2 Then
                Actualización()
            ElseIf ddlTipoSolicitud.SelectedValue = 3 Then
                Anulacion()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Error")

        End Try
        'Borrado de campos
        txtNomDoc.Text = ""
        txtCodDoc.Text = ""
        txtUbiArchivo.Text = ""
        txtMetRec.Text = ""
        txtTiempoRet.Text = ""
        txtDisFin.Text = ""

        'Prueba envio de correo
        'Dim sm As EnviarCorreo = New EnviarCorreo
        'Dim des As List(Of String) = New List(Of String)
        'des.Add("miguelito_k-do@hotmail.com")
        'des.Add("mprieto@energiasolarsa.com")
        'des.Add("miguel.prieto87@gmail.com")
        'sm.sendMail(des, "PRUEBA", "CONTENIDO")


    End Sub

#Region "Almcenar"
    Public Sub Construccion()

        If String.IsNullOrEmpty(txtNomDoc.Text) Then
            Throw New ApplicationException("Debe ingresar el nombre del Documento.")
        End If

        If String.IsNullOrEmpty(txtCodDoc.Text) Then
            Throw New ApplicationException("Debe ingresar el nombre del documento.")
        End If

        'If String.IsNullOrEmpty(txt.Text) Then
        '    Throw New ApplicationException("Debe ingresar el código del documento.")
        'End If
        Dim idproceso As Integer = ddlProceso.SelectedValue
        Dim res As String = ddlResponsable.SelectedItem.ToString
        'agregarMaestroDoc.IngresarDocumentoMaestro(txtNomDoc.Text, True, False, txtCodDoc.Text, idproceso, res)
        Dim lastid As Integer = GDProc.IngresarDocumentoMaestro2(txtNomDoc.Text, True, False, txtCodDoc.Text, idproceso, res)
        GDProc.IngresarDocumentoControlado(lastid, False, txtUbiArchivo.Text, txtMetRec.Text, txtTiempoRet.Text, txtDisFin.Text)
        MsgBox("Documento agredado", MsgBoxStyle.Information)

    End Sub
    Public Sub Actualización()
        Dim docid As Integer = ddlNomDocumento.SelectedValue
        GDProc.IngresarDocumentoControlado(docid, False, txtUbiArchivo.Text, txtMetRec.Text, txtTiempoRet.Text, txtDisFin.Text)
        MsgBox("Documento actualizado", MsgBoxStyle.Information)
    End Sub

    Public Sub Anulacion()
        Dim docid As Integer = ddlNomDocumento.SelectedValue
        GDProc.DocMaestroActivo(docid)
        GDProc.DocControlados(docid)
    End Sub
#End Region


End Class