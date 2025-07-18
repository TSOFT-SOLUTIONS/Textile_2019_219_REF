<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Empty_Bag_Receipt
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
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.msk_date = New System.Windows.Forms.MaskedTextBox()
        Me.cbo_Filter_PartyName = New System.Windows.Forms.ComboBox()
        Me.btn_closefilter = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btn_filtershow = New System.Windows.Forms.Button()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.dc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btn_Print = New System.Windows.Forms.Button()
        Me.dtp_FilterTo_date = New System.Windows.Forms.DateTimePicker()
        Me.dtp_FilterFrom_date = New System.Windows.Forms.DateTimePicker()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.pnl_filter = New System.Windows.Forms.Panel()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txt_emptybags = New System.Windows.Forms.TextBox()
        Me.txt_Party_DcNo = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.txt_Book_No = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cbo_vehicleno = New System.Windows.Forms.ComboBox()
        Me.lbl_ReceiptNo = New System.Windows.Forms.Label()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_remarks = New System.Windows.Forms.TextBox()
        Me.cbo_PartyName = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.lbl_UserName = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_filter.SuspendLayout()
        Me.pnl_back.SuspendLayout()
        Me.SuspendLayout()
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'msk_date
        '
        Me.msk_date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_date.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_date.Location = New System.Drawing.Point(397, 23)
        Me.msk_date.Mask = "00-00-0000"
        Me.msk_date.Name = "msk_date"
        Me.msk_date.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_date.Size = New System.Drawing.Size(164, 22)
        Me.msk_date.TabIndex = 0
        '
        'cbo_Filter_PartyName
        '
        Me.cbo_Filter_PartyName.FormattingEnabled = True
        Me.cbo_Filter_PartyName.Location = New System.Drawing.Point(93, 82)
        Me.cbo_Filter_PartyName.Name = "cbo_Filter_PartyName"
        Me.cbo_Filter_PartyName.Size = New System.Drawing.Size(450, 23)
        Me.cbo_Filter_PartyName.TabIndex = 2
        Me.cbo_Filter_PartyName.Text = "cbo_Filter_PartyName"
        '
        'btn_closefilter
        '
        Me.btn_closefilter.Location = New System.Drawing.Point(456, 45)
        Me.btn_closefilter.Name = "btn_closefilter"
        Me.btn_closefilter.Size = New System.Drawing.Size(87, 27)
        Me.btn_closefilter.TabIndex = 4
        Me.btn_closefilter.Text = "&CLOSE"
        Me.btn_closefilter.UseVisualStyleBackColor = True
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.DimGray
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(560, 30)
        Me.Label16.TabIndex = 8
        Me.Label16.Text = "FILTER"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btn_filtershow
        '
        Me.btn_filtershow.Location = New System.Drawing.Point(353, 45)
        Me.btn_filtershow.Name = "btn_filtershow"
        Me.btn_filtershow.Size = New System.Drawing.Size(87, 27)
        Me.btn_filtershow.TabIndex = 3
        Me.btn_filtershow.Text = "SHOW"
        Me.btn_filtershow.UseVisualStyleBackColor = True
        '
        'dgv_filter
        '
        Me.dgv_filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dc, Me.Column1, Me.Column2, Me.Column5})
        Me.dgv_filter.Location = New System.Drawing.Point(12, 122)
        Me.dgv_filter.Name = "dgv_filter"
        Me.dgv_filter.RowHeadersVisible = False
        Me.dgv_filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_filter.Size = New System.Drawing.Size(531, 148)
        Me.dgv_filter.TabIndex = 5
        '
        'dc
        '
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dc.DefaultCellStyle = DataGridViewCellStyle9
        Me.dc.HeaderText = "Rec.No"
        Me.dc.MaxInputLength = 8
        Me.dc.Name = "dc"
        Me.dc.ReadOnly = True
        Me.dc.Width = 60
        '
        'Column1
        '
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle10
        Me.Column1.HeaderText = "Rec.Date"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 90
        '
        'Column2
        '
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle11
        Me.Column2.HeaderText = "PARTY NAME"
        Me.Column2.MaxInputLength = 35
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 250
        '
        'Column5
        '
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle12
        Me.Column5.HeaderText = "EMPTY BAG"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 110
        '
        'btn_Print
        '
        Me.btn_Print.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Print.ForeColor = System.Drawing.Color.White
        Me.btn_Print.Location = New System.Drawing.Point(419, 233)
        Me.btn_Print.Name = "btn_Print"
        Me.btn_Print.Size = New System.Drawing.Size(73, 30)
        Me.btn_Print.TabIndex = 14
        Me.btn_Print.TabStop = False
        Me.btn_Print.Text = "&PRINT"
        Me.btn_Print.UseVisualStyleBackColor = False
        '
        'dtp_FilterTo_date
        '
        Me.dtp_FilterTo_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterTo_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterTo_date.Location = New System.Drawing.Point(231, 47)
        Me.dtp_FilterTo_date.Name = "dtp_FilterTo_date"
        Me.dtp_FilterTo_date.Size = New System.Drawing.Size(102, 23)
        Me.dtp_FilterTo_date.TabIndex = 1
        '
        'dtp_FilterFrom_date
        '
        Me.dtp_FilterFrom_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterFrom_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterFrom_date.Location = New System.Drawing.Point(93, 47)
        Me.dtp_FilterFrom_date.Name = "dtp_FilterFrom_date"
        Me.dtp_FilterFrom_date.Size = New System.Drawing.Size(98, 23)
        Me.dtp_FilterFrom_date.TabIndex = 0
        '
        'PrintDocument1
        '
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(502, 233)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(73, 30)
        Me.btn_close.TabIndex = 15
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label11.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(0, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(617, 35)
        Me.Label11.TabIndex = 27
        Me.Label11.Text = "EMPTY BAG RECEIPT"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(12, 86)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(72, 15)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "Party Name"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(201, 51)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(19, 15)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "To"
        '
        'pnl_filter
        '
        Me.pnl_filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_filter.Controls.Add(Me.btn_closefilter)
        Me.pnl_filter.Controls.Add(Me.Label16)
        Me.pnl_filter.Controls.Add(Me.btn_filtershow)
        Me.pnl_filter.Controls.Add(Me.dgv_filter)
        Me.pnl_filter.Controls.Add(Me.cbo_Filter_PartyName)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterTo_date)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterFrom_date)
        Me.pnl_filter.Controls.Add(Me.Label15)
        Me.pnl_filter.Controls.Add(Me.Label14)
        Me.pnl_filter.Controls.Add(Me.Label13)
        Me.pnl_filter.Location = New System.Drawing.Point(20, 359)
        Me.pnl_filter.Name = "pnl_filter"
        Me.pnl_filter.Size = New System.Drawing.Size(562, 284)
        Me.pnl_filter.TabIndex = 28
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(12, 51)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(55, 15)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Rec.Date"
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(336, 233)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(73, 30)
        Me.btn_save.TabIndex = 13
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.ForeColor = System.Drawing.Color.Blue
        Me.Label18.Location = New System.Drawing.Point(10, 153)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(69, 15)
        Me.Label18.TabIndex = 30
        Me.Label18.Text = "Empty Bags"
        '
        'txt_emptybags
        '
        Me.txt_emptybags.Location = New System.Drawing.Point(111, 149)
        Me.txt_emptybags.MaxLength = 8
        Me.txt_emptybags.Name = "txt_emptybags"
        Me.txt_emptybags.Size = New System.Drawing.Size(167, 23)
        Me.txt_emptybags.TabIndex = 4
        '
        'txt_Party_DcNo
        '
        Me.txt_Party_DcNo.Location = New System.Drawing.Point(111, 107)
        Me.txt_Party_DcNo.MaxLength = 15
        Me.txt_Party_DcNo.Name = "txt_Party_DcNo"
        Me.txt_Party_DcNo.Size = New System.Drawing.Size(167, 23)
        Me.txt_Party_DcNo.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(10, 111)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(68, 15)
        Me.Label4.TabIndex = 25
        Me.Label4.Text = "Party DcNo"
        '
        'pnl_back
        '
        Me.pnl_back.AutoSize = True
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.Label12)
        Me.pnl_back.Controls.Add(Me.Label5)
        Me.pnl_back.Controls.Add(Me.msk_date)
        Me.pnl_back.Controls.Add(Me.btn_Print)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.txt_emptybags)
        Me.pnl_back.Controls.Add(Me.Label18)
        Me.pnl_back.Controls.Add(Me.txt_Party_DcNo)
        Me.pnl_back.Controls.Add(Me.Label4)
        Me.pnl_back.Controls.Add(Me.txt_Book_No)
        Me.pnl_back.Controls.Add(Me.Label10)
        Me.pnl_back.Controls.Add(Me.cbo_vehicleno)
        Me.pnl_back.Controls.Add(Me.lbl_ReceiptNo)
        Me.pnl_back.Controls.Add(Me.dtp_Date)
        Me.pnl_back.Controls.Add(Me.Label9)
        Me.pnl_back.Controls.Add(Me.Label8)
        Me.pnl_back.Controls.Add(Me.txt_remarks)
        Me.pnl_back.Controls.Add(Me.cbo_PartyName)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label6)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Location = New System.Drawing.Point(6, 42)
        Me.pnl_back.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(595, 288)
        Me.pnl_back.TabIndex = 26
        '
        'txt_Book_No
        '
        Me.txt_Book_No.Location = New System.Drawing.Point(397, 107)
        Me.txt_Book_No.MaxLength = 20
        Me.txt_Book_No.Name = "txt_Book_No"
        Me.txt_Book_No.Size = New System.Drawing.Size(178, 23)
        Me.txt_Book_No.TabIndex = 3
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Blue
        Me.Label10.Location = New System.Drawing.Point(304, 111)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(53, 15)
        Me.Label10.TabIndex = 27
        Me.Label10.Text = "Book No"
        '
        'cbo_vehicleno
        '
        Me.cbo_vehicleno.DropDownHeight = 125
        Me.cbo_vehicleno.FormattingEnabled = True
        Me.cbo_vehicleno.IntegralHeight = False
        Me.cbo_vehicleno.Location = New System.Drawing.Point(397, 149)
        Me.cbo_vehicleno.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbo_vehicleno.Name = "cbo_vehicleno"
        Me.cbo_vehicleno.Size = New System.Drawing.Size(178, 23)
        Me.cbo_vehicleno.TabIndex = 5
        '
        'lbl_ReceiptNo
        '
        Me.lbl_ReceiptNo.BackColor = System.Drawing.Color.White
        Me.lbl_ReceiptNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_ReceiptNo.Location = New System.Drawing.Point(111, 23)
        Me.lbl_ReceiptNo.Name = "lbl_ReceiptNo"
        Me.lbl_ReceiptNo.Size = New System.Drawing.Size(167, 23)
        Me.lbl_ReceiptNo.TabIndex = 21
        Me.lbl_ReceiptNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtp_Date
        '
        Me.dtp_Date.CustomFormat = ""
        Me.dtp_Date.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Date.Location = New System.Drawing.Point(557, 23)
        Me.dtp_Date.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(18, 22)
        Me.dtp_Date.TabIndex = 0
        Me.dtp_Date.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Blue
        Me.Label9.Location = New System.Drawing.Point(304, 153)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(65, 15)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "Vehicle No"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(10, 69)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(72, 15)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Party Name"
        '
        'txt_remarks
        '
        Me.txt_remarks.Location = New System.Drawing.Point(111, 191)
        Me.txt_remarks.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txt_remarks.MaxLength = 100
        Me.txt_remarks.Name = "txt_remarks"
        Me.txt_remarks.Size = New System.Drawing.Size(464, 23)
        Me.txt_remarks.TabIndex = 6
        '
        'cbo_PartyName
        '
        Me.cbo_PartyName.DropDownHeight = 175
        Me.cbo_PartyName.FormattingEnabled = True
        Me.cbo_PartyName.IntegralHeight = False
        Me.cbo_PartyName.Location = New System.Drawing.Point(111, 65)
        Me.cbo_PartyName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbo_PartyName.MaxLength = 35
        Me.cbo_PartyName.Name = "cbo_PartyName"
        Me.cbo_PartyName.Size = New System.Drawing.Size(464, 23)
        Me.cbo_PartyName.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(10, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Receipt No."
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 260)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(0, 15)
        Me.Label7.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(304, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Date"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(10, 194)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(54, 15)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Remarks"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 15)
        Me.Label3.TabIndex = 2
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Red
        Me.lbl_Company.ForeColor = System.Drawing.Color.White
        Me.lbl_Company.Location = New System.Drawing.Point(57, -79)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 29
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'lbl_UserName
        '
        Me.lbl_UserName.AutoSize = True
        Me.lbl_UserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(55, Byte), Integer), CType(CType(91, Byte), Integer))
        Me.lbl_UserName.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_UserName.ForeColor = System.Drawing.Color.White
        Me.lbl_UserName.Location = New System.Drawing.Point(501, 9)
        Me.lbl_UserName.Name = "lbl_UserName"
        Me.lbl_UserName.Size = New System.Drawing.Size(105, 19)
        Me.lbl_UserName.TabIndex = 270
        Me.lbl_UserName.Text = "USER : ADMIN"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(81, 69)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(13, 15)
        Me.Label5.TabIndex = 302
        Me.Label5.Text = "*"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Red
        Me.Label12.Location = New System.Drawing.Point(336, 27)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(13, 15)
        Me.Label12.TabIndex = 303
        Me.Label12.Text = "*"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Empty_Bag_Receipt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(617, 346)
        Me.Controls.Add(Me.lbl_UserName)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.pnl_filter)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.lbl_Company)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Empty_Bag_Receipt"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EMPTY BAG RECEIPT"
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_filter.ResumeLayout(False)
        Me.pnl_filter.PerformLayout()
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents msk_date As System.Windows.Forms.MaskedTextBox
    Friend WithEvents cbo_Filter_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents btn_closefilter As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btn_filtershow As System.Windows.Forms.Button
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_Print As System.Windows.Forms.Button
    Friend WithEvents dtp_FilterTo_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp_FilterFrom_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents pnl_filter As System.Windows.Forms.Panel
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txt_emptybags As System.Windows.Forms.TextBox
    Friend WithEvents txt_Party_DcNo As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents txt_Book_No As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cbo_vehicleno As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_ReceiptNo As System.Windows.Forms.Label
    Friend WithEvents dtp_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_remarks As System.Windows.Forms.TextBox
    Friend WithEvents cbo_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents dc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lbl_UserName As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
End Class
