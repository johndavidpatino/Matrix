Imports System.Data.Entity.Core.Objects

'Imports CoreProject.GD_Model
Imports System.Data.SqlClient

Namespace GD
    Public Class GD_Procedimientos

        Private gdContext As New GD_Entities


        Public Function ObtenerTipoSolicitud() As List(Of GD_TipoSolicitud_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GD_TipoSolicitud_Get_Result) = gdContext.GD_TipoSolicitud_Get
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ObtenerEstado() As List(Of GD_Estados_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GD_Estados_Get_Result) = gdContext.GD_Estados_Get
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ObtenerProcesos() As List(Of GD_Procesos_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GD_Procesos_Get_Result) = gdContext.GD_Procesos_Get
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ObtenerUsuarios() As List(Of GD_US_Usuarios_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GD_US_Usuarios_Get_Result) = gdContext.GD_US_Usuarios_Get
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ObtenerDocumentos() As List(Of GD_MaestroDocumentos_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GD_MaestroDocumentos_Get_Result) = gdContext.GD_MaestroDocumentos_Get
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function


        Public Function IngresarSolicitudDocumento(ByVal fechaSolicitud As DateTime, ByVal Solicitante As Integer,
                                   ByVal area As String, ByVal cargo As String,
                                   ByVal tipoSolicitud As Integer, ByVal DocumentoId As Integer,
                                   ByVal nomDocumento As String, ByVal codigoDoc As String,
                                   ByVal areaUso As String, ByVal sitioAcceso As String,
                                   ByVal razonSolicitud As String, ByVal descSolicitud As String,
                                   ByVal estadoId As Integer, ByVal fechaEstado As DateTime,
                                   ByVal comentarios As String, ByVal modificacion As String
                                   ) As Integer
            Dim ingresarSolDoc As New GD_Entities
            Try
                Try
                    Return ingresarSolDoc.GD_SolDocumentos_Add(fechaSolicitud, Solicitante, area, cargo, tipoSolicitud, DocumentoId, nomDocumento, codigoDoc, areaUso, sitioAcceso, razonSolicitud, descSolicitud, estadoId, fechaEstado, comentarios, modificacion)

                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function IngresarDocumentoMaestro2(ByVal nomDoc As String, ByVal controlado As Boolean,
                                    ByVal activo As Boolean, ByVal codigoDoc As String,
                                    ByVal idProceso As Integer, ByVal responsable As String) As Integer
            Try
                Try
                    Dim Res As ObjectResult(Of GD_MaestroDocumentos_Add2_Result)
                    'ingresarDocMaestro.GD_MaestroDocumentos_Add(nomDoc, controlado, activo, codigoDoc, idProceso, responsable)
                    Res = gdContext.GD_MaestroDocumentos_Add2(nomDoc, controlado, activo, codigoDoc, idProceso, responsable)
                    'devuelvo el ultimo id ingresado en la tabla de Actas
                    Return Res(0).ultimoId
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function IngresarDocumentoControlado(ByVal docId As Integer,
                                   ByVal activo As Boolean, ByVal ubiArchivo As String, ByVal metRecuperacion As String,
                                   ByVal tiempoRetención As String, ByVal dispoFinal As String) As Integer

            Dim ingresarDocControl As New GD_Entities
            Try
                Try
                    Return ingresarDocControl.GD_DocumentosControlados_Add(docId, False, ubiArchivo, metRecuperacion, tiempoRetención, dispoFinal)
                    'Return ingresarDocControl.GD_DocumentosControlados_Add(docId, False, ubiArchivo, metRecuperacion, tiempoRetención, dispoFinal).ToList(0).Id
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                If (IsNothing(ex.InnerException)) Then
                    Throw ex
                Else
                    Throw (ex.InnerException)
                End If
            End Try

        End Function

        Public Function DocMaestroActivo(ByVal docId As Integer) As Integer

            Dim docMaestroAct As New GD_Entities
            Try
                Try
                    Return docMaestroAct.GD_DocumentosMaestros_Update(docId)
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                If (IsNothing(ex.InnerException)) Then
                    Throw ex
                Else
                    Throw (ex.InnerException)
                End If
            End Try

        End Function

        Public Function DocControlados(ByVal docId As Integer) As Integer

            Dim docControl As New GD_Entities
            Try
                Try
                    Return docControl.GD_DocumentosControlados_Activo(docId)
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                If (IsNothing(ex.InnerException)) Then
                    Throw ex
                Else
                    Throw (ex.InnerException)
                End If
            End Try

        End Function

        Public Sub eliminarDocumentoEscaneado(ByVal Id As Int64?, ByVal IdDocumento As Int64?, ByVal IdTrabajo As Int64?)
            gdContext.GD_EscanerDocumentos_Del(IdTrabajo, Id, IdDocumento)
        End Sub


