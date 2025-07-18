<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Market_Status_Creation
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
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.btn_CloseOpen = New System.Windows.Forms.Button()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.pnl_Back.SuspendLayout()
        Me.grp_Open.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.btn_Save)
        Me.pnl_Back.Controls.Add(Me.txt_Name)
        Me.pnl_Back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Location = New System.Drawing.Point(7, 42)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(572, 159)
        Me.pnl_Back.TabIndex = 0
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.Tomato
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(468, 110)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(72, 32)
        Me.btn_Close.TabIndex = 2
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.Location = New System.Drawing.Point(366, 110)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(72, 32)
        Me.btn_Save.TabIndex = 1
        Me.btn_Save.TabStop = False
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'txt_Name
        '
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Location = New System.Drawing.Point(141, 64)
        Me.txt_Name.MaxLength = 50
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(399, 23)
        Me.txt_Name.TabIndex = 0
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Location = New System.Drawing.Point(141, 18)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(399, 23)
        Me.lbl_IdNo.TabIndex = 15
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(15, 60)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(85, 30)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Market Status Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(15, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 15)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "IdNo"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(585, 35)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "MARKET STATUS CREATION"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grp_Open
        '
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Controls.Add(Me.btn_CloseOpen)
        Me.grp_Open.Controls.Add(Me.btn_Open)
        Me.grp_Open.Location = New System.Drawing.Point(11, 205)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(567, 194)
        Me.grp_Open.TabIndex = 2
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "FINDING"
        Me.grp_Open.Visible = False
        '
        'cbo_Open
        '
        Me.cbo_Open.DropDownHeight = 90
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(15, 30)
        Me.cbo_Open.MaxDropDownItems = 4
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(522, 23)
        Me.cbo_Open.TabIndex = 3
        Me.cbo_Open.TabStop = False
        '
        'btn_CloseOpen
        '
        Me.btn_CloseOpen.BackColor = System.Drawing.Color.Tomato
        Me.btn_CloseOpen.ForeColor = System.Drawing.Color.White
        Me.btn_CloseOpen.Location = New System.Drawing.Point(468, 146)
        Me.btn_CloseOpen.Name = "btn_CloseOpen"
        Me.btn_CloseOpen.Size = New System.Drawing.Size(72, 32)
        Me.btn_CloseOpen.TabIndex = 5
        Me.btn_CloseOpen.TabStop = False
        Me.btn_CloseOpen.Text = "&CLOSE"
        Me.btn_CloseOpen.UseVisualStyleBackColor = False
        '
        'btn_Open
        '
        Me.btn_Open.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_Open.ForeColor = System.Drawing.Color.White
        Me.btn_Open.Location = New System.Drawing.Point(366, 146)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(72, 32)
        Me.btn_Open.TabIndex = 4
        Me.btn_Open.TabStop = False
        Me.btn_Open.Text = "&FIND"
        Me.btn_Open.UseVisualStyleBackColor = False
        '
        'Market_Status_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(585, 408)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Market_Status_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MARKET STATUS CREATION"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.grp_Open.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents btn_CloseOpen As System.Windows.Forms.Button
    Friend WithEvents btn_Open As System.Windows.Forms.Button
End Class
