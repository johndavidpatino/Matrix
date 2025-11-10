

''Imports CoreProject.CU_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Pqr
    '        Private cuContext As New CU_Entities

    '#Region "GestionPQR"
    '        Public Function EditarPQRSa(ByVal id As Integer, ByVal Situacion As String, ByVal Accion As String) As Integer
    '            Try
    '                Try
    '                    Return cuContext.CU_PQR_EditSA(id, Situacion, Accion)
    '                Catch ex As SqlException
    '                    Throw ex
    '                End Try
    '            Catch ex As Exception
    '                If IsNothing(ex.InnerException) Then
    '                    Throw ex
    '                Else
    '                    Throw ex.InnerException
    '                End If
    '            End Try
    '        End Function

    '        Public Function ObtenerPQRAbiertas(ByVal EstudioId As Integer) As List(Of CU_PQR_GET_Abiertas_Result)
    '            Try
    '                Try
    '                    Dim List As ObjectResult(Of CU_PQR_GET_Abiertas_Result)
    '                    'ingresarDocMaestro.GD_MaestroDocumentos_Add(nomDoc, controlado, activo, codigoDoc, idProceso, responsable)
    '                    List = cuContext.CU_PQR_GET_Abiertas(EstudioId)
    '                    'devuelvo el ultimo id ingresado en la tabla de Actas
    '                    Return List.ToList()
    '                Catch ex As SqlException
    '                    Throw ex
    '                End Try
    '            Catch ex As Exception
    '                If IsNothing(ex.InnerException) Then
    '                    Throw ex
    '                Else
    '                    Throw ex.InnerException
    '                End If
    '            End Try
    '        End Function

    '        Public Function AgregarPQRActa(ByVal idPQR As Integer, ByVal IdActa As Integer) As Integer
    '            Try
    '                Try
    '                    Return cuContext.CU_PQRActas_Add(idPQR, IdActa)
    '                Catch ex As SqlException
    '                    Throw ex
    '                End Try
    '            Catch ex As Exception
    '                If IsNothing(ex.InnerException) Then
    '                    Throw ex
    '                Else
    '                    Throw ex.InnerException
    '                End If
    '            End Try
    '        End Function
    '#End Region


    '        Public Function ObtenerPQRxEstudioId(ByVal Estudioid As Integer) As List(Of CU_PQR_GET_Result)
    '            Try
    '                Try
    '                    Dim List As ObjectResult(Of CU_PQR_GET_Result) = cuContext.CU_PQR_GET(Estudioid)
    '                    Return List.ToList()
    '                Catch ex As SqlException
    '                    Throw ex
    '                End Try
    '            Catch ex As Exception
    '                If IsNothing(ex.InnerException) Then
    '                    Throw ex
    '                Else
    '                    Throw ex.InnerException
    '                End If
    '            End Try
    '        End Function


    '        Public Function EliminarPQR(ByVal idPQR As Integer) As Integer

    '            Try
    '                Try
    '                    Return cuContext.CU_PQR_DELETE(idPQR)
    '                Catch ex As Exception
    '                    Throw ex
    '                End Try
    '            Catch ex As Exception
    '                If (IsNothing(ex.InnerException)) Then
    '                    Throw ex
    '                Else
    '                    Throw (ex.InnerException)
    '                End If
    '            End Try
    '        End Function

    '        Public Function GuardarPQR(ByVal fecPQR As DateTime, ByVal contactoId As Integer, ByVal estudioId As Integer,
    '                                   ByVal desc As String, ByVal accinmediata As String, ByVal funcionarioRec As Integer, ByVal funcdesignado As Integer,
    '                                   ByVal resImplementacion As Integer, ByVal fecimplepropuesta As Date
    '                                   ) As Integer

    '            Try
    '                Try
    '                    Return cuContext.CU_PQR_ADD(fecPQR, contactoId, estudioId, desc, accinmediata, funcionarioRec, funcdesignado, resImplementacion, fecimplepropuesta)
    '                Catch ex As Exception
    '                    Throw ex
    '                End Try
    '            Catch ex As Exception
    '                If (IsNothing(ex.InnerException)) Then
    '                    Throw ex
    '                Else
    '                    Throw (ex.InnerException)
    '                End If
    '            End Try

    '        End Function

    '        Public Function EditarPQR(ByVal id As Integer,
    '                                  ByVal fecPQR As DateTime, ByVal contactoId As Integer, ByVal estudioId As Integer,
    '                                   ByVal desc As String, ByVal accinmediata As String, ByVal funcionarioRec As Integer,
    '                                   ByVal funcdesignado As Integer,
    '                                   ByVal resImplementacion As Integer, ByVal fecimplepropuesta As Date) As Integer
    '            Try
    '                Try
    '                    Return cuContext.CU_PQR_EDIT(id, fecPQR, contactoId, estudioId, desc, accinmediata, funcionarioRec, funcdesignado, resImplementacion, fecimplepropuesta)
    '                Catch ex As SqlException
    '                    Throw ex
    '                End Try
    '            Catch ex As Exception
    '                If IsNothing(ex.InnerException) Then
    '                    Throw ex
    '                Else
    '                    Throw ex.InnerException
    '                End If
    '            End Try
    '        End Function

    '        Public Function CerrarPQR(ByVal idPQR As Integer, ByVal fechaCierre As Date, ByVal resPQR As String, ByVal FunCierre As Integer) As Integer

    '            Try
    '                Try
    '                    Return cuContext.CU_PQR_CERRAR(idPQR, fechaCierre, resPQR, funCierre)
    '                Catch ex As Exception
    '                    Throw ex
    '                End Try
    '            Catch ex As Exception
    '                If (IsNothing(ex.InnerException)) Then
    '                    Throw ex
    '                Else
    '                    Throw (ex.InnerException)
    '                End If
    '            End Try
    '        End Function

    '        Public Function ObtenerCorreoContacto(ByVal idCon As Integer) As List(Of CU_Contactos_GetMail_Result)
    '            Try
    '                Try
    '                    Dim List As ObjectResult(Of CU_Contactos_GetMail_Result) = cuContext.CU_Contactos_GetMail(idCon)
    '                    Return List.ToList()
    '                Catch ex As SqlException
    '                    Throw ex
    '                End Try
    '            Catch ex As Exception
    '                If IsNothing(ex.InnerException) Then
    '                    Throw ex
    '                Else
    '                    Throw ex.InnerException
    '                End If
    '            End Try
    '        End Function

    '        Public Function ObtenerCorreoUsuario(ByVal idUsu As Integer) As List(Of US_Usuarios_GetMail_Result)
    '            Try
    '                Try
    '                    Dim List As ObjectResult(Of US_Usuarios_GetMail_Result) = cuContext.US_Usuarios_GetMail(idUsu)
    '                    Return List.ToList()
    '                Catch ex As SqlException
    '                    Throw ex
    '                End Try
    '            Catch ex As Exception
    '                If IsNothing(ex.InnerException) Then
    '                    Throw ex
    '                Else
    '                    Throw ex.InnerException
    '                End If
    '            End Try
    '        End Function

    '        Public Function ObtenerContactos() As List(Of CU_CONTACTOS_PQR_GET_Result)
    '            Try
    '                Try
    '                    Dim List As ObjectResult(Of CU_CONTACTOS_PQR_GET_Result) = cuContext.CU_CONTACTOS_PQR_GET()
    '                    Return List.ToList()
    '                Catch ex As SqlException
    '                    Throw ex
    '                End Try
    '            Catch ex As Exception
    '                If IsNothing(ex.InnerException) Then
    '                    Throw ex
    '                Else
    '                    Throw ex.InnerException
    '                End If
    '            End Try
    '        End Function