#Region "Revision"

        Public Function guardarRevision(ByVal DocumentoId As Integer, ByVal usuarioId As Integer, ByVal fechaAprobacion As DateTime, ByVal tipoRevision As Integer) As Integer
            Dim ingresarSolDoc As New GD_Entities
            Try
                Try
                    Return ingresarSolDoc.GD_Revisiones_Add(DocumentoId, usuarioId, fechaAprobacion, tipoRevision)

                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function editarRevision(ByVal revisionId As Integer, ByVal DocumentoId As Integer, ByVal usuarioId As Integer, ByVal fechaAprobacion As DateTime, ByVal tipoRevision As Integer) As Integer
            Dim ingresarSolDoc As New GD_Entities
            Try
                Try
                    Return ingresarSolDoc.GD_Revisiones_Edit(revisionId, DocumentoId, usuarioId, fechaAprobacion, tipoRevision)

                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ObtenerRevisionUsuario(ByVal Usuario As Integer) As List(Of GD_Revisiones_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GD_Revisiones_Get_Result) = gdContext.GD_Revisiones_Get(Usuario)
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ObtenerRevisionAprobarUsuario(ByVal Usuario As Integer) As List(Of GD_Revisiones_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GD_Revisiones_Get_Result) = gdContext.GD_Revisiones_GetRev(Usuario)
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function actualizarRevision(ByVal DocumentoId As Integer, ByVal usuarioId As Integer, ByVal fechaAprobacion As DateTime, ByVal tipoRevision As Integer) As Integer
            Dim ingresarSolDoc As New GD_Entities
            Try
                Try
                    Return ingresarSolDoc.GD_Revisiones_Add(DocumentoId, usuarioId, fechaAprobacion, tipoRevision)

                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

#End Region

#Region "Procesos"

        Public Function ObtenerProcesoxNombre(ByVal nomProceso As String) As List(Of GD_Procesos_Get_F_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GD_Procesos_Get_F_Result)
                    'ingresarDocMaestro.GD_MaestroDocumentos_Add(nomDoc, controlado, activo, codigoDoc, idProceso, responsable)
                    List = gdContext.GD_Procesos_Get_F(nomProceso)
                    'devuelvo el ultimo id ingresado en la tabla de Actas
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function EliminarProceso(ByVal idProceso As Integer) As Integer

            Try
                Try
                    Return gdContext.GD_Procesos_Del(idProceso)
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                If (IsNothing(ex.InnerException)) Then
                    Throw ex
                Else
                    Throw (ex.InnerException)
                End If
            End Try
        End Function

        Public Function GuardarProceso(ByVal nomProceso As String) As Integer

            Try
                Try
                    Return gdContext.GD_Procesos_Add(nomProceso)
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                If (IsNothing(ex.InnerException)) Then
                    Throw ex
                Else
                    Throw (ex.InnerException)
                End If
            End Try

        End Function

        Public Function EditarProceso(ByVal idProceso As Integer, ByVal Proceso As String) As Integer
            Try
                Try
                    Return gdContext.GD_Procesos_Edit(idProceso, Proceso)
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function


#End Region

#Region "Tipo Solicitud"

        Public Function ObtenerTipoSolicitudxNombre(ByVal nomSolicitud As String) As List(Of GD_TipoSolicitud_Get_F_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GD_TipoSolicitud_Get_F_Result)
                    List = gdContext.GD_TipoSolicitud_Get_F(nomSolicitud)
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function EliminarTipoSolicitud(ByVal idTipoSol As Integer) As Integer

            Try
                Try
                    Return gdContext.GD_TipoSolicitud_Del(idTipoSol)
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                If (IsNothing(ex.InnerException)) Then
                    Throw ex
                Else
                    Throw (ex.InnerException)
                End If
            End Try
        End Function

        Public Function GuardarTipoSolicitud(ByVal nomTipoSol As String) As Integer

            Try
                Try
                    Return gdContext.GD_TipoSolicitud_Add(nomTipoSol)
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                If (IsNothing(ex.InnerException)) Then
                    Throw ex
                Else
                    Throw (ex.InnerException)
                End If
            End Try

        End Function

        Public Function EditarTipoSolicitud(ByVal idTipoSol As Integer, ByVal nomTipoSol As String) As Integer
            Try
                Try
                    Return gdContext.GD_TipoSolicitud_Edit(idTipoSol, nomTipoSol)
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

#End Region

#Region "Estado solicitud"

        Public Function ObtenerEstadoSolicitudxNombre(ByVal nomEstado As String) As List(Of GD_EstadoSolicitud_Get_F_Result)
            Try
                Try
                    Dim List As ObjectResult(Of GD_EstadoSolicitud_Get_F_Result)
                    List = gdContext.GD_EstadoSolicitud_Get_F(nomEstado)
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function EliminarEstadoSolicitud(ByVal idEstadoSol As Integer) As Integer

            Try
                Try
                    Return gdContext.GD_EstadoSolicitud_Del(idEstadoSol)
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                If (IsNothing(ex.InnerException)) Then
                    Throw ex
                Else
                    Throw (ex.InnerException)
                End If
            End Try
        End Function

        Public Function GuardarEstadoSolicitud(ByVal nomEstadoSol As String) As Integer

            Try
                Try
                    Return gdContext.GD_EstadoSolicitud_Add(nomEstadoSol)
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                If (IsNothing(ex.InnerException)) Then
                    Throw ex
                Else
                    Throw (ex.InnerException)
                End If
            End Try

        End Function

        Public Function EditarEstadoSolicitud(ByVal idEstadoSol As Integer, ByVal nomEstadoSol As String) As Integer
            Try
                Try
                    Return gdContext.GD_EstadoSolicitud_Edit(idEstadoSol, nomEstadoSol)
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

