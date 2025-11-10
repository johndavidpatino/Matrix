Imports System.IO
Imports CoreProject
Public Class UsrCtrl_LoadFiles
    Inherits System.Web.UI.UserControl

    Dim ProcedimientoUpload As New CoreProject.UploadFile

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpload.Click
        Dim sExt As String = String.Empty
        Dim sName As String = String.Empty
        Dim idDoc As Integer
        Dim idUsu As Integer
        Dim idCon As Integer

        'Borramos mensajes
        lblMensaje2.Text = ""
        lbldoccheck.Text = ""
        lblusucheck.Text = ""
        If UpLoadFile.HasFile Then
            sName = UpLoadFile.FileName
            sExt = Path.GetExtension(sName)
            Dim sNameSeparado = sName.Split(".")
            Dim nombre = sNameSeparado(0)
            'Proceso de llenado
            If txtidDoc.Text = "" Then
                lbldoccheck.Text = "Campo Obligatorio"
                txtidDoc.Focus()
            Else
                idDoc = Integer.Parse(txtidDoc.Text)
                If txtIdUsu.Text = "" Then
                    lblusucheck.Text = "Campo Obligatorio"
                    txtIdUsu.Focus()
                Else
                    idUsu = Integer.Parse(txtIdUsu.Text)
                    If TxtIdCon.Text = "" Then
                        idCon = 0
                    Else
                        idCon = Integer.Parse(TxtIdCon.Text)
                    End If

                    Dim comentario As String = txtcoment.Text
                    Dim ruta As String = MapPath("~/uploads/" & sName).ToString
                    Try
                        ProcedimientoUpload.uploadFile(nombre, ruta, idDoc, comentario, idUsu, idCon)
                        UpLoadFile.SaveAs(MapPath("~/uploads/" & sName))
                        lblMensaje2.Text = "Archivo cargado correctamente."
                        txtidDoc.Text = ""
                        txtIdUsu.Text = ""
                        TxtIdCon.Text = ""
                        txtcoment.Text = ""
                    Catch ex As Exception
                        MsgBox(ex.ToString, MsgBoxStyle.Exclamation, "Error:")
                    End Try
                End If
            End If

        Else
            lblMensaje2.Text = "Seleccione el archivo que desea subir."
        End If

    End Sub
    'Metodo para validar el tipo de archivo recibido.
    Private Function ValidaExtension(ByVal sExtension As String) As Boolean
        Select Case sExtension
            Case ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".xls", ".doc", ".pdf", ".docx", ".xlsx", ".ppt", ".pptx"
                Return True
            Case Else
                Return False
        End Select
    End Function
End Class