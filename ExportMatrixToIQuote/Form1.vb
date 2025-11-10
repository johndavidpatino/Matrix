Imports System.Threading
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Chrome.ChromeDriverService
Imports OpenQA.Selenium.Support.UI
Imports OpenQA.Selenium.Support.UI.SelectElement

Public Class Form1

#Region "Variables Globales para Grids"
	Dim _TotalViaticos As Decimal = 0
	Dim _TotalHoteles As Decimal = 0
	Dim _TotalTransporte As Decimal = 0
	Dim _Presupuestado As Decimal = 0
	Dim _TotalHoras As Integer = 0
	Dim _Presupuestado2 As Decimal = 0
	Dim _Total As Decimal = 0
	Dim TimeWindows As Integer = 6
	Dim TimeScript As Integer = 4
	Dim TimeShort As Integer = 1

#End Region

	Dim driver As New ChromeDriver
	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
		'''' ### Comment

		Dim flag As Boolean = True
		Dim oCot As New DAL.CotizadorGeneral
		Dim listAlternativas As List(Of DTO.IQ_AlternativasMarkedToIquote_Result) = oCot.GetAlternativasToExport(txtUsuario.Text, txtPassword.Text)
		If listAlternativas.Count = 0 Then
			Exit Sub
		End If
		Me.Visible = False
		For Each alternativa In listAlternativas
			Dim messageResult As String = ExportiQuoteGeneral(alternativa.Propuesta, alternativa.Alternativa, alternativa.JobBook)
			If messageResult = "Se ha cancelado la operación" Then
				flag = False
			End If
			If messageResult.Contains("Ha surgido") Then
				MsgBox(messageResult)
			End If
		Next
		If flag = True Then
			MsgBox("Se han exportado todas las alternativas")
		End If
		LoadAlternativasToExport()
		Me.Visible = True

		'''' ### Comment


		'Dim listado = ObtenerActividadesSubcontratadas(10932, 8, "Subcontract")
		'Dim Propuesta As Integer = 10970
		'Dim Alternativa As Integer = 1
		'Dim oCot As New DAL.CotizadorGeneral
		'Dim PRGeneral = oCot.GetGeneralByAlternativa(Propuesta, Alternativa)
		'Dim PRlst = oCot.GetAllPresupuestosByAlternativa(Propuesta, Alternativa)
		'Dim lProcesos = ObtenerProcesos(Propuesta, Alternativa, 210, 1)
		'Dim muestra = oCot.GetTotalMuestra(Propuesta, Alternativa, 210, 1)
		'Exit Sub
		''Dim driver As New ChromeDriver
		''driver.Navigate.GoToUrl("http://nwb.ipsos.com/")

		''MessageBox.Show("Wait")
		''driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#1176086-B1")
		''Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		''ExportiQuoteOtherDirectCosts(driver, 10970, 1)
		''ExportiQuoteSubContract(driver, 10970, 1)
		'Dim result = MessageBox.Show("Haga clic en Aceptar (OK) cuando ya haya cargado iQuote completamente o Cancelar (Cancel) para abortar la sincronización", "Continuar a iQuote", MessageBoxButtons.OKCancel)
		'If result = DialogResult.Cancel Then
		'	driver.Quit()
		'Else
		'	driver.Quit()
		'End If
	End Sub


	Function ExportiQuoteGeneral(ByVal Propuesta As Int32, ByVal Alternativa As Int32, ByVal NumeroIQuoteToSend As String, Optional NumJobBook As String = "") As String
		''Dim driver As New ChromeDriver

		'Try
		'#####driver.Navigate.GoToUrl("https://nwb.ipsos.com/")

		'#####Dim btnEnter = driver.FindElement(By.Id("wsfOauthLogonButton"))
		'#####btnEnter.Click()

		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 5, 0))
		'#####waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlToBe("https://nwb.ipsos.com/JobExec/JobExecutionHome.aspx"))

		'Dim result = MessageBox.Show("Haga clic en Aceptar (OK) cuando ya haya cargado iQuote completamente o Cancelar (Cancel) para abortar la sincronización", "Continuar a iQuote", MessageBoxButtons.OKCancel)
		'If result = DialogResult.Cancel Then
		'	Return "Se ha cancelado la operación"
		'End If

		Try
			driver.ExecuteScript("document.body.style.zoom='80%'")
		Catch ex As Exception

		End Try

		Dim oCot As New DAL.CotizadorGeneral
		Dim PRGeneral = oCot.GetGeneralByAlternativa(Propuesta, Alternativa)
		Dim PRlst = oCot.GetAllPresupuestosByAlternativa(Propuesta, Alternativa)


		'#### TODO REVERSE
		' ---> Búsqueda de propuesta
		'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#1283763-S1")
		'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#1285041-S1")
		'driver.Navigate.GoToUrl("https://nwb.ipsos.com/iquote/#1285045-S1")
		'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#proposal~" & NumJobBook.ToString)
		'driver.Navigate.GoToUrl("https://nwb.ipsos.com/iquote/#1288227-B1")
		driver.Navigate.GoToUrl("https://nwb.ipsos.com/iquote/#" & NumeroIQuoteToSend)
		Try
			driver.SwitchTo().Alert().Dismiss()
		Catch ex As Exception

		End Try
		waitdriver = New WebDriverWait(driver, New TimeSpan(0, 0, 30))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o503")))
		'waitdriver.Until(driver.)
		' ---> Nueva estimación


		''TODO Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))

		'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#" & NumIQuote)
		PRGeneral.Descripcion = PRGeneral.Descripcion.Replace(vbCrLf, " -- ").Replace(vbTab, " || ").Replace("&", "")
		'/driver.FindElement(By.Id("o503")).SendKeys(PRGeneral.Descripcion.Replace("vbCrLf", " -- ").Replace("vbTab", " || ").Replace("&", "")) ' Descripción de presupuesto
		driver.ExecuteScript("document.getElementById('o503').value = '" & PRGeneral.Descripcion.Replace("vbCrLf", " -- ").Replace("vbTab", " || ").Replace("&", "") & "';")
		'/driver.FindElement(By.Id("o511")).SendKeys("Alternativa " & Alternativa.ToString) ' Descripción de versión
		'driver.ExecuteScript("document.getElementById('o511').value = 'Alternativa " & Alternativa.ToString & "';")
		driver.ExecuteScript("document.getElementById('o511').value = 'Propuesta: " & Propuesta & ". Alternativa: " & Alternativa.ToString & ". From Matrix';")
		driver.ExecuteScript("jsSaveLinkClick(0);")
		'Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		Try
			driver.SwitchTo().Alert().Dismiss()
		Catch ex As Exception

		End Try
		Dim NumIQuote As String = ""
		Try
			NumIQuote = driver.Url.Split("#")(1)
		Catch ex As Exception

		End Try
		For Each presup In PRlst
			Dim pCampo As Boolean = False
			Dim pCodificacion As Boolean = False
			Dim pDP As Boolean = False
			Dim pScripting As Boolean = False
			If presup.TecCodigo = "100" Or presup.TecCodigo = "200" Or presup.TecCodigo = "300" Then
				Dim lProcesos = ObtenerProcesos(Propuesta, Alternativa, presup.MetCodigo, presup.ParNacional)
				Dim CostosToIquote = ObtenerCostosToIquote(Propuesta, Alternativa, presup.MetCodigo, presup.ParNacional)
				For Each item In lProcesos
					Select Case item.ProcCodigo
						Case 1
							pCampo = True
						'DirectCast(UCHeader.FindControl("chbProcessCampo"), CheckBox).Checked = True
						Case 2
						'DirectCast(UCHeader.FindControl("chbProcessVerificacion"), CheckBox).Checked = True
						Case 3
						'DirectCast(UCHeader.FindControl("chbProcessCritica"), CheckBox).Checked = True
						Case 4
							pCodificacion = True
						'DirectCast(UCHeader.FindControl("chbProcessCodificacion"), CheckBox).Checked = True
						Case 6 To 9
							pDP = True
						'DirectCast(UCHeader.FindControl("chbProcessDataClean"), CheckBox).Checked = True
						'DirectCast(UCHeader.FindControl("chbProcessTopLines"), CheckBox).Checked = True
						'DirectCast(UCHeader.FindControl("chbProcessProceso"), CheckBox).Checked = True
						'DirectCast(UCHeader.FindControl("chbProcessArchivos"), CheckBox).Checked = True
						Case 10
							pScripting = True
							'DirectCast(UCHeader.FindControl("chbProcessScripting"), CheckBox).Checked = True
					End Select
				Next
				If pCampo = True Or CostosToIquote.Where(Function(x) x.id = 14).ToList.Count > 0 Or CostosToIquote.Where(Function(x) x.id = 13).ToList.Count > 0 Then
					ExportiQuoteTelefonico(driver, presup, PRGeneral, CostosToIquote)
					ExportiQuoteF2F(driver, presup, PRGeneral, CostosToIquote)
					ExportiQuoteOnline(driver, presup, PRGeneral, CostosToIquote)
					Try
						driver.ExecuteScript("jsDialogDirectScriptingWaveClose_click();")
					Catch ex As Exception

					End Try
				End If
				If pCodificacion = True Then
					ExportiQuoteCodificacion(driver, presup, PRGeneral, CostosToIquote)
					Try
						driver.ExecuteScript("jsDialogDirectCodingWaveClose_click();")
					Catch ex As Exception

					End Try
				End If
				If pDP = True Then
					ExportiQuoteDP(driver, presup, PRGeneral, CostosToIquote)
					Try
						driver.ExecuteScript("jsDialogDirectDataProcessingWaveClose_click();")
					Catch ex As Exception

					End Try
				End If
				If pScripting = True Then
					ExportiQuoteScripting(driver, presup, PRGeneral, CostosToIquote)
					driver.ExecuteScript("jsDialogDirectScriptingWaveClose_click();")
				End If
				If presup.TecCodigo = "200" Then driver.ExecuteScript("jsDialogDirectTelephoneWaveClose_click();")
				If presup.TecCodigo = "100" Then
					driver.ExecuteScript("jsDialogXSUM_39_OPSActualSalesStartingAmountClose();")
					driver.ExecuteScript("jsDialogDirectXSUM_39_OPSClose();")
					driver.ExecuteScript("jsDialogDirectF2FWaveClose_click();")
				End If
				Try
					If pCodificacion = True Then driver.ExecuteScript("jsDialogDirectCodingWaveClose_click();")
					If pDP = True Then driver.ExecuteScript("jsDialogDirectDataProcessingWaveClose_click();")
					If pScripting = True Then driver.ExecuteScript("jsDialogDirectScriptingWaveClose_click();")
				Catch ex As Exception

				End Try
				Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
				ExportiQuoteOtherDirectCosts(driver, presup, CostosToIquote)
			End If
			driver.ExecuteScript("jsSectionSave_click(0);")
			'Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
			'If presup.TecCodigo = "200" Then driver.ExecuteScript("jsDialogDirectTelephoneWaveClose_click();")
			'If presup.TecCodigo = "100" Then
			'	driver.ExecuteScript("jsDialogXSUM_39_OPSActualSalesStartingAmountClose();")
			'	driver.ExecuteScript("jsDialogDirectXSUM_39_OPSClose();")
			'	driver.ExecuteScript("jsDialogDirectF2FWaveClose_click();")
			'End If
			'If pCodificacion = True Then driver.ExecuteScript("jsDialogDirectCodingWaveClose_click();")
			'If pDP = True Then driver.ExecuteScript("jsDialogDirectDataProcessingWaveClose_click();")
			'If pScripting = True Then driver.ExecuteScript("jsDialogDirectScriptingWaveClose_click();")
			Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		Next
		''' TODO
		'driver.Navigate.GoToUrl(driver.Url)

		ExportiQuoteProfessionalTime(driver, Propuesta, Alternativa)
		'ExportiQuoteSubContract(driver, Propuesta, Alternativa)
		driver.ExecuteScript("jsSectionSave_click(0);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.ExecuteScript("jsDialogApprovalInfoGet('Opening');")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("iqtDialogApprovalInfo")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("iqtDialogApprovalInfo")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("iqtDialogApprovalInfo_selectAll")))
		driver.FindElement(By.Id("iqtDialogApprovalInfo_selectAll")).Click()
		driver.ExecuteScript("jsDialogApprovalInfoBulkGet_click();")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("iqtDialogApprovalRequestBulk")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("iqtDialogApprovalRequestBulk")))
		driver.ExecuteScript("jsDialogApprovalRequestBulkSet();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		driver.ExecuteScript("jsDialogApprovalInfoClose();")

		Try
			driver.ExecuteScript("document.body.style.zoom='80%'")
		Catch ex As Exception

		End Try

		'Try
		'	driver.Navigate.GoToUrl(driver.Url)
		'	Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		'	driver.ExecuteScript("document.getElementById('footer_90008_override').value = '" & oCot.GetTotalVenta(Propuesta, Alternativa) & "';")
		'	driver.ExecuteScript("jsPostFooter(true);")
		'	driver.ExecuteScript("jsSectionSave_click(0);")
		'	Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'Catch ex As Exception

		'End Try

		'TODO oCot.UpdateNumIQuote(Propuesta, Alternativa, NumIQuote)
		'TODO oCot.UpdateMarkedAlternativa(Propuesta, Alternativa, NumIQuote)
		'ShowWarning(TypesWarning.Information, "Se ha exportado la alternativa al iQuote #" & NumIQuote)
		Return "Se ha exportado la alternativa al iQuote #" & NumIQuote
		'Catch ex As Exception
		'ShowWarning(TypesWarning.Warning, "Ha surgido el siguiente error: " & ex.Message.ToString)
		'Return "Ha surgido el siguiente error: " & ex.Message.ToString & " | " & ex.Source
		'Finally
		'///driver.Quit()
		'///driver.Dispose()
		'End Try
	End Function

	Sub ExportiQuoteTelefonico(ByRef driver As ChromeDriver, presup As DTO.IQ_Parametros, pGeneral As DTO.IQ_DatosGeneralesPresupuesto, costosToIquote As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))
		If Not presup.TecCodigo = "200" Then Exit Sub
		' ---> TELEFÓNICO <---
		driver.ExecuteScript("javascript:jsDialogDirectTelephoneWaveNew();")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o20230a")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o20230a")))
		'Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		'/driver.FindElement(By.Id("o20230a")).Clear()
		'/driver.FindElement(By.Id("o20230a")).SendKeys(NombreFase(presup.ParNacional)) ' Nombre de la fase
		driver.ExecuteScript("document.getElementById('o20230a').value = '" & NombreFase(presup.ParNacional) & " - " & NombreMetodologia(presup.MetCodigo) & "';")
		driver.ExecuteScript("javascript:jsSaveLinkClick(20);")
		'Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		If pGeneral.NumeroMediciones > 1 Then
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("o20240_1")))
			driver.FindElement(By.Id("o20240_1")).Click()
			driver.ExecuteScript("jsTelephoneIsTracking();")
			'/driver.FindElement(By.Id("o20242")).Clear()
			'/driver.FindElement(By.Id("o20242")).SendKeys(pGeneral.NumeroMediciones.ToString) 'Número de olas
			driver.ExecuteScript("document.getElementById('o20242').value = '" & pGeneral.NumeroMediciones.ToString & "';")
		Else
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("o20240_0")))
			driver.FindElement(By.Id("o20240_0")).Click()
			driver.ExecuteScript("jsTelephoneIsTracking();")
		End If

		'		Tema
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o20244")))
		Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20244")))
		Selectionddl.SelectByIndex(2) 'Consumidor mediano interés
		'/driver.FindElement(By.Id("o20244_comment")).SendKeys("") 'Comentarios tema o grupo objetivo
		'		Días en campo
		'/driver.FindElement(By.Id("o20250")).Clear()
		'/driver.FindElement(By.Id("o20250")).SendKeys(pGeneral.DiasCampo.ToString) 'Días en campo
		'/driver.FindElement(By.Id("o20250_comment")).SendKeys("") 'Comentarios días en campo
		driver.ExecuteScript("document.getElementById('o20250').value = '" & pGeneral.DiasCampo.ToString & "';")
		'		Quotas
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20291")))
		'/driver.FindElement(By.Id("o20291_comment")).SendKeys("") 'Comentarios cuotas
		Selectionddl.SelectByIndex(1) 'Tipo de control de cuota -> No Cuota
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20293")))
		Selectionddl.SelectByIndex(1) 'Número de celdas de cuotas -> No Cuota
		'	GUARDAR TELEFÓNICO
		driver.ExecuteScript("jsSectionSave_click(20);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'DETALLE 
		driver.ExecuteScript("javascript:jsDialogDirectTelephoneSampleNew();")
		'Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o20930")))
		'		Número de encuestas
		Dim oCot1 As New DAL.CotizadorGeneral
		'/driver.FindElement(By.Id("o20930")).SendKeys(oCot1.GetTotalMuestra(presup.IdPropuesta, presup.ParAlternativa, presup.MetCodigo, presup.ParNacional).ToString)
		driver.ExecuteScript("document.getElementById('o20930').value = '" & oCot1.GetTotalMuestra(presup.IdPropuesta, presup.ParAlternativa, presup.MetCodigo, presup.ParNacional).ToString & "';")
		'		Tipo de respondiente
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20940")))
		Selectionddl.SelectByIndex(1) 'Consumidor individual
		'/driver.FindElement(By.Id("o20940_comment")).SendKeys("n/a") 'Comentarios 
		driver.ExecuteScript("document.getElementById('o20940_comment').value = 'n/a';")
		'		Selección respondientes
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20945")))
		Selectionddl.SelectByIndex(1) 'Quota selection
		'/driver.FindElement(By.Id("o20945_comment")).SendKeys("n/a") 'Comentarios 
		driver.ExecuteScript("document.getElementById('o20945_comment').value = 'n/a';")
		'		Origen de muestra
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20965")))
		Selectionddl.SelectByIndex(2) 'Client supplied
		'/driver.FindElement(By.Id("o20965_comment")).SendKeys("n/a") 'Comentarios 
		driver.ExecuteScript("document.getElementById('o20965_comment').value = 'n/a';")
		'		Disponibilidad de muestra
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20970")))
		Selectionddl.SelectByIndex(3) '7:1 to 10:1
		'/driver.FindElement(By.Id("o20970_comment")).SendKeys("n/a") 'Comentarios 
		driver.ExecuteScript("document.getElementById('o20970_comment').value = 'n/a';")
		'		Incidencia
		Dim valIncidencia = Math.Round(CInt(presup.ParIncidencia), 0)
		If valIncidencia < 40 Then valIncidencia = 40
		'/driver.FindElement(By.Id("o20925")).SendKeys(Math.Round(CInt(presup.ParIncidencia), 0).ToString) 'Comentarios 
		driver.ExecuteScript("document.getElementById('o20925').value = '" & valIncidencia.ToString & "';")
		'/driver.FindElement(By.Id("o20925_comment")).SendKeys("n/a") 'Comentarios 
		driver.ExecuteScript("document.getElementById('o20925_comment').value = 'n/a';")
		'		Screener Lenght
		'/driver.FindElement(By.Id("o20980")).Clear()
		'/driver.FindElement(By.Id("o20980")).SendKeys(presup.ParTiempoEncuesta)
		driver.ExecuteScript("document.getElementById('o20980').value = '" & presup.ParTiempoEncuesta.ToString & "';")
		'		Duración encuesta
		'/driver.FindElement(By.Id("o20935")).SendKeys(presup.ParTiempoEncuesta)
		driver.ExecuteScript("document.getElementById('o20935').value = '" & presup.ParTiempoEncuesta.ToString & "';")
		'		Identificación cliente
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o20985")))
		Selectionddl.SelectByIndex(1) 'None / Unsure
		'/driver.FindElement(By.Id("o20985_comment")).SendKeys("n/a") 'Comentarios 
		driver.ExecuteScript("document.getElementById('o20985_comment').value = 'n/a';")
		driver.ExecuteScript("jsSectionSave_click(200);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogDirectTelephoneSampleDialogClose();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'	GUARDAR TELEFÓNICO
		driver.ExecuteScript("jsSectionSave_click(20);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		LoadCATI(driver, costosToIquote)
		driver.ExecuteScript("jsDialogDirectTelephoneWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))

	End Sub

	Sub ExportiQuoteOnline(ByRef driver As ChromeDriver, presup As DTO.IQ_Parametros, pGeneral As DTO.IQ_DatosGeneralesPresupuesto, costosToIquote As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		If Not presup.TecCodigo = "300" Then Exit Sub
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))
		waitdriver.IgnoreExceptionTypes(GetType(NoSuchElementException))
		' ---> SCRIPTING <---
		driver.ExecuteScript("jsDialogDirectScriptingWaveNew();")

		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o36230a")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o36230a")))
		'Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		'/driver.FindElement(By.Id("o36230a")).Clear()
		'/driver.FindElement(By.Id("o36230a")).SendKeys(NombreFase(presup.ParNacional)) ' Nombre de la fase
		driver.ExecuteScript("document.getElementById('o36230a').value = 'Campo Online - " & NombreFase(presup.ParNacional) & " - " & NombreMetodologia(presup.MetCodigo) & "';")
		driver.ExecuteScript("jsSaveLinkClick(36);")
		'Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		'		Setup en minutos
		'/driver.FindElement(By.Id("o36280")).Clear()
		'/driver.FindElement(By.Id("o36280")).SendKeys(presup.ParTiempoEncuesta.ToString)
		'/driver.FindElement(By.Id("o36380_comment")).SendKeys("") ' Comentarios
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o36280")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o36280")))
		driver.ExecuteScript("document.getElementById('o36280').value = '" & presup.ParTiempoEncuesta.ToString & "';")
		driver.ExecuteScript("document.getElementById('o36380_comment').value = 'n/a';")
		'		Tipo de scripting
		Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36380")))
		driver.ExecuteScript("document.getElementById('o36380').options.selectedIndex=1;")
		'Selectionddl.SelectByIndex(1) 'Data Collection Script
		'		Número de paises
		'/driver.FindElement(By.Id("o36270")).Clear()
		'/driver.FindElement(By.Id("o36270")).SendKeys("1")
		'/driver.FindElement(By.Id("o36270_comment")).SendKeys("") ' Comentarios
		driver.ExecuteScript("document.getElementById('o36270').value = '1';")
		driver.ExecuteScript("document.getElementById('o36270_comment').value = ' ';")
		'		Setup Survey en minutos
		'/driver.FindElement(By.Id("o36280")).Clear()
		'/driver.FindElement(By.Id("o36280")).SendKeys(presup.ParTiempoEncuesta.ToString)
		'/driver.FindElement(By.Id("o36280_comment")).SendKeys("") ' Comentarios
		driver.ExecuteScript("document.getElementById('o36280').value = '" & presup.ParTiempoEncuesta.ToString & "';")
		driver.ExecuteScript("document.getElementById('o36380_comment').value = 'n/a';")
		'		Complejidad del scripting
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o36295")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o36295")))
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36295")))
		If presup.DPComplejidadCuestionario IsNot Nothing Then
			Selectionddl.SelectByIndex(presup.DPComplejidadCuestionario)
		Else
			Selectionddl.SelectByIndex(1)
		End If
		'/driver.FindElement(By.Id("o36295_comment")).SendKeys("") ' Comentarios 
		driver.ExecuteScript("document.getElementById('o36295_comment').value = ' ';")
		'/driver.ExecuteScript("document.getElementById('o36380_comment').value = 'n/a';")
		'		Es Online? No
		If presup.TecCodigo = "300" Then
			driver.FindElement(By.Id("o36300_1")).Click()
		Else
			driver.FindElement(By.Id("o36300_0")).Click()
		End If
		'		Telefónico? Sí

		If presup.TecCodigo = "200" Then
			driver.FindElement(By.Id("o36305_1")).Click()
			driver.ExecuteScript("jsScriptingTelephoneRequired();")
			'Thread.Sleep(TimeSpan.FromSeconds(TimeShort
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o36456")))
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o36456")))
			Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36456")))
			Selectionddl.SelectByIndex(1) 'No quotas
		Else
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o36305_0")))
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o36305_0")))
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("o36305_0")))
			driver.FindElement(By.Id("o36305_0")).Click()
			driver.ExecuteScript("jsScriptingTelephoneRequired();")
		End If
		'		Es F2F? No
		If presup.TecCodigo = "100" Then
			driver.FindElement(By.Id("o36310_1")).Click()
		Else
			driver.FindElement(By.Id("o36310_0")).Click()
			driver.FindElement(By.Id("o36310_0")).Click()
		End If

		'	GUARDAR SCRIPTING
		'/driver.FindElement(By.Id("o36375_comment")).SendKeys("") ' Otros Comentarios generales
		driver.ExecuteScript("jsSectionSave_click(36);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogDirectScriptingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#proposal#" & NumIQuote)
		LoadOnline(driver, costosToIquote)
		'driver.ExecuteScript("jsDialogDirectScriptingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
	End Sub
	Sub ExportiQuoteF2F(ByRef driver As ChromeDriver, presup As DTO.IQ_Parametros, pGeneral As DTO.IQ_DatosGeneralesPresupuesto, costosToIquote As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))
		waitdriver.IgnoreExceptionTypes(GetType(NoSuchElementException))
		If Not presup.TecCodigo = "100" Then Exit Sub
		driver.ExecuteScript("javascript:jsDialogDirectF2FWaveNew();")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o39230a")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o39230a")))
		'waitdriver.Until(Function(x) x.FindElement(By.Id("o39230a")).Displayed)
		'Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		'/driver.FindElement(By.Id("o39230a")).Clear()
		'/driver.FindElement(By.Id("o39230a")).SendKeys(NombreFase(presup.ParNacional).ToString) ' Nombre de la fase
		driver.ExecuteScript("document.getElementById('o39230a').value = '" & NombreFase(presup.ParNacional).ToString & " - " & NombreMetodologia(presup.MetCodigo) & "';")
		driver.ExecuteScript("javascript:jsSaveLinkClick(39);")
		'Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		If pGeneral.NumeroMediciones > 1 Then
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("o39240_1")))
			driver.FindElement(By.Id("o39240_1")).Click()
			driver.ExecuteScript("jsF2FIsTracking();")
			'/driver.FindElement(By.Id("o39242")).Clear()
			'/driver.FindElement(By.Id("o39242")).SendKeys(pGeneral.NumeroMediciones.ToString) 'Número de olas
			driver.ExecuteScript("document.getElementById('o39242').value = '" & pGeneral.NumeroMediciones.ToString & "';")
		Else
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("o39240_0")))
			driver.FindElement(By.Id("o39240_0")).Click()
			driver.ExecuteScript("jsF2FIsTracking();")
		End If
		'		Tema
		Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39244")))
		Selectionddl.SelectByIndex(2) 'Consumidor mediano interés
		'Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		Try
			'/driver.FindElement(By.Id("o39244_comment")).SendKeys("n/a") 'Comentarios tema o grupo objetivo
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o39244_comment")))
			driver.ExecuteScript("document.getElementById('o39244_comment').value = 'n/a';")
		Catch ex As Exception

		End Try

		'		Es Product Test? Sí
		If presup.MetCodigo = 105 Or presup.MetCodigo = 120 Or presup.MetCodigo = 125 Then
			driver.FindElement(By.Id("o39302_1")).Click()
			driver.ExecuteScript("jsF2FIsThisProductChange();")
			'		Requiere compra de producto? Sí
			If Not presup.PTCompra Is Nothing Then
				If presup.PTCompra = True Then
					driver.FindElement(By.Id("o39250_1")).Click()
					'/driver.FindElement(By.Id("o39250_comment")).SendKeys("n/a") 'Comentarios compra de producto
					driver.ExecuteScript("document.getElementById('o39250_comment').value = 'n/a';")
				Else
					driver.FindElement(By.Id("o39250_0")).Click()
				End If
			Else
				driver.FindElement(By.Id("o39250_0")).Click()
			End If
			'		Tipo de producto
			Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39251")))
			Selectionddl.SelectByIndex(1) 'Small
			'		Productos por respondiente
			If Not presup.PTProductosEvaluar Is Nothing Then
				'/driver.FindElement(By.Id("o39252")).SendKeys(presup.PTProductosEvaluar.ToString) 'Comentarios tema o grupo objetivo
				driver.ExecuteScript("document.getElementById('o39252').value = '" & presup.PTProductosEvaluar.ToString & "';")
			Else
				'/driver.FindElement(By.Id("o39252")).SendKeys("1") 'Comentarios tema o grupo objetivo
				driver.ExecuteScript("document.getElementById('o39252').value = '1';")
			End If

		Else
			driver.FindElement(By.Id("o39302_0")).Click()
			driver.ExecuteScript("jsF2FIsThisProductChange();")
		End If
		'	GUARDAR F2F
		driver.ExecuteScript("jsSectionSave_click(39);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Especificaciones F2F - Stage
		driver.ExecuteScript("jsDialogDirectF2FStageNew();")
		'Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o39910")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o39910")))
		'/driver.FindElement(By.Id("o39910")).SendKeys("Main Interview") 'Nombre stage
		driver.ExecuteScript("document.getElementById('o39910').value = 'Main Interview';")
		'		Stage
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("o39915")))
		driver.ExecuteScript("document.getElementById('o39915').options.selectedIndex=1;")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("o39948")))
		driver.ExecuteScript("document.getElementById('o39948').options.selectedIndex=1;")
		''Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39915")))
		''Selectionddl.SelectByIndex(1) 'Main interview
		'		Data collection mode
		'Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39948")))
		'Selectionddl.SelectByIndex(1) 'Data collection mode - iField
		'		Duración días
		'/driver.FindElement(By.Id("o39955")).SendKeys(pGeneral.DiasCampo) 'Número de días en campo
		'/driver.FindElement(By.Id("o39960")).SendKeys(pGeneral.DiasCampo) 'Número de días iField
		driver.ExecuteScript("document.getElementById('o39955').value = '" & pGeneral.DiasCampo.ToString & "';")
		driver.ExecuteScript("document.getElementById('o39960').value = '" & pGeneral.DiasCampo.ToString & "';")
		'		Sample Strategy
		driver.ExecuteScript("document.getElementById('o39878').options.selectedIndex=2;")
		'Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39878")))
		'Selectionddl.SelectByIndex(2) 'Free find
		'		Geographical coverage
		driver.ExecuteScript("document.getElementById('o39874').options.selectedIndex=1;")
		'Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o39874")))
		'Selectionddl.SelectByIndex(1) 'National coverage
		'		Target group name
		'/driver.FindElement(By.Id("iqtMain_sample001_1")).SendKeys("General") '
		driver.ExecuteScript("document.getElementById('iqtMain_sample001_1').value = 'General';")
		'		Respondent type
		driver.ExecuteScript("document.getElementById('iqtMain_sample002_1').options.selectedIndex=2;")
		'Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample002_1")))
		'Selectionddl.SelectByIndex(2) 'Consumers individuals
		'		Respondent selection
		driver.ExecuteScript("document.getElementById('iqtMain_sample003_1').options.selectedIndex=1;")
		'Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample003_1")))
		'Selectionddl.SelectByIndex(1) 'Quota selection
		'		Cantidad e incidencia
		Dim oCot1 As New DAL.CotizadorGeneral
		'/driver.FindElement(By.Id("iqtMain_sample004_1")).SendKeys(oCot1.GetTotalMuestra(presup.IdPropuesta, presup.ParAlternativa, presup.MetCodigo, presup.ParNacional).ToString) 'Número de encustas
		'/driver.FindElement(By.Id("iqtMain_sample005_1")).SendKeys(Math.Round(CInt(presup.ParIncidencia), 0).ToString) 'Gen Pop Incidence
		'/driver.FindElement(By.Id("iqtMain_sample006_1")).SendKeys(Math.Round(CInt(presup.ParIncidencia), 0).ToString) 'Target incidence
		driver.ExecuteScript("document.getElementById('iqtMain_sample004_1').value = '" & oCot1.GetTotalMuestra(presup.IdPropuesta, presup.ParAlternativa, presup.MetCodigo, presup.ParNacional).ToString & "';")
		Dim IncidenciaVal As Integer = presup.ParIncidencia
		If IncidenciaVal < 31 Then
			IncidenciaVal = 31
		End If
		driver.ExecuteScript("document.getElementById('iqtMain_sample005_1').value = '" & Math.Round(CInt(IncidenciaVal), 0).ToString & "';")
		driver.ExecuteScript("document.getElementById('iqtMain_sample006_1').value = '" & Math.Round(CInt(IncidenciaVal), 0).ToString & "';")
		'		Quota control
		driver.ExecuteScript("document.getElementById('iqtMain_sample007_1').options.selectedIndex=1;")
		'Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample007_1")))
		'Selectionddl.SelectByIndex(1) 'Parallel flexible quota
		'		Quota categories
		driver.ExecuteScript("document.getElementById('iqtMain_sample008_1').options.selectedIndex=1;")
		'Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("iqtMain_sample008_1")))
		'Selectionddl.SelectByIndex(1) 'Standar quotas
		'		Duración
		'/driver.FindElement(By.Id("iqtMain_sample009_1")).SendKeys(presup.ParTiempoEncuesta.ToString) 'Screener lenght
		'/driver.FindElement(By.Id("iqtMain_sample010_1")).SendKeys(presup.ParTiempoEncuesta.ToString) 'Interview lenght
		driver.ExecuteScript("document.getElementById('iqtMain_sample009_1').value = '" & presup.ParTiempoEncuesta.ToString & "';")
		driver.ExecuteScript("document.getElementById('iqtMain_sample010_1').value = '" & presup.ParTiempoEncuesta.ToString & "';")

		driver.ExecuteScript("jsDialogDirectF2FStageSet(false,'none',0);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o39244_comment")))
		driver.ExecuteScript("jsDialogDirectF2FStageDialogClose();")
		'Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("F2FSaveButton")))

		driver.ExecuteScript("jsSectionSave_click(39);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("F2FSaveButton")))
		driver.ExecuteScript("jsDialogDirectF2FWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		LoadF2F(driver, costosToIquote)
		'driver.Manage.Window.Minimize()
		driver.ExecuteScript("jsDialogDirectF2FWaveClose_click();")
	End Sub

	Sub ExportiQuoteCodificacion(ByRef driver As ChromeDriver, presup As DTO.IQ_Parametros, pGeneral As DTO.IQ_DatosGeneralesPresupuesto, costosToIquote As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))
		' ---> CODIFICACIÓN <---
		driver.ExecuteScript("javascript:jsDialogDirectCodingWaveNew();")
		'Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		'/driver.FindElement(By.Id("o22233")).Clear()
		'/driver.FindElement(By.Id("o22233")).SendKeys(NombreFase(presup.ParNacional)) ' Nombre de la fase
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o22233")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o22233")))
		driver.ExecuteScript("document.getElementById('o22233').value = '" & NombreFase(presup.ParNacional).ToString & " - " & NombreMetodologia(presup.MetCodigo) & "';")

		driver.ExecuteScript("javascript:jsSaveLinkClick(22);")
		'Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		If pGeneral.NumeroMediciones > 1 Then
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o22240_1")))
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("o22240_1")))
			driver.FindElement(By.Id("o22240_1")).Click()
			driver.ExecuteScript("jsCodingIsTracking();")
			'/driver.FindElement(By.Id("o22242")).Clear()
			'/driver.FindElement(By.Id("o22242")).SendKeys(pGeneral.NumeroMediciones) 'Número de olas
			driver.ExecuteScript("document.getElementById('o22242').value = '" & pGeneral.NumeroMediciones & "';")
		Else
			'driver.FindElement(By.Id("o22240_0")).Click()
		End If
		Dim oCot1 As New DAL.CotizadorGeneral

		'		Número de encuestas
		'/driver.FindElement(By.Id("o22245")).SendKeys(oCot1.GetTotalMuestra(presup.IdPropuesta, presup.ParAlternativa, presup.MetCodigo, presup.ParNacional).ToString)
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o22245")))
		driver.ExecuteScript("document.getElementById('o22245').value = '" & oCot1.GetTotalMuestra(presup.IdPropuesta, presup.ParAlternativa, presup.MetCodigo, presup.ParNacional).ToString & "';")
		'		Complejidad de la codificación
		Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o22246")))
		If presup.ComplejidadCodificacion IsNot Nothing Then
			Selectionddl.SelectByIndex(presup.ComplejidadCodificacion) 'Standard
		Else
			Selectionddl.SelectByIndex(2) 'Standard
		End If
		driver.FindElement(By.Id("o22246_comment")).SendKeys("n/a") 'Comentarios 
		Dim IQ_Preguntas = oCot1.GetPreguntas(presup.IdPropuesta, presup.ParAlternativa, presup.MetCodigo, presup.ParNacional)
		'		Preguntas abiertas múltiples
		'/driver.FindElement(By.Id("o22250")).SendKeys(IQ_Preguntas.PregAbiertasMultiples.ToString)
		driver.ExecuteScript("document.getElementById('o22250').value = '" & IQ_Preguntas.PregAbiertasMultiples.ToString & "';")
		'		Complejidad de la codificación (cleaner preguntas)
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o22252")))
		Selectionddl.SelectByIndex(2) 'Standard
		'		Preguntas abiertas
		'/driver.FindElement(By.Id("o22254")).SendKeys(IQ_Preguntas.PregAbiertas)
		driver.ExecuteScript("document.getElementById('o22254').value = '" & IQ_Preguntas.PregAbiertas.ToString & "';")
		'		Preguntas otros
		'/driver.FindElement(By.Id("o22258")).SendKeys(IQ_Preguntas.PregOtras.ToString)
		driver.ExecuteScript("document.getElementById('o22258').value = '" & IQ_Preguntas.PregOtras.ToString & "';")
		driver.ExecuteScript("jsSectionSave_click(22);")
		'Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("codingSaveButton")))
		driver.ExecuteScript("jsDialogDirectCodingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		LoadCoding(driver, costosToIquote)
	End Sub

	Sub ExportiQuoteScripting(ByRef driver As ChromeDriver, presup As DTO.IQ_Parametros, pGeneral As DTO.IQ_DatosGeneralesPresupuesto, costosToIquote As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))
		waitdriver.IgnoreExceptionTypes(GetType(NoSuchElementException))
		' ---> SCRIPTING <---
		driver.ExecuteScript("jsDialogDirectScriptingWaveNew();")

		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o36230a")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o36230a")))
		'Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		'/driver.FindElement(By.Id("o36230a")).Clear()
		'/driver.FindElement(By.Id("o36230a")).SendKeys(NombreFase(presup.ParNacional)) ' Nombre de la fase
		driver.ExecuteScript("document.getElementById('o36230a').value = '" & NombreFase(presup.ParNacional) & " - " & NombreMetodologia(presup.MetCodigo) & "';")
		driver.ExecuteScript("jsSaveLinkClick(36);")
		'Thread.Sleep(TimeSpan.FromSeconds(TimeScript))

		'		Setup en minutos
		'/driver.FindElement(By.Id("o36280")).Clear()
		'/driver.FindElement(By.Id("o36280")).SendKeys(presup.ParTiempoEncuesta.ToString)
		'/driver.FindElement(By.Id("o36380_comment")).SendKeys("") ' Comentarios
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o36280")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o36280")))
		driver.ExecuteScript("document.getElementById('o36280').value = '" & presup.ParTiempoEncuesta.ToString & "';")
		driver.ExecuteScript("document.getElementById('o36380_comment').value = 'n/a';")
		'		Tipo de scripting
		Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36380")))
		driver.ExecuteScript("document.getElementById('o36380').options.selectedIndex=1;")
		'Selectionddl.SelectByIndex(1) 'Data Collection Script
		'		Número de paises
		'/driver.FindElement(By.Id("o36270")).Clear()
		'/driver.FindElement(By.Id("o36270")).SendKeys("1")
		'/driver.FindElement(By.Id("o36270_comment")).SendKeys("") ' Comentarios
		driver.ExecuteScript("document.getElementById('o36270').value = '1';")
		driver.ExecuteScript("document.getElementById('o36270_comment').value = ' ';")
		'		Setup Survey en minutos
		'/driver.FindElement(By.Id("o36280")).Clear()
		'/driver.FindElement(By.Id("o36280")).SendKeys(presup.ParTiempoEncuesta.ToString)
		'/driver.FindElement(By.Id("o36280_comment")).SendKeys("") ' Comentarios
		driver.ExecuteScript("document.getElementById('o36280').value = '" & presup.ParTiempoEncuesta.ToString & "';")
		driver.ExecuteScript("document.getElementById('o36380_comment').value = 'n/a';")
		'		Complejidad del scripting
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o36295")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o36295")))
		Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36295")))
		If presup.DPComplejidadCuestionario IsNot Nothing Then
			Selectionddl.SelectByIndex(presup.DPComplejidadCuestionario)
		Else
			Selectionddl.SelectByIndex(1)
		End If
		'/driver.FindElement(By.Id("o36295_comment")).SendKeys("") ' Comentarios 
		driver.ExecuteScript("document.getElementById('o36295_comment').value = ' ';")
		'/driver.ExecuteScript("document.getElementById('o36380_comment').value = 'n/a';")
		'		Es Online? No
		If presup.TecCodigo = "300" Then
			driver.FindElement(By.Id("o36300_1")).Click()
		Else
			driver.FindElement(By.Id("o36300_0")).Click()
		End If
		'		Telefónico? Sí

		If presup.TecCodigo = "200" Then
			driver.FindElement(By.Id("o36305_1")).Click()
			driver.ExecuteScript("jsScriptingTelephoneRequired();")
			'Thread.Sleep(TimeSpan.FromSeconds(TimeShort
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o36456")))
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o36456")))
			Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o36456")))
			Selectionddl.SelectByIndex(1) 'No quotas
		Else
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o36305_0")))
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o36305_0")))
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("o36305_0")))
			driver.FindElement(By.Id("o36305_0")).Click()
			driver.ExecuteScript("jsScriptingTelephoneRequired();")
		End If
		'		Es F2F? No
		If presup.TecCodigo = "100" Then
			driver.FindElement(By.Id("o36310_1")).Click()
		Else
			driver.FindElement(By.Id("o36310_0")).Click()
			driver.FindElement(By.Id("o36310_0")).Click()
		End If

		'	GUARDAR SCRIPTING
		'/driver.FindElement(By.Id("o36375_comment")).SendKeys("") ' Otros Comentarios generales
		driver.ExecuteScript("jsSectionSave_click(36);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogDirectScriptingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		'driver.Navigate.GoToUrl("http://nwb.ipsos.com/iquote/#proposal#" & NumIQuote)
		LoadScripting(driver, costosToIquote)
	End Sub

	Sub ExportiQuoteDP(ByRef driver As ChromeDriver, presup As DTO.IQ_Parametros, pGeneral As DTO.IQ_DatosGeneralesPresupuesto, costosToIquote As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		driver.ExecuteScript("javascript:jsDialogDirectDataProcessingWaveNew();")
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))
		'Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		'/driver.FindElement(By.Id("o21230a")).Clear()
		'/driver.FindElement(By.Id("o21230a")).SendKeys(NombreFase(presup.ParNacional)) ' Nombre de la fase
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("o21230a")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o21230a")))
		driver.ExecuteScript("document.getElementById('o21230a').value = '" & NombreFase(presup.ParNacional).ToString & " - " & NombreMetodologia(presup.MetCodigo) & "';")
		driver.ExecuteScript("javascript:jsSaveLinkClick(21);")
		'Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'		Es Tracking? Sí
		If pGeneral.MesesMediciones > 1 Then
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("o21250_1")))
			driver.FindElement(By.Id("o21250_1")).Click()
			driver.ExecuteScript("jsDataProcessingIsTracking();")

			'/driver.FindElement(By.Id("o21260")).Clear()
			'/driver.FindElement(By.Id("o21260")).SendKeys(pGeneral.NumeroMediciones) 'Número de olas
			driver.ExecuteScript("document.getElementById('o21260').value = '" & pGeneral.NumeroMediciones & "';")
			'/driver.FindElement(By.Id("oDataProcessingsw_none")).Clear()
			'/driver.FindElement(By.Id("oDataProcessingsw_none")).SendKeys((pGeneral.NumeroMediciones - 1).ToString) 'Cambios
			driver.ExecuteScript("document.getElementById('oDataProcessingsw_none').value = '" & (pGeneral.NumeroMediciones - 1).ToString & "';")
			'		Tipo de Ola
			Dim Selectionddl1 As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o21255")))
			Selectionddl1.SelectByIndex(1) 'Setup wave / New study
			'/driver.FindElement(By.Id("o21255_comment")).SendKeys("n/a") 'Comentarios 
			driver.ExecuteScript("document.getElementById('o21255_comment').value = 'n/a';")

		Else
			waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("o21250_0")))
			driver.FindElement(By.Id("o21250_0")).Click()
			driver.ExecuteScript("jsDataProcessingIsTracking();")
		End If

		'		Origen de la data
		driver.ExecuteScript("document.getElementById('o21290').options.selectedIndex=1;")
		'Dim Selectionddl As New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o21290")))
		'Selectionddl.SelectByIndex(1) 'Ipsos OPS Collected Data
		'/driver.FindElement(By.Id("o21290_comment")).SendKeys("n/a") 'Comentarios 
		driver.ExecuteScript("document.getElementById('o21290_comment').value = 'n/a';")
		'		Survey Setup length
		'/driver.FindElement(By.Id("o21280")).SendKeys(presup.ParTiempoEncuesta.ToString)
		driver.ExecuteScript("document.getElementById('o21280').value = '" & presup.ParTiempoEncuesta.ToString & "';")
		'		Requiere datacleaning? Sí
		If presup.ParNProcesosDC > 0 Then
			driver.FindElement(By.Id("o21285_1")).Click()
			driver.ExecuteScript("jsDataProcessingIsTracking();")
		Else
			driver.FindElement(By.Id("o21285_0")).Click()
			driver.ExecuteScript("jsDataProcessingIsTracking();")
		End If
		'		Complejidad de tablas
		Dim Selectionddl = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id("o21295")))
		If presup.DPComplejidad IsNot Nothing Then
			Selectionddl.SelectByIndex(presup.DPComplejidad) 'Standard
		Else
			Selectionddl.SelectByIndex(2) 'Standard
		End If
		driver.FindElement(By.Id("o21295_comment")).SendKeys("n/a") 'Comentarios 
		driver.ExecuteScript("document.getElementById('o21295_comment').value = 'n/a';")
		'		Es CATI? SI
		If presup.TecCodigo = "200" Then
			driver.FindElement(By.Id("o21315_1")).Click()
		Else
			driver.FindElement(By.Id("o21315_0")).Click()
		End If
		'		Es ONLINE? SI
		If presup.TecCodigo = "300" Then
			driver.FindElement(By.Id("o21320_1")).Click()
		Else
			driver.FindElement(By.Id("o21320_0")).Click()
		End If
		'		Es HandHeld? SI
		driver.FindElement(By.Id("o21335_1")).Click()
		driver.ExecuteScript("jsSectionSave_click(21);")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("dataProcessingSaveButton")))
		driver.ExecuteScript("jsDialogDirectDataProcessingWaveClose_click();")
		Thread.Sleep(TimeSpan.FromSeconds(2))
		driver.ExecuteScript("jsSectionSave_click(0);")
		LoadDP(driver, costosToIquote)

	End Sub

	Sub ExportiQuoteProfessionalTime(ByRef driver As ChromeDriver, IdPropuesta As Int64, Alternativa As Int32)
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))
		Dim oHorasProfesionales = ObtenerHorasProfesionalesXAlternativa(IdPropuesta, Alternativa).Where(Function(x) x.TotalHoras > 0).ToList
		If oHorasProfesionales.Count = 0 Then
			Exit Sub
		End If
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("toggleImg_5_")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("toggleImg_5_")))
		driver.FindElement(By.Id("toggleImg_5_")).Click()
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		For Each level In oHorasProfesionales
			If level.TotalHoras > 0 Then
				Try
					driver.ExecuteScript("jsDetailGlobalProfTimeAdd();")
				Catch ex As Exception
					driver.SwitchTo().Alert().Dismiss()
					Try
						'driver.ExecuteScript("jsDetailGlobalProfTimeAdd();")
						driver.SwitchTo().Alert().Dismiss()
					Catch ex2 As Exception
						driver.SwitchTo().Alert().Dismiss()
					End Try
				End Try
			End If
		Next
		Try
			driver.SwitchTo().Alert().Dismiss()
			driver.SwitchTo().Alert().Dismiss()
		Catch ex As Exception

		End Try
		Dim tabHoras As IWebElement
		Try
			tabHoras = driver.FindElement(By.Id("o5701"))
		Catch ex As Exception
			Try
				driver.SwitchTo().Alert().Dismiss()
				driver.SwitchTo().Alert().Dismiss()
			Catch ex2 As Exception

			End Try
		End Try
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		tabHoras = driver.FindElement(By.Id("o5701"))
		Dim lstSelect As IList(Of IWebElement) = tabHoras.FindElements(By.TagName("Select"))
		While lstSelect.Count < oHorasProfesionales.Count
			Try
				driver.ExecuteScript("jsDetailGlobalProfTimeAdd();")
				Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
			Catch ex As Exception
				Try
					driver.SwitchTo().Alert().Dismiss()
				Catch ex2 As Exception
					Try
						driver.SwitchTo().Alert().Dismiss()
					Catch ex3 As Exception
						driver.SwitchTo().Alert().Dismiss()
					End Try
				End Try
			End Try
			Try
				lstSelect = tabHoras.FindElements(By.TagName("Select"))
			Catch ex As Exception
				driver.SwitchTo().Alert().Dismiss()
			End Try
			'lstSelect = tabHoras.FindElements(By.TagName("Select"))
			Try
				lstSelect = tabHoras.FindElements(By.TagName("Select"))
			Catch ex As Exception
				driver.SwitchTo().Alert().Dismiss()
				Exit While
			End Try

		End While
		Try
			driver.SwitchTo().Alert().Dismiss()
			driver.SwitchTo().Alert().Dismiss()
		Catch ex As Exception

		End Try
		Try
			lstSelect = tabHoras.FindElements(By.TagName("Select"))
		Catch ex As Exception
			Try
				driver.SwitchTo().Alert().Dismiss()
			Catch ex2 As Exception
			End Try
			lstSelect = tabHoras.FindElements(By.TagName("Select"))
		End Try
		Dim indSelect As Integer = 0
		For Each elem In lstSelect
			If indSelect = oHorasProfesionales.Count Then Exit For
			Dim SelectElement As New OpenQA.Selenium.Support.UI.SelectElement(elem)
			SelectElement.SelectByIndex(oHorasProfesionales(indSelect).PGrCodigo - 3001)
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours2") & "').value = '" & oHorasProfesionales(indSelect).PreField.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours3") & "').value = '" & oHorasProfesionales(indSelect).FieldWork.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours4") & "').value = '" & oHorasProfesionales(indSelect).DPandCoding.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours5") & "').value = '" & oHorasProfesionales(indSelect).Analysis.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours6") & "').value = '" & oHorasProfesionales(indSelect).PM.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours8") & "').value = '" & oHorasProfesionales(indSelect).Meetings.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours10") & "').value = '" & oHorasProfesionales(indSelect).Presentation.ToString & "';")
			driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("level", "hours9") & "').value = '" & oHorasProfesionales(indSelect).ClientTravel.ToString & "';")
			indSelect += 1
		Next
		driver.ExecuteScript("jsSectionSave_click(5);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
	End Sub

	Sub ExportiQuoteSubContract(ByRef driver As ChromeDriver, IdPropuesta As Int64, Alternativa As Int32)

		Dim oCostosSubcontratados = ObtenerActividadesSubcontratadas(IdPropuesta, Alternativa, "Subcontract")
		If oCostosSubcontratados.Count = 0 Then
			Exit Sub
		End If
		driver.FindElement(By.Id("toggleImg_9_")).Click()
		Thread.Sleep(TimeSpan.FromSeconds(TimeWindows))
		For i As Integer = 1 To oCostosSubcontratados.Count - 1
			Try
				driver.ExecuteScript("jsDetailHomeSubContractAdd();")
				Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
			Catch ex As Exception
				Try
					driver.SwitchTo().Alert().Dismiss()
				Catch ex2 As Exception

				End Try
			End Try
		Next
		Dim tabElements As IWebElement
		Try
			tabElements = driver.FindElement(By.Id("o9600"))
		Catch ex As Exception
			Try
				driver.SwitchTo().Alert().Dismiss()
				driver.SwitchTo().Alert().Dismiss()
			Catch ex2 As Exception

			End Try
		End Try
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		tabElements = driver.FindElement(By.Id("o9600"))
		Dim lstSelect As IList(Of IWebElement) = tabElements.FindElements(By.TagName("Select"))
		Try
			lstSelect = tabElements.FindElements(By.TagName("Select"))
		Catch ex As Exception
			Try
				driver.SwitchTo().Alert().Dismiss()
			Catch ex2 As Exception
			End Try
			lstSelect = tabElements.FindElements(By.TagName("Select"))
		End Try

		Dim indSelect As Integer = 0
		For Each elem In lstSelect
			If indSelect = oCostosSubcontratados.Count Then Exit For
			If elem.GetDomAttribute("id").ToString.Contains("costType") Then
				Dim SelectElement As New OpenQA.Selenium.Support.UI.SelectElement(elem)
				SelectElement.SelectByIndex(1)
				SelectElement = New OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(By.Id(elem.GetDomAttribute("id").ToString.Replace("costType", "subcontractType"))))
				SelectElement.SelectByIndex(oCostosSubcontratados(indSelect).OrderIQuote)
				'driver.FindElement(By.Id("o21255"))

				driver.ExecuteScript("document.getElementById('" & elem.GetDomAttribute("id").ToString.Replace("costType", "unitCost") & "').value = '" & oCostosSubcontratados(indSelect).Valor.ToString & "';")
				indSelect += 1
			End If
		Next
		driver.ExecuteScript("jsSectionSave_click(9);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
	End Sub

	Sub ExportiQuoteOtherDirectCosts(ByRef driver As ChromeDriver, presup As DTO.IQ_Parametros, costs As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		Dim oCot As New DAL.CotizadorGeneral
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("toggleImg_10_")))
		driver.FindElement(By.Id("toggleImg_10_")).Click()
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))

		'Dim tabElements As IWebElement
		'Try
		'	tabElements = driver.FindElement(By.Id("o10600"))
		'Catch ex As Exception
		'	Try
		'		driver.SwitchTo().Alert().Dismiss()
		'		driver.SwitchTo().Alert().Dismiss()
		'	Catch ex2 As Exception

		'	End Try
		'End Try
		'Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		'tabElements = driver.FindElement(By.Id("o10600"))
		'Dim lstSelect As IList(Of IWebElement) = tabElements.FindElements(By.TagName("Select"))
		'Try
		'	lstSelect = tabElements.FindElements(By.TagName("Select"))
		'Catch ex As Exception
		'	Try
		'		driver.SwitchTo().Alert().Dismiss()
		'	Catch ex2 As Exception
		'	End Try
		'	lstSelect = tabElements.FindElements(By.TagName("Select"))
		'End Try

		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("o10600")))

		Dim TabOtherDC = driver.FindElement(By.Id("o10600"))
		If costs.Where(Function(x) x.id = 30).ToList.Count > 0 Then
			driver.ExecuteScript("javascript:jsDetailHomeOtherDirectCostsAdd();")
			Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
			Dim rowsSummary = TabOtherDC.FindElements(By.TagName("tr"))
			Dim rowToSelect = rowsSummary(rowsSummary.Count - 2)
			Dim selectElements = rowToSelect.FindElements(By.TagName("select"))
			For Each selElement In selectElements
				If selElement.GetDomAttribute("id").Contains("costType") Then
					driver.ExecuteScript("document.getElementById('" & selElement.GetDomAttribute("id").ToString & "').value=5024")
				End If
			Next
			Dim inputElements = rowToSelect.FindElements(By.TagName("input"))
			For Each inpElemen In inputElements
				If inpElemen.GetDomAttribute("id").Contains("comment") Then
					driver.ExecuteScript("document.getElementById('" & inpElemen.GetDomAttribute("id").ToString & "').value='" & oCot.GetMetodologia(presup.MetCodigo) & " - " & oCot.GetFase(presup.ParNacional) & "'")
				End If
				If inpElemen.GetDomAttribute("id").Contains("unitCost") Then
					driver.ExecuteScript("document.getElementById('" & inpElemen.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 30).First.Valor)
				End If
			Next
		End If
		If costs.Where(Function(x) x.id = 31).ToList.Count > 0 Then
			driver.ExecuteScript("javascript:jsDetailHomeOtherDirectCostsAdd();")
			Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
			Dim rowsSummary = TabOtherDC.FindElements(By.TagName("tr"))
			Dim rowToSelect = rowsSummary(rowsSummary.Count - 2)
			Dim selectElements = rowToSelect.FindElements(By.TagName("select"))
			For Each selElement In selectElements
				If selElement.GetDomAttribute("id").Contains("costType") Then
					driver.ExecuteScript("document.getElementById('" & selElement.GetDomAttribute("id").ToString & "').value=5022")
				End If
			Next
			Dim inputElements = rowToSelect.FindElements(By.TagName("input"))
			For Each inpElemen In inputElements
				If inpElemen.GetDomAttribute("id").Contains("comment") Then
					driver.ExecuteScript("document.getElementById('" & inpElemen.GetDomAttribute("id").ToString & "').value='" & oCot.GetMetodologia(presup.MetCodigo) & " - " & oCot.GetFase(presup.ParNacional) & "'")
				End If
				If inpElemen.GetDomAttribute("id").Contains("unitCost") Then
					driver.ExecuteScript("document.getElementById('" & inpElemen.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 31).First.Valor)
				End If
			Next
		End If
		If costs.Where(Function(x) x.id = 32).ToList.Count > 0 Then
			driver.ExecuteScript("javascript:jsDetailHomeOtherDirectCostsAdd();")
			Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
			Dim rowsSummary = TabOtherDC.FindElements(By.TagName("tr"))
			Dim rowToSelect = rowsSummary(rowsSummary.Count - 2)
			Dim selectElements = rowToSelect.FindElements(By.TagName("select"))
			For Each selElement In selectElements
				If selElement.GetDomAttribute("id").Contains("costType") Then
					driver.ExecuteScript("document.getElementById('" & selElement.GetDomAttribute("id").ToString & "').value=5027")
				End If
			Next
			Dim inputElements = rowToSelect.FindElements(By.TagName("input"))
			For Each inpElemen In inputElements
				If inpElemen.GetDomAttribute("id").Contains("comment") Then
					driver.ExecuteScript("document.getElementById('" & inpElemen.GetDomAttribute("id").ToString & "').value='" & oCot.GetMetodologia(presup.MetCodigo) & " - " & oCot.GetFase(presup.ParNacional) & "'")
				End If
				If inpElemen.GetDomAttribute("id").Contains("unitCost") Then
					driver.ExecuteScript("document.getElementById('" & inpElemen.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 32).First.Valor)
				End If
			Next
		End If

		driver.ExecuteScript("jsSectionSave_click(10);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.FindElement(By.Id("toggleImg_10_")).Click()
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))

	End Sub

#Region "Funciones"
	Private Function ObtenerProcesos(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of DTO.IQ_ProcesosPresupuesto)
		Dim oCot As New DAL.CotizadorGeneral
		Return oCot.GetProcesos(idPropuesta, Alternativa, Metodologia, Fase)
	End Function

	Private Function ObtenerHorasProfesionalesXAlternativa(ByVal idPropuesta As Int64, Alternativa As Integer) As List(Of DTO.IQ_ObtenerHorasProfesionalesXAlternativa_Result)
		Dim oCot As New DAL.CotizadorGeneral
		Return oCot.GetHorasProfesionalesByAlternativa(idPropuesta, Alternativa)
	End Function

	Private Function ObtenerActividadesSubcontratadas(ByVal idPropuesta As Int64, Alternativa As Integer, TipoCosto As String) As List(Of DTO.IQ_COSTOACTIVIDADES_GET_TO_IQUOTE_Result)
		Dim oCot As New DAL.CotizadorGeneral
		Return oCot.GetCostoActividadesIQuote(idPropuesta, Alternativa, TipoCosto)
	End Function

	Private Function ObtenerCostosToIquote(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of DTO.IQ_CostosOPS_ToIQuote_Result)
		Dim oCot As New DAL.CotizadorGeneral
		Return oCot.GetCalculosOPSToIQUOTE(idPropuesta, Alternativa, Metodologia, Fase)
	End Function

	Function NombreFase(ByVal idFase As Int32) As String
		'Dim oCot As New DAL.CotizadorGeneral
		If idFase = 1 Then
			Return "Nacional"
		Else
			Return "Fase " & (idFase - 2).ToString
		End If
		'Return oCot.GetFase(idFase)
	End Function

	Function NombreMetodologia(ByVal idMetod As Int32) As String
		Dim oCot As New DAL.CotizadorGeneral
		Return oCot.GetMetodologia(idMetod)
	End Function

	Private Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
		LoadAlternativasToExport()
	End Sub

	Sub LoadAlternativasToExport()
		Dim oCot As New DAL.CotizadorGeneral
		dgvAlternativas.DataSource = oCot.GetAlternativasToExport(txtUsuario.Text, txtPassword.Text)
		dgvAlternativas.Refresh()
	End Sub

	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		driver.Navigate.GoToUrl("https://nwb.ipsos.com/")

		Dim btnEnter = driver.FindElement(By.Id("wsfOauthLogonButton"))
		btnEnter.Click()

		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 5, 0))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlToBe("https://nwb.ipsos.com/JobExec/JobExecutionHome.aspx"))
	End Sub

#End Region

#Region "Varios"
	'	tabField=document.getElementById("o39610");
	'rowsField=tabField.getElementsByTagName("tr");
	'jsDialogDirectF2FWaveSumOpen(rowsField[1].id.substr(7));
	'jsDialogDirectF2FWaveSumOpen(215041);

	'tabXSUM39=document.getElementById("XSUM_39_OPS");
	'rowsXSUM39=tabXSUM39.getElementsByTagName("tr");
	'inputXSUM39=rowsXSUM39[0].getElementsByTagName("input");
	'inputXSUM39=rowsXSUM39[1].getElementsByTagName("input");
#End Region

	Sub LoadCATI(ByRef driver As ChromeDriver, costs As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))

		Dim TabF2f = driver.FindElement(By.Id("o20610"))
		Dim rowsSummary = TabF2f.FindElements(By.TagName("tr"))
		Dim rowToSelect = rowsSummary(rowsSummary.Count - 2)

		driver.ExecuteScript("jsDialogDirectTelephoneWaveSumOpen(" & rowToSelect.GetDomAttribute("xdetailkey").ToString & ");")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("iqtXSUM_OPS")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("iqtXSUM_OPS")))

		Dim iElement As IWebElement
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("XSUM_OPS")))
		iElement = driver.FindElement(By.Id("XSUM_OPS"))
		Dim iRows As IList(Of IWebElement) = iElement.FindElements(By.TagName("tr"))

		'Tel Interviewing Home
		Dim inpuitXSUM39 = iRows(0).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitsOverride") Then
				If costs.Where(Function(x) x.id = 21).ToList.Count > 0 Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 21).First.Unidades.ToString.Replace(",", "."))
				Else
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				End If
				Exit For
			End If
		Next

		'Briefing Hours
		inpuitXSUM39 = iRows(2).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitsOverride") Then
				driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				Exit For
			End If
		Next

		'TEL Supervision/QC Hours
		inpuitXSUM39 = iRows(3).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitsOverride") Then
				If costs.Where(Function(x) x.id = 22).ToList.Count > 0 Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 22).First.Unidades.ToString.Replace(",", "."))
				Else
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				End If
				Exit For
			End If
		Next

		'Telephone Charges
		inpuitXSUM39 = iRows(13).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
				If costs.Where(Function(x) x.id = 23).ToList.Count > 0 Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 23).First.CostoOPS)
				Else
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				End If
				Exit For
			End If
		Next

		'Miscellanous costs
		inpuitXSUM39 = iRows(16).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
				If costs.Where(Function(x) x.id = 13).ToList.Count > 0 Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 13).First.CostoOPS)
				Else
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				End If
				Exit For
			End If
		Next


		driver.ExecuteScript("jsDialogDirectXSUM_OPSSet();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmountOpen(event);")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("XSUM_OPS_startingAmount")))
		Dim suma = costs.Where(Function(y) y.id = 13 Or y.id = 21 Or y.id = 22 Or y.id = 23).Sum(Function(x) x.VentaOPS)
		driver.ExecuteScript("document.getElementById('XSUM_OPS_startingAmount').value=" & costs.Where(Function(y) y.id = 13 Or y.id = 21 Or y.id = 22 Or y.id = 23).Sum(Function(x) x.VentaOPS))
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmount_click(1);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmountClose();")
		driver.ExecuteScript("jsDialogDirectXSUM_OPSClose();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
	End Sub

	Sub LoadOnline(ByRef driver As ChromeDriver, costs As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))

		Dim TabF2f = driver.FindElement(By.Id("o36610"))
		Dim rowsSummary = TabF2f.FindElements(By.TagName("tr"))
		Dim rowToSelect = rowsSummary(rowsSummary.Count - 2)

		driver.ExecuteScript("jsDialogDirectScriptingWaveSumOpen(" & rowToSelect.GetDomAttribute("xdetailkey").ToString & ");")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("iqtXSUM_OPS")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("iqtXSUM_OPS")))

		Dim iElement As IWebElement
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("XSUM_OPS")))
		iElement = driver.FindElement(By.Id("XSUM_OPS"))
		Dim iRows As IList(Of IWebElement) = iElement.FindElements(By.TagName("tr"))
		Dim inpuitXSUM39 = iRows(0).FindElements(By.TagName("input"))

		inpuitXSUM39 = iRows(0).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitsOverride") Then
				driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				Exit For
			End If
		Next

		inpuitXSUM39 = iRows(3).FindElements(By.TagName("input"))
		If costs.Where(Function(x) x.id = 13).ToList.Count > 0 Then
			For Each inputelement In inpuitXSUM39
				If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 13).First.CostoOPS)
					Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
				End If
			Next
		End If

		driver.ExecuteScript("jsDialogDirectXSUM_OPSSet();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmountOpen(event);")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("XSUM_OPS_startingAmount")))
		driver.ExecuteScript("document.getElementById('XSUM_OPS_startingAmount').value=" & costs.Where(Function(x) x.id = 13).First.VentaOPS)
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmount_click(1);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmountClose();")
		driver.ExecuteScript("jsDialogDirectXSUM_OPSClose();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
	End Sub
	Sub LoadF2F(ByRef driver As ChromeDriver, costs As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))

		Dim TabF2f = driver.FindElement(By.Id("o39610"))
		Dim rowsSummary = TabF2f.FindElements(By.TagName("tr"))
		Dim rowToSelect = rowsSummary(rowsSummary.Count - 2)

		driver.ExecuteScript("jsDialogDirectF2FWaveSumOpen(" & rowToSelect.GetDomAttribute("xdetailkey").ToString & ");")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("iqtXSUM_39_F2F_OPS")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("iqtXSUM_39_F2F_OPS")))

		Dim iElement As IWebElement
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("iqtXSUM_39_F2F_OPSSum_gridHeader")))
		iElement = driver.FindElement(By.Id("XSUM_39_OPS"))
		Dim iRows As IList(Of IWebElement) = iElement.FindElements(By.TagName("tr"))

		Dim inpuitXSUM39 = iRows(0).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
				If costs.Where(Function(x) x.id = 14).ToList.Count > 0 Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 14).First.CostoOPS)
				Else
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				End If
				Exit For
			End If
		Next


		inpuitXSUM39 = iRows(9).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
				If costs.Where(Function(x) x.id = 15).ToList.Count > 0 Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 15).First.CostoOPS)
				Else
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				End If
				Exit For
			End If
		Next


		inpuitXSUM39 = iRows(18).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
				If costs.Where(Function(x) x.id = 16).ToList.Count > 0 Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 16).First.CostoOPS)
				Else
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				End If
				Exit For
			End If
		Next


		inpuitXSUM39 = iRows(36).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
				If costs.Where(Function(x) x.id = 12).ToList.Count > 0 Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 12).First.CostoOPS)
				Else
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				End If
				Exit For
			End If
		Next

		inpuitXSUM39 = iRows(45).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
				driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				Exit For
			End If
		Next

		inpuitXSUM39 = iRows(49).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
				If costs.Where(Function(x) x.id = 10).ToList.Count > 0 Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 10).First.CostoOPS)
				Else
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				End If
				Exit For
			End If
		Next


		inpuitXSUM39 = iRows(50).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
				If costs.Where(Function(x) x.id = 11).ToList.Count > 0 Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 11).First.CostoOPS)
				Else
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				End If
				Exit For
			End If
		Next

		inpuitXSUM39 = iRows(51).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
				If costs.Where(Function(x) x.id = 20).ToList.Count > 0 Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 20).First.CostoOPS)
				Else
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				End If
				Exit For
			End If
		Next


		inpuitXSUM39 = iRows(54).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
				If costs.Where(Function(x) x.id = 13).ToList.Count > 0 Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 13).First.CostoOPS)
				Else
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
				End If
				Exit For
			End If
		Next

		inpuitXSUM39 = iRows(55).FindElements(By.TagName("input"))
		For Each inputelement In inpuitXSUM39
			If inputelement.GetDomAttribute("id").Contains("unitCostOverride") Then
				driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=0")
			End If
		Next
		driver.ExecuteScript("jsDialogDirectXSUM_39_OPSSet();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogXSUM_39_OPSActualSalesStartingAmountOpen(event);")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("XSUM_39_OPS_startingAmount")))
		Dim suma = costs.Where(Function(y) y.Unidades Is Nothing And Not (y.id = 30 Or y.id = 31 Or y.id = 32)).Sum(Function(x) x.VentaOPS)
		driver.ExecuteScript("document.getElementById('XSUM_39_OPS_startingAmount').value=" & costs.Where(Function(y) y.Unidades Is Nothing And Not (y.id = 30 Or y.id = 31 Or y.id = 32)).Sum(Function(x) x.VentaOPS))
		driver.ExecuteScript("jsDialogXSUM_39_OPSActualSalesStartingAmount_click(1);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		driver.ExecuteScript("jsDialogXSUM_39_OPSActualSalesStartingAmountClose();")
		driver.ExecuteScript("jsDialogDirectXSUM_39_OPSClose();")
	End Sub

	Sub LoadScripting(ByRef driver As ChromeDriver, costs As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))

		Dim TabF2f = driver.FindElement(By.Id("o36610"))
		Dim rowsSummary = TabF2f.FindElements(By.TagName("tr"))
		Dim rowToSelect = rowsSummary(rowsSummary.Count - 2)

		driver.ExecuteScript("jsDialogDirectScriptingWaveSumOpen(" & rowToSelect.GetDomAttribute("xdetailkey").ToString & ");")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("iqtXSUM_OPS")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("iqtXSUM_OPS")))

		Dim iElement As IWebElement
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("XSUM_OPS")))
		iElement = driver.FindElement(By.Id("XSUM_OPS"))
		Dim iRows As IList(Of IWebElement) = iElement.FindElements(By.TagName("tr"))
		Dim inpuitXSUM39 = iRows(0).FindElements(By.TagName("input"))
		If costs.Where(Function(x) x.id = 18).ToList.Count > 0 Then
			For Each inputelement In inpuitXSUM39
				If inputelement.GetDomAttribute("id").Contains("unitsOverride") Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 18).First.Unidades.ToString.Replace(",", "."))
					Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
				End If
			Next
		End If

		driver.ExecuteScript("jsDialogDirectXSUM_OPSSet();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmountOpen(event);")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("XSUM_OPS_startingAmount")))
		driver.ExecuteScript("document.getElementById('XSUM_OPS_startingAmount').value=" & costs.Where(Function(x) x.id = 18).First.VentaOPS)
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmount_click(1);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmountClose();")
		driver.ExecuteScript("jsDialogDirectXSUM_OPSClose();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
	End Sub

	Sub LoadDP(ByRef driver As ChromeDriver, costs As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))

		Dim TabF2f = driver.FindElement(By.Id("o21610"))
		Dim rowsSummary = TabF2f.FindElements(By.TagName("tr"))
		Dim rowToSelect = rowsSummary(rowsSummary.Count - 2)

		driver.ExecuteScript("jsDialogDirectDataProcessingWaveSumOpen(" & rowToSelect.GetDomAttribute("xdetailkey").ToString & ");")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("iqtXSUM_OPS")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("iqtXSUM_OPS")))

		Dim iElement As IWebElement
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("XSUM_OPS")))
		iElement = driver.FindElement(By.Id("XSUM_OPS"))
		Dim iRows As IList(Of IWebElement) = iElement.FindElements(By.TagName("tr"))
		Dim inpuitXSUM39 = iRows(0).FindElements(By.TagName("input"))
		If costs.Where(Function(x) x.id = 19).ToList.Count > 0 Then
			For Each inputelement In inpuitXSUM39
				If inputelement.GetDomAttribute("id").Contains("unitsOverride") Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 19).First.Unidades.ToString.Replace(",", "."))
					Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
				End If
			Next
		End If

		driver.ExecuteScript("jsDialogDirectXSUM_OPSSet();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmountOpen(event);")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("XSUM_OPS_startingAmount")))
		driver.ExecuteScript("document.getElementById('XSUM_OPS_startingAmount').value=" & costs.Where(Function(x) x.id = 19).First.VentaOPS)
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmount_click(1);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmountClose();")
		driver.ExecuteScript("jsDialogDirectXSUM_OPSClose();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
	End Sub

	Sub LoadCoding(ByRef driver As ChromeDriver, costs As List(Of DTO.IQ_CostosOPS_ToIQuote_Result))
		Dim waitdriver As New WebDriverWait(driver, New TimeSpan(0, 0, 30))

		Dim TabF2f = driver.FindElement(By.Id("o22610"))
		Dim rowsSummary = TabF2f.FindElements(By.TagName("tr"))
		Dim rowToSelect = rowsSummary(rowsSummary.Count - 2)

		driver.ExecuteScript("jsDialogDirectCodingWaveSumOpen(" & rowToSelect.GetDomAttribute("xdetailkey").ToString & ");")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("iqtXSUM_OPS")))
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("iqtXSUM_OPS")))

		Dim iElement As IWebElement
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("XSUM_OPS")))
		iElement = driver.FindElement(By.Id("XSUM_OPS"))
		Dim iRows As IList(Of IWebElement) = iElement.FindElements(By.TagName("tr"))
		Dim inpuitXSUM39 = iRows(0).FindElements(By.TagName("input"))
		If costs.Where(Function(x) x.id = 17).ToList.Count > 0 Then
			For Each inputelement In inpuitXSUM39
				If inputelement.GetDomAttribute("id").Contains("unitsOverride") Then
					driver.ExecuteScript("document.getElementById('" & inputelement.GetDomAttribute("id").ToString & "').value=" & costs.Where(Function(x) x.id = 17).First.Unidades.ToString.Replace(",", "."))
					Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
				End If
			Next
		End If

		driver.ExecuteScript("jsDialogDirectXSUM_OPSSet();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeScript))
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmountOpen(event);")
		waitdriver.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("XSUM_OPS_startingAmount")))
		driver.ExecuteScript("document.getElementById('XSUM_OPS_startingAmount').value=" & costs.Where(Function(x) x.id = 17).First.VentaOPS)
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmount_click(1);")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
		driver.ExecuteScript("jsDialogXSUM_OPSActualSalesStartingAmountClose();")
		driver.ExecuteScript("jsDialogDirectXSUM_OPSClose();")
		Thread.Sleep(TimeSpan.FromSeconds(TimeShort))
	End Sub

	Private Sub btnSincronizarOneByOne_Click(sender As Object, e As EventArgs) Handles btnSincronizarOneByOne.Click
		Dim resultado = MessageBox.Show("Una vez inicie no podrá cancelar la operación. ¿Confirma que desea continuar?", "Confirmación", MessageBoxButtons.YesNo)
		If Not (resultado = DialogResult.Yes) Then
			Exit Sub
		End If
		If Not (IsNumeric(txtPropuesta.Text)) Then
			MessageBox.Show("La propuesta debe ser numérica")
			Exit Sub
		End If
		If Not (IsNumeric(txtAlternativa.Text)) Then
			MessageBox.Show("La alternativa debe ser numérica")
			Exit Sub
		End If
		If (IsNumeric(txtiQuote.Text)) Then
			MessageBox.Show("Por favor escriba el número de iQuote completo")
			Exit Sub
		End If
		Me.Visible = False
		ExportiQuoteGeneral(txtPropuesta.Text, txtAlternativa.Text, txtiQuote.Text)
		Me.Visible = True
		MessageBox.Show("Se ha completado la sincronización")
	End Sub

	Private Sub TestCodificacion_Click(sender As Object, e As EventArgs) Handles TestCodificacion.Click
		Dim oCot As New DAL.CotizadorGeneral
		Dim PRGeneral = oCot.GetGeneralByAlternativa(txtPropuesta.Text, txtAlternativa.Text)
		Dim PRlst = oCot.GetAllPresupuestosByAlternativa(txtPropuesta.Text, txtAlternativa.Text).Where(Function(x) x.MetCodigo = txtMet.Text And x.ParNacional = txtFase.Text)
		Dim CostosToIquote = ObtenerCostosToIquote(txtPropuesta.Text, txtAlternativa.Text, txtMet.Text, txtFase.Text)
		ExportiQuoteCodificacion(driver, PRlst.First, PRGeneral, CostosToIquote)
		MessageBox.Show("Prueba finalizada")
	End Sub

	Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
		driver.Navigate.GoToUrl("https://nwb.ipsos.com/")
	End Sub

	Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
		driver.Navigate.GoToUrl("https://www.ipsos.com.co/")
	End Sub
End Class
