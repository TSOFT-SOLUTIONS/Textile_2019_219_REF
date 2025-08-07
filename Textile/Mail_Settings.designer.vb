<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Mail_Settings
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grp_Mail_Setting = New System.Windows.Forms.GroupBox()
        Me.cbo_Mail_Host = New System.Windows.Forms.ComboBox()
        Me.txt_Mail_Port = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_Mail_Pwd = New System.Windows.Forms.TextBox()
        Me.txt_MailID = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btn_Mail_Close = New System.Windows.Forms.Button()
        Me.btn_Mail_Save = New System.Windows.Forms.Button()
        Me.grp_Mail_Setting.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(656, 35)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "E-MAIL SETTINGS"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grp_Mail_Setting
        '
        Me.grp_Mail_Setting.Controls.Add(Me.cbo_Mail_Host)
        Me.grp_Mail_Setting.Controls.Add(Me.txt_Mail_Port)
        Me.grp_Mail_Setting.Controls.Add(Me.Label7)
        Me.grp_Mail_Setting.Controls.Add(Me.Label8)
        Me.grp_Mail_Setting.Controls.Add(Me.txt_Mail_Pwd)
        Me.grp_Mail_Setting.Controls.Add(Me.txt_MailID)
        Me.grp_Mail_Setting.Controls.Add(Me.Label3)
        Me.grp_Mail_Setting.Controls.Add(Me.Label2)
        Me.grp_Mail_Setting.ForeColor = System.Drawing.Color.Black
        Me.grp_Mail_Setting.Location = New System.Drawing.Point(8, 42)
        Me.grp_Mail_Setting.Name = "grp_Mail_Setting"
        Me.grp_Mail_Setting.Size = New System.Drawing.Size(636, 208)
        Me.grp_Mail_Setting.TabIndex = 0
        Me.grp_Mail_Setting.TabStop = False
        Me.grp_Mail_Setting.Text = "MAIL Settings"
        '
        'cbo_Mail_Host
        '
        Me.cbo_Mail_Host.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Mail_Host.FormattingEnabled = True
        Me.cbo_Mail_Host.Location = New System.Drawing.Point(137, 118)
        Me.cbo_Mail_Host.Name = "cbo_Mail_Host"
        Me.cbo_Mail_Host.Size = New System.Drawing.Size(466, 23)
        Me.cbo_Mail_Host.TabIndex = 2
        '
        'txt_Mail_Port
        '
        Me.txt_Mail_Port.Location = New System.Drawing.Point(137, 158)
        Me.txt_Mail_Port.MaxLength = 30
        Me.txt_Mail_Port.Name = "txt_Mail_Port"
        Me.txt_Mail_Port.Size = New System.Drawing.Size(466, 23)
        Me.txt_Mail_Port.TabIndex = 3
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Label7.Location = New System.Drawing.Point(29, 162)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 15)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "E-Mail Port"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Label8.Location = New System.Drawing.Point(29, 122)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 15)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "E-Mail Host"
        '
        'txt_Mail_Pwd
        '
        Me.txt_Mail_Pwd.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Mail_Pwd.Location = New System.Drawing.Point(137, 74)
        Me.txt_Mail_Pwd.Name = "txt_Mail_Pwd"
        Me.txt_Mail_Pwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txt_Mail_Pwd.Size = New System.Drawing.Size(466, 27)
        Me.txt_Mail_Pwd.TabIndex = 1
        '
        'txt_MailID
        '
        Me.txt_MailID.Location = New System.Drawing.Point(137, 34)
        Me.txt_MailID.MaxLength = 50
        Me.txt_MailID.Name = "txt_MailID"
        Me.txt_MailID.Size = New System.Drawing.Size(466, 23)
        Me.txt_MailID.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Label3.Location = New System.Drawing.Point(29, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 15)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Password"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Label2.Location = New System.Drawing.Point(29, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "E-Mail ID"
        '
        'btn_Mail_Close
        '
        Me.btn_Mail_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Mail_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Mail_Close.Location = New System.Drawing.Point(526, 266)
        Me.btn_Mail_Close.Name = "btn_Mail_Close"
        Me.btn_Mail_Close.Size = New System.Drawing.Size(85, 35)
        Me.btn_Mail_Close.TabIndex = 5
        Me.btn_Mail_Close.TabStop = False
        Me.btn_Mail_Close.Text = "&CLOSE"
        Me.btn_Mail_Close.UseVisualStyleBackColor = False
        '
        'btn_Mail_Save
        '
        Me.btn_Mail_Save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Mail_Save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btn_Mail_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Mail_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Mail_Save.Location = New System.Drawing.Point(419, 266)
        Me.btn_Mail_Save.Name = "btn_Mail_Save"
        Me.btn_Mail_Save.Size = New System.Drawing.Size(85, 35)
        Me.btn_Mail_Save.TabIndex = 4
        Me.btn_Mail_Save.TabStop = False
        Me.btn_Mail_Save.Text = "&SAVE"
        Me.btn_Mail_Save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btn_Mail_Save.UseVisualStyleBackColor = False
        '
        'Mail_Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ClientSize = New System.Drawing.Size(656, 315)
        Me.Controls.Add(Me.grp_Mail_Setting)
        Me.Controls.Add(Me.btn_Mail_Close)
        Me.Controls.Add(Me.btn_Mail_Save)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Mail_Settings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "E-MAIL SETTINGS"
        Me.grp_Mail_Setting.ResumeLayout(False)
        Me.grp_Mail_Setting.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grp_Mail_Setting As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txt_MailID As System.Windows.Forms.TextBox
    Friend WithEvents txt_Mail_Pwd As System.Windows.Forms.TextBox
    Friend WithEvents btn_Mail_Close As System.Windows.Forms.Button
    Friend WithEvents btn_Mail_Save As System.Windows.Forms.Button
    Friend WithEvents txt_Mail_Port As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cbo_Mail_Host As System.Windows.Forms.ComboBox
End Class
