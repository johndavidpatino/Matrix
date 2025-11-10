<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		components = New ComponentModel.Container()
		Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
		Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
		Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
		btnGenerar = New Button()
		dgvAlternativas = New DataGridView()
		txtUsuario = New TextBox()
		Label1 = New Label()
		btnConsultar = New Button()
		BindingSource1 = New BindingSource(components)
		txtPassword = New TextBox()
		Label2 = New Label()
		txtPropuesta = New TextBox()
		txtAlternativa = New TextBox()
		txtiQuote = New TextBox()
		btnSincronizarOneByOne = New Button()
		Label3 = New Label()
		Label4 = New Label()
		Label5 = New Label()
		PictureBox1 = New PictureBox()
		TestCodificacion = New Button()
		txtMet = New TextBox()
		txtFase = New TextBox()
		Button1 = New Button()
		Button2 = New Button()
		CType(dgvAlternativas, ComponentModel.ISupportInitialize).BeginInit()
		CType(BindingSource1, ComponentModel.ISupportInitialize).BeginInit()
		CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
		SuspendLayout()
		' 
		' btnGenerar
		' 
		btnGenerar.Font = New Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point)
		btnGenerar.Location = New Point(303, 51)
		btnGenerar.Margin = New Padding(3, 2, 3, 2)
		btnGenerar.Name = "btnGenerar"
		btnGenerar.Size = New Size(108, 22)
		btnGenerar.TabIndex = 3
		btnGenerar.Text = "Enviar a iQuote"
		btnGenerar.UseVisualStyleBackColor = True
		btnGenerar.Visible = False
		' 
		' dgvAlternativas
		' 
		dgvAlternativas.AllowUserToAddRows = False
		dgvAlternativas.AllowUserToDeleteRows = False
		dgvAlternativas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
		dgvAlternativas.BackgroundColor = SystemColors.ControlLight
		DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle1.BackColor = SystemColors.Control
		DataGridViewCellStyle1.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
		DataGridViewCellStyle1.ForeColor = SystemColors.WindowText
		DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
		DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
		DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
		dgvAlternativas.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
		dgvAlternativas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
		DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle2.BackColor = SystemColors.Window
		DataGridViewCellStyle2.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
		DataGridViewCellStyle2.ForeColor = SystemColors.ControlText
		DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
		DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
		DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
		dgvAlternativas.DefaultCellStyle = DataGridViewCellStyle2
		dgvAlternativas.Location = New Point(10, 33)
		dgvAlternativas.Margin = New Padding(3, 2, 3, 2)
		dgvAlternativas.Name = "dgvAlternativas"
		dgvAlternativas.ReadOnly = True
		DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle3.BackColor = SystemColors.Control
		DataGridViewCellStyle3.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
		DataGridViewCellStyle3.ForeColor = SystemColors.WindowText
		DataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight
		DataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText
		DataGridViewCellStyle3.WrapMode = DataGridViewTriState.True
		dgvAlternativas.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
		dgvAlternativas.RowHeadersWidth = 51
		dgvAlternativas.Size = New Size(679, 14)
		dgvAlternativas.TabIndex = 1
		dgvAlternativas.Visible = False
		' 
		' txtUsuario
		' 
		txtUsuario.Location = New Point(119, 7)
		txtUsuario.Margin = New Padding(3, 2, 3, 2)
		txtUsuario.Name = "txtUsuario"
		txtUsuario.Size = New Size(156, 23)
		txtUsuario.TabIndex = 0
		txtUsuario.Visible = False
		' 
		' Label1
		' 
		Label1.AutoSize = True
		Label1.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
		Label1.ForeColor = Color.White
		Label1.Location = New Point(32, 138)
		Label1.Name = "Label1"
		Label1.Size = New Size(93, 21)
		Label1.TabIndex = 3
		Label1.Text = "# Propuesta"
		' 
		' btnConsultar
		' 
		btnConsultar.Location = New Point(582, 6)
		btnConsultar.Margin = New Padding(3, 2, 3, 2)
		btnConsultar.Name = "btnConsultar"
		btnConsultar.Size = New Size(82, 22)
		btnConsultar.TabIndex = 2
		btnConsultar.Text = "Consultar"
		btnConsultar.UseVisualStyleBackColor = True
		btnConsultar.Visible = False
		' 
		' txtPassword
		' 
		txtPassword.Location = New Point(406, 7)
		txtPassword.Margin = New Padding(3, 2, 3, 2)
		txtPassword.Name = "txtPassword"
		txtPassword.PasswordChar = "*"c
		txtPassword.Size = New Size(156, 23)
		txtPassword.TabIndex = 1
		txtPassword.UseSystemPasswordChar = True
		txtPassword.Visible = False
		' 
		' Label2
		' 
		Label2.AutoSize = True
		Label2.Location = New Point(303, 9)
		Label2.Name = "Label2"
		Label2.Size = New Size(94, 15)
		Label2.TabIndex = 6
		Label2.Text = "Password Matrix"
		Label2.Visible = False
		' 
		' txtPropuesta
		' 
		txtPropuesta.Font = New Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point)
		txtPropuesta.Location = New Point(32, 162)
		txtPropuesta.Name = "txtPropuesta"
		txtPropuesta.Size = New Size(100, 32)
		txtPropuesta.TabIndex = 1
		' 
		' txtAlternativa
		' 
		txtAlternativa.Font = New Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point)
		txtAlternativa.Location = New Point(184, 162)
		txtAlternativa.Name = "txtAlternativa"
		txtAlternativa.Size = New Size(100, 32)
		txtAlternativa.TabIndex = 2
		' 
		' txtiQuote
		' 
		txtiQuote.Font = New Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point)
		txtiQuote.Location = New Point(318, 163)
		txtiQuote.Name = "txtiQuote"
		txtiQuote.Size = New Size(165, 32)
		txtiQuote.TabIndex = 3
		' 
		' btnSincronizarOneByOne
		' 
		btnSincronizarOneByOne.BackColor = Color.AliceBlue
		btnSincronizarOneByOne.Font = New Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point)
		btnSincronizarOneByOne.ForeColor = Color.Navy
		btnSincronizarOneByOne.Location = New Point(509, 162)
		btnSincronizarOneByOne.Margin = New Padding(3, 2, 3, 2)
		btnSincronizarOneByOne.Name = "btnSincronizarOneByOne"
		btnSincronizarOneByOne.Size = New Size(155, 33)
		btnSincronizarOneByOne.TabIndex = 4
		btnSincronizarOneByOne.Text = "Sincronizar"
		btnSincronizarOneByOne.UseVisualStyleBackColor = False
		' 
		' Label3
		' 
		Label3.AutoSize = True
		Label3.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
		Label3.ForeColor = Color.White
		Label3.Location = New Point(184, 138)
		Label3.Name = "Label3"
		Label3.Size = New Size(98, 21)
		Label3.TabIndex = 3
		Label3.Text = "# Alternativa"
		' 
		' Label4
		' 
		Label4.AutoSize = True
		Label4.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
		Label4.ForeColor = Color.White
		Label4.Location = New Point(362, 138)
		Label4.Name = "Label4"
		Label4.Size = New Size(70, 21)
		Label4.TabIndex = 3
		Label4.Text = "# IQuote"
		' 
		' Label5
		' 
		Label5.AutoSize = True
		Label5.Font = New Font("Segoe UI", 18F, FontStyle.Italic, GraphicsUnit.Point)
		Label5.ForeColor = Color.White
		Label5.Location = New Point(78, 84)
		Label5.Name = "Label5"
		Label5.Size = New Size(553, 32)
		Label5.TabIndex = 8
		Label5.Text = "Simple, Speed, secure, substantial... Synchronization!"
		' 
		' PictureBox1
		' 
		PictureBox1.Image = My.Resources.Resources.Ipsos_logo_with_transparent_background
		PictureBox1.Location = New Point(600, -1)
		PictureBox1.Name = "PictureBox1"
		PictureBox1.Size = New Size(100, 82)
		PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
		PictureBox1.TabIndex = 9
		PictureBox1.TabStop = False
		' 
		' TestCodificacion
		' 
		TestCodificacion.Location = New Point(440, 222)
		TestCodificacion.Name = "TestCodificacion"
		TestCodificacion.Size = New Size(182, 22)
		TestCodificacion.TabIndex = 10
		TestCodificacion.Text = "Test Codificacion"
		TestCodificacion.UseVisualStyleBackColor = True
		' 
		' txtMet
		' 
		txtMet.Location = New Point(314, 221)
		txtMet.Name = "txtMet"
		txtMet.Size = New Size(56, 23)
		txtMet.TabIndex = 11
		' 
		' txtFase
		' 
		txtFase.Location = New Point(376, 221)
		txtFase.Name = "txtFase"
		txtFase.Size = New Size(56, 23)
		txtFase.TabIndex = 12
		' 
		' Button1
		' 
		Button1.Location = New Point(40, 227)
		Button1.Name = "Button1"
		Button1.Size = New Size(75, 23)
		Button1.TabIndex = 13
		Button1.Text = "Button1"
		Button1.UseVisualStyleBackColor = True
		' 
		' Button2
		' 
		Button2.Location = New Point(140, 230)
		Button2.Name = "Button2"
		Button2.Size = New Size(75, 23)
		Button2.TabIndex = 14
		Button2.Text = "Button2"
		Button2.UseVisualStyleBackColor = True
		' 
		' Form1
		' 
		AcceptButton = btnConsultar
		AutoScaleDimensions = New SizeF(7F, 15F)
		AutoScaleMode = AutoScaleMode.Font
		BackColor = Color.MidnightBlue
		ClientSize = New Size(700, 256)
		Controls.Add(Button2)
		Controls.Add(Button1)
		Controls.Add(txtFase)
		Controls.Add(txtMet)
		Controls.Add(TestCodificacion)
		Controls.Add(PictureBox1)
		Controls.Add(Label5)
		Controls.Add(txtiQuote)
		Controls.Add(txtAlternativa)
		Controls.Add(txtPropuesta)
		Controls.Add(Label2)
		Controls.Add(txtPassword)
		Controls.Add(btnConsultar)
		Controls.Add(Label4)
		Controls.Add(Label3)
		Controls.Add(Label1)
		Controls.Add(txtUsuario)
		Controls.Add(dgvAlternativas)
		Controls.Add(btnSincronizarOneByOne)
		Controls.Add(btnGenerar)
		FormBorderStyle = FormBorderStyle.FixedToolWindow
		Margin = New Padding(3, 2, 3, 2)
		Name = "Form1"
		StartPosition = FormStartPosition.CenterScreen
		Text = "Sincronización Matrix - iQuote"
		TopMost = True
		CType(dgvAlternativas, ComponentModel.ISupportInitialize).EndInit()
		CType(BindingSource1, ComponentModel.ISupportInitialize).EndInit()
		CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
		ResumeLayout(False)
		PerformLayout()
	End Sub

	Friend WithEvents btnGenerar As Button
	Friend WithEvents dgvAlternativas As DataGridView
	Friend WithEvents txtUsuario As TextBox
	Friend WithEvents Label1 As Label
	Friend WithEvents btnConsultar As Button
	Friend WithEvents BindingSource1 As BindingSource
	Friend WithEvents txtPassword As TextBox
	Friend WithEvents Label2 As Label
	Friend WithEvents txtPropuesta As TextBox
	Friend WithEvents txtAlternativa As TextBox
	Friend WithEvents txtiQuote As TextBox
	Friend WithEvents btnSincronizarOneByOne As Button
	Friend WithEvents Label3 As Label
	Friend WithEvents Label4 As Label
	Friend WithEvents Label5 As Label
	Friend WithEvents PictureBox1 As PictureBox
	Friend WithEvents TestCodificacion As Button
	Friend WithEvents txtMet As TextBox
	Friend WithEvents txtFase As TextBox
	Friend WithEvents Button1 As Button
	Friend WithEvents Button2 As Button
End Class
