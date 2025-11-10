Imports System.Security.Cryptography
Imports System.Text
Imports System.IO
Imports Microsoft.VisualBasic

'Breve configuración de la función Cifrado:
' •Modo: ◦1 es Encriptar;
' ◦2 es Desencriptar

'•Algoritmo: ◦1 es DES
' ◦2 es TripleDES
' ◦3 es RC2
' ◦4 es Rijndael

'•Cadena: Es la Cadena de Caracter a Cifrar.
' •Key: Clave Secreta
' •VecI: Vector de Inicialización

Public Class Encripcion
    Public Shared Function Cifrado(ByVal modo As Byte, ByVal Algoritmo As Byte, ByVal cadena As String, ByVal key As String, ByVal VecI As String) As String
        Dim plaintext() As Byte = Nothing
        If modo = 1 Then
            plaintext = Encoding.ASCII.GetBytes(cadena)
        ElseIf modo = 2 Then
            plaintext = Convert.FromBase64String(cadena)
        End If

        Dim keys() As Byte = Encoding.ASCII.GetBytes(key)
        Dim memdata As New MemoryStream
        Dim transforma As ICryptoTransform = Nothing


        Select Case Algoritmo
            Case 1
                Dim des As New DESCryptoServiceProvider ' DES
                des.Mode = CipherMode.CBC
                If modo = 1 Then
                    transforma = des.CreateEncryptor(keys, Encoding.ASCII.GetBytes(VecI))
                ElseIf modo = 2 Then
                    transforma = des.CreateDecryptor(keys, Encoding.ASCII.GetBytes(VecI))
                End If

            Case 2
                Dim des3 As New TripleDESCryptoServiceProvider 'TripleDES
                des3.Mode = CipherMode.CBC
                If modo = 1 Then
                    transforma = des3.CreateEncryptor(keys, Encoding.ASCII.GetBytes(VecI))
                ElseIf modo = 2 Then
                    transforma = des3.CreateDecryptor(keys, Encoding.ASCII.GetBytes(VecI))
                End If

            Case 3
                Dim rc2 As New RC2CryptoServiceProvider 'RC2
                rc2.Mode = CipherMode.CBC
                If modo = 1 Then
                    transforma = rc2.CreateEncryptor(keys, Encoding.ASCII.GetBytes(VecI))
                ElseIf modo = 2 Then
                    transforma = rc2.CreateDecryptor(keys, Encoding.ASCII.GetBytes(VecI))
                End If

            Case 4
                Dim rj As New RijndaelManaged 'Rijndael
                rj.Mode = CipherMode.CBC
                If modo = 1 Then
                    transforma = rj.CreateEncryptor(keys, Encoding.ASCII.GetBytes(VecI))
                ElseIf modo = 2 Then
                    transforma = rj.CreateDecryptor(keys, Encoding.ASCII.GetBytes(VecI))
                End If
            Case 5
                Dim keyFromBase64 = Convert.FromBase64String(key)
                Dim aes As New AesCryptoServiceProvider
                aes.Mode = CipherMode.ECB
                If modo = 1 Then
                    transforma = aes.CreateEncryptor(keyFromBase64, Nothing)
                ElseIf modo = 2 Then
                    transforma = aes.CreateDecryptor(keyFromBase64, Nothing)
                End If

        End Select

        Dim encstream As New CryptoStream(memdata, transforma, CryptoStreamMode.Write)

        encstream.Write(plaintext, 0, plaintext.Length)
        encstream.FlushFinalBlock()
        encstream.Close()

        If modo = 1 Then
            cadena = Convert.ToBase64String(memdata.ToArray)
        ElseIf modo = 2 Then
            cadena = Encoding.ASCII.GetString(memdata.ToArray)
        End If
        Return cadena 'Aquí Devuelve los Datos Cifrados

    End Function

End Class
