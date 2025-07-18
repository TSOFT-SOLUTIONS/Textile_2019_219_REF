<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Transfer_Piece_Opening
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
        Me.btn_All_Piece_Transfer = New System.Windows.Forms.Button()
        Me.dtp_UpDate = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btn_JobWorker_Delivered_Piece_Transfer = New System.Windows.Forms.Button()
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
        'btn_All_Piece_Transfer
        '
        Me.btn_All_Piece_Transfer.Enabled = False
        Me.btn_All_Piece_Transfer.Location = New System.Drawing.Point(122, 122)
        Me.btn_All_Piece_Transfer.Name = "btn_All_Piece_Transfer"
        Me.btn_All_Piece_Transfer.Size = New System.Drawing.Size(149, 66)
        Me.btn_All_Piece_Transfer.TabIndex = 2
        Me.btn_All_Piece_Transfer.Text = "&ALL PIECE TRANSFER"
        Me.btn_All_Piece_Transfer.UseVisualStyleBackColor = True
        '
        'dtp_UpDate
        '
        Me.dtp_UpDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_UpDate.Location = New System.Drawing.Point(124, 78)
        Me.dtp_UpDate.Name = "dtp_UpDate"
        Me.dtp_UpDate.Size = New System.Drawing.Size(127, 23)
        Me.dtp_UpDate.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(21, 82)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Up To Date"
        '
        'btn_JobWorker_Delivered_Piece_Transfer
        '
        Me.btn_JobWorker_Delivered_Piece_Transfer.Location = New System.Drawing.Point(329, 122)
        Me.btn_JobWorker_Delivered_Piece_Transfer.Name = "btn_JobWorker_Delivered_Piece_Transfer"
        Me.btn_JobWorker_Delivered_Piece_Transfer.Size = New System.Drawing.Size(149, 66)
        Me.btn_JobWorker_Delivered_Piece_Transfer.TabIndex = 4
        Me.btn_JobWorker_Delivered_Piece_Transfer.Text = "&JOBWORK DELIVERD PIECE TRANSFER"
        Me.btn_JobWorker_Delivered_Piece_Transfer.UseVisualStyleBackColor = True
        '
        'Transfer_Piece_Opening
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(554, 213)
        Me.Controls.Add(Me.btn_JobWorker_Delivered_Piece_Transfer)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtp_UpDate)
        Me.Controls.Add(Me.btn_All_Piece_Transfer)
        Me.Controls.Add(Me.cbo_DBFrom)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "Transfer_Piece_Opening"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TRANSFER MASTERS"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbo_DBFrom As System.Windows.Forms.ComboBox
    Friend WithEvents btn_All_Piece_Transfer As System.Windows.Forms.Button
    Friend WithEvents dtp_UpDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btn_JobWorker_Delivered_Piece_Transfer As System.Windows.Forms.Button
End Class
