Imports System.Data.Entity.Core.Objects

'Imports CoreProject.OP_Model
Imports System.Data.SqlClient

Namespace OP
    Public Class IPSClass
       #Region "Variables Globales"
        Private MatrixContext As IPSEntities
#End Region
#Region "Constructores"
        'Constructor public 
        Public Sub New()
            MatrixContext = New IPSEntities
        End Sub
#End Region

#Region "Obtener"
        Public Function TraerIPSRevision(ByVal ID As Int64, ByVal TrabajoId As Int64) As List(Of OP_IPS_Revision_Get_Result)
            Return MatrixContext.OP_IPS_Revision_Get(ID, TrabajoId).ToList()
        End Function

        Public Function TraerIPSRevisionRegistro(ByVal id As Int64) As OP_IPS_Revision_Get_Result
            Return MatrixContext.OP_IPS_Revision_Get(id, Nothing).FirstOrDefault
        End Function

        Public Function TraerIPSRevisionRegistroTabla(ByVal id As Int64) As OP_IPS_Revision
            Return MatrixContext.OP_IPS_Revision.Where(Function(x) x.id = id).FirstOrDefault
        End Function

        Public Function TraerIPSRevisionTabla(ByVal TrabajoID As Int64) As List(Of OP_IPS_Revision)
            Return MatrixContext.OP_IPS_Revision.Where(Function(x) x.TrabajoId = TrabajoID).ToList.OrderBy(Function(y) y.Pregunta)
        End Function

        Public Function Suma(numero1 As Int16, numero2 As Int16) As Int16
            Return numero1 + numero2
        End Function

        Public Function NombreAplicacion() As String
            Return "Matrix"
        End Function

#End Region

#Region "Guardar"
        Sub GuardarRegistroRevision(ByVal TrabajoId As Int64, Pregunta As String, Observacion As String, Descripcion As String, Respuesta As String)
            MatrixContext.OP_IPS_Revision_Add(TrabajoId, Pregunta, Observacion, Descripcion, Respuesta)
        End Sub

        Sub GuardarRegistroRevisionEntidad(ByVal TrabajoId As Int64, Pregunta As String, Observacion As String, Descripcion As String, Respuesta As String)
            Dim e As New OP_IPS_Revision
            e.TrabajoId = TrabajoId
            e.Pregunta = Pregunta
            e.Observacion = Observacion
            e.DescripcionObservacion = Descripcion
            e.RespuestaProgramador = Respuesta
            MatrixContext.OP_IPS_Revision.Add(e)
            MatrixContext.SaveChanges()
        End Sub

        Sub GuardarRegistroEntidad(ByRef e As OP_IPS_Revision)
            If e.id = 0 Then
                MatrixContext.OP_IPS_Revision.Add(e)
            End If
            MatrixContext.SaveChanges()
        End Sub

#End Region
    End Class
End Namespace