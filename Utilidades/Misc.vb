Imports System.Security.Cryptography
Imports System.Text

Public Class Misc
    Public Shared Function ObtenerNombreDeUsuarioConDominio() As String
        Return Security.Principal.WindowsIdentity.GetCurrent().Name()
    End Function

    Public Shared Function ObtenerNombreDeMaquina() As String
        Return Environment.MachineName
    End Function

    Public Shared Function ObtenerMD5(ByVal entrada As String) As String
        ' Create a new instance of the MD5 object.
        Dim md5Hasher As MD5 = MD5.Create()

        ' Convert the input string to a byte array and compute the hash.
        Dim data As Byte() = md5Hasher.ComputeHash(System.Text.Encoding.Default.GetBytes(entrada))

        ' Create a new Stringbuilder to collect the bytes
        ' and create a string.
        Dim sBuilder As New Text.StringBuilder()

        ' Loop through each byte of the hashed data 
        ' and format each one as a hexadecimal string.
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i

        ' Return the hexadecimal string.
        Return sBuilder.ToString()

    End Function

    Public Shared Function StrToByteArray(ByVal str As String, ByVal codificacion As System.Text.UTF8Encoding) As Byte()
        'Dim encoding As New System.Text.UTF8Encoding()
        Return codificacion.GetBytes(str)
    End Function 'StrToByteArray

    Public Shared Function Encriptar(ByVal texto As String) As String
        Dim key As String = "ABCDEFGHIJKLMiOPQRSTUVWXYZabcdefghijklmniopqrstuvwxyz"
        'arreglo de bytes donde guardaremos la llave
        Dim keyArray() As Byte
        'arreglo de bytes donde guardaremos el texto que vamos a encriptar
        Dim Arreglo_a_Cifrar() As Byte = UTF8Encoding.UTF8.GetBytes(texto)
        'se utilizan las clases de encriptacion proveidas por el Framework
        'Algritmo MD5
        Dim hashmd5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
        'se guarda la llave para que se le realice hashing
        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key))
        hashmd5.Clear()
        'Algoritmo 3DAS
        Dim tdes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider
        tdes.Key = keyArray
        tdes.Mode = CipherMode.ECB
        tdes.Padding = PaddingMode.PKCS7
        'se empieza con la transformaion de la cadena
        Dim cTransform As ICryptoTransform = tdes.CreateEncryptor
        'arreglo de bytes donde se guarda la cadena cifrada
        Dim ArrayResultado() As Byte = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length)
        tdes.Clear()
        'se regresa el resultado en forma de una cadena
        Return Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length)
    End Function

    Public Shared Function Desencriptar(ByVal textoEncriptado As String) As String
        Dim key As String = "ABCDEFGHIJKLMiOPQRSTUVWXYZabcdefghijklmniopqrstuvwxyz"
        Dim keyArray() As Byte
        'convierte el texto en una secuencia de bytes
        Dim Array_a_Descifrar() As Byte = Convert.FromBase64String(textoEncriptado)
        'se llama a las clases ke tienen los algoritmos de encriptacion
        'se le aplica hashing
        Dim hashmd5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key))
        hashmd5.Clear()
        Dim tdes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider
        tdes.Key = keyArray
        tdes.Mode = CipherMode.ECB
        tdes.Padding = PaddingMode.PKCS7
        Dim cTransform As ICryptoTransform = tdes.CreateDecryptor
        Dim resultArray() As Byte = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length)
        tdes.Clear()
        Dim res As String = UTF8Encoding.UTF8.GetString(resultArray)
        Return res
    End Function

End Class
