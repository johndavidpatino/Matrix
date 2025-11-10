'Imports CoreProject
'Imports CoreProject.IQ_ENTITIES

Namespace IQ



    Public Class UCPreguntas
        Private _IQ As New IQ_MODEL
        Public Producto As IQ_Productos

        Public Function ObtenerPreguntasProducto(ByVal Producto As IQ_Productos) As IQ_Productos

            Try
                Producto = (From p In _IQ.IQ_Productos Where p.Pr_ProductCode = Producto.Pr_ProductCode Select p).First()
                Return Producto
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerProductosPorUnidad(ByVal oferta As String) As List(Of IQ_Productos)

            Try
                Dim lstProds As New List(Of IQ_Productos)

                lstProds = (From p In _IQ.IQ_Productos Where p.Pr_Offeringcode = oferta Select p).ToList()
                lstProds.Insert(0, New IQ_Productos With {.Pr_ProductCode = "0", .Pr_ProductDescription = "Seleccione..."})
                Return lstProds
            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Public Function ObtenerOferta(ByVal unidad As Integer) As List(Of ObtenerOfertas_Result)

            Try

                Dim lstOfertas As New List(Of ObtenerOfertas_Result)
                lstOfertas = (From o In _IQ.ObtenerOfertas(unidad) Select o).ToList()
                lstOfertas.Insert(0, New ObtenerOfertas_Result With {.Pr_Offeringcode = "0", .Pr_OfferingDescription = "Seleccionar..."})
                Return lstOfertas

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Sub llenarPreguntas()




        End Sub

        Public Function CalcularTiempo(ByVal PregCerradas As Integer, ByVal PregCerradasMultiples As Integer, ByVal PregAbiertas As Integer, ByVal PregAbiertasMultiples As Integer, ByVal PregOtras As Integer, ByVal PregDemograficos As Integer) As Integer
            Try
                Dim duracion As Integer

                duracion = ((PregCerradas + (PregCerradasMultiples / 10)) + PregAbiertas + PregAbiertasMultiples + PregOtras + PregDemograficos) / 6

                Return duracion
            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Function TraerPreguntas(ByVal idpropuesta As Int64, alternativa As Int32, metodologia As Int32, fase As Int32) As IQ_Preguntas
            Dim _IQ_Entities = New IQ_MODEL
            Return _IQ_Entities.IQ_Preguntas.Where(Function(x) x.IdPropuesta = idpropuesta AndAlso x.ParAlternativa = alternativa AndAlso x.MetCodigo = metodologia AndAlso x.ParNacional = fase).FirstOrDefault
        End Function




        Public Function ObtenerPreguntasHistoricos(jb As String, unidad As Integer, producto As String, Nombre As String) As List(Of iq_obtenerHistorialPreguntas_result)

            Return _IQ.IQ_ObtenerHistorialPreguntas(producto, jb, unidad, Nombre).ToList()
        End Function
    End Class

    
End Namespace

