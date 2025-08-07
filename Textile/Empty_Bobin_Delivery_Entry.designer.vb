<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Empty_Bobin_Delivery_Entry
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.cbo_vehicleno = New System.Windows.Forms.ComboBox()
        Me.lbl_DeliveryNo = New System.Windows.Forms.Label()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.cbo_Bobin_Size = New System.Windows.Forms.ComboBox()
        Me.msk_date = New System.Windows.Forms.MaskedTextBox()
        Me.txt_EmptyBobin = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbl_emptybobin_Caption = New System.Windows.Forms.Label()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.txt_Party_DcNo = New System.Windows.Forms.TextBox()
        Me.lbl_partyDc_caption = New System.Windows.Forms.Label()
        Me.txt_Book_No = New System.Windows.Forms.TextBox()
        Me.lbl_bookno_caption = New System.Windows.Forms.Label()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.lbl_vehicle_caption = New System.Windows.Forms.Label()
        Me.lbl_partynamecaptiion = New System.Windows.Forms.Label()
        Me.txt_remarks = New System.Windows.Forms.TextBox()
        Me.cbo_PartyName = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lbl_RemarksCaption = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btn_Print = New System.Windows.Forms.Button()
        Me.pnl_filter = New System.Windows.Forms.Panel()
        Me.btn_closefilter = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btn_filtershow = New System.Windows.Forms.Button()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.dc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cbo_Filter_PartyName = New System.Windows.Forms.ComboBox()
        Me.dtp_FilterTo_date = New System.Windows.Forms.DateTimePicker()
        Me.dtp_FilterFrom_date = New System.Windows.Forms.DateTimePicker()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.lbl_UserName = New System.Windows.Forms.Label()
        Me.btn_UserModification = New System.Windows.Forms.Button()
        Me.pnl_back.SuspendLayout()
        Me.pnl_filter.SuspendLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cbo_vehicleno
        '
        Me.cbo_vehicleno.DropDownHeight = 125
        Me.cbo_vehicleno.FormattingEnabled = True
        Me.cbo_vehicleno.IntegralHeight = False
        Me.cbo_vehicleno.Location = New System.Drawing.Point(399, 129)
        Me.cbo_vehicleno.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbo_vehicleno.Name = "cbo_vehicleno"
        Me.cbo_vehicleno.Size = New System.Drawing.Size(178, 23)
        Me.cbo_vehicleno.TabIndex = 5
        '
        'lbl_DeliveryNo
        '
        Me.lbl_DeliveryNo.BackColor = System.Drawing.Color.White
        Me.lbl_DeliveryNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_DeliveryNo.Location = New System.Drawing.Point(113, 15)
        Me.lbl_DeliveryNo.Name = "lbl_DeliveryNo"
        Me.lbl_DeliveryNo.Size = New System.Drawing.Size(167, 23)
        Me.lbl_DeliveryNo.TabIndex = 21
        Me.lbl_DeliveryNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnl_back
        '
        Me.pnl_back.AutoSize = True
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.btn_UserModification)
        Me.pnl_back.Controls.Add(Me.cbo_Bobin_Size)
        Me.pnl_back.Controls.Add(Me.msk_date)
        Me.pnl_back.Controls.Add(Me.txt_EmptyBobin)
        Me.pnl_back.Controls.Add(Me.Label4)
        Me.pnl_back.Controls.Add(Me.lbl_emptybobin_Caption)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.txt_Party_DcNo)
        Me.pnl_back.Controls.Add(Me.lbl_partyDc_caption)
        Me.pnl_back.Controls.Add(Me.txt_Book_No)
        Me.pnl_back.Controls.Add(Me.lbl_bookno_caption)
        Me.pnl_back.Controls.Add(Me.cbo_vehicleno)
        Me.pnl_back.Controls.Add(Me.lbl_DeliveryNo)
        Me.pnl_back.Controls.Add(Me.dtp_Date)
        Me.pnl_back.Controls.Add(Me.lbl_vehicle_caption)
        Me.pnl_back.Controls.Add(Me.lbl_partynamecaptiion)
        Me.pnl_back.Controls.Add(Me.txt_remarks)
        Me.pnl_back.Controls.Add(Me.cbo_PartyName)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.lbl_RemarksCaption)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Location = New System.Drawing.Point(6, 50)
        Me.pnl_back.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(595, 307)
        Me.pnl_back.TabIndex = 9
        '
        'cbo_Bobin_Size
        '
        Me.cbo_Bobin_Size.FormattingEnabled = True
        Me.cbo_Bobin_Size.Location = New System.Drawing.Point(113, 167)
        Me.cbo_Bobin_Size.Name = "cbo_Bobin_Size"
        Me.cbo_Bobin_Size.Size = New System.Drawing.Size(464, 23)
        Me.cbo_Bobin_Size.TabIndex = 6
        '
        'msk_date
        '
        Me.msk_date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_date.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_date.Location = New System.Drawing.Point(399, 15)
        Me.msk_date.Mask = "00-00-0000"
        Me.msk_date.Name = "msk_date"
        Me.msk_date.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_date.Size = New System.Drawing.Size(158, 22)
        Me.msk_date.TabIndex = 0
        '
        'txt_EmptyBobin
        '
        Me.txt_EmptyBobin.Location = New System.Drawing.Point(113, 129)
        Me.txt_EmptyBobin.MaxLength = 8
        Me.txt_EmptyBobin.Name = "txt_EmptyBobin"
        Me.txt_EmptyBobin.Size = New System.Drawing.Size(167, 23)
        Me.txt_EmptyBobin.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(12, 171)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 15)
        Me.Label4.TabIndex = 35
        Me.Label4.Text = "Bobin Size"
        '
        'lbl_emptybobin_Caption
        '
        Me.lbl_emptybobin_Caption.AutoSize = True
        Me.lbl_emptybobin_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_emptybobin_Caption.Location = New System.Drawing.Point(12, 133)
        Me.lbl_emptybobin_Caption.Name = "lbl_emptybobin_Caption"
        Me.lbl_emptybobin_Caption.Size = New System.Drawing.Size(76, 15)
        Me.lbl_emptybobin_Caption.TabIndex = 35
        Me.lbl_emptybobin_Caption.Text = "Empty Bobin" & Global.Microsoft.VisualBasic.ChrW(13)
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(495, 250)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(80, 35)
        Me.btn_close.TabIndex = 9
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(397, 250)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(80, 35)
        Me.btn_save.TabIndex = 8
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'txt_Party_DcNo
        '
        Me.txt_Party_DcNo.Location = New System.Drawing.Point(113, 91)
        Me.txt_Party_DcNo.MaxLength = 15
        Me.txt_Party_DcNo.Name = "txt_Party_DcNo"
        Me.txt_Party_DcNo.Size = New System.Drawing.Size(167, 23)
        Me.txt_Party_DcNo.TabIndex = 2
        '
        'lbl_partyDc_caption
        '
        Me.lbl_partyDc_caption.AutoSize = True
        Me.lbl_partyDc_caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_partyDc_caption.Location = New System.Drawing.Point(12, 95)
        Me.lbl_partyDc_caption.Name = "lbl_partyDc_caption"
        Me.lbl_partyDc_caption.Size = New System.Drawing.Size(68, 15)
        Me.lbl_partyDc_caption.TabIndex = 25
        Me.lbl_partyDc_caption.Text = "Party DcNo"
        '
        'txt_Book_No
        '
        Me.txt_Book_No.Location = New System.Drawing.Point(399, 91)
        Me.txt_Book_No.MaxLength = 20
        Me.txt_Book_No.Name = "txt_Book_No"
        Me.txt_Book_No.Size = New System.Drawing.Size(178, 23)
        Me.txt_Book_No.TabIndex = 3
        '
        'lbl_bookno_caption
        '
        Me.lbl_bookno_caption.AutoSize = True
        Me.lbl_bookno_caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_bookno_caption.Location = New System.Drawing.Point(318, 95)
        Me.lbl_bookno_caption.Name = "lbl_bookno_caption"
        Me.lbl_bookno_caption.Size = New System.Drawing.Size(53, 15)
        Me.lbl_bookno_caption.TabIndex = 27
        Me.lbl_bookno_caption.Text = "Book No"
        '
        'dtp_Date
        '
        Me.dtp_Date.CustomFormat = ""
        Me.dtp_Date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Date.Location = New System.Drawing.Point(556, 15)
        Me.dtp_Date.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(21, 22)
        Me.dtp_Date.TabIndex = 0
        Me.dtp_Date.TabStop = False
        '
        'lbl_vehicle_caption
        '
        Me.lbl_vehicle_caption.AutoSize = True
        Me.lbl_vehicle_caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_vehicle_caption.Location = New System.Drawing.Point(318, 133)
        Me.lbl_vehicle_caption.Name = "lbl_vehicle_caption"
        Me.lbl_vehicle_caption.Size = New System.Drawing.Size(65, 15)
        Me.lbl_vehicle_caption.TabIndex = 15
        Me.lbl_vehicle_caption.Text = "Vehicle No"
        '
        'lbl_partynamecaptiion
        '
        Me.lbl_partynamecaptiion.AutoSize = True
        Me.lbl_partynamecaptiion.ForeColor = System.Drawing.Color.Blue
        Me.lbl_partynamecaptiion.Location = New System.Drawing.Point(12, 57)
        Me.lbl_partynamecaptiion.Name = "lbl_partynamecaptiion"
        Me.lbl_partynamecaptiion.Size = New System.Drawing.Size(72, 15)
        Me.lbl_partynamecaptiion.TabIndex = 14
        Me.lbl_partynamecaptiion.Text = "Party Name"
        '
        'txt_remarks
        '
        Me.txt_remarks.Location = New System.Drawing.Point(113, 205)
        Me.txt_remarks.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txt_remarks.MaxLength = 100
        Me.txt_remarks.Name = "txt_remarks"
        Me.txt_remarks.Size = New System.Drawing.Size(464, 23)
        Me.txt_remarks.TabIndex = 7
        '
        'cbo_PartyName
        '
        Me.cbo_PartyName.DropDownHeight = 175
        Me.cbo_PartyName.FormattingEnabled = True
        Me.cbo_PartyName.IntegralHeight = False
        Me.cbo_PartyName.Location = New System.Drawing.Point(113, 53)
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
        Me.Label1.Location = New System.Drawing.Point(12, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Delivery No."
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
        Me.Label2.Location = New System.Drawing.Point(318, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Date"
        '
        'lbl_RemarksCaption
        '
        Me.lbl_RemarksCaption.AutoSize = True
        Me.lbl_RemarksCaption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_RemarksCaption.Location = New System.Drawing.Point(12, 209)
        Me.lbl_RemarksCaption.Name = "lbl_RemarksCaption"
        Me.lbl_RemarksCaption.Size = New System.Drawing.Size(54, 15)
        Me.lbl_RemarksCaption.TabIndex = 5
        Me.lbl_RemarksCaption.Text = "Remarks"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 15)
        Me.Label3.TabIndex = 2
        '
        'btn_Print
        '
        Me.btn_Print.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Print.ForeColor = System.Drawing.Color.White
        Me.btn_Print.Location = New System.Drawing.Point(650, 296)
        Me.btn_Print.Name = "btn_Print"
        Me.btn_Print.Size = New System.Drawing.Size(73, 30)
        Me.btn_Print.TabIndex = 9
        Me.btn_Print.TabStop = False
        Me.btn_Print.Text = "&PRINT"
        Me.btn_Print.UseVisualStyleBackColor = False
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
        Me.pnl_filter.Location = New System.Drawing.Point(24, 453)
        Me.pnl_filter.Name = "pnl_filter"
        Me.pnl_filter.Size = New System.Drawing.Size(562, 284)
        Me.pnl_filter.TabIndex = 24
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
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dc.DefaultCellStyle = DataGridViewCellStyle1
        Me.dc.HeaderText = "Rec.No"
        Me.dc.MaxInputLength = 8
        Me.dc.Name = "dc"
        Me.dc.ReadOnly = True
        Me.dc.Width = 60
        '
        'Column1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "Rec.Date"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 90
        '
        'Column2
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.HeaderText = "PARTY NAME"
        Me.Column2.MaxInputLength = 35
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 250
        '
        'Column5
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column5.HeaderText = "EMPTY BEAM"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 110
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
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(12, 51)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(55, 15)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Rec.Date"
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
        Me.Label11.Size = New System.Drawing.Size(616, 40)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "EMPTY BOBIN DELIVERY"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PrintDocument1
        '
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Red
        Me.lbl_Company.ForeColor = System.Drawing.Color.White
        Me.lbl_Company.Location = New System.Drawing.Point(50, 15)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 25
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'lbl_UserName
        '
        Me.lbl_UserName.AutoSize = True
        Me.lbl_UserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(55, Byte), Integer), CType(CType(91, Byte), Integer))
        Me.lbl_UserName.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_UserName.ForeColor = System.Drawing.Color.White
        Me.lbl_UserName.Location = New System.Drawing.Point(498, 11)
        Me.lbl_UserName.Name = "lbl_UserName"
        Me.lbl_UserName.Size = New System.Drawing.Size(105, 19)
        Me.lbl_UserName.TabIndex = 269
        Me.lbl_UserName.Text = "USER : ADMIN"
        '
        'btn_UserModification
        '
        Me.btn_UserModification.BackColor = System.Drawing.Color.OrangeRed
        Me.btn_UserModification.ForeColor = System.Drawing.Color.White
        Me.btn_UserModification.Location = New System.Drawing.Point(17, 255)
        Me.btn_UserModification.Name = "btn_UserModification"
        Me.btn_UserModification.Size = New System.Drawing.Size(103, 25)
        Me.btn_UserModification.TabIndex = 1178
        Me.btn_UserModification.TabStop = False
        Me.btn_UserModification.Text = "MODIFICATION"
        Me.btn_UserModification.UseVisualStyleBackColor = False
        Me.btn_UserModification.Visible = False
        '
        'Empty_Bobin_Delivery_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(616, 371)
        Me.Controls.Add(Me.lbl_UserName)
        Me.Controls.Add(Me.lbl_Company)
        Me.Controls.Add(Me.pnl_filter)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.btn_Print)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Empty_Bobin_Delivery_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EMPTY BOBIN RECEIPT"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.pnl_filter.ResumeLayout(False)
        Me.pnl_filter.PerformLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbo_vehicleno As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_DeliveryNo As System.Windows.Forms.Label
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents dtp_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_vehicle_caption As System.Windows.Forms.Label
    Friend WithEvents lbl_partynamecaptiion As System.Windows.Forms.Label
    Friend WithEvents txt_remarks As System.Windows.Forms.TextBox
    Friend WithEvents cbo_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbl_RemarksCaption As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents pnl_filter As System.Windows.Forms.Panel
    Friend WithEvents btn_closefilter As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btn_filtershow As System.Windows.Forms.Button
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents cbo_Filter_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents dtp_FilterTo_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp_FilterFrom_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents dc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents lbl_partyDc_caption As System.Windows.Forms.Label
    Friend WithEvents txt_Book_No As System.Windows.Forms.TextBox
    Friend WithEvents lbl_bookno_caption As System.Windows.Forms.Label
    Friend WithEvents txt_Party_DcNo As System.Windows.Forms.TextBox
    Friend WithEvents btn_Print As System.Windows.Forms.Button
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents txt_EmptyBobin As System.Windows.Forms.TextBox
    Friend WithEvents lbl_emptybobin_Caption As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents msk_date As System.Windows.Forms.MaskedTextBox
    Friend WithEvents lbl_UserName As System.Windows.Forms.Label
    Friend WithEvents cbo_Bobin_Size As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btn_UserModification As System.Windows.Forms.Button
End Class
