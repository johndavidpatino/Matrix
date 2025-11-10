Namespace FI
    Public Class Formatos
#Region "Variables Globales"
        Private oMatrixContext As FI_Entities
#End Region
#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New FI_Entities
        End Sub
#End Region
#Region "Obtener"
        Function ObtenerQuejasReclamosProveedores(id As Nullable(Of Long), proveedor As Nullable(Of Long), contacto As String, tipoEstudio As Nullable(Of Byte), noEstudio As String, nombreGerente As String, fechaImplementacion As Nullable(Of Date), satisfaccion As Nullable(Of Byte), fecha As Nullable(Of Date), usuario As Nullable(Of Long)) As List(Of FI_QuejasReclamosProveedores_Get_Result)
            Return oMatrixContext.FI_QuejasReclamosProveedores_Get(id, proveedor, contacto, tipoEstudio, noEstudio, nombreGerente, fechaImplementacion, satisfaccion, fecha, usuario).ToList
        End Function

        Function ObtenerEvaluacionProveedor(id As Integer) As FI_EvaluacionProveedores
            Return oMatrixContext.FI_EvaluacionProveedores.Where(Function(x) x.id = id).FirstOrDefault()
        End Function
#End Region
#Region "Guardar"
        Sub GuardarQuejasReclamosProveedores(ByRef oS As FI_QuejasReclamosProveedores)
            If oS.Id = 0 Then
                oMatrixContext.FI_QuejasReclamosProveedores.Add(oS)
            Else
                oMatrixContext.Entry(oS).State = Entity.EntityState.Modified
            End If
            oMatrixContext.SaveChanges()
        End Sub

        Sub GuardarEvaluacionProveedor(ByRef ent As FI_EvaluacionProveedores)
            oMatrixContext.Entry(ent).State = Entity.EntityState.Modified
            oMatrixContext.SaveChanges()
        End Sub
#End Region

#Region "Borrar"

#End Region
    End Class
End Namespace