#End Region

#Region "Documentos Maestros"

        Function obtenerDocumentoMaestroXId(ByVal idDocumento As Int64) As GD_GD_MaestroDocumentos_Get2_Result
            Return gdContext.GD_GD_MaestroDocumentos_Get2(idDocumento, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
        End Function

        Function obtenerDocumentosCierre() As List(Of GD_GD_MaestroDocumentos_Get2_Result)
            Return gdContext.GD_GD_MaestroDocumentos_Get2(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing, Nothing, Nothing, Nothing).ToList
        End Function

        Private Function GD_GD_MaestroDocumentos_Get2_Result(ByVal idDocumento As Nullable(Of Global.System.Int64), ByVal documento As Global.System.String, ByVal controlado As Nullable(Of Global.System.Boolean), ByVal activo As Nullable(Of Global.System.Boolean), ByVal codigo As Global.System.String, ByVal idProceso As Nullable(Of Global.System.Int16), ByVal responsable As Global.System.String, ByVal uRL As Global.System.String, ByVal cierre As Nullable(Of Global.System.Boolean), ByVal uRLOtroServidor As Global.System.String, ByVal tipoArchivo As Global.System.String, ByVal recuperacion As Nullable(Of Global.System.Boolean), ByVal uRLRecuperacion As Global.System.String) As List(Of GD_GD_MaestroDocumentos_Get2_Result)
            Return gdContext.GD_GD_MaestroDocumentos_Get2(idDocumento, documento, controlado, activo, codigo, idProceso, responsable, uRL, cierre, uRLOtroServidor, tipoArchivo, recuperacion, uRLRecuperacion).ToList
        End Function
#End Region

#Region "Escaner Documentos"
        Public Function DevolverTodos() As List(Of GD_EscanerDocumentos_Get_Result)
            Try
                Return GD_EscanerDocumentos_Get(Nothing, Nothing, Nothing, Nothing, Nothing)
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function DevolverxId(ByVal Id As Int64?) As List(Of GD_EscanerDocumentos_Get_Result)
            Try
                Return GD_EscanerDocumentos_Get(Id, Nothing, Nothing, Nothing, Nothing)
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function DevolverxIdTrabajo(ByVal IdTrabajo As Int64?) As List(Of GD_EscanerDocumentos_Get_Result)
            Try
                Return GD_EscanerDocumentos_Get(Nothing, IdTrabajo, Nothing, Nothing, Nothing)
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function DevolverxEncontrado(ByVal IdTrabajo As Int64?) As List(Of GD_EscanerDocumentos_Get_Result)
            Try
                Return GD_EscanerDocumentos_Get(Nothing, IdTrabajo, Nothing, False, Nothing)
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function DevolverxIdTrabajoIdRolResponsable(ByVal IdTrabajo As Int64?, ByVal idRolResponsable As Integer?) As List(Of GD_EscanerDocumentos_Get_Result)
            Try
                Return GD_EscanerDocumentos_Get(Nothing, IdTrabajo, Nothing, Nothing, idRolResponsable)
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Private Function GD_EscanerDocumentos_Get(ByVal Id As Int64?, ByVal IdTrabajo As Int64?, ByVal IdDocumento As Int64?, ByVal CodEncontrado As Boolean?, rolResponsableCierre As Integer?) As List(Of GD_EscanerDocumentos_Get_Result)
            Try
                Dim oResult As ObjectResult(Of GD_EscanerDocumentos_Get_Result) = gdContext.GD_EscanerDocumentos_Get(Id, IdTrabajo, IdDocumento, CodEncontrado, rolResponsableCierre)
                Return oResult.ToList()
            Catch ex As Exception
                If (ex.InnerException Is Nothing) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function Guardar(IdTrabajo As Int64?, ByVal IdDocumento As Int64?, ByVal Encontrado As Boolean?) As Decimal
            Return gdContext.GD_EscanerDocumentos_Add(IdTrabajo, IdDocumento, Encontrado).FirstOrDefault
        End Function

        Public Function ActualizarObservacion(ByVal Id As Int64?, ByVal Observacion As String) As Decimal
            Return gdContext.GD_EscanerDocumentos_Edit(Id, Nothing, Nothing, Nothing, Observacion)
        End Function

        Public Function ActualizarDocumento(ByVal Id As Int64?, ByVal IdTrabajo As Int64?, ByVal IdDocumento As Int64?, ByVal Encontrado As Boolean?) As Decimal
            Return gdContext.GD_EscanerDocumentos_Edit(Id, IdTrabajo, IdDocumento, Encontrado, Nothing)
        End Function

#End Region

    End Class
End Namespace

