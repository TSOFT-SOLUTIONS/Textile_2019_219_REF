<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Software_Options
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
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txt_AutoBakClient1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txt_AutoBakClient2 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_UPDATE = New System.Windows.Forms.Button()
        Me.txt_AutoBakServer = New System.Windows.Forms.TextBox()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Salmon
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(521, 35)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "OPTIONS"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.txt_AutoBakClient1)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.txt_AutoBakClient2)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.btn_UPDATE)
        Me.pnl_Back.Controls.Add(Me.txt_AutoBakServer)
        Me.pnl_Back.Location = New System.Drawing.Point(8, 47)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(498, 204)
        Me.pnl_Back.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(161, 15)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Auto BackUp Path - CLIENT 1"
        '
        'txt_AutoBakClient1
        '
        Me.txt_AutoBakClient1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_AutoBakClient1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_AutoBakClient1.Location = New System.Drawing.Point(185, 61)
        Me.txt_AutoBakClient1.MaxLength = 40
        Me.txt_AutoBakClient1.Name = "txt_AutoBakClient1"
        Me.txt_AutoBakClient1.Size = New System.Drawing.Size(282, 23)
        Me.txt_AutoBakClient1.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(161, 15)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Auto BackUp Path - CLIENT 2"
        '
        'txt_AutoBakClient2
        '
        Me.txt_AutoBakClient2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_AutoBakClient2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_AutoBakClient2.Location = New System.Drawing.Point(185, 104)
        Me.txt_AutoBakClient2.MaxLength = 40
        Me.txt_AutoBakClient2.Name = "txt_AutoBakClient2"
        Me.txt_AutoBakClient2.Size = New System.Drawing.Size(282, 23)
        Me.txt_AutoBakClient2.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(157, 15)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Auto BackUp Path -  SERVER"
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.Salmon
        Me.btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_Close.FlatAppearance.BorderColor = System.Drawing.Color.Blue
        Me.btn_Close.FlatAppearance.BorderSize = 2
        Me.btn_Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow
        Me.btn_Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(359, 153)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(108, 32)
        Me.btn_Close.TabIndex = 2
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_UPDATE
        '
        Me.btn_UPDATE.BackColor = System.Drawing.Color.Salmon
        Me.btn_UPDATE.FlatAppearance.BorderSize = 2
        Me.btn_UPDATE.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_UPDATE.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_UPDATE.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_UPDATE.ForeColor = System.Drawing.Color.White
        Me.btn_UPDATE.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_UPDATE.Location = New System.Drawing.Point(236, 153)
        Me.btn_UPDATE.Name = "btn_UPDATE"
        Me.btn_UPDATE.Size = New System.Drawing.Size(108, 32)
        Me.btn_UPDATE.TabIndex = 1
        Me.btn_UPDATE.TabStop = False
        Me.btn_UPDATE.Text = "&SAVE"
        Me.btn_UPDATE.UseVisualStyleBackColor = False
        '
        'txt_AutoBakServer
        '
        Me.txt_AutoBakServer.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_AutoBakServer.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_AutoBakServer.Location = New System.Drawing.Point(185, 18)
        Me.txt_AutoBakServer.MaxLength = 40
        Me.txt_AutoBakServer.Name = "txt_AutoBakServer"
        Me.txt_AutoBakServer.Size = New System.Drawing.Size(282, 23)
        Me.txt_AutoBakServer.TabIndex = 0
        '
        'Options
        '
        Me.AcceptButton = Me.btn_UPDATE
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Bisque
        Me.CancelButton = Me.btn_Close
        Me.ClientSize = New System.Drawing.Size(521, 271)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Options"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OPTIONS"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_UPDATE As System.Windows.Forms.Button
    Friend WithEvents txt_AutoBakServer As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txt_AutoBakClient1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txt_AutoBakClient2 As System.Windows.Forms.TextBox
End Class
