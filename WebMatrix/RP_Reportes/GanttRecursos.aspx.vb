Imports System.Math
Imports InfosoftGlobal
Imports WebMatrix.Util
Imports CoreProject

Public Class GanttRecursos
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarUnidades()
            'Me.ddlUnidades.SelectedValue = 14
            'Me.txtFechaInicio.Text = "2013-01-01"
            'Me.txtFechaTerminacion.Text = "2013-04-01"
            'CargarGantt()
            'ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If

    End Sub

    Private Sub CargarUnidades()
        Dim oUnidades As New CoreProject.US.Unidades

        'ddlUnidades.DataSource = oUnidades.ObtenerUnidadCombo
        ddlUnidades.DataSource = oUnidades.ObtenerUnidadesxUsuario(Session("IDUsuario").ToString)
        ddlUnidades.DataTextField = "Unidad"
        ddlUnidades.DataValueField = "id"
        ddlUnidades.DataBind()

        ddlUnidades.Items.Insert(0, New ListItem With {.Value = -1, .Text = "--Seleccione--"})

    End Sub

    Sub CargarGantt()
        Dim o As New CoreProject.Reportes.Directores
        Dim info As New List(Of REP_AsignacionRecursosXUnidad_Result)

        'Lee la información filtrando por fechas seleccionadas y unidad seleccionada
        info = o.ObtenerAsignacionRecursosXUnidad(CDate(txtFechaInicio.Text).Date, CDate(txtFechaTerminacion.Text).Date, ddlUnidades.SelectedValue)

        'Crea un nuevo objeto stringbuilder para almacenar el xml del gantt
        Dim xmlGantt As New StringBuilder()

        Dim fechaini As DateTime
        Dim mes1 As Integer
        fechaini = txtFechaInicio.Text
        mes1 = fechaini.Month
        Dim ultimo2 As Date
        Dim fechacar As Date

        Dim fechafin As DateTime
        Dim mes2 As Integer
        fechafin = txtFechaTerminacion.Text
        mes2 = fechafin.Month

        Dim WDias As Integer
        WDias = DateDiff("d", fechaini, fechafin)
        Dim mesdif As Integer
        mesdif = DateDiff("m", fechaini, fechafin)
        Dim nommes As String
        If mesdif <> 0 Then
            xmlGantt.Append("<chart scrollColor='99CCCC' scrollPadding='3' scrollHeight='20' scrollBtnWidth='28' dateFormat='dd/mm/yyyy' scrollBtnPadding='3' caption='Tareas Gerentes'  ganttPaneDuration='57' ganttPaneDurationUnit='d' hoverBandColor='BEFFFF' hoverBandAlpha='40' processHoverBandColor='D1FFA4' processHoverBandAlpha='40'>")
            xmlGantt.Append("<categories font='Arial' fontColor='ffffff' isBold='1' fontSize='12' bgColor='333333'>")
            fechacar = txtFechaInicio.Text
            For I = 0 To mesdif
                nommes = MonthName(mes1)
                ultimo2 = fechacar.AddDays(-fechacar.Day + 1).AddMonths(1).AddDays(-1)
                xmlGantt.Append("<category start='" & fechacar & "' end='" & ultimo2 & "' label='" & nommes & "' />")
                If mes1 >= 13 Then
                    Exit For
                End If
                mes1 = mes1 + 1
                If fechaini.Year <> fechafin.Year Then

                Else : fechacar = "01/" & mes1 & "/" & fechafin.Year
                End If
            Next
            xmlGantt.Append("</categories>")
        Else
            xmlGantt.Append("<chart scrollColor='99CCCC' scrollPadding='3' scrollHeight='20' scrollBtnWidth='28' dateFormat='dd/mm/yyyy' scrollBtnPadding='3' caption='Tareas Gerentes' ganttPaneDuration='57' ganttPaneDurationUnit='d' hoverBandColor='BEFFFF' hoverBandAlpha='40' processHoverBandColor='D1FFA4' processHoverBandAlpha='40'>")
            xmlGantt.Append("<categories font='Arial' fontColor='ffffff' isBold='1' fontSize='12' bgColor='333333'>")
            xmlGantt.Append("<category start='" & txtFechaInicio.Text & "' end='" & txtFechaTerminacion.Text & "' label='" & fechaini.ToString("MMMM") & "' />")
            xmlGantt.Append("</categories>")
        End If
        xmlGantt.Append("<categories font='Arial' fontColor='ffffff' isBold='1' fontSize='12' bgColor='333333'>")
        For N = 0 To WDias + 1
            Dim wfecha As String
            wfecha = Format(fechaini.AddDays(N), "dd/MM/yyyy")
            Dim tam As String
            tam = Mid(wfecha, 1, 2)
            xmlGantt.Append("<category start='" & wfecha & "' end='" & wfecha & "' label='" & tam & "' align='left'/>")
        Next N
        xmlGantt.Append("</categories>")
        'Obtiene el listado de los recursos en esas fechas
        Dim list = (From l1 In o.ObtenerAsignacionRecursosXUnidad(CDate(txtFechaInicio.Text).Date, CDate(txtFechaTerminacion.Text).Date, ddlUnidades.SelectedValue)
        Select Usuario = l1.UsuarioAsignado).Distinct.ToList

        Dim list2 = (From l2 In o.ObtenerAsignacionRecursosXUnidad(CDate(txtFechaInicio.Text).Date, CDate(txtFechaTerminacion.Text).Date, ddlUnidades.SelectedValue)
        Select Usuario = l2.idusuario).Distinct.ToList
        Dim ltareas = o.ObtenerAsignacionRecursosXUnidad(CDate(txtFechaInicio.Text).Date, CDate(txtFechaTerminacion.Text).Date, ddlUnidades.SelectedValue)

        'Procesos: Tareas de cada usuario
        xmlGantt.Append("<processes fontSize='12' isBold='1' align='left' headerText='Gerentes' headerFontSize='18' headerVAlign='bottom' headerAlign='left' >")
        For I = 0 To list.Count - 1
            xmlGantt.Append("<process label='" & list(I).ToString & "' id='" & list2.Item(I) & "'  />")
        Next I
        xmlGantt.Append("</processes>")
        'Tareas: Corresponden a las actividades de cada persona desde la tabla
        'Recorre las tareas y asigna a los recursos
        xmlGantt.Append("<tasks showLabels='2' >")
        For I = 0 To ltareas.ToList.Count - 1
            'xmlGantt.Append("<task processId='" & ltareas(i).idusuario & "' start='" & ltareas(i).FIniP & "' end='" & ltareas(i).FFinP & "' Id='" & i & "' color='EEEEEE' height='25%' topPadding='70%' />")
            'xmlGantt.Append("<task  start='" & ltareas(i).FIniP & "' end='" & ltareas(i).FFinP & "' toolText='" & ltareas(i).FIniP & "' processId='" & ltareas(i).idusuario & "' color='8BBA00' borderColor='8BBA00' width='10' topPadding='95%' height='10%' />")
            If ltareas(I).FFinR IsNot Nothing Then
                xmlGantt.Append("<task start='" & ltareas(I).FIniP & "' end='" & ltareas(I).FFinP & "' toolText='" & ltareas(I).NombreTrabajo & " - " & ltareas(I).Tarea & " - " & ltareas(I).FIniP & "' processId='" & ltareas(I).idusuario & "' color='e1f5ff' borderColor='AFD8F8' width='12' />")
            Else
                xmlGantt.Append("<task start='" & ltareas(I).FIniP & "' end='" & ltareas(I).FFinP & "' toolText='" & ltareas(I).NombreTrabajo & " - " & ltareas(I).Tarea & " - " & ltareas(I).FIniP & "' processId='" & ltareas(I).idusuario & "' color='8BBA00' borderColor='8BBA00' width='12' />")
            End If

        Next I
        'Cierra las tareas
        xmlGantt.Append("</tasks>")
        'Cierra el gráfico
        xmlGantt.Append("</chart>")
        'Grafica el Gantt en el aspx
        Gantt1.Text = FusionCharts.RenderChart("../FusionWidgets/Gantt.swf", "", xmlGantt.ToString(), "chartid2", "100%", "400px", False, True)

    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If IsDate(txtFechaInicio.Text) And IsDate(txtFechaTerminacion.Text) And Not (ddlUnidades.SelectedValue) Then
            If Not (CDate(txtFechaTerminacion.Text) > CDate(txtFechaInicio.Text)) Then
                ShowNotification("Datos Erroneos", ShowNotifications.ErrorNotification)
                Exit Sub
            Else
                CargarGantt()
                ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            End If
        End If
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub
End Class