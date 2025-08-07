<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Bobin_Warping_Production_Entry
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
        Me.Pnl_back = New System.Windows.Forms.Panel()
        Me.lbl_Reel = New System.Windows.Forms.Label()
        Me.msk_date = New System.Windows.Forms.MaskedTextBox()
        Me.dtp_date = New System.Windows.Forms.DateTimePicker()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cbo_empName = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txt_Bobin = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txt_meters = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txt_ends = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbl_RefNo = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbl_company = New System.Windows.Forms.Label()
        Me.pnl_Filter = New System.Windows.Forms.Panel()
        Me.btn_filter_close = New System.Windows.Forms.Button()
        Me.btn_Filter_Show = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cbo_Filter_EmpName = New System.Windows.Forms.ComboBox()
        Me.dtp_Filter_Todate = New System.Windows.Forms.DateTimePicker()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.dtp_Filter_Fromdate = New System.Windows.Forms.DateTimePicker()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.dgv_Filter_Details = New System.Windows.Forms.DataGridView()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Pnl_back.SuspendLayout()
        Me.pnl_Filter.SuspendLayout()
        CType(Me.dgv_Filter_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Pnl_back
        '
        Me.Pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Pnl_back.Controls.Add(Me.lbl_Reel)
        Me.Pnl_back.Controls.Add(Me.msk_date)
        Me.Pnl_back.Controls.Add(Me.dtp_date)
        Me.Pnl_back.Controls.Add(Me.btn_close)
        Me.Pnl_back.Controls.Add(Me.btn_Save)
        Me.Pnl_back.Controls.Add(Me.Label8)
        Me.Pnl_back.Controls.Add(Me.cbo_empName)
        Me.Pnl_back.Controls.Add(Me.Label7)
        Me.Pnl_back.Controls.Add(Me.txt_Bobin)
        Me.Pnl_back.Controls.Add(Me.Label6)
        Me.Pnl_back.Controls.Add(Me.txt_meters)
        Me.Pnl_back.Controls.Add(Me.Label5)
        Me.Pnl_back.Controls.Add(Me.txt_ends)
        Me.Pnl_back.Controls.Add(Me.Label4)
        Me.Pnl_back.Controls.Add(Me.lbl_RefNo)
        Me.Pnl_back.Controls.Add(Me.Label3)
        Me.Pnl_back.Controls.Add(Me.Label2)
        Me.Pnl_back.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Pnl_back.Location = New System.Drawing.Point(21, 42)
        Me.Pnl_back.Name = "Pnl_back"
        Me.Pnl_back.Size = New System.Drawing.Size(495, 281)
        Me.Pnl_back.TabIndex = 0
        '
        'lbl_Reel
        '
        Me.lbl_Reel.BackColor = System.Drawing.Color.White
        Me.lbl_Reel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_Reel.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Reel.Location = New System.Drawing.Point(167, 193)
        Me.lbl_Reel.Name = "lbl_Reel"
        Me.lbl_Reel.Size = New System.Drawing.Size(288, 23)
        Me.lbl_Reel.TabIndex = 14
        '
        'msk_date
        '
        Me.msk_date.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_date.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_date.Location = New System.Drawing.Point(326, 19)
        Me.msk_date.Mask = "00-00-0000"
        Me.msk_date.Name = "msk_date"
        Me.msk_date.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_date.Size = New System.Drawing.Size(111, 23)
        Me.msk_date.TabIndex = 0
        '
        'dtp_date
        '
        Me.dtp_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_date.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_date.Location = New System.Drawing.Point(429, 19)
        Me.dtp_date.Name = "dtp_date"
        Me.dtp_date.Size = New System.Drawing.Size(26, 22)
        Me.dtp_date.TabIndex = 1
        Me.dtp_date.TabStop = False
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.DeepPink
        Me.btn_close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(379, 236)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(75, 33)
        Me.btn_close.TabIndex = 8
        Me.btn_close.Text = "Close"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.DeepPink
        Me.btn_Save.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.Location = New System.Drawing.Point(269, 236)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(75, 33)
        Me.btn_Save.TabIndex = 7
        Me.btn_Save.Text = "Save"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(39, 197)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(31, 15)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "Reel"
        '
        'cbo_empName
        '
        Me.cbo_empName.DropDownHeight = 80
        Me.cbo_empName.DropDownWidth = 200
        Me.cbo_empName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_empName.FormattingEnabled = True
        Me.cbo_empName.IntegralHeight = False
        Me.cbo_empName.Location = New System.Drawing.Point(167, 49)
        Me.cbo_empName.Name = "cbo_empName"
        Me.cbo_empName.Size = New System.Drawing.Size(287, 23)
        Me.cbo_empName.TabIndex = 2
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(39, 53)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(97, 15)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Employee Name"
        '
        'txt_Bobin
        '
        Me.txt_Bobin.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Bobin.Location = New System.Drawing.Point(167, 121)
        Me.txt_Bobin.Name = "txt_Bobin"
        Me.txt_Bobin.Size = New System.Drawing.Size(288, 23)
        Me.txt_Bobin.TabIndex = 4
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(39, 125)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(77, 15)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "No.Of.Bobins"
        '
        'txt_meters
        '
        Me.txt_meters.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_meters.Location = New System.Drawing.Point(167, 157)
        Me.txt_meters.Name = "txt_meters"
        Me.txt_meters.Size = New System.Drawing.Size(288, 23)
        Me.txt_meters.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(39, 161)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(47, 15)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Meters"
        '
        'txt_ends
        '
        Me.txt_ends.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_ends.Location = New System.Drawing.Point(167, 85)
        Me.txt_ends.Name = "txt_ends"
        Me.txt_ends.Size = New System.Drawing.Size(288, 23)
        Me.txt_ends.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(39, 89)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(32, 15)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Ends"
        '
        'lbl_RefNo
        '
        Me.lbl_RefNo.BackColor = System.Drawing.Color.White
        Me.lbl_RefNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_RefNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RefNo.Location = New System.Drawing.Point(167, 19)
        Me.lbl_RefNo.Name = "lbl_RefNo"
        Me.lbl_RefNo.Size = New System.Drawing.Size(111, 22)
        Me.lbl_RefNo.TabIndex = 4
        Me.lbl_RefNo.Text = "lbl_RefNo."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(280, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(33, 15)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Date"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(39, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 15)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Ref No."
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(541, 30)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "WARPING BOBIN ENTRY"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_company
        '
        Me.lbl_company.AutoSize = True
        Me.lbl_company.Location = New System.Drawing.Point(52, 11)
        Me.lbl_company.Name = "lbl_company"
        Me.lbl_company.Size = New System.Drawing.Size(66, 13)
        Me.lbl_company.TabIndex = 2
        Me.lbl_company.Text = "lbl_company"
        '
        'pnl_Filter
        '
        Me.pnl_Filter.BackColor = System.Drawing.Color.White
        Me.pnl_Filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Filter.Controls.Add(Me.btn_filter_close)
        Me.pnl_Filter.Controls.Add(Me.btn_Filter_Show)
        Me.pnl_Filter.Controls.Add(Me.Label13)
        Me.pnl_Filter.Controls.Add(Me.cbo_Filter_EmpName)
        Me.pnl_Filter.Controls.Add(Me.dtp_Filter_Todate)
        Me.pnl_Filter.Controls.Add(Me.Label12)
        Me.pnl_Filter.Controls.Add(Me.dtp_Filter_Fromdate)
        Me.pnl_Filter.Controls.Add(Me.Label11)
        Me.pnl_Filter.Controls.Add(Me.dgv_Filter_Details)
        Me.pnl_Filter.Controls.Add(Me.Label10)
        Me.pnl_Filter.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_Filter.Location = New System.Drawing.Point(587, 92)
        Me.pnl_Filter.Name = "pnl_Filter"
        Me.pnl_Filter.Size = New System.Drawing.Size(549, 302)
        Me.pnl_Filter.TabIndex = 13
        '
        'btn_filter_close
        '
        Me.btn_filter_close.ForeColor = System.Drawing.Color.MediumBlue
        Me.btn_filter_close.Location = New System.Drawing.Point(451, 33)
        Me.btn_filter_close.Name = "btn_filter_close"
        Me.btn_filter_close.Size = New System.Drawing.Size(54, 68)
        Me.btn_filter_close.TabIndex = 13
        Me.btn_filter_close.Text = "Close"
        Me.btn_filter_close.UseVisualStyleBackColor = False
        '
        'btn_Filter_Show
        '
        Me.btn_Filter_Show.ForeColor = System.Drawing.Color.MediumBlue
        Me.btn_Filter_Show.Location = New System.Drawing.Point(396, 33)
        Me.btn_Filter_Show.Name = "btn_Filter_Show"
        Me.btn_Filter_Show.Size = New System.Drawing.Size(52, 68)
        Me.btn_Filter_Show.TabIndex = 8
        Me.btn_Filter_Show.Text = "Show"
        Me.btn_Filter_Show.UseVisualStyleBackColor = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(12, 79)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(114, 19)
        Me.Label13.TabIndex = 7
        Me.Label13.Text = "Employee Name"
        '
        'cbo_Filter_EmpName
        '
        Me.cbo_Filter_EmpName.DropDownHeight = 80
        Me.cbo_Filter_EmpName.DropDownWidth = 200
        Me.cbo_Filter_EmpName.FormattingEnabled = True
        Me.cbo_Filter_EmpName.IntegralHeight = False
        Me.cbo_Filter_EmpName.Location = New System.Drawing.Point(128, 75)
        Me.cbo_Filter_EmpName.Name = "cbo_Filter_EmpName"
        Me.cbo_Filter_EmpName.Size = New System.Drawing.Size(262, 27)
        Me.cbo_Filter_EmpName.TabIndex = 6
        '
        'dtp_Filter_Todate
        '
        Me.dtp_Filter_Todate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Filter_Todate.Location = New System.Drawing.Point(281, 33)
        Me.dtp_Filter_Todate.Name = "dtp_Filter_Todate"
        Me.dtp_Filter_Todate.Size = New System.Drawing.Size(109, 27)
        Me.dtp_Filter_Todate.TabIndex = 5
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(251, 37)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(24, 19)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "To"
        '
        'dtp_Filter_Fromdate
        '
        Me.dtp_Filter_Fromdate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Filter_Fromdate.Location = New System.Drawing.Point(128, 33)
        Me.dtp_Filter_Fromdate.Name = "dtp_Filter_Fromdate"
        Me.dtp_Filter_Fromdate.Size = New System.Drawing.Size(109, 27)
        Me.dtp_Filter_Fromdate.TabIndex = 3
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(12, 37)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(40, 19)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Date"
        '
        'dgv_Filter_Details
        '
        Me.dgv_Filter_Details.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter_Details.ColumnHeadersHeight = 30
        Me.dgv_Filter_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Filter_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6})
        Me.dgv_Filter_Details.Location = New System.Drawing.Point(14, 122)
        Me.dgv_Filter_Details.MultiSelect = False
        Me.dgv_Filter_Details.Name = "dgv_Filter_Details"
        Me.dgv_Filter_Details.ReadOnly = True
        Me.dgv_Filter_Details.RowHeadersVisible = False
        Me.dgv_Filter_Details.RowHeadersWidth = 40
        Me.dgv_Filter_Details.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_Filter_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter_Details.Size = New System.Drawing.Size(514, 160)
        Me.dgv_Filter_Details.TabIndex = 1
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.MediumBlue
        Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label10.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label10.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Label10.Location = New System.Drawing.Point(0, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(547, 23)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Filter"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Column1
        '
        Me.Column1.HeaderText = "Ref No."
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 50
        '
        'Column2
        '
        Me.Column2.HeaderText = "Employee Name"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 150
        '
        'Column3
        '
        Me.Column3.HeaderText = "Ends"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 70
        '
        'Column4
        '
        Me.Column4.HeaderText = "Bobins"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Width = 70
        '
        'Column5
        '
        Me.Column5.HeaderText = "Meters"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Width = 70
        '
        'Column6
        '
        Me.Column6.HeaderText = "Reel"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        Me.Column6.Width = 80
        '
        'Warping_Bobin_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(541, 346)
        Me.Controls.Add(Me.pnl_Filter)
        Me.Controls.Add(Me.lbl_company)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Pnl_back)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Warping_Bobin_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Warping_Bobin_Entry"
        Me.Pnl_back.ResumeLayout(False)
        Me.Pnl_back.PerformLayout()
        Me.pnl_Filter.ResumeLayout(False)
        Me.pnl_Filter.PerformLayout()
        CType(Me.dgv_Filter_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Pnl_back As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbl_RefNo As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txt_ends As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txt_Bobin As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txt_meters As System.Windows.Forms.TextBox
    Friend WithEvents cbo_empName As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents msk_date As System.Windows.Forms.MaskedTextBox
    Friend WithEvents dtp_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_company As System.Windows.Forms.Label
    Friend WithEvents pnl_Filter As System.Windows.Forms.Panel
    Friend WithEvents btn_Filter_Show As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cbo_Filter_EmpName As System.Windows.Forms.ComboBox
    Friend WithEvents dtp_Filter_Todate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents dtp_Filter_Fromdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents dgv_Filter_Details As System.Windows.Forms.DataGridView
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents btn_filter_close As System.Windows.Forms.Button
    Friend WithEvents lbl_Reel As System.Windows.Forms.Label
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
