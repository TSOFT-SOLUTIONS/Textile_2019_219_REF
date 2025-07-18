<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Script_Generation
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
        Me.Label120 = New System.Windows.Forms.Label()
        Me.cbo_PartyName = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btn_SaveAll = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.RichTextBox1)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Button3)
        Me.Panel1.Controls.Add(Me.Button4)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.btn_SaveAll)
        Me.Panel1.Controls.Add(Me.ComboBox1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label120)
        Me.Panel1.Controls.Add(Me.cbo_PartyName)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(730, 465)
        Me.Panel1.TabIndex = 0
        '
        'Label120
        '
        Me.Label120.AutoSize = True
        Me.Label120.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label120.ForeColor = System.Drawing.Color.Red
        Me.Label120.Location = New System.Drawing.Point(119, 34)
        Me.Label120.Name = "Label120"
        Me.Label120.Size = New System.Drawing.Size(13, 15)
        Me.Label120.TabIndex = 1161
        Me.Label120.Text = "*"
        Me.Label120.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbo_PartyName
        '
        Me.cbo_PartyName.DropDownHeight = 350
        Me.cbo_PartyName.DropDownWidth = 350
        Me.cbo_PartyName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_PartyName.FormattingEnabled = True
        Me.cbo_PartyName.IntegralHeight = False
        Me.cbo_PartyName.Location = New System.Drawing.Point(151, 30)
        Me.cbo_PartyName.MaxDropDownItems = 15
        Me.cbo_PartyName.MaxLength = 50
        Me.cbo_PartyName.Name = "cbo_PartyName"
        Me.cbo_PartyName.Size = New System.Drawing.Size(324, 23)
        Me.cbo_PartyName.Sorted = True
        Me.cbo_PartyName.TabIndex = 1159
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(25, 34)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(93, 15)
        Me.Label5.TabIndex = 1160
        Me.Label5.Text = "DataBase Name"
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownHeight = 350
        Me.ComboBox1.DropDownWidth = 350
        Me.ComboBox1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.IntegralHeight = False
        Me.ComboBox1.Location = New System.Drawing.Point(150, 83)
        Me.ComboBox1.MaxDropDownItems = 15
        Me.ComboBox1.MaxLength = 50
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(324, 23)
        Me.ComboBox1.Sorted = True
        Me.ComboBox1.TabIndex = 1162
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(24, 87)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 15)
        Me.Label2.TabIndex = 1163
        Me.Label2.Text = "Table Name"
        '
        'btn_SaveAll
        '
        Me.btn_SaveAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.btn_SaveAll.ForeColor = System.Drawing.Color.White
        Me.btn_SaveAll.Location = New System.Drawing.Point(519, 28)
        Me.btn_SaveAll.Name = "btn_SaveAll"
        Me.btn_SaveAll.Size = New System.Drawing.Size(152, 29)
        Me.btn_SaveAll.TabIndex = 1167
        Me.btn_SaveAll.TabStop = False
        Me.btn_SaveAll.Text = "CREATE &DATABASE SCRIPT"
        Me.btn_SaveAll.UseVisualStyleBackColor = False
        Me.btn_SaveAll.Visible = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(519, 83)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(152, 29)
        Me.Button1.TabIndex = 1168
        Me.Button1.TabStop = False
        Me.Button1.Text = "CREATE &TABLE SCRIPT"
        Me.Button1.UseVisualStyleBackColor = False
        Me.Button1.Visible = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(565, 424)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(73, 29)
        Me.Button2.TabIndex = 1170
        Me.Button2.TabStop = False
        Me.Button2.Text = "C&LEAR"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Button3.ForeColor = System.Drawing.Color.White
        Me.Button3.Location = New System.Drawing.Point(645, 424)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(73, 29)
        Me.Button3.TabIndex = 1171
        Me.Button3.TabStop = False
        Me.Button3.Text = "&CLOSE"
        Me.Button3.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Button4.ForeColor = System.Drawing.Color.White
        Me.Button4.Location = New System.Drawing.Point(486, 424)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(73, 29)
        Me.Button4.TabIndex = 1169
        Me.Button4.TabStop = False
        Me.Button4.Text = "C&OPY"
        Me.Button4.UseVisualStyleBackColor = False
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Location = New System.Drawing.Point(28, 118)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(679, 300)
        Me.RichTextBox1.TabIndex = 1172
        Me.RichTextBox1.Text = ""
        '
        'Script_Generation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(730, 465)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Script_Generation"
        Me.Text = "SCRIPT GENERATION"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label120 As System.Windows.Forms.Label
    Friend WithEvents cbo_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btn_SaveAll As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
End Class
