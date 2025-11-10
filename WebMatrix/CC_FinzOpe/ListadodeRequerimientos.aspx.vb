Imports WebMatrix.Util
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports CoreProject
Public Class ListadodeRequerimientos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If Not Request.QueryString("Tipo") Is Nothing Then
                hfidTipo.Value = Request.QueryString("Tipo").ToString
                If Not Request.QueryString("Id") Is Nothing Then hfidcedula.Value = Request.QueryString("Id").ToString
                If Not Request.QueryString("TrabajoId") Is Nothing Then hfIdTrabajo.Value = Request.QueryString("TrabajoId").ToString
                If Not Request.QueryString("JobBook") Is Nothing Then hfJobBook.Value = Request.QueryString("JobBook").ToString
                If Not Request.QueryString("Contratista") Is Nothing Then hfContratista.Value = Replace(Request.QueryString("Contratista").ToString, "&", "Y")
            End If
            'If Not Request.QueryString("IdMuestra") Is Nothing Then
            '    hfidMuestra.Value = Request.QueryString("IdMuestra").ToString
            'End If
        End If
        If hfidTipo.Value = 1 Then
            CargarpdfTipo1(hfIdTrabajo.Value)
        ElseIf hfidTipo.Value = 2 Then
            CargarpdfTipo2()
        ElseIf hfidTipo.Value = 3 Then
            CargarpdfTipo3(hfidcedula.Value)
        ElseIf hfidTipo.Value = 4 Then
            Cargarpdftipo4(hfJobBook.Value, hfContratista.Value)
        ElseIf hfidTipo.Value = 5 Then
            cargarpdftipo5(hfidcedula.Value)
        ElseIf hfidTipo.Value = 6 Then
            cargarpdftipo6(hfidcedula.Value, hfIdTrabajo.Value)
        ElseIf hfidTipo.Value = 7 Then
            cargarpdftipo7(hfidcedula.Value)
        End If

    End Sub


    Sub CargarpdfTipo1(ByVal trabajo As Int64)
        Response.Redirect("..\Requisiciones\" & trabajo & "-" & Now.Year & "-" & Now.Month & "-" & Now.Day & ".pdf")
    End Sub
    Sub CargarpdfTipo2()
        Response.Redirect("..\CuentasdeCobro\" & "CuentadeCobro.pdf")
    End Sub
    Sub CargarpdfTipo3(ByVal Cedula As Int64)
        Response.Redirect("..\CuentasdeCobro\" & "CuentadeCobro-Bonificacion-" & Cedula & ".pdf")
    End Sub

    Sub Cargarpdftipo4(ByVal JobBook As String, ByVal Contratista As String)
        Response.Redirect("..\OrdenesdeServicio\" & "OrdendeServicio-" & JobBook & "-" & Replace(Contratista, "&", "Y") & ".pdf")
    End Sub

    Sub cargarpdftipo5(ByVal Cedula As Int64)
        Response.Redirect("..\ResumenProduccion\" & "ResumenProduccion-" & Cedula & ".pdf")
    End Sub

    Sub cargarpdftipo6(ByVal Id As Int64, ByVal TrabajoId As Int64)
        Response.Redirect("..\PlanillaCapacitacion\" & "PlanillaCapacitacion-" & Id & "-" & TrabajoId & ".pdf")
    End Sub

    Sub cargarpdftipo7(ByVal nombre As String)
        Response.Redirect("..\ResumenProduccion\" & nombre)
    End Sub

End Class