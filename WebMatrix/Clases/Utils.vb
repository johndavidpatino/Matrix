Imports Microsoft.VisualBasic
Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class Util

    Shared oUtil As New Util

    Public Shared Function GetSizeMB(ByVal sizeKB As Double) As String
        Dim sizeMB As Double = 0
        sizeMB = sizeKB / 1024

        If sizeMB <= 1024 Then
            Return FormatNumber(sizeMB, 0).ToString & " KB"
        Else
            Return FormatNumber(sizeMB, 0).ToString & " MB"
        End If

    End Function

    Public Shared Sub ShowWindows(ByVal url As String)
        Dim mePage As Page = CType(HttpContext.Current.Handler, Page)
        Dim jsString As String = "popup('" & url & "', 'Reporte', '800', '600');"
        ScriptManager.RegisterStartupScript(mePage, oUtil.GetType(), "Ventana", jsString, True)
    End Sub

    Public Shared Sub ActivateAccordion(ByVal i As Integer, ByVal animatedEffect As EffectActivateAccordion)
        Dim mePage As Page = CType(HttpContext.Current.Handler, Page)
        ScriptManager.RegisterStartupScript(mePage, oUtil.GetType(), "accordionActivate", "$('#accordion').accordion({ header: 'h3',  autoHeight: false, animated: " & animatedEffect.EffetcActivateAccordionString & "} );  $('#accordion').accordion('activate', " & i.ToString() & "); ", True)
    End Sub

    Public Shared Sub ActivateAccordion(ByVal i As Integer)
        Dim mePage As Page = CType(HttpContext.Current.Handler, Page)
        ScriptManager.RegisterStartupScript(mePage, oUtil.GetType(), "accordionActivate", "$('#accordion').accordion({ header: 'h3',  autoHeight: false } );  $('#accordion').accordion('activate', " & i.ToString() & "); ", True)
    End Sub

    Public Shared Function InsertarItemSeleccion() As ListItem
        Dim oItem As New ListItem
        oItem.Value = "-1"
        oItem.Text = "-- Seleccione --"
        Return oItem
    End Function

    Public Shared Sub ChangeTab(ByVal i As Integer, Optional ByVal nombreTab As String = "tabs")
        Dim mePage As Page = CType(HttpContext.Current.Handler, Page)
        Dim db As New StringBuilder
        db.AppendLine("$('#" + nombreTab + "').tabs();")
        db.AppendLine("$('#" + nombreTab + "').tabs('select'," & i.ToString() & ");")
        ScriptManager.RegisterStartupScript(mePage, oUtil.GetType(), "changeTab", db.ToString(), True)
    End Sub

    Public Shared Sub ShowNotificationUtil(ByVal control As Label, ByVal notificationText As String, ByVal tipoNotification As ShowNotifications)
        Select Case tipoNotification
            Case ShowNotifications.ErrorNotification
                control.Text = notificationText

            Case ShowNotifications.InfoNotification
                control.Text = notificationText

        End Select

        ScriptManager.RegisterStartupScript(control.Page, oUtil.GetType(), "notification", tipoNotification.NotificationString, True)
    End Sub

    Public Shared Sub ShowNotification(ByVal notificationText As String, ByVal tipoNotification As ShowNotifications)

        Dim mePage As Page = CType(HttpContext.Current.Handler, Page)
        Dim jsScript As New StringBuilder

        Select Case tipoNotification
            Case ShowNotifications.ErrorNotification
                jsScript.AppendLine("$('#lbltextError').text('" & notificationText & "');")

            Case ShowNotifications.InfoNotification
                jsScript.AppendLine("$('#lblTextInfo').text('" & notificationText & "');")

            Case ShowNotifications.ErrorNotificationLong
                jsScript.AppendLine("$('#lbltextError').text('" & notificationText & "');")

            Case ShowNotifications.InfoNotificationLong
                jsScript.AppendLine("$('#lblTextInfo').text('" & notificationText & "');")

        End Select

        jsScript.AppendLine(tipoNotification.NotificationString)

        ScriptManager.RegisterStartupScript(mePage.Page, oUtil.GetType(), "notification", jsScript.ToString(), True)
    End Sub

    Public Shared Function ValidarSesionActiva(ByVal nombreSesion As String) As Boolean
        Try
            If HttpContext.Current.Session("InfoUsuario") IsNot Nothing Then
                Return True
            Else
                FormsAuthentication.SignOut()
                FormsAuthentication.RedirectToLoginPage()
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function obtenerUrlRaiz() As String
        Return HttpContext.Current.Request.Url.Scheme & HttpContext.Current.Request.Url.SchemeDelimiter & HttpContext.Current.Request.Url.Authority & HttpContext.Current.Request.ApplicationPath
    End Function

End Class

Public Module StringExtensions

    <Extension()>
    Public Function NotificationString(ByVal s As ShowNotifications) As String
        Dim fi As Reflection.FieldInfo = s.GetType().GetField(s.ToString())
        Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
        If aattr.Length > 0 Then
            Return aattr(0).Description
        Else
            Return s.ToString()
        End If
        Return s
    End Function

    <Extension()>
    Public Function EffetcActivateAccordionString(ByVal s As EffectActivateAccordion) As String
        Dim fi As Reflection.FieldInfo = s.GetType().GetField(s.ToString())
        Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
        If aattr.Length > 0 Then
            Return aattr(0).Description
        Else
            Return s.ToString()
        End If
        Return s
    End Function

End Module

Public Enum ShowNotifications
    <Description("$(function () { $('#info').hide(); $('#error').show('blind', {}, 500, function () { setTimeout(function () { $('#error:visible').removeAttr('style').hide('blind'); }, 5500); }); });")> ErrorNotification
    <Description("$(function () { $('#error').hide(); $('#info').show('blind', {}, 500, function () { setTimeout(function () { $('#info:visible').removeAttr('style').hide('blind'); }, 5500); }); });")> InfoNotification
    <Description("$(function () { $('#info').hide(); $('#error').show('blind', {}, 500, function () { setTimeout(function () { $('#error:visible').removeAttr('style').hide('blind'); }, 10000); }); });")> ErrorNotificationLong
    <Description("$(function () { $('#error').hide(); $('#info').show('blind', {}, 500, function () { setTimeout(function () { $('#info:visible').removeAttr('style').hide('blind'); }, 10000); }); });")> InfoNotificationLong
End Enum

Public Enum EffectActivateAccordion
    <Description("'slide'")> SlideEffect
    <Description("false")> NoEffect
End Enum


Public Class oJobBook
    Protected Friend Property IdBrief As Int64
    Protected Friend Property IdPropuesta As Int64
    Protected Friend Property IdEstudio As Int64
    Protected Friend Property Cliente As String
    Protected Friend Property Titulo As String
    Protected Friend Property MarcaCategoria As String
    Protected Friend Property Estado As String
    Protected Friend Property GerenteCuentas As String
    Protected Friend Property GerenteCuentasID As String
    Protected Friend Property Unidad As String
    Protected Friend Property IdUnidad As String
    Protected Friend Property NumJobBook As String
    Protected Friend Property Viabilidad As Nullable(Of Boolean)
    Protected Friend Property GuardarCambios As Boolean
    Protected Friend Property Alternativa As Int32
    Protected Friend ReviewOPS As Boolean = False
    Protected Friend ReviewFI As Boolean = False
End Class

Public Class oContenedorDocumento
    Protected Friend Property ContenedorId As Int64
    Protected Friend Property DocumentoId As Int64
End Class

Public Enum TypesWarning
    Warning = 1
    ErrorMessage = 2
    Information = 3
End Enum