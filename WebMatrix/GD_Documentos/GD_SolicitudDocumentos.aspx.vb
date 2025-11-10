Imports CoreProject

Public Class GD_SolicitudDocumentos
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
    Public Function ObtenerEstados() As List(Of GD_Estados_Get_Result)
        Dim Data As New GD.GD_Procedimientos
        Try
            Return Data.ObtenerEstado
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
            CargarCombo(ddlEstadoId, "id", "Estado", ObtenerEstados())
            CargarCombo(ddlSolicitante, "id", "Nombres", ObtenerResponsable())
            txtfecha.Text = Date.UtcNow.AddHours(-5)
            txtAsunto.Text = "Solicitud de revisión de documentos"
            txtContenido.Content = "<p style=""margin: 0px; text-align: left;""><br />" _
            & " </p><span style=""font-weight: bold;"">SOLICITUD DE REVISIÓN DE DOCUMENTO</span><br />" _
            & "<br /><br />Estimado usuario, usted tiene un documento pendiente por revisar.<br />" _
            & "<br />" _
            & "<br />" _
            & "por favor ingrese&nbsp;aquí&nbsp;para revisar sus documentos&nbsp;"
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

            'txtUbiArchivo.Visible = False
            'txtMetRec.Visible = False
            'txtTiempoRet.Visible = False
            'txtDisFin.Visible = False

            'lblUbiArchivo.Visible = False
            'lblMetRec.Visible = False
            'lblTiempoRet.Visible = False
            'LblDisFin.Visible = False
        Else
            'txtUbiArchivo.Visible = True
            'txtMetRec.Visible = True
            'txtTiempoRet.Visible = True
            'txtDisFin.Visible = True

            'lblUbiArchivo.Visible = True
            'lblMetRec.Visible = True
            'lblTiempoRet.Visible = True
            'LblDisFin.Visible = True
        End If


    End Sub

    Sub EnviarYRevisar(ByVal UltimoId As Integer)
        Dim List As List(Of Usuarios_Result)
        Dim mailUsu As New List(Of String)
        List = Session("Usuarios")
        For Each a As Usuarios_Result In List
            Try
                mailUsu.Add(a.Email)
                GDProc.guardarRevision(UltimoId, a.id, DateTime.UtcNow.AddHours(-5), 1)
            Catch ex As Exception
                Throw ex
            End Try
        Next
        'enviamos el correo
        Dim sm As New EnviarCorreo
        sm.sendMail(mailUsu, "PRUEBA", txtContenido.Content.ToString)
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
    End Sub

#Region "AlmAcenar"
    Public Sub Construccion()

        If String.IsNullOrEmpty(txtNomDoc.Text) Then
            Throw New ApplicationException("Debe ingresar el nombre del Documento.")
        End If

        If String.IsNullOrEmpty(txtCodDoc.Text) Then
            Throw New ApplicationException("Debe ingresar el nombre del documento.")
        End If
        If String.IsNullOrEmpty(txtArea.Text) Then
            Throw New ApplicationException("Debe ingresar el area de trabajo.")
        End If
        If String.IsNullOrEmpty(txtCargo.Text) Then
            Throw New ApplicationException("Debe ingresar el cargo.")
        End If

        'If String.IsNullOrEmpty(txt.Text) Then
        '    Throw New ApplicationException("Debe ingresar el código del documento.")
        'End If
        Dim idproceso As Integer = ddlProceso.SelectedValue
        Dim res As String = ddlResponsable.SelectedItem.ToString
        'agregarMaestroDoc.IngresarDocumentoMaestro(txtNomDoc.Text, True, False, txtCodDoc.Text, idproceso, res)
        Dim lastid As Integer = GDProc.IngresarDocumentoMaestro2(txtNomDoc.Text, True, False, txtCodDoc.Text, idproceso, res)
        GDProc.IngresarSolicitudDocumento(Date.UtcNow.AddHours(-5), ddlSolicitante.SelectedValue, txtArea.Text, txtCargo.Text, ddlTipoSolicitud.SelectedValue, lastid, txtNomDoc.Text, txtCodDoc.Text, txtAreaUso.Text, txtSitAcce.Text, txtRazSol.Text, txtDescSol.Text, ddlEstadoId.SelectedValue, Date.Now, txtComentarios.Text, txtMods.Text)
        'GDProc.IngresarDocumentoControlado(1, lastid, False, txtUbiArchivo.Text, txtMetRec.Text, txtTiempoRet.Text, txtDisFin.Text)

        'me devuelve un id empanadoso ya que no se le puso autoincrement, no se supo mas que hacer
        Dim DCLastId As Integer = GDProc.IngresarDocumentoControlado(lastid, False, "", "", "", "")
        EnviarYRevisar(DCLastId)
        MsgBox("Solicitud aceptada", MsgBoxStyle.Information)
    End Sub

    Public Sub Actualización()
        Dim docid As Integer = ddlNomDocumento.SelectedValue
        'me devuelve un id empanadoso ya que no se le puso autoincrement
        GDProc.IngresarSolicitudDocumento(Date.UtcNow.AddHours(-5), ddlSolicitante.SelectedValue, txtArea.Text, txtCargo.Text, ddlTipoSolicitud.SelectedValue, docid, txtNomDoc.Text, txtCodDoc.Text, txtAreaUso.Text, txtSitAcce.Text, txtRazSol.Text, txtDescSol.Text, ddlEstadoId.SelectedValue, Date.Now, txtComentarios.Text, txtMods.Text)
        Dim DCLastId As Integer = GDProc.IngresarDocumentoControlado(docid, False, "", "", "", "")
        EnviarYRevisar(DCLastId)
        MsgBox("Documento actualizado", MsgBoxStyle.Information)
    End Sub

    Public Sub Anulacion()
        Dim docid As Integer = ddlNomDocumento.SelectedValue
        GDProc.IngresarSolicitudDocumento(Date.UtcNow.AddHours(-5), ddlSolicitante.SelectedValue, txtArea.Text, txtCargo.Text, ddlTipoSolicitud.SelectedValue, docid, txtNomDoc.Text, txtCodDoc.Text, txtAreaUso.Text, txtSitAcce.Text, txtRazSol.Text, txtDescSol.Text, ddlEstadoId.SelectedValue, Date.Now, txtComentarios.Text, txtMods.Text)
        GDProc.DocMaestroActivo(docid)
        GDProc.DocControlados(docid)
    End Sub
#End Region

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsultar.Click
        SelUser.CargarGrid(1, txtQuery.Text)
    End Sub
End Class