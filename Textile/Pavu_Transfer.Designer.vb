<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Pavu_Transfer
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnl_Filter = New System.Windows.Forms.Panel()
        Me.btn_Filter_Close = New System.Windows.Forms.Button()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.btn_Filter_Show = New System.Windows.Forms.Button()
        Me.dgv_Filter_Details = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column17 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cbo_Filter_Party = New System.Windows.Forms.ComboBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.dtp_Filter_ToDate = New System.Windows.Forms.DateTimePicker()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.dtp_Filter_Fromdate = New System.Windows.Forms.DateTimePicker()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cbo_EndscountTo = New System.Windows.Forms.ComboBox()
        Me.cbo_EndsCountFrom = New System.Windows.Forms.ComboBox()
        Me.txt_MetersFrom = New System.Windows.Forms.TextBox()
        Me.txt_MetersTo = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.cbo_PartyFrom = New System.Windows.Forms.ComboBox()
        Me.lbl_RefNo = New System.Windows.Forms.Label()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.lbl_Sales_OrderNo_To = New System.Windows.Forms.Label()
        Me.cbo_ClothSales_OrderCode_forSelection_To = New System.Windows.Forms.ComboBox()
        Me.lbl_Sales_OrderNo_From = New System.Windows.Forms.Label()
        Me.cbo_ClothSales_OrderCode_forSelection_From = New System.Windows.Forms.ComboBox()
        Me.txt_remarks = New System.Windows.Forms.TextBox()
        Me.lbl_remarks = New System.Windows.Forms.Label()
        Me.cbo_Sizing_JobCardNo = New System.Windows.Forms.ComboBox()
        Me.lbl_Sizing_jobcardno_Caption = New System.Windows.Forms.Label()
        Me.cbo_weaving_job_no = New System.Windows.Forms.ComboBox()
        Me.lbl_weaving_job_no = New System.Windows.Forms.Label()
        Me.btn_UserModification = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.msk_Date = New System.Windows.Forms.MaskedTextBox()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.btn_Print = New System.Windows.Forms.Button()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.cbo_PartyTo = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lbl_UserName = New System.Windows.Forms.Label()
        Me.pnl_Filter.SuspendLayout()
        CType(Me.dgv_Filter_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Filter
        '
        Me.pnl_Filter.BackColor = System.Drawing.Color.White
        Me.pnl_Filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Filter.Controls.Add(Me.btn_Filter_Close)
        Me.pnl_Filter.Controls.Add(Me.Label29)
        Me.pnl_Filter.Controls.Add(Me.btn_Filter_Show)
        Me.pnl_Filter.Controls.Add(Me.dgv_Filter_Details)
        Me.pnl_Filter.Controls.Add(Me.cbo_Filter_Party)
        Me.pnl_Filter.Controls.Add(Me.Label32)
        Me.pnl_Filter.Controls.Add(Me.dtp_Filter_ToDate)
        Me.pnl_Filter.Controls.Add(Me.Label31)
        Me.pnl_Filter.Controls.Add(Me.dtp_Filter_Fromdate)
        Me.pnl_Filter.Controls.Add(Me.Label30)
        Me.pnl_Filter.Controls.Add(Me.Label34)
        Me.pnl_Filter.Location = New System.Drawing.Point(61, 446)
        Me.pnl_Filter.Margin = New System.Windows.Forms.Padding(0)
        Me.pnl_Filter.Name = "pnl_Filter"
        Me.pnl_Filter.Size = New System.Drawing.Size(578, 267)
        Me.pnl_Filter.TabIndex = 25
        '
        'btn_Filter_Close
        '
        Me.btn_Filter_Close.BackColor = System.Drawing.Color.White
        Me.btn_Filter_Close.BackgroundImage = Global.Textile.My.Resources.Resources.Close1
        Me.btn_Filter_Close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Filter_Close.FlatAppearance.BorderSize = 0
        Me.btn_Filter_Close.Location = New System.Drawing.Point(537, -1)
        Me.btn_Filter_Close.Name = "btn_Filter_Close"
        Me.btn_Filter_Close.Size = New System.Drawing.Size(40, 38)
        Me.btn_Filter_Close.TabIndex = 40
        Me.btn_Filter_Close.UseVisualStyleBackColor = True
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.BackColor = System.Drawing.Color.Purple
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ForeColor = System.Drawing.Color.White
        Me.Label29.Location = New System.Drawing.Point(638, -55)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(71, 20)
        Me.Label29.TabIndex = 37
        Me.Label29.Text = "FILTER"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btn_Filter_Show
        '
        Me.btn_Filter_Show.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Filter_Show.ForeColor = System.Drawing.Color.Blue
        Me.btn_Filter_Show.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Filter_Show.Location = New System.Drawing.Point(478, 50)
        Me.btn_Filter_Show.Name = "btn_Filter_Show"
        Me.btn_Filter_Show.Size = New System.Drawing.Size(76, 59)
        Me.btn_Filter_Show.TabIndex = 30
        Me.btn_Filter_Show.Text = "&SHOW"
        Me.btn_Filter_Show.UseVisualStyleBackColor = False
        '
        'dgv_Filter_Details
        '
        Me.dgv_Filter_Details.AllowUserToAddRows = False
        Me.dgv_Filter_Details.AllowUserToDeleteRows = False
        Me.dgv_Filter_Details.AllowUserToResizeColumns = False
        Me.dgv_Filter_Details.AllowUserToResizeRows = False
        Me.dgv_Filter_Details.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Filter_Details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_Filter_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Filter_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.Column17, Me.Column10, Me.Column1})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Filter_Details.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgv_Filter_Details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter_Details.Location = New System.Drawing.Point(-1, 122)
        Me.dgv_Filter_Details.MultiSelect = False
        Me.dgv_Filter_Details.Name = "dgv_Filter_Details"
        Me.dgv_Filter_Details.ReadOnly = True
        Me.dgv_Filter_Details.RowHeadersVisible = False
        Me.dgv_Filter_Details.RowHeadersWidth = 15
        Me.dgv_Filter_Details.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_Filter_Details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter_Details.Size = New System.Drawing.Size(578, 144)
        Me.dgv_Filter_Details.TabIndex = 32
        Me.dgv_Filter_Details.TabStop = False
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn1.Frozen = True
        Me.DataGridViewTextBoxColumn1.HeaderText = "REF.NO"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 50
        '
        'DataGridViewTextBoxColumn2
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewTextBoxColumn2.HeaderText = "DATE"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 70
        '
        'Column17
        '
        Me.Column17.HeaderText = "PARTY NAME"
        Me.Column17.Name = "Column17"
        Me.Column17.ReadOnly = True
        Me.Column17.Width = 200
        '
        'Column10
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column10.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column10.HeaderText = "METERS FROM"
        Me.Column10.Name = "Column10"
        Me.Column10.ReadOnly = True
        Me.Column10.Width = 120
        '
        'Column1
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column1.HeaderText = "METERS TO"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 115
        '
        'cbo_Filter_Party
        '
        Me.cbo_Filter_Party.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Filter_Party.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Filter_Party.FormattingEnabled = True
        Me.cbo_Filter_Party.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.cbo_Filter_Party.Location = New System.Drawing.Point(88, 86)
        Me.cbo_Filter_Party.MaxDropDownItems = 15
        Me.cbo_Filter_Party.Name = "cbo_Filter_Party"
        Me.cbo_Filter_Party.Size = New System.Drawing.Size(349, 23)
        Me.cbo_Filter_Party.Sorted = True
        Me.cbo_Filter_Party.TabIndex = 29
        Me.cbo_Filter_Party.Text = "cbo_Filter_Party"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.ForeColor = System.Drawing.Color.Blue
        Me.Label32.Location = New System.Drawing.Point(4, 90)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(72, 13)
        Me.Label32.TabIndex = 30
        Me.Label32.Text = "Party Name"
        '
        'dtp_Filter_ToDate
        '
        Me.dtp_Filter_ToDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Filter_ToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Filter_ToDate.Location = New System.Drawing.Point(306, 48)
        Me.dtp_Filter_ToDate.Name = "dtp_Filter_ToDate"
        Me.dtp_Filter_ToDate.Size = New System.Drawing.Size(131, 23)
        Me.dtp_Filter_ToDate.TabIndex = 27
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.ForeColor = System.Drawing.Color.Blue
        Me.Label31.Location = New System.Drawing.Point(260, 54)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(22, 13)
        Me.Label31.TabIndex = 29
        Me.Label31.Text = "To"
        '
        'dtp_Filter_Fromdate
        '
        Me.dtp_Filter_Fromdate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Filter_Fromdate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Filter_Fromdate.Location = New System.Drawing.Point(88, 49)
        Me.dtp_Filter_Fromdate.Name = "dtp_Filter_Fromdate"
        Me.dtp_Filter_Fromdate.Size = New System.Drawing.Size(146, 23)
        Me.dtp_Filter_Fromdate.TabIndex = 26
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.ForeColor = System.Drawing.Color.Blue
        Me.Label30.Location = New System.Drawing.Point(1, 52)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(38, 13)
        Me.Label30.TabIndex = 27
        Me.Label30.Text = " Date"
        '
        'Label34
        '
        Me.Label34.BackColor = System.Drawing.Color.Indigo
        Me.Label34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label34.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label34.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.ForeColor = System.Drawing.Color.White
        Me.Label34.Location = New System.Drawing.Point(0, 0)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(576, 38)
        Me.Label34.TabIndex = 41
        Me.Label34.Text = "FILTER"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(631, 35)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "PAVU TRANSFER"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(321, 133)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(80, 15)
        Me.Label8.TabIndex = 60
        Me.Label8.Text = "EndsCount To"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.Blue
        Me.Label7.Location = New System.Drawing.Point(14, 133)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(97, 15)
        Me.Label7.TabIndex = 59
        Me.Label7.Text = "EndsCount From"
        '
        'cbo_EndscountTo
        '
        Me.cbo_EndscountTo.FormattingEnabled = True
        Me.cbo_EndscountTo.Location = New System.Drawing.Point(416, 129)
        Me.cbo_EndscountTo.Name = "cbo_EndscountTo"
        Me.cbo_EndscountTo.Size = New System.Drawing.Size(181, 23)
        Me.cbo_EndscountTo.TabIndex = 4
        '
        'cbo_EndsCountFrom
        '
        Me.cbo_EndsCountFrom.FormattingEnabled = True
        Me.cbo_EndsCountFrom.Location = New System.Drawing.Point(117, 129)
        Me.cbo_EndsCountFrom.Name = "cbo_EndsCountFrom"
        Me.cbo_EndsCountFrom.Size = New System.Drawing.Size(198, 23)
        Me.cbo_EndsCountFrom.TabIndex = 3
        '
        'txt_MetersFrom
        '
        Me.txt_MetersFrom.Location = New System.Drawing.Point(117, 166)
        Me.txt_MetersFrom.MaxLength = 15
        Me.txt_MetersFrom.Name = "txt_MetersFrom"
        Me.txt_MetersFrom.Size = New System.Drawing.Size(198, 23)
        Me.txt_MetersFrom.TabIndex = 5
        '
        'txt_MetersTo
        '
        Me.txt_MetersTo.Location = New System.Drawing.Point(416, 166)
        Me.txt_MetersTo.MaxLength = 15
        Me.txt_MetersTo.Name = "txt_MetersTo"
        Me.txt_MetersTo.Size = New System.Drawing.Size(181, 23)
        Me.txt_MetersTo.TabIndex = 6
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(16, 129)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(0, 15)
        Me.Label6.TabIndex = 50
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Blue
        Me.Label12.Location = New System.Drawing.Point(14, 170)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(79, 15)
        Me.Label12.TabIndex = 44
        Me.Label12.Text = "Meters From"
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Lime
        Me.lbl_Company.Location = New System.Drawing.Point(123, 12)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(67, 13)
        Me.lbl_Company.TabIndex = 33
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'cbo_PartyFrom
        '
        Me.cbo_PartyFrom.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_PartyFrom.FormattingEnabled = True
        Me.cbo_PartyFrom.Location = New System.Drawing.Point(117, 55)
        Me.cbo_PartyFrom.MaxDropDownItems = 15
        Me.cbo_PartyFrom.MaxLength = 50
        Me.cbo_PartyFrom.Name = "cbo_PartyFrom"
        Me.cbo_PartyFrom.Size = New System.Drawing.Size(480, 23)
        Me.cbo_PartyFrom.Sorted = True
        Me.cbo_PartyFrom.TabIndex = 1
        '
        'lbl_RefNo
        '
        Me.lbl_RefNo.BackColor = System.Drawing.Color.White
        Me.lbl_RefNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_RefNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RefNo.Location = New System.Drawing.Point(117, 16)
        Me.lbl_RefNo.Name = "lbl_RefNo"
        Me.lbl_RefNo.Size = New System.Drawing.Size(198, 23)
        Me.lbl_RefNo.TabIndex = 0
        Me.lbl_RefNo.Text = "lbl_RefNo"
        Me.lbl_RefNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnl_Back
        '
        Me.pnl_Back.AutoScroll = True
        Me.pnl_Back.AutoSize = True
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.lbl_Sales_OrderNo_To)
        Me.pnl_Back.Controls.Add(Me.cbo_ClothSales_OrderCode_forSelection_To)
        Me.pnl_Back.Controls.Add(Me.lbl_Sales_OrderNo_From)
        Me.pnl_Back.Controls.Add(Me.cbo_ClothSales_OrderCode_forSelection_From)
        Me.pnl_Back.Controls.Add(Me.txt_remarks)
        Me.pnl_Back.Controls.Add(Me.lbl_remarks)
        Me.pnl_Back.Controls.Add(Me.cbo_Sizing_JobCardNo)
        Me.pnl_Back.Controls.Add(Me.lbl_Sizing_jobcardno_Caption)
        Me.pnl_Back.Controls.Add(Me.cbo_weaving_job_no)
        Me.pnl_Back.Controls.Add(Me.lbl_weaving_job_no)
        Me.pnl_Back.Controls.Add(Me.btn_UserModification)
        Me.pnl_Back.Controls.Add(Me.Label11)
        Me.pnl_Back.Controls.Add(Me.Label10)
        Me.pnl_Back.Controls.Add(Me.Label9)
        Me.pnl_Back.Controls.Add(Me.Label14)
        Me.pnl_Back.Controls.Add(Me.Label23)
        Me.pnl_Back.Controls.Add(Me.msk_Date)
        Me.pnl_Back.Controls.Add(Me.dtp_Date)
        Me.pnl_Back.Controls.Add(Me.btn_Print)
        Me.pnl_Back.Controls.Add(Me.btn_close)
        Me.pnl_Back.Controls.Add(Me.btn_save)
        Me.pnl_Back.Controls.Add(Me.Label8)
        Me.pnl_Back.Controls.Add(Me.Label7)
        Me.pnl_Back.Controls.Add(Me.cbo_EndscountTo)
        Me.pnl_Back.Controls.Add(Me.cbo_EndsCountFrom)
        Me.pnl_Back.Controls.Add(Me.txt_MetersFrom)
        Me.pnl_Back.Controls.Add(Me.txt_MetersTo)
        Me.pnl_Back.Controls.Add(Me.Label6)
        Me.pnl_Back.Controls.Add(Me.Label12)
        Me.pnl_Back.Controls.Add(Me.lbl_RefNo)
        Me.pnl_Back.Controls.Add(Me.cbo_PartyTo)
        Me.pnl_Back.Controls.Add(Me.cbo_PartyFrom)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.Label3)
        Me.pnl_Back.Controls.Add(Me.Label13)
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Enabled = False
        Me.pnl_Back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_Back.Location = New System.Drawing.Point(8, 43)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(616, 358)
        Me.pnl_Back.TabIndex = 23
        '
        'lbl_Sales_OrderNo_To
        '
        Me.lbl_Sales_OrderNo_To.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Sales_OrderNo_To.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Sales_OrderNo_To.Location = New System.Drawing.Point(321, 237)
        Me.lbl_Sales_OrderNo_To.Name = "lbl_Sales_OrderNo_To"
        Me.lbl_Sales_OrderNo_To.Size = New System.Drawing.Size(89, 30)
        Me.lbl_Sales_OrderNo_To.TabIndex = 1503
        Me.lbl_Sales_OrderNo_To.Text = "Sales Order No To"
        Me.lbl_Sales_OrderNo_To.Visible = False
        '
        'cbo_ClothSales_OrderCode_forSelection_To
        '
        Me.cbo_ClothSales_OrderCode_forSelection_To.BackColor = System.Drawing.Color.White
        Me.cbo_ClothSales_OrderCode_forSelection_To.DropDownHeight = 150
        Me.cbo_ClothSales_OrderCode_forSelection_To.DropDownWidth = 150
        Me.cbo_ClothSales_OrderCode_forSelection_To.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_ClothSales_OrderCode_forSelection_To.FormattingEnabled = True
        Me.cbo_ClothSales_OrderCode_forSelection_To.IntegralHeight = False
        Me.cbo_ClothSales_OrderCode_forSelection_To.Location = New System.Drawing.Point(416, 241)
        Me.cbo_ClothSales_OrderCode_forSelection_To.MaxDropDownItems = 15
        Me.cbo_ClothSales_OrderCode_forSelection_To.MaxLength = 50
        Me.cbo_ClothSales_OrderCode_forSelection_To.Name = "cbo_ClothSales_OrderCode_forSelection_To"
        Me.cbo_ClothSales_OrderCode_forSelection_To.Size = New System.Drawing.Size(181, 23)
        Me.cbo_ClothSales_OrderCode_forSelection_To.TabIndex = 1504
        Me.cbo_ClothSales_OrderCode_forSelection_To.Text = "Sales_OrderNo_To"
        Me.cbo_ClothSales_OrderCode_forSelection_To.Visible = False
        '
        'lbl_Sales_OrderNo_From
        '
        Me.lbl_Sales_OrderNo_From.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Sales_OrderNo_From.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Sales_OrderNo_From.Location = New System.Drawing.Point(13, 237)
        Me.lbl_Sales_OrderNo_From.Name = "lbl_Sales_OrderNo_From"
        Me.lbl_Sales_OrderNo_From.Size = New System.Drawing.Size(89, 30)
        Me.lbl_Sales_OrderNo_From.TabIndex = 1501
        Me.lbl_Sales_OrderNo_From.Text = "Sales Order No From"
        Me.lbl_Sales_OrderNo_From.Visible = False
        '
        'cbo_ClothSales_OrderCode_forSelection_From
        '
        Me.cbo_ClothSales_OrderCode_forSelection_From.BackColor = System.Drawing.Color.White
        Me.cbo_ClothSales_OrderCode_forSelection_From.DropDownHeight = 150
        Me.cbo_ClothSales_OrderCode_forSelection_From.DropDownWidth = 150
        Me.cbo_ClothSales_OrderCode_forSelection_From.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_ClothSales_OrderCode_forSelection_From.FormattingEnabled = True
        Me.cbo_ClothSales_OrderCode_forSelection_From.IntegralHeight = False
        Me.cbo_ClothSales_OrderCode_forSelection_From.Location = New System.Drawing.Point(117, 241)
        Me.cbo_ClothSales_OrderCode_forSelection_From.MaxDropDownItems = 15
        Me.cbo_ClothSales_OrderCode_forSelection_From.MaxLength = 50
        Me.cbo_ClothSales_OrderCode_forSelection_From.Name = "cbo_ClothSales_OrderCode_forSelection_From"
        Me.cbo_ClothSales_OrderCode_forSelection_From.Size = New System.Drawing.Size(198, 23)
        Me.cbo_ClothSales_OrderCode_forSelection_From.TabIndex = 1502
        Me.cbo_ClothSales_OrderCode_forSelection_From.Text = "Sales_OrderNo_From"
        Me.cbo_ClothSales_OrderCode_forSelection_From.Visible = False
        '
        'txt_remarks
        '
        Me.txt_remarks.Location = New System.Drawing.Point(117, 277)
        Me.txt_remarks.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txt_remarks.MaxLength = 100
        Me.txt_remarks.Name = "txt_remarks"
        Me.txt_remarks.Size = New System.Drawing.Size(480, 23)
        Me.txt_remarks.TabIndex = 9
        Me.txt_remarks.Text = "txt_Remarks"
        '
        'lbl_remarks
        '
        Me.lbl_remarks.AutoSize = True
        Me.lbl_remarks.ForeColor = System.Drawing.Color.Blue
        Me.lbl_remarks.Location = New System.Drawing.Point(13, 281)
        Me.lbl_remarks.Name = "lbl_remarks"
        Me.lbl_remarks.Size = New System.Drawing.Size(54, 15)
        Me.lbl_remarks.TabIndex = 1233
        Me.lbl_remarks.Text = "Remarks"
        '
        'cbo_Sizing_JobCardNo
        '
        Me.cbo_Sizing_JobCardNo.BackColor = System.Drawing.Color.Yellow
        Me.cbo_Sizing_JobCardNo.DropDownHeight = 350
        Me.cbo_Sizing_JobCardNo.DropDownWidth = 350
        Me.cbo_Sizing_JobCardNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Sizing_JobCardNo.FormattingEnabled = True
        Me.cbo_Sizing_JobCardNo.IntegralHeight = False
        Me.cbo_Sizing_JobCardNo.Location = New System.Drawing.Point(416, 203)
        Me.cbo_Sizing_JobCardNo.MaxDropDownItems = 15
        Me.cbo_Sizing_JobCardNo.MaxLength = 50
        Me.cbo_Sizing_JobCardNo.Name = "cbo_Sizing_JobCardNo"
        Me.cbo_Sizing_JobCardNo.Size = New System.Drawing.Size(181, 23)
        Me.cbo_Sizing_JobCardNo.Sorted = True
        Me.cbo_Sizing_JobCardNo.TabIndex = 8
        Me.cbo_Sizing_JobCardNo.Visible = False
        '
        'lbl_Sizing_jobcardno_Caption
        '
        Me.lbl_Sizing_jobcardno_Caption.AutoSize = True
        Me.lbl_Sizing_jobcardno_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Sizing_jobcardno_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Sizing_jobcardno_Caption.Location = New System.Drawing.Point(321, 211)
        Me.lbl_Sizing_jobcardno_Caption.Name = "lbl_Sizing_jobcardno_Caption"
        Me.lbl_Sizing_jobcardno_Caption.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Sizing_jobcardno_Caption.TabIndex = 1232
        Me.lbl_Sizing_jobcardno_Caption.Text = "Sizing Job No"
        Me.lbl_Sizing_jobcardno_Caption.Visible = False
        '
        'cbo_weaving_job_no
        '
        Me.cbo_weaving_job_no.BackColor = System.Drawing.Color.Yellow
        Me.cbo_weaving_job_no.DropDownHeight = 110
        Me.cbo_weaving_job_no.DropDownWidth = 155
        Me.cbo_weaving_job_no.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_weaving_job_no.FormattingEnabled = True
        Me.cbo_weaving_job_no.IntegralHeight = False
        Me.cbo_weaving_job_no.Location = New System.Drawing.Point(117, 203)
        Me.cbo_weaving_job_no.MaxDropDownItems = 15
        Me.cbo_weaving_job_no.MaxLength = 50
        Me.cbo_weaving_job_no.Name = "cbo_weaving_job_no"
        Me.cbo_weaving_job_no.Size = New System.Drawing.Size(198, 23)
        Me.cbo_weaving_job_no.Sorted = True
        Me.cbo_weaving_job_no.TabIndex = 7
        Me.cbo_weaving_job_no.Visible = False
        '
        'lbl_weaving_job_no
        '
        Me.lbl_weaving_job_no.AutoSize = True
        Me.lbl_weaving_job_no.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_weaving_job_no.ForeColor = System.Drawing.Color.Blue
        Me.lbl_weaving_job_no.Location = New System.Drawing.Point(13, 207)
        Me.lbl_weaving_job_no.Name = "lbl_weaving_job_no"
        Me.lbl_weaving_job_no.Size = New System.Drawing.Size(89, 15)
        Me.lbl_weaving_job_no.TabIndex = 1231
        Me.lbl_weaving_job_no.Text = "Weaver job No"
        Me.lbl_weaving_job_no.Visible = False
        '
        'btn_UserModification
        '
        Me.btn_UserModification.BackColor = System.Drawing.Color.OrangeRed
        Me.btn_UserModification.ForeColor = System.Drawing.Color.White
        Me.btn_UserModification.Location = New System.Drawing.Point(16, 320)
        Me.btn_UserModification.Name = "btn_UserModification"
        Me.btn_UserModification.Size = New System.Drawing.Size(103, 25)
        Me.btn_UserModification.TabIndex = 1177
        Me.btn_UserModification.TabStop = False
        Me.btn_UserModification.Text = "MODIFICATION"
        Me.btn_UserModification.UseVisualStyleBackColor = False
        Me.btn_UserModification.Visible = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Red
        Me.Label11.Location = New System.Drawing.Point(350, 20)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(13, 15)
        Me.Label11.TabIndex = 304
        Me.Label11.Text = "*"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(379, 167)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(13, 15)
        Me.Label10.TabIndex = 303
        Me.Label10.Text = "*"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Red
        Me.Label9.Location = New System.Drawing.Point(89, 167)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(13, 15)
        Me.Label9.TabIndex = 302
        Me.Label9.Text = "*"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Red
        Me.Label14.Location = New System.Drawing.Point(61, 92)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(13, 15)
        Me.Label14.TabIndex = 302
        Me.Label14.Text = "*"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Red
        Me.Label23.Location = New System.Drawing.Point(78, 55)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(13, 15)
        Me.Label23.TabIndex = 302
        Me.Label23.Text = "*"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'msk_Date
        '
        Me.msk_Date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_Date.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_Date.Location = New System.Drawing.Point(416, 16)
        Me.msk_Date.Mask = "00-00-0000"
        Me.msk_Date.Name = "msk_Date"
        Me.msk_Date.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_Date.Size = New System.Drawing.Size(164, 22)
        Me.msk_Date.TabIndex = 0
        '
        'dtp_Date
        '
        Me.dtp_Date.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Date.Location = New System.Drawing.Point(579, 16)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(18, 22)
        Me.dtp_Date.TabIndex = 278
        Me.dtp_Date.TabStop = False
        '
        'btn_Print
        '
        Me.btn_Print.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_Print.ForeColor = System.Drawing.Color.White
        Me.btn_Print.Location = New System.Drawing.Point(434, 306)
        Me.btn_Print.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_Print.Name = "btn_Print"
        Me.btn_Print.Size = New System.Drawing.Size(71, 30)
        Me.btn_Print.TabIndex = 11
        Me.btn_Print.TabStop = False
        Me.btn_Print.Text = "&PRINT"
        Me.btn_Print.UseVisualStyleBackColor = False
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(527, 306)
        Me.btn_close.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(71, 30)
        Me.btn_close.TabIndex = 12
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(339, 306)
        Me.btn_save.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(71, 30)
        Me.btn_save.TabIndex = 10
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'cbo_PartyTo
        '
        Me.cbo_PartyTo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_PartyTo.FormattingEnabled = True
        Me.cbo_PartyTo.Location = New System.Drawing.Point(117, 92)
        Me.cbo_PartyTo.MaxDropDownItems = 15
        Me.cbo_PartyTo.MaxLength = 50
        Me.cbo_PartyTo.Name = "cbo_PartyTo"
        Me.cbo_PartyTo.Size = New System.Drawing.Size(480, 23)
        Me.cbo_PartyTo.Sorted = True
        Me.cbo_PartyTo.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(321, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(33, 15)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Date"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(321, 170)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 15)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Meters To"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.Blue
        Me.Label13.Location = New System.Drawing.Point(14, 96)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(51, 15)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "Party To"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(14, 59)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(68, 15)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Party From"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(14, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Ref.No"
        '
        'lbl_UserName
        '
        Me.lbl_UserName.AutoSize = True
        Me.lbl_UserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(55, Byte), Integer), CType(CType(91, Byte), Integer))
        Me.lbl_UserName.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_UserName.ForeColor = System.Drawing.Color.White
        Me.lbl_UserName.Location = New System.Drawing.Point(514, 9)
        Me.lbl_UserName.Name = "lbl_UserName"
        Me.lbl_UserName.Size = New System.Drawing.Size(105, 19)
        Me.lbl_UserName.TabIndex = 267
        Me.lbl_UserName.Text = "USER : ADMIN"
        '
        'Pavu_Transfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(631, 410)
        Me.Controls.Add(Me.lbl_UserName)
        Me.Controls.Add(Me.pnl_Filter)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.lbl_Company)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Pavu_Transfer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PAVU TRANSFER"
        Me.pnl_Filter.ResumeLayout(False)
        Me.pnl_Filter.PerformLayout()
        CType(Me.dgv_Filter_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnl_Filter As System.Windows.Forms.Panel
    Friend WithEvents btn_Filter_Close As System.Windows.Forms.Button
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents btn_Filter_Show As System.Windows.Forms.Button
    Friend WithEvents dgv_Filter_Details As System.Windows.Forms.DataGridView
    Friend WithEvents dtp_Filter_ToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents dtp_Filter_Fromdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cbo_EndscountTo As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_EndsCountFrom As System.Windows.Forms.ComboBox
    Friend WithEvents txt_MetersFrom As System.Windows.Forms.TextBox
    Friend WithEvents txt_MetersTo As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents cbo_PartyFrom As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_RefNo As System.Windows.Forms.Label
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbo_Filter_Party As System.Windows.Forms.ComboBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents btn_Print As System.Windows.Forms.Button
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column17 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lbl_UserName As System.Windows.Forms.Label
    Friend WithEvents msk_Date As System.Windows.Forms.MaskedTextBox
    Friend WithEvents dtp_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents cbo_PartyTo As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents btn_UserModification As System.Windows.Forms.Button
    Friend WithEvents cbo_Sizing_JobCardNo As ComboBox
    Friend WithEvents lbl_Sizing_jobcardno_Caption As Label
    Friend WithEvents cbo_weaving_job_no As ComboBox
    Friend WithEvents lbl_weaving_job_no As Label
    Friend WithEvents txt_remarks As TextBox
    Friend WithEvents lbl_remarks As Label
    Friend WithEvents lbl_Sales_OrderNo_From As Label
    Friend WithEvents cbo_ClothSales_OrderCode_forSelection_From As ComboBox
    Friend WithEvents lbl_Sales_OrderNo_To As Label
    Friend WithEvents cbo_ClothSales_OrderCode_forSelection_To As ComboBox
End Class
