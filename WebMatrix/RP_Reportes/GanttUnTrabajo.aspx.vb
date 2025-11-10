Imports System.Math
Imports InfosoftGlobal
Imports CoreProject
Public Class GanttUnTrabajo
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

        Dim xmlGantt As New StringBuilder()
        Dim WFechaIni As Date
        Dim WFechaFin As Date
        Dim WFecha As String
        Dim WDias As Integer
        Dim WIniciaP As String
        Dim WFinP As String
        Dim WIniciaR As String
        Dim WFinR As String
        Dim WID As String

        Dim IdTrabajo As Integer

        If Request.QueryString("TrabajoId") IsNot Nothing Then
            IdTrabajo = Request.QueryString("TrabajoId")
        End If

        Dim FechasUnTrabajoAD As New OperacionesTableAdapters.RP_PlanUnTrabajoIniciaFinTableAdapter
        Dim FechasUnTrabajoDT As New Operaciones.RP_PlanUnTrabajoIniciaFinDataTable
        Dim FechasUntrabajo As Operaciones.RP_PlanUnTrabajoIniciaFinRow
        FechasUnTrabajoDT = FechasUnTrabajoAD.GetData(IdTrabajo)
        FechasUntrabajo = FechasUnTrabajoDT.Rows(0)
        WFechaIni = FechasUntrabajo.FechaI
        WFechaFin = FechasUntrabajo.FechaF
        WDias = DateDiff("d", WFechaIni, WFechaFin)    'Dias Duracion plan

        Dim PlanUnTrabajoAD As New OperacionesTableAdapters.RP_PlanUnTrabajoTableAdapter
        Dim PlanUnTrabajoDT As New Operaciones.RP_PlanUnTrabajoDataTable
        Dim PlanUntrabajo As Operaciones.RP_PlanUnTrabajoRow

        PlanUnTrabajoDT = PlanUnTrabajoAD.GetData(IdTrabajo)
        PlanUntrabajo = PlanUnTrabajoDT.Rows(0)
        xmlGantt.Append("<chart fontSize='12' dateFormat='dd/mm/yyyy' caption='Planeacion' subCaption='" & PlanUntrabajo.NombreTrabajo & " ' showTaskEndDate='1' showPercentLabel='1'  >")

        xmlGantt.Append("<categories>")
        For I = 0 To WDias
            WFecha = Format(WFechaIni.AddDays(I), "dd/MM/yyyy")
            xmlGantt.Append("<category start='" & WFecha & " ' end='" & WFecha & "' />")
        Next I
        xmlGantt.Append("</categories>")

        xmlGantt.Append("<processes fontSize='12' isBold='1' align='right' headerFontSize='12' >")
        For I = 0 To PlanUnTrabajoDT.Rows.Count - 1
            PlanUntrabajo = PlanUnTrabajoDT.Rows(I)
            xmlGantt.Append("<process label=' " & PlanUntrabajo.Tarea & " ' id= '" & PlanUntrabajo.TareaId & "' />")
        Next I
        xmlGantt.Append("</processes>")

        xmlGantt.Append("<tasks showLabels='1' >")
        For I = 0 To PlanUnTrabajoDT.Rows.Count - 1
            PlanUntrabajo = PlanUnTrabajoDT.Rows(I)
            WIniciaP = Format(PlanUntrabajo.FIniP, "dd/MM/yyyy")
            WFinP = Format(PlanUntrabajo.FFinP, "dd/MM/yyyy")
            WID = Str(PlanUntrabajo.TareaId) & "-1"
            

            xmlGantt.Append("<task label='Plan' processId='" & PlanUntrabajo.TareaId & "' start=' " & WIniciaP & "' end='" & WFinP & "' id='" & WID & "' color='4567aa' height='25%' topPadding='22%' />")
            Try
                WIniciaR = Format(PlanUntrabajo.FIniR, "dd/MM/yyyy")
                WFinR = Format(PlanUntrabajo.FFinR, "dd/MM/yyyy")
                xmlGantt.Append("<task label='Ejec' processId='" & PlanUntrabajo.TareaId & "' start=' " & WIniciaR & "' end='" & WFinR & "' id='" & PlanUntrabajo.TareaId & "' color='EEEEEE' height='25%' topPadding='70%' />")
            Catch ex As Exception

            End Try
            
        Next I
        xmlGantt.Append("</tasks>")
        xmlGantt.Append("</chart>")
        Gantt1.Text = FusionCharts.RenderChart("../FusionWidgets/Gantt.swf", "", xmlGantt.ToString(), "chartid1", "100%", "400px", False, True)


    End Sub
End Class