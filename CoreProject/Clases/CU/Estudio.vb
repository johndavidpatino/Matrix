
'Imports CoreProject.CU_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Estudio
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
    Public Function Todos() As List(Of CU_Estudios_Get_Result)
        Try
            Dim oResult As List(Of CU_Estudios_Get_Result)
            oResult = CU_Estudios_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            Return oResult
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function ObtenerXID(ByVal Id As Int64) As CU_Estudios
        Return oMatrixContext.CU_Estudios.Where(Function(x) x.id = Id).FirstOrDefault()

    End Function
    Public Function obtenerTodosCampos(ByVal idGerenteCuentas As Int64, ByVal todosCampos As String) As List(Of CU_Estudios_Get_Result)
        Return CU_Estudios_Get(Nothing, Nothing, Nothing, idGerenteCuentas, Nothing, Nothing, todosCampos)
    End Function
    Private Function CU_Estudios_Get(ByVal ID As Int64?, ByVal propuestaId As Int64?, ByVal jobBook As String, ByVal gerenteCuentas As Int64?, ByVal nombre As String, ByVal valor As Double?, ByVal todosCampos As String) As List(Of CU_Estudios_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_Estudios_Get_Result) = oMatrixContext.CU_Estudios_Get(ID, propuestaId, jobBook, gerenteCuentas, nombre, valor, todosCampos)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function ObtenerXIdPropuesta(ByVal IdPropuesta As Int64) As List(Of CU_Estudios_Get_Result)
        Return CU_Estudios_Get(Nothing, IdPropuesta, Nothing, Nothing, Nothing, Nothing, Nothing)
    End Function
    Public Function ObtenerXIdGerenteCuentas(ByVal idGerenteCuentas As Int64) As List(Of CU_Estudios_Get_Result)
        Return CU_Estudios_Get(Nothing, Nothing, Nothing, idGerenteCuentas, Nothing, Nothing, Nothing)
    End Function
    Public Function ObtenerConsultaEstudios(ByVal ID As Int64?, ByVal propuestaId As Int64?, ByVal jobBook As String, ByVal nombre As String) As List(Of CU_Estudios_Get_Result)
        Return CU_Estudios_Get(ID, propuestaId, jobBook, Nothing, nombre, Nothing, Nothing)
    End Function

    Public Function ObtenerTrabajos(ByVal id As Int64?, NombreTrabajo As String, Estado As Int64?, JobBook As String, ProyectoId As Int64?, COE As Int64?, GerenteCuentas As Int64?, Unidad As Int64?, Gerencia As Int64?, Propuesta As Int64?, Estudio As Int64?) As List(Of CU_Trabajos_Get_Result)
        Return oMatrixContext.CU_Trabajos_Get(id, Estado, NombreTrabajo, JobBook, ProyectoId, COE, GerenteCuentas, Unidad, Gerencia, Propuesta, Estudio).ToList
    End Function

    Public Function ObtenerTrabajos(ByVal id As Int64?, NombreTrabajo As String, Estado As Int64?, Estudio As Int64?) As List(Of CU_Trabajos_Get_Result)
        Return oMatrixContext.CU_Trabajos_Get(id, Estado, NombreTrabajo, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Estudio).ToList
    End Function

    Public Function ObtenerDocumentosSoporte() As List(Of CU_Estudios_DocumentosSoporte)
        Return oMatrixContext.CU_Estudios_DocumentosSoporte.ToList
    End Function
#End Region
#Region "Guardar"
    Public Function Guardar(ByVal idEstudio As Int64?, ByVal JobBook As String, ByVal PropuestaId As Int64?, ByVal GerenteCuentas As Int64?, ByVal Nombre As String, ByVal Valor As Decimal, ByVal observaciones As String, ByVal fechaInicio As Date?, ByVal fechaTerminacion As Date?, ByVal anticipo As Int16, ByVal saldo As Int16, ByVal plazo As Int16) As Decimal
        Try
            Dim estudioId As Decimal = 0

            If idEstudio.HasValue Then
                oMatrixContext.CU_Estudios_Add(JobBook, PropuestaId, GerenteCuentas, Nombre, Valor, observaciones, fechaInicio, fechaTerminacion, anticipo, saldo, plazo)
                estudioId = idEstudio
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.CU_Estudios_Add(JobBook, PropuestaId, GerenteCuentas, Nombre, Valor, observaciones, fechaInicio, fechaTerminacion, anticipo, saldo, plazo)
                estudioId = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return estudioId
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Sub GuardarEstudio(ByRef e As CU_Estudios)
        If e.id = 0 Then
            oMatrixContext.CU_Estudios.Add(e)
        End If
        oMatrixContext.SaveChanges()
    End Sub

#End Region
End Class
