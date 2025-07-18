<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Crimp_Entry
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
        Me.cbo_vehicleno = New System.Windows.Forms.ComboBox()
        Me.lbl_ReceiptNo = New System.Windows.Forms.Label()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.lbl_crimp_mtrs = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.txt_crimp_Percentage = New System.Windows.Forms.TextBox()
        Me.msk_todate = New System.Windows.Forms.MaskedTextBox()
        Me.dtp_todate = New System.Windows.Forms.DateTimePicker()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.msk_frmdate = New System.Windows.Forms.MaskedTextBox()
        Me.dtp_Frmdate = New System.Windows.Forms.DateTimePicker()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Cbo_Endscount = New System.Windows.Forms.ComboBox()
        Me.cbo_Clothname = New System.Windows.Forms.ComboBox()
        Me.btn_UserModification = New System.Windows.Forms.Button()
        Me.msk_date = New System.Windows.Forms.MaskedTextBox()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.txt_Receipt_Mtrs = New System.Windows.Forms.TextBox()
        Me.lbl_receipt_mtrs = New System.Windows.Forms.Label()
        Me.lbl_bookno_caption = New System.Windows.Forms.Label()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.lbl_partynamecaptiion = New System.Windows.Forms.Label()
        Me.txt_remarks = New System.Windows.Forms.TextBox()
        Me.cbo_WeaverName = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lbl_RemarksCaption = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cbo_BobinSize = New System.Windows.Forms.ComboBox()
        Me.txt_EmptyBobin = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbl_emptybobin_Caption = New System.Windows.Forms.Label()
        Me.lbl_vehicle_caption = New System.Windows.Forms.Label()
        Me.btn_Print = New System.Windows.Forms.Button()
        Me.pnl_filter = New System.Windows.Forms.Panel()
        Me.btn_closefilter = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btn_filtershow = New System.Windows.Forms.Button()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
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
        Me.dc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
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
        Me.cbo_vehicleno.Location = New System.Drawing.Point(1008, 243)
        Me.cbo_vehicleno.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbo_vehicleno.Name = "cbo_vehicleno"
        Me.cbo_vehicleno.Size = New System.Drawing.Size(178, 23)
        Me.cbo_vehicleno.TabIndex = 5
        Me.cbo_vehicleno.Visible = False
        '
        'lbl_ReceiptNo
        '
        Me.lbl_ReceiptNo.BackColor = System.Drawing.Color.White
        Me.lbl_ReceiptNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_ReceiptNo.Location = New System.Drawing.Point(113, 10)
        Me.lbl_ReceiptNo.Name = "lbl_ReceiptNo"
        Me.lbl_ReceiptNo.Size = New System.Drawing.Size(179, 23)
        Me.lbl_ReceiptNo.TabIndex = 21
        Me.lbl_ReceiptNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnl_back
        '
        Me.pnl_back.AutoSize = True
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.lbl_crimp_mtrs)
        Me.pnl_back.Controls.Add(Me.Label45)
        Me.pnl_back.Controls.Add(Me.txt_crimp_Percentage)
        Me.pnl_back.Controls.Add(Me.msk_todate)
        Me.pnl_back.Controls.Add(Me.dtp_todate)
        Me.pnl_back.Controls.Add(Me.Label8)
        Me.pnl_back.Controls.Add(Me.msk_frmdate)
        Me.pnl_back.Controls.Add(Me.dtp_Frmdate)
        Me.pnl_back.Controls.Add(Me.Label6)
        Me.pnl_back.Controls.Add(Me.Label9)
        Me.pnl_back.Controls.Add(Me.Label5)
        Me.pnl_back.Controls.Add(Me.Cbo_Endscount)
        Me.pnl_back.Controls.Add(Me.cbo_Clothname)
        Me.pnl_back.Controls.Add(Me.btn_UserModification)
        Me.pnl_back.Controls.Add(Me.msk_date)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.txt_Receipt_Mtrs)
        Me.pnl_back.Controls.Add(Me.lbl_receipt_mtrs)
        Me.pnl_back.Controls.Add(Me.lbl_bookno_caption)
        Me.pnl_back.Controls.Add(Me.lbl_ReceiptNo)
        Me.pnl_back.Controls.Add(Me.dtp_Date)
        Me.pnl_back.Controls.Add(Me.lbl_partynamecaptiion)
        Me.pnl_back.Controls.Add(Me.txt_remarks)
        Me.pnl_back.Controls.Add(Me.cbo_WeaverName)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.lbl_RemarksCaption)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Location = New System.Drawing.Point(6, 49)
        Me.pnl_back.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(597, 304)
        Me.pnl_back.TabIndex = 9
        '
        'lbl_crimp_mtrs
        '
        Me.lbl_crimp_mtrs.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbl_crimp_mtrs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_crimp_mtrs.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_crimp_mtrs.Location = New System.Drawing.Point(464, 185)
        Me.lbl_crimp_mtrs.Name = "lbl_crimp_mtrs"
        Me.lbl_crimp_mtrs.Size = New System.Drawing.Size(114, 24)
        Me.lbl_crimp_mtrs.TabIndex = 1204
        Me.lbl_crimp_mtrs.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.ForeColor = System.Drawing.Color.Blue
        Me.Label45.Location = New System.Drawing.Point(442, 190)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(16, 15)
        Me.Label45.TabIndex = 1203
        Me.Label45.Text = "%"
        '
        'txt_crimp_Percentage
        '
        Me.txt_crimp_Percentage.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_crimp_Percentage.Location = New System.Drawing.Point(399, 186)
        Me.txt_crimp_Percentage.MaxLength = 10
        Me.txt_crimp_Percentage.Name = "txt_crimp_Percentage"
        Me.txt_crimp_Percentage.Size = New System.Drawing.Size(41, 23)
        Me.txt_crimp_Percentage.TabIndex = 7
        Me.txt_crimp_Percentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'msk_todate
        '
        Me.msk_todate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold)
        Me.msk_todate.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_todate.Location = New System.Drawing.Point(398, 154)
        Me.msk_todate.Mask = "00-00-0000"
        Me.msk_todate.Name = "msk_todate"
        Me.msk_todate.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_todate.Size = New System.Drawing.Size(161, 22)
        Me.msk_todate.TabIndex = 5
        '
        'dtp_todate
        '
        Me.dtp_todate.CustomFormat = ""
        Me.dtp_todate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold)
        Me.dtp_todate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_todate.Location = New System.Drawing.Point(558, 154)
        Me.dtp_todate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtp_todate.Name = "dtp_todate"
        Me.dtp_todate.Size = New System.Drawing.Size(18, 22)
        Me.dtp_todate.TabIndex = 1184
        Me.dtp_todate.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(305, 158)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(48, 15)
        Me.Label8.TabIndex = 1186
        Me.Label8.Text = "To Date"
        '
        'msk_frmdate
        '
        Me.msk_frmdate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold)
        Me.msk_frmdate.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_frmdate.Location = New System.Drawing.Point(113, 153)
        Me.msk_frmdate.Mask = "00-00-0000"
        Me.msk_frmdate.Name = "msk_frmdate"
        Me.msk_frmdate.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_frmdate.Size = New System.Drawing.Size(161, 22)
        Me.msk_frmdate.TabIndex = 4
        '
        'dtp_Frmdate
        '
        Me.dtp_Frmdate.CustomFormat = ""
        Me.dtp_Frmdate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold)
        Me.dtp_Frmdate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Frmdate.Location = New System.Drawing.Point(273, 153)
        Me.dtp_Frmdate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtp_Frmdate.Name = "dtp_Frmdate"
        Me.dtp_Frmdate.Size = New System.Drawing.Size(18, 22)
        Me.dtp_Frmdate.TabIndex = 1181
        Me.dtp_Frmdate.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(12, 157)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 15)
        Me.Label6.TabIndex = 1183
        Me.Label6.Text = "From date"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Blue
        Me.Label9.Location = New System.Drawing.Point(12, 125)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(74, 15)
        Me.Label9.TabIndex = 1180
        Me.Label9.Text = "EndsCount *"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(12, 91)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(81, 15)
        Me.Label5.TabIndex = 1180
        Me.Label5.Text = "Cloth Name *"
        '
        'Cbo_Endscount
        '
        Me.Cbo_Endscount.DropDownHeight = 175
        Me.Cbo_Endscount.FormattingEnabled = True
        Me.Cbo_Endscount.IntegralHeight = False
        Me.Cbo_Endscount.Location = New System.Drawing.Point(113, 121)
        Me.Cbo_Endscount.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Cbo_Endscount.MaxLength = 35
        Me.Cbo_Endscount.Name = "Cbo_Endscount"
        Me.Cbo_Endscount.Size = New System.Drawing.Size(464, 23)
        Me.Cbo_Endscount.TabIndex = 3
        '
        'cbo_Clothname
        '
        Me.cbo_Clothname.DropDownHeight = 175
        Me.cbo_Clothname.FormattingEnabled = True
        Me.cbo_Clothname.IntegralHeight = False
        Me.cbo_Clothname.Location = New System.Drawing.Point(113, 87)
        Me.cbo_Clothname.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbo_Clothname.MaxLength = 35
        Me.cbo_Clothname.Name = "cbo_Clothname"
        Me.cbo_Clothname.Size = New System.Drawing.Size(464, 23)
        Me.cbo_Clothname.TabIndex = 2
        '
        'btn_UserModification
        '
        Me.btn_UserModification.BackColor = System.Drawing.Color.OrangeRed
        Me.btn_UserModification.ForeColor = System.Drawing.Color.White
        Me.btn_UserModification.Location = New System.Drawing.Point(12, 268)
        Me.btn_UserModification.Name = "btn_UserModification"
        Me.btn_UserModification.Size = New System.Drawing.Size(103, 25)
        Me.btn_UserModification.TabIndex = 1178
        Me.btn_UserModification.TabStop = False
        Me.btn_UserModification.Text = "MODIFICATION"
        Me.btn_UserModification.UseVisualStyleBackColor = False
        Me.btn_UserModification.Visible = False
        '
        'msk_date
        '
        Me.msk_date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold)
        Me.msk_date.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_date.Location = New System.Drawing.Point(399, 10)
        Me.msk_date.Mask = "00-00-0000"
        Me.msk_date.Name = "msk_date"
        Me.msk_date.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_date.Size = New System.Drawing.Size(161, 22)
        Me.msk_date.TabIndex = 0
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(498, 264)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(80, 32)
        Me.btn_close.TabIndex = 10
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(400, 264)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(80, 32)
        Me.btn_save.TabIndex = 9
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'txt_Receipt_Mtrs
        '
        Me.txt_Receipt_Mtrs.Location = New System.Drawing.Point(113, 189)
        Me.txt_Receipt_Mtrs.MaxLength = 15
        Me.txt_Receipt_Mtrs.Name = "txt_Receipt_Mtrs"
        Me.txt_Receipt_Mtrs.Size = New System.Drawing.Size(179, 23)
        Me.txt_Receipt_Mtrs.TabIndex = 6
        '
        'lbl_receipt_mtrs
        '
        Me.lbl_receipt_mtrs.AutoSize = True
        Me.lbl_receipt_mtrs.ForeColor = System.Drawing.Color.Blue
        Me.lbl_receipt_mtrs.Location = New System.Drawing.Point(11, 193)
        Me.lbl_receipt_mtrs.Name = "lbl_receipt_mtrs"
        Me.lbl_receipt_mtrs.Size = New System.Drawing.Size(90, 15)
        Me.lbl_receipt_mtrs.TabIndex = 25
        Me.lbl_receipt_mtrs.Text = "Tot.Recipt Mtrs"
        '
        'lbl_bookno_caption
        '
        Me.lbl_bookno_caption.AutoSize = True
        Me.lbl_bookno_caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_bookno_caption.Location = New System.Drawing.Point(306, 190)
        Me.lbl_bookno_caption.Name = "lbl_bookno_caption"
        Me.lbl_bookno_caption.Size = New System.Drawing.Size(40, 15)
        Me.lbl_bookno_caption.TabIndex = 27
        Me.lbl_bookno_caption.Text = "Crimp"
        '
        'dtp_Date
        '
        Me.dtp_Date.CustomFormat = ""
        Me.dtp_Date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold)
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Date.Location = New System.Drawing.Point(559, 10)
        Me.dtp_Date.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(18, 22)
        Me.dtp_Date.TabIndex = 0
        Me.dtp_Date.TabStop = False
        '
        'lbl_partynamecaptiion
        '
        Me.lbl_partynamecaptiion.AutoSize = True
        Me.lbl_partynamecaptiion.ForeColor = System.Drawing.Color.Blue
        Me.lbl_partynamecaptiion.Location = New System.Drawing.Point(12, 54)
        Me.lbl_partynamecaptiion.Name = "lbl_partynamecaptiion"
        Me.lbl_partynamecaptiion.Size = New System.Drawing.Size(93, 15)
        Me.lbl_partynamecaptiion.TabIndex = 14
        Me.lbl_partynamecaptiion.Text = "Weaver name *"
        '
        'txt_remarks
        '
        Me.txt_remarks.Location = New System.Drawing.Point(113, 225)
        Me.txt_remarks.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txt_remarks.MaxLength = 100
        Me.txt_remarks.Name = "txt_remarks"
        Me.txt_remarks.Size = New System.Drawing.Size(466, 23)
        Me.txt_remarks.TabIndex = 8
        '
        'cbo_WeaverName
        '
        Me.cbo_WeaverName.DropDownHeight = 175
        Me.cbo_WeaverName.FormattingEnabled = True
        Me.cbo_WeaverName.IntegralHeight = False
        Me.cbo_WeaverName.Location = New System.Drawing.Point(113, 50)
        Me.cbo_WeaverName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbo_WeaverName.MaxLength = 35
        Me.cbo_WeaverName.Name = "cbo_WeaverName"
        Me.cbo_WeaverName.Size = New System.Drawing.Size(464, 23)
        Me.cbo_WeaverName.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(12, 14)
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
        Me.Label2.Location = New System.Drawing.Point(306, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Date"
        '
        'lbl_RemarksCaption
        '
        Me.lbl_RemarksCaption.AutoSize = True
        Me.lbl_RemarksCaption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_RemarksCaption.Location = New System.Drawing.Point(12, 229)
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
        'cbo_BobinSize
        '
        Me.cbo_BobinSize.FormattingEnabled = True
        Me.cbo_BobinSize.Location = New System.Drawing.Point(722, 283)
        Me.cbo_BobinSize.Name = "cbo_BobinSize"
        Me.cbo_BobinSize.Size = New System.Drawing.Size(464, 23)
        Me.cbo_BobinSize.TabIndex = 6
        Me.cbo_BobinSize.Visible = False
        '
        'txt_EmptyBobin
        '
        Me.txt_EmptyBobin.Location = New System.Drawing.Point(722, 243)
        Me.txt_EmptyBobin.MaxLength = 8
        Me.txt_EmptyBobin.Name = "txt_EmptyBobin"
        Me.txt_EmptyBobin.Size = New System.Drawing.Size(167, 23)
        Me.txt_EmptyBobin.TabIndex = 4
        Me.txt_EmptyBobin.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(619, 287)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 15)
        Me.Label4.TabIndex = 35
        Me.Label4.Text = "Bobin Size"
        Me.Label4.Visible = False
        '
        'lbl_emptybobin_Caption
        '
        Me.lbl_emptybobin_Caption.AutoSize = True
        Me.lbl_emptybobin_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_emptybobin_Caption.Location = New System.Drawing.Point(621, 247)
        Me.lbl_emptybobin_Caption.Name = "lbl_emptybobin_Caption"
        Me.lbl_emptybobin_Caption.Size = New System.Drawing.Size(76, 15)
        Me.lbl_emptybobin_Caption.TabIndex = 35
        Me.lbl_emptybobin_Caption.Text = "Empty Bobin" & Global.Microsoft.VisualBasic.ChrW(13)
        Me.lbl_emptybobin_Caption.Visible = False
        '
        'lbl_vehicle_caption
        '
        Me.lbl_vehicle_caption.AutoSize = True
        Me.lbl_vehicle_caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_vehicle_caption.Location = New System.Drawing.Point(915, 247)
        Me.lbl_vehicle_caption.Name = "lbl_vehicle_caption"
        Me.lbl_vehicle_caption.Size = New System.Drawing.Size(65, 15)
        Me.lbl_vehicle_caption.TabIndex = 15
        Me.lbl_vehicle_caption.Text = "Vehicle No"
        Me.lbl_vehicle_caption.Visible = False
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
        Me.pnl_filter.Location = New System.Drawing.Point(96, 420)
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
        Me.Label15.Location = New System.Drawing.Point(5, 86)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(86, 15)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "Weaver Name"
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
        Me.Label13.Size = New System.Drawing.Size(33, 15)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Date"
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
        Me.Label11.Size = New System.Drawing.Size(623, 40)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "CRIMP ENTRY"
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
        'dc
        '
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dc.DefaultCellStyle = DataGridViewCellStyle9
        Me.dc.HeaderText = "NO"
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
        Me.Column1.HeaderText = "DATE"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 90
        '
        'Column2
        '
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle11
        Me.Column2.HeaderText = "WEAVER NAME"
        Me.Column2.MaxInputLength = 35
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 250
        '
        'Column5
        '
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle12
        Me.Column5.HeaderText = "CRIMP METERS"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 110
        '
        'Crimp_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(623, 371)
        Me.Controls.Add(Me.lbl_UserName)
        Me.Controls.Add(Me.lbl_Company)
        Me.Controls.Add(Me.pnl_filter)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.btn_Print)
        Me.Controls.Add(Me.lbl_emptybobin_Caption)
        Me.Controls.Add(Me.lbl_vehicle_caption)
        Me.Controls.Add(Me.cbo_vehicleno)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txt_EmptyBobin)
        Me.Controls.Add(Me.cbo_BobinSize)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Crimp_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CRIMP ENTRY"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.pnl_filter.ResumeLayout(False)
        Me.pnl_filter.PerformLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbo_vehicleno As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_ReceiptNo As System.Windows.Forms.Label
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents dtp_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_vehicle_caption As System.Windows.Forms.Label
    Friend WithEvents lbl_partynamecaptiion As System.Windows.Forms.Label
    Friend WithEvents txt_remarks As System.Windows.Forms.TextBox
    Friend WithEvents cbo_WeaverName As System.Windows.Forms.ComboBox
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
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents lbl_receipt_mtrs As System.Windows.Forms.Label
    Friend WithEvents lbl_bookno_caption As System.Windows.Forms.Label
    Friend WithEvents txt_Receipt_Mtrs As System.Windows.Forms.TextBox
    Friend WithEvents btn_Print As System.Windows.Forms.Button
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents txt_EmptyBobin As System.Windows.Forms.TextBox
    Friend WithEvents lbl_emptybobin_Caption As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents msk_date As System.Windows.Forms.MaskedTextBox
    Friend WithEvents lbl_UserName As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbo_BobinSize As System.Windows.Forms.ComboBox
    Friend WithEvents btn_UserModification As System.Windows.Forms.Button
    Friend WithEvents msk_todate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents dtp_todate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents msk_frmdate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents dtp_Frmdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cbo_Clothname As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_crimp_mtrs As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents txt_crimp_Percentage As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Cbo_Endscount As System.Windows.Forms.ComboBox
    Friend WithEvents dc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
