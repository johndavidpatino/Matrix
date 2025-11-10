Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Drawing
Imports Dapper
Public Class BriefDapper
	Private _db As IDbConnection
	Public Sub BriefDapper()
		_db = New SqlConnection(ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString)
	End Sub
	'Public Sub Add(brief As CU_BriefDTO)
	'Dim queryParameters = New DynamicParameters
	'	queryParameters.Add("@Cedula", Cedula)
	'	queryParameters.Add("@Valor", Valor)
	'
	'_db.Execute("INSERT INTO ConnectToBI.CU_Brief '(Cliente,Contcto,TipoBrief,Titulo,GerenteCuentas,Viabilidad,Unidad,MarcaViabilidad,FechaViabilidad,) VALUES(@CEDULA,@VALOR,@FECHA)", 'New Object With { Cedula = Cedula})
	'End Sub

	Public Class CU_BriefDTO
		Private _Cliente As String
		Public Property Cliente() As String
			Get
				Return _Cliente
			End Get
			Set(ByVal value As String)
				_Cliente = value
			End Set
		End Property

		Private _Contacto As String
		Public Property Contacto() As String
			Get
				Return _Contacto
			End Get
			Set(ByVal value As String)
				_Contacto = value
			End Set
		End Property
		Private _Titulo As String
		Public Property Titulo() As String
			Get
				Return _Titulo
			End Get
			Set(ByVal value As String)
				_Titulo = value
			End Set
		End Property
		Private _GerenteCuentas As Long
		Public Property GerenteCuentas() As Long
			Get
				Return _GerenteCuentas
			End Get
			Set(ByVal value As Long)
				_GerenteCuentas = value
			End Set
		End Property

		Private _Viabilidad As Boolean
		Public Property Viabilidad() As Boolean
			Get
				Return _Viabilidad
			End Get
			Set(ByVal value As Boolean)
				_Viabilidad = value
			End Set
		End Property
		Private _Unidad As Integer
		Public Property Unidad() As Integer
			Get
				Return _Unidad
			End Get
			Set(ByVal value As Integer)
				_Unidad = value
			End Set
		End Property
		Private _JobBookFromBI As String
		Public Property JobBookFromBI() As String
			Get
				Return _JobBookFromBI
			End Get
			Set(ByVal value As String)
				_JobBookFromBI = value
			End Set
		End Property
		Private _DateSyncFromBI As Date?
		Public Property DateSyncFromBI() As Date?
			Get
				Return _DateSyncFromBI
			End Get
			Set(ByVal value As Date?)
				_DateSyncFromBI = value
			End Set
		End Property
		Private _FechaViabilidad As Date
		Public Property FechaViabilidad() As Date
			Get
				Return _FechaViabilidad
			End Get
			Set(ByVal value As Date)
				_FechaViabilidad = value
			End Set
		End Property
		Private _MarcaViabilidad As Long
		Public Property MarcaViabilidad() As Long
			Get
				Return _MarcaViabilidad
			End Get
			Set(ByVal value As Long)
				_MarcaViabilidad = value
			End Set
		End Property
		Private _Antecedentes As String
		Public Property Antecedentes() As String
			Get
				Return _Antecedentes
			End Get
			Set(ByVal value As String)
				_Antecedentes = value
			End Set
		End Property

	End Class
End Class
