
'Imports CoreProject.CU_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Auxiliares
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
    Public Function DevolverPaises() As List(Of CU_Paises_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_Paises_Get_Result) = oMatrixContext.CU_Paises_Get()
            Return oResult.ToList
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverDepartamentos(ByVal IdPais As Int32) As List(Of CU_Departamentos_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_Departamentos_Get_Result) = oMatrixContext.CU_Departamentos_Get(IdPais)
            Return oResult.ToList
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverCiudades(ByVal IdDepartamento As Int32) As List(Of CU_Ciudades_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_Ciudades_Get_Result) = oMatrixContext.CU_Ciudades_Get(IdDepartamento)
            Return oResult.ToList
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverNombreCiudad(ByVal IdCiudad As Int32) As String
        Return oMatrixContext.C_Divipola.Where(Function(x) x.DivMunicipio = IdCiudad).FirstOrDefault.DivMuniNombre
    End Function
    Public Function DevolverTipoCliente() As List(Of CU_TipoCliente_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_TipoCliente_Get_Result) = oMatrixContext.CU_TipoCliente_Get
            Return oResult.ToList
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverSectores() As List(Of CU_Sectores_GET_Result)
        Try
            Dim oResult As ObjectResult(Of CU_Sectores_GET_Result) = oMatrixContext.CU_Sectores_GET
            Return oResult.ToList
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverTipoPropuesta() As List(Of CU_TipoPropuesta_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_TipoPropuesta_Get_Result) = oMatrixContext.CU_TipoPropuesta_Get
            Return oResult.ToList
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverProbabilidadAprobacion() As List(Of CU_ProbabilidadAprobacion_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_ProbabilidadAprobacion_Get_Result) = oMatrixContext.CU_ProbabilidadAprobacion_Get
            Return oResult.ToList
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverEstadoPropuesta() As List(Of CU_EstadoPropuesta_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_EstadoPropuesta_Get_Result) = oMatrixContext.CU_EstadoPropuesta_Get
            Return oResult.ToList
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverOrigenPropuesta() As List(Of CU_OrigenPropuesta_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_OrigenPropuesta_Get_Result) = oMatrixContext.CU_OrigenPropuesta_Get
            Return oResult.ToList
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverRazonesNoAprobacion() As List(Of CU_RazonesNoAprobacion_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_RazonesNoAprobacion_Get_Result) = oMatrixContext.CU_RazonesNoAprobacion_Get
            Return oResult.ToList
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
