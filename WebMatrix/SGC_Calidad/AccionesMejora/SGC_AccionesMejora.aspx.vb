Imports CoreProject
Imports System.Web.Script.Services

Public Class SGC_AccionesMejora
    Inherits System.Web.UI.Page
    Shared registroAccionesRepository As New AccionesMejoraDapper()
    Shared userId As Long

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userId = HttpContext.Current.Session("IDUsuario")

    End Sub

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function Users() As Result(Of List(Of AccionesMejoraUsuario_Get_Response))
        Return registroAccionesRepository.GetUsuarios()
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function Processes() As Result(Of List(Of AccionMejoraProcesos_Get_Response))
        Return registroAccionesRepository.GetAccionMejoraProcesos()
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function Fuentes() As Result(Of List(Of TiposFuente_Get_Response))
        Return registroAccionesRepository.GetTiposFuente()
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function AcccionesMejora(request As AccionMejoraFull_Add_Request) As Result
        Dim resultAddAccion = registroAccionesRepository.AddAccionMejora(New AccionesMejora_Add_Request With {
            .DescripcionAccion = request.DescripcionAccion,
            .FechaIncidente = request.FechaIncidente,
            .UsuarioReporta = request.UsuarioReporta,
            .ProcesoId = request.ProcesoId,
            .UsuarioResponsable = request.UsuarioResponsable,
            .Descripcion = request.Descripcion,
            .Correccion = request.Correccion,
            .FuenteNoConformidadId = request.FuenteNoConformidadId,
            .FuenteId = request.FuenteId
        })
        If (resultAddAccion.Success = False) Then
            HttpContext.Current.Response.StatusCode = 500
            Return resultAddAccion
        End If

        Dim resultAddCausas = registroAccionesRepository.AddCausas(resultAddAccion.Data, request.Causas)

        If resultAddCausas.Success = False Then
            HttpContext.Current.Response.StatusCode = 500
            Return resultAddCausas
        End If

        Dim resultAddPlanesAccion = registroAccionesRepository.AddPlanesAccion(resultAddAccion.Data, request.PlanesAccion)

        If resultAddPlanesAccion.Success = False Then
            HttpContext.Current.Response.StatusCode = 500
            Return resultAddPlanesAccion
        End If

        Return New Result With {
            .Success = True,
            .Message = "Acción de mejora registrada correctamente"
        }
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function GetAccionesMejora(request As AccionesMejora_Get_Request) As Result(Of List(Of AccionesMejora_Get_Response))
        Return registroAccionesRepository.GetAccionesMejora(request)
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function DeleteAccionMejora(accionMejoraId As Integer) As Result
        Return registroAccionesRepository.DeleteAccionMejora(accionMejoraId)
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function GetAccionMejoraById(accionMejoraId As Integer) As Result(Of AccionMejoraFull_Get_Response)
        Dim result = registroAccionesRepository.GetAccionesMejora(New AccionesMejora_Get_Request With {
            .AccionMejoraId = accionMejoraId
        })
        If result.Success = False Then
            HttpContext.Current.Response.StatusCode = 500
            Return New Result With {
                .Success = False,
                .Message = "Error al obtener la acción de mejora"
            }
        End If
        Dim accionMejora = result.Data.FirstOrDefault()

        If accionMejora Is Nothing Then
            HttpContext.Current.Response.StatusCode = 404
            Return New Result With {
                .Success = False,
                .Message = "Acción de mejora no encontrada"
            }
        End If

        Dim resultCausas = registroAccionesRepository.GetCausasByAccionMejoraId(accionMejoraId)

        If resultCausas.Success = False Then
            HttpContext.Current.Response.StatusCode = 500
            Return New Result With {
                .Success = False,
                .Message = "Error al obtener las causas de la acción de mejora"
            }
        End If

        Dim resultPlanesAccion = registroAccionesRepository.GetPlanesAccionByAccionMejoraId(accionMejoraId)

        If resultPlanesAccion.Success = False Then
            HttpContext.Current.Response.StatusCode = 500
            Return New Result With {
                .Success = False,
                .Message = "Error al obtener los planes de acción de la acción de mejora"
            }
        End If

        Return New Result(Of AccionMejoraFull_Get_Response)(
            True,
            "Acción de mejora obtenida correctamente",
            New AccionMejoraFull_Get_Response With {
                .AccionMejoraId = accionMejora.AccionMejoraId,
                .DescripcionAccion = accionMejora.DescripcionAccion,
                .FechaIncidente = accionMejora.FechaIncidente,
                .UsuarioReporta = accionMejora.UsuarioReporta,
                .ProcesoId = accionMejora.ProcesoId,
                .UsuarioResponsable = accionMejora.UsuarioResponsable,
                .Descripcion = accionMejora.Descripcion,
                .Correccion = accionMejora.Correccion,
                .TipoFuenteId = accionMejora.TipoFuenteId,
                .FuenteId = accionMejora.FuenteId,
                .Causas = resultCausas.Data,
                .PlanesAccion = resultPlanesAccion.Data
            }
        )
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function UpdateAccionMejora(request As AccionMejoraFull_Edit_Request) As Result
        Dim resultUpdateAccion = registroAccionesRepository.UpdateAccionMejora(New AccionesMejora_Edit_Request With {
            .AccionMejoraId = request.AccionMejoraId,
            .DescripcionAccion = request.DescripcionAccion,
            .FechaIncidente = request.FechaIncidente,
            .UsuarioReporta = request.UsuarioReporta,
            .ProcesoId = request.ProcesoId,
            .UsuarioResponsable = request.UsuarioResponsable,
            .Descripcion = request.Descripcion,
            .Correccion = request.Correccion,
            .FuenteNoConformidadId = request.FuenteNoConformidadId
        })

        If resultUpdateAccion.Success = False Then
            HttpContext.Current.Response.StatusCode = 500
            Return resultUpdateAccion
        End If

        Dim storedCausas = registroAccionesRepository.GetCausasByAccionMejoraId(request.AccionMejoraId).Data
        Dim storedPlanes = registroAccionesRepository.GetPlanesAccionByAccionMejoraId(request.AccionMejoraId).Data

        Dim nuevasCausas = New List(Of Causas_Add_Request)
        Dim editCausas = New List(Of Causas_Edit_Request)

        For Each causa In request.Causas
            If (causa.CausaId Is Nothing) Then
                nuevasCausas.Add(New Causas_Add_Request With {
                    .DescripcionCausa = causa.DescripcionCausa,
                    .AccionMejoraId = request.AccionMejoraId
                })
                Continue For
            End If
            editCausas.Add(causa)
        Next

        Dim causasForDelete = storedCausas.Where(Function(c) editCausas.FindIndex(Function(ed) ed.CausaId = c.CausaId) = -1)

        Dim resultDeleteCausas = registroAccionesRepository.DeleteCausas(causasForDelete.Select(Function(c) c.CausaId).ToList())

        If resultDeleteCausas.Success = False Then
            HttpContext.Current.Response.StatusCode = 500
            Return resultDeleteCausas
        End If

        Dim resultUpdateCausas = registroAccionesRepository.EditCausas(editCausas)

        If resultUpdateCausas.Success = False Then
            HttpContext.Current.Response.StatusCode = 500
            Return resultUpdateCausas
        End If

        Dim resultAddCausas = registroAccionesRepository.AddCausas(request.AccionMejoraId, nuevasCausas)

        If resultAddCausas.Success = False Then
            HttpContext.Current.Response.StatusCode = 500
            Return resultAddCausas
        End If

        Dim planesAccionUpdate = New List(Of PlanesAccion_Edit_Request)
        Dim planesAccionAdd = New List(Of PlanesAccion_Add_Request)

        For Each plan In request.PlanesAccion
            If plan.PlanAccionId Is Nothing Then
                planesAccionAdd.Add(New PlanesAccion_Add_Request With {
                    .DescripcionPlan = plan.DescripcionPlan,
                    .FechaPlaneado = plan.FechaPlaneado,
                    .AccionMejoraId = request.AccionMejoraId,
                    .EficaciaPlan = plan.EficaciaPlan,
                    .FechaEjecutado = plan.FechaEjecutado,
                    .FechaRevision = plan.FechaRevision
                                    })
                Continue For
            End If
            planesAccionUpdate.Add(plan)
        Next

        Dim planesAccionForDelete = storedPlanes.Where(Function(p) planesAccionUpdate.FindIndex(Function(ed) ed.PlanAccionId = p.PlanAccionId) = -1)

        Dim resultDeletePlanesAccion = registroAccionesRepository.DeletePlanesAccion(planesAccionForDelete.Select(Function(p) p.PlanAccionId).ToList())

        If resultDeletePlanesAccion.Success = False Then
            HttpContext.Current.Response.StatusCode = 500
            Return resultDeletePlanesAccion
        End If

        Dim resultUpdatePlanesAccion = registroAccionesRepository.EditPlanesAccion(planesAccionUpdate)

        If resultUpdatePlanesAccion.Success = False Then
            HttpContext.Current.Response.StatusCode = 500
            Return resultUpdatePlanesAccion
        End If

        Dim resultAddPlanesAccion = registroAccionesRepository.AddPlanesAccion(request.AccionMejoraId, planesAccionAdd)

        If resultAddPlanesAccion.Success = False Then
            HttpContext.Current.Response.StatusCode = 500
            Return resultAddPlanesAccion
        End If

        Return New Result With {
            .Success = True,
            .Message = "Acción de mejora actualizada correctamente"
        }

    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function GetAuditoriasInternas() As Result(Of List(Of AuditoriaInterna_Get_Response))
        Return registroAccionesRepository.GetAuditoriasInterna()
    End Function

    Public Class AccionMejoraFull_Add_Request
        Property DescripcionAccion As String
        Property FechaIncidente As DateTimeOffset
        Property AccionCorrectiva As String = Nothing
        Property OportunidadMejora As String = Nothing
        Property UsuarioReporta As Long
        Property ProcesoId As Integer
        Property UsuarioResponsable As Long
        Property Descripcion As String
        Property Correccion As String
        Property FuenteNoConformidadId As Integer
        Property FuenteId As Integer?
        Property Causas As List(Of Causas_Add_Request)
        Property PlanesAccion As List(Of PlanesAccion_Add_Request)
    End Class

    Public Class AccionMejoraFull_Edit_Request
        Property AccionMejoraId As Integer
        Property DescripcionAccion As String
        Property FechaIncidente As DateTimeOffset
        Property UsuarioReporta As Long
        Property ProcesoId As Integer
        Property UsuarioResponsable As Long
        Property Descripcion As String
        Property Correccion As String
        Property FuenteNoConformidadId As Integer
        Property FuenteId As Integer?
        Property Causas As List(Of Causas_Edit_Request)
        Property PlanesAccion As List(Of PlanesAccion_Edit_Request)
    End Class

    Public Class AccionMejoraFull_Get_Response : Inherits AccionesMejora_Get_Response
        Property Causas As List(Of Causas_Get_Response)
        Property PlanesAccion As List(Of PlanesAccion_Get_Response)
    End Class
End Class