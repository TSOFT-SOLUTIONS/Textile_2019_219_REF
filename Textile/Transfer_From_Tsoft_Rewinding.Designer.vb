<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Transfer_From_Tsoft_Rewinding
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
        Me.cbo_DBFrom = New System.Windows.Forms.ComboBox()
        Me.btn_Transfer = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(21, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Database From"
        '
        'cbo_DBFrom
        '
        Me.cbo_DBFrom.FormattingEnabled = True
        Me.cbo_DBFrom.Location = New System.Drawing.Point(124, 34)
        Me.cbo_DBFrom.Name = "cbo_DBFrom"
        Me.cbo_DBFrom.Size = New System.Drawing.Size(397, 23)
        Me.cbo_DBFrom.TabIndex = 0
        '
        'btn_Transfer
        '
        Me.btn_Transfer.Location = New System.Drawing.Point(204, 103)
        Me.btn_Transfer.Name = "btn_Transfer"
        Me.btn_Transfer.Size = New System.Drawing.Size(149, 66)
        Me.btn_Transfer.TabIndex = 1
        Me.btn_Transfer.Text = "&TRANSFER"
        Me.btn_Transfer.UseVisualStyleBackColor = True
        '
        'Transfer_Master_Ledgers_From_Smart_TextileNT10
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(554, 213)
        Me.Controls.Add(Me.btn_Transfer)
        Me.Controls.Add(Me.cbo_DBFrom)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "Transfer_Master_Ledgers_From_Smart_TextileNT10"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TRANSFER MASTERS"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbo_DBFrom As System.Windows.Forms.ComboBox
    Friend WithEvents btn_Transfer As System.Windows.Forms.Button
End Class
