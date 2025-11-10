Imports System.Configuration
Imports Ipsos.Apis.BIScraper
Imports Ipsos.Apis.BIScraper.HTMLScrapping
Imports Ipsos.Apis.BIScraper.Reports.JobAndFinance
Imports Newtonsoft.Json
Imports Utilidades

Namespace ExternalServices.BIServices

    Public Class BIService
        Private ReadOnly _requestService As RequestService
        Sub New(requestService As RequestService)
            _requestService = requestService
        End Sub
        Public Function JobsBy(countryId As UShort, pageSize As Integer, pageIndex As Integer, status As String, searchValue As String) As IEnumerable(Of JobModel)
            Dim httpeClient = New Net.WebClient()
            Dim url As String = ""
            Dim builderURL = New BuilderURLPart(pageIndex, pageSize)
            Dim response As String

            If (Not String.IsNullOrWhiteSpace(status)) Then
                builderURL.AddStatus(status)
            End If

            If (Not String.IsNullOrWhiteSpace(searchValue)) Then
                builderURL.AddValueToSearch(searchValue)
            End If

            url = builderURL.Build()

            response = _requestService.RequestGet(url, "")

            Dim responseDeserialized = JsonConvert.DeserializeObject(Of IEnumerable(Of JobModel))(response)

            Return responseDeserialized

        End Function
        Public Function JobsBy(countryId As UShort, pageSize As Integer, pageIndex As Integer)
            Return JobsBy(countryId, pageSize, pageIndex, Nothing, Nothing)
        End Function
        Public Function JobsByStatus(countryId As UShort, pageSize As Integer, pageIndex As Integer, status As String)
            Return JobsBy(countryId, pageSize, pageIndex, status, Nothing)
        End Function
        Public Function JobsBySearchValue(countryId As UShort, pageSize As Integer, pageIndex As Integer, searchValue As String) As IEnumerable(Of JobModel)
            Return JobsBy(countryId, pageSize, pageIndex, Nothing, searchValue)
        End Function
        Public Function JobBookModerationInfo(jobBook As Long) As Result(Of CoreProject.ExternalServices.BIServices.JobBookModerationResult)
            Dim CredentialsManager_BI_IdUser_Report_JobAndFinance As Integer = ConfigurationManager.AppSettings("CredentialsManager_BI_IdUser_Report_JobAndFinance")
            Dim CredentialsManager_KeyEncript = ConfigurationManager.AppSettings("KeyCredentialManager")
            Dim CredentialsManagerRepository As New CredentialsManager.CredentialsManagerRepository()
            Dim BIUserCredentials = CredentialsManagerRepository.CredentialsBy(CredentialsManager_BI_IdUser_Report_JobAndFinance)
            Dim passwordDecripted = Encripcion.Cifrado(2, 5, BIUserCredentials.Password, CredentialsManager_KeyEncript, Nothing)
            Dim loginManager = New LoginManager()
            Dim BICredentials As New BIScraperCredentials() With {
                .UserName = BIUserCredentials.UserName,
                .Password = passwordDecripted
                }

            Dim BIScrapper_Report_ExternalJobFactPage As New ExternalJobFactPage(BICredentials, loginManager)
            Dim report = BIScrapper_Report_ExternalJobFactPage.JobBookModerationInfo(Currency.COP, jobBook)

            Return Map(report)

        End Function

        Private Class BuilderURLPart
            Private urlPart As String = ""
            Private status As String
            Private valueToSearch As String
            Private pageIndex As Integer
            Private pageSize As Integer

            Public Sub New(pageIndex As Integer, pageSize As Integer)
                Me.pageIndex = pageIndex
                Me.pageSize = pageSize
            End Sub

            Public Sub AddStatus(status As String)
                Me.status = status
            End Sub
            Public Sub AddValueToSearch(valueToSearch As String)
                Me.valueToSearch = valueToSearch
            End Sub
            Public Function Build() As String
                If (Not String.IsNullOrWhiteSpace(Me.status)) Then
                    Me.urlPart = Me.urlPart & "/Status/" & Me.status
                End If
                If (Not String.IsNullOrWhiteSpace(Me.valueToSearch)) Then
                    Me.urlPart = Me.urlPart & "/SearchValue/" & Me.valueToSearch
                End If
                Me.urlPart = Me.urlPart & "/PageSize/" & Me.pageSize & "/PageIndex/" & Me.pageIndex
                Return Me.urlPart
            End Function

        End Class

        Public Class JobModel
            Public Property CodJob As Long
            Public Property JobId As Long
            Public Property DescJob As String
            Public Property CodCliente As Long
            Public Property DescCliente As String
            Public Property ServiceLineId As Long
            Public Property ServiceLineName As String
        End Class

        Private Function Map(Result As HTMLScrapping.Result(Of Reports.JobAndFinance.JobBookModerationResult)) As CoreProject.ExternalServices.BIServices.Result(Of CoreProject.ExternalServices.BIServices.JobBookModerationResult)
            Return New CoreProject.ExternalServices.BIServices.Result(Of CoreProject.ExternalServices.BIServices.JobBookModerationResult) With {
                .IsSuccess = Result.IsSuccess,
                .ErrorResult = Map(Result.Error),
                .Data = Map(Result.Data)
                }
        End Function

        Private Function Map(Err As HTMLScrapping.Error) As ErrorResult
            Return New ErrorResult With {
                .Message = Err.Message,
                .StackTrace = Err.StackTrace
                }
        End Function
        Private Function Map(Data As Reports.JobAndFinance.JobBookModerationResult) As CoreProject.ExternalServices.BIServices.JobBookModerationResult
            If Data Is Nothing Then
                Return Nothing
            End If

            Return New CoreProject.ExternalServices.BIServices.JobBookModerationResult With {
                .JobBook = Data.JobBook,
                .StatusProject = Data.StatusProject,
                .Budget_3320_Moderation = Data.Budget_3320_Moderation,
                .ServicelineName = Data.ServiceLineName
                }
        End Function

    End Class
End Namespace