#Region "Variables Globales"
    Private oMatrixContext As CU_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CU_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function DevolverTodos() As List(Of CU_PQR_Get_Result)
        Try
            Return CU_PQR_Get(Nothing, Nothing, Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As CU_PQR_Get_Result
        Try
            Dim oResult As List(Of CU_PQR_Get_Result)
            oResult = CU_PQR_Get(ID, Nothing, Nothing, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxFuncionarioRecibeID(ByVal FuncionarioID As Int64) As List(Of CU_PQR_Get_Result)
        Try
            Return CU_PQR_Get(Nothing, FuncionarioID, Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxFuncionarioDesignadoID(ByVal FuncionarioID As Int64) As List(Of CU_PQR_Get_Result)
        Try
            Return CU_PQR_Get(Nothing, Nothing, FuncionarioID, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxFuncionarioCierreID(ByVal FuncionarioID As Int64) As List(Of CU_PQR_Get_Result)
        Try
            Return CU_PQR_Get(Nothing, Nothing, Nothing, FuncionarioID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function CU_PQR_Get(ByVal ID As Int64, ByVal FuncionarioRecibeID As Int64, ByVal FuncionarioDesignadoID As Int64, ByVal FuncionarioCierreID As Int64) As List(Of CU_PQR_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_PQR_Get_Result) = oMatrixContext.CU_PQR_Get(ID, FuncionarioRecibeID, FuncionarioDesignadoID, FuncionarioCierreID)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

#End Region
#Region "Guardar"
    Public Function Guardar(ByVal ID As Int64?, ByVal Fecha As DateTime?, ByVal EstablecidaPor As String, ByVal Empresa As String, ByVal Descripcion As String, ByVal AccionInmediata As String, ByVal FuncionarioRecibe As Int64?, ByVal FuncionarioDesignado As Int64?, ByVal Situacion As String, ByVal Accion As String) As Decimal
        Try
            Dim PQRID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.CU_PQR_Edit(ID, Fecha, EstablecidaPor, Empresa, Descripcion, AccionInmediata, FuncionarioRecibe, FuncionarioDesignado, Situacion, Accion)
                PQRID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.CU_PQR_Add(Fecha, EstablecidaPor, Empresa, Descripcion, AccionInmediata, FuncionarioRecibe, FuncionarioDesignado, Situacion, Accion)
                PQRID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return PQRID
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Sub Cerrar(ByVal ID As Int64?, ByVal FechaCierre As DateTime?, ByVal FuncionarioCierre As Int64?, ByVal RespuestaPQR As String)
        Try
            If ID > 0 Then
                oMatrixContext.CU_PQR_Cerrar(ID, FechaCierre, FuncionarioCierre, RespuestaPQR)
            End If
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Sub
#End Region
#Region "Eliminar"
    Public Function Eliminar(ByVal ID As Int64) As Integer
        Try
            Return oMatrixContext.CU_PQR_Del(ID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region
End Class




