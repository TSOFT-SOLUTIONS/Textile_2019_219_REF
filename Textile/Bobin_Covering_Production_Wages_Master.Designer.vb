<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Bobin_Covering_Production_Wages_Master
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
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.txt_rate = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnl_back.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_back
        '
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.txt_rate)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_back.Location = New System.Drawing.Point(12, 54)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(285, 107)
        Me.pnl_back.TabIndex = 0
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.DarkRed
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(174, 64)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(87, 33)
        Me.btn_close.TabIndex = 2
        Me.btn_close.Text = "&Close"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.DarkRed
        Me.btn_save.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btn_save.Location = New System.Drawing.Point(71, 64)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(87, 33)
        Me.btn_save.TabIndex = 1
        Me.btn_save.Text = "&Update"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'txt_rate
        '
        Me.txt_rate.Location = New System.Drawing.Point(143, 22)
        Me.txt_rate.Name = "txt_rate"
        Me.txt_rate.Size = New System.Drawing.Size(118, 27)
        Me.txt_rate.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(18, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(119, 38)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Covering Wages / Spindle"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Black
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Yellow
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(309, 35)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "COVERING RATE"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Bobin_Covering_Production_Wages_Master
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ClientSize = New System.Drawing.Size(309, 174)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pnl_back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Bobin_Covering_Production_Wages_Master"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "COVERING PRODUCTION WAGES"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txt_rate As System.Windows.Forms.TextBox
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents btn_close As System.Windows.Forms.Button
End Class
