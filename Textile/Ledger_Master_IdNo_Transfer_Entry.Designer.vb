<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Ledger_Master_IdNo_Transfer_Entry
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Ledger_Master_IdNo_Transfer_Entry))
        Me.Label3 = New System.Windows.Forms.Label()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_CloseOpen = New System.Windows.Forms.Button()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.btn_Transfer = New System.Windows.Forms.Button()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.cbo_ValueTo = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbo_ValueFrom = New System.Windows.Forms.ComboBox()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.grp_Open.SuspendLayout()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(414, 33)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "LEDGER NAME TRANSFER"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grp_Open
        '
        Me.grp_Open.Controls.Add(Me.btn_CloseOpen)
        Me.grp_Open.Controls.Add(Me.btn_Open)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Location = New System.Drawing.Point(20, 439)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(481, 219)
        Me.grp_Open.TabIndex = 41
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "FINIDING"
        '
        'btn_CloseOpen
        '
        Me.btn_CloseOpen.BackColor = System.Drawing.Color.MidnightBlue
        Me.btn_CloseOpen.ForeColor = System.Drawing.Color.White
        Me.btn_CloseOpen.Image = CType(resources.GetObject("btn_CloseOpen.Image"), System.Drawing.Image)
        Me.btn_CloseOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseOpen.Location = New System.Drawing.Point(384, 143)
        Me.btn_CloseOpen.Name = "btn_CloseOpen"
        Me.btn_CloseOpen.Size = New System.Drawing.Size(82, 35)
        Me.btn_CloseOpen.TabIndex = 4
        Me.btn_CloseOpen.Text = "&CLOSE"
        Me.btn_CloseOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseOpen.UseVisualStyleBackColor = False
        '
        'btn_Open
        '
        Me.btn_Open.BackColor = System.Drawing.Color.MidnightBlue
        Me.btn_Open.ForeColor = System.Drawing.Color.White
        Me.btn_Open.Image = CType(resources.GetObject("btn_Open.Image"), System.Drawing.Image)
        Me.btn_Open.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Open.Location = New System.Drawing.Point(269, 143)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(82, 35)
        Me.btn_Open.TabIndex = 3
        Me.btn_Open.Text = "&OPEN     "
        Me.btn_Open.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Open.UseVisualStyleBackColor = False
        '
        'cbo_Open
        '
        Me.cbo_Open.DropDownHeight = 100
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(23, 44)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(443, 23)
        Me.cbo_Open.TabIndex = 0
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'btn_Transfer
        '
        Me.btn_Transfer.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Transfer.ForeColor = System.Drawing.Color.White
        Me.btn_Transfer.Location = New System.Drawing.Point(108, 140)
        Me.btn_Transfer.Name = "btn_Transfer"
        Me.btn_Transfer.Size = New System.Drawing.Size(78, 28)
        Me.btn_Transfer.TabIndex = 3
        Me.btn_Transfer.TabStop = False
        Me.btn_Transfer.Text = "&TRANSFER"
        Me.btn_Transfer.UseVisualStyleBackColor = False
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(209, 140)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(66, 28)
        Me.btn_Close.TabIndex = 14
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.ForeColor = System.Drawing.Color.Black
        Me.Label39.Location = New System.Drawing.Point(20, 69)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(104, 15)
        Me.Label39.TabIndex = 322
        Me.Label39.Text = "LEDGER VALUE TO"
        '
        'cbo_ValueTo
        '
        Me.cbo_ValueTo.DropDownHeight = 100
        Me.cbo_ValueTo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_ValueTo.FormattingEnabled = True
        Me.cbo_ValueTo.IntegralHeight = False
        Me.cbo_ValueTo.Location = New System.Drawing.Point(20, 87)
        Me.cbo_ValueTo.MaxDropDownItems = 15
        Me.cbo_ValueTo.MaxLength = 50
        Me.cbo_ValueTo.Name = "cbo_ValueTo"
        Me.cbo_ValueTo.Size = New System.Drawing.Size(338, 23)
        Me.cbo_ValueTo.Sorted = True
        Me.cbo_ValueTo.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(20, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 15)
        Me.Label1.TabIndex = 324
        Me.Label1.Text = "LEDGER VALUE FROM"
        '
        'cbo_ValueFrom
        '
        Me.cbo_ValueFrom.DropDownHeight = 150
        Me.cbo_ValueFrom.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_ValueFrom.FormattingEnabled = True
        Me.cbo_ValueFrom.IntegralHeight = False
        Me.cbo_ValueFrom.Location = New System.Drawing.Point(20, 34)
        Me.cbo_ValueFrom.MaxDropDownItems = 15
        Me.cbo_ValueFrom.MaxLength = 50
        Me.cbo_ValueFrom.Name = "cbo_ValueFrom"
        Me.cbo_ValueFrom.Size = New System.Drawing.Size(338, 23)
        Me.cbo_ValueFrom.Sorted = True
        Me.cbo_ValueFrom.TabIndex = 1
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.cbo_ValueFrom)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.cbo_ValueTo)
        Me.pnl_Back.Controls.Add(Me.Label39)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.btn_Transfer)
        Me.pnl_Back.Location = New System.Drawing.Point(9, 36)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(393, 188)
        Me.pnl_Back.TabIndex = 39
        '
        'Ledger_Master_IdNo_Transfer_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(414, 241)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.Label3)
        Me.Font = New System.Drawing.Font("Calibri", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Ledger_Master_IdNo_Transfer_Entry"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TRANSPORT TRANSFER"
        Me.grp_Open.ResumeLayout(False)
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents btn_CloseOpen As System.Windows.Forms.Button
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btn_Transfer As System.Windows.Forms.Button
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents cbo_ValueTo As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbo_ValueFrom As System.Windows.Forms.ComboBox
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
End Class
