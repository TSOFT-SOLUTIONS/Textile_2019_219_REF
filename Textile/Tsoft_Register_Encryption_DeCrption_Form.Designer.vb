<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Tsoft_Register_Encryption_DeCrption_Form
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lbl_SystemNo = New System.Windows.Forms.Label()
        Me.btn_Show_LicenseCode = New System.Windows.Forms.Button()
        Me.btn_Register = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txt_LicenseCode = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.btn_Login = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txt_SqlPassword = New System.Windows.Forms.TextBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txt_UserPwd_EncryptionCode = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txt_UserPassword = New System.Windows.Forms.TextBox()
        Me.Panel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btn_Close)
        Me.Panel1.Controls.Add(Me.TabControl1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(791, 474)
        Me.Panel1.TabIndex = 0
        '
        'btn_Close
        '
        Me.btn_Close.FlatAppearance.BorderSize = 2
        Me.btn_Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.Red
        Me.btn_Close.Location = New System.Drawing.Point(688, 418)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(91, 38)
        Me.btn_Close.TabIndex = 3
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(767, 387)
        Me.TabControl1.TabIndex = 20
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Panel2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(759, 359)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "REGISTER"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lbl_SystemNo)
        Me.Panel2.Controls.Add(Me.btn_Show_LicenseCode)
        Me.Panel2.Controls.Add(Me.btn_Register)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.txt_LicenseCode)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Location = New System.Drawing.Point(28, 35)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(645, 210)
        Me.Panel2.TabIndex = 19
        '
        'lbl_SystemNo
        '
        Me.lbl_SystemNo.BackColor = System.Drawing.Color.White
        Me.lbl_SystemNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_SystemNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_SystemNo.Location = New System.Drawing.Point(107, 21)
        Me.lbl_SystemNo.Name = "lbl_SystemNo"
        Me.lbl_SystemNo.Size = New System.Drawing.Size(520, 23)
        Me.lbl_SystemNo.TabIndex = 29
        Me.lbl_SystemNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btn_Show_LicenseCode
        '
        Me.btn_Show_LicenseCode.FlatAppearance.BorderSize = 2
        Me.btn_Show_LicenseCode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_Show_LicenseCode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Show_LicenseCode.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Show_LicenseCode.ForeColor = System.Drawing.Color.Navy
        Me.btn_Show_LicenseCode.Location = New System.Drawing.Point(356, 111)
        Me.btn_Show_LicenseCode.Name = "btn_Show_LicenseCode"
        Me.btn_Show_LicenseCode.Size = New System.Drawing.Size(138, 38)
        Me.btn_Show_LicenseCode.TabIndex = 2
        Me.btn_Show_LicenseCode.TabStop = False
        Me.btn_Show_LicenseCode.Text = "&SHOW LICENSE CODE"
        Me.btn_Show_LicenseCode.UseVisualStyleBackColor = False
        '
        'btn_Register
        '
        Me.btn_Register.FlatAppearance.BorderSize = 2
        Me.btn_Register.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_Register.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Register.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Register.ForeColor = System.Drawing.Color.Navy
        Me.btn_Register.Location = New System.Drawing.Point(180, 111)
        Me.btn_Register.Name = "btn_Register"
        Me.btn_Register.Size = New System.Drawing.Size(87, 38)
        Me.btn_Register.TabIndex = 1
        Me.btn_Register.TabStop = False
        Me.btn_Register.Text = "&REGISTER"
        Me.btn_Register.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(13, 70)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 15)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "License Code"
        '
        'txt_LicenseCode
        '
        Me.txt_LicenseCode.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_LicenseCode.Location = New System.Drawing.Point(107, 66)
        Me.txt_LicenseCode.MaxLength = 40
        Me.txt_LicenseCode.Name = "txt_LicenseCode"
        Me.txt_LicenseCode.Size = New System.Drawing.Size(520, 23)
        Me.txt_LicenseCode.TabIndex = 0
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(13, 25)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 15)
        Me.Label6.TabIndex = 24
        Me.Label6.Text = "System No"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Panel4)
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(759, 359)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "SQL CODE"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.btn_Login)
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Controls.Add(Me.TextBox1)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Controls.Add(Me.txt_SqlPassword)
        Me.Panel4.Location = New System.Drawing.Point(50, 51)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(540, 183)
        Me.Panel4.TabIndex = 21
        '
        'btn_Login
        '
        Me.btn_Login.FlatAppearance.BorderSize = 2
        Me.btn_Login.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_Login.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Login.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Login.ForeColor = System.Drawing.Color.Navy
        Me.btn_Login.Location = New System.Drawing.Point(208, 109)
        Me.btn_Login.Name = "btn_Login"
        Me.btn_Login.Size = New System.Drawing.Size(87, 38)
        Me.btn_Login.TabIndex = 22
        Me.btn_Login.TabStop = False
        Me.btn_Login.Text = "&ENCRYPT"
        Me.btn_Login.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(39, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 13)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Code"
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(100, 80)
        Me.TextBox1.MaxLength = 40
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(381, 23)
        Me.TextBox1.TabIndex = 20
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(39, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(117, 13)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "Enter Sql Password"
        '
        'txt_SqlPassword
        '
        Me.txt_SqlPassword.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_SqlPassword.Location = New System.Drawing.Point(182, 47)
        Me.txt_SqlPassword.MaxLength = 40
        Me.txt_SqlPassword.Name = "txt_SqlPassword"
        Me.txt_SqlPassword.Size = New System.Drawing.Size(299, 23)
        Me.txt_SqlPassword.TabIndex = 18
        '
        'TabPage3
        '
        Me.TabPage3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TabPage3.Controls.Add(Me.Panel3)
        Me.TabPage3.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(759, 359)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "USER CODE"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Button1)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.txt_UserPwd_EncryptionCode)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.txt_UserPassword)
        Me.Panel3.Location = New System.Drawing.Point(59, 72)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(522, 140)
        Me.Panel3.TabIndex = 20
        '
        'Button1
        '
        Me.Button1.FlatAppearance.BorderSize = 2
        Me.Button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.Button1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.Navy
        Me.Button1.Location = New System.Drawing.Point(209, 82)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(87, 38)
        Me.Button1.TabIndex = 27
        Me.Button1.TabStop = False
        Me.Button1.Text = "&ENCRYPT"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(40, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(36, 13)
        Me.Label3.TabIndex = 26
        Me.Label3.Text = "Code"
        '
        'txt_UserPwd_EncryptionCode
        '
        Me.txt_UserPwd_EncryptionCode.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_UserPwd_EncryptionCode.Location = New System.Drawing.Point(101, 53)
        Me.txt_UserPwd_EncryptionCode.MaxLength = 40
        Me.txt_UserPwd_EncryptionCode.Name = "txt_UserPwd_EncryptionCode"
        Me.txt_UserPwd_EncryptionCode.Size = New System.Drawing.Size(381, 23)
        Me.txt_UserPwd_EncryptionCode.TabIndex = 25
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Navy
        Me.Label4.Location = New System.Drawing.Point(40, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(125, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "Enter User Password"
        '
        'txt_UserPassword
        '
        Me.txt_UserPassword.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_UserPassword.Location = New System.Drawing.Point(209, 20)
        Me.txt_UserPassword.MaxLength = 40
        Me.txt_UserPassword.Name = "txt_UserPassword"
        Me.txt_UserPassword.Size = New System.Drawing.Size(273, 23)
        Me.txt_UserPassword.TabIndex = 23
        '
        'Tsoft_Register_Encryption_DeCrption_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(791, 474)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "Tsoft_Register_Encryption_DeCrption_Form"
        Me.Text = "TSOFT REGISTER"
        Me.Panel1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btn_Show_LicenseCode As System.Windows.Forms.Button
    Friend WithEvents btn_Register As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txt_LicenseCode As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents btn_Login As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txt_SqlPassword As System.Windows.Forms.TextBox
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txt_UserPwd_EncryptionCode As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txt_UserPassword As System.Windows.Forms.TextBox
    Friend WithEvents lbl_SystemNo As System.Windows.Forms.Label
End Class
