<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OE_Machine_Details
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.txt_Efficiency = New System.Windows.Forms.TextBox()
        Me.txt_CountHank = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cbo_MachineName = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.cbo_Manufacturer = New System.Windows.Forms.ComboBox()
        Me.txt_MachineNo = New System.Windows.Forms.TextBox()
        Me.txt_Speed = New System.Windows.Forms.TextBox()
        Me.lbl_Speed = New System.Windows.Forms.Label()
        Me.cbo_Count = New System.Windows.Forms.ComboBox()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txt_EndingBackNo = New System.Windows.Forms.TextBox()
        Me.grp_find = New System.Windows.Forms.GroupBox()
        Me.btn_FindOpen = New System.Windows.Forms.Button()
        Me.btn_FindClose = New System.Windows.Forms.Button()
        Me.cbo_Find = New System.Windows.Forms.ComboBox()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.btn_Filteropen = New System.Windows.Forms.Button()
        Me.btn_FilterClose = New System.Windows.Forms.Button()
        Me.pnl_back.SuspendLayout()
        Me.grp_find.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.ForeColor = System.Drawing.Color.Blue
        Me.Label21.Location = New System.Drawing.Point(351, 138)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(66, 15)
        Me.Label21.TabIndex = 80
        Me.Label21.Text = "Efficiency%"
        '
        'txt_Efficiency
        '
        Me.txt_Efficiency.Location = New System.Drawing.Point(440, 133)
        Me.txt_Efficiency.Name = "txt_Efficiency"
        Me.txt_Efficiency.Size = New System.Drawing.Size(217, 23)
        Me.txt_Efficiency.TabIndex = 7
        '
        'txt_CountHank
        '
        Me.txt_CountHank.Location = New System.Drawing.Point(440, 92)
        Me.txt_CountHank.Name = "txt_CountHank"
        Me.txt_CountHank.ReadOnly = True
        Me.txt_CountHank.Size = New System.Drawing.Size(217, 23)
        Me.txt_CountHank.TabIndex = 5
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.ForeColor = System.Drawing.Color.Blue
        Me.Label22.Location = New System.Drawing.Point(7, 54)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(78, 15)
        Me.Label22.TabIndex = 34
        Me.Label22.Text = "Model Name"
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.ForeColor = System.Drawing.Color.Black
        Me.lbl_IdNo.Location = New System.Drawing.Point(103, 10)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(217, 23)
        Me.lbl_IdNo.TabIndex = 0
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbl_Heading
        '
        Me.lbl_Heading.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.lbl_Heading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(687, 35)
        Me.lbl_Heading.TabIndex = 38
        Me.lbl_Heading.Text = "MACHINE DETAILS"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(575, 175)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(82, 35)
        Me.btn_close.TabIndex = 10
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(351, 56)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(82, 15)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Manufacturer"
        '
        'cbo_MachineName
        '
        Me.cbo_MachineName.DropDownHeight = 250
        Me.cbo_MachineName.FormattingEnabled = True
        Me.cbo_MachineName.IntegralHeight = False
        Me.cbo_MachineName.Location = New System.Drawing.Point(103, 51)
        Me.cbo_MachineName.MaxLength = 35
        Me.cbo_MachineName.Name = "cbo_MachineName"
        Me.cbo_MachineName.Size = New System.Drawing.Size(217, 23)
        Me.cbo_MachineName.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(7, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Machine IdNo"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 260)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(0, 15)
        Me.Label7.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(28, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 15)
        Me.Label3.TabIndex = 2
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Blue
        Me.Label10.Location = New System.Drawing.Point(351, 94)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(70, 15)
        Me.Label10.TabIndex = 71
        Me.Label10.Text = "Count Hank"
        '
        'pnl_back
        '
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.cbo_Manufacturer)
        Me.pnl_back.Controls.Add(Me.txt_MachineNo)
        Me.pnl_back.Controls.Add(Me.txt_Speed)
        Me.pnl_back.Controls.Add(Me.Label21)
        Me.pnl_back.Controls.Add(Me.txt_Efficiency)
        Me.pnl_back.Controls.Add(Me.txt_CountHank)
        Me.pnl_back.Controls.Add(Me.Label10)
        Me.pnl_back.Controls.Add(Me.lbl_Speed)
        Me.pnl_back.Controls.Add(Me.cbo_Count)
        Me.pnl_back.Controls.Add(Me.Label22)
        Me.pnl_back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.Label8)
        Me.pnl_back.Controls.Add(Me.cbo_MachineName)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Controls.Add(Me.Label5)
        Me.pnl_back.Location = New System.Drawing.Point(6, 42)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(671, 223)
        Me.pnl_back.TabIndex = 37
        '
        'cbo_Manufacturer
        '
        Me.cbo_Manufacturer.FormattingEnabled = True
        Me.cbo_Manufacturer.Location = New System.Drawing.Point(440, 51)
        Me.cbo_Manufacturer.Name = "cbo_Manufacturer"
        Me.cbo_Manufacturer.Size = New System.Drawing.Size(217, 23)
        Me.cbo_Manufacturer.TabIndex = 3
        '
        'txt_MachineNo
        '
        Me.txt_MachineNo.Location = New System.Drawing.Point(440, 10)
        Me.txt_MachineNo.MaxLength = 50
        Me.txt_MachineNo.Name = "txt_MachineNo"
        Me.txt_MachineNo.Size = New System.Drawing.Size(217, 23)
        Me.txt_MachineNo.TabIndex = 1
        '
        'txt_Speed
        '
        Me.txt_Speed.Location = New System.Drawing.Point(103, 133)
        Me.txt_Speed.Name = "txt_Speed"
        Me.txt_Speed.Size = New System.Drawing.Size(217, 23)
        Me.txt_Speed.TabIndex = 6
        '
        'lbl_Speed
        '
        Me.lbl_Speed.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Speed.Location = New System.Drawing.Point(7, 135)
        Me.lbl_Speed.Name = "lbl_Speed"
        Me.lbl_Speed.Size = New System.Drawing.Size(82, 21)
        Me.lbl_Speed.TabIndex = 67
        Me.lbl_Speed.Text = "Speed"
        '
        'cbo_Count
        '
        Me.cbo_Count.FormattingEnabled = True
        Me.cbo_Count.Location = New System.Drawing.Point(103, 91)
        Me.cbo_Count.Name = "cbo_Count"
        Me.cbo_Count.Size = New System.Drawing.Size(217, 23)
        Me.cbo_Count.TabIndex = 4
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(472, 175)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(82, 35)
        Me.btn_save.TabIndex = 9
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(351, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Machine No"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(8, 95)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 15)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Count"
        '
        'txt_EndingBackNo
        '
        Me.txt_EndingBackNo.Location = New System.Drawing.Point(108, 84)
        Me.txt_EndingBackNo.Name = "txt_EndingBackNo"
        Me.txt_EndingBackNo.Size = New System.Drawing.Size(202, 20)
        Me.txt_EndingBackNo.TabIndex = 3
        '
        'grp_find
        '
        Me.grp_find.BackColor = System.Drawing.Color.LightSkyBlue
        Me.grp_find.Controls.Add(Me.btn_FindOpen)
        Me.grp_find.Controls.Add(Me.btn_FindClose)
        Me.grp_find.Controls.Add(Me.cbo_Find)
        Me.grp_find.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.grp_find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_find.Location = New System.Drawing.Point(752, 50)
        Me.grp_find.Name = "grp_find"
        Me.grp_find.Size = New System.Drawing.Size(516, 174)
        Me.grp_find.TabIndex = 39
        Me.grp_find.TabStop = False
        Me.grp_find.Text = "FINDING"
        '
        'btn_FindOpen
        '
        Me.btn_FindOpen.BackColor = System.Drawing.Color.Maroon
        Me.btn_FindOpen.ForeColor = System.Drawing.Color.White
        Me.btn_FindOpen.Location = New System.Drawing.Point(320, 25)
        Me.btn_FindOpen.Name = "btn_FindOpen"
        Me.btn_FindOpen.Size = New System.Drawing.Size(77, 28)
        Me.btn_FindOpen.TabIndex = 1
        Me.btn_FindOpen.Text = "&OPEN"
        Me.btn_FindOpen.UseVisualStyleBackColor = False
        '
        'btn_FindClose
        '
        Me.btn_FindClose.BackColor = System.Drawing.Color.Maroon
        Me.btn_FindClose.ForeColor = System.Drawing.Color.White
        Me.btn_FindClose.Location = New System.Drawing.Point(429, 25)
        Me.btn_FindClose.Name = "btn_FindClose"
        Me.btn_FindClose.Size = New System.Drawing.Size(77, 28)
        Me.btn_FindClose.TabIndex = 2
        Me.btn_FindClose.Text = "&CLOSE"
        Me.btn_FindClose.UseVisualStyleBackColor = False
        '
        'cbo_Find
        '
        Me.cbo_Find.DropDownHeight = 80
        Me.cbo_Find.FormattingEnabled = True
        Me.cbo_Find.IntegralHeight = False
        Me.cbo_Find.Location = New System.Drawing.Point(18, 25)
        Me.cbo_Find.Name = "cbo_Find"
        Me.cbo_Find.Size = New System.Drawing.Size(276, 23)
        Me.cbo_Find.TabIndex = 5
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_Filteropen)
        Me.grp_Filter.Controls.Add(Me.btn_FilterClose)
        Me.grp_Filter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(733, 346)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(516, 221)
        Me.grp_Filter.TabIndex = 40
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "Filter"
        '
        'dgv_Filter
        '
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Filter.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_Filter.Location = New System.Drawing.Point(18, 25)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(475, 156)
        Me.dgv_Filter.TabIndex = 1
        '
        'btn_Filteropen
        '
        Me.btn_Filteropen.BackColor = System.Drawing.Color.Maroon
        Me.btn_Filteropen.ForeColor = System.Drawing.Color.White
        Me.btn_Filteropen.Location = New System.Drawing.Point(315, 187)
        Me.btn_Filteropen.Name = "btn_Filteropen"
        Me.btn_Filteropen.Size = New System.Drawing.Size(77, 28)
        Me.btn_Filteropen.TabIndex = 10
        Me.btn_Filteropen.Text = "&OPEN"
        Me.btn_Filteropen.UseVisualStyleBackColor = False
        '
        'btn_FilterClose
        '
        Me.btn_FilterClose.BackColor = System.Drawing.Color.Maroon
        Me.btn_FilterClose.ForeColor = System.Drawing.Color.White
        Me.btn_FilterClose.Location = New System.Drawing.Point(416, 187)
        Me.btn_FilterClose.Name = "btn_FilterClose"
        Me.btn_FilterClose.Size = New System.Drawing.Size(77, 28)
        Me.btn_FilterClose.TabIndex = 1
        Me.btn_FilterClose.Text = "&CLOSE"
        Me.btn_FilterClose.UseVisualStyleBackColor = False
        '
        'OE_Machine_Details
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(687, 272)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.grp_find)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Controls.Add(Me.pnl_back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OE_Machine_Details"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MACHINE DETAILS"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.grp_find.ResumeLayout(False)
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents txt_Efficiency As System.Windows.Forms.TextBox
    Friend WithEvents txt_CountHank As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cbo_MachineName As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents cbo_Count As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txt_EndingBackNo As System.Windows.Forms.TextBox
    Friend WithEvents txt_MachineNo As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grp_find As System.Windows.Forms.GroupBox
    Friend WithEvents btn_FindOpen As System.Windows.Forms.Button
    Friend WithEvents btn_FindClose As System.Windows.Forms.Button
    Friend WithEvents cbo_Find As System.Windows.Forms.ComboBox
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_Filteropen As System.Windows.Forms.Button
    Friend WithEvents btn_FilterClose As System.Windows.Forms.Button
    Friend WithEvents btn_save As Button
    Friend WithEvents cbo_Manufacturer As ComboBox
    Friend WithEvents txt_Speed As TextBox
    Friend WithEvents lbl_Speed As Label
End Class
