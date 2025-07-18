<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Waste_Creation
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
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cbo_coneType = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cbo_bagType = New System.Windows.Forms.ComboBox()
        Me.txt_weightemptybag = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btn_FilterClose = New System.Windows.Forms.Button()
        Me.btn_FilterOpen = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.grp_Find = New System.Windows.Forms.GroupBox()
        Me.btn_FindClose = New System.Windows.Forms.Button()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.cbo_Find = New System.Windows.Forms.ComboBox()
        Me.grp_filter = New System.Windows.Forms.GroupBox()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.pnl_back.SuspendLayout()
        Me.grp_Find.SuspendLayout()
        Me.grp_filter.SuspendLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnl_back
        '
        Me.pnl_back.BackColor = System.Drawing.SystemColors.Control
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.Label23)
        Me.pnl_back.Controls.Add(Me.Label6)
        Me.pnl_back.Controls.Add(Me.cbo_coneType)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Controls.Add(Me.cbo_bagType)
        Me.pnl_back.Controls.Add(Me.txt_weightemptybag)
        Me.pnl_back.Controls.Add(Me.Label5)
        Me.pnl_back.Controls.Add(Me.btn_Close)
        Me.pnl_back.Controls.Add(Me.btn_Save)
        Me.pnl_back.Controls.Add(Me.txt_Name)
        Me.pnl_back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_back.Location = New System.Drawing.Point(8, 50)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(478, 189)
        Me.pnl_back.TabIndex = 12
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(255, 83)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 15)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Cone Type"
        '
        'cbo_coneType
        '
        Me.cbo_coneType.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_coneType.FormattingEnabled = True
        Me.cbo_coneType.Location = New System.Drawing.Point(327, 79)
        Me.cbo_coneType.MaxDropDownItems = 15
        Me.cbo_coneType.MaxLength = 50
        Me.cbo_coneType.Name = "cbo_coneType"
        Me.cbo_coneType.Size = New System.Drawing.Size(137, 23)
        Me.cbo_coneType.Sorted = True
        Me.cbo_coneType.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 82)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 15)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Bag Type"
        '
        'cbo_bagType
        '
        Me.cbo_bagType.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_bagType.FormattingEnabled = True
        Me.cbo_bagType.Location = New System.Drawing.Point(119, 78)
        Me.cbo_bagType.MaxDropDownItems = 15
        Me.cbo_bagType.MaxLength = 50
        Me.cbo_bagType.Name = "cbo_bagType"
        Me.cbo_bagType.Size = New System.Drawing.Size(126, 23)
        Me.cbo_bagType.Sorted = True
        Me.cbo_bagType.TabIndex = 1
        '
        'txt_weightemptybag
        '
        Me.txt_weightemptybag.Location = New System.Drawing.Point(119, 112)
        Me.txt_weightemptybag.MaxLength = 8
        Me.txt_weightemptybag.Name = "txt_weightemptybag"
        Me.txt_weightemptybag.Size = New System.Drawing.Size(345, 23)
        Me.txt_weightemptybag.TabIndex = 3
        Me.txt_weightemptybag.Text = "txt_weightemptyBag"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 116)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(99, 15)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Weight\Material"
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(377, 150)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(87, 27)
        Me.btn_Close.TabIndex = 3
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_Save.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.Location = New System.Drawing.Point(261, 150)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(87, 27)
        Me.btn_Save.TabIndex = 4
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'txt_Name
        '
        Me.txt_Name.BackColor = System.Drawing.Color.White
        Me.txt_Name.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Name.Location = New System.Drawing.Point(119, 45)
        Me.txt_Name.MaxLength = 35
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(345, 23)
        Me.txt_Name.TabIndex = 0
        Me.txt_Name.Text = "TXT_NAME"
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_IdNo.Location = New System.Drawing.Point(119, 8)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(345, 27)
        Me.lbl_IdNo.TabIndex = 2
        Me.lbl_IdNo.Text = "lbl_idno"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(14, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(14, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IdNo"
        '
        'btn_FilterClose
        '
        Me.btn_FilterClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_FilterClose.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_FilterClose.ForeColor = System.Drawing.Color.White
        Me.btn_FilterClose.Location = New System.Drawing.Point(369, 132)
        Me.btn_FilterClose.Name = "btn_FilterClose"
        Me.btn_FilterClose.Size = New System.Drawing.Size(87, 27)
        Me.btn_FilterClose.TabIndex = 2
        Me.btn_FilterClose.Text = "&CLOSE"
        Me.btn_FilterClose.UseVisualStyleBackColor = False
        '
        'btn_FilterOpen
        '
        Me.btn_FilterOpen.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_FilterOpen.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_FilterOpen.ForeColor = System.Drawing.Color.White
        Me.btn_FilterOpen.Location = New System.Drawing.Point(261, 132)
        Me.btn_FilterOpen.Name = "btn_FilterOpen"
        Me.btn_FilterOpen.Size = New System.Drawing.Size(87, 27)
        Me.btn_FilterOpen.TabIndex = 1
        Me.btn_FilterOpen.Text = "&OPEN"
        Me.btn_FilterOpen.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(496, 40)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "WASTE CREATION"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grp_Find
        '
        Me.grp_Find.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.grp_Find.Controls.Add(Me.btn_FindClose)
        Me.grp_Find.Controls.Add(Me.btn_Open)
        Me.grp_Find.Controls.Add(Me.cbo_Find)
        Me.grp_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Find.Location = New System.Drawing.Point(8, 276)
        Me.grp_Find.Name = "grp_Find"
        Me.grp_Find.Size = New System.Drawing.Size(478, 172)
        Me.grp_Find.TabIndex = 15
        Me.grp_Find.TabStop = False
        Me.grp_Find.Text = "FINDING"
        '
        'btn_FindClose
        '
        Me.btn_FindClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_FindClose.ForeColor = System.Drawing.Color.White
        Me.btn_FindClose.Location = New System.Drawing.Point(369, 122)
        Me.btn_FindClose.Name = "btn_FindClose"
        Me.btn_FindClose.Size = New System.Drawing.Size(87, 27)
        Me.btn_FindClose.TabIndex = 2
        Me.btn_FindClose.Text = "CLOSE"
        Me.btn_FindClose.UseVisualStyleBackColor = False
        '
        'btn_Open
        '
        Me.btn_Open.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Open.ForeColor = System.Drawing.Color.White
        Me.btn_Open.Location = New System.Drawing.Point(253, 122)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(87, 27)
        Me.btn_Open.TabIndex = 1
        Me.btn_Open.Text = "&OPEN"
        Me.btn_Open.UseVisualStyleBackColor = False
        '
        'cbo_Find
        '
        Me.cbo_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Find.FormattingEnabled = True
        Me.cbo_Find.Location = New System.Drawing.Point(27, 35)
        Me.cbo_Find.Name = "cbo_Find"
        Me.cbo_Find.Size = New System.Drawing.Size(429, 23)
        Me.cbo_Find.TabIndex = 0
        Me.cbo_Find.Text = "cbo_find"
        '
        'grp_filter
        '
        Me.grp_filter.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.grp_filter.Controls.Add(Me.btn_FilterClose)
        Me.grp_filter.Controls.Add(Me.btn_FilterOpen)
        Me.grp_filter.Controls.Add(Me.dgv_filter)
        Me.grp_filter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_filter.Location = New System.Drawing.Point(513, 198)
        Me.grp_filter.Name = "grp_filter"
        Me.grp_filter.Size = New System.Drawing.Size(478, 172)
        Me.grp_filter.TabIndex = 13
        Me.grp_filter.TabStop = False
        Me.grp_filter.Text = "FILTER"
        '
        'dgv_filter
        '
        Me.dgv_filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_filter.Location = New System.Drawing.Point(27, 25)
        Me.dgv_filter.Name = "dgv_filter"
        Me.dgv_filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_filter.Size = New System.Drawing.Size(429, 98)
        Me.dgv_filter.TabIndex = 0
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Red
        Me.Label23.Location = New System.Drawing.Point(50, 50)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(13, 15)
        Me.Label23.TabIndex = 300
        Me.Label23.Text = "*"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Waste_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(496, 257)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.grp_Find)
        Me.Controls.Add(Me.grp_filter)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Waste_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "WASTE CREATION"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.grp_Find.ResumeLayout(False)
        Me.grp_filter.ResumeLayout(False)
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btn_FilterClose As System.Windows.Forms.Button
    Friend WithEvents btn_FilterOpen As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents grp_Find As System.Windows.Forms.GroupBox
    Friend WithEvents btn_FindClose As System.Windows.Forms.Button
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents cbo_Find As System.Windows.Forms.ComboBox
    Friend WithEvents grp_filter As System.Windows.Forms.GroupBox
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents txt_weightemptybag As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cbo_bagType As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cbo_coneType As System.Windows.Forms.ComboBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
End Class
