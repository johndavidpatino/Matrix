Imports System.IO
Imports System.Runtime.Remoting.Contexts
Imports System.Web.Script.Services
Imports CoreProject
Imports CoreProject.AusenciasEquipo

Public Class AusenciasEquipo
    Inherits System.Web.UI.Page
    Shared userId As Int64
    Shared ausenciasEquipo As AusenciasEquipoDapper
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userId = HttpContext.Current.Session("IDUsuario")
        AusenciasEquipo = New AusenciasEquipoDapper()
        Dim o As New TH_Ausencia.DAL
        Dim datos = ausenciasEquipo.GetAusenciasPersonas(Session("IDUsuario"), "")

        If datos.Count = 0 Then
            Response.Redirect("../TH_TalentoHumano/TH_SolicitudAusencia.aspx")
        End If

    End Sub



    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getCurrentUserId() As Int64
        Return userId
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getAusenciasEquipo(ByVal jefeId As Int64, fInicio As DateTime, fFin As DateTime) As List(Of AusenciasEquipoDapper.AusenciasEquipoResult)


        Return ausenciasEquipo.GetAusenciasEquipo(jefeId, fInicio, fFin)

    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getBeneficiosPendientes(ByVal idempleado As Int64) As List(Of AusenciasEquipoDapper.BeneficiosPendientesResult)



        Return ausenciasEquipo.GetBeneficiosPendientes(idempleado)

    End Function


    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getAusenciasSubordinados(ByVal jefeId As Int64) As List(Of AusenciasEquipoDapper.AusenciasSubordinado)



        Return ausenciasEquipo.GetAusenciasSubordinados(jefeId)

    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getAusenciasPersonas(ByVal jefeId As Int64, search As String) As List(Of AusenciasEquipoDapper.AusenciasSubordinado)



        Return ausenciasEquipo.GetAusenciasPersonas(jefeId, search)

    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function removeAusenciasSubordinado(ByVal subordinadoId As Int64)



        Return ausenciasEquipo.RemoveAusenciasSubordinado(subordinadoId)

    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function addAusenciasSubordinado(ByVal jefeId As Int64, empleadoId As Int64)



        Return ausenciasEquipo.AddAusenciasSubordinado(jefeId, empleadoId)

    End Function
End Class