<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Delete_Old_Company_Data
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
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.cbo_Company = New System.Windows.Forms.ComboBox()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_Delete_OldData = New System.Windows.Forms.Button()
        Me.cbo_FromYear = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.cbo_FromYear)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.cbo_Company)
        Me.pnl_Back.Controls.Add(Me.btn_close)
        Me.pnl_Back.Controls.Add(Me.btn_Delete_OldData)
        Me.pnl_Back.Location = New System.Drawing.Point(7, 8)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(569, 195)
        Me.pnl_Back.TabIndex = 225
        '
        'cbo_Company
        '
        Me.cbo_Company.DropDownHeight = 150
        Me.cbo_Company.FormattingEnabled = True
        Me.cbo_Company.IntegralHeight = False
        Me.cbo_Company.Location = New System.Drawing.Point(156, 32)
        Me.cbo_Company.Name = "cbo_Company"
        Me.cbo_Company.Size = New System.Drawing.Size(150, 23)
        Me.cbo_Company.TabIndex = 0
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.Red
        Me.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(421, 114)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(94, 35)
        Me.btn_close.TabIndex = 2
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_Delete_OldData
        '
        Me.btn_Delete_OldData.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Delete_OldData.ForeColor = System.Drawing.Color.White
        Me.btn_Delete_OldData.Location = New System.Drawing.Point(288, 114)
        Me.btn_Delete_OldData.Name = "btn_Delete_OldData"
        Me.btn_Delete_OldData.Size = New System.Drawing.Size(94, 35)
        Me.btn_Delete_OldData.TabIndex = 1
        Me.btn_Delete_OldData.TabStop = False
        Me.btn_Delete_OldData.Text = "&DELETE"
        Me.btn_Delete_OldData.UseVisualStyleBackColor = False
        '
        'cbo_FromYear
        '
        Me.cbo_FromYear.DropDownHeight = 150
        Me.cbo_FromYear.FormattingEnabled = True
        Me.cbo_FromYear.IntegralHeight = False
        Me.cbo_FromYear.Location = New System.Drawing.Point(444, 36)
        Me.cbo_FromYear.Name = "cbo_FromYear"
        Me.cbo_FromYear.Size = New System.Drawing.Size(109, 23)
        Me.cbo_FromYear.TabIndex = 227
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(323, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(105, 30)
        Me.Label1.TabIndex = 228
        Me.Label1.Text = "Delete Old  Year " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(to maintain data)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(30, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(105, 30)
        Me.Label2.TabIndex = 229
        Me.Label2.Text = "Delete Company " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(to maintain data)"
        '
        'Delete_Old_Company_Data
        '
        Me.AcceptButton = Me.btn_Delete_OldData
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.CancelButton = Me.btn_close
        Me.ClientSize = New System.Drawing.Size(589, 211)
        Me.Controls.Add(Me.pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Delete_Old_Company_Data"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DELETE OLD DATA"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_Delete_OldData As System.Windows.Forms.Button
    Friend WithEvents cbo_Company As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_FromYear As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
End Class
